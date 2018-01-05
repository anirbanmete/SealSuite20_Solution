'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmResults                             '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY17                                '
'                                                                              '
'===============================================================================

Imports System.Windows.Forms
Imports System.Data
Imports System.Data.OleDb
Imports System.Math

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports System.Globalization
Imports System.Threading
Imports System.Linq
Imports clsLibrary11


Public Class IPE_frmResult
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        '=============
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
    Public WithEvents imgLogo As System.Windows.Forms.PictureBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents txtProjectName As System.Windows.Forms.TextBox

    Public WithEvents cmdSummaryTable As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBrief As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grdProjectData As System.Windows.Forms.DataGridView
    Public WithEvents lblResultSummary As System.Windows.Forms.Label
    Friend WithEvents mnuChangeFormat As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ToolTipReport As System.Windows.Forms.ToolTip
    Friend WithEvents mnuCustom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Public WithEvents cmdMainForm As System.Windows.Forms.Button
    Public WithEvents cmdDrawings_Reports As System.Windows.Forms.Button
    Public WithEvents txtParkerPN As System.Windows.Forms.TextBox
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents txtPlatform As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents TextBox3 As System.Windows.Forms.TextBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents lblDesc As System.Windows.Forms.Label
    Public WithEvents cmdCreateInFile As System.Windows.Forms.Button
    Friend WithEvents chkSel_Project As System.Windows.Forms.CheckBox
    Friend WithEvents mnuDetails As System.Windows.Forms.ToolStripMenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmResult))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtProjectName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdSummaryTable = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.imgLogo = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBrief = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeFormat = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCustom = New System.Windows.Forms.ToolStripMenuItem()
        Me.grdProjectData = New System.Windows.Forms.DataGridView()
        Me.lblResultSummary = New System.Windows.Forms.Label()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkSel_Project = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblDesc = New System.Windows.Forms.Label()
        Me.cmdDrawings_Reports = New System.Windows.Forms.Button()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.txtParkerPN = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPlatform = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmdCreateInFile = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ToolTipReport = New System.Windows.Forms.ToolTip(Me.components)
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.cmdMainForm = New System.Windows.Forms.Button()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.grdProjectData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtProjectName
        '
        Me.txtProjectName.AcceptsReturn = True
        Me.txtProjectName.BackColor = System.Drawing.Color.LightGray
        Me.txtProjectName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProjectName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProjectName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProjectName.Location = New System.Drawing.Point(114, 47)
        Me.txtProjectName.MaxLength = 0
        Me.txtProjectName.Name = "txtProjectName"
        Me.txtProjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProjectName.Size = New System.Drawing.Size(165, 21)
        Me.txtProjectName.TabIndex = 5
        Me.txtProjectName.TabStop = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(18, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(90, 18)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Customer"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdSummaryTable
        '
        Me.cmdSummaryTable.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdSummaryTable.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdSummaryTable.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSummaryTable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSummaryTable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSummaryTable.Image = CType(resources.GetObject("cmdSummaryTable.Image"), System.Drawing.Image)
        Me.cmdSummaryTable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSummaryTable.Location = New System.Drawing.Point(455, 438)
        Me.cmdSummaryTable.Name = "cmdSummaryTable"
        Me.cmdSummaryTable.Size = New System.Drawing.Size(142, 30)
        Me.cmdSummaryTable.TabIndex = 4
        Me.cmdSummaryTable.Text = "      Summary  &Table"
        Me.cmdSummaryTable.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(936, 435)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(75, 27)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "   &Close"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'imgLogo
        '
        Me.imgLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.imgLogo.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(842, 4)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(169, 65)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgLogo.TabIndex = 21
        Me.imgLogo.TabStop = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOptions})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1028, 24)
        Me.MenuStrip1.TabIndex = 31
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuBrief, Me.mnuDetails, Me.mnuChangeFormat, Me.mnuCustom})
        Me.mnuOptions.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuOptions.Size = New System.Drawing.Size(102, 20)
        Me.mnuOptions.Text = "View Options"
        '
        'mnuBrief
        '
        Me.mnuBrief.Name = "mnuBrief"
        Me.mnuBrief.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.mnuBrief.Size = New System.Drawing.Size(171, 22)
        Me.mnuBrief.Text = "&Brief"
        '
        'mnuDetails
        '
        Me.mnuDetails.Name = "mnuDetails"
        Me.mnuDetails.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.mnuDetails.Size = New System.Drawing.Size(171, 22)
        Me.mnuDetails.Text = "&Details"
        '
        'mnuChangeFormat
        '
        Me.mnuChangeFormat.Name = "mnuChangeFormat"
        Me.mnuChangeFormat.Size = New System.Drawing.Size(171, 22)
        Me.mnuChangeFormat.Text = "&Change Format"
        Me.mnuChangeFormat.Visible = False
        '
        'mnuCustom
        '
        Me.mnuCustom.Name = "mnuCustom"
        Me.mnuCustom.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuCustom.Size = New System.Drawing.Size(171, 22)
        Me.mnuCustom.Text = "&Custom"
        '
        'grdProjectData
        '
        Me.grdProjectData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdProjectData.BackgroundColor = System.Drawing.SystemColors.ActiveBorder
        Me.grdProjectData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdProjectData.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdProjectData.ColumnHeadersHeight = 55
        Me.grdProjectData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdProjectData.DefaultCellStyle = DataGridViewCellStyle2
        Me.grdProjectData.Location = New System.Drawing.Point(5, 125)
        Me.grdProjectData.Name = "grdProjectData"
        Me.grdProjectData.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.NullValue = Nothing
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdProjectData.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.grdProjectData.RowHeadersWidth = 25
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        Me.grdProjectData.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.grdProjectData.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.grdProjectData.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White
        Me.grdProjectData.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdProjectData.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black
        Me.grdProjectData.Size = New System.Drawing.Size(1012, 281)
        Me.grdProjectData.TabIndex = 10
        Me.grdProjectData.TabStop = False
        '
        'lblResultSummary
        '
        Me.lblResultSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblResultSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblResultSummary.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblResultSummary.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResultSummary.ForeColor = System.Drawing.Color.Black
        Me.lblResultSummary.Location = New System.Drawing.Point(335, 98)
        Me.lblResultSummary.Name = "lblResultSummary"
        Me.lblResultSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblResultSummary.Size = New System.Drawing.Size(356, 19)
        Me.lblResultSummary.TabIndex = 33
        Me.lblResultSummary.Text = "Summary  Results  Table"
        Me.lblResultSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(2, 28)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(1024, 482)
        Me.lblBorder.TabIndex = 34
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chkSel_Project)
        Me.Panel1.Controls.Add(Me.cmdSummaryTable)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.lblDesc)
        Me.Panel1.Controls.Add(Me.cmdDrawings_Reports)
        Me.Panel1.Controls.Add(Me.TextBox3)
        Me.Panel1.Controls.Add(Me.txtParkerPN)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtPlatform)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.grdProjectData)
        Me.Panel1.Controls.Add(Me.cmdCreateInFile)
        Me.Panel1.Controls.Add(Me.cmdClose)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblResultSummary)
        Me.Panel1.Controls.Add(Me.imgLogo)
        Me.Panel1.Controls.Add(Me.txtProjectName)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(3, 29)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1022, 480)
        Me.Panel1.TabIndex = 35
        '
        'chkSel_Project
        '
        Me.chkSel_Project.AutoSize = True
        Me.chkSel_Project.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkSel_Project.ForeColor = System.Drawing.Color.Blue
        Me.chkSel_Project.Location = New System.Drawing.Point(46, 98)
        Me.chkSel_Project.Name = "chkSel_Project"
        Me.chkSel_Project.Size = New System.Drawing.Size(139, 17)
        Me.chkSel_Project.TabIndex = 75
        Me.chkSel_Project.Text = "Selected for Project"
        Me.chkSel_Project.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(488, 420)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(76, 13)
        Me.Label6.TabIndex = 48
        Me.Label6.Text = "All Analyses"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDesc
        '
        Me.lblDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDesc.AutoSize = True
        Me.lblDesc.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.ForeColor = System.Drawing.Color.Blue
        Me.lblDesc.Location = New System.Drawing.Point(19, 420)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDesc.Size = New System.Drawing.Size(0, 13)
        Me.lblDesc.TabIndex = 47
        Me.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdDrawings_Reports
        '
        Me.cmdDrawings_Reports.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdDrawings_Reports.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdDrawings_Reports.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDrawings_Reports.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDrawings_Reports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDrawings_Reports.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDrawings_Reports.Location = New System.Drawing.Point(58, 438)
        Me.cmdDrawings_Reports.Name = "cmdDrawings_Reports"
        Me.cmdDrawings_Reports.Size = New System.Drawing.Size(142, 30)
        Me.cmdDrawings_Reports.TabIndex = 39
        Me.cmdDrawings_Reports.Text = "  Drawings && Reports"
        Me.cmdDrawings_Reports.UseVisualStyleBackColor = False
        '
        'TextBox3
        '
        Me.TextBox3.AcceptsReturn = True
        Me.TextBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox3.BackColor = System.Drawing.Color.LightGray
        Me.TextBox3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.ForeColor = System.Drawing.Color.Black
        Me.TextBox3.Location = New System.Drawing.Point(915, 98)
        Me.TextBox3.MaxLength = 0
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox3.Size = New System.Drawing.Size(46, 21)
        Me.TextBox3.TabIndex = 44
        Me.TextBox3.TabStop = False
        Me.TextBox3.Text = "in"
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtParkerPN
        '
        Me.txtParkerPN.AcceptsReturn = True
        Me.txtParkerPN.BackColor = System.Drawing.Color.LightGray
        Me.txtParkerPN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParkerPN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParkerPN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParkerPN.Location = New System.Drawing.Point(650, 47)
        Me.txtParkerPN.MaxLength = 0
        Me.txtParkerPN.Name = "txtParkerPN"
        Me.txtParkerPN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParkerPN.Size = New System.Drawing.Size(165, 21)
        Me.txtParkerPN.TabIndex = 42
        Me.txtParkerPN.TabStop = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(556, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(90, 18)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Parker PN"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPlatform
        '
        Me.txtPlatform.AcceptsReturn = True
        Me.txtPlatform.BackColor = System.Drawing.Color.LightGray
        Me.txtPlatform.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPlatform.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlatform.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPlatform.Location = New System.Drawing.Point(382, 47)
        Me.txtPlatform.MaxLength = 0
        Me.txtPlatform.Name = "txtPlatform"
        Me.txtPlatform.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlatform.Size = New System.Drawing.Size(165, 21)
        Me.txtPlatform.TabIndex = 40
        Me.txtPlatform.TabStop = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(285, 47)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(90, 18)
        Me.Label4.TabIndex = 41
        Me.Label4.Text = "Platform"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdCreateInFile
        '
        Me.cmdCreateInFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCreateInFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdCreateInFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCreateInFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreateInFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCreateInFile.Image = CType(resources.GetObject("cmdCreateInFile.Image"), System.Drawing.Image)
        Me.cmdCreateInFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCreateInFile.Location = New System.Drawing.Point(650, 438)
        Me.cmdCreateInFile.Name = "cmdCreateInFile"
        Me.cmdCreateInFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCreateInFile.Size = New System.Drawing.Size(142, 30)
        Me.cmdCreateInFile.TabIndex = 38
        Me.cmdCreateInFile.Text = "    Create Input File"
        Me.cmdCreateInFile.UseVisualStyleBackColor = False
        Me.cmdCreateInFile.Visible = False
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(804, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(105, 19)
        Me.Label3.TabIndex = 37
        Me.Label3.Text = "Length Unit"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdMainForm
        '
        Me.cmdMainForm.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdMainForm.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdMainForm.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMainForm.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMainForm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMainForm.Image = CType(resources.GetObject("cmdMainForm.Image"), System.Drawing.Image)
        Me.cmdMainForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdMainForm.Location = New System.Drawing.Point(941, 408)
        Me.cmdMainForm.Name = "cmdMainForm"
        Me.cmdMainForm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMainForm.Size = New System.Drawing.Size(75, 25)
        Me.cmdMainForm.TabIndex = 1
        Me.cmdMainForm.Text = "   &Close"
        Me.cmdMainForm.UseVisualStyleBackColor = False
        '
        'IPE_frmResult
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1028, 512)
        Me.ControlBox = False
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimizeBox = False
        Me.Name = "IPE_frmResult"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Summary Results"
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.grdProjectData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


#Region "EVENT DECLARATION:"

    Event grdProjectButtonClick(sender As DataGridView, e As DataGridViewCellEventArgs)
#End Region


#Region "MEMBER VARIABLE DECLARATION:"
    '===================================
    '....Variable to hold the direction of form activation e.g. whether it is being
    '........activated from the frmDrawing.

    Public mFromFrmDrawing As Boolean

    '....Unit Parameters                                
    Private mUnitFCon As String
    Private mUnitStress As String

    '....DataGrid Table Style and Column                
    Private mTextColumn(0) As DataGridViewTextBoxColumn    '....Dynamic Array    

    '....Currently Selected Analysis
    Private mISel As Integer = 0

#End Region


#Region "CLASS PROPERTY ROUTINE:"
    '============================

    '....ISel
    Public ReadOnly Property ISel() As Integer
        '======================================
        Get
            Return mISel
        End Get

    End Property


    Public Property FromFrmDrawing() As Boolean
        '======================================
        Get
            Return mFromFrmDrawing
        End Get

        Set(ByVal value As Boolean)
            mFromFrmDrawing = value
        End Set

    End Property

#End Region


#Region "FORM EVENT ROUTINES: "
    '==========================

    Private Sub frmResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) _
                                Handles MyBase.Load
        '=============================================================================
        '....Initialize the form activation direction.
        mFromFrmDrawing = False

        With imgLogo
            .Width = 176 : .Height = .Width / gcSngLogoAspectRatio
        End With
        LoadImageLogo(imgLogo)

        With gIPE_Unit
            mUnitFCon = .F & "/" & .UserL
            mUnitStress = .UserStress
            'gUnitSel.System = .System
        End With

        Dim FontBold As New Font(grdProjectData.ColumnHeadersDefaultCellStyle.Font, FontStyle.Bold)
        grdProjectData.ColumnHeadersDefaultCellStyle.Font = FontBold

        '....CUSTOM Setting is Default.
        SetDataGridControl_CUSTOM()

        PopulateDataGrid()

        DisplayData()

        grdProjectData.Refresh()

        If grdProjectData.Rows.Count <= 0 Then

            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes As Integer
            Dim pintAnswer As Integer

            pstrTitle = "WARNING MESSAGE: "
            pstrMsg = "No record for the selected project." & vbCrLf & _
                      "Please reselect project properly."
            pintAttributes = MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly

            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

            Me.Close()
            Exit Sub
        End If

        Analysis_Desc()

        'grdProjectData.Rows(0).Selected = True

        If (Not IsNothing(grdProjectData.CurrentRow)) Then
            Dim pIndex As Integer = grdProjectData.Rows(grdProjectData.CurrentRow.Index).Cells(0).Value - 1
            If (gIPE_Project.Analysis(pIndex).Seal.Selected = True) Then
                chkSel_Project.Checked = True

            Else
                chkSel_Project.Checked = False

            End If
        End If

    End Sub


    Private Sub PopulateDataGrid()
        '=========================
        grdProjectData.Rows.Clear()
        Dim pRowIndx As Integer = 0
        For i As Integer = 0 To modMain_IPE.gIPE_Project.Analysis.Count - 1

            If (modMain_IPE.gIPE_Project.Analysis(i).State = IPE_clsAnalysis.eState.Complete) Then
                grdProjectData.Rows.Add()
                grdProjectData.Rows(pRowIndx).Cells(0).Value = (i + 1).ToString()
                grdProjectData.Rows(pRowIndx).Cells(1).Value = modMain_IPE.gIPE_Project.Analysis(i).Seal.MCrossSecNo
                grdProjectData.Rows(pRowIndx).Cells(2).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).Seal.Hfree)
                grdProjectData.Rows(pRowIndx).Cells(3).Value = modMain_IPE.gIPE_Project.Analysis(i).DateCreated
                grdProjectData.Rows(pRowIndx).Cells(4).Value = modMain_IPE.gIPE_Project.Analysis(i).TimeCreated.ToString("hh:mm tt")

                grdProjectData.Rows(pRowIndx).Cells(5).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).Cavity.Dia(2))
                grdProjectData.Rows(pRowIndx).Cells(6).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).Cavity.Dia(1))
                grdProjectData.Rows(pRowIndx).Cells(7).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).Cavity.Depth)

                grdProjectData.Rows(pRowIndx).Cells(8).Value = gIPE_Unit.FormatPDiffUnitUser(modMain_IPE.gIPE_Project.Analysis(i).OpCond.PDiff)
                grdProjectData.Rows(pRowIndx).Cells(9).Value = modMain_IPE.gIPE_Project.Analysis(i).OpCond.TOper
                grdProjectData.Rows(pRowIndx).Cells(10).Value = modMain_IPE.gIPE_Project.Analysis(i).OpCond.POrient

                grdProjectData.Rows(pRowIndx).Cells(11).Value = modMain_IPE.gIPE_Project.Analysis(i).AppLoad.PreComp.Exists
                grdProjectData.Rows(pRowIndx).Cells(12).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).AppLoad.PreComp.HMin)
                grdProjectData.Rows(pRowIndx).Cells(13).Value = modMain_IPE.gIPE_Project.Analysis(i).AppLoad.RadConstraint

                grdProjectData.Rows(pRowIndx).Cells(14).Value = modMain_IPE.gIPE_Project.Analysis(i).LoadCase.Type.ToString()
                grdProjectData.Rows(pRowIndx).Cells(15).Value = modMain_IPE.gIPE_Project.Analysis(i).Compression.TolType
                grdProjectData.Rows(pRowIndx).Cells(16).Value = modMain_IPE.gIPE_Project.Analysis(i).LoadCase.Name

                'AES 20MAR17
                If (modMain_IPE.gIPE_Project.Analysis(i).MatModel = IPE_clsAnalysis.eMatModel.ElastoPlastic) Then
                    grdProjectData.Rows(pRowIndx).Cells(17).Value = "Elasto-Plastic"

                ElseIf (modMain_IPE.gIPE_Project.Analysis(i).MatModel = IPE_clsAnalysis.eMatModel.LinearElastic) Then
                    grdProjectData.Rows(pRowIndx).Cells(17).Value = "Linear Elastic"
                End If
                'grdProjectData.Rows(pRowIndx).Cells(17).Value = modMain.gIPE_Project.Analysis(i).MatModel.ToString()    'AES 17MAR17

                'grdProjectData.Rows(i).Cells(17).Value = modMain.gIPE_Project.Analysis(i).Seal.Seg
                If (modMain_IPE.gIPE_Project.Analysis(i).Seal.IsSegmented = True) Then
                    grdProjectData.Rows(pRowIndx).Cells(18).Value = "Y"
                    grdProjectData.Rows(pRowIndx).Cells(19).Value = modMain_IPE.gIPE_Project.Analysis(i).Seal.CountSegment      'AES 19SEP16
                Else
                    grdProjectData.Rows(pRowIndx).Cells(18).Value = "N"
                    grdProjectData.Rows(pRowIndx).Cells(19).Value = "0"
                End If
                'grdProjectData.Rows(pRowIndx).Cells(18).Value = modMain.gIPE_Project.Analysis(i).Seal.CountSegment

                grdProjectData.Rows(pRowIndx).Cells(20).Value = modMain_IPE.gIPE_Project.Analysis(i).Seal.Mat.Name
                grdProjectData.Rows(pRowIndx).Cells(21).Value = modMain_IPE.gIPE_Project.Analysis(i).Seal.Mat.HT
                grdProjectData.Rows(pRowIndx).Cells(22).Value = modMain_IPE.gIPE_Project.Analysis(i).Seal.Mat.Coating

                If (modMain_IPE.gIPE_Project.Analysis(i).Seal.Type = "E-Seal") Then
                    grdProjectData.Rows(pRowIndx).Cells(23).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsESeal).SFinish)
                ElseIf (modMain_IPE.gIPE_Project.Analysis(i).Seal.Type = "C-Seal") Then
                    grdProjectData.Rows(pRowIndx).Cells(24).Value = CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsCSeal).Plating.Code
                End If

                If (Math.Abs(modMain_IPE.gIPE_Project.Analysis(i).Seal.ZClear_Given) > gcEPS) Then
                    grdProjectData.Rows(pRowIndx).Cells(25).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).Seal.ZClear_Given)

                Else
                    grdProjectData.Rows(pRowIndx).Cells(25).Value = gIPE_Unit.WriteInUserL(modMain_IPE.gIPE_Project.Analysis(i).Seal.ZClear_Calc(modMain_IPE.gIPE_Project.Analysis(i).Cavity.HFree_Rep))

                End If

                'grdProjectData.Rows(pRowIndx).Cells(24).Value = gIPE_Unit.WriteInUserL(modMain.gIPE_Project.Analysis(i).Seal.ZClear)

                grdProjectData.Rows(pRowIndx).Cells(26).Value = modMain_IPE.gIPE_Project.Analysis(i).Seal.Adjusted

                If (modMain_IPE.gIPE_Project.Analysis(i).Seal.Type = "E-Seal") Then
                    grdProjectData.Rows(pRowIndx).Cells(27).Value = CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsESeal).DThetaE1
                    grdProjectData.Rows(pRowIndx).Cells(28).Value = CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsESeal).DThetaM1

                ElseIf (modMain_IPE.gIPE_Project.Analysis(i).Seal.Type = "C-Seal") Then
                    grdProjectData.Rows(pRowIndx).Cells(29).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsCSeal).DHfree)
                    grdProjectData.Rows(pRowIndx).Cells(30).Value = CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsCSeal).DThetaOpening
                    grdProjectData.Rows(pRowIndx).Cells(31).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsCSeal).T, "LFormat") 'CType(modMain.gIPE_Project.Analysis(i).Seal, IPE_clsCSeal).T

                ElseIf (modMain_IPE.gIPE_Project.Analysis(i).Seal.Type = "U-Seal") Then
                    grdProjectData.Rows(pRowIndx).Cells(32).Value = CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsUSeal).DTheta(1)
                    grdProjectData.Rows(pRowIndx).Cells(33).Value = CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsUSeal).DTheta(2)

                    grdProjectData.Rows(pRowIndx).Cells(34).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsUSeal).DRad(1))
                    grdProjectData.Rows(pRowIndx).Cells(35).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsUSeal).DRad(2))
                    grdProjectData.Rows(pRowIndx).Cells(36).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsUSeal).DLLeg)

                    grdProjectData.Rows(pRowIndx).Cells(37).Value = gIPE_Unit.WriteInUserL(CType(modMain_IPE.gIPE_Project.Analysis(i).Seal, IPE_clsUSeal).T, "LFormat")
                End If

                Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)
                pLoadStep = gIPE_Project.Analysis(i).LoadStep()

                'Dim pIndex As Integer = 35
                'For j As Integer = 0 To pLoadStep.Count - 1

                '    grdProjectData.Rows(i).Cells(pIndex).Value = gIPE_Project.Analysis(i).Result_LoadStep(j).FContact
                '    pIndex = pIndex + 1
                '    grdProjectData.Rows(i).Cells(pIndex).Value = gIPE_Project.Analysis(i).Result_LoadStep(j).SigEqvMax
                '    pIndex = pIndex + 1
                'Next
                Dim pSpringBack As Integer = 0
                pSpringBack = gIPE_Project.Analysis(i).Seal.SpringBack(gIPE_Project.Analysis(i).Compression.TolType,
                                                                   gIPE_Project.Analysis(i).Cavity.DepthActual(gIPE_Project.Analysis(i).Compression.TolType),
                                                                   gIPE_Project.Analysis(i).Compression.Val)

                grdProjectData.Rows(pRowIndx).Cells(38).Value = pSpringBack 'modMain.gIPE_Project.Analysis(i).Result_Gen.HFreeFinal
                grdProjectData.Rows(pRowIndx).Cells(39).Value = modMain_IPE.gIPE_Project.Analysis(i).Result_Gen.Leakage_BL.ToString("#0.00")

                'AES 07MAR17
                '....SeatingLoad5   'BASELINE
                Dim pFSeat_Unit As Single
                Dim pstrFSeat_Unit As String = ""
                If (modMain_IPE.gIPE_Project.Analysis(i).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    If (modMain_IPE.gIPE_Project.Analysis(i).AppLoad.PreComp.Exists = True) Then
                        pFSeat_Unit = modMain_IPE.gIPE_Project.Analysis(i).Seal.FSeat_Unit(2) * gIPE_Unit.CFacUserL       '....User Unit.
                    Else
                        pFSeat_Unit = modMain_IPE.gIPE_Project.Analysis(i).Seal.FSeat_Unit(1) * gIPE_Unit.CFacUserL       '....User Unit.
                    End If
                Else
                    pFSeat_Unit = modMain_IPE.gIPE_Project.Analysis(i).Seal.FSeat_Unit(1) * gIPE_Unit.CFacUserL       '....User Unit.
                End If
                'psngFConUnit = Project_In.Analysis(gIPE_frmResults.ISel).Seal.FConUnit(5) * Unit_In.CFacUserL       '....User Unit.


                If pFSeat_Unit <= 10.0# Then
                    pstrFSeat_Unit = Format(pFSeat_Unit, "##.00")
                ElseIf pFSeat_Unit > 10.0# Then
                    pstrFSeat_Unit = Format(NInt(pFSeat_Unit), "##,##0")
                End If

                grdProjectData.Rows(pRowIndx).Cells(40).Value = pstrFSeat_Unit

                pRowIndx = pRowIndx + 1
            End If

        Next

    End Sub


    Private Sub DisplayData()
        '====================
        txtProjectName.Text = modMain_IPE.gIPE_Project.Customer()
        txtPlatform.Text = modMain_IPE.gIPE_Project.Platform()
        txtParkerPN.Text = modMain_IPE.gIPE_Project.ParkerPN()

        '....Menu
        mnuBrief.Checked = False
        mnuDetails.Checked = False
        mnuChangeFormat.Checked = False
        mnuCustom.Checked = False

        For i As Integer = 0 To grdProjectData.RowCount - 1
            'pCurRowIndex = grdProjectData.CurrentRow.Index
            Dim pIndex As Integer = grdProjectData.Rows(i).Cells(0).Value - 1
            If (gIPE_Project.Analysis(pIndex).Seal.Selected = True) Then

                For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                    grdProjectData.Rows(i).Cells(j).Style.ForeColor = Color.Blue
                Next
            Else
                For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                    grdProjectData.Rows(i).Cells(j).Style.ForeColor = Color.Black
                Next
            End If
        Next

    End Sub


#End Region


#Region "MENU EVENT ROUTINES:"


    Private Sub mnuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                              Handles mnuBrief.Click, mnuDetails.Click, mnuChangeFormat.Click, mnuCustom.Click
        '=======================================================================================================
        Dim pMenuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)

        Select Case pMenuItem.Text

            Case "&Brief"
                SetDataGridControl_BRIEF()

            Case "&Details"
                SetDataGridControl_DETAILS()

            Case "&Change Format"

            Case "&Custom"
                Dim pfrmCustomResults As New IPE_frmCustomResults()
                pfrmCustomResults.ShowDialog()
                SetDataGridControl_CUSTOM()
                PopulateDataGrid()

        End Select

    End Sub

