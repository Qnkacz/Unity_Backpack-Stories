using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DraggingObjects : MonoBehaviour
{
    //public AudioSource onGoodGive;
    private CraftingTable craftingTable;
    public float moveSpeed;
    public float offset = 0.05f;
    private bool following;
    public int itemID;
    public static DraggingObjects instance;
    public Vector3 prevPosition = new Vector3(0, 0, 0);
    public GameObject GBP;
    private int EmptyInCraftingTable;
    private int whereAmI=-1;

    private float startPosX;
    private float startPosY;

    #region ThrowDynamics
    Vector2 startPos,endPos,direction;
    float touchTimeStart, touchTimeFinish, timeInterval;
    public float force = .3f;
    #endregion

    #region decay
    public float timeToDecay;
    private float opacity;
     private bool Deletion()
    {
        BossScript.instance.itemCount -= 1;
        Destroy(this.gameObject);
        return true;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        craftingTable = FindObjectOfType<CraftingTable>();
        
        opacity = 1;
        instance = this;
        following = false;
        offset += 10;
        StartCoroutine(BreakTime());
    }

    // Update is called once per frame
    void Update()
    {
        
        //SanityCheck();

        if(following==true)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
            if(whereAmI!=-1)
            {
                if(this.gameObject.transform.position!=craftingTable.Tiles[whereAmI].transform.position)
                {
                    craftingTable.ItemsInside[whereAmI] = null;
                }
            }
        }
        

    }

   

  
    public void OnMouseDown()
    {

        
        if(Input.GetMouseButtonDown(0))
        {
            touchTimeStart = Time.time;
            startPos = Input.mousePosition;
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;
            following = true;
        }
        EmptyInCraftingTable = craftingTable.WhatIsEmpty();
        
        
    }

    public void OnMouseUp()
    {
        touchTimeFinish = Time.time;
        timeInterval = touchTimeFinish - touchTimeStart;
        endPos = Input.mousePosition;
        direction = startPos - endPos;
        GetComponent<Rigidbody2D>().AddForce(-direction / timeInterval * force);
        following = false;
        CheckIfReturn();

    }
  
  
    private IEnumerator BreakTime(float time = 60f)
    {

        timeToDecay = time;
        while (timeToDecay >= 0)
        {
            if (timeToDecay == 0)
            {
                yield return Deletion();
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, opacity);
                //Debug.Log("Countdown: " + timeToDecay);
                yield return new WaitForSeconds(1.0f);
                timeToDecay--;
                opacity -= (1/time);
            }
        }
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name=="CraftingArray")
        {
            following = false;
            this.gameObject.transform.position = craftingTable.Tiles[EmptyInCraftingTable].transform.position;
            craftingTable.ItemsInside[EmptyInCraftingTable] =this.gameObject;
            whereAmI = EmptyInCraftingTable;
        }
    }
    private void CheckIfReturn()
    {
        if ((this.gameObject == craftingTable.ItemsInside[0]
            || this.gameObject == craftingTable.ItemsInside[1]
            || this.gameObject == craftingTable.ItemsInside[2])
            &&
            (this.gameObject.transform.position != craftingTable.Tiles[0].transform.position
            || this.gameObject.transform.position != craftingTable.Tiles[1].transform.position
            || this.gameObject.transform.position != craftingTable.Tiles[2].transform.position))
        {
            this.gameObject.transform.position = craftingTable.Tiles[whereAmI].transform.position;
        }
    }




}