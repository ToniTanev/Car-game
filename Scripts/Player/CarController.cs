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

    private List<GameObject> wheelsVFX = new List<GameObject>();

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

            if(child.tag == "WheelVFX")
            {
                wheelsVFX.Add(child.gameObject);
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
    
    private void FixedUpdate()
    {
        float verticalInput = -Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // calculate speed
        float speedKMH = frontLeftWheelCollider.rpm * wheelRadius * 2 * Mathf.PI * 60 / 1000;
        speedKMH = -speedKMH;

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
        bool isBraking = Input.GetKey(KeyCode.Space);
        float currBreakForce = isBraking ? brakeForce : 0f;

        rearLeftWheelCollider.brakeTorque = currBreakForce;
        rearRightWheelCollider.brakeTorque = currBreakForce;
        //frontLeftWheelCollider.brakeTorque = currBreakForce;
        //frontRightWheelCollider.brakeTorque = currBreakForce;

        // update wheels visual
        CarController.UpdateWheelVisual(frontLeftWheelCollider, frontLeftWheel.transform, wheelZRot);
        CarController.UpdateWheelVisual(frontRightWheelCollider, frontRightWheel.transform, wheelZRot);
        CarController.UpdateWheelVisual(rearLeftWheelCollider, rearLeftWheel.transform, wheelZRot);
        CarController.UpdateWheelVisual(rearRightWheelCollider, rearRightWheel.transform, wheelZRot);

        bool isCarOnGround = frontLeftWheelCollider.isGrounded && frontRightWheelCollider.isGrounded && rearLeftWheelCollider.isGrounded && rearRightWheelCollider.isGrounded;
        bool isWheelVFXActive = isCarOnGround && (isBraking || CarController.IsDrifting(this.gameObject, prevPos));

        foreach (GameObject wheelVFX in wheelsVFX)
        {
            CarController.SetWheelVFXActive(wheelVFX, isWheelVFXActive);
        }

        prevPos = transform.position;
    }

    static public void UpdateWheelVisual(WheelCollider wheelCollider, Transform wheelTransform, float wheelZRot)
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

    public static bool IsDrifting(GameObject car, Vector3 prevPos)
    {
        Vector3 speed = (car.transform.position - prevPos).normalized;
        Vector3 carDirection = -car.transform.forward.normalized;

        float angle = Mathf.Acos(Vector3.Dot(speed, carDirection)) * Mathf.Rad2Deg;
        float angleThreshold = 30f;

        return angle > angleThreshold && angle < 180 - angleThreshold;
    }

    public static void SetWheelVFXActive(GameObject wheelVFX, bool isActive)
    {
        ParticleSystem particleSystem = wheelVFX.GetComponent<ParticleSystem>();

        if (isActive)
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
        
        TrailRenderer trailRenderer = wheelVFX.GetComponent<TrailRenderer>();
        trailRenderer.emitting = isActive;
    }
}
