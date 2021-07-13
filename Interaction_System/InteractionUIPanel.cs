using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VHS
{        
    public class InteractionUIPanel : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        public bool coolingDown;
        public float waitTime = 60.0f;

        void Update()
        {
            if (coolingDown = true)
            {
                progressBar.fillAmount -= 0.25f / waitTime;
            }
        }

        public void UpdateProgressBar(float fillAmount)
        {
            progressBar.fillAmount += fillAmount / (waitTime *0.5f);

        }

        public void ResetUI()
        {
            progressBar.fillAmount = 0f;
        }
    }
}
