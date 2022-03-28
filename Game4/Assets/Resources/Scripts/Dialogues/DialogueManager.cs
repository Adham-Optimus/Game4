using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private GameObject UI;
    private Image Portrate;
    private Text Name;
    private Text ReplicaText;
    private GameObject[] buttons;
    private Text[] buttonTexts;
    private DialogueSettings dialogueSettings;
    private Dialogue dialogue;
    private int replicaIndex;
    private int textIndex;
    private bool stopReplicas = false;
    private GameObject itemPref;

    public static DialogueManager Instance;

    public bool dialogueSettingsIsStarted => dialogueSettings.dialogueStarted;
    private void Awake() 
    {
        Initialize();
    }

    public void StartDialogue(DialogueSettings newDialogueSettings)
    {
        dialogueSettings = newDialogueSettings;
        dialogueSettings.dialogueStarted = true;

        if (dialogueSettings.dialogueEnded) dialogue = dialogueSettings.dialogueEnd;
        else dialogue = dialogueSettings.dialogue;

        Portrate.sprite = dialogue.npcImage;
        Name.text = dialogue.npcName;

        replicaIndex = 0;
        textIndex = 0;

        UI.SetActive(true);
        DialogueReader();
    }

    private void Initialize()
    {
        Instance = this;
        UI = transform.GetChild(0).gameObject;

        Portrate    = UI.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Name        = UI.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        ReplicaText = UI.transform.GetChild(2).GetComponent<Text>();

        buttons = new GameObject[3];
        buttonTexts = new Text[3];

        for (int i = 0; i < 3; i++)
        {
            buttons[i] = UI.transform.GetChild(i + 3).gameObject;
            buttonTexts[i] = buttons[i].transform.GetChild(0).GetComponent<Text>() ;
            buttons[i].SetActive(false);
        }
        buttons[0].GetComponent<Button>().onClick.AddListener(But0);
        buttons[1].GetComponent<Button>().onClick.AddListener(But1);
        buttons[2].GetComponent<Button>().onClick.AddListener(But2);

        itemPref = Resources.Load<GameObject>("Prefabs/Other/Item");

        UI.SetActive(false);
    }
     
    private void Update()
    {
        if (dialogue)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !stopReplicas) DialogueReader();
        }
    }

    private void DialogueReader()
    {
        if (textIndex < dialogue.replicas[replicaIndex].replicaText.Length) OutReplicaText();
        else OnAnswers();
    }

    private void OutReplicaText()
    {
        ReplicaText.text = dialogue.replicas[replicaIndex].replicaText[textIndex];
        textIndex++;
    }
    private void AnswerReader(AnswerTypes type, int link)
    {
        switch (type)
        {
            case AnswerTypes.next_L:
                replicaIndex = link;
                textIndex = 0;
                DialogueReader();
                break;
            ////////////////////////////
            case AnswerTypes.exit:
                Exit();
                break;
            ////////////////////////////
            case AnswerTypes.shop:
                // OPEN SHOP
                break;
            ////////////////////////////
            case AnswerTypes.healHp_F:
                PlayerStats.PlayerHealth = PlayerStats.PlayerMaxHealth;
                dialogueSettings.dialogueEnded = true;
                Exit();
                break;
            ////////////////////////////
            case AnswerTypes.healMp_F:
                PlayerStats.PlayerMana = PlayerStats.PlayerMaxMana;
                dialogueSettings.dialogueEnded = true;
                Exit();
                break;
            ////////////////////////////
            case AnswerTypes.healAll_F:
                PlayerStats.PlayerMana = PlayerStats.PlayerMaxMana;
                PlayerStats.PlayerHealth = PlayerStats.PlayerMaxHealth;
                dialogueSettings.dialogueEnded = true;
                Exit();
                break;
            ////////////////////////////
            case AnswerTypes.giveItem_L_F:
                if(Inventory.inv.AddItem(ItemManager.Items[link], 1, true))
                {
                    Debug.Log("You get " + ItemManager.Items[link].itemName);
                }
                else
                {
                    Debug.Log("You have not enough money");
                    /*ItemsSettings temp = Instantiate(itemPref, Controller.con.transform.position, Quaternion.identity).GetComponent<ItemsSettings>();
                    temp.thisItem = ItemManager.Items[link];
                    temp.count = 1;*/
                }
                //dialogueSettings.dialogueEnded = true;
                Exit();
                break;
            ////////////////////////////
            case AnswerTypes.finish:
                dialogueSettings.dialogueEnded = true;
                Destroy(dialogueSettings.gameObject);
                Exit();
                break;
            ////////////////////////////
            default:
                break;
        }

        OffAnswers();
    }
    private void Exit( )
    {
        dialogueSettings.dialogueStarted = false;
        replicaIndex = 0;
        textIndex = 0;
        dialogue = null;
        dialogueSettings = null;
        UI.SetActive(false);
        Interactive.player.dialogue = null;
    }
    private void OffAnswers()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
            buttonTexts[i].text = "";
        }
        stopReplicas = false;
    }
    private void OnAnswers()
    {
        for (int i = 0; i < dialogue.replicas[replicaIndex].answers.Length; i++)
        {
            if (i >= buttons.Length)
            {
                break;
            }
            buttons[i].SetActive(true);
            buttonTexts[i].text = dialogue.replicas[replicaIndex].answers[i].answerText;
        }
        stopReplicas = true;
        ReplicaText.text = "";
    }

    private void But0()
    {
        AnswerReader(dialogue.replicas[replicaIndex].answers[0].answerType, dialogue.replicas[replicaIndex].answers[0].link);
    }

    private void But1()
    {
        AnswerReader(dialogue.replicas[replicaIndex].answers[1].answerType, dialogue.replicas[replicaIndex].answers[1].link);
    }

    private void But2()
    {
        AnswerReader(dialogue.replicas[replicaIndex].answers[2].answerType, dialogue.replicas[replicaIndex].answers[2].link);
    }
}
