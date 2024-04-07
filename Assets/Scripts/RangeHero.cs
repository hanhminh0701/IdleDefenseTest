using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHero : HeroController
{
    [SerializeField] BulletController _bulletPrefabs;
    Queue<BulletController> _bullets => GameManager.Instance.Bullets;
    protected override void Attack()
    {
        base.Attack();
        //Invoke(nameof(FireBullet), _skillDelayTime);
        StartCoroutine(FireBullet(_enemyTarget.transform));
    }
    void FireBullet()
    {
        BulletController bullet;
        if (_bullets.Count > 0) bullet = _bullets.Dequeue();
        else bullet = Instantiate(_bulletPrefabs, GameManager.Instance.BulletPool);
        bullet.transform.position = _attackPoint.position;
        bullet.UpdateDirection(_direction);
        bullet.gameObject.SetActive(true);
    }

    IEnumerator FireBullet(Transform target)
    {
        yield return new WaitForSeconds(_skillDelayTime);
        BulletController bullet;
        if (_bullets.Count > 0) bullet = _bullets.Dequeue();
        else bullet = Instantiate(_bulletPrefabs, GameManager.Instance.BulletPool);
        bullet.transform.position = _attackPoint.position;
        bullet.UpdateTarget(target);
        bullet.gameObject.SetActive(true);
    }
}
