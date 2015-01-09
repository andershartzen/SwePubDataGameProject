using UnityEngine;
using System.Collections;

public class ResolutionManager : MonoBehaviour {

    public int Width;
    public int Height;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(this.Width, this.Height, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
