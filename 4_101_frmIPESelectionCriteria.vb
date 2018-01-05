'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                       Form MODULE  :  frmSelectionCriteria                   '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05APR16                                '
'                                                                              '
'===============================================================================
'
Imports System
Imports System.Drawing.Printing
Imports clsLibrary11
Imports SealIPELib = SealIPELib101

Public Class IPE_frmSelectionCriteria
    Inherits System.Windows.Forms.Form

    Private mZClearEntered As Boolean

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdPrintForm As System.Windows.Forms.Button
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents lblHFreeMinAllowed As System.Windows.Forms.Label
    Public WithEvents lblCompressMinReqd As System.Windows.Forms.Label
    Public WithEvents lblCavityDepth As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblSplitter As System.Windows.Forms.Label
    Public WithEvents txtFacBuckling As System.Windows.Forms.TextBox
    Public WithEvents txtH11Tol As System.Windows.Forms.TextBox
    Public WithEvents lblWidMaxAllowed As System.Windows.Forms.Label
    Public WithEvents lblCavityWid As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents lblUnitUserL As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PrintDocu As System.Drawing.Printing.PrintDocument
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Public WithEvents lblZClear As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmSelectionCriteria))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblZClear = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdPrintForm = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblHFreeMinAllowed = New System.Windows.Forms.Label()
        Me.lblCompressMinReqd = New System.Windows.Forms.Label()
        Me.lblCavityDepth = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblSplitter = New System.Windows.Forms.Label()
        Me.txtFacBuckling = New System.Windows.Forms.TextBox()
        Me.txtH11Tol = New System.Windows.Forms.TextBox()
        Me.lblWidMaxAllowed = New System.Windows.Forms.Label()
        Me.lblCavityWid = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblUnitUserL = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PrintDocu = New System.Drawing.Printing.PrintDocument()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblZClear)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdPrintForm)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.lblHFreeMinAllowed)
        Me.Panel1.Controls.Add(Me.lblCompressMinReqd)
        Me.Panel1.Controls.Add(Me.lblCavityDepth)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.lblSplitter)
        Me.Panel1.Controls.Add(Me.txtFacBuckling)
        Me.Panel1.Controls.Add(Me.txtH11Tol)
        Me.Panel1.Controls.Add(Me.lblWidMaxAllowed)
        Me.Panel1.Controls.Add(Me.lblCavityWid)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.lblUnitUserL)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(404, 356)
        Me.Panel1.TabIndex = 1
        '
        'lblZClear
        '
        Me.lblZClear.BackColor = System.Drawing.Color.Transparent
        Me.lblZClear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblZClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblZClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZClear.ForeColor = System.Drawing.Color.Black
        Me.lblZClear.Location = New System.Drawing.Point(120, 81)
        Me.lblZClear.Name = "lblZClear"
        Me.lblZClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblZClear.Size = New System.Drawing.Size(54, 21)
        Me.lblZClear.TabIndex = 56
        Me.lblZClear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(44, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(70, 16)
        Me.Label4.TabIndex = 54
        Me.Label4.Text = " ZClear "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.cmdOK.Location = New System.Drawing.Point(301, 317)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(82, 28)
        Me.cmdOK.TabIndex = 53
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = " &OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdPrintForm
        '
        Me.cmdPrintForm.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdPrintForm.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrintForm.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintForm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrintForm.Image = CType(resources.GetObject("cmdPrintForm.Image"), System.Drawing.Image)
        Me.cmdPrintForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPrintForm.Location = New System.Drawing.Point(27, 317)
        Me.cmdPrintForm.Name = "cmdPrintForm"
        Me.cmdPrintForm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrintForm.Size = New System.Drawing.Size(118, 28)
        Me.cmdPrintForm.TabIndex = 45
        Me.cmdPrintForm.Text = "    &Print Form"
        Me.cmdPrintForm.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(366, 235)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(17, 17)
        Me.Label11.TabIndex = 44
        Me.Label11.Text = "%"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblHFreeMinAllowed
        '
        Me.lblHFreeMinAllowed.BackColor = System.Drawing.Color.Transparent
        Me.lblHFreeMinAllowed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblHFreeMinAllowed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHFreeMinAllowed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHFreeMinAllowed.ForeColor = System.Drawing.Color.Blue
        Me.lblHFreeMinAllowed.Location = New System.Drawing.Point(310, 276)
        Me.lblHFreeMinAllowed.Name = "lblHFreeMinAllowed"
        Me.lblHFreeMinAllowed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHFreeMinAllowed.Size = New System.Drawing.Size(53, 21)
        Me.lblHFreeMinAllowed.TabIndex = 43
        Me.lblHFreeMinAllowed.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCompressMinReqd
        '
        Me.lblCompressMinReqd.BackColor = System.Drawing.Color.Transparent
        Me.lblCompressMinReqd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCompressMinReqd.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompressMinReqd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompressMinReqd.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblCompressMinReqd.Location = New System.Drawing.Point(310, 235)
        Me.lblCompressMinReqd.Name = "lblCompressMinReqd"
        Me.lblCompressMinReqd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCompressMinReqd.Size = New System.Drawing.Size(53, 21)
        Me.lblCompressMinReqd.TabIndex = 42
        Me.lblCompressMinReqd.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCavityDepth
        '
        Me.lblCavityDepth.BackColor = System.Drawing.Color.Transparent
        Me.lblCavityDepth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCavityDepth.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCavityDepth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCavityDepth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCavityDepth.Location = New System.Drawing.Point(120, 235)
        Me.lblCavityDepth.Name = "lblCavityDepth"
        Me.lblCavityDepth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCavityDepth.Size = New System.Drawing.Size(54, 21)
        Me.lblCavityDepth.TabIndex = 41
        Me.lblCavityDepth.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(60, 276)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(220, 16)
        Me.Label12.TabIndex = 39
        Me.Label12.Text = "Minimum Allowable Seal  Height"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(230, 236)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(75, 16)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "Min. Comp"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(35, 236)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(80, 16)
        Me.Label9.TabIndex = 37
        Me.Label9.Text = "Cavity Depth "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(24, 203)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(234, 17)
        Me.Label8.TabIndex = 36
        Me.Label8.Text = "Free Height Selection Criterion :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSplitter
        '
        Me.lblSplitter.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSplitter.ForeColor = System.Drawing.SystemColors.InactiveBorder
        Me.lblSplitter.Location = New System.Drawing.Point(0, 183)
        Me.lblSplitter.Name = "lblSplitter"
        Me.lblSplitter.Size = New System.Drawing.Size(410, 2)
        Me.lblSplitter.TabIndex = 35
        '
        'txtFacBuckling
        '
        Me.txtFacBuckling.AcceptsReturn = True
        Me.txtFacBuckling.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtFacBuckling.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFacBuckling.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFacBuckling.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFacBuckling.Location = New System.Drawing.Point(310, 81)
        Me.txtFacBuckling.MaxLength = 0
        Me.txtFacBuckling.Name = "txtFacBuckling"
        Me.txtFacBuckling.ReadOnly = True
        Me.txtFacBuckling.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFacBuckling.Size = New System.Drawing.Size(54, 21)
        Me.txtFacBuckling.TabIndex = 31
        Me.txtFacBuckling.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtH11Tol
        '
        Me.txtH11Tol.AcceptsReturn = True
        Me.txtH11Tol.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtH11Tol.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtH11Tol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtH11Tol.ForeColor = System.Drawing.Color.Blue
        Me.txtH11Tol.Location = New System.Drawing.Point(120, 108)
        Me.txtH11Tol.MaxLength = 0
        Me.txtH11Tol.Name = "txtH11Tol"
        Me.txtH11Tol.ReadOnly = True
        Me.txtH11Tol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtH11Tol.Size = New System.Drawing.Size(54, 21)
        Me.txtH11Tol.TabIndex = 29
        Me.txtH11Tol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblWidMaxAllowed
        '
        Me.lblWidMaxAllowed.BackColor = System.Drawing.Color.Transparent
        Me.lblWidMaxAllowed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWidMaxAllowed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWidMaxAllowed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidMaxAllowed.ForeColor = System.Drawing.Color.Blue
        Me.lblWidMaxAllowed.Location = New System.Drawing.Point(310, 142)
        Me.lblWidMaxAllowed.Name = "lblWidMaxAllowed"
        Me.lblWidMaxAllowed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWidMaxAllowed.Size = New System.Drawing.Size(54, 21)
        Me.lblWidMaxAllowed.TabIndex = 34
        Me.lblWidMaxAllowed.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCavityWid
        '
        Me.lblCavityWid.BackColor = System.Drawing.Color.Transparent
        Me.lblCavityWid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCavityWid.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCavityWid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCavityWid.ForeColor = System.Drawing.Color.Black
        Me.lblCavityWid.Location = New System.Drawing.Point(120, 53)
        Me.lblCavityWid.Name = "lblCavityWid"
        Me.lblCavityWid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCavityWid.Size = New System.Drawing.Size(54, 21)
        Me.lblCavityWid.TabIndex = 33
        Me.lblCavityWid.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(64, 142)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(220, 16)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Maximum Allowable Seal  Width"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(198, 81)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(110, 16)
        Me.Label5.TabIndex = 30
        Me.Label5.Text = "Additional  Factor"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(44, 109)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(70, 16)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "H11 Tol"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblUnitUserL
        '
        Me.lblUnitUserL.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitUserL.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnitUserL.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitUserL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnitUserL.Location = New System.Drawing.Point(338, 15)
        Me.lblUnitUserL.Name = "lblUnitUserL"
        Me.lblUnitUserL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnitUserL.Size = New System.Drawing.Size(40, 17)
        Me.lblUnitUserL.TabIndex = 26
        Me.lblUnitUserL.Text = "(in)"
        Me.lblUnitUserL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(35, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(80, 16)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Cavity Width "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(264, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "Length Unit:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(24, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(194, 17)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = " Width Selection Criterion :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PrintDocu
        '
        Me.PrintDocu.DocumentName = ""
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(406, 358)
        Me.lblBorder.TabIndex = 0
        '
        'IPE_frmSelectionCriteria
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(412, 364)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmSelectionCriteria"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Candidate Seal Selection Criteria"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


#Region "FORM EVENT ROUTINES:"

    Private Sub frmSealCandidates_Load(ByVal sender As Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles MyBase.Load
        '===========================================================================
        InitializeControls()
        DisplayData()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===============================
        lblUnitUserL.Text = "(" & gIPE_Unit.UserL & ")"
        'txtZClear.Enabled = False
    End Sub


    Public Sub DisplayData()
        '=======================
        '....Data is displayed in user unit.
        With gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity
            lblCavityWid.Text = gIPE_Unit.WriteInUserL(.WidMin)
            lblCavityDepth.Text = gIPE_Unit.WriteInUserL(.Depth)
        End With


        With gIPE_SealCandidates
            'AES 04APR16
            If (Math.Abs(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.ZClear_Given) > gcEPS) Then
                lblZClear.Text = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.ZClear_Given)
                lblZClear.ForeColor = Color.Black
            Else
                lblZClear.Text = gIPE_Unit.WriteInUserL(.ZClear)
                lblZClear.ForeColor = Color.Blue
            End If

            txtFacBuckling.Text = .FacBuckling
            txtH11Tol.Text = gIPE_Unit.WriteInUserL(.H11Tol)
            lblWidMaxAllowed.Text = gIPE_Unit.WriteInUserL(.WidMaxAllowed)

            'AES 04APR16 
            lblCompressMinReqd.Text = .CompressPcentValueMinReqd
            lblHFreeMinAllowed.Text = gIPE_Unit.WriteInUserL(.HFreeMinAllowed)
        End With

    End Sub

#End Region

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdPrintForm_Click(ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs) Handles cmdPrintForm.Click

        '==============================================================
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


    Private Sub cmdOK_Click(ByVal sender As Object, _
                            ByVal e As System.EventArgs) Handles cmdOK.Click
        '====================================================================

        Me.Close()
    End Sub

#End Region

End Class


