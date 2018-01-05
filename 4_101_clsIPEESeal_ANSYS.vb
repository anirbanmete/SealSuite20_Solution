'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsESeal                               '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  24FEB16                                '
'                                                                              '
'===============================================================================

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

    'SECONDARY or INTERMEDIATE VARIABLES(For ANSYS):
    '-----------------------------------------------
    Private mKP(100) As PointF  '.....Keypoints: Seal.            
    ' 
    '.....Key Points:
    Dim mKP_RF(4) As PointF      '....Keypoints: Radial Flange.
    Dim mKP_AF(4) As PointF      '....Keypoints: Axial Flange.

    '....Holds the Last Key Point of Half Conv
    Dim mKP_Last As Int16
    Dim mCountAddHalfConv As Int16      '....# of Additional 1/2 Convolutions.

    '....Holds Additional Centre Points
    Dim mCenAdd(,) As PointF

    '....KeyPoints For Template = 2             SG 19APR11
    Private mKP_Tpl2(2) As PointF


#End Region

    '*******************************************************************************
    '*                        CLASS METHODS - BEGIN                                *
    '*******************************************************************************

#Region "KEY POINTS FOR TEMPLATE 1:"

    '-------------------------------------------------------------------------------
    '*                       KEY POINTS FOR TEMPLATE 1 - BEGIN                     *
    '-------------------------------------------------------------------------------

    Private Sub Calc_KP_T1()
        '===================
        '....TEMPLATE NO = "1".

        Try

            Calc_KP_RHS_EndHalfConv()       '....RHS END     - 1/2 Conv.
            Calc_KP_RHS_MidEndHalfConv()    '....RHS MID END - 1/2 Conv.

            If mNConv > 1 Then
                '....Holds Last Key Point Number of BASIC 1/2 Convolution
                mKP_Last = 16
                '....Holds Additional Half Conv Number
                mCountAddHalfConv = mNConv - 1

                '   Calculate Key Points of Additional MID MID - 1/2 Convolution
                Calc_KP_AddHalfConv(mCenE(1), mCenM(3))

            End If

            '   Calculate Key Points of Axial Flange
            '   ------------------------------------
            Calc_KP_AF()

            '   Calculate Key Points of Radial Flange
            '   -------------------------------------
            Calc_KP_RF()

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
            pstrMsg = "Error in KP calculation in JS File!!" & vbCrLf
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly

            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

            Exit Sub

        End Try

    End Sub


    Private Sub Calc_KP_RHS_EndHalfConv()
        '===============================

        '....Intermediate variables:
        Dim pRadP As Single
        Dim pRadM As Single

        '   End Convolution - RHS 1/2:
        '   --------------------------
        '
        '       Segment E1.
        '       -----------
        '
        '   ASSUMPTION for the following derivation for sign:
        '       The angle thetaE1 > 90.
        '
        '....Calculate plus & minus radii.
        pRadP = mRadE(1) + 0.5 * mTStd
        pRadM = mRadE(1) - 0.5 * mTStd


        '....Key Point 9.
        With mKP(9)

            If POrient = "External" Then
                .X = mCenE(1).X - pRadM
            ElseIf POrient = "Internal" Then
                .X = mCenE(1).X + pRadM
            End If

            .Y = mCenE(1).Y

        End With

        '....Key Point 10.
        With mKP(10)

            If POrient = "External" Then
                .X = mCenE(1).X - pRadP
            ElseIf POrient = "Internal" Then
                .X = mCenE(1).X + pRadP
            End If

            .Y = mCenE(1).Y

        End With

        '....Calculate the relevant geometric angles for drawing:       
        Dim pThetaE1Draw As Single
        '....Adjusted Geometry
        pThetaE1Draw = mThetaE(1) + mDThetaE1

        '....Angle made by E1-E2 junction with the horizontal. 
        Dim pBetaE1 As Single
        pBetaE1 = pThetaE1Draw - 90

        '....Key Point 7.
        With mKP(7)

            If POrient = "External" Then
                .X = mCenE(1).X + pRadM * SinD(pBetaE1)
            ElseIf POrient = "Internal" Then
                .X = mCenE(1).X - pRadM * SinD(pBetaE1)
            End If

            .Y = mCenE(1).Y - pRadM * CosD(pBetaE1)

        End With

        '....Key Point 8
        With mKP(8)

            If POrient = "External" Then
                .X = mCenE(1).X + pRadP * SinD(pBetaE1)
            ElseIf POrient = "Internal" Then
                .X = mCenE(1).X - pRadP * SinD(pBetaE1)
            End If

            .Y = mCenE(1).Y - pRadP * CosD(pBetaE1)

        End With



        '       Segment E2
        '       ----------.
        '
        pRadP = mRadE(2) + 0.5 * mTStd
        pRadM = mRadE(2) - 0.5 * mTStd

        '....The angle made by E2-E3 junction with the horizontal.
        Dim pAlphaE3 As Single
        pAlphaE3 = mThetaE(2) - pBetaE1

        '....Key Point 6.
        With mKP(6)

            If POrient = "External" Then
                .X = mCenE(2).X + pRadM * SinD(pAlphaE3)
            ElseIf POrient = "Internal" Then
                .X = mCenE(2).X - pRadM * SinD(pAlphaE3)
            End If

            .Y = mCenE(2).Y + pRadM * CosD(pAlphaE3)

        End With

        '....Key Point 5.
        With mKP(5)

            If POrient = "External" Then
                .X = mCenE(2).X + pRadP * SinD(pAlphaE3)
            ElseIf POrient = "Internal" Then
                .X = mCenE(2).X - pRadP * SinD(pAlphaE3)
            End If

            .Y = mCenE(2).Y + pRadP * CosD(pAlphaE3)

        End With


        '       Segment E3:
        '       -----------
        '
        '....Center Point. 

        pRadP = mRadE(3) + 0.5 * mTStd
        pRadM = mRadE(3) - 0.5 * mTStd

        '....Key Point 3.
        With mKP(3)
            .X = mCenE(3).X
            .Y = mCenE(3).Y - pRadM
        End With


        '....Key Point 4.
        With mKP(4)
            .X = mCenE(3).X
            .Y = mCenE(3).Y - pRadP
        End With


        '....Key Point 1.
        With mKP(1)
            If POrient = "External" Then
                .X = mCenE(3).X + pRadM * SinD(mThetaE(3))
            ElseIf POrient = "Internal" Then
                .X = mCenE(3).X - pRadM * SinD(mThetaE(3))
            End If
            .Y = mCenE(3).Y - pRadM * CosD(mThetaE(3))
        End With


        '....Key Point 2.

        With mKP(2)

            If POrient = "External" Then
                .X = mCenE(3).X + pRadP * SinD(mThetaE(3))
            ElseIf POrient = "Internal" Then
                .X = mCenE(3).X - pRadP * SinD(mThetaE(3))
            End If

            .Y = mCenE(3).Y - pRadP * CosD(mThetaE(3))
        End With

    End Sub


    Private Sub Calc_KP_RHS_MidEndHalfConv()
        '===================================

        '....Intermediate variables:
        Dim pRadP As Single
        Dim pRadM As Single

        '   Mid Convolution - RHS 1/2:
        '   -------------------------
        '
        '       Segment M1.
        '       -----------
        '
        '....Calculate the relevant geometric angles for drawing:       
        Dim pThetaM1Draw As Single
        '....Adjusted Geometry.
        pThetaM1Draw = mThetaM(1) + mDThetaM1

        '....Angle made by radial line thru' M1-M2 junction with the horizontal. 
        Dim pBetaM1 As Single
        pBetaM1 = pThetaM1Draw - 90

        pRadP = mRadM(1) + 0.5 * mTStd
        pRadM = mRadM(1) - 0.5 * mTStd

        '....Key Point 11.
        With mKP(11)

            If POrient = "External" Then
                .X = mCenE(1).X + pRadM * SinD(pBetaM1)
            ElseIf POrient = "Internal" Then
                .X = mCenE(1).X - pRadM * SinD(pBetaM1)
            End If

            .Y = mCenE(1).Y + pRadM * CosD(pBetaM1)

        End With

        '....Key Point 12.
        With mKP(12)

            If POrient = "External" Then
                .X = mCenE(1).X + pRadP * SinD(pBetaM1)
            ElseIf POrient = "Internal" Then
                .X = mCenE(1).X - pRadP * SinD(pBetaM1)
            End If

            .Y = mCenE(1).Y + pRadP * CosD(pBetaM1)

        End With


        '       Segment M2 (Straight).
        '       ----------------------
        '
        '   ASSUMPTION for the following derivation for sign:
        '       The angle ThetaM1Draw > 90.
        '
        Dim pThetaM3Draw As Single
        pThetaM3Draw = mThetaM(3) + mDThetaM3

        '....Angle made by radial line thru' M1-M2 junction with the horizontal. 
        Dim pBetaM3 As Single
        pBetaM3 = pThetaM3Draw - 90

        pRadP = mRadM(3) + 0.5 * mTStd
        pRadM = mRadM(3) - 0.5 * mTStd


        '....Key Point 13.
        With mKP(13)

            If POrient = "External" Then
                .X = mCenM(3).X - pRadP * SinD(pBetaM3)
            ElseIf POrient = "Internal" Then
                .X = mCenM(3).X + pRadP * SinD(pBetaM3)
            End If

            .Y = mCenM(3).Y - pRadP * CosD(pBetaM3)

        End With

        '....Key Point 14.     
        With mKP(14)

            If POrient = "External" Then
                .X = mCenM(3).X - pRadM * SinD(pBetaM3)
            ElseIf POrient = "Internal" Then
                .X = mCenM(3).X + pRadM * SinD(pBetaM3)
            End If

            .Y = mCenM(3).Y - pRadM * CosD(pBetaM3)

        End With
        '       Segment M3
        '       ----------
        '
        '....Angle made by radial line thru' M2-M3 junction with the horizontal. 
        pBetaM3 = pThetaM3Draw - 90

        pRadP = mRadM(3) + 0.5 * mTStd
        pRadM = mRadM(3) - 0.5 * mTStd

        '....Key Point 15,
        With mKP(15)

            If POrient = "External" Then
                .X = mCenM(3).X + pRadP
            ElseIf POrient = "Internal" Then
                .X = mCenM(3).X - pRadP
            End If

            .Y = mCenM(3).Y

        End With

        '....Key Point 16,
        With mKP(16)

            If POrient = "External" Then
                .X = mCenM(3).X + pRadM
            ElseIf POrient = "Internal" Then
                .X = mCenM(3).X - pRadM
            End If

            .Y = mCenM(3).Y

        End With

    End Sub


    Private Sub Calc_KP_AddHalfConv(ByVal Cen1_In As PointF, ByVal Cen2_In As PointF)
        '============================================================================
        '....Reinitialize Additional Center points
        ReDim mCenAdd(mCountAddHalfConv, 2)

        '....Initialize 
        mCenAdd(0, 1) = Cen1_In
        mCenAdd(0, 2) = Cen2_In

        '....Holds Pitch for Additional Convolution
        Dim pPitchM As Single

        If TemplateNo = "1" Then
            pPitchM = mPitchM
        ElseIf TemplateNo = "1Gen" Or TemplateNo = "1GenS" Then
            pPitchM = mPitchM_Mid

        End If

        Dim i As Int16
        For i = 1 To mCountAddHalfConv
            '-------------------------
            '
            '....Center 1
            mCenAdd(i, 1) = mCenAdd(i - 1, 2)
            '
            '....Center 2
            With mCenAdd(i, 2)
                .X = mCenAdd(i - 1, 1).X
                .Y = mCenAdd(i - 1, 1).Y + pPitchM
            End With
            '
            '....Holds the location of Symmetry Axis
            Dim pY_SymAxis As Single
            '
            '....Gets the location of Symmetry Axis
            pY_SymAxis = mCenAdd(i, 1).Y

            Dim j As Int16
            For j = 1 To 3

                Dim m As Int16
                For m = 1 To 2

                    Dim pKP_Ref As Int16    '....Holds the Ref. Key Points of
                    '               '.... (i-1) convolution
                    '               '.... in 1st iteration it is Last key Point
                    '               '.... in 2nd iteration it is Last key Point - 1

                    Dim pY1_SymDist As Single      '....Holds Symmetry distance of RHS
                    Dim pY1_SymDist_Refl As Single '....Holds Symmetry Reflection in LHS

                    pKP_Ref = mKP_Last - (m - 1)
                    pY1_SymDist = mKP(pKP_Ref - j * 2).Y - pY_SymAxis
                    pY1_SymDist_Refl = -pY1_SymDist

                    With mKP(pKP_Ref + j * 2)
                        .X = mKP(pKP_Ref - j * 2).X
                        .Y = pY_SymAxis + pY1_SymDist_Refl    '....Transformation of coordinates. Y1 ==> Y.
                    End With

                Next

            Next

            mKP_Last = mKP_Last + 6
        Next


    End Sub


    Private Sub Calc_KP_AF()
        '===================

        '....Holds Convolution Number 
        Dim pNConv As Int16
        pNConv = mNConv

        Dim pKP_Ref_X As Int16  '....Holds the reference Key Point  
        '                                           '....number from which Axial Flange 
        '                                           '....should draw
        pKP_Ref_X = 10

        If pNConv > 1 Then
            '=============

            '....Holds X Co-Ordinate KP Ref number
            If POrient = "Internal" Then
                '-----------------------
                If mKP(10).X > mKP(22).X Then
                    pKP_Ref_X = 10
                Else
                    pKP_Ref_X = 22
                End If

            ElseIf POrient = "External" Then
                '---------------------------
                If mKP(10).X > mKP(22).X Then
                    pKP_Ref_X = 22
                Else
                    pKP_Ref_X = 10
                End If

            End If

        End If

        Dim pKP_Y As Single    '....Holds Y Co-Ordinate Value  'SB 17MAR08

        '....Check Y Co-Ordinate Value of KP 8 & 4
        If mKP(4).Y > mKP(8).Y Then
            pKP_Y = mCenE(1).Y - mRadE(1) - 0.5 * TStd         'SB 17MAR08
        Else
            pKP_Y = mKP(4).Y                                   'SB 17MAR08
        End If

        '       Axial Flange:
        '       ------------
        '
        '....Key Point 104,
        With mKP_AF(4)

            If POrient = "External" Then
                .X = mKP(pKP_Ref_X).X + 0.1 * mTStd
            ElseIf POrient = "Internal" Then
                .X = mKP(pKP_Ref_X).X - 0.1 * mTStd
            End If

            .Y = pKP_Y 'mKP(pKP_Ref_Y).Y '- 0.005              'SB 17MAR08
        End With

        '....Key Point 103,
        With mKP_AF(3)
            .X = mKP_AF(4).X
            .Y = mKP_AF(4).Y - 2 * mTStd
        End With

        '....Key Point 101,
        With mKP_AF(1)

            If POrient = "External" Then
                .X = mKP(2).X + mTStd
            ElseIf POrient = "Internal" Then
                .X = mKP(2).X - mTStd
            End If

            .Y = mKP_AF(4).Y

        End With

        '....Key Point 102,
        With mKP_AF(2)
            .X = mKP_AF(1).X
            .Y = mKP_AF(3).Y
        End With

    End Sub


    Private Sub Calc_KP_RF()
        '===================

        '       Radial Flange
        '       ------------
        '
        '....Key Point 201,
        With mKP_RF(1)

            If POrient = "External" Then
                '.X = mKP_AF(4).X - mZClear         'PB 07SEP09
                .X = mKP_AF(4).X - 0.5 * mZClear

            ElseIf POrient = "Internal" Then
                '.X = mKP_AF(4).X + mZClear         'PB 07SEP09
                .X = mKP_AF(4).X + 0.5 * mZClear
            End If

            .Y = mKP_AF(4).Y

        End With

        '....Key Point 202,
        With mKP_RF(2)

            If POrient = "External" Then
                .X = mKP_RF(1).X - 2 * mTStd
            ElseIf POrient = "Internal" Then
                .X = mKP_RF(1).X + 2 * mTStd
            End If

            .Y = mKP_RF(1).Y

        End With

        '....Holds Y Co-ordinate value
        Dim pY_Flange As Single

        If mTemplateNo = "1Gen" Or mTemplateNo = "1GenS" Then
            '================================================
            pY_Flange = (mNConv - 1) * mPitchM_Mid * 0.5 + mPitch_Half_M_End

        ElseIf mTemplateNo = "1" Then
            pY_Flange = mNConv * mPitchM * 0.5

        End If

        '....Key Point 204,
        With mKP_RF(4)
            .X = mKP_RF(1).X
            .Y = mCenE(1).Y + pY_Flange
        End With

        '....Key Point 203,
        With mKP_RF(3)
            .X = mKP_RF(2).X
            .Y = mKP_RF(4).Y
        End With

    End Sub

    '-------------------------------------------------------------------------------
    '*                        KEY POINTS FOR TEMPLATE 1 - END                      *
    '-------------------------------------------------------------------------------

