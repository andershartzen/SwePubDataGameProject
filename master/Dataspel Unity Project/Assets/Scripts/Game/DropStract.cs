using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum DropStractGameState { 
    INITIALIZING, PLAYERCONFIG, LOADING_AUTHORS, LOADING_PUBLICATIONS, GAMEPLAY, PUZZLE_COMPLETED, MENU, OPTIONSMENU, COLLECTEDPUBLICATIONS, HIGHSCORESCREEN, GAME_COMPLETED,
}

public class DropStract : MonoBehaviour
{
    #region Events
    public delegate void ButtonClickedEventHandler(Button clicked);
    public static event ButtonClickedEventHandler ButtonClicked;

    public delegate void GameStateChangedEventHandler(DropStractGameState newState, DropStractGameState oldState);
    public static event GameStateChangedEventHandler GameStateChanged;

    public delegate void GameResetEventHandler();
    public static event GameResetEventHandler GameReset;

    public delegate void SearchStartedEventHandler(Request req);
    public static event SearchStartedEventHandler SearchStarted;

    public delegate void NewPuzzleEventHandler(Puzzle newPuzzle);
    public static event NewPuzzleEventHandler NewPuzzle;

    public delegate void FailureImminentEventHandler();
    public static event FailureImminentEventHandler FailureImminent;

    public delegate void MessageShownToPlayerEventHandler(string message);
    public static event MessageShownToPlayerEventHandler MessageShownToPlayer;

    #endregion

    #region Fields
    public DataSourceManager DataSrcMan;
    public UniversityManager UniMan;

    public Stack<Request> RequestStack = new Stack<Request>();

    public DropStractGameState GameState
    {
        get { return this._currGameState; }
        set {
            DropStractGameState oldState = this._currGameState;
            this._currGameState = value;
            if (GameStateChanged != null)
            {
                GameStateChanged(this._currGameState, oldState);
            }
        }
    } private DropStractGameState _currGameState = DropStractGameState.INITIALIZING;
    
    public DropstractOptions GameOptions;
    public ScoreManager GameScoreManager;
    public int MaxNumberOfAnswers;
    public TextMesh ScoreText;
    public GUIManager GUIMAN;

    private List<AbstractPuzzle> CurrentPuzzles;
    private int ActivePuzzleIndex; 
    private List<SearchTerm> ActiveSearchTerms; 
    private TextDisplayer TextDis = null;
    private List<Publication> pubsFound = null;
    private bool goToGameState = false;
    private bool goToNoPubFoundErrorState = false;
    private bool optionalSearchTerm = false;
    private string searchTermString = string.Empty;
    public int SearchLimit = 200;

    private float PuzzleStartTime = 0.0f;
    private bool AssignPuzzleStartTime = false;

    private bool ImminentEventFired = false;
    #endregion

    #region Event Handling
    void DataRetriever_PublicationSearchCompleted(Request resolvedRequest)
    {
        if (resolvedRequest.Error)
        {
            //this.ShowErrorMessage(resolvedRequest.ErrorMessage);
            return;
        }

        if (resolvedRequest.Cancelled)
        {
            return;
        }
        Debug.Log("publications found: " + resolvedRequest.RequestResult.Count);
        //if (foundPublications.Count < 2)
        //{
        //    this.goToNoPubFoundErrorState = true;
        //}
        //else
        //{
        this.pubsFound = resolvedRequest.RequestResult;
        this.goToGameState = true;
        //}

        //List<Publication> publicationsFound = foundPublications;
        

        //if (publicationsFound.Count < 2)
        //{
        //    //TODO: Ask Player to search for something else.
        //    this.CurrentPuzzles = PuzzleFactory.CreateAbstractPuzzles(foundPublications, 4);

        //    this.BeginGamePlay(publicationsFound);
        //}
        //else
        //{
        //this.CurrentPuzzles = PuzzleFactory.CreateAbstractPuzzles(foundPublications, 4);
        ////this.GameState = DropStractGameState.GAMEPLAY;
        //Debug.Log("Puzzles created: " + this.CurrentPuzzles.Count);
        //this.BeginGamePlay(publicationsFound);
        //Debug.Log("Gone to new state");
        //}
        //this.ArticlesFound = articlesFound;
        //this.PuzzleArticles = this.RandomlySelectPuzzleArticles(articlesFound, this.MinNumberOfArticles);
    }

