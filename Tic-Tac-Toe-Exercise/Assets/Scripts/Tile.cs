using UnityEngine;

public class Tile : MonoBehaviour
{
    public State CurrentState { get; private set; }

    private void Awake()
    {
        CurrentState = State.Undecided;
    }

    public void SetTilePiece(State state)
    {
        if (state != State.O || state != State.X)
        {
            CurrentState = state;
        }
    }

    public void ClearTile()
    {
        CurrentState = State.Undecided;
        var piece = GetComponentInChildren<SpriteRenderer>();
        if(piece != null)
        {
            Destroy(piece);
        }
    }
}