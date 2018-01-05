<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPE_frmAnalysisSet
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmAnalysisSet))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdExportToPNDB = New System.Windows.Forms.Button()
        Me.lblDesc = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.grpGroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmdFEA = New System.Windows.Forms.Button()
        Me.cmdOpCond = New System.Windows.Forms.Button()
        Me.cmdAppLoad = New System.Windows.Forms.Button()
        Me.cmdSealDesign = New System.Windows.Forms.Button()
        Me.cmdCavity = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdImport = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.chkSel_Project = New System.Windows.Forms.CheckBox()
        Me.cmdSummaryResult = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.optExisting = New System.Windows.Forms.RadioButton()
        Me.optNew = New System.Windows.Forms.RadioButton()
        Me.grdAnalysisSet = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdCopy = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        Me.grpGroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.grdAnalysisSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(2, 2)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(704, 424)
        Me.lblBorder.TabIndex = 9
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmdExportToPNDB)
        Me.Panel1.Controls.Add(Me.lblDesc)
        Me.Panel1.Controls.Add(Me.lblName)
        Me.Panel1.Controls.Add(Me.grpGroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(702, 422)
        Me.Panel1.TabIndex = 10
        '
        'cmdExportToPNDB
        '
        Me.cmdExportToPNDB.BackColor = System.Drawing.Color.Silver
        Me.cmdExportToPNDB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExportToPNDB.Image = CType(resources.GetObject("cmdExportToPNDB.Image"), System.Drawing.Image)
        Me.cmdExportToPNDB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdExportToPNDB.Location = New System.Drawing.Point(23, 384)
        Me.cmdExportToPNDB.Name = "cmdExportToPNDB"
        Me.cmdExportToPNDB.Size = New System.Drawing.Size(172, 28)
        Me.cmdExportToPNDB.TabIndex = 77
        Me.cmdExportToPNDB.Text = "   &Export To Main P/N DB"
        Me.cmdExportToPNDB.UseVisualStyleBackColor = False
        '
        'lblDesc
        '
        Me.lblDesc.AutoSize = True
        Me.lblDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.Location = New System.Drawing.Point(70, 355)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(0, 13)
        Me.lblDesc.TabIndex = 75
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(25, 355)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(46, 13)
        Me.lblName.TabIndex = 74
        Me.lblName.Text = "Desc: "
        '
        'grpGroupBox2
        '
        Me.grpGroupBox2.Controls.Add(Me.cmdFEA)
        Me.grpGroupBox2.Controls.Add(Me.cmdOpCond)
        Me.grpGroupBox2.Controls.Add(Me.cmdAppLoad)
        Me.grpGroupBox2.Controls.Add(Me.cmdSealDesign)
        Me.grpGroupBox2.Controls.Add(Me.cmdCavity)
        Me.grpGroupBox2.Location = New System.Drawing.Point(583, 8)
        Me.grpGroupBox2.Name = "grpGroupBox2"
        Me.grpGroupBox2.Size = New System.Drawing.Size(108, 330)
        Me.grpGroupBox2.TabIndex = 73
        Me.grpGroupBox2.TabStop = False
        '
        'cmdFEA
        '
        Me.cmdFEA.BackColor = System.Drawing.Color.Wheat
        Me.cmdFEA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFEA.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFEA.Location = New System.Drawing.Point(7, 146)
        Me.cmdFEA.Name = "cmdFEA"
        Me.cmdFEA.Size = New System.Drawing.Size(94, 30)
        Me.cmdFEA.TabIndex = 79
        Me.cmdFEA.Text = "&FEA"
        Me.cmdFEA.UseVisualStyleBackColor = False
        '
        'cmdOpCond
        '
        Me.cmdOpCond.BackColor = System.Drawing.Color.Wheat
        Me.cmdOpCond.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOpCond.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOpCond.Location = New System.Drawing.Point(7, 47)
        Me.cmdOpCond.Name = "cmdOpCond"
        Me.cmdOpCond.Size = New System.Drawing.Size(94, 30)
        Me.cmdOpCond.TabIndex = 76
        Me.cmdOpCond.Text = "&Oper. Cond."
        Me.cmdOpCond.UseVisualStyleBackColor = False
        '
        'cmdAppLoad
        '
        Me.cmdAppLoad.BackColor = System.Drawing.Color.Wheat
        Me.cmdAppLoad.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAppLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAppLoad.Location = New System.Drawing.Point(7, 80)
        Me.cmdAppLoad.Name = "cmdAppLoad"
        Me.cmdAppLoad.Size = New System.Drawing.Size(94, 30)
        Me.cmdAppLoad.TabIndex = 77
        Me.cmdAppLoad.Text = "&App. Load"
        Me.cmdAppLoad.UseVisualStyleBackColor = False
        '
        'cmdSealDesign
        '
        Me.cmdSealDesign.BackColor = System.Drawing.Color.Wheat
        Me.cmdSealDesign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSealDesign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSealDesign.Location = New System.Drawing.Point(7, 113)
        Me.cmdSealDesign.Name = "cmdSealDesign"
        Me.cmdSealDesign.Size = New System.Drawing.Size(94, 30)
        Me.cmdSealDesign.TabIndex = 78
        Me.cmdSealDesign.Text = "&Seal Design"
        Me.cmdSealDesign.UseVisualStyleBackColor = False
        '
        'cmdCavity
        '
        Me.cmdCavity.BackColor = System.Drawing.Color.Wheat
        Me.cmdCavity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCavity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCavity.Location = New System.Drawing.Point(7, 15)
        Me.cmdCavity.Name = "cmdCavity"
        Me.cmdCavity.Size = New System.Drawing.Size(94, 30)
        Me.cmdCavity.TabIndex = 73
        Me.cmdCavity.Text = "&Cavity"
        Me.cmdCavity.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdImport)
        Me.GroupBox1.Controls.Add(Me.cmdPrint)
        Me.GroupBox1.Controls.Add(Me.chkSel_Project)
        Me.GroupBox1.Controls.Add(Me.cmdSummaryResult)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.optExisting)
        Me.GroupBox1.Controls.Add(Me.optNew)
        Me.GroupBox1.Controls.Add(Me.grdAnalysisSet)
        Me.GroupBox1.Controls.Add(Me.cmdEdit)
        Me.GroupBox1.Controls.Add(Me.cmdDelete)
        Me.GroupBox1.Controls.Add(Me.cmdCopy)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(564, 330)
        Me.GroupBox1.TabIndex = 72
        Me.GroupBox1.TabStop = False
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.Color.Silver
        Me.cmdImport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.Image = CType(resources.GetObject("cmdImport.Image"), System.Drawing.Image)
        Me.cmdImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdImport.Location = New System.Drawing.Point(99, 296)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.Size = New System.Drawing.Size(130, 28)
        Me.cmdImport.TabIndex = 77
        Me.cmdImport.Text = "    &Import from File"
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.Color.Silver
        Me.cmdPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.Image = CType(resources.GetObject("cmdPrint.Image"), System.Drawing.Image)
        Me.cmdPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPrint.Location = New System.Drawing.Point(285, 248)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(130, 28)
        Me.cmdPrint.TabIndex = 76
        Me.cmdPrint.Text = "   &Export To File"
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'chkSel_Project
        '
        Me.chkSel_Project.AutoSize = True
        Me.chkSel_Project.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkSel_Project.ForeColor = System.Drawing.Color.Blue
        Me.chkSel_Project.Location = New System.Drawing.Point(419, 20)
        Me.chkSel_Project.Name = "chkSel_Project"
        Me.chkSel_Project.Size = New System.Drawing.Size(139, 17)
        Me.chkSel_Project.TabIndex = 74
        Me.chkSel_Project.Text = "Selected for Project"
        Me.chkSel_Project.UseVisualStyleBackColor = True
        '
        'cmdSummaryResult
        '
        Me.cmdSummaryResult.BackColor = System.Drawing.Color.Silver
        Me.cmdSummaryResult.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSummaryResult.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSummaryResult.Location = New System.Drawing.Point(426, 248)
        Me.cmdSummaryResult.Name = "cmdSummaryResult"
        Me.cmdSummaryResult.Size = New System.Drawing.Size(132, 28)
        Me.cmdSummaryResult.TabIndex = 73
        Me.cmdSummaryResult.Text = "Summary Results"
        Me.cmdSummaryResult.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(-8, 288)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(580, 1)
        Me.Label4.TabIndex = 71
        '
        'optExisting
        '
        Me.optExisting.AutoSize = True
        Me.optExisting.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optExisting.Location = New System.Drawing.Point(16, 19)
        Me.optExisting.Name = "optExisting"
        Me.optExisting.Size = New System.Drawing.Size(69, 17)
        Me.optExisting.TabIndex = 11
        Me.optExisting.TabStop = True
        Me.optExisting.Text = "Existing"
        Me.optExisting.UseVisualStyleBackColor = True
        '
        'optNew
        '
        Me.optNew.AutoSize = True
        Me.optNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNew.Location = New System.Drawing.Point(16, 302)
        Me.optNew.Name = "optNew"
        Me.optNew.Size = New System.Drawing.Size(49, 17)
        Me.optNew.TabIndex = 12
        Me.optNew.TabStop = True
        Me.optNew.Text = "New"
        Me.optNew.UseVisualStyleBackColor = True
        '
        'grdAnalysisSet
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdAnalysisSet.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.grdAnalysisSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdAnalysisSet.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column4, Me.Column3, Me.Column5, Me.Column6})
        Me.grdAnalysisSet.Location = New System.Drawing.Point(12, 48)
        Me.grdAnalysisSet.MultiSelect = False
        Me.grdAnalysisSet.Name = "grdAnalysisSet"
        Me.grdAnalysisSet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdAnalysisSet.Size = New System.Drawing.Size(546, 186)
        Me.grdAnalysisSet.TabIndex = 66
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "No"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 30
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "MCS"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 70
        '
        'Column4
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column4.HeaderText = "HFree"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 50
        '
        'Column3
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column3.HeaderText = "Load Case Name"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 150
        '
        'Column5
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle6
        Me.Column5.HeaderText = "Date Created"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column6
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column6.DefaultCellStyle = DataGridViewCellStyle7
        Me.Column6.HeaderText = "Time Created"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.Color.Silver
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdEdit.Location = New System.Drawing.Point(12, 248)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(72, 28)
        Me.cmdEdit.TabIndex = 70
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.Color.Silver
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDelete.Location = New System.Drawing.Point(99, 248)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(72, 28)
        Me.cmdDelete.TabIndex = 64
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.Color.Silver
        Me.cmdCopy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCopy.Location = New System.Drawing.Point(186, 248)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.Size = New System.Drawing.Size(72, 28)
        Me.cmdCopy.TabIndex = 63
        Me.cmdCopy.Text = "&Copy"
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(544, 384)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 21
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
        Me.cmdCancel.Location = New System.Drawing.Point(622, 384)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 22
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'IPE_frmAnalysisSet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(709, 428)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmAnalysisSet"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Analysis Set"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grpGroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.grdAnalysisSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents grdAnalysisSet As System.Windows.Forms.DataGridView
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents cmdCopy As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents optNew As System.Windows.Forms.RadioButton
    Friend WithEvents optExisting As System.Windows.Forms.RadioButton
    Friend WithEvents cmdEdit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdSummaryResult As System.Windows.Forms.Button
    Friend WithEvents cmdAppLoad As System.Windows.Forms.Button
    Friend WithEvents cmdOpCond As System.Windows.Forms.Button
    Friend WithEvents cmdCavity As System.Windows.Forms.Button
    Friend WithEvents cmdFEA As System.Windows.Forms.Button
    Friend WithEvents cmdSealDesign As System.Windows.Forms.Button
    Friend WithEvents grpGroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents chkSel_Project As System.Windows.Forms.CheckBox
    Friend WithEvents cmdPrint As System.Windows.Forms.Button
    Friend WithEvents cmdImport As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cmdExportToPNDB As System.Windows.Forms.Button
End Class
