using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
    public GameObject pauseHolder;
    public GameObject pauseSprite;
    public GameObject gameOverImage;
    public GameObject levelCompleteImage;
    public GameObject mainMenuButton;
    public GameObject nextLevelButton;
    public GameObject restartButton;

    bool isPaused = false;
    public bool winState = false;
    public bool deathState = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("p"))
        {
            if (!isPaused)
            {
                isPaused = true;
                //LeanTween.moveX(pauseHolder.GetComponent<RectTransform>(), -100, 1);
                pauseHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -5);
                pauseSprite.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            else
            {
                isPaused = false;
                LeanTween.moveX(pauseHolder.GetComponent<RectTransform>(), 102.5f, 1);
                pauseSprite.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
        }

        if(winState)
        {
            //player cant be killed and enemies drop aggro
            //rotate camera
            //disable controller
            levelCompleteImage.SetActive(true);
            mainMenuButton.SetActive(true);
            nextLevelButton.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(deathState)
        {
            //enemies drop aggro
            //play death animation
            //rotate camera
            //disable controller
            gameOverImage.SetActive(true);
            mainMenuButton.SetActive(true);
            restartButton.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("Title_Menu");
    }
    public void RestartPressed()
    {
        SceneManager.LoadScene("Game");
    }
    public void NextLevelPressed()
    {
        SceneManager.LoadScene("Game");
    }
}
