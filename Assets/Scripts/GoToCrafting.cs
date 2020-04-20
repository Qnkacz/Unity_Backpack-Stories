using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCrafting : MonoBehaviour
{
    public GameObject CraftingTable;
    private GameObject spawnedCraftingTable;
    public bool isOnCrafting = false;
    public static GoToCrafting instance;
    public GameObject HPBar;
    public GameObject StaminaBar;
    public GameObject SanityBar;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isOnCrafting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c") && isOnCrafting == false)
        {
            Camera.main.transform.position = new Vector3(9f, 2.79f, -10f);
            spawnedCraftingTable = (GameObject)Instantiate(CraftingTable,new Vector3(15.6f,1.5f, 0), Quaternion.identity);
            isOnCrafting = true;
            HPBar.SetActive(false);
            StaminaBar.SetActive(false);
            SanityBar.SetActive(false);
        }
        else if(Input.GetKeyDown("c") && isOnCrafting == true)
        {
            Camera.main.transform.position = new Vector3(3.55f, 2.79f, -10f);
            isOnCrafting = false;
            Destroy(spawnedCraftingTable);

            HPBar.SetActive(true);
            StaminaBar.SetActive(true);
            SanityBar.SetActive(true);
        }
    }
}
