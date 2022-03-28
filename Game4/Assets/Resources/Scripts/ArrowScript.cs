using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowSpeed = 2000f;
    private Rigidbody2D rigidbody;
    private bool isHited;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    
    void FixedUpdate()
    {
        if (!isHited)
        {
            rigidbody.velocity = transform.right * arrowSpeed * Time.deltaTime;
        }
        else rigidbody.velocity *= 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Player" & !isHited)
        {
            isHited = true;
            GetComponent<Collider2D>().enabled = false;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;

            if (collision.transform.tag == "Enemy")
            {
                transform.SetParent(collision.transform);
                collision.transform.GetComponent<EnemyCanDie>().ArrowHit(transform.position);
            }
        }
    }
}
