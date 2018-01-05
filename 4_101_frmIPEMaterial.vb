
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmMaterial                            '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28APR16                                '
'                                                                              '
'===============================================================================

Imports System.Drawing.Printing
Imports SealIPELib = SealIPELib101


Public Class IPE_frmMaterial

#Region "MEMBER VARIABLES:"

    Dim mblnCoating As Boolean = False

#End Region


    Public Sub New()
        '============                                   

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub PopulateCoating(ByVal MatName_In As String)
        '================================================== 
        '....Populate Coating Combo Box:
        With cmbCoating
            .Items.Clear()

            If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.IsCoatingExists(MatName_In, "Tricom") Then
                .Items.Add("Tricom")
            End If
            If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.IsCoatingExists(MatName_In, "T800") Then
                .Items.Add("T800")
            End If

        End With


    End Sub

    Private Sub frmMaterial_Load(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles MyBase.Load
        '======================================================= 
        '....Material List
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.PopulateMaterialList(cmbMatName)


        chkCoating.Checked = False             '....Initialise Coating CheckBox

        '....As Metric is not working now
        If gIPE_Unit.System <> "English" Then
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.UnitSystem = "English"
        End If

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat Is Nothing = False Then
            '============================
            With gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat
                If .Name Is Nothing = False Then
                    cmbMatName.SelectedIndex = IIf(.Name <> "", cmbMatName.Items.IndexOf(.Name), 0)
                Else
                    cmbMatName.SelectedIndex = 0
                End If


            End With
        End If

    End Sub


    Private Sub cmbMatName_SelectedIndexChanged(ByVal sender As System.Object, _
                                                ByVal e As System.EventArgs) _
                                                Handles cmbMatName.SelectedIndexChanged
        '==============================================================================

        Dim pMatName As String = cmbMatName.Text.Trim()
        Dim pCoating As String = "None"

        PopulateCoating(pMatName)

        mblnCoating = IIf(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating <> "" And _
                          gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating <> "None" And _
                          gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.IsCoatingExists(pMatName, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating), True, False)

        chkCoating.Checked = mblnCoating
        cmbCoating.Visible = mblnCoating


        If mblnCoating Then
            cmbCoating.SelectedIndex = cmbCoating.Items.IndexOf(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating)
            pCoating = cmbCoating.Text.Trim()
        Else
            If cmbCoating.Items.Count > 0 Then
                cmbCoating.SelectedIndex = 0
            End If
        End If


        If pMatName <> "" Then
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.RetrieveProp(pMatName, pCoating)
            DisplayData()
        End If


    End Sub


    Private Sub chkCoating_CheckedChanged(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles chkCoating.CheckedChanged
        '===================================================================

        Dim pMatName As String = cmbMatName.Text.Trim()
        Dim pCoating As String = "None"


        If chkCoating.Checked Then
            If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating <> "" And _
                gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating <> "None" And _
                    cmbCoating.Items.Contains(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating) Then

                cmbCoating.SelectedIndex = cmbCoating.Items.IndexOf(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating)
                chkCoating.Checked = True
                pCoating = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating

            Else
                cmbCoating.SelectedIndex = 0
                pCoating = cmbCoating.Items(0)
            End If
        End If

        cmbCoating.Visible = chkCoating.Checked
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat Is Nothing = False And pMatName <> "" Then
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.RetrieveProp(pMatName, pCoating)
            DisplayData()
        End If
    End Sub


    Private Sub cmbCoating_SelectedIndexChanged(ByVal sender As System.Object, _
                                                ByVal e As System.EventArgs) _
                                                Handles cmbCoating.SelectedIndexChanged
        '=============================================================================== 
        Dim pMatName As String = cmbMatName.Text.Trim()
        Dim pCoating As String = "None"

        If chkCoating.Checked Then
            pCoating = cmbCoating.Text.Trim()
            'Dim pblnExists As Boolean = gMat.IsCoatingExists(pMatName, pCoating)

            'If Not pblnExists Then
            '    Dim pMsg As String = "There is no value for '" + pCoating + "' coating in the material file."
            '    MessageBox.Show(pMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            '    chkCoating.Checked = False
            '    pCoating = "None"
            'End If

        End If
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat Is Nothing = False And pMatName <> "" Then
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.RetrieveProp(pMatName, pCoating)
            DisplayData()
        End If

    End Sub


    Private Sub DisplayData()
        '====================
        txtDensity.Text = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.SpWt.ToString("#0.000")

        Dim pNu As String, pEMod As String, pSigmaY As String, pTanMod As String
        Dim i As Integer

        grdProperty.Rows.Clear()

        For i = 1 To gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.NTemp
            pNu = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Nu(i).ToString("#0.000") ' gIPE_Unit.WriteInUserL(gIPE_Project.Analysis.Seal.Mat.Nu(i), "#0.000")
            pEMod = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Emod(i).ToString("#0.0") 'gIPE_Unit.WriteInUserL(gIPE_Project.Analysis.Seal.Mat.Emod(i), "#0.0")
            pSigmaY = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.SigmaY(i).ToString("#0.0") ' gIPE_Unit.WriteInUserL(gMat.SigmaY(i), "#0.0")
            pTanMod = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.TanMod(i).ToString("#0.00") 'gIPE_Unit.WriteInUserL(gMat.TanMod(i), "#0.0")
            Dim pRow() As String = {gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Temp(i).ToString(), pNu, pEMod, pSigmaY, pTanMod}
            Dim pRows As DataGridViewRowCollection = grdProperty.Rows
            grdProperty.AllowUserToAddRows = True
            pRows.Add(pRow)
        Next

        grdProperty.AllowUserToAddRows = False

    End Sub

    Private Sub cmdPrintForm_Click(ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs) _
                                   Handles cmdPrintForm.Click
        '==============================================================
        Dim pPrintDoc As New PrintDocument
        AddHandler pPrintDoc.PrintPage, AddressOf OnPrintPage
        pPrintDoc.Print()

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                              ByVal e As System.EventArgs) _
                              Handles cmdOK.Click, cmdCancel.Click
        '===========================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()
        Me.Close()

    End Sub


    Private Sub SaveData()
        '=================
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Name = cmbMatName.Text.Trim()

        If chkCoating.Checked Then
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating = cmbCoating.Text.Trim()
        Else
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Coating = "None"
        End If

        If gIPE_Unit.System <> "English" Then
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.UnitSystem = gIPE_Unit.System
        End If

    End Sub


    Private Sub OnPrintPage(ByVal sender As System.Object, _
                            ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '============================================================================

        Dim hwndForm As IntPtr
        hwndForm = Me.Handle

        Dim hdcDIBSection As IntPtr
        Dim hdcRef As IntPtr
        Dim hbmDIBSection As IntPtr
        Dim hbmDIBSectionOld As IntPtr
        Dim BMPheader As IPE_clsAPICalls.BITMAPINFOHEADER

        hdcRef = IPE_clsAPICalls.GetDC(IntPtr.Zero)
        hdcDIBSection = IPE_clsAPICalls.CreateCompatibleDC(hdcRef)
        IPE_clsAPICalls.ReleaseDC(IntPtr.Zero, hdcRef)

        BMPheader.biBitCount = 24
        BMPheader.biClrImportant = 0
        BMPheader.biClrUsed = 0
        BMPheader.biCompression = IPE_clsAPICalls.BI_RGB
        BMPheader.biSize = 40
        BMPheader.biHeight = Me.Height
        BMPheader.biPlanes = 1
        BMPheader.biSizeImage = 0
        BMPheader.biWidth = Me.Width
        BMPheader.biXPelsPerMeter = 0
        BMPheader.biYPelsPerMeter = 0

        hbmDIBSection = IPE_clsAPICalls.CreateDIBSection(hdcDIBSection, BMPheader, _
                                                      IPE_clsAPICalls.DIB_RGB_COLORS, _
                                                      IntPtr.Zero, IntPtr.Zero, 0)

        hbmDIBSectionOld = IPE_clsAPICalls.SelectObject(hdcDIBSection, hbmDIBSection)
        IPE_clsAPICalls.PatBlt(hdcDIBSection, 0, 0, Me.Width, Me.Height, IPE_clsAPICalls.WHITENESS)
        IPE_clsAPICalls.PrintWindow(hwndForm, hdcDIBSection, 0)
        IPE_clsAPICalls.SelectObject(hdcDIBSection, hbmDIBSectionOld)

        Dim imageFrm As Bitmap
        imageFrm = Image.FromHbitmap(hbmDIBSection)
        e.Graphics.DrawImage(imageFrm, 0, 0)

        IPE_clsAPICalls.DeleteDC(hdcDIBSection)
        IPE_clsAPICalls.DeleteObject(hbmDIBSection)

    End Sub

End Class