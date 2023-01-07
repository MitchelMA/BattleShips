using System.Runtime.InteropServices;
using BattleShipConsole.Ansi;
using BattleShipConsole.Fields;

namespace BattleShipConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        AnsiHelper.EnableAnsi();

        StartField field = new StartField(new() {4, 8, 2}, 20, 10);
        // field.GetInput();
        Console.WriteLine("Prachtig, niet?");
        while (field.BoatSizes.Count > 0)
        {
            field.GetInput();
            field.PlaceCurrent();
        }
    }
}