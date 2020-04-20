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
        string[] gar = { one.name.ToLower(), two.name.ToLower(), three.name.ToLower() };
        //Debug.Log(one.name.ToLower());
        //zupa
        if (gar.Contains("stone(clone)".ToLower()) && gar.Contains("empty_bowl(clone)".ToLower()) && gar.Contains("onion(clone)".ToLower())) //itemki skladowe
        {
            GameObject bone = Instantiate(ItemList[0], spawner.transform.position, Quaternion.identity); //spawoanie
            //bone.transform.
            Destroy(one); //kasowanie
            Destroy(two);
            Destroy(three);
            ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
        }
        //strzala
        if (gar.Contains("stick(clone)".ToLower()) && gar.Contains("stone(clone)".ToLower()) && gar.Contains("feather(clone)".ToLower()))
        {
            GameObject bone = Instantiate(ItemList[4], spawner.transform.position, Quaternion.identity);
            //bone.transform.
            Destroy(one);
            Destroy(two);
            Destroy(three);
            ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
        }
        //scroll
        if (gar.Contains("paper(clone)".ToLower()) && gar.Contains("paper(clone)".ToLower()) && gar.Contains("paper(clone)".ToLower()))
        {
            GameObject bone = Instantiate(ItemList[3], spawner.transform.position, Quaternion.identity);
            //bone.transform.
            Destroy(one);
            Destroy(two);
            Destroy(three);
            ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
        }
        //miecz
        if (gar.Contains("stick(clone)".ToLower()) && gar.Contains("metal_scrap(clone)".ToLower()) && gar.Contains("rope(clone)".ToLower()))
        {
            GameObject bone = Instantiate(ItemList[1], spawner.transform.position, Quaternion.identity);
            //bone.transform.
            Destroy(one);
            Destroy(two);
            Destroy(three);
            ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
        }
        //magic_scroll
        if (gar.Contains("paper(clone)".ToLower()) && gar.Contains("ink_bottle(clone)".ToLower()) && gar.Contains("feather(clone)".ToLower()))
        {
            GameObject bone = Instantiate(ItemList[1], spawner.transform.position, Quaternion.identity);
            //bone.transform.
            Destroy(one);
            Destroy(two);
            Destroy(three);
            ItemsInside[0] = null; ItemsInside[1] = null; ItemsInside[2] = null;
        }

    }
    
}
