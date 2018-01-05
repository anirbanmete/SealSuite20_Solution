'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsESeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28APR11                                '
'                                                                              '
'===============================================================================

'Routines
'--------
'
'   METHODS:
'   --------  
'   Public Sub          Create_DXF              ()

'   Private Sub         DrawAddHalfConv_CAD     ()
'   Private Sub         Draw_LHS_MidMidHalfConv ()
'   Private Sub         Draw_LHS_MidEndHalfConv ()
'   Private Sub         Draw_LHS_EndHalfConv    ()
'   Private Sub         DrawAddHalfConv         ()
'   Private Sub         DrawAddHalfConv_St      ()

'--------------------------------------------------------------------------------

Imports System.Data.OleDb
Imports System.Math
Imports System.IO

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports clsLibrary11

Imports System.Windows.Forms


Partial Public Class IPE_clsESeal
    Inherits IPE_clsSeal


#Region "MEMBER VARIABLE DECLARATIONS:"
    Private mY_Sym_Axis As Single       '....Symmetry Axis Location. 
    'Private mSW As StreamWriter         '....Stream Writer for DXf File. Move to clsSeal
#End Region


    '*******************************************************************************
    '*                        CLASS METHODS - BEGIN                                *
    '*******************************************************************************

    Public Sub Create_DXF(ByVal FileName_In As String)
        '==================================================    

        'Dim pACadVersion As String = UserInfo_In.ACADVersion
        'Try
        '    If File.Exists(FileName_In) Then _
        '        File.Delete(FileName_In)

        '    mSW = File.CreateText(FileName_In)

        '    '....HEADER.
        '    With mSW
        '        .WriteLine("0")
        '        .WriteLine("SECTION")
        '        .WriteLine("2")
        '        .WriteLine("ENTITIES")
        '    End With

        '....HEADER.
        DXF_Header(FileName_In)


        '=======================================================
        '                 DXF CREATION DRAWING                 '
        '=======================================================

        DrawESeal_DXF()

        'Draw_End()                   '....Draw  End     Half Convolutions (RHS & LHS).
        'Draw_MidEnd()                '....Draw  Mid-End Half Convolutions (RHS & LHS).

        'If mNConv >= 2 Then
        '    Draw_MidMid()            '....Draw Mid-Mid Half Convolutions (RHS & LHS).


        '    If mNConv >= 3 Then
        '        mCountAddHalfConv = mNConv - 2      '....# of additional Half Convolutions.
        '        mKP_Last = 22                       '....Last KeyPoint # of the basic Half Convolutions.

        '        Draw_Add_HalfConv()                 '....Draw Additional Mid-End Convolutions (RHS & LHS).

        '    End If

        'End If


        '....FOOTER.
        DXF_Footer()
        'With mSW
        '    .WriteLine("0")
        '    .WriteLine("ENDSEC")
        '    .WriteLine("0")
        '    .WriteLine("EOF")

        '    .Close()
        'End With


        '....Message.                   'Move to form SG 18APR11
        'Dim pMsg As String
        'pMsg = ExtractPreData(gFiles.In_Title, ".") & ".DXF" & _
        '        " file has been created successfully. "
        'MessageBox.Show(pMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

        'Catch ex As Exception
        '    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

    End Sub


