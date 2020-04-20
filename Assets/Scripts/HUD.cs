using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour
{
    
    
    public int itemToGive = -1;
    public static HUD instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



   
    string GenerateNeededItem()
    {
        string requestedItemname;
        int neededItem = (int)Random.Range(0, 6);
        switch (neededItem)
        {
            case 0:
                requestedItemname = "An Onion!";
                itemToGive = 0;
                break;
            case 1:
                requestedItemname = "The grimuar!";
                itemToGive = 1;
                break;
            case 2:
                requestedItemname = "A spring!";
                itemToGive = 2;
                break;
            case 3:
                requestedItemname = "A magic scroll! ";
                itemToGive = 3;
                break;
            case 4:
                requestedItemname = "A bone!";
                itemToGive = 4;
                break;
            case 5:
                requestedItemname = "An empty bottle!";
                itemToGive = 5;
                break;
            case 6:
                requestedItemname = "Some warm soup!";
                itemToGive = 6;
                break;
            default:
                requestedItemname = "your attention! </3";
                itemToGive = 6;
                break;
        }

        return requestedItemname;

    }

   

    
   

}
