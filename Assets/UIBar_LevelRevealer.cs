using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar_LevelRevealer : MonoBehaviour {

    public Kataklizma.Gameplay.EntityAttributes Attributes;

    public List<GameObject> LevelDisplays;

	// Use this for initialization
	void Start () {
        Attributes.ValueChanged += Attributes_ValueChanged;

        ShowLevel((int)Attributes[ValueType.CharacterLevel]);
    }

    private void Attributes_ValueChanged(Object sender, ValueType type, ValueSubtype subtype, float oldValue)
    {
        if ( type == ValueType.CharacterLevel)
        {
            ShowLevel((int)Attributes[ValueType.CharacterLevel]);
        }
    }

    void ShowLevel(int level)
    {
        level -= 1;

        foreach (var obj in LevelDisplays)
            if (obj != null) obj.SetActive(false);

        if (level < LevelDisplays.Count)
            LevelDisplays[level].SetActive(true);
    }
}
