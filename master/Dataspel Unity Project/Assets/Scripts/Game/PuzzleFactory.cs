using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PuzzleFactory {

    public static List<AbstractPuzzle> CreateAbstractPuzzles(List<Publication> Publications, int maxNumOfAnswers)
    {
        CheckAndRemoveDuplicates(Publications);

        List<AbstractPuzzle> Puzzles = new List<AbstractPuzzle>();
        if (maxNumOfAnswers > Publications.Count)
        {
            return Puzzles;
        }
        //TODO: Randomize publication list?
        Publications.Shuffle<Publication>();
        int numOfPuzzles = (int)(Publications.Count / maxNumOfAnswers);
        //Debug.Log("maxNumofAnswers " + maxNumOfAnswers);
        //Debug.Log("numOfPuzzles " + numOfPuzzles);

        for (int i = 0; i < Publications.Count; i = i + maxNumOfAnswers)
        {
            List<Publication> puzzleContent = new List<Publication>();
            int j = i;
            int answer = 0;
            while (j < Publications.Count && answer < maxNumOfAnswers)
            {
                puzzleContent.Add(Publications[j]);

                j++;
                answer++;
            }
            Puzzles.Add(new AbstractPuzzle(puzzleContent));
        }


            //for (int p = 0; p < Publications.Count; p = p + maxNumOfAnswers)
            //{
            //    List<Publication> puzCont = new List<Publication>();
            //    for (int i = p; i < maxNumOfAnswers + p; i++)
            //    {
            //        puzCont.Add(Publications[i]);
            //        //Debug.Log("i " + i);
            //    }

            //    //Debug.Log("puzcount " + puzCont.Count);
            //    if (puzCont.Count != 0)
            //    {
            //        Puzzles.Add(new AbstractPuzzle(puzCont));
            //    }

            //}
        return Puzzles;
    }

    private static void CheckAndRemoveDuplicates(List<Publication> publications)
    {
        List<Publication> toRemove = new List<Publication>();

        foreach(Publication pub in publications)
        {
            for (int i = 0; i < publications.Count; i++)
            {
                if (publications[i] == pub)
                {
                    continue;
                }

                if (publications[i].Title == pub.Title)
                {
                    toRemove.Add(publications[i]);
                }
            }
        }

        foreach (Publication pubs in toRemove)
        {
            publications.Remove(pubs);
        }
    }

    private static void CheckAndRemoveMissingAbstracts(List<Publication> publications)
    {
        List<Publication> toRemove = new List<Publication>();

        foreach (Publication pub in publications)
        {
            if (pub.Abstract == string.Empty)
            {
                toRemove.Add(pub);
            }
        }

        foreach (Publication pubs in toRemove)
        {
            publications.Remove(pubs);
        }
    }

}

//From http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public static class ThreadSafeRandom
{
    [System.ThreadStatic]
    private static System.Random Local;

    public static System.Random ThisThreadsRandom
    {
        get { return Local ?? (Local = new System.Random(unchecked(System.Environment.TickCount * 31 + System.Threading.Thread.CurrentThread.ManagedThreadId))); }
    }
}

