using UnityEngine;
using System.Collections;

public class HighScore : System.IComparable {

    public int Score
    {
        get { return this._score; }
        private set { this._score = value; }
    } private int _score = 0;

    public string Date
    {
        get { return this._date; }
        private set { }
    } private string _date = string.Empty;

    public string Name
    {
        get { return this._name; }
        private set { this._name = value; }
    } private string _name = string.Empty;

    public HighScore(int score, string name, string date)
    {
        this.Score = score;
        this.Name = name;
        this.Date = date;
    }

    public override string ToString()
    {
        return this.Score + " " + this.Name + " " + this.Date;
    }

    public string SaveString()
    {
        return this.Score + "," + this.Name + "," + this.Date;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        HighScore HS = obj as HighScore;

        if (HS != null)
        {
            if (this.Score > HS.Score)
            {
                return -1;
            }
            else if (this.Score < HS.Score)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
            throw new System.ArgumentException("Object is not a HighScore");

    }
}
