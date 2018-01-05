'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmDesignESeal                         '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY17                                '
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


Public Class IPE_frmDesignESeal
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    '....Shared Variables:  Initialize.
    Private Shared mFormOpened_FirstTime As Boolean = True
    Private Shared mSuccess_Candidate_CrossSecs As Boolean = False

    Private mFlag_DontFitCavity_Msg As Boolean = False

    Private mMargin(4) As Single
    Private mESeal As IPE_clsESeal           '....Local Seal object.    

    Private mCrossSecList As New List(Of String)
    Public WithEvents lblZClear As System.Windows.Forms.Label
    Public WithEvents lblComp As System.Windows.Forms.Label
    Friend WithEvents cmbCompressTolType As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Private mCandidateList As New List(Of Boolean)

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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents lblAdjusted As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents chkAutoSelect As System.Windows.Forms.CheckBox
    Public WithEvents cmdForm_SelectionCriteria As System.Windows.Forms.Button
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
    Public WithEvents cmbCrossSec As System.Windows.Forms.ComboBox
    Friend WithEvents cmdAdjustGeometry As System.Windows.Forms.Button
    Friend WithEvents lblSpliter2 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents toolTipUpdateHFree As System.Windows.Forms.ToolTip
    Friend WithEvents lblNote As System.Windows.Forms.Label
    Friend WithEvents lblShowCriteria As System.Windows.Forms.Label
    Friend WithEvents picThumbnail As System.Windows.Forms.PictureBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents lblThick As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label

    '....Cross-section no. on comboBox. 'PB 13MAR08

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmDesignESeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbCompressTolType = New System.Windows.Forms.ComboBox()
        Me.lblComp = New System.Windows.Forms.Label()
        Me.lblZClear = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblThick = New System.Windows.Forms.Label()
        Me.picThumbnail = New System.Windows.Forms.PictureBox()
        Me.lblShowCriteria = New System.Windows.Forms.Label()
        Me.lblNote = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblSpliter2 = New System.Windows.Forms.Label()
        Me.cmdAdjustGeometry = New System.Windows.Forms.Button()
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
        Me.chkAutoSelect = New System.Windows.Forms.CheckBox()
        Me.cmdForm_SelectionCriteria = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCrossSec = New System.Windows.Forms.ComboBox()
        Me.lblAdjusted = New System.Windows.Forms.Label()
        Me.toolTipUpdateHFree = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.picThumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(688, 340)
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
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.lblThick)
        Me.Panel1.Controls.Add(Me.picThumbnail)
        Me.Panel1.Controls.Add(Me.lblShowCriteria)
        Me.Panel1.Controls.Add(Me.lblNote)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.lblSpliter2)
        Me.Panel1.Controls.Add(Me.cmdAdjustGeometry)
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
        Me.Panel1.Controls.Add(Me.chkAutoSelect)
        Me.Panel1.Controls.Add(Me.cmdForm_SelectionCriteria)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbCrossSec)
        Me.Panel1.Controls.Add(Me.lblAdjusted)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(543, 335)
        Me.Panel1.TabIndex = 1
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(347, 6)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(28, 13)
        Me.Label16.TabIndex = 186
        Me.Label16.Text = "Tol."
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(433, 6)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(82, 13)
        Me.Label13.TabIndex = 185
        Me.Label13.Text = "Compression"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbCompressTolType
        '
        Me.cmbCompressTolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompressTolType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCompressTolType.Items.AddRange(New Object() {"Nominal", "Minimum", "Maximum"})
        Me.cmbCompressTolType.Location = New System.Drawing.Point(320, 25)
        Me.cmbCompressTolType.Name = "cmbCompressTolType"
        Me.cmbCompressTolType.Size = New System.Drawing.Size(82, 21)
        Me.cmbCompressTolType.TabIndex = 184
        '
        'lblComp
        '
        Me.lblComp.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblComp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblComp.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblComp.Location = New System.Drawing.Point(419, 25)
        Me.lblComp.Name = "lblComp"
        Me.lblComp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComp.Size = New System.Drawing.Size(110, 21)
        Me.lblComp.TabIndex = 183
        Me.lblComp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblZClear
        '
        Me.lblZClear.BackColor = System.Drawing.Color.White
        Me.lblZClear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblZClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblZClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZClear.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblZClear.Location = New System.Drawing.Point(476, 205)
        Me.lblZClear.Name = "lblZClear"
        Me.lblZClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblZClear.Size = New System.Drawing.Size(53, 21)
        Me.lblZClear.TabIndex = 182
        Me.lblZClear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(420, 209)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(54, 13)
        Me.Label4.TabIndex = 180
        Me.Label4.Text = " ZClear "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(390, 162)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(2, 122)
        Me.Label2.TabIndex = 179
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(239, 82)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(120, 23)
        Me.Label15.TabIndex = 177
        Me.Label15.Text = "Cross Section Set"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(37, 253)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(63, 13)
        Me.Label14.TabIndex = 176
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
        Me.lblThick.Location = New System.Drawing.Point(106, 249)
        Me.lblThick.Name = "lblThick"
        Me.lblThick.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblThick.Size = New System.Drawing.Size(53, 21)
        Me.lblThick.TabIndex = 175
        Me.lblThick.Text = "0.015"
        Me.lblThick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'picThumbnail
        '
        Me.picThumbnail.BackColor = System.Drawing.Color.AliceBlue
        Me.picThumbnail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picThumbnail.Location = New System.Drawing.Point(7, 24)
        Me.picThumbnail.Name = "picThumbnail"
        Me.picThumbnail.Size = New System.Drawing.Size(114, 121)
        Me.picThumbnail.TabIndex = 75
        Me.picThumbnail.TabStop = False
        '
        'lblShowCriteria
        '
        Me.lblShowCriteria.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowCriteria.Location = New System.Drawing.Point(137, 113)
        Me.lblShowCriteria.Name = "lblShowCriteria"
        Me.lblShowCriteria.Size = New System.Drawing.Size(90, 15)
        Me.lblShowCriteria.TabIndex = 72
        Me.lblShowCriteria.Text = "Show Criteria"
        Me.lblShowCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNote
        '
        Me.lblNote.AutoSize = True
        Me.lblNote.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNote.ForeColor = System.Drawing.Color.Red
        Me.lblNote.Location = New System.Drawing.Point(12, 323)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(0, 15)
        Me.lblNote.TabIndex = 71
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(145, 6)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 14)
        Me.Label12.TabIndex = 68
        Me.Label12.Text = "Number"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSpliter2
        '
        Me.lblSpliter2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSpliter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSpliter2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpliter2.Location = New System.Drawing.Point(0, 282)
        Me.lblSpliter2.Name = "lblSpliter2"
        Me.lblSpliter2.Size = New System.Drawing.Size(555, 2)
        Me.lblSpliter2.TabIndex = 64
        '
        'cmdAdjustGeometry
        '
        Me.cmdAdjustGeometry.BackColor = System.Drawing.Color.Silver
        Me.cmdAdjustGeometry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdjustGeometry.Location = New System.Drawing.Point(409, 105)
        Me.cmdAdjustGeometry.Name = "cmdAdjustGeometry"
        Me.cmdAdjustGeometry.Size = New System.Drawing.Size(120, 30)
        Me.cmdAdjustGeometry.TabIndex = 5
        Me.cmdAdjustGeometry.Text = "Adjust &Geometry"
        Me.cmdAdjustGeometry.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(370, 300)
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
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(457, 300)
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
        Me.Label11.Location = New System.Drawing.Point(402, 253)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 13)
        Me.Label11.TabIndex = 60
        Me.Label11.Text = "Control Dia"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(273, 207)
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
        Me.lblHFree.Location = New System.Drawing.Point(106, 205)
        Me.lblHFree.Name = "lblHFree"
        Me.lblHFree.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHFree.Size = New System.Drawing.Size(53, 21)
        Me.lblHFree.TabIndex = 58
        Me.lblHFree.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(166, 226)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(22, 12)
        Me.Label9.TabIndex = 57
        Me.Label9.Text = "(—)"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(167, 198)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(20, 12)
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
        Me.Label7.Location = New System.Drawing.Point(8, 207)
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
        Me.txtHFreeTolPlus.Location = New System.Drawing.Point(193, 193)
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
        Me.txtHFreeTolMinus.Location = New System.Drawing.Point(193, 221)
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
        Me.lblUnitUserL.Location = New System.Drawing.Point(86, 176)
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
        Me.Label6.Location = New System.Drawing.Point(12, 176)
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
        Me.lblDControl.Location = New System.Drawing.Point(477, 249)
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
        Me.lblWid.Location = New System.Drawing.Point(319, 205)
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
        Me.lblSplitter1.Size = New System.Drawing.Size(555, 2)
        Me.lblSplitter1.TabIndex = 39
        '
        'chkAutoSelect
        '
        Me.chkAutoSelect.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.chkAutoSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoSelect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoSelect.Location = New System.Drawing.Point(238, 114)
        Me.chkAutoSelect.Name = "chkAutoSelect"
        Me.chkAutoSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoSelect.Size = New System.Drawing.Size(13, 14)
        Me.chkAutoSelect.TabIndex = 38
        Me.chkAutoSelect.UseVisualStyleBackColor = False
        '
        'cmdForm_SelectionCriteria
        '
        Me.cmdForm_SelectionCriteria.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cmdForm_SelectionCriteria.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdForm_SelectionCriteria.Enabled = False
        Me.cmdForm_SelectionCriteria.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdForm_SelectionCriteria.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdForm_SelectionCriteria.Location = New System.Drawing.Point(227, 106)
        Me.cmdForm_SelectionCriteria.Name = "cmdForm_SelectionCriteria"
        Me.cmdForm_SelectionCriteria.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdForm_SelectionCriteria.Size = New System.Drawing.Size(145, 30)
        Me.cmdForm_SelectionCriteria.TabIndex = 4
        Me.cmdForm_SelectionCriteria.Text = "     All Cross Sections"
        Me.cmdForm_SelectionCriteria.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(232, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 16)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Adjusted"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 33
        Me.Label1.Text = "Cross Section:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbCrossSec
        '
        Me.cmbCrossSec.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCrossSec.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCrossSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCrossSec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCrossSec.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCrossSec.Location = New System.Drawing.Point(142, 25)
        Me.cmbCrossSec.Name = "cmbCrossSec"
        Me.cmbCrossSec.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbCrossSec.Size = New System.Drawing.Size(70, 21)
        Me.cmbCrossSec.TabIndex = 1
        '
        'lblAdjusted
        '
        Me.lblAdjusted.BackColor = System.Drawing.Color.White
        Me.lblAdjusted.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAdjusted.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdjusted.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdjusted.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblAdjusted.Location = New System.Drawing.Point(245, 25)
        Me.lblAdjusted.Name = "lblAdjusted"
        Me.lblAdjusted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdjusted.Size = New System.Drawing.Size(32, 20)
        Me.lblAdjusted.TabIndex = 27
        Me.lblAdjusted.Text = "N"
        Me.lblAdjusted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'IPE_frmDesignESeal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(548, 340)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmDesignESeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "E-Seal Design"
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

        'AES 20SEP16
        With cmbCompressTolType
            .Items().Clear()
            .Items.Add("Nominal")
            .Items.Add("Minimum")
            .Items.Add("Maximum")
        End With

    End Sub


