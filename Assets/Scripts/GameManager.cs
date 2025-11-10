using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentFrame { get; private set; } = 0;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialization()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            return;
        }

        var instance = new GameObject { name = "AutoSingleton" };
        instance.AddComponent<GameManager>();
        DontDestroyOnLoad(instance);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentFrame++; // update frame tracker for next frame
    }
}
