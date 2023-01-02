using BattleShipConsole.Interfaces;

namespace BattleShipConsole;

public class Cursor : ICursor
{
    private int _x;
    private int _y;

    public readonly int XLimit;
    public readonly int YLimit;

    public int X
    {
        get => _x;
        set => SetX(value);
    }

    public int Y
    {
        get => _y;
        set => SetY(value);
    }

    public Cursor(int xLimit = 10, int yLimit = 10)
    {
        XLimit = xLimit;
        YLimit = yLimit;
    }

    public int GetX() => _x;
    public int GetY() => _y;

    public int SetX(int val)
    {
        while (val >= XLimit)
            val -= XLimit;

        while (val < 0)
            val += XLimit;

        _x = val;
        return _x;
    }
    public int SetY(int val)
    {
        while (val >= YLimit)
            val -= YLimit;

        while (val < 0)
            val += YLimit;

        _y = val;
        return _y;
    }

    public int GetLimitX() => XLimit;
    public int GetLimitY() => YLimit;
}