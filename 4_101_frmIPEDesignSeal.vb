
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmDesign                              '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  30AUG16                                '
'                                                                              '
'===============================================================================
'
Imports System.Linq
Imports clsLibrary11

Public Class IPE_frmDesignSeal
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    'Private mESeal As IPE_clsESeal
    Private mSeal As IPE_clsSeal
    Private mMat As IPE_clsMaterial

#End Region

    '....Local Seal object is not necessary here as no global data is changed 
    '.......while the form remains open.

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
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdDetailDesign As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblType As System.Windows.Forms.Label
    Public WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents grpGroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeg As System.Windows.Forms.CheckBox
    Friend WithEvents txtSegNo As System.Windows.Forms.TextBox
    Friend WithEvents lblSegNo As System.Windows.Forms.Label
    Public WithEvents chkCoating As System.Windows.Forms.CheckBox
    Public WithEvents lblUnitSFinish As System.Windows.Forms.Label
    Friend WithEvents lblSFinish As System.Windows.Forms.Label
    Public WithEvents cmbSFinish As System.Windows.Forms.ComboBox
    Public WithEvents cmbCoating As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents cmbMatName As System.Windows.Forms.ComboBox
    Public WithEvents cmbHT As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpCoating As System.Windows.Forms.GroupBox

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents toolTipCmdSeal As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmDesignSeal))
        Me.toolTipCmdSeal = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpCoating = New System.Windows.Forms.GroupBox()
        Me.cmbCoating = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.chkCoating = New System.Windows.Forms.CheckBox()
        Me.lblSFinish = New System.Windows.Forms.Label()
        Me.cmbSFinish = New System.Windows.Forms.ComboBox()
        Me.lblUnitSFinish = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbHT = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbMatName = New System.Windows.Forms.ComboBox()
        Me.grpGroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtSegNo = New System.Windows.Forms.TextBox()
        Me.lblSegNo = New System.Windows.Forms.Label()
        Me.chkSeg = New System.Windows.Forms.CheckBox()
        Me.cmdDetailDesign = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.lblType = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.Panel1.SuspendLayout()
        Me.grpCoating.SuspendLayout()
        Me.grpGroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(382, 216)
        Me.lblBorder.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.grpCoating)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cmbHT)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbMatName)
        Me.Panel1.Controls.Add(Me.grpGroupBox1)
        Me.Panel1.Controls.Add(Me.cmdDetailDesign)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.lblType)
        Me.Panel1.Controls.Add(Me.cmbType)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(380, 214)
        Me.Panel1.TabIndex = 30
        '
        'grpCoating
        '
        Me.grpCoating.Controls.Add(Me.cmbCoating)
        Me.grpCoating.Controls.Add(Me.Label13)
        Me.grpCoating.Controls.Add(Me.chkCoating)
        Me.grpCoating.Controls.Add(Me.lblSFinish)
        Me.grpCoating.Controls.Add(Me.cmbSFinish)
        Me.grpCoating.Controls.Add(Me.lblUnitSFinish)
        Me.grpCoating.Location = New System.Drawing.Point(182, 81)
        Me.grpCoating.Name = "grpCoating"
        Me.grpCoating.Size = New System.Drawing.Size(182, 66)
        Me.grpCoating.TabIndex = 187
        Me.grpCoating.TabStop = False
        '
        'cmbCoating
        '
        Me.cmbCoating.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCoating.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCoating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCoating.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCoating.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCoating.Items.AddRange(New Object() {"Tricom", "T800"})
        Me.cmbCoating.Location = New System.Drawing.Point(12, 34)
        Me.cmbCoating.Name = "cmbCoating"
        Me.cmbCoating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCoating.Size = New System.Drawing.Size(64, 21)
        Me.cmbCoating.TabIndex = 178
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(26, 16)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(51, 13)
        Me.Label13.TabIndex = 177
        Me.Label13.Text = "Coating"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkCoating
        '
        Me.chkCoating.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.chkCoating.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCoating.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCoating.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCoating.Location = New System.Drawing.Point(12, 16)
        Me.chkCoating.Name = "chkCoating"
        Me.chkCoating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCoating.Size = New System.Drawing.Size(13, 14)
        Me.chkCoating.TabIndex = 182
        Me.chkCoating.UseVisualStyleBackColor = False
        '
        'lblSFinish
        '
        Me.lblSFinish.AutoSize = True
        Me.lblSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSFinish.Location = New System.Drawing.Point(93, 15)
        Me.lblSFinish.Name = "lblSFinish"
        Me.lblSFinish.Size = New System.Drawing.Size(47, 13)
        Me.lblSFinish.TabIndex = 180
        Me.lblSFinish.Text = "SFinish"
        Me.lblSFinish.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbSFinish
        '
        Me.cmbSFinish.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSFinish.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSFinish.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSFinish.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSFinish.Items.AddRange(New Object() {"65", "125"})
        Me.cmbSFinish.Location = New System.Drawing.Point(92, 34)
        Me.cmbSFinish.Name = "cmbSFinish"
        Me.cmbSFinish.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSFinish.Size = New System.Drawing.Size(48, 21)
        Me.cmbSFinish.TabIndex = 179
        '
        'lblUnitSFinish
        '
        Me.lblUnitSFinish.AutoSize = True
        Me.lblUnitSFinish.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitSFinish.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnitSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitSFinish.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnitSFinish.Location = New System.Drawing.Point(146, 37)
        Me.lblUnitSFinish.Name = "lblUnitSFinish"
        Me.lblUnitSFinish.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnitSFinish.Size = New System.Drawing.Size(29, 13)
        Me.lblUnitSFinish.TabIndex = 181
        Me.lblUnitSFinish.Text = "rms"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Silver
        Me.Label3.Location = New System.Drawing.Point(0, 164)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(380, 2)
        Me.Label3.TabIndex = 186
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Silver
        Me.Label2.Location = New System.Drawing.Point(0, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(380, 2)
        Me.Label2.TabIndex = 185
        '
        'cmbHT
        '
        Me.cmbHT.BackColor = System.Drawing.SystemColors.Window
        Me.cmbHT.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbHT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbHT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbHT.Items.AddRange(New Object() {"16", "15", "62"})
        Me.cmbHT.Location = New System.Drawing.Point(116, 114)
        Me.cmbHT.Name = "cmbHT"
        Me.cmbHT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbHT.Size = New System.Drawing.Size(48, 21)
        Me.cmbHT.TabIndex = 184
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(129, 98)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 13)
        Me.Label1.TabIndex = 183
        Me.Label1.Text = "HT"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(29, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 176
        Me.Label4.Text = "Material"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbMatName
        '
        Me.cmbMatName.BackColor = System.Drawing.SystemColors.Window
        Me.cmbMatName.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbMatName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMatName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMatName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbMatName.Items.AddRange(New Object() {"Alloy718", "Waspaloy", "Rene41", "X-750"})
        Me.cmbMatName.Location = New System.Drawing.Point(18, 115)
        Me.cmbMatName.Name = "cmbMatName"
        Me.cmbMatName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbMatName.Size = New System.Drawing.Size(78, 21)
        Me.cmbMatName.TabIndex = 175
        '
        'grpGroupBox1
        '
        Me.grpGroupBox1.Controls.Add(Me.txtSegNo)
        Me.grpGroupBox1.Controls.Add(Me.lblSegNo)
        Me.grpGroupBox1.Controls.Add(Me.chkSeg)
        Me.grpGroupBox1.Location = New System.Drawing.Point(182, 10)
        Me.grpGroupBox1.Name = "grpGroupBox1"
        Me.grpGroupBox1.Size = New System.Drawing.Size(182, 44)
        Me.grpGroupBox1.TabIndex = 33
        Me.grpGroupBox1.TabStop = False
        '
        'txtSegNo
        '
        Me.txtSegNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSegNo.Location = New System.Drawing.Point(137, 15)
        Me.txtSegNo.Name = "txtSegNo"
        Me.txtSegNo.Size = New System.Drawing.Size(27, 21)
        Me.txtSegNo.TabIndex = 35
        '
        'lblSegNo
        '
        Me.lblSegNo.AutoSize = True
        Me.lblSegNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSegNo.Location = New System.Drawing.Point(116, 19)
        Me.lblSegNo.Name = "lblSegNo"
        Me.lblSegNo.Size = New System.Drawing.Size(16, 13)
        Me.lblSegNo.TabIndex = 34
        Me.lblSegNo.Text = "#"
        '
        'chkSeg
        '
        Me.chkSeg.AutoSize = True
        Me.chkSeg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeg.Location = New System.Drawing.Point(12, 17)
        Me.chkSeg.Name = "chkSeg"
        Me.chkSeg.Size = New System.Drawing.Size(91, 17)
        Me.chkSeg.TabIndex = 33
        Me.chkSeg.Text = "Segmented"
        '
        'cmdDetailDesign
        '
        Me.cmdDetailDesign.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdDetailDesign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailDesign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDetailDesign.Location = New System.Drawing.Point(10, 179)
        Me.cmdDetailDesign.Name = "cmdDetailDesign"
        Me.cmdDetailDesign.Size = New System.Drawing.Size(106, 24)
        Me.cmdDetailDesign.TabIndex = 2
        Me.cmdDetailDesign.Text = "&Detailed Design"
        Me.cmdDetailDesign.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(211, 179)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 24)
        Me.cmdOK.TabIndex = 3
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
        Me.cmdCancel.Location = New System.Drawing.Point(292, 179)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 24)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.Location = New System.Drawing.Point(38, 10)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(35, 13)
        Me.lblType.TabIndex = 28
        Me.lblType.Text = "Type"
        Me.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Enabled = False
        Me.cmbType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbType.Location = New System.Drawing.Point(16, 30)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(80, 21)
        Me.cmbType.TabIndex = 1
        '
        'IPE_frmDesignSeal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(387, 221)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmDesignSeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Seal Design"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grpCoating.ResumeLayout(False)
        Me.grpCoating.PerformLayout()
        Me.grpGroupBox1.ResumeLayout(False)
        Me.grpGroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New()
        '===========
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub


