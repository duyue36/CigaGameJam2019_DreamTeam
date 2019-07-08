using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
    
public class StartButton : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("SC_Main");
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