    void DataRetriever_AuthorSearchCompleted(List<Author> foundAuthors)
    {
        //this.AuthorsFound = foundAuthors;
        //Debug.Log("Authors found: " + this.AuthorsFound.Count);
        //BeginLoadingArticles();
    }
    #endregion

    #region Unity Functions
    // Use this for initialization
	void Start () {
        this.GameState = DropStractGameState.INITIALIZING;

        this.CurrentPuzzles = new List<AbstractPuzzle>();
        this.ActivePuzzleIndex = 0;
        this.MaxNumberOfAnswers = 4;
        this.ActiveSearchTerms = new List<SearchTerm>();
        
        this.DataSrcMan.DataRetriveObj.AuthorSearchCompleted += DataRetriever_AuthorSearchCompleted;
        this.DataSrcMan.DataRetriveObj.PublicationSearchCompleted += DataRetriever_PublicationSearchCompleted;
        ShipManager.ShipCompleted += ShipManager_ShipCompleted;
        //INIT Stuff HERE
        this.GameState = DropStractGameState.MENU;
	}

    void ShipManager_ShipCompleted()
    {
        this.GUIMAN.Panel_GameplayScreen.SetActive(false);
        this.GameScoreManager.Reset();
        this.GameState = DropStractGameState.GAME_COMPLETED;
    }
    
	
	// Update is called once per frame
	void Update () {
        if (this.GameState == DropStractGameState.GAMEPLAY || this.GameState == DropStractGameState.PUZZLE_COMPLETED)
        {
            //this.ScoreText.text = this.GameScoreManager.getScoreString();
            GUIMAN.ScoreText.text = this.GameScoreManager.getScoreString();
        }
        else 
        {
            //this.ScoreText.text = string.Empty;
            GUIMAN.ScoreText.text = string.Empty;
            GUIMAN.TimeLeftText.text = string.Empty;
        }

        if (this.GameState == DropStractGameState.MENU)
        {}
        else if (this.GameState == DropStractGameState.OPTIONSMENU)
        {
            if (!this.GameOptions.MenuOptionsEnabled)
            {
                this.GameState = DropStractGameState.MENU;
                this.GUIMAN.Panel_OptionsMenu.SetActive(false);
                this.GUIMAN.Panel_MainMenu.SetActive(true);
            }
        }
        else if (this.GameState == DropStractGameState.PLAYERCONFIG)
        {
            GUIMAN.SearchTermText.text = this.GetActiveSearchTermsString();
            GUIMAN.SearchTermText_Simple.text = this.GetActiveSearchTermsString();
        }
        else if (this.GameState == DropStractGameState.GAMEPLAY)
        {
            if (this.AssignPuzzleStartTime)
            {
                this.PuzzleStartTime = Time.time;
                this.AssignPuzzleStartTime = false;
            }
            GUIMAN.NextPuzzleButton.gameObject.SetActive(false);

            if (this.GameOptions.StarWarsTextModeEnabled || this.GameOptions.SpreederTextModeEnabled)
            {
                this.TextDis.Update(Time.deltaTime);
            }

            if (this.CurrentPuzzles[this.ActivePuzzleIndex].Solved)
            {
                this.GameState = DropStractGameState.PUZZLE_COMPLETED;
                this.GameScoreManager.CorrectAnswer(this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication);
            }

            if (this.GameOptions.AllTextAtOnceEnabled)
            {
                GUIMAN.AbstractText.text = this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication.Abstract;
            }
            else if (this.GameOptions.StarWarsTextModeEnabled)
            {
                //GUIMAN.AbstractText.text = this.TextDis.getDisplayString();
                System.Text.StringBuilder bob = new System.Text.StringBuilder();
                bob.AppendLine(); bob.AppendLine(); bob.AppendLine(); bob.AppendLine();
                bob.AppendLine(this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication.Abstract);
                GUIMAN.AbstractText_Small.text = bob.ToString();
                float scrollRatio = this.GetStarWarsScrollRatio(Time.time);

                if (scrollRatio > 1.0f)
                {
                    GUIMAN.AbstractText_Small.text = string.Empty;
                }
                else
                {
                    GUIMAN.SmallTextScrollBar.value = scrollRatio;
                }
            }
            else if (this.GameOptions.SpreederTextModeEnabled)
            {
                GUIMAN.AbstractText.text = this.TextDis.getDisplayString();
            }

            if (this.GameOptions.TimeScoreMode || this.GameOptions.TimeAndMultiplierScoreMode)
            {
                GUIMAN.TimeLeftText.text = this.GameScoreManager.getTimeLeftString();
                float ratioLeft = this.GameScoreManager.CurrentTimeScore / ScoreManager.MaxTimeScore;

                if (ratioLeft <= 0.25f)
                { 
                    if(FailureImminent != null && !this.ImminentEventFired)
                    {
                        FailureImminent();
                        this.ImminentEventFired = true;
                    }
                }
                
                if (this.GameScoreManager.CurrentTimeScore <= 0)
                {
                    //Fail puzzle, ship level reset
                    this.CurrentPuzzles[this.ActivePuzzleIndex].Failed = true;
                    GUIMAN.AbstractText_Small.text = string.Empty;
                    this.GameState = DropStractGameState.PUZZLE_COMPLETED;
                }
            }

            if (this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers().Count >= 1)
            {
                string title = this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers()[0];
                GUIMAN.Answer1.GetComponentInChildren<Text>().text = title;
                GUIMAN.Answer1.interactable = !this.CurrentPuzzles[this.ActivePuzzleIndex].AttemptedTitle(0);
            }

            if (this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers().Count >= 2)
            {
                string title = this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers()[1];
                GUIMAN.Answer2.GetComponentInChildren<Text>().text = title;
                GUIMAN.Answer2.interactable = !this.CurrentPuzzles[this.ActivePuzzleIndex].AttemptedTitle(1);
            }

            if (this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers().Count >= 3)
            {
                string title = this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers()[2];
                GUIMAN.Answer3.GetComponentInChildren<Text>().text = title;
                GUIMAN.Answer3.interactable = !this.CurrentPuzzles[this.ActivePuzzleIndex].AttemptedTitle(2);
            }

            if (this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers().Count >= 4)
            {
                string title = this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers()[3];
                GUIMAN.Answer4.GetComponentInChildren<Text>().text = title;
                GUIMAN.Answer4.interactable = !this.CurrentPuzzles[this.ActivePuzzleIndex].AttemptedTitle(3);
            }


        }
        else if (this.GameState == DropStractGameState.LOADING_AUTHORS)
        { 
            
        }
        else if (this.GameState == DropStractGameState.LOADING_PUBLICATIONS)
        {
            if (this.RequestStack.Peek().Error)
            {
                this.ShowErrorMessage(this.RequestStack.Peek().ErrorMessage);
                this.CancelSearch();
            }

            if (this.goToGameState)
            {
                GUIMAN.Panel_LoadingScreen.SetActive(false);
                this.BeginGamePlay();
            }
            else
            {
                GUIMAN.Panel_LoadingScreen.SetActive(true);
            }
        }
        else if (this.GameState == DropStractGameState.PUZZLE_COMPLETED)
        {
            if (this.CanWeGoToNextPuzzle())
            {
                GUIMAN.NextPuzzleButton.gameObject.SetActive(true);
                GUIMAN.NextPuzzleButton.interactable = true;
            }
            else
            {
                GUIMAN.NextPuzzleButton.gameObject.SetActive(false);
                //ToDO: Display message telling user that there are no more puzzles

                this.ShowErrorMessage("You ran out of abstract fuel");
                if (GameOptions.advancedSearch)
                {
                    this.GUIMAN.Panel_GameSetUpMenu.SetActive(true);
                }
                else if (!GameOptions.SuperSimpleSearchMode)
                {
                    this.GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(true);
                }
                else
                {
                    this.GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(true);
                }
                this.GUIMAN.Panel_GameplayScreen.SetActive(false);
                this.GameScoreManager.Reset();
                this.GoToGameSetup();                
            }
        }
	
	}
    #endregion 
    
