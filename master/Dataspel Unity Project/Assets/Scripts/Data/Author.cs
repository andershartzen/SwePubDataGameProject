using UnityEngine;
using System.Collections;

public class Author {

    public string GivenName
    {
        get { return this._givenName;}
        private set {this._givenName = value;}
    }
    private string _givenName;

    public string FamilyName
    {
        get { return this._familyName;}
        private set {this._familyName = value;}
    }
    private string _familyName;

    public string FullName
    {
        get { return this.GivenName + " " + this.FamilyName; }
    }

    public Author(string firstName, string familyName)
    {
        this.GivenName = firstName;
        this.FamilyName = familyName;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
