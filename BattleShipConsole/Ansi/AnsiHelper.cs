using System.Runtime.InteropServices;

namespace BattleShipConsole.Ansi;

public static class AnsiHelper
{
    #region ANSI CODE ENABLE

#if _WINDOWS
    private const int StdOutputHandle = -11;
    private const int EnableVirtualTerminalProcessing = 0x0004;
    private const int DisableNewlineAutoReturn = 0x0008;

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern uint GetLastError();

#endif // _WINDOWS

    internal static void EnableAnsi()
    {
#if _WINDOWS
        var iStdOut = GetStdHandle(StdOutputHandle);
        if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
        {
            Console.WriteLine($"Failed to get output console mode, error-code: {GetLastError()}");
            Console.ReadKey();
            return;
        }

        outConsoleMode |= EnableVirtualTerminalProcessing;
        if (!SetConsoleMode(iStdOut, outConsoleMode))
        {
            Console.WriteLine($"Failed to set output console mode, error-code: {GetLastError()}");
            Console.ReadKey();
        }
#endif
    }

    #endregion
}