#End Region

#Region "KEY POINTS FOR TEMPLATE 2:" 'SG 19APR11
    '-------------------------------------------------------------------------------
    '*                      KEY POINTS FOR TEMPLATE 2 - BEGIN                   *
    '-------------------------------------------------------------------------------
    Private Sub Calc_KP_T2()
        '===================
        Calc_KP_T1()

        'Draw the straight segment.     Move to clsESeal_ANSYS               
        '-------------------------
        'TemplateNo = 2 : Flat Segment

        '....Calculate the relevant geometric angles for drawing:       
        Dim pThetaE1Draw As Single
        '....Adjusted Geometry
        pThetaE1Draw = mThetaE(1) + mDThetaE1

        '....Angle made by E1-E2 junction with the horizontal. 
        Dim pBetaE1 As Single
        pBetaE1 = pThetaE1Draw - 90


        With mKP_Tpl2(1)
            If POrient = "External" Then
                .X = mKP(7).X + mLFlatE * CosD(pBetaE1)
            ElseIf POrient = "Internal" Then
                .X = mKP(7).X - mLFlatE * CosD(pBetaE1)
            End If

            .Y = mKP(7).Y + mLFlatE * SinD(pBetaE1)
        End With

        With mKP_Tpl2(2)
            If POrient = "External" Then
                .X = mKP(8).X + mLFlatE * CosD(pBetaE1)
            ElseIf POrient = "Internal" Then
                .X = mKP(8).X - mLFlatE * CosD(pBetaE1)
            End If
            .Y = mKP(8).Y + mLFlatE * SinD(pBetaE1)

        End With

    End Sub

    '-------------------------------------------------------------------------------
    '*                      KEY POINTS FOR TEMPLATE 2 - END                        *
    '-------------------------------------------------------------------------------

