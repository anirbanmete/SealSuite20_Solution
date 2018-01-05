'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmAdjGeomESeal                        '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  11MAR16                                '
'                                                                              '
'===============================================================================
Imports System.Math
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports clsLibrary11


Public Class IPE_frmAdjGeomESeal
    Inherits System.Windows.Forms.Form


#Region "MEMBER VARIABLES:"

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single
    Private mESeal As IPE_clsESeal        '....Local Seal object.  

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
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents lblFreeHeight As System.Windows.Forms.Label
    Friend WithEvents lblStandard As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents txtESealTemplateNo As System.Windows.Forms.TextBox
    Public WithEvents txtESealStageNo As System.Windows.Forms.TextBox
    Friend WithEvents lblTemplate As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents txtESealNConv As System.Windows.Forms.TextBox
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblESealThetaM1 As System.Windows.Forms.Label
    Friend WithEvents lblESealThetaE1 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents lblPlusE1 As System.Windows.Forms.Label
    Public WithEvents lblPlusM1 As System.Windows.Forms.Label
    Friend WithEvents lblESealAdjusted As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo As System.Windows.Forms.TextBox
    Public WithEvents txtPOrient As System.Windows.Forms.TextBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Public WithEvents txtT As System.Windows.Forms.TextBox
    Public WithEvents txtESealDThetaE1 As System.Windows.Forms.TextBox
    Public WithEvents txtESealDThetaM1 As System.Windows.Forms.TextBox
    Public WithEvents txtESealThetaE1 As System.Windows.Forms.TextBox
    Public WithEvents txtESealThetaM1 As System.Windows.Forms.TextBox
    Friend WithEvents updDThetaM1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents updDThetaE1 As System.Windows.Forms.NumericUpDown
    Public WithEvents picSeal As System.Windows.Forms.PictureBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCreateWordDocu As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdViewNomenclature As System.Windows.Forms.Button
    Friend WithEvents cmdDXF As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmAdjGeomESeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdDXF = New System.Windows.Forms.Button()
        Me.cmdViewNomenclature = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.lblStandard = New System.Windows.Forms.Label()
        Me.lblESealAdjusted = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.updDThetaM1 = New System.Windows.Forms.NumericUpDown()
        Me.updDThetaE1 = New System.Windows.Forms.NumericUpDown()
        Me.txtESealDThetaM1 = New System.Windows.Forms.TextBox()
        Me.txtESealDThetaE1 = New System.Windows.Forms.TextBox()
        Me.lblPlusM1 = New System.Windows.Forms.Label()
        Me.lblPlusE1 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtESealThetaE1 = New System.Windows.Forms.TextBox()
        Me.txtESealThetaM1 = New System.Windows.Forms.TextBox()
        Me.lblESealThetaM1 = New System.Windows.Forms.Label()
        Me.lblESealThetaE1 = New System.Windows.Forms.Label()
        Me.txtPOrient = New System.Windows.Forms.TextBox()
        Me.txtDControl = New System.Windows.Forms.TextBox()
        Me.txtESealNConv = New System.Windows.Forms.TextBox()
        Me.txtT = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTemplate = New System.Windows.Forms.Label()
        Me.txtESealTemplateNo = New System.Windows.Forms.TextBox()
        Me.txtESealStageNo = New System.Windows.Forms.TextBox()
        Me.txtCrossSecNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblFreeHeight = New System.Windows.Forms.Label()
        Me.picSeal = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCreateWordDocu = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Panel1.SuspendLayout()
        CType(Me.updDThetaM1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updDThetaE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 24)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(700, 612)
        Me.lblBorder.TabIndex = 0
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmdDXF)
        Me.Panel1.Controls.Add(Me.cmdViewNomenclature)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.lblStandard)
        Me.Panel1.Controls.Add(Me.lblESealAdjusted)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.updDThetaM1)
        Me.Panel1.Controls.Add(Me.updDThetaE1)
        Me.Panel1.Controls.Add(Me.txtESealDThetaM1)
        Me.Panel1.Controls.Add(Me.txtESealDThetaE1)
        Me.Panel1.Controls.Add(Me.lblPlusM1)
        Me.Panel1.Controls.Add(Me.lblPlusE1)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.txtESealThetaE1)
        Me.Panel1.Controls.Add(Me.txtESealThetaM1)
        Me.Panel1.Controls.Add(Me.lblESealThetaM1)
        Me.Panel1.Controls.Add(Me.lblESealThetaE1)
        Me.Panel1.Controls.Add(Me.txtPOrient)
        Me.Panel1.Controls.Add(Me.txtDControl)
        Me.Panel1.Controls.Add(Me.txtESealNConv)
        Me.Panel1.Controls.Add(Me.txtT)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblTemplate)
        Me.Panel1.Controls.Add(Me.txtESealTemplateNo)
        Me.Panel1.Controls.Add(Me.txtESealStageNo)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblFreeHeight)
        Me.Panel1.Controls.Add(Me.picSeal)
        Me.Panel1.Location = New System.Drawing.Point(4, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(698, 610)
        Me.Panel1.TabIndex = 1
        '
        'cmdDXF
        '
        Me.cmdDXF.BackColor = System.Drawing.Color.Silver
        Me.cmdDXF.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDXF.Image = CType(resources.GetObject("cmdDXF.Image"), System.Drawing.Image)
        Me.cmdDXF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDXF.Location = New System.Drawing.Point(11, 566)
        Me.cmdDXF.Name = "cmdDXF"
        Me.cmdDXF.Size = New System.Drawing.Size(140, 28)
        Me.cmdDXF.TabIndex = 183
        Me.cmdDXF.Text = "     Export To DXF"
        Me.cmdDXF.UseVisualStyleBackColor = False
        '
        'cmdViewNomenclature
        '
        Me.cmdViewNomenclature.BackColor = System.Drawing.Color.Silver
        Me.cmdViewNomenclature.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewNomenclature.Image = CType(resources.GetObject("cmdViewNomenclature.Image"), System.Drawing.Image)
        Me.cmdViewNomenclature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdViewNomenclature.Location = New System.Drawing.Point(178, 566)
        Me.cmdViewNomenclature.Name = "cmdViewNomenclature"
        Me.cmdViewNomenclature.Size = New System.Drawing.Size(140, 28)
        Me.cmdViewNomenclature.TabIndex = 182
        Me.cmdViewNomenclature.Text = "     View Nomenclature"
        Me.cmdViewNomenclature.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(514, 574)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 105
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblStandard
        '
        Me.lblStandard.BackColor = System.Drawing.Color.White
        Me.lblStandard.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStandard.Location = New System.Drawing.Point(346, 18)
        Me.lblStandard.Name = "lblStandard"
        Me.lblStandard.Size = New System.Drawing.Size(310, 15)
        Me.lblStandard.TabIndex = 2
        Me.lblStandard.Text = "Standard"
        '
        'lblESealAdjusted
        '
        Me.lblESealAdjusted.BackColor = System.Drawing.Color.White
        Me.lblESealAdjusted.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblESealAdjusted.Location = New System.Drawing.Point(346, 34)
        Me.lblESealAdjusted.Name = "lblESealAdjusted"
        Me.lblESealAdjusted.Size = New System.Drawing.Size(310, 14)
        Me.lblESealAdjusted.TabIndex = 104
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(610, 574)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 100
        Me.cmdCancel.Text = "   &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'updDThetaM1
        '
        Me.updDThetaM1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDThetaM1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDThetaM1.Location = New System.Drawing.Point(640, 456)
        Me.updDThetaM1.Name = "updDThetaM1"
        Me.updDThetaM1.Size = New System.Drawing.Size(20, 20)
        Me.updDThetaM1.TabIndex = 98
        '
        'updDThetaE1
        '
        Me.updDThetaE1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDThetaE1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDThetaE1.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updDThetaE1.Location = New System.Drawing.Point(640, 430)
        Me.updDThetaE1.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updDThetaE1.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updDThetaE1.Name = "updDThetaE1"
        Me.updDThetaE1.Size = New System.Drawing.Size(20, 20)
        Me.updDThetaE1.TabIndex = 97
        '
        'txtESealDThetaM1
        '
        Me.txtESealDThetaM1.AcceptsReturn = True
        Me.txtESealDThetaM1.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealDThetaM1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealDThetaM1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealDThetaM1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtESealDThetaM1.Location = New System.Drawing.Point(587, 457)
        Me.txtESealDThetaM1.MaxLength = 0
        Me.txtESealDThetaM1.Name = "txtESealDThetaM1"
        Me.txtESealDThetaM1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealDThetaM1.Size = New System.Drawing.Size(48, 21)
        Me.txtESealDThetaM1.TabIndex = 95
        Me.txtESealDThetaM1.Text = "0.000"
        Me.txtESealDThetaM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtESealDThetaE1
        '
        Me.txtESealDThetaE1.AcceptsReturn = True
        Me.txtESealDThetaE1.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealDThetaE1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealDThetaE1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealDThetaE1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtESealDThetaE1.Location = New System.Drawing.Point(587, 430)
        Me.txtESealDThetaE1.MaxLength = 0
        Me.txtESealDThetaE1.Name = "txtESealDThetaE1"
        Me.txtESealDThetaE1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealDThetaE1.Size = New System.Drawing.Size(48, 21)
        Me.txtESealDThetaE1.TabIndex = 94
        Me.txtESealDThetaE1.Text = "0.000"
        Me.txtESealDThetaE1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPlusM1
        '
        Me.lblPlusM1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblPlusM1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPlusM1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlusM1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPlusM1.Location = New System.Drawing.Point(566, 461)
        Me.lblPlusM1.Name = "lblPlusM1"
        Me.lblPlusM1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlusM1.Size = New System.Drawing.Size(15, 16)
        Me.lblPlusM1.TabIndex = 92
        Me.lblPlusM1.Text = "+"
        Me.lblPlusM1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPlusE1
        '
        Me.lblPlusE1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblPlusE1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPlusE1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlusE1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPlusE1.Location = New System.Drawing.Point(566, 434)
        Me.lblPlusE1.Name = "lblPlusE1"
        Me.lblPlusE1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlusE1.Size = New System.Drawing.Size(15, 16)
        Me.lblPlusE1.TabIndex = 91
        Me.lblPlusE1.Text = "+"
        Me.lblPlusE1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(418, 392)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(127, 16)
        Me.Label9.TabIndex = 90
        Me.Label9.Text = "Adjust  Geometry :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtESealThetaE1
        '
        Me.txtESealThetaE1.AcceptsReturn = True
        Me.txtESealThetaE1.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealThetaE1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealThetaE1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealThetaE1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtESealThetaE1.Location = New System.Drawing.Point(492, 430)
        Me.txtESealThetaE1.MaxLength = 0
        Me.txtESealThetaE1.Name = "txtESealThetaE1"
        Me.txtESealThetaE1.ReadOnly = True
        Me.txtESealThetaE1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealThetaE1.Size = New System.Drawing.Size(66, 21)
        Me.txtESealThetaE1.TabIndex = 89
        Me.txtESealThetaE1.TabStop = False
        Me.txtESealThetaE1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtESealThetaM1
        '
        Me.txtESealThetaM1.AcceptsReturn = True
        Me.txtESealThetaM1.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealThetaM1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealThetaM1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealThetaM1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtESealThetaM1.Location = New System.Drawing.Point(492, 457)
        Me.txtESealThetaM1.MaxLength = 0
        Me.txtESealThetaM1.Name = "txtESealThetaM1"
        Me.txtESealThetaM1.ReadOnly = True
        Me.txtESealThetaM1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealThetaM1.Size = New System.Drawing.Size(66, 21)
        Me.txtESealThetaM1.TabIndex = 87
        Me.txtESealThetaM1.TabStop = False
        Me.txtESealThetaM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblESealThetaM1
        '
        Me.lblESealThetaM1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblESealThetaM1.Location = New System.Drawing.Point(396, 461)
        Me.lblESealThetaM1.Name = "lblESealThetaM1"
        Me.lblESealThetaM1.Size = New System.Drawing.Size(90, 14)
        Me.lblESealThetaM1.TabIndex = 83
        Me.lblESealThetaM1.Text = "Conv Arc"
        Me.lblESealThetaM1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblESealThetaE1
        '
        Me.lblESealThetaE1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblESealThetaE1.Location = New System.Drawing.Point(396, 432)
        Me.lblESealThetaE1.Name = "lblESealThetaE1"
        Me.lblESealThetaE1.Size = New System.Drawing.Size(90, 14)
        Me.lblESealThetaE1.TabIndex = 80
        Me.lblESealThetaE1.Text = "Leg Conv Arc"
        Me.lblESealThetaE1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPOrient
        '
        Me.txtPOrient.AcceptsReturn = True
        Me.txtPOrient.BackColor = System.Drawing.SystemColors.Window
        Me.txtPOrient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPOrient.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPOrient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPOrient.Location = New System.Drawing.Point(104, 430)
        Me.txtPOrient.MaxLength = 0
        Me.txtPOrient.Name = "txtPOrient"
        Me.txtPOrient.ReadOnly = True
        Me.txtPOrient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPOrient.Size = New System.Drawing.Size(80, 21)
        Me.txtPOrient.TabIndex = 69
        Me.txtPOrient.TabStop = False
        Me.txtPOrient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDControl
        '
        Me.txtDControl.AcceptsReturn = True
        Me.txtDControl.BackColor = System.Drawing.SystemColors.Window
        Me.txtDControl.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDControl.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDControl.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDControl.Location = New System.Drawing.Point(104, 452)
        Me.txtDControl.MaxLength = 0
        Me.txtDControl.Name = "txtDControl"
        Me.txtDControl.ReadOnly = True
        Me.txtDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDControl.Size = New System.Drawing.Size(80, 21)
        Me.txtDControl.TabIndex = 68
        Me.txtDControl.TabStop = False
        Me.txtDControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtESealNConv
        '
        Me.txtESealNConv.AcceptsReturn = True
        Me.txtESealNConv.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealNConv.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealNConv.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealNConv.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtESealNConv.Location = New System.Drawing.Point(104, 474)
        Me.txtESealNConv.MaxLength = 0
        Me.txtESealNConv.Name = "txtESealNConv"
        Me.txtESealNConv.ReadOnly = True
        Me.txtESealNConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealNConv.Size = New System.Drawing.Size(80, 21)
        Me.txtESealNConv.TabIndex = 67
        Me.txtESealNConv.TabStop = False
        Me.txtESealNConv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtT
        '
        Me.txtT.AcceptsReturn = True
        Me.txtT.BackColor = System.Drawing.SystemColors.Window
        Me.txtT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtT.Location = New System.Drawing.Point(104, 496)
        Me.txtT.MaxLength = 0
        Me.txtT.Name = "txtT"
        Me.txtT.ReadOnly = True
        Me.txtT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtT.Size = New System.Drawing.Size(80, 21)
        Me.txtT.TabIndex = 66
        Me.txtT.TabStop = False
        Me.txtT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 498)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(90, 14)
        Me.Label8.TabIndex = 61
        Me.Label8.Text = "Thickness"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 476)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(90, 14)
        Me.Label7.TabIndex = 60
        Me.Label7.Text = "No.  Of  Conv"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 454)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(90, 14)
        Me.Label6.TabIndex = 59
        Me.Label6.Text = "Control  Dia"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 432)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 14)
        Me.Label5.TabIndex = 58
        Me.Label5.Text = "Press. Orient "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(188, 380)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 57
        Me.Label3.Text = "Stage"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label3.Visible = False
        '
        'lblTemplate
        '
        Me.lblTemplate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemplate.Location = New System.Drawing.Point(230, 380)
        Me.lblTemplate.Name = "lblTemplate"
        Me.lblTemplate.Size = New System.Drawing.Size(60, 13)
        Me.lblTemplate.TabIndex = 56
        Me.lblTemplate.Text = "Template"
        Me.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtESealTemplateNo
        '
        Me.txtESealTemplateNo.AcceptsReturn = True
        Me.txtESealTemplateNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealTemplateNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealTemplateNo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealTemplateNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtESealTemplateNo.Location = New System.Drawing.Point(237, 397)
        Me.txtESealTemplateNo.MaxLength = 0
        Me.txtESealTemplateNo.Name = "txtESealTemplateNo"
        Me.txtESealTemplateNo.ReadOnly = True
        Me.txtESealTemplateNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealTemplateNo.Size = New System.Drawing.Size(45, 21)
        Me.txtESealTemplateNo.TabIndex = 55
        Me.txtESealTemplateNo.TabStop = False
        Me.txtESealTemplateNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtESealStageNo
        '
        Me.txtESealStageNo.AcceptsReturn = True
        Me.txtESealStageNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtESealStageNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtESealStageNo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtESealStageNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtESealStageNo.Location = New System.Drawing.Point(192, 397)
        Me.txtESealStageNo.MaxLength = 0
        Me.txtESealStageNo.Name = "txtESealStageNo"
        Me.txtESealStageNo.ReadOnly = True
        Me.txtESealStageNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtESealStageNo.Size = New System.Drawing.Size(32, 21)
        Me.txtESealStageNo.TabIndex = 54
        Me.txtESealStageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtESealStageNo.Visible = False
        '
        'txtCrossSecNo
        '
        Me.txtCrossSecNo.AcceptsReturn = True
        Me.txtCrossSecNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCrossSecNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCrossSecNo.Location = New System.Drawing.Point(104, 397)
        Me.txtCrossSecNo.MaxLength = 0
        Me.txtCrossSecNo.Name = "txtCrossSecNo"
        Me.txtCrossSecNo.ReadOnly = True
        Me.txtCrossSecNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCrossSecNo.Size = New System.Drawing.Size(80, 21)
        Me.txtCrossSecNo.TabIndex = 53
        Me.txtCrossSecNo.TabStop = False
        Me.txtCrossSecNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(120, 380)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Number"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 400)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Cross Sec."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFreeHeight
        '
        Me.lblFreeHeight.BackColor = System.Drawing.Color.White
        Me.lblFreeHeight.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFreeHeight.Location = New System.Drawing.Point(231, 18)
        Me.lblFreeHeight.Name = "lblFreeHeight"
        Me.lblFreeHeight.Size = New System.Drawing.Size(100, 16)
        Me.lblFreeHeight.TabIndex = 1
        Me.lblFreeHeight.Text = "Free  Height  :"
        Me.lblFreeHeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'picSeal
        '
        Me.picSeal.BackColor = System.Drawing.Color.White
        Me.picSeal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picSeal.Location = New System.Drawing.Point(13, 8)
        Me.picSeal.Name = "picSeal"
        Me.picSeal.Size = New System.Drawing.Size(667, 371)
        Me.picSeal.TabIndex = 0
        Me.picSeal.TabStop = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOptions})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(706, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCreateWordDocu})
        Me.mnuOptions.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.Size = New System.Drawing.Size(62, 20)
        Me.mnuOptions.Text = "Options"
        Me.mnuOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mnuCreateWordDocu
        '
        Me.mnuCreateWordDocu.Image = CType(resources.GetObject("mnuCreateWordDocu.Image"), System.Drawing.Image)
        Me.mnuCreateWordDocu.Name = "mnuCreateWordDocu"
        Me.mnuCreateWordDocu.Size = New System.Drawing.Size(203, 22)
        Me.mnuCreateWordDocu.Text = "Create Word Document"
        '
        'IPE_frmAdjGeomESeal
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(706, 638)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "IPE_frmAdjGeomESeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Adjust Geometry - ESeal"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.updDThetaM1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updDThetaE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub New()
        '===========

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub


