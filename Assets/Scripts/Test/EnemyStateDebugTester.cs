using UnityEngine;

public class EnemyStateDebugTester : MonoBehaviour
{
    private EnemyController[] enemies;

    private void Start()
    {
        enemies = FindObjectsOfType<EnemyController>();
    }

    private void Update()
    {
        if (enemies == null || enemies.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (var e in enemies)
                e.stateMachine.ChangeState(e.MoveState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var e in enemies)
                e.stateMachine.ChangeState(e.AttackState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (var e in enemies)
                e.stateMachine.ChangeState(e.HurtState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (var e in enemies)
                e.stateMachine.ChangeState(e.DeathState);
        }
    }
}
