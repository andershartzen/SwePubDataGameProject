using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SPARQLQueryFactory  {

    private static string publication = "?publication";
    private static string publicationID = "?publicationID";
    private static string abstractTxtID = "?abstractTxt";
    private static string titleTxtID = "?titleTxt";
    
    private static string pubUniID = "?pubUNI";
    private static string authorUniID = "?authorUNI";
    private static string authorGiveNameID = "?authorNameGiven";
    private static string authorFamilyNameID = "?authorNameFamily";
    
    private static string Prefix = "PREFIX sweDat: <http://swepub.kb.se/SwePubAnalysis/model#>";
    private static string Prefix2 = "PREFIX mods_m: <http://swepub.kb.se/mods/model#>";
    private static string Header = "SELECT DISTINCT " + publicationID + " " + abstractTxtID + " " + titleTxtID + " ";
    private static string WhereBegin = " WHERE { " + publication + " a mods_m:Mods . " + publication + " swpa_m:localID " + publicationID + ". " + publication + " mods_m:hasAbstract ?abstract . ?abstract mods_m:abstractValue " + abstractTxtID + " . "+ publication +" mods_m:hasTitleInfo ?titleInfo . ?titleInfo mods_m:hasTitle ?title . ?title mods_m:titleValue " + titleTxtID + " . ";
    private static string WhereBasic = publication + " a mods_m:Mods . " + publication + " swpa_m:localID " + publicationID + ". " + publication + " mods_m:hasAbstract ?abstract . ?abstract mods_m:abstractValue " + abstractTxtID + " . " + publication + " mods_m:hasTitleInfo ?titleInfo . ?titleInfo mods_m:hasTitle ?title . ?title mods_m:titleValue " + titleTxtID + " . ";
    private static string End = "} LIMIT ";

    private static List<SearchTerm_FreeText> FreeTexts = new List<SearchTerm_FreeText>();
    public static SPARQLQuery CreateQueryFromSearchTerms(Request req, int Limit)
    {
        List<SearchTerm> searchTerms = req.RequestSearchTerms;

        string Query = string.Empty;

        Query += Prefix;
        Query += Prefix2;
        Query += Header;
        Query += WhereBegin;

        //First iteration - resolve non-free text searches
        foreach (SearchTerm searchTerm in searchTerms)
        {
            if (searchTerm is SearchTerm_FreeText)
            {
                FreeTexts.Add(searchTerm as SearchTerm_FreeText);
                continue;
            }
            Query += " ";
            Query += ResolveSearchTerm(searchTerm);
        }

        //Second Iteration - take care of 
        for (int i = 0; i < FreeTexts.Count; i++)
        {
            if (i > 0)
            {
                Query += " UNION ";
            }
            Query += ResolveSearchTerm(FreeTexts[i] as SearchTerm);
        }           

        Query += End;
        Query += Limit;

        FreeTexts.Clear();

        Debug.Log(Query);

        return new SPARQLQuery(Query);
    }

    private static string ResolveSearchTerm(SearchTerm searchTerm)
    {
        string resolvedQuery = string.Empty;
        if (searchTerm is SearchTerm_CreatorName)
        {
            resolvedQuery = resolveSearchTerm_CreatorName(searchTerm as SearchTerm_CreatorName);
        }
        else if (searchTerm is SearchTerm_CreatorUni)
        {
            resolvedQuery = resolveSearchTerm_CreatorUni(searchTerm as SearchTerm_CreatorUni);
        }
        else if (searchTerm is SearchTerm_PublicationTitle) //TODO: Multiple words in search?
        {
            resolvedQuery = resolveSearchTerm_PublicationTitle(searchTerm as SearchTerm_PublicationTitle);
        }
        else if (searchTerm is SearchTerm_PublicationTopic)
        {
            resolvedQuery = resolveSearchTerm_PublicationTopic(searchTerm as SearchTerm_PublicationTopic);
        }
        else if (searchTerm is SearchTerm_PublicationType)
        {
            throw new System.NotImplementedException("Search using Publication Type not implemented"); //TODO: Search using publication type.
        }
        else if (searchTerm is SearchTerm_PublicationUniversity)
        {
            resolvedQuery = resolveSearchTerm_PublicationUniversity(searchTerm as SearchTerm_PublicationUniversity);
        }
        else if (searchTerm is SearchTerm_PublicationYear)
        {
            resolvedQuery = resolveSearchTerm_PublicationYear(searchTerm as SearchTerm_PublicationYear);
        }
        else if(searchTerm is SearchTerm_FreeText)
        {
            resolvedQuery = resolveSearchTerm_FreeText(searchTerm as SearchTerm_FreeText);
        }

        if (resolvedQuery == string.Empty)
        { throw new System.Exception("Search term not correctly resolved"); }

        if (searchTerm.Optional)
        {
            return "OPTIONAL{ " + resolvedQuery + "}";
        }
        else
        {
            return resolvedQuery;
        }
    }

    public static SPARQLQuery CreateDebugQuery()
    {
        return new SPARQLQuery("PREFIX sweDat: <http://swepub.kb.se/SwePubAnalysis/model#> SELECT DISTINCT ?publicationID ?abstractTxt ?titleTxt WHERE { ?publication a mods_m:Mods . ?publication swpa_m:localID ?publicationID . ?publication mods_m:hasAbstract ?abstract . ?abstract mods_m:abstractValue ?abstractTxt . ?publication mods_m:hasTitleInfo ?titleInfo . ?titleInfo mods_m:hasTitle ?title . ?title mods_m:titleValue ?titleTxt . } LIMIT 100 ");
    }

    #region Internal Helper Methods

    private static string resolveSearchTerm_CreatorName(SearchTerm_CreatorName searchTerm)
    {
        string line1 = publication + " mods_m:hasName " + searchTerm.ID + " . ";
        string line2 = searchTerm.ID + " mods_m:hasNamePart " + searchTerm.ID + "NamePartOne . ";
        string line3 = searchTerm.ID + " mods_m:hasNamePart " + searchTerm.ID + "NamePartTwo . ";
        string line4 = searchTerm.ID + "NamePartOne" + " mods_m:type " + searchTerm.ID + "NamePartOne" + "TypeOne . ";
        string line5 = searchTerm.ID + "NamePartOne" + "TypeOne" + " bif:contains 'given' . ";
        string line6 = searchTerm.ID + "NamePartTwo" + " mods_m:type " + searchTerm.ID + "NamePartTwo" + "TypeTwo . ";
        string line7 = searchTerm.ID + "NamePartTwo" + "TypeTwo" + " bif:contains 'family' . ";
        string line8 = searchTerm.ID + "NamePartOne" + " mods_m:namePartValue " + searchTerm.ID + "NamePartOneVal . ";
        string line9 = searchTerm.ID + "NamePartTwo" + " mods_m:namePartValue " + searchTerm.ID + "NamePartTwoVal . ";

        string line10 = string.Empty, line11 = string.Empty;

        if (searchTerm.GivenName != string.Empty)
        { 
            line10 = searchTerm.ID + "NamePartOneVal" + " bif:contains " + "'" + '"' + searchTerm.GivenName + '"' + "' . "; 
        }
        if (searchTerm.FamilyName != string.Empty)
        {
            line11 = searchTerm.ID + "NamePartTwoVal" + " bif:contains " + "'" + '"' + searchTerm.FamilyName + '"' + "' . ";
        }
       
        //string line8 = searchTerm.ID + "NamePartOne" + " mods_m:namePartValue " + "'" + (searchTerm as SearchTerm_CreatorName).GivenName + "' . ";
        //string line9 = searchTerm.ID + "NamePartTwo" + " mods_m:namePartValue " + "'" + (searchTerm as SearchTerm_CreatorName).FamilyName + "' . ";

        return (line1 + line2 + line3 + line4 + line5 + line6 + line7 + line8 + line9 + line10 + line11);
    }

    private static string resolveSearchTerm_CreatorUni(SearchTerm_CreatorUni searchTerm)
    {
        string line1 = publication + " mods_m:hasName " + searchTerm.ID + " . ";
        string line2 = searchTerm.ID + " swpa_m:affiliationID " + searchTerm.ID + "UNI .";
        string line3 = searchTerm.ID + "UNI bif:contains " + "'" + searchTerm.CreatorUniversity.UniversityCode + "' . ";
        return (line1 + line2 + line3);
    }

    private static string resolveSearchTerm_PublicationTitle(SearchTerm_PublicationTitle searchTerm)
    {
        string line1 = titleTxtID + " bif:contains '\"" + searchTerm.PublicationTitle + "\"' . ";
        return line1;
    }

    private static string resolveSearchTerm_PublicationTopic(SearchTerm_PublicationTopic searchTerm)
    {
        string line1 = publication + " mods_m:hasSubject " + searchTerm.ID + "Subject . ";
        string line2 = searchTerm.ID + "Subject mods_m:hasTopic " + searchTerm.ID + "Topic .";
        string line3 = searchTerm.ID + "Topic mods_m:topicValue " + searchTerm.ID + "TopicValue .";
        string line4 = searchTerm.ID + "TopicValue bif:contains '\"" + searchTerm.PublicationTopic + "\"' . ";
        return (line1 + line2 + line3 + line4);
    }

    private static string resolveSearchTerm_PublicationUniversity(SearchTerm_PublicationUniversity searchTerm)
    {
        string line1 = publication + " mods_m:recordContentSourceValue " + searchTerm.ID + "UNI . ";
        string line2 = searchTerm.ID + "UNI bif:contains '" + searchTerm.PublicationUniversity.UniversityCode + "' . ";
        return (line1 + line2);
    }

    private static string resolveSearchTerm_PublicationYear(SearchTerm_PublicationYear searchTerm)
    {
        return publication + " swpa_m:publicationYear " + searchTerm.PublicationYear + ". ";
    }

    private static string resolveSearchTerm_FreeText(SearchTerm_FreeText searchTerm)
    {
        string resolvedQuery = string.Empty;
        resolvedQuery += "{";
        resolvedQuery += WhereBasic;
        resolvedQuery += resolveSearchTerm_CreatorName(processNameFreeText(searchTerm.FreeText));
        resolvedQuery += "}";
        resolvedQuery += "UNION";

        //UNI
        University uni = UniversityManager.UniMan.FindUniversityBasedOnName(searchTerm.FreeText);
        if(uni != null)
        {
            resolvedQuery += "{";
            resolvedQuery += WhereBasic;
            resolvedQuery += resolveSearchTerm_CreatorUni(new SearchTerm_CreatorUni(uni,false));
            resolvedQuery += "}";        
            resolvedQuery += "UNION";
        }

        resolvedQuery += "{";
        resolvedQuery += WhereBasic;
        resolvedQuery += resolveSearchTerm_PublicationTitle(new SearchTerm_PublicationTitle(searchTerm.FreeText, false));
        resolvedQuery += "}";
        resolvedQuery += "UNION";
        resolvedQuery += "{";
        resolvedQuery += WhereBasic;
        resolvedQuery += resolveSearchTerm_PublicationTopic(new SearchTerm_PublicationTopic(searchTerm.FreeText, false));
        resolvedQuery += "}";
        if(uni != null)
        {
            resolvedQuery += "UNION";
            resolvedQuery += "{";
            resolvedQuery += WhereBasic;
            resolvedQuery += resolveSearchTerm_PublicationUniversity(new SearchTerm_PublicationUniversity(UniversityManager.UniMan.FindUniversityBasedOnName(searchTerm.FreeText), false));
            resolvedQuery += "}";
        }        
        if (ProcessPublicationYear(searchTerm.FreeText) != null)
        {
            resolvedQuery += "UNION";
            resolvedQuery += "{";
            resolvedQuery += WhereBasic;
            resolvedQuery += resolveSearchTerm_PublicationYear(ProcessPublicationYear(searchTerm.FreeText));
            resolvedQuery += "}";
        }


        return resolvedQuery;
    }

    private static SearchTerm_CreatorName processNameFreeText(string freetext)
    {
        string firstname = string.Empty, lastname = string.Empty;
        string[] bananasplit = freetext.Split(' ');
        if (bananasplit.Length >= 2)
        {
            firstname = bananasplit[0];
            for (int i = 1; i < bananasplit.Length; i++)
            {
                lastname += bananasplit[i];
            }
        }
        else
        {
            lastname = bananasplit[0];
        }

        return new SearchTerm_CreatorName(firstname, lastname, false);
    }

    private static SearchTerm_PublicationYear ProcessPublicationYear(string freeText)
    {
        int year = 0;

        if (int.TryParse(freeText,out year))
        {
            char[] freetextchars = freeText.ToCharArray();
            if (freetextchars.Length == 4)
            {
                return new SearchTerm_PublicationYear(year.ToString(), false);
            }
        }
        return null;
    }

    #endregion

}
