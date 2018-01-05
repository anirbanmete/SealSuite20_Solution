'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                       FORM MODULE  :  frmFEA                                 '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  20MAR17                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports SealIPELib = SealIPELib101

Public Class IPE_frmFEA
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        '=================================================================
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
    Private mfrmFEAParams As IPE_frmFEAParams
    Private mfrmFEAParamsESeal As IPE_frmFEAParamsESeal
    Private mfrmFEAParamsCSeal As IPE_frmFEAParamsCSeal
    Friend WithEvents cmdLoadStep As System.Windows.Forms.Button
    Friend WithEvents grpLoadCase As System.Windows.Forms.GroupBox
    Friend WithEvents optAdditional As System.Windows.Forms.RadioButton
    Friend WithEvents optBaseline As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbCompressTolType As System.Windows.Forms.ComboBox
    Public WithEvents txtAnalysysName As System.Windows.Forms.TextBox
    Public WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
    Private mfrmFEAParamsUSeal As IPE_frmFEAParamsUSeal
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdRunANSYS As System.Windows.Forms.Button
    Friend WithEvents cmdFEAGraphics As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents mnuFEA As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuParameters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGeneral As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSeal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpGroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdFatigueData As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkFatigueData As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents optLinearElastic As System.Windows.Forms.RadioButton
    Friend WithEvents optElastoPlastic As System.Windows.Forms.RadioButton
    Friend WithEvents tpFatigue As System.Windows.Forms.ToolTip

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmFEA))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.optLinearElastic = New System.Windows.Forms.RadioButton()
        Me.optElastoPlastic = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkFatigueData = New System.Windows.Forms.CheckBox()
        Me.cmdLoadStep = New System.Windows.Forms.Button()
        Me.cmdRunANSYS = New System.Windows.Forms.Button()
        Me.grpGroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmdFatigueData = New System.Windows.Forms.Button()
        Me.cmdFEAGraphics = New System.Windows.Forms.Button()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.grpLoadCase = New System.Windows.Forms.GroupBox()
        Me.optAdditional = New System.Windows.Forms.RadioButton()
        Me.optBaseline = New System.Windows.Forms.RadioButton()
        Me.txtAnalysysName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbCompressTolType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.mnuFEA = New System.Windows.Forms.MenuStrip()
        Me.mnuParameters = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGeneral = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeal = New System.Windows.Forms.ToolStripMenuItem()
        Me.tpFatigue = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpGroupBox2.SuspendLayout()
        Me.grpGroupBox1.SuspendLayout()
        Me.grpLoadCase.SuspendLayout()
        Me.mnuFEA.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(1, 24)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(387, 442)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.grpGroupBox2)
        Me.Panel1.Controls.Add(Me.grpGroupBox1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Location = New System.Drawing.Point(2, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(385, 440)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.optLinearElastic)
        Me.GroupBox2.Controls.Add(Me.optElastoPlastic)
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(10, 165)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(364, 55)
        Me.GroupBox2.TabIndex = 187
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Material Model:"
        '
        'optLinearElastic
        '
        Me.optLinearElastic.AutoSize = True
        Me.optLinearElastic.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optLinearElastic.Location = New System.Drawing.Point(141, 25)
        Me.optLinearElastic.Name = "optLinearElastic"
        Me.optLinearElastic.Size = New System.Drawing.Size(100, 17)
        Me.optLinearElastic.TabIndex = 13
        Me.optLinearElastic.Text = "Linear Elastic"
        Me.optLinearElastic.UseVisualStyleBackColor = True
        '
        'optElastoPlastic
        '
        Me.optElastoPlastic.AutoSize = True
        Me.optElastoPlastic.Checked = True
        Me.optElastoPlastic.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optElastoPlastic.Location = New System.Drawing.Point(20, 25)
        Me.optElastoPlastic.Name = "optElastoPlastic"
        Me.optElastoPlastic.Size = New System.Drawing.Size(100, 17)
        Me.optElastoPlastic.TabIndex = 12
        Me.optElastoPlastic.TabStop = True
        Me.optElastoPlastic.Text = "Elasto-Plastic"
        Me.optElastoPlastic.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkFatigueData)
        Me.GroupBox1.Controls.Add(Me.cmdLoadStep)
        Me.GroupBox1.Controls.Add(Me.cmdRunANSYS)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 226)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(364, 80)
        Me.GroupBox1.TabIndex = 186
        Me.GroupBox1.TabStop = False
        '
        'chkFatigueData
        '
        Me.chkFatigueData.AutoSize = True
        Me.chkFatigueData.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFatigueData.Location = New System.Drawing.Point(18, 15)
        Me.chkFatigueData.Name = "chkFatigueData"
        Me.chkFatigueData.Size = New System.Drawing.Size(155, 17)
        Me.chkFatigueData.TabIndex = 187
        Me.chkFatigueData.Text = "Generate Fatigue Data"
        Me.chkFatigueData.UseVisualStyleBackColor = True
        '
        'cmdLoadStep
        '
        Me.cmdLoadStep.BackColor = System.Drawing.Color.Silver
        Me.cmdLoadStep.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.cmdLoadStep.Image = CType(resources.GetObject("cmdLoadStep.Image"), System.Drawing.Image)
        Me.cmdLoadStep.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdLoadStep.Location = New System.Drawing.Point(14, 39)
        Me.cmdLoadStep.Name = "cmdLoadStep"
        Me.cmdLoadStep.Size = New System.Drawing.Size(110, 30)
        Me.cmdLoadStep.TabIndex = 74
        Me.cmdLoadStep.Text = "    Load Steps"
        Me.cmdLoadStep.UseVisualStyleBackColor = False
        '
        'cmdRunANSYS
        '
        Me.cmdRunANSYS.BackColor = System.Drawing.Color.Silver
        Me.cmdRunANSYS.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.cmdRunANSYS.Image = CType(resources.GetObject("cmdRunANSYS.Image"), System.Drawing.Image)
        Me.cmdRunANSYS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdRunANSYS.Location = New System.Drawing.Point(237, 39)
        Me.cmdRunANSYS.Name = "cmdRunANSYS"
        Me.cmdRunANSYS.Size = New System.Drawing.Size(110, 30)
        Me.cmdRunANSYS.TabIndex = 70
        Me.cmdRunANSYS.Text = "    Run &ANSYS"
        Me.cmdRunANSYS.UseVisualStyleBackColor = False
        '
        'grpGroupBox2
        '
        Me.grpGroupBox2.Controls.Add(Me.cmdFatigueData)
        Me.grpGroupBox2.Controls.Add(Me.cmdFEAGraphics)
        Me.grpGroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGroupBox2.Location = New System.Drawing.Point(10, 315)
        Me.grpGroupBox2.Name = "grpGroupBox2"
        Me.grpGroupBox2.Size = New System.Drawing.Size(364, 66)
        Me.grpGroupBox2.TabIndex = 185
        Me.grpGroupBox2.TabStop = False
        Me.grpGroupBox2.Text = "Output:"
        '
        'cmdFatigueData
        '
        Me.cmdFatigueData.BackColor = System.Drawing.Color.Silver
        Me.cmdFatigueData.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.cmdFatigueData.Image = CType(resources.GetObject("cmdFatigueData.Image"), System.Drawing.Image)
        Me.cmdFatigueData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFatigueData.Location = New System.Drawing.Point(237, 25)
        Me.cmdFatigueData.Name = "cmdFatigueData"
        Me.cmdFatigueData.Size = New System.Drawing.Size(110, 30)
        Me.cmdFatigueData.TabIndex = 72
        Me.cmdFatigueData.Text = "     &Fatigue Data"
        Me.cmdFatigueData.UseVisualStyleBackColor = False
        '
        'cmdFEAGraphics
        '
        Me.cmdFEAGraphics.BackColor = System.Drawing.Color.Silver
        Me.cmdFEAGraphics.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.cmdFEAGraphics.Image = CType(resources.GetObject("cmdFEAGraphics.Image"), System.Drawing.Image)
        Me.cmdFEAGraphics.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFEAGraphics.Location = New System.Drawing.Point(14, 25)
        Me.cmdFEAGraphics.Name = "cmdFEAGraphics"
        Me.cmdFEAGraphics.Size = New System.Drawing.Size(110, 30)
        Me.cmdFEAGraphics.TabIndex = 71
        Me.cmdFEAGraphics.Text = "   &Graphics"
        Me.cmdFEAGraphics.UseVisualStyleBackColor = False
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.grpLoadCase)
        Me.grpGroupBox1.Controls.Add(Me.txtAnalysysName)
        Me.grpGroupBox1.Controls.Add(Me.Label5)
        Me.grpGroupBox1.Controls.Add(Me.Label3)
        Me.grpGroupBox1.Controls.Add(Me.cmbCompressTolType)
        Me.grpGroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGroupBox1.Location = New System.Drawing.Point(10, 4)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(364, 152)
        Me.grpGroupBox1.TabIndex = 184
        Me.grpGroupBox1.TabStop = False
        Me.grpGroupBox1.Text = "Load Case:"
        '
        'grpLoadCase
        '
        Me.grpLoadCase.Controls.Add(Me.optAdditional)
        Me.grpLoadCase.Controls.Add(Me.optBaseline)
        Me.grpLoadCase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpLoadCase.Location = New System.Drawing.Point(14, 24)
        Me.grpLoadCase.Name = "grpLoadCase"
        Me.grpLoadCase.Size = New System.Drawing.Size(224, 72)
        Me.grpLoadCase.TabIndex = 77
        Me.grpLoadCase.TabStop = False
        Me.grpLoadCase.Text = "Type:"
        '
        'optAdditional
        '
        Me.optAdditional.AutoSize = True
        Me.optAdditional.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAdditional.Location = New System.Drawing.Point(20, 49)
        Me.optAdditional.Name = "optAdditional"
        Me.optAdditional.Size = New System.Drawing.Size(81, 17)
        Me.optAdditional.TabIndex = 13
        Me.optAdditional.TabStop = True
        Me.optAdditional.Text = "Additional"
        Me.optAdditional.UseVisualStyleBackColor = True
        '
        'optBaseline
        '
        Me.optBaseline.AutoSize = True
        Me.optBaseline.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optBaseline.Location = New System.Drawing.Point(20, 25)
        Me.optBaseline.Name = "optBaseline"
        Me.optBaseline.Size = New System.Drawing.Size(194, 17)
        Me.optBaseline.TabIndex = 12
        Me.optBaseline.TabStop = True
        Me.optBaseline.Text = "Baseline w/ Pre-Compression"
        Me.optBaseline.UseVisualStyleBackColor = True
        '
        'txtAnalysysName
        '
        Me.txtAnalysysName.AcceptsReturn = True
        Me.txtAnalysysName.BackColor = System.Drawing.Color.White
        Me.txtAnalysysName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAnalysysName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnalysysName.ForeColor = System.Drawing.Color.Black
        Me.txtAnalysysName.Location = New System.Drawing.Point(63, 114)
        Me.txtAnalysysName.MaxLength = 0
        Me.txtAnalysysName.Name = "txtAnalysysName"
        Me.txtAnalysysName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAnalysysName.Size = New System.Drawing.Size(284, 21)
        Me.txtAnalysysName.TabIndex = 183
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(17, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 182
        Me.Label5.Text = "Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(252, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 79
        Me.Label3.Text = "Compression Tol."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbCompressTolType
        '
        Me.cmbCompressTolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompressTolType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCompressTolType.Items.AddRange(New Object() {"Nominal", "Minimum", "Maximum"})
        Me.cmbCompressTolType.Location = New System.Drawing.Point(265, 57)
        Me.cmbCompressTolType.Name = "cmbCompressTolType"
        Me.cmbCompressTolType.Size = New System.Drawing.Size(82, 21)
        Me.cmbCompressTolType.TabIndex = 78
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Silver
        Me.Label2.Location = New System.Drawing.Point(0, 395)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(385, 2)
        Me.Label2.TabIndex = 80
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(302, 406)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 72
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'mnuFEA
        '
        Me.mnuFEA.AutoSize = False
        Me.mnuFEA.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuParameters})
        Me.mnuFEA.Location = New System.Drawing.Point(0, 0)
        Me.mnuFEA.Name = "mnuFEA"
        Me.mnuFEA.Size = New System.Drawing.Size(389, 22)
        Me.mnuFEA.TabIndex = 2
        Me.mnuFEA.Text = "MenuStrip1"
        '
        'mnuParameters
        '
        Me.mnuParameters.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGeneral, Me.mnuSeal})
        Me.mnuParameters.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.mnuParameters.Name = "mnuParameters"
        Me.mnuParameters.Size = New System.Drawing.Size(85, 18)
        Me.mnuParameters.Text = "Parameters"
        Me.mnuParameters.TextAlign = System.Drawing.ContentAlignment.TopLeft
        '
        'mnuGeneral
        '
        Me.mnuGeneral.Name = "mnuGeneral"
        Me.mnuGeneral.Size = New System.Drawing.Size(119, 22)
        Me.mnuGeneral.Text = "&General"
        '
        'mnuSeal
        '
        Me.mnuSeal.Name = "mnuSeal"
        Me.mnuSeal.Size = New System.Drawing.Size(119, 22)
        Me.mnuSeal.Text = "&Seal"
        '
        'tpFatigue
        '
        Me.tpFatigue.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.tpFatigue.IsBalloon = True
        Me.tpFatigue.ShowAlways = True
        '
        'IPE_frmFEA
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(389, 467)
        Me.ControlBox = False
        Me.Controls.Add(Me.mnuFEA)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmFEA"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ANSYS FEA"
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpGroupBox2.ResumeLayout(False)
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.grpLoadCase.ResumeLayout(False)
        Me.grpLoadCase.PerformLayout()
        Me.mnuFEA.ResumeLayout(False)
        Me.mnuFEA.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


