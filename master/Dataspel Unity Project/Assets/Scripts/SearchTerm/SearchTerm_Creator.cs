using UnityEngine;
using System.Collections;

public abstract class SearchTerm_Creator : SearchTerm{
    public SearchTerm_Creator(string Denominator, string PrintString, bool Optional)
        : base(Denominator, PrintString, Optional)
    { }
}
