using Godot;
using System;

public partial class Player : XROrigin3D
{
    [Export] VideoStreamPlayer deathAnimation;
    [Export] Area3D collision;

    public async void Die()
    {
        GD.Print("I'm dead");
        collision.CollisionLayer = 2;
        deathAnimation.Visible = true;
        deathAnimation.Play();

        await ToSignal(GetTree().CreateTimer(6.0f), "timeout");

        GlobalPosition = new Vector3(-15,0,15);
        collision.CollisionLayer = 1;
        deathAnimation.Visible = false;
        GD.Print("I'm alive");
    }
}
