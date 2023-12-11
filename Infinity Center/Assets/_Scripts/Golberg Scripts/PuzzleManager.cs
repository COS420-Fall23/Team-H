/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Transform[] PuzzleObjects;

    public PuzzleState SavePuzzleState()
    {
        PuzzleObjectTransform[] transforms = new PuzzleObjectTransform[PuzzleObjects.Length];
        for (int i = 0; i < PuzzleObjects.Length; i++)
        {
            transforms[i] = PuzzleObjectTransform.FromTransform(PuzzleObjects[i]);
        }

        return new PuzzleState(transforms);
    }
        public void LoadPuzzleState(PuzzleState state)
    {
        for (int i = 0; i < state.ObjectTransforms.Length; i++)
        {
            PuzzleObjects[i].position = state.ObjectTransforms[i].Position;
            PuzzleObjects[i].rotation = state.ObjectTransforms[i].Rotation;
            PuzzleObjects[i].localScale = state.ObjectTransforms[i].Scale;
        }
    }
}
*/
