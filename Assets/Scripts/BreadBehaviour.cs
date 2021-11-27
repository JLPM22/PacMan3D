using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBehaviour : MonoBehaviour
{
    public Vector3 RotAxis;
    public float RotSpeed;

    private void Update()
    {
        transform.Rotate(RotAxis, RotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        ScoreManager.Instance.AddScore(1);
        Destroy(gameObject);
    }
}