#End Region

#Region "KEY POINTS FOR TEMPLATE 1GEN:"

    '-------------------------------------------------------------------------------
    '*                      KEY POINTS FOR TEMPLATE 1GEN - BEGIN                   *
    '-------------------------------------------------------------------------------

    Private Sub Calc_KP_T1Gen()
        '======================  

        '....Recalculate some of the "DERIVED" member variable set, using the latest 
        '........geometry variables.
        ''Calc_MemberVariables("STD")         'PB 22MAR08.
        Calc_MemberVariables("ADJ")           'PB 14APR10. 

        Try
            'COORDINATE CALCULATIONS:
            '========================
            '
            '....Holds Convolution Number 
            Dim pNConv As Int16
            pNConv = mNConv

            '   Calculate Key Points of RHS END - 1/2 Convolution
            '   -------------------------------------------------
            Calc_KP_RHS_EndHalfConv()

            '   Calculate Key Points of RHS MID END - 1/2 Convolution
            '   -----------------------------------------------------    
            Calc_KP_RHS_MidEndHalfConv()

            If pNConv >= 2 Then

                '   Calculate Key Points of RHS MID MID - 1/2 Convolution
                '   -----------------------------------------------------
                Calc_KP_RHS_MidMidHalfConv_T1Gen()

                If pNConv > 2 Then
                    '==================

                    '....Holds Last Key Point Number of BASIC 1/2 Convolution
                    mKP_Last = 22
                    '....Holds Additional Half Conv Number
                    mCountAddHalfConv = mNConv - 2

                    '   Calculate Key Points of Additional - 1/2 Convolution
                    '   -----------------------------------------------------
                    Calc_KP_AddHalfConv(mCenM(3), mCenM(5))

                End If
            End If

            '   Calculate Key Points on Axial Flange
            '   ------------------------------------
            Calc_KP_AF()

            '   Calculate Key Points on Radial Flange
            '   -------------------------------------
            Calc_KP_RF()

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
            pstrMsg = "Error in KP Generation in KP File!!" & vbCrLf
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly

            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

            Exit Sub

        End Try

    End Sub


    Private Sub Calc_KP_RHS_MidMidHalfConv_T1Gen()
        '=========================================

        '....Intermediate variables:
        Dim pRadP As Single
        Dim pRadM As Single

        '   Mid-Mid Convolution - RHS 1/2:
        '   -----------------------------
        '
        '       Segment M5.
        '       -----------
        '
        '....Calculate the relevant geometric angles for drawing:   
        Dim pThetaM5Draw As Single
        pThetaM5Draw = mThetaM(5) + mDThetaM5

        '....Angle made by radial line thru' M2-M3 junction with the horizontal. 
        Dim pBetaM5 As Single
        pBetaM5 = pThetaM5Draw - 90

        pRadP = mRadM(5) + 0.5 * mTStd
        pRadM = mRadM(5) - 0.5 * mTStd

        '....Key Point 19.
        With mKP(19)

            If POrient = "External" Then
                .X = mCenM(5).X + pRadM * SinD(pBetaM5)
            ElseIf POrient = "Internal" Then
                .X = mCenM(5).X - pRadM * SinD(pBetaM5)
            End If

            .Y = mCenM(5).Y - pRadM * CosD(pBetaM5)

        End With

        '....Key Point 20.
        With mKP(20)

            If POrient = "External" Then
                .X = mCenM(5).X + pRadP * SinD(pBetaM5)
            ElseIf POrient = "Internal" Then
                .X = mCenM(5).X - pRadP * SinD(pBetaM5)
            End If

            .Y = mCenM(5).Y - pRadP * CosD(pBetaM5)

        End With

        '       Segment M2 
        '       ----------
        '   
        '
        '....Calculate the relevant geometric angles for drawing:       
        Dim pThetaM3Draw As Single
        pThetaM3Draw = mThetaM(3) + mDThetaM3

        '....Angle made by radial line thru' M2-M3 junction with the horizontal. 
        Dim pBetaM3 As Single
        pBetaM3 = pThetaM3Draw - 90

        pRadP = mRadM(3) + 0.5 * mTStd
        pRadM = mRadM(3) - 0.5 * mTStd

        '....Key Point 17.
        With mKP(17)

            If POrient = "External" Then
                .X = mCenM(3).X - pRadP * SinD(pBetaM3)
            ElseIf POrient = "Internal" Then
                .X = mCenM(3).X + pRadP * SinD(pBetaM3)
            End If

            .Y = mCenM(3).Y + pRadP * CosD(pBetaM3)

        End With

        '....Key Point 18.     
        With mKP(18)

            If POrient = "External" Then
                .X = mCenM(3).X - pRadM * SinD(pBetaM3)
            ElseIf POrient = "Internal" Then
                .X = mCenM(3).X + pRadM * SinD(pBetaM3)
            End If

            .Y = mCenM(3).Y + pRadM * CosD(pBetaM3)

        End With

        '       Segment M3
        '       ----------
        pRadP = mRadM(5) + 0.5 * mTStd
        pRadM = mRadM(5) - 0.5 * mTStd

        '....Key Point 21.
        With mKP(21)

            If POrient = "External" Then
                .X = mCenM(5).X - pRadM
            ElseIf POrient = "Internal" Then
                .X = mCenM(5).X + pRadM
            End If

            .Y = mCenM(5).Y

        End With

        '....Key Point 22.
        With mKP(22)

            If POrient = "External" Then
                .X = mCenM(5).X - pRadP
            ElseIf POrient = "Internal" Then
                .X = mCenM(5).X + pRadP
            End If

            .Y = mCenM(5).Y

        End With
    End Sub

    '-------------------------------------------------------------------------------
    '*                   CALCULATE KEY POINTS FOR TEMPLATE 1GEN - END              *
    '-------------------------------------------------------------------------------

