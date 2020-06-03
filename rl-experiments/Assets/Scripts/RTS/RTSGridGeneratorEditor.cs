using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if( UNITY_EDITOR )

[CustomEditor(typeof(RTSGridGenerator))]
public class RTSGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Generate Grid"))
        {
            RTSGridGenerator myScript = (RTSGridGenerator)target;
            myScript.GenerateGrid();
        }

        if (GUILayout.Button("Delete Grid"))
        {
            RTSGridGenerator myScript = (RTSGridGenerator)target;
            myScript.DeleteGrid();
        }
        DrawDefaultInspector();
    }
}
#endif