using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using SemWeb;
using SemWeb.Remote;

public class DataRetrieverSwePub : DataRetriever {
    //http://virhp07.libris.kb.se/sparql
    //old URL: http://hp07.libris.kb.se:8890/sparql
    //auth URL: http://virhp07.libris.kb.se/sparql-auth

    private string Endpoint = string.Empty;
    public bool DebugMode = false;

    public DataRetrieverSwePub()
        : base()
    {
        this.Endpoint = "http://hp07.libris.kb.se:8890/sparql";
        //this.Endpoint = "http://virhp07.libris.kb.se/sparql-auth";
        System.Net.ServicePointManager.Expect100Continue = false; // don't send HTTP Expect: headers which confuse some servers
    }


    

    /// <summary>
    /// Returns null if no match was found
    /// </summary>
    /// <param name="rawUni"></param>
    /// <returns></returns>
    private University MatchUniversity(string rawUni)
    {
        foreach (University uni in UniversityManager.UniversityList)
        {
            if (rawUni.Contains(uni.UniversityName) || rawUni.Contains(uni.UniversityCode))
            {
                return uni;
            }
        }

        return null;
    }

    public override void FindPublication(Request req, int Limit)
    {
        //Step 1 Get Query from searchTerms
        SPARQLQuery query = SPARQLQueryFactory.CreateQueryFromSearchTerms(req, Limit);
        Debug.Log("FindPublication Step 1");

        if (this.DebugMode)
        {
            query = SPARQLQueryFactory.CreateDebugQuery();
        }

        //Step 2 Execute the querry in another thread using the ExecuteSPARQLQuery method
        base.StartThread(new Thread(() => ExecuteSPARQLQuery(query, req)));
        //ExecuteSPARQLQuery(query, req);
        //ExecuteSPARQLQuery(query);
        //Debug.Log("FindPublication Step 2");
    }

    private void ExecuteSPARQLQuery(SPARQLQuery Query, Request req)
    {
        List<Publication> res = new List<Publication>();

        try
        {
            SparqlHttpSource source = new SparqlHttpSource(this.Endpoint);


            source.RunSparqlQuery(Query.Query, new SwePubQuerySink(res));
        }
        catch (Exception e)
        {
            req.Error = true;
            req.ErrorMessage = e.Message;
        }

        Debug.Log("ExecuteSPARQLQuery Result");
        req.RequestResult = res;
        PublicationSearchDone(req);
        Debug.Log("ExecuteSPARQLQuery Event Fired");
    }

    //private List<Publication> ProcessQueryResults(SparqlResultSet resultSet)
    //{
    //    List<Publication> ProcessedPublications = new List<Publication>();
    //    int i1, i2 = 0; //TODO: Move author cleaning into own method
    //    //TODO: Make it easier to change what is extracted via SPARQL
    //    foreach (SparqlResult sparqResult in resultSet.Results)
    //    {
    //        string publicationID = sparqResult.Value("publicationID").ToString();
    //        publicationID = publicationID.Remove(publicationID.IndexOf('^'));
    //        string abstractTxt = sparqResult.Value("abstractTxt").ToString();
    //        abstractTxt = abstractTxt.Remove(abstractTxt.IndexOf('^'));
    //        string titleTxt = sparqResult.Value("titleTxt").ToString();
    //        titleTxt = titleTxt.Remove(titleTxt.IndexOf('^'));
    //        ProcessedPublications.Add(new Publication(publicationID,titleTxt, abstractTxt));
    //    }
    //    return ProcessedPublications;
    //}
}
