'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      FORM MODULE   :  frmTest_Load                           '
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

Public Class Test_frmLoad

#Region "MEMBER VARIABLES:"
    Private mTestProject As New Test_clsProject(gPartProject)
    Private mCurRecNo As Integer = 0
    Private mControl() As Control

    'Private mSealTestEntities As New SealTestDBEntities()
#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_Load_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        '===========================================================================================
        If (gIsTestLoadActive = False) Then
            mTestProject = gTest_Project.Clone()

            'mTestProject.Unit = gTest_Project.Unit.Clone()
            gIsTestLoadActive = True
        End If
    End Sub


    Private Sub frmTest_Load_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==============================================================================================
        Me.Text = "SealTest: Load Form"
        mTestProject = gTest_Project.Clone()
        mControl = {cmbStand, cmbSN, txtCalDue, cmbLoadCell, cmbLoadCell_SN, _
                  txtCavityDepth, cmbLoadType, txtFHIni_Max, txtFHIni_Measured, txtFHIni_Min, txtPre_Max, txtPre_Measured, txtPre_Min, _
                  txtLoad_Max, txtLoad_Measured, txtLoad_Min, txtFHFinal_Measured, chkSpringBack, txtSpringBack_Min, txtPost_Measured, _
                  cmdSealIPE}
        InitializeControls()
        SetUnits_Label()
        'mTestProject = gTest_Project.Clone()
        PopulateCmbBoxes()
        mTestProject.Test_Spec.RetrieveFrom_DB(mTestProject.ID)
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).RetrieveFrom_DB(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)

                'AES 14NOV16
                For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal.Count - 1
                    Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                    If (pIsLoad = True Or pIsLeak = False) Then
                        mCurRecNo = i + 1
                        Exit For
                    End If
                Next

            End If
        End If

        DisplayData()
        DetermineStatus()       'AES 29NOV16
        gIsTestMainActive = False

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

            If (pVal.Contains("Design")) Then

                Dim pTemp As Double = 0.0
                If (mControl(i).Name = "txtCavityDepth") Then
                    If (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadMin_CavityDepth)

                    ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadMax_CavityDepth)

                    ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadRange_CavityDepth)
                    End If
                End If
                ttpToolTip2.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtFHIni_Max") Then
                    If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Plated)
                    Else
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
                    End If
                End If
                ttpToolTip2.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtFHIni_Min") Then
                    If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Plated)
                    Else
                        pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)
                    End If

                End If
                ttpToolTip2.SetToolTip(mControl(i), pTemp.ToString())

                If (mControl(i).Name = "txtPre_Max") Then
                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                        If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Plated)
                        Else
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                        End If

                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                        If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Plated)
                        Else
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                        End If

                    End If
                End If
                ttpToolTip2.SetToolTip(mControl(i), pTemp.ToString())


                If (mControl(i).Name = "txtPre_Min") Then
                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                        If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Plated)
                        Else
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
                        End If

                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                        If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Plated)
                        Else
                            pTemp = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
                        End If

                    End If
                End If
                ttpToolTip2.SetToolTip(mControl(i), pTemp.ToString())

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

                    'mControl(i).Enabled = True
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

    Private Sub SetUnits_Label()
        '=======================
        lblCavityDepth_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblIniFH_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblPre_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblLoad_Unit.Text = "(" & mTestProject.Test_Unit.FUnit_PH & ")"
        lblUnitLoad_Unit.Text = "(" & mTestProject.Test_Unit.FUnit_PH & "/" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblFinalFH_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblSpringBack_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"
        lblPost_Unit.Text = "(" & mTestProject.Test_Unit.LUnit_PH & ")"

    End Sub

    Private Sub PopulateCmbBoxes()
        '=========================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Stand
        Dim pQryStand = (From pRec In pSealTestEntities.tblForceStand Select pRec.fldName Distinct).ToList()

        If (pQryStand.Count() > 0) Then
            cmbStand.Items.Clear()

            For i As Integer = 0 To pQryStand.Count() - 1
                cmbStand.Items.Add(pQryStand(i).Trim())
            Next

            cmbStand.SelectedIndex = 0
        End If

        '....LoadCell
        Dim pQryLoadCellMake = (From pRec In pSealTestEntities.tblLoadCell Select pRec.fldMake Distinct).ToList()

        If (pQryLoadCellMake.Count() > 0) Then
            cmbLoadCell.Items.Clear()

            For i As Integer = 0 To pQryLoadCellMake.Count() - 1
                cmbLoadCell.Items.Add(pQryLoadCellMake(i).Trim())
            Next

            cmbLoadCell.SelectedIndex = 0
        End If

        cmbLoadType.Text = mTestProject.Test_Spec.LoadType.ToString()

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
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.StandName <> "") Then
                    cmbStand.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.StandName

                End If

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.StandSN <> "") Then
                    cmbSN.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.StandSN
                End If

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.LoadCellMake <> "") Then
                    cmbLoadCell.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.LoadCellMake
                End If

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.LoadCellSN <> "") Then
                    cmbLoadCell_SN.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).LoadEquip.LoadCellSN
                End If
            End If
        End If

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

                txtFHIni_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.FHIni)
                txtFHFinal_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.FHFinal)

                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    txtPre_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.ODPre)
                    txtPost_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.ODPost)
                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    txtPre_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.IDPre)
                    txtPost_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.IDPost)

                End If

                txtLoad_Measured.Text = (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.Val).ToString("####.#")
                chkSpringBack.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load.Springback

                'Dim pCavityDepth As Double = 0.0
                'If (mTestProject.Test_Spec.LoadType = clsTest_Spec.eLoadType.Min) Then
                '    pCavityDepth = mTestProject.Test_Spec.LoadMin_CavityDepth

                'ElseIf (mTestProject.Test_Spec.LoadType = clsTest_Spec.eLoadType.Max) Then
                '    pCavityDepth = mTestProject.Test_Spec.LoadMax_CavityDepth

                'ElseIf (mTestProject.Test_Spec.LoadType = clsTest_Spec.eLoadType.Range) Then
                '    pCavityDepth = mTestProject.Test_Spec.LoadRange_CavityDepth
                'End If

                'Dim pCompressionVal As Double = mTestProject.Test_Spec.SealFHIni(1) - pCavityDepth
                'Dim pFHIni_Measured As Double = mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.FHIni
                'Dim pCompressionVal_Measured As Double = pFHIni_Measured - pCavityDepth

                Dim pDSealing As Double = mTestProject.Test_Spec.Result.DSealing

                If (pDSealing > gcEPS) Then
                    txtUnitLoad_Measured.Text = (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1).Load.Val / (Math.PI * pDSealing)).ToString("####.#")
                End If

                'If (pFHIni_Measured > gcEPS And mTestProject.Test_Spec.Result.Load_UnitLoad > gcEPS) Then
                '    txtUnitLoad_Measured.Text = mTestProject.Test_Unit.WriteInUserL_PH((mTestProject.Test_Spec.Result.Load_UnitLoad * pCompressionVal_Measured) / pCompressionVal)
                'Else
                '    txtUnitLoad_Measured.Text = ""
                'End If

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


        If (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadMin_CavityDepth)

        ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadMax_CavityDepth)

        ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadRange_CavityDepth)

        End If

        cmbLoadType.Text = mTestProject.Test_Spec.LoadType

        If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
            txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Plated)
            txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Plated)
        Else
            txtFHIni_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(1).Unplated)
            txtFHIni_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealFHIni(0).Unplated)
        End If


        txtUnitLoad.Text = (mTestProject.Test_Spec.Result.Load_UnitLoad).ToString("####.#")
        txtFHFinal_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.Result.Load_FHFinal)

        If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
            lblPre.Text = "OD Pre"
            lblPost.Text = "OD Post"
            If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Plated)
                txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Plated)
            Else
                txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(1).Unplated)
                txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealODPre(0).Unplated)
            End If


        ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
            lblPre.Text = "ID Pre"
            lblPost.Text = "ID Post"

            If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Plated)
                txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Plated)
            Else
                txtPre_Max.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(1).Unplated)
                txtPre_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.SealIDPre(0).Unplated)
            End If


        End If

        If (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
            txtLoad_Max.Visible = False
            txtLoad_Min.Visible = True

            txtLoad_Min.Text = mTestProject.Test_Spec.LoadVal(0).ToString("####.#")

        ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
            txtLoad_Max.Visible = True
            txtLoad_Min.Visible = False
            txtLoad_Max.Text = mTestProject.Test_Spec.LoadVal(1).ToString("####.#")

        ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
            txtLoad_Max.Visible = True
            txtLoad_Min.Visible = True
            txtLoad_Min.Text = mTestProject.Test_Spec.LoadVal(0).ToString("####.#")
            txtLoad_Max.Text = mTestProject.Test_Spec.LoadVal(1).ToString("####.#")

        End If

        If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
            txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadSpringBackMin.Plated)
        Else
            txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadSpringBackMin.Unplated)
        End If

        CalcSpringBack()

    End Sub


    Private Sub CalcSpringBack()
        '=======================
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
        Dim pLoadMax As Double = 0.0
        Dim pLoadMeasured As Double = 0.0
        Dim pLoadMin As Double = 0.0

        If (txtLoad_Max.Text <> "") Then
            pLoadMax = ConvertToSng(txtLoad_Max.Text)
        End If

        If (txtLoad_Measured.Text <> "") Then
            pLoadMeasured = ConvertToSng(txtLoad_Measured.Text)
        End If

        If (txtLoad_Min.Text <> "") Then
            pLoadMin = ConvertToSng(txtLoad_Min.Text)
        End If

        If (cmbLoadType.Text = Test_clsSpec.eLoadType.Min.ToString()) Then

            If (pLoadMeasured > pLoadMin Or Math.Abs(pLoadMeasured - pLoadMin) < gcEPS) Then
                optPass.Enabled = True
                optPass.Checked = True
                optPass.ForeColor = Color.Green

                optFail.ForeColor = Color.Black
                optFail.Enabled = False
            Else
                optPass.ForeColor = Color.Black
                optPass.Enabled = False

                optFail.Enabled = True
                optFail.ForeColor = Color.Red
                optFail.Checked = True
            End If

        ElseIf (cmbLoadType.Text = Test_clsSpec.eLoadType.Max.ToString()) Then

            If (pLoadMeasured < pLoadMax Or Math.Abs(pLoadMeasured - pLoadMax) < gcEPS) Then
                optPass.Enabled = True
                optPass.Checked = True
                optPass.ForeColor = Color.Green

                optFail.ForeColor = Color.Black
                optFail.Enabled = False
            Else
                optPass.ForeColor = Color.Black
                optPass.Enabled = False

                optFail.Enabled = True
                optFail.ForeColor = Color.Red
                optFail.Checked = True
            End If

        ElseIf (cmbLoadType.Text = Test_clsSpec.eLoadType.Range.ToString()) Then

            If ((pLoadMeasured > pLoadMin Or Math.Abs(pLoadMeasured - pLoadMin) < gcEPS) And
                (pLoadMeasured < pLoadMax Or Math.Abs(pLoadMeasured - pLoadMax) < gcEPS)) Then
                optPass.Enabled = True
                optPass.Checked = True
                optPass.ForeColor = Color.Green

                optFail.ForeColor = Color.Black
                optFail.Enabled = False
            Else
                optPass.ForeColor = Color.Black
                optPass.Enabled = False

                optFail.Enabled = True
                optFail.ForeColor = Color.Red
                optFail.Checked = True
            End If
        End If

    End Sub

