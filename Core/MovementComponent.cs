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


namespace NetworkedMovement
{
    struct MovementFrame
    {
        public int frameId;
        public Vector3 position;
        public Vector3 movementDelta;
    }

    public partial class MovementComponent : Node3D
    {
        [Export]
        public bool UseClientPrediction {get; set;}
        
        private Node3D parent;
        private MovementFrame[] movementFrames = new MovementFrame[1024];


        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // grab parent and store it ( this is where movement will happen )
            parent = GetParent<Node3D>();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}