#Region "Symmetry Axes and Additional Center Point:"

    Private Sub Calc_Sym_Axes()
        '======================
        '....This routine calculates the Y-location of the Symmetry Axis.

        Select Case mNConv

            Case 1
                mY_Sym_Axis = mCenM(3).Y

            Case 2
                mY_Sym_Axis = mCenM(5).Y


            Case Is >= 3

                Calc_Add_Cen()      '.... CalCulate Additional Center Point

                If mNConv Mod 2 = 0 Then
                    mY_Sym_Axis = mCenAdd(mCountAddHalfConv, 1).Y

                ElseIf mNConv Mod 2 = 1 Then
                    '....Get the location of Symmetry Axis
                    mY_Sym_Axis = mCenAdd(mCountAddHalfConv, 3).Y

                End If

        End Select

    End Sub


    Private Sub Calc_Add_Cen()
        '=====================

        '   CalCulate Additional Center Point
        '   ---------------------------------
        '
        '....Reinitialize Additional Center points
        ReDim mCenAdd(mCountAddHalfConv, 3)

        '....Initialize Center Points
        mCenAdd(0, 1) = mCenM(5)
        mCenAdd(0, 2) = mCenM(4)
        mCenAdd(0, 3) = mCenM(3)

        Dim i As Int16
        For i = 1 To mCountAddHalfConv

            Dim pY_Sym_Axis As Single

            '....Initialize.
            If i Mod 2 = 1 Then
                pY_Sym_Axis = mCenAdd(i - 1, 1).Y       '....ODD  i

            ElseIf i Mod 2 = 0 Then
                pY_Sym_Axis = mCenAdd(i - 1, 3).Y       '....EVEN i
            End If


            Dim j As Int16
            For j = 1 To 3

                Dim pY1_SymDist As Single      '....Symmetry distance of RHS
                Dim pY1_SymDist_Refl As Single '....Symmetry Reflection in LHS

                pY1_SymDist = mCenAdd(i - 1, j).Y - pY_Sym_Axis
                pY1_SymDist_Refl = -pY1_SymDist

                With mCenAdd(i, j)
                    .X = mCenAdd(i - 1, j).X
                    .Y = pY_Sym_Axis + pY1_SymDist_Refl    '....Transformation of coordinates. Y1 ==> Y.
                End With
            Next

        Next

    End Sub

#End Region


#Region "DXF FILE CREATION:"

    Private Sub DrawESeal_DXF()
        '========================

        If mTemplateNo = "1" Then
            '------------------------------------------
            Calc_KP_T1()        '...."KEY POINTS FOR TEMPLATE 1"  

        ElseIf mTemplateNo = "2" Then
            '-------------------------                              SG 19APR11
            Calc_KP_T2()        '...."KEY POINTS FOR TEMPLATE 2"  

        ElseIf mTemplateNo = "1Gen" Or mTemplateNo = "1GenS" Then
            '-----------------------------------------------------
            Calc_KP_T1Gen()     '.... Key Points: Template "1Gen" & "1GenS"
        End If


        Calc_Sym_Axes()     '.... Determine Symmetry Axis Y-Location.


        Draw_End()                   '....Draw  End     Half Convolutions (RHS & LHS).
        Draw_MidEnd()                '....Draw  Mid-End Half Convolutions (RHS & LHS).

        If mNConv >= 2 Then
            Draw_MidMid()            '....Draw Mid-Mid Half Convolutions (RHS & LHS).


            If mNConv >= 3 Then
                mCountAddHalfConv = mNConv - 2      '....# of additional Half Convolutions.
                mKP_Last = 22                       '....Last KeyPoint # of the basic Half Convolutions.

                Draw_Add_HalfConv()                 '....Draw Additional Mid-End Convolutions (RHS & LHS).

            End If

        End If

    End Sub

