using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public CellScript[] cells;// ссылки на €чейки инвентор€
    private Transform cellPanel; // ссылка на главную панель €чеек ()
    public Transform cursor; // ссылка на объект прив€занный у курсору
    private Image cursorImage;
    private Text cursorText;

    //Interactive Settings
    public Items selectedItem; //сохранненый выделенный предмет
    public int selectedCount; // сохранненое колво выделенного предмета
    public CellScript cursorCell; // выделенна€ €чейка курсором
    public CellScript selectedCell; // выбранна€ €чейка
    public CellScript previousCell; // стартова€ €чейка перетаскивани€

    //InfoPanelSettings
    private Transform infoPanel; 
    public Image infoImage;
    private Text infoName;
    private Text infoText;
    private Text infoDescription;
    private Text infoEffect;
    private Text infoCost;

    //EquipPanel Settings
    private Transform equipPanel;
    private EquipCellScript[] equipCells;

    //Colors
    [Header("Inventory Colors")]
    public Color myColor;
    public Color cursorColor;
    public Color selectColor;
    public Color equipColor;
    public Color[] colors;
    private GameObject itemPref;

    

    public void Access()
    {
        cellPanel = transform.GetChild(0);
        infoPanel = transform.GetChild(1);
        equipPanel = transform.GetChild(2);
        //Cursor
        if (cursor)
        {
            cursorImage = cursor.GetComponent<Image>();
            cursorText = cursor.GetChild(0).GetComponent<Text>();
        }


        cells = new CellScript[25];
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = cellPanel.GetChild(i).GetComponent<CellScript>().GetLinkSetSettings(i, this);
            
        }


        infoImage = infoPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        infoName = infoPanel.GetChild(1).GetComponent<Text>();
        infoDescription = infoPanel.GetChild(2).GetComponent<Text>();
        infoEffect = infoPanel.GetChild(3).GetComponent<Text>();
        infoCost = infoPanel.GetChild(4).GetComponent<Text>();

        //Equip
        equipCells = new EquipCellScript[3];
        for (int i = 0; i < equipCells.Length;i++)
        {
            equipCells[i] = equipPanel.GetChild(i).GetComponent<EquipCellScript>();
        }
        itemPref = Resources.Load<GameObject>("Prefabs/Other/Item");
    }
    private void Update()
    {
        if (cursorCell)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectCellSwitch();
                Debug.Log("LKM" + cursorCell);
            }
            if (Input.GetMouseButtonDown(1))
            {
                if(Inventory.inv.Use(cursorCell.cellId)) RefreshAll();
                Debug.Log("PKM" + cursorCell);
            }
        }
    }

    private void SelectCellSwitch()
    {
        if (!selectedCell)
        {
            selectedCell = cursorCell;
        }
        else
        {
            if (selectedCell == cursorCell)
            {
                selectedCell = null;
            }
            else
            {
                selectedCell = cursorCell;
            }
        }
        RefreshAll();
    }
    public void CurorCellSwitch(CellScript newCell)
    {
        if(!cursorCell) cursorCell = newCell;
        else cursorCell = null;
        RefreshAll();
    }

    public void RefreshAll()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].isEquipt = false;  

            if (Inventory.inv.items[i])
            {
                int index = (int)Inventory.inv.items[i].myType;
                if(Inventory.inv.equipment.Length > index)// 
                {
                    cells[i].isEquipt = Inventory.inv.equipment[index] == Inventory.inv.items[i];
                }
            }
            cells[i].Refresh();

            if (cells[i].isEquipt)
            {
                cells[i].SetColor(equipColor);
            }
            else cells[i].SetColor(myColor);

        }
        if (cursorCell && !cursorCell.isEquipt)
        {
            cursorCell.SetColor(cursorColor);
        }
        if (selectedCell)
        {
            if (!selectedCell.isEquipt)
            {
                selectedCell.SetColor(selectColor);
            }
            InfoChange(Inventory.inv.items[selectedCell.cellId]);
        }
        else InfoChange();
        
        for (int i = 0; i < equipCells.Length; i++)
        {
            equipCells[i].Refresh();
        }
    }

    private void InfoChange(Items itemInfo = null)
    {
        if (itemInfo)
        {
            infoImage.enabled = true;
            infoImage.sprite = itemInfo.sprite;

            infoName.text = itemInfo.itemName;
            infoDescription.text = itemInfo.description;    
            infoEffect.text = itemInfo.effect;  
            infoCost.text = "Cost " + itemInfo.cost;
        }
        else
        {
            infoImage.enabled = false;

            infoName.text = "";
            infoDescription.text = "";
            infoEffect.text = "";
            infoCost.text = "";
        }
    }

    public void Cleaner()
    {
        ClearCurrsor();
        cursorCell = null;
        selectedCell = null;
        RefreshAll();
    }

    public void OnDrag(PointerEventData eventData)
    {
        cursor.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!cursorCell) { return; }
        if (!Inventory.inv.items[cursorCell.cellId]) return;


        previousCell = cursorCell;
        selectedCell = cursorCell;
        RefreshAll();

        cursor.gameObject.SetActive(true);
        cursorImage.sprite = Inventory.inv.items[previousCell.cellId].sprite;
        cursorText.text = Inventory.inv.count[previousCell.cellId].ToString();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(cursorCell == previousCell)
        {
            ClearCurrsor();
            return;
        }
        if(!cursorCell && previousCell)
        {
            DropItem();
            ClearCurrsor();
        }
        if (cursorCell && previousCell)
        {
            if (cursorCell.isFree) Inventory.inv.MoveItem(previousCell.cellId, cursorCell.cellId);
            else                   Inventory.inv.SwapItem(previousCell.cellId, cursorCell.cellId);
            ClearCurrsor();
        }
    }

    private void DropItem()
    {
        int index = (int)Inventory.inv.items[previousCell.cellId].myType;

        if(Inventory.inv.equipment.Length > index)
        {
            if(Inventory.inv.equipment[index] == Inventory.inv.items[previousCell.cellId])
            {
                Inventory.inv.equipment[index] = null; 
                previousCell.isEquipt = false;
            }
        }
        Vector3 tempVec = Controller.con.transform.position + Random.insideUnitSphere * 1.5f;
        tempVec.z = -0.1f;
        ItemsSettings temp = Instantiate(itemPref, tempVec, Quaternion.identity).GetComponent<ItemsSettings>();
        temp.thisItem = Inventory.inv.items[previousCell.cellId];
        temp.count = Inventory.inv.count[previousCell.cellId];

        Inventory.inv.items[previousCell.cellId] = null;
        Inventory.inv.count[previousCell.cellId] = 0;
    }

    private void ClearCurrsor()
    {
        cursor.gameObject.SetActive(false);
        previousCell = null;
        if(cursorCell)selectedCell = cursorCell;
        RefreshAll();
    }
}
