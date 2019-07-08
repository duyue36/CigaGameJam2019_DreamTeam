using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public int PlayerID;
    private bool hasTriggerOnce;

    [CanBeNull] public ParticleSystem HeartParticleSystem;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Boss")
        {
            if (!hasTriggerOnce)
            {
                if (HeartParticleSystem != null)
                {
                    HeartParticleSystem.Play();
                }
                SoundManager.Singleton.PlayBabyCry();
                GameManager.Singleton.PlayerWin(PlayerID);

                hasTriggerOnce = true;
            }
            
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
       
    //    if (other.gameObject.tag == "Boss")
    //    {
    //        if (!hasTriggerOnce)
    //        {
    //            HeartParticleSystem.Play();
    //            SoundManager.Singleton.PlayBabyCry();
    //            GameManager.Singleton.PlayerWin(PlayerID);

    //            hasTriggerOnce = true;
    //        }
            
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
