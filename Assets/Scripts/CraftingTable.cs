using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingTable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawner;
    public GameObject[] Tiles = new GameObject[3];
    public GameObject[] ItemsInside = new GameObject[3];
    public GameObject[] ItemList;
    
    void Start()
    {
        Startup();
    }

    // Update is called once per frame
    void Update()
    {
        if(ItemsInside[0]!=null && ItemsInside[1]!=null && ItemsInside[2]!=null)
        {
            CraftItem(ItemsInside[0], ItemsInside[1], ItemsInside[2]);
        }
    }
   
    private void Startup()
    {

    
       
    }
   public int WhatIsEmpty()
    {
        if(ItemsInside[2]==null)
        {
            return 2;
        }
        else if(ItemsInside[1]==null)
        {
            return 1;
        }
        else if(ItemsInside[0]==null)
        {
            return 0;
        }
        else
        {
            return 5;
        }
    }
  

    private void CraftItem(GameObject one, GameObject two, GameObject three)
    {
        //Debug.Log("beep");
        string poststrig = "clone".ToLower();
        //string[] gar = { one.name.ToLower(), two.name.ToLower(), three.name.ToLower() };

        List<string> gar = new List<string>();

        gar.Add(one.name.ToLower());
        gar.Add(two.name.ToLower());
        gar.Add(three.name.ToLower());

        //Debug.Log(one.name.ToLower());
        //zupa
        if (gar.Contains("onion(clone)".ToLower()))
        {
            List<string> garCpy = new List<string>(gar);
            int itemPosition = 0;
            while (garCpy[itemPosition].Contains("onion(clone)".ToLower()) == false && itemPosition < garCpy.Count)
            {
                itemPosition++;
            }
            garCpy.RemoveAt(itemPosition);

            if (garCpy.Contains("empty_bowl(clone)".ToLower()))
            {
                itemPosition = 0;
                while (garCpy[itemPosition].Contains("empty_bowl(clone)".ToLower()) == false && itemPosition < garCpy.Count)
                {
                    itemPosition++;
                }
                garCpy.RemoveAt(itemPosition);

                if (garCpy.Contains("stone(clone)".ToLower()))
                {
                    GameObject soup = Instantiate(ItemList[0], spawner.transform.position, Quaternion.identity);
                    //bone.transform.
                    Destroy(one);
                    Destroy(two);
                    Destroy(three);
                    ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
                    BossScript.instance.itemCount -= 2;
                }
            }
        }
        //strzala
        if (gar.Contains("stick(clone)".ToLower()))
        {
            List<string> garCpy = new List<string>(gar);
            int itemPosition = 0;
            while (garCpy[itemPosition].Contains("stick(clone)".ToLower()) == false && itemPosition < garCpy.Count)
            {
                itemPosition++;
            }
            garCpy.RemoveAt(itemPosition);

            if (garCpy.Contains("feather(clone)".ToLower()))
            {
                itemPosition = 0;
                while (garCpy[itemPosition].Contains("feather(clone)".ToLower()) == false && itemPosition < garCpy.Count)
                {
                    itemPosition++;
                }
                garCpy.RemoveAt(itemPosition);

                if (garCpy.Contains("stone(clone)".ToLower()))
                {
                    GameObject arrow = Instantiate(ItemList[4], spawner.transform.position, Quaternion.identity);
                    //bone.transform.
                    Destroy(one);
                    Destroy(two);
                    Destroy(three);
                    ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
                    BossScript.instance.itemCount -= 2;
                }
            }
        }
        //scroll
        if (gar.Contains("paper(clone)".ToLower()) )
        {
            List<string> garCpy = new List<string>(gar);
            int itemPosition = 0;
            while( garCpy[itemPosition].Contains("paper(clone)".ToLower()) == false && itemPosition < garCpy.Count )
            {
                itemPosition++;
            }
            garCpy.RemoveAt(itemPosition);

            if (garCpy.Contains("ink_bottle(clone)".ToLower()))
            {
                itemPosition = 0;
                while (garCpy[itemPosition].Contains("ink_bottle(clone)".ToLower()) == false && itemPosition < garCpy.Count)
                {
                    itemPosition++;
                }
                garCpy.RemoveAt(itemPosition);

                if (garCpy.Contains("feather(clone)".ToLower()))
                {
                    GameObject scroll = Instantiate(ItemList[3], spawner.transform.position, Quaternion.identity);
                    //bone.transform.
                    Destroy(one);
                    Destroy(two);
                    Destroy(three);
                    ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
                    BossScript.instance.itemCount -= 2;
                }
            }
        }
        //miecz
        if (gar.Contains("metal_scrap(clone)".ToLower()))
        {
            List<string> garCpy = new List<string>(gar);
            int itemPosition = 0;
            while (garCpy[itemPosition].Contains("metal_scrap(clone)".ToLower()) == false && itemPosition < garCpy.Count)
            {
                itemPosition++;
            }
            garCpy.RemoveAt(itemPosition);

            if (garCpy.Contains("metal_scrap(clone)".ToLower()))
            {
                itemPosition = 0;
                while (garCpy[itemPosition].Contains("metal_scrap(clone)".ToLower()) == false && itemPosition < garCpy.Count)
                {
                    itemPosition++;
                }
                garCpy.RemoveAt(itemPosition);

                if (garCpy.Contains("stick(clone)".ToLower()))
                {
                    GameObject sword = Instantiate(ItemList[1], spawner.transform.position, Quaternion.identity);
                    //bone.transform.
                    Destroy(one);
                    Destroy(two);
                    Destroy(three);
                    ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
                    BossScript.instance.itemCount -= 2;
                }
            }
        }
       

    }
    
}
