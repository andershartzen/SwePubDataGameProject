using UnityEngine;
using System.Collections;

public class Publication {

    public string PublicationID
    {
        get { return this._publicationID; }
        private set { this._publicationID = value; }
    }private string _publicationID;

    public string Title
    {
        get { return this._title; }
        private set { this._title = value; }
    }private string _title;

    public string Abstract
    {
        get { return this._abstract; }
        private set { this._abstract = value; }
    } private string _abstract;

    public Publication(string PublicationID,string Title, string Abstract)
    {
        this.PublicationID = PublicationID;
        this.Title = Title;
        this.Abstract = Abstract;
    }
}
