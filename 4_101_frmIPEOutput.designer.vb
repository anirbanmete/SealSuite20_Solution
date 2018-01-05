<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPE_frmOutput
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmOutput))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkCavityInfo = New System.Windows.Forms.CheckBox()
        Me.cmdPreProduction = New System.Windows.Forms.Button()
        Me.cmdCustomer = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpGroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtAnaDesc = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdCust_PPT = New System.Windows.Forms.Button()
        Me.cmdPDF = New System.Windows.Forms.Button()
        Me.cmdWORD = New System.Windows.Forms.Button()
        Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.grpGroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.grpGroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(2, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(422, 340)
        Me.lblBorder.TabIndex = 2
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(337, 303)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.chkCavityInfo)
        Me.grpGroupBox1.Controls.Add(Me.cmdPreProduction)
        Me.grpGroupBox1.Controls.Add(Me.cmdCustomer)
        Me.grpGroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGroupBox1.Location = New System.Drawing.Point(15, 72)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(394, 98)
        Me.grpGroupBox1.TabIndex = 33
        Me.grpGroupBox1.TabStop = False
        Me.grpGroupBox1.Text = "Drawings:"
        '
        'chkCavityInfo
        '
        Me.chkCavityInfo.AutoSize = True
        Me.chkCavityInfo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCavityInfo.Location = New System.Drawing.Point(22, 28)
        Me.chkCavityInfo.Name = "chkCavityInfo"
        Me.chkCavityInfo.Size = New System.Drawing.Size(134, 17)
        Me.chkCavityInfo.TabIndex = 70
        Me.chkCavityInfo.Text = "Cavity Information"
        '
        'cmdPreProduction
        '
        Me.cmdPreProduction.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdPreProduction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPreProduction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPreProduction.Location = New System.Drawing.Point(259, 58)
        Me.cmdPreProduction.Name = "cmdPreProduction"
        Me.cmdPreProduction.Size = New System.Drawing.Size(113, 26)
        Me.cmdPreProduction.TabIndex = 4
        Me.cmdPreProduction.Text = "&Pre-Production"
        Me.cmdPreProduction.UseVisualStyleBackColor = False
        '
        'cmdCustomer
        '
        Me.cmdCustomer.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdCustomer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCustomer.Location = New System.Drawing.Point(19, 58)
        Me.cmdCustomer.Name = "cmdCustomer"
        Me.cmdCustomer.Size = New System.Drawing.Size(113, 26)
        Me.cmdCustomer.TabIndex = 3
        Me.cmdCustomer.Text = "&Customer"
        Me.cmdCustomer.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.grpGroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.grpGroupBox1)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Location = New System.Drawing.Point(3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(420, 338)
        Me.Panel1.TabIndex = 31
        '
        'grpGroupBox2
        '
        Me.grpGroupBox2.Controls.Add(Me.txtAnaDesc)
        Me.grpGroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGroupBox2.Location = New System.Drawing.Point(16, 12)
        Me.grpGroupBox2.Name = "grpGroupBox2"
        Me.grpGroupBox2.Size = New System.Drawing.Size(393, 54)
        Me.grpGroupBox2.TabIndex = 36
        Me.grpGroupBox2.TabStop = False
        Me.grpGroupBox2.Text = "Analysis Desc.:"
        '
        'txtAnaDesc
        '
        Me.txtAnaDesc.AcceptsReturn = True
        Me.txtAnaDesc.BackColor = System.Drawing.Color.LightGray
        Me.txtAnaDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAnaDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnaDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAnaDesc.Location = New System.Drawing.Point(6, 23)
        Me.txtAnaDesc.MaxLength = 0
        Me.txtAnaDesc.Name = "txtAnaDesc"
        Me.txtAnaDesc.ReadOnly = True
        Me.txtAnaDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAnaDesc.Size = New System.Drawing.Size(381, 21)
        Me.txtAnaDesc.TabIndex = 36
        Me.txtAnaDesc.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdCust_PPT)
        Me.GroupBox1.Controls.Add(Me.cmdPDF)
        Me.GroupBox1.Controls.Add(Me.cmdWORD)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(15, 183)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(394, 106)
        Me.GroupBox1.TabIndex = 34
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Reports:"
        '
        'cmdCust_PPT
        '
        Me.cmdCust_PPT.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdCust_PPT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCust_PPT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCust_PPT.Location = New System.Drawing.Point(10, 67)
        Me.cmdCust_PPT.Name = "cmdCust_PPT"
        Me.cmdCust_PPT.Size = New System.Drawing.Size(156, 26)
        Me.cmdCust_PPT.TabIndex = 5
        Me.cmdCust_PPT.Text = "Customer Power Point"
        Me.cmdCust_PPT.UseVisualStyleBackColor = False
        '
        'cmdPDF
        '
        Me.cmdPDF.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdPDF.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPDF.Location = New System.Drawing.Point(259, 26)
        Me.cmdPDF.Name = "cmdPDF"
        Me.cmdPDF.Size = New System.Drawing.Size(113, 26)
        Me.cmdPDF.TabIndex = 4
        Me.cmdPDF.Text = "PDF"
        Me.cmdPDF.UseVisualStyleBackColor = False
        '
        'cmdWORD
        '
        Me.cmdWORD.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdWORD.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWORD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdWORD.Location = New System.Drawing.Point(10, 26)
        Me.cmdWORD.Name = "cmdWORD"
        Me.cmdWORD.Size = New System.Drawing.Size(113, 26)
        Me.cmdWORD.TabIndex = 3
        Me.cmdWORD.Text = "&WORD"
        Me.cmdWORD.UseVisualStyleBackColor = False
        '
        'IPE_frmOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(425, 344)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmOutput"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Drawings & Reports Output"
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.grpGroupBox2.ResumeLayout(False)
        Me.grpGroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdPreProduction As System.Windows.Forms.Button
    Friend WithEvents cmdCustomer As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdPDF As System.Windows.Forms.Button
    Friend WithEvents cmdWORD As System.Windows.Forms.Button
    Friend WithEvents cmdCust_PPT As System.Windows.Forms.Button
    Public WithEvents txtAnaDesc As System.Windows.Forms.TextBox
    Friend WithEvents chkCavityInfo As System.Windows.Forms.CheckBox
    Friend WithEvents saveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents grpGroupBox2 As System.Windows.Forms.GroupBox
End Class
