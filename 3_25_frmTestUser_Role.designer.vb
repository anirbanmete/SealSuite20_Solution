<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmUser_Role
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmUser_Role))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.optViewer = New System.Windows.Forms.RadioButton()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.grpgroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pnlpanel1 = New System.Windows.Forms.Panel()
        Me.optSignoff = New System.Windows.Forms.RadioButton()
        Me.optAdmin = New System.Windows.Forms.RadioButton()
        Me.optQuality = New System.Windows.Forms.RadioButton()
        Me.optEngg = New System.Windows.Forms.RadioButton()
        Me.optTester = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.pnlView.SuspendLayout()
        Me.grpgroupBox1.SuspendLayout()
        Me.pnlpanel1.SuspendLayout()
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
        Me.lblBorder.Size = New System.Drawing.Size(427, 170)
        Me.lblBorder.TabIndex = 9
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.pnlView)
        Me.Panel1.Controls.Add(Me.lblName)
        Me.Panel1.Controls.Add(Me.lbl1)
        Me.Panel1.Controls.Add(Me.grpgroupBox1)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(425, 168)
        Me.Panel1.TabIndex = 10
        '
        'pnlView
        '
        Me.pnlView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlView.Controls.Add(Me.optViewer)
        Me.pnlView.Location = New System.Drawing.Point(19, 126)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(86, 32)
        Me.pnlView.TabIndex = 31
        '
        'optViewer
        '
        Me.optViewer.AutoSize = True
        Me.optViewer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optViewer.Location = New System.Drawing.Point(10, 7)
        Me.optViewer.Name = "optViewer"
        Me.optViewer.Size = New System.Drawing.Size(64, 17)
        Me.optViewer.TabIndex = 4
        Me.optViewer.Text = "Viewer"
        Me.optViewer.UseVisualStyleBackColor = True
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(70, 14)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(83, 13)
        Me.lblName.TabIndex = 30
        Me.lblName.Text = "Kermit Bierut"
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.Location = New System.Drawing.Point(16, 14)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(48, 13)
        Me.lbl1.TabIndex = 29
        Me.lbl1.Text = "Name:"
        '
        'grpgroupBox1
        '
        Me.grpgroupBox1.Controls.Add(Me.pnlpanel1)
        Me.grpgroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpgroupBox1.Location = New System.Drawing.Point(9, 45)
        Me.grpgroupBox1.Name = "grpgroupBox1"
        Me.grpgroupBox1.Size = New System.Drawing.Size(406, 71)
        Me.grpgroupBox1.TabIndex = 28
        Me.grpgroupBox1.TabStop = False
        Me.grpgroupBox1.Text = "Please select your current role:"
        '
        'pnlpanel1
        '
        Me.pnlpanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlpanel1.Controls.Add(Me.optSignoff)
        Me.pnlpanel1.Controls.Add(Me.optAdmin)
        Me.pnlpanel1.Controls.Add(Me.optQuality)
        Me.pnlpanel1.Controls.Add(Me.optEngg)
        Me.pnlpanel1.Controls.Add(Me.optTester)
        Me.pnlpanel1.Controls.Add(Me.Label1)
        Me.pnlpanel1.Location = New System.Drawing.Point(10, 24)
        Me.pnlpanel1.Name = "pnlpanel1"
        Me.pnlpanel1.Size = New System.Drawing.Size(386, 35)
        Me.pnlpanel1.TabIndex = 1
        '
        'optSignoff
        '
        Me.optSignoff.AutoSize = True
        Me.optSignoff.BackColor = System.Drawing.Color.Silver
        Me.optSignoff.Enabled = False
        Me.optSignoff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSignoff.Location = New System.Drawing.Point(77, 8)
        Me.optSignoff.Name = "optSignoff"
        Me.optSignoff.Size = New System.Drawing.Size(70, 17)
        Me.optSignoff.TabIndex = 7
        Me.optSignoff.Text = "Sign-off"
        Me.optSignoff.UseVisualStyleBackColor = False
        Me.optSignoff.Visible = False
        '
        'optAdmin
        '
        Me.optAdmin.AutoSize = True
        Me.optAdmin.BackColor = System.Drawing.Color.Silver
        Me.optAdmin.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAdmin.Location = New System.Drawing.Point(10, 8)
        Me.optAdmin.Name = "optAdmin"
        Me.optAdmin.Size = New System.Drawing.Size(61, 17)
        Me.optAdmin.TabIndex = 6
        Me.optAdmin.Text = "Admin"
        Me.optAdmin.UseVisualStyleBackColor = False
        '
        'optQuality
        '
        Me.optQuality.AutoSize = True
        Me.optQuality.Enabled = False
        Me.optQuality.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optQuality.Location = New System.Drawing.Point(307, 8)
        Me.optQuality.Name = "optQuality"
        Me.optQuality.Size = New System.Drawing.Size(65, 17)
        Me.optQuality.TabIndex = 5
        Me.optQuality.Text = "Quality"
        Me.optQuality.UseVisualStyleBackColor = True
        '
        'optEngg
        '
        Me.optEngg.AutoSize = True
        Me.optEngg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optEngg.Location = New System.Drawing.Point(243, 8)
        Me.optEngg.Name = "optEngg"
        Me.optEngg.Size = New System.Drawing.Size(50, 17)
        Me.optEngg.TabIndex = 4
        Me.optEngg.Text = "Eng."
        Me.optEngg.UseVisualStyleBackColor = True
        '
        'optTester
        '
        Me.optTester.AutoSize = True
        Me.optTester.Checked = True
        Me.optTester.Enabled = False
        Me.optTester.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTester.Location = New System.Drawing.Point(175, 8)
        Me.optTester.Name = "optTester"
        Me.optTester.Size = New System.Drawing.Size(61, 17)
        Me.optTester.TabIndex = 3
        Me.optTester.TabStop = True
        Me.optTester.Text = "Tester"
        Me.optTester.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Silver
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(147, 25)
        Me.Label1.TabIndex = 31
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(263, 130)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
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
        Me.cmdCancel.Location = New System.Drawing.Point(343, 130)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Test_frmUser_Role
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 176)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Test_frmUser_Role"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Role Selection"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlView.ResumeLayout(False)
        Me.pnlView.PerformLayout()
        Me.grpgroupBox1.ResumeLayout(False)
        Me.pnlpanel1.ResumeLayout(False)
        Me.pnlpanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Private WithEvents lblName As System.Windows.Forms.Label
    Private WithEvents lbl1 As System.Windows.Forms.Label
    Private WithEvents grpgroupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents pnlpanel1 As System.Windows.Forms.Panel
    Private WithEvents optQuality As System.Windows.Forms.RadioButton
    Private WithEvents optEngg As System.Windows.Forms.RadioButton
    Private WithEvents optTester As System.Windows.Forms.RadioButton
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents optSignoff As System.Windows.Forms.RadioButton
    Private WithEvents optAdmin As System.Windows.Forms.RadioButton
    Private WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Private WithEvents optViewer As System.Windows.Forms.RadioButton
End Class
