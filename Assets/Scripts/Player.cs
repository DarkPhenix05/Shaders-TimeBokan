using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input")]
    public PlayerInput _playerInput;
    public PlayerInputActions _playerInputActions;

    [Header("Movement & Rotation")]
    public float _speed;
    public float _rotationSpeed;

    private Rigidbody _rb;
    
    public Transform _cameraTransform;
    
    private Vector3 _rbSpeed;
    public Vector2 _rotation = Vector2.zero;
    public Vector2 _mouseInput = Vector2.zero;
    private Vector2 _movement;
    private Vector3 _movementDir;
    private Vector3 _localRight;
    private Vector3 _localFoward;

    public bool _mouseState;
    public bool _snapping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerInput = this.gameObject.GetComponent<PlayerInput>();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Movement.performed += Movement_performed;

        _rb = this.gameObject.GetComponent<Rigidbody>();

        _cameraTransform = Camera.main.transform;
        
        _mouseState = false;
        LockMouse();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_cameraTransform)
        {
            Debug.LogWarning("CAMERA IS == NULL");
            return;
        }

        Movement();

        Rotate();

        if (Input.GetKeyDown(KeyCode.L))
        {
            if(_mouseState)
            {
                LockMouse();
                Debug.Log("LOCKED");
            }
            else
            {
                FreeMouse();
                Debug.Log("FREE");
            }
        }
    }

    void Movement()
    {
        _movement = _playerInputActions.Player.Movement.ReadValue<Vector2>();

        //WASD();

        _localFoward = _cameraTransform.forward;
        _localRight = _cameraTransform.right;

        _localFoward.y = 0;     
        _localRight.y = 0;

        Vector3 rightRelative = _movement.x * _localRight;
        Vector3 forwardRelative = _movement.y * _localFoward;

        Vector3 moveDir = (forwardRelative + rightRelative).normalized * _speed;

        _rb.linearVelocity = new Vector3(moveDir.x, _rb.linearVelocity.y, moveDir.z);

        _rbSpeed = (_rb.linearVelocity).normalized;

        if (_rbSpeed != Vector3.zero)
        {
            if (_rbSpeed.y != 0)
            {
                //Debug.Log("FALLING");
            }
            else
            {
                //Debug.Log("OnGround");
            }
        }
    }

    public void Movement_performed(InputAction.CallbackContext context)
    {
        //Debug.Log("MOVEMENT: " + context.ReadValue<Vector2>());
        _movement = context.ReadValue<Vector2>();
    }
    private void Rotate()
    {
            if (_snapping)
            {
                if(_rbSpeed.x != 0.0f || _rbSpeed.z != 0.0f)
                {
                    transform.forward = new Vector3(_rbSpeed.x, 0, _rbSpeed.z);
                }
            }
            else
            {
                //Quaternion toRotation = Quaternion.LookRotation(new Vector3(_rbSpeed.x, 0, _rbSpeed.z));
                Quaternion toRotation = Quaternion.LookRotation(new Vector3(_rbSpeed.x, 0, _rbSpeed.z));
                toRotation.Normalize();

                Debug.Log(toRotation.ToString());

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            }
    }

    public void Rotation_performed(InputAction.CallbackContext context)
    {
        //Debug.Log("ROTATION: " + context.ReadValue<Vector2>());
        _mouseInput = context.ReadValue<Vector2>();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _mouseState = false;
    }

    public void FreeMouse()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _mouseState= true;
    }

    public void UpdateTransform(Transform transform)
    {
        this.gameObject.transform.localPosition = transform.localPosition;
    }
}
