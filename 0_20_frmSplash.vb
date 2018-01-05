'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  frmSplash                              '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  21DEC17                                '
'                                                                              '
'===============================================================================

Public Class frmSplash

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load


    End Sub


    Private Sub cmdExit_Click(sender As System.Object, e As System.EventArgs) Handles cmdExit.Click
        '============================================================================================
        Me.Close()
    End Sub

    Private Sub cmdMainForm_Click(sender As System.Object, e As System.EventArgs) Handles cmdMainForm.Click
        '===================================================================================================

        Me.Hide()
        '....Create all the forms here:
        '....Part
        'Dim pPart_frmMain As New frmPartMain("PN")
        'pPart_frmMain.ShowDialog()
        gPart_frmMain.ShowDialog()

        'gTest_frmSealOpening.ShowDialog()

    End Sub

End Class
