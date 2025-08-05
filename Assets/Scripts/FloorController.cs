using System;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public float PosX => _transform.position.x;
    public float ScaleX => _transform.localScale.x * _collider.bounds.size.x;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider;

    private void Awake()
    {
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        _rigidbody2D.position += velocity;
    }
}