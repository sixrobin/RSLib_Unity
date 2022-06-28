namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "New Recoil Data", menuName = "RSLib/2D Platformer/Recoil Data")]
    public class RecoilData : ScriptableObject
    {
        [SerializeField] private float _force = 10f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private bool _checkEdge = true;
        [SerializeField] private float _airborneMultiplier = 1f;

        public float Force => _force;
        public float Duration => _duration;
        public bool CheckEdge => _checkEdge;
        public float AirborneMultiplier => _airborneMultiplier;
        
        public static RecoilData Default => Create(10f, 0.5f, true, 1f);

        public static RecoilData Create(float force, float duration, bool checkEdge, float airborneMultiplier)
        {
            RecoilData recoilData = CreateInstance<RecoilData>();
            recoilData._force = force;
            recoilData._duration = duration;
            recoilData._checkEdge = checkEdge;
            recoilData._airborneMultiplier = airborneMultiplier;
            
            return recoilData;
        }
    }
}