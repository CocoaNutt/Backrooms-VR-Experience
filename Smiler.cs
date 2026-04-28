using Godot;
using System;
using System.Collections.Generic;

public partial class Smiler : CharacterBody3D
{
    [Export] public float speed;
    [Export] public float chase; 
    [Export] public NavigationAgent3D nav;
    public CharacterBody3D player;
    public Random rand = new Random();
    private bool inChase = false;
    public int attackCooldown = 0;

    public override void _Ready()
    {
        base._Ready();

        nav.MaxSpeed = 5.0f;
        newDest();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
            try
            {

                var Players = GetTree().GetNodesInGroup("Player");
                CharacterBody3D closestPlayer = null;
                float closestDist = float.MaxValue;

                foreach(Node p in Players)
                {
                    /*Player body = p as Player;
                    if (body == null) continue;

                    float dist = GlobalPosition.DistanceTo(body.GlobalPosition);

                    if (dist < closestDist && !body.isDying)
                    {
                        closestDist = dist;
                        closestPlayer = body;
                    }*/
                }

                player = closestPlayer;

                float distToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
                if (distToPlayer <= chase) //If distance from player/enemy less than chase dist
                {
                    if (!inChase)
                    {
                        inChase = true;
                    }
                    nav.TargetPosition = player.GlobalPosition; //Follows player
                }
                else
                {
                    if (inChase)
                    {
                        inChase = false;
                    }
                    if (nav.DistanceToTarget()<2)
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
                if(Velocity.Length() > .1f)
                {
                    //animPlayer.Play("RUN");
                }
                MoveAndSlide();
                //GD.Print("After MoveAndSlide: " + GlobalPosition);
            }
            catch(Exception e)
            {
                GD.Print("Exception: " + e);
            }
            
        
        
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
        GD.Print("Touched someone");
    }

    public void OnAreaExited(Area3D area)
    {
        GD.Print("Someone exited");
    }	


}
