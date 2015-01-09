using UnityEngine;
using System.Collections;

public class SearchTerm_PublicationTopic : SearchTerm_Publication {

    public string PublicationTopic
    {
        get { return this._publicationTopic; }
        private set { this._publicationTopic = value; }
    }private string _publicationTopic;


    public SearchTerm_PublicationTopic(string PublicationTopic, bool Optional)
        : base("?pubTopic", "Publication topic = "+PublicationTopic, Optional)
    {
        this.PublicationTopic = PublicationTopic;
    }
}
