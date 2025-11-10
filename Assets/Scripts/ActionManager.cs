using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(MotionInput))]
[RequireComponent(typeof(ButtonInput))]
public class ActionManager : MonoBehaviour
{
    private static StringBuilder stringBuilder = new StringBuilder();

    private GameManager gameManager;

    private PlayerManager playerManager;

    private MotionInput motionInput;
    private ButtonInput buttonInput;

    public static string GenerateActionCode(Direction direction, ButtonType button, bool airborne = false, bool? shortDistance = null)
    {
        stringBuilder.Clear();

        if (airborne)
        {
            stringBuilder.Append("j.");
        }
        
        if (shortDistance.HasValue)
        {
            stringBuilder.Append(shortDistance.Value ? "c" : "f");

            if (!airborne) // avoid adding multiple dots, do not mark with a dot if one is present
            {
                stringBuilder.Append(".");
            }
        }
        else
        {
            stringBuilder.Append((int)direction);
        }
        
        stringBuilder.Append(button.ToString()[0]);

        return stringBuilder.ToString();
    }

    public static string GenerateActionCode(MotionType motion, ButtonType button, bool airborne = false)
    {
        stringBuilder.Clear();

        if (airborne)
        {
            stringBuilder.Append("j."); // append jumping ID
        }
        
        stringBuilder.Append((int)motion); // append motion code (i.e. 214 for quarter circle back)
        stringBuilder.Append(button.ToString()[0]); // append first letter of action (i.e. K for KICK)

        return stringBuilder.ToString();
    }

    public bool FindAction(string code, List<PlayerAction> actions, out PlayerAction result)
    {
        result = actions.Find(action => action.actionCode == code);
        
        return result != null;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        
        playerManager = GetComponent<PlayerManager>();

        motionInput = GetComponent<MotionInput>();
        buttonInput = GetComponent<ButtonInput>();

        /*
        print(GenerateActionCode(Direction.SOUTH, ButtonType.KICK) + " = 2K");
        print(GenerateActionCode(Direction.NEUTRAL, ButtonType.MEDIUM, true) + " j.5M");
        print(GenerateActionCode(Direction.EAST, ButtonType.PUNCH, false, true));
        print(GenerateActionCode(Direction.EAST, ButtonType.HEAVY, true, false));

        print(GenerateActionCode(MotionType.HBACK ,ButtonType.PUNCH, true));
        print(GenerateActionCode(MotionType.NONE, ButtonType.WIPE));
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (buttonInput.pressedButtons.Count > 0)
        {
            print(GenerateActionCode(motionInput.inputRecord[^1].direction, buttonInput.pressedButtons[^1]));
            print(GenerateActionCode(motionInput.userMotion.motion, buttonInput.pressedButtons[^1]));
            
            

            string motCode = GenerateActionCode(motionInput.userMotion.motion, buttonInput.pressedButtons[^1]);
            string dirCode = GenerateActionCode(motionInput.inputRecord[^1].direction, buttonInput.pressedButtons[^1]);

            // if (playerManager.actionsByCode.TryGetValue(motCode, out PlayerAction mResult))
            {
                //print(mResult.actionCode);
            }

            // if (playerManager.actionsByCode.TryGetValue(dirCode, out PlayerAction dResult))
            {
                //print(dResult.actionCode);
            }
        }
    }
}
