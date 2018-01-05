'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  frmSealTestOpening                     '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05JUL17                                '
'                                                                              '
'===============================================================================

Public Class Test_frmSealOpening

    Private Sub cmdButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                 Handles cmdPN.Click, cmdSealTest.Click, cmdExit.Click
        '======================================================================================

        'Error Message:              
        '-------------
        Dim pintAttributes As String
        Dim pstrTitle As String
        Dim pstrMsg As String

        pintAttributes = vbCritical + vbOKOnly

        Dim pcmdButton As Button = CType(sender, Button)

        Try

            Select Case pcmdButton.Name

                Case "cmdPN"
                    '------------

                    Me.Cursor = Cursors.WaitCursor

                    ''Dim pfrmPartInfo As New frmPartInfo("SealTest")
                    gPart_frmPartInfo.ShowDialog()

                    'AES 07JUN17
                    gTest_Project.PN_Selected = gPart_frmPartInfo.PN
                    gTest_Project.Rev_Selected = gPart_frmPartInfo.PN_Rev

                    Me.Cursor = Cursors.Default

                Case "cmdSealTest"
                    '----------------

                    gTest_User.RetrieveUserRoles()

                    Dim pRoleCount As Integer = 0

                    If (gTest_User.Admin) Then
                        pRoleCount = pRoleCount + 1
                    End If

                    If (gTest_User.Tester) Then
                        pRoleCount = pRoleCount + 1
                    End If

                    If (gTest_User.Engg) Then
                        pRoleCount = pRoleCount + 1
                    End If

                    If (gTest_User.Quality) Then
                        pRoleCount = pRoleCount + 1
                    End If

                    If (pRoleCount > 1) Then

                        Dim pTest_frmUser_Role As New Test_frmUser_Role()
                        pTest_frmUser_Role.ShowDialog()

                    Else
                        gTest_User.Role = Test_clsUser.eRole.Viewer
                        gTest_frmMain.ShowDialog()
                    End If


                Case "cmdExit"
                    '------------
                    End

            End Select

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub cmdPN_Click(sender As System.Object, e As System.EventArgs) Handles cmdPN.Click, cmdPartNo.Click

    End Sub

    Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '=======================================================================================
        Me.Text = "SealTest: Opening Form"
    End Sub
End Class