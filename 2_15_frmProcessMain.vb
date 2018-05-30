'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmMain                        '
'                        VERSION NO  :  1.5                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  30MAY18                                '
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

Public Class Process_frmMain

#Region "MEMBER VARIABLES:"

    '....Local Object. 
    Private mProcess_Project As New clsProcessProj(gPartProject)

    Private mPNID As Integer
    Private mRevID As Integer
    Private mCustomerID As Integer
    Private mLocationID As Integer
    Private mPlatformID As Integer

    Private mRowIndex As Integer
    Private mUserName As New List(Of String)
    Private mUserID As New List(Of Integer)

    Dim mDTP_IssueComment As DateTimePicker
    Dim mDTP_Quote As DateTimePicker

    '....tab Variables
    Dim mHeader, mPreOrder, mExport, mOrdEntry, mCost, mApp, mDesign, mManf, mPurchase, mQlty, mDwg, mTest, mPlanning, mShipping, mIssue, mKeyChar As Boolean
    Dim mTabIndex As New List(Of Integer)

    '....Variables for Deleting Records from GridView
    Dim mblngrdCustContact_PreOrder As Boolean = False
    Dim mblngrdQuote_PreOrder As Boolean = False
    Dim mblngrdForecast_PreOrder As Boolean = False
    Dim mblngrdCustContact_OrdEntry As Boolean = False
    Dim mblngrdSplOperation_Cost As Boolean = False
    Dim mblngrdCavityFace_App As Boolean = False
    Dim mblngrdCavityAxial_App As Boolean = False
    Dim mblngrdDesignVerfication_Design As Boolean = False
    Dim mblngrdInput_Design As Boolean = False
    Dim mblngrdCustSpec_Design As Boolean = False
    Dim mblngrdSealDim_Design As Boolean = False
    Dim mblngrdToolNGag_Manf As Boolean = False
    Dim mblngrdMat_Purchasing As Boolean = False
    Dim mbkngrdDWG_Purchasing As Boolean = False
    Dim mblngrdSplOperation_Qlty As Boolean = False
    Dim mblngrdNeeded_DWG As Boolean = False
    Dim mblngrdBOM_DWG As Boolean = False
    Dim mblngrdIssueComment As Boolean = False


#End Region

#Region "FORM CONSTRUCTOR:"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Populate Combo Boxes:
        '--------------------
        '....Pop Coding.
        With cmbPopCoding.Items
            .Clear()
            .Add("Premier")
            .Add("Select")
        End With

        '....Rating.
        With cmbRating.Items
            .Clear()
            .Add("DO")
            .Add("DX")
            .Add("None")
        End With

        '....Type.
        With cmbType.Items
            .Clear()
            .Add("Quoted")
            .Add("NPL")
            .Add("ECO")
            .Add("Production")
            .Add("Inactive")
            .Add("Replaced")
        End With

        '....PreOrder
        'With cmbMgrPreOrder.Items
        '    .Clear()
        '    .Add("Anthony Wang")
        '    .Add("Bret Sleicher")
        '    .Add("Jeffrey LaBonte")
        '    .Add("Mark Ryals")
        '    .Add("Rob DiMilia")
        '    .Add("Rob Krzanowski")
        'End With

        '....Export_Reqd.
        With cmbExport_Reqd.Items
            .Clear()
            .Add("Y")
            .Add("N")
        End With

        With cmbExport_Status.Items
            .Clear()
            .Add("Submitted")
            .Add("Filed")
        End With

        With cmbPartFamily.Items
            .Clear()
            .Add("E-Seal")
            .Add("C-Seal")
            .Add("S-Seal")
            .Add("U-Seal")
            '.Add("Wire Ring")
            '.Add("Air Duct")
            '.Add("EnerSeal")
            '.Add("PG11")
            '.Add("PG7")
        End With

        With cmbPartType.Items
            .Clear()
            .Add("Custom")
            .Add("Design Guide")
        End With

        With cmbPreOrderSeg.Items
            .Clear()
            .Add("AERO")
            .Add("AUTO")
            .Add("CPI(ChemProcInd)")
            .Add("DIESEL")
            .Add("INDUSTRIAL")
            .Add("IT")
            .Add("LIFE SCIENCE")
            .Add("MILITARY")
            .Add("OIL & GAS")
            .Add("OTHER TRANS")
            .Add("POWER GEN")
            .Add("SEMICON")
        End With

        With cmbPreOrderChannel.Items
            .Clear()
            .Add("Direct")
            .Add("Distribution")
            .Add("TP")
            .Add("ICTP")
        End With

        With cmbCostFileLoc.Items
            .Clear()
            .Add("Product Drive - Part")
            .Add("Product Drive - Customer")
            .Add("Enovia")
        End With

        With cmbRFQPkgLoc.Items
            .Clear()
            .Add("Product Drive - Part")
            .Add("Product Drive - Customer")
            .Add("Enovia")
        End With

        With cmbCost_QuoteFile.Items
            .Clear()
            .Add("Product Drive - Part")
            .Add("Product Drive - Customer")
            .Add("Enovia")
        End With

        With cmbApp_InsertLoc.Items
            .Clear()
            .Add("Face")
            .Add("Axial")
        End With

    End Sub

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub Process_ProductInfo_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===================================================================================================
        SetTabPrivilege()
        GetPartProjectInfo()

        Initialize_tbTesting_Controls()
        InitializeControls()

        PopulateDropDownList()      'AES 28MAY18

        If (gPartProject.PNR.SealType.ToString() = "SC") Then
            PopulateMatComboBox(gPartProject.PNR.SealType.ToString(), cmbDesign_Mat_Spring)
        Else
            PopulateMatComboBox(gPartProject.PNR.SealType.ToString(), cmbDesign_Mat_Seal)
        End If


        Dim pPartEntities As New SealPartDBEntities()

        Dim pPartProjectRecCount As Integer = (From Project In pPartEntities.tblProject
                                               Where Project.fldCustID = mCustomerID And
                                             Project.fldPlatformID = mPlatformID And
                                             Project.fldLocID = mLocationID And
                                             Project.fldPNID = mPNID And
                                             Project.fldRevID = mRevID).Count()

        Dim pPartProjectID As Integer = 0
        If (pPartProjectRecCount > 0) Then
            Dim pCustInfo = (From Project In pPartEntities.tblProject
                             Where Project.fldCustID = mCustomerID And
                                             Project.fldPlatformID = mPlatformID And
                                             Project.fldLocID = mLocationID And
                                             Project.fldPNID = mPNID And
                                             Project.fldRevID = mRevID).First()

            pPartProjectID = pCustInfo.fldID
        End If

        mProcess_Project.ID = pPartProjectID

        RetrieveFromDB()
        DisplayData()
        TabControl1.SelectedIndex = 1
        TabControl1.SelectedIndex = 0

        '''AES 11APR18
        ''grpPreOrderEdited.Enabled = True
        ''chkPreOrderUserSigned.Enabled = True
        ''cmdPreOrderUserSign.Enabled = True

        ''grpExportEdited.Enabled = True
        ''chkITAR_Export_UserSigned.Enabled = True
        ''cmdITAR_Export_UserSign.Enabled = True

        ''grpOrdEntryEdited.Enabled = True
        ''chkOrdEntry_UserSigned.Enabled = True
        ''cmdOrdEntry_UserSign.Enabled = True

        ''grpCostEdited.Enabled = True
        ''chkCost_UserSigned.Enabled = True
        ''cmdCost_UserSign.Enabled = True

        ''grpApp_Face_Edited.Enabled = True
        ''chkApp_UserSigned_Face.Enabled = True
        ''cmdApp_UserSign_Face.Enabled = True

        ''grpDesign_Edited.Enabled = True
        ''chkDesign_UserSigned.Enabled = True
        ''cmdDesign_UserSign.Enabled = True


        '....Move the vertical scrollbar at the Top

        txtParkerPart.Focus()
        txtParkerPart.Select()
        'gIsProcessMainActive = True

        '....Status Bar Panels:
        Dim pWidth As Int32 = (SBar1.Width) / 3

        SBpanel1.Width = pWidth
        SBPanel3.Width = pWidth

        SBpanel1.Text = gUser.FirstName + " " + gUser.LastName
        SBPanel3.Text = "Role: " & gUser.Role
        Dim pCI As New CultureInfo("en-US")

        SBPanel4.Text = Today.ToString(" MMMM dd, yyyy", pCI.DateTimeFormat()) 'US Format only 

    End Sub

    Private Sub Process_frmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        '=======================================================================================
        Dim pCI As New CultureInfo("en-US")

        If (gIsCustomizeTabActive) Then

            If (chkHeaderUserSigned.Checked) Then       'AES 18APR18
                gIsCustomizeTabActive = False
                SetTabPrivilege()
                Initialize_tbTesting_Controls()
                ReInitializeControls()
                TabControl1.Refresh()
            End If

        End If

        If (gIsIssueCommentActive) Then
            gIsIssueCommentActive = False
            With grdIssueComment
                .AllowUserToAddRows = False
                grdIssueComment.Rows.Clear()
                For i As Integer = 0 To mProcess_Project.IssueCommnt.ID.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells(0).Value = mProcess_Project.IssueCommnt.Comment(i)
                    .Rows(i).Cells(1).Value = mProcess_Project.IssueCommnt.ByDept(i)
                    .Rows(i).Cells(2).Value = mProcess_Project.IssueCommnt.ByName(i)
                    .Rows(i).Cells(3).Value = mProcess_Project.IssueCommnt.ByDate(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    .Rows(i).Cells(4).Value = mProcess_Project.IssueCommnt.ToDept(i)
                    .Rows(i).Cells(5).Value = IIf(mProcess_Project.IssueCommnt.Resolved(i), "Y", "N")
                    .Rows(i).Cells(6).Value = mProcess_Project.IssueCommnt.Name(i)
                    If (mProcess_Project.IssueCommnt.DateResolution(i) <> DateTime.MinValue) Then
                        .Rows(i).Cells(7).Value = mProcess_Project.IssueCommnt.DateResolution(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    Else
                        .Rows(i).Cells(7).Value = ""
                    End If

                    .Rows(i).Cells(8).Value = mProcess_Project.IssueCommnt.Resolution(i)
                Next
            End With
            TabControl1.SelectedIndex = 14

        ElseIf (gIsResolutionActive) Then
            gIsResolutionActive = False
            grdIssueComment.Rows.Clear()
            With grdIssueComment
                For i As Integer = 0 To mProcess_Project.IssueCommnt.ID.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells(0).Value = mProcess_Project.IssueCommnt.Comment(i)
                    .Rows(i).Cells(1).Value = mProcess_Project.IssueCommnt.ByDept(i)
                    .Rows(i).Cells(2).Value = mProcess_Project.IssueCommnt.ByName(i)
                    .Rows(i).Cells(3).Value = mProcess_Project.IssueCommnt.ByDate(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    .Rows(i).Cells(4).Value = mProcess_Project.IssueCommnt.ToDept(i)
                    .Rows(i).Cells(5).Value = IIf(mProcess_Project.IssueCommnt.Resolved(i), "Y", "N")
                    .Rows(i).Cells(6).Value = mProcess_Project.IssueCommnt.Name(i)
                    If (mProcess_Project.IssueCommnt.DateResolution(i) <> DateTime.MinValue) Then
                        .Rows(i).Cells(7).Value = mProcess_Project.IssueCommnt.DateResolution(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    Else
                        .Rows(i).Cells(7).Value = ""
                    End If

                    .Rows(i).Cells(8).Value = mProcess_Project.IssueCommnt.Resolution(i)
                Next
            End With

        ElseIf (gIsProcessMainActive = False) Then
            mProcess_Project = gProcessProject.Clone()
            SetLabel_Unit_Cust()
        End If

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub PopulateDropDownList()
        '=============================

        '....Header
        PopulateDropDownList(cmbPopCoding, "tblHeader_PopCoding")
        PopulateDropDownList(cmbRating, "tblHeader_Rating")
        PopulateDropDownList(cmbType, "tblHeader_Type")

        '....Pre-Order
        Dim pCmbColDept_PreOrd As New DataGridViewComboBoxColumn
        pCmbColDept_PreOrd = grdCustContact.Columns.Item(0)
        PopulateDropDownList(pCmbColDept_PreOrd, "tblPreOrder_CustContactDept")

        PopulateDropDownList(cmbExport_Status, "tblPreOrder_ExpComplianceStatus")
        PopulateDropDownList(cmbPartType, "tblPreOrder_PartType")
        PopulateDropDownList(cmbPreOrderSeg, "tblPreOrder_MktSeg")
        PopulateDropDownList(cmbPreOrderChannel, "tblPreOrder_MktChannel")
        PopulateDropDownList(cmbCostFileLoc, "tblPreOrder_Loc")
        PopulateDropDownList(cmbRFQPkgLoc, "tblPreOrder_Loc")

        '....Cost Estimating
        PopulateDropDownList(cmbCost_QuoteFile, "tblPreOrder_Loc")

        Dim pCmbColDesc_Cost As New DataGridViewComboBoxColumn
        pCmbColDesc_Cost = grdCost_SplOperation.Columns.Item(0)
        PopulateDropDownList(pCmbColDesc_Cost, "tblCostEst_SplOpDesc")

        '....Export
        PopulateDropDownList(cmbITAR_Export_Status, "tblExp_Status")
        PopulateDropDownList(cmbITAR_Export_EAR_Classification, "tblExp_EARClassification")

        '....Ord Entry
        Dim pCmbColDept_OrdEntry As New DataGridViewComboBoxColumn
        pCmbColDept_OrdEntry = grdOrdEntry_CustContact.Columns.Item(0)
        PopulateDropDownList(pCmbColDept_OrdEntry, "tblPreOrder_CustContactDept")

        '....Application
        Dim pCmbColCavityDim As New DataGridViewComboBoxColumn
        pCmbColCavityDim = grdApp_Face_Cavity.Columns.Item(0)
        PopulateDropDownList(pCmbColCavityDim, "tblApp_CavityDim")
        PopulateDropDownList(cmbFace_SF_ProcessName, "tblApp_SFinish_Measure")
        PopulateDropDownList(cmbFace_SF_Unit, "tblApp_SFinish_Unit")

        '....Design
        Dim pCmbColDesc_Verify As New DataGridViewComboBoxColumn
        pCmbColDesc_Verify = grdDesign_Verification.Columns.Item(0)
        PopulateDropDownList(pCmbColDesc_Verify, "tblDesign_VerificationDesc")

        Dim pCmbColDesc_Input As New DataGridViewComboBoxColumn
        pCmbColDesc_Input = grdDesign_Input.Columns.Item(0)
        PopulateDropDownList(pCmbColDesc_Input, "tblDesign_InputDesc")

        Dim pCmbColDesc_CustSpec As New DataGridViewComboBoxColumn
        pCmbColDesc_CustSpec = grdDesign_CustSpec.Columns.Item(0)
        PopulateDropDownList(pCmbColDesc_CustSpec, "tblDesign_CustSpecType")

        '....Manufacturing
        Dim pCmbColManfStatus_ToolNGage As New DataGridViewComboBoxColumn
        pCmbColManfStatus_ToolNGage = grdManf_ToolNGage.Columns.Item(3)
        PopulateDropDownList(pCmbColManfStatus_ToolNGage, "tblManf_ToolNGages_Status")

        Dim pCmbColManfDesignResponsibility_ToolNGage As New DataGridViewComboBoxColumn
        pCmbColManfDesignResponsibility_ToolNGage = grdManf_ToolNGage.Columns.Item(5)
        PopulateDropDownList(pCmbColManfDesignResponsibility_ToolNGage, "tblManf_ToolNGages_DesignResponsibility")

        '....Purchasing
        Dim pCmbColPurchase_Unit As New DataGridViewComboBoxColumn
        pCmbColPurchase_Unit = grdPurchase_Mat.Columns.Item(2)
        PopulateDropDownList(pCmbColPurchase_Unit, "tblPurchase_Unit")

        '....Quality
        PopulateDropDownList(cmbQuality_VisualInspection_Type, "tblQlty_VisualInspection")
        PopulateDropDownList(cmbQuality_CustAcceptStd, "tblQlty_CustAcceptStd")

        '....Drawing
        PopulateDropDownList(cmbDwg_DesignLevel, "tblDwg_DesignLevel")

        '....Testing
        PopulateDropDownList(cmbTest_MediaPre_Leak, "tblTest_Medium")
        PopulateDropDownList(cmbTest_MediaPost_Leak, "tblTest_Medium")
        PopulateDropDownList(cmbTest_FreqPre_Leak, "tblTest_Freq")
        PopulateDropDownList(cmbTest_FreqPost_Leak, "tblTest_Freq")




    End Sub

    Private Sub ReInitializeControls()
        '=============================
        InitializeTabs()

        cmdDel_Rec.Enabled = False
        txtParkerPart.ReadOnly = True
        txtPN_Rev.ReadOnly = True
        txtCustomer.ReadOnly = True
        txtCustomerPN.ReadOnly = True
        txtCustomerPN_Rev.ReadOnly = True

        'AES 24APR18
        If (cmbExport_Reqd.Text = "N") Then
            cmbExport_Status.SelectedIndex = -1
            cmbExport_Status.Enabled = False
        Else
            If (mPreOrder) Then
                cmbExport_Status.Enabled = True
            End If

        End If

        '....Export
        If (cmbITAR_Export_ProductITAR_Reg.Text = "N") Then
            txtITAR_Export_ITAR_Classification.Enabled = False
            cmbITAR_Export_Status.SelectedIndex = -1
            cmbITAR_Export_Status.Enabled = False
            txtExportControlled.Text = "N"
        Else
            If (mExport) Then
                txtITAR_Export_ITAR_Classification.Enabled = True
                cmbITAR_Export_Status.Enabled = True
                txtExportControlled.Text = "Y"
            End If

        End If

        If (cmbITAR_Export_SaleExportControlled.Text = "N") Then
            cmbITAR_Export_EAR_Classification.Enabled = False
        Else
            If (mExport) Then
                cmbITAR_Export_EAR_Classification.Enabled = True
            End If

        End If

        If (mDesign) Then
            chkNewRef_Dim.Enabled = False
            txtParkerPN_Part1_NewRef_Dim.Enabled = False
            cmbParkerPN_Part2_NewRef_Dim.Enabled = True
            txtParkerPN_Part3_NewRef_Dim.Enabled = False
            txtPN_PH_Rev_NewRef_Dim.Enabled = False
            chkNewRef_Notes.Enabled = False
            txtParkerPN_Part1_Notes_Dim.Enabled = False
            cmbParkerPN_Part2_Notes_Dim.Enabled = True
            txtParkerPN_Part3_Notes_Dim.Enabled = False
            txtParkerPN_Rev_Notes_Dim.Enabled = False
            chkLegacyRef_Dim.Enabled = False
            txtLegacyRef_Dim.Enabled = False
            txtLegacyRef_Dim_Rev.Enabled = False
            chkLegacyRef_Notes.Enabled = False
            txtLegacyRef_Notes.Enabled = False
            txtLegacyRef_Notes_Rev.Enabled = False
        End If

        Dim pSealType As String = gPartProject.PNR.SealType.ToString()

        '....Coating
        If (pSealType = "E") Then
            If (mDesign) Then
                grpCoating.Enabled = True
            End If


            If gPartProject.PNR.HW.Coating <> "None" Then
                chkCoating.Checked = True
            Else
                chkCoating.Checked = False
                If (mDesign) Then
                    cmbCoating.Enabled = False
                End If

            End If

            '........Populate Surface Finish Combo Box.
            PopulateCmbSFinish()

            If gPartProject.PNR.HW.Coating <> "" And gPartProject.PNR.HW.Coating <> "None" Then
                If (mDesign) Then
                    cmbCoating.Enabled = True
                End If

                cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
                cmbCoating.Text = gPartProject.PNR.HW.Coating

                If cmbCoating.Text = "T800" Then
                    If (mDesign) Then
                        lblSFinish.Enabled = True
                        cmbSFinish.Enabled = True
                    End If

                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                Else
                    If (mDesign) Then
                        lblSFinish.Enabled = False
                        cmbSFinish.Enabled = False
                    End If

                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                    cmbSFinish.Text = ""
                End If

            Else
                If (mDesign) Then
                    cmbCoating.Enabled = False
                    lblSFinish.Enabled = False
                    cmbSFinish.Enabled = False
                End If

                cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
                cmbCoating.Text = ""

                cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                cmbSFinish.Text = ""
            End If

            If cmbSFinish.Items.Count > 0 Then
                If gPartProject.PNR.HW.SFinish > gcEPS Then
                    cmbSFinish.Text = gPartProject.PNR.HW.SFinish

                Else
                    cmbSFinish.SelectedIndex = 0

                End If
            End If

        Else
            If (mDesign) Then
                grpCoating.Enabled = False
            End If

        End If

        '....Plating
        If (pSealType = "C" Or pSealType = "SC") Then
            If (mDesign) Then
                grpPlating.Enabled = True
                cmbPlatingCode.Enabled = True
                cmbPlatingThickCode.Enabled = True

            End If

            'chkPlating.Checked = False
            chkPlating.Enabled = False      'AES 17APR18

            'AES 25APR18
            If (mManf And pSealType = "SC") Then
                txtManf_MatPartNo_Spring.Enabled = True
            Else
                txtManf_MatPartNo_Spring.Enabled = False
            End If

        Else
            txtManf_MatPartNo_Spring.Enabled = False
            If (mDesign) Then
                grpPlating.Enabled = False
            End If

        End If

        'AES 24APR18

        '....Application
        grpSegment.Enabled = False

        If (cmbApp_PressCycle.Text = "N") Then
            txtApp_PressCycleFreq.Text = ""
            txtApp_PressCycleAmp.Text = ""
            txtApp_PressCycleFreq.Enabled = False
            txtApp_PressCycleAmp.Enabled = False
        Else
            If (mApp) Then
                txtApp_PressCycleFreq.Enabled = True
                txtApp_PressCycleAmp.Enabled = True
            Else
                txtApp_PressCycleFreq.Enabled = False
                txtApp_PressCycleAmp.Enabled = False
            End If

        End If

        If (cmbDesign_Winnovation.Text = "N") Then
            txtDesign_WinnovationNo.Enabled = False
        Else
            If (mDesign) Then
                txtDesign_WinnovationNo.Enabled = True
            Else
                txtDesign_WinnovationNo.Enabled = False
            End If

        End If

        If (cmbQuality_CustComplaint.Text = "N") Then
            txtQuality_Reason.Text = ""
            txtQuality_Reason.Enabled = False
        Else
            If (mQlty) Then
                txtQuality_Reason.Enabled = True
            Else
                txtQuality_Reason.Enabled = False
            End If

        End If

        If (cmbQuality_VisualInspection.Text = "N") Then
            cmbQuality_VisualInspection_Type.Text = ""
            cmbQuality_VisualInspection_Type.Enabled = False
        Else
            cmbQuality_VisualInspection_Type.Text = ""
            If (mQlty) Then
                cmbQuality_VisualInspection_Type.Enabled = True
            Else
                cmbQuality_VisualInspection_Type.Enabled = False
            End If

        End If

        If (mTest) Then
            If (chkTest.Checked) Then
                EnableTab(tabLeak, True)
                EnableTab(tabLoad, True)
                EnableTab(tabSpringBack, True)
                txtTest_Other.Enabled = True
            Else
                EnableTab(tabLeak, False)
                EnableTab(tabLoad, False)
                EnableTab(tabSpringBack, False)
                txtTest_Other.Enabled = False
            End If

        End If

    End Sub

    Private Sub SetTabPrivilege()
        '=======================
        Dim pRoleID As Integer = gUser.GetRoleID(gUser.Role)

        Dim pSealProcessEntities As New SealProcessDBEntities()

        Dim pUserRolePrivilege = (From pRec In pSealProcessEntities.tblRolePrivilege
                                  Where pRec.fldRoleID = pRoleID Select pRec).ToList()

        If (pUserRolePrivilege.Count > 0) Then
            mHeader = pUserRolePrivilege(0).fldHeader
            mPreOrder = pUserRolePrivilege(0).fldPreOrder
            mExport = pUserRolePrivilege(0).fldExport
            mOrdEntry = pUserRolePrivilege(0).fldOrdEntry
            mCost = pUserRolePrivilege(0).fldCost
            mApp = pUserRolePrivilege(0).fldApp
            mDesign = pUserRolePrivilege(0).fldDesign
            mManf = pUserRolePrivilege(0).fldManf
            mPurchase = pUserRolePrivilege(0).fldPurchase
            mQlty = pUserRolePrivilege(0).fldQlty
            mDwg = pUserRolePrivilege(0).fldDwg
            mTest = pUserRolePrivilege(0).fldTest
            mPlanning = pUserRolePrivilege(0).fldPlanning
            mShipping = pUserRolePrivilege(0).fldShipping
            mKeyChar = pUserRolePrivilege(0).fldKeyChar
        End If

        '''AES 27FEB18
        ''If (gUser.Role = "Admin") Then
        ''    mHeader = True
        ''    mPreOrder = True
        ''    mExport = True
        ''    mOrdEntry = True
        ''    mCost = True
        ''    mApp = True
        ''    mDesign = True
        ''    mManf = True
        ''    mPurchase = True
        ''    mQlty = True
        ''    mDwg = True
        ''    mTest = True
        ''    mPlanning = True
        ''    mShipping = True
        ''    mKeyChar = True
        ''End If
        mTabIndex.Clear()       'AES 04APR18

        If (mHeader) Then
            mTabIndex.Add(99)
        End If

        If (mPreOrder) Then
            mTabIndex.Add(0)
        End If
        If (mExport) Then
            mTabIndex.Add(1)
        End If
        If (mOrdEntry) Then
            mTabIndex.Add(2)
        End If
        If (mCost) Then
            mTabIndex.Add(3)
        End If
        If (mApp) Then
            mTabIndex.Add(4)
        End If
        If (mDesign) Then
            mTabIndex.Add(5)
        End If
        If (mManf) Then
            mTabIndex.Add(6)
        End If
        If (mPurchase) Then
            mTabIndex.Add(7)
        End If
        If (mQlty) Then
            mTabIndex.Add(8)
        End If
        If (mDwg) Then
            mTabIndex.Add(9)
        End If
        If (mTest) Then
            mTabIndex.Add(10)
        End If
        If (mPlanning) Then
            ''mTabIndex.Add(11)
        End If
        If (mShipping) Then
            mTabIndex.Add(12)
        End If
        If (mKeyChar) Then
            ''mTabIndex.Add(13)
        End If

        'mTabIndex.Add(15)   'AES 02APR18

        If (gUser.Role <> "Viewer") Then
            mIssue = True
        End If

    End Sub

    Private Sub InitializeControls()
        '===========================
        ''....Status Bar Panels:

        'Dim pWidth As Int32 = (SBar1.Width) / 3

        'SBpanel1.Width = pWidth
        'SBPanel3.Width = pWidth

        'SBpanel1.Text = gUser.FirstName + " " + gUser.LastName
        'SBPanel3.Text = "Role: " & gUser.Role
        'Dim pCI As New CultureInfo("en-US")
        ''SBPanel4.Text = Today.DayOfWeek.ToString() & ", " &
        ''                Today.ToString(" MMMM dd, yyyy", pCI.DateTimeFormat()) 'US Format only
        'SBPanel4.Text = Today.ToString(" MMMM dd, yyyy", pCI.DateTimeFormat()) 'US Format only 'Today.ToString("dd MMM yyyy", pCI.DateTimeFormat()) 'US Format only
        ''--------------------------------------------------------------------------------------

        InitializeTabs()        'AES 01MAR18

        'cmdSealPart.Enabled = True
        cmdDel_Rec.Enabled = False
        txtParkerPart.ReadOnly = True
        txtPN_Rev.ReadOnly = True
        txtCustomer.ReadOnly = True
        txtCustomerPN.ReadOnly = True
        txtCustomerPN_Rev.ReadOnly = True

        txtParkerPart.Text = ""
        txtPN_Rev.Text = ""
        txtCustomer.Text = ""
        txtCustomerPN.Text = ""
        txtCustomerPN_Rev.Text = ""

        '....Set cmbBoxes to "N" as default
        'cmbGovt.Text = "N"
        cmbExport_Reqd.Text = "N"
        cmbITAR_Export_CustOnDenialList.Text = "N"
        cmbITAR_Export_CountryProhibited.Text = "N"
        cmbITAR_Export_AntiBoycottLang.Text = "N"
        cmbITAR_Export_ProductITAR_Reg.Text = "N"
        cmbITAR_Export_SaleExportControlled.Text = "N"
        cmbOrdEntry_SpecialReq.Text = "N"
        cmbOrdEntry_Tooling.Text = "N"
        cmbOrdEntry_SplPkgNLbl.Text = "N"
        cmbOrdEntry_Expedited.Text = "N"
        cmbOrdEntry_DFAR.Text = "N"

        cmbApp_PressCycle.Text = "N"
        cmbApp_Shaped.Text = "N"
        cmbApp_OutOfRound.Text = "N"
        cmbApp_SplitRing.Text = "N"
        cmbApp_PreComp.Text = "N"
        cmbApp_Static_Axial.Text = "N"
        cmbApp_Rotate_Axial.Text = "N"
        cmbApp_Recip_Axial.Text = "N"
        cmbApp_Osc_Axial.Text = "N"

        cmbDesign_Frozen.Text = "N"
        cmbDesign_Process.Text = "N"
        cmbDesign_Class1.Text = "N"
        cmbDesign_BuildToPrint.Text = "N"
        cmbDesign_Winnovation.Text = "N"
        cmbDesign_OutsideVendor.Text = "N"

        cmbQuality_ApprovedSupplier.Text = "N"
        cmbQuality_TNG.Text = "N"
        cmbQuality_CustComplaint.Text = "N"
        cmbQuality_VisualInspection.Text = "N"
        cmbQuality_SPC.Text = "N"
        cmbQuality_GageRnR_Reqd.Text = "N"

        cmbPopCoding.SelectedIndex = 0
        cmbRating.SelectedIndex = 0
        cmbType.SelectedIndex = 1   '0 23APR18
        cmbExport_Status.SelectedIndex = 0
        cmbPartFamily.SelectedIndex = 0
        cmbPartType.SelectedIndex = 0
        cmbPreOrderChannel.SelectedIndex = 1

        '....Pre-Order
        PopulateMarketingMgr()

        'If (cmbExport_Reqd.Text = "N") Then
        '    cmbExport_Status.SelectedIndex = -1
        '    cmbExport_Status.Enabled = False
        'Else
        '    cmbExport_Status.Enabled = True
        'End If

        Dim pCmbColDept_PreOrd As New DataGridViewComboBoxColumn
        pCmbColDept_PreOrd = grdCustContact.Columns.Item(0)
        pCmbColDept_PreOrd.Items.Add("Engineering")
        pCmbColDept_PreOrd.Items.Add("Purchasing")
        pCmbColDept_PreOrd.Items.Add("Distributor")
        pCmbColDept_PreOrd.Items.Add("Quality")
        pCmbColDept_PreOrd.Items.Add("Service")

        '....Ord Entry
        Dim pCmbColDept_OrdEntry As New DataGridViewComboBoxColumn
        pCmbColDept_OrdEntry = grdOrdEntry_CustContact.Columns.Item(0)
        pCmbColDept_OrdEntry.Items.Add("Engineering")
        pCmbColDept_OrdEntry.Items.Add("Purchasing")
        pCmbColDept_OrdEntry.Items.Add("Distributor")
        pCmbColDept_OrdEntry.Items.Add("Quality")
        pCmbColDept_OrdEntry.Items.Add("Service")

        '....Application
        'cmbApp_InsertLoc.Text = "Face"
        mProcess_Project.App.Type = "Face"
        cmbApp_InsertLoc.SelectedIndex = 0
        cmbFace_SF_ProcessName.SelectedIndex = 0
        cmbFace_SF_Unit.SelectedIndex = 0
        cmbAxial_SF_ProcessName.SelectedIndex = 0
        cmbAxial_SF_Unit.SelectedIndex = 0
        cmbApp_Face_POrient.SelectedIndex = 0

        '....Design Tab
        With cmbParkerPN_Part2_NewRef_Dim.Items
            .Clear()
            .Add("69")
            .Add("76")
            .Add("79")
        End With

        With cmbParkerPN_Part2_Notes_Dim.Items
            .Clear()
            .Add("69")
            .Add("76")
            .Add("79")
        End With



        Dim pSealType As String = gPartProject.PNR.SealType.ToString()

        Dim pCmbColDesign_Seal As New DataGridViewComboBoxColumn
        pCmbColDesign_Seal = grdDesign_Seal.Columns.Item(0)

        If (pSealType = "E") Then
            pCmbColDesign_Seal.Items.Add("OD")
            pCmbColDesign_Seal.Items.Add("OR")
            pCmbColDesign_Seal.Items.Add("ID")
            pCmbColDesign_Seal.Items.Add("IR")
            pCmbColDesign_Seal.Items.Add("Roundness")
            pCmbColDesign_Seal.Items.Add("Flatness")
            pCmbColDesign_Seal.Items.Add("OoR")
            pCmbColDesign_Seal.Items.Add("Thickness")
            pCmbColDesign_Seal.Items.Add("FH")
            pCmbColDesign_Seal.Items.Add("Radial Wid")
            pCmbColDesign_Seal.Items.Add("Pre Comp Height")
            pCmbColDesign_Seal.Items.Add("Pre Comp Width")
            pCmbColDesign_Seal.Items.Add("Segment Len")
            pCmbColDesign_Seal.Items.Add("Initial Cut Len")
            pCmbColDesign_Seal.Items.Add("Segment Gap")

        ElseIf (pSealType = "C" Or pSealType = "SC") Then
            pCmbColDesign_Seal.Items.Add("OD")
            pCmbColDesign_Seal.Items.Add("OR")
            pCmbColDesign_Seal.Items.Add("ID")
            pCmbColDesign_Seal.Items.Add("IR")
            pCmbColDesign_Seal.Items.Add("Roundness")
            pCmbColDesign_Seal.Items.Add("Flatness")
            pCmbColDesign_Seal.Items.Add("OoR")
            pCmbColDesign_Seal.Items.Add("Thickness")
            pCmbColDesign_Seal.Items.Add("FH")
            pCmbColDesign_Seal.Items.Add("Radial Wid")

        End If

        ''....Application
        'grpSegment.Enabled = False

        If (mDesign) Then
            chkNewRef_Dim.Enabled = False
            txtParkerPN_Part1_NewRef_Dim.Enabled = False
            cmbParkerPN_Part2_NewRef_Dim.Enabled = True
            txtParkerPN_Part3_NewRef_Dim.Enabled = False
            txtPN_PH_Rev_NewRef_Dim.Enabled = False
            chkNewRef_Notes.Enabled = False
            txtParkerPN_Part1_Notes_Dim.Enabled = False
            cmbParkerPN_Part2_Notes_Dim.Enabled = True
            txtParkerPN_Part3_Notes_Dim.Enabled = False
            txtParkerPN_Rev_Notes_Dim.Enabled = False
            chkLegacyRef_Dim.Enabled = False
            txtLegacyRef_Dim.Enabled = False
            txtLegacyRef_Dim_Rev.Enabled = False
            chkLegacyRef_Notes.Enabled = False
            txtLegacyRef_Notes.Enabled = False
            txtLegacyRef_Notes_Rev.Enabled = False

            ''AES 24APR18
            'If (cmbDesign_Winnovation.Text = "N") Then
            '    txtDesign_WinnovationNo.Enabled = False
            'Else
            '    txtDesign_WinnovationNo.Enabled = True
            'End If
        End If


        '....Manf
        Dim pCmbColManf_ToolNGage As New DataGridViewComboBoxColumn
        pCmbColManf_ToolNGage = grdManf_ToolNGage.Columns.Item(1)
        With pCmbColManf_ToolNGage
            .Items.Add("Strip gauge")
            .Items.Add("Roll tooling")
            .Items.Add("Triroll")
            .Items.Add("Die")
            .Items.Add("Spin polish fixture(s)")
            .Items.Add("Stretch tooling")
            .Items.Add("Template")
            .Items.Add("Window gauge")
            .Items.Add("Precompression fixture")
            .Items.Add("Inspection gauge")
            .Items.Add("Test fixture")
        End With

        '........Status
        Dim pCmbColManfStatus_ToolNGage As New DataGridViewComboBoxColumn
        pCmbColManfStatus_ToolNGage = grdManf_ToolNGage.Columns.Item(3)
        With pCmbColManfStatus_ToolNGage
            .Items.Add("Inventory")
            .Items.Add("Make")
            .Items.Add("Buy")
        End With

        '........Design Responsibility
        Dim pCmbColManfDesignResponsibility_ToolNGage As New DataGridViewComboBoxColumn
        pCmbColManfDesignResponsibility_ToolNGage = grdManf_ToolNGage.Columns.Item(5)
        With pCmbColManfDesignResponsibility_ToolNGage
            .Items.Add("Mfg")
            .Items.Add("Eng")
            .Items.Add("Drawing")
        End With

        '....Purchasing
        Dim pCmbColPurchase_Unit As New DataGridViewComboBoxColumn
        pCmbColPurchase_Unit = grdPurchase_Mat.Columns.Item(2)
        With pCmbColPurchase_Unit
            .Items.Add("lb")
            .Items.Add("ft")
        End With

        '........Coating
        If (pSealType = "E") Then
            If (mDesign) Then
                grpCoating.Enabled = True
            End If

            '....Populate Coating Combo Box:
            With cmbCoating
                .Items.Clear()
                .Items.Add("Tricom")
                .Items.Add("T800")

                .SelectedIndex = 0
            End With

            If gPartProject.PNR.HW.Coating <> "None" Then
                chkCoating.Checked = True
            Else
                chkCoating.Checked = False
                If (mDesign) Then
                    cmbCoating.Enabled = False
                End If

            End If

            '........Populate Surface Finish Combo Box.
            PopulateCmbSFinish()

            If gPartProject.PNR.HW.Coating <> "" And gPartProject.PNR.HW.Coating <> "None" Then
                If (mDesign) Then
                    cmbCoating.Enabled = True
                End If

                cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
                cmbCoating.Text = gPartProject.PNR.HW.Coating

                If cmbCoating.Text = "T800" Then
                    If (mDesign) Then
                        lblSFinish.Enabled = True
                        cmbSFinish.Enabled = True
                    End If

                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                Else
                    If (mDesign) Then
                        lblSFinish.Enabled = False
                        cmbSFinish.Enabled = False
                    End If

                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                    cmbSFinish.Text = ""
                End If

            Else
                If (mDesign) Then
                    cmbCoating.Enabled = False
                    lblSFinish.Enabled = False
                    cmbSFinish.Enabled = False
                End If

                cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
                cmbCoating.Text = ""

                cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                cmbSFinish.Text = ""
            End If

            If cmbSFinish.Items.Count > 0 Then
                If gPartProject.PNR.HW.SFinish > gcEPS Then
                    cmbSFinish.Text = gPartProject.PNR.HW.SFinish

                Else
                    cmbSFinish.SelectedIndex = 0

                End If
            End If

        Else
            If (mDesign) Then
                grpCoating.Enabled = False
            End If

        End If

        '....Plating
        If (pSealType = "C" Or pSealType = "SC") Then
            If (mDesign) Then
                grpPlating.Enabled = True
                cmbPlatingCode.Enabled = True
                cmbPlatingThickCode.Enabled = True
            End If

            chkPlating.Checked = False


        Else
            If (mDesign) Then
                grpPlating.Enabled = False
            End If

        End If

        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        '....Issue Comments
        Dim pCmbToDept_IssueComment As New DataGridViewComboBoxColumn
        pCmbToDept_IssueComment = grdIssueComment.Columns.Item(4)
        With pCmbToDept_IssueComment
            Dim pQryRole_User = (From pRec In pSealSuiteEntities.tblRole Where pRec.fldIsSuperRole = 0
                                 Select pRec).ToList()
            If (pQryRole_User.Count > 0) Then
                For i As Integer = 0 To pQryRole_User.Count - 1
                    .Items.Add(pQryRole_User(i).fldRole)
                Next
            End If

        End With


        '....Attendies
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole
                        Where pRec.fldRole <> "Admin" Select pRec).ToList()
        If (pQryRole.Count > 0) Then
            For i As Integer = 0 To pQryRole.Count - 1
                grdApproval_Attendees.Rows.Add()
            Next
        End If

        '''....Attendies
        ''Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole
        ''                Where pRec.fldRole <> "Admin" Select pRec).ToList()
        ''If (pQryRole.Count > 0) Then
        ''    For i As Integer = 0 To pQryRole.Count - 1
        ''        grdApproval_Attendees.Rows.Add()
        ''    Next

        ''    For i As Integer = 0 To pQryRole.Count - 1
        ''        grdApproval_Attendees.Rows(i).Cells(0).Value = pQryRole(i).fldRole
        ''        Dim pRoleID As Integer = pQryRole(i).fldID

        ''        Dim dgvcc As New DataGridViewComboBoxCell

        ''        Dim pQryUserRole = (From pRec In pSealSuiteEntities.tblProcess_UserRole
        ''                            Where pRec.fldRoleID = pRoleID Select pRec).ToList()

        ''        If (pQryUserRole.Count > 0) Then
        ''            Dim pListUserID As New List(Of Integer)
        ''            For j As Integer = 0 To pQryUserRole.Count - 1
        ''                Dim pUserID As Integer = pQryUserRole(j).fldUserID
        ''                If (Not pListUserID.Contains(pUserID)) Then
        ''                    pListUserID.Add(pUserID)
        ''                End If

        ''            Next

        ''            For k As Integer = 0 To pListUserID.Count - 1
        ''                Dim pID As Integer = pListUserID(k)
        ''                Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser
        ''                                Where pRec.fldID = pID Select pRec).ToList()

        ''                If (pQryUser.Count > 0) Then
        ''                    Dim pUserName As String = pQryUser(0).fldFirstName & " " & pQryUser(0).fldLastName
        ''                    If (Not mUserName.Contains(pUserName)) Then
        ''                        mUserName.Add(pUserName)
        ''                        mUserID.Add(pID)
        ''                    End If

        ''                    dgvcc.Items.Add(pUserName)

        ''                End If

        ''            Next

        ''            grdApproval_Attendees.Item(1, i) = dgvcc

        ''        End If
        ''    Next

        ''End If

        ''....ToDo..
        'If (Not dgvcc.Items.Contains("Adam ")) Then
        '    dgvcc.Items.Add("Adam ")
        'End If

        'grdApproval_Attendees.Item(1, i) = dgvcc1

        'Try


        '    grdApproval_Attendees.Item(1, i) = dgvcc

        '    Dim dgvcc1 As New DataGridViewComboBoxCell
        '    dgvcc1 = grdApproval_Attendees.Item(1, i)
        '    '....ToDo..
        '    If (Not dgvcc1.Items.Contains("Adam ")) Then
        '        dgvcc1.Items.Add("Adam ")
        '    End If

        '    grdApproval_Attendees.Item(1, i) = dgvcc1
        'Catch ex As Exception

        'End Try

        grdApproval_Attendees.AllowUserToAddRows = False

    End Sub

    Private Sub InitializeTabs()
        '=======================
        grpParker.Enabled = mHeader
        grpCust.Enabled = mHeader
        cmbRating.Enabled = mHeader
        lblRating.Enabled = mHeader
        cmbType.Enabled = mHeader
        lblType.Enabled = mHeader
        lblStatus.Enabled = mHeader
        grpDate.Enabled = mHeader
        cmdSetUnits.Enabled = mHeader
        cmdDelete.Enabled = mHeader

        'AES 18APR18
        If (mTabIndex.Contains(99)) Then
            chkHeaderUserSigned.Enabled = True
        Else
            chkHeaderUserSigned.Enabled = False
        End If


        EnableTab(tabPreOrder_P1, mPreOrder)
        EnableTab(tabPreOrder_P2, mPreOrder)
        txtPreOrderUserDate.Enabled = False
        dtpPreOrderUserDate.Enabled = False
        txtPreOrderUserName.Enabled = False
        If (mTabIndex.Contains(0)) Then
            grpPreOrderEdited.Enabled = True
            chkPreOrderUserSigned.Enabled = True
            cmdPreOrderUserSign.Enabled = True
        Else
            grpPreOrderEdited.Enabled = False
        End If

        EnableTab(tabExport, mExport)
        txtITAR_Export_UserDate.Enabled = False
        dtpITAR_Export_UserDate.Enabled = False
        txtITAR_Export_UserName.Enabled = False
        If (mTabIndex.Contains(1)) Then
            grpExportEdited.Enabled = True
        Else
            grpExportEdited.Enabled = False
        End If
        chkITAR_Export_UserSigned.Enabled = True
        cmdITAR_Export_UserSign.Enabled = True

        EnableTab(tabOrder, mOrdEntry)
        txtOrdEntry_UserDate.Enabled = False
        dtpOrdEntry_UserDate.Enabled = False
        txtOrdEntry_UserName.Enabled = False
        If (mTabIndex.Contains(2)) Then
            grpOrdEntryEdited.Enabled = True
        Else
            grpOrdEntryEdited.Enabled = False
        End If
        chkOrdEntry_UserSigned.Enabled = True
        cmdOrdEntry_UserSign.Enabled = True

        EnableTab(tabCosting, mCost)
        txtCost_UserDate.Enabled = False
        dtpCost_UserDate.Enabled = False
        txtCost_UserName.Enabled = False
        If (mTabIndex.Contains(3)) Then
            grpCostEdited.Enabled = True
        Else
            grpCostEdited.Enabled = False
        End If
        chkCost_UserSigned.Enabled = True
        cmdCost_UserSign.Enabled = True

        EnableTab(tbpGen, mApp)
        EnableTab(tbpFace, mApp)
        EnableTab(tbpAxial, mApp)
        txtApp_UserDate_Face.Enabled = False
        dtpApp_UserDate_Face.Enabled = False
        txtApp_UserName_Face.Enabled = False
        If (mTabIndex.Contains(4)) Then
            grpApp_Face_Edited.Enabled = True
        Else
            grpApp_Face_Edited.Enabled = False
        End If
        chkApp_UserSigned_Face.Enabled = True
        cmdApp_UserSign_Face.Enabled = True


        EnableTab(tbpDesign_P1, mDesign)
        EnableTab(tbpDesign_P2, mDesign)
        txtDesign_UserDate.Enabled = False
        dtpDesign_UserDate.Enabled = False
        txtDesign_UserName.Enabled = False
        If (mTabIndex.Contains(5)) Then
            grpDesign_Edited.Enabled = True
        Else
            grpDesign_Edited.Enabled = False
        End If
        chkDesign_UserSigned.Enabled = True
        cmdDesign_UserSign.Enabled = True

        EnableTab(tabManufacturing, mManf)
        txtManf_UserDate.Enabled = False
        dtpManf_UserDate.Enabled = False
        txtManf_UserName.Enabled = False
        If (mTabIndex.Contains(6)) Then
            grpManf_Edited.Enabled = True
        Else
            grpManf_Edited.Enabled = False
        End If
        chkManf_UserSigned.Enabled = True
        cmdManf_UserSign.Enabled = True

        EnableTab(tabPurchasing, mPurchase)
        txtPurchase_UserDate.Enabled = False
        dtpPurchase_UserDate.Enabled = False
        txtPurchase_UserName.Enabled = False
        If (mTabIndex.Contains(7)) Then
            grpPurchase_Edited.Enabled = True
        Else
            grpPurchase_Edited.Enabled = False
        End If
        chkPurchase_UserSigned.Enabled = True
        cmdPurchase_UserSign.Enabled = True

        EnableTab(tabQuality, mQlty)
        txtQuality_UserDate.Enabled = False
        dtpQuality_UserDate.Enabled = False
        txtQuality_UserName.Enabled = False
        If (mTabIndex.Contains(8)) Then
            grpQlty_Edited.Enabled = True
        Else
            grpQlty_Edited.Enabled = False
        End If
        chkQuality_UserSigned.Enabled = True
        cmdQuality_UserSign.Enabled = True

        EnableTab(tabDrawing, mDwg)
        txtDwg_UserDate.Enabled = False
        dtpDwg_UserDate.Enabled = False
        txtDwg_UserName.Enabled = False
        If (mTabIndex.Contains(9)) Then
            grpDwg_Edited.Enabled = True
        Else
            grpDwg_Edited.Enabled = False
        End If
        chkDwg_UserSigned.Enabled = True
        cmdDwg_UserSign.Enabled = True

        chkTest.Enabled = mTest
        grpUnit_Test.Enabled = mTest
        txtTest_Other.Enabled = mTest
        EnableTab(tabLeak, mTest)
        EnableTab(tabLoad, mTest)
        EnableTab(tabSpringBack, mTest)
        txtTest_UserDate.Enabled = False
        dtpTest_UserDate.Enabled = False
        txtTest_UserName.Enabled = False
        If (mTabIndex.Contains(10)) Then
            grpTest_Edited.Enabled = True
        Else
            grpTest_Edited.Enabled = False
        End If
        chkTest_UserSigned.Enabled = True
        cmdTest_UserSign.Enabled = True

        EnableTab(tabPlanning, False)
        txtPlanning_UserDate.Enabled = False
        dtpPlanning_UserDate.Enabled = False
        txtPlanning_UserName.Enabled = False
        If (mTabIndex.Contains(11)) Then
            grpPlanning_Edited.Enabled = True
        Else
            grpPlanning_Edited.Enabled = False
        End If
        chkPlanning_UserSigned.Enabled = True
        cmdPlanning_UserSign.Enabled = True

        EnableTab(tabShipping, mShipping)
        txtShipping_UserDate.Enabled = False
        dtpShipping_UserDate.Enabled = False
        txtShipping_UserName.Enabled = False
        If (mTabIndex.Contains(12)) Then
            grpShipping_Edited.Enabled = True
        Else
            grpShipping_Edited.Enabled = False
        End If
        chkShipping_UserSigned.Enabled = True
        cmdShipping_UserSign.Enabled = True

        EnableTab(tabKeyChar, False)

        'EnableTab(tabApproval, True)        'AES 02APR18
        IsApprovalTab_Enabled()        'AES 12APR18

        If (gUser.Role = "Viewer") Then
            EnableTab(tabIssue, False)
            cmdRiskAna.Enabled = False
            cmdIssueComment.Enabled = False
            'cmdSealPart.Enabled = False
        Else
            EnableTab(tabIssue, True)

            cmdIssueComment.Enabled = True      'AES 23APR18
            cmdRiskAna.Enabled = True
        End If

    End Sub

    Private Function IsApprovalTab_Enabled() As Boolean
        '==============================================    'AES 12APR18
        Dim pApproval As Boolean = False

        If (chkPreOrderUserSigned.Checked And chkITAR_Export_UserSigned.Checked And chkOrdEntry_UserSigned.Checked And
            chkCost_UserSigned.Checked And chkApp_UserSigned_Face.Checked And chkDesign_UserSigned.Checked And
            chkManf_UserSigned.Checked And chkPurchase_UserSigned.Checked And chkQuality_UserSigned.Checked And
            chkDwg_UserSigned.Checked And chkTest_UserSigned.Checked And chkShipping_UserSigned.Checked) Then

            'chkPlanning_UserSigned.Checked And
            If (gUser.Role <> "Viewer") Then        'AES 16APR18
                If (Not mTabIndex.Contains(15)) Then
                    mTabIndex.Add(15)
                End If

                EnableTab(tabApproval, True)
                pApproval = True
            End If

        Else
            If (mTabIndex.Contains(15)) Then
                mTabIndex.Remove(15)
            End If
            EnableTab(tabApproval, False)
            pApproval = False
        End If

        'TabControl1.Refresh()
        Return pApproval

    End Function

    Private Sub GetPartProjectInfo()
        '===========================
        Dim pPartEntities As New SealPartDBEntities()
        Dim pQryProject = (From it In pPartEntities.tblProject
                           Where it.fldID = gPartProject.Project_ID Select it).ToList()

        If (pQryProject.Count() > 0) Then
            mPNID = pQryProject(0).fldPNID
            mRevID = pQryProject(0).fldRevID
            mCustomerID = pQryProject(0).fldCustID
            mPlatformID = pQryProject(0).fldPlatformID
            mLocationID = pQryProject(0).fldLocID

            RetrieveFromDB_PartProj()
        End If

    End Sub

    Private Sub Initialize_tbTesting_Controls()
        '======================================
        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            If (gPartProject.PNR.HW.Plating.Exists) Then
                '....Leak
                txtTest_CompressPost_Leak.Enabled = True
                cmbTest_MediaPost_Leak.Enabled = True
                txtTest_PressPost_Leak.Enabled = True
                txtTest_ReqPost_Leak.Enabled = True
                cmbTest_QtyPost_Leak.Enabled = True
                cmbTest_FreqPost_Leak.Enabled = True

                '....Load
                txtTest_CompressPost_Load.Enabled = True
                txtTest_ReqPost_Load.Enabled = True
                cmbTest_QtyPost_Load.Enabled = True
                cmbTest_FreqPost_Load.Enabled = True

                '....SpringBack
                txtTest_CompressPost_SpringBack.Enabled = True
                txtTest_ReqPost_SpringBack.Enabled = True
                cmbTest_QtyPost_SpringBack.Enabled = True
                cmbTest_FreqPost_SpringBack.Enabled = True
            Else
                '....Leak
                txtTest_CompressPost_Leak.Enabled = False
                cmbTest_MediaPost_Leak.Enabled = False
                txtTest_PressPost_Leak.Enabled = False
                txtTest_ReqPost_Leak.Enabled = False
                cmbTest_QtyPost_Leak.Enabled = False
                cmbTest_FreqPost_Leak.Enabled = False

                '....Load
                txtTest_CompressPost_Load.Enabled = False
                txtTest_ReqPost_Load.Enabled = False
                cmbTest_QtyPost_Load.Enabled = False
                cmbTest_FreqPost_Load.Enabled = False

                '....SpringBack
                txtTest_CompressPost_SpringBack.Enabled = False
                txtTest_ReqPost_SpringBack.Enabled = False
                cmbTest_QtyPost_SpringBack.Enabled = False
                cmbTest_FreqPost_SpringBack.Enabled = False
            End If
        Else
            '....Leak
            txtTest_CompressPost_Leak.Enabled = False
            cmbTest_MediaPost_Leak.Enabled = False
            txtTest_PressPost_Leak.Enabled = False
            txtTest_ReqPost_Leak.Enabled = False
            cmbTest_QtyPost_Leak.Enabled = False
            cmbTest_FreqPost_Leak.Enabled = False

            '....Load
            txtTest_CompressPost_Load.Enabled = False
            txtTest_ReqPost_Load.Enabled = False
            cmbTest_QtyPost_Load.Enabled = False
            cmbTest_FreqPost_Load.Enabled = False

            '....SpringBack
            txtTest_CompressPost_SpringBack.Enabled = False
            txtTest_ReqPost_SpringBack.Enabled = False
            cmbTest_QtyPost_SpringBack.Enabled = False
            cmbTest_FreqPost_SpringBack.Enabled = False
        End If
    End Sub
    Private Sub PopulateMatComboBox(ByVal SealType_In As String, ByRef cmbBox As ComboBox)
        '=================================================================================
        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pMatList As New List(Of String)
        cmbBox.Items.Clear()

        Dim pUsedMatCodeType As New List(Of String)
        pUsedMatCodeType.Add("P")
        pUsedMatCodeType.Add("S")
        pUsedMatCodeType.Add("O")


        Select Case SealType_In
            Case "E"
                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                                Where pRec.fldE = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "C"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                                Where pRec.fldC = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "U"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                                Where pRec.fldU = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "SC"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
                                Where pRec.fldC = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            Dim pMatName As String = pQry(j).fldName.Trim()
                            If (pMatName.Contains("Cobalt Chromium-Nickel Alloy")) Then
                                pMatName = pMatName.Replace("Cobalt Chromium-Nickel Alloy", "Co-Cr-Ni")
                            End If

                            pMatList.Add(pMatName)
                        Next
                    End If
                Next

        End Select

        For i As Integer = 0 To pMatList.Count - 1
            cmbBox.Items.Add(pMatList(i))
        Next

        If (cmbBox.Items.Count > 0) Then
            cmbBox.SelectedIndex = 0
        End If

    End Sub

    Private Sub RetrieveFromDB()
        '=======================
        mProcess_Project.RetrieveFromDB(mPNID, mRevID)

        mProcess_Project.Unit.RetrieveFrom_DB(mProcess_Project.ID)
        mProcess_Project.CustContact.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.PreOrder.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.ITAR_Export.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.OrdEntry.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Cost.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.App.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Design.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Manf.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Purchase.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Qlty.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Dwg.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Test.RetrieveFromDB(mProcess_Project.ID)
        ''mProcess_Project.Planning.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Shipping.RetrieveFromDB(mProcess_Project.ID)

        mProcess_Project.IssueCommnt.RetrieveFromDB(mProcess_Project.ID)
        mProcess_Project.Approval.RetrieveFromDB(mProcess_Project.ID)   'AES 02APR18

        'AES 25APR18
        '....Pre-Order
        If (IsNothing(mProcess_Project.PreOrder.Part.Family) Or mProcess_Project.PreOrder.Part.Family = "") Then
            mProcess_Project.PreOrder.Part_Family = gPartProject.PNR.SealType.ToString() & "-Seal"
        End If

        '....App
        If (IsNothing(mProcess_Project.App.Type) Or mProcess_Project.App.Type = "") Then
            mProcess_Project.App.Type = "Face"
        End If

        If (IsNothing(mProcess_Project.App.CavityFlange.MeasureSF) Or mProcess_Project.App.CavityFlange.MeasureSF = "") Then
            mProcess_Project.App.CavityFlange.MeasureSF = "Ra"
        End If

        If (IsNothing(mProcess_Project.App.CavityFlange.UnitSF) Or mProcess_Project.App.CavityFlange.UnitSF = "") Then
            mProcess_Project.App.CavityFlange.UnitSF = "µin"
        End If

        If (IsNothing(mProcess_Project.App.Face.POrient) Or mProcess_Project.App.Face.POrient = "") Then
            If (Not IsNothing(gPartProject.PNR.HW)) Then
                mProcess_Project.App.Face.POrient = gPartProject.PNR.HW.POrient
            End If
        End If

        '....Quality
        If (IsNothing(mProcess_Project.Qlty.VisualInspection_Type) ) Then
            mProcess_Project.Qlty.VisualInspection_Type = "10x"
        End If




        'AES 26FEB18
        If (mProcess_Project.Unit.LUnit_Cust = "in") Then
            gUnit.SetLFormat("English")
        Else
            gUnit.SetLFormat("Metric")
        End If

    End Sub


    Private Sub DisplayData()
        '====================
        Dim pCI As New CultureInfo("en-US")
        '.... "General:"
        txtParkerPart.Text = gPartProject.PNR.PN
        txtPN_Rev.Text = gPartProject.PNR.PN_Rev

        txtCustomer.Text = gPartProject.CustInfo.CustName
        txtCustomerPN.Text = gPartProject.CustInfo.PN_Cust
        txtCustomerPN_Rev.Text = gPartProject.CustInfo.PN_Cust_Rev

        If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Header")) Then
            If (mProcess_Project.EditedBy.Name(0) <> "") Then
                txtHeaderUserName.Text = mProcess_Project.EditedBy.Name(0)
                chkHeaderUserSigned.Checked = True
            End If
        Else
            txtHeaderUserName.Text = ""
            chkHeaderUserSigned.Checked = False
        End If

        SetControls_HeaderUserSign()

        With mProcess_Project
            If (.POPCoding <> "") Then
                cmbPopCoding.Text = .POPCoding
            Else
                cmbPopCoding.SelectedIndex = -1
            End If

            If (.Rating <> "") Then
                cmbRating.Text = .Rating
            Else
                cmbRating.SelectedIndex = -1

            End If

            If (.Type <> "") Then
                cmbType.Text = .Type
            Else
                cmbType.SelectedIndex = -1
            End If


            If (.DateOpen <> DateTime.MinValue) Then
                txtStartDate.Text = .DateOpen.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            Else
                'txtStartDate.Text = ""
                txtStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())       'AES 23APR18
            End If

            If (.DateLastModified <> DateTime.MinValue) Then
                txtDateMod.Text = .DateLastModified.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            Else
                txtDateMod.Text = ""
            End If

            txtModifiedBy.Text = .LastModifiedBy
        End With

        '.... "PreOrder:"
        With mProcess_Project.PreOrder

            If (.Mgr.Mkt <> "") Then
                cmbMgrPreOrder.Text = .Mgr.Mkt
            Else
                cmbMgrPreOrder.SelectedIndex = -1
            End If

            txtMgrSales.Text = .Mgr.Sales

                If (.Export.Reqd) Then
                    cmbExport_Reqd.Text = "Y"
                    cmbExport_Status.Text = .Export.Status
                    cmbExport_Status.Enabled = True
                Else
                    cmbExport_Reqd.Text = "N"
                    cmbExport_Status.Text = ""
                    cmbExport_Status.Enabled = False
                End If

                If (gPartProject.PNR.SealType.ToString() = "E") Then
                    cmbPartFamily.Text = "E-Seal"
                    'cmbPartFamily.Enabled = False
                ElseIf (gPartProject.PNR.SealType.ToString() = "C") Then
                    cmbPartFamily.Text = "C-Seal"
                    'cmbPartFamily.Enabled = False
                ElseIf (gPartProject.PNR.SealType.ToString() = "SC") Then
                    cmbPartFamily.Text = "SC-Seal"
                    'cmbPartFamily.Enabled = False
                ElseIf (gPartProject.PNR.SealType.ToString() = "U") Then
                    cmbPartFamily.Text = "U-Seal"
                    'cmbPartFamily.Enabled = False
                Else
                    cmbPartFamily.Text = .Part.Family
                    'cmbPartFamily.Enabled = True
                End If

            If (.Part.Type <> "") Then
                cmbPartType.Text = .Part.Type
            Else
                cmbPartType.SelectedIndex = -1
            End If

            cmbPreOrderSeg.Text = .Mkt.Seg
            cmbPreOrderChannel.Text = .Mkt.Channel

            txtPreOrderNotes.Text = .Notes

            cmbCostFileLoc.Text = .Loc.CostFile
            cmbRFQPkgLoc.Text = .Loc.RFQPkg
            txtPreOrderPriceNotes.Text = .Notes_Price

            '....Cust Contact Pre-Order
            grdCustContact.Rows.Clear()
            For j As Integer = 0 To mProcess_Project.CustContact.DeptName.Count - 1
                Dim pCmbColDept_PreOrd As New DataGridViewComboBoxColumn
                pCmbColDept_PreOrd = grdCustContact.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(mProcess_Project.CustContact.DeptName(j))) Then
                    pVal = mProcess_Project.CustContact.DeptName(j)
                End If
                If (Not pCmbColDept_PreOrd.Items.Contains(pVal)) Then
                    pCmbColDept_PreOrd.Items.Add(pVal)
                End If
            Next

            For j As Integer = 0 To mProcess_Project.CustContact.DeptName.Count - 1
                grdCustContact.Rows.Add()
                If (Not IsNothing(mProcess_Project.CustContact.DeptName(j))) Then
                    grdCustContact.Rows(j).Cells(0).Value = mProcess_Project.CustContact.DeptName(j)
                Else
                    grdCustContact.Rows(j).Cells(0).Value = ""
                End If

                grdCustContact.Rows(j).Cells(1).Value = mProcess_Project.CustContact.Name(j)

                grdCustContact.Rows(j).Cells(2).Value = mProcess_Project.CustContact.Phone(j)
                grdCustContact.Rows(j).Cells(3).Value = mProcess_Project.CustContact.Email(j)
            Next

            '....Cust Contact Order-Entry
            grdOrdEntry_CustContact.Rows.Clear()
            For j As Integer = 0 To mProcess_Project.CustContact.DeptName.Count - 1
                Dim pCmbColDept_OrdEntry As New DataGridViewComboBoxColumn
                pCmbColDept_OrdEntry = grdOrdEntry_CustContact.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(mProcess_Project.CustContact.DeptName(j))) Then
                    pVal = mProcess_Project.CustContact.DeptName(j)
                End If
                If (Not pCmbColDept_OrdEntry.Items.Contains(pVal)) Then
                    pCmbColDept_OrdEntry.Items.Add(pVal)
                End If
            Next

            For j As Integer = 0 To mProcess_Project.CustContact.DeptName.Count - 1
                grdOrdEntry_CustContact.Rows.Add()
                'grdOrdEntry_CustContact.Rows(j).Cells(0).Value = mProcess_Project.CustContact.DeptName(j)
                If (Not IsNothing(mProcess_Project.CustContact.DeptName(j))) Then
                    grdOrdEntry_CustContact.Rows(j).Cells(0).Value = mProcess_Project.CustContact.DeptName(j)
                Else
                    grdOrdEntry_CustContact.Rows(j).Cells(0).Value = ""
                End If
                grdOrdEntry_CustContact.Rows(j).Cells(1).Value = mProcess_Project.CustContact.Name(j)

                grdOrdEntry_CustContact.Rows(j).Cells(2).Value = mProcess_Project.CustContact.Phone(j)
                grdOrdEntry_CustContact.Rows(j).Cells(3).Value = mProcess_Project.CustContact.Email(j)
            Next

            '....Quote
            grdQuote.Rows.Clear()
            For k As Integer = 0 To .Quote.QDate.Count - 1
                grdQuote.Rows.Add()

                If (.Quote.QDate(k) <> DateTime.MinValue) Then
                    grdQuote.Rows(k).Cells(0).Value = .Quote.QDate(k).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                Else
                    grdQuote.Rows(k).Cells(0).Value = ""
                End If

                grdQuote.Rows(k).Cells(1).Value = .Quote.No(k)
            Next

            '....SalesData
            grdPreOrder_SalesData.Rows.Clear()
            For j As Integer = 0 To .SalesData.Year.Count - 1
                grdPreOrder_SalesData.Rows.Add()
                grdPreOrder_SalesData.Rows(j).Cells(0).Value = .SalesData.Year(j)
                grdPreOrder_SalesData.Rows(j).Cells(1).Value = .SalesData.Qty(j)
                grdPreOrder_SalesData.Rows(j).Cells(2).Value = .SalesData.Price(j).ToString("#0.##")
                grdPreOrder_SalesData.Rows(j).Cells(3).Value = .SalesData.Total(j).ToString("#0.##")
            Next

            '....EditedBy
            grdPreOrderEditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "PreOrder")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdPreOrderEditedBy.Rows.Add()
                    grdPreOrderEditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdPreOrderEditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdPreOrderEditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtPreOrderUserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtPreOrderUserName.Text = mProcess_Project.EditedBy.Name(j)
                Next

            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "PreOrder")) Then
                txtPreOrderUserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtPreOrderUserName.Text = mProcess_Project.EditedBy.User.Name
                chkPreOrderUserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtPreOrderUserDate.Text = ""
                txtPreOrderUserName.Text = ""
                chkPreOrderUserSigned.Checked = False
            End If

        End With


        '.... "ITAR_Export:"
        With mProcess_Project.ITAR_Export
            cmdExport_HTS.Enabled = False

            If (.IsCustOnDenialList) Then
                cmbITAR_Export_CustOnDenialList.Text = "Y"
            Else
                cmbITAR_Export_CustOnDenialList.Text = "N"
            End If

            If (.CountryProhibited) Then
                cmbITAR_Export_CountryProhibited.Text = "Y"
            Else
                cmbITAR_Export_CountryProhibited.Text = "N"
            End If

            If (.HasAntiBoycottLang) Then
                cmbITAR_Export_AntiBoycottLang.Text = "Y"
            Else
                cmbITAR_Export_AntiBoycottLang.Text = "N"
            End If

            If (.IsUnder_ITAR_Reg) Then
                cmbITAR_Export_ProductITAR_Reg.Text = "Y"
                txtExportControlled.Text = "Y"
                txtITAR_Export_ITAR_Classification.Enabled = True
            Else
                cmbITAR_Export_ProductITAR_Reg.Text = "N"
                txtExportControlled.Text = "N"
                txtExportStatus.Text = ""
                txtITAR_Export_ITAR_Classification.Enabled = False
            End If

            If (.SaleExportControlled) Then
                cmbITAR_Export_SaleExportControlled.Text = "Y"
                cmbITAR_Export_EAR_Classification.Enabled = True
            Else
                cmbITAR_Export_SaleExportControlled.Text = "N"
                cmbITAR_Export_EAR_Classification.Enabled = False
            End If

            txtITAR_Export_ITAR_Classification.Text = .ITAR_Class
            cmbITAR_Export_EAR_Classification.Text = .EAR_Class
            cmbITAR_Export_Status.Text = .Status
            txtITAR_Export_HTS_Classification.Text = .HTS_Class

            'If (.EditedBy.User.Signed) Then
            '    chkITAR_Export_UserSigned.Checked = True
            '    txtITAR_Export_UserName.Text = .EditedBy.User.Name
            '    txtITAR_Export_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()

            'Else
            '    chkITAR_Export_UserSigned.Checked = False
            '    txtITAR_Export_UserName.Text = ""
            '    txtITAR_Export_UserDate.Text = ""
            'End If

            grdExport_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Export")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdExport_EditedBy.Rows.Add()
                    grdExport_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdExport_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdExport_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtITAR_Export_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtITAR_Export_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Export")) Then
                txtITAR_Export_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtITAR_Export_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkITAR_Export_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtITAR_Export_UserDate.Text = ""
                txtITAR_Export_UserName.Text = ""
                chkITAR_Export_UserSigned.Checked = False
            End If

        End With


        '.... "OrderEntrty:"
        With mProcess_Project.OrdEntry
            txtOrdEntry_SalesOrderNo.Text = .SalesOrderNo

            If (.DateSales <> DateTime.MinValue) Then
                txtOrdEntry_SalesDate.Text = .DateSales.ToShortDateString()
            Else
                txtOrdEntry_SalesDate.Text = ""
            End If

            If (Math.Abs(.LeadTimeQuoted) > gcEPS) Then
                txtOrderEntry_QtdLeadTime.Text = .LeadTimeQuoted
            Else
                txtOrderEntry_QtdLeadTime.Text = ""
            End If

            txtOrdEntry_PONo.Text = .PONo
            If (.DatePO <> DateTime.MinValue) Then
                txtOrdEntry_PODate.Text = .DatePO.ToShortDateString()
            Else
                txtOrdEntry_PODate.Text = ""
            End If

            If (.DatePO_EDI <> DateTime.MinValue) Then
                txtOrdEntry_PODate_EDI.Text = .DatePO_EDI.ToShortDateString()
            Else
                txtOrdEntry_PODate_EDI.Text = ""
            End If


            If (.HasSplReq) Then
                cmbOrdEntry_SpecialReq.Text = "Y"
            Else
                cmbOrdEntry_SpecialReq.Text = "N"
            End If

            If (.Tool_Reqd) Then
                cmbOrdEntry_Tooling.Text = "Y"
            Else
                cmbOrdEntry_Tooling.Text = "N"
            End If

            If (.SplPkg_Lbl_Reqd) Then
                cmbOrdEntry_SplPkgNLbl.Text = "Y"
            Else
                cmbOrdEntry_SplPkgNLbl.Text = "N"
            End If

            If (.OrdQty > 0) Then
                txtOrdEntry_OrderQty.Text = .OrdQty.ToString()
            Else
                txtOrdEntry_OrderQty.Text = ""
            End If

            If (.DateOrdShip <> DateTime.MinValue) Then
                txtOrdEntry_OrderShipDate.Text = .DateOrdShip.ToShortDateString()
            Else
                txtOrdEntry_OrderShipDate.Text = ""
            End If

            If (.Expedited) Then
                cmbOrdEntry_Expedited.Text = "Y"
            Else
                cmbOrdEntry_Expedited.Text = "N"
            End If

            If (.IsDFAR) Then
                cmbOrdEntry_DFAR.Text = "Y"
            Else
                cmbOrdEntry_DFAR.Text = "N"
            End If

            'If (.EditedBy.User.Signed) Then
            '    chkOrdEntry_UserSigned.Checked = True
            '    txtOrdEntry_UserName.Text = .EditedBy.User.Name
            '    txtOrdEntry_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
            'Else
            '    chkOrdEntry_UserSigned.Checked = False
            '    txtOrdEntry_UserName.Text = ""
            '    txtOrdEntry_UserDate.Text = ""
            'End If

            grdOrdEntry_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "OrdEntry")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdOrdEntry_EditedBy.Rows.Add()
                    grdOrdEntry_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdOrdEntry_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdOrdEntry_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtOrdEntry_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtOrdEntry_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "OrdEntry")) Then
                txtOrdEntry_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtOrdEntry_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkOrdEntry_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtOrdEntry_UserDate.Text = ""
                txtOrdEntry_UserName.Text = ""
                chkOrdEntry_UserSigned.Checked = False
            End If

        End With

        '.... "Cost Estimating:"
        With mProcess_Project.Cost
            cmbCost_QuoteFile.Text = .QuoteFileLoc
            txtCost_Notes.Text = .Notes

            '....Spl Operation
            grdCost_SplOperation.Rows.Clear()
            For j As Integer = 0 To .SplOperation.Desc.Count - 1
                Dim pCmbColDesc_Cost As New DataGridViewComboBoxColumn
                pCmbColDesc_Cost = grdCost_SplOperation.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(.SplOperation.Desc(j))) Then
                    pVal = .SplOperation.Desc(j)
                End If
                If (Not pCmbColDesc_Cost.Items.Contains(pVal)) Then
                    pCmbColDesc_Cost.Items.Add(pVal)
                End If
            Next

            '....Spl Operation
            For j As Integer = 0 To .SplOperation.Desc.Count - 1
                grdCost_SplOperation.Rows.Add()
                If (Not IsNothing(.SplOperation.Desc(j))) Then
                    grdCost_SplOperation.Rows(j).Cells(0).Value = .SplOperation.Desc(j)
                Else
                    grdCost_SplOperation.Rows(j).Cells(0).Value = ""
                End If
                'grdCost_SplOperation.Rows(j).Cells(0).Value = .SplOperation.Desc(j)
                grdCost_SplOperation.Rows(j).Cells(1).Value = .SplOperation.Spec(j)
                grdCost_SplOperation.Rows(j).Cells(2).Value = gUnit.Format_Val(.SplOperation.LeadTime(j)) '.SplOperation.LeadTime(j)
                grdCost_SplOperation.Rows(j).Cells(3).Value = .SplOperation.Cost(j).ToString("#0.##")
            Next

            'If (.EditedBy.User.Signed) Then
            '    chkCost_UserSigned.Checked = True
            '    txtCost_UserName.Text = .EditedBy.User.Name
            '    txtCost_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
            'Else
            '    chkCost_UserSigned.Checked = False
            '    txtCost_UserName.Text = .EditedBy.User.Name
            '    txtCost_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
            'End If

            grdCost_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Cost")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdCost_EditedBy.Rows.Add()
                    grdCost_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdCost_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdCost_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtCost_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtCost_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Cost")) Then
                txtCost_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtCost_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkCost_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtCost_UserDate.Text = ""
                txtCost_UserName.Text = ""
                chkCost_UserSigned.Checked = False
            End If

        End With


        '.... "Application:"
        With mProcess_Project.App
            txtApp_Equip.Text = .Eqp
            txtApp_ExistingSeal.Text = .ExistingSeal
            cmbApp_InsertLoc.Text = .Type

            'If (.InsertLoc = "Face") Then
            '    tbApp.Controls.Remove(tbpAxial)
            'Else
            '    tbApp.Controls.Remove(tbpFace)
            'End If

            txtApp_Fluid.Text = .Fluid
            If (Math.Abs(.MaxLeak) > gcEPS) Then
                txtApp_MaxLeak.Text = gUnit.Format_LeakVal(.MaxLeak) 'Format(.MaxLeak, gUnit.LFormat)    'AES 26FEB18 '.MaxLeak.ToString("##0.000")
            Else
                txtApp_MaxLeak.Text = ""
            End If

            If (.IsPressCyclic) Then
                cmbApp_PressCycle.Text = "Y"
                txtApp_PressCycleFreq.Enabled = True
                txtApp_PressCycleFreq.Text = gUnit.Format_Val(.PressCycle_Freq) '.PressCycle_Freq.ToString("##0.000")
                txtApp_PressCycleAmp.Text = gUnit.Format_Val(.PressCycle_Amp) '.PressCycle_Amp.ToString("##0.000")
                txtApp_PressCycleAmp.Enabled = True
            Else
                cmbApp_PressCycle.Text = "N"
                txtApp_PressCycleFreq.Text = ""
                txtApp_PressCycleAmp.Text = ""
                txtApp_PressCycleFreq.Enabled = False
                txtApp_PressCycleAmp.Enabled = False
            End If

            If (.Shaped) Then
                cmbApp_Shaped.Text = "Y"
            Else
                cmbApp_Shaped.Text = "N"
            End If

            If (.IsOoR) Then
                cmbApp_OutOfRound.Text = "Y"
            Else
                cmbApp_OutOfRound.Text = "N"
            End If

            If (.IsSplitRing) Then
                cmbApp_SplitRing.Text = "Y"
            Else
                cmbApp_SplitRing.Text = "N"
            End If

            If (.IsPreComp) Then
                cmbApp_PreComp.Text = "Y"
            Else
                cmbApp_PreComp.Text = "N"
            End If

            If (gPartProject.PNR.HW.IsSegmented) Then
                chkSeg.Checked = True
                txtSegNo.Text = gPartProject.PNR.HW.CountSegment.ToString()
            Else
                chkSeg.Checked = False
                txtSegNo.Text = ""
            End If

            grdApp_OpCond.Rows.Clear()

            grdApp_OpCond.Rows.Add()
            grdApp_OpCond.Rows.Add()
            grdApp_OpCond.Rows(0).Cells(0).Value = "Temperature"
            grdApp_OpCond.Rows(1).Cells(0).Value = "Pressure"
            grdApp_OpCond.Columns(0).ReadOnly = True
            grdApp_OpCond.AllowUserToAddRows = False

            If (Math.Abs(.OpCond.T.Assy) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(1).Value = gUnit.Format_Val(.OpCond.T.Assy) '.OpCond.T.Assy.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(1).Value = ""
            End If

            If (Math.Abs(.OpCond.T.Min) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(2).Value = gUnit.Format_Val(.OpCond.T.Min) '.OpCond.T.Min.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(2).Value = ""
            End If

            If (Math.Abs(.OpCond.T.Max) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(3).Value = gUnit.Format_Val(.OpCond.T.Max) '.OpCond.T.Max.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(3).Value = ""
            End If

            If (Math.Abs(.OpCond.T.Oper) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(4).Value = gUnit.Format_Val(.OpCond.T.Oper) '.OpCond.T.Oper.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(4).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Assy) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(1).Value = gUnit.Format_Val(.OpCond.Press.Assy) '.OpCond.Press.Assy.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(1).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Min) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(2).Value = gUnit.Format_Val(.OpCond.Press.Min) '.OpCond.Press.Min.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(2).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Max) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(3).Value = gUnit.Format_Val(.OpCond.Press.Max) '.OpCond.Press.Max.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(3).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Oper) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(4).Value = gUnit.Format_Val(.OpCond.Press.Oper) '.OpCond.Press.Oper.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(4).Value = ""
            End If

            grdApp_Load.Rows.Clear()
            grdApp_Load.Rows.Add()
            grdApp_Load.Rows.Add()
            grdApp_Load.Rows(0).Cells(0).Value = "Assembly"
            grdApp_Load.Rows(1).Cells(0).Value = "Operating"
            grdApp_Load.Columns(0).ReadOnly = True
            grdApp_Load.AllowUserToAddRows = False

            If (Math.Abs(.Load.Assy.Min) > gcEPS) Then
                grdApp_Load.Rows(0).Cells(1).Value = Format(.Load.Assy.Min, gUnit.LFormat) '.Load.Assy.Min.ToString("##0.0")
            Else
                grdApp_Load.Rows(0).Cells(1).Value = ""
            End If

            If (Math.Abs(.Load.Assy.Max) > gcEPS) Then
                grdApp_Load.Rows(0).Cells(2).Value = Format(.Load.Assy.Max, gUnit.LFormat) '.Load.Assy.Max.ToString("##0.0")
            Else
                grdApp_Load.Rows(0).Cells(2).Value = ""
            End If

            If (Math.Abs(.Load.Oper.Min) > gcEPS) Then
                grdApp_Load.Rows(1).Cells(1).Value = Format(.Load.Oper.Min, gUnit.LFormat) '.Load.Oper.Min.ToString("##0.0")
            Else
                grdApp_Load.Rows(1).Cells(1).Value = ""
            End If

            If (Math.Abs(.Load.Oper.Max) > gcEPS) Then
                grdApp_Load.Rows(1).Cells(2).Value = Format(.Load.Oper.Max, gUnit.LFormat) '.Load.Oper.Max.ToString("##0.0")
            Else
                grdApp_Load.Rows(1).Cells(2).Value = ""
            End If

            '....Face Seal
            grdApp_Face_Cavity.Rows.Clear()
            Dim pType As String = .Type
            If (.Type = "Face") Then
                '....Cavity Dimension
                For j As Integer = 0 To .Cavity.ID_Cavity.Count - 1
                    Dim pCmbColCavityDim As New DataGridViewComboBoxColumn
                    pCmbColCavityDim = grdApp_Face_Cavity.Columns.Item(0)
                    Dim pVal As String = ""
                    If (Not IsNothing(.Cavity.DimName(j))) Then
                        pVal = .Cavity.DimName(j)
                    End If
                    If (Not pCmbColCavityDim.Items.Contains(pVal)) Then
                        pCmbColCavityDim.Items.Add(pVal)
                    End If
                Next

                '....Cavity Dimension
                For j As Integer = 0 To .Cavity.ID_Cavity.Count - 1
                    grdApp_Face_Cavity.Rows.Add()
                    If (Not IsNothing(.Cavity.DimName(j))) Then
                        grdApp_Face_Cavity.Rows(j).Cells(0).Value = .Cavity.DimName(j)
                    Else
                        grdApp_Face_Cavity.Rows(j).Cells(0).Value = ""
                    End If
                    grdApp_Face_Cavity.Rows(j).Cells(1).Value = Format(.Cavity.Assy(j).Min, gUnit.LFormat)
                    grdApp_Face_Cavity.Rows(j).Cells(2).Value = Format(.Cavity.Assy(j).Max, gUnit.LFormat)
                    grdApp_Face_Cavity.Rows(j).Cells(3).Value = Format(.Cavity.Oper(j).Min, gUnit.LFormat)
                    grdApp_Face_Cavity.Rows(j).Cells(4).Value = Format(.Cavity.Oper(j).Max, gUnit.LFormat)
                Next

                'For j As Integer = 0 To .Cavity.ID_Cavity.Count - 1
                '    grdApp_Face_Cavity.Rows.Add()
                '    grdApp_Face_Cavity.Rows(j).Cells(0).Value = .Cavity.DimName(j)
                '    grdApp_Face_Cavity.Rows(j).Cells(1).Value = .Cavity.Assy(j).Min.ToString("##0.000")
                '    grdApp_Face_Cavity.Rows(j).Cells(2).Value = .Cavity.Assy(j).Max.ToString("##0.000")
                '    grdApp_Face_Cavity.Rows(j).Cells(3).Value = .Cavity.Oper(j).Min.ToString("##0.000")
                '    grdApp_Face_Cavity.Rows(j).Cells(4).Value = .Cavity.Oper(j).Max.ToString("##0.000")
                'Next

                txtApp_Mat1_Face.Text = .CavityFlange.Mat1
                txtApp_Mat2_Face.Text = .CavityFlange.Mat2

                If (Math.Abs(.CavityFlange.Hard1) > gcEPS) Then
                    txtApp_Hardness1_Face.Text = Format(.CavityFlange.Hard1, gUnit.LFormat) '.CavityFlange.Hard1.ToString("##0.000")
                Else
                    txtApp_Hardness1_Face.Text = ""
                End If

                If (Math.Abs(.CavityFlange.Hard2) > gcEPS) Then
                    txtApp_Hardness2_Face.Text = Format(.CavityFlange.Hard2, gUnit.LFormat) '.CavityFlange.Hard2.ToString("##0.000")
                Else
                    txtApp_Hardness2_Face.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF1) > gcEPS) Then
                    txtApp_SF1_Face.Text = Format(.CavityFlange.SF1, gUnit.LFormat) '.CavityFlange.SF1.ToString("##0.000")
                Else
                    txtApp_SF1_Face.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF2) > gcEPS) Then
                    txtApp_SF2_Face.Text = Format(.CavityFlange.SF2, gUnit.LFormat) '.CavityFlange.SF2.ToString("##0.000")
                Else
                    txtApp_SF2_Face.Text = ""
                End If

                cmbFace_SF_ProcessName.Text = .CavityFlange.MeasureSF
                cmbFace_SF_Unit.Text = .CavityFlange.UnitSF


                cmbApp_Face_POrient.Text = gPartProject.PNR.HW.POrient '.Face.POrient       'AES 09JAN18

                If (Math.Abs(.Face.MaxFlangeSep) > gcEPS) Then
                    txtApp_Face_MaxFlangeSeparation.Text = Format(.Face.MaxFlangeSep, gUnit.LFormat) '.Face.MaxFlangeSep.ToString("##0.000")
                Else
                    txtApp_Face_MaxFlangeSeparation.Text = ""
                End If

                grdApp_EditedBy_Face.Rows.Clear()
                If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "App")) Then
                    For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                        grdApp_EditedBy_Face.Rows.Add()
                        grdApp_EditedBy_Face.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdApp_EditedBy_Face.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                        grdApp_EditedBy_Face.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                        txtApp_UserDate_Face.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtApp_UserName_Face.Text = mProcess_Project.EditedBy.Name(j)
                    Next
                End If

                If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "App")) Then
                    txtApp_UserName_Face.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtApp_UserDate_Face.Text = mProcess_Project.EditedBy.User.Name
                    chkApp_UserSigned_Face.Checked = mProcess_Project.EditedBy.User.Signed
                Else
                    txtApp_UserName_Face.Text = ""
                    txtApp_UserDate_Face.Text = ""
                    chkApp_UserSigned_Face.Checked = False
                End If

            ElseIf (.Type = "Axial") Then
                grdApp_Axial_Cavity.Rows.Clear()
                '....Cavity Dimension
                For j As Integer = 0 To .Cavity.ID_Cavity.Count - 1
                    Dim pCmbColCavityDim As New DataGridViewComboBoxColumn
                    pCmbColCavityDim = grdApp_Axial_Cavity.Columns.Item(0)
                    Dim pVal As String = ""
                    If (Not IsNothing(.Cavity.DimName(j))) Then
                        pVal = .Cavity.DimName(j)
                    End If
                    If (Not pCmbColCavityDim.Items.Contains(pVal)) Then
                        pCmbColCavityDim.Items.Add(pVal)
                    End If
                Next

                '....Cavity Dimension
                For j As Integer = 0 To .Cavity.ID_Cavity.Count - 1
                    grdApp_Axial_Cavity.Rows.Add()
                    If (Not IsNothing(.Cavity.DimName(j))) Then
                        grdApp_Axial_Cavity.Rows(j).Cells(0).Value = .Cavity.DimName(j)
                    Else
                        grdApp_Axial_Cavity.Rows(j).Cells(0).Value = ""
                    End If
                    grdApp_Axial_Cavity.Rows(j).Cells(1).Value = Format(.Cavity.Assy(j).Min, gUnit.LFormat) '.Cavity.Assy(j).Min.ToString("##0.000")
                    grdApp_Axial_Cavity.Rows(j).Cells(2).Value = Format(.Cavity.Assy(j).Max, gUnit.LFormat) '.Cavity.Assy(j).Max.ToString("##0.000")
                    grdApp_Axial_Cavity.Rows(j).Cells(3).Value = Format(.Cavity.Oper(j).Min, gUnit.LFormat) '.Cavity.Oper(j).Min.ToString("##0.000")
                    grdApp_Axial_Cavity.Rows(j).Cells(4).Value = Format(.Cavity.Oper(j).Max, gUnit.LFormat) '.Cavity.Oper(j).Max.ToString("##0.000")
                Next

                ''....Axial Seal
                'For j As Integer = 0 To .Cavity.ID_Cavity.Count - 1
                '    grdApp_Axial_Cavity.Rows.Add()
                '    grdApp_Axial_Cavity.Rows(j).Cells(0).Value = .Cavity.DimName(j)
                '    grdApp_Axial_Cavity.Rows(j).Cells(1).Value = .Cavity.Assy(j).Min.ToString("##0.000")
                '    grdApp_Axial_Cavity.Rows(j).Cells(2).Value = .Cavity.Assy(j).Max.ToString("##0.000")
                '    grdApp_Axial_Cavity.Rows(j).Cells(3).Value = .Cavity.Oper(j).Min.ToString("##0.000")
                '    grdApp_Axial_Cavity.Rows(j).Cells(4).Value = .Cavity.Oper(j).Max.ToString("##0.000")
                'Next

                txtApp_Mat1_Axial.Text = .CavityFlange.Mat1
                txtApp_Mat2_Axial.Text = .CavityFlange.Mat2

                If (Math.Abs(.CavityFlange.Hard1) > gcEPS) Then
                    txtApp_Hardness1_Axial.Text = Format(.CavityFlange.Hard1, gUnit.LFormat) '.CavityFlange.Hard1.ToString("##0.000")
                Else
                    txtApp_Hardness1_Axial.Text = ""
                End If

                If (Math.Abs(.CavityFlange.Hard2) > gcEPS) Then
                    txtApp_Hardness2_Axial.Text = Format(.CavityFlange.Hard2, gUnit.LFormat) '.CavityFlange.Hard2.ToString("##0.000")
                Else
                    txtApp_Hardness2_Axial.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF1) > gcEPS) Then
                    txtApp_SF1_Axial.Text = Format(.CavityFlange.SF1, gUnit.LFormat) '.CavityFlange.SF1.ToString("##0.000")
                Else
                    txtApp_SF1_Axial.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF2) > gcEPS) Then
                    txtApp_SF2_Axial.Text = Format(.CavityFlange.SF2, gUnit.LFormat) '.CavityFlange.SF2.ToString("##0.000")
                Else
                    txtApp_SF2_Axial.Text = ""
                End If

                cmbAxial_SF_ProcessName.Text = .CavityFlange.MeasureSF
                cmbAxial_SF_Unit.Text = .CavityFlange.UnitSF

                If (.Axial.IsStatic) Then
                    cmbApp_Static_Axial.Text = "Y"
                Else
                    cmbApp_Static_Axial.Text = "N"
                End If

                If (.Axial.IsRotating) Then
                    cmbApp_Rotate_Axial.Text = "Y"
                    If (Math.Abs(.Axial.RPM)) Then
                        txtApp_RotateRPM_Axial.Text = gUnit.Format_Val(.Axial.RPM) '.Axial.RPM.ToString("##0")
                    Else
                        txtApp_RotateRPM_Axial.Text = ""
                    End If

                Else
                    cmbApp_Rotate_Axial.Text = "N"
                    txtApp_RotateRPM_Axial.Text = ""
                End If

                If (.Axial.IsRecip) Then
                    cmbApp_Recip_Axial.Text = "Y"

                    If (Math.Abs(.Axial.Recip_Stroke)) Then
                        txtApp_RecipStrokeL_Axial.Text = gUnit.Format_Val(.Axial.Recip_Stroke) '.Axial.Recip_Stroke.ToString("##0")
                    Else
                        txtApp_RecipStrokeL_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Recip_V)) Then
                        txtApp_RecipV_Axial.Text = gUnit.Format_Val(.Axial.Recip_V) '.Axial.Recip_V.ToString("##0")
                    Else
                        txtApp_RecipV_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Recip_CycleRate)) Then
                        txtApp_RecipCycleRate_Axial.Text = gUnit.Format_Val(.Axial.Recip_CycleRate) '.Axial.Recip_CycleRate.ToString("##0")
                    Else
                        txtApp_RecipCycleRate_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Recip_ServiceLife)) Then
                        txtApp_RecipServiceLife_Axial.Text = gUnit.Format_Val(.Axial.Recip_ServiceLife) '.Axial.Recip_ServiceLife.ToString("##0")
                    Else
                        txtApp_RecipServiceLife_Axial.Text = ""
                    End If

                Else
                    cmbApp_Recip_Axial.Text = "N"
                    txtApp_RecipStrokeL_Axial.Text = ""
                    txtApp_RecipV_Axial.Text = ""
                    txtApp_RecipCycleRate_Axial.Text = ""
                    txtApp_RecipServiceLife_Axial.Text = ""
                End If

                If (.Axial.IsOscilatory) Then
                    cmbApp_Osc_Axial.Text = "Y"

                    If (Math.Abs(.Axial.Oscilate_Rot)) Then
                        txtApp_OscRot_Axial.Text = gUnit.Format_Val(.Axial.Oscilate_Rot) '.Axial.Oscilate_Rot.ToString("##0")
                    Else
                        txtApp_OscRot_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Oscilate_V)) Then
                        txtApp_OscV_Axial.Text = gUnit.Format_Val(.Axial.Oscilate_V) '.Axial.Oscilate_V.ToString("##0")
                    Else
                        txtApp_OscV_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Oscilate_CycleRate)) Then
                        txtApp_OscCycleRate_Axial.Text = gUnit.Format_Val(.Axial.Oscilate_CycleRate) '.Axial.Oscilate_CycleRate.ToString("##0")
                    Else
                        txtApp_OscCycleRate_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Oscilate_ServiceLife)) Then
                        txtApp_OscServiceLife_Axial.Text = gUnit.Format_Val(.Axial.Oscilate_ServiceLife) '.Axial.Oscilate_ServiceLife.ToString("##0")
                    Else
                        txtApp_OscServiceLife_Axial.Text = ""
                    End If

                Else
                    cmbApp_Osc_Axial.Text = "N"
                    txtApp_OscRot_Axial.Text = ""
                    txtApp_OscV_Axial.Text = ""
                    txtApp_OscCycleRate_Axial.Text = ""
                    txtApp_OscServiceLife_Axial.Text = ""
                End If

                grdApp_EditedBy_Axial.Rows.Clear()
                If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "App")) Then
                    For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                        grdApp_EditedBy_Axial.Rows.Add()
                        grdApp_EditedBy_Axial.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdApp_EditedBy_Axial.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                        grdApp_EditedBy_Axial.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                        txtApp_UserDate_Axial.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtApp_UserName_Axial.Text = mProcess_Project.EditedBy.Name(j)
                    Next
                End If

                If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "App")) Then
                    txtApp_UserName_Axial.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtApp_UserDate_Axial.Text = mProcess_Project.EditedBy.User.Name
                    chkApp_UserSigned_Axial.Checked = mProcess_Project.EditedBy.User.Signed
                Else
                    txtApp_UserName_Axial.Text = ""
                    txtApp_UserDate_Axial.Text = ""
                    chkApp_UserSigned_Axial.Checked = False
                End If

            End If

        End With


        '...."Design:"
        With mProcess_Project.Design

            txtDesign_CustDwgNo.Text = .CustDwgNo
            txtDesign_CustDwgRev.Text = .CustDwgRev

            If (.Frozen.Design) Then
                cmbDesign_Frozen.Text = "Y"
            Else
                cmbDesign_Frozen.Text = "N"
            End If

            If (.Frozen.Process) Then
                cmbDesign_Process.Text = "Y"
            Else
                cmbDesign_Process.Text = "N"
            End If

            If (.IsClass1) Then
                cmbDesign_Class1.Text = "Y"
            Else
                cmbDesign_Class1.Text = "N"
            End If

            If (.IsBuildToPrint) Then
                cmbDesign_BuildToPrint.Text = "Y"
            Else
                cmbDesign_BuildToPrint.Text = "N"
            End If

            '....Verfication Desc
            grdDesign_Verification.Rows.Clear()
            For j As Integer = 0 To .Verification.ID_Verification.Count - 1
                Dim pCmbColDesc_Verify As New DataGridViewComboBoxColumn
                pCmbColDesc_Verify = grdDesign_Verification.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(.Verification.Desc(j))) Then
                    pVal = .Verification.Desc(j)
                End If
                If (Not pCmbColDesc_Verify.Items.Contains(pVal)) Then
                    pCmbColDesc_Verify.Items.Add(pVal)
                End If
            Next

            For i As Integer = 0 To .Verification.ID_Verification.Count - 1
                grdDesign_Verification.Rows.Add()
                If (Not IsNothing(.Verification.Desc(i))) Then
                    grdDesign_Verification.Rows(i).Cells(0).Value = .Verification.Desc(i)
                Else
                    grdDesign_Verification.Rows(i).Cells(0).Value = ""
                End If
                grdDesign_Verification.Rows(i).Cells(1).Value = .Verification.Owner(i)
                grdDesign_Verification.Rows(i).Cells(2).Value = .Verification.Result(i)
            Next

            '....Referance PN
            '........Current Dim
            chkNewRef_Dim.Checked = gPartProject.PNR.RefDimCurrent.Exists
            If (gPartProject.PNR.RefDimCurrent.Exists) Then
                Dim pParkerPN As String = "NH-" & gPartProject.PNR.RefDimCurrent.TypeNo & gPartProject.PNR.RefDimCurrent.Val
                Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                cmbParkerPN_Part2_NewRef_Dim.Text = pParkerPN_Prefix
                Dim pParkerPN_No As String = pParkerPN.Substring(5)
                txtParkerPN_Part3_NewRef_Dim.Text = pParkerPN_No
                txtPN_PH_Rev_NewRef_Dim.Text = gPartProject.PNR.RefDimCurrent.Rev.Trim()
            Else
                cmbParkerPN_Part2_NewRef_Dim.Text = ""
                txtParkerPN_Part3_NewRef_Dim.Text = ""
                txtPN_PH_Rev_NewRef_Dim.Text = ""
            End If

            '........Current Notes
            chkNewRef_Notes.Checked = gPartProject.PNR.RefNotesCurrent.Exists
            If (gPartProject.PNR.RefNotesCurrent.Exists) Then
                Dim pParkerPN As String = "NH-" & gPartProject.PNR.RefNotesCurrent.TypeNo & gPartProject.PNR.RefNotesCurrent.Val
                Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                cmbParkerPN_Part2_Notes_Dim.Text = pParkerPN_Prefix
                Dim pParkerPN_No As String = pParkerPN.Substring(5)
                txtParkerPN_Part3_Notes_Dim.Text = pParkerPN_No
                txtParkerPN_Rev_Notes_Dim.Text = gPartProject.PNR.RefNotesCurrent.Rev.Trim()
            Else
                cmbParkerPN_Part2_Notes_Dim.Text = ""
                txtParkerPN_Part3_Notes_Dim.Text = ""
                txtParkerPN_Rev_Notes_Dim.Text = ""
            End If

            '........Current Legacy Dim
            chkLegacyRef_Dim.Checked = gPartProject.PNR.RefDimLegacy.Exists
            If (gPartProject.PNR.RefDimLegacy.Exists) Then
                txtLegacyRef_Dim.Text = gPartProject.PNR.RefDimLegacy.Val
                txtLegacyRef_Dim_Rev.Text = gPartProject.PNR.RefDimLegacy.Rev.Trim()
            Else
                txtLegacyRef_Dim.Text = ""
                txtLegacyRef_Dim_Rev.Text = ""
            End If

            '........Current Legacy Notes
            chkLegacyRef_Notes.Checked = gPartProject.PNR.RefNotesLegacy.Exists
            If (gPartProject.PNR.RefNotesLegacy.Exists) Then
                txtLegacyRef_Notes.Text = gPartProject.PNR.RefNotesLegacy.Val
                txtLegacyRef_Notes_Rev.Text = gPartProject.PNR.RefNotesLegacy.Rev.Trim()
            Else
                txtLegacyRef_Notes.Text = ""
                txtLegacyRef_Notes_Rev.Text = ""
            End If

            txtDesign_MCS.Text = gPartProject.PNR.HW.MCrossSecNo '.MCS      'AES 09JAN18

            If (.IsWinnovation) Then
                cmbDesign_Winnovation.Text = "Y"
                txtDesign_WinnovationNo.Text = .WinnovationNo
                txtDesign_WinnovationNo.Enabled = True
            Else
                cmbDesign_Winnovation.Text = "N"
                txtDesign_WinnovationNo.Text = ""
                txtDesign_WinnovationNo.Enabled = False
            End If

            'cmbDesign_TemperType.Text = .TemperType

            If (.IsMat_OutsideVender) Then
                cmbDesign_OutsideVendor.Text = "Y"
            Else
                cmbDesign_OutsideVendor.Text = "N"
            End If

            txtDesign_FOD_Risks.Text = .FOD_Risks

            '....Material Section from HW
            If (gPartProject.PNR.SealType.ToString() = "SC") Then
                cmbDesign_Mat_Spring.Text = gPartProject.PNR.HW.MatName
                cmbDesign_Mat_Seal.Enabled = False
                cmbDesign_Mat_Spring.Enabled = True
            Else
                cmbDesign_Mat_Seal.Text = gPartProject.PNR.HW.MatName
                cmbDesign_Mat_Seal.Enabled = True
                cmbDesign_Mat_Spring.Enabled = False
            End If
            'cmbHT.Text = gPartProject.HT
            Dim pTemperCode As Integer = gPartProject.PNR.HW.Temper

            If (pTemperCode = 1) Then
                cmbDesign_TemperType.Text = "Work(Hardened)"
            ElseIf (pTemperCode = 2) Then
                cmbDesign_TemperType.Text = "Age(Hardened)"
            ElseIf (pTemperCode = 4) Then
                cmbDesign_TemperType.Text = "Annealed"
            ElseIf (pTemperCode = 6) Then
                cmbDesign_TemperType.Text = "Solution and Precip"
            ElseIf (pTemperCode = 8) Then
                cmbDesign_TemperType.Text = "NACE"
            End If

            If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                If (gPartProject.PNR.HW.Coating = "None") Then
                    chkCoating.Checked = False
                    cmbCoating.Text = ""
                Else
                    chkCoating.Checked = True
                    cmbCoating.Text = gPartProject.PNR.HW.Coating
                End If

                If (gPartProject.PNR.HW.SFinish = "0") Then
                    cmbSFinish.Text = ""
                Else
                    cmbSFinish.Text = gPartProject.PNR.HW.SFinish
                End If

            ElseIf (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                If (gPartProject.PNR.HW.Plating.Code <> "") Then
                    chkPlating.Checked = True
                    cmbPlatingCode.Text = gPartProject.PNR.HW.Plating.Code
                    cmbPlatingThickCode.Text = gPartProject.PNR.HW.Plating.ThickCode
                End If

            End If

            '....Input
            grdDesign_Input.Rows.Clear()
            For j As Integer = 0 To .Input.ID_Input.Count - 1
                Dim pCmbColDesc_Input As New DataGridViewComboBoxColumn
                pCmbColDesc_Input = grdDesign_Input.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(.Input.Desc(j))) Then
                    pVal = .Input.Desc(j)
                End If
                If (Not pCmbColDesc_Input.Items.Contains(pVal)) Then
                    pCmbColDesc_Input.Items.Add(pVal)
                End If
            Next

            For i As Integer = 0 To .Input.ID_Input.Count - 1
                grdDesign_Input.Rows.Add()
                If (Not IsNothing(.Input.Desc(i))) Then
                    grdDesign_Input.Rows(i).Cells(0).Value = .Input.Desc(i)
                Else
                    grdDesign_Input.Rows(i).Cells(0).Value = ""
                End If
                'grdDesign_Input.Rows(i).Cells(0).Value = .Input.Desc(i)
            Next

            '....Cust Spec
            grdDesign_CustSpec.Rows.Clear()
            For j As Integer = 0 To .CustSpec.ID_Cust.Count - 1
                Dim pCmbColDesc_CustSpec As New DataGridViewComboBoxColumn
                pCmbColDesc_CustSpec = grdDesign_CustSpec.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(.CustSpec.Type(j))) Then
                    pVal = .CustSpec.Type(j)
                End If
                If (Not pCmbColDesc_CustSpec.Items.Contains(pVal)) Then
                    pCmbColDesc_CustSpec.Items.Add(pVal)
                End If
            Next

            For i As Integer = 0 To .CustSpec.ID_Cust.Count - 1
                grdDesign_CustSpec.Rows.Add()
                If (Not IsNothing(.CustSpec.Type(i))) Then
                    grdDesign_CustSpec.Rows(i).Cells(0).Value = .CustSpec.Type(i)
                Else
                    grdDesign_CustSpec.Rows(i).Cells(0).Value = ""
                End If

                grdDesign_CustSpec.Rows(i).Cells(1).Value = .CustSpec.Desc(i)
                grdDesign_CustSpec.Rows(i).Cells(2).Value = .CustSpec.Interpret(i)

            Next

            '....Seal Dimension
            grdDesign_Seal.Rows.Clear()
            For j As Integer = 0 To .SealDim.ID_Seal.Count - 1
                Dim pCmbColDesc_SealDim As New DataGridViewComboBoxColumn
                pCmbColDesc_SealDim = grdDesign_Seal.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(.SealDim.Name(j))) Then
                    pVal = .SealDim.Name(j)
                End If
                If (Not pCmbColDesc_SealDim.Items.Contains(pVal)) Then
                    pCmbColDesc_SealDim.Items.Add(pVal)
                End If
            Next

            For i As Integer = 0 To .SealDim.ID_Seal.Count - 1
                grdDesign_Seal.Rows.Add()

                If (Not IsNothing(.SealDim.Name(i))) Then
                    grdDesign_Seal.Rows(i).Cells(0).Value = .SealDim.Name(i)
                Else
                    grdDesign_Seal.Rows(i).Cells(0).Value = ""
                End If

                'grdDesign_Seal.Rows(i).Cells(0).Value = .SealDim.Name(i)

                If (Math.Abs(.SealDim.Min(i)) > gcEPS) Then
                    grdDesign_Seal.Rows(i).Cells(1).Value = Format(.SealDim.Min(i), gUnit.LFormat) '.SealDim.Min(i).ToString("##0.000")
                Else
                    grdDesign_Seal.Rows(i).Cells(1).Value = ""
                End If

                If (Math.Abs(.SealDim.Nom(i)) > gcEPS) Then
                    grdDesign_Seal.Rows(i).Cells(2).Value = Format(.SealDim.Nom(i), gUnit.LFormat) '.SealDim.Nom(i).ToString("##0.000")
                Else
                    grdDesign_Seal.Rows(i).Cells(2).Value = "" '.SealDim.Nom(i).ToString("##0.000")
                End If

                If (Math.Abs(.SealDim.Max(i)) > gcEPS) Then
                    grdDesign_Seal.Rows(i).Cells(3).Value = Format(.SealDim.Max(i), gUnit.LFormat) '.SealDim.Max(i).ToString("##0.000")
                Else
                    grdDesign_Seal.Rows(i).Cells(3).Value = "" '.SealDim.Max(i).ToString("##0.000")
                End If

            Next

            txtDesign_LessonsLearned.Text = .LessonsLearned
            txtDesign_Notes.Text = .Notes

            grdDesign_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Design")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdDesign_EditedBy.Rows.Add()
                    grdDesign_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdDesign_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdDesign_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtDesign_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtDesign_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Design")) Then
                txtDesign_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtDesign_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkDesign_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtDesign_UserDate.Text = ""
                txtDesign_UserName.Text = ""
                chkDesign_UserSigned.Checked = False
            End If

        End With


        '.... "Manufacturing:"
        With mProcess_Project.Manf

            txtManf_MatPartNo_Base.Text = .BaseMat_PartNo
            txtManf_MatPartNo_Spring.Text = .SpringMat_PartNo
            txtManf_HT.Text = .HT
            cmbManf_PrecompressionGlue.Text = .PreComp_Glue

            '....Tool and Gages
            '........Desc
            grdManf_ToolNGage.Rows.Clear()
            For j As Integer = 0 To .ToolNGage.ID_Tool.Count - 1
                Dim pCmbColDesc_ToolGage As New DataGridViewComboBoxColumn
                pCmbColDesc_ToolGage = grdManf_ToolNGage.Columns.Item(1)
                Dim pVal As String = ""
                If (Not IsNothing(.ToolNGage.Desc(j))) Then
                    pVal = .ToolNGage.Desc(j)
                End If
                If (Not pCmbColDesc_ToolGage.Items.Contains(pVal)) Then
                    pCmbColDesc_ToolGage.Items.Add(pVal)
                End If
            Next

            For i As Integer = 0 To .ToolNGage.ID_Tool.Count - 1
                grdManf_ToolNGage.Rows.Add()
                grdManf_ToolNGage.Rows(i).Cells(0).Value = .ToolNGage.PartNo(i)
                If (Not IsNothing(.ToolNGage.Desc(i))) Then
                    grdManf_ToolNGage.Rows(i).Cells(1).Value = .ToolNGage.Desc(i)
                Else
                    grdManf_ToolNGage.Rows(i).Cells(1).Value = ""
                End If

                'grdManf_ToolNGage.Rows(i).Cells(1).Value = .ToolNGage.Desc(i)

                If ("Roll tooling" = .ToolNGage.Desc(i)) Then

                    Dim dgvcc As New DataGridViewComboBoxCell

                    dgvcc.Items.Clear()
                    dgvcc.Items.Add("E-Seal form")
                    dgvcc.Items.Add("Pre-form")
                    dgvcc.Items.Add("C-Ring")
                    grdManf_ToolNGage.Item(2, i) = dgvcc

                ElseIf ("Die" = .ToolNGage.Desc(i)) Then

                    Dim dgvcc As New DataGridViewComboBoxCell
                    dgvcc.Items.Clear()
                    dgvcc.Items.Add("Std")
                    dgvcc.Items.Add("Pre-form PF")
                    dgvcc.Items.Add("After-plate AP")
                    grdManf_ToolNGage.Item(2, i) = dgvcc

                ElseIf ("Window gauge" = .ToolNGage.Desc(i)) Then

                    Dim dgvcc As New DataGridViewComboBoxCell
                    dgvcc.Items.Clear()
                    dgvcc.Items.Add("Std")
                    dgvcc.Items.Add("3D gauge")
                    dgvcc.Items.Add("3D tooling")
                    grdManf_ToolNGage.Item(2, i) = dgvcc

                Else
                    Dim dgvcc As New DataGridViewComboBoxCell
                    dgvcc.Items.Clear()
                    grdManf_ToolNGage.Item(2, i) = dgvcc

                End If


                grdManf_ToolNGage.Rows(i).Cells(2).Value = .ToolNGage.Type(i)
                grdManf_ToolNGage.Rows(i).Cells(3).Value = .ToolNGage.Status(i)

                If (Math.Abs(.ToolNGage.LeadTime(i)) > gcEPS) Then
                    grdManf_ToolNGage.Rows(i).Cells(4).Value = gUnit.Format_Val(.ToolNGage.LeadTime(i)) '.ToolNGage.LeadTime(i)
                Else
                    grdManf_ToolNGage.Rows(i).Cells(4).Value = ""
                End If

                grdManf_ToolNGage.Rows(i).Cells(5).Value = .ToolNGage.DesignResponsibility(i)

            Next


            For i As Integer = 0 To .ToolNGage.ID_Tool.Count - 1
                If (.ToolNGage.Status(i) = "Buy") Then
                    grdPurchase_ToolNGages.Rows.Add()
                    grdPurchase_ToolNGages.Rows(i).Cells(0).Value = .ToolNGage.PartNo(i)
                    grdPurchase_ToolNGages.Rows(i).Cells(1).Value = .ToolNGage.Desc(i)
                    grdPurchase_ToolNGages.Rows(i).Cells(2).Value = .ToolNGage.Type(i)
                    'grdPurchase_ToolNGages.Rows(i).Cells(3).Value = .ToolNGage.Status(i)

                    If (Math.Abs(.ToolNGage.LeadTime(i)) > gcEPS) Then
                        grdPurchase_ToolNGages.Rows(i).Cells(3).Value = gUnit.Format_Val(.ToolNGage.LeadTime(i)) '.ToolNGage.LeadTime(i)
                    Else
                        grdPurchase_ToolNGages.Rows(i).Cells(3).Value = ""
                    End If

                    grdPurchase_ToolNGages.Rows(i).Cells(4).Value = .ToolNGage.DesignResponsibility(i)
                End If
            Next
            grdPurchase_ToolNGages.AllowUserToAddRows = False
            grdPurchase_ToolNGages.Enabled = False

            grdManf_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Manf")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdManf_EditedBy.Rows.Add()
                    grdManf_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdManf_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdManf_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtManf_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtManf_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Manf")) Then
                txtManf_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtManf_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkManf_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtManf_UserDate.Text = ""
                txtManf_UserName.Text = ""
                chkManf_UserSigned.Checked = False
            End If

        End With


        '....Purchasing
        With mProcess_Project.Purchase

            For j As Integer = 0 To .Mat.ID_Mat.Count - 1
                Dim pCmbColPurchase_Unit As New DataGridViewComboBoxColumn
                pCmbColPurchase_Unit = grdPurchase_Mat.Columns.Item(2)
                Dim pVal As String = ""
                If (Not IsNothing(mProcess_Project.Purchase.Mat.Qty_Unit(j))) Then
                    pVal = mProcess_Project.Purchase.Mat.Qty_Unit(j)
                End If
                If (Not pCmbColPurchase_Unit.Items.Contains(pVal)) Then
                    pCmbColPurchase_Unit.Items.Add(pVal)
                End If
            Next

            '....Material
            grdPurchase_Mat.Rows.Clear()
            For i As Integer = 0 To .Mat.ID_Mat.Count - 1
                grdPurchase_Mat.Rows.Add()
                grdPurchase_Mat.Rows(i).Cells(0).Value = .Mat.Item(i)
                If (Math.Abs(.Mat.EstQty(i)) > gcEPS) Then
                    grdPurchase_Mat.Rows(i).Cells(1).Value = .Mat.EstQty(i)
                Else
                    grdPurchase_Mat.Rows(i).Cells(1).Value = ""
                End If

                If (Not IsNothing(mProcess_Project.Purchase.Mat.Qty_Unit(i))) Then
                    grdPurchase_Mat.Rows(i).Cells(2).Value = mProcess_Project.Purchase.Mat.Qty_Unit(i)
                Else
                    grdPurchase_Mat.Rows(i).Cells(2).Value = ""
                End If

                grdPurchase_Mat.Rows(i).Cells(3).Value = .Mat.Status(i)

                If (Math.Abs(.Mat.LeadTime(i)) > gcEPS) Then
                    grdPurchase_Mat.Rows(i).Cells(4).Value = gUnit.Format_Val(.Mat.LeadTime(i)) '.Mat.LeadTime(i)
                Else
                    grdPurchase_Mat.Rows(i).Cells(4).Value = ""
                End If

            Next

            '....Drawing
            grdPurchase_Drawing.Rows.Clear()
            For i As Integer = 0 To .Dwg.ID_Dwg.Count - 1
                grdPurchase_Drawing.Rows.Add()
                grdPurchase_Drawing.Rows(i).Cells(0).Value = .Dwg.No(i)
                grdPurchase_Drawing.Rows(i).Cells(1).Value = .Dwg.Desc(i)

                If (Math.Abs(.Dwg.LeadTime(i)) > gcEPS) Then
                    grdPurchase_Drawing.Rows(i).Cells(2).Value = gUnit.Format_Val(.Dwg.LeadTime(i)) '.Dwg.LeadTime(i).ToString("#0.0")
                Else
                    grdPurchase_Drawing.Rows(i).Cells(2).Value = ""
                End If

            Next

            grdPurchase_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Purchase")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdPurchase_EditedBy.Rows.Add()
                    grdPurchase_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdPurchase_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdPurchase_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtPurchase_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtPurchase_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If


            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Purchase")) Then
                txtPurchase_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtPurchase_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkPurchase_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtPurchase_UserDate.Text = ""
                txtPurchase_UserName.Text = ""
                chkPurchase_UserSigned.Checked = False
            End If

        End With


        '.... "Quality:"
        With mProcess_Project.Qlty

            If (.IsApvdSupplierOnly) Then
                cmbQuality_ApprovedSupplier.Text = "Y"
            Else
                cmbQuality_ApprovedSupplier.Text = "N"
            End If

            If (.Separate_Tool_Gage_Reqd) Then
                cmbQuality_TNG.Text = "Y"
            Else
                cmbQuality_TNG.Text = "N"
            End If

            If (.HasCustComplaint) Then
                cmbQuality_CustComplaint.Text = "Y"
                txtQuality_Reason.Text = .Reason
            Else
                cmbQuality_CustComplaint.Text = "N"
                txtQuality_Reason.Text = ""
            End If

            If (.VisualInspection) Then
                cmbQuality_VisualInspection.Text = "Y"
                cmbQuality_VisualInspection_Type.Text = .VisualInspection_Type
            Else
                cmbQuality_VisualInspection.Text = "N"
                cmbQuality_VisualInspection_Type.Text = ""
            End If

            If (.SPC_Reqd) Then
                cmbQuality_SPC.Text = "Y"
            Else
                cmbQuality_SPC.Text = "N"
            End If

            If (.GageRnR_Reqd) Then
                cmbQuality_GageRnR_Reqd.Text = "Y"
            Else
                cmbQuality_GageRnR_Reqd.Text = "N"
            End If

            cmbQuality_CustAcceptStd.Text = .CustAcceptStd

            '....Spl Operation
            grdQuality_SplOperation.Rows.Clear()
            For j As Integer = 0 To mProcess_Project.Cost.SplOperation.Desc.Count - 1
                Dim pCmbColDesc_Cost As New DataGridViewComboBoxColumn
                pCmbColDesc_Cost = grdQuality_SplOperation.Columns.Item(0)
                Dim pVal As String = ""
                If (Not IsNothing(mProcess_Project.Cost.SplOperation.Desc(j))) Then
                    pVal = mProcess_Project.Cost.SplOperation.Desc(j)
                End If
                If (Not pCmbColDesc_Cost.Items.Contains(pVal)) Then
                    pCmbColDesc_Cost.Items.Add(pVal)
                End If
            Next

            '....Spl Operation
            For j As Integer = 0 To mProcess_Project.Cost.SplOperation.Desc.Count - 1
                grdQuality_SplOperation.Rows.Add()
                If (Not IsNothing(mProcess_Project.Cost.SplOperation.Desc(j))) Then
                    grdQuality_SplOperation.Rows(j).Cells(0).Value = mProcess_Project.Cost.SplOperation.Desc(j)
                Else
                    grdQuality_SplOperation.Rows(j).Cells(0).Value = ""
                End If

                'grdQuality_SplOperation.Rows(j).Cells(0).Value = mProcess_Project.Cost.SplOperation.Desc(j)
                grdQuality_SplOperation.Rows(j).Cells(1).Value = mProcess_Project.Cost.SplOperation.Spec(j)
                grdQuality_SplOperation.Rows(j).Cells(2).Value = gUnit.Format_Val(mProcess_Project.Cost.SplOperation.LeadTime(j)) 'mProcess_Project.Cost.SplOperation.LeadTime(j)
                grdQuality_SplOperation.Rows(j).Cells(3).Value = mProcess_Project.Cost.SplOperation.Cost(j).ToString("#0.##")
            Next

            grdQuality_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Qlty")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdQuality_EditedBy.Rows.Add()
                    grdQuality_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdQuality_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdQuality_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtQuality_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtQuality_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Qlty")) Then
                txtQuality_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtQuality_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkQuality_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtQuality_UserDate.Text = ""
                txtQuality_UserName.Text = ""
                chkQuality_UserSigned.Checked = False
            End If

        End With

        '.... "Drawing:"
        With mProcess_Project.Dwg
            cmbDwg_DesignLevel.Text = .DesignLevel

            grdDrawing_Needed.Rows.Clear()
            For j As Integer = 0 To .Needed.ID_Needed.Count - 1
                grdDrawing_Needed.Rows.Add()
                grdDrawing_Needed.Rows(j).Cells(0).Value = .Needed.DwgNo(j)
                grdDrawing_Needed.Rows(j).Cells(1).Value = .Needed.Desc(j)
                grdDrawing_Needed.Rows(j).Cells(2).Value = .Needed.Status(j)

                If (Math.Abs(.Needed.LeadTime(j)) > gcEPS) Then
                    grdDrawing_Needed.Rows(j).Cells(3).Value = gUnit.Format_Val(.Needed.LeadTime(j)) '.Needed.LeadTime(j)
                Else
                    grdDrawing_Needed.Rows(j).Cells(3).Value = ""
                End If
            Next

            For j As Integer = 0 To .BOM.ID_BOM.Count - 1
                grdDrawing_BOM.Rows.Add()
                grdDrawing_BOM.Rows(j).Cells(0).Value = .BOM.Parent_PartNo(j)
                grdDrawing_BOM.Rows(j).Cells(1).Value = .BOM.Child_PartNo(j)

                If (.BOM.Qty(j) > 0) Then
                    grdDrawing_BOM.Rows(j).Cells(2).Value = .BOM.Qty(j)
                Else
                    grdDrawing_BOM.Rows(j).Cells(2).Value = ""
                End If
            Next

            'If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Dwg")) Then
            '    If (mProcess_Project.EditedBy.Name <> "") Then
            '        If (mProcess_Project.EditedBy.DateEdited <> DateTime.MinValue) Then
            '            grdDwg_EditedBy.Rows(0).Cells(0).Value = mProcess_Project.EditedBy.DateEdited.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            '        Else
            '            grdDwg_EditedBy.Rows(0).Cells(0).Value = ""
            '        End If

            '        grdDwg_EditedBy.Rows(0).Cells(1).Value = mProcess_Project.EditedBy.Name
            '        grdDwg_EditedBy.Rows(0).Cells(2).Value = mProcess_Project.EditedBy.Comment
            '    End If
            'End If

            grdDwg_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Dwg")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdDwg_EditedBy.Rows.Add()
                    grdDwg_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdDwg_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdDwg_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtDwg_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtDwg_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Dwg")) Then
                txtDwg_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtDwg_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkDwg_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtDwg_UserDate.Text = ""
                txtDwg_UserName.Text = ""
                chkDwg_UserSigned.Checked = False
            End If

        End With


        '.... "Testing:"
        With mProcess_Project.Test

            If (.IsNeeded()) Then
                chkTest.Checked = True
            Else
                chkTest.Checked = True
                chkTest.Checked = False
            End If

            txtTest_Other.Text = .Other

            '...Leak
            If (Math.Abs(.Leak.Compress_Unplated) > gcEPS) Then
                txtTest_CompressPre_Leak.Text = Format(.Leak.Compress_Unplated, gUnit.LFormat) '.Leak.Compress_Unplated.ToString("#0.###")
            Else
                txtTest_CompressPre_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Compress_Plated) > gcEPS) Then
                txtTest_CompressPost_Leak.Text = Format(.Leak.Compress_Plated, gUnit.LFormat) '.Leak.Compress_Plated.ToString("#0.###")
            Else
                txtTest_CompressPost_Leak.Text = ""
            End If

            If (.Leak.Medium_Unplated <> "") Then
                cmbTest_MediaPre_Leak.Text = .Leak.Medium_Unplated
            Else
                cmbTest_MediaPre_Leak.Text = -1
            End If

            If (.Leak.Medium_Plated <> "") Then
                cmbTest_MediaPost_Leak.Text = .Leak.Medium_Plated
            Else
                cmbTest_MediaPost_Leak.Text = -1
            End If

            If (Math.Abs(.Leak.Press_Unplated) > gcEPS) Then
                txtTest_PressPre_Leak.Text = gUnit.Format_Val(.Leak.Press_Unplated) '.Leak.Press_Unplated.ToString("#0.###")
            Else
                txtTest_PressPre_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Press_Plated) > gcEPS) Then
                txtTest_PressPost_Leak.Text = gUnit.Format_Val(.Leak.Press_Plated) '.Leak.Press_Plated.ToString("#0.###")
            Else
                txtTest_PressPost_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Max_Unplated) > gcEPS) Then
                txtTest_ReqPre_Leak.Text = gUnit.Format_LeakVal(.Leak.Max_Unplated) 'Format(.Leak.Max_Unplated, gUnit.LFormat) '.Leak.Max_Unplated.ToString("#0.###")
            Else
                txtTest_ReqPre_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Max_Plated) > gcEPS) Then
                txtTest_ReqPost_Leak.Text = gUnit.Format_LeakVal(.Leak.Max_Plated) 'Format(.Leak.Max_Plated, gUnit.LFormat) '.Leak.Max_Plated.ToString("#0.###")
            Else
                txtTest_ReqPost_Leak.Text = ""
            End If

            cmbTest_QtyPre_Leak.Text = .Leak.Qty_Unplated
            cmbTest_QtyPost_Leak.Text = .Leak.Qty_Plated

            cmbTest_FreqPre_Leak.Text = .Leak.Freq_Unplated
            cmbTest_FreqPost_Leak.Text = .Leak.Freq_Plated

            '...Load
            If (Math.Abs(.Load.Compress_Unplated) > gcEPS) Then
                txtTest_CompressPre_Load.Text = Format(.Load.Compress_Unplated, gUnit.LFormat) '.Load.Compress_Unplated.ToString("#0.###")
            Else
                txtTest_CompressPre_Load.Text = ""
            End If

            If (Math.Abs(.Load.Compress_Plated) > gcEPS) Then
                txtTest_CompressPost_Load.Text = Format(.Load.Compress_Plated, gUnit.LFormat) '.Load.Compress_Plated.ToString("#0.###")
            Else
                txtTest_CompressPost_Load.Text = ""
            End If

            If (Math.Abs(.Load.Max_Unplated) > gcEPS) Then
                txtTest_ReqPre_Load.Text = Format(.Load.Max_Unplated, gUnit.LFormat) '.Load.Max_Unplated.ToString("#0.###")
            Else
                txtTest_ReqPre_Load.Text = ""
            End If

            If (Math.Abs(.Load.Max_Plated) > gcEPS) Then
                txtTest_ReqPost_Load.Text = Format(.Load.Max_Plated, gUnit.LFormat) '.Load.Max_Plated.ToString("#0.###")
            Else
                txtTest_ReqPost_Load.Text = ""
            End If

            cmbTest_QtyPre_Load.Text = .Load.Qty_Unplated
            cmbTest_QtyPost_Load.Text = .Load.Qty_Plated

            cmbTest_FreqPre_Load.Text = .Load.Freq_Unplated
            cmbTest_FreqPost_Load.Text = .Load.Freq_Plated

            '....SpringBack
            If (Math.Abs(.SpringBack.Compress_Unplated) > gcEPS) Then
                txtTest_CompressPre_SpringBack.Text = Format(.SpringBack.Compress_Unplated, gUnit.LFormat) '.SpringBack.Compress_Unplated.ToString("#0.###")
            Else
                txtTest_CompressPre_SpringBack.Text = ""
            End If

            If (Math.Abs(.SpringBack.Compress_Plated) > gcEPS) Then
                txtTest_CompressPost_SpringBack.Text = Format(.SpringBack.Compress_Plated, gUnit.LFormat) '.SpringBack.Compress_Plated.ToString("#0.###")
            Else
                txtTest_CompressPost_SpringBack.Text = ""
            End If

            If (Math.Abs(.SpringBack.Max_Unplated) > gcEPS) Then
                txtTest_ReqPre_SpringBack.Text = Format(.SpringBack.Max_Unplated, gUnit.LFormat) '.SpringBack.Max_Unplated.ToString("#0.###")
            Else
                txtTest_ReqPre_SpringBack.Text = ""
            End If

            If (Math.Abs(.SpringBack.Max_Plated) > gcEPS) Then
                txtTest_ReqPost_SpringBack.Text = Format(.SpringBack.Max_Plated, gUnit.LFormat) '.SpringBack.Max_Plated.ToString("#0.###")
            Else
                txtTest_ReqPost_SpringBack.Text = ""
            End If

            cmbTest_QtyPre_SpringBack.Text = .SpringBack.Qty_Unplated
            cmbTest_QtyPost_SpringBack.Text = .SpringBack.Qty_Plated

            cmbTest_FreqPre_SpringBack.Text = .SpringBack.Freq_Unplated
            cmbTest_FreqPost_SpringBack.Text = .SpringBack.Freq_Plated

            'If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Test")) Then
            '    If (mProcess_Project.EditedBy.Name <> "") Then
            '        If (mProcess_Project.EditedBy.DateEdited <> DateTime.MinValue) Then
            '            grdTest_EditedBy.Rows(0).Cells(0).Value = mProcess_Project.EditedBy.DateEdited.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            '        Else
            '            grdTest_EditedBy.Rows(0).Cells(0).Value = ""
            '        End If

            '        grdTest_EditedBy.Rows(0).Cells(1).Value = mProcess_Project.EditedBy.Name
            '        grdTest_EditedBy.Rows(0).Cells(2).Value = mProcess_Project.EditedBy.Comment
            '    End If
            'End If

            grdTest_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Test")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdTest_EditedBy.Rows.Add()
                    grdTest_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdTest_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdTest_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtTest_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtTest_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Test")) Then
                txtTest_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtTest_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkTest_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtTest_UserDate.Text = ""
                txtTest_UserName.Text = ""
                chkTest_UserSigned.Checked = False
            End If

        End With


        '.... "Planning:"

        '....Planning
        ''With mProcess_Project.Planning

        ''    For j As Integer = 0 To .SplOperation.ID_SplOperation.Count - 1
        ''        grdPlanning_Ordered.Rows.Add()
        ''        grdPlanning_Ordered.Rows(j).Cells(0).Value = .SplOperation.Desc(j)

        ''        If (Math.Abs(.SplOperation.LeadTimeStart(j)) > gcEPS) Then
        ''            grdPlanning_Ordered.Rows(j).Cells(1).Value = .SplOperation.LeadTimeStart(j)
        ''        Else
        ''            grdPlanning_Ordered.Rows(j).Cells(1).Value = ""
        ''        End If
        ''    Next

        ''    grdPlanning_Ordered.AllowUserToAddRows = False

        ''    For j As Integer = 0 To .MileOperation.ID_MileOperation.Count - 1
        ''        grdPlanning_MileStoneOperation.Rows.Add()
        ''        grdPlanning_MileStoneOperation.Rows(j).Cells(0).Value = .MileOperation.Name(j)

        ''        If (Math.Abs(.MileOperation.LeadTime(j)) > gcEPS) Then
        ''            grdPlanning_MileStoneOperation.Rows(j).Cells(1).Value = .MileOperation.LeadTime(j)
        ''        Else
        ''            grdPlanning_MileStoneOperation.Rows(j).Cells(1).Value = ""
        ''        End If
        ''    Next

        ''    lstPlanning_Notes_Dim.Items.Clear()
        ''    For i As Integer = 0 To mProcess_Project.Cost.SplOperation.ID_Cost.Count - 1
        ''        Dim pName As String = mProcess_Project.Cost.SplOperation.Desc(i)

        ''        Dim pFlag As Boolean = False
        ''        For j As Integer = 0 To .SplOperation.ID_SplOperation.Count - 1
        ''            If (pName = .SplOperation.Desc(j)) Then
        ''                pFlag = True
        ''                Exit For
        ''            End If
        ''        Next
        ''        If (Not pFlag) Then
        ''            lstPlanning_Notes_Dim.Items.Add(pName)
        ''        End If
        ''    Next

        ''    txtPlanning_Notes.Text = .Notes

        ''End With


        '.... "Shipping:"
        With mProcess_Project.Shipping

            Dim pRowCount As Integer = 0
            For i As Integer = 0 To mProcess_Project.Design.CustSpec.ID_Cust.Count - 1
                If (mProcess_Project.Design.CustSpec.Type(i) = "Packaging") Then
                    grdShipping_CustSpec.Rows.Add()
                    grdShipping_CustSpec.Rows(pRowCount).Cells(0).Value = mProcess_Project.Design.CustSpec.Desc(i)
                    grdShipping_CustSpec.Rows(pRowCount).Cells(1).Value = mProcess_Project.Design.CustSpec.Interpret(i)
                    pRowCount = pRowCount + 1
                End If
            Next
            grdShipping_CustSpec.AllowUserToAddRows = False
            'grpCustSpec_Shipping.Enabled = False
            txtShipping_Notes.Text = .Notes

            grdShipping_EditedBy.Rows.Clear()
            If (mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Shipping")) Then
                For j As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    grdShipping_EditedBy.Rows.Add()
                    grdShipping_EditedBy.Rows(j).Cells(0).Value = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    grdShipping_EditedBy.Rows(j).Cells(1).Value = mProcess_Project.EditedBy.Name(j)
                    grdShipping_EditedBy.Rows(j).Cells(2).Value = mProcess_Project.EditedBy.Comment(j)

                    txtShipping_UserDate.Text = mProcess_Project.EditedBy.DateEdited(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    txtShipping_UserName.Text = mProcess_Project.EditedBy.Name(j)
                Next
            End If

            If (mProcess_Project.EditedBy.RetrieveFromDB_UserSignOff(mProcess_Project.ID, "Shipping")) Then
                txtShipping_UserDate.Text = mProcess_Project.EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtShipping_UserName.Text = mProcess_Project.EditedBy.User.Name
                chkShipping_UserSigned.Checked = mProcess_Project.EditedBy.User.Signed
            Else
                txtShipping_UserDate.Text = ""
                txtShipping_UserName.Text = ""
                chkShipping_UserSigned.Checked = False
            End If

        End With


        '.... "IssueComment:"
        With mProcess_Project.IssueCommnt

            For i As Integer = 0 To .ID.Count - 1

                grdIssueComment.Rows.Add()
                'grdIssueComment.Rows(i).Cells(0).Value = .SlNo(i)
                grdIssueComment.Rows(i).Cells(0).Value = .Comment(i)
                grdIssueComment.Rows(i).Cells(1).Value = .ByDept(i)
                grdIssueComment.Rows(i).Cells(2).Value = .ByName(i)
                grdIssueComment.Rows(i).Cells(3).Value = .ByDate(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                grdIssueComment.Rows(i).Cells(4).Value = .ToDept(i)
                'grdIssueComment.Rows(i).Cells(6).Value = .Resolved(i)

                If (.Resolved(i)) Then
                    grdIssueComment.Rows(i).Cells(5).Value = "Y"
                    grdIssueComment.Rows(i).Cells(6).Value = .Name(i)
                    grdIssueComment.Rows(i).Cells(7).Value = .DateResolution(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())

                    grdIssueComment.Rows(i).Cells(8).Value = .Resolution(i)
                Else
                    grdIssueComment.Rows(i).Cells(5).Value = "N"
                    grdIssueComment.Rows(i).Cells(6).Value = .Name(i)
                    grdIssueComment.Rows(i).Cells(7).Value = ""

                    grdIssueComment.Rows(i).Cells(8).Value = .Resolution(i)
                End If

            Next
        End With


        '.... "Approval:"
        With mProcess_Project.Approval

            Try

                For j As Integer = 0 To .ID_Approval.Count - 1

                    'Dim dgvcc As New DataGridViewComboBoxCell
                    'dgvcc.Items.Clear()
                    'Dim pFlag As Boolean = False
                    'dgvcc = grdApproval_Attendees.Item(1, j)
                    'If (Not dgvcc.Items.Contains(.Name(j))) Then
                    '    dgvcc.Items.Add(.Name(j))
                    '    pFlag = True
                    'End If

                    'If (pFlag) Then
                    '    grdApproval_Attendees.Item(1, j) = dgvcc
                    'End If

                    PopulateAttendies(.Name(j), j)

                    grdApproval_Attendees.Rows(j).Cells(1).Value = .Name(j)
                    grdApproval_Attendees.Rows(j).Cells(2).Value = .Title(j)
                    grdApproval_Attendees.Rows(j).Cells(3).Value = .Signed(j)

                    If (.Signed(j)) Then

                        grdApproval_Attendees.Rows(j).Cells(4).Value = .DateSigned(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                    Else
                        grdApproval_Attendees.Rows(j).Cells(4).Value = ""
                    End If

                Next

            Catch ex As Exception

            End Try

        End With

        'AES 06APR18
        For i As Integer = 0 To grdApproval_Attendees.Rows.Count - 1
            Dim pUserName As String = gUser.FirstName + " " + gUser.LastName
            If (gUser.Role = grdApproval_Attendees.Rows(i).Cells(0).Value) Then
                If (grdApproval_Attendees.Rows(i).Cells(3).Value = True) Then
                    If (pUserName = grdApproval_Attendees.Rows(i).Cells(1).Value) Then
                        grdApproval_Attendees.ReadOnly = False
                        grdApproval_Attendees.EnableHeadersVisualStyles = False
                        grdApproval_Attendees.Rows(i).HeaderCell.Style.BackColor = Color.Yellow

                        For j As Integer = 0 To grdApproval_Attendees.ColumnCount - 1
                            grdApproval_Attendees.Rows(i).Cells(j).Style.ForeColor = Color.Black
                            grdApproval_Attendees.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                        Next
                    Else
                        grdApproval_Attendees.ReadOnly = True
                    End If
                Else
                    grdApproval_Attendees.ReadOnly = False
                    grdApproval_Attendees.EnableHeadersVisualStyles = False
                    grdApproval_Attendees.Rows(i).HeaderCell.Style.BackColor = Color.Yellow
                    For j As Integer = 0 To grdApproval_Attendees.ColumnCount - 1
                        grdApproval_Attendees.Rows(i).Cells(j).Style.ForeColor = Color.Black
                        grdApproval_Attendees.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                    Next
                End If
            Else
                grdApproval_Attendees.ReadOnly = True
            End If
        Next

        grdApproval_Attendees.Columns(3).ReadOnly = True

        SetLabel_Unit_Cust()

    End Sub

    Private Sub PopulateAttendies(ByVal Name_In As String, ByVal RowIndex_In As Integer)
        '===============================================================================        '20APR18

        Dim pSealSuiteEntities As New SealSuiteDBEntities

        '....Attendies
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole
                        Where pRec.fldRole <> "Admin" Select pRec).ToList()
        If (pQryRole.Count > 0) Then

            grdApproval_Attendees.Rows(RowIndex_In).Cells(0).Value = pQryRole(RowIndex_In).fldRole
            Dim pRoleID As Integer = pQryRole(RowIndex_In).fldID

            Dim dgvcc As New DataGridViewComboBoxCell

                Dim pQryUserRole = (From pRec In pSealSuiteEntities.tblProcess_UserRole
                                    Where pRec.fldRoleID = pRoleID Select pRec).ToList()

            If (pQryUserRole.Count > 0) Then
                Dim pListUserID As New List(Of Integer)
                For j As Integer = 0 To pQryUserRole.Count - 1
                    Dim pUserID As Integer = pQryUserRole(j).fldUserID
                    If (Not pListUserID.Contains(pUserID)) Then
                        pListUserID.Add(pUserID)
                    End If

                Next

                For k As Integer = 0 To pListUserID.Count - 1
                    Dim pID As Integer = pListUserID(k)
                    Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser
                                    Where pRec.fldID = pID Select pRec).ToList()

                    If (pQryUser.Count > 0) Then
                        Dim pUserName As String = pQryUser(0).fldFirstName & " " & pQryUser(0).fldLastName
                        If (Not mUserName.Contains(pUserName)) Then
                            mUserName.Add(pUserName)
                            mUserID.Add(pID)
                        End If

                        dgvcc.Items.Add(pUserName)

                    End If

                Next

            End If

            If (Not IsNothing(Name_In)) Then
                If (Not dgvcc.Items.Contains(Name_In)) Then
                    dgvcc.Items.Add(Name_In)
                End If
            End If

            grdApproval_Attendees.Item(1, RowIndex_In) = dgvcc

        End If

    End Sub

    Private Sub SetLabel_Unit_Cust()
        '===========================
        If (mProcess_Project.Unit.LUnit_Cust = "in") Then
            gUnit.SetLFormat("English")
        Else
            gUnit.SetLFormat("Metric")
        End If

        '....Application
        lblApp_MaxLeak_Unit.Text = mProcess_Project.Unit.LeakUnit_Cust
        lblApp_T_Unit.Text = mProcess_Project.Unit.TUnit_Cust
        lblApp_Press_Unit.Text = mProcess_Project.Unit.PUnit_Cust
        grpApp_Load.Text = "Load (" & mProcess_Project.Unit.FUnit_Cust & "/" & mProcess_Project.Unit.LUnit_Cust & "):"
        grpApp_Face_Cavity.Text = "Cavity Dimensions (" & mProcess_Project.Unit.LUnit_Cust & "):"
        grpApp_Axial_Cavity.Text = "Cavity Dimensions (" & mProcess_Project.Unit.LUnit_Cust & "):"

        '....Design
        grpDesign_SealDim.Text = "Seal Dimensions (" & mProcess_Project.Unit.LUnit_Cust & "):"

        '....Testing
        lblTest_CompressTo_Unit_Leak.Text = mProcess_Project.Unit.LUnit_Cust
        lblTest_CompressTo_Unit_Load.Text = mProcess_Project.Unit.LUnit_Cust
        lblTest_CompressTo_Unit_SpringBack.Text = mProcess_Project.Unit.LUnit_Cust
        lblTest_Press_Unit_Leak.Text = mProcess_Project.Unit.PUnit_Cust
        lblTest_Req_Unit_Leak.Text = mProcess_Project.Unit.LeakUnit_Cust
        lblTest_Req_Unit_Load.Text = mProcess_Project.Unit.FUnit_Cust & "/" & mProcess_Project.Unit.LUnit_Cust
        lblTest_Req_Unit_SpringBack.Text = mProcess_Project.Unit.LUnit_Cust

    End Sub

    Private Sub SetLabel_Unit_PH()
        '===========================
        If (mProcess_Project.Unit.LUnit_PH = "in") Then
            gUnit.SetLFormat("English")
        Else
            gUnit.SetLFormat("Metric")
        End If

        '....Application
        lblApp_MaxLeak_Unit.Text = mProcess_Project.Unit.LeakUnit_PH
        lblApp_T_Unit.Text = mProcess_Project.Unit.TUnit_PH
        lblApp_Press_Unit.Text = mProcess_Project.Unit.PUnit_PH
        grpApp_Load.Text = "Load (" & mProcess_Project.Unit.FUnit_PH & "/" & mProcess_Project.Unit.LUnit_PH & "):"
        grpApp_Face_Cavity.Text = "Cavity Dimensions (" & mProcess_Project.Unit.LUnit_PH & "):"
        grpApp_Axial_Cavity.Text = "Cavity Dimensions (" & mProcess_Project.Unit.LUnit_PH & "):"

        '....Design
        grpDesign_SealDim.Text = "Seal Dimensions (" & mProcess_Project.Unit.LUnit_PH & "):"

        '....Testing
        lblTest_CompressTo_Unit_Leak.Text = mProcess_Project.Unit.LUnit_PH
        lblTest_CompressTo_Unit_Load.Text = mProcess_Project.Unit.LUnit_PH
        lblTest_CompressTo_Unit_SpringBack.Text = mProcess_Project.Unit.LUnit_PH
        lblTest_Press_Unit_Leak.Text = mProcess_Project.Unit.PUnit_PH
        lblTest_Req_Unit_Leak.Text = mProcess_Project.Unit.LeakUnit_PH
        lblTest_Req_Unit_Load.Text = mProcess_Project.Unit.FUnit_PH & "/" & mProcess_Project.Unit.LUnit_PH
        lblTest_Req_Unit_SpringBack.Text = mProcess_Project.Unit.LUnit_PH

    End Sub

    Private Sub CompareControls()
        '========================
        For Each txtBox In Me.Controls.OfType(Of TextBox)()
            If txtBox.Modified Then
                'Show message
            End If
        Next

        For Each cmbBox In Me.Controls.OfType(Of ComboBox)()
            'If cmbBox.TextUpdate() Then
            'Show message
            'End If
        Next

    End Sub


#Region "SUB-HELPER ROUTINES:"

    Private Sub RetrieveFromDB_PartProj()
        '================================
        Try

            Dim pPartEntities As New SealPartDBEntities()

            '....HW_Face table
            Dim pHWFace_Rec_Count As Integer = (From HWFace In pPartEntities.tblHW_Face
                                                Where HWFace.fldPNID = mPNID And
                                                HWFace.fldRevID = mRevID Select HWFace).Count()
            If (pHWFace_Rec_Count > 0) Then

                Dim pHWFace_Rec = (From HWFace In pPartEntities.tblHW_Face
                                   Where HWFace.fldPNID = mPNID And
                                            HWFace.fldRevID = mRevID Select HWFace).First()

                Dim pType As String = pHWFace_Rec.fldType.ToString().Trim()
                gPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), pType), clsPartProject.clsPNR.eType)
                gPartProject.PNR.HW.InitializePNR(gPartProject.PNR)

                If (gPartProject.PNR.Legacy.Exists And gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then

                    Dim pSealType As String = pHWFace_Rec.fldType.ToString().Trim()

                    With gPartProject.PNR.HW
                        gPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), pSealType), clsPartProject.clsPNR.eType) 'pSealType
                        .MCrossSecNo = pHWFace_Rec.fldMCS
                        '.Hfree = pHWFace_Rec.fldHfreeStd
                        .HFreeTol(1) = pHWFace_Rec.fldHFreeTol1
                        .HFreeTol(2) = pHWFace_Rec.fldHFreeTol2
                        .T = .TStd      'AES 02AUG17

                        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                            '....HW_AdjCSeal table
                            Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
                                                                     Where HWFace_AdjCSeal.fldPNID = mPNID And
                                                            HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).Count()
                            If (pHW_AdjCSeal_Rec_Count > 0) Then

                                Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
                                                            Where HWFace_AdjCSeal.fldPNID = mPNID And
                                                            HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).First()

                                With gPartProject.PNR.HW
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

                            If (Not IsDBNull(pHWFace_Rec.fldIsPlating) And Not IsNothing(pHWFace_Rec.fldIsPlating)) Then
                                .PlatingExists = pHWFace_Rec.fldIsPlating
                            End If

                            If (Not IsDBNull(pHWFace_Rec.fldPlatingThickCode) And Not IsNothing(pHWFace_Rec.fldPlatingThickCode)) Then
                                .PlatingThickCode = pHWFace_Rec.fldPlatingThickCode

                                If (.Plating.ThickCode = "X") Then
                                    .PlatingThickMin = pHWFace_Rec.fldPlatingThickMin
                                    .PlatingThickMax = pHWFace_Rec.fldPlatingThickMax

                                Else
                                    Dim pMCSEntities As New SealIPEMCSDBEntities()

                                    Dim pThickCode As String = .Plating.ThickCode
                                    Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = pThickCode
                                                Select pRec).ToList()

                                    If (pQry.Count() > 0) Then

                                        If (gPartProject.PNR.HW.UnitSystem = "English") Then
                                            .PlatingThickMin = pQry(0).fldPlatingThickMinEng
                                            .PlatingThickMax = pQry(0).fldPlatingThickMaxEng
                                        Else
                                            .PlatingThickMin = pQry(0).fldPlatingThickMinMet
                                            .PlatingThickMax = pQry(0).fldPlatingThickMaxMet
                                        End If
                                    End If

                                End If
                            Else
                                .PlatingThickCode = ""
                                .PlatingThickMin = 0
                                .PlatingThickMax = 0
                            End If

                        ElseIf (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then

                            '....HW_AdjESeal table
                            Dim pHW_AdjESeal_Rec_Count As Integer = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
                                                                     Where HWFace_AdjESeal.fldPNID = mPNID And
                                                            HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).Count()
                            If (pHW_AdjESeal_Rec_Count > 0) Then

                                Dim pHWFace_AdjESeal_Rec = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
                                                            Where HWFace_AdjESeal.fldPNID = mPNID And
                                                            HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).First()

                                With gPartProject.PNR.HW
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

                        Exit Sub
                    End With

                Else
                    If (gPartProject.PNR.Current.Exists) Then
                        Dim pPN As String = gPartProject.PNR.PN
                        Dim pSealType As String = ""
                        Dim pSealType_No As String = pPN.Substring(3, 2)

                        Select Case pSealType_No

                            Case "69"
                                pSealType = "E"

                            Case "76"
                                pSealType = "C"

                            Case "79"
                                pSealType = "U"

                            Case "44"
                                pSealType = "SC"

                        End Select

                        If (pSealType <> pHWFace_Rec.fldType) Then
                            Exit Sub
                        End If

                    End If

                End If

                With gPartProject.PNR.HW
                    .POrient = pHWFace_Rec.fldPOrient
                    .MCrossSecNo = pHWFace_Rec.fldMCS
                    .IsSegmented = pHWFace_Rec.fldSegmented
                    If (.IsSegmented) Then
                        .CountSegment = pHWFace_Rec.fldSegmentCount
                    End If

                    .MatName = pHWFace_Rec.fldMatName
                    .HT = pHWFace_Rec.fldHT
                    .Temper = pHWFace_Rec.fldTemper
                    .T = .TStd      'AES 31JUL17

                    If (Not IsDBNull(pHWFace_Rec.fldCoating) And Not IsNothing(pHWFace_Rec.fldCoating)) Then
                        .Coating = pHWFace_Rec.fldCoating
                    Else
                        .Coating = "None"
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldSFinish) And Not IsNothing(pHWFace_Rec.fldSFinish)) Then
                        .SFinish = pHWFace_Rec.fldSFinish
                    Else
                        .SFinish = 0
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldIsPlating) And Not IsNothing(pHWFace_Rec.fldIsPlating)) Then
                        .PlatingExists = pHWFace_Rec.fldIsPlating
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldPlatingCode) And Not IsNothing(pHWFace_Rec.fldPlatingCode)) Then
                        .PlatingCode = pHWFace_Rec.fldPlatingCode

                    Else
                        .PlatingCode = ""
                    End If

                    If (Not IsDBNull(pHWFace_Rec.fldPlatingThickCode) And Not IsNothing(pHWFace_Rec.fldPlatingThickCode)) Then
                        .PlatingThickCode = pHWFace_Rec.fldPlatingThickCode

                        If (.Plating.ThickCode = "X") Then
                            .PlatingThickMin = pHWFace_Rec.fldPlatingThickMin
                            .PlatingThickMax = pHWFace_Rec.fldPlatingThickMax

                        Else
                            Dim pMCSEntities As New SealIPEMCSDBEntities()

                            Dim pThickCode As String = .Plating.ThickCode
                            Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = pThickCode
                                        Select pRec).ToList()

                            If (pQry.Count() > 0) Then

                                If (gPartProject.PNR.HW.UnitSystem = "English") Then
                                    .PlatingThickMin = pQry(0).fldPlatingThickMinEng
                                    .PlatingThickMax = pQry(0).fldPlatingThickMaxEng
                                Else
                                    .PlatingThickMin = pQry(0).fldPlatingThickMinMet
                                    .PlatingThickMax = pQry(0).fldPlatingThickMaxMet
                                End If
                            Else
                                .PlatingThickCode = ""
                                .PlatingThickMin = 0
                                .PlatingThickMax = 0
                            End If

                        End If
                    Else
                        .PlatingThickCode = ""
                        .PlatingThickMin = 0
                        .PlatingThickMax = 0
                    End If

                    '.Hfree = pHWFace_Rec.fldHfreeStd
                    .HFreeTol(1) = pHWFace_Rec.fldHFreeTol1
                    .HFreeTol(2) = pHWFace_Rec.fldHFreeTol2
                    .DControl = pHWFace_Rec.fldDControl
                    '.H11Tol = pHWFace_Rec.fldH11Tol

                End With

            End If

            If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                '....HW_AdjCSeal table
                Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
                                                         Where HWFace_AdjCSeal.fldPNID = mPNID And
                                                HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).Count()
                If (pHW_AdjCSeal_Rec_Count > 0) Then

                    Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
                                                Where HWFace_AdjCSeal.fldPNID = mPNID And
                                                    HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).First()

                    With gPartProject.PNR.HW
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

            ElseIf (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then

                '....HW_AdjESeal table
                Dim pHW_AdjESeal_Rec_Count As Integer = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
                                                         Where HWFace_AdjESeal.fldPNID = mPNID And
                                                 HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).Count()
                If (pHW_AdjESeal_Rec_Count > 0) Then

                    Dim pHWFace_AdjESeal_Rec = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
                                                Where HWFace_AdjESeal.fldPNID = mPNID And
                                                    HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).First()

                    With gPartProject.PNR.HW
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
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PopulateMarketingMgr()
        '============================
        cmbMgrPreOrder.Items.Clear()
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pRole As String = "ProgramManager"
        Dim pQryRole = (From pRec In pSealSuiteEntities.tblRole
                        Where pRec.fldRole = pRole Select pRec).ToList()

        If (pQryRole.Count > 0) Then
            Dim pRoleID As Integer = pQryRole(0).fldID

            Dim pQryUserRole = (From pRec In pSealSuiteEntities.tblProcess_UserRole
                                Where pRec.fldRoleID = pRoleID Select pRec).ToList()

            If (pQryUserRole.Count > 0) Then
                Dim pListUserID As New List(Of Integer)
                For j As Integer = 0 To pQryUserRole.Count - 1
                    Dim pUserID As Integer = pQryUserRole(j).fldUserID
                    If (Not pListUserID.Contains(pUserID)) Then
                        pListUserID.Add(pUserID)
                    End If
                Next

                Dim pListUserName As New List(Of String)
                For k As Integer = 0 To pListUserID.Count - 1
                    Dim pID As Integer = pListUserID(k)
                    Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser
                                    Where pRec.fldID = pID Select pRec).ToList()

                    If (pQryUser.Count > 0) Then
                        Dim pUserName As String = pQryUser(0).fldFirstName & " " & pQryUser(0).fldLastName
                        If (Not pListUserName.Contains(pUserName)) Then
                            pListUserName.Add(pUserName)
                            cmbMgrPreOrder.Items.Add(pUserName)
                            'mUserID.Add(pID)
                        End If

                    End If
                Next

            End If

        End If

    End Sub

    Private Sub PopulateCmbSFinish()
        '===========================  
        '....This routine populates the Surface Finish combo box. (Database Driven).
        Dim pSealMCSEntity As New SealIPEMCSDBEntities()
        Dim pRecordLeak = (From it In pSealMCSEntity.tblESeal_Leak_T800
                           Select it.fldSFinish Distinct).ToList()

        cmbSFinish.Items.Clear()
        Dim pRecord As New tblESeal_Leak_T800

        If (pRecordLeak.Count > 0) Then
            For i As Integer = 0 To pRecordLeak.Count - 1
                cmbSFinish.Items.Add(pRecordLeak(i))
            Next
        End If

        cmbSFinish.SelectedIndex = 0
    End Sub


#End Region

#End Region

#End Region

#Region "MENU EVENT ROUTINES:"

    Private Sub mnuTabView_Click(sender As Object, e As EventArgs) Handles mnuTabView.Click
        '===================================================================================
        Dim pfrmProcessMain_Custom As New Process_frmCustomTab()
        pfrmProcessMain_Custom.ShowDialog()
    End Sub

    Private Sub mnuDropDownList_Click(sender As Object, e As EventArgs) Handles mnuDropDownList.Click
        '==============================================================================================
        If (gUser.Role = "Admin") Then
            With openFileDialog1

                .Filter = "Drop Down List (*.xlsx)|*.xlsx"
                .FilterIndex = 1
                .InitialDirectory = gProcessFile.DirProgramDataFile
                .FileName = ""
                .Title = "Open"

                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim pDDListFileName As String = .FileName
                    Cursor.Current = Cursors.WaitCursor
                    ReadExcel_UpdateDB(pDDListFileName)
                    PopulateDropDownList()
                    DisplayData()
                    Cursor.Current = Cursors.Default
                End If
            End With
        Else
            Dim pMsg As String = "Only 'Admin' user can load the 'Drop-Down DataFile'."
            MessageBox.Show(pMsg, "Permission Denied!", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If

    End Sub

    Private Sub mnuRiskQ_Click(sender As Object, e As EventArgs) Handles mnuRiskQ.Click
        '===============================================================================
        If (gUser.Role = "Admin") Then
            With openFileDialog1

                .Filter = "Risk Analysis DataFile (*.xlsx)|*.xlsx"
                .FilterIndex = 1
                .InitialDirectory = gProcessFile.DirProgramDataFile
                .FileName = ""
                .Title = "Open"

                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim pRiskAnaFileName As String = .FileName
                    Cursor.Current = Cursors.WaitCursor
                    mProcess_Project.RiskAna.LoadRiskQ(pRiskAnaFileName)
                    Cursor.Current = Cursors.Default

                End If
            End With
        Else
            Dim pMsg As String = "Only 'Admin' user can load the 'Risk Analysis DataFile'."
            MessageBox.Show(pMsg, "Permission Denied!", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If


    End Sub

#End Region

#Region "DROP-DOWN LIST RELATED ROUTINES:"

    Private Sub ReadExcel_UpdateDB(ByVal FileName_In As String)
        '=====================================================
        Dim pRow_Start As Integer = 0
        Dim pColumn_Start As Integer = 0
        Dim pItem As New List(Of String)

        CloseExcelFiles()

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()

        'pApp.DisplayAlerts = False

        '....Open WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing
        Dim pExitLoop As Boolean = False

        Dim pSealProcessDDListEntities As New SealProcess_DDListDBEntities()

        Try

            pWkbOrg = pApp.Workbooks.Open(FileName_In, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value,
                                              Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                              Missing.Value, Missing.Value, Missing.Value)
            '....Header
            '....Table - tblHeader_PopCoding
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Header", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblHeader_PopCoding")

            '....Table - tblHeader_Rating
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Header", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblHeader_Rating")

            '....Table - tblHeader_Type
            pRow_Start = 5
            pColumn_Start = 5
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Header", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblHeader_Type")

            '....Pre-Order
            '....Table - tblPreOrder_CustContactDept
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Pre-Order", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPreOrder_CustContactDept")

            '....Table - tblPreOrder_ExpComplianceStatus
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Pre-Order", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPreOrder_ExpComplianceStatus")

            '....Table - tblPreOrder_PartType
            pRow_Start = 5
            pColumn_Start = 5
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Pre-Order", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPreOrder_PartType")

            '....Table - tblPreOrder_MktSeg
            pRow_Start = 5
            pColumn_Start = 7
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Pre-Order", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPreOrder_MktSeg")

            '....Table - tblPreOrder_MktChannel
            pRow_Start = 5
            pColumn_Start = 9
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Pre-Order", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPreOrder_MktChannel")

            '....Table - tblPreOrder_Loc
            pRow_Start = 5
            pColumn_Start = 11
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Pre-Order", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPreOrder_Loc")

            '....Export
            '....Table - tblExp_Status
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Export", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblExp_Status")

            '....Table - tblExp_EARClassification
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Export", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblExp_EARClassification")

            '....Cost Estimating
            '....Table - tblCostEst_SplOpDesc
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Cost Estimating", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblCostEst_SplOpDesc")


            '....Application
            '....Table - tblApp_CavityDim
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Application", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblApp_CavityDim")

            '....Table - tblApp_SFinish_Measure
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Application", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblApp_SFinish_Measure")

            '....Table - tblApp_SFinish_Unit
            pRow_Start = 5
            pColumn_Start = 5
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Application", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblApp_SFinish_Unit")

            '....Design
            '....Table - tblDesign_VerificationDesc
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_VerificationDesc")

            '....Table - tblDesign_InputDesc
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_InputDesc")

            '....Table - tblDesign_CustSpecType
            pRow_Start = 5
            pColumn_Start = 5
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_CustSpecType")

            '....Table - tblDesign_CSeal
            pRow_Start = 32
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_CSeal")

            '....Table - tblDesign_ESeal
            pRow_Start = 32
            pColumn_Start = 2
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_ESeal")

            '....Table - tblDesign_USeal
            pRow_Start = 32
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_USeal")

            '....Table - tblDesign_OSeal
            pRow_Start = 32
            pColumn_Start = 4
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_OSeal")

            '....Table - tblDesign_WSeal
            pRow_Start = 32
            pColumn_Start = 5
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Design", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDesign_WSeal")

            '....Manufacturing
            '....Table - tblManf_ToolNGages
            '....Special Case.
            pRow_Start = 6
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Manufacturing", pRow_Start, pColumn_Start)
            'UpdateDB(pItem, "tblManf_ToolNGages")

            Dim pManf_ToolNGageType As New List(Of String)
            pManf_ToolNGageType = ReadDropDownList_Manf_ToolNGage_Type(pWkbOrg, "Manufacturing", pRow_Start, pColumn_Start)
            UpdateDB_Manf_ToolNGages(pItem, pManf_ToolNGageType, "tblManf_ToolNGages")
            'UpdateDB(pManf_ToolNGageType, "tblManf_ToolNGages")

            '....Table - tblManf_ToolNGages_Status
            pRow_Start = 5
            pColumn_Start = 4
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Manufacturing", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblManf_ToolNGages_Status")

            '....Table - tblManf_ToolNGages_DesignResponsibility
            pRow_Start = 5
            pColumn_Start = 6
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Manufacturing", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblManf_ToolNGages_DesignResponsibility")

            '....Purchasing
            '....Table - tblPurchase_Unit
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Purchasing", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblPurchase_Unit")

            '....Quality
            '....Table - tblQlty_VisualInspection
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Quality", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblQlty_VisualInspection")

            '....Table - tblQlty_CustAcceptStd
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Quality", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblQlty_CustAcceptStd")

            '....Dwg
            '....Table - tblDwg_DesignLevel
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Drawing", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblDwg_DesignLevel")

            '....Testing
            '....Table - tblTest_Medium
            pRow_Start = 5
            pColumn_Start = 1
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Testing", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblTest_Medium")

            '....Table - tblTest_Freq
            pRow_Start = 5
            pColumn_Start = 3
            pItem = New List(Of String)
            pItem = ReadDropDownList(pWkbOrg, "Testing", pRow_Start, pColumn_Start)
            UpdateDB(pItem, "tblTest_Freq")

            pWkbOrg.Close()
            pApp = Nothing
            Dim pFileTitle As String = System.IO.Path.GetFileName(FileName_In)
            Dim pMsg As String = "Updated from: " & Space(2) & pFileTitle
            MessageBox.Show(pMsg, "Drop-Down Data File Loading", MessageBoxButtons.OK)

        Catch ex As Exception

        End Try

    End Sub

    Private Function ReadDropDownList(ByVal WorkBook_In As EXCEL.Workbook, ByVal SheetName_In As String,
                                      ByVal Row_In As Integer, ByVal Column_In As Integer) As List(Of String)
        '===============================================================================================================

        Dim pWkSheet As EXCEL.Worksheet
        Dim pExitLoop As Boolean = False

        pWkSheet = WorkBook_In.Worksheets(SheetName_In)

        Dim pItem As New List(Of String)
        Dim pIndx As Integer = 0

        While (Not pExitLoop)
            Dim pVal As String = pWkSheet.Cells(Row_In + pIndx, Column_In).value
            pExitLoop = String.IsNullOrEmpty(pVal)

            If (Not pExitLoop) Then
                pItem.Add(pVal)
                pIndx = pIndx + 1

            End If

        End While

        Return pItem

    End Function

    Private Function ReadDropDownList_Manf_ToolNGage_Type(ByVal WorkBook_In As EXCEL.Workbook, ByVal SheetName_In As String,
                                                           ByVal Row_In As Integer, ByVal Column_In As Integer) As List(Of String)
        '===============================================================================================================

        Dim pWkSheet As EXCEL.Worksheet
        Dim pExitLoop As Boolean = False

        pWkSheet = WorkBook_In.Worksheets(SheetName_In)

        Dim pItem As New List(Of String)
        Dim pIndx As Integer = 0

        Dim pType As New List(Of String)
        Dim pType_Column As Integer = Column_In + 1

        While (Not pExitLoop)

            Dim pVal As String = pWkSheet.Cells(Row_In + pIndx, Column_In).value
            Dim pVal1 As String = pWkSheet.Cells(Row_In + pIndx, pType_Column).value

            If (IsNothing(pVal1)) Then
                pVal1 = ""
            End If
            pExitLoop = String.IsNullOrEmpty(pVal)

            If (Not pExitLoop) Then
                pItem.Add(pVal)
                pType.Add(pVal1)
                pIndx = pIndx + 1
            End If

        End While

        Return pType

    End Function

    Private Sub UpdateDB_Manf_ToolNGages(ByVal Item_In As List(Of String), ByVal Type_In As List(Of String), ByVal TableName_In As String)
        '===============================================================================================================================
        Dim pSealProcessDDListEntities As New SealProcess_DDListDBEntities()
        Dim pRec = (From Rec In pSealProcessDDListEntities.tblManf_ToolNGages
                    Select Rec).ToList()

        For i As Integer = 0 To pRec.Count() - 1
            pSealProcessDDListEntities.DeleteObject(pRec(i))
            pSealProcessDDListEntities.SaveChanges()
        Next

        Dim pTableRec As New List(Of tblManf_ToolNGages)

        For i As Integer = 0 To Item_In.Count - 1
            Dim pList As New tblManf_ToolNGages
            pTableRec.Add(pList)
            pTableRec(i).fldID = i + 1
            pTableRec(i).fldDesc = Item_In(i)
            pTableRec(i).fldType = Type_In(i)

            pSealProcessDDListEntities.AddTotblManf_ToolNGages(pTableRec(i))
        Next

        pSealProcessDDListEntities.SaveChanges()
    End Sub

    Private Sub UpdateDB(ByVal Item_In As List(Of String), ByVal TableName_In As String)
        '===============================================================================
        Dim pSealProcessDDListEntities As New SealProcess_DDListDBEntities()

        If (Item_In.Count > 0) Then

            '....Header
            If (TableName_In = "tblHeader_PopCoding") Then
                Dim pPopCodingRec = (From Rec In pSealProcessDDListEntities.tblHeader_PopCoding
                                     Select Rec).ToList()

                For i As Integer = 0 To pPopCodingRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pPopCodingRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pPopCodingList As New List(Of tblHeader_PopCoding)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pPopCodeNameList As New tblHeader_PopCoding
                    pPopCodingList.Add(pPopCodeNameList)
                    pPopCodingList(i).fldID = i + 1
                    pPopCodingList(i).fldPopCoding = Item_In(i)

                    pSealProcessDDListEntities.AddTotblHeader_PopCoding(pPopCodingList(i))
                Next

            ElseIf (TableName_In = "tblHeader_Rating") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblHeader_Rating
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableList As New List(Of tblHeader_Rating)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblHeader_Rating
                    pTableList.Add(pList)
                    pTableList(i).fldID = i + 1
                    pTableList(i).fldRating = Item_In(i)

                    pSealProcessDDListEntities.AddTotblHeader_Rating(pTableList(i))
                Next

            ElseIf (TableName_In = "tblHeader_Type") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblHeader_Type
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblHeader_Type)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblHeader_Type
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldType = Item_In(i)

                    pSealProcessDDListEntities.AddTotblHeader_Type(pTableRec(i))
                Next

                '....Pre-Order
            ElseIf (TableName_In = "tblPreOrder_CustContactDept") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_CustContactDept
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPreOrder_CustContactDept)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPreOrder_CustContactDept
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldCustContactDept = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPreOrder_CustContactDept(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblPreOrder_ExpComplianceStatus") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_ExpComplianceStatus
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPreOrder_ExpComplianceStatus)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPreOrder_ExpComplianceStatus
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldExpComplianceStatus = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPreOrder_ExpComplianceStatus(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblPreOrder_PartType") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_PartType
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPreOrder_PartType)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPreOrder_PartType
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldPartType = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPreOrder_PartType(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblPreOrder_MktSeg") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_MktSeg
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPreOrder_MktSeg)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPreOrder_MktSeg
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldMktSeg = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPreOrder_MktSeg(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblPreOrder_MktChannel") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_MktChannel
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPreOrder_MktChannel)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPreOrder_MktChannel
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldMktChannel = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPreOrder_MktChannel(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblPreOrder_Loc") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_Loc
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPreOrder_Loc)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPreOrder_Loc
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldLoc = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPreOrder_Loc(pTableRec(i))
                Next

                '....Cost Estimating
            ElseIf (TableName_In = "tblCostEst_SplOpDesc") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblCostEst_SplOpDesc
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblCostEst_SplOpDesc)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblCostEst_SplOpDesc
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldSplOpDesc = Item_In(i)

                    pSealProcessDDListEntities.AddTotblCostEst_SplOpDesc(pTableRec(i))
                Next

                '....Export
            ElseIf (TableName_In = "tblExp_Status") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblExp_Status
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblExp_Status)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblExp_Status
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldStatus = Item_In(i)

                    pSealProcessDDListEntities.AddTotblExp_Status(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblExp_EARClassification") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblExp_EARClassification
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblExp_EARClassification)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblExp_EARClassification
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldEARClassification = Item_In(i)

                    pSealProcessDDListEntities.AddTotblExp_EARClassification(pTableRec(i))
                Next

                '....Application
            ElseIf (TableName_In = "tblApp_CavityDim") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblApp_CavityDim
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblApp_CavityDim)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblApp_CavityDim
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldCavityDim = Item_In(i)

                    pSealProcessDDListEntities.AddTotblApp_CavityDim(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblApp_SFinish_Measure") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblApp_SFinish_Measure
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblApp_SFinish_Measure)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblApp_SFinish_Measure
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldSFinish_Measure = Item_In(i)

                    pSealProcessDDListEntities.AddTotblApp_SFinish_Measure(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblApp_SFinish_Unit") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblApp_SFinish_Unit
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblApp_SFinish_Unit)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblApp_SFinish_Unit
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldSFinish_Unit = Item_In(i)

                    pSealProcessDDListEntities.AddTotblApp_SFinish_Unit(pTableRec(i))
                Next

                '....Design
            ElseIf (TableName_In = "tblDesign_VerificationDesc") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_VerificationDesc
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_VerificationDesc)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_VerificationDesc
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldDesignVerification = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_VerificationDesc(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_VerificationDesc") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_VerificationDesc
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_VerificationDesc)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_VerificationDesc
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldDesignVerification = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_VerificationDesc(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_InputDesc") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_InputDesc
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_InputDesc)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_InputDesc
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldInputDesc = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_InputDesc(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_CustSpecType") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_CustSpecType
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_CustSpecType)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_CustSpecType
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldCustSpecType = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_CustSpecType(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_CSeal") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_CSeal
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_CSeal)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_CSeal
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldCSeal = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_CSeal(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_ESeal") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_ESeal
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_ESeal)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_ESeal
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldESeal = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_ESeal(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_USeal") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_USeal
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_USeal)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_USeal
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldUSeal = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_USeal(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_OSeal") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_OSeal
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_OSeal)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_OSeal
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldOSeal = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_OSeal(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblDesign_WSeal") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDesign_WSeal
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDesign_WSeal)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDesign_WSeal
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldWSeal = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDesign_WSeal(pTableRec(i))
                Next

                '....Manufacturing
                ''ElseIf (TableName_In = "tblManf_ToolNGages") Then
                ''    Dim pRec = (From Rec In pSealProcessDDListEntities.tblManf_ToolNGages
                ''                Select Rec).ToList()

                ''    For i As Integer = 0 To pRec.Count() - 1
                ''        pSealProcessDDListEntities.DeleteObject(pRec(i))
                ''        pSealProcessDDListEntities.SaveChanges()
                ''    Next

                ''    Dim pTableRec As New List(Of tblManf_ToolNGages)

                ''    For i As Integer = 0 To Item_In.Count - 1
                ''        Dim pList As New tblManf_ToolNGages
                ''        pTableRec.Add(pList)
                ''        pTableRec(i).fldID = i + 1
                ''        pTableRec(i).fldDesc = Item_In(i)

                ''        pSealProcessDDListEntities.AddTotblManf_ToolNGages(pTableRec(i))
                ''    Next

            ElseIf (TableName_In = "tblManf_ToolNGages_Status") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblManf_ToolNGages_Status
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblManf_ToolNGages_Status)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblManf_ToolNGages_Status
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldStatus = Item_In(i)

                    pSealProcessDDListEntities.AddTotblManf_ToolNGages_Status(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblManf_ToolNGages_DesignResponsibility") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblManf_ToolNGages_DesignResponsibility
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblManf_ToolNGages_DesignResponsibility)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblManf_ToolNGages_DesignResponsibility
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldDesignResponsibility = Item_In(i)

                    pSealProcessDDListEntities.AddTotblManf_ToolNGages_DesignResponsibility(pTableRec(i))
                Next

                '....Purchasing
            ElseIf (TableName_In = "tblPurchase_Unit") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblPurchase_Unit
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblPurchase_Unit)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblPurchase_Unit
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldUnit = Item_In(i)

                    pSealProcessDDListEntities.AddTotblPurchase_Unit(pTableRec(i))
                Next

                '....Quality
            ElseIf (TableName_In = "tblQlty_VisualInspection") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblQlty_VisualInspection
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblQlty_VisualInspection)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblQlty_VisualInspection
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldVisualInspectionType = Item_In(i)

                    pSealProcessDDListEntities.AddTotblQlty_VisualInspection(pTableRec(i))
                Next

            ElseIf (TableName_In = "tblQlty_CustAcceptStd") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblQlty_CustAcceptStd
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblQlty_CustAcceptStd)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblQlty_CustAcceptStd
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldCustAcceptStd = Item_In(i)

                    pSealProcessDDListEntities.AddTotblQlty_CustAcceptStd(pTableRec(i))
                Next

                '....Drawing
            ElseIf (TableName_In = "tblDwg_DesignLevel") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblDwg_DesignLevel
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblDwg_DesignLevel)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblDwg_DesignLevel
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldDesignLevel = Item_In(i)

                    pSealProcessDDListEntities.AddTotblDwg_DesignLevel(pTableRec(i))
                Next

                '....Testing
            ElseIf (TableName_In = "tblTest_Medium") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblTest_Medium
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblTest_Medium)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblTest_Medium
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldMedium = Item_In(i)

                    pSealProcessDDListEntities.AddTotblTest_Medium(pTableRec(i))
                Next


            ElseIf (TableName_In = "tblTest_Freq") Then
                Dim pRec = (From Rec In pSealProcessDDListEntities.tblTest_Freq
                            Select Rec).ToList()

                For i As Integer = 0 To pRec.Count() - 1
                    pSealProcessDDListEntities.DeleteObject(pRec(i))
                    pSealProcessDDListEntities.SaveChanges()
                Next

                Dim pTableRec As New List(Of tblTest_Freq)

                For i As Integer = 0 To Item_In.Count - 1
                    Dim pList As New tblTest_Freq
                    pTableRec.Add(pList)
                    pTableRec(i).fldID = i + 1
                    pTableRec(i).fldFreq = Item_In(i)

                    pSealProcessDDListEntities.AddTotblTest_Freq(pTableRec(i))
                Next

            End If

            pSealProcessDDListEntities.SaveChanges()

        End If

    End Sub

    Private Sub PopulateDropDownList(ByVal ComboBox_In As ComboBox, ByVal TableName_In As String)
        '=======================================================================================
        Try

            Dim pSealProcessDDListEntities As New SealProcess_DDListDBEntities()

            Dim pRec As Object = Nothing
            If (TableName_In = "tblHeader_PopCoding") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblHeader_PopCoding
                        Select Rec.fldPopCoding).ToList()
            ElseIf (TableName_In = "tblHeader_Rating") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblHeader_Rating
                        Select Rec.fldRating).ToList()
            ElseIf (TableName_In = "tblHeader_Type") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblHeader_Type
                        Select Rec.fldType).ToList()
            ElseIf (TableName_In = "tblPreOrder_ExpComplianceStatus") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_ExpComplianceStatus
                        Select Rec.fldExpComplianceStatus).ToList()
            ElseIf (TableName_In = "tblPreOrder_PartType") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_PartType
                        Select Rec.fldPartType).ToList()
            ElseIf (TableName_In = "tblPreOrder_MktSeg") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_MktSeg
                        Select Rec.fldMktSeg).ToList()
            ElseIf (TableName_In = "tblPreOrder_MktChannel") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_MktChannel
                        Select Rec.fldMktChannel).ToList()
            ElseIf (TableName_In = "tblPreOrder_Loc") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_Loc
                        Select Rec.fldLoc).ToList()
            ElseIf (TableName_In = "tblExp_Status") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblExp_Status
                        Select Rec.fldStatus).ToList()
            ElseIf (TableName_In = "tblExp_EARClassification") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblExp_EARClassification
                        Select Rec.fldEARClassification).ToList()
            ElseIf (TableName_In = "tblApp_SFinish_Measure") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblApp_SFinish_Measure
                        Select Rec.fldSFinish_Measure).ToList()
            ElseIf (TableName_In = "tblApp_SFinish_Unit") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblApp_SFinish_Unit
                        Select Rec.fldSFinish_Unit).ToList()
            ElseIf (TableName_In = "tblQlty_VisualInspection") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblQlty_VisualInspection
                        Select Rec.fldVisualInspectionType).ToList()
            ElseIf (TableName_In = "tblQlty_CustAcceptStd") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblQlty_CustAcceptStd
                        Select Rec.fldCustAcceptStd).ToList()
            ElseIf (TableName_In = "tblDwg_DesignLevel") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblDwg_DesignLevel
                        Select Rec.fldDesignLevel).ToList()
            ElseIf (TableName_In = "tblTest_Medium") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblTest_Medium
                        Select Rec.fldMedium).ToList()
            ElseIf (TableName_In = "tblTest_Freq") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblTest_Freq
                        Select Rec.fldFreq).ToList()

            End If

            ComboBox_In.Items.Clear()

            For i As Integer = 0 To pRec.Count() - 1
                ComboBox_In.Items.Add(pRec(i))
            Next

        Catch ex As Exception

        End Try

    End Sub

    Private Sub PopulateDropDownList(ByVal ComboBox_In As DataGridViewComboBoxColumn, ByVal TableName_In As String)
        '=========================================================================================================
        Try

            Dim pSealProcessDDListEntities As New SealProcess_DDListDBEntities()

            Dim pRec As Object = Nothing
            If (TableName_In = "tblPreOrder_CustContactDept") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPreOrder_CustContactDept
                        Select Rec.fldCustContactDept).ToList()
            ElseIf (TableName_In = "tblCostEst_SplOpDesc") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblCostEst_SplOpDesc
                        Select Rec.fldSplOpDesc).ToList()
            ElseIf (TableName_In = "tblApp_CavityDim") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblApp_CavityDim
                        Select Rec.fldCavityDim).ToList()
            ElseIf (TableName_In = "tblDesign_VerificationDesc") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblDesign_VerificationDesc
                        Select Rec.fldDesignVerification).ToList()
            ElseIf (TableName_In = "tblDesign_InputDesc") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblDesign_InputDesc
                        Select Rec.fldInputDesc).ToList()
            ElseIf (TableName_In = "tblDesign_CustSpecType") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblDesign_CustSpecType
                        Select Rec.fldCustSpecType).ToList()
            ElseIf (TableName_In = "tblManf_ToolNGages_Status") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblManf_ToolNGages_Status
                        Select Rec.fldStatus).ToList()
            ElseIf (TableName_In = "tblManf_ToolNGages_DesignResponsibility") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblManf_ToolNGages_DesignResponsibility
                        Select Rec.fldDesignResponsibility).ToList()
            ElseIf (TableName_In = "tblPurchase_Unit") Then
                pRec = (From Rec In pSealProcessDDListEntities.tblPurchase_Unit
                        Select Rec.fldUnit).ToList()

            End If

            ComboBox_In.Items.Clear()

            For i As Integer = 0 To pRec.Count() - 1
                ComboBox_In.Items.Add(pRec(i))
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub CloseExcelFiles()
        '=======================

        Dim pProcesses As Process() = Process.GetProcesses()

        Try
            For Each p As Process In pProcesses
                If p.ProcessName = "EXCEL" Then
                    p.Kill()
                End If
            Next

        Catch pEXP As Exception
        End Try
    End Sub

#End Region

#Region "TAB CONTROL RELATED ROUTINES:"

    Private Sub TabControl1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles TabControl1.SelectedIndexChanged
        '============================================================================================
        Dim pCI As New CultureInfo("en-US")

        cmdRiskAna.Enabled = False
        cmdIssueComment.Enabled = False
        'cmdSealPart.Enabled = False

        'If (mPreOrder And TabControl1.SelectedIndex = 0) Then
        If (TabControl1.SelectedIndex = 0) Then
                cmdRiskAna.Enabled = True
                cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = True
        End If

        'If (mExport And TabControl1.SelectedIndex = 1) Then
        If (TabControl1.SelectedIndex = 1) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mOrdEntry And TabControl1.SelectedIndex = 2) Then
        If (TabControl1.SelectedIndex = 2) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mCost And TabControl1.SelectedIndex = 3) Then
        If (TabControl1.SelectedIndex = 3) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mApp And TabControl1.SelectedIndex = 4) Then
        If (TabControl1.SelectedIndex = 4) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mDesign And TabControl1.SelectedIndex = 5) Then
        If (TabControl1.SelectedIndex = 5) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = True

        End If

        'If (mManf And TabControl1.SelectedIndex = 6) Then
        If (TabControl1.SelectedIndex = 6) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mPurchase And TabControl1.SelectedIndex = 7) Then
        If (TabControl1.SelectedIndex = 7) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mQlty And TabControl1.SelectedIndex = 8) Then
        If (TabControl1.SelectedIndex = 8) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mDwg And TabControl1.SelectedIndex = 9) Then
        If (TabControl1.SelectedIndex = 9) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mTest And TabControl1.SelectedIndex = 10) Then
        If (TabControl1.SelectedIndex = 10) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        'If (mPlanning And TabControl1.SelectedIndex = 11) Then
        If (TabControl1.SelectedIndex = 11) Then
            cmdRiskAna.Enabled = False
            cmdIssueComment.Enabled = False
            'cmdSealPart.Enabled = False

        End If

        'If (mShipping And TabControl1.SelectedIndex = 12) Then
        If (TabControl1.SelectedIndex = 12) Then
            cmdRiskAna.Enabled = True
            cmdIssueComment.Enabled = True
            'cmdSealPart.Enabled = False

        End If

        If (TabControl1.SelectedIndex = 13) Then
            cmdRiskAna.Enabled = False
            cmdIssueComment.Enabled = False
            'cmdSealPart.Enabled = False

        End If

        If (TabControl1.SelectedIndex = 14) Then
            cmdRiskAna.Enabled = False
            cmdIssueComment.Enabled = False
            'cmdSealPart.Enabled = False

        End If

        If (TabControl1.SelectedIndex = 15) Then
            cmdRiskAna.Enabled = False
            cmdIssueComment.Enabled = False
            'cmdSealPart.Enabled = False

        End If


        If (CompareVal_Header()) Then
            txtDateMod.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            txtModifiedBy.Text = gUser.FirstName + " " + gUser.LastName
        End If

        If (TabControl1.SelectedIndex = 0) Then
            CopyDataGridView(grdOrdEntry_CustContact, grdCustContact)

        ElseIf (TabControl1.SelectedIndex = 2) Then
            CopyDataGridView(grdCustContact, grdOrdEntry_CustContact)

        ElseIf (TabControl1.SelectedIndex = 3) Then
            CopyDataGridView(grdQuality_SplOperation, grdCost_SplOperation)

        ElseIf (TabControl1.SelectedIndex = 8) Then
            CopyDataGridView(grdCost_SplOperation, grdQuality_SplOperation)
        End If

        SaveData()

        If (TabControl1.SelectedIndex = 11) Then
            ''lstPlanning_Notes_Dim.Items.Clear()
            ''For i As Integer = 0 To mProcess_Project.Cost.SplOperation.ID_Cost.Count - 1
            ''    Dim pName As String = mProcess_Project.Cost.SplOperation.Desc(i)
            ''    Dim pFlag As Boolean = False
            ''    For j As Integer = 0 To mProcess_Project.Planning.SplOperation.ID_SplOperation.Count - 1
            ''        If (pName = mProcess_Project.Planning.SplOperation.Desc(j)) Then
            ''            pFlag = True
            ''            Exit For
            ''        End If
            ''    Next
            ''    If (Not pFlag) Then
            ''        lstPlanning_Notes_Dim.Items.Add(pName)
            ''    End If
            ''Next

        ElseIf (TabControl1.SelectedIndex = 7) Then
            '....Purchasing
            grdPurchase_ToolNGages.Rows.Clear()
            For i As Integer = 0 To mProcess_Project.Manf.ToolNGage.ID_Tool.Count - 1
                If (mProcess_Project.Manf.ToolNGage.Status(i) = "Buy") Then
                    grdPurchase_ToolNGages.Rows.Add()
                    grdPurchase_ToolNGages.Rows(i).Cells(0).Value = mProcess_Project.Manf.ToolNGage.PartNo(i)
                    grdPurchase_ToolNGages.Rows(i).Cells(1).Value = mProcess_Project.Manf.ToolNGage.Desc(i)
                    grdPurchase_ToolNGages.Rows(i).Cells(2).Value = mProcess_Project.Manf.ToolNGage.Type(i)
                    'grdPurchase_ToolNGages.Rows(i).Cells(3).Value = mProcess_Project.Manf.ToolNGage.Status(i)

                    If (Math.Abs(mProcess_Project.Manf.ToolNGage.LeadTime(i)) > gcEPS) Then
                        grdPurchase_ToolNGages.Rows(i).Cells(3).Value = mProcess_Project.Manf.ToolNGage.LeadTime(i)
                    Else
                        grdPurchase_ToolNGages.Rows(i).Cells(3).Value = ""
                    End If

                    grdPurchase_ToolNGages.Rows(i).Cells(4).Value = mProcess_Project.Manf.ToolNGage.DesignResponsibility(i)
                End If
            Next

        ElseIf (TabControl1.SelectedIndex = 12) Then

            '....Shipping
            grdShipping_CustSpec.Rows.Clear()
            Dim pRowCount As Integer = 0
            For i As Integer = 0 To mProcess_Project.Design.CustSpec.ID_Cust.Count - 1
                If (mProcess_Project.Design.CustSpec.Type(i) = "Packaging") Then
                    grdShipping_CustSpec.Rows.Add()
                    grdShipping_CustSpec.Rows(pRowCount).Cells(0).Value = mProcess_Project.Design.CustSpec.Desc(i)
                    grdShipping_CustSpec.Rows(pRowCount).Cells(1).Value = mProcess_Project.Design.CustSpec.Interpret(i)
                    pRowCount = pRowCount + 1
                End If
            Next
            grdShipping_CustSpec.AllowUserToAddRows = False
            'grpCustSpec_Shipping.Enabled = False
        End If

        'AES 25APR18
        Me.VerticalScroll.Value = 0
        Me.AutoScrollPosition = New Point(txtMenu.Left, txtMenu.Top)

        If (TabControl1.SelectedIndex = 15) Then
            grdApproval_Attendees.Focus()
            Me.VerticalScroll.Value = 0
            Me.AutoScrollPosition = New Point(txtMenu.Left, txtMenu.Top)
        End If

    End Sub

    Private Sub TabControl1_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) Handles TabControl1.DrawItem
        '==============================================================================================================================
        Dim pTabControl As TabControl = DirectCast(sender, TabControl)
        Dim pText As String = pTabControl.TabPages(e.Index).Text
        Dim pGrph As Graphics = e.Graphics
        Dim pFont As Font = pTabControl.Font
        Dim pFormat = New System.Drawing.StringFormat
        'CHANGES HERE...
        pFormat.Alignment = StringAlignment.Center
        pFormat.LineAlignment = StringAlignment.Center
        Dim pPencil As New SolidBrush(Color.Black)
        ''RENAMED VARIABLE HERE...
        Dim pRect As RectangleF = RectangleF.op_Implicit(pTabControl.GetTabRect(e.Index))
        pGrph.FillRectangle(Brushes.LightGray, pRect)

        'ControlPaint.DrawBorder(e.Graphics, TabControl1.TabPages(e.Index).ClientRectangle, Color.Black, ButtonBorderStyle.Solid)
        If (e.Index = TabControl1.SelectedIndex) Then
            pPencil = New SolidBrush(Color.White)
            pGrph.FillRectangle(Brushes.Gray, pRect)
        End If

        If mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.LightSteelBlue, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.SteelBlue, pRect)
            End If

            'ControlPaint.DrawBorder(e.Graphics, pTabControl.TabPages(e.Index).ClientRectangle, Color.Black, ButtonBorderStyle.Solid)

            'pPencil = New SolidBrush(Color.White)
            ''CHANGED BACKGROUN COLOR HERE...
            'pGrph.FillRectangle(Brushes.Green, pRect)
        End If

        If e.Index = 0 And chkPreOrderUserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 0 And chkPreOrderUserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 1 And chkITAR_Export_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 1 And chkITAR_Export_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 2 And chkOrdEntry_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 2 And chkOrdEntry_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 3 And chkCost_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 3 And chkCost_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 4 And chkApp_UserSigned_Face.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 4 And chkApp_UserSigned_Face.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 5 And chkDesign_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 5 And chkDesign_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 6 And chkManf_UserSigned.Checked And mTabIndex.Contains(e.Index) Then

            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 6 And chkManf_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If


        ElseIf e.Index = 7 And chkPurchase_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 7 And chkPurchase_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 8 And chkQuality_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 8 And chkQuality_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 9 And chkDwg_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 9 And chkDwg_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 10 And chkTest_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 10 And chkTest_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 11 And chkPlanning_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 11 And chkPlanning_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

        ElseIf e.Index = 12 And chkShipping_UserSigned.Checked And mTabIndex.Contains(e.Index) Then
            pFont = New Font(pFont, FontStyle.Bold)
            pGrph.FillRectangle(Brushes.Green, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.White)
                pGrph.FillRectangle(Brushes.Green, pRect)
            End If

        ElseIf e.Index = 12 And chkShipping_UserSigned.Checked Then
            pPencil = New SolidBrush(Color.Black)
            pFont = New Font(pFont, FontStyle.Regular)
            pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)

            If (e.Index = TabControl1.SelectedIndex) Then
                pPencil = New SolidBrush(Color.Black)
                pFont = New Font(pFont, FontStyle.Underline)
                pGrph.FillRectangle(Brushes.DarkSeaGreen, pRect)
            End If

            'ElseIf e.Index = 15 And IsApprovalTab_Enabled() Then

            '    pFont = New Font(pFont, FontStyle.Bold)
            '    pGrph.FillRectangle(Brushes.Green, pRect)

            '    If (e.Index = TabControl1.SelectedIndex) Then
            '        pPencil = New SolidBrush(Color.White)
            '        pGrph.FillRectangle(Brushes.Green, pRect)
            '    End If
        End If


        If (gUser.Role <> "Viewer") Then
            If e.Index = 14 And mIssue = True Then
                pFont = New Font(pFont, FontStyle.Bold)
                pGrph.FillRectangle(Brushes.LightSteelBlue, pRect)

                If (e.Index = TabControl1.SelectedIndex) Then
                    pPencil = New SolidBrush(Color.White)
                    pGrph.FillRectangle(Brushes.SteelBlue, pRect)
                End If

                'ElseIf e.Index = 15 And IsApprovalTab_Enabled() Then        'AES 16APR18
                '    pFont = New Font(pFont, FontStyle.Bold)
                '    pGrph.FillRectangle(Brushes.LightSteelBlue, pRect)

                '    If (e.Index = TabControl1.SelectedIndex) Then
                '        pPencil = New SolidBrush(Color.White)
                '        pGrph.FillRectangle(Brushes.SteelBlue, pRect)
                '    End If
            End If
        End If

        'pPencil = New SolidBrush(Color.White)
        'CHANGED BACKGROUND COLOR HERE...
        'pGrph.FillRectangle(Brushes.LightSteelBlue, pRect)
        pGrph.DrawString(pText, pFont, pPencil, pRect, pFormat)

    End Sub

    Private Sub tbApp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbApp.SelectedIndexChanged
        '=======================================================================================================
        optApp_Cust_Gen.Checked = True
        txtMenu.Focus()
        txtMenu.Select()

    End Sub

#Region "HELPER ROUTINE:"

    Private Sub CopyDataGridView(ByVal Dgv_Source As DataGridView, ByVal Dgv_Target As DataGridView)
        '===========================================================================================
        Dgv_Target.Rows.Clear()
        Dim pTotIndex As Integer = Dgv_Target.Rows.Count
        If Dgv_Target.Columns.Count = 0 Then
            For Each dgvc As DataGridViewColumn In Dgv_Source.Columns
                Dgv_Target.Columns.Add(TryCast(dgvc.Clone(), DataGridViewColumn))
            Next
        End If

        Dim row As New DataGridViewRow()

        For i As Integer = 0 To Dgv_Source.Rows.Count - 2
            row = DirectCast(Dgv_Source.Rows(i).Clone(), DataGridViewRow)
            Dim intColIndex As Integer = 0
            For Each cell As DataGridViewCell In Dgv_Source.Rows(i).Cells
                row.Cells(intColIndex).Value = cell.Value
                intColIndex += 1
            Next
            Dgv_Target.Rows.Add(row)
        Next

    End Sub

#End Region

#End Region

#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtDesign_MCS_MouseHover(sender As Object, e As EventArgs) Handles txtDesign_MCS.MouseHover
        '==================================================================================================
        ToolTip1.SetToolTip(txtDesign_MCS, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub txtDesign_MCS_MouseLeave(sender As Object, e As EventArgs) Handles txtDesign_MCS.MouseLeave
        '==================================================================================================
        'txtDesign_MCS.Focus()        'AES 24APR18
    End Sub

    Private Sub txtOrderEntry_QtdLeadTime_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                    Handles txtOrderEntry_QtdLeadTime.Validating
        '===========================================================================================================
        txtOrderEntry_QtdLeadTime.Text = CleanInputNummeric(txtOrderEntry_QtdLeadTime.Text)
    End Sub

    Private Sub txtOrdEntry_OrderQty_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles txtOrdEntry_OrderQty.Validating
        '======================================================================================================
        txtOrdEntry_OrderQty.Text = CleanInputNumber(txtOrdEntry_OrderQty.Text)
    End Sub

    Private Sub txtApp_MaxLeak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                          Handles txtApp_MaxLeak.Validating
        '================================================================================================
        txtApp_MaxLeak.Text = CleanInputNummeric(txtApp_MaxLeak.Text)

    End Sub

    Private Sub txtApp_PressCycleFreq_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles txtApp_PressCycleFreq.Validating
        '=======================================================================================================
        txtApp_PressCycleFreq.Text = CleanInputNummeric(txtApp_PressCycleFreq.Text)
    End Sub

    Private Sub txtApp_PressCycleAmp_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles txtApp_PressCycleAmp.Validating
        '======================================================================================================
        txtApp_PressCycleAmp.Text = CleanInputNummeric(txtApp_PressCycleAmp.Text)
    End Sub

    Private Sub txtApp_Hardness1_Face_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                 Handles txtApp_Hardness1_Face.Validating
        '========================================================================================================
        txtApp_Hardness1_Face.Text = CleanInputNummeric(txtApp_Hardness1_Face.Text)

    End Sub

    Private Sub txtApp_Hardness2_Face_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                 Handles txtApp_Hardness2_Face.Validating
        '========================================================================================================
        txtApp_Hardness2_Face.Text = CleanInputNummeric(txtApp_Hardness2_Face.Text)
    End Sub

    Private Sub txtApp_SF1_Face_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                           Handles txtApp_SF1_Face.Validating
        '==================================================================================================
        txtApp_SF1_Face.Text = CleanInputNummeric(txtApp_SF1_Face.Text)

    End Sub

    Private Sub txtApp_SF2_Face_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                           Handles txtApp_SF2_Face.Validating
        '=================================================================================================
        txtApp_SF2_Face.Text = CleanInputNummeric(txtApp_SF2_Face.Text)

    End Sub

    Private Sub txtApp_Face_MaxFlangeSeparation_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                           Handles txtApp_Face_MaxFlangeSeparation.Validating
        '===================================================================================================================
        txtApp_Face_MaxFlangeSeparation.Text = CleanInputNummeric(txtApp_Face_MaxFlangeSeparation.Text)

    End Sub

    Private Sub txtApp_Hardness1_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                  Handles txtApp_Hardness1_Axial.Validating
        '=========================================================================================================
        txtApp_Hardness1_Axial.Text = CleanInputNummeric(txtApp_Hardness1_Axial.Text)
    End Sub

    Private Sub txtApp_Hardness2_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                  Handles txtApp_Hardness2_Axial.Validating
        '========================================================================================================
        txtApp_Hardness2_Axial.Text = CleanInputNummeric(txtApp_Hardness2_Axial.Text)
    End Sub

    Private Sub txtApp_SF1_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                            Handles txtApp_SF1_Axial.Validating
        '====================================================================================================
        txtApp_SF1_Axial.Text = CleanInputNummeric(txtApp_SF1_Axial.Text)
    End Sub

    Private Sub txtApp_SF2_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                            Handles txtApp_SF2_Axial.Validating
        '===================================================================================================
        txtApp_SF2_Axial.Text = CleanInputNummeric(txtApp_SF2_Axial.Text)
    End Sub

    Private Sub txtApp_RotateRPM_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                  Handles txtApp_RotateRPM_Axial.Validating
        '===========================================================================================================
        txtApp_RotateRPM_Axial.Text = CleanInputNummeric(txtApp_RotateRPM_Axial.Text)
    End Sub

    Private Sub txtApp_RecipStrokeL_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                     Handles txtApp_RecipStrokeL_Axial.Validating
        '============================================================================================================
        txtApp_RecipStrokeL_Axial.Text = CleanInputNummeric(txtApp_RecipStrokeL_Axial.Text)
    End Sub

    Private Sub txtApp_RecipV_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                               Handles txtApp_RecipV_Axial.Validating
        '=======================================================================================================
        txtApp_RecipV_Axial.Text = CleanInputNummeric(txtApp_RecipV_Axial.Text)
    End Sub

    Private Sub txtApp_RecipCycleRate_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                       Handles txtApp_RecipCycleRate_Axial.Validating
        '==============================================================================================================
        txtApp_RecipCycleRate_Axial.Text = CleanInputNummeric(txtApp_RecipCycleRate_Axial.Text)
    End Sub

    Private Sub txtApp_RecipServiceLife_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                         Handles txtApp_RecipServiceLife_Axial.Validating
        '================================================================================================================
        txtApp_RecipServiceLife_Axial.Text = CleanInputNummeric(txtApp_RecipServiceLife_Axial.Text)
    End Sub

    Private Sub txtApp_OscRot_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                               Handles txtApp_OscRot_Axial.Validating
        '=======================================================================================================
        txtApp_OscRot_Axial.Text = CleanInputNummeric(txtApp_OscRot_Axial.Text)
    End Sub

    Private Sub txtApp_OscV_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                             Handles txtApp_OscV_Axial.Validating
        '=====================================================================================================
        txtApp_OscV_Axial.Text = CleanInputNummeric(txtApp_OscV_Axial.Text)
    End Sub

    Private Sub txtApp_OscCycleRate_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                     Handles txtApp_OscCycleRate_Axial.Validating
        '=============================================================================================================
        txtApp_OscCycleRate_Axial.Text = CleanInputNummeric(txtApp_OscCycleRate_Axial.Text)
    End Sub

    Private Sub txtApp_OscServiceLife_Axial_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                       Handles txtApp_OscServiceLife_Axial.Validating
        '================================================================================================================
        txtApp_OscServiceLife_Axial.Text = CleanInputNummeric(txtApp_OscServiceLife_Axial.Text)
    End Sub

    Private Sub txtTest_CompressPre_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                    Handles txtTest_CompressPre_Leak.Validating
        '=============================================================================================================
        txtTest_CompressPre_Leak.Text = CleanInputNummeric(txtTest_CompressPre_Leak.Text)

    End Sub

    Private Sub txtTest_CompressPost_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                     Handles txtTest_CompressPost_Leak.Validating
        '=============================================================================================================
        txtTest_CompressPost_Leak.Text = CleanInputNummeric(txtTest_CompressPost_Leak.Text)

    End Sub

    Private Sub txtTest_PressPre_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                 Handles txtTest_PressPre_Leak.Validating
        '=========================================================================================================
        txtTest_PressPre_Leak.Text = CleanInputNummeric(txtTest_PressPre_Leak.Text)

    End Sub

    Private Sub txtTest_PressPost_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                  Handles txtTest_PressPost_Leak.Validating
        '=========================================================================================================
        txtTest_PressPost_Leak.Text = CleanInputNummeric(txtTest_PressPost_Leak.Text)

    End Sub

    Private Sub txtTest_ReqPre_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                               Handles txtTest_ReqPre_Leak.Validating
        '=======================================================================================================
        txtTest_ReqPre_Leak.Text = CleanInputNummeric(txtTest_ReqPre_Leak.Text)
    End Sub

    Private Sub txtTest_ReqPost_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles txtTest_ReqPost_Leak.Validating
        '=======================================================================================================
        txtTest_ReqPost_Leak.Text = CleanInputNummeric(txtTest_ReqPost_Leak.Text)
    End Sub

    Private Sub txtTest_CompressPre_Load_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                    Handles txtTest_CompressPre_Load.Validating
        '===========================================================================================================
        txtTest_CompressPre_Load.Text = CleanInputNummeric(txtTest_CompressPre_Load.Text)
    End Sub

    Private Sub txtTest_CompressPost_Load_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                     Handles txtTest_CompressPost_Load.Validating
        '===========================================================================================================
        txtTest_CompressPost_Load.Text = CleanInputNummeric(txtTest_CompressPost_Load.Text)
    End Sub

    Private Sub txtTest_ReqPre_Load_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                               Handles txtTest_ReqPre_Load.Validating
        '========================================================================================================
        txtTest_ReqPre_Load.Text = CleanInputNummeric(txtTest_ReqPre_Load.Text)
    End Sub

    Private Sub txtTest_ReqPost_Load_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles txtTest_ReqPost_Load.Validating
        '========================================================================================================
        txtTest_ReqPost_Load.Text = CleanInputNummeric(txtTest_ReqPost_Load.Text)
    End Sub

    Private Sub txtTest_CompressPre_SpringBack_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                          Handles txtTest_CompressPre_SpringBack.Validating
        '==================================================================================================================
        txtTest_CompressPre_SpringBack.Text = CleanInputNummeric(txtTest_CompressPre_SpringBack.Text)
    End Sub

    Private Sub txtTest_CompressPost_SpringBack_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                           Handles txtTest_CompressPost_SpringBack.Validating
        '===================================================================================================================
        txtTest_CompressPost_SpringBack.Text = CleanInputNummeric(txtTest_CompressPost_SpringBack.Text)
    End Sub

    Private Sub txtTest_ReqPre_SpringBack_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                     Handles txtTest_ReqPre_SpringBack.Validating
        '=============================================================================================================
        txtTest_ReqPre_SpringBack.Text = CleanInputNummeric(txtTest_ReqPre_SpringBack.Text)
    End Sub

    Private Sub txtTest_ReqPost_SpringBack_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                      Handles txtTest_ReqPost_SpringBack.Validating
        '=============================================================================================================
        txtTest_ReqPost_SpringBack.Text = CleanInputNummeric(txtTest_ReqPost_SpringBack.Text)
    End Sub

#End Region

#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbExport_Reqd_SelectedIndexChanged(sender As System.Object,
                                                    e As System.EventArgs) Handles cmbExport_Reqd.SelectedIndexChanged
        '================================================================================================================
        If (cmbExport_Reqd.Text = "N") Then
            cmbExport_Status.Enabled = False
            cmbExport_Status.SelectedIndex = -1
        Else
            cmbExport_Status.Enabled = True
            cmbExport_Status.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmbITAR_Export_SaleExportControlled_SelectedIndexChanged(sender As System.Object,
                                                                         e As System.EventArgs) Handles cmbITAR_Export_SaleExportControlled.SelectedIndexChanged
        '========================================================================================================================================================
        If (cmbITAR_Export_SaleExportControlled.Text = "N") Then
            cmbITAR_Export_EAR_Classification.Enabled = False
        Else
            If (mExport) Then
                cmbITAR_Export_EAR_Classification.Enabled = True
            End If

        End If

    End Sub

    Private Sub cmbITAR_Export_SaleExportControlled_TextChanged(sender As Object, e As EventArgs) Handles cmbITAR_Export_SaleExportControlled.TextChanged

        'If (mProcess_Project.ITAR_Export.SaleExportControlled <> cmbITAR_Export_SaleExportControlled.Text) Then
        '    grdExport_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName
        'End If

    End Sub


    Private Sub cmbITAR_Export_ProductITAR_Reg_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                                    Handles cmbITAR_Export_ProductITAR_Reg.SelectedIndexChanged
        '======================================================================================================================
        If (cmbITAR_Export_ProductITAR_Reg.Text = "N") Then
            txtITAR_Export_ITAR_Classification.Enabled = False
            cmbITAR_Export_Status.SelectedIndex = -1
            cmbITAR_Export_Status.Enabled = False
            txtExportStatus.Text = ""
            txtExportControlled.Text = "N"
        Else
            txtITAR_Export_ITAR_Classification.Enabled = True
            cmbITAR_Export_Status.Enabled = True
            cmbITAR_Export_Status.SelectedIndex = 0
            txtExportStatus.Text = cmbITAR_Export_Status.Text
            txtExportControlled.Text = "Y"
        End If

    End Sub

    Private Sub cmbITAR_Export_Status_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                           Handles cmbITAR_Export_Status.SelectedIndexChanged
        '========================================================================================================
        If (cmbITAR_Export_ProductITAR_Reg.Text = "N") Then
            'cmbITAR_Export_Status.Enabled = False
            txtExportStatus.Text = ""
        Else
            'cmbITAR_Export_Status.Enabled = True
            txtExportStatus.Text = cmbITAR_Export_Status.Text
        End If
        'txtExportStatus.Text = cmbITAR_Export_Status.Text
    End Sub

    Private Sub cmbApp_InsertLoc_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles cmbApp_InsertLoc.SelectedIndexChanged
        '=============================================================================================
        cmbApp_InsertLoc.Text = "Face"

        If (cmbApp_InsertLoc.Text = "Face") Then
            Dim pFaceExists As Boolean = False
            Dim pAxialExists As Boolean = False
            For i As Integer = 0 To tbApp.TabPages.Count - 1

                If (tbApp.TabPages(i).Name = "tbpFace") Then
                    pFaceExists = True
                ElseIf (tbApp.TabPages(i).Name = "tbpAxial") Then
                    pAxialExists = True
                End If
            Next

            If (Not pFaceExists) Then
                tbApp.TabPages.Add(tbpFace)
            End If

            If (pAxialExists) Then
                tbApp.TabPages.Remove(tbpAxial)
            End If

        ElseIf (cmbApp_InsertLoc.Text = "Axial") Then
            Dim pFaceExists As Boolean = False
            Dim pAxialExists As Boolean = False
            For i As Integer = 0 To tbApp.TabPages.Count - 1

                If (tbApp.TabPages(i).Name = "tbpFace") Then
                    pFaceExists = True
                ElseIf (tbApp.TabPages(i).Name = "tbpAxial") Then
                    pAxialExists = True
                End If
            Next

            If (pFaceExists) Then
                tbApp.TabPages.Remove(tbpFace)
            End If

            If (Not pAxialExists) Then
                tbApp.TabPages.Add(tbpAxial)
            End If
        End If

    End Sub

    Private Sub cmbApp_PressCycle_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles cmbApp_PressCycle.SelectedIndexChanged
        '===============================================================================================
        If (cmbApp_PressCycle.Text = "N") Then
            txtApp_PressCycleFreq.Enabled = False
            txtApp_PressCycleAmp.Enabled = False
            txtApp_PressCycleFreq.Text = ""
            txtApp_PressCycleAmp.Text = ""
        Else
            txtApp_PressCycleFreq.Enabled = True
            txtApp_PressCycleAmp.Enabled = True
        End If

    End Sub

    Private Sub cmbApp_Rotate_Axial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                        Handles cmbApp_Rotate_Axial.SelectedIndexChanged
        '=================================================================================================
        If (cmbApp_Rotate_Axial.Text = "N") Then
            txtApp_RotateRPM_Axial.Enabled = False
            txtApp_RotateRPM_Axial.Text = ""
        Else
            txtApp_RotateRPM_Axial.Enabled = True
        End If

    End Sub

    Private Sub cmbApp_Recip_Axial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                        Handles cmbApp_Recip_Axial.SelectedIndexChanged
        '================================================================================================
        If (cmbApp_Recip_Axial.Text = "N") Then
            txtApp_RecipStrokeL_Axial.Enabled = False
            txtApp_RecipV_Axial.Enabled = False
            txtApp_RecipCycleRate_Axial.Enabled = False
            txtApp_RecipServiceLife_Axial.Enabled = False

            txtApp_RecipStrokeL_Axial.Text = ""
            txtApp_RecipV_Axial.Text = ""
            txtApp_RecipCycleRate_Axial.Text = ""
            txtApp_RecipServiceLife_Axial.Text = ""
        Else
            txtApp_RecipStrokeL_Axial.Enabled = True
            txtApp_RecipV_Axial.Enabled = True
            txtApp_RecipCycleRate_Axial.Enabled = True
            txtApp_RecipServiceLife_Axial.Enabled = True
        End If
    End Sub

    Private Sub cmbApp_Osc_Axial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles cmbApp_Osc_Axial.SelectedIndexChanged
        '=================================================================================================
        If (cmbApp_Osc_Axial.Text = "N") Then
            txtApp_OscRot_Axial.Enabled = False
            txtApp_OscV_Axial.Enabled = False
            txtApp_OscCycleRate_Axial.Enabled = False
            txtApp_OscServiceLife_Axial.Enabled = False

            txtApp_OscRot_Axial.Text = ""
            txtApp_OscV_Axial.Text = ""
            txtApp_OscCycleRate_Axial.Text = ""
            txtApp_OscServiceLife_Axial.Text = ""
        Else
            txtApp_OscRot_Axial.Enabled = True
            txtApp_OscV_Axial.Enabled = True
            txtApp_OscCycleRate_Axial.Enabled = True
            txtApp_OscServiceLife_Axial.Enabled = True
        End If
    End Sub

    Private Sub cmbDesign_Winnovation_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                           Handles cmbDesign_Winnovation.SelectedIndexChanged
        '====================================================================================================
        If (cmbDesign_Winnovation.Text = "N") Then
            txtDesign_WinnovationNo.Enabled = False
            txtDesign_WinnovationNo.Text = ""
        Else
            txtDesign_WinnovationNo.Enabled = True
        End If

    End Sub

    Private Sub cmbCoating_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles cmbCoating.SelectedIndexChanged
        '========================================================================================
        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
            If (gPartProject.PNR.HW.Coating = "None") Then
                chkCoating.Checked = False
                cmbCoating.Text = ""
            Else
                chkCoating.Checked = True
                cmbCoating.Text = gPartProject.PNR.HW.Coating
            End If

        End If

    End Sub

    Private Sub cmbQuality_CustComplaint_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                             Handles cmbQuality_CustComplaint.SelectedIndexChanged
        '=========================================================================================================
        If (cmbQuality_CustComplaint.Text = "N") Then
            txtQuality_Reason.Text = ""
            txtQuality_Reason.Enabled = False
        Else
            If (mQlty) Then
                txtQuality_Reason.Enabled = True
            Else
                txtQuality_Reason.Enabled = False
            End If

        End If

    End Sub


    Private Sub cmbQuality_VisualInspection_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                                Handles cmbQuality_VisualInspection.SelectedIndexChanged
        '===============================================================================================================
        If (cmbQuality_VisualInspection.Text = "N") Then
            cmbQuality_VisualInspection_Type.Text = ""
            cmbQuality_VisualInspection_Type.Enabled = False
        Else
            cmbQuality_VisualInspection_Type.Text = ""
            If (mQlty) Then
                cmbQuality_VisualInspection_Type.Enabled = True
            End If

        End If

    End Sub

    Private Sub cmbPartFamily_MouseHover(sender As Object, e As EventArgs) Handles cmbPartFamily.MouseHover
        '==================================================================================================
        ToolTip1.SetToolTip(cmbPartFamily, "Enter Data in SealPart.")
        'cmdSealPart.Focus()      'AES 24APR18
    End Sub

    Private Sub cmbPartFamily_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPartFamily.SelectedIndexChanged
        '======================================================================================================================
        cmbPartFamily.Text = gPartProject.PNR.SealType.ToString() & "-Seal"
        
    End Sub

    Private Sub cmbPartFamily_MouseLeave(sender As Object, e As EventArgs) Handles cmbPartFamily.MouseLeave
        '==================================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbApp_Face_POrient_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                         Handles cmbApp_Face_POrient.SelectedIndexChanged
        '================================================================================================
        cmbApp_Face_POrient.Text = gPartProject.PNR.HW.POrient
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbApp_Face_POrient_MouseHover(sender As Object, e As EventArgs) Handles cmbApp_Face_POrient.MouseHover
        '=============================================================================================================
        ToolTip1.SetToolTip(cmbApp_Face_POrient, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbApp_Face_POrient_MouseLeave(sender As Object, e As EventArgs) Handles cmbApp_Face_POrient.MouseLeave
        '==============================================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbParkerPN_Part2_NewRef_Dim_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                                  Handles cmbParkerPN_Part2_NewRef_Dim.SelectedIndexChanged
        '==================================================================================================================
        If (gPartProject.PNR.RefDimCurrent.Exists) Then
            Dim pParkerPN As String = "NH-" & gPartProject.PNR.RefDimCurrent.TypeNo & gPartProject.PNR.RefDimCurrent.Val
            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
            cmbParkerPN_Part2_NewRef_Dim.Text = pParkerPN_Prefix
        Else
            cmbParkerPN_Part2_NewRef_Dim.Text = ""
        End If
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbParkerPN_Part2_Notes_Dim_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                                 Handles cmbParkerPN_Part2_Notes_Dim.SelectedIndexChanged
        '================================================================================================================
        If (gPartProject.PNR.RefNotesCurrent.Exists) Then
            Dim pParkerPN As String = "NH-" & gPartProject.PNR.RefNotesCurrent.TypeNo & gPartProject.PNR.RefNotesCurrent.Val
            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
            cmbParkerPN_Part2_Notes_Dim.Text = pParkerPN_Prefix
        Else
            cmbParkerPN_Part2_Notes_Dim.Text = ""

        End If
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbParkerPN_Part2_NewRef_Dim_MouseHover(sender As Object, e As EventArgs) _
                                                        Handles cmbParkerPN_Part2_NewRef_Dim.MouseHover
        '==============================================================================================
        ToolTip1.SetToolTip(cmbParkerPN_Part2_NewRef_Dim, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbParkerPN_Part2_NewRef_Dim_MouseLeave(sender As Object, e As EventArgs) _
                                                        Handles cmbParkerPN_Part2_NewRef_Dim.MouseLeave
        '==============================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbParkerPN_Part2_Notes_Dim_MouseHover(sender As Object, e As EventArgs) _
                                                       Handles cmbParkerPN_Part2_Notes_Dim.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(cmbParkerPN_Part2_Notes_Dim, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbParkerPN_Part2_Notes_Dim_MouseLeave(sender As Object, e As EventArgs) _
                                                       Handles cmbParkerPN_Part2_Notes_Dim.MouseLeave
        '============================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub


    Private Sub cmbDesign_Mat_Seal_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                        Handles cmbDesign_Mat_Seal.SelectedIndexChanged
        '==============================================================================================
        cmbDesign_Mat_Seal.Text = gPartProject.PNR.HW.MatName
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_Mat_Spring_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                          Handles cmbDesign_Mat_Spring.SelectedIndexChanged
        '==================================================================================================
        cmbDesign_Mat_Spring.Text = gPartProject.PNR.HW.MatName
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_Mat_Seal_MouseHover(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Seal.MouseHover
        '============================================================================================================
        ToolTip1.SetToolTip(cmbDesign_Mat_Seal, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_Mat_Seal_MouseLeave(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Seal.MouseLeave
        '============================================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_Mat_Spring_MouseLeave(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Spring.MouseLeave
        '================================================================================================================
        'cmdSealPart.Focus()     'AES 24APR18
    End Sub

    Private Sub cmbDesign_Mat_Spring_MouseHover(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Spring.MouseHover
        '================================================================================================================
        ToolTip1.SetToolTip(cmbDesign_Mat_Spring, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_TemperType_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                          Handles cmbDesign_TemperType.SelectedIndexChanged
        '==================================================================================================
        Dim pTemperCode As Integer = gPartProject.PNR.HW.Temper

        If (pTemperCode = 1) Then
            cmbDesign_TemperType.Text = "Work(Hardened)"
        ElseIf (pTemperCode = 2) Then
            cmbDesign_TemperType.Text = "Age(Hardened)"
        ElseIf (pTemperCode = 4) Then
            cmbDesign_TemperType.Text = "Annealed"
        ElseIf (pTemperCode = 6) Then
            cmbDesign_TemperType.Text = "Solution and Precip"
        ElseIf (pTemperCode = 8) Then
            cmbDesign_TemperType.Text = "NACE"
        End If
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_TemperType_MouseHover(sender As Object, e As EventArgs) Handles cmbDesign_TemperType.MouseHover
        '================================================================================================================
        ToolTip1.SetToolTip(cmbDesign_TemperType, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbDesign_TemperType_MouseLeave(sender As Object, e As EventArgs) Handles cmbDesign_TemperType.MouseLeave
        '================================================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbSFinish_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSFinish.SelectedIndexChanged
        '================================================================================================================
        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then

            If (gPartProject.PNR.HW.SFinish = "0") Then
                cmbSFinish.Text = ""
            Else
                cmbSFinish.Text = gPartProject.PNR.HW.SFinish
            End If
        End If
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbCoating_MouseLeave(sender As Object, e As EventArgs) Handles cmbCoating.MouseLeave
        '============================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbCoating_MouseHover(sender As Object, e As EventArgs) Handles cmbCoating.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(cmbCoating, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbSFinish_MouseHover(sender As Object, e As EventArgs) Handles cmbSFinish.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(cmbSFinish, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18

    End Sub

    Private Sub cmbSFinish_MouseLeave(sender As Object, e As EventArgs) Handles cmbSFinish.MouseLeave
        '============================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbPlatingCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPlatingCode.SelectedIndexChanged
        '========================================================================================================================
        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            If (gPartProject.PNR.HW.Plating.Code <> "") Then
                'chkPlating.Checked = True
                cmbPlatingCode.Text = gPartProject.PNR.HW.Plating.Code
                'cmbPlatingThickCode.Text = gPartProject.PNR.HW.Plating.ThickCode
            End If
        End If
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbPlatingCode_MouseHover(sender As Object, e As EventArgs) Handles cmbPlatingCode.MouseHover
        '===================================================================================================
        ToolTip1.SetToolTip(cmbPlatingCode, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbPlatingCode_MouseLeave(sender As Object, e As EventArgs) Handles cmbPlatingCode.MouseLeave
        '====================================================================================================
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbPlatingThickCode_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                         Handles cmbPlatingThickCode.SelectedIndexChanged
        '================================================================================================
        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            If (gPartProject.PNR.HW.Plating.Code <> "") Then
                cmbPlatingThickCode.Text = gPartProject.PNR.HW.Plating.ThickCode
            End If
        End If
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbPlatingThickCode_MouseHover(sender As Object, e As EventArgs) Handles cmbPlatingThickCode.MouseHover
        '==============================================================================================================
        ToolTip1.SetToolTip(cmbPlatingThickCode, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub cmbPlatingThickCode_MouseLeave(sender As Object, e As EventArgs) Handles cmbPlatingThickCode.MouseLeave
        '==============================================================================================================
        'cmdSealPart.Focus()     'AES 24APR18

    End Sub

    Private Sub cmbTest_QtyPre_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                               Handles cmbTest_QtyPre_Leak.Validating
        '=======================================================================================================
        cmbTest_QtyPre_Leak.Text = CleanInputNummeric(cmbTest_QtyPre_Leak.Text)
    End Sub

    Private Sub cmbTest_QtyPost_Leak_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles cmbTest_QtyPost_Leak.Validating
        '=======================================================================================================
        cmbTest_QtyPost_Leak.Text = CleanInputNummeric(cmbTest_QtyPost_Leak.Text)
    End Sub

    Private Sub cmbTest_QtyPre_Load_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                               Handles cmbTest_QtyPre_Load.Validating
        '=========================================================================================================
        cmbTest_QtyPre_Load.Text = CleanInputNummeric(cmbTest_QtyPre_Load.Text)

    End Sub

    Private Sub cmbTest_QtyPost_Load_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                Handles cmbTest_QtyPost_Load.Validating
        '=========================================================================================================
        cmbTest_QtyPost_Load.Text = CleanInputNummeric(cmbTest_QtyPost_Load.Text)
    End Sub

    Private Sub cmbTest_QtyPre_SpringBack_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                     Handles cmbTest_QtyPre_SpringBack.Validating
        '=============================================================================================================
        cmbTest_QtyPre_SpringBack.Text = CleanInputNummeric(cmbTest_QtyPre_SpringBack.Text)
    End Sub

    Private Sub cmbTest_QtyPost_SpringBack_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) _
                                                      Handles cmbTest_QtyPost_SpringBack.Validating
        '==============================================================================================================
        cmbTest_QtyPost_SpringBack.Text = CleanInputNummeric(cmbTest_QtyPost_SpringBack.Text)
    End Sub

#End Region

#Region "CHECK BOX RELATED ROUTINES:"
    Private Sub chkUserSigned_CheckedChanged(sender As System.Object,
                                                     e As System.EventArgs) Handles chkPreOrderUserSigned.CheckedChanged
        '================================================================================================================
        txtPreOrderUserName.Text = gUser.FirstName + " " + gUser.LastName
        txtPreOrderUserDate.Text = DateTime.Now.ToShortDateString()

        If (chkPreOrderUserSigned.Checked) Then
            mPreOrder = False
            cmdPreOrderUserSign.Text = "Signed-off"
        Else
            mPreOrder = True
            cmdPreOrderUserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpPreOrderEdited.Enabled = True
        chkPreOrderUserSigned.Enabled = True
        cmdPreOrderUserSign.Enabled = True
        TabControl1.Refresh()

    End Sub


    Private Sub chkITAR_Export_UserSigned_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                         Handles chkITAR_Export_UserSigned.CheckedChanged
        '==================================================================================================
        txtITAR_Export_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtITAR_Export_UserDate.Text = DateTime.Now.ToShortDateString()
        If (chkITAR_Export_UserSigned.Checked) Then
            mExport = False
            cmdITAR_Export_UserSign.Text = "Signed-off"
        Else
            mExport = True
            cmdITAR_Export_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpExportEdited.Enabled = True
        chkITAR_Export_UserSigned.Enabled = True
        cmdITAR_Export_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkOrdEntry_UserSigned_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles chkOrdEntry_UserSigned.CheckedChanged
        '===============================================================================================
        txtOrdEntry_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtOrdEntry_UserDate.Text = DateTime.Now.ToShortDateString()
        If (chkOrdEntry_UserSigned.Checked) Then
            mOrdEntry = False
            cmdOrdEntry_UserSign.Text = "Signed-off"
        Else
            mOrdEntry = True
            cmdOrdEntry_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpOrdEntryEdited.Enabled = True
        chkOrdEntry_UserSigned.Enabled = True
        cmdOrdEntry_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkCost_UserSigned_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                  Handles chkCost_UserSigned.CheckedChanged
        '===========================================================================================
        txtCost_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtCost_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkCost_UserSigned.Checked) Then
            mCost = False
            cmdCost_UserSign.Text = "Signed-off"
        Else
            mCost = True
            cmdCost_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpCostEdited.Enabled = True
        chkCost_UserSigned.Enabled = True
        cmdCost_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkApp_UserSigned_Face_CheckedChanged(sender As Object, e As EventArgs) _
                                                      Handles chkApp_UserSigned_Face.CheckedChanged
        '==========================================================================================
        txtApp_UserName_Face.Text = gUser.FirstName + " " + gUser.LastName
        txtApp_UserDate_Face.Text = DateTime.Now.ToShortDateString()

        If (chkApp_UserSigned_Face.Checked) Then
            mApp = False
            cmdApp_UserSign_Face.Text = "Signed-off"
        Else
            mApp = True
            cmdApp_UserSign_Face.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpApp_Face_Edited.Enabled = True
        chkApp_UserSigned_Face.Enabled = True
        cmdApp_UserSign_Face.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkDesign_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                    Handles chkDesign_UserSigned.CheckedChanged
        '=======================================================================================
        txtDesign_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtDesign_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkDesign_UserSigned.Checked) Then
            mDesign = False
            cmdDesign_UserSign.Text = "Signed-off"
        Else
            mDesign = True
            cmdDesign_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpDesign_Edited.Enabled = True
        chkDesign_UserSigned.Enabled = True
        cmdDesign_UserSign.Enabled = True
        TabControl1.Refresh()

    End Sub

    Private Sub chkManf_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                  Handles chkManf_UserSigned.CheckedChanged
        '===================================================================================
        txtManf_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtManf_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkManf_UserSigned.Checked) Then
            mManf = False
            cmdManf_UserSign.Text = "Signed-off"
        Else
            mManf = True
            cmdManf_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpManf_Edited.Enabled = True
        chkManf_UserSigned.Enabled = True
        cmdManf_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkPurchase_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                      Handles chkPurchase_UserSigned.CheckedChanged
        '============================================================================================
        txtPurchase_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtPurchase_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkPurchase_UserSigned.Checked) Then
            mPurchase = False
            cmdPurchase_UserSign.Text = "Signed-off"
        Else
            mPurchase = True
            cmdPurchase_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpPurchase_Edited.Enabled = True
        chkPurchase_UserSigned.Enabled = True
        cmdPurchase_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkQuality_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                     Handles chkQuality_UserSigned.CheckedChanged
        '=========================================================================================
        txtQuality_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtQuality_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkQuality_UserSigned.Checked) Then
            mQlty = False
            cmdQuality_UserSign.Text = "Signed-off"
        Else
            mQlty = True
            cmdQuality_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpQlty_Edited.Enabled = True
        chkQuality_UserSigned.Enabled = True
        cmdQuality_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkDwg_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                 Handles chkDwg_UserSigned.CheckedChanged
        '=================================================================================
        txtDwg_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtDwg_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkDwg_UserSigned.Checked) Then
            mDwg = False
            cmdDwg_UserSign.Text = "Signed-off"
        Else
            mDwg = True
            cmdDwg_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpDwg_Edited.Enabled = True
        chkDwg_UserSigned.Enabled = True
        cmdDwg_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkTest_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                  Handles chkTest_UserSigned.CheckedChanged
        '====================================================================================
        txtTest_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtTest_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkTest_UserSigned.Checked) Then
            mTest = False
            cmdTest_UserSign.Text = "Signed-off"
        Else
            mTest = True
            cmdTest_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpTest_Edited.Enabled = True
        chkTest_UserSigned.Enabled = True
        cmdTest_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub
    Private Sub chkPlanning_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                      Handles chkPlanning_UserSigned.CheckedChanged
        '===========================================================================================
        txtPlanning_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtPlanning_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkPlanning_UserSigned.Checked) Then
            mPlanning = False
            cmdPlanning_UserSign.Text = "Signed-off"
        Else
            mPlanning = True
            cmdPlanning_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpPlanning_Edited.Enabled = True
        chkPlanning_UserSigned.Enabled = True
        cmdPlanning_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkShipping_UserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                      Handles chkShipping_UserSigned.CheckedChanged
        '============================================================================================
        txtShipping_UserName.Text = gUser.FirstName + " " + gUser.LastName
        txtShipping_UserDate.Text = DateTime.Now.ToShortDateString()

        If (chkShipping_UserSigned.Checked) Then
            mShipping = False
            cmdShipping_UserSign.Text = "Signed-off"
        Else
            mShipping = True
            cmdShipping_UserSign.Text = "Sign-off"
        End If

        Initialize_tbTesting_Controls()
        ReInitializeControls()
        grpPlanning_Edited.Enabled = True
        chkShipping_UserSigned.Enabled = True
        cmdShipping_UserSign.Enabled = True
        TabControl1.Refresh()
    End Sub

    Private Sub chkNewRef_Dim_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                             Handles chkNewRef_Dim.CheckedChanged
        '=====================================================================================
        If (chkNewRef_Dim.Checked) Then
            chkLegacyRef_Dim.Checked = False
        End If
    End Sub

    Private Sub chkLegacyRef_Notes_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                  Handles chkLegacyRef_Notes.CheckedChanged
        '==========================================================================================
        If (chkLegacyRef_Notes.Checked) Then
            chkNewRef_Notes.Checked = False
        End If

    End Sub

    Private Sub chkNewRef_Notes_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                               Handles chkNewRef_Notes.CheckedChanged
        '=======================================================================================
        If (chkNewRef_Notes.Checked) Then
            chkLegacyRef_Notes.Checked = False
        End If

    End Sub

    Private Sub chkCoating_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                        Handles chkCoating.CheckedChanged
        '==================================================================================
        If chkCoating.Checked Then
            cmbCoating.Enabled = True
            cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCoating.SelectedIndex = 0
            cmbCoating.Text = gPartProject.PNR.HW.Coating

            If cmbCoating.Text = "T800" Then
                lblSFinish.Enabled = True
                cmbSFinish.Enabled = True
                cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                cmbSFinish.Text = gPartProject.PNR.HW.SFinish.ToString()
                lblUnitSFinish.Enabled = True
            End If

        Else
            cmbCoating.Enabled = False
            cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
            cmbCoating.Text = ""

            lblSFinish.Enabled = False
            cmbSFinish.Enabled = False
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
            cmbSFinish.Text = ""
            lblUnitSFinish.Enabled = False

        End If

    End Sub

    Private Sub chkPlating_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                         Handles chkPlating.CheckedChanged
        '===================================================================================
        If (chkPlating.Checked) Then
            'If (grpPlating.Enabled) Then
            cmbPlatingCode.Enabled = True
            cmbPlatingThickCode.Enabled = True

            cmbPlatingCode.DropDownStyle = ComboBoxStyle.DropDownList
            cmbPlatingThickCode.DropDownStyle = ComboBoxStyle.DropDownList

            cmbPlatingCode.SelectedIndex = 0
            cmbPlatingThickCode.SelectedIndex = 0
            'End If

        Else
            cmbPlatingCode.Enabled = False
            cmbPlatingThickCode.Enabled = False

            cmbPlatingCode.DropDownStyle = ComboBoxStyle.DropDown
            cmbPlatingCode.Text = ""

            cmbPlatingThickCode.DropDownStyle = ComboBoxStyle.DropDown
            cmbPlatingThickCode.Text = ""

        End If

    End Sub

    Private Sub chkLegacyRef_Dim_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles chkLegacyRef_Dim.CheckedChanged
        '========================================================================================
        If (chkLegacyRef_Dim.Checked) Then
            chkNewRef_Dim.Checked = False
        End If

    End Sub

    Private Sub chkTest_CheckedChanged(sender As Object, e As EventArgs) Handles chkTest.CheckedChanged
        '==============================================================================================
        If (chkTest.Checked = False) Then
            '....Leak
            txtTest_CompressPre_Leak.Text = ""
            txtTest_CompressPost_Leak.Text = ""
            cmbTest_MediaPre_Leak.SelectedIndex = -1
            cmbTest_MediaPost_Leak.SelectedIndex = -1
            txtTest_PressPre_Leak.Text = ""
            txtTest_PressPost_Leak.Text = ""
            txtTest_ReqPre_Leak.Text = ""
            txtTest_ReqPost_Leak.Text = ""

            cmbTest_QtyPre_Leak.Text = ""
            cmbTest_QtyPre_Leak.SelectedIndex = -1

            cmbTest_QtyPost_Leak.Text = ""
            cmbTest_QtyPost_Leak.SelectedIndex = -1

            cmbTest_FreqPre_Leak.Text = ""
            cmbTest_FreqPre_Leak.SelectedIndex = -1

            cmbTest_FreqPost_Leak.Text = ""
            cmbTest_FreqPost_Leak.SelectedIndex = -1

            '....Load
            txtTest_CompressPre_Load.Text = ""
            txtTest_CompressPost_Load.Text = ""
            txtTest_ReqPre_Load.Text = ""
            txtTest_ReqPost_Load.Text = ""

            cmbTest_QtyPre_Load.Text = ""
            cmbTest_QtyPre_Load.SelectedIndex = -1
            cmbTest_QtyPost_Load.Text = ""
            cmbTest_QtyPost_Load.SelectedIndex = -1
            cmbTest_FreqPre_Load.Text = ""
            cmbTest_FreqPre_Load.SelectedIndex = -1
            cmbTest_FreqPost_Load.Text = ""
            cmbTest_FreqPost_Load.SelectedIndex = -1

            '....SpringBack
            txtTest_CompressPre_SpringBack.Text = ""
            txtTest_CompressPost_SpringBack.Text = ""
            txtTest_ReqPre_SpringBack.Text = ""
            txtTest_ReqPost_SpringBack.Text = ""

            cmbTest_QtyPre_SpringBack.Text = ""
            cmbTest_QtyPre_SpringBack.SelectedIndex = -1

            cmbTest_QtyPost_SpringBack.Text = ""
            cmbTest_QtyPost_SpringBack.SelectedIndex = -1

            cmbTest_FreqPre_SpringBack.Text = ""
            cmbTest_FreqPre_SpringBack.SelectedIndex = -1

            cmbTest_FreqPost_SpringBack.Text = ""
            cmbTest_FreqPost_SpringBack.SelectedIndex = -1

            txtTest_Other.Text = ""
            txtTest_Other.Enabled = False

            EnableTab(tabLeak, False)
            EnableTab(tabLoad, False)
            EnableTab(tabSpringBack, False)
        Else
            If (mTest) Then
                EnableTab(tabLeak, True)
                EnableTab(tabLoad, True)
                EnableTab(tabSpringBack, True)
                txtTest_Other.Enabled = True
            End If

        End If

    End Sub

#Region "HELPER ROUTINES:"

    Public Sub EnableTab(ByVal page As TabPage, ByVal enable As Boolean)
        '===============================================================
        EnableControls(page.Controls, enable)
    End Sub

    Private Sub EnableControls(ByVal ctls As Control.ControlCollection, ByVal enable As Boolean)
        '=======================================================================================
        For Each ctl As Control In ctls
            ctl.Enabled = enable
            EnableControls(ctl.Controls, enable)
        Next
    End Sub

#End Region



#End Region

#Region "DATETIME PICKER RELATED ROUTINES:"
    Private Sub dtpStartDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpStartDate.ValueChanged
        '==================================================================================================================
        Dim pCI As New CultureInfo("en-US")
        txtStartDate.Text = dtpStartDate.Value.ToShortDateString()

        txtDateMod.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())

        If (gUser.FirstName <> "") Then
            txtModifiedBy.Text = gUser.FirstName & " " & gUser.LastName
        End If

    End Sub

    Private Sub dtpDateMod_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpDateMod.ValueChanged
        '==============================================================================================================
        'txtDateMod.Text = dtpDateMod.Value.ToShortDateString()
        'If (gUser.FirstName <> "") Then
        '    txtModifiedBy.Text = gUser.FirstName & " " & gUser.LastName
        'End If
    End Sub

    Private Sub dtpPreOrderUserDate_ValueChanged(sender As System.Object,
                                                 e As System.EventArgs) Handles dtpPreOrderUserDate.ValueChanged
        '=========================================================================================================
        txtPreOrderUserDate.Text = dtpPreOrderUserDate.Value.ToShortDateString()
    End Sub

    Private Sub dtpOrdEntry_SalesDate_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                                   Handles dtpOrdEntry_SalesDate.ValueChanged
        '===========================================================================================
        txtOrdEntry_SalesDate.Text = dtpOrdEntry_SalesDate.Value.ToShortDateString()
    End Sub

    Private Sub dtpOrdEntry_PODate_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles dtpOrdEntry_PODate.ValueChanged
        '=========================================================================================
        txtOrdEntry_PODate.Text = dtpOrdEntry_PODate.Value.ToShortDateString()
    End Sub

    Private Sub dtpOrdEntry_PODate_EDI_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                                    Handles dtpOrdEntry_PODate_EDI.ValueChanged
        '============================================================================================
        txtOrdEntry_PODate_EDI.Text = dtpOrdEntry_PODate_EDI.Value.ToShortDateString()
    End Sub

    Private Sub dtpOrdEntry_OrderShipDate_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                                       Handles dtpOrdEntry_OrderShipDate.ValueChanged
        '================================================================================================
        txtOrdEntry_OrderShipDate.Text = dtpOrdEntry_OrderShipDate.Value.ToShortDateString()
    End Sub

    Private Sub dtpITAR_Export_UserDate_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                                    Handles dtpITAR_Export_UserDate.ValueChanged
        '==============================================================================================
        txtITAR_Export_UserDate.Text = dtpITAR_Export_UserDate.Value.ToShortDateString()
    End Sub

    Private Sub dtpOrdEntry_UserDate_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                                 Handles dtpOrdEntry_UserDate.ValueChanged
        '===========================================================================================
        txtOrdEntry_UserDate.Text = dtpOrdEntry_UserDate.Value.ToShortDateString()
    End Sub

    Private Sub dtpCost_UserDate_ValueChanged(sender As System.Object, e As System.EventArgs) _
                                              Handles dtpCost_UserDate.ValueChanged
        '=======================================================================================
        txtCost_UserDate.Text = dtpCost_UserDate.Value.ToShortDateString()
    End Sub

#End Region

#Region "DATAGRIDVIEW RELATED ROUTINES:"

    Private Sub grdApp_Axial_Cavity_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                                   Handles grdApp_Axial_Cavity.CellValidating
        '=========================================================================================================
        If (grdApp_Axial_Cavity.CurrentCellAddress.X = DataGridViewComboBoxColumn6.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn6.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn6.Items.Add(e.FormattedValue)
                grdApp_Axial_Cavity.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If

        Dim pCell As DataGridViewCell = grdApp_Axial_Cavity.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdApp_Axial_Cavity.EditingControl

            Select Case grdApp_Axial_Cavity.Columns(e.ColumnIndex).HeaderText
                Case "Assy. Min", "Assy. Max", "Oper. Min", "Oper. Max"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdApp_Axial_Cavity_EditingControlShowing(sender As Object,
                                                          e As DataGridViewEditingControlShowingEventArgs) _
                                                          Handles grdApp_Axial_Cavity.EditingControlShowing
        '=====================================================================================================
        If (grdApp_Axial_Cavity.CurrentCellAddress.X = DataGridViewComboBoxColumn6.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdApproval_Attendees_EditingControlShowing(sender As System.Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) _
                                                            Handles grdApproval_Attendees.EditingControlShowing
        '===================================================================================================================================================
        Dim pComboBox As ComboBox = TryCast(e.Control, ComboBox)

        'AES 02APR18
        'Dim pRowIndex As Integer = grdApproval_Attendees.CurrentCell.RowIndex
        'Dim pUserName As String = ""
        'pUserName = gUser.FirstName + " " + gUser.LastName
        'If (gUser.Role = grdApproval_Attendees.Rows(pRowIndex).Cells(0).Value) Then
        '    grdApproval_Attendees.Rows(pRowIndex).ReadOnly = False
        If (pComboBox IsNot Nothing) Then
            RemoveHandler pComboBox.SelectionChangeCommitted, New EventHandler(AddressOf ComboBox_SelectionChangeCommitted)

            mRowIndex = grdApproval_Attendees.CurrentCell.RowIndex
            AddHandler pComboBox.SelectionChangeCommitted, New EventHandler(AddressOf ComboBox_SelectionChangeCommitted)
        End If
        'Else
        '    grdApproval_Attendees.Rows(pRowIndex).ReadOnly = True
        'End If

    End Sub

    Private Sub grdManf_ToolNGage_CellValidating(sender As Object,
                                                 e As DataGridViewCellValidatingEventArgs) _
                                                 Handles grdManf_ToolNGage.CellValidating
        '======================================================================================

        If (grdManf_ToolNGage.CurrentCellAddress.X = DataGridViewTextBoxColumn12.DisplayIndex) Then
            If (Not DataGridViewTextBoxColumn12.Items.Contains(e.FormattedValue)) Then
                DataGridViewTextBoxColumn12.Items.Add(e.FormattedValue)
                grdManf_ToolNGage.Rows(e.RowIndex).Cells(1).Value = e.FormattedValue
            End If
            'ElseIf (grdManf_ToolNGage.CurrentCellAddress.X = Column2.DisplayIndex) Then
            '    If (Not Column2.Items.Contains(e.FormattedValue)) Then
            '        Column2.Items.Add(e.FormattedValue)
            '        grdManf_ToolNGage.Rows(e.RowIndex).Cells(2).Value = e.FormattedValue
            '    End If
            'ElseIf (grdManf_ToolNGage.CurrentCellAddress.X = Column7.DisplayIndex) Then
            '    If (Not Column7.Items.Contains(e.FormattedValue)) Then
            '        Column7.Items.Add(e.FormattedValue)
            '        grdManf_ToolNGage.Rows(e.RowIndex).Cells(3).Value = e.FormattedValue
            '    End If
            'ElseIf (grdManf_ToolNGage.CurrentCellAddress.X = Column8.DisplayIndex) Then
            '    If (Not Column8.Items.Contains(e.FormattedValue)) Then
            '        Column8.Items.Add(e.FormattedValue)
            '        grdManf_ToolNGage.Rows(e.RowIndex).Cells(5).Value = e.FormattedValue
            '    End If
        End If

        Dim pCell As DataGridViewCell = grdManf_ToolNGage.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdManf_ToolNGage.EditingControl

            Select Case grdManf_ToolNGage.Columns(e.ColumnIndex).HeaderText
                Case "Lead Time (wks)"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub
    Private Sub grdManf_ToolNGage_EditingControlShowing(sender As System.Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) _
                                                        Handles grdManf_ToolNGage.EditingControlShowing
        '===============================================================================================================================================
        Try
            Dim pComboBoxManf As ComboBox = TryCast(e.Control, ComboBox)

            If (pComboBoxManf IsNot Nothing) Then
                RemoveHandler pComboBoxManf.SelectionChangeCommitted, New EventHandler(AddressOf ComboBoxManf_SelectionChangeCommitted)

                AddHandler pComboBoxManf.SelectionChangeCommitted, New EventHandler(AddressOf ComboBoxManf_SelectionChangeCommitted)
            End If

            If (grdManf_ToolNGage.CurrentCellAddress.X = DataGridViewTextBoxColumn12.DisplayIndex) Then
                Dim pCmbBox As ComboBox = e.Control

                If (Not IsNothing(pCmbBox)) Then
                    pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
                End If
                'ElseIf (grdManf_ToolNGage.CurrentCellAddress.X = Column2.DisplayIndex) Then
                '    Dim pCmbBox As ComboBox = e.Control

                '    If (Not IsNothing(pCmbBox)) Then
                '        pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
                '    End If
                'ElseIf (grdManf_ToolNGage.CurrentCellAddress.X = Column7.DisplayIndex) Then
                '    Dim pCmbBox As ComboBox = e.Control

                '    If (Not IsNothing(pCmbBox)) Then
                '        pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
                '    End If
                'ElseIf (grdManf_ToolNGage.CurrentCellAddress.X = Column8.DisplayIndex) Then
                '    Dim pCmbBox As ComboBox = e.Control

                '    If (Not IsNothing(pCmbBox)) Then
                '        pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
                '    End If
            End If


        Catch ex As Exception

        End Try

    End Sub

    Private Sub grdIssueComment_CellClick(sender As Object,
                                          e As DataGridViewCellEventArgs) Handles grdIssueComment.CellClick
        '===================================================================================================
        Dim pRowIndex As Integer = grdIssueComment.CurrentRow.Index
        Dim pColumnIndex As Integer = grdIssueComment.CurrentCell.ColumnIndex
        Dim pColumnName As String = grdIssueComment.Columns(grdIssueComment.CurrentCell.ColumnIndex).Name

        If pColumnName = "Column17" Then
            Dim pProcess_frmIssueComnt_Resolution As New Process_frmIssueComnt_Resolution(mProcess_Project, pRowIndex)
            pProcess_frmIssueComnt_Resolution.ShowDialog()
        End If

        If (e.ColumnIndex = 7) Then
            mDTP_IssueComment = New DateTimePicker()
            grdIssueComment.Controls.Add(mDTP_IssueComment)
            mDTP_IssueComment.Format = DateTimePickerFormat.Short
            Dim pRectangle As Rectangle = grdIssueComment.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            mDTP_IssueComment.Size = New Size(pRectangle.Width, pRectangle.Height)
            mDTP_IssueComment.Location = New Point(pRectangle.X, pRectangle.Y)
            AddHandler mDTP_IssueComment.CloseUp, New EventHandler(AddressOf mDateTimePicker_CloseUp)
            AddHandler mDTP_IssueComment.ValueChanged, New EventHandler(AddressOf mDateTimePicker_OnTextChange)
        End If

    End Sub

    Private Sub grdCustContact_EditingControlShowing(sender As Object,
                                                     e As DataGridViewEditingControlShowingEventArgs) _
                                                     Handles grdCustContact.EditingControlShowing
        '===============================================================================================
        If (grdCustContact.CurrentCellAddress.X = DataGridViewComboBoxColumn4.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdCustContact_CellValidating(sender As Object,
                                              e As DataGridViewCellValidatingEventArgs) _
                                              Handles grdCustContact.CellValidating
        '=================================================================================
        If (grdCustContact.CurrentCellAddress.X = DataGridViewComboBoxColumn4.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn4.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn4.Items.Add(e.FormattedValue)
                DataGridViewComboBoxColumn9.Items.Add(e.FormattedValue)
                grdCustContact.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If
    End Sub

    Private Sub grdOrdEntry_CustContact_CellValidating(sender As Object,
                                                       e As DataGridViewCellValidatingEventArgs) _
                                                       Handles grdOrdEntry_CustContact.CellValidating
        '==============================================================================================
        If (grdOrdEntry_CustContact.CurrentCellAddress.X = DataGridViewComboBoxColumn9.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn9.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn9.Items.Add(e.FormattedValue)
                DataGridViewComboBoxColumn4.Items.Add(e.FormattedValue)
                grdOrdEntry_CustContact.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If
    End Sub

    Private Sub grdOrdEntry_CustContact_EditingControlShowing(sender As Object,
                                                              e As DataGridViewEditingControlShowingEventArgs) _
                                                              Handles grdOrdEntry_CustContact.EditingControlShowing
        '===========================================================================================================
        If (grdOrdEntry_CustContact.CurrentCellAddress.X = DataGridViewComboBoxColumn9.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdPurchase_Mat_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                               Handles grdPurchase_Mat.CellValidating
        '====================================================================================================
        If (grdPurchase_Mat.CurrentCellAddress.X = Column34.DisplayIndex) Then
            If (Not Column34.Items.Contains(e.FormattedValue)) Then
                Column34.Items.Add(e.FormattedValue)
                grdPurchase_Mat.Rows(e.RowIndex).Cells(2).Value = e.FormattedValue
            End If
        End If

        Dim pCell As DataGridViewCell = grdPurchase_Mat.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdPurchase_Mat.EditingControl

            Select Case grdPurchase_Mat.Columns(e.ColumnIndex).HeaderText
                Case "Lead Time (wks)", "Est. Qty"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If
    End Sub

    Private Sub grdPurchase_Mat_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) _
                                                      Handles grdPurchase_Mat.EditingControlShowing
        '==================================================================================================================
        If (grdPurchase_Mat.CurrentCellAddress.X = Column34.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdCustContact_RowHeaderMouseClick(sender As Object,
                                                   e As DataGridViewCellMouseEventArgs) _
                                                   Handles grdCustContact.RowHeaderMouseClick
        '=====================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdCustContact_PreOrder = True

        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdCost_SplOperation_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                                    Handles grdCost_SplOperation.CellValidating
        '=========================================================================================================
        If (grdCost_SplOperation.CurrentCellAddress.X = DataGridViewTextBoxColumn71.DisplayIndex) Then
            If (Not DataGridViewTextBoxColumn71.Items.Contains(e.FormattedValue)) Then
                DataGridViewTextBoxColumn71.Items.Add(e.FormattedValue)
                DataGridViewComboBoxColumn8.Items.Add(e.FormattedValue)
                grdCost_SplOperation.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If

        Dim pCell As DataGridViewCell = grdCost_SplOperation.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdCost_SplOperation.EditingControl

            Select Case grdCost_SplOperation.Columns(e.ColumnIndex).HeaderText
                Case "Lead Time (wks)", "Cost ($)"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If
    End Sub

    Private Sub grdCost_SplOperation_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) _
                                                           Handles grdCost_SplOperation.EditingControlShowing
        '========================================================================================================================
        If (grdCost_SplOperation.CurrentCellAddress.X = DataGridViewTextBoxColumn71.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdQuality_SplOperation_CellValidating(sender As Object,
                                                       e As DataGridViewCellValidatingEventArgs) _
                                                       Handles grdQuality_SplOperation.CellValidating
        '=============================================================================================
        If (grdQuality_SplOperation.CurrentCellAddress.X = DataGridViewComboBoxColumn8.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn8.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn8.Items.Add(e.FormattedValue)
                DataGridViewTextBoxColumn71.Items.Add(e.FormattedValue)
                grdQuality_SplOperation.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If

        Dim pCell As DataGridViewCell = grdQuality_SplOperation.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdQuality_SplOperation.EditingControl

            Select Case grdQuality_SplOperation.Columns(e.ColumnIndex).HeaderText
                Case "Lead Time (wks)", "Cost ($)"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If
    End Sub

    Private Sub grdQuality_SplOperation_EditingControlShowing(sender As Object,
                                                              e As DataGridViewEditingControlShowingEventArgs) _
                                                              Handles grdQuality_SplOperation.EditingControlShowing
        '===========================================================================================================
        If (grdQuality_SplOperation.CurrentCellAddress.X = DataGridViewComboBoxColumn8.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdApp_Face_Cavity_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                                  Handles grdApp_Face_Cavity.CellValidating
        '========================================================================================================
        If (grdApp_Face_Cavity.CurrentCellAddress.X = DataGridViewComboBoxColumn5.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn5.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn5.Items.Add(e.FormattedValue)
                grdApp_Face_Cavity.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If

        Dim pCell As DataGridViewCell = grdApp_Face_Cavity.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdApp_Face_Cavity.EditingControl

            Select Case grdApp_Face_Cavity.Columns(e.ColumnIndex).HeaderText
                Case "Assy. Min", "Assy. Max", "Oper. Min", "Oper. Max"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdApp_Face_Cavity_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) _
                                                         Handles grdApp_Face_Cavity.EditingControlShowing
        '====================================================================================================================
        If (grdApp_Face_Cavity.CurrentCellAddress.X = DataGridViewComboBoxColumn5.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdDesign_Verification_CellValidating(sender As Object,
                                                      e As DataGridViewCellValidatingEventArgs) _
                                                      Handles grdDesign_Verification.CellValidating
        '===========================================================================================
        If (grdDesign_Verification.CurrentCellAddress.X = DataGridViewComboBoxColumn10.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn10.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn10.Items.Add(e.FormattedValue)
                grdDesign_Verification.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If

    End Sub

    Private Sub grdDesign_Verification_EditingControlShowing(sender As Object,
                                                             e As DataGridViewEditingControlShowingEventArgs) _
                                                             Handles grdDesign_Verification.EditingControlShowing
        '==========================================================================================================
        If (grdDesign_Verification.CurrentCellAddress.X = DataGridViewComboBoxColumn10.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdDesign_Input_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                               Handles grdDesign_Input.CellValidating
        '=====================================================================================================
        If (grdDesign_Input.CurrentCellAddress.X = DataGridViewComboBoxColumn7.DisplayIndex) Then
            If (Not DataGridViewComboBoxColumn7.Items.Contains(e.FormattedValue)) Then
                DataGridViewComboBoxColumn7.Items.Add(e.FormattedValue)
                'grdDesign_Input.Rows.Add()
                grdDesign_Input.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
                'If (e.RowIndex = grdDesign_Input.Rows.Count-1) Then

                'End If
            End If
        End If
    End Sub

    Private Sub grdDesign_Input_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) _
                                                      Handles grdDesign_Input.EditingControlShowing
        '=================================================================================================================
        If (grdDesign_Input.CurrentCellAddress.X = DataGridViewComboBoxColumn7.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdDesign_CustSpec_CellValidating(sender As Object,
                                                  e As DataGridViewCellValidatingEventArgs) _
                                                  Handles grdDesign_CustSpec.CellValidating
        '=====================================================================================
        If (grdDesign_CustSpec.CurrentCellAddress.X = DataGridViewTextBoxColumn4.DisplayIndex) Then
            If (Not DataGridViewTextBoxColumn4.Items.Contains(e.FormattedValue)) Then
                DataGridViewTextBoxColumn4.Items.Add(e.FormattedValue)
                grdDesign_CustSpec.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If
    End Sub

    Private Sub grdDesign_CustSpec_EditingControlShowing(sender As Object,
                                                         e As DataGridViewEditingControlShowingEventArgs) _
                                                         Handles grdDesign_CustSpec.EditingControlShowing
        '==================================================================================================
        If (grdDesign_CustSpec.CurrentCellAddress.X = DataGridViewTextBoxColumn4.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdDesign_Seal_CellValidating(sender As Object,
                                              e As DataGridViewCellValidatingEventArgs) _
                                              Handles grdDesign_Seal.CellValidating
        '================================================================================
        If (grdDesign_Seal.CurrentCellAddress.X = DataGridViewTextBoxColumn18.DisplayIndex) Then
            If (Not DataGridViewTextBoxColumn18.Items.Contains(e.FormattedValue)) Then
                DataGridViewTextBoxColumn18.Items.Add(e.FormattedValue)
                grdDesign_Seal.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
            End If
        End If

        Dim pCell As DataGridViewCell = grdDesign_Seal.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdDesign_Seal.EditingControl

            Select Case grdDesign_Seal.Columns(e.ColumnIndex).HeaderText
                Case "Min", "Nom", "Max"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If
    End Sub

    Private Sub grdDesign_Seal_EditingControlShowing(sender As Object,
                                                     e As DataGridViewEditingControlShowingEventArgs) _
                                                     Handles grdDesign_Seal.EditingControlShowing
        '===============================================================================================
        If (grdDesign_Seal.CurrentCellAddress.X = DataGridViewTextBoxColumn18.DisplayIndex) Then
            Dim pCmbBox As ComboBox = e.Control

            If (Not IsNothing(pCmbBox)) Then
                pCmbBox.DropDownStyle = ComboBoxStyle.DropDown
            End If
        End If
    End Sub

    Private Sub grdQuote_RowHeaderMouseClick(sender As Object,
                                             e As DataGridViewCellMouseEventArgs) Handles grdQuote.RowHeaderMouseClick
        '===============================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdQuote_PreOrder = True

        mblngrdCustContact_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdPreOrder_SalesData_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                          Handles grdPreOrder_SalesData.RowHeaderMouseClick
        '=========================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdForecast_PreOrder = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdOrdEntry_CustContact_RowHeaderMouseClick(sender As Object,
                                                            e As DataGridViewCellMouseEventArgs) _
                                                            Handles grdOrdEntry_CustContact.RowHeaderMouseClick
        '======================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdCustContact_OrdEntry = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdCost_SplOperation_RowHeaderMouseClick(sender As Object,
                                                         e As DataGridViewCellMouseEventArgs) _
                                                         Handles grdCost_SplOperation.RowHeaderMouseClick
        '===================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdSplOperation_Cost = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdApp_Face_Cavity_RowHeaderMouseClick(sender As Object,
                                                       e As DataGridViewCellMouseEventArgs) _
                                                       Handles grdApp_Face_Cavity.RowHeaderMouseClick
        '============================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdCavityFace_App = True


        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdApp_Axial_Cavity_RowHeaderMouseClick(sender As Object,
                                                        e As DataGridViewCellMouseEventArgs) _
                                                        Handles grdApp_Axial_Cavity.RowHeaderMouseClick
        '===============================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdCavityAxial_App = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdDesign_Verification_RowHeaderMouseClick(sender As Object,
                                                           e As DataGridViewCellMouseEventArgs) _
                                                           Handles grdDesign_Verification.RowHeaderMouseClick
        '=====================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdDesignVerfication_Design = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdDesign_Input_RowHeaderMouseClick(sender As Object,
                                                    e As DataGridViewCellMouseEventArgs) _
                                                    Handles grdDesign_Input.RowHeaderMouseClick
        '=======================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdInput_Design = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdDesign_CustSpec_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                       Handles grdDesign_CustSpec.RowHeaderMouseClick
        '======================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdCustSpec_Design = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False

    End Sub

    Private Sub grdDesign_Seal_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                   Handles grdDesign_Seal.RowHeaderMouseClick
        '===================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdSealDim_Design = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False

    End Sub

    Private Sub grdManf_ToolNGage_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                      Handles grdManf_ToolNGage.RowHeaderMouseClick
        '======================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdToolNGag_Manf = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdPurchase_Mat_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                    Handles grdPurchase_Mat.RowHeaderMouseClick
        '===================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdMat_Purchasing = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdPurchase_Drawing_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                        Handles grdPurchase_Drawing.RowHeaderMouseClick
        '=======================================================================================================
        cmdDel_Rec.Enabled = True
        mbkngrdDWG_Purchasing = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdQuality_SplOperation_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                            Handles grdQuality_SplOperation.RowHeaderMouseClick
        '============================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdSplOperation_Qlty = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdDrawing_Needed_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                      Handles grdDrawing_Needed.RowHeaderMouseClick
        '=====================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdNeeded_DWG = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdBOM_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdDrawing_BOM_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                   Handles grdDrawing_BOM.RowHeaderMouseClick
        '===================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdBOM_DWG = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdIssueComment = False
    End Sub

    Private Sub grdIssueComment_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
                                                    Handles grdIssueComment.RowHeaderMouseClick
        '====================================================================================================
        cmdDel_Rec.Enabled = True
        mblngrdIssueComment = True

        mblngrdCustContact_PreOrder = False
        mblngrdQuote_PreOrder = False
        mblngrdForecast_PreOrder = False
        mblngrdCustContact_OrdEntry = False
        mblngrdSplOperation_Cost = False
        mblngrdCavityFace_App = False
        mblngrdCavityAxial_App = False
        mblngrdDesignVerfication_Design = False
        mblngrdInput_Design = False
        mblngrdCustSpec_Design = False
        mblngrdSealDim_Design = False
        mblngrdToolNGag_Manf = False
        mblngrdMat_Purchasing = False
        mbkngrdDWG_Purchasing = False
        mblngrdSplOperation_Qlty = False
        mblngrdNeeded_DWG = False
        mblngrdBOM_DWG = False

    End Sub

    Private Sub grdCustContact_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles grdCustContact.DataError
        '=======================================================================================================================
        If (e.Exception.Message = "DataGridViewComboBoxCell value is not valid.") Then
            Dim pVal As Object = grdCustContact.Rows(e.RowIndex).Cells(0).Value
            If (Not DataGridViewComboBoxColumn4.Items.Contains(pVal)) Then
                DataGridViewComboBoxColumn4.Items.Add(pVal)
                e.ThrowException = False
            Else
                e.ThrowException = False
            End If
        End If

    End Sub

    Private Sub grdOrdEntry_CustContact_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles grdOrdEntry_CustContact.DataError
        '========================================================================================================================================
        If (e.Exception.Message = "DataGridViewComboBoxCell value is not valid.") Then
            Dim pVal As Object = grdOrdEntry_CustContact.Rows(e.RowIndex).Cells(0).Value
            If (Not DataGridViewComboBoxColumn9.Items.Contains(pVal)) Then
                DataGridViewComboBoxColumn9.Items.Add(pVal)
                e.ThrowException = False
            Else
                e.ThrowException = False
            End If
        End If
    End Sub

    Private Sub grdQuote_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles grdQuote.CellClick
        '======================================================================================================
        Dim pCI As New CultureInfo("en-US")
        If (e.ColumnIndex = 0) Then
            mDTP_Quote = New DateTimePicker()
            grdQuote.Controls.Add(mDTP_Quote)
            mDTP_Quote.Format = DateTimePickerFormat.Short
            Dim pRectangle As Rectangle = grdQuote.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            mDTP_Quote.Size = New Size(pRectangle.Width, pRectangle.Height)
            mDTP_Quote.Location = New Point(pRectangle.X, pRectangle.Y)
            grdQuote.CurrentCell.Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            AddHandler mDTP_Quote.CloseUp, New EventHandler(AddressOf DTP_Quote_CloseUp)
            AddHandler mDTP_Quote.ValueChanged, New EventHandler(AddressOf DTP_Quote_OnTextChange)
        End If

    End Sub

    Private Sub grdPreOrder_SalesData_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                                     Handles grdPreOrder_SalesData.CellValidating
        '=========================================================================================================
        Dim pCell As DataGridViewCell = grdPreOrder_SalesData.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdPreOrder_SalesData.EditingControl

            Select Case grdPreOrder_SalesData.Columns(e.ColumnIndex).HeaderText
                Case "Year", "Qty"
                    c.Text = CleanInputNumber(c.Text)
                Case "Price ($)", "Total ($)"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdApp_OpCond_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                            Handles grdApp_OpCond.CellValidating
        '=================================================================================================
        Dim pCell As DataGridViewCell = grdApp_OpCond.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdApp_OpCond.EditingControl

            Select Case grdApp_OpCond.Columns(e.ColumnIndex).HeaderText
                Case "Assembly", "Min", "Max", "Operating"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdApp_Load_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                            Handles grdApp_Load.CellValidating
        '===============================================================================================
        Dim pCell As DataGridViewCell = grdApp_Load.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdApp_Load.EditingControl

            Select Case grdApp_Load.Columns(e.ColumnIndex).HeaderText
                Case "Min", "Max"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If
    End Sub

    Private Sub grdPurchase_Drawing_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                                   Handles grdPurchase_Drawing.CellValidating
        '========================================================================================================
        Dim pCell As DataGridViewCell = grdPurchase_Drawing.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdPurchase_Drawing.EditingControl

            Select Case grdPurchase_Drawing.Columns(e.ColumnIndex).HeaderText
                Case "Lead Time (wks)"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdDrawing_Needed_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                                Handles grdDrawing_Needed.CellValidating
        '=====================================================================================================
        Dim pCell As DataGridViewCell = grdDrawing_Needed.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdDrawing_Needed.EditingControl

            Select Case grdDrawing_Needed.Columns(e.ColumnIndex).HeaderText
                Case "Lead Time (wks)"
                    c.Text = CleanInputNummeric(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdDrawing_BOM_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
                                              Handles grdDrawing_BOM.CellValidating
        '===================================================================================================
        Dim pCell As DataGridViewCell = grdDrawing_BOM.Item(e.ColumnIndex, e.RowIndex)

        If pCell.IsInEditMode Then
            Dim c As Control = grdDrawing_BOM.EditingControl

            Select Case grdDrawing_BOM.Columns(e.ColumnIndex).HeaderText
                Case "Qty"
                    c.Text = CleanInputNumber(c.Text)
            End Select
        End If

    End Sub

    Private Sub grdDesign_Input_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles grdDesign_Input.CellEndEdit
        '=========================================================================================================================
        If (grdDesign_Input.CurrentRow.Index = grdDesign_Input.Rows.Count - 1) Then
            Dim pVal As String = grdDesign_Input.Rows(grdDesign_Input.CurrentRow.Index).Cells(0).Value
            grdDesign_Input.Rows.Add()
            grdDesign_Input.Rows(grdDesign_Input.Rows.Count - 1).Cells(0).Value = ""
            grdDesign_Input.Rows(grdDesign_Input.CurrentRow.Index - 1).Cells(0).Value = pVal
        End If
    End Sub

    Private Sub grdPreOrderEditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                              Handles grdPreOrderEditedBy.CellClick
        '========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdPreOrderEditedBy.Rows.Count > 0) Then
            If (pDate = grdPreOrderEditedBy.Rows(grdPreOrderEditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdPreOrderEditedBy.Rows(grdPreOrderEditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdPreOrderEditedBy.Rows(grdPreOrderEditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdExport_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                             Handles grdExport_EditedBy.CellClick
        '========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdExport_EditedBy.Rows.Count > 0) Then
            If (pDate = grdExport_EditedBy.Rows(grdExport_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdExport_EditedBy.Rows(grdExport_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdExport_EditedBy.Rows(grdExport_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If

    End Sub

    Private Sub grdOrdEntry_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                               Handles grdOrdEntry_EditedBy.CellClick
        '==========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdOrdEntry_EditedBy.Rows.Count > 0) Then
            If (pDate = grdOrdEntry_EditedBy.Rows(grdOrdEntry_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdOrdEntry_EditedBy.Rows(grdOrdEntry_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdOrdEntry_EditedBy.Rows(grdOrdEntry_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdCost_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                           Handles grdCost_EditedBy.CellClick
        '======================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdCost_EditedBy.Rows.Count > 0) Then
            If (pDate = grdCost_EditedBy.Rows(grdCost_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdCost_EditedBy.Rows(grdCost_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdCost_EditedBy.Rows(grdCost_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdApp_EditedBy_Face_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                               Handles grdApp_EditedBy_Face.CellClick
        '===========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdApp_EditedBy_Face.Rows.Count > 0) Then
            If (pDate = grdApp_EditedBy_Face.Rows(grdApp_EditedBy_Face.Rows.Count - 1).Cells(0).Value And
                pUserName = grdApp_EditedBy_Face.Rows(grdApp_EditedBy_Face.Rows.Count - 1).Cells(1).Value) Then

                grdApp_EditedBy_Face.Rows(grdApp_EditedBy_Face.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdApp_EditedBy_Axial_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                                Handles grdApp_EditedBy_Axial.CellClick
        '===========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdApp_EditedBy_Axial.Rows.Count > 0) Then
            If (pDate = grdApp_EditedBy_Axial.Rows(grdApp_EditedBy_Axial.Rows.Count - 1).Cells(0).Value And
                pUserName = grdApp_EditedBy_Axial.Rows(grdApp_EditedBy_Axial.Rows.Count - 1).Cells(1).Value) Then

                grdApp_EditedBy_Axial.Rows(grdApp_EditedBy_Axial.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdDesign_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                             Handles grdDesign_EditedBy.CellClick
        '========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdDesign_EditedBy.Rows.Count > 0) Then
            If (pDate = grdDesign_EditedBy.Rows(grdDesign_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdDesign_EditedBy.Rows(grdDesign_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdDesign_EditedBy.Rows(grdDesign_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdManf_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                           Handles grdManf_EditedBy.CellClick
        '======================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdManf_EditedBy.Rows.Count > 0) Then
            If (pDate = grdManf_EditedBy.Rows(grdManf_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdManf_EditedBy.Rows(grdManf_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdManf_EditedBy.Rows(grdManf_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If

    End Sub

    Private Sub grdPurchase_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                               Handles grdPurchase_EditedBy.CellClick
        '==========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdPurchase_EditedBy.Rows.Count > 0) Then
            If (pDate = grdPurchase_EditedBy.Rows(grdPurchase_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdPurchase_EditedBy.Rows(grdPurchase_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdPurchase_EditedBy.Rows(grdPurchase_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If

    End Sub

    Private Sub grdQuality_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                              Handles grdQuality_EditedBy.CellClick
        '==========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdQuality_EditedBy.Rows.Count > 0) Then
            If (pDate = grdQuality_EditedBy.Rows(grdQuality_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdQuality_EditedBy.Rows(grdQuality_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdQuality_EditedBy.Rows(grdQuality_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If

    End Sub

    Private Sub grdDwg_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                          Handles grdDwg_EditedBy.CellClick
        '======================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdDwg_EditedBy.Rows.Count > 0) Then
            If (pDate = grdDwg_EditedBy.Rows(grdDwg_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdDwg_EditedBy.Rows(grdDwg_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdDwg_EditedBy.Rows(grdDwg_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdTest_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                           Handles grdTest_EditedBy.CellClick
        '=======================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdTest_EditedBy.Rows.Count > 0) Then
            If (pDate = grdTest_EditedBy.Rows(grdTest_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdTest_EditedBy.Rows(grdTest_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdTest_EditedBy.Rows(grdTest_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If
    End Sub

    Private Sub grdShipping_EditedBy_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
                                               Handles grdShipping_EditedBy.CellClick
        '===========================================================================================
        Dim pCI As New CultureInfo("en-US")
        Dim pDate As String = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName

        If (grdShipping_EditedBy.Rows.Count > 0) Then
            If (pDate = grdShipping_EditedBy.Rows(grdShipping_EditedBy.Rows.Count - 1).Cells(0).Value And
                pUserName = grdShipping_EditedBy.Rows(grdShipping_EditedBy.Rows.Count - 1).Cells(1).Value) Then

                grdShipping_EditedBy.Rows(grdShipping_EditedBy.Rows.Count - 1).Cells(2).ReadOnly = False
            End If
        End If

    End Sub


#Region "HELPER ROUTINE"

    Private Sub ComboBox_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '====================================================================================================
        Dim combo As ComboBox = CType(sender, ComboBox)

        'AES 02APR18
        Dim pUserID As Integer = 0
        Dim pUserName As String = gUser.FirstName + " " + gUser.LastName
        If (combo.SelectedItem.ToString() <> pUserName) Then
            combo.SelectedItem = pUserName
        End If

        For i As Integer = 0 To mUserName.Count - 1
            If (mUserName(i) = combo.SelectedItem.ToString()) Then
                pUserID = mUserID(i)
                Exit For
            End If
        Next

        If (pUserID <> 0) Then
            Dim pSealSuiteEntities As New SealSuiteDBEntities()
            Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser
                            Where pRec.fldID = pUserID Select pRec).ToList()

            If (pQryUser.Count > 0) Then
                Dim pTitleID As Integer = pQryUser(0).fldTitleID

                Dim pQryTitle = (From pRec In pSealSuiteEntities.tblTitle
                                 Where pRec.fldID = pTitleID Select pRec).ToList()

                If (pQryTitle.Count > 0) Then
                    grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentCell.RowIndex).Cells(2).Value = pQryTitle(0).fldTitle
                End If

            End If
        End If

    End Sub

    Private Sub ComboBoxManf_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '====================================================================================================
        Try

            Dim combo As ComboBox = CType(sender, ComboBox)

            If (grdManf_ToolNGage.CurrentCellAddress.X = DataGridViewTextBoxColumn12.DisplayIndex) Then

                If ("Roll tooling" = combo.SelectedItem.ToString()) Then

                    Dim dgvcc As New DataGridViewComboBoxCell

                    dgvcc.Items.Clear()
                    dgvcc.Items.Add("E-Seal form")
                    dgvcc.Items.Add("Pre-form")
                    dgvcc.Items.Add("C-Ring")
                    grdManf_ToolNGage.Item(2, grdManf_ToolNGage.CurrentRow.Index) = dgvcc

                ElseIf ("Die" = combo.SelectedItem.ToString()) Then

                    Dim dgvcc As New DataGridViewComboBoxCell
                    dgvcc.Items.Clear()
                    dgvcc.Items.Add("Std")
                    dgvcc.Items.Add("Pre-form PF")
                    dgvcc.Items.Add("After-plate AP")
                    grdManf_ToolNGage.Item(2, grdManf_ToolNGage.CurrentRow.Index) = dgvcc

                ElseIf ("Window gauge" = combo.SelectedItem.ToString()) Then

                    Dim dgvcc As New DataGridViewComboBoxCell
                    dgvcc.Items.Clear()
                    dgvcc.Items.Add("Std")
                    dgvcc.Items.Add("3D gauge")
                    dgvcc.Items.Add("3D tooling")
                    grdManf_ToolNGage.Item(2, grdManf_ToolNGage.CurrentRow.Index) = dgvcc

                Else
                    Dim dgvcc As New DataGridViewComboBoxCell
                    dgvcc.Items.Clear()
                    grdManf_ToolNGage.Item(2, grdManf_ToolNGage.CurrentRow.Index) = dgvcc

                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub mDateTimePicker_CloseUp(ByVal sender As Object, ByVal e As EventArgs)
        '=============================================================================
        mDTP_IssueComment.Visible = False
    End Sub

    Private Sub mDateTimePicker_OnTextChange(ByVal sender As Object, ByVal e As EventArgs)
        '==================================================================================
        grdIssueComment.CurrentCell.Value = mDTP_IssueComment.Text.ToString()
    End Sub

    Private Sub DTP_Quote_CloseUp(ByVal sender As Object, ByVal e As EventArgs)
        '=============================================================================
        mDTP_Quote.Visible = False
    End Sub

    Private Sub DTP_Quote_OnTextChange(ByVal sender As Object, ByVal e As EventArgs)
        '==================================================================================
        grdQuote.CurrentCell.Value = mDTP_Quote.Text.ToString()
    End Sub

#End Region

#End Region

#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdSetUnits_Click(sender As System.Object, e As System.EventArgs) Handles cmdSetUnits.Click
        '==================================================================================================
        SaveData()
        Dim pfrmProcess_Unit As New Process_frmUnit()
        pfrmProcess_Unit.ShowDialog()
    End Sub

    Private Sub cmdNotes_Click(sender As System.Object, e As System.EventArgs) Handles cmdNotes.Click
        '=============================================================================================
        'Dim pfrmNotes As New Process_frmNotes()
        'pfrmNotes.ShowDialog()
    End Sub

    Private Sub cmdApproval_Sign_Click(sender As System.Object, e As System.EventArgs) Handles cmdApproval_Sign.Click
        '============================================================================================================
        'AES 02APR18
        Dim pUserName As String = ""
        pUserName = gUser.FirstName + " " + gUser.LastName
        If (gUser.Role = grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(0).Value And
            pUserName = grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(1).Value) Then

            If (grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(3).Value = True) Then
                grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(3).Value = False
                grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(4).Value = ""
            Else
                grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(3).Value = True
                Dim pCI As New CultureInfo("en-US")
                grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(4).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            End If
        End If

    End Sub

    Private Sub cmdSelectNote_Click(sender As System.Object, e As System.EventArgs)
        '======================================================================================================
        Try
            ''If (lstPlanning_Notes_Dim.SelectedIndex > -1) Then
            ''    Dim pIndex As Integer = lstPlanning_Notes_Dim.SelectedIndex
            ''    Dim pName As String = lstPlanning_Notes_Dim.SelectedItem.ToString()
            ''    lstPlanning_Notes_Dim.Items.RemoveAt(pIndex)
            ''    grdPlanning_Ordered.Rows.Add()
            ''    grdPlanning_Ordered.Rows(grdPlanning_Ordered.Rows.Count - 1).Cells(0).Value = pName
            ''End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub cmdSelectAllNote_Click(sender As System.Object, e As System.EventArgs)
        '=============================================================================================================
        Try
            ''If (lstPlanning_Notes_Dim.Items.Count > 0) Then
            ''    For i As Integer = 0 To lstPlanning_Notes_Dim.Items.Count - 1
            ''        Dim pName As String = lstPlanning_Notes_Dim.Items(i)
            ''        grdPlanning_Ordered.Rows.Add()
            ''        grdPlanning_Ordered.Rows(grdPlanning_Ordered.Rows.Count - 1).Cells(0).Value = pName
            ''    Next
            ''    lstPlanning_Notes_Dim.Items.Clear()
            ''End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub cmdResetSelectedNote_Click(sender As System.Object, e As System.EventArgs)
        '===================================================================================================================
        Try
            ''Dim pCurRowIndex As Integer = grdPlanning_Ordered.CurrentRow.Index
            ''Dim pName As String = grdPlanning_Ordered.Rows(pCurRowIndex).Cells(0).Value
            ''grdPlanning_Ordered.Rows.RemoveAt(pCurRowIndex)
            ''lstPlanning_Notes_Dim.Items.Add(pName)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub cmdResetSelectedAllNote_Click(sender As System.Object, e As System.EventArgs)
        '====================================================================================
        Try
            ''For i As Integer = 0 To grdPlanning_Ordered.Rows.Count - 1
            ''    Dim pName As String = grdPlanning_Ordered.Rows(i).Cells(0).Value
            ''    lstPlanning_Notes_Dim.Items.Add(pName)
            ''Next
            ''grdPlanning_Ordered.Rows.Clear()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub cmdSealPart_Click(sender As Object, e As EventArgs)
        '====================================================================================
        SaveData()
        SaveToDB()
        Me.Close()
    End Sub

    Private Sub cmdIssueComment_Click(sender As Object, e As EventArgs) Handles cmdIssueComment.Click
        '=============================================================================================
        SaveData()
        Dim pProcess_frmIssueComnt As New Process_frmIssueComnt(mProcess_Project)
        pProcess_frmIssueComnt.ShowDialog()
    End Sub

    Private Sub cmdCreatePDS_Click(sender As System.Object, e As System.EventArgs) Handles cmdCreatePDS.Click
        '=======================================================================================================
        SaveData()

        Cursor = Cursors.WaitCursor

        'mProcess_Project.Handle_PDSFile("Write", gProcessFile.PDSMappingFile)
        gProcessFile.Handle_PDSFile("Write", mProcess_Project, gPartProject)
        Cursor = Cursors.Default
        'Dim pProcess_PDSMapping As New Process_frmPDS
        'pProcess_PDSMapping.ShowDialog()

    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        SaveData()
        SaveToDB()
        MessageBox.Show("Data Saved Succecfully.", "Save Record", MessageBoxButtons.OK)
        Me.Close()

    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub

    Private Sub cmdDel_Rec_Click(sender As Object, e As EventArgs) Handles cmdDel_Rec.Click
        '==================================================================================

        If (mblngrdCustContact_PreOrder) Then
            Delete_Record(grdCustContact, grdCustContact.CurrentRow.Index)
            mblngrdCustContact_PreOrder = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdQuote_PreOrder) Then
            Delete_Record(grdQuote, grdQuote.CurrentRow.Index)
            mblngrdQuote_PreOrder = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdForecast_PreOrder) Then
            Delete_Record(grdPreOrder_SalesData, grdPreOrder_SalesData.CurrentRow.Index)
            mblngrdForecast_PreOrder = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdCustContact_OrdEntry) Then
            Delete_Record(grdOrdEntry_CustContact, grdOrdEntry_CustContact.CurrentRow.Index)
            mblngrdCustContact_OrdEntry = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdSplOperation_Cost) Then
            Delete_Record(grdCost_SplOperation, grdCost_SplOperation.CurrentRow.Index)
            mblngrdSplOperation_Cost = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdCavityFace_App) Then
            Delete_Record(grdApp_Face_Cavity, grdApp_Face_Cavity.CurrentRow.Index)
            mblngrdCavityFace_App = False
            cmdDel_Rec.Enabled = False


        ElseIf (mblngrdCavityAxial_App) Then
            Delete_Record(grdApp_Axial_Cavity, grdApp_Axial_Cavity.CurrentRow.Index)
            mblngrdCavityAxial_App = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdDesignVerfication_Design) Then
            Delete_Record(grdDesign_Verification, grdDesign_Verification.CurrentRow.Index)
            mblngrdDesignVerfication_Design = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdInput_Design) Then
            Delete_Record(grdDesign_Input, grdDesign_Input.CurrentRow.Index)
            mblngrdInput_Design = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdCustSpec_Design) Then
            Delete_Record(grdDesign_CustSpec, grdDesign_CustSpec.CurrentRow.Index)
            mblngrdCustSpec_Design = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdSealDim_Design) Then
            Delete_Record(grdDesign_Seal, grdDesign_Seal.CurrentRow.Index)
            mblngrdSealDim_Design = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdToolNGag_Manf) Then
            Delete_Record(grdManf_ToolNGage, grdManf_ToolNGage.CurrentRow.Index)
            mblngrdToolNGag_Manf = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdMat_Purchasing) Then
            Delete_Record(grdPurchase_Mat, grdPurchase_Mat.CurrentRow.Index)
            mblngrdMat_Purchasing = False
            cmdDel_Rec.Enabled = False

        ElseIf (mbkngrdDWG_Purchasing) Then
            Delete_Record(grdPurchase_Drawing, grdPurchase_Drawing.CurrentRow.Index)
            mbkngrdDWG_Purchasing = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdSplOperation_Qlty) Then
            Delete_Record(grdQuality_SplOperation, grdQuality_SplOperation.CurrentRow.Index)
            mblngrdSplOperation_Qlty = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdNeeded_DWG) Then
            Delete_Record(grdDrawing_Needed, grdDrawing_Needed.CurrentRow.Index)
            mblngrdNeeded_DWG = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdBOM_DWG) Then
            Delete_Record(grdDrawing_BOM, grdDrawing_BOM.CurrentRow.Index)
            mblngrdBOM_DWG = False
            cmdDel_Rec.Enabled = False

        ElseIf (mblngrdIssueComment) Then
            Delete_Record(grdIssueComment, grdIssueComment.CurrentRow.Index)
            mblngrdIssueComment = False
            cmdDel_Rec.Enabled = False
        End If

    End Sub

    Private Sub cmdRiskAna_Click(sender As Object, e As EventArgs) Handles cmdRiskAna.Click
        '==================================================================================
        SaveData()

        Dim pTabName As String = ""
        Dim pTabIndex As Integer = TabControl1.SelectedIndex
        If (pTabIndex = 0) Then
            pTabName = "PreOrder"
        ElseIf (pTabIndex = 1) Then
            pTabName = "Export"
        ElseIf (pTabIndex = 2) Then
            pTabName = "OrdEntry"
        ElseIf (pTabIndex = 3) Then
            pTabName = "Cost"
        ElseIf (pTabIndex = 4) Then
            pTabName = "App"
        ElseIf (pTabIndex = 5) Then
            pTabName = "Design"
        ElseIf (pTabIndex = 6) Then
            pTabName = "Manf"
        ElseIf (pTabIndex = 7) Then
            pTabName = "Purchase"
        ElseIf (pTabIndex = 8) Then
            pTabName = "Qlty"
        ElseIf (pTabIndex = 9) Then
            pTabName = "Dwg"
        ElseIf (pTabIndex = 10) Then
            pTabName = "Test"
        ElseIf (pTabIndex = 11) Then
            pTabName = "Planning"
        ElseIf (pTabIndex = 12) Then
            pTabName = "Shipping"
        End If

        Dim pProcess_frmRiskAna As New Process_frmRiskAnalysis(mProcess_Project, pTabName)
        pProcess_frmRiskAna.ShowDialog()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '==================
        Try

            Dim pCI As New CultureInfo("en-US")

            optApp_Cust_Gen.Checked = True
            optDesign_Cust.Checked = True
            optTest_Cust.Checked = True

            '....Header
            If (CompareVal_Header()) Then
                txtDateMod.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                txtModifiedBy.Text = gUser.FirstName + " " + gUser.LastName
            End If

            '.... "Header:"
            With mProcess_Project
                .POPCoding = cmbPopCoding.Text

                .Rating = cmbRating.Text
                .Type = cmbType.Text

                If (txtStartDate.Text <> "") Then
                    If (txtStartDate.Text <> DateTime.MinValue.ToShortDateString()) Then
                        .DateOpen = Convert.ToDateTime(txtStartDate.Text)
                    End If

                End If

                If (txtDateMod.Text <> "") Then
                    If (txtDateMod.Text <> DateTime.MinValue) Then
                        .DateLastModified = Convert.ToDateTime(txtDateMod.Text)
                    End If
                End If

                '.CloseDate = = Convert.ToDateTime(txtc.Text)
                .LastModifiedBy = txtModifiedBy.Text

            End With

            mProcess_Project.SaveToDB(mPNID, mRevID)

            If (TabControl1.SelectedIndex = 2) Then
                CopyDataGridView(grdOrdEntry_CustContact, grdCustContact)
            End If

            If (TabControl1.SelectedIndex = 8) Then
                CopyDataGridView(grdQuality_SplOperation, grdCost_SplOperation)
            End If

            ''....Header
            'If (CompareVal_Header()) Then
            '    txtDateMod.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            '    txtModifiedBy.Text = gUser.FirstName + " " + gUser.LastName
            'End If

            '....Edited By
            If (CompareVal_PreOrder()) Then
                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName
                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdPreOrderEditedBy.Rows.Add()
                            grdPreOrderEditedBy.Rows(grdPreOrderEditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdPreOrderEditedBy.Rows(grdPreOrderEditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtPreOrderUserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtPreOrderUserName.Text = pName

                        End If
                    Else
                        grdPreOrderEditedBy.Rows.Add()
                        grdPreOrderEditedBy.Rows(0).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdPreOrderEditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtPreOrderUserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtPreOrderUserName.Text = pName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdPreOrderEditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdPreOrderEditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdPreOrderEditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdPreOrderEditedBy.Rows(j).Cells(2).Value)

                    Next

                End With
                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "PreOrder")

            End If

            If (txtPreOrderUserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtPreOrderUserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtPreOrderUserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkPreOrderUserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "PreOrder")     'AES 11APR18
            End If


            If (CompareVal_Export()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName
                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdExport_EditedBy.Rows.Add()
                            grdExport_EditedBy.Rows(grdExport_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdExport_EditedBy.Rows(grdExport_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtITAR_Export_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtITAR_Export_UserName.Text = pName

                        End If
                    Else
                        grdExport_EditedBy.Rows.Add()
                        grdExport_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdExport_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtITAR_Export_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtITAR_Export_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdExport_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdExport_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdExport_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdExport_EditedBy.Rows(j).Cells(2).Value)

                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Export")
            End If

            If (txtITAR_Export_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtITAR_Export_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtITAR_Export_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkITAR_Export_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Export")     'AES 11APR18
            End If


            If (CompareVal_OrdEntry()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName
                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdOrdEntry_EditedBy.Rows.Add()
                            grdOrdEntry_EditedBy.Rows(grdOrdEntry_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdOrdEntry_EditedBy.Rows(grdOrdEntry_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtOrdEntry_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtOrdEntry_UserName.Text = pName

                        End If
                    Else
                        grdOrdEntry_EditedBy.Rows.Add()
                        grdOrdEntry_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdOrdEntry_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtOrdEntry_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtOrdEntry_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdOrdEntry_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdOrdEntry_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdOrdEntry_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdOrdEntry_EditedBy.Rows(j).Cells(2).Value)

                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "OrdEntry")

            End If

            If (txtOrdEntry_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtOrdEntry_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtOrdEntry_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkOrdEntry_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "OrdEntry")     'AES 11APR18
            End If


            If (CompareVal_CostEstimating()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName
                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdCost_EditedBy.Rows.Add()
                            grdCost_EditedBy.Rows(grdCost_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdCost_EditedBy.Rows(grdCost_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtCost_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtCost_UserName.Text = pName

                        End If
                    Else
                        grdCost_EditedBy.Rows.Add()
                        grdCost_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdCost_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtCost_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtCost_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdCost_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdCost_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdCost_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdCost_EditedBy.Rows(j).Cells(2).Value)

                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Cost")

            End If

            If (txtCost_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtCost_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtCost_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkCost_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Cost")     'AES 11APR18
            End If


            If (CompareVal_App()) Then

                If (cmbApp_InsertLoc.Text = "Face") Then
                    With mProcess_Project.EditedBy
                        Dim pDate As Date = DateTime.Now()
                        Dim pName As String = gUser.FirstName + " " + gUser.LastName
                        If (.DateEdited.Count > 0) Then
                            If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                                grdApp_EditedBy_Face.Rows.Add()
                                grdApp_EditedBy_Face.Rows(grdApp_EditedBy_Face.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                                grdApp_EditedBy_Face.Rows(grdApp_EditedBy_Face.Rows.Count - 1).Cells(1).Value = pName
                                txtApp_UserDate_Face.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                                txtApp_UserName_Face.Text = pName

                            End If
                        Else
                            grdApp_EditedBy_Face.Rows.Add()
                            grdApp_EditedBy_Face.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdApp_EditedBy_Face.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                            txtApp_UserDate_Face.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtApp_UserName_Face.Text = gUser.FirstName + " " + gUser.LastName
                        End If

                        mProcess_Project.EditedBy.ID_Edit.Clear()
                        mProcess_Project.EditedBy.DateEdited.Clear()
                        mProcess_Project.EditedBy.Name.Clear()
                        mProcess_Project.EditedBy.Comment.Clear()

                        For j As Integer = 0 To grdApp_EditedBy_Face.Rows.Count - 1
                            mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                            mProcess_Project.EditedBy.DateEdited.Add(grdApp_EditedBy_Face.Rows(j).Cells(0).Value)
                            mProcess_Project.EditedBy.Name.Add(grdApp_EditedBy_Face.Rows(j).Cells(1).Value)
                            mProcess_Project.EditedBy.Comment.Add(grdApp_EditedBy_Face.Rows(j).Cells(2).Value)
                        Next
                    End With
                    mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "App")
                Else
                    With mProcess_Project.EditedBy
                        Dim pDate As Date = DateTime.Now()
                        Dim pName As String = gUser.FirstName + " " + gUser.LastName
                        If (.DateEdited.Count > 0) Then
                            If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                                grdApp_EditedBy_Axial.Rows.Add()
                                grdApp_EditedBy_Axial.Rows(grdApp_EditedBy_Axial.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                                grdApp_EditedBy_Axial.Rows(grdApp_EditedBy_Axial.Rows.Count - 1).Cells(1).Value = pName
                                txtApp_UserDate_Axial.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                                txtApp_UserName_Axial.Text = pName

                            End If
                        Else
                            grdApp_EditedBy_Axial.Rows.Add()
                            grdApp_EditedBy_Axial.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdApp_EditedBy_Axial.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                            txtApp_UserDate_Axial.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtApp_UserName_Axial.Text = gUser.FirstName + " " + gUser.LastName
                        End If

                        mProcess_Project.EditedBy.ID_Edit.Clear()
                        mProcess_Project.EditedBy.DateEdited.Clear()
                        mProcess_Project.EditedBy.Name.Clear()
                        mProcess_Project.EditedBy.Comment.Clear()

                        For j As Integer = 0 To grdApp_EditedBy_Axial.Rows.Count - 1
                            mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                            mProcess_Project.EditedBy.DateEdited.Add(grdApp_EditedBy_Axial.Rows(j).Cells(0).Value)
                            mProcess_Project.EditedBy.Name.Add(grdApp_EditedBy_Axial.Rows(j).Cells(1).Value)
                            mProcess_Project.EditedBy.Comment.Add(grdApp_EditedBy_Axial.Rows(j).Cells(2).Value)
                        Next
                    End With
                    mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "App")

                End If

            End If

            If (txtApp_UserDate_Face.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtApp_UserName_Face.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtApp_UserDate_Face.Text)
                mProcess_Project.EditedBy.User_Signed = chkApp_UserSigned_Face.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "App")     'AES 11APR18
            End If


            If (CompareVal_Design()) Then

                With mProcess_Project.EditedBy
                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName
                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdDesign_EditedBy.Rows.Add()
                            grdDesign_EditedBy.Rows(grdDesign_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdDesign_EditedBy.Rows(grdDesign_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtDesign_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtDesign_UserName.Text = pName

                        End If
                    Else
                        grdDesign_EditedBy.Rows.Add()
                        grdDesign_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdDesign_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtDesign_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtDesign_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdDesign_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdDesign_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdDesign_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdDesign_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Design")
            End If

            If (txtDesign_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtDesign_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtDesign_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkDesign_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Design")     'AES 11APR18

            End If

            If (CompareVal_Manf()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName

                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdManf_EditedBy.Rows.Add()
                            grdManf_EditedBy.Rows(grdManf_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdManf_EditedBy.Rows(grdManf_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtManf_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtManf_UserName.Text = pName

                        End If
                    Else
                        grdManf_EditedBy.Rows.Add()
                        grdManf_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdManf_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtManf_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtManf_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdManf_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdManf_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdManf_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdManf_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Manf")
            End If

            If (txtManf_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtManf_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtManf_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkManf_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Manf")     'AES 11APR18
            End If


            If (CompareVal_Purchase()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName

                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdPurchase_EditedBy.Rows.Add()
                            grdPurchase_EditedBy.Rows(grdPurchase_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdPurchase_EditedBy.Rows(grdPurchase_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtPurchase_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtPurchase_UserName.Text = pName

                        End If
                    Else
                        grdPurchase_EditedBy.Rows.Add()
                        grdPurchase_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdPurchase_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtPurchase_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtPurchase_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdPurchase_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdPurchase_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdPurchase_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdPurchase_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Purchase")
            End If

            If (txtPurchase_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtPurchase_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtPurchase_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkPurchase_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Purchase")     'AES 11APR18
            End If


            If (CompareVal_Qlty()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName

                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdQuality_EditedBy.Rows.Add()
                            grdQuality_EditedBy.Rows(grdQuality_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdQuality_EditedBy.Rows(grdQuality_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtQuality_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtQuality_UserName.Text = pName

                        End If
                    Else
                        grdQuality_EditedBy.Rows.Add()
                        grdQuality_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdQuality_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtQuality_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtQuality_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdQuality_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdQuality_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdQuality_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdQuality_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Qlty")
            End If

            If (txtQuality_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtQuality_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtQuality_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkQuality_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Qlty")     'AES 11APR18
            End If


            If (CompareVal_DWG()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName

                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdDwg_EditedBy.Rows.Add()
                            grdDwg_EditedBy.Rows(grdDwg_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdDwg_EditedBy.Rows(grdDwg_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtDwg_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtDwg_UserName.Text = pName

                        End If
                    Else
                        grdDwg_EditedBy.Rows.Add()
                        grdDwg_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdDwg_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtDwg_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtDwg_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdDwg_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdDwg_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdDwg_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdDwg_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Dwg")
            End If

            If (txtDwg_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtDwg_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtDwg_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkDwg_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Dwg")     'AES 11APR18
            End If


            If (CompareVal_Test()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName

                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdTest_EditedBy.Rows.Add()
                            grdTest_EditedBy.Rows(grdTest_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdTest_EditedBy.Rows(grdTest_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtTest_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtTest_UserName.Text = pName

                        End If
                    Else
                        grdTest_EditedBy.Rows.Add()
                        grdTest_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdTest_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtTest_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtTest_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdTest_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdTest_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdTest_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdTest_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Test")
            End If

            If (txtTest_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtTest_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtTest_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkTest_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Test")     'AES 11APR18
            End If


            If (CompareVal_Shipping()) Then

                With mProcess_Project.EditedBy

                    Dim pDate As Date = DateTime.Now()
                    Dim pName As String = gUser.FirstName + " " + gUser.LastName

                    If (.DateEdited.Count > 0) Then
                        If (.DateEdited(.DateEdited.Count - 1).ToString("MM/dd/yyyy", pCI.DateTimeFormat()) <> pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat()) Or .Name(.Name.Count - 1) <> pName) Then
                            grdShipping_EditedBy.Rows.Add()
                            grdShipping_EditedBy.Rows(grdShipping_EditedBy.Rows.Count - 1).Cells(0).Value = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            grdShipping_EditedBy.Rows(grdShipping_EditedBy.Rows.Count - 1).Cells(1).Value = pName
                            txtShipping_UserDate.Text = pDate.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                            txtShipping_UserName.Text = pName

                        End If
                    Else
                        grdShipping_EditedBy.Rows.Add()
                        grdShipping_EditedBy.Rows(0).Cells(0).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        grdShipping_EditedBy.Rows(0).Cells(1).Value = gUser.FirstName + " " + gUser.LastName

                        txtShipping_UserDate.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        txtShipping_UserName.Text = gUser.FirstName + " " + gUser.LastName
                    End If

                    mProcess_Project.EditedBy.ID_Edit.Clear()
                    mProcess_Project.EditedBy.DateEdited.Clear()
                    mProcess_Project.EditedBy.Name.Clear()
                    mProcess_Project.EditedBy.Comment.Clear()

                    For j As Integer = 0 To grdShipping_EditedBy.Rows.Count - 1
                        mProcess_Project.EditedBy.ID_Edit.Add(j + 1)
                        mProcess_Project.EditedBy.DateEdited.Add(grdShipping_EditedBy.Rows(j).Cells(0).Value)
                        mProcess_Project.EditedBy.Name.Add(grdShipping_EditedBy.Rows(j).Cells(1).Value)
                        mProcess_Project.EditedBy.Comment.Add(grdShipping_EditedBy.Rows(j).Cells(2).Value)
                    Next

                End With

                mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Shipping")
            End If

            If (txtShipping_UserDate.Text <> "") Then
                mProcess_Project.EditedBy.User_Name = txtShipping_UserName.Text
                mProcess_Project.EditedBy.User_DateSigned = Convert.ToDateTime(txtShipping_UserDate.Text)
                mProcess_Project.EditedBy.User_Signed = chkShipping_UserSigned.Checked
                mProcess_Project.EditedBy.SaveToDB_UserSignOff(mProcess_Project.ID, "Shipping")     'AES 11APR18
            End If



            ''.... "Header:"
            'With mProcess_Project
            '    .POPCoding = cmbPopCoding.Text

            '    .Rating = cmbRating.Text
            '    .Type = cmbType.Text

            '    If (txtStartDate.Text <> "") Then
            '        If (txtStartDate.Text <> DateTime.MinValue) Then
            '            .DateOpen = Convert.ToDateTime(txtStartDate.Text)
            '        End If

            '    End If

            '    If (txtDateMod.Text <> "") Then
            '        If (txtDateMod.Text <> DateTime.MinValue) Then
            '            .DateLastModified = Convert.ToDateTime(txtDateMod.Text)
            '        End If
            '    End If

            '    '.CloseDate = = Convert.ToDateTime(txtc.Text)
            '    .LastModifiedBy = txtModifiedBy.Text

            'End With
            'mProcess_Project.SaveToDB(mPNID, mRevID)


            '...."Pre-Order:"
            With mProcess_Project.PreOrder
                .Mgr_PreOrder = cmbMgrPreOrder.Text
                .Mgr_Sales = txtMgrSales.Text

                If (cmbExport_Reqd.Text = "Y") Then
                    .Export_Reqd = True
                Else
                    .Export_Reqd = False
                End If

                .Export_Status = cmbExport_Status.Text

                .Part_Family = cmbPartFamily.Text
                .Part_Type = cmbPartType.Text

                .PreOrder_Seg = cmbPreOrderSeg.Text
                .PreOrder_Channel = cmbPreOrderChannel.Text
                .Notes = txtPreOrderNotes.Text

                .Loc_CostFile = cmbCostFileLoc.Text
                .Loc_RFQPkg = cmbRFQPkgLoc.Text
                .Notes_Price = txtPreOrderPriceNotes.Text

                If (chkPreOrderUserSigned.Checked) Then
                    '.User_Name = txtPreOrderUserName.Text
                    .EditedBy.User_Name = txtPreOrderUserName.Text
                    .EditedBy.User_DateSigned = Convert.ToDateTime(txtPreOrderUserDate.Text)
                    .EditedBy.User_Signed = True

                Else
                    .EditedBy.User_Name = ""
                    .EditedBy.User_DateSigned = DateTime.MinValue
                    .EditedBy.User_Signed = False
                End If

                '....Cust Contact Pre-Order
                mProcess_Project.CustContact.ID_Cust.Clear()
                mProcess_Project.CustContact.DeptName.Clear()
                mProcess_Project.CustContact.Name.Clear()
                mProcess_Project.CustContact.Phone.Clear()
                mProcess_Project.CustContact.Email.Clear()

                'grdOrdEntry_CustContact = grdCustContact

                For j As Integer = 0 To grdCustContact.Rows.Count - 2
                    mProcess_Project.CustContact.ID_Cust.Add(j + 1)
                    mProcess_Project.CustContact.DeptName.Add(grdCustContact.Rows(j).Cells(0).Value)
                    mProcess_Project.CustContact.Name.Add(grdCustContact.Rows(j).Cells(1).Value)
                    mProcess_Project.CustContact.Phone.Add(grdCustContact.Rows(j).Cells(2).Value)
                    mProcess_Project.CustContact.Email.Add(grdCustContact.Rows(j).Cells(3).Value)
                Next

                CopyDataGridView(grdCustContact, grdOrdEntry_CustContact)
                '....Cust Contact Order-Entry
                mProcess_Project.CustContact.ID_Cust.Clear()
                mProcess_Project.CustContact.DeptName.Clear()
                mProcess_Project.CustContact.Name.Clear()
                mProcess_Project.CustContact.Phone.Clear()
                mProcess_Project.CustContact.Email.Clear()
                For j As Integer = 0 To grdOrdEntry_CustContact.Rows.Count - 2
                    mProcess_Project.CustContact.ID_Cust.Add(j + 1)
                    mProcess_Project.CustContact.DeptName.Add(grdOrdEntry_CustContact.Rows(j).Cells(0).Value)
                    mProcess_Project.CustContact.Name.Add(grdOrdEntry_CustContact.Rows(j).Cells(1).Value)
                    mProcess_Project.CustContact.Phone.Add(grdOrdEntry_CustContact.Rows(j).Cells(2).Value)
                    mProcess_Project.CustContact.Email.Add(grdOrdEntry_CustContact.Rows(j).Cells(3).Value)
                Next

                '....Quote
                .Quote.QID.Clear()
                .Quote.QDate.Clear()
                .Quote.No.Clear()

                For j As Integer = 0 To grdQuote.Rows.Count - 2
                    .Quote.QID.Add(j + 1)
                    'grdQuote.Rows(j).Cells(0).Value <> "" And 
                    If (Not IsNothing(grdQuote.Rows(j).Cells(0).Value)) Then
                        .Quote.QDate.Add(grdQuote.Rows(j).Cells(0).Value)
                    Else
                        .Quote.QDate.Add(DateTime.MinValue)
                    End If

                    .Quote.No.Add(grdQuote.Rows(j).Cells(1).Value)
                Next

                '....Sales Data
                .SalesData.ID_Sales.Clear()
                .SalesData.Year.Clear()
                .SalesData.Qty.Clear()
                .SalesData.Price.Clear()
                .SalesData.Total.Clear()

                For j As Integer = 0 To grdPreOrder_SalesData.Rows.Count - 2
                    .SalesData.ID_Sales.Add(j + 1)
                    .SalesData.Year.Add(grdPreOrder_SalesData.Rows(j).Cells(0).Value)
                    .SalesData.Qty.Add(grdPreOrder_SalesData.Rows(j).Cells(1).Value)
                    .SalesData.Price.Add(grdPreOrder_SalesData.Rows(j).Cells(2).Value)
                    .SalesData.Total.Add(grdPreOrder_SalesData.Rows(j).Cells(3).Value)
                Next

            End With


            '...."ITAR_Export:"

            With mProcess_Project.ITAR_Export
                .IsCustOnDenialList = IIf(cmbITAR_Export_CustOnDenialList.Text = "Y", True, False)
                .CountryProhibited = IIf(cmbITAR_Export_CountryProhibited.Text = "Y", True, False)
                .HasAntiBoycottLang = IIf(cmbITAR_Export_AntiBoycottLang.Text = "Y", True, False)
                .IsUnder_ITAR_Reg = IIf(cmbITAR_Export_ProductITAR_Reg.Text = "Y", True, False)
                .ITAR_Class = txtITAR_Export_ITAR_Classification.Text
                .SaleExportControlled = IIf(cmbITAR_Export_SaleExportControlled.Text = "Y", True, False)
                .EAR_Class = cmbITAR_Export_EAR_Classification.Text
                .Status = cmbITAR_Export_Status.Text
                .HTS_Class = txtITAR_Export_HTS_Classification.Text

                If (chkITAR_Export_UserSigned.Checked) Then
                    .EditedBy.User_Name = txtITAR_Export_UserName.Text
                    .EditedBy.User_DateSigned = Convert.ToDateTime(txtITAR_Export_UserDate.Text)
                    .EditedBy.User_Signed = True

                Else
                    .EditedBy.User_Name = ""
                    .EditedBy.User_DateSigned = DateTime.MinValue
                    .EditedBy.User_Signed = False
                End If
            End With


            '.... "OrderEntry:"

            With mProcess_Project.OrdEntry
                .SalesOrderNo = txtOrdEntry_SalesOrderNo.Text
                If (txtOrdEntry_SalesDate.Text <> "") Then
                    .DateSales = Convert.ToDateTime(txtOrdEntry_SalesDate.Text)
                End If

                If (txtOrderEntry_QtdLeadTime.Text <> "") Then
                    .LeadTimeQuoted = ConvertToDbl(txtOrderEntry_QtdLeadTime.Text)
                End If

                .PONo = txtOrdEntry_PONo.Text
                If (txtOrdEntry_PODate.Text <> "") Then
                    .DatePO = Convert.ToDateTime(txtOrdEntry_PODate.Text)
                End If

                If (txtOrdEntry_PODate_EDI.Text <> "") Then
                    .DatePO_EDI = Convert.ToDateTime(txtOrdEntry_PODate_EDI.Text)
                End If

                .HasSplReq = IIf(cmbOrdEntry_SpecialReq.Text = "Y", True, False)
                .Tool_Reqd = IIf(cmbOrdEntry_Tooling.Text = "Y", True, False)
                .SplPkg_Lbl_Reqd = IIf(cmbOrdEntry_SplPkgNLbl.Text = "Y", True, False)
                If (txtOrdEntry_OrderQty.Text <> "") Then
                    .OrdQty = Math.Round(ConvertToDbl(txtOrdEntry_OrderQty.Text))
                End If

                If (txtOrdEntry_OrderShipDate.Text <> "") Then
                    .DateOrdShip = Convert.ToDateTime(txtOrdEntry_OrderShipDate.Text)
                End If

                .Expedited = IIf(cmbOrdEntry_Expedited.Text = "Y", True, False)
                .IsDFAR = IIf(cmbOrdEntry_DFAR.Text = "Y", True, False)

                If (chkOrdEntry_UserSigned.Checked) Then
                    .EditedBy.User_Name = txtOrdEntry_UserName.Text
                    .EditedBy.User_DateSigned = Convert.ToDateTime(txtOrdEntry_UserDate.Text)
                    .EditedBy.User_Signed = True

                Else
                    .EditedBy.User_Name = ""
                    .EditedBy.User_DateSigned = DateTime.MinValue
                    .EditedBy.User_Signed = False
                End If

            End With


            '.... "Cost Estimating:"

            With mProcess_Project.Cost
                .QuoteFileLoc = cmbCost_QuoteFile.Text
                .Notes = txtCost_Notes.Text

                'If (grdQuality_SplOperation.Rows.Count > 1) Then
                '    grdCost_SplOperation = grdQuality_SplOperation
                'End If

                .SplOperation.ID_SplOp.Clear()
                .SplOperation.Desc.Clear()
                .SplOperation.Spec.Clear()
                .SplOperation.LeadTime.Clear()
                .SplOperation.Cost.Clear()

                For j As Integer = 0 To grdCost_SplOperation.Rows.Count - 2
                    .SplOperation.ID_SplOp.Add(j + 1)
                    .SplOperation.Desc.Add(grdCost_SplOperation.Rows(j).Cells(0).Value)
                    .SplOperation.Spec.Add(grdCost_SplOperation.Rows(j).Cells(1).Value)

                    If (Not IsNothing(grdCost_SplOperation.Rows(j).Cells(2).Value)) Then
                        If (grdCost_SplOperation.Rows(j).Cells(2).Value.ToString() <> "") Then
                            .SplOperation.LeadTime.Add(grdCost_SplOperation.Rows(j).Cells(2).Value)
                        Else
                            .SplOperation.LeadTime.Add(0)
                        End If

                    Else
                        .SplOperation.LeadTime.Add(0)
                    End If

                    If (Not IsNothing(grdCost_SplOperation.Rows(j).Cells(3).Value)) Then
                        If (grdCost_SplOperation.Rows(j).Cells(3).Value.ToString() <> "") Then
                            .SplOperation.Cost.Add(grdCost_SplOperation.Rows(j).Cells(3).Value)
                        Else
                            .SplOperation.Cost.Add(0)
                        End If

                    Else
                        .SplOperation.Cost.Add(0)
                    End If

                    '.SplOperation.LeadTime.Add(grdCost_SplOperation.Rows(j).Cells(2).Value)
                    '.SplOperation.Cost.Add(grdCost_SplOperation.Rows(j).Cells(3).Value)
                Next

                ''....Spl Operation
                'grdCost_SplOperation.Rows.Clear()
                'For j As Integer = 0 To .SplOperation.Desc.Count - 1
                '    grdCost_SplOperation.Rows.Add()
                '    grdCost_SplOperation.Rows(j).Cells(0).Value = .SplOperation.Desc(j)
                '    grdCost_SplOperation.Rows(j).Cells(1).Value = .SplOperation.Spec(j)
                '    If (Math.Abs(.SplOperation.LeadTime(j)) > gcEPS) Then
                '        grdCost_SplOperation.Rows(j).Cells(2).Value = .SplOperation.LeadTime(j)
                '    Else
                '        grdCost_SplOperation.Rows(j).Cells(2).Value = ""
                '    End If

                '    If (Math.Abs(.SplOperation.Cost(j)) > gcEPS) Then
                '        grdCost_SplOperation.Rows(j).Cells(3).Value = .SplOperation.Cost(j).ToString("#.00")
                '    Else
                '        grdCost_SplOperation.Rows(j).Cells(3).Value = ""
                '    End If
                'Next
                'grdCost_SplOperation.Refresh()
                'TabControl1.Refresh()

                If (chkCost_UserSigned.Checked) Then
                    .EditedBy.User_Name = txtCost_UserName.Text
                    .EditedBy.User_DateSigned = Convert.ToDateTime(txtCost_UserDate.Text)
                    .EditedBy.User_Signed = True

                Else
                    .EditedBy.User_Name = ""
                    .EditedBy.User_DateSigned = DateTime.MinValue
                    .EditedBy.User_Signed = False
                End If

            End With


            '.... "Application:"

            With mProcess_Project.App
                .Eqp = txtApp_Equip.Text
                .ExistingSeal = txtApp_ExistingSeal.Text
                .Type = cmbApp_InsertLoc.Text

                .Fluid = txtApp_Fluid.Text
                If (txtApp_MaxLeak.Text <> "") Then
                    .MaxLeak = ConvertToDbl(txtApp_MaxLeak.Text)
                End If


                .IsPressCyclic = IIf(cmbApp_PressCycle.Text = "Y", True, False)

                If (.IsPressCyclic) Then
                    If (txtApp_PressCycleFreq.Text <> "") Then
                        .PressCycle_Freq = ConvertToDbl(txtApp_PressCycleFreq.Text)
                    End If

                    If (txtApp_PressCycleAmp.Text <> "") Then
                        .PressCycle_Amp = ConvertToDbl(txtApp_PressCycleAmp.Text)
                    End If
                Else
                    .PressCycle_Freq = 0
                    .PressCycle_Amp = 0
                End If

                .Shaped = IIf(cmbApp_Shaped.Text = "Y", True, False)
                .IsOoR = IIf(cmbApp_OutOfRound.Text = "Y", True, False)
                .IsSplitRing = IIf(cmbApp_SplitRing.Text = "Y", True, False)
                .IsPreComp = IIf(cmbApp_PreComp.Text = "Y", True, False)

                If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(1).Value) And grdApp_OpCond.Rows(0).Cells(1).Value <> "") Then
                    .OpCond.T_Assy = ConvertToDbl(grdApp_OpCond.Rows(0).Cells(1).Value)
                Else
                    .OpCond.T_Assy = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(2).Value) And grdApp_OpCond.Rows(0).Cells(2).Value <> "") Then
                    .OpCond.T_Min = ConvertToDbl(grdApp_OpCond.Rows(0).Cells(2).Value)
                Else
                    .OpCond.T_Min = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(3).Value) And grdApp_OpCond.Rows(0).Cells(3).Value <> "") Then
                    .OpCond.T_Max = ConvertToDbl(grdApp_OpCond.Rows(0).Cells(3).Value)
                Else
                    .OpCond.T_Max = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(4).Value) And grdApp_OpCond.Rows(0).Cells(4).Value <> "") Then
                    .OpCond.T_Oper = ConvertToDbl(grdApp_OpCond.Rows(0).Cells(4).Value)
                Else
                    .OpCond.T_Oper = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(1).Value) And grdApp_OpCond.Rows(1).Cells(1).Value <> "") Then
                    .OpCond.Press_Assy = ConvertToDbl(grdApp_OpCond.Rows(1).Cells(1).Value)
                Else
                    .OpCond.Press_Assy = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(2).Value) And grdApp_OpCond.Rows(1).Cells(2).Value <> "") Then
                    .OpCond.Press_Min = ConvertToDbl(grdApp_OpCond.Rows(1).Cells(2).Value)
                Else
                    .OpCond.Press_Min = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(3).Value) And grdApp_OpCond.Rows(1).Cells(3).Value <> "") Then
                    .OpCond.Press_Max = ConvertToDbl(grdApp_OpCond.Rows(1).Cells(3).Value)
                Else
                    .OpCond.Press_Max = 0.0
                End If

                If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(4).Value) And grdApp_OpCond.Rows(1).Cells(4).Value <> "") Then
                    .OpCond.Press_Oper = ConvertToDbl(grdApp_OpCond.Rows(1).Cells(4).Value)
                Else
                    .OpCond.Press_Oper = 0.0
                End If

                If (Not IsNothing(grdApp_Load.Rows(0).Cells(1).Value) And grdApp_Load.Rows(0).Cells(1).Value <> "") Then
                    .Load.Assy_Min = ConvertToDbl(grdApp_Load.Rows(0).Cells(1).Value)
                Else
                    .Load.Assy_Min = 0.0
                End If

                If (Not IsNothing(grdApp_Load.Rows(0).Cells(2).Value) And grdApp_Load.Rows(0).Cells(2).Value <> "") Then
                    .Load.Assy_Max = ConvertToDbl(grdApp_Load.Rows(0).Cells(2).Value)
                Else
                    .Load.Assy_Max = 0.0
                End If

                If (Not IsNothing(grdApp_Load.Rows(1).Cells(1).Value) And grdApp_Load.Rows(1).Cells(1).Value <> "") Then
                    .Load.Oper_Min = ConvertToDbl(grdApp_Load.Rows(1).Cells(1).Value)
                Else
                    .Load.Oper_Min = 0.0
                End If

                If (Not IsNothing(grdApp_Load.Rows(0).Cells(2).Value) And grdApp_Load.Rows(0).Cells(2).Value <> "") Then
                    .Load.Oper_Max = ConvertToDbl(grdApp_Load.Rows(1).Cells(2).Value)
                Else
                    .Load.Oper_Max = 0.0
                End If

                If (.Type = "Face") Then
                    '....Face
                    .Cavity.ID_Cavity.Clear()
                    .Cavity.DimName.Clear()
                    .Cavity.Assy.Clear()
                    .Cavity.Oper.Clear()
                    For i As Integer = 0 To grdApp_Face_Cavity.Rows.Count - 2
                        .Cavity.ID_Cavity.Add(i + 1)
                        .Cavity.DimName.Add(grdApp_Face_Cavity.Rows(i).Cells(0).Value)

                        Dim pAssyMin As Double = 0
                        If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(1).Value) And grdApp_Face_Cavity.Rows(i).Cells(1).Value <> "") Then
                            pAssyMin = ConvertToDbl(grdApp_Face_Cavity.Rows(i).Cells(1).Value)
                        End If

                        Dim pAssyMax As Double = 0
                        If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(2).Value) And grdApp_Face_Cavity.Rows(i).Cells(2).Value <> "") Then
                            pAssyMax = ConvertToDbl(grdApp_Face_Cavity.Rows(i).Cells(2).Value)
                        End If

                        Dim pAssy As clsProcessProj_App.clsCavity.sAssy
                        pAssy.Min = pAssyMin
                        pAssy.Max = pAssyMax
                        .Cavity.Assy.Add(pAssy)

                        Dim pOperMin As Double = 0
                        If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(3).Value) And grdApp_Face_Cavity.Rows(i).Cells(3).Value <> "") Then
                            pOperMin = ConvertToDbl(grdApp_Face_Cavity.Rows(i).Cells(3).Value)
                        End If

                        Dim pOperMax As Double = 0
                        If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(4).Value) And grdApp_Face_Cavity.Rows(i).Cells(4).Value <> "") Then
                            pOperMax = ConvertToDbl(grdApp_Face_Cavity.Rows(i).Cells(4).Value)
                        End If

                        Dim pOper As clsProcessProj_App.clsCavity.sOper
                        pOper.Min = pOperMin
                        pOper.Max = pOperMax
                        .Cavity.Oper.Add(pOper)
                    Next

                    .CavityFlange.Mat1 = txtApp_Mat1_Face.Text
                    .CavityFlange.Mat2 = txtApp_Mat2_Face.Text

                    If (txtApp_Hardness1_Face.Text <> "") Then
                        .CavityFlange.Hard1 = ConvertToDbl(txtApp_Hardness1_Face.Text)
                    Else
                        .CavityFlange.Hard1 = 0
                    End If

                    If (txtApp_Hardness2_Face.Text <> "") Then
                        .CavityFlange.Hard2 = ConvertToDbl(txtApp_Hardness2_Face.Text)
                    Else
                        .CavityFlange.Hard2 = 0
                    End If

                    If (txtApp_SF1_Face.Text <> "") Then
                        .CavityFlange.SF1 = ConvertToDbl(txtApp_SF1_Face.Text)
                    Else
                        .CavityFlange.SF1 = 0
                    End If

                    If (txtApp_SF2_Face.Text <> "") Then
                        .CavityFlange.SF2 = ConvertToDbl(txtApp_SF2_Face.Text)
                    Else
                        .CavityFlange.SF2 = 0
                    End If

                    .CavityFlange.MeasureSF = cmbFace_SF_ProcessName.Text
                    .CavityFlange.UnitSF = cmbFace_SF_Unit.Text

                    .Face.POrient = cmbApp_Face_POrient.Text
                    If (txtApp_Face_MaxFlangeSeparation.Text <> "") Then
                        .Face.MaxFlangeSep = txtApp_Face_MaxFlangeSeparation.Text
                    Else
                        .Face.MaxFlangeSep = 0
                    End If

                ElseIf (.Type = "Axial") Then

                    '....Axial
                    .Cavity.ID_Cavity.Clear()
                    .Cavity.DimName.Clear()
                    .Cavity.Assy.Clear()
                    .Cavity.Oper.Clear()
                    For i As Integer = 0 To grdApp_Axial_Cavity.Rows.Count - 2
                        .Cavity.ID_Cavity.Add(i + 1)
                        .Cavity.DimName.Add(grdApp_Axial_Cavity.Rows(i).Cells(0).Value)

                        Dim pAssyMin As Double = 0
                        If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(1).Value) And grdApp_Axial_Cavity.Rows(i).Cells(1).Value <> "") Then
                            pAssyMin = ConvertToDbl(grdApp_Axial_Cavity.Rows(i).Cells(1).Value)
                        End If

                        Dim pAssyMax As Double = 0
                        If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(2).Value) And grdApp_Axial_Cavity.Rows(i).Cells(2).Value <> "") Then
                            pAssyMax = ConvertToDbl(grdApp_Axial_Cavity.Rows(i).Cells(2).Value)
                        End If

                        Dim pAssy As clsProcessProj_App.clsCavity.sAssy
                        pAssy.Min = pAssyMin
                        pAssy.Max = pAssyMax
                        .Cavity.Assy.Add(pAssy)

                        Dim pOperMin As Double = 0
                        If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(3).Value) And grdApp_Axial_Cavity.Rows(i).Cells(3).Value <> "") Then
                            pOperMin = ConvertToDbl(grdApp_Axial_Cavity.Rows(i).Cells(3).Value)
                        End If

                        Dim pOperMax As Double = 0
                        If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(4).Value) And grdApp_Axial_Cavity.Rows(i).Cells(4).Value <> "") Then
                            pOperMax = ConvertToDbl(grdApp_Axial_Cavity.Rows(i).Cells(4).Value)
                        End If

                        Dim pOper As clsProcessProj_App.clsCavity.sOper
                        pOper.Min = pOperMin
                        pOper.Max = pOperMax
                        .Cavity.Oper.Add(pOper)

                    Next

                    .CavityFlange.Mat1 = txtApp_Mat1_Axial.Text
                    .CavityFlange.Mat2 = txtApp_Mat2_Axial.Text

                    If (txtApp_Hardness1_Axial.Text <> "") Then
                        .CavityFlange.Hard1 = ConvertToDbl(txtApp_Hardness1_Axial.Text)
                    Else
                        .CavityFlange.Hard1 = 0
                    End If

                    If (txtApp_Hardness2_Axial.Text <> "") Then
                        .CavityFlange.Hard2 = ConvertToDbl(txtApp_Hardness2_Axial.Text)
                    Else
                        .CavityFlange.Hard2 = 0
                    End If

                    If (txtApp_SF1_Axial.Text <> "") Then
                        .CavityFlange.SF1 = ConvertToDbl(txtApp_SF1_Axial.Text)
                    Else
                        .CavityFlange.SF1 = 0
                    End If

                    If (txtApp_SF2_Axial.Text <> "") Then
                        .CavityFlange.SF2 = ConvertToDbl(txtApp_SF2_Axial.Text)
                    Else
                        .CavityFlange.SF2 = 0
                    End If

                    .CavityFlange.MeasureSF = cmbAxial_SF_ProcessName.Text
                    .CavityFlange.UnitSF = cmbAxial_SF_Unit.Text

                    .Axial.IsStatic = IIf(cmbApp_Static_Axial.Text = "Y", True, False)

                    .Axial.IsRotating = IIf(cmbApp_Rotate_Axial.Text = "Y", True, False)
                    If (.Axial.IsRotating) Then
                        If (txtApp_RotateRPM_Axial.Text <> "") Then
                            .Axial.RPM = Convert.ToInt64(txtApp_RotateRPM_Axial.Text)
                        Else
                            .Axial.RPM = 0.0
                        End If

                    Else
                        .Axial.RPM = 0.0
                    End If

                    .Axial.IsRecip = IIf(cmbApp_Recip_Axial.Text = "Y", True, False)
                    If (.Axial.IsRecip) Then
                        .Axial.Recip_Stroke = ConvertToDbl(txtApp_RecipStrokeL_Axial.Text)
                        .Axial.Recip_V = ConvertToDbl(txtApp_RecipV_Axial.Text)
                        .Axial.Recip_CycleRate = ConvertToDbl(txtApp_RecipCycleRate_Axial.Text)
                        .Axial.Recip_ServiceLife = ConvertToDbl(txtApp_RecipServiceLife_Axial.Text)
                    Else
                        .Axial.Recip_Stroke = 0.0
                        .Axial.Recip_V = 0.0
                        .Axial.Recip_CycleRate = 0.0
                        .Axial.Recip_ServiceLife = 0.0
                    End If

                    .Axial.IsOscilatory = IIf(cmbApp_Osc_Axial.Text = "Y", True, False)
                    If (.Axial.IsOscilatory) Then
                        .Axial.Oscilate_Rot = ConvertToDbl(txtApp_OscRot_Axial.Text)
                        .Axial.Oscilate_V = ConvertToDbl(txtApp_OscV_Axial.Text)
                        .Axial.Oscilate_CycleRate = ConvertToDbl(txtApp_OscCycleRate_Axial.Text)
                        .Axial.Oscilate_ServiceLife = ConvertToDbl(txtApp_OscServiceLife_Axial.Text)
                    Else
                        .Axial.Oscilate_Rot = 0.0
                        .Axial.Oscilate_V = 0.0
                        .Axial.Oscilate_CycleRate = 0.0
                        .Axial.Oscilate_ServiceLife = 0.0
                    End If

                End If

            End With

            '.... "Design:"

            With mProcess_Project.Design
                .CustDwgNo = txtDesign_CustDwgNo.Text
                .CustDwgRev = txtDesign_CustDwgRev.Text

                .Frozen_Design = IIf(cmbDesign_Frozen.Text = "Y", True, False)
                .Frozen_Process = IIf(cmbDesign_Process.Text = "Y", True, False)

                .IsClass1 = IIf(cmbDesign_Class1.Text = "Y", True, False)
                .IsBuildToPrint = IIf(cmbDesign_BuildToPrint.Text = "Y", True, False)

                '.VerfyType = cmbDesign_TemperType.Text
                '.PerformedBy = cmbDesign_PerformedBy.Text
                .MCS = txtDesign_MCS.Text

                .IsWinnovation = IIf(cmbDesign_Winnovation.Text = "Y", True, False)

                If (.IsWinnovation) Then
                    .WinnovationNo = txtDesign_WinnovationNo.Text
                End If

                .Verification.ID_Verification.Clear()
                .Verification.Desc.Clear()
                .Verification.Owner.Clear()
                .Verification.Result.Clear()

                For i As Integer = 0 To grdDesign_Verification.Rows.Count - 2
                    .Verification.ID_Verification.Add(i + 1)
                    .Verification.Desc.Add(grdDesign_Verification.Rows(i).Cells(0).Value)
                    .Verification.Owner.Add(grdDesign_Verification.Rows(i).Cells(1).Value)
                    .Verification.Result.Add(grdDesign_Verification.Rows(i).Cells(2).Value)
                Next

                .IsMat_OutsideVender = IIf(cmbDesign_OutsideVendor.Text = "Y", True, False)

                .Input.ID_Input.Clear()
                .Input.Desc.Clear()
                For i As Integer = 0 To grdDesign_Input.Rows.Count - 2
                    .Input.ID_Input.Add(i + 1)
                    .Input.Desc.Add(grdDesign_Input.Rows(i).Cells(0).Value)
                Next

                '.KeyChar.ID_Key.Clear()
                '.KeyChar.KeyChar.Clear()
                'For i As Integer = 0 To grdDesign_KeyChar.Rows.Count - 2
                '    .KeyChar.ID_Key.Add(i + 1)
                '    .KeyChar.KeyChar.Add(grdDesign_Input.Rows(i).Cells(0).Value)
                'Next

                .FOD_Risks = txtDesign_FOD_Risks.Text

                .CustSpec.ID_Cust.Clear()
                .CustSpec.Type.Clear()
                .CustSpec.Desc.Clear()
                .CustSpec.Interpret.Clear()

                For i As Integer = 0 To grdDesign_CustSpec.Rows.Count - 2
                    .CustSpec.ID_Cust.Add(i + 1)
                    .CustSpec.Type.Add(grdDesign_CustSpec.Rows(i).Cells(0).Value)
                    .CustSpec.Desc.Add(grdDesign_CustSpec.Rows(i).Cells(1).Value)
                    .CustSpec.Interpret.Add(grdDesign_CustSpec.Rows(i).Cells(2).Value)
                Next

                .LessonsLearned = txtDesign_LessonsLearned.Text

                .SealDim.ID_Seal.Clear()
                .SealDim.Name.Clear()
                .SealDim.Min.Clear()
                .SealDim.Nom.Clear()
                .SealDim.Max.Clear()
                For i As Integer = 0 To grdDesign_Seal.Rows.Count - 2
                    .SealDim.ID_Seal.Add(i + 1)
                    .SealDim.Name.Add(grdDesign_Seal.Rows(i).Cells(0).Value)
                    If (Not IsNothing(grdDesign_Seal.Rows(i).Cells(1).Value)) Then
                        If (grdDesign_Seal.Rows(i).Cells(1).Value.ToString() <> "") Then
                            .SealDim.Min.Add(grdDesign_Seal.Rows(i).Cells(1).Value)
                        Else
                            .SealDim.Min.Add(0)
                        End If

                    Else
                        .SealDim.Min.Add(0)
                    End If

                    If (Not IsNothing(grdDesign_Seal.Rows(i).Cells(2).Value)) Then
                        If (grdDesign_Seal.Rows(i).Cells(2).Value.ToString() <> "") Then
                            .SealDim.Nom.Add(grdDesign_Seal.Rows(i).Cells(2).Value)
                        Else
                            .SealDim.Nom.Add(0)
                        End If

                    Else
                        .SealDim.Nom.Add(0)
                    End If

                    If (Not IsNothing(grdDesign_Seal.Rows(i).Cells(3).Value)) Then
                        If (grdDesign_Seal.Rows(i).Cells(3).Value.ToString() <> "") Then
                            .SealDim.Max.Add(grdDesign_Seal.Rows(i).Cells(3).Value)
                        Else
                            .SealDim.Max.Add(0)
                        End If

                    Else
                        .SealDim.Max.Add(0)
                    End If
                Next

                .Notes = txtDesign_Notes.Text

            End With


            '.... "Manufacturing:"

            With mProcess_Project.Manf
                .BaseMat_PartNo = txtManf_MatPartNo_Base.Text
                .SpringMat_PartNo = txtManf_MatPartNo_Spring.Text

                If (txtManf_HT.Text <> "") Then
                    .HT = txtManf_HT.Text
                Else
                    .HT = ""
                End If

                .PreComp_Glue = cmbManf_PrecompressionGlue.Text

                .ToolNGage.ID_Tool.Clear()
                .ToolNGage.PartNo.Clear()
                .ToolNGage.Desc.Clear()
                .ToolNGage.Type.Clear()
                .ToolNGage.Status.Clear()
                .ToolNGage.LeadTime.Clear()
                .ToolNGage.DesignResponsibility.Clear()

                For i As Integer = 0 To grdManf_ToolNGage.Rows.Count - 2
                    .ToolNGage.ID_Tool.Add(i + 1)
                    .ToolNGage.PartNo.Add(grdManf_ToolNGage.Rows(i).Cells(0).Value)
                    .ToolNGage.Desc.Add(grdManf_ToolNGage.Rows(i).Cells(1).Value)
                    .ToolNGage.Type.Add(grdManf_ToolNGage.Rows(i).Cells(2).Value)
                    .ToolNGage.Status.Add(grdManf_ToolNGage.Rows(i).Cells(3).Value)

                    If (Not IsNothing(grdManf_ToolNGage.Rows(i).Cells(4).Value)) Then
                        If (grdManf_ToolNGage.Rows(i).Cells(4).Value.ToString() <> "") Then
                            .ToolNGage.LeadTime.Add(grdManf_ToolNGage.Rows(i).Cells(4).Value)
                        Else
                            .ToolNGage.LeadTime.Add(0)
                        End If


                    Else
                        .ToolNGage.LeadTime.Add(0)
                    End If

                    .ToolNGage.DesignResponsibility.Add(grdManf_ToolNGage.Rows(i).Cells(5).Value)
                Next

            End With


            '.... "Purchasing:"

            '....Purchase
            With mProcess_Project.Purchase
                .Mat.ID_Mat.Clear()
                .Mat.Item.Clear()
                .Mat.EstQty.Clear()
                .Mat.Qty_Unit.Clear()
                .Mat.Status.Clear()
                .Mat.LeadTime.Clear()

                For i As Integer = 0 To grdPurchase_Mat.Rows.Count - 2
                    .Mat.ID_Mat.Add(i + 1)
                    .Mat.Item.Add(grdPurchase_Mat.Rows(i).Cells(0).Value)
                    If (Not IsNothing(grdPurchase_Mat.Rows(i).Cells(1).Value)) Then
                        If (grdPurchase_Mat.Rows(i).Cells(1).Value.ToString() <> "") Then
                            .Mat.EstQty.Add(grdPurchase_Mat.Rows(i).Cells(1).Value)
                        Else
                            .Mat.EstQty.Add(0)
                        End If
                    Else
                        .Mat.EstQty.Add(0)
                    End If

                    .Mat.Qty_Unit.Add(grdPurchase_Mat.Rows(i).Cells(2).Value)

                    .Mat.Status.Add(grdPurchase_Mat.Rows(i).Cells(3).Value)

                    If (Not IsNothing(grdPurchase_Mat.Rows(i).Cells(4).Value)) Then
                        If (grdPurchase_Mat.Rows(i).Cells(4).Value.ToString() <> "") Then
                            .Mat.LeadTime.Add(grdPurchase_Mat.Rows(i).Cells(4).Value)
                        Else
                            .Mat.LeadTime.Add(0)
                        End If

                    Else
                        .Mat.LeadTime.Add(0)
                    End If
                Next

                '.ToolNGage.ID_Tool.Clear()
                '.ToolNGage.PartNo.Clear()
                '.ToolNGage.Desc.Clear()
                '.ToolNGage.Type.Clear()
                '.ToolNGage.LeadTime.Clear()
                '.ToolNGage.DesignResponsibility.Clear()

                'For i As Integer = 0 To grdPurchase_ToolNGages.Rows.Count - 2
                '    .ToolNGage.ID_Tool.Add(i + 1)
                '    .ToolNGage.PartNo.Add(grdPurchase_ToolNGages.Rows(i).Cells(0).Value)
                '    .ToolNGage.Desc.Add(grdPurchase_ToolNGages.Rows(i).Cells(1).Value)
                '    .ToolNGage.Type.Add(grdPurchase_ToolNGages.Rows(i).Cells(2).Value)

                '    If (Not IsNothing(grdPurchase_ToolNGages.Rows(i).Cells(3).Value) And grdPurchase_ToolNGages.Rows(i).Cells(3).Value <> "") Then
                '        .ToolNGage.LeadTime.Add(grdManf_ToolNGage.Rows(i).Cells(3).Value)
                '    Else
                '        .ToolNGage.LeadTime.Add(0)
                '    End If

                '    .ToolNGage.DesignResponsibility.Add(grdPurchase_ToolNGages.Rows(i).Cells(4).Value)
                'Next

                .Dwg.ID_Dwg.Clear()
                .Dwg.No.Clear()
                .Dwg.Desc.Clear()
                .Dwg.LeadTime.Clear()
                For i As Integer = 0 To grdPurchase_Drawing.Rows.Count - 2
                    .Dwg.ID_Dwg.Add(i + 1)
                    .Dwg.No.Add(grdPurchase_Drawing.Rows(i).Cells(0).Value)
                    .Dwg.Desc.Add(grdPurchase_Drawing.Rows(i).Cells(1).Value)
                    If (Not IsNothing(grdPurchase_Drawing.Rows(i).Cells(2).Value)) Then
                        If (grdPurchase_Drawing.Rows(i).Cells(2).Value.ToString() <> "") Then
                            .Dwg.LeadTime.Add(grdPurchase_Drawing.Rows(i).Cells(2).Value)
                        Else
                            .Dwg.LeadTime.Add(0)
                        End If

                    Else
                        .Dwg.LeadTime.Add(0)
                    End If
                Next
            End With

            '.... "Qlty:"
            With mProcess_Project.Qlty
                If (cmbQuality_ApprovedSupplier.Text = "Y") Then
                    .IsApvdSupplierOnly = True
                Else
                    .IsApvdSupplierOnly = False
                End If

                If (cmbQuality_TNG.Text = "Y") Then
                    .Separate_Tool_Gage_Reqd = True
                Else
                    .Separate_Tool_Gage_Reqd = False
                End If

                If (cmbQuality_CustComplaint.Text = "Y") Then
                    .HasCustComplaint = True
                    .Reason = txtQuality_Reason.Text
                Else
                    .HasCustComplaint = False
                    .Reason = ""
                End If

                If (cmbQuality_VisualInspection.Text = "Y") Then
                    .VisualInspection = True
                    .VisualInspection_Type = cmbQuality_VisualInspection_Type.Text
                Else
                    .VisualInspection = False
                    .VisualInspection_Type = ""
                End If

                .CustAcceptStd = cmbQuality_CustAcceptStd.Text

                If (cmbQuality_SPC.Text = "Y") Then
                    .SPC_Reqd = True
                Else
                    .SPC_Reqd = False
                End If

                If (cmbQuality_GageRnR_Reqd.Text = "Y") Then
                    .GageRnR_Reqd = True
                Else
                    .GageRnR_Reqd = False
                End If

                'If (grdCost_SplOperation.Rows.Count > 1) Then
                '    grdQuality_SplOperation = grdCost_SplOperation
                'End If
                CopyDataGridView(grdCost_SplOperation, grdQuality_SplOperation)
                mProcess_Project.Cost.SplOperation.ID_SplOp.Clear()
                mProcess_Project.Cost.SplOperation.Desc.Clear()
                mProcess_Project.Cost.SplOperation.Spec.Clear()
                mProcess_Project.Cost.SplOperation.LeadTime.Clear()
                mProcess_Project.Cost.SplOperation.Cost.Clear()

                For j As Integer = 0 To grdQuality_SplOperation.Rows.Count - 2
                    mProcess_Project.Cost.SplOperation.ID_SplOp.Add(j + 1)
                    mProcess_Project.Cost.SplOperation.Desc.Add(grdQuality_SplOperation.Rows(j).Cells(0).Value)
                    mProcess_Project.Cost.SplOperation.Spec.Add(grdQuality_SplOperation.Rows(j).Cells(1).Value)

                    If (Not IsNothing(grdQuality_SplOperation.Rows(j).Cells(2).Value)) Then
                        If (grdQuality_SplOperation.Rows(j).Cells(2).Value.ToString() <> "") Then
                            mProcess_Project.Cost.SplOperation.LeadTime.Add(grdQuality_SplOperation.Rows(j).Cells(2).Value)
                        Else
                            mProcess_Project.Cost.SplOperation.LeadTime.Add(0)
                        End If

                    Else
                        mProcess_Project.Cost.SplOperation.LeadTime.Add(0)
                    End If

                    If (Not IsNothing(grdQuality_SplOperation.Rows(j).Cells(3).Value)) Then
                        If (grdQuality_SplOperation.Rows(j).Cells(3).Value.ToString() <> "") Then
                            mProcess_Project.Cost.SplOperation.Cost.Add(grdQuality_SplOperation.Rows(j).Cells(3).Value)
                        Else
                            mProcess_Project.Cost.SplOperation.Cost.Add(0)
                        End If


                    Else
                        mProcess_Project.Cost.SplOperation.Cost.Add(0)
                    End If

                Next

                'grdQuality_SplOperation.Refresh()
                'TabControl1.Refresh()

            End With


            '.... "Drawing:"

            With mProcess_Project.Dwg
                .DesignLevel = cmbDwg_DesignLevel.Text

                .Needed.ID_Needed.Clear()
                .Needed.DwgNo.Clear()
                .Needed.Desc.Clear()
                .Needed.Status.Clear()
                .Needed.LeadTime.Clear()

                For i As Integer = 0 To grdDrawing_Needed.Rows.Count - 2
                    .Needed.ID_Needed.Add(i + 1)
                    .Needed.DwgNo.Add(grdDrawing_Needed.Rows(i).Cells(0).Value)
                    .Needed.Desc.Add(grdDrawing_Needed.Rows(i).Cells(1).Value)
                    .Needed.Status.Add(grdDrawing_Needed.Rows(i).Cells(2).Value)
                    If (Not IsNothing(grdDrawing_Needed.Rows(i).Cells(3).Value)) Then
                        If (grdDrawing_Needed.Rows(i).Cells(3).Value.ToString() <> "") Then
                            .Needed.LeadTime.Add(grdDrawing_Needed.Rows(i).Cells(3).Value)
                        Else
                            .Needed.LeadTime.Add(0)
                        End If

                    Else
                        .Needed.LeadTime.Add(0)
                    End If
                Next

                .BOM.ID_BOM.Clear()
                .BOM.Parent_PartNo.Clear()
                .BOM.Child_PartNo.Clear()
                .BOM.Qty.Clear()


                For i As Integer = 0 To grdDrawing_BOM.Rows.Count - 2
                    .BOM.ID_BOM.Add(i + 1)
                    .BOM.Parent_PartNo.Add(grdDrawing_BOM.Rows(i).Cells(0).Value)
                    .BOM.Child_PartNo.Add(grdDrawing_BOM.Rows(i).Cells(1).Value)
                    .BOM.Qty.Add(grdDrawing_BOM.Rows(i).Cells(2).Value)
                Next

            End With

            '.... "Testing:"

            With mProcess_Project.Test

                .Other = txtTest_Other.Text

                '....Leak
                If (txtTest_CompressPre_Leak.Text <> "") Then
                    .Leak.Compress_Unplated = txtTest_CompressPre_Leak.Text
                Else
                    .Leak.Compress_Unplated = 0
                End If

                If (txtTest_CompressPost_Leak.Text <> "") Then
                    .Leak.Compress_Plated = txtTest_CompressPost_Leak.Text
                Else
                    .Leak.Compress_Plated = 0
                End If

                .Leak.Medium_Unplated = cmbTest_MediaPre_Leak.Text
                .Leak.Medium_Plated = cmbTest_MediaPost_Leak.Text

                If (txtTest_PressPre_Leak.Text <> "") Then
                    .Leak.Press_Unplated = txtTest_PressPre_Leak.Text
                Else
                    .Leak.Press_Unplated = 0
                End If

                If (txtTest_PressPost_Leak.Text <> "") Then
                    .Leak.Press_Plated = txtTest_PressPost_Leak.Text
                Else
                    .Leak.Press_Plated = 0
                End If

                If (txtTest_ReqPre_Leak.Text <> "") Then
                    .Leak.Max_Unplated = txtTest_ReqPre_Leak.Text
                Else
                    .Leak.Max_Unplated = 0
                End If

                If (txtTest_ReqPost_Leak.Text <> "") Then
                    .Leak.Max_Plated = txtTest_ReqPost_Leak.Text
                Else
                    .Leak.Max_Plated = 0
                End If

                If (cmbTest_QtyPre_Leak.Text <> "") Then
                    .Leak.Qty_Unplated = ConvertToDbl(cmbTest_QtyPre_Leak.Text)
                Else
                    .Leak.Qty_Unplated = 0
                End If

                If (cmbTest_QtyPost_Leak.Text <> "") Then
                    .Leak.Qty_Plated = cmbTest_QtyPost_Leak.Text
                Else
                    .Leak.Qty_Plated = 0
                End If


                .Leak.Freq_Unplated = cmbTest_FreqPre_Leak.Text
                .Leak.Freq_Plated = cmbTest_FreqPost_Leak.Text


                '....Load
                If (txtTest_CompressPre_Load.Text <> "") Then
                    .Load.Compress_Unplated = txtTest_CompressPre_Load.Text
                Else
                    .Load.Compress_Unplated = 0
                End If

                If (txtTest_CompressPost_Load.Text <> "") Then
                    .Load.Compress_Plated = txtTest_CompressPost_Load.Text
                Else
                    .Load.Compress_Plated = 0
                End If

                If (txtTest_ReqPre_Load.Text <> "") Then
                    .Load.Max_Unplated = txtTest_ReqPre_Load.Text
                Else
                    .Load.Max_Unplated = 0
                End If

                If (txtTest_ReqPost_Load.Text <> "") Then
                    .Load.Max_Plated = txtTest_ReqPost_Load.Text
                Else
                    .Load.Max_Plated = 0
                End If

                If (cmbTest_QtyPre_Load.Text <> "") Then
                    .Load.Qty_Unplated = ConvertToDbl(cmbTest_QtyPre_Load.Text)
                Else
                    .Load.Qty_Unplated = 0
                End If

                If (cmbTest_QtyPost_Load.Text <> "") Then
                    .Load.Qty_Plated = cmbTest_QtyPost_Load.Text
                Else
                    .Load.Qty_Plated = 0
                End If

                '.Load.QtyPre = cmbTest_QtyPre_Load.Text
                '.Load.QtyPost = cmbTest_QtyPost_Load.Text
                .Load.Freq_Unplated = cmbTest_FreqPre_Load.Text
                .Load.Freq_Plated = cmbTest_FreqPost_Load.Text

                '....SpringBack
                If (txtTest_CompressPre_SpringBack.Text <> "") Then
                    .SpringBack.Compress_Unplated = txtTest_CompressPre_SpringBack.Text
                Else
                    .SpringBack.Compress_Unplated = 0
                End If

                If (txtTest_CompressPost_SpringBack.Text <> "") Then
                    .SpringBack.Compress_Plated = txtTest_CompressPost_SpringBack.Text
                Else
                    .SpringBack.Compress_Plated = 0
                End If

                If (txtTest_ReqPre_SpringBack.Text <> "") Then
                    .SpringBack.Max_Unplated = txtTest_ReqPre_SpringBack.Text
                Else
                    .SpringBack.Max_Unplated = 0
                End If

                If (txtTest_ReqPost_SpringBack.Text <> "") Then
                    .SpringBack.Max_Plated = txtTest_ReqPost_SpringBack.Text
                Else
                    .SpringBack.Max_Plated = 0
                End If

                If (cmbTest_QtyPre_SpringBack.Text <> "") Then
                    .SpringBack.Qty_Unplated = ConvertToDbl(cmbTest_QtyPre_SpringBack.Text)
                Else
                    .SpringBack.Qty_Unplated = 0
                End If

                If (cmbTest_QtyPost_SpringBack.Text <> "") Then
                    .SpringBack.Qty_Plated = cmbTest_QtyPost_SpringBack.Text
                Else
                    .SpringBack.Qty_Plated = 0
                End If

                '.SpringBack.QtyPre = cmbTest_QtyPre_SpringBack.Text
                '.SpringBack.QtyPost = cmbTest_QtyPost_SpringBack.Text
                .SpringBack.Freq_Unplated = cmbTest_FreqPre_SpringBack.Text
                .SpringBack.Freq_Plated = cmbTest_FreqPost_SpringBack.Text

            End With

            '.... "Planning:"

            ' ''....Planning
            '' ''With mProcess_Project.Planning

            '' ''    .SplOperation.ID_SplOperation.Clear()
            '' ''    .SplOperation.Desc.Clear()
            '' ''    .SplOperation.LeadTimeStart.Clear()
            '' ''    .SplOperation.Index.Clear()

            '' ''    For i As Integer = 0 To grdPlanning_Ordered.Rows.Count - 1
            '' ''        .SplOperation.ID_SplOperation.Add(i + 1)
            '' ''        .SplOperation.Desc.Add(grdPlanning_Ordered.Rows(i).Cells(0).Value)
            '' ''        If (Not IsNothing(grdPlanning_Ordered.Rows(i).Cells(1).Value)) Then
            '' ''            If (grdPlanning_Ordered.Rows(i).Cells(1).Value.ToString() <> "") Then
            '' ''                .SplOperation.LeadTimeStart.Add(grdPlanning_Ordered.Rows(i).Cells(1).Value)
            '' ''            Else
            '' ''                .SplOperation.LeadTimeStart.Add(0)
            '' ''            End If

            '' ''        Else
            '' ''            .SplOperation.LeadTimeStart.Add(0)
            '' ''        End If
            '' ''        .SplOperation.Index.Add(i + 1)
            '' ''    Next

            '' ''    .MileOperation.ID_MileOperation.Clear()
            '' ''    .MileOperation.Name.Clear()
            '' ''    .MileOperation.LeadTime.Clear()

            '' ''    For i As Integer = 0 To grdPlanning_Ordered.Rows.Count - 2
            '' ''        .MileOperation.ID_MileOperation.Add(i + 1)
            '' ''        .MileOperation.Name.Add(grdPlanning_MileStoneOperation.Rows(i).Cells(0).Value)
            '' ''        If (Not IsNothing(grdPlanning_MileStoneOperation.Rows(i).Cells(1).Value) And grdPlanning_MileStoneOperation.Rows(i).Cells(1).Value <> "") Then
            '' ''            .MileOperation.LeadTime.Add(grdPlanning_MileStoneOperation.Rows(i).Cells(1).Value)
            '' ''        Else
            '' ''            .MileOperation.LeadTime.Add(0)
            '' ''        End If
            '' ''    Next

            '' ''    .Notes = txtPlanning_Notes.Text

            '' ''End With


            '.... "Shipping:"

            With mProcess_Project.Shipping
                .Notes = txtShipping_Notes.Text

            End With

            '.... "IssueCommnt:"

            With mProcess_Project.IssueCommnt
                '....IssueCommnt
                .ID.Clear()
                .Comment.Clear()
                .ByDept.Clear()
                .ByName.Clear()
                .ByDate.Clear()
                .ToDept.Clear()
                .Resolved.Clear()
                .Name.Clear()
                '.ResolvedDate.Clear()
                .DateResolution.Clear()
                .Resolution.Clear()


                For j As Integer = 0 To grdIssueComment.Rows.Count - 1
                    .ID.Add(j + 1)
                    '.SlNo.Add(grdIssueComment.Rows(j).Cells(0).Value)
                    .Comment.Add(grdIssueComment.Rows(j).Cells(0).Value)
                    .ByDept.Add(grdIssueComment.Rows(j).Cells(1).Value)
                    .ByName.Add(grdIssueComment.Rows(j).Cells(2).Value)
                    If (grdIssueComment.Rows(j).Cells(3).Value <> "" And Not IsNothing(grdIssueComment.Rows(j).Cells(3).Value)) Then
                        .ByDate.Add(grdIssueComment.Rows(j).Cells(3).Value)
                    Else
                        .ByDate.Add(DateTime.MinValue)
                    End If

                    .ToDept.Add(grdIssueComment.Rows(j).Cells(4).Value)
                    If (grdIssueComment.Rows(j).Cells(5).Value = "Y") Then
                        .Resolved.Add(True)
                    Else
                        .Resolved.Add(False)
                    End If
                    '.Resolved.Add(grdIssueComment.Rows(j).Cells(6).Value)
                    .Name.Add(grdIssueComment.Rows(j).Cells(6).Value)

                    If (grdIssueComment.Rows(j).Cells(7).Value <> "" And Not IsNothing(grdIssueComment.Rows(j).Cells(7).Value)) Then
                        .DateResolution.Add(grdIssueComment.Rows(j).Cells(7).Value)
                    Else
                        .DateResolution.Add(DateTime.MinValue)
                    End If

                    'If (grdIssueComment.Rows(j).Cells(7).Value <> "" And Not IsNothing(grdIssueComment.Rows(j).Cells(7).Value)) Then
                    '    .DaResolvedDate.Add(grdIssueComment.Rows(j).Cells(7).Value)
                    'Else
                    '    .ResolvedDate.Add(DateTime.MinValue)
                    'End If

                    .Resolution.Add(grdIssueComment.Rows(j).Cells(8).Value)
                Next

            End With


            '.... "Approval:"   
            'AES 02APR18
            With mProcess_Project.Approval
                .ID_Approval.Clear()
                .Dept.Clear()
                .Name.Clear()
                .Title.Clear()
                .Signed.Clear()
                .DateSigned.Clear()

                For j As Integer = 0 To grdApproval_Attendees.Rows.Count - 1
                    .ID_Approval.Add(j + 1)
                    .Dept.Add(grdApproval_Attendees.Rows(j).Cells(0).Value)
                    If (grdApproval_Attendees.Rows(j).Cells(3).Value = True) Then
                        .Name.Add(grdApproval_Attendees.Rows(j).Cells(1).Value)
                        .Title.Add(grdApproval_Attendees.Rows(j).Cells(2).Value)
                        .Signed.Add(grdApproval_Attendees.Rows(j).Cells(3).Value)
                        If (grdApproval_Attendees.Rows(j).Cells(4).Value <> "" And Not IsNothing(grdApproval_Attendees.Rows(j).Cells(4).Value)) Then
                            .DateSigned.Add(grdApproval_Attendees.Rows(j).Cells(4).Value)
                        Else
                            .DateSigned.Add(DateTime.MinValue)
                        End If
                    Else
                        .Name.Add(Nothing)
                        .Title.Add(Nothing)
                        .Signed.Add(False)
                        .DateSigned.Add(DateTime.MinValue)

                    End If
                    '.Name.Add(grdApproval_Attendees.Rows(j).Cells(1).Value)
                    '.Title.Add(grdApproval_Attendees.Rows(j).Cells(2).Value)
                    '.Signed.Add(grdApproval_Attendees.Rows(j).Cells(3).Value)
                    'If (grdApproval_Attendees.Rows(j).Cells(4).Value <> "" And Not IsNothing(grdApproval_Attendees.Rows(j).Cells(4).Value)) Then
                    '    .DateSigned.Add(grdApproval_Attendees.Rows(j).Cells(4).Value)
                    'Else
                    '    .DateSigned.Add(DateTime.MinValue)
                    'End If

                Next
            End With

            gProcessProject = mProcess_Project.Clone()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SaveToDB()
        '==================
        'mProcess_Project.SaveToDB(mPNID, mRevID)

        '....Header Signed-Off
        mProcess_Project.EditedBy.ID_Edit.Clear()
        mProcess_Project.EditedBy.DateEdited.Clear()
        mProcess_Project.EditedBy.Name.Clear()
        mProcess_Project.EditedBy.Comment.Clear()

        If (txtDateMod.Text <> "") Then
            mProcess_Project.EditedBy.ID_Edit.Add(1)
            mProcess_Project.EditedBy.DateEdited.Add(txtDateMod.Text)
            mProcess_Project.EditedBy.Name.Add(txtHeaderUserName.Text)
            mProcess_Project.EditedBy.Comment.Add("")
            mProcess_Project.EditedBy.SaveToDB(mProcess_Project.ID, "Header")
        End If

        mProcess_Project.PreOrder.SaveToDB(mProcess_Project.ID)
        mProcess_Project.CustContact.SaveToDB(mProcess_Project.ID)
        mProcess_Project.ITAR_Export.SaveToDB(mProcess_Project.ID)
        mProcess_Project.OrdEntry.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Cost.SaveToDB(mProcess_Project.ID)
        mProcess_Project.App.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Design.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Manf.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Purchase.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Qlty.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Dwg.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Test.SaveToDB(mProcess_Project.ID, chkTest.Checked)
        'mProcess_Project.Planning.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Shipping.SaveToDB(mProcess_Project.ID)
        mProcess_Project.IssueCommnt.SaveToDB(mProcess_Project.ID)
        mProcess_Project.Approval.SaveToDB(mProcess_Project.ID)     'AES 02APR18

    End Sub

    Private Function CompareVal_Header() As Boolean
        '===========================================
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pCI As New CultureInfo("en-US")

        With mProcess_Project
            CompareVal(.POPCoding, cmbPopCoding.Text, pCount)
            CompareVal(.Rating, cmbRating.Text, pCount)
            CompareVal(.Type, cmbType.Text, pCount)
            CompareVal(.DateOpen.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), txtStartDate.Text, pCount)

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_PreOrder() As Boolean
        '==============================================

        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pCI As New CultureInfo("en-US")
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.PreOrder
            CompareVal(.Mgr.Mkt, cmbMgrPreOrder.Text, pCount)

            CompareVal(.Mgr.Sales, txtMgrSales.Text, pCount)

            Dim pExpReq As Boolean
            If (cmbExport_Reqd.Text = "Y") Then
                pExpReq = True
            Else
                pExpReq = False
            End If

            CompareVal(.Export.Reqd, pExpReq, pCount)
            CompareVal(.Export.Status, cmbExport_Status.Text, pCount)
            CompareVal(.Part.Family, cmbPartFamily.Text, pCount)
            CompareVal(.Export.Status, cmbExport_Status.Text, pCount)
            CompareVal(.Mkt.Seg, cmbPreOrderSeg.Text, pCount)
            CompareVal(.Mkt.Channel, cmbPreOrderChannel.Text, pCount)
            CompareVal(.Notes, txtPreOrderNotes.Text, pCount)
            CompareVal(.Loc.CostFile, cmbCostFileLoc.Text, pCount)
            CompareVal(.Loc.RFQPkg, cmbRFQPkgLoc.Text, pCount)
            CompareVal(.Notes_Price, txtPreOrderPriceNotes.Text, pCount)

        End With

        If (grdCustContact.Rows.Count - 1 <> mProcess_Project.CustContact.ID_Cust.Count) Then
            pCount = pCount + 1
        Else
            For i As Integer = 0 To mProcess_Project.CustContact.ID_Cust.Count - 1
                CompareVal(mProcess_Project.CustContact.DeptName(i), grdCustContact.Rows(i).Cells(0).Value, pCount)
                CompareVal(mProcess_Project.CustContact.Name(i), grdCustContact.Rows(i).Cells(1).Value, pCount)
                CompareVal(mProcess_Project.CustContact.Phone(i), grdCustContact.Rows(i).Cells(2).Value, pCount)
                CompareVal(mProcess_Project.CustContact.Email(i), grdCustContact.Rows(i).Cells(3).Value, pCount)
            Next
        End If


        If (grdQuote.Rows.Count - 1 <> mProcess_Project.PreOrder.Quote.QID.Count) Then
            pCount = pCount + 1
        Else
            For i As Integer = 0 To mProcess_Project.PreOrder.Quote.QID.Count - 1
                pDateVal = DateTime.MinValue
                If (grdQuote.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdQuote.Rows(i).Cells(0).Value)) Then
                    pDateVal = Convert.ToDateTime(grdQuote.Rows(i).Cells(0).Value)
                End If

                CompareVal(mProcess_Project.PreOrder.Quote.QDate(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                CompareVal(mProcess_Project.PreOrder.Quote.No(i), grdQuote.Rows(i).Cells(1).Value, pCount)

            Next
        End If


        If (grdPreOrder_SalesData.Rows.Count - 1 <> mProcess_Project.PreOrder.SalesData.ID_Sales.Count) Then
            pCount = pCount + 1

        Else
            For i As Integer = 0 To mProcess_Project.PreOrder.SalesData.ID_Sales.Count - 1
                CompareVal(mProcess_Project.PreOrder.SalesData.Year(i).ToString(), grdPreOrder_SalesData.Rows(i).Cells(0).Value, pCount)
                Dim pVal As Integer = 0

                If (Not IsNothing(grdPreOrder_SalesData.Rows(i).Cells(1).Value)) Then
                    pVal = Convert.ToInt32(grdPreOrder_SalesData.Rows(i).Cells(1).Value)
                End If
                CompareVal(mProcess_Project.PreOrder.SalesData.Qty(i), pVal, pCount)
                CompareVal(mProcess_Project.PreOrder.SalesData.Price(i), grdPreOrder_SalesData.Rows(i).Cells(2).Value, pCount)
                CompareVal(mProcess_Project.PreOrder.SalesData.Total(i), grdPreOrder_SalesData.Rows(i).Cells(3).Value, pCount)

            Next
        End If

        mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "PreOrder")
        If (grdPreOrderEditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
            pCount = pCount + 1
        Else
            For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                pDateVal = DateTime.MinValue
                If (grdPreOrderEditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdPreOrderEditedBy.Rows(i).Cells(0).Value)) Then
                    pDateVal = Convert.ToDateTime(grdPreOrderEditedBy.Rows(i).Cells(0).Value)
                End If
                CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                CompareVal(mProcess_Project.EditedBy.Name(i), grdPreOrderEditedBy.Rows(i).Cells(1).Value, pCount)
                CompareVal(mProcess_Project.EditedBy.Comment(i), grdPreOrderEditedBy.Rows(i).Cells(2).Value, pCount)
            Next
        End If

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Export() As Boolean
        '===========================================
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pCI As New CultureInfo("en-US")
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.ITAR_Export

            Dim pblnFlag As Boolean
            If (cmbITAR_Export_CustOnDenialList.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsCustOnDenialList, pblnFlag, pCount)

            If (cmbITAR_Export_CountryProhibited.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.CountryProhibited, pblnFlag, pCount)

            If (cmbITAR_Export_AntiBoycottLang.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.HasAntiBoycottLang, pblnFlag, pCount)

            If (cmbITAR_Export_ProductITAR_Reg.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsUnder_ITAR_Reg, pblnFlag, pCount)

            CompareVal(.ITAR_Class, txtITAR_Export_ITAR_Classification.Text, pCount)

            If (cmbITAR_Export_SaleExportControlled.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.SaleExportControlled, pblnFlag, pCount)

            CompareVal(.EAR_Class, cmbITAR_Export_EAR_Classification.Text, pCount)

            CompareVal(.Status, cmbITAR_Export_Status.Text, pCount)

            CompareVal(.HTS_Class, txtITAR_Export_HTS_Classification.Text, pCount)

        End With

        mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Export")
        If (grdExport_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
            pCount = pCount + 1
        Else
            For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                pDateVal = DateTime.MinValue
                If (grdExport_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdExport_EditedBy.Rows(i).Cells(0).Value)) Then
                    pDateVal = Convert.ToDateTime(grdExport_EditedBy.Rows(i).Cells(0).Value)
                End If

                CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                CompareVal(mProcess_Project.EditedBy.Name(i), grdExport_EditedBy.Rows(i).Cells(1).Value, pCount)
                CompareVal(mProcess_Project.EditedBy.Comment(i), grdExport_EditedBy.Rows(i).Cells(2).Value, pCount)
            Next
        End If

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_OrdEntry() As Boolean
        '=============================================
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pCI As New CultureInfo("en-US")
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.OrdEntry

            CompareVal(.SalesOrderNo, txtOrdEntry_SalesOrderNo.Text, pCount)

            pDateVal = DateTime.MinValue
            If (txtOrdEntry_SalesDate.Text <> "") Then
                pDateVal = Convert.ToDateTime(txtOrdEntry_SalesDate.Text)
            End If
            CompareVal(.DateSales.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)

            CompareVal(.LeadTimeQuoted, ConvertToDbl(txtOrderEntry_QtdLeadTime.Text), pCount)

            If (grdOrdEntry_CustContact.Rows.Count - 1 <> mProcess_Project.CustContact.ID_Cust.Count) Then
                pCount = pCount + 1
            Else

                For i As Integer = 0 To mProcess_Project.CustContact.ID_Cust.Count - 1

                    CompareVal(mProcess_Project.CustContact.DeptName(i), grdOrdEntry_CustContact.Rows(i).Cells(0).Value, pCount)
                    CompareVal(mProcess_Project.CustContact.Name(i), grdOrdEntry_CustContact.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.CustContact.Phone(i), grdOrdEntry_CustContact.Rows(i).Cells(2).Value, pCount)
                    CompareVal(mProcess_Project.CustContact.Email(i), grdOrdEntry_CustContact.Rows(i).Cells(3).Value, pCount)

                Next

            End If

            CompareVal(.PONo, txtOrdEntry_PONo.Text, pCount)

            pDateVal = DateTime.MinValue
            If (txtOrdEntry_PODate.Text <> "") Then
                pDateVal = Convert.ToDateTime(txtOrdEntry_PODate.Text)
            End If
            CompareVal(.DatePO.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)

            pDateVal = DateTime.MinValue
            If (txtOrdEntry_PODate_EDI.Text <> "") Then
                pDateVal = Convert.ToDateTime(txtOrdEntry_PODate_EDI.Text)
            End If
            CompareVal(.DatePO_EDI.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)

            Dim pblnFlag As Boolean
            If (cmbOrdEntry_SpecialReq.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.HasSplReq, pblnFlag, pCount)

            If (cmbOrdEntry_Tooling.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.Tool_Reqd, pblnFlag, pCount)

            If (cmbOrdEntry_SplPkgNLbl.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.SplPkg_Lbl_Reqd, pblnFlag, pCount)

            Dim pVal As Integer = 0
            If (txtOrdEntry_OrderQty.Text <> "") Then
                pVal = Convert.ToInt32(txtOrdEntry_OrderQty.Text)
            End If
            CompareVal(.OrdQty, pVal, pCount)

            pDateVal = DateTime.MinValue
            If (txtOrdEntry_OrderShipDate.Text <> "") Then
                pDateVal = Convert.ToDateTime(txtOrdEntry_OrderShipDate.Text)
            End If
            CompareVal(.DateOrdShip.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)

            If (cmbOrdEntry_Expedited.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.Expedited, pblnFlag, pCount)

            If (cmbOrdEntry_DFAR.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsDFAR, pblnFlag, pCount)

        End With

        mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "OrdEntry")
        If (grdOrdEntry_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
            pCount = pCount + 1
        Else
            For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                pDateVal = DateTime.MinValue
                If (grdOrdEntry_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdOrdEntry_EditedBy.Rows(i).Cells(0).Value)) Then
                    pDateVal = Convert.ToDateTime(grdOrdEntry_EditedBy.Rows(i).Cells(0).Value)
                End If
                CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                CompareVal(mProcess_Project.EditedBy.Name(i), grdOrdEntry_EditedBy.Rows(i).Cells(1).Value, pCount)
                CompareVal(mProcess_Project.EditedBy.Comment(i), grdOrdEntry_EditedBy.Rows(i).Cells(2).Value, pCount)
            Next
        End If

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged
    End Function

    Private Function CompareVal_CostEstimating() As Boolean
        '===================================================
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pCI As New CultureInfo("en-US")
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Cost

            CompareVal(.QuoteFileLoc, cmbCost_QuoteFile.Text, pCount)

            If (grdCost_SplOperation.Rows.Count - 1 <> .SplOperation.ID_SplOp.Count) Then
                pCount = pCount + 1

            Else
                For i As Integer = 0 To .SplOperation.ID_SplOp.Count - 1

                    CompareVal(.SplOperation.Desc(i), grdCost_SplOperation.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.SplOperation.Spec(i), grdCost_SplOperation.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.SplOperation.LeadTime(i), grdCost_SplOperation.Rows(i).Cells(2).Value, pCount)
                    CompareVal(.SplOperation.Cost(i), grdCost_SplOperation.Rows(i).Cells(3).Value, pCount)

                Next

            End If

            CompareVal(.Notes, txtCost_Notes.Text, pCount)

        End With

        mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Cost")
        If (grdCost_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
            pCount = pCount + 1
        Else
            For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                pDateVal = DateTime.MinValue
                If (grdCost_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdCost_EditedBy.Rows(i).Cells(0).Value)) Then
                    pDateVal = Convert.ToDateTime(grdCost_EditedBy.Rows(i).Cells(0).Value)
                End If
                CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), grdCost_EditedBy.Rows(i).Cells(0).Value, pCount)
                CompareVal(mProcess_Project.EditedBy.Name(i), grdCost_EditedBy.Rows(i).Cells(1).Value, pCount)
                CompareVal(mProcess_Project.EditedBy.Comment(i), grdCost_EditedBy.Rows(i).Cells(2).Value, pCount)
            Next
        End If

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_App() As Boolean
        '========================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.App

            '....tab Genearal
            CompareVal(.Eqp, txtApp_Equip.Text, pCount)

            CompareVal(.ExistingSeal, txtApp_ExistingSeal.Text, pCount)

            CompareVal(.Type, cmbApp_InsertLoc.Text, pCount)

            CompareVal(.Fluid, txtApp_Fluid.Text, pCount)

            CompareVal(.MaxLeak, ConvertToDbl(txtApp_MaxLeak.Text), pCount)

            Dim pblnFlag As Boolean
            If (cmbApp_PressCycle.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsPressCyclic, pblnFlag, pCount)
            CompareVal(.PressCycle_Freq, ConvertToDbl(txtApp_PressCycleFreq.Text), pCount)
            CompareVal(.PressCycle_Amp, ConvertToDbl(txtApp_PressCycleAmp.Text), pCount)

            If (cmbApp_Shaped.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.Shaped, pblnFlag, pCount)

            If (cmbApp_OutOfRound.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsOoR, pblnFlag, pCount)


            If (cmbApp_SplitRing.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsSplitRing, pblnFlag, pCount)


            If (cmbApp_PreComp.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsPreComp, pblnFlag, pCount)

            '....Operating Condition
            CompareVal(.OpCond.T.Assy, ConvertToDbl(grdApp_OpCond.Rows(0).Cells(1).Value), pCount)

            CompareVal(.OpCond.T.Min, ConvertToDbl(grdApp_OpCond.Rows(0).Cells(2).Value), pCount)

            CompareVal(.OpCond.T.Max, ConvertToDbl(grdApp_OpCond.Rows(0).Cells(3).Value), pCount)

            CompareVal(.OpCond.T.Oper, ConvertToDbl(grdApp_OpCond.Rows(0).Cells(4).Value), pCount)

            CompareVal(.OpCond.Press.Assy, ConvertToDbl(grdApp_OpCond.Rows(1).Cells(1).Value), pCount)

            CompareVal(.OpCond.Press.Min, ConvertToDbl(grdApp_OpCond.Rows(1).Cells(2).Value), pCount)

            CompareVal(.OpCond.Press.Max, ConvertToDbl(grdApp_OpCond.Rows(1).Cells(3).Value), pCount)

            CompareVal(.OpCond.Press.Oper, ConvertToDbl(grdApp_OpCond.Rows(1).Cells(4).Value), pCount)

            '....Load
            CompareVal(.Load.Assy.Min, ConvertToDbl(grdApp_Load.Rows(0).Cells(1).Value), pCount)

            CompareVal(.Load.Assy.Max, ConvertToDbl(grdApp_Load.Rows(0).Cells(2).Value), pCount)

            '....Load
            CompareVal(.Load.Oper.Min, ConvertToDbl(grdApp_Load.Rows(1).Cells(1).Value), pCount)

            CompareVal(.Load.Oper.Max, ConvertToDbl(grdApp_Load.Rows(1).Cells(2).Value), pCount)

        End With

        If (mProcess_Project.App.Type = "Face") Then

            '....tab Face Seal
            With mProcess_Project.App

                If (grdApp_Face_Cavity.Rows.Count - 1 <> .Cavity.ID_Cavity.Count) Then
                    pCount = pCount + 1

                Else
                    For i As Integer = 0 To .Cavity.ID_Cavity.Count - 1

                        CompareVal(.Cavity.DimName(i), grdApp_Face_Cavity.Rows(i).Cells(0).Value, pCount)

                        CompareVal(.Cavity.Assy(i).Min, grdApp_Face_Cavity.Rows(i).Cells(1).Value, pCount)
                        CompareVal(.Cavity.Assy(i).Max, grdApp_Face_Cavity.Rows(i).Cells(2).Value, pCount)

                        CompareVal(.Cavity.Oper(i).Min, grdApp_Face_Cavity.Rows(i).Cells(3).Value, pCount)
                        CompareVal(.Cavity.Oper(i).Max, grdApp_Face_Cavity.Rows(i).Cells(4).Value, pCount)

                    Next

                End If

                CompareVal(.CavityFlange.Mat1, txtApp_Mat1_Face.Text, pCount)
                CompareVal(.CavityFlange.Mat2, txtApp_Mat2_Face.Text, pCount)

                CompareVal(.CavityFlange.Hard1, ConvertToDbl(txtApp_Hardness1_Face.Text), pCount)
                CompareVal(.CavityFlange.Hard2, ConvertToDbl(txtApp_Hardness2_Face.Text), pCount)

                CompareVal(.CavityFlange.SF1, ConvertToDbl(txtApp_SF1_Face.Text), pCount)
                CompareVal(.CavityFlange.SF2, ConvertToDbl(txtApp_SF2_Face.Text), pCount)

                CompareVal(.CavityFlange.MeasureSF, cmbFace_SF_ProcessName.Text, pCount)
                CompareVal(.CavityFlange.UnitSF, cmbFace_SF_Unit.Text, pCount)

                CompareVal(.Face.POrient, cmbApp_Face_POrient.Text, pCount)
                CompareVal(.Face.MaxFlangeSep, ConvertToDbl(txtApp_Face_MaxFlangeSeparation.Text), pCount)

            End With

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "App")
            If (grdApp_EditedBy_Face.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdApp_EditedBy_Face.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdApp_EditedBy_Face.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdApp_EditedBy_Face.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdApp_EditedBy_Face.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdApp_EditedBy_Face.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        Else
            '....tab Axial Seal
            With mProcess_Project.App

                If (grdApp_Axial_Cavity.Rows.Count - 1 <> .Cavity.ID_Cavity.Count) Then
                    pCount = pCount + 1

                Else
                    For i As Integer = 0 To .Cavity.ID_Cavity.Count - 1

                        CompareVal(.Cavity.DimName(i), grdApp_Axial_Cavity.Rows(i).Cells(0).Value, pCount)

                        CompareVal(.Cavity.Assy(i).Min, grdApp_Axial_Cavity.Rows(i).Cells(1).Value, pCount)
                        CompareVal(.Cavity.Assy(i).Max, grdApp_Axial_Cavity.Rows(i).Cells(2).Value, pCount)

                        CompareVal(.Cavity.Oper(i).Min, grdApp_Axial_Cavity.Rows(i).Cells(3).Value, pCount)
                        CompareVal(.Cavity.Oper(i).Max, grdApp_Axial_Cavity.Rows(i).Cells(4).Value, pCount)

                    Next

                End If

                CompareVal(.CavityFlange.Mat1, txtApp_Mat1_Axial.Text, pCount)
                CompareVal(.CavityFlange.Mat2, txtApp_Mat2_Axial.Text, pCount)

                CompareVal(.CavityFlange.Hard1, ConvertToDbl(txtApp_Hardness1_Axial.Text), pCount)
                CompareVal(.CavityFlange.Hard2, ConvertToDbl(txtApp_Hardness2_Axial.Text), pCount)

                CompareVal(.CavityFlange.SF1, ConvertToDbl(txtApp_SF1_Axial.Text), pCount)
                CompareVal(.CavityFlange.SF2, ConvertToDbl(txtApp_SF2_Axial.Text), pCount)

                CompareVal(.CavityFlange.MeasureSF, cmbAxial_SF_ProcessName.Text, pCount)
                CompareVal(.CavityFlange.UnitSF, cmbAxial_SF_Unit.Text, pCount)

                Dim pblnFlag As Boolean
                If (cmbApp_Static_Axial.Text = "Y") Then
                    pblnFlag = True
                Else
                    pblnFlag = False
                End If
                CompareVal(.Axial.IsStatic, pblnFlag, pCount)

                If (cmbApp_Rotate_Axial.Text = "Y") Then
                    pblnFlag = True
                Else
                    pblnFlag = False
                End If
                CompareVal(.Axial.IsRotating, pblnFlag, pCount)
                CompareVal(.Axial.RPM, ConvertToDbl(txtApp_RotateRPM_Axial.Text), pCount)

                If (cmbApp_Recip_Axial.Text = "Y") Then
                    pblnFlag = True
                Else
                    pblnFlag = False
                End If
                CompareVal(.Axial.IsRecip, pblnFlag, pCount)
                CompareVal(.Axial.Recip_Stroke, ConvertToDbl(txtApp_RecipStrokeL_Axial.Text), pCount)
                CompareVal(.Axial.Recip_V, ConvertToDbl(txtApp_RecipV_Axial.Text), pCount)
                CompareVal(.Axial.Recip_CycleRate, ConvertToDbl(txtApp_RecipCycleRate_Axial.Text), pCount)
                CompareVal(.Axial.Recip_ServiceLife, ConvertToDbl(txtApp_RecipServiceLife_Axial.Text), pCount)

                If (cmbApp_Osc_Axial.Text = "Y") Then
                    pblnFlag = True
                Else
                    pblnFlag = False
                End If
                CompareVal(.Axial.IsOscilatory, pblnFlag, pCount)
                CompareVal(.Axial.Oscilate_Rot, ConvertToDbl(txtApp_OscRot_Axial.Text), pCount)
                CompareVal(.Axial.Oscilate_V, ConvertToDbl(txtApp_OscV_Axial.Text), pCount)
                CompareVal(.Axial.Oscilate_CycleRate, ConvertToDbl(txtApp_OscCycleRate_Axial.Text), pCount)
                CompareVal(.Axial.Oscilate_ServiceLife, ConvertToDbl(txtApp_OscServiceLife_Axial.Text), pCount)

            End With

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "App")
            If (grdApp_EditedBy_Axial.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdApp_EditedBy_Axial.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdApp_EditedBy_Axial.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdApp_EditedBy_Axial.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdApp_EditedBy_Axial.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdApp_EditedBy_Axial.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End If

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Design() As Boolean
        '========================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Design
            CompareVal(.CustDwgNo, txtDesign_CustDwgNo.Text, pCount)
            CompareVal(.CustDwgRev, txtDesign_CustDwgRev.Text, pCount)

            Dim pblnFlag As Boolean
            If (cmbDesign_Frozen.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.Frozen.Design, pblnFlag, pCount)

            If (cmbDesign_Process.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.Frozen.Process, pblnFlag, pCount)

            If (cmbDesign_Class1.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsClass1, pblnFlag, pCount)

            If (cmbDesign_BuildToPrint.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsBuildToPrint, pblnFlag, pCount)

            If (grdDesign_Verification.Rows.Count - 1 <> .Verification.ID_Verification.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .Verification.ID_Verification.Count - 1
                    CompareVal(.Verification.Desc(i), grdDesign_Verification.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.Verification.Owner(i), grdDesign_Verification.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.Verification.Result(i), grdDesign_Verification.Rows(i).Cells(2).Value, pCount)
                Next

            End If

            If (cmbDesign_Winnovation.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsWinnovation, pblnFlag, pCount)
            If (pblnFlag) Then
                CompareVal(.WinnovationNo, txtDesign_WinnovationNo.Text, pCount)
            End If


            If (cmbDesign_OutsideVendor.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsMat_OutsideVender, pblnFlag, pCount)

            If (grdDesign_Input.Rows.Count - 1 <> .Input.ID_Input.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .Input.ID_Input.Count - 1
                    CompareVal(.Input.Desc(i), grdDesign_Input.Rows(i).Cells(0).Value, pCount)
                Next

            End If

            CompareVal(.FOD_Risks, txtDesign_FOD_Risks.Text, pCount)

            '....Page 2
            If (grdDesign_CustSpec.Rows.Count - 1 <> .CustSpec.ID_Cust.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .CustSpec.ID_Cust.Count - 1
                    CompareVal(.CustSpec.Type(i), grdDesign_CustSpec.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.CustSpec.Desc(i), grdDesign_CustSpec.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.CustSpec.Interpret(i), grdDesign_CustSpec.Rows(i).Cells(2).Value, pCount)
                Next
            End If

            CompareVal(.LessonsLearned, txtDesign_LessonsLearned.Text, pCount)

            If (grdDesign_Seal.Rows.Count - 1 <> .SealDim.ID_Seal.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .SealDim.ID_Seal.Count - 1
                    CompareVal(.SealDim.Name(i), grdDesign_Seal.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.SealDim.Min(i), ConvertToDbl(grdDesign_Seal.Rows(i).Cells(1).Value), pCount)
                    CompareVal(.SealDim.Nom(i), ConvertToDbl(grdDesign_Seal.Rows(i).Cells(2).Value), pCount)
                    CompareVal(.SealDim.Max(i), ConvertToDbl(grdDesign_Seal.Rows(i).Cells(3).Value), pCount)
                Next
            End If

            CompareVal(.Notes, txtDesign_Notes.Text, pCount)

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Design")
            If (grdDesign_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdDesign_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdDesign_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdDesign_EditedBy.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdDesign_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdDesign_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Manf() As Boolean
        '========================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Manf
            CompareVal(.BaseMat_PartNo, txtManf_MatPartNo_Base.Text, pCount)
            CompareVal(.SpringMat_PartNo, txtManf_MatPartNo_Spring.Text, pCount)

            CompareVal(.HT, txtManf_HT.Text, pCount)
            CompareVal(.PreComp_Glue, cmbManf_PrecompressionGlue.Text, pCount)

            If (grdManf_ToolNGage.Rows.Count - 1 <> .ToolNGage.ID_Tool.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .ToolNGage.ID_Tool.Count - 1
                    CompareVal(.ToolNGage.PartNo(i), grdManf_ToolNGage.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.ToolNGage.Desc(i), grdManf_ToolNGage.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.ToolNGage.Type(i), grdManf_ToolNGage.Rows(i).Cells(2).Value, pCount)
                    CompareVal(.ToolNGage.Status(i), grdManf_ToolNGage.Rows(i).Cells(3).Value, pCount)
                    CompareVal(.ToolNGage.LeadTime(i), ConvertToDbl(grdManf_ToolNGage.Rows(i).Cells(4).Value), pCount)
                    CompareVal(.ToolNGage.DesignResponsibility(i), grdManf_ToolNGage.Rows(i).Cells(5).Value, pCount)
                Next

            End If

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Manf")
            If (grdManf_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdManf_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdManf_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdManf_EditedBy.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdManf_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdManf_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Purchase() As Boolean
        '=============================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Purchase

            If (grdPurchase_Mat.Rows.Count - 1 <> .Mat.ID_Mat.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .Mat.ID_Mat.Count - 1
                    CompareVal(.Mat.Item(i), grdPurchase_Mat.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.Mat.EstQty(i), ConvertToDbl(grdPurchase_Mat.Rows(i).Cells(1).Value), pCount)    'AES 29MAY18
                    CompareVal(.Mat.Qty_Unit(i), grdPurchase_Mat.Rows(i).Cells(2).Value, pCount)
                    CompareVal(.Mat.Status(i), grdPurchase_Mat.Rows(i).Cells(3).Value, pCount)
                    CompareVal(.Mat.LeadTime(i), ConvertToDbl(grdPurchase_Mat.Rows(i).Cells(4).Value), pCount)
                Next
            End If

            If (grdPurchase_Drawing.Rows.Count - 1 <> .Dwg.ID_Dwg.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .Dwg.ID_Dwg.Count - 1
                    CompareVal(.Dwg.No(i), grdPurchase_Drawing.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.Dwg.Desc(i), grdPurchase_Drawing.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.Dwg.LeadTime(i), ConvertToDbl(grdPurchase_Drawing.Rows(i).Cells(2).Value), pCount)
                Next
            End If

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Purchase")
            If (grdPurchase_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdPurchase_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdPurchase_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdPurchase_EditedBy.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdPurchase_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdPurchase_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Qlty() As Boolean
        '===========================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Qlty

            Dim pblnFlag As Boolean
            If (cmbQuality_ApprovedSupplier.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.IsApvdSupplierOnly, pblnFlag, pCount)

            If (cmbQuality_TNG.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.Separate_Tool_Gage_Reqd, pblnFlag, pCount)

            If (cmbQuality_CustComplaint.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.HasCustComplaint, pblnFlag, pCount)

            CompareVal(.Reason, txtQuality_Reason.Text, pCount)

            If (cmbQuality_VisualInspection.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.VisualInspection, pblnFlag, pCount)

            If (pblnFlag) Then
                CompareVal(.VisualInspection_Type, cmbQuality_VisualInspection_Type.Text, pCount)
            End If

            If (cmbQuality_SPC.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.SPC_Reqd, pblnFlag, pCount)

            If (cmbQuality_GageRnR_Reqd.Text = "Y") Then
                pblnFlag = True
            Else
                pblnFlag = False
            End If
            CompareVal(.GageRnR_Reqd, pblnFlag, pCount)


            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Qlty")
            If (grdQuality_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdQuality_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdQuality_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdQuality_EditedBy.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdQuality_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdQuality_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_DWG() As Boolean
        '=========================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Dwg

            CompareVal(.DesignLevel, cmbDwg_DesignLevel.Text, pCount)

            If (grdDrawing_Needed.Rows.Count - 1 <> .Needed.ID_Needed.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .Needed.ID_Needed.Count - 1
                    CompareVal(.Needed.DwgNo(i), grdDrawing_Needed.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.Needed.Desc(i), grdDrawing_Needed.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.Needed.Status(i), grdDrawing_Needed.Rows(i).Cells(2).Value, pCount)
                    CompareVal(.Needed.LeadTime(i), ConvertToDbl(grdDrawing_Needed.Rows(i).Cells(3).Value), pCount)
                Next
            End If

            If (grdDrawing_BOM.Rows.Count - 1 <> .BOM.ID_BOM.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To .BOM.ID_BOM.Count - 1
                    CompareVal(.BOM.Parent_PartNo(i), grdDrawing_BOM.Rows(i).Cells(0).Value, pCount)
                    CompareVal(.BOM.Child_PartNo(i), grdDrawing_BOM.Rows(i).Cells(1).Value, pCount)
                    CompareVal(.BOM.Qty(i).ToString(), grdDrawing_BOM.Rows(i).Cells(2).Value, pCount)
                Next
            End If

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "DWG")
            If (grdDwg_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdDwg_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdDwg_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdDwg_EditedBy.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdDwg_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdDwg_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Test() As Boolean
        '=========================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Test

            '....Leak
            CompareVal(.Leak.Compress_Unplated, ConvertToDbl(txtTest_CompressPre_Leak.Text), pCount)
            CompareVal(.Leak.Compress_Plated, ConvertToDbl(txtTest_CompressPost_Leak.Text), pCount)

            CompareVal(.Leak.Medium_Unplated, cmbTest_MediaPre_Leak.Text, pCount)
            CompareVal(.Leak.Medium_Plated, cmbTest_MediaPost_Leak.Text, pCount)

            CompareVal(.Leak.Press_Unplated, ConvertToDbl(txtTest_PressPre_Leak.Text), pCount)
            CompareVal(.Leak.Press_Plated, ConvertToDbl(txtTest_PressPost_Leak.Text), pCount)

            CompareVal(.Leak.Max_Unplated, ConvertToDbl(txtTest_ReqPre_Leak.Text), pCount)
            CompareVal(.Leak.Max_Plated, ConvertToDbl(txtTest_ReqPost_Leak.Text), pCount)

            Dim pVal As Integer = 0
            If (cmbTest_QtyPre_Leak.Text <> "") Then
                pVal = Convert.ToInt32(cmbTest_QtyPre_Leak.Text)
            End If
            CompareVal(.Leak.Qty_Unplated, pVal, pCount)

            pVal = 0
            If (cmbTest_QtyPost_Leak.Text <> "") Then
                pVal = Convert.ToInt32(cmbTest_QtyPost_Leak.Text)
            End If
            CompareVal(.Leak.Qty_Plated, pVal, pCount)

            CompareVal(.Leak.Freq_Unplated, cmbTest_FreqPre_Leak.Text, pCount)
            CompareVal(.Leak.Freq_Plated, cmbTest_FreqPost_Leak.Text, pCount)

            '....Load
            CompareVal(.Load.Compress_Unplated, ConvertToDbl(txtTest_CompressPre_Load.Text), pCount)
            CompareVal(.Load.Compress_Plated, ConvertToDbl(txtTest_CompressPost_Load.Text), pCount)

            CompareVal(.Load.Max_Unplated, ConvertToDbl(txtTest_ReqPre_Load.Text), pCount)
            CompareVal(.Load.Max_Plated, ConvertToDbl(txtTest_ReqPost_Load.Text), pCount)

            pVal = 0
            If (cmbTest_QtyPre_Load.Text <> "") Then
                pVal = Convert.ToInt32(cmbTest_QtyPre_Load.Text)
            End If
            CompareVal(.Load.Qty_Unplated, pVal, pCount)

            pVal = 0
            If (cmbTest_QtyPost_Load.Text <> "") Then
                pVal = Convert.ToInt32(cmbTest_QtyPost_Load.Text)
            End If
            CompareVal(.Load.Qty_Plated, pVal, pCount)

            CompareVal(.Load.Freq_Unplated, cmbTest_FreqPre_Load.Text, pCount)
            CompareVal(.Load.Freq_Plated, cmbTest_FreqPost_Load.Text, pCount)

            '....SpringBack
            CompareVal(.SpringBack.Compress_Unplated, ConvertToDbl(txtTest_CompressPre_SpringBack.Text), pCount)
            CompareVal(.SpringBack.Compress_Plated, ConvertToDbl(txtTest_CompressPost_SpringBack.Text), pCount)

            CompareVal(.SpringBack.Max_Unplated, ConvertToDbl(txtTest_ReqPre_SpringBack.Text), pCount)
            CompareVal(.SpringBack.Max_Plated, ConvertToDbl(txtTest_ReqPost_SpringBack.Text), pCount)

            pVal = 0
            If (cmbTest_QtyPre_SpringBack.Text <> "") Then
                pVal = Convert.ToInt32(cmbTest_QtyPre_SpringBack.Text)
            End If
            CompareVal(.SpringBack.Qty_Unplated, pVal, pCount)

            pVal = 0
            If (cmbTest_QtyPost_SpringBack.Text <> "") Then
                pVal = Convert.ToInt32(cmbTest_QtyPost_SpringBack.Text)
            End If
            CompareVal(.SpringBack.Qty_Plated, pVal, pCount)

            CompareVal(.SpringBack.Freq_Unplated, cmbTest_FreqPre_SpringBack.Text, pCount)
            CompareVal(.SpringBack.Freq_Plated, cmbTest_FreqPost_SpringBack.Text, pCount)

            CompareVal(.Other, txtTest_Other.Text, pCount)

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Test")
            If (grdTest_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdTest_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdTest_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdTest_EditedBy.Rows(i).Cells(0).Value)
                    End If
                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdTest_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdTest_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Function CompareVal_Shipping() As Boolean
        '============================================
        Dim pCI As New CultureInfo("en-US")
        Dim pblnValChanged As Boolean = False
        Dim pCount As Integer = 0
        Dim pDateVal As Date = DateTime.MinValue

        With mProcess_Project.Shipping

            CompareVal(.Notes, txtShipping_Notes.Text, pCount)

            mProcess_Project.EditedBy.RetrieveFromDB(mProcess_Project.ID, "Shipping")
            If (grdShipping_EditedBy.Rows.Count <> mProcess_Project.EditedBy.ID_Edit.Count) Then
                pCount = pCount + 1
            Else
                For i As Integer = 0 To mProcess_Project.EditedBy.ID_Edit.Count - 1
                    pDateVal = DateTime.MinValue
                    If (grdShipping_EditedBy.Rows(i).Cells(0).Value <> "" And Not IsNothing(grdShipping_EditedBy.Rows(i).Cells(0).Value)) Then
                        pDateVal = Convert.ToDateTime(grdShipping_EditedBy.Rows(i).Cells(0).Value)
                    End If

                    CompareVal(mProcess_Project.EditedBy.DateEdited(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pDateVal.ToString("MM/dd/yyyy", pCI.DateTimeFormat()), pCount)
                    CompareVal(mProcess_Project.EditedBy.Name(i), grdShipping_EditedBy.Rows(i).Cells(1).Value, pCount)
                    CompareVal(mProcess_Project.EditedBy.Comment(i), grdShipping_EditedBy.Rows(i).Cells(2).Value, pCount)
                Next
            End If

        End With

        If (pCount > 0) Then
            pblnValChanged = True
        End If

        Return pblnValChanged

    End Function

    Private Sub Delete_Record(ByVal GrdView_In As DataGridView, ByVal RowIndex_In As Integer)
        '====================================================================================
        Dim pintAnswer As Integer

        If (GrdView_In.AllowUserToAddRows) Then
            If (RowIndex_In <> GrdView_In.Rows.Count - 1) Then
                pintAnswer = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                If pintAnswer = Windows.Forms.DialogResult.Yes Then
                    GrdView_In.Rows.RemoveAt(RowIndex_In)
                End If

            End If

        Else
            pintAnswer = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If pintAnswer = Windows.Forms.DialogResult.Yes Then
                GrdView_In.Rows.RemoveAt(RowIndex_In)
            End If
        End If

    End Sub

#End Region

#End Region

#Region "GROUP BOX EVENT ROUTINES:"
    Private Sub grpRefPN_MouseHover(sender As Object, e As EventArgs) Handles grpRefPN.MouseHover
        '========================================================================================
        ToolTip1.SetToolTip(grpRefPN, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub grpRefPN_MouseLeave(sender As Object, e As EventArgs) Handles grpRefPN.MouseLeave
        '========================================================================================
        'grpRefPN.Focus()        'AES 24APR18
    End Sub

    Private Sub TabControl2_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                 Handles TabControl2.SelectedIndexChanged
        '================================================================================
        optDesign_Cust.Checked = True

        txtMenu.Focus()
        txtMenu.Select()

        ''If (TabControl2.SelectedIndex = 0) Then
        ''    txtParkerPart.Focus()
        ''    txtParkerPart.Select()
        ''ElseIf (TabControl2.SelectedIndex = 1) Then
        ''    txtParkerPart.Focus()
        ''    txtParkerPart.Select()
        ''End If

    End Sub

    Private Sub tbTest_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tbTest.SelectedIndexChanged
        '========================================================================================================
        optTest_Cust.Checked = True

        txtMenu.Focus()
        txtMenu.Select()

        ''If (tbTest.SelectedIndex = 0) Then
        ''    txtParkerPart.Focus()
        ''    txtParkerPart.Select()
        ''ElseIf (tbTest.SelectedIndex = 1) Then
        ''    txtParkerPart.Focus()
        ''    txtParkerPart.Select()
        ''ElseIf (tbTest.SelectedIndex = 2) Then
        ''    txtParkerPart.Focus()
        ''    txtParkerPart.Select()
        ''End If

    End Sub

    Private Sub grdApproval_Attendees_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles grdApproval_Attendees.CellClick
        '================================================================================================================================   'AES 02APR18
        If (Not IsNothing(grdApproval_Attendees.CurrentCell)) Then

            Dim pRowIndex As Integer = grdApproval_Attendees.CurrentCell.RowIndex
            Dim pUserName As String = gUser.FirstName + " " + gUser.LastName
            If (gUser.Role = grdApproval_Attendees.Rows(pRowIndex).Cells(0).Value) Then

                If (grdApproval_Attendees.Rows(pRowIndex).Cells(3).Value = True) Then
                    If (pUserName = grdApproval_Attendees.Rows(pRowIndex).Cells(1).Value) Then
                        grdApproval_Attendees.ReadOnly = False
                        'grdApproval_Attendees.EnableHeadersVisualStyles = False
                        'grdApproval_Attendees.Rows(pRowIndex).HeaderCell.Style.BackColor = Color.Green

                    Else
                        grdApproval_Attendees.ReadOnly = True
                    End If
                Else
                    grdApproval_Attendees.ReadOnly = False
                    'grdApproval_Attendees.EnableHeadersVisualStyles = False
                    'grdApproval_Attendees.Rows(pRowIndex).HeaderCell.Style.BackColor = Color.Green

                End If
                'grdApproval_Attendees.ReadOnly = False
            Else
                grdApproval_Attendees.ReadOnly = True
            End If

        End If

        grdApproval_Attendees.Columns(3).ReadOnly = True

    End Sub

    Private Sub chkHeaderUserSigned_CheckedChanged(sender As Object, e As EventArgs) _
                                                   Handles chkHeaderUserSigned.CheckedChanged
        '====================================================================================
        SetControls_HeaderUserSign()

    End Sub

    Private Sub cmdUserSign_Click(sender As Object, e As EventArgs) Handles _
                                  cmdPreOrderUserSign.Click, cmdITAR_Export_UserSign.Click,
                                  cmdOrdEntry_UserSign.Click, cmdCost_UserSign.Click,
                                  cmdApp_UserSign_Face.Click, cmdDesign_UserSign.Click,
                                  cmdManf_UserSign.Click, cmdPurchase_UserSign.Click,
                                  cmdQuality_UserSign.Click, cmdDwg_UserSign.Click,
                                  cmdTest_UserSign.Click, cmdPlanning_UserSign.Click,
                                  cmdShipping_UserSign.Click
        '======================================================================================
        Dim pCmdButton As Button = CType(sender, Button)

        Select Case pCmdButton.Name

            Case "cmdPreOrderUserSign"
                '---------------------
                If (chkPreOrderUserSigned.Checked) Then
                    chkPreOrderUserSigned.Checked = False
                    cmdPreOrderUserSign.Text = "Sign-off"
                Else
                    chkPreOrderUserSigned.Checked = True
                    cmdPreOrderUserSign.Text = "Signed-off"
                End If

            Case "cmdITAR_Export_UserSign"
                '---------------------
                If (chkITAR_Export_UserSigned.Checked) Then
                    chkITAR_Export_UserSigned.Checked = False
                    cmdITAR_Export_UserSign.Text = "Sign-off"
                Else
                    chkITAR_Export_UserSigned.Checked = True
                    cmdITAR_Export_UserSign.Text = "Signed-off"
                End If

            Case "cmdOrdEntry_UserSign"
                '----------------------
                If (chkOrdEntry_UserSigned.Checked) Then
                    chkOrdEntry_UserSigned.Checked = False
                    cmdOrdEntry_UserSign.Text = "Sign-off"
                Else
                    chkOrdEntry_UserSigned.Checked = True
                    cmdOrdEntry_UserSign.Text = "Signed-off"
                End If

            Case "cmdCost_UserSign"
                '------------------
                If (chkCost_UserSigned.Checked) Then
                    chkCost_UserSigned.Checked = False
                    cmdCost_UserSign.Text = "Sign-off"
                Else
                    chkCost_UserSigned.Checked = True
                    cmdCost_UserSign.Text = "Signed-off"
                End If

            Case "cmdApp_UserSign_Face"
                '---------------------
                If (chkApp_UserSigned_Face.Checked) Then
                    chkApp_UserSigned_Face.Checked = False
                    cmdApp_UserSign_Face.Text = "Sign-off"
                Else
                    chkApp_UserSigned_Face.Checked = True
                    cmdApp_UserSign_Face.Text = "Signed-off"
                End If

            Case "cmdDesign_UserSign"
                '------------------
                If (chkDesign_UserSigned.Checked) Then
                    chkDesign_UserSigned.Checked = False
                    cmdDesign_UserSign.Text = "Sign-off"
                Else
                    chkDesign_UserSigned.Checked = True
                    cmdDesign_UserSign.Text = "Signed-off"
                End If

            Case "cmdManf_UserSign"
                '------------------
                If (chkManf_UserSigned.Checked) Then
                    chkManf_UserSigned.Checked = False
                    cmdManf_UserSign.Text = "Sign-off"
                Else
                    chkManf_UserSigned.Checked = True
                    cmdManf_UserSign.Text = "Signed-off"
                End If

            Case "cmdPurchase_UserSign"
                '----------------------
                If (chkPurchase_UserSigned.Checked) Then
                    chkPurchase_UserSigned.Checked = False
                    cmdPurchase_UserSign.Text = "Sign-off"
                Else
                    chkPurchase_UserSigned.Checked = True
                    cmdPurchase_UserSign.Text = "Signed-off"
                End If

            Case "cmdQuality_UserSign"
                '---------------------
                If (chkQuality_UserSigned.Checked) Then
                    chkQuality_UserSigned.Checked = False
                    cmdQuality_UserSign.Text = "Sign-off"
                Else
                    chkQuality_UserSigned.Checked = True
                    cmdQuality_UserSign.Text = "Signed-off"
                End If

            Case "cmdDwg_UserSign"
                '------------------
                If (chkDwg_UserSigned.Checked) Then
                    chkDwg_UserSigned.Checked = False
                    cmdDwg_UserSign.Text = "Sign-off"
                Else
                    chkDwg_UserSigned.Checked = True
                    cmdDwg_UserSign.Text = "Signed-off"
                End If

            Case "cmdTest_UserSign"
                '------------------
                If (chkTest_UserSigned.Checked) Then
                    chkTest_UserSigned.Checked = False
                    cmdTest_UserSign.Text = "Sign-off"
                Else
                    chkTest_UserSigned.Checked = True
                    cmdTest_UserSign.Text = "Signed-off"
                End If

            Case "cmdPlanning_UserSign"
                '----------------------
                If (chkPlanning_UserSigned.Checked) Then
                    chkPlanning_UserSigned.Checked = False
                    cmdPlanning_UserSign.Text = "Sign-off"
                Else
                    chkPlanning_UserSigned.Checked = True
                    cmdPlanning_UserSign.Text = "Signed-off"
                End If

            Case "cmdShipping_UserSign"
                '------------------
                If (chkShipping_UserSigned.Checked) Then
                    chkShipping_UserSigned.Checked = False
                    cmdShipping_UserSign.Text = "Sign-off"
                Else
                    chkShipping_UserSigned.Checked = True
                    cmdShipping_UserSign.Text = "Signed-off"
                End If
        End Select

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        '=================================================================================      'AES 19APR18
        Dim pSealProcessEntities As New SealProcessDBEntities()

        Dim pProcessProjectCount As Integer = (From ProcessProject In pSealProcessEntities.tblProcessProject
                                               Where ProcessProject.fldID = mProcess_Project.ID).Count()

        If (pProcessProjectCount > 0) Then

            Dim pintAnswer As Integer
            pintAnswer = MessageBox.Show("Are you sure you want to permanently delete this project data?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If pintAnswer = Windows.Forms.DialogResult.Yes Then
                Dim pProcessProject = (From ProcessProject In pSealProcessEntities.tblProcessProject
                                       Where ProcessProject.fldID = mProcess_Project.ID).First()
                pSealProcessEntities.DeleteObject(pProcessProject)
                pSealProcessEntities.SaveChanges()

                mProcess_Project.ID = 0
                RetrieveFromDB()
                DisplayData()
            End If


        End If

    End Sub

    Public Sub ClearForm(Frm As Form)
        '=================
        Dim Ctr As Control

        Try

            For Each Ctr In Frm.Controls
                If TypeOf Ctr Is TextBox Then
                    Ctr.Text = ""
                ElseIf TypeOf Ctr Is ComboBox Then
                    If CType(Ctr, ComboBox).DropDownStyle = ComboBoxStyle.DropDownList Then
                        CType(Ctr, ComboBox).SelectedIndex = -1

                    Else CType(Ctr, ComboBox).DropDownStyle = ComboBoxStyle.DropDown
                        CType(Ctr, ComboBox).Text = ""

                    End If

                ElseIf TypeOf Ctr Is CheckBox Then
                    CType(Ctr, CheckBox).Checked = False

                ElseIf TypeOf Ctr Is DataGridView Then
                    CType(Ctr, DataGridView).Rows.Clear()

                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub mnuAnalytics_Click(sender As Object, e As EventArgs) Handles mnuAnalytics.Click
        '======================================================================================

        Cursor = Cursors.WaitCursor

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing

        Dim pAnalyticsFileName As String = gProcessFile.AnalysisFileName

        pWkbOrg = pApp.Workbooks.Open(pAnalyticsFileName, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value,
                                        Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                        Missing.Value, Missing.Value, Missing.Value)

        pApp.Visible = True

        pApp.WindowState = EXCEL.XlWindowState.xlMinimized
        pApp.WindowState = EXCEL.XlWindowState.xlMaximized

        Cursor = Cursors.Default

    End Sub

    Private Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged
        '===========================================================================================================
        mProcess_Project.Type = cmbType.Text

        If (Not IsNothing(mProcess_Project.Type) And mProcess_Project.Type <> "") Then
            cmbType.SelectedIndex = 1

        Else
            cmbType.SelectedIndex = -1
        End If


    End Sub


    Private Sub cmbType_DrawItem(sender As Object, e As DrawItemEventArgs) Handles cmbType.DrawItem

        'If (cmbType.SelectedIndex > -1) Then
        '    Dim text As String = Me.cmbType.GetItemText(cmbType.Items(e.Index))
        '    e.DrawBackground()
        '    Using br As SolidBrush = New SolidBrush(e.ForeColor)
        '        e.Graphics.DrawString(text, e.Font, br, e.Bounds)
        '    End Using

        '    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
        '        Me.ToolTip1.Show(text, cmbType, e.Bounds.Right, e.Bounds.Bottom)
        '    Else
        '        Me.ToolTip1.Hide(cmbType)
        '    End If

        '    e.DrawFocusRectangle()
        'End If

    End Sub

    Private Sub TabControl3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl3.SelectedIndexChanged
        '====================================================================================================================

        txtMenu.Focus()
        txtMenu.Select()

        'If (TabControl3.SelectedIndex = 0) Then
        '    txtMenu.Focus()
        '    txtMenu.Select()

        'ElseIf (TabControl3.SelectedIndex = 1) Then
        '    txtMenu.Focus()
        '    txtMenu.Select()
        'End If
    End Sub


    Private Sub SetControls_HeaderUserSign()
        '===================================
        Dim pCI As New CultureInfo("en-US")

        If (chkHeaderUserSigned.Checked) Then
            chkHeaderUserSigned.Text = "Signed-off"
            SetTabPrivilege()

            '....Header disabled
            mHeader = False
            cmdDelete.Enabled = mHeader

            '....Enable all tabs according to User Previlege
            Initialize_tbTesting_Controls()
            ReInitializeControls()
            TabControl1.Refresh()
            txtHeaderUserName.Text = gUser.FirstName + " " + gUser.LastName
            txtDateMod.Text = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            txtModifiedBy.Text = gUser.FirstName + " " + gUser.LastName
        Else
            chkHeaderUserSigned.Text = "Sign-off"

            chkPreOrderUserSigned.Checked = False
            chkITAR_Export_UserSigned.Checked = False
            chkOrdEntry_UserSigned.Checked = False
            chkCost_UserSigned.Checked = False
            chkApp_UserSigned_Face.Checked = False
            chkDesign_UserSigned.Checked = False
            chkManf_UserSigned.Checked = False
            chkPurchase_UserSigned.Checked = False
            chkQuality_UserSigned.Checked = False
            chkDwg_UserSigned.Checked = False
            chkTest_UserSigned.Checked = False
            chkPlanning_UserSigned.Checked = False
            chkShipping_UserSigned.Checked = False

            For i As Integer = 0 To grdApproval_Attendees.Rows.Count - 1
                grdApproval_Attendees.Rows(i).Cells(3).Value = False
                grdApproval_Attendees.Rows(i).Cells(4).Value = ""
            Next

            If (mTabIndex.Contains(99)) Then
                '....Header enabled
                mHeader = True
                txtHeaderUserName.Text = ""

                Dim pSealProcessEntities As New SealProcessDBEntities()
                Dim pProcessProjectCount As Integer = (From ProcessProject In pSealProcessEntities.tblProcessProject
                                                       Where ProcessProject.fldID = mProcess_Project.ID And ProcessProject.fldLastModifiedBy <> "").Count()

                If (pProcessProjectCount > 0) Then
                    cmdDelete.Enabled = True
                Else
                    cmdDelete.Enabled = False
                End If

            End If

            '....Disable all tabs according to User Previlege
            mTabIndex.Clear()
            If (mHeader) Then
                mTabIndex.Add(99)       'AES 18APR18
            End If

            mPreOrder = False
            mExport = False
            mOrdEntry = False
            mCost = False
            mApp = False
            mDesign = False
            mManf = False
            mPurchase = False
            mQlty = False
            mDwg = False
            mTest = False
            mPlanning = False
            mShipping = False
            mKeyChar = False
            mIssue = False
            EnableTab(tabIssue, False)
            EnableTab(tabApproval, False)
            ReInitializeControls()
            TabControl1.Refresh()

        End If
    End Sub

    Private Sub grpCoating_MouseHover(sender As Object, e As EventArgs) Handles grpCoating.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(grpCoating, "Enter Data in SealPart.")
        'cmdSealPart.Focus()         'AES 24APR18
    End Sub

    Private Sub grpCoating_MouseLeave(sender As Object, e As EventArgs) Handles grpCoating.MouseLeave
        '============================================================================================
        'grpCoating.Focus()      'AES 24APR18
    End Sub


#End Region

#Region "OPTION BUTTON RELATED ROUTINES:"
    Private Sub optApp_Parker_Gen_CheckedChanged(sender As Object, e As EventArgs) _
                                                Handles optApp_Parker_Gen.CheckedChanged
        '=================================================================================
        If (optApp_Parker_Gen.Checked) Then
            ConvVal_PH()
        End If

    End Sub

    Private Sub optApp_Cust_Gen_CheckedChanged(sender As Object, e As EventArgs) _
                                               Handles optApp_Cust_Gen.CheckedChanged
        '==============================================================================
        If (optApp_Cust_Gen.Checked) Then
            ConvVal_Cust()
        End If

    End Sub

    Private Sub optApp_Parker_Face_CheckedChanged(sender As Object, e As EventArgs) _
                                                  Handles optApp_Parker_Face.CheckedChanged
        '==================================================================================
        If (optApp_Parker_Face.Checked) Then
            ConvVal_PH()
        End If

    End Sub

    Private Sub optApp_Cust_Face_CheckedChanged(sender As Object, e As EventArgs) _
                                                Handles optApp_Cust_Face.CheckedChanged
        '==============================================================================
        If (optApp_Cust_Face.Checked) Then
            ConvVal_Cust()
        End If

    End Sub

    Private Sub optDesign_Parker_CheckedChanged(sender As Object, e As EventArgs) _
                                                Handles optDesign_Parker.CheckedChanged
        '================================================================================
        If (optDesign_Parker.Checked) Then
            ConvVal_PH()
        End If
    End Sub

    Private Sub optDesign_Cust_CheckedChanged(sender As Object, e As EventArgs) _
                                              Handles optDesign_Cust.CheckedChanged
        '============================================================================
        If (optDesign_Cust.Checked) Then
            ConvVal_Cust()
        End If
    End Sub

    Private Sub optTest_Parker_CheckedChanged(sender As Object, e As EventArgs) _
                                              Handles optTest_Parker.CheckedChanged
        '============================================================================
        If (optTest_Parker.Checked) Then
            ConvVal_PH()
        End If
    End Sub

    Private Sub optTest_Cust_CheckedChanged(sender As Object, e As EventArgs) _
                                            Handles optTest_Cust.CheckedChanged
        '=======================================================================
        If (optTest_Cust.Checked) Then
            ConvVal_Cust()
        End If
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub ConvVal_PH()
        '===================
        SetLabel_Unit_PH()

        If (txtApp_MaxLeak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtApp_MaxLeak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust)
            txtApp_MaxLeak.Text = gUnit.Format_LeakVal(pVal) 'gUnit.Format_Val(pVal) 'Convert.ToDouble(txtApp_MaxLeak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.000")
        End If

        For i As Integer = 0 To grdApp_OpCond.Rows.Count - 1
            For j As Integer = 1 To grdApp_OpCond.Columns.Count - 1
                If (Not IsNothing(grdApp_OpCond.Rows(i).Cells(j).Value)) Then
                    If (i = 0) Then
                        If (grdApp_OpCond.Rows(i).Cells(j).Value.ToString() <> "") Then
                            grdApp_OpCond.Rows(i).Cells(j).Value = gUnit.Format_Val(gUnit.ConvFToC(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value))) ' gUnit.ConvFToC(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value)).ToString("#0.0")
                        End If

                    Else
                        If (grdApp_OpCond.Rows(i).Cells(j).Value.ToString() <> "") Then
                            Dim pVal As Double = (Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust))
                            grdApp_OpCond.Rows(i).Cells(j).Value = gUnit.Format_Val(pVal) '(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)).ToString("#0.000")
                        End If
                    End If

                End If
            Next
        Next

        Dim pF_Fact As Double = gUnit.ConvF("F", mProcess_Project.Unit.FIndx_PH, mProcess_Project.Unit.FIndx_Cust)
        Dim pL_Fact As Double = gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
        Dim pFact As Double = pF_Fact / pL_Fact

        For i As Integer = 0 To grdApp_Load.Rows.Count - 1
            For j As Integer = 1 To grdApp_Load.Columns.Count - 1
                If (Not IsNothing(grdApp_Load.Rows(i).Cells(j).Value)) Then

                    'Dim pF_Fact As Double = gUnit.ConvF("F", mProcess_Project.Unit.FIndx_PH, mProcess_Project.Unit.FIndx_Cust)
                    'Dim pL_Fact As Double = gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
                    'Dim pFact As Double = pF_Fact / pL_Fact

                    If (grdApp_Load.Rows(i).Cells(j).Value.ToString() <> "") Then
                        Dim pVal As Double = (Convert.ToDouble(grdApp_Load.Rows(i).Cells(j).Value) * pFact)
                        grdApp_Load.Rows(i).Cells(j).Value = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(grdApp_Load.Rows(i).Cells(j).Value) * pFact).ToString("#0.000")
                    End If

                End If
            Next
        Next

        For i As Integer = 0 To grdApp_Face_Cavity.Rows.Count - 1
            For j As Integer = 1 To grdApp_Face_Cavity.Columns.Count - 1
                If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(j).Value)) Then

                    If (grdApp_Face_Cavity.Rows(i).Cells(j).Value.ToString() <> "") Then
                        Dim pVal As Double = (Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(j).Value) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust))
                        grdApp_Face_Cavity.Rows(i).Cells(j).Value = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(j).Value) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)).ToString("#0.000")
                    End If

                End If
            Next
        Next

        For i As Integer = 0 To grdDesign_Seal.Rows.Count - 1
            For j As Integer = 1 To grdDesign_Seal.Columns.Count - 1
                If (Not IsNothing(grdDesign_Seal.Rows(i).Cells(j).Value)) Then

                    If (grdDesign_Seal.Rows(i).Cells(j).Value.ToString() <> "") Then
                        Dim pVal As Double = (Convert.ToDouble(grdDesign_Seal.Rows(i).Cells(j).Value) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust))
                        grdDesign_Seal.Rows(i).Cells(j).Value = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(grdDesign_Seal.Rows(i).Cells(j).Value) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)).ToString("#0.000")
                    End If

                End If
            Next
        Next

        '....Leak
        If (txtTest_CompressPre_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPre_Leak.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPre_Leak.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPre_Leak.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_CompressPost_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPost_Leak.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPost_Leak.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPost_Leak.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_PressPre_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_PressPre_Leak.Text) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)
            txtTest_PressPre_Leak.Text = gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_PressPre_Leak.Text) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_PressPost_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_PressPost_Leak.Text) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)
            txtTest_PressPost_Leak.Text = gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_PressPost_Leak.Text) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPre_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPre_Leak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust)
            txtTest_ReqPre_Leak.Text = gUnit.Format_LeakVal(pVal) 'gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_ReqPre_Leak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPost_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPost_Leak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
            txtTest_ReqPost_Leak.Text = gUnit.Format_LeakVal(pVal) 'gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_ReqPost_Leak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
        End If

        '....Load
        If (txtTest_CompressPre_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPre_Load.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPre_Load.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPre_Load.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_CompressPost_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPost_Load.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPost_Load.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPost_Load.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPre_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPre_Load.Text) * pFact
            txtTest_ReqPre_Load.Text = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(txtTest_ReqPre_Load.Text) * pFact).ToString("#0.#00")
        End If

        If (txtTest_ReqPost_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPost_Load.Text) * pFact
            txtTest_ReqPost_Load.Text = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(txtTest_ReqPost_Load.Text) * pFact).ToString("#0.#00")
        End If

        '....SpringBack
        If (txtTest_CompressPre_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPre_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPre_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPre_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_CompressPost_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPost_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPost_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPost_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPre_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPre_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_ReqPre_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_ReqPre_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPost_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPost_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_ReqPost_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_ReqPost_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

    End Sub

    Private Sub ConvVal_Cust()
        '===================
        SetLabel_Unit_Cust()
        If (txtApp_MaxLeak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtApp_MaxLeak.Text) / gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust)
            txtApp_MaxLeak.Text = gUnit.Format_LeakVal(pVal) 'gUnit.Format_Val(pVal) 'Convert.ToDouble(txtApp_MaxLeak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.000")
        End If

        For i As Integer = 0 To grdApp_OpCond.Rows.Count - 1
            For j As Integer = 1 To grdApp_OpCond.Columns.Count - 1
                If (Not IsNothing(grdApp_OpCond.Rows(i).Cells(j).Value)) Then
                    If (i = 0) Then
                        If (grdApp_OpCond.Rows(i).Cells(j).Value.ToString() <> "") Then
                            grdApp_OpCond.Rows(i).Cells(j).Value = gUnit.Format_Val(gUnit.ConvCToF(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value))) ' gUnit.ConvFToC(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value)).ToString("#0.0")
                        End If

                    Else
                        If (grdApp_OpCond.Rows(i).Cells(j).Value.ToString() <> "") Then
                            Dim pVal As Double = (Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value) / gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust))
                            grdApp_OpCond.Rows(i).Cells(j).Value = gUnit.Format_Val(pVal) '(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)).ToString("#0.000")
                        End If
                    End If

                End If
            Next
        Next

        Dim pF_Fact As Double = gUnit.ConvF("F", mProcess_Project.Unit.FIndx_PH, mProcess_Project.Unit.FIndx_Cust)
        Dim pL_Fact As Double = gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
        Dim pFact As Double = pF_Fact / pL_Fact

        For i As Integer = 0 To grdApp_Load.Rows.Count - 1
            For j As Integer = 1 To grdApp_Load.Columns.Count - 1
                If (Not IsNothing(grdApp_Load.Rows(i).Cells(j).Value)) Then

                    'Dim pF_Fact As Double = gUnit.ConvF("F", mProcess_Project.Unit.FIndx_PH, mProcess_Project.Unit.FIndx_Cust)
                    'Dim pL_Fact As Double = gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
                    'Dim pFact As Double = pF_Fact / pL_Fact

                    If (grdApp_Load.Rows(i).Cells(j).Value.ToString() <> "") Then
                        Dim pVal As Double = (Convert.ToDouble(grdApp_Load.Rows(i).Cells(j).Value) / pFact)
                        grdApp_Load.Rows(i).Cells(j).Value = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(grdApp_Load.Rows(i).Cells(j).Value) * pFact).ToString("#0.000")
                    End If

                End If
            Next
        Next

        For i As Integer = 0 To grdApp_Face_Cavity.Rows.Count - 1
            For j As Integer = 1 To grdApp_Face_Cavity.Columns.Count - 1
                If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(j).Value)) Then

                    If (grdApp_Face_Cavity.Rows(i).Cells(j).Value.ToString() <> "") Then
                        Dim pVal As Double = (Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(j).Value) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust))
                        grdApp_Face_Cavity.Rows(i).Cells(j).Value = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(j).Value) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)).ToString("#0.000")
                    End If

                End If
            Next
        Next

        For i As Integer = 0 To grdDesign_Seal.Rows.Count - 1
            For j As Integer = 1 To grdDesign_Seal.Columns.Count - 1
                If (Not IsNothing(grdDesign_Seal.Rows(i).Cells(j).Value)) Then

                    If (grdDesign_Seal.Rows(i).Cells(j).Value.ToString() <> "") Then
                        Dim pVal As Double = (Convert.ToDouble(grdDesign_Seal.Rows(i).Cells(j).Value) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust))
                        grdDesign_Seal.Rows(i).Cells(j).Value = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(grdDesign_Seal.Rows(i).Cells(j).Value) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)).ToString("#0.000")
                    End If

                End If
            Next
        Next

        '....Leak
        If (txtTest_CompressPre_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPre_Leak.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPre_Leak.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPre_Leak.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_CompressPost_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPost_Leak.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPost_Leak.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPost_Leak.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_PressPre_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_PressPre_Leak.Text) / gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)
            txtTest_PressPre_Leak.Text = gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_PressPre_Leak.Text) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_PressPost_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_PressPost_Leak.Text) / gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)
            txtTest_PressPost_Leak.Text = gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_PressPost_Leak.Text) * gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPre_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPre_Leak.Text) / gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust)
            txtTest_ReqPre_Leak.Text = gUnit.Format_LeakVal(pVal) 'gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_ReqPre_Leak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPost_Leak.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPost_Leak.Text) / gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
            txtTest_ReqPost_Leak.Text = gUnit.Format_LeakVal(pVal) 'gUnit.Format_Val(pVal) 'Convert.ToDouble(txtTest_ReqPost_Leak.Text) * gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
        End If

        '....Load
        If (txtTest_CompressPre_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPre_Load.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPre_Load.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPre_Load.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_CompressPost_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPost_Load.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPost_Load.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPost_Load.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPre_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPre_Load.Text) / pFact
            txtTest_ReqPre_Load.Text = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(txtTest_ReqPre_Load.Text) * pFact).ToString("#0.#00")
        End If

        If (txtTest_ReqPost_Load.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPost_Load.Text) / pFact
            txtTest_ReqPost_Load.Text = Format(pVal, gUnit.LFormat) '(Convert.ToDouble(txtTest_ReqPost_Load.Text) * pFact).ToString("#0.#00")
        End If

        '....SpringBack
        If (txtTest_CompressPre_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPre_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPre_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPre_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_CompressPost_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_CompressPost_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_CompressPost_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_CompressPost_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPre_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPre_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_ReqPre_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_ReqPre_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

        If (txtTest_ReqPost_SpringBack.Text <> "") Then
            Dim pVal As Double = Convert.ToDouble(txtTest_ReqPost_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
            txtTest_ReqPost_SpringBack.Text = Format(pVal, gUnit.LFormat) 'Convert.ToDouble(txtTest_ReqPost_SpringBack.Text) * gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
        End If

    End Sub

    'Private Sub ConvVal_Cust()
    '    '===================
    '    SetLabel_Unit_Cust()
    '    If (txtApp_MaxLeak.Text <> "") Then
    '        txtApp_MaxLeak.Text = Convert.ToDouble(txtApp_MaxLeak.Text) / gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.000")
    '    End If

    '    For i As Integer = 0 To grdApp_OpCond.Rows.Count - 1
    '        For j As Integer = 1 To grdApp_OpCond.Columns.Count - 1
    '            If (Not IsNothing(grdApp_OpCond.Rows(i).Cells(j).Value)) Then
    '                If (i = 0) Then
    '                    If (grdApp_OpCond.Rows(i).Cells(j).Value.ToString() <> "") Then
    '                        grdApp_OpCond.Rows(i).Cells(j).Value = gUnit.ConvCToF(Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value)).ToString("#0.0")
    '                    End If

    '                Else
    '                    If (grdApp_OpCond.Rows(i).Cells(j).Value.ToString() <> "") Then
    '                        grdApp_OpCond.Rows(i).Cells(j).Value = (Convert.ToDouble(grdApp_OpCond.Rows(i).Cells(j).Value) / gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust)).ToString("#0.000")
    '                    End If
    '                End If

    '            End If
    '        Next
    '    Next


    '    Dim pF_Fact As Double = gUnit.ConvF("F", mProcess_Project.Unit.FIndx_PH, mProcess_Project.Unit.FIndx_Cust)
    '    Dim pL_Fact As Double = gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)
    '    Dim pFact As Double = pF_Fact / pL_Fact

    '    For i As Integer = 0 To grdApp_Load.Rows.Count - 1
    '        For j As Integer = 1 To grdApp_Load.Columns.Count - 1
    '            If (Not IsNothing(grdApp_Load.Rows(i).Cells(j).Value)) Then

    '                If (grdApp_Load.Rows(i).Cells(j).Value.ToString() <> "") Then
    '                    grdApp_Load.Rows(i).Cells(j).Value = (Convert.ToDouble(grdApp_Load.Rows(i).Cells(j).Value) / pFact).ToString("#0.000")
    '                End If

    '            End If
    '        Next
    '    Next

    '    For i As Integer = 0 To grdApp_Face_Cavity.Rows.Count - 1
    '        For j As Integer = 1 To grdApp_Face_Cavity.Columns.Count - 1
    '            If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(j).Value)) Then

    '                If (grdApp_Face_Cavity.Rows(i).Cells(j).Value.ToString() <> "") Then
    '                    grdApp_Face_Cavity.Rows(i).Cells(j).Value = (Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(j).Value) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)).ToString("#0.000")
    '                End If

    '            End If
    '        Next
    '    Next

    '    For i As Integer = 0 To grdDesign_Seal.Rows.Count - 1
    '        For j As Integer = 1 To grdDesign_Seal.Columns.Count - 1
    '            If (Not IsNothing(grdDesign_Seal.Rows(i).Cells(j).Value)) Then

    '                If (grdDesign_Seal.Rows(i).Cells(j).Value.ToString() <> "") Then
    '                    grdDesign_Seal.Rows(i).Cells(j).Value = (Convert.ToDouble(grdDesign_Seal.Rows(i).Cells(j).Value) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust)).ToString("#0.000")
    '                End If

    '            End If
    '        Next
    '    Next

    '    '....Leak
    '    If (txtTest_CompressPre_Leak.Text <> "") Then
    '        txtTest_CompressPre_Leak.Text = Convert.ToDouble(txtTest_CompressPre_Leak.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_CompressPost_Leak.Text <> "") Then
    '        txtTest_CompressPost_Leak.Text = Convert.ToDouble(txtTest_CompressPost_Leak.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_PressPre_Leak.Text <> "") Then
    '        txtTest_PressPre_Leak.Text = Convert.ToDouble(txtTest_PressPre_Leak.Text) / gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_PressPost_Leak.Text <> "") Then
    '        txtTest_PressPost_Leak.Text = Convert.ToDouble(txtTest_PressPost_Leak.Text) / gUnit.ConvF("P", mProcess_Project.Unit.PIndx_PH, mProcess_Project.Unit.PIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_ReqPre_Leak.Text <> "") Then
    '        txtTest_ReqPre_Leak.Text = Convert.ToDouble(txtTest_ReqPre_Leak.Text) / gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_ReqPost_Leak.Text <> "") Then
    '        txtTest_ReqPost_Leak.Text = Convert.ToDouble(txtTest_ReqPost_Leak.Text) / gUnit.ConvF("Leak", mProcess_Project.Unit.LeakIndx_PH, mProcess_Project.Unit.LeakIndx_Cust).ToString("#0.#00")
    '    End If

    '    '....Load
    '    If (txtTest_CompressPre_Load.Text <> "") Then
    '        txtTest_CompressPre_Load.Text = Convert.ToDouble(txtTest_CompressPre_Load.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_CompressPost_Load.Text <> "") Then
    '        txtTest_CompressPost_Load.Text = Convert.ToDouble(txtTest_CompressPost_Load.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_ReqPre_Load.Text <> "") Then

    '        txtTest_ReqPre_Load.Text = (Convert.ToDouble(txtTest_ReqPre_Load.Text) / pFact).ToString("#0.#00")
    '    End If

    '    If (txtTest_ReqPost_Load.Text <> "") Then
    '        txtTest_ReqPost_Load.Text = (Convert.ToDouble(txtTest_ReqPost_Load.Text) / pFact).ToString("#0.#00")
    '    End If

    '    '....SpringBack
    '    If (txtTest_CompressPre_SpringBack.Text <> "") Then
    '        txtTest_CompressPre_SpringBack.Text = Convert.ToDouble(txtTest_CompressPre_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_CompressPost_SpringBack.Text <> "") Then
    '        txtTest_CompressPost_SpringBack.Text = Convert.ToDouble(txtTest_CompressPost_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_ReqPre_SpringBack.Text <> "") Then
    '        txtTest_ReqPre_SpringBack.Text = Convert.ToDouble(txtTest_ReqPre_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    '    If (txtTest_ReqPost_SpringBack.Text <> "") Then
    '        txtTest_ReqPost_SpringBack.Text = Convert.ToDouble(txtTest_ReqPost_SpringBack.Text) / gUnit.ConvF("L", mProcess_Project.Unit.LIndx_PH, mProcess_Project.Unit.LIndx_Cust).ToString("#0.#00")
    '    End If

    'End Sub

#End Region

#End Region

#Region "DATA VALIDATION UTILITY ROUTINES:"
    Private Function CleanInputNumber(ByVal str As String) As String
        '===========================================================
        Return System.Text.RegularExpressions.Regex.Replace(str, "[a-zA-Z\b\s-.@~`!#$%^&*()_//?<>:;+={}/[\]'""]", "")
    End Function

    Private Function CleanInputNummeric(ByVal str As String) As String
        '===========================================================
        Return System.Text.RegularExpressions.Regex.Replace(str, "[a-zA-Z\b\s-@~`!#$%^&*()_//?<>:;+={}/[\]'""]", "")
    End Function

#End Region


    'Private Sub FileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles mnuUserGroup.Click
    '    '=============================================================================================================
    '    With openFileDialog1

    '        .Filter = "SealSuite DataFile Use (*.xls)|*.xls"
    '        .FilterIndex = 1
    '        .InitialDirectory = gProcessFile.DirUserData
    '        .FileName = ""
    '        .Title = "Open"

    '        If .ShowDialog = Windows.Forms.DialogResult.OK Then
    '            Dim pUserDataFileName As String = .FileName
    '            Cursor.Current = Cursors.WaitCursor
    '            mProcess_Project.User.UpdateTo_DB(pUserDataFileName)
    '            Cursor.Current = Cursors.Default
    '        End If
    '    End With
    'End Sub

    'Private Sub CompareVal(VarOrg_In As String, VarMod_In As String, ByRef Count As Integer)
    '    '===================================================================================
    '    If (IsNothing(VarOrg_In)) Then
    '        VarOrg_In = ""
    '    End If

    '    If (IsNothing(VarMod_In)) Then
    '        VarMod_In = ""
    '    End If

    '    If Trim(VarOrg_In) <> Trim(VarMod_In) Then Count += 1

    'End Sub

    'Private Sub CompareVal(VarOrg_In As Integer, VarMod_In As Integer, ByRef Count As Integer)

    '    If VarOrg_In <> VarMod_In Then Count += 1
    'End Sub

    'Private Sub CompareVal(VarOrg_In As Double, VarMod_In As Double, ByRef Count As Integer)
    '    If Math.Abs(VarOrg_In - VarMod_In) >= gcEPS Then Count += 1
    'End Sub

    'Private Sub CompareVal(VarOrg_In As Boolean, VarMod_In As Boolean, ByRef Count As Integer)
    '    If VarOrg_In <> VarMod_In Then Count += 1
    'End Sub

End Class