'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmPN_Entry                            '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28MAR17                                '
'                                                                              '
'===============================================================================
Imports System.Globalization
Imports System.Linq
Public Class IPE_frmPN_Entry

    Public Sub New(Optional ByVal SealProcess_In As String = "SealIPE")
        '===============

        MyBase.New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '....Pressure Orientation.
        With cmbSealPOrient.Items
            .Clear()
            .Add("External")
            .Add("Internal")
        End With

        If (SealProcess_In = "SealTest") Then
            Me.Text = "SealTest: Legacy P/N Entry Form"
        Else
            optOther.Visible = False
            txtOtherPN.Visible = False

        End If

    End Sub


    Private Sub frmPN_Entry_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==============================================================================================
        cmbSealPOrient.SelectedIndex = 0
        DisplayData()
    End Sub

    Private Sub DisplayData()
        '====================
        ''Dim pProjectEntities As New ProjectDBEntities()

        ''Dim pQry = (From pRec In pProjectEntities.tblPN
        ''                           Where pRec.fldID = gIPE_Project.PN_ID Select pRec).First()
        ''Dim pPN_Legacy As String = ""
        ''If (Not IsDBNull(pQry.fldLegacy) And Not IsNothing(pQry.fldLegacy)) Then
        ''    pPN_Legacy = pQry.fldLegacy
        ''    If (Not IsDBNull(pQry.fldLegacyType) And Not IsNothing(pQry.fldLegacyType)) Then
        ''        Dim pCatalogued As Boolean = pQry.fldLegacyType
        ''        If (pCatalogued) Then
        ''            optCatalogued.Checked = True
        ''            Dim pPN_Legacy_Array() As String = pPN_Legacy.Split("-")
        ''            Dim pOrient As String = pPN_Legacy_Array(0).Substring(2, 1)
        ''            If (pOrient = "E") Then
        ''                cmbSealPOrient.SelectedIndex = 0
        ''            Else
        ''                cmbSealPOrient.SelectedIndex = 1
        ''            End If
        ''            Dim pSealType As String = pPN_Legacy_Array(0).Substring(1, 1)
        ''            If (pSealType = "C") Then
        ''                cmbCataloguedPN_Part1.SelectedIndex = 0
        ''            ElseIf (pSealType = "S") Then
        ''                cmbCataloguedPN_Part1.SelectedIndex = 1
        ''            ElseIf (pSealType = "E") Then
        ''                cmbCataloguedPN_Part1.SelectedIndex = 2
        ''            ElseIf (pSealType = "U") Then
        ''                cmbCataloguedPN_Part1.SelectedIndex = 3
        ''            End If

        ''            txtCataloguedPN_Part2.Text = pPN_Legacy_Array(1)
        ''            cmbCataloguedPN_Part3.Text = pPN_Legacy_Array(2)
        ''            cmbCataloguedPN_Part4.Text = pPN_Legacy_Array(3)
        ''            cmbCataloguedPN_Part5.Text = pPN_Legacy_Array(4)
        ''            If (pPN_Legacy_Array(5) <> "") Then
        ''                chkPlating.Checked = True
        ''                cmbCataloguedPN_Part6.Text = pPN_Legacy_Array(5).Substring(0, 2)
        ''                cmbCataloguedPN_Part7.Text = pPN_Legacy_Array(5).Substring(2, 1)
        ''            End If

        ''        Else
        ''            optOther.Checked = True
        ''            txtOtherPN.Text = pPN_Legacy
        ''        End If

        ''    End If

        ''End If




    End Sub

    Private Sub PopulatePrefix()
        '========================
        cmbCataloguedPN_Part1.Items.Clear()
        If (cmbSealPOrient.SelectedIndex = 0) Then
            If (gIPE_Unit.System = "English") Then
                cmbCataloguedPN_Part1.Items.Add("ECE")
                cmbCataloguedPN_Part1.Items.Add("ESE")
                cmbCataloguedPN_Part1.Items.Add("EEE")
                cmbCataloguedPN_Part1.Items.Add("EUE")
            ElseIf (gIPE_Unit.System = "Metric") Then
                cmbCataloguedPN_Part1.Items.Add("MCE")
                cmbCataloguedPN_Part1.Items.Add("MSE")
                cmbCataloguedPN_Part1.Items.Add("MEE")
                cmbCataloguedPN_Part1.Items.Add("MUE")
            End If

        ElseIf (cmbSealPOrient.SelectedIndex = 1) Then
            If (gIPE_Unit.System = "English") Then
                cmbCataloguedPN_Part1.Items.Add("ECI")
                cmbCataloguedPN_Part1.Items.Add("ESI")
                cmbCataloguedPN_Part1.Items.Add("EEI")
                cmbCataloguedPN_Part1.Items.Add("EUI")
            ElseIf (gIPE_Unit.System = "Metric") Then
                cmbCataloguedPN_Part1.Items.Add("MCI")
                cmbCataloguedPN_Part1.Items.Add("MSI")
                cmbCataloguedPN_Part1.Items.Add("MEI")
                cmbCataloguedPN_Part1.Items.Add("MUI")
            End If

        End If

        cmbCataloguedPN_Part1.SelectedIndex = 0
    End Sub

    Private Sub cmbSealPOrient_SelectedIndexChanged(sender As System.Object,
                                                    e As System.EventArgs) Handles cmbSealPOrient.SelectedIndexChanged
        '==============================================================================================================
        PopulatePrefix()
    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '========================================================================================
        SaveData()
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '===============================================================================================
        Me.Close()
    End Sub


    Private Sub SaveData()
        '==================
        Dim pPN As String = ""
        If (optCatalogued.Checked) Then
            gIPE_Project.Catalogued = True
            Dim pDia As String = txtCataloguedPN_Part2.Text.Replace(" ", 0)
            Dim pDia_Act As String = pDia
            For i As Integer = pDia.Length To 5
                pDia_Act = pDia_Act + "0"
            Next

            pPN = cmbCataloguedPN_Part1.Text + "-" + pDia_Act + "-" + cmbCataloguedPN_Part3.Text + "-" +
                  cmbCataloguedPN_Part4.Text + "-" + cmbCataloguedPN_Part5.Text + "-" + cmbCataloguedPN_Part6.Text + cmbCataloguedPN_Part7.Text
        Else
            gIPE_Project.Catalogued = False

            pPN = txtOtherPN.Text
        End If

        gIPE_Project.PN_Legacy = pPN

    End Sub


End Class