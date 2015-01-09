using UnityEngine;
using System.Collections;

public class SearchTerm_CreatorUni : SearchTerm_Creator {
    public University CreatorUniversity
    {
        get { return this._creatorUniversity; }
        private set { this._creatorUniversity = value; }
    }private University _creatorUniversity;

    public SearchTerm_CreatorUni(University CreatorUniversity, bool Optional)
        : base("?creatorUni", "creatorUni = " + CreatorUniversity.UniversityName, Optional)
    {
        this.CreatorUniversity = CreatorUniversity;
    }
}
