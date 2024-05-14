using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    public void ToMainmenu()
    {

        SceneManager.LoadScene(0);

    }
    
    public void loadLevel(int index)
    {

        SceneManager.LoadScene(index);

    }
    
     
    public void closeApp()
    {

         Application.Quit();

    }
}
