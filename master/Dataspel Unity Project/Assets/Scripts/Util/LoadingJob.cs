using UnityEngine;
using System.Collections;

public class LoadingJob : ThreadedJob {
    private DataRetriever DataStore;

    public LoadingJob(DataRetriever DataStore)
    {
        this.DataStore = DataStore;
    }

	protected override void ThreadFunction()
    {
        
    }

    protected override void OnFinished()
    {
        
    }
}
