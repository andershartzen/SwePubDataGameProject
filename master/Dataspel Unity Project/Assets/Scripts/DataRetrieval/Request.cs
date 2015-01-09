using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Request {

    public bool Cancelled
    {
        get { return this._cancelled; }
        set { this._cancelled = value; }
    }private bool _cancelled;

    public bool Error
    {
        get { return this._error; }
        set { this._error = value; }
    }private bool _error;

    public string ErrorMessage
    {
        get { return this._errorMsg; }
        set { this._errorMsg = value; }
    }private string _errorMsg;

    public List<Publication> RequestResult;

    public List<SearchTerm> RequestSearchTerms;

    public Request(List<SearchTerm> RequestSearchTerms)
    {
        this.RequestSearchTerms = RequestSearchTerms;
        this.Cancelled = false;
        this.Error = false;
        this.ErrorMessage = string.Empty;
    }

}
