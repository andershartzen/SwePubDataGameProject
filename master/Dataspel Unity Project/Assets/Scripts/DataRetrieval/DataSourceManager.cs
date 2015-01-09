using UnityEngine;
using System.Collections;

public enum DataSource
{ SwePub, OfflineTesting, File}

public class DataSourceManager : MonoBehaviour {

    public DataSource DataSrc;

    public bool DebugMode = false;

    public DataRetriever DataRetriveObj
    {
        get
        {
            if (this.DataSrc == DataSource.SwePub)
            {
                return this.SwePub;
            }
            else if (this.DataSrc == DataSource.OfflineTesting)
            {
                return this.OfflineTesting;
            }
            else if (this.DataSrc == DataSource.File)
            {
                return this.File;
            }
            return this.SwePub;

        }
    }

    private DataRetriever SwePub;
    private DataRetriever File;
    private DataRetriever OfflineTesting;

	// Use this for initialization
	void Start () {
        SwePub = new DataRetrieverSwePub();
        File = new DataRetrieverFile();
        OfflineTesting = new DataRetrieverOfflineTesting();

        if (DebugMode)
        {
            DataRetrieverSwePub swed = (DataRetrieverSwePub)SwePub;
            swed.DebugMode = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
