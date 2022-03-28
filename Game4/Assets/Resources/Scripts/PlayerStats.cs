using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats stats;
    public static string PlayerName; 
    public static Sprite PlayerSprite;

    public static int PlayerHealth;
    public static int PlayerMaxHealth;
    public static int PlayerMana;
    public static int PlayerMaxMana;

    public static float PlayerStamina;
    public static float PlayerMaxStamina;

    public static int Strenght;
    public static int Agility;
    public static int Constitution;
    public static int Intelligence;

    public static int Level = 1;
    public static int Exp;
    public static int ExpPoint;
    public static int PlayerProtection;
    public static int PlayerMelDamage;
    public static int PlayerDisDamage;
    public static int PlayerMagDamage;

    public int[] levelChart;

    [Header("Start Parameters")]
    public int StartStrength = 1;
    public int StartAgility = 1;
    public int StartConstitution = 1;
    public int StartIntelligence = 1;
    public int StartExpPoints = 0;

    [Header("Bonus Settings")]
    public int strDam = 3;
    public float strWeight = 0.25f;
    public int strStam = 4;

    [Space]
    public int aglDam = 3;
    public float aglSpeed = 0.1f;
    public float aglDash = 0.25f;
    public float aglStam = 4;
    [Space]
    public int conHealth = 10;
    public int conProtect = 1;
    public int requireHealthRegen = 5;
    public float timeHealthRegen = 0.1f;

    [Space]
    public int intMana = 5;
    public int intMag = 2;
    public int requireManaRegen = 5;
    public float timeManaRegen = 0.1f;

    [Space]
    public int lvlHealth = 5;
    public int lvlMana = 5;
    public int lvlStamina = 5;

    [Space]
    [SerializeField] private float staminaWaitModificator;
    [SerializeField] private float staminaRegenModificator;

    public static float staminaWait = 1;
    public static float healthWait = 1;
    public static float manaWait = 1;

    private void Awake()
    {
        stats = this;  
    }
    private void Start()
    {
        Strenght = StartStrength;
        Agility = StartAgility;
        Constitution = StartConstitution;
        Intelligence = StartIntelligence;

        Exp = 0;
        ExpPoint = StartExpPoints;
        Manager();
        PlayerHealth = PlayerMaxHealth;
        PlayerMana = PlayerMaxMana;
        PlayerStamina = PlayerMaxStamina;
    }
    private void FixedUpdate()
    {
        Manager();
    }
    private void Update()
    {
        HealthRegen();
        ManaRegen();
        StaminaRegen();
    }
    private void Manager()
    {
        SetMaxParameters();
        SetDamageParameters();
        SetProtectionParameters();
    }

    private void SetMaxParameters()
    {
        if (PlayerHealth > PlayerMaxHealth) PlayerHealth = PlayerMaxHealth;
        if (PlayerMana > PlayerMaxMana) PlayerMana = PlayerMaxMana;
        if (PlayerStamina > PlayerMaxStamina) PlayerStamina = PlayerMaxStamina;

        if(PlayerStamina < 0) PlayerStamina = 0;
        if(PlayerMana < 0) PlayerMana=0;

        PlayerMaxHealth = 10 + (lvlHealth * Level) + (conHealth * Constitution);
        PlayerMaxMana = 10 + (lvlMana * Level) + (intMana * Intelligence);
        PlayerMaxStamina = 20 + (lvlStamina * Level) + (strStam * Strenght) + (aglStam * Agility);
    }

    private void SetDamageParameters()
    {
        PlayerMelDamage = (strStam * Strenght);
        //PlayerDisDamage = (aglDam * Agility);
        PlayerMagDamage = (intMag * Intelligence);

        if(Inventory.inv.equipment[0]) PlayerMelDamage += Inventory.inv.equipment[0].damage;
        if(Inventory.inv.equipment[1]) PlayerMelDamage += Inventory.inv.equipment[1].damage;
    }

    private void SetProtectionParameters()
    {
        PlayerProtection = Level + (conProtect * Constitution);
        if(Inventory.inv.equipment[2]) PlayerProtection += Inventory.inv.equipment[2].protection;
    }

    private void CheckLevel()
    {
        if(Exp >= levelChart[Level])
        {
            Level++;
            ExpPoint += 2;
            SetMaxParameters();

            PlayerHealth = PlayerMaxHealth;
            PlayerMana = PlayerMaxMana;
            PlayerStamina = PlayerMaxStamina;    
        }
    }

    public void AddExp(int addExp)
    {
        Exp += addExp;
        CheckLevel();
    }

    public void PlayerDamage(int damage)
    {
        PlayerHealth -= damage;
        if(PlayerHealth <= 0)
        {
            StartCoroutine(Restart());
            PlayerHealth = 0;
        }
    }

    public static bool PlayerStaminaDamage(float damage, float parameter = 0, float modificator = 0)
    {
        damage -= modificator * parameter;
        if (damage <= 1) damage = 1f;

        if (PlayerStamina >= damage)
        {
            staminaWait = 0;
            PlayerStamina -= damage;
            return true;
        }
        else return false;
    }

    public void PlayerManaDamage(int damage)
    {
        PlayerMana -= damage;
        if (PlayerMana <= 0)
        {
            PlayerMana = 0;
        }
    }
    private void HealthRegen()
    {
        if(Constitution >= requireHealthRegen)
        {
            if (healthWait < 1)
            {
                healthWait += timeHealthRegen * (1 + Constitution - requireHealthRegen) * Time.deltaTime;
            }
        }
        else if(PlayerHealth < PlayerMaxHealth)
        {
            PlayerHealth += 1;
            healthWait = 0;
        }
    }

    public void ManaRegen()
    {
        if (Intelligence >= requireManaRegen)
        {
            if (manaWait < 1)
            {
                manaWait += timeManaRegen * (1 + Intelligence - requireManaRegen) * Time.deltaTime;
            }
            else if(PlayerMana < PlayerMaxMana)
            {
                PlayerMana += 1;
                manaWait = 0;
            }
        }
    }

    private void StaminaRegen()
    {
        /*if (staminaWait < 1)
        {
            staminaWait += staminaWaitModificator * Time.deltaTime;
        }*/
        //else
        {
            if (PlayerStamina < PlayerMaxStamina)
            {
                PlayerStamina += Time.deltaTime * (PlayerMaxStamina * staminaRegenModificator);
            }
        }
    }
    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("SampleScene");
    }
}
