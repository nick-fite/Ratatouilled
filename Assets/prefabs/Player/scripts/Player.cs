using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Basic movement system player, combat and outside elements more interesting.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _LookSensX;
    [SerializeField] private float _LookSensY;
    private Camera _camera;
    private PlayerInput _playerInput;
    private CharacterController _charCont;
    [SerializeField] private Animator _armsAnim;
    private ArmsProceduralAnimation armAnimator;
    private Vector2 _moveInput;
    private bool _punchingRight;
    private bool _punchingLeft;
    private float verticalRot = 0;
    [SerializeField] private float _gravity;
    [SerializeField] private float _charVel;
    [SerializeField] private float _jumpPower;
    private Vector3 _charDir;


    void Start()
    {        
        _playerInput = new PlayerInput();
        _charCont = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _armsAnim = GetComponentInChildren<Animator>();
        armAnimator = GetComponent<ArmsProceduralAnimation>();

        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.Player.Enable();
    }

    void FixedUpdate()
    {
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
            armAnimator.StartWalkCycle();
        }
        else
        {
            armAnimator.StopWalkCycle();
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
}
