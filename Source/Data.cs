using System;
using System.Collections.Generic;

[Serializable]
public class Settings
{
    public float cameraSpeed;
    public SoundSettings soundsettings;
}

[Serializable]
public class SoundSettings
{
    public float master;
    public float effects;
    public float bgm;
}

[Serializable]
public class ProgressionData
{
    public int tutorials;
    public int scenarios;
}

[Serializable]
public class SaveData
{
    public string gameMode;
    public string scenarioName;
    public string activeTeam;
    public bool gameOver;
    public int AIlevel;
    public int extracted;
    public int extractionLimit;
    public int turn;
    public int turnLimit;
    public int unitcount;
    public int unitLimit;
    public List<Terrain> terrain;
    public List<Units> units;
}


[Serializable]
public class Scenario
{
    public string scenarioName;
    public string gameMode;
    public int AIlevel;
    public string activeTeam;
    public int extractionLimit;
    public int turnLimit;
    public int unitcount;
    public int unitLimit;
    public List<Terrain> terrain;
    public List<Units> units;
}

[Serializable]
public class Terrain
{
    public int posX;
    public int posY;
    public string sprite;
}

[Serializable]
public class Units
{
    public int posX;
    public int posY;
    public string team;
    public string name;
}

public class ModData
{
    public string name;
    public int progress;
    public List<Scenario> scenarios;
}