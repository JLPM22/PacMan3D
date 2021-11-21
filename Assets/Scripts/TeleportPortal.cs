using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    public Transform TeleportTarget;
    public PacManMovement.Direction Direction;

    private PacManMovement Player;

    private void Start()
    {
        Player = FindObjectOfType<PacManMovement>();
    }

    private void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < 0.1f &&
            Direction == Player.CurrentDirection)
        {
            Player.transform.position = TeleportTarget.position;
        }
    }
}
