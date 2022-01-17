using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDialogue
{
    [SerializeField] string name;
    [SerializeField] List<string> lines;
    [SerializeField] List<Sprite> expressions;

    public string Name
    {
        get { return name; }
    }

    public List<string> Lines
    {
        get { return lines; }
    }

    public List<Sprite> Expressions
    {
        get { return expressions; }
    }
}
