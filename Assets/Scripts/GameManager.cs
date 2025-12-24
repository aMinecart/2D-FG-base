using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentFrame { get; private set; } = 0;

    public static List<T> GetJSONAsList<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            string listAsText = File.ReadAllText(filePath);
            print(listAsText);

            return JsonConvert.DeserializeObject<List<T>>(listAsText); ;
        }

        Debug.LogWarning($"Could not find file at {filePath}");
        return null;
    }

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
