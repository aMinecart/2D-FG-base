using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class PlayerManager : MonoBehaviour
{
    private List<PlayerAction> playerActions = new List<PlayerAction>();
    [HideInInspector] public Dictionary<string, PlayerAction> actionsByCode = new Dictionary<string, PlayerAction>();

    // Start is called before the first frame update
    void Start()
    {
        playerActions.Add(new PlayerAction("j.H", new FrameData(1, 10, 100), new BoxInfo[] { new BoxInfo(2, 3, 4, 6) }));
        playerActions.Add(new PlayerAction("6M", new FrameData(0, 50, 500), new BoxInfo[] { new BoxInfo(1, 1, 1, 1) }));
        
        // PlayerAction pA = new PlayerAction("j.H", new FrameData(1, 10, 100), new BoxInfo[] { new BoxInfo(2, 3, 4, 6) });
        
        print(JsonConvert.SerializeObject(playerActions, Formatting.None));

        string filepath = Path.Combine(Application.persistentDataPath + ".json");

        // print(filename);
        File.WriteAllText(filepath, JsonConvert.SerializeObject(playerActions, Formatting.Indented));

        if (File.Exists(filepath))
        {
            string actions = File.ReadAllText(filepath);

            playerActions.Clear();
            playerActions.AddRange(JsonConvert.DeserializeObject<List<PlayerAction>>(actions));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
