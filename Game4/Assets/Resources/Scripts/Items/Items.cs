using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Items : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite sprite;
    [TextArea(3, 10)]
    public string description = "Здесть атмос опис";
    [TextArea(3, 10)]
    public string effect = "Здесь опис свва предмета";
    public int cost;

    public enum ItemTypes
    {
        melWeapon = 0,
        disWeapon = 1,
        armor = 2, 
        item =  3,
        gold = 4
    }

    public ItemTypes myType = ItemTypes.item;

    [Header("Item Settings")]
    public bool isUseful;
    public bool isArrow;
    public int arrowID;

    [Header("weapon Settings")]
    public int damage;
    public float pulse;
    public float speed;
    public float weight;
    public Sprite spriteForBowState;
    public GameObject myArrow;
    [Space]
    public float length;
    public float offset;
    public int protection;
    public SpellBookUI spellBook;


}
