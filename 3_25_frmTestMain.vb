'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      FORM MODULE   :  frmMain                                '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07JUL17                                '
'                                                                              '
'===============================================================================
'
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.IO
Imports System.Reflection

Public Class Test_frmMain

#Region "MEMBER VARIABLES:"

    ''Private mProjectEntities As New ProjectDBEntities()
    Private mSealTestEntities As New SealTestDBEntities()

    Private mPNID As Integer
    Private mRevID As Integer
    Private mCustomerID As Integer
    Private mLocationID As Integer
    Private mPlatformID As Integer

    Private mProjectID As Integer
    Private mSealID As Integer

    '....Local Object. 
    Private mTestProject As Test_clsProject
    Private mUnit As clsUnit

    Private mPN As String
    Private mPNRev As String

    Private mMO_Sel As Integer
    Private mReport_Sel As Integer

    Private mControl() As Control
    'Private mTesterClicked As Boolean = False

#End Region


#Region "PROPERTY ROUTINES:"
    '======================

    '....MO_Sel
    Public Property MO_Sel() As Integer
        '=============================
        Get
            Return mMO_Sel
        End Get

        Set(ByVal value As Integer)
            mMO_Sel = value
        End Set
    End Property

    '....Report_Sel
    Public Property Report_Sel() As Integer
        '===================================
        Get
            Return mReport_Sel
        End Get

        Set(ByVal value As Integer)
            mReport_Sel = value
        End Set
    End Property

