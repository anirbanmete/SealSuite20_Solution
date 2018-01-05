'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmPDS                         '
'                        VERSION NO  :  1.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  02NOV17                                '
'                                                                              '
'===============================================================================

Public Class Process_frmPDS

    Private mProcess_Project As New clsProcessProj(gPartProject)

    'Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
    '    '=============================================================================================
    '    With openFileDialog1
    '        .Filter = "SealProcess PDS Mapping File (*.xlsx)|*.xls"
    '        .FilterIndex = 1
    '        .InitialDirectory = "C:\SealSuite\SealProcess\Program Data Files"
    '        .FileName = ""
    '        .Title = "Open"

    '        If .ShowDialog = Windows.Forms.DialogResult.OK Then
    '            mProcess_Project.PDSMappingFile = openFileDialog1.FileName
    '            txtFileName.Text = mProcess_Project.PDSMappingFile
    '        End If

    '    End With

    'End Sub

    'Private Sub cmdGenerate_Click(sender As System.Object, e As System.EventArgs) Handles cmdGenerate.Click
    '    '==================================================================================================
    '    Cursor = Cursors.WaitCursor
    '    'mProcess_Project.ReadPDSMappingFile()
    '    mProcess_Project.Handle_PDSFile("Write")
    '    Cursor = Cursors.Default
    'End Sub


    Private Sub Process_frmPDS_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==============================================================================================
        mProcess_Project = gProcessProject.Clone()
    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub
End Class