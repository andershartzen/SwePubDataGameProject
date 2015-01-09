using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveCollectedPublications : MonoBehaviour {
    public InputField CollectedPublicationsHolder;
    public ScoreManager ScoreMan;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        System.Text.StringBuilder bob = new System.Text.StringBuilder();
        foreach (Publication pub in this.ScoreMan.CorrectlyGuessed)
        {
            bob.AppendLine("Publication ID: " + pub.PublicationID);
            bob.AppendLine("Publication Title: " + pub.Title);
            //bob.AppendLine("Publication Abstract: " + pub.Abstract);
            //bob.AppendLine();
            bob.AppendLine();
        }
        this.CollectedPublicationsHolder.text = bob.ToString();
    }

    public void Exit()
    {
        this.gameObject.SetActive(false);
    }

    public void SelectAllText()
    { 
        
    }
}
