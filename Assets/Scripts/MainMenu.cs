using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //
    public static MainMenu instance;
    private void Awake()
    {
        instance = this;
    }
    public void PlayGameStory()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1 );
    }
    public void PlayGameFreemode()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    private void Update()
    {
        
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    //private void OnMouseDown()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log("kliknałeś");
    //        playmodes.SetActive(false);
    //        options.SetActive(false);
    //    }
    //}

}
