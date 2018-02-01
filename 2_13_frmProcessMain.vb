'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmMain                        '
'                        VERSION NO  :  1.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB18                                '
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

    Dim mDateTimePicker As DateTimePicker

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
            '.Add("Quoted")
            .Add("NPL")
            '.Add("ECO")
            '.Add("Production")
            '.Add("Inactive")
            '.Add("Replaced")
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
            .Add("")
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
            .Add("Product Drive -Part")
            .Add("Product Drive -Customer")
            .Add("Enovia")
        End With

        With cmbRFQPkgLoc.Items
            .Clear()
            .Add("Product Drive -Part")
            .Add("Product Drive -Customer")
            .Add("Enovia")
        End With

    End Sub

#End Region

#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub Process_ProductInfo_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===================================================================================================

        InitializeControls()

        GetPartProjectInfo()

        Initialize_tbTesting_Controls()

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

        '....Move the vertical scrollbar at the Top
        txtParkerPart.Focus()
        txtParkerPart.Select()
        gIsProcessMainActive = True

    End Sub

    Private Sub Process_frmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        '=======================================================================================
        Dim pCI As New CultureInfo("en-US")
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
            SetLabel_Unit()
        End If

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
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
        cmbType.SelectedIndex = 0
        cmbExport_Status.SelectedIndex = 0
        cmbPartFamily.SelectedIndex = 0
        cmbPartType.SelectedIndex = 0
        cmbPreOrderChannel.SelectedIndex = 1

        '....Pre-Order
        PopulateMarketingMgr()

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

        '........Coating
        If (pSealType = "E") Then
            grpCoating.Enabled = True

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
                cmbCoating.Enabled = False
            End If

            '........Populate Surface Finish Combo Box.
            PopulateCmbSFinish()

            If gPartProject.PNR.HW.Coating <> "" And gPartProject.PNR.HW.Coating <> "None" Then
                cmbCoating.Enabled = True
                cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
                cmbCoating.Text = gPartProject.PNR.HW.Coating

                If cmbCoating.Text = "T800" Then
                    lblSFinish.Enabled = True
                    cmbSFinish.Enabled = True
                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                Else
                    lblSFinish.Enabled = False
                    cmbSFinish.Enabled = False
                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                    cmbSFinish.Text = ""
                End If

            Else
                cmbCoating.Enabled = False
                cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
                cmbCoating.Text = ""
                lblSFinish.Enabled = False
                cmbSFinish.Enabled = False
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
            grpCoating.Enabled = False
        End If

        '....Plating
        If (pSealType = "C" Or pSealType = "SC") Then
            grpPlating.Enabled = True
            chkPlating.Checked = False
            cmbPlatingCode.Enabled = True
            cmbPlatingThickCode.Enabled = True

        Else
            grpPlating.Enabled = False
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

            For i As Integer = 0 To pQryRole.Count - 1
                grdApproval_Attendees.Rows(i).Cells(0).Value = pQryRole(i).fldRole
                Dim pRoleID As Integer = pQryRole(i).fldID

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

                    grdApproval_Attendees.Item(1, i) = dgvcc

                End If
            Next

        End If

        grdApproval_Attendees.AllowUserToAddRows = False

    End Sub

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
        mProcess_Project.Approval.RetrieveFromDB(mProcess_Project.ID)

    End Sub


    Private Sub DisplayData()
        '====================

        '.... "General:"
        txtParkerPart.Text = gPartProject.PNR.PN
        txtPN_Rev.Text = gPartProject.PNR.PN_Rev

        txtCustomer.Text = gPartProject.CustInfo.CustName
        txtCustomerPN.Text = gPartProject.CustInfo.PN_Cust
        txtCustomerPN_Rev.Text = gPartProject.CustInfo.PN_Cust_Rev

        With mProcess_Project
            cmbPopCoding.Text = .POPCoding

            cmbRating.Text = .Rating
            cmbType.Text = .Type
            Dim pCI As New CultureInfo("en-US")
            If (.DateOpen <> DateTime.MinValue) Then
                txtStartDate.Text = .DateOpen.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
            Else
                txtStartDate.Text = ""
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
            Dim pCI As New CultureInfo("en-US")
            cmbMgrPreOrder.Text = .Mgr.Mkt
            txtMgrSales.Text = .Mgr.Sales

            If (.Export.Reqd) Then
                cmbExport_Reqd.Text = "Y"
                cmbExport_Status.Text = .Export.Status
            Else
                cmbExport_Reqd.Text = "N"
                cmbExport_Status.Text = ""
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

            cmbPartType.Text = .Part.Type

            cmbPreOrderSeg.Text = .Mkt.Seg
            cmbPreOrderChannel.Text = .Mkt.Channel
            txtPreOrderNotes.Text = .Notes

            cmbCostFileLoc.Text = .Loc.CostFile
            cmbRFQPkgLoc.Text = .Loc.RFQPkg
            txtPreOrderPriceNotes.Text = .Notes_Price

            If (.EditedBy.User.Signed) Then
                chkPreOrderUserSigned.Checked = True
                txtPreOrderUserName.Text = .EditedBy.User.Name

                'Dim pCI As New CultureInfo("en-US")
                If (.EditedBy.User.DateSigned <> DateTime.MinValue) Then
                    txtPreOrderUserDate.Text = .EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                Else
                    txtPreOrderUserDate.Text = ""
                End If

                'txtPreOrderUserDate.Text = .User.SignedDate.ToShortDateString()


            Else
                chkPreOrderUserSigned.Checked = False
                txtPreOrderUserName.Text = .EditedBy.User.Name
                'txtPreOrderUserDate.Text = .User.SignedDate.ToShortDateString()

                'Dim pCI As New CultureInfo("en-US")
                If (.EditedBy.User.DateSigned <> DateTime.MinValue) Then
                    txtPreOrderUserDate.Text = .EditedBy.User.DateSigned.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                Else
                    txtPreOrderUserDate.Text = ""
                End If
            End If

            '....Cust Contact Pre-Order
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
            For j As Integer = 0 To .SalesData.Year.Count - 1
                grdPreOrder_SalesData.Rows.Add()
                grdPreOrder_SalesData.Rows(j).Cells(0).Value = .SalesData.Year(j)
                grdPreOrder_SalesData.Rows(j).Cells(1).Value = .SalesData.Qty(j)
                grdPreOrder_SalesData.Rows(j).Cells(2).Value = .SalesData.Price(j)
                grdPreOrder_SalesData.Rows(j).Cells(3).Value = .SalesData.Total(j)
            Next

        End With


        '.... "ITAR_Export:"

        With mProcess_Project.ITAR_Export

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
            Else
                cmbITAR_Export_ProductITAR_Reg.Text = "N"
                txtExportControlled.Text = "N"
                txtExportStatus.Text = ""
            End If

            If (.SaleExportControlled) Then
                cmbITAR_Export_SaleExportControlled.Text = "Y"
            Else
                cmbITAR_Export_SaleExportControlled.Text = "N"
            End If

            txtITAR_Export_ITAR_Classification.Text = .ITAR_Class
            txtITAR_Export_EAR_Classification.Text = .EAR_Class
            cmbITAR_Export_Status.Text = .Status
            txtITAR_Export_HTS_Classification.Text = .HTS_Class

            If (.EditedBy.User.Signed) Then
                chkITAR_Export_UserSigned.Checked = True
                txtITAR_Export_UserName.Text = .EditedBy.User.Name
                txtITAR_Export_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()

            Else
                chkITAR_Export_UserSigned.Checked = False
                txtITAR_Export_UserName.Text = .EditedBy.User.Name
                txtITAR_Export_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
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
            End If

            If (.DatePO_EDI <> DateTime.MinValue) Then
                txtOrdEntry_PODate_EDI.Text = .DatePO_EDI.ToShortDateString()
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

            If (.EditedBy.User.Signed) Then
                chkOrdEntry_UserSigned.Checked = True
                txtOrdEntry_UserName.Text = .EditedBy.User.Name
                txtOrdEntry_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
            Else
                chkOrdEntry_UserSigned.Checked = False
                txtOrdEntry_UserName.Text = .EditedBy.User.Name
                txtOrdEntry_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
            End If

        End With


        '.... "Cost Estimating:"
        With mProcess_Project.Cost
            cmbCost_QuoteFile.Text = .QuoteFileLoc
            txtCost_Notes.Text = .Notes

            '....Spl Operation
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
                grdCost_SplOperation.Rows(j).Cells(2).Value = .SplOperation.LeadTime(j)
                grdCost_SplOperation.Rows(j).Cells(3).Value = .SplOperation.Cost(j).ToString("#.00")
            Next

            If (.EditedBy.User.Signed) Then
                chkCost_UserSigned.Checked = True
                txtCost_UserName.Text = .EditedBy.User.Name
                txtCost_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
            Else
                chkCost_UserSigned.Checked = False
                txtCost_UserName.Text = .EditedBy.User.Name
                txtCost_UserDate.Text = .EditedBy.User.DateSigned.ToShortDateString()
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
                txtApp_MaxLeak.Text = .MaxLeak.ToString("##0.000")
            Else
                txtApp_MaxLeak.Text = ""
            End If

            If (.IsPressCyclic) Then
                cmbApp_PressCycle.Text = "Y"
                txtApp_PressCycleFreq.Enabled = True
                txtApp_PressCycleFreq.Text = .PressCycle_Freq.ToString("##0.000")
                txtApp_PressCycleAmp.Text = .PressCycle_Amp.ToString("##0.000")
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

            grdApp_OpCond.Rows.Add()
            grdApp_OpCond.Rows.Add()
            grdApp_OpCond.Rows(0).Cells(0).Value = "Temperature"
            grdApp_OpCond.Rows(1).Cells(0).Value = "Pressure"
            grdApp_OpCond.Columns(0).ReadOnly = True
            grdApp_OpCond.AllowUserToAddRows = False

            If (Math.Abs(.OpCond.T.Assy) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(1).Value = .OpCond.T.Assy.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(1).Value = ""
            End If

            If (Math.Abs(.OpCond.T.Min) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(2).Value = .OpCond.T.Min.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(2).Value = ""
            End If

            If (Math.Abs(.OpCond.T.Max) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(3).Value = .OpCond.T.Max.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(3).Value = ""
            End If

            If (Math.Abs(.OpCond.T.Oper) > gcEPS) Then
                grdApp_OpCond.Rows(0).Cells(4).Value = .OpCond.T.Oper.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(0).Cells(4).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Assy) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(1).Value = .OpCond.Press.Assy.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(1).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Min) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(2).Value = .OpCond.Press.Min.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(2).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Max) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(3).Value = .OpCond.Press.Max.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(3).Value = ""
            End If

            If (Math.Abs(.OpCond.Press.Oper) > gcEPS) Then
                grdApp_OpCond.Rows(1).Cells(4).Value = .OpCond.Press.Oper.ToString("##0.0")
            Else
                grdApp_OpCond.Rows(1).Cells(4).Value = ""
            End If

            grdApp_Load.Rows.Add()
            grdApp_Load.Rows.Add()
            grdApp_Load.Rows(0).Cells(0).Value = "Assembly"
            grdApp_Load.Rows(1).Cells(0).Value = "Operating"
            grdApp_Load.Columns(0).ReadOnly = True
            grdApp_Load.AllowUserToAddRows = False

            If (Math.Abs(.Load.Assy.Min) > gcEPS) Then
                grdApp_Load.Rows(0).Cells(1).Value = .Load.Assy.Min.ToString("##0.0")
            Else
                grdApp_Load.Rows(0).Cells(1).Value = ""
            End If

            If (Math.Abs(.Load.Assy.Max) > gcEPS) Then
                grdApp_Load.Rows(0).Cells(2).Value = .Load.Assy.Max.ToString("##0.0")
            Else
                grdApp_Load.Rows(0).Cells(2).Value = ""
            End If

            If (Math.Abs(.Load.Oper.Min) > gcEPS) Then
                grdApp_Load.Rows(1).Cells(1).Value = .Load.Oper.Min.ToString("##0.0")
            Else
                grdApp_Load.Rows(1).Cells(1).Value = ""
            End If

            If (Math.Abs(.Load.Oper.Max) > gcEPS) Then
                grdApp_Load.Rows(1).Cells(2).Value = .Load.Oper.Max.ToString("##0.0")
            Else
                grdApp_Load.Rows(1).Cells(2).Value = ""
            End If

            '....Face Seal
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
                    grdApp_Face_Cavity.Rows(j).Cells(1).Value = .Cavity.Assy(j).Min.ToString("##0.000")
                    grdApp_Face_Cavity.Rows(j).Cells(2).Value = .Cavity.Assy(j).Max.ToString("##0.000")
                    grdApp_Face_Cavity.Rows(j).Cells(3).Value = .Cavity.Oper(j).Min.ToString("##0.000")
                    grdApp_Face_Cavity.Rows(j).Cells(4).Value = .Cavity.Oper(j).Max.ToString("##0.000")
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
                    txtApp_Hardness1_Face.Text = .CavityFlange.Hard1.ToString("##0.000")
                Else
                    txtApp_Hardness1_Face.Text = ""
                End If

                If (Math.Abs(.CavityFlange.Hard2) > gcEPS) Then
                    txtApp_Hardness2_Face.Text = .CavityFlange.Hard2.ToString("##0.000")
                Else
                    txtApp_Hardness2_Face.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF1) > gcEPS) Then
                    txtApp_SF1_Face.Text = .CavityFlange.SF1.ToString("##0.000")
                Else
                    txtApp_SF1_Face.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF2) > gcEPS) Then
                    txtApp_SF2_Face.Text = .CavityFlange.SF2.ToString("##0.000")
                Else
                    txtApp_SF2_Face.Text = ""
                End If

                cmbFace_SF_ProcessName.Text = .CavityFlange.MeasureSF
                cmbFace_SF_Unit.Text = .CavityFlange.UnitSF


                cmbApp_Face_POrient.Text = gPartProject.PNR.HW.POrient '.Face.POrient       'AES 09JAN18

                If (Math.Abs(.Face.MaxFlangeSep) > gcEPS) Then
                    txtApp_Face_MaxFlangeSeparation.Text = .Face.MaxFlangeSep.ToString("##0.000")
                Else
                    txtApp_Face_MaxFlangeSeparation.Text = ""
                End If

            ElseIf (.Type = "Axial") Then
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
                    grdApp_Axial_Cavity.Rows(j).Cells(1).Value = .Cavity.Assy(j).Min.ToString("##0.000")
                    grdApp_Axial_Cavity.Rows(j).Cells(2).Value = .Cavity.Assy(j).Max.ToString("##0.000")
                    grdApp_Axial_Cavity.Rows(j).Cells(3).Value = .Cavity.Oper(j).Min.ToString("##0.000")
                    grdApp_Axial_Cavity.Rows(j).Cells(4).Value = .Cavity.Oper(j).Max.ToString("##0.000")
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
                    txtApp_Hardness1_Axial.Text = .CavityFlange.Hard1.ToString("##0.000")
                Else
                    txtApp_Hardness1_Axial.Text = ""
                End If

                If (Math.Abs(.CavityFlange.Hard2) > gcEPS) Then
                    txtApp_Hardness2_Axial.Text = .CavityFlange.Hard2.ToString("##0.000")
                Else
                    txtApp_Hardness2_Axial.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF1) > gcEPS) Then
                    txtApp_SF1_Axial.Text = .CavityFlange.SF1.ToString("##0.000")
                Else
                    txtApp_SF1_Axial.Text = ""
                End If

                If (Math.Abs(.CavityFlange.SF2) > gcEPS) Then
                    txtApp_SF2_Axial.Text = .CavityFlange.SF2.ToString("##0.000")
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
                        txtApp_RotateRPM_Axial.Text = .Axial.RPM.ToString("##0")
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
                        txtApp_RecipStrokeL_Axial.Text = .Axial.Recip_Stroke.ToString("##0")
                    Else
                        txtApp_RecipStrokeL_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Recip_V)) Then
                        txtApp_RecipV_Axial.Text = .Axial.Recip_V.ToString("##0")
                    Else
                        txtApp_RecipV_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Recip_CycleRate)) Then
                        txtApp_RecipCycleRate_Axial.Text = .Axial.Recip_CycleRate.ToString("##0")
                    Else
                        txtApp_RecipCycleRate_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Recip_ServiceLife)) Then
                        txtApp_RecipServiceLife_Axial.Text = .Axial.Recip_ServiceLife.ToString("##0")
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
                        txtApp_OscRot_Axial.Text = .Axial.Oscilate_Rot.ToString("##0")
                    Else
                        txtApp_OscRot_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Oscilate_V)) Then
                        txtApp_OscV_Axial.Text = .Axial.Oscilate_V.ToString("##0")
                    Else
                        txtApp_OscV_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Oscilate_CycleRate)) Then
                        txtApp_OscCycleRate_Axial.Text = .Axial.Oscilate_CycleRate.ToString("##0")
                    Else
                        txtApp_OscCycleRate_Axial.Text = ""
                    End If

                    If (Math.Abs(.Axial.Oscilate_ServiceLife)) Then
                        txtApp_OscServiceLife_Axial.Text = .Axial.Oscilate_ServiceLife.ToString("##0")
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
            Else
                cmbDesign_Winnovation.Text = "N"
                txtDesign_WinnovationNo.Text = ""
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
                    grdDesign_Seal.Rows(i).Cells(1).Value = .SealDim.Min(i)
                Else
                    grdDesign_Seal.Rows(i).Cells(1).Value = ""
                End If

                If (Math.Abs(.SealDim.Nom(i)) > gcEPS) Then
                    grdDesign_Seal.Rows(i).Cells(2).Value = .SealDim.Nom(i)
                Else
                    grdDesign_Seal.Rows(i).Cells(2).Value = .SealDim.Nom(i)
                End If

                If (Math.Abs(.SealDim.Max(i)) > gcEPS) Then
                    grdDesign_Seal.Rows(i).Cells(3).Value = .SealDim.Max(i)
                Else
                    grdDesign_Seal.Rows(i).Cells(3).Value = .SealDim.Max(i)
                End If

            Next
            txtDesign_LessonsLearned.Text = .LessonsLearned
            txtDesign_Notes.Text = .Notes

        End With


        '.... "Manufacturing:"

        With mProcess_Project.Manf

            txtManf_MatPartNo_Base.Text = .BaseMat_PartNo
            txtManf_MatPartNo_Spring.Text = .SpringMat_PartNo
            txtManf_HT.Text = .HT
            cmbManf_PrecompressionGlue.Text = .PreComp_Glue

            '....Desc
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
                    grdManf_ToolNGage.Rows(i).Cells(4).Value = .ToolNGage.LeadTime(i)
                Else
                    grdManf_ToolNGage.Rows(i).Cells(4).Value = ""
                End If

                grdManf_ToolNGage.Rows(i).Cells(5).Value = .ToolNGage.DesignResponsibility(i)
            Next


            '....Purchasing
            For i As Integer = 0 To .ToolNGage.ID_Tool.Count - 1
                If (.ToolNGage.Status(i) = "Buy") Then
                    grdPurchase_ToolNGages.Rows.Add()
                    grdPurchase_ToolNGages.Rows(i).Cells(0).Value = .ToolNGage.PartNo(i)
                    grdPurchase_ToolNGages.Rows(i).Cells(1).Value = .ToolNGage.Desc(i)
                    grdPurchase_ToolNGages.Rows(i).Cells(2).Value = .ToolNGage.Type(i)
                    'grdPurchase_ToolNGages.Rows(i).Cells(3).Value = .ToolNGage.Status(i)

                    If (Math.Abs(.ToolNGage.LeadTime(i)) > gcEPS) Then
                        grdPurchase_ToolNGages.Rows(i).Cells(3).Value = .ToolNGage.LeadTime(i)
                    Else
                        grdPurchase_ToolNGages.Rows(i).Cells(3).Value = ""
                    End If

                    grdPurchase_ToolNGages.Rows(i).Cells(4).Value = .ToolNGage.DesignResponsibility(i)
                End If
            Next
            grdPurchase_ToolNGages.AllowUserToAddRows = False
            grdPurchase_ToolNGages.Enabled = False

        End With


        '....Purchasing
        With mProcess_Project.Purchase

            For i As Integer = 0 To .Mat.ID_Mat.Count - 1
                grdPurchase_Mat.Rows.Add()
                grdPurchase_Mat.Rows(i).Cells(0).Value = .Mat.Item(i)
                If (Math.Abs(.Mat.EstQty(i)) > gcEPS) Then
                    grdPurchase_Mat.Rows(i).Cells(1).Value = .Mat.EstQty(i)
                Else
                    grdPurchase_Mat.Rows(i).Cells(1).Value = ""
                End If

                grdPurchase_Mat.Rows(i).Cells(2).Value = .Mat.Status(i)

                If (Math.Abs(.Mat.LeadTime(i)) > gcEPS) Then
                    grdPurchase_Mat.Rows(i).Cells(3).Value = .Mat.LeadTime(i)
                Else
                    grdPurchase_Mat.Rows(i).Cells(3).Value = ""
                End If

            Next

            'For j As Integer = 0 To .ToolNGage.ID_Tool.Count - 2
            '    grdPurchase_ToolNGages.Rows.Add()
            'Next

            'For i As Integer = 0 To .ToolNGage.ID_Tool.Count - 1
            '    grdPurchase_ToolNGages.Rows.Add()
            '    grdPurchase_ToolNGages.Rows(i).Cells(0).Value = .ToolNGage.PartNo(i)
            '    grdPurchase_ToolNGages.Rows(i).Cells(1).Value = .ToolNGage.Desc(i)
            '    grdPurchase_ToolNGages.Rows(i).Cells(2).Value = .ToolNGage.Type(i)

            '    If (Math.Abs(.ToolNGage.LeadTime(i)) > gcEPS) Then
            '        grdPurchase_ToolNGages.Rows(i).Cells(4).Value = .ToolNGage.LeadTime(i)
            '    Else
            '        grdPurchase_ToolNGages.Rows(i).Cells(4).Value = ""
            '    End If

            '    grdPurchase_ToolNGages.Rows(i).Cells(5).Value = .ToolNGage.DesignResponsibility(i)
            'Next

            'For j As Integer = 0 To .Dwg.ID_Dwg.Count - 2
            '    grdPurchase_Drawing.Rows.Add()
            'Next

            For i As Integer = 0 To .Dwg.ID_Dwg.Count - 1
                grdPurchase_Drawing.Rows.Add()
                grdPurchase_Drawing.Rows(i).Cells(0).Value = .Dwg.No(i)
                grdPurchase_Drawing.Rows(i).Cells(1).Value = .Dwg.Desc(i)

                If (Math.Abs(.Dwg.LeadTime(i)) > gcEPS) Then
                    grdPurchase_Drawing.Rows(i).Cells(2).Value = .Dwg.LeadTime(i).ToString("#0.0")
                Else
                    grdPurchase_Drawing.Rows(i).Cells(2).Value = ""
                End If

            Next

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
                grdQuality_SplOperation.Rows(j).Cells(2).Value = mProcess_Project.Cost.SplOperation.LeadTime(j)
                grdQuality_SplOperation.Rows(j).Cells(3).Value = mProcess_Project.Cost.SplOperation.Cost(j).ToString("#.00")
            Next

        End With

        '.... "Drawing:"
        With mProcess_Project.Dwg
            cmbDwg_DesignLevel.Text = .DesignLevel

            For j As Integer = 0 To .Needed.ID_Needed.Count - 1
                grdDrawing_Needed.Rows.Add()
                grdDrawing_Needed.Rows(j).Cells(0).Value = .Needed.DwgNo(j)
                grdDrawing_Needed.Rows(j).Cells(1).Value = .Needed.Desc(j)
                grdDrawing_Needed.Rows(j).Cells(2).Value = .Needed.Status(j)

                If (Math.Abs(.Needed.LeadTime(j)) > gcEPS) Then
                    grdDrawing_Needed.Rows(j).Cells(3).Value = .Needed.LeadTime(j)
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
                txtTest_CompressPre_Leak.Text = .Leak.Compress_Unplated.ToString("##0.000")
            Else
                txtTest_CompressPre_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Compress_Plated) > gcEPS) Then
                txtTest_CompressPost_Leak.Text = .Leak.Compress_Plated.ToString("##0.000")
            Else
                txtTest_CompressPost_Leak.Text = ""
            End If

            cmbTest_MediaPre_Leak.Text = .Leak.Medium_Unplated
            cmbTest_MediaPost_Leak.Text = .Leak.Medium_Plated

            If (Math.Abs(.Leak.Press_Unplated) > gcEPS) Then
                txtTest_PressPre_Leak.Text = .Leak.Press_Unplated.ToString("##0.000")
            Else
                txtTest_PressPre_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Press_Plated) > gcEPS) Then
                txtTest_PressPost_Leak.Text = .Leak.Press_Plated.ToString("##0.000")
            Else
                txtTest_PressPost_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Max_Unplated) > gcEPS) Then
                txtTest_ReqPre_Leak.Text = .Leak.Max_Unplated.ToString("##0.000")
            Else
                txtTest_ReqPre_Leak.Text = ""
            End If

            If (Math.Abs(.Leak.Max_Plated) > gcEPS) Then
                txtTest_ReqPost_Leak.Text = .Leak.Max_Plated.ToString("##0.000")
            Else
                txtTest_ReqPost_Leak.Text = ""
            End If

            cmbTest_QtyPre_Leak.Text = .Leak.Qty_Unplated
            cmbTest_QtyPost_Leak.Text = .Leak.Qty_Plated

            cmbTest_FreqPre_Leak.Text = .Leak.Freq_Unplated
            cmbTest_FreqPost_Leak.Text = .Leak.Freq_Plated

            '...Load
            If (Math.Abs(.Load.Compress_Unplated) > gcEPS) Then
                txtTest_CompressPre_Load.Text = .Load.Compress_Unplated.ToString("##0.000")
            Else
                txtTest_CompressPre_Load.Text = ""
            End If

            If (Math.Abs(.Load.Compress_Plated) > gcEPS) Then
                txtTest_CompressPost_Load.Text = .Load.Compress_Plated.ToString("##0.000")
            Else
                txtTest_CompressPost_Load.Text = ""
            End If

            If (Math.Abs(.Load.Max_Unplated) > gcEPS) Then
                txtTest_ReqPre_Load.Text = .Load.Max_Unplated.ToString("##0.000")
            Else
                txtTest_ReqPre_Load.Text = ""
            End If

            If (Math.Abs(.Load.Max_Plated) > gcEPS) Then
                txtTest_ReqPost_Load.Text = .Load.Max_Plated.ToString("##0.000")
            Else
                txtTest_ReqPost_Load.Text = ""
            End If

            cmbTest_QtyPre_Load.Text = .Load.Qty_Unplated
            cmbTest_QtyPost_Load.Text = .Load.Qty_Plated

            cmbTest_FreqPre_Load.Text = .Load.Freq_Unplated
            cmbTest_FreqPost_Load.Text = .Load.Freq_Plated

            '....SpringBack
            If (Math.Abs(.SpringBack.Compress_Unplated) > gcEPS) Then
                txtTest_CompressPre_SpringBack.Text = .SpringBack.Compress_Unplated.ToString("##0.000")
            Else
                txtTest_CompressPre_SpringBack.Text = ""
            End If

            If (Math.Abs(.SpringBack.Compress_Plated) > gcEPS) Then
                txtTest_CompressPost_SpringBack.Text = .SpringBack.Compress_Plated.ToString("##0.000")
            Else
                txtTest_CompressPost_SpringBack.Text = ""
            End If

            If (Math.Abs(.SpringBack.Max_Unplated) > gcEPS) Then
                txtTest_ReqPre_SpringBack.Text = .SpringBack.Max_Unplated.ToString("##0.000")
            Else
                txtTest_ReqPre_SpringBack.Text = ""
            End If

            If (Math.Abs(.SpringBack.Max_Plated) > gcEPS) Then
                txtTest_ReqPost_SpringBack.Text = .SpringBack.Max_Plated.ToString("##0.000")
            Else
                txtTest_ReqPost_SpringBack.Text = ""
            End If

            cmbTest_QtyPre_SpringBack.Text = .SpringBack.Qty_Unplated
            cmbTest_QtyPost_SpringBack.Text = .SpringBack.Qty_Plated

            cmbTest_FreqPre_SpringBack.Text = .SpringBack.Freq_Unplated
            cmbTest_FreqPost_SpringBack.Text = .SpringBack.Freq_Plated

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

            For i As Integer = 0 To mProcess_Project.Design.CustSpec.ID_Cust.Count - 1
                grdShipping_CustSpec.Rows.Add()
                grdShipping_CustSpec.Rows(i).Cells(0).Value = mProcess_Project.Design.CustSpec.Desc(i)
                grdShipping_CustSpec.Rows(i).Cells(1).Value = mProcess_Project.Design.CustSpec.Interpret(i)
            Next
            grdShipping_CustSpec.AllowUserToAddRows = False
            grpCustSpec_Shipping.Enabled = False
            txtShipping_Notes.Text = .Notes
        End With


        '.... "IssueComment:"
        With mProcess_Project.IssueCommnt
            Dim pCI As New CultureInfo("en-US")
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

            For j As Integer = 0 To .ID_Approval.Count - 1
                grdApproval_Attendees.Rows(j).Cells(1).Value = .Name(j)
                grdApproval_Attendees.Rows(j).Cells(2).Value = .Title(j)
                grdApproval_Attendees.Rows(j).Cells(3).Value = .Signed(j)

                If (.Signed(j)) Then
                    Dim pCI As New CultureInfo("en-US")
                    grdApproval_Attendees.Rows(j).Cells(4).Value = .DateSigned(j).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                Else
                    grdApproval_Attendees.Rows(j).Cells(4).Value = ""
                End If

            Next

        End With

        SetLabel_Unit()

    End Sub

    Private Sub SetLabel_Unit()
        '======================
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

