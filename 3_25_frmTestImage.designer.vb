<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmImage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmImage))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpGroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdDown = New System.Windows.Forms.Button()
        Me.cmdUp = New System.Windows.Forms.Button()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.lstImageName = New System.Windows.Forms.ListBox()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.grpGroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCaption = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtNameTag = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.txtImageFileName = New System.Windows.Forms.TextBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        Me.grpGroupBox2.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpGroupBox1.SuspendLayout()
        Me.grpGroupBox3.SuspendLayout()
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
        Me.lblBorder.Size = New System.Drawing.Size(472, 458)
        Me.lblBorder.TabIndex = 6
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.grpGroupBox2)
        Me.Panel1.Controls.Add(Me.grpGroupBox1)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(470, 456)
        Me.Panel1.TabIndex = 7
        '
        'grpGroupBox2
        '
        Me.grpGroupBox2.Controls.Add(Me.Label2)
        Me.grpGroupBox2.Controls.Add(Me.cmdDelete)
        Me.grpGroupBox2.Controls.Add(Me.cmdAdd)
        Me.grpGroupBox2.Controls.Add(Me.cmdDown)
        Me.grpGroupBox2.Controls.Add(Me.cmdUp)
        Me.grpGroupBox2.Controls.Add(Me.picPreview)
        Me.grpGroupBox2.Controls.Add(Me.lstImageName)
        Me.grpGroupBox2.Location = New System.Drawing.Point(7, 180)
        Me.grpGroupBox2.Name = "grpGroupBox2"
        Me.grpGroupBox2.Size = New System.Drawing.Size(454, 231)
        Me.grpGroupBox2.TabIndex = 581
        Me.grpGroupBox2.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(322, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 613
        Me.Label2.Text = "Preview"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.Color.Wheat
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDelete.Location = New System.Drawing.Point(131, 19)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(72, 24)
        Me.cmdDelete.TabIndex = 606
        Me.cmdDelete.Tag = "Tester"
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.Color.Wheat
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAdd.Location = New System.Drawing.Point(9, 19)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(72, 24)
        Me.cmdAdd.TabIndex = 611
        Me.cmdAdd.Tag = "Tester"
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDown
        '
        Me.cmdDown.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cmdDown.BackColor = System.Drawing.Color.Silver
        Me.cmdDown.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDown.Image = CType(resources.GetObject("cmdDown.Image"), System.Drawing.Image)
        Me.cmdDown.Location = New System.Drawing.Point(210, 144)
        Me.cmdDown.Name = "cmdDown"
        Me.cmdDown.Size = New System.Drawing.Size(35, 30)
        Me.cmdDown.TabIndex = 368
        Me.cmdDown.Tag = "Tester"
        Me.cmdDown.UseVisualStyleBackColor = False
        '
        'cmdUp
        '
        Me.cmdUp.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cmdUp.BackColor = System.Drawing.Color.Silver
        Me.cmdUp.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUp.Image = CType(resources.GetObject("cmdUp.Image"), System.Drawing.Image)
        Me.cmdUp.Location = New System.Drawing.Point(210, 108)
        Me.cmdUp.Name = "cmdUp"
        Me.cmdUp.Size = New System.Drawing.Size(35, 30)
        Me.cmdUp.TabIndex = 367
        Me.cmdUp.Tag = "Tester"
        Me.cmdUp.UseVisualStyleBackColor = False
        '
        'picPreview
        '
        Me.picPreview.BackColor = System.Drawing.Color.White
        Me.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picPreview.Location = New System.Drawing.Point(251, 48)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(194, 173)
        Me.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picPreview.TabIndex = 1
        Me.picPreview.TabStop = False
        '
        'lstImageName
        '
        Me.lstImageName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstImageName.FormattingEnabled = True
        Me.lstImageName.Location = New System.Drawing.Point(9, 48)
        Me.lstImageName.Name = "lstImageName"
        Me.lstImageName.Size = New System.Drawing.Size(194, 173)
        Me.lstImageName.TabIndex = 0
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.grpGroupBox3)
        Me.grpGroupBox1.Controls.Add(Me.Label1)
        Me.grpGroupBox1.Controls.Add(Me.cmdBrowse)
        Me.grpGroupBox1.Controls.Add(Me.txtImageFileName)
        Me.grpGroupBox1.Location = New System.Drawing.Point(7, 7)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(450, 167)
        Me.grpGroupBox1.TabIndex = 580
        Me.grpGroupBox1.TabStop = False
        '
        'grpGroupBox3
        '
        Me.grpGroupBox3.Controls.Add(Me.Label3)
        Me.grpGroupBox3.Controls.Add(Me.txtCaption)
        Me.grpGroupBox3.Controls.Add(Me.Label19)
        Me.grpGroupBox3.Controls.Add(Me.txtNameTag)
        Me.grpGroupBox3.Location = New System.Drawing.Point(15, 62)
        Me.grpGroupBox3.Name = "grpGroupBox3"
        Me.grpGroupBox3.Size = New System.Drawing.Size(420, 92)
        Me.grpGroupBox3.TabIndex = 607
        Me.grpGroupBox3.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 613
        Me.Label3.Text = "Caption"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCaption
        '
        Me.txtCaption.AcceptsReturn = True
        Me.txtCaption.BackColor = System.Drawing.Color.White
        Me.txtCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaption.ForeColor = System.Drawing.Color.Black
        Me.txtCaption.Location = New System.Drawing.Point(77, 54)
        Me.txtCaption.MaxLength = 0
        Me.txtCaption.Name = "txtCaption"
        Me.txtCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaption.Size = New System.Drawing.Size(322, 21)
        Me.txtCaption.TabIndex = 612
        Me.txtCaption.Tag = "Tester"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(6, 23)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(65, 13)
        Me.Label19.TabIndex = 610
        Me.Label19.Text = "Name Tag"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNameTag
        '
        Me.txtNameTag.AcceptsReturn = True
        Me.txtNameTag.BackColor = System.Drawing.Color.White
        Me.txtNameTag.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNameTag.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNameTag.ForeColor = System.Drawing.Color.Black
        Me.txtNameTag.Location = New System.Drawing.Point(77, 19)
        Me.txtNameTag.MaxLength = 0
        Me.txtNameTag.Name = "txtNameTag"
        Me.txtNameTag.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNameTag.Size = New System.Drawing.Size(145, 21)
        Me.txtNameTag.TabIndex = 609
        Me.txtNameTag.Tag = "Tester"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 606
        Me.Label1.Text = "Image File:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdBrowse
        '
        Me.cmdBrowse.BackColor = System.Drawing.Color.Silver
        Me.cmdBrowse.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdBrowse.Location = New System.Drawing.Point(363, 32)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(72, 24)
        Me.cmdBrowse.TabIndex = 581
        Me.cmdBrowse.Tag = "Tester"
        Me.cmdBrowse.Text = "&Browse"
        Me.cmdBrowse.UseVisualStyleBackColor = False
        '
        'txtImageFileName
        '
        Me.txtImageFileName.AcceptsReturn = True
        Me.txtImageFileName.BackColor = System.Drawing.Color.White
        Me.txtImageFileName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtImageFileName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImageFileName.ForeColor = System.Drawing.Color.Black
        Me.txtImageFileName.Location = New System.Drawing.Point(15, 34)
        Me.txtImageFileName.MaxLength = 0
        Me.txtImageFileName.Name = "txtImageFileName"
        Me.txtImageFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtImageFileName.Size = New System.Drawing.Size(342, 21)
        Me.txtImageFileName.TabIndex = 579
        Me.txtImageFileName.Tag = "Tester"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(309, 422)
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
        Me.cmdCancel.Location = New System.Drawing.Point(389, 422)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'openFileDialog1
        '
        Me.openFileDialog1.FileName = "OpenFileDialog1"
        '
        'Test_frmImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 464)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Test_frmImage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Image Form"
        Me.Panel1.ResumeLayout(False)
        Me.grpGroupBox2.ResumeLayout(False)
        Me.grpGroupBox2.PerformLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.grpGroupBox3.ResumeLayout(False)
        Me.grpGroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Public WithEvents txtImageFileName As System.Windows.Forms.TextBox
    Friend WithEvents grpGroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents picPreview As System.Windows.Forms.PictureBox
    Friend WithEvents lstImageName As System.Windows.Forms.ListBox
    Private WithEvents cmdDown As System.Windows.Forms.Button
    Private WithEvents cmdUp As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpGroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents txtCaption As System.Windows.Forms.TextBox
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Public WithEvents txtNameTag As System.Windows.Forms.TextBox
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
