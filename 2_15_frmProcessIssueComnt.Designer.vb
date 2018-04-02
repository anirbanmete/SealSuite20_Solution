<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Process_frmIssueComnt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Process_frmIssueComnt))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.pnlPanel1 = New System.Windows.Forms.Panel()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblDeptName = New System.Windows.Forms.Label()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbDeptName = New System.Windows.Forms.ComboBox()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.pnlPanel1.SuspendLayout()
        Me.grpGroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBorder.Location = New System.Drawing.Point(1, 1)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(472, 234)
        Me.lblBorder.TabIndex = 1
        '
        'pnlPanel1
        '
        Me.pnlPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPanel1.Controls.Add(Me.cmdOK)
        Me.pnlPanel1.Controls.Add(Me.grpGroupBox1)
        Me.pnlPanel1.Controls.Add(Me.cmdCancel)
        Me.pnlPanel1.Location = New System.Drawing.Point(2, 2)
        Me.pnlPanel1.Name = "pnlPanel1"
        Me.pnlPanel1.Size = New System.Drawing.Size(470, 232)
        Me.pnlPanel1.TabIndex = 2
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(290, 197)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 762
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.lblDeptName)
        Me.grpGroupBox1.Controls.Add(Me.lblUserName)
        Me.grpGroupBox1.Controls.Add(Me.Label1)
        Me.grpGroupBox1.Controls.Add(Me.cmbDeptName)
        Me.grpGroupBox1.Controls.Add(Me.Label105)
        Me.grpGroupBox1.Controls.Add(Me.txtComments)
        Me.grpGroupBox1.Controls.Add(Me.Label23)
        Me.grpGroupBox1.Location = New System.Drawing.Point(19, 3)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(428, 177)
        Me.grpGroupBox1.TabIndex = 0
        Me.grpGroupBox1.TabStop = False
        '
        'lblDeptName
        '
        Me.lblDeptName.AutoSize = True
        Me.lblDeptName.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblDeptName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeptName.ForeColor = System.Drawing.Color.Blue
        Me.lblDeptName.Location = New System.Drawing.Point(195, 37)
        Me.lblDeptName.Name = "lblDeptName"
        Me.lblDeptName.Size = New System.Drawing.Size(34, 13)
        Me.lblDeptName.TabIndex = 764
        Me.lblDeptName.Text = "Dept"
        Me.lblDeptName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUserName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.ForeColor = System.Drawing.Color.Blue
        Me.lblUserName.Location = New System.Drawing.Point(19, 37)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(40, 13)
        Me.lblUserName.TabIndex = 763
        Me.lblUserName.Text = "Name"
        Me.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 762
        Me.Label1.Text = "User Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbDeptName
        '
        Me.cmbDeptName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDeptName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDeptName.FormattingEnabled = True
        Me.cmbDeptName.Items.AddRange(New Object() {"Custom", "Design Guide"})
        Me.cmbDeptName.Location = New System.Drawing.Point(197, 34)
        Me.cmbDeptName.Name = "cmbDeptName"
        Me.cmbDeptName.Size = New System.Drawing.Size(145, 21)
        Me.cmbDeptName.TabIndex = 761
        '
        'Label105
        '
        Me.Label105.AutoSize = True
        Me.Label105.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label105.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label105.Location = New System.Drawing.Point(195, 17)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(34, 13)
        Me.Label105.TabIndex = 760
        Me.Label105.Text = "Dept"
        Me.Label105.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtComments
        '
        Me.txtComments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.Color.Black
        Me.txtComments.Location = New System.Drawing.Point(19, 83)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(390, 82)
        Me.txtComments.TabIndex = 759
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label23.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(19, 67)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(74, 13)
        Me.Label23.TabIndex = 758
        Me.Label23.Text = "Comments:"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(375, 197)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 763
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Process_frmIssueComnt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(475, 236)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlPanel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Process_frmIssueComnt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SealProcess: Issue/Comment"
        Me.pnlPanel1.ResumeLayout(False)
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents pnlPanel1 As System.Windows.Forms.Panel
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents cmbDeptName As System.Windows.Forms.ComboBox
    Friend WithEvents Label105 As System.Windows.Forms.Label
    Private WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblUserName As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblDeptName As Label
End Class
