using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    //objects
    public Animator anim;
    public Rigidbody2D rigBody;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    protected float detectionRange = 7f;
    public bool enemyInRange = false;
    public bool canMove = true;
    public int facingDirection = 0; // direction the character is facing.

    // 0 = right, 1 = down, 2 = left, 3 = up.
    // The last pressed movement direction dictates which way the player character will face.

    public bool hasCollidedWithDecor = false;
    public bool hasEnteredAreaCollider = false;
    public bool hasEnteredDialogueArea = false;

    public int indexNumber; //for player handler
    public bool isUsable = true;
    public bool isInvincible = false;

    //enemy detection
    protected Collider2D[] detectedEnemies;
    public bool targetingOn = false;

    /*//decorative detection
    protected Collider2D[] collideWithDecorative;
    */

    //regular character stats
    // - I wonder if I should make these variables static because they may mot depend on the state of the class
    protected int health = 15;
    protected int attack = 5;
    protected int defense = 2;
    protected int magic = 1;
    protected int critical = 1;
    protected int energy = 1;

    //TODO special stats (make depenndent on regular stats)
    protected float attackRate = 1.5f;
    protected float nextAttackTime = 0f;
    protected float minMvtSpeed = 0f;
    protected float maxMvtSpeed = 3.2f;

    //properties
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }
        set
        {
            defense = value;
        }
    }

    public int Magic
    {
        get
        {
            return magic;
        }
        set
        {
            magic = value;
        }
    }

    public int Critical
    {
        get
        {
            return critical;
        }
        set
        {
            critical = value;
        }
    }

    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
        }
    }

    public float MaxMvtSpd
    {
        get
        {
            return maxMvtSpeed;
        }
    }

    public bool EnemyInRange
    {
        get
        {
            return this.enemyInRange;
        }
    }

    public Collider2D[] DetectedEnemies
    {
        get
        {
            return this.detectedEnemies;
        }
    }


    //***** Utilities *****//

    
    //drawing hurtbox
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    

    //stat change calculation
    public int StatChangeAmountCalc(string statName, float multiplier)
    {
        switch (statName)
        {
            case "health":
                return Mathf.RoundToInt(health * multiplier);
            case "attack":
                return Mathf.RoundToInt(attack * multiplier);
            case "defense":
                return Mathf.RoundToInt(defense * multiplier);
            case "magic":
                return Mathf.RoundToInt(magic * multiplier);
            case "critical":
                return Mathf.RoundToInt(critical * multiplier);
            case "energy":
                return Mathf.RoundToInt(energy * multiplier);
            default:
                return 0;
        }
    }

    //*****Character Actions*****//
    public virtual void BasicAttack()
    {
        Debug.Log("Trying to hit!!");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("hit!");
        }
        return;
    }

    public virtual void Skill0()
    {
        return;
    }

    public virtual void Skill1()
    {
        return;
    }

    public virtual void Deflect()
    {
        return;
    }

    public virtual void OnDeath()
    {
        return;
    }

    public virtual void Dodge()
    {
        return;
    }

    public virtual bool Target()
    {
        Debug.Log("Manually trying to target!");
        detectedEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        if (detectedEnemies.Length == 0) //if we were already targeting something, turn it off
        {
            targetingOn = false;
            return targetingOn;
        }

        if (targetingOn)
        {
            targetingOn = false;
            return targetingOn;
        }

        for (int x = 0; x < detectedEnemies.Length; x++)
        {
            //Debug.Log("Targeted!");
            targetingOn = true;
        }
        //Debug.Log("TargetingOn " + targetingOn);
        return targetingOn;
    }

    public virtual void DetectEnemies()
    {
        detectedEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        if (detectedEnemies.Length == 0)
        {
            enemyInRange = false;
            targetingOn = false;
            return;
        }
        //Debug.Log("In Range...");
        enemyInRange = true;
    }
    
    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "DecorativeCollider")
        {
            //Debug.Log("Decor!");
            hasCollidedWithDecor = true;
        }

        if (collider.tag == "AreaCollider")
        {
            Debug.Log("Next Area Collided!");
        }
    }

    // this will have to do for now :D
    public virtual void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "DialogueAreaCollider")
        {
            Debug.Log("Dialogue Area Collided!");
            hasEnteredDialogueArea = true;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "DecorativeCollider")
        {
            //Debug.Log("No Decor!");
            hasCollidedWithDecor = false;
        }
        if (collider.tag == "DialogueAreaCollider")
        {
            Debug.Log("Dialogue Area Stopped Colliding.");
            hasEnteredDialogueArea = false;
        }
    }

}
