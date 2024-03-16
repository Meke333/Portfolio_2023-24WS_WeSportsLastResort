using General.SingletonClass;
using UnityEngine.Splines;

namespace Level
{
    public class LevelPathSingleton : SingletonClass<LevelPathSingleton>
    {
        public SplineContainer levelPath;

        public override void Awake()
        {
            base.Awake();

            levelPath = GetComponent<SplineContainer>();
        }
    }
}
