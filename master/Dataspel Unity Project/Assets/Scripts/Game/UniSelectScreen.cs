using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UniSelectScreen : MonoBehaviour {

    public Button ExitButton;
    public Button NextPageButton;
    public Button PrevPageNutton;
    private int CurrentIndexStart;
    private int IndexChangeConst;
    public List<Button> TitleButtons = new List<Button>();
    public GUIManager GUIMan;
    public Button SelectedButton;
    public static University SelectedUniversity = null;

	// Use this for initialization
	void Start () {
        this.IndexChangeConst = this.TitleButtons.Count;
        this.CurrentIndexStart = 0;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < this.TitleButtons.Count; i++)
        {
            if ((i + this.CurrentIndexStart) < UniversityManager.UniversityList.Count)
            {
                this.TitleButtons[i].enabled = true;
                //string name = UniversityManager.UniversityList[i + this.CurrentIndexStart].UniversityName;
                this.TitleButtons[i].GetComponentInChildren<Text>().text = UniversityManager.UniversityList[i + this.CurrentIndexStart].UniversityName;
            }
            else
            {
                this.TitleButtons[i].enabled = false;
                this.TitleButtons[i].GetComponentInChildren<Text>().text = string.Empty;
            }
        }

        if (UniversityManager.UniversityList.Count - this.CurrentIndexStart <= this.IndexChangeConst)
        {
            this.NextPageButton.enabled = false;
        }
        else if (!this.NextPageButton.enabled)
        {
            this.NextPageButton.enabled = true;
        }

        if (this.CurrentIndexStart - this.IndexChangeConst < 0)
        {
            this.PrevPageNutton.enabled = false;
        }
        else if (!this.PrevPageNutton.enabled)
        {
            this.PrevPageNutton.enabled = true;
        }
	}

    void OnEnable()
    { 
         
    }

    public void NextPage()
    {
        if (this.CurrentIndexStart + this.IndexChangeConst < UniversityManager.UniversityList.Count)
        {
            this.CurrentIndexStart += this.IndexChangeConst;
        }
    }

    public void PreviousPage()
    {
        if (this.CurrentIndexStart - this.IndexChangeConst >= 0)
        {
            this.CurrentIndexStart -= this.IndexChangeConst;
        }
    }

    public void ButtonPress(int buttonPressed)
    {
        if(this.SelectedButton != null)
        {
            this.SelectedButton.interactable = true;
        }
        this.SelectedButton = this.TitleButtons[buttonPressed];
        this.SelectedButton.interactable = false;
        SelectedUniversity = UniversityManager.UniMan.FindUniversityBasedOnName(this.SelectedButton.GetComponentInChildren<Text>().text);
    }

    public void ExitButtonPressed()
    {
        this.gameObject.SetActive(false);
    }
}
