using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartPlayerNameEntry : MonoBehaviour {

    public GameObject MainMenuPanel;
    public DropstractOptions Options;
    public InputField NameEntry;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OKButtonPressed()
    {
        Options.FirstPlayerNameOK(this.NameEntry.text);
        this.gameObject.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
}
