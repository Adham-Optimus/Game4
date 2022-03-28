using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static Items[] Items { get; private set; }
    [SerializeField] private Items[] items;

    private void Awake()
    {
        Items = items;
    }
}
