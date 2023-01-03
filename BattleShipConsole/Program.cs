using System.Runtime.InteropServices;
using BattleShipConsole.Fields;

namespace BattleShipConsole;

public static class Program
{
#if _WINDOWS

    #region ANSI CODE ENABLE

    private const int STD_OUTPUT_HANDLE = -11;
    private const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    private const int DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern uint GetLastError();

    private static void EnableAnsi()
    {
        var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
        if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
        {
            Console.WriteLine($"Failed to get output console mode, error-code: {GetLastError()}");
            Console.ReadKey();
            return;
        }

        outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        if (!SetConsoleMode(iStdOut, outConsoleMode))
        {
            Console.WriteLine($"Failed to set output console mode, error-code: {GetLastError()}");
            Console.ReadKey();
            return;
        }
    }

    #endregion

#endif // _WINDOWS

    public static void Main(string[] args)
    {
#if _WINDOWS
        EnableAnsi();
#endif // _WINDOWS

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