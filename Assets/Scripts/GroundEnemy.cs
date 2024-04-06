using UnityEngine;

public class GroundEnemy : EnemyController
{
    int _pathTarget;
    protected override void OnEnable()
    {
        base.OnEnable();
        _pathTarget = 0;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.GroundEnemies.Enqueue(this);
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

    protected override void Die()
    {
        base.Die();
        Invoke(nameof(Deactive), 2);
    }
    void Deactive() => gameObject.SetActive(false);
}
