'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                       FORM MODULE  :  frmPlating                             '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  21MAR16                                ' 
'                                                                              '
'===============================================================================

Imports System.Data.OleDb
Imports System.Math
Imports System.Linq
Imports clsLibrary11
Imports SealIPELib = SealIPELib101

Public Class IPE_frmPlating
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mPlateThickNom As Single    '....Nom. Thick. for the selected Plating Code.

#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents cmbPlatingCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label0 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbPlatingThickCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents lblUnit As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtPlatingThick As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmPlating))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.lblUnit = New System.Windows.Forms.Label()
        Me.txtPlatingThick = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbPlatingThickCode = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbPlatingCode = New System.Windows.Forms.ComboBox()
        Me.Label0 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(3, 3)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(352, 134)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.lblUnit)
        Me.Panel1.Controls.Add(Me.txtPlatingThick)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cmbPlatingThickCode)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbPlatingCode)
        Me.Panel1.Controls.Add(Me.Label0)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(350, 132)
        Me.Panel1.TabIndex = 1
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(176, 92)
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
        Me.cmdCancel.Location = New System.Drawing.Point(264, 92)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 53
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lblUnit
        '
        Me.lblUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnit.Location = New System.Drawing.Point(305, 42)
        Me.lblUnit.Name = "lblUnit"
        Me.lblUnit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnit.Size = New System.Drawing.Size(36, 16)
        Me.lblUnit.TabIndex = 31
        Me.lblUnit.Text = "lblUnit"
        '
        'txtPlatingThick
        '
        Me.txtPlatingThick.AcceptsReturn = True
        Me.txtPlatingThick.BackColor = System.Drawing.Color.White
        Me.txtPlatingThick.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPlatingThick.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlatingThick.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPlatingThick.Location = New System.Drawing.Point(248, 40)
        Me.txtPlatingThick.MaxLength = 0
        Me.txtPlatingThick.Name = "txtPlatingThick"
        Me.txtPlatingThick.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlatingThick.Size = New System.Drawing.Size(54, 21)
        Me.txtPlatingThick.TabIndex = 28
        Me.txtPlatingThick.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label2.Location = New System.Drawing.Point(196, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 16)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Value"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbPlatingThickCode
        '
        Me.cmbPlatingThickCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPlatingThickCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPlatingThickCode.Location = New System.Drawing.Point(130, 40)
        Me.cmbPlatingThickCode.Name = "cmbPlatingThickCode"
        Me.cmbPlatingThickCode.Size = New System.Drawing.Size(40, 21)
        Me.cmbPlatingThickCode.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(124, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Thick"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbPlatingCode
        '
        Me.cmbPlatingCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPlatingCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPlatingCode.Location = New System.Drawing.Point(40, 40)
        Me.cmbPlatingCode.Name = "cmbPlatingCode"
        Me.cmbPlatingCode.Size = New System.Drawing.Size(54, 21)
        Me.cmbPlatingCode.TabIndex = 11
        '
        'Label0
        '
        Me.Label0.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label0.Location = New System.Drawing.Point(40, 20)
        Me.Label0.Name = "Label0"
        Me.Label0.Size = New System.Drawing.Size(48, 16)
        Me.Label0.TabIndex = 12
        Me.Label0.Text = "Code"
        Me.Label0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'IPE_frmPlating
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(358, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmPlating"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Plating"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


#Region "FORM EVENT RELATED ROUTINES:"

    Private Sub frmPlating_Load(ByVal sender As Object, _
                                ByVal e As System.EventArgs) _
                                Handles MyBase.Load
        '======================================================
        '....Populate Combo Boxes:

        PopulateCmbbox()

        DisplayData()
        lblUnit.Text = gIPE_Unit.UserL

    End Sub

#Region "HELPER ROUTINES:"


    Private Sub PopulateCmbbox()
        '=======================   
        '....This routine populates the Cross Sec No. combo box. (Database Driven).

        'AES 21MAAR16
        'Dim pSealCADDBEntities As New SealCADDBEntities()
        Dim pSealMCSDB As New SealIPEMCSDBEntities()

        'Dim pQryPlatingType = (From it In pSealCADDBEntities.tblPlatingType
        '                            Order By it.fldPlatingCode Ascending Select it).Distinct()

        Dim pQryPlatingType = (From it In pSealMCSDB.tblPlatingType
                                   Select it.fldPlatingCode Distinct).ToList()
        cmbPlatingCode.Items.Clear()
        For i As Integer = 0 To pQryPlatingType.Count() - 1
            cmbPlatingCode.Items.Add(pQryPlatingType(i))
        Next

        'Dim pQryPlatingThick = (From it In pSealCADDBEntities.tblPlatingThick
        '                            Order By it.fldPlatingThickCode Ascending Select it).Distinct()
        'Dim pRecThick As tblPlatingThick
        'cmbPlatingThickCode.Items.Clear()
        'For Each pRecThick In pQryPlatingThick
        '    cmbPlatingThickCode.Items.Add(pRecThick.fldPlatingThickCode)
        'Next

        Dim pQryPlatingThick = (From it In pSealMCSDB.tblPlatingThick
                                 Select it.fldPlatingThickCode Distinct).ToList()
        cmbPlatingThickCode.Items.Clear()
        For i As Integer = 0 To pQryPlatingThick.Count() - 1
            cmbPlatingThickCode.Items.Add(pQryPlatingThick(i))
        Next

    End Sub


    Private Sub DisplayData()
        '====================

        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal).Plating

            'Set Combo boxes:
            '----------------
            '....Code:
            If .Code <> "" Then
                cmbPlatingCode.Text = .Code
            Else
                cmbPlatingCode.SelectedIndex = 0
            End If


            '....Thickness Code:
            '........The following assignment triggers "SelectedIndexChanged" Event.
            '
            If .ThickCode <> "" Then
                cmbPlatingThickCode.Text = .ThickCode
            Else
                cmbPlatingThickCode.SelectedIndex = 0
            End If


            'Thickness Value.
            '----------------------   
            If Abs(.Thick) > gcEPS Then
                Dim pThick_UserU As Single
                pThick_UserU = gIPE_Unit.L_ConToUser(.Thick)     '....in User Unit (in or mm).
                txtPlatingThick.Text = Format(pThick_UserU, gIPE_Unit.TFormat)
            End If

        End With

    End Sub

#End Region

#End Region


#Region "CONTROL EVENT ROUTINES:"

#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbPlatingThickCode_SelectedIndexChanged(ByVal sender As System.Object, _
                                                         ByVal e As System.EventArgs) _
                                        Handles cmbPlatingThickCode.SelectedIndexChanged
        '===============================================================================
        GetAndDisplay_PlatingThick()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub GetAndDisplay_PlatingThick()
        '==================================
        Dim pThickCode As String
        pThickCode = cmbPlatingThickCode.Text.Trim()

        Dim pThickMin As Double, pThickMax As Double
        Dim pThickMinMet As Double, pThickMaxMet As Double
        RetrieveThicknesses(pThickCode, pThickMin, pThickMax, pThickMinMet, pThickMaxMet)


        '....Display Nominal Thickness Value.
        DisplayNomThickness(pThickMin, pThickMax, pThickMinMet, pThickMaxMet)

    End Sub

