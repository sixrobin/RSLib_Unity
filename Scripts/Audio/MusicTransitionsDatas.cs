namespace RSLib.Audio
{
    using RSLib.CSharp.Maths;
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Music Transition", menuName = "RSLib/Audio/Music Transition")]
    public class MusicTransitionsDatas : ScriptableObject
    {
        [SerializeField, Min(0f)]
        private float _duration = 1f;
        [SerializeField]
        private Curve _curveOut = Curve.InOutSine;
        [SerializeField]
        private Curve _curveIn = Curve.InOutSine;
        [SerializeField]
        private bool _crossFade = true;

        public static MusicTransitionsDatas Default
        {
            get
            {
                MusicTransitionsDatas transitionDatas = CreateInstance<MusicTransitionsDatas>();
                transitionDatas._duration = 1f;
                transitionDatas._curveOut = Curve.InOutSine;
                transitionDatas._curveIn = Curve.InOutSine;
                transitionDatas._crossFade = true;

                return transitionDatas;
            }
        }

        public static MusicTransitionsDatas Instantaneous
        {
            get
            {
                MusicTransitionsDatas transitionDatas = CreateInstance<MusicTransitionsDatas>();
                transitionDatas._duration = 0f;
                transitionDatas._curveOut = Curve.Linear;
                transitionDatas._curveIn = Curve.Linear;
                transitionDatas._crossFade = true;

                return transitionDatas;
            }
        }

        public float Duration => _duration;
        public Curve CurveOut => _curveOut;
        public Curve CurveIn => _curveIn;
        public bool CrossFade => _crossFade;
    }
}