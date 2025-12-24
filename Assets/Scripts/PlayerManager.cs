using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

enum Character
{
    TEST
}

[RequireComponent(typeof(ActionManager))]
public class PlayerManager : MonoBehaviour
{
    public bool actionable { get; set; } = true;

    // private List<PlayerActionOld> playerActions = new List<PlayerActionOld>();
    [HideInInspector] public Dictionary<string, PlayerActionOld> actionsByCode = new Dictionary<string, PlayerActionOld>();

    private string FindCharacterJSONPath(Character name)
    {
        return name switch
        {
            _ => "TestCharacter"
        };
    }

    private void GenerateTestJSON()
    {
        List<(string, PlayerActionOld)> actions = new List<(string, PlayerActionOld)>() {
            ("5H", new PlayerActionOld("5H", new FrameData(1, 10, 100), new BoxInfo[] { new BoxInfo(2, 3, 4, 6) })),
            ("6M", new PlayerActionOld("6M", new FrameData(15, 50, 30), new BoxInfo[] { new BoxInfo(1, 0, 1, 1) }))
            };

        string filepath = Path.Combine(Application.persistentDataPath + "TestCharacter_NEW.json");
        // print(filepath);

        //print(JsonConvert.SerializeObject(list, Formatting.None));
        File.WriteAllText(filepath, JsonConvert.SerializeObject(actions, Formatting.Indented));
    }

    // Start is called before the first frame update
    private void Start()
    {
        // PlayerActionOld pA = new PlayerActionOld("j.H", new FrameData(1, 10, 100), new BoxInfo[] { new BoxInfo(2, 3, 4, 6) });
        // List<PlayerActionOld> playerActions = new List<PlayerActionOld>();
        
        GenerateTestJSON(); // remove once finalized

        string filePath = Path.Combine(Application.persistentDataPath + FindCharacterJSONPath(Character.TEST) + ".json");
        List<PlayerActionOld> actions = GameManager.GetJSONAsList<PlayerActionOld>(filePath);

        foreach (PlayerActionOld action in actions)
        {
            actionsByCode.Add(action.actionCode, action);
            // print(action.frameData);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // print(actionable);
    }
}
