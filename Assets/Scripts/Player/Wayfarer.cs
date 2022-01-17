using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wayfarer : BaseCharacter
{
    public Wayfarer()
    {
        this.maxMvtSpeed = 3.4f;
    }

    public override void BasicAttack()
    {
        Debug.Log("Trying to hit!!");

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("WayfarerBasicAttack1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("WayfarerB1Up") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("WayfarerB1Down") ||
            anim.GetAnimatorTransitionInfo(0).IsName("WayfarerBasicAttack1 -> WayfarerIdle") ||
            anim.GetAnimatorTransitionInfo(0).IsName("WayfarerB1Up -> WayfarerIdleUp") ||
            anim.GetAnimatorTransitionInfo(0).IsName("WayfarerB1Down -> WayfarerIdleDown"))
        {
            anim.SetTrigger("basicAttack2");
        }
        else
        {
            anim.SetTrigger("basicAttack1");
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        for (int x = 0; x < hitEnemies.Length; x++)
        {
            Debug.Log("hit!");
        }
        return;
    }

    public override void Skill0()
    {
        anim.SetTrigger("skill0");
        //Debug.Log("skill0 triggered!");
        return;
    }

    public override void Skill1()
    {
        base.Skill1();
    }

    public override void OnDeath()
    {
        this.isUsable = false;
    }
}
