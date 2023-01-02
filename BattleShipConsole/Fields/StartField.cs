using BattleShipConsole.BoatData;
using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;
using System.Numerics;

namespace BattleShipConsole.Fields;

public class StartField : IField, IInput
{
    private readonly List<int> _boatSizes;
    private int _currentBoatIdx = 0;
    private PlaceableBoat? _currentBoat;
    private List<Boat> _placed = new();

    public int Width { get; }
    public int Height { get; }
    public ICursor Cursor => CurrentBoat;
    public InputState InputState { get; private set; }
    public List<int> BoatSizes => new(_boatSizes);

    public PlaceableBoat CurrentBoat
    {
        get
        {
            if (_currentBoat is not null)
                return _currentBoat;

            _currentBoat = new PlaceableBoat(this, _boatSizes[_currentBoatIdx], 0, 0);
            return _currentBoat;
        }
    }

    public int CurrentBoatIdx
    {
        get => _currentBoatIdx;
        set
        {
            if (value == _currentBoatIdx)
                return;

            if (value >= _boatSizes.Count || value < 0)
                return;

            PlaceableBoat previousBoat = CurrentBoat;

            _currentBoatIdx = value;
            _currentBoat = new PlaceableBoat(this, _boatSizes[value], previousBoat.Cords);
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
        _boatSizes = boatSizes;

        // TODO: remove this later it is just as a test
        _placed = new List<Boat>()
        {
            new Boat(this, 2, 4, 0),
        };
    }

    public override string ToString()
    {
        string buffer = "";
        Vector2 cursorCords = CurrentBoat.Cords;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2 walkCords = new Vector2(x, y);

                bool hasPart = CurrentBoat.Parts.Any(part => walkCords == part.Cords) ||
                               _placed.Any(boat => boat.Parts.Any(part => walkCords == part.Cords));
                string addition = walkCords == cursorCords ? "[" : " ";
                addition += hasPart ? '#' : '.';
                addition += walkCords == cursorCords ? "]" : " ";
                buffer += addition;
            }

            buffer += Environment.NewLine;
        }

        return buffer;
    }

    public void Display()
    {
        CurrentBoat.SetX(19);
        CurrentBoat.Rotate(Facing.East);
        Console.WriteLine($"Selected idx: {_currentBoatIdx}");
        Console.WriteLine(ToString());
        PlaceCurrent();
        // CurrentBoatIdx++;
        Console.WriteLine($"Selected idx: {_currentBoatIdx}");
        CurrentBoat.SetX(8);
        Console.WriteLine(ToString());
        PlaceCurrent();
        // CurrentBoatIdx++;
        Console.WriteLine($"Selected idx: {_currentBoatIdx}");
        CurrentBoat.SetY(1);
        CurrentBoat.SetX(0);
        
        Console.WriteLine(CurrentBoat.Cords);
        Console.WriteLine(CurrentBoat.GetLimitY());
        Console.WriteLine(CurrentBoat.GetLimitX());
        Console.WriteLine(ToString());
    }

    public (int xInput, int yInput) GetInput()
    {
        throw new NotImplementedException();
    }

    private void HandleInput()
    {
        throw new NotImplementedException();
    }

    public void PlaceCurrent()
    {
        int oldIdx = _currentBoatIdx;
        PlaceableBoat previousBoat = CurrentBoat;
        
        _boatSizes.RemoveAt(oldIdx);
        int newIdx = _currentBoatIdx > _boatSizes.Count - 1 ? _boatSizes.Count - 1 : _currentBoatIdx;
        _currentBoatIdx = newIdx;
        
        _placed.Add(CurrentBoat);
        _currentBoat = new PlaceableBoat(this, _boatSizes[_currentBoatIdx], previousBoat.Cords);
    }

    public void ResetInput()
    {
        InputState = InputState.NotStarted;
    }
}