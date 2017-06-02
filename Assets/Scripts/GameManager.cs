using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    public event Action EventGameStart;
    public event Action EventGameOver;

    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    public bool isGod = false;
    public int debugLevel = 0;

    [HideInInspector]
    public VolcanoAshSpawner spawner;
    [HideInInspector]
    public UIManager uiManager;
    [HideInInspector]
    public DataManager dataManager;

    public GameObject player;
    [HideInInspector]
    public PlayerCtrl playerCtrl;

    public bool isPlaying;

    float playTime = 0f;
    float shakingDelay = 0f;
    float shakingAmount = 0f;
    public float beginShakingAmount = 0f;
    public float shakingAddAmount = 0.1f;
    public float shakingAddDelay = 7f;

    public GameObject Clear;
    public GameObject Fail;
    CameraCtrl cameraCtrl;

    // Use this for initialization
    void Start () {
        spawner = GameObject.FindObjectOfType(typeof(VolcanoAshSpawner)) as VolcanoAshSpawner;
        uiManager = UIManager.Instance;
        dataManager = DataManager.Instance;

        EventGameStart += OnGameStart;
        if(player != null)
        {
            playerCtrl = player.GetComponent<PlayerCtrl>();
            playerCtrl.EventPlayerDie += OnGameOver;
        }

        cameraCtrl = Camera.main.GetComponent<CameraCtrl>(); 

        OnClickedBtnGameStart();
	}

    public GameObject[] arrTreePrefab;
    public GameObject treesObject;
    public int spawnTreeCnt = 200;

    public void SpawnTrees()
    {

        for (int i = 0; i < spawnTreeCnt; i++)
        {
            Vector3 randomNormalVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 0f)).normalized;
            Vector3 spawnPos = randomNormalVector * UnityEngine.Random.Range(100f, 280f);
            spawnPos.y -= 1f;
            Transform tr = GameObject.Instantiate(arrTreePrefab[UnityEngine.Random.Range(0, 5)], spawnPos, Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f))).transform;
            tr.parent = treesObject.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(isPlaying)
        {
            playTime += Time.smoothDeltaTime;
            ShakeCamera();
        }
	}

    public void OnClickedBtnGameStart()
    {
        EventGameStart();
    }

    public void OnGameStart()
    {
        int level = dataManager.Level;
        if (debugLevel > 0)
            level = debugLevel;

        //변수 설정
        spawner.decoSpawnCnt = 2;       //high 어려움
        spawner.decoDelay = Mathf.Clamp(0.2f - (level * 0.02f) + spawner.decoSpawnCnt * 0.04f, 0.05f, 3f);       //low 어려움
        spawner.spawnDelay = Mathf.Clamp(0.2f - (level * 0.02f), 0.05f, 3f);      //low 어려움
        spawner.spawnNearDelay = Mathf.Clamp(0.2f - (level * 0.02f), 0.05f, 3f);  //low 어려움

        if (level > 3)
        {
            spawner.isEnableNearSpawn = true;
        }

        if(level > 4)
        {
            spawner.decoSpawnCnt = 2 + level / 4;
        }

        beginShakingAmount = Mathf.Clamp((level / 2) * 0.1f, 0f, 1f);      //high 어려움
        shakingAddAmount = Mathf.Clamp((level / 3) * 0.1f, 0f, 1f);        //high 어려움
        shakingAddDelay = Mathf.Clamp(8f - (level / 2) * 0.5f, 4f, 15f);           //low 어려움

        shakingAmount = beginShakingAmount;


        spawnTreeCnt = Mathf.Clamp(100 + (level * 20), 50, 400); //high 어려움

        SpawnTrees();

        if(playerCtrl != null)
        {
            playerCtrl.maxStamina = Mathf.Clamp(40f - (level * 3f), 15f, 50f);
            playerCtrl.staminaRestoreAmount = Mathf.Clamp(3f - (level * 0.2f), 1f, 5f);
            playerCtrl.staminaConsumeAmount = Mathf.Clamp(0.5f + (level * 0.2f), 0f, 3f);
            playerCtrl.staminaDashAmount = Mathf.Clamp(4f + (level * 0.1f), 2f, 8f);
        }

        isPlaying = true;
    }

    public void GameWin()
    {
        if (isPlaying == false)
            return;
        isPlaying = false;
        Clear.SetActive(true);
    }

    public void OnGameOver()
    {
        if (isPlaying == false)
            return;
        isPlaying = false;
        Fail.SetActive(true);
    }

    public void ShakeCamera()
    {
        shakingDelay += Time.smoothDeltaTime;
        if (shakingDelay > shakingAddDelay)
        {
            shakingDelay = 0f;
            shakingAmount += shakingAddAmount;
            cameraCtrl.shake = 1f;
            cameraCtrl.shakeAmount = shakingAmount;
        }
    }

}
