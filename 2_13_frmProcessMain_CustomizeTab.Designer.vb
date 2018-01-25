<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Process_frmCustomTab
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Process_frmCustomTab))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.pnlPanel1 = New System.Windows.Forms.Panel()
        Me.chkExport = New System.Windows.Forms.CheckBox()
        Me.chkPreOrder = New System.Windows.Forms.CheckBox()
        Me.chkOrdEntry = New System.Windows.Forms.CheckBox()
        Me.chkCost = New System.Windows.Forms.CheckBox()
        Me.chkDesign = New System.Windows.Forms.CheckBox()
        Me.chkApp = New System.Windows.Forms.CheckBox()
        Me.chkManf = New System.Windows.Forms.CheckBox()
        Me.chkPurchase = New System.Windows.Forms.CheckBox()
        Me.chkDwg = New System.Windows.Forms.CheckBox()
        Me.chkQlty = New System.Windows.Forms.CheckBox()
        Me.chkTest = New System.Windows.Forms.CheckBox()
        Me.chkPlan = New System.Windows.Forms.CheckBox()
        Me.chkIssue = New System.Windows.Forms.CheckBox()
        Me.chkShip = New System.Windows.Forms.CheckBox()
        Me.chkApproval = New System.Windows.Forms.CheckBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
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
        Me.lblBorder.Size = New System.Drawing.Size(437, 202)
        Me.lblBorder.TabIndex = 0
        '
        'pnlPanel1
        '
        Me.pnlPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPanel1.Controls.Add(Me.grpGroupBox1)
        Me.pnlPanel1.Controls.Add(Me.cmdOK)
        Me.pnlPanel1.Controls.Add(Me.cmdCancel)
        Me.pnlPanel1.Location = New System.Drawing.Point(2, 2)
        Me.pnlPanel1.Name = "pnlPanel1"
        Me.pnlPanel1.Size = New System.Drawing.Size(435, 200)
        Me.pnlPanel1.TabIndex = 1
        '
        'chkExport
        '
        Me.chkExport.AutoSize = True
        Me.chkExport.Checked = True
        Me.chkExport.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExport.Location = New System.Drawing.Point(15, 43)
        Me.chkExport.Name = "chkExport"
        Me.chkExport.Size = New System.Drawing.Size(63, 17)
        Me.chkExport.TabIndex = 43
        Me.chkExport.Text = "Export"
        '
        'chkPreOrder
        '
        Me.chkPreOrder.AutoSize = True
        Me.chkPreOrder.Checked = True
        Me.chkPreOrder.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPreOrder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPreOrder.Location = New System.Drawing.Point(15, 21)
        Me.chkPreOrder.Name = "chkPreOrder"
        Me.chkPreOrder.Size = New System.Drawing.Size(83, 17)
        Me.chkPreOrder.TabIndex = 40
        Me.chkPreOrder.Text = "Pre-Order"
        '
        'chkOrdEntry
        '
        Me.chkOrdEntry.AutoSize = True
        Me.chkOrdEntry.Checked = True
        Me.chkOrdEntry.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOrdEntry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOrdEntry.Location = New System.Drawing.Point(15, 65)
        Me.chkOrdEntry.Name = "chkOrdEntry"
        Me.chkOrdEntry.Size = New System.Drawing.Size(93, 17)
        Me.chkOrdEntry.TabIndex = 41
        Me.chkOrdEntry.Text = "Order Entry"
        '
        'chkCost
        '
        Me.chkCost.AutoSize = True
        Me.chkCost.Checked = True
        Me.chkCost.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCost.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCost.Location = New System.Drawing.Point(15, 87)
        Me.chkCost.Name = "chkCost"
        Me.chkCost.Size = New System.Drawing.Size(115, 17)
        Me.chkCost.TabIndex = 42
        Me.chkCost.Text = "Cost Estimating"
        '
        'chkDesign
        '
        Me.chkDesign.AutoSize = True
        Me.chkDesign.Checked = True
        Me.chkDesign.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDesign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDesign.Location = New System.Drawing.Point(156, 18)
        Me.chkDesign.Name = "chkDesign"
        Me.chkDesign.Size = New System.Drawing.Size(65, 17)
        Me.chkDesign.TabIndex = 47
        Me.chkDesign.Text = "Design"
        '
        'chkApp
        '
        Me.chkApp.AutoSize = True
        Me.chkApp.Checked = True
        Me.chkApp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkApp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApp.Location = New System.Drawing.Point(16, 109)
        Me.chkApp.Name = "chkApp"
        Me.chkApp.Size = New System.Drawing.Size(88, 17)
        Me.chkApp.TabIndex = 44
        Me.chkApp.Text = "Application"
        '
        'chkManf
        '
        Me.chkManf.AutoSize = True
        Me.chkManf.Checked = True
        Me.chkManf.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkManf.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkManf.Location = New System.Drawing.Point(156, 41)
        Me.chkManf.Name = "chkManf"
        Me.chkManf.Size = New System.Drawing.Size(106, 17)
        Me.chkManf.TabIndex = 45
        Me.chkManf.Text = "Manufacturing"
        '
        'chkPurchase
        '
        Me.chkPurchase.AutoSize = True
        Me.chkPurchase.Checked = True
        Me.chkPurchase.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPurchase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPurchase.Location = New System.Drawing.Point(156, 64)
        Me.chkPurchase.Name = "chkPurchase"
        Me.chkPurchase.Size = New System.Drawing.Size(88, 17)
        Me.chkPurchase.TabIndex = 46
        Me.chkPurchase.Text = "Purchasing"
        '
        'chkDwg
        '
        Me.chkDwg.AutoSize = True
        Me.chkDwg.Checked = True
        Me.chkDwg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDwg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDwg.Location = New System.Drawing.Point(156, 110)
        Me.chkDwg.Name = "chkDwg"
        Me.chkDwg.Size = New System.Drawing.Size(73, 17)
        Me.chkDwg.TabIndex = 51
        Me.chkDwg.Text = "Drawing"
        '
        'chkQlty
        '
        Me.chkQlty.AutoSize = True
        Me.chkQlty.Checked = True
        Me.chkQlty.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkQlty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkQlty.Location = New System.Drawing.Point(156, 87)
        Me.chkQlty.Name = "chkQlty"
        Me.chkQlty.Size = New System.Drawing.Size(66, 17)
        Me.chkQlty.TabIndex = 48
        Me.chkQlty.Text = "Quality"
        '
        'chkTest
        '
        Me.chkTest.AutoSize = True
        Me.chkTest.Checked = True
        Me.chkTest.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTest.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTest.Location = New System.Drawing.Point(279, 21)
        Me.chkTest.Name = "chkTest"
        Me.chkTest.Size = New System.Drawing.Size(67, 17)
        Me.chkTest.TabIndex = 49
        Me.chkTest.Text = "Testing"
        '
        'chkPlan
        '
        Me.chkPlan.AutoSize = True
        Me.chkPlan.Checked = True
        Me.chkPlan.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPlan.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPlan.Location = New System.Drawing.Point(279, 43)
        Me.chkPlan.Name = "chkPlan"
        Me.chkPlan.Size = New System.Drawing.Size(74, 17)
        Me.chkPlan.TabIndex = 50
        Me.chkPlan.Text = "Planning"
        '
        'chkIssue
        '
        Me.chkIssue.AutoSize = True
        Me.chkIssue.Checked = True
        Me.chkIssue.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIssue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIssue.Location = New System.Drawing.Point(280, 87)
        Me.chkIssue.Name = "chkIssue"
        Me.chkIssue.Size = New System.Drawing.Size(118, 17)
        Me.chkIssue.TabIndex = 55
        Me.chkIssue.Text = "Issue/Comment"
        '
        'chkShip
        '
        Me.chkShip.AutoSize = True
        Me.chkShip.Checked = True
        Me.chkShip.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShip.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShip.Location = New System.Drawing.Point(280, 65)
        Me.chkShip.Name = "chkShip"
        Me.chkShip.Size = New System.Drawing.Size(75, 17)
        Me.chkShip.TabIndex = 52
        Me.chkShip.Text = "Shipping"
        '
        'chkApproval
        '
        Me.chkApproval.AutoSize = True
        Me.chkApproval.Checked = True
        Me.chkApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkApproval.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApproval.Location = New System.Drawing.Point(280, 109)
        Me.chkApproval.Name = "chkApproval"
        Me.chkApproval.Size = New System.Drawing.Size(77, 17)
        Me.chkApproval.TabIndex = 53
        Me.chkApproval.Text = "Approval"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(265, 162)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 56
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
        Me.cmdCancel.Location = New System.Drawing.Point(350, 162)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 57
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.chkPurchase)
        Me.grpGroupBox1.Controls.Add(Me.chkCost)
        Me.grpGroupBox1.Controls.Add(Me.chkOrdEntry)
        Me.grpGroupBox1.Controls.Add(Me.chkIssue)
        Me.grpGroupBox1.Controls.Add(Me.chkPreOrder)
        Me.grpGroupBox1.Controls.Add(Me.chkShip)
        Me.grpGroupBox1.Controls.Add(Me.chkExport)
        Me.grpGroupBox1.Controls.Add(Me.chkApproval)
        Me.grpGroupBox1.Controls.Add(Me.chkDwg)
        Me.grpGroupBox1.Controls.Add(Me.chkManf)
        Me.grpGroupBox1.Controls.Add(Me.chkQlty)
        Me.grpGroupBox1.Controls.Add(Me.chkApp)
        Me.grpGroupBox1.Controls.Add(Me.chkTest)
        Me.grpGroupBox1.Controls.Add(Me.chkDesign)
        Me.grpGroupBox1.Controls.Add(Me.chkPlan)
        Me.grpGroupBox1.Location = New System.Drawing.Point(10, 5)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(412, 137)
        Me.grpGroupBox1.TabIndex = 58
        Me.grpGroupBox1.TabStop = False
        '
        'Process_frmCustomTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(441, 205)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlPanel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Process_frmCustomTab"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SealProcess: Tab View Customization"
        Me.pnlPanel1.ResumeLayout(False)
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents pnlPanel1 As System.Windows.Forms.Panel
    Friend WithEvents chkExport As System.Windows.Forms.CheckBox
    Friend WithEvents chkPreOrder As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrdEntry As System.Windows.Forms.CheckBox
    Friend WithEvents chkCost As System.Windows.Forms.CheckBox
    Friend WithEvents chkIssue As System.Windows.Forms.CheckBox
    Friend WithEvents chkShip As System.Windows.Forms.CheckBox
    Friend WithEvents chkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkDwg As System.Windows.Forms.CheckBox
    Friend WithEvents chkQlty As System.Windows.Forms.CheckBox
    Friend WithEvents chkTest As System.Windows.Forms.CheckBox
    Friend WithEvents chkPlan As System.Windows.Forms.CheckBox
    Friend WithEvents chkDesign As System.Windows.Forms.CheckBox
    Friend WithEvents chkApp As System.Windows.Forms.CheckBox
    Friend WithEvents chkManf As System.Windows.Forms.CheckBox
    Friend WithEvents chkPurchase As System.Windows.Forms.CheckBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
End Class
