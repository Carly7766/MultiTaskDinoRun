using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Color[] dinoTypeColors;
    public int[] dinoTypeLayer;

    private DinoType _targetDinoType;
    public DinoType TargetDinoType => _targetDinoType;

    public float PosX => _transform.position.x;
    public float ScaleX => _transform.localScale.x * _collider.bounds.size.x;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 velocity)
    {
        _rigidbody2D.position += velocity;
    }

    public void SetTargetDinoType(DinoType targetDinoType)
    {
        _targetDinoType = targetDinoType;
        _spriteRenderer.color = dinoTypeColors[(int)_targetDinoType];
        gameObject.layer = dinoTypeLayer[(int)_targetDinoType];
    }
}