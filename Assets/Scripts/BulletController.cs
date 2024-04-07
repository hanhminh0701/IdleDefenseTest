using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _damage;
    [SerializeField] float _lifeTime;

    Vector2 _direction;
    Transform _target;
    bool _active;

    void OnEnable()
    {
        //Invoke(nameof(Deactive), _lifeTime);
        _active = true;
    }

    void OnDisable() => GameManager.Instance.Bullets.Enqueue(this);

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, _target.position) < .1) Deactive();
    }
    public void Move() => transform.Translate(_direction * _speed * Time.fixedDeltaTime);
    public void UpdateDirection(Vector2 direction) => _direction = direction;
    public void UpdateTarget(Transform target) => _target = target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            var enemy = collision.GetComponent<EnemyController>();
            if (enemy.IsDead || !_active) return;
            _active = false;
            enemy.TakeDamage(_damage);
            Deactive();
            CancelInvoke(nameof(Deactive));
        }
    }

    void Deactive() => gameObject.SetActive(false);
}
