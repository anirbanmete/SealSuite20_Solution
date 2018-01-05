
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmDesignCenterESeal                   '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  11MAR16                                '
'                                                                              '
'===============================================================================

Imports System.Windows.Forms
Imports System.Math
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports System.Drawing.Printing
Imports System.IO
Imports System.Xml.Serialization
Imports System.Globalization.CultureInfo
Imports System.Threading
Imports System.Globalization
Imports SealIPELib = SealIPELib101
Imports clsLibrary11


Public Class IPE_frmDesignCenterESeal
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mESeal As IPE_clsESeal      '....Local Seal object.
    'Private mESealTemp As IPE_clsESeal  '....Temporary ESeal Object to save last proper ESeal parameters 
    '....before distortion.      

    Private Const mcThetaMax As Single = 120
    Private Const mcThetaMin As Single = 0

    Private Const mcRadMax As Single = 5.0
    Private Const mcRadMin As Single = 0.01

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single

    '   Boolean Variables to indicate Control Event Type:   
    '   -------------------------------------------------
    '
    '   ....UpDown Buttons:
    '   ........If variable TRUE  ==> Clicked by the user. 
    '   ........            FALSE ==> Value set programmatically.  
    '
    Private mblnUpdHConv_Entered As Boolean = False
    Private mblnUpdRadE1_Entered As Boolean = False
    Private mblnUpdRadE2_Entered As Boolean = False
    Private mblnUpdRadE3_Entered As Boolean = False

    Private mblnUpdThetaE1_Entered As Boolean = False
    Private mblnUpdThetaE2_Entered As Boolean = False
    Private mblnUpdThetaE3_Entered As Boolean = False

    Private mblnUpdRadM2_Entered As Boolean = False
    Private mblnUpdRadM3_Entered As Boolean = False
    Private mblnUpdRadM4_Entered As Boolean = False
    Private mblnUpdRadM5_Entered As Boolean = False

    Private mblnUpdThetaM1_Entered As Boolean = False
    Private mblnUpdThetaM2_Entered As Boolean = False
    Private mblnUpdThetaM4_Entered As Boolean = False

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
    Friend WithEvents lblHFreeOriginal As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents txtTemplateNo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblHConv As System.Windows.Forms.Label
    Public WithEvents txtNConv As System.Windows.Forms.TextBox
    Public WithEvents txtHConv As System.Windows.Forms.TextBox
    Public WithEvents txtRadE1 As System.Windows.Forms.TextBox
    Public WithEvents txtRadE2 As System.Windows.Forms.TextBox
    Public WithEvents txtRadE3 As System.Windows.Forms.TextBox
    Public WithEvents txtRadM3 As System.Windows.Forms.TextBox
    Public WithEvents txtThetaE3 As System.Windows.Forms.TextBox
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblThetaM1 As System.Windows.Forms.Label
    Friend WithEvents lblThetaE3 As System.Windows.Forms.Label
    Friend WithEvents lblThetaE2 As System.Windows.Forms.Label
    Friend WithEvents lblThetaE1 As System.Windows.Forms.Label
    Friend WithEvents lblRadM3 As System.Windows.Forms.Label
    Friend WithEvents lblRadE3 As System.Windows.Forms.Label
    Friend WithEvents lblRadE2 As System.Windows.Forms.Label
    Friend WithEvents lblRadE1 As System.Windows.Forms.Label
    Friend WithEvents lblHFreeNew As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo As System.Windows.Forms.TextBox
    Public WithEvents txtPOrient As System.Windows.Forms.TextBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Public WithEvents txtT As System.Windows.Forms.TextBox
    Public WithEvents txtThetaE2 As System.Windows.Forms.TextBox
    Public WithEvents txtThetaE1_New As System.Windows.Forms.TextBox
    Public WithEvents txtThetaM1_New As System.Windows.Forms.TextBox
    Public WithEvents txtThetaE1 As System.Windows.Forms.TextBox
    Public WithEvents txtThetaM1 As System.Windows.Forms.TextBox
    Friend WithEvents updThetaM1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents updThetaE1 As System.Windows.Forms.NumericUpDown
    Public WithEvents picSeal As System.Windows.Forms.PictureBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents updRadM2 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadM2_New As System.Windows.Forms.TextBox
    Friend WithEvents updRadE3 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadE3_New As System.Windows.Forms.TextBox
    Friend WithEvents updRadE2 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadE2_New As System.Windows.Forms.TextBox
    Friend WithEvents updRadE1 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadE1_New As System.Windows.Forms.TextBox
    Public WithEvents txtRadM2 As System.Windows.Forms.TextBox
    Friend WithEvents lblRadM2 As System.Windows.Forms.Label
    Friend WithEvents updRadM5 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadM5_New As System.Windows.Forms.TextBox
    Public WithEvents txtRadM5 As System.Windows.Forms.TextBox
    Friend WithEvents lblRadM5 As System.Windows.Forms.Label
    Friend WithEvents updRadM3 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadM3_New As System.Windows.Forms.TextBox
    Public WithEvents txtRadM4 As System.Windows.Forms.TextBox
    Friend WithEvents lblRadM4 As System.Windows.Forms.Label
    Friend WithEvents updThetaM2 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtThetaM2_New As System.Windows.Forms.TextBox
    Public WithEvents txtThetaM2 As System.Windows.Forms.TextBox
    Friend WithEvents lblThetaM2 As System.Windows.Forms.Label
    Friend WithEvents updThetaM4 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtThetaM4_New As System.Windows.Forms.TextBox
    Public WithEvents txtThetaM4 As System.Windows.Forms.TextBox
    Friend WithEvents lblThetaM4 As System.Windows.Forms.Label
    Friend WithEvents updThetaE3 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtThetaE3_New As System.Windows.Forms.TextBox
    Friend WithEvents updThetaE2 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtThetaE2_New As System.Windows.Forms.TextBox
    Friend WithEvents updRadM4 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRadM4_New As System.Windows.Forms.TextBox
    Friend WithEvents grpConv As System.Windows.Forms.GroupBox
    Friend WithEvents updIndexTArray As System.Windows.Forms.NumericUpDown
    Public WithEvents txtT_New As System.Windows.Forms.TextBox
    Friend WithEvents lblAdj As System.Windows.Forms.Label
    Friend WithEvents lblStd As System.Windows.Forms.Label
    Friend WithEvents lblCrossSecNew As System.Windows.Forms.Label
    Friend WithEvents lblCrossSecOrg As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo_New As System.Windows.Forms.TextBox
    Friend WithEvents cmdDXF As System.Windows.Forms.Button
    Friend WithEvents txtNConv_New As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblWidOriginal As System.Windows.Forms.Label
    Friend WithEvents lblWidNew As System.Windows.Forms.Label
    Friend WithEvents lblWidth As System.Windows.Forms.Label
    '....Local Seal object. 
    Friend WithEvents updHConv As System.Windows.Forms.NumericUpDown
    Public WithEvents txtHConv_New As System.Windows.Forms.TextBox
    Friend WithEvents updNConv As System.Windows.Forms.NumericUpDown
    Friend WithEvents PrintDocu As System.Drawing.Printing.PrintDocument
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuPrintForm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblErrT As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents saveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cmdViewNomenclature As System.Windows.Forms.Button
    Friend WithEvents grpGenParam As System.Windows.Forms.GroupBox
    Friend WithEvents grpLeg As System.Windows.Forms.GroupBox


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmDesignCenterESeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpGenParam = New System.Windows.Forms.GroupBox()
        Me.updNConv = New System.Windows.Forms.NumericUpDown()
        Me.txtNConv_New = New System.Windows.Forms.TextBox()
        Me.lblAdj = New System.Windows.Forms.Label()
        Me.lblStd = New System.Windows.Forms.Label()
        Me.updIndexTArray = New System.Windows.Forms.NumericUpDown()
        Me.txtT_New = New System.Windows.Forms.TextBox()
        Me.txtNConv = New System.Windows.Forms.TextBox()
        Me.txtT = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmdViewNomenclature = New System.Windows.Forms.Button()
        Me.lblErrT = New System.Windows.Forms.Label()
        Me.lblWidOriginal = New System.Windows.Forms.Label()
        Me.lblWidNew = New System.Windows.Forms.Label()
        Me.lblWidth = New System.Windows.Forms.Label()
        Me.cmdDXF = New System.Windows.Forms.Button()
        Me.txtCrossSecNo_New = New System.Windows.Forms.TextBox()
        Me.lblCrossSecNew = New System.Windows.Forms.Label()
        Me.lblCrossSecOrg = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.lblHFreeOriginal = New System.Windows.Forms.Label()
        Me.lblHFreeNew = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtPOrient = New System.Windows.Forms.TextBox()
        Me.txtDControl = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTemplateNo = New System.Windows.Forms.TextBox()
        Me.txtCrossSecNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblFreeHeight = New System.Windows.Forms.Label()
        Me.grpConv = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtRadM3 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.updRadM4 = New System.Windows.Forms.NumericUpDown()
        Me.updRadM2 = New System.Windows.Forms.NumericUpDown()
        Me.updHConv = New System.Windows.Forms.NumericUpDown()
        Me.updThetaM2 = New System.Windows.Forms.NumericUpDown()
        Me.txtHConv_New = New System.Windows.Forms.TextBox()
        Me.txtRadM5_New = New System.Windows.Forms.TextBox()
        Me.txtThetaM2_New = New System.Windows.Forms.TextBox()
        Me.lblRadM5 = New System.Windows.Forms.Label()
        Me.txtRadM2 = New System.Windows.Forms.TextBox()
        Me.txtRadM5 = New System.Windows.Forms.TextBox()
        Me.lblRadM2 = New System.Windows.Forms.Label()
        Me.lblThetaM2 = New System.Windows.Forms.Label()
        Me.txtThetaM2 = New System.Windows.Forms.TextBox()
        Me.updRadM5 = New System.Windows.Forms.NumericUpDown()
        Me.updRadM3 = New System.Windows.Forms.NumericUpDown()
        Me.txtRadM3_New = New System.Windows.Forms.TextBox()
        Me.updThetaM4 = New System.Windows.Forms.NumericUpDown()
        Me.txtThetaM4_New = New System.Windows.Forms.TextBox()
        Me.txtThetaM4 = New System.Windows.Forms.TextBox()
        Me.lblThetaM4 = New System.Windows.Forms.Label()
        Me.lblRadM4 = New System.Windows.Forms.Label()
        Me.txtRadM4 = New System.Windows.Forms.TextBox()
        Me.lblRadM3 = New System.Windows.Forms.Label()
        Me.updThetaM1 = New System.Windows.Forms.NumericUpDown()
        Me.txtThetaM1 = New System.Windows.Forms.TextBox()
        Me.txtThetaM1_New = New System.Windows.Forms.TextBox()
        Me.lblThetaM1 = New System.Windows.Forms.Label()
        Me.txtHConv = New System.Windows.Forms.TextBox()
        Me.txtRadM2_New = New System.Windows.Forms.TextBox()
        Me.lblHConv = New System.Windows.Forms.Label()
        Me.txtRadM4_New = New System.Windows.Forms.TextBox()
        Me.picSeal = New System.Windows.Forms.PictureBox()
        Me.grpLeg = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtThetaE3 = New System.Windows.Forms.TextBox()
        Me.txtThetaE1 = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtThetaE2 = New System.Windows.Forms.TextBox()
        Me.lblThetaE3 = New System.Windows.Forms.Label()
        Me.txtThetaE1_New = New System.Windows.Forms.TextBox()
        Me.lblThetaE2 = New System.Windows.Forms.Label()
        Me.lblRadE2 = New System.Windows.Forms.Label()
        Me.lblThetaE1 = New System.Windows.Forms.Label()
        Me.updThetaE1 = New System.Windows.Forms.NumericUpDown()
        Me.lblRadE3 = New System.Windows.Forms.Label()
        Me.lblRadE1 = New System.Windows.Forms.Label()
        Me.txtRadE1 = New System.Windows.Forms.TextBox()
        Me.txtRadE2 = New System.Windows.Forms.TextBox()
        Me.updThetaE3 = New System.Windows.Forms.NumericUpDown()
        Me.txtRadE3 = New System.Windows.Forms.TextBox()
        Me.updRadE1 = New System.Windows.Forms.NumericUpDown()
        Me.updRadE3 = New System.Windows.Forms.NumericUpDown()
        Me.txtRadE2_New = New System.Windows.Forms.TextBox()
        Me.txtRadE3_New = New System.Windows.Forms.TextBox()
        Me.txtThetaE3_New = New System.Windows.Forms.TextBox()
        Me.txtThetaE2_New = New System.Windows.Forms.TextBox()
        Me.updThetaE2 = New System.Windows.Forms.NumericUpDown()
        Me.updRadE2 = New System.Windows.Forms.NumericUpDown()
        Me.txtRadE1_New = New System.Windows.Forms.TextBox()
        Me.PrintDocu = New System.Drawing.Printing.PrintDocument()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuPrintForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Panel1.SuspendLayout()
        Me.grpGenParam.SuspendLayout()
        CType(Me.updNConv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updIndexTArray, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpConv.SuspendLayout()
        CType(Me.updRadM4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRadM2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updHConv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updThetaM2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRadM5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRadM3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updThetaM4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updThetaM1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpLeg.SuspendLayout()
        CType(Me.updThetaE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updThetaE3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRadE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRadE3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updThetaE2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRadE2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblBorder.Size = New System.Drawing.Size(887, 723)
        Me.lblBorder.TabIndex = 0
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.grpGenParam)
        Me.Panel1.Controls.Add(Me.cmdViewNomenclature)
        Me.Panel1.Controls.Add(Me.lblErrT)
        Me.Panel1.Controls.Add(Me.lblWidOriginal)
        Me.Panel1.Controls.Add(Me.lblWidNew)
        Me.Panel1.Controls.Add(Me.lblWidth)
        Me.Panel1.Controls.Add(Me.cmdDXF)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo_New)
        Me.Panel1.Controls.Add(Me.lblCrossSecNew)
        Me.Panel1.Controls.Add(Me.lblCrossSecOrg)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.lblHFreeOriginal)
        Me.Panel1.Controls.Add(Me.lblHFreeNew)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.txtPOrient)
        Me.Panel1.Controls.Add(Me.txtDControl)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtTemplateNo)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblFreeHeight)
        Me.Panel1.Controls.Add(Me.grpConv)
        Me.Panel1.Controls.Add(Me.picSeal)
        Me.Panel1.Controls.Add(Me.grpLeg)
        Me.Panel1.Location = New System.Drawing.Point(3, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(883, 719)
        Me.Panel1.TabIndex = 1
        '
        'grpGenParam
        '
        Me.grpGenParam.Controls.Add(Me.updNConv)
        Me.grpGenParam.Controls.Add(Me.txtNConv_New)
        Me.grpGenParam.Controls.Add(Me.lblAdj)
        Me.grpGenParam.Controls.Add(Me.lblStd)
        Me.grpGenParam.Controls.Add(Me.updIndexTArray)
        Me.grpGenParam.Controls.Add(Me.txtT_New)
        Me.grpGenParam.Controls.Add(Me.txtNConv)
        Me.grpGenParam.Controls.Add(Me.txtT)
        Me.grpGenParam.Controls.Add(Me.Label8)
        Me.grpGenParam.Controls.Add(Me.Label7)
        Me.grpGenParam.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGenParam.ForeColor = System.Drawing.Color.Black
        Me.grpGenParam.Location = New System.Drawing.Point(20, 458)
        Me.grpGenParam.Name = "grpGenParam"
        Me.grpGenParam.Size = New System.Drawing.Size(249, 86)
        Me.grpGenParam.TabIndex = 182
        Me.grpGenParam.TabStop = False
        Me.grpGenParam.Text = "General Params:"
        '
        'updNConv
        '
        Me.updNConv.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updNConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updNConv.Location = New System.Drawing.Point(156, 33)
        Me.updNConv.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.updNConv.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.updNConv.Name = "updNConv"
        Me.updNConv.Size = New System.Drawing.Size(20, 21)
        Me.updNConv.TabIndex = 178
        Me.updNConv.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtNConv_New
        '
        Me.txtNConv_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtNConv_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNConv_New.Location = New System.Drawing.Point(178, 33)
        Me.txtNConv_New.Name = "txtNConv_New"
        Me.txtNConv_New.ReadOnly = True
        Me.txtNConv_New.Size = New System.Drawing.Size(60, 21)
        Me.txtNConv_New.TabIndex = 2
        Me.txtNConv_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblAdj
        '
        Me.lblAdj.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblAdj.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdj.Location = New System.Drawing.Point(178, 17)
        Me.lblAdj.Name = "lblAdj"
        Me.lblAdj.Size = New System.Drawing.Size(60, 13)
        Me.lblAdj.TabIndex = 162
        Me.lblAdj.Text = "New"
        Me.lblAdj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblStd
        '
        Me.lblStd.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStd.Location = New System.Drawing.Point(94, 17)
        Me.lblStd.Name = "lblStd"
        Me.lblStd.Size = New System.Drawing.Size(60, 13)
        Me.lblStd.TabIndex = 161
        Me.lblStd.Text = "Original"
        Me.lblStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'updIndexTArray
        '
        Me.updIndexTArray.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updIndexTArray.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updIndexTArray.Location = New System.Drawing.Point(156, 57)
        Me.updIndexTArray.Name = "updIndexTArray"
        Me.updIndexTArray.Size = New System.Drawing.Size(20, 21)
        Me.updIndexTArray.TabIndex = 159
        '
        'txtT_New
        '
        Me.txtT_New.AcceptsReturn = True
        Me.txtT_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtT_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT_New.ForeColor = System.Drawing.Color.Black
        Me.txtT_New.Location = New System.Drawing.Point(178, 57)
        Me.txtT_New.MaxLength = 0
        Me.txtT_New.Name = "txtT_New"
        Me.txtT_New.ReadOnly = True
        Me.txtT_New.Size = New System.Drawing.Size(60, 21)
        Me.txtT_New.TabIndex = 3
        Me.txtT_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtNConv
        '
        Me.txtNConv.AcceptsReturn = True
        Me.txtNConv.BackColor = System.Drawing.SystemColors.Window
        Me.txtNConv.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNConv.ForeColor = System.Drawing.Color.Magenta
        Me.txtNConv.Location = New System.Drawing.Point(94, 33)
        Me.txtNConv.MaxLength = 0
        Me.txtNConv.Name = "txtNConv"
        Me.txtNConv.ReadOnly = True
        Me.txtNConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNConv.Size = New System.Drawing.Size(60, 21)
        Me.txtNConv.TabIndex = 67
        Me.txtNConv.TabStop = False
        Me.txtNConv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtT
        '
        Me.txtT.AcceptsReturn = True
        Me.txtT.BackColor = System.Drawing.SystemColors.Window
        Me.txtT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT.ForeColor = System.Drawing.Color.Magenta
        Me.txtT.Location = New System.Drawing.Point(94, 57)
        Me.txtT.MaxLength = 0
        Me.txtT.Name = "txtT"
        Me.txtT.ReadOnly = True
        Me.txtT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtT.Size = New System.Drawing.Size(60, 21)
        Me.txtT.TabIndex = 66
        Me.txtT.TabStop = False
        Me.txtT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 14)
        Me.Label8.TabIndex = 61
        Me.Label8.Text = "Thickness"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 33)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 14)
        Me.Label7.TabIndex = 60
        Me.Label7.Text = "No.  Of  Conv"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdViewNomenclature
        '
        Me.cmdViewNomenclature.BackColor = System.Drawing.Color.Silver
        Me.cmdViewNomenclature.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewNomenclature.Image = CType(resources.GetObject("cmdViewNomenclature.Image"), System.Drawing.Image)
        Me.cmdViewNomenclature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdViewNomenclature.Location = New System.Drawing.Point(153, 684)
        Me.cmdViewNomenclature.Name = "cmdViewNomenclature"
        Me.cmdViewNomenclature.Size = New System.Drawing.Size(147, 28)
        Me.cmdViewNomenclature.TabIndex = 181
        Me.cmdViewNomenclature.Text = "     View Nomenclature"
        Me.cmdViewNomenclature.UseVisualStyleBackColor = False
        '
        'lblErrT
        '
        Me.lblErrT.AutoSize = True
        Me.lblErrT.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrT.Location = New System.Drawing.Point(8, 530)
        Me.lblErrT.Name = "lblErrT"
        Me.lblErrT.Size = New System.Drawing.Size(0, 14)
        Me.lblErrT.TabIndex = 180
        Me.lblErrT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWidOriginal
        '
        Me.lblWidOriginal.BackColor = System.Drawing.Color.White
        Me.lblWidOriginal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidOriginal.Location = New System.Drawing.Point(612, 14)
        Me.lblWidOriginal.Name = "lblWidOriginal"
        Me.lblWidOriginal.Size = New System.Drawing.Size(170, 14)
        Me.lblWidOriginal.TabIndex = 150
        Me.lblWidOriginal.Text = "Original"
        '
        'lblWidNew
        '
        Me.lblWidNew.BackColor = System.Drawing.Color.White
        Me.lblWidNew.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidNew.ForeColor = System.Drawing.Color.Black
        Me.lblWidNew.Location = New System.Drawing.Point(612, 28)
        Me.lblWidNew.Name = "lblWidNew"
        Me.lblWidNew.Size = New System.Drawing.Size(170, 14)
        Me.lblWidNew.TabIndex = 172
        '
        'lblWidth
        '
        Me.lblWidth.BackColor = System.Drawing.Color.White
        Me.lblWidth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidth.Location = New System.Drawing.Point(506, 13)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(100, 16)
        Me.lblWidth.TabIndex = 170
        Me.lblWidth.Text = "Width  :"
        Me.lblWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdDXF
        '
        Me.cmdDXF.BackColor = System.Drawing.Color.Silver
        Me.cmdDXF.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDXF.Image = CType(resources.GetObject("cmdDXF.Image"), System.Drawing.Image)
        Me.cmdDXF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDXF.Location = New System.Drawing.Point(12, 684)
        Me.cmdDXF.Name = "cmdDXF"
        Me.cmdDXF.Size = New System.Drawing.Size(123, 28)
        Me.cmdDXF.TabIndex = 18
        Me.cmdDXF.Text = "   Export To DXF"
        Me.cmdDXF.UseVisualStyleBackColor = False
        '
        'txtCrossSecNo_New
        '
        Me.txtCrossSecNo_New.AcceptsReturn = True
        Me.txtCrossSecNo_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtCrossSecNo_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo_New.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCrossSecNo_New.Location = New System.Drawing.Point(112, 421)
        Me.txtCrossSecNo_New.MaxLength = 0
        Me.txtCrossSecNo_New.Name = "txtCrossSecNo_New"
        Me.txtCrossSecNo_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCrossSecNo_New.Size = New System.Drawing.Size(80, 21)
        Me.txtCrossSecNo_New.TabIndex = 1
        Me.txtCrossSecNo_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblCrossSecNew
        '
        Me.lblCrossSecNew.AutoSize = True
        Me.lblCrossSecNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSecNew.Location = New System.Drawing.Point(73, 421)
        Me.lblCrossSecNew.Name = "lblCrossSecNew"
        Me.lblCrossSecNew.Size = New System.Drawing.Size(31, 13)
        Me.lblCrossSecNew.TabIndex = 164
        Me.lblCrossSecNew.Text = "New"
        Me.lblCrossSecNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCrossSecOrg
        '
        Me.lblCrossSecOrg.AutoSize = True
        Me.lblCrossSecOrg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSecOrg.Location = New System.Drawing.Point(55, 399)
        Me.lblCrossSecOrg.Name = "lblCrossSecOrg"
        Me.lblCrossSecOrg.Size = New System.Drawing.Size(51, 13)
        Me.lblCrossSecOrg.TabIndex = 163
        Me.lblCrossSecOrg.Text = "Original"
        Me.lblCrossSecOrg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(687, 685)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 20
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblHFreeOriginal
        '
        Me.lblHFreeOriginal.BackColor = System.Drawing.Color.White
        Me.lblHFreeOriginal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHFreeOriginal.Location = New System.Drawing.Point(153, 14)
        Me.lblHFreeOriginal.Name = "lblHFreeOriginal"
        Me.lblHFreeOriginal.Size = New System.Drawing.Size(170, 14)
        Me.lblHFreeOriginal.TabIndex = 2
        Me.lblHFreeOriginal.Text = "Original"
        '
        'lblHFreeNew
        '
        Me.lblHFreeNew.BackColor = System.Drawing.Color.White
        Me.lblHFreeNew.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHFreeNew.ForeColor = System.Drawing.Color.Black
        Me.lblHFreeNew.Location = New System.Drawing.Point(153, 28)
        Me.lblHFreeNew.Name = "lblHFreeNew"
        Me.lblHFreeNew.Size = New System.Drawing.Size(287, 14)
        Me.lblHFreeNew.TabIndex = 104
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(777, 685)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(80, 28)
        Me.cmdCancel.TabIndex = 21
        Me.cmdCancel.Text = "   &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtPOrient
        '
        Me.txtPOrient.AcceptsReturn = True
        Me.txtPOrient.BackColor = System.Drawing.SystemColors.Window
        Me.txtPOrient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPOrient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPOrient.Location = New System.Drawing.Point(430, 390)
        Me.txtPOrient.MaxLength = 0
        Me.txtPOrient.Name = "txtPOrient"
        Me.txtPOrient.ReadOnly = True
        Me.txtPOrient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPOrient.Size = New System.Drawing.Size(90, 21)
        Me.txtPOrient.TabIndex = 69
        Me.txtPOrient.TabStop = False
        Me.txtPOrient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDControl
        '
        Me.txtDControl.AcceptsReturn = True
        Me.txtDControl.BackColor = System.Drawing.SystemColors.Window
        Me.txtDControl.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDControl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDControl.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDControl.Location = New System.Drawing.Point(697, 393)
        Me.txtDControl.MaxLength = 0
        Me.txtDControl.Name = "txtDControl"
        Me.txtDControl.ReadOnly = True
        Me.txtDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDControl.Size = New System.Drawing.Size(90, 21)
        Me.txtDControl.TabIndex = 68
        Me.txtDControl.TabStop = False
        Me.txtDControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(596, 394)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(90, 14)
        Me.Label6.TabIndex = 59
        Me.Label6.Text = "Control  Dia"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(334, 391)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 14)
        Me.Label5.TabIndex = 58
        Me.Label5.Text = "Press. Orient "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(195, 380)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 56
        Me.Label4.Text = "Template"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTemplateNo
        '
        Me.txtTemplateNo.AcceptsReturn = True
        Me.txtTemplateNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtTemplateNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTemplateNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTemplateNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTemplateNo.Location = New System.Drawing.Point(198, 397)
        Me.txtTemplateNo.MaxLength = 0
        Me.txtTemplateNo.Name = "txtTemplateNo"
        Me.txtTemplateNo.ReadOnly = True
        Me.txtTemplateNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTemplateNo.Size = New System.Drawing.Size(53, 21)
        Me.txtTemplateNo.TabIndex = 55
        Me.txtTemplateNo.TabStop = False
        Me.txtTemplateNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtCrossSecNo
        '
        Me.txtCrossSecNo.AcceptsReturn = True
        Me.txtCrossSecNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCrossSecNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCrossSecNo.Location = New System.Drawing.Point(112, 397)
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
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(128, 380)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Number"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 378)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Cross Section:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFreeHeight
        '
        Me.lblFreeHeight.BackColor = System.Drawing.Color.White
        Me.lblFreeHeight.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFreeHeight.Location = New System.Drawing.Point(47, 13)
        Me.lblFreeHeight.Name = "lblFreeHeight"
        Me.lblFreeHeight.Size = New System.Drawing.Size(100, 16)
        Me.lblFreeHeight.TabIndex = 1
        Me.lblFreeHeight.Text = "Free  Height  :"
        Me.lblFreeHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpConv
        '
        Me.grpConv.Controls.Add(Me.Label10)
        Me.grpConv.Controls.Add(Me.txtRadM3)
        Me.grpConv.Controls.Add(Me.Label11)
        Me.grpConv.Controls.Add(Me.updRadM4)
        Me.grpConv.Controls.Add(Me.updRadM2)
        Me.grpConv.Controls.Add(Me.updHConv)
        Me.grpConv.Controls.Add(Me.updThetaM2)
        Me.grpConv.Controls.Add(Me.txtHConv_New)
        Me.grpConv.Controls.Add(Me.txtRadM5_New)
        Me.grpConv.Controls.Add(Me.txtThetaM2_New)
        Me.grpConv.Controls.Add(Me.lblRadM5)
        Me.grpConv.Controls.Add(Me.txtRadM2)
        Me.grpConv.Controls.Add(Me.txtRadM5)
        Me.grpConv.Controls.Add(Me.lblRadM2)
        Me.grpConv.Controls.Add(Me.lblThetaM2)
        Me.grpConv.Controls.Add(Me.txtThetaM2)
        Me.grpConv.Controls.Add(Me.updRadM5)
        Me.grpConv.Controls.Add(Me.updRadM3)
        Me.grpConv.Controls.Add(Me.txtRadM3_New)
        Me.grpConv.Controls.Add(Me.updThetaM4)
        Me.grpConv.Controls.Add(Me.txtThetaM4_New)
        Me.grpConv.Controls.Add(Me.txtThetaM4)
        Me.grpConv.Controls.Add(Me.lblThetaM4)
        Me.grpConv.Controls.Add(Me.lblRadM4)
        Me.grpConv.Controls.Add(Me.txtRadM4)
        Me.grpConv.Controls.Add(Me.lblRadM3)
        Me.grpConv.Controls.Add(Me.updThetaM1)
        Me.grpConv.Controls.Add(Me.txtThetaM1)
        Me.grpConv.Controls.Add(Me.txtThetaM1_New)
        Me.grpConv.Controls.Add(Me.lblThetaM1)
        Me.grpConv.Controls.Add(Me.txtHConv)
        Me.grpConv.Controls.Add(Me.txtRadM2_New)
        Me.grpConv.Controls.Add(Me.lblHConv)
        Me.grpConv.Controls.Add(Me.txtRadM4_New)
        Me.grpConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpConv.ForeColor = System.Drawing.Color.Black
        Me.grpConv.Location = New System.Drawing.Point(279, 436)
        Me.grpConv.Name = "grpConv"
        Me.grpConv.Size = New System.Drawing.Size(275, 242)
        Me.grpConv.TabIndex = 6
        Me.grpConv.TabStop = False
        Me.grpConv.Text = "Convolutions:"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(205, 18)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 170
        Me.Label10.Text = "New"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRadM3
        '
        Me.txtRadM3.AcceptsReturn = True
        Me.txtRadM3.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM3.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadM3.Location = New System.Drawing.Point(112, 189)
        Me.txtRadM3.MaxLength = 0
        Me.txtRadM3.Name = "txtRadM3"
        Me.txtRadM3.ReadOnly = True
        Me.txtRadM3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM3.Size = New System.Drawing.Size(66, 21)
        Me.txtRadM3.TabIndex = 75
        Me.txtRadM3.TabStop = False
        Me.txtRadM3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(115, 19)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 13)
        Me.Label11.TabIndex = 169
        Me.Label11.Text = "Original"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'updRadM4
        '
        Me.updRadM4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadM4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadM4.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadM4.Location = New System.Drawing.Point(180, 113)
        Me.updRadM4.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadM4.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadM4.Name = "updRadM4"
        Me.updRadM4.Size = New System.Drawing.Size(20, 21)
        Me.updRadM4.TabIndex = 156
        '
        'updRadM2
        '
        Me.updRadM2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadM2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadM2.Location = New System.Drawing.Point(180, 61)
        Me.updRadM2.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadM2.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadM2.Name = "updRadM2"
        Me.updRadM2.Size = New System.Drawing.Size(20, 21)
        Me.updRadM2.TabIndex = 120
        '
        'updHConv
        '
        Me.updHConv.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updHConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updHConv.Location = New System.Drawing.Point(180, 138)
        Me.updHConv.Name = "updHConv"
        Me.updHConv.Size = New System.Drawing.Size(20, 21)
        Me.updHConv.TabIndex = 175
        '
        'updThetaM2
        '
        Me.updThetaM2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updThetaM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updThetaM2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updThetaM2.Location = New System.Drawing.Point(180, 35)
        Me.updThetaM2.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.updThetaM2.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updThetaM2.Name = "updThetaM2"
        Me.updThetaM2.Size = New System.Drawing.Size(20, 21)
        Me.updThetaM2.TabIndex = 137
        '
        'txtHConv_New
        '
        Me.txtHConv_New.AcceptsReturn = True
        Me.txtHConv_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtHConv_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHConv_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHConv_New.ForeColor = System.Drawing.Color.Black
        Me.txtHConv_New.Location = New System.Drawing.Point(202, 138)
        Me.txtHConv_New.MaxLength = 0
        Me.txtHConv_New.Name = "txtHConv_New"
        Me.txtHConv_New.Size = New System.Drawing.Size(60, 21)
        Me.txtHConv_New.TabIndex = 5
        Me.txtHConv_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRadM5_New
        '
        Me.txtRadM5_New.AcceptsReturn = True
        Me.txtRadM5_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM5_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM5_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM5_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadM5_New.Location = New System.Drawing.Point(202, 163)
        Me.txtRadM5_New.MaxLength = 0
        Me.txtRadM5_New.Name = "txtRadM5_New"
        Me.txtRadM5_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM5_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadM5_New.TabIndex = 12
        Me.txtRadM5_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaM2_New
        '
        Me.txtThetaM2_New.AcceptsReturn = True
        Me.txtThetaM2_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaM2_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaM2_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaM2_New.ForeColor = System.Drawing.Color.Black
        Me.txtThetaM2_New.Location = New System.Drawing.Point(202, 35)
        Me.txtThetaM2_New.MaxLength = 0
        Me.txtThetaM2_New.Name = "txtThetaM2_New"
        Me.txtThetaM2_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaM2_New.Size = New System.Drawing.Size(60, 21)
        Me.txtThetaM2_New.TabIndex = 17
        Me.txtThetaM2_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblRadM5
        '
        Me.lblRadM5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadM5.Location = New System.Drawing.Point(11, 163)
        Me.lblRadM5.Name = "lblRadM5"
        Me.lblRadM5.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRadM5.Size = New System.Drawing.Size(100, 16)
        Me.lblRadM5.TabIndex = 110
        Me.lblRadM5.Text = "External Conv R"
        Me.lblRadM5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRadM2
        '
        Me.txtRadM2.AcceptsReturn = True
        Me.txtRadM2.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM2.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadM2.Location = New System.Drawing.Point(112, 61)
        Me.txtRadM2.MaxLength = 0
        Me.txtRadM2.Name = "txtRadM2"
        Me.txtRadM2.ReadOnly = True
        Me.txtRadM2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM2.Size = New System.Drawing.Size(66, 21)
        Me.txtRadM2.TabIndex = 122
        Me.txtRadM2.TabStop = False
        Me.txtRadM2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRadM5
        '
        Me.txtRadM5.AcceptsReturn = True
        Me.txtRadM5.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM5.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadM5.Location = New System.Drawing.Point(112, 163)
        Me.txtRadM5.MaxLength = 0
        Me.txtRadM5.Name = "txtRadM5"
        Me.txtRadM5.ReadOnly = True
        Me.txtRadM5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM5.Size = New System.Drawing.Size(66, 21)
        Me.txtRadM5.TabIndex = 129
        Me.txtRadM5.TabStop = False
        Me.txtRadM5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblRadM2
        '
        Me.lblRadM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadM2.Location = New System.Drawing.Point(11, 61)
        Me.lblRadM2.Name = "lblRadM2"
        Me.lblRadM2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRadM2.Size = New System.Drawing.Size(100, 16)
        Me.lblRadM2.TabIndex = 121
        Me.lblRadM2.Text = "Outer Leg R"
        Me.lblRadM2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblThetaM2
        '
        Me.lblThetaM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThetaM2.Location = New System.Drawing.Point(11, 35)
        Me.lblThetaM2.Name = "lblThetaM2"
        Me.lblThetaM2.Size = New System.Drawing.Size(100, 14)
        Me.lblThetaM2.TabIndex = 133
        Me.lblThetaM2.Text = "Outer Leg Arc"
        Me.lblThetaM2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtThetaM2
        '
        Me.txtThetaM2.AcceptsReturn = True
        Me.txtThetaM2.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaM2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaM2.ForeColor = System.Drawing.Color.Magenta
        Me.txtThetaM2.Location = New System.Drawing.Point(112, 35)
        Me.txtThetaM2.MaxLength = 0
        Me.txtThetaM2.Name = "txtThetaM2"
        Me.txtThetaM2.ReadOnly = True
        Me.txtThetaM2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaM2.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaM2.TabIndex = 134
        Me.txtThetaM2.TabStop = False
        Me.txtThetaM2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'updRadM5
        '
        Me.updRadM5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadM5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadM5.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadM5.Location = New System.Drawing.Point(180, 163)
        Me.updRadM5.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadM5.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadM5.Name = "updRadM5"
        Me.updRadM5.Size = New System.Drawing.Size(20, 21)
        Me.updRadM5.TabIndex = 132
        '
        'updRadM3
        '
        Me.updRadM3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadM3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadM3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadM3.Location = New System.Drawing.Point(180, 189)
        Me.updRadM3.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadM3.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadM3.Name = "updRadM3"
        Me.updRadM3.Size = New System.Drawing.Size(20, 21)
        Me.updRadM3.TabIndex = 127
        '
        'txtRadM3_New
        '
        Me.txtRadM3_New.AcceptsReturn = True
        Me.txtRadM3_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM3_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM3_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM3_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadM3_New.Location = New System.Drawing.Point(202, 189)
        Me.txtRadM3_New.MaxLength = 0
        Me.txtRadM3_New.Name = "txtRadM3_New"
        Me.txtRadM3_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM3_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadM3_New.TabIndex = 10
        Me.txtRadM3_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'updThetaM4
        '
        Me.updThetaM4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updThetaM4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updThetaM4.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updThetaM4.Location = New System.Drawing.Point(180, 87)
        Me.updThetaM4.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.updThetaM4.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updThetaM4.Name = "updThetaM4"
        Me.updThetaM4.Size = New System.Drawing.Size(20, 21)
        Me.updThetaM4.TabIndex = 142
        '
        'txtThetaM4_New
        '
        Me.txtThetaM4_New.AcceptsReturn = True
        Me.txtThetaM4_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaM4_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaM4_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaM4_New.ForeColor = System.Drawing.Color.Black
        Me.txtThetaM4_New.Location = New System.Drawing.Point(202, 87)
        Me.txtThetaM4_New.MaxLength = 0
        Me.txtThetaM4_New.Name = "txtThetaM4_New"
        Me.txtThetaM4_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaM4_New.Size = New System.Drawing.Size(60, 21)
        Me.txtThetaM4_New.TabIndex = 18
        Me.txtThetaM4_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaM4
        '
        Me.txtThetaM4.AcceptsReturn = True
        Me.txtThetaM4.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaM4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaM4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaM4.ForeColor = System.Drawing.Color.Magenta
        Me.txtThetaM4.Location = New System.Drawing.Point(112, 87)
        Me.txtThetaM4.MaxLength = 0
        Me.txtThetaM4.Name = "txtThetaM4"
        Me.txtThetaM4.ReadOnly = True
        Me.txtThetaM4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaM4.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaM4.TabIndex = 139
        Me.txtThetaM4.TabStop = False
        Me.txtThetaM4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblThetaM4
        '
        Me.lblThetaM4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThetaM4.Location = New System.Drawing.Point(11, 87)
        Me.lblThetaM4.Name = "lblThetaM4"
        Me.lblThetaM4.Size = New System.Drawing.Size(100, 14)
        Me.lblThetaM4.TabIndex = 138
        Me.lblThetaM4.Text = "Inner Leg Arc"
        Me.lblThetaM4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRadM4
        '
        Me.lblRadM4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadM4.Location = New System.Drawing.Point(11, 113)
        Me.lblRadM4.Name = "lblRadM4"
        Me.lblRadM4.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRadM4.Size = New System.Drawing.Size(100, 16)
        Me.lblRadM4.TabIndex = 123
        Me.lblRadM4.Text = "Inner Leg R"
        Me.lblRadM4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRadM4
        '
        Me.txtRadM4.AcceptsReturn = True
        Me.txtRadM4.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM4.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadM4.Location = New System.Drawing.Point(112, 113)
        Me.txtRadM4.MaxLength = 0
        Me.txtRadM4.Name = "txtRadM4"
        Me.txtRadM4.ReadOnly = True
        Me.txtRadM4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM4.Size = New System.Drawing.Size(66, 21)
        Me.txtRadM4.TabIndex = 124
        Me.txtRadM4.TabStop = False
        Me.txtRadM4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblRadM3
        '
        Me.lblRadM3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadM3.Location = New System.Drawing.Point(11, 189)
        Me.lblRadM3.Name = "lblRadM3"
        Me.lblRadM3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRadM3.Size = New System.Drawing.Size(100, 14)
        Me.lblRadM3.TabIndex = 74
        Me.lblRadM3.Text = "Internal Conv R"
        Me.lblRadM3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'updThetaM1
        '
        Me.updThetaM1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updThetaM1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updThetaM1.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updThetaM1.Location = New System.Drawing.Point(180, 215)
        Me.updThetaM1.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.updThetaM1.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updThetaM1.Name = "updThetaM1"
        Me.updThetaM1.Size = New System.Drawing.Size(20, 21)
        Me.updThetaM1.TabIndex = 98
        '
        'txtThetaM1
        '
        Me.txtThetaM1.AcceptsReturn = True
        Me.txtThetaM1.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaM1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaM1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaM1.ForeColor = System.Drawing.Color.Magenta
        Me.txtThetaM1.Location = New System.Drawing.Point(112, 215)
        Me.txtThetaM1.MaxLength = 0
        Me.txtThetaM1.Name = "txtThetaM1"
        Me.txtThetaM1.ReadOnly = True
        Me.txtThetaM1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaM1.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaM1.TabIndex = 87
        Me.txtThetaM1.TabStop = False
        Me.txtThetaM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaM1_New
        '
        Me.txtThetaM1_New.AcceptsReturn = True
        Me.txtThetaM1_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaM1_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaM1_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaM1_New.ForeColor = System.Drawing.Color.Black
        Me.txtThetaM1_New.Location = New System.Drawing.Point(202, 215)
        Me.txtThetaM1_New.MaxLength = 0
        Me.txtThetaM1_New.Name = "txtThetaM1_New"
        Me.txtThetaM1_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaM1_New.Size = New System.Drawing.Size(60, 21)
        Me.txtThetaM1_New.TabIndex = 16
        Me.txtThetaM1_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblThetaM1
        '
        Me.lblThetaM1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThetaM1.Location = New System.Drawing.Point(11, 216)
        Me.lblThetaM1.Name = "lblThetaM1"
        Me.lblThetaM1.Size = New System.Drawing.Size(100, 14)
        Me.lblThetaM1.TabIndex = 83
        Me.lblThetaM1.Text = "Conv Arc"
        Me.lblThetaM1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtHConv
        '
        Me.txtHConv.AcceptsReturn = True
        Me.txtHConv.BackColor = System.Drawing.SystemColors.Window
        Me.txtHConv.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHConv.ForeColor = System.Drawing.Color.Blue
        Me.txtHConv.Location = New System.Drawing.Point(112, 138)
        Me.txtHConv.MaxLength = 0
        Me.txtHConv.Name = "txtHConv"
        Me.txtHConv.ReadOnly = True
        Me.txtHConv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHConv.Size = New System.Drawing.Size(66, 21)
        Me.txtHConv.TabIndex = 4
        Me.txtHConv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRadM2_New
        '
        Me.txtRadM2_New.AcceptsReturn = True
        Me.txtRadM2_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM2_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM2_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM2_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadM2_New.Location = New System.Drawing.Point(202, 61)
        Me.txtRadM2_New.MaxLength = 0
        Me.txtRadM2_New.Name = "txtRadM2_New"
        Me.txtRadM2_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM2_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadM2_New.TabIndex = 9
        Me.txtRadM2_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblHConv
        '
        Me.lblHConv.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHConv.Location = New System.Drawing.Point(11, 141)
        Me.lblHConv.Name = "lblHConv"
        Me.lblHConv.Size = New System.Drawing.Size(100, 14)
        Me.lblHConv.TabIndex = 62
        Me.lblHConv.Text = "Conv Width"
        Me.lblHConv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRadM4_New
        '
        Me.txtRadM4_New.AcceptsReturn = True
        Me.txtRadM4_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadM4_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadM4_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadM4_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadM4_New.Location = New System.Drawing.Point(202, 113)
        Me.txtRadM4_New.MaxLength = 0
        Me.txtRadM4_New.Name = "txtRadM4_New"
        Me.txtRadM4_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadM4_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadM4_New.TabIndex = 11
        Me.txtRadM4_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'picSeal
        '
        Me.picSeal.BackColor = System.Drawing.Color.White
        Me.picSeal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picSeal.Location = New System.Drawing.Point(3, 3)
        Me.picSeal.Name = "picSeal"
        Me.picSeal.Size = New System.Drawing.Size(858, 369)
        Me.picSeal.TabIndex = 0
        Me.picSeal.TabStop = False
        '
        'grpLeg
        '
        Me.grpLeg.Controls.Add(Me.Label12)
        Me.grpLeg.Controls.Add(Me.txtThetaE3)
        Me.grpLeg.Controls.Add(Me.txtThetaE1)
        Me.grpLeg.Controls.Add(Me.Label13)
        Me.grpLeg.Controls.Add(Me.txtThetaE2)
        Me.grpLeg.Controls.Add(Me.lblThetaE3)
        Me.grpLeg.Controls.Add(Me.txtThetaE1_New)
        Me.grpLeg.Controls.Add(Me.lblThetaE2)
        Me.grpLeg.Controls.Add(Me.lblRadE2)
        Me.grpLeg.Controls.Add(Me.lblThetaE1)
        Me.grpLeg.Controls.Add(Me.updThetaE1)
        Me.grpLeg.Controls.Add(Me.lblRadE3)
        Me.grpLeg.Controls.Add(Me.lblRadE1)
        Me.grpLeg.Controls.Add(Me.txtRadE1)
        Me.grpLeg.Controls.Add(Me.txtRadE2)
        Me.grpLeg.Controls.Add(Me.updThetaE3)
        Me.grpLeg.Controls.Add(Me.txtRadE3)
        Me.grpLeg.Controls.Add(Me.updRadE1)
        Me.grpLeg.Controls.Add(Me.updRadE3)
        Me.grpLeg.Controls.Add(Me.txtRadE2_New)
        Me.grpLeg.Controls.Add(Me.txtRadE3_New)
        Me.grpLeg.Controls.Add(Me.txtThetaE3_New)
        Me.grpLeg.Controls.Add(Me.txtThetaE2_New)
        Me.grpLeg.Controls.Add(Me.updThetaE2)
        Me.grpLeg.Controls.Add(Me.updRadE2)
        Me.grpLeg.Controls.Add(Me.txtRadE1_New)
        Me.grpLeg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpLeg.ForeColor = System.Drawing.Color.Black
        Me.grpLeg.Location = New System.Drawing.Point(567, 436)
        Me.grpLeg.Name = "grpLeg"
        Me.grpLeg.Size = New System.Drawing.Size(294, 191)
        Me.grpLeg.TabIndex = 183
        Me.grpLeg.TabStop = False
        Me.grpLeg.Text = "Leg:"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(221, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(60, 13)
        Me.Label12.TabIndex = 170
        Me.Label12.Text = "New"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtThetaE3
        '
        Me.txtThetaE3.AcceptsReturn = True
        Me.txtThetaE3.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaE3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaE3.ForeColor = System.Drawing.Color.Magenta
        Me.txtThetaE3.Location = New System.Drawing.Point(130, 35)
        Me.txtThetaE3.MaxLength = 0
        Me.txtThetaE3.Name = "txtThetaE3"
        Me.txtThetaE3.ReadOnly = True
        Me.txtThetaE3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaE3.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaE3.TabIndex = 86
        Me.txtThetaE3.TabStop = False
        Me.txtThetaE3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaE1
        '
        Me.txtThetaE1.AcceptsReturn = True
        Me.txtThetaE1.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaE1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaE1.ForeColor = System.Drawing.Color.Magenta
        Me.txtThetaE1.Location = New System.Drawing.Point(130, 139)
        Me.txtThetaE1.MaxLength = 0
        Me.txtThetaE1.Name = "txtThetaE1"
        Me.txtThetaE1.ReadOnly = True
        Me.txtThetaE1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaE1.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaE1.TabIndex = 89
        Me.txtThetaE1.TabStop = False
        Me.txtThetaE1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(131, 19)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 13)
        Me.Label13.TabIndex = 169
        Me.Label13.Text = "Original"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtThetaE2
        '
        Me.txtThetaE2.AcceptsReturn = True
        Me.txtThetaE2.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaE2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaE2.ForeColor = System.Drawing.Color.Magenta
        Me.txtThetaE2.Location = New System.Drawing.Point(130, 87)
        Me.txtThetaE2.MaxLength = 0
        Me.txtThetaE2.Name = "txtThetaE2"
        Me.txtThetaE2.ReadOnly = True
        Me.txtThetaE2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaE2.Size = New System.Drawing.Size(66, 21)
        Me.txtThetaE2.TabIndex = 85
        Me.txtThetaE2.TabStop = False
        Me.txtThetaE2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblThetaE3
        '
        Me.lblThetaE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThetaE3.Location = New System.Drawing.Point(11, 38)
        Me.lblThetaE3.Name = "lblThetaE3"
        Me.lblThetaE3.Size = New System.Drawing.Size(117, 14)
        Me.lblThetaE3.TabIndex = 82
        Me.lblThetaE3.Text = "Sealing Surface Arc"
        Me.lblThetaE3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtThetaE1_New
        '
        Me.txtThetaE1_New.AcceptsReturn = True
        Me.txtThetaE1_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaE1_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaE1_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaE1_New.ForeColor = System.Drawing.Color.Black
        Me.txtThetaE1_New.Location = New System.Drawing.Point(220, 139)
        Me.txtThetaE1_New.MaxLength = 0
        Me.txtThetaE1_New.Name = "txtThetaE1_New"
        Me.txtThetaE1_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaE1_New.Size = New System.Drawing.Size(60, 21)
        Me.txtThetaE1_New.TabIndex = 13
        Me.txtThetaE1_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblThetaE2
        '
        Me.lblThetaE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThetaE2.Location = New System.Drawing.Point(11, 89)
        Me.lblThetaE2.Name = "lblThetaE2"
        Me.lblThetaE2.Size = New System.Drawing.Size(117, 14)
        Me.lblThetaE2.TabIndex = 81
        Me.lblThetaE2.Text = "Waist Arc"
        Me.lblThetaE2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRadE2
        '
        Me.lblRadE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadE2.Location = New System.Drawing.Point(11, 113)
        Me.lblRadE2.Name = "lblRadE2"
        Me.lblRadE2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRadE2.Size = New System.Drawing.Size(117, 17)
        Me.lblRadE2.TabIndex = 71
        Me.lblRadE2.Text = "Waist R"
        Me.lblRadE2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblThetaE1
        '
        Me.lblThetaE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThetaE1.Location = New System.Drawing.Point(11, 139)
        Me.lblThetaE1.Name = "lblThetaE1"
        Me.lblThetaE1.Size = New System.Drawing.Size(117, 14)
        Me.lblThetaE1.TabIndex = 80
        Me.lblThetaE1.Text = "Leg Conv Arc"
        Me.lblThetaE1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'updThetaE1
        '
        Me.updThetaE1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updThetaE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updThetaE1.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updThetaE1.Location = New System.Drawing.Point(198, 139)
        Me.updThetaE1.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.updThetaE1.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updThetaE1.Name = "updThetaE1"
        Me.updThetaE1.Size = New System.Drawing.Size(20, 21)
        Me.updThetaE1.TabIndex = 97
        '
        'lblRadE3
        '
        Me.lblRadE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadE3.Location = New System.Drawing.Point(11, 62)
        Me.lblRadE3.Name = "lblRadE3"
        Me.lblRadE3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblRadE3.Size = New System.Drawing.Size(117, 17)
        Me.lblRadE3.TabIndex = 72
        Me.lblRadE3.Text = "Sealing Surface R"
        Me.lblRadE3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRadE1
        '
        Me.lblRadE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRadE1.Location = New System.Drawing.Point(11, 163)
        Me.lblRadE1.Name = "lblRadE1"
        Me.lblRadE1.Size = New System.Drawing.Size(117, 14)
        Me.lblRadE1.TabIndex = 70
        Me.lblRadE1.Text = "Leg Conv R"
        Me.lblRadE1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRadE1
        '
        Me.txtRadE1.AcceptsReturn = True
        Me.txtRadE1.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadE1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadE1.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadE1.Location = New System.Drawing.Point(130, 163)
        Me.txtRadE1.MaxLength = 0
        Me.txtRadE1.Name = "txtRadE1"
        Me.txtRadE1.ReadOnly = True
        Me.txtRadE1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadE1.Size = New System.Drawing.Size(66, 21)
        Me.txtRadE1.TabIndex = 79
        Me.txtRadE1.TabStop = False
        Me.txtRadE1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRadE2
        '
        Me.txtRadE2.AcceptsReturn = True
        Me.txtRadE2.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadE2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadE2.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadE2.Location = New System.Drawing.Point(130, 113)
        Me.txtRadE2.MaxLength = 0
        Me.txtRadE2.Name = "txtRadE2"
        Me.txtRadE2.ReadOnly = True
        Me.txtRadE2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadE2.Size = New System.Drawing.Size(66, 21)
        Me.txtRadE2.TabIndex = 78
        Me.txtRadE2.TabStop = False
        Me.txtRadE2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'updThetaE3
        '
        Me.updThetaE3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updThetaE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updThetaE3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updThetaE3.Location = New System.Drawing.Point(198, 35)
        Me.updThetaE3.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.updThetaE3.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updThetaE3.Name = "updThetaE3"
        Me.updThetaE3.Size = New System.Drawing.Size(20, 21)
        Me.updThetaE3.TabIndex = 153
        '
        'txtRadE3
        '
        Me.txtRadE3.AcceptsReturn = True
        Me.txtRadE3.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadE3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadE3.ForeColor = System.Drawing.Color.Magenta
        Me.txtRadE3.Location = New System.Drawing.Point(130, 61)
        Me.txtRadE3.MaxLength = 0
        Me.txtRadE3.Name = "txtRadE3"
        Me.txtRadE3.ReadOnly = True
        Me.txtRadE3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadE3.Size = New System.Drawing.Size(66, 21)
        Me.txtRadE3.TabIndex = 77
        Me.txtRadE3.TabStop = False
        Me.txtRadE3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'updRadE1
        '
        Me.updRadE1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadE1.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadE1.Location = New System.Drawing.Point(198, 163)
        Me.updRadE1.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadE1.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadE1.Name = "updRadE1"
        Me.updRadE1.Size = New System.Drawing.Size(20, 21)
        Me.updRadE1.TabIndex = 108
        '
        'updRadE3
        '
        Me.updRadE3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadE3.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadE3.Location = New System.Drawing.Point(198, 61)
        Me.updRadE3.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadE3.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadE3.Name = "updRadE3"
        Me.updRadE3.Size = New System.Drawing.Size(20, 21)
        Me.updRadE3.TabIndex = 114
        '
        'txtRadE2_New
        '
        Me.txtRadE2_New.AcceptsReturn = True
        Me.txtRadE2_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadE2_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadE2_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadE2_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadE2_New.Location = New System.Drawing.Point(220, 113)
        Me.txtRadE2_New.MaxLength = 0
        Me.txtRadE2_New.Name = "txtRadE2_New"
        Me.txtRadE2_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadE2_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadE2_New.TabIndex = 7
        Me.txtRadE2_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRadE3_New
        '
        Me.txtRadE3_New.AcceptsReturn = True
        Me.txtRadE3_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadE3_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadE3_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadE3_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadE3_New.Location = New System.Drawing.Point(220, 61)
        Me.txtRadE3_New.MaxLength = 0
        Me.txtRadE3_New.Name = "txtRadE3_New"
        Me.txtRadE3_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadE3_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadE3_New.TabIndex = 8
        Me.txtRadE3_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaE3_New
        '
        Me.txtThetaE3_New.AcceptsReturn = True
        Me.txtThetaE3_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaE3_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaE3_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaE3_New.ForeColor = System.Drawing.Color.Black
        Me.txtThetaE3_New.Location = New System.Drawing.Point(220, 35)
        Me.txtThetaE3_New.MaxLength = 0
        Me.txtThetaE3_New.Name = "txtThetaE3_New"
        Me.txtThetaE3_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaE3_New.Size = New System.Drawing.Size(60, 21)
        Me.txtThetaE3_New.TabIndex = 15
        Me.txtThetaE3_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaE2_New
        '
        Me.txtThetaE2_New.AcceptsReturn = True
        Me.txtThetaE2_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaE2_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaE2_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaE2_New.ForeColor = System.Drawing.Color.Black
        Me.txtThetaE2_New.Location = New System.Drawing.Point(220, 87)
        Me.txtThetaE2_New.MaxLength = 0
        Me.txtThetaE2_New.Name = "txtThetaE2_New"
        Me.txtThetaE2_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThetaE2_New.Size = New System.Drawing.Size(60, 21)
        Me.txtThetaE2_New.TabIndex = 14
        Me.txtThetaE2_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'updThetaE2
        '
        Me.updThetaE2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updThetaE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updThetaE2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updThetaE2.Location = New System.Drawing.Point(198, 87)
        Me.updThetaE2.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.updThetaE2.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updThetaE2.Name = "updThetaE2"
        Me.updThetaE2.Size = New System.Drawing.Size(20, 21)
        Me.updThetaE2.TabIndex = 150
        '
        'updRadE2
        '
        Me.updRadE2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRadE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRadE2.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.updRadE2.Location = New System.Drawing.Point(198, 113)
        Me.updRadE2.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.updRadE2.Minimum = New Decimal(New Integer() {15, 0, 0, -2147483648})
        Me.updRadE2.Name = "updRadE2"
        Me.updRadE2.Size = New System.Drawing.Size(20, 21)
        Me.updRadE2.TabIndex = 111
        '
        'txtRadE1_New
        '
        Me.txtRadE1_New.AcceptsReturn = True
        Me.txtRadE1_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRadE1_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRadE1_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRadE1_New.ForeColor = System.Drawing.Color.Black
        Me.txtRadE1_New.Location = New System.Drawing.Point(220, 163)
        Me.txtRadE1_New.MaxLength = 0
        Me.txtRadE1_New.Name = "txtRadE1_New"
        Me.txtRadE1_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRadE1_New.Size = New System.Drawing.Size(60, 21)
        Me.txtRadE1_New.TabIndex = 6
        Me.txtRadE1_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PrintDocu
        '
        Me.PrintDocu.DocumentName = ""
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuStrip1.AutoSize = False
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPrintForm})
        Me.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(888, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuPrintForm
        '
        Me.mnuPrintForm.AutoSize = False
        Me.mnuPrintForm.Image = CType(resources.GetObject("mnuPrintForm.Image"), System.Drawing.Image)
        Me.mnuPrintForm.Name = "mnuPrintForm"
        Me.mnuPrintForm.Size = New System.Drawing.Size(81, 20)
        Me.mnuPrintForm.Text = " &PrintForm"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
        Me.ErrorProvider1.ContainerControl = Me
        '
        'IPE_frmDesignCenterESeal
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(889, 748)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmDesignCenterESeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Design Center - ESeal"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grpGenParam.ResumeLayout(False)
        Me.grpGenParam.PerformLayout()
        CType(Me.updNConv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updIndexTArray, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpConv.ResumeLayout(False)
        Me.grpConv.PerformLayout()
        CType(Me.updRadM4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRadM2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updHConv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updThetaM2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRadM5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRadM3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updThetaM4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updThetaM1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpLeg.ResumeLayout(False)
        Me.grpLeg.PerformLayout()
        CType(Me.updThetaE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updThetaE3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRadE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRadE3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updThetaE2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRadE2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region


#Region "FORM CONSTRUCTOR & RELATED ROUTINES:"

    '*******************************************************************************
    '*              FORM CONSTRUCTOR & RELATED ROUTINES - BEGIN                    *
    '*******************************************************************************

    Public Sub New()
        '===========

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        '....Logo.
        'LoadImageLogo(imgLogo)

        '   ------------
        '   Picture Box: 
        '   ------------
        '....Set Margin of the Picture Box. 
        Dim pUniformMargin As Single = 0.4       '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        '....Margins Calculation  
        'Const pcUniformMargin As Single = 0.4   '....Uniform margin around the
        ''                                       '........Picture Box - (in)

        'Dim psngMargin As Single
        'psngMargin = gIPE_Unit.EngLToUserL(pcUniformMargin) '....In user unit (in or mm)

        ''....Margins around the graphics in the picture box.
        'mMargin(1) = psngMargin                 '....Left
        'mMargin(2) = psngMargin                 '....Right

        ''....The margin at the top is 60% of the total height margin and
        ''....at the bottom is the rest 40%.
        'mMargin(3) = 2 * psngMargin * 0.6       '....Top
        'mMargin(4) = 2 * psngMargin * 0.4       '....Bottom
        '-------------------------------------------------------------------------------

        '   ---------------
        '   UpDown Buttons:
        '   ---------------    

        '   Set the Properties of the UpDown Buttons.
        '   -----------------------------------------
        SetProperties_UpDownButton_All()

    End Sub

    '*******************************************************************************
    '*                FORM CONSTRUCTOR & RELATED ROUTINES - END                    *
    '*******************************************************************************

#End Region


#Region "FORM LOAD EVENT & RELATED ROUTINES:"


    Private Sub frmDesignCenterESeal_Activated(ByVal sender As System.Object, _
                                              ByVal e As System.EventArgs) _
                                              Handles MyBase.Activated
        '=====================================================================     
        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then _
            cmdViewNomenclature.Enabled = gIPE_frmNomenclature_DesignCenter.FormClose
    End Sub


    Private Sub frmDesignCentreESeal_Load(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles MyBase.Load
        '===========================================================================

        '   INITIALIZATIONS:
        '   ----------------
        Initialize_LocalSealObject()                 '....gSeal ===> mESeal
        Initialize_Controls()

        '....Display data on the form.  
        '........Local seal object "mSeal" is used.
        DisplayData()

        '....Display graphics on the picture box.
        DoGraphics()


    End Sub


#Region "HELPER ROUTINES:"

    Private Sub Initialize_LocalSealObject()
        '===================================
        '....From gSeal ===> mSeal. 
        '........Now onwards, mSeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal only in the 
        '........"SaveData" routine which is called when the form is exited and/or 
        '........another form is opened either modal or non-modal.     

        '....Create & initialize the local Seal Object.     
        mESeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo <> "" Then
            'mESeal = CType(gSeal, IPE_clsESeal).Clone()
            mESeal = CType(gIPE_SealNew, IPE_clsESeal).Clone()
        End If

        '....The local seal object may be redesigned on this form, starting with the   
        '........design parameters of the already existing global ESeal object "gSeal".
        '........Hence, the following property setting is necessary.  

        If mESeal.NewDesign = False Then _
            mESeal.MCrossSecNo = ""


        '....The above assignment will automatically set NewDesign = TRUE, if it is not
        '........so to start with.

    End Sub


    Private Sub Initialize_Controls()
        '============================

        '   Reference Parameters:
        '   ---------------------
        '   TemplateNo = "1Gen" & "1GenS': HLeg.
        '   TemplateNo = "1Gen" only     : HConv.    


        'Template-independent Control settings:
        '---------------------------------------
        '....The following two controls can be changed only thru' the corresponding 
        '........up-down buttons.
        txtNConv_New.ReadOnly = True
        txtT_New.ReadOnly = True

        'txtHLeg_New.ReadOnly = True   '....Always a ref. variable. (No up-down control).


        'Template-dependent Control settings:
        '------------------------------------
        If mESeal.TemplateNo = "1Gen" Then
            txtHConv.ForeColor = Color.Blue         '....Ref. Value.

        ElseIf mESeal.TemplateNo = "1GenS" Then
            txtHConv.ForeColor = Color.Magenta      '....Independent parameter.
        End If


        Dim pbln_1Gen As Boolean
        pbln_1Gen = IIf(mESeal.TemplateNo = "1Gen", True, False)

        '....HConv related controls:
        'lblRSpan.Visible = Not pbln_1Gen
        lblHConv.Visible = Not pbln_1Gen
        txtHConv.Visible = Not pbln_1Gen
        'lblHConvRef.Visible = Not pbln_1Gen
        txtHConv_New.Visible = Not pbln_1Gen
        txtHConv_New.ReadOnly = pbln_1Gen

        With updHConv
            .Visible = Not pbln_1Gen
            .Enabled = Not pbln_1Gen
        End With


        '....Segment M2 related controls:
        lblRadM2.Visible = pbln_1Gen
        updRadM2.Visible = pbln_1Gen
        txtRadM2.Visible = pbln_1Gen
        txtRadM2_New.Visible = pbln_1Gen

        lblThetaM2.Visible = pbln_1Gen
        updThetaM2.Visible = pbln_1Gen
        txtThetaM2.Visible = pbln_1Gen
        txtThetaM2_New.Visible = pbln_1Gen


        '....Segment M4 related controls:
        lblRadM4.Visible = pbln_1Gen
        updRadM4.Visible = pbln_1Gen
        txtRadM4.Visible = pbln_1Gen
        txtRadM4_New.Visible = pbln_1Gen

        lblThetaM4.Visible = pbln_1Gen
        updThetaM4.Visible = pbln_1Gen
        txtThetaM4.Visible = pbln_1Gen
        txtThetaM4_New.Visible = pbln_1Gen

        Dim pTop As Integer = 0, pIncr As Integer = 26
        'pTop = IIf(mESeal.TemplateNo = "1GenS", 35, 138)
        pTop = IIf(CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).TemplateNo = "1GenS", 35, 138)
        lblHConv.Top = pTop : txtHConv.Top = pTop : updHConv.Top = pTop : txtHConv_New.Top = pTop
        pTop += 26
        lblRadM5.Top = pTop : txtRadM5.Top = pTop : updRadM5.Top = pTop : txtRadM5_New.Top = pTop
        pTop += 26
        lblRadM3.Top = pTop : txtRadM3.Top = pTop : updRadM3.Top = pTop : txtRadM3_New.Top = pTop
        pTop += 26
        lblThetaM1.Top = pTop : txtThetaM1.Top = pTop : updThetaM1.Top = pTop : txtThetaM1_New.Top = pTop

        'grpConv.Height = IIf(mESeal.TemplateNo = "1GenS", 140, 242)
        grpConv.Height = IIf(CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).TemplateNo = "1GenS", 140, 242)

        'AES 30MAR16
        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            txtCrossSecNo_New.Enabled = False

            txtNConv_New.Enabled = False
            updNConv.Enabled = False

            txtT_New.Enabled = False
            updIndexTArray.Enabled = False

            txtThetaM2_New.Enabled = False
            updThetaM2.Enabled = False

            txtRadM2_New.Enabled = False
            updRadM2.Enabled = False

            txtThetaM4_New.Enabled = False
            updThetaM4.Enabled = False

            txtRadM4_New.Enabled = False
            updRadM4.Enabled = False

            txtHConv_New.Enabled = False
            updHConv.Enabled = False

            txtRadM5_New.Enabled = False
            updRadM5.Enabled = False

            txtRadM3_New.Enabled = False
            updRadM3.Enabled = False

            txtThetaM1_New.Enabled = False
            updThetaM1.Enabled = False

            txtThetaE3_New.Enabled = False
            updThetaE3.Enabled = False

            txtRadE3_New.Enabled = False
            updRadE3.Enabled = False

            txtThetaE2_New.Enabled = False
            updThetaE2.Enabled = False

            txtRadE2_New.Enabled = False
            updRadE2.Enabled = False

            txtThetaE1_New.Enabled = False
            updThetaE1.Enabled = False

            txtRadE1_New.Enabled = False
            updRadE1.Enabled = False

        Else
            txtCrossSecNo_New.Enabled = True

            txtNConv_New.Enabled = True
            updNConv.Enabled = True

            txtT_New.Enabled = True
            updIndexTArray.Enabled = True

            txtThetaM2_New.Enabled = True
            updThetaM2.Enabled = True

            txtRadM2_New.Enabled = True
            updRadM2.Enabled = True

            txtThetaM4_New.Enabled = True
            updThetaM4.Enabled = True

            txtRadM4_New.Enabled = True
            updRadM4.Enabled = True

            txtHConv_New.Enabled = True
            updHConv.Enabled = True

            txtRadM5_New.Enabled = True
            updRadM5.Enabled = True

            txtRadM3_New.Enabled = True
            updRadM3.Enabled = True

            txtThetaM1_New.Enabled = True
            updThetaM1.Enabled = True

            txtThetaE3_New.Enabled = True
            updThetaE3.Enabled = True

            txtRadE3_New.Enabled = True
            updRadE3.Enabled = True

            txtThetaE2_New.Enabled = True
            updThetaE2.Enabled = True

            txtRadE2_New.Enabled = True
            updRadE2.Enabled = True

            txtThetaE1_New.Enabled = True
            updThetaE1.Enabled = True

            txtRadE1_New.Enabled = True
            updRadE1.Enabled = True
        End If
    End Sub


    Private Sub DisplayData()
        '====================
        '....This routine displays the original seal object and the current state of the
        '........local object. 

        'MsgBox("frmDesignCentereESeal: DisplayData")  '....Diagnostic Statement.

        '   ORIGINAL PARAMETERS:  (for reference)
        '   ====================  
        '
        '   ....Display the original global seal object parameters. 
        '
        With CType(gIPE_SealOrg, IPE_clsESeal)
            'With CType(gSeal, IPE_clsESeal)
            txtCrossSecNo.Text = .MCrossSecNo
            txtCrossSecNo.BackColor = Color.Gainsboro

            txtCrossSecNo_New.Text = CType(gIPE_SealNew, IPE_clsESeal).MCrossSecNo

            txtTemplateNo.Text = .TemplateNo
            txtTemplateNo.BackColor = Color.Gainsboro

            txtPOrient.Text = .POrient
            txtPOrient.BackColor = Color.Gainsboro

            txtDControl.Text = gIPE_Unit.WriteInUserL(.DControl)
            txtDControl.BackColor = Color.Gainsboro

            txtNConv.Text = .NConv
            txtNConv.BackColor = Color.Gainsboro    'AM 18JAN10

            txtT.Text = gIPE_Unit.WriteInUserL(.T)
            txtT.BackColor = Color.Gainsboro

            txtHConv.Text = gIPE_Unit.WriteInUserL(.HConv)
            txtHConv.BackColor = Color.Gainsboro

            'txtHLeg.Text = gIPE_Unit.WriteInUserL(.HLeg)
            'txtHLeg.BackColor = Color.Gainsboro


            '   End 1/2 Convolution:
            '   --------------------
            '
            '....Radii:
            '
            txtRadE1.Text = gIPE_Unit.WriteInUserL(.RadE(1))
            txtRadE1.BackColor = Color.Gainsboro

            txtRadE2.Text = gIPE_Unit.WriteInUserL(.RadE(2))
            txtRadE2.BackColor = Color.Gainsboro

            txtRadE3.Text = gIPE_Unit.WriteInUserL(.RadE(3))
            txtRadE3.BackColor = Color.Gainsboro


            '....Angles:    
            '
            txtThetaE1.Text = gIPE_Unit.WriteAngle(.ThetaE(1))
            txtThetaE1.BackColor = Color.Gainsboro

            txtThetaE2.Text = gIPE_Unit.WriteAngle(.ThetaE(2))
            txtThetaE2.BackColor = Color.Gainsboro

            txtThetaE3.Text = gIPE_Unit.WriteAngle(.ThetaE(3))
            txtThetaE3.BackColor = Color.Gainsboro


            '   Mid 1/2 Convolutions:
            '   ---------------------
            '
            '....Radii:
            '
            txtRadM3.Text = gIPE_Unit.WriteInUserL(.RadM(3))
            txtRadM3.BackColor = Color.Gainsboro

            txtRadM5.Text = gIPE_Unit.WriteInUserL(.RadM(5))
            txtRadM5.BackColor = Color.Gainsboro


            If .TemplateNo = "1Gen" Then
                txtRadM2.Text = gIPE_Unit.WriteInUserL(.RadM(2))
                txtRadM2.BackColor = Color.Gainsboro

                txtRadM4.Text = gIPE_Unit.WriteInUserL(.RadM(4))
                txtRadM4.BackColor = Color.Gainsboro

            ElseIf .TemplateNo = "1GenS" Then
                txtRadM2.Text = "Straight"
                txtRadM2.ForeColor = Color.Blue
                txtRadM2.BackColor = Color.Gainsboro

                txtRadM4.Text = "Straight"
                txtRadM4.ForeColor = Color.Blue
                txtRadM4.BackColor = Color.Gainsboro

                txtThetaM2.ForeColor = Color.Blue
                txtThetaM2.BackColor = Color.Gainsboro

                txtThetaM4.ForeColor = Color.Blue
                txtThetaM4.BackColor = Color.Gainsboro
            End If


            '....Angles:        
            txtThetaM1.Text = gIPE_Unit.WriteAngle(.ThetaM(1))
            txtThetaM1.BackColor = Color.Gainsboro

            txtThetaM2.Text = gIPE_Unit.WriteAngle(.ThetaM(2))
            txtThetaM2.BackColor = Color.Gainsboro

            'txtThetaM3.Text = gIPE_Unit.WriteAngle(.ThetaM(3))
            'txtThetaM3.BackColor = Color.Gainsboro

            txtThetaM4.Text = gIPE_Unit.WriteAngle(.ThetaM(4))
            txtThetaM4.BackColor = Color.Gainsboro

            'txtThetaM5.Text = gIPE_Unit.WriteAngle(.ThetaM(5))
            'txtThetaM5.BackColor = Color.Gainsboro

        End With


        '   MODIFIED PARAMETERS:
        '   ====================
        '   ....Display the local seal object parameters as initialization. 
        '   ........They may be modified by the user later. 
        '
        With mESeal

            If IPE_clsESeal.CrossSecList.Contains(.MCrossSecNo) Then
                '....A new cross-section number has not been assigned yet.
                'txtCrossSecNo_New.Text = ""
            Else
                txtCrossSecNo_New.Text = CType(gIPE_SealNew, IPE_clsESeal).MCrossSecNo

                If IPE_clsESeal.CrossSecNewList.Count > 0 And _
                        Not IPE_clsESeal.CrossSecNewList.Contains(CType(gIPE_SealNew, IPE_clsESeal).MCrossSecNo) And _
                            Not IPE_clsESeal.CrossSecList.Contains(txtCrossSecNo_New.Text.Trim()) Then
                    txtCrossSecNo_New.ForeColor = Color.Green
                Else
                    txtCrossSecNo_New.ForeColor = Color.Black
                End If
            End If

            updNConv.Value = .NConv
            txtNConv_New.Text = .NConv

            txtT_New.Text = gIPE_Unit.WriteInUserL(.T)

            If mESeal.TemplateNo = "1GenS" Then
                updHConv.Value = gIPE_Unit.WriteInUserL(.HConv)  '....HConv: Independent var.
            End If

            txtHConv_New.Text = gIPE_Unit.WriteInUserL(.HConv)
            'txtHLeg_New.Text = gIPE_Unit.WriteInUserL(.HLeg)


            '   End 1/2 Convolution:
            '   --------------------
            '
            '....Radii:
            txtRadE1_New.Text = gIPE_Unit.WriteInUserL(.RadE(1))
            txtRadE2_New.Text = gIPE_Unit.WriteInUserL(.RadE(2))
            txtRadE3_New.Text = gIPE_Unit.WriteInUserL(.RadE(3))

            '....Angles:                   
            txtThetaE1_New.Text = gIPE_Unit.WriteAngle(.ThetaE(1))
            txtThetaE2_New.Text = gIPE_Unit.WriteAngle(.ThetaE(2))
            txtThetaE3_New.Text = gIPE_Unit.WriteAngle(.ThetaE(3))


            '   Mid 1/2 Convolutions:
            '   ---------------------
            '
            '....Radii:     
            txtRadM3_New.Text = gIPE_Unit.WriteInUserL(.RadM(3))
            txtRadM5_New.Text = gIPE_Unit.WriteInUserL(.RadM(5))

            If .TemplateNo = "1Gen" Then
                txtRadM2_New.Text = gIPE_Unit.WriteInUserL(.RadM(2))
                txtRadM4_New.Text = gIPE_Unit.WriteInUserL(.RadM(4))
            End If


            '....Angles:       
            txtThetaM1_New.Text = gIPE_Unit.WriteAngle(.ThetaM(1))
            'txtThetaM3_New.Text = gIPE_Unit.WriteAngle(.ThetaM(3))
            'txtThetaM5_New.Text = gIPE_Unit.WriteAngle(.ThetaM(5))

            If .TemplateNo = "1Gen" Then
                txtThetaM2_New.Text = gIPE_Unit.WriteAngle(.ThetaM(2))
                txtThetaM4_New.Text = gIPE_Unit.WriteAngle(.ThetaM(4))
            End If

        End With

    End Sub

#End Region


#End Region


#Region "CONTROL EVENT ROUTINES:"


#Region "TEXT BOX RELATED ROUTINES:"


    Private Sub txt_New_TextChanged(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) _
                                    Handles txtNConv_New.TextChanged, _
                                    txtT_New.TextChanged, _
                                    txtHConv_New.TextChanged, _
                                    txtRadE1_New.TextChanged, _
                                    txtRadE2_New.TextChanged, _
                                    txtRadE3_New.TextChanged, _
                                    txtRadM2_New.TextChanged, _
                                    txtRadM3_New.TextChanged, _
                                    txtRadM4_New.TextChanged, _
                                    txtRadM5_New.TextChanged, _
                                    txtThetaE1_New.TextChanged, _
                                    txtThetaE2_New.TextChanged, _
                                    txtThetaE3_New.TextChanged, _
                                    txtThetaM1_New.TextChanged, _
                                    txtThetaM2_New.TextChanged, _
                                    txtThetaM4_New.TextChanged
        '================================================================
        If mESeal Is Nothing = True Then Exit Sub

        Dim pTxtBox As TextBox = CType(sender, TextBox)

        With mESeal

            '   Select the Up-Down button corresponding to the text box.
            '   --------------------------------------------------------
            Dim pUpd As New NumericUpDown
            Dim pVal_Org As Single, pVal_New As Single

            Select Case pTxtBox.Name

                Case "txtNConv_New"
                    '.NConv = Val(txtNConv_New.Text)
                    .NConv = ConvertToInt(txtNConv_New.Text)
                    pVal_Org = ConvertToInt(txtNConv.Text)


                Case "txtT_New"
                    '.T = ConvertToSng(txtT_New.Text)
                    pVal_Org = ConvertToSng(txtT.Text)
                    pVal_New = ConvertToSng(txtT_New.Text)


                Case "txtHConv_New"

                    If .TemplateNo = "1GenS" Then
                        pUpd = updHConv
                        pVal_New = ConvertToSng(txtHConv_New.Text)

                    End If

                    pVal_Org = ConvertToSng(txtHConv.Text)


                Case "txtHLeg_New"
                    'pVal_Org = ConvertToSng(txtHLeg.Text)


                    '   Radii:
                    '   ------
                    '
                Case "txtRadE1_New"
                    pUpd = updRadE1
                    pVal_Org = ConvertToSng(txtRadE1.Text)
                    pVal_New = ConvertToSng(txtRadE1_New.Text)

                    .Check_TxtRad_Val(pTxtBox, pVal_New)


                Case "txtRadE2_New"
                    pUpd = updRadE2
                    pVal_Org = ConvertToSng(txtRadE2.Text)
                    pVal_New = ConvertToSng(txtRadE2_New.Text)

                    .Check_TxtRad_Val(pTxtBox, pVal_New)

                Case "txtRadE3_New"
                    pUpd = updRadE3
                    pVal_Org = ConvertToSng(txtRadE3.Text)
                    pVal_New = ConvertToSng(txtRadE3_New.Text)


                    .Check_TxtRad_Val(pTxtBox, pVal_New)


                Case "txtRadM2_New"

                    If .TemplateNo = "1Gen" Then
                        pUpd = updRadM2
                        pVal_Org = ConvertToSng(txtRadM2.Text)
                        pVal_New = ConvertToSng(txtRadM2_New.Text)


                        .Check_TxtRad_Val(pTxtBox, pVal_New)
                    End If


                Case "txtRadM3_New"
                    pUpd = updRadM3
                    pVal_Org = ConvertToSng(txtRadM3.Text)
                    pVal_New = ConvertToSng(txtRadM3_New.Text)


                    .Check_TxtRad_Val(pTxtBox, pVal_New)


                Case "txtRadM4_New"
                    If .TemplateNo = "1Gen" Then
                        pUpd = updRadM4
                        pVal_Org = ConvertToSng(txtRadM4.Text)
                        pVal_New = ConvertToSng(txtRadM4_New.Text)


                        .Check_TxtRad_Val(pTxtBox, pVal_New)
                    End If

                Case "txtRadM5_New"
                    pUpd = updRadM5
                    pVal_Org = ConvertToSng(txtRadM5.Text)
                    pVal_New = ConvertToSng(txtRadM5_New.Text)


                    .Check_TxtRad_Val(pTxtBox, pVal_New)

                    '   Angles:
                    '   -------
                    '
                Case "txtThetaE1_New"
                    pUpd = updThetaE1
                    pVal_Org = ConvertToSng(txtThetaE1.Text)
                    pVal_New = ConvertToSng(txtThetaE1_New.Text)


                    .Check_TxtTheta_Val(pTxtBox, pVal_New)


                Case "txtThetaE2_New"
                    pUpd = updThetaE2
                    pVal_Org = ConvertToSng(txtThetaE2.Text)
                    pVal_New = ConvertToSng(txtThetaE2_New.Text)


                    .Check_TxtTheta_Val(pTxtBox, pVal_New)


                Case "txtThetaE3_New"
                    pUpd = updThetaE3
                    pVal_Org = ConvertToSng(txtThetaE3.Text)
                    pVal_New = ConvertToSng(txtThetaE3_New.Text)

                    .Check_TxtTheta_Val(pTxtBox, pVal_New)


                Case "txtThetaM1_New"
                    pUpd = updThetaM1
                    pVal_Org = ConvertToSng(txtThetaM1.Text)
                    pVal_New = ConvertToSng(txtThetaM1_New.Text)


                    .Check_TxtTheta_Val(pTxtBox, pVal_New)


                Case "txtThetaM2_New"

                    If .TemplateNo = "1Gen" Then
                        pUpd = updThetaM2
                        pVal_Org = ConvertToSng(txtThetaM2.Text)
                        pVal_New = ConvertToSng(txtThetaM2_New.Text)


                        .Check_TxtTheta_Val(pTxtBox, pVal_New)
                    End If


                Case "txtThetaM4_New"

                    If .TemplateNo = "1Gen" Then
                        pUpd = updThetaM4
                        pVal_Org = ConvertToSng(txtThetaM4.Text)
                        pVal_New = ConvertToSng(txtThetaM4_New.Text)


                        .Check_TxtTheta_Val(pTxtBox, pVal_New)
                    End If

                Case Else
                    Exit Sub

            End Select


            '   Check the Text Box Value, reset if necessary, and set its color.
            '   ----------------------------------------------------------------
            '   ....Reset if necessary.
            '
            SetValue_TxtBox(pUpd, pTxtBox, pVal_New)
            SetForeColor(pTxtBox, pVal_Org)


            '....Re-store the text box value (in case it has been reset above).
            pVal_New = ConvertToSng(pTxtBox.Text)


            '   Reset Updown Button Value (if necessary only):
            '   ---------------------------------------------
            '   ....Do resetting only if the updown button value is different from 
            '   ........the text box value. 
            If Abs(pUpd.Value - pVal_New) > gcEPS Then _
                AssignValue_UpDownButton(pUpd, pVal_New)



            '   Store the "New" values in the local seal object.
            '   ------------------------------------------------
            '

            Select Case pTxtBox.Name
                Case "txtT_New"
                    .T = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtHConv_New"
                    .HConv = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadE1_New"
                    .RadE(1) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadE2_New"
                    .RadE(2) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadE3_New"
                    .RadE(3) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadM2_New"
                    .RadM(2) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadM3_New"
                    .RadM(3) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadM4_New"
                    .RadM(4) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtRadM5_New"
                    .RadM(5) = gIPE_Unit.L_UserToCon(pVal_New)
                Case "txtThetaE1_New"
                    .ThetaE(1) = pVal_New

                Case "txtThetaE2_New"
                    .ThetaE(2) = pVal_New

                Case "txtThetaE3_New"
                    .ThetaE(3) = pVal_New

                Case "txtThetaM1_New"
                    .ThetaM(1) = pVal_New

                Case "txtThetaM2_New"
                    .ThetaM(2) = pVal_New

                Case "txtThetaM4_New"
                    .ThetaM(4) = pVal_New

            End Select


            Dim pblnRad As Boolean = .Check_InnerRad()
            Dim pblnTheta As Boolean = .Check_Graphics_Distortion()


            If (Not pblnRad Or Not pblnTheta) Then
                ErrorProvider1.SetError(pTxtBox, "Current value is not allowed.")
                ErrorProvider1.GetError(pTxtBox)
            Else
                ErrorProvider1.Clear()
            End If


            '....Error Msg for Thickness
            Dim pMsg As String = ""
            If pblnRad = False And pTxtBox.Name = "txtT_New" Then
                pMsg = "WARNING: Thickness value is not allowable."
            End If

            lblErrT.Text = pMsg

            '   Display Derived Parameters:
            '   ---------------------------
            Update_Display_RefVariables()


            '   Redo the Graphics.
            '   ------------------
            DoGraphics()


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
            txtbox_In.Text = ConvertToStr(updown_In.Minimum, "#0.000")


        ElseIf pVal > updown_In.Maximum Then
            txtbox_In.Text = ConvertToStr(updown_In.Maximum, "#0.000")
        End If

    End Sub


    Private Sub Check_Theta(ByRef textBox_In As TextBox)
        '===============================================
        Dim pVal As Single

        '....Check if the entered value exceeds the limits.
        pVal = Val(textBox_In.Text)

        If pVal > mcThetaMax Then
            textBox_In.Text = mcThetaMax


        ElseIf pVal < mcThetaMin Then
            textBox_In.Text = mcThetaMin

        End If

    End Sub


    Private Sub Check_Rad(ByRef textBox_In As TextBox)
        '=============================================   
        Dim pVal As Single

        '....Check if the entered value exceeds the limits.
        pVal = Val(textBox_In.Text)

        If pVal > mcRadMax Then
            '....Reset the text box display.
            textBox_In.Text = mcRadMax

        ElseIf pVal < mcRadMin Then
            '....Reset the text box display.
            textBox_In.Text = mcRadMin

        End If

    End Sub


    Private Sub Update_Display_RefVariables()
        '====================================

        With mESeal

            txtDControl.Text = gIPE_Unit.WriteInUserL(.DControl)

            If .TemplateNo = "1Gen" Then
                txtHConv_New.Text = gIPE_Unit.WriteInUserL(.HConv)
            End If

        End With

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

    Private Sub txtCrossSecNo_New_TextChanged(ByVal sender As System.Object, _
                                              ByVal e As System.EventArgs) _
                                          Handles txtCrossSecNo_New.TextChanged
        '========================================================================

        If IPE_clsESeal.CrossSecNewList.Count > 0 And _
               Not IPE_clsESeal.CrossSecNewList.Contains(txtCrossSecNo_New.Text.Trim()) And _
                    Not IPE_clsESeal.CrossSecList.Contains(txtCrossSecNo_New.Text.Trim()) Then

            lblCrossSecNew.ForeColor = Color.Green
            txtCrossSecNo_New.ForeColor = Color.Green
        Else

            lblCrossSecNew.ForeColor = Color.Black
            txtCrossSecNo_New.ForeColor = Color.Black

        End If

    End Sub

    Private Sub TxtBox_KeyPress(ByVal sender As System.Object, _
                                     ByVal e As KeyPressEventArgs) _
                                     Handles txtRadE1_New.KeyPress,
                                     txtRadE2_New.KeyPress,
                                     txtRadE3_New.KeyPress,
                                     txtRadM2_New.KeyPress,
                                     txtRadM3_New.KeyPress,
                                     txtRadM4_New.KeyPress,
                                     txtRadM5_New.KeyPress,
                                     txtThetaE1_New.KeyPress,
                                     txtThetaE2_New.KeyPress,
                                     txtThetaE3_New.KeyPress,
                                     txtThetaM1_New.KeyPress,
                                     txtThetaM2_New.KeyPress,
                                     txtThetaM4_New.KeyPress,
                                     txtHConv_New.KeyPress
        '============================================================
        Dim pCulture = gIPE_Project.CultureName

        Select Case pCulture
            Case "USA", "UK"
                If e.KeyChar = "," Then e.KeyChar = "."
            Case "Germany", "France"
                If e.KeyChar = "." Then e.KeyChar = ","
        End Select

    End Sub



#End Region


#Region "UP-DOWN BUTTON RELATED ROUTINES:"


    Private Sub SetProperties_UpDownButton_All()
        '========================================
        '....This routine is called by:
        '       1. New (Constructor).

        'Assign Min., Max. & Increment Values:  
        '-------------------------------------
        Dim pMinVal() As Single = {Nothing, 1, 0, 0, 0, 0}
        Dim pMaxVal() As Single = {Nothing, 10, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.UBArrayTStd, 5, 5, 120}
        Dim pIncVal(5) As Single

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.UnitSystem = "English" Then

            pIncVal(1) = 1
            pIncVal(2) = 1
            pIncVal(3) = 0.001
            'pIncVal(4) = 0.05
            pIncVal(4) = 0.005
            pIncVal(5) = 0.2

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.UnitSystem = "Metric" Then

            pIncVal(1) = 1.0
            pIncVal(2) = 1.0
            pIncVal(3) = 0.001
            pIncVal(4) = 0.05
            pIncVal(5) = 0.2
        End If

        'Set Properties to all UpDown Buttons:  
        '---------------------------------------
        Dim pUpd() As NumericUpDown = {Nothing, updNConv, updIndexTArray, updHConv, _
                                       updRadE1, updRadE2, updRadE3, updRadM2, updRadM3, updRadM4, updRadM5, _
                                       updThetaE1, updThetaE2, updThetaE3, updThetaM1, updThetaM2, updThetaM4}

        Dim i As Int16, iNext As Int16
        '....Set NConv, T, HConv
        For i = 1 To 3
            With pUpd(i)
                .Minimum = pMinVal(i)
                .Maximum = pMaxVal(i)
                .Increment = pIncVal(i)
                .Refresh()
            End With
        Next
        iNext = i

        '....Radii:
        For i = iNext To 10
            With pUpd(i)
                .Minimum = pMinVal(4)
                .Maximum = pMaxVal(4)
                .Increment = pIncVal(4)
                .Refresh()
            End With
        Next
        iNext = i

        '....Thetas:
        For i = iNext To 16
            With pUpd(i)
                .Minimum = pMinVal(5)
                .Maximum = pMaxVal(5)
                .Increment = pIncVal(5)
                .Refresh()
            End With
        Next

    End Sub


    Private Sub AssignValue_UpDownButton(ByRef upd_In As NumericUpDown, _
                                                 ByVal value_In As Single)
        '==================================================================
        '   This routine is called by:
        '       1. txt_New_TextChanged.

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

            Case "updNConv"

            Case "updIndexTArray"

            Case "updHConv"
                mblnUpdHConv_Entered = False

            Case "updRadE1"
                mblnUpdRadE1_Entered = False

            Case "updRadE2"
                mblnUpdRadE2_Entered = False

            Case "updRadE3"
                mblnUpdRadE3_Entered = False

            Case "updRadM2"
                mblnUpdRadM2_Entered = False

            Case "updRadM3"
                mblnUpdRadM3_Entered = False

            Case "updRadM4"
                mblnUpdRadM4_Entered = False

            Case "updRadM5"
                mblnUpdRadM5_Entered = False

            Case "updThetaE1"
                mblnUpdThetaE1_Entered = False

            Case "updThetaE2"
                mblnUpdThetaE2_Entered = False

            Case "updThetaE3"
                mblnUpdThetaE3_Entered = False

            Case "updThetaM1"
                mblnUpdThetaM1_Entered = False

            Case "updThetaM2"
                mblnUpdThetaM2_Entered = False

            Case "updThetaM4"
                mblnUpdThetaM4_Entered = False

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


    Private Sub upd_Enter(ByVal sender As Object, ByVal e As System.EventArgs) _
                          Handles updRadE1.Enter, updRadE2.Enter, updRadE3.Enter, _
                          updRadM2.Enter, updRadM3.Enter, updRadM4.Enter, updRadM5.Enter, _
                          updThetaE1.Enter, updThetaE2.Enter, updThetaE3.Enter, updThetaM1.Enter, _
                          updThetaM2.Enter, updThetaM4.Enter, updHConv.Enter
        '================================================================================
        Dim pUpDown As NumericUpDown = CType(sender, NumericUpDown)

        Select Case pUpDown.Name
            Case "updHConv" : mblnUpdHConv_Entered = True

            Case "updRadE1" : mblnUpdRadE1_Entered = True
            Case "updRadE2" : mblnUpdRadE2_Entered = True
            Case "updRadE3" : mblnUpdRadE3_Entered = True

            Case "updRadM2" : mblnUpdRadM2_Entered = True
            Case "updRadM3" : mblnUpdRadM3_Entered = True
            Case "updRadM4" : mblnUpdRadM4_Entered = True
            Case "updRadM5" : mblnUpdRadM5_Entered = True

            Case "updThetaE1" : mblnUpdThetaE1_Entered = True
            Case "updThetaE2" : mblnUpdThetaE2_Entered = True
            Case "updThetaE3" : mblnUpdThetaE3_Entered = True

            Case "updThetaM1" : mblnUpdThetaM1_Entered = True
            Case "updThetaM2" : mblnUpdThetaM2_Entered = True
            Case "updThetaM4" : mblnUpdThetaM4_Entered = True

        End Select

    End Sub


    Private Sub upd_ValueChanged(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles updNConv.ValueChanged, _
                                 updIndexTArray.ValueChanged, _
                                 updHConv.ValueChanged, _
                                 updRadE1.ValueChanged, _
                                 updRadE2.ValueChanged, _
                                 updRadE3.ValueChanged, _
                                 updRadM2.ValueChanged, _
                                 updRadM3.ValueChanged, _
                                 updRadM4.ValueChanged, _
                                 updRadM5.ValueChanged, _
                                 updThetaE1.ValueChanged, _
                                 updThetaE2.ValueChanged, _
                                 updThetaE3.ValueChanged, _
                                 updThetaM1.ValueChanged, _
                                 updThetaM2.ValueChanged, _
                                 updThetaM4.ValueChanged
        '==============================================================
        If mESeal Is Nothing = True Then Exit Sub

        Dim pUpDown As NumericUpDown = CType(sender, NumericUpDown)
        Dim iSel As Int16 = updIndexTArray.Value

        With mESeal

            Select Case pUpDown.Name

                Case "updNConv"
                    txtNConv_New.Text = updNConv.Value

                Case "updIndexTArray"
                    txtT_New.Text = gIPE_Unit.WriteInUserL(.ArrayTStd(iSel), "LFormat")

                Case "updHConv"
                    If .TemplateNo = "1GenS" Then
                        If mblnUpdHConv_Entered Then _
                        'txtHConv_New.Text = gIPE_Unit.WriteInUserL(updHConv.Value)
                            txtHConv_New.Text = updHConv.Value
                        End If
                    End If


                Case "updRadE1"
                    If mblnUpdRadE1_Entered Then _
                    'txtRadE1_New.Text = gIPE_Unit.WriteInUserL(updRadE1.Value)
                        txtRadE1_New.Text = updRadE1.Value
                    End If

                Case "updRadE2"
                    If mblnUpdRadE2_Entered Then _
                    'txtRadE2_New.Text = gIPE_Unit.WriteInUserL(updRadE2.Value)
                        txtRadE2_New.Text = updRadE2.Value
                    End If

                Case "updRadE3"
                    If mblnUpdRadE3_Entered Then _
                    'txtRadE3_New.Text = gIPE_Unit.WriteInUserL(updRadE3.Value)
                        txtRadE3_New.Text = updRadE3.Value
                    End If

                Case "updRadM2"
                    If mblnUpdRadM2_Entered Then _
                    'txtRadM2_New.Text = gIPE_Unit.WriteInUserL(updRadM2.Value)
                        txtRadM2_New.Text = updRadM2.Value
                    End If

                Case "updRadM3"
                    If mblnUpdRadM3_Entered Then _
                    'txtRadM3_New.Text = gIPE_Unit.WriteInUserL(updRadM3.Value)
                        txtRadM3_New.Text = updRadM3.Value
                    End If

                Case "updRadM4"
                    If mblnUpdRadM4_Entered Then _
                    'txtRadM4_New.Text = gIPE_Unit.WriteInUserL(updRadM4.Value)
                        txtRadM4_New.Text = updRadM4.Value
                    End If

                Case "updRadM5"
                    If mblnUpdRadM5_Entered Then _
                    'txtRadM5_New.Text = gIPE_Unit.WriteInUserL(updRadM5.Value)
                        txtRadM5_New.Text = updRadM5.Value
                    End If

                Case "updThetaE1"
                    If mblnUpdThetaE1_Entered Then _
                    txtThetaE1_New.Text = gIPE_Unit.WriteAngle(updThetaE1.Value)

                Case "updThetaE2"
                    If mblnUpdThetaE2_Entered Then _
                    txtThetaE2_New.Text = gIPE_Unit.WriteAngle(updThetaE2.Value)

                Case "updThetaE3"
                    If mblnUpdThetaE3_Entered Then _
                    txtThetaE3_New.Text = gIPE_Unit.WriteAngle(updThetaE3.Value)

                Case "updThetaM1"
                    If mblnUpdThetaM1_Entered Then _
                    txtThetaM1_New.Text = gIPE_Unit.WriteAngle(updThetaM1.Value)

                Case "updThetaM2"
                    If mblnUpdThetaM2_Entered Then _
                    txtThetaM2_New.Text = gIPE_Unit.WriteAngle(updThetaM2.Value)

                Case "updThetaM4"
                    If mblnUpdThetaM4_Entered Then _
                    txtThetaM4_New.Text = gIPE_Unit.WriteAngle(updThetaM4.Value)

            End Select

        End With

    End Sub


#End Region

#End Region


#Region "MENU: PRINT FORM:"

    Private Sub mnuPrintForm_Click(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles mnuPrintForm.Click
        '===========================================================
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


#Region "COMMAND BUTTON RELATED ROUTINES:"


    Private Sub cmdDXF_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
                                Handles cmdDXF.Click
        '=============================================================================

        'Resolves SealIPE80Beta1_DR ERROR # 7.
        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then
            If Not gIPE_frmNomenclature_DesignCenter.FormClose Then
                gIPE_frmNomenclature_DesignCenter.SendToBack()
            End If
        End If

        If SaveData() = True Then
            Me.Cursor = Cursors.WaitCursor

            With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal)

                saveFileDialog1.FilterIndex = 1
                saveFileDialog1.Filter = "Configuration files (*.DXF)|*.DXF"
                saveFileDialog1.Title = "Save"
                saveFileDialog1.FileName = ExtractPreData(gIPE_File.In_Title, ".") & "_" & _
                                           .MCrossSecNo & ".DXF"

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                    Dim pFileName As String
                    pFileName = saveFileDialog1.FileName
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


    Private Sub cmdViewNomenclature_Click(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles cmdViewNomenclature.Click
        '===================================================================
        gIPE_frmNomenclature_DesignCenter = New IPE_frmNomenclature_DesignCenter()
        gIPE_frmNomenclature_DesignCenter.Show()

        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then _
            cmdViewNomenclature.Enabled = gIPE_frmNomenclature_DesignCenter.FormClose
    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==========================================================

        Dim pCmdBtn As Button = CType(sender, Button)

        If pCmdBtn.Name = "cmdOK" Then
            If SaveData() = False Then Exit Sub
        End If

        If Not gIPE_frmNomenclature_DesignCenter Is Nothing Then
            If Not gIPE_frmNomenclature_DesignCenter.FormClose Then
                gIPE_frmNomenclature_DesignCenter.Close()
            End If
        End If

        Me.Close()

    End Sub


    Private Function SaveData() As Boolean
        '=================================

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
                Dim pCrossSec = UCase(Trim(txtCrossSecNo_New.Text))

                With mESeal
                    .MCrossSecNo = pCrossSec
                    .CrossSecNoOrg = gIPE_SealOrg.MCrossSecNo

                    If pblnAddRecToNewDB = True Then
                        '....Add the corresponding record to ESeal New DB and also write 
                        '........the text file.

                        .AddRecToESealNewDB(gIPE_File, gIPE_Project, gIPE_User, gIPE_Unit, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity)

                    End If

                End With

                If IPE_clsESeal.CrossSecNewList.Count > 0 And _
                     Not IPE_clsESeal.CrossSecNewList.Contains(mESeal.MCrossSecNo) Then
                    IPE_clsESeal.CrossSecNewList.Add(mESeal.MCrossSecNo)
                End If

                With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal)
                    .MCrossSecNo = pCrossSec

                    If .TemplateNo = "1GenS" Then
                        .HConv = gIPE_Unit.L_UserToCon(txtHConv_New.Text)
                    End If

                End With

                Return True

            End If

        End If

    End Function


    Function Check_LocalSealObj() As Boolean
        '====================================       
        '....This routine compares mESeal with gIPE_SealOrg and gSeal
        '
        Dim pDataChanged_SealOrg As Integer
        Dim pDataChanged_Seal As Integer

        '....Holds the datachange number between mESeal & gIPE_SealOrg
        pDataChanged_SealOrg = CType(gIPE_SealOrg, IPE_clsESeal).Compare(mESeal)

        '....Holds the datachange number between mESeal & gSeal
        pDataChanged_Seal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Compare(mESeal)

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
            CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).MCrossSecNo = gIPE_SealOrg.MCrossSecNo
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
        'Dim pDataChanged As Integer      

        If pCrossSecNo_New = "" Then
            '-----------------------
            iCase = 1

        ElseIf pCrossSecNo_New = CType(gIPE_SealOrg, IPE_clsESeal).MCrossSecNo Then
            '-------------------------------------------------------------
            iCase = 2

        ElseIf IPE_clsESeal.CrossSecList.Contains(pCrossSecNo_New) Then
            '-------------------------------------------------------
            iCase = 2

        ElseIf pCrossSecNo_New = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).MCrossSecNo Then
            '----------------------------------------------------------
            iCase = 3


        ElseIf IPE_clsESeal.CrossSecNewList.Contains(pCrossSecNo_New) Then
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
                pstrPrompt = "The NEW CrossSection No. exists in the new database. " & vbCrLf & _
                             "Do you want to overwrite the existing data? "
                pintAnswer = MessageBox.Show(pstrPrompt, pstrTitle, _
                                             MessageBoxButtons.YesNo, _
                                             MessageBoxIcon.Warning)

                If pintAnswer = Windows.Forms.DialogResult.Yes Then
                    '....Delete the corresponding record from the ESeal New DB and 
                    '........the text file.
                    IPE_clsESeal.DeleteNewESealRec(pCrossSecNo_New, gIPE_File)
                    blnSave_Out = True
                    blnAddRecToNewDB_Out = True

                ElseIf pintAnswer = Windows.Forms.DialogResult.No Then
                    blnSave_Out = False
                    txtCrossSecNo_New.Focus()
                    Exit Sub
                End If

        End Select

    End Sub


#End Region


#Region "GRAPHICS ROUTINES:"


    Private Sub DoGraphics()
        '===================

        'This routine draws the 'Original' & 'New' cross-section geometries.

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

        ''....Draw "Original" Seal Geometry.
        'CType(gSeal, IPE_clsESeal).Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
        '                            "STD", "SCALE_BY_STD", 1.25, _
        '                            xEnvpTopL, yEnvpTopL, xEnvpBotR, yEnvpBotR)


        '....Draw "New" Seal Geometry.        
        Try

            mESeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, _
                         pDashStyle, "STD", "SCALE_BY_STD", 1.0, EnvpTopL, EnvpBotR)


            'Caption Labels:        
            '---------------

            '....ORIGINAL Values:
            '
            Dim pHFree_Org As Single
            Dim pWid_Org As Single

            With gIPE_SealOrg
                pHFree_Org = .HfreeStd
                pWid_Org = .WidStd
            End With

            With gIPE_Unit
                lblHFreeOriginal.Text = "Original   =   " & .WriteInUserL(pHFree_Org)
                lblWidOriginal.Text = "Original   =   " & .WriteInUserL(pWid_Org)
            End With


            '....NEW Values:
            '
            With mESeal
                Dim pHFree_New As Single = .HfreeStd
                Dim pDHFree_PCent As Single
                'pDHFree_PCent = ((pHFree_New - pHFree_Org) * 100 / pHFree_Org) / 1000
                pDHFree_PCent = ((pHFree_New - pHFree_Org) * 100 / pHFree_Org) 'AM 06AUG09

                If Abs(pDHFree_PCent) <= 0.0# Then
                    lblHFreeNew.Visible = False

                ElseIf Abs(pDHFree_PCent) > 0.0# Then
                    lblHFreeNew.Visible = True

                    'Dim pstr_New As String = gIPE_Unit.WriteInUserL(pHFree_New / 1000)
                    Dim pstr_New As String = gIPE_Unit.WriteInUserL(pHFree_New)     'AM 06AUG09
                    Dim pstrD_PCent As String = Format(pDHFree_PCent, "##0.0")
                    Dim pstrTxt As String
                    pstrTxt = pstr_New & "  ( " & pstrD_PCent & " %)"

                    lblHFreeNew.Text = "New         =   " & pstrTxt

                End If

                'lblWidNew.Text = "New         =   " & gIPE_Unit.WriteInUserL((.Wid / 1000))
                lblWidNew.Text = "New         =   " & gIPE_Unit.WriteInUserL((.Wid))    'AM 06AUG09

            End With


        Catch pEXP As Exception
            MsgBox(pEXP.StackTrace)
        End Try

    End Sub


#End Region


End Class
