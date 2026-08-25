using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimedSequence<T>
{
    public IEnumerable<KeyValuePair<int, T>> sequence { get; }

    public int[] timings
    {
        get
        {
            return sequence.Select(kvp => kvp.Key).ToArray();
        }
    }

    public T[] items
    {
        get
        {
            return sequence.Select(kvp => kvp.Value).ToArray();
        }
    }

    public TimedSequence(IEnumerable<KeyValuePair<int, T>> sequence)
    {
        this.sequence = sequence.OrderBy(kvp => kvp.Key).ToArray();
    }

    public IEnumerator<KeyValuePair<int, T>> GetSequenceIterator()
    {
        return sequence.GetEnumerator();
    }

    /* private Dictionary<int, T> sequence { get; }

    public int[] timings
    {
        get
        {
            return sequence.Keys.ToArray();
        }
    }

    public T[] items {
        get
        {
            return sequence.Values.ToArray();
        }
    }

    public TimedSequence(Dictionary<int, T> sequence)
    {
        this.sequence = sequence;
    }

    public T RetrieveItemOrDefault(int timing)
    {
        return sequence.GetValueOrDefault(timing);
    }
    */
}

/*
public class TimedIterator<T>
{
    private KeyValuePair<int, T>[] sequence;
    private int currIndex = 0;

    public TimedIterator(TimedSequence<T> timedSequence)
    {
        this.sequence = timedSequence.sequence.ToArray();
    }

    public T CheckTiming(int timing)
    {
        if (currIndex >= sequence.Length || sequence[currIndex].Key != timing)
        {
            return default;
        }

        return sequence[currIndex++].Value;
    }
}
*/