using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static StringBuilder stringBuilder = new StringBuilder();

    [HideInInspector] public static int currentFrame = 0;

    [SerializeField] private PlayerManager playerManager;

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
            stringBuilder.Append("j.");
        }
        
        stringBuilder.Append((int)motion);
        stringBuilder.Append(button.ToString()[0]);

        return stringBuilder.ToString();
    }

    public bool FindAction(string code, List<PlayerAction> actions, out PlayerAction result)
    {
        result = actions.Find(action => action.actionCode == code); ;

        if (result != null)
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
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
            // print(GenerateActionCode(motionInput.inputRecord[^1].direction, buttonInput.pressedButtons[^1]));
            // print(GenerateActionCode(motionInput.userMotion.motion, buttonInput.pressedButtons[^1]));
            
            List<PlayerAction> list = new List<PlayerAction>();

            string motCode = GenerateActionCode(motionInput.userMotion.motion, buttonInput.pressedButtons[^1]);
            string dirCode = GenerateActionCode(motionInput.inputRecord[^1].direction, buttonInput.pressedButtons[^1]);

            if (FindAction(motCode, list, out PlayerAction mResult))
            {
                Instantiate(mResult.hitbox);
            }
            else if (FindAction(dirCode, list, out PlayerAction dResult))
            {

                Instantiate(dResult.hitbox);
            }
        }

        currentFrame++; // update frame tracker for next frame
    }
}
