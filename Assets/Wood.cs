using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (!other.GetComponent<PlayerController>().carryingWood)
            {
                other.GetComponent<PlayerController>().CarryWood();
                Destroy(gameObject);
            }
        }
    }

}