#Region "FORM EVENT ROUTINES:"

    Private Sub frmDesignESeal_Load(ByVal sender As Object, _
                                    ByVal e As System.EventArgs) _
                                    Handles MyBase.Load
        '==========================================================

        '....This form load event is triggered only when the form is displayed by
        '........"Show Dialog" in the frmDesign, not when it is created and neither when control 
        '........comes back to this form after a subordinate form closes.
        '
        '   Set Margin of the Picture Box.
        '   ------------------------------        
        Dim pUniformMargin As Single = 0.05       '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        '   Update Working DB Table:        
        '   ========================
        '
        'If mFormOpened_FirstTime = True Then
        '    '   ....This update operation will be done once in every execution session.
        '    '
        '    '   ....This operation reads the records of each cross-sections from the ESeaDB
        '    '   ........and repopulates the "CrossSecNo", "HfreeStd", "DHfreeAdjMax", "WidMax" 
        '    '   ........fields in the Working DB and initializes the "Candidate" field to "NO".
        '    '
        '    Update_WorkingDB()
        '    mFormOpened_FirstTime = False            '....Reset the FLAG.
        'End If

        'AES 29MAR16
        Dim pSealMCSDB As New SealIPEMCSDBEntities()
        mCrossSecList.Clear()
        mCandidateList.Clear()

        '....ESealGeom
        'Dim pQryESealGeom = (From it In pSealDBEntities.tblESeal_Geom Order By it.fldCrossSecNo Ascending Select it).Distinct()

        Dim pQryESealGeom = (From it In pSealMCSDB.tblESeal_Geom Select it.fldCrossSecNo Distinct).ToList()

        'Dim pRecord As tblESeal_Geom
        For i As Integer = 0 To pQryESealGeom.Count - 1
            mCrossSecList.Add(pQryESealGeom(i))
            mCandidateList.Add(False)
        Next

        Update_Envelope()


        '   Update the "Candidate" field of the "ESealCandidate" table in the  
        '   ....Working DB. 
        '=======================================================================
        '
        If gUpdate_Candidate_CrossSecs = True Then
            '-------------------------------------

            '....The following flag indicates that a candidate design set has been found  
            '........in the database for the current cavity envelope. 

            mSuccess_Candidate_CrossSecs = gIPE_SealCandidates.Update_Candidate_CrossSecs _
                                                       (gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.WidMin, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth, _
                                                        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.HFree_Rep, mCandidateList)

            If mSuccess_Candidate_CrossSecs = False Then
                Display_Msg_NoCandidate_CrossSecs()
            End If

            '....Candidate selection has been done. 
            gUpdate_Candidate_CrossSecs = False         '....Reset the FLAG.

        End If

        InitializeControls()

        '....Create & initialize the local seal object.
        InitializeLocalObject()                   '....gSeal ===> mESeal.      


        'Set the AutoSelect checkbox "CheckState". 
        '-----------------------------------------
        '....The following assignment may or may not trigger the    
        '........"chkAutoSelect_CheckedChanged" event, which in turn calls 
        '........PopulateCmbCrossSec() & DisplayData()routines.
        '
        If gDisplay_Candidate_CrossSecs = True And mSuccess_Candidate_CrossSecs = True Then
            chkAutoSelect.CheckState = CheckState.Checked
        Else
            chkAutoSelect.CheckState = CheckState.Unchecked
        End If

        '....Populate Cross-section Combo Box.
        PopulateCmbCrossSec()

        '....Refresh Display upon entry to the form.
        '........Local seal object "mSeal" is used.
        DisplayData()



    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        lblUnitUserL.Text = "(" & gIPE_Unit.UserL & ")"

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            cmbCrossSec.Enabled = False
            chkAutoSelect.Enabled = False
            txtHFreeTolPlus.Enabled = False
            txtHFreeTolMinus.Enabled = False
            cmbCompressTolType.Enabled = False
        Else
            cmbCrossSec.Enabled = True
            chkAutoSelect.Enabled = True
            txtHFreeTolPlus.Enabled = True
            txtHFreeTolMinus.Enabled = True
            cmbCompressTolType.Enabled = True
        End If
        'txtZClear.Enabled = False
    End Sub


    Private Sub Update_Envelope()
        '========================

        Me.Cursor = Cursors.WaitCursor  '....Show Hourglass Cursor.

        If IPE_clsSealCandidates.CheckIfReqd_Populate_Envelope(gIPE_File, gIPE_User, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type) = True Then
            MessageBox.Show("Envelope for ESeal is being populated. Please wait...", _
                       "Update E-Seal Envelope", MessageBoxButtons.OK, MessageBoxIcon.Information)
            IPE_clsSealCandidates.Populate_Envelope(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type)
        End If


        Me.Cursor = Cursors.Default     '....Restore the Default Cursor.

    End Sub


    Private Sub Display_Msg_NoCandidate_CrossSecs()
        '==========================================     

        Dim pstrTitle, pstrMsg As String
        Dim pintAttributes As Integer

        pstrTitle = "Candidate CrossSection Selection"
        pintAttributes = MsgBoxStyle.OkOnly + MsgBoxStyle.Information

        '....No candidate cross-section set has been found. 
        '........All the cross-sections will be displayed.
        pstrMsg = "No candidate STD cross section is found for " & _
                  "the current cavity envelope." & Chr(Keys.Return) & _
                  "The menu will display all the available cross sections."
        MsgBox(pstrMsg, pintAttributes, pstrTitle)

    End Sub


    Private Sub InitializeLocalObject()
        '=============================
        '....From gSeal ===> mSeal. 
        '........Now onwards, mSeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal only in the 
        '........"SaveData" routine which is called when the form is exited and/or another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local E-Seal Object. 
        mESeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo = "" Or CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).NewDesign = True Then

            '....PRIMARY ASSIGNMENTS are not done.
            '........Initialize with the first cross-section of the list.
            mESeal.MCrossSecNo = IPE_clsESeal.CrossSecList(0)

            '....Cross-section is assigned here locally and hence, no need to 
            '........show the message.
            mFlag_DontFitCavity_Msg = False

            '....SECONDARY ASSIGNMENTS:
            Dim i As Int16
            For i = 1 To 2

                If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity Is Nothing = False Then
                    mESeal.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
                End If
            Next


        Else
            mESeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone

            '....Cross-section is coming from the global variable and hence, 
            '........if necessary, show the message.
            mFlag_DontFitCavity_Msg = True
        End If

    End Sub


    Private Sub DisplayData()
        '====================
        '....This routine displays the current state of the local seal object "mSeal". 

        'Cross-Section & Stage No:
        '------------------------
        With mESeal

            If .MCrossSecNo <> "" Then

                If cmbCrossSec.Items.Contains(.MCrossSecNo) = True Then

                    cmbCrossSec.Text = .MCrossSecNo  '...."SelectedIndexChanged" Event

                Else '....FALSE
                    '    '-----

                    If mFlag_DontFitCavity_Msg = True Then

                        Dim pstrTitle, pstrMsg As String
                        Dim pintAttributes As Integer

                        pstrTitle = "WARNING MESSAGE:  Seal Design"
                        pstrMsg = "Seal Design does not fit in the current cavity! " & _
                                   vbCrLf & "A new design is to be selected from " & _
                                   "the candidate cross-sections."
                        pintAttributes = MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly
                        MsgBox(pstrMsg, pintAttributes, pstrTitle)
                    End If

                    cmbCrossSec.SelectedIndex = 0

                End If


            Else    '....Either .CrossSecNo = "" or .StageNo = ""
                cmbCrossSec.SelectedIndex = 0
            End If

        End With

        '....Display all the relevant length parameters:        
        '........Adjusted, HfreeStd & Tolerances, Wid & DControl.    
        DisplayLengthParams()

        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        cmbCompressTolType.Text = gIPE_Project.Analysis(pICur).Compression.TolType
        DisplayCompressionVal()     'AES 22SEP16

    End Sub


