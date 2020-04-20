using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class inventory : MonoBehaviour
{

    GameObject inventoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    ItemDatabase database;

    private int SlotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        database = GetComponent<ItemDatabase>();
        SlotAmount = 16;
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
        for (int i = 0; i < SlotAmount; i++)
        {

            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }
        Additem(0);
    }
    public void Additem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == -1)
            {
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                itemObj.transform.SetParent(slots[i].transform);
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.transform.position = Vector2.zero;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
