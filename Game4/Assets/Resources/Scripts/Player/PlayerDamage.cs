using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private Controller controller;
    private SpriteRenderer spriteRenderer;

    private Color myColor;
    [SerializeField] private Color hitColor;
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();  
        controller = GetComponent<Controller>();  
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        myColor = spriteRenderer.color;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!controller.isHited)
        {
            if (collision.transform.tag == "Enemy")
            {
                Hit(collision.transform.GetComponent<EnemyStats>());
            }
        }
    }

    private void Hit(EnemyStats enemyStats)
    {
        int damage = enemyStats.enemyDamage - PlayerStats.PlayerProtection / 2;
        if (damage <= 0) damage = 1;
        PlayerStats.stats.PlayerDamage(damage);

        StartCoroutine(Stun());

        rigidbody2D.velocity = Vector2.zero;

        if (enemyStats.transform.position.x > transform.position.x)
        {
            rigidbody2D.AddForce(Vector2.left * enemyStats.enemyPulse, ForceMode2D.Impulse);
        }
        else rigidbody2D.AddForce(Vector2.right * enemyStats.enemyPulse, ForceMode2D.Impulse);
    }
    private IEnumerator Stun() 
    {
        controller.isHited = true;
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = myColor;
        controller.isHited = false;
    }

}
