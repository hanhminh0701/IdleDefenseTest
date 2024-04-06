using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEnemy : EnemyController
{
    protected override void Move()
    {
        base.Move();
        var direction = (_thronePlace.position - Transform.position).normalized;
        Transform.Translate(_moveSpeed * direction);
        Flip(direction.x);
    }
}
