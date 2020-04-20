using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    #region Variables
    public float MaxHP, MaxMana, MaxStamina;
    public float StartHP, StartMana, StartStamina;
    public float currHP, currMana, currStamina;
    public float time;
    private float timeleft;
    public int difficulty;
    public float fillSpeed;
    public int itemToGive;
    public int itemCount;
    public GameObject[] itemlist;
    #endregion

    #region obj
    public Slider HPBar;
    public Slider ManaBar;
    public Slider Staminabar;
    public Slider TimerBar;
    public static BossScript instance;
    public GameObject EndScreen;
    public GameObject testnewItem;
    public GameObject textBox;
    public GameObject Spirit;
    public GameObject audio;
   
    #endregion

    #region particles
    public ParticleSystem HPParticles;
    public ParticleSystem ManaParticles;
    public ParticleSystem StaminaParticles;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        itemCount = 0;
        Startrup();
        StartCoroutine(StaminaCheck(5));
    }

    // Update is called once per frame
    void Update()
    {
        timerFill();
        if(currMana <=0 || currHP <= 0 || currStamina <= 0)
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            audio.GetComponent<AudioSource>().Stop();
            EndScreen.SetActive(true);
            GameObject UI = GameObject.Find("GameUI");
            UI.SetActive(false);
            GameObject CandyCrush = GameObject.Find("CandyCrush");
            CandyCrush.SetActive(false);
            GameObject backgroud = GameObject.Find("background Scaler"); 
            backgroud.SetActive(false);
            currHP = currMana = currStamina = -1;
        }
       
    }
    private void Startrup()
    {
        instance = this;

        switch (difficulty)
        {
            case 1:
                MaxHP = MaxMana = MaxStamina = 100;
                StartHP = StartMana = StartStamina = 100;
                currHP = currMana = currStamina = 100;
                break;
            case 2:
                MaxHP = MaxMana = MaxStamina = 70;
                StartHP = StartMana = StartStamina = 70;
                currHP = currMana = currStamina = 70;
                break;
            case 3:
                MaxHP = MaxMana = MaxStamina = 50;
                StartHP = StartMana = StartStamina = 50;
                currHP = currMana = currStamina = 50;
                break;
        }
        TimerBar.maxValue = time;
        HPBar.maxValue = MaxHP;
        ManaBar.maxValue = MaxMana;
        Staminabar.maxValue = MaxStamina;
        
        timeleft = time;
        HPBar.value = MaxHP;
        ManaBar.value = MaxMana;
        Staminabar.value = MaxStamina;
        GenerateNeededItem();
    }
    private void timerFill()
    {
        if(timeleft>0)
        {
            timeleft -= Time.deltaTime;
            TimerBar.value = timeleft;
        }
        else
        {
            //Debug.Log("isgiven is: "+Spirit.GetComponent<Hand>().isGiven);
            if (Spirit.GetComponent<Hand>().isGiven == false)
            {
                currHP -= 10;
            }
            else Spirit.GetComponent<Hand>().isGiven = false;
            TimerBar.value = timeleft;
            timeleft = time;
            GenerateNeededItem();
           
        }
    }

    private void GiveHP(float value)
    {
        currHP += value;
    }

    public void test_GivePoints()
    {
        
            currHP += 10;
            currMana += 10;
            currStamina+= 10;
        GameObject newItem = Instantiate(testnewItem, new Vector3(Random.Range(4, 10), 6, 0), Quaternion.identity);
    }

    public void test_SubstractPoints()
    {
       
            currHP -= 10;
            currMana -= 10;
            currStamina -= 10;
        
    }

    string GenerateNeededItem()
    {
        string requestedItemname;
        int neededItem = (int)Random.Range(0,itemlist.Length);
        switch (neededItem)
        {
            case 0:
                requestedItemname = "An arrow!";
                itemToGive = 0;
                break;
            case 1:
                requestedItemname = "A bone!";
                itemToGive = 1;
                break;
            case 2:
                requestedItemname = "An empty bottle!";
                itemToGive = 2;
                break;
            case 3:
                requestedItemname = "A empty bowl!";
                itemToGive = 3;
                break;
            case 4:
                requestedItemname = "A feather!";
                itemToGive = 4;
                break;
            case 5:
                requestedItemname = "Herbs!";
                itemToGive = 5;
                break;
            case 6:
                requestedItemname = "An ink bottle!";
                itemToGive = 6;
                break;
            case 7:
                requestedItemname = "Some metal!";
                itemToGive = 7;
                break;
            case 8:
                requestedItemname = "An onion!";
                itemToGive = 8;
                break;
            case 9:
                requestedItemname = "Some paper!";
                itemToGive = 9;
                break;
            case 10:
                requestedItemname = "A hope rope!";
                itemToGive = 10;
                break;
            case 11:
                requestedItemname = "A scroll!";
                itemToGive = 11;
                break;
            case 12:
                requestedItemname = "A bowl of Soup!";
                itemToGive = 12;
                break;
            case 13:
                requestedItemname = "A stick!";
                itemToGive = 13;
                break;
            case 14:
                requestedItemname = "A stone!";
                itemToGive = 14;
                break;
            case 15:
                requestedItemname = "A sword!";
                itemToGive = 15;
                break;
            default:
                requestedItemname = "your attention! </3";
                itemToGive = 6;
                break;
        }
        textBox.GetComponent<Text>().text = requestedItemname;
        return requestedItemname;

    }
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public string GetItemName(GameObject obj)
    {
        return obj.name;
    }

    
    public void SanityCheck(int val,bool ifadd)
    {
        if(ifadd==true)
        {
            if(currMana+val<MaxMana)
            {
                currMana += val;
            }
            if(currMana+val>MaxMana)
            {
                currMana = MaxMana;
            }
        }
        else
        {
            currMana -= val;
        }
    }
    private IEnumerator StaminaCheck(int seconds)
    {
        //Debug.Log("zaczynam ienumeratora");
        int counter = seconds;
        while(counter>=0)
        {
            //Debug.Log("wszedlem w while");
            StaminaRegulator();
            yield return new WaitForSecondsRealtime(5f);
            counter--;
            if(counter==0)
            {
                
                counter = 5;
            }
        }
        StaminaRegulator();
    }
    private void StaminaRegulator()
    {
        if (itemCount > 10)
        {
            currStamina -= 10;
            //Debug.Log("powinno zmniejszac stamine");
        }
        else
        {
            if (currStamina + 10 > MaxStamina)
            {
                currStamina = MaxStamina;
            }
            else
            {
                currStamina += 10;
            }
            //Debug.Log("powinno zwiekszac stamine");
        }
    }
}
