using UnityEngine;

public class SlashController : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _lifeTime;
    [SerializeField] float _skillAngle;
    public void Spawn(LayerMask enemyMask, float range)
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        foreach (var enemy in enemies)
        {
            var direction = enemy.transform.position - transform.position;
            if (Vector2.Angle(transform.up, direction) < _skillAngle / 2) enemy.GetComponent<EnemyController>().TakeDamage(_damage);
        }
        Invoke(nameof(Deactive), _lifeTime);
    }
    void Deactive() => gameObject.SetActive(false);
}
