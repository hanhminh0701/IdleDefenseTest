using UnityEngine;

public class GroundEnemy : EnemyController
{
    int _pathTarget;
    protected override void OnEnable()
    {
        base.OnEnable();
        _pathTarget = 0;
    }
    protected override void Move()
    {
        base.Move();
        var direction = (_groundPath[_pathTarget].position - Transform.position).normalized;
        Transform.Translate(_moveSpeed * direction);
        Flip(direction.x);

        var distance = Vector2.Distance(Transform.position, _groundPath[_pathTarget].position);
        if (distance <= .1) _pathTarget++;
    }
}
