using Godot;
using System;
using System.Collections.Generic;

public partial class Smiler : CharacterBody3D
{
    [Export] public float speed;
    [Export] public float chase; 
    [Export] public NavigationAgent3D nav;
    public Node3D player;
    public Random rand = new Random();
    private bool inChase = false;
    public int PursueCooldown = 1200;

    public override void _Ready()
    {
        base._Ready();

        nav.MaxSpeed = 5.0f;
        FindPlayer();
        newDest();
    }

    public void FindPlayer()
    {
         var players = GetTree().GetNodesInGroup("Player");
        foreach (Node n in players)
        {
            if (n is Node3D node3d)
            {
                player = node3d;
                break;
            }
        }
    }

    public override void _Process(double delta)
    {
        //GD.Print("Process");
        base._Process(delta);
        PursueCooldown--;

        if (player != null)
        {

            float distToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);

            if (distToPlayer <= chase && PursueCooldown <= 0)
            {
                nav.TargetPosition = player.GlobalPosition;
                //GD.Print("Pursuing player");
            }
            else if (nav.DistanceToTarget() < 2)
            {
                newDest();
            }
        }

        Vector3 nextPos = nav.GetNextPathPosition(); //Update path position
        //GD.Print("Next point: " + nav.GetNextPathPosition());
        Vector3 dir = (nextPos - GlobalPosition).Normalized(); //Get direction
        //GD.Print("Direction: " + dir);


        if (dir.Length() > 0.75f)
        {
        }
        else
        {
            dir = Vector3.Zero;
        }

        Velocity = speed * dir;
        //GD.Print("Before MoveAndSlide: " + GlobalPosition);
        //GD.Print("Current speed: " + speed);
        //GD.Print("Current direction: " + dir);
        MoveAndSlide();
        //GD.Print("After MoveAndSlide: " + GlobalPosition);
            
            
        
        
    }

    public void newDest()
    {
            var Target = GetTree().GetNodesInGroup("PatrolPoints");
            Marker3D target = Target[rand.Next(Target.Count)] as Marker3D;
            Vector3 targetLoc = target.GlobalPosition;
            nav.TargetPosition = targetLoc;
    }

    public void OnAreaEntered(Area3D area)
    {
        if (area.GetParent().IsInGroup("Player"))
        {
            Player player = (Player)area.GetParent();
            player.Die();
            PursueCooldown = 600;
        }
    }

    public void OnAreaExited(Area3D area)
    {
        GD.Print("Someone exited");
    }	


}
