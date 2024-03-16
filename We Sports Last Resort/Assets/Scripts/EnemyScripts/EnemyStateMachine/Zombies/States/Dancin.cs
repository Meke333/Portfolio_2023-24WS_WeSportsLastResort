namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Dancin : ZombieStateMachine
    {

        public Dancin(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Dancin";
            _type = ZombieStateEnum.Dancin;
            
        }
    }
}
