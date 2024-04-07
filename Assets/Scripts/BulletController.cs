using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _damage;
    [SerializeField] float _lifeTime;
    Vector2 _direction;
    bool _active;

    private void OnEnable()
    {
        Invoke(nameof(Deactive), _lifeTime);
        _active = true;
    }

    private void OnDisable() => GameManager.Instance.Bullets.Enqueue(this);

    private void FixedUpdate() => Move();
    public void Move() => transform.Translate(_direction * _speed * Time.fixedDeltaTime);
    public void UpdateDirection(Vector2 direction) => _direction = direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            var enemy = collision.GetComponent<EnemyController>();
            if (enemy.IsDead || !_active) return;
            _active = false;
            enemy.TakeDamage(_damage);
            gameObject.SetActive(false);
            CancelInvoke(nameof(Deactive));
        }
    }

    void Deactive() => gameObject.SetActive(false);
}
