using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HIV;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<StickMan>().enabled = false;
        FindObjectOfType<StickMan2>().enabled = false;
    }

    public void PlayerWin(int PlayerID)
    {
        Debug.Log("Player " + PlayerID+" Win!!!");

        

        //Invoke("ReloadScene",3f);
        if(PlayerID == 1)
            PolicySystem.Instance.ExecuteWinnersPolicy(Winner.Player1);
        else
            PolicySystem.Instance.ExecuteWinnersPolicy(Winner.Player2);


        PolicySystem.Instance.NextStep();
    }



    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
            .name);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                .name);
        }
    }
}
