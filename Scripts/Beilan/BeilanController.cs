using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MarchingBytes;
using UnityEngine.Playables;

public class BeilanController : MonoBehaviour
{
    [Header("Position")]
    public GameObject beilan;
    public Rigidbody2D rb;
    public Animator anim;
    public bool facingRight = true;
    public int playerSpeed = 10;
    private int facingDirection = 1;

    [Header("Scripts")]
    BeilanAttack ba;
    BeilanHealth bh;

    [Header("Jump")]
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    public float wallHopForce;
    bool isWallJumping;
    float iniWallCheckDistance;
    Vector2 forceToAdd;

    [Header("Control")]
    public float fJumpPressedRememberTime = 0.2f;
    public float fGroundedRememberTime = 0.2f;
    public float fJumpVelocity = 1f;
    float fJumpPressedRemember;
    float fGroundedRemember;

    [Range(0.0f, 1.0f)]
    public float fHorizontalDampingWhenStopping;

    [Range(0.0f, 1.0f)]
    public float fHorizontalDampingWhenTurning;

    [Range(0.0f, 1.0f)]
    public float fHorizontalDampingBasic;

    [Range(0.0f, 1.0f)]
    public float fCutJumpHeight;

    [Header("WallSlide")]
    public bool isWallSliding;
    public bool isTouchingWall;
    public Transform wallCheck;
    public float wallCheckDistance;
    public float wallSlideSpeed;

    [Header("Ground")]
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    [Header("Water")]
    bool inWater = false;

    [Header("Pray")]
    public bool praying = false;
    public LayerMask whatIsPray;
    float startRestoreTime = 1f;
    float restoreTime;

    [Header("Particle Systems")]
    public ParticleSystem dust;

    [Header("Sounds")]
    public AudioSource[] sounds;

    bool bGrounded;
    bool canPray;
    bool oneTime = false;
    GameObject dustMB;

