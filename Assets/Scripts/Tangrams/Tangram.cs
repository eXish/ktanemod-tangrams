using System;
using System.Collections.Generic;
using System.Linq;

public class Tangram
{
    public Tangram(TangramGrid grid, TangramPiece[] pieces)
    {
        Grid = grid;
        Pieces = pieces;

        WalkAllInputs();
    }

    public TangramGrid Grid
    {
        get;
        private set;
    }

    public TangramPiece[] Pieces
    {
        get;
        private set;
    }

    public int ValidInputCount
    {
        get
        {
            return _validInputOutputConnections.GroupBy((x) => x.PointA).Count();
        }
    }

    public string ChipCode
    {
        get
        {
            return string.Join("", Pieces.Select((x) => x.Index.ToString()).ToArray());
        }
    }

    private readonly List<TangramGridConnection> _validInputOutputConnections = new List<TangramGridConnection>();

    public void LogInfo(KMBombModule bombModule)
    {
        bombModule.LogFormat("Generated tangram details: Grid={0}, Code={1}, Pieces={{{2}}}", Grid.name, ChipCode, string.Join(", ", Pieces.Select((x) => x.name).ToArray()));
        bombModule.LogFormat("Possible input/output solutions: [{0}]", string.Join(", ", _validInputOutputConnections.Select(delegate(TangramGridConnection connection)
        {
            int pointAIndex = GetExternalConnectionIndex(connection.PointA);
            int pointBIndex = GetExternalConnectionIndex(connection.PointB);

            return string.Format("{0}→{1}", pointAIndex + 1, pointBIndex + 1);
        }).ToArray()));
    }

    public int GetExternalConnectionIndex(TangramGridConnectionPoint point)
    {
        return Array.IndexOf(Grid.ExternalConnections, point);
    }

    public bool IsValidConnection(TangramGridConnection connection)
    {
        return IsValidConnection(connection.PointA, connection.PointB);
    }

    public bool IsValidConnection(TangramGridConnectionPoint pointA, TangramGridConnectionPoint pointB)
    {
        return _validInputOutputConnections.Any((x) => x.PointA.Equals(pointA) && x.PointB.Equals(pointB));
    }

    private void WalkAllInputs()
    {
        List<TangramGridConnectionPoint> discoveredPoints = new List<TangramGridConnectionPoint>();
        List<TangramGridConnectionPoint> discoveredExternalPoints = new List<TangramGridConnectionPoint>();
        foreach (TangramGridConnectionPoint inputPoint in Grid.ExternalConnections)
        {
            discoveredPoints.Clear();
            discoveredExternalPoints.Clear();
            Walk(inputPoint.CellIndex, inputPoint.CellPointIndex, ref discoveredPoints, ref discoveredExternalPoints);

            foreach (TangramGridConnectionPoint outputPoint in discoveredExternalPoints)
            {
                _validInputOutputConnections.Add(new TangramGridConnection() { PointA = inputPoint, PointB = outputPoint });
            }
        }
    }
    
    private void Walk(int cellIndex, int cellPointIndex, ref List<TangramGridConnectionPoint> discoveredPoints, ref List<TangramGridConnectionPoint> discoveredExternalPoints)
    {
        TangramGridConnectionPoint point = new TangramGridConnectionPoint() { CellIndex = cellIndex, CellPointIndex = cellPointIndex };
        if (discoveredPoints.Contains(point))
        {
            return;
        }

        discoveredPoints.Add(point);

        TangramPieceConnections connections = Pieces[cellIndex].Connectivity[cellPointIndex];
        foreach (int pointConnection in connections.Connections)
        {
            int toCellIndex, toCellPointIndex;
            if (Grid.GetConnection(cellIndex, pointConnection, out toCellIndex, out toCellPointIndex))
            {
                Walk(toCellIndex, toCellPointIndex, ref discoveredPoints, ref discoveredExternalPoints);
            }
            else
            {
                if (Grid.ExternalConnections.Any(x => x.CellIndex == cellIndex && x.CellPointIndex == pointConnection))
                {
                    TangramGridConnectionPoint externalPoint = new TangramGridConnectionPoint() { CellIndex = cellIndex, CellPointIndex = pointConnection };
                    if (!discoveredExternalPoints.Contains(externalPoint) && !externalPoint.Equals(discoveredPoints[0]))
                    {
                        discoveredExternalPoints.Add(externalPoint);
                    }
                }
            }
        }
    }
}