#End Region


#Region "TAB CONTROL RELATED ROUTINES:"

    Private Sub TabControl1_SelectedIndexChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        '=======================================================================================================
        SaveData()
        DisplayData()

    End Sub

#End Region


#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbStand_SelectedIndexChanged(sender As System.Object,
                                              e As System.EventArgs) Handles cmbStand.SelectedIndexChanged
        '===================================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Force Stand
        Dim pQryForceStand = (From pRec In pSealTestEntities.tblForceStand Where pRec.fldName.Trim() = cmbStand.Text.Trim() Select pRec).ToList()

        If (pQryForceStand.Count() > 0) Then
            cmbSN.Items.Clear()

            For i As Integer = 0 To pQryForceStand.Count() - 1
                cmbSN.Items.Add(pQryForceStand(i).fldSN.Trim())
            Next

            cmbSN.SelectedIndex = 0
        End If
    End Sub


    Private Sub cmbSN_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbSN.SelectedIndexChanged
        '=====================================================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Force Stand
        Dim pQryForceStand = (From pRec In pSealTestEntities.tblForceStand Where pRec.fldSN.Trim() = cmbSN.Text.Trim() Select pRec).ToList()

        If (pQryForceStand.Count() > 0) Then

            For i As Integer = 0 To pQryForceStand.Count() - 1
                Dim pCalDue As DateTime = pQryForceStand(i).fldDateCalibrationDue
                txtCalDue.Text = pCalDue.ToShortDateString()
            Next

        End If
    End Sub

    Private Sub cmbLoadCell_SelectedIndexChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles cmbLoadCell.SelectedIndexChanged
        '========================================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Load Cell
        Dim pQryLoadCell = (From pRec In pSealTestEntities.tblLoadCell Where pRec.fldMake.Trim() = cmbLoadCell.Text.Trim() Select pRec).ToList()

        If (pQryLoadCell.Count() > 0) Then
            cmbLoadCell_SN.Items.Clear()

            For i As Integer = 0 To pQryLoadCell.Count() - 1
                cmbLoadCell_SN.Items.Add(pQryLoadCell(i).fldSN.Trim())
            Next

            cmbLoadCell_SN.SelectedIndex = 0
        End If
    End Sub


    Private Sub cmbLoadCell_SN_SelectedIndexChanged(sender As System.Object,
                                                    e As System.EventArgs) Handles cmbLoadCell_SN.SelectedIndexChanged
        '==============================================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....Load Cell
        Dim pQryLoadCell = (From pRec In pSealTestEntities.tblLoadCell Where pRec.fldSN.Trim() = cmbLoadCell_SN.Text.Trim() Select pRec).ToList()

        If (pQryLoadCell.Count() > 0) Then

            For i As Integer = 0 To pQryLoadCell.Count() - 1

                Dim pRange As String = pQryLoadCell(i).fldRange.Trim()
                txtLoadCellRange.Text = pRange
                txtModelNo.Text = pQryLoadCell(i).fldModelNo.Trim()
                Dim pCalDue As DateTime = pQryLoadCell(i).fldDateCalibrationDue
                txtLoadCell_CalDue.Text = pCalDue.ToShortDateString()
            Next

        End If
    End Sub


    Private Sub cmbLoadType_SelectedIndexChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles cmbLoadType.SelectedIndexChanged
        '=========================================================================================================

        If (cmbLoadType.Text = Test_clsSpec.eLoadType.Min.ToString()) Then
            txtLoad_Max.Visible = False
            txtLoad_Min.Visible = True
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadMin_CavityDepth)

        ElseIf (cmbLoadType.Text = Test_clsSpec.eLoadType.Max.ToString()) Then
            txtLoad_Max.Visible = True
            txtLoad_Min.Visible = False
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadMax_CavityDepth)

        ElseIf (cmbLoadType.Text = Test_clsSpec.eLoadType.Range.ToString()) Then
            txtLoad_Min.Visible = True
            txtLoad_Max.Visible = True
            txtCavityDepth.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LoadRange_CavityDepth)
        End If

        DetermineStatus()

    End Sub


