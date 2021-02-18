using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public int sessionNumber;
    public List<Line> defaultLines;
    public List<Line> altLines;
}

[System.Serializable]
public class Line
{
    public Sprite speakerIcon;
    public string speakerName;
    public string text;
}