#End Region

#Region "WRITE JSCRIPT FILE:"

    '-------------------------------------------------------------------------------
    '*                   WRITING JSCRIPT FILE ROUTINES - BEGIN                     *
    '-------------------------------------------------------------------------------

    Public Sub WriteFile_DM_JS_T1(ByVal JS_FileName_In As String)   'SB 22FEB08
        '========================================================      

        Dim pSW As StreamWriter = Nothing

        Try

            'COORDINATE CALCULATIONS:
            '========================
            '
            Calc_KP_T1()

            'WRITE JS FILE.
            '==============
            '
            pSW = File.CreateText(JS_FileName_In)

            With pSW

                .WriteLine(" function planeSketchesOnly (p)")
                .WriteLine(" {")
                .WriteLine(" //Plane")
                .WriteLine(" p.Plane  = agb.GetActivePlane();")
                .WriteLine(" p.Origin = p.Plane.GetOrigin();")
                .WriteLine(" p.XAxis  = p.Plane.GetXAxis();")
                .WriteLine(" p.YAxis  = p.Plane.GetYAxis();")
                .WriteLine()


                ' ''   Construction Lines & Points: 
                ' ''   ----------------------------
                ' ''
                ''.WriteLine(" //Sketch")
                ''.WriteLine(" p.Sk1 = p.Plane.NewSketch();")
                ''.WriteLine(" p.Sk1.Name = ""Construction"";")
                ''.WriteLine()

                ''.WriteLine(" //Edges")
                ''.WriteLine(" with (p.Sk1)")
                ''.WriteLine(" {")

                ''.WriteLine(" p.Ln14 = Line(" & LineStr(mCenE(1), mCenE(2)) & ");")
                ''.WriteLine(" p.Ln15 = Line(" & LineStr(mCenE(2), mCenE(3)) & ");")

                ' ''....Intermediate variables,
                ''Dim pPt1 As PointF, pPt2 As PointF

                ''With pPt2
                ''    .X = mCenE(3).X
                ''    .Y = mCenE(2).Y
                ''End With
                ''.WriteLine(" p.Ln16 = Line(" & LineStr(mCenE(3), pPt2) & ");")

                ''With pPt2
                ''    .X = mCenE(3).X + Abs(mCenE(2).Y) * TanD(mThetaE(3))
                ''    .Y = mCenE(2).Y
                ''End With
                ''.WriteLine(" p.Ln17 = Line(" & LineStr(mCenE(3), pPt2) & ");")

                ''With pPt1
                ''    .X = 0.0
                ''    .Y = 0.0
                ''End With
                ''.WriteLine(" p.Ln13 = Line(" & LineStr(pPt1, mCenE(1)) & ");")
                ''.WriteLine(" p.Ln21 = Line(" & LineStr(mCenM(3), mKP(15)) & ");")
                ''.WriteLine(" p.Ln22 = Line(" & LineStr(mCenM(3), mKP(13)) & ");")
                ''.WriteLine(" p.Ln18 = Line(" & LineStr(mCenE(2), mKP(7)) & ");")
                ''.WriteLine(" p.Ln20 = Line(" & LineStr(mCenE(3), mKP(5)) & ");")
                ''.WriteLine(" }")


                '   End Convolution - RHS 1/2: 
                '   --------------------------
                '
                .WriteLine(" //Sketch")
                .WriteLine(" p.Sk2 = p.Plane.NewSketch();")
                .WriteLine(" p.Sk2.Name = ""ESeal_EHalf_Outline"";")
                .WriteLine()
                .WriteLine(" //Edges")
                .WriteLine(" with (p.Sk2)")
                .WriteLine(" {")

                '....Segment 1.
                .WriteLine(" p.Cr1 = ArcCtrEdge(" & ArcStr(mCenE(1), _
                mKP(9), mKP(7)) & ");")
                .WriteLine(" p.Cr2 = ArcCtrEdge(" & ArcStr(mCenE(1), _
                mKP(10), mKP(8)) & ");")


                '....Segment 2.
                .WriteLine(" p.Cr5 = ArcCtrEdge(" & ArcStr(mCenE(2), _
                mKP(6), mKP(8)) & ");")
                .WriteLine(" p.Cr6 = ArcCtrEdge(" & ArcStr(mCenE(2), _
                mKP(5), mKP(7)) & ");")


                '....Draw Segment 3
                .WriteLine(" p.Cr7 = ArcCtrEdge(" & ArcStr(mCenE(3), _
                mKP(6), mKP(4)) & ");")
                .WriteLine(" p.Cr8 = ArcCtrEdge(" & ArcStr(mCenE(3), _
                mKP(5), mKP(3)) & ");")
                .WriteLine(" p.Cr9 = ArcCtrEdge(" & ArcStr(mCenE(3), _
                mKP(3), mKP(1)) & ");")
                .WriteLine(" p.Cr10 = ArcCtrEdge(" & ArcStr(mCenE(3), _
                mKP(4), mKP(2)) & ");")

                .WriteLine(" p.Ln1 = Line(" & LineStr(mKP(1), mKP(2)) & ");")
                .WriteLine(" p.Ln1 = Line(" & LineStr(mKP(9), mKP(10)) & ");")

                .WriteLine(" }")

                '   Mid Convolution - 1st RHS Half (Pattern): 
                '   -----------------------------------------
                '
                .WriteLine(" //Sketch")
                .WriteLine(" p.Sk3 = p.Plane.NewSketch();")
                .WriteLine(" p.Sk3.Name = ""ESeal_MHalf_Outline"";")
                .WriteLine()
                .WriteLine(" //Edges")
                .WriteLine(" with (p.Sk3)")
                .WriteLine(" {")

                '....Segment 1.
                .WriteLine(" p.Cr3 = ArcCtrEdge(" & ArcStr(mCenE(1), _
                mKP(11), mKP(9)) & ");")
                .WriteLine(" p.Cr4 = ArcCtrEdge(" & ArcStr(mCenE(1), _
                mKP(12), mKP(10)) & ");")

                '....Segment 2 (Straight).
                .WriteLine(" p.Ln2 = Line(" & LineStr(mKP(11), mKP(13)) & ");")
                .WriteLine(" p.Ln3 = Line(" & LineStr(mKP(12), mKP(14)) & ");")

                '....Segment 3.
                .WriteLine(" p.Cr11 = ArcCtrEdge(" & ArcStr(mCenM(3), _
                mKP(14), mKP(16)) & ");")
                .WriteLine(" p.Cr12 = ArcCtrEdge(" & ArcStr(mCenM(3), _
                mKP(13), mKP(15)) & ");")

                If mNConv = 1 Then
                    .WriteLine(" p.Ln4 = Line(" & LineStr(mKP(15), mKP(16)) & ");")
                End If
                .WriteLine(" p.Ln19 = Line(" & LineStr(mKP(9), mKP(10)) & ");")

                Dim k As Int16 = 22

                '   Draw Additional 1/2 Conv 
                '   ------------------------

                If mNConv > 1 Then
                    '==================
                    mKP_Last = 16

                    Dim i As Int16
                    For i = 1 To mCountAddHalfConv
                        '===================

                        '   Segment 1:
                        '   ----------
                        If i Mod 2 = 0 Then
                            '==============
                            '...Draw Arc 1.
                            .WriteLine(" p.Cr" & k & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 1), mKP(mKP_Last + 2), _
                                mKP(mKP_Last)) & ");")
                            '...Draw Arc 2.
                            .WriteLine(" p.Cr" & k + 1 & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 1), mKP(mKP_Last + 1), _
                                mKP(mKP_Last - 1)) & ");")

                        ElseIf i Mod 2 <> 0 Then
                            '===================
                            '...Draw Arc 1.
                            .WriteLine(" p.Cr" & k & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 1), mKP(mKP_Last), _
                                mKP(mKP_Last + 2)) & ");")
                            '...Draw Arc 2.
                            .WriteLine(" p.Cr" & k + 1 & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 1), mKP(mKP_Last - 1), _
                                mKP(mKP_Last + 1)) & ");")
                        End If

                        '   Segment 2(Straight Segment):
                        '   ----------------------------
                        '...Draw Line 1.
                        .WriteLine(" p.Ln" & k + 2 & " = Line(" & _
                            LineStr(mKP(mKP_Last + 1), mKP(mKP_Last + 3)) & ");")
                        '...Draw Line 2.
                        .WriteLine(" p.Ln" & k + 3 & " = Line(" & _
                            LineStr(mKP(mKP_Last + 2), mKP(mKP_Last + 4)) & ");")


                        '   Segment 3:
                        '   ----------
                        If i Mod 2 = 0 Then
                            '==============
                            '...Draw Arc 1.
                            .WriteLine(" p.Cr" & k + 4 & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 2), mKP(mKP_Last + 3), _
                                mKP(mKP_Last + 5)) & ");")
                            '...Draw Arc 2.
                            .WriteLine(" p.Cr" & k + 5 & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 2), mKP(mKP_Last + 4), _
                                mKP(mKP_Last + 6)) & ");")

                        ElseIf i Mod 2 <> 0 Then
                            '===================
                            '...Draw Arc 1.
                            .WriteLine(" p.Cr" & k + 4 & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 2), mKP(mKP_Last + 5), _
                                mKP(mKP_Last + 3)) & ");")
                            '...Draw Arc 2.
                            .WriteLine(" p.Cr" & k + 5 & " = ArcCtrEdge(" & _
                                ArcStr(mCenAdd(i, 2), mKP(mKP_Last + 6), _
                                mKP(mKP_Last + 4)) & ");")
                        End If

                        k = k + 7
                        mKP_Last = mKP_Last + 6

                        If i = mCountAddHalfConv Then
                            .WriteLine(" p.Ln" & k + 6 & " = Line(" & _
                                LineStr(mKP(mKP_Last), mKP(mKP_Last - 1)) & ");")
                        End If

                    Next
                End If

                .WriteLine(" }")

                '   Axial Flange:
                '   -------------
                '
                .WriteLine(" //Sketch")
                .WriteLine(" p.Sk4 = p.Plane.NewSketch();")
                .WriteLine(" p.Sk4.Name = ""AxialFlange_Outline"";")
                .WriteLine()
                .WriteLine(" //Edges")
                .WriteLine(" with (p.Sk4)")
                .WriteLine(" {")

                .WriteLine(" p.Ln5 = Line(" & LineStr(mKP_AF(4), mKP_AF(1)) & ");")
                .WriteLine(" p.Ln6 = Line(" & LineStr(mKP_AF(4), mKP_AF(3)) & ");")
                .WriteLine(" p.Ln7 = Line(" & LineStr(mKP_AF(3), mKP_AF(2)) & ");")
                .WriteLine(" p.Ln8 = Line(" & LineStr(mKP_AF(1), mKP_AF(2)) & ");")

                .WriteLine(" }")


                '   Radial Flange.  
                '   --------------
                '
                .WriteLine(" //Sketch")
                .WriteLine(" p.Sk5 = p.Plane.NewSketch();")
                .WriteLine(" p.Sk5.Name = ""RadialFlange_Outline"";")
                .WriteLine()
                .WriteLine(" //Edges")
                .WriteLine(" with (p.Sk5)")
                .WriteLine(" {")

                .WriteLine(" p.Ln9 = Line(" & LineStr(mKP_RF(4), mKP_RF(1)) _
                & ");")
                .WriteLine(" p.Ln10 = Line(" & LineStr(mKP_RF(1), mKP_RF(2)) _
                & ");")
                .WriteLine(" p.Ln11 = Line(" & LineStr(mKP_RF(2), mKP_RF(3)) _
                & ");")
                .WriteLine(" p.Ln12 = Line(" & LineStr(mKP_RF(3), mKP_RF(4)) _
                & ");")

                .WriteLine(" }")

                .WriteLine("return p;")
                .WriteLine(" } //End Plane JScript function: planeSketchesOnly")
                .WriteLine()
                .WriteLine("//Call Plane JScript function")
                .WriteLine("var ps1 = planeSketchesOnly (new Object());")
                .WriteLine()
                .WriteLine("//Finish")
                .WriteLine("agb.Regen(); //To insure model validity")
                .WriteLine("//End DM JScript")
            End With

            '....End of Java Script

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

    '-------------------------------------------------------------------------------
    '*                     WRITING JSCRIPT FILE ROUTINES - END                     *
    '-------------------------------------------------------------------------------

