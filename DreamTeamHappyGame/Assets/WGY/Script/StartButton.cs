﻿using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadMainScene();
        }
    }
}
