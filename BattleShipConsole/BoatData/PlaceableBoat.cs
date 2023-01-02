using System.Numerics;
using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;

namespace BattleShipConsole.BoatData;

public class PlaceableBoat : Boat, ICursor
{
    public int X
    {
        get => GetX();
        set => SetX(value);
    }

    public int Y
    {
        get => GetY();
        set => SetY(value);
    }

    public PlaceableBoat(IField field, int length, int x, int y) : base(field, length, x, y)
    {
        // setting the x and y again to make sure that the boat stays in bounds
        X = x;
        Y = y;
    }

    public PlaceableBoat(IField field, int length, Vector2 cords) : base(field, length, cords)
    {
        // setting the x and y again to make sure that the boat stays in bounds
        X = (int) cords.X;
        Y = (int) cords.Y;
    }

    public int GetX() => (int) Cords.X;
    public int GetY() => (int) Cords.Y;

    public int SetX(int val)
    {
        int lim = GetLimitX();
        while (val >= lim)
            val -= Direction is Facing.East ? Field.GetWidth() - (Length - 1) : lim;

        int offset = Direction == Facing.East ? Length - 1 : 0;

        while (val < offset)
            val += Direction is Facing.East ? Field.GetWidth() - (Length - 1) : lim;

        Cords = Cords with {X = val};
        foreach (var part in Parts)
            part.SetPosition();

        return val;
    }

    public int SetY(int val)
    {
        int lim = GetLimitY();
        while (val >= lim)
            val -= Direction is Facing.South ? Field.GetHeight() - (Length - 1) : lim;

        int offset = Direction == Facing.South ? Length - 1 : 0;

        while (val < offset)
            val += Direction is Facing.South ? Field.GetHeight() - (Length - 1) : lim;

        Cords = Cords with {Y = val};
        foreach (var part in Parts)
            part.SetPosition();

        return val;
    }

    public int GetLimitX()
    {
        if (Direction is Facing.West)
            return Field.GetWidth() - (Length - 1);
        return Field.GetWidth();
    }

    public int GetLimitY()
    {
        if (Direction is Facing.North)
            return Field.GetHeight() - (Length - 1);
        return Field.GetHeight();
    }
}