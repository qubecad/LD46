using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Actions : MonoBehaviour
{
    public void StartGame()
    {

        

        FMODUnity.RuntimeManager.LoadBank("Master");
        //SceneManager.LoadScene(charGeneration);
        StartCoroutine(ChangeScene(1));
    }


    IEnumerator ChangeScene(int buildID)
    {
        yield return new WaitForSeconds(1);

        if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            SceneManager.LoadScene(buildID);

        }
        else
        {
            Debug.Log("Master bank not loaded looping");
            StartCoroutine(ChangeScene(buildID));
        }


    }
}
