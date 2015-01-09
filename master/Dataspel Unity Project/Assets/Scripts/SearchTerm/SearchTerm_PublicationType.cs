using UnityEngine;
using System.Collections;

public class SearchTerm_PublicationType : SearchTerm_Publication {

    public string PublicationType
    {
        get { return this._publicationType; }
        private set { this._publicationType = value; }
    }private string _publicationType;

    public SearchTerm_PublicationType(string PublicationType, bool Optional)
        : base("?pubType", "Publication type"+PublicationType, Optional)
    {
        this.PublicationType = PublicationType;
    }
}
