using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMap : MonoBehaviour
{
    public GameObject trackName;
    public GameObject difficulty;

    private List<GameObject> trackBackgrounds = new List<GameObject>();
    public static int currTrack = 0;

    private List<string> difficultyStrings = new List<string> { "Easy", "Medium", "Hard" };
    private int currDifficultyString = 0;

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjs = scene.GetRootGameObjects();

        foreach (GameObject obj in rootObjs)
        {
            if (obj.name == "Canvas")
            {
                foreach (Transform obj2 in obj.transform)
                {
                    if (obj2.tag == "TrackBG")
                    {
                        trackBackgrounds.Add(obj2.gameObject);
                        obj2.gameObject.SetActive(false);
                    }
                }
            }
        }

        trackBackgrounds[currTrack].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNextButton()
    {
        SceneManager.LoadScene(Constants.chooseCarMenuIndex);
    }

    public void OnMapButton()
    {
        trackBackgrounds[currTrack].SetActive(false);
        currTrack = (currTrack + 1) % trackBackgrounds.Count;
        trackBackgrounds[currTrack].SetActive(true);

        trackName.GetComponent<TextMeshProUGUI>().text = trackBackgrounds[currTrack].name;
    }

    public void OnDifficultyButton()
    {
        currDifficultyString = (currDifficultyString + 1) % difficultyStrings.Count;
        difficulty.GetComponent<TextMeshProUGUI>().text = difficultyStrings[currDifficultyString];
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene(Constants.mainMenuIndex);
    }
}
