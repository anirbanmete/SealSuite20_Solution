
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsUSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  14JUN11                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports clsLibrary11
Imports System.Drawing

Partial Public Class IPE_clsUSeal
    Inherits IPE_clsSeal


    Public Sub Draw(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                   ByVal margin_In() As Single, ByVal color_In() As Color, _
                   ByVal intDrawWid_In() As Integer, ByVal intDashStyle_In() As Integer, _
                   ByVal strGeomType_In As String, ByVal strScaling_In As String, _
                   ByVal sngMultFacWidDir_In As Single, ByRef EnvpTopL As PointF, _
                   ByRef EnvpBotR As PointF)
        '====================================================================================
        '
        '....The envelope location (output parameters) are in the "Page Coordinate" system:
        '       Top    Left  Corner: (xEnvpTopL, yEnvpTopL)
        '       Bottom Right Corner: (xEnvpBotR, yEnvpBotR)

        If grphObj_In Is Nothing = True Then Exit Sub

        'SG 10JUN11
        '....Calculate the Scale Factor for VB graphics & the modified margins.
        Calc_MemberVariables("STD")     '....Gets mHFree & mWid.
        CalcScale(grphObj_In, size_In, margin_In, strScaling_In, sngMultFacWidDir_In, _
                  mScalePCS, mMarginMod)

        'CalcOrigWCS(grphObj_In, size_In, mMarginMod, "STD")

        'Draw USeal Geometries:
        '----------------------
        '
        Dim iBeg As Int16, iEnd As Int16

        If strGeomType_In = "STD" Then
            iBeg = 1
            iEnd = 1

        ElseIf strGeomType_In = "ADJ" Then
            iBeg = 2
            iEnd = 2

        ElseIf strGeomType_In = "BOTH" Then
            '....Additional Plot: "Adjusted" Geometry superimposed on the "Standard".
            iBeg = 1
            iEnd = 2
        End If

        Dim pstrGeomType As String = ""

        Dim i As Int16

        For i = iBeg To iEnd
            If i = 1 Then
                pstrGeomType = "STD"
            ElseIf i = 2 Then
                pstrGeomType = "ADJ"
                Calc_MemberVariables(pstrGeomType)
                CalcScale(grphObj_In, size_In, margin_In, "SCALE_BY_ADJ", sngMultFacWidDir_In, _
                                                                mScalePCS, mMarginMod)
            End If


            DrawUSeal(grphObj_In, size_In, pstrGeomType, color_In(i - iBeg), _
                      intDrawWid_In(i - iBeg), intDashStyle_In(i - iBeg), _
                      EnvpTopL, EnvpBotR)
        Next

    End Sub


