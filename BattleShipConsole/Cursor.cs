namespace BattleShipConsole;

public class Cursor
{
    private int _x = 0;
    private int _y = 0;

    public readonly int XLimit;
    public readonly int YLimit;

    public int X
    {
        get => _x;
        set
        {
            int val = value;
            while (val > XLimit)
            {
                val -= XLimit;
            }

            while (val < 0)
            {
                val += XLimit;
            }

            _x = val;
        }
    }

    public int Y
    {
        get => _y;
        set
        {
            int val = value;
            while (val > YLimit)
            {
                val -= YLimit;
            }

            while (val < 0)
            {
                val += YLimit;
            }

            _y = val;
        }
    }

    public Cursor(int xLimit = 10, int yLimit = 10)
    {
        XLimit = xLimit;
        YLimit = yLimit;
    }
}