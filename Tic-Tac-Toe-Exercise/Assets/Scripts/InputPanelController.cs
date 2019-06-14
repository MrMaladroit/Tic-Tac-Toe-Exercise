using UnityEngine;
using UnityEngine.UI;

public class InputPanelController : MonoBehaviour
{
    [SerializeField] private Text[] inputTexts;   

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

}