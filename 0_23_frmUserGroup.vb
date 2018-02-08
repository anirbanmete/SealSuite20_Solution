'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealSuite"                            '
'                      FORM MODULE   :  frmUserGroup                           '
'                        VERSION NO  :  2.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  06FEBC18                                '
'                                                                              '
'===============================================================================
'
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms
Imports System.IO

Public Class frmUserGroup

#Region "EVENT DECLARATION:"

    Event grdUsersButtonClick(sender As DataGridView, e As DataGridViewCellEventArgs)
#End Region

#Region "MEMBER VARIABLES:"

    Private mblnAdd, mblnEdit As Boolean
    Private mUserID As Integer = 0

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmUserGroup_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '============================================================================================
        InitializeControls()
        RetrieveFromDB()
        txtFileName.Text = gUser.ProgramDataFile
        PopulateDataGrid()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        Dim pCmbColDesign_Title As New DataGridViewComboBoxColumn
        pCmbColDesign_Title = grdUsers.Columns.Item(3)

        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        '....tblTitle
        Dim pQry = (From pRec In pSealSuiteEntities.tblTitle Order By pRec.fldID Ascending Select pRec).ToList()

        If (pQry.Count > 0) Then
            pCmbColDesign_Title.Items.Clear()
            For i As Integer = 0 To pQry.Count - 1
                pCmbColDesign_Title.Items.Add(pQry(i).fldTitle.Trim())
            Next

        End If

        cmdSave.Enabled = False
        cmdAdd.Enabled = True
        cmdEdit.Enabled = True
        cmdDelete.Enabled = True

    End Sub


    Private Sub PopulateDataGrid()
        '==========================
        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        Try

            grdUsers.Rows.Clear()

            Dim pQry = (From pRec In pSealSuiteEntities.tblUser Select pRec).ToList()

            For i As Integer = 0 To pQry.Count - 1
                grdUsers.AllowUserToAddRows = False
                grdUsers.Rows.Add()
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(0).Value = pQry(i).fldLastName
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(1).Value = pQry(i).fldFirstName
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(2).Value = pQry(i).fldSystemLogin

                Dim pTitleID As Integer = pQry(i).fldTitleID
                Dim pQryTitle = (From pRec In pSealSuiteEntities.tblTitle Where pRec.fldID = pTitleID Select pRec).ToList()
                If (pQryTitle.Count > 0) Then
                    grdUsers.Rows(grdUsers.RowCount - 1).Cells(3).Value = pQryTitle(0).fldTitle
                End If

                grdUsers.Rows(grdUsers.RowCount - 1).Cells(4).Value = "Browse"
                If Not IsNothing(pQry(i).fldSignature) Then
                    Dim pArray As Byte() = DirectCast(pQry(i).fldSignature, Byte())
                    Dim pMS As New MemoryStream(pArray)
                    grdUsers.Rows(grdUsers.RowCount - 1).Cells(5).Value = Image.FromStream(pMS)
                End If

                grdUsers.Rows(grdUsers.RowCount - 1).Cells(6).Value = pQry(i).fldProcess
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(7).Value = pQry(i).fldTest
                grdUsers.Rows(grdUsers.RowCount - 1).Cells(8).Value = pQry(i).fldIPE

                grdUsers.Rows(i).ReadOnly = True
            Next

            grdUsers.Rows(0).ReadOnly = True

        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region

