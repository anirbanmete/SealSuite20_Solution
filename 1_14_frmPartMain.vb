'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  frmMain                                '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  25APR18                                '
'                                                                              '
'===============================================================================
'
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports System.Threading

Public Class frmPartMain
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mPartEntities As New SealPartDBEntities()

    Private mblnAdd As Boolean
    Private mblnEdit As Boolean

    Private mPNID As Integer
    Private mRevID As Integer
    Private mCustomerID As Integer
    Private mLocationID As Integer
    Private mPlatformID As Integer

    Private NodesThatMatch As New List(Of TreeNode)
    Private mProjectView As Boolean = False
    Private mPNView As Boolean = False

    Private mIPEProjectID As Integer = 0
    Private mPN As String = ""
    Private mPN_Rev As String = ""

    Private mPartProject As clsPartProject                    '....Local Project Object. 

#End Region


#Region "FORM CONSTRUCTOR:"

    Public Sub New(Optional ByVal View_In As String = "Project")
        '==============================================================

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If (View_In = "Project") Then
            mProjectView = True

        ElseIf (View_In = "PN") Then
            mPNView = True

        End If

        '....Unit
        With cmbUnit.Items
            .Clear()
            .Add("English")
            '.Add("Metric")     'Not open in SealTest 2.3

        End With

        With cmbParkerPN_Part2.Items
            .Clear()
            .Add("69")
            .Add("76")
            .Add("79")
            '.Add("44")
        End With

        With cmbParentCur_Part2.Items
            .Clear()
            .Add("69")
            .Add("76")
            .Add("79")
            .Add("44")
        End With


        With cmbRefPNNewDim_Part2.Items
            .Clear()
            .Add("69")
            .Add("76")
            .Add("79")
            .Add("44")
        End With

        With cmbRefNotesNewPN_Part2.Items
            .Clear()
            .Add("69")
            .Add("76")
            .Add("79")
            .Add("44")
        End With

        If mProjectView Then
            mnuProject.Checked = True
            mnuPN.Checked = False
            cmbUnit.Items.Add("Metric")

        ElseIf mPNView Then
            mnuProject.Checked = False
            mnuPN.Checked = True
            cmbParkerPN_Part2.Items.Add("44")
        End If

        InitializeLocalObject()

    End Sub

#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmPartProject_Load(sender As System.Object, e As System.EventArgs) _
                                Handles MyBase.Load
        '==========================================================================

        'InitializeLocalObject()

        'Add any initialization after the InitializeComponent() call
        '....European Convention
        ''PopulateCultureCmbBox(cmbCulturalFormat)
        tsbAdd.Enabled = True
        tsbEdit.Enabled = True
        tsbSave.Enabled = False
        tsbDelete.Enabled = True

        mblnAdd = False
        mblnEdit = False
        cmdHardware.Enabled = False
        cmdSealProcess.Enabled = False


        GetPartProjectInfo()

        grpCustomer.Text = ""
        If (mProjectView) Then
            UpdateIndexField()
            grpCustomer.Text = "Customer:"
            'Me.Text = "SealIPE: Part No"
        ElseIf (mPNView) Then
            UpdateIndexField()
            grpCustomer.Text = "P/N:"
            'Me.Text = "SealTest: Part No"
            'RetrievePN()
        End If

        '....Read the Ini file:                     
        gFile.ReadIniFile(gUser, gIPE_Project, gIPE_ANSYS, gIPE_Unit)  '....Get UserName, Phone No & Unit System. 

        DisplayData()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject(Optional ByVal SealProcess_In As String = "Project")
        '=================================================================================
        '....Instantiate Local Cavity Object. 
        mPartProject = New clsPartProject()

        With mPartProject
            .Project_ID = gPartProject.Project_ID
        End With

    End Sub

    Private Sub GetPartProjectInfo()
        '===========================
        Dim pQryProject = (From it In mPartEntities.tblProject
                               Where it.fldID = gPartProject.Project_ID Select it).ToList()

        If (pQryProject.Count() > 0) Then
            mPNID = pQryProject(0).fldPNID
            mRevID = pQryProject(0).fldRevID
            mCustomerID = pQryProject(0).fldCustID
            mPlatformID = pQryProject(0).fldPlatformID
            mLocationID = pQryProject(0).fldLocID
        End If
    End Sub

    Private Sub RetrievePN()
        '====================
        ''gPart_PN = New List(Of clsPart_PN)
        ''Dim pQryPN = (From it In mPartEntities.tblPN
        ''                       Order By it.fldID Ascending Select it).ToList()

        ''If (pQryPN.Count() > 0) Then
        ''    For i As Integer = 0 To pQryPN.Count() - 1
        ''        Dim pPN_New As String = pQryPN(i).fldNew


        ''        Dim pPN_No As String = ""
        ''        Dim pTypeNo As Integer = 0
        ''        Dim pVal As String = ""

        ''        If (pPN_New.Contains("NH")) Then
        ''            pPN_No = pPN_New.Substring(3, pPN_New.Length - 3)
        ''            pTypeNo = Convert.ToInt16(pPN_No.Substring(0, 2))
        ''            pVal = pPN_No.Substring(2, pPN_No.Length - 2)
        ''        End If

        ''        Dim pPN As New clsPart_PN()
        ''        pPN.ID = pQryPN(i).fldID
        ''        If (pPN_New <> "") Then
        ''            pPN.New_Exists = True
        ''            pPN.New_TypeNo = pTypeNo
        ''            pPN.New_Val = pVal

        ''            Dim pQryRevList = (From it In mPartEntities.tblRev Where it.fldPNID = pPN.ID
        ''                 Order By it.fldID Ascending Select it).ToList()

        ''            If (pQryRevList.Count() > 0) Then
        ''                Dim pRevList As New List(Of String)
        ''                For j As Integer = 0 To pQryRevList.Count - 1
        ''                    If (Not IsNothing(pQryRevList(j).fldNew) And Not IsDBNull(pQryRevList(j).fldNew)) Then
        ''                        Dim pRev As String = pQryRevList(j).fldNew
        ''                        If (pRev <> "") Then
        ''                            pRevList.Add(pRev)
        ''                        End If
        ''                    End If
        ''                Next
        ''                pPN.New_Rev = pRevList
        ''            End If
        ''        End If



        ''        Dim pLegacyType As Integer = 0
        ''        If (Not IsNothing(pQryPN(i).fldLegacyType) And Not IsDBNull(pQryPN(i).fldLegacyType)) Then
        ''            pLegacyType = pQryPN(i).fldLegacyType
        ''        End If

        ''        Dim pLegacy As String = ""
        ''        If (Not IsNothing(pQryPN(i).fldLegacy) And Not IsDBNull(pQryPN(i).fldLegacy)) Then
        ''            pLegacy = pQryPN(i).fldLegacy
        ''            pPN.Legacy_Exists = True
        ''            pPN.Legacy_Type = pLegacyType
        ''            pPN.Legacy_Val = pLegacy

        ''            Dim pQryRevList = (From it In mPartEntities.tblRev Where it.fldPNID = pPN.ID
        ''                 Order By it.fldID Ascending Select it).ToList()

        ''            If (pQryRevList.Count() > 0) Then
        ''                Dim pRevList As New List(Of String)
        ''                For j As Integer = 0 To pQryRevList.Count - 1
        ''                    If (Not IsNothing(pQryRevList(j).fldLegacy) And Not IsDBNull(pQryRevList(j).fldLegacy)) Then
        ''                        Dim pRev As String = pQryRevList(j).fldLegacy
        ''                        If (pRev <> "") Then
        ''                            pRevList.Add(pRev)
        ''                        End If
        ''                    End If
        ''                Next
        ''                pPN.Legacy_Rev = pRevList
        ''            End If

        ''        End If

        ''        If (Not IsNothing(pQryPN(i).fldParent) And Not IsDBNull(pQryPN(i).fldParent)) Then
        ''            pPN.Parent = pQryPN(i).fldParent
        ''        End If

        ''        If (Not IsNothing(pQryPN(i).fldParentRev) And Not IsDBNull(pQryPN(i).fldParentRev)) Then
        ''            pPN.ParentRev = pQryPN(i).fldParentRev
        ''        End If

        ''        '============
        ''        If (Not IsNothing(pQryPN(i).fldNewRef_Dim) And Not IsDBNull(pQryPN(i).fldNewRef_Dim)) Then
        ''            Dim pRefDimNew As String = pQryPN(i).fldNewRef_Dim

        ''            If (pRefDimNew <> "") Then
        ''                pPN.RefDimNew_Val = pRefDimNew
        ''                pPN.RefDimNew_Exists = True

        ''                If (Not IsNothing(pQryPN(i).fldNewRef_Dim_Rev) And Not IsDBNull(pQryPN(i).fldNewRef_Dim_Rev)) Then
        ''                    Dim pRefDimNewRev As String = pQryPN(i).fldNewRef_Dim_Rev
        ''                    If (pRefDimNewRev <> "") Then
        ''                        pPN.RefDimNew_Rev = pRefDimNewRev
        ''                    End If

        ''                End If
        ''            End If
        ''        End If

        ''        If (Not IsNothing(pQryPN(i).fldLegacyRef_Dim) And Not IsDBNull(pQryPN(i).fldLegacyRef_Dim)) Then
        ''            Dim pRefDimLegacy As String = pQryPN(i).fldLegacyRef_Dim

        ''            If (pRefDimLegacy <> "") Then
        ''                pPN.RefDimLegacy_Val = pRefDimLegacy
        ''                pPN.RefDimLegacy_Exists = True

        ''                If (Not IsNothing(pQryPN(i).fldLegacyRef_Dim_Rev) And Not IsDBNull(pQryPN(i).fldLegacyRef_Dim_Rev)) Then
        ''                    Dim pRefDimLegacyRev As String = pQryPN(i).fldLegacyRef_Dim_Rev
        ''                    If (pRefDimLegacyRev <> "") Then
        ''                        pPN.RefDimLegacy_Rev = pRefDimLegacyRev
        ''                    End If

        ''                End If
        ''            End If
        ''        End If

        ''        If (Not IsNothing(pQryPN(i).fldNewRef_Notes) And Not IsDBNull(pQryPN(i).fldNewRef_Notes)) Then
        ''            Dim pRefNotesNew As String = pQryPN(i).fldNewRef_Notes

        ''            If (pRefNotesNew <> "") Then
        ''                pPN.RefNotesNew_Val = pRefNotesNew
        ''                pPN.RefNotesNew_Exists = True

        ''                If (Not IsNothing(pQryPN(i).fldNewRef_Notes_Rev) And Not IsDBNull(pQryPN(i).fldNewRef_Notes_Rev)) Then
        ''                    Dim pRefNotesNewRev As String = pQryPN(i).fldNewRef_Notes_Rev
        ''                    If (pRefNotesNewRev <> "") Then
        ''                        pPN.RefNotesNew_Rev = pRefNotesNewRev
        ''                    End If

        ''                End If
        ''            End If
        ''        End If


        ''        If (Not IsNothing(pQryPN(i).fldLegacyRef_Notes) And Not IsDBNull(pQryPN(i).fldLegacyRef_Notes)) Then
        ''            Dim pRefNotesLegacy As String = pQryPN(i).fldLegacyRef_Notes

        ''            If (pRefNotesLegacy <> "") Then
        ''                pPN.RefNotesLegacy_Val = pRefNotesLegacy
        ''                pPN.RefNotesLegacy_Exists = True

        ''                If (Not IsNothing(pQryPN(i).fldLegacyRef_Notes_Rev) And Not IsDBNull(pQryPN(i).fldLegacyRef_Notes_Rev)) Then
        ''                    Dim pRefNotesLegacyRev As String = pQryPN(i).fldLegacyRef_Notes_Rev
        ''                    If (pRefNotesLegacyRev <> "") Then
        ''                        pPN.RefNotesLegacy_Rev = pRefNotesLegacyRev
        ''                    End If

        ''                End If
        ''            End If
        ''        End If

        ''        '==========

        ''        If (Not IsNothing(pQryPN(i).fldAppType) And Not IsDBNull(pQryPN(i).fldAppType)) Then
        ''            pPN.AppType = pQryPN(i).fldAppType
        ''        End If

        ''        If (Not IsNothing(pQryPN(i).fldGeomTemplate) And Not IsDBNull(pQryPN(i).fldGeomTemplate)) Then
        ''            pPN.GeomTemplate = pQryPN(i).fldGeomTemplate
        ''        End If

        ''        ''If (Not IsNothing(pQryPN(i).fldSealIPE) And Not IsDBNull(pQryPN(i).fldSealIPE)) Then
        ''        ''    pPN.SealIPE = pQryPN(i).fldSealIPE
        ''        ''End If

        ''        ''If (Not IsNothing(pQryPN(i).fldSealTest) And Not IsDBNull(pQryPN(i).fldSealTest)) Then
        ''        ''    pPN.SealTest = pQryPN(i).fldSealTest
        ''        ''End If

        ''        ''If (Not IsNothing(pQryPN(i).fldSealProcess) And Not IsDBNull(pQryPN(i).fldSealProcess)) Then
        ''        ''    pPN.SealProcess = pQryPN(i).fldSealProcess
        ''        ''End If

        ''        If (Not IsNothing(pQryPN(i).fldDate) And Not IsDBNull(pQryPN(i).fldDate)) Then
        ''            pPN.DateTime = pQryPN(i).fldDate
        ''        End If

        ''        If (Not IsNothing(pQryPN(i).fldIndex) And Not IsDBNull(pQryPN(i).fldIndex)) Then
        ''            pPN.Index = pQryPN(i).fldIndex
        ''        End If

        ''        'Dim pQryRev = (From it In mPartEntities.tblRev Where it.fldPNID = pPN.ID
        ''        '          Order By it.fldID Ascending Select it).ToList()


        ''        'If (pQryRev.Count() > 0) Then

        ''        '    Dim pRev_New As String = ""
        ''        '    Dim pRev_Legacy As String = ""

        ''        '    For j As Integer = 0 To pQryRev.Count() - 1
        ''        '        Dim pRev As New clsPart_PN.sRev
        ''        '        pRev.ID = pQryRev(j).fldID

        ''        '        If (Not IsNothing(pQryRev(j).fldNew) And Not IsDBNull(pQryRev(j).fldNew)) Then
        ''        '            pRev_New = pQryRev(j).fldNew
        ''        '        End If

        ''        '        If (Not IsNothing(pQryRev(j).fldLegacy) And Not IsDBNull(pQryRev(j).fldLegacy)) Then
        ''        '            pRev_Legacy = pQryRev(j).fldLegacy
        ''        '        End If

        ''        '        pRev.NewVal = pRev_New
        ''        '        pRev.Legacy = pRev_Legacy

        ''        '        pPN.Rev.Add(pRev)

        ''        '    Next

        ''        'End If

        ''        gPart_PN.Add(pPN)

        ''    Next
        ''End If

    End Sub

    Private Sub DisplayData()
        '=====================
        Try

            InitializeControl(False)
            SetDefaultData()

            If (mProjectView) Then

                If (mCustomerID <> 0) Then
                    Dim query = (From it In mPartEntities.tblCustomer
                                    Where it.fldID = mCustomerID Select it).First()

                    txtCustomer.Text = query.fldName
                    Dim pUnit As String = query.fldDimUnit
                    cmbUnit.Text = pUnit.Trim()
                    'AES 21JUL17
                    ''cmbCulturalFormat.Text = query.fldCulturalFormat.Trim()
                    ''mPartProject.CultureName = cmbCulturalFormat.Text

                    If (mPlatformID <> 0) Then
                        Dim query1 = (From it In mPartEntities.tblPlatform
                                        Where it.fldCustID = mCustomerID And
                                              it.fldID = mPlatformID
                                        Order By it.fldID Ascending Select it).First()

                        txtPlatform.Text = query1.fldName

                        If (mLocationID <> 0) Then
                            Dim query2 = (From it In mPartEntities.tblLocation
                                            Where it.fldCustID = mCustomerID And
                                                  it.fldPlatformID = mPlatformID And
                                                  it.fldID = mLocationID Select it).First()

                            txtLocation.Text = query2.fldLoc

                            If (mPNID <> 0) Then
                                Dim query3 = (From it In mPartEntities.tblPN
                                            Where it.fldID = mPNID Select it).First()

                                'AES 21MAR17
                                If (Not IsDBNull(query3.fldCurrent) And Not IsNothing(query3.fldCurrent)) Then
                                    Dim pParkerPN As String = query3.fldCurrent
                                    Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                                    cmbParkerPN_Part2.Text = pParkerPN_Prefix
                                    Dim pParkerPN_No As String = pParkerPN.Substring(5)
                                    txtParkerPN_Part3.Text = pParkerPN_No
                                    chkNew.Checked = True
                                End If

                                If (Not IsDBNull(query3.fldLegacy) And Not IsNothing(query3.fldLegacy)) Then
                                    txtParkerPN_Legacy.Text = query3.fldLegacy
                                    chkLegacy.Checked = False ''chkLegacy.Checked = True    AES 26APR17
                                Else
                                    chkLegacy.Checked = False
                                    txtParkerPN_Legacy.Text = ""
                                End If

                                'AES 21JUL17
                                ' ''....Geom Template
                                ''If (Not IsDBNull(query3.fldGeomTemplate) And Not IsNothing(query3.fldGeomTemplate)) Then
                                ''    chkGeomTemplate.Checked = query3.fldGeomTemplate
                                ''Else
                                ''    chkGeomTemplate.Checked = False
                                ''End If

                                ' ''....SealIPE
                                ''If (Not IsDBNull(query3.fldSealIPE) And Not IsNothing(query3.fldSealIPE)) Then
                                ''    chkSealIPE.Checked = query3.fldSealIPE
                                ''Else
                                ''    chkSealIPE.Checked = False
                                ''End If

                                ' ''....SealTest
                                ''If (Not IsDBNull(query3.fldSealTest) And Not IsNothing(query3.fldSealTest)) Then
                                ''    chkSealTest.Checked = query3.fldSealTest
                                ''Else
                                ''    chkSealTest.Checked = False
                                ''End If


                                If (mRevID <> 0) Then
                                    Dim query4 = (From it In mPartEntities.tblRev
                                           Where it.fldPNID = mPNID And
                                                 it.fldID = mRevID Select it).First()

                                    If (Not IsDBNull(query4.fldCurrent) And Not IsNothing(query4.fldCurrent)) Then
                                        txtPN_PH_Rev.Text = query4.fldCurrent
                                        mPartProject.PNR.Current_Rev = txtPN_PH_Rev.Text
                                    End If

                                    If (Not IsDBNull(query4.fldLegacy) And Not IsNothing(query4.fldLegacy)) Then
                                        txtParkerPNLegacy_Rev.Text = query4.fldLegacy
                                        mPartProject.PNR.Legacy_Rev = txtParkerPNLegacy_Rev.Text
                                    End If


                                    Dim query5 = (From it In mPartEntities.tblProject
                                                    Where it.fldCustID = mCustomerID And
                                                          it.fldPlatformID = mPlatformID And
                                                          it.fldLocID = mLocationID And
                                                          it.fldPNID = mPNID And
                                                          it.fldRevID = mRevID
                                                          Select it).First()

                                    txtCustomerPN.Text = query5.fldPN_Cust


                                    'If (Not IsDBNull(query5.fldSealIPEDesign) And Not IsNothing(query5.fldSealIPEDesign)) Then
                                    '    Dim pIPEDesign As Boolean = query5.fldSealIPEDesign
                                    '    If (pIPEDesign) Then
                                    '        optYes.Checked = True
                                    '    Else
                                    '        optNo.Checked = True
                                    '    End If
                                    'Else
                                    '    optYes.Checked = True

                                    'End If

                                End If

                            End If

                        End If

                    End If

                End If

            ElseIf (mPNView) Then

                If (mPNID <> 0) Then
                    Dim query3 = (From it In mPartEntities.tblPN
                                Where it.fldID = mPNID Select it).First()

                    If (Not IsDBNull(query3.fldCurrent) And Not IsNothing(query3.fldCurrent)) Then
                        If (query3.fldCurrent <> "") Then
                            Dim pParkerPN As String = query3.fldCurrent
                            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                            cmbParkerPN_Part2.Text = pParkerPN_Prefix
                            Dim pParkerPN_No As String = pParkerPN.Substring(5)
                            txtParkerPN_Part3.Text = pParkerPN_No
                            chkNew.Checked = True
                            mPartProject.PNR.Current_Exists = True
                            'mPartProject.PNR.SealTypeNo = pParkerPN_Prefix
                            mPartProject.PNR.Current_TypeNo = pParkerPN_Prefix
                            mPartProject.PNR.Current_Val = pParkerPN_No
                        End If
                    End If

                    If (Not IsDBNull(query3.fldLegacy) And Not IsNothing(query3.fldLegacy)) Then
                        If (query3.fldLegacy <> "") Then
                            txtParkerPN_Legacy.Text = query3.fldLegacy
                            chkLegacy.Checked = True    'AES 02MAY17
                        Else
                            chkLegacy.Checked = False
                            txtParkerPN_Legacy.Text = ""
                            mPartProject.PNR.Legacy_Exists = False
                            mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.None
                            mPartProject.PNR.Legacy_Val = ""
                        End If

                    Else
                        chkLegacy.Checked = False
                        txtParkerPN_Legacy.Text = ""
                    End If

                    '....Ref. Part No
                    '======================
                    If (Not IsDBNull(query3.fldRefDimCurrentExists) And Not IsNothing(query3.fldRefDimCurrentExists)) Then
                        If (query3.fldRefDimCurrentExists) Then
                            Dim pParkerPN As String = query3.fldRefDimCurrent
                            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                            cmbRefPNNewDim_Part2.Text = pParkerPN_Prefix
                            Dim pParkerPN_No As String = pParkerPN.Substring(5)
                            txtRefPNNewDim_Part3.Text = pParkerPN_No
                            chkRefDimNew_Exists.Checked = True
                            txtRefPNNewDim_Rev.Text = query3.fldRefDimCurrentRev
                        Else
                            chkRefDimNew_Exists.Checked = False
                            txtRefPNNewDim_Part3.Text = ""
                            txtRefPNNewDim_Rev.Text = ""

                        End If
                    Else
                        chkRefDimNew_Exists.Checked = False
                        txtRefPNNewDim_Part3.Text = ""
                        txtRefPNNewDim_Rev.Text = ""
                    End If

                    'If (Not IsDBNull(pQry2.fldCurrentRef_DimExists) And Not IsNothing(pQry2.fldCurrentRef_DimExists)) Then
                    '    If (pQry2.fldCurrentRef_DimExists) Then
                    '        txtRefPNNewDim_Rev.Text = pQry2.fldCurrentRef_Dim_Rev
                    '    Else
                    '        txtRefPNNewDim_Rev.Text = ""
                    '    End If
                    'Else
                    '    txtRefPNNewDim_Rev.Text = ""
                    'End If

                    If (Not IsDBNull(query3.fldRefNotesCurrentExists) And Not IsNothing(query3.fldRefNotesCurrentExists)) Then
                        If (query3.fldRefNotesCurrentExists) Then
                            Dim pParkerPN As String = query3.fldRefNotesCurrent
                            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                            cmbRefNotesNewPN_Part2.Text = pParkerPN_Prefix
                            Dim pParkerPN_No As String = pParkerPN.Substring(5)
                            txtRefPNNotes_Part3.Text = pParkerPN_No
                            chkRefDimNotes_Exists.Checked = True
                            txtRefPNNewNotes_Rev.Text = query3.fldRefNotesCurrentRev
                        Else
                            chkRefDimNotes_Exists.Checked = False
                            txtRefPNNotes_Part3.Text = ""
                            txtRefPNNewNotes_Rev.Text = ""
                        End If
                    Else
                        chkRefDimNotes_Exists.Checked = False
                        txtRefPNNotes_Part3.Text = ""
                        txtRefPNNewNotes_Rev.Text = ""
                    End If

                    'If (Not IsDBNull(pQry2.fldNewRef_Notes_Rev) And Not IsNothing(pQry2.fldNewRef_Notes_Rev)) Then
                    '    If (pQry2.fldNewRef_Notes_Rev <> "") Then
                    '        txtRefPNNewNotes_Rev.Text = pQry2.fldNewRef_Notes_Rev
                    '    Else
                    '        txtRefPNNewNotes_Rev.Text = ""
                    '    End If
                    'Else
                    '    txtRefPNNewNotes_Rev.Text = ""
                    'End If

                    If (Not IsDBNull(query3.fldRefDimLegacyExists) And Not IsNothing(query3.fldRefDimLegacyExists)) Then
                        If (query3.fldRefDimLegacyExists) Then
                            txtRefPNNewDim_Legacy.Text = query3.fldRefDimLegacy
                            chkRefDimLegacy_Exists.Checked = True
                            txtRefPNLegacyDim_Rev.Text = query3.fldRefDimLegacyRev
                        Else
                            txtRefPNNewDim_Legacy.Text = ""
                            chkRefDimLegacy_Exists.Checked = False
                            txtRefPNLegacyDim_Rev.Text = ""
                        End If
                    Else
                        txtRefPNNewDim_Legacy.Text = ""
                        chkRefDimLegacy_Exists.Checked = False
                        txtRefPNLegacyDim_Rev.Text = ""
                    End If

                    'If (Not IsDBNull(pQry2.fldLegacyRef_Dim_Rev) And Not IsNothing(pQry2.fldLegacyRef_Dim_Rev)) Then
                    '    If (pQry2.fldLegacyRef_Dim_Rev <> "") Then
                    '        txtRefPNLegacyDim_Rev.Text = pQry2.fldLegacyRef_Dim_Rev
                    '    Else
                    '        txtRefPNLegacyDim_Rev.Text = ""
                    '    End If
                    'Else
                    '    txtRefPNLegacyDim_Rev.Text = ""
                    'End If

                    If (Not IsDBNull(query3.fldRefNotesLegacyExists) And Not IsNothing(query3.fldRefNotesLegacyExists)) Then
                        If (query3.fldRefNotesLegacyExists) Then
                            txtRefPNNewNotes_Legacy.Text = query3.fldRefNotesLegacy
                            chkRefNotesLegacy_Exists.Checked = True
                            txtRefPNLegacyNotes_Rev.Text = query3.fldRefNotesLegacyRev
                        Else
                            txtRefPNNewNotes_Legacy.Text = ""
                            chkRefNotesLegacy_Exists.Checked = False
                            txtRefPNLegacyNotes_Rev.Text = ""
                        End If
                    Else
                        txtRefPNNewNotes_Legacy.Text = ""
                        chkRefNotesLegacy_Exists.Checked = False
                        txtRefPNLegacyNotes_Rev.Text = ""
                    End If

                    'If (Not IsDBNull(pQry2.fldLegacyRef_Notes_Rev) And Not IsNothing(pQry2.fldLegacyRef_Notes_Rev)) Then
                    '    If (pQry2.fldLegacyRef_Notes_Rev <> "") Then
                    '        txtRefPNLegacyNotes_Rev.Text = pQry2.fldLegacyRef_Notes_Rev
                    '    Else
                    '        txtRefPNLegacyNotes_Rev.Text = ""
                    '    End If
                    'Else
                    '    txtRefPNLegacyNotes_Rev.Text = ""
                    'End If

                    '======================

                    'AES 21JUL17
                    '....Geom Template
                    ''If (Not IsDBNull(query3.fldGeomTemplate) And Not IsNothing(query3.fldGeomTemplate)) Then
                    ''    chkGeomTemplate.Checked = query3.fldGeomTemplate
                    ''Else
                    ''    chkGeomTemplate.Checked = False
                    ''End If




                    If (mRevID <> 0) Then
                        Dim query4 = (From it In mPartEntities.tblRev
                               Where it.fldPNID = mPNID And
                                     it.fldID = mRevID Select it).First()

                        If (Not IsDBNull(query4.fldCurrent) And Not IsNothing(query4.fldCurrent)) Then
                            txtPN_PH_Rev.Text = query4.fldCurrent
                            mPartProject.PNR.Current_Rev = txtPN_PH_Rev.Text
                        End If

                        If (Not IsDBNull(query4.fldLegacy) And Not IsNothing(query4.fldLegacy)) Then
                            txtParkerPNLegacy_Rev.Text = query4.fldLegacy
                            mPartProject.PNR.Legacy_Rev = txtParkerPNLegacy_Rev.Text
                        End If

                        ' ''....SealIPE
                        ''If (Not IsDBNull(query4.fldSealIPE) And Not IsNothing(query4.fldSealIPE)) Then
                        ''    chkSealIPE.Checked = query4.fldSealIPE
                        ''Else
                        ''    chkSealIPE.Checked = False
                        ''End If

                        ' ''....SealTest
                        ''If (Not IsDBNull(query4.fldSealTest) And Not IsNothing(query4.fldSealTest)) Then
                        ''    chkSealTest.Checked = query4.fldSealTest
                        ''Else
                        ''    chkSealTest.Checked = False
                        ''End If

                        If (mCustomerID <> 0) Then
                            Dim query = (From it In mPartEntities.tblCustomer
                                            Where it.fldID = mCustomerID Select it).First()

                            txtCustomer.Text = query.fldName
                            Dim pUnit As String = query.fldDimUnit
                            cmbUnit.Text = pUnit.Trim()
                            ''cmbCulturalFormat.Text = query.fldCulturalFormat.Trim()
                            ''mPartProject.CultureName = cmbCulturalFormat.Text

                            If (mPlatformID <> 0) Then
                                Dim query1 = (From it In mPartEntities.tblPlatform
                                                Where it.fldCustID = mCustomerID And
                                                      it.fldID = mPlatformID
                                                Order By it.fldID Ascending Select it).First()

                                txtPlatform.Text = query1.fldName

                                If (mLocationID <> 0) Then
                                    Dim query2 = (From it In mPartEntities.tblLocation
                                                    Where it.fldCustID = mCustomerID And
                                                          it.fldPlatformID = mPlatformID And
                                                          it.fldID = mLocationID Select it).First()

                                    txtLocation.Text = query2.fldLoc


                                    Dim query5 = (From it In mPartEntities.tblProject
                                                    Where it.fldCustID = mCustomerID And
                                                          it.fldPlatformID = mPlatformID And
                                                          it.fldLocID = mLocationID And
                                                          it.fldPNID = mPNID And
                                                          it.fldRevID = mRevID
                                                          Select it).First()

                                    txtCustomerPN.Text = query5.fldPN_Cust
                                End If

                            End If
                        End If  'customer

                    End If
                End If

            End If

            PopulateTreeView()
            trvProjects.ExpandAll()

            SelectTreeNode()

        Catch ex As Exception
            MessageBox.Show("Error occurred while establishing a connection to SQL Server.", "Database Connection Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            gDBConnectionState = False
            Me.Close()
        End Try

    End Sub


    Private Function SearchTheTreeView(ByVal TReeView_In As TreeView, ByVal TextToFind_In As String) As TreeNode
        '=====================================================================================================
        '....Empty previous
        NodesThatMatch.Clear()

        ' Keep calling RecursiveSearch
        For Each TN As TreeNode In TReeView_In.Nodes
            If TN.Text = TextToFind_In Then
                NodesThatMatch.Add(TN)
            End If

            RecursiveSearch(TN, TextToFind_In)
        Next

        If NodesThatMatch.Count > 0 Then
            Return NodesThatMatch(0)
        Else
            Return Nothing
        End If

    End Function


    Private Sub RecursiveSearch(ByVal TreeNode_In As TreeNode, ByVal TextToFind_In As String)
        '=====================================================================================

        '....Keep calling the test recursively.
        For Each TN As TreeNode In TreeNode_In.Nodes
            If TN.Text = TextToFind_In Then
                NodesThatMatch.Add(TN)
            End If

            RecursiveSearch(TN, TextToFind_In)
        Next
    End Sub


    Private Sub SelectTreeNode()
        '=======================

        Try

            If (mProjectView) Then

                ''If (mPartProject.Customer.CustName <> "") Then
                ''    Dim pNode As TreeNode = SearchTheTreeView(trvProjects, mPartProject.Customer.CustName)
                ''    Dim pChildNode As TreeNode = Nothing

                ''    '....Platform
                ''    For i As Integer = 0 To pNode.Nodes.Count - 1
                ''        If (pNode.Nodes(i).Text = mPartProject.Customer.PlatName) Then
                ''            pChildNode = pNode.Nodes(i)
                ''            Exit For
                ''        End If
                ''    Next

                ''    '....Location
                ''    Dim pGrandChildNode As TreeNode = Nothing

                ''    If (Not IsNothing(pChildNode)) Then
                ''        For j As Integer = 0 To pChildNode.Nodes.Count - 1
                ''            If (pChildNode.Nodes(j).Text = mPartProject.Customer.LocName) Then
                ''                pGrandChildNode = pChildNode.Nodes(j)
                ''                Exit For
                ''            End If
                ''        Next

                ''        '....PN
                ''        Dim pGrandGrandChildNode As TreeNode = Nothing

                ''        If (Not IsNothing(pGrandChildNode)) Then
                ''            For k As Integer = 0 To pGrandChildNode.Nodes.Count - 1
                ''                If (pGrandChildNode.Nodes(k).Text = mPartProject.PNR.PN()) Then
                ''                    pGrandGrandChildNode = pGrandChildNode.Nodes(k)
                ''                    Exit For
                ''                End If
                ''            Next

                ''            '....Rev
                ''            If (Not IsNothing(pGrandGrandChildNode)) Then
                ''                Dim pGGrandGrandChildNode As TreeNode = Nothing
                ''                For l As Integer = 0 To pGrandGrandChildNode.Nodes.Count - 1
                ''                    If (pGrandGrandChildNode.Nodes(l).Text = mPartProject.PNR.PN_Rev()) Then
                ''                        pGGrandGrandChildNode = pGrandGrandChildNode.Nodes(l)
                ''                        Exit For
                ''                    End If
                ''                Next

                ''                If (Not IsNothing(pGGrandGrandChildNode)) Then
                ''                    trvProjects.SelectedNode = pGGrandGrandChildNode
                ''                    trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
                ''                    trvProjects.Select()
                ''                End If
                ''            End If

                ''        End If

                ''    End If

                ''End If

            ElseIf (mPNView) Then

                If (mPNID <> 0) Then

                    Dim pNode As TreeNode = Nothing
                    Dim pChildNode As TreeNode = Nothing
                    Dim pPN As String = mPartProject.PNR.PN

                    If (mPartProject.PNR.Legacy.Exists) Then

                        If (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                            pNode = SearchTheTreeView(trvProjects, "Legacy-Catalogued")
                        Else
                            pNode = SearchTheTreeView(trvProjects, "Legacy-Other")
                        End If

                        'pPN = mProject.PN_Legacy
                    Else
                        pNode = SearchTheTreeView(trvProjects, "New")
                    End If

                    '....PN
                    For i As Integer = 0 To pNode.Nodes.Count - 1
                        If (pNode.Nodes(i).Text = pPN) Then
                            pChildNode = pNode.Nodes(i)
                            Exit For
                        End If
                    Next

                    '....Rev
                    Dim pGrandChildNode As TreeNode = Nothing

                    If (Not IsNothing(pChildNode)) Then
                        For j As Integer = 0 To pChildNode.Nodes.Count - 1
                            If (pChildNode.Nodes(j).Text = mPartProject.PNR.PN_Rev) Then
                                pGrandChildNode = pChildNode.Nodes(j)
                                Exit For
                            End If
                        Next

                        If (Not IsNothing(pGrandChildNode)) Then
                            trvProjects.SelectedNode = pGrandChildNode
                            trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
                            trvProjects.Select()
                        End If
                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region


#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbCulturalFormat_MouseHover(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs)

        '======================================================================
        '....For Display Sample

        ''Dim pCultureName As String = cmbCulturalFormat.Text.Trim()

        ' ''....Culture Independent
        ''Dim pCI As CultureInfo = CultureInfo.CurrentCulture
        ''Select Case pCultureName
        ''    Case "USA"
        ''        pCI = New CultureInfo("en-US")

        ''    Case "UK"
        ''        pCI = New CultureInfo("en-GB")

        ''    Case "Germany"
        ''        pCI = New CultureInfo("de-DE")

        ''    Case "France"
        ''        pCI = New CultureInfo("fr-FR")
        ''End Select

        ''Dim pNumFormat As NumberFormatInfo
        ''Dim pDateFormat As String, pTimeFormat As String

        ''With pCI
        ''    pNumFormat = .NumberFormat()
        ''    pDateFormat = .DateTimeFormat().ShortDatePattern()
        ''    pTimeFormat = .DateTimeFormat().LongTimePattern()
        ''End With


        ''Dim pDate As String = Now.ToString(pDateFormat)
        ''Dim pTime As Date = Now.ToString(pTimeFormat)

        ''Dim pStr As String = "Decimal Format:" & Space(8) & "99" & _
        ''                     pNumFormat.NumberDecimalSeparator & "99" & _
        ''                     vbCrLf & vbCrLf & _
        ''                     "Digit Group Format:" & Space(2) & "123" & _
        ''                     pNumFormat.NumberGroupSeparator & "456" & _
        ''                     vbCrLf & vbCrLf & _
        ''                     "Date Format:" & Space(12) & _
        ''                     pDate & vbCrLf & vbCrLf & _
        ''                     "Time Format:" & Space(12) & pTime

        ''tTipFormat.SetToolTip(cmbCulturalFormat, pStr)

    End Sub


    Private Sub cmbCulturalFormat_SelectedIndexChanged(sender As System.Object,
                                                       e As System.EventArgs)
        '====================================================================================================================
        ''mPartProject.CultureName = cmbCulturalFormat.Text
    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                 Handles cmdSealProcess.Click, cmdSealTest.Click, cmdSealIPE.Click
        '===========================================================================================
        Dim pcmdButton As Button = CType(sender, Button)

        Select Case pcmdButton.Name

            Case "cmdSealProcess"
                'SetDefaultData()
                SaveData()
                gUser.RetrieveUserTitle()

                Dim pUserRole As New List(Of String)
                pUserRole = gUser.RetrieveProcessUserRoles()

                gIsProcessMainActive = True     'AES 17APR18
                Dim pfrmProcessRoleSelection As New Process_frmRoleSelection()
                pfrmProcessRoleSelection.ShowDialog()

                SelectTreeNode()
                'If (pUserRole.Count > 0) Then
                '    Dim pfrmProcessRoleSelection As New Process_frmRoleSelection()
                '    pfrmProcessRoleSelection.ShowDialog()
                'Else
                '    gUser.Role = ""
                '    Dim pProcess_frmMain As New Process_frmMain()
                '    pProcess_frmMain.Size = New Size(1130, 700)
                '    pProcess_frmMain.AutoScroll = True
                '    pProcess_frmMain.ShowDialog()
                'End If

            Case "cmdSealTest"
                SetDefaultData()
                SaveData()


                gTest_User.RetrieveUserRoles()

                Dim pRoleCount As Integer = 0

                If (gTest_User.Admin) Then
                    pRoleCount = pRoleCount + 1
                End If

                If (gTest_User.Tester) Then
                    pRoleCount = pRoleCount + 1
                End If

                If (gTest_User.Engg) Then
                    pRoleCount = pRoleCount + 1
                End If

                If (gTest_User.Quality) Then
                    pRoleCount = pRoleCount + 1
                End If

                If (pRoleCount > 1) Then

                    Dim pfrmTestUserRole As New Test_frmUser_Role()
                    pfrmTestUserRole.ShowDialog()

                Else
                    gTest_User.Role = Test_clsUser.eRole.Viewer
                    gTest_frmMain.ShowDialog()
                End If

            Case "cmdSealIPE"
                SetDefaultData()
                SaveData()

                'AES 19MAY17
                gIPE_Project.Analysis.Clear()


                'gIPE_Project.Project_ID = pfrmPartInfo.IPEProjectID
                gIPE_Project.Retrive_FromPNR(gIPE_Project.Project_ID)

                Dim pSealIPEEntities As New SealIPEDBEntities()
                Dim pQryAnalysis = (From pRec In pSealIPEEntities.tblAnalysis
                                    Where pRec.fldProjectID = gIPE_Project.Project_ID Order By pRec.fldID Ascending Select pRec).ToList()

                Dim pCount As Integer = pQryAnalysis.Count()
                For i As Integer = 0 To pCount - 1
                    gIPE_Project.Add_Analysis()
                    gIPE_Project.Analysis(i).ID = pQryAnalysis(i).fldID
                    gIPE_Project.Analysis(i).Retrieve_FromDB(gIPE_Unit, gIPE_ANSYS)
                Next

                Me.Cursor = Cursors.Default

                gIPE_frmAnalysisSet.ShowDialog()

        End Select

    End Sub

    'Private Sub SetUserRole()
    '    '=====================
    '    Dim pSealSuiteEntities As New SealSuiteDBEntities()

    '    Dim pRecCount As Integer = (From pRec In pSealSuiteEntities.tblUser
    '                                Where pRec.fldSystemLogin = gUser.SystemLogin Select pRec).Count()
    '    Dim pUserID As Integer = 0
    '    Dim pRoleID As Integer = 0
    '    Dim pRole As String = ""
    '    If (pRecCount > 0) Then

    '        Dim pQry = (From pRec In pSealSuiteEntities.tblUser
    '                    Where pRec.fldSystemLogin = gUser.SystemLogin Select pRec).First()

    '        pUserID = pQry.fldID

    '        Dim pQryProcess_UserRole_Count As Integer = (From pRec In pSealSuiteEntities.tblProcess_UserRole
    '                                                     Where pRec.fldUserID = pUserID Select pRec).Count()
    '        If (pQryProcess_UserRole_Count > 0) Then

    '            Dim pQryProcess_UserRole = (From pRec In pSealSuiteEntities.tblProcess_UserRole
    '                                        Where pRec.fldUserID = pUserID Select pRec).First()

    '            pRoleID = pQryProcess_UserRole.fldRoleID

    '            Dim pQry_Role_Count As Integer = (From pRec In pSealSuiteEntities.tblRole
    '                                              Where pRec.fldID = pRoleID Select pRec).Count()

    '            If (pQry_Role_Count > 0) Then

    '                Dim pQry_Role = (From pRec In pSealSuiteEntities.tblRole
    '                                 Where pRec.fldID = pRoleID Select pRec).First()

    '                pRole = pQry_Role.fldRole.ToString().Trim()
    '            End If

    '        End If

    '    End If

    '    gUser.Role = pRole

    'End Sub

    Private Sub cmdUpDown_Click(sender As System.Object, e As System.EventArgs) Handles cmdUp.Click, cmdDown.Click
        '==========================================================================================================
        'Dim pPartEntities As New SealPartDBEntities()

        'Dim pcmdButton As Button = CType(sender, Button)
        'Dim pblnUp As Boolean = False
        'Select Case pcmdButton.Name
        '    Case "cmdUp"
        '        pblnUp = True
        '    Case "cmdDown"
        '        pblnUp = False
        'End Select

        'If (trvProjects.Nodes.Count > 0) Then
        '    '....Level 1
        '    If (trvProjects.SelectedNode.Level = 1) Then
        '        Dim pPN As String = trvProjects.SelectedNode.Text
        '        Dim pParentNode As String = trvProjects.SelectedNode.Parent.Text
        '        Dim pPNID As Integer = Convert.ToInt64(trvProjects.SelectedNode.Tag)

        '        Dim pListPNIndex As New List(Of Integer)
        '        Dim pQryPNIndex = (From pRec In pPartEntities.tblPN_Index
        '                                            Order By pRec.fldIndex Ascending Select pRec).ToList()
        '        If (pQryPNIndex.Count() > 0) Then
        '            For i As Integer = 0 To pQryPNIndex.Count() - 1
        '                pListPNIndex.Add(pQryPNIndex(i).fldIndex)
        '            Next
        '        End If
        '        Dim pQry = (From pRec In pPartEntities.tblPN_Index
        '                                     Where pRec.fldID = pPNID Select pRec).First()

        '        Dim pPNIndex As Integer = pQry.fldIndex

        '        If (pblnUp) Then
        '            For i As Integer = pListPNIndex.Count - 1 To 0 Step -1
        '                If (pPNIndex > pListPNIndex(i)) Then
        '                    Dim pIndex As Integer = pListPNIndex(i)
        '                    Dim pQryPN_Index = (From Rec In mPartEntities.tblPN_Index
        '                                Where Rec.fldID = pPNID And Rec.fldIndex = pPNIndex).First()
        '                    pQryPN_Index.fldIndex = pIndex

        '                    Dim pPN_Prev = (From Rec In mPartEntities.tblPN_Index
        '                                Where Rec.fldIndex = pIndex).First()
        '                    pPN_Prev.fldIndex = pPNIndex
        '                    mPartEntities.SaveChanges()

        '                    PopulateTreeView()
        '                    SelectTreeNode(1, pPN, pParentNode)
        '                    Exit For

        '                End If
        '            Next
        '        End If

        '    End If

        'End If


        ''If (trvProjects.Nodes.Count > 0) Then
        ''    '....Level 0    Customer
        ''    If (trvProjects.SelectedNode.Level = 0) Then
        ''        Dim pSelectedNode As String = trvProjects.SelectedNode.Text        '0 Customer

        ''        Dim pCustName As String = txtCustomer.Text
        ''        Dim pCustomerID As Integer = mPartProject.GetCustID(pCustName)

        ''        Dim pListCustIndex As New List(Of Integer)

        ''        Dim pQryCust = (From pRec In pPartEntities.tblCustomer
        ''                                    Order By pRec.fldIndex Ascending Select pRec).ToList()
        ''        If (pQryCust.Count() > 0) Then
        ''            For i As Integer = 0 To pQryCust.Count() - 1
        ''                pListCustIndex.Add(pQryCust(i).fldIndex)
        ''            Next
        ''        End If

        ''        Dim pQry = (From pRec In pPartEntities.tblCustomer
        ''                                     Where pRec.fldID = pCustomerID Select pRec).First()

        ''        Dim pCustIndex As Integer = pQry.fldIndex

        ''        If (pblnUp) Then
        ''            For i As Integer = pListCustIndex.Count - 1 To 0 Step -1
        ''                If (pCustIndex > pListCustIndex(i)) Then
        ''                    Dim pIndex As Integer = pListCustIndex(i)
        ''                    Dim pCustomer = (From Cust In mPartEntities.tblCustomer
        ''                                Where Cust.fldID = pCustomerID And Cust.fldIndex = pCustIndex).First()
        ''                    pCustomer.fldIndex = pIndex

        ''                    Dim pCustomer_Prev = (From Cust In mPartEntities.tblCustomer
        ''                                Where Cust.fldIndex = pIndex).First()
        ''                    pCustomer_Prev.fldIndex = pCustIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(0, pSelectedNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        Else
        ''            For i As Integer = 0 To pListCustIndex.Count - 1
        ''                If (pCustIndex < pListCustIndex(i)) Then
        ''                    Dim pIndex As Integer = pListCustIndex(i)
        ''                    Dim pCustomer = (From Cust In mPartEntities.tblCustomer
        ''                                Where Cust.fldID = pCustomerID And Cust.fldIndex = pCustIndex).First()
        ''                    pCustomer.fldIndex = pIndex

        ''                    Dim pCustomer_Prev = (From Cust In mPartEntities.tblCustomer
        ''                                          Where Cust.fldIndex = pIndex).First()
        ''                    pCustomer_Prev.fldIndex = pCustIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(0, pSelectedNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        End If

        ''        '....Level 1    Platform
        ''    ElseIf (trvProjects.SelectedNode.Level = 1) Then
        ''        Dim pSelectedNode As String = trvProjects.SelectedNode.Text        '1 Platform
        ''        Dim pParentNode As String = trvProjects.SelectedNode.Parent.Text   '0 Customer

        ''        Dim pCustName As String = txtCustomer.Text
        ''        Dim pCustomerID As Integer = mPartProject.GetCustID(pCustName)

        ''        Dim pPlatName As String = txtPlatform.Text
        ''        Dim pPlatID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatName)

        ''        Dim pListPlatIndex As New List(Of Integer)

        ''        Dim pQryPlat = (From pRec In pPartEntities.tblPlatform Where pRec.fldCustID = pCustomerID
        ''                                   Select pRec).ToList()
        ''        If (pQryPlat.Count() > 0) Then
        ''            For i As Integer = 0 To pQryPlat.Count() - 1
        ''                pListPlatIndex.Add(pQryPlat(i).fldIndex)
        ''            Next
        ''        End If

        ''        Dim pQry = (From pRec In pPartEntities.tblPlatform
        ''                                     Where pRec.fldCustID = pCustomerID And pRec.fldID = pPlatID Select pRec).First()

        ''        Dim pPlatIndex As Integer = pQry.fldIndex

        ''        If (pblnUp) Then
        ''            For i As Integer = pListPlatIndex.Count - 1 To 0 Step -1
        ''                If (pPlatIndex > pListPlatIndex(i)) Then
        ''                    Dim pIndex As Integer = pListPlatIndex(i)
        ''                    Dim pPlatform = (From Plat In mPartEntities.tblPlatform
        ''                                Where Plat.fldCustID = pCustomerID And Plat.fldID = pPlatID And Plat.fldIndex = pPlatIndex).First()
        ''                    pPlatform.fldIndex = pIndex

        ''                    Dim pPlat_Prev = (From Plat In mPartEntities.tblPlatform
        ''                                Where Plat.fldCustID = pCustomerID And Plat.fldIndex = pIndex).First()
        ''                    pPlat_Prev.fldIndex = pPlatIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(1, pSelectedNode, pParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        Else
        ''            For i As Integer = 0 To pListPlatIndex.Count - 1
        ''                If (pPlatIndex < pListPlatIndex(i)) Then
        ''                    Dim pIndex As Integer = pListPlatIndex(i)
        ''                    Dim pPlatform = (From Plat In mPartEntities.tblPlatform
        ''                                Where Plat.fldCustID = pCustomerID And Plat.fldID = pPlatID And Plat.fldIndex = pPlatIndex).First()
        ''                    pPlatform.fldIndex = pIndex

        ''                    Dim pPlat_Prev = (From Plat In mPartEntities.tblPlatform
        ''                                Where Plat.fldCustID = pCustomerID And Plat.fldIndex = pIndex).First()
        ''                    pPlat_Prev.fldIndex = pPlatIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(1, pSelectedNode, pParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        End If

        ''        '....Level 2    Location
        ''    ElseIf (trvProjects.SelectedNode.Level = 2) Then

        ''        Dim pSelectedNode As String = trvProjects.SelectedNode.Text        '2 Location
        ''        Dim pParentNode As String = trvProjects.SelectedNode.Parent.Text   '1 Platform
        ''        Dim pGParentNode As String = trvProjects.SelectedNode.Parent.Parent.Text   '0 Customer

        ''        Dim pCustName As String = txtCustomer.Text
        ''        Dim pCustomerID As Integer = mPartProject.GetCustID(pCustName)

        ''        Dim pPlatName As String = txtPlatform.Text
        ''        Dim pPlatID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatName)

        ''        Dim pLocName As String = txtLocation.Text
        ''        Dim pLocID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatID, pLocName)

        ''        Dim pListLocIndex As New List(Of Integer)

        ''        Dim pQryLoc = (From pRec In pPartEntities.tblLocation Where pRec.fldCustID = pCustomerID And pRec.fldPlatformID = pPlatID
        ''                                    Order By pRec.fldIndex Ascending Select pRec).ToList()
        ''        If (pQryLoc.Count() > 0) Then
        ''            For i As Integer = 0 To pQryLoc.Count() - 1
        ''                pListLocIndex.Add(pQryLoc(i).fldIndex)
        ''            Next
        ''        End If

        ''        Dim pQry = (From pRec In pPartEntities.tblLocation
        ''                                     Where pRec.fldCustID = pCustomerID And pRec.fldPlatformID = pPlatID And pRec.fldID = pLocID Select pRec).First()

        ''        Dim pLocIndex As Integer = pQry.fldIndex

        ''        If (pblnUp) Then
        ''            For i As Integer = pListLocIndex.Count - 1 To 0 Step -1
        ''                If (pLocIndex > pListLocIndex(i)) Then
        ''                    Dim pIndex As Integer = pListLocIndex(i)
        ''                    Dim pLocation = (From Loc In mPartEntities.tblLocation
        ''                                Where Loc.fldCustID = pCustomerID And Loc.fldPlatformID = pPlatID And Loc.fldID = pLocID And Loc.fldIndex = pLocIndex).First()
        ''                    pLocation.fldIndex = pIndex

        ''                    Dim pLoc_Prev = (From Loc In mPartEntities.tblLocation
        ''                                Where Loc.fldCustID = pCustomerID And Loc.fldPlatformID = pPlatID And Loc.fldIndex = pIndex).First()
        ''                    pLoc_Prev.fldIndex = pLocIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(2, pSelectedNode, pParentNode, pGParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        Else
        ''            For i As Integer = 0 To pListLocIndex.Count - 1
        ''                If (pLocIndex < pListLocIndex(i)) Then
        ''                    Dim pIndex As Integer = pListLocIndex(i)
        ''                    Dim pLocation = (From Loc In mPartEntities.tblLocation
        ''                                Where Loc.fldCustID = pCustomerID And Loc.fldPlatformID = pPlatID And Loc.fldID = pLocID And Loc.fldIndex = pLocIndex).First()
        ''                    pLocation.fldIndex = pIndex

        ''                    Dim pLoc_Prev = (From Loc In mPartEntities.tblLocation
        ''                                Where Loc.fldCustID = pCustomerID And Loc.fldPlatformID = pPlatID And Loc.fldIndex = pIndex).First()
        ''                    pLoc_Prev.fldIndex = pLocIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(2, pSelectedNode, pParentNode, pGParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        End If

        ''        '....Level 3    ParkerPN
        ''    ElseIf (trvProjects.SelectedNode.Level = 3) Then

        ''        Dim pSelectedNode As String = trvProjects.SelectedNode.Text        '3 PN
        ''        Dim pParentNode As String = trvProjects.SelectedNode.Parent.Text   '2 Location
        ''        Dim pGParentNode As String = trvProjects.SelectedNode.Parent.Parent.Text   '1 Platform
        ''        Dim pGGParentNode As String = trvProjects.SelectedNode.Parent.Parent.Parent.Text   '0 Customer

        ''        Dim pCustName As String = txtCustomer.Text
        ''        Dim pCustomerID As Integer = mPartProject.GetCustID(pCustName)

        ''        Dim pPlatName As String = txtPlatform.Text
        ''        Dim pPlatID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatName)

        ''        Dim pLocName As String = txtLocation.Text
        ''        Dim pLocID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatID, pLocName)

        ''        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
        ''        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

        ''        Dim pListPNIndex As New List(Of Integer)

        ''        Dim pQryPN = (From pRec In pPartEntities.tblProject Where pRec.fldCustID = pCustomerID And pRec.fldPlatformID = pPlatID And pRec.fldLocID = pLocID
        ''                                    Order By pRec.fldPNIndex Ascending Select pRec).ToList()
        ''        If (pQryPN.Count() > 0) Then
        ''            For i As Integer = 0 To pQryPN.Count() - 1
        ''                pListPNIndex.Add(pQryPN(i).fldPNIndex)
        ''            Next
        ''        End If

        ''        Dim pQry = (From pRec In pPartEntities.tblProject
        ''                                     Where pRec.fldCustID = pCustomerID And pRec.fldPlatformID = pPlatID And pRec.fldLocID = pLocID And pRec.fldPNID = pPNID Select pRec).First()

        ''        Dim pPNIndex As Integer = pQry.fldPNIndex

        ''        If (pblnUp) Then
        ''            For i As Integer = pListPNIndex.Count - 1 To 0 Step -1
        ''                If (pPNIndex > pListPNIndex(i)) Then
        ''                    Dim pIndex As Integer = pListPNIndex(i)
        ''                    Dim pProject = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNID = pPNID And Proj.fldPNIndex = pPNIndex).First()
        ''                    pProject.fldPNIndex = pIndex

        ''                    Dim pProj_Prev = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNIndex = pIndex).First()
        ''                    pProj_Prev.fldPNIndex = pPNIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(3, pSelectedNode, pParentNode, pGParentNode, pGGParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        Else
        ''            For i As Integer = 0 To pListPNIndex.Count - 1
        ''                If (pPNIndex < pListPNIndex(i)) Then
        ''                    Dim pIndex As Integer = pListPNIndex(i)
        ''                    Dim pProject = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNID = pPNID And Proj.fldPNIndex = pPNIndex).First()
        ''                    pProject.fldPNIndex = pIndex

        ''                    Dim pProj_Prev = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNIndex = pIndex).First()
        ''                    pProj_Prev.fldPNIndex = pPNIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(3, pSelectedNode, pParentNode, pGParentNode, pGGParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        End If

        ''        '....Level 4    ParkerPNRev
        ''    ElseIf (trvProjects.SelectedNode.Level = 4) Then

        ''        Dim pSelectedNode As String = trvProjects.SelectedNode.Text        '4 Rev
        ''        Dim pParentNode As String = trvProjects.SelectedNode.Parent.Text   '3 PN
        ''        Dim pGParentNode As String = trvProjects.SelectedNode.Parent.Parent.Text   '2 Location
        ''        Dim pGGParentNode As String = trvProjects.SelectedNode.Parent.Parent.Parent.Text   '1 Platform
        ''        Dim pGGGParentNode As String = trvProjects.SelectedNode.Parent.Parent.Parent.Parent.Text   '0 Customer

        ''        Dim pCustName As String = txtCustomer.Text
        ''        Dim pCustomerID As Integer = mPartProject.GetCustID(pCustName)

        ''        Dim pPlatName As String = txtPlatform.Text
        ''        Dim pPlatID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatName)

        ''        Dim pLocName As String = txtLocation.Text
        ''        Dim pLocID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatID, pLocName)

        ''        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
        ''        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

        ''        Dim pRevName As String = txtPN_PH_Rev.Text
        ''        Dim pPNRevID As Integer = mPartProject.GetRevID(pPNID, pRevName)

        ''        Dim pListRevIndex As New List(Of Integer)

        ''        Dim pQryRev = (From pRec In pPartEntities.tblProject Where pRec.fldCustID = pCustomerID And pRec.fldPlatformID = pPlatID And pRec.fldLocID = pLocID And pRec.fldPNID = pPNID
        ''                                    Order By pRec.fldRevIndex Ascending Select pRec).ToList()
        ''        If (pQryRev.Count() > 0) Then
        ''            For i As Integer = 0 To pQryRev.Count() - 1
        ''                pListRevIndex.Add(pQryRev(i).fldRevIndex)
        ''            Next
        ''        End If

        ''        Dim pQry = (From pRec In pPartEntities.tblProject
        ''                                     Where pRec.fldCustID = pCustomerID And pRec.fldPlatformID = pPlatID And pRec.fldLocID = pLocID And pRec.fldPNID = pPNID And pRec.fldRevID = pPNRevID Select pRec).First()

        ''        Dim pRevIndex As Integer = pQry.fldRevIndex

        ''        If (pblnUp) Then
        ''            For i As Integer = pListRevIndex.Count - 1 To 0 Step -1
        ''                If (pRevIndex > pListRevIndex(i)) Then
        ''                    Dim pIndex As Integer = pListRevIndex(i)
        ''                    Dim pProject = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNID = pPNID And Proj.fldRevIndex = pRevIndex).First()
        ''                    pProject.fldRevIndex = pIndex

        ''                    Dim pProj_Prev = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNID = pPNID And Proj.fldRevIndex = pIndex).First()
        ''                    pProj_Prev.fldRevIndex = pRevIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(4, pSelectedNode, pParentNode, pGParentNode, pGGParentNode, pGGGParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        Else
        ''            For i As Integer = 0 To pListRevIndex.Count - 1
        ''                If (pRevIndex < pListRevIndex(i)) Then
        ''                    Dim pIndex As Integer = pListRevIndex(i)
        ''                    Dim pProject = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNID = pPNID And Proj.fldRevIndex = pRevIndex).First()
        ''                    pProject.fldRevIndex = pIndex

        ''                    Dim pProj_Prev = (From Proj In mPartEntities.tblProject
        ''                                Where Proj.fldCustID = pCustomerID And Proj.fldPlatformID = pPlatID And Proj.fldLocID = pLocID And Proj.fldPNID = pPNID And Proj.fldRevIndex = pIndex).First()
        ''                    pProj_Prev.fldRevIndex = pRevIndex
        ''                    mPartEntities.SaveChanges()

        ''                    PopulateTreeView()
        ''                    SelectTreeNode(4, pSelectedNode, pParentNode, pGParentNode, pGGParentNode, pGGGParentNode)
        ''                    Exit For
        ''                End If
        ''            Next

        ''        End If

        ''    End If

        ''End If

    End Sub


    Private Sub SelectTreeNode(ByVal NodeLavel_In As Integer, ByVal SelectedNode_In As String,
                               Optional ByVal ParentNode_In As String = "", Optional ByVal GrandParentNode_In As String = "",
                               Optional ByVal GGrandParentNode_In As String = "", Optional ByVal GGGrandParentNode_In As String = "")
        '============================================================================================================================
        If (NodeLavel_In = 0) Then

            Dim pNode As TreeNode = Nothing
            pNode = SearchTheTreeView(trvProjects, SelectedNode_In)

            trvProjects.SelectedNode = pNode
            trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
            trvProjects.Select()

        ElseIf (NodeLavel_In = 1) Then
            Dim pNode As TreeNode = Nothing
            pNode = SearchTheTreeView(trvProjects, ParentNode_In)
            Dim pChildNode As TreeNode = Nothing

            '....Platform
            For i1 As Integer = 0 To pNode.Nodes.Count - 1
                If (pNode.Nodes(i1).Text = SelectedNode_In) Then
                    pChildNode = pNode.Nodes(i1)
                    Exit For
                End If
            Next

            trvProjects.SelectedNode = pChildNode
            trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
            trvProjects.Select()

        ElseIf (NodeLavel_In = 2) Then

            Dim pNode As TreeNode = Nothing
            pNode = SearchTheTreeView(trvProjects, GrandParentNode_In)
            Dim pChildNode As TreeNode = Nothing

            '....Platform
            For i1 As Integer = 0 To pNode.Nodes.Count - 1
                If (pNode.Nodes(i1).Text = ParentNode_In) Then
                    pChildNode = pNode.Nodes(i1)
                    Exit For
                End If
            Next

            '....Location
            Dim pGrandChildNode As TreeNode = Nothing
            For j As Integer = 0 To pChildNode.Nodes.Count - 1
                If (pChildNode.Nodes(j).Text = SelectedNode_In) Then
                    pGrandChildNode = pChildNode.Nodes(j)
                    Exit For
                End If
            Next

            trvProjects.SelectedNode = pGrandChildNode
            trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
            trvProjects.Select()


        ElseIf (NodeLavel_In = 3) Then
            Dim pNode As TreeNode = Nothing
            pNode = SearchTheTreeView(trvProjects, GGrandParentNode_In)
            Dim pChildNode As TreeNode = Nothing

            '....Platform
            For i1 As Integer = 0 To pNode.Nodes.Count - 1
                If (pNode.Nodes(i1).Text = GrandParentNode_In) Then
                    pChildNode = pNode.Nodes(i1)
                    Exit For
                End If
            Next

            '....Location
            Dim pGrandChildNode As TreeNode = Nothing
            For j As Integer = 0 To pChildNode.Nodes.Count - 1
                If (pChildNode.Nodes(j).Text = ParentNode_In) Then
                    pGrandChildNode = pChildNode.Nodes(j)
                    Exit For
                End If
            Next

            '....PN
            Dim pGGrandChildNode As TreeNode = Nothing
            For k As Integer = 0 To pGrandChildNode.Nodes.Count - 1
                If (pGrandChildNode.Nodes(k).Text = SelectedNode_In) Then
                    pGGrandChildNode = pGrandChildNode.Nodes(k)
                    Exit For
                End If
            Next

            trvProjects.SelectedNode = pGGrandChildNode
            trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
            trvProjects.Select()


        ElseIf (NodeLavel_In = 4) Then

            Dim pNode As TreeNode = Nothing
            pNode = SearchTheTreeView(trvProjects, GGGrandParentNode_In)
            Dim pChildNode As TreeNode = Nothing

            '....Platform
            For i1 As Integer = 0 To pNode.Nodes.Count - 1
                If (pNode.Nodes(i1).Text = GGrandParentNode_In) Then
                    pChildNode = pNode.Nodes(i1)
                    Exit For
                End If
            Next

            '....Location
            Dim pGrandChildNode As TreeNode = Nothing
            For j As Integer = 0 To pChildNode.Nodes.Count - 1
                If (pChildNode.Nodes(j).Text = GrandParentNode_In) Then
                    pGrandChildNode = pChildNode.Nodes(j)
                    Exit For
                End If
            Next

            '....PN
            Dim pGrandGrandChildNode As TreeNode = Nothing
            For k As Integer = 0 To pGrandChildNode.Nodes.Count - 1
                If (pGrandChildNode.Nodes(k).Text = ParentNode_In) Then
                    pGrandGrandChildNode = pGrandChildNode.Nodes(k)
                    Exit For
                End If
            Next

            '....Rev
            Dim pGGrandGrandChildNode As TreeNode = Nothing
            For l As Integer = 0 To pGrandGrandChildNode.Nodes.Count - 1
                If (pGrandGrandChildNode.Nodes(l).Text = SelectedNode_In) Then
                    pGGrandGrandChildNode = pGrandGrandChildNode.Nodes(l)
                    Exit For
                End If
            Next

            trvProjects.SelectedNode = pGGrandGrandChildNode
            trvProjects.SelectedNode.BackColor = SystemColors.HighlightText
            trvProjects.Select()
        End If


    End Sub



    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles tsbAdd.Click
        '=========================================================================================
        If (mProjectView) Then

            If (trvProjects.Nodes.Count > 0) Then

                If (trvProjects.SelectedNode.Level = 0) Then
                    txtCustomer.Text = ""
                    txtPlatform.Text = ""
                    txtLocation.Text = ""
                    txtCustomerPN.Text = ""
                    txtParkerPN_Part3.Text = ""
                    cmbParkerPN_Part2.SelectedIndex = -1
                    txtPN_PH_Rev.Text = ""
                    'AES 23MAR17
                    txtParkerPN_Legacy.Text = ""
                    txtParkerPNLegacy_Rev.Text = ""

                    txtCustomer.ReadOnly = False
                    txtCustomer.BackColor = Color.White
                    txtCustomer.ForeColor = Color.Black
                    txtCustomer.Focus()

                    cmbUnit.Enabled = True

                    txtPlatform.ReadOnly = False
                    txtPlatform.BackColor = Color.White
                    txtPlatform.ForeColor = Color.Black

                    txtLocation.ReadOnly = False
                    txtLocation.BackColor = Color.White
                    txtLocation.ForeColor = Color.Black

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                    txtParkerPN_Part1.ReadOnly = True
                    txtParkerPN_Part1.BackColor = Color.White
                    txtParkerPN_Part1.ForeColor = Color.Black
                    cmbParkerPN_Part2.Enabled = True
                    cmbParkerPN_Part2.SelectedIndex = 0

                    txtParkerPN_Part3.ReadOnly = False
                    txtParkerPN_Part3.BackColor = Color.White
                    txtParkerPN_Part3.ForeColor = Color.Black

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black


                    chkNew.Enabled = True
                    chkLegacy.Enabled = True
                    chkNew.Checked = True
                    cmdLegacy.Enabled = True


                    txtParkerPN_Legacy.ReadOnly = False
                    txtParkerPN_Legacy.BackColor = Color.White
                    txtParkerPN_Legacy.ForeColor = Color.Black

                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    'txtPNNew_Parent.Enabled = True
                    'txtPNNew_Parent.BackColor = Color.White
                    'txtPNNew_Parent.ForeColor = Color.Black

                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black

                ElseIf (trvProjects.SelectedNode.Level = 1) Then
                    cmbUnit.Enabled = False

                    txtPlatform.Text = ""
                    txtLocation.Text = ""
                    txtCustomerPN.Text = ""
                    cmbParkerPN_Part2.SelectedIndex = -1
                    txtParkerPN_Part3.Text = ""
                    txtPN_PH_Rev.Text = ""
                    'AES 23MAR17
                    txtParkerPN_Legacy.Text = ""
                    txtParkerPNLegacy_Rev.Text = ""

                    txtPlatform.ReadOnly = False
                    txtPlatform.BackColor = Color.White
                    txtPlatform.ForeColor = Color.Black
                    txtPlatform.Focus()

                    txtLocation.ReadOnly = False
                    txtLocation.BackColor = Color.White
                    txtLocation.ForeColor = Color.Black

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                    txtParkerPN_Part1.ReadOnly = True
                    txtParkerPN_Part1.BackColor = Color.White
                    txtParkerPN_Part1.ForeColor = Color.Black

                    cmbParkerPN_Part2.Enabled = True
                    cmbParkerPN_Part2.SelectedIndex = 0

                    txtParkerPN_Part3.ReadOnly = False
                    txtParkerPN_Part3.BackColor = Color.White
                    txtParkerPN_Part3.ForeColor = Color.Black

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black

                    'AES 23MAR17
                    chkNew.Enabled = True
                    chkLegacy.Enabled = True
                    chkNew.Checked = True
                    cmdLegacy.Enabled = True

                    txtParkerPN_Legacy.ReadOnly = False
                    txtParkerPN_Legacy.BackColor = Color.White
                    txtParkerPN_Legacy.ForeColor = Color.Black

                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    'txtPNNew_Parent.Enabled = True
                    'txtPNNew_Parent.BackColor = Color.White
                    'txtPNNew_Parent.ForeColor = Color.Black

                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black


                ElseIf (trvProjects.SelectedNode.Level = 2) Then
                    cmbUnit.Enabled = False
                    txtLocation.Text = ""
                    txtCustomerPN.Text = ""
                    cmbParkerPN_Part2.SelectedIndex = -1
                    txtParkerPN_Part3.Text = ""
                    txtPN_PH_Rev.Text = ""
                    'AES 23MAR17
                    txtParkerPN_Legacy.Text = ""
                    txtParkerPNLegacy_Rev.Text = ""

                    txtLocation.ReadOnly = False
                    txtLocation.BackColor = Color.White
                    txtLocation.ForeColor = Color.Black
                    txtLocation.Focus()

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                    txtParkerPN_Part1.ReadOnly = True
                    txtParkerPN_Part1.BackColor = Color.White
                    txtParkerPN_Part1.ForeColor = Color.Black

                    cmbParkerPN_Part2.Enabled = True
                    cmbParkerPN_Part2.SelectedIndex = 0

                    txtParkerPN_Part3.ReadOnly = False
                    txtParkerPN_Part3.BackColor = Color.White
                    txtParkerPN_Part3.ForeColor = Color.Black

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black

                    'AES 23MAR17
                    chkNew.Enabled = True
                    chkLegacy.Enabled = True
                    chkNew.Checked = True
                    cmdLegacy.Enabled = True

                    txtParkerPN_Legacy.ReadOnly = False
                    txtParkerPN_Legacy.BackColor = Color.White
                    txtParkerPN_Legacy.ForeColor = Color.Black

                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    'txtPNNew_Parent.Enabled = True
                    'txtPNNew_Parent.BackColor = Color.White
                    'txtPNNew_Parent.ForeColor = Color.Black

                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black

                ElseIf (trvProjects.SelectedNode.Level = 3) Then
                    cmbUnit.Enabled = False
                    txtCustomerPN.Text = ""
                    cmbParkerPN_Part2.SelectedIndex = -1
                    txtParkerPN_Part3.Text = ""
                    txtPN_PH_Rev.Text = ""
                    'AES 23MAR17
                    txtParkerPN_Legacy.Text = ""
                    txtParkerPNLegacy_Rev.Text = ""

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                    txtParkerPN_Part1.ReadOnly = True
                    txtParkerPN_Part1.BackColor = Color.White
                    txtParkerPN_Part1.ForeColor = Color.Black

                    cmbParkerPN_Part2.Enabled = True
                    cmbParkerPN_Part2.SelectedIndex = 0

                    txtParkerPN_Part3.ReadOnly = False
                    txtParkerPN_Part3.BackColor = Color.White
                    txtParkerPN_Part3.ForeColor = Color.Black
                    txtParkerPN_Part3.Focus()

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black

                    chkNew.Enabled = True
                    chkLegacy.Enabled = True
                    chkNew.Checked = True
                    cmdLegacy.Enabled = True


                    txtParkerPN_Legacy.ReadOnly = False
                    txtParkerPN_Legacy.BackColor = Color.White
                    txtParkerPN_Legacy.ForeColor = Color.Black

                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    'txtPNNew_Parent.Enabled = True
                    'txtPNNew_Parent.BackColor = Color.White
                    'txtPNNew_Parent.ForeColor = Color.Black

                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black

                ElseIf (trvProjects.SelectedNode.Level = 4) Then
                    cmbUnit.Enabled = False
                    txtPN_PH_Rev.Text = ""
                    'AES 23MAR17
                    txtParkerPNLegacy_Rev.Text = ""

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black
                    txtPN_PH_Rev.Focus()

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black


                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    'txtPNNew_Parent.Enabled = True
                    'txtPNNew_Parent.BackColor = Color.White
                    'txtPNNew_Parent.ForeColor = Color.Black

                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black

                End If

            Else

                txtCustomer.Text = ""
                txtPlatform.Text = ""
                txtLocation.Text = ""
                txtCustomerPN.Text = ""
                txtParkerPN_Part3.Text = ""
                cmbParkerPN_Part2.SelectedIndex = -1
                txtPN_PH_Rev.Text = ""
                'AES 23MAR17
                txtParkerPN_Legacy.Text = ""
                txtParkerPNLegacy_Rev.Text = ""


                txtCustomer.ReadOnly = False
                txtCustomer.BackColor = Color.White
                txtCustomer.ForeColor = Color.Black
                txtCustomer.Focus()

                cmbUnit.Enabled = True

                txtPlatform.ReadOnly = False
                txtPlatform.BackColor = Color.White
                txtPlatform.ForeColor = Color.Black

                txtLocation.ReadOnly = False
                txtLocation.BackColor = Color.White
                txtLocation.ForeColor = Color.Black

                txtCustomerPN.ReadOnly = False
                txtCustomerPN.BackColor = Color.White
                txtCustomerPN.ForeColor = Color.Black

                txtParkerPN_Part1.ReadOnly = True
                txtParkerPN_Part1.BackColor = Color.White
                txtParkerPN_Part1.ForeColor = Color.Black
                cmbParkerPN_Part2.Enabled = True
                cmbParkerPN_Part2.SelectedIndex = 0

                txtParkerPN_Part3.ReadOnly = False
                txtParkerPN_Part3.BackColor = Color.White
                txtParkerPN_Part3.ForeColor = Color.Black

                txtPN_PH_Rev.ReadOnly = False
                txtPN_PH_Rev.BackColor = Color.White
                txtPN_PH_Rev.ForeColor = Color.Black

                chkNew.Enabled = True
                chkLegacy.Enabled = True
                chkNew.Checked = True
                cmdLegacy.Enabled = True


                txtParkerPN_Legacy.ReadOnly = False
                txtParkerPN_Legacy.BackColor = Color.White
                txtParkerPN_Legacy.ForeColor = Color.Black

                txtParkerPNLegacy_Rev.ReadOnly = False
                txtParkerPNLegacy_Rev.BackColor = Color.White
                txtParkerPNLegacy_Rev.ForeColor = Color.Black

                'txtPNNew_Parent.Enabled = True
                'txtPNNew_Parent.BackColor = Color.White
                'txtPNNew_Parent.ForeColor = Color.Black

                txtParentCur_Part1.ReadOnly = True
                txtParentCur_Part1.BackColor = Color.White
                txtParentCur_Part1.ForeColor = Color.Black
                cmbParentCur_Part2.Enabled = True
                cmbParentCur_Part2.SelectedIndex = 0

                txtParentCur_Part3.ReadOnly = False
                txtParentCur_Part3.BackColor = Color.White
                txtParentCur_Part3.ForeColor = Color.Black

                txtParentCur_Rev.Enabled = True
                txtParentCur_Rev.BackColor = Color.White
                txtParentCur_Rev.ForeColor = Color.Black

            End If

        ElseIf (mPNView) Then

            If (trvProjects.Nodes.Count > 0) Then
                gPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.None

                If (trvProjects.SelectedNode.Level = 1) Then
                    txtCustomer.Text = ""
                    txtPlatform.Text = ""
                    txtLocation.Text = ""
                    txtCustomerPN.Text = ""
                    txtCustomerPN_Rev.Text = ""
                    txtParkerPN_Part3.Text = ""
                    cmbParkerPN_Part2.SelectedIndex = -1
                    txtPN_PH_Rev.Text = ""

                    chkLegacy.Checked = False
                    txtParkerPN_Legacy.Text = ""
                    txtParkerPNLegacy_Rev.Text = ""

                    chkPNNew_Parent.Checked = False
                    cmbParentCur_Part2.SelectedIndex = -1
                    txtParentCur_Part3.Text = ""
                    txtParentCur_Rev.Text = ""

                    chkPNLegacy_Parent.Checked = False
                    txtPNLegacy_Parent.Text = ""
                    txtPNParentLegacy_Rev.Text = ""


                    chkRefDimNew_Exists.Checked = False
                    cmbRefPNNewDim_Part2.SelectedIndex = -1
                    txtRefPNNewDim_Part3.Text = ""
                    txtRefPNNewDim_Rev.Text = ""

                    chkRefDimLegacy_Exists.Checked = False
                    txtRefPNNewDim_Legacy.Text = ""
                    txtRefPNLegacyDim_Rev.Text = ""

                    chkRefDimNotes_Exists.Checked = False
                    cmbRefNotesNewPN_Part2.SelectedIndex = -1
                    txtRefPNNotes_Part3.Text = ""
                    txtRefPNNewNotes_Rev.Text = ""

                    chkRefNotesLegacy_Exists.Checked = False
                    txtRefPNNewNotes_Legacy.Text = ""
                    txtRefPNLegacyNotes_Rev.Text = ""

                    txtCustomer.ReadOnly = False
                    txtCustomer.BackColor = Color.White
                    txtCustomer.ForeColor = Color.Black
                    'txtCustomer.Focus()

                    cmbUnit.Enabled = True
                    'cmbCulturalFormat.Enabled = True

                    txtPlatform.ReadOnly = False
                    txtPlatform.BackColor = Color.White
                    txtPlatform.ForeColor = Color.Black

                    txtLocation.ReadOnly = False
                    txtLocation.BackColor = Color.White
                    txtLocation.ForeColor = Color.Black

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                    txtCustomerPN_Rev.ReadOnly = False
                    txtCustomerPN_Rev.BackColor = Color.White
                    txtCustomerPN_Rev.ForeColor = Color.Black

                    If (chkLegacy.Checked) Then

                        chkNew.Enabled = True
                        chkNew.Checked = False
                        chkLegacy.Enabled = True
                        cmdLegacy.Enabled = True
                        txtParkerPNLegacy_Rev.Enabled = True
                        cmbParkerPN_Part2.Enabled = False
                        txtParkerPN_Part3.Enabled = False
                        txtPN_PH_Rev.Enabled = False
                    Else
                        chkNew.Enabled = True
                        chkNew.Checked = True
                        chkLegacy.Enabled = True
                        cmdLegacy.Enabled = False
                        txtParkerPNLegacy_Rev.Enabled = False
                        cmbParkerPN_Part2.Enabled = True
                        txtParkerPN_Part3.Enabled = True
                        txtPN_PH_Rev.Enabled = True
                    End If

                    txtParkerPN_Part1.ReadOnly = True
                    txtParkerPN_Part1.BackColor = Color.White
                    txtParkerPN_Part1.ForeColor = Color.Black
                    cmbParkerPN_Part2.Enabled = True
                    cmbParkerPN_Part2.SelectedIndex = 0

                    txtParkerPN_Part3.ReadOnly = False
                    txtParkerPN_Part3.BackColor = Color.White
                    txtParkerPN_Part3.ForeColor = Color.Black
                    txtParkerPN_Part3.Focus()

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black

                    'chkNew.Enabled = True
                    'chkLegacy.Enabled = True
                    'chkNew.Checked = True
                    'cmdLegacy.Enabled = True

                    txtParkerPN_Legacy.ReadOnly = False
                    txtParkerPN_Legacy.BackColor = Color.White
                    txtParkerPN_Legacy.ForeColor = Color.Black

                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    chkPNNew_Parent.Enabled = True
                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Text = ""
                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black


                    chkPNLegacy_Parent.Enabled = True
                    txtPNLegacy_Parent.Text = ""
                    txtPNLegacy_Parent.Enabled = True
                    txtPNLegacy_Parent.BackColor = Color.White
                    txtPNLegacy_Parent.ForeColor = Color.Black

                    txtPNParentLegacy_Rev.Text = ""
                    txtPNParentLegacy_Rev.Enabled = True
                    txtPNParentLegacy_Rev.BackColor = Color.White
                    txtPNParentLegacy_Rev.ForeColor = Color.Black


                    chkRefDimNew_Exists.Enabled = True
                    cmbRefPNNewDim_Part2.Enabled = True
                    txtRefPNNewDim_Part3.Enabled = True
                    txtRefPNNewDim_Rev.Enabled = True

                    chkRefDimNotes_Exists.Enabled = True
                    cmbRefNotesNewPN_Part2.Enabled = True
                    txtRefPNNotes_Part3.Enabled = True
                    txtRefPNNewNotes_Rev.Enabled = True

                    chkRefDimLegacy_Exists.Enabled = True
                    txtRefPNNewDim_Legacy.Enabled = True
                    txtRefPNLegacyDim_Rev.Enabled = True

                    chkRefNotesLegacy_Exists.Enabled = True
                    txtRefPNNewNotes_Legacy.Enabled = True
                    txtRefPNLegacyNotes_Rev.Enabled = True

                ElseIf (trvProjects.SelectedNode.Level = 2) Then

                    If (txtPN_PH_Rev.Text <> "") Then
                        txtPN_PH_Rev.Text = ""
                        txtPN_PH_Rev.Focus()

                    ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                        txtParkerPNLegacy_Rev.Text = ""
                        txtParkerPNLegacy_Rev.Focus()
                    End If
                    txtPN_PH_Rev.Enabled = True
                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black


                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    'grpIPEDesign.Enabled = False    ''True  'AES 26APR17

                End If
            Else

                txtCustomer.Text = ""
                txtPlatform.Text = ""
                txtLocation.Text = ""
                txtCustomerPN.Text = ""
                txtCustomerPN_Rev.Text = ""
                txtParkerPN_Part3.Text = ""
                cmbParkerPN_Part2.SelectedIndex = -1
                txtPN_PH_Rev.Text = ""

                chkLegacy.Checked = False
                txtParkerPN_Legacy.Text = ""
                txtParkerPNLegacy_Rev.Text = ""

                chkPNNew_Parent.Checked = False
                cmbParentCur_Part2.SelectedIndex = -1
                txtParentCur_Part3.Text = ""
                txtParentCur_Rev.Text = ""

                chkPNLegacy_Parent.Checked = False
                txtPNLegacy_Parent.Text = ""
                txtPNParentLegacy_Rev.Text = ""

                chkRefDimNew_Exists.Checked = False
                cmbRefPNNewDim_Part2.SelectedIndex = -1
                txtRefPNNewDim_Part3.Text = ""
                txtRefPNNewDim_Rev.Text = ""

                chkRefDimLegacy_Exists.Checked = False
                txtRefPNNewDim_Legacy.Text = ""
                txtRefPNLegacyDim_Rev.Text = ""

                chkRefDimNotes_Exists.Checked = False
                cmbRefNotesNewPN_Part2.SelectedIndex = -1
                txtRefPNNotes_Part3.Text = ""
                txtRefPNNewNotes_Rev.Text = ""

                chkRefNotesLegacy_Exists.Checked = False
                txtRefPNNewNotes_Legacy.Text = ""
                txtRefPNLegacyNotes_Rev.Text = ""

                txtCustomer.ReadOnly = False
                txtCustomer.BackColor = Color.White
                txtCustomer.ForeColor = Color.Black
                'txtCustomer.Focus()

                cmbUnit.Enabled = True
                'cmbCulturalFormat.Enabled = True

                txtPlatform.ReadOnly = False
                txtPlatform.BackColor = Color.White
                txtPlatform.ForeColor = Color.Black

                txtLocation.ReadOnly = False
                txtLocation.BackColor = Color.White
                txtLocation.ForeColor = Color.Black

                txtCustomerPN.ReadOnly = False
                txtCustomerPN.BackColor = Color.White
                txtCustomerPN.ForeColor = Color.Black

                txtCustomerPN_Rev.ReadOnly = False
                txtCustomerPN_Rev.BackColor = Color.White
                txtCustomerPN_Rev.ForeColor = Color.Black

                txtParkerPN_Part1.ReadOnly = True
                txtParkerPN_Part1.BackColor = Color.White
                txtParkerPN_Part1.ForeColor = Color.Black
                cmbParkerPN_Part2.Enabled = True
                cmbParkerPN_Part2.SelectedIndex = 0

                txtParkerPN_Part3.ReadOnly = False
                txtParkerPN_Part3.BackColor = Color.White
                txtParkerPN_Part3.ForeColor = Color.Black
                txtParkerPN_Part3.Focus()

                txtPN_PH_Rev.ReadOnly = False
                txtPN_PH_Rev.BackColor = Color.White
                txtPN_PH_Rev.ForeColor = Color.Black

                'AES 23MAR17
                chkNew.Enabled = True
                chkLegacy.Enabled = True
                chkNew.Checked = True
                cmdLegacy.Enabled = True        'AES 02MAY17

                txtParkerPN_Legacy.ReadOnly = False
                txtParkerPN_Legacy.BackColor = Color.White
                txtParkerPN_Legacy.ForeColor = Color.Black

                txtParkerPNLegacy_Rev.ReadOnly = False
                txtParkerPNLegacy_Rev.BackColor = Color.White
                txtParkerPNLegacy_Rev.ForeColor = Color.Black

                chkPNNew_Parent.Enabled = True
                txtParentCur_Part1.ReadOnly = True
                txtParentCur_Part1.BackColor = Color.White
                txtParentCur_Part1.ForeColor = Color.Black
                cmbParentCur_Part2.Enabled = True
                cmbParentCur_Part2.SelectedIndex = 0

                txtParentCur_Part3.ReadOnly = False
                txtParentCur_Part3.BackColor = Color.White
                txtParentCur_Part3.ForeColor = Color.Black

                txtParentCur_Rev.Text = ""
                txtParentCur_Rev.Enabled = True
                txtParentCur_Rev.BackColor = Color.White
                txtParentCur_Rev.ForeColor = Color.Black

                chkPNLegacy_Parent.Enabled = True
                txtPNLegacy_Parent.Text = ""        'AES 16OCT17
                txtPNLegacy_Parent.Enabled = True
                txtPNLegacy_Parent.BackColor = Color.White
                txtPNLegacy_Parent.ForeColor = Color.Black

                txtPNParentLegacy_Rev.Text = ""
                txtPNParentLegacy_Rev.Enabled = True
                txtPNParentLegacy_Rev.BackColor = Color.White
                txtPNParentLegacy_Rev.ForeColor = Color.Black

                'grpIPEDesign.Enabled = False    ''True  AES 26APR17
                chkRefDimNew_Exists.Enabled = True
                cmbRefPNNewDim_Part2.Enabled = True

                txtRefPNNewDim_Part3.Enabled = True
                txtRefPNNewDim_Part3.Text = ""

                txtRefPNNewDim_Rev.Enabled = True
                txtRefPNNewDim_Rev.Text = ""

                chkRefDimNotes_Exists.Enabled = True
                cmbRefNotesNewPN_Part2.Enabled = True

                txtRefPNNotes_Part3.Enabled = True
                txtRefPNNotes_Part3.Text = ""

                txtRefPNNewNotes_Rev.Enabled = True
                txtRefPNNewNotes_Rev.Text = ""

                chkRefDimLegacy_Exists.Enabled = True

                txtRefPNNewDim_Legacy.Enabled = True
                txtRefPNNewDim_Legacy.Text = ""

                txtRefPNLegacyDim_Rev.Enabled = True
                txtRefPNLegacyDim_Rev.Text = ""

                chkRefNotesLegacy_Exists.Enabled = True

                txtRefPNNewNotes_Legacy.Enabled = True
                txtRefPNNewNotes_Legacy.Text = ""

                txtRefPNLegacyNotes_Rev.Enabled = True
                txtRefPNLegacyNotes_Rev.Text = ""
            End If

        End If

        mblnAdd = True
        mblnEdit = False

        tsbAdd.Enabled = False
        tsbEdit.Enabled = False
        tsbSave.Enabled = True
        tsbDelete.Enabled = False

    End Sub


    Private Sub cmdEdit_Click(sender As System.Object, e As System.EventArgs) _
                              Handles tsbEdit.Click
        '=======================================================================

        If (trvProjects.Nodes.Count > 0) Then
            If (mProjectView) Then

                If (trvProjects.SelectedNode.Level = 0) Then
                    txtCustomer.ReadOnly = False
                    txtCustomer.BackColor = Color.White
                    txtCustomer.ForeColor = Color.Black
                    txtCustomer.Focus()

                    cmbUnit.Enabled = True

                ElseIf (trvProjects.SelectedNode.Level = 1) Then
                    cmbUnit.Enabled = False

                    txtPlatform.ReadOnly = False
                    txtPlatform.BackColor = Color.White
                    txtPlatform.ForeColor = Color.Black
                    txtPlatform.Focus()

                ElseIf (trvProjects.SelectedNode.Level = 2) Then
                    cmbUnit.Enabled = False

                    txtLocation.ReadOnly = False
                    txtLocation.BackColor = Color.White
                    txtLocation.ForeColor = Color.Black
                    txtLocation.Focus()

                ElseIf (trvProjects.SelectedNode.Level = 3) Then
                    cmbUnit.Enabled = False

                    If (trvProjects.SelectedNode.Text.Contains("NH")) Then
                        cmbParkerPN_Part2.Enabled = True
                        txtParkerPN_Part3.Focus()

                        txtParkerPN_Part3.ReadOnly = False
                        txtParkerPN_Part3.BackColor = Color.White
                        txtParkerPN_Part3.ForeColor = Color.Black

                        chkNew.Enabled = True
                    Else
                        cmbParkerPN_Part2.Enabled = False
                        chkNew.Enabled = False
                        chkLegacy.Enabled = True

                        txtParkerPN_Legacy.Enabled = True
                        txtParkerPN_Legacy.ReadOnly = False
                        txtParkerPN_Legacy.BackColor = Color.White
                        txtParkerPN_Legacy.ForeColor = Color.Black

                        txtParkerPNLegacy_Rev.Enabled = True
                        txtParkerPNLegacy_Rev.ReadOnly = False
                        txtParkerPNLegacy_Rev.BackColor = Color.White
                        txtParkerPNLegacy_Rev.ForeColor = Color.Black
                    End If


                    'txtParkerPN_Part3.ReadOnly = False
                    'txtParkerPN_Part3.BackColor = Color.White
                    'txtParkerPN_Part3.ForeColor = Color.Black



                    'AES 23MAR17
                    'chkNew.Enabled = True
                    'chkLegacy.Enabled = True
                    'cmdLegacy.Enabled = True

                    'txtParkerPN_Legacy.ReadOnly = False
                    'txtParkerPN_Legacy.BackColor = Color.White
                    'txtParkerPN_Legacy.ForeColor = Color.Black

                    'txtPNNew_Parent.Enabled = True
                    'txtPNNew_Parent.BackColor = Color.White
                    'txtPNNew_Parent.ForeColor = Color.Black

                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    'txtParkerPNParent_Rev.Enabled = True
                    'txtParkerPNParent_Rev.BackColor = Color.White
                    'txtParkerPNParent_Rev.ForeColor = Color.Black


                ElseIf (trvProjects.SelectedNode.Level = 4) Then
                    cmbUnit.Enabled = False

                    If (chkLegacy.Checked) Then
                        txtParkerPNLegacy_Rev.Enabled = True
                        txtParkerPNLegacy_Rev.ReadOnly = False
                        txtParkerPNLegacy_Rev.BackColor = Color.White
                        txtParkerPNLegacy_Rev.ForeColor = Color.Black
                        txtParkerPNLegacy_Rev.Focus()
                        txtPN_PH_Rev.ReadOnly = True
                    Else

                        txtPN_PH_Rev.Enabled = True
                        txtPN_PH_Rev.ReadOnly = False
                        txtPN_PH_Rev.BackColor = Color.White
                        txtPN_PH_Rev.ForeColor = Color.Black
                        txtPN_PH_Rev.Focus()
                        txtParkerPNLegacy_Rev.ReadOnly = True
                    End If

                    'txtPN_PH_Rev.ReadOnly = False
                    'txtPN_PH_Rev.BackColor = Color.White
                    'txtPN_PH_Rev.ForeColor = Color.Black

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                End If

            ElseIf (mPNView) Then

                If (trvProjects.SelectedNode.Level = 1) Then
                    cmbUnit.Enabled = False
                    'cmbCulturalFormat.Enabled = False

                    'AES 13OCT17
                    'chkNew.Enabled = True
                    'chkLegacy.Enabled = True

                    If (chkLegacy.Checked) Then
                        'AES 13OCT17
                        chkLegacy.Enabled = True
                        chkNew.Enabled = False

                        cmdLegacy.Enabled = True
                        txtParkerPNLegacy_Rev.Enabled = True
                        cmbParkerPN_Part2.Enabled = False
                        txtParkerPN_Part3.Enabled = False
                        'txtPN_PH_Rev.Enabled = False

                        txtParkerPNLegacy_Rev.Enabled = True
                        txtPN_PH_Rev.Enabled = False

                        txtParkerPNLegacy_Rev.ReadOnly = False
                        txtParkerPNLegacy_Rev.BackColor = Color.White
                        txtParkerPNLegacy_Rev.ForeColor = Color.Black

                    Else
                        'AES 13OCT17
                        chkLegacy.Enabled = False
                        chkNew.Enabled = True

                        cmdLegacy.Enabled = False
                        txtParkerPNLegacy_Rev.Enabled = False
                        cmbParkerPN_Part2.Enabled = True
                        txtParkerPN_Part3.Enabled = True
                        'txtPN_PH_Rev.Enabled = True

                        txtParkerPNLegacy_Rev.Enabled = False
                        txtPN_PH_Rev.Enabled = True

                        txtPN_PH_Rev.ReadOnly = False
                        txtPN_PH_Rev.BackColor = Color.White
                        txtPN_PH_Rev.ForeColor = Color.Black

                    End If

                    txtParkerPN_Part3.ReadOnly = False
                    txtParkerPN_Part3.BackColor = Color.White
                    txtParkerPN_Part3.ForeColor = Color.Black

                    cmbParkerPN_Part2.Enabled = True

                    '....Parent New
                    chkPNNew_Parent.Enabled = True
                    txtParentCur_Part1.ReadOnly = True
                    txtParentCur_Part1.BackColor = Color.White
                    txtParentCur_Part1.ForeColor = Color.Black
                    cmbParentCur_Part2.Enabled = True
                    cmbParentCur_Part2.SelectedIndex = 0

                    txtParentCur_Part3.ReadOnly = False
                    txtParentCur_Part3.BackColor = Color.White
                    txtParentCur_Part3.ForeColor = Color.Black

                    txtParentCur_Rev.Enabled = True
                    txtParentCur_Rev.ReadOnly = False
                    txtParentCur_Rev.BackColor = Color.White
                    txtParentCur_Rev.ForeColor = Color.Black

                    '....Parent Legacy
                    chkPNLegacy_Parent.Enabled = True
                    txtPNLegacy_Parent.Enabled = True
                    txtPNLegacy_Parent.ReadOnly = False
                    txtPNLegacy_Parent.BackColor = Color.White
                    txtPNLegacy_Parent.ForeColor = Color.Black

                    txtPNParentLegacy_Rev.Enabled = True
                    txtPNParentLegacy_Rev.ReadOnly = False
                    txtPNParentLegacy_Rev.BackColor = Color.White
                    txtPNParentLegacy_Rev.ForeColor = Color.Black

                    chkRefDimNew_Exists.Enabled = True
                    cmbRefPNNewDim_Part2.Enabled = True
                    txtRefPNNewDim_Part3.Enabled = True
                    txtRefPNNewDim_Rev.Enabled = True

                    chkRefDimNotes_Exists.Enabled = True
                    cmbRefNotesNewPN_Part2.Enabled = True
                    txtRefPNNotes_Part3.Enabled = True
                    txtRefPNNewNotes_Rev.Enabled = True
                    chkRefDimLegacy_Exists.Enabled = True
                    txtRefPNNewDim_Legacy.Enabled = True
                    txtRefPNLegacyDim_Rev.Enabled = True
                    chkRefNotesLegacy_Exists.Enabled = True
                    txtRefPNNewNotes_Legacy.Enabled = True
                    txtRefPNLegacyNotes_Rev.Enabled = True

                    txtParkerPN_Part3.Focus()


                ElseIf (trvProjects.SelectedNode.Level = 2) Then
                    cmbUnit.Enabled = True
                    'cmbCulturalFormat.Enabled = True

                    txtCustomer.ReadOnly = False
                    txtCustomer.BackColor = Color.White
                    txtCustomer.ForeColor = Color.Black

                    txtPlatform.ReadOnly = False
                    txtPlatform.BackColor = Color.White
                    txtPlatform.ForeColor = Color.Black

                    txtLocation.ReadOnly = False
                    txtLocation.BackColor = Color.White
                    txtLocation.ForeColor = Color.Black

                    txtCustomerPN.ReadOnly = False
                    txtCustomerPN.BackColor = Color.White
                    txtCustomerPN.ForeColor = Color.Black

                    txtCustomerPN_Rev.ReadOnly = False
                    txtCustomerPN_Rev.BackColor = Color.White
                    txtCustomerPN_Rev.ForeColor = Color.Black

                    If (chkLegacy.Checked) Then
                        txtParkerPNLegacy_Rev.Enabled = True
                        txtPN_PH_Rev.Enabled = False
                    Else
                        txtParkerPNLegacy_Rev.Enabled = False
                        txtPN_PH_Rev.Enabled = True
                    End If

                    txtPN_PH_Rev.ReadOnly = False
                    txtPN_PH_Rev.BackColor = Color.White
                    txtPN_PH_Rev.ForeColor = Color.Black
                    txtPN_PH_Rev.Focus()

                    txtParkerPNLegacy_Rev.ReadOnly = False
                    txtParkerPNLegacy_Rev.BackColor = Color.White
                    txtParkerPNLegacy_Rev.ForeColor = Color.Black

                End If

            End If

            mblnEdit = True
            mblnAdd = False

            tsbAdd.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbDelete.Enabled = False

        End If

    End Sub


    Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) _
                              Handles tsbSave.Click
        '======================================================================
        SaveData()      'AES 18APR18
        If (mblnAdd) Then
            AddRecords(mCustomerID, mPlatformID, mLocationID, mPNID, mRevID)

        ElseIf (mblnEdit) Then
            UpdateRecords(mCustomerID, mPlatformID, mLocationID,
                      mPNID, mRevID)

        End If

        tsbAdd.Enabled = True
        tsbEdit.Enabled = True
        tsbSave.Enabled = False
        tsbDelete.Enabled = True

    End Sub


    Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsbDelete.Click
        '===============================================================================================
        Dim pintAnswer As Integer
        pintAnswer = MessageBox.Show("Are you sure you want to permanently delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If pintAnswer = Windows.Forms.DialogResult.Yes Then
            SaveData()
            DeleteRecords(mCustomerID, mPlatformID, mLocationID,
                          mPNID, mRevID)

            mPNID = 0
            mRevID = 0
            DisplayData()
            MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK)

        End If

    End Sub


    Private Sub cmdTestModule_Click(sender As System.Object, e As System.EventArgs)

        '=============================================================================
        'SaveData()

        'gTest_User.RetrieveUserRoles()


        'Dim pRoleCount As Integer = 0

        'If (gTest_User.Admin) Then
        '    pRoleCount = pRoleCount + 1
        'End If

        'If (gTest_User.Tester) Then
        '    pRoleCount = pRoleCount + 1
        'End If

        'If (gTest_User.Engg) Then
        '    pRoleCount = pRoleCount + 1
        'End If

        'If (gTest_User.Quality) Then
        '    pRoleCount = pRoleCount + 1
        'End If

        'If (pRoleCount > 1) Then

        '    Dim pfrmTestUserRole As New frmTest_User_Role()
        '    pfrmTestUserRole.ShowDialog()

        'Else
        '    gTest_User.Role = Test_clsUser.eRole.Viewer
        '    'Dim pfrmTestMain As New frmTest_Main()
        '    gfrmTestMain.ShowDialog()
        'End If

    End Sub


    'Private Function GetUserName() As String
    '    If TypeOf My.User.CurrentPrincipal Is 
    '      Security.Principal.WindowsPrincipal Then
    '        ' The application is using Windows authentication.
    '        ' The name format is DOMAIN\USERNAME.
    '        Dim parts() As String = Split(My.User.Name, "\")
    '        Dim username As String = parts(1)
    '        Return username
    '    Else
    '        ' The application is using custom authentication.
    '        Return My.User.Name
    '    End If
    'End Function


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        'SetDefaultData()

        If (mProjectView) Then
            With mPartProject

                If (mCustomerID = 0) Then
                    MessageBox.Show("Customer can't be blank", "Customer Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtCustomer.Focus()
                    Exit Sub
                Else
                    If (mPlatformID = 0) Then
                        MessageBox.Show("Platform can't be blank", "Platform Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtPlatform.Focus()
                        Exit Sub
                    Else
                        If (mLocationID = 0) Then
                            MessageBox.Show("Location can't be blank", "Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            txtLocation.Focus()
                            Exit Sub
                        Else
                            If (mPNID = 0) Then
                                MessageBox.Show("P/N can't be blank", "P/N Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtParkerPN_Part3.Focus()
                                Exit Sub
                            Else
                                If (mRevID = 0) Then
                                    'MessageBox.Show("Select a P/N Rev", "P/N Rev Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    'txtPN_PH_Rev.Focus()
                                    mRevID = 1
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If
            End With
        End If

        gFile.SaveIniFile(gUser, gIPE_Project, gIPE_ANSYS, gIPE_Unit)
        SaveData()

        'AES 25APR18
        If (mblnAdd) Then
            AddRecords(mCustomerID, mPlatformID, mLocationID, mPNID, mRevID)

            tsbAdd.Enabled = True
            tsbEdit.Enabled = True
            tsbSave.Enabled = False
            tsbDelete.Enabled = True

        ElseIf (mblnEdit) Then
            UpdateRecords(mCustomerID, mPlatformID, mLocationID,
                      mPNID, mRevID)

            tsbAdd.Enabled = True
            tsbEdit.Enabled = True
            tsbSave.Enabled = False
            tsbDelete.Enabled = True

        Else
            Environment.Exit(0)
        End If

        '....UPDATE MAIN FORM DISPLAY.
        ''gTest_frmMain.UpdateDisplay()
        ' ''modMain.gIPE_frmAnalysisSet.ShowDialog()

        ''If (gPartProject.Project_ID <> 0) Then
        ''    gIPE_frmAnalysisSet = New frmAnalysisSet()
        ''    modMain.gIPE_frmAnalysisSet.ShowDialog()
        ''End If

    End Sub


    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
        Environment.Exit(0)
    End Sub


#Region "HELPER ROUTINES:"

    'Private Function GetCurrentRole(ByRef CurrentRole_Out As List(Of String)) As String
    '    '===============================================================================
    '    Dim pRoleList As New List(Of String)
    '    Dim pSealTestEntities As New SealTestDBEntities()
    '    Dim pName As String = ""
    '    Dim pLoginName As String = SystemInformation.UserName

    '    Dim pRecCount As Integer = (From pRec In pSealTestEntities.tblTestUser
    '                                  Where pRec.fldSystemLogin = pLoginName Select pRec).Count()

    '    If (pRecCount > 0) Then

    '        Dim pAdmin As String = "Admin"
    '        Dim pTester As String = "Tester"
    '        Dim pEngg As String = "Engineer"
    '        Dim pQuality As String = "Quality"

    '        Dim pQry = (From pRec In pSealTestEntities.tblTestUser
    '                                   Where pRec.fldSystemLogin = pLoginName Select pRec).First()
    '        pName = pQry.fldName
    '        If (pQry.fldRoleAdmin) Then
    '            pRoleList.Add("Admin")
    '        End If
    '        If (pQry.fldRoleTester) Then
    '            pRoleList.Add("Tester")
    '        End If
    '        If (pQry.fldRoleEngg) Then
    '            pRoleList.Add("Engineer")
    '        End If
    '        If (pQry.fldRoleQuality) Then
    '            pRoleList.Add("Quality")
    '        End If
    '    End If
    '    CurrentRole_Out = pRoleList
    '    Return pName

    'End Function

    Private Sub SaveData()
        '=================

        With mPartProject

            gPartProject.CustInfo.CustName = .CustInfo.CustName
            gPartProject.CustInfo.PlatName = .CustInfo.PlatName
            gPartProject.CustInfo.LocName = .CustInfo.LocName
            gPartProject.CustInfo.PN_Cust = .CustInfo.PN_Cust
            gPartProject.CustInfo.PN_Cust_Rev = .CustInfo.PN_Cust_Rev
            gPartProject.Project_ID = .Project_ID
            gUnit.System = mPartProject.PNR.UnitSystem.ToString()
            gPartProject.PNR.UnitSystem = mPartProject.PNR.UnitSystem

            'gPartProject.CultureName = .CultureName

            If (chkNew.Checked) Then
                gPartProject.PNR.Current_Exists = True
                gPartProject.PNR.Current_TypeNo = cmbParkerPN_Part2.Text
                gPartProject.PNR.Current_Val = txtParkerPN_Part3.Text.Trim()
                gPartProject.PNR.Current_Rev = txtPN_PH_Rev.Text
            Else
                gPartProject.PNR.Current_Exists = False
                gPartProject.PNR.Current_Val = ""
                gPartProject.PNR.Current_Rev = ""
            End If

            If (chkLegacy.Checked) Then
                gPartProject.PNR.Legacy_Exists = True
                gPartProject.PNR.Legacy_Val = txtParkerPN_Legacy.Text
                gPartProject.PNR.Legacy_Type = mPartProject.PNR.Legacy.Type
                gPartProject.PNR.Legacy_Rev = txtParkerPNLegacy_Rev.Text
            Else
                gPartProject.PNR.Legacy_Exists = False
                gPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.None
                gPartProject.PNR.Legacy_Val = ""
                gPartProject.PNR.Legacy_Rev = ""
            End If

            If (chkPNNew_Parent.Checked) Then
                gPartProject.PNR.ParentCurrent_Exists = True
                gPartProject.PNR.ParentCurrent_TypeNo = cmbParentCur_Part2.Text
                gPartProject.PNR.ParentCurrent_Val = txtParentCur_Part3.Text.Trim()
                gPartProject.PNR.ParentCurrent_Rev = txtParentCur_Rev.Text
            Else
                gPartProject.PNR.ParentCurrent_Exists = False
                gPartProject.PNR.ParentCurrent_Val = ""
                gPartProject.PNR.ParentCurrent_Rev = ""
            End If

            If (chkPNLegacy_Parent.Checked) Then
                gPartProject.PNR.ParentLegacy_Exists = True
                gPartProject.PNR.ParentLegacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
                gPartProject.PNR.ParentLegacy_Val = txtPNLegacy_Parent.Text
                gPartProject.PNR.ParentLegacy_Rev = txtPNParentLegacy_Rev.Text
            Else
                gPartProject.PNR.ParentLegacy_Exists = False
                gPartProject.PNR.ParentLegacy_Type = clsPartProject.clsPNR.eLegacyType.None
                gPartProject.PNR.ParentLegacy_Val = ""
                gPartProject.PNR.ParentLegacy_Rev = ""
            End If

            If (chkRefDimNew_Exists.Checked) Then
                gPartProject.PNR.RefDimCurrent_Exists = True
                gPartProject.PNR.RefDimCurrent_TypeNo = cmbRefPNNewDim_Part2.Text
                gPartProject.PNR.RefDimCurrent_Val = txtRefPNNewDim_Part3.Text.Trim()
                gPartProject.PNR.RefDimCurrent_Rev = txtRefPNNewDim_Rev.Text
            Else
                gPartProject.PNR.RefDimCurrent_Exists = False
                gPartProject.PNR.RefDimCurrent_Val = ""
                gPartProject.PNR.RefDimCurrent_Rev = ""
            End If

            If (chkRefDimLegacy_Exists.Checked) Then
                gPartProject.PNR.RefDimLegacy_Exists = True
                gPartProject.PNR.RefDimLegacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
                gPartProject.PNR.RefDimLegacy_Val = txtRefPNNewDim_Legacy.Text
                gPartProject.PNR.RefDimLegacy_Rev = txtRefPNLegacyDim_Rev.Text
            Else
                gPartProject.PNR.RefDimLegacy_Exists = False
                gPartProject.PNR.RefDimLegacy_Type = clsPartProject.clsPNR.eLegacyType.None
                gPartProject.PNR.RefDimLegacy_Val = ""
                gPartProject.PNR.RefDimLegacy_Rev = ""
            End If

            If (chkRefDimNotes_Exists.Checked) Then
                gPartProject.PNR.RefNotesCurrent_Exists = True
                gPartProject.PNR.RefNotesCurrent_TypeNo = cmbRefNotesNewPN_Part2.Text
                gPartProject.PNR.RefNotesCurrent_Val = txtRefPNNotes_Part3.Text.Trim()
                gPartProject.PNR.RefNotesCurrent_Rev = txtRefPNNewNotes_Rev.Text
            Else
                gPartProject.PNR.RefNotesCurrent_Exists = False
                gPartProject.PNR.RefNotesCurrent_Val = ""
                gPartProject.PNR.RefNotesCurrent_Rev = ""
            End If

            If (chkRefNotesLegacy_Exists.Checked) Then
                gPartProject.PNR.RefNotesLegacy_Exists = True
                gPartProject.PNR.RefNotesLegacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
                gPartProject.PNR.RefNotesLegacy_Val = txtRefPNNewNotes_Legacy.Text
                gPartProject.PNR.RefNotesLegacy_Rev = txtRefPNLegacyNotes_Rev.Text
            Else
                gPartProject.PNR.RefNotesLegacy_Exists = False
                gPartProject.PNR.RefNotesLegacy_Type = clsPartProject.clsPNR.eLegacyType.None
                gPartProject.PNR.RefNotesLegacy_Val = ""
                gPartProject.PNR.RefNotesLegacy_Rev = ""
            End If

            'End If
            'gUnit.System = gUnit.System

            ''mIPEProjectID = .Project_ID
            gIPE_Project.Project_ID = .Project_ID
            'gIPEProject.PN_ID = .PN_ID
        End With

        'gIPEProject.CustName = mCustName

    End Sub


    Private Sub AddRecords(ByVal CustID_In As Integer, ByVal PlatformID_In As Integer, _
                           ByVal LocationID_In As Integer, ByVal PNID_In As Integer, ByVal RevID_In As Integer)
        '======================================================================================================

        If (mProjectView) Then

            'Dim pCustID As Integer = CustID_In

            'Try
            '    If (trvProjects.Nodes.Count = 0) Then

            '        If (txtCustomer.Text = "") Then
            '            MessageBox.Show("Customer can't be blank", "Customer Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            txtCustomer.Focus()
            '            Exit Sub
            '        Else

            '            If (txtPlatform.Text = "") Then
            '                MessageBox.Show("Platform can't be blank", "Platform Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                txtPlatform.Focus()
            '                Exit Sub
            '            Else
            '                If (txtLocation.Text = "") Then
            '                    MessageBox.Show("Location can't be blank", "Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                    txtLocation.Focus()
            '                    Exit Sub

            '                Else
            '                    If (txtParkerPN_Part3.Text = "") Then
            '                        MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                        txtParkerPN_Part3.Focus()
            '                        Exit Sub
            '                    End If
            '                End If
            '            End If
            '        End If

            '        '....Customer
            '        Dim pCustRecCount As Integer = (From Customer In mPartEntities.tblCustomer).Count()
            '        Dim pCustomerID As Integer = 0
            '        If (pCustRecCount > 0) Then
            '            Dim pCustName As String = txtCustomer.Text.Trim()
            '            Dim pCustCount As Integer = (From Customer In mPartEntities.tblCustomer Where Customer.fldName = pCustName).Count()

            '            If (pCustCount > 0) Then
            '                '....Existing Customer
            '                Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
            '                             Where Customer.fldName = pCustName).First()
            '                pCustomerID = pCustomer_Out.fldID
            '                pCustID = pCustomerID
            '            Else
            '                '....New Customer
            '                Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
            '                             Order By Customer.fldID Descending).First()

            '                pCustomerID = pCustomer_Out.fldID
            '                Dim pCustomer As New tblCustomer
            '                pCustID = pCustomerID + 1
            '                pCustomer.fldID = pCustID
            '                pCustomer.fldName = txtCustomer.Text.Trim()
            '                pCustomer.fldDimUnit = cmbUnit.Text
            '                mPartEntities.AddTotblCustomer(pCustomer)
            '                mPartEntities.SaveChanges()
            '            End If

            '        Else
            '            '....New Customer
            '            pCustomerID = 1
            '            Dim pCustomer As New tblCustomer
            '            pCustID = 1
            '            pCustomer.fldID = pCustID
            '            pCustomer.fldName = txtCustomer.Text.Trim()
            '            pCustomer.fldDimUnit = cmbUnit.Text
            '            mPartEntities.AddTotblCustomer(pCustomer)
            '            mPartEntities.SaveChanges()
            '        End If

            '        mCustomerID = pCustID

            '        '....Platform 
            '        Dim pPlatformName As String = txtPlatform.Text.Trim()

            '        Dim pPlatformRecCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID).Count()
            '        Dim pPlatformID As Integer = 0

            '        If (pPlatformRecCount > 0) Then

            '            Dim pPlatCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).Count()

            '            If (pPlatCount > 0) Then
            '                '....Existing Platform
            '                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
            '                            Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).First()
            '                pPlatformID = pPlatform_Out.fldID
            '            Else
            '                '....New Platform
            '                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
            '                           Where Platform.fldCustID = pCustID Order By Platform.fldID Descending).First()

            '                pPlatformID = pPlatform_Out.fldID + 1
            '                Dim pPlatform As New tblPlatform
            '                pPlatform.fldCustID = pCustID
            '                pPlatform.fldID = pPlatformID
            '                pPlatform.fldName = pPlatformName
            '                mPartEntities.AddTotblPlatform(pPlatform)
            '                mPartEntities.SaveChanges()

            '            End If
            '        Else
            '            '....New Customer & Platform
            '            Dim pPlatform As New tblPlatform
            '            pPlatform.fldCustID = pCustID
            '            pPlatform.fldID = 1
            '            pPlatform.fldName = pPlatformName
            '            mPartEntities.AddTotblPlatform(pPlatform)
            '            mPartEntities.SaveChanges()
            '            pPlatformID = 1
            '        End If

            '        mPlatformID = pPlatformID

            '        '....Location
            '        Dim pLocationName As String = txtLocation.Text.Trim()

            '        Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
            '        Dim pLocationID As Integer = 0

            '        If (pLocationRecCount > 0) Then

            '            Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

            '            If pLocCount > 0 Then
            '                '....Existing Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                            Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
            '                pLocationID = pLoc_Out.fldID
            '            Else
            '                '....New Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                           Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
            '                pLocationID = pLoc_Out.fldID + 1

            '                Dim pLocation As New tblLocation
            '                pLocation.fldCustID = pCustID
            '                pLocation.fldPlatformID = pPlatformID
            '                pLocation.fldID = pLocationID
            '                pLocation.fldLoc = pLocationName
            '                mPartEntities.AddTotblLocation(pLocation)
            '                mPartEntities.SaveChanges()
            '            End If

            '        Else
            '            '....New Platform & Location
            '            Dim pLocation As New tblLocation
            '            pLocation.fldCustID = pCustID
            '            pLocation.fldPlatformID = pPlatformID
            '            pLocationID = 1
            '            pLocation.fldID = pLocationID
            '            pLocation.fldLoc = pLocationName
            '            mPartEntities.AddTotblLocation(pLocation)
            '            mPartEntities.SaveChanges()

            '        End If

            '        mLocationID = pLocationID


            '        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
            '        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

            '        Dim pRevision As String
            '        If (txtPN_PH_Rev.Text <> "") Then
            '            pRevision = txtPN_PH_Rev.Text
            '        Else
            '            pRevision = "0"
            '        End If

            '        Dim pRevID As Integer = 0

            '        If pPNID = 0 Then
            '            '....Add to tblPN
            '            Dim pPN As New tblPN
            '            Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
            '                                            Order By PN.fldID Descending).Count()
            '            Dim pID As Integer = 0
            '            If (pPN_Rec_Count > 0) Then
            '                Dim pPN_Out = (From PN In mPartEntities.tblPN
            '                            Order By PN.fldID Descending).First()
            '                pID = pPN_Out.fldID
            '            End If

            '            pPN.fldID = pID + 1
            '            pPN.fldNew = pParkerPN

            '            If (mPartProject.Catalogued) Then
            '                pPN.fldLegacyType = 1
            '                mPartProject.Catalogued = True
            '            Else
            '                pPN.fldLegacyType = 0
            '                mPartProject.Catalogued = False
            '            End If

            '            pPN.fldLegacy = txtParkerPN_Legacy.Text

            '            pPN.fldParent = txtPNNew_Parent.Text
            '            pPN.fldParentRev = txtPNParentNew_Rev.Text
            '            pPN.fldAppType = "Face-Seal"
            '            'pPN.fldGeomTemplate = chkGeomTemplate.Checked
            '            pPN.fldDate = DateTime.Now

            '            pPNID = pID + 1

            '            mPartEntities.AddTotblPN(pPN)
            '            mPartEntities.SaveChanges()

            '            '....Add to tblRev                
            '            Dim pRev As New tblRev
            '            pRev.fldPNID = pID + 1
            '            pRev.fldID = 1
            '            pRev.fldNew = pRevision
            '            pRevID = 1
            '            pRev.fldSealIPE = True
            '            pRev.fldSealTest = False

            '            mPartEntities.AddTotblRev(pRev)
            '            mPartEntities.SaveChanges()

            '        Else
            '            '....PN Exists
            '            pRevID = mPartProject.GetRevID(pPNID, pRevision)

            '            If pRevID = 0 Then

            '                Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
            '                                                Where Rev.fldPNID = pPNID
            '                                                Order By Rev.fldID Descending).Count()
            '                Dim pID As Integer = 0
            '                If (pRev_Rec_Count > 0) Then
            '                    '....Add to tblRev
            '                    Dim pRev_Out = (From Rev In mPartEntities.tblRev
            '                                    Where Rev.fldPNID = pPNID
            '                                    Order By Rev.fldID Descending).First()

            '                    pID = pRev_Out.fldID
            '                End If

            '                Dim pRev As New tblRev
            '                pRev.fldPNID = pPNID
            '                pRev.fldID = pID + 1
            '                pRev.fldNew = pRevision
            '                pRev.fldLegacy = txtParkerPNLegacy_Rev.Text     'AES 23MAR17
            '                pRevID = pID + 1
            '                pRev.fldSealIPE = True
            '                pRev.fldSealTest = False

            '                mPartEntities.AddTotblRev(pRev)
            '                mPartEntities.SaveChanges()
            '            Else
            '                'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                'Exit Sub
            '            End If

            '        End If

            '        mPNID = pPNID
            '        mParkerPN_Rev = pRevision
            '        mRevID = pRevID

            '        Dim pPartPNR_CustInfo As New tblProject
            '        pPartPNR_CustInfo.fldCustID = pCustID
            '        pPartPNR_CustInfo.fldPlatformID = pPlatformID
            '        pPartPNR_CustInfo.fldLocID = pLocationID
            '        pPartPNR_CustInfo.fldPNID = pPNID
            '        pPartPNR_CustInfo.fldRevID = pRevID
            '        pPartPNR_CustInfo.fldPN_Cust = txtCustomerPN.Text
            '        mPartProject.PN_Cust = txtCustomerPN.Text

            '        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject
            '                                        Where Project.fldPNID = pPNID And Project.fldRevID = pRevID And
            '                                        Project.fldCustID = pCustID And Project.fldPlatformID = pPlatformID And
            '                                        Project.fldLocID = pLocationID).Count()
            '        Dim pProjectID As Integer = 0
            '        If (pProjectCount > 0) Then
            '            'Record already exists. No action needed.
            '        Else
            '            'Record not exists.
            '            Dim pProject_Count As Integer = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).Count()
            '            If (pProject_Count > 0) Then
            '                Dim pProject_Out = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).First()

            '                pProjectID = pProject_Out.fldID
            '                pPartPNR_CustInfo.fldID = pProjectID + 1

            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            Else
            '                pProjectID = 1
            '                pPartPNR_CustInfo.fldID = pProjectID
            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            End If

            '        End If



            '    ElseIf (trvProjects.SelectedNode.Level = 0) Then

            '        If (txtCustomer.Text = "") Then
            '            MessageBox.Show("Customer can't be blank", "Customer Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            txtCustomer.Focus()
            '            Exit Sub
            '        Else

            '            If (txtPlatform.Text = "") Then
            '                MessageBox.Show("Platform can't be blank", "Platform Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                txtPlatform.Focus()
            '                Exit Sub
            '            Else
            '                If (txtLocation.Text = "") Then
            '                    MessageBox.Show("Location can't be blank", "Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                    txtLocation.Focus()
            '                    Exit Sub

            '                Else
            '                    If (txtParkerPN_Part3.Text = "") Then
            '                        MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                        txtParkerPN_Part3.Focus()
            '                        Exit Sub
            '                    End If
            '                End If
            '            End If
            '        End If

            '        '....Customer
            '        Dim pCustRecCount As Integer = (From Customer In mPartEntities.tblCustomer).Count()
            '        Dim pCustomerID As Integer = 0
            '        If (pCustRecCount > 0) Then
            '            Dim pCustName As String = txtCustomer.Text.Trim()
            '            Dim pCustCount As Integer = (From Customer In mPartEntities.tblCustomer Where Customer.fldName = pCustName).Count()

            '            If (pCustCount > 0) Then
            '                '....Existing Customer
            '                Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
            '                             Where Customer.fldName = pCustName).First()
            '                pCustomerID = pCustomer_Out.fldID
            '                pCustID = pCustomerID
            '            Else
            '                '....New Customer
            '                Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
            '                             Order By Customer.fldID Descending).First()

            '                pCustomerID = pCustomer_Out.fldID
            '                Dim pCustomer As New tblCustomer
            '                pCustID = pCustomerID + 1
            '                pCustomer.fldID = pCustID
            '                pCustomer.fldName = txtCustomer.Text.Trim()
            '                pCustomer.fldDimUnit = cmbUnit.Text
            '                mPartEntities.AddTotblCustomer(pCustomer)
            '                mPartEntities.SaveChanges()
            '            End If

            '        Else
            '            '....New Customer
            '            pCustomerID = 1
            '            Dim pCustomer As New tblCustomer
            '            pCustID = 1
            '            pCustomer.fldID = pCustID
            '            pCustomer.fldName = txtCustomer.Text.Trim()
            '            pCustomer.fldDimUnit = cmbUnit.Text
            '            mPartEntities.AddTotblCustomer(pCustomer)
            '            mPartEntities.SaveChanges()
            '        End If

            '        mCustomerID = pCustID

            '        '....Platform 
            '        Dim pPlatformName As String = txtPlatform.Text.Trim()

            '        Dim pPlatformRecCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID).Count()
            '        Dim pPlatformID As Integer = 0

            '        If (pPlatformRecCount > 0) Then

            '            Dim pPlatCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).Count()

            '            If (pPlatCount > 0) Then
            '                '....Existing Platform
            '                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
            '                            Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).First()
            '                pPlatformID = pPlatform_Out.fldID
            '            Else
            '                '....New Platform
            '                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
            '                           Where Platform.fldCustID = pCustID Order By Platform.fldID Descending).First()

            '                pPlatformID = pPlatform_Out.fldID + 1
            '                Dim pPlatform As New tblPlatform
            '                pPlatform.fldCustID = pCustID
            '                pPlatform.fldID = pPlatformID
            '                pPlatform.fldName = pPlatformName
            '                mPartEntities.AddTotblPlatform(pPlatform)
            '                mPartEntities.SaveChanges()

            '            End If
            '        Else
            '            '....New Customer & Platform
            '            Dim pPlatform As New tblPlatform
            '            pPlatform.fldCustID = pCustID
            '            pPlatform.fldID = 1
            '            pPlatform.fldName = pPlatformName
            '            mPartEntities.AddTotblPlatform(pPlatform)
            '            mPartEntities.SaveChanges()
            '            pPlatformID = 1
            '        End If

            '        mPlatformID = pPlatformID

            '        '....Location
            '        Dim pLocationName As String = txtLocation.Text.Trim()

            '        Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
            '        Dim pLocationID As Integer = 0

            '        If (pLocationRecCount > 0) Then

            '            Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

            '            If pLocCount > 0 Then
            '                '....Existing Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                            Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
            '                pLocationID = pLoc_Out.fldID
            '            Else
            '                '....New Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                           Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
            '                pLocationID = pLoc_Out.fldID + 1

            '                Dim pLocation As New tblLocation
            '                pLocation.fldCustID = pCustID
            '                pLocation.fldPlatformID = pPlatformID
            '                pLocation.fldID = pLocationID
            '                pLocation.fldLoc = pLocationName
            '                mPartEntities.AddTotblLocation(pLocation)
            '                mPartEntities.SaveChanges()
            '            End If

            '        Else
            '            '....New Platform & Location
            '            Dim pLocation As New tblLocation
            '            pLocation.fldCustID = pCustID
            '            pLocation.fldPlatformID = pPlatformID
            '            pLocationID = 1
            '            pLocation.fldID = pLocationID
            '            pLocation.fldLoc = pLocationName
            '            mPartEntities.AddTotblLocation(pLocation)
            '            mPartEntities.SaveChanges()

            '        End If

            '        mLocationID = pLocationID


            '        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
            '        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

            '        Dim pRevision As String
            '        If (txtPN_PH_Rev.Text <> "") Then
            '            pRevision = txtPN_PH_Rev.Text
            '        Else
            '            pRevision = "0"
            '        End If

            '        Dim pRevID As Integer = 0

            '        If pPNID = 0 Then
            '            '....Add to tblPN
            '            Dim pPN As New tblPN
            '            Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
            '                                            Order By PN.fldID Descending).Count()
            '            Dim pID As Integer = 0
            '            If (pPN_Rec_Count > 0) Then
            '                Dim pPN_Out = (From PN In mPartEntities.tblPN
            '                            Order By PN.fldID Descending).First()
            '                pID = pPN_Out.fldID
            '            End If

            '            pPN.fldID = pID + 1
            '            pPN.fldNew = pParkerPN

            '            If (mPartProject.Catalogued) Then
            '                pPN.fldLegacyType = 1
            '                mPartProject.Catalogued = True
            '            Else
            '                pPN.fldLegacyType = 0
            '                mPartProject.Catalogued = False
            '            End If

            '            pPN.fldLegacy = txtParkerPN_Legacy.Text

            '            pPN.fldParent = txtPNNew_Parent.Text
            '            pPN.fldParentRev = txtPNParentNew_Rev.Text
            '            pPN.fldAppType = "Face-Seal"
            '            'pPN.fldGeomTemplate = chkGeomTemplate.Checked
            '            pPN.fldDate = DateTime.Now

            '            pPNID = pID + 1

            '            mPartEntities.AddTotblPN(pPN)
            '            mPartEntities.SaveChanges()

            '            '....Add to tblRev                
            '            Dim pRev As New tblRev
            '            pRev.fldPNID = pID + 1
            '            pRev.fldID = 1
            '            pRev.fldNew = pRevision
            '            pRevID = 1
            '            pRev.fldSealIPE = True
            '            pRev.fldSealTest = False

            '            mPartEntities.AddTotblRev(pRev)
            '            mPartEntities.SaveChanges()

            '        Else
            '            '....PN Exists
            '            pRevID = mPartProject.GetRevID(pPNID, pRevision)

            '            If pRevID = 0 Then

            '                Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
            '                                                Where Rev.fldPNID = pPNID
            '                                                Order By Rev.fldID Descending).Count()
            '                Dim pID As Integer = 0
            '                If (pRev_Rec_Count > 0) Then
            '                    '....Add to tblRev
            '                    Dim pRev_Out = (From Rev In mPartEntities.tblRev
            '                                    Where Rev.fldPNID = pPNID
            '                                    Order By Rev.fldID Descending).First()

            '                    pID = pRev_Out.fldID
            '                End If

            '                Dim pRev As New tblRev
            '                pRev.fldPNID = pPNID
            '                pRev.fldID = pID + 1
            '                pRev.fldNew = pRevision
            '                pRev.fldLegacy = txtParkerPNLegacy_Rev.Text     'AES 23MAR17
            '                pRevID = pID + 1
            '                pRev.fldSealIPE = True
            '                pRev.fldSealTest = False

            '                mPartEntities.AddTotblRev(pRev)
            '                mPartEntities.SaveChanges()
            '            Else
            '                'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                'Exit Sub
            '            End If

            '        End If

            '        mPNID = pPNID
            '        mParkerPN_Rev = pRevision
            '        mRevID = pRevID

            '        Dim pPartPNR_CustInfo As New tblProject
            '        pPartPNR_CustInfo.fldCustID = pCustID
            '        pPartPNR_CustInfo.fldPlatformID = pPlatformID
            '        pPartPNR_CustInfo.fldLocID = pLocationID
            '        pPartPNR_CustInfo.fldPNID = pPNID
            '        pPartPNR_CustInfo.fldRevID = pRevID
            '        pPartPNR_CustInfo.fldPN_Cust = txtCustomerPN.Text
            '        mPartProject.Customer.CustName = txtCustomerPN.Text

            '        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject
            '                                        Where Project.fldPNID = pPNID And Project.fldRevID = pRevID And
            '                                        Project.fldCustID = pCustID And Project.fldPlatformID = pPlatformID And
            '                                        Project.fldLocID = pLocationID).Count()
            '        Dim pProjectID As Integer = 0
            '        If (pProjectCount > 0) Then
            '            'Record already exists. No action needed.
            '        Else
            '            'Record not exists.
            '            Dim pProject_Count As Integer = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).Count()
            '            If (pProject_Count > 0) Then
            '                Dim pProject_Out = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).First()

            '                pProjectID = pProject_Out.fldID
            '                pPartPNR_CustInfo.fldID = pProjectID + 1

            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            Else
            '                pProjectID = 1
            '                pPartPNR_CustInfo.fldID = pProjectID
            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            End If

            '        End If

            '    ElseIf (trvProjects.SelectedNode.Level = 1) Then

            '        If (txtPlatform.Text = "") Then
            '            MessageBox.Show("Platform can't be blank", "Platform Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            txtPlatform.Focus()
            '            Exit Sub
            '        Else
            '            If (txtLocation.Text = "") Then
            '                MessageBox.Show("Location can't be blank", "Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                txtLocation.Focus()
            '                Exit Sub

            '            Else
            '                If (txtParkerPN_Part3.Text = "") Then
            '                    MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                    txtParkerPN_Part3.Focus()
            '                    Exit Sub
            '                End If
            '            End If
            '        End If

            '        'Dim pPlatform_Rec_Count As Integer = (From Platform In mPartEntities.tblPlatform
            '        '                                      Where Platform.fldCustID = CustID_In
            '        '                                      Order By Platform.fldID Descending).Count()

            '        pCustID = CustID_In
            '        '....Platform 
            '        Dim pPlatformName As String = txtPlatform.Text.Trim()

            '        Dim pPlatformRecCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID).Count()
            '        Dim pPlatformID As Integer = 0

            '        If (pPlatformRecCount > 0) Then

            '            Dim pPlatCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).Count()

            '            If (pPlatCount > 0) Then
            '                '....Existing Platform
            '                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
            '                            Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).First()
            '                pPlatformID = pPlatform_Out.fldID
            '            Else
            '                '....New Platform
            '                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
            '                           Where Platform.fldCustID = pCustID Order By Platform.fldID Descending).First()

            '                pPlatformID = pPlatform_Out.fldID + 1
            '                Dim pPlatform As New tblPlatform
            '                pPlatform.fldCustID = pCustID
            '                pPlatform.fldID = pPlatformID
            '                pPlatform.fldName = pPlatformName
            '                mPartEntities.AddTotblPlatform(pPlatform)
            '                mPartEntities.SaveChanges()

            '            End If
            '        Else
            '            '....New Customer & Platform
            '            Dim pPlatform As New tblPlatform
            '            pPlatform.fldCustID = pCustID
            '            pPlatform.fldID = 1
            '            pPlatform.fldName = pPlatformName
            '            mPartEntities.AddTotblPlatform(pPlatform)
            '            mPartEntities.SaveChanges()
            '            pPlatformID = 1
            '        End If

            '        mPlatformID = pPlatformID

            '        '....Location
            '        Dim pLocationName As String = txtLocation.Text.Trim()

            '        Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
            '        Dim pLocationID As Integer = 0

            '        If (pLocationRecCount > 0) Then

            '            Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

            '            If pLocCount > 0 Then
            '                '....Existing Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                            Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
            '                pLocationID = pLoc_Out.fldID
            '            Else
            '                '....New Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                           Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
            '                pLocationID = pLoc_Out.fldID + 1

            '                Dim pLocation As New tblLocation
            '                pLocation.fldCustID = pCustID
            '                pLocation.fldPlatformID = pPlatformID
            '                pLocation.fldID = pLocationID
            '                pLocation.fldLoc = pLocationName
            '                mPartEntities.AddTotblLocation(pLocation)
            '                mPartEntities.SaveChanges()
            '            End If

            '        Else
            '            '....New Platform & Location
            '            Dim pLocation As New tblLocation
            '            pLocation.fldCustID = pCustID
            '            pLocation.fldPlatformID = pPlatformID
            '            pLocationID = 1
            '            pLocation.fldID = pLocationID
            '            pLocation.fldLoc = pLocationName
            '            mPartEntities.AddTotblLocation(pLocation)
            '            mPartEntities.SaveChanges()

            '        End If

            '        mLocationID = pLocationID


            '        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
            '        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

            '        Dim pRevision As String
            '        If (txtPN_PH_Rev.Text <> "") Then
            '            pRevision = txtPN_PH_Rev.Text
            '        Else
            '            pRevision = "0"
            '        End If

            '        Dim pRevID As Integer = 0

            '        If pPNID = 0 Then
            '            '....Add to tblPN
            '            Dim pPN As New tblPN
            '            Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
            '                                            Order By PN.fldID Descending).Count()
            '            Dim pID As Integer = 0
            '            If (pPN_Rec_Count > 0) Then
            '                Dim pPN_Out = (From PN In mPartEntities.tblPN
            '                            Order By PN.fldID Descending).First()
            '                pID = pPN_Out.fldID
            '            End If

            '            pPN.fldID = pID + 1
            '            pPN.fldNew = pParkerPN

            '            If (mPartProject.Catalogued) Then
            '                pPN.fldLegacyType = 1
            '                mPartProject.Catalogued = True
            '            Else
            '                pPN.fldLegacyType = 0
            '                mPartProject.Catalogued = False
            '            End If

            '            pPN.fldLegacy = txtParkerPN_Legacy.Text

            '            pPN.fldParent = txtPNNew_Parent.Text
            '            pPN.fldParentRev = txtPNParentNew_Rev.Text
            '            pPN.fldAppType = "Face-Seal"
            '            'pPN.fldGeomTemplate = chkGeomTemplate.Checked
            '            pPN.fldDate = DateTime.Now

            '            pPNID = pID + 1

            '            mPartEntities.AddTotblPN(pPN)
            '            mPartEntities.SaveChanges()

            '            '....Add to tblRev                
            '            Dim pRev As New tblRev
            '            pRev.fldPNID = pID + 1
            '            pRev.fldID = 1
            '            pRev.fldNew = pRevision
            '            pRevID = 1
            '            pRev.fldSealIPE = True
            '            pRev.fldSealTest = False

            '            mPartEntities.AddTotblRev(pRev)
            '            mPartEntities.SaveChanges()

            '        Else
            '            '....PN Exists
            '            pRevID = mPartProject.GetRevID(pPNID, pRevision)

            '            If pRevID = 0 Then

            '                Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
            '                                                Where Rev.fldPNID = pPNID
            '                                                Order By Rev.fldID Descending).Count()
            '                Dim pID As Integer = 0
            '                If (pRev_Rec_Count > 0) Then
            '                    '....Add to tblRev
            '                    Dim pRev_Out = (From Rev In mPartEntities.tblRev
            '                                    Where Rev.fldPNID = pPNID
            '                                    Order By Rev.fldID Descending).First()

            '                    pID = pRev_Out.fldID
            '                End If

            '                Dim pRev As New tblRev
            '                pRev.fldPNID = pPNID
            '                pRev.fldID = pID + 1
            '                pRev.fldNew = pRevision
            '                pRev.fldLegacy = txtParkerPNLegacy_Rev.Text     'AES 23MAR17
            '                pRevID = pID + 1
            '                pRev.fldSealIPE = True
            '                pRev.fldSealTest = False

            '                mPartEntities.AddTotblRev(pRev)
            '                mPartEntities.SaveChanges()
            '            Else
            '                'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                'Exit Sub
            '            End If

            '        End If

            '        mPNID = pPNID
            '        mParkerPN_Rev = pRevision
            '        mRevID = pRevID

            '        Dim pPartPNR_CustInfo As New tblProject
            '        pPartPNR_CustInfo.fldCustID = pCustID
            '        pPartPNR_CustInfo.fldPlatformID = pPlatformID
            '        pPartPNR_CustInfo.fldLocID = pLocationID
            '        pPartPNR_CustInfo.fldPNID = pPNID
            '        pPartPNR_CustInfo.fldRevID = pRevID
            '        pPartPNR_CustInfo.fldPN_Cust = txtCustomerPN.Text
            '        mPartProject.PN_Cust = txtCustomerPN.Text

            '        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject
            '                                        Where Project.fldPNID = pPNID And Project.fldRevID = pRevID And
            '                                        Project.fldCustID = pCustID And Project.fldPlatformID = pPlatformID And
            '                                        Project.fldLocID = pLocationID).Count()
            '        Dim pProjectID As Integer = 0
            '        If (pProjectCount > 0) Then
            '            'Record already exists. No action needed.
            '        Else
            '            'Record not exists.
            '            Dim pProject_Count As Integer = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).Count()
            '            If (pProject_Count > 0) Then
            '                Dim pProject_Out = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).First()

            '                pProjectID = pProject_Out.fldID
            '                pPartPNR_CustInfo.fldID = pProjectID + 1

            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            Else
            '                pProjectID = 1
            '                pPartPNR_CustInfo.fldID = pProjectID
            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            End If

            '        End If


            '    ElseIf (trvProjects.SelectedNode.Level = 2) Then

            '        If (txtLocation.Text = "") Then
            '            MessageBox.Show("Location can't be blank", "Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            txtLocation.Focus()
            '            Exit Sub

            '        Else
            '            If (txtParkerPN_Part3.Text = "") Then
            '                MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '                txtParkerPN_Part3.Focus()
            '                Exit Sub
            '            End If
            '        End If

            '        'Dim pLocation_Rec_Count As Integer = (From Location In mPartEntities.tblLocation
            '        '                                      Where Location.fldCustID = CustID_In And
            '        '                                            Location.fldPlatformID = PlatformID_In
            '        '                                      Order By Location.fldID Descending).Count()

            '        pCustID = CustID_In
            '        Dim pPlatformID As Integer = PlatformID_In

            '        '....Location
            '        Dim pLocationName As String = txtLocation.Text.Trim()

            '        Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
            '        Dim pLocationID As Integer = 0

            '        If (pLocationRecCount > 0) Then

            '            Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

            '            If pLocCount > 0 Then
            '                '....Existing Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                            Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
            '                pLocationID = pLoc_Out.fldID
            '            Else
            '                '....New Location
            '                Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
            '                           Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
            '                pLocationID = pLoc_Out.fldID + 1

            '                Dim pLocation As New tblLocation
            '                pLocation.fldCustID = pCustID
            '                pLocation.fldPlatformID = pPlatformID
            '                pLocation.fldID = pLocationID
            '                pLocation.fldLoc = pLocationName
            '                mPartEntities.AddTotblLocation(pLocation)
            '                mPartEntities.SaveChanges()
            '            End If

            '        Else
            '            '....New Platform & Location
            '            Dim pLocation As New tblLocation
            '            pLocation.fldCustID = pCustID
            '            pLocation.fldPlatformID = pPlatformID
            '            pLocationID = 1
            '            pLocation.fldID = pLocationID
            '            pLocation.fldLoc = pLocationName
            '            mPartEntities.AddTotblLocation(pLocation)
            '            mPartEntities.SaveChanges()

            '        End If

            '        mLocationID = pLocationID


            '        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
            '        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

            '        Dim pRevision As String
            '        If (txtPN_PH_Rev.Text <> "") Then
            '            pRevision = txtPN_PH_Rev.Text
            '        Else
            '            pRevision = "0"
            '        End If

            '        Dim pRevID As Integer = 0

            '        If pPNID = 0 Then
            '            '....Add to tblPN
            '            Dim pPN As New tblPN
            '            Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
            '                                            Order By PN.fldID Descending).Count()
            '            Dim pID As Integer = 0
            '            If (pPN_Rec_Count > 0) Then
            '                Dim pPN_Out = (From PN In mPartEntities.tblPN
            '                            Order By PN.fldID Descending).First()
            '                pID = pPN_Out.fldID
            '            End If

            '            pPN.fldID = pID + 1
            '            pPN.fldNew = pParkerPN

            '            If (mPartProject.Catalogued) Then
            '                pPN.fldLegacyType = 1
            '                mPartProject.Catalogued = True
            '            Else
            '                pPN.fldLegacyType = 0
            '                mPartProject.Catalogued = False
            '            End If

            '            pPN.fldLegacy = txtParkerPN_Legacy.Text

            '            pPN.fldParent = txtPNNew_Parent.Text
            '            pPN.fldParentRev = txtPNParentNew_Rev.Text
            '            pPN.fldAppType = "Face-Seal"
            '            'pPN.fldGeomTemplate = chkGeomTemplate.Checked
            '            pPN.fldDate = DateTime.Now

            '            pPNID = pID + 1

            '            mPartEntities.AddTotblPN(pPN)
            '            mPartEntities.SaveChanges()

            '            '....Add to tblRev                
            '            Dim pRev As New tblRev
            '            pRev.fldPNID = pID + 1
            '            pRev.fldID = 1
            '            pRev.fldNew = pRevision
            '            pRevID = 1
            '            pRev.fldSealIPE = True
            '            pRev.fldSealTest = False

            '            mPartEntities.AddTotblRev(pRev)
            '            mPartEntities.SaveChanges()

            '        Else
            '            '....PN Exists
            '            pRevID = mPartProject.GetRevID(pPNID, pRevision)

            '            If pRevID = 0 Then

            '                Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
            '                                                Where Rev.fldPNID = pPNID
            '                                                Order By Rev.fldID Descending).Count()
            '                Dim pID As Integer = 0
            '                If (pRev_Rec_Count > 0) Then
            '                    '....Add to tblRev
            '                    Dim pRev_Out = (From Rev In mPartEntities.tblRev
            '                                    Where Rev.fldPNID = pPNID
            '                                    Order By Rev.fldID Descending).First()

            '                    pID = pRev_Out.fldID
            '                End If

            '                Dim pRev As New tblRev
            '                pRev.fldPNID = pPNID
            '                pRev.fldID = pID + 1
            '                pRev.fldNew = pRevision
            '                pRev.fldLegacy = txtParkerPNLegacy_Rev.Text     'AES 23MAR17
            '                pRevID = pID + 1
            '                pRev.fldSealIPE = True
            '                pRev.fldSealTest = False

            '                mPartEntities.AddTotblRev(pRev)
            '                mPartEntities.SaveChanges()
            '            Else
            '                'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                'Exit Sub
            '            End If

            '        End If

            '        mPNID = pPNID
            '        mParkerPN_Rev = pRevision
            '        mRevID = pRevID

            '        Dim pPartPNR_CustInfo As New tblProject
            '        pPartPNR_CustInfo.fldCustID = pCustID
            '        pPartPNR_CustInfo.fldPlatformID = pPlatformID
            '        pPartPNR_CustInfo.fldLocID = pLocationID
            '        pPartPNR_CustInfo.fldPNID = pPNID
            '        pPartPNR_CustInfo.fldRevID = pRevID
            '        pPartPNR_CustInfo.fldPN_Cust = txtCustomerPN.Text
            '        mPartProject.PN_Cust = txtCustomerPN.Text

            '        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject
            '                                        Where Project.fldPNID = pPNID And Project.fldRevID = pRevID And
            '                                        Project.fldCustID = pCustID And Project.fldPlatformID = pPlatformID And
            '                                        Project.fldLocID = pLocationID).Count()
            '        Dim pProjectID As Integer = 0
            '        If (pProjectCount > 0) Then
            '            'Record already exists. No action needed.
            '        Else
            '            'Record not exists.
            '            Dim pProject_Count As Integer = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).Count()
            '            If (pProject_Count > 0) Then
            '                Dim pProject_Out = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).First()

            '                pProjectID = pProject_Out.fldID
            '                pPartPNR_CustInfo.fldID = pProjectID + 1

            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            Else
            '                pProjectID = 1
            '                pPartPNR_CustInfo.fldID = pProjectID
            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            End If

            '        End If

            '    ElseIf (trvProjects.SelectedNode.Level = 3) Then

            '        If (txtParkerPN_Part3.Text = "") Then
            '            MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            txtParkerPN_Part3.Focus()
            '            Exit Sub
            '        End If

            '        pCustID = CustID_In
            '        Dim pPlatformID As Integer = PlatformID_In
            '        Dim pLocationID As Integer = LocationID_In

            '        Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
            '        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)

            '        Dim pRevision As String
            '        If (txtPN_PH_Rev.Text <> "") Then
            '            pRevision = txtPN_PH_Rev.Text
            '        Else
            '            pRevision = "0"
            '        End If

            '        Dim pRevID As Integer = 0

            '        If pPNID = 0 Then
            '            '....Add to tblPN
            '            Dim pPN As New tblPN
            '            Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
            '                                            Order By PN.fldID Descending).Count()
            '            Dim pID As Integer = 0
            '            If (pPN_Rec_Count > 0) Then
            '                Dim pPN_Out = (From PN In mPartEntities.tblPN
            '                            Order By PN.fldID Descending).First()
            '                pID = pPN_Out.fldID
            '            End If

            '            pPN.fldID = pID + 1
            '            pPN.fldNew = pParkerPN

            '            If (mPartProject.Catalogued) Then
            '                pPN.fldLegacyType = 1
            '                mPartProject.Catalogued = True
            '            Else
            '                pPN.fldLegacyType = 0
            '                mPartProject.Catalogued = False
            '            End If

            '            pPN.fldLegacy = txtParkerPN_Legacy.Text

            '            pPN.fldParent = txtPNNew_Parent.Text
            '            pPN.fldParentRev = txtPNParentNew_Rev.Text
            '            pPN.fldAppType = "Face-Seal"
            '            'pPN.fldGeomTemplate = chkGeomTemplate.Checked
            '            pPN.fldDate = DateTime.Now

            '            pPNID = pID + 1

            '            mPartEntities.AddTotblPN(pPN)
            '            mPartEntities.SaveChanges()

            '            '....Add to tblRev                
            '            Dim pRev As New tblRev
            '            pRev.fldPNID = pID + 1
            '            pRev.fldID = 1
            '            pRev.fldNew = pRevision
            '            pRevID = 1
            '            pRev.fldSealIPE = True
            '            pRev.fldSealTest = False

            '            mPartEntities.AddTotblRev(pRev)
            '            mPartEntities.SaveChanges()

            '        Else
            '            '....PN Exists
            '            pRevID = mPartProject.GetRevID(pPNID, pRevision)

            '            If pRevID = 0 Then

            '                Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
            '                                                Where Rev.fldPNID = pPNID
            '                                                Order By Rev.fldID Descending).Count()
            '                Dim pID As Integer = 0
            '                If (pRev_Rec_Count > 0) Then
            '                    '....Add to tblRev
            '                    Dim pRev_Out = (From Rev In mPartEntities.tblRev
            '                                    Where Rev.fldPNID = pPNID
            '                                    Order By Rev.fldID Descending).First()

            '                    pID = pRev_Out.fldID
            '                End If

            '                Dim pRev As New tblRev
            '                pRev.fldPNID = pPNID
            '                pRev.fldID = pID + 1
            '                pRev.fldNew = pRevision
            '                pRev.fldLegacy = txtParkerPNLegacy_Rev.Text     'AES 23MAR17
            '                pRevID = pID + 1
            '                pRev.fldSealIPE = True
            '                pRev.fldSealTest = False

            '                mPartEntities.AddTotblRev(pRev)
            '                mPartEntities.SaveChanges()
            '            Else
            '                'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '                'Exit Sub
            '            End If

            '        End If

            '        mPNID = pPNID
            '        mParkerPN_Rev = pRevision
            '        mRevID = pRevID

            '        Dim pPartPNR_CustInfo As New tblProject
            '        pPartPNR_CustInfo.fldCustID = pCustID
            '        pPartPNR_CustInfo.fldPlatformID = pPlatformID
            '        pPartPNR_CustInfo.fldLocID = pLocationID
            '        pPartPNR_CustInfo.fldPNID = pPNID
            '        pPartPNR_CustInfo.fldRevID = pRevID
            '        pPartPNR_CustInfo.fldPN_Cust = txtCustomerPN.Text
            '        mPartProject.PN_Cust = txtCustomerPN.Text

            '        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject
            '                                        Where Project.fldPNID = pPNID And Project.fldRevID = pRevID And
            '                                        Project.fldCustID = pCustID And Project.fldPlatformID = pPlatformID And
            '                                        Project.fldLocID = pLocationID).Count()
            '        Dim pProjectID As Integer = 0
            '        If (pProjectCount > 0) Then
            '            'Record already exists. No action needed.
            '        Else
            '            'Record not exists.
            '            Dim pProject_Count As Integer = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).Count()
            '            If (pProject_Count > 0) Then
            '                Dim pProject_Out = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).First()

            '                pProjectID = pProject_Out.fldID
            '                pPartPNR_CustInfo.fldID = pProjectID + 1

            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            Else
            '                pProjectID = 1
            '                pPartPNR_CustInfo.fldID = pProjectID
            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            End If

            '        End If

            '    ElseIf (trvProjects.SelectedNode.Level = 4) Then

            '        Dim pRevision As String
            '        If (txtPN_PH_Rev.Text <> "") Then
            '            pRevision = txtPN_PH_Rev.Text
            '        Else
            '            pRevision = "0"
            '        End If

            '        Dim pRevID As Integer = 0

            '        '....PN Exists
            '        pRevID = mPartProject.GetRevID(PNID_In, pRevision)

            '        If pRevID = 0 Then

            '            Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
            '                                            Where Rev.fldPNID = PNID_In
            '                                            Order By Rev.fldID Descending).Count()

            '            Dim pID As Integer = 0
            '            If (pRev_Rec_Count > 0) Then
            '                '....Add to tblRev
            '                Dim pRev_Out = (From Rev In mPartEntities.tblRev
            '                                Where Rev.fldPNID = PNID_In
            '                                Order By Rev.fldID Descending).First()

            '                pID = pRev_Out.fldID
            '            End If

            '            Dim pRev As New tblRev
            '            pRev.fldPNID = PNID_In
            '            pRev.fldID = pID + 1
            '            pRev.fldNew = pRevision
            '            pRev.fldLegacy = txtParkerPNLegacy_Rev.Text     'AES 23MAR17
            '            pRevID = pID + 1
            '            pRev.fldSealIPE = True
            '            pRev.fldSealTest = False

            '            mPartEntities.AddTotblRev(pRev)
            '            mPartEntities.SaveChanges()

            '        Else
            '            'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '            'Exit Sub
            '        End If

            '        mPNID = PNID_In
            '        mParkerPN_Rev = pRevision       'AES 29AUG16
            '        mRevID = pRevID

            '        pCustID = CustID_In
            '        Dim pPlatformID As Integer = PlatformID_In
            '        Dim pLocationID As Integer = LocationID_In
            '        Dim pPNID As Integer = PNID_In

            '        Dim pPartPNR_CustInfo As New tblProject
            '        pPartPNR_CustInfo.fldCustID = pCustID
            '        pPartPNR_CustInfo.fldPlatformID = pPlatformID
            '        pPartPNR_CustInfo.fldLocID = pLocationID
            '        pPartPNR_CustInfo.fldPNID = pPNID
            '        pPartPNR_CustInfo.fldRevID = pRevID
            '        pPartPNR_CustInfo.fldPN_Cust = txtCustomerPN.Text
            '        mPartProject.PN_Cust = txtCustomerPN.Text

            '        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject
            '                                        Where Project.fldPNID = pPNID And Project.fldRevID = pRevID And
            '                                        Project.fldCustID = pCustID And Project.fldPlatformID = pPlatformID And
            '                                        Project.fldLocID = pLocationID).Count()
            '        Dim pProjectID As Integer = 0
            '        If (pProjectCount > 0) Then
            '            'Record already exists. No action needed.
            '        Else
            '            'Record not exists.
            '            Dim pProject_Count As Integer = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).Count()
            '            If (pProject_Count > 0) Then
            '                Dim pProject_Out = (From Project In mPartEntities.tblProject
            '                                Order By Project.fldID Descending).First()

            '                pProjectID = pProject_Out.fldID
            '                pPartPNR_CustInfo.fldID = pProjectID + 1

            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            Else
            '                pProjectID = 1
            '                pPartPNR_CustInfo.fldID = pProjectID
            '                mPartEntities.AddTotblProject(pPartPNR_CustInfo)
            '                mPartEntities.SaveChanges()
            '            End If

            '        End If

            '    End If

            '    mblnAdd = False     'AES 12OCT17
            '    InitializeControl(False)
            '    UpdateIndexField()  'AES 22SEP16
            '    PopulateTreeView()
            '    trvProjects.ExpandAll()
            '    SelectTreeNode()

            'Catch ex As Exception

            'End Try

        ElseIf (mPNView) Then

            If (trvProjects.Nodes.Count = 0) Then

                If (txtParkerPN_Part3.Text = "" And txtParkerPN_Legacy.Text = "") Then
                    MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtParkerPN_Part3.Focus()
                    Exit Sub

                End If

                If (txtCustomer.Text.Trim() <> "") Then
                    '....Customer
                    Dim pCustID As Integer = 0
                    Dim pCustRecCount As Integer = (From Customer In mPartEntities.tblCustomer).Count()
                    Dim pCustomerID As Integer = 0
                    If (pCustRecCount > 0) Then
                        Dim pCustName As String = txtCustomer.Text.Trim()
                        Dim pCustCount As Integer = (From Customer In mPartEntities.tblCustomer Where Customer.fldName = pCustName).Count()

                        If (pCustCount > 0) Then
                            '....Existing Customer
                            Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
                                         Where Customer.fldName = pCustName).First()
                            pCustomerID = pCustomer_Out.fldID
                            pCustID = pCustomerID
                        Else
                            '....New Customer
                            Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
                                         Order By Customer.fldID Descending).First()

                            pCustomerID = pCustomer_Out.fldID
                            Dim pCustomer As New tblCustomer
                            pCustID = pCustomerID + 1
                            pCustomer.fldID = pCustID
                            pCustomer.fldName = txtCustomer.Text.Trim()
                            pCustomer.fldDimUnit = cmbUnit.Text
                            'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text
                            mPartEntities.AddTotblCustomer(pCustomer)
                            mPartEntities.SaveChanges()
                        End If

                    Else
                        '....New Customer
                        pCustomerID = 1
                        Dim pCustomer As New tblCustomer
                        pCustID = 1
                        pCustomer.fldID = pCustID
                        pCustomer.fldName = txtCustomer.Text.Trim()
                        pCustomer.fldDimUnit = cmbUnit.Text
                        'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text
                        mPartEntities.AddTotblCustomer(pCustomer)
                        mPartEntities.SaveChanges()
                    End If

                    mCustomerID = pCustID

                    If (txtPlatform.Text.Trim() <> "") Then
                        '....Platform 
                        Dim pPlatformName As String = txtPlatform.Text.Trim()

                        Dim pPlatformRecCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID).Count()
                        Dim pPlatformID As Integer = 0

                        If (pPlatformRecCount > 0) Then

                            Dim pPlatCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).Count()

                            If (pPlatCount > 0) Then
                                '....Existing Platform
                                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
                                            Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).First()
                                pPlatformID = pPlatform_Out.fldID
                            Else
                                '....New Platform
                                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
                                           Where Platform.fldCustID = pCustID Order By Platform.fldID Descending).First()

                                pPlatformID = pPlatform_Out.fldID + 1
                                Dim pPlatform As New tblPlatform
                                pPlatform.fldCustID = pCustID
                                pPlatform.fldID = pPlatformID
                                pPlatform.fldName = pPlatformName
                                mPartEntities.AddTotblPlatform(pPlatform)
                                mPartEntities.SaveChanges()

                            End If
                        Else
                            '....New Customer & Platform
                            Dim pPlatform As New tblPlatform
                            pPlatform.fldCustID = pCustID
                            pPlatform.fldID = 1
                            pPlatform.fldName = pPlatformName
                            mPartEntities.AddTotblPlatform(pPlatform)
                            mPartEntities.SaveChanges()
                            pPlatformID = 1
                        End If

                        mPlatformID = pPlatformID

                        If (txtLocation.Text.Trim() <> "") Then
                            '....Location
                            Dim pLocationName As String = txtLocation.Text.Trim()

                            Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
                            Dim pLocationID As Integer = 0

                            If (pLocationRecCount > 0) Then

                                Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

                                If pLocCount > 0 Then
                                    '....Existing Location
                                    Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
                                                Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
                                    pLocationID = pLoc_Out.fldID
                                Else
                                    '....New Location
                                    Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
                                               Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
                                    pLocationID = pLoc_Out.fldID + 1

                                    Dim pLocation As New tblLocation
                                    pLocation.fldCustID = pCustID
                                    pLocation.fldPlatformID = pPlatformID
                                    pLocation.fldID = pLocationID
                                    pLocation.fldLoc = pLocationName
                                    mPartEntities.AddTotblLocation(pLocation)
                                    mPartEntities.SaveChanges()
                                End If

                            Else
                                '....New Platform & Location
                                Dim pLocation As New tblLocation
                                pLocation.fldCustID = pCustID
                                pLocation.fldPlatformID = pPlatformID
                                pLocationID = 1
                                pLocation.fldID = pLocationID
                                pLocation.fldLoc = pLocationName
                                mPartEntities.AddTotblLocation(pLocation)
                                mPartEntities.SaveChanges()

                            End If

                            mLocationID = pLocationID
                        End If

                    End If

                End If


                Dim pParkerPN As String = ""

                If (txtParkerPN_Part3.Text <> "") Then
                    pParkerPN = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text.Trim()
                Else
                    pParkerPN = txtParkerPN_Legacy.Text
                End If

                Dim pPNID As Integer = mPNID 'mPartProject.GetPNID(pParkerPN)

                If pPNID = 0 Then
                    Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
                                                    Order By PN.fldID Descending).Count()
                    Dim pID As Integer = 0
                    If (pPN_Rec_Count > 0) Then
                        Dim pPN_Out = (From PN In mPartEntities.tblPN
                                   Order By PN.fldID Descending).First()
                        pID = pPN_Out.fldID
                    End If

                    '....Add to tblPN
                    Dim pPN As New tblPN
                    pPNID = pID + 1
                    pPN.fldID = pPNID
                    If (txtParkerPN_Part3.Text <> "") Then
                        pPN.fldCurrentExists = True
                        pPN.fldCurrent = pParkerPN
                    Else
                        pPN.fldCurrentExists = False
                        pPN.fldCurrent = ""
                    End If


                    If (mPartProject.PNR.Legacy.Exists) Then
                        pPN.fldLegacyExists = True
                        If (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                            pPN.fldLegacyType = 0
                        ElseIf (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Other) Then
                            pPN.fldLegacyType = 1
                        Else
                            pPN.fldLegacyType = -1
                        End If
                        pPN.fldLegacy = txtParkerPN_Legacy.Text
                    Else
                        pPN.fldLegacyExists = False
                        pPN.fldLegacyType = -1
                        pPN.fldLegacy = ""
                    End If

                    If (mPartProject.PNR.ParentCurrent.Exists) Then
                        pPN.fldParentCurrentExists = True
                        Dim pParentCurrent As String = ""
                        If (txtParentCur_Part3.Text <> "") Then
                            pParentCurrent = txtParentCur_Part1.Text + "-" + cmbParentCur_Part2.Text + txtParentCur_Part3.Text.Trim() 'txtPNNew_Parent.Text
                        End If
                        pPN.fldParentCurrent = pParentCurrent
                        pPN.fldParentCurrentRev = txtParentCur_Rev.Text
                    Else
                        pPN.fldParentCurrentExists = False
                        pPN.fldParentCurrent = ""
                        pPN.fldParentCurrentRev = ""
                    End If

                    If (mPartProject.PNR.ParentLegacy.Exists) Then
                        pPN.fldParentLegacyExists = True
                        pPN.fldParentLegacy = txtPNLegacy_Parent.Text
                        pPN.fldParentLegacyRev = txtPNParentLegacy_Rev.Text
                    Else
                        pPN.fldParentLegacyExists = False
                        pPN.fldParentLegacy = ""
                        pPN.fldParentLegacyRev = ""
                    End If

                    pPN.fldAppType = "Face-Seal"
                    pPN.fldDimUnit = cmbUnit.Text
                    'pPN.fldGeomTemplate = chkGeomTemplate.Checked
                    'pPN.fldDate = DateTime.Now
                    'pPN.fldIndex = pPNID

                    '======================================
                    If (txtRefPNNewDim_Part3.Text <> "") Then
                        Dim pRefPNNewDim As String = "NH-" & cmbRefPNNewDim_Part2.Text & txtRefPNNewDim_Part3.Text.Trim()
                        pPN.fldRefDimCurrentExists = True
                        pPN.fldRefDimCurrent = pRefPNNewDim
                        pPN.fldRefDimCurrentRev = txtRefPNNewDim_Rev.Text
                    Else
                        pPN.fldRefDimCurrentExists = False
                        pPN.fldRefDimCurrent = ""
                        pPN.fldRefDimCurrentRev = ""
                    End If

                    If (txtRefPNNewDim_Legacy.Text <> "") Then
                        pPN.fldRefDimLegacyExists = True
                        pPN.fldRefDimLegacy = txtRefPNNewDim_Legacy.Text
                        pPN.fldRefDimLegacyRev = txtRefPNLegacyDim_Rev.Text
                    Else
                        pPN.fldRefDimLegacyExists = False
                        pPN.fldRefDimLegacy = ""
                        pPN.fldRefDimLegacyRev = ""
                    End If

                    If (txtRefPNNotes_Part3.Text <> "") Then
                        Dim pRefPNNewNotes As String = "NH-" & cmbRefNotesNewPN_Part2.Text & txtRefPNNotes_Part3.Text.Trim()
                        pPN.fldRefNotesCurrentExists = True
                        pPN.fldRefNotesCurrent = pRefPNNewNotes
                        pPN.fldRefNotesCurrentRev = txtRefPNNewNotes_Rev.Text
                    Else
                        pPN.fldRefNotesCurrentExists = False
                        pPN.fldRefNotesCurrent = ""
                        pPN.fldRefNotesCurrentRev = ""
                    End If

                    If (txtRefPNNewNotes_Legacy.Text <> "") Then
                        pPN.fldRefNotesLegacyExists = True
                        pPN.fldRefNotesLegacy = txtRefPNNewNotes_Legacy.Text
                        pPN.fldRefNotesLegacyRev = txtRefPNLegacyNotes_Rev.Text
                    Else
                        pPN.fldRefNotesLegacyExists = False
                        pPN.fldRefNotesLegacy = ""
                        pPN.fldRefNotesLegacyRev = ""
                    End If

                    '======================================
                    mPartEntities.AddTotblPN(pPN)
                    mPartEntities.SaveChanges()

                    Dim pRevision As String
                    If (txtPN_PH_Rev.Text <> "") Then
                        pRevision = txtPN_PH_Rev.Text

                    ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                        pRevision = txtParkerPNLegacy_Rev.Text

                    Else
                        pRevision = "0"
                    End If

                    Dim pRevID As Integer = 0

                    '....PN Exists
                    'pRevID = mRevID 'mPartProject.GetRevID(pPNID, pRevision)
                    Dim pRev_Rec = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID And Rev.fldCurrent = pRevision
                                                        Select Rev).ToList()
                    If (pRev_Rec.Count > 0) Then
                        pRevID = pRev_Rec(0).fldID

                    Else
                        Dim pRev_Rec1 = (From Rev In mPartEntities.tblRev
                                                       Where Rev.fldPNID = pPNID And Rev.fldLegacy = pRevision
                                                       Select Rev).ToList()
                        If (pRev_Rec1.Count > 0) Then
                            pRevID = pRev_Rec1(0).fldID
                        End If
                    End If

                    If pRevID = 0 Then

                        Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID
                                                        Order By Rev.fldID Descending).Count()

                        Dim pRev_ID As Integer = 0
                        If (pRev_Rec_Count > 0) Then
                            '....Add to tblRev
                            Dim pRev_Out = (From Rev In mPartEntities.tblRev
                                            Where Rev.fldPNID = pPNID
                                            Order By Rev.fldID Descending).First()

                            pRev_ID = pRev_Out.fldID
                        End If

                        Dim pRev As New tblRev
                        pRev.fldPNID = pPNID
                        pRev.fldID = pRev_ID + 1
                        If (txtPN_PH_Rev.Text <> "") Then
                            pRev.fldCurrent = pRevision
                        Else
                            pRev.fldCurrent = ""
                        End If

                        If (txtParkerPNLegacy_Rev.Text <> "") Then
                            pRev.fldLegacy = pRevision
                        Else
                            pRev.fldLegacy = ""
                        End If

                        pRevID = pRev_ID + 1

                        'pRev.fldSealIPE = chkSealIPE.Checked
                        'pRev.fldSealTest = True
                        'pRev.fldIndex = pRevID
                        mPartEntities.AddTotblRev(pRev)
                        mPartEntities.SaveChanges()

                    Else
                        'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'Exit Sub
                    End If

                    mPNID = pPNID
                    'mParkerPN_Rev = pRevision
                    mRevID = pRevID

                    Dim pPNR_CustInfoID As Integer = 0

                    '....Customer Exists
                    If (mCustomerID <> 0 And mPlatformID <> 0 And mLocationID <> 0) Then

                        Dim pProject As New tblProject
                        pProject.fldCustID = mCustomerID
                        pProject.fldPlatformID = mPlatformID
                        pProject.fldLocID = mLocationID
                        pProject.fldPNID = pPNID
                        pProject.fldRevID = pRevID
                        pProject.fldPN_Cust = txtCustomerPN.Text
                        pProject.fldPN_Cust_Rev = txtCustomerPN_Rev.Text
                        'pProject.fldSealIPE = False
                        'pProject.fldSealTest = False
                        'pProject.fldSealProcess = False
                        pProject.fldDateCreated = DateTime.Now()
                        pProject.fldDateLastModified = DateTime.Now()

                        'mPartProject.PN_Cust = txtCustomerPN.Text

                        Dim pProjectRecCount As Integer = (From pRec In mPartEntities.tblProject
                                                 Where pRec.fldCustID = mCustomerID And
                                                       pRec.fldPlatformID = mPlatformID And
                                                       pRec.fldLocID = mLocationID And
                                                       pRec.fldPNID = pPNID And
                                                       pRec.fldRevID = pRevID Select pRec).Count()

                        If (pProjectRecCount = 0) Then

                            Dim pPNR_RecCount As Integer = (From Project In mPartEntities.tblProject).Count()
                            Dim pPNR_ProjectID As Integer = 0
                            If (pPNR_RecCount > 0) Then
                                Dim pProject_Out = (From Project In mPartEntities.tblProject
                                               Order By Project.fldID Descending).First()

                                pPNR_ProjectID = pProject_Out.fldID
                            End If

                            pProject.fldID = pPNR_ProjectID + 1

                            mPartEntities.AddTotblProject(pProject)
                            mPartEntities.SaveChanges()

                            pPNR_CustInfoID = pPNR_ProjectID + 1

                        Else
                            MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    End If

                Else

                    'Dim pRevID As Integer = 0

                    Dim pRevision As String
                    If (txtPN_PH_Rev.Text <> "") Then
                        pRevision = txtPN_PH_Rev.Text

                    ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                        pRevision = txtParkerPNLegacy_Rev.Text

                    Else
                        pRevision = "0"
                    End If

                    '....PN Exists
                    'pRevID = mRevID 'mPartProject.GetRevID(pPNID, pRevision)
                    Dim pRevID As Integer = 0

                    '....PN Exists
                    Dim pRev_Rec = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID And Rev.fldCurrent = pRevision
                                                        Select Rev).ToList()
                    If (pRev_Rec.Count > 0) Then
                        pRevID = pRev_Rec(0).fldID

                    Else
                        Dim pRev_Rec1 = (From Rev In mPartEntities.tblRev
                                                       Where Rev.fldPNID = pPNID And Rev.fldLegacy = pRevision
                                                       Select Rev).ToList()
                        If (pRev_Rec1.Count > 0) Then
                            pRevID = pRev_Rec1(0).fldID
                        End If
                    End If

                    If pRevID = 0 Then

                        Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID
                                                        Order By Rev.fldID Descending).Count()

                        Dim pRev_ID As Integer = 0
                        If (pRev_Rec_Count > 0) Then
                            '....Add to tblRev
                            Dim pRev_Out = (From Rev In mPartEntities.tblRev
                                            Where Rev.fldPNID = pPNID
                                            Order By Rev.fldID Descending).First()

                            pRev_ID = pRev_Out.fldID
                        End If

                        Dim pRev As New tblRev
                        pRev.fldPNID = pPNID
                        pRev.fldID = pRev_ID + 1
                        If (txtPN_PH_Rev.Text <> "") Then
                            pRev.fldCurrent = pRevision
                        Else
                            pRev.fldCurrent = ""
                        End If

                        If (txtParkerPNLegacy_Rev.Text <> "") Then
                            pRev.fldLegacy = pRevision
                        Else
                            pRev.fldLegacy = ""
                        End If

                        pRevID = pRev_ID + 1

                        'pRev.fldSealIPE = chkSealIPE.Checked
                        'pRev.fldSealTest = True
                        'pRev.fldIndex = pRevID
                        mPartEntities.AddTotblRev(pRev)
                        mPartEntities.SaveChanges()

                    Else
                        'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'Exit Sub
                    End If

                    mPNID = pPNID
                    'mParkerPN_Rev = pRevision
                    mRevID = pRevID

                    Dim pPNR_CustInfoID As Integer = 0

                    '....Customer Exists
                    If (mCustomerID <> 0 And mPlatformID <> 0 And mLocationID <> 0) Then

                        Dim pProject As New tblProject
                        pProject.fldCustID = mCustomerID
                        pProject.fldPlatformID = mPlatformID
                        pProject.fldLocID = mLocationID
                        pProject.fldPNID = pPNID
                        pProject.fldRevID = pRevID
                        pProject.fldPN_Cust = txtCustomerPN.Text
                        'mPartProject.PN_Cust = txtCustomerPN.Text
                        pProject.fldPN_Cust_Rev = txtCustomerPN_Rev.Text
                        'pProject.fldSealIPE = False
                        'pProject.fldSealTest = False
                        'pProject.fldSealProcess = False
                        pProject.fldDateCreated = DateTime.Now()
                        pProject.fldDateLastModified = DateTime.Now()

                        Dim pProjectRecCount As Integer = (From pRec In mPartEntities.tblProject
                                                 Where pRec.fldCustID = mCustomerID And
                                                       pRec.fldPlatformID = mPlatformID And
                                                       pRec.fldLocID = mLocationID And
                                                       pRec.fldPNID = pPNID And
                                                       pRec.fldRevID = pRevID Select pRec).Count()

                        If (pProjectRecCount = 0) Then

                            Dim pPNR_RecCount As Integer = (From Project In mPartEntities.tblProject).Count()
                            Dim pPNR_ProjectID As Integer = 0
                            If (pPNR_RecCount > 0) Then
                                Dim pProject_Out = (From Project In mPartEntities.tblProject
                                               Order By Project.fldID Descending).First()

                                pPNR_ProjectID = pProject_Out.fldID
                            End If

                            pProject.fldID = pPNR_ProjectID + 1

                            mPartEntities.AddTotblProject(pProject)
                            mPartEntities.SaveChanges()

                            pPNR_CustInfoID = pPNR_ProjectID + 1

                        Else
                            MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    End If

                End If

            ElseIf (trvProjects.SelectedNode.Level = 1) Then


                If (txtParkerPN_Part3.Text = "" And txtParkerPN_Legacy.Text = "") Then
                    MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtParkerPN_Part3.Focus()
                    Exit Sub

                End If

                If (txtCustomer.Text.Trim() <> "") Then
                    '....Customer
                    Dim pCustID As Integer = 0
                    Dim pCustRecCount As Integer = (From Customer In mPartEntities.tblCustomer).Count()
                    Dim pCustomerID As Integer = 0
                    If (pCustRecCount > 0) Then
                        Dim pCustName As String = txtCustomer.Text.Trim()
                        Dim pCustCount As Integer = (From Customer In mPartEntities.tblCustomer Where Customer.fldName = pCustName).Count()

                        If (pCustCount > 0) Then
                            '....Existing Customer
                            Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
                                         Where Customer.fldName = pCustName).First()
                            pCustomerID = pCustomer_Out.fldID
                            pCustID = pCustomerID
                        Else
                            '....New Customer
                            Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
                                         Order By Customer.fldID Descending).First()

                            pCustomerID = pCustomer_Out.fldID
                            Dim pCustomer As New tblCustomer
                            pCustID = pCustomerID + 1
                            pCustomer.fldID = pCustID
                            pCustomer.fldName = txtCustomer.Text.Trim()
                            pCustomer.fldDimUnit = cmbUnit.Text.Trim()
                            'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text
                            mPartEntities.AddTotblCustomer(pCustomer)
                            mPartEntities.SaveChanges()
                        End If

                    Else
                        '....New Customer
                        pCustomerID = 1
                        Dim pCustomer As New tblCustomer
                        pCustID = 1
                        pCustomer.fldID = pCustID
                        pCustomer.fldName = txtCustomer.Text.Trim()
                        pCustomer.fldDimUnit = cmbUnit.Text.Trim()
                        'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text
                        mPartEntities.AddTotblCustomer(pCustomer)
                        mPartEntities.SaveChanges()
                    End If

                    mCustomerID = pCustID

                    If (txtPlatform.Text.Trim() <> "") Then
                        '....Platform 
                        Dim pPlatformName As String = txtPlatform.Text.Trim()

                        Dim pPlatformRecCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID).Count()
                        Dim pPlatformID As Integer = 0

                        If (pPlatformRecCount > 0) Then

                            Dim pPlatCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).Count()

                            If (pPlatCount > 0) Then
                                '....Existing Platform
                                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
                                            Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).First()
                                pPlatformID = pPlatform_Out.fldID
                            Else
                                '....New Platform
                                Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
                                           Where Platform.fldCustID = pCustID Order By Platform.fldID Descending).First()

                                pPlatformID = pPlatform_Out.fldID + 1
                                Dim pPlatform As New tblPlatform
                                pPlatform.fldCustID = pCustID
                                pPlatform.fldID = pPlatformID
                                pPlatform.fldName = pPlatformName
                                mPartEntities.AddTotblPlatform(pPlatform)
                                mPartEntities.SaveChanges()

                            End If
                        Else
                            '....New Customer & Platform
                            Dim pPlatform As New tblPlatform
                            pPlatform.fldCustID = pCustID
                            pPlatform.fldID = 1
                            pPlatform.fldName = pPlatformName
                            mPartEntities.AddTotblPlatform(pPlatform)
                            mPartEntities.SaveChanges()
                            pPlatformID = 1
                        End If

                        mPlatformID = pPlatformID

                        If (txtLocation.Text.Trim() <> "") Then
                            '....Location
                            Dim pLocationName As String = txtLocation.Text.Trim()

                            Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
                            Dim pLocationID As Integer = 0

                            If (pLocationRecCount > 0) Then

                                Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

                                If pLocCount > 0 Then
                                    '....Existing Location
                                    Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
                                                Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
                                    pLocationID = pLoc_Out.fldID
                                Else
                                    '....New Location
                                    Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
                                               Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
                                    pLocationID = pLoc_Out.fldID + 1

                                    Dim pLocation As New tblLocation
                                    pLocation.fldCustID = pCustID
                                    pLocation.fldPlatformID = pPlatformID
                                    pLocation.fldID = pLocationID
                                    pLocation.fldLoc = pLocationName
                                    mPartEntities.AddTotblLocation(pLocation)
                                    mPartEntities.SaveChanges()
                                End If

                            Else
                                '....New Platform & Location
                                Dim pLocation As New tblLocation
                                pLocation.fldCustID = pCustID
                                pLocation.fldPlatformID = pPlatformID
                                pLocationID = 1
                                pLocation.fldID = pLocationID
                                pLocation.fldLoc = pLocationName
                                mPartEntities.AddTotblLocation(pLocation)
                                mPartEntities.SaveChanges()

                            End If

                            mLocationID = pLocationID
                        End If

                    End If

                End If


                Dim pParkerPN As String = ""

                If (txtParkerPN_Part3.Text <> "") Then
                    pParkerPN = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text.Trim()
                Else
                    pParkerPN = txtParkerPN_Legacy.Text
                End If

                Dim pPNID As Integer = 0 'mPNID 'mPartProject.GetPNID(pParkerPN)
                Dim pPN_Rec = (From PN In mPartEntities.tblPN Where PN.fldCurrent = pParkerPN Select PN).ToList()
                If (pPN_Rec.Count > 0) Then
                    pPNID = pPN_Rec(0).fldID
                Else
                    Dim pPN_Rec1 = (From PN In mPartEntities.tblPN Where PN.fldLegacy = pParkerPN Select PN).ToList()
                    If (pPN_Rec1.Count > 0) Then
                        pPNID = pPN_Rec1(0).fldID
                    End If
                End If

                If pPNID = 0 Then
                    Dim pPN_Rec_Count As Integer = (From PN In mPartEntities.tblPN
                                                    Order By PN.fldID Descending).Count()
                    Dim pID As Integer = 0
                    If (pPN_Rec_Count > 0) Then
                        Dim pPN_Out = (From PN In mPartEntities.tblPN
                                   Order By PN.fldID Descending).First()
                        pID = pPN_Out.fldID
                    End If

                    '....Add to tblPN
                    Dim pPN As New tblPN
                    pPNID = pID + 1
                    pPN.fldID = pPNID
                    If (txtParkerPN_Part3.Text <> "") Then
                        pPN.fldCurrentExists = True
                        pPN.fldCurrent = pParkerPN
                    Else
                        pPN.fldCurrentExists = False
                        pPN.fldCurrent = ""
                    End If


                    If (mPartProject.PNR.Legacy.Exists) Then
                        pPN.fldLegacyExists = True
                        If (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                            pPN.fldLegacyType = 0
                        ElseIf (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Other) Then
                            pPN.fldLegacyType = 1
                        Else
                            pPN.fldLegacyType = -1
                        End If
                        pPN.fldLegacy = txtParkerPN_Legacy.Text
                    Else
                        pPN.fldLegacyExists = False
                        pPN.fldLegacyType = -1
                        pPN.fldLegacy = ""
                    End If

                    If (mPartProject.PNR.ParentCurrent.Exists) Then
                        pPN.fldParentCurrentExists = True
                        Dim pParentCurrent As String = ""
                        If (txtParentCur_Part3.Text <> "") Then
                            pParentCurrent = txtParentCur_Part1.Text + "-" + cmbParentCur_Part2.Text + txtParentCur_Part3.Text.Trim() 'txtPNNew_Parent.Text
                        End If
                        pPN.fldParentCurrent = pParentCurrent
                        pPN.fldParentCurrentRev = txtParentCur_Rev.Text
                    End If

                    If (mPartProject.PNR.ParentLegacy.Exists) Then
                        pPN.fldParentLegacyExists = True
                        pPN.fldParentLegacy = txtPNLegacy_Parent.Text
                        pPN.fldParentLegacyRev = txtPNParentLegacy_Rev.Text
                    End If

                    pPN.fldAppType = "Face-Seal"
                    pPN.fldDimUnit = cmbUnit.Text.Trim()
                    'pPN.fldGeomTemplate = chkGeomTemplate.Checked
                    'pPN.fldDate = DateTime.Now
                    'pPN.fldIndex = pPNID

                    '======================================
                    If (txtRefPNNewDim_Part3.Text <> "") Then
                        Dim pRefPNNewDim As String = "NH-" & cmbRefPNNewDim_Part2.Text & txtRefPNNewDim_Part3.Text.Trim()
                        pPN.fldRefDimCurrentExists = True
                        pPN.fldRefDimCurrent = pRefPNNewDim
                        pPN.fldRefDimCurrentRev = txtRefPNNewDim_Rev.Text
                    Else
                        pPN.fldRefDimCurrentExists = False
                        pPN.fldRefDimCurrent = ""
                        pPN.fldRefDimCurrentRev = ""
                    End If

                    If (txtRefPNNewDim_Legacy.Text <> "") Then
                        pPN.fldRefDimLegacyExists = True
                        pPN.fldRefDimLegacy = txtRefPNNewDim_Legacy.Text
                        pPN.fldRefDimLegacyRev = txtRefPNLegacyDim_Rev.Text
                    Else
                        pPN.fldRefDimLegacyExists = False
                        pPN.fldRefDimLegacy = ""
                        pPN.fldRefDimLegacyRev = ""
                    End If

                    If (txtRefPNNotes_Part3.Text <> "") Then
                        Dim pRefPNNewNotes As String = "NH-" & cmbRefNotesNewPN_Part2.Text & txtRefPNNotes_Part3.Text.Trim()
                        pPN.fldRefNotesCurrentExists = True
                        pPN.fldRefNotesCurrent = pRefPNNewNotes
                        pPN.fldRefNotesCurrentRev = txtRefPNNewNotes_Rev.Text
                    Else
                        pPN.fldRefNotesCurrentExists = False
                        pPN.fldRefNotesCurrent = ""
                        pPN.fldRefNotesCurrentRev = ""
                    End If

                    If (txtRefPNNewNotes_Legacy.Text <> "") Then
                        pPN.fldRefNotesLegacyExists = True
                        pPN.fldRefNotesLegacy = txtRefPNNewNotes_Legacy.Text
                        pPN.fldRefNotesLegacyRev = txtRefPNLegacyNotes_Rev.Text
                    Else
                        pPN.fldRefNotesLegacyExists = False
                        pPN.fldRefNotesLegacy = ""
                        pPN.fldRefNotesLegacyRev = ""
                    End If

                    '======================================
                    mPartEntities.AddTotblPN(pPN)
                    mPartEntities.SaveChanges()

                    Dim pRevision As String
                    If (txtPN_PH_Rev.Text <> "") Then
                        pRevision = txtPN_PH_Rev.Text

                    ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                        pRevision = txtParkerPNLegacy_Rev.Text

                    Else
                        pRevision = "0"
                    End If


                    '....PN Exists
                    'pRevID = mRevID 'mPartProject.GetRevID(pPNID, pRevision)
                    Dim pRevID As Integer = 0

                    '....PN Exists
                    Dim pRev_Rec = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID And Rev.fldCurrent = pRevision
                                                        Select Rev).ToList()
                    If (pRev_Rec.Count > 0) Then
                        pRevID = pRev_Rec(0).fldID

                    Else
                        Dim pRev_Rec1 = (From Rev In mPartEntities.tblRev
                                                       Where Rev.fldPNID = pPNID And Rev.fldLegacy = pRevision
                                                       Select Rev).ToList()
                        If (pRev_Rec1.Count > 0) Then
                            pRevID = pRev_Rec1(0).fldID
                        End If
                    End If

                    If pRevID = 0 Then

                        Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID
                                                        Order By Rev.fldID Descending).Count()

                        Dim pRev_ID As Integer = 0
                        If (pRev_Rec_Count > 0) Then
                            '....Add to tblRev
                            Dim pRev_Out = (From Rev In mPartEntities.tblRev
                                            Where Rev.fldPNID = pPNID
                                            Order By Rev.fldID Descending).First()

                            pRev_ID = pRev_Out.fldID
                        End If

                        Dim pRev As New tblRev
                        pRev.fldPNID = pPNID
                        pRev.fldID = pRev_ID + 1
                        If (txtPN_PH_Rev.Text <> "") Then
                            pRev.fldCurrent = pRevision
                        Else
                            pRev.fldCurrent = ""
                        End If

                        If (txtParkerPNLegacy_Rev.Text <> "") Then
                            pRev.fldLegacy = pRevision
                        Else
                            pRev.fldLegacy = ""
                        End If

                        pRevID = pRev_ID + 1

                        'pRev.fldSealIPE = chkSealIPE.Checked
                        'pRev.fldSealTest = True
                        'pRev.fldIndex = pRevID
                        mPartEntities.AddTotblRev(pRev)
                        mPartEntities.SaveChanges()

                    Else
                        'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'Exit Sub
                    End If

                    mPNID = pPNID
                    'mParkerPN_Rev = pRevision
                    mRevID = pRevID

                    Dim pPNR_CustInfoID As Integer = 0

                    '....Customer Exists
                    If (mCustomerID <> 0 And mPlatformID <> 0 And mLocationID <> 0) Then

                        Dim pProject As New tblProject
                        pProject.fldCustID = mCustomerID
                        pProject.fldPlatformID = mPlatformID
                        pProject.fldLocID = mLocationID
                        pProject.fldPNID = pPNID
                        pProject.fldRevID = pRevID
                        pProject.fldPN_Cust = txtCustomerPN.Text
                        'mPartProject.PN_Cust = txtCustomerPN.Text
                        pProject.fldPN_Cust_Rev = txtCustomerPN_Rev.Text
                        'pProject.fldSealIPE = False
                        'pProject.fldSealTest = False
                        'pProject.fldSealProcess = False
                        pProject.fldDateCreated = DateTime.Now()
                        pProject.fldDateLastModified = DateTime.Now()

                        Dim pProjectRecCount As Integer = (From pRec In mPartEntities.tblProject
                                                 Where pRec.fldCustID = mCustomerID And
                                                       pRec.fldPlatformID = mPlatformID And
                                                       pRec.fldLocID = mLocationID And
                                                       pRec.fldPNID = pPNID And
                                                       pRec.fldRevID = pRevID Select pRec).Count()

                        If (pProjectRecCount = 0) Then

                            Dim pPNR_RecCount As Integer = (From Project In mPartEntities.tblProject).Count()
                            Dim pPNR_ProjectID As Integer = 0
                            If (pPNR_RecCount > 0) Then
                                Dim pProject_Out = (From Project In mPartEntities.tblProject
                                               Order By Project.fldID Descending).First()

                                pPNR_ProjectID = pProject_Out.fldID
                            End If

                            pProject.fldID = pPNR_ProjectID + 1

                            mPartEntities.AddTotblProject(pProject)
                            mPartEntities.SaveChanges()

                            pPNR_CustInfoID = pPNR_ProjectID + 1

                        Else
                            MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    End If

                Else

                    Dim pRevID As Integer = 0

                    Dim pRevision As String
                    If (txtPN_PH_Rev.Text <> "") Then
                        pRevision = txtPN_PH_Rev.Text

                    ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                        pRevision = txtParkerPNLegacy_Rev.Text

                    Else
                        pRevision = "0"
                    End If

                    '....PN Exists
                    pRevID = mRevID 'mPartProject.GetRevID(pPNID, pRevision)

                    If pRevID = 0 Then

                        Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
                                                        Where Rev.fldPNID = pPNID
                                                        Order By Rev.fldID Descending).Count()

                        Dim pRev_ID As Integer = 0
                        If (pRev_Rec_Count > 0) Then
                            '....Add to tblRev
                            Dim pRev_Out = (From Rev In mPartEntities.tblRev
                                            Where Rev.fldPNID = pPNID
                                            Order By Rev.fldID Descending).First()

                            pRev_ID = pRev_Out.fldID
                        End If

                        Dim pRev As New tblRev
                        pRev.fldPNID = pPNID
                        pRev.fldID = pRev_ID + 1
                        If (txtPN_PH_Rev.Text <> "") Then
                            pRev.fldCurrent = pRevision
                        Else
                            pRev.fldCurrent = ""
                        End If

                        If (txtParkerPNLegacy_Rev.Text <> "") Then
                            pRev.fldLegacy = pRevision
                        Else
                            pRev.fldLegacy = ""
                        End If

                        pRevID = pRev_ID + 1

                        'pRev.fldSealIPE = chkSealIPE.Checked
                        'pRev.fldSealTest = True
                        'pRev.fldIndex = pRevID
                        mPartEntities.AddTotblRev(pRev)
                        mPartEntities.SaveChanges()

                    Else
                        'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'Exit Sub
                    End If

                    mPNID = pPNID
                    'mParkerPN_Rev = pRevision
                    mRevID = pRevID

                    Dim pPNR_CustInfoID As Integer = 0

                    '....Customer Exists
                    If (mCustomerID <> 0 And mPlatformID <> 0 And mLocationID <> 0) Then

                        Dim pProject As New tblProject
                        pProject.fldCustID = mCustomerID
                        pProject.fldPlatformID = mPlatformID
                        pProject.fldLocID = mLocationID
                        pProject.fldPNID = pPNID
                        pProject.fldRevID = pRevID
                        pProject.fldPN_Cust = txtCustomerPN.Text
                        'mPartProject.PN_Cust = txtCustomerPN.Text
                        pProject.fldPN_Cust_Rev = txtCustomerPN_Rev.Text
                        'pProject.fldSealIPE = False
                        'pProject.fldSealTest = False
                        'pProject.fldSealProcess = False
                        pProject.fldDateCreated = DateTime.Now()
                        pProject.fldDateLastModified = DateTime.Now()

                        Dim pProjectRecCount As Integer = (From pRec In mPartEntities.tblProject
                                                 Where pRec.fldCustID = mCustomerID And
                                                       pRec.fldPlatformID = mPlatformID And
                                                       pRec.fldLocID = mLocationID And
                                                       pRec.fldPNID = pPNID And
                                                       pRec.fldRevID = pRevID Select pRec).Count()

                        If (pProjectRecCount = 0) Then

                            Dim pPNR_RecCount As Integer = (From Project In mPartEntities.tblProject).Count()
                            Dim pPNR_ProjectID As Integer = 0
                            If (pPNR_RecCount > 0) Then
                                Dim pProject_Out = (From Project In mPartEntities.tblProject
                                               Order By Project.fldID Descending).First()

                                pPNR_ProjectID = pProject_Out.fldID
                            End If

                            pProject.fldID = pPNR_ProjectID + 1

                            mPartEntities.AddTotblProject(pProject)
                            mPartEntities.SaveChanges()

                            pPNR_CustInfoID = pPNR_ProjectID + 1

                        Else
                            MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    End If

                End If


            ElseIf (trvProjects.SelectedNode.Level = 2) Then

                Dim pRevision As String
                If (txtPN_PH_Rev.Text <> "") Then
                    pRevision = txtPN_PH_Rev.Text

                ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                    pRevision = txtParkerPNLegacy_Rev.Text

                Else
                    pRevision = "0"
                End If

                '....PN Exists
                'pRevID = mRevID 'mPartProject.GetRevID(PNID_In, pRevision)
                Dim pRevID As Integer = 0

                '....PN Exists
                Dim pRev_Rec = (From Rev In mPartEntities.tblRev
                                                    Where Rev.fldPNID = PNID_In And Rev.fldCurrent = pRevision
                                                    Select Rev).ToList()
                If (pRev_Rec.Count > 0) Then
                    pRevID = pRev_Rec(0).fldID
                    mPartProject.PNR.Current_Exists = True
                    mPartProject.PNR.Legacy_Exists = False
                Else
                    Dim pRev_Rec1 = (From Rev In mPartEntities.tblRev
                                                   Where Rev.fldPNID = PNID_In And Rev.fldLegacy = pRevision
                                                   Select Rev).ToList()
                    If (pRev_Rec1.Count > 0) Then
                        pRevID = pRev_Rec1(0).fldID
                        mPartProject.PNR.Legacy_Exists = True
                        mPartProject.PNR.Current_Exists = False
                    End If
                End If

                If pRevID = 0 Then

                    Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
                                                    Where Rev.fldPNID = PNID_In
                                                    Order By Rev.fldID Descending).Count()

                    Dim pID As Integer = 0
                    If (pRev_Rec_Count > 0) Then
                        '....Add to tblRev
                        Dim pRev_Out = (From Rev In mPartEntities.tblRev
                                        Where Rev.fldPNID = PNID_In
                                        Order By Rev.fldID Descending).First()

                        pID = pRev_Out.fldID
                    End If

                    Dim pRev As New tblRev
                    pRev.fldPNID = PNID_In
                    pRev.fldID = pID + 1
                    If (txtPN_PH_Rev.Text <> "") Then
                        pRev.fldCurrent = pRevision
                    Else
                        pRev.fldCurrent = ""
                    End If

                    If (txtParkerPNLegacy_Rev.Text <> "") Then
                        pRev.fldLegacy = pRevision
                    Else
                        pRev.fldLegacy = ""
                    End If

                    pRevID = pID + 1

                    'pRev.fldSealIPE = chkSealIPE.Checked
                    'pRev.fldSealTest = True
                    'pRev.fldIndex = pRevID
                    mPartEntities.AddTotblRev(pRev)
                    mPartEntities.SaveChanges()

                    '....Retrive records of previous Rev HW.        'AES 18APR18
                    If (pID > 0) Then
                        mPartProject.PNR.RetrieveFromDB(PNID_In, pID)
                        If (Not IsNothing(mPartProject.PNR.HW.MCrossSecNo)) Then

                            If (mPartProject.PNR.HW.MCrossSecNo <> "") Then
                                mPartProject.PNR.SaveToDB(PNID_In, pRevID)

                                'AES 19APR18
                                Dim pPrev_Rev As String = ""
                                Dim pQryRev = (From it In mPartEntities.tblRev
                                               Where it.fldPNID = PNID_In And it.fldID = pID Select it).First()

                                If (Not IsDBNull(pQryRev.fldCurrent) And Not IsNothing(pQryRev.fldCurrent)) Then
                                    pPrev_Rev = pQryRev.fldCurrent
                                End If

                                MessageBox.Show("All the Hardware data of the Previous Rev " & pPrev_Rev & vbLf & "have been copied to this New Rev " +
                                            txtPN_PH_Rev.Text & ".", "New Rev Creation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If

                        End If

                    End If

                    Else
                    'MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Exit Sub
                End If

                mPNID = PNID_In
                'mParkerPN_Rev = pRevision
                mRevID = pRevID
                If (mPartProject.PNR.Current.Exists) Then
                    mPartProject.PNR.Current_Rev = txtPN_PH_Rev.Text

                ElseIf (mPartProject.PNR.Legacy.Exists) Then
                    mPartProject.PNR.Legacy_Rev = txtPN_PH_Rev.Text
                End If


                Dim pPNR_CustInfoID As Integer = 0
                '....Customer Exists
                If (CustID_In <> 0 And PlatformID_In <> 0 And LocationID_In <> 0) Then

                    Dim pProject As New tblProject
                    pProject.fldCustID = CustID_In
                    pProject.fldPlatformID = PlatformID_In
                    pProject.fldLocID = LocationID_In
                    pProject.fldPNID = PNID_In
                    pProject.fldRevID = pRevID
                    pProject.fldPN_Cust = txtCustomerPN.Text
                    'mPartProject.PN_Cust = txtCustomerPN.Text
                    pProject.fldPN_Cust_Rev = txtCustomerPN_Rev.Text
                    'pProject.fldSealIPE = False
                    'pProject.fldSealTest = False
                    'pProject.fldSealProcess = False
                    pProject.fldDateCreated = DateTime.Now()
                    pProject.fldDateLastModified = DateTime.Now()

                    Dim pProjectRecCount As Integer = (From pRec In mPartEntities.tblProject
                                             Where pRec.fldCustID = CustID_In And
                                                   pRec.fldPlatformID = PlatformID_In And
                                                   pRec.fldLocID = LocationID_In And
                                                   pRec.fldPNID = PNID_In And
                                                   pRec.fldRevID = pRevID Select pRec).Count()

                    If (pProjectRecCount = 0) Then

                        Dim pProjectCount As Integer = (From Project In mPartEntities.tblProject).Count()
                        Dim pProjectID As Integer = 0
                        If (pProjectCount > 0) Then
                            Dim pProject_Out = (From Project In mPartEntities.tblProject
                                           Order By Project.fldID Descending).First()

                            pProjectID = pProject_Out.fldID
                        End If

                        pProject.fldID = pProjectID + 1

                        mPartEntities.AddTotblProject(pProject)
                        mPartEntities.SaveChanges()

                        pPNR_CustInfoID = pProjectID + 1

                    Else
                        MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                End If

            End If

            mblnAdd = False
            InitializeControl(False)
            ''UpdateIndexField()  
            'PopulateTreeView()
            'trvProjects.ExpandAll()
            'SelectTreeNode()

            DisplayData()   'AES 25APR18

        End If

    End Sub


    Private Sub UpdateRecords(ByVal CustID_In As Integer, ByVal PlatformID_In As Integer, _
                              ByVal LocationID_In As Integer, ByVal PNID_In As Integer, ByVal PNRevID_In As Integer)
        '===========================================================================================================

        If (trvProjects.Nodes.Count > 0) Then

            If (mProjectView) Then

                'If (trvProjects.SelectedNode.Level = 0) Then
                '    Dim pCustomer = (From Cust In mPartEntities.tblCustomer
                '                        Where Cust.fldID = CustID_In).First()
                '    pCustomer.fldName = txtCustomer.Text
                '    pCustomer.fldDimUnit = cmbUnit.Text
                '    'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text

                'ElseIf (trvProjects.SelectedNode.Level = 1) Then
                '    Dim pPlatform = (From Platform In mPartEntities.tblPlatform
                '                        Where Platform.fldCustID = CustID_In And
                '                              Platform.fldID = PlatformID_In).First()
                '    pPlatform.fldName = txtPlatform.Text

                'ElseIf (trvProjects.SelectedNode.Level = 2) Then
                '    Dim pLocation = (From Location In mPartEntities.tblLocation
                '                        Where Location.fldCustID = CustID_In And
                '                              Location.fldPlatformID = PlatformID_In And
                '                              Location.fldID = LocationID_In).First()
                '    pLocation.fldLoc = txtLocation.Text

                'ElseIf (trvProjects.SelectedNode.Level = 3) Then
                '    Dim pPN = (From PN In mPartEntities.tblPN
                '                        Where PN.fldID = PNID_In).First()

                '    Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
                '    pPN.fldNew = pParkerPN.Trim()
                '    pPN.fldLegacy = txtParkerPN_Legacy.Text
                '    ''pPN.fldLegacy_Type = gProject.Catalogued     'AES 23MAR17    'Check AM/BG
                '    If (gPartProject.Catalogued) Then
                '        pPN.fldLegacyType = 1
                '    Else
                '        pPN.fldLegacyType = 0
                '    End If
                '    pPN.fldAppType = "Face-Seal"
                '    pPN.fldParent = txtPNNew_Parent.Text
                '    pPN.fldParentRev = txtPNParentNew_Rev.Text
                '    'pPN.fldGeomTemplate = chkGeomTemplate.Checked
                '    'pPN.fldSealIPE = chkSealIPE.Checked
                '    ''pPN.fldSealTest = chkSealTest.Checked
                '    ''pPN.fldSealProcess = False
                '    pPN.fldDate = DateTime.Now


                'ElseIf (trvProjects.SelectedNode.Level = 4) Then
                '    Dim pPNRev = (From PNRev In mPartEntities.tblRev
                '                        Where PNRev.fldPNID = PNID_In And
                '                              PNRev.fldID = PNRevID_In).First()
                '    pPNRev.fldNew = txtPN_PH_Rev.Text
                '    pPNRev.fldSealIPE = True
                '    pPNRev.fldSealTest = chkSealTest.Checked

                '    Dim pProject = (From Project In mPartEntities.tblProject
                '                        Where Project.fldCustID = CustID_In And
                '                              Project.fldPlatformID = PlatformID_In And
                '                              Project.fldLocID = LocationID_In And
                '                              Project.fldPNID = PNID_In And
                '                              Project.fldRevID = PNRevID_In).First()

                '    pProject.fldPN_Cust = txtCustomerPN.Text.Trim()
                '    mPartProject.PN_Cust = txtCustomerPN.Text.Trim()


                'End If

                'mPartEntities.SaveChanges()
                'mblnEdit = False    'AES 13OCT17
                'InitializeControl(False)
                'PopulateTreeView()
                'trvProjects.ExpandAll()
                'SelectTreeNode()

            ElseIf (mPNView) Then

                If (trvProjects.SelectedNode.Level = 1) Then

                    Dim pPN = (From PN In mPartEntities.tblPN
                                       Where PN.fldID = PNID_In).First()

                    Dim pParkerPN As String = ""
                    If (txtParkerPN_Part3.Text <> "") Then
                        pParkerPN = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text.Trim()
                    End If

                    If (chkLegacy.Checked) Then
                        pParkerPN = ""
                        txtPN_PH_Rev.Text = ""
                    Else
                        txtParkerPN_Legacy.Text = ""
                        txtParkerPNLegacy_Rev.Text = ""
                    End If

                    If (mPartProject.PNR.Current.Exists) Then
                        pPN.fldCurrentExists = True
                        pPN.fldCurrent = pParkerPN.Trim()
                    End If


                    If (mPartProject.PNR.Legacy.Exists) Then
                        If (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                            pPN.fldLegacyType = 0
                        ElseIf (mPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Other) Then
                            pPN.fldLegacyType = 1
                        Else
                            pPN.fldLegacyType = -1
                        End If
                        pPN.fldLegacyExists = True
                        pPN.fldLegacy = txtParkerPN_Legacy.Text

                    End If

                    If (mPartProject.PNR.ParentCurrent.Exists) Then
                        pPN.fldParentCurrentExists = True
                        'pPN.fldParentCurrent = txtPNNew_Parent.Text
                        Dim pParentCurrent As String = ""
                        If (txtParentCur_Part3.Text <> "") Then
                            pParentCurrent = txtParentCur_Part1.Text + "-" + cmbParentCur_Part2.Text + txtParentCur_Part3.Text.Trim()  'txtPNNew_Parent.Text
                        End If
                        pPN.fldParentCurrent = pParentCurrent
                        pPN.fldParentCurrentRev = txtParentCur_Rev.Text.Trim()
                    Else
                        pPN.fldParentCurrentExists = False
                        pPN.fldParentCurrent = ""
                        pPN.fldParentCurrentRev = ""
                    End If

                    If (mPartProject.PNR.ParentLegacy.Exists) Then
                        pPN.fldParentLegacyExists = True
                        pPN.fldParentLegacy = txtPNLegacy_Parent.Text
                        pPN.fldParentLegacyRev = txtPNParentLegacy_Rev.Text
                    Else
                        pPN.fldParentLegacyExists = False
                        pPN.fldParentLegacy = ""
                        pPN.fldParentLegacyRev = ""
                    End If

                    '======================================
                    If (txtRefPNNewDim_Part3.Text <> "") Then
                        Dim pRefPNNewDim As String = "NH-" & cmbRefPNNewDim_Part2.Text & txtRefPNNewDim_Part3.Text.Trim()
                        pPN.fldRefDimCurrentExists = True
                        pPN.fldRefDimCurrent = pRefPNNewDim
                        pPN.fldRefDimCurrentRev = txtRefPNNewDim_Rev.Text
                    Else
                        pPN.fldRefDimCurrentExists = False
                        pPN.fldRefDimCurrent = ""
                        pPN.fldRefDimCurrentRev = ""
                    End If

                    If (txtRefPNNewDim_Legacy.Text <> "") Then
                        pPN.fldRefDimLegacyExists = True
                        pPN.fldRefDimLegacy = txtRefPNNewDim_Legacy.Text
                        pPN.fldRefDimLegacyRev = txtRefPNLegacyDim_Rev.Text
                    Else
                        pPN.fldRefDimLegacyExists = False
                        pPN.fldRefDimLegacy = ""
                        pPN.fldRefDimLegacyRev = ""
                    End If

                    If (txtRefPNNotes_Part3.Text <> "") Then
                        Dim pRefPNNewNotes As String = "NH-" & cmbRefNotesNewPN_Part2.Text & txtRefPNNotes_Part3.Text.Trim()
                        pPN.fldRefNotesCurrentExists = True
                        pPN.fldRefNotesCurrent = pRefPNNewNotes
                        pPN.fldRefNotesCurrentRev = txtRefPNNewNotes_Rev.Text
                    Else
                        pPN.fldRefNotesCurrentExists = False
                        pPN.fldRefNotesCurrent = ""
                        pPN.fldRefNotesCurrentRev = ""
                    End If

                    If (txtRefPNNewNotes_Legacy.Text <> "") Then
                        pPN.fldRefNotesLegacyExists = True
                        pPN.fldRefNotesLegacy = txtRefPNNewNotes_Legacy.Text
                        pPN.fldRefNotesLegacyRev = txtRefPNLegacyNotes_Rev.Text
                    Else
                        pPN.fldRefNotesLegacyExists = False
                        pPN.fldRefNotesLegacy = ""
                        pPN.fldRefNotesLegacyRev = ""
                    End If

                    '======================================

                    pPN.fldAppType = "Face-Seal"
                    'pPN.fldGeomTemplate = chkGeomTemplate.Checked
                    'pPN.fldDate = DateTime.Now

                    mPartEntities.SaveChanges()

                    '....Rev
                    Dim pRevision As String
                    If (txtPN_PH_Rev.Text <> "") Then
                        pRevision = txtPN_PH_Rev.Text

                    ElseIf (txtParkerPNLegacy_Rev.Text <> "") Then
                        pRevision = txtParkerPNLegacy_Rev.Text

                    Else
                        pRevision = "0"
                    End If

                    Dim pRevID As Integer = 0

                    If (txtPN_PH_Rev.Text <> "" Or txtParkerPNLegacy_Rev.Text <> "") Then

                        '....PN Exists
                        pRevID = mRevID 'mPartProject.GetRevID(PNID_In, pRevision)

                        If pRevID = 0 Then

                            Dim pRev_Rec_Count As Integer = (From Rev In mPartEntities.tblRev
                                                            Where Rev.fldPNID = PNID_In
                                                            Order By Rev.fldID Descending).Count()

                            Dim pRev_ID As Integer = 0
                            If (pRev_Rec_Count > 0) Then
                                '....Add to tblRev
                                Dim pRev_Out = (From Rev In mPartEntities.tblRev
                                                Where Rev.fldPNID = PNID_In
                                                Order By Rev.fldID Descending).First()

                                pRev_ID = pRev_Out.fldID
                            End If

                            Dim pRev As New tblRev
                            pRev.fldPNID = PNID_In
                            pRev.fldID = pRev_ID + 1
                            If (txtPN_PH_Rev.Text <> "") Then
                                pRev.fldCurrent = pRevision
                            Else
                                pRev.fldCurrent = ""
                            End If

                            If (txtParkerPNLegacy_Rev.Text <> "") Then
                                pRev.fldLegacy = pRevision
                            Else
                                pRev.fldLegacy = ""
                            End If

                            pRevID = pRev_ID + 1

                            'pRev.fldSealIPE = chkSealIPE.Checked
                            'pRev.fldSealTest = True
                            'pRev.fldIndex = pRevID
                            mPartEntities.AddTotblRev(pRev)
                            mPartEntities.SaveChanges()
                        End If

                    End If

                ElseIf (trvProjects.SelectedNode.Level = 2) Then

                    Dim pPNRev = (From PNRev In mPartEntities.tblRev
                                        Where PNRev.fldPNID = PNID_In And
                                              PNRev.fldID = PNRevID_In).First()

                    pPNRev.fldCurrent = txtPN_PH_Rev.Text
                    pPNRev.fldLegacy = txtParkerPNLegacy_Rev.Text
                    'pPNRev.fldSealIPE = chkSealIPE.Checked
                    'pPNRev.fldSealTest = chkSealTest.Checked

                    If (CustID_In <> 0 And PlatformID_In <> 0 And LocationID_In <> 0) Then
                        '....PNR_CustInfo Table
                        Dim pProject = (From Project In mPartEntities.tblProject
                                        Where Project.fldCustID = CustID_In And
                                              Project.fldPlatformID = PlatformID_In And
                                              Project.fldLocID = LocationID_In And
                                              Project.fldPNID = PNID_In And
                                              Project.fldRevID = PNRevID_In).First()

                        pProject.fldPN_Cust = txtCustomerPN.Text.Trim()
                        mPartProject.CustInfo.PN_Cust = txtCustomerPN.Text.Trim()

                        '....Customer Table
                        Dim pCustomer = (From Cust In mPartEntities.tblCustomer
                                       Where Cust.fldID = CustID_In).First()
                        pCustomer.fldName = txtCustomer.Text
                        pCustomer.fldDimUnit = cmbUnit.Text.Trim()
                        'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text

                        '....Platform Table
                        Dim pPlatform = (From Platform In mPartEntities.tblPlatform
                                       Where Platform.fldCustID = CustID_In And
                                             Platform.fldID = PlatformID_In).First()
                        pPlatform.fldName = txtPlatform.Text

                        '....Location Table
                        Dim pLocation = (From Location In mPartEntities.tblLocation
                                        Where Location.fldCustID = CustID_In And
                                              Location.fldPlatformID = PlatformID_In And
                                              Location.fldID = LocationID_In).First()
                        pLocation.fldLoc = txtLocation.Text

                    Else

                        If (txtCustomer.Text <> "" Or txtPlatform.Text <> "" Or txtLocation.Text <> "" Or txtCustomerPN.Text <> "") Then
                            If (txtCustomer.Text = "") Then
                                MessageBox.Show("Customer can't be blank", "Customer Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtCustomer.Focus()
                                Exit Sub
                            Else

                                If (txtPlatform.Text = "") Then
                                    MessageBox.Show("Platform can't be blank", "Platform Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    txtPlatform.Focus()
                                    Exit Sub
                                Else
                                    If (txtLocation.Text = "") Then
                                        MessageBox.Show("Location can't be blank", "Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        txtLocation.Focus()
                                        Exit Sub

                                    Else

                                        If (chkNew.Checked And txtParkerPN_Part3.Text = "") Then
                                            MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            txtParkerPN_Part3.Focus()
                                            Exit Sub
                                        ElseIf (chkLegacy.Checked And txtParkerPN_Legacy.Text = "") Then
                                            MessageBox.Show("ParkerPN can't be blank", "ParkerPN Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            cmdLegacy.Focus()
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        'AES 12OCT17
                        If (txtCustomer.Text.Trim() <> "") Then

                            '....Customer
                            Dim pCustID As Integer = 0
                            Dim pCustRecCount As Integer = (From Customer In mPartEntities.tblCustomer).Count()
                            Dim pCustomerID As Integer = 0
                            If (pCustRecCount > 0) Then
                                Dim pCustName As String = txtCustomer.Text.Trim()
                                Dim pCustCount As Integer = (From Customer In mPartEntities.tblCustomer Where Customer.fldName = pCustName).Count()

                                If (pCustCount > 0) Then
                                    '....Existing Customer
                                    Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
                                                 Where Customer.fldName = pCustName).First()
                                    pCustomerID = pCustomer_Out.fldID
                                    pCustID = pCustomerID
                                Else
                                    '....New Customer
                                    Dim pCustomer_Out = (From Customer In mPartEntities.tblCustomer
                                                 Order By Customer.fldID Descending).First()

                                    pCustomerID = pCustomer_Out.fldID
                                    Dim pCustomer As New tblCustomer
                                    pCustID = pCustomerID + 1
                                    pCustomer.fldID = pCustID
                                    pCustomer.fldName = txtCustomer.Text.Trim()
                                    pCustomer.fldDimUnit = cmbUnit.Text.Trim()
                                    'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text
                                    mPartEntities.AddTotblCustomer(pCustomer)
                                    mPartEntities.SaveChanges()
                                End If

                            Else
                                '....New Customer
                                pCustomerID = 1
                                Dim pCustomer As New tblCustomer
                                pCustID = 1
                                pCustomer.fldID = pCustID
                                pCustomer.fldName = txtCustomer.Text.Trim()
                                pCustomer.fldDimUnit = cmbUnit.Text.Trim()
                                'pCustomer.fldCulturalFormat = cmbCulturalFormat.Text
                                mPartEntities.AddTotblCustomer(pCustomer)
                                mPartEntities.SaveChanges()
                            End If

                            mCustomerID = pCustID

                            If (txtPlatform.Text.Trim() <> "") Then
                                '....Platform 
                                Dim pPlatformName As String = txtPlatform.Text.Trim()

                                Dim pPlatformRecCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID).Count()
                                Dim pPlatformID As Integer = 0

                                If (pPlatformRecCount > 0) Then

                                    Dim pPlatCount As Integer = (From Platform In mPartEntities.tblPlatform Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).Count()

                                    If (pPlatCount > 0) Then
                                        '....Existing Platform
                                        Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
                                                    Where Platform.fldCustID = pCustID And Platform.fldName = pPlatformName).First()
                                        pPlatformID = pPlatform_Out.fldID
                                    Else
                                        '....New Platform
                                        Dim pPlatform_Out = (From Platform In mPartEntities.tblPlatform
                                                   Where Platform.fldCustID = pCustID Order By Platform.fldID Descending).First()

                                        pPlatformID = pPlatform_Out.fldID + 1
                                        Dim pPlatform As New tblPlatform
                                        pPlatform.fldCustID = pCustID
                                        pPlatform.fldID = pPlatformID
                                        pPlatform.fldName = pPlatformName
                                        mPartEntities.AddTotblPlatform(pPlatform)
                                        mPartEntities.SaveChanges()

                                    End If
                                Else
                                    '....New Customer & Platform
                                    Dim pPlatform As New tblPlatform
                                    pPlatform.fldCustID = pCustID
                                    pPlatform.fldID = 1
                                    pPlatform.fldName = pPlatformName
                                    mPartEntities.AddTotblPlatform(pPlatform)
                                    mPartEntities.SaveChanges()
                                    pPlatformID = 1
                                End If

                                mPlatformID = pPlatformID

                                If (txtLocation.Text.Trim() <> "") Then
                                    '....Location
                                    Dim pLocationName As String = txtLocation.Text.Trim()

                                    Dim pLocationRecCount As Integer = (From Location In mPartEntities.tblLocation Where Location.fldCustID = pCustID And Location.fldPlatformID = pPlatformID).Count()
                                    Dim pLocationID As Integer = 0

                                    If (pLocationRecCount > 0) Then

                                        Dim pLocCount As Integer = (From Loc In mPartEntities.tblLocation Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).Count()

                                        If pLocCount > 0 Then
                                            '....Existing Location
                                            Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
                                                        Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID And Loc.fldLoc = pLocationName).First()
                                            pLocationID = pLoc_Out.fldID
                                        Else
                                            '....New Location
                                            Dim pLoc_Out = (From Loc In mPartEntities.tblLocation
                                                       Where Loc.fldCustID = pCustID And Loc.fldPlatformID = pPlatformID Order By Loc.fldID Descending).First()
                                            pLocationID = pLoc_Out.fldID + 1

                                            Dim pLocation As New tblLocation
                                            pLocation.fldCustID = pCustID
                                            pLocation.fldPlatformID = pPlatformID
                                            pLocation.fldID = pLocationID
                                            pLocation.fldLoc = pLocationName
                                            mPartEntities.AddTotblLocation(pLocation)
                                            mPartEntities.SaveChanges()
                                        End If

                                    Else
                                        '....New Platform & Location
                                        Dim pLocation As New tblLocation
                                        pLocation.fldCustID = pCustID
                                        pLocation.fldPlatformID = pPlatformID
                                        pLocationID = 1
                                        pLocation.fldID = pLocationID
                                        pLocation.fldLoc = pLocationName
                                        mPartEntities.AddTotblLocation(pLocation)
                                        mPartEntities.SaveChanges()

                                    End If

                                    mLocationID = pLocationID
                                End If

                            End If

                        End If

                        mPNID = PNID_In
                        'mParkerPN_Rev = pRevision
                        mRevID = PNRevID_In

                        Dim pPNR_CustInfoID As Integer = 0

                        '....Customer Exists
                        If (mCustomerID <> 0 And mPlatformID <> 0 And mLocationID <> 0) Then

                            Dim pProject As New tblProject
                            pProject.fldCustID = mCustomerID
                            pProject.fldPlatformID = mPlatformID
                            pProject.fldLocID = mLocationID
                            pProject.fldPNID = mPNID
                            pProject.fldRevID = mRevID
                            pProject.fldPN_Cust = txtCustomerPN.Text
                            mPartProject.CustInfo.PN_Cust = txtCustomerPN.Text
                            pProject.fldPN_Cust_Rev = txtCustomerPN_Rev.Text.Trim()
                            mPartProject.CustInfo.PN_Cust_Rev = txtCustomerPN_Rev.Text.Trim()
                            'pProject.fldSealIPE = False
                            'pProject.fldSealTest = False
                            'pProject.fldSealProcess = False
                            pProject.fldDateCreated = DateTime.Now()
                            pProject.fldDateLastModified = DateTime.Now()

                            Dim pProjectRecCount As Integer = (From pRec In mPartEntities.tblProject
                                                     Where pRec.fldCustID = mCustomerID And
                                                           pRec.fldPlatformID = mPlatformID And
                                                           pRec.fldLocID = mLocationID And
                                                           pRec.fldPNID = mPNID And
                                                           pRec.fldRevID = mRevID Select pRec).Count()

                            If (pProjectRecCount = 0) Then

                                Dim pPNR_RecCount As Integer = (From Project In mPartEntities.tblProject).Count()
                                Dim pPNR_ProjectID As Integer = 0
                                If (pPNR_RecCount > 0) Then
                                    Dim pProject_Out = (From Project In mPartEntities.tblProject
                                                   Order By Project.fldID Descending).First()

                                    pPNR_ProjectID = pProject_Out.fldID
                                End If

                                pProject.fldID = pPNR_ProjectID + 1

                                mPartEntities.AddTotblProject(pProject)
                                mPartEntities.SaveChanges()

                                pPNR_CustInfoID = pPNR_ProjectID + 1

                            Else
                                MessageBox.Show("Record already exists.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If

                        End If


                    End If

                    mPartEntities.SaveChanges()

                End If

                mblnEdit = False    'AES 13OCT17
                InitializeControl(False)
                'PopulateTreeView()
                'trvProjects.ExpandAll()
                'SelectTreeNode()
                DisplayData()       'AES 25APR18

            End If

        End If

    End Sub


    Private Sub DeleteRecords(ByVal CustID_In As Integer, ByVal PlatformID_In As Integer, _
                              ByVal LocationID_In As Integer, ByVal PNID_In As Integer, ByVal PNRevID_In As Integer)
        '===========================================================================================================

        Try
            If (trvProjects.Nodes.Count > 0) Then

                If (mProjectView) Then
                    If (trvProjects.SelectedNode.Level = 4) Then
                        'Dim pProjectID As Integer = mProject.GetID()

                        Dim pProject = (From Project In mPartEntities.tblProject
                                           Where Project.fldCustID = CustID_In And
                                           Project.fldPlatformID = PlatformID_In And
                                           Project.fldLocID = LocationID_In And
                                           Project.fldPNID = PNID_In And
                                           Project.fldRevID = PNRevID_In).First()



                        'Dim pProjectID As Integer = mProject.GetID()

                        ''Dim pIPEProject As New SealIPE.clsProject()
                        ''pIPEProject.DeleteProject(PNID_In, PNRevID_In, pProjectID)

                        mPartEntities.DeleteObject(pProject)
                        mPartEntities.SaveChanges()

                        'mProject.PN_Rev_ID = 0
                        'mProject.Project_ID = 0

                    ElseIf (trvProjects.SelectedNode.Level = 3) Then

                        'Dim pProjectID As Integer = mProject.GetID()

                        Dim pProject = (From Project In mPartEntities.tblProject
                                            Where Project.fldCustID = CustID_In And
                                            Project.fldPlatformID = PlatformID_In And
                                            Project.fldLocID = LocationID_In And
                                            Project.fldPNID = PNID_In Select Project).ToList()

                        For i As Integer = 0 To pProject.Count() - 1
                            mPartEntities.DeleteObject(pProject(i))
                        Next

                        mPartEntities.SaveChanges()



                        ''Dim pIPEProject As New SealIPE.clsProject()
                        ''pIPEProject.DeleteProject(PNID_In, PNRevID_In, pProjectID)

                        'mProject.PN_ID = 0
                        'mProject.PN_Rev_ID = 0
                        'mProject.Project_ID = 0

                    ElseIf (trvProjects.SelectedNode.Level = 2) Then
                        Dim pLocation = (From Location In mPartEntities.tblLocation
                                            Where Location.fldCustID = CustID_In And
                                                  Location.fldPlatformID = PlatformID_In And
                                                  Location.fldID = LocationID_In).First()

                        mPartEntities.DeleteObject(pLocation)
                        mPartEntities.SaveChanges()
                        'mProject.Location_ID = 0
                        'mProject.PN_ID = 0
                        'mProject.PN_Rev_ID = 0
                        'mProject.Project_ID = 0
                        'mProject.PN_Cust = ""

                    ElseIf (trvProjects.SelectedNode.Level = 1) Then
                        Dim pPlatform = (From Platform In mPartEntities.tblPlatform
                                            Where Platform.fldCustID = CustID_In And
                                                  Platform.fldID = PlatformID_In).First()

                        mPartEntities.DeleteObject(pPlatform)
                        mPartEntities.SaveChanges()
                        'mProject.Platform_ID = 0
                        'mProject.Location_ID = 0
                        'mProject.PN_ID = 0
                        'mProject.PN_Rev_ID = 0
                        'mProject.Project_ID = 0

                    ElseIf (trvProjects.SelectedNode.Level = 0) Then
                        Dim pCustomer = (From Cust In mPartEntities.tblCustomer
                                            Where Cust.fldID = CustID_In).First()

                        mPartEntities.DeleteObject(pCustomer)
                        mPartEntities.SaveChanges()
                        'mProject.Customer_ID = 0
                        'mProject.Platform_ID = 0
                        'mProject.Location_ID = 0
                        'mProject.PN_ID = 0
                        'mProject.PN_Rev_ID = 0
                        'mProject.PN_Cust = ""
                        'mProject.Project_ID = 0

                    End If

                ElseIf (mPNView) Then

                    If (trvProjects.SelectedNode.Level = 1) Then

                        Dim pPN = (From PN In mPartEntities.tblPN
                                            Where PN.fldID = PNID_In).First()

                        mPartEntities.DeleteObject(pPN)
                        mPartEntities.SaveChanges()

                        'mProject.PN_ID = 0
                        'mProject.PN_Rev_ID = 0
                        'mProject.Project_ID = 0
                        'mProject.Customer_ID = 0
                        'mProject.Platform_ID = 0
                        'mProject.Location_ID = 0

                        '....SealTestProject
                        Dim pSealTestEntities As New SealTestDBEntities()
                        Dim pTestProject = (From TestProject In pSealTestEntities.tblTestProject
                                            Where TestProject.fldPNID = PNID_In).First()
                        pSealTestEntities.DeleteObject(pTestProject)
                        pSealTestEntities.SaveChanges()

                        '....SealIPEProject
                        Dim pSealIPEEntities As New SealIPEDBEntities()
                        Dim pIPEProject = (From IPEProject In pSealIPEEntities.tblIPEProject
                                            Where IPEProject.fldPNID = PNID_In).First()
                        pSealIPEEntities.DeleteObject(pIPEProject)
                        pSealIPEEntities.SaveChanges()

                    ElseIf (trvProjects.SelectedNode.Level = 2) Then

                        'AES 13OCT17
                        Dim pRevCount As Integer = (From Rev In mPartEntities.tblRev
                                            Where Rev.fldPNID = PNID_In And
                                            Rev.fldID = PNRevID_In).Count()

                        If (pRevCount > 0) Then
                            Dim pRev = (From Rev In mPartEntities.tblRev
                                        Where Rev.fldPNID = PNID_In And
                                                Rev.fldID = PNRevID_In).First()
                            mPartEntities.DeleteObject(pRev)
                            mPartEntities.SaveChanges()

                            'mProject.PN_ID = 0
                            'mProject.PN_Rev_ID = 0
                            'mProject.Project_ID = 0
                            'mProject.Customer_ID = 0
                            'mProject.Platform_ID = 0
                            'mProject.Location_ID = 0
                        End If

                        '....SealProcess
                        Dim pSealProcessEntities As New SealProcessDBEntities()
                        Dim pPartProjectID As Integer = gPartProject.Project_ID

                        Dim pProcessProjectCount As Integer = (From ProcessProject In pSealProcessEntities.tblProcessProject
                                                               Where ProcessProject.fldPartProjectID = pPartProjectID).Count()

                        If (pProcessProjectCount > 0) Then
                            Dim pProcessProject = (From ProcessProject In pSealProcessEntities.tblProcessProject
                                                   Where ProcessProject.fldPartProjectID = pPartProjectID).First()
                            pSealProcessEntities.DeleteObject(pProcessProject)
                            pSealProcessEntities.SaveChanges()
                        End If

                        '....SealTestProject
                        Dim pSealTestEntities As New SealTestDBEntities()

                        Dim pTestProjectCount As Integer = (From TestProject In pSealTestEntities.tblTestProject
                                            Where TestProject.fldPNID = PNID_In And
                                            TestProject.fldRevID = PNRevID_In).Count()

                        If (pTestProjectCount > 0) Then
                            Dim pTestProject = (From TestProject In pSealTestEntities.tblTestProject
                                               Where TestProject.fldPNID = PNID_In And
                                               TestProject.fldRevID = PNRevID_In).First()
                            pSealTestEntities.DeleteObject(pTestProject)
                            pSealTestEntities.SaveChanges()
                        End If


                        '....SealIPEProject
                        Dim pSealIPEEntities As New SealIPEDBEntities()
                        Dim pIPEProjectCount As Integer = (From IPEProject In pSealIPEEntities.tblIPEProject
                                            Where IPEProject.fldPNID = PNID_In And
                                            IPEProject.fldRevID = PNRevID_In).Count()

                        If (pIPEProjectCount > 0) Then
                            Dim pIPEProject = (From IPEProject In pSealIPEEntities.tblIPEProject
                                                Where IPEProject.fldPNID = PNID_In And
                                                IPEProject.fldRevID = PNRevID_In).First()
                            pSealIPEEntities.DeleteObject(pIPEProject)
                            pSealIPEEntities.SaveChanges()
                        End If

                    End If

                End If

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region


#Region "TEXT BOX EVENT ROUTINES:"

    Private Sub txtBox_TextChanged(sender As System.Object, e As System.EventArgs) _
                                   Handles txtCustomer.TextChanged, txtPlatform.TextChanged,
                                   txtCustomerPN.TextChanged, txtLocation.TextChanged, txtPN_PH_Rev.TextChanged
        '===================================================================================

        Dim pTxtBox As TextBox = CType(sender, TextBox)

        Select Case pTxtBox.Name

            Case "txtCustomer"
                '----------------
                mPartProject.CustInfo.CustName = pTxtBox.Text

            Case "txtPlatform"
                '-----------------
                mPartProject.CustInfo.PlatName = pTxtBox.Text

            Case "txtCustomerPN"
                '--------------------
                mPartProject.CustInfo.PN_Cust = pTxtBox.Text

            Case "txtLocation"
                '------------------
                mPartProject.CustInfo.LocName = pTxtBox.Text

                'Case "txtPN_PH_Rev"
                '    '----------------------
                '    mPartProject.PNR. = pTxtBox.Text

        End Select

    End Sub

    Private Sub txtParkerPN_Part3_MaskInputRejected(sender As System.Object,
                                                    e As System.Windows.Forms.MaskInputRejectedEventArgs) _
                                                    Handles txtParkerPN_Part3.MaskInputRejected
        '===================================================================================================
        'Dim pParkerPN As String = txtParkerPN_Part1.Text + "-" + cmbParkerPN_Part2.Text + txtParkerPN_Part3.Text
        'mParkerPN = pParkerPN
    End Sub

#End Region


#Region "TREE VIEW CONTROL EVENT ROUTINES:"

    Private Sub trvCustomers_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles trvProjects.AfterSelect
        '====================================================================================================================================
        If (mProjectView) Then

            ''    Dim pPartDB As New SealPartDBEntities()
            ''    If (trvProjects.SelectedNode.Level = 4) Then
            ''        Dim pCustName As String = e.Node.Parent.Parent.Parent.Parent.Text
            ''        Dim pPlatformName As String = e.Node.Parent.Parent.Parent.Text
            ''        Dim pLocationName As String = e.Node.Parent.Parent.Text
            ''        Dim pParkerPN As String = e.Node.Parent.Text
            ''        Dim pRev As String = e.Node.Text

            ''        txtCustomer.Text = pCustName
            ''        txtPlatform.Text = pPlatformName
            ''        txtLocation.Text = pLocationName

            ''        txtPN_PH_Rev.Text = pRev

            ''        mCustName = pCustName
            ''        mPlatformName = pPlatformName
            ''        mLocation = pLocationName
            ''        mParkerPN = pParkerPN
            ''        mParkerPN_Rev = pRev

            ''        Dim pCustomerID As Integer = mPartProject.GetCustID(pCustName)
            ''        Dim pPlatformID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatformName)
            ''        Dim pLocationID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatformID, pLocationName)
            ''        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)
            ''        Dim pRevID As Integer = mPartProject.GetRevID(pPNID, pRev)

            ''        Dim pQryPN = (From pRec In pPartDB.tblPN
            ''                              Where pRec.fldID = pPNID Select pRec).First()

            ''        If (pQryPN.fldCurrent <> "") Then
            ''            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
            ''            cmbParkerPN_Part2.Text = pParkerPN_Prefix
            ''            Dim pParkerPN_No As String = pParkerPN.Substring(5)
            ''            txtParkerPN_Part3.Text = pParkerPN_No
            ''            txtParkerPN_Legacy.Text = ""
            ''            chkNew.Checked = True
            ''        Else
            ''            txtParkerPN_Legacy.Text = pParkerPN
            ''            cmbParkerPN_Part2.Text = ""
            ''            txtParkerPN_Part3.Text = ""
            ''            chkLegacy.Checked = True
            ''        End If

            ''        'AES 21JUL17
            ''        ''If (Not IsDBNull(pQryPN.fldGeomTemplate) And Not IsNothing(pQryPN.fldGeomTemplate)) Then
            ''        ''    chkGeomTemplate.Checked = pQryPN.fldGeomTemplate
            ''        ''Else
            ''        ''    chkGeomTemplate.Checked = False
            ''        ''End If


            ''        'Dim pPartEntities As New SealPartDBEntities()
            ''        Dim pQry = (From pRec In pPartDB.tblCustomer
            ''                                    Where pRec.fldID = pCustomerID Select pRec).First()


            ''        mCustomerID = pCustomerID
            ''        mPlatformID = pPlatformID
            ''        mLocationID = pLocationID
            ''        mPNID = pPNID
            ''        mRevID = pRevID
            ''        'mPartProject.Customer.PN_Cust = txtCustomerPN.Text.Trim()

            ''        Dim pProjectID As Integer = mPartProject.GetProjectID()
            ''        mPartProject.Project_ID = pProjectID


            ''        Dim pQry1 = (From pRec In pPartDB.tblProject
            ''                                    Where pRec.fldID = pProjectID Select pRec).First()

            ''        Dim pPNCustName As String = pQry1.fldPN_Cust
            ''        txtCustomerPN.Text = pPNCustName
            ''        mPartProject.Customer.PN_Cust = txtCustomerPN.Text.Trim()

            ''        Dim pUnit As String = "English"
            ''        pUnit = pQry.fldDimUnit

            ''        If (pUnit.Trim() = "English") Then
            ''            cmbUnit.SelectedIndex = 0

            ''        Else
            ''            cmbUnit.SelectedIndex = 1
            ''        End If

            ''        Dim pQryRev = (From pRec In pPartDB.tblRev
            ''                                 Where pRec.fldPNID = pPNID And pRec.fldID = pRevID Select pRec).First()

            ''        If (Not IsDBNull(pQryRev.fldSealIPE) And Not IsNothing(pQryRev.fldSealIPE)) Then
            ''            chkSealIPE.Checked = pQryRev.fldSealIPE
            ''        Else
            ''            chkSealIPE.Checked = False
            ''        End If

            ''        If (Not IsDBNull(pQryRev.fldSealTest) And Not IsNothing(pQryRev.fldSealTest)) Then
            ''            chkSealTest.Checked = pQryRev.fldSealTest
            ''        Else
            ''            chkSealTest.Checked = False
            ''        End If

            ''        ''Dim pCustomerID As Integer
            ''        ''Dim pUnit As String = "English"

            ''        ''pCustomerID = mPartProject.GetCustID(pCustName)

            ''        ''Dim pPartEntities As New SealPartDBEntities()
            ''        ''Dim pQry = (From pRec In pPartEntities.tblCustomer
            ''        ''                            Where pRec.fldID = pCustomerID Select pRec).First()

            ''        ''pUnit = pQry.fldDimUnit

            ''        ''If (pUnit.Trim() = "English") Then
            ''        ''    cmbUnit.SelectedIndex = 0

            ''        ''Else
            ''        ''    cmbUnit.SelectedIndex = 1
            ''        ''End If

            ''        ''Dim pPlatformID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatformName)
            ''        ''Dim pLocationID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatformID, pLocationName)
            ''        ''Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)
            ''        ''Dim pRevID As Integer = mPartProject.GetRevID(pPNID, pRev)

            ''        ''mCustomerID = pCustomerID
            ''        ''mPlatformID = pPlatformID
            ''        ''mLocationID = pLocationID
            ''        ''mPNID = pPNID
            ''        ''mRevID = pRevID
            ''        ''mPartProject.PN_Cust = txtCustomerPN.Text.Trim()

            ''        'Dim pProjectID As Integer = mPartProject.GetID()

            ''        ''mPartProject.Project_ID = pProjectID


            ''        ''Dim pQry1 = (From pRec In pPartEntities.tblProject
            ''        ''                            Where pRec.fldID = pProjectID Select pRec).First()

            ''        ''Dim pPNCustName As String = pQry1.fldPN_Cust
            ''        ''txtCustomerPN.Text = pPNCustName

            ''        ' ''AES 23MAR17
            ''        '' ''If (Not IsDBNull(pQry1.fldSealIPEDesign) And Not IsNothing(pQry1.fldSealIPEDesign)) Then
            ''        '' ''    Dim pIPEDesign As Boolean = pQry1.fldSealIPEDesign
            ''        '' ''    If (pIPEDesign) Then
            ''        '' ''        optYes.Checked = True
            ''        '' ''    Else
            ''        '' ''        optNo.Checked = True
            ''        '' ''    End If
            ''        '' ''Else
            ''        '' ''    optYes.Checked = True

            ''        '' ''End If

            ''        ' ''AES 21MAR17
            ''        ''Dim pQry2 = (From pRec In pPartEntities.tblPN
            ''        ''                           Where pRec.fldID = pPNID Select pRec).First()

            ''        ''Dim pNo As String = ""
            ''        ''Dim pNo_Legacy As String = ""
            ''        ''If (Not IsDBNull(pQry2.fldNew) And Not IsNothing(pQry2.fldNew)) Then
            ''        ''    pParkerPN = pQry2.fldNew
            ''        ''    Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
            ''        ''    cmbParkerPN_Part2.Text = pParkerPN_Prefix
            ''        ''    Dim pParkerPN_No As String = pParkerPN.Substring(5)
            ''        ''    txtParkerPN_Part3.Text = pParkerPN_No
            ''        ''    chkNew.Checked = True
            ''        ''Else
            ''        ''    chkNew.Checked = False
            ''        ''End If

            ''        ''If (Not IsDBNull(pQry2.fldLegacy) And Not IsNothing(pQry2.fldLegacy)) Then
            ''        ''    txtParkerPN_Legacy.Text = pQry2.fldLegacy
            ''        ''    chkLegacy.Checked = False   ''chkLegacy.Checked = True  AES 26APR17
            ''        ''Else
            ''        ''    txtParkerPN_Legacy.Text = ""
            ''        ''    chkLegacy.Checked = False
            ''        ''End If

            ''        ''If (Not IsDBNull(pQry2.fldGeomTemplate) And Not IsNothing(pQry2.fldGeomTemplate)) Then
            ''        ''    chkGeomTemplate.Checked = pQry2.fldGeomTemplate
            ''        ''Else
            ''        ''    chkGeomTemplate.Checked = False
            ''        ''End If

            ''        '' ''If (Not IsDBNull(pQry2.fldSealIPE) And Not IsNothing(pQry2.fldSealIPE)) Then
            ''        '' ''    chkSealIPE.Checked = pQry2.fldSealIPE
            ''        '' ''Else
            ''        '' ''    chkSealIPE.Checked = False
            ''        '' ''End If

            ''        '' ''If (Not IsDBNull(pQry2.fldSealTest) And Not IsNothing(pQry2.fldSealTest)) Then
            ''        '' ''    chkSealTest.Checked = pQry2.fldSealTest
            ''        '' ''Else
            ''        '' ''    chkSealTest.Checked = False
            ''        '' ''End If

            ''        ' ''AES 21MAR17
            ''        ''Dim pQry3 = (From pRec In pPartEntities.tblRev
            ''        ''                         Where pRec.fldPNID = pPNID And
            ''        ''                               pRec.fldID = pRevID Select pRec).First()

            ''        ''Dim pRevVal As String = ""
            ''        ''Dim pRev_Legacy As String = ""
            ''        ''If (Not IsDBNull(pQry3.fldNew) And Not IsNothing(pQry3.fldNew)) Then
            ''        ''    pRev = pQry3.fldNew
            ''        ''    txtPN_PH_Rev.Text = pRev
            ''        ''Else
            ''        ''    txtPN_PH_Rev.Text = ""
            ''        ''End If
            ''        ''If (Not IsDBNull(pQry3.fldLegacy) And Not IsNothing(pQry3.fldLegacy)) Then
            ''        ''    pRev = pQry3.fldLegacy
            ''        ''    txtParkerPNLegacy_Rev.Text = pRev
            ''        ''Else
            ''        ''    txtParkerPNLegacy_Rev.Text = ""
            ''        ''End If


            ''    ElseIf (trvProjects.SelectedNode.Level = 3) Then

            ''        Dim pCustName As String = e.Node.Parent.Parent.Parent.Text
            ''        Dim pPlatformName As String = e.Node.Parent.Parent.Text
            ''        Dim pLocationName As String = e.Node.Parent.Text
            ''        Dim pParkerPN As String = e.Node.Text

            ''        'Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
            ''        'cmbParkerPN_Part2.Text = pParkerPN_Prefix
            ''        'Dim pParkerPN_No As String = pParkerPN.Substring(5)
            ''        'txtParkerPN_Part3.Text = pParkerPN_No

            ''        txtCustomer.Text = pCustName
            ''        txtPlatform.Text = pPlatformName
            ''        txtLocation.Text = pLocationName

            ''        mCustName = pCustName
            ''        mPlatformName = pPlatformName
            ''        mLocation = pLocationName
            ''        mParkerPN = pParkerPN

            ''        Dim pCustomerID As Integer
            ''        Dim pUnit As String = "English"

            ''        pCustomerID = mPartProject.GetCustID(pCustName)

            ''        Dim pPartEntities As New SealPartDBEntities()
            ''        Dim pQry = (From pRec In pPartEntities.tblCustomer
            ''                                    Where pRec.fldID = pCustomerID Select pRec).First()

            ''        pUnit = pQry.fldDimUnit

            ''        If (pUnit.Trim() = "English") Then
            ''            cmbUnit.SelectedIndex = 0
            ''        Else
            ''            cmbUnit.SelectedIndex = 1
            ''        End If

            ''        Dim pPlatformID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatformName)
            ''        Dim pLocationID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatformID, pLocationName)
            ''        Dim pPNID As Integer = mPartProject.GetPNID(pParkerPN)


            ''        mCustomerID = pCustomerID
            ''        mPlatformID = pPlatformID
            ''        mLocationID = pLocationID
            ''        mPNID = pPNID
            ''        mRevID = 0
            ''        mPartProject.Project_ID = 0
            ''        mPartProject.Customer.PN_Cust = ""

            ''        txtCustomerPN.Text = ""
            ''        txtPN_PH_Rev.Text = ""

            ''        Dim pQryPN = (From pRec In pPartDB.tblPN
            ''                            Where pRec.fldID = pPNID Select pRec).First()

            ''        If (pQryPN.fldCurrent <> "") Then
            ''            Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
            ''            cmbParkerPN_Part2.Text = pParkerPN_Prefix
            ''            Dim pParkerPN_No As String = pParkerPN.Substring(5)
            ''            txtParkerPN_Part3.Text = pParkerPN_No
            ''            txtParkerPN_Legacy.Text = ""
            ''            chkNew.Checked = True
            ''        Else
            ''            txtParkerPN_Legacy.Text = pParkerPN
            ''            cmbParkerPN_Part2.Text = ""
            ''            txtParkerPN_Part3.Text = ""
            ''            chkLegacy.Checked = True
            ''        End If

            ''        'AES 21JUL17
            ''        ''If (Not IsDBNull(pQryPN.fldGeomTemplate) And Not IsNothing(pQryPN.fldGeomTemplate)) Then
            ''        ''    chkGeomTemplate.Checked = pQryPN.fldGeomTemplate
            ''        ''Else
            ''        ''    chkGeomTemplate.Checked = False
            ''        ''End If               

            ''    ElseIf (trvProjects.SelectedNode.Level = 2) Then
            ''        Dim pCustName As String = e.Node.Parent.Parent.Text
            ''        Dim pPlatformName As String = e.Node.Parent.Text
            ''        Dim pLocationName As String = e.Node.Text

            ''        txtCustomer.Text = pCustName
            ''        txtPlatform.Text = pPlatformName
            ''        txtLocation.Text = pLocationName

            ''        mCustName = pCustName
            ''        mPlatformName = pPlatformName
            ''        mLocation = pLocationName

            ''        Dim pCustomerID As Integer
            ''        Dim pUnit As String = "English"

            ''        pCustomerID = mPartProject.GetCustID(pCustName)

            ''        Dim pPartEntities As New SealPartDBEntities()
            ''        Dim pQry = (From pRec In pPartEntities.tblCustomer
            ''                                    Where pRec.fldID = pCustomerID Select pRec).First()

            ''        pUnit = pQry.fldDimUnit

            ''        If (pUnit.Trim() = "English") Then
            ''            cmbUnit.SelectedIndex = 0
            ''        Else
            ''            cmbUnit.SelectedIndex = 1
            ''        End If

            ''        Dim pPlatformID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatformName)
            ''        Dim pLocationID As Integer = mPartProject.GetLocationID(pCustomerID, pPlatformID, pLocationName)

            ''        mCustomerID = pCustomerID
            ''        mPlatformID = pPlatformID
            ''        mLocationID = pLocationID
            ''        mPNID = 0
            ''        mRevID = 0
            ''        mPartProject.Project_ID = 0
            ''        mPartProject.Customer.PN_Cust = ""

            ''        mCustPN = ""
            ''        mParkerPN = ""
            ''        mParkerPN_Rev = ""

            ''        txtCustomerPN.Text = ""
            ''        txtParkerPN_Part3.Text = ""
            ''        txtPN_PH_Rev.Text = ""
            ''        txtParkerPN_Legacy.Text = ""
            ''        txtParkerPNLegacy_Rev.Text = ""

            ''    ElseIf (trvProjects.SelectedNode.Level = 1) Then

            ''        Dim pCustomerID As Integer
            ''        Dim pUnit As String = "English"
            ''        Dim pCustName As String = e.Node.Parent.Text

            ''        pCustomerID = mPartProject.GetCustID(pCustName)

            ''        Dim pPartEntities As New SealPartDBEntities()
            ''        Dim pQry = (From pRec In pPartEntities.tblCustomer
            ''                                    Where pRec.fldID = pCustomerID Select pRec).First()

            ''        pUnit = pQry.fldDimUnit

            ''        If (pUnit.Trim() = "English") Then
            ''            cmbUnit.SelectedIndex = 0
            ''        Else
            ''            cmbUnit.SelectedIndex = 1
            ''        End If

            ''        Dim pPlatformName As String = e.Node.Text
            ''        Dim pPlatformID As Integer = mPartProject.GetPlatformID(pCustomerID, pPlatformName)

            ''        mCustomerID = pCustomerID
            ''        mPlatformID = pPlatformID
            ''        mLocationID = 0
            ''        mPNID = 0
            ''        mRevID = 0
            ''        mPartProject.Project_ID = 0

            ''        mCustName = pCustName
            ''        mPlatformName = pPlatformName
            ''        mLocation = ""
            ''        mCustPN = ""
            ''        mParkerPN = ""
            ''        mParkerPN_Rev = ""

            ''        txtCustomer.Text = pCustName
            ''        txtPlatform.Text = pPlatformName
            ''        txtLocation.Text = ""
            ''        txtCustomerPN.Text = ""
            ''        txtParkerPN_Part3.Text = ""
            ''        txtPN_PH_Rev.Text = ""
            ''        txtParkerPN_Legacy.Text = ""
            ''        txtParkerPNLegacy_Rev.Text = ""


            ''    ElseIf (trvProjects.SelectedNode.Level = 0) Then

            ''        Dim pCustomerID As Integer
            ''        Dim pUnit As String = "English"
            ''        Dim pCustName As String = e.Node.Text

            ''        pCustomerID = mPartProject.GetCustID(pCustName)

            ''        Dim pPartEntities As New SealPartDBEntities()

            ''        Dim pQry = (From pRec In pPartEntities.tblCustomer
            ''                                     Where pRec.fldID = pCustomerID Select pRec).First()

            ''        pUnit = pQry.fldDimUnit

            ''        If (pUnit.Trim() = "English") Then
            ''            cmbUnit.SelectedIndex = 0
            ''        Else
            ''            cmbUnit.SelectedIndex = 1
            ''        End If

            ''        mCustomerID = pCustomerID
            ''        mPlatformID = 0
            ''        mLocationID = 0
            ''        mPNID = 0
            ''        mRevID = 0
            ''        mPartProject.Project_ID = 0

            ''        mCustName = pCustName
            ''        mPlatformName = ""
            ''        mLocation = ""
            ''        mCustPN = ""
            ''        mParkerPN = ""
            ''        mParkerPN_Rev = ""

            ''        txtCustomer.Text = pCustName
            ''        txtPlatform.Text = ""
            ''        txtLocation.Text = ""
            ''        txtCustomerPN.Text = ""
            ''        txtParkerPN_Part3.Text = ""
            ''        txtPN_PH_Rev.Text = ""
            ''        txtParkerPN_Legacy.Text = ""
            ''        txtParkerPNLegacy_Rev.Text = ""

            ''    End If

        ElseIf (mPNView) Then

            If (mblnEdit Or mblnAdd) Then
                mblnEdit = False
                mblnAdd = False
                InitializeControl(False)
            End If

            If (trvProjects.SelectedNode.Level = 2) Then

                cmdHardware.Enabled = True
                cmdSealProcess.Enabled = True

                Dim pPartEntities As New SealPartDBEntities()

                Dim pPNType As String = e.Node.Parent.Parent.Text
                Dim pPN As String = e.Node.Parent.Text
                Dim pRev As String = e.Node.Text

                mPN = pPN
                mPN_Rev = pRev
                'mCurrent.Rev = pRev

                Dim pPNID As Integer = Convert.ToInt64(e.Node.Parent.Tag)
                Dim pRevID As Integer = Convert.ToInt64(e.Node.Tag)

                mPNID = pPNID
                mRevID = pRevID

                If (pPNType = "New") Then
                    mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.None

                ElseIf (pPNType = "Legacy-Catalogued") Then
                    mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued

                ElseIf (pPNType = "Legacy-Other") Then
                    mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Other

                End If


                Dim pQry2 = (From pRec In pPartEntities.tblPN
                                           Where pRec.fldID = pPNID Select pRec).First()

                Dim pNo As String = ""
                Dim pNo_Legacy As String = ""
                If (Not IsDBNull(pQry2.fldCurrent) And Not IsNothing(pQry2.fldCurrent)) Then
                    pPN = pQry2.fldCurrent
                    If (pPN <> "") Then
                        Dim pParkerPN_Prefix As String = pPN.Substring(3, 2)
                        cmbParkerPN_Part2.Text = pParkerPN_Prefix
                        Dim pParkerPN_No As String = pPN.Substring(5)
                        txtParkerPN_Part3.Text = pParkerPN_No
                        chkNew.Checked = True
                        mPartProject.PNR.Current_Exists = True
                        mPartProject.PNR.Current_Val = pParkerPN_No
                        'mPartProject.PNR.SealTypeNo = pParkerPN_Prefix
                        mPartProject.PNR.Current_TypeNo = pParkerPN_Prefix
                    Else
                        mPartProject.PNR.Current_Exists = False
                        chkNew.Checked = False
                        cmbParkerPN_Part2.Text = ""
                        txtParkerPN_Part3.Text = ""
                    End If

                Else
                    mPartProject.PNR.Current_Exists = False
                    chkNew.Checked = False
                End If

                If (Not IsDBNull(pQry2.fldLegacy) And Not IsNothing(pQry2.fldLegacy)) Then
                    txtParkerPN_Legacy.Text = pQry2.fldLegacy
                    If (pQry2.fldLegacy <> "") Then
                        mPartProject.PNR.Legacy_Exists = True
                        mPartProject.PNR.Legacy_Val = txtParkerPN_Legacy.Text
                        chkLegacy.Checked = True    'AES 02MAY17

                    Else
                        mPartProject.PNR.Legacy_Exists = False
                        chkLegacy.Checked = False
                    End If

                    'Dim pVal As Integer = 0
                    'pVal = pQry2.fldLegacyType
                    'If (pVal = 0) Then
                    '    mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
                    'ElseIf (pVal = 1) Then
                    '    mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Other
                    'End If


                Else
                    txtParkerPN_Legacy.Text = ""
                    mPartProject.PNR.Legacy_Exists = False
                    chkLegacy.Checked = False
                End If

                '....Parent Current
                If (Not IsDBNull(pQry2.fldParentCurrent) And Not IsNothing(pQry2.fldParentCurrent)) Then
                    Dim pVal As String = pQry2.fldParentCurrent
                    If (pVal.Contains("NH-")) Then
                        Dim pValPart2 As String = pVal.Substring(3, 2)
                        Dim pValPart3 As String = pVal.Substring(5)
                        cmbParentCur_Part2.Text = pValPart2
                        txtParentCur_Part3.Text = pValPart3
                        mPartProject.PNR.ParentCurrent_Exists = True
                    Else
                        chkPNNew_Parent.Checked = False
                        cmbParentCur_Part2.Text = ""
                        txtParentCur_Part3.Text = ""
                        mPartProject.PNR.ParentCurrent_Exists = False
                    End If

                    mPartProject.PNR.ParentCurrent_Val = txtParentCur_Part3.Text
                Else
                    chkPNNew_Parent.Checked = False
                    cmbParentCur_Part2.Text = ""
                    mPartProject.PNR.ParentCurrent_Exists = False
                    mPartProject.PNR.ParentCurrent_Val = ""
                End If

                '....ParentRev
                If (Not IsDBNull(pQry2.fldParentCurrentRev) And Not IsNothing(pQry2.fldParentCurrentRev)) Then
                    txtParentCur_Rev.Text = pQry2.fldParentCurrentRev
                    mPartProject.PNR.ParentCurrent_Rev = txtParentCur_Rev.Text
                Else
                    txtParentCur_Rev.Text = ""
                    mPartProject.PNR.ParentCurrent_Rev = ""
                End If

                '....Parent Legacy
                If (Not IsDBNull(pQry2.fldParentLegacy) And Not IsNothing(pQry2.fldParentLegacy)) Then
                    Dim pVal As String = pQry2.fldParentLegacy
                    If (pVal <> "") Then
                        chkPNLegacy_Parent.Checked = True
                        txtPNLegacy_Parent.Text = pVal
                        mPartProject.PNR.ParentLegacy_Exists = True
                    Else
                        chkPNLegacy_Parent.Checked = False
                        txtPNLegacy_Parent.Text = ""
                        mPartProject.PNR.ParentLegacy_Exists = False
                    End If

                    mPartProject.PNR.ParentLegacy_Val = txtPNLegacy_Parent.Text
                Else
                    chkPNLegacy_Parent.Checked = False
                    txtPNLegacy_Parent.Text = ""
                    mPartProject.PNR.ParentLegacy_Exists = False
                    mPartProject.PNR.ParentLegacy_Val = ""
                End If

                '....Parent LegacyRev
                If (Not IsDBNull(pQry2.fldParentLegacyRev) And Not IsNothing(pQry2.fldParentLegacyRev)) Then
                    txtPNParentLegacy_Rev.Text = pQry2.fldParentLegacyRev
                    mPartProject.PNR.ParentLegacy_Rev = txtPNParentLegacy_Rev.Text
                Else
                    txtPNParentLegacy_Rev.Text = ""
                    mPartProject.PNR.ParentLegacy_Rev = ""
                End If

                '....Ref. Part No
                '======================
                If (Not IsDBNull(pQry2.fldRefDimCurrentExists) And Not IsNothing(pQry2.fldRefDimCurrentExists)) Then
                    If (pQry2.fldRefDimCurrentExists) Then
                        Dim pParkerPN As String = pQry2.fldRefDimCurrent
                        Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                        cmbRefPNNewDim_Part2.Text = pParkerPN_Prefix
                        Dim pParkerPN_No As String = pParkerPN.Substring(5)
                        txtRefPNNewDim_Part3.Text = pParkerPN_No
                        chkRefDimNew_Exists.Checked = True
                        txtRefPNNewDim_Rev.Text = pQry2.fldRefDimCurrentRev

                        mPartProject.PNR.RefDimCurrent_Exists = True
                        mPartProject.PNR.RefDimCurrent_Val = pParkerPN
                        mPartProject.PNR.RefDimCurrent_Rev = txtRefPNNewDim_Rev.Text

                    Else
                        chkRefDimNew_Exists.Checked = False
                        txtRefPNNewDim_Part3.Text = ""
                        txtRefPNNewDim_Rev.Text = ""
                        mPartProject.PNR.RefDimCurrent_Exists = False
                        mPartProject.PNR.RefDimCurrent_Val = ""
                        mPartProject.PNR.RefDimCurrent_Rev = ""

                    End If
                Else
                    chkRefDimNew_Exists.Checked = False
                    txtRefPNNewDim_Part3.Text = ""
                    txtRefPNNewDim_Rev.Text = ""
                    mPartProject.PNR.RefDimCurrent_Exists = False
                    mPartProject.PNR.RefDimCurrent_Val = ""
                    mPartProject.PNR.RefDimCurrent_Rev = ""
                End If


                If (Not IsDBNull(pQry2.fldRefNotesCurrentExists) And Not IsNothing(pQry2.fldRefNotesCurrentExists)) Then
                    If (pQry2.fldRefNotesCurrentExists) Then
                        Dim pParkerPN As String = pQry2.fldRefNotesCurrent
                        Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                        cmbRefNotesNewPN_Part2.Text = pParkerPN_Prefix
                        Dim pParkerPN_No As String = pParkerPN.Substring(5)
                        txtRefPNNotes_Part3.Text = pParkerPN_No
                        chkRefDimNotes_Exists.Checked = True
                        txtRefPNNewNotes_Rev.Text = pQry2.fldRefNotesCurrentRev

                        mPartProject.PNR.RefNotesCurrent_Exists = True
                        mPartProject.PNR.RefNotesCurrent_Val = pParkerPN
                        mPartProject.PNR.RefNotesCurrent_Rev = txtRefPNNewNotes_Rev.Text
                    Else
                        chkRefDimNotes_Exists.Checked = False
                        txtRefPNNotes_Part3.Text = ""
                        txtRefPNNewNotes_Rev.Text = ""

                        mPartProject.PNR.RefNotesCurrent_Exists = False
                        mPartProject.PNR.RefNotesCurrent_Val = ""
                        mPartProject.PNR.RefNotesCurrent_Rev = ""
                    End If
                Else
                    chkRefDimNotes_Exists.Checked = False
                    txtRefPNNotes_Part3.Text = ""
                    txtRefPNNewNotes_Rev.Text = ""

                    mPartProject.PNR.RefNotesCurrent_Exists = False
                    mPartProject.PNR.RefNotesCurrent_Val = ""
                    mPartProject.PNR.RefNotesCurrent_Rev = ""
                End If


                If (Not IsDBNull(pQry2.fldRefDimLegacyExists) And Not IsNothing(pQry2.fldRefDimLegacyExists)) Then
                    If (pQry2.fldRefDimLegacyExists) Then
                        txtRefPNNewDim_Legacy.Text = pQry2.fldRefDimLegacy
                        chkRefDimLegacy_Exists.Checked = True
                        txtRefPNLegacyDim_Rev.Text = pQry2.fldRefDimLegacyRev

                        mPartProject.PNR.RefDimLegacy_Exists = True
                        mPartProject.PNR.RefDimLegacy_Val = txtRefPNNewDim_Legacy.Text
                        mPartProject.PNR.RefDimLegacy_Rev = txtRefPNLegacyDim_Rev.Text
                    Else
                        txtRefPNNewDim_Legacy.Text = ""
                        chkRefDimLegacy_Exists.Checked = False
                        txtRefPNLegacyDim_Rev.Text = ""

                        mPartProject.PNR.RefDimLegacy_Exists = False
                        mPartProject.PNR.RefDimLegacy_Val = ""
                        mPartProject.PNR.RefDimLegacy_Rev = ""
                    End If
                Else
                    txtRefPNNewDim_Legacy.Text = ""
                    chkRefDimLegacy_Exists.Checked = False
                    txtRefPNLegacyDim_Rev.Text = ""
                End If


                If (Not IsDBNull(pQry2.fldRefNotesLegacyExists) And Not IsNothing(pQry2.fldRefNotesLegacyExists)) Then
                    If (pQry2.fldRefNotesLegacyExists) Then
                        txtRefPNNewNotes_Legacy.Text = pQry2.fldRefNotesLegacy
                        chkRefNotesLegacy_Exists.Checked = True
                        txtRefPNLegacyNotes_Rev.Text = pQry2.fldRefNotesLegacyRev

                        mPartProject.PNR.RefNotesLegacy_Exists = True
                        mPartProject.PNR.RefNotesLegacy_Val = txtRefPNNewNotes_Legacy.Text
                        mPartProject.PNR.RefNotesLegacy_Rev = txtRefPNLegacyNotes_Rev.Text
                    Else
                        txtRefPNNewNotes_Legacy.Text = ""
                        chkRefNotesLegacy_Exists.Checked = False
                        txtRefPNLegacyNotes_Rev.Text = ""

                        mPartProject.PNR.RefNotesLegacy_Exists = False
                        mPartProject.PNR.RefNotesLegacy_Val = ""
                        mPartProject.PNR.RefNotesLegacy_Rev = ""
                    End If
                Else
                    txtRefPNNewNotes_Legacy.Text = ""
                    chkRefNotesLegacy_Exists.Checked = False
                    txtRefPNLegacyNotes_Rev.Text = ""
                End If

                '======================


                'AES 21JUL17
                ' ''....Geom Template
                ''If (Not IsDBNull(pQry2.fldGeomTemplate) And Not IsNothing(pQry2.fldGeomTemplate)) Then
                ''    chkGeomTemplate.Checked = pQry2.fldGeomTemplate
                ''Else
                ''    chkGeomTemplate.Checked = False
                ''End If

                Dim pQry3 = (From pRec In pPartEntities.tblRev
                                         Where pRec.fldPNID = pPNID And
                                               pRec.fldID = pRevID Select pRec).First()

                Dim pRevVal As String = ""
                Dim pRev_Legacy As String = ""
                If (Not IsDBNull(pQry3.fldCurrent) And Not IsNothing(pQry3.fldCurrent)) Then
                    pRev = pQry3.fldCurrent
                    If (pRev <> "") Then
                        txtPN_PH_Rev.Text = pRev
                    Else
                        txtPN_PH_Rev.Text = ""
                    End If

                Else
                    txtPN_PH_Rev.Text = ""
                End If

                If (Not IsDBNull(pQry3.fldLegacy) And Not IsNothing(pQry3.fldLegacy)) Then
                    pRev = pQry3.fldLegacy
                    txtParkerPNLegacy_Rev.Text = pRev
                Else
                    txtParkerPNLegacy_Rev.Text = ""
                End If

                'If (Not IsDBNull(pQry3.fldSealIPE) And Not IsNothing(pQry3.fldSealIPE)) Then
                '    chkSealIPE.Checked = pQry3.fldSealIPE
                'Else
                '    chkSealIPE.Checked = False
                'End If

                'If (Not IsDBNull(pQry3.fldSealTest) And Not IsNothing(pQry3.fldSealTest)) Then
                '    chkSealTest.Checked = pQry3.fldSealTest
                'Else
                '    chkSealTest.Checked = False
                'End If

                '...tblProject
                Dim pRecCount As Integer = (From pRec In pPartEntities.tblProject
                                        Where pRec.fldPNID = pPNID And
                                        pRec.fldRevID = pRevID Select pRec).Count()

                mPNID = pPNID
                mRevID = pRevID

                If (pRecCount > 0) Then

                    Dim pQry4 = (From pRec In pPartEntities.tblProject
                                            Where pRec.fldPNID = pPNID And
                                            pRec.fldRevID = pRevID Select pRec).First()
                    mPartProject.Project_ID = pQry4.fldID

                    If (Not IsDBNull(pQry4.fldPN_Cust) And Not IsNothing(pQry4.fldPN_Cust)) Then
                        txtCustomerPN.Text = pQry4.fldPN_Cust
                    Else
                        txtCustomerPN.Text = ""
                    End If

                    If (Not IsDBNull(pQry4.fldPN_Cust_Rev) And Not IsNothing(pQry4.fldPN_Cust_Rev)) Then
                        txtCustomerPN_Rev.Text = pQry4.fldPN_Cust_Rev
                    Else
                        txtCustomerPN_Rev.Text = ""
                    End If

                    '....Customer
                    If (Not IsDBNull(pQry4.fldCustID) And Not IsNothing(pQry4.fldCustID)) Then
                        Dim pCustID As Integer = pQry4.fldCustID
                        Dim pPlatformID As Integer = pQry4.fldPlatformID
                        Dim pLocID As Integer = pQry4.fldLocID

                        'txtCustomer.Text = mPartProject.Customer.GetCustomer(pCustID)

                        '...tblCustomer
                        Dim pQry5 = (From pRec In pPartEntities.tblCustomer
                                                Where pRec.fldID = pCustID Select pRec).First()

                        If (Not IsDBNull(pQry5.fldName) And Not IsNothing(pQry5.fldName)) Then
                            txtCustomer.Text = pQry5.fldName
                        End If

                        '....Unit
                        If (Not IsDBNull(pQry5.fldDimUnit) And Not IsNothing(pQry5.fldDimUnit)) Then
                            cmbUnit.Text = pQry5.fldDimUnit.Trim()
                        Else
                            cmbUnit.SelectedIndex = 0
                        End If
                        mPartProject.PNR.UnitSystem = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eDimUnit), cmbUnit.Text), clsPartProject.clsPNR.eDimUnit)

                        ' ''....Cultural Format
                        ''If (Not IsDBNull(pQry5.fldCulturalFormat) And Not IsNothing(pQry5.fldCulturalFormat)) Then
                        ''    cmbCulturalFormat.Text = pQry5.fldCulturalFormat
                        ''Else
                        ''    cmbCulturalFormat.SelectedIndex = 0

                        ''End If

                        '....tblPlatform
                        Dim pQry6 = (From pRec In pPartEntities.tblPlatform
                                                Where pRec.fldCustID = pCustID And
                                                pRec.fldID = pPlatformID Select pRec).First()

                        If (Not IsDBNull(pQry6.fldName) And Not IsNothing(pQry6.fldName)) Then
                            txtPlatform.Text = pQry6.fldName
                        Else
                            txtPlatform.Text = ""
                        End If

                        '....tblLocation
                        Dim pQry7 = (From pRec In pPartEntities.tblLocation
                                                Where pRec.fldCustID = pCustID And
                                                pRec.fldPlatformID = pPlatformID And
                                                pRec.fldID = pLocID Select pRec).First()

                        If (Not IsDBNull(pQry7.fldLoc) And Not IsNothing(pQry7.fldLoc)) Then
                            txtLocation.Text = pQry7.fldLoc
                        Else
                            txtLocation.Text = ""
                        End If

                        mCustomerID = pCustID
                        mPlatformID = pPlatformID
                        mLocationID = pLocID

                        mPartProject.CustInfo.CustName = txtCustomer.Text.Trim()
                        mPartProject.CustInfo.PlatName = txtPlatform.Text.Trim()
                        mPartProject.CustInfo.LocName = txtLocation.Text.Trim()
                        mPartProject.CustInfo.PN_Cust = txtCustomerPN.Text.Trim()
                        mPartProject.CustInfo.PN_Cust_Rev = txtCustomerPN_Rev.Text.Trim()

                        'Dim pProjectID As Integer = mPartProject.GetID()

                        'mPartProject.Project_ID = pProjectID

                    Else
                        txtCustomer.Text = ""
                        cmbUnit.SelectedIndex = 0
                        ''cmbCulturalFormat.SelectedIndex = 0
                        txtPlatform.Text = ""
                        txtLocation.Text = ""

                        mCustomerID = 0
                        mPlatformID = 0
                        mLocationID = 0
                        mPartProject.CustInfo.PN_Cust = txtCustomerPN.Text.Trim()
                        mPartProject.CustInfo.PN_Cust_Rev = txtCustomerPN_Rev.Text.Trim()
                    End If
                Else
                    txtCustomer.Text = ""
                    cmbUnit.SelectedIndex = 0
                    ''cmbCulturalFormat.SelectedIndex = 0
                    txtPlatform.Text = ""
                    txtLocation.Text = ""
                    txtCustomerPN.Text = ""
                    txtCustomerPN_Rev.Text = ""

                    mCustomerID = 0
                    mPlatformID = 0
                    mLocationID = 0
                    mPartProject.CustInfo.PN_Cust = txtCustomerPN.Text.Trim()
                    mPartProject.CustInfo.PN_Cust_Rev = txtCustomerPN_Rev.Text.Trim()

                End If

            ElseIf (trvProjects.SelectedNode.Level = 1) Then

                cmdHardware.Enabled = False
                cmdSealProcess.Enabled = False
                Dim pPartEntities As New SealPartDBEntities()
                Dim pPNType As String = e.Node.Parent.Text
                Dim pPN As String = e.Node.Text

                mPN = pPN

                Dim pPNID As Integer = Convert.ToInt64(e.Node.Tag)

                'Dim pPNID As Integer = mPartProject.GetPNID(pPN)

                Dim pQry2 = (From pRec In pPartEntities.tblPN
                                          Where pRec.fldID = pPNID Select pRec).First()

                mPNID = pPNID
                mRevID = 0
                mCustomerID = 0
                mPlatformID = 0
                mLocationID = 0
                mPartProject.CustInfo.PN_Cust = txtCustomerPN.Text.Trim()
                mPartProject.CustInfo.PN_Cust_Rev = txtCustomerPN_Rev.Text.Trim()

                Dim pNo As String = ""
                Dim pNo_Legacy As String = ""
                If (Not IsDBNull(pQry2.fldCurrent) And Not IsNothing(pQry2.fldCurrent)) Then
                    pPN = pQry2.fldCurrent
                    If (pPN <> "") Then
                        Dim pParkerPN_Prefix As String = pPN.Substring(3, 2)
                        cmbParkerPN_Part2.Text = pParkerPN_Prefix
                        Dim pParkerPN_No As String = pPN.Substring(5)
                        txtParkerPN_Part3.Text = pParkerPN_No
                        chkNew.Checked = True
                    Else
                        chkNew.Checked = False
                        cmbParkerPN_Part2.Text = ""
                        txtParkerPN_Part3.Text = ""
                    End If

                Else
                    chkNew.Checked = False
                End If

                If (Not IsDBNull(pQry2.fldLegacy) And Not IsNothing(pQry2.fldLegacy)) Then
                    txtParkerPN_Legacy.Text = pQry2.fldLegacy
                    If (pQry2.fldLegacy <> "") Then
                        'chkLegacy.Checked = False ''chkLegacy.Checked = True        AES 26APR17
                        chkLegacy.Checked = True    'AES 02MAY17
                    Else
                        chkLegacy.Checked = False
                    End If

                    Dim pVal As Integer = 0
                    pVal = pQry2.fldLegacyType
                    If (pVal = 0) Then
                        mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Catalogued
                    ElseIf (pVal = 1) Then
                        mPartProject.PNR.Legacy_Type = clsPartProject.clsPNR.eLegacyType.Other
                    End If

                Else
                    txtParkerPN_Legacy.Text = ""
                    chkLegacy.Checked = False
                End If

                '....Parent Current
                If (Not IsDBNull(pQry2.fldParentCurrent) And Not IsNothing(pQry2.fldParentCurrent)) Then
                    Dim pVal As String = pQry2.fldParentCurrent
                    If (pVal.Contains("NH-")) Then
                        chkPNNew_Parent.Checked = True
                        Dim pValPart2 As String = pVal.Substring(3, 2)
                        Dim pValPart3 As String = pVal.Substring(5)
                        cmbParentCur_Part2.Text = pValPart2
                        txtParentCur_Part3.Text = pValPart3
                        mPartProject.PNR.ParentCurrent_Exists = True
                    Else
                        chkPNNew_Parent.Checked = False
                        cmbParentCur_Part2.Text = ""
                        txtParentCur_Part3.Text = ""
                        mPartProject.PNR.ParentCurrent_Exists = False
                    End If

                    mPartProject.PNR.ParentCurrent_Val = txtParentCur_Part3.Text
                Else
                    chkPNNew_Parent.Checked = False
                    cmbParentCur_Part2.Text = ""
                    mPartProject.PNR.ParentCurrent_Exists = False
                    mPartProject.PNR.ParentCurrent_Val = ""
                End If

                '....ParentRev
                If (Not IsDBNull(pQry2.fldParentCurrentRev) And Not IsNothing(pQry2.fldParentCurrentRev)) Then
                    txtParentCur_Rev.Text = pQry2.fldParentCurrentRev
                    mPartProject.PNR.ParentCurrent_Rev = txtParentCur_Rev.Text
                Else
                    txtParentCur_Rev.Text = ""
                    mPartProject.PNR.ParentCurrent_Rev = ""
                End If

                '....Parent Legacy
                If (Not IsDBNull(pQry2.fldParentLegacy) And Not IsNothing(pQry2.fldParentLegacy)) Then
                    Dim pVal As String = pQry2.fldParentLegacy
                    If (pVal <> "") Then
                        chkPNLegacy_Parent.Checked = True
                        txtPNLegacy_Parent.Text = pVal
                        mPartProject.PNR.ParentLegacy_Exists = True
                    Else
                        chkPNLegacy_Parent.Checked = False
                        txtPNLegacy_Parent.Text = ""
                        mPartProject.PNR.ParentLegacy_Exists = False
                    End If

                    mPartProject.PNR.ParentLegacy_Val = txtPNLegacy_Parent.Text
                Else
                    chkPNLegacy_Parent.Checked = False
                    txtPNLegacy_Parent.Text = ""
                    mPartProject.PNR.ParentLegacy_Exists = False
                    mPartProject.PNR.ParentLegacy_Val = ""
                End If

                '....Parent LegacyRev
                If (Not IsDBNull(pQry2.fldParentLegacyRev) And Not IsNothing(pQry2.fldParentLegacyRev)) Then
                    txtPNParentLegacy_Rev.Text = pQry2.fldParentLegacyRev
                    mPartProject.PNR.ParentLegacy_Rev = txtPNParentLegacy_Rev.Text
                Else
                    txtPNParentLegacy_Rev.Text = ""
                    mPartProject.PNR.ParentLegacy_Rev = ""
                End If

                '....Ref. Part No
                '======================
                If (Not IsDBNull(pQry2.fldRefDimCurrentExists) And Not IsNothing(pQry2.fldRefDimCurrentExists)) Then
                    If (pQry2.fldRefDimCurrentExists) Then
                        Dim pParkerPN As String = pQry2.fldRefDimCurrent
                        Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                        cmbRefPNNewDim_Part2.Text = pParkerPN_Prefix
                        Dim pParkerPN_No As String = pParkerPN.Substring(5)
                        txtRefPNNewDim_Part3.Text = pParkerPN_No
                        chkRefDimNew_Exists.Checked = True
                        txtRefPNNewDim_Rev.Text = pQry2.fldRefDimCurrentRev

                        mPartProject.PNR.RefDimCurrent_Exists = True
                        mPartProject.PNR.RefDimCurrent_Val = pParkerPN
                        mPartProject.PNR.RefDimCurrent_Rev = txtRefPNNewDim_Rev.Text

                    Else
                        chkRefDimNew_Exists.Checked = False
                        txtRefPNNewDim_Part3.Text = ""
                        txtRefPNNewDim_Rev.Text = ""
                        mPartProject.PNR.RefDimCurrent_Exists = False
                        mPartProject.PNR.RefDimCurrent_Val = ""
                        mPartProject.PNR.RefDimCurrent_Rev = ""

                    End If
                Else
                    chkRefDimNew_Exists.Checked = False
                    txtRefPNNewDim_Part3.Text = ""
                    txtRefPNNewDim_Rev.Text = ""
                    mPartProject.PNR.RefDimCurrent_Exists = False
                    mPartProject.PNR.RefDimCurrent_Val = ""
                    mPartProject.PNR.RefDimCurrent_Rev = ""
                End If


                If (Not IsDBNull(pQry2.fldRefNotesCurrentExists) And Not IsNothing(pQry2.fldRefNotesCurrentExists)) Then
                    If (pQry2.fldRefNotesCurrentExists) Then
                        Dim pParkerPN As String = pQry2.fldRefNotesCurrent
                        Dim pParkerPN_Prefix As String = pParkerPN.Substring(3, 2)
                        cmbRefNotesNewPN_Part2.Text = pParkerPN_Prefix
                        Dim pParkerPN_No As String = pParkerPN.Substring(5)
                        txtRefPNNotes_Part3.Text = pParkerPN_No
                        chkRefDimNotes_Exists.Checked = True
                        txtRefPNNewNotes_Rev.Text = pQry2.fldRefNotesCurrentRev

                        mPartProject.PNR.RefNotesCurrent_Exists = True
                        mPartProject.PNR.RefNotesCurrent_Val = pParkerPN
                        mPartProject.PNR.RefNotesCurrent_Rev = txtRefPNNewNotes_Rev.Text
                    Else
                        chkRefDimNotes_Exists.Checked = False
                        txtRefPNNotes_Part3.Text = ""
                        txtRefPNNewNotes_Rev.Text = ""

                        mPartProject.PNR.RefNotesCurrent_Exists = False
                        mPartProject.PNR.RefNotesCurrent_Val = ""
                        mPartProject.PNR.RefNotesCurrent_Rev = ""
                    End If
                Else
                    chkRefDimNotes_Exists.Checked = False
                    txtRefPNNotes_Part3.Text = ""
                    txtRefPNNewNotes_Rev.Text = ""

                    mPartProject.PNR.RefNotesCurrent_Exists = False
                    mPartProject.PNR.RefNotesCurrent_Val = ""
                    mPartProject.PNR.RefNotesCurrent_Rev = ""
                End If


                If (Not IsDBNull(pQry2.fldRefDimLegacyExists) And Not IsNothing(pQry2.fldRefDimLegacyExists)) Then
                    If (pQry2.fldRefDimLegacyExists) Then
                        txtRefPNNewDim_Legacy.Text = pQry2.fldRefDimLegacy
                        chkRefDimLegacy_Exists.Checked = True
                        txtRefPNLegacyDim_Rev.Text = pQry2.fldRefDimLegacyRev

                        mPartProject.PNR.RefDimLegacy_Exists = True
                        mPartProject.PNR.RefDimLegacy_Val = txtRefPNNewDim_Legacy.Text
                        mPartProject.PNR.RefDimLegacy_Rev = txtRefPNLegacyDim_Rev.Text
                    Else
                        txtRefPNNewDim_Legacy.Text = ""
                        chkRefDimLegacy_Exists.Checked = False
                        txtRefPNLegacyDim_Rev.Text = ""

                        mPartProject.PNR.RefDimLegacy_Exists = False
                        mPartProject.PNR.RefDimLegacy_Val = ""
                        mPartProject.PNR.RefDimLegacy_Rev = ""
                    End If
                Else
                    txtRefPNNewDim_Legacy.Text = ""
                    chkRefDimLegacy_Exists.Checked = False
                    txtRefPNLegacyDim_Rev.Text = ""
                End If


                If (Not IsDBNull(pQry2.fldRefNotesLegacyExists) And Not IsNothing(pQry2.fldRefNotesLegacyExists)) Then
                    If (pQry2.fldRefNotesLegacyExists) Then
                        txtRefPNNewNotes_Legacy.Text = pQry2.fldRefNotesLegacy
                        chkRefNotesLegacy_Exists.Checked = True
                        txtRefPNLegacyNotes_Rev.Text = pQry2.fldRefNotesLegacyRev

                        mPartProject.PNR.RefNotesLegacy_Exists = True
                        mPartProject.PNR.RefNotesLegacy_Val = txtRefPNNewNotes_Legacy.Text
                        mPartProject.PNR.RefNotesLegacy_Rev = txtRefPNLegacyNotes_Rev.Text
                    Else
                        txtRefPNNewNotes_Legacy.Text = ""
                        chkRefNotesLegacy_Exists.Checked = False
                        txtRefPNLegacyNotes_Rev.Text = ""

                        mPartProject.PNR.RefNotesLegacy_Exists = False
                        mPartProject.PNR.RefNotesLegacy_Val = ""
                        mPartProject.PNR.RefNotesLegacy_Rev = ""
                    End If
                Else
                    txtRefPNNewNotes_Legacy.Text = ""
                    chkRefNotesLegacy_Exists.Checked = False
                    txtRefPNLegacyNotes_Rev.Text = ""
                End If


                '======================

                'AES 21JUL17
                ' ''....Geom Template
                ''If (Not IsDBNull(pQry2.fldGeomTemplate) And Not IsNothing(pQry2.fldGeomTemplate)) Then
                ''    chkGeomTemplate.Checked = pQry2.fldGeomTemplate
                ''Else
                ''    chkGeomTemplate.Checked = False
                ''End If


                txtCustomer.Text = ""
                cmbUnit.SelectedIndex = 0
                'cmbCulturalFormat.SelectedIndex = 0
                txtPlatform.Text = ""
                txtLocation.Text = ""
                txtCustomerPN.Text = ""
                txtCustomerPN_Rev.Text = ""
                txtPN_PH_Rev.Text = ""
                txtParkerPNLegacy_Rev.Text = ""
                chkSealIPE.Checked = False
                chkSealIPE.Checked = False

            ElseIf (trvProjects.SelectedNode.Level = 0) Then

                cmdHardware.Enabled = False
                cmdSealProcess.Enabled = False

            End If
        End If

    End Sub

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub InitializeControl(ByVal Status_In As Boolean)
        '====================================================

        txtCustomer.ReadOnly = Not Status_In
        cmbUnit.Enabled = Status_In
        txtPlatform.ReadOnly = Not Status_In
        txtCustomerPN.ReadOnly = Not Status_In
        txtCustomerPN_Rev.ReadOnly = Not Status_In
        txtParkerPN_Part3.ReadOnly = Not Status_In
        txtPN_PH_Rev.ReadOnly = Not Status_In
        txtLocation.ReadOnly = Not Status_In

        chkNew.Enabled = Status_In
        chkLegacy.Enabled = Status_In
        cmdLegacy.Enabled = Status_In

        chkLegacy.Checked = Status_In

        txtParkerPN_Legacy.ReadOnly = Not Status_In
        txtParkerPNLegacy_Rev.ReadOnly = Not Status_In
        'grpIPEDesign.Enabled = False ''Status_In   

        chkPNNew_Parent.Enabled = Status_In
        'txtPNNew_Parent.Enabled = Status_In

        txtParentCur_Part1.Enabled = Status_In
        cmbParentCur_Part2.Enabled = Status_In
        txtParentCur_Part3.Enabled = Not Status_In
       

        txtParentCur_Rev.Enabled = Status_In

        chkPNLegacy_Parent.Enabled = Status_In
        txtPNLegacy_Parent.Enabled = Status_In
        txtPNParentLegacy_Rev.Enabled = Status_In

        ''cmbCulturalFormat.Enabled = Status_In

        chkRefDimNew_Exists.Enabled = Status_In
        cmbRefPNNewDim_Part2.Enabled = Status_In
        txtRefPNNewDim_Part3.Enabled = Status_In
        txtRefPNNewDim_Rev.Enabled = Status_In
        chkRefDimNotes_Exists.Enabled = Status_In
        cmbRefNotesNewPN_Part2.Enabled = Status_In
        txtRefPNNotes_Part3.Enabled = Status_In
        txtRefPNNewNotes_Rev.Enabled = Status_In
        chkRefDimLegacy_Exists.Enabled = Status_In
        txtRefPNNewDim_Legacy.Enabled = Status_In
        txtRefPNLegacyDim_Rev.Enabled = Status_In
        chkRefNotesLegacy_Exists.Enabled = Status_In
        txtRefPNNewNotes_Legacy.Enabled = Status_In
        txtRefPNLegacyNotes_Rev.Enabled = Status_In


        Dim pColor As Color = Color.FromArgb(240, 240, 240)

        If (Status_In) Then
            txtCustomer.BackColor = Color.White
            txtCustomer.ForeColor = Color.Black

            txtPlatform.BackColor = Color.White
            txtPlatform.ForeColor = Color.Black

            txtLocation.BackColor = Color.White
            txtLocation.ForeColor = Color.Black

            txtCustomerPN.BackColor = Color.White
            txtCustomerPN.ForeColor = Color.Black

            txtCustomerPN_Rev.BackColor = Color.White
            txtCustomerPN_Rev.ForeColor = Color.Black

            txtParkerPN_Part1.BackColor = Color.White
            txtParkerPN_Part1.ForeColor = Color.Black

            cmbParkerPN_Part2.Enabled = Status_In

            txtParkerPN_Part3.BackColor = Color.White
            txtParkerPN_Part3.ForeColor = Color.Black

            txtPN_PH_Rev.BackColor = Color.White
            txtPN_PH_Rev.ForeColor = Color.Black

            txtParkerPNLegacy_Rev.BackColor = Color.White
            txtParkerPNLegacy_Rev.ForeColor = Color.Black

            'txtPNNew_Parent.BackColor = Color.White
            'txtPNNew_Parent.ForeColor = Color.Black

            txtParentCur_Part1.BackColor = Color.White
            txtParentCur_Part1.ForeColor = Color.Black

            cmbParentCur_Part2.Enabled = Status_In

            txtParentCur_Part3.BackColor = Color.White
            txtParentCur_Part3.ForeColor = Color.Black

            txtParentCur_Rev.BackColor = Color.White
            txtParentCur_Rev.ForeColor = Color.Black


            'AES 23MAR17
            'txtParkerPN_Legacy.BackColor = Color.White
            'txtParkerPN_Legacy.ForeColor = Color.Black

            'txtParkerPNLegacy_Rev.BackColor = Color.White
            'txtParkerPNLegacy_Rev.ForeColor = Color.Black

        Else

            txtCustomer.BackColor = pColor
            txtCustomer.ForeColor = Color.DimGray

            txtPlatform.BackColor = pColor
            txtPlatform.ForeColor = Color.DimGray

            txtLocation.BackColor = pColor
            txtLocation.ForeColor = Color.DimGray

            txtCustomerPN.BackColor = pColor
            txtCustomerPN.ForeColor = Color.DimGray

            txtCustomerPN_Rev.BackColor = pColor
            txtCustomerPN_Rev.ForeColor = Color.DimGray

            cmbParkerPN_Part2.Enabled = Status_In

            txtParkerPN_Part1.BackColor = pColor
            txtParkerPN_Part1.ForeColor = Color.DimGray

            txtParkerPN_Part3.BackColor = pColor
            txtParkerPN_Part3.ForeColor = Color.DimGray

            txtPN_PH_Rev.BackColor = pColor
            txtPN_PH_Rev.ForeColor = Color.DimGray

            txtParkerPNLegacy_Rev.BackColor = pColor
            txtParkerPNLegacy_Rev.ForeColor = Color.DimGray

            'txtPNNew_Parent.BackColor = pColor
            'txtPNNew_Parent.ForeColor = Color.DimGray

            txtParentCur_Part1.BackColor = pColor
            txtParentCur_Part1.ForeColor = Color.DimGray

            cmbParentCur_Part2.Enabled = Status_In

            txtParentCur_Part3.BackColor = pColor
            txtParentCur_Part3.ForeColor = Color.DimGray

            txtParentCur_Rev.BackColor = pColor
            txtParentCur_Rev.ForeColor = Color.DimGray

            'AES 23MAR17
            'txtParkerPN_Legacy.BackColor = pColor
            'txtParkerPN_Legacy.ForeColor = Color.DimGray

            'txtParkerPNLegacy_Rev.BackColor = pColor
            'txtParkerPNLegacy_Rev.ForeColor = Color.DimGray

        End If

    End Sub


    Private Sub SetDefaultData()
        '=======================

        If (mProjectView) Then

            If mCustomerID = 0 Then

                Dim pRecCount As Integer = (From it In mPartEntities.tblCustomer
                                                Order By it.fldID Ascending Select it).Count()
                If pRecCount > 0 Then
                    Dim query = (From it In mPartEntities.tblCustomer
                                    Order By it.fldID Ascending Select it).First()
                    mCustomerID = query.fldID

                    Dim pRecCountPlatform As Integer = (From it In mPartEntities.tblPlatform
                                                            Where it.fldCustID = mCustomerID
                                                            Order By it.fldID Ascending Select it).Count()
                    If pRecCountPlatform > 0 Then
                        Dim pQuery = (From it In mPartEntities.tblPlatform
                                        Where it.fldCustID = mCustomerID
                                        Order By it.fldID Ascending Select it).First()
                        mPlatformID = pQuery.fldID

                        Dim pRecCountLocation As Integer = (From it In mPartEntities.tblLocation
                                                            Where it.fldCustID = mCustomerID And
                                                                  it.fldPlatformID = mPlatformID
                                                            Order By it.fldID Ascending Select it).Count()
                        If pRecCountLocation > 0 Then
                            Dim pQuery1 = (From it In mPartEntities.tblLocation
                                       Where it.fldCustID = mCustomerID And
                                             it.fldPlatformID = mPlatformID
                                       Order By it.fldID Ascending Select it).First()

                            mLocationID = pQuery1.fldID

                            Dim pRecCountPN As Integer = (From it In mPartEntities.tblProject
                                                            Where it.fldCustID = mCustomerID And
                                                                  it.fldPlatformID = mPlatformID And
                                                                  it.fldLocID = mLocationID
                                                         Order By it.fldPNID Ascending Select it).Count()
                            If pRecCountPN > 0 Then
                                Dim pQuery2 = (From it In mPartEntities.tblProject
                                                            Where it.fldCustID = mCustomerID And
                                                                  it.fldPlatformID = mPlatformID And
                                                                  it.fldLocID = mLocationID
                                                         Order By it.fldPNID Ascending Select it).First()

                                mPNID = pQuery2.fldPNID

                                Dim pRecCountPNRev As Integer = (From it In mPartEntities.tblRev
                                                            Where it.fldPNID = mPNID
                                                         Order By it.fldID Ascending Select it).Count()

                                If pRecCountPNRev > 0 Then

                                    Dim pQuery3 = (From it In mPartEntities.tblRev
                                                            Where it.fldPNID = mPNID
                                                         Order By it.fldID Ascending Select it).First()
                                    mRevID = pQuery3.fldID

                                End If

                            End If

                        End If
                    End If
                End If

            ElseIf mPlatformID = 0 Then
                Dim pRecCount As Integer = (From it In mPartEntities.tblPlatform
                                                Where it.fldCustID = mCustomerID
                                                Order By it.fldID Ascending Select it).Count()
                If pRecCount > 0 Then
                    Dim pQuery = (From it In mPartEntities.tblPlatform
                                    Where it.fldCustID = mCustomerID
                                    Order By it.fldID Ascending Select it).First()
                    mPlatformID = pQuery.fldID

                    Dim pRecCountLocation As Integer = (From it In mPartEntities.tblLocation
                                                            Where it.fldCustID = mCustomerID And
                                                                  it.fldPlatformID = mPlatformID
                                                            Order By it.fldID Ascending Select it).Count()
                    If pRecCountLocation > 0 Then
                        Dim pQuery1 = (From it In mPartEntities.tblLocation
                                   Where it.fldCustID = mCustomerID And
                                         it.fldPlatformID = mPlatformID
                                   Order By it.fldID Ascending Select it).First()

                        mLocationID = pQuery1.fldID

                        Dim pRecCountPN As Integer = (From it In mPartEntities.tblProject
                                                        Where it.fldCustID = mCustomerID And
                                                              it.fldPlatformID = mPlatformID And
                                                              it.fldLocID = mLocationID
                                                     Order By it.fldPNID Ascending Select it).Count()
                        If pRecCountPN > 0 Then
                            Dim pQuery2 = (From it In mPartEntities.tblProject
                                                        Where it.fldCustID = mCustomerID And
                                                              it.fldPlatformID = mPlatformID And
                                                              it.fldLocID = mLocationID
                                                     Order By it.fldPNID Ascending Select it).First()

                            mPNID = pQuery2.fldPNID

                            Dim pRecCountPNRev As Integer = (From it In mPartEntities.tblRev
                                                        Where it.fldPNID = mPNID
                                                     Order By it.fldID Ascending Select it).Count()

                            If pRecCountPNRev > 0 Then

                                Dim pQuery3 = (From it In mPartEntities.tblRev
                                                        Where it.fldPNID = mPNID
                                                     Order By it.fldID Ascending Select it).First()
                                mRevID = pQuery3.fldID

                            End If

                        End If

                    End If

                End If

            ElseIf mLocationID = 0 Then

                Dim pRecCountLocation As Integer = (From it In mPartEntities.tblLocation
                                                            Where it.fldCustID = mCustomerID And
                                                                  it.fldPlatformID = mPlatformID
                                                            Order By it.fldID Ascending Select it).Count()
                If pRecCountLocation > 0 Then
                    Dim pQuery1 = (From it In mPartEntities.tblLocation
                               Where it.fldCustID = mCustomerID And
                                     it.fldPlatformID = mPlatformID
                               Order By it.fldID Ascending Select it).First()

                    mLocationID = pQuery1.fldID

                    Dim pRecCountPN As Integer = (From it In mPartEntities.tblProject
                                                    Where it.fldCustID = mCustomerID And
                                                          it.fldPlatformID = mPlatformID And
                                                          it.fldLocID = mLocationID
                                                 Order By it.fldPNID Ascending Select it).Count()
                    If pRecCountPN > 0 Then
                        Dim pQuery2 = (From it In mPartEntities.tblProject
                                                    Where it.fldCustID = mCustomerID And
                                                          it.fldPlatformID = mPlatformID And
                                                          it.fldLocID = mLocationID
                                                 Order By it.fldPNID Ascending Select it).First()

                        mPNID = pQuery2.fldPNID

                        Dim pRecCountPNRev As Integer = (From it In mPartEntities.tblRev
                                                    Where it.fldPNID = mPNID
                                                 Order By it.fldID Ascending Select it).Count()

                        If pRecCountPNRev > 0 Then

                            Dim pQuery3 = (From it In mPartEntities.tblRev
                                                    Where it.fldPNID = mPNID
                                                 Order By it.fldID Ascending Select it).First()
                            mRevID = pQuery3.fldID

                        End If

                    End If

                End If

            ElseIf mPNID = 0 Then
                Dim pRecCountPN As Integer = (From it In mPartEntities.tblProject
                                                    Where it.fldCustID = mCustomerID And
                                                          it.fldPlatformID = mPlatformID And
                                                          it.fldLocID = mLocationID
                                                 Order By it.fldPNID Ascending Select it).Count()
                If pRecCountPN > 0 Then
                    Dim pQuery2 = (From it In mPartEntities.tblProject
                                                Where it.fldCustID = mCustomerID And
                                                      it.fldPlatformID = mPlatformID And
                                                      it.fldLocID = mLocationID
                                             Order By it.fldPNID Ascending Select it).First()

                    mPNID = pQuery2.fldPNID

                    Dim pRecCountPNRev As Integer = (From it In mPartEntities.tblRev
                                                Where it.fldPNID = mPNID
                                             Order By it.fldID Ascending Select it).Count()

                    If pRecCountPNRev > 0 Then

                        Dim pQuery3 = (From it In mPartEntities.tblRev
                                                Where it.fldPNID = mPNID
                                             Order By it.fldID Ascending Select it).First()
                        mRevID = pQuery3.fldID

                    End If

                End If

            ElseIf mRevID = 0 Then

                Dim pRecCountPNRev As Integer = (From it In mPartEntities.tblRev
                                                Where it.fldPNID = mPNID
                                             Order By it.fldID Ascending Select it).Count()

                If pRecCountPNRev > 0 Then

                    Dim pQuery3 = (From it In mPartEntities.tblRev
                                            Where it.fldPNID = mPNID
                                         Order By it.fldID Ascending Select it).First()
                    mRevID = pQuery3.fldID

                End If

            End If

        ElseIf (mPNView) Then

            If mPNID = 0 Then

                Dim pPNCount As Integer = (From it In mPartEntities.tblPN
                                               Order By it.fldID Ascending Select it).Count()
                If pPNCount > 0 Then
                    Dim query = (From it In mPartEntities.tblPN
                                    Order By it.fldID Ascending Select it).ToList()

                    For i As Integer = 0 To pPNCount - 1
                        mPNID = query(i).fldID

                        If mRevID = 0 Then
                            Dim pRecCountPNRev As Integer = (From it In mPartEntities.tblRev
                                        Where it.fldPNID = mPNID
                                     Order By it.fldID Ascending Select it).Count()

                            If pRecCountPNRev > 0 Then

                                Dim pQuery3 = (From it In mPartEntities.tblRev
                                               Where it.fldPNID = mPNID
                                               Order By it.fldID Descending Select it).ToList()


                                For m As Integer = 0 To pRecCountPNRev - 1
                                    mRevID = pQuery3(m).fldID

                                    Dim pRecCountPN As Integer = (From it In mPartEntities.tblProject
                                                                            Where it.fldPNID = mPNID And
                                                                                  it.fldRevID = mRevID
                                                                         Order By it.fldID Ascending Select it).Count()
                                    If pRecCountPN > 0 Then
                                        Dim pQuery2 = (From it In mPartEntities.tblProject
                                                                    Where it.fldPNID = mPNID And
                                                                          it.fldRevID = mRevID
                                                                 Order By it.fldID Ascending Select it).First()

                                        mCustomerID = pQuery2.fldCustID
                                        mPlatformID = pQuery2.fldPlatformID
                                        mLocationID = pQuery2.fldLocID

                                    End If
                                    Exit Sub
                                Next

                            End If
                        End If
                    Next

                End If
            End If

        End If

    End Sub


    Private Sub UpdateIndexField()
        '=========================     
        'Try
        '    '....PN
        '    Dim query = (From it In mPartEntities.tblPN Order By it.fldID Ascending Select it)
        '    Dim pPNList As New List(Of Integer)

        '    For Each pPN In query
        '        pPNList.Add(pPN.fldID)
        '    Next

        '    For i As Integer = 0 To pPNList.Count - 1
        '        Dim pPNID As Integer = pPNList(i)
        '        Dim pQryPN_Index = (From it In mPartEntities.tblPN_Index Where it.fldID = pPNID Select it).ToList()

        '        If (pQryPN_Index.Count = 0) Then
        '            Dim pPN_Index As New tblPN_Index
        '            pPN_Index.fldID = pPNID
        '            pPN_Index.fldIndex = pPNID

        '            mPartEntities.AddTotblPN_Index(pPN_Index)
        '            mPartEntities.SaveChanges()
        '        End If

        '        '....Rev
        '        Dim pPNRevList As New List(Of Integer)
        '        Dim queryRev = (From it In mPartEntities.tblRev Where it.fldPNID = pPNID Select it).ToList()
        '        If (queryRev.Count > 0) Then
        '            For j As Integer = 0 To queryRev.Count - 1
        '                pPNRevList.Add(queryRev(j).fldID)
        '            Next
        '            For j As Integer = 0 To pPNRevList.Count - 1
        '                Dim pPNRevID As Integer = pPNRevList(j)
        '                Dim pQryPNRev_Index = (From it In mPartEntities.tblRev_Index Where it.fldPNID = pPNID And it.fldID = pPNRevID Select it).ToList()
        '                If (pQryPNRev_Index.Count = 0) Then
        '                    Dim pPNRev_Index As New tblRev_Index
        '                    pPNRev_Index.fldPNID = pPNID
        '                    pPNRev_Index.fldID = pPNRevID
        '                    pPNRev_Index.fldIndex = pPNRevID

        '                    mPartEntities.AddTotblRev_Index(pPNRev_Index)
        '                    mPartEntities.SaveChanges()
        '                End If
        '            Next

        '        End If


        '    Next

        'Catch ex As Exception

        'End Try

       

    End Sub


    Private Sub PopulateTreeView()
        '=========================
        trvProjects.Nodes.Clear()
        Dim pPartDB As New SealPartDBEntities

        If (mProjectView) Then
            Dim pPNID As Integer
            Dim pRevID As Integer
            Dim pCustID As Integer
            Dim pPlatformID As Integer
            Dim pLocID As Integer
            Dim pID As Integer
            Dim pPN_Cust As String
            Dim pQuery = (From it In pPartDB.tblProject Select it).ToList()

            If (pQuery.Count > 0) Then

                For i As Integer = 0 To pQuery.Count - 1
                    pPNID = pQuery(i).fldPNID
                    pRevID = pQuery(i).fldRevID
                    pCustID = pQuery(i).fldCustID
                    pPlatformID = pQuery(i).fldPlatformID
                    pLocID = pQuery(i).fldLocID
                    pID = pQuery(i).fldID
                    pPN_Cust = pQuery(i).fldPN_Cust

                    mPNID = pPNID
                    mRevID = pRevID
                    mCustomerID = pCustID
                    mPlatformID = pPlatformID
                    mLocationID = pLocID
                    mPartProject.Project_ID = pID

                    Dim pQueryPN = (From it In pPartDB.tblPN Where it.fldID = pPNID And it.fldLegacyType = 0 Select it).ToList()

                    If (pQueryPN.Count > 0) Then
                        Dim pCustName As String = mPartProject.CustInfo.CustName 'mPartProject.CustApp.GetCustomer(pCustID)
                        Dim pParentNode As TreeNode
                        pParentNode = SearchTheTreeView(trvProjects, pCustName)
                        'Dim pParentNode As TreeNode
                        'pParentNode = New TreeNode(pCustName)
                        'trvProjects.Nodes.Add(pParentNode)
                        If (IsNothing(pParentNode)) Then
                            pParentNode = New TreeNode(pCustName)
                            trvProjects.Nodes.Add(pParentNode)
                        End If

                        Dim pPlatform As String = mPartProject.CustInfo.PlatName
                        Dim pChildNode As TreeNode = Nothing

                        For i1 As Integer = 0 To pParentNode.Nodes.Count - 1
                            If (pParentNode.Nodes(i1).Text = pPlatform) Then
                                pChildNode = pParentNode.Nodes(i1)
                                Exit For
                            End If
                        Next

                        If (IsNothing(pChildNode)) Then
                            pChildNode = New TreeNode()
                            pChildNode = pParentNode.Nodes.Add(pPlatform)
                        End If

                        'pChildNode = New TreeNode()
                        'pChildNode = pParentNode.Nodes.Add(pPlatform)

                        Dim pLocation As String = mPartProject.CustInfo.LocName 'mPartProject.CustApp.Location()
                        Dim pGrandChildNode As TreeNode = Nothing

                        For i1 As Integer = 0 To pChildNode.Nodes.Count - 1
                            If (pChildNode.Nodes(i1).Text = pLocation) Then
                                pGrandChildNode = pChildNode.Nodes(i1)
                                Exit For
                            End If
                        Next

                        If (IsNothing(pGrandChildNode)) Then
                            pGrandChildNode = New TreeNode()
                            pGrandChildNode = pChildNode.Nodes.Add(pLocation)
                        End If
                        'pGrandChildNode = New TreeNode()
                        'pGrandChildNode = pChildNode.Nodes.Add(pLocation)

                        Dim pPN As String = mPartProject.PNR.PN
                        Dim pGrandGrandChildNode As TreeNode = Nothing

                        For i1 As Integer = 0 To pGrandChildNode.Nodes.Count - 1
                            If (pGrandChildNode.Nodes(i1).Text = pPN) Then
                                pGrandGrandChildNode = pGrandChildNode.Nodes(i1)
                                Exit For
                            End If
                        Next

                        If (IsNothing(pGrandGrandChildNode)) Then
                            pGrandGrandChildNode = New TreeNode()
                            pGrandGrandChildNode = pGrandChildNode.Nodes.Add(pPN)
                        End If

                        'pGrandGrandChildNode = New TreeNode()
                        'pGrandGrandChildNode = pGrandChildNode.Nodes.Add(pPN)

                        Dim pPN_Rev As String = mPartProject.PNR.PN_Rev()
                        Dim pGGrandGrandChildNode As TreeNode = Nothing

                        For i1 As Integer = 0 To pGrandGrandChildNode.Nodes.Count - 1
                            If (pGrandGrandChildNode.Nodes(i1).Text = pPN_Rev) Then
                                pGGrandGrandChildNode = pGrandGrandChildNode.Nodes(i1)
                                Exit For
                            End If
                        Next

                        If (IsNothing(pGGrandGrandChildNode)) Then
                            pGGrandGrandChildNode = New TreeNode()
                            pGGrandGrandChildNode = pGrandGrandChildNode.Nodes.Add(pPN_Rev)
                        End If

                        'pGGrandGrandChildNode = New TreeNode()
                        'pGGrandGrandChildNode = pGrandGrandChildNode.Nodes.Add(pPN_Rev)
                    End If


                Next

            End If


        ElseIf (mPNView) Then

            Dim pGrandParentNode As New TreeNode
            Dim pParentNode As New TreeNode
            Dim pCurrent As Boolean = False
            Dim pLegacy_Catalogued As Boolean = False
            Dim pLegacy_Other As Boolean = False

            Dim pQryPNIndex = (From it In mPartEntities.tblPN
                                 Order By it.fldID Ascending Select it).ToList()

            For i As Integer = 0 To pQryPNIndex.Count() - 1
                Dim pPN As Integer = pQryPNIndex(i).fldID
                Dim pQryPN = (From it In mPartEntities.tblPN
                                Where it.fldID = pPN Select it).ToList()

                If (pQryPN.Count() > 0) Then
                    If (Not IsDBNull(pQryPN(0).fldCurrent) And Not IsNothing(pQryPN(0).fldCurrent)) Then
                        If (pQryPN(0).fldCurrent <> "") Then

                            If (pCurrent = False) Then
                                pCurrent = True
                                pGrandParentNode = New TreeNode("New")
                                trvProjects.Nodes.Add(pGrandParentNode)
                            Else
                                pGrandParentNode = SearchTheTreeView(trvProjects, "New")
                            End If
                            pParentNode = New TreeNode(pQryPN(0).fldCurrent)
                            pParentNode.Tag = pQryPN(0).fldID
                        Else
                            If (Not IsDBNull(pQryPN(0).fldLegacy) And Not IsNothing(pQryPN(0).fldLegacy)) Then
                                If (pQryPN(0).fldLegacyType = 0) Then
                                    If (pLegacy_Catalogued = False) Then
                                        pLegacy_Catalogued = True
                                        pGrandParentNode = New TreeNode("Legacy-Catalogued")
                                        trvProjects.Nodes.Add(pGrandParentNode)
                                    Else
                                        pGrandParentNode = SearchTheTreeView(trvProjects, "Legacy-Catalogued")
                                    End If
                                    pParentNode = New TreeNode(pQryPN(0).fldLegacy)
                                    pParentNode.Tag = pQryPN(0).fldID
                                ElseIf (pQryPN(0).fldLegacyType = 1) Then
                                    If (pLegacy_Other = False) Then
                                        pLegacy_Other = True
                                        pGrandParentNode = New TreeNode("Legacy-Other")
                                        trvProjects.Nodes.Add(pGrandParentNode)
                                    Else
                                        pGrandParentNode = SearchTheTreeView(trvProjects, "Legacy-Other")
                                    End If
                                    pParentNode = New TreeNode(pQryPN(0).fldLegacy)
                                    pParentNode.Tag = pQryPN(0).fldID

                                End If
                            End If
                        End If
                    Else
                        If (Not IsDBNull(pQryPN(0).fldLegacy) And Not IsNothing(pQryPN(0).fldLegacy)) Then
                            If (pQryPN(0).fldLegacyType = 0) Then
                                If (pLegacy_Catalogued = False) Then
                                    pLegacy_Catalogued = True
                                    pGrandParentNode = New TreeNode("Legacy-Catalogued")
                                    trvProjects.Nodes.Add(pGrandParentNode)
                                Else
                                    pGrandParentNode = SearchTheTreeView(trvProjects, "Legacy-Catalogued")
                                End If
                                pParentNode = New TreeNode(pQryPN(0).fldLegacy)
                                pParentNode.Tag = pQryPN(0).fldID
                            ElseIf (pQryPN(0).fldLegacyType = 1) Then
                                If (pLegacy_Other = False) Then
                                    pLegacy_Other = True
                                    pGrandParentNode = New TreeNode("Legacy-Other")
                                    trvProjects.Nodes.Add(pGrandParentNode)
                                Else
                                    pGrandParentNode = SearchTheTreeView(trvProjects, "Legacy-Other")
                                End If
                                pParentNode = New TreeNode(pQryPN(0).fldLegacy)
                                pParentNode.Tag = pQryPN(0).fldID

                            End If
                        End If

                    End If
                    pGrandParentNode.Nodes.Add(pParentNode)

                    Dim pPNID As Integer = pQryPN(0).fldID
                    Dim pQryPNRevIndex = (From it In mPartEntities.tblRev Where it.fldPNID = pPNID
                                 Order By it.fldID Select it).ToList()
                    If (pQryPNRevIndex.Count > 0) Then
                        For j As Integer = 0 To pQryPNRevIndex.Count() - 1
                            Dim pRevID As Integer = pQryPNRevIndex(j).fldID
                            Dim pChildNode As TreeNode
                            pChildNode = New TreeNode()

                            Dim pQryPNRev = (From it In mPartEntities.tblRev Where it.fldPNID = pPNID And it.fldID = pRevID
                                          Select it).ToList()
                            If (pQryPNRev.Count > 0) Then
                                If (pQryPNRev(0).fldCurrent <> "") Then
                                    Dim pCurRev As String = pQryPNRev(0).fldCurrent
                                    pChildNode = pParentNode.Nodes.Add(pCurRev)
                                    pChildNode.Tag = pQryPNRev(0).fldID

                                ElseIf (pQryPNRev(0).fldLegacy <> "") Then

                                    Dim pLegacyRev As String = pQryPNRev(0).fldLegacy
                                    pChildNode = pParentNode.Nodes.Add(pLegacyRev)
                                    pChildNode.Tag = pQryPNRev(0).fldID
                                End If
                            End If

                        Next
                    End If

                End If
            Next

        End If

    End Sub


    Private Sub PopulateCultureCmbBox(ByVal cmbBox_In As ComboBox)
        '==========================================================

        ''With cmbBox_In
        ''    .Items.Clear()
        ''    .Items.Add("USA")
        ''    .Items.Add("UK")
        ''    .Items.Add("Germany")
        ''    .Items.Add("France")
        ''End With

        ''With cmbBox_In

        ''    Select Case (CultureInfo.CurrentCulture.ToString())
        ''        Case "en-US"
        ''            .SelectedItem = "USA"
        ''        Case "en-GB"
        ''            .SelectedItem = "UK"
        ''        Case "de-DE"
        ''            .SelectedItem = "Germany"
        ''        Case "fr-FR"
        ''            .SelectedItem = "France"

        ''    End Select

        ''End With

        ''gPartProject.CultureName = cmbBox_In.SelectedItem

    End Sub

#End Region



    Private Sub cmdCopyFrom_Click(sender As System.Object, e As System.EventArgs)
        '==========================================================================
        'Dim pfrmUserInfo As New frmUserInfo()
        'pfrmUserInfo.ShowDialog()

        'Dim pPN As New clsPart_PN()
        ''pPN.ID = 1
        'pPN.New_TypeNo = clsPart_PN.eTypeNo.E
        'pPN.New_Val = 221
        'pPN.Legacy_Type = clsPart_PN.eLegacyType.Catalogued
        'pPN.Legacy_Val = "EEE-"

        'Dim pRev As New clsPart_PN.sRev
        ''pRev.ID = 1
        'pRev.NewVal = ""
        'pRev.Legacy = "0"
        'pPN.Rev.Add(pRev)

        ''pRev.ID = 2
        'pRev.NewVal = ""
        'pRev.Legacy = "B"
        'pPN.Rev.Add(pRev)

        ''pRev.ID = 3
        'pRev.NewVal = "0"
        'pRev.Legacy = "C"
        'pPN.Rev.Add(pRev)

        'gPart_PN.Add(pPN)

        'Dim a As String = gPart_PN(0).NewVal

    End Sub


    Private Sub cmdLegacy_Click(sender As System.Object, e As System.EventArgs) Handles cmdLegacy.Click
        '===============================================================================================
        SaveData()
        Dim pView As String = "Project"
        If (mProjectView) Then
            pView = "Project"
        ElseIf (mPNView) Then
            pView = "PN"
        End If

        Dim pfrmPN_Entry As New Part_frmLegacyPN(mPNID, pView)
        pfrmPN_Entry.ShowDialog()

    End Sub


    Private Sub cmdHardware_Click(sender As System.Object, e As System.EventArgs) Handles cmdHardware.Click
        '===================================================================================================
        SaveData()
        If (chkSealIPE.Checked) Then
            Dim pfrmHW As New Part_frmHW("Project")
            pfrmHW.ShowDialog()
        Else
            Dim pfrmHW As New Part_frmHW("PN", mPNID, mRevID)
            pfrmHW.ShowDialog()
        End If

        SelectTreeNode()
    End Sub


    Private Sub frmPartInfo_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        '======================================================================================================
        If (gIsLegacyPNActive) Then
            gIsLegacyPNActive = False

            If (gPartProject.PNR.Legacy.Exists) Then
                txtParkerPN_Legacy.Text = gPartProject.PNR.Legacy.Val
                mPartProject.PNR.Legacy_Exists = True
                mPartProject.PNR.Legacy_Type = gPartProject.PNR.Legacy.Type
                mPartProject.PNR.Legacy_Val = gPartProject.PNR.Legacy.Val
            End If
        End If

        'AES 17APR18
        If (gIsHWActive) Then
            gIsHWActive = False
            mPartProject = gPartProject.Clone()
        End If

        If (gIsProcessMainActive) Then
            gIsProcessMainActive = False
            mPartProject = gPartProject.Clone()
        End If

    End Sub

    Private Sub cmbParkerPN_Part2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                        Handles cmbParkerPN_Part2.SelectedIndexChanged
        '================================================================================================

        If (mPNView) Then
            If (cmbParkerPN_Part2.Text = "69") Then
                ''chkGeomTemplate.Checked = True
                chkSealTest.Checked = True

            ElseIf (cmbParkerPN_Part2.Text = "76") Then
                ''chkGeomTemplate.Checked = True
                chkSealTest.Checked = True

            ElseIf (cmbParkerPN_Part2.Text = "79") Then
                ''chkGeomTemplate.Checked = True
                chkSealTest.Checked = True

            ElseIf (cmbParkerPN_Part2.Text = "44") Then
                ''chkGeomTemplate.Checked = True
                chkSealTest.Checked = True
            End If

        ElseIf (mProjectView) Then
            If (cmbParkerPN_Part2.Text = "69") Then
                ''chkGeomTemplate.Checked = True
                chkSealIPE.Checked = True

            ElseIf (cmbParkerPN_Part2.Text = "76") Then
                ''chkGeomTemplate.Checked = True
                chkSealIPE.Checked = True

            ElseIf (cmbParkerPN_Part2.Text = "79") Then
                ''chkGeomTemplate.Checked = True
                chkSealIPE.Checked = True
            End If
        End If

    End Sub

    Private Sub chkLegacy_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                         Handles chkLegacy.CheckedChanged
        '=================================================================================

        If (chkLegacy.Enabled) Then
            cmdLegacy.Enabled = chkLegacy.Checked
            txtParkerPN_Legacy.Enabled = True
            txtParkerPNLegacy_Rev.Enabled = True
        Else
            txtParkerPN_Legacy.Enabled = False
            txtParkerPNLegacy_Rev.Enabled = False
        End If

        If (chkLegacy.Checked) Then
            mPartProject.PNR.Legacy_Exists = True
            cmbParkerPN_Part2.Enabled = False
            txtParkerPN_Part3.Enabled = False
            txtPN_PH_Rev.Enabled = False
            If (mblnEdit Or mblnAdd) Then
                txtParkerPNLegacy_Rev.Enabled = True
            End If

        Else
            mPartProject.PNR.Legacy_Exists = False
            If (mblnEdit Or mblnAdd) Then
                cmbParkerPN_Part2.Enabled = True
                txtParkerPN_Part3.Enabled = True
                txtPN_PH_Rev.Enabled = True
            End If

            txtParkerPNLegacy_Rev.Enabled = False
        End If

        chkNew.Checked = Not chkLegacy.Checked

    End Sub

    Private Sub chkNew_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                      Handles chkNew.CheckedChanged
        '===============================================================================
        'InitializeControl(False)

        If (chkNew.Checked) Then
            cmdLegacy.Enabled = False
            txtParkerPN_Legacy.Enabled = False
            txtParkerPNLegacy_Rev.Enabled = False
            mPartProject.PNR.Current_Exists = True
        Else
            mPartProject.PNR.Current_Exists = False
            If (mblnEdit Or mblnAdd) Then
                cmdLegacy.Enabled = True
                txtParkerPN_Legacy.Enabled = True
                txtParkerPNLegacy_Rev.Enabled = True
            End If

        End If

        chkLegacy.Checked = Not chkNew.Checked

    End Sub


    Private Sub mnuPN_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles mnuPN.CheckedChanged
        'If (mnuPN.Checked) Then
        '    mnuProject.Checked = False
        '    InitializeLocalObject()

        '    mSealIPE = True
        '    UpdateIndexField()
        '    grpCustomer.Text = "Projects:"
        '    Me.Text = "SealIPE: Part No"

        '    DisplayData()
        'End If
    End Sub

    Private Sub mnuProject_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles mnuProject.CheckedChanged
        'If (mnuProject.Checked) Then
        '    mnuPN.Checked = False
        '    InitializeLocalObject()

        '    mSealTest = True

        '    grpCustomer.Text = "P/N:"
        '    Me.Text = "SealTest: Part No"
        '    RetrievePN()

        '    DisplayData()
        'End If
    End Sub

    Private Sub mnuPN_Click(sender As System.Object, e As System.EventArgs) Handles mnuPN.Click
        '======================================================================================

        trvProjects.Refresh()
        InitializeLocalObject()

        If (mnuPN.Checked) Then
            mnuProject.Checked = True
            mnuPN.Checked = False

            'InitializeLocalObject()

            mPNView = True
            mProjectView = False
            grpCustomer.Text = "P/N:"
            Me.Text = "SealTest: Part No"
            RetrievePN()

            'DisplayData()

        Else
            mnuPN.Checked = True
            mnuProject.Checked = False

            'InitializeLocalObject()

            mProjectView = True
            mPNView = False
            UpdateIndexField()
            grpCustomer.Text = "Projects:"
            Me.Text = "SealIPE: Part No"

            'DisplayData()

        End If

        DisplayData()
    End Sub

    Private Sub mnuProject_Click(sender As System.Object, e As System.EventArgs) Handles mnuProject.Click
        '================================================================================================
        trvProjects.Refresh()
        InitializeLocalObject()

        If (mnuProject.Checked) Then
            mnuProject.Checked = False
            mnuPN.Checked = True
            'InitializeLocalObject()

            mProjectView = True
            mPNView = False
            UpdateIndexField()
            grpCustomer.Text = "Customer:"
            Me.Text = "SealIPE: Part No"

            'DisplayData()
        Else
            mnuProject.Checked = True
            mnuPN.Checked = False

            'InitializeLocalObject()

            mPNView = True
            mProjectView = False

            grpCustomer.Text = "P/N:"
            Me.Text = "SealTest: Part No"
            RetrievePN()

            'DisplayData()
        End If

        DisplayData()

    End Sub

    Private Sub chkPNNew_Parent_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                               Handles chkPNNew_Parent.CheckedChanged
        '=====================================================================================
        If (chkPNNew_Parent.Checked) Then
            mPartProject.PNR.ParentCurrent_Exists = True
            chkPNLegacy_Parent.Checked = False
            txtPNLegacy_Parent.Enabled = False
            txtPNParentLegacy_Rev.Enabled = False
        Else
            mPartProject.PNR.ParentCurrent_Exists = False
            ''txtParentCur_Part3.Text = ""
            ''txtParentCur_Rev.Text = ""
            chkPNLegacy_Parent.Checked = True
            txtPNLegacy_Parent.Enabled = True
            txtPNParentLegacy_Rev.Enabled = True
        End If

    End Sub

    Private Sub chkPNLegacy_Parent_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                  Handles chkPNLegacy_Parent.CheckedChanged
        '=============================================================================================
        If (chkPNLegacy_Parent.Checked) Then
            mPartProject.PNR.ParentLegacy_Exists = True
            chkPNNew_Parent.Checked = False
            cmbParentCur_Part2.Enabled = False
            txtParentCur_Part3.Enabled = False
            txtParentCur_Rev.Enabled = False
        Else
            mPartProject.PNR.ParentLegacy_Exists = False
            ''txtPNLegacy_Parent.Text = ""
            ''txtPNParentLegacy_Rev.Text = ""
            chkPNNew_Parent.Checked = True
            cmbParentCur_Part2.Enabled = True
            txtParentCur_Part3.Enabled = True
            txtParentCur_Rev.Enabled = True
        End If

    End Sub

    Private Sub chkRefDimNew_Exists_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                   Handles chkRefDimNew_Exists.CheckedChanged
        '===========================================================================================
        If (chkRefDimNew_Exists.Checked) Then
            mPartProject.PNR.RefDimCurrent_Exists = True
            chkRefDimLegacy_Exists.Checked = False
            txtRefPNNewDim_Legacy.Enabled = False
            txtRefPNLegacyDim_Rev.Enabled = False
        Else
            mPartProject.PNR.RefDimCurrent_Exists = False
            ''txtRefPNNewDim_Part3.Text = ""
            ''txtRefPNNewDim_Rev.Text = ""
            chkRefDimLegacy_Exists.Checked = True
            txtRefPNNewDim_Legacy.Enabled = True
            txtRefPNLegacyDim_Rev.Enabled = True
        End If

    End Sub

    Private Sub chkRefDimLegacy_Exists_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                      Handles chkRefDimLegacy_Exists.CheckedChanged
        '==============================================================================================
        If (chkRefDimLegacy_Exists.Checked) Then
            mPartProject.PNR.RefDimLegacy_Exists = True
            chkRefDimNew_Exists.Checked = False
            cmbRefPNNewDim_Part2.Enabled = False
            txtRefPNNewDim_Part3.Enabled = False
            txtRefPNNewDim_Rev.Enabled = False
        Else
            mPartProject.PNR.RefDimLegacy_Exists = False
            ''txtRefPNNewNotes_Legacy.Text = ""
            ''txtRefPNLegacyNotes_Rev.Text = ""
            chkRefDimNew_Exists.Checked = True
            cmbRefPNNewDim_Part2.Enabled = True
            txtRefPNNewDim_Part3.Enabled = True
            txtRefPNNewDim_Rev.Enabled = True
        End If

    End Sub

    Private Sub chkRefDimNotes_Exists_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                     Handles chkRefDimNotes_Exists.CheckedChanged
        '==============================================================================================
        If (chkRefDimNotes_Exists.Checked) Then
            mPartProject.PNR.RefNotesCurrent_Exists = True
            chkRefNotesLegacy_Exists.Checked = False
            txtRefPNNewNotes_Legacy.Enabled = False
            txtRefPNLegacyNotes_Rev.Enabled = False
        Else
            mPartProject.PNR.RefNotesCurrent_Exists = False
            ''txtRefPNNotes_Part3.Text = ""
            ''txtRefPNNewNotes_Rev.Text = ""
            chkRefNotesLegacy_Exists.Checked = True
            txtRefPNNewNotes_Legacy.Enabled = True
            txtRefPNLegacyNotes_Rev.Enabled = True
        End If
    End Sub

    Private Sub chkRefNotesLegacy_Exists_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                                        Handles chkRefNotesLegacy_Exists.CheckedChanged
        '==================================================================================================
        If (chkRefNotesLegacy_Exists.Checked) Then
            mPartProject.PNR.RefNotesLegacy_Exists = True
            chkRefDimNotes_Exists.Checked = False
            cmbRefNotesNewPN_Part2.Enabled = False
            txtRefPNNotes_Part3.Enabled = False
            txtRefPNNewNotes_Rev.Enabled = False
        Else
            mPartProject.PNR.RefNotesLegacy_Exists = False
            ''txtRefPNNewNotes_Legacy.Text = ""
            ''txtRefPNLegacyNotes_Rev.Text = ""
            chkRefDimNotes_Exists.Checked = True
            cmbRefNotesNewPN_Part2.Enabled = True
            txtRefPNNotes_Part3.Enabled = True
            txtRefPNNewNotes_Rev.Enabled = True
        End If
    End Sub

    Private Sub UserGroupToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) _
                                                Handles UserGroupToolStripMenuItem.Click
        '==========================================================================================

        Dim pfrmUserGroup As New frmUserGroup()
        pfrmUserGroup.ShowDialog()

        ''Dim pUserRole As New List(Of String)
        ''pUserRole = gUser.RetrieveProcessUserRoles()

        ''If (pUserRole.Contains("Admin")) Then
        ''    Dim pfrmUserGroup As New frmUserGroup()
        ''    pfrmUserGroup.ShowDialog()
        ''Else
        ''    Dim pfrmAdminLogin As New frmAdminLogin()
        ''    pfrmAdminLogin.ShowDialog()
        ''End If


    End Sub
End Class