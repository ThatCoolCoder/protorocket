using Godot;
using System;

namespace Common;

public class Prototype
{
    public string Name = "";
    public string RootScenePath = "";

    public Prototype(string name, string rootScenePath)
    {
        Name = name;
        RootScenePath = rootScenePath;
    }
}