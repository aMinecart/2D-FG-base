using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimedSequence<T>
{
    public KeyValuePair<int, T>[] sequence { get; }

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

    public IEnumerable<T> GetCompleteSequence(int sequenceLength)
    {
        int currIndex = 0;
        for (int timing = 0; timing <= sequenceLength; timing++)
        {
            yield return (sequence[currIndex].Key == timing) ? sequence[currIndex++].Value : default;
        }
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

public class TimedIterator<T>
{
    private KeyValuePair<int, T>[] sequence;
    private int currIndex = 0;

    public TimedIterator(TimedSequence<T> timedSequence)
    {
        this.sequence = timedSequence.sequence;
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