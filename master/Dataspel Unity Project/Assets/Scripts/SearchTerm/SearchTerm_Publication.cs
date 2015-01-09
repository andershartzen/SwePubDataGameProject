using UnityEngine;
using System.Collections;

public abstract class SearchTerm_Publication : SearchTerm {
    public SearchTerm_Publication(string Denominator, string PrintString, bool Optional)
        :base(Denominator, PrintString, Optional)
    {}
	
}
