using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTillEnd : MonoBehaviour
{
    public UnityEngine.UI.Text czas;
    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator StartCountdown(float countdownValue = 120)
    {


        float currCountdownValue = countdownValue;
        while (currCountdownValue >= 0)
        {
            if (currCountdownValue == 0)
            {
                gameOverScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                czas.text = currCountdownValue.ToString();
                //Debug.Log("Countdown: " + currCountdownValue);
                yield return new WaitForSeconds(1.0f);
                currCountdownValue--;
            }
        }


    }

}
