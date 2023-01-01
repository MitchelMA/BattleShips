namespace BattleShipConsole.Interfaces;

public interface ICursor
{
    public int GetX();
    public int GetY();

    public int SetX(int val);
    public int SetY(int val);

    public int GetLimitX();
    public int GetLimitY();
}