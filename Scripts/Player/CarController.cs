using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private GameObject frontLeftWheel;
    private GameObject frontRightWheel;
    private GameObject rearLeftWheel;
    private GameObject rearRightWheel;

    private WheelCollider frontLeftWheelCollider;
    private WheelCollider frontRightWheelCollider;
    private WheelCollider rearLeftWheelCollider;
    private WheelCollider rearRightWheelCollider;

    public float motorForce = 200f;
    public float maxTurnAngle = 45f;
    public float brakeForce = 10000f;
    public float maxSpeed = 100;
    public float wheelRadius = 0.2f;  // 0.2 meters radius = ~16 inch diameter
    public bool is4x4 = true;
    public float wheelZRot = 0f;

    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        bool customCenterOfMassFound = false;

        foreach (Transform child in transform)
        {
            if (child.name == "Wheels")
            {
                foreach (Transform child2 in child)
                {
                    if (child2.name == "Front left wheel")
                    {
                        frontLeftWheel = child2.gameObject;
                    }
                    if (child2.name == "Front right wheel")
                    {
                        frontRightWheel = child2.gameObject;
                    }
                    if (child2.name == "Rear left wheel")
                    {
                        rearLeftWheel = child2.gameObject;
                    }
                    if (child2.name == "Rear right wheel")
                    {
                        rearRightWheel = child2.gameObject;
                    }
                }
            }

            if (child.name == "Wheel colliders")
            {
                foreach (Transform child2 in child)
                {
                    if (child2.name == "Front left wheel collider")
                    {
                        frontLeftWheelCollider = child2.GetComponent<WheelCollider>();
                    }
                    if (child2.name == "Front right wheel collider")
                    {
                        frontRightWheelCollider = child2.GetComponent<WheelCollider>();
                    }
                    if (child2.name == "Rear left wheel collider")
                    {
                        rearLeftWheelCollider = child2.GetComponent<WheelCollider>();
                    }
                    if (child2.name == "Rear right wheel collider")
                    {
                        rearRightWheelCollider = child2.GetComponent<WheelCollider>();
                    }
                }
            }

            if (child.name == "Center of mass")
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.centerOfMass = child.localPosition;

                customCenterOfMassFound = true;
            }
        }

        if (!customCenterOfMassFound)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            float yOffset = -1.0f;
            rb.centerOfMass.Set(rb.centerOfMass.x, rb.centerOfMass.y + yOffset, rb.centerOfMass.z);
        }

        prevPos = transform.position;
    }
    private void UpdateWheelVisual(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        //wheelTransform.rotation = rot;

        var localRot = (Quaternion.Inverse(wheelCollider.transform.parent.rotation) * Quaternion.Euler(rot.eulerAngles));

        float xRot = localRot.eulerAngles.x;
        float yRot = localRot.eulerAngles.y;
        float zRot = wheelZRot;

        if (yRot > 300 || yRot < 60)
        {
            yRot += 180;
            xRot = 180 - xRot;
        }

        if (wheelCollider.name.Contains("left"))
        {
            zRot = -zRot;
        }

        //wheelTransform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, zRot);

        wheelTransform.localRotation = Quaternion.Euler(xRot, yRot, zRot);
    }
    private void FixedUpdate()
    {
        float verticalInput = -Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // calculate speed
        float speedKMH = frontLeftWheelCollider.rpm * wheelRadius * 2 * Mathf.PI * 60 / 1000;
        speedKMH = -speedKMH;
        //print(speedKMH);

        float dist = (transform.position - prevPos).magnitude;
        prevPos = transform.position;
        float speed = dist / Time.fixedDeltaTime;

        // forward
        if (speedKMH <= maxSpeed)
        {
            if (is4x4)
            {
                frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
                frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            }
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        else
        {
            if (is4x4)
            {
                frontLeftWheelCollider.motorTorque = 0;
                frontRightWheelCollider.motorTorque = 0;
            }
            rearLeftWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
        }

        // turn
        frontLeftWheelCollider.steerAngle = horizontalInput * maxTurnAngle;
        frontRightWheelCollider.steerAngle = horizontalInput * maxTurnAngle;

        // brake
        //bool isBraking = Input.GetKey(KeyCode.Space) || ((speed) >= 15 && horizontalInput > 0);
        bool isBraking = Input.GetKey(KeyCode.Space);
        float currBreakForce = isBraking ? brakeForce : 0f;

        rearLeftWheelCollider.brakeTorque = currBreakForce;
        rearRightWheelCollider.brakeTorque = currBreakForce;
        //frontLeftWheelCollider.brakeTorque = currBreakForce;
        //frontRightWheelCollider.brakeTorque = currBreakForce;

        // update wheels visual
        this.UpdateWheelVisual(frontLeftWheelCollider, frontLeftWheel.transform);
        this.UpdateWheelVisual(frontRightWheelCollider, frontRightWheel.transform);
        this.UpdateWheelVisual(rearLeftWheelCollider, rearLeftWheel.transform);
        this.UpdateWheelVisual(rearRightWheelCollider, rearRightWheel.transform);
    }
}
