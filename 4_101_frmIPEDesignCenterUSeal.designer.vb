<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPE_frmDesignCenterUSeal
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmDesignCenterUSeal))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuPrintForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCreateWordDocu = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblHFree_New = New System.Windows.Forms.Label()
        Me.cmdViewNomenclature = New System.Windows.Forms.Button()
        Me.txtCrossSecNo_New = New System.Windows.Forms.TextBox()
        Me.lblCrossSecNew = New System.Windows.Forms.Label()
        Me.lblCrossSecOrg = New System.Windows.Forms.Label()
        Me.lblErrMsg = New System.Windows.Forms.Label()
        Me.cmdDXF = New System.Windows.Forms.Button()
        Me.updIndexTArray = New System.Windows.Forms.NumericUpDown()
        Me.txtT_New = New System.Windows.Forms.TextBox()
        Me.txtT = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.updRad2 = New System.Windows.Forms.NumericUpDown()
        Me.txtRad2_New = New System.Windows.Forms.TextBox()
        Me.txtRad2Std = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.updRad1 = New System.Windows.Forms.NumericUpDown()
        Me.txtRad1_New = New System.Windows.Forms.TextBox()
        Me.txtRad1Std = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.updLLeg = New System.Windows.Forms.NumericUpDown()
        Me.txtLLeg_New = New System.Windows.Forms.TextBox()
        Me.txtLLegStd = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblAdj = New System.Windows.Forms.Label()
        Me.lblStd = New System.Windows.Forms.Label()
        Me.lblWid_New = New System.Windows.Forms.Label()
        Me.lblWidStd = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblUSealAdjusted = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.updTheta2 = New System.Windows.Forms.NumericUpDown()
        Me.updTheta1 = New System.Windows.Forms.NumericUpDown()
        Me.txtTheta2_New = New System.Windows.Forms.TextBox()
        Me.txtTheta1_New = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtTheta2Std = New System.Windows.Forms.TextBox()
        Me.txtTheta1Std = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtPOrient = New System.Windows.Forms.TextBox()
        Me.txtDControl = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCrossSecNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblHFree_Org = New System.Windows.Forms.Label()
        Me.lblFreeHeight = New System.Windows.Forms.Label()
        Me.picSeal = New System.Windows.Forms.PictureBox()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.PrintDocu = New System.Drawing.Printing.PrintDocument()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.MenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.updIndexTArray, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRad2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updRad1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updLLeg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updTheta2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.updTheta1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.AutoSize = False
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPrintForm, Me.mnuOptions})
        Me.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(703, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuPrintForm
        '
        Me.mnuPrintForm.AutoSize = False
        Me.mnuPrintForm.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuPrintForm.Image = CType(resources.GetObject("mnuPrintForm.Image"), System.Drawing.Image)
        Me.mnuPrintForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.mnuPrintForm.Name = "mnuPrintForm"
        Me.mnuPrintForm.Size = New System.Drawing.Size(81, 20)
        Me.mnuPrintForm.Text = "&PrintForm"
        Me.mnuPrintForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'mnuOptions
        '
        Me.mnuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCreateWordDocu})
        Me.mnuOptions.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuOptions.Name = "mnuOptions"
        Me.mnuOptions.Size = New System.Drawing.Size(65, 20)
        Me.mnuOptions.Text = "Options"
        '
        'mnuCreateWordDocu
        '
        Me.mnuCreateWordDocu.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuCreateWordDocu.Image = CType(resources.GetObject("mnuCreateWordDocu.Image"), System.Drawing.Image)
        Me.mnuCreateWordDocu.Name = "mnuCreateWordDocu"
        Me.mnuCreateWordDocu.Size = New System.Drawing.Size(204, 22)
        Me.mnuCreateWordDocu.Text = "Create Word Document"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblHFree_New)
        Me.Panel1.Controls.Add(Me.cmdViewNomenclature)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo_New)
        Me.Panel1.Controls.Add(Me.lblCrossSecNew)
        Me.Panel1.Controls.Add(Me.lblCrossSecOrg)
        Me.Panel1.Controls.Add(Me.lblErrMsg)
        Me.Panel1.Controls.Add(Me.cmdDXF)
        Me.Panel1.Controls.Add(Me.updIndexTArray)
        Me.Panel1.Controls.Add(Me.txtT_New)
        Me.Panel1.Controls.Add(Me.txtT)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.updRad2)
        Me.Panel1.Controls.Add(Me.txtRad2_New)
        Me.Panel1.Controls.Add(Me.txtRad2Std)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.updRad1)
        Me.Panel1.Controls.Add(Me.txtRad1_New)
        Me.Panel1.Controls.Add(Me.txtRad1Std)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.updLLeg)
        Me.Panel1.Controls.Add(Me.txtLLeg_New)
        Me.Panel1.Controls.Add(Me.txtLLegStd)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.lblAdj)
        Me.Panel1.Controls.Add(Me.lblStd)
        Me.Panel1.Controls.Add(Me.lblWid_New)
        Me.Panel1.Controls.Add(Me.lblWidStd)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.lblUSealAdjusted)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.updTheta2)
        Me.Panel1.Controls.Add(Me.updTheta1)
        Me.Panel1.Controls.Add(Me.txtTheta2_New)
        Me.Panel1.Controls.Add(Me.txtTheta1_New)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtTheta2Std)
        Me.Panel1.Controls.Add(Me.txtTheta1Std)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtPOrient)
        Me.Panel1.Controls.Add(Me.txtDControl)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblHFree_Org)
        Me.Panel1.Controls.Add(Me.lblFreeHeight)
        Me.Panel1.Controls.Add(Me.picSeal)
        Me.Panel1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(3, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(697, 644)
        Me.Panel1.TabIndex = 5
        '
        'lblHFree_New
        '
        Me.lblHFree_New.BackColor = System.Drawing.Color.White
        Me.lblHFree_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHFree_New.Location = New System.Drawing.Point(348, 35)
        Me.lblHFree_New.Name = "lblHFree_New"
        Me.lblHFree_New.Size = New System.Drawing.Size(230, 15)
        Me.lblHFree_New.TabIndex = 169
        Me.lblHFree_New.Text = "New"
        '
        'cmdViewNomenclature
        '
        Me.cmdViewNomenclature.BackColor = System.Drawing.Color.Silver
        Me.cmdViewNomenclature.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewNomenclature.Image = CType(resources.GetObject("cmdViewNomenclature.Image"), System.Drawing.Image)
        Me.cmdViewNomenclature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdViewNomenclature.Location = New System.Drawing.Point(153, 599)
        Me.cmdViewNomenclature.Name = "cmdViewNomenclature"
        Me.cmdViewNomenclature.Size = New System.Drawing.Size(147, 28)
        Me.cmdViewNomenclature.TabIndex = 168
        Me.cmdViewNomenclature.Text = "     View Nomenclature"
        Me.cmdViewNomenclature.UseVisualStyleBackColor = False
        '
        'txtCrossSecNo_New
        '
        Me.txtCrossSecNo_New.AcceptsReturn = True
        Me.txtCrossSecNo_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtCrossSecNo_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo_New.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCrossSecNo_New.Location = New System.Drawing.Point(103, 442)
        Me.txtCrossSecNo_New.MaxLength = 0
        Me.txtCrossSecNo_New.Name = "txtCrossSecNo_New"
        Me.txtCrossSecNo_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCrossSecNo_New.Size = New System.Drawing.Size(80, 21)
        Me.txtCrossSecNo_New.TabIndex = 165
        Me.txtCrossSecNo_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblCrossSecNew
        '
        Me.lblCrossSecNew.AutoSize = True
        Me.lblCrossSecNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSecNew.Location = New System.Drawing.Point(61, 442)
        Me.lblCrossSecNew.Name = "lblCrossSecNew"
        Me.lblCrossSecNew.Size = New System.Drawing.Size(31, 13)
        Me.lblCrossSecNew.TabIndex = 167
        Me.lblCrossSecNew.Text = "New"
        Me.lblCrossSecNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCrossSecOrg
        '
        Me.lblCrossSecOrg.AutoSize = True
        Me.lblCrossSecOrg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSecOrg.Location = New System.Drawing.Point(43, 420)
        Me.lblCrossSecOrg.Name = "lblCrossSecOrg"
        Me.lblCrossSecOrg.Size = New System.Drawing.Size(51, 13)
        Me.lblCrossSecOrg.TabIndex = 166
        Me.lblCrossSecOrg.Text = "Original"
        Me.lblCrossSecOrg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblErrMsg
        '
        Me.lblErrMsg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrMsg.Location = New System.Drawing.Point(434, 568)
        Me.lblErrMsg.Name = "lblErrMsg"
        Me.lblErrMsg.Size = New System.Drawing.Size(256, 20)
        Me.lblErrMsg.TabIndex = 130
        '
        'cmdDXF
        '
        Me.cmdDXF.BackColor = System.Drawing.Color.Silver
        Me.cmdDXF.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDXF.Image = CType(resources.GetObject("cmdDXF.Image"), System.Drawing.Image)
        Me.cmdDXF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDXF.Location = New System.Drawing.Point(12, 599)
        Me.cmdDXF.Name = "cmdDXF"
        Me.cmdDXF.Size = New System.Drawing.Size(123, 28)
        Me.cmdDXF.TabIndex = 66
        Me.cmdDXF.Text = "    Export To DXF"
        Me.cmdDXF.UseVisualStyleBackColor = False
        '
        'updIndexTArray
        '
        Me.updIndexTArray.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updIndexTArray.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updIndexTArray.Location = New System.Drawing.Point(578, 419)
        Me.updIndexTArray.Name = "updIndexTArray"
        Me.updIndexTArray.Size = New System.Drawing.Size(20, 21)
        Me.updIndexTArray.TabIndex = 5
        '
        'txtT_New
        '
        Me.txtT_New.AcceptsReturn = True
        Me.txtT_New.BackColor = System.Drawing.Color.Gainsboro
        Me.txtT_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT_New.ForeColor = System.Drawing.Color.Black
        Me.txtT_New.Location = New System.Drawing.Point(602, 419)
        Me.txtT_New.MaxLength = 0
        Me.txtT_New.Name = "txtT_New"
        Me.txtT_New.ReadOnly = True
        Me.txtT_New.Size = New System.Drawing.Size(66, 21)
        Me.txtT_New.TabIndex = 5
        Me.txtT_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtT
        '
        Me.txtT.AcceptsReturn = True
        Me.txtT.BackColor = System.Drawing.Color.White
        Me.txtT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtT.ForeColor = System.Drawing.Color.Magenta
        Me.txtT.Location = New System.Drawing.Point(507, 419)
        Me.txtT.MaxLength = 0
        Me.txtT.Name = "txtT"
        Me.txtT.ReadOnly = True
        Me.txtT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtT.Size = New System.Drawing.Size(66, 21)
        Me.txtT.TabIndex = 127
        Me.txtT.TabStop = False
        Me.txtT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(390, 421)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(115, 14)
        Me.Label11.TabIndex = 126
        Me.Label11.Text = "t"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'updRad2
        '
        Me.updRad2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRad2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRad2.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.updRad2.Location = New System.Drawing.Point(578, 534)
        Me.updRad2.Name = "updRad2"
        Me.updRad2.Size = New System.Drawing.Size(20, 21)
        Me.updRad2.TabIndex = 4
        Me.updRad2.ThousandsSeparator = True
        '
        'txtRad2_New
        '
        Me.txtRad2_New.AcceptsReturn = True
        Me.txtRad2_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRad2_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRad2_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRad2_New.ForeColor = System.Drawing.Color.Black
        Me.txtRad2_New.Location = New System.Drawing.Point(602, 534)
        Me.txtRad2_New.MaxLength = 0
        Me.txtRad2_New.Name = "txtRad2_New"
        Me.txtRad2_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRad2_New.Size = New System.Drawing.Size(66, 21)
        Me.txtRad2_New.TabIndex = 4
        Me.txtRad2_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRad2Std
        '
        Me.txtRad2Std.AcceptsReturn = True
        Me.txtRad2Std.BackColor = System.Drawing.Color.White
        Me.txtRad2Std.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRad2Std.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRad2Std.ForeColor = System.Drawing.Color.Magenta
        Me.txtRad2Std.Location = New System.Drawing.Point(507, 534)
        Me.txtRad2Std.MaxLength = 0
        Me.txtRad2Std.Name = "txtRad2Std"
        Me.txtRad2Std.ReadOnly = True
        Me.txtRad2Std.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRad2Std.Size = New System.Drawing.Size(66, 21)
        Me.txtRad2Std.TabIndex = 123
        Me.txtRad2Std.TabStop = False
        Me.txtRad2Std.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(390, 536)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(115, 14)
        Me.Label14.TabIndex = 122
        Me.Label14.Text = "Sealing Surface R"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'updRad1
        '
        Me.updRad1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updRad1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updRad1.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.updRad1.Location = New System.Drawing.Point(578, 488)
        Me.updRad1.Name = "updRad1"
        Me.updRad1.Size = New System.Drawing.Size(20, 21)
        Me.updRad1.TabIndex = 3
        Me.updRad1.ThousandsSeparator = True
        '
        'txtRad1_New
        '
        Me.txtRad1_New.AcceptsReturn = True
        Me.txtRad1_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtRad1_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRad1_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRad1_New.ForeColor = System.Drawing.Color.Black
        Me.txtRad1_New.Location = New System.Drawing.Point(602, 488)
        Me.txtRad1_New.MaxLength = 0
        Me.txtRad1_New.Name = "txtRad1_New"
        Me.txtRad1_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRad1_New.Size = New System.Drawing.Size(66, 21)
        Me.txtRad1_New.TabIndex = 3
        Me.txtRad1_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRad1Std
        '
        Me.txtRad1Std.AcceptsReturn = True
        Me.txtRad1Std.BackColor = System.Drawing.Color.White
        Me.txtRad1Std.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRad1Std.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRad1Std.ForeColor = System.Drawing.Color.Magenta
        Me.txtRad1Std.Location = New System.Drawing.Point(507, 488)
        Me.txtRad1Std.MaxLength = 0
        Me.txtRad1Std.Name = "txtRad1Std"
        Me.txtRad1Std.ReadOnly = True
        Me.txtRad1Std.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRad1Std.Size = New System.Drawing.Size(66, 21)
        Me.txtRad1Std.TabIndex = 119
        Me.txtRad1Std.TabStop = False
        Me.txtRad1Std.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(390, 490)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(115, 14)
        Me.Label13.TabIndex = 118
        Me.Label13.Text = "Conv R"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'updLLeg
        '
        Me.updLLeg.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updLLeg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updLLeg.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.updLLeg.Location = New System.Drawing.Point(578, 442)
        Me.updLLeg.Name = "updLLeg"
        Me.updLLeg.Size = New System.Drawing.Size(20, 21)
        Me.updLLeg.TabIndex = 2
        Me.updLLeg.ThousandsSeparator = True
        '
        'txtLLeg_New
        '
        Me.txtLLeg_New.AcceptsReturn = True
        Me.txtLLeg_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtLLeg_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLLeg_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLLeg_New.ForeColor = System.Drawing.Color.Black
        Me.txtLLeg_New.Location = New System.Drawing.Point(602, 442)
        Me.txtLLeg_New.MaxLength = 0
        Me.txtLLeg_New.Name = "txtLLeg_New"
        Me.txtLLeg_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLLeg_New.Size = New System.Drawing.Size(66, 21)
        Me.txtLLeg_New.TabIndex = 2
        Me.txtLLeg_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLLegStd
        '
        Me.txtLLegStd.AcceptsReturn = True
        Me.txtLLegStd.BackColor = System.Drawing.Color.White
        Me.txtLLegStd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLLegStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLLegStd.ForeColor = System.Drawing.Color.Magenta
        Me.txtLLegStd.Location = New System.Drawing.Point(507, 442)
        Me.txtLLegStd.MaxLength = 0
        Me.txtLLegStd.Name = "txtLLegStd"
        Me.txtLLegStd.ReadOnly = True
        Me.txtLLegStd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLLegStd.Size = New System.Drawing.Size(66, 21)
        Me.txtLLegStd.TabIndex = 115
        Me.txtLLegStd.TabStop = False
        Me.txtLLegStd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(390, 444)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(115, 14)
        Me.Label12.TabIndex = 114
        Me.Label12.Text = "Leg Length"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAdj
        '
        Me.lblAdj.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblAdj.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdj.Location = New System.Drawing.Point(611, 403)
        Me.lblAdj.Name = "lblAdj"
        Me.lblAdj.Size = New System.Drawing.Size(48, 13)
        Me.lblAdj.TabIndex = 111
        Me.lblAdj.Text = "New"
        Me.lblAdj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblStd
        '
        Me.lblStd.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStd.Location = New System.Drawing.Point(510, 403)
        Me.lblStd.Name = "lblStd"
        Me.lblStd.Size = New System.Drawing.Size(60, 13)
        Me.lblStd.TabIndex = 110
        Me.lblStd.Text = "Original"
        Me.lblStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWid_New
        '
        Me.lblWid_New.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblWid_New.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWid_New.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblWid_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWid_New.Location = New System.Drawing.Point(218, 443)
        Me.lblWid_New.Name = "lblWid_New"
        Me.lblWid_New.Size = New System.Drawing.Size(60, 20)
        Me.lblWid_New.TabIndex = 109
        Me.lblWid_New.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWidStd
        '
        Me.lblWidStd.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblWidStd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWidStd.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblWidStd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWidStd.Location = New System.Drawing.Point(218, 419)
        Me.lblWidStd.Name = "lblWidStd"
        Me.lblWidStd.Size = New System.Drawing.Size(60, 20)
        Me.lblWidStd.TabIndex = 107
        Me.lblWidStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(219, 402)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 14)
        Me.Label5.TabIndex = 106
        Me.Label5.Text = "Width"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUSealAdjusted
        '
        Me.lblUSealAdjusted.BackColor = System.Drawing.Color.White
        Me.lblUSealAdjusted.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUSealAdjusted.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblUSealAdjusted.Location = New System.Drawing.Point(346, 35)
        Me.lblUSealAdjusted.Name = "lblUSealAdjusted"
        Me.lblUSealAdjusted.Size = New System.Drawing.Size(246, 14)
        Me.lblUSealAdjusted.TabIndex = 104
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(518, 609)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 6
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
        Me.cmdCancel.Location = New System.Drawing.Point(602, 609)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 7
        Me.cmdCancel.Text = "   &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'updTheta2
        '
        Me.updTheta2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updTheta2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updTheta2.Location = New System.Drawing.Point(578, 511)
        Me.updTheta2.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.updTheta2.Name = "updTheta2"
        Me.updTheta2.Size = New System.Drawing.Size(20, 21)
        Me.updTheta2.TabIndex = 1
        '
        'updTheta1
        '
        Me.updTheta1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.updTheta1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updTheta1.Location = New System.Drawing.Point(578, 465)
        Me.updTheta1.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.updTheta1.Name = "updTheta1"
        Me.updTheta1.Size = New System.Drawing.Size(20, 21)
        Me.updTheta1.TabIndex = 0
        '
        'txtTheta2_New
        '
        Me.txtTheta2_New.AcceptsReturn = True
        Me.txtTheta2_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtTheta2_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTheta2_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTheta2_New.ForeColor = System.Drawing.Color.Black
        Me.txtTheta2_New.Location = New System.Drawing.Point(602, 511)
        Me.txtTheta2_New.MaxLength = 0
        Me.txtTheta2_New.Name = "txtTheta2_New"
        Me.txtTheta2_New.Size = New System.Drawing.Size(66, 21)
        Me.txtTheta2_New.TabIndex = 1
        Me.txtTheta2_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtTheta1_New
        '
        Me.txtTheta1_New.AcceptsReturn = True
        Me.txtTheta1_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtTheta1_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTheta1_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTheta1_New.ForeColor = System.Drawing.Color.Black
        Me.txtTheta1_New.Location = New System.Drawing.Point(602, 465)
        Me.txtTheta1_New.MaxLength = 0
        Me.txtTheta1_New.Name = "txtTheta1_New"
        Me.txtTheta1_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTheta1_New.Size = New System.Drawing.Size(66, 21)
        Me.txtTheta1_New.TabIndex = 0
        Me.txtTheta1_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(389, 396)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 16)
        Me.Label6.TabIndex = 90
        Me.Label6.Text = " Geometry:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTheta2Std
        '
        Me.txtTheta2Std.AcceptsReturn = True
        Me.txtTheta2Std.BackColor = System.Drawing.Color.White
        Me.txtTheta2Std.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTheta2Std.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTheta2Std.ForeColor = System.Drawing.Color.Magenta
        Me.txtTheta2Std.Location = New System.Drawing.Point(507, 511)
        Me.txtTheta2Std.MaxLength = 0
        Me.txtTheta2Std.Name = "txtTheta2Std"
        Me.txtTheta2Std.ReadOnly = True
        Me.txtTheta2Std.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTheta2Std.Size = New System.Drawing.Size(66, 21)
        Me.txtTheta2Std.TabIndex = 88
        Me.txtTheta2Std.TabStop = False
        Me.txtTheta2Std.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtTheta1Std
        '
        Me.txtTheta1Std.AcceptsReturn = True
        Me.txtTheta1Std.BackColor = System.Drawing.Color.White
        Me.txtTheta1Std.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTheta1Std.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTheta1Std.ForeColor = System.Drawing.Color.Magenta
        Me.txtTheta1Std.Location = New System.Drawing.Point(507, 465)
        Me.txtTheta1Std.MaxLength = 0
        Me.txtTheta1Std.Name = "txtTheta1Std"
        Me.txtTheta1Std.ReadOnly = True
        Me.txtTheta1Std.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTheta1Std.Size = New System.Drawing.Size(66, 21)
        Me.txtTheta1Std.TabIndex = 87
        Me.txtTheta1Std.TabStop = False
        Me.txtTheta1Std.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(390, 513)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(115, 14)
        Me.Label9.TabIndex = 82
        Me.Label9.Text = "Sealing Surface Arc"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(390, 467)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(115, 14)
        Me.Label8.TabIndex = 81
        Me.Label8.Text = "Conv Arc"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPOrient
        '
        Me.txtPOrient.AcceptsReturn = True
        Me.txtPOrient.BackColor = System.Drawing.Color.White
        Me.txtPOrient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPOrient.ForeColor = System.Drawing.Color.Black
        Me.txtPOrient.Location = New System.Drawing.Point(103, 479)
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
        Me.txtDControl.BackColor = System.Drawing.Color.White
        Me.txtDControl.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDControl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDControl.ForeColor = System.Drawing.Color.Black
        Me.txtDControl.Location = New System.Drawing.Point(103, 503)
        Me.txtDControl.MaxLength = 0
        Me.txtDControl.Name = "txtDControl"
        Me.txtDControl.ReadOnly = True
        Me.txtDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDControl.Size = New System.Drawing.Size(80, 21)
        Me.txtDControl.TabIndex = 68
        Me.txtDControl.TabStop = False
        Me.txtDControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 505)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 14)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Control  Dia"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(10, 481)
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
        Me.txtCrossSecNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo.ForeColor = System.Drawing.Color.Black
        Me.txtCrossSecNo.Location = New System.Drawing.Point(103, 419)
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
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(121, 402)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Number"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 396)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 16)
        Me.Label1.TabIndex = 111
        Me.Label1.Text = "Cross Sec."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblHFree_Org
        '
        Me.lblHFree_Org.BackColor = System.Drawing.Color.White
        Me.lblHFree_Org.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHFree_Org.Location = New System.Drawing.Point(346, 18)
        Me.lblHFree_Org.Name = "lblHFree_Org"
        Me.lblHFree_Org.Size = New System.Drawing.Size(230, 15)
        Me.lblHFree_Org.TabIndex = 2
        Me.lblHFree_Org.Text = "Original"
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
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(2, 25)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(699, 646)
        Me.lblBorder.TabIndex = 4
        Me.lblBorder.Text = "Label1"
        '
        'IPE_frmDesignCenterUSeal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(703, 674)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmDesignCenterUSeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Design Center - USeal"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.updIndexTArray, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRad2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updRad1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updLLeg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updTheta2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.updTheta1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSeal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuPrintForm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblAdj As System.Windows.Forms.Label
    Friend WithEvents lblStd As System.Windows.Forms.Label
    Friend WithEvents lblWid_New As System.Windows.Forms.Label
    Friend WithEvents lblWidStd As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblUSealAdjusted As System.Windows.Forms.Label
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents updTheta2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents updTheta1 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtTheta2_New As System.Windows.Forms.TextBox
    Public WithEvents txtTheta1_New As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtTheta2Std As System.Windows.Forms.TextBox
    Public WithEvents txtTheta1Std As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents txtPOrient As System.Windows.Forms.TextBox
    Public WithEvents txtDControl As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblHFree_Org As System.Windows.Forms.Label
    Friend WithEvents lblFreeHeight As System.Windows.Forms.Label
    Public WithEvents picSeal As System.Windows.Forms.PictureBox
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents updRad1 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRad1_New As System.Windows.Forms.TextBox
    Public WithEvents txtRad1Std As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents updLLeg As System.Windows.Forms.NumericUpDown
    Public WithEvents txtLLeg_New As System.Windows.Forms.TextBox
    Public WithEvents txtLLegStd As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents updRad2 As System.Windows.Forms.NumericUpDown
    Public WithEvents txtRad2_New As System.Windows.Forms.TextBox
    Public WithEvents txtRad2Std As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents updIndexTArray As System.Windows.Forms.NumericUpDown
    Public WithEvents txtT_New As System.Windows.Forms.TextBox
    Public WithEvents txtT As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents mnuOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdDXF As System.Windows.Forms.Button
    Public WithEvents lblErrMsg As System.Windows.Forms.Label
    Friend WithEvents mnuCreateWordDocu As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents txtCrossSecNo_New As System.Windows.Forms.TextBox
    Friend WithEvents lblCrossSecNew As System.Windows.Forms.Label
    Friend WithEvents lblCrossSecOrg As System.Windows.Forms.Label
    Friend WithEvents cmdViewNomenclature As System.Windows.Forms.Button
    Friend WithEvents lblHFree_New As System.Windows.Forms.Label
    Friend WithEvents PrintDocu As System.Drawing.Printing.PrintDocument
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
End Class
