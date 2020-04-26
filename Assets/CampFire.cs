using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour


{


    public float fireLevel = 100;
    public float fireCoolDowntimer = 0f;
    public float wildAnimalCoolDowntimer = 0f;
    public float playerWarningCoolDowntimer = 0f;
    [FMODUnity.EventRef]
    public string wildAnimalSound;

    [FMODUnity.EventRef]
    public string playerWarningSound;

    ParticleSystem fireParticleSystem;
    MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        fireParticleSystem = GetComponentInChildren<ParticleSystem>();

        mapManager = FindObjectOfType<MapManager>();

    }

    // Update is called once per frame
    void Update()

    {

        if (mapManager.gameEnded)
        {

            return;
        }


        if (fireCoolDowntimer > 1)
        {
            fireLevel -= 0.5f;
            fireCoolDowntimer = 0f;
            ParticleSystem.EmissionModule emissionModule = fireParticleSystem.emission;
            emissionModule.rateOverTime = fireLevel;

            mapManager.FireFuelSlider.value = fireLevel;

            Debug.Log("Fire Level Slider :"+mapManager.FireFuelSlider.value);
        }
        else
        {
            fireCoolDowntimer += Time.deltaTime;
        }

        if (fireLevel < 25)
        {
            if (wildAnimalCoolDowntimer > 5f)
            {

                wildAnimalCoolDowntimer = 0f;

                FMODUnity.RuntimeManager.PlayOneShot(wildAnimalSound, transform.position);

            }
            else
            {
                wildAnimalCoolDowntimer += Time.deltaTime;
            }


        }
        if (fireLevel < 40)
        {
            if (playerWarningCoolDowntimer > 10f)
            {

                playerWarningCoolDowntimer = 0f;

                FMODUnity.RuntimeManager.PlayOneShot(playerWarningSound);

            }
            else
            {
                playerWarningCoolDowntimer += Time.deltaTime;
            }

        }



        if (fireLevel <= 0)
        {
            Debug.Log("Fire has gone out");
            FindObjectOfType<MapManager>().EndGameLose("The Fire has gone out.");

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>().carryingWood)
        {

            fireLevel += 10f;
            other.GetComponent<PlayerController>().DropWood();
        }
    }

    public void Snowing()
    {
        fireLevel -= 5f;
    }




}
