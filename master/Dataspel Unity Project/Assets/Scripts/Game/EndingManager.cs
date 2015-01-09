using UnityEngine;
using System.Collections;

public class EndingManager : MonoBehaviour {
    public Camera MainCamera;
    public GameObject EndSceneObject;
    public GameObject Congrats_Panel;

    private bool showEndScene = false;
    private float endSceneAnimLength = 6.083f;
    private float endSceneAnimTime = 0.0f;


	// Use this for initialization
	void Start () {
        DropStract.GameStateChanged += DropStract_GameStateChanged;
	}

    void DropStract_GameStateChanged(DropStractGameState newState, DropStractGameState oldState)
    {
        if (newState == DropStractGameState.GAME_COMPLETED)
        {
            this.MainCamera.gameObject.SetActive(false);
            this.EndSceneObject.SetActive(true);
            this.showEndScene = true;
        }
        else
        {
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (this.showEndScene)
        {
            if (this.endSceneAnimTime >= this.endSceneAnimLength)
            {
                this.showEndScene = false;
                this.Congrats_Panel.SetActive(true);
                this.MainCamera.gameObject.SetActive(true);
                this.EndSceneObject.SetActive(false);
            }
            else
            {
                this.endSceneAnimTime += Time.deltaTime;
            }
        }
	}

}
