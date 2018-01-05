'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsESeal_Graphics                      '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB16                                '
'                                                                              '
'===============================================================================

'Routines
'--------
'
'   METHODS:
'   --------  
'
'       Public  Sub             Draw                        ()
'       Private Sub             CalcScale                   ()

'       Private Sub             DrawESeal                   ()
'       Private Sub             Calc_MemberVariables        ()

'       Private Sub             Calc_GraphicsParams         ()
'       Private Sub             Calc_GraphicsParams_External()
'       Private Sub             Calc_GraphicsParams_Internal()

'       Private Function        CylToCart                   ()
'       Private Function        RotateAxes                  ()
'       Private Function        TranslateAxes               ()

'       Private Sub             Calc_HFree                  ()
'       Private Sub             Calc_Wid                    ()
'       Public Function         CalcZClear                  ()
'       Private Sub             CalcOrigWCS                 ()
'
'       Private Sub             Draw_EndHalfConvs           ()
'       Private Sub             Draw_MidConv                ()
'       Private Sub             Draw_Mid_End_HalfConvs      ()
'       Private Sub             Draw_Mid_Mid_Conv           ()
'       Private Sub             Draw_HalfConv               ()

'       Private Sub             Arc                         ()
'       Private Sub             Line                        ()

'       Private Function        CalcXVB                     ()
'       Private Function        CalcYVB                     ()
'--------------------------------------------------------------------------------

'PB 23 APR11. Get_Pt_PCS ==> Pt_PCS. CalcScale, CalcXVB, CalcYVB moved to clsSeal.

Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports clsLibrary11


Partial Public Class IPE_clsESeal
    Inherits IPE_clsSeal

    '*******************************************************************************
    '*                    MEMBER VARIABLE DECLARATIONS  - BEGIN                    *
    '*******************************************************************************

    '   GEOMETRY PARAMETERS:
    '   ====================
    '
    '....Intersection points between the straight segment (E5) & the adjacent 
    '........curved segments (E1 & E2).
    Private mE15 As PointF                                              '(DERIVED)
    Private mE52 As PointF                                              '(DERIVED)
    '
    '....Intersection point of various adjacent segments:
    Private mM12 As PointF
    Private mM23 As PointF
    Private mM34 As PointF
    Private mM45 As PointF

    '   Pitch(es) - Mid Convolutions.
    '   ---------
    '   Convention: Pitch is measured on the mid line.
    '........TemplateNo = "1" & "2"  
    Private mPitchM As Single

    '........TemplateNo = "1Gen" & "1GenS".
    Private mPitch_Half_M_End As Single     '....1/2  Pitch: Mid-End 1/2 Convolution.
    Private mPitchM_Mid As Single           '....Full Pitch: Mid-Mid Full Convolution
    '                                       '........Exists when NConv > 1.
    '....Segment 3:
    'Private mAlphaE3 As Single      'TODO. PB 16NOV07. To be changed to mBetaE3. 
    Private mBetaE3 As Single


    '*******************************************************************************
    '*                    MEMBER VARIABLE DECLARATIONS  - END                    *
    '*******************************************************************************


    '*******************************************************************************
    '*                        CLASS METHODS - BEGIN                                *
    '*******************************************************************************

#Region "GRAPHICS:"
    '-------------------------------------------------------------------------------
    '                          GRAPHICS METHODS - BEGIN                            '
    '-------------------------------------------------------------------------------

#Region "....OVERALL GRAPHICS (PUBLIC INTERFACE): HIGHEST LEVEL."

    Public Sub Draw(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                    ByVal margin_In() As Single, _
                    ByVal color_In() As Color, ByVal intDrawWid_In() As Integer, _
                    ByVal intDashStyle_In() As Integer, ByVal GeomType_In As String, _
                    ByVal strScaling_In As String, ByVal sngMultFacWidDir_In As Single, _
                    ByRef EnvpTopL As PointF, ByRef EnvpBotR As PointF)                     'SB 13DEC07
        '===============================================================================
        '
        '....The envelope location (output parameters) are in the "Page Coordinate" system:
        '       Top    Left  Corner: (xEnvpTopL, yEnvpTopL)
        '       Bottom Right Corner: (xEnvpBotR, yEnvpBotR)

        If grphObj_In Is Nothing = True Then Exit Sub

        '....Calculate the Scale Factor for VB graphics & the modified margins.
        Calc_MemberVariables("STD")     '....Gets mHFree & mWid.
        CalcScale(grphObj_In, size_In, margin_In, strScaling_In, sngMultFacWidDir_In, _
                  mScalePCS, mMarginMod)


        'Draw ESeal Geometries:
        '----------------------
        '
        Dim iBeg As Int16, iEnd As Int16

        If GeomType_In = "STD" Then
            iBeg = 1
            iEnd = 1

        ElseIf GeomType_In = "ADJ" Then
            iBeg = 2
            iEnd = 2

        ElseIf GeomType_In = "BOTH" Then
            '....Additional Plot: "Adjusted" Geometry superimposed on the "Standard".
            iBeg = 1
            iEnd = 2
        End If

        Dim pGeomType As String = ""
        Dim i As Int16

        For i = iBeg To iEnd

            If i = 1 Then
                pGeomType = "STD"
            ElseIf i = 2 Then
                pGeomType = "ADJ"
            End If

            DrawESeal(grphObj_In, size_In, pGeomType, _
                      color_In(i - iBeg), intDrawWid_In(i - iBeg), intDashStyle_In(i - iBeg), _
                      EnvpTopL, EnvpBotR)       'SB 13DEC07
        Next

    End Sub


#Region "........SCALING: HELPER ROUTINE: LOWER LEVEL."

    'PB 23APR11. Moved to clsSeal.
    ''....SCALING:
    'Private Sub CalcScale(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
    '                ByVal margin_In() As Single, ByVal strScalingGeom_In As String, _
    '                ByVal sngMultFacWidDir_In As Single, _
    '                ByRef scalePCS_Out As Single, ByRef marginMod_Out() As Single)
    '    '==============================================================================
    '    '
    '    '....TEMPLATE INDEPENDENT.
    '    '
    '    'This function calculates an appropriate scale based on the given Geometry Type 
    '    '....e.g. "STD" or "ADJ".
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
    '    Dim pHfreeScale As Single
    '    Dim pWidScale As Single

    '    If strScalingGeom_In = "SCALE_BY_STD" Then
    '        pHfreeScale = mHfreeStd
    '        pWidScale = mWidStd

    '    ElseIf strScalingGeom_In = "SCALE_BY_ADJ" Then
    '        pHfreeScale = mHfree
    '        pWidScale = mWid
    '    End If


    '    Dim pHfree_UUnit As Single
    '    pHfree_UUnit = mUnit.L_ConToUser(pHfreeScale)

    '    Dim pWid_UUnit As Single
    '    pWid_UUnit = mUnit.L_ConToUser(pWidScale)


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

#End Region

#End Region


#Region "........DRAW ESEAL: INTERMEDIATE LEVEL."

    Private Sub DrawESeal(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                          ByVal strGeomType_In As String, _
                          ByVal color_In As Color, ByVal intDrawWid_In As Integer, _
                          ByVal intDashStyle_In As Integer, _
                          ByRef EnvpTopL As PointF, ByRef EnvpBotR As PointF)               'SB 13DEC07
        '================================================================================
        'Refer to the Figure 1 of the Project Definition Document for the
        'World Coordinate System' (WCS).

        '....Calculate the relevant geometric angles for drawing:       'TODO: pThetaE1Draw.
        Dim thetaE1Draw As Single
        Dim thetaM1Draw As Single

        Dim thetaM3Draw As Single
        Dim thetaM5Draw As Single

        If strGeomType_In = "STD" Then
            thetaE1Draw = mThetaE(1)
            thetaM1Draw = mThetaM(1)
            thetaM3Draw = ThetaM(3)
            thetaM5Draw = ThetaM(5)

            '....Calc_MemberVariables("STD") has already been called in the prior 
            '........higher level routine "Draw".

        ElseIf strGeomType_In = "ADJ" Then
            thetaE1Draw = mThetaE(1) + mDThetaE1
            thetaM1Draw = mThetaM(1) + mDThetaM1
            thetaM3Draw = ThetaM(3) + mDThetaM3
            thetaM5Draw = ThetaM(5) + mDThetaM5

            Calc_MemberVariables("ADJ")     'TODO. PB 13NOV07. See if this is necessary.
        End If

        CalcOrigWCS(grphObj_In, size_In, mMarginMod, strGeomType_In)


        'Draw the ESeal.
        '--------------
        '....End 1/2 Convolutions.  Irrespective of TemplatNo.
        Draw_EndHalfConvs(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          thetaE1Draw, EnvpTopL, EnvpBotR)          'SB 13DEC07

        '....Mid Convolutions.
        Dim pYOffset As Single
        Dim iConv As Int16

        If mTemplateNo = "1" Or mTemplateNo = "2" Then
            '-----------------------------------------

            For iConv = 1 To mNConv
                '....Starting Y Coordinate of the Mid Convolution in WCS.
                pYOffset = (iConv - 1) * mPitchM
                Draw_MidConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                             pYOffset, thetaM1Draw, thetaM3Draw)
            Next


        ElseIf mTemplateNo = "1Gen" Or mTemplateNo = "1GenS" Then
            '----------------------------------------------------
            Draw_Mid_End_HalfConvs(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                                   thetaM1Draw, thetaM3Draw)

            If mNConv > 1 Then

                For iConv = 1 To mNConv - 1
                    pYOffset = (iConv - 1) * mPitchM_Mid
                    Draw_Mid_Mid_Conv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                                      pYOffset, thetaM3Draw, thetaM5Draw)
                Next

            End If

        End If

    End Sub


