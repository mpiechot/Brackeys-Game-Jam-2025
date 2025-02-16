using Assets.Player.Actions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float dashStrengh = 4f;

    [SerializeField]
    private float dashCooldown = 4f;

    private Rigidbody2D rigidBody;
    private Vector2 moveInput;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private bool canMove = true;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (!canMove)
        {
            rigidBody.linearVelocity = Vector2.zero;
            return;
        }

        rigidBody.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>
    ///     Moves the player according to the given move-context.
    ///     Must be linked in the Unity Inspektor to the Move-Input
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(ExecuteDash());
        }
    }

    private IEnumerator ExecuteDash()
    {
        canDash = false;
        isDashing = true;
        rigidBody.linearVelocity = moveInput * dashStrengh;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }
}
