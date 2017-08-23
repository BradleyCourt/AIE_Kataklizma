using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour {

    public GameObject text;
    public float AnimationTime = 2.0f;

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
                var descr = LeanTween.alpha(text.GetComponent<RectTransform>(), 0.5f, AnimationTime);
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
