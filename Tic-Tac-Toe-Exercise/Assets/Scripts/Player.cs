using UnityEngine;

public class Player : MonoBehaviour
{
    public State Piece { get; private set; }


    public void SetPlayerPiece(State piece)
    {
        this.Piece = piece;
    }
}
