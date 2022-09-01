using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCheckpoint : MonoBehaviour
{
    private GameObject checkpointPanel;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.name == "CheckpointPanel")
            {
                checkpointPanel = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        PlayerCheckpoints.OnPlayerCheckpoint += PopupCheckpoint;
    }

    private void OnDisable()
    {
        PlayerCheckpoints.OnPlayerCheckpoint -= PopupCheckpoint;
    }

    private void PopupCheckpoint(int notUsed)
    {
        StartCoroutine(PopupCheckpointCoroutine());
    }

    private IEnumerator PopupCheckpointCoroutine()
    {
        checkpointPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        checkpointPanel.SetActive(false);

        yield break;
    }
}
