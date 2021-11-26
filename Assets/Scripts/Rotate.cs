using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 Axis;
    public float Speed;

    private void Update()
    {
        transform.Rotate(Axis, Speed * Time.deltaTime);
    }

}