    private float GetStarWarsScrollRatio(float timeSinceStart)
    {
        float timeSincePuzzleStart = timeSinceStart - this.PuzzleStartTime;
        float scrollRatio = (timeSincePuzzleStart / ScoreManager.MaxTimeScore);

        if (scrollRatio >= 1.0f)
        {
            return scrollRatio;
        }
        else
        {
            return 1.0f - scrollRatio;
        }
    }

    private void ShowErrorMessage(string errMessage)
    {
        ErrorMessage.CurrentErrorMessage = errMessage;
        GUIMAN.Panel_ErrorMessage.SetActive(true);

        if (MessageShownToPlayer != null)
        {
            MessageShownToPlayer(errMessage);
        }
    }
    
    /// <summary>
    /// Setup of PlayerConfig state
    /// </summary>
    private void GoToGameSetup()
    {
        this.GameState = DropStractGameState.PLAYERCONFIG;
        this.ActiveSearchTerms.Clear();

        this.ActivePuzzleIndex = 0;
        
        this.searchTermString = string.Empty;
    }

    /// <summary>
    /// Begins loading articles from the data source based on the player query
    /// </summary>
    /// <param name="searchTerms"></param>
    /// <param name="Limit"></param>
    private void BeginLoadingPublications(Request req, int Limit)
    {
        this.GameState = DropStractGameState.LOADING_PUBLICATIONS;
        this.RequestStack.Push(req);

        if (SearchStarted != null)
        { SearchStarted(req); }

        this.DataSrcMan.DataRetriveObj.FindPublication(req, Limit);
    }

