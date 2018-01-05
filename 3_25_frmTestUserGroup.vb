'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_UserGroup                      '
'                        VERSION NO  :  2.6                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JUL17                                '
'                                                                              '
'===============================================================================
'
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms
Imports System.IO

Public Class Test_frmUserGroup

#Region "EVENT DECLARATION:"

    Event grdUsersButtonClick(sender As DataGridView, e As DataGridViewCellEventArgs)
#End Region

#Region "MEMBER VARIABLES:"

    Private mSealTestEntities As New SealTestDBEntities()
    Private mblnAdd, mblnEdit As Boolean

    '....Currently Selected Rows
    Private mISel As Integer = 0

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_UserInfo_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==================================================================================================
        PopulateDataGrid()

        'AES 20JUL17
        If (Not IsNothing(grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).Value)) Then
            If (gTest_User.Role = Test_clsUser.eRole.SuperAdmin) Then
                cmdDelete.Enabled = True

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                Dim pSystemLogin As String = grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).Value

                If (pSystemLogin.ToUpper() = gTest_User.SystemLogin.ToUpper()) Then
                    cmdDelete.Enabled = False
                Else
                    cmdDelete.Enabled = True
                End If
            End If

        End If

        cmdSave.Enabled = False

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub PopulateDataGrid()
        '==========================
        Try

            grdUsers.Rows.Clear()

            Dim pQry = (From pRec In mSealTestEntities.tblTestUser Select pRec).ToList()

            For i As Integer = 0 To pQry.Count - 1
                grdUsers.AllowUserToAddRows = False
                grdUsers.Rows.Add()
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(0).Value = pQry(i).fldName
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(1).Value = pQry(i).fldSystemLogin
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(2).Value = pQry(i).fldRoleTester
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(3).Value = pQry(i).fldRoleEngg
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(4).Value = pQry(i).fldRoleQuality
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(5).Value = pQry(i).fldRoleAdmin
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(6).Value = "Browse"

                If Not IsNothing(pQry(i).fldSignature) Then
                    Dim pArray As Byte() = DirectCast(pQry(i).fldSignature, Byte())
                    Dim pMS As New MemoryStream(pArray)
                    grdUsers.Rows(grdUsers.RowCount - 1).Cells(7).Value = Image.FromStream(pMS)
                End If
                grdUsers.Rows(i).ReadOnly = True
            Next
            grdUsers.Rows(0).ReadOnly = True

        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region

