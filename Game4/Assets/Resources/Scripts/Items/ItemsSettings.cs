using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSettings : MonoBehaviour
{
    public Items thisItem;
    public int count = 1;
    void Start()
    {
        if(thisItem)gameObject.name = thisItem.name;
        if(thisItem)GetComponent<SpriteRenderer>().sprite = thisItem.sprite;  
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Interactive>().item = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Interactive>().item = null;
        }
    }

}
