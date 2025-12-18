using System;
using UnityEngine;

public class PlayerEnemyTargetDetector : MonoBehaviour
{
    [SerializeField] private float _detectDistance = 50f;
    [SerializeField] private LayerMask _enemyLayer;

    private EnemyController _enemy;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        DetectEnemy();
    }

    private void DetectEnemy()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, this._detectDistance, this._enemyLayer))
        {
            EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();

            if (enemy != null && enemy.IsAlive)
            {
                if (enemy != this._enemy)
                {
                    this._enemy = enemy;
                    EnemyTargetUI.Instance.Show(enemy);
                }
                return;
            }
        }
        this._enemy = null;
        EnemyTargetUI.Instance.Hide();
    }
}
