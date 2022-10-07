using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool update = false;
    [SerializeField] private bool reset = false;

    [SerializeField] private MeshFilter m;
    [SerializeField] private float scale;
    [SerializeField] private float spaceScale;

    private Vector3[] verts;

    // 5000k default
    // 2 intensity

    private void OnValidate()
    {
        if (update)
        {
            update = false;
            var mesh = m.sharedMesh;
            var vertices = mesh.vertices;
            verts = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float n = Perlin.Noise(vertices[i] * spaceScale);
                vertices[i] *= 1 + (n * scale);
            }
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }
        if (reset)
        {
            reset = false;
            var mesh = m.sharedMesh;
            mesh.vertices = verts;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }
    }
}
