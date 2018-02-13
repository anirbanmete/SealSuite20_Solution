'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  frmNonStdCS_ESeal                      '
'                        VERSION NO  :  1.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  11OCT17                                '
'                                                                              '
'===============================================================================
Imports System.Math
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports clsLibrary11

Public Class Part_frmNonStdCS_ESeal

#Region "MEMBER VARIABLES:"

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single

    'Private mHW As New clsPartProject.clsPNR.clsHW(gPartProject.PNR)
    Private mPartProject As New clsPartProject
    Private mESeal As IPE_clsESeal        '....Local Seal object.  

#End Region

#Region "FORM EVENT ROUTINES:"

    Public Sub New(ByRef PartProject_In As clsPartProject)
        '================================
        ' This call is required by the designer.
        InitializeComponent()

        'mHW = HW_In.Clone()
        mPartProject = PartProject_In
    End Sub

    Private Sub frmNonStdCS_ESeal_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        '=================================================================================================
        'If Not gfrmNomenclature_AdjGeom Is Nothing Then
        '    cmdViewNomenclature.Enabled = gfrmNomenclature_AdjGeom.FormClose
        'End If
    End Sub

    Private Sub frmNonStdCS_ESeal_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==================================================================================================
        'MsgBox("frmAdjGeomESeal_Load")        '....Diagnostic statement.


        '   --------------
        '   Picture Box   '                 
        '   --------------
        '
        '....Set Margin of the Picture Box.
        '   
        Dim pUniformMargin As Single = 0.4       '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        '....Initialize the Local Seal Object.
        InitializeLocalObject()                 '....gSeal ===> mESeal

        '....Set the "Maximum", "Minimum" & "Increment" properties of the UpDown buttons 
        '........assign the local object's value.
        '
        SetUpDown_DThetaE1()                    '....DThetaE1
        SetUpDown_DThetaM1()                    '....DThetaM1

        InitializeControls()

        lblTemplate.Left = 191
        txtESealTemplateNo.Left = 196

        '....Display data on the form.
        '........Local seal object "mESeal" (which is recently initialized above) is used.
        DisplayData()

        '....Display graphics on the picture box.
        DoGraphics()
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '==============================
        '....From gSeal ===> mESeal. 
        '........Now onwards, mESeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal in the 
        '........"SaveData" routine which is called when the form is exited and another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local Seal Object.
        mESeal = New IPE_clsESeal("E-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
        mESeal.MCrossSecNo = mPartProject.PNR.HW.MCrossSecNo

    End Sub


    Private Sub SetUpDown_DThetaE1()
        '===========================
        '....This routine the sets the "Minimum", "Maximum" & "Increment" properties 
        '........of the UpDown button for the "DThetaE1".

        '....Initialize and make an attempt to assign the current object's adjustment value.
        With mESeal
            Initialize_UpDownButton(updDThetaE1)
            Assign_UpDownButtonValue(updDThetaE1, .DThetaE1)
        End With

    End Sub


    Private Sub SetUpDown_DThetaM1()
        '===========================
        '....This routine the sets the "Minimum", "Maximum" & "Increment" properties 
        '........of the UpDown button for the "DThetaM1".

        '....Initialize and make an attempt to assign the current object's value.
        With mESeal
            Initialize_UpDownButton(updDThetaM1)
            Assign_UpDownButtonValue(updDThetaM1, .DThetaM1)
        End With

    End Sub


    Private Sub Initialize_UpDownButton(ByRef updown_In As NumericUpDown)
        '================================================================

        'Initialize Numeric Up Down Controls:  (Unit-Independent)
        '-----------------------------------
        Dim pMinVal As Integer
        Dim pMaxVal As Integer
        Dim pIncrementVal As Single

        pMinVal = -15
        pMaxVal = 15
        pIncrementVal = 0.1

        With updown_In
            .Minimum = pMinVal
            .Maximum = pMaxVal
            .Increment = pIncrementVal
            .Refresh()
        End With

    End Sub

    Private Sub Assign_UpDownButtonValue(ByRef updown_In As NumericUpDown, _
                                         ByVal value_In As Single)
        '====================================================================

        Dim pVal As Decimal = CDec(value_In)

        If pVal >= updown_In.Minimum And pVal <= updown_In.Maximum Then
            '....Value acceptable.    

            If Abs(updown_In.Value - pVal) > gcEPS Then _
                   updown_In.Value = pVal


        ElseIf pVal < updown_In.Minimum Then
            updown_In.Value = updown_In.Minimum

        ElseIf pVal > updown_In.Maximum Then
            updown_In.Value = updown_In.Maximum
        End If

    End Sub


    Private Sub InitializeControls()
        '===========================

        txtESealThetaE1.Text = "Baseline"
        txtESealThetaM1.Text = "Baseline"

        With lblESealThetaE1
            '.Text = "Free Height"
            .TextAlign = ContentAlignment.MiddleRight
        End With

        With lblESealThetaM1
            '.Text = "Heel"
            .TextAlign = ContentAlignment.MiddleRight
        End With

        ''If (gfrmAnalysisSet.ModeOper = frmAnalysisSet.eModeOper.None) Then
        ''    txtESealDThetaE1.Enabled = False
        ''    updDThetaE1.Enabled = False

        ''    txtESealDThetaM1.Enabled = False
        ''    updDThetaM1.Enabled = False
        ''Else
        ''    txtESealDThetaE1.Enabled = True
        ''    updDThetaE1.Enabled = True

        ''    txtESealDThetaM1.Enabled = True
        ''    updDThetaM1.Enabled = True
        ''End If

    End Sub


    Private Sub DisplayData()
        '====================
        '....This routine displays the current state of the local object. 

        '....Use the local seal object.
        With mESeal
            txtCrossSecNo.Text = .MCrossSecNo
            'txtESealStageNo.Text = .StageNo
            txtESealTemplateNo.Text = .TemplateNo

            txtPOrient.Text = .POrient
            txtDControl.Text = gUnit.WriteInUserL(.DControl)
            txtESealNConv.Text = .NConv
            txtT.Text = gUnit.WriteInUserL(.T)

            'AES 11OCT17
            ' ''....Adjusted parameter values:
            ''txtESealDThetaE1.Text = Format(.DThetaE1, "##0.000")
            ''txtESealDThetaM1.Text = Format(.DThetaM1, "##0.000")

            '....Set Fore color & Back color.       
            txtCrossSecNo.BackColor = Color.Gainsboro

            txtESealTemplateNo.ForeColor = Color.Magenta
            txtESealTemplateNo.BackColor = Color.Gainsboro

            txtPOrient.BackColor = Color.Gainsboro

            txtDControl.ForeColor = Color.Blue
            txtDControl.BackColor = Color.Gainsboro

            txtESealNConv.ForeColor = Color.Magenta
            txtESealNConv.BackColor = Color.Gainsboro

            txtT.ForeColor = Color.Magenta
            txtT.BackColor = Color.Gainsboro

            txtESealThetaE1.BackColor = Color.Gainsboro
            txtESealThetaM1.BackColor = Color.Gainsboro


        End With

        With mPartProject.PNR.HW
            txtESealDThetaE1.Text = Format(.DThetaE1, "##0.000")
            txtESealDThetaM1.Text = Format(.DThetaM1, "##0.000")
        End With

    End Sub


#End Region

#End Region


#Region "CONTROL EVENT ROUTINES:"

#Region "UP-DOWN BUTTON SETTING ROUTINES:"

    Private Sub updDThetaE1_Click(ByVal sender As Object, _
                                  ByVal e As System.EventArgs) _
                                  Handles updDThetaE1.Click
        '=======================================================
        'MsgBox("updDThetaE1 Click")
        ' txtESealDThetaE1.Text = Format(updDThetaE1.Value, "#0.000")
    End Sub


    Private Sub updDThetaE1_ValueChanged(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) _
                                         Handles updDThetaE1.ValueChanged
        '==================================================================
        Dim pDThetaE1 As Single = updDThetaE1.Value
        txtESealDThetaE1.Text = ConvertToStr(pDThetaE1, "#0.000")

    End Sub


    Private Sub updDThetaM1_Click(ByVal sender As Object, _
                                  ByVal e As System.EventArgs) _
                                  Handles updDThetaM1.Click
        '========================================================

        'txtESealDThetaM1.Text = Format(updDThetaM1.Value, "#0.000")
    End Sub


    Private Sub updDThetaM1_ValueChanged(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) _
                                         Handles updDThetaM1.ValueChanged
        '==================================================================
        'txtESealDThetaM1.Text = Format(updDThetaM1.Value, "#0.000")

        Dim pDThetaM1 As Single = updDThetaM1.Value
        txtESealDThetaM1.Text = ConvertToStr(pDThetaM1, "#0.000")

    End Sub

#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtESealDThetaE1_TextChanged(ByVal eventSender As System.Object, _
                                             ByVal eventArgs As System.EventArgs) _
                                             Handles txtESealDThetaE1.TextChanged
        '================================================================================
        If mESeal Is Nothing = True Then Exit Sub

        '--------------------------------------------------------------------------------

        'Check if the entered value of the "DThetaE1" exceeds the preset hard limits.
        '--------------------------------------------------------------------------------
        Const pcDThetaE1Max_HardLim As Single = 30
        Const pcDThetaE1Min_HardLim As Single = -30

        Dim pESealDThetaE1 As String = txtESealDThetaE1.Text
        Dim pDThetaE1_Entered As Single = ConvertToSng(pESealDThetaE1)

        Dim pstrMsg As String

        If pDThetaE1_Entered > pcDThetaE1Max_HardLim Then
            pstrMsg = "DThetaE1 can't be higher than " & pcDThetaE1Max_HardLim
            MessageBox.Show(pstrMsg, "DThetaE1 Error", MessageBoxButtons.OK, _
                                                       MessageBoxIcon.Error)
            '....Reset the text box display.
            txtESealDThetaE1.Text = pcDThetaE1Max_HardLim


        ElseIf pDThetaE1_Entered < pcDThetaE1Min_HardLim Then
            pstrMsg = "DThetaE1 can't be lower than " & pcDThetaE1Min_HardLim
            MessageBox.Show(pstrMsg, "DThetaE1 Error", MessageBoxButtons.OK, _
                                                       MessageBoxIcon.Error)

            '....Reset the text box display.
            txtESealDThetaE1.Text = pcDThetaE1Min_HardLim
        End If

        Dim pDThetaE1 As Single = ConvertToSng(txtESealDThetaE1.Text)
        Assign_UpDownButtonValue(updDThetaE1, pDThetaE1)

        '....Assign the DThetaE1 value to the local seal object property,

        With mESeal
            .DThetaE1 = ConvertToSng(txtESealDThetaE1.Text)
            txtDControl.Text = gUnit.WriteInUserL(.DControl)    '....Update display.
        End With

        '....Redraw Seal Geometries. 
        DoGraphics()

    End Sub


    Private Sub txtESealDThetaM1_TextChanged(ByVal eventSender As System.Object, _
                                             ByVal eventArgs As System.EventArgs) _
                                             Handles txtESealDThetaM1.TextChanged
        '=============================================================================

        If mESeal Is Nothing = True Then Exit Sub

        '--------------------------------------------------------------------------------

        Const pcDThetaM1Max_HardLim As Single = 30
        Const pcDThetaM1Min_HardLim As Single = -30

        Dim pESealDThetaM1 As String = txtESealDThetaE1.Text
        Dim pDThetaM1_Entered As Single = ConvertToSng(pESealDThetaM1)

        Dim pstrMsg As String

        If pDThetaM1_Entered > pcDThetaM1Max_HardLim Then
            pstrMsg = "DThetaM1 can't be higher than " & pcDThetaM1Max_HardLim
            MessageBox.Show(pstrMsg, "DThetaM1 Error", MessageBoxButtons.OK, _
                                                       MessageBoxIcon.Error)
            '....Reset the text box display.
            txtESealDThetaM1.Text = pcDThetaM1Max_HardLim


        ElseIf pDThetaM1_Entered < pcDThetaM1Min_HardLim Then
            pstrMsg = "DThetaM1 can't be lower than " & pcDThetaM1Min_HardLim
            MessageBox.Show(pstrMsg, "DThetaM1 Error", MessageBoxButtons.OK, _
                                                       MessageBoxIcon.Error)

            '....Reset the text box display.
            txtESealDThetaM1.Text = pcDThetaM1Min_HardLim
        End If

        Dim pDThetaM1 As Single = ConvertToSng(txtESealDThetaM1.Text)
        Assign_UpDownButtonValue(updDThetaM1, pDThetaM1)

        '....Assign the DThetaM1 value to the local seal object property,
        With mESeal
            .DThetaM1 = ConvertToSng(txtESealDThetaM1.Text)
            txtDControl.Text = gUnit.WriteInUserL(.DControl)    '....Update display.
        End With


        '....Redraw Seal Geometries.
        'txtESealDThetaM3.Text = txtESealDThetaM1.Text
        DoGraphics()                '....Draw Seal Geometries.


    End Sub


    Private Sub TxtBox_KeyPress(ByVal sender As System.Object, _
                                     ByVal e As KeyPressEventArgs) _
                                     Handles txtESealDThetaE1.KeyPress, txtESealDThetaM1.KeyPress
        '============================================================
        'Dim pCulture = gUserInfo.CultureName
        ''Dim pCulture = gPartProject.CultureName

        ''Select Case pCulture
        ''    Case "USA", "UK"
        ''        If e.KeyChar = "," Then e.KeyChar = "."
        ''    Case "Germany", "France"
        ''        If e.KeyChar = "." Then e.KeyChar = ","
        ''End Select

    End Sub


