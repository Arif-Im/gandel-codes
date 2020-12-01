using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float distance;
    public Animator anim;

    private bool movingRight = true;

    public Transform groundDetection;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        anim.SetBool("isRunning", true);

        int layer_mask = LayerMask.GetMask("Ground");
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, layer_mask);

        if (groundInfo.collider == false)
        {
            Turn();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            Turn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Turn();
    }

    void Turn()
    {
        anim.SetBool("isRunning", false);
        if (movingRight == true)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }
}
