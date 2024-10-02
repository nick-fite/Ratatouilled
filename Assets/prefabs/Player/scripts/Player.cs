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
    private PowerUp _CurrentPowerUp;

    void Start()
    {        
        _playerInput = new PlayerInput();
        _charCont = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _armsAnim = GetComponentInChildren<Animator>();
        _ArmAnimator = GetComponent<ArmsProceduralAnimation>();
        _Arms = GetComponentInChildren<Arms>();
        _CurrentPowerUp = GetComponent<PowerUp>();

        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.Player.Enable();

        _Arms.OnHandCollision += HandleHandCollision;
    }

    void FixedUpdate()
    {
        _CurrentPowerUp.OnUpdate();
        Movement();
    }

    void LateUpdate()
    {
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
        _punchingRight = value.Get<float>() > 0;
    }

    public void OnLeftPunch(InputValue value)
    {
        _punchingLeft = value.Get<float>() > 0;
    }

    public void OnJump(InputValue value)
    {
        bool jumping = value.Get<float>() > 0;
        if(jumping && _charCont.isGrounded) _charVel += _jumpPower;
    }

    public void OnThrowPowerUp(InputValue value)
    {
        _CurrentPowerUp.Eject(transform.forward);
        _CurrentPowerUp = GetComponent<PowerUp>();
        AdjustToPowerUp(_CurrentPowerUp);
    }

    public void OnSpecial(InputValue value)
    {
        _CurrentPowerUp.OnSpecialPressed(value.Get<float>() > 0);
    }

    public void OnGrab(InputValue value)
    {
        _armsAnim.SetTrigger("GrabItem");
    }

    void HandleHandCollision(GameObject obj, ArmActions action)
    {
        Debug.Log(obj.name);
        if(obj.tag == "Enemy" && action == ArmActions.Punching)
            obj.GetComponent<Enemy>().AddHealth(-_PunchPower, _PointOfImpact.position, _PushForce);

        if(obj.tag == "Pickup" && action == ArmActions.Grabbing)
        {
            Destroy(obj);
            _CurrentPowerUp = obj.GetComponent<PowerUp>();
            AdjustToPowerUp(_CurrentPowerUp);
            _CurrentPowerUp.OnStart();

        }
    }

    void AdjustToPowerUp(PowerUp powerUp)
    {
        _PunchPower = powerUp.Strength;
        _jumpPower = powerUp.Jump;
        _PushForce = powerUp.PushPower;
    }
}
