namespace RSLib.Audio
{
    public abstract class AudioPlaylist : UnityEngine.ScriptableObject
    {
        public abstract void Init();
        public abstract AudioClipPlayDatas GetNextClipDatas();
    }
}