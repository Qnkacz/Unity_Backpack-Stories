using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSwipe : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector2 direction;
    private bool isBeingHeld;
    private float xPos;
    private float CCxPos;
    public Rigidbody2D rb;
    public float moveSpeed;


    public GameObject CC; //candy crush field object
    private Rigidbody2D CCrb;
    // Start is called before the first frame update
    void Start()
    {
        CCrb = CC.GetComponent<Rigidbody2D>();
        isBeingHeld = false;
        moveSpeed = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        xPos = this.gameObject.transform.localPosition.x;
        CCxPos = CC.transform.localPosition.x;
        CheckPos();
        if(isBeingHeld)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = (mousePosition - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * moveSpeed, 0);
                CCrb.velocity = new Vector2(direction.x * moveSpeed * 50, 0);
                
                xPos = this.gameObject.transform.localPosition.x;
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
        if (xPos < -959)
        {
            
            this.gameObject.transform.localPosition = new Vector3(-959, -529.0242f, 0);
            //CCrb.velocity = Vector2.zero;
            CC.transform.position = new Vector3(-277.6f, 269, 0);

        }
        if (xPos > -938.4503f)
        {
            this.gameObject.transform.localPosition = new Vector3(-938.4503f, -529.0242f, 0);
           // CCrb.velocity = Vector2.zero;
            CC.transform.position = new Vector3(230, 269, 0);
        }

    }

    public void MoveCandyCrush()
    {

    }
    
}
