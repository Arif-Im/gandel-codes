using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BeilanAttack : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Attack Times")]
    public float startTimeBtwAttack;
    public float startTimeBtwAttackHeavy;
    public float startTimeBtwAttackSpecial;

    [HideInInspector] public float timeBtwAttack;
    [HideInInspector] public float timeBtwAttackHeavy;
    [HideInInspector] public float timeBtwAttackSpecial;

    [Header("Attack")]
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator anim;
    public float attackRange;
    public int damage;
    public int damageHeavy;
    public int damageSpecial;

    float posX;

    [Header("Knockback")]
    public float knockback;
    public bool lunging = false;

    [Header("Sounds")]
    public AudioSource[] sounds;

    public PlayableDirector director;

    SimpleCameraShakeInCinemachine camShake;

    void Start()
    {
        camShake = FindObjectOfType<SimpleCameraShakeInCinemachine>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if(director == null)
        {
            posX = gameObject.transform.position.x;
            Light();
            Heavy();
            Special();
        }
        else
        {
            if (director.state != PlayState.Playing)
            {
                posX = gameObject.transform.position.x;
                Light();
                Heavy();
                Special();
            }
            else
            {
                return;
            }
        }
    }

    void Light()
    {
        if (timeBtwAttack <= 0 && timeBtwAttackHeavy <= 0 && timeBtwAttackSpecial <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetTrigger("attack");
                Invoke("LightStart", 0.2f);
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void Heavy()
    {
        if (timeBtwAttack <= 0 && timeBtwAttackHeavy <= 0 && timeBtwAttackSpecial <= 0)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("attackHeavy");
                Invoke("HeavyStart", 0.5f);
                timeBtwAttackHeavy = startTimeBtwAttackHeavy;
            }
        }
        else
        {
            timeBtwAttackHeavy -= Time.deltaTime;
        }
    }

    void Special()
    {
        if (timeBtwAttack <= 0 && timeBtwAttackHeavy <= 0 && timeBtwAttackSpecial <= 0)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                rb.AddForce(new Vector2(50 * rb.transform.localScale.normalized.x, 25), ForceMode2D.Impulse);
                anim.SetTrigger("attackSpecial");
                Invoke("SpecialStart", 0.6f);
                timeBtwAttackSpecial = startTimeBtwAttackSpecial;
            }
        }
        else
        {
            timeBtwAttackSpecial -= Time.deltaTime;
        }
    }

    void LightStart()
    {
        sounds[0].Play();
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

    void HeavyStart()
    {
        sounds[1].Play();
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damageHeavy);
        }
    }

    void SpecialStart()
    {
        sounds[2].Play();
        camShake.ShakeCam();
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<KnockEnemy>().Knockback(knockback);
            enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damageSpecial);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
