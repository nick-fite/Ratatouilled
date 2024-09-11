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
    private float _punching;
    private float verticalRot = 0;
    [SerializeField] private Transform TopWalkingPos;
    [SerializeField] private Transform DefaultPos;

    [SerializeField] private float WalkCycleSpeed;
    private float UpRate;
    private float DownRate;
    private float t;
    private bool isWalking;
    

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

        // Camera controls
        float mouseX = Input.GetAxis("Mouse X") * _LookSensX;
        float mouseY = Input.GetAxis("Mouse Y") * _LookSensY;
        
        verticalRot -= mouseY;
        verticalRot = Mathf.Clamp(verticalRot, -90, 90);

        _camera.transform.localRotation = Quaternion.Euler(verticalRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if(isWalking){
        Vector3 target;
        if(_armsAnim.gameObject.transform.localPosition == TopWalkingPos.localPosition)
        {
            target = DefaultPos.localPosition;
            t += Time.deltaTime * DownRate;
        }
        else 
        {
            target = TopWalkingPos.localPosition;
            t += Time.deltaTime * UpRate;  
        }
        Vector3 newPos = _armsAnim.gameObject.transform.localPosition;
        newPos  = Vector3.Lerp(newPos, target, t);

        _armsAnim.gameObject.transform.SetLocalPositionAndRotation(newPos, _armsAnim.gameObject.transform.localRotation);
        }
    }
    
    void Punching()
    {
        if(_punching > 0)
            _armsAnim.SetTrigger("Punching");
            _punching = 0;
    }



/*
if(_moveInput != Vector2.zero)
        {
            Vector3 test =_armsAnim.gameObject.transform.localPosition;
            test = Vector3.Lerp(test, TopWalkingPos.localPosition, 1);
            _armsAnim.gameObject.transform.SetLocalPositionAndRotation(test, _armsAnim.gameObject.transform.localRotation);

        }
*/

    public void OnWalk(InputValue value)
    {
        UpRate = 1.0f/ Vector3.Distance(TopWalkingPos.localPosition, _armsAnim.transform.localPosition) * WalkCycleSpeed;
        DownRate = 1.0f/ Vector3.Distance(DefaultPos.localPosition, TopWalkingPos.localPosition) * WalkCycleSpeed;
        _moveInput = value.Get<Vector2>();
        isWalking = true;
        if(_moveInput == Vector2.zero)
        {
            isWalking = false;
        }
    }

    public void OnPunch(InputValue value)
    {
        _punching = value.Get<float>();
    }

    public IEnumerator WalkCycle()
    {
        yield return null;
    }
}