#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_Main_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        '===========================================================================================

        If (gIsTestMainActive = False) Then
            mTestProject = gTest_Project.Clone()
            gIsTestMainActive = True
            DisplayData()
        End If

    End Sub


    Private Sub frmTest_Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        '==================================================================================
        '   Initialize Controls:  
        '   --------------------
        '
        Me.Text = "SealTest: Main Form"

        '   Menu Controls:
        '
        '   
        'If (gTest_User.IsAdminExists()) Then
        '    If (gTest_User.Role = clsTestUser.eRole.Admin) Then
        '        mnuUserInfo.Enabled = True
        '    Else
        '        mnuUserInfo.Enabled = False
        '    End If

        'Else
        '    mnuUserInfo.Enabled = True
        'End If

        'AES 14NOV16
        'If (gTest_User.Role <> clsTestUser.eRole.Viewer) Then
        '    mnuUserInfo.Enabled = True
        'Else
        '    mnuUserInfo.Enabled = False
        'End If

        'AES 21NOV16
        mnuUserInfo.Enabled = True


        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            mnuUpdEquipList.Enabled = True
        Else
            mnuUpdEquipList.Enabled = False
        End If

        mControl = {chkSignOff, cmdSealIPEData, cmdSetUnits, cmdMO_New, cmdMO_Delete, cmdReport_New, cmdReport_Delete,
                    updSealQty, chkLeakage, chkLoad, chkPressure, txtDate_Tester, dtpTester, chkTester_Signed,
                    txtDate_Engg, dtpEngg, chkEngg_Signed, txtDate_Quality, dtpQuality,
                    chkQuality_Signed, cmdNotes, cmdReports, txtMO, txtReport, cmdMO_Save, cmdReport_Save, cmdTester_Sign, cmdEnggMgr_Sign, cmdQualityMgr_Sign}


        InitializeControls()        'Initialize_FormControls

        gTest_Test.RetrieveFrom_DB()

        PopulateParkerPN_Rev()

        'gIsTestMainActive = True

        '....Status Bar Panels:
        Dim pWidth As Int32 = (SBar1.Width) / 4

        SBpanel1.Width = pWidth + 40
        SBpanel1.Text = gTest_User.Name & " (" & gTest_User.SystemLogin & ")"

        SBPanel3.Width = pWidth - 70

        If (gTest_User.Role = Test_clsUser.eRole.Eng) Then
            SBPanel3.Text = "Eng."
        Else
            SBPanel3.Text = gTest_User.Role.ToString()
        End If

        SBPanel2.Width = pWidth + 50
        SBPanel4.Width = pWidth - 40

        Dim pCI As New CultureInfo("en-US")
        'SBPanel4.Text = Today.DayOfWeek.ToString() & ", " & _
        '                Today.ToString(" MMMM dd, yyyy", pCI.DateTimeFormat()) 'US Format only
        SBPanel4.Text = Today.ToString(" MMMM dd, yyyy", pCI.DateTimeFormat()) 'US Format only

        gIsTestMainActive = True

    End Sub

    'Private Sub GetPartProjectInfo()
    '    '===========================

    '    Dim pPartEntities As New SealPartDBEntities
    '    Dim pQryProject = (From it In pPartEntities.tblProject
    '                           Where it.fldID = gPartProject.Project_ID Select it).ToList()

    '    If (pQryProject.Count() > 0) Then
    '        mPNID = pQryProject(0).fldPNID
    '        mRevID = pQryProject(0).fldRevID
    '        mCustID = pQryProject(0).fldCustID
    '    End If
    'End Sub

    Private Sub GetPartProjectInfo(ByVal PNID_In As Integer, ByVal RevID_In As Integer, ByVal ProjectID_In As Integer)
        '=============================================================================================================
        Dim pPartEntities As New SealPartDBEntities()
        Dim pQryProject = (From it In pPartEntities.tblProject
                           Where it.fldID = ProjectID_In Select it).ToList()

        If (pQryProject.Count() > 0) Then
            mPNID = pQryProject(0).fldPNID
            mRevID = pQryProject(0).fldRevID
            mCustomerID = pQryProject(0).fldCustID
            mPlatformID = pQryProject(0).fldPlatformID
            mLocationID = pQryProject(0).fldLocID

            mTestProject.PartProject.CustInfo.PN_Cust = pQryProject(0).fldPN_Cust
            mTestProject.PartProject.CustInfo.PN_Cust_Rev = pQryProject(0).fldPN_Cust_Rev
        Else
            mPNID = PNID_In
            mRevID = RevID_In
            mCustomerID = 0
            mPlatformID = 0
            mLocationID = 0
        End If

        RetrieveFromDB_PartProject()

    End Sub

    Private Sub RetrieveFromDB_PartProject()
        '===================================
        Try

            Dim pPartEntities As New SealPartDBEntities()

            '....Customer
            Dim pQryCust = (From it In pPartEntities.tblCustomer
                            Where it.fldID = mCustomerID Select it).ToList()
            If (pQryCust.Count() > 0) Then
                mTestProject.PartProject.CustInfo.CustName = pQryCust(0).fldName
                Dim pUnit As String = pQryCust(0).fldDimUnit.Trim()
                If (pUnit = "Metric") Then
                    mTestProject.PartProject.PNR.UnitSystem = clsPartProject.clsPNR.eDimUnit.Metric
                Else
                    mTestProject.PartProject.PNR.UnitSystem = clsPartProject.clsPNR.eDimUnit.English
                End If

            End If

            '....Platform
            Dim pQryPlat = (From it In pPartEntities.tblPlatform
                            Where it.fldCustID = mCustomerID And it.fldID = mPlatformID Select it).ToList()

            If (pQryPlat.Count() > 0) Then
                mTestProject.PartProject.CustInfo.PlatName = pQryPlat(0).fldName
            End If

            '....Location
            Dim pQryLoc = (From it In pPartEntities.tblLocation
                           Where it.fldCustID = mCustomerID And it.fldPlatformID = mPlatformID And it.fldID = mLocationID Select it).ToList()

            If (pQryLoc.Count() > 0) Then
                mTestProject.PartProject.CustInfo.LocName = pQryLoc(0).fldLoc
            End If

            '....PN
            Dim pQryPN = (From it In pPartEntities.tblPN
                          Where it.fldID = mPNID Select it).ToList()
            If (pQryPN.Count() > 0) Then
                mTestProject.PartProject.PNR.Current_Exists = pQryPN(0).fldCurrentExists

                If (mTestProject.PartProject.PNR.Current.Exists) Then
                    If (Not IsDBNull(pQryPN(0).fldCurrent) And Not IsNothing(pQryPN(0).fldCurrent)) Then
                        Dim pPN As String = pQryPN(0).fldCurrent

                        If (pPN <> "") Then
                            Dim pParkerPN_Prefix As String = pPN.Substring(3, 2)
                            Dim pParkerPN_No As String = pPN.Substring(5)
                            mTestProject.PartProject.PNR.Current_Val = pParkerPN_No
                            mTestProject.PartProject.PNR.Current_TypeNo = pParkerPN_Prefix
                        End If

                    End If
                End If

                Dim pLegacyType As Integer = -1
                If (Not IsDBNull(pQryPN(0).fldLegacyType) And Not IsNothing(pQryPN(0).fldLegacyType)) Then
                    pLegacyType = pQryPN(0).fldLegacyType

                    If (pLegacyType = -1) Then
                        mTestProject.PartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.None

                    ElseIf (pLegacyType = 0) Then
                        mTestProject.PartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued

                    ElseIf (pLegacyType = 1) Then
                        mTestProject.PartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Other
                    End If
                End If

                If (Not IsDBNull(pQryPN(0).fldLegacyExists) And Not IsNothing(pQryPN(0).fldLegacyExists)) Then
                    mTestProject.PartProject.PNR.Legacy_Exists = pQryPN(0).fldLegacyExists
                End If

                If (Not IsDBNull(pQryPN(0).fldLegacy) And Not IsNothing(pQryPN(0).fldLegacy)) Then
                    mTestProject.PartProject.PNR.Legacy_Val = pQryPN(0).fldLegacy
                End If

                If (Not IsDBNull(pQryPN(0).fldDimUnit) And Not IsNothing(pQryPN(0).fldDimUnit)) Then
                    Dim pUnit As String = pQryPN(0).fldDimUnit.Trim()
                    If (pUnit = "Metric") Then
                        mTestProject.PartProject.PNR.UnitSystem = clsPartProject.clsPNR.eDimUnit.Metric
                    Else
                        mTestProject.PartProject.PNR.UnitSystem = clsPartProject.clsPNR.eDimUnit.English
                    End If
                End If

            End If

            '....Rev
            Dim pQryRev = (From it In pPartEntities.tblRev
                           Where it.fldPNID = mPNID And it.fldID = mRevID Select it).ToList()
            If (pQryRev.Count() > 0) Then
                If (Not IsDBNull(pQryRev(0).fldCurrent) And Not IsNothing(pQryRev(0).fldCurrent)) Then
                    mTestProject.PartProject.PNR.Current_Rev = pQryRev(0).fldCurrent
                End If

                If (Not IsDBNull(pQryRev(0).fldLegacy) And Not IsNothing(pQryRev(0).fldLegacy)) Then
                    mTestProject.PartProject.PNR.Legacy_Rev = pQryRev(0).fldLegacy
                End If
            End If

            '....HW
            'mTestProject.PartProject.PNR.HW.InitializePNR(mTestProject.PartProject.PNR)

            Dim pSealPartFile As New clsPartFile()
            Dim pMatList_Prop As New List(Of String)
            pMatList_Prop = pSealPartFile.MatList_Prop

            '....HW_Face table
            Dim pHWFace_Rec_Count As Integer = (From HWFace In pPartEntities.tblHW_Face
                                                Where HWFace.fldPNID = mPNID And
                                            HWFace.fldRevID = mRevID Select HWFace).Count()

            If (pHWFace_Rec_Count > 0) Then

                Dim pHWFace_Rec = (From HWFace In pPartEntities.tblHW_Face
                                   Where HWFace.fldPNID = mPNID And
                                            HWFace.fldRevID = mRevID Select HWFace).First()

                Dim pType As String = pHWFace_Rec.fldType.ToString().Trim()
                mTestProject.PartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), pType), clsPartProject.clsPNR.eType)
                mTestProject.PartProject.PNR.HW.InitializePNR(mTestProject.PartProject.PNR)

                With mTestProject.PartProject.PNR.HW
                    .POrient = pHWFace_Rec.fldPOrient
                    .MCrossSecNo = pHWFace_Rec.fldMCS
                    If (Not IsDBNull(pHWFace_Rec.fldGeomTemplate) And Not IsNothing(pHWFace_Rec.fldGeomTemplate)) Then
                        .GeomTemplate = pHWFace_Rec.fldGeomTemplate
                    End If

                    .IsSegmented = pHWFace_Rec.fldSegmented
                    .CountSegment = pHWFace_Rec.fldSegmentCount
                    .MatName = pHWFace_Rec.fldMatName
                    .Adjusted = pHWFace_Rec.fldAdjusted

                    If (Not IsDBNull(pHWFace_Rec.fldHT)) Then
                        .HT = pHWFace_Rec.fldHT
                    Else
                        .HT = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldTemper)) Then
                        .Temper = pHWFace_Rec.fldTemper
                    Else
                        .Temper = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldCoating)) Then
                        .Coating = pHWFace_Rec.fldCoating
                    Else
                        .Coating = "None"
                    End If

                    If (.Coating = "T800") Then
                        .SFinish = pHWFace_Rec.fldSFinish
                    Else
                        .SFinish = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldIsPlating)) Then
                        .PlatingExists = pHWFace_Rec.fldIsPlating
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldPlatingCode)) Then
                        .PlatingCode = pHWFace_Rec.fldPlatingCode
                    Else
                        .PlatingCode = ""
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldPlatingThickCode)) Then
                        .PlatingThickCode = pHWFace_Rec.fldPlatingThickCode
                    Else
                        .PlatingThickCode = ""
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldHfreeStd)) Then
                        .Hfree = pHWFace_Rec.fldHfreeStd
                    Else
                        .Hfree = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldHFreeTol1)) Then
                        .HFreeTol(1) = pHWFace_Rec.fldHFreeTol1
                    Else
                        .HFreeTol(1) = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldHFreeTol2)) Then
                        .HFreeTol(2) = pHWFace_Rec.fldHFreeTol2
                    Else
                        .HFreeTol(2) = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldDControl)) Then
                        .DControl = pHWFace_Rec.fldDControl
                    Else
                        .DControl = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldH11Tol)) Then
                        .H11Tol = pHWFace_Rec.fldH11Tol
                    Else
                        .H11Tol = 0
                    End If

                    If (mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                        If (.Adjusted) Then
                            '....HW_AdjCSeal table
                            Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
                                                                     Where HWFace_AdjCSeal.fldPNID = mPNID And
                                                                HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).Count()
                            If (pHW_AdjCSeal_Rec_Count > 0) Then

                                Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
                                                            Where HWFace_AdjCSeal.fldPNID = mPNID And
                                                                HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).First()

                                With mTestProject.PartProject.PNR.HW
                                    If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDHFree)) Then
                                        .DHfree = pHWFace_AdjCSeal_Rec.fldDHFree
                                    Else
                                        .DHfree = 0.0#
                                    End If

                                    If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDThetaOpening)) Then
                                        .DThetaOpening = pHWFace_AdjCSeal_Rec.fldDThetaOpening
                                    Else
                                        .DThetaOpening = 0.0#
                                    End If

                                    If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDT)) Then
                                        .T = pHWFace_AdjCSeal_Rec.fldDT
                                    Else
                                        .T = 0.0#
                                    End If

                                End With

                            End If

                        End If

                    ElseIf (mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                        If (.Adjusted) Then
                            '....HW_AdjESeal table
                            Dim pHW_AdjESeal_Rec_Count As Integer = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
                                                                     Where HWFace_AdjESeal.fldPNID = mPNID And
                                                                HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).Count()
                            If (pHW_AdjESeal_Rec_Count > 0) Then

                                Dim pHWFace_AdjESeal_Rec = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
                                                            Where HWFace_AdjESeal.fldPNID = mPNID And
                                                                HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).First()

                                With mTestProject.PartProject.PNR.HW
                                    If (Not IsNothing(pHWFace_AdjESeal_Rec.fldDThetaE1)) Then
                                        .DThetaE1 = pHWFace_AdjESeal_Rec.fldDThetaE1
                                    Else
                                        .DThetaE1 = 0.0#
                                    End If

                                    If (Not IsNothing(pHWFace_AdjESeal_Rec.fldDThetaM1)) Then
                                        .DThetaM1 = pHWFace_AdjESeal_Rec.fldDThetaM1
                                    Else
                                        .DThetaM1 = 0.0#
                                    End If

                                End With

                            End If

                        End If
                    End If

                    If (mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.U) Then
                        Dim pSealEntities As New SealIPEMCSDBEntities()
                        Dim pRecord = (From pRec In pSealEntities.tblUSeal_Geom
                                       Where pRec.fldCrossSecNo = .MCrossSecNo Select pRec).ToList()
                        If (pRecord.Count > 0) Then
                            If (pRecord(0).fldGeomTemplate = False) Then
                                mTestProject.SealIPE_FEA = False
                            Else
                                mTestProject.SealIPE_FEA = True
                            End If
                        Else
                            mTestProject.SealIPE_FEA = True
                        End If
                    Else
                        mTestProject.SealIPE_FEA = True
                    End If

                    Dim pIsMatExists As Boolean = False
                    For i As Integer = 0 To pMatList_Prop.Count - 1
                        If (.MatName = pMatList_Prop(i)) Then
                            pIsMatExists = True
                            Exit For
                        End If
                    Next

                    If (pIsMatExists = False) Then
                        mTestProject.SealIPE_FEA = False
                    End If

                    mTestProject.Test_Spec = New Test_clsSpec(mTestProject)
                    mTestProject.Set_SpecData_DefVal()
                    mTestProject.Test_Spec.RetrieveFrom_DB(mTestProject.ID)

                End With
            End If

        Catch ex As Exception

        End Try

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '===============================
        '....Instantiate Local Objects. 
        mUnit = New clsUnit()

    End Sub


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

                If (mControl(i).Name = "cmdSealIPEData") Then
                    mControl(i).Enabled = False     'For 2.0 SealTest   AES 27APR17
                Else
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (pVal.Contains("ADef") Or pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Eng) Then

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Quality) Then

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Viewer) Then        'AES 02MAR17

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            End If
        Next

        chkSignOff.Checked = False
        chkSignOff.Text = "Sign-off"

        txtDateOpen.Text = ""

        cmdMO_Save.Visible = False
        cmdMO_Delete.Visible = True

        cmdReport_Save.Visible = False
        cmdReport_Delete.Visible = True

        txtMO.Visible = False
        cmbMO.Visible = True            'AES 29JUN17
        txtReport.Visible = False
        cmbReportNo.Visible = True      'AES 29JUN17

        '....Seal
        cmbSealType.Text = ""
        cmbPOrient.Text = ""
        txtMCS.Text = ""
        lblStandard.Text = ""
        updSealQty.Value = 1
        cmbMatName.Text = ""
        cmbSFinish.Text = ""
        cmbPlatingCode.Text = ""
        cmbPlatingThickCode.Text = ""
        txtPlatingThick_Min.Text = ""
        txtPlatingThick_Max.Text = ""

        'cmbName_Tester.Text = ""
        txtName_Tester.Text = ""
        txtDate_Tester.Text = ""

        txtName_Engg.Text = ""
        txtDate_Engg.Text = ""

        chkEngg_Signed.Checked = False
        cmdEnggMgr_Sign.Text = "Sign"

        txtName_Quality.Text = ""
        txtDate_Quality.Text = ""
        chkQuality_Signed.Checked = False
        cmdQualityMgr_Sign.Text = "Sign"

        chkTester_Signed.Checked = False
        cmdTester_Sign.Text = "Sign"

        GetRoleName()

        lblStatus.Visible = False

    End Sub


    Public Sub GetRoleName()
        '======================
        Dim pLoginName As String = ""
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pQry = (From pRec In pSealTestEntities.tblReport
                                Where pRec.fldTestProjectID = gTest_Project.ProjectID Select pRec).ToList()


        If (pQry.Count() > 0) Then
            If (Not IsDBNull(pQry(0).fldUserTester)) Then
                txtName_Tester.Text = pQry(0).fldUserTester
            Else
                If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
                    txtName_Tester.Text = gTest_User.Name
                Else
                    txtName_Tester.Text = ""
                End If
            End If

            If (Not IsDBNull(pQry(0).fldUserEngg)) Then
                txtName_Engg.Text = pQry(0).fldUserEngg
            Else
                If (gTest_User.Role = Test_clsUser.eRole.Eng) Then
                    txtName_Engg.Text = gTest_User.Name
                Else
                    txtName_Engg.Text = ""
                End If

            End If

            If (Not IsDBNull(pQry(0).fldUserQuality)) Then
                txtName_Quality.Text = pQry(0).fldUserQuality
            Else
                If (gTest_User.Role = Test_clsUser.eRole.Quality) Then
                    txtName_Quality.Text = gTest_User.Name
                Else
                    txtName_Quality.Text = ""
                End If
            End If

        Else

            If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
                txtName_Tester.Text = gTest_User.Name
            Else
                txtName_Tester.Text = ""
            End If

            If (gTest_User.Role = Test_clsUser.eRole.Eng) Then
                txtName_Engg.Text = gTest_User.Name
            Else
                txtName_Engg.Text = ""
            End If

            If (gTest_User.Role = Test_clsUser.eRole.Quality) Then
                txtName_Quality.Text = gTest_User.Name
            Else
                txtName_Quality.Text = ""
            End If

        End If
    End Sub


    Private Function GetProjectID() As Integer
        '=====================================
        'Dim pTestProjectID As Integer = 0

        'Dim pPart_Project As New clsPartProject

        'Dim pPNID As Integer = pPart_Project.GetPNID(cmbParkerPN.Text)
        'Dim pRevID As Integer = pPart_Project.GetRevID(pPNID, cmbParkerPN_Rev.Text)

        'Dim pSealTestEntities As New SealTestDBEntities
        'Dim pQry = (From pRec In pSealTestEntities.tblTestProject Where pRec.fldPNID = pPNID And pRec.fldRevID = pRevID Select pRec.fldID).ToList()

        'If (pQry.Count() > 0) Then
        '    pTestProjectID = pQry(0)
        'End If

        'Return pTestProjectID

    End Function


    Private Sub PopulateParkerPN_Rev()
        '=============================
        cmbParkerPN.Items.Clear()
        cmbParkerPN_Rev.Items.Clear()

        If (IsNothing(mTestProject)) Then
            mTestProject = New Test_clsProject(gPartProject)
        End If

        Dim pPartEntities As New SealPartDBEntities
        Dim pQryPN = (From pRec In pPartEntities.tblPN Select pRec).ToList()

        If (pQryPN.Count > 0) Then
            For i As Integer = 0 To pQryPN.Count - 1
                Dim pNew_PN As String = pQryPN(i).fldCurrent
                If (pNew_PN <> "") Then
                    If (Not cmbParkerPN.Items.Contains(pNew_PN)) Then
                        cmbParkerPN.Items.Add(pNew_PN)
                    End If
                Else
                    Dim pLegacy_PN As String = pQryPN(i).fldLegacy

                    If (pLegacy_PN <> "") Then
                        If (Not cmbParkerPN.Items.Contains(pLegacy_PN)) Then
                            cmbParkerPN.Items.Add(pLegacy_PN)
                        End If
                    End If

                End If
            Next

            'AES 21JUN17
            If (gTest_Project.PN_Selected <> "") Then
                cmbParkerPN.SelectedIndex = cmbParkerPN.Items.IndexOf(gTest_Project.PN_Selected)
            Else
                cmbParkerPN.SelectedIndex = 0
            End If

        End If

        Dim pPart_Project As New clsPartProject
        Dim pPN_ID As Integer = mPNID

        Dim pQryRev = (From pRec In pPartEntities.tblRev
                       Where pRec.fldPNID = pPN_ID Order By pRec.fldID Ascending Select pRec).ToList()

        If (pQryRev.Count > 0) Then

            For i As Integer = 0 To pQryRev.Count - 1
                Dim pNew_Rev As String = pQryRev(i).fldCurrent
                If (pNew_Rev <> "") Then
                    If (Not cmbParkerPN_Rev.Items.Contains(pNew_Rev)) Then
                        cmbParkerPN_Rev.Items.Add(pNew_Rev)
                    End If
                Else
                    Dim pLegacy_Rev As String = pQryRev(i).fldLegacy

                    If (pLegacy_Rev <> "") Then
                        If (Not cmbParkerPN_Rev.Items.Contains(pLegacy_Rev)) Then
                            cmbParkerPN_Rev.Items.Add(pLegacy_Rev)
                        End If
                    End If

                End If

            Next
            'AES 21JUN17
            If (gTest_Project.Rev_Selected <> "") Then
                cmbParkerPN_Rev.SelectedIndex = cmbParkerPN_Rev.Items.IndexOf(gTest_Project.Rev_Selected)
            Else
                cmbParkerPN_Rev.SelectedIndex = 0
            End If

        End If

        mPN = cmbParkerPN.Text
        mPNRev = cmbParkerPN_Rev.Text

    End Sub

