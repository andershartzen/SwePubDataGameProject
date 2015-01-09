using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AuthorManager : MonoBehaviour {

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<Author> GetAuthorsFromUni(University Uni)
    {
        List<Author> res = new List<Author>();

        TextAsset authors = Resources.Load<TextAsset>(Uni.UniversityCode);

        StringReader reader = new StringReader(authors.text);
        string line = reader.ReadLine();
        while (line != null)
        { 
            string[] split = line.Split(',');
            res.Add(new Author(split[0], split[1]));
            line = reader.ReadLine();
        }
        reader.Close();
        return res;
    }
}
