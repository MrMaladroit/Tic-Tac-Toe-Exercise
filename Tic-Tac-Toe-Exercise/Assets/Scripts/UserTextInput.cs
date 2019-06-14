using UnityEngine;
using UnityEngine.UI;

public class UserTextInput : MonoBehaviour
{
    [SerializeField] private Text firstPlayerName;
    [SerializeField] private Text secondPlayerName;

    [SerializeField] private Text firstPlaceHolderText;
    [SerializeField] private Text secondPlaceHolderText;

    private int numberOfDefaults = 1;

    public void SetUpInputPanel(Player[] players)
    {
        firstPlayerName.text = players[0].Name;
        secondPlayerName.text = players[1].Name;
    }

    public void SetInputToPlayerName(Player[] players)
    {
        players[0].Name = firstPlayerName.text;
        players[1].Name = secondPlayerName.text;
    }

    public void AmendPlayerName(Player[] players)
    {
        foreach (Player player in players)
        {
            if (string.IsNullOrEmpty(player.Name))
            {
                player.Name = "Player " + numberOfDefaults;
                numberOfDefaults++;
            }
        }
        PlayerPrefs.SetString("First Player Name", players[0].Name);
        PlayerPrefs.SetString("Second Player Name", players[1].Name);
    }
}