#Region "HELPER-ROUTINES:"

    Private Sub SetDataGridControl_BRIEF()
        '=================================                                   
        'This subroutine arranges the datagrid columns.

        mnuBrief.Checked = True
        mnuDetails.Checked = False
        mnuCustom.Checked = False
        mnuOptions.Text = "View Options (B)"

        Const pcColCount As Integer = 28

        For i As Integer = 0 To pcColCount
            If ((i >= 3 And i <= 5) Or (i >= 9 And i <= 13) Or i = 18 Or i = 27 Or i = 28) Then
                grdProjectData.Columns(i).Visible = False
            End If
        Next

    End Sub


    Private Sub SetDataGridControl_DETAILS()
        '===================================                 

        mnuBrief.Checked = False
        mnuDetails.Checked = True
        mnuCustom.Checked = False
        mnuOptions.Text = "View Options (D)"

        Const pcColCount As Integer = 28

        For i As Integer = 0 To pcColCount
            grdProjectData.Columns(i).Visible = True
        Next

    End Sub


    Private Sub SetDataGridControl_CUSTOM()
        '===================================               
        mnuBrief.Checked = False
        mnuDetails.Checked = False
        mnuCustom.Checked = True
        mnuOptions.Text = "View Options (C)"

        grdProjectData.Columns.Clear()
        grdProjectData.Refresh()

        Const pcColCount As Integer = 40 '39 '38 '55        'AES 17MAR17
        Dim pTextColumn(pcColCount) As DataGridViewTextBoxColumn
        Dim pCmdColumns As New DataGridViewButtonColumn
        For j As Integer = 0 To pcColCount
            pTextColumn(j) = New DataGridViewTextBoxColumn()
            If (j = 14) Then
                grdProjectData.Columns.Add(pCmdColumns)
            Else
                grdProjectData.Columns.Add(pTextColumn(j))

                With pTextColumn(j)
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.BackColor = Color.White
                End With
            End If

        Next

        With pTextColumn(0)
            .HeaderText = "No."
            .Width = 55
            .Visible = True 'My.Settings.MCS
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(1)
            .HeaderText = "MCS"
            .Width = 55
            .Visible = My.Settings.MCS
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(2)
            .HeaderText = "HFree" & vbCrLf & "(" & gIPE_Unit.UserL & ")"
            .Width = 50
            .Visible = My.Settings.HFree
            .DefaultCellStyle.Format = gIPE_Unit.LFormat
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(3)
            .HeaderText = "Date Created"
            .Width = 75
            .Visible = My.Settings.DateCreated
            .DefaultCellStyle.Format = "d"
            .SortMode = DataGridViewColumnSortMode.Automatic
        End With

        With pTextColumn(4)
            .HeaderText = "Time Created"
            .Width = 75
            .Visible = My.Settings.TimeCreated
            .DefaultCellStyle.Format = "t"
            .SortMode = DataGridViewColumnSortMode.Automatic
        End With

        With pTextColumn(5)
            .HeaderText = "Cavity OD"
            .Width = 50
            .Visible = My.Settings.CavityOD
            .DefaultCellStyle.Format = gIPE_Unit.LFormat
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(6)
            .HeaderText = "Cavity ID"
            .Width = 50
            .Visible = My.Settings.CavityID
            .DefaultCellStyle.Format = gIPE_Unit.LFormat
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(7)
            .HeaderText = "Cavity Depth" & "  (" & gIPE_Unit.UserL & ")"
            .Width = 70
            .Visible = My.Settings.CavityDepth
            .DefaultCellStyle.Format = gIPE_Unit.LFormat
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(8)
            .HeaderText = "Press" & vbCrLf & "(" & gIPE_Unit.UserP & ")"
            .Width = 70
            .Visible = My.Settings.Press
            .DefaultCellStyle.Format = "#0.0"
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(9)
            .HeaderText = "TOper" & vbCrLf & "(" & gIPE_Unit.T & ")"
            .Width = 50
            .Visible = My.Settings.TOper
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(10)
            .HeaderText = "POrient"
            .Width = 70
            .Visible = My.Settings.POrient
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(11)
            .HeaderText = "Pre- Compressed"
            .Width = 90
            .Visible = My.Settings.PreCompressed
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(12)
            .HeaderText = "Pre-Compressed HMin"
            .Width = 120
            .Visible = My.Settings.PreCompressedH
            .DefaultCellStyle.Format = gIPE_Unit.LFormat
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(13)
            .HeaderText = "Radial Constraint"
            .Width = 75
            .Visible = My.Settings.RadCon
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pCmdColumns
            .HeaderText = "Load Case Type"
            .Width = 85
            .Visible = My.Settings.LoadCaseType
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(15)
            .HeaderText = "Compress Tol. Type"
            .Width = 100
            .Visible = My.Settings.CompressTolType
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(16)
            .HeaderText = "Name"
            .Width = 100
            .Visible = My.Settings.Name
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(17)
            .HeaderText = "Mat Model"
            .Width = 80
            .Visible = My.Settings.MatModel
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(18)
            .HeaderText = "Segmented"
            .Width = 80
            .Visible = My.Settings.Seg
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(19)
            .HeaderText = "Segment Count"
            .Width = 70
            .Visible = My.Settings.SegCount
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(20)
            .HeaderText = "Material Name"
            .Width = 60
            .Visible = My.Settings.MatName
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(21)
            .HeaderText = "Heat Treatment"
            .Width = 80
            .Visible = My.Settings.HT
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(22)
            .HeaderText = "Coating"
            .Width = 60
            .Visible = My.Settings.Coating
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With


        With pTextColumn(23)
            .DataPropertyName = "fldSFinish"
            .HeaderText = "SFinish" & " (rms)"
            .Width = 60
            If gIPE_Project.SealType = "E-Seal" Then
                .Visible = My.Settings.SFinish
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With


        With pTextColumn(24)
            .HeaderText = "Plating"
            .Width = 60
            If gIPE_Project.SealType = "C-Seal" Then
                .Visible = My.Settings.Plating
            Else
                .Visible = False
            End If

            '.DefaultCellStyle.Format = gIPE_Unit.LFormat
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With


        With pTextColumn(25)
            .HeaderText = "ZClear"
            .Width = 60
            .Visible = My.Settings.ZClear
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        'With pTextColumn(24)
        '    .HeaderText = "DT"
        '    .Width = 60
        '    .Visible = My.Settings.DT_CSeal
        'End With

        With pTextColumn(26)
            .HeaderText = "Adjusted"
            .Width = 70
            .Visible = My.Settings.Adjusted
            '.DefaultCellStyle.Format = "##,##0"
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(27)
            .HeaderText = "DThetaE1"
            .Width = 70
            If gIPE_Project.SealType = "E-Seal" Then
                .Visible = My.Settings.DThetaE1
            Else
                .Visible = False
            End If

            '.DefaultCellStyle.Format = "##,##0"
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(28)
            .HeaderText = "DThetaM1"
            .Width = 70
            If gIPE_Project.SealType = "E-Seal" Then
                .Visible = My.Settings.DThetaM1
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(29)
            .HeaderText = "DHFree"
            .Width = 70
            If gIPE_Project.SealType = "C-Seal" Then
                .Visible = My.Settings.DHFree
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(30)
            .HeaderText = "DThetaOpening"
            .Width = 70
            If gIPE_Project.SealType = "C-Seal" Then
                .Visible = My.Settings.DThetaOpening
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(31)
            .HeaderText = "DT"
            .Width = 60
            '.Visible = My.Settings.DT_CSeal
            If gIPE_Project.SealType = "C-Seal" Then
                .Visible = My.Settings.DT_CSeal
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With


        With pTextColumn(32)
            .HeaderText = "DTheta1"
            .Width = 70
            If gIPE_Project.SealType = "U-Seal" Then
                .Visible = My.Settings.DTheta1
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(33)
            .HeaderText = "DTheta2"
            .Width = 70
            If gIPE_Project.SealType = "U-Seal" Then
                .Visible = My.Settings.DTheta2
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(34)
            .HeaderText = "DRad1"
            .Width = 70
            If gIPE_Project.SealType = "U-Seal" Then
                .Visible = My.Settings.DRad1
            Else
                .Visible = False

            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(35)
            .HeaderText = "DRad2"
            .Width = 70
            If gIPE_Project.SealType = "U-Seal" Then
                .Visible = My.Settings.DRad2
            Else
                .Visible = False

            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(36)
            .HeaderText = "DLLeg"
            .Width = 70
            If gIPE_Project.SealType = "U-Seal" Then
                .Visible = My.Settings.DLLeg
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(37)
            .HeaderText = "DT"
            .Width = 60
            '.Visible = My.Settings.DT_CSeal
            If gIPE_Project.SealType = "U-Seal" Then
                .Visible = My.Settings.DT_USeal
            Else
                .Visible = False
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With pTextColumn(38)
            .HeaderText = "Spring Back"
            .Width = 80
            .Visible = My.Settings.SpringBack
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With


        With pTextColumn(39)
            .HeaderText = "Leakage BL"
            .Width = 80
            .Visible = My.Settings.Leakage_BL
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        'AES 07MAR17
        With pTextColumn(40)
            .HeaderText = "Unit Seating Load"
            .Width = 80
            .Visible = My.Settings.UnitSeatLoad
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        With grdProjectData
            .ReadOnly = True
            .AutoGenerateColumns = False
            .AllowUserToAddRows = False
        End With

    End Sub