#Region "TAB CONTROL RELATED ROUTINES:"

    Private Sub TabControl1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles TabControl1.SelectedIndexChanged
        '============================================================================================

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
        End If

        txtParkerPart.Focus()
        txtParkerPart.Select()

    End Sub

    Private Sub TabControl1_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) Handles TabControl1.DrawItem
        '==============================================================================================================================
        'Dim tabContas As TabControl = DirectCast(sender, TabControl)
        'Dim sTexto As String = tabContas.TabPages(e.Index).Text
        'Dim g As Graphics = e.Graphics
        'Dim fonte As Font = tabContas.Font
        'Dim format = New System.Drawing.StringFormat
        ''CHANGES HERE...
        'format.Alignment = StringAlignment.Center
        'format.LineAlignment = StringAlignment.Center
        'Dim pincel As New SolidBrush(Color.Black)
        ''RENEMED VARIEBLE HERE...
        'Dim retangulo As RectangleF = RectangleF.op_Implicit(tabContas.GetTabRect(e.Index))
        'If tabContas.SelectedIndex = e.Index Then
        '    fonte = New Font(fonte, FontStyle.Bold)
        '    pincel = New SolidBrush(Color.White)
        '    'CHANGED BACKGROUN COLOR HERE...
        '    g.FillRectangle(Brushes.Green, retangulo)
        'End If
        'g.DrawString(sTexto, fonte, pincel, retangulo, format)

        'Dim pProcessApp As New Process_clsApp()
        'pProcessApp.OpCond.T = New List(Of Process_clsApp.clsOpCond.sT)

        'Dim pT As New Process_clsApp.clsOpCond.sT
        'pT()
        ''      mTest_Report = New List(Of Test_clsReport)

        'For i As Integer = 0 To pQry.Count - 1

        '    Dim pReport As New Test_clsReport
        '    pReport.ID = pQry(i).fldID

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
        cmdSealPart.Focus()
    End Sub

    Private Sub txtDesign_MCS_MouseLeave(sender As Object, e As EventArgs) Handles txtDesign_MCS.MouseLeave
        '==================================================================================================
        txtDesign_MCS.Focus()
    End Sub

