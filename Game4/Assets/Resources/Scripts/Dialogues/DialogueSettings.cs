using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSettings : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue dialogueEnd;
    
    public bool isShop;

    [Header("Debug")]
    public bool dialogueEnded;
    public bool dialogueStarted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Player"))
        {
            if (dialogueEnded && dialogueEnd == null)
            {
                return;
            }
            collision.GetComponent<Interactive>().dialogue = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            collision.GetComponent<Interactive>().dialogue = null;
        }
    }

}