#End Region


#End Region


#Region "CHECK BOX RELATED ROUTINE:"

    '....AES 08MAR16
    Private Sub chkSel_Project_CheckedChanged(sender As System.Object,
                                              e As System.EventArgs) Handles chkSel_Project.CheckedChanged
        '===================================================================================================

        Dim pCurRowIndex As Integer

        If (chkSel_Project.Checked) Then
            If (Not IsNothing(grdProjectData.CurrentRow)) Then
                pCurRowIndex = grdProjectData.CurrentRow.Index
                Dim pIndex As Integer = grdProjectData.Rows(pCurRowIndex).Cells(0).Value - 1

                gIPE_Project.Analysis(pIndex).Seal.Selected = True
            End If
            For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                grdProjectData.Rows(pCurRowIndex).Cells(j).Style.ForeColor = Color.Blue
            Next

            For i As Integer = 0 To grdProjectData.RowCount - 1
                If (i <> grdProjectData.CurrentRow.Index) Then
                    Dim pIndex As Integer = grdProjectData.Rows(i).Cells(0).Value - 1
                    gIPE_Project.Analysis(pIndex).Seal.Selected = False
                    For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                        grdProjectData.Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next
                End If
            Next

        Else
            pCurRowIndex = grdProjectData.CurrentRow.Index
            Dim pIndex As Integer = grdProjectData.Rows(pCurRowIndex).Cells(0).Value - 1

            gIPE_Project.Analysis(pIndex).Seal.Selected = False
            For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                grdProjectData.Rows(pCurRowIndex).Cells(j).Style.ForeColor = Color.Black
            Next
        End If
    End Sub

