using UnityEngine;
using System.Collections;

public class SearchTerm_CreatorName : SearchTerm_Creator {

    public string GivenName
    {
        get { return this._givenName; }
        private set { this._givenName = value; }
    }private string _givenName;

    public string FamilyName
    {
        get { return this._familyName; }
        private set { this._familyName = value; }
    }private string _familyName;

    public SearchTerm_CreatorName(string GivenName, string FamilyName, bool Optional)
        :base("?creatorName", "CreatorName = " +GivenName + " " + FamilyName, Optional)
    {
        this.GivenName = GivenName;
        this.FamilyName = FamilyName;
    }
}
