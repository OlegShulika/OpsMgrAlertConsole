using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;  

namespace OpsMgrAlertConsole
{

class SendKeyX 
{
    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);


    [DllImport("user32", SetLastError = true, EntryPoint = "BroadcastSystemMessage")]
    private static extern int BroadcastSystemMessageRecipients(MessageBroadcastFlags dwFlags, ref MessageBroadcastRecipients lpdwRecipients, uint uiMessage, IntPtr wParam, IntPtr lParam);

    [DllImport("user32", SetLastError = true)]
    private static extern int BroadcastSystemMessage(MessageBroadcastFlags dwFlags, IntPtr lpdwRecipients, uint uiMessage, IntPtr wParam, IntPtr lParam);

    public const int WM_SETTEXT = 12;
    public const int WM_GETTEXT = 13; 

    public static Int32 KEYEVENTF_EXTENDEDKEY = 0x1;
    public static Int32 KEYEVENTF_KEYUP = 0x2;

    [Flags]
    enum MessageBroadcastFlags : uint
    {
        BSF_QUERY = 0x00000001,
        BSF_IGNORECURRENTTASK = 0x00000002,
        BSF_FLUSHDISK = 0x00000004,
        BSF_NOHANG = 0x00000008,
        BSF_POSTMESSAGE = 0x00000010,
        BSF_FORCEIFHUNG = 0x00000020,
        BSF_NOTIMEOUTIFNOTHUNG = 0x00000040,
        BSF_ALLOWSFW = 0x00000080,
        BSF_SENDNOTIFYMESSAGE = 0x00000100,
        BSF_RETURNHDESK = 0x00000200,
        BSF_LUID = 0x00000400,
    }

    [Flags]
    enum MessageBroadcastRecipients : uint
    {
        BSM_ALLCOMPONENTS = 0x00000000,
        BSM_VXDS = 0x00000001,
        BSM_NETDRIVER = 0x00000002,
        BSM_INSTALLABLEDRIVERS = 0x00000004,
        BSM_APPLICATIONS = 0x00000008,
        BSM_ALLDESKTOPS = 0x00000010,
    }

    private static Int32 WM_KEYDOWN = 0x100; 
    //private static Int32 WM_KEYUP = 0x101;  
    //private static Int32 WM_CHAR = 0x102; 
    //private static Int32 WM_DEADCHAR = 0x103; 
    private static Int32 WM_SYSKEYDOWN = 0x104;
    private static Int32 WM_SYSKEYUP = 0x105;
    //private static Int32 WM_SYSCHAR = 0x106;
    //private static Int32 WM_SYSDEADCHAR = 0x107;
  
    [return: MarshalAs(UnmanagedType.Bool)]    
    [DllImport("user32.dll", SetLastError = true)] 
    static extern bool PostMessage(IntPtr hWnd, int Msg, System.Windows.Forms.Keys wParam, int lParam);    
    
    [DllImport("user32.dll", SetLastError = true)] 
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetShellWindow();


    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, StringBuilder lParam);
    //public static extern int SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);


    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    [DllImport("user32.dll")]
    public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId(); 

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetFocus();

    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(IntPtr hWnd);

    public enum GetWindow_Cmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);


    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

    public static IntPtr FindWindow(string windowName) 
    { 
        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses()) 
        { 
            if (p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.ToLower() == windowName.ToLower())                
                return p.MainWindowHandle; 
        } 
        return IntPtr.Zero; 
    } 
    
    public static IntPtr FindWindow(IntPtr parent, string childClassName) 
    { 
        return FindWindowEx(parent, IntPtr.Zero, childClassName, string.Empty); 
    } 
    
    public static void SendKey(IntPtr hWnd, System.Windows.Forms.Keys key) 
    { 
        PostMessage(hWnd, WM_KEYDOWN, key, 0); 
    }

    public static void SendAltTAB(IntPtr hWnd)
    {
        PostMessage(hWnd, WM_SYSKEYDOWN, System.Windows.Forms.Keys.Tab, 0);
        PostMessage(hWnd, WM_SYSKEYUP, System.Windows.Forms.Keys.Tab, 0);
    } 


}

}