#Region "....................GRAPHICS PARAMETERS: HELPER ROUTINE: LOWEST LEVEL."

    '-------------------------------------------------------------------------------
    '                GRAPHICS PARAMETERS CALCULATION METHOD - BEGIN                '
    '-------------------------------------------------------------------------------

    Private Sub Calc_GraphicsParams(ByVal thetaE1Draw_In As Single, _
                                    ByVal thetaM1Draw_In As Single, _
                                    ByVal thetaM3Draw_In As Single)
        '===========================================================================
        'This subroutine calculates various Graphics Parameters in CYS = CARTW (WCS),  
        '....which are needed to draw an E-Seal graphics on a selected device. 
        '....e.g. picture box or printer object.
        '
        'Refer to the Figure 1 of the Theoretical Manual for the 
        '....World Coordinate System'(WCS).
        '
        'Before plotting, the above graphics parameters in WCS will be converted to 
        '....the 'VB Coordinate System' (PCS) attached to the picture box. 
        '....This conversion will be done in a seperate routine.

        If POrient = "External" Then
            Calc_GraphicsParams_External(thetaE1Draw_In, thetaM1Draw_In, thetaM3Draw_In)

        ElseIf POrient = "Internal" Then
            Calc_GraphicsParams_Internal(thetaE1Draw_In, thetaM1Draw_In, thetaM3Draw_In)
        End If

        If mTemplateNo Is Nothing = False Then
            If mTemplateNo.Contains("1Gen") Then
                '....Calculate Arc length of the cross-section profile of the half model 
                '........in the meridional direction. 
                '....TemplateNo = "1Gen" ("1GenS").
                mLenArcHalfModel = Calc_LenArc()
            End If
        End If
    End Sub


    Private Function Calc_LenArc() As Single
        '===================================    
        Dim pLen As Single
        Dim i As Int16

        '   End Convolution:  
        '   ================
        Dim pLen_End As Single
        For i = 1 To 3
            pLen_End = pLen_End + mRadE(i) * mThetaE(i) * gcFAC_RAD
        Next

        '....Add the complementary angle arc.
        pLen_End = pLen_End + mRadE(3) * mBetaE3 * gcFAC_RAD


        '   Mid Convolution:
        '   ================
        '
        Dim iStep As Int16

        If mTemplateNo = "1Gen" Then
            '....Six circular arc segments (Segment 3 comes twice). 
            iStep = 1

        ElseIf mTemplateNo = "1GenS" Then
            '....Four circular arc & two straight segments. (Segment 3 comes twice).
            iStep = 2
        End If


        '   ....Mid-End Convolution.
        '   ------------------------
        '
        Dim pLen_MidEnd As Single
        For i = 1 To 3 Step iStep
            pLen_MidEnd = pLen_MidEnd + mRadM(i) * mThetaM(i) * gcFAC_RAD
        Next

        If mTemplateNo = "1GenS" Then
            '....Add the straight segment length as iStep = 2 above.
            pLen_MidEnd = pLen_MidEnd + mLFlatM_End 'SB 09APR08
        End If

        pLen = pLen_End + pLen_MidEnd


        If mNConv > 1 Then
            '
            '   ....Mid-Mid Convolution.
            '   ------------------------
            '
            Dim pLen_MidMid As Single
            For i = 3 To 5 Step iStep
                pLen_MidMid = pLen_MidMid + mRadM(i) * mThetaM(i) * gcFAC_RAD
            Next

            If mTemplateNo = "1GenS" Then
                '....Add the straight segment length as iStep = 2 above.
                pLen_MidMid = pLen_MidMid + mLFlatM_Mid 'SB 09APR08
            End If

            pLen = pLen + (mNConv - 1) * pLen_MidMid

        End If

        Return pLen

    End Function


    'TODO: PB 13NOV07. Review if any subroutine can be created for _External & _Internal.  
    Private Sub Calc_GraphicsParams_External(ByVal thetaE1Draw_In As Single, _
                                             ByVal thetaM1Draw_In As Single, _
                                             ByVal thetaM3Draw_In As Single)
        '===========================================================================
        'This subroutine calculates various Graphics Parameters in CYS = CARTW (WCS).

        'Input Data   : (All dimensions in inches)
        '--------------
        '   DControl   : Control Dia of ESeal
        '   T          : Thickness
        '
        '   Radii & Angles of the End Convolution:
        '       mRadE (1), thetaE1Draw_In. 
        '       mRadE (i), mThetaE(i),    i = 2 to 3.
        '
        '
        '   Radii & Angles of the Mid Convolution:      
        '   --------------------------------------
        '   Template = "1Gen":
        '       mRadM(1), thetaM1Draw_In
        '       mRadM(3), thetaM3Draw_In
        '       mRadM(i), mThetaM(i),     i = 2, 4
        '       mRadM(5).
        '
        '   Template = "1GenS":
        '       mRadM(1), thetaM1Draw_In
        '       mRadM(3), thetaM3Draw_In
        '       mRadM(5).
        '       mLFlatM                :  i = 2 & 4.


        'Output - Center points coordinates:
        '-----------------------------------
        '
        ' End Convolution:
        '   1. mCenE (i), i = 1 to 3
        '   2. mAlphaE3: Complementary angle for the End convolution segment E(3).
        '
        '   TemplateNo = 2: Straight segment points.
        '   3. mE15, mE52 
        '
        ' Mid Convolution:
        '
        '   Template = "1Gen":
        '       mCenM (i): Center Point coordinates , i = 1 to 5. 
        '
        '    Template = "1GenS":
        '       mCenM (i): Center Point coordinates , i = 1, 3 & 5.
        '       mM12 & mM23 : Straight Segment M2.
        '       mM34 & mM45 : Straight Segment M4.

        '   ABBREVIATION:  
        '   -------------
        '       CYS: Coordinate System.

        Dim pTheta As Single
        Dim pRad As Single

        '....Coordinates of an arbitrary point/ 
        Dim pPt_P As PointF         '....CYS = CARTP
        Dim pPt_1 As PointF         '....CYS = CART1

        Dim pAlpha As Single        '.....Angle between the X axes of the initial and 
        '                           '........the final coordnate systems.

        'CALCULATE THE COORDINATES OF THE CENTER POINTS IN WCS.
        '=====================================================

        '   End Convolution:
        '   ===============
        '
        '       Segment E(1):
        '       ------------       

        With mCenE(1)
            .X = 0.5 * (DControl + T) + mRadE(1)
            .Y = 0.0#
        End With

        '       Segment E(2):
        '       -------------
        '
        '....CSYS = CYL-E1 (The datum axis along -X direction. Angle measured CCW).             
        pRad = mRadE(1) + mRadE(2)
        pTheta = thetaE1Draw_In

        '....CSYS = CARTP-E1 (XP axis along -X and YP along -Y).
        pPt_P = CylToCart(pRad, pTheta)

        '....CSYS = CART1_E1 (X1 axis along X and Y1 along Y).
        pAlpha = 180    '....CCW angle of rotation between X1 and X axes.
        pPt_1 = RotateAxes(pPt_P, pAlpha)

        '....CSYS = CARTW
        mCenE(2) = TranslateAxes(pPt_1, mCenE(1))


        If mTemplateNo = "2" Then
            '--------------------
            '
            '   Segment E5 (Straight):
            '   ----------------------

            '   ASSUMPTION for the following derivation for sign:
            '       The angle thetaE1Draw_In > 90.

            '       Begin Point: mE15.
            '       ------------------
            '
            '...CSYS = CYL_E1
            pRad = mRadE(1)
            pTheta = thetaE1Draw_In

            '...CSYS = CARTP_E1
            Dim pE15_P As PointF
            pE15_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E1
            Dim pE15_1 As PointF
            pAlpha = 180
            pE15_1 = RotateAxes(pE15_P, pAlpha)

            '....CSYS = WCS
            mE15 = TranslateAxes(pE15_1, mCenE(1))


            '       End Point: mE52.
            '       ----------------
            '
            '....Angle made by LFlatE with the vertical.
            Dim pBetaE1 As Single
            pBetaE1 = thetaE1Draw_In - 90

            '....Coordinate increments from the end of Segment 1 to the
            '........end of straight leg (Segment 5).
            '
            Dim pHFlatE As Single
            pHFlatE = mLFlatE * CosD(pBetaE1)

            With mE52
                .X = mE15.X + mLFlatE * CosD(pBetaE1)
                .Y = mE15.Y + mLFlatE * SinD(pBetaE1)
            End With


            'Correct the Coordinates of 'CenE (2)'
            '------------------------------------
            '....CSYS = WCS.

            With mCenE(2)
                .X = mCenE(2).X + mLFlatE * CosD(pBetaE1)
                .Y = mCenE(2).Y + mLFlatE * SinD(pBetaE1)
            End With

        End If


        '       Segment E(3):       
        '       -------------
        '
        '....CSYS = CYL-E2. The datum axis is along CenE(2)-(1).
        pRad = mRadE(2) + mRadE(3)
        pTheta = -mThetaE(2)

        '....CSYS = CARTP_E2. 
        '........(XP is along the above datum axis & YP is in the downward direction).
        pPt_P = CylToCart(pRad, pTheta)

        '....CSYS = CART1_E2
        pAlpha = -thetaE1Draw_In
        pPt_1 = RotateAxes(pPt_P, pAlpha)

        '....CSYS = CARTW
        mCenE(3) = TranslateAxes(pPt_1, mCenE(2))


        '....The angle subtended by the line mCenE(2)-(3) with the vertical.
        Dim pThetaDiff As Single
        pThetaDiff = thetaE1Draw_In - mThetaE(2)

        '....Complementary Angle with the horizontal.
        mBetaE3 = 90 - pThetaDiff


        '   Mid Convolution:
        '   ================
        '       Segment M1: 
        '       -----------
        '
        mCenM(1) = mCenE(1)

        If mTemplateNo = "1" Or mTemplateNo = "2" Or mTemplateNo = "1GenS" Then
            '==================================================================
            '....Template No = "1", "2" & "1GenS"

            '   Segment M2 (Straight)
            '   ---------------------
            '
            '   Flat section :
            '........Pertains to the adjusted geometry.
            '
            '....Assumption for derivation: thetaM1Draw_In > 90 per sketch.


            '       Begin Point: mM12.
            '       ------------------
            '
            '...CSYS = CYL_E1
            pRad = mRadM(1)
            pTheta = -thetaM1Draw_In

            '...CSYS = CARTP_E1
            Dim pM12_P As PointF
            pM12_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E1
            Dim pM12_1 As PointF
            pAlpha = 180
            pM12_1 = RotateAxes(pM12_P, pAlpha)

            '....CSYS = WCS
            mM12 = TranslateAxes(pM12_1, mCenE(1))


            '       Begin Point: mM23.
            '       ------------------
            '
            '....Angle subtended by the Segment M2 with the vertical.
            Dim pBetaM2 As Single
            pBetaM2 = thetaM1Draw_In - 90

            Dim pHFlatM As Single
            'pHFlatM = mLFlatM * CosD(pBetaM2)
            pHFlatM = mLFlatM_End * CosD(pBetaM2)   'SB 08APR08

            With mM23
                .X = mM12.X + pHFlatM
                '.Y = mM12.Y - mLFlatM * SinD(pBetaM2)
                .Y = mM12.Y - mLFlatM_End * SinD(pBetaM2)   'SB 08APR08
            End With


            '   Segment M3:
            '   -----------  
            With mCenM(3)
                .X = mM23.X + mRadM(3) * SinD(pBetaM2)
                .Y = mM23.Y + mRadM(3) * CosD(pBetaM2)
            End With


            '   Segment M4 (Straight):
            '   ----------------------
            '
            '   Flat section :
            '........Pertains to the adjusted geometry.


            '       Begin Point: mM34.
            '       ------------------
            '
            '...CSYS = CYL_M3. The datum vertically upwards. 
            pRad = mRadM(3)
            pTheta = thetaM3Draw_In

            '...CSYS = CARTP_M3.  XP along datum and YP to the LHS. 
            Dim pM34_P As PointF
            pM34_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M3. X1-Y1 is same as the XP-YP.
            Dim pM34_1 As PointF
            pAlpha = 0
            pM34_1 = RotateAxes(pM34_P, pAlpha)

            '....CSYS = WCS
            mM34 = TranslateAxes(pM34_1, mCenM(3))


            '       End Point: mM45.
            '       ---------------
            '
            '....Angle subtended by the Segment M4 with the vertical.
            Dim pBetaM4 As Single
            pBetaM4 = pBetaM2

            pHFlatM = mLFlatM_Mid * CosD(pBetaM2)   'SB 08APR08


            With mM45
                .X = mM34.X - pHFlatM
                '.Y = mM34.Y - mLFlatM * SinD(pBetaM4)
                .Y = mM34.Y - mLFlatM_Mid * SinD(pBetaM4)   'SB 08APR08
            End With


            '   Segment M5:
            '   -----------
            With mCenM(5)
                .X = mM45.X - mRadM(5) * SinD(pBetaM4)
                .Y = mM45.Y + mRadM(5) * CosD(pBetaM4)
            End With


        ElseIf mTemplateNo = "1Gen" Then
            '===========================
            '
            '   Segment M2: 
            '   ----------
            '
            '....CSYS = CYL_E1. (The datum line is vertically downwards and angle is measuresd CCW).
            pTheta = -thetaM1Draw_In
            pRad = mRadM(1) + mRadM(2)

            '....CSYS = CARTP_E1 (XP is along datum line and YP extends to the RHS).
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E1
            pAlpha = 180
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(2) = TranslateAxes(pPt_1, mCenE(1))


            '   Segment M3: 
            '   -----------

            '....CSYS = CYL_M2. Datum along CenM (2)-(1). Angle measured CCW.
            pTheta = mThetaM(2)
            pRad = mRadM(2) - mRadM(3)

            '....CSYS = CARTP_M2. XP along the datum and YP upwards.
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M2
            pAlpha = thetaM1Draw_In
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(3) = TranslateAxes(pPt_1, mCenM(2))


            '   Segment M4: 
            '   ----------
            '
            '....CSYS = CYL_M3. The datum vertically upwards. 
            pTheta = thetaM3Draw_In + 180
            pRad = mRadM(4) - mRadM(3)

            '....CSYS = CARTP_M3. XP along datum and YP to the LHS. 
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M3. X1-Y1 is same as the XP-YP.
            pAlpha = 0
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(4) = TranslateAxes(pPt_1, mCenM(3))

            '
            '   Segment M5: 
            '   ----------
            '
            '....CSYS = CYL_M4. The datum line is along CenM (4)-(3).
            pTheta = mThetaM(4)
            pRad = mRadM(4) + mRadM(5)

            '....CSYS = CARTP_M4. XP along the datum and YP downwards.
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M4
            pAlpha = -thetaM3Draw_In
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(5) = TranslateAxes(pPt_1, mCenM(4))

        End If

    End Sub


    Private Sub Calc_GraphicsParams_Internal(ByVal thetaE1Draw_In As Single, _
                                             ByVal thetaM1Draw_In As Single, _
                                             ByVal thetaM3Draw_In As Single)
        '===========================================================================   
        'This subroutine calculates various Graphics Parameters in CYS = CARTW (WCS).

        'Input Data   : (All dimensions in inches)
        '--------------
        '   DControl   : Control Dia of ESeal
        '   T          : Thickness
        '
        '   Radii & Angles of the End Convolution:
        '       mRadE (1), thetaE1Draw_In. 
        '       mRadE (i), mThetaE(i), i = 2 to 3.
        '
        '
        '   Radii & Angles of the Mid Convolution:      
        '   --------------------------------------
        '   Template = "1Gen":
        '       mRadM(1), thetaM1Draw_In
        '       mRadM(3), thetaM3Draw_In
        '       mRadM(i), mThetaM(i),     i = 2, 4
        '       mRadM(5).
        '
        '   Template = "1GenS":
        '       mRadM(1), thetaM1Draw_In
        '       mRadM(3), thetaM3Draw_In
        '       mRadM(5).
        '       mLFlatM                :  For i = 2 & 4.


        'Output - Center points coordinates:
        '-----------------------------------
        '
        ' End Convolution:
        '   1. mCenE (i), i = 1 to 3
        '   2. mAlphaE3: Complementary angle for the End convolution segment E(3).
        '
        '   TemplateNo = 2: Straight segment points.
        '   3. mE15, mE52.
        '
        ' Mid Convolution:
        '
        '   Template = "1Gen":
        '       mCenM (i): Center Point coordinates , i = 1 to 5. 
        '
        '    Template = "1GenS":
        '       mCenM (i): Center Point coordinates , i = 1, 3 & 5.
        '       mM12 & mM23 : Straight Segment M2.
        '       mM34 & mM45 : Straight Segment M4.

        '   ABBREVIATION:  
        '   -------------
        '       CYS: Coordinate System.

        Dim pTheta As Single
        Dim pRad As Single

        '....Coordinates of an arbitrary point/ 
        Dim pPt_P As PointF         '....CYS = CARTP
        Dim pPt_1 As PointF         '....CYS = CART1

        Dim pAlpha As Single        '.....Angle between the X axes of the initial and 
        '                           '........the final coordnate systems.

        'CALCULATE THE COORDINATES OF THE CENTER POINTS IN WCS.
        '=====================================================

        '   End Convolution:
        '   ===============
        '
        '       Segment E(1):
        '       ------------
        With mCenE(1)
            .X = 0.5 * (DControl - T) - mRadE(1)
            .Y = 0.0#
        End With

        '       Segment E(2):
        '       -------------
        '
        '....CSYS = CYL-E1  (The datum axis along X direction. Angle measured CW.)
        pRad = mRadE(1) + mRadE(2)
        pTheta = -thetaE1Draw_In

        '....CSYS = CARTP-E1    (XP along datum and YP to the LHS.)
        pPt_P = CylToCart(pRad, pTheta)

        '....CSYS = CART1_E1    (X1-Y1 is same as the XP-YP.)
        pAlpha = 0
        pPt_1 = RotateAxes(pPt_P, pAlpha)

        '....CSYS = CARTW
        mCenE(2) = TranslateAxes(pPt_1, mCenE(1))


        If mTemplateNo = "2" Then

            '   Segment E5 (Straight):
            '   ----------------------

            '   ASSUMPTION for the following derivation for sign:
            '       The angle thetaE1Draw_In > 90.

            '       Begin Point: mE15.
            '       ------------------
            '
            '...CSYS = CYL_E1
            pRad = mRadE(1)
            pTheta = -thetaE1Draw_In

            '...CSYS = CARTP_E1
            Dim pE15_P As PointF
            pE15_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E1
            Dim pE15_1 As PointF
            pAlpha = 0
            pE15_1 = RotateAxes(pE15_P, pAlpha)

            '....CSYS = WCS
            mE15 = TranslateAxes(pE15_1, mCenE(1))


            '       End Point: mE52.
            '       ----------------
            '
            '....Angle made by LFlatE with the vertical.
            Dim pBetaE1 As Single
            pBetaE1 = thetaE1Draw_In - 90

            '....Coordinate increments from the end of Segment 1 to the
            '........end of straight leg (Segment 5).
            '
            Dim pHFlatE As Single
            pHFlatE = mLFlatE * CosD(pBetaE1)

            With mE52
                .X = mE15.X - mLFlatE * CosD(pBetaE1)
                .Y = mE15.Y + mLFlatE * SinD(pBetaE1)
            End With


            'Correct the Coordinates of 'CenE (2)'
            '------------------------------------
            '....CSYS = WCS.
            '
            With mCenE(2)
                .X = mCenE(2).X - mLFlatE * CosD(pBetaE1)
                .Y = mCenE(2).Y + mLFlatE * SinD(pBetaE1)
            End With

        End If


        '       Segment E(3):       
        '       -------------
        '
        '....CSYS = CYL-E1. The datum axis is along CenE(2)-(1). Angle measured CCW.
        pRad = mRadE(2) + mRadE(3)
        pTheta = mThetaE(2)

        '....CSYS=CARTP_E3
        '....(XP is along the above datum axis & YP is in the downward direction).
        pPt_P = CylToCart(pRad, pTheta)

        '....CSYS = CART1_E3    (X1 axis along X and Y1 along Y).
        pAlpha = 180 + thetaE1Draw_In
        pPt_1 = RotateAxes(pPt_P, pAlpha)


        '....CSYS = CARTW
        mCenE(3) = TranslateAxes(pPt_1, mCenE(2))

        '....The angle subtended by the line mCen(2)-(3) with the vertical.
        Dim thetaDiff As Single
        thetaDiff = (180 - thetaE1Draw_In) + mThetaE(2)

        '....Complementary Angle with the horizontal.
        mBetaE3 = thetaDiff - 90


        '   Mid Convolution:
        '   ================
        '       Segment M1: 
        '       -----------
        '
        mCenM(1) = mCenE(1)

        If mTemplateNo = "1" Or mTemplateNo = "2" Or mTemplateNo = "1GenS" Then
            '==================================================================
            '....Template No = "1", "2" & "1GenS"

            '   Segment M2 (Straight) 
            '   ---------------------
            '
            '....Assumption for derivation: thetaM1Draw_In > 90 per sketch.

            '       Begin Point: mM12.
            '       ------------------
            '
            '...CSYS = CYL_E1
            pRad = mRadM(1)
            pTheta = thetaM1Draw_In

            '...CSYS = CARTP_E1
            Dim pM12_P As PointF
            pM12_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E1
            Dim pM12_1 As PointF
            pAlpha = 0
            pM12_1 = RotateAxes(pM12_P, pAlpha)

            '....CSYS = WCS
            mM12 = TranslateAxes(pM12_1, mCenE(1))


            '       End Point: mM23.
            '       ----------------
            '
            '....Angle subtended by the Segment M2 with the vertical.
            Dim pBetaM2 As Single
            pBetaM2 = thetaM1Draw_In - 90

            Dim pHFlatM As Single
            'pHFlatM = mLFlatM * CosD(pBetaM2)
            pHFlatM = mLFlatM_End * CosD(pBetaM2)           'SB 08APR08

            With mM23
                .X = mM12.X - pHFlatM
                '.Y = mM12.Y - mLFlatM * SinD(pBetaM2)
                .Y = mM12.Y - mLFlatM_End * SinD(pBetaM2)   'SB 08APR08
            End With


            '   Segment M3:
            '   -----------
            With mCenM(3)
                .X = mM23.X - mRadM(3) * SinD(pBetaM2)
                .Y = mM23.Y + mRadM(3) * CosD(pBetaM2)
            End With


            '   Segment M4 (Straight):
            '   ----------------------
            '
            '       Begin Point: mM34.
            '       ------------------
            '
            '...CSYS = CYL_M3. The datum vertically downwards.
            pRad = mRadM(3)
            pTheta = -thetaM3Draw_In

            '....CSYS = CARTP_M3.   XP along datum and YP to the RHS.
            Dim pM34_P As PointF
            pM34_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M3. X1-Y1 is same as the XP-YP.
            Dim pM34_1 As PointF
            pAlpha = 180
            pM34_1 = RotateAxes(pM34_P, pAlpha)

            '....CSYS = WCS
            mM34 = TranslateAxes(pM34_1, mCenM(3))


            '       End Point: mM45.
            '       ----------------
            '
            '....Angle subtended by the Segment M4 with the vertical.
            Dim pBetaM4 As Single
            pBetaM4 = pBetaM2
            pHFlatM = mLFlatM_Mid * CosD(pBetaM2)       'SB 08APR08

            With mM45
                .X = mM34.X + pHFlatM
                .Y = mM34.Y - pHFlatM * TanD(pBetaM4)   'SB 08APR08
            End With


            '   Segment M5:
            '   -----------
            With mCenM(5)
                .X = mM45.X + mRadM(5) * SinD(pBetaM4)
                .Y = mM45.Y + mRadM(5) * CosD(pBetaM4)
            End With


        ElseIf mTemplateNo = "1Gen" Then
            '===========================
            '
            '   Segment M2: 
            '   ----------
            '
            '....CSYS = CYL_E1. (the datum line is vertically upwards and angle is
            '....                measured CCW).
            pTheta = thetaM1Draw_In
            pRad = mRadM(1) + mRadM(2)

            '....CSYS = CARTP_E1 (XP is along datum line and YP extends to the LHS).
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E1       
            pAlpha = 0
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(2) = TranslateAxes(pPt_1, mCenE(1))


            '   Segment M3: 
            '   -----------
            '
            '....CSYS = CYL_M2. Datum along CenM (2)-(1). Angle measured CW.
            pTheta = -mThetaM(2)
            pRad = mRadM(2) - mRadM(3)

            '....CSYS = CARTP_M2. XP along the datum and YP downwards.
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M2
            pAlpha = 180 - thetaM1Draw_In
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(3) = TranslateAxes(pPt_1, mCenM(2))


            '   Segment M4: 
            '   ----------
            '
            '....CSYS = CYL_M3.
            pTheta = -180 - thetaM3Draw_In
            pRad = mRadM(4) - mRadM(3)

            '....CSYS = CARTP_M3
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M3
            pAlpha = 180
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(4) = TranslateAxes(pPt_1, mCenM(3))

            '
            '   Segment M5: 
            '   ----------
            '
            '....CSYS = CYL_M4.
            pTheta = -mThetaM(4)
            pRad = mRadM(4) + mRadM(5)

            '....CSYS = CARTP_M4
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_M4
            pAlpha = 180 + thetaM3Draw_In
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            mCenM(5) = TranslateAxes(pPt_1, mCenM(4))

        End If

    End Sub


    Private Function CylToCart(ByVal Rad As Single, ByVal Theta As Single) As PointF
        '============================================================================   
        CylToCart.X = Rad * CosD(Theta)
        CylToCart.Y = Rad * SinD(Theta)

    End Function

    'SG 18APR11
    'Private Function RotateAxes(ByVal pPt_P As PointF, ByVal Alpha As Single) As PointF
    '    '===============================================================================    
    '    RotateAxes.X = pPt_P.X * CosD(Alpha) + pPt_P.Y * SinD(Alpha)
    '    RotateAxes.Y = -pPt_P.X * SinD(Alpha) + pPt_P.Y * CosD(Alpha)

    'End Function


    'Private Function TranslateAxes(ByVal pPt_1 As PointF, ByVal Org_1_W As PointF) As PointF
    '    '===================================================================================
    '    TranslateAxes.X = pPt_1.X + Org_1_W.X
    '    TranslateAxes.Y = pPt_1.Y + Org_1_W.Y

    'End Function

    '-------------------------------------------------------------------------------
    '                  GRAPHICS PARAMETERS CALCULATION METHOD - END                '
    '-------------------------------------------------------------------------------

