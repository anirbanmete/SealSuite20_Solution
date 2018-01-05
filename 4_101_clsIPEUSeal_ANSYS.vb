'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsUSeal_ANSYS                         '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  23FEB16                                '
'                                                                              '
'===============================================================================
Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Collections.Specialized
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Threading
Imports System.Globalization
Imports clsLibrary11
Imports System.Windows.Forms


Partial Public Class IPE_clsUSeal
    Inherits IPE_clsSeal

    'Reference: Diagrams in SDD

#Region "MEMBER VARIABLE DECLARATIONS:"

    '....Key Points - Radial & Axial Flanges:
    '
    '   Axial Flange: 
    '       RHS: 1 (Open End), 2 (Control Dia End).
    '       LHS: 3 (Open End), 4 (Control Dia End).
    '
    '   Radial Flange:
    '       5 (RHS End), 6 (Mid Pt.), 7 (LHS End).
    '
    Dim mKP_Flange(6) As PointF      '....Flange.

#End Region


#Region "WRITE KEY POINT FILE:"

    Public Sub WriteFile_KP_T1Gen(ByVal Unit_In As IPE_clsUnit, ByVal ANSYS_In As IPE_clsANSYS)
        '==============================================================================    
        '....This Routine creates the Key Point File for USeal, 
        '........which is subsequently read by the ANSYS program upon execution.

        '....Update the member variables, if not yet updated.
        If mAdjusted = "N" Then
            Calc_MemberVariables("STD")

        ElseIf mAdjusted = "Y" Then
            Calc_MemberVariables("ADJ")
        End If


        Dim pSW As StreamWriter = Nothing
        'ANSYS_In.Model = "Full"        'AM 29MAY09     'For testing.

        Try

            'WRITE KEY POINT FILE.
            '=====================
            '
            pSW = File.CreateText(ANSYS_In.USealKP_FileName)

            With pSW

                .WriteLine("/COM, Key Points Coordinate File.")
                .WriteLine("/COM, File Name = USeal_KP_T1Gen_V1.txt.")
                .WriteLine("/COM, Written by: SealIPE")
                .WriteLine(" ")
                .WriteLine("/COM, HEADER INFORMATION:")
                .WriteLine("/COM, ===================")
                .WriteLine("!")
                .WriteLine(vbTab & "unitSys       = '" & Unit_In.System & "'")
                .WriteLine(vbTab & "SealType      = '" & Type & "'")
                .WriteLine(vbTab & "CrossSecNo    = '" & MCrossSecNo & "'")
                .WriteLine(vbTab & "Adjusted      = '" & Adjusted & "'")
                .WriteLine(vbTab & "POrient       = '" & POrient & "'")
                .WriteLine(vbTab & "ANSYS_Model   = '" & ANSYS_In.Model & "'")
                '.WriteLine(vbTab & "ANSYS_Edition = '" & ANSYS_In.Edition & "'")
                .WriteLine("!")
                .WriteLine("/COM, ===================")
                .WriteLine(" ")


                '   Centre Point Variables:
                '   -----------------------

                .WriteLine("/COM, KeyPoints & Center Points:")
                .WriteLine("/COM, ==========================")
                .WriteLine("/COM,")
                .WriteLine("kOffCen = 150")
                .WriteLine("kCEN1 = kOffCen + 1")
                .WriteLine("kCEN2 = kOffCen + 2")
                .WriteLine("kCEN3 = kOffCen + 3")
                .WriteLine(" ")

                '.WriteLine("K, kCEN1, " & LPFormat_KP(Unit_In, mCen(1)))
                .WriteLine("K, kCEN1, " & LPFormat_KP(mCen(1)))
                .WriteLine("K, kCEN2, " & LPFormat_KP(mCen(2)))
                .WriteLine("K, kCEN3, " & LPFormat_KP(mCen(3)))
                .WriteLine(" ")

                '   End Conv - RHS 1/2:
                '   -------------------
                '
                .WriteLine("/COM, Seal Keypoints:")
                .WriteLine("/COM, ===============")
                .WriteLine("/COM, ")
                .WriteLine("/COM, End Conv - RHS 1/2:")
                .WriteLine("/COM, -------------------")
                .WriteLine("/COM, ")

                '       Segment 1.
                '       -----------
                .WriteLine("/COM,   Segment 1:")
                .WriteLine("K,  8, " & LPFormat_KP(mKP(8)))
                .WriteLine("K,  7, " & LPFormat_KP(mKP(7)))
                .WriteLine("K,  6, " & LPFormat_KP(mKP(6)))
                .WriteLine("K,  5, " & LPFormat_KP(mKP(5)))
                '
                '       Segment 2.
                '       -----------
                '
                .WriteLine("/COM,   Segment 2:")
                .WriteLine("K,  4, " & LPFormat_KP(mKP(4)))
                .WriteLine("K,  3, " & LPFormat_KP(mKP(3)))
                '
                '       Segment 3.
                '       -----------
                '
                .WriteLine("/COM,   Segment 3:")
                .WriteLine("K,  2, " & LPFormat_KP(mKP(2)))
                .WriteLine("K,  1, " & LPFormat_KP(mKP(1)))
                .WriteLine(" ")


                If ANSYS_In.Model = "Full" Then
                    '--------------------------         
                    .WriteLine("/COM, ")
                    .WriteLine("/COM, End Conv - LHS 1/2:")
                    .WriteLine("/COM, -------------------")
                    .WriteLine("/COM, ")
                    '       Segment 1.
                    '       -----------
                    '
                    .WriteLine("/COM,   Segment 1:")
                    .WriteLine("K,  9, " & LPFormat_KP(mKP(9)))
                    .WriteLine("K, 10, " & LPFormat_KP(mKP(10)))
                    '
                    '       Segment 2.
                    '       -----------
                    '
                    .WriteLine("/COM,   Segment 2:")
                    .WriteLine("K, 11, " & LPFormat_KP(mKP(11)))
                    .WriteLine("K, 12, " & LPFormat_KP(mKP(12)))
                    '
                    '       Segment 3.
                    '       -----------
                    '
                    .WriteLine("/COM,   Segment 3:")
                    .WriteLine("K, 13, " & LPFormat_KP(mKP(13)))
                    .WriteLine("K, 14, " & LPFormat_KP(mKP(14)))
                    .WriteLine(" ")
                End If

                '   Secondary Key Points
                '   --------------------

                .WriteLine("/COM, Secondary Key Points - RHS 1/2:")
                .WriteLine("/COM, -------------------------------")

                .WriteLine("kOffSec = 50")

                Dim pKP_Sec(4) As PointF

                With pKP_Sec(1)
                    .X = mCen(2).X
                    .Y = mCen(2).Y - (mR(2) - 0.5 * mT)
                End With

                With pKP_Sec(2)
                    .X = mCen(2).X
                    .Y = mCen(2).Y - (mR(2) + 0.5 * mT)
                End With

                .WriteLine("K, kOffSec+1, " & LPFormat_KP(pKP_Sec(1)))
                .WriteLine("K, kOffSec+2, " & LPFormat_KP(pKP_Sec(2)))

                If ANSYS_In.Model = "Full" Then
                    '--------------------------     

                    With pKP_Sec(3)
                        .X = mCen(3).X
                        .Y = mCen(3).Y + (mR(2) - 0.5 * mT)
                    End With

                    With pKP_Sec(4)
                        .X = mCen(3).X
                        .Y = mCen(3).Y + (mR(2) + 0.5 * mT)
                    End With

                    .WriteLine("K, kOffSec+3, " & LPFormat_KP(pKP_Sec(3)))
                    .WriteLine("K, kOffSec+4, " & LPFormat_KP(pKP_Sec(4)))
                End If

                .WriteLine("")

                '   Calculate Key Points of Flange
                '   ------------------------------
                Calc_KP_Flange(ANSYS_In)

                .WriteLine("kOffFlange = 100")
                .WriteLine("")
                '       Axial Flange:
                '       ------------
                '
                .WriteLine("/COM, Axial Flange:")
                .WriteLine("/COM, -------------")
                .WriteLine("")

                '....RHS 
                .WriteLine("/COM, RHS:")
                '    Open End Point.
                '....Key Point 101
                .WriteLine("K, kOffFlange+1, " & LPFormat_KP(mKP_Flange(1)))

                '    Control Dia End Point.
                '....Key Point 102
                .WriteLine("K, kOffFlange+2, " & LPFormat_KP(mKP_Flange(2)))
                .WriteLine("")

                If ANSYS_In.Model = "Full" Then
                    '--------------------------     
                    '....LHS
                    .WriteLine("/COM, LHS:")
                    '    Open End Point.
                    '....Key Point 103
                    .WriteLine("K, kOffFlange+3, " & LPFormat_KP(mKP_Flange(3)))
                    '    Control Dia End Point.
                    '....Key Point 104
                    .WriteLine("K, kOffFlange+4, " & LPFormat_KP(mKP_Flange(4)))
                    .WriteLine(" ")
                End If


                '       Radial Flange
                '       ------------
                '
                .WriteLine("/COM, Radial Flange:")
                .WriteLine("/COM, --------------")

                '    RHS Point.
                '....Key Point 105
                .WriteLine("K, kOffFlange+5, " & LPFormat_KP(mKP_Flange(5)))

                '    LHS Point.
                '....Key Point 106
                .WriteLine("K, kOffFlange+6, " & LPFormat_KP(mKP_Flange(6)))

            End With

        Catch pEXP As IOException

            '....ERROR HANDLER            
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
                                                                 MessageBoxIcon.Error)
            Exit Sub

        Catch pEXP As Exception

            '....ERROR HANDLER
            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes, pintAnswer As Short

            pstrTitle = "ERROR MESSAGE:  Output Data File Writing"
            pstrMsg = "Error in file writing!!" & vbCrLf
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly

            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

            Exit Sub

        Finally
            pSW.Close()
        End Try

    End Sub

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub Calc_KP_Flange(ByVal ANSYS_In As IPE_clsANSYS)
        '=================================================

        '....Sealing Point RHS.
        Dim pSealingPt As Single
        pSealingPt = mCen(2).Y - (mR(2) + 0.5 * mT)


        '   AXIAL FLANGE:
        '   -------------
        '   
        '   ....RHS:
        '       Open End Point.
        '
        With mKP_Flange(1)
            If POrient = "External" Then
                .X = mKP(2).X + mTStd
            ElseIf POrient = "Internal" Then
                .X = mKP(2).X - mTStd
            End If

            .Y = pSealingPt

        End With

        '       Control Dia End Point.

        With mKP_Flange(2)
            .X = mKP(8).X
            .Y = mKP_Flange(1).Y
        End With

        '   ....LHS:
        '....Sealing Point LHS.
        pSealingPt = mCen(3).Y + (mR(2) + 0.5 * mT)

        '       Open End Point.

        With mKP_Flange(3)
            If POrient = "External" Then
                .X = mKP(2).X + mTStd
            ElseIf POrient = "Internal" Then
                .X = mKP(2).X - mTStd
            End If

            .Y = pSealingPt

        End With

        '       Control Dia End Point.

        With mKP_Flange(4)
            .X = mKP(8).X
            .Y = mKP_Flange(3).Y
        End With


        '   RADIAL FLANGE:
        '   -------------
        '   
        '       RHS Point.

        With mKP_Flange(5)
            If POrient = "External" Then
                .X = mKP_Flange(2).X - mZClear
            ElseIf POrient = "Internal" Then
                .X = mKP_Flange(2).X + mZClear
            End If

            .Y = mKP_Flange(2).Y
        End With

        '       LHS Point.
        With mKP_Flange(6)
            .X = mKP_Flange(5).X

            If ANSYS_In.Model = "Full" Then
                .Y = mKP_Flange(4).Y

            ElseIf ANSYS_In.Model = "Half" Then
                .Y = mCen(1).Y
            End If

        End With

        ''       LHS Point.

        'With mKP_Flange(7)
        '    .X = mKP_Flange(5).X
        '    .Y = mKP_Flange(4).Y
        'End With

    End Sub

    Private Function LPFormat_KP(ByVal Pt_In As PointF) As String
        '========================================================
        'This function is used in displaying length data with eight(8) precision.

        Dim Pt_X As String = ""  '....Holds formated string of X Co-ordinate
        Dim Pt_Y As String = "" '....Holds formated string of Y Co-ordinate

        'If Unit_In.System = "English" Then  '....Four(4)precision for "English" Unit.
        '    '------------------------------

        '    If Pt_In.X >= 0 Then
        '        Pt_X = Format(Pt_In.X, " " & "##0.0000")
        '    Else
        '        Pt_X = Format(Pt_In.X, "##0.0000")
        '    End If

        '    If Pt_In.Y >= 0 Then
        '        Pt_Y = Format(Pt_In.Y, " " & "##0.0000")
        '    Else
        '        Pt_Y = Format(Pt_In.Y, "##0.0000")
        '    End If

        'ElseIf Unit_In.System = "Metric" Then   '....Eight(8)precision for "Metric" Unit.
        '    '---------------------------------

        '....Eight(8)precision
        If Pt_In.X >= 0 Then
            Pt_X = Format(Pt_In.X, " " & "##0.00000000")
        Else
            Pt_X = Format(Pt_In.X, "##0.00000000")
        End If

        If Pt_In.Y >= 0 Then
            Pt_Y = Format(Pt_In.Y, " " & "##0.00000000")
        Else
            Pt_Y = Format(Pt_In.Y, "##0.00000000")
        End If

        'End If

        Return Pt_X & "," & Pt_Y

    End Function

#End Region

End Class
