namespace YukaiLarkStateTransitionDiagram;

using System;
using System.Runtime.InteropServices;
using System.Text;

internal static class WindowsImeCompositionReader
{
    private const int GcsCompositionString = 0x0008;
    private const int NiCompositionString = 0x0015;
    private const int CpsCancel = 0x0004;

    public static string GetCompositionString()
    {
        var hwnd = GetActiveWindow();
        if (hwnd == IntPtr.Zero)
        {
            return string.Empty;
        }

        var inputContext = ImmGetContext(hwnd);
        if (inputContext == IntPtr.Zero)
        {
            return string.Empty;
        }

        try
        {
            var byteLength = ImmGetCompositionString(inputContext, GcsCompositionString, null, 0);
            if (byteLength <= 0)
            {
                return string.Empty;
            }

            var buffer = new byte[byteLength];
            var copied = ImmGetCompositionString(inputContext, GcsCompositionString, buffer, buffer.Length);
            return copied > 0 ? Encoding.Unicode.GetString(buffer, 0, copied) : string.Empty;
        }
        finally
        {
            _ = ImmReleaseContext(hwnd, inputContext);
        }
    }

    public static bool IsOpen()
    {
        var hwnd = GetActiveWindow();
        if (hwnd == IntPtr.Zero)
        {
            return false;
        }

        var inputContext = ImmGetContext(hwnd);
        if (inputContext == IntPtr.Zero)
        {
            return false;
        }

        try
        {
            return ImmGetOpenStatus(inputContext);
        }
        finally
        {
            _ = ImmReleaseContext(hwnd, inputContext);
        }
    }

    public static void SetOpen(bool isOpen)
    {
        var hwnd = GetActiveWindow();
        if (hwnd == IntPtr.Zero)
        {
            return;
        }

        var inputContext = ImmGetContext(hwnd);
        if (inputContext == IntPtr.Zero)
        {
            return;
        }

        try
        {
            if (!isOpen)
            {
                _ = ImmNotifyIME(inputContext, NiCompositionString, CpsCancel, 0);
            }

            _ = ImmSetOpenStatus(inputContext, isOpen);
        }
        finally
        {
            _ = ImmReleaseContext(hwnd, inputContext);
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("imm32.dll")]
    private static extern IntPtr ImmGetContext(IntPtr hwnd);

    [DllImport("imm32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ImmReleaseContext(IntPtr hwnd, IntPtr inputContext);

    [DllImport("imm32.dll", EntryPoint = "ImmGetCompositionStringW")]
    private static extern int ImmGetCompositionString(IntPtr inputContext, int index, byte[]? buffer, int bufferLength);

    [DllImport("imm32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ImmGetOpenStatus(IntPtr inputContext);

    [DllImport("imm32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ImmSetOpenStatus(IntPtr inputContext, bool isOpen);

    [DllImport("imm32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ImmNotifyIME(IntPtr inputContext, int action, int index, int value);
}
