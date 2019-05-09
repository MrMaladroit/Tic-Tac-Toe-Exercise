using UnityEngine;

public class Player : MonoBehaviour
{
    public State state { get; private set; }


    public void SetPlayerPiece(State state)
    {
        this.state = state;
    }
}
