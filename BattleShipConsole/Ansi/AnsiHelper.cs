using System.Runtime.InteropServices;

namespace BattleShipConsole.Ansi;

public static class AnsiHelper
{
    #region Ansi Escape Codes
    // https://en.wikipedia.org/wiki/ANSI_escape_code
    
    /// Cursor Up (Moves the cursor up by `n`)
    public static string AnsiCuu(int n) => $"\x1b[{n}A";
    /// Cursor Down (Moves the cursor down by `n`)
    public static string AnsiCud(int n) => $"\x1b[{n}B";
    /// Cursor Forwards (Moves the cursor forward by `n`)
    public static string AnsiCuf(int n) => $"\x1b[{n}C";
    /// Cursor Back (Moves the cursor back by `n`)
    public static string AnsiCub(int n) => $"\x1b[{n}D";
    /// Cursor Next Line (Moves cursor to beginning of the line `n` lines down)
    public static string AnsiCnl(int n) => $"\x1b[{n}E";
    /// Cursor Previous Line (Moves cursor to beginning of the line `n` lines up)
    public static string AnsiCpl(int n) => $"\x1b[{n}F";
    /// Cursor Horizontal Absolute (Moves cursor to column `n`)
    public static string AnsiCha(int n) => $"\x1b[{n}G";
    /// Cursor Position (Moves cursor row `n`; column `m`. Values are 1-based)
    public static string AnsiCup(int n, int m) => $"\x1b[{n};{m}H";
    /// Erase in Display (0: clear from cursor to end; 1: cursor to beginning of screen; 2: entire screen)
    public static string AnsiEd(int n) => $"\x1b[{n}J";
    /// Erase in Line (0: from cursor to end; 1: from cursor to beginning; 2: entire line)
    public static string AnsiEl(int n) => $"\x1b[{n}K";
    /// Scroll Up (Scroll up by `n` lines)
    public static string AnsiSu(int n) => $"\x1b[{n}S";
    /// Scroll Down (Scroll down by `n` lines)
    public static string AnsiSd(int n) => $"\x1b[{n}T";
    /// Horizontal Vertical Position (Same as CUP, but counts as a format effector function)
    public static string AnsiHvp(int n, int m) => $"\x1b[{n};{m}f";
    
    public const string AnsiReset = "\x1b[0m";
    public const string AnsiDim = "\x1b[2m";
    public const string AnsiItalic = "\x1b[3m";
    public const string AnsiUnderline = "\x1b[4m";
    public const string AnsiSlowBlink = "\x1b[5m";
    public const string AnsiFastBlink = "\x1b[6m";
    public const string AnsiStrike = "\x1b[9m";
    public const string AnsiDUnderlineNoBold = "\x1b[21m";
    public const string AnsiNormalIntensity = "\x1b[22m";
    public const string AnsiNItalicNBlackLetter = "\x1b[23m";
    public const string AnsiNoUnderline = "\x1b[24m";
    public const string AnsiNoBlinking = "\x1b[25m";
    public const string AnsiNoStrike = "\x1b[29m";
    public const string AnsiOverLined = "\x1b[53m";
    public const string AnsiNoOverLined = "\x1b[55m";

    public const string AnsiForeBlack = "\x1b[30m";
    public const string AnsiForeRed = "\x1b[31m";
    public const string AnsiForeGreen = "\x1b[32m";
    public const string AnsiForeYellow = "\x1b[33m";
    public const string AnsiForeBlue = "\x1b[34m";
    public const string AnsiForePurple = "\x1b[35m";
    public const string AnsiForeCyan = "\x1b[36m";
    public const string AnsiForeWhite = "\x1b[37m";
    public static string SetForeColor(byte r, byte g, byte b) => $"\x1b[38;2;{r};{g};{b}m";
    public static string SetForeColor(byte n) => $"\x1b[38;5;{n}m";
    public const string AnsiDefaultFore = "\x1b[39m";

    public const string AnsiBForeBlack = "\x1b[90m";
    public const string AnsiBForeRed = "\x1b[91m";
    public const string AnsiBForeGreen = "\x1b[92m";
    public const string AnsiBForeYellow = "\x1b[93m";
    public const string AnsiBForeBlue = "\x1b[94m";
    public const string AnsiBForePurple = "\x1b[95m";
    public const string AnsiBForeCyan = "\x1b[96m";
    public const string AnsiBForeWhite = "\x1b[97m";

    public const string AnsiBackBlack = "\x1b[40m";
    public const string AnsiBackRed = "\x1b[41m";
    public const string AnsiBackGreen = "\x1b[42m";
    public const string AnsiBackYellow = "\x1b[43m";
    public const string AnsiBackBlue = "\x1b[44m";
    public const string AnsiBackPurple = "\x1b[45m";
    public const string AnsiBackCyan = "\x1b[46m";
    public const string AnsiBackWhite = "\x1b[47m";
    
    public static string SetBackColor(byte r, byte g, byte b) => $"\x1b[48;2;{r};{g};{b}m";
    public static string SetBackColor(byte n) => $"\x1b[48;5;{n}m";
    public const string AnsiDefaultBack = "\x1b[49m";

    public const string AnsiBBackBlack = "\x1b[100m";
    public const string AnsiBBackRed = "\x1b[101m";
    public const string AnsiBBackGreen = "\x1b[102m";
    public const string AnsiBBackYellow = "\x1b[103m";
    public const string AnsiBBackBlue = "\x1b[104m";
    public const string AnsiBBackPurple = "\x1b[105m";
    public const string AnsiBBackCyan = "\x1b[106m";
    public const string AnsiBBackWhite = "\x1b[107m";

    #endregion
    
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