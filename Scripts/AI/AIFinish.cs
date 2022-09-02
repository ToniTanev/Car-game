using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFinish : MonoBehaviour
{
    private int finishPassedCnt = 0;

    public static event Action<bool> OnAIFinish;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            finishPassedCnt++;

            if (finishPassedCnt == 2 && !GameState.isGameFinished)
            {
                GameState.isGameFinished = true;

                if (OnAIFinish != null)
                {
                    OnAIFinish(false);
                }
            }
        }
    }
}
