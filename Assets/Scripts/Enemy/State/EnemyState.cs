public abstract class EnemyState
{
    protected EnemyController _enemy;
    protected EnemyStateMachine _state;

    protected EnemyState(EnemyController enemy, EnemyStateMachine state)
    {
        this._enemy = enemy;
        this._state = state;
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }
}
