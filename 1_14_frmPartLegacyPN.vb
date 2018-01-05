'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  frmPN_Entry                            '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  12DEC17                                '
'                                                                              '
'===============================================================================
Imports System.Globalization
Imports System.Linq

Public Class Part_frmLegacyPN
    Private mPNID As Integer = 0

    Public Sub New(ByVal PNID_In As Integer, Optional ByVal SealProcess_In As String = "PN")
        '========================================================================================

        MyBase.New()

        ' This call is required by the designer.
        InitializeComponent()

        mPNID = PNID_In

        ' Add any initialization after the InitializeComponent() call.

        '....Pressure Orientation.
        With cmbSealPOrient.Items
            .Clear()
            .Add("External")
            .Add("Internal")
        End With

        Me.Text = "Legacy P/N Entry Form"

        If (SealProcess_In = "PN") Then
            optOther.Visible = True
            txtOtherPN.Visible = True

        ElseIf (SealProcess_In = "Project") Then
            optOther.Visible = False
            txtOtherPN.Visible = False

        End If

    End Sub


    Private Sub frmPN_Entry_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===========================================================================================
        optOther.Checked = False
        gIsLegacyPNActive = True
        chkPlating.Checked = False
        cmbCataloguedPN_Part6.Enabled = chkPlating.Checked
        cmbCataloguedPN_Part7.Enabled = chkPlating.Checked

        'PopulateMatCodeComboBox(cmbCataloguedPN_Part4)
        PopulatePlatingType()
        PopulatePlatingTCode()
        cmbSealPOrient.SelectedIndex = 0

        cmbCataloguedPN_Part6.Enabled = False
        cmbCataloguedPN_Part6.DropDownStyle = ComboBoxStyle.DropDown
        cmbCataloguedPN_Part6.Text = ""

        cmbCataloguedPN_Part7.Enabled = False
        cmbCataloguedPN_Part7.DropDownStyle = ComboBoxStyle.DropDown
        cmbCataloguedPN_Part7.Text = ""

        DisplayData()
    End Sub


    Private Sub DisplayData()
        '====================
        Dim pPartEntities As New SealPartDBEntities()

        Dim pQry = (From pRec In pPartEntities.tblPN
                                  Where pRec.fldID = mPNID Select pRec).First()

        Dim pPN_Legacy As String = ""
        If (Not IsDBNull(pQry.fldLegacy) And Not IsNothing(pQry.fldLegacy)) Then
            pPN_Legacy = pQry.fldLegacy
            If (Not IsDBNull(pQry.fldLegacyType) And Not IsNothing(pQry.fldLegacyType)) Then
                'Dim pCatalogued As Boolean = pQry.fldLegacyType
                Dim pVal As Integer = 0
                pVal = pQry.fldLegacyType
                If (pVal = 0) Then
                    gPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
                ElseIf (pVal = 1) Then
                    gPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Other
                End If

                If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                    optCatalogued.Checked = True
                    Dim pPN_Legacy_Array() As String = pPN_Legacy.Split("-")
                    Dim pOrient As String = pPN_Legacy_Array(0).Substring(2, 1)
                    If (pOrient = "E") Then
                        cmbSealPOrient.SelectedIndex = 0
                    Else
                        cmbSealPOrient.SelectedIndex = 1
                    End If
                    Dim pSealType As String = pPN_Legacy_Array(0).Substring(1, 1)
                    If (pSealType = "C") Then
                        grpPlating.Enabled = True
                        cmbCataloguedPN_Part1.SelectedIndex = 0
                    ElseIf (pSealType = "S") Then
                        grpPlating.Enabled = True
                        cmbCataloguedPN_Part1.SelectedIndex = 1
                    ElseIf (pSealType = "E") Then
                        grpPlating.Enabled = False
                        cmbCataloguedPN_Part1.SelectedIndex = 2
                    ElseIf (pSealType = "U") Then
                        grpPlating.Enabled = False
                        cmbCataloguedPN_Part1.SelectedIndex = 3
                    End If

                    txtCataloguedPN_Part2.Text = pPN_Legacy_Array(1)
                    cmbCataloguedPN_Part3.Text = pPN_Legacy_Array(2)
                    cmbCataloguedPN_Part4.Text = pPN_Legacy_Array(3)
                    cmbCataloguedPN_Part5.Text = pPN_Legacy_Array(4)
                    If (pPN_Legacy_Array(5) <> "") Then
                        chkPlating.Checked = True
                        cmbCataloguedPN_Part6.Text = pPN_Legacy_Array(5).Substring(0, 2)
                        cmbCataloguedPN_Part7.Text = pPN_Legacy_Array(5).Substring(2, 1)
                    End If

                Else
                    optOther.Checked = True
                    txtOtherPN.Text = pPN_Legacy
                End If

            End If

        End If

    End Sub


    Private Sub PopulatePrefix()
        '========================
        cmbCataloguedPN_Part1.Items.Clear()

        If (cmbSealPOrient.SelectedIndex = 0) Then          '....External

            If (gPartUnit.System = "English") Then

                cmbCataloguedPN_Part1.Items.Add("ECE")
                cmbCataloguedPN_Part1.Items.Add("ESE")
                cmbCataloguedPN_Part1.Items.Add("EEE")
                cmbCataloguedPN_Part1.Items.Add("EUE")

            ElseIf (gPartUnit.System = "Metric") Then
                cmbCataloguedPN_Part1.Items.Add("MCE")
                cmbCataloguedPN_Part1.Items.Add("MSE")
                cmbCataloguedPN_Part1.Items.Add("MEE")
                cmbCataloguedPN_Part1.Items.Add("MUE")
            End If

        ElseIf (cmbSealPOrient.SelectedIndex = 1) Then      '....Internal

            If (gPartUnit.System = "English") Then

                cmbCataloguedPN_Part1.Items.Add("ECI")
                cmbCataloguedPN_Part1.Items.Add("ESI")
                cmbCataloguedPN_Part1.Items.Add("EEI")
                cmbCataloguedPN_Part1.Items.Add("EUI")

            ElseIf (gPartUnit.System = "Metric") Then
                cmbCataloguedPN_Part1.Items.Add("MCI")
                cmbCataloguedPN_Part1.Items.Add("MSI")
                cmbCataloguedPN_Part1.Items.Add("MEI")
                cmbCataloguedPN_Part1.Items.Add("MUI")
            End If

        End If

        cmbCataloguedPN_Part1.SelectedIndex = 0
    End Sub


    Private Sub PopulateCrossSection()
        '=============================
        Try
            cmbCataloguedPN_Part3.Items.Clear()

            Dim pMCSEntities As New SealIPEMCSDBEntities()

            Dim pSealType As String = cmbCataloguedPN_Part1.Text.ToString().Substring(1, 1).ToUpper()

            If (pSealType = "C") Then

                Dim pQry = (From pRec In pMCSEntities.tblCSeal_Geom
                                      Order By pRec.fldCCS Ascending Select pRec).ToList()
                If (pQry.Count() > 0) Then
                    For i As Integer = 0 To pQry.Count() - 1
                        Dim pCCS As String = pQry(i).fldCCS.ToString().Trim()

                        If (pCCS <> "") Then
                            If (pCCS.Length = 1) Then
                                pCCS = "0" + pCCS
                            End If
                            cmbCataloguedPN_Part3.Items.Add(pCCS)
                        End If
                    Next

                End If

            ElseIf (pSealType = "S") Then

                Dim pQry = (From pRec In pMCSEntities.tblCSeal_Geom
                            Where pRec.fldSpringEnergized = True Order By pRec.fldCCS Ascending Select pRec).ToList()

                If (pQry.Count() > 0) Then
                    For i As Integer = 0 To pQry.Count() - 1
                        Dim pCCS As String = pQry(i).fldCCS.ToString().Trim()

                        If (pCCS <> "") Then
                            If (pCCS.Length = 1) Then
                                pCCS = "0" + pCCS
                            End If
                            cmbCataloguedPN_Part3.Items.Add(pCCS)
                        End If
                    Next

                End If

            ElseIf (pSealType = "E") Then

                Dim pQry = (From pRec In pMCSEntities.tblESeal_Geom
                                     Order By pRec.fldCCS Ascending Select pRec).ToList()
                If (pQry.Count() > 0) Then
                    For i As Integer = 0 To pQry.Count() - 1
                        Dim pCCS As String = pQry(i).fldCCS.ToString().Trim()
                        If (pCCS <> "") Then
                            If (pCCS.Length = 1) Then
                                pCCS = "0" + pCCS
                            End If
                            cmbCataloguedPN_Part3.Items.Add(pCCS)
                        End If

                    Next

                End If

            ElseIf (pSealType = "U") Then

                Dim pQry = (From pRec In pMCSEntities.tblUSeal_Geom
                                    Order By pRec.fldCCS Ascending Select pRec).ToList()
                If (pQry.Count() > 0) Then
                    For i As Integer = 0 To pQry.Count() - 1
                        Dim pCCS As String = pQry(i).fldCCS.ToString().Trim()

                        If (pCCS <> "") Then
                            If (pCCS.Length = 1) Then
                                pCCS = "0" + pCCS
                            End If
                            cmbCataloguedPN_Part3.Items.Add(pCCS)
                        End If
                    Next

                End If

            End If

            If (cmbCataloguedPN_Part3.Items.Count > 0) Then
                cmbCataloguedPN_Part3.SelectedIndex = 0
            End If


        Catch ex As Exception

        End Try

    End Sub


    Private Sub PopulateMatCodeComboBox(ByVal SealType_In As String, ByRef cmbBox As ComboBox)
        '=====================================================================================
        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pMatList As New List(Of String)
        cmbBox.Items.Clear()

        Dim pUsedMatCodeType As New List(Of String)
        pUsedMatCodeType.Add("P")
        pUsedMatCodeType.Add("S")
        pUsedMatCodeType.Add("O")


        Select Case SealType_In
            Case "E"
                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                        Where pRec.fldE = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "C"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                        Where pRec.fldC = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "U"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                        Where pRec.fldU = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "S"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
                        Where pRec.fldC = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next


        End Select


        For i As Integer = 0 To pMatList.Count - 1
            Dim pMatName As String = pMatList(i)

            If (SealType_In = "S") Then
                Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
                       Where pRec.fldName = pMatName Select pRec).ToList()

                If (pQry.Count > 0) Then
                    Dim pMatCode As String = pQry(0).fldCode.ToString().Trim()

                    If (pMatCode <> "") Then
                        If (pMatCode.Length = 1) Then
                            pMatCode = "0" + pMatCode
                        End If
                        cmbBox.Items.Add(pMatCode)
                    End If

                End If
            Else
                Dim pQry = (From pRec In pMCSEntities.tblMaterial
                       Where pRec.fldName = pMatName Select pRec).ToList()

                If (pQry.Count > 0) Then
                    Dim pMatCode As String = pQry(0).fldCode.ToString().Trim()

                    If (pMatCode <> "") Then
                        If (pMatCode.Length = 1) Then
                            pMatCode = "0" + pMatCode
                        End If
                        cmbBox.Items.Add(pMatCode)
                    End If

                End If

            End If

        Next

        If (cmbBox.Items.Count > 0) Then
            cmbBox.SelectedIndex = 0
        End If

    End Sub


    Private Sub PopulateTemperCode(ByVal MatCode_In As Integer)
        '======================================================
        cmbCataloguedPN_Part5.Items.Clear()

        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pQry = (From pRec In pMCSEntities.tblMatTemper
                    Where pRec.fldMatCode = MatCode_In Select pRec).ToList()
        If (pQry.Count() > 0) Then
            For i As Integer = 0 To pQry.Count() - 1
                Dim pCode As String = pQry(i).fldTemperCode.ToString().Trim()

                If (pCode <> "") Then
                    cmbCataloguedPN_Part5.Items.Add(pCode)
                End If
            Next

        End If

        If (cmbCataloguedPN_Part5.Items.Count > 0) Then
            cmbCataloguedPN_Part5.SelectedIndex = 0
        End If

    End Sub


    Private Sub PopulatePlatingType()
        '============================
        cmbCataloguedPN_Part6.Items.Clear()

        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pQry = (From pRec In pMCSEntities.tblPlatingType
                    Order By pRec.fldPlatingCode Ascending Select pRec).ToList()

        If (pQry.Count() > 0) Then
            For i As Integer = 0 To pQry.Count() - 1
                Dim pCode As String = pQry(i).fldPlatingCode.ToString()

                If (pCode <> "") Then
                    cmbCataloguedPN_Part6.Items.Add(pCode)
                End If
            Next

        End If

        If (cmbCataloguedPN_Part6.Items.Count > 0) Then
            cmbCataloguedPN_Part6.SelectedIndex = 0
        End If

    End Sub


    Private Sub PopulatePlatingTCode()
        '============================
        cmbCataloguedPN_Part7.Items.Clear()

        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pQry = (From pRec In pMCSEntities.tblPlatingThick
                    Order By pRec.fldPlatingThickCode Ascending Select pRec).ToList()

        If (pQry.Count() > 0) Then
            For i As Integer = 0 To pQry.Count() - 1
                Dim pCode As String = pQry(i).fldPlatingThickCode.ToString()

                If (pCode <> "") Then
                    cmbCataloguedPN_Part7.Items.Add(pCode)
                End If
            Next
            cmbCataloguedPN_Part7.Items.Add("X")

        End If

        If (cmbCataloguedPN_Part7.Items.Count > 0) Then
            cmbCataloguedPN_Part7.SelectedIndex = 0
        End If

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
            gPartProject.PNR.Legacy_Exists = True
            gPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
            Dim pDia As String = txtCataloguedPN_Part2.Text.Replace(" ", 0)
            Dim pDia_Act As String = pDia
            For i As Integer = pDia.Length To 5
                pDia_Act = pDia_Act + "0"
            Next

            Dim pSealType As String = cmbCataloguedPN_Part1.Text.ToString().Substring(1, 1).ToUpper()

            Dim pTemperCode As String = 0
            If (cmbCataloguedPN_Part5.Text <> "") Then
                pTemperCode = cmbCataloguedPN_Part5.Text
            End If

            If (pSealType = "C" Or pSealType = "S") Then
                pPN = cmbCataloguedPN_Part1.Text + "-" + pDia_Act + "-" + cmbCataloguedPN_Part3.Text + "-" +
                  cmbCataloguedPN_Part4.Text + "-" + pTemperCode + "-" + cmbCataloguedPN_Part6.Text + cmbCataloguedPN_Part7.Text
            Else
                pPN = cmbCataloguedPN_Part1.Text + "-" + pDia_Act + "-" + cmbCataloguedPN_Part3.Text + "-" +
                  cmbCataloguedPN_Part4.Text + "-" + pTemperCode + "-" + "00" + "0"
            End If

            ''Dim pMatCode As Integer = Convert.ToInt16(cmbCataloguedPN_Part4.Text)
            ''Dim pMatName As String = gProject.GetMatName(pSealType, pMatCode)

            ''Dim pIsMatExists As Boolean = False
            ''For i As Integer = 0 To gFile.MatList_Prop.Count - 1
            ''    If (gFile.MatList_Prop(i) = pMatName) Then
            ''        pIsMatExists = True
            ''        Exit For
            ''    End If
            ''Next

            ''gProject.GeomTemplate = pIsMatExists


        Else
            gPartProject.PNR.Legacy_Exists = True
            gPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Other
            pPN = txtOtherPN.Text
        End If

        gPartProject.PNR.Legacy_Val = pPN

    End Sub


    Private Sub cmbCataloguedPN_Part1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                           Handles cmbCataloguedPN_Part1.SelectedIndexChanged
        '====================================================================================================
        PopulateCrossSection()

        Dim pSealType As String = cmbCataloguedPN_Part1.Text.ToString().Substring(1, 1).ToUpper()

        If (pSealType = "C" Or pSealType = "S") Then
            grpPlating.Enabled = True
        Else
            grpPlating.Enabled = False
        End If

        PopulateMatCodeComboBox(pSealType, cmbCataloguedPN_Part4)

        chkPlating.Checked = False
        cmbCataloguedPN_Part6.Enabled = False
        cmbCataloguedPN_Part6.DropDownStyle = ComboBoxStyle.DropDown
        cmbCataloguedPN_Part6.Text = ""

        cmbCataloguedPN_Part7.Enabled = False
        cmbCataloguedPN_Part7.DropDownStyle = ComboBoxStyle.DropDown
        cmbCataloguedPN_Part7.Text = ""

    End Sub


    Private Sub cmbCataloguedPN_Part4_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                            Handles cmbCataloguedPN_Part4.SelectedIndexChanged
        '=====================================================================================================
        Dim pMatCode As Integer = Convert.ToInt16(cmbCataloguedPN_Part4.Text)
        PopulateTemperCode(pMatCode)
    End Sub


    Private Sub chkPlating_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                         Handles chkPlating.CheckedChanged
        '==================================================================================
        cmbCataloguedPN_Part6.Enabled = chkPlating.Checked
        cmbCataloguedPN_Part7.Enabled = chkPlating.Checked

        If (chkPlating.Checked) Then
            cmbCataloguedPN_Part6.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part7.DropDownStyle = ComboBoxStyle.DropDownList

            cmbCataloguedPN_Part6.SelectedIndex = 0
            cmbCataloguedPN_Part7.SelectedIndex = 0

        Else
            cmbCataloguedPN_Part6.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part6.Text = ""

            cmbCataloguedPN_Part7.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part7.Text = ""

        End If

    End Sub

    Private Sub cmbCataloguedPN_Part4_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) _
                                                Handles cmbCataloguedPN_Part4.DrawItem
        '==============================================================================================================
        e.DrawBackground()

        ' Get the item text    
        Dim text As String = (CType(sender, ComboBox)).Items(e.Index).ToString()

        ' Determine the forecolor based on whether or not the item is selected    
        Dim brush As Brush
        If e.Index = 0 Then
            brush = Brushes.Green
        Else
            brush = Brushes.Black
        End If

        ' Draw the text    
        e.Graphics.DrawString(text, (CType(sender, Control)).Font, brush, e.Bounds.X, e.Bounds.Y)

    End Sub

    Private Sub optOther_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                        Handles optOther.CheckedChanged
        '=================================================================================
        If optOther.Checked Then

            cmbCataloguedPN_Part1.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part1.Text = ""
            cmbCataloguedPN_Part1.Enabled = False

            txtCataloguedPN_Part2.Text = ""
            txtCataloguedPN_Part2.Enabled = False

            cmbCataloguedPN_Part3.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part3.Text = ""
            cmbCataloguedPN_Part3.Enabled = False

            cmbCataloguedPN_Part4.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part4.Text = ""
            cmbCataloguedPN_Part4.Enabled = False

            cmbCataloguedPN_Part5.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part5.Text = ""
            cmbCataloguedPN_Part5.Enabled = False

            chkPlating.Enabled = False
            cmbCataloguedPN_Part6.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part6.Text = ""
            cmbCataloguedPN_Part6.Enabled = False

            cmbCataloguedPN_Part7.DropDownStyle = ComboBoxStyle.DropDown
            cmbCataloguedPN_Part7.Text = ""
            cmbCataloguedPN_Part7.Enabled = False

        Else
            cmbCataloguedPN_Part1.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part1.SelectedIndex = 0
            cmbCataloguedPN_Part1.Enabled = True

            txtCataloguedPN_Part2.Text = ""
            txtCataloguedPN_Part2.Enabled = True

            cmbCataloguedPN_Part3.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part3.SelectedIndex = 0
            cmbCataloguedPN_Part3.Enabled = True

            cmbCataloguedPN_Part4.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part4.SelectedIndex = 0
            cmbCataloguedPN_Part4.Enabled = True

            cmbCataloguedPN_Part5.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part5.SelectedIndex = 0
            cmbCataloguedPN_Part5.Enabled = True

            chkPlating.Enabled = True
            cmbCataloguedPN_Part6.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part6.SelectedIndex = 0
            cmbCataloguedPN_Part6.Enabled = True

            cmbCataloguedPN_Part7.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCataloguedPN_Part7.SelectedIndex = 0
            cmbCataloguedPN_Part7.Enabled = True

        End If

    End Sub

End Class