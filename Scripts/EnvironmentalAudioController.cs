using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Microsoft.Acoustics;

namespace audioScripts
{
    public class EnvironmentalAudioController : MonoBehaviour
    {

        #region Variables
        public GameObject audioListener;
        public LayerMask Player;
        public float maxDistance = 0f;
        public float sphereRadius = 0f;
        private float _objectDistance;

        public GameObject[] environmentalObjects;

        public AudioSource[] audioSources;
        public AcousticsAdjust[] acousticsAdjust;
        public AcousticsAdjustExperimental[] acousticsAdjustExperimental;

        public AudioMixer environmentalMixer;
        public AudioMixerSnapshot[] forwardSnapshots;
        public AudioMixerSnapshot[] backwardSnapshots;
        public float mixerThreshold = 50;
        public float[] weights;
        #endregion

        // Start is called before the first frame update
        void Start()
        {

            environmentalObjects = GameObject.FindGameObjectsWithTag("Environmental");

            foreach (GameObject enviroObject in environmentalObjects)
            {
                audioSources = enviroObject.GetComponents<AudioSource>();
                acousticsAdjust = enviroObject.GetComponents<AcousticsAdjust>();
                acousticsAdjustExperimental = enviroObject.GetComponents<AcousticsAdjustExperimental>();

            }


        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit outInfo;
            LayerMask Player = LayerMask.GetMask("Player");
            Vector3 objectVector = audioListener.transform.position - transform.position;

            bool hit = Physics.Raycast(transform.position, objectVector, out outInfo, maxDistance, Player);

            Debug.DrawRay(transform.position, objectVector, hit ? Color.green : Color.red);

            if (hit)
            {
                _objectDistance = getDistance(outInfo);

                if (_objectDistance < 2.0f)
                {
                    float updateAmount = 1.0f * Time.deltaTime;

                    foreach (AcousticsAdjustExperimental _acousticsAdjustExp in acousticsAdjustExperimental)
                    {
                        _acousticsAdjustExp.IncreasePerceptualDistanceWarp(updateAmount);
                        //Debug.Log("Increasing Distance Warp!");
                    }

                    BlendSnapshots(_objectDistance);

                } else
                {
                    float updateAmount = 0.25f * Time.deltaTime;

                    foreach (AcousticsAdjustExperimental _acousticsAdjustExp in acousticsAdjustExperimental)
                    {
                        _acousticsAdjustExp.DecreasePerceptualDistanceWarp(updateAmount);
                        //Debug.Log("Decreasing Distance Warp!");

                    }
                    unBlendSnapshots(_objectDistance);

                }
            }

        }


        float getDistance(RaycastHit outInfo)
        {

            float objectDistance = outInfo.distance;

            return objectDistance;


        }

        public void BlendSnapshots(float distance)
        {

            //Debug.Log("Blending snapshots forward!");

            weights[0] = distance;
            weights[1] = mixerThreshold * (2 * distance);
            environmentalMixer.TransitionToSnapshots(forwardSnapshots, weights, 0.2f);

        }

        public void unBlendSnapshots(float distance)
        {

            //Debug.Log("Blending snapshots backward!");

            weights[0] = distance;
            weights[1] = mixerThreshold / distance;
            environmentalMixer.TransitionToSnapshots(backwardSnapshots, weights, 0.2f);

        }

    }

}