#End Region


#Region "....................CALCULATIONS: ZClear: LOWEST LEVEL."

    Public Function CalcZClear(ByVal HFree_In As Single) As Single
        '==========================================================
        'This function selects the necessary diametral clearance for a given free height.
        '
        '....Valid for: E-Seal only.
        '
        'STAND-ALONE FUNCTION:
        '---------------------
        '....Used in other modules (e.g. clsSealSelect), possibly without being associated
        '........with any particular seal design.

        '
        ' 'Unit Aware' Function.
        ' ----------------------

        'The following array table is in English unit e.g. in.
        '-----------------------------------------------------
        '(Ref : Advanced Products Catalogue - Page C-9, E-Ring)
        '....HfreeArray(UBound) value (e.g. 0.4) is chosen aribitrarily.

        Dim hFreeArray() As Single = {0.09, 0.125, 0.18, 0.25, 0.4}
        Dim zClearArray() As Single = {0.003, 0.003, 0.005, 0.006, 0.008}

        '....Lower & Upper Bounds of the arrays
        Dim iLow As Integer = LBound(hFreeArray)
        Dim iUp As Integer = UBound(hFreeArray)

        'Calculate ZClear.
        '-----------------
        Dim HFreeInEng As Single, ZClearEng As Single
        Dim i As Integer

        '....Convert mUnit.System ===> English Unit.
        HFreeInEng = HFree_In / mUnit.CFacConL

        '....Select the recommended Diametral Clearance (Z).
        If HFreeInEng <= hFreeArray(iLow) Then
            ZClearEng = zClearArray(iLow)

        ElseIf HFreeInEng > hFreeArray(iUp) Then
            ZClearEng = zClearArray(iUp)

        Else
            i = 1
            Do While HFreeInEng >= hFreeArray(i)
                i = i + 1
            Loop

            ZClearEng = zClearArray(i)
        End If

        '....Convert English Unit ===> mUnit.System.
        CalcZClear = ZClearEng * mUnit.CFacConL

    End Function

