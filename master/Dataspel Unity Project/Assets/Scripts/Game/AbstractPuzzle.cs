using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractPuzzle : Puzzle {

    private List<Publication> Publications;

    private List<Publication> AttemptedAnswers;

    private bool[] AnswerAttempted = new bool[4];

    public Publication SolutionPublication
    {
        get { return this._solutionPublication; }
        private set { this._solutionPublication = value; }
    }private Publication _solutionPublication;

    public AbstractPuzzle(List<Publication> Publications)
        : base()
    {
        this.Publications = Publications;
        this.AttemptedAnswers = new List<Publication>();
        this.SelectSolutionPublication(this.Publications);
        this.AnswerAttempted[0] = false;
        this.AnswerAttempted[1] = false;
        this.AnswerAttempted[2] = false;
        this.AnswerAttempted[3] = false;
    }

    private void SelectSolutionPublication(List<Publication> Publications)
    {
        if (Publications.Count > 0)
        {
           
            int index = ThreadSafeRandom.ThisThreadsRandom.Next(0, Publications.Count);
            this.SolutionPublication = Publications[index];
            //Debug.Log("SOL " + index);
        }
    }

    public List<string> GetPossibleAnswers()
    {
        List<string> answers = new List<string>();
        foreach (Publication pub in this.Publications)
        {
            answers.Add(pub.Title);
        }
        return answers;
    }

    public List<Publication> GetAttemptedAnswers()
    {
        return this.AttemptedAnswers;
    }

    public bool AttemptedTitle(string title)
    {
        foreach (Publication pub in this.AttemptedAnswers)
        {
            if (pub.Title == title)
            {
                return true;
            }
        }

        return false;
    }

    public bool AttemptedTitle(int index)
    {
        return this.AnswerAttempted[index];
    }

    public bool CheckAnswer(string answer)
    {
        this.Attempts++;
        if (answer == this.SolutionPublication.Title)
        {
            this.Solved = true;
            base.FireSolutionSubmittedEvent(true, answer);
            return true;
        }
        else
        {
            Publication toRemove = null;

            for (int i = 0; i < this.Publications.Count; i++)
            {
                if (this.Publications[i].Title == answer)
                {
                    this.AnswerAttempted[i] = true;
                }
            }

                foreach (Publication pub in this.Publications)
                {
                    if (pub.Title == answer)
                    {
                        toRemove = pub;
                    }
                }
            //this.Publications.Remove(toRemove);
            this.AttemptedAnswers.Add(toRemove);
            base.FireSolutionSubmittedEvent(false, answer);
            return false;
        }        
    }


	
}
