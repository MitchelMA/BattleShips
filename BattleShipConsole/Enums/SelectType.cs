namespace BattleShipConsole.Enums;

[Flags]
public enum SelectType
{
    NoObject = 1,  // 0b0001
    Object = 2,    // 0b0010
    Primary = 4,   // 0b0100
    Secondary = 8, // 0b1000
}