#Region "....Draw End Half Convolution"

    Private Sub Draw_End()
        '=================
        '....This routine draws the End Half Convolutions (RHS & LHS).

        Dim pRadIn As Single, pRadOut As Single
        Dim pStartAng As Single, pEndAng As Single

        '   End Convolution - RHS 1/2:
        '   --------------------------
        '
        '       Segment E1.
        '       -----------
        '
        '   ASSUMPTION for the following derivation for sign:
        '       The angle thetaE1 > 90.

        pRadIn = mRadE(1) - 0.5 * mT
        pRadOut = mRadE(1) + 0.5 * mT

        Dim pThetaE1Draw As Single
        pThetaE1Draw = mThetaE(1) + mDThetaE1

        Dim pBetaE1 As Single
        pBetaE1 = pThetaE1Draw - 90

        If POrient = "External" Then
            '-----------------------
            pStartAng = -90
            pEndAng = pBetaE1

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = -pBetaE1
            pEndAng = 90

        End If


        'DrawArc(mY_Sym_Axis, mCenE(1), pRadIn, pStartAng, pEndAng)  'SB 30DEC09
        'DrawArc(mY_Sym_Axis, mCenE(1), pRadOut, pStartAng, pEndAng)

        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenE(1), pRadIn, pStartAng, pEndAng)
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenE(1), pRadOut, pStartAng, pEndAng)

        '....Draw Line KP1 and KP2
        'DrawLine(mY_Sym_Axis, mKP(1), mKP(2))           'SB 30DEC09
        DXF_Lines_SymAxis(mY_Sym_Axis, mKP(1), mKP(2))


        '       Segment E2
        '       ----------.
        '
        pRadIn = mRadE(2) - 0.5 * mT
        pRadOut = mRadE(2) + 0.5 * mT

        Dim pThetaDiff As Single
        pThetaDiff = mThetaE(2) - pBetaE1

        If POrient = "External" Then
            '-----------------------
            pStartAng = 180 - pThetaDiff
            pEndAng = 180 + pBetaE1

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = 180 - pBetaE1
            pEndAng = 180 + pThetaDiff

        End If

        'BG 04AUG09
        'DrawArc(mY_Sym_Axis, mCenE(2), pRadIn, pStartAng, pEndAng)  'SB 30DEC09
        'DrawArc(mY_Sym_Axis, mCenE(2), pRadOut, pStartAng, pEndAng)

        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenE(2), pRadIn, pStartAng, pEndAng)  'SG 04APR11
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenE(2), pRadOut, pStartAng, pEndAng)

        '
        '       Segment E3.
        '       -----------

        pRadIn = mRadE(3) - 0.5 * mT
        pRadOut = mRadE(3) + 0.5 * mT

        Dim pBetaE3 As Single
        pBetaE3 = pThetaDiff

        If POrient = "External" Then
            '------------------------
            pStartAng = -pBetaE3
            pEndAng = mThetaE(3)

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = -mThetaE(3)
            pEndAng = pBetaE3

        End If

        'DrawArc(mY_Sym_Axis, mCenE(3), pRadIn, pStartAng, pEndAng)  
        'DrawArc(mY_Sym_Axis, mCenE(3), pRadOut, pStartAng, pEndAng)

        '....DrawArc () in clsSeal
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenE(3), pRadIn, pStartAng, pEndAng)
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenE(3), pRadOut, pStartAng, pEndAng)


        'Draw the straight segment.    Key points Move to clsESeal_ANSYS               'SG 19APR11
        '-------------------------
        'TemplateNo = 2 : Flat Segment

        If mTemplateNo = "2" Then
            '--------------------
            'Dim pE15 As PointF
            'With pE15
            '    If POrient = "External" Then
            '        .X = mKP(7).X + mLFlatE * CosD(pBetaE1)
            '    ElseIf POrient = "Internal" Then
            '        .X = mKP(7).X - mLFlatE * CosD(pBetaE1)
            '    End If

            '    .Y = mKP(7).Y + mLFlatE * SinD(pBetaE1)
            'End With

            'Dim pE52 As PointF
            'With pE52
            '    If POrient = "External" Then
            '        .X = mKP(8).X + mLFlatE * CosD(pBetaE1)
            '    ElseIf POrient = "Internal" Then
            '        .X = mKP(8).X - mLFlatE * CosD(pBetaE1)
            '    End If
            '    .Y = mKP(8).Y + mLFlatE * SinD(pBetaE1)

            'End With

            DXF_Lines_SymAxis(mY_Sym_Axis, mKP_Tpl2(1), mKP(7))
            DXF_Lines_SymAxis(mY_Sym_Axis, mKP_Tpl2(2), mKP(8))
        End If

    End Sub

