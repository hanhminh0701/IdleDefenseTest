using UnityEngine;

public class SkyEnemy : EnemyController
{
    protected override void Move()
    {
        base.Move();
        Transform.position = Vector2.MoveTowards(Transform.position, _thronePlace.position, _moveSpeed * Time.deltaTime);
        var direction = (_thronePlace.position - Transform.position).normalized;
        Flip(direction.x);
        var distance = Vector2.Distance(_thronePlace.position, Transform.position);
        if (distance <= .1)
        {
            Deactive();
            GameManager.Instance.Throne.TakeDamage(1);
        }
    }

    protected override void Die()
    {
        base.Die();
        Deactive();
    }

    protected override void Deactive()
    {
        base.Deactive();
        GameManager.Instance.SkyEnemies.Enqueue(this);
    }
}
