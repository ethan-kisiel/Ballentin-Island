 ///////////////////////////////////////////////////////////////////////////////
 // Project: BallentinIsland
 //
 //
 // Created by Ethan Kisiel
 // Created on Fri Aug 23 2024
 //
 //
 //
 // Copyright (c) 2024 Company
 ///////////////////////////////////////////////////////////////////////////////

using BallentinIsland.Core;
using Godot;
using System;

namespace BallentinIsland.Core
{
    public partial class PlayerController : Node3D
    {
        public PlayerCharacter Player {get; set;}

        public override void _EnterTree()
        {
            SetMultiplayerAuthority(int.Parse(Name));
        }

        public override void _Ready()
        {
            Player = GetNode<PlayerCharacter>("PlayerCharacter");

             // set mp synch to server auth
            //GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(1);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            Player.HandleMousePosition(delta, GetViewport().GetMousePosition());
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Input.IsActionPressed("target") && (IsMultiplayerAuthority() || Multiplayer.IsServer()))
            {
                
                //Rpc(nameof(Player.HandleTargetAction), GetViewport().GetMousePosition());
                Player.HandleTargetAction(GetViewport().GetMousePosition());
            }

            if (Input.IsActionPressed("attack"))
            {
                //Player.HandleAttackAction(GetViewport().GetMousePosition());
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("lock_camera"))
            {
                Player.HandleLockCamera();
            }

            if (@event.IsActionPressed("zoom_in"))
            {
                Player.HandleMouseWheel(-1);
            }

            if (@event.IsActionPressed("zoom_out"))
            {
                Player.HandleMouseWheel(1);
            }

        }
    }
}
