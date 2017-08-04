using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueType {
    Health,
    HealthMax,
    HealthRegen,
    Experience,
    Armour,    
    MoveSpeed,
    JumpHeight,
    Cooldown,
    Duration,
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
