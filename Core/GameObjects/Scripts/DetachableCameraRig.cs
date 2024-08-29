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
        public Camera3D Camera { get; set; }

        [Export]
        private bool IsAttached {get; set;} = true;

        [Export]
        private float DefaultVerticalOffset {get; set;}

        [Export]
        private float MinVerticalOffset {get; set;}

        [Export]
        private float MaxVerticalOffset {get; set;}

        private float currentVerticalOffset;

        private Vector3 GlobalPositionOverride {get; set;} = Vector3.Zero;
        private Node3D parent;


        public override void _Ready()
        {
            parent = GetParent<Node3D>();

            Camera = GetNode<Camera3D>("Camera");

            currentVerticalOffset = MaxVerticalOffset;

            if (IsAttached)
            {
                Position = new Vector3(0, currentVerticalOffset, 0);
            }
        }

        public override void _Process(double delta)
        {
            GlobalPosition = GetAlteredPosition();
        }

        public void ToggleCameraAttachment()
        {
            IsAttached = !IsAttached;

            if (!IsAttached)
            {
                GlobalPositionOverride = new Vector3(parent.GlobalPosition.X, currentVerticalOffset, parent.GlobalPosition.Z);
            }
        }

        public void MoveVerticalOffset(float delta)
        {
            if (Position.Y - delta <= MaxVerticalOffset && Position.Y - delta >= MinVerticalOffset)
            {
                currentVerticalOffset -= delta;
            }
        }


        public void Move(Vector3 movementDelta)
        {
            if (!IsAttached)
            {
                GlobalPositionOverride += movementDelta;
            }
        }

        public Vector3 GetAlteredPosition()
        {
            if (IsAttached)
            {
                return new Vector3(parent.GlobalPosition.X, currentVerticalOffset, parent.GlobalPosition.Z);
            }

            return new Vector3(GlobalPositionOverride.X, currentVerticalOffset, GlobalPositionOverride.Z);
        }
    }
}
