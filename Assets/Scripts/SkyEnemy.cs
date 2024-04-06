using UnityEngine;

public class SkyEnemy : EnemyController
{
    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.SkyEnemies.Enqueue(this);
    }
    protected override void Move()
    {
        base.Move();
        var direction = (_thronePlace.position - Transform.position).normalized;
        Transform.Translate(_moveSpeed * direction);
        Flip(direction.x);
    }

    protected override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }
}
