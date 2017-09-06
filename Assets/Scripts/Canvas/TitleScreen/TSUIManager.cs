﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TSUIManager : MonoBehaviour
{

	private bool buttonClicked = false;

	public VideoPlayer breakingNews;
	public GameObject background;
    public GameObject menuHolder;
	public GameObject settingsHolder;
	public GameObject unlocksHolder;
	public GameObject PAKPanel;
	public VideoPlayer channelSurfing;
    public GameObject Canvas;

	public float menuSlideTime = 0.5f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (buttonClicked == false && Input.anyKey)
		{
			channelSurfing.Stop();
			StartCoroutine("PlayVideo");
		}
	}

	IEnumerator PlayVideo()
	{
		breakingNews.Play();
		print("should be playing");
		buttonClicked = true;
		PAKPanel.SetActive(false);
		yield return new WaitForSeconds(3);
		breakingNews.Stop();
        background.SetActive(true);
        yield return new WaitForSeconds(3);
        LeanTween.scale(Canvas, Vector3.one * 0.7f, 1);
        LeanTween.moveX(Canvas.GetComponent<RectTransform>(), -105, 1);
        LeanTween.moveY(Canvas.GetComponent<RectTransform>(), 50, 1);
        LeanTween.moveX(menuHolder.GetComponent<RectTransform>(), -100, 1);
	}

	public void SettingsTransition()
	{
		LeanTween.moveX(settingsHolder.GetComponent<RectTransform>(), -100, menuSlideTime);
	}

	public void NewGame()
	{
		
	}

	public void UnlocksTransition()
	{
		LeanTween.moveX(unlocksHolder.GetComponent<RectTransform>(), -100, menuSlideTime);
	}

	public void Quit()
	{
		
	}

	public void UnlocksBack()
	{
		LeanTween.moveX(unlocksHolder.GetComponent<RectTransform>(), 102.5f, menuSlideTime);
	}

	public void SettingsBack()
	{
		LeanTween.moveX(settingsHolder.GetComponent<RectTransform>(), 102.5f, menuSlideTime);
	}
}
