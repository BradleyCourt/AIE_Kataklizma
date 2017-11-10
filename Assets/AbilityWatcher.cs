using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scriptables;

public class AbilityWatcher : MonoBehaviour {

    [System.Serializable]
    public struct DebugValues {
        public float CooldownRemaining;
        public float CooldownPercent;
    }

    public Image Background;
    public Image Filler;
    public Image Foreground;

    
    public ScriptedAbility TargetAbility;

    public DebugValues WatchValues;

	// Use this for initialization
	void Start () {
        Filler.fillAmount = 0;

        var x = FindObjectOfType<Kataklizma.Gameplay.PlayerController>();
        TargetAbility = x.Abilities[1].Ability;

	}
	

	// Update is called once per frame
	void Update () {

        WatchValues.CooldownRemaining = Mathf.Max(TargetAbility.CooldownEnds - Time.time,0);
        WatchValues.CooldownPercent = 100.0f * WatchValues.CooldownRemaining / TargetAbility.CooldownTime;

        //if (!TargetAbility) return;

        if (TargetAbility.CooldownEnds < Time.time)
            Filler.fillAmount = 0;
        else {
            Filler.fillAmount = (TargetAbility.CooldownEnds - Time.time) / TargetAbility.CooldownTime;
        }


        if ( Filler.fillAmount == 0 ) {
            // Change Foreground tint?
        }
	}
}
