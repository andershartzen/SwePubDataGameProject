using UnityEngine;
using System.Collections;

public abstract class Puzzle {

    public delegate void SolutionSubmittedEventHandler(bool correctSolution, string answerSubmitted);
    public static event SolutionSubmittedEventHandler SolutionSubmitted;

    public delegate void PuzzleFailedEventHandler(Puzzle p);
    public static event PuzzleFailedEventHandler PuzzleFailed;

    public bool Solved
    {
        get { return this._solved; }
        set { this._solved = value; }
    }private bool _solved;

    public int Attempts
    {
        get {return this._attempts ;}
        protected set { this._attempts = value; }
    }private int _attempts;

    public bool Failed
    {
        get { return this._failed; }
        set { this._failed = value;
        if (value == true && PuzzleFailed != null)
        {PuzzleFailed(this);}}
    }private bool _failed;

    public Puzzle()
    {
        this.Solved = false;
        this.Failed = false;
        this.Attempts = 0;
    }

    protected void FireSolutionSubmittedEvent(bool correctSolution, string answerSubmitted)
    {
        if (SolutionSubmitted != null)
        {
            SolutionSubmitted(correctSolution, answerSubmitted);
        }
    }
}
