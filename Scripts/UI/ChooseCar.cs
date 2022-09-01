using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCar : MonoBehaviour
{
    public GameObject carName;

    private List<GameObject> cars = new List<GameObject>();
    public static int currCar = 0;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjs = scene.GetRootGameObjects();

        foreach(GameObject obj in rootObjs)
        {
            if(obj.tag == "Car")
            {
                cars.Add(obj);
            }
        }

        foreach (GameObject car in cars)
        {
            car.SetActive(false);
        }

        cars[currCar].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStartButton()
    {
        SceneManager.LoadScene(ChooseMap.currTrack);
    }

    public void OnCarButton()
    {
        cars[currCar].SetActive(false);
        currCar = (currCar + 1) % cars.Count;
        cars[currCar].SetActive(true);
        carName.GetComponent<TextMeshProUGUI>().text = cars[currCar].name;
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene(Constants.chooseMapMenuIndex);
    }
}
