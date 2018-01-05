
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmCavity                              '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18APR16                                '
'                                                                              '
'===============================================================================

'PB 04APR16

Imports System.Math
Imports clsLibrary11
Imports System.Linq
Imports SealIPELib = SealIPELib101

Public Class IPE_frmCavity
    Inherits System.Windows.Forms.Form


#Region "MEMBER VARIABLES:"

    Private Const mcCompressPcentValueMinReqd As Single = 5.0   '....in %.

    ''Private mProjectEntity As New ProjectDBEntities()
    ''Private mSealMCSDB As New SealMCSDBEntities()

    '....Check box array:
    Private mchkDia As New ArrayList()

    '....Text box arrays:
    Private mtxtDia As New ArrayList()
    Private mtxtDepthTol As New ArrayList()
    Private mtxtOperMoveTol As New ArrayList()
    Public WithEvents txtZClear As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label

    Private mCavity As IPE_clsCavity
    Friend WithEvents lblMax As System.Windows.Forms.Label                    '....Local Cavity Object. 
    Private mSeal As IPE_clsSeal                        '....Local Seal Object.

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
    Friend WithEvents lblSeperator As System.Windows.Forms.Label
    Public WithEvents lblUnitUserL As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Public WithEvents txtDepth As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents txtMaxID As System.Windows.Forms.TextBox
    Public WithEvents txtMinOD As System.Windows.Forms.TextBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtDepthTolPlus As System.Windows.Forms.TextBox
    Public WithEvents txtDepthTolMinus As System.Windows.Forms.TextBox
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents txtCornerRad As System.Windows.Forms.TextBox
    Public WithEvents txtWidMin As System.Windows.Forms.TextBox
    Friend WithEvents chkWidMin As System.Windows.Forms.CheckBox
    Friend WithEvents chkMaxID As System.Windows.Forms.CheckBox
    Friend WithEvents chkMinOD As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmCavity))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblMax = New System.Windows.Forms.Label()
        Me.txtZClear = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblSeperator = New System.Windows.Forms.Label()
        Me.chkWidMin = New System.Windows.Forms.CheckBox()
        Me.chkMaxID = New System.Windows.Forms.CheckBox()
        Me.chkMinOD = New System.Windows.Forms.CheckBox()
        Me.txtWidMin = New System.Windows.Forms.TextBox()
        Me.txtCornerRad = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDepth = New System.Windows.Forms.TextBox()
        Me.txtMaxID = New System.Windows.Forms.TextBox()
        Me.txtMinOD = New System.Windows.Forms.TextBox()
        Me.txtDepthTolPlus = New System.Windows.Forms.TextBox()
        Me.txtDepthTolMinus = New System.Windows.Forms.TextBox()
        Me.lblUnitUserL = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(513, 207)
        Me.lblBorder.TabIndex = 0
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblMax)
        Me.Panel1.Controls.Add(Me.txtZClear)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblSeperator)
        Me.Panel1.Controls.Add(Me.chkWidMin)
        Me.Panel1.Controls.Add(Me.chkMaxID)
        Me.Panel1.Controls.Add(Me.chkMinOD)
        Me.Panel1.Controls.Add(Me.txtWidMin)
        Me.Panel1.Controls.Add(Me.txtCornerRad)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtDepth)
        Me.Panel1.Controls.Add(Me.txtMaxID)
        Me.Panel1.Controls.Add(Me.txtMinOD)
        Me.Panel1.Controls.Add(Me.txtDepthTolPlus)
        Me.Panel1.Controls.Add(Me.txtDepthTolMinus)
        Me.Panel1.Controls.Add(Me.lblUnitUserL)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(511, 205)
        Me.Panel1.TabIndex = 1
        '
        'lblMax
        '
        Me.lblMax.AutoSize = True
        Me.lblMax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMax.Location = New System.Drawing.Point(148, 55)
        Me.lblMax.Name = "lblMax"
        Me.lblMax.Size = New System.Drawing.Size(44, 13)
        Me.lblMax.TabIndex = 184
        Me.lblMax.Text = "(Max.)"
        '
        'txtZClear
        '
        Me.txtZClear.AcceptsReturn = True
        Me.txtZClear.BackColor = System.Drawing.Color.White
        Me.txtZClear.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtZClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZClear.ForeColor = System.Drawing.Color.Black
        Me.txtZClear.Location = New System.Drawing.Point(413, 125)
        Me.txtZClear.MaxLength = 0
        Me.txtZClear.Name = "txtZClear"
        Me.txtZClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtZClear.Size = New System.Drawing.Size(54, 21)
        Me.txtZClear.TabIndex = 183
        Me.txtZClear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(352, 129)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 182
        Me.Label1.Text = " ZClear "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSeperator
        '
        Me.lblSeperator.BackColor = System.Drawing.Color.Silver
        Me.lblSeperator.Location = New System.Drawing.Point(0, 95)
        Me.lblSeperator.Name = "lblSeperator"
        Me.lblSeperator.Size = New System.Drawing.Size(510, 2)
        Me.lblSeperator.TabIndex = 61
        '
        'chkWidMin
        '
        Me.chkWidMin.ForeColor = System.Drawing.Color.Black
        Me.chkWidMin.Location = New System.Drawing.Point(327, 20)
        Me.chkWidMin.Name = "chkWidMin"
        Me.chkWidMin.Size = New System.Drawing.Size(15, 15)
        Me.chkWidMin.TabIndex = 5
        Me.chkWidMin.TabStop = False
        Me.chkWidMin.UseVisualStyleBackColor = True
        '
        'chkMaxID
        '
        Me.chkMaxID.ForeColor = System.Drawing.Color.Black
        Me.chkMaxID.Location = New System.Drawing.Point(171, 20)
        Me.chkMaxID.Name = "chkMaxID"
        Me.chkMaxID.Size = New System.Drawing.Size(15, 15)
        Me.chkMaxID.TabIndex = 3
        Me.chkMaxID.TabStop = False
        Me.chkMaxID.UseVisualStyleBackColor = True
        '
        'chkMinOD
        '
        Me.chkMinOD.ForeColor = System.Drawing.Color.Black
        Me.chkMinOD.Location = New System.Drawing.Point(16, 20)
        Me.chkMinOD.Name = "chkMinOD"
        Me.chkMinOD.Size = New System.Drawing.Size(15, 15)
        Me.chkMinOD.TabIndex = 1
        Me.chkMinOD.TabStop = False
        Me.chkMinOD.UseVisualStyleBackColor = True
        '
        'txtWidMin
        '
        Me.txtWidMin.AcceptsReturn = True
        Me.txtWidMin.BackColor = System.Drawing.Color.White
        Me.txtWidMin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWidMin.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWidMin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWidMin.Location = New System.Drawing.Point(413, 17)
        Me.txtWidMin.MaxLength = 0
        Me.txtWidMin.Name = "txtWidMin"
        Me.txtWidMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWidMin.Size = New System.Drawing.Size(54, 21)
        Me.txtWidMin.TabIndex = 6
        Me.txtWidMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtCornerRad
        '
        Me.txtCornerRad.AcceptsReturn = True
        Me.txtCornerRad.BackColor = System.Drawing.Color.White
        Me.txtCornerRad.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCornerRad.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCornerRad.ForeColor = System.Drawing.Color.Magenta
        Me.txtCornerRad.Location = New System.Drawing.Point(91, 51)
        Me.txtCornerRad.MaxLength = 0
        Me.txtCornerRad.Name = "txtCornerRad"
        Me.txtCornerRad.ReadOnly = True
        Me.txtCornerRad.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCornerRad.Size = New System.Drawing.Size(54, 21)
        Me.txtCornerRad.TabIndex = 7
        Me.txtCornerRad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(1, 55)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(89, 13)
        Me.Label12.TabIndex = 52
        Me.Label12.Text = "Corner Radius"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(341, 169)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 11
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
        Me.cmdCancel.Location = New System.Drawing.Point(424, 169)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 12
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(150, 137)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(24, 16)
        Me.Label8.TabIndex = 47
        Me.Label8.Text = "(—)"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(151, 115)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(22, 16)
        Me.Label7.TabIndex = 46
        Me.Label7.Text = "(+)"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(92, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 45
        Me.Label6.Text = "Nominal"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(44, 129)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(41, 13)
        Me.Label5.TabIndex = 44
        Me.Label5.Text = "Depth"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(342, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(68, 16)
        Me.Label4.TabIndex = 43
        Me.Label4.Text = "Min. Width"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(184, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(56, 17)
        Me.Label3.TabIndex = 42
        Me.Label3.Text = "Max. ID"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(31, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(55, 16)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "Min. OD"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDepth
        '
        Me.txtDepth.AcceptsReturn = True
        Me.txtDepth.BackColor = System.Drawing.Color.White
        Me.txtDepth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDepth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDepth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDepth.Location = New System.Drawing.Point(91, 125)
        Me.txtDepth.MaxLength = 0
        Me.txtDepth.Name = "txtDepth"
        Me.txtDepth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDepth.Size = New System.Drawing.Size(54, 21)
        Me.txtDepth.TabIndex = 8
        Me.txtDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMaxID
        '
        Me.txtMaxID.AcceptsReturn = True
        Me.txtMaxID.BackColor = System.Drawing.Color.White
        Me.txtMaxID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxID.Location = New System.Drawing.Point(243, 17)
        Me.txtMaxID.MaxLength = 0
        Me.txtMaxID.Name = "txtMaxID"
        Me.txtMaxID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxID.Size = New System.Drawing.Size(54, 21)
        Me.txtMaxID.TabIndex = 4
        Me.txtMaxID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMinOD
        '
        Me.txtMinOD.AcceptsReturn = True
        Me.txtMinOD.BackColor = System.Drawing.Color.White
        Me.txtMinOD.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinOD.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinOD.ForeColor = System.Drawing.Color.Black
        Me.txtMinOD.Location = New System.Drawing.Point(91, 17)
        Me.txtMinOD.MaxLength = 0
        Me.txtMinOD.Name = "txtMinOD"
        Me.txtMinOD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinOD.Size = New System.Drawing.Size(54, 21)
        Me.txtMinOD.TabIndex = 2
        Me.txtMinOD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDepthTolPlus
        '
        Me.txtDepthTolPlus.AcceptsReturn = True
        Me.txtDepthTolPlus.BackColor = System.Drawing.Color.White
        Me.txtDepthTolPlus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDepthTolPlus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDepthTolPlus.ForeColor = System.Drawing.Color.Black
        Me.txtDepthTolPlus.Location = New System.Drawing.Point(174, 115)
        Me.txtDepthTolPlus.MaxLength = 0
        Me.txtDepthTolPlus.Name = "txtDepthTolPlus"
        Me.txtDepthTolPlus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDepthTolPlus.Size = New System.Drawing.Size(54, 21)
        Me.txtDepthTolPlus.TabIndex = 9
        Me.txtDepthTolPlus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDepthTolMinus
        '
        Me.txtDepthTolMinus.AcceptsReturn = True
        Me.txtDepthTolMinus.BackColor = System.Drawing.Color.White
        Me.txtDepthTolMinus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDepthTolMinus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDepthTolMinus.ForeColor = System.Drawing.Color.Black
        Me.txtDepthTolMinus.Location = New System.Drawing.Point(174, 139)
        Me.txtDepthTolMinus.MaxLength = 0
        Me.txtDepthTolMinus.Name = "txtDepthTolMinus"
        Me.txtDepthTolMinus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDepthTolMinus.Size = New System.Drawing.Size(54, 21)
        Me.txtDepthTolMinus.TabIndex = 10
        Me.txtDepthTolMinus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblUnitUserL
        '
        Me.lblUnitUserL.AutoSize = True
        Me.lblUnitUserL.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitUserL.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnitUserL.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitUserL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnitUserL.Location = New System.Drawing.Point(471, 20)
        Me.lblUnitUserL.Name = "lblUnitUserL"
        Me.lblUnitUserL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnitUserL.Size = New System.Drawing.Size(17, 13)
        Me.lblUnitUserL.TabIndex = 20
        Me.lblUnitUserL.Text = "in"
        '
        'IPE_frmCavity
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(519, 213)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmCavity"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cavity Dimensions - Cold Assembly"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public Sub New()
        '===========
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        '....Create list arrays with check boxes and text boxes.       
        With mchkDia
            .Add(chkMaxID)
            .Add(chkMinOD)
            .Add(chkWidMin)
        End With

        With mtxtDia
            .Add(txtMaxID)
            .Add(txtMinOD)
            .Add(txtWidMin)
        End With

        With mtxtDepthTol
            .Add(txtDepthTolMinus)
            .Add(txtDepthTolPlus)
        End With

        txtCornerRad.ReadOnly = False       '....Overridable.      

    End Sub


#Region "FORM EVENT ROUTINES:"

    Private Sub frmCavity_Load(ByVal sender As Object, _
                               ByVal e As System.EventArgs) _
                               Handles MyBase.Load
        '======================================================

        InitializeControls()
        InitializeLocalObject()
        'Me.Owner = gIPE_frmAnalysisSet
        DisplayData()

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            chkMinOD.Enabled = False
            txtMinOD.Enabled = False

            chkMaxID.Enabled = False
            txtMaxID.Enabled = False

            chkWidMin.Enabled = False
            txtWidMin.Enabled = False

            txtCornerRad.Enabled = False

            txtDepth.Enabled = False
            txtDepthTolMinus.Enabled = False
            txtDepthTolPlus.Enabled = False

            txtZClear.Enabled = False

        Else
            chkMinOD.Enabled = True
            txtMinOD.Enabled = True

            chkMaxID.Enabled = True
            txtMaxID.Enabled = True

            chkWidMin.Enabled = True
            txtWidMin.Enabled = True

            txtCornerRad.Enabled = True

            txtDepth.Enabled = True
            txtDepthTolMinus.Enabled = True
            txtDepthTolPlus.Enabled = True

            txtZClear.Enabled = True
        End If

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        lblUnitUserL.Text = gIPE_Unit.UserL
        'lblNote.Text = "Note: Calculated parameter checkbox should be checked."

        Set_ChkBoxes()

    End Sub


    Private Sub InitializeLocalObject()
        '==============================

        '....Instantiate Local Cavity Object. 
        'mCavity = New clsCavity(gIPE_Unit.System)      'AES 15APR16
        mCavity = New IPE_clsCavity(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type, gIPE_Unit.System)         'AES 15APR16
        'Dim a As Double = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.DepthTol(1)

        With mCavity

            Dim i As Int16
            For i = 1 To 2
                .Dia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
                .DepthTol(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.DepthTol(i)
            Next

            .WidMin = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.WidMin
            .Depth = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth
            .CornerRad = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.CornerRad

            '....Any assignments other than above are not necessary here.
            '.......mCavity is needed to calculate the 'Min. Width' only.   

        End With

        '....Instantiate Local Seal Object. 
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            mSeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
            mSeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "C-Seal" Then
            mSeal = New IPE_clsCSeal("C-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
            mSeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal).Clone

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
            mSeal = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
            mSeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).Clone
        End If

    End Sub


    Private Sub DisplayData()
        '===================

        ''....Data is displayed in user unit.
        Dim i As Int16

        With mCavity

            For i = 1 To 2
                mtxtDia(i - 1).Text = gIPE_Unit.WriteInUserL(.Dia(i))
                mtxtDepthTol(i - 1).Text = gIPE_Unit.WriteInUserL(.DepthTol(i))
            Next

            txtWidMin.Text = gIPE_Unit.WriteInUserL(.WidMin)

            txtDepth.Text = gIPE_Unit.WriteInUserL(.Depth)
            txtCornerRad.Text = gIPE_Unit.WriteInUserL(.CornerRad)
        End With


        If (Math.Abs(mSeal.ZClear_Given) > gcEPS) Then
            txtZClear.Text = gIPE_Unit.WriteInUserL(mSeal.ZClear_Given)
            txtZClear.ForeColor = Color.Black

        Else
            txtZClear.Text = gIPE_Unit.WriteInUserL(mSeal.ZClear_Calc(mCavity.HFree_Rep))
            txtZClear.ForeColor = Color.Blue
        End If

    End Sub


#Region "SUB-HELPER ROUTINES:"

    Private Sub Set_ChkBoxes()
        '======================                                         
        With gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity

            Select Case .Param_Calculated

                Case "MinOD"
                    '-------
                    If .Dia(2) > gcEPS Then chkMinOD.Checked = True

                Case "MaxID"
                    '------
                    If .Dia(1) > gcEPS Then chkMaxID.Checked = True

                Case "WidMin"
                    '--------
                    If .WidMin > gcEPS Then chkWidMin.Checked = True

            End Select
        End With

    End Sub

#End Region

#End Region

#End Region

#Region "CONTROL EVENT ROUTINES:"
    '
#Region "CHECKBOX CHECKED EVENT ROUTINES:"

    Private Sub chkBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                      Handles chkMinOD.CheckedChanged, chkMaxID.CheckedChanged, chkWidMin.CheckedChanged
        '=================================================================================================================
        '....CheckBox.checked = true,corresponding param value is calculated and readonly,
        '......then other two checkBoxes will be unchecked, corresponding param values can be writable by user.
        '......Index of mchkDia and mtxtDia array
        '......MaxID = 0, MinOD = 1 and WidMin =2

        Dim i As Integer
        Dim pblnChecked As Boolean = False
        Dim pChkBox As CheckBox = CType(sender, CheckBox)

        Select Case pChkBox.Name

            Case "chkMinOD"
                '----------
                pblnChecked = mchkDia(1).Checked

                If pblnChecked Then
                    mchkDia(1).Enabled = Not pblnChecked

                    With mtxtDia(1)
                        .ReadOnly = pblnChecked
                        .ForeColor = Color.Blue
                        .BackColor = Color.LightGray
                    End With

                    For i = 0 To 2
                        If i <> 1 Then
                            With mchkDia(i)
                                .checked = Not pblnChecked
                                .Enabled = pblnChecked
                            End With

                            With mtxtDia(i)
                                .ReadOnly = Not pblnChecked
                                .ForeColor = Color.Black
                                .BackColor = Color.White
                            End With

                        End If
                    Next

                End If


            Case "chkMaxID"
                '-----------
                pblnChecked = mchkDia(0).Checked

                If pblnChecked Then
                    mchkDia(0).Enabled = Not pblnChecked

                    With mtxtDia(0)
                        .ReadOnly = pblnChecked
                        .ForeColor = Color.Blue
                        .BackColor = Color.LightGray
                    End With

                    For i = 1 To 2
                        With mchkDia(i)
                            .checked = Not pblnChecked
                            .Enabled = pblnChecked
                        End With

                        With mtxtDia(i)
                            .ReadOnly = Not pblnChecked
                            .ForeColor = Color.Black
                            .BackColor = Color.White
                        End With
                    Next

                End If


            Case "chkWidMin"
                '-----------
                pblnChecked = mchkDia(2).Checked

                If pblnChecked Then
                    mchkDia(2).Enabled = Not pblnChecked

                    With mtxtDia(2)
                        .ReadOnly = pblnChecked
                        .ForeColor = Color.Blue
                        .BackColor = Color.LightGray
                    End With

                    For i = 0 To 1
                        With mchkDia(i)
                            .checked = Not pblnChecked
                            .Enabled = pblnChecked
                        End With

                        With mtxtDia(i)
                            .ReadOnly = Not pblnChecked
                            .ForeColor = Color.Black
                            .BackColor = Color.White
                        End With
                    Next

                End If

        End Select

    End Sub

#End Region


#Region "TEXTBOX TEXTCHANGE EVENT ROUTINES:"

    Private Sub txtMinOD_TextChanged(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) _
                                     Handles txtMinOD.TextChanged
        '============================================================

        If chkMinOD.Checked = False Then

            With mCavity

                .Dia(2) = gIPE_Unit.L_UserToCon(txtMinOD.Text)

                If .Dia(2) > gcEPS Then

                    If gIPE_Unit.L_UserToCon(txtMaxID.Text) > gcEPS And chkMaxID.Checked = False Then
                        .Dia(1) = gIPE_Unit.L_UserToCon(txtMaxID.Text)

                        If .WidMin > gcEPS Then
                            '....WidMin Calculated
                            txtWidMin.Text = gIPE_Unit.WriteInUserL(.WidMin)
                            If Not chkWidMin.Checked Then chkWidMin.Checked = True
                        End If

                    ElseIf gIPE_Unit.L_UserToCon(txtWidMin.Text) > gcEPS And chkWidMin.Checked = False Then
                        .WidMin = gIPE_Unit.L_UserToCon(txtWidMin.Text)
                        .CalcDia(2)

                        If .Dia(1) > gcEPS Then
                            '....MaxID Calculated
                            txtMaxID.Text = gIPE_Unit.WriteInUserL(.Dia(1))
                            If Not chkMaxID.Checked Then chkMaxID.Checked = True
                        End If

                    End If
                End If

            End With
        End If

    End Sub


    Private Sub txtMaxID_TextChanged(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) _
                                     Handles txtMaxID.TextChanged
        '================================================================

        If chkMaxID.Checked = False Then

            With mCavity
                .Dia(1) = gIPE_Unit.L_UserToCon(txtMaxID.Text)

                If .Dia(1) > gcEPS Then
                    If gIPE_Unit.L_UserToCon(txtMinOD.Text) > gcEPS And chkMinOD.Checked = False Then
                        .Dia(2) = gIPE_Unit.L_UserToCon(txtMinOD.Text)

                        If .WidMin > gcEPS Then
                            '....WidMin Calculated
                            txtWidMin.Text = gIPE_Unit.WriteInUserL(.WidMin)
                            If Not chkWidMin.Checked Then chkWidMin.Checked = True
                        End If


                    ElseIf gIPE_Unit.L_UserToCon(txtWidMin.Text) > gcEPS And chkWidMin.Checked = False Then
                        .WidMin = gIPE_Unit.L_UserToCon(txtWidMin.Text)
                        .CalcDia(1)

                        If .Dia(2) > gcEPS Then
                            '....MinOD Calculated
                            txtMinOD.Text = gIPE_Unit.WriteInUserL(.Dia(2))
                            If Not chkMinOD.Checked Then chkMinOD.Checked = True
                        End If

                    End If
                End If

            End With

        End If

    End Sub


    Private Sub txtWidMin_TextChanged(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) _
                                     Handles txtWidMin.TextChanged
        '==============================================================

        If chkWidMin.Checked = False Then

            With mCavity
                .WidMin = gIPE_Unit.L_UserToCon(txtWidMin.Text)

                If .WidMin > gcEPS Then

                    If gIPE_Unit.L_UserToCon(txtMinOD.Text) > gcEPS And chkMinOD.Checked = False Then
                        .CalcDia(2)

                        If .Dia(1) > gcEPS Then
                            '....MaxID Calculated
                            txtMaxID.Text = gIPE_Unit.WriteInUserL(.Dia(1))
                            If Not chkMaxID.Checked Then chkMaxID.Checked = True
                        End If


                    ElseIf gIPE_Unit.L_UserToCon(txtMaxID.Text) > gcEPS And chkMaxID.Checked = False Then
                        .CalcDia(1)

                        If .Dia(2) > gcEPS Then
                            '....MinOD Calculated
                            txtMinOD.Text = gIPE_Unit.WriteInUserL(.Dia(2))
                            If Not chkMinOD.Checked Then chkMinOD.Checked = True
                        End If
                    End If
                End If
            End With

        End If

    End Sub


    Private Sub txtCornerRad_TextChanged(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) _
                                         Handles txtCornerRad.TextChanged
        '=================================================================  
        Dim pColor As Color

        With mCavity
            .CornerRad = gIPE_Unit.L_UserToCon(txtCornerRad.Text)

            If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal Is Nothing = False Then
                pColor = IIf(Abs(.CornerRad - gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.CavityCornerRad) < gcEPS, _
                                                            Color.Magenta, Color.Black)
            Else
                pColor = IIf(Abs(.CornerRad - gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.CornerRad) < gcEPS, _
                                              Color.Magenta, Color.Black)
            End If

            txtCornerRad.ForeColor = pColor
        End With

    End Sub


    Private Sub txtDepth_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDepth.TextChanged
        '=========================================================================================================
        With mCavity
            .Depth = gIPE_Unit.L_UserToCon(txtDepth.Text)
        End With
    End Sub


    Private Sub txtDepthTol_TextChanged(sender As System.Object, e As System.EventArgs) _
                                        Handles txtDepthTolPlus.TextChanged, txtDepthTolMinus.TextChanged
        '==================================================================================================
        Dim pTxtBox As TextBox = CType(sender, TextBox)
        Dim pColor As Color

        Select Case pTxtBox.Name

            Case "txtDepthTolPlus"
                '------------------------
                With mCavity
                    .DepthTol(1) = gIPE_Unit.L_UserToCon(txtDepthTolPlus.Text)
                    If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity Is Nothing = False Then
                        pColor = IIf(Abs(.DepthTol(1) - gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.DepthTol(1)) < gcEPS, _
                                                          Color.Magenta, Color.Black)
                    Else
                        pColor = IIf(Abs(.DepthTol(1) - gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.DepthTol(1)) < gcEPS, _
                                                         Color.Magenta, Color.Black)
                    End If
                    txtDepthTolPlus.ForeColor = pColor
                End With


            Case "txtDepthTolMinus"
                '------------------------
                With mCavity
                    .DepthTol(2) = gIPE_Unit.L_UserToCon(txtDepthTolMinus.Text)
                    If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity Is Nothing = False Then
                        pColor = IIf(Abs(.DepthTol(2) - gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.DepthTol(2)) < gcEPS, _
                                                     Color.Magenta, Color.Black)
                    Else
                        pColor = IIf(Abs(.DepthTol(2) - gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.DepthTol(2)) < gcEPS, _
                                                    Color.Magenta, Color.Black)
                    End If
                    txtDepthTolMinus.ForeColor = pColor
                End With

        End Select

    End Sub


    Private Sub txtZClear_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtZClear.TextChanged
        '============================================================================================================

        If (Math.Abs(Val(txtZClear.Text) - mSeal.ZClear_Calc(mCavity.HFree_Rep)) > gcEPS) Then
            txtZClear.ForeColor = Color.Black

        End If

    End Sub

#End Region

#End Region

#Region "COMMAND BUTTON EVENT ROUTINE:"

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '==========================================================
        Dim pCmdBtn As Button = CType(sender, Button)

        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
        End If

        Me.Close()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '================

        '....Save the following previous cavity envelope data for reference, before 
        '........saving the current data on the form.
        Dim pWidMinPrev As Single = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.WidMin
        Dim pDepthPrev As Single = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth

        'Save the current data on the form:
        '----------------------------------
        Dim i As Int16

        With gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity

            For i = 1 To 2
                .Dia(i) = mCavity.Dia(i)
                .DepthTol(i) = mCavity.DepthTol(i)
            Next

            .WidMin = mCavity.WidMin
            .Depth = mCavity.Depth
            .CornerRad = mCavity.CornerRad

            For i = 0 To 2
                If mchkDia(i).checked Then

                    Select Case i
                        Case 0
                            .Param_Calculated = "MaxID"
                        Case 1
                            .Param_Calculated = "MinOD"
                        Case 2
                            .Param_Calculated = "WidMin"
                    End Select

                End If
            Next

        End With


        If (Math.Abs(Val(txtZClear.Text) - mSeal.ZClear_Calc(mCavity.HFree_Rep)) > gcEPS) Then
            mSeal.ZClear_Given = gIPE_Unit.L_UserToCon(txtZClear.Text)
        Else
            mSeal.ZClear_Given = 0.0
        End If

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.ZClear_Given = mSeal.ZClear_Given


        'SECONDARY ASSIGNMENTS:
        '----------------------
        '....Assign cavity diameters to the gSeal & gIPE_SealCandidates object members
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal Is Nothing = False And gIPE_SealCandidates Is Nothing = False Then
            For i = 1 To 2
                gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
                gIPE_SealCandidates.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
            Next
        End If


        'Check if the cavity enevelope has been changed. If so, the candidate seal 
        '....designs in the appropriate working database table should be updated.
        '-------------------------------------------------------------------------
        If Abs(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.WidMin - pWidMinPrev) > gcEPS Or _
           Abs(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth - pDepthPrev) > gcEPS Then

            '....Reset the boolean variable. The candidate designs need to be updated.
            gUpdate_Candidate_CrossSecs = True
        End If

    End Sub

#End Region

#End Region

End Class