#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtBox_TextChanged(sender As System.Object,
                                   e As System.EventArgs) Handles txtCavityDepth.TextChanged, txtFHIni_Max.TextChanged,
                                                               txtFHIni_Min.TextChanged, txtPre_Max.TextChanged,
                                                               txtPre_Min.TextChanged
        '===============================================================================================================
        Dim ptxtBox As TextBox = CType(sender, TextBox)

        Select Case ptxtBox.Name

            Case "txtCavityDepth"
                Dim pCavityDepth As Double
                If (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
                    pCavityDepth = mTestProject.Test_Spec.LoadMin_CavityDepth

                ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
                    pCavityDepth = mTestProject.Test_Spec.LoadMax_CavityDepth

                ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
                    pCavityDepth = mTestProject.Test_Spec.LoadRange_CavityDepth
                End If

                SetForeColor(ptxtBox, pCavityDepth)

            Case "txtFHIni_Max"
                Dim pVal As Double = 0.0

                If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                    pVal = mTestProject.Test_Spec.SealFHIni(1).Plated
                Else
                    pVal = mTestProject.Test_Spec.SealFHIni(1).Unplated
                End If

                SetForeColor(ptxtBox, pVal)

            Case "txtFHIni_Min"
                Dim pVal As Double = 0.0
                If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                    pVal = mTestProject.Test_Spec.SealFHIni(0).Plated
                Else
                    pVal = mTestProject.Test_Spec.SealFHIni(0).Unplated
                End If

                SetForeColor(ptxtBox, pVal)

            Case "txtPre_Max"
                Dim pMaxVal As Double
                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                        pMaxVal = mTestProject.Test_Spec.SealODPre(1).Plated
                    Else
                        pMaxVal = mTestProject.Test_Spec.SealODPre(1).Unplated
                    End If

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                        pMaxVal = mTestProject.Test_Spec.SealIDPre(1).Plated
                    Else
                        pMaxVal = mTestProject.Test_Spec.SealIDPre(1).Unplated
                    End If

                End If
                SetForeColor(ptxtBox, pMaxVal)

            Case "txtPre_Min"
                Dim pMinVal As Double
                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                        pMinVal = mTestProject.Test_Spec.SealODPre(0).Plated
                    Else
                        pMinVal = mTestProject.Test_Spec.SealODPre(0).Unplated
                    End If

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                        pMinVal = mTestProject.Test_Spec.SealIDPre(0).Plated
                    Else
                        pMinVal = mTestProject.Test_Spec.SealIDPre(0).Unplated
                    End If

                End If
                SetForeColor(ptxtBox, pMinVal)

        End Select

    End Sub


    Private Sub SetForeColor(ByVal TxtBox_In As TextBox, ByVal Val_Org_In As Double)
        '===========================================================================
        If (Math.Abs(ConvertToSng(TxtBox_In.Text) - Val_Org_In) < gcEPS) Then
            TxtBox_In.ForeColor = Color.Blue
        Else
            TxtBox_In.ForeColor = Color.Green
        End If
    End Sub


    Private Sub ttpToolTip2_Draw(sender As System.Object,
                                 e As System.Windows.Forms.DrawToolTipEventArgs) Handles ttpToolTip2.Draw
        '==================================================================================================
        e.DrawBackground()
        e.DrawBorder()
        e.DrawText()
    End Sub


    Private Sub txtLoad_Max_Validated(sender As System.Object,
                                      e As System.EventArgs) Handles txtLoad_Min.Validated,
                                      txtLoad_Measured.Validated, txtLoad_Max.Validated
        '==================================================================================
        SaveData()
        DetermineStatus()
        'DisplayData()      'AES 20FEB17

    End Sub


    Private Sub txtFHFinal_Measured_Validated(sender As System.Object,
                                              e As System.EventArgs) Handles txtFHFinal_Measured.Validated
        '=================================================================================================

        CalcSpringBack()
    End Sub


    Private Sub txtFHIni_Measured_Click(sender As System.Object, e As System.EventArgs) Handles txtFHIni_Measured.Click
        '==============================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Ini", , Me)
        pTest_frmMeasure_FH.ShowDialog()
    End Sub


    Private Sub txtFHIni_Measured_KeyDown(sender As System.Object,
                                          e As System.Windows.Forms.KeyEventArgs) Handles txtFHIni_Measured.KeyDown
        '===========================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Ini", , Me)
        pTest_frmMeasure_FH.ShowDialog()
    End Sub


    Private Sub txtFHFinal_Measured_Click(sender As System.Object, e As System.EventArgs) Handles txtFHFinal_Measured.Click
        '===================================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Final", , Me)
        pTest_frmMeasure_FH.ShowDialog()
    End Sub


    Private Sub txtFHFinal_Measured_KeyDown(sender As System.Object,
                                            e As System.Windows.Forms.KeyEventArgs) Handles txtFHFinal_Measured.KeyDown
        '===============================================================================================================
        Dim pTest_frmMeasure_FH As New Test_frmMeasure_FH("Final", , Me)
        pTest_frmMeasure_FH.ShowDialog()
    End Sub

    Private Sub txtFHFinal_Max_Validated(sender As System.Object, e As System.EventArgs) _
                                        Handles txtFHFinal_Max.Validated
        '==================================================================================
        CalcSpringBack()
    End Sub

#End Region


#Region "CHECK BOX RELATED ROUTINE:"

    Private Sub chkSpringBack_CheckedChanged(sender As System.Object,
                                             e As System.EventArgs) Handles chkSpringBack.CheckedChanged
        '===============================================================================================
        If (chkSpringBack.Checked) Then
            If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then
                txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Plated)
            Else
                txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(mTestProject.Test_Spec.LeakSpringBackMin.Unplated)
            End If

        Else
            txtSpringBack_Min.Text = mTestProject.Test_Unit.WriteInUserL_PH(0.0)
        End If
    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdProcedure_Click(sender As System.Object, e As System.EventArgs) Handles cmdProcedure.Click
        '=====================================================================================================
        SaveData()

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            Dim pTest_frmProcedure As New Test_frmProcedure(Test_frmProcedure.eType.Load)
            pTest_frmProcedure.ShowDialog()

        Else
            If (mTestProject.Test_Spec.LoadProcedureFile <> "") Then
                gTest_Report.OpenPowerPoint(mTestProject.Test_Spec.LoadProcedureFile)
            End If
        End If

    End Sub

    Private Sub cmdProcedure_MouseHover(sender As System.Object,
                                        e As System.EventArgs) Handles cmdProcedure.MouseHover
        '=======================================================================================
        'Dim pToolTip As New ToolTip()

        'If (mTestProject.Spec.LoadProcedureFile <> "") Then
        '    pToolTip.SetToolTip(cmdProcedure, Path.GetFileName(mTestProject.Spec.LoadProcedureFile))

        'End If

        If (mTestProject.Test_Spec.LoadProcedureFile <> "") Then
            Dim pFileTitle As String = Path.GetFileName(mTestProject.Test_Spec.LoadProcedureFile.Trim())
            ttpToolTip2.SetToolTip(cmdProcedure, pFileTitle)

        End If

    End Sub


    Private Sub cmdMoveFirst_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveFirst.Click
        '====================================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pTotalRec As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty
                Dim pRecNo As Integer = 0
                If (pTotalRec > 0) Then
                    'pRecNo = 1
                    'AES 14NOV16
                    For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty - 1

                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLoad = True Or pIsLeak = False) Then
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


    Private Sub cmdMoveLast_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveLast.Click
        '==================================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pTotalRec As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty
                Dim pRecNo As Integer = 0
                If (pTotalRec > 0) Then
                    'pRecNo = pTotalRec
                    'AES 14NOV16
                    For i As Integer = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty - 1 To 0 Step -1

                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLoad = True Or pIsLeak = False) Then
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
                    'txtRecNo.Text = (pCurRec - 1).ToString()
                    'mCurRecNo = ConvertToInt(txtRecNo.Text)
                    'AES 14NOV16
                    For i As Integer = pCurRecNo - 2 To 0 Step -1
                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLoad = True Or pIsLeak = False) Then
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


    Private Sub cmdMoveNext_Click(sender As System.Object, e As System.EventArgs) Handles cmdMoveNext.Click
        '==================================================================================================
        SaveData()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pCurRecNo As Integer = ConvertToInt(txtRecNo.Text)

                If (pCurRecNo < mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty) Then
                    'txtRecNo.Text = (pCurRec + 1).ToString()
                    'mCurRecNo = ConvertToInt(txtRecNo.Text)
                    'AES 14NOV16
                    For i As Integer = pCurRecNo To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty - 1

                        Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                        If (pIsLoad = True Or pIsLeak = False) Then
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


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        SaveData()
        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            '....tblTestSpec
            mTestProject.Test_Spec.SaveTo_DB()

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then
            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLoad(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLoadData(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
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

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                '....Equip Tab
                If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_StandName = cmbStand.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_SN = cmbSN.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_StandDateCalibrationDue = txtCalDue.Text

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_LoadCellMake = cmbLoadCell.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_LoadCellSN = cmbLoadCell_SN.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_LoadCellRange = txtLoadCellRange.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_LoadCellModelNo = txtModelNo.Text
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_LoadCellDateCalibrationDue = txtLoadCell_CalDue.Text
                End If

                'AES 16NOV16
                ''Dim pGenSeal As New clsTestReport.sGenSeal
                ''pGenSeal.SeqID = mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Seal(mCurRecNo - 1).SeqID
                ''pGenSeal.SN = txtSN.Text
                ''mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Seal(mCurRecNo - 1) = pGenSeal

                '....Seal tab
                If (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                    'If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then       'AES 16NOV16


                    Dim pTestSeal As New Test_clsSeal
                    pTestSeal.Load_FHIni = ConvertToSng(txtFHIni_Measured.Text)
                    pTestSeal.Load_Val = ConvertToSng(txtLoad_Measured.Text)
                    pTestSeal.Load_FHFinal = ConvertToSng(txtFHFinal_Measured.Text)

                    If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                        pTestSeal.Load_ODPre = ConvertToSng(txtPre_Measured.Text)
                        pTestSeal.Load_ODPost = ConvertToSng(txtPost_Measured.Text)

                    ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                        pTestSeal.Load_IDPre = ConvertToSng(txtPre_Measured.Text)
                        pTestSeal.Load_IDPost = ConvertToSng(txtPost_Measured.Text)
                    End If

                    'AES 14NOV16
                    If (optPass.Checked) Then
                        pTestSeal.Status = Test_clsSeal.eStatus.Pass
                    ElseIf (optFail.Checked) Then
                        pTestSeal.Status = Test_clsSeal.eStatus.Fail
                    End If

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal(mCurRecNo - 1) = pTestSeal

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_Springback = chkSpringBack.Checked

                    'End If

                End If

            End If
        End If

        mTestProject.Test_Spec.LoadType = DirectCast([Enum].Parse(GetType(Test_clsSpec.eLoadType), cmbLoadType.Text), Test_clsSpec.eLoadType) 'cmbLoadType.Text

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            If (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
                mTestProject.Test_Spec.LoadMin_CavityDepth = ConvertToSng(txtCavityDepth.Text)
                mTestProject.Test_Spec.LoadVal(0) = ConvertToSng(txtLoad_Min.Text)

            ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
                mTestProject.Test_Spec.LoadMax_CavityDepth = ConvertToSng(txtCavityDepth.Text)
                mTestProject.Test_Spec.LoadVal(1) = ConvertToSng(txtLoad_Max.Text)

            ElseIf (mTestProject.Test_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
                mTestProject.Test_Spec.LoadRange_CavityDepth = ConvertToSng(txtCavityDepth.Text)
                mTestProject.Test_Spec.LoadVal(0) = ConvertToSng(txtLoad_Min.Text)
                mTestProject.Test_Spec.LoadVal(1) = ConvertToSng(txtLoad_Max.Text)

            End If

            If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then

                mTestProject.Test_Spec.SealFHIni_Plated(1) = ConvertToSng(txtFHIni_Max.Text)
                mTestProject.Test_Spec.SealFHIni_Plated(0) = ConvertToSng(txtFHIni_Min.Text)

                If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                    mTestProject.Test_Spec.SealODPre_Plated(1) = ConvertToSng(txtPre_Max.Text)
                    mTestProject.Test_Spec.SealODPre_Plated(0) = ConvertToSng(txtPre_Min.Text)

                ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                    mTestProject.Test_Spec.SealIDPre_Plated(1) = ConvertToSng(txtPre_Max.Text)
                    mTestProject.Test_Spec.SealIDPre_Plated(0) = ConvertToSng(txtPre_Min.Text)

                End If
                mTestProject.Test_Spec.LoadSpringBackMin_Plated = ConvertToSng(txtSpringBack_Min.Text)

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
                mTestProject.Test_Spec.LoadSpringBackMin_Unplated = ConvertToSng(txtSpringBack_Min.Text)
            End If


            mTestProject.Test_Spec.Load_Springback = chkSpringBack.Checked
            mTestProject.Test_Spec.Result_Load_UnitLoad = ConvertToSng(txtUnitLoad.Text)
            mTestProject.Test_Spec.Result_Load_FHFinal = ConvertToSng(txtFHFinal_Max.Text)

        End If

        gTest_Project = mTestProject.Clone()

    End Sub

#End Region

#End Region


    Private Sub cmdSealIPE_Click(sender As System.Object, e As System.EventArgs) Handles cmdSealIPE.Click
        '=================================================================================================
        Cursor = Cursors.WaitCursor
        SaveData()
        mTestProject.RunANSYS_FEA("Load", gIPE_ANSYS)
        If mTestProject.Analysis.Result_Gen.SolnConv = 1 Then
            mTestProject.Test_Spec.Result_DSealing = mTestProject.Analysis.Seal.DSealing
            mTestProject.Test_Spec.Result_Load_UnitLoad = mTestProject.Analysis.Seal.FSeat_Unit(1)
            mTestProject.Test_Spec.Result_Load_FHFinal = mTestProject.Analysis.Seal.HfreeFinal

            '....tblTestSpec
            mTestProject.Test_Spec.SaveTo_DB()
        End If

        DisplayData()
        Cursor = Cursors.Default

    End Sub

End Class