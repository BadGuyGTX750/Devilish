using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public static bool followState;
    public GameObject player;

    public float speed;
    public float minDistance;
    public float retreatDistance;
    public float offsetX;
    public float offsetY;

    public int health;
    public GameObject bloodEffect;
    private float dazedTimeCounter;
    public float dazedTime;

    private bool playerToDamage;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsPlayer;

    public float attackTime;
    private float attackTimeCounter;

    private bool facingRight = true;
    private Vector2 lastEnemyPos;

    private Animator anim;

    void Start()
    {
        GameObject player = GetComponent<GameObject>();
        lastEnemyPos = transform.position;
        anim = GetComponent<Animator>();
        attackTimeCounter = 0;
        dazedTimeCounter = dazedTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 enemyPos = transform.position;

        if (followState == true)
        {
            attackTimeCounter += Time.deltaTime;
            if (attackTimeCounter < attackTime)
                anim.SetBool("isAttacking", false);

            if (enemyPos.x - playerPos.x > 0 && facingRight == true)
                Flip();
            else if (enemyPos.x - playerPos.x <= 0 && facingRight == false)
                Flip();

            if (Vector2.Distance(playerPos, enemyPos) > minDistance)
                transform.position = Vector2.MoveTowards(enemyPos, new Vector2(playerPos.x + offsetX, playerPos.y + offsetY), speed * Time.deltaTime);
            else if (Vector2.Distance(playerPos, enemyPos) <= minDistance && Vector2.Distance(playerPos, enemyPos) > retreatDistance)
            {
                transform.position = this.transform.position;
                if(attackTimeCounter >= attackTime)
                {
                    playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsPlayer);                                        
                    anim.SetBool("isAttacking",true);
                    attackTimeCounter = 0;
                }
            }              
            else if (Vector2.Distance(playerPos, enemyPos) <= retreatDistance)
                transform.position = Vector2.MoveTowards(enemyPos, playerPos, -2 * speed * Time.deltaTime);
        }    
    }

    void Update()
    {
        if(dazedTimeCounter < dazedTime)
        {
            speed = 0;
            dazedTimeCounter += Time.deltaTime;
        }
        else
        {
            speed = 4;
        }
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale *= new Vector2(-1,1);
    }
    public void TakeDamage()
    {
        dazedTimeCounter = 0;
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health--;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
