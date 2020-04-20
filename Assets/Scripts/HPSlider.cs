using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HPSlider : MonoBehaviour
{
    public static HPSlider HPSliderInstance;
    private Slider slider;
    public ParticleSystem particles;
    // Start is called before the first frame update
    private void Awake()
    {
        
        slider = this.gameObject.GetComponent<Slider>();
    }
    void Start()
    {
        HPSliderInstance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBarValue();
    }

    public void ChangeBarValue()
    {
        if (BossScript.instance.currHP != slider.value)
        {
            particles.Play();
            if(slider.value-BossScript.instance.currHP<0.2)
            {
                slider.value = BossScript.instance.currHP;
            }
            if (slider.value < BossScript.instance.currHP)
            {
               // Debug.Log("powinienem zwiekszac");
                slider.value += Time.deltaTime * BossScript.instance.fillSpeed;
            }
            if (slider.value > BossScript.instance.currHP)
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
