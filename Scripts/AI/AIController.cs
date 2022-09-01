using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private List<Transform> pathNodes;
    private int currNode;
    public int startNode = 0;

    private GameObject frontLeftWheel;
    private GameObject frontRightWheel;
    private GameObject rearLeftWheel;
    private GameObject rearRightWheel;

    private WheelCollider frontLeftWheelCollider;
    private WheelCollider frontRightWheelCollider;
    private WheelCollider rearLeftWheelCollider;
    private WheelCollider rearRightWheelCollider;

    private GameObject centerOfMassObj;
    private Vector3 prevPos;

    public float motorForce = 200f;
    public float maxTurnAngle = 45f;
    public float brakeForce = 10000f;
    public float maxSpeed = 100;
    public float minSpeed = 80f;
    public float wheelRadius = 0.2f;  // 0.2 meters radius = ~16 inch diameter
    public bool is4x4 = true;
    public float wheelZRot = 0f;
    public float angleForBreak = 25f;
    public bool frontBreak = false;
    public float maxDistanceForBreak = 100f;
    public float distToChangeNode = 50f;
    public float distanceNormalizationFactor = 1f;

    public AnimationCurve speedToBreakDistanceCurve;    // [0, 1] -> [0, 1]

    private List<Transform> FindPathNodes()
    {
        pathNodes = new List<Transform>();

        GameObject path = GameObject.FindGameObjectWithTag("Path");
        foreach(Transform node in path.transform)
        {
            pathNodes.Add(node);
        }
        
        return pathNodes;
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

    // Start is called before the first frame update
    void Start()
    {
        pathNodes = FindPathNodes();
        currNode = startNode;

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
                centerOfMassObj = child.gameObject;
            }
        }

        if (!centerOfMassObj)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            float yOffset = -1.0f;
            rb.centerOfMass.Set(rb.centerOfMass.x, rb.centerOfMass.y + yOffset, rb.centerOfMass.z);
        }

        prevPos = transform.position;
    }

    void FixedUpdate()
    {
        float speedKMH = frontLeftWheelCollider.rpm * wheelRadius * 2 * Mathf.PI * 60 / 1000;
        speedKMH = -speedKMH;
        //print(speedKMH);

        Vector3 relativeVector = transform.InverseTransformPoint(pathNodes[currNode].position);
        if (centerOfMassObj)
        {
            relativeVector = centerOfMassObj.transform.InverseTransformPoint(pathNodes[currNode].position);
        }

        Vector2 relativeVector2D = new Vector2(relativeVector.x, relativeVector.z);

        //print(relativeVector2D.magnitude);

        int nextInx = (currNode + 1) % pathNodes.Count;
        //float distToChangeNode = 10;
        //float distToChangeNode = speedKMH / 5;
        //float distToChangeNode = Mathf.Max(40, speedKMH / 3);
        float distToChangeNode = 50;

        //print(currNode);
        relativeVector2D /= distanceNormalizationFactor;
        print(relativeVector2D.magnitude);

        if (relativeVector2D.magnitude < distToChangeNode)
        {
            currNode = nextInx;
        }

        int prevInx = (currNode + pathNodes.Count - 1) % pathNodes.Count;

        Vector2 currPos2D = new Vector2(pathNodes[currNode].position.x, pathNodes[currNode].position.z);
        Vector2 nextPos2D = new Vector2(pathNodes[nextInx].position.x, pathNodes[nextInx].position.z);
        Vector2 prevPos2D = new Vector2(pathNodes[prevInx].position.x, pathNodes[prevInx].position.z);

        Vector2 vec1 = currPos2D - prevPos2D;
        Vector2 vec2 = nextPos2D - currPos2D;
        
        float angle = Vector2.Angle(vec1, vec2);

        bool shouldBreak = false;

        //print(angle);

        //float normalizedAngle = Mathf.Max(angle / 90, nextAngle / 90);
        
        float distToBreak = maxDistanceForBreak * speedToBreakDistanceCurve.Evaluate(speedKMH / maxSpeed);
        float currMinSpeed = minSpeed;

        if(angle >= 45)
        {
            currMinSpeed = Mathf.Min(currMinSpeed, 30);
        }

        //if ((angle > 20 || nextAngle > 20) && speedKMH > minSpeed && relativeVector2D.magnitude < ((speedKMH / 2) + (normalizedAngle * speedKMH / 2)))
        //if ((angle > 20 || nextAngle > 20) && speedKMH > minSpeed && relativeVector2D.magnitude < speedKMH / 2)
        //if ((angle > 20 || nextAngle > 20) && speedKMH > minSpeed && relativeVector2D.magnitude < distToBreak)
        if (angle > angleForBreak && speedKMH > currMinSpeed && relativeVector2D.magnitude < distToBreak)
        {
            shouldBreak = true;
        }

        if (pathNodes[currNode].tag == "Brake")
        {
            shouldBreak = true;
        }

        float dist = (transform.position - prevPos).magnitude;
        prevPos = transform.position;
        float posBasedSpeed = dist / Time.fixedDeltaTime;
        //print(posBasedSpeed);

        bool isMoving = posBasedSpeed > 15;
        if (!isMoving)
        {
            shouldBreak = false;
        }
        print(shouldBreak);

        float currBreakForce = shouldBreak ? brakeForce : 0f;

        rearLeftWheelCollider.brakeTorque = currBreakForce;
        rearRightWheelCollider.brakeTorque = currBreakForce;
        if (frontBreak)
        {
            frontLeftWheelCollider.brakeTorque = currBreakForce;
            frontRightWheelCollider.brakeTorque = currBreakForce;
        }

        float steerPower = -relativeVector2D.x / relativeVector2D.magnitude;
        frontLeftWheelCollider.steerAngle = steerPower * maxTurnAngle;
        frontRightWheelCollider.steerAngle = steerPower * maxTurnAngle;

        float verticalInput = -1f;

        if (speedKMH <= maxSpeed && !shouldBreak)
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

        this.UpdateWheelVisual(frontLeftWheelCollider, frontLeftWheel.transform);
        this.UpdateWheelVisual(frontRightWheelCollider, frontRightWheel.transform);
        this.UpdateWheelVisual(rearLeftWheelCollider, rearLeftWheel.transform);
        this.UpdateWheelVisual(rearRightWheelCollider, rearRightWheel.transform);
    }
}
