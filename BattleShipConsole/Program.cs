using System.Drawing;
using System.Runtime.InteropServices;
using BattleShipConsole.Ansi;
using BattleShipConsole.BoatData;
using BattleShipConsole.Fields;

namespace BattleShipConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "BattleShips";
        // enable the ansi escape sequences
        AnsiHelper.EnableAnsi();
        // catch SIGINT to handle exiting the alternative buffer etc..
        Console.CancelKeyPress += OnSigInt;
        
        // enter the alternative buffer for the game to be rendered into
        Console.Write(AnsiHelper.AnsiEnterAltBuffer);
        // hide the cursor
        Console.Write(AnsiHelper.AnsiHideCursor);
        // set the cursor position to top-left corner
        Console.Write(AnsiHelper.AnsiHvp(1, 1));
        // set the color of the title
        Console.Write(AnsiHelper.SetForeColor(50, 50, 200));
        Console.WriteLine("Place your boats" + AnsiHelper.AnsiReset);
        
        // create a field
        StartField field = new StartField(new() {4, 8, 2}, 20, 10);
        
        // loop of start field
        while (field.BoatSizes.Count > 0)
        {
            field.GetInput();
            field.PlaceCurrent();
            field.ResetInput();
        }
        
        // move the cursor down by the height of the field
        Console.Write(AnsiHelper.AnsiCnl(field.Height));
        // show the cursor
        Console.Write(AnsiHelper.AnsiShowCursor);
        // exit the alternative buffer
        Console.Write(AnsiHelper.AnsiExitAltBuffer);
        // thank the player for playing in the 'standard' terminal buffer
        Console.WriteLine("Thanks for playing!");
    }

    private static void OnSigInt(object? sender, ConsoleCancelEventArgs e)
    {
        Console.Write(AnsiHelper.AnsiShowCursor);
        Console.Write(AnsiHelper.AnsiReset);
        Console.Write(AnsiHelper.AnsiExitAltBuffer);
        Console.WriteLine("Thanks for playing!");
    }
}