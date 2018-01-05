'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmDesignCenterUSeal                   '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29MAR16                                '
'                                                                              '
'===============================================================================
'
Imports System.Math
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports System.Drawing.Printing
Imports clsLibrary11


Public Class IPE_frmDesignCenterUSeal
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLE DECLARATION:"

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single

    '....Flags to indicate if the UpDown buttons have been entered by the user.
    Private mblnUpdIndexTArray_Entered As Boolean = False
    Private mblnUpdLLeg_Entered As Boolean = False

    Private mblnUpdRad1_Entered As Boolean = False
    Private mblnUpdRad2_Entered As Boolean = False

    Private mblnUpdTheta1_Entered As Boolean = False
    Private mblnUpdTheta2_Entered As Boolean = False

    Dim mblnValidData As Boolean = True

    Private mUSeal As IPE_clsUSeal        '....Local Seal object.


#End Region


#Region "FORM CONSTRUCTOR & RELATED ROUTINES:"

    '....Constructor
    Public Sub New()
        '===========

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        '   --------------
        '   Picture Box  '
        '   --------------

        '   Set Margin of the Picture Box.
        '   ------------------------------
        Dim pUniformMargin As Single = 0.4       '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        '   ---------------
        '   UpDown Buttons:
        '   ---------------    

        '   Set the Properties of the UpDown Buttons.
        '   -----------------------------------------
        SetProperties_UpDownButton_All()

    End Sub


#Region "HELPER ROUTINES:"


    Private Sub SetProperties_UpDownButton_All()
        '========================================
        '....This routine is called by:
        '       1. New (Constructor).

        '....Set the "Maximum", "Minimum" & "Increment" properties of the UpDown buttons 
        '........assign the local object's value.
        '

        'Set Array of all UpDown Buttons:  
        '---------------------------------------
        Dim pUpd() As NumericUpDown = {Nothing, updIndexTArray, updLLeg, _
                                       updTheta1, updRad1, updTheta2, updRad2}


        'Assign Min., Max. & Increment Values:  
        '-------------------------------------
        Dim pIncVal As Single = 0.0F

        With pUpd(1)
            .Minimum = 0
            .Maximum = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.UBArrayTStd
            .Increment = 1
            .Refresh()
        End With

        Dim i As Integer
        For i = 2 To pUpd.Length - 1
            With pUpd(i)
                If i Mod 2 <> 0 Then
                    .Minimum = 0
                    .Maximum = 200
                    pIncVal = 0.5
                Else
                    .Minimum = -100
                    .Maximum = 100

                    If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.UnitSystem = "English" Then
                        pIncVal = 0.001
                    ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.UnitSystem = "Metric" Then
                        pIncVal = 0.01
                    End If
                End If
                .Increment = pIncVal

                .Refresh()
            End With
        Next

    End Sub

#End Region


#End Region


