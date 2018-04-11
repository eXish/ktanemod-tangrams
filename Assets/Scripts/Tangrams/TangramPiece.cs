using System;
using Newtonsoft.Json;
using UnityEngine;

public class TangramPiece : MonoBehaviour
{
    public TangramShape Shape;
    public int Index;

    [NonSerialized]
    public TangramPieceConnections[] Connectivity;

    public string Connections;

    public void Load()
    {
        if (Connectivity == null)
        {
            Connectivity = JsonConvert.DeserializeObject<TangramPieceConnections[]>(Connections);
        }
    }
}
