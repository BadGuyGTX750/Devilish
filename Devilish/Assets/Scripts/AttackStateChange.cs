using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateChange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D attackBox)
    {
        if (attackBox.CompareTag("Player")) 
            EnemyBehavior.followState = true;
    }
    void OnTriggerExit2D(Collider2D attackBox)
    {
        if (attackBox.CompareTag("Player")) 
            EnemyBehavior.followState = false;
    }
}
