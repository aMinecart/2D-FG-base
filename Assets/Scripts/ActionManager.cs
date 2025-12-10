using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(MotionInput))]
[RequireComponent(typeof(ButtonInput))]
public class ActionManager : MonoBehaviour
{
    private static StringBuilder stringBuilder = new StringBuilder();

    private GameManager gameManager;
    private PlayerManager playerManager;

    private MotionInput motionInput;
    private ButtonInput buttonInput;

    private int? actionStartFrame = null;
    private FrameData actionFrameData;
    private CustomCollider2D actionHitbox;

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
    
    private bool FindAction(string code, List<PlayerAction> actions, out PlayerAction result)
    {
        result = actions.Find(action => action.actionCode == code);
        
        return result != null;
    }

    private void FormHitbox(BoxInfo[] bases)
    {
        PhysicsShapeGroup2D shapes = new PhysicsShapeGroup2D();

        foreach (BoxInfo box in bases)
        {
            shapes.AddBox(new Vector2(box.push, box.raise), new Vector2(box.length, box.length));
        }
        
        actionHitbox.SetCustomShapes(shapes);
    }

    private void ManageActionStatus()
    {
        if (!actionStartFrame.HasValue)
        {
            return;
        }

        int timeSince = gameManager.currentFrame - actionStartFrame.Value;

        if (timeSince == 0)
        {
            playerManager.actionable = false;
            actionHitbox.enabled = false;
        }

        if (timeSince == actionFrameData.startup)
        {
            actionHitbox.enabled = true;
        }

        if (timeSince == actionFrameData.startup + actionFrameData.active)
        {
            actionHitbox.enabled = false;
        }

        if (timeSince == actionFrameData.startup + actionFrameData.active + actionFrameData.recovery)
        {
            actionStartFrame = null;
            playerManager.actionable = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerManager = GetComponent<PlayerManager>();

        motionInput = GetComponent<MotionInput>();
        buttonInput = GetComponent<ButtonInput>();

        actionHitbox = GetComponent<CustomCollider2D>();

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
        if (buttonInput.pressedButtons.Count > 0 && playerManager.actionable)
        {
            // test prints
            // print(GenerateActionCode(motionInput.inputRecord[^1].direction, buttonInput.pressedButtons[^1]));
            // print(GenerateActionCode(motionInput.userMotion.motion, buttonInput.pressedButtons[^1]));

            string specialCode = GenerateActionCode(motionInput.userMotion.motion, buttonInput.pressedButtons[^1]);
            string normalCode = GenerateActionCode(motionInput.inputRecord[^1].direction, buttonInput.pressedButtons[^1]);

            if (playerManager.actionsByCode.TryGetValue(specialCode, out PlayerAction specialAction))
            {
                print(specialAction.actionCode);
                FormHitbox(specialAction.hitboxes);

                actionStartFrame = gameManager.currentFrame;
                actionFrameData = specialAction.frameData;
            }
            else if (playerManager.actionsByCode.TryGetValue(normalCode, out PlayerAction normalAction))
            {
                print(normalAction.actionCode);
                FormHitbox(normalAction.hitboxes);

                actionStartFrame = gameManager.currentFrame;
                actionFrameData = normalAction.frameData;
            }
        }

        ManageActionStatus();
        print(playerManager.actionable);
    }
}
