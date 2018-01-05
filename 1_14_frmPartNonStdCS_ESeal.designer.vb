<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Part_frmNonStdCS_ESeal
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Part_frmNonStdCS_ESeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
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
        Me.Panel1.SuspendLayout()
        CType(Me.updDThetaM1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updDThetaE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 4)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(700, 612)
        Me.lblBorder.TabIndex = 4
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
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
        Me.Panel1.Location = New System.Drawing.Point(4, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(698, 610)
        Me.Panel1.TabIndex = 5
        '
        'cmdViewNomenclature
        '
        Me.cmdViewNomenclature.BackColor = System.Drawing.Color.Silver
        Me.cmdViewNomenclature.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewNomenclature.Image = CType(resources.GetObject("cmdViewNomenclature.Image"), System.Drawing.Image)
        Me.cmdViewNomenclature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdViewNomenclature.Location = New System.Drawing.Point(31, 559)
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
        'frmNonStdCS_ESeal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(706, 618)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmNonStdCS_ESeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Non Standard Cross Sec. - ESeal"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.updDThetaM1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updDThetaE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdViewNomenclature As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents lblStandard As System.Windows.Forms.Label
    Friend WithEvents lblESealAdjusted As System.Windows.Forms.Label
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents updDThetaM1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents updDThetaE1 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtESealDThetaM1 As System.Windows.Forms.TextBox
    Public WithEvents txtESealDThetaE1 As System.Windows.Forms.TextBox
    Public WithEvents lblPlusM1 As System.Windows.Forms.Label
    Public WithEvents lblPlusE1 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents txtESealThetaE1 As System.Windows.Forms.TextBox
    Public WithEvents txtESealThetaM1 As System.Windows.Forms.TextBox
    Friend WithEvents lblESealThetaM1 As System.Windows.Forms.Label
    Friend WithEvents lblESealThetaE1 As System.Windows.Forms.Label
    Public WithEvents txtPOrient As System.Windows.Forms.TextBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Public WithEvents txtESealNConv As System.Windows.Forms.TextBox
    Public WithEvents txtT As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTemplate As System.Windows.Forms.Label
    Public WithEvents txtESealTemplateNo As System.Windows.Forms.TextBox
    Public WithEvents txtESealStageNo As System.Windows.Forms.TextBox
    Public WithEvents txtCrossSecNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblFreeHeight As System.Windows.Forms.Label
    Public WithEvents picSeal As System.Windows.Forms.PictureBox
End Class
