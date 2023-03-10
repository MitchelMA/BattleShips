using System.Drawing;
using BattleShipConsole.BoatData;
using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;
using System.Numerics;
using BattleShipConsole.Ansi;

namespace BattleShipConsole.Fields;

public class StartField : IField, IInput<SelectType>
{
    private readonly List<int> _boatSizes;
    private int _currentBoatIdx = 0;
    private PlaceableBoat? _currentBoat;
    private readonly List<Boat> _placed = new();
    private SelectType _lastSelect = SelectType.NoObject;

    public int Width { get; }
    public int Height { get; }
    public ICursor Cursor => CurrentBoat;
    public InputState InputState { get; private set; } = InputState.Initialized;
    public IReadOnlyList<int> BoatSizes => _boatSizes.AsReadOnly();

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

    // Checks if any part of CurrentBoat is at the same position of any previously placed boats
    public bool IsOverlapping => CurrentBoat.Parts.Any(part =>
        _placed.Any(boat => boat.Parts.Any(boatPart => boatPart.Cords == part.Cords)));

    public int GetWidth() => Width;
    public int GetHeight() => Height;
    public ICursor GetCursor() => Cursor;
    public InputState GetInputState() => InputState;

    public IReadOnlyList<Boat> Placed => _placed.AsReadOnly();

    public StartField(int[] boatSizes, int width, int height)
    {
        Width = width;
        Height = height;
        _boatSizes = boatSizes.ToList();
    }

    public override string ToString()
    {
        string buffer = AnsiHelper.AnsiForeBlue;
        Point cursorCords = CurrentBoat.Cords;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Point walkCords = new Point(x, y);

                // Check if this coordinate has a part
                bool hasPart = CurrentBoat.Parts.Any(part => walkCords == part.Cords) ||
                               _placed.Any(boat => boat.Parts.Any(part => walkCords == part.Cords));

                // When any parts of the current boat (the one being placed) overlaps with any part of an already
                // placed boat -> true
                bool isOverlapping = CurrentBoat.Parts.Any(part => walkCords == part.Cords) &&
                                     _placed.Any(boat => boat.Parts.Any(part => walkCords == part.Cords));

                // "cursor open" when cursor coords and walk coords are equal
                string addition = walkCords == cursorCords ? AnsiHelper.AnsiDefaultFore + '[' : " ";
                if (isOverlapping)
                {
                    addition += AnsiHelper.AnsiBForeRed;
                }

                if (hasPart)
                {
                    if (!isOverlapping)
                        addition += AnsiHelper.AnsiDefaultFore;
                    
                    addition += '#' + AnsiHelper.AnsiDefaultFore;
                }
                else
                {
                    addition += '·';
                }

                addition += walkCords == cursorCords ? ']' : " ";
                addition += AnsiHelper.AnsiForeBlue;
                buffer += addition;
            }

            buffer += Environment.NewLine;
        }

        buffer += AnsiHelper.AnsiReset;
        return buffer;
    }

    public void Display()
    {
        Console.Write(ToString());
    }

    public ((int xInput, int yInput), SelectType selectType) GetInput()
    {
        if (InputState is InputState.Done)
            return ((Cursor.GetX(), Cursor.GetY()), _lastSelect);
        
        InputState = InputState.Running;
        
        // save the start cursor position
        Console.Write(AnsiHelper.AnsiSaveCursorPos);

        while (InputState == InputState.Running)
        {
            Display();
            HandleInput();
            // restore the start position
            Console.Write(AnsiHelper.AnsiRestoreCursorPos);
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
        if (IsOverlapping)
            return;
        
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