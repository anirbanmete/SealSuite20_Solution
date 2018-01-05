'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Measure_FH                     '
'                        VERSION NO  :  2.6                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  16FEB17                                '
'                                                                              '
'===============================================================================

Public Class Test_frmMeasure_FH

    Private mTest_frmLeak As Test_frmLeak
    Private mTest_frmLoad As Test_frmLoad
    Private mblnIni As Boolean = False
    Private mblnFinal As Boolean = False


    Private Sub frmTest_Measure_FH_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===================================================================================================
        Try
            grdData.AllowUserToAddRows = False
            grdData.Rows.Add(3)

            For i As Integer = 0 To grdData.Rows.Count - 1
                grdData.Rows(i).Cells(0).Value = i + 1
                grdData.Rows(i).Cells(0).ReadOnly = True
            Next
        Catch ex As Exception

        End Try

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
            If (mblnIni) Then
                If (Convert.ToDouble(txtAvgVal.Text) > 0) Then
                    mTest_frmLeak.txtFHIni_Measured.Text = txtAvgVal.Text
                End If

            ElseIf (mblnFinal) Then
                If (Convert.ToDouble(txtAvgVal.Text) > 0) Then
                    mTest_frmLeak.txtFHFinal_Measured.Text = txtAvgVal.Text
                End If
            End If

        ElseIf (Not IsNothing(mTest_frmLoad)) Then
            If (mblnIni) Then
                If (Convert.ToDouble(txtAvgVal.Text) > 0) Then
                    mTest_frmLoad.txtFHIni_Measured.Text = txtAvgVal.Text
                End If

            ElseIf (mblnFinal) Then
                If (Convert.ToDouble(txtAvgVal.Text) > 0) Then
                    mTest_frmLoad.txtFHFinal_Measured.Text = txtAvgVal.Text
                End If
            End If

        End If

        Me.Close()

    End Sub


    Public Sub New(ByVal FH_Stage_In As String, Optional ByRef Test_frmLeak_Out As Test_frmLeak = Nothing,
                   Optional ByRef Test_frmLoad_Out As Test_frmLoad = Nothing)
        '============================================================================================
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If (Not IsNothing(Test_frmLeak_Out)) Then
            mTest_frmLeak = Test_frmLeak_Out
        ElseIf (Not IsNothing(Test_frmLoad_Out)) Then
            mTest_frmLoad = Test_frmLoad_Out
        End If

        If (FH_Stage_In = "Ini") Then
            mblnIni = True

        ElseIf (FH_Stage_In = "Final") Then
            mblnFinal = True

        End If

    End Sub

End Class