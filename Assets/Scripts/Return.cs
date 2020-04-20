using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playmodes;
    public GameObject options;
    public GameObject main;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("kliknałeś");
            main.SetActive(true);
            playmodes.SetActive(false);
            options.SetActive(false);
            
        }
    }
}
