'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmIssueComnt                  '
'                        VERSION NO  :  1.3                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  25JAN18                                '
'                                                                              '
'===============================================================================

Public Class Process_frmIssueComnt

#Region "MEMBER VARIABLES:"

    Private mProcessProj As New clsProcessProj(gPartProject)

#End Region

#Region "CONSTRUCTOR:"
    Public Sub New(ByRef ProcessProj_In As clsProcessProj)
        '==================================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        mProcessProj = ProcessProj_In
    End Sub


#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub Process_frmIssueComnt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '========================================================================================
        gIsIssueCommentActive = True

        'PopulateDeptName()
        'cmbDeptName.Text = gUser.Title

        If (gUser.Role = "" Or IsSuperRole(gUser.Role)) Then

            lblDeptName.Visible = False
            cmbDeptName.Visible = True
            PopulateDeptName()

        ElseIf (gUser.Role <> "" And Not IsSuperRole(gUser.Role)) Then
            cmbDeptName.Visible = False
            lblDeptName.Visible = True
            lblDeptName.Text = gUser.Role
        End If

        lblUserName.Text = gUser.FirstName + " " + gUser.LastName
    End Sub




#Region "HELPER ROUTINES:"

    Private Function IsSuperRole(ByVal Role_In As String) As Boolean
        '============================================================
        Dim pSuperRole As Boolean = False
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pRecCount As Integer = (From pRec In pSealSuiteEntities.tblRole
                                    Where pRec.fldRole = Role_In And pRec.fldIsSuperRole = 1 Select pRec).Count()

        If (pRecCount > 0) Then
            pSuperRole = True
        End If

        Return pSuperRole

    End Function
    Private Sub PopulateDeptName()
        '============================
        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        cmbDeptName.Items.Clear()

        Try
            Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole Select pRec).ToList()

            If (pQryRole.Count > 0) Then
                For i As Integer = 0 To pQryRole.Count - 1
                    cmbDeptName.Items.Add(pQryRole(i).fldRole)
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub


#End Region

#End Region

#Region "COMBO BOX RELATED ROUTINES:"
    Private Sub cmbDeptName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDeptName.SelectedIndexChanged
        '====================================================================================================================
        'cmbDeptName.Text = gUser.Title
    End Sub


#End Region

#Region "COMMAND BUTTON RELATED ROUTINES:"
    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        '=========================================================================
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
        Dim pID As Integer = 0
        If (mProcessProj.IssueCommnt.ID.Count > 0) Then
            pID = mProcessProj.IssueCommnt.ID(mProcessProj.IssueCommnt.ID.Count - 1)
        End If
        mProcessProj.IssueCommnt.ID.Add(pID + 1)
        mProcessProj.IssueCommnt.Comment.Add(txtComments.Text)
        If (gUser.Role <> "" And Not IsSuperRole(gUser.Role)) Then
            mProcessProj.IssueCommnt.ByDept.Add(lblDeptName.Text)
        ElseIf (gUser.Role = "" Or IsSuperRole(gUser.Role)) Then
            mProcessProj.IssueCommnt.ByDept.Add(cmbDeptName.Text)
        End If

        mProcessProj.IssueCommnt.ByName.Add(lblUserName.Text)
        mProcessProj.IssueCommnt.ByDate.Add(DateTime.Now())

        mProcessProj.IssueCommnt.ToDept.Add("")
        mProcessProj.IssueCommnt.Resolved.Add(False)
        mProcessProj.IssueCommnt.Name.Add("")
        mProcessProj.IssueCommnt.DateResolution.Add(DateTime.MinValue)
        mProcessProj.IssueCommnt.Resolution.Add("")
    End Sub

#End Region


#End Region

End Class