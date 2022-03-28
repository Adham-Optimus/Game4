using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactive : MonoBehaviour
{
    public ItemsSettings item;
    //public Chest item;
    public DialogueSettings dialogue;
    //public Save save;
    //public Door door;

    public static Interactive player;
    private Inventory inv;
    void Awake()
    {
        player = this;
        inv = GetComponent<Inventory>();
    }

    
    void Update()
    {
        if (item)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Sound;
                if (item.thisItem.myType == Items.ItemTypes.gold) TakeGold();
                else TakeItem();
            }
        }
        else if (dialogue)
        {
            if (Input.GetKeyDown(KeyCode.E) && !dialogue.dialogueStarted)
            {
                DialogueManager.Instance.StartDialogue(dialogue);
                dialogue = null;
            }
        }
    }
     
    public void TakeItem()
    {
        if(inv.AddItem(item.thisItem, item.count, false))
        {
            Debug.Log("You take " + item.thisItem.name);
            if ((int)item.thisItem.myType == 1)
            {
                PlayerStats.PlayerDisDamage = item.thisItem.damage;
            }
            Destroy(item.gameObject);
            item = null;
        }
        else
        {
            Debug.Log("inventory is full");
        }
    }

    public void TakeGold()
    {
        inv.AddGold(item.count * item.thisItem.cost);
        Destroy(item.gameObject);
    }

    
}
