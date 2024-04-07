using System.Collections;
using UnityEngine;

public class MeleeHero : HeroController
{
    [SerializeField] SlashController _slash;
    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(SpawnSkill(_enemyLayer, _attackRange));
        AudioManager.Instance.PlaySFX(SFXType.Slash);
    }

    IEnumerator SpawnSkill(LayerMask enemy, float range)
    {
        yield return new WaitForSeconds(_skillDelayTime);
        _slash.transform.position = _attackPoint.position;
        _slash.gameObject.SetActive(true);
        _slash.Spawn(enemy, range, _direction);
    }
}
