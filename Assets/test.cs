using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform bone1, bone2;


    [ContextMenu("Test")]
    public void Test()
    {
        GetComponent<SkinnedMeshRenderer>().bones = new Transform[] { bone1, bone2 };
    }
}
