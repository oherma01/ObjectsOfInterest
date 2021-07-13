using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Variables    
            

            [SerializeField] private bool holdInteract = true;
            [SerializeField] private float holdDuration = 4f;
            
            [SerializeField] private bool isInteractable = true;
        #endregion

        #region Properties    
            public float HoldDuration => holdDuration; 

            public bool HoldInteract => holdInteract;

            public bool IsInteractable => isInteractable;
        #endregion

        #region Methods
        public virtual void OnInteract()
            {
                //Debug.Log("INTERACTED: " + gameObject.name);
            }
        #endregion
    }
}
