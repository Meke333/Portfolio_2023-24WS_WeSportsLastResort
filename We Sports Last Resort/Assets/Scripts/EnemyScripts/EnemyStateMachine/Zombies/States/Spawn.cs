using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Spawn : ZombieStateMachine
    {
        public Spawn(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Spawn";
            _type = ZombieStateEnum.Spawn;
        }
    }
}
