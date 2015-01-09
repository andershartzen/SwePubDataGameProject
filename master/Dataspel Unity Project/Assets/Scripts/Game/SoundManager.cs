using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public AudioSource BackgroundMusicAudioSource;
    public AudioSource SoundEffectAudioSource;
    public AudioSource SoundEffectAudioSource2;
    public AudioClip ClickSound; 
    public AudioClip PuzzleMusic1;
    public AudioClip SuccessSound; 
    public AudioClip WrongSound; 
    public AudioClip LoadingSound; 
    public AudioClip AlarmSound; 
    public AudioClip FailedSound;
    public AudioClip EndingSound;


	// Use this for initialization
	void Start () {
        DropStract.ButtonClicked += DropStract_ButtonClicked;
        DropStract.GameStateChanged += DropStract_GameStateChanged;
        Puzzle.SolutionSubmitted += AbstractPuzzle_AnswerSubmitted;
        DropStract.FailureImminent += DropStract_FailureImminent;
        Puzzle.PuzzleFailed += Puzzle_PuzzleFailed;
        ShipManager.ShipCompleted += ShipManager_ShipCompleted;
        DropStract.NewPuzzle += DropStract_NewPuzzle;
	}

    void DropStract_NewPuzzle(Puzzle newPuzzle)
    {
        this.SoundEffectAudioSource2.Stop();
    }

    void ShipManager_ShipCompleted()
    {
        this.BackgroundMusicAudioSource.Stop();
        this.BackgroundMusicAudioSource.clip = EndingSound;
        this.BackgroundMusicAudioSource.Play();
    }

    void Puzzle_PuzzleFailed(Puzzle p)
    {
        this.SoundEffectAudioSource2.Stop();
        this.SoundEffectAudioSource2.loop = false;
        this.SoundEffectAudioSource2.clip = FailedSound;
        this.SoundEffectAudioSource2.Play();
    }

    void DropStract_FailureImminent()
    {
        this.SoundEffectAudioSource2.Stop();
        this.SoundEffectAudioSource2.clip = this.AlarmSound;
        this.SoundEffectAudioSource2.loop = true;
        this.SoundEffectAudioSource2.Play();
    }

    void AbstractPuzzle_AnswerSubmitted(bool correctAnswer, string answer)
    {
        if (correctAnswer)
        {
            this.SoundEffectAudioSource2.Stop();
            this.SoundEffectAudioSource.Stop();
            this.SoundEffectAudioSource.clip = this.SuccessSound;
            this.SoundEffectAudioSource.Play();
        }
        else
        {
            this.SoundEffectAudioSource.Stop();
            this.SoundEffectAudioSource.clip = this.WrongSound;
            this.SoundEffectAudioSource.Play();
        }
    }

    void DropStract_GameStateChanged(DropStractGameState newState, DropStractGameState oldState)
    {
        
        if (newState == DropStractGameState.LOADING_PUBLICATIONS)
        {
            this.BackgroundMusicAudioSource.clip = LoadingSound;
            this.BackgroundMusicAudioSource.loop = true;
            this.BackgroundMusicAudioSource.Play();
        }
        else if (newState == DropStractGameState.GAMEPLAY)
        {
            this.BackgroundMusicAudioSource.Stop();
            this.BackgroundMusicAudioSource.clip = PuzzleMusic1;
            this.BackgroundMusicAudioSource.loop = true;
            this.BackgroundMusicAudioSource.Play();
        }
        else if (newState == DropStractGameState.PLAYERCONFIG)
        {
            this.BackgroundMusicAudioSource.Stop();
        }
    }

    void DropStract_ButtonClicked(UnityEngine.UI.Button clicked)
    {
        this.SoundEffectAudioSource.clip = ClickSound;
        this.SoundEffectAudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
