using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    NORTH = 8,
    NORTHEAST = 9,
    EAST = 6,
    SOUTHEAST = 3,
    SOUTH = 2,
    SOUTHWEST = 1,
    WEST = 4,
    NORTHWEST = 7,
    NEUTRAL = 5
}

public enum MotionType
{
    NONE = 0,
    QFOR = 236,
    QBACK = 214,
    DPFOR = 623,
    DPBACK = 421,
    HFOR = 41236,
    HBACK = 63214,
    DOUBLEQFOR = 236236,
    DOUBLEQBACK = 214214
}

public class MotionInput : MonoBehaviour
{
    public GameManager gameManager; // should be a script with int values currentFrame and effectiveCurrFrame
    
    public static readonly int maxInputLength = 15;
    public static readonly int maxBufferLength = 10;

    public Vector2 playerInput { get; private set; }
    public List<DirectionRecord> inputRecord { get; private set; } = new List<DirectionRecord>();
    public MotionRecord userMotion { get; private set; } = new MotionRecord(MotionType.NONE, 0);

    private static Direction CalcDirection(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            return Direction.NEUTRAL;
        }

        float angle = VectorFunctions.GetVectorAngle(input);
        return angle switch
        {
            > 337.5f or <= 22.5f => Direction.EAST,
            > 22.5f and <= 67.5f => Direction.NORTHEAST,
            > 67.5f and <= 112.5f => Direction.NORTH,
            > 112.5f and <= 157.5f => Direction.NORTHWEST,
            > 157.5f and <= 202.5f => Direction.WEST,
            > 202.5f and <= 247.5f => Direction.SOUTHWEST,
            > 247.5f and <= 292.5f => Direction.SOUTH,
            > 292.5f and <= 337.5f => Direction.SOUTHEAST,
            _ => Direction.NEUTRAL
        };
    }

    private static bool TestForMotionInput(MotionCriteria criteria, List<DirectionRecord> inputs,
                                           int inputIndex, int maxInputLength)
    {
        int endIndex = inputIndex;
        int skips = 0;

        TestDirection[] requirements = criteria.directionList;
        for (int i = 1; i <= requirements.Length; i++)
        {
            if (inputIndex > inputs.Count) // end search if end of inputs is reached
            {
                return false;
            }

            if (inputs[^inputIndex].direction == requirements[^i].direction)
            {
                inputIndex++;
            }
            else if (requirements[^i].skippable)
            {
                skips++;

                if (skips > criteria.allowedSkips)
                {
                    // print($"prints: {gameManager.currentFrame}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        if (inputs[^endIndex].startFrame - inputs[^(inputIndex - 1)].startFrame >= maxInputLength)
        {
            return false;
        }

        // print(inputs[^endIndex].startFrame - inputs[^(inputIndex - 1)].startFrame);
        return true;
    }

    private MotionType ReadMotionFromInputs(List<DirectionRecord> inputs, int startIndex,
                                            int maxInputLength, int maxBufferLength)
    {
        if (gameManager.currentFrame - inputs[^startIndex].startFrame >= maxBufferLength)
        {
            return MotionType.NONE;
        }

        if (TestForMotionInput(MotionFormats.doubleqfor_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.DOUBLEQFOR;
        }
        else if (TestForMotionInput(MotionFormats.doubleqback_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.DOUBLEQBACK;
        }
        else if (TestForMotionInput(MotionFormats.hfor_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.HFOR;
        }
        else if (TestForMotionInput(MotionFormats.hback_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.HBACK;
        }
        else if (TestForMotionInput(MotionFormats.dpfor_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.DPFOR;
        }
        else if (TestForMotionInput(MotionFormats.dpback_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.DPBACK;
        }
        else if (TestForMotionInput(MotionFormats.qfor_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.QFOR;
        }
        else if (TestForMotionInput(MotionFormats.qback_requirements, inputs, startIndex, maxInputLength))
        {
            return MotionType.QBACK;
        }
        else
        {
            return MotionType.NONE;
        }
    }

    private MotionType ScanListForMotion(List<DirectionRecord> directions, MotionRecord prevMotion,
                                         int maxInputLength, int maxBufferLength)
    {
        //MotionType motion;

        //for (int i = 1; i <= maxBufferLength; i++)
        //{
        //    if (directions.Count < i)
        //    {
        //        // stop search if accessing directions[^i] would cause an out-of-bounds error
        //        break;
        //    }

        //    motion = ReadMotionFromInputs(directions, i, maxInputLength, maxBufferLength);
        //    if (motion != MotionType.NONE)
        //    {
        //        return motion;
        //    }
        //}

        //// return NONE if no special motion type was detected
        //return MotionType.NONE;

        if (directions.Count == 0)
        {
            return MotionType.NONE;
        }

        MotionType motion = ReadMotionFromInputs(directions, 1, maxInputLength, maxBufferLength);
        if (motion == MotionType.NONE && gameManager.currentFrame - prevMotion.startFrame < maxBufferLength)
        {
            return prevMotion.motion;
        }

        return motion;
    }

    private void OnMove(InputValue moveValue)
    {
        playerInput = moveValue.Get<Vector2>();
    }

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        /*
        inputRecord = new List<DirectionRecord>() {
            new DirectionRecord(Direction.EAST, 2),
            new DirectionRecord(Direction.SOUTHEAST, 2),
            new DirectionRecord(Direction.SOUTH, 17),
            new DirectionRecord(Direction.SOUTHEAST, 17),
            new DirectionRecord(Direction.EAST, 17)
        };
        */

        Direction direction = CalcDirection(playerInput);
        if (inputRecord.Count == 0 || inputRecord[^1].direction != direction)
        {
            inputRecord.Add(new DirectionRecord(direction, gameManager.currentFrame));
        }

        MotionType motion = ScanListForMotion(inputRecord, userMotion, maxInputLength, maxBufferLength);
        if (motion != userMotion.motion)
        {
            userMotion = new MotionRecord(motion, gameManager.currentFrame);
        }

        /*

        MotionType motion = ReadMotionFromInputs(inputRecord, 1);
        if (motion != MotionType.NEUTRAL && userMotion.motion != motion)
        {
            userMotion = new MotionRecord(motion, gameManager.currentFrame);
        }
        else if (userMotion.motion != MotionType.NEUTRAL && gameManager.currentFrame - userMotion.startFrame >= maxBufferLength)
        {
            userMotion = new MotionRecord(MotionType.NEUTRAL, gameManager.currentFrame);
        }

        */

        // print(inputRecord[^1]);
        // print(userMotion);

        // print(direction);
        // print(gameManager.currentFrame);
        // print(ScanListForMotion(inputRecord));
    }
}