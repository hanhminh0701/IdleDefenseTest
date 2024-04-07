using UnityEngine;

public class GroundEnemy : EnemyController
{
    int _pathTarget;
    public override void Respawn(Vector2 position)
    {
        base.Respawn(position);
        _pathTarget = 0;
        GetComponent<Collider2D>().enabled = true;
    }
    protected override void Move()
    {
        base.Move();
        Transform.position = Vector2.MoveTowards(Transform.position, _groundPath[_pathTarget].position, _moveSpeed * Time.deltaTime);
        var direction = (_groundPath[_pathTarget].position - Transform.position).normalized;
        Flip(direction.x);

        var distance = Vector2.Distance(Transform.position, _groundPath[_pathTarget].position);
        if (distance <= .1) _pathTarget++;
        if (_pathTarget == _groundPath.Length)
        {
            Deactive();
            GameManager.Instance.Throne.TakeDamage(1);
        }
    }

    protected override void Die()
    {
        base.Die();
        Invoke(nameof(Deactive), 2);
        GetComponent<Collider2D>().enabled = false;
        AudioManager.Instance.PlaySFX(SFXType.GroundEnemy);
    }
    protected override void Deactive()
    {
        base.Deactive();
        GameManager.Instance.GroundEnemies.Enqueue(this);
    }
}
