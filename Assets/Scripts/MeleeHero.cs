using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHero : HeroController
{
    [SerializeField] SlashController _slash;
    protected override void Attack()
    {
        base.Attack();
    }
}
