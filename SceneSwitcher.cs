using Godot;
using System;

public partial class SceneSwitcher : Node3D
{
    public void OnAreaEntered(Area3D area)
    {
        GD.Print("Checking identity");

        if (area.GetParent().IsInGroup("Player"))
        {
            GD.Print("Player Found");
            GetTree().CallDeferred("change_scene_to_file", "res://Congrat.tscn");
        }
    }
}
