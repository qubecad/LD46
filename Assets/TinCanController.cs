using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinCanController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().foodLevel += 0.5f;
            Destroy(gameObject);
        }
    }

}
