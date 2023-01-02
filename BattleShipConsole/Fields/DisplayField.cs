using BattleShipConsole.Interfaces;

namespace BattleShipConsole.Fields;

public class DisplayField : IField
{
    private readonly int _width;
    private readonly int _height;

    public int Width => _width;
    public int Height => _height;

    public int GetWidth() => _width;
    public int GetHeight() => _height;

    public DisplayField(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public void Display()
    {
        throw new NotImplementedException();
    }
}