using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NORTH,
    NORTHEAST,
    EAST,
    SOUTHEAST,
    SOUTH,
    SOUTHWEST,
    WEST,
    NORTHWEST
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

    public static readonly int maxInputLength = 20;
    public static readonly int maxBufferLength = 15;

    private int currentFrame = 1;

    // public GameManager gameManager; // should be a script with int values currentFrame and effectiveCurrFrame

    public bool testForMotionInput(TestDirection[] requirements, List<UserDirection> inputs, int inputIndex, int validSkips)
    {
        if (inputIndex > inputs.Count)
        {
            return false;
        }

        int startIndex = inputIndex;
        int skips = 0;

        for (int i = 1; i <= requirements.Length; i++)
        {
            if (inputIndex > inputs.Count) // add a limiter to prevent iterating over every value in inputs
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
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        if (inputs[^startIndex].startFrame - inputs[^(inputIndex - 1)].startFrame > maxInputLength &&
            currentFrame - inputs[^startIndex].startFrame > maxBufferLength)
        {
            return false;
        }

        print(currentFrame);
        return true;
    }

    public void readMotionFromInputs(List<UserDirection> inputs, int index)
    {
        if (testForMotionInput(doubleqfor_requirements, inputs, index, 2))
        {
            print("doubleqfor");
        }
        else if (testForMotionInput(doubleqback_requirements, inputs, index, 2))
        {
            print("doubleqback");
        }
        else if (testForMotionInput(hfor_requirements, inputs, index, 1))
        {
            print("hfor");
        }
        else if (testForMotionInput(hback_requirements, inputs, index, 1))
        {
            print("hback");
        }
        else if (testForMotionInput(dpfor_requirements, inputs, index, 2))
        {
            print("dpfor");
        }
        else if (testForMotionInput(dpback_requirements, inputs, index, 2))
        {
            print("dpback");
        }
        else if (testForMotionInput(qfor_requirements, inputs, index, 0))
        {
            print("qfor");
        }
        else if (testForMotionInput(qback_requirements, inputs, index, 0))
        {
            print("qback");
        }
        else
        {
            print("none");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        List<UserDirection> user_inputs = new List<UserDirection>() {
            new UserDirection(Direction.EAST, 3),
            new UserDirection(Direction.SOUTHEAST, 5),
            new UserDirection(Direction.SOUTH, 5),
            new UserDirection(Direction.SOUTHEAST, 5),
            // new UserDirection(Direction.EAST, 22)
        };

        readMotionFromInputs(user_inputs, 1);

        // currentFrame++;
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
}