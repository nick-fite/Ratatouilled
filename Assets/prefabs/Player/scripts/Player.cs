using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _armsAnim;
    private Camera _camera;
    private PlayerInput _playerInput;
    private CharacterController _charCont;
    [SerializeField] private float _speed;
    [SerializeField] private float _LookSensX;
    [SerializeField] private float _LookSensY;
    [SerializeField] private float _gravity;
    [SerializeField] private float _charVel;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _PunchPower;
    [SerializeField] private float _PushForce;
    [SerializeField] private Transform _PointOfImpact;
    private Vector2 _moveInput;
    private bool _punchingRight;
    private bool _punchingLeft;
    private float verticalRot = 0;
    private Vector3 _charDir;
    private ArmsProceduralAnimation _ArmAnimator;
    private Arms _Arms;
    [SerializeField] private PowerUp _CurrentPowerUp;
    private PowerUp _DefaultPower;

    void Start()
    {        
        _playerInput = new PlayerInput();
        _charCont = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _armsAnim = GetComponentInChildren<Animator>();
        _ArmAnimator = GetComponent<ArmsProceduralAnimation>();
        _Arms = GetComponentInChildren<Arms>();
        _DefaultPower = GetComponent<PowerUp>();
        _CurrentPowerUp = _DefaultPower;

        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.Player.Enable();

        _Arms.OnHandCollision += HandleHandCollision;
    }

    void FixedUpdate()
    {
        _CurrentPowerUp.OnUpdate();
        if(!_CurrentPowerUp.NormalMovement)
            return;
        Movement();
    }

    void LateUpdate()
    {
        if(!_CurrentPowerUp.NormalMovement)
            return;
        Look();
        Punching();
    }

    void Movement()
    {
        _charDir = transform.forward * _moveInput.y + transform.right * _moveInput.x;
        
        if(_charCont.isGrounded && _charVel < 0.0f){
            _charVel = -1.0f;
        }
        else 
        {
            _charVel += -_gravity * Time.deltaTime;
        }

        _charDir.y = _charVel;
        _charCont.Move(_charDir * _speed * Time.deltaTime);
    }
    
    void Look() {
        float mouseX = Input.GetAxis("Mouse X") * _LookSensX;
        float mouseY = Input.GetAxis("Mouse Y") * _LookSensY;
        
        verticalRot -= mouseY;
        verticalRot = Mathf.Clamp(verticalRot, -90, 90);

        transform.Rotate(Vector3.up * mouseX);
        _camera.transform.localRotation = Quaternion.Euler(verticalRot, 0f, 0f);
    }

    void Punching()
    {
        if(_punchingLeft)
        {
            _armsAnim.SetTrigger("RightPunch");
            _punchingLeft = false;
        }
        
        if(_punchingRight)
        {
            _armsAnim.SetTrigger("LeftPunch");
            _punchingRight = false;
        }
    }

    public void OnWalk(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        if(_moveInput != Vector2.zero)
        {
            _ArmAnimator.StartWalkCycle();
        }
        else
        {
            _ArmAnimator.StopWalkCycle();
        }
    }

    public void OnRightPunch(InputValue value)
    {
        if(!_CurrentPowerUp.NormalMovement)
            return;
        _punchingRight = value.Get<float>() > 0;
    }

    public void OnLeftPunch(InputValue value)
    {
        if(!_CurrentPowerUp.NormalMovement)
            return;
        _punchingLeft = value.Get<float>() > 0;
    }

    public void OnJump(InputValue value)
    {
        if(!_CurrentPowerUp.NormalMovement)
            return;
        bool jumping = value.Get<float>() > 0;
        if(jumping && _charCont.isGrounded) _charVel += _jumpPower;
    }

    public void OnThrowPowerUp(InputValue value)
    {
        Debug.Log("Throwing Item");
        _armsAnim.SetTrigger("ThrowItem");

        if(value.Get<float>() > 0 && _CurrentPowerUp != _DefaultPower){
            _CurrentPowerUp.Eject(transform.forward, _PushForce * 10, _PointOfImpact.position);
            AdjustToPowerUp(_DefaultPower);
        }
    }

    public void OnSpecial(InputValue value)
    {
        if(!_CurrentPowerUp.NormalMovement)
            return;
        _CurrentPowerUp.OnSpecialPressed(value.Get<float>() > 0);
    }

    public void OnGrab(InputValue value)
    {
        if(!_CurrentPowerUp.NormalMovement)
            return;
        _armsAnim.SetTrigger("GrabItem");
    }

    void HandleHandCollision(GameObject obj, ArmActions action)
    {
        if(obj.tag == "Enemy" && action == ArmActions.Punching)
            obj.GetComponent<Enemy>().AddHealth(-_PunchPower, _PointOfImpact, _PushForce);

        if(obj.tag == "Pickup" && action == ArmActions.Grabbing)
        {
            Debug.Log(obj.name);
            obj.SetActive(false);
            AdjustToPowerUp(obj.GetComponent<PowerUp>());
            _CurrentPowerUp.OnStart();

        }
        if(obj.tag == "Cube" && action == ArmActions.Punching)
        {
            obj.GetComponent<ReplaceCube>().Replace();
            Debug.Log(obj.name);
        }
    }

    void AdjustToPowerUp(PowerUp powerUp)
    {
        _CurrentPowerUp = powerUp;
        _PunchPower = powerUp.Strength;
        _jumpPower = powerUp.Jump;
        _PushForce = powerUp.PushPower;
    }
}
