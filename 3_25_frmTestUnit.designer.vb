<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmUnit
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmUnit))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbPUnitCust = New System.Windows.Forms.ComboBox()
        Me.cmbFUnitCust = New System.Windows.Forms.ComboBox()
        Me.cmbPUnitPH = New System.Windows.Forms.ComboBox()
        Me.cmbFUnitPH = New System.Windows.Forms.ComboBox()
        Me.cmbLUnitCust = New System.Windows.Forms.ComboBox()
        Me.cmbLUnitPH = New System.Windows.Forms.ComboBox()
        Me.lblCust = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbLeakUnitCust = New System.Windows.Forms.ComboBox()
        Me.cmbLeakUnitPH = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
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
        Me.lblBorder.Size = New System.Drawing.Size(272, 189)
        Me.lblBorder.TabIndex = 3
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmbPUnitCust)
        Me.Panel1.Controls.Add(Me.cmbFUnitCust)
        Me.Panel1.Controls.Add(Me.cmbPUnitPH)
        Me.Panel1.Controls.Add(Me.cmbFUnitPH)
        Me.Panel1.Controls.Add(Me.cmbLUnitCust)
        Me.Panel1.Controls.Add(Me.cmbLUnitPH)
        Me.Panel1.Controls.Add(Me.lblCust)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.cmbLeakUnitCust)
        Me.Panel1.Controls.Add(Me.cmbLeakUnitPH)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(270, 187)
        Me.Panel1.TabIndex = 4
        '
        'cmbPUnitCust
        '
        Me.cmbPUnitCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPUnitCust.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPUnitCust.FormattingEnabled = True
        Me.cmbPUnitCust.Items.AddRange(New Object() {"kPa"})
        Me.cmbPUnitCust.Location = New System.Drawing.Point(171, 85)
        Me.cmbPUnitCust.Name = "cmbPUnitCust"
        Me.cmbPUnitCust.Size = New System.Drawing.Size(90, 21)
        Me.cmbPUnitCust.TabIndex = 644
        '
        'cmbFUnitCust
        '
        Me.cmbFUnitCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFUnitCust.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFUnitCust.FormattingEnabled = True
        Me.cmbFUnitCust.Items.AddRange(New Object() {"N"})
        Me.cmbFUnitCust.Location = New System.Drawing.Point(171, 56)
        Me.cmbFUnitCust.Name = "cmbFUnitCust"
        Me.cmbFUnitCust.Size = New System.Drawing.Size(90, 21)
        Me.cmbFUnitCust.TabIndex = 643
        '
        'cmbPUnitPH
        '
        Me.cmbPUnitPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPUnitPH.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPUnitPH.FormattingEnabled = True
        Me.cmbPUnitPH.Items.AddRange(New Object() {"psi"})
        Me.cmbPUnitPH.Location = New System.Drawing.Point(53, 85)
        Me.cmbPUnitPH.Name = "cmbPUnitPH"
        Me.cmbPUnitPH.Size = New System.Drawing.Size(90, 21)
        Me.cmbPUnitPH.TabIndex = 642
        '
        'cmbFUnitPH
        '
        Me.cmbFUnitPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFUnitPH.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFUnitPH.FormattingEnabled = True
        Me.cmbFUnitPH.Items.AddRange(New Object() {"lbf"})
        Me.cmbFUnitPH.Location = New System.Drawing.Point(53, 56)
        Me.cmbFUnitPH.Name = "cmbFUnitPH"
        Me.cmbFUnitPH.Size = New System.Drawing.Size(90, 21)
        Me.cmbFUnitPH.TabIndex = 641
        '
        'cmbLUnitCust
        '
        Me.cmbLUnitCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLUnitCust.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLUnitCust.FormattingEnabled = True
        Me.cmbLUnitCust.Items.AddRange(New Object() {"mm"})
        Me.cmbLUnitCust.Location = New System.Drawing.Point(171, 27)
        Me.cmbLUnitCust.Name = "cmbLUnitCust"
        Me.cmbLUnitCust.Size = New System.Drawing.Size(90, 21)
        Me.cmbLUnitCust.TabIndex = 640
        '
        'cmbLUnitPH
        '
        Me.cmbLUnitPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLUnitPH.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLUnitPH.FormattingEnabled = True
        Me.cmbLUnitPH.Items.AddRange(New Object() {"in"})
        Me.cmbLUnitPH.Location = New System.Drawing.Point(53, 27)
        Me.cmbLUnitPH.Name = "cmbLUnitPH"
        Me.cmbLUnitPH.Size = New System.Drawing.Size(90, 21)
        Me.cmbLUnitPH.TabIndex = 639
        '
        'lblCust
        '
        Me.lblCust.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCust.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCust.Location = New System.Drawing.Point(182, 10)
        Me.lblCust.Name = "lblCust"
        Me.lblCust.Size = New System.Drawing.Size(69, 13)
        Me.lblCust.TabIndex = 638
        Me.lblCust.Text = "Customer"
        Me.lblCust.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(73, 10)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(51, 13)
        Me.Label10.TabIndex = 637
        Me.Label10.Text = "Parker"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbLeakUnitCust
        '
        Me.cmbLeakUnitCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLeakUnitCust.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLeakUnitCust.FormattingEnabled = True
        Me.cmbLeakUnitCust.Items.AddRange(New Object() {"cc/S"})
        Me.cmbLeakUnitCust.Location = New System.Drawing.Point(171, 114)
        Me.cmbLeakUnitCust.Name = "cmbLeakUnitCust"
        Me.cmbLeakUnitCust.Size = New System.Drawing.Size(90, 21)
        Me.cmbLeakUnitCust.TabIndex = 636
        '
        'cmbLeakUnitPH
        '
        Me.cmbLeakUnitPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLeakUnitPH.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLeakUnitPH.FormattingEnabled = True
        Me.cmbLeakUnitPH.Items.AddRange(New Object() {"mL/m"})
        Me.cmbLeakUnitPH.Location = New System.Drawing.Point(53, 114)
        Me.cmbLeakUnitPH.Name = "cmbLeakUnitPH"
        Me.cmbLeakUnitPH.Size = New System.Drawing.Size(90, 21)
        Me.cmbLeakUnitPH.TabIndex = 635
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(10, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 634
        Me.Label3.Text = "Leak"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(30, 89)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(14, 13)
        Me.Label2.TabIndex = 633
        Me.Label2.Text = "P"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(31, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 632
        Me.Label1.Text = "L"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(31, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(13, 13)
        Me.Label6.TabIndex = 631
        Me.Label6.Text = "F"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(120, 155)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(66, 24)
        Me.cmdOK.TabIndex = 25
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
        Me.cmdCancel.Location = New System.Drawing.Point(195, 155)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(66, 24)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "   &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Test_frmUnit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(278, 195)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MinimizeBox = False
        Me.Name = "Test_frmUnit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set Units"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents cmbPUnitCust As System.Windows.Forms.ComboBox
    Private WithEvents cmbFUnitCust As System.Windows.Forms.ComboBox
    Private WithEvents cmbPUnitPH As System.Windows.Forms.ComboBox
    Private WithEvents cmbFUnitPH As System.Windows.Forms.ComboBox
    Private WithEvents cmbLUnitCust As System.Windows.Forms.ComboBox
    Private WithEvents cmbLUnitPH As System.Windows.Forms.ComboBox
    Friend WithEvents lblCust As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Private WithEvents cmbLeakUnitCust As System.Windows.Forms.ComboBox
    Private WithEvents cmbLeakUnitPH As System.Windows.Forms.ComboBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
End Class
