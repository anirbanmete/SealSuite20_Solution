'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Notes                          '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29NOV16                                '
'                                                                              '
'===============================================================================
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports clsLibrary11

Public Class Test_frmNotes

#Region "MEMBER VARIABLES:"

    Private mTestProject As New Test_clsProject(gPartProject)
    Private mControl() As Control

#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_Notes_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '============================================================================================
        Me.Text = "SealTest: Notes Form"
        mTestProject = gTest_Project.Clone()
        mControl = {optPass, optFail, chkOverride, txtNotes}

        InitializeControls()

        grpStatus.Enabled = False
        gIsTestMainActive = False
        DisplayData()
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================

        '....Set all controls read-only.
        For i As Integer = 0 To mControl.Count - 1
            mControl(i).Enabled = False
        Next

        '....Set the controls (whether read-only or not) according to the gTest_User.Role
        For i As Integer = 0 To mControl.Count - 1
            Dim pVal As String = mControl(i).Tag

            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                'mControl(i).Enabled = True

                If (mTestProject.IsTesterSigned) Then
                    mControl(i).Enabled = False
                Else
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (pVal.Contains("ADef") Or pVal.Contains(gTest_User.Role.ToString())) Then
                    'mControl(i).Enabled = True

                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester.Signed) Then
                        mControl(i).Enabled = False
                    Else
                        mControl(i).Enabled = True
                    End If
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Eng) Then

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Quality) Then

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            End If
        Next

    End Sub

#End Region

#End Region


#Region "COMMAND BUTTON RELATED ROUTINE:"

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        SaveData()
        gTest_Project = mTestProject.Clone()
        Me.Close()
    End Sub


    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub

#End Region


#Region "CHECK BOX RELATED ROUTINE:"

    Private Sub chkOverride_CheckedChanged(sender As System.Object,
                                           e As System.EventArgs) Handles chkOverride.CheckedChanged
        '============================================================================================
        If (chkOverride.Checked) Then
            If (optFail.Checked) Then
                optPass.Checked = True
            End If
        Else
            optFail.Checked = True
        End If

    End Sub

    Private Sub ttpToolTip1_Draw(sender As System.Object,
                                 e As System.Windows.Forms.DrawToolTipEventArgs) Handles ttpToolTip1.Draw
        '================================================================================================
        e.DrawBackground()
        e.DrawBorder()
        e.DrawText()
    End Sub

    Private Sub chkOverride_MouseHover(sender As System.Object,
                                       e As System.EventArgs) Handles chkOverride.MouseHover
        '===================================================================================
        ttpToolTip1.SetToolTip(chkOverride, "Please give reasons below.")
    End Sub

#End Region


#Region "UTILITY ROUTINE:"

    Private Sub DisplayData()
        '=====================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                'AES 30NOV16
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SetTestStatus(mTestProject.Test_Spec.LeakMax.Unplated, mTestProject.Test_Spec.LeakSpringBackMin.Unplated, mTestProject.Test_Spec.LoadType, mTestProject.Test_Spec.LoadVal(1), mTestProject.Test_Spec.LoadVal(0))

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DetermineOverallStatus() = Test_clsSeal.eStatus.Pass) Then
                    optPass.Checked = True
                    chkOverride.Enabled = False
                Else
                    optFail.Checked = True
                    chkOverride.Enabled = True
                End If

                txtNotes.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Notes
                chkOverride.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Overridden
            End If
        End If

    End Sub

    Private Sub SaveData()
        '=================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Notes = txtNotes.Text
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Overridden = chkOverride.Checked
            End If
        End If
        gTest_Project = mTestProject.Clone()
    End Sub

#End Region


End Class