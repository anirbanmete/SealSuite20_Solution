'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsSuiteUser                           '
'                        VERSION NO  :  2.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.IO
Imports System.Threading

Public Class clsUser

#Region "MEMBER VARIABLE DECLARATIONS:"
    Private mSystemLogin As String
    Private mFirstName As String
    Private mLastName As String
    Private mTitle As String
    Private mProgramDataFile As String

#End Region

#Region "CLASS PROPERTY ROUTINE:"

    Public ReadOnly Property SystemLogin() As String
        '===========================================
        Get
            Return mSystemLogin
        End Get

    End Property


    Public ReadOnly Property FirstName() As String
        '=========================================
        Get
            Return mFirstName
        End Get

    End Property

    Public ReadOnly Property LastName() As String
        '=========================================
        Get
            Return mLastName
        End Get

    End Property


    Public ReadOnly Property Title() As String
        '=========================================
        Get
            Return mTitle
        End Get

    End Property


    Public Property ProgramDataFile() As String
        '=======================================
        Get
            Return mProgramDataFile
        End Get
        Set(value As String)
            mProgramDataFile = value
        End Set

    End Property

#End Region

    Public Sub RetrieveUserRoles()
        '=========================
        mSystemLogin = Environment.UserName
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pRecCount As Integer = (From pRec In pSealSuiteEntities.tblUser
                                      Where pRec.fldSystemLogin = mSystemLogin Select pRec).Count()

        Dim pTitleID As Integer = 0
        If (pRecCount > 0) Then
            Dim pQry = (From pRec In pSealSuiteEntities.tblUser
                                       Where pRec.fldSystemLogin = mSystemLogin Select pRec).First()
            mFirstName = pQry.fldFirstName.Trim()
            mLastName = pQry.fldLastName.Trim()
            pTitleID = pQry.fldTitleID

            Dim pQryRole = (From pRec In pSealSuiteEntities.tblTitle
                                       Where pRec.fldID = pTitleID Select pRec).ToList()
            If (pQryRole.Count > 0) Then
                mTitle = pQryRole(0).fldTitle.Trim()
            End If

        End If

    End Sub