#Region "SUB-HELPER ROUTINES:"

    Private Sub DisplayLengthParams()
        ''============================

        With mESeal

            lblAdjusted.Text = .Adjusted
            lblAdjusted.ForeColor = Color.Blue
            lblAdjusted.BackColor = Color.Gainsboro

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
            lblThick.BackColor = Color.Gainsboro

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

    Private Sub txtZClear_TextChanged(sender As System.Object,
                                      e As System.EventArgs)
        '=================================================================================

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


#Region "AUTO-SELECTION RELATED CONTROLS:"


    Private Sub chkAutoSelect_CheckedChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles chkAutoSelect.CheckedChanged
        '=========================================================================

        'Show Criteria Label    
        '-------------------
        lblShowCriteria.Visible = chkAutoSelect.Checked


        If chkAutoSelect.CheckState = CheckState.Checked And _
            mSuccess_Candidate_CrossSecs = False Then

            Display_Msg_NoCandidate_CrossSecs()
            chkAutoSelect.CheckState = CheckState.Unchecked
            Exit Sub
        End If


        If chkAutoSelect.CheckState = CheckState.Checked Then
            '------------------------------------------------
            '
            gDisplay_Candidate_CrossSecs = True

            With cmdForm_SelectionCriteria
                .Enabled = True
                .Text = " Candidates Only"
                .TextAlign = ContentAlignment.MiddleCenter
            End With


        ElseIf chkAutoSelect.CheckState = CheckState.Unchecked Then
            '------------------------------------------------------

            gDisplay_Candidate_CrossSecs = False

            With cmdForm_SelectionCriteria
                .Enabled = False
                .Text = "   All Cross Sections"
                .TextAlign = ContentAlignment.MiddleCenter
            End With

        End If

        PopulateCmbCrossSec()

        '....Display the current state of the local seal object.
        DisplayData()

    End Sub

    Private Sub cmdForm_SelectionCriteria_EnabledChanged _
                                (ByVal sender As Object, ByVal e As System.EventArgs) _
                                 Handles cmdForm_SelectionCriteria.EnabledChanged
        '==============================================================================

        With cmdForm_SelectionCriteria

            If .Enabled = True Then
                .Text = "Auto-Select"
                .TextAlign = ContentAlignment.MiddleCenter

            ElseIf .Enabled = False Then
                .Text = "No Auto-Select"
                .TextAlign = ContentAlignment.MiddleRight
            End If

        End With

    End Sub


    Private Sub cmdForm_SelectionCriteria_Click(ByVal sender As System.Object, _
                                                ByVal e As System.EventArgs) _
                                                Handles cmdForm_SelectionCriteria.Click
        '===============================================================================
        Dim pfrmSelectionCriteria As New IPE_frmSelectionCriteria()
        pfrmSelectionCriteria.ShowDialog()

    End Sub


