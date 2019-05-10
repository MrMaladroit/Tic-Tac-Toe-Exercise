using UnityEngine;
using UnityEngine.UI;

public class UITurnText : MonoBehaviour
{
    private Text turnText;

    private void Awake()
    {
        turnText = GetComponent<Text>();
    }

    public void SetTurnText(Player player)
    {
        turnText.text = player.name;
    }
}
