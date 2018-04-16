'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmMain                                '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  19MAY17                                '
'                                                                              '
'===============================================================================
'
Option Explicit On
Imports System.Windows.Forms
Imports System.IO
Imports System.Globalization
Imports System.Math
Imports System.Diagnostics
Imports clsLibrary11
Imports System.Linq



Public Class IPE_frmMain
    Inherits System.Windows.Forms.Form

    Private mProject As IPE_clsProject
    Private mUnit As IPE_clsUnit
    Private mOpCond As IPE_clsOpCond
    Private mCavity As IPE_clsCavity
    Private mSeal As IPE_clsSeal
    Private mMat As IPE_clsMaterial
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Private mANSYS As IPE_clsANSYS


#Region " Windows Form Designer generated code "

    Public Sub New()
        '============
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        '================================================================
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
    Friend WithEvents lblProjectCustomerPN As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents lblProjectParkerPN As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents lblPlatform As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents mnuUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip

    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdIPEProject As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdResults As System.Windows.Forms.Button
    Friend WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblUnitSys As System.Windows.Forms.Label
    Friend WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblProjectCustomer As System.Windows.Forms.Label
    Friend WithEvents cmdAnalysisSet As System.Windows.Forms.Button
    Friend WithEvents SBar1 As System.Windows.Forms.StatusStrip
    Friend WithEvents SBpanel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SBPanel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SBPanel4 As System.Windows.Forms.ToolStripStatusLabel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmMain))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdIPEProject = New System.Windows.Forms.Button()
        Me.cmdResults = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdAnalysisSet = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblProjectCustomer = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblUnitSys = New System.Windows.Forms.Label()
        Me.SBar1 = New System.Windows.Forms.StatusStrip()
        Me.SBpanel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SBPanel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SBPanel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblProjectCustomerPN = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.lblProjectParkerPN = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.lblPlatform = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.mnuUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.imgLogo = New System.Windows.Forms.PictureBox()
        Me.GroupBox1.SuspendLayout()
        Me.SBar1.SuspendLayout()
        Me.MainMenu.SuspendLayout()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmdIPEProject)
        Me.GroupBox1.Controls.Add(Me.cmdResults)
        Me.GroupBox1.Controls.Add(Me.cmdExit)
        Me.GroupBox1.Controls.Add(Me.cmdAnalysisSet)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(561, 28)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(138, 347)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(0, 343)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(137, 2)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Label4"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.BackColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(136, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(2, 341)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Label3"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(1, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(137, 2)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(1, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(2, 341)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Label1"
        '
        'cmdIPEProject
        '
        Me.cmdIPEProject.AllowDrop = True
        Me.cmdIPEProject.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdIPEProject.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdIPEProject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdIPEProject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdIPEProject.Location = New System.Drawing.Point(5, 10)
        Me.cmdIPEProject.Name = "cmdIPEProject"
        Me.cmdIPEProject.Size = New System.Drawing.Size(129, 30)
        Me.cmdIPEProject.TabIndex = 0
        Me.cmdIPEProject.Text = "IPE &Project"
        Me.cmdIPEProject.UseVisualStyleBackColor = False
        '
        'cmdResults
        '
        Me.cmdResults.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdResults.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdResults.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdResults.Location = New System.Drawing.Point(5, 75)
        Me.cmdResults.Name = "cmdResults"
        Me.cmdResults.Size = New System.Drawing.Size(129, 30)
        Me.cmdResults.TabIndex = 5
        Me.cmdResults.Text = "Summary Results"
        Me.cmdResults.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdExit.Location = New System.Drawing.Point(5, 107)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(129, 30)
        Me.cmdExit.TabIndex = 6
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAnalysisSet
        '
        Me.cmdAnalysisSet.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdAnalysisSet.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAnalysisSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAnalysisSet.Location = New System.Drawing.Point(5, 43)
        Me.cmdAnalysisSet.Name = "cmdAnalysisSet"
        Me.cmdAnalysisSet.Size = New System.Drawing.Size(129, 30)
        Me.cmdAnalysisSet.TabIndex = 4
        Me.cmdAnalysisSet.Text = "&Analysis Set"
        Me.cmdAnalysisSet.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(2, 79)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(112, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Customer:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label5.Visible = False
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Navy
        Me.Label7.Location = New System.Drawing.Point(457, 79)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Unit:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label7.Visible = False
        '
        'lblProjectCustomer
        '
        Me.lblProjectCustomer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProjectCustomer.Location = New System.Drawing.Point(116, 79)
        Me.lblProjectCustomer.Name = "lblProjectCustomer"
        Me.lblProjectCustomer.Size = New System.Drawing.Size(134, 13)
        Me.lblProjectCustomer.TabIndex = 7
        Me.lblProjectCustomer.Visible = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(16, 47)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(160, 24)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Project Information:"
        Me.Label10.Visible = False
        '
        'lblUnitSys
        '
        Me.lblUnitSys.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitSys.ForeColor = System.Drawing.Color.Black
        Me.lblUnitSys.Location = New System.Drawing.Point(505, 79)
        Me.lblUnitSys.Name = "lblUnitSys"
        Me.lblUnitSys.Size = New System.Drawing.Size(56, 13)
        Me.lblUnitSys.TabIndex = 10
        Me.lblUnitSys.Text = "English"
        Me.lblUnitSys.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblUnitSys.Visible = False
        '
        'SBar1
        '
        Me.SBar1.AutoSize = False
        Me.SBar1.BackColor = System.Drawing.Color.AliceBlue
        Me.SBar1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.SBar1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.SBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SBpanel1, Me.SBPanel3, Me.SBPanel4})
        Me.SBar1.Location = New System.Drawing.Point(0, 394)
        Me.SBar1.Name = "SBar1"
        Me.SBar1.Size = New System.Drawing.Size(704, 22)
        Me.SBar1.TabIndex = 67
        '
        'SBpanel1
        '
        Me.SBpanel1.AutoSize = False
        Me.SBpanel1.BackColor = System.Drawing.Color.AliceBlue
        Me.SBpanel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.SBpanel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.SBpanel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SBpanel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBpanel1.Name = "SBpanel1"
        Me.SBpanel1.Size = New System.Drawing.Size(230, 17)
        '
        'SBPanel3
        '
        Me.SBPanel3.AutoSize = False
        Me.SBPanel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.SBPanel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.SBPanel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SBPanel3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBPanel3.Name = "SBPanel3"
        Me.SBPanel3.Size = New System.Drawing.Size(230, 17)
        '
        'SBPanel4
        '
        Me.SBPanel4.AutoSize = False
        Me.SBPanel4.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.SBPanel4.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.SBPanel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SBPanel4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBPanel4.Name = "SBPanel4"
        Me.SBPanel4.Size = New System.Drawing.Size(220, 17)
        '
        'lblProjectCustomerPN
        '
        Me.lblProjectCustomerPN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProjectCustomerPN.Location = New System.Drawing.Point(116, 110)
        Me.lblProjectCustomerPN.Name = "lblProjectCustomerPN"
        Me.lblProjectCustomerPN.Size = New System.Drawing.Size(134, 13)
        Me.lblProjectCustomerPN.TabIndex = 75
        Me.lblProjectCustomerPN.Visible = False
        '
        'Label39
        '
        Me.Label39.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.Color.Navy
        Me.Label39.Location = New System.Drawing.Point(2, 110)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(112, 13)
        Me.Label39.TabIndex = 74
        Me.Label39.Text = "Customer PN:"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label39.Visible = False
        '
        'lblProjectParkerPN
        '
        Me.lblProjectParkerPN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProjectParkerPN.Location = New System.Drawing.Point(334, 110)
        Me.lblProjectParkerPN.Name = "lblProjectParkerPN"
        Me.lblProjectParkerPN.Size = New System.Drawing.Size(99, 13)
        Me.lblProjectParkerPN.TabIndex = 77
        Me.lblProjectParkerPN.Visible = False
        '
        'Label41
        '
        Me.Label41.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.Color.Navy
        Me.Label41.Location = New System.Drawing.Point(245, 110)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(88, 13)
        Me.Label41.TabIndex = 76
        Me.Label41.Text = "Parker PN:"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label41.Visible = False
        '
        'lblPlatform
        '
        Me.lblPlatform.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlatform.Location = New System.Drawing.Point(334, 79)
        Me.lblPlatform.Name = "lblPlatform"
        Me.lblPlatform.Size = New System.Drawing.Size(113, 13)
        Me.lblPlatform.TabIndex = 69
        Me.lblPlatform.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPlatform.Visible = False
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Navy
        Me.Label33.Location = New System.Drawing.Point(266, 79)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(67, 13)
        Me.Label33.TabIndex = 68
        Me.Label33.Text = "Platform:"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label33.Visible = False
        '
        'mnuUser
        '
        Me.mnuUser.Name = "mnuUser"
        Me.mnuUser.Size = New System.Drawing.Size(42, 20)
        Me.mnuUser.Text = "&User"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(107, 22)
        Me.mnuAbout.Text = "&About"
        '
        'MainMenu
        '
        Me.MainMenu.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuUser, Me.mnuHelp})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(704, 24)
        Me.MainMenu.TabIndex = 65
        Me.MainMenu.Text = "MainMenu"
        '
        'imgLogo
        '
        Me.imgLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(24, 325)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(176, 56)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgLogo.TabIndex = 63
        Me.imgLogo.TabStop = False
        '
        'IPE_frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(704, 416)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblProjectParkerPN)
        Me.Controls.Add(Me.Label41)
        Me.Controls.Add(Me.lblProjectCustomerPN)
        Me.Controls.Add(Me.Label39)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblPlatform)
        Me.Controls.Add(Me.Label33)
        Me.Controls.Add(Me.SBar1)
        Me.Controls.Add(Me.imgLogo)
        Me.Controls.Add(Me.lblUnitSys)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.lblProjectCustomer)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.MainMenu)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SealIPE 10.1: Main Form                                              Metalic Seal" & _
    ": Integrated Product Engineering System"
        Me.GroupBox1.ResumeLayout(False)
        Me.SBar1.ResumeLayout(False)
        Me.SBar1.PerformLayout()
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


