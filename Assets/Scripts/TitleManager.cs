using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    public GameObject HelpUI;

    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        DataManager.Instance.ResetLevel();
    }

    public void OnBtnClickedInStartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnBtnClickedHelpButton()
    {
        if (HelpUI.activeSelf == false)
        {
            HelpUI.SetActive(true);
        }
        else
        {
            HelpUI.SetActive(false);
        }
    }
}
