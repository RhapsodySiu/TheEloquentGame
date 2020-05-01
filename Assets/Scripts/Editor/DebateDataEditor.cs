using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DebateDataEditor : EditorWindow
{

    public Debate debate;

    private string gameDataProjectFilePath = "/StreamingAssets/debateData.json";

    [MenuItem ("Window/Debate Data Editor")]
    static void Init()
    {
        DebateDataEditor window = (DebateDataEditor)EditorWindow.GetWindow(typeof(DebateDataEditor));
        window.Show();
    }

    private void OnGUI()
    {
        if (debate != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("debate");

            EditorGUILayout.PropertyField(serializedProperty, true);

            // apply change outside the editor
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }

        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }


    private void LoadGameData()
    {
        
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            debate = JsonUtility.FromJson<Debate> (dataAsJson);
        } else
        {
            debate = new Debate();
        }
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(debate);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }
}
