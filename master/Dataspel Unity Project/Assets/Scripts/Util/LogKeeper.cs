using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class LogKeeper : MonoBehaviour {
    public DropstractOptions Options;
    public DataSourceManager DataSrcMan;
    public ScoreManager ScoreMan;
    private List<string> LogKeep = new List<string>();
    private string GameStart;
    private float CurrentTime = 0.0f;
	// Use this for initialization
	void Start () {
        DropStract.ButtonClicked += DropStract_ButtonClicked;
        GameStart = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year + " " + System.DateTime.Now.Hour + "_" + System.DateTime.Now.Minute;
        DropStract.GameStateChanged += DropStract_GameStateChanged;
        Puzzle.SolutionSubmitted += Puzzle_SolutionSubmitted;
        DropStract.SearchStarted += DropStract_SearchStarted;
        this.DataSrcMan.DataRetriveObj.PublicationSearchCompleted += DataRetriveObj_PublicationSearchCompleted;
        DropStract.NewPuzzle += DropStract_NewPuzzle;
        ShipManager.ShipCompleted += ShipManager_ShipCompleted;
        DropstractOptions.OptionsExited += DropstractOptions_OptionsExited;
        DropStract.MessageShownToPlayer += DropStract_MessageShownToPlayer;
	}

    void DropStract_MessageShownToPlayer(string message)
    {
        this.AddMsgToLog("Message shown to player: " + message);
    }

    void DropstractOptions_OptionsExited(string optionVals)
    {
        this.AddMsgToLog("OptionValues: " + optionVals);
    }

    void ShipManager_ShipCompleted()
    {
        this.AddMsgToLog("Ship fully upgraded");
    }

    void DropStract_NewPuzzle(Puzzle newPuzzle)
    {
        if(!(newPuzzle is AbstractPuzzle))
        {return;}

        AbstractPuzzle aPuzzle = (AbstractPuzzle) newPuzzle;
        List<string> posAnswers = aPuzzle.GetPossibleAnswers();
        //ToDo: Check for null
        string msg = "New Puzzle Opened - Titles: ";
        for (int i = 0; i < posAnswers.Count; i++)
        {
            msg += i +"= " + posAnswers[i] +" ";
        }

        msg += " - Solution = " + aPuzzle.SolutionPublication.Title + " - Abstract: " + aPuzzle.SolutionPublication.Abstract;
        this.AddMsgToLog(msg);
    }

    void DataRetriveObj_PublicationSearchCompleted(Request resolvedRequest)
    {
        if (resolvedRequest.Cancelled)
        { return; }

        string msg = "Search Completed, found " + resolvedRequest.RequestResult.Count + " publications";
        this.AddMsgToLog(msg);
    }

    void DropStract_SearchStarted(Request req)
    {
        StringBuilder Bob = new StringBuilder();
        Bob.Append("Search started - Searchterms:");

        foreach (SearchTerm st in req.RequestSearchTerms)
        {
            Bob.Append(" ");
            Bob.Append(st.ToString());
        }
        this.AddMsgToLog(Bob.ToString());
    }

    void Puzzle_SolutionSubmitted(bool correctSolution, string answer)
    {
        string corretStr = string.Empty;
        if (correctSolution) { corretStr = "CORRECT"; } else { corretStr = "WRONG"; }
        string msg = "Player submitted answer: " + answer + " was " + corretStr;
        this.AddMsgToLog(msg);
    }

    void DropStract_GameStateChanged(DropStractGameState newState, DropStractGameState oldState)
    {
        this.AddMsgToLog("State Change FROM " + oldState.ToString() + " TO " + newState.ToString());
    }

    void DropStract_ButtonClicked(UnityEngine.UI.Button clicked)
    {
//        clicked.gameObject.name
        this.AddMsgToLog("Button Clicked: " + clicked.gameObject.name);
    }

    void AddMsgToLog(string Msg)
    {
        this.LogKeep.Add("| " + Msg + " | GameTime: " + this.CurrentTime + " sec |");
    }
	
	// Update is called once per frame
	void Update () {
        this.CurrentTime = Time.time;
	}

    void OnApplicationQuit()
    {
        this.AddMsgToLog("Game Ended | Total Score: "+ScoreMan.TotalScore);
        this.SaveLogToDisk();
    }

    public void SaveLogToDisk()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string pcPath = Application.dataPath;
            pcPath += "/../";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(pcPath + Options.PlayerName + "_" + GameStart + ".txt"))
            {
                foreach (string logMsg in this.LogKeep)
                {
                    file.WriteLine(logMsg);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            string macPath = Application.dataPath;
            macPath += "/../../";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(macPath + Options.PlayerName + "_" + GameStart + ".txt"))
            {
                foreach (string logMsg in this.LogKeep)
                {
                    file.WriteLine(logMsg);
                }
            }
        }
    }
    
}
