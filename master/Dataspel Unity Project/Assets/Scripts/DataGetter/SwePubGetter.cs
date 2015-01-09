using UnityEngine;
using System;
using System.Collections;

public class SwePubGetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //HelloWorld();
        SwePubTest();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void HelloWorld()
    {
        //Fill in the code shown on this page here to build your hello world application
        //Graph g = new Graph();

        //IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));
        //IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
        //ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
        //ILiteralNode bonjourMonde = g.CreateLiteralNode("Bonjour tout le Monde", "fr");

        //g.Assert(new Triple(dotNetRDF, says, helloWorld));
        //g.Assert(new Triple(dotNetRDF, says, bonjourMonde));

        //foreach (Triple t in g.Triples)
        //{
        //    Debug.Log(t.ToString());
        //}
    }

    void SwePubTest()
    {
        Debug.Log("Query Intiated...");
        //Define a remote endpoint
        //Use the DBPedia SPARQL endpoint with the default Graph set to DBPedia
        //SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://hp07.libris.kb.se:8890/sparql"), "");

        //Make a SELECT query against the Endpoint
        //IGraph resGraph = endpoint.QueryWithResultGraph("DESCRIBE");
        //SparqlResultSet results = endpoint.QueryWithResultSet("SELECT DISTINCT ?Concept WHERE {[] a ?Concept}");
        //SparqlResultSet results = endpoint.QueryWithResultSet("SELECT DISTINCT ?Concept WHERE {[] a ?Concept}");
        //SparqlResultSet results = endpoint.QueryWithResultSet("PREFIX sweDat: <http://swepub.kb.se/SwePubAnalysis/model#> SELECT DISTINCT ?article ?abstractTxt ?titleTxt ?UNI WHERE{ ?article a mods_m:Mods . ?article mods_m:hasName ?author . ?article mods_m:hasAbstract ?abstract . ?abstract mods_m:abstractValue ?abstractTxt . ?article mods_m:hasSubject ?subject . ?article mods_m:hasTitleInfo ?titleInfo . ?titleInfo mods_m:hasTitle ?title . ?title mods_m:titleValue ?titleTxt .  ?article sweDat:authorCount ?authorCount . FILTER(?authorCount > 0 ) . OPTIONAL{?author mods_m:hasAffiliation ?affiliation .}  OPTIONAL{?affiliation mods_m:affiliationValue ?UNI .}  } LIMIT 1000");
        //foreach (SparqlResult result in results)
        //{
        //    Debug.Log(result.ToString());
        //}
    }
}
