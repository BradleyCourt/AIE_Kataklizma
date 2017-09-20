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

[System.Serializable]
[System.Flags]
public enum AbilityActivationTriggerType {
    None = 0,
    Charge = 1,
    Channel = 2
}

public enum AbilityActivationState {
    Ready,
    Charging,
    Channeling,
    Cleanup,
}

[System.Serializable]
public enum MapTileType {
    NONE,
    Tile5m,
    Tile10m,
    Tile20m,
    Tile30m,
    Tile40m,
    TileRoad
}