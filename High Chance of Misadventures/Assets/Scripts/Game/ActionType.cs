using System;

[Serializable]
public enum ActionType
{
    None,
    Fire,
    Water,
    Earth
}

public enum CombatResult
{
    None,
    PlayerFireWin,
    PlayerFireTie,
    PlayerFireLose,
    PlayerEarthWin,
    PlayerEarthTie,
    PlayerEarthLose,
    PlayerWaterWin,
    PlayerWaterTie,
    PlayerWaterLose

}

public enum GameType
{
    LO,
    HO
}
