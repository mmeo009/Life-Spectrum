using LIFESPECTRUM;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSystem))]
public class GameSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameSystem gameSystem = (GameSystem)target;

        if (gameSystem == null)
            return;

        if (GUILayout.Button("Load Story"))
        {
            gameSystem.LoadStoryObject();
        }
        else if (GUILayout.Button("Pick Cards"))
        {
            gameSystem.PickStory();
        }
        else if (GUILayout.Button("Reset Storys"))
        {
            gameSystem.ResetStory();
        }
        else if (GUILayout.Button("Save All Storys"))
        {
            gameSystem.SaveAllStorys();
        }
        else if (GUILayout.Button("Load All Storys"))
        {
            gameSystem.LoadAllData();
        }
    }

}
