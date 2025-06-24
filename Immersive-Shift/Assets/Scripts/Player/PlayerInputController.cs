using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public event Action OnAttack;
    public event Action OnJump;

    private InputSystem_Actions inputActions;
    private InputSystem_Actions.PlayerActionActions playerActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerActions = inputActions.PlayerAction;

        playerActions.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        playerActions.Move.canceled += ctx => MoveInput = Vector2.zero;

        playerActions.Attack.performed += ctx => OnAttack?.Invoke();

        playerActions.Jump.performed += ctx => OnJump?.Invoke();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }
}