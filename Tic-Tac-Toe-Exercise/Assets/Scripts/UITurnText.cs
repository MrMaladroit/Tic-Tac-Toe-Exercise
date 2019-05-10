using UnityEngine;
using UnityEngine.UI;

public class UITurnText : MonoBehaviour
{
    [SerializeField] private Text turnText;
    [SerializeField] private Text PlayerOnePiece;
    [SerializeField] private Text PlayerTwoPiece;

    public void SetTurnText(Player player)
    {
        turnText.text = player.name + "'s turn";
    }

    public void SetPlayerPieceText(Player[] players)
    {
        PlayerOnePiece.text = "Player 1's Piece : " + players[0].Piece;
        PlayerTwoPiece.text = "Player 2's Piece : " + players[1].Piece;
    }
}
