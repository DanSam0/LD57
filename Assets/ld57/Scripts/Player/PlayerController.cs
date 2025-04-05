using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [Header("Submarine")]
    [SerializeField] private float _speedLimit;
    [SerializeField] private float _speedAcceleration;
    [SerializeField] private float _verticalForce;

    [Header("Misc")]
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _cameraTargetOffsetRange;

    private float _currentSpeed = 0f;
    private float _verticalAxis = 0f;
    private float _horizontalAxis = 0f;
    private bool _isAccelerating = false;

    public Rigidbody2D RigidBody { get; private set; }

    private InputAction _horizontalAction;
    private InputAction _verticalAction;


    private void Awake()
    {

        Instance = this;

        RigidBody = GetComponent<Rigidbody2D>();

        _horizontalAction = InputSystem.actions.FindAction("Acceleration");
        _verticalAction = InputSystem.actions.FindAction("Vertical");
    }


    private void Update()
    {
        SetCameraTargetPosition();
    }


    private void FixedUpdate()
    {
        Move();

        if (_isAccelerating)
            ChangeSpeed();
    }


    private void Move()
    {
        RigidBody.AddForce(
            new Vector2(_currentSpeed, _verticalAxis * _verticalForce), 
            ForceMode2D.Force);
    }


    private void ChangeSpeed()
    {
        float resultSpeed = _currentSpeed;

        if (_horizontalAxis > 0f)
            resultSpeed = _currentSpeed + _speedAcceleration * Time.deltaTime;
        if (_horizontalAxis < 0f)
            resultSpeed = _currentSpeed - _speedAcceleration * Time.deltaTime;

        if (resultSpeed > _speedLimit)
            _currentSpeed = _speedLimit;
        else if (resultSpeed < -_speedLimit)
            _currentSpeed = -_speedLimit;
        else
            _currentSpeed = resultSpeed;
    }


    private void SetCameraTargetPosition()
    {
        _cameraTarget.localPosition = RigidBody.linearVelocity
            * _cameraTargetOffsetRange;
    }


    private void OnEnable()
    {
        _horizontalAction.started += HorizontalInput;
        _verticalAction.performed += VerticalInput;

        _horizontalAction.canceled += ResetHorizontalInput;
        _verticalAction.canceled += ResetVerticalInput;
    }


    private void OnDisable()
    {
        _horizontalAction.started -= HorizontalInput;
        _verticalAction.performed -= VerticalInput;

        _horizontalAction.canceled -= ResetHorizontalInput;
        _verticalAction.canceled -= ResetVerticalInput;
    }


    private void HorizontalInput(CallbackContext callbackContext)
    {
        float value = callbackContext.ReadValue<float>();

        _horizontalAxis = value;

        if (_horizontalAxis != 0f)
            _isAccelerating = true;
    }


    private void VerticalInput(CallbackContext callbackContext)
    {
        float value = callbackContext.ReadValue<float>();

        _verticalAxis = value;        
    }


    private void ResetVerticalInput(CallbackContext callbackContext)
    {
        _verticalAxis = 0f;
    }


    private void ResetHorizontalInput(CallbackContext callbackContext)
    {
        _isAccelerating = false;
        _horizontalAxis = 0f;
    }


    private void OnDrawGizmosSelected()
    {
        if(_cameraTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _cameraTargetOffsetRange);
        }
    }

}
