using Godot;
using System;

public class PartInfo
{
    public string Name;
    public string Path
    {
        get
        {
            return $"res://Konstruct/Parts/{Name}.tscn";
        }
    }
}