#End Region


    Private Sub CalcOrigWCS(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                            ByRef marginMod_In() As Single, ByVal strGeomType_In As String)     'TODO: Review.
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
        '----------------------------------------------------------------
        Dim xVB_Right As Single, yVB_Botm As Single, yVB_Top As Single
        Dim yVB_Botm_EndConv As Single, yVB_Botm_MidConv As Single
        Dim yVB_Top_EndConv As Single, yVB_Top_MidConv As Single

        xVB_Right = mUnit.L_ConToUser((mRadE(3) - mCenE(3).Y) + 0.5 * T) * mScalePCS
        mXVB_OrigWCS = size_In.Width - marginMod_In(2) - xVB_Right


        If POrient = "External" Then
            '-----------------------
            yVB_Botm_EndConv = mUnit.L_ConToUser(mCenE(1).X - mRadE(1) - 0.5 * T) * mScalePCS
            yVB_Botm_MidConv = mUnit.L_ConToUser(mCenM(5).X - mRadM(5) - 0.5 * T) * mScalePCS

            If yVB_Botm_EndConv <= yVB_Botm_MidConv Then
                yVB_Botm = yVB_Botm_EndConv
            Else
                yVB_Botm = yVB_Botm_MidConv
            End If

            mYVB_OrigWCS = size_In.Height - marginMod_In(4) + yVB_Botm


        ElseIf POrient = "Internal" Then
            '---------------------------
            yVB_Top_EndConv = mUnit.L_ConToUser(mCenE(1).X + mRadE(1) + 0.5 * T) * mScalePCS
            yVB_Top_MidConv = mUnit.L_ConToUser(mCenM(5).X + mRadM(5) + 0.5 * T) * mScalePCS

            If yVB_Top_EndConv >= yVB_Top_MidConv Then
                yVB_Top = yVB_Top_EndConv
            Else
                yVB_Top = yVB_Top_MidConv
            End If

            mYVB_OrigWCS = marginMod_In(3) + yVB_Top   'size_In.Height +
        End If

    End Sub



#End Region


