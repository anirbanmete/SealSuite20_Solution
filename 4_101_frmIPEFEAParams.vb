'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmFEAParams                           '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  06DEC16                                '
'                                                                              '
'===============================================================================


Public Class IPE_frmFEAParams
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mANSYS As New IPE_clsANSYS(gIPE_File.DirWorkANSYS)

#End Region


#Region " Windows Form Designer generated code "



    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents txtNelMax As System.Windows.Forms.TextBox
    Public WithEvents cmbANSYSVersion As System.Windows.Forms.ComboBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblSplitter1 As System.Windows.Forms.Label
    Friend WithEvents lblSplitter0 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label0 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents cmbModel As System.Windows.Forms.ComboBox
    Public WithEvents cmbRunType As System.Windows.Forms.ComboBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents cmbSolve As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmFEAParams))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbModel = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblSplitter0 = New System.Windows.Forms.Label()
        Me.cmbRunType = New System.Windows.Forms.ComboBox()
        Me.cmbSolve = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.lblSplitter1 = New System.Windows.Forms.Label()
        Me.txtNelMax = New System.Windows.Forms.TextBox()
        Me.cmbANSYSVersion = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label0 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(342, 230)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbModel)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.lblSplitter0)
        Me.Panel1.Controls.Add(Me.cmbRunType)
        Me.Panel1.Controls.Add(Me.cmbSolve)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.lblSplitter1)
        Me.Panel1.Controls.Add(Me.txtNelMax)
        Me.Panel1.Controls.Add(Me.cmbANSYSVersion)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label0)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(340, 228)
        Me.Panel1.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(167, 138)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(61, 20)
        Me.Label4.TabIndex = 981
        Me.Label4.Text = "Run Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbModel
        '
        Me.cmbModel.BackColor = System.Drawing.SystemColors.Window
        Me.cmbModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbModel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbModel.ForeColor = System.Drawing.Color.Black
        Me.cmbModel.Location = New System.Drawing.Point(119, 56)
        Me.cmbModel.Name = "cmbModel"
        Me.cmbModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbModel.Size = New System.Drawing.Size(60, 21)
        Me.cmbModel.TabIndex = 980
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(69, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(44, 20)
        Me.Label8.TabIndex = 979
        Me.Label8.Text = "Model"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSplitter0
        '
        Me.lblSplitter0.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSplitter0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSplitter0.ForeColor = System.Drawing.SystemColors.InactiveBorder
        Me.lblSplitter0.Location = New System.Drawing.Point(0, 42)
        Me.lblSplitter0.Name = "lblSplitter0"
        Me.lblSplitter0.Size = New System.Drawing.Size(340, 2)
        Me.lblSplitter0.TabIndex = 978
        '
        'cmbRunType
        '
        Me.cmbRunType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbRunType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbRunType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRunType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRunType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbRunType.Location = New System.Drawing.Point(232, 138)
        Me.cmbRunType.Name = "cmbRunType"
        Me.cmbRunType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbRunType.Size = New System.Drawing.Size(96, 21)
        Me.cmbRunType.TabIndex = 974
        '
        'cmbSolve
        '
        Me.cmbSolve.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSolve.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSolve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSolve.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSolve.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSolve.Location = New System.Drawing.Point(101, 138)
        Me.cmbSolve.Name = "cmbSolve"
        Me.cmbSolve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSolve.Size = New System.Drawing.Size(40, 21)
        Me.cmbSolve.TabIndex = 973
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(53, 138)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(44, 20)
        Me.Label6.TabIndex = 971
        Me.Label6.Text = "Solve"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 20)
        Me.Label3.TabIndex = 964
        Me.Label3.Text = "Solution Parameters:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(167, 192)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 52
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
        Me.cmdCancel.Location = New System.Drawing.Point(255, 192)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 53
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lblSplitter1
        '
        Me.lblSplitter1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSplitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSplitter1.ForeColor = System.Drawing.SystemColors.InactiveBorder
        Me.lblSplitter1.Location = New System.Drawing.Point(0, 90)
        Me.lblSplitter1.Name = "lblSplitter1"
        Me.lblSplitter1.Size = New System.Drawing.Size(340, 2)
        Me.lblSplitter1.TabIndex = 36
        '
        'txtNelMax
        '
        Me.txtNelMax.AcceptsReturn = True
        Me.txtNelMax.BackColor = System.Drawing.SystemColors.Window
        Me.txtNelMax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNelMax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNelMax.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtNelMax.Location = New System.Drawing.Point(275, 57)
        Me.txtNelMax.MaxLength = 0
        Me.txtNelMax.Name = "txtNelMax"
        Me.txtNelMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNelMax.Size = New System.Drawing.Size(53, 21)
        Me.txtNelMax.TabIndex = 25
        Me.txtNelMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbANSYSVersion
        '
        Me.cmbANSYSVersion.BackColor = System.Drawing.SystemColors.Window
        Me.cmbANSYSVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbANSYSVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbANSYSVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbANSYSVersion.ForeColor = System.Drawing.Color.Black
        Me.cmbANSYSVersion.Location = New System.Drawing.Point(119, 9)
        Me.cmbANSYSVersion.Name = "cmbANSYSVersion"
        Me.cmbANSYSVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbANSYSVersion.Size = New System.Drawing.Size(52, 21)
        Me.cmbANSYSVersion.TabIndex = 23
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(201, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(69, 34)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Max No. of Elements"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label0
        '
        Me.Label0.BackColor = System.Drawing.Color.Transparent
        Me.Label0.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label0.Location = New System.Drawing.Point(9, 11)
        Me.Label0.Name = "Label0"
        Me.Label0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label0.Size = New System.Drawing.Size(104, 16)
        Me.Label0.TabIndex = 20
        Me.Label0.Text = "ANSYS Version"
        Me.Label0.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'IPE_frmFEAParams
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(348, 236)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmFEAParams"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FEA Parameters"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public Sub New()
        '==========

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call        

        ''POPULATE COMBO BOX LISTS:
        ''-------------------------

        '....ANSYS VERSION:
        '........The ComboBox item list depends on the ANSYS Edition chosen.
        '........Hence the list is put in the "cmbANSYSEdition_SelectedIndexChanged" Event.

        '....Solve Options   
        With cmbSolve.Items
            .Clear()
            .Add("Y")
            .Add("N")
        End With
        cmbSolve.SelectedIndex = 0

        '....Run Type Options
        With cmbRunType.Items
            .Clear()
            .Add("Batch")
            .Add("Interactive")
        End With
        cmbRunType.SelectedIndex = 0

    End Sub


#Region "FORM EVENT ROUTINES"

    Private Sub frmFEA_Load(ByVal sender As Object, ByVal e As System.EventArgs) _
                            Handles MyBase.Load
        '===========================================================================

        '....Create & initialize the local Seal Object.        
        InitializeLocalObject()             '....gIPE_ANSYS ===> mANSYS.   

        '....ANSYS Model.
        With cmbModel.Items
            .Clear()

            'AES 19APR16
            'If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "C-Seal" Or gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
            '    .Add("Full")
            'End If
            .Add("Full")
            .Add("Half")

            '.Add("Full")
            'If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            '    .Add("Half")
            'End If

        End With
        cmbModel.SelectedIndex = 0

        SetCmbVersion()
        SetTextNelMax()

        DisplayData()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '==============================
        '....Copy global object to local Object.  gIPE_ANSYS ===> mANSYS.  
        mANSYS = gIPE_ANSYS.Clone()
    End Sub


    Private Sub DisplayData()
        '=================== 

        With mANSYS
            If .Version <> "" Then cmbANSYSVersion.Text = .Version
            If .Model <> "" Then cmbModel.Text = .Model

            txtNelMax.Text = .NelMax

            If .Solve <> "" Then cmbSolve.Text = .Solve
            If .RunType <> "" Then cmbRunType.Text = .RunType
        End With

    End Sub

#End Region

#End Region


#Region "CONTROL EVENT ROUTINES:"


    Private Sub cmbANSYSVersion_SelectedIndexChanged(ByVal sender As System.Object, _
                                            ByVal e As System.EventArgs) _
                                            Handles cmbANSYSVersion.SelectedIndexChanged
        '===============================================================================

        If cmbANSYSVersion.Text <> mANSYS.Version Then
            mANSYS.Version = cmbANSYSVersion.Text
        End If

        SetTextNelMax()
    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '===========================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
            gFile.SaveIniFile(gUser, gIPE_Project, gIPE_ANSYS, gIPE_Unit)
        End If

        Me.Close()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '=================

        With gIPE_ANSYS
            .Version = cmbANSYSVersion.Text
            .Model = cmbModel.Text
            .NelMax = txtNelMax.Text
            .Solve = cmbSolve.Text
            .RunType = cmbRunType.Text

            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.NelMax = .NelMax
        End With

    End Sub

#End Region

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub SetCmbVersion()
        '====================== 

        Dim pIndexLast As Int16

        With cmbANSYSVersion.Items
            .Clear()
            .Add("13.0")
            .Add("15.0")
            .Add("16.0")
            .Add("17.0")
        End With

        '....Default set to 17
        pIndexLast = 3

        If mANSYS.Version = "13.0" Or mANSYS.Version = "13" Then
            pIndexLast = 0

        ElseIf mANSYS.Version = "15.0" Or mANSYS.Version = "15" Then
            pIndexLast = 1

        ElseIf mANSYS.Version = "16.0" Or mANSYS.Version = "16" Then
            pIndexLast = 2

        ElseIf mANSYS.Version = "17.0" Or mANSYS.Version = "17" Then
            pIndexLast = 3

        End If

        '....Select the latest version.
        cmbANSYSVersion.SelectedIndex = pIndexLast

    End Sub


    Private Sub SetTextNelMax()
        '======================      
        Dim pColor_F As Color
        Dim pColor_B As Color
        Dim pblnTxt As Boolean

        pColor_F = Color.Magenta
        pColor_B = Color.White
        pblnTxt = False

        With txtNelMax
            .Text = mANSYS.NelMax
            .ForeColor = pColor_F
        End With

    End Sub

#End Region

End Class
