using BattleShipConsole.BoatData;
using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;
using System.Numerics;

namespace BattleShipConsole.Fields;

public class StartField : IField, IInput
{
    private readonly List<int> _boatSizes;
    private int _currentBoatIdx = 0;
    private Boat? _currentBoat;
    private List<Boat> _placed = new();

    public int Width { get; }
    public int Height { get; }
    public Cursor Cursor { get; }
    public InputState InputState { get; private set; }
    public List<int> BoatSizes => new(_boatSizes);

    public Boat CurrentBoat
    {
        get
        {
            if (_currentBoat is not null)
                return _currentBoat;

            _currentBoat = new Boat(this, _boatSizes[_currentBoatIdx], Cursor.X, Cursor.Y);
            return _currentBoat;
        }
    }

    public int CurrentBoatIdx
    {
        get => _currentBoatIdx;
        set
        {
            if (value >= _boatSizes.Count || value < 0)
                return;

            _currentBoatIdx = value;
            _currentBoat = new Boat(this, _boatSizes[value], Cursor.X, Cursor.Y);
        }
    }

    public int GetWidth() => Width;
    public int GetHeight() => Height;
    public ICursor GetCursor() => Cursor;
    public InputState GetInputState() => InputState;

    public List<Boat> Placed => new(_placed);

    public StartField(List<int> boatSizes, int width, int height)
    {
        Width = width;
        Height = height;
        Cursor = new Cursor(width, height);
        _boatSizes = boatSizes;
        
        // TODO: remove this later it is just as a test
        _placed = new List<Boat>()
        {
            new Boat(this, 2, 5, 1),
        };
    }

    public override string ToString()
    {
        string buffer = "";
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2 walkCords = new Vector2(x, y);

                bool hasPart = CurrentBoat.Parts.Any(part => walkCords == part.Cords) || _placed.Any(boat => boat.Parts.Any(part => walkCords == part.Cords));
                
                char ch = hasPart ? '#' : '.';
                buffer += ch;
            }

            buffer += Environment.NewLine;
        }

        return buffer;
    }

    public void Display()
    {
        Console.WriteLine(ToString());
    }

    public (int xInput, int yInput) GetInput()
    {
        throw new NotImplementedException();
    }

    public void HandleInput()
    {
        throw new NotImplementedException();
    }

    public void ResetInput()
    {
        InputState = InputState.NotStarted;
    }
}