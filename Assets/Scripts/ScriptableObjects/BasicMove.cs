using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Character Abilities/Move", order = 1)]
public class BasicMove : ScriptableObject {

    public float Speed;
    public float Duration;
    public float Cooldown;

    public void Run() {

    }
}
