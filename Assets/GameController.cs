using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public Pause PauseMenu;
    public Animator CharacterAnimator;
    public Kataklizma.Gameplay.EntityAttributes Attributes;
    public Kataklizma.Gameplay.LevelSystem Levels;


    // Use this for initialization
    void Start()
    {

        Attributes.ValueChanged += Attributes_ValueChanged;

    }

    private void Attributes_ValueChanged(Object sender, ValueType type, ValueSubtype subtype, float oldValue)
    {
        if (type == ValueType.Experience && !Levels.HasMoreLevels && Levels.CurrentXpThreshold <= Levels.CurrentXp)
        {
            HandleWinning();
        }

        if (type == ValueType.Health && Attributes[ValueType.Health] <= 0)
        {

            HandleLosing();

        }
    }

    protected void HandleWinning()
    {
        Attributes.GetComponent<Kataklizma.Gameplay.PlayerController>().enabled = false;
        CharacterAnimator.SetFloat("WalkSpeed", 0);
        player.tag = "Untagged";
        PauseMenu.winState = true;
        // Do game win
    }

    protected void HandleLosing()
    {
        Attributes.GetComponent<Kataklizma.Gameplay.PlayerController>().enabled = false;
        CharacterAnimator.SetFloat("WalkSpeed", 0);
        player.tag = "Untagged";
        // Do death animation
        //CharacterAnimator.SetTrigger("Death");

        // Show Death Menu

        StartCoroutine(this.DelayedAction(0.3f, () =>
        {
            PauseMenu.deathState = true;
        }));

    }
}
if