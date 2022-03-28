using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public MainPanel mainPanel;
    public static bool mainPanelIsOpen;

    public RectTransform hpUI;
    public RectTransform spUI;
    public RectTransform mpUI;

    Image hp;
    Image sp;
    Image mp;

    private Text hpText;
    private Text spText;
    private Text mpText;

    public static CanvasScript canvas;

    private void Awake()
    {
        canvas = this; 

        if (hpUI)
        {
            hp = hpUI.GetChild(0).GetComponent<Image>();
            hpText = hpUI.GetChild(1).GetComponent<Text>();

            sp = spUI.GetChild(0).GetComponent<Image>();
            spText = spUI.GetChild(1).GetComponent<Text>();

            mp = mpUI.GetChild(0).GetComponent<Image>();
            mpText = mpUI.GetChild(1).GetComponent<Text>();
        }
        AccessAll();

        mainPanelIsOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mainPanelIsOpen = !mainPanelIsOpen;
            mainPanel.gameObject.SetActive(mainPanelIsOpen);
        }
    }
    private void FixedUpdate()
    {
        DataBars();
        SizeBars();

    }

    private void DataBars()
    {
        float perHealth = PlayerStats.PlayerMaxHealth / 100;
        float perStamina = PlayerStats.PlayerMaxStamina / 100;
        float perMana = PlayerStats.PlayerMaxMana / 100;
         
        hp.fillAmount = ((float)PlayerStats.PlayerHealth  / PlayerStats.PlayerMaxHealth);
        sp.fillAmount = (       PlayerStats.PlayerStamina / PlayerStats.PlayerMaxStamina);
        mp.fillAmount = ((float)PlayerStats.PlayerMana    / PlayerStats.PlayerMaxMana);

        hpText.text = PlayerStats.PlayerHealth + "/" + PlayerStats.PlayerMaxHealth;
        spText.text = PlayerStats.PlayerStamina.ToString("0") + "/" + PlayerStats.PlayerMaxStamina.ToString("0");
        mpText.text = PlayerStats.PlayerMana + "/" + PlayerStats.PlayerMaxMana;
    }

    private void SizeBars()
    {
        hpUI.sizeDelta = new Vector2(100 + PlayerStats.PlayerMaxHealth * 3, hpUI.sizeDelta.y);
        spUI.sizeDelta = new Vector2(100 + PlayerStats.PlayerMaxStamina * 3, spUI.sizeDelta.y);
        mpUI.sizeDelta = new Vector2(100 + PlayerStats.PlayerMaxMana * 3, mpUI.sizeDelta.y);
    }
    private void AccessAll()
    {
        mainPanel.Access();
    }
}