    private void BeginGamePlay()
    {
        this.CurrentPuzzles = PuzzleFactory.CreateAbstractPuzzles(this.pubsFound, this.MaxNumberOfAnswers);
        this.goToGameState = false;
        if (this.CurrentPuzzles.Count > 0)
        {
            this.TextDis = new TextDisplayer(this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication.Abstract, this.GameOptions.TextWordsPrSec, this.GameOptions.SpreederTextModeEnabled);
            this.GameState = DropStractGameState.GAMEPLAY;
            if (GameOptions.advancedSearch)
            {
                this.GUIMAN.Panel_GameSetUpMenu.SetActive(false);
            }
            else if (!GameOptions.SuperSimpleSearchMode)
            {
                this.GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(false);
            }
            else
            {
                this.GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(false);
            }
            GUIMAN.Panel_GameplayScreen.SetActive(true);
            this.GameScoreManager.NewPuzzle(this.ComputeTimeToRead(this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication.Abstract));
            FireNewPuzzleEvent(this.CurrentPuzzles[this.ActivePuzzleIndex]);
            if (this.GameOptions.StarWarsTextModeEnabled)
            {
                this.GUIMAN.PanelLargeTextDisplay.SetActive(false);
                this.GUIMAN.PanelSmallTextDisplay.SetActive(true);
            }
            else
            {
                this.GUIMAN.PanelLargeTextDisplay.SetActive(true);
                this.GUIMAN.PanelSmallTextDisplay.SetActive(false);
            }
            this.AssignPuzzleStartTime = true;
        }
        else
        {
            //this.GameState = DropStractGameState.PLAYERCONFIG;
            this.GoToGameSetup();
            this.ShowErrorMessage("No Publications Found");
        }
    }

    private float ComputeTimeToRead(string pubAbstract)
    {
        string[] split = pubAbstract.Split(' ');
        float readingSeconds = split.Length / this.GameOptions.TextWordsPrSec;
        float readingRatio = this.GameOptions.TextWordsPrSec / split.Length;
        // (1-(10*readingRatio)*readingSeconds)
        //float readingTime = ((1.0f + (100.0f * readingRatio)) * readingSeconds);
        float readingTime = 0.0f;

        if(readingSeconds <= 30.0f && readingSeconds >= 20.0f)
        {
            readingTime = readingSeconds;        
        }
        else if (readingSeconds < 20.0f)
        {
            readingTime = 20.0f;
        }
        else if (readingSeconds > 30.0f)
        {
            readingTime = 30.0f;
        }
        //split.Length / this.GameOptions.TextWordsPrSec;
        Debug.Log("readingRatio: " + readingRatio + " readingSeconds: " + readingSeconds + " readingTime: " + readingTime + " abstract length: " + split.Length);
        return readingTime;
    }