#Region "COMMAND RELATED ROUTINES:"

    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        '========================================================================================
        mblnEdit = True
        grdUsers.AllowUserToAddRows = False
        grdUsers.Rows.Add()
        Dim pCurRow As Integer = grdUsers.Rows.Count - 1

        grdUsers.Rows(pCurRow).ReadOnly = False
        grdUsers.Rows(pCurRow).Selected = True
        cmdSave.Enabled = True

    End Sub

    Private Sub cmdEdit_Click(sender As System.Object, e As System.EventArgs) Handles cmdEdit.Click
        '==========================================================================================
        mblnEdit = True

        For i As Integer = 0 To grdUsers.Rows.Count - 1
            grdUsers.Rows(i).ReadOnly = False
        Next

        'AES 20JUL17
        If (Not IsNothing(grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).Value)) Then
            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                Dim pSystemLogin As String = grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).Value

                If (pSystemLogin.ToUpper() = gTest_User.SystemLogin.ToUpper()) Then
                    '....System Login should not be edited.
                    grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).ReadOnly = True
                Else
                    grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).ReadOnly = False
                End If

            End If
        End If

        cmdSave.Enabled = True

    End Sub

    Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
        '==========================================================================================
        Try

            If (mblnAdd) Then
                Dim pCurRow As Integer = grdUsers.Rows.Count - 1
                Dim pUser As New tblTestUser

                pUser.fldName = grdUsers.Rows(pCurRow).Cells(0).Value
                pUser.fldSystemLogin = grdUsers.Rows(pCurRow).Cells(1).Value
                pUser.fldRoleTester = grdUsers.Rows(pCurRow).Cells(2).Value
                pUser.fldRoleEngg = grdUsers.Rows(pCurRow).Cells(3).Value
                pUser.fldRoleQuality = grdUsers.Rows(pCurRow).Cells(4).Value
                pUser.fldRoleAdmin = grdUsers.Rows(pCurRow).Cells(5).Value
                'Dim pMS As New MemoryStream()
                Dim pImg As Image = grdUsers.Rows(pCurRow).Cells(7).Value
                Dim pArray As Byte() = imgToByteArray(pImg)
                pUser.fldSignature = pArray

                mSealTestEntities.AddTotblTestUser(pUser)
                mSealTestEntities.SaveChanges()

                mblnAdd = False
                cmdSave.Enabled = False
                grdUsers.Rows(pCurRow).ReadOnly = True

            ElseIf (mblnEdit) Then
                Dim pQry = (From pRec In mSealTestEntities.tblTestUser Select pRec).ToList()
                For i As Integer = 0 To pQry.Count - 1
                    mSealTestEntities.DeleteObject(pQry(i))
                    mSealTestEntities.SaveChanges()
                Next

                For i As Integer = 0 To grdUsers.Rows.Count - 1
                    Dim pUser As New tblTestUser

                    pUser.fldName = grdUsers.Rows(i).Cells(0).Value
                    pUser.fldSystemLogin = grdUsers.Rows(i).Cells(1).Value
                    pUser.fldRoleTester = grdUsers.Rows(i).Cells(2).Value
                    pUser.fldRoleEngg = grdUsers.Rows(i).Cells(3).Value
                    pUser.fldRoleQuality = grdUsers.Rows(i).Cells(4).Value
                    pUser.fldRoleAdmin = grdUsers.Rows(i).Cells(5).Value

                    'Dim pMS As New MemoryStream()
                    Dim pImg As Image = grdUsers.Rows(i).Cells(7).Value
                    If Not IsNothing(pImg) Then
                        Dim pArray As Byte() = imgToByteArray(pImg)
                        pUser.fldSignature = pArray
                    End If

                    mSealTestEntities.AddTotblTestUser(pUser)
                    mSealTestEntities.SaveChanges()
                    grdUsers.Rows(i).ReadOnly = True
                Next

                mblnEdit = False
                cmdSave.Enabled = False

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Function imgToByteArray(ByVal img As Image) As Byte()
        '=========================================================
        Using mStream As New MemoryStream()
            img.Save(mStream, img.RawFormat)
            Return mStream.ToArray()
        End Using
    End Function

    Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete.Click
        '===============================================================================================

        Dim pName As String = grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(0).Value
        Dim pQry = (From pRec In mSealTestEntities.tblTestUser Where pRec.fldName = pName Select pRec).First()

        mSealTestEntities.DeleteObject(pQry)
        mSealTestEntities.SaveChanges()

        PopulateDataGrid()

    End Sub


    Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
        '=============================================================================================
        Me.Close()
    End Sub

#End Region


#Region "GRIDVIEW EVENT RELATED ROUTINES:"

    Private Sub grdUsers_CellContentClick(sender As System.Object,
                                          e As DataGridViewCellEventArgs) Handles grdUsers.CellContentClick
        '===================================================================================================
        Dim senderGrid = DirectCast(sender, DataGridView)
        If TypeOf senderGrid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn AndAlso e.RowIndex >= 0 Then
            RaiseEvent grdUsersButtonClick(senderGrid, e)
        End If

    End Sub

    Private Sub grdUsers_ButtonClick(sender As DataGridView,
                                       e As DataGridViewCellEventArgs) Handles Me.grdUsersButtonClick
        '============================================================================================

        If mblnAdd Or mblnEdit Then
            Dim pCurIndex As Integer = grdUsers.CurrentRow.Index

            Dim pImage As Image = Nothing
            openFileDialog1.Filter = "jpeg|*.jpg|bmp|*.bmp|png|*.png|all files|*.*"
            Dim res As DialogResult = openFileDialog1.ShowDialog()
            If res = DialogResult.OK Then
                pImage = Image.FromFile(openFileDialog1.FileName)
            End If

            grdUsers.Rows(pCurIndex).Cells(7).Value = pImage
        End If

    End Sub

    Private Sub grdUsers_CellClick(sender As System.Object,
                                   e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdUsers.CellClick
        '=============================================================================================================
        'AES 20JUL17
        If (Not IsNothing(grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).Value)) Then
            Dim pSystemLogin As String = grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(1).Value

            If (gTest_User.Role = Test_clsUser.eRole.SuperAdmin) Then
                cmdDelete.Enabled = True

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                If (pSystemLogin.ToUpper() = gTest_User.SystemLogin.ToUpper()) Then
                    cmdDelete.Enabled = False
                Else
                    cmdDelete.Enabled = True
                End If

            End If
        End If
    End Sub

#End Region


End Class