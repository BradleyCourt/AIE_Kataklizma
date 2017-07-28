using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSystem : MonoBehaviour
{
    public int XP;
    public int CurrentLevel;

    int totaldifference;
    int experienceToNextLevel;
    int differencexp;

    public Slider xpBar;
	// Use this for initialization
	void Start ()
    {
        xpBar.value = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (XP > 900 && CurrentLevel == 3)
        {
            XP = 900;
            CurrentLevel = 3;
        }
 
        UpdateXP(5); // checks how much xp to add every frame
        xpBar.value = updateXPBar(); 
	}

    public void UpdateXP (int experiece)
    {
        XP += experiece;

        int Charlvl = (int)(0.1f * Mathf.Sqrt(XP));

        if (Charlvl != CurrentLevel) // if the characters level is no longer = to your current level
        {
            CurrentLevel = Charlvl; // add level
        }

        experienceToNextLevel = 50 * (CurrentLevel + 1) * (CurrentLevel + 1);

        differencexp = experienceToNextLevel - XP;

        

        //total difference / difference xp is what needs to be displayed on the UI
     }

    float updateXPBar()
    {
        return totaldifference = experienceToNextLevel - (100 * CurrentLevel * CurrentLevel);
    }
}
