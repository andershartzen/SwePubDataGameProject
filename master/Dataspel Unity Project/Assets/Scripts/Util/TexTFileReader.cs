using UnityEngine;
using System.Collections;

public static class TexTFileReader {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="textFileName"></param>
    /// <returns></returns>
    public static string LoadTextFileAsString(string textFileName)
    {
        TextAsset textFile = Resources.Load(textFileName) as TextAsset;
        return textFile.text;
    }
}
