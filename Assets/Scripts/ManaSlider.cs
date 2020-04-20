using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{
    public static ManaSlider ManaSliderInstance;

    private Slider slider;
    public ParticleSystem particles;
    private void Awake()
    {
        
        slider = this.gameObject.GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ManaSliderInstance = this;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeBarValue();
    }
    public void ChangeBarValue()
    {
        if (BossScript.instance.currMana != slider.value)
        {
            if (slider.value - BossScript.instance.currMana < 0.2)
            {
                slider.value = BossScript.instance.currMana;
            }
            if (slider.value < BossScript.instance.currMana)
            {
                //Debug.Log("powinienem zwiekszac");
                slider.value += Time.deltaTime * BossScript.instance.fillSpeed;
            }
            if (slider.value > BossScript.instance.currMana)
            {
                //Debug.Log("powinienem zmniejszac");
                slider.value -= Time.deltaTime * BossScript.instance.fillSpeed;
            }
        }
        else
        {
            particles.Stop();
        }
    }





}