#Region "FORM EVENT ROUTINES:"

    Private Sub frmMain_Load(ByVal sender As Object, _
                             ByVal e As System.EventArgs) _
                             Handles MyBase.Load
        '=====================================================
        GroupBox1.Visible = True

        '....Read the Ini file:                     
        gFile.ReadIniFile(gUser, gIPE_Project, gIPE_ANSYS, gIPE_Unit)  '....Get UserName, Phone No & Unit System. 

        '---------------------------------------------------------------------------------
        '....Status Bar Panels:

        Dim pWidth As Int32 = (SBar1.Width) / 3

        SBpanel1.Width = pWidth
        SBPanel3.Width = pWidth

        Dim pCI As New CultureInfo("en-US")
        SBPanel4.Text = Today.DayOfWeek.ToString() & ", " & _
                        Today.ToString(" MMMM dd, yyyy", pCI.DateTimeFormat()) 'US Format only
        '--------------------------------------------------------------------------------------

        '....Display logo:
        With imgLogo
            .Width = 176 : .Height = .Width / gcSngLogoAspectRatio
        End With
        LoadImageLogo(imgLogo)

        '....Form caption.
        Me.Text = gcstrIPEProgramName & " " & gcstrIPEVersionNo & Space(80) & "Main Form"

        '....It is used to display Default Units.    
        UpdateDisplay()


    End Sub


    Private Sub frmMain_FormClosing(ByVal sender As Object, _
                            ByVal e As System.Windows.Forms.FormClosingEventArgs) _
                            Handles Me.FormClosing
        '==========================================================================
        End

    End Sub

