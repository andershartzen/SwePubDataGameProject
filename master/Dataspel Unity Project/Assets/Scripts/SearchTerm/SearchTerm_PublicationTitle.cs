using UnityEngine;
using System.Collections;

public class SearchTerm_PublicationTitle : SearchTerm_Publication {

    public string PublicationTitle
    {
        get { return this._publicationTitle; }
        private set { this._publicationTitle = value; }
    }private string _publicationTitle;

    public SearchTerm_PublicationTitle(string PublicationTitle, bool Optional)
        : base("?pubTitle", "Publication title = "+PublicationTitle, Optional)
    {
        this.PublicationTitle = PublicationTitle;
    }

}
