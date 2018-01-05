'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsESeal_DataValidate                  '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JUN08                                '
'                                                                              '
'===============================================================================

'Routines
'--------
'           Public  Function    Check_InnerRad              ()                        
'           Public  Function    Check_Graphics_Distortion   ()
'           Public  Sub         Check_TxtVal                ()

'--------------------------------------------------------------------------------

Imports clsLibrary11
Imports System.Windows.Forms
Imports System.Drawing

Partial Public Class IPE_clsESeal
    Inherits IPE_clsSeal

    'SB 31MAR08
    Public Function Check_InnerRad() As Boolean
        '=======================================
        Dim pbln As Boolean = True

        Dim i As Integer

        Dim pRadE_In As Single, pSng As Single

        If mUnit.System = "English" Then
            pSng = 0.005
        Else
            pSng = 0.0002
        End If

        For i = 1 To 3
            '=========
            pRadE_In = mRadE(i) - 0.5 * mT

            If pRadE_In <= pSng Then
                pbln = False
            End If
        Next

        Dim pRadM_In As Single
        For i = 1 To 5
            '=========
            pRadM_In = mRadM(i) - 0.5 * mT

            If pRadM_In <= pSng Then
                pbln = False
            End If
        Next

        Return pbln

    End Function


    Public Function Check_Graphics_Distortion() As Boolean
        '=================================================      'SB 03JUN08
        Dim pbln As Boolean = True

        Calc_MemberVariables("STD")

        '....Graphics checking for ThetaM2 & ThetaM4
        If POrient = "Internal" Then

            If mCenM(3).X >= mCenM(1).X Then
                pbln = False
            End If

        ElseIf POrient = "External" Then

            If mCenM(3).X <= mCenM(1).X Then
                pbln = False
            End If

        End If


        If mNConv > 1 Then      '....Graphics checking if Convolution > 1

            '....Graphics checking for ThetaM1

            Dim pPt1 As PointF   '....Intermediate point
            With pPt1
                .X = mCenM(5).X
                .Y = mCenM(5).Y - (mRadM(5) + 0.5 * mT) * CosD(ThetaM(5) - 90)
            End With

            Dim pPt2 As PointF
            With pPt2
                .X = mCenE(1).X
                .Y = mCenE(1).Y + (mRadM(1) + 0.5 * mT) * CosD(ThetaM(1) - 90)
            End With

            '....checking whether Y Co-ordinate of Mid-End Outer Point
            '......is greater than Y Co-ordinate of Mid-Mid Outer Point
            If pPt2.Y >= pPt1.Y Then
                pbln = False
            End If

            '....Graphics checking for ThetaM2 & ThetaM4
            If POrient = "Internal" Then

                If mCenM(3).X >= mCenM(5).X Then
                    pbln = False
                End If

            ElseIf POrient = "External" Then

                If mCenM(3).X <= mCenM(5).X Then
                    pbln = False
                End If

            End If

        End If

        Return pbln

    End Function



    Public Sub Check_TxtRad_Val(ByRef txtbox_In As TextBox, _
                                ByVal value_In As Single)
        '===================================================
        If value_In <= gcEPS Then Exit Sub

        Dim pStr As String = ExtractPostData(value_In.ToString(), ".")

        If pStr.Length >= 4 Then
            Dim pChar As Char = pStr(3)

            If Val(pChar) >= 5 Then
                Dim pVal As String = ConvertToStr(value_In, "#0.000")
                txtbox_In.Text = pVal
            End If

        End If

    End Sub

    Public Sub Check_TxtTheta_Val(ByRef txtbox_In As TextBox, _
                                  ByVal value_In As Single)
        '===================================================
        If value_In <= gcEPS Then Exit Sub

        Dim pStr As String = ExtractPostData(value_In.ToString(), ".")

        If pStr.Length >= 3 Then
            Dim pChar As Char = pStr(2)

            If Val(pChar) >= 5 Then
                Dim pVal As String = ConvertToStr(value_In, "#0.00")
                txtbox_In.Text = pVal
            End If

        End If

    End Sub

End Class