#End Region


#Region "CROSS-SECTION COMBO-BOX RELATED CONTROLS:"


    Private Sub PopulateCmbCrossSec()
        '============================       
        '....This routine populates the Cross Sec No. combo box. (Database Driven).

        'Dim pSealDBWorkingEntities As New SealDBWorkingEntities()
        If chkAutoSelect.CheckState = CheckState.Checked Then
            cmbCrossSec.Items.Clear()
            For i As Integer = 0 To mCrossSecList.Count - 1
                If (mCandidateList(i) = True) Then
                    cmbCrossSec.Items.Add(mCrossSecList(i))
                End If
            Next

        Else
            cmbCrossSec.Items.Clear()
            For i As Integer = 0 To mCrossSecList.Count - 1
                cmbCrossSec.Items.Add(mCrossSecList(i))
            Next

        End If

    End Sub


    Private Sub cmbCrossSec_SelectedIndexChanged(ByVal sender As System.Object, _
                                                 ByVal e As System.EventArgs) _
                                                 Handles cmbCrossSec.SelectedIndexChanged
        '==============================================================================
        'MsgBox("cmbCrossSec_SelectedIndexChanged")     '....For diagnostic purpose. 
        If mESeal Is Nothing = True Then Exit Sub

        With mESeal

            If cmbCrossSec.Text <> .MCrossSecNo Then
                '
                '....The following assignment resets all the length parameters to the 
                '........standard values for the selected "CrossSecNo". The above check
                '........makes sure that the "CrossSecNo" selected is different from that
                '........of the local seal object and only then the resetting will be 
                '........effected.

                .MCrossSecNo = cmbCrossSec.Text
                DisplayLengthParams()      '....Update Length Parameters Display.
            End If


            '....Change CrossSecNo.in ComboBox,then updates reflacts on DesignESeal Form
            '........and Main form also.
            '
            'SaveData()          'PB 02MAR08. Check logic later. Violates our convention. Saving data
            'before exiting the form is usually not done.

            'If cmbCrossSec.Text <> .CrossSecNo Then SaveData() 


            'PB 13SEP08. A deep copy method should be implemented in clsAppCond later.
            '   Update the "Compression Data" on the frmMain.
            '   ---------------------------------------------
            '   ....Miscellaneous enhancements, as requested by APBU.
            '   ........(Task #6 Proposal Phase VII).
            '   
            'Dim pAppCond As New clsAppCond()

            '....SECONDARY ASSIGNMENTS.
            'With pAppCond
            '    .Cavity = gAnalysis_Cur.Cavity
            '    .UnitSystem = gAppCond.UnitSystem
            '    .CompressionTolType = gAppCond.Compression.TolType

            '    .Seal = mESeal
            'End With

            'gfrmMain.UpdateDisplay_AppCond_Compression(gIPE_Project)
            DoGraphics()

            DisplayCompressionVal()

        End With

    End Sub


    Private Sub cmbCompressTolType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles cmbCompressTolType.SelectedIndexChanged
        '=================================================================================================
        DisplayCompressionVal()

    End Sub


    Private Sub DisplayCompressionVal()
        '==============================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        Dim pVal As Double = mESeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
        Dim pPCentVal As Double = (pVal / mESeal.Hfree) * 100.0#

        lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

    End Sub

