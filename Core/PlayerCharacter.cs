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
using System;
using System.ComponentModel;

namespace BallentinIsland.Core
{
    public partial class PlayerCharacter : CharacterBody3D
    {
        // Called when the node enters the scene tree for the first time.
        
        [Export]
        public float CameraMovementScreenOffset { get; set; } = 20.0f;// offset where mouse will move the camera rig
        [Export]
        public float CameraSpeed { get; set; } = 5.0f;
        [Export]
        public float CameraZoomStep { get; set; } = 1.0f;

        [Export]
        public float PlayerSpeed { get; set; } = 15.0f;
        
        private MovementComponent Movement { get; set;}
        private DetachableCameraRig CameraRig { get; set; }

        [Export]
        public Vector3 TargetPosition {get; set;}

        [Export] 
        public float MovementSpeed { get; set;} = 500.0f;

        private NavigationAgent3D navAgent;
        private Vector3 nextTargetPos;

        //private NavigationAgent3D NavAgent { get; set; }


        public override void _Ready()
        {
            Movement = GetNodeOrNull<MovementComponent>("MovementComponentBase");
            CameraRig = GetNodeOrNull<DetachableCameraRig>("CameraRig");
            navAgent = GetNodeOrNull<NavigationAgent3D>("NavAgent");

            //NavAgent = GetNodeOrNull<NavigationAgent3D>("NavAgent");

            if (IsMultiplayerAuthority())
            {
                CameraRig.Camera.Current = true;
                //GD.Print($"Setting camera rig as current: {Multiplayer.GetUniqueId()}");
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (IsMultiplayerAuthority())
            {
                CameraRig.Camera.Current = true;
                //GD.Print($"Setting camera rig as current: {Multiplayer.GetUniqueId()}");
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            Movement.PerformMovement(delta);
        }

        
        [Rpc(MultiplayerApi.RpcMode.Authority)]
        public void HandleTargetAction(Vector2 mousePos)
        {
            // movement action

            Vector3 mouseOrigin = CameraRig.Camera.ProjectPosition(mousePos, 0);
            Vector3 mouseRayDirection = CameraRig.Camera.ProjectPosition(mousePos, 1) - mouseOrigin;

            float t = -mouseOrigin.Y / mouseRayDirection.Y;
            
            Vector3 groundPosition = mouseOrigin + t * mouseRayDirection;
            groundPosition.Y = GlobalPosition.Y;

            // NavAgent.TargetPosition = groundPosition;
            
            // if (Multiplayer.IsServer())
            // {
            //     Rpc(nameof(Movement.SetTargetNavPosition), groundPosition);
            // }

            if (IsMultiplayerAuthority() || Multiplayer.IsServer())
            {
                Movement.ClientSetTargetNavPosition(groundPosition);
            }
        }

        public void HandleAttackAction(Vector2 mousePos)
        {
            // click action

            Vector3 mouseOrigin = CameraRig.Camera.ProjectPosition(mousePos, 0);
            Vector3 mouseRayDirection = CameraRig.Camera.ProjectPosition(mousePos, 1) - mouseOrigin;

            float t = -mouseOrigin.Y / mouseRayDirection.Z;
            
            Vector3 groundPosition = mouseOrigin + t * mouseRayDirection;
            groundPosition.Y = GlobalPosition.Y;

            // NavAgent.TargetPosition = groundPosition;

            if (Multiplayer.IsServer())
            {
                navAgent.TargetPosition = groundPosition;
            }

            if (IsMultiplayerAuthority())
            {
                navAgent.TargetPosition = groundPosition;
            }
        }


        public void HandleMouseWheel(float direction)
        {
            CameraRig.MoveVerticalOffset(direction * CameraZoomStep);
        }

        public void HandleMousePosition(double delta, Vector2 mousePos)
        {
            Vector3 cameraMovement = Vector3.Zero;

            if (mousePos.X >= GetViewport().GetVisibleRect().Size.X - CameraMovementScreenOffset)
            {
                cameraMovement.X = 1;
            }
            if (mousePos.X <= CameraMovementScreenOffset)
            {
                cameraMovement.X = -1;
            }

            if (mousePos.Y >= GetViewport().GetVisibleRect().Size.Y - CameraMovementScreenOffset)
            {
                cameraMovement.Z = 1;
            }

            if (mousePos.Y <= CameraMovementScreenOffset)
            {
                cameraMovement.Z = -1;
            }

            CameraRig.Move(cameraMovement.Normalized() * (float)delta * CameraSpeed);
        }

        public void HandleLockCamera()
        {
            CameraRig.ToggleCameraAttachment();
        }



        [Rpc(MultiplayerApi.RpcMode.Authority)]
        public void HandleTargetActionRpc(Vector3 position)
        {
            if (Multiplayer.IsServer())
            {
                Rpc(nameof(Movement.SetTargetNavPosition), position);
                //NavAgent.TargetPosition = position;
            }
        }
    }
}