#End Region


#Region "....Draw Mid-End Half Convolution"

    Private Sub Draw_MidEnd()
        '====================

        '....Holds radii. 
        Dim pRadIn As Single, pRadOut As Single

        '....Holds angels.
        Dim pStartAng As Single, pEndAng As Single

        '   Mid-End Convolution - RHS 1/2:
        '   -----------------------------
        '
        '       Segment M1.
        '       -----------
        '
        pRadIn = mRadM(1) - 0.5 * mT
        pRadOut = mRadM(1) + 0.5 * mT

        Dim pThetaM1Draw As Single
        pThetaM1Draw = mThetaM(1) + mDThetaM1

        Dim pBetaM1 As Single
        pBetaM1 = pThetaM1Draw - 90

        If POrient = "External" Then
            '------------------------
            pStartAng = 180 - pBetaM1
            pEndAng = 180 + 90

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = 180 - 90
            pEndAng = 180 + pBetaM1

        End If

        'DrawArc(mY_Sym_Axis, mCenM(1), pRadIn, pStartAng, pEndAng)
        'DrawArc(mY_Sym_Axis, mCenM(1), pRadOut, pStartAng, pEndAng)

        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(1), pRadIn, pStartAng, pEndAng)
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(1), pRadOut, pStartAng, pEndAng)

        '       Segment M2.
        '       -----------
        '
        If mTemplateNo = "1Gen" Then
            '========================

            pRadIn = mRadM(2) - 0.5 * mT
            pRadOut = mRadM(2) + 0.5 * mT

            Dim pThetaDiff As Single
            pThetaDiff = mThetaM(2) - pBetaM1

            If POrient = "External" Then
                '------------------------
                pStartAng = -pBetaM1
                pEndAng = pThetaDiff

            ElseIf POrient = "Internal" Then
                '---------------------------
                pStartAng = -pThetaDiff
                pEndAng = pBetaM1

            End If

            'BG 04AUG09
            'DrawArc(mY_Sym_Axis, mCenM(2), pRadIn, pStartAng, pEndAng)  'SB 30DEC09
            'DrawArc(mY_Sym_Axis, mCenM(2), pRadOut, pStartAng, pEndAng)

            '....DrawArc () in clsSeal
            DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(2), pRadIn, pStartAng, pEndAng)  'SG 04APR11
            DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(2), pRadOut, pStartAng, pEndAng)


        ElseIf mTemplateNo = "1GenS" Or mTemplateNo = "1" Or mTemplateNo = "2" Then
            '======================================================================
            '....Draw Straight Segment

            'DrawLine(mY_Sym_Axis, mKP(11), mKP(13)) 'SB 30DEC09
            'DrawLine(mY_Sym_Axis, mKP(12), mKP(14))

            DXF_Lines_SymAxis(mY_Sym_Axis, mKP(11), mKP(13))
            DXF_Lines_SymAxis(mY_Sym_Axis, mKP(12), mKP(14))

        End If

        '       Segment M3
        '       ----------
        '
        pRadIn = mRadM(3) - 0.5 * mT
        pRadOut = mRadM(3) + 0.5 * mT

        Dim pThetaM3Draw As Single
        pThetaM3Draw = mThetaM(3) + mDThetaM3

        Dim pBetaM3 As Single
        pBetaM3 = pThetaM3Draw - 90

        If POrient = "External" Then
            '-----------------------
            pStartAng = -pBetaM3
            pEndAng = 180 + pBetaM3

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = 180 - pBetaM3
            pEndAng = pBetaM3

        End If

        'BG 04AUG09
        'DrawArc(mY_Sym_Axis, mCenM(3), pRadIn, pStartAng, pEndAng)  'SB 30DEC09
        'DrawArc(mY_Sym_Axis, mCenM(3), pRadOut, pStartAng, pEndAng)

        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(3), pRadIn, pStartAng, pEndAng)  'SG 18APR11
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(3), pRadOut, pStartAng, pEndAng)

    End Sub