#End Region


#Region "ADJUST GEOMETRY COMMAND BUTTON:"

    Private Sub cmdAdjustGeometry_Click(ByVal sender As System.Object, _
                                        ByVal e As System.EventArgs) _
                                        Handles cmdAdjustGeometry.Click
        '====================================================================

        '....Save before opening frmAdjGeomCSeal form       
        SaveData()

        Dim pblnError As Boolean = False    '....Initialize
        gIPE_DataValidate.CheckForNullCrossSectionNo(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, pblnError)

        If pblnError = False Then
            Dim pfrmAdjGeomESeal As New IPE_frmAdjGeomESeal()
            pfrmAdjGeomESeal.ShowDialog()
        End If


        'RETURNED FROM "frmAdjGeomCSeal":
        '--------------------------------
        '....Upon returning from the Adjust Geometry Form, update the local
        '........seal object. gSeal ===> mESeal.
        mESeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).Clone

        '....Update Length parameters display.
        DisplayLengthParams()

    End Sub

#End Region


#Region "COMMAND BUTTONS RELATED ROUTINES:"

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) _
                                Handles cmdOK.Click, cmdCancel.Click
        '=======================================================================
        Dim pCmdBtn As Button = CType(sender, Button)

        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
        End If

        Me.Close()

    End Sub


    'Private Sub cmdFEA_Click(sender As System.Object, e As System.EventArgs)
    '    '========================================================================================
    '    modMain.gfrmFEA.ShowDialog()
    'End Sub

