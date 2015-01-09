using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CollectedPublications : MonoBehaviour {
    public Text AbstractTextHolder;
    public ScoreManager ScoreMan;
    public Button ExitButton;
    public Button NextPageButton;
    public Button PrevPageNutton;
    private int CurrentIndexStart;
    private int IndexChangeConst;
    public List<Button> TitleButtons = new List<Button>();
    public GUIManager GUIMan;

    public void ButtonPressed(int num)
    {
        this.SetAbstracText(this.GetPublication(num).Abstract, this.GetPublication(num).Title, this.GetPublication(num).PublicationID);
    }

    private void SetAbstracText(string abstractText, string title, string ID)
    {
        StringBuilder bob = new StringBuilder();
        bob.AppendLine("Title: " + title);
        bob.AppendLine("ID: " + ID);
        bob.AppendLine("Abstract: ");
        bob.AppendLine(abstractText);
        this.AbstractTextHolder.text = bob.ToString();
    }

    private Publication GetPublication(int numButtonPressed)
    {
        int index = this.CurrentIndexStart + (numButtonPressed);
        return this.ScoreMan.CorrectlyGuessed[index];
    }

	// Use this for initialization
	void Start () {
        this.IndexChangeConst = this.TitleButtons.Count;
        this.CurrentIndexStart = 0;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < this.TitleButtons.Count; i++)
        {
            if ((i + this.CurrentIndexStart) < this.ScoreMan.CorrectlyGuessed.Count)
            {
                this.TitleButtons[i].enabled = true;
                this.TitleButtons[i].GetComponentInChildren<Text>().text = this.ScoreMan.CorrectlyGuessed[i + this.CurrentIndexStart].Title;
            }
            else
            {
                this.TitleButtons[i].enabled = false;
                this.TitleButtons[i].GetComponentInChildren<Text>().text = string.Empty;
            }
        }

        if (this.ScoreMan.CorrectlyGuessed.Count - this.CurrentIndexStart <= this.IndexChangeConst)
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

    public void NextPage()
    {
        if (this.CurrentIndexStart + this.IndexChangeConst < this.ScoreMan.CorrectlyGuessed.Count)
        {
            this.CurrentIndexStart += this.IndexChangeConst;
        }
    }

    public void PreviousPage()
    {
        if (this.CurrentIndexStart - this.IndexChangeConst > 0)
        {
            this.CurrentIndexStart -= this.IndexChangeConst;
        }
    }

    public void SaveCollectedPublications()
    {
        //StringBuilder bob = new StringBuilder();
        //foreach (Publication pub in this.ScoreMan.CorrectlyGuessed)
        //{
        //    bob.AppendLine("Publication ID: " + pub.PublicationID);
        //    bob.AppendLine("Publication Title: " + pub.Title);
        //    //bob.AppendLine("Publication Abstract: " + pub.Abstract);
        //    //bob.AppendLine();
        //    bob.AppendLine();
        //}
        //Application.OpenURL("mailto:?subject=test&body="+bob.ToString());

        this.GUIMan.Panel_SaveCollectedPublications.gameObject.SetActive(true);
    }
}
