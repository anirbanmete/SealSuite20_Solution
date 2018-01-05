'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      FORM MODULE   :  frmTest_Leak                           '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05JUL17                                '
'                                                                              '
'===============================================================================
'
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports System.IO
Imports clsLibrary11

Public Class Test_frmLeak

#Region "MEMBER VARIABLES:"
    Private mTestProject As New Test_clsProject(gPartProject)
    Private mCurRecNo As Integer = 0
    Private mControl() As Control


#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_Leak_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        '===========================================================================================
        If (gIsTestLeakActive = False) Then
            mTestProject = gTest_Project.Clone()

            'mTestProject.Unit = gTest_Project.Unit.Clone()
            gIsTestLeakActive = True
        End If
    End Sub


    Private Sub frmTest_Leak_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '============================================================================================
        Me.Text = "SealTest: Leakage Form"
        mTestProject = gTest_Project.Clone()
        mControl = {cmbStand, txtFixture, cmbMedium, txtTemp, txtTarget, txtActual, txtDesc, cmbType, cmbSN, _
                    txtPress, txtFHIni_Max, txtFHIni_Measured, txtFHIni_Min, txtPre_Max, txtPre_Measured, txtPre_Min, _
                    txtLeak_Max, txtLeak_Measured, txtFHFinal_Measured, chkSpringBack, txtSpringBack_Min, txtPost_Measured, _
                    cmbPlatenSurfaceFinish, cmdSealIPE}
        InitializeControls()
        SetUnits_Label()
        PopulateCmbBoxes()
        mTestProject.Test_Spec.RetrieveFrom_DB(mTestProject.ID)
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).RetrieveFrom_DB(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)

                'AES 14NOV16
                For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal.Count - 1
                    Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                    If (pIsLeak = True Or pIsLoad = False) Then
                        mCurRecNo = i + 1
                        Exit For
                    End If
                Next

            End If
        End If

        DisplayData()
        DetermineStatus()                       'AES 29NOV16
        gIsTestMainActive = False

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

            If (pVal.Contains("Design")) Then
                Dim pTemp As Double = 0.0
                If (mControl(i).Name = "txtTarget") Then
                    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakCavityDepth)

                End If
                ttpToolTip1.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtFHIni_Max") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Plated)
                        Else
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
                        End If
                    Else
                        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                                    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Plated)
                                Else
                                    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
                                End If
                            End If

                        End If

                    End If

                    'pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
                End If
                ttpToolTip1.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtFHIni_Min") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Plated)
                        Else
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)
                        End If
                    Else
                        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                                    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Plated)
                                Else
                                    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)
                                End If
                            End If

                        End If
                    End If
                    'pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)
                End If
                ttpToolTip1.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtPre_Max") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Plated)
                            ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Plated)
                            End If
                        Else
                            If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                            ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                            End If
                        End If
                    Else
                        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Plated)
                                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Plated)
                                    End If
                                Else
                                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                                    End If
                                End If
                            End If

                        End If
                    End If

                    'If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    '    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                    'ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    '    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                    'End If
                End If
                ttpToolTip1.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtPre_Min") Then

                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Plated)
                            ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Plated)
                            End If
                        Else
                            If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
                            ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
                            End If
                        End If
                    Else
                        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Plated)
                                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Plated)
                                    End If
                                Else
                                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
                                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
                                    End If
                                End If
                            End If

                        End If
                    End If

                    'If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    '    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
                    'ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    '    pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
                    'End If
                End If
                ttpToolTip1.SetToolTip(mControl(i), pTemp.ToString())

            End If

            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then

                If (mTestProject.IsTesterSigned) Then
                    mControl(i).Enabled = False
                Else
                    mControl(i).Enabled = True
                    'AES 11MAY17
                    If (mTestProject.SealIPE_FEA = False) Then
                        cmdSealIPE.Enabled = False
                    End If
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (pVal.Contains("ADef") Or pVal.Contains(gTest_User.Role.ToString())) Then

                    'AES 09JUN17
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

        '....comment out for demo
        'If (gTestUser.Role = clsTestUser.eRole.Admin) Then
        '    cmdLoadStep.Visible = True
        '    cmdFEAGraphics.Visible = True
        'Else
        '    cmdLoadStep.Visible = False
        '    cmdFEAGraphics.Visible = False

        'End If

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakPress < gcEPS) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakPress = mTestProject.Test_Spec.LeakPress
                End If


                If (mTestProject.Test_Spec.Leak.Springback = True) Then
                    chkSpringBack.Checked = True
                    'mTestProject.Test_MO(gTest_frmMain.MO_Sel).Report(gTest_frmMain.Report_Sel).Leak_Springback = True
                End If
            End If

        End If
    End Sub


    Private Sub SetUnits_Label()
        '=======================

        lblCavityDepth_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblPress_Unit.Text = "(" & mTestProject.Test_Unit.PUnit_PH & ")"
        lblIniFH_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblPre_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblLeak_Unit.Text = "(" & mTestProject.Test_Unit.LeakUnit_PH & ")"
        lblUnitLoad_Unit.Text = "(" & mTestProject.Test_Unit.FUnit_PH & "/" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblFinalFH_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblSpringBack_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        'lblSurface_Unit.Text = "(" & mTestProject.Unit.LUnit_PH & ")"

    End Sub

    Private Sub PopulateCmbBoxes()
        '=========================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Stand
        Dim pQryStand = (From pRec In pSealTestEntities.tblLeakStand Select pRec).ToList()

        If (pQryStand.Count() > 0) Then
            cmbStand.Items.Clear()

            For i As Integer = 0 To pQryStand.Count() - 1
                cmbStand.Items.Add(pQryStand(i).fldName.Trim())
            Next

            cmbStand.SelectedIndex = 0
        End If

        '....Medium
        Dim pQryLeakMedium = (From pRec In pSealTestEntities.tblLeakMedium Select pRec).ToList()

        If (pQryLeakMedium.Count() > 0) Then
            cmbMedium.Items.Clear()

            For i As Integer = 0 To pQryLeakMedium.Count() - 1
                cmbMedium.Items.Add(pQryLeakMedium(i).fldName.Trim())
            Next

            cmbMedium.SelectedIndex = 0
        End If

        '....Make
        Dim pQryType = (From pRec In pSealTestEntities.tblFlowMeter Select pRec.fldMake Distinct).ToList()
        cmbType.Items.Clear()

        If (pQryType.Count() > 0) Then

            For i As Integer = 0 To pQryType.Count() - 1
                cmbType.Items.Add(pQryType(i).Trim())
            Next

            cmbType.SelectedIndex = 0
        End If

        'AES 15MAR17
        '....Platen Surface Finish
        Dim pQryPlatenSF = (From pRec In pSealTestEntities.tblPlatenSurfaceFinish Select pRec).ToList()

        If (pQryPlatenSF.Count() > 0) Then
            cmbPlatenSurfaceFinish.Items.Clear()

            For i As Integer = 0 To pQryPlatenSF.Count() - 1
                cmbPlatenSurfaceFinish.Items.Add(pQryPlatenSF(i).fldSF_Platen)
            Next

            cmbPlatenSurfaceFinish.SelectedIndex = 0
        End If

    End Sub


