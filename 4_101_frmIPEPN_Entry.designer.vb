<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPE_frmPN_Entry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmPN_Entry))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.pnlPanel1 = New System.Windows.Forms.Panel()
        Me.cmbSealPOrient = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtOtherPN = New System.Windows.Forms.TextBox()
        Me.optOther = New System.Windows.Forms.RadioButton()
        Me.cmbCataloguedPN_Part7 = New System.Windows.Forms.ComboBox()
        Me.chkPlating = New System.Windows.Forms.CheckBox()
        Me.cmbCataloguedPN_Part6 = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbCataloguedPN_Part5 = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbCataloguedPN_Part4 = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbCataloguedPN_Part3 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCataloguedPN_Part2 = New System.Windows.Forms.MaskedTextBox()
        Me.cmbCataloguedPN_Part1 = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.optCatalogued = New System.Windows.Forms.RadioButton()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.pnlPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(1, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(538, 246)
        Me.lblBorder.TabIndex = 6
        Me.lblBorder.Text = "Label1"
        '
        'pnlPanel1
        '
        Me.pnlPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.pnlPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPanel1.Controls.Add(Me.cmbSealPOrient)
        Me.pnlPanel1.Controls.Add(Me.Label1)
        Me.pnlPanel1.Controls.Add(Me.GroupBox1)
        Me.pnlPanel1.Controls.Add(Me.cmdOK)
        Me.pnlPanel1.Controls.Add(Me.cmdCancel)
        Me.pnlPanel1.Location = New System.Drawing.Point(2, 4)
        Me.pnlPanel1.Name = "pnlPanel1"
        Me.pnlPanel1.Size = New System.Drawing.Size(536, 244)
        Me.pnlPanel1.TabIndex = 7
        '
        'cmbSealPOrient
        '
        Me.cmbSealPOrient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSealPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSealPOrient.Location = New System.Drawing.Point(94, 14)
        Me.cmbSealPOrient.Name = "cmbSealPOrient"
        Me.cmbSealPOrient.Size = New System.Drawing.Size(72, 21)
        Me.cmbSealPOrient.TabIndex = 681
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 680
        Me.Label1.Text = "Orientation"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtOtherPN)
        Me.GroupBox1.Controls.Add(Me.optOther)
        Me.GroupBox1.Controls.Add(Me.cmbCataloguedPN_Part7)
        Me.GroupBox1.Controls.Add(Me.chkPlating)
        Me.GroupBox1.Controls.Add(Me.cmbCataloguedPN_Part6)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cmbCataloguedPN_Part5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cmbCataloguedPN_Part4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cmbCataloguedPN_Part3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtCataloguedPN_Part2)
        Me.GroupBox1.Controls.Add(Me.cmbCataloguedPN_Part1)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.optCatalogued)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 43)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(516, 145)
        Me.GroupBox1.TabIndex = 658
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "P/N:"
        '
        'txtOtherPN
        '
        Me.txtOtherPN.BackColor = System.Drawing.Color.White
        Me.txtOtherPN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOtherPN.ForeColor = System.Drawing.Color.Black
        Me.txtOtherPN.Location = New System.Drawing.Point(207, 108)
        Me.txtOtherPN.Name = "txtOtherPN"
        Me.txtOtherPN.Size = New System.Drawing.Size(188, 21)
        Me.txtOtherPN.TabIndex = 677
        '
        'optOther
        '
        Me.optOther.AutoSize = True
        Me.optOther.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOther.Location = New System.Drawing.Point(13, 108)
        Me.optOther.Name = "optOther"
        Me.optOther.Size = New System.Drawing.Size(186, 17)
        Me.optOther.TabIndex = 676
        Me.optOther.Text = "Other Face Seal (C, S, E, U)"
        Me.optOther.UseVisualStyleBackColor = True
        '
        'cmbCataloguedPN_Part7
        '
        Me.cmbCataloguedPN_Part7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCataloguedPN_Part7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCataloguedPN_Part7.FormattingEnabled = True
        Me.cmbCataloguedPN_Part7.Items.AddRange(New Object() {"A", "B", "C", "D"})
        Me.cmbCataloguedPN_Part7.Location = New System.Drawing.Point(460, 60)
        Me.cmbCataloguedPN_Part7.Name = "cmbCataloguedPN_Part7"
        Me.cmbCataloguedPN_Part7.Size = New System.Drawing.Size(42, 21)
        Me.cmbCataloguedPN_Part7.TabIndex = 675
        '
        'chkPlating
        '
        Me.chkPlating.AutoSize = True
        Me.chkPlating.Location = New System.Drawing.Point(422, 42)
        Me.chkPlating.Name = "chkPlating"
        Me.chkPlating.Size = New System.Drawing.Size(61, 17)
        Me.chkPlating.TabIndex = 674
        Me.chkPlating.Text = " Plating"
        Me.chkPlating.UseVisualStyleBackColor = True
        '
        'cmbCataloguedPN_Part6
        '
        Me.cmbCataloguedPN_Part6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCataloguedPN_Part6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCataloguedPN_Part6.FormattingEnabled = True
        Me.cmbCataloguedPN_Part6.Items.AddRange(New Object() {"AP", "CP", "GP", "IP", "LP"})
        Me.cmbCataloguedPN_Part6.Location = New System.Drawing.Point(414, 60)
        Me.cmbCataloguedPN_Part6.Name = "cmbCataloguedPN_Part6"
        Me.cmbCataloguedPN_Part6.Size = New System.Drawing.Size(42, 21)
        Me.cmbCataloguedPN_Part6.TabIndex = 673
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(344, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 13)
        Me.Label6.TabIndex = 672
        Me.Label6.Text = "Temper"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbCataloguedPN_Part5
        '
        Me.cmbCataloguedPN_Part5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCataloguedPN_Part5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCataloguedPN_Part5.FormattingEnabled = True
        Me.cmbCataloguedPN_Part5.Items.AddRange(New Object() {"1", "2", "4", "6", "8"})
        Me.cmbCataloguedPN_Part5.Location = New System.Drawing.Point(349, 60)
        Me.cmbCataloguedPN_Part5.Name = "cmbCataloguedPN_Part5"
        Me.cmbCataloguedPN_Part5.Size = New System.Drawing.Size(42, 21)
        Me.cmbCataloguedPN_Part5.TabIndex = 671
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(275, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 670
        Me.Label4.Text = "Material"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbCataloguedPN_Part4
        '
        Me.cmbCataloguedPN_Part4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCataloguedPN_Part4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCataloguedPN_Part4.FormattingEnabled = True
        Me.cmbCataloguedPN_Part4.Items.AddRange(New Object() {"14", "29", "23"})
        Me.cmbCataloguedPN_Part4.Location = New System.Drawing.Point(280, 60)
        Me.cmbCataloguedPN_Part4.Name = "cmbCataloguedPN_Part4"
        Me.cmbCataloguedPN_Part4.Size = New System.Drawing.Size(42, 21)
        Me.cmbCataloguedPN_Part4.TabIndex = 669
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(185, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 668
        Me.Label3.Text = "Cross-Section"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbCataloguedPN_Part3
        '
        Me.cmbCataloguedPN_Part3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCataloguedPN_Part3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCataloguedPN_Part3.FormattingEnabled = True
        Me.cmbCataloguedPN_Part3.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"})
        Me.cmbCataloguedPN_Part3.Location = New System.Drawing.Point(207, 60)
        Me.cmbCataloguedPN_Part3.Name = "cmbCataloguedPN_Part3"
        Me.cmbCataloguedPN_Part3.Size = New System.Drawing.Size(42, 21)
        Me.cmbCataloguedPN_Part3.TabIndex = 667
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(88, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 666
        Me.Label2.Text = "Dia w/o Plating"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCataloguedPN_Part2
        '
        Me.txtCataloguedPN_Part2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCataloguedPN_Part2.Location = New System.Drawing.Point(104, 60)
        Me.txtCataloguedPN_Part2.Mask = "000000"
        Me.txtCataloguedPN_Part2.Name = "txtCataloguedPN_Part2"
        Me.txtCataloguedPN_Part2.PromptChar = Global.Microsoft.VisualBasic.ChrW(48)
        Me.txtCataloguedPN_Part2.Size = New System.Drawing.Size(61, 21)
        Me.txtCataloguedPN_Part2.TabIndex = 665
        Me.txtCataloguedPN_Part2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbCataloguedPN_Part1
        '
        Me.cmbCataloguedPN_Part1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCataloguedPN_Part1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCataloguedPN_Part1.FormattingEnabled = True
        Me.cmbCataloguedPN_Part1.Location = New System.Drawing.Point(29, 60)
        Me.cmbCataloguedPN_Part1.Name = "cmbCataloguedPN_Part1"
        Me.cmbCataloguedPN_Part1.Size = New System.Drawing.Size(52, 21)
        Me.cmbCataloguedPN_Part1.TabIndex = 664
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(35, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 663
        Me.Label5.Text = "Prefix"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'optCatalogued
        '
        Me.optCatalogued.AutoSize = True
        Me.optCatalogued.Checked = True
        Me.optCatalogued.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCatalogued.Location = New System.Drawing.Point(13, 18)
        Me.optCatalogued.Name = "optCatalogued"
        Me.optCatalogued.Size = New System.Drawing.Size(90, 17)
        Me.optCatalogued.TabIndex = 662
        Me.optCatalogued.TabStop = True
        Me.optCatalogued.Text = "Catalogued"
        Me.optCatalogued.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(376, 205)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 656
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
        Me.cmdCancel.Location = New System.Drawing.Point(454, 205)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 657
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'IPE_frmPN_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(541, 250)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlPanel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmPN_Entry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SealIPE: Legacy P/N Entry Form"
        Me.pnlPanel1.ResumeLayout(False)
        Me.pnlPanel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents pnlPanel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtOtherPN As System.Windows.Forms.TextBox
    Friend WithEvents optOther As System.Windows.Forms.RadioButton
    Friend WithEvents cmbCataloguedPN_Part7 As System.Windows.Forms.ComboBox
    Friend WithEvents chkPlating As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCataloguedPN_Part6 As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCataloguedPN_Part5 As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbCataloguedPN_Part4 As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbCataloguedPN_Part3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtCataloguedPN_Part2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents cmbCataloguedPN_Part1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents optCatalogued As System.Windows.Forms.RadioButton
    Friend WithEvents cmbSealPOrient As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
