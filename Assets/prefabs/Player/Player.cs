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
    private float _punching;
    private float verticalRot = 0;
    [SerializeField] private Transform TopWalkingPos;
    [SerializeField] private Transform DefaultPos;

    void Start()
    {        
        _playerInput = new PlayerInput();
        _charCont = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _armsAnim = GetComponentInChildren<Animator>();
        
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

        if(_moveInput != Vector2.zero)
        {
            Vector3 test =_armsAnim.gameObject.transform.localPosition;
            test = Vector3.Lerp(test, TopWalkingPos.localPosition, 1);
            _armsAnim.gameObject.transform.SetLocalPositionAndRotation(test, _armsAnim.gameObject.transform.localRotation);

        }

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
        if(_punching > 0)
            _armsAnim.SetTrigger("Punching");
            _punching = 0;
    }

    public void OnWalk(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnPunch(InputValue value)
    {
        _punching = value.Get<float>();
    }
}
