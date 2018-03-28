'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  Process_frmNotes                       '
'                        VERSION NO  :  1.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  02NOV17                                '
'                                                                              '
'===============================================================================
Public Class Process_frmNotes

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub Process_frmNotes_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'For i As Integer = 0 To lstNotes_Dim.Items.Count - 1
        '    If (i = lstNotes_Dim.Items.Count - 1) Then
        '        lstNotes_Dim.Items.Item
        '    End If
        'Next
    End Sub
End Class