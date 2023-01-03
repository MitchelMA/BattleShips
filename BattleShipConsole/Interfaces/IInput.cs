using BattleShipConsole.Enums;

namespace BattleShipConsole.Interfaces;

public interface IInput<T>
    where T : Enum
{
    public ICursor GetCursor();
    public InputState GetInputState();
    public ((int xInput, int yInput), T selectType) GetInput();
    public void ResetInput();
}