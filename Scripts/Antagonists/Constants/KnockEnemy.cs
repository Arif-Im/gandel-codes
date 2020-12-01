using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEnemy : MonoBehaviour
{
    GameObject beilan;
    Rigidbody2D rb;
    BeilanAttack ba;

    // Start is called before the first frame update
    void Start()
    {
        beilan = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        ba = beilan.GetComponent<BeilanAttack>();
    }

    public void Knockback(float knockback)
    {
        Vector2 difference = rb.transform.position - beilan.transform.position;
        difference = difference.normalized * knockback;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }
}
