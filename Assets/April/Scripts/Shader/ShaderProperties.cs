using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShaderProperties : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            UpdateShaderProperties();
            transform.hasChanged = false; // Reset the hasChanged flag
        }
    }

    private void UpdateShaderProperties()
    {
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        foreach (var r in renderers)
        {
            Material m;
#if UNITY_EDITOR
            m = r.sharedMaterial;
#else
            m = r.material;
#endif
            if (string.Compare(m.shader.name, "Shader Graphs/ToonRamp") == 0 || string.Compare(m.shader.name, "Shader Graphs/ToonRampColor") == 0)
            {
                m.SetVector("_LightDir", transform.forward);
            }
        }
    }
}