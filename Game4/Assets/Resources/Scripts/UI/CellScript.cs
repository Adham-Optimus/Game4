using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryUI ui;
    public int cellId = 0;

    public bool isFree = true;
    public bool isEquipt = false;

    private Image myImage;
    private Image myIcon;
    private Text myCount;

    public void Refresh()
    {
        if (!Inventory.inv) return;
        if (Inventory.inv.items[cellId])
        {
            isFree = false;
            myIcon.gameObject.SetActive(true);
            myCount.gameObject.SetActive(true);
            myIcon.sprite = Inventory.inv.items[cellId].sprite;
            myCount.text = Inventory.inv.count[cellId].ToString();
        }
        else
        {
            isFree=true;
            myIcon.gameObject.SetActive(false);
            myCount.gameObject.SetActive(false);
        }
    }
    void Access()
    {
        ui = transform.parent.GetComponentInParent<InventoryUI>();
    }

    

    public CellScript GetLinkSetSettings(int newId, InventoryUI newUI)
    {
        ui = newUI;
        cellId = newId;
        isFree = true;
        myImage = GetComponent<Image>();
        myIcon= transform.GetChild(0).GetComponent< Image>();
        myCount = transform.GetChild(1).GetComponent<Text>();
        return this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui) ui.CurorCellSwitch(this);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(ui)ui.cursorCell = null;
    }

    public void SetColor(Color newColor)
    {
        myImage.color = newColor;
    }
}
