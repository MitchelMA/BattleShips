using System.Runtime.InteropServices;
using BattleShipConsole.Ansi;
using BattleShipConsole.BoatData;
using BattleShipConsole.Fields;

namespace BattleShipConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        AnsiHelper.EnableAnsi();

        StartField field = new StartField(new() {4, 8, 2}, 20, 10);
        // field.GetInput();
        Console.Write(AnsiHelper.SetForeColor(50, 50, 200));
        
        // for(int i = 0; i < 10; i++)
        Console.WriteLine("Place your boats" + AnsiHelper.AnsiReset);
        while (field.BoatSizes.Count > 0)
        {
            field.GetInput();
            field.PlaceCurrent();
            field.ResetInput();
        }
        Console.Write(AnsiHelper.AnsiCnl(field.Height));
    }
}