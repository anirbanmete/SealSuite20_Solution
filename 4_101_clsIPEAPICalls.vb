
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsAPICalls                            '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  20MAR07                                '
'                                                                              '
'===============================================================================

Imports System
Imports System.Drawing.Printing
Imports System.Drawing.Graphics
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class IPE_clsAPICalls

    Public Const DIB_RGB_COLORS As Integer = 0
    Public Const BI_RGB As Integer = 0
    Public Const WHITENESS As Integer = 16711778

    <DllImport("user32.dll", EntryPoint:="PrintWindow", _
    SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function PrintWindow(ByVal hWnd As IntPtr, ByVal hDC As IntPtr, _
                                                ByVal dwFlags As Integer) As UInt32
    End Function

    <StructLayout(LayoutKind.Sequential, pack:=8, CharSet:=CharSet.Auto)> _
    Structure BITMAPINFOHEADER
        Dim biSize As Int32
        Dim biWidth As Int32
        Dim biHeight As Int32
        Dim biPlanes As Int16
        Dim biBitCount As Int16
        Dim biCompression As Int32
        Dim biSizeImage As Int32
        Dim biXPelsPerMeter As Int32
        Dim biYPelsPerMeter As Int32
        Dim biClrUsed As Int32
        Dim biClrImportant As Int32
    End Structure

    <DllImport("gdi32.dll", EntryPoint:="CreateDIBSection", _
    SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function CreateDIBSection(ByVal hdc As IntPtr, ByRef pbmi As BITMAPINFOHEADER, _
    ByVal iUsage As Int32, ByVal ppvBits As IntPtr, ByVal hSection As IntPtr, _
    ByVal dwOffset As Int32) As IntPtr
    End Function

    <DllImport("gdi32.dll", EntryPoint:="PatBlt", _
    SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function PatBlt(ByVal hDC As IntPtr, ByVal nXLeft As Int32, _
        ByVal nYLeft As Int32, ByVal nWidth As Int32, ByVal nHeight As Int32, _
        ByVal dwRop As Int32) As Boolean
    End Function

    <DllImport("gdi32.dll", EntryPoint:="SelectObject", _
    SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function SelectObject(ByVal hDC As IntPtr, ByVal hObj As IntPtr) As IntPtr
    End Function

    <DllImport("GDI32.dll", EntryPoint:="CreateCompatibleDC", SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function CreateCompatibleDC(ByVal hRefDC As IntPtr) As IntPtr
    End Function

    <DllImport("GDI32.dll", EntryPoint:="DeleteDC", SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function DeleteDC(ByVal hDC As IntPtr) As Boolean
    End Function

    <DllImport("GDI32.dll", EntryPoint:="DeleteObject", SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function DeleteObject(ByVal hObj As IntPtr) As Boolean
    End Function

    <DllImport("User32.dll", EntryPoint:="ReleaseDC", SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function ReleaseDC(ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As Boolean
    End Function

    <DllImport("User32.dll", EntryPoint:="GetDC", SetLastError:=True, CharSet:=CharSet.Unicode, _
    ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function GetDC(ByVal hWnd As IntPtr) As IntPtr
    End Function

End Class
