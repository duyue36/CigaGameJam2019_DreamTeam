using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;
    
    private void Awake()
    {
        Singleton = this;
    }

    public AudioSource FistAudioSourcePrefab;
    public AudioSource FistAudioSourcePrefab2;
    
    public AudioSource BabyCryAudioSourcePrefab;
    public AudioSource BabyLaughAudioSourcePrefab2;
    public void PlayFist()
    {
        var fistAS = GameObject.Instantiate(FistAudioSourcePrefab);
        fistAS.Play();
        Destroy(fistAS.gameObject,fistAS.clip.length);
    }
    
     
    public void PlayFist2()
    {
        var fistAS = GameObject.Instantiate(FistAudioSourcePrefab2);
        fistAS.Play();
        Destroy(fistAS.gameObject,fistAS.clip.length);
    }
    
    public void PlayBabyCry()
    {
        if (!BabyCryAudioSourcePrefab.isPlaying)
        {
            BabyCryAudioSourcePrefab.loop = true;
            BabyCryAudioSourcePrefab.Play();
        }

//        var fistAS = GameObject.Instantiate(BabyCryAudioSourcePrefab);
//        fistAS.Play();
//        Destroy(fistAS.gameObject,fistAS.clip.length);
    }
    
    public void PlayBabyLaugh()
    {
        if (!BabyLaughAudioSourcePrefab2.isPlaying)
        {
            BabyLaughAudioSourcePrefab2.loop = true;
            BabyLaughAudioSourcePrefab2.Play();
        }
//        var fistAS = GameObject.Instantiate(BabyLaughAudioSourcePrefab2);
//        fistAS.Play();
//        Destroy(fistAS.gameObject,fistAS.clip.length);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
