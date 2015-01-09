using UnityEngine;
using System.Collections;

public abstract class SearchTerm {

    protected static int counter = 1;
    public string ID
    {
        get { return this._id; }
        protected set { this._id = value; }
    } private string _id;

    public bool Optional
    {
        get { return this._optional; }
        protected set { this._optional = value; }
    }
    private bool _optional;

    private string PrintString = string.Empty;

    public SearchTerm(string Denominator, string PrintString, bool Optional)
    {
        this.ID = Denominator + counter;
        SearchTerm.counter++;
        this.PrintString = PrintString;
        this.Optional = Optional;
    }

    public override string ToString()
    {
        if (this.Optional)
        {
            return this.PrintString + " (Optional)";
        }
        else
        {
            return this.PrintString;
        }
    }

    
}
