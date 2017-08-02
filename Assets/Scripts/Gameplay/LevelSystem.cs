using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSystem : MonoBehaviour
{

    public List<int> XpThresholds;

    public int XP;
    private int CurrentLevel;  // Character's 'evolution' level
    public int CurrentXpThreshold {  get { return XpThresholds[Mathf.Min(CurrentLevel - 1, XpThresholds.Count - 1)]; } }
    public bool HasMoreLevels {  get { return CurrentLevel < XpThresholds.Count; } }

    int totaldifference;
    int experienceToNextLevel;
    int differencexp;

    public Slider xpBar;
	// Use this for initialization
	void Start ()
    {
        CurrentLevel = 1;
        xpBar.value = 0;
        xpBar.maxValue = CurrentXpThreshold;
	}
	
	// Update is called once per frame
	void Update ()
    {

        UpdateXP(1); // checks how much xp to add every frame
        Debug.Log("XP: " + XP);
	}

    public void UpdateXP (int experiece)
    {
        XP += experiece;

        bool shouldLevelUp = HasMoreLevels && XP >= CurrentXpThreshold;

       // int Charlvl = (int)(0.1f * Mathf.Sqrt(XP));

        if (shouldLevelUp) // if the characters level is no longer = to your current level
        {
            CurrentLevel++; // add level
            xpBar.maxValue = CurrentXpThreshold; // updates the xp threshold
            Debug.Log("I gained a level   " + CurrentLevel);
        }

        xpBar.value = XP;

     }


}
