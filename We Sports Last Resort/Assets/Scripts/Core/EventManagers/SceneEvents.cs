using System;
using Unity.VisualScripting;

namespace Core.EventManagers
{
    public class SceneEvents
    {
        public Action<SceneSetups> OnNewSceneSetupLoad;
        public Action<string> OnNewSceneToLoadAdditive;
        public Action<string> OnSingleSceneToUnload;
        public Action<string> OnToggleSceneToLoadOrUnload;
        public Action OnCloseGame;
    }
}
