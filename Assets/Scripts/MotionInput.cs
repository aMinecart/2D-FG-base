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
    public int currentFrame = 0;

    // public GameManager gameManager; // should be a script with int values currentFrame and effectiveCurrFrame

    public bool testForMotionInput(TestDirection[] requirements, List<UserDirection> inputs, int indexFromEnd, int validSkips)
    {
        if (indexFromEnd > inputs.Count)
        {
            return false;
        }

        int skips = 0;

        for (int i = 1; i <= requirements.Length; i++)
        {
            if (indexFromEnd > inputs.Count)
            {
                return false;
            }

            if (inputs[^indexFromEnd].direction == requirements[^i].direction)
            {
                indexFromEnd++;
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

        if (currentFrame - inputs[^indexFromEnd].startFrame > maxInputLength)
        {
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
        TestDirection[] requirements = new TestDirection[] {
            new TestDirection(Direction.SOUTH, false),
            new TestDirection(Direction.SOUTHEAST, false),
            new TestDirection(Direction.EAST, false)
        };

        List<UserDirection> inputs = new List<UserDirection>() {
            new UserDirection(Direction.SOUTH, 3),
            new UserDirection(Direction.SOUTHEAST, 4),
            new UserDirection(Direction.EAST, 5)
        };

        print(testForMotionInput(requirements, inputs, inputs.Count, 0));

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