'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealSuite"                            '
'                      FORM MODULE   :  frmAdminLogin                          '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  15FEB18                                '
'                                                                              '
'===============================================================================
'
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms
Imports System.IO
Public Class frmAdminLogin

    Private Const mcLoginPassword As String = "Abercrombie"

    Private Sub frmAdminLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '================================================================================
        lblSystemUserName.Text = "System Login Name : " & Environment.UserName
        txtPassword.Text = ""
    End Sub

    Private Sub cmdButtons_Click(sender As Object, e As EventArgs) Handles cmdOK.Click, cmdCancel.Click
        '==============================================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then SaveData()
        Me.Close()
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        '===================================================================================================
        If e.KeyChar = ChrW(13) Then
            SaveData()
            Me.Close()
        End If
    End Sub

    Private Sub SaveData()
        '=================
        Dim pMsg As String = ""
        Dim pPassword As String = txtPassword.Text.Trim()

        If (pPassword = mcLoginPassword) Then
            'pMsg = "Password matched."
            'pMsg = pMsg & vbCrLf & "Privilege set to Admin."
            'MsgBox(pMsg, MsgBoxStyle.Information, "User Privilege Status")

            Dim pfrmUserGroup As New frmUserGroup()
            pfrmUserGroup.ShowDialog()
        Else
            pMsg = "Password not matched."
            MsgBox(pMsg, MsgBoxStyle.Information, "User Privilege Status")
        End If

    End Sub

End Class