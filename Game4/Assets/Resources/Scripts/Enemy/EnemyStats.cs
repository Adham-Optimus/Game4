using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("EnemyStats")]
    public int enemyHealth;
    public int enemyDamage;
    public int enemyPulse;

    [Header("Drop")]
    public int exp;
    
    private GameObject itemPref;
    public Loot[] loot;            

    void Start()
    {
        itemPref = Resources.Load<GameObject>("Prefabs/Other/Item");
    }

    public void SetDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0) Die();
    }
    

    private void Die()
    {
        for(int i= 0; i < loot.Length; i++)
        {
            if (!loot[i].item || loot[i].count <= 0)
            {
                Debug.Log(loot[i] + ": loot under index " + i + ", of object " + gameObject + " was ");
                continue;
            }

            float random = Random.Range(0, 100f);

            if (random <= loot[i].chance)
            {

                ItemsSettings temp = Instantiate(itemPref, Random.insideUnitSphere * 0.3f + transform.position, Quaternion.identity)
                    .GetComponent<ItemsSettings>();
                temp.thisItem = loot[i].item;
                temp.count = loot[i].count;
            }
        }

        PlayerStats.stats.AddExp(exp);
        Destroy(gameObject);
    }

    [System.Serializable]
    public struct Loot
    {
        public Items item;
        public int count;
        public float chance;

    }
}
