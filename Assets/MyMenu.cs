using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyMenu : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene("ColliderTest");
    }
       public void ExitScene()  {
        Application.Quit();}
    
}