#Region "............DRAW END HALF CONVOLUTIONS: LOWER LEVEL."

    Private Sub Draw_EndHalfConvs(ByVal grphObj_In As Graphics, _
                                  ByVal color_In As Color, _
                                  ByVal intDrawWid_In As Integer, _
                                  ByVal intDashStyle_In As Integer, _
                                  ByVal thetaE1Draw As Double, _
                                  ByRef EnvpTopL As PointF, ByRef EnvpBotR As PointF)       'SG 01JUL11
        '=================================================================================
        '
        'This subroutine draws the RHS & LHS Halves of the End Convolution. 

        'TEMPLATE dependent calculation.
        '-------------------------------
        '....Axial distance between the RHS 1/2 & LHS 1/2 Convolutions.
        Dim pYOffset As Single

        If mTemplateNo = "1" Or mTemplateNo = "2" Then
            pYOffset = mPitchM * mNConv

        ElseIf mTemplateNo = "1Gen" Or mTemplateNo = "1GenS" Then
            pYOffset = 2 * mPitch_Half_M_End

            If mNConv > 1 Then
                pYOffset = pYOffset + (mNConv - 1) * mPitchM_Mid
            End If

        End If

        '---------------------------------------------------------------------------
        '                     TEMPLATE independent calculations.                   '
        '---------------------------------------------------------------------------

        'Segment Angles:
        '===============

        Dim pAngE_Start(2, 3) As Single, pAngE_Sweep(2, 3) As Single


        If POrient = "External" Then
            '=======================

            'RHS 1/2 Convolution : i = 1
            '----------------------------
            '....Segment 1 (j = 1)
            pAngE_Start(1, 1) = 90 - thetaE1Draw
            pAngE_Sweep(1, 1) = thetaE1Draw

            '....Segment 2 (j = 2)
            pAngE_Start(1, 2) = 270 - thetaE1Draw
            pAngE_Sweep(1, 2) = mThetaE(2)

            '....Segment 3 (j = 3)
            pAngE_Start(1, 3) = -mThetaE(3)
            pAngE_Sweep(1, 3) = mThetaE(3) + mBetaE3


            'LHS 1/2 Convolution : i = 2
            '---------------------------
            '....Segment 1 : j = 1
            pAngE_Start(2, 1) = 90
            pAngE_Sweep(2, 1) = thetaE1Draw

            '....Segment 2 : j = 2
            pAngE_Start(2, 2) = -(90 - thetaE1Draw)
            pAngE_Sweep(2, 2) = -mThetaE(2)

            '....Segment 3 : j = 3
            pAngE_Start(2, 3) = 180 - mBetaE3
            pAngE_Sweep(2, 3) = mBetaE3 + mThetaE(3)


        ElseIf POrient = "Internal" Then
            '============================

            'RHS 1/2 Convolution : i = 1
            '----------------------------
            '....Segment 1 (j = 1)
            pAngE_Start(1, 1) = -90
            pAngE_Sweep(1, 1) = thetaE1Draw

            '....Segment 2 (j = 2)
            pAngE_Start(1, 2) = 90 + thetaE1Draw
            pAngE_Sweep(1, 2) = -mThetaE(2)

            '....Segment 3 : j = 3
            pAngE_Start(1, 3) = -mBetaE3
            pAngE_Sweep(1, 3) = mBetaE3 + mThetaE(3)


            'LHS 1/2 Convolution : i = 2
            '---------------------------
            '....Segment 1 : j = 1
            pAngE_Start(2, 1) = 270
            pAngE_Sweep(2, 1) = -thetaE1Draw

            '....Segment 2 : j = 2
            pAngE_Start(2, 2) = (90 - thetaE1Draw)
            pAngE_Sweep(2, 2) = mThetaE(2)


            '....Segment 3 : j = 3
            pAngE_Start(2, 3) = 180 + mBetaE3
            pAngE_Sweep(2, 3) = -(mBetaE3 + mThetaE(3))

        End If


        'Draw the End Convolutions
        '=========================
        Dim i As Integer, j As Integer, k As Integer
        Dim pCen As PointF
        Dim pSign As Integer


        '   Draw the Thickness Lines.
        '   -------------------------
        '   ....k = -1 : Inside Surface
        '   ....k =  0 : Mid    Line
        '   ....k =  1 : Outside Surface

        Dim kBeg As Integer, kEnd As Integer, kStep As Integer
        kBeg = -1
        kEnd = 1
        kStep = 2

        For k = kBeg To kEnd Step kStep         '....Thickness Loop - BEGIN

            For i = 1 To 2                      '....Convolution Loop - BEGIN
                '                               '........i = 1 :  RHS 1/2 Convolution, 
                '                               '........i = 2 :  LHS 1/2 Convolution
                '
                For j = 1 To 3                  '....Segment Loop - BEGIN

                    '....Center coordinates in WCS:
                    pCen.X = mCenE(j).X

                    If i = 1 Then                   '....RHS 1/2 Convolution
                        pCen.Y = mCenE(j).Y

                    ElseIf i = 2 Then               '....LHS 1/2 Convolution
                        pCen.Y = -mCenE(j).Y + pYOffset
                    End If

                    '....Center coordinates in PCS:
                    Dim pCen_PCS As PointF
                    pCen_PCS = Pt_PCS(pCen)


                    '....Arc Start & Sweep Angles:
                    Dim pAngStart As Single, pAngSweep As Single
                    pAngStart = pAngE_Start(i, j)
                    pAngSweep = pAngE_Sweep(i, j)

                    pSign = -(-1) ^ j

                    Dim radEjk As Single
                    radEjk = mUnit.L_ConToUser(mRadE(j) + pSign * k * mT * 0.5)
                    Dim pRad_PCS As Single = radEjk * mScalePCS

                    Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                        pCen_PCS, pRad_PCS, pAngStart, pAngSweep)


                    '....Store Seal Envelope point coordinates, required for the
                    '....proposal drawing.      
                    '-----------------------------------------------------------
                    If j = 3 And k = kEnd Then
                        If i = 1 Then
                            EnvpBotR.X = pCen_PCS.X + radEjk * mScalePCS
                        ElseIf i = 2 Then
                            EnvpTopL.X = pCen_PCS.X - radEjk * mScalePCS
                        End If
                    End If

                    If j = 1 And k = kEnd Then
                        If POrient = "External" Then
                            EnvpBotR.Y = pCen_PCS.Y + radEjk * mScalePCS
                        ElseIf POrient = "Internal" Then
                            EnvpTopL.Y = pCen_PCS.Y - radEjk * mScalePCS
                        End If
                    End If
                    '...............................................................


                    If mTemplateNo = "2" And j = 1 Then

                        'Draw the straight segment.
                        '-------------------------
                        'TemplateNo = 2 : Flat Segment
                        '
                        'Dim xBFlatEK As Single, yBFlatEK As Single                 'SG 01JUL11
                        'Dim xEFlatEK As Single, yEFlatEK As Single
                        Dim pBFlatEK As PointF, pEFlatEK As PointF

                        Dim yBFlatE_LHS As Single, yEFlatE_LHS As Single
                        'Dim yBFlatEK_LHS As Single, yEFlatEK_LHS As Single
                        Dim pBFlatEK_LHS As PointF, pEFlatEK_LHS As PointF

                        If POrient = "External" Then
                            'xBFlatEK = mE15.X + (-k) * T * 0.5 * CosD(thetaE1Draw)
                            pBFlatEK.X = mE15.X + (-k) * T * 0.5 * CosD(thetaE1Draw)
                        ElseIf POrient = "Internal" Then
                            'xBFlatEK = mE15.X + k * T * 0.5 * CosD(thetaE1Draw)
                            pBFlatEK.X = mE15.X + k * T * 0.5 * CosD(thetaE1Draw)
                        End If

                        'yBFlatEK = mE15.Y + (-k) * T * 0.5 * SinD(thetaE1Draw)
                        pBFlatEK.Y = mE15.Y + (-k) * T * 0.5 * SinD(thetaE1Draw)

                        Dim pPtFlatE_VB(1) As PointF
                        'pPtFlatE_VB(0).X = CalcXVB(xBFlatEK, yBFlatEK)
                        'pPtFlatE_VB(0).Y = CalcYVB(xBFlatEK, yBFlatEK)
                        pPtFlatE_VB(0) = Pt_PCS(pBFlatEK)


                        If POrient = "External" Then
                            'xEFlatEK = mE52.X + (-k) * T * 0.5 * CosD(thetaE1Draw)
                            pEFlatEK.X = mE52.X + (-k) * T * 0.5 * CosD(thetaE1Draw)
                        ElseIf POrient = "Internal" Then
                            'xEFlatEK = mE52.X + k * T * 0.5 * CosD(thetaE1Draw)
                            pEFlatEK.X = mE52.X + k * T * 0.5 * CosD(thetaE1Draw)
                        End If

                        'yEFlatEK = mE52.Y + (-k) * T * 0.5 * SinD(thetaE1Draw)
                        pEFlatEK.Y = mE52.Y + (-k) * T * 0.5 * SinD(thetaE1Draw)

                        'pPtFlatE_VB(1).X = CalcXVB(xEFlatEK, yEFlatEK)
                        'pPtFlatE_VB(1).Y = CalcYVB(xEFlatEK, yEFlatEK)
                        pPtFlatE_VB(1) = Pt_PCS(pEFlatEK)


                        If i = 2 Then

                            yBFlatE_LHS = mCenE(1).Y + pYOffset + (mCenE(1).Y - mE15.Y)

                            Dim dyK As Single
                            dyK = k * T * 0.5 * SinD(thetaE1Draw)
                            'yBFlatEK_LHS = yBFlatE_LHS + dyK
                            pBFlatEK_LHS.Y = yBFlatE_LHS + dyK
                            pBFlatEK_LHS.X = pBFlatEK.X

                            'pPtFlatE_VB(0).X = CalcXVB(xBFlatEK, yBFlatEK_LHS)
                            'pPtFlatE_VB(0).Y = CalcYVB(xBFlatEK, yBFlatEK_LHS)
                            pPtFlatE_VB(0) = Pt_PCS(pBFlatEK_LHS)


                            yEFlatE_LHS = mCenE(1).Y + pYOffset + (mCenE(1).Y - mE52.Y)
                            'yEFlatEK_LHS = yEFlatE_LHS + dyK
                            pEFlatEK_LHS.Y = yEFlatE_LHS + dyK
                            pEFlatEK_LHS.X = pEFlatEK.X

                            'pPtFlatE_VB(1).X = CalcXVB(xEFlatEK, yEFlatEK_LHS)
                            'pPtFlatE_VB(1).Y = CalcYVB(xEFlatEK, yEFlatEK_LHS)
                            pPtFlatE_VB(1) = Pt_PCS(pEFlatEK_LHS)

                        End If

                        Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                             pPtFlatE_VB(0), pPtFlatE_VB(1))

                    End If

                Next            '....Segment     Loop - END (j) - 3 Segments. 
            Next                '....Convolution Loop - END (i) - RHS & LHS.
        Next                    '....Thickness   Loop - END (k) - Inner & Outer Surfaces.



        '   Draw the End Lines.
        '   ------------------
        '
        Dim pPt(2) As PointF        '....Index = 0: k = -1, Inside  Surface Point.
        '                           '....Index = 2: k =  1, Outside Surface Point.

        For i = 1 To 2              '....i = 1 :  RHS 1/2 Convolution
            '                       '....i = 2 :  LHS 1/2 Convolution

            '....Coordinates of the Center of the 3rd Curved Segment.
            pCen.X = mCenE(3).X

            If i = 1 Then
                pCen.Y = mCenE(3).Y
            ElseIf i = 2 Then
                pCen.Y = -mCenE(3).Y + pYOffset
            End If


            '....kBeg  = -1, kEnd  =  1, kStep =  2
            For k = kBeg To kEnd Step kStep

                Dim pDX As Single, pDY As Single
                pDX = (mRadE(3) + k * T * 0.5) * SinD(mThetaE(3))
                pDY = (mRadE(3) + k * T * 0.5) * CosD(mThetaE(3))


                'Thickness Index Transformation.
                '-------------------------------
                '....This following transformation is used to avoid -ive index of an
                '........array related to the position along a thickness direction. 
                '   ....kShift = 0 : Inside Surface
                '   ....kShift = 1 : Mid    Line
                '   ....kShift = 2 : Outside Surface
                '
                Dim kShift As Int16
                kShift = k + 1

                pSign = (-1) ^ i

                Dim pCen_Mod As PointF
                pCen_Mod.Y = pCen.Y + pSign * pDY

                If POrient = "External" Then
                    'pPt(kShift).X = CalcXVB(pCen.X + pDX, pCen.Y + pSign * pDY)
                    'pPt(kShift).Y = CalcYVB(pCen.X + pDX, pCen.Y + pSign * pDY)
                    pCen_Mod.X = pCen.X + pDX

                ElseIf POrient = "Internal" Then
                    'pPt(kShift).X = CalcXVB(pCen.X - pDX, pCen.Y + pSign * pDY)
                    'pPt(kShift).Y = CalcYVB(pCen.X - pDX, pCen.Y + pSign * pDY)
                    pCen_Mod.X = pCen.X - pDX
                End If

                pPt(kShift) = Pt_PCS(pCen_Mod)
            Next

            Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, pPt(0), pPt(2))


            '....Store the seal envelope point coordinates for the proposal drawing.
            '----------------------------------------------------------------------
            If i = 2 Then
                If POrient = "External" Then
                    EnvpTopL.Y = pPt(kEnd + 1).Y

                ElseIf POrient = "Internal" Then
                    EnvpBotR.Y = pPt(kEnd + 1).Y
                End If
            End If

        Next

    End Sub


#End Region


