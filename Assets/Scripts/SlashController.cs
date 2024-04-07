using UnityEngine;

public class SlashController : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _lifeTime;
    [SerializeField] float _skillAngle;

    private void Start() => gameObject.SetActive(false);
    public void Spawn(LayerMask enemyMask, float range, Vector2 mainDirection)
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        foreach (var enemy in enemies)
        {
            var direction = enemy.transform.position - transform.position;
            if (Vector2.Angle(mainDirection, direction) < _skillAngle / 2) enemy.GetComponent<EnemyController>().TakeDamage(_damage);
        }
        Invoke(nameof(Deactive), _lifeTime);
    }
    void Deactive() => gameObject.SetActive(false);
}