#End Region

#Region "WRITE KEY POINT FILE:"

    '-------------------------------------------------------------------------------
    '*                          KEY POINT FILES WRITE - BEGIN                      *
    '-------------------------------------------------------------------------------


    Public Sub WriteFile_KP_T1Gen(ByVal Unit_In As IPE_clsUnit, ByVal ANSYS_In As IPE_clsANSYS)
        '==============================================================================    
        '....This Routine creates the Key Point File for "1Gen" (& "1GenS") template, 
        '........which is subsequently read by the ANSYS program upon execution.

        Dim pSW As StreamWriter = Nothing

        Try
            'COORDINATE CALCULATIONS:
            '========================
            '
            CalC_KP_T1Gen()

            'WRITE KEY POINT FILE.
            '=====================
            '
            pSW = File.CreateText(ANSYS_In.ESealKP_FileName)

            With pSW

                .WriteLine("/COM, Key Points Coordinate File.")
                .WriteLine("/COM, File Name = ESeal_KP_T1Gen_V1.txt.")
                .WriteLine("/COM, Written by: SealIPE")
                .WriteLine(" ")
                .WriteLine("/COM, HEADER INFORMATION:")
                .WriteLine("/COM, ===================")
                .WriteLine("!")
                .WriteLine(vbTab & "unitSys     = '" & Unit_In.System & "'")
                .WriteLine(vbTab & "SealType    = '" & Type & "'")
                .WriteLine(vbTab & "CrossSecNo  = '" & MCrossSecNo & "'")
                .WriteLine(vbTab & "Adjusted    = '" & Adjusted & "'")
                .WriteLine(vbTab & "POrient     = '" & POrient & "'")
                .WriteLine(vbTab & "ANSYS_Model = '" & ANSYS_In.Model & "'")
                .WriteLine(vbTab & "TemplateNo  = '" & mTemplateNo & "'")
                .WriteLine(vbTab & "NConv       = " & mNConv)
                .WriteLine("!")
                .WriteLine("/COM, ===================")
                .WriteLine(" ")

                '   Centre Point Variable
                '   ---------------------
                .WriteLine("/COM, Auxiliary KeyPoints @ Center Points:")
                .WriteLine("/COM, ====================================")
                .WriteLine("/COM,")
                .WriteLine("/COM, End Conv - RHS 1/2:")
                .WriteLine("/COM, -------------------")
                .WriteLine("kOffCenE = 300")
                .WriteLine("kCENE1 = kOffCenE + 1")
                .WriteLine("kCENE2 = kOffCenE + 2")
                .WriteLine("kCENE3 = kOffCenE + 3")
                .WriteLine(" ")
                '....Center Point of Segment E1:
                .WriteLine("K, kCENE1, " & LPFormat_KP(Unit_In, mCenE(1)))
                '....Center Point of Segment E2:
                .WriteLine("K, kCENE2, " & LPFormat_KP(Unit_In, mCenE(2)))
                '....Center Point of Segment E3:
                .WriteLine("K, kCENE3, " & LPFormat_KP(Unit_In, mCenE(3)))
                .WriteLine(" ")
                .WriteLine("/COM, Mid-end Conv - RHS 1/2 & Mid-mid Conv - LHS 1/2:")
                .WriteLine("/COM, ------------------------------------------------")
                .WriteLine("kOffCenM = 400")

                Dim i As Int16
                If mTemplateNo = "1Gen" Then
                    For i = 2 To 5
                        .WriteLine("kCENM" & i & " = " & "kOffCenM+" & i)
                    Next
                Else
                    For i = 3 To 5
                        If i Mod 2 <> 0 Then
                            .WriteLine("kCENM" & i & " = " & "kOffCenM+" & i)
                        End If
                    Next
                End If

                .WriteLine(" ")
                '....Centre Point of Segment M2:
                If mTemplateNo = "1Gen" Then
                    .WriteLine("K, kCENM2, " & LPFormat_KP(Unit_In, mCenM(2)))
                End If
                '....Centre Point of Segment M3:
                .WriteLine("K, kCENM3, " & LPFormat_KP(Unit_In, mCenM(3)))
                '....Centre Point of Segment M4:
                If mTemplateNo = "1Gen" Then

                    .WriteLine("K, kCENM4, " & LPFormat_KP(Unit_In, mCenM(4)))
                End If
                '....Centre Point of Segment M5:
                .WriteLine("K, kCENM5, " & LPFormat_KP(Unit_In, mCenM(5)))
                .WriteLine(" ")

                '   End Conv - RHS 1/2:
                '   -------------------
                '
                '       Segment E1.
                '       -----------
                '
                '   ASSUMPTION for the following derivation for sign:
                '       The angle thetaE1 > 90.
                '
                .WriteLine("/COM, Seal Keypoints:")
                .WriteLine("/COM, ===============")
                .WriteLine("/COM, ")
                .WriteLine("/COM, End Conv - RHS 1/2:")
                .WriteLine("/COM, -------------------")
                .WriteLine("/COM, ")
                .WriteLine("/COM,   Segment E1:")
                '....Key Point 9
                .WriteLine("K,  9, " & LPFormat_KP(Unit_In, mKP(9)))
                '....Key Point 10
                .WriteLine("K, 10, " & LPFormat_KP(Unit_In, mKP(10)))
                '....Key Point 7
                .WriteLine("K,  7, " & LPFormat_KP(Unit_In, mKP(7)))
                '....Key Point 8
                .WriteLine("K,  8, " & LPFormat_KP(Unit_In, mKP(8)))
                .WriteLine(" ")

                '       Segment E2
                '       ----------.
                '
                .WriteLine("/COM,   Segment E2:")
                '....Key Point 5
                .WriteLine("K,  5, " & LPFormat_KP(Unit_In, mKP(5)))
                '....Key Point 6
                .WriteLine("K,  6, " & LPFormat_KP(Unit_In, mKP(6)))
                .WriteLine(" ")

                '       Segment E3
                '       ----------.
                '
                .WriteLine("/COM,   Segment E3:")
                '....Key Point 3
                .WriteLine("K,  3, " & LPFormat_KP(Unit_In, mKP(3)))
                '....Key Point 4
                .WriteLine("K,  4, " & LPFormat_KP(Unit_In, mKP(4)))
                '....Key Point 1
                .WriteLine("K,  1, " & LPFormat_KP(Unit_In, mKP(1)))
                '....Key Point 2
                .WriteLine("K,  2, " & LPFormat_KP(Unit_In, mKP(2)))
                .WriteLine(" ")

                '   Mid-End Conv - RHS 1/2:
                '   -----------------------
                '
                '       Segment M1.
                '       -----------
                .WriteLine("/COM, Mid-end Conv - RHS 1/2:")
                .WriteLine("/COM, -----------------------")
                .WriteLine("/COM, ")
                .WriteLine("/COM,   Segment M1:")
                '....Key Point 11
                .WriteLine("K, 11, " & LPFormat_KP(Unit_In, mKP(11)))
                '....Key Point 12
                .WriteLine("K, 12, " & LPFormat_KP(Unit_In, mKP(12)))
                .WriteLine(" ")


                '       Segment M2 
                '       ----------
                '
                .WriteLine("/COM,   Segment M2:")
                '....Key Point 13
                .WriteLine("K, 13, " & LPFormat_KP(Unit_In, mKP(13)))
                '....Key Point 14
                .WriteLine("K, 14, " & LPFormat_KP(Unit_In, mKP(14)))
                .WriteLine(" ")

                '       Segment M3 
                '       ----------
                '
                .WriteLine("/COM,   Segment M3:")
                '....Key Point 15
                .WriteLine("K, 15, " & LPFormat_KP(Unit_In, mKP(15)))
                '....Key Point 16
                .WriteLine("K, 16, " & LPFormat_KP(Unit_In, mKP(16)))
                .WriteLine(" ")

                '   Mid-Mid Conv - RHS 1/2:
                '   -----------------------
                '
                '       Segment M3.
                '       -----------
                .WriteLine("/COM, Mid-mid Conv - RHS 1/2: ")
                .WriteLine("/COM, -----------------------")
                .WriteLine("/COM, ")
                .WriteLine("/COM,   Segment M3:")
                '....Key Point 17
                .WriteLine("K, 17, " & LPFormat_KP(Unit_In, mKP(17)))
                '....Key Point 18
                .WriteLine("K, 18, " & LPFormat_KP(Unit_In, mKP(18)))
                .WriteLine(" ")


                '       Segment M4. 
                '       ----------
                '
                .WriteLine("/COM,   Segment M4:")
                '....Key Point 19
                .WriteLine("K, 19, " & LPFormat_KP(Unit_In, mKP(19)))
                '....Key Point 20
                .WriteLine("K, 20, " & LPFormat_KP(Unit_In, mKP(20)))
                .WriteLine(" ")

                '       Segment M5. 
                '       ----------
                '
                .WriteLine("/COM,   Segment M5:")
                '....Key Point 21
                .WriteLine("K, 21, " & LPFormat_KP(Unit_In, mKP(21)))
                '....Key Point 22
                .WriteLine("K, 22, " & LPFormat_KP(Unit_In, mKP(22)))
                .WriteLine(" ")

                '   Number of Key Points
                '   --------------------
                If mNConv > 2 Then
                    .WriteLine("countKP_Seal = " & mKP_Last)

                ElseIf mNConv = 2 Then      'SB 10DEC08
                    .WriteLine("countKP_Seal = 22")

                ElseIf mNConv = 1 Then      'SB 10DEC08
                    .WriteLine("countKP_Seal = 16")
                End If
                .WriteLine(" ")

                '....For Greater than 2 Convolution


                .WriteLine("/COM, ADDITIONAL 1/2 MID-mid CONVOLUTIONS:" & _
                            "(For NConv > 2 only)")
                .WriteLine("/COM,   ----------------------------------")
                .WriteLine("/COM, ")
                .WriteLine(" ")
                .WriteLine("countAddHalfConv = " & mCountAddHalfConv)
                .WriteLine(" ")
                If mNConv > 2 Then

                    For i = 23 To mKP_Last
                        .WriteLine("K, " & i & ", " & LPFormat_KP(Unit_In, mKP(i)))
                    Next
                End If

                '       Axial Flange:
                '       ------------
                '
                .WriteLine("/COM, Axial Flange:")
                .WriteLine("/COM, -------------")
                .WriteLine("kOffAF = 100")

                '....Key Point 200
                .WriteLine("K, kOffAF+1, " & LPFormat_KP(Unit_In, mKP_AF(1)))
                '....Key Point 201
                .WriteLine("K, kOffAF+2, " & LPFormat_KP(Unit_In, mKP_AF(2)))
                '....Key Point 202
                .WriteLine("K, kOffAF+3, " & LPFormat_KP(Unit_In, mKP_AF(3)))
                '....Key Point 203
                .WriteLine("K, kOffAF+4, " & LPFormat_KP(Unit_In, mKP_AF(4)))
                .WriteLine(" ")


                '       Radial Flange
                '       ------------
                '
                .WriteLine("/COM, Radial Flange:")
                .WriteLine("/COM, --------------")
                .WriteLine("kOffRF = 200")
                '....Key Point 101
                .WriteLine("K, kOffRF+1, " & LPFormat_KP(Unit_In, mKP_RF(1)))
                '....Key Point 102
                .WriteLine("K, kOffRF+2, " & LPFormat_KP(Unit_In, mKP_RF(2)))
                '....Key Point 103
                .WriteLine("K, kOffRF+3, " & LPFormat_KP(Unit_In, mKP_RF(3)))
                '....Key Point 104
                .WriteLine("K, kOffRF+4, " & LPFormat_KP(Unit_In, mKP_RF(4)))
                .WriteLine(" ")

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

    '-------------------------------------------------------------------------------
    '*                          KEY POINT FILES WRITE - END                        *
    '-------------------------------------------------------------------------------

