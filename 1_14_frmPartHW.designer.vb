<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Part_frmHW
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Part_frmHW))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.pnlHW = New System.Windows.Forms.Panel()
        Me.lblStd = New System.Windows.Forms.Label()
        Me.lblStandard = New System.Windows.Forms.Label()
        Me.txtH11Tol = New System.Windows.Forms.TextBox()
        Me.txtThick = New System.Windows.Forms.TextBox()
        Me.txtDControl = New System.Windows.Forms.TextBox()
        Me.txtHFree = New System.Windows.Forms.TextBox()
        Me.grpPlating = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPlatingThickMax = New System.Windows.Forms.TextBox()
        Me.txtPlatingThickMin = New System.Windows.Forms.TextBox()
        Me.cmbPlatingCode = New System.Windows.Forms.ComboBox()
        Me.chkPlating = New System.Windows.Forms.CheckBox()
        Me.cmbPlatingThickCode = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtHFreeTolPlus = New System.Windows.Forms.TextBox()
        Me.txtHFreeTolMinus = New System.Windows.Forms.TextBox()
        Me.cmbTemperCode = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblCrossSec = New System.Windows.Forms.Label()
        Me.cmbCrossSec = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbPOrient = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpCoating = New System.Windows.Forms.GroupBox()
        Me.cmbCoating = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.chkCoating = New System.Windows.Forms.CheckBox()
        Me.lblSFinish = New System.Windows.Forms.Label()
        Me.cmbSFinish = New System.Windows.Forms.ComboBox()
        Me.lblUnitSFinish = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbHT = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbMatName = New System.Windows.Forms.ComboBox()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtSegNo = New System.Windows.Forms.TextBox()
        Me.lblSegNo = New System.Windows.Forms.Label()
        Me.chkSeg = New System.Windows.Forms.CheckBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.pnlPanel1 = New System.Windows.Forms.Panel()
        Me.cmdNonStdCS = New System.Windows.Forms.Button()
        Me.ttpToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlHW.SuspendLayout()
        Me.grpPlating.SuspendLayout()
        Me.grpCoating.SuspendLayout()
        Me.grpGroupBox1.SuspendLayout()
        Me.pnlPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 1)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(731, 338)
        Me.lblBorder.TabIndex = 3
        '
        'pnlHW
        '
        Me.pnlHW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlHW.Controls.Add(Me.lblStd)
        Me.pnlHW.Controls.Add(Me.lblStandard)
        Me.pnlHW.Controls.Add(Me.txtH11Tol)
        Me.pnlHW.Controls.Add(Me.txtThick)
        Me.pnlHW.Controls.Add(Me.txtDControl)
        Me.pnlHW.Controls.Add(Me.txtHFree)
        Me.pnlHW.Controls.Add(Me.grpPlating)
        Me.pnlHW.Controls.Add(Me.Label16)
        Me.pnlHW.Controls.Add(Me.Label15)
        Me.pnlHW.Controls.Add(Me.Label14)
        Me.pnlHW.Controls.Add(Me.Label10)
        Me.pnlHW.Controls.Add(Me.Label9)
        Me.pnlHW.Controls.Add(Me.Label8)
        Me.pnlHW.Controls.Add(Me.Label7)
        Me.pnlHW.Controls.Add(Me.txtHFreeTolPlus)
        Me.pnlHW.Controls.Add(Me.txtHFreeTolMinus)
        Me.pnlHW.Controls.Add(Me.cmbTemperCode)
        Me.pnlHW.Controls.Add(Me.Label6)
        Me.pnlHW.Controls.Add(Me.lblCrossSec)
        Me.pnlHW.Controls.Add(Me.cmbCrossSec)
        Me.pnlHW.Controls.Add(Me.Label11)
        Me.pnlHW.Controls.Add(Me.cmbPOrient)
        Me.pnlHW.Controls.Add(Me.Label5)
        Me.pnlHW.Controls.Add(Me.grpCoating)
        Me.pnlHW.Controls.Add(Me.Label3)
        Me.pnlHW.Controls.Add(Me.cmbHT)
        Me.pnlHW.Controls.Add(Me.Label1)
        Me.pnlHW.Controls.Add(Me.Label4)
        Me.pnlHW.Controls.Add(Me.cmbMatName)
        Me.pnlHW.Controls.Add(Me.grpGroupBox1)
        Me.pnlHW.Controls.Add(Me.lblType)
        Me.pnlHW.Controls.Add(Me.cmbType)
        Me.pnlHW.Location = New System.Drawing.Point(4, 1)
        Me.pnlHW.Name = "pnlHW"
        Me.pnlHW.Size = New System.Drawing.Size(729, 290)
        Me.pnlHW.TabIndex = 32
        '
        'lblStd
        '
        Me.lblStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStd.Location = New System.Drawing.Point(354, 16)
        Me.lblStd.Name = "lblStd"
        Me.lblStd.Size = New System.Drawing.Size(69, 16)
        Me.lblStd.TabIndex = 697
        Me.lblStd.Text = "Standard"
        Me.lblStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblStandard
        '
        Me.lblStandard.BackColor = System.Drawing.Color.White
        Me.lblStandard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStandard.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStandard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStandard.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblStandard.Location = New System.Drawing.Point(372, 35)
        Me.lblStandard.Name = "lblStandard"
        Me.lblStandard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStandard.Size = New System.Drawing.Size(32, 20)
        Me.lblStandard.TabIndex = 696
        Me.lblStandard.Text = "Y"
        Me.lblStandard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtH11Tol
        '
        Me.txtH11Tol.AcceptsReturn = True
        Me.txtH11Tol.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtH11Tol.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtH11Tol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtH11Tol.ForeColor = System.Drawing.Color.Blue
        Me.txtH11Tol.Location = New System.Drawing.Point(502, 199)
        Me.txtH11Tol.MaxLength = 0
        Me.txtH11Tol.Name = "txtH11Tol"
        Me.txtH11Tol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtH11Tol.Size = New System.Drawing.Size(54, 21)
        Me.txtH11Tol.TabIndex = 695
        Me.txtH11Tol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtThick
        '
        Me.txtThick.AcceptsReturn = True
        Me.txtThick.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtThick.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtThick.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThick.ForeColor = System.Drawing.Color.Magenta
        Me.txtThick.Location = New System.Drawing.Point(105, 253)
        Me.txtThick.MaxLength = 0
        Me.txtThick.Name = "txtThick"
        Me.txtThick.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtThick.Size = New System.Drawing.Size(54, 21)
        Me.txtThick.TabIndex = 694
        Me.txtThick.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDControl
        '
        Me.txtDControl.AcceptsReturn = True
        Me.txtDControl.BackColor = System.Drawing.Color.White
        Me.txtDControl.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDControl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDControl.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDControl.Location = New System.Drawing.Point(356, 199)
        Me.txtDControl.MaxLength = 0
        Me.txtDControl.Name = "txtDControl"
        Me.txtDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDControl.Size = New System.Drawing.Size(54, 21)
        Me.txtDControl.TabIndex = 693
        Me.txtDControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtHFree
        '
        Me.txtHFree.AcceptsReturn = True
        Me.txtHFree.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.txtHFree.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHFree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFree.ForeColor = System.Drawing.Color.Blue
        Me.txtHFree.Location = New System.Drawing.Point(105, 203)
        Me.txtHFree.MaxLength = 0
        Me.txtHFree.Name = "txtHFree"
        Me.txtHFree.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHFree.Size = New System.Drawing.Size(54, 21)
        Me.txtHFree.TabIndex = 692
        Me.txtHFree.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpPlating
        '
        Me.grpPlating.Controls.Add(Me.Label12)
        Me.grpPlating.Controls.Add(Me.Label2)
        Me.grpPlating.Controls.Add(Me.txtPlatingThickMax)
        Me.grpPlating.Controls.Add(Me.txtPlatingThickMin)
        Me.grpPlating.Controls.Add(Me.cmbPlatingCode)
        Me.grpPlating.Controls.Add(Me.chkPlating)
        Me.grpPlating.Controls.Add(Me.cmbPlatingThickCode)
        Me.grpPlating.Location = New System.Drawing.Point(484, 84)
        Me.grpPlating.Name = "grpPlating"
        Me.grpPlating.Size = New System.Drawing.Size(235, 75)
        Me.grpPlating.TabIndex = 691
        Me.grpPlating.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(181, 26)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(30, 13)
        Me.Label12.TabIndex = 704
        Me.Label12.Text = "Max"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(127, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 703
        Me.Label2.Text = "Min"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPlatingThickMax
        '
        Me.txtPlatingThickMax.AcceptsReturn = True
        Me.txtPlatingThickMax.BackColor = System.Drawing.Color.White
        Me.txtPlatingThickMax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPlatingThickMax.Enabled = False
        Me.txtPlatingThickMax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlatingThickMax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPlatingThickMax.Location = New System.Drawing.Point(170, 42)
        Me.txtPlatingThickMax.MaxLength = 0
        Me.txtPlatingThickMax.Name = "txtPlatingThickMax"
        Me.txtPlatingThickMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlatingThickMax.Size = New System.Drawing.Size(54, 21)
        Me.txtPlatingThickMax.TabIndex = 702
        Me.txtPlatingThickMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPlatingThickMin
        '
        Me.txtPlatingThickMin.AcceptsReturn = True
        Me.txtPlatingThickMin.BackColor = System.Drawing.Color.White
        Me.txtPlatingThickMin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPlatingThickMin.Enabled = False
        Me.txtPlatingThickMin.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlatingThickMin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPlatingThickMin.Location = New System.Drawing.Point(110, 42)
        Me.txtPlatingThickMin.MaxLength = 0
        Me.txtPlatingThickMin.Name = "txtPlatingThickMin"
        Me.txtPlatingThickMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlatingThickMin.Size = New System.Drawing.Size(54, 21)
        Me.txtPlatingThickMin.TabIndex = 701
        Me.txtPlatingThickMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbPlatingCode
        '
        Me.cmbPlatingCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPlatingCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPlatingCode.FormattingEnabled = True
        Me.cmbPlatingCode.Location = New System.Drawing.Point(14, 42)
        Me.cmbPlatingCode.Name = "cmbPlatingCode"
        Me.cmbPlatingCode.Size = New System.Drawing.Size(42, 21)
        Me.cmbPlatingCode.TabIndex = 676
        '
        'chkPlating
        '
        Me.chkPlating.AutoSize = True
        Me.chkPlating.Location = New System.Drawing.Point(14, 20)
        Me.chkPlating.Name = "chkPlating"
        Me.chkPlating.Size = New System.Drawing.Size(61, 17)
        Me.chkPlating.TabIndex = 677
        Me.chkPlating.Text = " Plating"
        Me.chkPlating.UseVisualStyleBackColor = True
        '
        'cmbPlatingThickCode
        '
        Me.cmbPlatingThickCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPlatingThickCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPlatingThickCode.FormattingEnabled = True
        Me.cmbPlatingThickCode.Location = New System.Drawing.Point(62, 42)
        Me.cmbPlatingThickCode.Name = "cmbPlatingThickCode"
        Me.cmbPlatingThickCode.Size = New System.Drawing.Size(42, 21)
        Me.cmbPlatingThickCode.TabIndex = 678
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Silver
        Me.Label16.Location = New System.Drawing.Point(0, 172)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(728, 2)
        Me.Label16.TabIndex = 690
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(34, 256)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(63, 13)
        Me.Label15.TabIndex = 689
        Me.Label15.Text = "Thickness"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Silver
        Me.Label14.Location = New System.Drawing.Point(0, 287)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(727, 2)
        Me.Label14.TabIndex = 760
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(431, 201)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(66, 16)
        Me.Label10.TabIndex = 685
        Me.Label10.Text = "H11 Tol"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(165, 223)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(22, 12)
        Me.Label9.TabIndex = 683
        Me.Label9.Text = "(—)"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(166, 195)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(20, 12)
        Me.Label8.TabIndex = 682
        Me.Label8.Text = "(+)"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(7, 204)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(90, 16)
        Me.Label7.TabIndex = 681
        Me.Label7.Text = "Free Height"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtHFreeTolPlus
        '
        Me.txtHFreeTolPlus.AcceptsReturn = True
        Me.txtHFreeTolPlus.BackColor = System.Drawing.Color.White
        Me.txtHFreeTolPlus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHFreeTolPlus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFreeTolPlus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHFreeTolPlus.Location = New System.Drawing.Point(192, 190)
        Me.txtHFreeTolPlus.MaxLength = 0
        Me.txtHFreeTolPlus.Name = "txtHFreeTolPlus"
        Me.txtHFreeTolPlus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHFreeTolPlus.Size = New System.Drawing.Size(54, 21)
        Me.txtHFreeTolPlus.TabIndex = 679
        Me.txtHFreeTolPlus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtHFreeTolMinus
        '
        Me.txtHFreeTolMinus.AcceptsReturn = True
        Me.txtHFreeTolMinus.BackColor = System.Drawing.Color.White
        Me.txtHFreeTolMinus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHFreeTolMinus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFreeTolMinus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHFreeTolMinus.Location = New System.Drawing.Point(192, 218)
        Me.txtHFreeTolMinus.MaxLength = 0
        Me.txtHFreeTolMinus.Name = "txtHFreeTolMinus"
        Me.txtHFreeTolMinus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHFreeTolMinus.Size = New System.Drawing.Size(54, 21)
        Me.txtHFreeTolMinus.TabIndex = 680
        Me.txtHFreeTolMinus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbTemperCode
        '
        Me.cmbTemperCode.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTemperCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTemperCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTemperCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTemperCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTemperCode.Items.AddRange(New Object() {"16", "15", "62"})
        Me.cmbTemperCode.Location = New System.Drawing.Point(259, 126)
        Me.cmbTemperCode.Name = "cmbTemperCode"
        Me.cmbTemperCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTemperCode.Size = New System.Drawing.Size(48, 21)
        Me.cmbTemperCode.TabIndex = 195
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(258, 110)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 13)
        Me.Label6.TabIndex = 194
        Me.Label6.Text = "Temper"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCrossSec
        '
        Me.lblCrossSec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSec.Location = New System.Drawing.Point(256, 19)
        Me.lblCrossSec.Name = "lblCrossSec"
        Me.lblCrossSec.Size = New System.Drawing.Size(64, 14)
        Me.lblCrossSec.TabIndex = 193
        Me.lblCrossSec.Text = "MCS"
        Me.lblCrossSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbCrossSec
        '
        Me.cmbCrossSec.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCrossSec.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCrossSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCrossSec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCrossSec.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCrossSec.Location = New System.Drawing.Point(248, 36)
        Me.cmbCrossSec.Name = "cmbCrossSec"
        Me.cmbCrossSec.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCrossSec.Size = New System.Drawing.Size(80, 21)
        Me.cmbCrossSec.TabIndex = 192
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(278, 203)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 13)
        Me.Label11.TabIndex = 191
        Me.Label11.Text = "Control Dia"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbPOrient
        '
        Me.cmbPOrient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPOrient.Location = New System.Drawing.Point(133, 36)
        Me.cmbPOrient.Name = "cmbPOrient"
        Me.cmbPOrient.Size = New System.Drawing.Size(72, 21)
        Me.cmbPOrient.TabIndex = 189
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(134, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 13)
        Me.Label5.TabIndex = 188
        Me.Label5.Text = "Orientation"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpCoating
        '
        Me.grpCoating.Controls.Add(Me.cmbCoating)
        Me.grpCoating.Controls.Add(Me.Label13)
        Me.grpCoating.Controls.Add(Me.chkCoating)
        Me.grpCoating.Controls.Add(Me.lblSFinish)
        Me.grpCoating.Controls.Add(Me.cmbSFinish)
        Me.grpCoating.Controls.Add(Me.lblUnitSFinish)
        Me.grpCoating.Location = New System.Drawing.Point(322, 84)
        Me.grpCoating.Name = "grpCoating"
        Me.grpCoating.Size = New System.Drawing.Size(153, 75)
        Me.grpCoating.TabIndex = 187
        Me.grpCoating.TabStop = False
        '
        'cmbCoating
        '
        Me.cmbCoating.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCoating.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCoating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCoating.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCoating.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCoating.Items.AddRange(New Object() {"Tricom", "T800"})
        Me.cmbCoating.Location = New System.Drawing.Point(12, 42)
        Me.cmbCoating.Name = "cmbCoating"
        Me.cmbCoating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCoating.Size = New System.Drawing.Size(64, 21)
        Me.cmbCoating.TabIndex = 178
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(26, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(51, 13)
        Me.Label13.TabIndex = 177
        Me.Label13.Text = "Coating"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkCoating
        '
        Me.chkCoating.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.chkCoating.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCoating.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCoating.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCoating.Location = New System.Drawing.Point(12, 14)
        Me.chkCoating.Name = "chkCoating"
        Me.chkCoating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCoating.Size = New System.Drawing.Size(13, 14)
        Me.chkCoating.TabIndex = 182
        Me.chkCoating.UseVisualStyleBackColor = False
        '
        'lblSFinish
        '
        Me.lblSFinish.AutoSize = True
        Me.lblSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSFinish.Location = New System.Drawing.Point(93, 13)
        Me.lblSFinish.Name = "lblSFinish"
        Me.lblSFinish.Size = New System.Drawing.Size(47, 13)
        Me.lblSFinish.TabIndex = 180
        Me.lblSFinish.Text = "SFinish"
        Me.lblSFinish.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbSFinish
        '
        Me.cmbSFinish.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSFinish.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSFinish.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSFinish.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSFinish.Items.AddRange(New Object() {"65", "125"})
        Me.cmbSFinish.Location = New System.Drawing.Point(92, 42)
        Me.cmbSFinish.Name = "cmbSFinish"
        Me.cmbSFinish.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSFinish.Size = New System.Drawing.Size(48, 21)
        Me.cmbSFinish.TabIndex = 179
        '
        'lblUnitSFinish
        '
        Me.lblUnitSFinish.AutoSize = True
        Me.lblUnitSFinish.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitSFinish.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnitSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitSFinish.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnitSFinish.Location = New System.Drawing.Point(97, 27)
        Me.lblUnitSFinish.Name = "lblUnitSFinish"
        Me.lblUnitSFinish.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnitSFinish.Size = New System.Drawing.Size(39, 13)
        Me.lblUnitSFinish.TabIndex = 181
        Me.lblUnitSFinish.Text = "(rms)"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Silver
        Me.Label3.Location = New System.Drawing.Point(0, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(728, 2)
        Me.Label3.TabIndex = 186
        '
        'cmbHT
        '
        Me.cmbHT.BackColor = System.Drawing.SystemColors.Window
        Me.cmbHT.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbHT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbHT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbHT.Items.AddRange(New Object() {"16", "15", "62"})
        Me.cmbHT.Location = New System.Drawing.Point(192, 126)
        Me.cmbHT.Name = "cmbHT"
        Me.cmbHT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbHT.Size = New System.Drawing.Size(48, 21)
        Me.cmbHT.TabIndex = 184
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(205, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 13)
        Me.Label1.TabIndex = 183
        Me.Label1.Text = "HT"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(23, 108)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 176
        Me.Label4.Text = "Material"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbMatName
        '
        Me.cmbMatName.BackColor = System.Drawing.SystemColors.Window
        Me.cmbMatName.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbMatName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMatName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMatName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbMatName.Location = New System.Drawing.Point(10, 126)
        Me.cmbMatName.Name = "cmbMatName"
        Me.cmbMatName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbMatName.Size = New System.Drawing.Size(168, 21)
        Me.cmbMatName.TabIndex = 175
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.txtSegNo)
        Me.grpGroupBox1.Controls.Add(Me.lblSegNo)
        Me.grpGroupBox1.Controls.Add(Me.chkSeg)
        Me.grpGroupBox1.Location = New System.Drawing.Point(484, 17)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(157, 44)
        Me.grpGroupBox1.TabIndex = 33
        Me.grpGroupBox1.TabStop = False
        '
        'txtSegNo
        '
        Me.txtSegNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSegNo.Location = New System.Drawing.Point(121, 15)
        Me.txtSegNo.Name = "txtSegNo"
        Me.txtSegNo.Size = New System.Drawing.Size(27, 21)
        Me.txtSegNo.TabIndex = 35
        '
        'lblSegNo
        '
        Me.lblSegNo.AutoSize = True
        Me.lblSegNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSegNo.Location = New System.Drawing.Point(98, 19)
        Me.lblSegNo.Name = "lblSegNo"
        Me.lblSegNo.Size = New System.Drawing.Size(16, 13)
        Me.lblSegNo.TabIndex = 34
        Me.lblSegNo.Text = "#"
        '
        'chkSeg
        '
        Me.chkSeg.AutoSize = True
        Me.chkSeg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeg.Location = New System.Drawing.Point(7, 18)
        Me.chkSeg.Name = "chkSeg"
        Me.chkSeg.Size = New System.Drawing.Size(91, 17)
        Me.chkSeg.TabIndex = 33
        Me.chkSeg.Text = "Segmented"
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.Location = New System.Drawing.Point(32, 19)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(35, 13)
        Me.lblType.TabIndex = 28
        Me.lblType.Text = "Type"
        Me.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbType.Location = New System.Drawing.Point(10, 36)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(80, 21)
        Me.cmbType.TabIndex = 1
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(555, 11)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 3
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
        Me.cmdCancel.Location = New System.Drawing.Point(636, 11)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'pnlPanel1
        '
        Me.pnlPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPanel1.Controls.Add(Me.cmdNonStdCS)
        Me.pnlPanel1.Controls.Add(Me.cmdOK)
        Me.pnlPanel1.Controls.Add(Me.cmdCancel)
        Me.pnlPanel1.Location = New System.Drawing.Point(4, 290)
        Me.pnlPanel1.Name = "pnlPanel1"
        Me.pnlPanel1.Size = New System.Drawing.Size(729, 48)
        Me.pnlPanel1.TabIndex = 33
        '
        'cmdNonStdCS
        '
        Me.cmdNonStdCS.BackColor = System.Drawing.Color.Silver
        Me.cmdNonStdCS.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNonStdCS.Location = New System.Drawing.Point(10, 11)
        Me.cmdNonStdCS.Name = "cmdNonStdCS"
        Me.cmdNonStdCS.Size = New System.Drawing.Size(122, 28)
        Me.cmdNonStdCS.TabIndex = 587
        Me.cmdNonStdCS.Text = "Non Standard CS"
        Me.cmdNonStdCS.UseVisualStyleBackColor = False
        '
        'Part_frmHW
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(736, 340)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlPanel1)
        Me.Controls.Add(Me.pnlHW)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Part_frmHW"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SealTest: Hardware"
        Me.pnlHW.ResumeLayout(False)
        Me.pnlHW.PerformLayout()
        Me.grpPlating.ResumeLayout(False)
        Me.grpPlating.PerformLayout()
        Me.grpCoating.ResumeLayout(False)
        Me.grpCoating.PerformLayout()
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.pnlPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents pnlHW As System.Windows.Forms.Panel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents txtHFreeTolPlus As System.Windows.Forms.TextBox
    Public WithEvents txtHFreeTolMinus As System.Windows.Forms.TextBox
    Friend WithEvents cmbPlatingThickCode As System.Windows.Forms.ComboBox
    Friend WithEvents chkPlating As System.Windows.Forms.CheckBox
    Friend WithEvents cmbPlatingCode As System.Windows.Forms.ComboBox
    Public WithEvents cmbTemperCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblCrossSec As System.Windows.Forms.Label
    Public WithEvents cmbCrossSec As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbPOrient As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grpCoating As System.Windows.Forms.GroupBox
    Public WithEvents cmbCoating As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents chkCoating As System.Windows.Forms.CheckBox
    Friend WithEvents lblSFinish As System.Windows.Forms.Label
    Public WithEvents cmbSFinish As System.Windows.Forms.ComboBox
    Public WithEvents lblUnitSFinish As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents cmbHT As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents cmbMatName As System.Windows.Forms.ComboBox
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtSegNo As System.Windows.Forms.TextBox
    Friend WithEvents lblSegNo As System.Windows.Forms.Label
    Friend WithEvents chkSeg As System.Windows.Forms.CheckBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblType As System.Windows.Forms.Label
    Public WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents grpPlating As System.Windows.Forms.GroupBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Public WithEvents txtHFree As System.Windows.Forms.TextBox
    Public WithEvents txtThick As System.Windows.Forms.TextBox
    Public WithEvents txtH11Tol As System.Windows.Forms.TextBox
    Friend WithEvents pnlPanel1 As System.Windows.Forms.Panel
    Friend WithEvents lblStd As System.Windows.Forms.Label
    Public WithEvents lblStandard As System.Windows.Forms.Label
    Friend WithEvents cmdNonStdCS As System.Windows.Forms.Button
    Friend WithEvents ttpToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents txtPlatingThickMax As System.Windows.Forms.TextBox
    Public WithEvents txtPlatingThickMin As System.Windows.Forms.TextBox
End Class
