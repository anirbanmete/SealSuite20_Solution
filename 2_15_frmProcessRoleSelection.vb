'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmRoleSelection               '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  14FEB18                                '
'                                                                              '
'===============================================================================
Imports System.Globalization
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Public Class Process_frmRoleSelection
    Private Sub Process_frmRoleSelection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '=============================================================================================
        Dim pUserRole As New List(Of String)
        pUserRole = gUser.RetrieveProcessUserRoles()

        lblName.Text = gUser.FirstName + " " + gUser.LastName

        cmbRole.Items.Clear()
        For i As Integer = 0 To pUserRole.Count - 1
            cmbRole.Items.Add(pUserRole(i))
        Next
        cmbRole.Items.Add("Viewer")

        'AES 16APR18
        If (Not IsNothing(gUser.Role) And gUser.Role <> "") Then
            For i As Integer = 0 To cmbRole.Items.Count - 1
                If (gUser.Role = cmbRole.Items(i)) Then
                    cmbRole.SelectedIndex = i
                    Exit Sub
                End If
            Next

        End If
        cmbRole.SelectedIndex = 0
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        '========================================================================
        gUser.Role = cmbRole.Text
        Me.Close()
        Dim pProcess_frmMain As New Process_frmMain()
        pProcess_frmMain.Size = New Size(1130, 700)     'AES 09JAN18
        pProcess_frmMain.AutoScroll = True
        pProcess_frmMain.ShowDialog()

    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        '================================================================================
        Me.Close()

    End Sub
End Class