#End Region

#End Region


#Region "MENU RELATED ROUTINES:"

    Private Sub mnuUserInfo_Click(sender As System.Object, e As System.EventArgs) Handles mnuUserInfo.Click
        '===================================================================================================
        'If gTest_User.IsAdminExists() Then
        If gTest_User.Admin() Then
            Dim pTest_frmUserGroup As New Test_frmUserGroup()
            pTest_frmUserGroup.ShowDialog()
        Else
            Dim pTest_frmAdminLogIn As New Test_frmAdminLogIn()
            pTest_frmAdminLogIn.ShowDialog()
        End If

    End Sub


    Private Sub mnuUpdEquipList_Click(sender As System.Object, e As System.EventArgs) Handles mnuUpdEquipList.Click
        '==========================================================================================================
        With openFileDialog1
            .Filter = "SealTest Equipment Files (*.xlsx)|*.xlsx"
            .FilterIndex = 1
            .InitialDirectory = gTest_File.DirEquip
            .FileName = ""
            .Title = "Open"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim pEquipFileName As String = .FileName

                gTest_Equipment.UpdateTo_DB(pEquipFileName)
                If (pEquipFileName <> "") Then
                    gTest_Test.FileName_EquipList = pEquipFileName
                    gTest_Test.SaveTo_DB()
                End If
            End If

        End With
    End Sub

#End Region


#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbParkerPN_SelectedIndexChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles cmbParkerPN.SelectedIndexChanged
        '========================================================================================================

        If (gTest_Project.PN_Selected <> "") Then
            gTest_Project.PN_Selected = cmbParkerPN.Text
            mTestProject.PN_Selected = gTest_Project.PN_Selected
        End If

        cmbParkerPN_Rev.Items.Clear()

        Dim pPart_Project As New clsPartProject
        Dim pPN_ID As Integer = mTestProject.GetPNID(cmbParkerPN.Text)

        Dim pPartEntities As New SealPartDBEntities

        Dim pQryRev = (From pRec In pPartEntities.tblRev
                       Where pRec.fldPNID = pPN_ID Order By pRec.fldID Ascending Select pRec).ToList()

        If (pQryRev.Count > 0) Then

            For i As Integer = 0 To pQryRev.Count - 1
                Dim pNew_Rev As String = pQryRev(i).fldCurrent
                If (pNew_Rev <> "") Then
                    If (Not cmbParkerPN_Rev.Items.Contains(pNew_Rev)) Then
                        cmbParkerPN_Rev.Items.Add(pNew_Rev)
                    End If
                Else
                    Dim pLegacy_Rev As String = pQryRev(i).fldLegacy

                    If (pLegacy_Rev <> "") Then
                        If (Not cmbParkerPN_Rev.Items.Contains(pLegacy_Rev)) Then
                            cmbParkerPN_Rev.Items.Add(pLegacy_Rev)
                        End If
                    End If

                End If

            Next

            'cmbParkerPN_Rev.SelectedIndex = 0

            'AES 07JUN17
            'If (gTest_Project.Rev_Selected <> "") Then
            '    cmbParkerPN_Rev.SelectedIndex = cmbParkerPN_Rev.Items.IndexOf(gTest_Project.Rev_Selected)
            'Else
            '    cmbParkerPN_Rev.SelectedIndex = 0
            'End If

            If (gTest_Project.Rev_Selected <> "") Then

                If (Not cmbParkerPN_Rev.Items.Contains(gTest_Project.Rev_Selected)) Then
                    cmbParkerPN_Rev.SelectedIndex = 0
                Else
                    cmbParkerPN_Rev.SelectedIndex = cmbParkerPN_Rev.Items.IndexOf(gTest_Project.Rev_Selected)
                End If
            Else
                cmbParkerPN_Rev.SelectedIndex = 0

            End If

        End If

        mPN = cmbParkerPN.Text
        mPNRev = cmbParkerPN_Rev.Text

        '....Get TestProjectID
        Dim pTestProjectID As Integer = 0
        pTestProjectID = GetProjectID()

        gTest_Project.ProjectID = pTestProjectID

        InitializeControls()    'AES 21NOV16

        '....Retrieve HW data
        'mTestProject.RetrieveFromHW(mPNID, mRevID)

        CreateNewTestProject()

        '....Retrieve HW data
        'mTestProject.RetrieveFromHW(mPNID, mRevID)

        If (pTestProjectID > 0) Then

            mTestProject.ID = pTestProjectID
            '....Retrieve Test Project Data
            mTestProject.RetrieveFrom_DB(gUnit)
        End If

        Dim pPNID As Integer = mTestProject.GetPNID(mPN)
        Dim pRevID As Integer = mTestProject.GetRevID(pPNID, mPNRev)
        mProjectID = GetPartProjectID(pPNID, pRevID)
        GetPartProjectInfo(pPNID, pRevID, mProjectID)

        ResetControls("PNR")
        PopulateMO()
        DisplayData()
        LockControls()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev & ", " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No() & "- " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).No
            Else
                SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev & ", " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No()
            End If
        Else
            SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev
        End If


    End Sub


    Private Sub PopulateMO()
        '===================

        cmbMO.Items.Clear()

        For i As Integer = 0 To mTestProject.Test_MO.Count - 1
            cmbMO.Items.Add(mTestProject.Test_MO(i).No.ToString())
        Next

        If (mTestProject.Test_MO.Count > 0) Then
            cmbMO.SelectedIndex = 0
        End If

    End Sub


    Private Sub PopulateReport(ByVal MOID_In As Integer)
        '================================================
        cmbReportNo.Items.Clear()

        For i As Integer = 0 To mTestProject.Test_MO.Count - 1
            If (mTestProject.Test_MO(i).ID = MOID_In) Then
                For j As Integer = 0 To mTestProject.Test_MO(i).Test_Report.Count - 1
                    cmbReportNo.Items.Add(mTestProject.Test_MO(i).Test_Report(j).No.ToString())
                Next
                If (mTestProject.Test_MO(i).Test_Report.Count > 0) Then
                    cmbReportNo.SelectedIndex = 0
                End If
                Exit For
            End If
        Next

    End Sub


    Private Sub cmbParkerPN_Rev_SelectedIndexChanged(sender As System.Object,
                                                     e As System.EventArgs) Handles cmbParkerPN_Rev.SelectedIndexChanged
        '=================================================================================================================
        Dim pPN As String = cmbParkerPN.Text
        Dim pPNRev As String = cmbParkerPN_Rev.Text

        If (gTest_Project.Rev_Selected <> "") Then
            gTest_Project.Rev_Selected = pPNRev
            mTestProject.Rev_Selected = gTest_Project.Rev_Selected
        End If

        Dim pPart_Project As New clsPartProject()
        Dim pPNID As Integer = mTestProject.GetPNID(pPN)
        Dim pRevID As Integer = mTestProject.GetRevID(pPNID, pPNRev)

        Dim pPartEntities As New SealPartDBEntities
        Dim pQryPart = (From pRec In pPartEntities.tblProject
                              Where pRec.fldPNID = pPNID And
                              pRec.fldRevID = pRevID Select pRec).ToList()

        If (pQryPart.Count > 0) Then
            Dim pCustID As Integer = pQryPart(0).fldCustID
            txtCust.Text = mTestProject.PartProject.CustInfo.CustName
            txtCustPN.Text = pQryPart(0).fldPN_Cust

        Else     'AES 04JUL17 
            txtCust.Text = ""
            txtCustPN.Text = ""
        End If

        'mTestProject.CustName = txtCust.Text
        'mTestProject.PN_Cust = txtCustPN.Text

        Dim pSignedOff As Boolean = False
        If (IsSignedOff(pPNID, pRevID)) Then
            pSignedOff = True
            lblPN.BackColor = Color.LightGray
            lblRev.BackColor = Color.LightGray

            cmbMO.Enabled = True
            txtMO.Enabled = True
            cmdMO_New.Enabled = True
            cmdMO_Save.Enabled = False
            cmdMO_Delete.Enabled = True

            cmbReportNo.Enabled = True
            txtReport.Enabled = True
            cmdReport_New.Enabled = True
            cmdReport_Save.Enabled = False
            cmdReport_Delete.Enabled = True

            grpUser.Enabled = True

        Else
            lblPN.BackColor = Color.OrangeRed
            lblRev.BackColor = Color.OrangeRed

            cmbMO.Enabled = False
            txtMO.Enabled = False
            cmdMO_New.Enabled = False
            cmdMO_Save.Enabled = True
            cmdMO_Delete.Enabled = False

            cmbReportNo.Enabled = False
            txtReport.Enabled = False
            cmdReport_New.Enabled = False
            cmdReport_Save.Enabled = True
            cmdReport_Delete.Enabled = False

            grpUser.Enabled = False

        End If

        Dim pTestProjectID As Integer = 0
        pTestProjectID = GetProjectID()

        'mTestProject.PartProject.PNR.PN = pPN
        'mTestProject.PartProject.PNR.PN_Rev = pPNRev

        gTest_Project.ProjectID = pTestProjectID     'AES 28JUN17
        InitializeControls()

        If (pTestProjectID > 0) Then
            CreateNewTestProject()
            mTestProject.ID = pTestProjectID

            '....Retrieve HW data
            'mTestProject.RetrieveFromHW()

            '....Retrieve Test Project Data
            mTestProject.RetrieveFrom_DB(gUnit)

        Else
            CreateNewTestProject()
            mTestProject.ID = pTestProjectID

            '....Retrieve HW data
            'mTestProject.RetrieveFromHW()
        End If

        'Dim pPNID As Integer = mTestProject.GetPNID(mPN)
        'Dim pRevID As Integer = mTestProject.GetRevID(pPNID, mPNRev)
        mProjectID = GetPartProjectID(pPNID, pRevID)
        GetPartProjectInfo(pPNID, pRevID, mProjectID)

        If (pSignedOff) Then
            Dim pSealEntities As New SealTestDBEntities
            Dim pQry = (From pRec In pSealEntities.tblTestProject
                                Where pRec.fldPNID = pPNID And pRec.fldRevID = pRevID Select pRec).ToList()

            If (pQry.Count > 0) Then
                txtSignedOff.Text = pQry(0).fldUserSignedOff
            Else
                txtSignedOff.Text = mTestProject.UserAdmin
            End If

            chkSignOff.Checked = True
            chkSignOff.Text = "Signed-off"
        Else
            chkSignOff.Checked = False
            chkSignOff.Text = "Sign-off"
        End If

        ResetControls("PNR")
        PopulateMO()

        DisplayData()
        LockControls()

    End Sub


    Private Sub cmbMO_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                           Handles cmbMO.SelectedIndexChanged, cmbMO.TextChanged
        '========================================================================================
        Dim pMO_No As Int64 = 0

        If (cmbMO.Text <> "") Then
            pMO_No = Convert.ToInt64(cmbMO.Text)
            For i As Integer = 0 To mTestProject.Test_MO.Count - 1
                If (mTestProject.Test_MO(i).No = pMO_No) Then
                    Dim pMO_ID As Integer = mTestProject.Test_MO(i).ID
                    PopulateReport(pMO_ID)
                    gTest_frmMain.MO_Sel = cmbMO.SelectedIndex
                End If
            Next

        End If

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev & ", " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No() & "- " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).No
            Else
                SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev & ", " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No()
            End If
        Else
            SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev
        End If

    End Sub


    Private Sub cmbReportNo_SelectedIndexChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles cmbReportNo.SelectedIndexChanged
        '========================================================================================================
        gTest_frmMain.Report_Sel = cmbReportNo.SelectedIndex

        DisplayData()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev & ", " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No() & "- " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).No
            Else
                SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev & ", " & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No()
            End If
        Else
            SBPanel2.Text = mTestProject.PartProject.PNR.PN & "-" & mTestProject.PartProject.PNR.PN_Rev
        End If

    End Sub