#Region "FORM EVENT ROUTINES:"

    Private Sub frmDesign_Load(ByVal sender As Object, _
                               ByVal e As System.EventArgs) Handles MyBase.Load
        '======================================================================
        InitializeLocalObject()

        With cmbType
            .Items.Clear()
            .Items.Add("E-Seal")
            .Items.Add("C-Seal")
            .Items.Add("U-Seal")

            If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
                .SelectedIndex = 0       '...."E-Seal".

            ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "C-Seal" Then
                .SelectedIndex = 1       '...."E-Seal".

            ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
                .SelectedIndex = 2       '...."E-Seal".
            End If

        End With

        cmbMatName.Items.Clear()
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.PopulateMaterialList(cmbMatName)               '....Material List 

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then

            grpCoating.Visible = True
            '....Populate Coating Combo Box:
            With cmbCoating
                .Items.Clear()
                .Items.Add("Tricom")
                .Items.Add("T800")

                .SelectedIndex = 0
            End With

            'Coating:
            '-------
            'If Not IsNothing(mMat.Coating) Then


            If mMat.Coating <> "None" Then
                chkCoating.Checked = True
                'cmbCoating.Enabled = True
            Else
                chkCoating.Checked = False
                cmbCoating.Enabled = False
            End If
            'Else
            '    chkCoating.Checked = False
            '    cmbCoating.Visible = False
            'End If

            '....Populate Surface Finish Combo Box.
            PopulateCmbSFinish()

        Else
            grpCoating.Visible = False

        End If


        DisplayData()

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            cmbType.Enabled = False

            chkSeg.Enabled = False
            txtSegNo.Enabled = False

            cmbMatName.Enabled = False
            cmbHT.Enabled = False
            chkCoating.Enabled = False
            cmbCoating.Enabled = False
            cmbSFinish.Enabled = False

        Else
            chkSeg.Enabled = True
            txtSegNo.Enabled = True

            cmbMatName.Enabled = True
            cmbHT.Enabled = True
            chkCoating.Enabled = True
            'cmbCoating.Enabled = True
            'cmbSFinish.Enabled = True

        End If

        'AES 30AUG16
        '....Segment not yet implemented for C-Seal & U-Seal.
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "C-Seal" Or
           gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
            chkSeg.Enabled = False
            chkSeg.Checked = False
            txtSegNo.Enabled = False

        End If


    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '===============================
        '....Create & initialize the local Seal Object. 

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            mSeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "C-Seal" Then
            mSeal = New IPE_clsCSeal("C-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
            mSeal = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        End If

        mMat = New IPE_clsMaterial(gIPE_File.DirWorkANSYS)

        With mSeal
            .IsSegmented = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.IsSegmented

            If (.IsSegmented) Then
                .CountSegment = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.CountSegment
            End If
        End With

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            CType(mSeal, IPE_clsESeal).SFinish = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).SFinish
        End If

        With mMat
            .Name = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Name
            .HT = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.HT
            .Coating = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating
        End With

    End Sub


    Private Sub DisplayData()
        '===================

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal Is Nothing = False Then
            cmbType.Text = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type
        End If

        chkSeg.Checked = mSeal.IsSegmented

        If (mSeal.IsSegmented) Then
            lblSegNo.Enabled = True
            txtSegNo.Enabled = True
        Else
            lblSegNo.Enabled = False
            txtSegNo.Enabled = False
        End If

        If (chkSeg.Checked) Then
            txtSegNo.Text = ConvertToInt(mSeal.CountSegment)
        End If

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then

            If mMat.Coating <> "" And mMat.Coating <> "None" Then
                cmbCoating.Enabled = True
                cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
                cmbCoating.Text = mMat.Coating

                If cmbCoating.Text = "T800" Then
                    lblSFinish.Enabled = True
                    cmbSFinish.Enabled = True
                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                Else
                    lblSFinish.Enabled = False
                    cmbSFinish.Enabled = False
                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                    cmbSFinish.Text = ""
                End If

            Else
                cmbCoating.Enabled = False
                cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
                cmbCoating.Text = ""
                lblSFinish.Enabled = False
                cmbSFinish.Enabled = False
                cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                cmbSFinish.Text = ""
            End If

            If cmbSFinish.Items.Count > 0 Then
                If CType(mSeal, IPE_clsESeal).SFinish > gcEPS Then
                    cmbSFinish.Text = CType(mSeal, IPE_clsESeal).SFinish

                Else
                    cmbSFinish.SelectedIndex = 0

                End If
            End If

        End If

        'Material Name
        '-------------
        '....As the global variable "gMat" does not get modified while the form is open,
        '........no local "Material" object is used here.
        If mMat.Name <> "" Then
            cmbMatName.Text = mMat.Name
        Else
            cmbMatName.SelectedIndex = 0
        End If

    End Sub


    Private Sub PopulateCmbSFinish()
        '===========================  
        '....This routine populates the Surface Finish combo box. (Database Driven).
        Dim pSealMCSDB As New SealIPEMCSDBEntities()
        Dim pRecordLeak = (From it In pSealMCSDB.tblESeal_Leak_T800
                                    Select it.fldSFinish Distinct).ToList()     'AES 29MAR16

        cmbSFinish.Items.Clear()
        Dim pRecord As New tblESeal_Leak_T800

        If (pRecordLeak.Count > 0) Then
            For i As Integer = 0 To pRecordLeak.Count - 1
                cmbSFinish.Items.Add(pRecordLeak(i))
            Next
        End If

        cmbSFinish.SelectedIndex = 0
    End Sub