#Region "............DRAW MID HALF CONVOLUTIONS: LOWER LEVEL."

    '--------------------------------------------------------------------------------
    '                             TEMPLATE = "1", "2"                               '
    '--------------------------------------------------------------------------------
    '
    Private Sub Draw_MidConv(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                    ByVal intDrawWid_In As Integer, ByVal intDashStyle_In As Integer, _
                    ByVal YOffset_In As Single, _
                    ByVal thetaM1Draw As Double, ByVal thetaM3Draw As Double)
        '================================================================================
        'This subroutine draws a Mid Convolution with its RHS starting location
        '....at 'YOffset_In' in WCS.  TODO. Review. PB 16NOV07.

        'Draw the Mid Convolutions
        '=========================
        'Dim xCenVB As Single, yCenVB As Single
        Dim pAngStart As Single, pAngSweep As Single

        'Dim xBFlatK As Single, yBFlatK As Single                               'SG 01JUL11
        'Dim xEFlatK As Single, yEFlatK As Single
        Dim pBFlatK As PointF, pEFlatK As PointF

        'Dim yBFlatLHS As Single, yEFlatLHS As Single
        Dim pBFlatLHS As PointF, pEFlatLHS As PointF

        Dim radM1k As Single, radM3k As Single

        'Dim sign As Integer

        '   Draw the Thickness Lines.
        '   -------------------------
        '   ....k = -1 : Inside Surface
        '   ....k =  0 : Mid    Line
        '   ....k =  1 : Outside Surface

        Dim kBeg As Integer, kEnd As Integer, kStep As Integer
        kBeg = -1
        kEnd = 1
        kStep = 2

        Dim k As Integer
        For k = kBeg To kEnd Step kStep

            Dim pCen As PointF

            'RHS 1/2 Convolution: Segment 1. 
            '------------------------------
            pCen.X = mCenM(1).X
            pCen.Y = mCenM(1).Y + YOffset_In

            '....Center coordinates in PCS:
            Dim pCen_PCS As PointF
            pCen_PCS = Pt_PCS(pCen)


            If POrient = "External" Then
                pAngStart = 90
                pAngSweep = thetaM1Draw

            ElseIf POrient = "Internal" Then
                pAngStart = -90
                pAngSweep = -thetaM1Draw
            End If

            radM1k = mUnit.L_ConToUser(mRadM(1) + k * T * 0.5)
            Dim pRad_PCS As Single
            pRad_PCS = radM1k * mScalePCS

            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                pCen_PCS, pRad_PCS, pAngStart, pAngSweep)


            'RHS 1/2 Convolution - Segment 2 (Flat). 
            '--------------------------------------
            Dim pPtFlat_VB(1) As PointF  'TODO: Logic error should be thetaM1Draw. PB 07NOV07.
            Dim pPt_Temp As PointF

            If POrient = "External" Then
                'xBFlatK = mM12.X + (-k) * T * 0.5 * CosD(mThetaM(1))
                'xBFlatK = mM12.X + (-k) * T * 0.5 * CosD(thetaM1Draw)
                pBFlatK.X = mM12.X + (-k) * T * 0.5 * CosD(thetaM1Draw)

            ElseIf POrient = "Internal" Then
                'xBFlatK = mM12.X + k * T * 0.5 * CosD(thetaM1Draw)
                pBFlatK.X = mM12.X + k * T * 0.5 * CosD(thetaM1Draw)
            End If

            'yBFlatK = mM12.Y + k * T * 0.5 * SinD(thetaM1Draw)
            pBFlatK.Y = (mM12.Y + k * T * 0.5 * SinD(thetaM1Draw))

            With pPt_Temp
                .X = pBFlatK.X
                .Y = pBFlatK.Y + YOffset_In
            End With
            'With pPtFlat_VB(0)
            '    .X = CalcXVB(pBFlatK.X, pBFlatK.Y + YOffset_In)
            '    .Y = CalcYVB(pBFlatK.X, pBFlatK.Y + YOffset_In)
            'End With
            pPtFlat_VB(0) = Pt_PCS(pPt_Temp)

            If POrient = "External" Then
                'xEFlatK = mM23.X + (-k) * T * 0.5 * CosD(thetaM1Draw)
                pEFlatK.X = mM23.X + (-k) * T * 0.5 * CosD(thetaM1Draw)
            ElseIf POrient = "Internal" Then
                'xEFlatK = mM23.X + k * T * 0.5 * CosD(thetaM1Draw)
                pEFlatK.X = mM23.X + k * T * 0.5 * CosD(thetaM1Draw)
            End If

            'yEFlatK = mM23.Y + k * T * 0.5 * SinD(thetaM1Draw)
            pEFlatK.Y = (mM23.Y + k * T * 0.5 * SinD(thetaM1Draw))

            With pPt_Temp
                .X = pEFlatK.X
                .Y = pEFlatK.Y + YOffset_In
            End With

            'With pPtFlat_VB(1)
            '    .X = CalcXVB(pEFlatK.X, pEFlatK.Y + YOffset_In)
            '    .Y = CalcYVB(pEFlatK.X, pEFlatK.Y + YOffset_In)
            'End With
            pPtFlat_VB(1) = Pt_PCS(pPt_Temp)


            Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                                         pPtFlat_VB(0), pPtFlat_VB(1))


            'Both RHS & LHS 1/2 Convolutions - Segment 3. 
            '--------------------------------------------
            With pCen
                .X = mCenM(3).X
                .Y = mCenM(3).Y + YOffset_In
            End With

            '....Center coordinates in PCS:
            pCen_PCS = Pt_PCS(pCen)

            If POrient = "External" Then
                pAngStart = -(90 - thetaM3Draw)
                pAngSweep = -2 * thetaM3Draw

            ElseIf POrient = "Internal" Then
                pAngStart = (90 - thetaM3Draw)
                pAngSweep = 2 * thetaM3Draw
            End If

            radM3k = mUnit.L_ConToUser(mRadM(3) - k * T * 0.5)
            pRad_PCS = radM3k * mScalePCS

            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                pCen_PCS, pRad_PCS, pAngStart, pAngSweep)


            'LHS 1/2 Convolution - Segment 1.
            '-------------------------------
            With pCen
                .X = mCenM(1).X
                .Y = mCenM(1).Y + mPitchM + YOffset_In
            End With

            '....Center coordinates in PCS:
            pCen_PCS = Pt_PCS(pCen)

            If POrient = "External" Then
                pAngStart = 90
                pAngSweep = -thetaM1Draw

            ElseIf POrient = "Internal" Then
                pAngStart = -90
                pAngSweep = thetaM1Draw
            End If

            radM1k = mUnit.L_ConToUser(mRadM(1) + k * T * 0.5)
            pRad_PCS = radM1k * mScalePCS

            Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                pCen_PCS, pRad_PCS, pAngStart, pAngSweep)


            ' LHS 1/2 Convolution - Segment 2 (Flat)
            '---------------------------------------
            'yBFlatLHS = pCen.Y - (pBFlatK.Y - mCenM(1).Y)
            'yEFlatLHS = mCenM(3).Y + YOffset_In + (mCenM(3).Y - pEFlatK.Y)

            'With pPtFlat_VB(0)
            '    .X = CalcXVB(pBFlatK.X, yBFlatLHS)
            '    .Y = CalcYVB(pBFlatK.X, yBFlatLHS)
            'End With
            'With pPtFlat_VB(1)
            '    .X = CalcXVB(pEFlatK.X, yEFlatLHS)
            '    .Y = CalcYVB(pEFlatK.X, yEFlatLHS)
            'End With

            pBFlatLHS.Y = pCen.Y - (pBFlatK.Y - mCenM(1).Y)
            pBFlatLHS.X = pBFlatK.X
            pPtFlat_VB(0) = Pt_PCS(pBFlatLHS)

            pEFlatLHS.Y = mCenM(3).Y + YOffset_In + (mCenM(3).Y - pEFlatK.Y)
            pEFlatLHS.X = pEFlatK.X
            pPtFlat_VB(1) = Pt_PCS(pEFlatLHS)

            Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                                                     pPtFlat_VB(0), pPtFlat_VB(1))
        Next k

    End Sub


    '--------------------------------------------------------------------------------
    '                           TEMPLATE = "1Gen" & "1GenS"                         '
    '--------------------------------------------------------------------------------

    Private Sub Draw_Mid_End_HalfConvs(ByVal grphObj_In As Graphics, _
                                       ByVal color_In As Color, _
                                       ByVal intDrawWid_In As Integer, _
                                       ByVal intDashStyle_In As Integer, _
                                       ByVal thetaM1Draw_In As Double, _
                                       ByVal thetaM3Draw_In As Double)
        '=============================================================================
        '....This subroutine draws the Mid-End Half Convolutions at RHS & LHS.

        Dim pRad(3) As Single
        Dim pCen(3) As PointF
        Dim pAngStart(3) As Single, pAngSweep(3) As Single


        '   Calculate the Angles and other graphical parameters & 
        '   ....draw RHS & LHS 1/2 convolutions.
        '   ====================================================
        '
        '       Segment Radii:
        '       --------------
        pRad(1) = mRadM(1)
        pRad(2) = mRadM(2)
        pRad(3) = mRadM(3)

        '
        '       RHS 1/2 Convolution: (i = 1)
        '       ----------------------------
        '
        '       ....Segment 1.  (j = 1)
        '
        pCen(1) = mCenM(1)

        If POrient = "External" Then
            pAngStart(1) = 90
            pAngSweep(1) = thetaM1Draw_In

        ElseIf POrient = "Internal" Then
            pAngStart(1) = -90
            pAngSweep(1) = -thetaM1Draw_In
        End If


        '       ....Segment 2.  (j = 2)
        '
        '....For Template No = "1GenS".
        Dim pM2Beg As PointF
        Dim pM2End As PointF
        Dim pBeta_M2Beg_Thick As Single         '....Convention - CW: +ive.


        If mTemplateNo = "1Gen" Then
            '-----------------------
            pCen(2) = mCenM(2)

            If POrient = "External" Then
                pAngStart(2) = thetaM1Draw_In - 90
                pAngSweep(2) = -mThetaM(2)

            ElseIf POrient = "Internal" Then
                'AM/SB
                pAngStart(2) = 90 - thetaM1Draw_In
                pAngSweep(2) = mThetaM(2)
            End If


        ElseIf mTemplateNo = "1GenS" Then
            '----------------------------
            pCen(2) = Nothing
            pAngStart(2) = Nothing
            pAngSweep(2) = Nothing

            '....Segment 2 (Staright) begin & end points. 
            pM2Beg = mM12
            pM2End = mM23


            '....Angle made by the thickness line thru' pM2Beg with the horizontal thru'
            '........the corresponding center point.
            If POrient = "External" Then
                pBeta_M2Beg_Thick = thetaM1Draw_In - 90

            ElseIf POrient = "Internal" Then
                pBeta_M2Beg_Thick = 90 - thetaM1Draw_In
            End If

        End If


        '       ....Segment 3.  (j = 3)
        '
        pCen(3) = mCenM(3)

        If POrient = "External" Then
            pAngStart(3) = -(90 - thetaM3Draw_In)
            pAngSweep(3) = -thetaM3Draw_In

        ElseIf POrient = "Internal" Then
            'AM/SB
            pAngStart(3) = (90 - thetaM3Draw_In)
            pAngSweep(3) = thetaM3Draw_In
        End If


        '   Draw RHS 1/2 Mid-End Convolution:
        '   ---------------------------------
        '

        If mTemplateNo = "1Gen" Then
            '-----------------------
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          pCen, pRad, pAngStart, pAngSweep)


        ElseIf mTemplateNo = "1GenS" Then
            '----------------------------
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          "RHS", pCen, pRad, pAngStart, pAngSweep, _
                          pM2Beg, pBeta_M2Beg_Thick, pM2End)
        End If


        '       LHS 1/2 Convolution: (i = 2)
        '       ----------------------------
        '
        '....Axial offset between the Mid-End RHS 1/2 & LHS 1/2 Convolutions.
        Dim pYOffset As Single
        pYOffset = 2 * mPitch_Half_M_End

        If mNConv >= 2 Then
            pYOffset = pYOffset + (mNConv - 1) * mPitchM_Mid
        End If

        '
        '       ....Segment 1.  (j = 1)
        '
        pCen(1).X = mCenM(1).X
        pCen(1).Y = mCenM(1).Y + pYOffset

        If POrient = "External" Then
            pAngStart(1) = 90
            pAngSweep(1) = -thetaM1Draw_In

        ElseIf POrient = "Internal" Then
            pAngStart(1) = -90
            pAngSweep(1) = thetaM1Draw_In
        End If

        '
        '       ....Segment 2.  (j = 2)
        '

        If mTemplateNo = "1Gen" Then
            '-----------------------
            pCen(2).X = mCenM(2).X
            pCen(2).Y = -mCenM(2).Y + pYOffset

            If POrient = "External" Then
                pAngStart(2) = (180 - thetaM1Draw_In) + 90
                pAngSweep(2) = mThetaM(2)

            ElseIf POrient = "Internal" Then
                'AM/SB
                pAngStart(2) = thetaM1Draw_In - 270
                pAngSweep(2) = -mThetaM(2)
            End If


        ElseIf mTemplateNo = "1GenS" Then
            '----------------------------
            pCen(2) = Nothing
            pAngStart(2) = Nothing
            pAngSweep(2) = Nothing

            '....Segment 2 (Staright) begin & end points. 
            With pM2Beg
                .X = mM12.X
                .Y = -mM12.Y + pYOffset
            End With

            With pM2End
                .X = mM23.X
                .Y = -mM23.Y + pYOffset
            End With

            If POrient = "External" Then
                pBeta_M2Beg_Thick = 90 - thetaM1Draw_In

            ElseIf POrient = "Internal" Then
                pBeta_M2Beg_Thick = thetaM1Draw_In - 90
            End If

        End If

        '
        '       ....Segment 3.  (j = 3)
        '
        pCen(3).X = mCenM(3).X
        pCen(3).Y = -mCenM(3).Y + pYOffset

        If POrient = "External" Then
            pAngStart(3) = 180 + (90 - thetaM3Draw_In)
            pAngSweep(3) = thetaM3Draw_In

        ElseIf POrient = "Internal" Then
            pAngStart(3) = (-270 + thetaM3Draw_In)
            pAngSweep(3) = -thetaM3Draw_In
        End If


        '   Draw LHS 1/2 Mid-End Convolution:
        '   ---------------------------------
        If mTemplateNo = "1Gen" Then
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          pCen, pRad, pAngStart, pAngSweep)

        ElseIf mTemplateNo = "1GenS" Then
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          "LHS", pCen, pRad, pAngStart, pAngSweep, _
                          pM2Beg, pBeta_M2Beg_Thick, pM2End)
        End If

    End Sub


    Private Sub Draw_Mid_Mid_Conv(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                                  ByVal intDrawWid_In As Integer, _
                                  ByVal intDashStyle_In As Integer, _
                                  ByVal YOffset_In As Single, ByVal thetaM3Draw_In As Double, _
                                  ByVal thetaM5Draw_In As Double)
        '==============================================================================================

        '....This subroutine draws a full (RHS & LHS) Mid-Mid convolution with its 
        '........RHS end located at 'YOffset_In' in WCS.

        Dim pRad(3) As Single
        Dim pCen(3) As PointF
        Dim pAngStart(3) As Single, pAngSweep(3) As Single

        '   Calculate the Angles & other graphical parameters & then 
        '   ....draw the RHS & LHS Mid-Mid 1/2 convolutions pair.
        '   ========================================================
        '
        '       Segment Radii:
        '       --------------
        pRad(1) = mRadM(5)
        pRad(2) = mRadM(4)
        pRad(3) = mRadM(3)

        '       RHS 1/2 Convolution: (i = 1)
        '       ----------------------------
        '
        '       ....Segment 1.  (j = 1)
        '
        With pCen(1)
            .X = mCenM(5).X
            .Y = mCenM(5).Y + YOffset_In
        End With

        If POrient = "External" Then
            pAngStart(1) = -(thetaM5Draw_In - 90)
            pAngSweep(1) = thetaM5Draw_In

        ElseIf POrient = "Internal" Then
            pAngStart(1) = -90 + thetaM5Draw_In
            pAngSweep(1) = -thetaM5Draw_In
        End If

        '
        '       ....Segment 2.  (j = 2)
        '
        Dim pM2Beg As PointF
        Dim pM2End As PointF
        Dim pBeta_M2Beg_Thick As Single         '....Convention - CW: +ive.

        If mTemplateNo = "1Gen" Then
            '-----------------------

            With pCen(2)
                .X = mCenM(4).X
                .Y = mCenM(4).Y + YOffset_In
            End With

            If POrient = "External" Then
                pAngStart(2) = 90 + (180 - thetaM5Draw_In)
                pAngSweep(2) = mThetaM(4)

            ElseIf POrient = "Internal" Then
                pAngStart(2) = 90 + thetaM3Draw_In     'PB 15NOV07. Check
                pAngSweep(2) = mThetaM(4)
            End If


        ElseIf mTemplateNo = "1GenS" Then
            '----------------------------

            '....Segment 2 (Staright) begin & end points. 
            pCen(2) = Nothing
            pAngStart(2) = Nothing
            pAngSweep(2) = Nothing
            pRad(2) = 0

            With pM2Beg
                .X = mM45.X
                .Y = mM45.Y + YOffset_In
            End With

            With pM2End
                .X = mM34.X
                .Y = mM34.Y + YOffset_In
            End With

            If POrient = "External" Then
                pBeta_M2Beg_Thick = 90 - thetaM5Draw_In

            ElseIf POrient = "Internal" Then
                pBeta_M2Beg_Thick = thetaM5Draw_In - 90
            End If

        End If

        '
        '       ....Segment 3.  (j = 3)
        '
        With pCen(3)
            .X = mCenM(3).X
            .Y = mCenM(3).Y + YOffset_In
        End With

        If POrient = "External" Then
            pAngStart(3) = 180 + (90 - thetaM3Draw_In)
            pAngSweep(3) = thetaM3Draw_In

        ElseIf POrient = "Internal" Then
            'AM/SB
            pAngStart(3) = 90
            pAngSweep(3) = thetaM3Draw_In
        End If


        '   Draw RHS 1/2 Mid-Mid Convolution:
        '   ---------------------------------

        If mTemplateNo = "1Gen" Then
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          pCen, pRad, pAngStart, pAngSweep)

        ElseIf mTemplateNo = "1GenS" Then
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          "RHS", pCen, pRad, pAngStart, pAngSweep, _
                          pM2Beg, pBeta_M2Beg_Thick, pM2End)
        End If


        '       LHS 1/2 Convolution: (i = 2)
        '       ----------------------------
        '
        '       ....Segment 1.  (j = 1)
        '
        With pCen(1)
            .X = mCenM(5).X
            .Y = mCenM(5).Y + YOffset_In
        End With

        If POrient = "External" Then
            pAngStart(1) = 90
            pAngSweep(1) = thetaM5Draw_In

        ElseIf POrient = "Internal" Then
            pAngStart(1) = -90
            pAngSweep(1) = -thetaM5Draw_In
        End If

        '
        '       ....Segment 2.  (j = 2)
        '
        '....Translation of coordinate system.  X-Y (WCS) ==> XSym1-YSym1. 
        '........The symmetry axis XSym1 is passing thru' the mid-section of the 
        '........first Mid-Mid convolution.  
        '
        Dim pYTrans As Single      '....Symmetry Line
        pYTrans = mPitch_Half_M_End + 0.5 * mPitchM_Mid

        If mTemplateNo = "1Gen" Then
            '-----------------------
            With pCen(2)
                .X = mCenM(4).X
                .Y = -mCenM(4).Y + 2 * pYTrans + YOffset_In
            End With

            If POrient = "External" Then
                pAngStart(2) = thetaM5Draw_In - 90
                pAngSweep(2) = -mThetaM(4)

            ElseIf POrient = "Internal" Then
                pAngStart(2) = 90 - thetaM3Draw_In
                pAngSweep(2) = -mThetaM(4)
            End If


        ElseIf mTemplateNo = "1GenS" Then
            '----------------------------
            pCen(2) = Nothing
            pAngStart(2) = Nothing
            pAngSweep(2) = Nothing

            With pM2End
                .X = mM34.X
                .Y = -mM34.Y + 2 * pYTrans + YOffset_In
            End With

            With pM2Beg
                .X = mM45.X
                .Y = -mM45.Y + 2 * pYTrans + YOffset_In
            End With

            If POrient = "External" Then
                pBeta_M2Beg_Thick = thetaM5Draw_In - 90

            ElseIf POrient = "Internal" Then
                pBeta_M2Beg_Thick = 90 - thetaM5Draw_In
            End If

        End If

        '
        '       ....Segment 3.  (j = 3)
        '
        With pCen(3)
            .X = mCenM(3).X
            .Y = -mCenM(3).Y + 2 * pYTrans + YOffset_In
            '.Y = mCenM(3).Y + YOffset_In + mPitchM_Mid     '....Alternate method.
        End With

        If POrient = "External" Then
            pAngStart(3) = -(90 - thetaM3Draw_In)
            pAngSweep(3) = -thetaM3Draw_In

        ElseIf POrient = "Internal" Then
            pAngStart(3) = 90
            pAngSweep(3) = -thetaM3Draw_In
        End If


        '   Draw LHS 1/2 Mid-Mid Convolution:
        '   ---------------------------------
        '
        If mTemplateNo = "1Gen" Then
            '-----------------------
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          pCen, pRad, pAngStart, pAngSweep)

        ElseIf mTemplateNo = "1GenS" Then
            '----------------------------
            Draw_HalfConv(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                          "LHS", pCen, pRad, pAngStart, pAngSweep, _
                          pM2Beg, pBeta_M2Beg_Thick, pM2End)
        End If

    End Sub

