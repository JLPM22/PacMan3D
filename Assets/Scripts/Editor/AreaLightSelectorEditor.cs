using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AreaLightSelectorEditor : EditorWindow
{
    [MenuItem("Window/AreaLightSelector")]
    static void Init()
    {
        AreaLightSelectorEditor window = (AreaLightSelectorEditor)EditorWindow.GetWindow(typeof(AreaLightSelectorEditor));
        window.Show();
    }

    private List<Vector3> AlreadyEnabled = new List<Vector3>();

    void OnGUI()
    {
        if (GUILayout.Button("Disable All"))
        {
            AlreadyEnabled.Clear();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Emissive_to_baked"))
            {
                foreach (Light l in go.GetComponentsInChildren<Light>())
                {
                    l.gameObject.SetActive(false);
                }
            }
        }

        if (GUILayout.Button("Enable based on Scene Camera"))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Emissive_to_baked"))
            {
                foreach (Light l in go.GetComponentsInChildren<Light>(true))
                {
                    Vector3 cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
                    if (CheckIfAlreadyEnabled(l.transform.position)) continue;
                    if (!Physics.CheckSphere(l.transform.position + (cameraPos - l.transform.position).normalized * 0.1f, 0.01f))
                    {
                        l.gameObject.SetActive(true);
                        AlreadyEnabled.Add(l.transform.position);
                    }
                }
            }
        }
    }

    private bool CheckIfAlreadyEnabled(Vector3 pos)
    {
        foreach (Vector3 v in AlreadyEnabled)
        {
            if (Vector3.Distance(v, pos) < 0.1f) return true;
        }
        return false;
    }
}
