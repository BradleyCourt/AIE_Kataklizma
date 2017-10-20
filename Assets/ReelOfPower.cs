﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scriptables;

public class ReelOfPower : MonoBehaviour {

    public ScriptedAbility[] abilities;
    public ScriptedAbility[] reelAbilities;

    public Image[] images;

    public float startPosition;
    public float offset;

    public float positionInReel = 0;

    public float RateForStarting = 15f;
    float rateTurnDownTime;
    public float MotionRate = 0f;

    public bool stopped = true;
    // Use this for initialization
    public void Action()
    {
        stopped = false;
        MotionRate = RateForStarting;

        ReelOfPower[] reels = transform.parent.GetComponentsInChildren<ReelOfPower>();
        foreach (ReelOfPower reel in reels)
        {
            reel.MotionRate = reel.RateForStarting;
        }
    }

    void Start ()
    {
        // take the abilities and make up a reel of them according to weights.
        int t_Size = 0;
        foreach(ScriptedAbility single in abilities)
        {
            t_Size += single.weight;
        }
        reelAbilities = new ScriptedAbility[t_Size];

        int index = 0;
        foreach (ScriptedAbility single in abilities)
        {
            for (int i = 0; i < single.weight; i++)
            {
                reelAbilities[index] = single;
                index++;
            }
        }

        for(int i = 0; i < reelAbilities.Length; i++)
        {

            //Shuffle the deck of cards.
            //Grab the first index, move it to a new location of Random.Range(0, reelAbilitys.Length - 1)
            //Grab the following index and repeat the next step. foreach in abilities.
            int index1 = Random.Range(0, reelAbilities.Length);
            int index2 = Random.Range(0, reelAbilities.Length);
            ScriptedAbility temp = reelAbilities[index2];
            reelAbilities[index2] = reelAbilities[index1];
            reelAbilities[index1] = temp;
        }

        Image[] tempImages= transform.GetComponentsInChildren<Image>();
        // this list includes the reel, so copy everything else into an array
        index = 0;
        images = new Image[tempImages.Length - 1];
        foreach (Image image in tempImages)
        {
            if (image.gameObject != gameObject)
            {
                images[index] = image;
                index++;
            }
        }

        startPosition = transform.position.y;
        offset = images[1].transform.position.y- images[0].transform.position.y;
        faction_OnePercentValue = (startPosition + offset) - startPosition;
    }
    public float faction_OnePercentValue = 0;
    public float factionPercent = 0;
	// Update is called once per frame
	void Update ()
    {
        //rateTurnDownTime = MotionRate;
        //rateTurnDownTime = (rateTurnDownTime) % RateForStarting;
        positionInReel += MotionRate * Time.deltaTime;

        //if (Input.GetKey(KeyCode.K))
        //    positionInReel -= 4*Time.deltaTime;



        int startIndex = (int)positionInReel;

        // get the fraction of a second between 0 and 1
        float fraction = positionInReel - startIndex;

        for (int i = 0; i < images.Length; i++)
        {
            int indexInReel = (i + startIndex) % reelAbilities.Length;
            images[i].sprite = reelAbilities[indexInReel].icon;
        }

        Vector3 pos = transform.position;
        if (MotionRate == 0)
        {
            Debug.Log("Return Faction: " + fraction);
            // keep moving slowly until we snap
            if (fraction > 0.01f)
                positionInReel += 0.1f * Time.deltaTime;

            //if(pos.y > startPosition + 0.01f /*|| pos.y < startPosition - 0.1f*/ /*&& fraction < 1.1f*/)
            //{
                //float customRate
            //    positionInReel = Mathf.Lerp(positionInReel, 0f, 1f);
            //}

            //if(fraction )

            //if (transform.position.y <= startPosition - 0.5f && transform.position.y >= startPosition + 0.5f || transform.position.y <= startPosition + offset && transform.position.y >= startPosition - offset)
            //{
            //    if (transform.position.y > startPosition + (offset / 1.9))
            //    {
            //        //pos.y = startPosition + offset;
            //        pos = Vector3.Lerp(pos, new Vector3(pos.x, startPosition + offset, pos.z), 0.1f);
            //    }
            //    else
            //    {
            //        pos = Vector3.Lerp(pos, new Vector3(pos.x, startPosition, pos.z), 0.1f);
            //    }
            //}
        }
        else
        {
            //slow it down over time
            MotionRate *= 0.98f;
            if (MotionRate < 0.1f)
            {
                MotionRate = 0;
            }
        }
        pos.y = startPosition - offset * fraction;
        transform.position = pos;
	}
}
 