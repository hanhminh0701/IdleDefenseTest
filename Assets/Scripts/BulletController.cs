using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _damage;
    [SerializeField] float _lifeTime;
    Vector2 _direction;

    private void Start() => gameObject.SetActive(false);
    private void OnEnable() => Invoke(nameof(Deactive), 3);

    private void OnDisable() => GameManager.Instance.Bullets.Enqueue(this);

    private void FixedUpdate() => Move();
    public void UpdateDirection(Vector2 direction) => _direction = direction;
    public void Move() => transform.Translate(_direction * _speed * Time.fixedDeltaTime);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }

    void Deactive() => gameObject.SetActive(false);
}
