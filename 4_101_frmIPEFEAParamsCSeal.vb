'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmFEAParamsCSeal                      '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  31MAR16                                '
'                                                                              '
'===============================================================================
Imports SealIPELib = SealIPELib101
Public Class IPE_frmFEAParamsCSeal
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "


    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        '================================================================
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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtBetaT As System.Windows.Forms.TextBox
    Friend WithEvents cmbNLayer As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFacKN As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtFacTOLN As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbNelConRad As System.Windows.Forms.ComboBox
    Friend WithEvents cmbNelConSeal As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbPlatingNLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtNSBSTP As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtNSBMN As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtNSBMX As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblPlatingNLayer As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmFEAParamsCSeal))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtNSBMN = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtNSBMX = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtNSBSTP = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtBetaT = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbPlatingNLayer = New System.Windows.Forms.ComboBox()
        Me.lblPlatingNLayer = New System.Windows.Forms.Label()
        Me.cmbNLayer = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtFacTOLN = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbNelConRad = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFacKN = New System.Windows.Forms.TextBox()
        Me.cmbNelConSeal = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
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
        Me.lblBorder.Size = New System.Drawing.Size(462, 452)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.GroupBox4)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(460, 450)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Controls.Add(Me.txtNSBMN)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.txtNSBMX)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Controls.Add(Me.txtNSBSTP)
        Me.GroupBox4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(16, 335)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(424, 58)
        Me.GroupBox4.TabIndex = 1001
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Load Sub Step Definitions:"
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(286, 24)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 20)
        Me.Label13.TabIndex = 987
        Me.Label13.Text = "NSBMN"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNSBMN
        '
        Me.txtNSBMN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNSBMN.Location = New System.Drawing.Point(352, 24)
        Me.txtNSBMN.Name = "txtNSBMN"
        Me.txtNSBMN.Size = New System.Drawing.Size(60, 21)
        Me.txtNSBMN.TabIndex = 988
        Me.txtNSBMN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(146, 24)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 20)
        Me.Label11.TabIndex = 985
        Me.Label11.Text = "NSBMX"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNSBMX
        '
        Me.txtNSBMX.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNSBMX.Location = New System.Drawing.Point(212, 24)
        Me.txtNSBMX.Name = "txtNSBMX"
        Me.txtNSBMX.Size = New System.Drawing.Size(60, 21)
        Me.txtNSBMX.TabIndex = 986
        Me.txtNSBMX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 20)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "NSBSTP"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNSBSTP
        '
        Me.txtNSBSTP.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNSBSTP.Location = New System.Drawing.Point(72, 24)
        Me.txtNSBSTP.Name = "txtNSBSTP"
        Me.txtNSBSTP.Size = New System.Drawing.Size(60, 21)
        Me.txtNSBSTP.TabIndex = 984
        Me.txtNSBSTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.txtBetaT)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.cmbPlatingNLayer)
        Me.GroupBox1.Controls.Add(Me.lblPlatingNLayer)
        Me.GroupBox1.Controls.Add(Me.cmbNLayer)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(424, 122)
        Me.GroupBox1.TabIndex = 1000
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Across Thickness:"
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(174, 51)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(170, 20)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "Grid Clustering Parameter"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBetaT
        '
        Me.txtBetaT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBetaT.Location = New System.Drawing.Point(352, 51)
        Me.txtBetaT.Name = "txtBetaT"
        Me.txtBetaT.Size = New System.Drawing.Size(60, 21)
        Me.txtBetaT.TabIndex = 984
        Me.txtBetaT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(14, 20)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(88, 20)
        Me.Label14.TabIndex = 986
        Me.Label14.Text = "# of Elements"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'cmbPlatingNLayer
        '
        Me.cmbPlatingNLayer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPlatingNLayer.Location = New System.Drawing.Point(72, 85)
        Me.cmbPlatingNLayer.Name = "cmbPlatingNLayer"
        Me.cmbPlatingNLayer.Size = New System.Drawing.Size(60, 21)
        Me.cmbPlatingNLayer.TabIndex = 985
        '
        'lblPlatingNLayer
        '
        Me.lblPlatingNLayer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlatingNLayer.Location = New System.Drawing.Point(13, 85)
        Me.lblPlatingNLayer.Name = "lblPlatingNLayer"
        Me.lblPlatingNLayer.Size = New System.Drawing.Size(53, 20)
        Me.lblPlatingNLayer.TabIndex = 983
        Me.lblPlatingNLayer.Text = "Plating"
        Me.lblPlatingNLayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbNLayer
        '
        Me.cmbNLayer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNLayer.Location = New System.Drawing.Point(72, 51)
        Me.cmbNLayer.Name = "cmbNLayer"
        Me.cmbNLayer.Size = New System.Drawing.Size(60, 21)
        Me.cmbNLayer.TabIndex = 982
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 20)
        Me.Label1.TabIndex = 977
        Me.Label1.Text = "Base Material"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(368, 413)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 28)
        Me.cmdCancel.TabIndex = 999
        Me.cmdCancel.Text = "  Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(267, 413)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 985
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.txtFacTOLN)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.cmbNelConRad)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtFacKN)
        Me.GroupBox2.Controls.Add(Me.cmbNelConSeal)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(16, 149)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(424, 171)
        Me.GroupBox2.TabIndex = 997
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Contact Regions:"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(14, 89)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(136, 20)
        Me.Label10.TabIndex = 1002
        Me.Label10.Text = "Parameters :  (Factors)"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(14, 22)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(220, 20)
        Me.Label9.TabIndex = 1001
        Me.Label9.Text = "#  of Contact Elements on either side :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(262, 137)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(88, 13)
        Me.Label7.TabIndex = 990
        Me.Label7.Text = "(Range < 1.0)"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtFacTOLN
        '
        Me.txtFacTOLN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFacTOLN.Location = New System.Drawing.Point(352, 121)
        Me.txtFacTOLN.Name = "txtFacTOLN"
        Me.txtFacTOLN.Size = New System.Drawing.Size(60, 21)
        Me.txtFacTOLN.TabIndex = 989
        Me.txtFacTOLN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(280, 121)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 16)
        Me.Label6.TabIndex = 988
        Me.Label6.Text = "FacTOLN"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(19, 138)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 13)
        Me.Label5.TabIndex = 987
        Me.Label5.Text = "(Range 0.001-100)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbNelConRad
        '
        Me.cmbNelConRad.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNelConRad.Location = New System.Drawing.Point(352, 54)
        Me.cmbNelConRad.Name = "cmbNelConRad"
        Me.cmbNelConRad.Size = New System.Drawing.Size(60, 21)
        Me.cmbNelConRad.TabIndex = 985
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(72, 121)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 16)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "FacKN"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFacKN
        '
        Me.txtFacKN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFacKN.Location = New System.Drawing.Point(136, 121)
        Me.txtFacKN.Name = "txtFacKN"
        Me.txtFacKN.Size = New System.Drawing.Size(60, 21)
        Me.txtFacKN.TabIndex = 30
        Me.txtFacKN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbNelConSeal
        '
        Me.cmbNelConSeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNelConSeal.Location = New System.Drawing.Point(136, 54)
        Me.cmbNelConSeal.Name = "cmbNelConSeal"
        Me.cmbNelConSeal.Size = New System.Drawing.Size(60, 21)
        Me.cmbNelConSeal.TabIndex = 983
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(30, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Sealing Region"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(248, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 20)
        Me.Label3.TabIndex = 986
        Me.Label3.Text = "Radial Region"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'IPE_frmFEAParamsCSeal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(467, 457)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "IPE_frmFEAParamsCSeal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " FEA  Parameters: C-Seal"
        Me.Panel1.ResumeLayout(False)
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

        '   No. of Layers thru' thickness:
        '   ------------------------------

        '....Base Material.
        For i = 1 To 7
            cmbNLayer.Items.Add(i)
        Next
        cmbNLayer.SelectedIndex = 0


        '....Plating.
        For i = 1 To 5
            cmbPlatingNLayer.Items.Add(i)
        Next
        cmbPlatingNLayer.SelectedIndex = 0


        '   No. Contact Elements:
        '   ---------------------

        '....Sealing Region.
        For i = 4 To 20
            cmbNelConSeal.Items.Add(i)
        Next
        cmbNelConSeal.SelectedIndex = 0


        '....Radial Constraint Region.  
        For i = 4 To 20
            cmbNelConRad.Items.Add(i)
        Next
        cmbNelConRad.SelectedIndex = 0

    End Sub

    '*******************************************************************************
    '*                       FORM EVENT ROUTINES - BEGIN                           *
    '*******************************************************************************

    Private Sub frmFEAParamsCSeal_Load(ByVal sender As System.Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles MyBase.Load
        '================================================================
        DisplayData()
    End Sub


    Private Sub DisplayData()
        '====================

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal)

            If .NLayer > 0 Then cmbNLayer.Text = .NLayer

            lblPlatingNLayer.Visible = .Plating.Exists
            cmbPlatingNLayer.Visible = .Plating.Exists

            If .Plating.Exists And .Plating.NLayer > 0 Then _
                cmbPlatingNLayer.Text = .Plating.NLayer

            If .NelConSeal > 0 Then cmbNelConSeal.Text = .NelConSeal
            If .NelConRad > 0 Then cmbNelConRad.Text = .NelConRad

            txtFacKN.Text = .FacKN
            txtFacTOLN.Text = .FacTOLN
            txtBetaT.Text = .BetaT

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
    '*                    CONTROL EVENT ROUTINES - BEGIN                           *
    '*******************************************************************************

    'SB 15SEP08
    Private Sub txt_KeyPress(ByVal sender As System.Object, _
                             ByVal e As System.Windows.Forms.KeyPressEventArgs) _
                             Handles txtFacKN.KeyPress, txtBetaT.KeyPress
        '==============================================================================

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
    '                    OK & CANCEL COMMAND BUTTONS - BEGIN                       '
    '-------------------------------------------------------------------------------

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()

        Me.Close()          'PB 21SEP08

    End Sub


    Private Sub SaveData()
        '=================

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal)

            If cmbNLayer.Text <> "" Then .NLayer = Val(cmbNLayer.Text)
            If .Plating.Exists And cmbPlatingNLayer.Text <> "" Then _
               .PlatingNLayer = Val(cmbPlatingNLayer.Text)

            If cmbNelConSeal.Text <> "" Then .NelConSeal = Val(cmbNelConSeal.Text)
            If cmbNelConRad.Text <> "" Then .NelConRad = Val(cmbNelConRad.Text)

            If txtFacKN.Text <> "" Then .FacKN = txtFacKN.Text
            If txtFacTOLN.Text <> "" Then .FacTOLN = txtFacTOLN.Text

            If txtBetaT.Text <> "" Then .BetaT = txtBetaT.Text

            '....Load sub-step parameters:
            If txtNSBSTP.Text <> "" Then .NSBSTP = txtNSBSTP.Text
            If txtNSBMX.Text <> "" Then .NSBMX = txtNSBMX.Text
            If txtNSBMN.Text <> "" Then .NSBMN = txtNSBMN.Text

        End With

    End Sub

    '-------------------------------------------------------------------------------
    '                    OK & CANCEL COMMAND BUTTONS - END                         '
    '-------------------------------------------------------------------------------


    '*******************************************************************************
    '*                    CONTROL EVENT ROUTINES - END                             *
    '*******************************************************************************


End Class
