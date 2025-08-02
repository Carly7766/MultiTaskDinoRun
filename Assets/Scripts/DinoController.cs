using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DinoController : MonoBehaviour
{
    public enum DinoType
    {
        Doux,
        Mort,
        Tard,
        Vita
    }

    public DinoType dinoType;
    public float jumpForce = 10f;
    private Rigidbody2D _rigidbody;
    private bool _isGrounded;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        InputSystem.actions[$"{dinoType.ToString()}_Jump"].performed += ctx => Jump();
    }

    private void OnDestroy()
    {
        InputSystem.actions[$"{dinoType.ToString()}_Jump"].performed -= ctx => Jump();
    }

    void Jump()
    {
        if (!_isGrounded) return;
        _rigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        _isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}