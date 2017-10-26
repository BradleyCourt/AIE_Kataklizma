using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    public GameObject pauseHolder;
    public GameObject pauseSprite;
    bool isPaused = false;
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
                LeanTween.moveX(pauseHolder.GetComponent<RectTransform>(), -100, 1);
                pauseSprite.SetActive(true);
            }

            else
            {
                isPaused = false;
                LeanTween.moveX(pauseHolder.GetComponent<RectTransform>(), 102.5f, 1);
                pauseSprite.SetActive(false);
            }
        }

    }

}
