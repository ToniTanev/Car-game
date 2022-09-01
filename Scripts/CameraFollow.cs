using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform lookTarget;
    public float smoothSpeed = 0.125f;

    private void FindActiveCar()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        foreach (GameObject car in cars)
        {
            if (car.activeSelf)
            {
                foreach (Transform child in car.transform)
                {
                    if (child.name == "Camera target")
                    {
                        target = child;
                    }
                    else if (child.name == "Camera look target")
                    {
                        lookTarget = child;
                    }
                }
            }
        }
    }

    private void Start()
    {
        //FindActiveCar();
    }

    private void FixedUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed);

            if (lookTarget)
            {
                transform.LookAt(lookTarget);
            }
            else
            {
                transform.LookAt(target.parent.position);
            }

            //transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
        }
        else
        {
            FindActiveCar();
        }
    }
}