#End Region

#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbExport_Reqd_SelectedIndexChanged(sender As System.Object,
                                                    e As System.EventArgs) Handles cmbExport_Reqd.SelectedIndexChanged
        '================================================================================================================
        If (cmbExport_Reqd.Text = "N") Then
            cmbExport_Status.Enabled = False
        Else
            cmbExport_Status.Enabled = True
        End If
    End Sub

    Private Sub cmbITAR_Export_SaleExportControlled_SelectedIndexChanged(sender As System.Object,
                                                                         e As System.EventArgs) Handles cmbITAR_Export_SaleExportControlled.SelectedIndexChanged
        '========================================================================================================================================================

        If (cmbITAR_Export_SaleExportControlled.Text = "N") Then
            txtITAR_Export_EAR_Classification.Enabled = False
        Else
            txtITAR_Export_EAR_Classification.Enabled = True
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
            cmbITAR_Export_Status.Enabled = False
            txtExportStatus.Text = ""
        Else
            txtITAR_Export_ITAR_Classification.Enabled = True
            cmbITAR_Export_Status.Enabled = True
            cmbITAR_Export_Status.SelectedIndex = 0
            txtExportStatus.Text = cmbITAR_Export_Status.Text
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
            txtQuality_Reason.Enabled = True
        End If

    End Sub


    Private Sub cmbQuality_VisualInspection_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                                Handles cmbQuality_VisualInspection.SelectedIndexChanged
        '===============================================================================================================
        If (cmbQuality_VisualInspection.Text = "N") Then
            cmbQuality_VisualInspection_Type.Text = ""
            cmbQuality_VisualInspection_Type.Enabled = False
        Else
            cmbQuality_VisualInspection_Type.Enabled = True
        End If

    End Sub

    Private Sub cmbPartFamily_MouseHover(sender As Object, e As EventArgs) Handles cmbPartFamily.MouseHover
        '==================================================================================================
        ToolTip1.SetToolTip(cmbPartFamily, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPartFamily_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPartFamily.SelectedIndexChanged
        '======================================================================================================================
        cmbPartFamily.Text = gPartProject.PNR.SealType.ToString() & "-Seal"
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPartFamily_MouseLeave(sender As Object, e As EventArgs) Handles cmbPartFamily.MouseLeave
        '==================================================================================================
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbApp_Face_POrient_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                         Handles cmbApp_Face_POrient.SelectedIndexChanged
        '================================================================================================
        cmbApp_Face_POrient.Text = gPartProject.PNR.HW.POrient
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbApp_Face_POrient_MouseHover(sender As Object, e As EventArgs) Handles cmbApp_Face_POrient.MouseHover
        '=============================================================================================================
        ToolTip1.SetToolTip(cmbApp_Face_POrient, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbApp_Face_POrient_MouseLeave(sender As Object, e As EventArgs) Handles cmbApp_Face_POrient.MouseLeave
        '==============================================================================================================
        cmdSealPart.Focus()
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
        cmdSealPart.Focus()
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
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbParkerPN_Part2_NewRef_Dim_MouseHover(sender As Object, e As EventArgs) _
                                                        Handles cmbParkerPN_Part2_NewRef_Dim.MouseHover
        '==============================================================================================
        ToolTip1.SetToolTip(cmbParkerPN_Part2_NewRef_Dim, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbParkerPN_Part2_NewRef_Dim_MouseLeave(sender As Object, e As EventArgs) _
                                                        Handles cmbParkerPN_Part2_NewRef_Dim.MouseLeave
        '==============================================================================================
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbParkerPN_Part2_Notes_Dim_MouseHover(sender As Object, e As EventArgs) _
                                                       Handles cmbParkerPN_Part2_Notes_Dim.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(cmbParkerPN_Part2_Notes_Dim, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbParkerPN_Part2_Notes_Dim_MouseLeave(sender As Object, e As EventArgs) _
                                                       Handles cmbParkerPN_Part2_Notes_Dim.MouseLeave
        '============================================================================================
        cmdSealPart.Focus()
    End Sub


    Private Sub cmbDesign_Mat_Seal_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                        Handles cmbDesign_Mat_Seal.SelectedIndexChanged
        '==============================================================================================
        cmbDesign_Mat_Seal.Text = gPartProject.PNR.HW.MatName
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_Mat_Spring_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                          Handles cmbDesign_Mat_Spring.SelectedIndexChanged
        '==================================================================================================
        cmbDesign_Mat_Spring.Text = gPartProject.PNR.HW.MatName
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_Mat_Seal_MouseHover(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Seal.MouseHover
        '============================================================================================================
        ToolTip1.SetToolTip(cmbDesign_Mat_Seal, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_Mat_Seal_MouseLeave(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Seal.MouseLeave
        '============================================================================================================
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_Mat_Spring_MouseLeave(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Spring.MouseLeave
        '================================================================================================================
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_Mat_Spring_MouseHover(sender As Object, e As EventArgs) Handles cmbDesign_Mat_Spring.MouseHover
        '================================================================================================================
        ToolTip1.SetToolTip(cmbDesign_Mat_Spring, "Enter Data in SealPart.")
        cmdSealPart.Focus()
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
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_TemperType_MouseHover(sender As Object, e As EventArgs) Handles cmbDesign_TemperType.MouseHover
        '================================================================================================================
        ToolTip1.SetToolTip(cmbDesign_TemperType, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbDesign_TemperType_MouseLeave(sender As Object, e As EventArgs) Handles cmbDesign_TemperType.MouseLeave
        '================================================================================================================
        cmdSealPart.Focus()
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
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbCoating_MouseLeave(sender As Object, e As EventArgs) Handles cmbCoating.MouseLeave
        '============================================================================================
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbCoating_MouseHover(sender As Object, e As EventArgs) Handles cmbCoating.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(cmbCoating, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbSFinish_MouseHover(sender As Object, e As EventArgs) Handles cmbSFinish.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(cmbSFinish, "Enter Data in SealPart.")
        cmdSealPart.Focus()

    End Sub

    Private Sub cmbSFinish_MouseLeave(sender As Object, e As EventArgs) Handles cmbSFinish.MouseLeave
        '============================================================================================
        cmdSealPart.Focus()
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
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPlatingCode_MouseHover(sender As Object, e As EventArgs) Handles cmbPlatingCode.MouseHover
        '===================================================================================================
        ToolTip1.SetToolTip(cmbPlatingCode, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPlatingCode_MouseLeave(sender As Object, e As EventArgs) Handles cmbPlatingCode.MouseLeave
        '====================================================================================================
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPlatingThickCode_SelectedIndexChanged(sender As Object, e As EventArgs) _
                                                         Handles cmbPlatingThickCode.SelectedIndexChanged
        '================================================================================================
        If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            If (gPartProject.PNR.HW.Plating.Code <> "") Then
                cmbPlatingThickCode.Text = gPartProject.PNR.HW.Plating.ThickCode
            End If
        End If
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPlatingThickCode_MouseHover(sender As Object, e As EventArgs) Handles cmbPlatingThickCode.MouseHover
        '==============================================================================================================
        ToolTip1.SetToolTip(cmbPlatingThickCode, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub cmbPlatingThickCode_MouseLeave(sender As Object, e As EventArgs) Handles cmbPlatingThickCode.MouseLeave
        '==============================================================================================================
        cmdSealPart.Focus()

    End Sub

#End Region

#Region "CHECK BOX RELATED ROUTINES:"
    Private Sub chkPreOrderUserSigned_CheckedChanged(sender As System.Object,
                                                     e As System.EventArgs) Handles chkPreOrderUserSigned.CheckedChanged
        '================================================================================================================
        If (chkPreOrderUserSigned.Checked) Then
            cmdPreOrderUserSign.Text = "Signed"
            'txtPreOrderUserName.Text = "Jeffrey LaBonte"
            txtPreOrderUserDate.Text = DateTime.Now.ToShortDateString()
        Else
            cmdPreOrderUserSign.Text = "Sign"
            txtPreOrderUserName.Text = ""
            txtPreOrderUserDate.Text = ""
        End If

    End Sub


    Private Sub chkITAR_Export_UserSigned_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                         Handles chkITAR_Export_UserSigned.CheckedChanged
        '==================================================================================================
        If (chkITAR_Export_UserSigned.Checked) Then
            cmdITAR_Export_UserSign.Text = "Signed"
            'txtITAR_Export_UserName.Text = "Jeffrey LaBonte"
            txtITAR_Export_UserDate.Text = DateTime.Now.ToShortDateString()
        Else
            cmdITAR_Export_UserSign.Text = "Sign"
            txtITAR_Export_UserName.Text = ""
            txtITAR_Export_UserDate.Text = ""
        End If
    End Sub

    Private Sub chkOrdEntry_UserSigned_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles chkOrdEntry_UserSigned.CheckedChanged
        '===============================================================================================
        If (chkOrdEntry_UserSigned.Checked) Then
            cmdOrdEntry_UserSign.Text = "Signed"
            'txtOrdEntry_UserName.Text = "Jeffrey LaBonte"
            txtOrdEntry_UserDate.Text = DateTime.Now.ToShortDateString()
        Else
            cmdOrdEntry_UserSign.Text = "Sign"
            txtOrdEntry_UserName.Text = ""
            txtOrdEntry_UserDate.Text = ""
        End If
    End Sub

    Private Sub chkCost_UserSigned_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                  Handles chkCost_UserSigned.CheckedChanged
        '===========================================================================================
        If (chkCost_UserSigned.Checked) Then
            cmdCost_UserSign.Text = "Signed"
            'txtCost_UserName.Text = "Jeffrey LaBonte"
            txtCost_UserDate.Text = DateTime.Now.ToShortDateString()
        Else
            cmdCost_UserSign.Text = "Sign"
            txtCost_UserName.Text = ""
            txtCost_UserDate.Text = ""
        End If
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

#End Region

#Region "DATETIME PICKER RELATED ROUTINES:"
    Private Sub dtpStartDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpStartDate.ValueChanged
        '==================================================================================================================
        txtStartDate.Text = dtpStartDate.Value.ToShortDateString()
        txtDateMod.Text = DateTime.Now.ToShortDateString()

        If (gUser.FirstName <> "") Then
            txtModifiedBy.Text = gUser.FirstName & " " & gUser.LastName
        End If

    End Sub

    Private Sub dtpDateMod_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpDateMod.ValueChanged
        '==============================================================================================================
        txtDateMod.Text = dtpDateMod.Value.ToShortDateString()
        If (gUser.FirstName <> "") Then
            txtModifiedBy.Text = gUser.FirstName & " " & gUser.LastName
        End If
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

        If (pComboBox IsNot Nothing) Then
            RemoveHandler pComboBox.SelectionChangeCommitted, New EventHandler(AddressOf ComboBox_SelectionChangeCommitted)

            mRowIndex = grdApproval_Attendees.CurrentCell.RowIndex
            AddHandler pComboBox.SelectionChangeCommitted, New EventHandler(AddressOf ComboBox_SelectionChangeCommitted)
        End If

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

            mDateTimePicker = New DateTimePicker()
            grdIssueComment.Controls.Add(mDateTimePicker)
            mDateTimePicker.Format = DateTimePickerFormat.Short
            Dim pRectangle As Rectangle = grdIssueComment.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            mDateTimePicker.Size = New Size(pRectangle.Width, pRectangle.Height)
            mDateTimePicker.Location = New Point(pRectangle.X, pRectangle.Y)
            AddHandler mDateTimePicker.CloseUp, New EventHandler(AddressOf mDateTimePicker_CloseUp)
            AddHandler mDateTimePicker.ValueChanged, New EventHandler(AddressOf mDateTimePicker_OnTextChange)

        End If

    End Sub


#Region "HELPER ROUTINE"

    Private Sub ComboBox_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '====================================================================================================
        Dim combo As ComboBox = CType(sender, ComboBox)
        'Dim pStr As String = grdApproval_Attendees.CurrentCell.RowIndex.ToString() & " " & combo.SelectedItem.ToString()
        'MessageBox.Show(pStr)
        Dim pUserID As Integer = 0
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
        mDateTimePicker.Visible = False
    End Sub

    Private Sub mDateTimePicker_OnTextChange(ByVal sender As Object, ByVal e As EventArgs)
        '==================================================================================
        grdIssueComment.CurrentCell.Value = mDateTimePicker.Text.ToString()
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
        Dim pfrmNotes As New Process_frmNotes()
        pfrmNotes.ShowDialog()
    End Sub

    Private Sub cmdApproval_Sign_Click(sender As System.Object, e As System.EventArgs) Handles cmdApproval_Sign.Click
        '============================================================================================================
        If (grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(3).Value = True) Then
            grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(3).Value = False
            grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(4).Value = ""
        Else
            grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(3).Value = True
            Dim pCI As New CultureInfo("en-US")
            grdApproval_Attendees.Rows(grdApproval_Attendees.CurrentRow.Index).Cells(4).Value = DateTime.Now.ToString("MM/dd/yyyy", pCI.DateTimeFormat())
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

    Private Sub cmdSealPart_Click(sender As Object, e As EventArgs) Handles cmdSealPart.Click
        '====================================================================================
        SaveData()
        SaveToDB()
        Me.Close()
    End Sub

    Private Sub cmdIssueComment_Click(sender As Object, e As EventArgs) Handles cmdIssueComment.Click
        '=============================================================================================
        SaveData()
        'SaveToDB()
        'Dim pSelTabName As String =  TabControl1.SelectedTab.Name
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
        Me.Close()

    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '==================

        If (TabControl1.SelectedIndex = 2) Then
            CopyDataGridView(grdOrdEntry_CustContact, grdCustContact)
        End If

        If (TabControl1.SelectedIndex = 8) Then
            CopyDataGridView(grdQuality_SplOperation, grdCost_SplOperation)
        End If


        '.... "Header:"

        With mProcess_Project
            .POPCoding = cmbPopCoding.Text

            .Rating = cmbRating.Text
            .Type = cmbType.Text

            If (txtStartDate.Text <> "") Then
                If (txtStartDate.Text <> DateTime.MinValue) Then
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
            .EAR_Class = txtITAR_Export_EAR_Classification.Text
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
                .LeadTimeQuoted = Convert.ToDouble(txtOrderEntry_QtdLeadTime.Text)
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
                .OrdQty = Convert.ToInt16(txtOrdEntry_OrderQty.Text)
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
                .MaxLeak = Convert.ToDouble(txtApp_MaxLeak.Text)
            End If


            .IsPressCyclic = IIf(cmbApp_PressCycle.Text = "Y", True, False)

            If (.IsPressCyclic) Then
                If (txtApp_PressCycleFreq.Text <> "") Then
                    .PressCycle_Freq = Convert.ToDouble(txtApp_PressCycleFreq.Text)
                End If

                If (txtApp_PressCycleAmp.Text <> "") Then
                    .PressCycle_Amp = Convert.ToDouble(txtApp_PressCycleAmp.Text)
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
                .OpCond.T_Assy = Convert.ToDouble(grdApp_OpCond.Rows(0).Cells(1).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(2).Value) And grdApp_OpCond.Rows(0).Cells(2).Value <> "") Then
                .OpCond.T_Min = Convert.ToDouble(grdApp_OpCond.Rows(0).Cells(2).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(3).Value) And grdApp_OpCond.Rows(0).Cells(3).Value <> "") Then
                .OpCond.T_Max = Convert.ToDouble(grdApp_OpCond.Rows(0).Cells(3).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(0).Cells(4).Value) And grdApp_OpCond.Rows(0).Cells(4).Value <> "") Then
                .OpCond.T_Oper = Convert.ToDouble(grdApp_OpCond.Rows(0).Cells(4).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(1).Value) And grdApp_OpCond.Rows(1).Cells(1).Value <> "") Then
                .OpCond.Press_Assy = Convert.ToDouble(grdApp_OpCond.Rows(1).Cells(1).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(2).Value) And grdApp_OpCond.Rows(1).Cells(2).Value <> "") Then
                .OpCond.Press_Min = Convert.ToDouble(grdApp_OpCond.Rows(1).Cells(2).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(3).Value) And grdApp_OpCond.Rows(1).Cells(3).Value <> "") Then
                .OpCond.Press_Max = Convert.ToDouble(grdApp_OpCond.Rows(1).Cells(3).Value)
            End If

            If (Not IsNothing(grdApp_OpCond.Rows(1).Cells(4).Value) And grdApp_OpCond.Rows(1).Cells(4).Value <> "") Then
                .OpCond.Press_Oper = Convert.ToDouble(grdApp_OpCond.Rows(1).Cells(4).Value)
            End If

            If (Not IsNothing(grdApp_Load.Rows(0).Cells(1).Value) And grdApp_Load.Rows(0).Cells(1).Value <> "") Then
                .Load.Assy_Min = Convert.ToDouble(grdApp_Load.Rows(0).Cells(1).Value)
            End If

            If (Not IsNothing(grdApp_Load.Rows(0).Cells(2).Value) And grdApp_Load.Rows(0).Cells(2).Value <> "") Then
                .Load.Assy_Max = Convert.ToDouble(grdApp_Load.Rows(0).Cells(2).Value)
            End If

            If (Not IsNothing(grdApp_Load.Rows(1).Cells(1).Value) And grdApp_Load.Rows(1).Cells(1).Value <> "") Then
                .Load.Oper_Min = Convert.ToDouble(grdApp_Load.Rows(1).Cells(1).Value)
            End If

            If (Not IsNothing(grdApp_Load.Rows(0).Cells(2).Value) And grdApp_Load.Rows(0).Cells(2).Value <> "") Then
                .Load.Oper_Max = Convert.ToDouble(grdApp_Load.Rows(1).Cells(2).Value)
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
                        pAssyMin = Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(1).Value)
                    End If

                    Dim pAssyMax As Double = 0
                    If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(2).Value) And grdApp_Face_Cavity.Rows(i).Cells(2).Value <> "") Then
                        pAssyMax = Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(2).Value)
                    End If

                    Dim pAssy As clsProcessProj_App.clsCavity.sAssy
                    pAssy.Min = pAssyMin
                    pAssy.Max = pAssyMax
                    .Cavity.Assy.Add(pAssy)

                    Dim pOperMin As Double = 0
                    If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(3).Value) And grdApp_Face_Cavity.Rows(i).Cells(3).Value <> "") Then
                        pOperMin = Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(3).Value)
                    End If

                    Dim pOperMax As Double = 0
                    If (Not IsNothing(grdApp_Face_Cavity.Rows(i).Cells(4).Value) And grdApp_Face_Cavity.Rows(i).Cells(4).Value <> "") Then
                        pOperMax = Convert.ToDouble(grdApp_Face_Cavity.Rows(i).Cells(4).Value)
                    End If

                    Dim pOper As clsProcessProj_App.clsCavity.sOper
                    pOper.Min = pOperMin
                    pOper.Max = pOperMax
                    .Cavity.Oper.Add(pOper)
                Next

                .CavityFlange.Mat1 = txtApp_Mat1_Face.Text
                .CavityFlange.Mat2 = txtApp_Mat2_Face.Text

                If (txtApp_Hardness1_Face.Text <> "") Then
                    .CavityFlange.Hard1 = Convert.ToDouble(txtApp_Hardness1_Face.Text)
                Else
                    .CavityFlange.Hard1 = 0
                End If

                If (txtApp_Hardness2_Face.Text <> "") Then
                    .CavityFlange.Hard2 = Convert.ToDouble(txtApp_Hardness2_Face.Text)
                Else
                    .CavityFlange.Hard2 = 0
                End If

                If (txtApp_SF1_Face.Text <> "") Then
                    .CavityFlange.SF1 = Convert.ToDouble(txtApp_SF1_Face.Text)
                Else
                    .CavityFlange.SF1 = 0
                End If

                If (txtApp_SF2_Face.Text <> "") Then
                    .CavityFlange.SF2 = Convert.ToDouble(txtApp_SF2_Face.Text)
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
                        pAssyMin = Convert.ToDouble(grdApp_Axial_Cavity.Rows(i).Cells(1).Value)
                    End If

                    Dim pAssyMax As Double = 0
                    If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(2).Value) And grdApp_Axial_Cavity.Rows(i).Cells(2).Value <> "") Then
                        pAssyMax = Convert.ToDouble(grdApp_Axial_Cavity.Rows(i).Cells(2).Value)
                    End If

                    Dim pAssy As clsProcessProj_App.clsCavity.sAssy
                    pAssy.Min = pAssyMin
                    pAssy.Max = pAssyMax
                    .Cavity.Assy.Add(pAssy)

                    Dim pOperMin As Double = 0
                    If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(3).Value) And grdApp_Axial_Cavity.Rows(i).Cells(3).Value <> "") Then
                        pOperMin = Convert.ToDouble(grdApp_Axial_Cavity.Rows(i).Cells(3).Value)
                    End If

                    Dim pOperMax As Double = 0
                    If (Not IsNothing(grdApp_Axial_Cavity.Rows(i).Cells(4).Value) And grdApp_Axial_Cavity.Rows(i).Cells(4).Value <> "") Then
                        pOperMax = Convert.ToDouble(grdApp_Axial_Cavity.Rows(i).Cells(4).Value)
                    End If

                    Dim pOper As clsProcessProj_App.clsCavity.sOper
                    pOper.Min = pOperMin
                    pOper.Max = pOperMax
                    .Cavity.Oper.Add(pOper)

                Next

                .CavityFlange.Mat1 = txtApp_Mat1_Axial.Text
                .CavityFlange.Mat2 = txtApp_Mat2_Axial.Text

                If (txtApp_Hardness1_Axial.Text <> "") Then
                    .CavityFlange.Hard1 = Convert.ToDouble(txtApp_Hardness1_Axial.Text)
                Else
                    .CavityFlange.Hard1 = 0
                End If

                If (txtApp_Hardness2_Axial.Text <> "") Then
                    .CavityFlange.Hard2 = Convert.ToDouble(txtApp_Hardness2_Axial.Text)
                Else
                    .CavityFlange.Hard2 = 0
                End If

                If (txtApp_SF1_Axial.Text <> "") Then
                    .CavityFlange.SF1 = Convert.ToDouble(txtApp_SF1_Axial.Text)
                Else
                    .CavityFlange.SF1 = 0
                End If

                If (txtApp_SF2_Axial.Text <> "") Then
                    .CavityFlange.SF2 = Convert.ToDouble(txtApp_SF2_Axial.Text)
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
                    .Axial.Recip_Stroke = Convert.ToDouble(txtApp_RecipStrokeL_Axial.Text)
                    .Axial.Recip_V = Convert.ToDouble(txtApp_RecipV_Axial.Text)
                    .Axial.Recip_CycleRate = Convert.ToDouble(txtApp_RecipCycleRate_Axial.Text)
                    .Axial.Recip_ServiceLife = Convert.ToDouble(txtApp_RecipServiceLife_Axial.Text)
                Else
                    .Axial.Recip_Stroke = 0.0
                    .Axial.Recip_V = 0.0
                    .Axial.Recip_CycleRate = 0.0
                    .Axial.Recip_ServiceLife = 0.0
                End If

                .Axial.IsOscilatory = IIf(cmbApp_Osc_Axial.Text = "Y", True, False)
                If (.Axial.IsOscilatory) Then
                    .Axial.Oscilate_Rot = Convert.ToDouble(txtApp_OscRot_Axial.Text)
                    .Axial.Oscilate_V = Convert.ToDouble(txtApp_OscV_Axial.Text)
                    .Axial.Oscilate_CycleRate = Convert.ToDouble(txtApp_OscCycleRate_Axial.Text)
                    .Axial.Oscilate_ServiceLife = Convert.ToDouble(txtApp_OscServiceLife_Axial.Text)
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

                .Mat.Status.Add(grdPurchase_Mat.Rows(i).Cells(2).Value)

                If (Not IsNothing(grdPurchase_Mat.Rows(i).Cells(3).Value)) Then
                    If (grdPurchase_Mat.Rows(i).Cells(3).Value.ToString() <> "") Then
                        .Mat.LeadTime.Add(grdPurchase_Mat.Rows(i).Cells(3).Value)
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
                .Leak.Qty_Unplated = Convert.ToDouble(cmbTest_QtyPre_Leak.Text)
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
                .Load.Qty_Unplated = Convert.ToDouble(cmbTest_QtyPre_Load.Text)
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
                .SpringBack.Qty_Unplated = Convert.ToDouble(cmbTest_QtyPre_SpringBack.Text)
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
                .Name.Add(grdApproval_Attendees.Rows(j).Cells(1).Value)
                .Title.Add(grdApproval_Attendees.Rows(j).Cells(2).Value)
                .Signed.Add(grdApproval_Attendees.Rows(j).Cells(3).Value)
                If (grdApproval_Attendees.Rows(j).Cells(4).Value <> "" And Not IsNothing(grdApproval_Attendees.Rows(j).Cells(4).Value)) Then
                    .DateSigned.Add(grdApproval_Attendees.Rows(j).Cells(4).Value)
                Else
                    .DateSigned.Add(DateTime.MinValue)
                End If

            Next
        End With

        gProcessProject = mProcess_Project.Clone()

    End Sub

    Private Sub SaveToDB()
        '==================
        mProcess_Project.SaveToDB(mPNID, mRevID)
        mProcess_Project.CustContact.SaveToDB(mProcess_Project.ID)
        mProcess_Project.PreOrder.SaveToDB(mProcess_Project.ID)
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
        mProcess_Project.Approval.SaveToDB(mProcess_Project.ID)
    End Sub

    Private Function CompareVal_PreOrder() As Boolean
        '==============================================
        Dim pblnValChanged As Boolean = False

        With mProcess_Project.PreOrder
            If (.Mgr.Mkt <> cmbMgrPreOrder.Text) Then
                pblnValChanged = True
            End If

            If (.Mgr.Sales <> txtMgrSales.Text) Then
                pblnValChanged = True
            End If

            Dim pExpReq As Boolean
            If (cmbExport_Reqd.Text = "Y") Then
                pExpReq = True
            Else
                pExpReq = False
            End If

            If (.Export.Reqd <> pExpReq) Then
                pblnValChanged = True
            End If

            If (.Export.Status <> cmbExport_Status.Text) Then
                pblnValChanged = True
            End If

            If (.Part.Family <> cmbPartFamily.Text) Then
                pblnValChanged = True
            End If

            If (.Part.Type <> cmbPartType.Text) Then
                pblnValChanged = True
            End If

            If (.Mkt.Seg <> cmbPreOrderSeg.Text) Then
                pblnValChanged = True
            End If

            If (.Mkt.Channel <> cmbPreOrderChannel.Text) Then
                pblnValChanged = True
            End If

            If (.Notes <> txtPreOrderNotes.Text) Then
                pblnValChanged = True
            End If

            If (.Loc.CostFile <> cmbCostFileLoc.Text) Then
                pblnValChanged = True
            End If

            If (.Loc.RFQPkg <> cmbRFQPkgLoc.Text) Then
                pblnValChanged = True
            End If

            If (.Notes_Price <> txtPreOrderPriceNotes.Text) Then
                pblnValChanged = True
            End If
        End With

        For i As Integer = 0 To mProcess_Project.CustContact.ID_Cust.Count - 1
            If (mProcess_Project.CustContact.DeptName(i) <> grdCustContact.Rows(i).Cells(0).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.CustContact.Name(i) <> grdCustContact.Rows(i).Cells(1).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.CustContact.Phone(i) <> grdCustContact.Rows(i).Cells(2).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.CustContact.Email(i) <> grdCustContact.Rows(i).Cells(3).Value) Then
                pblnValChanged = True
            End If
        Next


        For i As Integer = 0 To mProcess_Project.PreOrder.Quote.QID.Count - 1
            If (mProcess_Project.PreOrder.Quote.QDate(i) <> grdQuote.Rows(i).Cells(0).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.PreOrder.Quote.No(i) <> grdQuote.Rows(i).Cells(1).Value) Then
                pblnValChanged = True
            End If
        Next

        For i As Integer = 0 To mProcess_Project.PreOrder.SalesData.ID_Sales.Count - 1
            If (mProcess_Project.PreOrder.SalesData.Year(i) <> grdPreOrder_SalesData.Rows(i).Cells(0).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.PreOrder.SalesData.Qty(i) <> grdPreOrder_SalesData.Rows(i).Cells(1).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.PreOrder.SalesData.Price(i) <> grdPreOrder_SalesData.Rows(i).Cells(2).Value) Then
                pblnValChanged = True
            End If

            If (mProcess_Project.PreOrder.SalesData.Total(i) <> grdPreOrder_SalesData.Rows(i).Cells(3).Value) Then
                pblnValChanged = True
            End If
        Next


        '''...."Pre-Order:"

        ''With mProcess_Project.PreOrder
        ''    .Mgr_PreOrder = cmbMgrPreOrder.Text
        ''    .Mgr_Sales = txtMgrSales.Text

        ''    If (cmbExport_Reqd.Text = "Y") Then
        ''        .Export_Reqd = True
        ''    Else
        ''        .Export_Reqd = False
        ''    End If

        ''    .Export_Status = cmbExport_Status.Text

        ''    .Part_Family = cmbPartFamily.Text
        ''    .Part_Type = cmbPartType.Text

        ''    .PreOrder_Seg = cmbPreOrderSeg.Text
        ''    .PreOrder_Channel = cmbPreOrderChannel.Text
        ''    .Notes = txtPreOrderNotes.Text

        ''    .Loc_CostFile = cmbCostFileLoc.Text
        ''    .Loc_RFQPkg = cmbRFQPkgLoc.Text
        ''    .Notes_Price = txtPreOrderPriceNotes.Text

        ''    If (chkPreOrderUserSigned.Checked) Then
        ''        '.User_Name = txtPreOrderUserName.Text
        ''        .EditedBy.User_Name = txtPreOrderUserName.Text
        ''        .EditedBy.User_DateSigned = Convert.ToDateTime(txtPreOrderUserDate.Text)
        ''        .EditedBy.User_Signed = True

        ''    Else
        ''        .EditedBy.User_Name = ""
        ''        .EditedBy.User_DateSigned = DateTime.MinValue
        ''        .EditedBy.User_Signed = False
        ''    End If

        ''    '....Cust Contact Pre-Order
        ''    mProcess_Project.CustContact.ID_Cust.Clear()
        ''    mProcess_Project.CustContact.DeptName.Clear()
        ''    mProcess_Project.CustContact.Name.Clear()
        ''    mProcess_Project.CustContact.Phone.Clear()
        ''    mProcess_Project.CustContact.Email.Clear()

        ''    'grdOrdEntry_CustContact = grdCustContact

        ''    For j As Integer = 0 To grdCustContact.Rows.Count - 2
        ''        mProcess_Project.CustContact.ID_Cust.Add(j + 1)
        ''        mProcess_Project.CustContact.DeptName.Add(grdCustContact.Rows(j).Cells(0).Value)
        ''        mProcess_Project.CustContact.Name.Add(grdCustContact.Rows(j).Cells(1).Value)
        ''        mProcess_Project.CustContact.Phone.Add(grdCustContact.Rows(j).Cells(2).Value)
        ''        mProcess_Project.CustContact.Email.Add(grdCustContact.Rows(j).Cells(3).Value)
        ''    Next

        ''    '....Cust Contact Order-Entry
        ''    mProcess_Project.CustContact.ID_Cust.Clear()
        ''    mProcess_Project.CustContact.DeptName.Clear()
        ''    mProcess_Project.CustContact.Name.Clear()
        ''    mProcess_Project.CustContact.Phone.Clear()
        ''    mProcess_Project.CustContact.Email.Clear()
        ''    For j As Integer = 0 To grdOrdEntry_CustContact.Rows.Count - 2
        ''        mProcess_Project.CustContact.ID_Cust.Add(j + 1)
        ''        mProcess_Project.CustContact.DeptName.Add(grdOrdEntry_CustContact.Rows(j).Cells(0).Value)
        ''        mProcess_Project.CustContact.Name.Add(grdOrdEntry_CustContact.Rows(j).Cells(1).Value)
        ''        mProcess_Project.CustContact.Phone.Add(grdOrdEntry_CustContact.Rows(j).Cells(2).Value)
        ''        mProcess_Project.CustContact.Email.Add(grdOrdEntry_CustContact.Rows(j).Cells(3).Value)
        ''    Next

        ''    '....Quote
        ''    .Quote.QID.Clear()
        ''    .Quote.QDate.Clear()
        ''    .Quote.No.Clear()

        ''    For j As Integer = 0 To grdQuote.Rows.Count - 2
        ''        .Quote.QID.Add(j + 1)
        ''        'grdQuote.Rows(j).Cells(0).Value <> "" And 
        ''        If (Not IsNothing(grdQuote.Rows(j).Cells(0).Value)) Then
        ''            .Quote.QDate.Add(grdQuote.Rows(j).Cells(0).Value)
        ''        Else
        ''            .Quote.QDate.Add(DateTime.MinValue)
        ''        End If

        ''        .Quote.No.Add(grdQuote.Rows(j).Cells(1).Value)
        ''    Next

        ''    '....Sales Data
        ''    .SalesData.ID_Sales.Clear()
        ''    .SalesData.Year.Clear()
        ''    .SalesData.Qty.Clear()
        ''    .SalesData.Price.Clear()
        ''    .SalesData.Total.Clear()

        ''    For j As Integer = 0 To grdPreOrder_SalesData.Rows.Count - 2
        ''        .SalesData.ID_Sales.Add(j + 1)
        ''        .SalesData.Year.Add(grdPreOrder_SalesData.Rows(j).Cells(0).Value)
        ''        .SalesData.Qty.Add(grdPreOrder_SalesData.Rows(j).Cells(1).Value)
        ''        .SalesData.Price.Add(grdPreOrder_SalesData.Rows(j).Cells(2).Value)
        ''        .SalesData.Total.Add(grdPreOrder_SalesData.Rows(j).Cells(3).Value)
        ''    Next

        ''End With

        Return pblnValChanged

    End Function

#End Region

#End Region

#Region "GROUP BOX EVENT ROUTINES:"
    Private Sub grpRefPN_MouseHover(sender As Object, e As EventArgs) Handles grpRefPN.MouseHover
        '========================================================================================
        ToolTip1.SetToolTip(grpRefPN, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub grpRefPN_MouseLeave(sender As Object, e As EventArgs) Handles grpRefPN.MouseLeave
        '========================================================================================
        grpRefPN.Focus()
    End Sub

    Private Sub grpCoating_MouseHover(sender As Object, e As EventArgs) Handles grpCoating.MouseHover
        '============================================================================================
        ToolTip1.SetToolTip(grpCoating, "Enter Data in SealPart.")
        cmdSealPart.Focus()
    End Sub

    Private Sub grpCoating_MouseLeave(sender As Object, e As EventArgs) Handles grpCoating.MouseLeave
        '============================================================================================
        grpCoating.Focus()
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

    Private Sub Delete_Record(ByVal GrdView_In As DataGridView, ByVal RowIndex_In As Integer)
        '====================================================================================
        If (RowIndex_In <> GrdView_In.Rows.Count - 1) Then
            GrdView_In.Rows.RemoveAt(RowIndex_In)
        End If

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
                grdDesign_Input.Rows(e.RowIndex).Cells(0).Value = e.FormattedValue
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
            EnableTab(tabLeak, True)
            EnableTab(tabLoad, True)
            EnableTab(tabSpringBack, True)
            txtTest_Other.Enabled = True
        End If

    End Sub

    Public Sub EnableTab(ByVal page As TabPage, ByVal enable As Boolean)
        EnableControls(page.Controls, enable)
    End Sub

    Private Sub EnableControls(ByVal ctls As Control.ControlCollection, ByVal enable As Boolean)
        For Each ctl As Control In ctls
            ctl.Enabled = enable
            EnableControls(ctl.Controls, enable)
        Next
    End Sub




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

End Class