#Region "UTILITY ROUTINES:"

    Private Sub LockControls()
        '=====================
        If (cmbMO.Items.Count = 0) Then
            cmdMO_Delete.Enabled = False
            cmdReport_New.Enabled = False
            cmdReport_Delete.Enabled = False
        Else
            If (gTest_User.Role = Test_clsUser.eRole.Admin Or gTest_User.Role = Test_clsUser.eRole.Tester) Then
                cmdMO_New.Enabled = True
                cmdMO_Delete.Enabled = True
                cmdReport_New.Enabled = True
                'cmdReport_Delete.Enabled = True
            End If

            If (cmbReportNo.Items.Count = 0) Then
                cmdReport_Delete.Enabled = False
            Else
                If (gTest_User.Role = Test_clsUser.eRole.Admin Or gTest_User.Role = Test_clsUser.eRole.Tester) Then
                    cmdReport_Delete.Enabled = True
                End If

            End If
        End If

        If (Not IsNothing(mTestProject)) Then

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    cmdGeneral.Enabled = True
                    cmdLeakage.Enabled = True
                    cmdLoad.Enabled = True
                Else
                    If gTest_User.Role = Test_clsUser.eRole.Admin Then
                        cmdGeneral.Enabled = True
                        cmdLeakage.Enabled = True
                        cmdLoad.Enabled = True
                    Else
                        cmdGeneral.Enabled = False
                        cmdLeakage.Enabled = False
                        cmdLoad.Enabled = False
                    End If

                End If
            Else
                If gTest_User.Role = Test_clsUser.eRole.Admin Then
                    cmdGeneral.Enabled = True
                    cmdLeakage.Enabled = True
                    cmdLoad.Enabled = True
                Else
                    cmdGeneral.Enabled = False
                    cmdLeakage.Enabled = False
                    cmdLoad.Enabled = False
                End If
            End If
        End If

    End Sub


    'Private Function GetProjectID(ByVal PNID_In As Integer, ByVal RevID_In As Integer,
    '                             ByVal CustID_In As Integer, ByVal CustPN As String) As Integer
    '    '=========================================================================================
    '    Dim pProjectID As Integer = 0

    '    Dim pTestProjectEntities As New SealTestDBEntities
    '    Dim pQry = (From pRec In pTestProjectEntities.tblTestProject
    '                      Where pRec.fldPNID = PNID_In And
    '                      pRec.fldRevID = RevID_In Select pRec).First()


    '    pProjectID = pQry.fldID

    '    mProjectID = pProjectID

    '    Return pProjectID

    'End Function


    Private Function GetPartProjectID(ByVal PNID_In As Integer, ByVal RevID_In As Integer) As Integer
        '=========================================================================================
        Dim pProjectID As Integer = 0

        Dim pPartProjectEntities As New SealPartDBEntities
        Dim pQryPart = (From pRec In pPartProjectEntities.tblProject
                          Where pRec.fldPNID = PNID_In And
                          pRec.fldRevID = RevID_In Select pRec).ToList()

        If (pQryPart.Count > 0) Then
            pProjectID = pQryPart(0).fldID
        End If


        Dim pTestProjectEntities As New SealTestDBEntities
        Dim pRecCount As Integer = (From pRec In pTestProjectEntities.tblTestProject
                                    Where pRec.fldPNID = PNID_In And
                                    pRec.fldRevID = RevID_In Select pRec).Count()

        If (pRecCount > 0) Then
            Dim pQry = (From pRec In pTestProjectEntities.tblTestProject
                                   Where pRec.fldPNID = PNID_In And
                                   pRec.fldRevID = RevID_In Select pRec).First()

            pQry.fldPNID = PNID_In
            pQry.fldRevID = RevID_In
            pQry.fldPNR_CustInfoID = pProjectID
            'pQry.fldID = pQry.fldID
            pTestProjectEntities.SaveChanges()
        End If


        Return pProjectID

    End Function


    Private Sub RetrieveThicknesses(ByVal ThickCode_In As String)
        '==========================================================

        ''For a given plating thickness code, this subroutine returns the min and max
        ''....plating thicknessess from the database.
        Dim pThickMin, pThickMax As Double
        Dim pThickMinMet, pThickMaxMet As Double

        Dim pSealCADDBEntities As New SealIPEMCSDBEntities()

        Dim pQryPlatingThick = (From it In pSealCADDBEntities.tblPlatingThick
                                   Where it.fldPlatingThickCode = ThickCode_In
                                   Order By it.fldPlatingThickCode Ascending Select it).First()

        pThickMin = pQryPlatingThick.fldPlatingThickMinEng
        pThickMax = pQryPlatingThick.fldPlatingThickMaxEng

        '....Metric Unit (User Unit): 
        pThickMinMet = pQryPlatingThick.fldPlatingThickMinMet
        pThickMaxMet = pQryPlatingThick.fldPlatingThickMaxMet

        If gUnit.System = "English" Then

            txtPlatingThick_Min.Text = gUnit.WriteInUserL(pThickMin, "TFormat")
            txtPlatingThick_Max.Text = gUnit.WriteInUserL(pThickMax, "TFormat")

        ElseIf gUnit.System = "Metric" Then

            txtPlatingThick_Min.Text = gUnit.WriteInUserL(pThickMinMet, "TFormat")
            txtPlatingThick_Max.Text = gUnit.WriteInUserL(pThickMaxMet, "TFormat")

        End If

    End Sub

