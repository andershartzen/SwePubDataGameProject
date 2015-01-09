using UnityEngine;
using System.Collections;

public class TextAssetManager : MonoBehaviour {

    public static TextAssetManager TextAssetMan;

    public TextAsset SweUniList;
    public TextAsset AuthorBasedOnUni;
    public TextAsset QueryAuthorSearch;
    public TextAsset QueryNoSearch;
    public TextAsset QueryUniSearch;
    public TextAsset QueryUniSearchAuthorSearch;
    public TextAsset SimpleQueryResult1000;

    private string SweUniList_string;
    private string AuthorBasedOnUni_string;
    private string QueryAuthorSearch_string;
    private string QueryNoSearch_string;
    private string QueryUniSearch_string;
    private string QueryUniSearchAuthorSearch_string;

    public string getTextAssetAsString(string txtfile)
    {
        switch (txtfile)
        { 
            case "SweUniList":
                return this.SweUniList_string;
            case "AuthorBasedOnUni":
                return this.AuthorBasedOnUni_string;
            case "QueryAuthorSearch":
                return this.QueryAuthorSearch_string;
            case "QueryNoSearch":
                return this.QueryNoSearch_string;
            case "QueryUniSearch":
                return this.QueryUniSearch_string;
            case "QueryUniSearchAuthorSearch":
                return this.QueryUniSearchAuthorSearch_string;
            default:
                return string.Empty;
        }
    }

	// Use this for initialization
	void Start () {
        TextAssetMan = this;
        //Debug.Log("TextAssetManager Start");

        this.SweUniList_string = this.SweUniList.text;
        this.AuthorBasedOnUni_string = this.AuthorBasedOnUni.text;
        this.QueryAuthorSearch_string = this.QueryAuthorSearch.text;
        this.QueryNoSearch_string = this.QueryNoSearch.text;
        this.QueryUniSearch_string = this.QueryUniSearch.text;
        this.QueryUniSearchAuthorSearch_string = this.QueryUniSearchAuthorSearch.text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
