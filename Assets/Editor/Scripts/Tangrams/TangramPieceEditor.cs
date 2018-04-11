using UnityEngine;
using UnityEditor;
using System.Linq;
using Newtonsoft.Json;

[CustomEditor(typeof(TangramPiece))]
public class TangramPieceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TangramPiece tangramPiece = (TangramPiece)target;

        tangramPiece.Connectivity = JsonConvert.DeserializeObject<TangramPieceConnections[]>(tangramPiece.Connections);

        TangramShape oldShape = tangramPiece.Shape;
        tangramPiece.Shape = (TangramShape)EditorGUILayout.EnumPopup("Shape", tangramPiece.Shape);
        if (oldShape != tangramPiece.Shape)
        {
            EditorUtility.SetDirty(target);
        }

        int oldIndex = tangramPiece.Index;
        tangramPiece.Index = EditorGUILayout.IntField("Index", tangramPiece.Index);
        if (oldIndex != tangramPiece.Index)
        {
            EditorUtility.SetDirty(target);
        }

        int connectionPointCount = tangramPiece.Shape.GetConnectionPoints().Length;

        if (tangramPiece.Connectivity == null || tangramPiece.Connectivity.Length != connectionPointCount)
        {
            tangramPiece.Connectivity = new TangramPieceConnections[connectionPointCount];
            for (int connectionPointIndex = 0; connectionPointIndex < connectionPointCount; ++connectionPointIndex)
            {
                tangramPiece.Connectivity[connectionPointIndex] = new TangramPieceConnections() { Connections = new int[0] };
            }

            EditorUtility.SetDirty(target);
        }

        for (int connectionPointIndex = 0; connectionPointIndex < connectionPointCount; ++connectionPointIndex)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(connectionPointIndex.ToString(), GUILayout.Width(30.0f));
            for (int endConnectionPointIndex = 0; endConnectionPointIndex < connectionPointCount; ++endConnectionPointIndex)
            {
                if (connectionPointIndex == endConnectionPointIndex)
                {
                    GUI.enabled = false;
                    GUILayout.Toggle(false, endConnectionPointIndex.ToString(), GUILayout.Width(30.0f));
                    GUI.enabled = true;
                }
                else
                {
                    bool oldToggled = tangramPiece.Connectivity[connectionPointIndex].Connections.Contains(endConnectionPointIndex);
                    bool toggled = GUILayout.Toggle(oldToggled, endConnectionPointIndex.ToString(), GUILayout.Width(30.0f));
                    if (toggled != oldToggled)
                    {
                        if (toggled)
                        {
                            ArrayUtility.Add(ref tangramPiece.Connectivity[connectionPointIndex].Connections, endConnectionPointIndex);
                        }
                        else
                        {
                            ArrayUtility.Remove(ref tangramPiece.Connectivity[connectionPointIndex].Connections, endConnectionPointIndex);
                        }

                        EditorUtility.SetDirty(target);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        tangramPiece.Connections = JsonConvert.SerializeObject(tangramPiece.Connectivity);
    }
}
