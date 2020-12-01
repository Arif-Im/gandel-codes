using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MarchingBytes;

public class ShyTanAttack : MonoBehaviour
{
    [Header("Boss")]
    Rigidbody2D rb;
    bool facingRight;

    [Header("Attack Times")]
    public float startTimeBtwAttack;
    float timeBtwAttack;

    [Header("Attack")]
    public Transform attackPos;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;
    public Animator anim;
    float distance;
    public GameObject debrisPos;

    [Header("JumpAttack")]
    public float jumpLength;
    public float jumpHeight;
    public int jumpDamage;

    [Header("Particle Systems")]
    public ParticleSystem debris;

    [Header("Sounds")]
    public AudioSource[] sounds;

    GameObject player;
    GameObject debrisMB;

    public CutsceneActivator cutscene;
    SimpleCameraShakeInCinemachine camShake;

    int chooseState;

    // Start is called before the first frame update
    void Start()
    {
        camShake = FindObjectOfType<SimpleCameraShakeInCinemachine>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (cutscene.startPlaying == false)
        {
            return;
        }
        else
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < 3f)
            {
                ChooseState();
            }
        }
    }

    void ChooseState()
    {
        chooseState = Random.Range(0, 2);

        if (chooseState == 0)
        {
            StartAttack();
        }
        else if (chooseState == 1)
        {
            StartJumpAttack();
        }
    }

    void StartAttack()
    {
        if (timeBtwAttack <= 0)
        {
            anim.SetTrigger("Attack");
            //Invoke("Attack", 0.6f);
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            anim.SetTrigger("Idle");
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<BeilanHealth>().TakeDamage(damage);
        }

        if (debrisMB == null)
        {
            debrisMB = EasyObjectPool.instance.GetObjectFromPool("Debris", gameObject.transform.position, Quaternion.identity);
        }
        EasyObjectPool.instance.ReturnObjectToPool(debrisMB);
        debrisMB = EasyObjectPool.instance.GetObjectFromPool("Debris", debrisPos.transform.position, Quaternion.Euler(-90, -90, -90));

        sounds[0].Play();
        camShake.ShakeCam();
    }

    void StartJumpAttack()
    {
        Vector2 difference = rb.transform.position - player.transform.position;

        if (timeBtwAttack <= 0)
        {
            anim.SetTrigger("JumpAttackStart");
            rb.velocity = new Vector2(-difference.x * jumpLength, jumpHeight);
            Invoke("JumpAnimEnd", 0.6f);
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            anim.SetTrigger("Idle");
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void JumpAnimEnd()
    {
        anim.SetTrigger("JumpAttackEnd");
    }

    public void JumpAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<BeilanHealth>().TakeDamage(jumpDamage);
        }

        if (debrisMB == null)
        {
            debrisMB = EasyObjectPool.instance.GetObjectFromPool("Debris", gameObject.transform.position, Quaternion.identity);
        }
        EasyObjectPool.instance.ReturnObjectToPool(debrisMB);
        debrisMB = EasyObjectPool.instance.GetObjectFromPool("Debris", debrisPos.transform.position, Quaternion.Euler(-90, -90, -90));

        sounds[1].Play();
        camShake.ShakeCam();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
