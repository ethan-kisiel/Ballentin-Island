 ///////////////////////////////////////////////////////////////////////////////
 // Project: BallentinIsland
 //
 //
 // Created by Ethan Kisiel
 // Created on Thu Aug 22 2024
 //
 //
 //
 // Copyright (c) 2024 Company
 ///////////////////////////////////////////////////////////////////////////////

using Godot;
using Godot.Collections;
using System;


namespace BallentinIsland.Core
{
    public partial class MovementComponent : Node3D
    {
        [Export]
        public bool UseClientPrediction {get; set;}

        [Export]
        public int PredictionBufferSize {get; set;} = 1024;

        [Export]
        public Vector3 TargetPosition {get; set;}

        [Export] 
        public float MovementSpeed { get; set;} = 500.0f;

        private PlayerCharacter parent;
        private NavigationAgent3D navAgent;
        private Vector3 TargetRotation {get; set;}

        private Vector3 nextTargetPos;

        public override void _EnterTree()
        {
            GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(1);
            //SetMultiplayerAuthority(1);
        }


        public override void _Ready()
        {
            // grab parent and store it ( this is where movement will happen )
            parent = GetParent<PlayerCharacter>();
            navAgent = parent.GetNode<NavigationAgent3D>("NavAgent");

            TargetPosition = parent.GlobalPosition;
            GD.Print(GetMultiplayerAuthority());
        }

        public void PerformMovement(double delta)
        {
            if (Multiplayer.IsServer())
            {

                nextTargetPos = navAgent.GetNextPathPosition();
                //GD.Print(nextTargetPos.DistanceTo(parent.GlobalPosition));
                if (nextTargetPos.DistanceTo(parent.GlobalPosition) >= 1)
                {
                    if (Multiplayer.IsServer() || IsMultiplayerAuthority())
                    {
                        Vector3 direction = nextTargetPos - parent.GlobalPosition;
                        //GD.Print($"NOT NORMALIZED: {direction}");
                        direction = direction.Normalized();

                        parent.Velocity = direction * MovementSpeed * (float)delta;
                        parent.MoveAndSlide();

                        if(Multiplayer.IsServer())
                        {
                            TargetPosition = parent.GlobalPosition;
                        }
    
                    }
                }

                if (!Multiplayer.IsServer())
                {
                    parent.GlobalPosition = parent.GlobalPosition.Lerp(TargetPosition, 0.5f);
                }

            }

            //GD.Print(TargetPosition);

            if (!Multiplayer.IsServer())
            {
                parent.GlobalPosition = parent.GlobalPosition.Lerp(TargetPosition, 0.5f);
            }
        }

        public void ClientSetTargetNavPosition(Vector3 targetNavPos)
        {
            //GD.Print($"Network AUTH is : {GetMultiplayerAuthority()}");
            Rpc(nameof(SetTargetNavPosition), targetNavPos);
        }

        [Rpc(MultiplayerApi.RpcMode.Authority)]
        public void SetTargetNavPosition(Vector3 targetNavPos)
        {
            //GD.Print($"In RPC");
            if (Multiplayer.IsServer())
            {
                navAgent.TargetPosition = targetNavPos;
            }

            if (IsMultiplayerAuthority())
            {
                navAgent.TargetPosition = targetNavPos;
            }
        }
    }
}