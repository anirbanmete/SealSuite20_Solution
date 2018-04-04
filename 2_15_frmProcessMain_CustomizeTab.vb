'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmCustomaTab                  '
'                        VERSION NO  :  1.5                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  04APR18                                '
'                                                                              '
'===============================================================================

Public Class Process_frmCustomTab

#Region "MEMBER VARIABLES:"

    '....tab Variables
    Dim mHeader, mPreOrder, mExport, mOrdEntry, mCost, mApp, mDesign, mManf, mPurchase As Boolean
    Dim mQlty, mDwg, mTest, mPlanning, mShipping, mKeyChar As Boolean

#End Region
    Private Sub Process_frmCustomTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '=======================================================================================
        gIsCustomizeTabActive = True
        SetTabPrivilege()
        If (IsUserAdmin()) Then
            grpCheckBox.Enabled = True
        Else
            grpCheckBox.Enabled = False
        End If
    End Sub

    Private Sub SetTabPrivilege()
        '=======================
        Dim pRoleID As Integer = gUser.GetRoleID(gUser.Role)

        Dim pSealProcessEntities As New SealProcessDBEntities()

        Dim pUserRolePrivilege = (From pRec In pSealProcessEntities.tblRolePrivilege
                                  Where pRec.fldRoleID = pRoleID Select pRec).ToList()

        If (pUserRolePrivilege.Count > 0) Then
            mHeader = pUserRolePrivilege(0).fldHeader
            mPreOrder = pUserRolePrivilege(0).fldPreOrder
            mExport = pUserRolePrivilege(0).fldExport
            mOrdEntry = pUserRolePrivilege(0).fldOrdEntry
            mCost = pUserRolePrivilege(0).fldCost
            mApp = pUserRolePrivilege(0).fldApp
            mDesign = pUserRolePrivilege(0).fldDesign
            mManf = pUserRolePrivilege(0).fldManf
            mPurchase = pUserRolePrivilege(0).fldPurchase
            mQlty = pUserRolePrivilege(0).fldQlty
            mDwg = pUserRolePrivilege(0).fldDwg
            mTest = pUserRolePrivilege(0).fldTest
            mPlanning = pUserRolePrivilege(0).fldPlanning
            mShipping = pUserRolePrivilege(0).fldShipping
            mKeyChar = pUserRolePrivilege(0).fldKeyChar
        End If

        chkHeader.Checked = mHeader
        chkPreOrder.Checked = mPreOrder
        chkExport.Checked = mExport
        chkOrdEntry.Checked = mOrdEntry
        chkCost.Checked = mCost
        chkApp.Checked = mApp
        chkDesign.Checked = mDesign
        chkManf.Checked = mManf
        chkPurchase.Checked = mPurchase
        chkQlty.Checked = mQlty
        chkDwg.Checked = mDwg
        chkTest.Checked = mTest
        chkPlan.Checked = mPlanning
        chkShip.Checked = mShipping
        chkKeyChar.Checked = mKeyChar
        chkIssue.Checked = True
        chkApproval.Checked = True
    End Sub

    Private Function IsUserAdmin() As Boolean
        '====================================
        Dim pAdmin As Boolean = False
        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        Dim pProcessUserRole = (From pRec In pSealSuiteEntities.tblProcess_UserRole
                                Where pRec.fldUserID = gUser.ID Select pRec).ToList()
        Dim pRoleID As Integer = 0
        Dim pUserRole As String = ""
        If (pProcessUserRole.Count > 0) Then
            For i As Integer = 0 To pProcessUserRole.Count - 1
                pRoleID = pProcessUserRole(i).fldRoleID
                Dim pRole = (From pRec In pSealSuiteEntities.tblRole
                             Where pRec.fldID = pRoleID Select pRec).ToList()
                If pRole.Count > 0 Then
                    pUserRole = pRole(0).fldRole.Trim()

                End If
                If (pUserRole = "Admin") Then
                    pAdmin = True
                End If

            Next
        End If
        Return pAdmin

    End Function

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        '==========================================================================
        SaveData()
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        '===============================================================================
        Me.Close()
    End Sub

    Private Sub SaveData()
        '=================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()
        Dim pRoleID As Integer = gUser.GetRoleID(gUser.Role)

        Dim pRolePrivelegeRec = (From Rec In pSealProcessDBEntities.tblRolePrivilege Where Rec.fldRoleID = pRoleID
                                 Select Rec).ToList()

        For i As Integer = 0 To pRolePrivelegeRec.Count() - 1
            pSealProcessDBEntities.DeleteObject(pRolePrivelegeRec(i))
            pSealProcessDBEntities.SaveChanges()
        Next


        Dim pRecCount = (From Rec In pSealProcessDBEntities.tblRolePrivilege Order By Rec.fldID Descending Select Rec).ToList()
        Dim pID As Integer = 0
        If (pRecCount.Count > 0) Then
            pID = pRecCount(0).fldID + 1

            Dim pRolePrivelegeList As New tblRolePrivilege

            pRolePrivelegeList.fldID = pID
            pRolePrivelegeList.fldRoleID = pRoleID
            pRolePrivelegeList.fldHeader = chkHeader.Checked
            pRolePrivelegeList.fldPreOrder = chkPreOrder.Checked
            pRolePrivelegeList.fldExport = chkExport.Checked
            pRolePrivelegeList.fldOrdEntry = chkOrdEntry.Checked
            pRolePrivelegeList.fldCost = chkCost.Checked
            pRolePrivelegeList.fldApp = chkApp.Checked
            pRolePrivelegeList.fldDesign = chkDesign.Checked
            pRolePrivelegeList.fldManf = chkManf.Checked
            pRolePrivelegeList.fldPurchase = chkPurchase.Checked
            pRolePrivelegeList.fldQlty = chkQlty.Checked
            pRolePrivelegeList.fldDwg = chkDwg.Checked
            pRolePrivelegeList.fldTest = chkTest.Checked
            pRolePrivelegeList.fldPlanning = chkPlan.Checked
            pRolePrivelegeList.fldShipping = chkShip.Checked
            pRolePrivelegeList.fldKeyChar = chkKeyChar.Checked

            pSealProcessDBEntities.AddTotblRolePrivilege(pRolePrivelegeList)
            pSealProcessDBEntities.SaveChanges()
        End If

    End Sub

End Class