#Region "FORM EVENT ROUTINES:"

    Private Sub frmAdjGeomESeal_Activated(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) _
                                         Handles MyBase.Activated
        '================================================================== 
        If Not gIPE_frmNomenclature_AdjGeom Is Nothing Then
            cmdViewNomenclature.Enabled = gIPE_frmNomenclature_AdjGeom.FormClose
        End If

    End Sub

    Private Sub frmAdjGeomESeal_Load(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) Handles MyBase.Load
        '============================================================================

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
        mESeal = New IPE_clsESeal("E-Seal", gPartUnit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        mESeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone

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

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            txtESealDThetaE1.Enabled = False
            updDThetaE1.Enabled = False

            txtESealDThetaM1.Enabled = False
            updDThetaM1.Enabled = False
        Else
            txtESealDThetaE1.Enabled = True
            updDThetaE1.Enabled = True

            txtESealDThetaM1.Enabled = True
            updDThetaM1.Enabled = True
        End If

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
            txtDControl.Text = gPartUnit.WriteInUserL(.DControl)
            txtESealNConv.Text = .NConv
            txtT.Text = gPartUnit.WriteInUserL(.T)

            '....Adjusted parameter values:
            txtESealDThetaE1.Text = Format(.DThetaE1, "##0.000")
            txtESealDThetaM1.Text = Format(.DThetaM1, "##0.000")

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
            txtDControl.Text = gPartUnit.WriteInUserL(.DControl)    '....Update display.
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
            txtDControl.Text = gPartUnit.WriteInUserL(.DControl)    '....Update display.
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
        Dim pCulture = gIPE_Project.CultureName

        Select Case pCulture
            Case "USA", "UK"
                If e.KeyChar = "," Then e.KeyChar = "."
            Case "Germany", "France"
                If e.KeyChar = "." Then e.KeyChar = ","
        End Select

    End Sub


#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"


    Private Sub cmdDXF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                             Handles cmdDXF.Click
        '===========================================================================

        'Resolves SealIPE80Beta1_DR ERROR # 7.
        If Not gIPE_frmNomenclature_AdjGeom Is Nothing Then
            If Not gIPE_frmNomenclature_AdjGeom.FormClose Then
                gIPE_frmNomenclature_AdjGeom.SendToBack()
            End If
        End If

        SaveData()
        Me.Cursor = Cursors.WaitCursor

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal)

            SaveFileDialog1.FilterIndex = 1
            SaveFileDialog1.Filter = "Configuration files (*.DXF)|*.DXF"
            SaveFileDialog1.Title = "Save"
            SaveFileDialog1.FileName = ExtractPreData(gIPE_File.In_Title, ".") & "_" & _
                                       .MCrossSecNo & ".DXF"

            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                Dim pFileName As String
                pFileName = SaveFileDialog1.FileName
                .Create_DXF(pFileName)

                Me.Cursor = Cursors.Default
                '....Message.                   
                Dim pMsg As String
                pMsg = pFileName & " file has been created successfully. "
                MessageBox.Show(pMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End With

        Me.Cursor = Cursors.Default


    End Sub


    Private Sub cmdViewNomenclature_Click(ByVal sender As System.Object, _
                             ByVal e As System.EventArgs) _
                             Handles cmdViewNomenclature.Click
        '===================================================================
        gIPE_frmNomenclature_AdjGeom = New IPE_frmNomenclature_AdjGeom()
        gIPE_frmNomenclature_AdjGeom.Show()

        If Not gIPE_frmNomenclature_AdjGeom Is Nothing Then
            cmdViewNomenclature.Enabled = gIPE_frmNomenclature_AdjGeom.FormClose
        End If

    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==============================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()

        If Not gIPE_frmNomenclature_AdjGeom Is Nothing Then
            If Not gIPE_frmNomenclature_AdjGeom.FormClose Then
                gIPE_frmNomenclature_AdjGeom.Close()
            End If
        End If


        Me.Close()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '=================
        '....Local form data ===> Global Seal Object Data.

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal)
            .DThetaE1 = ConvertToSng(txtESealDThetaE1.Text)
            .DThetaM1 = ConvertToSng(txtESealDThetaM1.Text)

        End With
    End Sub

