using System.Drawing;
using System.Numerics;
using BattleShipConsole.Enums;

namespace BattleShipConsole.BoatData;

public class BoatPart
{
    public readonly Boat From;
    public readonly int Idx;

    public Point Cords { get; private set; } = new(0, 0);
    public readonly bool IsHead;

    public BoatPart(Boat from, int idx)
    {
        From = from;
        Idx = idx;
        IsHead = Idx == 0;
        SetPosition();
    }

    public bool SetPosition()
    {
        Cords = From.Direction switch
        {
            Facing.North => From.Cords with {Y = From.Cords.Y + Idx},
            Facing.East => From.Cords with {X = From.Cords.X - Idx},
            Facing.South => From.Cords with {Y = From.Cords.Y - Idx},
            Facing.West => From.Cords with {X = From.Cords.X + Idx},
            _ => throw new ArgumentOutOfRangeException()
        };

        return Cords.X >= 0 && Cords.X < From.Field.GetWidth() && Cords.Y >= 0 && Cords.Y < From.Field.GetHeight();
    }
}