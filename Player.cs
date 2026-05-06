using Godot;
using System;

public partial class Player : XROrigin3D
{
    [Export] MeshInstance3D deathMesh;
    [Export] VideoStreamPlayer deathAnimation;
    [Export] Area3D collision;

    public async void Die()
    {
        GD.Print("I'm dead");
        collision.CollisionLayer = 2;
        deathMesh.Visible = true;
        deathAnimation.Visible = true;
        deathAnimation.Play();

        await ToSignal(GetTree().CreateTimer(6.0f), "timeout");

        GlobalPosition = new Vector3(-8.43f,0.284f,11.70f);
        collision.CollisionLayer = 1;
        deathMesh.Visible = false;
        deathAnimation.Visible = false;
        GD.Print("I'm alive");
    }
}
