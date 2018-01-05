'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmDesignNewESeal                      '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  22SEP16                                '
'                                                                              '
'===============================================================================
'
Imports System.Math
Imports System.IO
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports System.Linq
Imports clsLibrary11
Imports SealIPELib = SealIPELib101


Public Class IPE_frmDesignESeal_New
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mESeal As IPE_clsESeal '....Local Seal object.
    Private mfrmDesignCentreESeal As IPE_frmDesignCenterESeal
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents lblZClear As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbCompressTolType As System.Windows.Forms.ComboBox
    Public WithEvents lblComp As System.Windows.Forms.Label
    Private mMargin(4) As Single

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
    Public WithEvents cmbCrossSecNo_New As System.Windows.Forms.ComboBox
    Friend WithEvents lblOrgText As System.Windows.Forms.Label
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblUnitUserL As System.Windows.Forms.Label
    Public WithEvents lblDControl As System.Windows.Forms.Label
    Public WithEvents lblWid As System.Windows.Forms.Label
    Public WithEvents lblHFree As System.Windows.Forms.Label
    Friend WithEvents lblSplitter1 As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtHFreeTolPlus As System.Windows.Forms.TextBox
    Public WithEvents txtHFreeTolMinus As System.Windows.Forms.TextBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents cmbCrossSecOrg As System.Windows.Forms.ComboBox
    Friend WithEvents lblSpliter2 As System.Windows.Forms.Label
    Friend WithEvents cmdDesignCenter As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents toolTipUpdateHFree As System.Windows.Forms.ToolTip
    Friend WithEvents lblCrossSecNew As System.Windows.Forms.Label
    Friend WithEvents txtTemplate As System.Windows.Forms.TextBox
    Friend WithEvents lblNote As System.Windows.Forms.Label
    Friend WithEvents lblCrossSecOrg As System.Windows.Forms.Label
    Public WithEvents txtCrossSecNo_New As System.Windows.Forms.TextBox    '....Cross-section no. on comboBox. 'PB 13MAR08
    Friend WithEvents picThumbnail As System.Windows.Forms.PictureBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents lblThick As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmDesignESeal_New))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbCompressTolType = New System.Windows.Forms.ComboBox()
        Me.lblComp = New System.Windows.Forms.Label()
        Me.lblZClear = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblOrgText = New System.Windows.Forms.Label()
        Me.cmbCrossSecNo_New = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblThick = New System.Windows.Forms.Label()
        Me.picThumbnail = New System.Windows.Forms.PictureBox()
        Me.txtCrossSecNo_New = New System.Windows.Forms.TextBox()
        Me.lblCrossSecOrg = New System.Windows.Forms.Label()
        Me.lblNote = New System.Windows.Forms.Label()
        Me.txtTemplate = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmdDesignCenter = New System.Windows.Forms.Button()
        Me.lblSpliter2 = New System.Windows.Forms.Label()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblHFree = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtHFreeTolPlus = New System.Windows.Forms.TextBox()
        Me.txtHFreeTolMinus = New System.Windows.Forms.TextBox()
        Me.lblUnitUserL = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblDControl = New System.Windows.Forms.Label()
        Me.lblWid = New System.Windows.Forms.Label()
        Me.lblSplitter1 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCrossSecOrg = New System.Windows.Forms.ComboBox()
        Me.lblCrossSecNew = New System.Windows.Forms.Label()
        Me.toolTipUpdateHFree = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.picThumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblBorder.Size = New System.Drawing.Size(577, 340)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.cmbCompressTolType)
        Me.Panel1.Controls.Add(Me.lblComp)
        Me.Panel1.Controls.Add(Me.lblZClear)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.lblOrgText)
        Me.Panel1.Controls.Add(Me.cmbCrossSecNo_New)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.lblThick)
        Me.Panel1.Controls.Add(Me.picThumbnail)
        Me.Panel1.Controls.Add(Me.txtCrossSecNo_New)
        Me.Panel1.Controls.Add(Me.lblCrossSecOrg)
        Me.Panel1.Controls.Add(Me.lblNote)
        Me.Panel1.Controls.Add(Me.txtTemplate)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.cmdDesignCenter)
        Me.Panel1.Controls.Add(Me.lblSpliter2)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lblHFree)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtHFreeTolPlus)
        Me.Panel1.Controls.Add(Me.txtHFreeTolMinus)
        Me.Panel1.Controls.Add(Me.lblUnitUserL)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.lblDControl)
        Me.Panel1.Controls.Add(Me.lblWid)
        Me.Panel1.Controls.Add(Me.lblSplitter1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbCrossSecOrg)
        Me.Panel1.Controls.Add(Me.lblCrossSecNew)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(575, 338)
        Me.Panel1.TabIndex = 1
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(378, 4)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(28, 13)
        Me.Label16.TabIndex = 190
        Me.Label16.Text = "Tol."
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(461, 6)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(82, 13)
        Me.Label13.TabIndex = 189
        Me.Label13.Text = "Compression"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbCompressTolType
        '
        Me.cmbCompressTolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompressTolType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCompressTolType.Items.AddRange(New Object() {"Nominal", "Minimum", "Maximum"})
        Me.cmbCompressTolType.Location = New System.Drawing.Point(351, 22)
        Me.cmbCompressTolType.Name = "cmbCompressTolType"
        Me.cmbCompressTolType.Size = New System.Drawing.Size(82, 21)
        Me.cmbCompressTolType.TabIndex = 188
        '
        'lblComp
        '
        Me.lblComp.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblComp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblComp.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblComp.Location = New System.Drawing.Point(452, 24)
        Me.lblComp.Name = "lblComp"
        Me.lblComp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComp.Size = New System.Drawing.Size(110, 21)
        Me.lblComp.TabIndex = 187
        Me.lblComp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblZClear
        '
        Me.lblZClear.BackColor = System.Drawing.Color.White
        Me.lblZClear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblZClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblZClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZClear.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblZClear.Location = New System.Drawing.Point(501, 205)
        Me.lblZClear.Name = "lblZClear"
        Me.lblZClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblZClear.Size = New System.Drawing.Size(53, 21)
        Me.lblZClear.TabIndex = 184
        Me.lblZClear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(444, 209)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(54, 13)
        Me.Label4.TabIndex = 183
        Me.Label4.Text = " ZClear "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(392, 162)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(2, 122)
        Me.Label2.TabIndex = 181
        '
        'lblOrgText
        '
        Me.lblOrgText.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrgText.Location = New System.Drawing.Point(127, 172)
        Me.lblOrgText.Name = "lblOrgText"
        Me.lblOrgText.Size = New System.Drawing.Size(249, 20)
        Me.lblOrgText.TabIndex = 180
        '
        'cmbCrossSecNo_New
        '
        Me.cmbCrossSecNo_New.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCrossSecNo_New.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCrossSecNo_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCrossSecNo_New.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCrossSecNo_New.Location = New System.Drawing.Point(184, 60)
        Me.cmbCrossSecNo_New.Name = "cmbCrossSecNo_New"
        Me.cmbCrossSecNo_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCrossSecNo_New.Size = New System.Drawing.Size(100, 21)
        Me.cmbCrossSecNo_New.TabIndex = 179
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(39, 253)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(63, 13)
        Me.Label14.TabIndex = 178
        Me.Label14.Text = "Thickness"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblThick
        '
        Me.lblThick.BackColor = System.Drawing.Color.White
        Me.lblThick.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblThick.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblThick.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThick.ForeColor = System.Drawing.Color.Magenta
        Me.lblThick.Location = New System.Drawing.Point(108, 249)
        Me.lblThick.Name = "lblThick"
        Me.lblThick.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThick.Size = New System.Drawing.Size(51, 21)
        Me.lblThick.TabIndex = 177
        Me.lblThick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'picThumbnail
        '
        Me.picThumbnail.BackColor = System.Drawing.Color.AliceBlue
        Me.picThumbnail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picThumbnail.Location = New System.Drawing.Point(7, 24)
        Me.picThumbnail.Name = "picThumbnail"
        Me.picThumbnail.Size = New System.Drawing.Size(114, 121)
        Me.picThumbnail.TabIndex = 166
        Me.picThumbnail.TabStop = False
        '
        'txtCrossSecNo_New
        '
        Me.txtCrossSecNo_New.AcceptsReturn = True
        Me.txtCrossSecNo_New.BackColor = System.Drawing.SystemColors.Window
        Me.txtCrossSecNo_New.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCrossSecNo_New.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrossSecNo_New.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCrossSecNo_New.Location = New System.Drawing.Point(184, 114)
        Me.txtCrossSecNo_New.MaxLength = 0
        Me.txtCrossSecNo_New.Name = "txtCrossSecNo_New"
        Me.txtCrossSecNo_New.ReadOnly = True
        Me.txtCrossSecNo_New.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCrossSecNo_New.Size = New System.Drawing.Size(64, 21)
        Me.txtCrossSecNo_New.TabIndex = 165
        Me.txtCrossSecNo_New.Visible = False
        '
        'lblCrossSecOrg
        '
        Me.lblCrossSecOrg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSecOrg.Location = New System.Drawing.Point(127, 29)
        Me.lblCrossSecOrg.Name = "lblCrossSecOrg"
        Me.lblCrossSecOrg.Size = New System.Drawing.Size(52, 14)
        Me.lblCrossSecOrg.TabIndex = 164
        Me.lblCrossSecOrg.Text = "Original"
        Me.lblCrossSecOrg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblNote
        '
        Me.lblNote.AutoSize = True
        Me.lblNote.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNote.ForeColor = System.Drawing.Color.Red
        Me.lblNote.Location = New System.Drawing.Point(9, 243)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(0, 15)
        Me.lblNote.TabIndex = 71
        '
        'txtTemplate
        '
        Me.txtTemplate.BackColor = System.Drawing.Color.White
        Me.txtTemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTemplate.Location = New System.Drawing.Point(279, 25)
        Me.txtTemplate.Name = "txtTemplate"
        Me.txtTemplate.ReadOnly = True
        Me.txtTemplate.Size = New System.Drawing.Size(46, 21)
        Me.txtTemplate.TabIndex = 70
        Me.txtTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(192, 6)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(54, 17)
        Me.Label12.TabIndex = 68
        Me.Label12.Text = "Number"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(270, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 16)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "Template"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdDesignCenter
        '
        Me.cmdDesignCenter.BackColor = System.Drawing.Color.Silver
        Me.cmdDesignCenter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDesignCenter.Image = CType(resources.GetObject("cmdDesignCenter.Image"), System.Drawing.Image)
        Me.cmdDesignCenter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDesignCenter.Location = New System.Drawing.Point(439, 105)
        Me.cmdDesignCenter.Name = "cmdDesignCenter"
        Me.cmdDesignCenter.Size = New System.Drawing.Size(125, 30)
        Me.cmdDesignCenter.TabIndex = 65
        Me.cmdDesignCenter.Text = "      Design Center"
        Me.cmdDesignCenter.UseVisualStyleBackColor = False
        '
        'lblSpliter2
        '
        Me.lblSpliter2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSpliter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSpliter2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpliter2.Location = New System.Drawing.Point(0, 282)
        Me.lblSpliter2.Name = "lblSpliter2"
        Me.lblSpliter2.Size = New System.Drawing.Size(610, 2)
        Me.lblSpliter2.TabIndex = 64
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(391, 300)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 8
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
        Me.cmdCancel.Location = New System.Drawing.Point(482, 300)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(426, 253)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 13)
        Me.Label11.TabIndex = 60
        Me.Label11.Text = "Control Dia"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(277, 207)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 16)
        Me.Label10.TabIndex = 59
        Me.Label10.Text = "Width"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblHFree
        '
        Me.lblHFree.BackColor = System.Drawing.Color.White
        Me.lblHFree.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblHFree.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHFree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHFree.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblHFree.Location = New System.Drawing.Point(110, 205)
        Me.lblHFree.Name = "lblHFree"
        Me.lblHFree.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHFree.Size = New System.Drawing.Size(53, 21)
        Me.lblHFree.TabIndex = 58
        Me.lblHFree.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(168, 219)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(23, 16)
        Me.Label9.TabIndex = 57
        Me.Label9.Text = "(--)"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(169, 195)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(22, 16)
        Me.Label8.TabIndex = 56
        Me.Label8.Text = "(+)"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(12, 207)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(90, 16)
        Me.Label7.TabIndex = 55
        Me.Label7.Text = "Free Height"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtHFreeTolPlus
        '
        Me.txtHFreeTolPlus.AcceptsReturn = True
        Me.txtHFreeTolPlus.BackColor = System.Drawing.Color.White
        Me.txtHFreeTolPlus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHFreeTolPlus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFreeTolPlus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHFreeTolPlus.Location = New System.Drawing.Point(191, 195)
        Me.txtHFreeTolPlus.MaxLength = 0
        Me.txtHFreeTolPlus.Name = "txtHFreeTolPlus"
        Me.txtHFreeTolPlus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHFreeTolPlus.Size = New System.Drawing.Size(54, 21)
        Me.txtHFreeTolPlus.TabIndex = 6
        Me.txtHFreeTolPlus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtHFreeTolMinus
        '
        Me.txtHFreeTolMinus.AcceptsReturn = True
        Me.txtHFreeTolMinus.BackColor = System.Drawing.Color.White
        Me.txtHFreeTolMinus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHFreeTolMinus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHFreeTolMinus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHFreeTolMinus.Location = New System.Drawing.Point(191, 219)
        Me.txtHFreeTolMinus.MaxLength = 0
        Me.txtHFreeTolMinus.Name = "txtHFreeTolMinus"
        Me.txtHFreeTolMinus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHFreeTolMinus.Size = New System.Drawing.Size(54, 21)
        Me.txtHFreeTolMinus.TabIndex = 7
        Me.txtHFreeTolMinus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblUnitUserL
        '
        Me.lblUnitUserL.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblUnitUserL.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnitUserL.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitUserL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnitUserL.Location = New System.Drawing.Point(86, 170)
        Me.lblUnitUserL.Name = "lblUnitUserL"
        Me.lblUnitUserL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnitUserL.Size = New System.Drawing.Size(40, 17)
        Me.lblUnitUserL.TabIndex = 51
        Me.lblUnitUserL.Text = "(in)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(12, 171)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(76, 13)
        Me.Label6.TabIndex = 50
        Me.Label6.Text = "Length Unit:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDControl
        '
        Me.lblDControl.BackColor = System.Drawing.Color.White
        Me.lblDControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDControl.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDControl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDControl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblDControl.Location = New System.Drawing.Point(501, 249)
        Me.lblDControl.Name = "lblDControl"
        Me.lblDControl.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDControl.Size = New System.Drawing.Size(53, 21)
        Me.lblDControl.TabIndex = 44
        Me.lblDControl.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblWid
        '
        Me.lblWid.BackColor = System.Drawing.Color.White
        Me.lblWid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWid.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWid.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblWid.Location = New System.Drawing.Point(323, 205)
        Me.lblWid.Name = "lblWid"
        Me.lblWid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWid.Size = New System.Drawing.Size(53, 21)
        Me.lblWid.TabIndex = 41
        Me.lblWid.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSplitter1
        '
        Me.lblSplitter1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSplitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSplitter1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSplitter1.Location = New System.Drawing.Point(0, 162)
        Me.lblSplitter1.Name = "lblSplitter1"
        Me.lblSplitter1.Size = New System.Drawing.Size(610, 2)
        Me.lblSplitter1.TabIndex = 39
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 15)
        Me.Label1.TabIndex = 33
        Me.Label1.Text = "Cross Section:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbCrossSecOrg
        '
        Me.cmbCrossSecOrg.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCrossSecOrg.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCrossSecOrg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCrossSecOrg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCrossSecOrg.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCrossSecOrg.Location = New System.Drawing.Point(184, 25)
        Me.cmbCrossSecOrg.Name = "cmbCrossSecOrg"
        Me.cmbCrossSecOrg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCrossSecOrg.Size = New System.Drawing.Size(70, 21)
        Me.cmbCrossSecOrg.TabIndex = 1
        '
        'lblCrossSecNew
        '
        Me.lblCrossSecNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCrossSecNew.ForeColor = System.Drawing.Color.Black
        Me.lblCrossSecNew.Location = New System.Drawing.Point(139, 64)
        Me.lblCrossSecNew.Name = "lblCrossSecNew"
        Me.lblCrossSecNew.Size = New System.Drawing.Size(40, 16)
        Me.lblCrossSecNew.TabIndex = 69
        Me.lblCrossSecNew.Text = "New"
        Me.lblCrossSecNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'IPE_frmDesignESeal_New
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(582, 345)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmDesignESeal_New"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "E-Seal New Design"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.picThumbnail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New()
        '===========
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'AES 20SEP16
        With cmbCompressTolType
            .Items().Clear()
            .Items.Add("Nominal")
            .Items.Add("Minimum")
            .Items.Add("Maximum")
        End With

    End Sub