#End Region


#End Region


#Region "CONTROL EVENT ROUTINES:"

#Region "TEXTBOX RELATED ROUTINES"

    Private Sub txtSegNo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSegNo.TextChanged
        '========================================================================================================
        If (mSeal.IsSegmented) Then
            mSeal.CountSegment = ConvertToInt(txtSegNo.Text)
        End If

    End Sub

    'AES 29APR16
    Private Sub txtSegNo_Validated(sender As System.Object, e As System.EventArgs) Handles txtSegNo.Validated
        '=====================================================================================================
        If (mSeal.IsSegmented) Then

            If (Not mSeal.CountSegment > 1) Then
                MessageBox.Show("No. of segment is always greater than 1", "Segment Count", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtSegNo.Focus()
                Return
            End If

        End If
    End Sub

#End Region

#Region "CHECKBOX RELATED ROUTINES"

    Private Sub chkSeg_CheckedChanged(sender As System.Object,
                                      e As System.EventArgs) Handles chkSeg.CheckedChanged
        '=================================================================================

        If (chkSeg.Checked) Then
            lblSegNo.Enabled = True
            txtSegNo.Enabled = True
            txtSegNo.Text = mSeal.CountSegment

        Else
            lblSegNo.Enabled = False
            txtSegNo.Text = ""
            txtSegNo.Enabled = False
        End If

        mSeal.IsSegmented = chkSeg.Checked

    End Sub


    Private Sub chkCoating_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                          Handles chkCoating.CheckedChanged
        '===================================================================================

        If chkCoating.Checked Then
            cmbCoating.Enabled = True
            cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCoating.Text = mMat.Coating

            If cmbCoating.Text = "T800" Then
                lblSFinish.Enabled = True
                cmbSFinish.Enabled = True
                cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                cmbSFinish.Text = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).SFinish.ToString()      'AES 04MAY16
                lblUnitSFinish.Enabled = True
            End If

        Else
            cmbCoating.Enabled = False
            cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
            cmbCoating.Text = ""            'AES 04MAY16

            lblSFinish.Enabled = False
            cmbSFinish.Enabled = False
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
            cmbSFinish.Text = ""
            lblUnitSFinish.Enabled = False

        End If

    End Sub