#End Region


#Region "GRIDVIEW EVENT RELATED ROUTINES:"

    Private Sub grdProjectData_CellContentClick(sender As System.Object,
                                                e As DataGridViewCellEventArgs) _
                                                Handles grdProjectData.CellContentClick
        '=================================================================================

        Dim senderGrid = DirectCast(sender, DataGridView)
        If TypeOf senderGrid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn AndAlso e.RowIndex >= 0 Then
            RaiseEvent grdProjectButtonClick(senderGrid, e)
        End If

    End Sub

    Private Sub grdProject_ButtonClick(sender As DataGridView,
                                       e As DataGridViewCellEventArgs) Handles Me.grdProjectButtonClick
        '================================================================================================
        Dim pCurIndex As Integer = grdProjectData.CurrentRow.Index
        mISel = grdProjectData.Rows(pCurIndex).Cells(0).Value - 1
        Dim pfrmLoadStep As New IPE_frmLoadStep(mISel)
        pfrmLoadStep.ShowDialog()

    End Sub

    '....AES 08MAR16
    Private Sub grdProjectData_RowHeaderMouseClick(sender As System.Object,
                                                   e As DataGridViewCellMouseEventArgs) Handles grdProjectData.RowHeaderMouseClick
        '==========================================================================================================================
        Analysis_Desc()

        If (Not IsNothing(grdProjectData.CurrentRow)) Then
            Dim pIndex As Integer = grdProjectData.Rows(grdProjectData.CurrentRow.Index).Cells(0).Value - 1
            If (gIPE_Project.Analysis(pIndex).Seal.Selected = True) Then
                chkSel_Project.Checked = True
                lblDesc.ForeColor = Color.Blue

                For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                    grdProjectData.Rows(grdProjectData.CurrentRow.Index).Cells(j).Style.ForeColor = Color.Blue
                Next

                For i As Integer = 0 To grdProjectData.RowCount - 1
                    If (i <> grdProjectData.CurrentRow.Index) Then

                        For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                            grdProjectData.Rows(i).Cells(j).Style.ForeColor = Color.Black
                        Next
                    End If
                Next

            Else
                chkSel_Project.Checked = False
                lblDesc.ForeColor = Color.Black

                For j As Int16 = 0 To grdProjectData.ColumnCount - 1
                    grdProjectData.Rows(grdProjectData.CurrentRow.Index).Cells(j).Style.ForeColor = Color.Black
                Next

            End If
        End If


    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                Handles cmdClose.Click, cmdSummaryTable.Click, cmdDrawings_Reports.Click
        '===============================================================================================
        Dim pCurRowIndex As Integer = grdProjectData.CurrentRow.Index

        mISel = grdProjectData.Rows(pCurRowIndex).Cells(0).Value - 1 'pCurRowIndex

        Dim pcmdButton As Button = CType(sender, Button)

        Select Case pcmdButton.Name

            Case "cmdDrawings_Reports"
                '----------------------------
                'Dim pfrmOutput As New frmOutput()
                gIPE_frmOutPut.ShowDialog()


            Case "cmdSummaryTable"
                '-----------------------
                gIPE_Report = New IPE_clsReport()
                gIPE_Report.WriteSummaryTable(gIPE_Project, mISel, gIPE_Unit, gIPE_ANSYS)


            Case "cmdClose"
                '--------------
                Me.Close()

        End Select

    End Sub

