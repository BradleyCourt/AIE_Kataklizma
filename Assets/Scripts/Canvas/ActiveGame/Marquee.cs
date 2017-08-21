using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marquee : MonoBehaviour {
    public string message = "STATE OF EMERGANCY";
    public float scrollSpeed;
    public Rect messageRect;

    void OnGUI()
    {
        //If there is no set Rect
        if(messageRect.width == 0)
        {
            var dimensions = GUI.skin.label.CalcSize(new GUIContent(message));

            messageRect.x = -dimensions.x;
            messageRect.width = dimensions.x;
            messageRect.height = dimensions.y;
        }

        messageRect.x += Time.deltaTime * scrollSpeed;

        if(messageRect.x > Screen.width)
        {
            messageRect.x = -messageRect.width;
        }

        GUI.Label(messageRect, message);
    }
}
