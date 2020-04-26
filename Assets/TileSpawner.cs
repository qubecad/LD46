using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{


    MapManager mapManager;
    GameObject environmentRoot;


    // Start is called before the first frame update
    void Awake()
    {
        mapManager=FindObjectOfType<MapManager>();
        environmentRoot = GameObject.Find("Environment");



        GameObject goFloor = Instantiate(mapManager.floorPrefab, transform.position, Quaternion.identity) as GameObject;
        goFloor.name=mapManager.floorPrefab.name;
        goFloor.transform.SetParent(environmentRoot.transform);


        if (transform.position.x > mapManager.maxX)
        {
            mapManager.maxX = transform.position.x;
        }

        if (transform.position.x < mapManager.minX)
        {
            mapManager.minX = transform.position.x;
        }

        if (transform.position.z > mapManager.maxZ)
        {
            mapManager.maxZ = transform.position.z;
        }

        if (transform.position.z < mapManager.minZ)
        {
            mapManager.minZ = transform.position.z;
        }



    }

    private void Start()
    {
        LayerMask envMask = LayerMask.GetMask("Ground", "Edges");


        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1 ; z <= 1; z++)
            {
                Vector3 targetPos = new Vector3(transform.position.x + x, 0,transform.position.z + z);

                Collider[] hit = Physics.OverlapBox(targetPos, transform.localScale / 4, Quaternion.identity, envMask);

                
               
                if (hit==null||hit.Length==0)
                {
                    // add an edge

                    int i = Random.Range(0, mapManager.edgePrefabs.Length);

                    GameObject goEdge = Instantiate(mapManager.edgePrefabs[i], targetPos, Quaternion.identity) as GameObject;
                    goEdge.name = mapManager.edgePrefabs[i].name;
                    goEdge.transform.SetParent(environmentRoot.transform);
              
                }

            }
        }



        Destroy(gameObject);

    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawCube(transform.position, Vector3.one);
    }




}
