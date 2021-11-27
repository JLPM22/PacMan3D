using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;

    public Material EatableMaterial;

    private PacManMovement Player;
    private GhostMovement[] Ghosts;
    private Material[] GhostMaterials;

    private void Awake()
    {
        Debug.Assert(Instance == null, "There can only be one GhostManager");
        Instance = this;

        Player = FindObjectOfType<PacManMovement>();

        Ghosts = FindObjectsOfType<GhostMovement>();
        GhostMaterials = new Material[Ghosts.Length];
        int i = 0;
        foreach (GhostMovement ghost in Ghosts)
        {
            GhostMaterials[i++] = ghost.GetComponentInChildren<Renderer>().material;
        }
    }

    public void EnableEatableGhosts()
    {
        int i = 0;
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.SetEatable(false);
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = GhostMaterials[i++];
        }
        Player.SetCanEat(false);
        StopAllCoroutines();
        StartCoroutine(EatGhostCoroutine());
    }

    private IEnumerator EatGhostCoroutine()
    {
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.SetEatable(true);
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = EatableMaterial;
        }
        Player.SetCanEat(true);
        yield return new WaitForSeconds(9.0f);
        int i = 0;
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = GhostMaterials[i++];
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = EatableMaterial;
        }
        yield return new WaitForSeconds(0.5f);
        i = 0;
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = GhostMaterials[i++];
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = EatableMaterial;
        }
        yield return new WaitForSeconds(0.5f);
        i = 0;
        foreach (GhostMovement ghost in Ghosts)
        {
            ghost.SetEatable(false);
            ghost.GetComponentInChildren<Renderer>().sharedMaterial = GhostMaterials[i++];
        }
        Player.SetCanEat(false);
    }

    private void OnDestroy()
    {
        foreach (Material m in GhostMaterials)
        {
            Destroy(m);
        }
    }
}