#Region "FORM EVENT ROUTINES:"


    Private Sub frmDesignNewESeal_Load(ByVal sender As Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles MyBase.Load
        '=============================================================

        '....This form load event is triggered only when the form is displayed by
        '........"Show Dialog", not when it is created and neither when control 
        '........comes back to this form after a subordinate form closes.
        '
        '
        '   Set Margin of the Picture Box.   
        Dim pUniformMargin As Single = 0.05       '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        InitializeControls()

        '....Create & initialize the local seal object.
        InitializeLocalObject()                   '....gSeal ===> mESeal.

        '....Polpulate Orginal Cross-section ComboBox.
        PopulateCmbCrossSec()


        '....Set Design Center Button.
        '........The "Design Center" command button is enabled only when 
        '............1. Privilege = "Engineering",
        '............2. Template  = "1Gen" or "1GenS" and
        '............3. Unit      = "English"
        '
        Set_Control_DesignCenter()

        '....Refresh Display upon entry to the form.
        '........Local seal object "mSeal" is used.
        DisplayData()

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            cmbCrossSecOrg.Enabled = False
            cmbCrossSecNo_New.Enabled = False

            txtHFreeTolMinus.Enabled = False
            txtHFreeTolPlus.Enabled = False
            cmbCompressTolType.Enabled = False

        Else
            cmbCrossSecOrg.Enabled = True
            cmbCrossSecNo_New.Enabled = True

            txtHFreeTolMinus.Enabled = True
            txtHFreeTolPlus.Enabled = True
            cmbCompressTolType.Enabled = True
        End If

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        lblUnitUserL.Text = "(" & gIPE_Unit.UserL & ")"
        lblOrgText.Visible = False
    End Sub


    Private Sub InitializeLocalObject()
        '=============================
        '....From gSeal ===> mESeal. 
        '........Now onwards, mESeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal only in the 
        '........"SaveData" routine which is called when the form is exited and/or another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local E-Seal Object. 
        mESeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        If gIPE_SealOrg Is Nothing Then
            gIPE_SealOrg = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        End If

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo = "" Then

            '....PRIMARY ASSIGNMENTS are not done.
            '........Initialize with the first cross-section of the list.
            mESeal.MCrossSecNo = IPE_clsESeal.CrossSecList(0)

            '....SECONDARY ASSIGNMENTS:
            Dim i As Int16
            For i = 1 To 2

                If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity Is Nothing = False Then
                    mESeal.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
                End If
            Next

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo <> "" Then
            mESeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone
            gIPE_SealOrg = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone()
        End If

    End Sub


    Private Sub PopulateCmbCrossSec()
        '============================       
        '....This routine populates the Cross Sec No. combo box. (Database Driven).
        'Dim pSealDBWorkingEntities As New SealDBWorkingEntities()

        'Dim pQryESealCandidate = (From it In pSealDBWorkingEntities.tblESealCandidate
        '                          Order By it.fldCrossSecNo Ascending Select it).Distinct()
        'Dim pRecord As New tblESealCandidate
        'cmbCrossSecOrg.Items.Clear()
        'For Each pRecord In pQryESealCandidate
        '    cmbCrossSecOrg.Items.Add(pRecord.fldCrossSecNo)
        'Next

        'AES 28MAR16
        Dim pSealMCSDB As New SealIPEMCSDBEntities

        'Dim pQryESeal = (From it In pSealDBEntities.tblESeal_Geom
        '                          Order By it.fldCrossSecNo Ascending Select it).Distinct()

        Dim pQryESeal = (From it In pSealMCSDB.tblESeal_Geom
                                  Select it.fldCrossSecNo Distinct).ToList()

        'Dim pRecord As New tblESeal_Geom
        cmbCrossSecOrg.Items.Clear()
        For i As Integer = 0 To pQryESeal.Count() - 1
            cmbCrossSecOrg.Items.Add(pQryESeal(i))
        Next

    End Sub


    Private Sub DisplayData()
        '====================
        '....This routine displays the current state of the local seal object "mSeal". 

        With mESeal
            cmbCrossSecOrg.Text = .CrossSecNoOrg      '...."SelectedIndexChanged" Event.

            If .TemplateNo.Contains("1Gen") Then
                If .NewDesign Then
                    cmbCrossSecNo_New.Text = .MCrossSecNo
                Else
                    cmbCrossSecNo_New.SelectedIndex = 0
                End If
            End If

        End With

        '....Display all the relevant length parameters:        
        '........Adjusted, HfreeStd & Tolerances, Wid & DControl.    
        DisplayLengthParams()

        'AES 20SEP16
        'Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        'cmbCompressTolType.Text = gIPE_Project.Analysis(pICur).Compression.TolType
        'Dim pVal As Double = mESeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
        'Dim pPCentVal As Double = (pVal / mESeal.Hfree) * 100.0#

        'lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"


        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        cmbCompressTolType.Text = gIPE_Project.Analysis(pICur).Compression.TolType
        DisplayCompressionVal()

    End Sub

#Region "SUB-HELPER ROUTINES:"

    Private Sub DisplayLengthParams()
        ''============================

        With mESeal
            lblHFree.Text = gIPE_Unit.WriteInUserL(.Hfree)
            lblHFree.BackColor = Color.Gainsboro
            txtHFreeTolMinus.Text = gIPE_Unit.WriteInUserL(.HFreeTol(1))
            txtHFreeTolPlus.Text = gIPE_Unit.WriteInUserL(.HFreeTol(2))

            lblWid.Text = gIPE_Unit.WriteInUserL(.Wid)
            lblWid.BackColor = Color.Gainsboro

            If (Math.Abs(mESeal.ZClear_Given) > gcEPS) Then
                lblZClear.Text = gIPE_Unit.WriteInUserL(mESeal.ZClear_Given)
                lblZClear.ForeColor = Color.Black
                lblZClear.BackColor = Color.Gainsboro
            Else
                lblZClear.Text = gIPE_Unit.WriteInUserL(.ZClear)
                lblZClear.ForeColor = Color.Blue
                lblZClear.BackColor = Color.Gainsboro
            End If

            lblDControl.Text = gIPE_Unit.WriteInUserL(.DControl)
            lblDControl.BackColor = Color.Gainsboro

            lblThick.Text = gIPE_Unit.WriteInUserL(.T)
        End With

    End Sub

#End Region

#End Region


#End Region


#Region "CONTROL EVENT ROUTINES:"

#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtHFreeTolMinus_TextChanged(ByVal sender As Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles txtHFreeTolMinus.TextChanged
        '=========================================================================
        '....i = 1 : Minus Value
        mESeal.HFreeTol(1) = gIPE_Unit.L_UserToCon(txtHFreeTolMinus.Text)
        SetForeColor_HfreeTol()
        DisplayCompressionVal() 'AES 22SEP16

    End Sub


    Private Sub txtHFreeTolPlus_TextChanged(ByVal sender As Object, _
                                            ByVal e As System.EventArgs) _
                                            Handles txtHFreeTolPlus.TextChanged
        '=========================================================================
        '....i = 2 : Plus Value
        mESeal.HFreeTol(2) = gIPE_Unit.L_UserToCon(txtHFreeTolPlus.Text)
        SetForeColor_HfreeTol()
        DisplayCompressionVal() 'AES 22SEP16

    End Sub


    Private Sub SetForeColor_HfreeTol()
        '==============================                 

        With mESeal

            Dim pColor As Color

            pColor = IIf(Abs(.HFreeTol(1) - .HfreeTolStd) < gcEPS, _
                                             Color.Magenta, Color.Black)
            txtHFreeTolMinus.ForeColor = pColor


            pColor = IIf(Abs(.HFreeTol(2) - .HfreeTolStd) < gcEPS, _
                                            Color.Magenta, Color.Black)
            txtHFreeTolPlus.ForeColor = pColor

        End With

    End Sub

#End Region


#Region "COMBO-BOX RELATED CONTROLS:"


    Private Sub cmbCrossSecOrg_SelectedIndexChanged(ByVal sender As System.Object, _
                                                    ByVal e As System.EventArgs) _
                                                    Handles cmbCrossSecOrg.SelectedIndexChanged
        '======================================================================================
        If mESeal Is Nothing = True Then Exit Sub

        With mESeal

            If cmbCrossSecOrg.Text <> .CrossSecNoOrg Then
                .MCrossSecNo = cmbCrossSecOrg.Text       '....Sets .NewDesign = False.
            End If

            txtTemplate.Text = .TemplateNo
            If .TemplateNo.Contains("1Gen") Then
                PopulateCmbCrossSec_New()
                If .NewDesign Then
                    cmbCrossSecNo_New.Text = .MCrossSecNo
                Else
                    If (cmbCrossSecNo_New.Items.Count > 0) Then
                        cmbCrossSecNo_New.SelectedIndex = 0
                    End If

                End If
            End If

            DisplayLengthParams()      '....Update Length Parameters Display.


            txtTemplate.ForeColor = Color.Magenta       '....Retrive from DB.   
            txtTemplate.BackColor = Color.Gainsboro     '....Not overridable.

            Set_Control_DesignCenter()

            DoGraphics()

            'AES 20SEP16
            'Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
            'Dim pVal As Double = mESeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
            'Dim pPCentVal As Double = (pVal / mESeal.Hfree) * 100.0#

            'lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

            DisplayCompressionVal()

        End With

    End Sub


    Private Sub cmbCrossSecNo_New_SelectedIndexChanged(ByVal sender As System.Object, _
                                                       ByVal e As System.EventArgs) _
                                                       Handles cmbCrossSecNo_New.SelectedIndexChanged
        '=============================================================================================
        Dim pCrossSecNoOrg As String = cmbCrossSecOrg.Text.Trim()
        Dim pCrossSecNoNew As String = cmbCrossSecNo_New.Text.Trim()

        lblOrgText.Visible = False

        With mESeal
            .MCrossSecNo = pCrossSecNoNew
            .CrossSecNoOrg = pCrossSecNoOrg
        End With

        If CType(gIPE_SealOrg, IPE_clsESeal).MCrossSecNo <> mESeal.CrossSecNoOrg Then
            CType(gIPE_SealOrg, IPE_clsESeal).MCrossSecNo = mESeal.CrossSecNoOrg
        End If

        DisplayLengthParams()      '....Update Length Parameters Display.

        DoGraphics()

        'AES 20SEP16
        'Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        'Dim pVal As Double = mESeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
        'Dim pPCentVal As Double = (pVal / mESeal.Hfree) * 100.0#

        'lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

        DisplayCompressionVal()

    End Sub


    Private Sub cmbCrossSecNo_New_TextChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles cmbCrossSecNo_New.TextChanged
        '============================================================================
        Dim pCrossSecNo As String = cmbCrossSecNo_New.Text.Trim()

        If IPE_clsESeal.CrossSecNewList.Count > 0 And _
                Not IPE_clsESeal.CrossSecNewList.Contains(pCrossSecNo) And _
                    Not IPE_clsESeal.CrossSecList.Contains(pCrossSecNo) Then

            lblCrossSecNew.ForeColor = Color.Green

            mESeal = CType(gIPE_SealOrg, IPE_clsESeal).Clone()
            DisplayLengthParams()                       '....Update Length Parameters Display.

            DoGraphics()

            With lblOrgText
                .Visible = True
                .Text = "Original CrossSection"
            End With
        Else
            lblCrossSecNew.ForeColor = Color.Black
            cmbCrossSecNo_New_SelectedIndexChanged(sender, e)

        End If

        'AES 20SEP16
        'Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        'Dim pVal As Double = mESeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
        'Dim pPCentVal As Double = (pVal / mESeal.Hfree) * 100.0#

        'lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

        DisplayCompressionVal()

    End Sub


    Private Sub cmbCompressTolType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                       Handles cmbCompressTolType.SelectedIndexChanged
        '=================================================================================================
        DisplayCompressionVal()

    End Sub

#Region "HELPER ROUTINES:"

    '....New Cross Section Number
    Private Sub PopulateCmbCrossSec_New()
        '================================       
        Dim pSealMCSDB As New SealIPEMCSDBEntities

        Dim pCrossSecNo As String = cmbCrossSecOrg.Text.Trim()
        Dim pQryESealCandidate = (From pRec In pSealMCSDB.tblESealNew_Geom
                                    Where pRec.fldCustID = gIPE_Project.Customer_ID And
                                          pRec.fldPlatformID = gIPE_Project.Platform_ID And
                                          pRec.fldProjectID = gIPE_Project.Project_ID And
                                          pRec.fldCrossSecNoOrg = pCrossSecNo
                                    Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
        Dim pRecord As tblESealNew_Geom
        cmbCrossSecNo_New.Items.Clear()
        For Each pRecord In pQryESealCandidate
            cmbCrossSecNo_New.Items.Add(pRecord.fldCrossSecNo)
        Next

    End Sub


    Private Sub DisplayCompressionVal()
        '==============================
        'AES 22SEP16
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        Dim pVal As Double = mESeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
        Dim pPCentVal As Double = (pVal / mESeal.Hfree) * 100.0#

        lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

    End Sub

#End Region


#End Region
    ' 
#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdDesignCenter_Click(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles cmdDesignCenter.Click
        '==================================================================
        Dim pblnSave As Boolean
        Check_CrossSecNo(pblnSave)
        If pblnSave = False Then Exit Sub

        '   ORIGINAL SEAL OBJECT:
        '   ---------------------
        '   ....Create it, if it doesn't already exist, and initialize it. 
        '
        If gIPE_SealOrg Is Nothing Then
            gIPE_SealOrg = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        ElseIf gIPE_SealOrg.UnitSystem <> gIPE_Unit.System Then
            gIPE_SealOrg = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
            CType(gIPE_SealOrg, IPE_clsESeal).MCrossSecNo = cmbCrossSecOrg.Text
        End If

        CType(gIPE_SealOrg, IPE_clsESeal).MCrossSecNo = cmbCrossSecOrg.Text


        '    New Seal Object:           
        '   ------------------
        If gIPE_SealNew Is Nothing Then
            gIPE_SealNew = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        ElseIf gIPE_SealNew.Type <> gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type Then
            gIPE_SealNew = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        End If
        gIPE_SealNew = mESeal.Clone()
        CType(gIPE_SealNew, IPE_clsESeal).MCrossSecNo = cmbCrossSecNo_New.Text

        '   Save data.
        '   ----------
        SaveData()

        '....Save the Design State before entering the "Design Centre" FORM.
        Dim pNewDesign_Before As Boolean
        pNewDesign_Before = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).NewDesign

        '   OPEN "Design Centre" FORM. 
        '   --------------------------
        mfrmDesignCentreESeal = New IPE_frmDesignCenterESeal()
        mfrmDesignCentreESeal.ShowDialog()


        '   RETURNED FROM "Design Center":
        '   ------------------------------
        '....Design State on return.
        Dim pNewDesign_After As Boolean
        pNewDesign_After = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).NewDesign

        '   ....Upon returning (may be with a new Cross-section), update the local
        '   ........seal object. gSeal ===> mESeal.
        mESeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone

        If IPE_clsESeal.CrossSecNewList.Count > 0 And _
             Not IPE_clsESeal.CrossSecNewList.Contains(mESeal.MCrossSecNo) Then
            IPE_clsESeal.CrossSecNewList.Add(mESeal.MCrossSecNo)
        End If

        '....See if the Design State has been toggled between "Before" entering into 
        '........and "After" returning from the "Design Centre" FORM.
        '
        '   CASE 1: pNewDesign_Before = FALSE and _After = FALSE. ==> Didn't Toggle.
        '               Basically, the user entered the form with a STD Design and 
        '               returned w/o doing anything by hitting CANCEL.
        '
        '   CASE 2: pNewDesign_Before = TRUE and _After = TRUE. ==> Didn't Toggle.
        '               Basically, the user entered the form with a NEW Design and 
        '               returned with another (OK) or the same NEW design (CANCEL).
        '
        '   CASE 3: pNewDesign_Before = FALSE and _After = TRUE. ==> Did Toggle.
        '               Basically, the user entered the form with a STD Design and 
        '               returned with a NEW design (OK).
        '
        '   CASE 4: pNewDesign_Before = TRUE and _After = FALSE. ==> Did Toggle.
        '               Basically, the user entered the form with a NEW Design and 
        '               returned with a STD (ORG) design (OK).

        DisplayData()

        DoGraphics()
        picThumbnail.Refresh()

    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '===============================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then
            SaveData()

        End If

        Me.Close()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub Check_CrossSecNo(ByRef blnSave_Out As Boolean)
        '======================================================
        '....Initialize.
        blnSave_Out = True

        Dim pCrossSec As String = cmbCrossSecNo_New.Text
        Dim pMsg As String = ""
        Dim pintAnswer As Integer

        If IPE_clsESeal.CrossSecList.Contains(pCrossSec) Then
            '....Exists in STD CrossSection List
            pMsg = "The NEW CrossSection No. must not be same as the existing STD CrossSection no." & _
                   vbCrLf & "Please assign an appropriate number."
            pintAnswer = MessageBox.Show(pMsg, "Data Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            If pintAnswer = Windows.Forms.DialogResult.OK Then
                cmbCrossSecNo_New.Focus()
                blnSave_Out = False
                Exit Sub
            End If

        End If

    End Sub


    Private Sub SaveData()
        '================

        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

        'MAIN SEAL OBJECT:
        '-----------------
        '
        With CType(gIPE_Project.Analysis(pICur).Seal, IPE_clsESeal)

            If Not .NewDesign And Not mESeal.NewDesign Then
                '   CASE 1:  "Standard Design"
                '    ------
                If .MCrossSecNo <> cmbCrossSecOrg.Text Then .MCrossSecNo = cmbCrossSecOrg.Text


            ElseIf .NewDesign And mESeal.NewDesign Then
                '   CASE 2: "New Design"
                '   -------
                '   ....The following conditions will be automatically satisfied:
                '        i) .CrossSecNoOrg = cmbCrossSec.Text 
                '       ii) .CrossSecNo    = txtCrossSecNo_New (Read-only text box).
                '
                '   ....No need to do anything.
                .MCrossSecNo = cmbCrossSecNo_New.Text
                .CrossSecNoOrg = cmbCrossSecOrg.Text

            ElseIf .NewDesign And Not mESeal.NewDesign Then
                '   CASE 3: Toggled from "New Design" to "Standard Design" on the form.
                '   ------
                .MCrossSecNo = cmbCrossSecOrg.Text

            ElseIf Not .NewDesign And mESeal.NewDesign Then
                '   CASE 4: Toggled from "Standard Design" to "New Design" on the form.
                '   --------
                .MCrossSecNo = cmbCrossSecNo_New.Text
                .CrossSecNoOrg = cmbCrossSecOrg.Text
            End If


            .HFreeTol(1) = gIPE_Unit.L_UserToCon(txtHFreeTolMinus.Text)  '....Minus Value
            .HFreeTol(2) = gIPE_Unit.L_UserToCon(txtHFreeTolPlus.Text)   '....Plus Value

        End With


        'SECONDARY ASSIGNMENTS:
        '----------------------                    
        If gIPE_Project.Analysis(pICur).Seal Is Nothing = False Then
            gIPE_Project.Analysis(pICur).Cavity.CornerRad = IIf(Abs(gIPE_Project.Analysis(pICur).Cavity.CornerRad - gIPE_Project.Analysis(pICur).Seal.CavityCornerRad) < gcEPS, _
                                                           gIPE_Project.Analysis(pICur).Seal.CavityCornerRad, gIPE_Project.Analysis(pICur).Cavity.CornerRad)
        End If

        'AES 20SEP16
        gIPE_Project.Analysis(pICur).CompressionTolType = cmbCompressTolType.Text

    End Sub
#End Region

#End Region


#Region "UTILITY ROUTINES: FORM SETTING"


    Private Sub Set_Control_DesignCenter()
        '================================
        With gIPE_User

            cmdDesignCenter.Visible = True

            '...."Design Center" is available for Templates "1Gen" & "1GenS" and for
            '........"English" unit only.
            '
            With mESeal

                Dim pblnCmd As Boolean = False
                Dim pblnNote As Boolean = False

                If mESeal.TemplateNo.Contains("1Gen") Then

                    If gIPE_Unit.System = "English" Then
                        pblnCmd = True
                        lblNote.Text = ""

                    ElseIf gIPE_Unit.System = "Metric" Then
                        pblnCmd = True
                    End If

                End If

                cmdDesignCenter.Enabled = pblnCmd
                lblNote.Visible = pblnNote

                lblCrossSecNew.Visible = pblnCmd
                cmbCrossSecNo_New.Visible = pblnCmd

            End With

        End With


    End Sub

#End Region


#Region "GRAPHICS ROUTINES:"

    Public Sub DoGraphics()
        '===================

        'This routine draws the 'Standard' & 'Adjusted' Geometries.

        '....Drawing envelope:
        Dim EnvpTopL As PointF
        Dim EnvpBotR As PointF


        'Graphics Settings:
        '------------------
        '....Array Index = 0 ===> "Standard Geometry"
        '....Array Index = 1 ===> "Adjusted Geometry"

        '....Color:
        Dim pColor(1) As Color
        pColor(0) = Color.Black
        pColor(1) = Color.Blue

        '....Drawing Width (Pixels)  
        Dim pDrawWid(1) As Integer
        pDrawWid(0) = 1
        pDrawWid(1) = 1     '....Width = 1 here doesn't work, nor necessary here 04JUL06.

        '....Dash Style:
        Dim pDashStyle(1) As Integer
        pDashStyle(0) = DashStyle.Solid     '....Value = 0
        pDashStyle(1) = DashStyle.DashDot   '....Value = 1    


        'Draw the seals.
        '---------------
        Dim pGr As Graphics = GetGraphicsObj(picThumbnail)

        '....Pixel densities per unit "PageUnit" dimension (in or mm)
        Dim pDpX As Single
        Dim pDpY As Single

        '....Set the PageUnit property.
        If gIPE_Unit.System = "English" Then
            pGr.PageUnit = GraphicsUnit.Inch

            '....# of Pixels/in
            pDpX = pGr.DpiX
            pDpY = pGr.DpiY

        ElseIf gIPE_Unit.System = "Metric" Then
            pGr.PageUnit = GraphicsUnit.Millimeter

            '....# of Pixels/mm
            pDpX = pGr.DpiX / gIPE_Unit.EngLToUserL(1.0)
            pDpY = pGr.DpiY / gIPE_Unit.EngLToUserL(1.0)
        End If


        '....Size of the graphics area in the "page unit" system.
        Dim pSize As New SizeF(picThumbnail.Width / pDpX, picThumbnail.Height / pDpY)

        '....Draw both "Standard" & "Adjusted" Seal Geometry.        
        mESeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                    "STD", "SCALE_BY_STD", 1.25, _
                                    EnvpTopL, EnvpBotR)


    End Sub


    Private Function GetGraphicsObj(ByVal picBox As PictureBox) As Graphics
        '==================================================================
        Dim pBmap As Bitmap
        pBmap = New Bitmap(picBox.Width, picBox.Height)
        picBox.Image = pBmap

        Dim pGr As Graphics
        pGr = Graphics.FromImage(pBmap)
        pGr.Clear(picBox.BackColor)

        Return pGr

    End Function


#End Region


End Class



