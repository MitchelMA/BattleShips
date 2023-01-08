using System.Runtime.InteropServices;
using BattleShipConsole.Ansi;
using BattleShipConsole.Fields;

namespace BattleShipConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        int input = int.Parse(args[0]);
        AnsiHelper.EnableAnsi();

        StartField field = new StartField(new() {4, 8, 2}, 20, 10);
        // field.GetInput();
        Console.Write(
            AnsiHelper.SetForeColor(255, 100, 100) + 
            AnsiHelper.AnsiBBackCyan + 
            AnsiHelper.AnsiSlowBlink);
        
        // for(int i = 0; i < 10; i++)
        Console.WriteLine("Prachtig, niet?" + AnsiHelper.AnsiReset);
        while (field.BoatSizes.Count > 0)
        {
            field.GetInput();
            field.PlaceCurrent();
        }
        Console.Write(AnsiHelper.AnsiCud(field.Height));
    }
}