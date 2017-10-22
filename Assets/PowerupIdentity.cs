using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerupIdentity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Sprite highlight;
    public Sprite Dissabled;
    public Sprite clicked;

    Image image;
    public bool highlighted = false;
    public bool interactable = false;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(interactable)
        {
            highlighted = true;
            image.sprite = highlight;
            Debug.Log("Is Interactable OnPointerEnter");
        }
        else
        {
            Debug.Log("Is Not Interactable OnPointerEnter");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        highlighted = false;
        image.sprite = Dissabled;

    }
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
	if(highlighted && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                image.sprite = clicked;
            }
            else
            {
                image.sprite = highlight;
            }
        }
        else if(!highlighted)
        {
            image.sprite = Dissabled;
        }
    else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            image.sprite = highlight;
        }
        	
	}
}
