'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmFEAParamsESeal                      '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  31MAR16                                '
'                                                                              '
'===============================================================================

Imports SealIPELib = SealIPELib101
Public Class IPE_frmFEAParamsESeal
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "



    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        '==================================================================
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
    Friend WithEvents cmbNLayer As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents txtWE2 As System.Windows.Forms.TextBox
    Friend WithEvents txtWE1 As System.Windows.Forms.TextBox
    Friend WithEvents txtWE3 As System.Windows.Forms.TextBox
    Friend WithEvents txtWM1 As System.Windows.Forms.TextBox
    Friend WithEvents txtWM3 As System.Windows.Forms.TextBox
    Friend WithEvents txtBetaT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtFacTOLN As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtFacKN As System.Windows.Forms.TextBox
    Friend WithEvents cmbNelConRad As System.Windows.Forms.ComboBox
    Friend WithEvents cmbNelConHeel As System.Windows.Forms.ComboBox
    Friend WithEvents cmbNelConMid As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtNSBMN As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtNSBMX As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtNSBSTP As System.Windows.Forms.TextBox
    Friend WithEvents txtWM2 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents cmbNelConSeal As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmFEAParamsESeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.txtBetaT = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbNLayer = New System.Windows.Forms.ComboBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtNSBMN = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtNSBMX = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtNSBSTP = New System.Windows.Forms.TextBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtFacTOLN = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbNelConRad = New System.Windows.Forms.ComboBox()
        Me.cmbNelConHeel = New System.Windows.Forms.ComboBox()
        Me.cmbNelConMid = New System.Windows.Forms.ComboBox()
        Me.cmbNelConSeal = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFacKN = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtWM2 = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtWM3 = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtWM1 = New System.Windows.Forms.TextBox()
        Me.txtWE3 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtWE1 = New System.Windows.Forms.TextBox()
        Me.txtWE2 = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(467, 490)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.GroupBox5)
        Me.Panel1.Controls.Add(Me.GroupBox4)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(465, 488)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtBetaT)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.cmbNLayer)
        Me.GroupBox5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(16, 7)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(432, 60)
        Me.GroupBox5.TabIndex = 1003
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Across Thickness:"
        '
        'txtBetaT
        '
        Me.txtBetaT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBetaT.Location = New System.Drawing.Point(350, 24)
        Me.txtBetaT.Name = "txtBetaT"
        Me.txtBetaT.Size = New System.Drawing.Size(60, 21)
        Me.txtBetaT.TabIndex = 40
        Me.txtBetaT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 20)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "# of Elements"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(180, 24)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(167, 20)
        Me.Label14.TabIndex = 20
        Me.Label14.Text = "Grid Clustering Parameter"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbNLayer
        '
        Me.cmbNLayer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNLayer.Location = New System.Drawing.Point(100, 24)
        Me.cmbNLayer.Name = "cmbNLayer"
        Me.cmbNLayer.Size = New System.Drawing.Size(60, 21)
        Me.cmbNLayer.TabIndex = 24
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label17)
        Me.GroupBox4.Controls.Add(Me.txtNSBMN)
        Me.GroupBox4.Controls.Add(Me.Label18)
        Me.GroupBox4.Controls.Add(Me.txtNSBMX)
        Me.GroupBox4.Controls.Add(Me.Label19)
        Me.GroupBox4.Controls.Add(Me.txtNSBSTP)
        Me.GroupBox4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(16, 376)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(432, 58)
        Me.GroupBox4.TabIndex = 1002
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Load Sub Step Definitions:"
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(296, 24)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(50, 20)
        Me.Label17.TabIndex = 987
        Me.Label17.Text = "NSBMN"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNSBMN
        '
        Me.txtNSBMN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNSBMN.Location = New System.Drawing.Point(350, 24)
        Me.txtNSBMN.Name = "txtNSBMN"
        Me.txtNSBMN.Size = New System.Drawing.Size(60, 21)
        Me.txtNSBMN.TabIndex = 988
        Me.txtNSBMN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(152, 24)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(50, 20)
        Me.Label18.TabIndex = 985
        Me.Label18.Text = "NSBMX"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNSBMX
        '
        Me.txtNSBMX.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNSBMX.Location = New System.Drawing.Point(206, 24)
        Me.txtNSBMX.Name = "txtNSBMX"
        Me.txtNSBMX.Size = New System.Drawing.Size(60, 21)
        Me.txtNSBMX.TabIndex = 986
        Me.txtNSBMX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(8, 24)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(50, 20)
        Me.Label19.TabIndex = 20
        Me.Label19.Text = "NSBSTP"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNSBSTP
        '
        Me.txtNSBSTP.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNSBSTP.Location = New System.Drawing.Point(62, 24)
        Me.txtNSBSTP.Name = "txtNSBSTP"
        Me.txtNSBSTP.Size = New System.Drawing.Size(60, 21)
        Me.txtNSBSTP.TabIndex = 984
        Me.txtNSBSTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(376, 453)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 974
        Me.cmdCancel.Text = "  Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(288, 453)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 37
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.txtFacTOLN)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.cmbNelConRad)
        Me.GroupBox1.Controls.Add(Me.cmbNelConHeel)
        Me.GroupBox1.Controls.Add(Me.cmbNelConMid)
        Me.GroupBox1.Controls.Add(Me.cmbNelConSeal)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtFacKN)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 82)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(432, 170)
        Me.GroupBox1.TabIndex = 971
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Contact Model:"
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(8, 20)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(87, 20)
        Me.Label21.TabIndex = 995
        Me.Label21.Text = "# of Elements:"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(8, 99)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(121, 20)
        Me.Label20.TabIndex = 994
        Me.Label20.Text = "Contact Parameters:"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtFacTOLN
        '
        Me.txtFacTOLN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFacTOLN.Location = New System.Drawing.Point(350, 130)
        Me.txtFacTOLN.Name = "txtFacTOLN"
        Me.txtFacTOLN.Size = New System.Drawing.Size(60, 21)
        Me.txtFacTOLN.TabIndex = 993
        Me.txtFacTOLN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(272, 130)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(64, 16)
        Me.Label16.TabIndex = 992
        Me.Label16.Text = "FacTOLN"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(259, 146)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(88, 13)
        Me.Label15.TabIndex = 991
        Me.Label15.Text = "(Range < 1.0)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(21, 146)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(115, 13)
        Me.Label7.TabIndex = 988
        Me.Label7.Text = "(Range 0.001-100)"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbNelConRad
        '
        Me.cmbNelConRad.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNelConRad.Location = New System.Drawing.Point(350, 65)
        Me.cmbNelConRad.Name = "cmbNelConRad"
        Me.cmbNelConRad.Size = New System.Drawing.Size(60, 21)
        Me.cmbNelConRad.TabIndex = 34
        '
        'cmbNelConHeel
        '
        Me.cmbNelConHeel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNelConHeel.Location = New System.Drawing.Point(245, 65)
        Me.cmbNelConHeel.Name = "cmbNelConHeel"
        Me.cmbNelConHeel.Size = New System.Drawing.Size(60, 21)
        Me.cmbNelConHeel.TabIndex = 33
        '
        'cmbNelConMid
        '
        Me.cmbNelConMid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNelConMid.Location = New System.Drawing.Point(140, 65)
        Me.cmbNelConMid.Name = "cmbNelConMid"
        Me.cmbNelConMid.Size = New System.Drawing.Size(60, 21)
        Me.cmbNelConMid.TabIndex = 32
        '
        'cmbNelConSeal
        '
        Me.cmbNelConSeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNelConSeal.Location = New System.Drawing.Point(35, 65)
        Me.cmbNelConSeal.Name = "cmbNelConSeal"
        Me.cmbNelConSeal.Size = New System.Drawing.Size(60, 21)
        Me.cmbNelConSeal.TabIndex = 31
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Sealing Region"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(130, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 20)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Mid Region"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(235, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 20)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Heel Region"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(340, 43)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 20)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Radial Region"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(72, 130)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 16)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "FacKN"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFacKN
        '
        Me.txtFacKN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFacKN.Location = New System.Drawing.Point(140, 130)
        Me.txtFacKN.Name = "txtFacKN"
        Me.txtFacKN.Size = New System.Drawing.Size(60, 21)
        Me.txtFacKN.TabIndex = 30
        Me.txtFacKN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtWM2)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.txtWM3)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.txtWM1)
        Me.GroupBox2.Controls.Add(Me.txtWE3)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.txtWE1)
        Me.GroupBox2.Controls.Add(Me.txtWE2)
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(16, 267)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(432, 94)
        Me.GroupBox2.TabIndex = 972
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Element Density Weightage on Segments:"
        '
        'txtWM2
        '
        Me.txtWM2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWM2.Location = New System.Drawing.Point(206, 56)
        Me.txtWM2.Name = "txtWM2"
        Me.txtWM2.Size = New System.Drawing.Size(60, 21)
        Me.txtWM2.TabIndex = 1003
        Me.txtWM2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(175, 56)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(27, 20)
        Me.Label13.TabIndex = 43
        Me.Label13.Text = " M2"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWM3
        '
        Me.txtWM3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWM3.Location = New System.Drawing.Point(350, 56)
        Me.txtWM3.Name = "txtWM3"
        Me.txtWM3.Size = New System.Drawing.Size(60, 21)
        Me.txtWM3.TabIndex = 42
        Me.txtWM3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(322, 58)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(25, 20)
        Me.Label12.TabIndex = 41
        Me.Label12.Text = "M3    "
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(35, 56)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(25, 16)
        Me.Label11.TabIndex = 40
        Me.Label11.Text = "M1"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWM1
        '
        Me.txtWM1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWM1.Location = New System.Drawing.Point(62, 56)
        Me.txtWM1.Name = "txtWM1"
        Me.txtWM1.Size = New System.Drawing.Size(60, 21)
        Me.txtWM1.TabIndex = 39
        Me.txtWM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtWE3
        '
        Me.txtWE3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWE3.Location = New System.Drawing.Point(350, 24)
        Me.txtWE3.Name = "txtWE3"
        Me.txtWE3.Size = New System.Drawing.Size(60, 21)
        Me.txtWE3.TabIndex = 38
        Me.txtWE3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(322, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(25, 16)
        Me.Label10.TabIndex = 37
        Me.Label10.Text = "E3"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(177, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(25, 16)
        Me.Label9.TabIndex = 36
        Me.Label9.Text = "E2"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(35, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(25, 16)
        Me.Label8.TabIndex = 35
        Me.Label8.Text = "E1"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtWE1
        '
        Me.txtWE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWE1.Location = New System.Drawing.Point(62, 24)
        Me.txtWE1.Name = "txtWE1"
        Me.txtWE1.Size = New System.Drawing.Size(60, 21)
        Me.txtWE1.TabIndex = 34
        Me.txtWE1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtWE2
        '
        Me.txtWE2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWE2.Location = New System.Drawing.Point(206, 24)
        Me.txtWE2.Name = "txtWE2"
        Me.txtWE2.Size = New System.Drawing.Size(60, 21)
        Me.txtWE2.TabIndex = 33
        Me.txtWE2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'IPE_frmFEAParamsESeal
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(473, 496)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "IPE_frmFEAParamsESeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FEA  Parameters: E-Seal"
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New()
        '===========

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        'Populate Combo Boxes:
        '---------------------
        Dim i As Integer

        '....No. of Layers
        For i = 5 To 12
            cmbNLayer.Items.Add(i)
        Next
        cmbNLayer.SelectedIndex = 0


        '....No. of Contact Elements:
        For i = 1 To 4
            cmbNelConSeal.Items.Add(i)
            cmbNelConMid.Items.Add(i)
            cmbNelConHeel.Items.Add(i)
            cmbNelConRad.Items.Add(i)
        Next

        cmbNelConSeal.SelectedIndex = 0
        cmbNelConMid.SelectedIndex = 0
        cmbNelConHeel.SelectedIndex = 0
        cmbNelConRad.SelectedIndex = 0

    End Sub

    '*******************************************************************************
    '*                       FORM EVENT ROUTINES - BEGIN                           *
    '*******************************************************************************

    Private Sub frmFEAParametersESeal_Load(ByVal sender As System.Object, _
                                           ByVal e As System.EventArgs) _
                                           Handles MyBase.Load
        '===========================================================================
        DisplayData()
    End Sub


    Public Sub DisplayData()
        '===================

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal)
            '....General parameters:
            If .NLayer <> Val("") Then cmbNLayer.Text = .NLayer
            If .NelConSeal <> Val("") Then cmbNelConSeal.Text = .NelConSeal
            If .NelConRad <> Val("") Then cmbNelConRad.Text = .NelConRad
            txtFacKN.Text = .FacKN
            txtFacTOLN.Text = .FacTOLN
            txtBetaT.Text = .BetaT

            '....E-Seal Specific parameters:
            If .NelConMid <> Val("") Then cmbNelConMid.Text = .NelConMid
            If .NelConHeel <> Val("") Then cmbNelConHeel.Text = .NelConHeel

            txtWE1.Text = .WtE(1)
            txtWE2.Text = .WtE(2)
            txtWE3.Text = .WtE(3)

            txtWM1.Text = .WtM(1)
            txtWM2.Text = .WtM(2)
            txtWM3.Text = .WtM(3)

            '....Load sub-step parameters:
            txtNSBSTP.Text = .NSBSTP
            txtNSBMX.Text = .NSBMX
            txtNSBMN.Text = .NSBMN

        End With

    End Sub

    '*******************************************************************************
    '*                       FORM EVENT ROUTINES - END                             *
    '*******************************************************************************


    '*******************************************************************************
    '*                       CONTROL EVENT ROUTINES - BEGIN                        *
    '*******************************************************************************

    Private Sub txt_KeyPress(ByVal sender As System.Object, _
                             ByVal e As System.Windows.Forms.KeyPressEventArgs) _
                             Handles txtWE1.KeyPress, txtBetaT.KeyPress
        '============================================================================

        'Dim pCulture = gUserInfo.CultureName
        Dim pCulture = gIPE_Project.CultureName

        Select Case pCulture
            Case "USA", "UK"
                If e.KeyChar = "," Then e.KeyChar = "."
            Case "Germany", "France"
                If e.KeyChar = "." Then e.KeyChar = ","
        End Select

    End Sub

    '-------------------------------------------------------------------------------
    '                    OK & CANCEL BUTTONS ROUTINES - BEGIN 
    '-------------------------------------------------------------------------------

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '====================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()
        Me.Close()

    End Sub


    Private Sub SaveData()
        '=================          

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal)
            '....General parameters:
            .NLayer = cmbNLayer.Text
            .NelConSeal = cmbNelConSeal.Text
            .NelConRad = cmbNelConRad.Text
            .FacKN = txtFacKN.Text
            .FacTOLN = txtFacTOLN.Text
            .BetaT = txtBetaT.Text

            '....E-Seal Specific parameters:
            .NelConMid = cmbNelConMid.Text
            .NelConHeel = cmbNelConHeel.Text

            .WtE(1) = txtWE1.Text
            .WtE(2) = txtWE2.Text
            .WtE(3) = txtWE3.Text

            .WtM(1) = txtWM1.Text
            .WtM(2) = txtWM2.Text
            .WtM(3) = txtWM3.Text

            '....Load sub-step parameters:
            If txtNSBSTP.Text <> "" Then .NSBSTP = txtNSBSTP.Text
            If txtNSBMX.Text <> "" Then .NSBMX = txtNSBMX.Text
            If txtNSBMN.Text <> "" Then .NSBMN = txtNSBMN.Text

        End With

    End Sub

    '-------------------------------------------------------------------------------
    '                    OK & CANCEL BUTTONS ROUTINES - END 
    '-------------------------------------------------------------------------------


    '*******************************************************************************
    '*                       CONTROL EVENT ROUTINES - END                          *
    '*******************************************************************************

End Class
