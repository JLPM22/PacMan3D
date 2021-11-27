using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBehaviour : MonoBehaviour
{
    public Vector3 RotAxis;
    public float RotSpeed;
    public float DissolveTime = 1.0f;
    public float MaxLight = 2000.0f;
    public float MinLight = 0.0f;

    private bool IsDissolving;
    private Light Light;

    private void Awake()
    {
        Light = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        transform.Rotate(RotAxis, RotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDissolving && other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(1);
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
            transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one * 5, Vector3.one * 2.5f, interpolation);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        Destroy(mat);
    }
}