#End Region

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub DisplayData()
        '====================

        If TabControl1.SelectedIndex = 0 Then
            'lblFEA.Visible = False
            cmdSealIPE.Enabled = False
        Else
            'lblFEA.Visible = True
            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                If (mTestProject.SealIPE_FEA = False) Then
                    cmdSealIPE.Enabled = False
                    txtUnitLoad.ReadOnly = False
                    txtUnitLoad.ForeColor = Color.Black

                    txtFHFinal_Max.ReadOnly = False
                    txtFHFinal_Max.ForeColor = Color.Black
                Else
                    cmdSealIPE.Enabled = True
                    txtUnitLoad.ReadOnly = True
                    txtUnitLoad.ForeColor = Color.Blue

                    txtFHFinal_Max.ReadOnly = True
                    txtFHFinal_Max.ForeColor = Color.Blue
                End If
            End If

        End If

        '....Equip Tab
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.StandName <> "") Then
                    cmbStand.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.StandName

                End If

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.Fixture <> "") Then
                    txtFixture.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.Fixture

                End If

                If (Math.Abs(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.T - 70.0) < gcEPS) Then
                    txtTemp.Text = "Room"
                Else
                    txtTemp.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.T.ToString("#0.0")
                End If

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.ShimActual > gcEPS) Then
                    txtActual.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.ShimActual)
                End If

                txtDesc.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.ShimDescrip
                'AES 15MAR17
                cmbPlatenSurfaceFinish.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.Platen_SF.ToString()

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.TestMeterMake <> "") Then
                    cmbType.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.TestMeterMake

                End If

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.TestMeterSN <> "") Then
                    cmbSN.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.TestMeterSN
                    cmbSN.SelectedIndex = cmbSN.Items.IndexOf(cmbSN.Text)
                End If

            End If

        End If


        If (mTestProject.Test_Spec.LeakMedium <> "") Then
            cmbMedium.Text = mTestProject.Test_Spec.LeakMedium

        End If

        txtTarget.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakCavityDepth)

        Dim pActual As Double = 0.0
        If (txtActual.Text <> "") Then
            pActual = Convert.ToSingle(txtActual.Text)
        End If

        'If (gTest_User.Role = clsTestUser.eRole.Tester) Then
        If (pActual < gcEPS) Then
            txtActual.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakCavityDepth)
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakCavityDepth)
        End If
        'End If


        '....Seal Tab
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
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

                txtSealNo.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SeqID.ToString()
                txtSN.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SN.ToString()

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.ShimActual > gcEPS) Then
                    txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.ShimActual)
                End If

                Dim pPress_Rpt As Double = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakPress
                txtPress.Text = pPress_Rpt

                txtFHIni_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.FHIni)

                txtFHFinal_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.FHFinal)

                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    txtPre_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.ODPre)
                    txtPost_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.ODPost)

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    txtPre_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.IDPre)
                    txtPost_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.IDPost)
                End If

                'txtLeak_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.Val)
                txtLeak_Measured.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Leak.Val.ToString("0.000E+0")      'AES 11OCT17
                chkSpringBack.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.Springback

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
            txtSealNo.Text = pRecNo
            txtSN.Text = pRecNo

        End If

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            Dim pPress_Rpt As Double = mTestProject.Test_Spec.LeakPress
            txtPress.Text = pPress_Rpt
        End If

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then

            If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Plated)
                txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Plated)

                txtLeak_Max.Text = mTestProject.Test_Spec.LeakMax.Plated.ToString("0.000E+0")
                txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Plated)
            Else
                txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
                txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)

                txtLeak_Max.Text = mTestProject.Test_Spec.LeakMax.Unplated.ToString("0.000E+0")
                txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Unplated)
            End If

        Else

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                        txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Plated)
                        txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Plated)

                        txtLeak_Max.Text = mTestProject.Test_Spec.LeakMax.Plated.ToString("0.000E+0")
                        txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Plated)
                    Else
                        txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
                        txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)

                        txtLeak_Max.Text = mTestProject.Test_Spec.LeakMax.Unplated.ToString("0.000E+0")
                        txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Unplated)
                    End If

                End If
            End If

        End If
        'txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
        'txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)

        txtUnitLoad.Text = mTestProject.Test_Spec.Result.Leak_UnitLoad.ToString("####.#")
        txtFHFinal_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.Result.Leak_FHFinal)

        If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
            lblPre.Text = "OD Pre"
            lblPost.Text = "OD Post"

            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                    txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Plated)
                    txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Plated)
                Else
                    txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                    txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
                End If
            Else

                If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                        If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                            txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Plated)
                            txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Plated)
                        Else
                            txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                            txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
                        End If

                    End If
                End If

            End If
            'txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
            'txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)


        ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
            lblPre.Text = "ID Pre"
            lblPost.Text = "ID Post"

            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                    txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Plated)
                    txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Plated)
                Else
                    txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                    txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
                End If
            Else

                If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                        If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                            txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Plated)
                            txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Plated)
                        Else
                            txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                            txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
                        End If

                    End If
                End If

            End If

            'txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
            'txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)

        End If

        'txtLeak_Max.Text = mTestProject.Test_Spec.LeakMax.Unplated.ToString("0.000E+0")
        'txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Unplated)

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            chkLeak.Checked = mTestProject.Test_Spec.Leak.Leakage
            chkSpringBack.Checked = mTestProject.Test_Spec.Leak.Springback
        End If
        CalcSpringBack()


    End Sub


    Private Sub CalcSpringBack()
        '=======================
        'Dim pFHIni As Double = 0.0
        Dim pFHFinal_Measured As Double = 0.0
        Dim pFHFinal_Max As Double = 0.0
        Dim pCavityDepth As Double = 0.0
        Dim pSpringBack As Double = 0.0

        If (txtCavityDepth.Text <> "") Then
            pCavityDepth = ConvertToSng(txtCavityDepth.Text)

        End If

        If (txtFHFinal_Measured.Text <> "") Then
            pFHFinal_Measured = ConvertToSng(txtFHFinal_Measured.Text)

        End If

        If (txtFHFinal_Max.Text <> "") Then
            pFHFinal_Max = ConvertToSng(txtFHFinal_Max.Text)
        End If

        If (pFHFinal_Measured > gcEPS And pCavityDepth > gcEPS) Then
            txtSpringBack_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(pFHFinal_Measured - pCavityDepth)
        Else
            txtSpringBack_Measured.Text = ""
        End If

        If (pFHFinal_Max > gcEPS And pCavityDepth > gcEPS) Then
            txtSpringBack_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(pFHFinal_Max - pCavityDepth)
        Else
            txtSpringBack_Max.Text = ""
        End If

    End Sub

    Private Sub DetermineStatus()
        '========================
        Dim pLeakMax As Double = 0.0
        Dim pLeakMeasured As Double = 0.0
        Dim pSpringMeasured As Double = 0.0
        Dim pSpringBackMin As Double = 0.0

        If (txtLeak_Max.Text <> "") Then
            pLeakMax = ConvertToSng(txtLeak_Max.Text)
        End If

        If (txtLeak_Measured.Text <> "") Then
            pLeakMeasured = ConvertToSng(txtLeak_Measured.Text)
        End If

        If (txtSpringBack_Measured.Text <> "") Then
            pSpringMeasured = ConvertToSng(txtSpringBack_Measured.Text)
        End If

        If (txtSpringBack_Min.Text <> "") Then
            pSpringBackMin = ConvertToSng(txtSpringBack_Min.Text)
        End If


        If (pLeakMeasured <> 0.0 And pLeakMax <> 0.0) Then

            If (pLeakMeasured <= pLeakMax) Then

                If (chkSpringBack.Checked) Then

                    If (pSpringMeasured > gcEPS And pSpringBackMin > gcEPS) Then
                        If (pSpringMeasured >= pSpringBackMin) Then
                            'optPass.Checked = True
                            optPass.Enabled = True
                            optPass.Checked = True
                            optPass.ForeColor = Color.Green

                            optFail.ForeColor = Color.Black
                            optFail.Enabled = False
                        Else
                            'optFail.Checked = True
                            optPass.ForeColor = Color.Black
                            optPass.Enabled = False

                            optFail.Enabled = True
                            optFail.ForeColor = Color.Red
                            optFail.Checked = True
                        End If
                    Else

                        'optPass.Checked = True
                        optPass.Enabled = True
                        optPass.Checked = True
                        optPass.ForeColor = Color.Green

                        optFail.ForeColor = Color.Black
                        optFail.Enabled = False
                    End If

                Else
                    'optPass.Checked = True
                    optPass.Enabled = True
                    optPass.Checked = True
                    optPass.ForeColor = Color.Green

                    optFail.ForeColor = Color.Black
                    optFail.Enabled = False

                End If
            Else        'AES 29NOV16
                'optFail.Checked = True
                optPass.ForeColor = Color.Black
                optPass.Enabled = False

                optFail.Enabled = True
                optFail.ForeColor = Color.Red
                optFail.Checked = True
            End If

        Else
            If (chkSpringBack.Checked) Then
                If (pSpringMeasured > gcEPS And pSpringBackMin > gcEPS) Then
                    If (pSpringMeasured >= pSpringBackMin) Then
                        'optPass.Checked = True
                        optPass.Enabled = True
                        optPass.Checked = True
                        optPass.ForeColor = Color.Green

                        optFail.ForeColor = Color.Black
                        optFail.Enabled = False
                    Else
                        'optFail.Checked = True
                        optPass.ForeColor = Color.Black
                        optPass.Enabled = False

                        optFail.Enabled = True
                        optFail.ForeColor = Color.Red
                        optFail.Checked = True
                    End If
                Else

                    'optPass.Checked = True
                    optPass.Enabled = True
                    optPass.Checked = True
                    optPass.ForeColor = Color.Green

                    optFail.ForeColor = Color.Black
                    optFail.Enabled = False
                End If

            Else
                'optPass.Checked = True
                optPass.Enabled = True
                optPass.Checked = True
                optPass.ForeColor = Color.Green

                optFail.ForeColor = Color.Black
                optFail.Enabled = False
            End If

        End If

    End Sub

