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


namespace BallentinIsland.Core
{
    public partial class MovementComponent : Node3D
    {
        [Export]
        public bool UseClientPrediction {get; set;}
        [Export]
        public int PredictionBufferSize {get; set;} = 1024;
        
        public MovementFrame LastAuthoratativeMovement {get; set;}

        private Node3D parent;
        private MovementFrame[] movementFrames;

        private Vector3 TargetPosition {get; set;}
        private Vector3 TargetRotation {get; set;}


        private int currentFrame = 0;
        private int lastRegisteredFrame = 0;
    

        public override void _Ready()
        {
            // grab parent and store it ( this is where movement will happen )
            parent = GetParent<Node3D>();
            movementFrames = new MovementFrame[PredictionBufferSize];
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!Multiplayer.IsServer())
            {
                ApplyMovementFrame(movementFrames[currentFrame]);
            }

            parent.GlobalPosition = parent.GlobalPosition.Lerp(TargetPosition, 0.5f);
            parent.GlobalRotation = parent.GlobalRotation.Lerp(TargetRotation, 0.5f);
        }

        public void ApplyMovementFrame(MovementFrame movementFrame)
        {
            TargetPosition = movementFrame.Position + movementFrame.MovementDelta;
            TargetRotation = movementFrame.Rotation;

            currentFrame = (movementFrame.FrameId + 1) % PredictionBufferSize;
        }

        // RPCS

        [Rpc(MultiplayerApi.RpcMode.Authority)]
        void ApplyAuthoratativeMovement(MovementFrame newFrame)
        {
            // Operates separate from simple (client side) Apply Movement frame
            // overwrites current frame to be the authoratative frame + 1

            if (!Multiplayer.IsServer())
            {
                LastAuthoratativeMovement = newFrame;
                currentFrame = (newFrame.FrameId + 1) % PredictionBufferSize;
            }
            else
            {
                ApplyMovementFrame(newFrame);
            }
        }



        // ADD Movement frame from the client 
        // ( this will be done from the player character/encapsulating class )
        public void AddMovementFrame(MovementFrame movementFrame)
        {
            // this will be called when we have a movement input from the authoratative client
            movementFrames[currentFrame+1] = movementFrame;

            if (Multiplayer.IsServer())
            {
                Rpc(nameof(ApplyAuthoratativeMovement), movementFrame);
            }
        }

        public MovementFrame CreateMovementFrame(Vector3 position)
        {
            MovementFrame newFrame = new MovementFrame();
            lastRegisteredFrame++;
            newFrame.FrameId = lastRegisteredFrame;
            newFrame.Position = position;

            return newFrame;
        }
        
        public MovementFrame CreateMovementFrame(Vector3 position, Vector3 movementDelta)
        {
            MovementFrame newFrame = CreateMovementFrame(position);
            newFrame.MovementDelta = movementDelta;

            return newFrame;
        }

        
        public MovementFrame CreateMovementFrame(Vector3 position, Vector3 movementDelta, Vector3 rotation)
        {
            MovementFrame newFrame = CreateMovementFrame(position, movementDelta);
            newFrame.Rotation = rotation;

            return newFrame;
        }
        
        // public void CreateMovementFrame(Vector3 position)
        // {
            
        // }
    }
}