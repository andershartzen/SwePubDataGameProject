using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class University : System.IComparable
{

    public string UniversityName
    {
        get { return this._uniName; }
        private set { this._uniName = value; }
    }
    private string _uniName;

    public string UniversityCode
    {
        get { return this._uniCode; }
        private set { this._uniCode = value; }
    }
    private string _uniCode;

    public string UniversityKeyword
    {
        get { return _uniKeyword; }
        private set { this._uniKeyword = value; }
    }private string _uniKeyword;

    public University(string uniName, string uniCode, string uniKeyword)
    {
        this.UniversityName = uniName;
        this.UniversityCode = uniCode;
        this.UniversityKeyword = uniKeyword;
    }

    public override string ToString()
    {
        return this.UniversityName;
    }



    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        University uni = obj as University;
        if (uni != null)
        {
            return this.UniversityName.CompareTo(uni.UniversityName);
        }
        else
        {
            throw new System.ArgumentException("Object is not a University");
        }
    }
}
