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
    public class MovementFrame
    {
        public int FrameId {get; set;} = -1;
        public Vector3 Position {get; set;} = Vector3.Zero;
        public Vector3 Rotation {get; set;} = Vector3.Zero;
        public Vector3 MovementDelta {get; set;} = Vector3.Zero;


        public Dictionary ToDictionary()
        {
            return new Dictionary
            {
                {"FrameId", FrameId},
                {"Position", Position},
                {"Rotation", Rotation},
                {"MovementDelta", MovementDelta},
            };
        }

        public void FromDictionary(Dictionary dict)
        {
            FrameId = (int)dict["FrameId"];
            Position = (Vector3)dict["Position"];
            Rotation = (Vector3)dict["Rotation"];
            MovementDelta = (Vector3)dict["MovementDelta"];
        }
    }
}