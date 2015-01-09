using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
    public Text ScoreText;
    public Text TimeLeftText;

    //Main Menu
    public Button CreatePuzzleButton;
    public Button OptionsButton;
    public Button ExitButton;
    public Button CollectedPublications;
    public Button HighScoreButton;
    public Button AboutButton;

    //Options Menu
    public Toggle AllAtOnceReadMode;
    public Toggle StarWarsReadMode;
    public Toggle SpreederReadMode;
    public Toggle NoScoreScoreMode;
    public Toggle TimeScoreScoreMode;
    public Toggle MultiplierScoreMode;
    public Toggle TimeAndMultiplierScoreMode;
    public Button ExitOptionsButton;
    public InputField WordsPrSec;

    //GameSetup
    public Toggle CreatorName;
    public Toggle PubTitle;
    public Toggle PubUni;
    public Toggle PubTopic;
    public Toggle PubYear;
    public InputField SearchInputField;
    public Toggle OptionalToggle;
    public Button AddSearchtermButton;
    public Button ResetButton;
    public Text SearchTermText;
    public Button GoButton;
    public Button ExitGameSetupButton;
    public Button AddUniversityButton;
    public Button AddTopicButton;

    //Simple Game Setup
    public Toggle OptionalToggle_Simple;
    public Button AddSearchtermButton_Simple;
    public Button ResetButton_Simple;
    public Text TopicText_Simple;
    public InputField FreeText_Simple;
    public Button GoButton_Simple;
    public Button ExitGameSetupButton_Simple;
    public Text SearchTermText_Simple;
    public Button AddTopicButton_Simple;

    //Super Simple Game Setup
    public Button GoButton_SuperSimple;
    public Button ExitGameSetupButton_SuperSimple;
    public InputField InputField_SuperSimple;
    public Button ResetButton_SuperSimple;

    //GamePlay
    public Text AbstractText;
    public Button Answer1;
    public Button Answer2;
    public Button Answer3;
    public Button Answer4;
    public Button GameScreenExit;
    public Button NextPuzzleButton;
    public GameObject PanelLargeTextDisplay;
    public GameObject PanelSmallTextDisplay;
    public Scrollbar SmallTextScrollBar;
    public Text AbstractText_Small;
    public Scrollbar LargeTextScrollBar;

    //CollectedPublications
    public Button ExitCollectedPublications;

    //HighScoreScreen
    public Button ExitHighScore;

    //Panels
    public GameObject Panel_MainMenu;
    public GameObject Panel_OptionsMenu;
    public GameObject Panel_GameSetUpMenu;
    public GameObject Panel_GameplayScreen;
    public GameObject Panel_ErrorMessage;
    public GameObject Panel_LoadingScreen;
    public GameObject Panel_CollectedPublications;
    public GameObject Panel_SaveCollectedPublications;
    public GameObject Panel_SimpleGameSetUpMenu;
    public GameObject Panel_HighScoreScreen;
    public GameObject Panel_AddUniversity;
    public GameObject Panel_AddTopic;
    public GameObject Panel_PlayerIntro;
    public GameObject Panel_Congrats;
    public GameObject Panel_SuperSimpleGameSetUpMenu;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