    public PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        iniWallCheckDistance = wallCheckDistance;
        rb = GetComponent<Rigidbody2D>();
        ba = GetComponent<BeilanAttack>();
        bh = GetComponent<BeilanHealth>();
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }
    
    void Update()
    {
        if(director == null)
        {
            Control();
            Pray();
            Swim();
            WallSlide();
            WallJump();
        }
        else
        {
            if (director.state != PlayState.Playing)
            {
                Control();
                Pray();
                Swim();
                WallSlide();
                WallJump();
            }
            else
            {
                return;
            }
        }
    }

    void Control()
    {
        Vector2 v2GroundedBoxCheckPosition = (Vector2)transform.position + new Vector2(0, -0.25f);
        Vector2 v2GroundedBoxCheckScale = (Vector2)transform.localScale + new Vector2(-0.5f, 0);
        bGrounded = Physics2D.OverlapBox(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, whatIsGround);

        if (praying || isWallSliding)
        {
            if (bGrounded)
            {
                isWallSliding = false;
            }
            else
            {
                return;
            }
        }
        else
        {
            fGroundedRemember -= Time.deltaTime;
            if (bGrounded || canPray || inWater)
            {
                fGroundedRemember = fGroundedRememberTime;
                anim.SetBool("isJumping", false);
            }
            else
            {
                anim.SetBool("isJumping", true);
            }
            
            fJumpPressedRemember -= Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                fJumpPressedRemember = fJumpPressedRememberTime;
                sounds[0].Play();
                anim.SetTrigger("TakeOff");
            }

            if (Input.GetButtonUp("Jump"))
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);
                }
            }

            if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
            {
                fJumpPressedRemember = 0;
                fGroundedRemember = 0;
                rb.velocity = new Vector2(rb.velocity.x, fJumpVelocity);
            }

            float fHorizontalVelocity = rb.velocity.x;
            fHorizontalVelocity += Input.GetAxisRaw("Horizontal");

            if (Input.GetAxisRaw("Horizontal") < 0 && facingRight == true)
            {
                FlipPlayer();
            }
            else if (Input.GetAxisRaw("Horizontal") > 0 && facingRight == false)
            {
                FlipPlayer();
            }

            if (Input.GetAxisRaw("Horizontal") < 0 && bGrounded)
            {
                if (!oneTime)
                {
                    if (dustMB == null)
                    {
                        dustMB = EasyObjectPool.instance.GetObjectFromPool("Dust", feetPos.transform.position, Quaternion.Euler(-90 + (70 * transform.localScale.normalized.x), -90, -90));
                    }
                    EasyObjectPool.instance.ReturnObjectToPool(dustMB);
                    dustMB = EasyObjectPool.instance.GetObjectFromPool("Dust", feetPos.transform.position, Quaternion.Euler(-90 + (70 * transform.localScale.normalized.x), -90, -90));
                    oneTime = true;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0 && bGrounded)
            {
                if (!oneTime)
                {
                    if (dustMB == null)
                    {
                        dustMB = EasyObjectPool.instance.GetObjectFromPool("Dust", feetPos.transform.position, Quaternion.Euler(-90 + (70 * transform.localScale.normalized.x), -90, -90));
                    }
                    EasyObjectPool.instance.ReturnObjectToPool(dustMB);
                    dustMB = EasyObjectPool.instance.GetObjectFromPool("Dust", feetPos.transform.position, Quaternion.Euler(-90 + (70 * transform.localScale.normalized.x), -90, -90));
                    oneTime = true;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") == 0 && bGrounded)
            {
                oneTime = false;
            }

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
            {
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenStopping, Time.deltaTime * 10f);
                anim.SetBool("isRunning", false);
            }
            else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(fHorizontalVelocity))
            {
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingWhenTurning, Time.deltaTime * 10f);
                anim.SetBool("isRunning", true);
            }
            else
            {
                fHorizontalVelocity *= Mathf.Pow(1f - fHorizontalDampingBasic, Time.deltaTime * 10f);
                anim.SetBool("isRunning", true);
            }

            rb.velocity = new Vector2(fHorizontalVelocity, rb.velocity.y);
        }
    }

    void Pray()
    {
        Vector2 v2GroundedBoxCheckPosition = (Vector2)transform.position + new Vector2(0, -0.25f);
        Vector2 v2GroundedBoxCheckScale = (Vector2)transform.localScale + new Vector2(-0.02f, 0);
        canPray = Physics2D.OverlapBox(v2GroundedBoxCheckPosition, v2GroundedBoxCheckScale, 0, whatIsPray);

        if (canPray == true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (praying == false)
                {
                    praying = true;
                    anim.SetTrigger("startPray");
                }
                else if (praying == true)
                {
                    praying = false;
                    anim.SetTrigger("endPray");
                }
            }
        }

        if (praying == true)
        {
            if (bh.health < bh.startHealth)
            {
                if (restoreTime <= 0)
                {
                    bh.health += 1;
                    bh.healthBar.fillAmount = bh.health / bh.startHealth;
                    restoreTime = startRestoreTime;
                }
                else
                {
                    restoreTime -= Time.deltaTime;
                }
            }
            else if (bh.health >= bh.startHealth)
            {
                bh.health = gameObject.GetComponent<BeilanHealth>().health;
            }
        }
        else if (praying == false)
        {
            bh.health = gameObject.GetComponent<BeilanHealth>().health;
        }
    }

    void WallSlide()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        if(isTouchingWall && rb.velocity.y < 0)
        {
            isWallSliding = true;
            anim.SetBool("isWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("isWallSliding", false);
        }
    }

    void WallJump()
    {
        void RecordedForceToAdd()
        {
            forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
        }

        if (isWallSliding)
        {
            RecordedForceToAdd();
        }

        if (isWallSliding == true && Input.GetKeyDown(KeyCode.Space))
        {
            isWallSliding = false;
            isWallJumping = true;
            rb.velocity = forceToAdd;
            sounds[0].Play();
            anim.SetTrigger("TakeOff");
        }

        if (isWallJumping == true && Input.GetButtonDown("Jump"))
        {
            if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
            {
                fJumpPressedRemember = 0;
                fGroundedRemember = 0;
                rb.velocity = forceToAdd;
            }
        }

        if (Input.GetButtonUp("Jump") || bGrounded == true)
        {
            isWallJumping = false;
        }
    }

    void Swim()
    {
        if (inWater == true)
        {
            anim.SetBool("isSwimming", true);
        }
        else
        {
            anim.SetBool("isSwimming", false);
        }
    }

    public void FlipPlayer()
    {
        if (!isWallSliding)
        {
            facingDirection *= -1;
            facingRight = !facingRight;
            Vector2 localScale = beilan.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        if (!facingRight)
        {
            wallCheckDistance = -wallCheckDistance;
        }
        else
        {
            wallCheckDistance = iniWallCheckDistance;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Water")
        {
            inWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Water")
        {
            inWater = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
