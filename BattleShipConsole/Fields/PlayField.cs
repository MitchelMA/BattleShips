using BattleShipConsole.Enums;
using BattleShipConsole.Interfaces;

namespace BattleShipConsole.Fields;

public class PlayField : IField, IInput
{
    public int Width { get; }
    public int Height { get; }
    public Cursor Cursor { get; }
    public InputState InputState { get; private set; }
    public int GetWidth() => Width;
    public int GetHeight() => Height;
    public ICursor GetCursor() => Cursor;
    public InputState GetInputState() => InputState;

    public PlayField(int width, int height)
    {
        Width = width;
        Height = height;

        Cursor = new Cursor(Width, Height);
    }
    

    public void Display()
    {
        throw new NotImplementedException();
    }


    public (int xInput, int yInput) GetInput()
    {
        throw new NotImplementedException();
    }

    public void ResetInput()
    {
        InputState = InputState.NotStarted;
    }
    
    private void HandleInput()
    {
        throw new NotImplementedException();
    }
}