
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmIssueComnt_Resolution       '
'                        VERSION NO  :  1.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  25JAN18                                '
'                                                                              '
'===============================================================================
Public Class Process_frmIssueComnt_Resolution

#Region "MEMBER VARIABLES:"

    Private mProcessProj As New clsProcessProj(gPartProject)
    Private mIndex As Integer

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New(ByRef ProcessProj_In As clsProcessProj, ByVal RowIndex_In As Integer)
        '===============================================================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mProcessProj = ProcessProj_In
        mIndex = RowIndex_In
    End Sub

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub Process_frmIssueComnt_Resolution_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '===================================================================================================
        gIsResolutionActive = True
        txtResolution.Text = mProcessProj.IssueCommnt.Resolution(mIndex)
    End Sub

#End Region

#Region "COMMAND BUTTON RELATED ROUTINES:"
    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        '===========================================================================
        SaveData()
        Me.Close()
    End Sub


    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        '=================================================================================
        Me.Close()
    End Sub

#Region "HELPER ROUTINE"

    Private Sub SaveData()
        '=================
        mProcessProj.IssueCommnt.Resolution(mIndex) = txtResolution.Text.Trim()
    End Sub

#End Region

#End Region
End Class