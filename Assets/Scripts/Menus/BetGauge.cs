using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetGauge : MonoBehaviour
{
    public Image[] betIcons;

    private int lastBestCount = -1;

    private void Start()
    {
        for (int i = 0; i < betIcons.Length; i++)
        {
            betIcons[i].enabled = false;
        }
    }

    public void SetBetAmount(int betCount)
    {
        if (betCount != lastBestCount)
        {
            lastBestCount = betCount;
            if (betCount <= betIcons.Length)
            {
                for (int i = 0; i < betIcons.Length; i++)
                {
                    if (betCount >= (i + 1))
                    {
                        betIcons[i].enabled = true;
                    }
                    else
                    {
                        betIcons[i].enabled = false;
                    }

                }
            }
        }        
    }

        
}
