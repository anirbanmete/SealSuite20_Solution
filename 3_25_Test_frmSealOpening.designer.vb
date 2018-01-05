<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Test_frmSealOpening
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Test_frmSealOpening))
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.mnuUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdSealTest = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdPN = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.imgLogo = New System.Windows.Forms.PictureBox()
        Me.SBar1 = New System.Windows.Forms.StatusStrip()
        Me.SBpanel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SBPanel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SBPanel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cmdPartNo = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(704, 24)
        Me.MainMenu.TabIndex = 66
        Me.MainMenu.Text = "MainMenu"
        '
        'mnuUser
        '
        Me.mnuUser.Name = "mnuUser"
        Me.mnuUser.Size = New System.Drawing.Size(42, 20)
        Me.mnuUser.Text = "&User"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(107, 22)
        Me.mnuAbout.Text = "&About"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.cmdSealTest)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmdPN)
        Me.GroupBox1.Controls.Add(Me.cmdExit)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(561, 28)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(138, 347)
        Me.GroupBox1.TabIndex = 78
        Me.GroupBox1.TabStop = False
        '
        'cmdSealTest
        '
        Me.cmdSealTest.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdSealTest.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSealTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSealTest.Location = New System.Drawing.Point(4, 43)
        Me.cmdSealTest.Name = "cmdSealTest"
        Me.cmdSealTest.Size = New System.Drawing.Size(129, 30)
        Me.cmdSealTest.TabIndex = 78
        Me.cmdSealTest.Text = "&SealTest"
        Me.cmdSealTest.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(0, 343)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(137, 2)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Label4"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.BackColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(136, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(2, 341)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Label3"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(1, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(137, 2)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(1, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(2, 341)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Label1"
        '
        'cmdPN
        '
        Me.cmdPN.AllowDrop = True
        Me.cmdPN.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdPN.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPN.Location = New System.Drawing.Point(5, 10)
        Me.cmdPN.Name = "cmdPN"
        Me.cmdPN.Size = New System.Drawing.Size(129, 30)
        Me.cmdPN.TabIndex = 0
        Me.cmdPN.Text = "P/N &View"
        Me.cmdPN.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdExit.Location = New System.Drawing.Point(5, 75)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(129, 30)
        Me.cmdExit.TabIndex = 6
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'imgLogo
        '
        Me.imgLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(24, 325)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(176, 56)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgLogo.TabIndex = 84
        Me.imgLogo.TabStop = False
        '
        'SBar1
        '
        Me.SBar1.AutoSize = False
        Me.SBar1.BackColor = System.Drawing.Color.AliceBlue
        Me.SBar1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.SBar1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.SBar1.Location = New System.Drawing.Point(0, 394)
        Me.SBar1.Name = "SBar1"
        Me.SBar1.Size = New System.Drawing.Size(704, 22)
        Me.SBar1.TabIndex = 91
        '
        'SBpanel1
        '
        Me.SBpanel1.AutoSize = False
        Me.SBpanel1.BackColor = System.Drawing.Color.AliceBlue
        Me.SBpanel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.SBpanel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.SBpanel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SBpanel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBpanel1.Name = "SBpanel1"
        Me.SBpanel1.Size = New System.Drawing.Size(230, 17)
        '
        'SBPanel3
        '
        Me.SBPanel3.AutoSize = False
        Me.SBPanel3.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.SBPanel3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.SBPanel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SBPanel3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBPanel3.Name = "SBPanel3"
        Me.SBPanel3.Size = New System.Drawing.Size(230, 17)
        '
        'SBPanel4
        '
        Me.SBPanel4.AutoSize = False
        Me.SBPanel4.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.SBPanel4.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner
        Me.SBPanel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.SBPanel4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SBPanel4.Name = "SBPanel4"
        Me.SBPanel4.Size = New System.Drawing.Size(220, 17)
        '
        'cmdPartNo
        '
        Me.cmdPartNo.AllowDrop = True
        Me.cmdPartNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdPartNo.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPartNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPartNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPartNo.Location = New System.Drawing.Point(5, 10)
        Me.cmdPartNo.Name = "cmdPartNo"
        Me.cmdPartNo.Size = New System.Drawing.Size(129, 30)
        Me.cmdPartNo.TabIndex = 0
        Me.cmdPartNo.Text = "P/N &View"
        Me.cmdPartNo.UseVisualStyleBackColor = False
        '
        'Test_frmSealOpening
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 416)
        Me.ControlBox = False
        Me.Controls.Add(Me.SBar1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.imgLogo)
        Me.Controls.Add(Me.MainMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Test_frmSealOpening"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SealTest: Main Form"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdSealTest As System.Windows.Forms.Button
    Private WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdPN As System.Windows.Forms.Button
    Friend WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents SBar1 As System.Windows.Forms.StatusStrip
    Friend WithEvents SBpanel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SBPanel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SBPanel4 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents cmdPartNo As System.Windows.Forms.Button
End Class
