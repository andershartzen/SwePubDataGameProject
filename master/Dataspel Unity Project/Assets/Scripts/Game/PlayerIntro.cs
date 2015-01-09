using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerIntro : MonoBehaviour {
    public bool FirstShow;
    public GameObject ToShowAfterClose;
    public Button CreditsButton;
    public Text CreditsText;
    public Text IntroText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        if (FirstShow)
        {
            this.CreditsButton.gameObject.SetActive(false);
            this.FirstShow = false;
        }
        else
        {
            this.CreditsButton.gameObject.SetActive(true);
            this.CreditsButton.GetComponentInChildren<Text>().text = "Credits";
        }
        CreditsText.gameObject.SetActive(false);
        IntroText.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        ToShowAfterClose.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void CreditsButtonPressed()
    {
        if (IntroText.gameObject.activeSelf)
        {
            CreditsText.gameObject.SetActive(true);
            IntroText.gameObject.SetActive(false);
            this.CreditsButton.GetComponentInChildren<Text>().text = "Intro";
        }
        else
        {
            CreditsText.gameObject.SetActive(false);
            IntroText.gameObject.SetActive(true);
            this.CreditsButton.GetComponentInChildren<Text>().text = "Credits";
        }
    }
}
