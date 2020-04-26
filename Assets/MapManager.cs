
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject floorPrefab;
    public GameObject firePrefab;
    public GameObject playerPrefab;

    public GameObject[] edgePrefabs;

    public GameObject[] itemPrefabs;

    public GameObject tilePrefab;

    public Transform fireTransform;
    public Slider BodyHeatSlider;
    public Slider FoodSlider;
    public Slider FireFuelSlider;
    public Slider GameTimeSlider;
    public Text GameOverText;
    public GameObject gameOverPanel;


    public ParticleSystem snowParticles;
    public GameObject UIPanel;

    [FMODUnity.EventRef]
    public string ControlMessageWin;

    [FMODUnity.EventRef]
    public string ControlMessageLose;

    private Transform itemsRootTransform;
    public bool gameEnded = false;



    public int totalFloorCount;
    [Range(0, 100)]
    public int itemSpawnPercent;

    public float minX, minZ, maxX, maxZ;

    private List<Vector3> floorList = new List<Vector3>();
    private Coroutine coroutine;
    private float gameTimer;
    private float timeBarUpdateTimer=0f;
    LayerMask groundMask;

    private void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        itemsRootTransform = GameObject.Find("Items").transform;
        gameOverPanel.SetActive(false);
        RandomWalker();

        Coroutine co=StartCoroutine(EnQueueGameEnd());

        
    }





    void RandomWalker()
    {
        Vector3 curPos = Vector3.zero;

        floorList.Add(curPos);
        while (floorList.Count < totalFloorCount)
        {

            switch (Random.Range(1, 5))
            {

                case 1:
                    curPos += Vector3.forward;
                    break;

                case 2:
                    curPos += Vector3.right;
                    break;

                case 3:
                    curPos += Vector3.back;
                    break;

                case 4:
                    curPos += Vector3.left;
                    break;



            }
            bool inFloorList = false;
            for (int i = 0; i < floorList.Count; i++)
            {


                if (Vector3.Equals(curPos, floorList[i]))
                {
                    inFloorList = true;
                    break;
                }
            }

            if (!inFloorList)
            {
                floorList.Add(curPos);
            }




        }

        for (int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tilePrefab, floorList[i], Quaternion.identity) as GameObject;

            goTile.name = tilePrefab.name;
            goTile.transform.SetParent(transform);

        }
        StartCoroutine(DelayProgress());
    }


    IEnumerator DelayProgress()
    {
        while (FindObjectsOfType<TileSpawner>().Length > 0)
        {
            yield return null;
        }
        for (int x = (int)minX; x <= maxX + 2; x++)
        {

            for (int z = (int)minZ; z <= maxZ + 2; z++)
            {
                Collider[] hit = Physics.OverlapBox(new Vector3(x, 0, z), transform.localScale / 4, Quaternion.identity, groundMask);



                if (hit != null && hit.Length > 0)
                {

                    if (Random.Range(0, 101) < itemSpawnPercent)
                    {

                        int i = Random.Range(0, itemPrefabs.Length);
                        GameObject goTile = Instantiate(itemPrefabs[i], hit[0].transform.position, Quaternion.identity) as GameObject;
                        goTile.transform.SetParent(itemsRootTransform);
                    }
                }

            }

        }

        PlaceStart();

    }

    private void PlaceStart()
    {



        Vector3 firePos = floorList[0];
        Vector3 playerPos = floorList[1];


        GameObject fire = Instantiate(firePrefab, firePos, Quaternion.identity) as GameObject;

        fire.name = firePrefab.name;

        fireTransform = fire.transform;

        GameObject player = Instantiate(playerPrefab, playerPos, Quaternion.identity) as GameObject;

        player.name = playerPrefab.name;

        Camera.main.transform.position = new Vector3(playerPos.x, Camera.main.transform.position.y, playerPos.z);

        Transform SnowTransform = GameObject.Find("SnowParticleSystem").transform;

        SnowTransform.position = new Vector3(playerPos.x, SnowTransform.position.y, playerPos.z);
    }

    public void EndGameLose(string reason)
    {

        if (!gameEnded)
        {
            UIPanel.GetComponent<Animator>().SetTrigger("GameOver");

            FMODUnity.RuntimeManager.PlayOneShot(ControlMessageLose);
            GameOverText.text = reason;

            gameEnded = true;
            gameOverPanel.SetActive(true);
            StopAllCoroutines();

            FindObjectOfType<WeatherManager>().StopAllCoroutines();

        }
    }

    public void EndGameWin(string reason)
    {

        if (!gameEnded)
        {

            UIPanel.GetComponent<Animator>().SetTrigger("GameOver");

            FMODUnity.RuntimeManager.PlayOneShot(ControlMessageWin);
            GameOverText.text = reason;
            gameEnded = true;
            gameOverPanel.SetActive(true);
            FindObjectOfType<WeatherManager>().StopAllCoroutines();

        }
    }

    IEnumerator EnQueueGameEnd()
    {

        yield return new WaitForSecondsRealtime(300f);

        if (!gameEnded)
        {

            EndGameWin("You managed to stay alive long enough for help to find you. ");

        }

    }

    private void Update()
    {
        gameTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)){

            GameTimeSlider.gameObject.SetActive(!GameTimeSlider.gameObject.activeInHierarchy);

        }




        if (timeBarUpdateTimer > 1)
        {

            timeBarUpdateTimer = 0f;
           GameTimeSlider.value = 300-gameTimer;

        }
        else
        {
            timeBarUpdateTimer += Time.deltaTime;
        }




    }


}