#Region "........DRAW USEAL:"

    Private Sub DrawUSeal(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                          ByVal strGeomType_In As String, _
                          ByVal color_In As Color, ByVal intDrawWid_In As Integer, _
                          ByVal intDashStyle_In As Integer, _
                          ByRef EnvpTopL As PointF, ByRef EnvpBotR As PointF)
        '============================================================================

        'AM/SB 04MAY09
        Dim pTheta1Draw As Single
        Dim pTheta2Draw As Single
        Dim pR1Draw As Single
        Dim pR2Draw As Single
        Dim pTDraw As Single

        If strGeomType_In = "STD" Then

            pTheta1Draw = mThetaStd(1)
            pTheta2Draw = mThetaStd(2)
            pR1Draw = mRStd(1)
            pR2Draw = mRStd(2)
            pTDraw = mTStd

        ElseIf strGeomType_In = "ADJ" Then

            pTheta1Draw = mTheta(1)
            pTheta2Draw = mTheta(2)
            pR1Draw = mR(1)
            pR2Draw = mR(2)
            pTDraw = mT

        End If

        CalcOrigWCS(grphObj_In, size_In, mMarginMod, strGeomType_In)


        'Draw the USeal.
        '---------------
        Dim pRad As Single
        Dim pAngStart As Single
        Dim pAngSweep As Single

        Dim pCen As PointF = Pt_PCS(mCen(1))    'SG 30MAY11

        '   SEGMENT 1:
        '   ----------
        pAngSweep = pTheta1Draw

        If POrient = "External" Then
            '------------------------
            pAngStart = 90 - (0.5 * pTheta1Draw)

            pRad = Rad_PCS(pR1Draw, -pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            pRad = Rad_PCS(pR1Draw, pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '   pCen.Y, mUnit.L_ConToUser(pR1Draw - 0.5 * pTDraw) * mScalePCS, _
            '   90 - (0.5 * pTheta1Draw), pTheta1Draw)
            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '    pCen.Y, mUnit.L_ConToUser(pR1Draw + 0.5 * pTDraw) * mScalePCS, _
            '    90 - (0.5 * pTheta1Draw), pTheta1Draw)

        ElseIf POrient = "Internal" Then
            '---------------------------
            pAngStart = -90 - (0.5 * pTheta1Draw)

            pRad = Rad_PCS(pR1Draw, -pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            pRad = Rad_PCS(pR1Draw, pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '   pCen.Y, mUnit.L_ConToUser(pR1Draw - 0.5 * pTDraw) * mScalePCS, _
            '   -90 - (0.5 * pTheta1Draw), pTheta1Draw)
            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '    pCen.Y, mUnit.L_ConToUser(pR1Draw + 0.5 * pTDraw) * mScalePCS, _
            '    -90 - (0.5 * pTheta1Draw), pTheta1Draw)

        End If

        '   SEGMENT 2:  straight segment.               'SG 30MAY11
        '   ----------

        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
             Pt_PCS(mKP(5)), Pt_PCS(mKP(3)))
        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
             Pt_PCS(mKP(6)), Pt_PCS(mKP(4)))

        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
             Pt_PCS(mKP(9)), Pt_PCS(mKP(11)))
        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
             Pt_PCS(mKP(10)), Pt_PCS(mKP(12)))


        '   SEGMENT 3:       'SG 30MAY11
        '   ----------

        pCen = Pt_PCS(mCen(2))
        Dim pBeta As Single = 90 - (0.5 * pTheta1Draw)


        pAngSweep = pTheta2Draw

        If POrient = "External" Then
            '-----------------------
            pRad = Rad_PCS(pR2Draw, -pTDraw)
            pAngStart = -pTheta2Draw + pBeta
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            pRad = Rad_PCS(pR2Draw, pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '    pCen.Y, mUnit.L_ConToUser(pR2Draw - 0.5 * pTDraw) * mScalePCS, _
            '    -pTheta2Draw + pBeta, pTheta2Draw)
            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '    pCen.Y, mUnit.L_ConToUser(pR2Draw + 0.5 * pTDraw) * mScalePCS, _
            '    -pTheta2Draw + pBeta, pTheta2Draw)

        ElseIf POrient = "Internal" Then
            '---------------------------
            pRad = Rad_PCS(pR2Draw, -pTDraw)
            pAngStart = -pBeta
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            pRad = Rad_PCS(pR2Draw, pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                    pRad, pAngStart, pAngSweep)

            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '         pCen.Y, mUnit.L_ConToUser(pR2Draw - 0.5 * pTDraw) * mScalePCS, _
            '         -pBeta, pTheta2Draw)
            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '         pCen.Y, mUnit.L_ConToUser(pR2Draw + 0.5 * pTDraw) * mScalePCS, _
            '         -pBeta, pTheta2Draw)
        End If

        pCen = Pt_PCS(mCen(3))

        If POrient = "External" Then
            '------------------------
            pAngStart = -180 + (pTheta2Draw - pBeta)
            pAngSweep = -pTheta2Draw
            pRad = Rad_PCS(pR2Draw, -pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            pRad = Rad_PCS(pR2Draw, pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '   pCen.Y, mUnit.L_ConToUser(pR2Draw - 0.5 * pTDraw) * mScalePCS, _
            '   -180 + (pTheta2Draw - pBeta), -pTheta2Draw)
            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '    pCen.Y, mUnit.L_ConToUser(pR2Draw + 0.5 * pTDraw) * mScalePCS, _
            '    -180 + (pTheta2Draw - pBeta), -pTheta2Draw)

        ElseIf POrient = "Internal" Then
            '----------------------------
            pAngStart = -180 - (pTheta2Draw - pBeta)
            pAngSweep = pTheta2Draw
            pRad = Rad_PCS(pR2Draw, -pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            pRad = Rad_PCS(pR2Draw, pTDraw)
            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen, _
                     pRad, pAngStart, pAngSweep)

            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '         pCen.Y, mUnit.L_ConToUser(pR2Draw - 0.5 * pTDraw) * mScalePCS, _
            '         -180 - (pTheta2Draw - pBeta), pTheta2Draw)
            'Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pCen.X, _
            '         pCen.Y, mUnit.L_ConToUser(pR2Draw + 0.5 * pTDraw) * mScalePCS, _
            '         -180 - (pTheta2Draw - pBeta), pTheta2Draw)


        End If

        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                  Pt_PCS(mKP(1)), Pt_PCS(mKP(2)))
        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                  Pt_PCS(mKP(13)), Pt_PCS(mKP(14)))

        '....Store Seal Envelope point coordinates, required for the
        '....proposal drawing.      
        '-----------------------------------------------------------

        Dim pEnvpBotR As PointF
        Dim pEnvpTopL As PointF

        If POrient = "External" Then
            With pEnvpTopL
                .X = mKP(14).X
                .Y = mCen(3).Y + (mR(2) + 0.5 * mT)
            End With

            With pEnvpBotR
                .X = mKP(8).X
                .Y = mCen(2).Y - (mR(2) + 0.5 * mT)
            End With

        ElseIf POrient = "Internal" Then
            With pEnvpTopL
                .X = mKP(8).X
                .Y = mCen(3).Y + (mR(2) + 0.5 * mT)
            End With

            With pEnvpBotR
                .X = mKP(2).X
                .Y = mCen(2).Y - (mR(2) + 0.5 * mT)
            End With

        End If

        EnvpBotR = Pt_PCS(pEnvpBotR)
        EnvpTopL = Pt_PCS(pEnvpTopL)

    End Sub


    Private Function Rad_PCS(ByVal RadDraw_In As Single, ByVal TDraw_In As Single) As Single
        '============================================================================

        Return mUnit.L_ConToUser(RadDraw_In + 0.5 * TDraw_In) * mScalePCS

    End Function

    Private Sub CalcOrigWCS(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                          ByRef marginMod_In() As Single, ByVal strGeomType_In As String)
        '==================================================================================
        '   TEMPLATE INDEPENDENT:
        '   
        'This function calculates the coordinates of the WCS origin w.r.t to the PCS.
        '.... This calculation should be done separately for the "STD" and the
        '...."ADJ" in order to center each plot independently.

        '....Get appropriate free height dimension in user unit.
        Dim Hfree_UUnit As Single
        If strGeomType_In = "STD" Then
            Hfree_UUnit = mUnit.L_ConToUser(mHfreeStd)

        ElseIf strGeomType_In = "ADJ" Then
            Hfree_UUnit = mUnit.L_ConToUser(Hfree)
        End If


        'Modify Side margins:
        '--------------------
        Dim pRatio As Single
        pRatio = marginMod_In(2) / (marginMod_In(2) + marginMod_In(1))

        Dim marginTot_WidDir As Single
        marginTot_WidDir = (size_In.Width - Hfree_UUnit * mScalePCS)

        marginMod_In(2) = marginTot_WidDir * pRatio             '....RHS Margin
        marginMod_In(1) = marginTot_WidDir - marginMod_In(2)    '....LHS Margin


        'Find the coordinates of the WCS origin with respect to the PCS:
        '----------------------------------------------------------------   SG 13JUN11
        Dim xVB_Right As Single, yVB_Botm As Single, yVB_Top As Single

        If strGeomType_In = "STD" Then
            xVB_Right = mUnit.L_ConToUser((mRStd(2) - mCen(2).Y) + 0.5 * mTStd) * mScalePCS
        ElseIf strGeomType_In = "ADJ" Then
            xVB_Right = mUnit.L_ConToUser((mR(2) - mCen(2).Y) + 0.5 * mT) * mScalePCS
        End If

        mXVB_OrigWCS = size_In.Width - marginMod_In(2) - xVB_Right

        If POrient = "External" Then
            '-----------------------
            If strGeomType_In = "STD" Then
                yVB_Botm = mUnit.L_ConToUser(mCen(1).X - mRStd(1) - 0.5 * mTStd) * mScalePCS
            ElseIf strGeomType_In = "ADJ" Then
                yVB_Botm = mUnit.L_ConToUser(mCen(1).X - mR(1) - 0.5 * mT) * mScalePCS
            End If
            mYVB_OrigWCS = size_In.Height - marginMod_In(4) + yVB_Botm


        ElseIf POrient = "Internal" Then
            '---------------------------
            If strGeomType_In = "STD" Then
                yVB_Top = mUnit.L_ConToUser(mCen(1).X + mRStd(1) + 0.5 * mTStd) * mScalePCS
            ElseIf strGeomType_In = "ADJ" Then
                yVB_Top = mUnit.L_ConToUser(mCen(1).X + mR(1) + 0.5 * mT) * mScalePCS
            End If

            mYVB_OrigWCS = marginMod_In(3) + yVB_Top

        End If

    End Sub


#Region "....................GRAPHICS PARAMETERS: HELPER ROUTINE:"

    '-------------------------------------------------------------------------------
    '                GRAPHICS PARAMETERS CALCULATION METHOD - BEGIN                '
    '-------------------------------------------------------------------------------
    Private Sub Calc_GraphicsParams(ByVal Theta1Draw_In As Single, _
                                    ByVal Theta2Draw_In As Single, _
                                    ByVal R1Draw_In As Single, _
                                    ByVal R2Draw_In As Single, _
                                    ByVal LLegDraw_In As Single, _
                                    ByVal TDraw_In As Single)
        '===========================================================================
        'This subroutine calculates various Graphics Parameters in CYS = CARTW (WCS),  
        '....which are needed to draw an U-Seal graphics on a selected device. 
        '....e.g. picture box or printer object.
        '
        'Refer to the Figure 1 of the Theoretical Manual for the 
        '....World Coordinate System'(WCS).
        '
        'Before plotting, the above graphics parameters in WCS will be converted to 
        '....the 'VB Coordinate System' (PCS) attached to the picture box. 
        '....This conversion will be done in a seperate routine.

        If POrient = "External" Then
            Calc_GraphicsParams_External(Theta1Draw_In, Theta2Draw_In,
                                         R1Draw_In, R2Draw_In, LLegDraw_In, TDraw_In)

        ElseIf POrient = "Internal" Then
            Calc_GraphicsParams_Internal(Theta1Draw_In, Theta2Draw_In,
                                         R1Draw_In, R2Draw_In, LLegDraw_In, TDraw_In)

        End If

    End Sub

    Private Sub Calc_GraphicsParams_External(ByVal Theta1_In As Single, _
                                            ByVal Theta2_In As Single, _
                                            ByVal R1_In As Single, _
                                            ByVal R2_In As Single, _
                                            ByVal LLeg_In As Single, _
                                            ByVal T_In As Single)
        '=================================================================

        Dim pRi, pRo As Single            '....Local Inner & Outer Radii.

        '....Complementary Angles:
        mBeta(1) = 90 - 0.5 * Theta1_In
        mBeta(2) = Theta2_In - mBeta(1)


        '   SEGMENT R1:
        '   ----------

        '....Inner & Outer Radii.
        pRi = R1_In - 0.5 * T_In
        pRo = R1_In + 0.5 * T_In

        '....Center Point 1.
        With mCen(1)
            .X = 0.5 * (DControl + T_In) + R1_In
            .Y = 0.0
        End With


        '....Key Point # 8.
        With mKP(8)
            .X = 0.5 * DControl
            .Y = 0.0
        End With

        '....Key Point # 7.
        With mKP(7)
            .X = mKP(8).X + T_In
            .Y = 0.0
        End With


        '....Key Point # 6.
        With mKP(6)
            .X = mCen(1).X - (pRo * SinD(mBeta(1)))
            .Y = mCen(1).Y - (pRo * CosD(mBeta(1)))
        End With

        '....Key Point # 5.
        With mKP(5)
            .X = mCen(1).X - (pRi * SinD(mBeta(1)))
            .Y = mCen(1).Y - (pRi * CosD(mBeta(1)))
        End With


        '....Key Point # 4.
        With mKP(4)
            .X = mKP(6).X + LLeg_In * CosD(mBeta(1))
            .Y = mKP(6).Y - LLeg_In * SinD(mBeta(1))
        End With

        '....Key Point # 3.
        With mKP(3)
            .X = mKP(5).X + LLeg_In * CosD(mBeta(1))
            .Y = mKP(5).Y - LLeg_In * SinD(mBeta(1))
        End With


        '   SEGMENT R2:
        '   ----------

        '....Inner & Outer Radii.
        pRi = R2_In - 0.5 * T_In
        pRo = R2_In + 0.5 * T_In


        '....Center Point 2.
        With mCen(2)
            .X = mKP(3).X + pRi * SinD(mBeta(1))
            .Y = mKP(3).Y + pRi * CosD(mBeta(1))
        End With


        '....Key Point # 2.
        With mKP(2)
            .X = mCen(2).X + pRo * SinD(mBeta(2))
            .Y = mCen(2).Y - pRo * CosD(mBeta(2))
        End With


        '....Key Point # 1.
        With mKP(1)
            .X = mCen(2).X + pRi * SinD(mBeta(2))
            .Y = mCen(2).Y - pRi * CosD(mBeta(2))
        End With


        '   SYMMETRY REFLECTION:
        '   ====================

        '....Key Point # 9. 
        With mKP(9)
            .X = mKP(5).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(5).Y)
        End With

        '....Key Point # 10. 
        With mKP(10)
            .X = mKP(6).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(6).Y)
        End With

        '....Key Point # 11. 
        With mKP(11)
            .X = mKP(3).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(3).Y)
        End With

        '....Key Point # 12. 
        With mKP(12)
            .X = mKP(4).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(4).Y)
        End With

        '....Key Point # 13. 
        With mKP(13)
            .X = mKP(1).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(1).Y)
        End With

        '....Key Point # 14. 
        With mKP(14)
            .X = mKP(2).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(2).Y)
        End With


        '....Center Point 3.
        With mCen(3)
            .X = mCen(2).X
            .Y = mCen(1).Y + (mCen(1).Y - mCen(2).Y)
        End With

    End Sub


    Private Sub Calc_GraphicsParams_Internal(ByVal Theta1_In As Single, _
                                             ByVal Theta2_In As Single, _
                                             ByVal R1_In As Single, _
                                             ByVal R2_In As Single, _
                                             ByVal LLeg_In As Single, _
                                             ByVal T_In As Single)
        '=================================================================  
        Dim pRi, pRo As Single            '....Local Inner & Outer Radii.

        mBeta(1) = 90 - 0.5 * Theta1_In
        mBeta(2) = Theta2_In - mBeta(1)

        '   SEGMENT R1:
        '   ----------

        '....Inner & Outer Radii.
        pRi = R1_In - 0.5 * T_In
        pRo = R1_In + 0.5 * T_In

        '....Center Point 1.
        With mCen(1)
            .X = 0.5 * (DControl - T_In) - R1_In
            .Y = 0.0
        End With


        '....Key Point # 8.
        With mKP(8)
            .X = 0.5 * DControl
            .Y = 0.0
        End With

        '....Key Point # 7.
        With mKP(7)
            .X = 0.5 * DControl - T_In
            .Y = 0.0
        End With

        '....Key Point # 6.
        With mKP(6)
            .X = mCen(1).X + (pRo * SinD(mBeta(1)))
            .Y = mCen(1).Y - (pRo * CosD(mBeta(1)))
        End With

        '....Key Point # 5.
        With mKP(5)
            .X = mCen(1).X + (pRi * SinD(mBeta(1)))
            .Y = mCen(1).Y - (pRi * CosD(mBeta(1)))
        End With

        '....Key Point # 4.
        With mKP(4)
            .X = mKP(6).X - LLeg_In * CosD(mBeta(1))
            .Y = mKP(6).Y - LLeg_In * SinD(mBeta(1))
        End With

        '....Key Point # 3.
        With mKP(3)
            .X = mKP(5).X - LLeg_In * CosD(mBeta(1))
            .Y = mKP(5).Y - LLeg_In * SinD(mBeta(1))
        End With


        '   SEGMENT R2:
        '   ----------

        '....Inner & Outer Radii.
        pRi = R2_In - 0.5 * T_In
        pRo = R2_In + 0.5 * T_In

        '....Center Point 2.
        With mCen(2)
            .X = mKP(3).X - pRi * SinD(mBeta(1))
            .Y = mKP(3).Y + pRi * CosD(mBeta(1))
        End With

        '....Key Point # 2.
        With mKP(2)
            .X = mCen(2).X - pRo * SinD(mBeta(2))
            .Y = mCen(2).Y - pRo * CosD(mBeta(2))
        End With

        '....Key Point # 1.
        With mKP(1)
            .X = mCen(2).X - pRi * SinD(mBeta(2))
            .Y = mCen(2).Y - pRi * CosD(mBeta(2))
        End With

        '   SYMMETRY REFLECTION:
        '   ====================

        '....Key Point # 9. 
        With mKP(9)
            .X = mKP(5).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(5).Y)
        End With

        '....Key Point # 10. 
        With mKP(10)
            .X = mKP(6).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(6).Y)
        End With

        '....Key Point # 11.
        With mKP(11)
            .X = mKP(3).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(3).Y)
        End With

        '....Key Point # 12.
        With mKP(12)
            .X = mKP(4).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(4).Y)
        End With

        '....Key Point # 13.
        With mKP(13)
            .X = mKP(1).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(1).Y)
        End With

        '....Key Point # 14.
        With mKP(14)
            .X = mKP(2).X
            .Y = mCen(1).Y + (mCen(1).Y - mKP(2).Y)
        End With

        '....Center Point 3.
        With mCen(3)
            .X = mCen(2).X
            .Y = mCen(1).Y + (mCen(1).Y - mCen(2).Y)
        End With

    End Sub

#End Region



#End Region

End Class
