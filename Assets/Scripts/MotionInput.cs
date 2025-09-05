using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    NORTH,
    NORTHEAST,
    EAST,
    SOUTHEAST,
    SOUTH,
    SOUTHWEST,
    WEST,
    NORTHWEST,
    NONE
}

public enum MotionType
{
    NONE,
    QFOR,
    QBACK,
    DPFOR,
    DPBACK,
    HFOR,
    HBACK,
    DOUBLEQFOR,
    DOUBLEQBACK
}

public class MotionInput : MonoBehaviour
{
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

    // public GameManager gameManager; // should be a script with int values currentFrame and effectiveCurrFrame
    
    public static readonly int maxInputLength = 15;
    public static readonly int maxBufferLength = 3;

    [HideInInspector] public int currentFrame = 0;

    private Vector2 playerInput;
    private List<UserDirection> userInputs = new List<UserDirection>();
    private UserMotion userMotion = new UserMotion(MotionType.NONE, 0);

    private Direction CalcDirection(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            return Direction.NONE;
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
            _ => Direction.NONE
        };
    }

    public bool TestForMotionInput(TestDirection[] requirements, List<UserDirection> inputs, int inputIndex, int validSkips)
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
                    // print($"prints: {currentFrame}");
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

    public MotionType ReadMotionFromInputs(List<UserDirection> inputs, int startIndex)
    {
        if (currentFrame - inputs[^startIndex].startFrame >= maxBufferLength)
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

    public MotionType ScanListForMotion(List<UserDirection> directions)
    {
        MotionType motion;
        for (int i = 1; i <= maxBufferLength; i++)
        {
            if (directions.Count < i)
            {
                // return if accessing directions[^i] would cause an out-of-bounds error
                return MotionType.NONE;
            }

            motion = ReadMotionFromInputs(userInputs, i);
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

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        /*
        userInputs = new List<UserDirection>() {
            new UserDirection(Direction.EAST, 2),
            new UserDirection(Direction.SOUTHEAST, 2),
            new UserDirection(Direction.SOUTH, 17),
            new UserDirection(Direction.SOUTHEAST, 17),
            new UserDirection(Direction.EAST, 17)
        };
        */

        Direction direction = CalcDirection(playerInput);
        if (userInputs.Count == 0 || userInputs[^1].direction != direction)
        {
            userInputs.Add(new UserDirection(direction, currentFrame));
        }

        MotionType motion = ScanListForMotion(userInputs);
        if (motion != userMotion.motion)
        {
            userMotion = new UserMotion(motion, currentFrame);
        }

        /*

        MotionType motion = ReadMotionFromInputs(userInputs, 1);
        if (motion != MotionType.NONE && userMotion.motion != motion)
        {
            userMotion = new UserMotion(motion, currentFrame);
        }
        else if (userMotion.motion != MotionType.NONE && currentFrame - userMotion.startFrame >= maxBufferLength)
        {
            userMotion = new UserMotion(MotionType.NONE, currentFrame);
        }

        */

        //print(userMotion);

        //print(direction);
        //print(currentFrame);
        //print(ScanListForMotion(userInputs));

        currentFrame++; // update frame tracker for next frame
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

public class UserDirection
{
    public Direction direction { get; }
    public int startFrame { get; }

    public UserDirection(Direction direction, int startFrame)
    {
        this.direction = direction;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{direction}; frame {startFrame}";
    }
}

public class UserMotion
{
    public MotionType motion { get; }
    public int startFrame { get; }

    public UserMotion(MotionType motion, int startFrame)
    {
        this.motion = motion;
        this.startFrame = startFrame;
    }

    public override string ToString()
    {
        return $"{motion}; frame {startFrame}";
    }
}