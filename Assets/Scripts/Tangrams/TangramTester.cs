using UnityEngine;

public class TangramTester : MonoBehaviour
{
    public TangramGrid Grid;
    public TangramPiece[] Pieces;

    private KMBombModule _bombModule = null;

    private void Awake()
    {
        _bombModule = gameObject.AddComponent<KMBombModule>();

        Grid.Load();
        foreach (TangramPiece piece in Pieces)
        {
            piece.Load();
        }

        Tangram tangram = new Tangram(Grid, Pieces);
        tangram.LogInfo(_bombModule);
    }
}
