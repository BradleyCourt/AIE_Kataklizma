using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scriptables;

public class AbilityWatcher : MonoBehaviour {

    [UnityEngine.Serialization.FormerlySerializedAs("Background")]
    public Image Active;
    [UnityEngine.Serialization.FormerlySerializedAs("Filler")]
    public Image Inactive;
    public Image Foreground;

    
    public ScriptedAbility TargetAbility;

    [Header("Timers")]
    protected float DurationTotal;
    protected float DurationRemaining;
    protected float _DurationPercent;
    protected float DurationPercent {
        get {
            return TargetAbility.DurationRemaining / TargetAbility.DurationTotal;
        }
    }

    protected float CooldownTotal;
    protected float CooldownRemaining;
    protected float _CooldownPercent;
    protected float CooldownPercent {
        get {
            if (TargetAbility.CooldownTime == 0) return 0;

            return Mathf.Clamp(TargetAbility.CooldownEnds - Time.time, 0, TargetAbility.CooldownTime) / TargetAbility.CooldownTime;
        }
    }

	// Use this for initialization
	void Start () {
        Inactive.fillAmount = 0;
    }
	

	// Update is called once per frame
	void Update () {

        if (TargetAbility == null) return;

        DurationTotal = TargetAbility.DurationTotal;
        DurationRemaining = TargetAbility.DurationRemaining;
        _DurationPercent = DurationPercent;

        CooldownTotal = TargetAbility.CooldownTime;
        CooldownRemaining = Mathf.Clamp(TargetAbility.CooldownEnds - Time.time, 0, TargetAbility.CooldownTime);
        _CooldownPercent = CooldownPercent;


        // If Ability State == Ready
        // - Set Active as first child
        // - Fill Active to 1
        // - Fill Inactive from Countdown
        // Else
        // - Set Inactive as first child
        // - Fill Inactive to 1
        // - Fill Active from accumulated Activation Sequence time

        if (TargetAbility.ActivationState == AbilityActivationState.Ready) {
            Active.transform.SetAsFirstSibling();
            Active.fillAmount = 1;
            Inactive.fillAmount = CooldownPercent;
        }
        else {
            Inactive.transform.SetAsFirstSibling();
            Inactive.fillAmount = 1;
            Active.fillAmount = DurationPercent;
        }
    }
}
