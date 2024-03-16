using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class LoadSceneSetups : ScriptableObject
    {
        public SceneInformation[] scenes;
    }

    [System.Serializable]
    public struct SceneInformation
    {
        public string path;
        public bool isLoaded;
        public bool isActive;
        public bool isSubScene;

    }
}