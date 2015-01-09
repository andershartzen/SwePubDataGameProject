using UnityEngine;
using System.Collections;

public class SearchTerm_PublicationYear : SearchTerm_Publication {

    public string PublicationYear
    {
        get { return this._publicationYear; }
        private set {this._publicationYear = value; }
    }private string _publicationYear;

    public SearchTerm_PublicationYear(string PublicationYear, bool Optional)
         : base("?pubYear", "Publication year = "+PublicationYear, Optional)
    {
        this.PublicationYear = PublicationYear;
    }
}
