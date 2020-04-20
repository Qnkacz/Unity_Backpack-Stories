using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresBar : MonoBehaviour
{

    private Slider slider;
    public ParticleSystem particles;
    float angle;
    private float targetProgress = 0;
    public float FillSpeed = 0.5f;
    private void Awake()
    {
        particles = GameObject.Find("particles").GetComponent<ParticleSystem>();
        slider = this.gameObject.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        particles.Pause();
        angle = GameObject.Find("particles").GetComponent<ParticleSystem>().shape.angle;
    }
   

    // Update is called once per frame
  
}
