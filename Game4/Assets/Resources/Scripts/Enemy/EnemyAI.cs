using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType
    {
        Simple = 0,
        Shooter = 1,
        Big = 2
    }

    public EnemyType myType = EnemyType.Simple;

    [Header("Move Settings")]
    public float speed;       // Скорость передвижения
    public float forceSpeed;  // Скорость рывка
    public float dashCountdown;// Время перезарядки навыка
    public float patrolSpeed;  // Скорость брождения

    [Header("Radius Settings")]
    public float chasingRadius; // радиус старта преследования
    public float attackRadius;  // радиус старта атаки
    public float retreatRadius; // радиус старта отступления
    public float maxX, minX, minY, maxY;

    [Header("Shooter Setttings")]
    public float fireRate;
    public GameObject bulletPref;

    [Header("Unity Parameters")]
    public bool moveRight = false;
    public bool moveStop = false;
    public bool canMove = true;

    //Other//
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer mySprite;
    private bool facingRight;
    private bool isForced;
    private float mySpeed;

    private Vector3 startPos;
    private Vector3 movePos;

    private EnemyStats myStats;

    private bool gameStarted;

    private void Start()
    {
        gameStarted = true;
        startPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<EnemyStats>();
        mySprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        mySpeed = speed;
        movePos = new Vector2(startPos.x + Random.Range(-minX, maxX), startPos.y + Random.Range(-minY, maxY));
    }

    private void OnDrawGizmosSelected()
    {
        if (!gameStarted) startPos = transform.position;

        Gizmos.color = Color.green;

        float yMod = (maxY - minY) / 2;
        float xMod = (maxX - minX) / 2;

        Vector3 pos = new Vector2(startPos.x, startPos.y);
        Vector3 size = new Vector3(maxX + minX, maxY + minY, 0.1f);

        Gizmos.DrawWireCube(pos, size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasingRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.15f);

    }

    private void FixedUpdate()
    {
        if (canMove) AIChecker();
    }

    private void AIChecker()
    {
        if (myType == EnemyType.Simple) Searching();
        else if (myType == EnemyType.Big) Searching();
        else if (myType == EnemyType.Shooter) return;
    }

    private void Searching()
    {
        if (Vector2.Distance(transform.position, target.position) <= chasingRadius) Chasing();
        else Patrol();
    }

    private void Chasing()
    {
        Flip(target.position);
        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, mySpeed * Time.deltaTime);
        rb.MovePosition(temp);
        if (!isForced) StartCoroutine(Force());
    }
    private IEnumerator Force()
    {
        isForced = true;
        mySpeed = 0.5f;
        yield return new WaitForSeconds(0.5f);
        mySpeed = forceSpeed;
        yield return new WaitForSeconds(0.2f);
        rb.AddForce(transform.forward * mySpeed, ForceMode2D.Impulse);
        mySpeed = speed;
        yield return new WaitForSeconds(dashCountdown);
        isForced = false;
    }
    private void Patrol()
    {
        Flip(movePos);
        Vector2 temp = Vector2.MoveTowards(transform.position, movePos, patrolSpeed * Time.deltaTime);
        rb.MovePosition(temp);

        if((Vector2.Distance(transform.position, movePos)) <= patrolSpeed * Time.deltaTime)
        {
            movePos = new Vector2(startPos.x + Random.Range(-minY, maxX), startPos.y + Random.Range(-minY, maxY));
        }
    }
    private void Flip(Vector3 flipTarget)
    {
        if (transform.position.x > flipTarget.x && facingRight) Flipper();
        if (transform.position.x < flipTarget.x && facingRight) Flipper();
        void Flipper()
        {
            facingRight = !facingRight;
            Vector3 temp = mySprite.transform.localScale;
            temp.x *= -1;
            mySprite.transform.localScale = temp;   
        }
    }
}