#End Region


#Region "UTILTY ROUTINES:"

    '....AES 08MAR16
    Private Sub Analysis_Desc()
        '======================
        Dim pCount As Integer = gIPE_Project.Analysis.Count

        If (pCount > 0) Then
            Dim pIndex As Integer = 0
            If (Not IsNothing(grdProjectData.CurrentRow)) Then
                pIndex = grdProjectData.CurrentRow.Index
            End If

            Dim pAnaDesc As String = ""
            'Dim pISel As Integer = gIPE_frmResults.ISel
            Dim pMCS As String = gIPE_Project.Analysis(pIndex).Seal.MCrossSecNo
            If (pMCS <> "") Then
                pAnaDesc = "MCS" & pMCS
            End If
            Dim pFreeHt As String = gIPE_Project.Analysis(pIndex).Seal.Hfree.ToString("#0.000").Replace(".", "")
            If (pFreeHt <> "") Then
                pAnaDesc = pAnaDesc & "_FH" & pFreeHt
            End If
            Dim pLoadCaseName As String = gIPE_Project.Analysis(pIndex).LoadCase.Name
            If (pLoadCaseName <> "") Then
                pAnaDesc = pAnaDesc & "_" & pLoadCaseName
            End If
            Dim pDate As String = ""
            Dim pTime As String = ""
            If (gIPE_Project.Analysis(pIndex).DateCreated <> Date.MinValue) Then
                pDate = gIPE_Project.Analysis(pIndex).DateCreated.ToString("ddMMMyy")
                pTime = gIPE_Project.Analysis(pIndex).TimeCreated.ToString("t").Replace(":", "").Trim().Replace(" ", "")
                pAnaDesc = pAnaDesc & "_" & pDate & "_" & pTime
            End If

            lblDesc.Text = pAnaDesc
        Else
            lblDesc.Text = ""
        End If

    End Sub

#End Region


    Private Overloads Sub Dispose()
        '==========================
        'mDataGridTableStyle = Nothing
        mTextColumn = Nothing
    End Sub





End Class
