using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSlider : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector2 direction;
    private bool isBeingHeld;
    private float yPos;
    private float CCyPos;
    public Rigidbody2D rb;
    public float moveSpeed;


    public GameObject crafting; //candy crush field object
    private Rigidbody2D CCrb;
    // Start is called before the first frame update
    void Start()
    {
        CCrb = crafting.GetComponent<Rigidbody2D>();
        isBeingHeld = false;
        moveSpeed = 1200f;
        Debug.Log(crafting.transform.localPosition);

    }

    // Update is called once per frame
    void Update()
    {
        yPos = this.gameObject.transform.localPosition.y;
        CCyPos = crafting.transform.localPosition.y;
        CheckPos();
        if (isBeingHeld)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = (mousePosition - transform.position).normalized;
                rb.velocity = new Vector2(0, direction.y * moveSpeed);
                CCrb.velocity = new Vector2(0, direction.y * moveSpeed * 1f);

                yPos = this.gameObject.transform.localPosition.y;
            }
            else
            {
                rb.velocity = Vector2.zero;
                CCrb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            CCrb.velocity = Vector2.zero;
        }


    }
    private void OnMouseDown()
    {
        isBeingHeld = true;
    }
    private void OnMouseUp()
    {
        isBeingHeld = false;
    }
    private void CheckPos()
    {
        if (yPos <= -7.452)
        {

            this.gameObject.transform.localPosition = new Vector3(2f, -7.452f, 0);
            //CCrb.velocity = Vector2.zero;
            crafting.transform.localPosition = new Vector3(250.4f, 0f, 0);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (yPos >= 2.07f)
        {
            this.gameObject.transform.localPosition = new Vector3(2f, 2.07f, 0);
            // CCrb.velocity = Vector2.zero;
            crafting.transform.localPosition = new Vector3(250.4f, 400.7f, 0);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    public void MoveCandyCrush()
    {

    }
}
