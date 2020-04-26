using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour


 

{


   

    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;
    public float playerBodyHeat;
    public float altSpeed = 0.1f;

   

    public float foodLevel;
    public float startingFood;
    public float startingHeat;
    public bool carryingWood = false;

    public float foodCoolDowntimer;

    [FMODUnity.EventRef]
    public string footStepsSound;
    [FMODUnity.EventRef]
    public string startingDialog;

    [FMODUnity.EventRef]
    public string playerDialog2;

    [FMODUnity.EventRef]
    public string playerDialog3;

    [FMODUnity.EventRef]
    public string woodDialog;



    public GameObject CarryingWoodModel;


    private EventInstance footStepEvent;
    private MapManager mapManager;
    private bool logSoundPlayed;
    private bool altControlMethod = false;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        foodLevel = startingFood;
        playerBodyHeat = startingHeat;

       // mapManager.BodyHeatText.text = "BodyHeat: " + playerBodyHeat;
        //mapManager.FoodText.text = "Food: " + foodLevel;

        mapManager = FindObjectOfType<MapManager>();

        footStepEvent = FMODUnity.RuntimeManager.CreateInstance(footStepsSound);
        footStepEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        StartCoroutine(StartingDialog());
    }

    // Update is called once per frame
    void Update()
    {

        if (mapManager.gameEnded)
        {

            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            altControlMethod = !altControlMethod;
        }


        CharacterController controller = GetComponent<CharacterController>();


        if (altControlMethod)
        {
            Debug.Log("Using Alt Control Method");
            float horz = Input.GetAxisRaw("Horizontal") ;
            float vert = Input.GetAxisRaw("Vertical");
           Vector3 moveDirection = new Vector3(horz, 0.0f, vert);
            moveDirection *= speed;
            controller.Move(moveDirection*Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(moveDirection);


            if (Mathf.Abs(horz) > 0|| Mathf.Abs(vert) > 0)
            {

                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }


        }
        else
        {


            // Rotate around y - axis
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

            // Move forward / backward
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            float curSpeed = speed * Input.GetAxis("Vertical");
            controller.SimpleMove(forward * curSpeed);

            if (Mathf.Abs(curSpeed) > 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
            
           
       
       

        Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);

        float distanceToCampFire=Vector3.Distance(transform.position, mapManager.fireTransform.position);

        if (distanceToCampFire > 5)
        {
            playerBodyHeat -= 0.01f;
            mapManager.BodyHeatSlider.value = playerBodyHeat;
        }
        else if(distanceToCampFire<2 && playerBodyHeat < 100)
        {
            playerBodyHeat += 0.01f;
            mapManager.BodyHeatSlider.value = playerBodyHeat;
        }else if (playerBodyHeat <= 0)
        {


            Debug.Log("Player Froze");
            mapManager.EndGameLose("You Froze.");
        }

        if (foodCoolDowntimer > 1)
        {
            foodLevel -= 0.2f;
            foodCoolDowntimer = 0f;
            mapManager.FoodSlider.value = foodLevel;

        }
        else
        {
            foodCoolDowntimer += Time.deltaTime;
        }

        if (foodLevel <= 0)
        {

            Debug.Log("Player starved");
            mapManager.EndGameLose("You  ran out of Food.");
        }

    }
    public void PlayFootSteps()
    {

        if (!footStepEvent.IsPlaying())
        {
            footStepEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

            footStepEvent.start();
        }
    }

    internal void CarryWood()
    {
        carryingWood = true;
        CarryingWoodModel.SetActive(true);

        if (!logSoundPlayed)
        {
            FMODUnity.RuntimeManager.PlayOneShot(woodDialog);
            logSoundPlayed = true;
        }

    }

    internal void DropWood()
    {
       animator.SetTrigger("Kneel");
        carryingWood = false;
      
    }

    public void HideWood()
    {
        CarryingWoodModel.SetActive(false);
    }


    IEnumerator StartingDialog()
    {
        yield return new WaitForSeconds(5f);


        FMODUnity.RuntimeManager.PlayOneShot(startingDialog);
    }
}
