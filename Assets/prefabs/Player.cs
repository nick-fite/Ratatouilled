using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _LookSensX;
    [SerializeField] private float _LookSensY;
    private Camera _camera;
    private PlayerInput _playerInput;
    private CharacterController _charCont;
    private Animator _armsAnim;
    private Vector2 _moveInput;
    private float _punching;
    
    private float verticalRot = 0;

    void Start()
    {        
        _playerInput = new PlayerInput();
        _charCont = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _armsAnim = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        _playerInput.Player.Enable();        
    }
    
    void Update()
    {
        _moveInput = _playerInput.Player.walk.ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x; 

        _charCont.Move(moveDirection * _speed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * _LookSensX;
        float mouseY = Input.GetAxis("Mouse Y") * _LookSensY;
        
        verticalRot -= mouseY;
        verticalRot = Mathf.Clamp(verticalRot, -90, 90);

        _camera.transform.localRotation = Quaternion.Euler(verticalRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
        
        _punching = _playerInput.Player.punch.ReadValue<float>();

        if(_punching > 0)
            _armsAnim.SetBool("Punching", true);
        else 
            _armsAnim.SetBool("Punching", false);
    }
}
