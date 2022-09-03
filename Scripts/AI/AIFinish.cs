using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFinish : MonoBehaviour
{
    private int finishPassesCnt = 0;

    public static event Action<bool> OnAIFinish;

    public void ResetFinishPasses()
    {
        finishPassesCnt = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FinishLine")
        {
            finishPassesCnt++;

            if (finishPassesCnt == 2 && !GameState.isGameFinished)
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
