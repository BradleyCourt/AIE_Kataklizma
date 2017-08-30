using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TSUIManager : MonoBehaviour
{

	private bool buttonClicked = false;

	public VideoPlayer breakingNews;
	public GameObject background;
	public GameObject PAKPanel;
	public VideoPlayer channelSurfing;
	
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
	}
}
