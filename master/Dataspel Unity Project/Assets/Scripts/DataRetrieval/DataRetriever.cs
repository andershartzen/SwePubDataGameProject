using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public abstract class DataRetriever {
    public bool isLoading
    {
        get {
            bool tmp;
            lock (_lock)
            {
                tmp = _isLoading;
            }
            return tmp;
        }
        internal set
        {
            lock (_lock)
            {
                _isLoading = value;
            }
        }
    } private bool _isLoading = false;

    private object _lock = new object();
    private System.Threading.Thread m_Thread = null;

    public delegate void AuthorSearchCompletedEventHandler(List<Author> foundAuthors);
    public event AuthorSearchCompletedEventHandler AuthorSearchCompleted;

    public delegate void PublicationSearchCompletedEventHandler(Request resolvedRequest);
    public event PublicationSearchCompletedEventHandler PublicationSearchCompleted;

    protected virtual void AuthorSearchDone(List<Author> foundAuthors)
    {
        Debug.Log("AuthorSearchDone called");
        this.isLoading = false;
        Debug.Log("AuthorSearchCompleted is " + AuthorSearchCompleted);
        if (AuthorSearchCompleted != null)
        {
            AuthorSearchCompleted(foundAuthors);
            Debug.Log("AuthorSearchCompleted fired");
        }
    }

    protected virtual void PublicationSearchDone(Request resolvedRequest)
    {
        this.isLoading = false;
        if (PublicationSearchCompleted != null)
        {
            PublicationSearchCompleted(resolvedRequest);
        }
    }

    protected void StartThread(Thread threadToStart)
    {
        this.isLoading = true;
        this.m_Thread = threadToStart;
        this.m_Thread.Start();
    }
    public abstract void FindPublication(Request req, int Limit);
}
