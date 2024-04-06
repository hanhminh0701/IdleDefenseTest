using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField] protected float _attackRange;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected float _attackCooldown;
    [SerializeField] protected Transform _attackPoint;
    protected Vector2 _direction;

    EnemyController _enemyTarget;
    float _cooldown;
    Transform Transform => transform;
    
    void Update()
    {
        CheckEnemy();
        CooldownAttack();
    }

    void CheckEnemy()
    {
        if (_enemyTarget == null)
        {
            var enemy = Physics2D.OverlapCircle(Transform.position, _attackRange, _enemyLayer);
            if (enemy != null)
            {
                _enemyTarget = enemy.GetComponent<EnemyController>();
                _enemyTarget.ON_DEAD += RemoveTarget;
            }
        }
        else
        {
            if (_cooldown <= 0 && IsTargetInRange())
            {
                Attack();
                _cooldown = _attackCooldown;
            }
            else if (!IsTargetInRange()) RemoveTarget();
        }
    }

    protected virtual void Attack() => _direction = _enemyTarget.transform.position - Transform.position;

    void CooldownAttack()
    {
        if (_cooldown > 0) _cooldown -= Time.deltaTime;
    }

    bool IsTargetInRange()
    {
        var distance = Vector2.Distance(Transform.position, _enemyTarget.transform.position);
        return distance <= _attackRange;
    }

    void RemoveTarget()
    {
        _enemyTarget.ON_DEAD -= RemoveTarget;
        _enemyTarget = null;
    }
}
