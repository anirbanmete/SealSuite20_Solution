'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmReportCust                          '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28APR16                                '
'                                                                              '
'===============================================================================

Imports System.Windows.Forms
Imports Microsoft.Office.Interop.PowerPoint
Imports Microsoft.Office.Core
Imports System.Reflection
Imports System.IO
Imports SealIPELib = SealIPELib101

Public Class IPE_frmReportCust

#Region "FORM EVENT ROUTINES:"

    Private Sub frmReportCust_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==============================================================================================
        gIPE_Report = New IPE_clsReport()

        grdLoadSteps.Columns(5).Name = ""
        grdLoadSteps.Columns(5).HeaderText = "Compression (" + gIPE_Project.Analysis(gIPE_frmResults.ISel).Compression.TolType + ")"
        Dim pDataGridViewColumn As DataGridViewColumn = grdLoadSteps.Columns(6)
        pDataGridViewColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        grdLoadSteps.Refresh()
        DisplayData()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub DisplayData()
        '====================
        'gIPE_Project.Analysis(gSel).CompressionTolType = gIPE_Project.Analysis(gSel).Compression.TolType

        txtLoadCase.Text = gIPE_Project.Analysis(gIPE_frmResults.ISel).LoadCase.Name

        Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)
        pLoadStep = gIPE_Project.Analysis(gIPE_frmResults.ISel).LoadStep()

        'For i As Integer = 0 To pLoadStep.Count - 1
        '    Dim pPCentVal As Double = (pLoadStep(i).CompressionVal / gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Hfree) * 100.0#
        '    AddToGrid(pLoadStep(i).PDiff, pLoadStep(i).T, pLoadStep(i).CavityDepth, pLoadStep(i).CompressionVal, pPCentVal, pLoadStep(i).Descrip)
        'Next

        For i As Integer = 0 To pLoadStep.Count - 1

            Dim pCavityDepth_Actual As Single

            Select Case gIPE_Project.Analysis(gIPE_frmResults.ISel).Compression.TolType

                Case "Minimum"
                    '---------
                    pCavityDepth_Actual = pLoadStep(i).CavityDepth + gIPE_Project.Analysis(gIPE_frmResults.ISel).Cavity.DepthTol(2)

                Case "Nominal"
                    '---------
                    pCavityDepth_Actual = pLoadStep(i).CavityDepth

                Case "Maximum"
                    '---------
                    pCavityDepth_Actual = pLoadStep(i).CavityDepth - gIPE_Project.Analysis(gIPE_frmResults.ISel).Cavity.DepthTol(1)
            End Select

            Dim pPCentVal As Double = (pLoadStep(i).CompressionVal / gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Hfree) * 100.0#
            AddToGrid(pLoadStep(i).PDiff, pLoadStep(i).T, pCavityDepth_Actual, pLoadStep(i).CompressionVal, pPCentVal, pLoadStep(i).Descrip)
        Next

        Dim pCount As Integer = 0
        For j As Integer = 0 To grdLoadSteps.RowCount - 1

            If (grdLoadSteps.Rows(j).Cells(6).Value = "Assembly") Then
                pCount = pCount + 1

                If (pCount = 2) Then
                    grdLoadSteps.Rows(j).ReadOnly = True
                    grdLoadSteps.Rows(j).Cells(0).Value = False
                    grdLoadSteps.Rows(j).Cells(0).Style.BackColor = Color.LightGray
                End If
            End If
        Next

    End Sub


#End Region


#Region "SUB-HELPER ROUTINES:"

    Private Sub AddToGrid(ByVal PDiff_In As Double, ByVal T_In As Double, ByVal CavityDepth_In As Double,
                          ByVal CompressionVal_In As Double, ByVal CompressionPCent_In As Double, ByVal Desc_In As String)
        '===================================================================================================================
        grdLoadSteps.AllowUserToAddRows = False
        grdLoadSteps.Rows.Add()
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(0).Value = True
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(1).Value = grdLoadSteps.RowCount
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(2).Value = gIPE_Unit.FormatPDiffUnitUser(PDiff_In)   'AES 18APR16 PDiff_In
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(3).Value = T_In
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(4).Value = gIPE_Unit.WriteInUserL(CavityDepth_In) '.ToString("#0.000")
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(5).Value = gIPE_Unit.WriteInUserL(CompressionVal_In) + " (" + CompressionPCent_In.ToString("#00.0") + "%)"
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(6).Value = Desc_In

    End Sub

#End Region


#End Region


#Region "COMMAND BUTTON EBENT ROUTINES:"

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '========================================================================================
        Me.Close()
        Dim pPicForm_Seal As New IPE_frmPicBox_Seal()
        pPicForm_Seal.ShowDialog()
        pPicForm_Seal.Close()

        Dim pPicForm_Cavity As New IPE_frmPicBox_Cavity()
        pPicForm_Cavity.ShowDialog()
        pPicForm_Cavity.Close()

        Dim pLoadStep As New List(Of Boolean)

        For i As Integer = 0 To grdLoadSteps.RowCount - 1

            If (grdLoadSteps.Rows(i).Cells(0).Value = True) Then
                pLoadStep.Add(True)
            Else
                pLoadStep.Add(False)
            End If

        Next
        Dim pICur As Integer = gIPE_frmResults.ISel
        Dim pTargetFolderTitle As String = gIPE_Project.Customer_ID & "-" & gIPE_Project.Platform_ID & "-" & gIPE_Project.Project_ID & "-" & gIPE_Project.Analysis(pICur).ID
        Dim pFolderName As String = modMain_IPE.gIPE_File.DirOut & pTargetFolderTitle

        'AES 01APR16
        If (Not Directory.Exists(modMain_IPE.gIPE_File.DirOut)) Then
            Directory.CreateDirectory(modMain_IPE.gIPE_File.DirOut)
        End If

        If (Directory.Exists(pFolderName)) Then

            Dim extensions As New List(Of String)
            extensions.Add("*.png")

            Dim pFileCount As Integer = 0
            For i As Integer = 0 To extensions.Count - 1
                pFileCount = pFileCount + Directory.GetFiles(pFolderName, extensions(i)).Length
            Next

            If (pFileCount < (pLoadStep.Count * 2) + 2) Then
                MessageBox.Show("Couldn't find ANSYS plot files. Please re-run ANSYS.", "ANSYS File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                gIPE_Report.CratePowerPoint_Report(gIPE_frmResults.ISel, pLoadStep, chkThermalGrowth.Checked, gIPE_Project)
            End If

        Else
            MessageBox.Show("Couldn't find ANSYS plot files. Please re-run ANSYS.", "ANSYS File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If


    End Sub


#End Region


End Class