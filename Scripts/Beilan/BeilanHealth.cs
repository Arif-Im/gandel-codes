using System.Collections;
using UnityEngine;
using MarchingBytes;
using UnityEngine.UI;

public class BeilanHealth : MonoBehaviour
{
    [Header("Enemy")]
    public LayerMask whatIsEnemies;
    public GameObject enemy;
    public Animator anim;
    public bool isBlocking = false;

    [Header("Particle Effects")]
    public GameObject blood;

    [Header("Health")]
    public int startHealth;

    [Header("Sounds")]
    public AudioSource[] sounds;

    [HideInInspector]
    public float health;

    Renderer rend;
    Color c;

    GameObject bloodMB;

    [Header("Unity Stuff")]
    public Image healthBar;

    BeilanAttack ba;

    private void Start()
    {
        ba = GetComponent<BeilanAttack>();
        Physics2D.IgnoreLayerCollision(8, 10, false);
        rend = GetComponent<Renderer>();
        c = rend.material.color;
        health = startHealth;
    }

    void Update()
    {
        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            FindObjectOfType<GameManager>().EndGame();
        }
        Block();
    }

    void Block()
    {
        if (ba.timeBtwAttack <= 0 && ba.timeBtwAttackHeavy <= 0 && ba.timeBtwAttackSpecial <= 0)
        {
            if (Input.GetKey(KeyCode.I))
            {
                anim.SetBool("isBlocking", true);
                isBlocking = true;
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                anim.SetBool("isBlocking", false);
                isBlocking = false;
            }
        }
        else
        {
            anim.SetBool("isBlocking", false);
            isBlocking = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isBlocking == false)
        {
            if (bloodMB == null)
            {
                bloodMB = EasyObjectPool.instance.GetObjectFromPool("Blood", gameObject.transform.position, Quaternion.identity);
            }
            EasyObjectPool.instance.ReturnObjectToPool(bloodMB);
            sounds[0].Play();
            health -= damage;
            healthBar.fillAmount = health / startHealth;
            bloodMB = EasyObjectPool.instance.GetObjectFromPool("Blood", gameObject.transform.position, Quaternion.identity);
            StartCoroutine("GetInvulnerable");
        }
        else
        {
            health = GetComponent<BeilanHealth>().health;
        }
    }

    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(8, 10, true);
        c.a = 0.5f;
        rend.material.color = c;
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreLayerCollision(8, 10, false);
        c.a = 1f;
        rend.material.color = c;
    }
}
