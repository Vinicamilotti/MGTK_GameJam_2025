using System;
using System.IO;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum PlayerSate : byte
{
    Moving,
    Looping,
}
public class PlayerMovement : MonoBehaviour
{

    PlayerSate playerState = PlayerSate.Moving;
    public float speed = 5f;
    public float acceleration;
    public float drag;

    InputAction move;
    InputAction control;
    InputAction loop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float xInput;
    float yInput;



    public float RotationControll;



    Rigidbody2D rb;
    void InitiateInputAction()
    {
        move = InputSystem.actions.FindAction("Move");
        control = InputSystem.actions.FindAction("Action");
        loop = InputSystem.actions.FindAction("Loop");
    }
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        InitiateInputAction();
    }


    void UpdateInput()
    {
        var vecMove = move.ReadValue<Vector2>();
        xInput = vecMove.x;
        yInput = vecMove.y;

    }

    private void UpdateState()
    {
        if (control.IsPressed())
        {
            playerState = PlayerSate.Looping;
            return;
        }

        playerState = PlayerSate.Moving;
    }
    void Moving()
    {
        rb.rotation = 0f; 
        rb.AddForce(new(xInput * acceleration, yInput * acceleration));
    }

    void Looping()
    {

        Vector2 vel = transform.right * (xInput * acceleration);
        rb.AddForce(vel, ForceMode2D.Force);
        float dir = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(Vector2.right));


        var modifier = dir > 0 ? 1f : -1f;
        var calculateRotation = (yInput * RotationControll * (rb.linearVelocity.magnitude / speed)) * modifier;


        if (acceleration > 0)
        {
            rb.rotation += calculateRotation;
        }


        float thrustForce = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(Vector2.down)) * 2f;

        Vector2 relForce = Vector2.up * thrustForce;

        rb.AddForce(rb.GetRelativeVector(relForce));

        if (rb.linearVelocity.magnitude > speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        } 
            ;
    }

    void MovePlayer()
    {
        if (playerState == PlayerSate.Looping)
        {
            Looping();
            return;
        }

        Moving();
    }

    private void FixedUpdate()
    {
        rb.linearDamping = drag;
        UpdateState();
        UpdateInput();
        MovePlayer();
    }

}