#End Region


#Region "................UTILITY ROUTINES: LOWEST LEVEL."

    '-------------------------------------------------------------------------------
    '                     GRAPHICS UTILITY ROUTINES - BEGIN                        '
    '-------------------------------------------------------------------------------

    '....OVERLOADED Version 1: TemplateNo = "1Gen".
    '
    Private Sub Draw_HalfConv(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                    ByVal intDrawWid_In As Integer, ByVal intDashStyle_In As Integer, _
                    ByVal Cen_In() As PointF, ByVal Rad_In() As Single, _
                    ByVal AngStart_In() As Single, ByVal AngSweep_In() As Single)
        '================================================================================
        '   TemplateNo = "1Gen":  

        Dim pCen_PCS As PointF
        Dim pRad_K_WCS As Single
        Dim pRad_K_PCS As Single

        Dim j As Int16              '........Segment #
        Dim k As Integer            '............Surface Line #

        '   Thickness Lines.
        '   ----------------
        '   ....k = -1 : Inside Surface
        '   ....k =  0 : Mid    Line        (not drawn).
        '   ....k =  1 : Outside Surface

        Dim kBeg As Integer, kEnd As Integer, kStep As Integer
        kBeg = -1
        kEnd = 1
        kStep = 2

        For j = 1 To 3                          '........Over Segments 1 To 3

            '....Convert the Segment 'j': Center Point coordinates from WCS ==> PCS.
            pCen_PCS = Pt_PCS(Cen_In(j))

            For k = kBeg To kEnd Step kStep     '............Over Surface Lines

                pRad_K_WCS = mUnit.L_ConToUser(Rad_In(j) + k * T * 0.5)
                pRad_K_PCS = pRad_K_WCS * mScalePCS

                Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                    pCen_PCS, pRad_K_PCS, AngStart_In(j), AngSweep_In(j))

            Next k

        Next j

    End Sub


    '....OVERLOADED Version 2: TemplateNo = "1GenS".
    '
    Private Sub Draw_HalfConv(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                        ByVal intDrawWid_In As Integer, ByVal intDashStyle_In As Integer, _
                        ByVal Side_In As String, _
                        ByVal Cen_In() As PointF, ByVal Rad_In() As Single, _
                        ByVal AngStart_In() As Single, ByVal AngSweep_In() As Single, _
                        ByVal M2Beg_In As PointF, ByVal Beta_M2Beg_Thick_In As Single, _
                        ByVal M2End_In As PointF)
        '================================================================================
        '   TemplateNo = "1GenS":   The index 2 of the argument arrays is not used.
        '   ....Instead, M2Beg_In & M2End_In are used to define Segment 2.


        Dim pCen_PCS As PointF
        Dim pRad_K_WCS As Single
        Dim pRad_K_PCS As Single

        Dim j As Int16              '........Segment #
        Dim k As Integer            '............Surface # (Inner, Mid or Outer)

        '   Thickness Lines.
        '   ----------------
        '   ....k = -1 : Inside Surface
        '   ....k =  0 : Mid    Line        (not drawn).
        '   ....k =  1 : Outside Surface

        Dim kBeg As Integer, kEnd As Integer, kStep As Integer
        kBeg = -1
        kEnd = 1
        kStep = 2

        For j = 1 To 3                          '........Over Segments 1 To 3

            For k = kBeg To kEnd Step kStep     '............Over Surface Lines

                If j = 1 Or j = 3 Then

                    '   Arc Segment.
                    '   ------------

                    '....Convert the Segment 'j': Center Point coordinates from WCS ==> PCS.
                    pCen_PCS = Pt_PCS(Cen_In(j))

                    pRad_K_WCS = mUnit.L_ConToUser(Rad_In(j) + k * T * 0.5)
                    pRad_K_PCS = pRad_K_WCS * mScalePCS

                    Draw_Arc(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                        pCen_PCS, pRad_K_PCS, AngStart_In(j), AngSweep_In(j))


                ElseIf j = 2 Then

                    '   Straight Segment.
                    '   -----------------
                    '
                    '   ....First determine the angle made by the thickness line thru' 
                    '   ........the M2Beg_In Point w.r.t the horizontal thru' the 
                    '   ........corresponding center point. 
                    '   ............Sign Convention: CW ive. 
                    '
                    Dim pBeta As Single
                    pBeta = Beta_M2Beg_Thick_In

                    Dim pFactor As Single
                    If Side_In = "RHS" Then       '....RHS Half is being drawn.
                        pFactor = -1

                    ElseIf Side_In = "LHS" Then    '....LHS Half is being drawn.
                        pFactor = 1
                    End If

                    Dim pDX As Single, pDY As Single
                    pDX = pFactor * k * T * 0.5 * SinD(pBeta)
                    pDY = pFactor * k * T * 0.5 * CosD(pBeta)


                    Dim pM2Beg_k As PointF, pM2Beg_k_PCS As PointF
                    With pM2Beg_k
                        .X = M2Beg_In.X + pDX
                        .Y = M2Beg_In.Y + pDY
                    End With

                    pM2Beg_k_PCS = Pt_PCS(pM2Beg_k)


                    Dim pM2End_k As PointF, pM2End_k_PCS As PointF
                    With pM2End_k
                        .X = M2End_In.X + pDX
                        .Y = M2End_In.Y + pDY
                    End With

                    pM2End_k_PCS = Pt_PCS(pM2End_k)

                    Draw_Line(grphObj_In, color_In, intDrawWid_In, intDashStyle_In, _
                                                 pM2Beg_k_PCS, pM2End_k_PCS)

                End If

            Next k

        Next j

    End Sub
    'Move to clsSeal SG 18APR11

    ''....Overloaded Version 1:
    'Private Sub Arc(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
    '                ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
    '                ByVal xCenPCS_In As Single, ByVal yCenPCS_In As Single, _
    '                ByVal rad_In As Single, _
    '                ByVal angStart_In As Single, ByVal angSweep_In As Single)
    '    '===========================================================================
    '    '
    '    '....TEMPLATE INDEPENDENT.
    '    '
    '    'This subroutine draws a circular arc. 

    '    'Enclosing Rectangle:
    '    '--------------------
    '    '....LHS Top.
    '    Dim pPtLoc As New PointF(xCenPCS_In - rad_In, yCenPCS_In - rad_In) '....Location
    '    Dim pSizeF As New SizeF(rad_In * 2, rad_In * 2)                    '....Size
    '    Dim pRect As RectangleF = New RectangleF(pPtLoc, pSizeF)           '....Rectangle      


    '    '....Pixel densities per unit "PageUnit" dimension (in or mm)
    '    Dim pDpX As Single
    '    pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

    '    Dim pPen As New Pen(color_In, drawWid_In / pDpX)
    '    pPen.DashStyle = intDashStyle_In
    '    grphObj_In.DrawArc(pPen, pRect, angStart_In, angSweep_In)
    '    pPen = Nothing

    'End Sub


    ''....Overloaded Version 2:
    'Private Sub Arc(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
    '                    ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
    '                    ByVal CenPCS_In As PointF, ByVal rad_In As Single, _
    '                    ByVal angStart_In As Single, ByVal angSweep_In As Single)
    '    '===========================================================================
    '    '
    '    '....TEMPLATE INDEPENDENT.
    '    '
    '    'This subroutine draws a circular arc. 

    '    'Enclosing Rectangle:
    '    '--------------------
    '    '....LHS Top.
    '    Dim pPtLoc As New PointF(CenPCS_In.X - rad_In, CenPCS_In.Y - rad_In) '....Location
    '    Dim pSizeF As New SizeF(rad_In * 2, rad_In * 2)                      '....Size
    '    Dim pRect As RectangleF = New RectangleF(pPtLoc, pSizeF)        '....Rectangle      


    '    '....Pixel densities per unit "PageUnit" dimension (in or mm)
    '    Dim pDpX As Single
    '    pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

    '    Dim pPen As New Pen(color_In, drawWid_In / pDpX)
    '    pPen.DashStyle = intDashStyle_In
    '    grphObj_In.DrawArc(pPen, pRect, angStart_In, angSweep_In)
    '    pPen = Nothing

    'End Sub


    'Private Sub Line(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
    '                 ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
    '                 ByVal ptBeg_In As PointF, ByVal ptEnd_In As PointF)
    '    '======================================================================
    '    '
    '    '....TEMPLATE INDEPENDENT.
    '    '
    '    'This subroutine draws a line. 

    '    '....Pixel densities per unit "PageUnit" dimension (in or mm)
    '    Dim pDpX As Single
    '    pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

    '    Dim pPen As New Pen(color_In, drawWid_In / pDpX)
    '    pPen.DashStyle = intDashStyle_In
    '    grphObj_In.DrawLine(pPen, ptBeg_In, ptEnd_In)
    '    pPen = Nothing

    'End Sub

    'PB 23APR11. Moved to clsSeal.

    'Private Function CalcXVB(ByVal xWCS_Point As Single, _
    '                         ByVal yWCS_Point As Single) As Single
    '    '==============================================================================
    '    ' This function calculates the X Coordinate in the PCS when the WCS coordinates 
    '    ' ....of a point are given. Dependent on "STD" or "ADJ" geometry.
    '    CalcXVB = mXVB_OrigWCS - mUnit.L_ConToUser(yWCS_Point) * mScalePCS
    'End Function


    'Private Function CalcYVB(ByVal xWCS_Point As Single, _
    '                         ByVal yWCS_Point As Single) As Single
    '    '================================================================================
    '    ' This function calculates the X Coordinate in the PCS when the WCS coordinates 
    '    ' ....of a point are given. Dependent on "STD" or "ADJ" geometry.
    '    CalcYVB = mYVB_OrigWCS - mUnit.L_ConToUser(xWCS_Point) * mScalePCS
    'End Function


    'PB 22APR11. Moved to clsSeal.
    '....More compact routine. Will be moved to clsSeal 18APR11
    'Private Function Get_Pt_PCS(ByVal Pt_WCS As PointF) As PointF
    '    '=======================================================
    '    ' This function calculates the coordinates in the PCS when the WCS coordinates 
    '    ' ....of a point are given. Dependent on "STD" or "ADJ" geometry.

    '    With Get_Pt_PCS
    '        .X = mXVB_OrigWCS - mUnit.L_ConToUser(Pt_WCS.Y) * mScalePCS
    '        .Y = mYVB_OrigWCS - mUnit.L_ConToUser(Pt_WCS.X) * mScalePCS
    '    End With

    '    Return Get_Pt_PCS

    'End Function

    '-------------------------------------------------------------------------------
    '                        GENERAL UTILITY ROUTINES - END                        '
    '-------------------------------------------------------------------------------

#End Region

    '-------------------------------------------------------------------------------
    '                           GRAPHICS METHODS - END                             '
    '-------------------------------------------------------------------------------

#End Region

    '*******************************************************************************
    '*                        CLASS METHODS - END                                  *
    '*******************************************************************************


End Class
