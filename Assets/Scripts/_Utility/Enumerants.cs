using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ValueType {
    _NONE,
    Health,
    HealthMax,
    HealthRegen,
    Experience,
    ExperienceThreshold,
    CharacterLevel,
    Armour,    
    MoveSpeed,
    JumpHeight,
    Cooldown,
    Duration,
    Damage,
    DamageReduction,
    Range,
}

[System.Serializable]
public enum ValueSubtype {
    Base,
    Modifier,
    Derived
}

public enum DamageType {
    Physical,
    Fire,
    Acid,
}


[System.Serializable]
public enum FilterType {
    WhitelistAny,
    WhitelistAll,
    BlacklistAny,
    BlacklistAll,
}

public enum AbilityActivationType{
    Normal,
    Charged,
    Channeled
}