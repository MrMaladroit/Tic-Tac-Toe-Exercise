using UnityEngine;

public class Player : MonoBehaviour
{
    public Pieces piece { get; private set; }


    public void SetPlayerPiece(Pieces piece)
    {
        this.piece = piece;
    }
}
