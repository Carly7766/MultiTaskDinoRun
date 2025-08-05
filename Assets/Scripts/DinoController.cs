using UnityEngine;
using UnityEngine.InputSystem;

public enum DinoType
{
    Doux = 0,
    Mort = 1,
    Tard = 2,
    Vita = 3
}

public class DinoController : MonoBehaviour
{
    public DinoType dinoType;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float randomJumpRange = 1.2f;
    private bool _isGrounded;
    public bool IsGrounded => _isGrounded;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private int _isMoveId = -1;
    private int _isJumpingId = -1;
    private int _isHurtId = -1;

    private InputAction _jumpAction; // 🔑 キャッシュしておく

    public delegate void OnTouchObstacleDelegate();

    public event OnTouchObstacleDelegate OnTouchObstacle = delegate { };

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _isMoveId = Animator.StringToHash("IsMove");
        _isJumpingId = Animator.StringToHash("IsJumping");
        _isHurtId = Animator.StringToHash("IsHurt");

        _jumpAction = InputSystem.actions.FindAction($"{dinoType.ToString()}_Jump");
    }

    private void OnEnable()
    {
        _jumpAction.Enable();
        _jumpAction.performed += OnJumpAction;
    }

    private void OnDisable()
    {
        _jumpAction.Disable();
        _jumpAction.performed -= OnJumpAction;
    }

    private void OnJumpAction(InputAction.CallbackContext ctx)
    {
        Jump(jumpForce);
    }

    void Jump(float force)
    {
        if (!_isGrounded) return;
        _rigidbody.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
        _isGrounded = false;
        _animator.SetBool(_isJumpingId, true);
    }

    public void RandomJump()
    {
        Jump(jumpForce + Random.Range(-randomJumpRange, randomJumpRange));
    }

    public void StartRunAnimation()
    {
        _animator.SetBool(_isMoveId, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _animator.SetBool(_isJumpingId, false);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            _animator.SetBool(_isHurtId, true);
            Invoke(nameof(SendTouchObstacleEvent), 0.35f);
        }
    }

    private void SendTouchObstacleEvent()
    {
        OnTouchObstacle?.Invoke();
    }
}