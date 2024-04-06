using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHero : HeroController
{
    [SerializeField] SlashController _slash;
    [SerializeField] float _skillDelayTime;
    protected override void Attack()
    {
        base.Attack();
       StartCoroutine(SpawnSkill(_enemyLayer, _attackRange));
    }

    IEnumerator SpawnSkill(LayerMask enemy, float range)
    {
        yield return new WaitForSeconds(_skillDelayTime);
        _slash.transform.position = _attackPoint.position;
        _slash.transform.up = _direction;
        _slash.Spawn(enemy, range);
    }
}
