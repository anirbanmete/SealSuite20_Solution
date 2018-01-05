<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmNotes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmNotes))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.lblNotes = New System.Windows.Forms.Label()
        Me.chkOverride = New System.Windows.Forms.CheckBox()
        Me.grpStatus = New System.Windows.Forms.GroupBox()
        Me.optPass = New System.Windows.Forms.RadioButton()
        Me.optFail = New System.Windows.Forms.RadioButton()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.ttpToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.panel1.SuspendLayout()
        Me.grpStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(1, 1)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(452, 382)
        Me.lblBorder.TabIndex = 9
        '
        'panel1
        '
        Me.panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panel1.Controls.Add(Me.lblNotes)
        Me.panel1.Controls.Add(Me.chkOverride)
        Me.panel1.Controls.Add(Me.grpStatus)
        Me.panel1.Controls.Add(Me.cmdCancel)
        Me.panel1.Controls.Add(Me.txtNotes)
        Me.panel1.Controls.Add(Me.cmdOK)
        Me.panel1.Location = New System.Drawing.Point(2, 2)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(450, 380)
        Me.panel1.TabIndex = 10
        '
        'lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotes.Location = New System.Drawing.Point(16, 78)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(44, 13)
        Me.lblNotes.TabIndex = 600
        Me.lblNotes.Text = "Notes:"
        '
        'chkOverride
        '
        Me.chkOverride.AutoSize = True
        Me.chkOverride.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverride.Location = New System.Drawing.Point(201, 36)
        Me.chkOverride.Name = "chkOverride"
        Me.chkOverride.Size = New System.Drawing.Size(76, 17)
        Me.chkOverride.TabIndex = 599
        Me.chkOverride.Tag = "Tester, Engg, Quality"
        Me.chkOverride.Text = "Override"
        Me.chkOverride.UseVisualStyleBackColor = True
        '
        'grpStatus
        '
        Me.grpStatus.Controls.Add(Me.optPass)
        Me.grpStatus.Controls.Add(Me.optFail)
        Me.grpStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpStatus.ForeColor = System.Drawing.Color.Black
        Me.grpStatus.Location = New System.Drawing.Point(16, 13)
        Me.grpStatus.Name = "grpStatus"
        Me.grpStatus.Size = New System.Drawing.Size(154, 48)
        Me.grpStatus.TabIndex = 598
        Me.grpStatus.TabStop = False
        Me.grpStatus.Text = "Overall Test Status:"
        '
        'optPass
        '
        Me.optPass.AutoSize = True
        Me.optPass.Checked = True
        Me.optPass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPass.Location = New System.Drawing.Point(13, 23)
        Me.optPass.Name = "optPass"
        Me.optPass.Size = New System.Drawing.Size(56, 17)
        Me.optPass.TabIndex = 3
        Me.optPass.TabStop = True
        Me.optPass.Tag = "Tester, Engg, Quality"
        Me.optPass.Text = "PASS"
        Me.optPass.UseVisualStyleBackColor = True
        '
        'optFail
        '
        Me.optFail.AutoSize = True
        Me.optFail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optFail.Location = New System.Drawing.Point(87, 23)
        Me.optFail.Name = "optFail"
        Me.optFail.Size = New System.Drawing.Size(50, 17)
        Me.optFail.TabIndex = 2
        Me.optFail.Tag = "Tester, Engg, Quality"
        Me.optFail.Text = "FAIL"
        Me.optFail.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(354, 342)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 30)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = " &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtNotes
        '
        Me.txtNotes.BackColor = System.Drawing.Color.White
        Me.txtNotes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(16, 97)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtNotes.Size = New System.Drawing.Size(418, 232)
        Me.txtNotes.TabIndex = 15
        Me.txtNotes.Tag = "Tester, Engg, Quality"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(258, 342)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 30)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'ttpToolTip1
        '
        Me.ttpToolTip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ttpToolTip1.ForeColor = System.Drawing.Color.Black
        Me.ttpToolTip1.OwnerDraw = True
        '
        'Test_frmNotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 385)
        Me.ControlBox = False
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Test_frmNotes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Notes Form"
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        Me.grpStatus.ResumeLayout(False)
        Me.grpStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents lblBorder As System.Windows.Forms.Label
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents lblNotes As System.Windows.Forms.Label
    Private WithEvents chkOverride As System.Windows.Forms.CheckBox
    Private WithEvents grpStatus As System.Windows.Forms.GroupBox
    Private WithEvents optPass As System.Windows.Forms.RadioButton
    Private WithEvents optFail As System.Windows.Forms.RadioButton
    Private WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents txtNotes As System.Windows.Forms.TextBox
    Private WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents ttpToolTip1 As System.Windows.Forms.ToolTip
End Class
