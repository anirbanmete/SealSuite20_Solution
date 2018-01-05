<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmGen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmGen))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pnlPanel2 = New System.Windows.Forms.Panel()
        Me.pnlNav = New System.Windows.Forms.Panel()
        Me.cmdMoveLast = New System.Windows.Forms.Button()
        Me.cmdMoveFirst = New System.Windows.Forms.Button()
        Me.cmdMovePrev = New System.Windows.Forms.Button()
        Me.lblTotalRec = New System.Windows.Forms.Label()
        Me.cmdMoveNext = New System.Windows.Forms.Button()
        Me.txtRecNo = New System.Windows.Forms.TextBox()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSealSN = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSealID = New System.Windows.Forms.TextBox()
        Me.txtMatThick = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.pnlPanel2.SuspendLayout()
        Me.pnlNav.SuspendLayout()
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
        Me.lblBorder.Size = New System.Drawing.Size(282, 192)
        Me.lblBorder.TabIndex = 2
        Me.lblBorder.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.pnlPanel2)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(280, 190)
        Me.Panel1.TabIndex = 3
        '
        'pnlPanel2
        '
        Me.pnlPanel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.pnlPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPanel2.Controls.Add(Me.pnlNav)
        Me.pnlPanel2.Controls.Add(Me.cmdAdd)
        Me.pnlPanel2.Controls.Add(Me.Label1)
        Me.pnlPanel2.Controls.Add(Me.txtSealSN)
        Me.pnlPanel2.Controls.Add(Me.Label6)
        Me.pnlPanel2.Controls.Add(Me.txtSealID)
        Me.pnlPanel2.Controls.Add(Me.txtMatThick)
        Me.pnlPanel2.Controls.Add(Me.Label9)
        Me.pnlPanel2.Controls.Add(Me.Label10)
        Me.pnlPanel2.Location = New System.Drawing.Point(9, 7)
        Me.pnlPanel2.Name = "pnlPanel2"
        Me.pnlPanel2.Size = New System.Drawing.Size(260, 137)
        Me.pnlPanel2.TabIndex = 605
        '
        'pnlNav
        '
        Me.pnlNav.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlNav.Controls.Add(Me.cmdMoveLast)
        Me.pnlNav.Controls.Add(Me.cmdMoveFirst)
        Me.pnlNav.Controls.Add(Me.cmdMovePrev)
        Me.pnlNav.Controls.Add(Me.lblTotalRec)
        Me.pnlNav.Controls.Add(Me.cmdMoveNext)
        Me.pnlNav.Controls.Add(Me.txtRecNo)
        Me.pnlNav.Location = New System.Drawing.Point(49, 110)
        Me.pnlNav.Name = "pnlNav"
        Me.pnlNav.Size = New System.Drawing.Size(210, 23)
        Me.pnlNav.TabIndex = 609
        '
        'cmdMoveLast
        '
        Me.cmdMoveLast.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveLast.ForeColor = System.Drawing.Color.RoyalBlue
        Me.cmdMoveLast.Location = New System.Drawing.Point(178, -2)
        Me.cmdMoveLast.Name = "cmdMoveLast"
        Me.cmdMoveLast.Size = New System.Drawing.Size(30, 23)
        Me.cmdMoveLast.TabIndex = 606
        Me.cmdMoveLast.Text = ">|"
        Me.cmdMoveLast.UseVisualStyleBackColor = True
        '
        'cmdMoveFirst
        '
        Me.cmdMoveFirst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveFirst.ForeColor = System.Drawing.Color.RoyalBlue
        Me.cmdMoveFirst.Location = New System.Drawing.Point(-1, -2)
        Me.cmdMoveFirst.Name = "cmdMoveFirst"
        Me.cmdMoveFirst.Size = New System.Drawing.Size(30, 23)
        Me.cmdMoveFirst.TabIndex = 607
        Me.cmdMoveFirst.Text = "|<"
        Me.cmdMoveFirst.UseVisualStyleBackColor = True
        '
        'cmdMovePrev
        '
        Me.cmdMovePrev.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMovePrev.ForeColor = System.Drawing.Color.RoyalBlue
        Me.cmdMovePrev.Location = New System.Drawing.Point(27, -2)
        Me.cmdMovePrev.Name = "cmdMovePrev"
        Me.cmdMovePrev.Size = New System.Drawing.Size(30, 23)
        Me.cmdMovePrev.TabIndex = 606
        Me.cmdMovePrev.Text = "<"
        Me.cmdMovePrev.UseVisualStyleBackColor = True
        '
        'lblTotalRec
        '
        Me.lblTotalRec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTotalRec.Location = New System.Drawing.Point(100, 0)
        Me.lblTotalRec.Name = "lblTotalRec"
        Me.lblTotalRec.Size = New System.Drawing.Size(49, 20)
        Me.lblTotalRec.TabIndex = 608
        Me.lblTotalRec.Text = "of (1)"
        Me.lblTotalRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdMoveNext
        '
        Me.cmdMoveNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveNext.ForeColor = System.Drawing.Color.RoyalBlue
        Me.cmdMoveNext.Location = New System.Drawing.Point(149, -2)
        Me.cmdMoveNext.Name = "cmdMoveNext"
        Me.cmdMoveNext.Size = New System.Drawing.Size(30, 23)
        Me.cmdMoveNext.TabIndex = 605
        Me.cmdMoveNext.Text = ">"
        Me.cmdMoveNext.UseVisualStyleBackColor = True
        '
        'txtRecNo
        '
        Me.txtRecNo.Location = New System.Drawing.Point(59, -1)
        Me.txtRecNo.Name = "txtRecNo"
        Me.txtRecNo.Size = New System.Drawing.Size(39, 20)
        Me.txtRecNo.TabIndex = 607
        Me.txtRecNo.Text = "1"
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.Color.Wheat
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAdd.Location = New System.Drawing.Point(166, 58)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(81, 26)
        Me.cmdAdd.TabIndex = 600
        Me.cmdAdd.Tag = "Tester"
        Me.cmdAdd.Text = "&Add Image"
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(29, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 603
        Me.Label1.Text = "Seal #"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSealSN
        '
        Me.txtSealSN.AcceptsReturn = True
        Me.txtSealSN.BackColor = System.Drawing.Color.White
        Me.txtSealSN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSealSN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSealSN.ForeColor = System.Drawing.Color.Black
        Me.txtSealSN.Location = New System.Drawing.Point(166, 16)
        Me.txtSealSN.MaxLength = 0
        Me.txtSealSN.Name = "txtSealSN"
        Me.txtSealSN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSealSN.Size = New System.Drawing.Size(54, 21)
        Me.txtSealSN.TabIndex = 581
        Me.txtSealSN.Tag = "Tester"
        Me.txtSealSN.Text = "1"
        Me.txtSealSN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(8, 66)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(63, 13)
        Me.Label6.TabIndex = 585
        Me.Label6.Text = "Thickness"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSealID
        '
        Me.txtSealID.AcceptsReturn = True
        Me.txtSealID.BackColor = System.Drawing.SystemColors.Control
        Me.txtSealID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSealID.Enabled = False
        Me.txtSealID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSealID.ForeColor = System.Drawing.Color.Gray
        Me.txtSealID.Location = New System.Drawing.Point(80, 16)
        Me.txtSealID.MaxLength = 0
        Me.txtSealID.Name = "txtSealID"
        Me.txtSealID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSealID.Size = New System.Drawing.Size(30, 21)
        Me.txtSealID.TabIndex = 602
        Me.txtSealID.Text = "1"
        Me.txtSealID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMatThick
        '
        Me.txtMatThick.AcceptsReturn = True
        Me.txtMatThick.BackColor = System.Drawing.SystemColors.Control
        Me.txtMatThick.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMatThick.Enabled = False
        Me.txtMatThick.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMatThick.ForeColor = System.Drawing.Color.Gray
        Me.txtMatThick.Location = New System.Drawing.Point(77, 63)
        Me.txtMatThick.MaxLength = 0
        Me.txtMatThick.Name = "txtMatThick"
        Me.txtMatThick.ReadOnly = True
        Me.txtMatThick.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMatThick.Size = New System.Drawing.Size(54, 21)
        Me.txtMatThick.TabIndex = 587
        Me.txtMatThick.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(134, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(28, 13)
        Me.Label9.TabIndex = 582
        Me.Label9.Text = "S/N"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(78, 46)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(52, 13)
        Me.Label10.TabIndex = 583
        Me.Label10.Text = "Material"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(123, 156)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 24)
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
        Me.cmdCancel.Location = New System.Drawing.Point(201, 156)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 24)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Test_frmGen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(288, 198)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Test_frmGen"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "General Form"
        Me.Panel1.ResumeLayout(False)
        Me.pnlPanel2.ResumeLayout(False)
        Me.pnlPanel2.PerformLayout()
        Me.pnlNav.ResumeLayout(False)
        Me.pnlNav.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtMatThick As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents txtSealSN As System.Windows.Forms.TextBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents txtSealID As System.Windows.Forms.TextBox
    Friend WithEvents pnlPanel2 As System.Windows.Forms.Panel
    Friend WithEvents pnlNav As System.Windows.Forms.Panel
    Friend WithEvents cmdMoveLast As System.Windows.Forms.Button
    Friend WithEvents cmdMoveFirst As System.Windows.Forms.Button
    Friend WithEvents cmdMovePrev As System.Windows.Forms.Button
    Friend WithEvents lblTotalRec As System.Windows.Forms.Label
    Friend WithEvents cmdMoveNext As System.Windows.Forms.Button
    Friend WithEvents txtRecNo As System.Windows.Forms.TextBox
End Class
