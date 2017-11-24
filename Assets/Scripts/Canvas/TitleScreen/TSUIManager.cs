using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TSUIManager : MonoBehaviour
{

	private bool buttonClicked = false;

	public VideoPlayer breakingNews;
	public GameObject background;
    public GameObject menuHolder;
	public GameObject settingsHolder;
	public GameObject unlocksHolder;
	public GameObject PAKPanel;
    public GameObject Canvas;
    public GameObject Static;
    public GameObject titleStatic;

	public float menuSlideTime = 0.5f;
	
	
	// Use this for initialization
	void Start () {
        StartCoroutine("StaticReset");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (buttonClicked == false && Input.anyKey)
		{
			StartCoroutine("PlayVideo");
		}
	}

	IEnumerator PlayVideo()
	{
		breakingNews.Play();
		buttonClicked = true;
		PAKPanel.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        titleStatic.SetActive(false);
        yield return new WaitForSeconds(2.2f);
		breakingNews.Stop();
        background.SetActive(true);
        yield return new WaitForSeconds(3);
        LeanTween.scale(Canvas, Vector3.one * 0.7f, 1);
        LeanTween.moveX(Canvas.GetComponent<RectTransform>(), -105, 1);
        LeanTween.moveY(Canvas.GetComponent<RectTransform>(), 50, 1);
        LeanTween.moveX(menuHolder.GetComponent<RectTransform>(), -100, 1);
	}

    IEnumerator StaticReset()
    {
        yield return new WaitForSeconds(20);
        Static.SetActive(true);
        var movers = FindObjectsOfType<ObjectMover>();

        foreach (var mover in movers)
            mover.ResetPos();
        yield return new WaitForSeconds(0.3f);
        Static.SetActive(false);
        StartCoroutine("StaticReset");

        
    }

	public void SettingsTransition()
	{
		LeanTween.moveX(settingsHolder.GetComponent<RectTransform>(), -100, menuSlideTime);
	}

	public void NewGame()
	{
        SceneManager.LoadScene("Game");
        print("loading game scene");
	}

	public void UnlocksTransition()
	{
		LeanTween.moveX(unlocksHolder.GetComponent<RectTransform>(), -100, menuSlideTime);
	}

	public void Quit()
	{
        Application.Quit();
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
