using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Items[] items;
    public int[] count;
    public Items[] equipment;
    public int money;

    private int arrowId;


    public static Inventory inv;
    void Awake()
    {
        inv = this;

        items = new Items[25];
        count = new int[25];
        equipment = new Items[3];
    }

    public bool AddItem(Items newItem, int newCount, bool canBuy)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] && items[i].id == newItem.id)
            {
                if (canBuy)
                {
                    if (newItem.cost <= money)
                    {
                        money -= newItem.cost;
                        count[i] += newCount;
                        return true;
                    }
                    else return false;
                }
                count[i] += newCount;
                return true;
            }
        }

        for (int i = 0; i< items.Length; i++)
        {
            if (!items[i])
            {
                if (canBuy)
                {
                    if (newItem.cost <= money)
                    {
                        money -= newItem.cost;
                        items[i] = newItem;
                        count[i] = newCount;
                        return true;
                    }
                    else return false;
                }
                items[i] = newItem;
                count[i] = newCount;
                return true;
            }
        }
        return false;
    }

    public void AddGold(int _count)
    {
        Debug.Log("Вы подняли золото в размере " + _count + ".");
        money += _count;
    }

    public bool Use(int id)
    {
        if (!items[id]) return false;
        switch (items[id].myType)
        {
            case Items.ItemTypes.item:return UseItem(id);
            default:SetEquip(items[id], id);return true;
        }
    }

    private bool UseItem(int id)
    {
        if(!items[id].isUseful) return false;

        if (count[id] > 1)
        {
            count[id]--;
        }
        else
        {
            count[id] = 0;
            items[id] = null;
        }
        return true;
    }

    public bool ArrowChecker(int id)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i])
            {
                if (items[i].id == id)
                {
                    arrowId = i;
                    return true;
                }
            }
        }
        arrowId = 0;
        return false;
    }

    private void SetEquip(Items item, int id)
    {
        Items.ItemTypes equipType = item.myType;
        if (equipment[(int)equipType] == items[id])
        {
            equipment[(int)equipType] = null;
        }
        else equipment[(int)equipType] = items[id];
        if((int)equipType == 1)
        {
            PlayerStats.PlayerDisDamage = item.damage;
        }
    }
    public Sprite GetArrowSprite() { return items[arrowId].sprite; }

    public void UseArrow()
    {
        count[arrowId]--;
        if(count[arrowId] <= 0)
        {
            items[arrowId] = null;
        }
    }
    public void MoveItem(int oldId, int newId)
    {
        items[newId] = items[oldId];
        count[newId] = count[oldId];

        items[oldId] = null;
        count[oldId] = 0;
    }

    public void SwapItem(int oldId, int newId)
    {
        Items tempItem = items[newId];
        int tempCount = count[newId];

        items[newId] = items[oldId];
        count[newId] = count[oldId];

        items[oldId] = tempItem;
        count[oldId] = tempCount;
    }
}