#End Region

#End Region

#End Region


#Region "MENU EVENT ROUTINES:"

    Private Sub mnuCreateWordDocu_Click(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) _
                                         Handles mnuCreateWordDocu.Click
        '===============================================================

        SaveData()
        gIPE_Report = New IPE_clsReport()
        gIPE_Report.CreateAdjGeomDoc(picSeal, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, gIPE_Unit, gIPE_User, gIPE_Project)

    End Sub

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
        If gPartUnit.System = "English" Then
            pGr.PageUnit = GraphicsUnit.Inch

            '....# of Pixels/in
            pDpX = pGr.DpiX
            pDpY = pGr.DpiY

        ElseIf gPartUnit.System = "Metric" Then
            pGr.PageUnit = GraphicsUnit.Millimeter

            '....# of Pixels/mm
            pDpX = pGr.DpiX / gPartUnit.EngLToUserL(1.0)
            pDpY = pGr.DpiY / gPartUnit.EngLToUserL(1.0)
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
            lblStandard.Text = "Standard  =  " & gPartUnit.WriteInUserL((mESeal.HfreeStd))

            Dim psngDelHfreePCent As Single
            psngDelHfreePCent = (mESeal.Hfree - mESeal.HfreeStd) * 100 / mESeal.HfreeStd

            If Abs(psngDelHfreePCent) <= 0.0# Then
                lblESealAdjusted.Visible = False

            ElseIf Abs(psngDelHfreePCent) > 0.0# Then
                lblESealAdjusted.Visible = True
                lblESealAdjusted.Text = "Adjusted   =  " & _
                                    gPartUnit.WriteInUserL((mESeal.Hfree)) & _
                                    "  ( " & Format(psngDelHfreePCent, "##0.0") & " %)"

            End If

        End If

    End Sub


#End Region


End Class
