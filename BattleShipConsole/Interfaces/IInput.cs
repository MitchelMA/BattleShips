using BattleShipConsole.Enums;

namespace BattleShipConsole.Interfaces;

public interface IInput
{
    public ICursor GetCursor();
    public InputState GetInputState();
    public (int xInput, int yInput) GetInput();
    protected void HandleInput();
    public void ResetInput();
}