#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
        '==============================================================================================
        With openFileDialog1

            .Filter = "SealSuite DataFile Use (*.xls)|*.xls"
            .FilterIndex = 1
            .InitialDirectory = gFile.DirUserData
            .FileName = ""
            .Title = "Open"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim pUserDataFileName As String = .FileName
                Cursor.Current = Cursors.WaitCursor
                gUser.UpdateTo_DB(pUserDataFileName)
                txtFileName.Text = pUserDataFileName
                gUser.ProgramDataFile = pUserDataFileName
                SaveToDB()
                InitializeControls()
                Cursor.Current = Cursors.Default
            End If

        End With

    End Sub

    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        '========================================================================================
        mblnAdd = True
        grdUsers.AllowUserToAddRows = False
        grdUsers.Rows.Add()
        Dim pCurRow As Integer = grdUsers.Rows.Count - 1

        grdUsers.Rows(pCurRow).ReadOnly = False
        grdUsers.Rows(pCurRow).Selected = True
        cmdSave.Enabled = True
        cmdAdd.Enabled = False
        cmdEdit.Enabled = False
        cmdDelete.Enabled = False

    End Sub

    Private Sub cmdEdit_Click(sender As System.Object, e As System.EventArgs) Handles cmdEdit.Click
        '==========================================================================================
        mblnEdit = True

        'For i As Integer = 0 To grdUsers.Rows.Count - 1
        '    grdUsers.Rows(i).ReadOnly = False
        'Next

        grdUsers.Rows(grdUsers.CurrentRow.Index).ReadOnly = False

        cmdSave.Enabled = True
        cmdAdd.Enabled = False
        cmdEdit.Enabled = False
        cmdDelete.Enabled = False
    End Sub

    Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
        '==========================================================================================
        Try
            Dim pSealSuiteEntities As New SealSuiteDBEntities()

            If (mblnAdd) Then
                Dim pCurRow As Integer = grdUsers.Rows.Count - 1
                Dim pUser As New tblUser

                Dim pQryID = (From pRec In pSealSuiteEntities.tblUser Select pRec Order By pRec.fldID Descending).ToList()

                If (pQryID.Count > 0) Then
                    pUser.fldID = pQryID(0).fldID + 1
                Else
                    pUser.fldID = 1
                End If

                pUser.fldLastName = grdUsers.Rows(pCurRow).Cells(0).Value
                pUser.fldFirstName = grdUsers.Rows(pCurRow).Cells(1).Value
                pUser.fldSystemLogin = grdUsers.Rows(pCurRow).Cells(2).Value

                Dim pTitle As String = ""
                pTitle = grdUsers.Rows(pCurRow).Cells(3).Value.ToString.Trim()
                Dim pQry = (From pRec In pSealSuiteEntities.tblTitle Where pRec.fldTitle = pTitle Select pRec).ToList()

                If (pQry.Count > 0) Then
                    pUser.fldTitleID = pQry(0).fldID
                End If

                'If (Not IsNothing(grdUsers.Rows(pCurRow).Cells(5).Value)) Then
                '    Dim pImg As Image = grdUsers.Rows(pCurRow).Cells(5).Value
                '    Dim pArray As Byte() = ImgToByteArray(pImg)
                '    pUser.fldSignature = pArray
                'End If

                Dim pImg As Image = grdUsers.Rows(pCurRow).Cells(5).Value
                If Not IsNothing(pImg) Then
                    Dim pArray As Byte() = ImgToByteArray(pImg)
                    pUser.fldSignature = pArray
                End If

                pUser.fldProcess = grdUsers.Rows(pCurRow).Cells(6).Value
                pUser.fldTest = grdUsers.Rows(pCurRow).Cells(7).Value
                pUser.fldIPE = grdUsers.Rows(pCurRow).Cells(8).Value


                pSealSuiteEntities.AddTotblUser(pUser)
                pSealSuiteEntities.SaveChanges()

                mblnAdd = False
                cmdSave.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
                grdUsers.Rows(pCurRow).ReadOnly = True

            ElseIf (mblnEdit) Then

                Dim pCurRow As Integer = grdUsers.CurrentRow.Index
                If (mUserID > 0) Then
                    Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldID = mUserID Select pRec).First()

                    pQryUser.fldID = mUserID
                    pQryUser.fldLastName = grdUsers.Rows(pCurRow).Cells(0).Value
                    pQryUser.fldFirstName = grdUsers.Rows(pCurRow).Cells(1).Value
                    pQryUser.fldSystemLogin = grdUsers.Rows(pCurRow).Cells(2).Value

                    Dim pTitle As String = ""
                    pTitle = grdUsers.Rows(pCurRow).Cells(3).Value.ToString.Trim()
                    Dim pQry1 = (From pRec In pSealSuiteEntities.tblTitle Where pRec.fldTitle = pTitle Select pRec).ToList()

                    If (pQry1.Count > 0) Then
                        pQryUser.fldTitleID = pQry1(0).fldID
                    End If

                    Dim pImg As Image = grdUsers.Rows(pCurRow).Cells(5).Value
                    If Not IsNothing(pImg) Then
                        Dim pArray As Byte() = ImgToByteArray(pImg)
                        pQryUser.fldSignature = pArray
                    End If

                    pQryUser.fldProcess = grdUsers.Rows(pCurRow).Cells(6).Value
                    pQryUser.fldTest = grdUsers.Rows(pCurRow).Cells(7).Value
                    pQryUser.fldIPE = grdUsers.Rows(pCurRow).Cells(8).Value

                    pSealSuiteEntities.SaveChanges()
                End If

                ''Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldFirstName =  Select pRec).ToList()


                'Dim pQry = (From pRec In pSealSuiteEntities.tblUser Select pRec).ToList()
                'For i As Integer = 0 To pQry.Count - 1
                '    pSealSuiteEntities.DeleteObject(pQry(i))
                '    pSealSuiteEntities.SaveChanges()
                'Next

                'For i As Integer = 0 To grdUsers.Rows.Count - 1
                '    Dim pUser As New tblUser

                '    Dim pQryID = (From pRec In pSealSuiteEntities.tblUser Select pRec Order By pRec.fldID Descending).ToList()

                '    If (pQryID.Count > 0) Then
                '        pUser.fldID = pQryID(0).fldID + 1
                '    Else
                '        pUser.fldID = 1
                '    End If

                '    pUser.fldLastName = grdUsers.Rows(i).Cells(0).Value
                '    pUser.fldFirstName = grdUsers.Rows(i).Cells(1).Value
                '    pUser.fldSystemLogin = grdUsers.Rows(i).Cells(2).Value

                '    Dim pTitle As String = ""
                '    pTitle = grdUsers.Rows(i).Cells(3).Value.ToString.Trim()
                '    Dim pQry1 = (From pRec In pSealSuiteEntities.tblTitle Where pRec.fldTitle = pTitle Select pRec).ToList()

                '    If (pQry1.Count > 0) Then
                '        pUser.fldTitleID = pQry1(0).fldID
                '    End If

                '    Dim pImg As Image = grdUsers.Rows(i).Cells(5).Value
                '    If Not IsNothing(pImg) Then
                '        Dim pArray As Byte() = ImgToByteArray(pImg)
                '        pUser.fldSignature = pArray
                '    End If

                '    pUser.fldProcess = grdUsers.Rows(i).Cells(6).Value
                '    pUser.fldTest = grdUsers.Rows(i).Cells(7).Value
                '    pUser.fldIPE = grdUsers.Rows(i).Cells(8).Value


                '    pSealSuiteEntities.AddTotblUser(pUser)
                '    pSealSuiteEntities.SaveChanges()
                '    grdUsers.Rows(i).ReadOnly = True
                'Next

                mblnEdit = False
                cmdSave.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete.Click
        '==============================================================================================
        Dim pintAnswer As Integer

        Dim pSealSuiteEntities As New SealSuiteDBEntities()
        Dim pSystemLogInName As String = grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(2).Value
        If (pSystemLogInName <> "") Then
            pintAnswer = MessageBox.Show("Are you sure you want to permanently delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If pintAnswer = Windows.Forms.DialogResult.Yes Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldSystemLogin = pSystemLogInName Select pRec).First()
                pSealSuiteEntities.DeleteObject(pQry)
                pSealSuiteEntities.SaveChanges()
                PopulateDataGrid()
            End If

        End If

    End Sub

    Private Sub cmdProcess_Click(sender As System.Object, e As System.EventArgs) Handles cmdProcess.Click
        '================================================================================================
        Dim pfrmUserRole As New frmUserRole("Process")
        pfrmUserRole.ShowDialog()
    End Sub

    Private Sub cmdTest_Click(sender As System.Object, e As System.EventArgs) Handles cmdTest.Click
        '==========================================================================================
        Dim pfrmUserRole As New frmUserRole("Test")
        pfrmUserRole.ShowDialog()
    End Sub

    Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
        '============================================================================================
        Me.Close()
    End Sub

#Region "HELPER ROUTINES:"

    Private Function ImgToByteArray(ByVal img As Image) As Byte()
        '=========================================================
        Using mStream As New MemoryStream()
            img.Save(mStream, img.RawFormat)
            Return mStream.ToArray()
        End Using
    End Function

#End Region

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

            grdUsers.Rows(pCurIndex).Cells(5).Value = pImage
        End If

    End Sub

    Private Sub grdUsers_CellClick(sender As System.Object,
                                   e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdUsers.CellClick
        '=============================================================================================================
        GetUserID()
        'If (Not IsNothing(grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(2).Value)) Then
        '    Dim pSystemLogin As String = grdUsers.Rows(grdUsers.CurrentRow.Index).Cells(2).Value

        '    'If (gTest_User.Role = Test_clsUser.eRole.SuperAdmin) Then
        '    cmdDelete.Enabled = True

        '    'ElseIf (gTest_User.Role = Test_clsUser.eRole.Admin) Then
        '    '    If (pSystemLogin.ToUpper() = gTest_User.SystemLogin.ToUpper()) Then
        '    '        cmdDelete.Enabled = False
        '    '    Else
        '    '        cmdDelete.Enabled = True
        '    '    End If

        '    'End If
        'End If
    End Sub

#End Region

#Region "DATABASE RELATED ROUTINES:"

    Private Sub RetrieveFromDB()
        '========================

        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pQry = (From pRec In pSealSuiteEntities.tblProgramDataFile Select pRec).ToList()

        If (pQry.Count > 0) Then
            gUser.ProgramDataFile = pQry(0).fldFileName_UserGroup.Trim()
        End If


    End Sub

    Private Sub grdUsers_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                             Handles grdUsers.RowHeaderMouseClick
        '============================================================================================
        GetUserID()
    End Sub

    Private Sub GetUserID()
        '==================
        Try

            Dim pSealSuiteEntities As New SealSuiteDBEntities()
            Dim pCurRow As Integer = grdUsers.CurrentRow.Index

            Dim pLastName As String = ""
            If (Not IsNothing(grdUsers.Rows(pCurRow).Cells(0).Value)) Then
                pLastName = grdUsers.Rows(pCurRow).Cells(0).Value
            Else
                pLastName = ""
            End If

            Dim pFirstName As String = ""
            If (Not IsNothing(grdUsers.Rows(pCurRow).Cells(1).Value)) Then
                pFirstName = grdUsers.Rows(pCurRow).Cells(1).Value
            Else
                pFirstName = ""
            End If

            Dim pSystemLogin As String = ""
            If (Not IsNothing(grdUsers.Rows(pCurRow).Cells(2).Value)) Then
                pSystemLogin = grdUsers.Rows(pCurRow).Cells(2).Value
            Else
                pSystemLogin = ""
            End If

            If (pLastName <> "" And pFirstName <> "" And pSystemLogin <> "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldLastName = pLastName And pRec.fldFirstName = pFirstName.Trim() And pRec.fldSystemLogin = pSystemLogin.Trim() Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If

            ElseIf (pLastName = "" And pFirstName <> "" And pSystemLogin <> "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldFirstName = pFirstName.Trim() And pRec.fldSystemLogin = pSystemLogin.Trim() Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If

            ElseIf (pLastName = "" And pFirstName = "" And pSystemLogin <> "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldSystemLogin = pSystemLogin.Trim() Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If

            ElseIf (pLastName <> "" And pFirstName <> "" And pSystemLogin = "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldLastName = pLastName And pRec.fldFirstName = pFirstName.Trim() Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If

            ElseIf (pLastName = "" And pFirstName <> "" And pSystemLogin = "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldFirstName = pFirstName.Trim() Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If

            ElseIf (pLastName <> "" And pFirstName = "" And pSystemLogin <> "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldLastName = pLastName And pRec.fldSystemLogin = pSystemLogin.Trim() Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If

            ElseIf (pLastName <> "" And pFirstName = "" And pSystemLogin = "") Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldLastName = pLastName Select pRec).ToList()

                If (pQry.Count > 0) Then
                    mUserID = pQry(0).fldID
                End If
            End If

            'Dim pQry = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldLastName = pLastName And pRec.fldFirstName = pFirstName.Trim() And pRec.fldSystemLogin = pSystemLogin.Trim() Select pRec).ToList()

            'If (pQry.Count > 0) Then
            '    mUserID = pQry(0).fldID
            'End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub SaveToDB()
        '=================
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pRecExists As Boolean = False
        '....tblProgramDataFile
        Dim pQry = (From pRec In pSealSuiteEntities.tblProgramDataFile
                    Where pRec.fldID = 1 Select pRec).ToList()

        Dim pProgramDataFile As New tblProgramDataFile

        If (pQry.Count > 0) Then
            pProgramDataFile = pQry(0)
            pRecExists = True
        End If

        With pProgramDataFile
            .fldID = 1
            .fldFileName_UserGroup = gUser.ProgramDataFile

        End With

        If (pRecExists) Then
            pSealSuiteEntities.SaveChanges()
        Else
            pSealSuiteEntities.AddTotblProgramDataFile(pProgramDataFile)
            pSealSuiteEntities.SaveChanges()

        End If

    End Sub

#End Region


End Class