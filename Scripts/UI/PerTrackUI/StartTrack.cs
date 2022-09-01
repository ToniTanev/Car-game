using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTrack : MonoBehaviour
{
    private List<GameObject> cars = new List<GameObject>();

    private GameObject startTrackPanel;
    private GameObject text1;
    private GameObject text2;
    private GameObject text3;
    private GameObject goText;

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjs = scene.GetRootGameObjects();

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

        foreach (Transform obj in transform)
        {
            if (obj.name == "StartTrackPanel")
            {
                startTrackPanel = obj.gameObject;

                foreach (Transform obj2 in obj)
                {
                    if (obj2.name == "Text1")
                    {
                        text1 = obj2.gameObject;
                    }

                    if (obj2.name == "Text2")
                    {
                        text2 = obj2.gameObject;
                    }

                    if (obj2.name == "Text3")
                    {
                        text3 = obj2.gameObject;
                    }

                    if (obj2.name == "GoText")
                    {
                        goText = obj2.gameObject;
                    }
                }
            }
        }

        startTrackPanel.SetActive(false);
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
        goText.SetActive(false);

        StartCoroutine(OnStartTrack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator OnStartTrack()
    {
        Time.timeScale = 0f;
        startTrackPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f);
        text3.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        text3.SetActive(false);
        text2.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        text2.SetActive(false);
        text1.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        text1.SetActive(false);
        goText.SetActive(true);

        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(1f);
        goText.SetActive(false);

        startTrackPanel.SetActive(false);

        yield break;
    }
}
