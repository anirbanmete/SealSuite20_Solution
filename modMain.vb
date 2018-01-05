'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealSuite"                            '
'                      FORM MODULE   :  modMain                                '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  10NOV17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb

Module modMain

#Region "GLOBAL CONSTANTS:"

    Public gcEPS As Single = 0.00001
    Public gDBConnectionState As Boolean = False

    '....Named Constants:
    Public Const gcSngLogoAspectRatio As Single = 2.8   '....Parker Hannifin logo
#End Region

#Region "INPUT DATA SET:"

    Public gUser As New clsUser
    Public gFile As New clsFile()

    Public gPart_frmMain As New frmPartMain("PN")

#End Region

#Region "UTILITY ROUTINES:"

    Public Sub LoadImageLogo(ByVal imgControl As PictureBox)
        '=================================================== 

        Try
            imgControl.Image = Image.FromFile(gIPE_File.Logo)

        Catch pEXP As IO.FileNotFoundException
            MessageBox.Show(pEXP.Message, "File Not Found", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
        End Try

    End Sub


    Public Function NumericDataValidation(ByVal Value_In As String) As Boolean
        '====================================================================== 
        Dim pVal As Boolean
        pVal = System.Text.RegularExpressions.Regex.IsMatch(Value_In, "^[-+]?[0-9]*\.?[0-9]+")

        Return pVal
    End Function


    Public Function MID_String(ByRef strVal As String, ByVal startVal As Integer, ByVal lengthVal As Integer) As String
        '=======================================================================================================
        Dim pVal As String = Mid(strVal, startVal, lengthVal)
        Return pVal
    End Function


    Public Sub GetOSVersion()
        '======================                               
        Dim pOS As System.OperatingSystem = System.Environment.OSVersion

        Dim pPlatform As String = pOS.Platform.ToString()
        Dim pMajor As Integer = pOS.Version.Major
        Dim pMinor As Integer = pOS.Version.Minor
        Dim pBit As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")

        Select Case pPlatform

            Case "Win32NT"
                '---------
                Select Case pMajor

                    Case 5
                        Select Case pMinor

                            Case 0
                                gOS = "Win2000"
                            Case 1
                                gOS = "WinXP"
                                gOSBit = IIf((String.IsNullOrEmpty(pBit) Or
                                              String.Compare(pBit, 0, "x86", 0, 3, True) = 0), 32, 64)
                            Case 2
                                gOS = "Win2003"

                        End Select

                    Case 6
                        Select Case pMinor

                            Case 0
                                gOS = "Vista/Win2008Server"
                            Case 1
                                gOS = "Win7"
                                gOSBit = GetOS_Bit()
                        End Select

                End Select

        End Select

    End Sub


    Private Function GetOS_Bit() As Integer
        '=================================  
        If Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Hardware\Description\System\CentralProcessor\0").GetValue("Identifier").ToString.Contains("x86") Then : Return 32
        Else : Return 64 : End If
    End Function


#End Region


#Region "GRAPHICS ROUTINES:"

    Public Sub Set_Margin_PicBox(ByVal UniformMargin_In As Single, _
                                 ByRef Margin_Out() As Single)
        '======================================================
        '....Picture Box Margins Calculation
        'Const pcUniformMargin As Single = 0.5   '....Uniform margin around the
        '                                       '........Picture Box - (in)

        Dim psngMargin As Single
        psngMargin = gIPE_Unit.EngLToUserL(UniformMargin_In) '....In user unit (in or mm)

        'psngMargin = gIPE_Unit.EngLToUserL(pcUniformMargin) '....In user unit (in or mm)

        '....Margins around the graphics in the picture box.
        Margin_Out(1) = psngMargin                 '....Left
        Margin_Out(2) = psngMargin                 '....Right

        '....The margin at the top is 60% of the total height margin and
        '....at the bottom is the rest 40%.
        Margin_Out(3) = 2 * psngMargin * 0.6       '....Top
        Margin_Out(4) = 2 * psngMargin * 0.4       '....Bottom

    End Sub


    Public Function GetGraphicsObj(ByVal picBox As PictureBox) As Graphics
        '==================================================================
        Dim pBmap As Bitmap
        pBmap = New Bitmap(picBox.Width, picBox.Height)
        picBox.Image = pBmap

        Dim pGr As Graphics
        pGr = Graphics.FromImage(pBmap)
        pGr.Clear(picBox.BackColor)

        Return pGr

    End Function

#End Region


#Region "CULTURE RELETED ROUTINES:"
    '******************************

    Public Function ConvertToInt(ByVal Str_In As String) As Int16
        '==========================================================
        Dim pVal As Int16 = 0
        If Str_In <> "" Then
            pVal = Convert.ToInt16(Str_In, CultureInfo.CurrentCulture)
        End If
        Return pVal
    End Function


    Public Function ConvertToSng(ByVal Str_In As String) As Single
        '==========================================================
        Dim pVal As Single = 0
        Try

            If Str_In <> "" Then
                pVal = Convert.ToSingle(Str_In, CultureInfo.CurrentCulture)

            End If
        Catch ex As Exception

        End Try

        Return pVal

    End Function


    Public Function ConvertToStr(ByVal Sng_In As Single, _
                                 ByVal Format_In As String) As String
        '==========================================================
        Return Sng_In.ToString(Format_In, CultureInfo.CurrentCulture)
    End Function


    Public Function ConvertToSng_USA(ByVal Str_In As String) As Single
        '=============================================================
        Dim pCI As New CultureInfo("en-US")
        If Str_In <> "" Then
            Return Convert.ToSingle(Str_In, pCI)
        End If
    End Function


    '....Not Used Now
    Public Function ReplaceStr(ByVal Sng_In As Single) As String
        '=======================================================
        Dim pStr = CStr(Sng_In)
        If pStr.Contains(",") Then
            pStr = pStr.Replace(",", ".")
        End If
        Return pStr
    End Function

#End Region

End Module
