
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmAdjGeomCSeal                        '
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
'Imports SealIPELib = SealIPELib101
Imports clsLibrary11


Public Class IPE_frmAdjGeomCSeal
    Inherits System.Windows.Forms.Form


#Region "MEMBER VARIABLES:"

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single
    Private mCSeal As IPE_clsCSeal        '....Local object.     

    '....Flags to indicate if the UpDown buttons have been entered by the user. 
    '........PB 22MAR07. Corrects the Error 8, DR V50.
    Private mblnUpdDHfree_Entered As Boolean
    Private mblnUpdDThetaOpening_Entered As Boolean
    Private mblnUpd_DIndexTArray_Entered As Boolean

#End Region


#Region " Windows Form Designer generated code "

    'Public Sub New()
    '    MyBase.New()

    '    'This call is required by the Windows Form Designer.
    '    InitializeComponent()

    '    'Add any initialization after the InitializeComponent() call

    'End Sub

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
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents lblCSealAdjusted As System.Windows.Forms.Label
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStandard As System.Windows.Forms.Label
    Friend WithEvents lblFreeHeight As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo As System.Windows.Forms.TextBox
    Public WithEvents txtPOrient As System.Windows.Forms.TextBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblWidStd As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblWid As System.Windows.Forms.Label
    Friend WithEvents updDThetaOpening As System.Windows.Forms.NumericUpDown
    Friend WithEvents updDHfree As System.Windows.Forms.NumericUpDown
    Public WithEvents txtHFreeStd As System.Windows.Forms.TextBox
    Public WithEvents txtTStd As System.Windows.Forms.TextBox
    Public WithEvents txtThetaOpeningStd As System.Windows.Forms.TextBox
    Public WithEvents picSeal As System.Windows.Forms.PictureBox
    Public WithEvents txtHfree As System.Windows.Forms.TextBox
    Public WithEvents txtT As System.Windows.Forms.TextBox
    Public WithEvents txtThetaOpening As System.Windows.Forms.TextBox
    Friend WithEvents lblStd As System.Windows.Forms.Label
    Friend WithEvents lblAdj As System.Windows.Forms.Label
    Public WithEvents lblErrMsg As System.Windows.Forms.Label
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCreateWordDocument As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents updDIndexTArray As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmdDXF As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmAdjGeomCSeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.lblCSealAdjusted = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.updDIndexTArray = New System.Windows.Forms.NumericUpDown()
        Me.updDThetaOpening = New System.Windows.Forms.NumericUpDown()
        Me.updDHfree = New System.Windows.Forms.NumericUpDown()
        Me.txtThetaOpening = New System.Windows.Forms.TextBox()
        Me.txtHfree = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtHFreeStd = New System.Windows.Forms.TextBox()
        Me.txtTStd = New System.Windows.Forms.TextBox()
        Me.txtThetaOpeningStd = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtPOrient = New System.Windows.Forms.TextBox()
        Me.txtDControl = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCrossSecNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblStandard = New System.Windows.Forms.Label()
        Me.lblFreeHeight = New System.Windows.Forms.Label()
        Me.picSeal = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdDXF = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblErrMsg = New System.Windows.Forms.Label()
        Me.lblAdj = New System.Windows.Forms.Label()
        Me.lblStd = New System.Windows.Forms.Label()
        Me.lblWid = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblWidStd = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtT = New System.Windows.Forms.TextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCreateWordDocument = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        CType(Me.updDIndexTArray, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updDThetaOpening, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updDHfree, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 24)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(700, 600)
        Me.lblBorder.TabIndex = 0
        Me.lblBorder.Text = "Label1"
        '
        'lblCSealAdjusted
        '
        Me.lblCSealAdjusted.BackColor = System.Drawing.Color.White
        Me.lblCSealAdjusted.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCSealAdjusted.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblCSealAdjusted.Location = New System.Drawing.Point(346, 34)
        Me.lblCSealAdjusted.Name = "lblCSealAdjusted"
        Me.lblCSealAdjusted.Size = New System.Drawing.Size(246, 14)
        Me.lblCSealAdjusted.TabIndex = 104
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(512, 561)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 101
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(608, 561)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 100
        Me.cmdCancel.Text = "   &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'updDIndexTArray
        '
        Me.updDIndexTArray.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDIndexTArray.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDIndexTArray.Location = New System.Drawing.Point(563, 474)
        Me.updDIndexTArray.Name = "updDIndexTArray"
        Me.updDIndexTArray.Size = New System.Drawing.Size(20, 20)
        Me.updDIndexTArray.TabIndex = 99
        '
        'updDThetaOpening
        '
        Me.updDThetaOpening.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDThetaOpening.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDThetaOpening.Location = New System.Drawing.Point(563, 452)
        Me.updDThetaOpening.Name = "updDThetaOpening"
        Me.updDThetaOpening.Size = New System.Drawing.Size(20, 20)
        Me.updDThetaOpening.TabIndex = 98
        '
        'updDHfree
        '
        Me.updDHfree.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDHfree.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDHfree.Increment = New Decimal(New Integer() {0, 0, 0, 0})
        Me.updDHfree.Location = New System.Drawing.Point(563, 430)
        Me.updDHfree.Name = "updDHfree"
        Me.updDHfree.Size = New System.Drawing.Size(20, 20)
        Me.updDHfree.TabIndex = 97
        Me.updDHfree.ThousandsSeparator = True
        '
        'txtThetaOpening
        '
        Me.txtThetaOpening.AcceptsReturn = True
        Me.txtThetaOpening.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaOpening.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaOpening.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaOpening.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtThetaOpening.Location = New System.Drawing.Point(587, 452)
        Me.txtThetaOpening.MaxLength = 0
        Me.txtThetaOpening.Name = "txtThetaOpening"
        Me.txtThetaOpening.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaOpening.Size = New System.Drawing.Size(48, 21)
        Me.txtThetaOpening.TabIndex = 95
        Me.txtThetaOpening.Text = "0.000"
        Me.txtThetaOpening.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtHfree
        '
        Me.txtHfree.AcceptsReturn = True
        Me.txtHfree.BackColor = System.Drawing.SystemColors.Window
        Me.txtHfree.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHfree.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHfree.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtHfree.Location = New System.Drawing.Point(587, 430)
        Me.txtHfree.MaxLength = 0
        Me.txtHfree.Name = "txtHfree"
        Me.txtHfree.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHfree.Size = New System.Drawing.Size(48, 21)
        Me.txtHfree.TabIndex = 94
        Me.txtHfree.Text = "0.000"
        Me.txtHfree.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(418, 392)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(127, 16)
        Me.Label6.TabIndex = 90
        Me.Label6.Text = "Adjust  Geometry :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtHFreeStd
        '
        Me.txtHFreeStd.AcceptsReturn = True
        Me.txtHFreeStd.BackColor = System.Drawing.Color.White
        Me.txtHFreeStd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHFreeStd.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFreeStd.ForeColor = System.Drawing.Color.Black
        Me.txtHFreeStd.Location = New System.Drawing.Point(492, 430)
        Me.txtHFreeStd.MaxLength = 0
        Me.txtHFreeStd.Name = "txtHFreeStd"
        Me.txtHFreeStd.ReadOnly = True
        Me.txtHFreeStd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHFreeStd.Size = New System.Drawing.Size(66, 21)
        Me.txtHFreeStd.TabIndex = 89
        Me.txtHFreeStd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtTStd
        '
        Me.txtTStd.AcceptsReturn = True
        Me.txtTStd.BackColor = System.Drawing.Color.White
        Me.txtTStd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTStd.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTStd.ForeColor = System.Drawing.Color.Black
        Me.txtTStd.Location = New System.Drawing.Point(492, 474)
        Me.txtTStd.MaxLength = 0
        Me.txtTStd.Name = "txtTStd"
        Me.txtTStd.ReadOnly = True
        Me.txtTStd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTStd.Size = New System.Drawing.Size(66, 21)
        Me.txtTStd.TabIndex = 88
        Me.txtTStd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaOpeningStd
        '
        Me.txtThetaOpeningStd.AcceptsReturn = True
        Me.txtThetaOpeningStd.BackColor = System.Drawing.Color.White
        Me.txtThetaOpeningStd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaOpeningStd.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaOpeningStd.ForeColor = System.Drawing.Color.Black
        Me.txtThetaOpeningStd.Location = New System.Drawing.Point(492, 452)
        Me.txtThetaOpeningStd.MaxLength = 0
        Me.txtThetaOpeningStd.Name = "txtThetaOpeningStd"
        Me.txtThetaOpeningStd.ReadOnly = True
        Me.txtThetaOpeningStd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaOpeningStd.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaOpeningStd.TabIndex = 87
        Me.txtThetaOpeningStd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(418, 476)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 14)
        Me.Label9.TabIndex = 82
        Me.Label9.Text = "Thickness"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(388, 454)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 14)
        Me.Label8.TabIndex = 81
        Me.Label8.Text = "Opening Angle"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(416, 432)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 14)
        Me.Label7.TabIndex = 80
        Me.Label7.Text = "Free Height"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPOrient
        '
        Me.txtPOrient.AcceptsReturn = True
        Me.txtPOrient.BackColor = System.Drawing.Color.White
        Me.txtPOrient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPOrient.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPOrient.ForeColor = System.Drawing.Color.Black
        Me.txtPOrient.Location = New System.Drawing.Point(104, 430)
        Me.txtPOrient.MaxLength = 0
        Me.txtPOrient.Name = "txtPOrient"
        Me.txtPOrient.ReadOnly = True
        Me.txtPOrient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPOrient.Size = New System.Drawing.Size(80, 21)
        Me.txtPOrient.TabIndex = 69
        Me.txtPOrient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDControl
        '
        Me.txtDControl.AcceptsReturn = True
        Me.txtDControl.BackColor = System.Drawing.Color.White
        Me.txtDControl.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDControl.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDControl.ForeColor = System.Drawing.Color.Black
        Me.txtDControl.Location = New System.Drawing.Point(104, 452)
        Me.txtDControl.MaxLength = 0
        Me.txtDControl.Name = "txtDControl"
        Me.txtDControl.ReadOnly = True
        Me.txtDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDControl.Size = New System.Drawing.Size(80, 21)
        Me.txtDControl.TabIndex = 68
        Me.txtDControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 456)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 14)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Control  Dia"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 432)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 14)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "Press. Orient "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCrossSecNo
        '
        Me.txtCrossSecNo.AcceptsReturn = True
        Me.txtCrossSecNo.BackColor = System.Drawing.Color.White
        Me.txtCrossSecNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo.ForeColor = System.Drawing.Color.Black
        Me.txtCrossSecNo.Location = New System.Drawing.Point(104, 397)
        Me.txtCrossSecNo.MaxLength = 0
        Me.txtCrossSecNo.Name = "txtCrossSecNo"
        Me.txtCrossSecNo.ReadOnly = True
        Me.txtCrossSecNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCrossSecNo.Size = New System.Drawing.Size(80, 21)
        Me.txtCrossSecNo.TabIndex = 53
        Me.txtCrossSecNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(121, 382)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Number"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 400)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Cross Sec."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStandard
        '
        Me.lblStandard.BackColor = System.Drawing.Color.White
        Me.lblStandard.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStandard.Location = New System.Drawing.Point(346, 18)
        Me.lblStandard.Name = "lblStandard"
        Me.lblStandard.Size = New System.Drawing.Size(230, 15)
        Me.lblStandard.TabIndex = 2
        Me.lblStandard.Text = "Standard"
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
        Me.picSeal.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picSeal.Location = New System.Drawing.Point(13, 8)
        Me.picSeal.Name = "picSeal"
        Me.picSeal.Size = New System.Drawing.Size(667, 371)
        Me.picSeal.TabIndex = 0
        Me.picSeal.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmdDXF)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.lblErrMsg)
        Me.Panel1.Controls.Add(Me.lblAdj)
        Me.Panel1.Controls.Add(Me.lblStd)
        Me.Panel1.Controls.Add(Me.lblWid)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lblWidStd)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.lblCSealAdjusted)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.updDIndexTArray)
        Me.Panel1.Controls.Add(Me.updDThetaOpening)
        Me.Panel1.Controls.Add(Me.updDHfree)
        Me.Panel1.Controls.Add(Me.txtT)
        Me.Panel1.Controls.Add(Me.txtThetaOpening)
        Me.Panel1.Controls.Add(Me.txtHfree)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtHFreeStd)
        Me.Panel1.Controls.Add(Me.txtTStd)
        Me.Panel1.Controls.Add(Me.txtThetaOpeningStd)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtPOrient)
        Me.Panel1.Controls.Add(Me.txtDControl)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblStandard)
        Me.Panel1.Controls.Add(Me.lblFreeHeight)
        Me.Panel1.Controls.Add(Me.picSeal)
        Me.Panel1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(4, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(698, 598)
        Me.Panel1.TabIndex = 1
        '
        'cmdDXF
        '
        Me.cmdDXF.BackColor = System.Drawing.Color.Silver
        Me.cmdDXF.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDXF.Image = CType(resources.GetObject("cmdDXF.Image"), System.Drawing.Image)
        Me.cmdDXF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDXF.Location = New System.Drawing.Point(13, 555)
        Me.cmdDXF.Name = "cmdDXF"
        Me.cmdDXF.Size = New System.Drawing.Size(139, 28)
        Me.cmdDXF.TabIndex = 114
        Me.cmdDXF.Text = "    Export To DXF"
        Me.cmdDXF.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(635, 430)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(63, 17)
        Me.Label11.TabIndex = 113
        Me.Label11.Text = "(UnPlated)"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblErrMsg
        '
        Me.lblErrMsg.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.lblErrMsg.Location = New System.Drawing.Point(416, 507)
        Me.lblErrMsg.Name = "lblErrMsg"
        Me.lblErrMsg.Size = New System.Drawing.Size(256, 30)
        Me.lblErrMsg.TabIndex = 112
        '
        'lblAdj
        '
        Me.lblAdj.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblAdj.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdj.Location = New System.Drawing.Point(580, 412)
        Me.lblAdj.Name = "lblAdj"
        Me.lblAdj.Size = New System.Drawing.Size(60, 13)
        Me.lblAdj.TabIndex = 111
        Me.lblAdj.Text = "Adjusted"
        Me.lblAdj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblStd
        '
        Me.lblStd.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblStd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStd.Location = New System.Drawing.Point(495, 412)
        Me.lblStd.Name = "lblStd"
        Me.lblStd.Size = New System.Drawing.Size(60, 13)
        Me.lblStd.TabIndex = 110
        Me.lblStd.Text = "Standard"
        Me.lblStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWid
        '
        Me.lblWid.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblWid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWid.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblWid.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWid.Location = New System.Drawing.Point(313, 452)
        Me.lblWid.Name = "lblWid"
        Me.lblWid.Size = New System.Drawing.Size(60, 20)
        Me.lblWid.TabIndex = 109
        Me.lblWid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(210, 454)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(95, 14)
        Me.Label10.TabIndex = 108
        Me.Label10.Text = "Adjusted"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblWidStd
        '
        Me.lblWidStd.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblWidStd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWidStd.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblWidStd.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidStd.Location = New System.Drawing.Point(313, 430)
        Me.lblWidStd.Name = "lblWidStd"
        Me.lblWidStd.Size = New System.Drawing.Size(60, 20)
        Me.lblWidStd.TabIndex = 107
        Me.lblWidStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(210, 432)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 14)
        Me.Label5.TabIndex = 106
        Me.Label5.Text = "Wid : Standard"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtT
        '
        Me.txtT.AcceptsReturn = True
        Me.txtT.BackColor = System.Drawing.SystemColors.Window
        Me.txtT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtT.Location = New System.Drawing.Point(587, 474)
        Me.txtT.MaxLength = 0
        Me.txtT.Name = "txtT"
        Me.txtT.Size = New System.Drawing.Size(48, 21)
        Me.txtT.TabIndex = 105
        Me.txtT.Text = "0.00"
        Me.txtT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCreateWordDocument})
        Me.mnuOptions.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.Size = New System.Drawing.Size(62, 20)
        Me.mnuOptions.Text = "Options"
        '
        'mnuCreateWordDocument
        '
        Me.mnuCreateWordDocument.Image = CType(resources.GetObject("mnuCreateWordDocument.Image"), System.Drawing.Image)
        Me.mnuCreateWordDocument.Name = "mnuCreateWordDocument"
        Me.mnuCreateWordDocument.Size = New System.Drawing.Size(203, 22)
        Me.mnuCreateWordDocument.Text = "Create Word Document"
        '
        'IPE_frmAdjGeomCSeal
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(706, 627)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "IPE_frmAdjGeomCSeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Adjust Geometry - CSeal"
        CType(Me.updDIndexTArray, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updDThetaOpening, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updDHfree, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
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

        '....Loading Image
        'LoadImageLogo(imgLogo)

    End Sub


#Region "FORM EVENT ROUTINES:"

    Private Sub frmAdjustGeometryCSeal_Load(ByVal sender As System.Object, _
                                            ByVal e As System.EventArgs) _
                                            Handles MyBase.Load
        '=====================================================================
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
        'psngMargin = gIPE_Unit.EngLToUserL(mcUniformMargin) '....In user unit (in or mm)

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


        '....Display data & graphics on the form.
        '........Local seal object "mCSeal" (which is recently initialized above) is used.
        DisplayData()

        '....Display graphics on the picture box.
        DoGraphics()



        'MsgBox("Hfree = " & mCSeal.Hfree)
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '============================
        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            txtHfree.Enabled = False
            updDHfree.Enabled = False

            txtThetaOpening.Enabled = False
            updDThetaOpening.Enabled = False

            txtT.Enabled = False
            updDIndexTArray.Enabled = False
        Else
            txtHfree.Enabled = True
            updDHfree.Enabled = True

            txtThetaOpening.Enabled = True
            updDThetaOpening.Enabled = True

            txtT.Enabled = True
            updDIndexTArray.Enabled = True
        End If

    End Sub


    Private Sub InitializeLocalObject()
        '==============================
        '....From gSeal ===> mCSeal. 
        '........Now onwards, mCSeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal in the 
        '........"SaveData" routine which is called when the form is exited and another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local Seal Object.
        mCSeal = New IPE_clsCSeal("C-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        mCSeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal).Clone

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
        pIncrement_Calc_UserL = gIPE_Unit.L_ConToUser(pIncrement_Calc)


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
        pDHfreeAdjMax_UserL = gIPE_Unit.L_ConToUser(mCSeal.DHfreeAdjMax)

        i = 0       '....Initialize
        Do While (i * pIncrement_UpDown < pDHfreeAdjMax_UserL)
            i = i + 1
        Loop

        pMax_UpDown = i * pIncrement_UpDown


        '........Minimum Value:
        '
        Dim pDHfreeAdjMin_UserL As Single
        pDHfreeAdjMin_UserL = gIPE_Unit.L_ConToUser(mCSeal.DHfreeAdjMin)

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
            pDHfree_UserL = gIPE_Unit.L_ConToUser(mCSeal.DHfree)
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

        '....For 'T', the index of the "ArrayTStd", exposed by the IPE_clsCSeal object is 
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

        '....Use the local seal object.
        With mCSeal

            txtCrossSecNo.Text = .MCrossSecNo
            txtPOrient.Text = .POrient
            txtDControl.Text = gIPE_Unit.WriteInUserL(.DControl)

            lblWidStd.Text = gIPE_Unit.WriteInUserL(.WidStd)
            lblWid.Text = gIPE_Unit.WriteInUserL(.Wid)

            '....Standard parameter values:
            txtHFreeStd.Text = gIPE_Unit.WriteInUserL(.HfreeStd)
            txtThetaOpeningStd.Text = Format(.ThetaOpeningStd, "##0.0")
            txtTStd.Text = gIPE_Unit.WriteInUserL(.TStd, "LFormat")

            '....Adjusted parameter values:
            txtHfree.Text = gIPE_Unit.WriteInUserL(.Hfree)
            txtThetaOpening.Text = Format(.ThetaOpening, "##0.0")
            txtT.Text = gIPE_Unit.WriteInUserL(.T, "LFormat")

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

    End Sub

#End Region

#End Region


#Region " CONTROL EVENT ROUTINES:"

#Region "NUMERIN UP-DOWN CONTROL RELATED ROUTINES:"


    Private Sub updDHfree_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
                                Handles updDHfree.Click
        '=============================================================================
        '....Diagonostic statement.
        'MsgBox("updDHfree_Click, Value = " & updDHfree.Value)

        'If mblnUpdDHfree_Clicked = False Then mblnUpdDHfree_Clicked = True

        'Dim pDHfree As Single
        'pDHfree = gIPE_Unit.L_UserToCon(CType(updDHfree.Value, Single))

        'Dim pHfree As Single
        'pHfree = mCSeal.HfreeStd + pDHfree

        'txtHfree.Text = gIPE_Unit.WriteInUserL(pHfree)

    End Sub


    Private Sub updDHfree_Enter(ByVal sender As Object, ByVal e As System.EventArgs) _
                                Handles updDHfree.Enter
        '=================================================================================
        'MsgBox("updDHfree_Enter")
        mblnUpdDHfree_Entered = True

    End Sub


    Private Sub updDHfree_ValueChanged(ByVal sender As System.Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles updDHfree.ValueChanged
        '==============================================================

        If mblnUpdDHfree_Entered = True Then

            Dim pDHfree As Single
            pDHfree = gIPE_Unit.L_UserToCon(CType(updDHfree.Value, Single))

            Dim pHfree As Single
            pHfree = mCSeal.HfreeStd + pDHfree

            txtHfree.Text = gIPE_Unit.WriteInUserL(pHfree)
        End If

    End Sub


    Private Sub updDThetaOpening_Click(ByVal sender As System.Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles updDThetaOpening.Click
        '===========================================================================
        'Dim pThetaOpening As Single
        'pThetaOpening = mCSeal.ThetaOpeningStd + updDThetaOpening.Value
        'txtThetaOpening.Text = Format(pThetaOpening, "##0.0")

    End Sub


    Private Sub updDThetaOpening_Enter(ByVal sender As Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles updDThetaOpening.Enter
        '=============================================================
        mblnUpdDThetaOpening_Entered = True

    End Sub


    Private Sub updDThetaOpening_ValueChanged(ByVal sender As System.Object, _
                                              ByVal e As System.EventArgs) _
                                              Handles updDThetaOpening.ValueChanged
        '===========================================================================

        If mblnUpdDThetaOpening_Entered = True Then
            Dim pThetaOpening As Single
            pThetaOpening = mCSeal.ThetaOpeningStd + updDThetaOpening.Value
            txtThetaOpening.Text = ConvertToStr(pThetaOpening, "##0.0")
        End If

    End Sub


    Private Sub updDIndexTArray_Click(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles updDIndexTArray.Click
        '===============================================================
        'Dim pIndexT As Int16

        'With mCSeal

        '    pIndexT = .IndexTStd + CInt(updDIndexTArray.Value)

        '    '....Check if the inner radius is calculated to be a positive value. 
        '    '........14NOV06. Resolves DR V4.1, Error 03.
        '    Dim pRin As Single
        '    pRin = (0.5 * .Hfree - .ArrayTStd(pIndexT))

        '    If pRin > gcEPS Then
        '        txtT.Text = gIPE_Unit.WriteInUserL(.ArrayTStd(pIndexT), "LFormat")

        '    Else
        '        Dim pstrMsg As String
        '        pstrMsg = "T can't be further incremented as inner radius " & _
        '                  "tends to become -ive."
        '        MessageBox.Show(pstrMsg, "Thickness Error", MessageBoxButtons.OK, _
        '                                                    MessageBoxIcon.Error)

        '        '....Reset the updown button to the previous value.
        '        updDIndexTArray.Value = updDIndexTArray.Value - 1
        '    End If

        'End With

    End Sub


    Private Sub updDIndexTArray_Enter(ByVal sender As Object, ByVal e As System.EventArgs) _
                                      Handles updDIndexTArray.Enter
        '====================================================================================
        mblnUpd_DIndexTArray_Entered = True
    End Sub


    Private Sub updDIndexTArray_ValueChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles updDIndexTArray.ValueChanged
        '=========================================================================

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
                        txtT.Text = gIPE_Unit.WriteInUserL(.ArrayTStd(pIndexT), "LFormat")

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

    Private Sub txtHfree_TextChanged(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) _
                                     Handles txtHfree.TextChanged
        '==============================================================

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
        pDHfree = gIPE_Unit.L_UserToCon(txtHfree.Text) - mCSeal.HfreeStd

        Dim pDHfree_UserL As Single
        pDHfree_UserL = gIPE_Unit.L_ConToUser(pDHfree)

        Dim pERROR_ROUND_OFF_UserL As Single
        If gIPE_Unit.System = "English" Then
            pERROR_ROUND_OFF_UserL = 0.0005

        ElseIf gIPE_Unit.System = "Metric" Then
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
                pDHfree = gIPE_Unit.L_UserToCon(pDHfree_UserL)
                pblnDHfree_Modified = True

                '....Resolves DR - V41: error 2. 13NOV06.
            ElseIf pDHfree_UserL > (updDHfree.Maximum + pERROR_ROUND_OFF_UserL) Then
                '-------------------------------------------------------------------      
                pstrMsg = "Not acceptable. Hfree is higher than the maximum value." & _
                                vbCrLf & "Hence, it is set to the maximum value."
                MessageBox.Show(pstrMsg, "DHfree Maximum Value Error", MessageBoxButtons.OK, _
                                                                    MessageBoxIcon.Error)
                pDHfree_UserL = CType(updDHfree.Maximum, Single)
                pDHfree = gIPE_Unit.L_UserToCon(pDHfree_UserL)
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
                txtHfree.Text = gIPE_Unit.WriteInUserL(.Hfree)

            txtDControl.Text = gIPE_Unit.WriteInUserL(.DControl)    '....Update display.
        End With


        '....Check the "T" value if it is within the allowable max. & min. manufacturing limits, 
        '........which are dependent on Hfree.  
        Dim pT As Single = gIPE_Unit.L_UserToCon(txtT.Text)
        CheckTValue(pT)

        '....Redraw Seal Geometries.
        DoGraphics()

    End Sub


    Private Sub txtThetaOpening_TextChanged(ByVal sender As System.Object, _
                                            ByVal e As System.EventArgs) _
                                            Handles txtThetaOpening.TextChanged
        '=======================================================================

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

        '....Re-draw Seal Geometries
        DoGraphics()

    End Sub


    Private Sub txtT_TextChanged(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles txtT.TextChanged
        '=================================================================

        If mCSeal Is Nothing = True Then Exit Sub

        '-------------------------------------------------------------------

        'Check if the entered value of the "Thickness" exceeds the preset hard limits.
        '-------------------------------------------------------------------------------

        '....Preset hard limits on Thickness: Minimum = 0.002 in & Maximum = 0.080 in.
        '
        '........In user unit: (in or mm)
        Dim pcThickMin_HardLim As Single = gIPE_Unit.L_ConToUser(0.002 * gIPE_Unit.CFacConL)
        Dim pcThickMax_HardLim As Single = gIPE_Unit.L_ConToUser(0.08 * gIPE_Unit.CFacConL)


        Dim pThick_Entered As Single = ConvertToSng(txtT.Text)
        Dim pstrMsg As String

        If pThick_Entered > pcThickMax_HardLim Then
            pstrMsg = "Thickness can't be higher than " & pcThickMax_HardLim
            MessageBox.Show(pstrMsg, "Thickness Error", MessageBoxButtons.OK, _
                                                        MessageBoxIcon.Error)
            '....Reset the text box display.
            txtT.Text = Format(pcThickMax_HardLim, gIPE_Unit.LFormat)


        ElseIf pThick_Entered < pcThickMin_HardLim Then
            pstrMsg = "Thickness can't be lower than " & pcThickMin_HardLim
            MessageBox.Show(pstrMsg, "Thick Error", MessageBoxButtons.OK, _
                                                    MessageBoxIcon.Error)
            '....Reset the text box display.
            txtT.Text = Format(pcThickMin_HardLim, gIPE_Unit.LFormat)

        End If


        Dim pT As Single = gIPE_Unit.L_UserToCon(txtT.Text)
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

            txtT.Text = gIPE_Unit.WriteInUserL(mCSeal.T, "LFormat")

        End If

    End Sub


    Private Sub TxtBox_KeyPress(ByVal sender As System.Object, _
                                     ByVal e As KeyPressEventArgs) _
                                     Handles txtHfree.KeyPress, txtThetaOpening.KeyPress, txtT.KeyPress
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

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdDXF_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) _
                                Handles cmdDXF.Click
        '========================================================

        SaveData()
        Me.Cursor = Cursors.WaitCursor

        Dim pFileName As String = ""
        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal)

            SaveFileDialog1.FilterIndex = 1
            SaveFileDialog1.Filter = "Configuration files (*.DXF)|*.DXF"
            SaveFileDialog1.Title = "Save"
            SaveFileDialog1.FileName = ExtractPreData(gIPE_File.In_Title, ".") & "_" & _
                                       .MCrossSecNo & ".DXF"

            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                pFileName = SaveFileDialog1.FileName
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


    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()
        Me.Close()

    End Sub


    Private Sub SaveData()
        '=================    
        '....Local form data ===> Global Seal Object Data.

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal)
            .DHfree = gIPE_Unit.L_UserToCon(txtHfree.Text) - mCSeal.HfreeStd
            .DThetaOpening = Val(txtThetaOpening.Text) - mCSeal.ThetaOpeningStd
            .T = gIPE_Unit.L_UserToCon(txtT.Text)
        End With

    End Sub

#End Region


#Region "MENU EVENT ROUTINE:"

    Private Sub mnuCreateWordDocument_Click(ByVal sender As System.Object, _
                                            ByVal e As System.EventArgs) _
                                            Handles mnuCreateWordDocument.Click
        '=========================================================================
        SaveData()
        gIPE_Report = New IPE_clsReport()
        gIPE_Report.CreateAdjGeomDoc(picSeal, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, gIPE_Unit, gIPE_User, gIPE_Project)
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


        '....Draw both "Standard" & "Adjusted" Seal Geometry.
        mCSeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                    "BOTH", "SCALE_BY_STD", 2.5, _
                                    xEnvpTopL, yEnvpTopL, xEnvpBotR, yEnvpBotR)


        'Caption Labels:       
        '---------------
        If mCSeal Is Nothing = False Then

            lblStandard.Text = "Standard  =  " & gIPE_Unit.WriteInUserL((mCSeal.HfreeStd))

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
            If gIPE_Unit.System = "English" Then
                pERROR_ROUNDING_Max = pERROR_ROUNDING_Max_Eng

            ElseIf gIPE_Unit.System = "Metric" Then
                pERROR_ROUNDING_Max = pERROR_ROUNDING_Max_Met
            End If

            Dim pERROR_DelHfreePCent_Max As Single
            Dim pHfreeStd_UserL As Single = gIPE_Unit.L_ConToUser(mCSeal.HfreeStd)
            pERROR_DelHfreePCent_Max = pERROR_ROUNDING_Max * 100 / pHfreeStd_UserL


            If Abs(psngDelHfreePCent) <= pERROR_DelHfreePCent_Max Then   '0.1# Then
                lblCSealAdjusted.Visible = False

            ElseIf Abs(psngDelHfreePCent) > pERROR_DelHfreePCent_Max Then  ' 0.1# Then
                lblCSealAdjusted.Visible = True
                lblCSealAdjusted.Text = "Adjusted   =  " & _
                                    gIPE_Unit.WriteInUserL(mCSeal.Hfree) & _
                                    "  ( " & Format(psngDelHfreePCent, "##0.0") & " %)"
            End If

            lblWid.Text = gIPE_Unit.WriteInUserL(mCSeal.Wid)

        End If

    End Sub

#End Region

End Class
