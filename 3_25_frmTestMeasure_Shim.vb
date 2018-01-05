'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Measure_Shim                   '
'                        VERSION NO  :  2.6                                 '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  16FEB17                                '
'                                                                              '
'===============================================================================

Public Class Test_frmMeasure_Shim
    Private mTest_frmLeak As Test_frmLeak

    Public Sub New(ByRef frmLeak_Out As Test_frmLeak)
        '=============================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If (Not IsNothing(frmLeak_Out)) Then
            mTest_frmLeak = frmLeak_Out
        End If

    End Sub

    Private Sub frmTest_Measure_Shim_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '====================================================================================================

        grdData.AllowUserToAddRows = False
        grdData.Rows.Add(3)

        For i As Integer = 0 To grdData.Rows.Count - 1
            grdData.Rows(i).Cells(0).Value = i + 1
            grdData.Rows(i).Cells(0).ReadOnly = True
        Next

    End Sub


    Private Sub cmdAvg_Click(sender As System.Object, e As System.EventArgs) Handles cmdAvg.Click
        '=========================================================================================
        CalcAvg()

    End Sub

    Private Sub CalcAvg()
        '================
        Dim pVal As Double = 0.0
        Dim pCount As Integer = 0
        For i As Integer = 0 To grdData.Rows.Count - 1
            If (Not IsNothing(grdData.Rows(i).Cells(1).Value)) Then
                pVal = pVal + grdData.Rows(i).Cells(1).Value
                pCount = pCount + 1
            End If
        Next

        If (pVal > 0) Then
            txtAvgVal.Text = (pVal / pCount).ToString("#0.000")
        Else
            txtAvgVal.Text = "0.000"
        End If

    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        CalcAvg()

        If (Not IsNothing(mTest_frmLeak)) Then
            If (Convert.ToDouble(txtAvgVal.Text) > 0) Then
                mTest_frmLeak.txtActual.Text = txtAvgVal.Text
            End If
        End If
        Me.Close()

    End Sub


End Class