#End Region


#Region "....Draw Mid-Mid Half Convolution"

    Private Sub Draw_MidMid()
        '====================
        '....Holds radii. 
        Dim pRadIn As Single, pRadOut As Single

        '....Holds angels.
        Dim pStartAng As Single, pEndAng As Single

        '   Mid-End Convolution - RHS 1/2:
        '   -----------------------------
        '
        '       Segment M4
        '       ----------
        '
        Dim pThetaM5Draw As Single
        pThetaM5Draw = mThetaM(5) + mDThetaM5

        Dim pBetaM5 As Single
        pBetaM5 = pThetaM5Draw - 90

        If mTemplateNo = "1Gen" Then
            '========================

            pRadIn = mRadM(4) - 0.5 * mT
            pRadOut = mRadM(4) + 0.5 * mT

            Dim pThetaDiff As Single
            pThetaDiff = mThetaM(4) - pBetaM5

            If POrient = "External" Then
                '------------------------
                pStartAng = 180 - pThetaDiff
                pEndAng = 180 + pBetaM5

            ElseIf POrient = "Internal" Then
                '---------------------------
                pStartAng = 180 - pBetaM5
                pEndAng = 180 + pThetaDiff

            End If

            'BG 04AUG09
            'DrawArc(mY_Sym_Axis, mCenM(4), pRadIn, pStartAng, pEndAng)  'SB 30DEC09
            'DrawArc(mY_Sym_Axis, mCenM(4), pRadOut, pStartAng, pEndAng)

            DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(4), pRadIn, pStartAng, pEndAng)
            DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(4), pRadOut, pStartAng, pEndAng)


        ElseIf mTemplateNo = "1GenS" Or mTemplateNo = "1" Or mTemplateNo = "2" Then
            '======================================================================  'SG 05APR11

            '....Draw Straight Segment
            'BG 04AUG09
            'DrawLine(mY_Sym_Axis, mKP(17), mKP(19)) 'SB 30DEC09
            'DrawLine(mY_Sym_Axis, mKP(18), mKP(20))

            DXF_Lines_SymAxis(mY_Sym_Axis, mKP(17), mKP(19))
            DXF_Lines_SymAxis(mY_Sym_Axis, mKP(18), mKP(20))

        End If

        '       Segment M5
        '       ----------
        '
        pRadIn = mRadM(5) - 0.5 * mT
        pRadOut = mRadM(5) + 0.5 * mT

        If POrient = "External" Then
            '------------------------
            pStartAng = -90
            pEndAng = pBetaM5

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = -pBetaM5
            pEndAng = 90

        End If

        'BG 04AUG09
        'DrawArc(mY_Sym_Axis, mCenM(5), pRadIn, pStartAng, pEndAng)  'SB 30DEC09
        'DrawArc(mY_Sym_Axis, mCenM(5), pRadOut, pStartAng, pEndAng)

        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(5), pRadIn, pStartAng, pEndAng)
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCenM(5), pRadOut, pStartAng, pEndAng)

    End Sub

#End Region


