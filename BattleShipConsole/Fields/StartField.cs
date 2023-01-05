using BattleShipConsole.BoatData;
using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;
using System.Numerics;

namespace BattleShipConsole.Fields;

public class StartField : IField, IInput<SelectType>
{
    private readonly List<int> _boatSizes;
    private int _currentBoatIdx = 0;
    private PlaceableBoat? _currentBoat;
    private readonly List<Boat> _placed;
    private SelectType _lastSelect = SelectType.NoObject;

    public int Width { get; }
    public int Height { get; }
    public ICursor Cursor => CurrentBoat;
    public InputState InputState { get; private set; } = InputState.Initialized;
    public IReadOnlyList<Boat> BoatSizes => _placed.AsReadOnly();

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

                bool isOverlapping = CurrentBoat.Parts.Any(part => walkCords == part.Cords) &&
                                     _placed.Any(boat => boat.Parts.Any(part => walkCords == part.Cords));

                string addition = walkCords == cursorCords ? "[" : " ";
                _ = isOverlapping ? addition += "\u001b[31m" : "";
                addition += hasPart ? '#' : '.';
                _ = isOverlapping ? addition += "\u001b[m" : "";
                addition += walkCords == cursorCords ? "]" : " ";
                buffer += addition;
            }

            buffer += Environment.NewLine;
        }

        return buffer;
    }

    public void Display()
    {
        Console.WriteLine(ToString());
    }

    public ((int xInput, int yInput), SelectType selectType) GetInput()
    {
        InputState = InputState.Running;

        var startPos = Console.GetCursorPosition();

        while (InputState == InputState.Running)
        {
            Display();
            HandleInput();
            Console.SetCursorPosition(startPos.Left, startPos.Top);
        }
        
        FlushInput();

        return ((Cursor.GetX(), Cursor.GetY()), _lastSelect);
    }

    private void HandleInput()
    {
        ConsoleKey input = Console.ReadKey(true).Key;
        switch (input)
        {
            case ConsoleKey.UpArrow:
            {
                Cursor.SetY(Cursor.GetY() - 1);
            }
                break;
            case ConsoleKey.RightArrow:
            {
                Cursor.SetX(Cursor.GetX() + 1);
            }
                break;
            case ConsoleKey.DownArrow:
            {
                Cursor.SetY(Cursor.GetY() + 1);
            }
                break;
            case ConsoleKey.LeftArrow:
            {
                Cursor.SetX(Cursor.GetX() - 1);
            }
                break;
            case ConsoleKey.R:
            {
                int curRot = (int) CurrentBoat.Direction;
                int nextRot = (curRot + 1) % 4;

                CurrentBoat.Rotate((Facing) nextRot);
            }
                break;
            case ConsoleKey.OemPeriod:
            {
                CurrentBoatIdx++;
            }
                break;
            case ConsoleKey.OemComma:
            {
                CurrentBoatIdx--;
            }
                break;

            case ConsoleKey.Enter:
            {
                InputState = InputState.Done;
                _lastSelect = SelectType.Primary | SelectType.Object;
            }
                break;

            default:
                break;
        }
    }

    private void FlushInput()
    {
        while (Console.KeyAvailable)
            Console.ReadKey(true);
    }
    
    public void PlaceCurrent()
    {
        int oldIdx = _currentBoatIdx;
        PlaceableBoat previousBoat = CurrentBoat;

        _boatSizes.RemoveAt(oldIdx);
        int newIdx = _currentBoatIdx > _boatSizes.Count - 1 ? _boatSizes.Count - 1 : _currentBoatIdx;
        _currentBoatIdx = newIdx;

        _placed.Add(CurrentBoat);

        if (_currentBoatIdx < 0)
            return;

        _currentBoat = new PlaceableBoat(this, _boatSizes[_currentBoatIdx], previousBoat.Cords);
    }

    public void ResetInput()
    {
        InputState = InputState.Initialized;
    }
}