#End Region

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdSetUnits_Click(sender As System.Object, e As System.EventArgs) Handles cmdSetUnits.Click
        '===================================================================================================
        gTest_Project.ID = mTestProject.ID
        'gTest_Project.CustName = mTestProject.CustName
        'gTest_Project.PN_Cust = mTestProject.PN_Cust
        gTest_Project.Test_Unit = mTestProject.Test_Unit.Clone()
        'gTest_Project.Part_HW = mTestProject.Part_HW.Clone()
        gTest_Project = mTestProject.Clone()

        Dim pTest_frmUnits As New Test_frmUnit()
        pTest_frmUnits.ShowDialog()
    End Sub


    Private Sub cmdMO_New_Click(sender As System.Object, e As System.EventArgs) Handles cmdMO_New.Click
        '===============================================================================================
        cmdMO_New.Enabled = False
        txtMO.Visible = True
        cmbMO.Visible = False

        cmdMO_Save.Visible = True
        cmdMO_Delete.Visible = False

        txtMO.Text = ""
        txtMO.Focus()

        ResetControls("MO")
    End Sub


    Private Sub cmdMO_Save_Click(sender As System.Object, e As System.EventArgs) Handles cmdMO_Save.Click
        '================================================================================================
        Save_MO()
        LockControls()

    End Sub


    Private Sub cmdMO_Delete_Click(sender As System.Object, e As System.EventArgs) Handles cmdMO_Delete.Click
        '====================================================================================================

        Dim pMO_No As Integer = 0
        If (cmbMO.Text <> "") Then
            pMO_No = Convert.ToInt64(cmbMO.Text)
        End If

        For i As Integer = 0 To mTestProject.Test_MO.Count - 1
            If (mTestProject.Test_MO(i).No = pMO_No) Then
                Dim pAns As Integer
                pAns = MessageBox.Show("All the Reports under this MO will be deleted. Do you want to continue?", "Delete MO", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If (pAns = DialogResult.Yes) Then
                    mTestProject.Test_MO.RemoveAt(i)
                    mTestProject.DeleteFrom_tblTestMO(pMO_No)
                    Exit For
                Else
                    Exit Sub
                End If

            End If
        Next

        cmbReportNo.Items.Clear()
        optOpen.Checked = True
        PopulateMO()
        LockControls()
    End Sub


    Private Sub cmdReport_New_Click(sender As System.Object, e As System.EventArgs) Handles cmdReport_New.Click
        '======================================================================================================

        cmdReport_New.Enabled = False
        txtReport.Visible = True
        cmbReportNo.Visible = False

        optOpen.Checked = True
        lblClose.Visible = False
        txtDateClosed.Visible = False

        cmdReport_Save.Visible = True
        cmdReport_Delete.Visible = False

        txtReport.Text = ""
        txtReport.Focus()

        ResetControls()

    End Sub


    Private Sub cmdReport_Delete_Click(sender As System.Object, e As System.EventArgs) Handles cmdReport_Delete.Click
        '============================================================================================================

        Dim pMO_No As Int64 = Convert.ToInt64(cmbMO.Text)

        Dim pReport_No As Integer = 0

        If (cmbReportNo.Text <> "") Then
            pReport_No = Convert.ToInt64(cmbReportNo.Text)
        End If


        For i As Integer = 0 To mTestProject.Test_MO.Count - 1

            If (mTestProject.Test_MO(i).No = pMO_No) Then
                For j As Integer = 0 To mTestProject.Test_MO(i).Test_Report.Count - 1
                    If (mTestProject.Test_MO(i).Test_Report(j).No = pReport_No) Then
                        Dim pMO_ID As Integer = mTestProject.Test_MO(i).ID
                        mTestProject.Test_MO(i).DeleteFrom_Report(mTestProject.ID, pReport_No)
                        mTestProject.Test_MO(i).Test_Report.RemoveAt(j)
                        PopulateReport(pMO_ID)
                        LockControls()
                        Exit For
                    End If
                Next

                Exit For
            End If
        Next

    End Sub

    Private Sub cmdLeakage_Click(sender As System.Object, e As System.EventArgs) Handles cmdLeakage.Click
        '================================================================================================
        SaveData()

        Try
            If ((chkLeakage.Checked And gTest_User.Role <> Test_clsUser.eRole.Admin) Or (gTest_User.Role = Test_clsUser.eRole.Admin)) Then

                Dim pTest_frmLeak As New Test_frmLeak()
                pTest_frmLeak.ShowDialog()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub cmdLoad_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoad.Click
        '==========================================================================================
        SaveData()

        If ((chkLoad.Checked And gTest_User.Role <> Test_clsUser.eRole.Admin) Or (gTest_User.Role = Test_clsUser.eRole.Admin)) Then

            Dim pTest_frmLoad As New Test_frmLoad()
            pTest_frmLoad.ShowDialog()
        End If
    End Sub


    Private Sub cmdSealIPEData_Click(sender As System.Object, e As System.EventArgs) Handles cmdSealIPEData.Click
        '========================================================================================================

        ''Dim pImport As Boolean = True
        ''If (mTestProject.Test_MO.Count > 0) Then
        ''    If (mTestProject.Test_MO(0).Report.Count > 0) Then
        ''        Dim pAns As Integer
        ''        pAns = MessageBox.Show("For this P/N & Rev. level, there are already existing record(s)." & vbCrLf &
        ''                    "Do you want to import seal data again?",
        ''                    "Import Seal Data Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        ''        If (pAns = DialogResult.No) Then
        ''            pImport = False
        ''        End If

        ''    End If
        ''End If

        ''If (pImport) Then
        ''    CreateNewTestProject()

        ''    mTestProject.Import_SealIPEDesign(mProject)
        ''    DisplayData()
        ''    mTestProject.SaveTo_DB_DesignData(gUnit)
        ''End If

    End Sub


    Private Sub cmdGeneral_Click(sender As System.Object, e As System.EventArgs) Handles cmdGeneral.Click
        '==================================================================================================
        SaveData()
        Dim pTest_frmGen As New Test_frmGen()
        pTest_frmGen.ShowDialog()

    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        SaveData()
        Me.Close()
    End Sub

    Private Sub SaveData()
        '=================
        gTest_Project.PN_Selected = cmbParkerPN.Text
        gTest_Project.Rev_Selected = cmbParkerPN_Rev.Text
        mTestProject.PN_Selected = gTest_Project.PN_Selected
        mTestProject.Rev_Selected = gTest_Project.Rev_Selected

        mTestProject.SignedOff = chkSignOff.Checked

        Dim pMO_No As Int64 = 0
        If (cmbMO.Text <> "") Then
            pMO_No = Convert.ToInt64(cmbMO.Text)
        End If

        Dim pReport_No As Int64 = 0
        If (cmbReportNo.Text <> "") Then
            pReport_No = Convert.ToInt64(cmbReportNo.Text)
        End If

        Dim pMO_ID As Integer = 0
        Dim pReport_ID As Integer = 0
        'Dim pMO_Index As Integer = 0, pReport_Index As Integer = 0

        For i As Integer = 0 To mTestProject.Test_MO.Count - 1
            If (mTestProject.Test_MO(i).No = pMO_No) Then
                pMO_ID = mTestProject.Test_MO(i).ID

                gTest_frmMain.MO_Sel = i
                'pMO_Index = i
                For j As Integer = 0 To mTestProject.Test_MO(i).Test_Report.Count - 1
                    If (mTestProject.Test_MO(i).Test_Report(j).No = pReport_No) Then
                        pReport_ID = mTestProject.Test_MO(i).Test_Report(j).ID
                        'pReport_Index = j
                        gTest_frmMain.Report_Sel = j
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next

        Dim pSealQty As Integer = updSealQty.Value

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            mTestProject.Test_Spec.SealQty = pSealQty
            mTestProject.Test_Spec.Leak_Exists = chkLeakage.Checked
            mTestProject.Test_Spec.Load_Exists = chkLoad.Checked
        End If

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty = pSealQty

                'AES 23NOV16
                If (IsNothing(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image)) Then

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).GenImage = New List(Of Test_clsReport.sGenImage)
                End If

                'AES 14NOV16
                If (pSealQty > mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal.Count) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal.Clear()
                    For i As Integer = 0 To pSealQty - 1
                        Dim pGenSeal As New Test_clsReport.sGenSeal

                        pGenSeal.SeqID = i + 1
                        pGenSeal.SN = i + 1
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Seal.Add(pGenSeal)
                        Dim pTestSeal As New Test_clsSeal()
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).TestSeal.Add(pTestSeal)
                    Next
                End If
            End If
        End If

        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_Exists = chkLeakage.Checked
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_Exists = chkLoad.Checked
                End If
            End If

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    'mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).SealQty = pSealQty

                    'AES 14NOV16
                    'If (pSealQty > mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Seal.Count) Then
                    '    mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Seal.Clear()
                    '    For i As Integer = 0 To pSealQty - 1
                    '        Dim pGenSeal As New clsTestReport.sGenSeal

                    '        pGenSeal.SeqID = i + 1
                    '        pGenSeal.SN = i + 1
                    '        mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Seal.Add(pGenSeal)
                    '    Next
                    'End If

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak_Exists = chkLeakage.Checked
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load_Exists = chkLoad.Checked

                    If (chkTester_Signed.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester_Signed = True
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester_Name = gTest_User.Name
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester_SignedDate = dtpTester.Value
                    Else
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester_Signed = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester_Name = ""
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester_SignedDate = DateTime.Now()
                    End If
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_Signed = chkEngg_Signed.Checked
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_Signed = chkQuality_Signed.Checked

                End If
            End If

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Eng) Then
            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    If (chkEngg_Signed.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_Signed = True
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_Name = gTest_User.Name
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_SignedDate = dtpEngg.Value
                    Else
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_Signed = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_Name = ""
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng_SignedDate = DateTime.Now()
                    End If
                End If
            End If

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Quality) Then
            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then


                    If (chkQuality_Signed.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_Signed = True
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_Name = gTest_User.Name
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_SignedDate = dtpQuality.Value
                    Else
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_Signed = False
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_Name = ""
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality_SignedDate = DateTime.Now()
                    End If

                End If
            End If
        End If

        mTestProject.SaveTo_DB(gUnit, mMO_Sel, mReport_Sel, mPNID, mRevID)

        gTest_Project = mTestProject.Clone()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub ResetControls(Optional ByVal Level_In As String = "")
        '============================================================
        If (Level_In = "PNR") Then
            cmbMO.Items.Clear()
            updSealQty.Value = 1
            cmbReportNo.Items.Clear()
            optOpen.Checked = True

        ElseIf (Level_In = "MO") Then
            cmbReportNo.Items.Clear()
            optOpen.Checked = True

        End If

        txtDateOpen.Text = ""

        chkEngg_Signed.Checked = False
        chkQuality_Signed.Checked = False
        chkTester_Signed.Checked = False

    End Sub


    Private Sub Save_MO()
        '================
        Try

            Dim pQry = (From pRec In mSealTestEntities.tblMO Where pRec.fldTestProjectID = mTestProject.ProjectID
                        Order By pRec.fldID Descending Select pRec).ToList()      'AES 30JUN17

            'Dim pNew_MO As Int64 = 1
            Dim pMO_ID As Integer = 1
            If (pQry.Count > 0) Then
                pMO_ID = pQry(0).fldID + 1
                'pNew_MO = pQry(0).fldNo + 1
            End If

            Dim pMO_No As Int64 = 0

            If (txtMO.Text <> "") Then
                pMO_No = Convert.ToInt64(txtMO.Text)
            End If

            If (pMO_No > 0) Then

                If (Not cmbMO.Items.Contains(Convert.ToString(pMO_No))) Then
                    Dim pMO As New Test_clsMO
                    pMO.ID = pMO_ID
                    pMO.No = pMO_No

                    cmbMO.Items.Add(Convert.ToString(pMO_No))
                    mTestProject.Test_MO.Add(pMO)
                    cmbMO.Text = Convert.ToString(pMO_No)
                End If

                cmdMO_New.Enabled = True
                txtMO.Visible = False
                cmbMO.Visible = True

                cmdMO_Save.Visible = False
                cmdMO_Delete.Visible = True

                ResetControls("MO")
                LockControls()

            Else

                MessageBox.Show("Please input proper MO No.", "Incorrect MO No.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                cmdMO_New.Enabled = True
                txtMO.Visible = False
                cmbMO.Visible = True

                cmdMO_Save.Visible = False
                cmdMO_Delete.Visible = True

                ResetControls("MO")
                LockControls()
                If (cmbMO.Items.Count > 0) Then
                    cmbMO.Text = ""
                    cmbMO.SelectedIndex = 0
                End If

            End If


        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub DisplayData()
        '====================
        If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
            If (mTestProject.IsTesterSigned()) Then
                chkSignOff.Enabled = False
            Else
                chkSignOff.Enabled = True
            End If
        End If

        'AES 07JUN17
        If (gTest_Project.PN_Selected <> "") Then
            cmbParkerPN.Text = gTest_Project.PN_Selected
        End If

        If (gTest_Project.Rev_Selected <> "") Then
            cmbParkerPN_Rev.Text = gTest_Project.Rev_Selected
        End If

        txtCust.Text = mTestProject.PartProject.CustInfo.CustName
        txtCustPN.Text = mTestProject.PartProject.CustInfo.PN_Cust

        'AES 02AUG17
        Dim pSealQty As Integer = mTestProject.Test_Spec.SealQty
        updSealQty.Value = mTestProject.Test_Spec.SealQty


        chkSignOff.Checked = mTestProject.SignedOff

        If (Not IsNothing(mTestProject.PartProject.PNR.SealType)) Then

            cmbSealType.Text = mTestProject.PartProject.PNR.SealType.ToString()
            cmbPOrient.Text = mTestProject.PartProject.PNR.HW.POrient
            txtMCS.Text = mTestProject.PartProject.PNR.HW.MCrossSecNo


            'AES 01AUG17
            'lblStandard.Text = "N"
            If (mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                If (mTestProject.PartProject.PNR.HW.Adjusted) Then
                    lblStandard.Text = "N"
                    lblStandard.BackColor = Color.Yellow
                Else
                    lblStandard.Text = "Y"
                    lblStandard.BackColor = Color.White
                End If
            Else
                lblStandard.Text = "Y"
                lblStandard.BackColor = Color.White
            End If

            cmbMatName.Text = mTestProject.PartProject.PNR.HW.MatName

            Dim pSFinish As Integer
            If (mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                pSFinish = mTestProject.PartProject.PNR.HW.SFinish
                cmbSFinish.Text = pSFinish.ToString()
            Else
                cmbSFinish.Text = ""
            End If

            If (mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mTestProject.PartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                If (mTestProject.PartProject.PNR.HW.Plating.Exists) Then

                    cmbPlatingCode.Visible = True
                    cmbPlatingThickCode.Visible = True
                    txtPlatingThick_Min.Visible = True
                    txtPlatingThick_Max.Visible = True
                    lblPlatingCode.Visible = True
                    lblPlatingThick.Visible = True
                    lblMin.Visible = True
                    lblMax.Visible = True

                    lblPlatingStatus.Text = ""

                    cmbPlatingCode.Text = mTestProject.PartProject.PNR.HW.Plating.Code
                    If (mTestProject.PartProject.PNR.HW.Plating.ThickCode <> Nothing) Then
                        cmbPlatingThickCode.Text = mTestProject.PartProject.PNR.HW.Plating.ThickCode
                        RetrieveThicknesses(cmbPlatingThickCode.Text)
                    End If
                Else
                    cmbPlatingCode.Text = ""
                    cmbPlatingThickCode.Text = ""
                    txtPlatingThick_Min.Text = ""
                    txtPlatingThick_Max.Text = ""
                    cmbPlatingCode.Visible = False
                    cmbPlatingThickCode.Visible = False
                    txtPlatingThick_Min.Visible = False
                    txtPlatingThick_Max.Visible = False
                    lblPlatingCode.Visible = False
                    lblPlatingThick.Visible = False
                    lblMin.Visible = False
                    lblMax.Visible = False

                    lblPlatingStatus.Text = "NONE"
                End If

            Else
                cmbPlatingCode.Text = ""
                cmbPlatingThickCode.Text = ""
                txtPlatingThick_Min.Text = ""
                txtPlatingThick_Max.Text = ""
                cmbPlatingCode.Visible = False
                cmbPlatingThickCode.Visible = False
                txtPlatingThick_Min.Visible = False
                txtPlatingThick_Max.Visible = False
                lblPlatingCode.Visible = False
                lblPlatingThick.Visible = False
                lblMin.Visible = False
                lblMax.Visible = False

                lblPlatingStatus.Text = "NONE"

            End If

        Else

            cmbSealType.Text = ""
            cmbPOrient.Text = ""
            txtMCS.Text = ""
            lblStandard.Text = ""

            cmbMatName.Text = mTestProject.PartProject.PNR.HW.MatName
            cmbSFinish.Text = ""
            cmbPlatingCode.Text = ""
            cmbPlatingThickCode.Text = ""
            txtPlatingThick_Min.Text = ""
            txtPlatingThick_Max.Text = ""

        End If

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                txtDateOpen.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DateOpen.ToShortDateString()

                If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                    chkLeakage.Checked = mTestProject.Test_Spec.Leak.Exists
                    chkLoad.Checked = mTestProject.Test_Spec.Load.Exists
                Else
                    chkLeakage.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Leak.Exists
                    chkLoad.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Load.Exists
                End If

                updSealQty.Value = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SealQty

                txtName_Tester.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester.Name
                If (txtName_Tester.Text = "" And gTest_User.Role = Test_clsUser.eRole.Tester) Then
                    txtName_Tester.Text = gTest_User.Name
                End If

                chkTester_Signed.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester.Signed
                If (chkTester_Signed.Checked) Then
                    txtDate_Tester.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester.DateSigned.ToShortDateString()
                Else
                    If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
                        txtDate_Tester.Text = DateTime.Now.ToShortDateString()
                    Else
                        txtDate_Tester.Text = ""
                    End If

                End If

                txtName_Engg.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng.Name
                If (txtName_Engg.Text = "" And gTest_User.Role = Test_clsUser.eRole.Eng) Then
                    txtName_Engg.Text = gTest_User.Name
                End If

                chkEngg_Signed.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng.Signed
                If (chkEngg_Signed.Checked) Then
                    txtDate_Engg.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Eng.DateSigned.ToShortDateString()
                Else
                    If (gTest_User.Role = Test_clsUser.eRole.Eng) Then
                        txtDate_Engg.Text = DateTime.Now.ToShortDateString()
                    Else
                        txtDate_Engg.Text = ""
                    End If

                End If

                txtName_Quality.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality.Name
                If (txtName_Quality.Text = "" And gTest_User.Role = Test_clsUser.eRole.Quality) Then
                    txtName_Quality.Text = gTest_User.Name
                End If

                chkQuality_Signed.Checked = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality.Signed
                If (chkQuality_Signed.Checked) Then
                    txtDate_Quality.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Quality.DateSigned.ToShortDateString()
                Else
                    If (gTest_User.Role = Test_clsUser.eRole.Quality) Then
                        txtDate_Quality.Text = DateTime.Now.ToShortDateString()
                    Else
                        txtDate_Quality.Text = ""
                    End If

                End If

            End If

        Else
            chkLeakage.Checked = mTestProject.Test_Spec.Leak.Exists
            chkLoad.Checked = mTestProject.Test_Spec.Load.Exists

            If (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (txtName_Tester.Text = "") Then
                    txtName_Tester.Text = gTest_User.Name
                End If
                If (txtDate_Tester.Text = "") Then
                    txtDate_Tester.Text = DateTime.Now.ToShortDateString()
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Eng) Then

                If (txtName_Engg.Text = "") Then
                    txtName_Engg.Text = gTest_User.Name
                End If
                If (txtDate_Engg.Text = "") Then
                    txtDate_Engg.Text = DateTime.Now.ToShortDateString()
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Quality) Then

                If (txtName_Quality.Text = "") Then
                    txtName_Quality.Text = gTest_User.Name
                End If
                If (txtDate_Quality.Text = "") Then
                    txtDate_Quality.Text = DateTime.Now.ToShortDateString()
                End If

            End If

        End If

        If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
            chkTester_Signed.Enabled = True
            cmdTester_Sign.Enabled = True
            chkEngg_Signed.Enabled = False
            cmdEnggMgr_Sign.Enabled = False
            chkQuality_Signed.Enabled = False
            cmdQualityMgr_Sign.Enabled = False

            If (chkTester_Signed.Checked) Then
                txtDate_Tester.Enabled = False
                dtpTester.Enabled = False
            Else
                txtDate_Tester.Enabled = True
                dtpTester.Enabled = True
            End If

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Eng) Then
            chkTester_Signed.Enabled = False
            cmdTester_Sign.Enabled = False
            chkEngg_Signed.Enabled = True
            cmdEnggMgr_Sign.Enabled = True
            chkQuality_Signed.Enabled = False
            cmdQualityMgr_Sign.Enabled = False

            If (chkEngg_Signed.Checked) Then
                txtDate_Engg.Enabled = False
                dtpEngg.Enabled = False
            Else
                txtDate_Engg.Enabled = True
                dtpEngg.Enabled = True
            End If

        ElseIf (gTest_User.Role = Test_clsUser.eRole.Quality) Then
            chkTester_Signed.Enabled = False
            cmdTester_Sign.Enabled = False
            chkEngg_Signed.Enabled = False
            cmdEnggMgr_Sign.Enabled = False
            chkQuality_Signed.Enabled = True
            cmdQualityMgr_Sign.Enabled = True

            If (chkQuality_Signed.Checked) Then
                txtDate_Quality.Enabled = False
                dtpQuality.Enabled = False
            Else
                txtDate_Quality.Enabled = True
                dtpQuality.Enabled = True
            End If
        Else
            '....Admin
            chkTester_Signed.Enabled = False
            cmdTester_Sign.Enabled = False
            chkEngg_Signed.Enabled = False
            cmdEnggMgr_Sign.Enabled = False
            chkQuality_Signed.Enabled = False
            cmdQualityMgr_Sign.Enabled = False
            txtDate_Engg.Enabled = False
            dtpEngg.Enabled = False
            txtDate_Quality.Enabled = False
            dtpQuality.Enabled = False
        End If

        If (chkTester_Signed.Checked And chkEngg_Signed.Checked And chkQuality_Signed.Checked) Then
            cmdReports.Enabled = True
            optClose.Checked = True
            lblClose.Visible = True
            txtDateClosed.Visible = True

            'AES 02MAR17
            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).RetrieveFrom_DB(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SetTestStatus(mTestProject.Test_Spec.LeakMax.Unplated, mTestProject.Test_Spec.LeakSpringBackMin.Unplated, mTestProject.Test_Spec.LoadType, mTestProject.Test_Spec.LoadVal(1), mTestProject.Test_Spec.LoadVal(0))

                    lblStatus.Visible = True
                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DetermineOverallStatus() = Test_clsSeal.eStatus.Pass) Then
                        lblStatus.Text = "PASS"
                        lblStatus.ForeColor = Color.Green
                    Else
                        lblStatus.Text = "FAIL"
                        lblStatus.ForeColor = Color.Red
                    End If
                End If

            End If
        Else
            cmdReports.Enabled = False
            optOpen.Checked = True
            lblClose.Visible = False
            txtDateClosed.Visible = False
            lblStatus.Visible = False
        End If



    End Sub


    Private Sub CreateNewTestProject()
        '==============================
        ''Dim pSealType As String = mTestProject.Part_HW.Type
        ''mTestProject = New Test_clsProject()

        ''mTestProject.PN = cmbParkerPN.Text
        ''mTestProject.Rev = cmbParkerPN_Rev.Text
        ''mTestProject.CustName = txtCust.Text
        ''mTestProject.PN_Cust = txtCustPN.Text
        ''mTestProject.Part_HW.Type = pSealType
        ''mTestProject.ProjectID = GetProjectID()

        ''Dim pPart_Project As New clsPartProject
        ''Dim pPNID As Integer = pPart_Project.GetPNID(cmbParkerPN.Text)
        ''Dim pRevID As Integer = pPart_Project.GetRevID(pPNID, cmbParkerPN_Rev.Text)

        ''mTestProject.Analysis = New IPE_clsAnalysis(mTestProject.Part_HW.Type, gUnit.System)
        ''gTest_File.ReadIniFile(gIPE_ANSYS, gIPE_Unit)
    End Sub

#End Region


#Region "CHECK BOX RELATED ROUTINES:"

    Private Sub chkSignOff_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                          Handles chkSignOff.CheckedChanged
        '===================================================================================
        If (chkSignOff.Checked) Then
            chkSignOff.Text = "Signed-off"
            txtSignedOff.Text = gTest_User.Name
            mTestProject.UserAdmin = txtSignedOff.Text
            mTestProject.SignedOff = True
            mTestProject.UserSignedOff = mTestProject.UserAdmin
            mTestProject.DateSignedOff = DateTime.Now()

            updSealQty.Enabled = False

        Else
            chkSignOff.Text = "Sign-off"
            txtSignedOff.Text = ""
            mTestProject.SignedOff = False
            mTestProject.UserSignedOff = ""
            mTestProject.DateSignedOff = DateTime.MaxValue
            updSealQty.Enabled = True
        End If

    End Sub

#End Region



    'Private Sub cmbParkerPN_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) Handles cmbParkerPN.DrawItem
    '    '==============================================================================================================================
    '    If e.Index < 0 Then
    '        Return
    '    End If

    '    Dim pCmbBox As ComboBox = DirectCast(sender, ComboBox)
    '    e.DrawBackground()
    '    Dim pBrush As Brush = Brushes.Black

    '    'If mBearing_Radial_FP.Mount.Fixture_Candidates.HolesEquiSpaced(e.Index) Then
    '    If pCmbBox.Items(e.Index).ToString() = "NH-76011" Then
    '        pBrush = Brushes.OrangeRed
    '    End If

    '    e.Graphics.DrawString(pCmbBox.Items(e.Index).ToString(), e.Font, pBrush, e.Bounds, StringFormat.GenericDefault)

    '    e.DrawFocusRectangle()
    'End Sub

    Private Function IsSignedOff(ByVal PNID_In As Integer, ByVal RevID_In As Integer) As Boolean
        '========================================================================================
        Dim pSignedOff As Boolean = False
        Dim pSealEntities As New SealTestDBEntities
        Dim pQry = (From pRec In pSealEntities.tblTestProject
                    Where pRec.fldPNID = PNID_In And pRec.fldRevID = RevID_In Select pRec).ToList()

        If (pQry.Count > 0) Then
            pSignedOff = pQry(0).fldSignedOff
        End If

        Return pSignedOff

    End Function


    Private Sub cmdReport_Save_Click(sender As System.Object, e As System.EventArgs) Handles cmdReport_Save.Click
        '========================================================================================================
        Save_Report()

        LockControls()
        If (cmbReportNo.Items.Count > 0) Then
            gTest_frmMain.Report_Sel = cmbReportNo.Items.Count - 1
        End If

        DisplayData()

    End Sub


    Private Sub Save_Report()
        '====================
        Try

            Dim pMO_ID As Integer = mTestProject.Test_MO(mMO_Sel).ID
            Dim pQry = (From pRec In mSealTestEntities.tblReport Where pRec.fldTestProjectID = mTestProject.ProjectID And pRec.fldTestMOID = pMO_ID
                        Order By pRec.fldID Descending Select pRec).ToList()     'AES 30JUN17

            'Dim pNew_Rpt As Integer = 1
            Dim pRpt_ID As Integer = 1
            If (pQry.Count > 0) Then
                pRpt_ID = pQry(0).fldID + 1
                'pNew_Rpt = pQry(0).fldNo + 1
            End If

            Dim pReport_No As Int64 = 0

            If (txtReport.Text <> "") Then
                pReport_No = Convert.ToInt64(txtReport.Text)
            End If

            If (pReport_No > 0) Then
                If (Not cmbReportNo.Items.Contains(Convert.ToString(pReport_No))) Then
                    cmbReportNo.Items.Add(Convert.ToString(pReport_No))
                    cmbReportNo.Text = Convert.ToString(pReport_No)
                    txtDateOpen.Text = DateTime.Now.ToShortDateString()

                    Dim pRpt As New Test_clsReport
                    pRpt.ID = pRpt_ID
                    pRpt.No = pReport_No
                    pRpt.DateOpen = DateTime.Now()
                    pRpt.SealQty = mTestProject.Test_Spec.SealQty
                    updSealQty.Value = mTestProject.Test_Spec.SealQty

                    For i As Integer = 0 To mTestProject.Test_MO.Count - 1
                        If (mTestProject.Test_MO(i).No = Convert.ToInt64(cmbMO.Text)) Then
                            mTestProject.Test_MO(i).Test_Report.Add(pRpt)
                            Exit For
                        End If
                    Next

                End If

                cmdReport_New.Enabled = True
                txtReport.Visible = False
                cmbReportNo.Visible = True

                cmdReport_Save.Visible = False
                cmdReport_Delete.Visible = True
                LockControls()
            Else
                MessageBox.Show("Please input proper Report No.", "Incorrect Report No.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                cmdReport_New.Enabled = True
                txtReport.Visible = False
                cmbReportNo.Visible = True

                cmdReport_Save.Visible = False
                cmdReport_Delete.Visible = True
                LockControls()
                If (cmbReportNo.Items.Count > 0) Then
                    cmbReportNo.Text = ""
                    cmbReportNo.SelectedIndex = 0
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub updSealQty_ValueChanged(sender As System.Object,
                                        e As System.EventArgs) Handles updSealQty.ValueChanged
        '=====================================================================================
        LockControls()
    End Sub


    Private Sub txtMO_KeyPress(sender As System.Object,
                               e As System.Windows.Forms.KeyPressEventArgs) Handles txtMO.KeyPress
        '=========================================================================================
        If e.KeyChar = ChrW(13) Then
            Save_MO()
        End If
    End Sub


    Private Sub txtReport_KeyPress(sender As System.Object,
                                   e As System.Windows.Forms.KeyPressEventArgs) Handles txtReport.KeyPress
        '=================================================================================================
        If e.KeyChar = ChrW(13) Then
            Save_Report()
        End If

    End Sub

    Private Sub cmdReports_Click(sender As System.Object,
                                 e As System.EventArgs) Handles cmdReports.Click
        '========================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pTest_frmRpt As New Test_frmReport()
                pTest_frmRpt.ShowDialog()

            End If
        End If

    End Sub

    Private Sub cmdNotes_Click(sender As System.Object,
                               e As System.EventArgs) Handles cmdNotes.Click
        '===================================================================
        SaveData()
        Dim pTest_frmNotes As New Test_frmNotes()
        pTest_frmNotes.ShowDialog()

    End Sub


    Private Sub cmdQualityMgr_Sign_Click(sender As System.Object,
                                         e As System.EventArgs) Handles cmdTester_Sign.Click,
                                                                cmdEnggMgr_Sign.Click, cmdQualityMgr_Sign.Click
        '========================================================================================================

        Dim pCmdButton As Button = CType(sender, Button)

        Select Case pCmdButton.Name

            Case "cmdTester_Sign"
                '------------------------
                If (cmdTester_Sign.Text = "Signed") Then
                    'mTesterClicked = True
                    cmdTester_Sign.Text = "Sign"
                    chkTester_Signed.Checked = False

                Else
                    chkTester_Signed.Checked = True
                End If

            Case "cmdEnggMgr_Sign"
                '-----------------------
                If (cmdEnggMgr_Sign.Text = "Signed") Then
                    chkEngg_Signed.Checked = False
                    cmdEnggMgr_Sign.Text = "Sign"

                Else
                    chkEngg_Signed.Checked = True
                End If

            Case "cmdQualityMgr_Sign"
                '-----------------------
                If (cmdQualityMgr_Sign.Text = "Signed") Then
                    chkQuality_Signed.Checked = False
                    cmdQualityMgr_Sign.Text = "Sign"

                Else
                    chkQuality_Signed.Checked = True
                End If

        End Select
    End Sub

    Private Sub chkTester_Signed_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles chkTester_Signed.CheckedChanged,
                                                        chkEngg_Signed.CheckedChanged,
                                                        chkQuality_Signed.CheckedChanged
        '============================================================================================
        Dim pChkBox As CheckBox = CType(sender, CheckBox)

        Select Case pChkBox.Name

            Case "chkTester_Signed"
                '------------------------
                If (chkTester_Signed.Checked) Then
                    cmdTester_Sign.Text = "Signed"

                    If (chkEngg_Signed.Checked And chkQuality_Signed.Checked) Then
                        cmdReports.Enabled = True
                        optClose.Checked = True
                    Else
                        cmdReports.Enabled = False
                        optOpen.Checked = True
                    End If

                    'AES 09JUN17
                    For i As Integer = 0 To mControl.Count - 1
                        Dim pVal As String = mControl(i).Tag

                        If (pVal = "ADef" Or pVal = "Tester") Then
                            mControl(i).Enabled = False
                        End If

                    Next

                Else

                    If (chkTester_Signed.Checked = False And (chkEngg_Signed.Checked Or chkQuality_Signed.Checked)) Then
                        'mTesterClicked = False
                        Dim pInt As Integer
                        pInt = MessageBox.Show("Unchecking of Tester Signed-off will also " & vbCrLf & "uncheck Engineering and Quality Signed-off." & vbCrLf & "Do you want to continue?", "Tester Signed-off", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        If (pInt = DialogResult.No) Then
                            chkTester_Signed.Checked = True
                            cmdTester_Sign.Enabled = True
                            cmdTester_Sign.Text = "Signed"
                            Exit Sub
                        End If
                    End If

                    cmdTester_Sign.Text = "Sign"

                    'AES 09JUN17
                    For i As Integer = 0 To mControl.Count - 1
                        Dim pVal As String = mControl(i).Tag

                        If (pVal = "ADef" Or pVal = "Tester") Then
                            mControl(i).Enabled = True
                            txtName_Engg.Enabled = False
                        End If

                    Next
                    chkEngg_Signed.Checked = False
                    chkQuality_Signed.Checked = False


                End If

                'AES 09JUN17
                If (gTest_User.Role = Test_clsUser.eRole.Tester) Then
                    chkTester_Signed.Enabled = True
                End If
                'chkTester_Signed.Enabled = True


            Case "chkEngg_Signed"
                '-----------------------
                If (chkEngg_Signed.Checked) Then
                    cmdEnggMgr_Sign.Text = "Signed"
                    If (chkTester_Signed.Checked And chkQuality_Signed.Checked) Then
                        cmdReports.Enabled = True
                        optClose.Checked = True

                    Else
                        cmdReports.Enabled = False
                        optOpen.Checked = True

                    End If

                    'AES 09JUN17
                    For i As Integer = 0 To mControl.Count - 1
                        Dim pVal As String = mControl(i).Tag

                        If (gTest_User.Role = Test_clsUser.eRole.Eng And pVal = "Engg") Then
                            mControl(i).Enabled = False
                        End If
                    Next
                Else
                    cmdEnggMgr_Sign.Text = "Sign"

                    For i As Integer = 0 To mControl.Count - 1
                        Dim pVal As String = mControl(i).Tag

                        If (gTest_User.Role = Test_clsUser.eRole.Eng And pVal = "Engg") Then
                            mControl(i).Enabled = True
                            txtName_Engg.Enabled = False
                        End If
                    Next
                End If

                'AES 09JUN17
                If (gTest_User.Role = Test_clsUser.eRole.Eng) Then
                    chkEngg_Signed.Enabled = True
                End If


            Case "chkQuality_Signed"
                '-----------------------
                If (chkQuality_Signed.Checked) Then
                    cmdQualityMgr_Sign.Text = "Signed"
                    If (chkTester_Signed.Checked And chkEngg_Signed.Checked) Then
                        cmdReports.Enabled = True
                        optClose.Checked = True
                    Else
                        cmdReports.Enabled = False
                        optOpen.Checked = True
                    End If

                    'AES 09JUN17
                    For i As Integer = 0 To mControl.Count - 1
                        Dim pVal As String = mControl(i).Tag

                        If (gTest_User.Role = Test_clsUser.eRole.Quality And pVal = "Quality") Then
                            mControl(i).Enabled = False
                        End If
                    Next

                Else
                    cmdQualityMgr_Sign.Text = "Sign"

                    'AES 09JUN17
                    For i As Integer = 0 To mControl.Count - 1
                        Dim pVal As String = mControl(i).Tag

                        If (gTest_User.Role = Test_clsUser.eRole.Quality And pVal = "Quality") Then
                            mControl(i).Enabled = True
                            txtName_Quality.Enabled = False
                        End If
                    Next
                End If

                'AES 09JUN17
                If (gTest_User.Role = Test_clsUser.eRole.Quality) Then
                    chkQuality_Signed.Enabled = True
                End If

        End Select
    End Sub


    Private Sub dtpTester_ValueChanged(sender As System.Object,
                                       e As System.EventArgs) Handles dtpTester.ValueChanged,
                                                                      dtpEngg.ValueChanged, dtpQuality.ValueChanged
        '=============================================================================================================

        Dim pdtpButton As DateTimePicker = CType(sender, DateTimePicker)

        Select Case pdtpButton.Name

            Case "dtpTester"
                '------------
                txtDate_Tester.Text = dtpTester.Value.ToShortDateString()

            Case "dtpEngg"
                '------------
                txtDate_Engg.Text = dtpEngg.Value.ToShortDateString()

            Case "dtpQuality"
                '------------
                txtDate_Quality.Text = dtpQuality.Value.ToShortDateString()
        End Select


    End Sub


    Private Sub mnuUpdEquipList_MouseMove(sender As System.Object,
                                          e As System.Windows.Forms.MouseEventArgs) Handles mnuUpdEquipList.MouseMove
        '==============================================================================================================
        If (gTest_Test.FileName_EquipList <> "") Then
            'ttpToolTip1.SetToolTip(mnuUpdEquipList, Path.GetFileName(gTest.FileName_EquipList))
            mnuUpdEquipList.ToolTipText = Path.GetFileName(gTest_Test.FileName_EquipList)
        End If
    End Sub


    Private Sub optClose_CheckedChanged(sender As System.Object,
                                        e As System.EventArgs) Handles optClose.CheckedChanged
        '=====================================================================================

        If (optClose.Checked) Then
            'cmdOpenAgain.Visible = True
            'cmdReport_New.Visible = False
            'cmdReport_Save.Visible = False
            'cmdReport_Delete.Visible = False
            lblClose.Visible = True
            txtDateClosed.Visible = True

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    SaveData()
                    txtDateClosed.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).GetClosedDate().ToShortDateString()
                End If
            End If

        End If

    End Sub

    Private Sub optOpen_CheckedChanged(sender As System.Object,
                                       e As System.EventArgs) Handles optOpen.CheckedChanged
        '===================================================================================

        If (optOpen.Checked) Then
            'cmdOpenAgain.Visible = False
            cmdReport_New.Visible = True
            cmdReport_Save.Visible = False
            cmdReport_Delete.Visible = True
            lblClose.Visible = False
            txtDateClosed.Text = ""
            txtDateClosed.Visible = False
        End If

    End Sub


    Private Sub mnuSave_Click(sender As System.Object, e As System.EventArgs) Handles mnuSave.Click
        '============================================================================================
        Cursor.Current = Cursors.WaitCursor
        SaveData()
        Set_SaveFileDialog()
        Cursor.Current = Cursors.Default

    End Sub

    Private Sub mnuSaveAs_Click(sender As System.Object, e As System.EventArgs) Handles mnuSaveAs.Click
        '===============================================================================================
        Cursor.Current = Cursors.WaitCursor
        SaveData()
        Set_SaveFileDialog()
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub Set_SaveFileDialog()
        '===========================
        With saveFileDialog1
            .Filter = "SealTest Session Files (*.SealTest)|*.SealTest"
            .FilterIndex = 1
            .InitialDirectory = ""
            .FileName = ""
            .Title = "Save"
        End With

        If (saveFileDialog1.ShowDialog() = DialogResult.OK) Then
            gTest_File.FileName_SealTest = saveFileDialog1.FileName
            gTest_File.Save_SessionData(gTest_Project)
        End If

    End Sub


    Private Sub mnuRestore_Click(sender As System.Object, e As System.EventArgs) Handles mnuRestore.Click
        '================================================================================================
        Cursor.Current = Cursors.WaitCursor
        Set_OpenFileDialog()
        PopulateParkerPN_Rev()          'AES 29NOV16
        Cursor.Current = Cursors.Default

    End Sub

    Private Sub Set_OpenFileDialog()
        '===========================
        Dim pFileName As String = ""
        Dim pFilePath As String = ""
        Dim pFileName_SealTest As String = ""
        If gTest_File.FileName_SealTest <> "" Then
            pFileName = gTest_File.FileName_SealTest.Remove(gTest_File.FileName_SealTest.Length - 9)
            pFilePath = gTest_File.FileName_SealTest.Substring(0, gTest_File.FileName_SealTest.LastIndexOf("\"))
            pFileName_SealTest = pFileName.Substring(pFileName.LastIndexOf("\") + 1)
        End If

        openFileDialog1.Filter = "SealTest Session Files (*.SealTest)|*.SealTest"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.InitialDirectory = pFilePath
        openFileDialog1.FileName = pFileName_SealTest
        openFileDialog1.Title = "Restore"

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            'gIPE_Project = New clsProject()
            gTest_Project = New Test_clsProject(gPartProject)

            gTest_File.FileName_SealTest = openFileDialog1.FileName
            If gTest_File.FileName_SealTest <> "" Then
                Dim pFileTitle_Temp As String = gTest_File.FileName_SealTest.Substring(gTest_File.FileName_SealTest.LastIndexOf("\") + 1)

                Dim pstrTemp As String = pFileTitle_Temp
                If pFileTitle_Temp.Contains("_"c) Then
                    pstrTemp = pFileTitle_Temp.Substring(0, pFileTitle_Temp.LastIndexOf("_"))
                End If
                pFilePath = gTest_File.FileName_SealTest.Remove(gTest_File.FileName_SealTest.Length - 9)

                gTest_File.Restore_SessionData(gTest_Project, pFilePath)

                ''gIPE_Project.Save_ToDB(gUnit, gIPE_ANSYS)

                Dim pProjectID As Integer = gTest_Project.ProjectID
                gTest_Project.ProjectID = pProjectID
                gTest_Project.SaveTo_DB(gUnit, mMO_Sel, mReport_Sel, mPNID, mRevID)
                ''gTest_Project.SaveTo_DB_DesignData(gUnit)

                If (gTest_Project.Test_MO.Count > gTest_frmMain.MO_Sel) Then
                    If (gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                        gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblTestRptGenSeal(gTest_Project.ID, gTest_Project.Test_MO(gTest_frmMain.MO_Sel).ID)
                        gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblTestRptGenImage(gTest_Project.ID, gTest_Project.Test_MO(gTest_frmMain.MO_Sel).ID)

                        gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLeak(gTest_Project.ID, gTest_Project.Test_MO(gTest_frmMain.MO_Sel).ID)
                        gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLeakData(gTest_Project.ID, gTest_Project.Test_MO(gTest_frmMain.MO_Sel).ID)

                        gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLoad(gTest_Project.ID, gTest_Project.Test_MO(gTest_frmMain.MO_Sel).ID)
                        gTest_Project.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblLoadData(gTest_Project.ID, gTest_Project.Test_MO(gTest_frmMain.MO_Sel).ID)
                    End If
                End If

                Cursor.Current = Cursors.WaitCursor
                Cursor.Current = Cursors.Default
            End If
        End If
    End Sub


    Private Sub mnuStatusReport_Click(sender As System.Object, e As System.EventArgs) Handles mnuStatusReport.Click
        '===========================================================================================================
        Cursor = Cursors.WaitCursor
        SaveData()
        modMain_Test.gTest_Test.StatusReport(cmbParkerPN, gUnit)
        Cursor = Cursors.Default
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '================================================================================================
        Me.Close()
    End Sub

End Class