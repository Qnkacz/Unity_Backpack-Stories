using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public static StaminaSlider StaminaSliderInstance;
    private Slider slider;
    public ParticleSystem particles;
    bool playparticles;
    private void Awake()
    {
       
        slider = this.gameObject.GetComponent<Slider>();
    }
    void Start()
    {
        playparticles = false;
        StaminaSliderInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBarValue();
    }
    
    public void ChangeBarValue()
    {
        if (BossScript.instance.currStamina != slider.value)
        {
            if (slider.value - BossScript.instance.currStamina < 0.2)
            {
                slider.value = BossScript.instance.currStamina;
            }
            if (slider.value < BossScript.instance.currStamina)
            {
               // Debug.Log("powinienem zwiekszac");
                slider.value += Time.deltaTime * BossScript.instance.fillSpeed;
            }
            if (slider.value > BossScript.instance.currStamina)
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
