using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class VolcanoAshSpawner : MonoBehaviour {

    public float decoSpawnCnt = 2;
    public float decoDelay = 0.3f;
    float beginDecoDelay;
    public float spawnDelay = 0.2f;
    float beginSpawnDelay;
    public float spawnNearDelay = 0.2f;
    float beginNearDelay;
    public GameObject prefab;
    public bool isEnableNearSpawn = false;

    public Transform spawnTransform;
    public Transform playerTransform;
    Vector3 targetVector;

    float distanceFromPlayer;
    float beginDistance;

    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    public float height = 10f;
    public float speedMultiply = 1f;

    float h;

    float distanceRatio;
    float destHeight;
    float destSpeedMultiply;

    public bool isTitle = false;

    // Use this for initialization
    void Start () {
        if (playerTransform != null)
            beginDistance = Vector3.Distance(transform.position, playerTransform.position);
        else
            beginDistance = 1f;
        beginSpawnDelay = spawnDelay;
        beginDecoDelay = decoDelay;
        beginNearDelay = spawnNearDelay;

        StartCoroutine(CoCalculate());
        StartCoroutine(CoSpawn());
        StartCoroutine(CoSpawnDeco());
        StartCoroutine(CoSpawnNearByPlayer());
    }
                                                                                                               
    // Update is called once per frame                                                                         
    void Update () {                                                                                           
        h = Input.GetAxis("Horizontal");                                                                       
    }                                                                                                          
                                                                                                               
    IEnumerator CoCalculate()                                                                                  
    {                                                                                                          
        distanceFromPlayer = Vector3.Distance(spawnTransform.position, playerTransform.position);              
        distanceRatio = distanceFromPlayer / beginDistance;                                                    
                                                                                                               
        destHeight = Mathf.Clamp(height * distanceRatio, 80f, 150f);                                           
        destSpeedMultiply = 1f + (1f - distanceRatio);                                                         
                                                                                                               
        spawnDelay = Mathf.Clamp(beginSpawnDelay * distanceRatio, 0.05f, 1f);                                  
        decoDelay = Mathf.Clamp(beginDecoDelay * distanceRatio, 0.05f, 1f);                                    
        spawnNearDelay = Mathf.Clamp(beginNearDelay * distanceRatio, 0.05f, 1f);
        

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CoCalculate());
    }

    IEnumerator CoSpawnDeco()
    {
        yield return new WaitForSeconds(decoDelay);
        for (int i = 0; i < decoSpawnCnt; i++)
        {
            //GameObject ashDeco = Instantiate(prefab, spawnTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
            Transform ashDeco = PoolManager.Pools["VolcanoAshPool"].Spawn(prefab, spawnTransform.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f)), Quaternion.identity);
            Rigidbody ashRigidbody = ashDeco.GetComponent<Rigidbody>();
            ashRigidbody.AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
            VolcanoAsh volcanoAsh = ashDeco.GetComponent<VolcanoAsh>();

            volcanoAsh.Init(new Vector3(Random.Range(-200f, 200f) + (h * 20f), y, Random.Range(-200f, 50f)), Random.Range(150f, 200f), speedMultiply * 2f);

        }
        StartCoroutine(CoSpawnDeco());
    }

    IEnumerator CoSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        //GameObject ash = Instantiate(prefab, spawnTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
        Transform ash = PoolManager.Pools["VolcanoAshPool"].Spawn(prefab, spawnTransform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), Quaternion.identity);
        Rigidbody ashRigidbody = ash.GetComponent<Rigidbody>();
        ashRigidbody.AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
        VolcanoAsh volcanoAsh = ash.GetComponent<VolcanoAsh>();
        volcanoAsh.Init(new Vector3(playerTransform.position.x + Random.Range(-40f, 40f) + (h * 20f), y, -Random.Range(distanceFromPlayer - 100f, distanceFromPlayer - 40f)), 200f, destSpeedMultiply);


        StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawnNearByPlayer()
    {
        yield return new WaitForSeconds(spawnNearDelay);
        if(isEnableNearSpawn)
        {
            //GameObject ashForPlayer = Instantiate(prefab, spawnTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
            Transform ashForPlayer = PoolManager.Pools["VolcanoAshPool"].Spawn(prefab, spawnTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
            Rigidbody ashRigidbody = ashForPlayer.GetComponent<Rigidbody>();
            ashRigidbody.AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
            VolcanoAsh volcanoAsh = ashForPlayer.GetComponent<VolcanoAsh>();

            volcanoAsh.Init(new Vector3(playerTransform.position.x + Random.Range(-20f, 20f) + (h * 20f), y, -Random.Range(distanceFromPlayer - 60, distanceFromPlayer - 10f)), destHeight, destSpeedMultiply);
        }
        if (isTitle == false)
            StartCoroutine(CoSpawnNearByPlayer());
    }

    void Spawn()
    {

    }

}
