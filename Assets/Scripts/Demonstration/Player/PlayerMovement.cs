using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions _playerInput;

    private Rigidbody2D _rb;
    private float _moveSpeed = 10; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = new InputSystem_Actions();
        _playerInput.Enable();

        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 _input = _playerInput.Player.Move.ReadValue<Vector2>();
        _rb.linearVelocityX += _input.x * _moveSpeed * Time.fixedDeltaTime;
    }
}