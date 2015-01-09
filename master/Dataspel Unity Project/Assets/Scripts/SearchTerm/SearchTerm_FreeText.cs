using UnityEngine;
using System.Collections;

public class SearchTerm_FreeText : SearchTerm {

    public string FreeText = string.Empty;

    public SearchTerm_FreeText(string freeText, bool Optional)
    : base("?freeText","freeText = "+freeText, Optional)
    {
        this.FreeText = freeText;
    }

}
