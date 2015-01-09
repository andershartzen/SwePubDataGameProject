using UnityEngine;
using System.Collections;
using System.Text;

public class TextDisplayer {

    public string FullText
    { 
    get {return this._fullText; }
    protected set {this._fullText = value;}    
    }
    private string _fullText;

    private float TimeSinceStart = 0.0f;
    private float TextWordsPrSec = 0.0f;
    private bool spreeder = false;

    private string[] textArray;

    private float SWScrollStart = 0.0f;
    private float SWScrollEnd = 1.0f;

    public TextDisplayer(string fullText, float TextWordsPrSec, bool spreeder)
    {
        this.FullText = fullText;
        this.TextWordsPrSec = TextWordsPrSec;
        this.spreeder = spreeder;
        this.textArray = this.FullText.Split(' ');
    }

    public void Update(float deltaTime)
    {
        this.TimeSinceStart += deltaTime;
    }

    public string getDisplayString()
    {
        int index = (int) (this.TextWordsPrSec * this.TimeSinceStart);
        
        if (this.spreeder && index < this.textArray.Length)
        {
            return this.textArray[index];
        }
        else if (!this.spreeder)
        {
            StringBuilder bob = new StringBuilder();

            for (int i = 0; i < index && i < this.textArray.Length; i++)
            {
                bob.Append(this.textArray[i]); bob.Append(" ") ;
            }
            //Debug.Log(bob.ToString());
            return bob.ToString();
        }

        return string.Empty;
    }

}
