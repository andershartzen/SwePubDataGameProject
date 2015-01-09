using UnityEngine;
using System.Collections;

public class SPARQLQuery  {
    public string Query
    {
        get { return this._query; }
        private set {this._query = value; }
    }private string _query;

    public SPARQLQuery(string Query)
    {
        this.Query = Query;
    }
}
