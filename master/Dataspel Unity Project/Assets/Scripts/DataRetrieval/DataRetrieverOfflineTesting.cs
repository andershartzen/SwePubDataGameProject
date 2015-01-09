using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataRetrieverOfflineTesting : DataRetriever {

    

    public DataRetrieverOfflineTesting()
    {
        
    }

    /// <summary>
    /// This method is currently only used for test purposes and always returns the same publications loaded from a file. This is
    /// useful for doing testing while being offline.
    /// </summary>
    /// <param name="searchTerms"></param>
    /// <param name="Limit"></param>
	public override void FindPublication(Request req, int Limit)
    {
        System.Collections.Generic.List<SearchTerm> searchTerms = req.RequestSearchTerms;
        StringReader reader = new StringReader(TextAssetManager.TextAssetMan.SimpleQueryResult1000.text);
        List<Publication> Result = new List<Publication>();
        string txt = string.Empty;
        txt = reader.ReadLine(); //Skip header line

        while((txt = reader.ReadLine()) != null )
        {
            string[] split = txt.Split(',');
            Result.Add(new Publication(split[0], split[2], split[1]));

        }

        //for (int i = 0; i < Limit; i++)
        //{
        //    Result.Add(new Publication("test" + i, "Test" + i, "The " + i + " test"));
        //}
        req.RequestResult = Result;
        PublicationSearchDone(req);
    }
}
