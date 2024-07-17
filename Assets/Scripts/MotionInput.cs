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
    public static readonly int maxInputLength = 20;
    public static readonly int maxBufferLength = 15;
    public int currentFrame = 1;

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

        //print(inputs[^startIndex].startFrame);

        if (/*inputs[^startIndex].startFrame - inputs[^(inputIndex - 1)].startFrame > maxInputLength*/
            currentFrame - inputs[^startIndex].startFrame > maxBufferLength)
        {
            print(currentFrame);
            return false;
        }

        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TestDirection[] qfor_requirements = new TestDirection[] {
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHEAST, false),
            new TestDirection(Direction.EAST, false)
        };

        TestDirection[] qback_requirements = new TestDirection[] {
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHWEST, false),
            new TestDirection(Direction.WEST, false)
        };

        TestDirection[] dpfor_requirements = new TestDirection[] {
            new TestDirection(Direction.EAST, false),
            new TestDirection(Direction.SOUTHEAST, true),
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHEAST, false),
            new TestDirection(Direction.EAST, true)
        };

        TestDirection[] dpback_requirements = new TestDirection[] {
            new TestDirection(Direction.WEST, false),
            new TestDirection(Direction.SOUTHWEST, true),
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHWEST, false),
            new TestDirection(Direction.WEST, true)
        };

        TestDirection[] hfor_requirements = new TestDirection[] {
            new TestDirection(Direction.WEST, false),
            new TestDirection(Direction.SOUTHWEST, true),
            new TestDirection(Direction.SOUTH, true),
            new TestDirection(Direction.SOUTHEAST, true),
            new TestDirection(Direction.EAST, false)
        };

        TestDirection[] hback_requirements = new TestDirection[] {
            new TestDirection(Direction.EAST, false),
            new TestDirection(Direction.SOUTHEAST, true),
            new TestDirection(Direction.SOUTH, true),
            new TestDirection(Direction.SOUTHWEST, true),
            new TestDirection(Direction.WEST, false)
        };

        TestDirection[] doubleqfor_requirements = new TestDirection[] {
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHEAST, true),
            new TestDirection(Direction.EAST, false),
            new TestDirection(Direction.SOUTH, true),
            new TestDirection(Direction.SOUTHEAST, true),
            new TestDirection(Direction.EAST, true)
        };

        TestDirection[] doubleqback_requirements = new TestDirection[] {
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHWEST, true),
            new TestDirection(Direction.WEST, false),
            new TestDirection(Direction.SOUTH, true),
            new TestDirection(Direction.SOUTHWEST, true),
            new TestDirection(Direction.WEST, true)
        };

        /*
        TestDirection[] hplusfor_requirements = new TestDirection[] {
            new TestDirection(Direction.EAST, false),
            new TestDirection(Direction.SOUTHEAST, true),
            new TestDirection(Direction.SOUTH, true),
            new TestDirection(Direction.SOUTHWEST, true),
            new TestDirection(Direction.WEST, false),
            new TestDirection(Direction.EAST, false)
        };
        */

        List<UserDirection> inputs = new List<UserDirection>() {
            new UserDirection(Direction.SOUTH, 1),
            new UserDirection(Direction.WEST, 1),
            new UserDirection(Direction.SOUTH, 1),
            new UserDirection(Direction.SOUTHWEST, 1),
            new UserDirection(Direction.WEST, 2)
        };

        if (testForMotionInput(doubleqfor_requirements, inputs, 1, 2))
        {
            print("doubleqfor");
        }
        else if (testForMotionInput(doubleqback_requirements, inputs, 1, 2))
        {
            print("doubleqback");
        }
        else if (testForMotionInput(hfor_requirements, inputs, 1, 1))
        {
            print("hfor");
        }
        else if (testForMotionInput(hback_requirements, inputs, 1, 1))
        {
            print("hback");
        }
        else if (testForMotionInput(dpfor_requirements, inputs, 1, 2))
        {
            print("dpfor");
        }
        else if (testForMotionInput(dpback_requirements, inputs, 1, 2))
        {
            print("dpback");
        }
        else if (testForMotionInput(qfor_requirements, inputs, 1, 0))
        {
            print("qfor");
        }
        else if (testForMotionInput(qback_requirements, inputs, 1, 0))
        {
            print("qback");
        }
        else
        {
            print("none");
        }
        
        currentFrame++;
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