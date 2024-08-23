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
using System.Runtime.CompilerServices;

namespace BallentinIsland.Core
{
    public partial class DetachableCameraRig : Node3D
    {
        
        [Export]
        private bool IsAttached {get; set;}

        [Export]
        private float DefaultVerticalOffset {get; set;}

        [Export]
        private float MinVerticalOffset {get; set;}

        [Export]
        private float MaxVerticalOffset {get; set;}

        private float currentVerticalOffset;

        private Vector3 GlobalPositionOverride {get; set;}
        private Camera3D camera;
        private Node3D parent;


        public override void _Ready()
        {
            parent = GetParent<Node3D>();

            camera = GetNode<Camera3D>("Camera");
        }

        public override void _Process(double delta)
        {
            if (!IsAttached)
            {
                Position = parent.GlobalPosition - GlobalPositionOverride;
            }
        }

        public void ToggleCameraAttachment()
        {
            IsAttached = !IsAttached;

            if (IsAttached)
            {
                Position = new Vector3(0, currentVerticalOffset, 0);
            }
        }

        public void MoveVerticalOffset(float delta)
        {
            if (Position.Y + delta <= MaxVerticalOffset && Position.Y >= MinVerticalOffset)
            {
                currentVerticalOffset += delta;
                Position = new Vector3(Position.X, currentVerticalOffset, Position.Z);
            }
        }


        public void Move(Vector3 movementDelta)
        {
            if (!IsAttached)
            {
                movementDelta.Y = 0;
                GlobalPositionOverride += movementDelta;
            }
        }
    }
}
