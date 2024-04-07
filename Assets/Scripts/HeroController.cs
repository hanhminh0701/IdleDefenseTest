using Spine.Unity;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField] protected SkeletonAnimation _anim;
    [SerializeField] protected AnimationReferenceAsset _idle, _attack;
    [SerializeField] protected float _attackRange, _minAttackCooldown, _maxAttackCooldown;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] protected float _attackAnimDuration;
    [SerializeField] protected float _skillDelayTime;

    protected Vector2 _direction;
    protected EnemyController _enemyTarget;

    float _cooldown;
    Transform Transform => transform;

    const string _attackState = "attack";
    const string _idleState = "idle";

    private void Start() => SwitchToState(_idleState);
    void Update()
    {
        CheckEnemy();
        CooldownAttack();
    }

    void SwitchToState(string state)
    {
        if (state == _attackState) SetAnim(_attack, false);
        else SetAnim(_idle, true);
    }
    void SetAnim(AnimationReferenceAsset anim, bool loop, float timeScale = 1) => _anim.state.SetAnimation(0, anim, loop).TimeScale = timeScale;

    void CheckEnemy()
    {
        if (_enemyTarget == null)
        {
            var enemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
            if (enemies.Length > 0)
            {
                _enemyTarget = enemies[0].GetComponent<EnemyController>();
                _enemyTarget.ON_DEACTIVE += RemoveTarget;
            }
        }
        else
        {
            if (!IsTargetInRange()) RemoveTarget();
            else if (_cooldown <= 0)
            {
                Attack();
                SetCooldown();
            }
        }
    }

    protected virtual void Attack()
    {
        _direction = _enemyTarget.Transform.position - Transform.position;
        SwitchToState(_attackState);
        Invoke(nameof(OnFinishAttack), _attackAnimDuration);
    }
    void OnFinishAttack() => SwitchToState(_idleState);

    void CooldownAttack()
    {
        if (_cooldown > 0) _cooldown -= Time.deltaTime;
    }
    void SetCooldown()
    {
        var cooldown = Random.Range(_minAttackCooldown, _maxAttackCooldown);
        _cooldown = cooldown;
    }
    bool IsTargetInRange() => Vector2.Distance(_attackPoint.position, _enemyTarget.Transform.position) <= _attackRange;

    void RemoveTarget()
    {
        _enemyTarget.ON_DEACTIVE -= RemoveTarget;
        _enemyTarget = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
