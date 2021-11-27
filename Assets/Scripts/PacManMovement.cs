using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PacManMovement : MonoBehaviour
{
    public float Speed = 1.0f;
    public float RotSpeed = 100.0f;
    public Transform RotTransform;

    public Direction CurrentDirection { get; private set; }

    public Transform ForwardTrigger;
    public Transform BackwardTrigger;
    public Transform RightTrigger;
    public Transform LeftTrigger;

    private Animator Animator;
    private int WalkHash = Animator.StringToHash("Walk");

    private Vector3 Target;
    private Vector3 TargetRot;
    private Vector3 LastPosition;

    private bool InputForward, InputBackward, InputRight, InputLeft;
    private bool MovingZAxis; // true -> z-axis, false -> x-axis

    private int PhysicsLayerMask;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        PhysicsLayerMask = ~LayerMask.GetMask("Bread");
    }

    private void Start()
    {
        Target = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputForward = true;
        MovingZAxis = true;
    }

    private void Update()
    {
        CheckInput();

        float x = transform.position.x - Mathf.Floor(transform.position.x);
        float z = transform.position.z - Mathf.Floor(transform.position.z);

        if (!MovingZAxis && x >= 0.5f && x < 0.6f && ((InputForward && !CheckCollision(Direction.Forward)) || (InputBackward && !CheckCollision(Direction.Backward))))
        {
            InputRight = false;
            InputLeft = false;
        }

        if (MovingZAxis && z >= 0.5f && z < 0.6f && ((InputRight && !CheckCollision(Direction.Right)) || (InputLeft && !CheckCollision(Direction.Left))))
        {
            InputForward = false;
            InputBackward = false;
        }

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

        float speed = Time.time - ScoreManager.Instance.LastTimeScoreChanged < 1.0f ? Speed * 0.75f : Speed;
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);
        RotTransform.rotation = Quaternion.RotateTowards(RotTransform.rotation, Quaternion.Euler(TargetRot), RotSpeed * Time.deltaTime);

        Animator.SetBool(WalkHash, Vector3.SqrMagnitude(transform.position - LastPosition) > 0.01f * 0.01f);
        LastPosition = transform.position;
    }

    private bool CheckCollision(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Physics.CheckSphere(ForwardTrigger.position, 0.1f, PhysicsLayerMask);
            case Direction.Backward:
                return Physics.CheckSphere(BackwardTrigger.position, 0.1f, PhysicsLayerMask);
            case Direction.Right:
                return Physics.CheckSphere(RightTrigger.position, 0.1f, PhysicsLayerMask);
            case Direction.Left:
                return Physics.CheckSphere(LeftTrigger.position, 0.1f, PhysicsLayerMask);
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
                MovingZAxis = true;
                break;
            case Direction.Backward:
                pos.z = Mathf.Round(pos.z + 0.5f) - 1.5f;
                pos.x = Mathf.Round(pos.x - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, 180, 0);
                CurrentDirection = Direction.Backward;
                MovingZAxis = true;
                break;
            case Direction.Right:
                pos.x = Mathf.Round(pos.x - 0.5f) + 1.5f;
                pos.z = Mathf.Round(pos.z - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, 90, 0);
                CurrentDirection = Direction.Right;
                MovingZAxis = false;
                break;
            case Direction.Left:
                pos.x = Mathf.Round(pos.x + 0.5f) - 1.5f;
                pos.z = Mathf.Round(pos.z - 0.5f) + 0.5f;
                TargetRot = new Vector3(0, -90, 0);
                CurrentDirection = Direction.Left;
                MovingZAxis = false;
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