#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

        'MAIN SEAL OBJECT:
        '-----------------
        '
        With CType(gIPE_Project.Analysis(pICur).Seal, IPE_clsESeal)

            If .MCrossSecNo <> cmbCrossSec.Text Then .MCrossSecNo = cmbCrossSec.Text

            .HFreeTol(1) = gIPE_Unit.L_UserToCon(txtHFreeTolMinus.Text)  '....Minus Value
            .HFreeTol(2) = gIPE_Unit.L_UserToCon(txtHFreeTolPlus.Text)   '....Plus Value

            '....AES 21MAR16
            '.ZClear = gIPE_Unit.L_UserToCon(txtZClear.Text)
        End With

        'AES 21MAR16
        'gIPE_SealCandidates.ZClear = gIPE_Unit.L_UserToCon(txtZClear.Text)


        'SECONDARY ASSIGNMENTS:
        '----------------------
        gIPE_Project.Analysis(pICur).Cavity.CornerRad = gIPE_Project.Analysis(pICur).Seal.CavityCornerRad
        'gAppCond.Seal = gAnalysis_Cur.Seal

        'AES 20SEP16
        gIPE_Project.Analysis(pICur).CompressionTolType = cmbCompressTolType.Text

    End Sub

#End Region

#End Region


#Region "GRAPHICS ROUTINES:"

    Private Sub DoGraphics()
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

        '....Draw "Standard" Seal Geometry.        
        mESeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                    "STD", "SCALE_BY_STD", 1.25, _
                                    EnvpTopL, EnvpBotR)

        picThumbnail.Refresh()

    End Sub

#End Region

#End Region




End Class



