using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBehaviour : MonoBehaviour
{
    public int Score = 10;
    public Vector3 RotAxis;
    public float RotSpeed;
    public float DissolveTime = 1.0f;
    public float MaxLight = 2000.0f;
    public float MinLight = 0.0f;
    public bool EatGhost;

    private bool IsDissolving;
    private Light Light;
    private float StartScale;

    private void Awake()
    {
        Light = GetComponentInChildren<Light>();
        StartScale = transform.GetChild(0).localScale.x;
    }

    private void Start()
    {
        ScoreManager.Instance.NumberBreads += 1;
    }

    private void Update()
    {
        transform.Rotate(RotAxis, RotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDissolving && other.CompareTag("Player"))
        {
            if (EatGhost)
            {
                GhostManager.Instance.EnableEatableGhosts();
            }

            ScoreManager.Instance.AddScore(Score);
            StartCoroutine(Dissolve());
        }
    }

    private IEnumerator Dissolve()
    {
        IsDissolving = true;
        Material mat = GetComponentInChildren<Renderer>().material;
        float t = 0.0f;
        while (t < DissolveTime)
        {
            float interpolation = t / DissolveTime;
            mat.SetFloat("DissolveT", interpolation);
            Light.intensity = Mathf.Lerp(MinLight, MaxLight, Mathf.PingPong(interpolation, 0.5f));
            transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one * StartScale, Vector3.one * StartScale * 0.5f, interpolation);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        Destroy(mat);
    }
}
