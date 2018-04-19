<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserRole
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUserRole))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.pnlPanel1 = New System.Windows.Forms.Panel()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.grdUsers = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlPanel1.SuspendLayout()
        CType(Me.grdUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBorder.Location = New System.Drawing.Point(2, 2)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(862, 258)
        Me.lblBorder.TabIndex = 0
        '
        'pnlPanel1
        '
        Me.pnlPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPanel1.Controls.Add(Me.cmdCancel)
        Me.pnlPanel1.Controls.Add(Me.cmdOK)
        Me.pnlPanel1.Controls.Add(Me.grdUsers)
        Me.pnlPanel1.Location = New System.Drawing.Point(3, 3)
        Me.pnlPanel1.Name = "pnlPanel1"
        Me.pnlPanel1.Size = New System.Drawing.Size(860, 256)
        Me.pnlPanel1.TabIndex = 1
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(769, 214)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 30)
        Me.cmdCancel.TabIndex = 82
        Me.cmdCancel.Text = " &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(673, 214)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 30)
        Me.cmdOK.TabIndex = 81
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'grdUsers
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdUsers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdUsers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7})
        Me.grdUsers.Location = New System.Drawing.Point(8, 7)
        Me.grdUsers.MultiSelect = False
        Me.grdUsers.Name = "grdUsers"
        Me.grdUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdUsers.Size = New System.Drawing.Size(841, 186)
        Me.grdUsers.TabIndex = 80
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "Name"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 160
        '
        'Column2
        '
        Me.Column2.HeaderText = "Role 1"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 135
        '
        'Column3
        '
        Me.Column3.HeaderText = "Role 2"
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 135
        '
        'Column4
        '
        Me.Column4.HeaderText = "Role 3"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 135
        '
        'Column5
        '
        Me.Column5.HeaderText = "Super Role 1"
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 110
        '
        'Column6
        '
        Me.Column6.HeaderText = "Super Role 2"
        Me.Column6.Name = "Column6"
        Me.Column6.Width = 110
        '
        'Column7
        '
        Me.Column7.HeaderText = "ID"
        Me.Column7.Name = "Column7"
        Me.Column7.Width = 10
        '
        'frmUserRole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(865, 260)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlPanel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmUserRole"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "User Role"
        Me.pnlPanel1.ResumeLayout(False)
        CType(Me.grdUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents pnlPanel1 As System.Windows.Forms.Panel
    Private WithEvents cmdCancel As System.Windows.Forms.Button
    Private WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents grdUsers As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewComboBoxColumn
    Friend WithEvents Column3 As DataGridViewComboBoxColumn
    Friend WithEvents Column4 As DataGridViewComboBoxColumn
    Friend WithEvents Column5 As DataGridViewComboBoxColumn
    Friend WithEvents Column6 As DataGridViewComboBoxColumn
    Friend WithEvents Column7 As DataGridViewTextBoxColumn
End Class
