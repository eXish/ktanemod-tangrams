using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

[CustomEditor(typeof(TangramGrid))]
public class TangramGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TangramGrid tangramGrid = (TangramGrid)target;

        TangramGrid.ConnectionJSON json = JsonConvert.DeserializeObject<TangramGrid.ConnectionJSON>(tangramGrid.AllConnections);
        tangramGrid.Connections = json.Connections;
        tangramGrid.ExternalConnections = json.ExternalConnections;

        TangramChip oldChip = tangramGrid.Chip;
        tangramGrid.Chip = (TangramChip)EditorGUILayout.ObjectField("Chip Prefab", oldChip, typeof(TangramChip), false);
        if (tangramGrid.Chip != oldChip)
        {
            EditorUtility.SetDirty(target);
        }

        string oldCode = tangramGrid.Code;
        tangramGrid.Code = EditorGUILayout.TextField("Code", oldCode);
        if (tangramGrid.Code != oldCode)
        {
            EditorUtility.SetDirty(target);
        }

        Texture oldTexture = tangramGrid.UnderlayTexture;
        tangramGrid.UnderlayTexture = (Texture)EditorGUILayout.ObjectField("Underlay Texture", oldTexture, typeof(Texture), false);

        EditorGUILayout.LabelField("Cells", EditorStyles.boldLabel);
        ArraySizeField("Cell Count", ref tangramGrid.Cells, target);

        if (tangramGrid.Cells != null)
        {
            EditorGUI.indentLevel++;
            for (int cellIndex = 0; cellIndex < tangramGrid.Cells.Length; ++cellIndex)
            {
                TangramShape oldShape = tangramGrid.Cells[cellIndex];
                tangramGrid.Cells[cellIndex] = (TangramShape)EditorGUILayout.EnumPopup(string.Format("Cell {0}", cellIndex + 1), oldShape);
                if (tangramGrid.Cells[cellIndex] != oldShape)
                {
                    EditorUtility.SetDirty(target);
                }
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("Connections", EditorStyles.boldLabel);

        ArraySizeField("Connection Count", ref tangramGrid.Connections, target);
        if (tangramGrid.Connections != null)
        {
            EditorGUI.indentLevel++;

            for (int connectionIndex = 0; connectionIndex < tangramGrid.Connections.Length; ++connectionIndex)
            {
                TangramGridConnection connection = tangramGrid.Connections[connectionIndex];
                if (connection == null)
                {
                    connection = new TangramGridConnection() { PointA = new TangramGridConnectionPoint(), PointB = new TangramGridConnectionPoint() };
                    EditorUtility.SetDirty(target);
                }
                else
                {
                    if (connection.PointA == null)
                    {
                        connection.PointA = new TangramGridConnectionPoint();
                        EditorUtility.SetDirty(target);
                    }

                    if (connection.PointB == null)
                    {
                        connection.PointB = new TangramGridConnectionPoint();
                        EditorUtility.SetDirty(target);
                    }
                }

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(string.Format("Connection {0}", connectionIndex + 1));
                IntField(ref connection.PointA.CellIndex, target);
                IntField(ref connection.PointA.CellPointIndex, target);
                IntField(ref connection.PointB.CellIndex, target);
                IntField(ref connection.PointB.CellPointIndex, target);

                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("External Connections", EditorStyles.boldLabel);

        ArraySizeField("Connection Count", ref tangramGrid.ExternalConnections, target);
        if (tangramGrid.ExternalConnections != null)
        {
            EditorGUI.indentLevel++;

            for (int connectionIndex = 0; connectionIndex < tangramGrid.ExternalConnections.Length; ++connectionIndex)
            {
                TangramGridConnectionPoint connectionPoint = tangramGrid.ExternalConnections[connectionIndex];
                if (connectionPoint == null)
                {
                    connectionPoint = new TangramGridConnectionPoint();
                    EditorUtility.SetDirty(target);
                }                

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(string.Format("Connection {0}", connectionIndex + 1));
                IntField(ref connectionPoint.CellIndex, target);
                IntField(ref connectionPoint.CellPointIndex, target);

                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        json = new TangramGrid.ConnectionJSON() { Connections = tangramGrid.Connections, ExternalConnections = tangramGrid.ExternalConnections };
        tangramGrid.AllConnections = JsonConvert.SerializeObject(json);
    }

    private static void ArraySizeField<T>(string label, ref T[] array, Object target)
    {
        int oldLength = array != null ? array.Length : 0;
        int newLength = EditorGUILayout.IntField(label, oldLength);
        if (newLength > oldLength)
        {
            if (array == null)
            {
                array = new T[newLength];
            }
            else
            {
                ArrayUtility.AddRange(ref array, new T[newLength - oldLength]);
            }
            EditorUtility.SetDirty(target);
        }
        else if (newLength < oldLength)
        {
            for (int count = 0; count < oldLength - newLength; ++count)
            {
                ArrayUtility.RemoveAt(ref array, array.Length - 1);
            }

            EditorUtility.SetDirty(target);
        }
    }

    private static void IntField(ref int value, Object target)
    {
        int oldValue = value;
        value = EditorGUILayout.IntField(value);
        if (value != oldValue)
        {
            EditorUtility.SetDirty(target);
        }        
    }
}
