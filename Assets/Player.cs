using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Private Set Unity Variables
    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] public float speed;
    [SerializeField] private float zRotateDegreesPerSecond;
    [SerializeField] private float zRotateDegreesPerSecondSmooth;
    [SerializeField] public float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float lookSpeed;

    // Private Variables
    private Body[] bodies;
    private float zRotateVelocity = 0;
    private float zRotateSpeed = 0;

    private void Start()
    {
        // Position Own Camera
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        Camera.main.transform.rotation = Quaternion.identity;

        SetCursorVisibility(false);
    }

    private void Awake()
    {
        bodies = FindObjectsOfType<Body>(true);
    }

    private void Update()
    {
        Vector3 v = transform.position.normalized;
        float lat = (float)Mathf.Acos(v.y / 1.001f); //theta
        float lon = (float)Mathf.Atan(v.x / v.z); //phi
        print(lat * Mathf.Rad2Deg + " : " + lon * Mathf.Rad2Deg);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorVisibility(!Cursor.visible);
        }

        Body largestForceBody = null;
        float largestForceMagnitude = float.NegativeInfinity;
        Vector3 largestForceDirection = Vector3.zero;
        Vector3 forceSum = Vector3.zero;
        foreach (var body in bodies)
        {
            Vector3 force = body.GetGravitationalForce(transform.position);
            float magnitude = force.magnitude;
            if (magnitude > largestForceMagnitude)
            {
                largestForceMagnitude = magnitude;
                largestForceBody = body;
                largestForceDirection = force.normalized;
            }
            forceSum += force;
        }
        rb.AddForce(forceSum * Time.deltaTime, ForceMode.VelocityChange);
        print(forceSum.magnitude * 5);

        float speedX = speed * Input.GetAxis("Vertical");
        float speedY = speed * Input.GetAxis("Horizontal");

        rb.AddForce(Time.deltaTime * speedX * transform.forward, ForceMode.VelocityChange);
        rb.AddForce(Time.deltaTime * speedY * transform.right, ForceMode.VelocityChange);

        // normalize force vector
        // get movement vector force
        // force = movement vector force.normalized * speed

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.AddForce(Time.deltaTime * speed * transform.up, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.AddForce(Time.deltaTime * speed * -transform.up, ForceMode.VelocityChange);
        }

        /*
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.VelocityChange);
        }
        */

        Quaternion quaternion = transform.rotation;

        quaternion *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * lookSpeed, 0, 0);
        quaternion *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            //zRotateSpeed = Mathf.SmoothDamp(zRotateSpeed, zRotateDegreesPerSecond, ref zRotateVelocity, zRotateDegreesPerSecondSmooth);
            quaternion *= Quaternion.Euler(0, 0, Time.deltaTime * zRotateDegreesPerSecond);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            //zRotateSpeed = Mathf.SmoothDamp(zRotateSpeed, -zRotateDegreesPerSecond, ref zRotateVelocity, zRotateDegreesPerSecondSmooth);
            quaternion *= Quaternion.Euler(0, 0, Time.deltaTime * -zRotateDegreesPerSecond);
        }
        else
        {
            // clear velocity and pos of smooth damp
            zRotateSpeed = 0;
            zRotateVelocity = 0;
        }

        transform.rotation = quaternion;

        /*
        if (transform.position.y < -8)
        {
            transform.position = new Vector3(0, 8, 0);
        }
        */
    }

    private void SetCursorVisibility(bool isVisible)
    {
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //rb.velocity = Vector3.zero;
    }
}
