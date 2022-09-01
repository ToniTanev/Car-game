using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoints : MonoBehaviour
{
    private int passedCheckpoints = 0;
    private int goalCheckpoints;

    public static event Action<int> OnPlayerCheckpoint;
    public static event Action<bool> OnPlayerFinish;
    // Start is called before the first frame update
    void Start()
    {
        goalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            other.gameObject.SetActive(false);
            passedCheckpoints++;

            if (OnPlayerCheckpoint != null)
            {
                OnPlayerCheckpoint(0 /* not used */);
            }
        }
        else if (other.tag == "FinishLine" && passedCheckpoints == goalCheckpoints)
        {
            if (OnPlayerFinish != null)
            {
                OnPlayerFinish(true);
            }
        }
    }
}
