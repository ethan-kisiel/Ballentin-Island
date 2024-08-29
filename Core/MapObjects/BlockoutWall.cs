 ///////////////////////////////////////////////////////////////////////////////
 // Project: BallentinIsland
 //
 //
 // Created by Ethan Kisiel
 // Created on Tue Aug 27 2024
 //
 //
 //
 // Copyright (c) 2024 Company
 ///////////////////////////////////////////////////////////////////////////////

using Godot;
using System;


namespace BallentinIsland.Core.MapObjects
{
    [Tool]
    public partial class BlockoutWall : Node3D
    {
        // Called when the node enters the scene tree for the first time.

        private Vector3 _scaleOverride;

        [Export]
        public Vector3 ScaleOverride { 
            get => _scaleOverride;
            set
            {
                _scaleOverride = value;
                OnScaleOverrideChanged();
            }
        }

        public NavigationObstacle3D NavObstacle { get; set; }
        public MeshInstance3D Mesh { get; set; }

        public override void _EnterTree()
        {
            NavObstacle = GetNode<NavigationObstacle3D>("NavObstacle");
            Mesh = GetNode<MeshInstance3D>("Mesh");

            OnScaleOverrideChanged();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.

        private void OnScaleOverrideChanged()
        {
            if (Mesh != null && NavObstacle != null)
            {
                Mesh.Scale = ScaleOverride;
                NavObstacle.Scale = ScaleOverride / 1.5f;
            }
        }
    }
}