#End Region

#Region "COMBO-BOX RELATED ROUTINES"

    Private Sub cmbCoating_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles _
                                                cmbCoating.SelectedIndexChanged
        '==================================================================================================

        Dim pCoat As String = cmbCoating.Text
        If pCoat = "T800" Then
            lblSFinish.Enabled = True
            cmbSFinish.Enabled = True
            lblUnitSFinish.Enabled = True
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
        Else
            lblSFinish.Enabled = False
            cmbSFinish.Enabled = False
            lblUnitSFinish.Enabled = False
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
            cmbSFinish.Text = ""
        End If

    End Sub

    Private Sub cmbMatName_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles cmbMatName.SelectedIndexChanged
        '==========================================================================================
        Pupulate_HT(cmbMatName.Text)

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub Pupulate_HT(ByVal MatName_In As String)
        '===============================================
        Dim pSealMCSDB As New SealIPEMCSDBEntities()
        cmbHT.Items.Clear()
        Dim pCount As Integer = (From it In pSealMCSDB.tblMaterial
                                    Where it.fldName = MatName_In Select it).Count()
        Dim pMatCode As String = ""
        If (pCount > 0) Then
            Dim pQry_Mat = (From it In pSealMCSDB.tblMaterial
                                Where it.fldName = MatName_In Select it).First()
            pMatCode = pQry_Mat.fldCode

            Dim pCount_HT As Integer = (From it In pSealMCSDB.tblHT
                                            Where it.fldMatCode = pMatCode Select it).Count()
            Dim pCode As String = ""
            If (pCount_HT > 0) Then
                Dim pQry_HT = (From it In pSealMCSDB.tblHT
                                Where it.fldMatCode = pMatCode Select it).First()
                cmbHT.Items.Add(pQry_HT.fldCode)
                cmbHT.SelectedIndex = 0
            End If
        Else
            cmbHT.SelectedIndex = -1
        End If
    End Sub


