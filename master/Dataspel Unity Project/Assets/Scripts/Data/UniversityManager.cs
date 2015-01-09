using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class UniversityManager : MonoBehaviour {

    public static List<University> UniversityList = new List<University>();
    public static UniversityManager UniMan;
    public TextAssetManager TextAssetMan;

    private bool firstUpdate = true;

    private GUIContent[] UniGUIList;

	// Use this for initialization
	void Start () {
        UniMan = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (UniversityList.Count == 0 && firstUpdate)
        {
            LoadUniversityList();
            firstUpdate = false;
        }
	}

    public University FindUniversityBasedOnCode(string uniCode)
    {
        foreach (University uni in UniversityList)
        {
            if (uni.UniversityCode == uniCode)
            {
                return uni;
            }
        }
        return null;
    }

    public University FindUniversityBasedOnName(string uniName)
    {
        foreach (University uni in UniversityList)
        {
            if (uni.UniversityCode == uniName)
            {
                return uni;
            }
            else if (uni.UniversityName == uniName)
            {
                return uni;
            }
            else if (uni.UniversityName.Contains(uniName))
            {
                return uni;
            }
        }
        return null;
    }

    private void LoadUniversityList()
    {
        string uniList = this.TextAssetMan.getTextAssetAsString("SweUniList");
        StringReader reader = new StringReader(uniList);

        string uniLine = reader.ReadLine();

        while (uniLine != null)
        {
            string[] uniSplit = uniLine.Split(';');
            UniversityList.Add(new University(uniSplit[0], uniSplit[1], uniSplit[2]));
            uniLine = reader.ReadLine();
        }

        UniversityList.Sort();

        this.UniGUIList = new GUIContent[UniversityList.Count];

        for (int i = 0; i < UniversityList.Count; i++)
        {
            
            this.UniGUIList[i] = new GUIContent(UniversityList[i].ToString());
        }
    }

    public GUIContent[] GetUniversityGUIList()
    {
        return this.UniGUIList;
    }


}
