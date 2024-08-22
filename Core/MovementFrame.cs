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
    public struct MovementFrame
    {
        public int FrameId {get; set;}
        public Vector3 Position {get; set;}
        public Vector3 Rotation {get; set;}
        public Vector3 MovementDelta {get; set;}
    }
}