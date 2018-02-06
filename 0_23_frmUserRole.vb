'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealSuite"                            '
'                      FORM MODULE   :  frmUserRole                            '
'                        VERSION NO  :  2.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================
'
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms
Imports System.IO

Public Class frmUserRole

#Region "MEMBER VARIABLES:"

    Private mModule As String

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New(ByVal ModuleIn As String)
        '===================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mModule = ModuleIn

        If (ModuleIn = "Process") Then
            grdUsers.Columns(4).HeaderText = "Super Role 1"
            grdUsers.Columns(5).Visible = True
        ElseIf (ModuleIn = "Test") Then
            grdUsers.Columns(4).HeaderText = "Super Role"
            grdUsers.Columns(5).Visible = False

        End If
        grdUsers.Columns(6).Visible = False
    End Sub

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmUserRole_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '============================================================================================
        InitializeControls()
        RetrieveFromDB()
        grdUsers.AllowUserToAddRows = False
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        Dim pCmbColDesign_Role1 As New DataGridViewComboBoxColumn
        Dim pCmbColDesign_Role2 As New DataGridViewComboBoxColumn
        Dim pCmbColDesign_Role3 As New DataGridViewComboBoxColumn

        Dim pCmbColDesign_SuperRole1 As New DataGridViewComboBoxColumn
        Dim pCmbColDesign_SuperRole2 As New DataGridViewComboBoxColumn

        pCmbColDesign_Role1 = grdUsers.Columns.Item(1)
        pCmbColDesign_Role2 = grdUsers.Columns.Item(2)
        pCmbColDesign_Role3 = grdUsers.Columns.Item(3)

        pCmbColDesign_SuperRole1 = grdUsers.Columns.Item(4)
        pCmbColDesign_SuperRole2 = grdUsers.Columns.Item(5)

        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        '....tblRole
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole Where pRec.fldIsSuperRole = False Order By pRec.fldID Ascending Select pRec).ToList()

        If (pQryRole.Count > 0) Then
            For i As Integer = 0 To pQryRole.Count - 1
                pCmbColDesign_Role1.Items.Add(pQryRole(i).fldRole.Trim())
                pCmbColDesign_Role2.Items.Add(pQryRole(i).fldRole.Trim())
                pCmbColDesign_Role3.Items.Add(pQryRole(i).fldRole.Trim())
            Next

        End If

        Dim pQrySuperRole = (From pRec In pSealSuiteEntities.tblRole Where pRec.fldIsSuperRole = True Order By pRec.fldID Ascending Select pRec).ToList()

        If (pQrySuperRole.Count > 0) Then
            For i As Integer = 0 To pQrySuperRole.Count - 1
                pCmbColDesign_SuperRole1.Items.Add(pQrySuperRole(i).fldRole.Trim())
                pCmbColDesign_SuperRole2.Items.Add(pQrySuperRole(i).fldRole.Trim())
            Next

        End If

    End Sub

#End Region


#End Region

#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdOK_Click_1(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=========================================================================================
        SaveToDB()
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '================================================================================================
        Me.Close()
    End Sub

#End Region

