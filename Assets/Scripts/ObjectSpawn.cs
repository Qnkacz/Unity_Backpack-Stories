using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    GameObject inventoryPanel;
    public GameObject inventoryItem;
    ItemDatabase database;

    public List<Item> items = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        database = GetComponent<ItemDatabase>();
        inventoryPanel = GameObject.Find("Inventory Panel");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
