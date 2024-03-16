namespace EnemyScripts.EnemyStateMachine.Zombies
{
    public enum ZombieStateEnum
    {
        None,
        Spawn,
        Standing,
        FollowPlayer,
        Attacking,
        Stunned,
        Hurt,
        Dead,
        Dancin
    }

    public enum ZombieGuardDirection
    {
        None,
        Up,
        Down,
        Left,
        Right,
        EveryDirection
    }
}