#End Region



#Region "UTILITY ROUTINES:"

    '-------------------------------------------------------------------------------
    '                         UTILITY ROUTINES - BEGIN                             '
    '-------------------------------------------------------------------------------

    Private Function LineStr(ByVal Pt1_In As PointF, ByVal Pt2_In As PointF) As String
        '=============================================================================
        Dim pstrPt1 As String, pstrPt2 As String
        pstrPt1 = LPFormat(Pt1_In.X) & "," & LPFormat(Pt1_In.Y)
        pstrPt2 = LPFormat(Pt2_In.X) & "," & LPFormat(Pt2_In.Y)

        Return pstrPt1 & "," & pstrPt2

    End Function


    Private Function ArcStr(ByVal Cen_In As PointF, ByVal Pt1_In As PointF, _
                            ByVal Pt2_In As PointF) As String
        '====================================================================
        Dim pstrCen As String, pstrPt1 As String, pstrPt2 As String
        pstrCen = LPFormat(Cen_In.X) & "," & LPFormat(Cen_In.Y)
        pstrPt1 = LPFormat(Pt1_In.X) & "," & LPFormat(Pt1_In.Y)
        pstrPt2 = LPFormat(Pt2_In.X) & "," & LPFormat(Pt2_In.Y)


        If POrient = "External" Then            'SB 13FEB08
            ArcStr = pstrCen & "," & pstrPt1 & "," & pstrPt2
        ElseIf POrient = "Internal" Then
            ArcStr = pstrCen & "," & pstrPt2 & "," & pstrPt1
        Else
            ArcStr = " "
        End If

    End Function


    Public Function LPFormat(ByVal sngData As Single) As String
        '==========================================================
        'This function is used in displaying length data with eight(8)precision.         
        LPFormat = Format(sngData, "##0.00000000")

    End Function


    Public Function LPFormat_KP(ByVal Unit_In As IPE_clsUnit, _
                                ByVal Pt_In As PointF) As String
        '========================================================
        'This function is used in displaying length data with four(4) or eight(8) 
        'precision.

        Dim Pt_X As String = ""  '....Holds formated string of X Co-ordinate
        Dim Pt_Y As String = "" '....Holds formated string of Y Co-ordinate

        If Unit_In.System = "English" Then  '....Four(4)precision for "English" Unit.
            '------------------------------

            If Pt_In.X >= 0 Then
                Pt_X = Format(Pt_In.X, " " & "##0.0000")
            Else
                Pt_X = Format(Pt_In.X, "##0.0000")
            End If

            If Pt_In.Y >= 0 Then
                Pt_Y = Format(Pt_In.Y, " " & "##0.0000")
            Else
                Pt_Y = Format(Pt_In.Y, "##0.0000")
            End If

        ElseIf Unit_In.System = "Metric" Then   '....Eight(8)precision for "Metric" Unit.
            '---------------------------------

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

        End If

        Return Pt_X & "," & Pt_Y

    End Function

    '-------------------------------------------------------------------------------
    '                         UTILITY ROUTINES  - END                              '
    '-------------------------------------------------------------------------------

#End Region

    '*******************************************************************************
    '*                        CLASS METHODS - END                                  *
    '*******************************************************************************

End Class
