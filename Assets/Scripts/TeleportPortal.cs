using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    public Transform TeleportTarget;
    public PacManMovement.Direction Direction;

    private PacManMovement Player;
    private GhostMovement[] Ghosts;

    private void Start()
    {
        Player = FindObjectOfType<PacManMovement>();
        Ghosts = FindObjectsOfType<GhostMovement>();
    }
    private void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < 0.1f &&
            Direction == Player.CurrentDirection)
        {
            Player.transform.position = TeleportTarget.position;
        }

        foreach (GhostMovement ghost in Ghosts)
        {
            if (Vector3.Distance(ghost.transform.position, transform.position) < 0.1f &&
                Direction == ghost.CurrentDirection)
            {
                ghost.transform.position = TeleportTarget.position;
            }
        }
    }
}
