using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeAndProgressManager : MonoBehaviour {

    public DropStract GameObj;
    public ScoreManager ScoreObj;
    public Text DisplayText;
    private float TimePlayed = 0.0f;
    private float DisplayTimePlayed = 0.0f;
    private string TimeString = "Time played in total: ";
    private string CollectedString = " Publications collected: ";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (GameObj.GameState == DropStractGameState.GAMEPLAY || GameObj.GameState == DropStractGameState.PUZZLE_COMPLETED)
        //{
        //    this.TimePlayed += Time.smoothDeltaTime;
        //    this.DisplayTimePlayed = Mathf.Round(this.TimePlayed);
        //    this.DisplayText.text = this.TimeString + this.DisplayTimePlayed + this.CollectedString + this.ScoreObj.CorrectlyGuessed.Count;
        //}
        //else
        //{
        //    this.DisplayText.text = string.Empty;
        //}
    }
}
