'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      FORM MODULE   :  frmTest_User_Role                      '
'                        VERSION NO  :  2.6                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  06JUN17                                '
'                                                                              '
'===============================================================================

Public Class Test_frmUser_Role

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_Role_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '============================================================================================
        InitializeControls()
        DisplayData()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        optAdmin.Enabled = False
        optSignoff.Enabled = False
        optTester.Enabled = False
        optEngg.Enabled = False
        optQuality.Enabled = False
        optViewer.Enabled = True

    End Sub

    Private Sub DisplayData()
        '====================
        lblName.Text = gTest_User.Name

        If (gTest_User.Admin) Then
            optAdmin.Enabled = True
            optAdmin.Checked = True
            optViewer.Checked = False
        End If

        If (gTest_User.Tester) Then
            optTester.Enabled = True
            If (Not optAdmin.Checked And Not optEngg.Checked And Not optQuality.Checked) Then
                optTester.Checked = True
                optViewer.Checked = False
            End If
        End If

        If (gTest_User.Engg) Then
            optEngg.Enabled = True
            If (Not optAdmin.Checked And Not optTester.Checked And Not optQuality.Checked) Then
                optEngg.Checked = True
                optViewer.Checked = False
            End If

        End If

        If (gTest_User.Quality) Then
            optQuality.Enabled = True
            If (Not optAdmin.Checked And Not optEngg.Checked And Not optTester.Checked) Then
                optQuality.Checked = True
                optViewer.Checked = False
            End If

        End If

    End Sub

#End Region

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        If (optAdmin.Checked) Then
            gTest_User.Role = Test_clsUser.eRole.Admin

        ElseIf (optTester.Checked) Then
            gTest_User.Role = Test_clsUser.eRole.Tester

        ElseIf (optEngg.Checked) Then
            gTest_User.Role = Test_clsUser.eRole.Eng

        ElseIf (optQuality.Checked) Then
            gTest_User.Role = Test_clsUser.eRole.Quality

        ElseIf (optViewer.Checked) Then
            gTest_User.Role = Test_clsUser.eRole.Viewer

        End If

        Me.Close()
        'Dim pfrmTestMain As New frmTest_Main()
        gTest_frmMain.ShowDialog()

    End Sub

#End Region


    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '===============================================================================================
        gTest_User.Role = Test_clsUser.eRole.Viewer

        gTest_User.Admin = False
        gTest_User.Tester = False
        gTest_User.Engg = False
        gTest_User.Quality = False
        gTest_User.Viewer = True

        Me.Close()
        Dim pTest_frmMain As New Test_frmMain()
        pTest_frmMain.ShowDialog()
    End Sub


    Private Sub optAdmin_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles _
                                                  optAdmin.CheckedChanged, optTester.CheckedChanged,
                                                  optEngg.CheckedChanged, optQuality.CheckedChanged
        '===========================================================================================

        If (optAdmin.Checked) Then
            optViewer.Checked = False

        ElseIf (optTester.Checked) Then
            optViewer.Checked = False

        ElseIf (optEngg.Checked) Then
            optViewer.Checked = False

        ElseIf (optQuality.Checked) Then
            optViewer.Checked = False

        End If

    End Sub


    Private Sub optViewer_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optViewer.CheckedChanged
        '================================================================================================================
        If (optViewer.Checked) Then
            optAdmin.Checked = False
            optTester.Checked = False
            optEngg.Checked = False
            optQuality.Checked = False
        End If
    End Sub
End Class