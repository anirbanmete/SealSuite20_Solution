<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Part_frmNonStdCS_CSeal
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Part_frmNonStdCS_CSeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblErrMsg = New System.Windows.Forms.Label()
        Me.lblAdj = New System.Windows.Forms.Label()
        Me.lblStd = New System.Windows.Forms.Label()
        Me.lblWid = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblWidStd = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblCSealAdjusted = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.updDIndexTArray = New System.Windows.Forms.NumericUpDown()
        Me.updDThetaOpening = New System.Windows.Forms.NumericUpDown()
        Me.updDHfree = New System.Windows.Forms.NumericUpDown()
        Me.txtT = New System.Windows.Forms.TextBox()
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblStandard = New System.Windows.Forms.Label()
        Me.lblFreeHeight = New System.Windows.Forms.Label()
        Me.picSeal = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.updDIndexTArray, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updDThetaOpening, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updDHfree, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(700, 600)
        Me.lblBorder.TabIndex = 1
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
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
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblStandard)
        Me.Panel1.Controls.Add(Me.lblFreeHeight)
        Me.Panel1.Controls.Add(Me.picSeal)
        Me.Panel1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(698, 598)
        Me.Panel1.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(630, 430)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(68, 17)
        Me.Label11.TabIndex = 113
        Me.Label11.Text = "(UnPlated)"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblErrMsg
        '
        Me.lblErrMsg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrMsg.Location = New System.Drawing.Point(415, 507)
        Me.lblErrMsg.Name = "lblErrMsg"
        Me.lblErrMsg.Size = New System.Drawing.Size(256, 30)
        Me.lblErrMsg.TabIndex = 112
        '
        'lblAdj
        '
        Me.lblAdj.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblAdj.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdj.Location = New System.Drawing.Point(575, 412)
        Me.lblAdj.Name = "lblAdj"
        Me.lblAdj.Size = New System.Drawing.Size(60, 13)
        Me.lblAdj.TabIndex = 111
        Me.lblAdj.Text = "Adjusted"
        Me.lblAdj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblStd
        '
        Me.lblStd.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStd.Location = New System.Drawing.Point(490, 412)
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
        Me.lblWid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWid.Location = New System.Drawing.Point(308, 452)
        Me.lblWid.Name = "lblWid"
        Me.lblWid.Size = New System.Drawing.Size(60, 20)
        Me.lblWid.TabIndex = 109
        Me.lblWid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(205, 454)
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
        Me.lblWidStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidStd.Location = New System.Drawing.Point(308, 430)
        Me.lblWidStd.Name = "lblWidStd"
        Me.lblWidStd.Size = New System.Drawing.Size(60, 20)
        Me.lblWidStd.TabIndex = 107
        Me.lblWidStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(205, 432)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 14)
        Me.Label5.TabIndex = 106
        Me.Label5.Text = "Wid : Standard"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCSealAdjusted
        '
        Me.lblCSealAdjusted.BackColor = System.Drawing.Color.White
        Me.lblCSealAdjusted.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.updDIndexTArray.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDIndexTArray.Location = New System.Drawing.Point(558, 474)
        Me.updDIndexTArray.Name = "updDIndexTArray"
        Me.updDIndexTArray.Size = New System.Drawing.Size(20, 21)
        Me.updDIndexTArray.TabIndex = 99
        '
        'updDThetaOpening
        '
        Me.updDThetaOpening.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDThetaOpening.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDThetaOpening.Location = New System.Drawing.Point(558, 452)
        Me.updDThetaOpening.Name = "updDThetaOpening"
        Me.updDThetaOpening.Size = New System.Drawing.Size(20, 21)
        Me.updDThetaOpening.TabIndex = 98
        '
        'updDHfree
        '
        Me.updDHfree.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updDHfree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updDHfree.Increment = New Decimal(New Integer() {0, 0, 0, 0})
        Me.updDHfree.Location = New System.Drawing.Point(558, 430)
        Me.updDHfree.Name = "updDHfree"
        Me.updDHfree.Size = New System.Drawing.Size(20, 21)
        Me.updDHfree.TabIndex = 97
        Me.updDHfree.ThousandsSeparator = True
        '
        'txtT
        '
        Me.txtT.AcceptsReturn = True
        Me.txtT.BackColor = System.Drawing.SystemColors.Window
        Me.txtT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtT.Location = New System.Drawing.Point(582, 474)
        Me.txtT.MaxLength = 0
        Me.txtT.Name = "txtT"
        Me.txtT.Size = New System.Drawing.Size(48, 21)
        Me.txtT.TabIndex = 105
        Me.txtT.Text = "0.00"
        Me.txtT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThetaOpening
        '
        Me.txtThetaOpening.AcceptsReturn = True
        Me.txtThetaOpening.BackColor = System.Drawing.SystemColors.Window
        Me.txtThetaOpening.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThetaOpening.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaOpening.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtThetaOpening.Location = New System.Drawing.Point(582, 452)
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
        Me.txtHfree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHfree.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtHfree.Location = New System.Drawing.Point(582, 430)
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
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(413, 392)
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
        Me.txtHFreeStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFreeStd.ForeColor = System.Drawing.Color.Black
        Me.txtHFreeStd.Location = New System.Drawing.Point(487, 430)
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
        Me.txtTStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTStd.ForeColor = System.Drawing.Color.Black
        Me.txtTStd.Location = New System.Drawing.Point(487, 474)
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
        Me.txtThetaOpeningStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThetaOpeningStd.ForeColor = System.Drawing.Color.Black
        Me.txtThetaOpeningStd.Location = New System.Drawing.Point(487, 452)
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
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(413, 476)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 14)
        Me.Label9.TabIndex = 82
        Me.Label9.Text = "Thickness"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(383, 454)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 14)
        Me.Label8.TabIndex = 81
        Me.Label8.Text = "Opening Angle"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(411, 432)
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
        Me.txtPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtDControl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDControl.ForeColor = System.Drawing.Color.Black
        Me.txtDControl.Location = New System.Drawing.Point(104, 452)
        Me.txtDControl.MaxLength = 0
        Me.txtDControl.Name = "txtDControl"
        Me.txtDControl.ReadOnly = True
        Me.txtDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDControl.Size = New System.Drawing.Size(80, 21)
        Me.txtDControl.TabIndex = 68
        Me.txtDControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDControl.Visible = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 456)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 14)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Control  Dia"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 433)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 14)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "Orientation"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCrossSecNo
        '
        Me.txtCrossSecNo.AcceptsReturn = True
        Me.txtCrossSecNo.BackColor = System.Drawing.Color.White
        Me.txtCrossSecNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 399)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "MCS"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblStandard
        '
        Me.lblStandard.BackColor = System.Drawing.Color.White
        Me.lblStandard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStandard.Location = New System.Drawing.Point(346, 18)
        Me.lblStandard.Name = "lblStandard"
        Me.lblStandard.Size = New System.Drawing.Size(230, 15)
        Me.lblStandard.TabIndex = 2
        Me.lblStandard.Text = "Standard"
        '
        'lblFreeHeight
        '
        Me.lblFreeHeight.BackColor = System.Drawing.Color.White
        Me.lblFreeHeight.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        'frmNonStdCS_CSeal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(705, 605)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmNonStdCS_CSeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Non Standard Cross Sec. - CSeal"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.updDIndexTArray, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updDThetaOpening, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updDHfree, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents lblErrMsg As System.Windows.Forms.Label
    Friend WithEvents lblAdj As System.Windows.Forms.Label
    Friend WithEvents lblStd As System.Windows.Forms.Label
    Friend WithEvents lblWid As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblWidStd As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblCSealAdjusted As System.Windows.Forms.Label
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents updDIndexTArray As System.Windows.Forms.NumericUpDown
    Friend WithEvents updDThetaOpening As System.Windows.Forms.NumericUpDown
    Friend WithEvents updDHfree As System.Windows.Forms.NumericUpDown
    Public WithEvents txtT As System.Windows.Forms.TextBox
    Public WithEvents txtThetaOpening As System.Windows.Forms.TextBox
    Public WithEvents txtHfree As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtHFreeStd As System.Windows.Forms.TextBox
    Public WithEvents txtTStd As System.Windows.Forms.TextBox
    Public WithEvents txtThetaOpeningStd As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents txtPOrient As System.Windows.Forms.TextBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStandard As System.Windows.Forms.Label
    Friend WithEvents lblFreeHeight As System.Windows.Forms.Label
    Public WithEvents picSeal As System.Windows.Forms.PictureBox
End Class