#End Region


#Region "MENU EVENT ROUTINES:"

    Private Sub mnuItem_Click(ByVal sender As System.Object, _
                              ByVal e As System.EventArgs) Handles mnuUser.Click, mnuAbout.Click
        '=======================================================================================

        Dim pMenuStrip As ToolStripItem = CType(sender, ToolStripItem)

        Select Case pMenuStrip.Text

            Case "&User"
                gIPE_frmUser.ShowDialog()
                'gfrmUserInfo.ShowDialog()       'AES 20MAR17


            Case "&About"
                Me.Cursor = Cursors.WaitCursor
                Dim pFrmAbout As New IPE_frmAbout()
                pFrmAbout.ShowDialog()
                Me.Cursor = Cursors.Default

        End Select

    End Sub



#Region "HELPER ROUTINES:"

    Private Sub OpenUsersManual()
        '=======================            
        Dim pProc As New Process()

        With pProc
            .StartInfo.FileName = gIPE_File.UsersManual
            .StartInfo.WindowStyle = ProcessWindowStyle.Maximized
            .Start()
        End With

    End Sub


#End Region


#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                 Handles cmdIPEProject.Click, cmdAnalysisSet.Click, cmdResults.Click, cmdExit.Click
        '=========================================================================================================== 

        'Error Message:              
        '-------------
        Dim pintAttributes As String
        Dim pstrTitle As String
        Dim pstrMsg As String

        pintAttributes = vbCritical + vbOKOnly

        Dim pcmdButton As Button = CType(sender, Button)

        Try

            Select Case pcmdButton.Name

                Case "cmdIPEProject"
                    '--------------------
                    Me.Cursor = Cursors.WaitCursor
                    'Dim pProjectID As Integer = 0
                    Dim pfrmPartInfo As New frmPartMain("SealIPE")
                    pfrmPartInfo.ShowDialog()

                    'AES 19MAY17
                    gIPE_Project.Analysis.Clear()

                    gIPE_Project.Project_ID = gPartProject.Project_ID 'pfrmPartInfo.IPEProjectID
                    gIPE_Project.Retrive_FromPNR(gIPE_Project.Project_ID)

                    Dim pSealIPEEntities As New SealIPEDBEntities()
                    Dim pQryAnalysis = (From pRec In pSealIPEEntities.tblAnalysis
                                            Where pRec.fldProjectID = gIPE_Project.Project_ID Order By pRec.fldID Ascending Select pRec).ToList()

                    Dim pCount As Integer = pQryAnalysis.Count()
                    For i As Integer = 0 To pCount - 1
                        gIPE_Project.Add_Analysis()
                        gIPE_Project.Analysis(i).ID = pQryAnalysis(i).fldID
                        gIPE_Project.Analysis(i).Retrieve_FromDB(gIPE_Unit, gIPE_ANSYS)
                    Next

                    'Dim pfrmProject As New frmProject()
                    'pfrmProject.ShowDialog()
                    Me.Cursor = Cursors.Default

                Case "cmdAnalysisSet"
                    '------------
                    gIPE_frmAnalysisSet.ShowDialog()

                Case "cmdResults"
                    '-------------                                      
                    If gIPE_Project.Project_ID = 0 Then
                        pstrTitle = "Error Message."
                        pstrMsg = "Plase select a Parker Part No. first."
                        MsgBox(pstrMsg, pintAttributes, pstrTitle)

                        ''gfrmProject.ShowDialog()
                        Exit Sub

                    Else
                        gIPE_frmResults.FromFrmDrawing = False
                        gIPE_frmResults.ShowDialog()

                    End If

                Case "cmdTestModule"
                    '---------------------  
                    'gTestUser.RetrieveUserRoles()

                    'Dim pRoleCount As Integer = 0

                    'If (gTestUser.Admin) Then
                    '    pRoleCount = pRoleCount + 1
                    'End If

                    'If (gTestUser.Tester) Then
                    '    pRoleCount = pRoleCount + 1
                    'End If

                    'If (gTestUser.Engg) Then
                    '    pRoleCount = pRoleCount + 1
                    'End If

                    'If (gTestUser.Quality) Then
                    '    pRoleCount = pRoleCount + 1
                    'End If

                    'If (pRoleCount > 1) Then
                    '    Dim pfrmTestUserRole As New frmTest_User_Role()
                    '    pfrmTestUserRole.ShowDialog()

                    'Else
                    '    'Dim pfrmTestMain As New frmTest_Main()
                    '    gfrmTestMain.ShowDialog()
                    'End If

                Case "cmdExit"
                    'AES 11MAR16
                    Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
                    If (gIPE_Project.Analysis.Count > 0) Then
                        If (Not gIPE_Project.Analysis(pICur).IsRecordExists_DB()) Then
                            Dim pstrPrompt As String
                            Dim pintAnswer As Integer
                            pstrPrompt = " Do you want to save the record to DB?" & vbCrLf
                            pstrTitle = "Save Data"
                            pintAnswer = MessageBox.Show(pstrPrompt, pstrTitle, _
                                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                            If pintAnswer = Windows.Forms.DialogResult.Yes Then

                                gIPE_Project.Analysis(pICur).DateCreated = DateTime.Now
                                gIPE_Project.Analysis(pICur).TimeCreated = DateTime.Now

                                gIPE_Project.Analysis(pICur).Save_ToDB(gIPE_Unit, gIPE_ANSYS)
                            End If

                        End If
                    End If

                    End

            End Select

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

#End Region

#Region "UPDATE DISPLAY:"

    Public Sub UpdateDisplay()
        '=====================

        SBpanel1.Text = gIPE_User.Name

        With gIPE_Project
            '.CultureName = "USA"
            SBPanel3.Text = IIf(.CultureName <> "", .CultureName + " Format", "")
        End With

        'Project Info:              (frmProject)
        '-------------
        lblProjectCustomer.Text = gIPE_Project.Customer()
        lblPlatform.Text = gIPE_Project.Platform()
        gIPE_Unit.System = gIPE_Project.UnitSystem()
        lblUnitSys.Text = gIPE_Unit.System

        If (gIPE_Project.CustomerPN() <> "") Then
            lblProjectCustomerPN.Text = gIPE_Project.CustomerPN()
        Else
            lblProjectCustomerPN.Text = "N/A"
        End If

        If (gIPE_Project.ParkerPN() <> "") Then
            lblProjectParkerPN.Text = gIPE_Project.ParkerPN()
        Else
            lblProjectParkerPN.Text = "N/A"
        End If

    End Sub

#End Region


End Class
