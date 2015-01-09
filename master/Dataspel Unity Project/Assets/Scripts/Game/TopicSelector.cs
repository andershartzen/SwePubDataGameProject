using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopicSelector : MonoBehaviour {
    public static string SelectedTopic = string.Empty;
    private Button SelectedButton;
    public InputField TopicInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (TopicInput.text != string.Empty)
        {
            SelectedTopic = this.TopicInput.text;
        }
	}

    void OnEnable()
    {
        SelectedTopic = string.Empty;
    }

    public void TopicButtonPressed(Button buttonPressed)
    {
        if (SelectedButton != null)
        {
            SelectedButton.interactable = true;
        }

        SelectedTopic = buttonPressed.GetComponentInChildren<Text>().text;
        SelectedButton = buttonPressed;
        SelectedButton.interactable = false;
    }

    public void AddOrExitButtonPressed()
    {
        this.gameObject.SetActive(false);
    }
}
