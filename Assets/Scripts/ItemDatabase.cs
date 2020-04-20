using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;
    // Start is called before the first frame update
    void Start()
    {
        
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
       ConstruItemDatabase();

        Debug.Log(database[1].Slug);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if(database[i].ID == id)
            {
                return database[i];
            }
        }
        return null;
    }
    void ConstruItemDatabase()
    {
        
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["title"].ToString(), (int)itemData[i]["value"],(bool)itemData[i]["stackable"],itemData[i]["slug"].ToString(),itemData[i]["destription"].ToString()));
        }
         
    }

}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public bool Stackable { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public Sprite Sprite { get; set; }


    public Item(int id, string title, int value, bool stackable, string slug, string description)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Stackable = stackable;
        this.Slug = slug;
        this.Description = description;
        this.Sprite = Resources.Load<Sprite>("Textures/Smol"+ slug);
    }
    public Item(int id, string title, int value)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
    }

    public Item()
    {
        this.ID = -1;
    }
}
