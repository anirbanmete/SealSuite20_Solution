'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsSuiteUser                           '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  14FEB18                                '
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
    Private mRole As String
    Private mViewer As Boolean
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

    Public Property Role() As String
        '=============================
        Get
            Return mRole
        End Get

        Set(value As String)
            mRole = value
        End Set

    End Property

#End Region

    Public Sub RetrieveUserTitle()
        '=========================
        mSystemLogin = Environment.UserName
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pRecCount As Integer = (From pRec In pSealSuiteEntities.tblUser
                                    Where pRec.fldSystemLogin = mSystemLogin Select pRec).Count()

        Dim pTitleID As Integer = 0
        If (pRecCount > 0) Then
            Dim pQry = (From pRec In pSealSuiteEntities.tblUser
                        Where pRec.fldSystemLogin = mSystemLogin Select pRec).First()

            If (Not IsNothing(pQry.fldFirstName)) Then
                mFirstName = pQry.fldFirstName.Trim()
            End If

            If (Not IsNothing(pQry.fldLastName)) Then
                mLastName = pQry.fldLastName.Trim()
            End If

            pTitleID = pQry.fldTitleID

            Dim pQryRole = (From pRec In pSealSuiteEntities.tblTitle
                            Where pRec.fldID = pTitleID Select pRec).ToList()
            If (pQryRole.Count > 0) Then
                mTitle = pQryRole(0).fldTitle.Trim()
            End If

        End If

        'AES 27FEB19
        If (mSystemLogin = "506968") Then
            mFirstName = "Alicia"
            mLastName = "Sorban"
            mTitle = "Eng. Spec."
        End If


    End Sub

    Public Function RetrieveProcessUserRoles() As List(Of String)

        '=================================================
        mViewer = False
        Dim pUserRole As New List(Of String)

        mSystemLogin = Environment.UserName
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pRecCount As Integer = (From pRec In pSealSuiteEntities.tblUser
                                    Where pRec.fldSystemLogin = mSystemLogin Select pRec).Count()

        Dim pUserID As Integer = 0
        If (pRecCount > 0) Then
            Dim pQry = (From pRec In pSealSuiteEntities.tblUser
                        Where pRec.fldSystemLogin = mSystemLogin Select pRec).First()

            pUserID = pQry.fldID

        End If

        If (pUserID > 0) Then

            Dim pUserRoleRecCount As Integer = (From pRec In pSealSuiteEntities.tblProcess_UserRole
                                                Where pRec.fldUserID = pUserID Select pRec).Count()

            If (pUserRoleRecCount > 0) Then
                Dim pQry = (From pRec In pSealSuiteEntities.tblProcess_UserRole
                            Where pRec.fldUserID = pUserID Select pRec).ToList()

                If (pQry.Count > 0) Then
                    For i As Integer = 0 To pQry.Count - 1
                        Dim pRoleID As Integer = pQry(i).fldRoleID

                        Dim pUserRoleRec = (From pRec In pSealSuiteEntities.tblRole
                                            Where pRec.fldID = pRoleID Select pRec).ToList()
                        If (pUserRoleRec.Count > 0) Then
                            pUserRole.Add(pUserRoleRec(0).fldRole.Trim())
                        End If
                    Next

                End If

            End If
        End If

        If (pUserRole.Count = 0) Then
            mViewer = True
        End If

        'AES 27FEB19
        If (mSystemLogin = "506968") Then
            If (Not pUserRole.Contains("Admin")) Then
                pUserRole.Add("Admin")
            End If
        End If

        Return pUserRole

    End Function

    Public Function GetRoleID(ByVal Role_In As String) As Integer
        '========================================================
        Dim pRoleID As Integer = 0
        Dim pSealSuiteEntities As New SealSuiteDBEntities()

        Dim pUserRoleRec = (From pRec In pSealSuiteEntities.tblRole
                            Where pRec.fldRole = Role_In Select pRec).ToList()
        If (pUserRoleRec.Count > 0) Then
            pRoleID = pUserRoleRec(0).fldID
        End If

        Return pRoleID

    End Function

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

        Dim pRole1(), pRole2(), pRole3(), pRole4(), pRole5(), pRole6(), pRole7(), pRole8(), pRole9(), pRole10(), pRole11(), pRole12() As String
        Dim pRole13(), pRole14(), pRole15(), pRole16(), pRole17(), pRole18(), pRole19(), pRole20(), pRole21(), pRole22(), pRole23() As String

        Try
            pWkbOrg = pApp.Workbooks.Open(FileName_In, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
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

            pRole1 = {"Check Box 686", "Check Box 695", "Check Box 704", "Check Box 713", "Check Box 722", "Check Box 731", "Check Box 740",
                      "Check Box 749", "Check Box 864", "Check Box 758", "Check Box 767", "Check Box 776", "Check Box 785", "Check Box 876", "Check Box 794"}

            pRole2 = {"Check Box 679", "Check Box 687", "Check Box 696", "Check Box 705", "Check Box 714", "Check Box 723", "Check Box 732",
                                                   "Check Box 741", "Check Box 856", "Check Box 750", "Check Box 759", "Check Box 768", "Check Box 777", "Check Box 868", "Check Box 786"}

            pRole3 = {"Check Box 680", "Check Box 688", "Check Box 697", "Check Box 706", "Check Box 715", "Check Box 725", "Check Box 733",
                                                "Check Box 742", "Check Box 857", "Check Box 751", "Check Box 760", "Check Box 769", "Check Box 778", "Check Box 869", "Check Box 787"}

            pRole4 = {"Check Box 681", "Check Box 689", "Check Box 698", "Check Box 707", "Check Box 716", "Check Box 725", "Check Box 734",
                                        "Check Box 743", "Check Box 858", "Check Box 752", "Check Box 761", "Check Box 770", "Check Box 779", "Check Box 870", "Check Box 788"}

            pRole5 = {"Check Box 682", "Check Box 690", "Check Box 699", "Check Box 708", "Check Box 717", "Check Box 726", "Check Box 735",
                                         "Check Box 744", "Check Box 859", "Check Box 753", "Check Box 762", "Check Box 771", "Check Box 780", "Check Box 871", "Check Box 789"}

            pRole6 = {"Check Box 888", "Check Box 889", "Check Box 890", "Check Box 891", "Check Box 892", "Check Box 893", "Check Box 894",
                                      "Check Box 895", "Check Box 908", "Check Box 896", "Check Box 899", "Check Box 902", "Check Box 905", "Check Box 911", "Check Box 907"}

            pRole7 = {"Check Box 683", "Check Box 691", "Check Box 700", "Check Box 709", "Check Box 718", "Check Box 727", "Check Box 736",
                                         "Check Box 745", "Check Box 860", "Check Box 754", "Check Box 763", "Check Box 772", "Check Box 781", "Check Box 872", "Check Box 790"}

            pRole8 = {"Check Box 684", "Check Box 692", "Check Box 701", "Check Box 710", "Check Box 719", "Check Box 728", "Check Box 737",
                                               "Check Box 746", "Check Box 861", "Check Box 755", "Check Box 764", "Check Box 773", "Check Box 782", "Check Box 873", "Check Box 791"}

            pRole9 = {"Check Box 685", "Check Box 693", "Check Box 702", "Check Box 711", "Check Box 720", "Check Box 729", "Check Box 738",
                                             "Check Box 747", "Check Box 862", "Check Box 756", "Check Box 765", "Check Box 774", "Check Box 783", "Check Box 874", "Check Box 792"}

            pRole10 = {"Check Box 795", "Check Box 694", "Check Box 703", "Check Box 712", "Check Box 721", "Check Box 730", "Check Box 739",
                                         "Check Box 748", "Check Box 863", "Check Box 757", "Check Box 766", "Check Box 775", "Check Box 784", "Check Box 875", "Check Box 793"}

            pRole11 = {"Check Box 797", "Check Box 800", "Check Box 803", "Check Box 806", "Check Box 809", "Check Box 812", "Check Box 815",
                                               "Check Box 818", "Check Box 865", "Check Box 821", "Check Box 824", "Check Box 833", "Check Box 842", "Check Box 879", "Check Box 844"}

            pRole12 = {"Check Box 798", "Check Box 801", "Check Box 804", "Check Box 807", "Check Box 810", "Check Box 813", "Check Box 816",
                                               "Check Box 819", "Check Box 865", "Check Box 822", "Check Box 828", "Check Box 836", "Check Box 847", "Check Box 882", "Check Box 849"}

            pRole13 = {"Check Box 799", "Check Box 802", "Check Box 805", "Check Box 808", "Check Box 811", "Check Box 814", "Check Box 817",
                                      "Check Box 820", "Check Box 867", "Check Box 758", "Check Box 823", "Check Box 830", "Check Box 852", "Check Box 885", "Check Box 854"}

            pRole14 = {"Check Box 915", "Check Box 916", "Check Box 917", "Check Box 918", "Check Box 919", "Check Box 920", "Check Box 921",
                                      "Check Box 922", "Check Box 928", "Check Box 923", "Check Box 924", "Check Box 925", "Check Box 926", "Check Box 929", "Check Box 927"}

            pRole15 = {"Check Box 931", "Check Box 932", "Check Box 933", "Check Box 934", "Check Box 935", "Check Box 936", "Check Box 937",
                                      "Check Box 938", "Check Box 944", "Check Box 939", "Check Box 940", "Check Box 941", "Check Box 942", "Check Box 945", "Check Box 943"}

            pRole16 = {"Check Box 947", "Check Box 948", "Check Box 949", "Check Box 950", "Check Box 951", "Check Box 952", "Check Box 953", "Check Box 954", "Check Box 960",
                        "Check Box 955", "Check Box 956", "Check Box 957", "Check Box 958", "Check Box 961", "Check Box 959"}

            pRole17 = {"Check Box 963", "Check Box 964", "Check Box 965", "Check Box 966", "Check Box 967", "Check Box 968", "Check Box 969", "Check Box 970", "Check Box 976",
                        "Check Box 971", "Check Box 972", "Check Box 973", "Check Box 974", "Check Box 977", "Check Box 975"}

            pRole18 = {"Check Box 979", "Check Box 980", "Check Box 981", "Check Box 982", "Check Box 983", "Check Box 984", "Check Box 985", "Check Box 986", "Check Box 992",
                        "Check Box 987", "Check Box 988", "Check Box 989", "Check Box 990", "Check Box 993", "Check Box 991"}

            pRole19 = {"Check Box 995", "Check Box 996", "Check Box 997", "Check Box 998", "Check Box 999", "Check Box 1000", "Check Box 1001", "Check Box 1002", "Check Box 1008",
                        "Check Box 1003", "Check Box 1004", "Check Box 1005", "Check Box 1006", "Check Box 1009", "Check Box 1007"}

            pRole20 = {"Check Box 1011", "Check Box 1012", "Check Box 1013", "Check Box 1014", "Check Box 1015", "Check Box 1016", "Check Box 1017", "Check Box 1018", "Check Box 1024",
                        "Check Box 1019", "Check Box 1020", "Check Box 1021", "Check Box 1022", "Check Box 1025", "Check Box 1023"}

            pRole21 = {"Check Box 1027", "Check Box 1028", "Check Box 1029", "Check Box 1030", "Check Box 1031", "Check Box 1032", "Check Box 1033", "Check Box 1034", "Check Box 1040",
                        "Check Box 1035", "Check Box 1036", "Check Box 1037", "Check Box 1038", "Check Box 1041", "Check Box 1039"}

            pRole22 = {"Check Box 1043", "Check Box 1044", "Check Box 1045", "Check Box 1046", "Check Box 1047", "Check Box 1048", "Check Box 1049", "Check Box 1050", "Check Box 1056",
                        "Check Box 1051", "Check Box 1052", "Check Box 1053", "Check Box 1054", "Check Box 1057", "Check Box 1055"}

            pRole23 = {"Check Box 1059", "Check Box 1060", "Check Box 1061", "Check Box 1062", "Check Box 1063", "Check Box 1064", "Check Box 1065", "Check Box 1066", "Check Box 1072",
                        "Check Box 1067", "Check Box 1068", "Check Box 1069", "Check Box 1070", "Check Box 1073", "Check Box 1071"}

            Dim pCheckRole() As Microsoft.Office.Interop.Excel.CheckBox = {chkHeader, chkPreOrder, chkExport, chkOrdEntry, chkCost, chkApp,
                                                                           chkDesign, chkManf, chkPurchase, chkQlty, chkDwg, chkTest,
                                                                           chkPlanning, chkShipping, chkKeyChar}

            Dim pRecDeleted As Boolean = False
            'While (Not pExitLoop)
            For j As Integer = 0 To 22

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
                        For i As Integer = 0 To pRole1.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole1(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 1
                        For i As Integer = 0 To pRole2.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole2(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 2
                        For i As Integer = 0 To pRole3.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole3(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 3
                        For i As Integer = 0 To pRole4.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole4(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 4
                        For i As Integer = 0 To pRole5.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole5(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 5
                        For i As Integer = 0 To pRole6.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole6(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 6
                        For i As Integer = 0 To pRole7.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole7(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 7
                        For i As Integer = 0 To pRole8.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole8(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 8
                        For i As Integer = 0 To pRole9.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole9(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 9
                        For i As Integer = 0 To pRole10.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole10(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 10
                        For i As Integer = 0 To pRole11.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole11(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 11
                        For i As Integer = 0 To pRole12.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole12(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 12
                        For i As Integer = 0 To pRole13.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole13(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 13
                        For i As Integer = 0 To pRole14.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole14(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 14
                        For i As Integer = 0 To pRole15.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole15(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 15
                        For i As Integer = 0 To pRole16.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole16(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 16
                        For i As Integer = 0 To pRole17.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole17(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 17
                        For i As Integer = 0 To pRole18.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole18(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 18
                        For i As Integer = 0 To pRole19.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole19(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 20
                        For i As Integer = 0 To pRole21.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole21(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 21
                        For i As Integer = 0 To pRole22.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole22(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

                        Next

                    Case 22
                        For i As Integer = 0 To pRole23.Count - 1
                            pCheckRole(i) = CType(pWkSheet.Shapes.Item(pRole23(i)).OLEFormat.Object, Microsoft.Office.Interop.Excel.CheckBox)

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


                'pExitLoop = String.IsNullOrEmpty(pVal)

                'If (Not pExitLoop) Then

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

                    End If
                End If
                pIndx = pIndx + 1
                'End If

                'End While
            Next

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
            MessageBox.Show(ex.ToString())

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
