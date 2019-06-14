using UnityEngine;
using UnityEngine.UI;

public class UITurnText : MonoBehaviour
{
    [SerializeField] private Text turnText;
    [SerializeField] private Text PlayerOnePiece;
    [SerializeField] private Text PlayerTwoPiece;

    public void SetTurnText(Player player)
    {
        turnText.text = player.Name + "'s turn";
    }

    public void SetPlayerPieceText(Player[] players)
    {
        PlayerOnePiece.text = players[0].Name + " : " + players[0].Piece;
        PlayerTwoPiece.text = players[1].Name + " : " + players[1].Piece;
    }
}
