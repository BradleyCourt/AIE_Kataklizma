using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIBar_Master : MonoBehaviour {

    [Header("Value Source")]
    public Kataklizma.Gameplay.EntityAttributes Attributes;
    public ValueType ValueSource;
    public ValueType MaxValueSource;

    [Tooltip("When Max Value changes, have Min Value become previous Max Value")]
    public bool MinChasesMax;

    [Header("UI Elements")]
    public UnityEngine.UI.Image Background;
    public UnityEngine.UI.Image ForegroundMask;
    public UnityEngine.UI.Image Foreground;

    [Space]
    [Range(0,1)]
    public float MaskOffset;

    [Header("Warnings")]
    public bool FlashEnabled;
    public Color FlashTint = Color.red;

    [Range(0,5)]
    public float FlashTime;

    [Range(0,1)]
    public float FlashThreshold;

    protected bool FlashActive = false;

    protected float MinValue;
    protected float MaxValue;

    protected float FillAmount;

    protected int FlashTweenId;
    //LTDescr FlashTween;

	// Use this for initialization
	void Start () {
        //FlashTween = LeanTween.value(gameObject, (value) => {
        //    Background.color = value;
        //    Foreground.color = value;
        //}, FlashTint, Color.white, FlashTime).setOnComplete(InvokeFlash);

        Attributes.ValueChanged += Attributes_ValueChanged;

        MaxValue = Attributes[MaxValueSource];
        UpdateValue();
	}

    private void Attributes_ValueChanged(Object sender, ValueType type, ValueSubtype subtype, float oldValue)
    {
        if ( type == ValueSource) UpdateValue();
        if ( type == MaxValueSource )
        {
            if (MinChasesMax)
                MinValue = MaxValue;

            MaxValue = Attributes[type];
            UpdateValue();         
        }

    }
    
    void UpdateValue()
    {
        FillAmount = (Attributes[ValueSource] - MinValue) / (MaxValue - MinValue);

        var usableRange = 1.0f - MaskOffset;
        var scaledValue = MaskOffset + (FillAmount * usableRange);

        LeanTween.value(ForegroundMask.gameObject, (value) => { ForegroundMask.fillAmount = value; }, ForegroundMask.fillAmount, scaledValue, 0.5f);
        //Mask.fillAmount = scaledValue;

        Flash();
    }

    void Flash()
    {
        if (FlashEnabled && FillAmount <= FlashThreshold)
        {
            if (FlashActive)
                FlashOff();
            else
                FlashOn();

        }
        else if (FlashActive)
            FlashOff();
    }

    void FlashOn()
    {
        if (LeanTween.isTweening(FlashTweenId)) return;

        FlashActive = true;
        var tween = LeanTween.value(gameObject, (value) =>
        {
            Background.color = value;
            Foreground.color = value;
        }, Color.white, FlashTint, FlashTime).setOnComplete(InvokeFlash);

        FlashTweenId = tween.id;

    }

    void FlashOff()
    {
        if (LeanTween.isTweening(FlashTweenId)) return;

        FlashActive = false;
        var tween = LeanTween.value(gameObject, (value) => {
            Background.color = value;
            Foreground.color = value;
        }, FlashTint, Color.white, FlashTime).setOnComplete(InvokeFlash);

        FlashTweenId = tween.id;
    }

    void InvokeFlash() {

        Invoke("Flash", 0);
    }
}
