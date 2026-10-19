using UnityEngine;
using TMPro;

public class InstructionsButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI howToPlayButtonText;
    [SerializeField] private GameObject instructionsGameObject;

    private void Start()
    {
        instructionsGameObject.SetActive(false);
        howToPlayButtonText.text = "How to play";
    }

    public void ToggleInstructions()
    {
        if (instructionsGameObject.activeSelf)
        {
            instructionsGameObject.SetActive(false);
            howToPlayButtonText.text = "How to play";
        }
        else
        {
            instructionsGameObject.SetActive(true);
            howToPlayButtonText.text = "Close";
        }
        
    }
}
