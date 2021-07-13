using UnityEngine;

namespace VHS
{
    [CreateAssetMenu(fileName = "HeadBobData", menuName = "FirstPersonController/Data/HeadBobData", order = 3)]
    public class HeadBobData : ScriptableObject
    {
        #region Variables    
            public AnimationCurve xCurve;
            public AnimationCurve yCurve;

            public float xAmplitude;
            public float yAmplitude;

            public float xFrequency;
            public float yFrequency;

            public float runAmplitudeMultiplier;
            public float runFrequencyMultiplier;

            public float crouchAmplitudeMultiplier;
            public float crouchFrequencyMultiplier;
        #endregion

        #region Properties
            public float MoveBackwardsFrequencyMultiplier {get;set;}
            public float MoveSideFrequencyMultiplier {get;set;}
        #endregion
    }
}
