using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Weapons")]
    // Sword //
    public GameObject mySword;
    public Animator mySwordAnimator;
    public SpriteRenderer mySwordRender;
    public BoxCollider2D mySwordCollider;
    private bool isMelee;

    [Space]
    // Bow //
    public GameObject distantWeapon;
    public SpriteRenderer myBow;
    public Transform arrowPoint;
    private bool arrowIsReady;
    private float bowIsReady;
    private bool bowIsCharged;

    [Header("Player Settings")]

    public float moveSpeed;
    public float dashForce;
    public float dashStaminaLose;
    public float dashCountdown;
    private float dashTime;
    private float dashReady;
    public bool isDashed;

    private bool facingRight = true;
    public bool isHited;
    private bool playerIsStand;

    private Rigidbody2D rb;

    [Header("Weapon")]
   

    



    private Vector3 cursor;
    private float x, y, xPlus, yPlus;
    private Transform mySprite;    

    public static Controller con;  
    void Awake()
    {
        con = this;
        rb = GetComponent<Rigidbody2D>();
        mySprite = transform.GetChild(0);

        mySword         = mySprite.GetChild(0).gameObject;
        mySwordAnimator = mySword.GetComponent<Animator>();
        mySwordRender   = mySword.GetComponent<SpriteRenderer>();
        mySwordCollider = mySword.GetComponent<BoxCollider2D>();

        distantWeapon   = transform.GetChild(1).gameObject;
        myBow           = distantWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrowPoint      = distantWeapon.transform.GetChild(1);

        distantWeapon.SetActive(false);
        mySword.SetActive(false);
    }

    
    void Update()
    {
        InputManager();
        Dash();
        Attack();
    }
    private void Attack()
    {
        if (Input.GetMouseButton(1)) Distance();
        else Melee();

        if (Input.GetMouseButtonUp(1))
        {
            /*arrowIsReady = false;
            bowIsCharged = false;*/
            bowIsReady = 0;
        }

        HitOff();
    }

    private void Melee()
    {
        if (isMelee) return;
        if(Input.GetMouseButtonDown(0) && Inventory.inv.equipment[0])
        {
            if (PlayerStats.PlayerStaminaDamage(Inventory.inv.equipment[0].weight, PlayerStats.Strenght, PlayerStats.stats.strWeight))
            {
                mySwordRender.sprite = Inventory.inv.equipment[0].sprite;
                mySwordCollider.size = new Vector2(0.4f, Inventory.inv.equipment[0].length);
                mySwordCollider.offset = new Vector2(0f, Inventory.inv.equipment[0].offset);

                float speed = Inventory.inv.equipment[0].speed;
                mySwordAnimator.speed = speed;

                isMelee = true;
                mySword.SetActive(true);
            }
        }
    }

    private void Distance()
    {
        if (isMelee || !Inventory.inv.equipment[1]) return;
        distantWeapon.SetActive(true);
        Vector2 bowPos = distantWeapon.transform.position;
        Vector2 direction = (Vector2)cursor - bowPos;
        distantWeapon.transform.right = direction;

        PlayerStats.staminaWait = 0;

        bowIsReady += Inventory.inv.equipment[1].speed * 0.5f * Time.deltaTime;

        if(bowIsReady <= 2)
        {
            if (myBow.sprite != Inventory.inv.equipment[1].sprite) myBow.sprite = Inventory.inv.equipment[1].sprite;
            
            if(bowIsReady < 1f)
            {
                arrowPoint.gameObject.SetActive(false);
            }
            else
            {
                if (Inventory.inv.ArrowChecker(Inventory.inv.equipment[1].arrowID))
                {
                    if (!arrowIsReady)
                    {
                        arrowIsReady = true;
                        arrowPoint.gameObject.SetActive(true);
                        arrowPoint.localPosition = new Vector3(1.5f, 0, 0);
                        arrowPoint.GetComponent<SpriteRenderer>().sprite = Inventory.inv.GetArrowSprite();
                    }

                }

            }
        }
        else if (arrowIsReady)
        {
            if (myBow.sprite != Inventory.inv.equipment[1].spriteForBowState)
            {
                myBow.sprite = Inventory.inv.equipment[1].spriteForBowState;
            }
            arrowPoint.localPosition = new Vector3(0.9f, 0, 0);
            bowIsCharged = true;
        }
        if (bowIsCharged)
        {
            if (Input.GetMouseButton(0))
            {
                if(PlayerStats.PlayerStaminaDamage(Inventory.inv.equipment[1].weight, PlayerStats.Strenght, PlayerStats.stats.strWeight))
                {
                    bowIsCharged = false;
                    arrowIsReady = false;
                    bowIsReady = 0f;
                    Inventory.inv.UseArrow();
                    Instantiate(Inventory.inv.equipment[1].myArrow, arrowPoint.position, arrowPoint.rotation);
                }
            }
        }
    }

    private void HitOff()
    {
        if(!Input.GetMouseButton(1)) distantWeapon.SetActive(false);

        if (!isMelee) { return; }
        if (!mySwordAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordBeating"))
        {
            isMelee=false;
            mySword.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        if (!isHited & !isDashed) Move();
    }

    private void Move()
    {
        mySprite.eulerAngles = new Vector3(0, 0, 15 * -x);    
        rb.velocity = new Vector2(x, y) * moveSpeed * Time.deltaTime;
    }
    private void InputManager()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xPlus = Input.GetAxis("Horizontal+");
        yPlus = Input.GetAxis("Vertical+");


        playerIsStand = (x == 0 & y == 0);

        cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 temp = mySprite.localScale;
        temp.x *= -1;
        mySprite.localScale = temp; 
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playerIsStand & !isDashed)
        {
            if (PlayerStats.PlayerStaminaDamage(dashStaminaLose, PlayerStats.Agility, PlayerStats.stats.aglDash))
            {
                isDashed = true;
                dashTime = dashCountdown;
                rb.AddForce(new Vector2 (xPlus * dashForce, yPlus * dashForce), ForceMode2D.Impulse);
            }
        }
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
        }
        else if (isDashed)
        {
            dashTime = 0;
            isDashed = false;
            rb.velocity = Vector2.zero;
        }
    }
}
