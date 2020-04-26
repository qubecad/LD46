using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public ParticleSystem snowParticleSystem;

    [FMODUnity.EventRef]
    public string weatherSound;
    EventInstance weatherSoundInstance;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Snowing Queued");
        StartCoroutine(Weather());

        weatherSoundInstance = FMODUnity.RuntimeManager.CreateInstance(weatherSound);

    }

   

    IEnumerator Weather()
    {

        float s = Random.Range(10, 30);

        Debug.Log("Snowing in " + s + " seconds");

        yield return new WaitForSeconds(s);


        snowParticleSystem.Play();
        weatherSoundInstance.start();

        StartCoroutine(CheckWeather());

        FindObjectOfType<CampFire>().Snowing();


    }

    IEnumerator CheckWeather()
    {
        yield return new WaitForSeconds(5f);


        
            snowParticleSystem.Stop();
        StartCoroutine(Weather());
        
    }

    private void Update()
    {
       
    }



}
