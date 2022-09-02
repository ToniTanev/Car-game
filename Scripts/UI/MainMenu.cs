using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(Constants.chooseMapMenuIndex);
    }

    public void OnOptionsButton()
    {
        SceneManager.LoadScene(Constants.optionsMenuIndex);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
