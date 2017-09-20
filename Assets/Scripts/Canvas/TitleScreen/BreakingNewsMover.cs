using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingNewsMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
        breakingNews = GetComponent<RectTransform>();

    }
    RectTransform breakingNews;
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right);
        if(breakingNews.localPosition.x >= 500)
        {
            float oldY = breakingNews.localPosition.y;
            transform.localPosition = new Vector3(-620, oldY, 0);
        }
    }
}
