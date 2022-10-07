using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float gravityScale;
    public float gm;

    // 0.05, 0.05
    // 0.01, 0.15

    public Vector3 GetGravitationalForce(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position) * 5;
        float magnitude = (gm * gravityScale) / (distance * distance);
        Vector3 vector = transform.position - position;
        return vector.normalized * magnitude;
    }
}
