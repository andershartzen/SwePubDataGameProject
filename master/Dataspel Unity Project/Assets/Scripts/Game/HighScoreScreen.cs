using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreScreen : MonoBehaviour {
    public ScoreManager ScoreMan;
    public Text HighScoreList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.ScoreMan.Highscores.Count == 0)
        {
            return;
        }

        System.Text.StringBuilder bob = new System.Text.StringBuilder();

        foreach (HighScore highAsAKite in this.ScoreMan.Highscores)
        {
            bob.AppendLine(highAsAKite.ToString());
        }
        this.HighScoreList.text = bob.ToString();
	}

    void OnEnable()
    {

    }
}
