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
    // public GameManager gameManager; // should be a script with int values currentFrame and effectiveCurrFrame
    
    public static readonly int maxInputLength = 15;
    public static readonly int maxBufferLength = 10;

    public Vector2 playerInput { get; private set; }
    public List<DirectionRecord> inputRecord { get; private set; } = new List<DirectionRecord>();
    public MotionRecord userMotion { get; private set; } = new MotionRecord(MotionType.NONE, 0);

    private static readonly TestDirection[] qfor_requirements = new TestDirection[] {
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHEAST, false),
        new TestDirection(Direction.EAST, false)
    };

    private static readonly TestDirection[] qback_requirements = new TestDirection[] {
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHWEST, false),
        new TestDirection(Direction.WEST, false)
    };

    private static readonly TestDirection[] dpfor_requirements = new TestDirection[] {
        new TestDirection(Direction.EAST, false),
        new TestDirection(Direction.SOUTHEAST, true),
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHEAST, false),
        new TestDirection(Direction.EAST, true)
    };

    private static readonly TestDirection[] dpback_requirements = new TestDirection[] {
        new TestDirection(Direction.WEST, false),
        new TestDirection(Direction.SOUTHWEST, true),
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHWEST, false),
        new TestDirection(Direction.WEST, true)
    };

    private static readonly TestDirection[] hfor_requirements = new TestDirection[] {
        new TestDirection(Direction.WEST, false),
        new TestDirection(Direction.SOUTHWEST, true),
        new TestDirection(Direction.SOUTH, true),
        new TestDirection(Direction.SOUTHEAST, true),
        new TestDirection(Direction.EAST, false)
    };

    private static readonly TestDirection[] hback_requirements = new TestDirection[] {
        new TestDirection(Direction.EAST, false),
        new TestDirection(Direction.SOUTHEAST, true),
        new TestDirection(Direction.SOUTH, true),
        new TestDirection(Direction.SOUTHWEST, true),
        new TestDirection(Direction.WEST, false)
    };

    private static readonly TestDirection[] doubleqfor_requirements = new TestDirection[] {
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHEAST, true),
        new TestDirection(Direction.EAST, false),
        new TestDirection(Direction.SOUTH, true),
        new TestDirection(Direction.SOUTHEAST, true),
        new TestDirection(Direction.EAST, true)
    };

    private static readonly TestDirection[] doubleqback_requirements = new TestDirection[] {
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHWEST, true),
        new TestDirection(Direction.WEST, false),
        new TestDirection(Direction.SOUTH, true),
        new TestDirection(Direction.SOUTHWEST, true),
        new TestDirection(Direction.WEST, true)
    };

    /*
    private static readonly TestDirection[] hplus_requirements = new TestDirection[] {
        new TestDirection(Direction.EAST, false),
        new TestDirection(Direction.SOUTHEAST, true),
        new TestDirection(Direction.SOUTH, false),
        new TestDirection(Direction.SOUTHWEST, true),
        new TestDirection(Direction.WEST, false),
        new TestDirection(Direction.EAST, false)
    };

    private static readonly TestDirection[] althplus_requirements = new TestDirection[] {
        new TestDirection(Direction.EAST, false),
        new TestDirection(Direction.SOUTHEAST, false),
        new TestDirection(Direction.SOUTH, true),
        new TestDirection(Direction.SOUTHWEST, false),
        new TestDirection(Direction.WEST, false),
        new TestDirection(Direction.EAST, false)
    };
    */

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

    private bool TestForMotionInput(TestDirection[] requirements, List<DirectionRecord> inputs, int inputIndex, int validSkips)
    {
        if (inputIndex > inputs.Count)
        {
            return false;
        }

        int startIndex = inputIndex;
        int skips = 0;

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

                if (skips > validSkips)
                {
                    // print($"prints: {ActionManager.currentFrame}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        if (inputs[^startIndex].startFrame - inputs[^(inputIndex - 1)].startFrame >= maxInputLength)
        {
            return false;
        }

        // print(inputs[^startIndex].startFrame - inputs[^(inputIndex - 1)].startFrame);
        return true;
    }

    private MotionType ReadMotionFromInputs(List<DirectionRecord> inputs, int startIndex)
    {
        if (ActionManager.currentFrame - inputs[^startIndex].startFrame >= maxBufferLength)
        {
            return MotionType.NONE;
        }

        if (TestForMotionInput(doubleqfor_requirements, inputs, startIndex, 2))
        {
            return MotionType.DOUBLEQFOR;
        }
        else if (TestForMotionInput(doubleqback_requirements, inputs, startIndex, 2))
        {
            return MotionType.DOUBLEQBACK;
        }
        else if (TestForMotionInput(hfor_requirements, inputs, startIndex, 1))
        {
            return MotionType.HFOR;
        }
        else if (TestForMotionInput(hback_requirements, inputs, startIndex, 1))
        {
            return MotionType.HBACK;
        }
        else if (TestForMotionInput(dpfor_requirements, inputs, startIndex, 2))
        {
            return MotionType.DPFOR;
        }
        else if (TestForMotionInput(dpback_requirements, inputs, startIndex, 2))
        {
            return MotionType.DPBACK;
        }
        else if (TestForMotionInput(qfor_requirements, inputs, startIndex, 0))
        {
            return MotionType.QFOR;
        }
        else if (TestForMotionInput(qback_requirements, inputs, startIndex, 0))
        {
            return MotionType.QBACK;
        }
        else
        {
            return MotionType.NONE;
        }
    }

    private MotionType ScanListForMotion(List<DirectionRecord> directions)
    {
        MotionType motion;

        for (int i = 1; i <= maxBufferLength; i++)
        {
            if (directions.Count < i)
            {
                // return if accessing directions[^i] would cause an out-of-bounds error
                return MotionType.NONE;
            }

            motion = ReadMotionFromInputs(inputRecord, i);
            if (motion != MotionType.NONE)
            {
                return motion;
            }
        }

        return MotionType.NONE;
    }

    private void OnMove(InputValue moveValue)
    {
        playerInput = moveValue.Get<Vector2>();
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
            inputRecord.Add(new DirectionRecord(direction, ActionManager.currentFrame));
        }

        MotionType motion = ScanListForMotion(inputRecord);
        if (motion != userMotion.motion)
        {
            userMotion = new MotionRecord(motion, ActionManager.currentFrame);
        }

        /*

        MotionType motion = ReadMotionFromInputs(inputRecord, 1);
        if (motion != MotionType.NEUTRAL && userMotion.motion != motion)
        {
            userMotion = new MotionRecord(motion, ActionManager.currentFrame);
        }
        else if (userMotion.motion != MotionType.NEUTRAL && ActionManager.currentFrame - userMotion.startFrame >= maxBufferLength)
        {
            userMotion = new MotionRecord(MotionType.NEUTRAL, ActionManager.currentFrame);
        }

        */

        // print(inputRecord[^1]);

        // print(direction);
        // print(ActionManager.currentFrame);
        // print(ScanListForMotion(inputRecord));
    }
}

public class TestDirection
{
    public Direction direction { get; }
    public bool skippable { get; }

    public TestDirection(Direction direction, bool skippable)
    {
        this.direction = direction;
        this.skippable = skippable;
    }
}

public class DirectionRecord
{
    public Direction direction { get; }
    public int startFrame { get; }

    public DirectionRecord(Direction direction, int startFrame)
    {
        this.direction = direction;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{direction}; frame {startFrame}";
    }
}

public class MotionRecord
{
    public MotionType motion { get; }
    public int startFrame { get; }

    public MotionRecord(MotionType motion, int startFrame)
    {
        this.motion = motion;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{motion}; frame {startFrame}";
    }
}