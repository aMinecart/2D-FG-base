using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

enum Character
{
    TEST
}

[RequireComponent(typeof(ActionManager))]
public class PlayerManager : MonoBehaviour
{
    public bool actionable { get; set; } = true;

    // private List<PlayerAction> playerActions = new List<PlayerAction>();
    [HideInInspector] public Dictionary<string, PlayerAction> actionsByCode = new Dictionary<string, PlayerAction>();

    private string FindCharacterJSONPath(Character name)
    {
        return name switch
        {
            _ => "TestCharacter"
        };
    }

    private void GenerateTestJSON()
    {
        List<PlayerAction> actions = new List<PlayerAction>();
        actions.Add(new PlayerAction("j.H", new FrameData(1, 10, 100), new BoxInfo[] { new BoxInfo(2, 3, 4, 6) }));
        actions.Add(new PlayerAction("6M", new FrameData(15, 50, 30), new BoxInfo[] { new BoxInfo(1, 0, 1, 1) }));

        string filepath = Path.Combine(Application.persistentDataPath + "TestCharacter.json");
        print(filepath);

        //print(JsonConvert.SerializeObject(actions, Formatting.None));
        File.WriteAllText(filepath, JsonConvert.SerializeObject(actions, Formatting.Indented));
    }

    // Start is called before the first frame update
    private void Start()
    {
        // PlayerAction pA = new PlayerAction("j.H", new FrameData(1, 10, 100), new BoxInfo[] { new BoxInfo(2, 3, 4, 6) });
        // List<PlayerAction> playerActions = new List<PlayerAction>();

        GenerateTestJSON(); // remove once finalized

        string filePath = Path.Combine(Application.persistentDataPath + FindCharacterJSONPath(Character.TEST) + ".json");

        if (File.Exists(filePath))
        {
            string actionsAsText = File.ReadAllText(filePath);
            print(actionsAsText);

            List<PlayerAction> actions = JsonConvert.DeserializeObject<List<PlayerAction>>(actionsAsText);
            // playerActions.AddRange(actions);

            foreach (PlayerAction action in actions)
            {
                actionsByCode.Add(action.actionCode, action);
                // print(action.frameData);
            }
        }

        foreach (var pair in actionsByCode)
        {
            print(pair);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // print(actionable);
    }
}
