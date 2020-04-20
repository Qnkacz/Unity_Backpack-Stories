using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour
{
    public static BackPack instance;
    public int itemCount;
    public int ItemCount
    {
        get { return itemCount; }
        set
        {
            itemCount++;
            if(itemCount>16)
            {
                BossScript.instance.currStamina -= 10;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