#Region "DB RELATED ROUTINES:"


    Public Sub UpdateTo_DB(ByVal FileName_In As String)
        '===============================================

        CloseExcelFiles()

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()
        'pApp.Visible = True

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing
        Dim pExitLoop As Boolean = False

        Dim pSealSuiteDBEntities As New SealSuiteDBEntities()
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try
            pWkbOrg = pApp.Workbooks.Open(FileName_In, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value, _
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
            Missing.Value, Missing.Value, Missing.Value)

            Dim pWkSheet As EXCEL.Worksheet

            pWkSheet = pWkbOrg.Worksheets("General")

            '....Table - tblTitle

            Dim pTitle_Start As Integer = 1

            Dim pTitle As New List(Of String)

            Dim pIndx As Integer = 0

            While (Not pExitLoop)

                Dim pVal As String = pWkSheet.Cells(5 + pIndx, pTitle_Start).value

                pExitLoop = String.IsNullOrEmpty(pVal)

                If (Not pExitLoop) Then
                    pTitle.Add(pVal)
                    pIndx = pIndx + 1
                End If

            End While

            If (pTitle.Count > 0) Then

                Dim pTitleRec = (From Rec In pSealSuiteDBEntities.tblTitle
                                     Select Rec).ToList()

                For i1 As Integer = 0 To pTitleRec.Count() - 1
                    pSealSuiteDBEntities.DeleteObject(pTitleRec(i1))
                    pSealSuiteDBEntities.SaveChanges()
                Next

                Dim pTitleList As New List(Of tblTitle)

                For i As Integer = 0 To pTitle.Count - 1
                    Dim pUserTitleList As New tblTitle
                    pTitleList.Add(pUserTitleList)
                    pTitleList(i).fldID = i + 1
                    pTitleList(i).fldTitle = pTitle(i)

                    pSealSuiteDBEntities.AddTotblTitle(pTitleList(i))
                Next

                pSealSuiteDBEntities.SaveChanges()

            End If


            '....Table - tblRole
            Dim pRole_Start As Integer = 3

            Dim pRole As New List(Of String)

            pIndx = 0
            pExitLoop = False

            While (Not pExitLoop)

                Dim pVal As String = pWkSheet.Cells(5 + pIndx, pRole_Start).value

                pExitLoop = String.IsNullOrEmpty(pVal)

                If (Not pExitLoop) Then
                    pRole.Add(pVal)
                    pIndx = pIndx + 1
                End If

            End While

            If (pRole.Count > 0) Then

                Dim pRoleRec = (From Rec In pSealSuiteDBEntities.tblRole
                                     Select Rec).ToList()

                For i1 As Integer = 0 To pRoleRec.Count() - 1
                    pSealSuiteDBEntities.DeleteObject(pRoleRec(i1))
                    pSealSuiteDBEntities.SaveChanges()
                Next

                Dim pRoleList As New List(Of tblRole)

                For i As Integer = 0 To pRole.Count - 1
                    Dim pUserRoleList As New tblRole
                    pRoleList.Add(pUserRoleList)
                    pRoleList(i).fldID = i + 1
                    pRoleList(i).fldRole = pRole(i)
                    pRoleList(i).fldIsSuperRole = False

                    pSealSuiteDBEntities.AddTotblRole(pRoleList(i))
                Next

                pSealSuiteDBEntities.SaveChanges()

            End If

            '....Table - tblSuperRole
            Dim pSuperRole_Start As Integer = 5
            Dim pSuperRole As New List(Of String)
            pIndx = 0
            pExitLoop = False

            While (Not pExitLoop)

                Dim pVal As String = pWkSheet.Cells(5 + pIndx, pSuperRole_Start).value
                pExitLoop = String.IsNullOrEmpty(pVal)

                If (Not pExitLoop) Then
                    pSuperRole.Add(pVal)
                    pIndx = pIndx + 1
                End If

            End While

            If (pSuperRole.Count > 0) Then

                Dim pLastID As Integer = 1
                Dim pRoleRec = (From Rec In pSealSuiteDBEntities.tblRole Order By Rec.fldID Descending Select Rec).ToList()

                If (pRoleRec.Count > 0) Then
                    pLastID = pRoleRec(0).fldID + 1
                End If

                Dim pSuperRoleList As New List(Of tblRole)

                For i As Integer = 0 To pSuperRole.Count - 1
                    Dim pUserSuperRoleList As New tblRole
                    pSuperRoleList.Add(pUserSuperRoleList)
                    pSuperRoleList(i).fldID = i + pLastID
                    pSuperRoleList(i).fldRole = pSuperRole(i)
                    pSuperRoleList(i).fldIsSuperRole = True

                    pSealSuiteDBEntities.AddTotblRole(pSuperRoleList(i))
                Next

                pSealSuiteDBEntities.SaveChanges()

            End If

            pWkSheet = pWkbOrg.Worksheets("SealProcess")

            '....Table - tblRolePrivilege
            Dim pRolePrivelege_Start As Integer = 1
            pIndx = 0
            pExitLoop = False
            Dim pRoleID As New List(Of Integer)

            Dim chkHeader As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkPreOrder As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkExport As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkOrdEntry As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkCost As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkApp As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkDesign As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkManf As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkPurchase As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkQlty As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkDwg As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkTest As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkPlanning As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkShipping As Microsoft.Office.Interop.Excel.CheckBox = Nothing
            Dim chkKeyChar As Microsoft.Office.Interop.Excel.CheckBox = Nothing

            Dim pCustServiceName() As String = {"Check Box 686", "Check Box 695", "Check Box 704", "Check Box 713", "Check Box 722", "Check Box 731", "Check Box 740", _
                                                "Check Box 749", "Check Box 864", "Check Box 758", "Check Box 767", "Check Box 776", "Check Box 785", "Check Box 876", "Check Box 794"}

            Dim pProgramManagerName() As String = {"Check Box 679", "Check Box 687", "Check Box 696", "Check Box 705", "Check Box 714", "Check Box 723", "Check Box 732", _
                                                   "Check Box 741", "Check Box 856", "Check Box 750", "Check Box 759", "Check Box 768", "Check Box 777", "Check Box 868", "Check Box 786"}

            Dim pEngineeringName() As String = {"Check Box 680", "Check Box 688", "Check Box 697", "Check Box 706", "Check Box 715", "Check Box 725", "Check Box 733", _
                                                "Check Box 742", "Check Box 857", "Check Box 751", "Check Box 760", "Check Box 769", "Check Box 778", "Check Box 869", "Check Box 787"}

            Dim pMfgName() As String = {"Check Box 681", "Check Box 689", "Check Box 698", "Check Box 707", "Check Box 716", "Check Box 725", "Check Box 734", _
                                        "Check Box 743", "Check Box 858", "Check Box 752", "Check Box 761", "Check Box 770", "Check Box 779", "Check Box 870", "Check Box 788"}

            Dim pQltyName() As String = {"Check Box 682", "Check Box 690", "Check Box 699", "Check Box 708", "Check Box 717", "Check Box 726", "Check Box 735", _
                                         "Check Box 744", "Check Box 859", "Check Box 753", "Check Box 762", "Check Box 771", "Check Box 780", "Check Box 871", "Check Box 789"}

            Dim pDrawingName() As String = {"Check Box 888", "Check Box 889", "Check Box 890", "Check Box 891", "Check Box 892", "Check Box 893", "Check Box 894", _
                                      "Check Box 895", "Check Box 908", "Check Box 896", "Check Box 899", "Check Box 902", "Check Box 905", "Check Box 911", "Check Box 907"}

            Dim pPlanName() As String = {"Check Box 683", "Check Box 691", "Check Box 700", "Check Box 709", "Check Box 718", "Check Box 727", "Check Box 736", _
                                         "Check Box 745", "Check Box 860", "Check Box 754", "Check Box 763", "Check Box 772", "Check Box 781", "Check Box 872", "Check Box 790"}

            Dim pPurchasingName() As String = {"Check Box 684", "Check Box 692", "Check Box 701", "Check Box 710", "Check Box 719", "Check Box 728", "Check Box 737", _
                                               "Check Box 746", "Check Box 861", "Check Box 755", "Check Box 764", "Check Box 773", "Check Box 782", "Check Box 873", "Check Box 791"}

            Dim pShippingName() As String = {"Check Box 685", "Check Box 693", "Check Box 702", "Check Box 711", "Check Box 720", "Check Box 729", "Check Box 738", _
                                             "Check Box 747", "Check Box 862", "Check Box 756", "Check Box 765", "Check Box 774", "Check Box 783", "Check Box 874", "Check Box 792"}

            Dim pTestName() As String = {"Check Box 795", "Check Box 694", "Check Box 703", "Check Box 712", "Check Box 721", "Check Box 730", "Check Box 739", _
                                         "Check Box 748", "Check Box 863", "Check Box 757", "Check Box 766", "Check Box 775", "Check Box 784", "Check Box 875", "Check Box 793"}

            Dim pChairPersonName() As String = {"Check Box 797", "Check Box 800", "Check Box 803", "Check Box 806", "Check Box 809", "Check Box 812", "Check Box 815", _
                                               "Check Box 818", "Check Box 865", "Check Box 821", "Check Box 824", "Check Box 833", "Check Box 842", "Check Box 879", "Check Box 844"}

            Dim pCoordinatorName() As String = {"Check Box 798", "Check Box 801", "Check Box 804", "Check Box 807", "Check Box 810", "Check Box 813", "Check Box 816", _
                                               "Check Box 819", "Check Box 865", "Check Box 822", "Check Box 828", "Check Box 836", "Check Box 847", "Check Box 882", "Check Box 849"}

            Dim pAdminName() As String = {"Check Box 799", "Check Box 802", "Check Box 805", "Check Box 808", "Check Box 811", "Check Box 814", "Check Box 817", _
                                      "Check Box 820", "Check Box 867", "Check Box 758", "Check Box 823", "Check Box 830", "Check Box 852", "Check Box 885", "Check Box 854"}

            Dim pCheckRole() As Microsoft.Office.Interop.Excel.CheckBox = {chkHeader, chkPreOrder, chkExport, chkOrdEntry, chkCost, chkApp, _
                                                                           chkDesign, chkManf, chkPurchase, chkQlty, chkDwg, chkTest, _
                                                                           chkPlanning, chkShipping, chkKeyChar}

            Dim pRecDeleted As Boolean = False
            While (Not pExitLoop)

                Dim pVal As String = pWkSheet.Cells(10 + pIndx, pRolePrivelege_Start).value

                Dim pHeader As Boolean = False
                Dim pPreOrder As Boolean = False
                Dim pExport As Boolean = False
                Dim pOrdEntry As Boolean = False
                Dim pCost As Boolean = False
                Dim pApplication As Boolean = False
                Dim pDesign As Boolean = False
                Dim pManf As Boolean = False
                Dim pPurchase As Boolean = False
                Dim pQlty As Boolean = False
                Dim pDwg As Boolean = False
                Dim pTest As Boolean = False
                Dim pPlan As Boolean = False
                Dim pShipping As Boolean = False
                Dim pKeyChar As Boolean = False

                Select Case pIndx

                    Case 0
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pCustServiceName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 1
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pProgramManagerName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 2
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pEngineeringName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 3
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pMfgName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 4
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pQltyName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 5
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pDrawingName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 6
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pPlanName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 7
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pPurchasingName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 8
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pShippingName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 9
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pTestName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 10
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pChairPersonName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 11
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pCoordinatorName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 12
                        For i As Integer = 0 To pCustServiceName.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pAdminName(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                End Select


                If (pCheckRole(0).Value > 0) Then
                    pHeader = True
                End If

                If (pCheckRole(1).Value > 0) Then
                    pPreOrder = True
                End If

                If (pCheckRole(2).Value > 0) Then
                    pExport = True
                End If

                If (pCheckRole(3).Value > 0) Then
                    pOrdEntry = True
                End If

                If (pCheckRole(4).Value > 0) Then
                    pCost = True
                End If

                If (pCheckRole(5).Value > 0) Then
                    pApplication = True
                End If

                If (pCheckRole(6).Value > 0) Then
                    pDesign = True
                End If

                If (pCheckRole(7).Value > 0) Then
                    pManf = True
                End If

                If (pCheckRole(8).Value > 0) Then
                    pPurchase = True
                End If

                If (pCheckRole(9).Value > 0) Then
                    pQlty = True
                End If

                If (pCheckRole(10).Value > 0) Then
                    pDwg = True
                End If

                If (pCheckRole(11).Value > 0) Then
                    pTest = True
                End If

                If (pCheckRole(12).Value > 0) Then
                    pPlan = True
                End If

                If (pCheckRole(13).Value > 0) Then
                    pShipping = True
                End If

                If (pCheckRole(14).Value > 0) Then
                    pKeyChar = True
                End If


                pExitLoop = String.IsNullOrEmpty(pVal)

                If (Not pExitLoop) Then

                    If (Not pRecDeleted) Then
                        Dim pRolePrivelegeRec = (From Rec In pSealProcessDBEntities.tblRolePrivilege
                                           Select Rec).ToList()

                        For i As Integer = 0 To pRolePrivelegeRec.Count() - 1
                            pSealProcessDBEntities.DeleteObject(pRolePrivelegeRec(i))
                            pSealProcessDBEntities.SaveChanges()
                        Next

                        pRecDeleted = True

                    End If

                    Dim pUserRoleID As Integer = 0
                    If (pVal <> "") Then
                        Dim pRoleRec = (From Rec In pSealSuiteDBEntities.tblRole Where Rec.fldRole = pVal Select Rec).ToList()

                        If (pRoleRec.Count > 0) Then
                            pUserRoleID = pRoleRec(0).fldID
                        End If
                    End If



                    Dim pRolePrivelegeList As New tblRolePrivilege

                    Dim pUserSuperRoleList As New tblRolePrivilege
                    pRolePrivelegeList.fldID = pIndx + 1
                    pRolePrivelegeList.fldRoleID = pUserRoleID
                    pRolePrivelegeList.fldHeader = pHeader
                    pRolePrivelegeList.fldPreOrder = pPreOrder
                    pRolePrivelegeList.fldExport = pExport
                    pRolePrivelegeList.fldOrdEntry = pOrdEntry
                    pRolePrivelegeList.fldCost = pCost
                    pRolePrivelegeList.fldApp = pApplication
                    pRolePrivelegeList.fldDesign = pDesign
                    pRolePrivelegeList.fldManf = pManf
                    pRolePrivelegeList.fldPurchase = pPurchase
                    pRolePrivelegeList.fldQlty = pQlty
                    pRolePrivelegeList.fldDwg = pDwg
                    pRolePrivelegeList.fldTest = pTest
                    pRolePrivelegeList.fldPlanning = pPlan
                    pRolePrivelegeList.fldShipping = pShipping
                    pRolePrivelegeList.fldKeyChar = pKeyChar

                    pSealProcessDBEntities.AddTotblRolePrivilege(pRolePrivelegeList)
                    pSealProcessDBEntities.SaveChanges()

                    pIndx = pIndx + 1
                End If

            End While

            ' ''....Table - tblSealUser
            ''Dim pLastName_Start As Integer = 1
            ''Dim pFirstName_Start As Integer = 2
            ''Dim pTitle_Start As Integer = 3
            ''Dim pSystemLogin_Start As Integer = 4
            ''Dim pRole1_Start As Integer = 5
            ''Dim pRole2_Start As Integer = 6
            ''Dim pRole3_Start As Integer = 7
            ''Dim pSuperRole1_Start As Integer = 8
            ''Dim pSuperRole2_Start As Integer = 9
            ''Dim pUserRoleTblStartIndex As Integer = 46


            ''Dim pUserSuperRole As New List(Of String)

            ''pIndx = 0
            ''Dim pUserRoleIndex As Integer = 1
            ''pExitLoop = False
            ''pRecDeleted = False
            ''While (Not pExitLoop)

            ''    Dim pVal1 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pLastName_Start).value
            ''    Dim pVal2 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pFirstName_Start).value
            ''    Dim pVal3 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pTitle_Start).value
            ''    Dim pVal4 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pSystemLogin_Start).value
            ''    Dim pVal5 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pRole1_Start).value
            ''    Dim pVal6 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pRole2_Start).value
            ''    Dim pVal7 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pRole3_Start).value
            ''    Dim pVal8 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pSuperRole1_Start).value
            ''    Dim pVal9 As String = pWkSheet.Cells(pUserRoleTblStartIndex + pIndx, pSuperRole2_Start).value

            ''    Dim pUserRole As New List(Of String)


            ''    If (IsNothing(pVal1)) Then
            ''        pVal1 = ""
            ''    End If

            ''    If (IsNothing(pVal2)) Then
            ''        pVal2 = ""
            ''    End If

            ''    If (IsNothing(pVal3)) Then
            ''        pVal3 = ""
            ''    End If

            ''    If (IsNothing(pVal5)) Then
            ''        pVal5 = ""
            ''    End If

            ''    If (IsNothing(pVal6)) Then
            ''        pVal6 = ""
            ''    End If

            ''    If (IsNothing(pVal7)) Then
            ''        pVal7 = ""
            ''    End If

            ''    If (IsNothing(pVal8)) Then
            ''        pVal8 = ""
            ''    End If

            ''    If (IsNothing(pVal9)) Then
            ''        pVal9 = ""
            ''    End If

            ''    pExitLoop = String.IsNullOrEmpty(pVal4)

            ''    If (Not pExitLoop) Then

            ''        If (Not pRecDeleted) Then

            ''            Dim pUserRec = (From Rec In pSealProcessDBEntities.tblUser
            ''                                Select Rec).ToList()

            ''            For i As Integer = 0 To pUserRec.Count() - 1
            ''                pSealProcessDBEntities.DeleteObject(pUserRec(i))
            ''                pSealProcessDBEntities.SaveChanges()
            ''            Next
            ''            pRecDeleted = True
            ''        End If


            ''        Dim pUserList As New tblUser
            ''        pUserList.fldID = pIndx + 1
            ''        Dim pUserID As Integer = pIndx + 1
            ''        pUserList.fldLastName = pVal1
            ''        pUserList.fldFirstName = pVal2
            ''        pUserList.fldTitle = pVal3
            ''        pUserList.fldSystemLogin = pVal4

            ''        pSealProcessDBEntities.AddTotblUser(pUserList)
            ''        pSealProcessDBEntities.SaveChanges()

            ''        If (pVal5 <> "") Then
            ''            pUserRole.Add(pVal5)
            ''        End If
            ''        If (pVal6 <> "") Then
            ''            pUserRole.Add(pVal6)
            ''        End If
            ''        If (pVal7 <> "") Then
            ''            pUserRole.Add(pVal7)
            ''        End If
            ''        If (pVal8 <> "") Then
            ''            pUserRole.Add(pVal8)
            ''        End If
            ''        If (pVal9 <> "") Then
            ''            pUserRole.Add(pVal9)
            ''        End If


            ''        Dim pRecAdded As Boolean = False
            ''        If (pUserRole.Count > 0) Then

            ''            For i As Integer = 0 To pUserRole.Count - 1

            ''                Dim pUserRoleName As String = pUserRole(i).Trim()
            ''                Dim pUserRoleRec = (From Rec In pSealProcessDBEntities.tblRole Where Rec.fldRole = pUserRoleName Select Rec).ToList()

            ''                If (pUserRoleRec.Count > 0) Then
            ''                    Dim pUserRoleList As New tblUserRole

            ''                    pUserRoleList.fldID = pUserRoleIndex
            ''                    pUserRoleList.fldUserID = pUserID
            ''                    pUserRoleList.fldRoleID = pUserRoleRec(0).fldID

            ''                    pSealProcessDBEntities.AddTotblUserRole(pUserRoleList)
            ''                    pSealProcessDBEntities.SaveChanges()
            ''                    pUserRoleIndex = pUserRoleIndex + 1
            ''                End If
            ''            Next

            ''        End If

            ''        pIndx = pIndx + 1
            ''    End If

            ''End While

            Dim pFileTitle As String = Path.GetFileName(FileName_In)
            Dim pMsg As String = "User Data Updated from: " & vbLf & Space(10) & pFileTitle
            MessageBox.Show(pMsg, "User Data File!", MessageBoxButtons.OK)

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

End Class