#End Region


#End Region

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '============================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
            Me.Close()
        Else
            Me.Close()
        End If

    End Sub


    Private Sub cmdDetailDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                      Handles cmdDetailDesign.Click
        '============================================================================================

        '....Exiting the form and hence, save data.
        SaveData()

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            Dim pfrmDesignChoiceESeal As New IPE_frmDesignChoiceESeal()
            pfrmDesignChoiceESeal.ShowDialog()

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "C-Seal" Then
            Dim pfrmDesignCSeal As New IPE_frmDesignCSeal()
            pfrmDesignCSeal.ShowDialog()

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
            Dim pfrmDesignUSeal As New IPE_frmDesignUSeal()
            pfrmDesignUSeal.ShowDialog()
        End If

    End Sub


    Private Sub SaveData()
        '=================
        '....This routine saves a particular data, only if it has been changed since the entry.    

        '....Assign an appropriate Case No.
        Dim iCase As Int16
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal Is Nothing = True Then
            iCase = 1

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal Is Nothing = False Then

            If cmbType.Text <> gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type Then
                '....First derefernce the active object and then add appropriate  
                '........reference to it in the subsequent statements.
                gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal = Nothing
                gIPE_SealCandidates = Nothing

                iCase = 1

            Else
                iCase = 2

            End If

        End If


        'Save data accroding to the Case No. assigned above:
        '---------------------------------------------------
        Dim pUnitSystem As String = gIPE_Unit.System
        Dim pPOrient As String = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient

        Select Case iCase

            Case 1

                If cmbType.Text = "E-Seal" Then
                    gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal = New IPE_clsESeal("E-Seal", pUnitSystem, pPOrient)

                ElseIf cmbType.Text = "C-Seal" Then
                    gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal = New IPE_clsCSeal("C-Seal", pUnitSystem, pPOrient)

                ElseIf cmbType.Text = "U-Seal" Then
                    gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal = New IPE_clsUSeal("U-Seal", pUnitSystem, pPOrient)
                End If

            Case 2

                '....No data saving necessary as there has been no change on this form.
        End Select


        gIPE_SealCandidates = New IPE_clsSealCandidates(cmbType.Text, gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        '....When the frmDesign*Seal is opened, "AutoSelect" should be checked. 
        gDisplay_Candidate_CrossSecs = True


        'The Seal Design has been changed. The candidate seal designs in the 
        '....appropriate working database table should be updated.
        '-------------------------------------------------------------------
        gUpdate_Candidate_CrossSecs = True


        'SECONDARY ASSIGNMENTS:
        '----------------------
        Dim i As Int16
        For i = 1 To 2
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
            gIPE_SealCandidates.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
        Next

        mMat.Name = cmbMatName.Text
        mMat.HT = ConvertToInt(cmbHT.Text)

        If chkCoating.Checked Then
            mMat.Coating = cmbCoating.Text

        Else
            mMat.Coating = "None"
        End If

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.IsSegmented = mSeal.IsSegmented
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.CountSegment = mSeal.CountSegment

        If (gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal") Then
            If (mMat.Coating = "T800") Then
                CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).SFinish = cmbSFinish.Text 'CType(mSeal, IPE_clsESeal).SFinish
            End If

            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating = mMat.Coating
        End If

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Name = mMat.Name
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.HT = mMat.HT

    End Sub

#End Region


End Class
