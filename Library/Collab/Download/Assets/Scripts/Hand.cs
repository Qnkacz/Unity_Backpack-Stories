using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    private SpriteRenderer spriteRenderer;
    public bool isGiven;
    bool mouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        isGiven = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseOver()
    {
        mouseOver = true;
        spriteRenderer.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        mouseOver = false;
        spriteRenderer.sprite = normalSprite;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Checkitem(other.gameObject);
        if(other.gameObject.name=="Scroll(copy)")
        {
            BossScript.instance.currMana -= 10;
        }
    }

    private void Checkitem(GameObject obj)
    {
        Debug.Log("checking item");
        if(obj.GetComponent<DraggingObjects>().itemID==BossScript.instance.itemToGive)
        {
            BossScript.instance.currHP += 10;
            isGiven = true;
        }
        else
        {
            BossScript.instance.currHP -= 10;
        }
        Destroy(obj);
        if (BossScript.instance.itemCount > 0)
        {
            BossScript.instance.itemCount--;
        }
    }
}
