<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmReport))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.txtDrawing = New System.Windows.Forms.TextBox()
        Me.grpImage = New System.Windows.Forms.GroupBox()
        Me.clbImage = New System.Windows.Forms.CheckedListBox()
        Me.grpReport = New System.Windows.Forms.GroupBox()
        Me.optRejection = New System.Windows.Forms.RadioButton()
        Me.optTest = New System.Windows.Forms.RadioButton()
        Me.cmdPDF = New System.Windows.Forms.Button()
        Me.cmdWORD = New System.Windows.Forms.Button()
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Panel1.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpImage.SuspendLayout()
        Me.grpReport.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(1, 2)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(389, 358)
        Me.lblBorder.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdAdd)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.picPreview)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmdBrowse)
        Me.Panel1.Controls.Add(Me.txtDrawing)
        Me.Panel1.Controls.Add(Me.grpImage)
        Me.Panel1.Controls.Add(Me.grpReport)
        Me.Panel1.Location = New System.Drawing.Point(2, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(387, 356)
        Me.Panel1.TabIndex = 4
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(299, 305)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 615
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.Color.Silver
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAdd.Location = New System.Drawing.Point(310, 203)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(61, 24)
        Me.cmdAdd.TabIndex = 614
        Me.cmdAdd.Tag = "Tester"
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(-3, 243)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(400, 2)
        Me.Label5.TabIndex = 613
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(274, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 612
        Me.Label2.Text = "Preview"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'picPreview
        '
        Me.picPreview.BackColor = System.Drawing.Color.White
        Me.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picPreview.Location = New System.Drawing.Point(230, 29)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(141, 139)
        Me.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picPreview.TabIndex = 611
        Me.picPreview.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 189)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 609
        Me.Label1.Text = "Drawing"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdBrowse
        '
        Me.cmdBrowse.BackColor = System.Drawing.Color.Silver
        Me.cmdBrowse.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdBrowse.Location = New System.Drawing.Point(243, 203)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(58, 24)
        Me.cmdBrowse.TabIndex = 608
        Me.cmdBrowse.Tag = "Tester"
        Me.cmdBrowse.Text = "&Browse"
        Me.cmdBrowse.UseVisualStyleBackColor = False
        '
        'txtDrawing
        '
        Me.txtDrawing.AcceptsReturn = True
        Me.txtDrawing.BackColor = System.Drawing.Color.White
        Me.txtDrawing.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDrawing.Enabled = False
        Me.txtDrawing.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDrawing.ForeColor = System.Drawing.Color.Black
        Me.txtDrawing.Location = New System.Drawing.Point(9, 205)
        Me.txtDrawing.MaxLength = 0
        Me.txtDrawing.Name = "txtDrawing"
        Me.txtDrawing.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDrawing.Size = New System.Drawing.Size(222, 21)
        Me.txtDrawing.TabIndex = 607
        Me.txtDrawing.Tag = "Tester"
        Me.txtDrawing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpImage
        '
        Me.grpImage.Controls.Add(Me.clbImage)
        Me.grpImage.Location = New System.Drawing.Point(9, 9)
        Me.grpImage.Name = "grpImage"
        Me.grpImage.Size = New System.Drawing.Size(212, 170)
        Me.grpImage.TabIndex = 76
        Me.grpImage.TabStop = False
        '
        'clbImage
        '
        Me.clbImage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clbImage.FormattingEnabled = True
        Me.clbImage.Location = New System.Drawing.Point(10, 20)
        Me.clbImage.Name = "clbImage"
        Me.clbImage.Size = New System.Drawing.Size(191, 132)
        Me.clbImage.TabIndex = 0
        Me.clbImage.Tag = "Tester"
        '
        'grpReport
        '
        Me.grpReport.Controls.Add(Me.optRejection)
        Me.grpReport.Controls.Add(Me.optTest)
        Me.grpReport.Controls.Add(Me.cmdPDF)
        Me.grpReport.Controls.Add(Me.cmdWORD)
        Me.grpReport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpReport.ForeColor = System.Drawing.Color.Red
        Me.grpReport.Location = New System.Drawing.Point(9, 255)
        Me.grpReport.Name = "grpReport"
        Me.grpReport.Size = New System.Drawing.Size(222, 88)
        Me.grpReport.TabIndex = 74
        Me.grpReport.TabStop = False
        Me.grpReport.Text = "Report:"
        '
        'optRejection
        '
        Me.optRejection.AutoSize = True
        Me.optRejection.ForeColor = System.Drawing.Color.Black
        Me.optRejection.Location = New System.Drawing.Point(101, 22)
        Me.optRejection.Name = "optRejection"
        Me.optRejection.Size = New System.Drawing.Size(78, 17)
        Me.optRejection.TabIndex = 616
        Me.optRejection.TabStop = True
        Me.optRejection.Text = "Rejection"
        Me.optRejection.UseVisualStyleBackColor = True
        '
        'optTest
        '
        Me.optTest.AutoSize = True
        Me.optTest.ForeColor = System.Drawing.Color.Black
        Me.optTest.Location = New System.Drawing.Point(22, 22)
        Me.optTest.Name = "optTest"
        Me.optTest.Size = New System.Drawing.Size(49, 17)
        Me.optTest.TabIndex = 5
        Me.optTest.TabStop = True
        Me.optTest.Text = "Test"
        Me.optTest.UseVisualStyleBackColor = True
        '
        'cmdPDF
        '
        Me.cmdPDF.BackColor = System.Drawing.Color.Wheat
        Me.cmdPDF.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPDF.ForeColor = System.Drawing.Color.Black
        Me.cmdPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPDF.Location = New System.Drawing.Point(121, 50)
        Me.cmdPDF.Name = "cmdPDF"
        Me.cmdPDF.Size = New System.Drawing.Size(80, 26)
        Me.cmdPDF.TabIndex = 4
        Me.cmdPDF.Text = "PDF"
        Me.cmdPDF.UseVisualStyleBackColor = False
        '
        'cmdWORD
        '
        Me.cmdWORD.BackColor = System.Drawing.Color.Wheat
        Me.cmdWORD.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWORD.ForeColor = System.Drawing.Color.Black
        Me.cmdWORD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdWORD.Location = New System.Drawing.Point(22, 50)
        Me.cmdWORD.Name = "cmdWORD"
        Me.cmdWORD.Size = New System.Drawing.Size(80, 26)
        Me.cmdWORD.TabIndex = 3
        Me.cmdWORD.Text = "&WORD"
        Me.cmdWORD.UseVisualStyleBackColor = False
        '
        'openFileDialog1
        '
        Me.openFileDialog1.FileName = "OpenFileDialog1"
        '
        'Test_frmReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 362)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Test_frmReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Report Form"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpImage.ResumeLayout(False)
        Me.grpReport.ResumeLayout(False)
        Me.grpReport.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents picPreview As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Public WithEvents txtDrawing As System.Windows.Forms.TextBox
    Friend WithEvents grpImage As System.Windows.Forms.GroupBox
    Friend WithEvents clbImage As System.Windows.Forms.CheckedListBox
    Friend WithEvents grpReport As System.Windows.Forms.GroupBox
    Friend WithEvents cmdPDF As System.Windows.Forms.Button
    Friend WithEvents cmdWORD As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents openFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents saveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents optRejection As System.Windows.Forms.RadioButton
    Friend WithEvents optTest As System.Windows.Forms.RadioButton
End Class
