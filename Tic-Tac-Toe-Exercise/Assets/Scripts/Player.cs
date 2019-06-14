using UnityEngine;

public class Player : MonoBehaviour
{
    public State Piece { get; private set; }
    public string Name;


    public void SetPlayerPiece(State piece)
    {
        this.Piece = piece;
    }
}
