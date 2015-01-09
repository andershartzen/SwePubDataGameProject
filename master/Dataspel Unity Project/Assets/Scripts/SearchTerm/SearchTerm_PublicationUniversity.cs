using UnityEngine;
using System.Collections;

public class SearchTerm_PublicationUniversity : SearchTerm {
    public University PublicationUniversity
    {
        get { return this._publicationUniversity; }
        private set { this._publicationUniversity = value; }
    }private University _publicationUniversity;

    public SearchTerm_PublicationUniversity(University PublicationUniversity, bool Optional)
         : base("?pubUniversity", "Publication Uni = "+PublicationUniversity.UniversityName, Optional)
    {
        this.PublicationUniversity = PublicationUniversity;
    }
}
