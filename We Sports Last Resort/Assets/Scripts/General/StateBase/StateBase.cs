using UnityEngine;

namespace General.StateBase
{
    [System.Serializable]
    public class StateBase
    {
        public enum EVENT
        {
            ENTER,
            UPDATE,
            EXIT,
        }

        public string stateName { get; protected set; } 

        protected EVENT stage;
        
        protected StateBase nextState;

        public StateBase()
        {
            
        }

        public virtual void Enter()
        {
            stage = EVENT.UPDATE;
            Debug.Log("StateName: " + stateName);
        }

        public virtual void Update()
        {
            stage = EVENT.UPDATE;
        }

        public virtual void Exit()
        {
            stage = EVENT.EXIT;
        }

        public StateBase Process()
        {
            switch (stage)
            {
                case EVENT.ENTER:
                    Enter();
                    break;
                case EVENT.UPDATE:
                    Update();
                    break;
                case EVENT.EXIT:
                    Exit();
                    return nextState;
            }

            return this;
        }
    }
    
}
