using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIController : MonoBehaviour
{
    public CinemachineFreeLook Camera;
    public GameObject[] Objects;
    public GameObject[] Ghosts;
    public GameObject PacManEye;
    public GameObject PacManGravel;
    public GameObject PacManPaint;
    public GameObject PacManSponge;
    public GameObject PacManMetal;

    public void PlayEye()
    {
        PacManEye.SetActive(true);
        SetCamera(PacManEye.transform);
        EnableObjects();
        DisableUI();
    }

    public void PlayGravel()
    {
        PacManGravel.SetActive(true);
        SetCamera(PacManGravel.transform);
        EnableObjects();
        DisableUI();
    }

    public void PlayPaint()
    {
        PacManPaint.SetActive(true);
        SetCamera(PacManPaint.transform);
        EnableObjects();
        DisableUI();
    }

    public void PlaySponge()
    {
        PacManSponge.SetActive(true);
        SetCamera(PacManSponge.transform);
        EnableObjects();
        DisableUI();
    }

    public void PlayMetal()
    {
        PacManMetal.SetActive(true);
        SetCamera(PacManMetal.transform);
        EnableObjects();
        DisableUI();
    }

    private void SetCamera(Transform t)
    {
        Camera.Follow = t;
        Camera.LookAt = t;
    }

    private void EnableObjects()
    {
        foreach (GameObject ghost in Ghosts)
        {
            ghost.SetActive(true);
        }

        foreach (GameObject obj in Objects)
        {
            obj.SetActive(true);
        }
    }

    private void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
