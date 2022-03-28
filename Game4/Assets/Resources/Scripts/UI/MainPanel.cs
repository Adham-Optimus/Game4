using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainPanel : MonoBehaviour
{
    public GameObject[] Panels;

    private StatsUI stats;
    private InventoryUI inventory;  
    private SpellBookUI spellbook;

    public void Access()
    {
        Panels = new GameObject[3];

        for(int i  = 0; i < Panels.Length; i++)
        {
            Panels[i] = transform.GetChild(i).gameObject;
        }

        stats =     Panels[0].GetComponent<StatsUI>();
        inventory = Panels[1].GetComponent<InventoryUI>();
        spellbook = Panels[2].GetComponent<SpellBookUI>();

        inventory.Access();


        gameObject.SetActive(false);    
    }

    private void OnEnable()
    {
        if (inventory)
        {
            inventory.Cleaner();
        }
        
    }

    private void OnDisable()
    {
        if (inventory)
        {
            inventory.Cleaner();
        }
    }

    public void Button(int index)
    {
        for(int i = 0; i < Panels.Length; i++)
        {
            Panels[i].SetActive(i == index);
        }
    }
}
