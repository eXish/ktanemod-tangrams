using System;

[Serializable]
public class TangramGridConnectionPoint
{
    public int CellIndex;
    public int CellPointIndex;

    public override bool Equals(object obj)
    {
        TangramGridConnectionPoint other = obj as TangramGridConnectionPoint;
        if (other != null)
        {
            return CellIndex == other.CellIndex && CellPointIndex == other.CellPointIndex;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
