using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MarchingBytes;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy")]
    public float health;
    public int startHealth;
    public float speed;

    [Header("Sounds")]
    public AudioSource deathSound;
    GameObject deathSoundObj;
    GameObject bloodMB;

    public Image bossHealthBar;

    [SerializeField] GameObject end;

    void Start()
    {
        deathSoundObj = GameObject.FindGameObjectWithTag("Death Sound");
        deathSound = deathSoundObj.GetComponent<AudioSource>();
        health = startHealth;
    }
    
    void Update()
    {
        if (gameObject.CompareTag("Boss"))
        {
            if (health <= 0)
            {
                Instantiate(end, transform.position, transform.rotation);
                deathSound.Play();
                Destroy(gameObject);
                Score.scoreValue += 100;
            }
        }
        else
        {
            if (health <= 0)
            {
                deathSound.Play();
                Destroy(gameObject);
                Score.scoreValue += 100;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (bloodMB == null)
        {
            bloodMB = EasyObjectPool.instance.GetObjectFromPool("Blood", gameObject.transform.position, Quaternion.identity);
        }
        EasyObjectPool.instance.ReturnObjectToPool(bloodMB);
        health -= damage;
        bloodMB = EasyObjectPool.instance.GetObjectFromPool("Blood", gameObject.transform.position, Quaternion.identity);

        if (bossHealthBar == null)
            return;

        bossHealthBar.fillAmount = health / startHealth;
    }
}