#Region "SUB-HELPER ROUTINES:"

    Private Sub RetrieveThicknesses(ByVal ThickCode_In As String, _
                                    ByRef ThickMin_Out As Double, _
                                    ByRef ThickMax_Out As Double, _
                                    ByRef ThickMinMet_Out As Double, _
                                    ByRef ThickMaxMet_Out As Double)
        '===============================================================

        ''For a given plating thickness code, this subroutine returns the min and max
        ''....plating thicknessess from the database.

        Dim pSealMCSDB As New SealIPEMCSDBEntities()

        Dim pQryPlatingThick = (From it In pSealMCSDB.tblPlatingThick
                                   Where it.fldPlatingThickCode = ThickCode_In
                                   Order By it.fldPlatingThickCode Ascending Select it).First()

        ThickMin_Out = pQryPlatingThick.fldPlatingThickMinEng
        ThickMax_Out = pQryPlatingThick.fldPlatingThickMaxEng

        '....Metric Unit (User Unit): 
        ThickMinMet_Out = pQryPlatingThick.fldPlatingThickMinMet
        ThickMaxMet_Out = pQryPlatingThick.fldPlatingThickMaxMet

    End Sub


    Private Sub DisplayNomThickness(ByVal ThickMin_In As Single, _
                                     ByVal ThickMax_In As Single, _
                                     ByVal ThickMinMet_In As Single, _
                                     ByVal ThickMaxMet_In As Single)
        '==========================================================================

        If gIPE_Unit.System = "English" Then
            mPlateThickNom = 0.5 * (ThickMin_In + ThickMax_In)

        ElseIf gIPE_Unit.System = "Metric" Then     '....In user unit.
            mPlateThickNom = 0.5 * (ThickMinMet_In + ThickMaxMet_In)
        End If

        txtPlatingThick.Text = gIPE_Unit.WriteInUserL(mPlateThickNom, "TFormat")

    End Sub

#End Region

#End Region

