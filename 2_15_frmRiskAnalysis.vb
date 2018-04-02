
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmRiskAnalysis                '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  20FEB18                                '
'                                                                              '
'===============================================================================
Public Class Process_frmRiskAnalysis


#Region "MEMBER VARIABLES:"

    Private mProcessProj As New clsProcessProj(gPartProject)
    Private mTabName As String = ""

#End Region

#Region "CONSTRUCTOR:"
    Public Sub New(ByRef ProcessProj_In As clsProcessProj, ByVal TabName_In As String)
        '============================================================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mProcessProj = ProcessProj_In
        mTabName = TabName_In

        If (mTabName = "PreOrder") Then
            Me.Text = "SealProcess: Risk Analysis - Pre-Order"
        ElseIf (mTabName = "OrdEntry") Then
            Me.Text = "SealProcess: Risk Analysis - Order Entry"
        ElseIf (mTabName = "Cost") Then
            Me.Text = "SealProcess: Risk Analysis - Cost Estimating"
        ElseIf (mTabName = "App") Then
            Me.Text = "SealProcess: Risk Analysis - Application"
        ElseIf (mTabName = "Manf") Then
            Me.Text = "SealProcess: Risk Analysis - Manufacturing"
        ElseIf (mTabName = "Purchase") Then
            Me.Text = "SealProcess: Risk Analysis - Purchasing"
        ElseIf (mTabName = "Qlty") Then
            Me.Text = "SealProcess: Risk Analysis - Quality"
        ElseIf (mTabName = "Dwg") Then
            Me.Text = "SealProcess: Risk Analysis - Drawing"
        ElseIf (mTabName = "Test") Then
            Me.Text = "SealProcess: Risk Analysis - Testing"
        Else
            Me.Text = "SealProcess: Risk Analysis - " & mTabName
        End If
    End Sub

    Private Sub Process_frmRiskAnalysis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '=========================================================================================
        mProcessProj.RiskAna.RetrieveFromDB(mProcessProj.ID, mTabName)
        DisplayData()
    End Sub

    Private Sub DisplayData()
        '=====================
        grdRiskAna.Rows.Clear()
        grdRiskAna.AllowUserToAddRows = False

        For i As Integer = 0 To mProcessProj.RiskAna.RiskAnlayisQ.Count - 1
            grdRiskAna.Rows.Add()
            grdRiskAna.Rows(i).Cells(0).Value = i + 1
            grdRiskAna.Rows(i).Cells(1).Value = mProcessProj.RiskAna.RiskAnlayisQ.ElementAt(i).Key

            If (mProcessProj.RiskAna.Answered(i)) Then
                grdRiskAna.Rows(i).Cells(2).Value = "Y"
            Else
                grdRiskAna.Rows(i).Cells(2).Value = "N"
            End If

            grdRiskAna.Rows(i).Cells(3).Value = mProcessProj.RiskAna.Reason(i)
        Next
        grdRiskAna.Columns(0).ReadOnly = True

    End Sub

    Private Sub SaveData()
        '=================
        For i As Integer = 0 To grdRiskAna.Rows.Count - 1
            If (grdRiskAna.Rows(i).Cells(2).Value = "Y") Then
                mProcessProj.RiskAna.Answered(i) = True
            Else
                mProcessProj.RiskAna.Answered(i) = False
            End If
            'mProcessProj.RiskAna.Reason(i) = grdRiskAna.Rows(i).Cells(2).Value
            mProcessProj.RiskAna.Reason(i) = grdRiskAna.Rows(i).Cells(3).Value
        Next

    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        '========================================================================
        SaveData()
        mProcessProj.RiskAna.SaveToDB(mProcessProj.ID, mTabName)
        Me.Close()

    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        '================================================================================
        Me.Close()
    End Sub




#End Region

End Class