#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdViewNomenclature_Click(ByVal sender As System.Object, _
                             ByVal e As System.EventArgs) _
                             Handles cmdViewNomenclature.Click
        '===================================================================
        gfrmNomenclature_NonStd_AdjGeom = New Part_frmNomenclature_NonStd_AdjGeom(mPartProject)
        gfrmNomenclature_NonStd_AdjGeom.Show()

    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==============================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()

        Me.Close()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '=================
        '....Local form data ===> Global Seal Object Data.

        With mPartProject.PNR.HW
            .DThetaE1 = ConvertToSng(txtESealDThetaE1.Text)
            .DThetaM1 = ConvertToSng(txtESealDThetaM1.Text)

        End With
    End Sub

#End Region

#End Region

#End Region


#Region "GRAPHICS ROUTINES:"

    Private Sub DoGraphics()
        '===================

        'This routine draws the 'Standard' & 'Adjusted' Geometries.

        '....Drawing envelope:
        Dim EnvpTopL As PointF  'SB 13DEC07
        Dim EnvpBotR As PointF  'SB 13DEC07


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
        pDrawWid(1) = 2     '....Width = 1 here doesn't work, nor necessary here 04JUL06.

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
        mESeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                    "BOTH", "SCALE_BY_STD", 1.25, _
                                    EnvpTopL, EnvpBotR)      'SB 13DEC07


        'Caption Labels:        
        '---------------
        If mESeal Is Nothing = False Then
            lblStandard.Text = "Standard  =  " & gUnit.WriteInUserL((mESeal.HfreeStd))

            Dim psngDelHfreePCent As Single
            psngDelHfreePCent = (mESeal.Hfree - mESeal.HfreeStd) * 100 / mESeal.HfreeStd

            If Abs(psngDelHfreePCent) <= 0.0# Then
                lblESealAdjusted.Visible = False

            ElseIf Abs(psngDelHfreePCent) > 0.0# Then
                lblESealAdjusted.Visible = True
                lblESealAdjusted.Text = "Adjusted   =  " & _
                                    gUnit.WriteInUserL((mESeal.Hfree)) & _
                                    "  ( " & Format(psngDelHfreePCent, "##0.0") & " %)"

            End If

        End If

    End Sub


#End Region



End Class