#Region "FORM RELATED ROUTINES:"


    Private Sub frmDesignCenterUSeal_Activated(ByVal sender As Object, _
                                              ByVal e As System.EventArgs) _
                                              Handles Me.Activated
        '======================================================================

        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then _
            cmdViewNomenclature.Enabled = gIPE_frmNomenclature_DesignCenter.FormClose

    End Sub


    Private Sub frmDesignCentreUSeal_Load(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles MyBase.Load
        '=====================================================================================

        '   INITIALIZATIONS:
        '   ----------------
        '....Initialize the Local Seal Object.
        InitializeLocalObject()                 '....gSeal ===> mESeal

        '....Display data on the form.
        '........Local seal object "mUSeal" (which is recently initialized above) is used.
        DisplayData()

        '....Display graphics on the picture box.
        DoGraphics()

        InitializeControls()
    End Sub



#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '==============================
        '....From gSeal ===> mUSeal. 
        '........Now onwards, mESeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal in the 
        '........"SaveData" routine which is called when the form is exited and another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local Seal Object.
        mUSeal = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo <> "" Then
            'mUSeal = CType(gSeal, IPE_clsUSeal).Clone()
            mUSeal = CType(gIPE_SealNew, IPE_clsUSeal).Clone()
        End If

        '....The local seal object may be redesigned on this form, starting with the   
        '........design parameters of the already existing global USeal object "gSeal".
        '........Hence, the following property setting is necessary.  


    End Sub


    Private Sub InitializeControls()
        '===========================

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            txtCrossSecNo_New.Enabled = False
            updLLeg.Enabled = False
            txtLLeg_New.Enabled = False
            updTheta1.Enabled = False
            txtTheta1_New.Enabled = False
            updRad1.Enabled = False
            txtRad1_New.Enabled = False
            updTheta2.Enabled = False
            txtTheta2_New.Enabled = False
            updRad2.Enabled = False
            txtRad2_New.Enabled = False

        Else
            txtCrossSecNo_New.Enabled = True
            updLLeg.Enabled = True
            txtLLeg_New.Enabled = True
            updTheta1.Enabled = True
            txtTheta1_New.Enabled = True
            updRad1.Enabled = True
            txtRad1_New.Enabled = True
            updTheta2.Enabled = True
            txtTheta2_New.Enabled = True
            updRad2.Enabled = True
            txtRad2_New.Enabled = True

        End If

    End Sub

    Private Sub DisplayData()
        '====================
        '....This routine displays the current state of the local object. 

        '   ORIGINAL PARAMETERS:  (for reference)
        '   ====================  
        '
        '   ....Display the original global seal object parameters. 
        '
        With CType(gIPE_SealOrg, IPE_clsUSeal)
            txtCrossSecNo.Text = .MCrossSecNo
            txtCrossSecNo.BackColor = Color.Gainsboro

            txtCrossSecNo_New.Text = CType(gIPE_SealNew, IPE_clsUSeal).MCrossSecNo

            txtPOrient.Text = .POrient
            txtPOrient.BackColor = Color.Gainsboro

            txtDControl.Text = gIPE_Unit.WriteInUserL(.DControl)
            txtDControl.BackColor = Color.Gainsboro

            lblWidStd.Text = gIPE_Unit.WriteInUserL(.WidStd)

            '....Standard parameter values:                      
            txtTheta1Std.Text = Format(.ThetaStd(1), gIPE_Unit.LFormat)
            txtTheta1Std.BackColor = Color.Gainsboro

            txtTheta2Std.Text = Format(.ThetaStd(2), gIPE_Unit.LFormat)
            txtTheta2Std.BackColor = Color.Gainsboro

            txtLLegStd.Text = gIPE_Unit.WriteInUserL(.LLegStd, "LFormat")
            txtLLegStd.BackColor = Color.Gainsboro

            txtRad1Std.Text = gIPE_Unit.WriteInUserL(.RStd(1), "LFormat")
            txtRad1Std.BackColor = Color.Gainsboro

            txtRad2Std.Text = gIPE_Unit.WriteInUserL(.RStd(2), "LFormat")
            txtRad2Std.BackColor = Color.Gainsboro

            'txtTStd.Text = gIPE_Unit.WriteInUserL(.TStd, "#0.000")
            txtT.Text = gIPE_Unit.WriteInUserL(.TStd, "LFormat")
            txtT.BackColor = Color.Gainsboro

        End With

        '   MODIFIED PARAMETERS:
        '   ====================
        '   ....Display the local seal object parameters as initialization. 
        '   ........They may be modified by the user later. 
        '
        With mUSeal
            If IPE_clsUSeal.CrossSecList.Contains(.MCrossSecNo) Then
                '....A new cross-section number has not been assigned yet.
                'txtCrossSecNo_New.Text = ""
            Else
                txtCrossSecNo_New.Text = CType(gIPE_SealNew, IPE_clsUSeal).MCrossSecNo
                If IPE_clsUSeal.CrossSecNewList.Count > 0 And _
                        Not IPE_clsUSeal.CrossSecNewList.Contains(CType(gIPE_SealNew, IPE_clsUSeal).MCrossSecNo) And _
                            Not IPE_clsUSeal.CrossSecList.Contains(txtCrossSecNo_New.Text.Trim()) Then
                    txtCrossSecNo_New.ForeColor = Color.Green
                Else
                    txtCrossSecNo_New.ForeColor = Color.Black
                End If
            End If

            lblWid_New.Text = gIPE_Unit.WriteInUserL(.Wid)

            If .Theta(1) - CType(gIPE_SealOrg, IPE_clsUSeal).Theta(1) > gcEPS Then
                txtTheta1_New.Text = Format(.Theta(1), "#0.0")
            Else
                txtTheta1_New.Text = Format(.Theta(1), gIPE_Unit.LFormat)
            End If

            If .Theta(2) - CType(gIPE_SealOrg, IPE_clsUSeal).Theta(2) > gcEPS Then
                txtTheta2_New.Text = Format(.Theta(2), "#0.0")
            Else
                txtTheta2_New.Text = Format(.Theta(2), gIPE_Unit.LFormat)
            End If


            txtLLeg_New.Text = gIPE_Unit.WriteInUserL(.LLeg, "LFormat")
            txtRad1_New.Text = gIPE_Unit.WriteInUserL(.R(1), "LFormat")
            txtRad2_New.Text = gIPE_Unit.WriteInUserL(.R(2), "LFormat")

            'txtT.Text = gIPE_Unit.WriteInUserL(.T, "#0.000")
            txtT_New.Text = gIPE_Unit.WriteInUserL(.T, "LFormat")

            '....Set Fore color & Back color.       
            txtCrossSecNo.BackColor = Color.Gainsboro
            txtPOrient.BackColor = Color.Gainsboro
            txtDControl.BackColor = Color.Gainsboro

        End With

    End Sub

#End Region


#End Region


#Region "CONTROL EVENT ROUTINES:"


#Region "UPDOWN CONTROL RELATED ROUTINES:"

    Private Sub upd_Enter(ByVal sender As Object, ByVal e As System.EventArgs) _
                          Handles updIndexTArray.Enter, updLLeg.Enter, _
                                  updRad1.Enter, updRad2.Enter, _
                                  updTheta1.Enter, updTheta2.Enter
        '==================================================================

        Dim pUpDown As NumericUpDown = CType(sender, NumericUpDown)

        Select Case pUpDown.Name

            Case "updIndexTArray"
                mblnUpdIndexTArray_Entered = True

            Case "updLLeg"
                mblnUpdLLeg_Entered = True

            Case "updRad1"
                mblnUpdRad1_Entered = True

            Case "updRad2"
                mblnUpdRad2_Entered = True

            Case "updTheta1"
                mblnUpdTheta1_Entered = True

            Case "updTheta2"
                mblnUpdTheta2_Entered = True

        End Select

    End Sub


    Private Sub upd_ValueChanged(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles updIndexTArray.ValueChanged, updLLeg.ValueChanged, _
                                 updRad1.ValueChanged, updRad2.ValueChanged, _
                                 updTheta1.ValueChanged, updTheta2.ValueChanged
        '=====================================================================================

        If mUSeal Is Nothing = True Then Exit Sub

        Dim pUpDown As NumericUpDown = CType(sender, NumericUpDown)
        Dim iSel As Int16 = updIndexTArray.Value

        With mUSeal

            Select Case pUpDown.Name

                Case "updIndexTArray"
                    '----------------
                    txtT_New.Text = gIPE_Unit.WriteInUserL(.ArrayTStd(iSel), "LFormat")

                Case "updLLeg"
                    '---------
                    If mblnUpdLLeg_Entered Then
                        '-----------------------------
                        txtLLeg_New.Text = updLLeg.Value
                    End If

                Case "updRad1"
                    '---------
                    If mblnUpdRad1_Entered Then
                        '-----------------------------
                        txtRad1_New.Text = updRad1.Value
                    End If

                Case "updRad2"
                    '---------
                    If mblnUpdRad2_Entered Then
                        '-----------------------------
                        txtRad2_New.Text = updRad2.Value
                    End If

                Case "updTheta1"
                    '------------
                    If mblnUpdTheta1_Entered Then
                        '--------------------------------
                        txtTheta1_New.Text = WriteAngle(updTheta1.Value)

                    End If

                Case "updTheta2"
                    '-----------
                    If mblnUpdTheta2_Entered Then
                        '---------------------------------
                        txtTheta2_New.Text = WriteAngle(updTheta2.Value)

                    End If

            End Select

        End With

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub AssignValue_UpdButton(ByRef upd_In As NumericUpDown, _
                                  ByVal value_In As Single)
        '===================================================================

        Dim pVal As Decimal = CDec(value_In)

        '   Set the Value (if necessary):
        '   --------------
        '
        '....Check if the updown button value is different from the given argument value.
        '
        '........If they are equal, this routine's tasks need not be performed ==> EXIT.
        If Abs(upd_In.Value - pVal) <= gcEPS Then Exit Sub
        '------------------------------------EXITED---------------------------------->

        '   Set the appropriate flag.
        '   -------------------------
        '   ....This flag indicates that the updown button value is set programmatically
        '   ........not by user clicking.
        '
        Dim pName As String = upd_In.Name
        Select Case pName

            Case "updIndexTArray"
                mblnUpdIndexTArray_Entered = False

            Case "updLLeg"
                mblnUpdLLeg_Entered = False

            Case "updRad1"
                mblnUpdRad1_Entered = False

            Case "updRad2"
                mblnUpdRad2_Entered = False

            Case "updTheta1"
                'mblnUpdTheta1_Entered = False

            Case "updTheta2"
                'mblnUpdTheta2_Entered = False
        End Select

        '....The updown button value is different from the given argument value. 
        If pVal >= upd_In.Minimum And pVal <= upd_In.Maximum Then
            '....Value acceptable. Set the updown button value only when it is 
            '.......different from "pVal". 
            upd_In.Value = pVal

        ElseIf pVal < upd_In.Minimum Then
            upd_In.Value = upd_In.Minimum

        ElseIf pVal > upd_In.Maximum Then
            upd_In.Value = upd_In.Maximum
        End If

    End Sub


    Public Function WriteAngle(ByVal Angel_In As Single) As String
        '=========================================================     

        Dim pIncr As Single = 0.5

        Dim pAngle As Single = Angel_In / pIncr
        Dim pintAngle As Integer = NInt(pAngle)

        pAngle = pintAngle * pIncr

        Return pAngle.ToString("#0.0")
    End Function

#End Region

#End Region


#Region "TEXTBOX RELATED ROUTINES:"

    Private Sub txt_New_TextChanged(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) _
                                    Handles txtT_New.TextChanged, txtLLeg_New.TextChanged, _
                                    txtRad1_New.TextChanged, txtRad2_New.TextChanged, _
                                    txtTheta1_New.TextChanged, txtTheta2_New.TextChanged
        '=================================================================================

        If mUSeal Is Nothing = True Then Exit Sub

        Dim pTxtBox As TextBox = CType(sender, TextBox)

        If mblnValidData = False Then
            mblnValidData = True
            Exit Sub
        End If


        With mUSeal

            '   Select the Up-Down button corresponding to the text box.
            '   --------------------------------------------------------
            Dim pUpd As New NumericUpDown
            Dim pVal_Org As Single, pVal_New As Single

            Select Case pTxtBox.Name

                Case "txtT_New"
                    '---------                    
                    pVal_Org = ConvertToSng(txtT.Text)
                    Dim pT As Single = ConvertToSng(txtT_New.Text)
                    pVal_New = pT

                Case "txtLLeg_New"
                    '-------------
                    pUpd = updLLeg
                    pVal_Org = ConvertToSng(txtLLegStd.Text)
                    pVal_New = ConvertToSng(txtLLeg_New.Text)

                Case "txtRad1_New"
                    '------------
                    pUpd = updRad1
                    pVal_Org = ConvertToSng(txtRad1Std.Text)
                    pVal_New = ConvertToSng(txtRad1_New.Text)

                Case "txtRad2_New"
                    '------------
                    pUpd = updRad2
                    pVal_Org = ConvertToSng(txtRad2Std.Text)
                    pVal_New = ConvertToSng(txtRad2_New.Text)

                Case "txtTheta1_New"
                    '--------------
                    pUpd = updTheta1
                    pVal_Org = ConvertToSng(txtTheta1Std.Text)
                    pVal_New = ConvertToSng(txtTheta1_New.Text)

                Case "txtTheta2_New"
                    '--------------
                    pUpd = updTheta2
                    pVal_Org = ConvertToSng(txtTheta2Std.Text)
                    pVal_New = ConvertToSng(txtTheta2_New.Text)

                Case Else
                    Exit Sub

            End Select

            If pTxtBox.Text <> "" Then
                mblnValidData = modMain.NumericDataValidation(pTxtBox.Text)
                If Not mblnValidData Then
                    pTxtBox.Text = ""
                    pTxtBox.Text = "0."
                End If
            End If

            '   Check the Text Box Value, reset if necessary, and set its color.
            '   ----------------------------------------------------------------
            '   ....Reset if necessary.
            '
            SetValue_TxtBox(pUpd, pTxtBox, pVal_New)
            SetForeColor(pTxtBox, pVal_Org)

            ''....Re-store the text box value (in case it has been reset above).
            pVal_New = ConvertToSng(pTxtBox.Text)

            '   Reset Updown Button Value (if necessary only):
            '   ---------------------------------------------
            '   ....Do resetting only if the updown button value is different from 
            '   ........the text box value. 
            If Abs(pUpd.Value - pVal_New) > gcEPS Then _
                AssignValue_UpdButton(pUpd, pVal_New)


            '   Store the "New" values in the local seal object.
            '   ------------------------------------------------
            '
            Select Case pTxtBox.Name
                Case "txtT_New"
                    .T = gIPE_Unit.L_UserToCon(pVal_New)

                Case "txtLLeg_New"
                    .LLeg = gIPE_Unit.L_UserToCon(pVal_New)

                Case "txtRad1_New"
                    .R(1) = gIPE_Unit.L_UserToCon(pVal_New)

                Case "txtRad2_New"
                    .R(2) = gIPE_Unit.L_UserToCon(pVal_New)

                Case "txtTheta1_New"
                    .Theta(1) = pVal_New

                Case "txtTheta2_New"
                    .Theta(2) = pVal_New
            End Select

            '   Display Derived Parameters:
            '   ---------------------------
            Update_Display_RefVariables()

            '   Redo the Graphics.
            '   ------------------   
            DoGraphics()

        End With


    End Sub


    Private Sub txtCrossSecNo_New_TextChanged(ByVal sender As System.Object, _
                                              ByVal e As System.EventArgs) _
                                          Handles txtCrossSecNo_New.TextChanged
        '========================================================================

        If IPE_clsUSeal.CrossSecNewList.Count > 0 And _
            Not IPE_clsUSeal.CrossSecNewList.Contains(txtCrossSecNo_New.Text.Trim()) And _
                Not IPE_clsUSeal.CrossSecList.Contains(txtCrossSecNo_New.Text.Trim()) Then

            lblCrossSecNew.ForeColor = Color.Green
            txtCrossSecNo_New.ForeColor = Color.Green
        Else
            lblCrossSecNew.ForeColor = Color.Black
            txtCrossSecNo_New.ForeColor = Color.Black

        End If

    End Sub


    Private Sub CrossSecNo_New_KeyPress(ByVal sender As Object, _
                                            ByVal e As KeyPressEventArgs) _
                                            Handles txtCrossSecNo_New.KeyPress
        '===================================================================='
        Dim pArray As Char() = {"/", "\", "*", ":", "?", "<", ">", """", "|"}

        For i As Int16 = 0 To pArray.Length - 1

            If e.KeyChar = pArray(i) Then
                MessageBox.Show("CrossSecNo. can not contain any of the " & _
                                "following characters:" & vbCrLf & "\/:<>*?""|", _
                                "New CrossSecNo. Error", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.KeyChar = ""
                Exit For
            End If

        Next

    End Sub

    Private Sub txtBox_KeyPress(ByVal sender As System.Object, _
                                ByVal e As System.Windows.Forms.KeyPressEventArgs) _
                                Handles txtTheta2_New.KeyPress, txtTheta1_New.KeyPress
        '=========================================================================
        '   Set the appropriate flag.
        '   -------------------------
        '   ....This flag indicates that the updown button value is set 
        '   ........by user keypress.
        '
        Dim pTxtBox As TextBox = CType(sender, TextBox)
        Dim pName As String = pTxtBox.Name

        Select Case pName

            Case "txtTheta1_New"
                mblnUpdTheta1_Entered = False

            Case "txtTheta2_New"
                mblnUpdTheta2_Entered = False
        End Select
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub Update_Display_RefVariables()
        '====================================

        With mUSeal

            txtDControl.Text = gIPE_Unit.WriteInUserL(.DControl)
            lblWid_New.Text = gIPE_Unit.WriteInUserL(.Wid)
        End With

    End Sub


    Public Shared Sub SetValue_TxtBox(ByRef updown_In As NumericUpDown, _
                                      ByRef txtbox_In As TextBox, _
                                      ByVal value_In As Single)
        '==========================================================
        'This routine is called when the value of a text box, associated with an 
        '....UpDown control, is changed. 

        Dim pVal As Decimal = CDec(value_In)

        If pVal >= updown_In.Minimum And pVal <= updown_In.Maximum Then
            '....Value acceptable. No action necessary here.


        ElseIf pVal < updown_In.Minimum Then
            '....Value not acceptable.
            txtbox_In.Text = ConvertToStr(updown_In.Minimum, "LFormat")


        ElseIf pVal > updown_In.Maximum Then
            '....Value not acceptable.
            txtbox_In.Text = ConvertToStr(updown_In.Maximum, "LFormat")
        End If

    End Sub


    Private Sub AssignValue_TxtBox(ByRef TxtBox_In As TextBox, ByVal TxtBoxType_In As String)
        '=================================================================================
        '....Check if the entered value of the "Theta" exceeds the preset hard limits.
        '--------------------------------------------------------------------------------

        Const pcThetaMax_HardLim As Single = 180
        Const pcThetaMin_HardLim As Single = 10

        '....Preset hard limits on Thickness: Maximum = 0.080 in & Minimum = 0.002 in.
        '
        '........In user unit: (in or mm)
        Dim pcTMax_HardLim As Single = gIPE_Unit.L_ConToUser(0.08 * gIPE_Unit.CFacConL)
        Dim pcTMin_HardLim As Single = gIPE_Unit.L_ConToUser(0.002 * gIPE_Unit.CFacConL)

        Dim pVal_Entered As Single = Val(TxtBox_In.Text)
        Dim pstrMsg As String = ""

        Select Case TxtBoxType_In
            Case "Theta"
                '-------
                If pVal_Entered > pcThetaMax_HardLim Then
                    pstrMsg = "Angle can't be higher than " & pcThetaMax_HardLim
                    MessageBox.Show(pstrMsg, "Angle Error", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
                    '....Reset the text box display.
                    TxtBox_In.Text = pcThetaMax_HardLim


                ElseIf pVal_Entered < pcThetaMin_HardLim Then
                    pstrMsg = "Angle can't be lower than " & pcThetaMin_HardLim
                    MessageBox.Show(pstrMsg, "Angle Error", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)

                    '....Reset the text box display.
                    TxtBox_In.Text = pcThetaMin_HardLim
                End If

            Case "T"
                '---
                'Check if the entered value of the "Thickness" exceeds the preset hard limits.
                '-------------------------------------------------------------------------------

                If pVal_Entered > pcTMax_HardLim Then
                    pstrMsg = "Thickness can't be higher than " & pcTMax_HardLim
                    MessageBox.Show(pstrMsg, "Thickness Error", MessageBoxButtons.OK, _
                                                                MessageBoxIcon.Error)
                    '....Reset the text box display.
                    TxtBox_In.Text = Format(pcTMax_HardLim, gIPE_Unit.LFormat)


                ElseIf pVal_Entered < pcTMin_HardLim Then
                    pstrMsg = "Thickness can't be lower than " & pcTMin_HardLim
                    MessageBox.Show(pstrMsg, "Thick Error", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
                    '....Reset the text box display.
                    TxtBox_In.Text = Format(pcTMin_HardLim, gIPE_Unit.LFormat)

                End If

        End Select

    End Sub
#End Region

#End Region


#End Region


#Region "MENU EVENT ROUTINES:"

    Private Sub mnuCreateWordDocu_Click(ByVal sender As System.Object, _
                                        ByVal e As System.EventArgs) _
                                        Handles mnuCreateWordDocu.Click
        '===================================================================

        SaveData()
        gIPE_Report = New IPE_clsReport()
        gIPE_Report.CreateAdjGeomDoc(picSeal, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, gIPE_Unit, gIPE_User, gIPE_Project)

    End Sub

    Private Sub mnuPrintForm_Click(ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs) _
                                   Handles mnuPrintForm.Click
        '============================================================
        Dim pPrintDoc As New PrintDocument
        AddHandler pPrintDoc.PrintPage, AddressOf OnPrintPage
        pPrintDoc.Print()
    End Sub

    Private Sub OnPrintPage(ByVal sender As System.Object, _
                            ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '============================================================================

        Dim hwndForm As IntPtr
        hwndForm = Me.Handle

        Dim hdcDIBSection As IntPtr
        Dim hdcRef As IntPtr
        Dim hbmDIBSection As IntPtr
        Dim hbmDIBSectionOld As IntPtr
        Dim BMPheader As IPE_clsAPICalls.BITMAPINFOHEADER

        hdcRef = IPE_clsAPICalls.GetDC(IntPtr.Zero)
        hdcDIBSection = IPE_clsAPICalls.CreateCompatibleDC(hdcRef)
        IPE_clsAPICalls.ReleaseDC(IntPtr.Zero, hdcRef)

        BMPheader.biBitCount = 24
        BMPheader.biClrImportant = 0
        BMPheader.biClrUsed = 0
        BMPheader.biCompression = IPE_clsAPICalls.BI_RGB
        BMPheader.biSize = 40
        BMPheader.biHeight = Me.Height
        BMPheader.biPlanes = 1
        BMPheader.biSizeImage = 0
        BMPheader.biWidth = Me.Width
        BMPheader.biXPelsPerMeter = 0
        BMPheader.biYPelsPerMeter = 0

        hbmDIBSection = IPE_clsAPICalls.CreateDIBSection(hdcDIBSection, BMPheader, _
                                                      IPE_clsAPICalls.DIB_RGB_COLORS, _
                                                      IntPtr.Zero, IntPtr.Zero, 0)

        hbmDIBSectionOld = IPE_clsAPICalls.SelectObject(hdcDIBSection, hbmDIBSection)
        IPE_clsAPICalls.PatBlt(hdcDIBSection, 0, 0, Me.Width, Me.Height, IPE_clsAPICalls.WHITENESS)
        IPE_clsAPICalls.PrintWindow(hwndForm, hdcDIBSection, 0)
        IPE_clsAPICalls.SelectObject(hdcDIBSection, hbmDIBSectionOld)

        Dim imageFrm As Bitmap
        imageFrm = Image.FromHbitmap(hbmDIBSection)
        e.Graphics.DrawImage(imageFrm, 0, 0)

        IPE_clsAPICalls.DeleteDC(hdcDIBSection)
        IPE_clsAPICalls.DeleteObject(hbmDIBSection)

    End Sub

#End Region


#Region " COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdViewNomenclature_Click(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles cmdViewNomenclature.Click
        '====================================================================     
        gIPE_frmNomenclature_DesignCenter = New IPE_frmNomenclature_DesignCenter()
        gIPE_frmNomenclature_DesignCenter.Show()

        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then _
            cmdViewNomenclature.Enabled = gIPE_frmNomenclature_DesignCenter.FormClose

    End Sub

    Private Sub cmdDXF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                             Handles cmdDXF.Click
        '=================================================================================

        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then
            If Not gIPE_frmNomenclature_DesignCenter.FormClose Then
                gIPE_frmNomenclature_DesignCenter.SendToBack()
            End If
        End If

        If SaveData() = True Then
            Me.Cursor = Cursors.WaitCursor

            With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal)

                SaveFileDialog1.FilterIndex = 1
                SaveFileDialog1.Filter = "Configuration files (*.DXF)|*.DXF"
                SaveFileDialog1.Title = "Save"
                SaveFileDialog1.FileName = ExtractPreData(gIPE_File.In_Title, ".") & "_" & _
                                           .MCrossSecNo & ".DXF"

                If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                    Dim pFileName As String
                    pFileName = SaveFileDialog1.FileName
                    gIPE_File.In_Name = pFileName
                    .Create_DXF(pFileName)

                    Me.Cursor = Cursors.Default
                    '....Message.                   
                    Dim pMsg As String
                    pMsg = pFileName & " file has been created successfully. "
                    MessageBox.Show(pMsg, "Information", MessageBoxButtons.OK,
                                                         MessageBoxIcon.Information)
                End If

            End With

            Me.Cursor = Cursors.Default
        End If

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==================================================================
        Dim pCmdBtn As Button = CType(sender, Button)

        If pCmdBtn.Name = "cmdOK" Then
            If CheckBetaAng() Then
                If SaveData() = False Then Exit Sub
            Else
                Return
            End If

        End If

        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then
            If Not gIPE_frmNomenclature_DesignCenter.FormClose Then
                gIPE_frmNomenclature_DesignCenter.Close()
            End If
        End If

        Me.Close()

    End Sub


    Private Function CheckBetaAng() As Boolean
        '======================================     
        Dim pbln As Boolean = True

        If (mUSeal.Theta(2) - mUSeal.Beta(1)) < gcEPS Then
            Dim pstrMsg As String
            pstrMsg = "The Current value of Theta2 makes the edge horizontal, " & _
                      "which is not supported in the existing FE model." & vbLf & _
                      "Please increase Theta2 by atleast 1 degree."

            MessageBox.Show(pstrMsg, "Input Data Validation : Warning", _
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)

            txtTheta2_New.Focus()
            pbln = False
        End If

        Return pbln

    End Function


    Private Function SaveData() As Boolean
        '===================================    
        '....Local form data ===> Global Seal Object Data.

        If Check_LocalSealObj() = True Then

            '....LocalSeal object is same as either gIPE_SealOrg or gSeal
            '
            Return True

        Else
            Dim pblnSave As Boolean, pblnAddRecToNewDB As Boolean
            Check_CrossSecNo(pblnSave, pblnAddRecToNewDB)

            If pblnSave = False Then
                Return False


            ElseIf pblnSave = True Then

                '....SAVE DATA:
                Dim pCrossSec As String = UCase(Trim(txtCrossSecNo_New.Text))

                With mUSeal 'CType(gSeal, IPE_clsUSeal)
                    .MCrossSecNo = pCrossSec
                    .CrossSecNoOrg = gIPE_SealOrg.MCrossSecNo

                    If pblnAddRecToNewDB = True Then
                        '....Add the corresponding record to USeal New DB and also write 
                        '........the text file.

                        .AddRecToUSealNewDB(gIPE_File, gIPE_Project, gIPE_User, gIPE_Unit, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity)

                    End If
                End With

                If IPE_clsUSeal.CrossSecNewList.Count > 0 And _
                     Not IPE_clsUSeal.CrossSecNewList.Contains(mUSeal.MCrossSecNo) Then
                    IPE_clsUSeal.CrossSecNewList.Add(mUSeal.MCrossSecNo)
                End If

                With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal)
                    .MCrossSecNo = pCrossSec

                    '....Conv. Arc                                             
                    .Theta(1) = mUSeal.Theta(1)
                    .DTheta(1) = .Theta(1) - .ThetaStd(1)

                    '....Sealing Surface Arc.
                    .Theta(2) = mUSeal.Theta(2)
                    .DTheta(2) = .Theta(2) - .ThetaStd(2)

                    '....Conv R
                    .R(1) = gIPE_Unit.L_UserToCon(txtRad1_New.Text)
                    If Abs(gIPE_Unit.L_UserToCon(txtRad1_New.Text) - mUSeal.RStd(1)) > gcEPS Then
                        .DRad(1) = .R(1) - .RStd(1)
                    Else
                        .DRad(1) = 0.0F
                    End If

                    '....Sealing Surface R
                    .R(2) = gIPE_Unit.L_UserToCon(txtRad2_New.Text)
                    If Abs(gIPE_Unit.L_UserToCon(txtRad2_New.Text) - mUSeal.RStd(2)) > gcEPS Then
                        .DRad(2) = .R(2) - .RStd(2)
                    Else
                        .DRad(2) = 0.0F
                    End If

                    Dim pLLegStd As Single
                    If mUSeal.UnitSystem = "English" Then
                        pLLegStd = Round(mUSeal.LLegStd, 3)
                    ElseIf mUSeal.UnitSystem = "Metric" Then
                        pLLegStd = mUSeal.LLegStd
                    End If

                    '....Leg Length
                    .LLeg = gIPE_Unit.L_UserToCon(txtLLeg_New.Text)
                    If Abs(gIPE_Unit.L_UserToCon(txtLLeg_New.Text) - pLLegStd) > gcEPS Then
                        .DLLeg = gIPE_Unit.L_UserToCon(txtLLeg_New.Text) - pLLegStd
                    Else
                        .DLLeg = 0.0F
                    End If

                    .T = gIPE_Unit.L_UserToCon(txtT_New.Text)

                End With

                Return True

            End If
        End If

    End Function


    Private Sub Check_CrossSecNo(ByRef blnSave_Out As Boolean, _
                                ByRef blnAddRecToNewDB_Out As Boolean)
        '=================================================================

        '....Initialize.
        blnSave_Out = False
        blnAddRecToNewDB_Out = False


        '....Check the new cross-section number before attempting to save data.
        Dim pstrTitle As String
        Dim pstrPrompt As String
        Dim pintAnswer As Integer

        Dim pCrossSecNo_New As String
        pCrossSecNo_New = UCase(txtCrossSecNo_New.Text)


        '....Determine the appropriate Case No:
        Dim iCase As Int16

        If pCrossSecNo_New = "" Then
            '-----------------------
            iCase = 1

        ElseIf pCrossSecNo_New = CType(gIPE_SealOrg, IPE_clsUSeal).MCrossSecNo Then
            '-------------------------------------------------------------           
            iCase = 2                       '....No data changed.

        ElseIf IPE_clsUSeal.CrossSecList.Contains(pCrossSecNo_New) Then
            '-------------------------------------------------------
            iCase = 2

        ElseIf pCrossSecNo_New = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).MCrossSecNo Then
            '----------------------------------------------------------           
            iCase = 3


        ElseIf IPE_clsUSeal.CrossSecNewList.Contains(pCrossSecNo_New) Then
            '---------------------------------------------------------
            iCase = 3

        Else

            iCase = 0

        End If


        Select Case iCase

            Case 0
                blnSave_Out = True
                blnAddRecToNewDB_Out = True       '.... data changed. Not conflict with any case
                '                                 '.... add record to the new DB and save data to gseal.

            Case 1
                blnSave_Out = False

                pstrTitle = "Data Saving Error"
                pstrPrompt = "Should the new design data be saved, " & vbCrLf & _
                             "the NEW CrossSection No. must not be blank." & vbCrLf & _
                             "Please assign an appropriate number."

                pintAnswer = MessageBox.Show(pstrPrompt, pstrTitle, _
                                             MessageBoxButtons.OK, _
                                             MessageBoxIcon.Error)

                If pintAnswer = Windows.Forms.DialogResult.OK Then
                    txtCrossSecNo_New.Focus()
                    Exit Sub
                End If


            Case 2
                blnSave_Out = False

                pstrTitle = "Data Saving Error"
                pstrPrompt = "Should the new design data be saved, " & vbCrLf & _
                     "the NEW CrossSection No. must not be same as the original no." & _
                     vbCrLf & "Please assign an appropriate number."

                pintAnswer = MessageBox.Show(pstrPrompt, pstrTitle, _
                                             MessageBoxButtons.OK, _
                                             MessageBoxIcon.Error)

                If pintAnswer = Windows.Forms.DialogResult.OK Then
                    txtCrossSecNo_New.Focus()
                    Exit Sub
                End If

            Case 3
                pstrTitle = "Data Saving Warning"
                pstrPrompt = "The NEW CrossSection No. exists in the 'SealNewDB' database. " & vbCrLf & _
                             "Do you want to overwrite the existing data? "
                pintAnswer = MessageBox.Show(pstrPrompt, pstrTitle, _
                                             MessageBoxButtons.YesNo, _
                                             MessageBoxIcon.Warning)

                If pintAnswer = Windows.Forms.DialogResult.Yes Then
                    '....Delete the corresponding record from the USeal New DB and 
                    '........the text file.
                    IPE_clsUSeal.DeleteNewUSealRec(pCrossSecNo_New, gIPE_File)
                    blnSave_Out = True
                    blnAddRecToNewDB_Out = True

                ElseIf pintAnswer = Windows.Forms.DialogResult.No Then
                    blnSave_Out = False
                    txtCrossSecNo_New.Focus()
                    Exit Sub
                End If

        End Select

    End Sub

    Function Check_LocalSealObj() As Boolean
        '====================================       
        '....This routine compares mESeal with gIPE_SealOrg and gSeal
        '
        Dim pDataChanged_SealOrg As Integer
        Dim pDataChanged_Seal As Integer

        '....Holds the datachange number between mUSeal & gIPE_SealOrg
        pDataChanged_SealOrg = CType(gIPE_SealOrg, IPE_clsUSeal).Compare(mUSeal)

        '....Holds the datachange number between mUSeal & gSeal
        pDataChanged_Seal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).Compare(mUSeal)

        Dim pCrossSecNo_New As String
        pCrossSecNo_New = UCase(txtCrossSecNo_New.Text)

        If pDataChanged_SealOrg = 0 Then

            If pCrossSecNo_New <> gIPE_SealOrg.MCrossSecNo Then
                MessageBox.Show("Local Seal Object = Global Original Seal object" & vbCrLf & _
                                "The CrossSecNo New Should be " & gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo, "Error Message", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)

                txtCrossSecNo_New.Text = gIPE_SealOrg.MCrossSecNo.ToString()
                txtCrossSecNo_New.Refresh()

            End If

            '....Data Saved to gseal
            CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).MCrossSecNo = gIPE_SealOrg.MCrossSecNo
            Return True

        ElseIf pDataChanged_Seal = 0 Then

            If pCrossSecNo_New <> gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo Then
                MessageBox.Show("Local Seal Object = Global Seal object" & vbCrLf & _
                                "The CrossSecNo New Should be " & gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo, "Error Message", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)

                txtCrossSecNo_New.Text = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo.ToString()
                txtCrossSecNo_New.Refresh()

            End If

            '....No need for save any data
            Return True
        Else
            Return False

        End If
    End Function


#End Region


#Region "GRAPHICS ROUTINES:"
    '=======================

    Private Sub DoGraphics()
        '===================        
        'This routine draws the 'Standard' & 'Adjusted' Geometries.

        '....Drawing envelope:
        Dim EnvpTopL As PointF
        Dim EnvpBotR As PointF


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
        pDrawWid(1) = 2

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
        If gIPE_Unit.System = "English" Then
            pGr.PageUnit = GraphicsUnit.Inch

            '....# of Pixels/in
            pDpX = pGr.DpiX
            pDpY = pGr.DpiY

        ElseIf gIPE_Unit.System = "Metric" Then
            pGr.PageUnit = GraphicsUnit.Millimeter

            '....# of Pixels/mm
            pDpX = pGr.DpiX / gIPE_Unit.EngLToUserL(1.0)
            pDpY = pGr.DpiY / gIPE_Unit.EngLToUserL(1.0)
        End If


        '....Size of the graphics area in the "page unit" system.
        Dim pSize As New SizeF(picSeal.Width / pDpX, picSeal.Height / pDpY)

        ''....Draw both "Standard" & "Adjusted" Seal Geometry. 
        mUSeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                   "BOTH", "SCALE_BY_STD", 1.25, EnvpTopL, EnvpBotR)

        'Caption Labels:        
        '---------------
        If mUSeal Is Nothing = False Then

            '....ORIGINAL Values:
            '
            Dim pHFree_Org As Single
            Dim pWid_Org As Single

            With gIPE_SealOrg
                pHFree_Org = .HfreeStd
                pWid_Org = .WidStd
            End With

            With gIPE_Unit
                lblHFree_Org.Text = "Original   =   " & .WriteInUserL(pHFree_Org)
            End With


            '....NEW Values:
            '
            With mUSeal
                Dim pHFree_New As Single = .Hfree
                Dim pDHFree_PCent As Single
                pDHFree_PCent = ((pHFree_New - pHFree_Org) * 100 / pHFree_Org) 'AM 06AUG09

                If Abs(pDHFree_PCent) <= 0.0# Then
                    lblHFree_New.Visible = False

                ElseIf Abs(pDHFree_PCent) > 0.0# Then
                    lblHFree_New.Visible = True

                    Dim pstr_New As String = gIPE_Unit.WriteInUserL(pHFree_New)
                    Dim pstrD_PCent As String = Format(pDHFree_PCent, "##0.0")
                    Dim pstrTxt As String
                    pstrTxt = pstr_New & "  ( " & pstrD_PCent & " %)"

                    lblHFree_New.Text = "New         =   " & pstrTxt

                End If

            End With


            lblWid_New.Text = gIPE_Unit.WriteInUserL(mUSeal.Wid)

        End If

    End Sub

#End Region


End Class