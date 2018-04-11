using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TangramGrid : MonoBehaviour
{
    [Serializable]
    public class ConnectionJSON
    {
        public TangramGridConnection[] Connections;
        public TangramGridConnectionPoint[] ExternalConnections;
    }

    public string Code = null;
    public Texture UnderlayTexture = null;

    public string FullCode
    {
        get
        {
            return string.Format("{0}-{1}", Chip.Type, Code);
        }
    }

    public TangramChip Chip;

    public TangramShape[] Cells;

    [NonSerialized]
    public TangramGridConnection[] Connections;

    [NonSerialized]
    public TangramGridConnectionPoint[] ExternalConnections;

    public string AllConnections;

    private List<TangramGridConnectionPoint[]> _fullConnections = new List<TangramGridConnectionPoint[]>();

    public void Load()
    {
        if (Connections == null || ExternalConnections == null)
        {
            ConnectionJSON jsonData = JsonConvert.DeserializeObject<ConnectionJSON>(AllConnections);
            Connections = jsonData.Connections;
            ExternalConnections = jsonData.ExternalConnections;
        }
    }

    public bool GetConnection(int fromCellIndex, int fromCellPointIndex, out int toCellIndex, out int toCellPointIndex)
    {
        if (_fullConnections.Count == 0)
        {
            BuildConnections();
        }

        TangramGridConnectionPoint connection = _fullConnections[fromCellIndex][fromCellPointIndex];
        if (connection != null)
        {
            toCellIndex = connection.CellIndex;
            toCellPointIndex = connection.CellPointIndex;
            return true;
        }

        toCellIndex = toCellPointIndex = -1;
        return false;
    }

    private void BuildConnections()
    {
        foreach (TangramShape cell in Cells)
        {
            _fullConnections.Add(new TangramGridConnectionPoint[cell.GetConnectionPoints().Length]);
        }

        foreach (TangramGridConnection connection in Connections)
        {
            _fullConnections[connection.PointA.CellIndex][connection.PointA.CellPointIndex] = connection.PointB;
            _fullConnections[connection.PointB.CellIndex][connection.PointB.CellPointIndex] = connection.PointA;
        }
    }
}
