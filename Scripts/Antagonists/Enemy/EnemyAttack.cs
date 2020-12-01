using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

    bool active = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        Light();
    }

    public void Light()
    {
        if (active)
        {
            if (timeBtwAttack <= 0)
            {
                Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < playersToDamage.Length; i++)
                {
                    playersToDamage[i].GetComponent<BeilanHealth>().TakeDamage(damage);
                }
                timeBtwAttack = startTimeBtwAttack;
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    public void Light2()
    {
        if (active)
        {
            InvokeRepeating("Attack", 0, 2f);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            active = true;
            player = col.gameObject;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            active = true;
            player = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        active = false;
        timeBtwAttack = 0;
    }

    void Attack()
    {
        Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < playersToDamage.Length; i++)
        {
            playersToDamage[i].GetComponent<BeilanHealth>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
