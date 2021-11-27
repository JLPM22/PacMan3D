using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Direction = PacManMovement.Direction;

public class GhostMovement : MonoBehaviour
{
    public GhostColor Color;
    public float Speed = 1.0f;

    public Direction CurrentDirection { get; private set; }

    public Transform ForwardTrigger;
    public Transform BackwardTrigger;
    public Transform RightTrigger;
    public Transform LeftTrigger;

    private Vector3 Target;
    private Vector3 LastPosition;

    private bool InputForward, InputBackward, InputRight, InputLeft;

    private int ForwardPhysicsLayerMask;
    private int PhysicsLayerMask;

    private PacManMovement PacMan;

    private void Awake()
    {
        ForwardPhysicsLayerMask = ~LayerMask.GetMask("OnlyGhost", "Ghost", "PacMan");
        PhysicsLayerMask = ~LayerMask.GetMask("Ghost", "PacMan");
        PacMan = FindObjectOfType<PacManMovement>();
    }

    private void Start()
    {
        Target = transform.position;
    }

    private void Update()
    {
        bool forwardBlocked = CheckCollision(Direction.Forward);
        bool backwardBlocked = CheckCollision(Direction.Backward);
        bool leftBlocked = CheckCollision(Direction.Left);
        bool rightBlocked = CheckCollision(Direction.Right);
        CheckInput(forwardBlocked, backwardBlocked, leftBlocked, rightBlocked);

        float x = transform.position.x - Mathf.Floor(transform.position.x);
        float z = transform.position.z - Mathf.Floor(transform.position.z);

        if (InputForward && x >= 0.5f && x < 0.6f)
        {
            ComputeTarget(Direction.Forward);
        }
        else if (InputBackward && x >= 0.5f && x < 0.6f)
        {
            ComputeTarget(Direction.Backward);
        }
        else if (InputLeft && z >= 0.5f && z < 0.6f)
        {
            ComputeTarget(Direction.Left);
        }
        else if (InputRight && z >= 0.5f && z < 0.6f)
        {
            ComputeTarget(Direction.Right);
        }

        float speed = ScoreManager.Instance.Score > 1800 && Color == GhostColor.Red ? Speed * 1.5f : Speed;
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);

        LastPosition = transform.position;
    }

    private bool CheckCollision(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Physics.CheckSphere(ForwardTrigger.position, 0.1f, ForwardPhysicsLayerMask);
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
                CurrentDirection = Direction.Forward;
                break;
            case Direction.Backward:
                pos.z = Mathf.Round(pos.z + 0.5f) - 1.5f;
                pos.x = Mathf.Round(pos.x - 0.5f) + 0.5f;
                CurrentDirection = Direction.Backward;
                break;
            case Direction.Right:
                pos.x = Mathf.Round(pos.x - 0.5f) + 1.5f;
                pos.z = Mathf.Round(pos.z - 0.5f) + 0.5f;
                CurrentDirection = Direction.Right;
                break;
            case Direction.Left:
                pos.x = Mathf.Round(pos.x + 0.5f) - 1.5f;
                pos.z = Mathf.Round(pos.z - 0.5f) + 0.5f;
                CurrentDirection = Direction.Left;
                break;
        }
        Target = pos;
    }

    private void CheckInput(bool forwardBlocked, bool backwardBlocked, bool leftBlocked, bool rightBlocked)
    {
        Vector3 dir = PacMan.transform.position - transform.position;
        if (Color == GhostColor.Blue)
        {
            float mag = dir.magnitude;
            dir = (PacMan.transform.position + Directions[(int)PacMan.CurrentDirection] * mag) - transform.position;
        }
        else if (Color == GhostColor.Pink)
        {
            float mag = dir.magnitude;
            dir = (PacMan.transform.position - Directions[(int)PacMan.CurrentDirection] * mag) - transform.position;
        }
        else if (Color == GhostColor.Orange)
        {
            dir = (PacMan.transform.position + dir) - transform.position;
        }

        Vector3 closestDir = FindClosestDirection(dir, forwardBlocked, backwardBlocked, leftBlocked, rightBlocked);

        InputForward = closestDir == Vector3.forward;
        InputBackward = closestDir == Vector3.back;
        InputRight = closestDir == Vector3.right;
        InputLeft = closestDir == Vector3.left;
    }

    private Vector3 FindClosestDirection(Vector3 dir, bool forwardBlocked, bool backwardBlocked, bool leftBlocked, bool rightBlocked)
    {
        dir = dir.normalized;
        Vector3 closest = Vector3.zero;
        float max = float.MinValue;
        foreach (Vector3 v in Directions)
        {
            if (forwardBlocked && v == Vector3.forward ||
                backwardBlocked && v == Vector3.back ||
                leftBlocked && v == Vector3.left ||
                rightBlocked && v == Vector3.right ||
                v == OppositeDirections[(int)CurrentDirection]) continue;

            float dist = Vector3.Dot(dir, v);
            if (dist > max)
            {
                max = dist;
                closest = v;
            }
        }
        return closest;
    }

    public enum GhostColor
    {
        Red,
        Blue,
        Pink,
        Orange
    }

    private static readonly Vector3[] Directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    private static readonly Vector3[] OppositeDirections = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right };
}