#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtPlatingThick_TextChanged(ByVal sender As System.Object, _
                                           ByVal e As System.EventArgs) _
                                           Handles txtPlatingThick.TextChanged
        '=======================================================================
        '....Increased from 0.0005 to 0.00051.  Resolves DR V41, Error 16. 15NOV06.
        '........This is to take care of round-off error, while reading value from the 
        '........text box. 

        Const pcERROR_ROUND_OFF_UserL_Eng As Single = 0.000051  '....inch.
        Const pcERROR_ROUND_OFF_UserL_Met As Single = 0.00051   '....mm.
        Dim pERROR_ROUND_OFF_UserL As Single

        If gIPE_Unit.System = "English" Then
            pERROR_ROUND_OFF_UserL = pcERROR_ROUND_OFF_UserL_Eng
        ElseIf gIPE_Unit.System = "Metric" Then
            pERROR_ROUND_OFF_UserL = pcERROR_ROUND_OFF_UserL_Met
        End If

        If Abs(Val(txtPlatingThick.Text) - mPlateThickNom) < pERROR_ROUND_OFF_UserL Then
            txtPlatingThick.ForeColor = Color.Magenta       '....(DEFAULT VALUE)
        Else
            txtPlatingThick.ForeColor = Color.Black
        End If

    End Sub


    Private Sub txt_KeyPress(ByVal sender As System.Object, _
                            ByVal e As System.Windows.Forms.KeyPressEventArgs) _
                            Handles txtPlatingThick.KeyPress
        '=========================================================================

        'Dim pCulture = gUserInfo.CultureName
        Dim pCulture = gIPE_Project.CultureName

        Select Case pCulture
            Case "USA", "UK"
                If e.KeyChar = "," Then e.KeyChar = "."
            Case "Germany", "France"
                If e.KeyChar = "." Then e.KeyChar = ","
        End Select

    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '====================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        'AES 20SEP16
        'Dim pBlnForm_Remain_Open As Boolean = False

        'If pCmdBtn.Name = "cmdOK" Then SaveData(pBlnForm_Remain_Open)
        'If pBlnForm_Remain_Open = False Then Me.Close()

        'AES 20SEP16
        If pCmdBtn.Name = "cmdOK" Then SaveData()
        Me.Close()

    End Sub

#Region "HELPER ROUTINES:"

    'Private Sub SaveData(ByRef bln_Remain_Open As Boolean)
    Private Sub SaveData()
        '=================  'AES 20SEP16
        Dim pDescrip As String = ""
        Dim pName As String = ""
        RetrieveNameAndDescrip(cmbPlatingCode.Text, pDescrip, pName)

        'AES 20SEP16
        'Select Case cmbPlatingCode.Text
        '    Case "CP", "SC"
        '        '----------
        '        bln_Remain_Open = False

        '    Case Else
        '        '----
        '        Dim pstrMsg As String = "Currently, the plating material database  " & _
        '                    "contians the properties of Cu Plating only." & vbCrLf & _
        '                    "Hence, the program overrides the selected Plating Code " & _
        '                    "and instead, selects CP ."
        '        MessageBox.Show(pstrMsg, "Plating Code Error", MessageBoxButtons.OK, _
        '                                                       MessageBoxIcon.Error)
        '        cmbPlatingCode.Text = "CP"
        '        bln_Remain_Open = True

        'End Select


        With CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsCSeal)
            .PlatingCode = cmbPlatingCode.Text
            .PlatingDescrip = pDescrip
            .PlatingThickCode = cmbPlatingThickCode.Text
            .PlatingThick = gIPE_Unit.L_UserToCon(txtPlatingThick.Text)
        End With

    End Sub

#End Region

#End Region


#End Region


#Region "UTILITY ROUTINES:"

    Public Sub RetrieveNameAndDescrip(ByVal Code_In As String, _
                                      ByRef Descrip_Out As String, _
                                      ByRef Name_Out As String)
        '================================================================================
        ''....Given a Plating Code, this routine retreives the corresponding description &
        ''........name from the database table.

        If Code_In = "" Then Exit Sub

        'AES 21MAR16
        'Dim pSealCADDBEntities As New SealCADDBEntities()
        Dim pSealMCSDB As New SealIPEMCSDBEntities()

        Dim pQryPlatingType = (From it In pSealMCSDB.tblPlatingType
                                    Order By it.fldPlatingCode = Code_In Select it).First()

        Descrip_Out = pQryPlatingType.fldPlatingDescrip
        Name_Out = GetPlatingName(Descrip_Out)

    End Sub


    Private Function GetPlatingName(ByVal strDescrip As String) As String
        '================================================================
        'This routine gets plating name from its description.

        GetPlatingName = ""

        Dim pstrSearch As String
        pstrSearch = "Plate"

        Dim iPos As Integer
        iPos = InStr(strDescrip, pstrSearch)

        If iPos <> 0 Then
            Dim jPos As Integer
            jPos = InStrRev(strDescrip, " ", iPos - 2)

            Dim nChar As Integer
            nChar = (iPos + 5) - jPos
            GetPlatingName = Mid(strDescrip, jPos + 1, nChar)

            Exit Function
        End If


        '....Special case.
        pstrSearch = "Teflon Coat"
        iPos = InStr(strDescrip, pstrSearch)

        If iPos > 0 Then
            GetPlatingName = pstrSearch
        End If

    End Function

#End Region


End Class
