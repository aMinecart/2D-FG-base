public static class MotionFormats
{
    public static readonly MotionCriteria qfor_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.SOUTH, false),
                new TestDirection(Direction.SOUTHEAST, false),
                new TestDirection(Direction.EAST, false)
            },
            allowedSkips: 0
        );

    public static readonly MotionCriteria qback_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.SOUTH, false),
                new TestDirection(Direction.SOUTHWEST, false),
                new TestDirection(Direction.WEST, false)
            },
            allowedSkips: 0
        );

    public static readonly MotionCriteria dpfor_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.EAST, false),
                new TestDirection(Direction.SOUTHEAST, true),
                new TestDirection(Direction.SOUTH, false),
                new TestDirection(Direction.SOUTHEAST, false),
                new TestDirection(Direction.EAST, true)
            },
            allowedSkips: 2
        );

    public static readonly MotionCriteria dpback_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.WEST, false),
                new TestDirection(Direction.SOUTHWEST, true),
                new TestDirection(Direction.SOUTH, false),
                new TestDirection(Direction.SOUTHWEST, false),
                new TestDirection(Direction.WEST, true)
            },
            allowedSkips: 2
        );

    public static readonly MotionCriteria hfor_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.WEST, false),
                new TestDirection(Direction.SOUTHWEST, true),
                new TestDirection(Direction.SOUTH, true),
                new TestDirection(Direction.SOUTHEAST, true),
                new TestDirection(Direction.EAST, false)
            },
            allowedSkips: 1
        );

    public static readonly MotionCriteria hback_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.EAST, false),
                new TestDirection(Direction.SOUTHEAST, true),
                new TestDirection(Direction.SOUTH, true),
                new TestDirection(Direction.SOUTHWEST, true),
                new TestDirection(Direction.WEST, false)
            },
            allowedSkips: 1
        );

    public static readonly MotionCriteria doubleqfor_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.SOUTH, false),
                new TestDirection(Direction.SOUTHEAST, true),
                new TestDirection(Direction.EAST, false),
                new TestDirection(Direction.SOUTH, true),
                new TestDirection(Direction.SOUTHEAST, true),
                new TestDirection(Direction.EAST, true)
            },
            allowedSkips: 2
        );

    public static readonly MotionCriteria doubleqback_requirements =
        new(
            directionList:
            new TestDirection[] {
                new TestDirection(Direction.SOUTH, false),
                new TestDirection(Direction.SOUTHWEST, true),
                new TestDirection(Direction.WEST, false),
                new TestDirection(Direction.SOUTH, true),
                new TestDirection(Direction.SOUTHWEST, true),
                new TestDirection(Direction.WEST, true)
            },
            allowedSkips: 2
        );

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
}

public readonly struct TestDirection
{
    public Direction direction { get; }
    public bool skippable { get; }

    public TestDirection(Direction direction, bool skippable)
    {
        this.direction = direction;
        this.skippable = skippable;
    }

    public override string ToString()
    {
        return $"{direction}; skippable: {skippable}";
    }
}

public readonly struct DirectionRecord
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

public readonly struct MotionRecord
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

public class MotionCriteria
{
    public TestDirection[] directionList { get; }
    public int allowedSkips { get; }

    public MotionCriteria(TestDirection[] directionList, int allowedSkips)
    {
        this.directionList = directionList;
        this.allowedSkips = allowedSkips;
    }
}