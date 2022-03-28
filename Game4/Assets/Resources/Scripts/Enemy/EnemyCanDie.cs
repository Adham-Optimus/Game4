using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanDie : MonoBehaviour
{
    public Color hitColor;
    public float hitColorChangeTime = 0.3f;

    private Rigidbody2D rb;
    private EnemyStats enemyStats;
    private EnemyAI enemyAI;
    private SpriteRenderer spriteRenderer;

    private Color myColor;
    private bool isHited;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
        enemyStats = GetComponent<EnemyStats>();
        enemyAI = GetComponent<EnemyAI>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        myColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Sword"))
        {
            SwordHit();
        }
    }

    private void SwordHit()
    {
        if (isHited) return;

        // агрессия
        StartCoroutine(HitVisual());
        enemyStats.SetDamage(PlayerStats.PlayerMelDamage);
        Transform player = Controller.con.transform;
        rb.velocity = Vector2.zero;

        if (player.position.x > transform.position.x)
        {
            rb.AddForce(Vector2.left * (Inventory.inv.equipment[0].pulse));
        }
        else
        {
            rb.AddForce(Vector2.right * (Inventory.inv.equipment[0].pulse));
        }
    }

    public void ArrowHit(Vector3 arrowPos)
    {
        if (isHited) return;

        StartCoroutine(HitVisual());
        enemyStats.SetDamage(PlayerStats.PlayerDisDamage);
        Debug.Log(PlayerStats.PlayerDisDamage);
        Transform player = Controller.con.transform;
        rb.velocity = Vector2.zero;

        if (arrowPos.x > transform.position.x)
        {
            rb.AddForce(Vector2.left * (Inventory.inv.equipment[1].pulse));
        }
        else
        {
            rb.AddForce(Vector2.right * (Inventory.inv.equipment[1].pulse));
        }
    }

    private IEnumerator HitVisual()
    {
        isHited = true;
        enemyAI.canMove = false;
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(hitColorChangeTime);
        
        isHited = false;
        enemyAI.canMove = true;
        spriteRenderer.color = myColor;
    }
}
