using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCellScript : MonoBehaviour
{
    public enum whatEquip
    {
        melee = 0,
        distant = 1,
        armor = 2,

    }

    public whatEquip what = whatEquip.melee;

    public void Refresh()
    {
        if (Inventory.inv.equipment[(int)what])
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<Image>().sprite = Inventory.inv.equipment[(int)what].sprite;
        }
        else transform.GetChild(0).gameObject.SetActive(false);

    }
}