#End Region


#Region "TAB CONTROL RELATED ROUTINES:"

    Private Sub TabControl1_SelectedIndexChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        '==========================================================================================================
        SaveData()
        DisplayData()

    End Sub

#End Region


#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbStand_SelectedIndexChanged(sender As System.Object,
                                              e As System.EventArgs) Handles cmbStand.SelectedIndexChanged
        '==================================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Fixture
        Dim pQryFixture = (From pRec In pSealTestEntities.tblLeakStand Where pRec.fldName = cmbStand.Text Select pRec).ToList()

        If (pQryFixture.Count() > 0) Then
            'cmbFixture.Items.Clear()

            For i As Integer = 0 To pQryFixture.Count() - 1
                Dim pFixtureName As String = pQryFixture(i).fldFixture.Trim()
                txtFixture.Text = pFixtureName
                'Dim pVal() As String = pFixtureName.Split(",")

                'For j As Integer = 0 To pVal.Length - 1
                '    cmbFixture.Items.Add(pVal(j).Trim())
                'Next

            Next

            'cmbFixture.SelectedIndex = 0
        End If

    End Sub


    Private Sub cmbSN_SelectedIndexChanged(sender As System.Object,
                                           e As System.EventArgs) Handles cmbSN.SelectedIndexChanged
        '============================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Flow Meter
        Dim pQryFM = (From pRec In pSealTestEntities.tblFlowMeter Where pRec.fldSN.Trim() = cmbSN.Text.Trim() Select pRec).ToList()

        If (pQryFM.Count() > 0) Then

            For i As Integer = 0 To pQryFM.Count() - 1

                Dim pRange As String = pQryFM(i).fldRange.Trim()
                txtRange.Text = pRange
                txtModelNo.Text = pQryFM(i).fldModelNo.Trim()
                Dim pCalDue As DateTime = pQryFM(i).fldDateCalibrationDue
                txtCalDue.Text = pCalDue.ToShortDateString()
            Next

        End If
    End Sub


    Private Sub cmbType_SelectedIndexChanged(sender As System.Object,
                                             e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        '===============================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Flow Meter
        Dim pQryFM = (From pRec In pSealTestEntities.tblFlowMeter Where pRec.fldMake.Trim() = cmbType.Text.Trim() Select pRec).ToList()

        If (pQryFM.Count() > 0) Then
            cmbSN.Items.Clear()

            For i As Integer = 0 To pQryFM.Count() - 1
                cmbSN.Items.Add(pQryFM(i).fldSN.Trim())
            Next

            cmbSN.SelectedIndex = 0
        End If

    End Sub


#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txt_TextChanged(sender As System.Object,
                                e As System.EventArgs) Handles txtTarget.TextChanged, txtFHIni_Max.TextChanged,
                                                               txtFHIni_Min.TextChanged, txtPre_Max.TextChanged,
                                                               txtPre_Min.TextChanged
        '=======================================================================================================
        Dim ptxtBox As TextBox = CType(sender, TextBox)

        Select Case ptxtBox.Name

            Case "txtTarget"
                Dim pVal As Double = mTestProject.Test_Spec.LeakCavityDepth
                SetForeColor(ptxtBox, pVal)

            Case "txtFHIni_Max"
                Dim pVal As Double = 0.0
                If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                    If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                        pVal = mTestProject.Test_Spec.SealFHIni(1).Plated
                    Else
                        pVal = mTestProject.Test_Spec.SealFHIni(1).Unplated
                    End If
                End If

                SetForeColor(ptxtBox, pVal)

            Case "txtFHIni_Min"
                'Dim pVal As Double = mTestProject.Test_Spec.SealFHIni(1).Unplated
                Dim pVal As Double = 0.0
                If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                    If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                        pVal = mTestProject.Test_Spec.SealFHIni(1).Plated
                    Else
                        pVal = mTestProject.Test_Spec.SealFHIni(1).Unplated
                    End If
                End If
                SetForeColor(ptxtBox, pVal)

            Case "txtPre_Max"
                Dim pMaxVal As Double
                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            pMaxVal = mTestProject.Test_Spec.SealODPre(1).Plated
                        Else
                            pMaxVal = mTestProject.Test_Spec.SealODPre(1).Unplated
                        End If
                    End If

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            pMaxVal = mTestProject.Test_Spec.SealIDPre(1).Plated
                        Else
                            pMaxVal = mTestProject.Test_Spec.SealIDPre(1).Unplated
                        End If
                    End If

                End If
                SetForeColor(ptxtBox, pMaxVal)

            Case "txtPre_Min"
                Dim pMinVal As Double
                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            pMinVal = mTestProject.Test_Spec.SealODPre(0).Plated
                        Else
                            pMinVal = mTestProject.Test_Spec.SealODPre(0).Unplated
                        End If
                    End If

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                        If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                            pMinVal = mTestProject.Test_Spec.SealIDPre(0).Plated
                        Else
                            pMinVal = mTestProject.Test_Spec.SealIDPre(0).Unplated
                        End If
                    End If

                End If
                SetForeColor(ptxtBox, pMinVal)

        End Select

    End Sub


    Private Sub ttpToolTip1_Draw(sender As System.Object,
                                 e As System.Windows.Forms.DrawToolTipEventArgs) Handles ttpToolTip1.Draw
        '=================================================================================================
        e.DrawBackground()
        e.DrawBorder()
        e.DrawText()

    End Sub


    Private Sub SetForeColor(ByVal TxtBox_In As TextBox, ByVal Val_Org_In As Double)
        '===========================================================================
        If (Math.Abs(ConvertToSng(TxtBox_In.Text) - Val_Org_In) < gcEPS) Then
            TxtBox_In.ForeColor = Color.Blue
        Else
            TxtBox_In.ForeColor = Color.Green
        End If
    End Sub


    Private Sub txtPress_Validated(sender As System.Object,
                                   e As System.EventArgs)
        '===================================================
        Dim pPress As Double = 0.0
        If (txtPress.Text <> "") Then
            pPress = ConvertToSng(txtPress.Text)

        End If

        If (pPress < gcEPS) Then
            txtLeak_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(0.0)
            txtLeak_Max.Enabled = True
        Else
            txtLeak_Max.Enabled = False
        End If

    End Sub


    Private Sub txtActual_Validated(sender As System.Object,
                                    e As System.EventArgs) Handles txtActual.Validated
        '=============================================================================
        Dim pCavityDepth As Double = 0.0
        If (txtActual.Text <> "") Then
            pCavityDepth = ConvertToSng(txtActual.Text)
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(pCavityDepth)
        End If
    End Sub


    Private Sub txtPress_Validated_1(sender As System.Object,
                                     e As System.EventArgs) Handles txtSpringBack_Min.Validated,
                                     txtPress.Validated, txtLeak_Measured.Validated, txtLeak_Max.Validated
        '===================================================================================================
        DetermineStatus()

    End Sub


    Private Sub txtFHFinal_Measured_Validated(sender As System.Object,
                                              e As System.EventArgs) Handles txtFHFinal_Measured.Validated
        '=================================================================================================
        CalcSpringBack()

    End Sub


    Private Sub txtActual_Click(sender As System.Object, e As System.EventArgs) Handles txtActual.Click
        '================================================================================================
        Dim pTest_frmMeasure_Shim As New Test_frmMeasure_Shim(Me)
        pTest_frmMeasure_Shim.ShowDialog()
    End Sub


    Private Sub txtActual_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtActual.KeyDown
        '====================================================================================================================
        Dim pTest_frmMeasure_Shim As New Test_frmMeasure_Shim(Me)
        pTest_frmMeasure_Shim.ShowDialog()
    End Sub


    Private Sub txtFHIni_Measured_Click(sender As System.Object, e As System.EventArgs) Handles txtFHIni_Measured.Click
        '==============================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Ini", Me)
        pTest_frmMeasure_FH.ShowDialog()

    End Sub


    Private Sub txtFHIni_Measured_KeyDown(sender As System.Object,
                                          e As System.Windows.Forms.KeyEventArgs) Handles txtFHIni_Measured.KeyDown
        '===========================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Ini", Me)
        pTest_frmMeasure_FH.ShowDialog()
    End Sub


    Private Sub txtFHFinal_Measured_Click(sender As System.Object, e As System.EventArgs) Handles txtFHFinal_Measured.Click
        '====================================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Final", Me)
        pTest_frmMeasure_FH.ShowDialog()

    End Sub


    Private Sub txtFHFinal_Measured_KeyDown(sender As System.Object,
                                            e As System.Windows.Forms.KeyEventArgs) Handles txtFHFinal_Measured.KeyDown
        '===============================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Final", Me)
        pTest_frmMeasure_FH.ShowDialog()

    End Sub

    Private Sub txtFHFinal_Max_Validated(sender As System.Object, e As System.EventArgs) _
                                        Handles txtFHFinal_Max.Validated
        '==================================================================================
        CalcSpringBack()
    End Sub

