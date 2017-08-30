using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour {

    public GameObject text;
    public float AnimationTime = 2.0f;
    public float fadeAmount;

    private bool FadeOut = true;

	// Use this for initialization
	void Start () {
        StartCoroutine(Coroutine());
        //LeanTween.alpha(text, 0f, 3f).setEase(LeanTweenType.easeInCirc);
        
	}
	
    IEnumerator Coroutine()
    {
        while (true)
        {
            if (FadeOut)
            {
                LeanTween.alpha(text.GetComponent<RectTransform>(), fadeAmount, AnimationTime);
            }
            else
                LeanTween.alpha(text.GetComponent<RectTransform>(), 1.0f, AnimationTime);

            yield return new WaitForSeconds(AnimationTime);
            FadeOut = !FadeOut;
        }
    }



	// Update is called once per frame
	void Update () {
        
	}
}
