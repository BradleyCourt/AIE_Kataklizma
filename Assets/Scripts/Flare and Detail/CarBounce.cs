using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBounce : MonoBehaviour {
    public float scaleAmount = 0.8f;
    public float jumpHeight = 0.4f;
    public float timeToTop = 0.7f;
    public float fastResetTime = 0.2f;
    public float fullResetTime = 1.0f;
    public bool PlayOnce = true;
    public bool loop = false;

    public LeanTweenType moveTypeOnHop = LeanTweenType.easeOutCubic;
    public LeanTweenType scaleTypeOnHop = LeanTweenType.easeOutSine;
    public LeanTweenType moveTypeOnSlam = LeanTweenType.easeOutBack;
    public LeanTweenType scaleTypeOnSlam = LeanTweenType.easeOutBack;

    protected bool animating;

    // Use this for initialization
    void Start () {
        animating = false;
    }
	
	// Update is called once per frame
	void Update () {
        if ((PlayOnce || loop ) && !animating)
            Activate();
	}

    public void Activate()
    {
        if (!animating)
            Hop();
    }


    void Hop()
    {
        animating = true;
        PlayOnce = false;
        LeanTween.moveLocalY(gameObject, jumpHeight, timeToTop).setEase(moveTypeOnHop);
        LeanTween.scaleY(gameObject, scaleAmount, timeToTop).setEase(scaleTypeOnHop).setOnComplete(Slam);
    }

    void Slam()
    {
        LeanTween.moveLocalY(gameObject, 0.0f, fastResetTime).setEase(moveTypeOnSlam);
        LeanTween.scaleY(gameObject, 1f, fullResetTime).setEase(scaleTypeOnSlam).setOnComplete(End);
    }


    void End()
    {
        animating = false;
    }
}