    private bool CanWeGoToNextPuzzle()
    {
        if (this.ActivePuzzleIndex + 1 < this.CurrentPuzzles.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Goes to the next generated puzzle (if present).
    /// </summary>
    /// <returns></returns>
    private bool GoToNextPuzzle()
    {
        if (this.ActivePuzzleIndex + 1 < this.CurrentPuzzles.Count)
        {
            this.ActivePuzzleIndex++;
            this.GameState = DropStractGameState.GAMEPLAY;
            this.TextDis = new TextDisplayer(this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication.Abstract, this.GameOptions.TextWordsPrSec, this.GameOptions.SpreederTextModeEnabled);
            this.GameScoreManager.NewPuzzle(this.ComputeTimeToRead(this.CurrentPuzzles[this.ActivePuzzleIndex].SolutionPublication.Abstract));
            FireNewPuzzleEvent(this.CurrentPuzzles[this.ActivePuzzleIndex]);
            this.AssignPuzzleStartTime = true;
            GUIMAN.LargeTextScrollBar.value = 1.0f;
            this.ImminentEventFired = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FireNewPuzzleEvent(Puzzle p)
    {
        if (NewPuzzle != null)
        {
            NewPuzzle(p);
        }
    }

    /// <summary>
    /// Used to add search term to current player query
    /// </summary>
    /// <param name="selectedSearchTermIndex"></param>
    /// <param name="searchString"></param>
    private void AddSearchTerm(int selectedSearchTermIndex, string searchString)
    {
        //this.SearchTermGUIContentList[0] = new GUIContent("Creator Name");
        //this.SearchTermGUIContentList[1] = new GUIContent("Publication Title");
        //this.SearchTermGUIContentList[2] = new GUIContent("Publication Topic");
        //this.SearchTermGUIContentList[3] = new GUIContent("Publication University");
        //this.SearchTermGUIContentList[4] = new GUIContent("Publication Year");
        if (selectedSearchTermIndex == 0)
        {
            string[] splite = searchString.Split(' ');
            string firstName = string.Empty;
            
            for (int i = 0; i < splite.Length-1; i++)
            {
                firstName += " "+splite[i];
            }
            string lastName = splite[splite.Length - 1];
            this.ActiveSearchTerms.Add(new SearchTerm_CreatorName(firstName, lastName, this.optionalSearchTerm));
        }
        else if (selectedSearchTermIndex == 1)
        {
            this.ActiveSearchTerms.Add(new SearchTerm_PublicationTitle(searchString, this.optionalSearchTerm));
        }
        else if (selectedSearchTermIndex == 2)
        {
            this.ActiveSearchTerms.Add(new SearchTerm_PublicationTopic(searchString, this.optionalSearchTerm));
        }
        else if (selectedSearchTermIndex == 3)
        {
           this.ActiveSearchTerms.Add(new SearchTerm_PublicationUniversity(this.UniMan.FindUniversityBasedOnName(searchString), this.optionalSearchTerm));
        }
        else if (selectedSearchTermIndex == 4)
        {
            try
            {
                int year = int.Parse(searchString);
                this.ActiveSearchTerms.Add(new SearchTerm_PublicationYear(year.ToString(), this.optionalSearchTerm));
            }
            catch (System.Exception e)
            { }
        }
        else if (selectedSearchTermIndex == 5)
        {
            this.ActiveSearchTerms.Add(new SearchTerm_FreeText(searchString, this.optionalSearchTerm));
        }
    }

    private string GetActiveSearchTermsString()
    {
        if (this.ActiveSearchTerms.Count == 0)
        {
            return "Active Search Terms Appear Here";
        }
        else
        {
            string searchterms = string.Empty;
            foreach (SearchTerm term in this.ActiveSearchTerms)
            {
                searchterms += term.ToString() + System.Environment.NewLine;
            }
            return searchterms;
        }
    }

    #region GUI

    /// <summary>
    /// Used to handle On Screen button presses based on the current game state.
    /// </summary>
    /// <param name="button">Button pressed by the player</param>
    public void ButtonPressed(Button button)
    {
        if (ButtonClicked != null)
        {
            ButtonClicked(button);
        }

        if (this.GameState == DropStractGameState.MENU)
        {
            this.ButtonPress_MenuState(button);
        }        
        else if (this.GameState == DropStractGameState.PLAYERCONFIG)
        {
            this.ButtonPress_PlayerConfigState(button);
        }
        else if(this.GameState == DropStractGameState.GAMEPLAY)
        {
            this.ButtonPress_GameplayState(button);
        }
        else if (this.GameState == DropStractGameState.PUZZLE_COMPLETED)
        {
            this.ButtonPress_PuzzleSolvedState(button);
        }
        else if (this.GameState == DropStractGameState.COLLECTEDPUBLICATIONS)
        {
            this.ButtonPress_CollectedPublications(button);
        }
        else if (this.GameState == DropStractGameState.HIGHSCORESCREEN)
        {
            this.ButtonPress_HighScoreScreen(button);
        }
        else if (this.GameState == DropStractGameState.GAME_COMPLETED)
        {
            this.ButtonPress_CongratsScreen(button);
        }

    }

    /// <summary>
    /// Used to handle on screen button press in the Menu game state.
    /// </summary>
    /// <param name="button">Button pressed by the player</param>
    private void ButtonPress_MenuState(Button button)
    {
        if (button == GUIMAN.CreatePuzzleButton)
        {
            this.GameState = DropStractGameState.PLAYERCONFIG;
            this.GUIMAN.Panel_MainMenu.SetActive(false);
            if (GameOptions.advancedSearch)
            {
                this.GUIMAN.Panel_GameSetUpMenu.SetActive(true);
            }
            else if (!GameOptions.SuperSimpleSearchMode)
            {
                this.GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(true);
            }
            else
            {
                this.GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(true);
            }
            
        }

        if (button == GUIMAN.OptionsButton)
        {
            this.GameOptions.MenuOptionsEnabled = true;
            this.GUIMAN.Panel_MainMenu.SetActive(false);
            this.GUIMAN.Panel_OptionsMenu.SetActive(true);
            this.GameState = DropStractGameState.OPTIONSMENU;
            this.GameOptions.MenuOpened();
        }

        if (button == GUIMAN.ExitButton)
        {
            Application.Quit();
        }

        if (button == GUIMAN.CollectedPublications)
        {
            this.GUIMAN.Panel_MainMenu.SetActive(false);
            this.GUIMAN.Panel_CollectedPublications.SetActive(true);
            this.GameState = DropStractGameState.COLLECTEDPUBLICATIONS;
        }

        if (button == GUIMAN.HighScoreButton)
        {
            this.GUIMAN.Panel_MainMenu.SetActive(false);
            this.GUIMAN.Panel_HighScoreScreen.SetActive(true);
            this.GameState = DropStractGameState.HIGHSCORESCREEN;
        }

        if(button == GUIMAN.AboutButton)
        {
            this.GUIMAN.Panel_MainMenu.SetActive(false);
            this.GUIMAN.Panel_PlayerIntro.SetActive(true);
        }
    }

    /// <summary>
    /// Used to handle on screen button press in the PlayerConfig game state
    /// </summary>
    /// <param name="button">Button pressed by the player</param>
    private void ButtonPress_PlayerConfigState(Button button)
    {
        if (this.GameOptions.advancedSearch)
        {
            searchTermString = this.GUIMAN.SearchInputField.text;

            this.optionalSearchTerm = this.GUIMAN.OptionalToggle.isOn;

            if (button == GUIMAN.AddSearchtermButton)
            {
                if (GUIMAN.CreatorName.isOn)
                {
                    this.AddSearchTerm(0, searchTermString);
                }
                else if (GUIMAN.PubTitle.isOn)
                {
                    this.AddSearchTerm(1, searchTermString);
                }
                else if (GUIMAN.PubYear.isOn)
                {
                    this.AddSearchTerm(4, searchTermString);
                }
                //else if (GUIMAN.PubTopic.isOn)
                //{
                //    this.AddSearchTerm(2, searchTermString);
                //}
                //else if (GUIMAN.PubUni.isOn)
                //{
                //    this.AddSearchTerm(3, searchTermString);
                //}
            }

            if (button == GUIMAN.GoButton)
            {
                this.BeginLoadingPublications(new Request(this.ActiveSearchTerms), this.SearchLimit);
            }

            //GUIMAN.SearchTermText.text = this.GetActiveSearchTermsString();

            if (button == GUIMAN.ExitGameSetupButton)
            {
                this.GameState = DropStractGameState.MENU;
                GUIMAN.Panel_GameSetUpMenu.SetActive(false);
                GUIMAN.Panel_MainMenu.SetActive(true);
            }

            if (button == GUIMAN.ResetButton)
            {
                this.GoToGameSetup();
                //this.ScoreText.text = string.Empty;
                GUIMAN.ScoreText.text = string.Empty;
            }
            if (button == GUIMAN.AddUniversityButton)
            {
                GUIMAN.Panel_AddUniversity.SetActive(true);
            }
            if (button == GUIMAN.AddTopicButton)
            {
                GUIMAN.Panel_AddTopic.SetActive(true);
            }
        }
        else if (!GameOptions.SuperSimpleSearchMode)
        {
            //string topicString = this.GUIMAN.TopicText_Simple.text;
            string freeString = this.GUIMAN.FreeText_Simple.text;
            searchTermString = this.GUIMAN.SearchInputField.text;

            this.optionalSearchTerm = this.GUIMAN.OptionalToggle_Simple.isOn;

            if (button == GUIMAN.AddSearchtermButton_Simple)
            {
                if (freeString != string.Empty)
                {
                    this.AddSearchTerm(5, freeString);
                }
            }

            if (button == GUIMAN.GoButton_Simple)
            {
                this.BeginLoadingPublications(new Request(this.ActiveSearchTerms), this.SearchLimit);
            }

            GUIMAN.SearchTermText_Simple.text = this.GetActiveSearchTermsString();

            if (button == GUIMAN.ExitGameSetupButton_Simple)
            {
                this.GameState = DropStractGameState.MENU;
                GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(false);
                GUIMAN.Panel_MainMenu.SetActive(true);
            }

            if (button == GUIMAN.ResetButton_Simple)
            {
                this.GoToGameSetup();
                //this.ScoreText.text = string.Empty;
                GUIMAN.ScoreText.text = string.Empty;
            }
            if (button == GUIMAN.AddTopicButton_Simple)
            {
                GUIMAN.Panel_AddTopic.SetActive(true);
            }
        }
        else
        {
            if (button == GUIMAN.ResetButton_SuperSimple)
            {
                GUIMAN.InputField_SuperSimple.text = string.Empty;
            }
            else if (button == GUIMAN.ExitGameSetupButton_SuperSimple)
            {
                this.GameState = DropStractGameState.MENU;
                GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(false);
                GUIMAN.Panel_MainMenu.SetActive(true);
            }
            else if (button == GUIMAN.GoButton_SuperSimple)
            {
                if (GUIMAN.InputField_SuperSimple.text != string.Empty)
                {
                    this.AddSearchTerm(5, GUIMAN.InputField_SuperSimple.text);
                }

                this.BeginLoadingPublications(new Request(this.ActiveSearchTerms), this.SearchLimit);
            }
        }
    }

    /// <summary>
    /// Used to handle on screen button press in the Gameplay game state
    /// </summary>
    /// <param name="button"></param>
    private void ButtonPress_GameplayState(Button button)
    {
        
        if(button == GUIMAN.Answer1)
        {
            //this.CurrentPuzzles[this.ActivePuzzleIndex].GetPossibleAnswers()[0])
            if (!this.CurrentPuzzles[this.ActivePuzzleIndex].CheckAnswer(GUIMAN.Answer1.GetComponentInChildren<Text>().text))
            {
                GameScoreManager.WrongAnswer();
                
            }
        }
        else if (button == GUIMAN.Answer2)
        {
            if (!this.CurrentPuzzles[this.ActivePuzzleIndex].CheckAnswer(GUIMAN.Answer2.GetComponentInChildren<Text>().text))
            {
                GameScoreManager.WrongAnswer();
                
            }
        }
        else if (button == GUIMAN.Answer3)
        {
            if (!this.CurrentPuzzles[this.ActivePuzzleIndex].CheckAnswer(GUIMAN.Answer3.GetComponentInChildren<Text>().text))
            {
                GameScoreManager.WrongAnswer();
                
            }
        }
        else if (button == GUIMAN.Answer4)
        {
            if (!this.CurrentPuzzles[this.ActivePuzzleIndex].CheckAnswer(GUIMAN.Answer4.GetComponentInChildren<Text>().text))
            {
                GameScoreManager.WrongAnswer();
                
            }
        }
        else if (button == GUIMAN.GameScreenExit)
        {
            if (GameOptions.advancedSearch)
            {
                this.GUIMAN.Panel_GameSetUpMenu.SetActive(true);
            }
            else if (!GameOptions.SuperSimpleSearchMode)
            {
                this.GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(true);
            }
            else
            {
                this.GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(true);
            }
            this.GUIMAN.Panel_GameplayScreen.SetActive(false);
            this.GameScoreManager.Reset();

            if (GameReset != null)
            { GameReset(); }

            //GUIMAN.Answer3.gameObject.SetActive(false);
            //GUIMAN.Answer4.gameObject.SetActive(false);
            this.GoToGameSetup();
        }

        
    }

    /// <summary>
    /// Used to handle on screen button press in the Solved Puzzle game state
    /// </summary>
    /// <param name="button"></param>
    private void ButtonPress_PuzzleSolvedState(Button button)
    {
        if (button == GUIMAN.NextPuzzleButton)
        {
            if (this.CanWeGoToNextPuzzle())
            {
                this.GoToNextPuzzle();
            }
            else
            {
                this.ShowErrorMessage("You ran out of abstract fuel");
                if (GameOptions.advancedSearch)
                {
                    this.GUIMAN.Panel_GameSetUpMenu.SetActive(true);
                }
                else if (GameOptions.SuperSimpleSearchMode)
                {
                    this.GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(true);
                }
                else
                {
                    this.GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(true);
                }
                this.GUIMAN.Panel_GameplayScreen.SetActive(false);
                this.GameScoreManager.Reset();
                this.GoToGameSetup();
            }
        }

        if (button == GUIMAN.GameScreenExit)
        {
            if (GameOptions.advancedSearch)
            {
                this.GUIMAN.Panel_GameSetUpMenu.SetActive(true);
            }
            else if (GameOptions.SuperSimpleSearchMode)
            {
                this.GUIMAN.Panel_SuperSimpleGameSetUpMenu.SetActive(true);
            }
            else
            {
                this.GUIMAN.Panel_SimpleGameSetUpMenu.SetActive(true);
            }
            this.GUIMAN.Panel_GameplayScreen.SetActive(false);
            this.GameScoreManager.Reset();
            this.GoToGameSetup();
        }
    }

    /// <summary>
    /// Used to handle on screen button press in the Solved Puzzle game state
    /// </summary>
    /// <param name="button"></param>
    private void ButtonPress_CollectedPublications(Button button)
    {
        if (button == GUIMAN.ExitCollectedPublications)
        {
            this.GameState = DropStractGameState.MENU;
            GUIMAN.Panel_CollectedPublications.SetActive(false);
            GUIMAN.Panel_MainMenu.SetActive(true);
        }
    }
    
    private void ButtonPress_HighScoreScreen(Button button)
    {
        if (button == GUIMAN.ExitHighScore)
        {
            this.GameState = DropStractGameState.MENU;
            GUIMAN.Panel_HighScoreScreen.SetActive(false);
            GUIMAN.Panel_MainMenu.SetActive(true);
        }
    }

    public void AddUniversityScreenClosed()
    {
        if (UniSelectScreen.SelectedUniversity != null)
        {
            this.AddSearchTerm(3, UniSelectScreen.SelectedUniversity.UniversityCode);
            UniSelectScreen.SelectedUniversity = null;
        }
    }

    public void AddTopicScreenClosed()
    {
        if (TopicSelector.SelectedTopic != string.Empty)
        {
            this.AddSearchTerm(2, TopicSelector.SelectedTopic);
            TopicSelector.SelectedTopic = string.Empty;
        }
    }

    public void CancelSearch()
    {
        if (this.RequestStack.Count > 0)
        {
            this.RequestStack.Peek().Cancelled = true;
            GUIMAN.Panel_LoadingScreen.SetActive(false);
            this.GoToGameSetup();
        }
    }

    public void ButtonPress_CongratsScreen(Button button)
    { 
        //Return to Main Menu
        GUIMAN.Panel_GameplayScreen.SetActive(false);
        GUIMAN.Panel_Congrats.SetActive(false);
        this.GameState = DropStractGameState.MENU;
        GUIMAN.Panel_MainMenu.SetActive(true);
    }

   
    #endregion
}
