'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  frmNonStdCS_CSeal                      '
'                        VERSION NO  :  1.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  31JUL17                                '
'                                                                              '
'===============================================================================
Imports System.Math
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports clsLibrary11


Public Class Part_frmNonStdCS_CSeal

    'Private mHW As New clsPartProject.clsPNR.clsHW(gPartProject.PNR)
    Private mPartProject As New clsPartProject
    Private mCSeal As IPE_clsCSeal        '....Local object. 

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single


    '....Flags to indicate if the UpDown buttons have been entered by the user. 
    '........PB 22MAR07. Corrects the Error 8, DR V50.
    Private mblnUpdDHfree_Entered As Boolean
    Private mblnUpdDThetaOpening_Entered As Boolean
    Private mblnUpd_DIndexTArray_Entered As Boolean

    Public Sub New(ByRef PartProject_In As clsPartProject)
        '================================
        ' This call is required by the designer.
        InitializeComponent()

        'mHW = HW_In.Clone()
        mPartProject = PartProject_In
    End Sub

    Private Sub frmNonStdCS_CSeal_Load(sender As System.Object,
                                       e As System.EventArgs) Handles MyBase.Load
        '=========================================================================

        'MsgBox("frmAdjustGeometryCSeal_Load")

        'Initialize Flags:
        '-----------------
        '....Flags to indicate that the user has actually entered on each updown button to
        '........distinguish from the event when their value is changed internally by the 
        '........program while setting its properties e.g. max, min or increment.
        mblnUpdDHfree_Entered = False
        mblnUpdDThetaOpening_Entered = False
        mblnUpd_DIndexTArray_Entered = False

        '   --------------
        '   Picture Box   '               
        '   --------------
        '
        '....Set Margin of the Picture Box.
        '   
        Dim pUniformMargin As Single = 0.4       '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        'Const mcUniformMargin As Single = 0.4       '....Uniform margin around the
        ''                                           '........Picture Box - (in)

        'Dim psngMargin As Single
        'psngMargin = gUnit.EngLToUserL(mcUniformMargin) '....In user unit (in or mm)

        ''....Margins around the graphics in the picture box.
        'mMargin(1) = psngMargin                     '....Left
        'mMargin(2) = psngMargin                     '....Right

        ''....The margin at the top is 60% of the total height margin and
        ''....at the bottom is the rest 40%.
        'mMargin(3) = 2 * psngMargin * 0.6           '....Top
        'mMargin(4) = 2 * psngMargin * 0.4           '....Bottom

        '---------------------------------------------------------------------------

        '....Initialize the Local Seal Object.
        InitializeLocalObject()             '....gSeal ===> mCSeal.


        '....Set the "Maximum", "Minimum" & "Increment" properties of the UpDown buttons.
        '
        SetUpDown_DHFree()                  '....DHfree
        SetUpDown_DThetaOpening()           '....DThetaOpening
        SetUpDown_DIndexTArray()            '....DIndexTArray

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C) Then
            Me.Text = "Non Standard Cross Sec. - C-Seal"

        ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            Me.Text = "Non Standard Cross Sec. - SC-Seal"
        End If

        '....Display data & graphics on the form.
        '........Local seal object "mCSeal" (which is recently initialized above) is used.
        DisplayData()

        '....Display graphics on the picture box.
        DoGraphics()

        'MsgBox("Hfree = " & mCSeal.Hfree)
    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '==============================
        '....From gSeal ===> mCSeal. 
        '........Now onwards, mCSeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal in the 
        '........"SaveData" routine which is called when the form is exited and another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local Seal Object.
        mCSeal = New IPE_clsCSeal("C-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
        mCSeal.MCrossSecNo = mPartProject.PNR.HW.MCrossSecNo
    End Sub

    Private Sub InitializeControls()
        '============================
        ''If (gfrmAnalysisSet.ModeOper = frmAnalysisSet.eModeOper.None) Then
        ''    txtHfree.Enabled = False
        ''    updDHfree.Enabled = False

        ''    txtThetaOpening.Enabled = False
        ''    updDThetaOpening.Enabled = False

        ''    txtT.Enabled = False
        ''    updDIndexTArray.Enabled = False
        ''Else
        ''    txtHfree.Enabled = True
        ''    updDHfree.Enabled = True

        ''    txtThetaOpening.Enabled = True
        ''    updDThetaOpening.Enabled = True

        ''    txtT.Enabled = True
        ''    updDIndexTArray.Enabled = True
        ''End If

    End Sub

    Private Sub SetUpDown_DHFree()
        '=========================               
        '....This routine the sets the "Minimum", "Maximum" & "Increment" properties 
        '........of the UpDown button for the "DHfree".

        'Unit Dependent:

        'Calculate an initial value of the "Increment".
        '---------------------------------------------
        '
        '....A conveniently selected # of Steps as an initial estimate.
        Const pcNStep As Int16 = 20
        Dim pIncrement_Calc As Single
        'PB 25OCT09. The following estimation may be a bit incorrect although it won't do any harm later.
        'I think, it should be (mCSeal.DHfreeAdjMax - mCSeal.DHfreeAdjMin). Check later.
        pIncrement_Calc = (mCSeal.DHfreeAdjMax + mCSeal.DHfreeAdjMin) / pcNStep

        Dim pIncrement_Calc_UserL As Single
        pIncrement_Calc_UserL = gUnit.L_ConToUser(pIncrement_Calc)


        'Determine a convinient increment value close to the above initial value.
        '------------------------------------------------------------------------
        '....A set of convenient increment values.
        Dim pArrayIncr_Eng() As Single = {0.001, 0.005, 0.01}
        Dim pArrayIncr_Met_mm() As Single = {0.02, 0.1, 0.25}     '....In User Unit.

        Dim pUBArray As Int16 = UBound(pArrayIncr_Eng)
        Dim pArrayIncr(pUBArray) As Single

        If mCSeal.UnitSystem = "English" Then
            pArrayIncr = pArrayIncr_Eng

        ElseIf mCSeal.UnitSystem = "Metric" Then
            pArrayIncr = pArrayIncr_Met_mm
        End If


        Dim pDiff As Single
        Dim pDiffMin As Single = 99.99   '....Initialized to an aribitrarily large value.

        Dim i As Int16, iMin As Int16
        Dim pIncrement_UpDown As Single, pMax_UpDown As Single, pMin_UpDown As Single

        For i = 0 To pUBArray
            pDiff = Abs(pArrayIncr(i) - pIncrement_Calc_UserL)

            If pDiff <= pDiffMin Then
                pDiffMin = pDiff
                iMin = i
            End If
        Next

        pIncrement_UpDown = pArrayIncr(iMin)


        'Determine the Max. and Min. values of the UpDown button.
        '--------------------------------------------------------
        '
        '........Maximum Value:
        '
        Dim pDHfreeAdjMax_UserL As Single
        pDHfreeAdjMax_UserL = gUnit.L_ConToUser(mCSeal.DHfreeAdjMax)

        i = 0       '....Initialize
        Do While (i * pIncrement_UpDown < pDHfreeAdjMax_UserL)
            i = i + 1
        Loop

        pMax_UpDown = i * pIncrement_UpDown


        '........Minimum Value:
        '
        Dim pDHfreeAdjMin_UserL As Single
        pDHfreeAdjMin_UserL = gUnit.L_ConToUser(mCSeal.DHfreeAdjMin)

        i = 0       '....Initialize
        Do While (i * pIncrement_UpDown < pDHfreeAdjMin_UserL)
            i = i + 1
        Loop

        pMin_UpDown = i * pIncrement_UpDown          '....Absolute Value.


        'UpDown Button Settings:
        '-----------------------
        With updDHfree
            .Maximum = pMax_UpDown
            .Minimum = -pMin_UpDown                 '....Algebraic Value.
            .Increment = pIncrement_UpDown
            .Refresh()

            'MsgBox("Hfree = " & mCSeal.Hfree)

            '....Make an attempt to assign the current object's value.
            Dim pDHfree_UserL As Single
            pDHfree_UserL = gUnit.L_ConToUser(mCSeal.DHfree)
            Assign_UpDownButtonValue(updDHfree, pDHfree_UserL)


            '....Diagnostic statement. 
            'MsgBox("pIncrement_UpDown = " & pIncrement_UpDown & _
            '       ", pMin_UpDown = " & pMin_UpDown & _
            '       ", pMax_UpDown = " & pMax_UpDown)

        End With

        'MsgBox("updDHfree.Value = " & updDHfree.Value)     '....Diagnostic statement. 

    End Sub

    Private Sub SetUpDown_DThetaOpening()
        '===============================               
        '....This routine the sets the "Minimum", "Maximum" & "Increment" properties 
        '........of the UpDown button for the "DThetaOpening".

        'Unit Independent:

        'Calculate the UpDown Button "Increment".
        '----------------------------------------
        '
        '....A conveniently selected # of Steps.
        Const pcNStep As Int16 = 20
        Dim pIncrement_UpDown As Single

        With mCSeal
            pIncrement_UpDown = (.DThetaOpeningAdjMax + .DThetaOpeningAdjMin) / pcNStep
        End With


        'UpDown Button Settings:
        '-----------------------    
        With updDThetaOpening
            .Maximum = mCSeal.DThetaOpeningAdjMax
            .Minimum = -mCSeal.DThetaOpeningAdjMin  '....Algebraic Val.
            .Increment = pIncrement_UpDown
            .Refresh()

            '....Make an attempt to assign the current object's value.
            Assign_UpDownButtonValue(updDThetaOpening, mCSeal.DThetaOpening)


        End With

    End Sub


    Private Sub SetUpDown_DIndexTArray()
        '=============================            
        '....This routine the sets the "Minimum", "Maximum" & "Increment" properties 
        '........of the UpDown button for the "Index" of the "ArrayTStd".

        'Unit Independent: (T itself is not unit independent, but the index of the 'TArray' is)

        '....For 'T', the index of the "ArrayTStd", exposed by the SealIPELib.clsCSeal object is 
        '........decremented or incremented.
        Dim pIncrement_UpDown As Int16, pMax_UpDown As Int16, pMin_UpDown As Int16

        With mCSeal
            '....Calculate the value of the "Increment".
            pIncrement_UpDown = 1       '....Index is always incremented by 1.

            '....Calculate the maximum and minimum index adjustments.
            '........Maximum Index = UBArrayTStd, Minimum Index = 0.
            pMax_UpDown = .UBArrayTStd - .IndexTStd
            pMin_UpDown = .IndexTStd - 0                '....Absolute Value.

        End With


        'UpDown Button Settings:
        '-----------------------
        With updDIndexTArray
            .Maximum = pMax_UpDown
            .Minimum = -pMin_UpDown                     '....Algebraic Value.
            .Increment = pIncrement_UpDown
            .Refresh()

            '....Make an attempt to assign the current object's value.      
            Dim pDIndexTArray As Int16
            pDIndexTArray = mCSeal.IndexT - mCSeal.IndexTStd
            Assign_UpDownButtonValue(updDIndexTArray, pDIndexTArray)

        End With

    End Sub

    Private Sub Assign_UpDownButtonValue(ByRef updown_In As NumericUpDown, _
                                         ByVal value_In As Single)
        '===================================================================

        Dim pValue As Decimal = CDec(value_In)

        If pValue >= updown_In.Minimum And pValue <= updown_In.Maximum Then
            '....Value acceptable.         
            If Abs(updown_In.Value - pValue) > gcEPS Then _
                   updown_In.Value = pValue


        ElseIf pValue < updown_In.Minimum Then
            updown_In.Value = updown_In.Minimum

        ElseIf pValue > updown_In.Maximum Then
            updown_In.Value = updown_In.Maximum
        End If

    End Sub

    Private Sub DisplayData()
        '====================
        '....This routine displays the current state of the local seal object "mCSeal". 

        'RetrieveFromDB()

        '....Use the local seal object.
        With mCSeal

            txtCrossSecNo.Text = .MCrossSecNo
            txtPOrient.Text = .POrient
            txtDControl.Text = gUnit.WriteInUserL(.DControl)

            lblWidStd.Text = gUnit.WriteInUserL(.WidStd)
            lblWid.Text = gUnit.WriteInUserL(.Wid)

            '....Standard parameter values:
            txtHFreeStd.Text = gUnit.WriteInUserL(.HfreeStd)
            txtThetaOpeningStd.Text = Format(.ThetaOpeningStd, "##0.0")
            txtTStd.Text = gUnit.WriteInUserL(.TStd, "LFormat")

            '....Adjusted parameter values:
            'txtHfree.Text = gUnit.WriteInUserL(.Hfree)
            'txtThetaOpening.Text = Format(.ThetaOpening, "##0.0")
            'txtT.Text = gUnit.WriteInUserL(.T, "LFormat")

            '....Set Fore color & Back color.       'AM 18JAN10
            txtCrossSecNo.BackColor = Color.Gainsboro
            txtPOrient.BackColor = Color.Gainsboro
            txtDControl.ForeColor = Color.Blue
            txtDControl.BackColor = Color.Gainsboro

            txtHFreeStd.ForeColor = Color.Magenta
            txtHFreeStd.BackColor = Color.Gainsboro

            txtThetaOpeningStd.ForeColor = Color.Magenta
            txtThetaOpeningStd.BackColor = Color.Gainsboro

            txtTStd.ForeColor = Color.Magenta
            txtTStd.BackColor = Color.Gainsboro

        End With


        With mPartProject.PNR.HW

            'If (.Adjusted) Then

            If (Math.Abs(.DHfree) > gcEPS) Then
                txtHfree.Text = gUnit.WriteInUserL(.Hfree + .DHfree)
            Else
                txtHfree.Text = gUnit.WriteInUserL(.Hfree)
            End If

            If (Math.Abs(.DThetaOpening) > gcEPS) Then
                txtThetaOpening.Text = Format((mCSeal.ThetaOpeningStd + .DThetaOpening), "##0.0")
            Else
                txtThetaOpening.Text = Format(mCSeal.ThetaOpening, "##0.0")

            End If

            If (Math.Abs(.T) > gcEPS) Then
                txtT.Text = gUnit.WriteInUserL(.T, "LFormat")
            Else
                txtT.Text = .TStd
            End If


            'End If

        End With

    End Sub

#End Region


#Region "NUMERIN UP-DOWN CONTROL RELATED ROUTINES:"

    Private Sub updDHfree_Enter(sender As System.Object,
                                e As System.EventArgs) Handles updDHfree.Enter
        '======================================================================
        mblnUpdDHfree_Entered = True
    End Sub

    Private Sub updDHfree_ValueChanged(sender As System.Object,
                                       e As System.EventArgs) Handles updDHfree.ValueChanged
        '=====================================================================================
        If mblnUpdDHfree_Entered = True Then

            Dim pDHfree As Single
            pDHfree = gUnit.L_UserToCon(CType(updDHfree.Value, Single))

            Dim pHfree As Single
            pHfree = mCSeal.HfreeStd + pDHfree

            txtHfree.Text = gUnit.WriteInUserL(pHfree)
        End If
    End Sub


    Private Sub updDThetaOpening_Enter(sender As System.Object,
                                       e As System.EventArgs) Handles updDThetaOpening.Enter
        '====================================================================================
        mblnUpdDThetaOpening_Entered = True
    End Sub

    Private Sub updDThetaOpening_ValueChanged(sender As System.Object,
                                              e As System.EventArgs) Handles updDThetaOpening.ValueChanged
        '==================================================================================================
        If mblnUpdDThetaOpening_Entered = True Then
            Dim pThetaOpening As Single
            pThetaOpening = mCSeal.ThetaOpeningStd + updDThetaOpening.Value
            txtThetaOpening.Text = ConvertToStr(pThetaOpening, "##0.0")
        End If
    End Sub

    Private Sub updDIndexTArray_Enter(sender As System.Object,
                                      e As System.EventArgs) Handles updDIndexTArray.Enter
        '==================================================================================
        mblnUpd_DIndexTArray_Entered = True

    End Sub

    Private Sub updDIndexTArray_ValueChanged(sender As System.Object,
                                             e As System.EventArgs) Handles updDIndexTArray.ValueChanged
        '================================================================================================
        If mblnUpd_DIndexTArray_Entered = True Then

            Try

                With mCSeal

                    Dim pIndexT As Int16
                    pIndexT = .IndexTStd + CInt(updDIndexTArray.Value)

                    '....Check if the inner radius is calculated to be a positive value. 
                    '........14NOV06. Resolves DR V4.1, Error 03.
                    Dim pRin As Single
                    pRin = (0.5 * .Hfree - .ArrayTStd(pIndexT))

                    If pRin > gcEPS Then
                        txtT.Text = gUnit.WriteInUserL(.ArrayTStd(pIndexT), "LFormat")

                    Else
                        Dim pstrMsg As String
                        pstrMsg = "T can't be further incremented as inner radius " & _
                                  "tends to become -ive."
                        MessageBox.Show(pstrMsg, "Thickness Error", MessageBoxButtons.OK, _
                                                                    MessageBoxIcon.Error)

                        '....Reset the updown button to the previous value.
                        updDIndexTArray.Value = updDIndexTArray.Value - 1
                        Exit Sub
                    End If

                End With

            Catch pEXP As Exception
                MessageBox.Show(pEXP.Message)
            End Try

        End If

    End Sub

#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtHfree_TextChanged(sender As System.Object,
                                     e As System.EventArgs) Handles txtHfree.TextChanged
        '=================================================================================
        If mCSeal Is Nothing = True Then Exit Sub

        '--------------------------------------------------------------------------------
        '
        Dim pstrMsg As String
        Dim pblnDHfree_Modified As Boolean

        '....Calculate DHfree value from the Hfree Value in the text box and 
        '........check if it is acceptable.
        '
        Dim pDHfree As Single
        '....Convert the text box value from User Unit ===> Consistent Unit.
        pDHfree = gUnit.L_UserToCon(txtHfree.Text) - mCSeal.HfreeStd

        Dim pDHfree_UserL As Single
        pDHfree_UserL = gUnit.L_ConToUser(pDHfree)

        Dim pERROR_ROUND_OFF_UserL As Single
        If gUnit.System = "English" Then
            pERROR_ROUND_OFF_UserL = 0.0005

        ElseIf gUnit.System = "Metric" Then
            pERROR_ROUND_OFF_UserL = 0.006          '....0.005 ==> 0.006. PB 22MAR07
        End If                                      '....Corrects Error # 25 DR V50.

        Try

            If pDHfree_UserL >= updDHfree.Minimum And pDHfree_UserL <= updDHfree.Maximum Then
                '----------------------------------------------------------------------------
                '....Value acceptable.
                pblnDHfree_Modified = False

                '....Resolves DR - V41: error 2. 13NOV06.
            ElseIf pDHfree_UserL < (updDHfree.Minimum - pERROR_ROUND_OFF_UserL) Then
                '-------------------------------------------------------------------      
                pstrMsg = "Not acceptable. Hfree is lower than the minimum value. " & _
                            vbCrLf & "Hence, it is set to the minimum value."
                MessageBox.Show(pstrMsg, "Hfree Minimum Value Error", MessageBoxButtons.OK, _
                                                                      MessageBoxIcon.Error)

                pDHfree_UserL = CType(updDHfree.Minimum, Single)
                pDHfree = gUnit.L_UserToCon(pDHfree_UserL)
                pblnDHfree_Modified = True

                '....Resolves DR - V41: error 2. 13NOV06.
            ElseIf pDHfree_UserL > (updDHfree.Maximum + pERROR_ROUND_OFF_UserL) Then
                '-------------------------------------------------------------------      
                pstrMsg = "Not acceptable. Hfree is higher than the maximum value." & _
                                vbCrLf & "Hence, it is set to the maximum value."
                MessageBox.Show(pstrMsg, "DHfree Maximum Value Error", MessageBoxButtons.OK, _
                                                                    MessageBoxIcon.Error)
                pDHfree_UserL = CType(updDHfree.Maximum, Single)
                pDHfree = gUnit.L_UserToCon(pDHfree_UserL)
                pblnDHfree_Modified = True

            End If

            Assign_UpDownButtonValue(updDHfree, pDHfree_UserL)

        Catch pEXP As Exception
            MessageBox.Show(pEXP.Message)
        End Try


        '....Assign the DHfree value to the local seal object property.
        With mCSeal
            .DHfree = pDHfree

            If (pblnDHfree_Modified = True) Then _
                txtHfree.Text = gUnit.WriteInUserL(.Hfree)

            txtDControl.Text = gUnit.WriteInUserL(.DControl)    '....Update display.
        End With

        'AES 27JUL17
        '....Assign the DHfree value to the local HW object property.
        mPartProject.PNR.HW.DHfree = pDHfree

        '....Check the "T" value if it is within the allowable max. & min. manufacturing limits, 
        '........which are dependent on Hfree.  
        Dim pT As Single = gUnit.L_UserToCon(txtT.Text)
        CheckTValue(pT)

        '....Redraw Seal Geometries.
        DoGraphics()

    End Sub


    Private Sub txtThetaOpening_TextChanged(sender As System.Object,
                                            e As System.EventArgs) Handles txtThetaOpening.TextChanged
        '==============================================================================================

        If mCSeal Is Nothing = True Then Exit Sub

        '................................................................................
        '

        'Check if the entered value of the "ThetaOpening" exceeds the preset hard limits.
        '--------------------------------------------------------------------------------
        Const pcThetaOpeningMax_HardLim As Single = 180
        Const pcThetaOpeningMin_HardLim As Single = 10

        Dim pThetaOpening_Entered As Single = Val(txtThetaOpening.Text)
        Dim pstrMsg As String = ""

        If pThetaOpening_Entered > pcThetaOpeningMax_HardLim Then
            pstrMsg = "Opening angle can't be higher than " & pcThetaOpeningMax_HardLim
            MessageBox.Show(pstrMsg, "Opening Angle Error", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
            '....Reset the text box display.
            txtThetaOpening.Text = pcThetaOpeningMax_HardLim


        ElseIf Val(txtThetaOpening.Text) < pcThetaOpeningMin_HardLim Then
            pstrMsg = "Opening angle can't be lower than " & pcThetaOpeningMin_HardLim
            MessageBox.Show(pstrMsg, "Opening Angle Error", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)

            '....Reset the text box display.
            txtThetaOpening.Text = pcThetaOpeningMin_HardLim
        End If

        '--------------------------------------------------------------------------------

        '....Calculate "DThetaOpening" value from the "ThetaOpening" Value in the 
        '........text box and check if it is acceptable.
        '
        Dim pDThetaOpening As Single
        pDThetaOpening = Val(txtThetaOpening.Text) - mCSeal.ThetaOpeningStd

        If pDThetaOpening >= updDThetaOpening.Minimum And _
           pDThetaOpening <= updDThetaOpening.Maximum Then
            '-----------------------------------------------
            '....Value acceptable.
            txtThetaOpening.BackColor = Color.White
            pstrMsg = ""


        ElseIf pDThetaOpening < updDThetaOpening.Minimum Then
            '-------------------------------------------------
            txtThetaOpening.BackColor = Color.Red
            pstrMsg = "WARNING: Opening angle is lower than the minimum allowable value."


        ElseIf pDThetaOpening > updDThetaOpening.Maximum Then
            '-------------------------------------------------
            txtThetaOpening.BackColor = Color.Red
            pstrMsg = "WARNING: Opening angle is higher than the maximum allowable value."

        End If

        lblErrMsg.Text = pstrMsg

        Assign_UpDownButtonValue(updDThetaOpening, pDThetaOpening)

        '....Assign the DThetaOpening value to the local seal object property.
        mCSeal.DThetaOpening = pDThetaOpening

        'AES 27JUL17
        '....Assign the DHfree value to the local HW object property.
        mPartProject.PNR.HW.DThetaOpening = pDThetaOpening

        '....Re-draw Seal Geometries
        DoGraphics()
    End Sub

    Private Sub txtT_TextChanged(sender As System.Object,
                                 e As System.EventArgs) Handles txtT.TextChanged
        '=======================================================================

        If mCSeal Is Nothing = True Then Exit Sub

        '-------------------------------------------------------------------

        'Check if the entered value of the "Thickness" exceeds the preset hard limits.
        '-------------------------------------------------------------------------------

        '....Preset hard limits on Thickness: Minimum = 0.002 in & Maximum = 0.080 in.
        '
        '........In user unit: (in or mm)
        Dim pcThickMin_HardLim As Single = gUnit.L_ConToUser(0.002 * gUnit.CFacConL)
        Dim pcThickMax_HardLim As Single = gUnit.L_ConToUser(0.08 * gUnit.CFacConL)


        Dim pThick_Entered As Single = ConvertToSng(txtT.Text)
        Dim pstrMsg As String

        If pThick_Entered > pcThickMax_HardLim Then
            pstrMsg = "Thickness can't be higher than " & pcThickMax_HardLim
            MessageBox.Show(pstrMsg, "Thickness Error", MessageBoxButtons.OK, _
                                                        MessageBoxIcon.Error)
            '....Reset the text box display.
            txtT.Text = Format(pcThickMax_HardLim, gUnit.LFormat)


        ElseIf pThick_Entered < pcThickMin_HardLim Then
            pstrMsg = "Thickness can't be lower than " & pcThickMin_HardLim
            MessageBox.Show(pstrMsg, "Thick Error", MessageBoxButtons.OK, _
                                                    MessageBoxIcon.Error)
            '....Reset the text box display.
            txtT.Text = Format(pcThickMin_HardLim, gUnit.LFormat)

        End If


        Dim pT As Single = gUnit.L_UserToCon(txtT.Text)
        CheckTValue(pT)

        'Re-draw Seal Geometries.
        '------------------------
        '....First check if the inner radius is calculated to be a positive value. 
        '........Resolves DR V4.1, Error 03
        '
        Dim pRin As Single
        pRin = (0.5 * mCSeal.Hfree - pT)


        If pRin > gcEPS Then
            With mCSeal
                '....Assign the Thickness value to the local seal object property.
                .T = pT

                Dim pDIndexTArray As Int16
                pDIndexTArray = .IndexT - .IndexTStd

                Assign_UpDownButtonValue(updDIndexTArray, pDIndexTArray)

            End With

            DoGraphics()

        Else
            pstrMsg = "T can't be further decremented as inner radius tends to become -ive."
            MessageBox.Show(pstrMsg, "Thickness Error", MessageBoxButtons.OK, _
                                                        MessageBoxIcon.Error)

            txtT.Text = gUnit.WriteInUserL(mCSeal.T, "LFormat")

        End If

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub CheckTValue(ByVal T_In As Single)
        '========================================
        '....This routine checks the input "T" value, obtained from the corresponding 
        '........Text Box and if it is outside the acceptable limits, gives warning.


        Dim pstrMsg As String = ""


        If T_In < mCSeal.TAdjMin Then
            '------------------------
            txtT.BackColor = Color.Red
            pstrMsg = "WARNING: Thickness is lower than the minimum allowable value. "


        ElseIf T_In >= mCSeal.TAdjMin And T_In <= mCSeal.TAdjMax Then
            '--------------------------------------------------------
            '....Value acceptable.
            txtT.BackColor = Color.White
            pstrMsg = ""


        ElseIf T_In > mCSeal.TAdjMax Then
            '----------------------------
            txtT.BackColor = Color.Red
            pstrMsg = "WARNING: Thickness is higher than the maximum allowable value. "

        End If

        lblErrMsg.Text = pstrMsg

    End Sub

#End Region

#End Region

#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==================================================================
        Dim pCmdBtn As Button = CType(sender, Button)

        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
        End If

        Me.Close()

    End Sub


    Private Sub SaveData()
        '=================    
        '....Local form data ===> Global Seal Object Data.
        With mPartProject.PNR.HW
            .DHfree = gUnit.L_UserToCon(txtHfree.Text) - mCSeal.HfreeStd
            .DThetaOpening = Val(txtThetaOpening.Text) - mCSeal.ThetaOpeningStd
            .T = gUnit.L_UserToCon(txtT.Text)
        End With

    End Sub

#End Region

#Region "UTILITY ROUTINE:"

    Private Sub DoGraphics()
        '===================
        'This routine draws the 'Standard' & 'Adjusted' Geometries.

        '....Drawing envelope:
        Dim xEnvpTopL As Single
        Dim yEnvpTopL As Single
        Dim xEnvpBotR As Single
        Dim yEnvpBotR As Single

        'Graphics Settings:
        '------------------
        '....Array Index = 0 ===> "Standard Geometry"
        '....Array Index = 1 ===> "Adjusted Geometry"

        '....Color:
        Dim pColor(1) As Color
        pColor(0) = Color.Black
        pColor(1) = Color.Blue

        '....Drawing Width (Pixels)  
        Dim pDrawWid(1) As Integer
        pDrawWid(0) = 2
        pDrawWid(1) = 2     '....Width = 1 here doesn't work, nor necessary. 04JUL06.

        '....Dash Style:
        Dim pDashStyle(1) As Integer
        pDashStyle(0) = DashStyle.Solid     '....Value = 0
        pDashStyle(1) = DashStyle.DashDot   '....Value = 1    


        'Draw the seals.
        '---------------
        Dim pGr As Graphics = GetGraphicsObj(picSeal)

        '....Pixel densities per unit "PageUnit" dimension (in or mm)
        Dim pDpX As Single
        Dim pDpY As Single

        '....Set the PageUnit property.
        If gUnit.System = "English" Then
            pGr.PageUnit = GraphicsUnit.Inch

            '....# of Pixels/in
            pDpX = pGr.DpiX
            pDpY = pGr.DpiY

        ElseIf gUnit.System = "Metric" Then
            pGr.PageUnit = GraphicsUnit.Millimeter

            '....# of Pixels/mm
            pDpX = pGr.DpiX / gUnit.EngLToUserL(1.0)
            pDpY = pGr.DpiY / gUnit.EngLToUserL(1.0)
        End If


        '....Size of the graphics area in the "page unit" system.
        Dim pSize As New SizeF(picSeal.Width / pDpX, picSeal.Height / pDpY)


        '....Draw both "Standard" & "Adjusted" Seal Geometry.
        mCSeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                    "BOTH", "SCALE_BY_STD", 2.5, _
                                    xEnvpTopL, yEnvpTopL, xEnvpBotR, yEnvpBotR)


        'Caption Labels:       
        '---------------
        If mCSeal Is Nothing = False Then

            lblStandard.Text = "Standard  =  " & gUnit.WriteInUserL((mCSeal.HfreeStd))

            Dim psngDelHfreePCent As Single
            psngDelHfreePCent = (mCSeal.Hfree - mCSeal.HfreeStd) * 100 / mCSeal.HfreeStd

            '....Calculate the maximum round-off/-up error on the "txtHfree" value 
            '........when DHFree = 0.0
            '
            '   ....Calculate the maximum DelHfreePCent possible.
            '
            '   ........English Unit:  LFormat = "##0.000". 
            '   ............Therefore, the maximum round-off/-up error = 0.0005.

            '   ....Metric  Unit:  LFormat = "##0.00". 
            '   ........Therefore, the maximum round-off/-up error = 0.005.

            Dim pERROR_ROUNDING_Max_Eng As Single = 0.0005
            Dim pERROR_ROUNDING_Max_Met As Single = 0.006    '....0.005 ==> 0.006. 
            '                                   PB 22MAR07. Corrects Error # 25 DR V50.

            Dim pERROR_ROUNDING_Max As Single
            If gUnit.System = "English" Then
                pERROR_ROUNDING_Max = pERROR_ROUNDING_Max_Eng

            ElseIf gUnit.System = "Metric" Then
                pERROR_ROUNDING_Max = pERROR_ROUNDING_Max_Met
            End If

            Dim pERROR_DelHfreePCent_Max As Single
            Dim pHfreeStd_UserL As Single = gUnit.L_ConToUser(mCSeal.HfreeStd)
            pERROR_DelHfreePCent_Max = pERROR_ROUNDING_Max * 100 / pHfreeStd_UserL


            If Abs(psngDelHfreePCent) <= pERROR_DelHfreePCent_Max Then   '0.1# Then
                lblCSealAdjusted.Visible = False

            ElseIf Abs(psngDelHfreePCent) > pERROR_DelHfreePCent_Max Then  ' 0.1# Then
                lblCSealAdjusted.Visible = True
                lblCSealAdjusted.Text = "Adjusted   =  " & _
                                    gUnit.WriteInUserL(mCSeal.Hfree) & _
                                    "  ( " & Format(psngDelHfreePCent, "##0.0") & " %)"
            End If

            lblWid.Text = gUnit.WriteInUserL(mCSeal.Wid)

        End If

    End Sub

#End Region


End Class