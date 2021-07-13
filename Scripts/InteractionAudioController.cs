using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace audioScripts
{
    public class InteractionAudioController : MonoBehaviour
    {
        private Camera m_cam;

        private ObjectsOfInterest objectsOfInterest;

        [SerializeField] private LayerMask interactableLayer = ~0;
        public float rayDistance = 0f;
        public float raySphereRadius = 0f;
        public float objectSphereRadius = 0f;

        public AudioSource[] audioSources;

        private bool m_interacting;

        // Start is called before the first frame update
        void Start()
        {
            LayerMask interactableLayer = LayerMask.GetMask("Interactable");
        }
        
        void Awake()
        {
            m_cam = FindObjectOfType<Camera>();
        }

        // Update is called once per frame
        void Update()
        {

            CheckForInteractable();

        }

        void CheckForInteractable()
        {
            Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
            RaycastHit _hitInfo;
            //Debug.Log("Sending out ray!");

            bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer);

            if (_hitSomething)
            {
                m_interacting = true;
                audioSources[0] = _hitInfo.transform.GetComponent<AudioSource>();
                PlaySound();
                Debug.Log("Interactable object found!");

            }
            else
            {
                m_interacting = false;

            }

            Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
        }

        void PlaySound()
        {

            LayerMask Player = LayerMask.GetMask("Player");
            if (Physics.CheckSphere(transform.position, objectSphereRadius, Player) && !audioSources[0].isPlaying)
            {
                //FadeIn(audioSources[0], 5.0f);
                //audioSources[0].UnPause();
                audioSources[0].Play();
                Debug.Log("<color=green> In sphere radius, unpausing audio!</color>");

            }
            else if (!Physics.CheckSphere(transform.position, objectSphereRadius, Player) && audioSources[0].isPlaying)
            {
                //FadeOut(audioSources[0], 2.0f);
                audioSources[0].Pause();
                Debug.Log("<color=red> Out of sphere radius, pausing audio!</color>");

            }
        }

        void FadeIn(AudioSource audioSource, float FadeTime)
        {

            if (audioSource.volume < 1)
            {
                audioSource.volume += 1 * Time.deltaTime / FadeTime;
                //Debug.Log("Sound 1 volume: " + (audioSource.volume).ToString());
            }

        }

        void FadeOut(AudioSource audioSource, float FadeTime)
        {

            if (audioSource.volume > 0)
            {
                audioSource.volume -= 1 * Time.deltaTime / FadeTime;
                //Debug.Log((audioSources[i].volume).ToString());
            }

        }

    }
}

