
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsCSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01JUL11                                '
'                                                                              '
'===============================================================================

Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports clsLibrary11
Imports System.Drawing

'PB 23 APR11. Get_Pt_PCS ==> Pt_PCS. CalcScale, CalcXVB, CalcYVB moved to clsSeal.

Partial Public Class IPE_clsCSeal
    Inherits IPE_clsSeal

    '*******************************************************************************
    '*                        CLASS METHODS - BEGIN                                *
    '*******************************************************************************

#Region "GRAPHICS ROUTINE"

    '-------------------------------------------------------------------------------
    '       World COORDINATE SYSTEM (WCS) PARAMETERS CALCULATION METHOD - END      '
    '-------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------
    '                          GRAPHICS METHODS - BEGIN                            '
    '-------------------------------------------------------------------------------

    Public Sub Draw(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                    ByVal margin_In() As Single, _
                    ByVal color_In() As Color, ByVal intDrawWid_In() As Integer, _
                    ByVal intDashStyle_In() As Integer, ByVal strGeomType_In As String, _
                    ByVal strScaling_In As String, ByVal sngMultFacWidDir_In As Single, _
                    ByRef xEnvpTopL As Single, ByRef yEnvpTopL As Single, _
                    ByRef xEnvpBotR As Single, ByRef yEnvpBotR As Single)
        '===============================================================================
        '
        '....The envelope location (output parameters) are in the "Page Coordinate" system:
        '       Top    Left  Corner: (xEnvpTopL, yEnvpTopL)
        '       Bottom Right Corner: (xEnvpBotR, yEnvpBotR)

        If grphObj_In Is Nothing = True Then Exit Sub

        '....Calculate the Scale Factor for VB graphics & the modified margins.
        CalcScale(grphObj_In, size_In, margin_In, strScaling_In, sngMultFacWidDir_In, _
                  mScalePCS, mMarginMod)


        'Draw the Seal Geometries:
        '-------------------------
        Dim pstrGeomType As String = ""

        If strGeomType_In = "STD" Or strGeomType_In = "BOTH" Then
            pstrGeomType = "STD"
        ElseIf strGeomType_In = "ADJ" Then
            pstrGeomType = "ADJ"
        End If

        DrawSeal(grphObj_In, size_In, pstrGeomType, color_In(0), intDrawWid_In(0), _
                  intDashStyle_In(0), xEnvpTopL, yEnvpTopL, xEnvpBotR, yEnvpBotR)


        If strGeomType_In = "BOTH" Then
            pstrGeomType = "ADJ"
            DrawSeal(grphObj_In, size_In, pstrGeomType, color_In(1), intDrawWid_In(1), _
                      intDashStyle_In(1), xEnvpTopL, yEnvpTopL, xEnvpBotR, yEnvpBotR)
        End If

    End Sub


    Private Sub DrawSeal(ByVal grphObj_In As Graphics, _
                         ByVal size_In As SizeF, _
                         ByVal strGeomType_In As String, _
                         ByVal color_In As Color, _
                         ByVal intDrawWid_In As Integer, _
                         ByVal intDashStyle_In As Integer, _
                         ByRef xEnvpTopL As Single, _
                         ByRef yEnvpTopL As Single, _
                         ByRef xEnvpBotR As Single, _
                         ByRef yEnvpBotR As Single)
        '===================================================================
        'Refer to the Figure 1 of the Project Definition Document for the
        'World Coordinate System' (WCS).

        Dim pHfreeDraw As Single
        Dim pThetaOpeningDraw As Single
        Dim pTDraw As Single

        If strGeomType_In = "STD" Then
            pHfreeDraw = mHfreeStd
            pThetaOpeningDraw = mThetaOpeningStd
            pTDraw = mTStd

        ElseIf strGeomType_In = "ADJ" Then
            pHfreeDraw = mHfree
            pThetaOpeningDraw = mThetaOpening
            pTDraw = mT
        End If

        Update_WCSParams()
        CalcOrigWCS(grphObj_In, size_In, mMarginMod, strGeomType_In)

        'Draw the Seal
        '=============
        Dim pRad As Single, pRad_UUint As Single
        Dim pAngStart As Single, pAngSweep As Single


        '   Outer Arc:                          SG 18APR11
        '   ---------
        '....Center coordinates in PCS:
        'Dim pXCenVB As Single, pYCenVB As Single
        'pXCenVB = CalcXVB(mCen.X, mCen.Y)
        'pYCenVB = CalcYVB(mCen.X, mCen.Y)

        '....Center coordinates in PCS:
        Dim pCen_PCS As PointF
        pCen_PCS = Pt_PCS(mCen)

        'PB 22MAR07. Corrects Error #6, DR V50.
        pRad = 0.5 * pHfreeDraw
        pRad_UUint = mUnit.L_ConToUser(pRad)        '....In User Unit.

        If POrient = "External" Then
            '-----------------------
            pAngStart = -(90 - 0.5 * pThetaOpeningDraw)
            pAngSweep = 360 - pThetaOpeningDraw

        ElseIf POrient = "Internal" Then
            '---------------------------
            pAngStart = (90 - 0.5 * pThetaOpeningDraw)
            pAngSweep = -(360 - pThetaOpeningDraw)
        End If

        'Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
        '    pXCenVB, pYCenVB, pRad_UUint * mScalePCS, pAngStart, pAngSweep)
        Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                 pCen_PCS, pRad_UUint * mScalePCS, pAngStart, pAngSweep)

        '....Coordinates of the two outer edge points:
        Dim pRad_COS As Single
        pRad_COS = pRad * CosD(0.5 * pThetaOpeningDraw)

        Dim pRad_SIN As Single
        pRad_SIN = pRad * SinD(0.5 * pThetaOpeningDraw)

        Dim pPtOut1_WCS As PointF
        If POrient = "External" Then
            pPtOut1_WCS.X = mCen.X + pRad_COS
        ElseIf POrient = "Internal" Then
            pPtOut1_WCS.X = mCen.X - pRad_COS
        End If

        pPtOut1_WCS.Y = mCen.Y - pRad_SIN


        Dim pPtOut2_WCS As PointF
        pPtOut2_WCS.X = pPtOut1_WCS.X
        pPtOut2_WCS.Y = mCen.Y + pRad_SIN


        '   Inner Arc:
        '   ---------
        'PB 22MAR07.  Corrects Error #6, DR V50.
        pRad = 0.5 * pHfreeDraw - pTDraw
        pRad_UUint = mUnit.L_ConToUser(pRad)        '....In User Unit.

        'If pRad < mcEPS Then
        '    MessageBox.Show("Inner Radius Become Negative....." & vbCrLf & _
        '                    "Thickness can't be incremented further.", _
        '                    "Thickness Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        'Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _                SG 18APR11
        '    pXCenVB, pYCenVB, pRad_UUint * mScalePCS, pAngStart, pAngSweep)

        Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                 pCen_PCS, pRad_UUint * mScalePCS, pAngStart, pAngSweep)

        '....Coordinates of the two inner edge points:
        pRad_COS = pRad * CosD(0.5 * pThetaOpeningDraw)
        pRad_SIN = pRad * SinD(0.5 * pThetaOpeningDraw)

        Dim pPtIn1_WCS As PointF
        If POrient = "External" Then
            pPtIn1_WCS.X = mCen.X + pRad_COS
        ElseIf POrient = "Internal" Then
            pPtIn1_WCS.X = mCen.X - pRad_COS
        End If

        pPtIn1_WCS.Y = mCen.Y - pRad_SIN


        Dim pPtIn2_WCS As PointF
        pPtIn2_WCS.X = pPtIn1_WCS.X
        pPtIn2_WCS.Y = mCen.Y + pRad_SIN


        '   Straight Edges:
        '   ---------------
        '....Edge No 1 (RHS).
        '
        Dim pPtOut1 As PointF, pPtIn1 As PointF
        'pPtOut1.X = CalcXVB(pPtOut1_WCS.X, pPtOut1_WCS.Y)
        'pPtOut1.Y = CalcYVB(pPtOut1_WCS.X, pPtOut1_WCS.Y)
        'pPtIn1.X = CalcXVB(pPtIn1_WCS.X, pPtIn1_WCS.Y)
        'pPtIn1.Y = CalcYVB(pPtIn1_WCS.X, pPtIn1_WCS.Y)

        pPtOut1 = Pt_PCS(pPtOut1_WCS)
        pPtIn1 = Pt_PCS(pPtIn1_WCS)                         '....CalcXVB() and CalcYVB() replaced with Pt_PCS() SG 01JUL11

        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pPtIn1, pPtOut1)


        '....Edge No 2 (LHS).
        '
        Dim pPtOut2 As PointF, pPtIn2 As PointF
        'pPtOut2.X = CalcXVB(pPtOut2_WCS.X, pPtOut2_WCS.Y)
        'pPtOut2.Y = CalcYVB(pPtOut2_WCS.X, pPtOut2_WCS.Y)
        'pPtIn2.X = CalcXVB(pPtIn2_WCS.X, pPtIn2_WCS.Y)
        'pPtIn2.Y = CalcYVB(pPtIn2_WCS.X, pPtIn2_WCS.Y)

        pPtOut2 = Pt_PCS(pPtOut2_WCS)               'SG 01JUL11
        pPtIn2 = Pt_PCS(pPtIn2_WCS)

        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pPtIn2, pPtOut2)


        'Envelope Dimensions:
        '--------------------
        Dim pHfreeDraw_UUnit As Single
        pHfreeDraw_UUnit = mUnit.L_ConToUser(pHfreeDraw)


        If POrient = "Internal" Then
            'xEnvpTopL = pXCenVB - 0.5 * pHfreeDraw_UUnit * mScalePCS
            'yEnvpTopL = pYCenVB - 0.5 * pHfreeDraw_UUnit * mScalePCS
            xEnvpTopL = pCen_PCS.X - 0.5 * pHfreeDraw_UUnit * mScalePCS
            yEnvpTopL = pCen_PCS.Y - 0.5 * pHfreeDraw_UUnit * mScalePCS

            'xEnvpBotR = pXCenVB + 0.5 * pHfreeDraw_UUnit * mScalePCS
            xEnvpBotR = pCen_PCS.X + 0.5 * pHfreeDraw_UUnit * mScalePCS
            yEnvpBotR = pPtOut1.Y

        ElseIf POrient = "External" Then
            'xEnvpTopL = pXCenVB - 0.5 * pHfreeDraw_UUnit * mScalePCS
            xEnvpTopL = pCen_PCS.X - 0.5 * pHfreeDraw_UUnit * mScalePCS
            yEnvpTopL = pPtOut2.Y

            'xEnvpBotR = pXCenVB + 0.5 * pHfreeDraw_UUnit * mScalePCS
            'yEnvpBotR = pYCenVB + 0.5 * pHfreeDraw_UUnit * mScalePCS
            xEnvpBotR = pCen_PCS.X + 0.5 * pHfreeDraw_UUnit * mScalePCS
            yEnvpBotR = pCen_PCS.Y + 0.5 * pHfreeDraw_UUnit * mScalePCS
        End If


    End Sub

    '....Move to claSeal SG 18APR11
    'Private Sub Arc(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
    '                ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
    '                ByVal xCenPCS_In As Single, ByVal yCenPCS_In As Single, _
    '                ByVal rad_In As Single, _
    '                ByVal angStart_In As Single, ByVal angSweep_In As Single)
    '    '===========================================================================
    '    'This subroutine draws a circular arc. 

    '    'Enclosing Rectangle:
    '    '--------------------
    '    '....LHS Top.
    '    Dim pPtLoc As New PointF(xCenPCS_In - rad_In, yCenPCS_In - rad_In) '....Location
    '    Dim pSizeF As New SizeF(rad_In * 2, rad_In * 2)                    '....Size
    '    Dim pRect As New RectangleF(pPtLoc, pSizeF)                        '....Rectangle  


    '    '....Pixel densities per unit "PageUnit" dimension (in or mm)
    '    Dim pDpX As Single
    '    pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

    '    Dim pPen As New Pen(color_In, drawWid_In / pDpX)
    '    pPen.DashStyle = intDashStyle_In

    '    Try
    '        grphObj_In.DrawArc(pPen, pRect, angStart_In, angSweep_In)

    '    Catch pEXP As Exception
    '        MsgBox("Inner Radius Become zero or Negative.")
    '        Exit Sub
    '    End Try

    '    pPen = Nothing

    'End Sub


    'Private Sub Line(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
    '                 ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
    '                 ByVal ptBeg_In As PointF, ByVal ptEnd_In As PointF)
    '    '======================================================================
    '    'This subroutine draws a line. 

    '    '....Pixel densities per unit "PageUnit" dimension (in or mm)
    '    Dim pDpX As Single
    '    pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

    '    Dim pPen As New Pen(color_In, drawWid_In / pDpX)
    '    pPen.DashStyle = intDashStyle_In
    '    grphObj_In.DrawLine(pPen, ptBeg_In, ptEnd_In)
    '    pPen = Nothing

    'End Sub

    '-------------------------------------------------------------------------------
    '                           GRAPHICS METHODS - END                             '
    '-------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------
    '                 GRAPHICS SCALING & POSITIONING METHODS - BEGIN               '
    '-------------------------------------------------------------------------------

    'PB 23APR11. Moved to clsSeal.

    'Private Sub CalcScale(ByVal grphObj_In As Graphics, _
    '                      ByVal size_In As SizeF, _
    '                      ByVal margin_In() As Single, _
    '                      ByVal strScalingGeom_In As String, _
    '                      ByVal sngMultFacWidDir_In As Single, _
    '                      ByRef scalePCS_Out As Single, _
    '                      ByRef marginMod_Out() As Single)
    '    '================================================================================

    '    'This function calculates an appropriate scale based on the given Geometry Type e.g.
    '    '...."STD" or "ADJ".
    '    '
    '    '   Input Margins:
    '    '       margin_In (1) : LHS
    '    '       margin_In (2) : RHS
    '    '       margin_In (3) : TOP
    '    '       margin_In (4) : BOT
    '    '
    '    '   Modified Margins:
    '    '       marginMod_Out ()


    '    'Seal envelope dimensions in user unit.
    '    '-------------------------------------
    '    Dim pHfreeCur As Single
    '    Dim pWidCur As Single

    '    If strScalingGeom_In = "SCALE_BY_STD" Then
    '        pHfreeCur = mHfreeStd
    '        pWidCur = mWidStd

    '    ElseIf strScalingGeom_In = "SCALE_BY_ADJ" Then
    '        pHfreeCur = mHfree
    '        pWidCur = mWid
    '    End If


    '    Dim pHfree_UUnit As Single
    '    pHfree_UUnit = mUnit.L_ConToUser(pHfreeCur)

    '    Dim pWid_UUnit As Single
    '    pWid_UUnit = mUnit.L_ConToUser(pWidCur)


    '    'Establish SCALING:
    '    '==================
    '    '
    '    'Width Direction - Based on Free Height:
    '    '---------------------------------------
    '    '....Along the graphics object's width direction.
    '    '........Accommodate some extra to account for adjustment of the Free Height.

    '    Dim psngWidEnv As Single
    '    psngWidEnv = sngMultFacWidDir_In * pHfree_UUnit

    '    Dim pMarginTot_WidDir As Single
    '    pMarginTot_WidDir = margin_In(1) + margin_In(2)

    '    Dim pScaleW As Single
    '    pScaleW = (size_In.Width - pMarginTot_WidDir) / psngWidEnv


    '    'Height Direction - Based on Radial Width:
    '    '-----------------------------------------
    '    '....Along the graphics object's height direction.
    '    Dim psngHtEnv As Single
    '    psngHtEnv = pWid_UUnit

    '    Dim pMarginTot_HtDir As Single
    '    pMarginTot_HtDir = margin_In(3) + margin_In(4)

    '    Dim pScaleH As Single
    '    pScaleH = (size_In.Height - pMarginTot_HtDir) / psngHtEnv


    '    'Scaling for the PCS - choose the smaller of the above two.
    '    '----------------------------------------------------------
    '    If pScaleW <= pScaleH Then
    '        scalePCS_Out = pScaleW

    '    ElseIf pScaleW > pScaleH Then
    '        scalePCS_Out = pScaleH
    '    End If


    '    'Find the modified margin at the Top & Botm (maintaining the same ratio).
    '    '-----------------------------------------------------------------------
    '    Dim pRatio As Single
    '    pRatio = margin_In(3) / (margin_In(3) + margin_In(4))
    '    pMarginTot_HtDir = (size_In.Height - pWid_UUnit * scalePCS_Out)

    '    marginMod_Out(3) = pMarginTot_HtDir * pRatio           '....Top    Margin
    '    marginMod_Out(4) = pMarginTot_HtDir * (1.0# - pRatio)  '....Bottom Margin

    '    '....Leave the LHS & RHS margins as before for now. They are modified in
    '    '........the routine CalcOrigWCS.
    '    marginMod_Out(1) = margin_In(1)
    '    marginMod_Out(2) = margin_In(2)

    'End Sub


    Private Sub CalcOrigWCS(ByVal grphObj_In As Graphics, _
                            ByVal size_In As SizeF, _
                            ByRef marginMod_In() As Single, _
                            ByVal strGeomType_In As String)
        '=====================================================
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
        '----------------------------------------------------------------
        mXVB_OrigWCS = marginMod_In(1) + 0.5 * Hfree_UUnit * mScalePCS

        Dim yVB_Botm As Single, yVB_Top As Single
        If POrient = "External" Then
            yVB_Botm = mUnit.L_ConToUser(mCen.X - 0.5 * HfreeStd) * mScalePCS
            mYVB_OrigWCS = size_In.Height - marginMod_In(4) + yVB_Botm

        ElseIf POrient = "Internal" Then
            yVB_Top = mUnit.L_ConToUser(mCen.X + 0.5 * HfreeStd) * mScalePCS
            mYVB_OrigWCS = marginMod_In(3) + yVB_Top
        End If

    End Sub


    'PB 23APR11. Moved to clsSeal.

    'Private Function CalcXVB(ByVal xWCS_Point As Single, _
    '                         ByVal yWCS_Point As Single) As Single
    '    '==============================================================================
    '    ' This function calculates the X Coordinate in the PCS when the WCS coordinates
    '    ' .... of a point are given. Dependent on "STD" or "ADJ" geometry.
    '    CalcXVB = mXVB_OrigWCS - mUnit.L_ConToUser(yWCS_Point) * mScalePCS
    'End Function


    'Private Function CalcYVB(ByVal xWCS_Point As Single, _
    '                         ByVal yWCS_Point As Single) As Single
    '    '================================================================================
    '    ' This function calculates the X Coordinate in the PCS when the WCS coordinates 
    '    ' ....of a point are given. Dependent on "STD" or "ADJ" geometry.
    '    CalcYVB = mYVB_OrigWCS - mUnit.L_ConToUser(xWCS_Point) * mScalePCS
    'End Function

    '--------------------------------------------------------------------------------
    '               GRAPHICS SCALING & POSITIONING METHODS  - END                   '
    '--------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------
    '      WORLD COORDINATE SYSTEM (WCS) PARAMETERS CALCULATION METHOD - BEGIN     '
    '-------------------------------------------------------------------------------

    Private Sub Update_WCSParams()
        '=========================
        'This subroutine updates the Graphics Parameters.

        'Input Data   : (All dimensions in inches)
        '--------------
        'POrient    : 'External' or 'Internal' Pressure Design
        'DControl   : Control Dia of ESeal
        'Hfree      : Thickness


        'Output - Graphics Parameters Set:
        '--------------------------------
        'The following parameters are needed to draw the C-Seal line graphics on
        '....a selected device e.g. a picture box or printer object.

        'Refer to the Figure 1 of the Theoretical Manual for the
        '....'World Coordinate System' (WCS).
        '
        '....The coordinates of the following point are in WCS:
        '
        '    mCen

        'Before plotting, the above graphics parameters which are in WCS will be converted to
        'the (PCS) attached to the picture box. This conversion will
        'be done in a seperate routine.


        'Center Point:
        '-------------
        If POrient = "External" Then
            mCen.X = 0.5 * (DControl + HfreeStd)
        ElseIf POrient = "Internal" Then
            mCen.X = 0.5 * (DControl - HfreeStd)
        End If

        mCen.Y = 0.0#

    End Sub

#End Region

    '*******************************************************************************
    '*                        CLASS METHODS - END                                  *
    '*******************************************************************************

End Class
