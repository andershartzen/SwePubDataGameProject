using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipManager : MonoBehaviour {
    public GameObject PlayerShip;
    public List<GameObject> ShipParts = new List<GameObject>();
    public GameObject ShipEngineLvl01ParticleEffect;
    public GameObject ShipEngineLvl07ParticleEffect;
    public GameObject StarParticles;
    public GameObject UpgradeEffect;
    public GameObject PointScoreEffect;
    public GameObject ErrorEffect;
    public GameObject DestructionEffect;
    public GameObject TimeLeftSign;
    public ScoreManager ScoreMan;
    public GameObject UpgradeOrbOrig;
    private GameObject UpgradeOrb;
    public float UpgradeOrbYStart = 0.0f;
    public GameObject TimeArrows;

    public GameObject ImpactEminentSign;
    public GameObject TimeWarningSign;

    private int CurrentShipLevel = 1;
    private Animator ShipAnimator;
    private Animator TimeLeftAnimator;
    private Animator TimeArrowsAnimator;

    private bool playUpgradeAnim = false;
    private float upgradeAnimTimeLength = 1.0f;
    private float upgradeAnimTime = 0.0f;

    private bool playGoingAnim = false;
    private float goingAnimLength = 3.9f;
    private float goingAnimTime = 0.0f;

    private bool playOrbAnim = false;

    private bool playErrorAnim = false;
    private float errorAnimTimeLength = 1.0f;
    private float errorAnimTime = 0.0f;

    private bool playDestructionAnim = false;
    private float destructionAnimTimeLength = 1.0f;
    private float destructionAnimTime = 0.0f;

    public delegate void ShipCompletedEventHandler();
    public static event ShipCompletedEventHandler ShipCompleted;

    //private bool playGoingAnim = false;
    //private float goingAnimTimeLength = 3.9f;
    //private float goingAnimTime = 0.0f;

	// Use this for initialization
	void Start () {
        Puzzle.SolutionSubmitted += Puzzle_SolutionSubmitted;
        DropStract.GameStateChanged += DropStract_GameStateChanged;
        Puzzle.PuzzleFailed += Puzzle_PuzzleFailed;
        ShipAnimator = PlayerShip.GetComponent<Animator>();
        TimeLeftAnimator = TimeLeftSign.GetComponent<Animator>();
        TimeArrowsAnimator = TimeArrows.GetComponent<Animator>();
        DropStract.NewPuzzle += DropStract_NewPuzzle;
        DropStract.FailureImminent += DropStract_FailureImminent;
	}

    void DropStract_FailureImminent()
    {
        this.TimeArrows.SetActive(true);
        float timeAnimSpeed = 3.0f / (this.ScoreMan.CurrentTimeScore);
        this.TimeArrowsAnimator.speed = timeAnimSpeed;
        this.TimeWarningSign.SetActive(true);
        this.ImpactEminentSign.SetActive(true);
    }

    void DropStract_NewPuzzle(Puzzle newPuzzle)
    {
        this.TimeArrows.SetActive(false);
        this.TimeWarningSign.SetActive(false);
        this.ImpactEminentSign.SetActive(false);
        //this.TimeLeftSign.SetActive(false);
        //this.TimeLeftSign.SetActive(true);
        //float timeAnimSpeed = 2.350f / this.ScoreMan.CurrentTimeScore;
        //this.TimeLeftAnimator.speed = timeAnimSpeed;
    }

    void Puzzle_PuzzleFailed(Puzzle p)
    {
        this.TimeArrows.SetActive(false);
        this.TimeLeftSign.SetActive(false);
        this.playDestructionAnim = true;
        this.DestructionEffect.SetActive(true);
        this.TimeWarningSign.SetActive(false);
        this.ImpactEminentSign.SetActive(false);
        this.ResetShipLevel();
    }

    void DropStract_GameStateChanged(DropStractGameState newState, DropStractGameState oldState)
    {
        if (newState == DropStractGameState.GAMEPLAY || newState == DropStractGameState.PUZZLE_COMPLETED)
        {
            this.ActiveSetter(true);
        }
        else
        {
            this.ResetShipLevel();
            this.ActiveSetter(false);
        }
    }

    void Puzzle_SolutionSubmitted(bool correctSolution, string Answer)
    {
        if (correctSolution)
        {
            this.TimeLeftSign.SetActive(false);
            this.TimeArrows.SetActive(false);
            this.TimeWarningSign.SetActive(false);
            this.ImpactEminentSign.SetActive(false);
            //this.playGoingAnim = true;
            //ShipAnimator.SetBool("going", true);
            //this.IncreaseShipLevel();
            this.playOrbAnim = true;            
            this.UpgradeOrb = Instantiate(this.UpgradeOrbOrig,this.UpgradeOrbOrig.transform.position,this.UpgradeOrbOrig.transform.rotation) as GameObject;
            this.UpgradeOrb.SetActive(true);
            //this.UpgradeEffect.SetActive(true);
        }
        else
        {
            this.ErrorEffect.SetActive(true);
            this.playErrorAnim = true;
            this.DecreaseShipLevel();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //this.CurrentShipLevel = (int) this.ScoreMan.CurrentMultiplier;

        if (this.playUpgradeAnim)
        {
            if (this.upgradeAnimTime < this.upgradeAnimTimeLength)
            {
                this.upgradeAnimTime += Time.deltaTime;
            }
            else
            {
                this.playUpgradeAnim = false;
                this.upgradeAnimTime = 0.0f;
                this.UpgradeEffect.SetActive(false);
            }
        }
        else if (this.playOrbAnim)
        {
            if (this.UpgradeOrb.transform.localPosition.y > 1.5f)
            {
                this.UpgradeOrb.transform.Translate(0, this.UpgradeOrb.transform.position.y * -0.75f*Time.deltaTime, 0, Space.World);
            }
            else
            {
                this.UpgradeEffect.SetActive(true);
                this.playUpgradeAnim = true;
                this.IncreaseShipLevel();
                //this.UpgradeOrb.transform.Translate(new Vector3(this.UpgradeOrb.transform.localPosition.x,this.UpgradeOrbYStart, this.UpgradeOrb.transform.localPosition.z));
                //this.UpgradeOrb.transform.position.Set(this.UpgradeOrb.transform.position.x, this.UpgradeOrbYStart, this.UpgradeOrb.transform.position.z);
                this.UpgradeOrb.SetActive(false);
                Destroy(this.UpgradeOrb);
                this.playOrbAnim = false;
            }
        }
        else if (this.playGoingAnim)
        {
            if (this.goingAnimTime >= this.goingAnimLength)
            {
                this.playGoingAnim = false;
                this.goingAnimTime = 0.0f;
                //Game Won
                if (ShipCompleted != null)
                {
                    ShipCompleted();
                }
            }
            else
            {
                this.goingAnimTime += Time.deltaTime;
            }
        }
        else if (this.playErrorAnim)
        {
            if (this.errorAnimTime < this.errorAnimTimeLength)
            {
                this.errorAnimTime += Time.deltaTime;
            }
            else
            {
                this.playErrorAnim = false;
                this.errorAnimTime = 0.0f;
                this.ErrorEffect.SetActive(false);
            }
        }
        else if (this.playDestructionAnim)
        {
            if (this.destructionAnimTime < this.destructionAnimTimeLength)
            {
                this.destructionAnimTime += Time.deltaTime;
            }
            else
            {
                this.playDestructionAnim = false;
                this.destructionAnimTime = 0.0f;
                this.DestructionEffect.SetActive(false);
            }
        }

	}

    private void IncreaseShipLevel()
    {
        if (CurrentShipLevel < this.ShipParts.Count)
        {
            CurrentShipLevel++;
            this.UpdateShipParts();

            if (CurrentShipLevel >= 7)
            {
                ShipEngineLvl07ParticleEffect.SetActive(true);
            }
            else
            {
                ShipEngineLvl07ParticleEffect.SetActive(false);
            }
        }
        else
        { 
            //Play
            this.playGoingAnim = true;
            this.ShipAnimator.Play("GoingAnimation");
        }
    }

    private void DecreaseShipLevel()
    {
        if (CurrentShipLevel > 1)
        {
            CurrentShipLevel--;
            this.UpdateShipParts();

            if (CurrentShipLevel >= 7)
            {
                ShipEngineLvl07ParticleEffect.SetActive(true);
            }
            else
            {
                ShipEngineLvl07ParticleEffect.SetActive(false);
            }
        }
    }


    private void ResetShipLevel()
    {
        this.CurrentShipLevel = 1;
        this.UpdateShipParts();
        ShipEngineLvl07ParticleEffect.SetActive(false);
        this.ShipAnimator.Play("IdleAnimation");
    }

    private void ActiveSetter(bool active)
    { 
        this.PlayerShip.SetActive(active);
        this.ShipEngineLvl01ParticleEffect.SetActive(active);
       // this.ShipEngineLvl07ParticleEffect.SetActive(active);
        this.StarParticles.SetActive(active);
        //this.UpgradeEffect.SetActive(active);
//        this.PointScoreEffect.SetActive(active);
        


        if (!active)
        {
            //this.ResetShipLevel();
            this.UpdateShipParts();
        }
    }

    private void UpdateShipParts()
    { 
        for (int i = 0; i < this.ShipParts.Count; i++)
        {
            if (i < this.CurrentShipLevel)
            {
                this.ShipParts[i].SetActive(true);
            }
            else
            {
                this.ShipParts[i].SetActive(false);
            }
        }
    }

    public void iWinButton()
    {
        this.playGoingAnim = true;
        this.ShipAnimator.Play("GoingAnimation");
    }
}
