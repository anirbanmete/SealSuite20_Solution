
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmAppCond                             '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  09FEB16                                '
'                                                                              '
'===============================================================================
'
Imports clsLibrary11
Imports System.Linq


Public Class IPE_frmOpCond
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mOpCond As IPE_clsOpCond         '....Local object.
    ''Private mProjectEntity As New ProjectDBEntities()

#End Region


#Region " Windows Form Designer generated code "

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents txtPDiff As System.Windows.Forms.TextBox
    Friend WithEvents cmbUnitUserPMetric As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSealPOrient As System.Windows.Forms.ComboBox
    Friend WithEvents txtTOper As System.Windows.Forms.TextBox
    Friend WithEvents lblUnitT As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label0 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblUnitUserPEng As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmOpCond))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label0 = New System.Windows.Forms.Label()
        Me.txtPDiff = New System.Windows.Forms.TextBox()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblUnitT = New System.Windows.Forms.Label()
        Me.lblUnitUserPEng = New System.Windows.Forms.Label()
        Me.cmbUnitUserPMetric = New System.Windows.Forms.ComboBox()
        Me.txtTOper = New System.Windows.Forms.TextBox()
        Me.cmbSealPOrient = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(190, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(14, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "T"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label0
        '
        Me.Label0.AutoSize = True
        Me.Label0.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label0.Location = New System.Drawing.Point(31, 23)
        Me.Label0.Name = "Label0"
        Me.Label0.Size = New System.Drawing.Size(34, 13)
        Me.Label0.TabIndex = 5
        Me.Label0.Text = "PDiff"
        Me.Label0.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPDiff
        '
        Me.txtPDiff.BackColor = System.Drawing.SystemColors.Window
        Me.txtPDiff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPDiff.Location = New System.Drawing.Point(18, 38)
        Me.txtPDiff.Name = "txtPDiff"
        Me.txtPDiff.Size = New System.Drawing.Size(60, 21)
        Me.txtPDiff.TabIndex = 6
        Me.txtPDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(428, 148)
        Me.lblBorder.TabIndex = 7
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(426, 146)
        Me.Panel1.TabIndex = 8
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtPDiff)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label0)
        Me.GroupBox1.Controls.Add(Me.lblUnitT)
        Me.GroupBox1.Controls.Add(Me.lblUnitUserPEng)
        Me.GroupBox1.Controls.Add(Me.cmbUnitUserPMetric)
        Me.GroupBox1.Controls.Add(Me.txtTOper)
        Me.GroupBox1.Controls.Add(Me.cmbSealPOrient)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 10)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(408, 79)
        Me.GroupBox1.TabIndex = 65
        Me.GroupBox1.TabStop = False
        '
        'lblUnitT
        '
        Me.lblUnitT.AutoSize = True
        Me.lblUnitT.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitT.Location = New System.Drawing.Point(230, 42)
        Me.lblUnitT.Name = "lblUnitT"
        Me.lblUnitT.Size = New System.Drawing.Size(19, 13)
        Me.lblUnitT.TabIndex = 23
        Me.lblUnitT.Text = "ºF"
        Me.lblUnitT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblUnitUserPEng
        '
        Me.lblUnitUserPEng.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitUserPEng.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitUserPEng.Location = New System.Drawing.Point(81, 39)
        Me.lblUnitUserPEng.Name = "lblUnitUserPEng"
        Me.lblUnitUserPEng.Size = New System.Drawing.Size(36, 16)
        Me.lblUnitUserPEng.TabIndex = 7
        Me.lblUnitUserPEng.Text = "psid"
        Me.lblUnitUserPEng.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbUnitUserPMetric
        '
        Me.cmbUnitUserPMetric.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUnitUserPMetric.DropDownWidth = 80
        Me.cmbUnitUserPMetric.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUnitUserPMetric.Location = New System.Drawing.Point(81, 15)
        Me.cmbUnitUserPMetric.Name = "cmbUnitUserPMetric"
        Me.cmbUnitUserPMetric.Size = New System.Drawing.Size(70, 21)
        Me.cmbUnitUserPMetric.TabIndex = 13
        '
        'txtTOper
        '
        Me.txtTOper.BackColor = System.Drawing.SystemColors.Window
        Me.txtTOper.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTOper.Location = New System.Drawing.Point(167, 38)
        Me.txtTOper.Name = "txtTOper"
        Me.txtTOper.Size = New System.Drawing.Size(60, 21)
        Me.txtTOper.TabIndex = 14
        Me.txtTOper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbSealPOrient
        '
        Me.cmbSealPOrient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSealPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSealPOrient.Location = New System.Drawing.Point(316, 39)
        Me.cmbSealPOrient.Name = "cmbSealPOrient"
        Me.cmbSealPOrient.Size = New System.Drawing.Size(72, 21)
        Me.cmbSealPOrient.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(317, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Orientation"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(266, 110)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 21
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(344, 110)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 22
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        Me.ErrorProvider1.DataMember = ""
        '
        'IPE_frmOpCond
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(434, 154)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmOpCond"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Operating Conditions"
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New()
        '===========

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        'Populate Combo Boxes:
        '--------------------
        '....Pressure Unit.
        With cmbUnitUserPMetric.Items
            .Clear()
            .Add("Bar")
            .Add("kPa")
            .Add("kg/cm" & Chr(94) & "2")
        End With

        '....Pressure Orientation.
        With cmbSealPOrient.Items
            .Clear()
            .Add("External")
            .Add("Internal")
        End With

    End Sub

#Region "FORM EVENT ROUTINES:"

    Private Sub frmAppCond_Load(ByVal sender As Object, _
                                ByVal e As System.EventArgs) _
                                Handles MyBase.Load
        '======================================================

        InitializeControls(gIPE_Unit.System, gIPE_Unit.UserP, gIPE_Unit.UserL, gIPE_Unit.T)

        InitializeLocalObject()

        DisplayData()

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            txtPDiff.Enabled = False
            txtTOper.Enabled = False
            cmbSealPOrient.Enabled = False
            cmbUnitUserPMetric.Enabled = False
        Else
            txtPDiff.Enabled = True
            txtTOper.Enabled = True
            cmbSealPOrient.Enabled = True
            cmbUnitUserPMetric.Enabled = True
        End If

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeControls(ByVal unitSystem_In As String, _
                                   ByVal unitUserP_In As String, _
                                   ByVal unitUserL_In As String, ByVal unitT_In As String)
        '=================================================================================

        If unitSystem_In = "English" Then
            '----------------------------
            cmbUnitUserPMetric.Visible = False

            With lblUnitUserPEng
                .Visible = True
                '.Text = unitUserP_In
            End With


        ElseIf unitSystem_In = "Metric" Then
            '-------------------------------
            lblUnitUserPEng.Visible = False

            With cmbUnitUserPMetric
                .Visible = True
                .Top = lblUnitUserPEng.Top
                .Left = lblUnitUserPEng.Left
            End With

            If unitUserP_In <> "" Then
                cmbUnitUserPMetric.Text = unitUserP_In
            Else
                cmbUnitUserPMetric.SelectedIndex = 0
            End If

        End If

        lblUnitT.Text = unitT_In
        'lblUnitUserL.Text = unitUserL_In

        'Clear Input Data Boxes:
        '-----------------------
        '....Clears the input text boxes that contain unit-dependent values and
        '........the labels that contain calculated values based on the input data.
        txtPDiff.Text = ""
        txtTOper.Text = ""


    End Sub


    Private Sub InitializeLocalObject()
        '==============================
        '....Instantiate Local Cavity Object. 
        mOpCond = New IPE_clsOpCond()

        With mOpCond
            .PDiff = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.PDiff
            .TOper = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.TOper
            .POrient = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient
        End With

    End Sub


    Private Sub DisplayData()
        '====================
        '....Data is displayed in user unit. Hence, conversion is necessary when appropriate.
        txtPDiff.Text = gIPE_Unit.FormatPDiffUnitUser(mOpCond.PDiff)           '....Pressure.
        If mOpCond.TOper <> 0 Then txtTOper.Text = NInt(mOpCond.TOper) '  '....Temperature.

        'Pressure Orientation.
        '---------------------
        If mOpCond.POrient <> "" Then
            cmbSealPOrient.Text = mOpCond.POrient.Trim()
        Else
            cmbSealPOrient.SelectedIndex = 0
        End If

    End Sub

#End Region


#End Region


#Region " CONTROL EVENT ROUTINES:"

    Private Sub txtBox_TextChanged(sender As System.Object, e As System.EventArgs) _
                                        Handles txtPDiff.TextChanged, txtTOper.TextChanged
        '==================================================================================================
        Dim pTxtBox As TextBox = CType(sender, TextBox)


        Select Case pTxtBox.Name

            Case "txtPDiff"
                '------------------------
                mOpCond.PDiff = ConvertToSng(txtPDiff.Text) * gIPE_Unit.CFacUserP

            Case "txtTOper"
                '------------------------
                mOpCond.TOper = Val(txtTOper.Text)
        End Select

    End Sub

    Private Sub txtPDiff_KeyPress(ByVal sender As System.Object, _
                                    ByVal e As KeyPressEventArgs) _
                                    Handles txtPDiff.KeyPress
        '============================================================
        Dim pCulture = gIPE_Project.CultureName

        Select Case pCulture
            Case "USA", "UK"
                If e.KeyChar = "," Then e.KeyChar = "."
            Case "Germany", "France"
                If e.KeyChar = "." Then e.KeyChar = ","
        End Select

    End Sub

    Private Sub cmbSealPOrient_SelectedIndexChanged(sender As System.Object, _
                                                    e As System.EventArgs) Handles cmbSealPOrient.SelectedIndexChanged
        '==============================================================================================================
        mOpCond.POrient = cmbSealPOrient.Text
    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '===========================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
            Me.Hide()
            'gfrmAppliedLoading.ShowDialog()

        Else
            Me.Close()
        End If

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '=================
        '....The data are saved to the dataset in consistent units.

        'PRIMARY ASSIGNMENTS:
        '--------------------
        If gIPE_Unit.System = "Metric" Then _
            gIPE_Unit.UserP = cmbUnitUserPMetric.Text

        With gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond
            .PDiff = mOpCond.PDiff
            .TOper = mOpCond.TOper

        End With

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient = mOpCond.POrient

        'SECONDARY ASSIGNMENTS:
        '----------------------

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal Is Nothing = False Then _
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.POrient = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.TOper = mOpCond.TOper     'AES 14MAR17

        If gIPE_SealOrg Is Nothing = False Then _
        gIPE_SealOrg.POrient = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient

    End Sub

#End Region

#End Region


End Class
