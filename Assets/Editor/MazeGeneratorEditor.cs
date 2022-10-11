using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MazeGenerator mazeGenerator = (MazeGenerator)target;
        if (GUILayout.Button("Generate Maze")){
            mazeGenerator.CreateMaze(true);
        }
    }
}
