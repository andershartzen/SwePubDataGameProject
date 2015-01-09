using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorMessage : MonoBehaviour {
    public static string CurrentErrorMessage = string.Empty;
    public Text errorText;
    public GameObject ParentPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        //print("script was enabled");
        this.errorText.text = CurrentErrorMessage;
    }

    public void OKButtonPressed()
    {
        CurrentErrorMessage = string.Empty;
        this.ParentPanel.SetActive(false);
    }

}
