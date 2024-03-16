//using Core.CategorizedEventManager;

using Core.EventManagers;
using General.SingletonClass;

namespace Core
{
    public class CoreEventManager : SingletonClass<CoreEventManager>
    {

        public UIEvents UIEvents;
        public GameEvents GameEvents;
        public AudioEvents AudioEvents;
        public SceneEvents SceneEvents;
        
        public override void Awake()
        {
            base.Awake();

            UIEvents = new UIEvents();
            GameEvents = new GameEvents();
            AudioEvents = new AudioEvents();
            SceneEvents = new SceneEvents();

        }
    }
}