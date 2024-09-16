using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _LookSensX;
    [SerializeField] private float _LookSensY;
    private Camera _camera;
    private PlayerInput _playerInput;
    private CharacterController _charCont;
    [SerializeField] private Animator _armsAnim;
    private Vector2 _moveInput;
    private float _punchingRight;
    private float _punchingLeft;
    private float verticalRot = 0;
    private ArmsAnim armAnimator;

    void Start()
    {        
        _playerInput = new PlayerInput();
        _charCont = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _armsAnim = GetComponentInChildren<Animator>();
        armAnimator = GetComponent<ArmsAnim>();

        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.Player.Enable();
    }

    void FixedUpdate()
    {
        Movement();
        Punching();
    }

    void Movement()
    {
        Vector3 moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x; 

        _charCont.Move(moveDirection * _speed * Time.deltaTime);

        // Camera controls
        float mouseX = Input.GetAxis("Mouse X") * _LookSensX;
        float mouseY = Input.GetAxis("Mouse Y") * _LookSensY;
        
        verticalRot -= mouseY;
        verticalRot = Mathf.Clamp(verticalRot, -90, 90);

        _camera.transform.localRotation = Quaternion.Euler(verticalRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    void Punching()
    {
        if(_punchingLeft  > 0)
        {
            _armsAnim.SetTrigger("RightPunch");
            _punchingLeft = 0;
        }
        
        if(_punchingRight > 0)
        {
            _armsAnim.SetTrigger("LeftPunch");
            _punchingRight = 0;
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
        _punchingRight = value.Get<float>();
    }

    public void OnLeftPunch(InputValue value)
    {
        _punchingLeft = value.Get<float>();
    }
}
