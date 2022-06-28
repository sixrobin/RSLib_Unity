namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    /// <summary>
    /// Used to apply a recoil on a controller.
    /// This class stores recoil data and its current duration, it's meant to be destroyed once done.
    /// </summary>
    public class Recoil
    {
        public Recoil(float direction, RecoilData recoilData)
        {
            Data = recoilData;
            Direction = Mathf.Sign(direction);
            DurationLeft = Data.Duration;
        }

        public RecoilData Data { get; }
        
        public float Direction { get; }
        
        public float DurationLeft { get; private set; }
        
        public bool IsComplete => DurationLeft <= 0f;
        
        /// <summary>
        /// Decreases the recoil time left. Should be called in the Unity Update loop.
        /// </summary>
        public void Update()
        {
            DurationLeft -= Time.deltaTime;
        }
    }
}