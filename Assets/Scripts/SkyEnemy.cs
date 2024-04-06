using UnityEngine;

public class SkyEnemy : EnemyController
{
    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    GameManager.Instance.SkyEnemies.Enqueue(this);
    //}
    protected override void Move()
    {
        base.Move();
        var direction = (_thronePlace.position - Transform.position).normalized;
        Transform.Translate(_moveSpeed * direction * Time.deltaTime);
        Flip(direction.x);
        var distance = Vector2.Distance(_thronePlace.position, Transform.position);
        if (distance <= .1)
        {
            gameObject.SetActive(false);
            GameManager.Instance.Throne.TakeDamage(1);
        }
    }

    protected override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }
}
