using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PacManMovement : MonoBehaviour
{
    public float Speed = 1.0f;
    public float RotSpeed = 100.0f;
    public Transform Visual;

    public Direction CurrentDirection { get; private set; }

    public Transform ForwardTrigger;
    public Transform BackwardTrigger;
    public Transform RightTrigger;
    public Transform LeftTrigger;

    private Vector3 Target;
    private Vector3 TargetRot;

    private bool InputForward, InputBackward, InputRight, InputLeft;

    private void Start()
    {
        Target = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputForward = true;
    }

    private void Update()
    {
        CheckInput();

        float x = transform.position.x - Mathf.Floor(transform.position.x);
        float z = transform.position.z - Mathf.Floor(transform.position.z);

        if (InputForward && !CheckCollision(Direction.Forward) && x >= 0.5f && x < 0.6f)
        {
            ComputeTarget(Direction.Forward);
        }
        else if (InputBackward && !CheckCollision(Direction.Backward) && x >= 0.5f && x < 0.6f)
        {
            ComputeTarget(Direction.Backward);
        }
        else if (InputLeft && !CheckCollision(Direction.Left) && z >= 0.5f && z < 0.6f)
        {
            ComputeTarget(Direction.Left);
        }
        else if (InputRight && !CheckCollision(Direction.Right) && z >= 0.5f && z < 0.6f)
        {
            ComputeTarget(Direction.Right);
        }

        if (x >= 0.5f && x < 0.6f)
        {
            if (InputBackward || InputRight || InputLeft) InputForward = false;
            if (InputForward || InputRight || InputLeft) InputBackward = false;
        }

        if (z >= 0.5f && z < 0.6f)
        {
            if (InputForward || InputBackward || InputRight) InputLeft = false;
            if (InputForward || InputBackward || InputLeft) InputRight = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
        Visual.transform.rotation = Quaternion.RotateTowards(Visual.transform.rotation, Quaternion.Euler(TargetRot), RotSpeed * Time.deltaTime);
    }

    private bool CheckCollision(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Physics.CheckSphere(ForwardTrigger.position, 0.1f);
            case Direction.Backward:
                return Physics.CheckSphere(BackwardTrigger.position, 0.1f);
            case Direction.Right:
                return Physics.CheckSphere(RightTrigger.position, 0.1f);
            case Direction.Left:
                return Physics.CheckSphere(LeftTrigger.position, 0.1f);
        }
        return false;
    }

    private void ComputeTarget(Direction direction)
    {
        Vector3 pos = transform.position;
        switch (direction)
        {
            case Direction.Forward:
                pos.z = Mathf.Round(pos.z - 0.5f) + 1.5f;
                pos.x = Mathf.Round(pos.x - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, 0, 0);
                CurrentDirection = Direction.Forward;
                break;
            case Direction.Backward:
                pos.z = Mathf.Round(pos.z + 0.5f) - 1.5f;
                pos.x = Mathf.Round(pos.x - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, 180, 0);
                CurrentDirection = Direction.Backward;
                break;
            case Direction.Right:
                pos.x = Mathf.Round(pos.x - 0.5f) + 1.5f;
                pos.z = Mathf.Round(pos.z - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, 90, 0);
                CurrentDirection = Direction.Right;
                break;
            case Direction.Left:
                pos.x = Mathf.Round(pos.x + 0.5f) - 1.5f;
                pos.z = Mathf.Round(pos.z - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, -90, 0);
                CurrentDirection = Direction.Left;
                break;
        }
        Target = pos;
    }

    private void CheckInput()
    {
        InputForward = InputForward || Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;
        if (InputForward) InputBackward = false;
        InputBackward = InputBackward || Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
        if (InputBackward) InputForward = false;
        InputRight = InputRight || Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
        if (InputRight) InputLeft = false;
        InputLeft = InputLeft || Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
        if (InputLeft) InputRight = false;
    }

    public enum Direction
    {
        Forward,
        Backward,
        Right,
        Left
    }
    private static readonly Vector3[] Directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

}
