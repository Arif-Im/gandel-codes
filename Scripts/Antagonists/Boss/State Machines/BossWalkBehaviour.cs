using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkBehaviour : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;

    private Transform playerPos;
    public float speed;

    public bool facingRight;
    bool oneTime = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            timer -= Time.deltaTime;
        }

        Vector2 localScale = animator.transform.localScale;
        Vector2 target = new Vector2(playerPos.position.x, animator.transform.position.y);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);

        if (playerPos.position.x > animator.transform.position.x && facingRight)
        {
            if (!oneTime)
            {
                localScale.x *= -1;
                animator.transform.localScale = localScale;
                facingRight = false;
                oneTime = true;
            }
            else
            {
                localScale.x *= -1;
                animator.transform.localScale = localScale;
                facingRight = true;
                oneTime = false;
            }
        }
        else if (playerPos.position.x < animator.transform.position.x && !facingRight)
        {
            if (oneTime)
            {
                localScale.x *= -1;
                animator.transform.localScale = localScale;
                facingRight = true;
                oneTime = false;
            }
            else
            {
                localScale.x *= -1;
                animator.transform.localScale = localScale;
                facingRight = false;
                oneTime = true;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