#Region "DATABASE RELATED ROUTINES:"

    Private Sub RetrieveFromDB()
        '=======================
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        If (mModule = "Process") Then
            Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldProcess = True Select pRec).ToList()

            If (pQry.Count > 0) Then

                For i As Integer = 0 To pQry.Count - 1
                    Dim pUserID As Integer = pQry(i).fldID
                    grdUsers.Rows.Add()
                    Dim pName As String = ""
                    If (Not IsNothing(pQry(i).fldLastName)) Then
                        If (pQry(i).fldLastName.Trim() <> "") Then
                            pName = pQry(i).fldLastName
                        End If
                    End If

                    If (Not IsNothing(pQry(i).fldFirstName)) Then
                        If (pQry(i).fldFirstName.Trim() <> "") Then
                            If (pName <> "") Then
                                pName = pName + ", " + pQry(i).fldFirstName
                            Else
                                pName = pQry(i).fldFirstName
                            End If
                        End If
                    End If

                    grdUsers.Rows(i).Cells(0).Value = pName.Trim()
                    grdUsers.Rows(i).Cells(6).Value = pUserID

                    Dim pQryRole = (From pRec In pSealSuiteEntities.tblProcess_UserRole Where pRec.fldUserID = pUserID Select pRec).ToList()
                    If (pQryRole.Count > 0) Then
                        Dim pRoleCount As Integer = 0
                        Dim pSuperRoleCount As Integer = 0
                        For j As Integer = 0 To pQryRole.Count - 1
                            If (Not IsSuperRole(pQryRole(j).fldRoleID)) Then
                                pRoleCount = pRoleCount + 1
                                If (pRoleCount = 1) Then
                                    grdUsers.Rows(i).Cells(1).Value = GetRole(pQryRole(j).fldRoleID)
                                    grdUsers.Rows(i).Cells(1).ToolTipText = GetRole(pQryRole(j).fldRoleID)
                                ElseIf (pRoleCount = 2) Then
                                    grdUsers.Rows(i).Cells(2).Value = GetRole(pQryRole(j).fldRoleID)
                                    grdUsers.Rows(i).Cells(2).ToolTipText = GetRole(pQryRole(j).fldRoleID)
                                ElseIf (pRoleCount = 3) Then
                                    grdUsers.Rows(i).Cells(3).Value = GetRole(pQryRole(j).fldRoleID)
                                    grdUsers.Rows(i).Cells(3).ToolTipText = GetRole(pQryRole(j).fldRoleID)
                                End If
                            Else
                                pSuperRoleCount = pSuperRoleCount + 1
                                If (pSuperRoleCount = 1) Then
                                    grdUsers.Rows(i).Cells(4).Value = GetRole(pQryRole(j).fldRoleID)
                                    grdUsers.Rows(i).Cells(4).ToolTipText = GetRole(pQryRole(j).fldRoleID)
                                ElseIf (pSuperRoleCount = 2) Then
                                    grdUsers.Rows(i).Cells(5).Value = GetRole(pQryRole(j).fldRoleID)
                                    grdUsers.Rows(i).Cells(5).ToolTipText = GetRole(pQryRole(j).fldRoleID)
                                End If
                            End If

                        Next

                    End If

                Next
                grdUsers.Columns(0).ReadOnly = True
            End If

        ElseIf (mModule = "Test") Then

        End If

    End Sub

    Private Sub SaveToDB()
        '=================
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pQry = (From pRec In pSealSuiteEntities.tblProcess_UserRole Select pRec).ToList()
        For i As Integer = 0 To pQry.Count - 1
            pSealSuiteEntities.DeleteObject(pQry(i))
            pSealSuiteEntities.SaveChanges()
        Next

        Dim pID As Integer = 1
        For j As Integer = 0 To grdUsers.Rows.Count - 1

            Dim pUserID As Integer = grdUsers.Rows(j).Cells(6).Value


            Dim pRole_List As New List(Of String)
            If (grdUsers.Rows(j).Cells(1).Value <> "" And Not IsNothing(grdUsers.Rows(j).Cells(1).Value)) Then
                pRole_List.Add(grdUsers.Rows(j).Cells(1).Value)
            End If

            If (grdUsers.Rows(j).Cells(2).Value <> "" And Not IsNothing(grdUsers.Rows(j).Cells(2).Value)) Then
                pRole_List.Add(grdUsers.Rows(j).Cells(2).Value)
            End If

            If (grdUsers.Rows(j).Cells(3).Value <> "" And Not IsNothing(grdUsers.Rows(j).Cells(3).Value)) Then
                pRole_List.Add(grdUsers.Rows(j).Cells(3).Value)
            End If

            If (grdUsers.Rows(j).Cells(4).Value <> "" And Not IsNothing(grdUsers.Rows(j).Cells(4).Value)) Then
                pRole_List.Add(grdUsers.Rows(j).Cells(4).Value)
            End If

            If (grdUsers.Rows(j).Cells(5).Value <> "" And Not IsNothing(grdUsers.Rows(j).Cells(5).Value)) Then
                pRole_List.Add(grdUsers.Rows(j).Cells(5).Value)
            End If

            For k As Integer = 0 To pRole_List.Count - 1
                Dim pRoleID As Integer = GetRoleID(pRole_List(k))
                Dim pProcessUserRole As New tblProcess_UserRole
                With pProcessUserRole
                    .fldID = pID
                    .fldUserID = pUserID
                    .fldRoleID = pRoleID
                    pSealSuiteEntities.AddTotblProcess_UserRole(pProcessUserRole)
                    pSealSuiteEntities.SaveChanges()
                    pID = pID + 1
                End With
            Next
        Next

    End Sub

#Region "HELPER ROUTINES:"

    Private Function GetRole(ByVal RoleID As Integer) As String
        '======================================================
        Dim pRole As String = ""
        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole Where pRec.fldID = RoleID Select pRec).ToList()

        If (pQryRole.Count > 0) Then
            If Not IsNothing(pQryRole(0).fldRole) Then
                pRole = pQryRole(0).fldRole

            End If
        End If

        Return pRole

    End Function

    Private Function GetRoleID(ByVal Role As String) As Integer
        '======================================================
        Dim pRoleID As Integer = 0
        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole Where pRec.fldRole = Role Select pRec).ToList()

        If (pQryRole.Count > 0) Then
            If Not IsNothing(pQryRole(0).fldID) Then
                pRoleID = pQryRole(0).fldID

            End If
        End If

        Return pRoleID

    End Function


    Private Function IsSuperRole(ByVal RoleID As Integer) As Boolean
        '===========================================================
        Dim pIsSuperRole As Boolean = False
        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole Where pRec.fldID = RoleID Select pRec).ToList()

        If (pQryRole.Count > 0) Then
            If Not IsNothing(pQryRole(0).fldIsSuperRole) Then
                pIsSuperRole = pQryRole(0).fldIsSuperRole

            End If
        End If

        Return pIsSuperRole
    End Function

#End Region

#End Region

End Class