#End Region


#Region "CHECK BOX RELATED ROUTINE:"

    Private Sub chkSpringBack_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSpringBack.CheckedChanged
        '========================================================================================================================

        If (chkSpringBack.Checked) Then
            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                    txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Plated)
                Else
                    txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Unplated)
                End If
            Else
                If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                        If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.LeakagePlate) Then
                            txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Plated)
                        Else
                            txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Unplated)
                        End If
                    End If

                End If

            End If
        Else
            txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(0.0)
        End If

        DetermineStatus()

    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"


    Private Sub cmdSealIPE_Click(sender As System.Object, e As System.EventArgs) Handles cmdSealIPE.Click
        '================================================================================================
        Cursor = Cursors.WaitCursor
        SaveData()
        mTestProject.RunANSYS_FEA("Leak", gIPE_ANSYS)
        If mTestProject.Analysis.Result_Gen.SolnConv = 1 Then
            mTestProject.Test_Spec.Result_DSealing = mTestProject.Analysis.Seal.DSealing
            mTestProject.Test_Spec.Result_Leak_UnitLoad = mTestProject.Analysis.Seal.FSeat_Unit(1)
            mTestProject.Test_Spec.Result_Leak_FHFinal = mTestProject.Analysis.Seal.HfreeFinal
            '....tblTestSpec
            mTestProject.Test_Spec.SaveTo_DB()
        End If

        DisplayData()
        Cursor = Cursors.Default
    End Sub

    Private Sub cmdMoveFirst_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveFirst.Click
        '====================================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pTotalRec As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty
                Dim pRecNo As Integer = 0
                If (pTotalRec > 0) Then
                    'AES 14NOV16
                    For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty - 1

                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLeak = True Or pIsLoad = False) Then
                            mCurRecNo = i + 1
                            txtRecNo.Text = mCurRecNo.ToString()
                            Exit For
                        End If
                    Next

                End If

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
                Dim pCurRecNo As Integer = ConvertToInt(txtRecNo.Text)
                If (pTotalRec > 0) Then
                    'pRecNo = pTotalRec
                    'AES 14NOV16
                    For i As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty - 1 To 0 Step -1

                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLeak = True Or pIsLoad = False) Then
                            mCurRecNo = i + 1
                            txtRecNo.Text = mCurRecNo.ToString()
                            Exit For
                        End If
                    Next
                End If
                'txtRecNo.Text = pRecNo
                'mCurRecNo = pRecNo
            End If
        End If

        DisplayData()

    End Sub


    Private Sub cmdMovePrev_Click(sender As System.Object, e As System.EventArgs) Handles cmdMovePrev.Click
        '=================================================================================================
        SaveData()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                Dim pCurRecNo As Integer = ConvertToInt(txtRecNo.Text)
                If (pCurRecNo > 1) Then

                    'AES 14NOV16
                    For i As Integer = pCurRecNo - 2 To 0 Step -1
                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLeak = True Or pIsLoad = False) Then
                            mCurRecNo = i + 1
                            txtRecNo.Text = mCurRecNo.ToString()
                            Exit For
                        End If
                    Next

                    'txtRecNo.Text = (pCurRecNo - 1).ToString()
                    'mCurRecNo = ConvertToInt(txtRecNo.Text)
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
                Dim pCurRecNo As Integer = Convert.ToInt64(txtRecNo.Text)

                If (pCurRecNo < mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty) Then

                    'AES 14NOV16
                    For i As Integer = pCurRecNo To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty - 1

                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLeak = True Or pIsLoad = False) Then
                            mCurRecNo = i + 1
                            txtRecNo.Text = mCurRecNo.ToString()
                            Exit For
                        End If
                    Next

                    'txtRecNo.Text = (pCurRecNo + 1).ToString()
                    'mCurRecNo = ConvertToInt(txtRecNo.Text)
                End If
            End If
        End If

        DisplayData()

    End Sub


    Private Sub cmdProcedure_Click(sender As System.Object, e As System.EventArgs) Handles cmdProcedure.Click
        '====================================================================================================
        SaveData()

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            Dim pTest_frmProcedure As New Test_frmProcedure(Test_frmProcedure.eType.Leak)
            pTest_frmProcedure.ShowDialog()

        Else
            If (mTestProject.Test_Spec.LeakProcedureFile <> "") Then
                gTest_Report.OpenPowerPoint(mTestProject.Test_Spec.LeakProcedureFile)
            End If
        End If

    End Sub


    Private Sub cmdProcedure_MouseHover(sender As System.Object,
                                        e As System.EventArgs) Handles cmdProcedure.MouseHover
        '=======================================================================================

        If (mTestProject.Test_Spec.LeakProcedureFile <> "") Then
            ttpToolTip1.SetToolTip(cmdProcedure, Path.GetFileName(mTestProject.Test_Spec.LeakProcedureFile))

        End If

    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        SaveData()

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            '....tblTestSpec
            mTestProject.Test_Spec.SaveTo_DB()

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then
            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLeak(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLeakData(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
                End If
            End If

        End If
        gTest_Project = mTestProject.Clone()
        Me.Close()

    End Sub


    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '==================
        '....Equip Tab
        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            mTestProject.Test_Spec.LeakMedium = cmbMedium.Text
            mTestProject.Test_Spec.LeakCavityDepth = ConvertToSng(txtTarget.Text)

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_StandName = cmbStand.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_Fixture = txtFixture.Text

                    If (txtTemp.Text = "Room") Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_T = 70
                    Else
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_T = ConvertToSng(txtTemp.Text)
                    End If

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_ShimActual = ConvertToSng(txtActual.Text)
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_ShimDescrip = txtDesc.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_PlatenSF = ConvertToInt(cmbPlatenSurfaceFinish.Text)  'AES 15MAR17

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_TestMeterMake = cmbType.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_TestMeterSN = cmbSN.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_TestMeterRange = txtRange.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_TestMeterModelNo = txtModelNo.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_TestMeterDateCalibrationDue = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakEquip.TestMeterDateCalibrationDue
                End If
            End If
        End If


        '....Seal tab
        If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    Dim pGenSeal As New Test_clsReport.sGenSeal
                    pGenSeal.SeqID = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SeqID
                    pGenSeal.SN = txtSN.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal(mCurRecNo - 1) = pGenSeal

                    'If (gTest_User.Role = clsTestUser.eRole.Tester) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_ShimActual = ConvertToSng(txtActual.Text)
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LeakPress = ConvertToSng(txtPress.Text)

                    Dim pTestSeal As New Test_clsSeal
                    pTestSeal.Leak_FHIni = ConvertToSng(txtFHIni_Measured.Text)
                    pTestSeal.Leak_Val = ConvertToSng(txtLeak_Measured.Text)
                    pTestSeal.Leak_FHFinal = ConvertToSng(txtFHFinal_Measured.Text)

                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                        pTestSeal.Leak_ODPre = ConvertToSng(txtPre_Measured.Text)
                        pTestSeal.Leak_ODPost = ConvertToSng(txtPost_Measured.Text)

                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                        pTestSeal.Leak_IDPre = ConvertToSng(txtPre_Measured.Text)
                        pTestSeal.Leak_IDPost = ConvertToSng(txtPost_Measured.Text)
                    End If

                    'AES 14NOV16
                    If (optPass.Checked) Then
                        pTestSeal.Status = Test_clsSeal.eStatus.Pass
                    ElseIf (optFail.Checked) Then
                        pTestSeal.Status = Test_clsSeal.eStatus.Fail
                    End If

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1) = pTestSeal

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_Springback = chkSpringBack.Checked

                    'End If
                End If

            End If

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Admin) Then

            If (mTestProject.Test_Spec.Leak.LeakagePlate) Then
                mTestProject.Test_Spec.SealFHIni_Plated(1) = ConvertToSng(txtFHIni_Max.Text)
                mTestProject.Test_Spec.SealFHIni_Plated(0) = ConvertToSng(txtFHIni_Min.Text)

                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    mTestProject.Test_Spec.SealODPre_Plated(1) = ConvertToSng(txtPre_Max.Text)
                    mTestProject.Test_Spec.SealODPre_Plated(0) = ConvertToSng(txtPre_Min.Text)

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    mTestProject.Test_Spec.SealIDPre_Plated(1) = ConvertToSng(txtPre_Max.Text)
                    mTestProject.Test_Spec.SealIDPre_Plated(0) = ConvertToSng(txtPre_Min.Text)

                End If
                mTestProject.Test_Spec.LeakMax_Plated = ConvertToSng(txtLeak_Max.Text)
                mTestProject.Test_Spec.LeakSpringBackMin_Plated = ConvertToSng(txtSpringBack_Min.Text)

            Else
                mTestProject.Test_Spec.SealFHIni_Unplated(1) = ConvertToSng(txtFHIni_Max.Text)
                mTestProject.Test_Spec.SealFHIni_Unplated(0) = ConvertToSng(txtFHIni_Min.Text)

                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    mTestProject.Test_Spec.SealODPre_Unplated(1) = ConvertToSng(txtPre_Max.Text)
                    mTestProject.Test_Spec.SealODPre_Unplated(0) = ConvertToSng(txtPre_Min.Text)

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    mTestProject.Test_Spec.SealIDPre_Unplated(1) = ConvertToSng(txtPre_Max.Text)
                    mTestProject.Test_Spec.SealIDPre_Unplated(0) = ConvertToSng(txtPre_Min.Text)

                End If
                mTestProject.Test_Spec.LeakMax_Unplated = ConvertToSng(txtLeak_Max.Text)
                mTestProject.Test_Spec.LeakSpringBackMin_Unplated = ConvertToSng(txtSpringBack_Min.Text)
            End If

            mTestProject.Test_Spec.LeakPress = ConvertToSng(txtPress.Text)
            mTestProject.Test_Spec.Leak_Springback = chkSpringBack.Checked

            mTestProject.Test_Spec.Result_Leak_UnitLoad = ConvertToSng(txtUnitLoad.Text)
            mTestProject.Test_Spec.Result_Leak_FHFinal = ConvertToSng(txtFHFinal_Max.Text)

        End If

        gTest_Project = mTestProject.Clone()

    End Sub

#End Region

#End Region


End Class