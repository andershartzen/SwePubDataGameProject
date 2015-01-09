using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class DropstractOptions : MonoBehaviour {

    public bool AllTextAtOnceEnabled = false;
    public bool SpreederTextModeEnabled = false;
    public bool StarWarsTextModeEnabled = true;
    public float TextWordsPrSec = 3.0f;

    public bool MenuOptionsEnabled = false;

    public bool NoScoreMode = false;
    public bool TimeScoreMode = false;
    public bool MultiplierScoreMode = false;
    public bool TimeAndMultiplierScoreMode = true;

    public bool SuperSimpleSearchMode = true;

    public GUIManager GUIMAN;

    public bool advancedSearch = false;

    public string PlayerName = "John Doe";
    public InputField PlayerNameInputField;
    public InputField PlayerNameInputFieldStart;

    public delegate void OptionsExitedEventHandler(string optionVals);
    public static event OptionsExitedEventHandler OptionsExited;

	// Use this for initialization
	void Start () {
        //ScoreModeToggleGroup.GetComponentsInChildren(Toggle);
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.MenuOptionsEnabled)
        {
            //if (this.StarWarsTextModeEnabled || this.SpreederTextModeEnabled)
            //{
            //    float f;
            //    if (float.TryParse(GUIMAN.WordsPrSec.value, out f))
            //    {
            //        this.TextWordsPrSec = f;
            //    }

            //}
            this.CheckToggleValues();
        }
	}

    public void ButtonClicked(Object buttonclicked)
    {
        if (buttonclicked == GUIMAN.ExitOptionsButton)
        {
            this.PlayerName = this.PlayerNameInputField.text;
            CheckToggleValues();
            this.FireExitOptionsEvent();
            this.MenuOptionsEnabled = false;
        }
    }

    private void FireExitOptionsEvent()
    {
        string values = "TextMode: AllTextAtOnceEnabled=" + GUIMAN.AllAtOnceReadMode.isOn + " StarWarsTextModeEnabled=" + GUIMAN.StarWarsReadMode.isOn + " SpreederTextModeEnabled=" + GUIMAN.SpreederReadMode.isOn;
        values += " ScoreMode: NoScoreMode=" + GUIMAN.NoScoreScoreMode.isOn + " TimeScoreMode=" + GUIMAN.TimeScoreScoreMode.isOn + " MultiplierScoreMode=" + GUIMAN.MultiplierScoreMode.isOn + " TimeAndMultiplierScoreMode=" + GUIMAN.TimeAndMultiplierScoreMode.isOn;
        values += " AdvancedMode: " + this.advancedSearch;
        values += " PlayerName= " + this.PlayerName;

        if (OptionsExited != null)
        {
            OptionsExited(values);
        }
    }

    public void CheckToggleValues()
    {
        this.AllTextAtOnceEnabled = GUIMAN.AllAtOnceReadMode.isOn;
        this.StarWarsTextModeEnabled = GUIMAN.StarWarsReadMode.isOn;
        this.SpreederTextModeEnabled = GUIMAN.SpreederReadMode.isOn;
        
        this.NoScoreMode = GUIMAN.NoScoreScoreMode.isOn;
        this.TimeScoreMode = GUIMAN.TimeScoreScoreMode.isOn;
        this.MultiplierScoreMode = GUIMAN.MultiplierScoreMode.isOn;
        this.TimeAndMultiplierScoreMode = GUIMAN.TimeAndMultiplierScoreMode.isOn;
    }


    public void OptionToggleChanged(Toggle toggle)
    {
        CheckToggleValues();
        return;
        //Toggle OptionToggle = (Toggle)toggle;
        //if (OptionToggle == GUIMAN.AllAtOnceReadMode)
        //{
        //    //GUIMAN.StarWarsReadMode.isOn = false;
        //    //GUIMAN.SpreederReadMode.isOn = false;
        //    this.ResolveReadModeSelected(0);
        //}
        //else if (OptionToggle == GUIMAN.StarWarsReadMode)
        //{
        //    //GUIMAN.AllAtOnceReadMode.isOn = false;
        //    //GUIMAN.SpreederReadMode.isOn = false;
        //    this.ResolveReadModeSelected(1);
        //}
        //else if (OptionToggle == GUIMAN.SpreederReadMode)
        //{
        //    //GUIMAN.StarWarsReadMode.isOn = false;
        //    //GUIMAN.AllAtOnceReadMode.isOn = false;
        //    this.ResolveReadModeSelected(3);
        //}

        //if (OptionToggle == GUIMAN.NoScoreScoreMode)
        //{
        //    //GUIMAN.TimeScoreScoreMode.isOn = false;
        //    //GUIMAN.MultiplierScoreMode.isOn = false;
        //    //GUIMAN.TimeAndMultiplierScoreMode.isOn = false;
        //    this.ResolveScoreModeSelected(0);
        //}
        //else if (OptionToggle == GUIMAN.TimeScoreScoreMode)
        //{
        //    //GUIMAN.NoScoreScoreMode.isOn = false;
        //    //GUIMAN.MultiplierScoreMode.isOn = false;
        //    //GUIMAN.TimeAndMultiplierScoreMode.isOn = false;
        //    this.ResolveScoreModeSelected(1);
        //}
        //else if (OptionToggle == GUIMAN.MultiplierScoreMode)
        //{
        //    //GUIMAN.TimeScoreScoreMode.isOn = false;
        //    //GUIMAN.NoScoreScoreMode.isOn = false;
        //    //GUIMAN.TimeAndMultiplierScoreMode.isOn = false;
        //    this.ResolveScoreModeSelected(2);
        //}
        //else if (OptionToggle == GUIMAN.TimeAndMultiplierScoreMode)
        //{
        //    //GUIMAN.TimeScoreScoreMode.isOn = false;
        //    //GUIMAN.MultiplierScoreMode.isOn = false;
        //    //GUIMAN.NoScoreScoreMode.isOn = false;
        //    this.ResolveScoreModeSelected(3);
        //}
    }

    private int readModeSelectIndex = 0;
    private string[] readModeSelectionStrings = { "All at once", "Star Wars", "Spreeder"};

    private int scoreModeSelectIndex = 3;
    private string[] scoreModeSelectionStrings = { "No Score", "Time Score", "Multiplier", "Time and Multiplier" };

    void OnGUI(){
        //if (this.MenuOptionsEnabled)
        //{
        //    windowRect = GUI.Window(0, windowRect, WindowFunction, "Options");

        //    //Debug.Log("Exit options");
        //    if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 50, 50, 50), new GUIContent("Exit Options")))
        //    {
        //        this.MenuOptionsEnabled = false;
        //    }

        //}
    }

    private Rect windowRect = new Rect(0, 0, Screen.width, Screen.height);
    private string textSpeedField = string.Empty;

    void WindowFunction(int windowID)
    {
        // Draw any Controls inside the window here
        //GUI.Box(new Rect(10, 30, 500, 120), "Read Mode Options");
        GUI.Label(new Rect(10, 0, 500, 30), "Read Mode Options");
        readModeSelectIndex = GUI.SelectionGrid(new Rect(30, 30, 500, 100), readModeSelectIndex, readModeSelectionStrings, 2);
        this.ResolveReadModeSelected(readModeSelectIndex);
        //GUI.Box(new Rect(10, 170, 500, 130), "Read Mode Options");
        GUI.Label(new Rect(10, 170, 500, 30), "Score Mode Options");
        scoreModeSelectIndex = GUI.SelectionGrid(new Rect(30, 200, 300, 100), scoreModeSelectIndex, scoreModeSelectionStrings, 2);
        this.ResolveScoreModeSelected(scoreModeSelectIndex);

        if (this.StarWarsTextModeEnabled || this.SpreederTextModeEnabled)
        {
            GUI.Label(new Rect(400, 150, 250, 30), "Words pr Sec");
            this.textSpeedField = GUI.TextField(new Rect(400, 170, 100, 30), this.textSpeedField);
            float f;
            if (float.TryParse(this.textSpeedField, out f))
            {
                this.TextWordsPrSec = f;
            }

        }

        
    }

    private void ResolveReadModeSelected(int selectIndex)
    {
        if (selectIndex == 0)
        {
            this.AllTextAtOnceEnabled = true;
            this.StarWarsTextModeEnabled = false;
            this.SpreederTextModeEnabled = false;
        }
        else if (selectIndex == 1)
        {
            this.AllTextAtOnceEnabled = false;
            this.StarWarsTextModeEnabled = true;
            this.SpreederTextModeEnabled = false;
        }
        else if (selectIndex == 2)
        {
            this.AllTextAtOnceEnabled = false;
            this.StarWarsTextModeEnabled = false;
            this.SpreederTextModeEnabled = true;
        }
    }

    private void ResolveScoreModeSelected(int selectIndex)
    {
        if (selectIndex == 0)
        {
            this.NoScoreMode = true;
            this.TimeScoreMode = false;
            this.MultiplierScoreMode = false;
            this.TimeAndMultiplierScoreMode = false;

        }
        else if (selectIndex == 1)
        {
            this.NoScoreMode = false;
            this.TimeScoreMode = true;
            this.MultiplierScoreMode = false;
            this.TimeAndMultiplierScoreMode = false;
        }
        else if (selectIndex == 2)
        {
            this.NoScoreMode = false;
            this.TimeScoreMode = false;
            this.MultiplierScoreMode = true;
            this.TimeAndMultiplierScoreMode = false;
        }
        else if (selectIndex == 3)
        {
            this.NoScoreMode = false;
            this.TimeScoreMode = false;
            this.MultiplierScoreMode = false;
            this.TimeAndMultiplierScoreMode = true;
        }
    }

    public void AdvancedToggle(bool newValue)
    {
        this.advancedSearch = newValue;
    }

    public void FirstPlayerNameOK(string name)
    {
        this.PlayerName = name;
    }

    public void MenuOpened()
    {
        this.PlayerNameInputField.text = this.PlayerName;
    }
}
