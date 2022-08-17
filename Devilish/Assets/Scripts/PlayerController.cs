using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    private float moveInput;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    
    public int health;

    public float attackTime;
    private float attackTimeCounter;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform feetPos;
    public float Radius;
    public LayerMask whatIsGround;

    public Transform attackPos;   
    public float attackRange;
    public LayerMask whatIsEnemies;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackTimeCounter = 0;
    }
    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }
    void Update()
    {
        Debug.Log(health);
        attackTimeCounter += Time.deltaTime;

        if(moveInput == 0)
            anim.SetBool("isRunning", false); 
        else
            anim.SetBool("isRunning", true);

        if (isGrounded == true)
            anim.SetBool("isJumping", false);
        else
            anim.SetBool("isJumping", true);

        isGrounded = Physics2D.OverlapCircle(feetPos.position, Radius, whatIsGround);
        if(isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("takeOff");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if(jumpTimeCounter > 0 && isJumping == true)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;            
            }
        }

        if(attackTimeCounter >= attackTime)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetBool("isAttacking", true);
                attackTimeCounter = 0;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                    enemiesToDamage[i].GetComponent<EnemyBehavior>().TakeDamage();
            }           
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
