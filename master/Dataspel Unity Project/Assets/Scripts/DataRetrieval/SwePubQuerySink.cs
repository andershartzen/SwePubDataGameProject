using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwePubQuerySink : SemWeb.Query.QueryResultSink
{
    private List<Publication> Publications;
    public SwePubQuerySink(List<Publication> Publications)
    {
        this.Publications = Publications;
    }

    public override bool Add(SemWeb.Query.VariableBindings result)
    {
        string publicationID = string.Empty, abstractTxt = string.Empty, titleTxt = string.Empty;

        foreach (SemWeb.Variable var in result.Variables)
        {
            if (var.LocalName != null && result[var] != null)
            {
                if (var.LocalName == "publicationID")
                {
                    publicationID = result[var].ToString();
                    publicationID = publicationID.Remove(publicationID.IndexOf('^'));
                }
                else if (var.LocalName == "abstractTxt")
                {
                    abstractTxt = result[var].ToString();
                    abstractTxt = abstractTxt.Remove(abstractTxt.IndexOf('^'));
                }
                else if (var.LocalName == "titleTxt")
                {
                    titleTxt = result[var].ToString();
                    titleTxt = titleTxt.Remove(titleTxt.IndexOf('^'));
                }                
            }
        }
        this.Publications.Add(new Publication(publicationID, titleTxt, abstractTxt));
        
        return true;       
    }
}
