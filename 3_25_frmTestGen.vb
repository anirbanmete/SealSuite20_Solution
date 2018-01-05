'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Gen                            '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05JUL17                                '
'                                                                              '
'===============================================================================
'
Imports System.DateTime
Imports System.Globalization
Imports System.Linq

Public Class Test_frmGen

#Region "MEMBER VARIABLES:"

    Private mTestProject As New Test_clsProject(gPartProject)
    Private mCurRecNo As Integer = 0
    Private mControl() As Control

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTestModule_Gen_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==================================================================================================
        'mControl = {txtSealSN, cmdAdd}
        mControl = {txtSealSN}
        mTestProject = gTest_Project.Clone()
        InitializeControls()

        DisplayData()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================

        '....Set all controls read-only.
        For i As Integer = 0 To mControl.Count - 1
            mControl(i).Enabled = False
        Next

        '....Set the controls (whether read-only or not) according to the gTestUser.Role
        For i As Integer = 0 To mControl.Count - 1
            Dim pVal As String = mControl(i).Tag

            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                If (mTestProject.IsTesterSigned) Then
                    mControl(i).Enabled = False
                Else
                    mControl(i).Enabled = True
                End If


            ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (pVal.Contains("ADef") Or pVal.Contains(gTest_User.Role.ToString())) Then
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

#Region "UTILITY ROUTINE:"

    Private Sub DisplayData()
        '====================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).RetrieveFrom_DB(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
                Dim pTotalRec As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty
                Dim pRecNo As Integer = mCurRecNo

                If (pRecNo = 0) Then

                    If (pTotalRec > 0) Then
                        pRecNo = 1

                    End If
                End If

                txtRecNo.Text = pRecNo
                mCurRecNo = pRecNo
                lblTotalRec.Text = "of " & pTotalRec

                txtSealID.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SeqID.ToString()
                txtSealSN.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SN.ToString()

            End If
        Else
            Dim pTotalRec As Integer = mTestProject.Test_Spec.SealQty
            Dim pRecNo As Integer = mCurRecNo

            If (pRecNo = 0) Then

                If (pTotalRec > 0) Then
                    pRecNo = 1

                End If
            End If

            txtRecNo.Text = pRecNo
            mCurRecNo = pRecNo
            lblTotalRec.Text = "of " & pTotalRec
            txtSealID.Text = pRecNo
            txtSealSN.Text = pRecNo
        End If

        'txtMatThick.Text = gTest_Project.Test_Unit.WriteInUserL_PH(mTestProject.Part_HW.T)

        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            If (gPartProject.PNR.HW.Adjusted) Then
                txtMatThick.Text = gTest_Project.Test_Unit.WriteInUserL_PH(gPartProject.PNR.HW.T)
            Else
                txtMatThick.Text = gTest_Project.Test_Unit.WriteInUserL_PH(gPartProject.PNR.HW.TStd)
            End If
        Else
            txtMatThick.Text = gTest_Project.Test_Unit.WriteInUserL_PH(gPartProject.PNR.HW.TStd)
        End If

    End Sub


    Private Sub SaveData()
        '=================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pGenSeal As New Test_clsReport.sGenSeal
                pGenSeal.SeqID = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SeqID
                pGenSeal.SN = txtSealSN.Text
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1) = pGenSeal

                gTest_Project = mTestProject.Clone()
            End If
        End If

    End Sub

#End Region

#Region "COMMAND BUTTON RELATED ROUTINE:"

    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        '===================================================================
        Dim pTest_frmImage As New Test_frmImage
        pTest_frmImage.ShowDialog()
    End Sub


    Private Sub cmdMoveFirst_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveFirst.Click
        '====================================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pTotalRec As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty
                Dim pRecNo As Integer = 0
                If (pTotalRec > 0) Then
                    pRecNo = 1
                End If
                txtRecNo.Text = pRecNo
                mCurRecNo = pRecNo
            End If
        End If

        DisplayData()

    End Sub


    Private Sub cmdMoveLast_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveLast.Click
        '==================================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pTotalRec As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty
                Dim pRecNo As Integer = 0
                If (pTotalRec > 0) Then
                    pRecNo = pTotalRec
                End If
                txtRecNo.Text = pRecNo
                mCurRecNo = pRecNo
            End If
        End If

        DisplayData()

    End Sub


    Private Sub cmdMovePrev_Click(sender As System.Object, e As System.EventArgs) Handles cmdMovePrev.Click
        '=================================================================================================
        SaveData()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                Dim pCurRec As Integer = ConvertToInt(txtRecNo.Text)
                If (pCurRec > 1) Then
                    txtRecNo.Text = (pCurRec - 1).ToString()
                    mCurRecNo = ConvertToInt(txtRecNo.Text)
                End If
            End If
        End If

        DisplayData()

    End Sub


    Private Sub cmdMoveNext_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveNext.Click
        '==================================================================================================
        SaveData()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pCurRec As Integer = ConvertToInt(txtRecNo.Text)

                If (pCurRec < mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty) Then
                    txtRecNo.Text = (pCurRec + 1).ToString()
                    mCurRecNo = ConvertToInt(txtRecNo.Text)
                End If
            End If
        End If

        DisplayData()

    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblTestRptGenSeal(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
            End If
        End If

        Me.Close()
    End Sub


    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub

#End Region


End Class