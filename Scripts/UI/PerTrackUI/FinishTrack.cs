using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrack : MonoBehaviour
{
    private GameObject finishTrackMenu;
    private GameObject victoryText;
    private GameObject defeatText;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform obj in transform)
        {
            if (obj.name == "FinishTrackMenu")
            {
                finishTrackMenu = obj.gameObject;

                foreach (Transform obj2 in obj)
                {
                    if (obj2.name == "VictoryText")
                    {
                        victoryText = obj2.gameObject;
                    }

                    if (obj2.name == "DefeatText")
                    {
                        defeatText = obj2.gameObject;
                    }
                }
            }
        }

        finishTrackMenu.SetActive(false);
        victoryText.SetActive(false);
        defeatText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        PlayerCheckpoints.OnPlayerFinish += OnGameEnd;
    }

    private void OnDisable()
    {
        PlayerCheckpoints.OnPlayerFinish -= OnGameEnd;
    }

    private void OnGameEnd(bool playerWon)
    {
        finishTrackMenu.SetActive(true);

        if(playerWon)
        {
            victoryText.SetActive(true);
        }
        else
        {
            defeatText.SetActive(true);
        }
    }
}
