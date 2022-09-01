using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateCars : MonoBehaviour
{
    public float rotSpeed = 1f;

    private GameObject[] cars;

    // Start is called before the first frame update
    void Start()
    {
        cars = GameObject.FindGameObjectsWithTag("Car");
    }

    void FixedUpdate()
    {
        foreach (GameObject car in cars)
        {
            Transform transform = car.transform;

            foreach(Transform obj in car.transform)
            {
                if(obj.name == "Rot")
                {
                    transform = obj;
                }
            }

            transform.Rotate(new Vector3(0, rotSpeed * Time.fixedDeltaTime, 0));
        }
    }
}
