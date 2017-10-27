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
    ContactDamage,
    ContactDamageBonus,
    WalkSpeed,
    DashSpeed,
}

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


public enum FilterType {
    WhitelistAny,
    WhitelistAll,
    BlacklistAny,
    BlacklistAll,
}

public enum AbilityActivationState {
    Ready,
    Charging,
    Channeling,
    Cleanup,
}

public enum AbilityActivationType {
    Active,
    Passive,    
}

public enum CharacterBindPoint {
    NONE,
    Forehead,
    Mouth,
    Torso,
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot,
    Tail
        
}

public enum MapTileType {
    NONE,
    Tile5m = 1,
    Tile10m = 2,
    Tile20m = 4,
    Tile30m = 6,
    Tile40m = 8,  
    Tile80m = 16,
    TileRoad = 255,
}

public enum RoadType {
    _Invalid = -1,
    End = 0,
    Turn = 1,
    Straight = 2,
    Tee = 3,
    Inter = 4
}

public enum RoadRotation {
    _Invalid = -1,
    Turn000 = 0,
    Turn090 = 90,
    Turn180 = 180,
    Turn270 = 270,
}