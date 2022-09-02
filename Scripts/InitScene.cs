using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjs = scene.GetRootGameObjects();

        List<GameObject> cars = new List<GameObject>();

        foreach (GameObject obj in rootObjs)
        {
            if (obj.tag == "Car")
            {
                cars.Add(obj);
            }
        }

        foreach (GameObject car in cars)
        {
            car.SetActive(false);
        }

        cars[ChooseCar.currCar].SetActive(true);

        GameState.isGameFinished = false;
    }
}