#Region "FORM EVENT ROUTINES:"

    Private Sub frmFEA_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '===========================================================================================  

        InitializeControl()
        mnuSeal.Text = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type 'PB 24FEB16. AM, check. It may not be needed 

        With gIPE_ANSYS
            cmdFEAGraphics.Enabled = .ExitNormal And File.Exists(gIPE_ANSYS.GraphFileName)

            Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

            'AES 30AUG16
            If (gIPE_Project.Analysis(pICur).FatigueData = True) Then

                chkFatigueData.Checked = True

                If (gIPE_Project.Analysis(pICur).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    cmdFatigueData.Enabled = .ExitNormal And File.Exists(gIPE_File.DirWorkANSYS() & "BL.out") _
                                                         And File.Exists(gIPE_File.DirWorkANSYS() & "Assembly.out")

                ElseIf (gIPE_Project.Analysis(pICur).LoadCase.Type = IPE_clsAnalysis.eLoadType.Additional) Then
                    cmdFatigueData.Enabled = .ExitNormal And File.Exists(gIPE_File.DirWorkANSYS() & "A1.out") _
                                                         And File.Exists(gIPE_File.DirWorkANSYS() & "Assembly.out")

                End If
            Else
                chkFatigueData.Checked = False
                cmdFatigueData.Enabled = False
            End If

        End With

        DisplayData()

    End Sub


    Private Sub InitializeControl()
        '==========================
        If (gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.AddLoad.Count > 0) Then
            optAdditional.Enabled = True
        Else
            optAdditional.Enabled = False
        End If

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            grpLoadCase.Enabled = False
            cmbCompressTolType.Enabled = False
            txtAnalysysName.Enabled = False
            cmdRunANSYS.Enabled = False
        Else
            grpLoadCase.Enabled = True
            cmbCompressTolType.Enabled = True
            txtAnalysysName.Enabled = True
            cmdRunANSYS.Enabled = True
        End If

    End Sub


    Private Sub DisplayData()
        '====================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        If gIPE_Project.Analysis(pICur).AppLoad.PreComp.Exists Then
            optBaseline.Text = "Baseline w/ Pre-Compression"
        Else
            optBaseline.Text = "Baseline"
        End If

        cmbCompressTolType.Text = gIPE_Project.Analysis(pICur).Compression.TolType

        If gIPE_Project.Analysis(pICur).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline Then
            optBaseline.Checked = True

        Else
            optAdditional.Checked = True
        End If

        If gIPE_Project.Analysis(pICur).MatModel = IPE_clsAnalysis.eMatModel.ElastoPlastic Then
            optElastoPlastic.Checked = True
        ElseIf gIPE_Project.Analysis(pICur).MatModel = IPE_clsAnalysis.eMatModel.LinearElastic Then
            optLinearElastic.Checked = True
        End If

    End Sub

#End Region


#Region "CONTROL EVENT ROUTINES:"

    Private Sub mnuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                              Handles mnuGeneral.Click, mnuSeal.Click
        '===============================================================================

        Dim pMenuStrip As ToolStripItem = CType(sender, ToolStripItem)

        Select Case pMenuStrip.Text

            Case "&General"
                mfrmFEAParams = New IPE_frmFEAParams()
                mfrmFEAParams.ShowDialog()

            Case "C-Seal"
                mfrmFEAParamsCSeal = New IPE_frmFEAParamsCSeal()
                mfrmFEAParamsCSeal.ShowDialog()

            Case "E-Seal"
                mfrmFEAParamsESeal = New IPE_frmFEAParamsESeal()
                mfrmFEAParamsESeal.ShowDialog()

            Case "U-Seal"
                mfrmFEAParamsUSeal = New IPE_frmFEAParamsUSeal()
                mfrmFEAParamsUSeal.ShowDialog()

        End Select

    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINE:"

    Private Sub cmdLoadStep_Click(ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs) _
                                  Handles cmdLoadStep.Click
        '============================================================
        SaveData()
        Dim pfrmLoadStep As New IPE_frmLoadStep(gIPE_frmAnalysisSet.ICur)
        pfrmLoadStep.ShowDialog()

    End Sub


    Private Sub cmdRunANSYS_Click(ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs) _
                                  Handles cmdRunANSYS.Click
        '=========================================================================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

        '....Store the chosen culture name.
        Dim pCultureName_Chosen As String = Thread.CurrentThread.CurrentCulture.Name()

        '......Change Current Culture to 'USA'. Required for storing data into the database.
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        '....Save data before Run ANSYS.
        SaveData()

        With gIPE_ANSYS

            '----------------------------------    
            'INITIAL FEA FILE MANIPULATION:
            '------------------------------
            .DeletePrevFiles_ANSYS()   '....Comment for testing 

            '....Create the Input data file - "file.inp". 
            gIPE_Project.Analysis(pICur).WriteFile_ANSYS_Input(gIPE_Unit, gIPE_ANSYS)

            'If .AnalysisType = "Static" Then       'PB 24FEB16.    Commented out
            '===========================
            If gIPE_Project.Analysis(pICur).Seal.Type = "E-Seal" Then

                Dim pESeal As IPE_clsESeal
                pESeal = CType(gIPE_Project.Analysis(pICur).Seal, IPE_clsESeal)

                If pESeal.TemplateNo.Contains("1Gen") Then
                    '....Includes both "1Gen" & "1GenS".
                    pESeal.WriteFile_KP_T1Gen(gIPE_Unit, gIPE_ANSYS)
                End If
            End If


            If gIPE_Project.Analysis(pICur).Seal.Type = "U-Seal" Then

                Dim pUSeal As IPE_clsUSeal
                pUSeal = CType(gIPE_Project.Analysis(pICur).Seal, IPE_clsUSeal)

                pUSeal.WriteFile_KP_T1Gen(gIPE_Unit, gIPE_ANSYS)

            End If

            .Run("ANSYS")           '....Comment it out for bypassing ANSYS run 
            '                       '........during TESTING.
            Dim pVal1 As Integer = gIPE_Project.Customer_ID
            'EXAMINE SOLUTION:
            '----------------
            If .Solve = "Y" And .ExitNormal = True Then

                '....The following method reads if the solution converged or not. 
                '........If converged, read the results into appropriate "gSeal" properties.
                gIPE_Project.Analysis(pICur).ReadFile_ANSYS_Output(gIPE_ANSYS)      'AES 29FEB16


                If .SolnConv = 0 Then               '....No convergence.
                    '----------------
                    Dim pstrPrompt As String
                    Dim pintAttributes As String
                    Dim pstrTitle As String
                    Dim pintAnswer As Integer

                    pstrTitle = "ANSYS Solution : No Convergence"
                    pstrPrompt = " The ANSYS solution did not converge." & vbCrLf & _
                            "You may try with different set of applied loading." & _
                            vbCrLf & "Consult with engineering."
                    pintAttributes = vbCritical + vbOKOnly
                    pintAnswer = MsgBox(pstrPrompt, pintAttributes, pstrTitle)

                    'AES 11MAR16
                    gIPE_Project.Analysis(pICur).DateCreated = DateTime.Now
                    gIPE_Project.Analysis(pICur).TimeCreated = DateTime.Now


                    gIPE_Project.Analysis(pICur).Save_ToDB(gIPE_Unit, gIPE_ANSYS)
                    gIPE_File.Move_GraphicsFiles(gIPE_Project.Customer_ID, gIPE_Project.Platform_ID, gIPE_Project.Project_ID, gIPE_Project.Analysis(pICur).ID)

                    '.....Reset the culture back to the "user-chosen" one.  
                    Thread.CurrentThread.CurrentCulture = New CultureInfo(pCultureName_Chosen)
                    Exit Sub


                ElseIf .SolnConv = 1 Then           '....Convergence achieved.
                    '--------------------
                    gIPE_Project.Analysis(pICur).DateCreated = DateTime.Now
                    gIPE_Project.Analysis(pICur).TimeCreated = DateTime.Now

                    gIPE_Project.Analysis(pICur).Save_ToDB(gIPE_Unit, gIPE_ANSYS)
                    'gIPE_File.Move_GraphicsFiles(gIPE_Project.Analysis(pICur).ID)
                    gIPE_File.Move_GraphicsFiles(gIPE_Project.Customer_ID, gIPE_Project.Platform_ID, gIPE_Project.Project_ID, gIPE_Project.Analysis(pICur).ID)
                End If

            End If

            cmdFEAGraphics.Enabled = .ExitNormal

            'AES 30AUG16
            If (gIPE_Project.Analysis(pICur).FatigueData = True) Then

                If (gIPE_Project.Analysis(pICur).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    cmdFatigueData.Enabled = .ExitNormal And File.Exists(gIPE_File.DirWorkANSYS() & "BL.out") _
                                                    And File.Exists(gIPE_File.DirWorkANSYS() & "Assembly.out")

                ElseIf (gIPE_Project.Analysis(pICur).LoadCase.Type = IPE_clsAnalysis.eLoadType.Additional) Then
                    cmdFatigueData.Enabled = .ExitNormal And File.Exists(gIPE_File.DirWorkANSYS() & "A1.out") _
                                                         And File.Exists(gIPE_File.DirWorkANSYS() & "Assembly.out")

                End If

            Else
                cmdFatigueData.Enabled = False

            End If


        End With

        '.....Reset the culture back to the "user-chosen" one.
        Thread.CurrentThread.CurrentCulture = New CultureInfo(pCultureName_Chosen)

    End Sub


    Private Sub cmdFEAGraphics_Click(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) _
                                     Handles cmdFEAGraphics.Click
        '================================================================
        gIPE_ANSYS.Run("DISPLAY")       '....Comment it out for bypassing ANSYS run 
        '                           '........during TESTING.
    End Sub


    Private Sub cmdOK_Click(ByVal sender As System.Object, _
                            ByVal e As System.EventArgs) _
                            Handles cmdOK.Click
        '==================================================
        SaveData()

        'PB 24FEB16.
        ''....Change the Modified agGUI.js file with its original version.
        'Dim pDM_agGUI_Org_FileName As String
        'pDM_agGUI_Org_FileName = gFiles.DirWorkANSYS & "agGUI_Org.js"


        'If File.Exists(gIPE_ANSYS.DM_agGUI_FileName) And _
        '   File.Exists(pDM_agGUI_Org_FileName) Then

        '    File.Copy(pDM_agGUI_Org_FileName, gIPE_ANSYS.DM_agGUI_FileName, True)
        'End If

        Me.Close()

    End Sub


#Region "HELPER ROUTINE:"

    Private Sub SaveData()
        '=================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

        If (optBaseline.Checked) Then
            gIPE_Project.Analysis(pICur).LoadType = IPE_clsAnalysis.eLoadType.Baseline
        ElseIf (optAdditional.Checked) Then
            gIPE_Project.Analysis(pICur).LoadType = IPE_clsAnalysis.eLoadType.Additional
        End If

        gIPE_Project.Analysis(pICur).CompressionTolType = cmbCompressTolType.Text
        gIPE_Project.Analysis(pICur).LoadCaseName = txtAnalysysName.Text

        'AES 10MAR17
        If (optElastoPlastic.Checked) Then
            gIPE_Project.Analysis(pICur).MatModel = IPE_clsAnalysis.eMatModel.ElastoPlastic
        ElseIf (optLinearElastic.Checked) Then
            gIPE_Project.Analysis(pICur).MatModel = IPE_clsAnalysis.eMatModel.LinearElastic
        End If

        'AES 30AUG16
        gIPE_Project.Analysis(pICur).FatigueData = chkFatigueData.Checked


    End Sub

#End Region

#End Region


    Private Sub optButton_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles _
                                         optBaseline.CheckedChanged, optAdditional.CheckedChanged
        '==========================================================================================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

        'Dim pLoadCaseType As String
        'If (optBaseline.Checked) Then
        '    pLoadCaseType = "Baseline"
        'Else
        '    pLoadCaseType = "Additional"
        'End If

        If (optBaseline.Checked) Then

            If (gIPE_Project.Analysis(pICur).AppLoad.PreComp.Exists) Then
                txtAnalysysName.Text = IPE_clsAnalysis.eLoadType.Baseline.ToString() & "_" & "PreComp" & "_" & cmbCompressTolType.Text.Substring(0, 3)

            Else
                txtAnalysysName.Text = IPE_clsAnalysis.eLoadType.Baseline.ToString() & "_" & cmbCompressTolType.Text.Substring(0, 3)
            End If
        Else
            txtAnalysysName.Text = IPE_clsAnalysis.eLoadType.Additional.ToString() & "_" & cmbCompressTolType.Text.Substring(0, 3)
        End If

    End Sub


    Private Sub cmdFatigueData_Click(sender As System.Object, e As System.EventArgs) Handles cmdFatigueData.Click
        '=========================================================================================================

        Cursor = Cursors.WaitCursor
        modMain_IPE.gIPE_Report.WriteFatigueData(gIPE_frmAnalysisSet.ICur, gIPE_Project)
        Cursor = Cursors.Default
    End Sub

End Class
