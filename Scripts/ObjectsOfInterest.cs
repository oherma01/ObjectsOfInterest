using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace audioScripts
{
    public class ObjectsOfInterest : MonoBehaviour
    {
        public bool requireFieldOfView = true;
        public GameObject Camera;
        public float verticalFieldOfView = 30;
        public float horizontalFieldOfView = 60;
        public float maximumInteractiveDistance = 5f;
        public LayerMask interactiveLayers;
        private AudioSource[] audioSources;

        // Start is called before the first frame update
        void Start()
        {
            LayerMask interactiveLayers = LayerMask.GetMask("Interactable");
        }

        // Update is called once per frame
        void Update()
        {
            List<GameObject> interactiveObjects = findInteractiveObjects();
            //printInteractiveObjects(interactiveObjects);
            audioSources = InteractiveObjectsAudio(interactiveObjects);
            FadeInMultiple(audioSources, 10.0f);
        }

        public List<GameObject> findInteractiveObjects()
        {
            Vector3 distance, adj, vHyp, hHyp;
            float hAngle, vAngle;
            List<GameObject> interactiveObjects = new List<GameObject>();
            List<GameObject> noninteractiveObjects = new List<GameObject>();

            // Determine Camera View
            Transform FOV;
            if (Camera != null)
                FOV = Camera.transform;
            else FOV = transform;

            // Find current colliders
            Collider[] proximityObjects = Physics.OverlapSphere(FOV.position, maximumInteractiveDistance, interactiveLayers.value);
            foreach (Collider col in proximityObjects)
            {
                distance = col.transform.position - FOV.position;
                adj = Vector3.Dot(distance, FOV.forward) * FOV.forward;

                vHyp = distance - (Vector3.Dot(distance, FOV.right) * FOV.right);
                vAngle = Mathf.Rad2Deg * Mathf.Acos(adj.magnitude / vHyp.magnitude);

                hHyp = distance - (Vector3.Dot(distance, FOV.up) * FOV.up); ;
                hAngle = Mathf.Rad2Deg * Mathf.Acos(adj.magnitude / hHyp.magnitude); ;

                //Debug.Log("Vertical angle: " + vAngle);
                //Debug.Log("Horizontal angle: " + hAngle);

                //Ensure they are in the Field of View
                if ((hAngle <= horizontalFieldOfView || vAngle <= verticalFieldOfView) || !requireFieldOfView)
                {
                    GameObject interactiveObj = col.gameObject;

                    if (interactiveObj != null)
                        interactiveObjects.Add(interactiveObj);
                    noninteractiveObjects.Remove(interactiveObj);
                }
                else
                {
                    GameObject interactiveObj = col.gameObject;

                    interactiveObjects.Remove(interactiveObj);
                    noninteractiveObjects.Add(interactiveObj);

                    FadeOutMultiple(InteractiveObjectsAudio(noninteractiveObjects), 10.0f);
                }
            }

            return interactiveObjects;
        }

        //Print a list of all the interactive objects in view
        public void printInteractiveObjects(List<GameObject> interactiveObjects)
        {
            foreach (GameObject gameObject in interactiveObjects)
            {
                Debug.Log("Object Tag: " + gameObject.tag);
            }

        }

        //Return an array with all of the audio sources of the interactive objects in view
        public AudioSource[] InteractiveObjectsAudio(List<GameObject> interactiveObjects)
        {

            foreach (GameObject gameObject in interactiveObjects)
            {
                audioSources = gameObject.GetComponents<AudioSource>();
            }

            return audioSources;
        }

        //Fades in all of the audio sources that have been enabled by being in view
        public void FadeInMultiple(AudioSource[] audioSources, float FadeTime)
        {

            if (audioSources != null && audioSources.Length != 0)
            {
                for (int i = 0; i < audioSources.Length; i++)
                {

                    if (audioSources[i].isPlaying == true && audioSources[i].volume < 0.6)
                    {
                        //audioSources[i].UnPause();
                        //audioSources[i].Play();

                        audioSources[i].volume += 1 * Time.deltaTime / FadeTime;
                        Debug.Log("<color=green>Fading In! </color>" + audioSources[i].gameObject.name + (audioSources[i].volume).ToString());

                    }

                }

            }

            //Debug.Log("Sound 1 volume: " + audioSources[0].volume);
            //Debug.Log("Sound 2 volume: " + audioSources[1].volume);

        }

        public void FadeOutMultiple(AudioSource[] audioSources, float FadeTime)
        {

            for (int i = 0; i < audioSources.Length; i++)
            {

                if (audioSources[i].volume > 0)
                {
                    audioSources[i].volume -= 2 * Time.deltaTime / FadeTime;
                    Debug.Log("<color=red>Fading Out! </color>" + audioSources[i].gameObject.name + (audioSources[i].volume).ToString());
                }

            }

        }

    }
}





