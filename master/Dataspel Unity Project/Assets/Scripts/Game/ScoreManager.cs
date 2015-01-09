using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ScoreManager : MonoBehaviour {
    public DropstractOptions GameOptions;

    public int CurrentScore = 1;
    public float CurrentMultiplier = 1.0f;
    public int TotalScore = 1;
    public static float MaxTimeScore = 30;
    public float CurrentTimeScore = 30;

    public float BaseScore = 100.0f;

    public List<Publication> CorrectlyGuessed = new List<Publication>();

    public List<HighScore> Highscores = new List<HighScore>();
    public static string HighScoreStoreKey = "HScore";

	// Use this for initialization
	void Start () {
        this.LoadHighScoreList();
        Puzzle.PuzzleFailed += Puzzle_PuzzleFailed;
	}

    void Puzzle_PuzzleFailed(Puzzle p)
    {
        this.CurrentMultiplier = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.CurrentTimeScore > 0){ 
            this.CurrentTimeScore = (this.CurrentTimeScore - 1* Time.deltaTime);
        }
        else
        {
            this.CurrentTimeScore = 0;
        }

	}

    public void Reset()
    {
        this.AddScoreToHighScore(this.CurrentScore);
        this.CurrentTimeScore = MaxTimeScore;
        this.CurrentMultiplier = 1.0f;
        this.CurrentScore = 0;
    }

    public void NewPuzzle(float newMaxTimeScore)
    {
        MaxTimeScore = newMaxTimeScore;
        this.CurrentTimeScore = MaxTimeScore;
    }

    public void CorrectAnswer(Publication pub)
    {
        this.CorrectlyGuessed.Add(pub);

        if (this.GameOptions.NoScoreMode)
        {
            return;
        }
        else if (this.GameOptions.MultiplierScoreMode)
        {
            this.CurrentMultiplier++;
            this.CurrentScore += (int)(this.BaseScore*this.CurrentMultiplier);
            this.TotalScore += (int)(this.BaseScore * this.CurrentMultiplier);
        }
        else if (this.GameOptions.TimeScoreMode)
        {
            this.CurrentScore += (int) (this.CurrentTimeScore * this.BaseScore);
            this.TotalScore += (int)(this.CurrentTimeScore * this.BaseScore);
        }
        else if (this.GameOptions.TimeAndMultiplierScoreMode)
        {
            this.CurrentScore += (int) (this.CurrentTimeScore * this.BaseScore);
            this.CurrentScore += (int)(this.BaseScore * this.CurrentMultiplier);
            this.TotalScore += (int)(this.CurrentTimeScore * this.BaseScore);
            this.TotalScore += (int)(this.BaseScore * this.CurrentMultiplier);
            this.CurrentMultiplier++;
        }
    }

    public void WrongAnswer()
    {
        if (this.CurrentMultiplier > 1.0f)
        {
            this.CurrentMultiplier--;
        }
        //= 0.0f;
    }

    public string getTimeLeftString()
    {
        return "Time Left: " + Mathf.Round(this.CurrentTimeScore);
    }

    public string getScoreString()
    {
        StringBuilder bob = new StringBuilder();
        bob.AppendLine("Ship Level: " + this.CurrentMultiplier);
        bob.Append("Current Score: " + this.CurrentScore);
        return bob.ToString();    
    }
    void OnApplicationQuit()
    {
        this.SaveHighScoreList();
        PlayerPrefs.Save();        
    }

    private void AddScoreToHighScore(int score)
    {

        this.Highscores.Add(new HighScore(score, this.GameOptions.PlayerName, System.DateTime.Today.Date + "/" + System.DateTime.Today.Month + "/" + System.DateTime.Today.Year));
        this.Highscores.Sort();

        if (this.Highscores.Count > 10)
        {
            this.Highscores.RemoveRange(10, this.Highscores.Count - 10);
        }

        //if (this.Highscores.Count == 0)
        //{ 
        //    this.Highscores.Add(new HighScore(score, this.GameOptions.PlayerName, System.DateTime.Today.Date + "/" + System.DateTime.Today.Month + "/" + System.DateTime.Today.Year));
        //    return;
        //}

        //int shiftIndex = -1;
        //for (int i = 0; i < this.Highscores.Count; i++)
        //{
        //    if (score >= this.Highscores[i].Score)
        //    {
        //        shiftIndex = i;
        //    }
        //}

        //if (shiftIndex == -1)
        //{
        //    return;
        //}

        //List<HighScore> newHighScoreList = new List<HighScore>();

        //for (int i = 0; i < this.Highscores.Count; i++)
        //{
        //    if (i < shiftIndex)
        //    {
        //        newHighScoreList.Add(this.Highscores[i]);
        //    }
        //    else if (i == shiftIndex)
        //    {
        //        newHighScoreList.Add(new HighScore(score, this.GameOptions.PlayerName, System.DateTime.Today.Date + "/" + System.DateTime.Today.Month + "/" + System.DateTime.Today.Year));
        //    }
        //    else
        //    {
        //        newHighScoreList.Add(this.Highscores[i-1]);
        //    }
        //}
        //newHighScoreList.RemoveAt(10);

        //this.Highscores = newHighScoreList;
    }

    private void LoadHighScoreList()
    {
        string highscoreString = PlayerPrefs.GetString(HighScoreStoreKey, string.Empty);
        if (highscoreString == string.Empty)
        {
            return;
        }

        string[] scores = highscoreString.Split(';');

        foreach (string highscore in scores)
        {
            string[] scorevals = highscore.Split(',');

            int score = -1;

            if (!int.TryParse(scorevals[0], out score))
            {
                PlayerPrefs.DeleteKey(HighScoreStoreKey);
            }
            else
            {
                this.Highscores.Add(new HighScore(score, scorevals[1], scorevals[2]));
            }            
        }
        this.Highscores.Sort();
    }

    private void SaveHighScoreList()
    {
        string saveString = string.Empty;
        foreach (HighScore HS in this.Highscores)
        {
            saveString += HS.SaveString() + ";";
        }
        PlayerPrefs.SetString(HighScoreStoreKey, saveString);
    }

    public void DeleteHighScoreList()
    {
        PlayerPrefs.DeleteKey(HighScoreStoreKey);
        this.Highscores.RemoveRange(0, this.Highscores.Count);
    }
}
