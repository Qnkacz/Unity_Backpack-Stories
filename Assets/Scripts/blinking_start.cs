using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinking_start : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animacja;
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animacja.Play("startButton");   
    }
}
