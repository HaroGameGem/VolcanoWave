using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject btnGameStart;
    public GameObject[] arrHpObject = new GameObject[5];
    public Slider staminaSlider;

    public Sprite[] sprNum;
    public SpriteRenderer waveNumRenderer;

    PlayerCtrl player;

    static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    GameManager gameManager;

    // Use this for initialization
    void Start () {
        gameManager = GameManager.Instance;
        if(gameManager.player != null)
        {
            player = gameManager.player.GetComponent<PlayerCtrl>();
            player.EventPlayerTakeDamage += OnPlayerTakeDamage;
            staminaSlider.maxValue = staminaSlider.value = player.maxStamina;
        }

        waveNumRenderer.sprite = sprNum[DataManager.Instance.Level];
	}

    private void FixedUpdate()
    {
        if(player != null)
        {
            staminaSlider.value = player.stamina;
        }
    }

    private void OnPlayerTakeDamage(int hp)
    {
        int idx = 5 - 1 - hp;
        arrHpObject[idx].SetActive(false);
    }

    public void OnBtnClickedTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void OnBtnClickedReplay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnBtnClickedNextWave()
    {
        DataManager.Instance.AddLevel();
        SceneManager.LoadScene(1);
    }

}
