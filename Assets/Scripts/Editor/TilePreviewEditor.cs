using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileBehaviour))]
public class TilePreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileBehaviour tileBehaviour = (TileBehaviour)target;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
        
        TileType newType = (TileType)EditorGUILayout.EnumPopup("Preview Type", tileBehaviour.GetComponent<Tile>().Type);
        
        if (GUILayout.Button("Apply Type"))
        {
            tileBehaviour.SetType(newType);
            EditorUtility.SetDirty(tileBehaviour.gameObject);
        }
    }
} 