#Region "....Draw Additional Half Convolution"

    Private Sub Draw_Add_HalfConv()
        '==========================

        '....Holds Angels.
        Dim pStartAng As Single, pEndAng As Single

        '....Additional Half Convolution Radii.
        Dim pRadOut(3) As Single
        Dim pRadIn(3) As Single

        Dim i As Int16
        For i = 1 To 3
            pRadOut(i) = mRadM(6 - i) + 0.5 * mT
            pRadIn(i) = mRadM(6 - i) - 0.5 * mT

        Next

        For i = 1 To mCountAddHalfConv

            If i Mod 2 = 0 Then         '....For Even i
                '==============

                '#Zone " Additional Half for Even Convolution "

                '#Zone " Segment 1 "

                '   Segment 1
                '   ---------
                '
                Dim pThetaM5Draw As Single
                pThetaM5Draw = mThetaM(5) + mDThetaM5

                Dim pBetaM5 As Single
                pBetaM5 = pThetaM5Draw - 90

                If POrient = "External" Then
                    '------------------------
                    pStartAng = -90
                    pEndAng = pBetaM5

                ElseIf POrient = "Internal" Then
                    '---------------------------
                    pStartAng = -pBetaM5
                    pEndAng = 90

                End If

                'BG 04AUG09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 1), pRadIn(1), pStartAng, pEndAng)  'SB 30DEC09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 1), pRadOut(1), pStartAng, pEndAng)

                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 1), pRadIn(1), pStartAng, pEndAng)
                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 1), pRadOut(1), pStartAng, pEndAng)

                '#End Zone

                '#Zone " Segment 2 "

                '   Segment 2
                '   ---------
                '
                If mTemplateNo = "1Gen" Then
                    '========================

                    Dim pThetaDiff As Single
                    pThetaDiff = mThetaM(4) - pBetaM5

                    If POrient = "External" Then
                        '------------------------
                        pStartAng = 180 - pThetaDiff
                        pEndAng = 180 + pBetaM5

                    ElseIf POrient = "Internal" Then
                        '---------------------------
                        pStartAng = 180 - pBetaM5
                        pEndAng = 180 + pThetaDiff

                    End If

                    'BG 04AUG09
                    'DrawArc(mY_Sym_Axis, mCenAdd(i, 2), pRadIn(2), pStartAng, pEndAng)  'SB 30DEC09
                    'DrawArc(mY_Sym_Axis, mCenAdd(i, 2), pRadOut(2), pStartAng, pEndAng)

                    DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 2), pRadIn(2), pStartAng, pEndAng)
                    DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 2), pRadOut(2), pStartAng, pEndAng)


                ElseIf mTemplateNo = "1GenS" Or mTemplateNo = "1" Or mTemplateNo = "2" Then
                    '======================================================================  'SG 05APR11    
                    '....Draw Straight Segment

                    'BG 04AUG09
                    'DrawLine(mY_Sym_Axis, mKP(mKP_Last + 1), mKP(mKP_Last + 3)) 'SB 30DEC09
                    'DrawLine(mY_Sym_Axis, mKP(mKP_Last + 2), mKP(mKP_Last + 4))


                    DXF_Lines_SymAxis(mY_Sym_Axis, mKP(mKP_Last + 1), mKP(mKP_Last + 3))
                    DXF_Lines_SymAxis(mY_Sym_Axis, mKP(mKP_Last + 2), mKP(mKP_Last + 4))

                End If

                '#End Zone

                '#Zone " Segment 3 "

                '   Segment 3
                '   ---------
                '
                Dim pThetaM3Draw As Single
                pThetaM3Draw = mThetaM(3) + mDThetaM3

                Dim pBetaM3 As Single
                pBetaM3 = pThetaM3Draw - 90

                If POrient = "External" Then
                    '------------------------
                    pStartAng = 180 - 90
                    pEndAng = 180 + pBetaM3

                ElseIf POrient = "Internal" Then
                    '---------------------------
                    pStartAng = 180 - pBetaM3
                    pEndAng = 180 + 90

                End If

                'BG 04AUG09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 3), pRadIn(3), pStartAng, pEndAng)  'SB 30DEC09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 3), pRadOut(3), pStartAng, pEndAng)

                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 3), pRadIn(3), pStartAng, pEndAng)
                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 3), pRadOut(3), pStartAng, pEndAng)

                '#End Zone
                '#End Zone


            ElseIf i Mod 2 <> 0 Then    '....For Odd i
                '===================

                '#Zone " Additional Half for Odd Convolution "

                '#Zone " Segment 1 "
                '   Segment 1
                '   ---------
                Dim pThetaM5Draw As Single
                pThetaM5Draw = mThetaM(5) + mDThetaM5

                Dim pBetaM5 As Single
                pBetaM5 = pThetaM5Draw - 90

                If POrient = "External" Then
                    '------------------------
                    pStartAng = 180 - pBetaM5
                    pEndAng = 180 + 90

                ElseIf POrient = "Internal" Then
                    '---------------------------
                    pStartAng = 180 - 90
                    pEndAng = 180 + pBetaM5
                End If

                'BG 04AUG09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 1), pRadIn(1), pStartAng, pEndAng)  'SB 30DEC09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 1), pRadOut(1), pStartAng, pEndAng)

                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 1), pRadIn(1), pStartAng, pEndAng)
                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 1), pRadOut(1), pStartAng, pEndAng)

                '#End Zone

                '#Zone " Segment 2 "
                '   Segment 2
                '   ---------
                '
                If mTemplateNo = "1Gen" Then
                    '------------------------

                    pBetaM5 = mThetaM(5) - 90

                    Dim pThetaDiff As Single
                    pThetaDiff = mThetaM(4) - pBetaM5

                    If POrient = "External" Then
                        '------------------------
                        pStartAng = -pBetaM5
                        pEndAng = pThetaDiff

                    ElseIf POrient = "Internal" Then
                        '----------------------------
                        pStartAng = -pThetaDiff
                        pEndAng = pBetaM5

                    End If

                    'BG 04AUG09
                    'DrawArc(mY_Sym_Axis, mCenAdd(i, 2), pRadIn(2), pStartAng, pEndAng)  'SB 30DEC09
                    'DrawArc(mY_Sym_Axis, mCenAdd(i, 2), pRadOut(2), pStartAng, pEndAng)

                    DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 2), pRadIn(2), pStartAng, pEndAng)
                    DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 2), pRadOut(2), pStartAng, pEndAng)


                ElseIf mTemplateNo = "1GenS" Or mTemplateNo = "1" Or mTemplateNo = "2" Then
                    '----------------------------------------------------------------------- 'SG 05APR11
                    'BG 04AUG09
                    'DrawLine(mY_Sym_Axis, mKP(mKP_Last + 1), mKP(mKP_Last + 3)) 'SB 30DEC09
                    'DrawLine(mY_Sym_Axis, mKP(mKP_Last + 2), mKP(mKP_Last + 4))

                    DXF_Lines_SymAxis(mY_Sym_Axis, mKP(mKP_Last + 1), mKP(mKP_Last + 3)) 'SG 04APR11
                    DXF_Lines_SymAxis(mY_Sym_Axis, mKP(mKP_Last + 2), mKP(mKP_Last + 4))

                End If

                '#End Zone

                '#Zone " Segment 3 "
                '   Segment 3
                '   ---------
                '
                Dim pThetaM3Draw As Single
                pThetaM3Draw = mThetaM(3) + mDThetaM3

                Dim pBetaM3 As Single
                pBetaM3 = mThetaM(3) - 90

                If POrient = "External" Then
                    '------------------------
                    pStartAng = -pBetaM3
                    pEndAng = 90

                ElseIf POrient = "Internal" Then
                    '----------------------------
                    pStartAng = -90
                    pEndAng = pBetaM3

                End If

                'BG 04AUG09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 3), pRadIn(3), pStartAng, pEndAng)  'SB 30DEC09
                'DrawArc(mY_Sym_Axis, mCenAdd(i, 3), pRadOut(3), pStartAng, pEndAng)

                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 3), pRadIn(3), pStartAng, pEndAng)
                DXF_Arcs_SymAxis(mY_Sym_Axis, mCenAdd(i, 3), pRadOut(3), pStartAng, pEndAng)

                '#End Zone

                '#End Zone

            End If

            '....Increse Last Key Point
            mKP_Last = mKP_Last + 6

        Next

    End Sub


#End Region



#End Region



    '-------------------------------------------------------------------------------
    '                         UTILITY ROUTINES - END                               '
    '-------------------------------------------------------------------------------


End Class
