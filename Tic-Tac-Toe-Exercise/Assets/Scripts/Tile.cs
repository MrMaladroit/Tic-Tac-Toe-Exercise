using UnityEngine;

public class Tile : MonoBehaviour
{
    public Pieces CurrentPiece { get; private set; }

    public void SetTilePiece(Pieces piece)
    {
        if (piece != Pieces.O || piece != Pieces.X)
        {
            CurrentPiece = piece;
        }
    }
}