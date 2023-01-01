using BattleShipConsole.Enums;

namespace BattleShipConsole.BoatData;

public class BoatPart
{
    public readonly Boat From;
    public readonly int Idx;

    public int X;
    public int Y;
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
        switch (From.Direction)
        {
            case Facing.North:
            {
                X = From.X;
                Y = From.Y + Idx;
            }
                break;
            case Facing.East:
            {
                X = From.X - Idx;
                Y = From.Y;
            }
                break;
            case Facing.South:
            {
                X = From.X;
                Y = From.Y - Idx;
            }
                break;
            case Facing.West:
            {
                X = From.X + Idx;
                Y = From.Y;
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return X >= 0 && X < From.Field.GetWidth() && Y >= 0 && Y < From.Field.GetHeight();
    }
}