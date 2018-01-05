'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                       FORM MODULE  :  frmLoadSteps                           '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07MAR17                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Threading
Imports System.Globalization

Public Class IPE_frmLoadStep

#Region "MEMBER VARIABLES:"

    Private mIndex As Integer

#End Region


    Public Sub New(ByVal Index_In As Integer)
        '====================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mIndex = Index_In

    End Sub


    Private Sub frmLoadSteps_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '=============================================================================================
        grdLoadSteps.Columns(4).Name = ""
        grdLoadSteps.Columns(4).HeaderText = "Compression (" + gIPE_Project.Analysis(mIndex).Compression.TolType + ")"
        grdLoadSteps.Refresh()
        DisplayData()
        grdLoadSteps.ReadOnly = True
    End Sub


    Private Sub DisplayData()
        '=====================
        Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)
        pLoadStep = gIPE_Project.Analysis(mIndex).LoadStep()

        For i As Integer = 0 To pLoadStep.Count - 1

            Dim pCavityDepth_Actual As Single

            'AES 07MAR17
            Select Case gIPE_Project.Analysis(mIndex).Compression.TolType

                Case "Minimum"
                    '---------
                    pCavityDepth_Actual = pLoadStep(i).CavityDepth + gIPE_Project.Analysis(mIndex).Cavity.DepthTol(2)

                Case "Nominal"
                    '---------
                    pCavityDepth_Actual = pLoadStep(i).CavityDepth

                Case "Maximum"
                    '---------
                    pCavityDepth_Actual = pLoadStep(i).CavityDepth - gIPE_Project.Analysis(mIndex).Cavity.DepthTol(1)
            End Select

            Dim pPCentVal As Double = (pLoadStep(i).CompressionVal / gIPE_Project.Analysis(mIndex).Seal.Hfree) * 100.0#
            AddToGrid(pLoadStep(i).PDiff, pLoadStep(i).T, pCavityDepth_Actual, pLoadStep(i).CompressionVal, pPCentVal, pLoadStep(i).Descrip)
        Next

    End Sub

    Private Sub AddToGrid(ByVal PDiff_In As Double, ByVal T_In As Double, ByVal CavityDepth_In As Double,
                          ByVal CompressionVal_In As Double, ByVal CompressionPCent_In As Double, ByVal Desc_In As String)
        '===================================================================================================================
        grdLoadSteps.AllowUserToAddRows = False
        grdLoadSteps.Rows.Add()
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(0).Value = grdLoadSteps.RowCount
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(1).Value = gIPE_Unit.FormatPDiffUnitUser(PDiff_In)   'AES 18APR16
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(2).Value = T_In
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(3).Value = gIPE_Unit.WriteInUserL(CavityDepth_In) '.ToString("#0.000")
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(4).Value = gIPE_Unit.WriteInUserL(CompressionVal_In) + " (" + CompressionPCent_In.ToString("#00.0") + "%)"
        grdLoadSteps.Rows(grdLoadSteps.RowCount - 1).Cells(5).Value = Desc_In

    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '========================================================================================
        Me.Close()
    End Sub
End Class