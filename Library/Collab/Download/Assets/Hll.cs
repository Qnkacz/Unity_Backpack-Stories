using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hll : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource fallout;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        fallout.Play();
        if(BossScript.instance.itemCount>0)
        {
            BossScript.instance.itemCount--;
        }
        
        BossScript.instance.GetItemName(collision.gameObject);
        BossScript.instance.SanityCheck(5, false);
        Destroy(collision.gameObject);
    }
   
}
