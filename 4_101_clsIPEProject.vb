
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsProject                             '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY17                                '
'                                                                              '
'===============================================================================

Imports System.Threading
Imports System.Globalization
Imports System.Linq
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class IPE_clsProject
    Implements ICloneable

#Region "STRUCTURES:"            'AES 23MAR17

    <Serializable()> _
    Public Structure sIPEDesign
        Public Exists As Boolean
        Public Selected As Boolean
    End Structure

#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"
    'Private mIPEDBEntities As New SealIPEDBEntities()

    Private mCustomer_ID As Integer
    Private mCustName As String

    Private mPlatform_ID As Integer
    Private mPlatName As String

    Private mLocation_ID As Integer
    Private mLocName As String

    Private mPN_ID As Integer
    Private mPN_No As String
    Private mPN_Legacy As String

    Private mPN_Rev_ID As Integer
    Private mPN_Rev_Name As String
    Private mPN_Rev_Legacy As String

    Private mPN_Cust As String

    Private mPN_Rev_Date As DateTime
    Private mProject_ID As Integer

    Private mUnit As String = "English"
    Private mCulture As String

    Private mIPEDesign As sIPEDesign                'AES 23MAR17
    Private mAnalysis As New List(Of IPE_clsAnalysis)

    'AES 22MAR17
    Private mIPECavity As IPE_clsCavity
    Private mIPEOpCond As New IPE_clsOpCond
    Private mIPESeal As IPE_clsSeal

    Private mCatalogued As Boolean  'AES 21MAR17

#End Region

#Region "PROPERTY ROUTINES:"

    '....Customer_ID.
    Public Property Customer_ID() As Integer
        '===================================
        Get
            Return mCustomer_ID
            mCustName = Customer()
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mCustomer_ID = sngData
            mCustName = Customer()
        End Set

    End Property


    Public ReadOnly Property CustName() As String
        '=========================================
        Get
            Return mCustName
        End Get
    End Property


    '....Platform_ID.
    Public Property Platform_ID() As Integer
        '===================================
        Get
            Return mPlatform_ID
            mPlatName = Platform()
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mPlatform_ID = sngData
            mPlatName = Platform()
        End Set

    End Property


    Public ReadOnly Property PlatName() As String
        '=========================================
        Get
            Return mPlatName
        End Get
    End Property


    '....Location_ID.
    Public Property Location_ID() As Integer
        '===================================
        Get
            Return mLocation_ID
            mLocName = Location()
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mLocation_ID = sngData
            mLocName = Location()
        End Set

    End Property


    Public ReadOnly Property LocName() As String
        '=========================================
        Get
            Return mLocName
        End Get
    End Property


    '....Parker PN_ID.
    Public Property PN_ID() As Integer
        '===================================
        Get
            Return mPN_ID
            mPN_No = PN()
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mPN_ID = sngData
            mPN_No = PN()
        End Set

    End Property

    Public ReadOnly Property PN_No() As String
        '======================================
        Get
            Return mPN_No
        End Get
    End Property

    '....Parker PN_Legacy
    Public Property PN_Legacy() As String
        '===================================
        Get
            Return mPN_Legacy
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mPN_Legacy = strData
        End Set

    End Property


    '....Parker PN_Rev_ID.
    Public Property PN_Rev_ID() As Integer
        '==================================
        Get
            Return mPN_Rev_ID
            mPN_Rev_Name = PN_Rev()
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mPN_Rev_ID = sngData
            mPN_Rev_Name = PN_Rev()
        End Set

    End Property

    Public ReadOnly Property PN_Rev_Name() As String
        '=======================================
        Get
            Return mPN_Rev_Name
        End Get
    End Property


    '....Parker PN_Rev_Legacy
    Public Property PN_Rev_Legacy() As String
        '===================================
        Get
            Return mPN_Rev_Legacy
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mPN_Rev_Legacy = strData
        End Set

    End Property

    '....PN_Cust
    Public Property PN_Cust() As String
        '================================
        Get
            Return mPN_Cust
        End Get

        Set(ByVal value As String)
            mPN_Cust = value
        End Set

    End Property


    '....Project_ID.
    Public Property Project_ID() As Integer
        '===================================
        Get
            Return mProject_ID
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mProject_ID = sngData
        End Set

    End Property


    '....IPEDesign
    Public ReadOnly Property IPEDesign() As sIPEDesign
        '=============================================          'AES 23MAR17
        Get
            Return mIPEDesign
        End Get
    End Property


    '....IPEDesign_Exists
    Public WriteOnly Property IPEDesign_Exists() As Boolean
        '===================================================        'AES 23MAR17
        Set(ByVal blnVal As Boolean)
            mIPEDesign.Exists = blnVal
        End Set
    End Property


    '....IPEDesign_Selected
    Public WriteOnly Property IPEDesign_Selected() As Boolean
        '=====================================================      'AES 23MAR17
        Set(ByVal blnVal As Boolean)
            mIPEDesign.Selected = blnVal
        End Set
    End Property


    '....Analysis Set
    Public Property Analysis() As List(Of IPE_clsAnalysis)
        '=============================================
        Get
            Return mAnalysis
        End Get

        Set(Obj As List(Of IPE_clsAnalysis))
            mAnalysis = Obj
        End Set
    End Property

    '....Unit.    
    Public Property Unit() As String
        '============================
        Get
            Return mUnit
        End Get

        Set(ByVal strData As String)
            '-----------------------
            mUnit = strData
        End Set

    End Property

    '....Culture Name.    
    Public Property CultureName() As String
        '==================================
        Get
            Return mCulture
        End Get

        Set(ByVal strData As String)
            '-----------------------
            mCulture = strData
            SetCulture()

        End Set

    End Property

    '....IPECavity
    Public Property IPECavity() As IPE_clsCavity
        '================================
        Get
            Return mIPECavity
        End Get

        Set(Obj As IPE_clsCavity)
            mIPECavity = Obj
        End Set
    End Property


    '....IPEOpCond
    Public Property IPEOpCond() As IPE_clsOpCond
        '================================
        Get
            Return mIPEOpCond
        End Get

        Set(Obj As IPE_clsOpCond)
            mIPEOpCond = Obj
        End Set
    End Property

    '....IPESeal
    Public Property IPESeal() As IPE_clsSeal
        '============================
        Get
            Return mIPESeal
        End Get

        Set(Obj As IPE_clsSeal)
            mIPESeal = Obj
        End Set

    End Property

    '....Catalogued.    
    Public Property Catalogued() As Boolean
        '==================================
        Get
            Return mCatalogued
        End Get

        Set(ByVal pblnVal As Boolean)
            '--------------------------------
            mCatalogued = pblnVal
        End Set

    End Property

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New()
        '===========
        mCulture = "USA"
    End Sub

#End Region

#Region "UTILITY ROUTINES:"

    Public Sub Retrive_FromPNR(ByVal ID_In As Integer)
        '==============================================
        Dim pPartDBEntities As New SealPartDBEntities()
        Dim pQry = (From pRec In pPartDBEntities.tblProject
                            Where pRec.fldID = ID_In Select pRec).First()

        mPN_ID = pQry.fldPNID
        mPN_Rev_ID = pQry.fldRevID
        mCustomer_ID = pQry.fldCustID
        mPlatform_ID = pQry.fldPlatformID
        mLocation_ID = pQry.fldLocID

    End Sub

    Public Sub SaveProject_FromPart(ByVal PNID_In As Integer, ByVal RevID_In As Integer,
                                    ByVal ProjectID_In As Integer, ByVal User_In As String)
        '===============================================================================
        Dim pIPEDBEntities As New SealIPEDBEntities
        Dim pProject As New tblIPEProject
        Dim pProjectID As Integer = 0

        Dim pQryProject = (From pRec In pIPEDBEntities.tblIPEProject
                           Where pRec.fldPNID = PNID_In And
                                 pRec.fldRevID = RevID_In And
                                 pRec.fldPNR_CustInfoID = ProjectID_In
                                 Select pRec).ToList()

        If (pQryProject.Count > 0) Then
            pProject = pQryProject(0)
            pProjectID = pProject.fldID
        Else
            pProject.fldPNID = PNID_In
            pProject.fldRevID = RevID_In
            pProject.fldPNR_CustInfoID = ProjectID_In
            pProject.fldUser = User_In
            pProject.fldDate = DateTime.Now()

            Dim pRecProject_Count As Integer = (From pRec In pIPEDBEntities.tblIPEProject Order By pRec.fldID Descending
                               Select pRec).Count()

            If (pRecProject_Count > 0) Then
                Dim pRecProject = (From pRec In pIPEDBEntities.tblIPEProject Order By pRec.fldID Descending
                               Select pRec).First()

                pProjectID = pRecProject.fldID + 1
                pProject.fldID = pProjectID

            Else
                pProjectID = 1
                pProject.fldID = pProjectID
            End If

            pIPEDBEntities.AddTotblIPEProject(pProject)
            pIPEDBEntities.SaveChanges()
            pIPEDBEntities = Nothing
        End If

        mPN_ID = PNID_In
        mPN_Rev_ID = RevID_In
        mProject_ID = pProjectID


    End Sub


    Public Sub DeleteProject(ByVal PNID_In As Integer, ByVal RevID_In As Integer,
                             ByVal ProjectID_In As Integer)
        '=========================================================================
        Dim pIPEDBEntities As New SealIPEDBEntities
        Dim pProject = (From Project In pIPEDBEntities.tblIPEProject
                                       Where Project.fldPNID = PNID_In And
                                       Project.fldRevID = RevID_In And
                                       Project.fldID = ProjectID_In).First()

        pIPEDBEntities.DeleteObject(pProject)

        pIPEDBEntities.SaveChanges()
        pIPEDBEntities = Nothing

    End Sub


    Public Function Customer() As String
        '===============================    
        Dim pSealPartEntities As New SealPartDBEntities()

        Dim pCustomer As String = ""
        If (mCustomer_ID > 0) Then
            Dim pQry = (From pRec In pSealPartEntities.tblCustomer
                            Where pRec.fldID = mCustomer_ID Select pRec).First()
            pCustomer = pQry.fldName
        End If

        Return pCustomer

    End Function

    Public Function GetCustomer(ByVal CustID_in As Integer) As String
        '=============================================================    
        ''Dim pProjectEntities As New ProjectDBEntities()

        ''Dim pCustomer As String = ""
        ''If (CustID_in > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblCustomer
        ''                    Where pRec.fldID = CustID_in Select pRec).First()
        ''    pCustomer = pQry.fldName
        ''End If

        ''Return pCustomer

    End Function


    Public Function UnitSystem() As String
        '=================================   
        'Dim pProjectEntities As New ProjectDBEntities()
        Dim pSealPartEntities As New SealPartDBEntities()
        Dim pUnit As String = "English"

        If (mProject_ID > 0) Then
            Dim pQry = (From pRec In pSealPartEntities.tblCustomer
                            Where pRec.fldID = mCustomer_ID Select pRec).ToList()

            If (pQry.Count() > 0) Then
                Dim pstr As String = pQry(0).fldDimUnit
                pUnit = pstr.Trim()
            End If

        End If

        Return pUnit

    End Function


    Public Function Platform() As String
        '===============================    
        Dim pSealPartEntities As New SealPartDBEntities()
        Dim pPlatformName As String = ""

        If (mPlatform_ID > 0) Then
            Dim pQry = (From pRec In pSealPartEntities.tblPlatform
                            Where pRec.fldCustID = mCustomer_ID And
                                  pRec.fldID = mPlatform_ID Select pRec).First()

            pPlatformName = pQry.fldName

        End If

        Return pPlatformName

    End Function


    Public Function Location() As String
        '===============================    
        Dim pSealPartEntities As New SealPartDBEntities()
        Dim pLocation As String = ""

        If (mLocation_ID > 0) Then
            Dim pQry = (From pRec In pSealPartEntities.tblLocation
                            Where pRec.fldCustID = mCustomer_ID And
                                  pRec.fldPlatformID = mPlatform_ID And
                                  pRec.fldID = mLocation_ID Select pRec).First()

            pLocation = pQry.fldLoc
        End If

        Return pLocation

    End Function


    Public Function PN() As String
        '=========================   
        Dim pSealPartEntities As New SealPartDBEntities()
        Dim pPN As String = ""

        If (mPN_ID > 0) Then
            Dim pQry = (From pRec In pSealPartEntities.tblPN
                            Where pRec.fldID = mPN_ID Select pRec).First()

            If (pQry.fldLegacyType = 1) Then
                pPN = pQry.fldLegacy
            Else
                pPN = pQry.fldCurrent
            End If

        End If

        Return pPN

    End Function


    Public Function PN_Rev() As String
        '===============================    
        Dim pSealPartEntities As New SealPartDBEntities()
        Dim pPN_Rev As String = ""

        If (mPN_Rev_ID > 0) Then
            Dim pQry = (From pRec In pSealPartEntities.tblRev
                            Where pRec.fldPNID = mPN_ID And
                            pRec.fldID = mPN_Rev_ID Select pRec).First()

            If (pQry.fldCurrent <> "") Then
                pPN_Rev = pQry.fldCurrent
            Else
                pPN_Rev = pQry.fldLegacy
            End If


        End If

        Return pPN_Rev

    End Function


    Public Function GetPN_Rev(ByVal PNID_In As Integer, ByVal RevID_In As Integer) As String
        '===================================================================================        'AES 14SEP16   
        ''Dim pProjectEntities As New ProjectDBEntities()
        ''Dim pPN_Rev As String = ""

        ''If (RevID_In > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblRev
        ''                    Where pRec.fldPNID = PNID_In And
        ''                    pRec.fldID = RevID_In Select pRec).First()

        ''    pPN_Rev = pQry.fldNew
        ''End If

        ''Return pPN_Rev

    End Function


    Public Function PN_Rev_Date() As DateTime
        '====================================    
        ''Dim pProjectEntities As New ProjectDBEntities()
        ''Dim pPN_Rev_Date As DateTime

        ''If (mPN_Rev_ID > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblRev
        ''                    Where pRec.fldPNID = mPN_ID And
        ''                    pRec.fldID = mPN_Rev_ID Select pRec).First()

        ''    pPN_Rev_Date = pQry.fldDate
        ''End If

        ''Return pPN_Rev_Date

    End Function


    Public Function GetCustID(ByVal CustName_In As String) As Integer
        '============================================================   'AES 26AUG16
        ''Dim pProjectEntities As New ProjectDBEntities()

        ''Dim pQry = (From pRec In pProjectEntities.tblCustomer
        ''                             Where pRec.fldName = CustName_In Select pRec).First()

        ''Dim pCustID As Integer = pQry.fldID

        ''Return pCustID

    End Function


    Public Function GetPlatformID(ByVal CustID_In As Integer, ByVal PlatformName_In As String) As Integer
        '================================================================================================   'AES 26AUG16
        ''Dim pProjectEntities As New ProjectDBEntities()

        ''Dim pQry = (From pRec In pProjectEntities.tblPlatform
        ''                             Where pRec.fldCustID = CustID_In And
        ''                             pRec.fldName = PlatformName_In Select pRec).First()

        ''Dim pPlatformID As Integer = pQry.fldID

        ''Return pPlatformID

    End Function


    Public Function GetLocationID(ByVal CustID_In As Integer, ByVal PlatformID_In As Integer,
                                  ByVal LocName_In As String) As Integer
        '================================================================================   'AES 26AUG16
        ''Dim pProjectEntities As New ProjectDBEntities()

        ''Dim pQry = (From pRec In pProjectEntities.tblLocation
        ''                             Where pRec.fldCustID = CustID_In And
        ''                                   pRec.fldPlatformID = PlatformID_In And
        ''                                   pRec.fldLoc = LocName_In Select pRec).First()

        ''Dim pLocID As Integer = pQry.fldID

        ''Return pLocID

    End Function


    Public Function GetPNID(ByVal PN_In As String) As Integer
        '====================================================   
        ''Dim pProjectEntities As New ProjectDBEntities()

        ''Dim pPNID As Integer = 0
        ''Dim pQryCount As Integer = (From pRec In pProjectEntities.tblPN
        ''                             Where pRec.fldNew = PN_In Select pRec).Count()
        ''If (pQryCount > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblPN
        ''                             Where pRec.fldNew = PN_In Select pRec).First()
        ''    pPNID = pQry.fldID

        ''Else
        ''    'AES 21MAR17
        ''    pQryCount = (From pRec In pProjectEntities.tblPN
        ''                             Where pRec.fldLegacy = PN_In Select pRec).Count()
        ''    If (pQryCount > 0) Then
        ''        Dim pQry = (From pRec In pProjectEntities.tblPN
        ''                                 Where pRec.fldLegacy = PN_In Select pRec).First()
        ''        pPNID = pQry.fldID
        ''    End If

        ''End If

        ''Return pPNID

    End Function


    Public Function GetRevID(ByVal PNID_In As Integer, ByVal Rev_In As String) As Integer
        '================================================================================   'AES 26AUG16
        ''Dim pProjectEntities As New ProjectDBEntities()
        ''Dim pRevID As Integer = 0

        ''Dim pQryCount = (From pRec In pProjectEntities.tblRev
        ''                             Where pRec.fldPNID = PNID_In And
        ''                                   pRec.fldNew = Rev_In Select pRec).Count()
        ''If (pQryCount > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblRev
        ''                            Where pRec.fldPNID = PNID_In And
        ''                                  pRec.fldNew = Rev_In Select pRec).First()
        ''    pRevID = pQry.fldID
        ''Else
        ''    'AES 21MAR17
        ''    pQryCount = (From pRec In pProjectEntities.tblRev
        ''                             Where pRec.fldPNID = PNID_In And
        ''                                   pRec.fldLegacy = Rev_In Select pRec).Count()

        ''    If (pQryCount > 0) Then
        ''        Dim pQry = (From pRec In pProjectEntities.tblRev
        ''                                Where pRec.fldPNID = PNID_In And
        ''                                      pRec.fldLegacy = Rev_In Select pRec).First()
        ''        pRevID = pQry.fldID
        ''    End If

        ''End If

        ''Return pRevID

    End Function


    Public Function GetID() As Integer
        '==============================     'AES 25AUG16
        ''Dim pProjectEntities As New ProjectDBEntities()
        ''Dim pID As Integer = 0

        ''If (mCustomer_ID <> 0 And mPlatform_ID <> 0 And mLocation_ID <> 0 And mPN_ID <> 0 And mPN_Rev_ID <> 0) Then
        ''    Dim pQryCount As Integer = (From pRec In pProjectEntities.tblProject
        ''                             Where pRec.fldCustID = mCustomer_ID And
        ''                                   pRec.fldPlatformID = mPlatform_ID And
        ''                                   pRec.fldLocID = mLocation_ID And
        ''                                   pRec.fldPNID = mPN_ID And
        ''                                   pRec.fldRevID = mPN_Rev_ID Select pRec).Count()
        ''    If (pQryCount > 0) Then
        ''        Dim pQry1 = (From pRec In pProjectEntities.tblProject
        ''                             Where pRec.fldCustID = mCustomer_ID And
        ''                                   pRec.fldPlatformID = mPlatform_ID And
        ''                                   pRec.fldLocID = mLocation_ID And
        ''                                   pRec.fldPNID = mPN_ID And
        ''                                   pRec.fldRevID = mPN_Rev_ID Select pRec).First()
        ''        pID = pQry1.fldID
        ''    End If

        ''End If

        ''Return pID

    End Function


    Public Function CustomerPN() As String
        '==================================    
        ''Dim pProjectEntities As New ProjectDBEntities()
        ''Dim pCustomerPN As String = ""

        ''If (mProject_ID > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblProject
        ''                    Where pRec.fldCustID = mCustomer_ID And
        ''                          pRec.fldPlatformID = mPlatform_ID And
        ''                          pRec.fldLocID = mLocation_ID And
        ''                          pRec.fldPNID = mPN_ID And
        ''                          pRec.fldRevID = mPN_Rev_ID And
        ''                          pRec.fldID = mProject_ID Select pRec).First()

        ''    pCustomerPN = pQry.fldPN_Cust
        ''End If

        ''Return pCustomerPN

    End Function


    Public Function ParkerPN() As String
        '===============================
        Dim pParkerPN As String = ""
        If (PN_Rev() = "0") Then
            pParkerPN = PN()
        Else
            pParkerPN = PN() + "-" + PN_Rev()
        End If

        Return pParkerPN

    End Function


    Public Function GetParkerPN(ByVal PNID_In As Integer) As String
        '==========================================================   'AES 14SEP16
        ''Dim pProjectEntities As New ProjectDBEntities()
        ''Dim pPN As String = ""

        ''If (PNID_In > 0) Then
        ''    Dim pQry = (From pRec In pProjectEntities.tblPN
        ''                    Where pRec.fldID = PNID_In Select pRec).First()

        ''    pPN = pQry.fldNew
        ''End If

        ''Return pPN

    End Function


    Public Sub Add_Analysis(Optional ByVal iCopy_In As Integer = -1)
        '============================================================
        '....W/o argument New Analysis
        '....W/ argument Copy existing Analysis

        If (iCopy_In = -1) Then                     '....New Analysis
            Dim pAnalysis As New IPE_clsAnalysis(Me)
            mAnalysis.Add(pAnalysis)

        Else                                        '....Copy Existing Analysis 
            mAnalysis.Add(mAnalysis(iCopy_In).Clone())
        End If

    End Sub


    Public Sub Reset_Analysis()
        '======================
        mAnalysis.Clear()
    End Sub


    Public Function SealType() As String
        '===============================

        Dim pParkerPN As String = ParkerPN()

        Dim pSealPartEntities As New SealPartDBEntities()
        Dim pQry = (From pRec In pSealPartEntities.tblPN
                          Where pRec.fldID = mPN_ID Select pRec).First()

        Dim pSealType As String = ""
        If (pQry.fldLegacyType = 0) Then

            Dim pMCS As String = pParkerPN.Substring(3, pParkerPN.Length - 3)
            Dim pPrefix As Integer = ConvertToInt(pMCS.Substring(0, 2))

            If (pMCS.Length > 2) Then

                Select Case (pPrefix)

                    Case 69
                        pSealType = "E-Seal"

                    Case 76
                        pSealType = "C-Seal"

                    Case 79
                        pSealType = "U-Seal"

                End Select
            End If

        ElseIf (pQry.fldLegacyType = 1) Then

            Dim pSealPrefix As String = pParkerPN.Substring(1, 1)

            Select Case (pSealPrefix)
                Case "E"
                    pSealType = "E-Seal"

                Case "C"
                    pSealType = "C-Seal"

                Case "U"
                    pSealType = "U-Seal"

            End Select

        End If

        Return pSealType

    End Function


    Private Sub SetCulture()
        '===================
        '....Set Culture of the application

        Dim pName As String = ""

        Select Case (mCulture)

            Case "USA"
                pName = "en-US"

            Case "UK"
                pName = "en-GB"

            Case "Germany"
                pName = "de-DE"

            Case "France"
                pName = "fr-FR"

        End Select

        Thread.CurrentThread.CurrentCulture = New CultureInfo(pName)

    End Sub


#End Region

#Region "DATABASE RELATED ROUTINES:"

    Public Sub Save_ToDB(ByVal Unit_In As IPE_clsUnit, ByVal ANSYS_In As IPE_clsANSYS)
        '======================================================================
        Try


            ' ''....Save to tblCustomer
            ''Dim pProjectEntities As New ProjectDBEntities
            ''Dim pCustomer As New tblCustomer

            ''Dim pRecCount As Integer = (From Customer In pProjectEntities.tblCustomer).Count()
            ''Dim pCustomerID As Integer = 1
            ''Dim pIsRecExists_Cust As Boolean = False
            ''If (pRecCount > 0) Then
            ''    Dim pCustomerRec = (From Customer In pProjectEntities.tblCustomer
            ''                       Where Customer.fldName = mCustName Select Customer).ToList()
            ''    If (pCustomerRec.Count() > 0) Then
            ''        pCustomerID = pCustomerRec(0).fldID
            ''        pIsRecExists_Cust = True
            ''        pCustomer = pCustomerRec(0)
            ''    Else

            ''        Dim pCustomer_Out = (From Customer In pProjectEntities.tblCustomer
            ''                            Order By Customer.fldID Descending).First()

            ''        pCustomerID = pCustomer_Out.fldID + 1
            ''    End If

            ''End If

            ''pCustomer.fldID = pCustomerID
            ''pCustomer.fldName = mCustName
            ''pCustomer.fldUnit = mUnit
            ''pCustomer.fldIndex = pCustomerID

            ''If (Not pIsRecExists_Cust) Then
            ''    pProjectEntities.AddTotblCustomer(pCustomer)
            ''End If

            ''pProjectEntities.SaveChanges()


            ' ''....Save to tblPlatform
            ''Dim pPlatform As New tblPlatform
            ''Dim pPlatID As Integer = 1
            ''Dim pIsRecExists_Plat As Boolean = False

            ''Dim pPlatRec = (From Platform In pProjectEntities.tblPlatform
            ''                       Where Platform.fldCustID = pCustomerID And Platform.fldName = mPlatName Select Platform).ToList()
            ''If (pPlatRec.Count() > 0) Then
            ''    pPlatID = pPlatRec(0).fldID
            ''    pIsRecExists_Plat = True
            ''    pPlatform = pPlatRec(0)
            ''Else

            ''    Dim pRec = (From Platform In pProjectEntities.tblPlatform
            ''                 Where Platform.fldCustID = pCustomerID Order By Platform.fldID Descending).ToList()
            ''    If (pRec.Count() > 0) Then
            ''        pPlatID = pRec(0).fldID + 1
            ''    End If

            ''End If
            ''pPlatform.fldCustID = pCustomerID
            ''pPlatform.fldID = pPlatID
            ''pPlatform.fldName = mPlatName
            ''pPlatform.fldIndex = pPlatID

            ''If (Not pIsRecExists_Plat) Then
            ''    pProjectEntities.AddTotblPlatform(pPlatform)
            ''End If
            ''pProjectEntities.SaveChanges()

            ' ''....Save to tblLocation
            ''Dim pLocation As New tblLocation
            ''Dim pLocID As Integer = 1
            ''Dim pIsRecExists_Loc As Boolean = False

            ''Dim pLocRec = (From Location In pProjectEntities.tblLocation
            ''                       Where Location.fldCustID = pCustomerID And Location.fldPlatformID = pPlatID And Location.fldLoc = mLocName Select Location).ToList()
            ''If (pLocRec.Count() > 0) Then
            ''    pLocID = pLocRec(0).fldID
            ''    pIsRecExists_Loc = True
            ''    pLocation = pLocRec(0)
            ''Else

            ''    Dim pRec = (From Location In pProjectEntities.tblLocation
            ''                 Where Location.fldCustID = pCustomerID And Location.fldPlatformID = pPlatID Order By Location.fldID Descending).ToList()

            ''    If (pRec.Count() > 0) Then
            ''        pLocID = pRec(0).fldID + 1
            ''    End If

            ''End If

            ''pLocation.fldCustID = pCustomerID
            ''pLocation.fldPlatformID = pPlatID
            ''pLocation.fldID = pLocID
            ''pLocation.fldLoc = mLocName
            ''pLocation.fldIndex = pLocID

            ''If (Not pIsRecExists_Loc) Then
            ''    pProjectEntities.AddTotblLocation(pLocation)
            ''End If
            ''pProjectEntities.SaveChanges()

            ' ''....Save to tblPN
            ''Dim pPN As New tblPN
            ''Dim pPNID As Integer = 1
            ''Dim pIsRecExists_PN As Boolean = False

            ''Dim pPNRec = (From PN In pProjectEntities.tblPN
            ''                       Where PN.fldNew = mPN_No Select PN).ToList()
            ''If (pPNRec.Count() > 0) Then
            ''    pPNID = pPNRec(0).fldID
            ''    pIsRecExists_PN = True
            ''    pPN = pPNRec(0)
            ''Else

            ''    Dim pRec = (From PN In pProjectEntities.tblPN
            ''                 Order By PN.fldID Descending).ToList()
            ''    If (pRec.Count() > 0) Then
            ''        pPNID = pRec(0).fldID + 1
            ''    End If

            ''End If

            ''pPN.fldID = pPNID
            ''pPN.fldNew = mPN_No

            ''If (Not pIsRecExists_PN) Then
            ''    pProjectEntities.AddTotblPN(pPN)
            ''End If
            ''pProjectEntities.SaveChanges()

            ' ''....Save to tblRev
            ''Dim pRev As New tblRev
            ''Dim pRevID As Integer = 1
            ''Dim pIsRecExists_Rev As Boolean = False

            ''Dim pRevRec = (From Rev In pProjectEntities.tblRev
            ''                       Where Rev.fldPNID = pPNID And Rev.fldNew = mPN_Rev_Name Select Rev).ToList()
            ''If (pRevRec.Count() > 0) Then
            ''    pRevID = pRevRec(0).fldID
            ''    pIsRecExists_Rev = True
            ''    pRev = pRevRec(0)
            ''Else

            ''    Dim pRec = (From Rev In pProjectEntities.tblRev
            ''                Where Rev.fldPNID = pPNID Order By Rev.fldID Descending).ToList()
            ''    If (pRec.Count() > 0) Then
            ''        pRevID = pRec(0).fldID + 1
            ''    End If

            ''End If

            ''pRev.fldPNID = pPNID
            ''pRev.fldID = pRevID
            ''pRev.fldNew = mPN_Rev_Name

            ''If (Not pIsRecExists_Rev) Then
            ''    pProjectEntities.AddTotblRev(pRev)
            ''End If
            ''pProjectEntities.SaveChanges()


            ' ''....Save to tblProject
            ''Dim pIsRecExists_Project As Boolean = False
            ''Dim pProject As New tblProject()
            ''Dim pProject_ID As Integer = 1
            ''Dim pProjectRec = (From Project In pProjectEntities.tblProject
            ''                      Where Project.fldCustID = pCustomerID And Project.fldPlatformID = pPlatID _
            ''                      And Project.fldLocID = pLocID And Project.fldPNID = pPNID And Project.fldRevID = pRevID Select Project).ToList()

            ''If (pProjectRec.Count() > 0) Then
            ''    pProject_ID = pProjectRec(0).fldID
            ''    pIsRecExists_Project = True
            ''    pProject = pProjectRec(0)
            ''Else

            ''    Dim pRec = (From Project In pProjectEntities.tblProject
            ''                Order By Project.fldID Descending).ToList()
            ''    If (pRec.Count() > 0) Then
            ''        pProject_ID = pRec(0).fldID + 1
            ''    End If

            ''End If

            ''pProject.fldCustID = pCustomerID
            ''pProject.fldPlatformID = pPlatID
            ''pProject.fldLocID = pLocID
            ''pProject.fldPNID = pPNID
            ''pProject.fldRevID = pRevID
            ''pProject.fldID = pProject_ID
            ''pProject.fldPN_Cust = mPN_Cust

            ''mProject_ID = pProject_ID

            ''If (Not pIsRecExists_Project) Then
            ''    pProjectEntities.AddTotblProject(pProject)
            ''End If
            ''pProjectEntities.SaveChanges()


            ' ''....Save to tblAnalysis
            ''mCustomer_ID = pCustomerID
            ''mPlatform_ID = pPlatID
            ''mLocation_ID = pLocID
            ''mPN_ID = pPNID
            ''mPN_Rev_ID = pRevID

            ''For i As Integer = 0 To mAnalysis.Count - 1
            ''    Dim pAnalysis As New clsAnalysis(Me)
            ''    pAnalysis.ID = i + 1
            ''    pAnalysis.Cavity = mAnalysis(i).Cavity.Clone()
            ''    pAnalysis.OpCond = mAnalysis(i).OpCond.Clone()
            ''    pAnalysis.Seal = mAnalysis(i).Seal.Clone()
            ''    pAnalysis.ANSYS = mAnalysis(i).ANSYS.Clone()
            ''    pAnalysis.CompressionTolType = mAnalysis(i).Compression.TolType
            ''    pAnalysis.LoadCaseName = mAnalysis(i).LoadCase.Name
            ''    pAnalysis.DateCreated = DateTime.Now()
            ''    pAnalysis.TimeCreated = DateTime.Now()
            ''    pAnalysis.Save_ToDB(Unit_In, ANSYS_In)

            ''Next



        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "SERIALIZE-DESERIALIZE:"

    Public Function SaveData_Serialize(FilePath_In As String) As Boolean
        '===============================================================
        Try
            Dim serializer As IFormatter = New BinaryFormatter()
            Dim pFileName As String = FilePath_In & "1.SealIPE"

            Dim saveFile As New FileStream(pFileName, FileMode.Create, FileAccess.Write)

            serializer.Serialize(saveFile, Me)

            saveFile.Close()

            Return True
        Catch
            Return False
        End Try
    End Function


    Public Function RestoreData_Deserialize(FilePath_In As String) As Object
        '===================================================================
        Dim serializer As IFormatter = New BinaryFormatter()
        Dim pFileName As String = FilePath_In & "1.SealIPE"
        Dim openFile As New FileStream(pFileName, FileMode.Open, FileAccess.Read)
        Dim pObj As Object
        pObj = serializer.Deserialize(openFile)

        openFile.Close()

        Return pObj

    End Function

#End Region

#Region "CLONE METHOD"

    '   DEEP CLONING:
    '   -------------
    '
    Public Function Clone() As Object Implements ICloneable.Clone
        '========================================================
        '....Inherited from the ICloneable interface, supports deep cloning
        Try


            Dim pMemBuffer As New MemoryStream()
            Dim pBinSerializer As New BinaryFormatter(Nothing, _
                              New StreamingContext(StreamingContextStates.Clone))


            '....Serialize the object into the memory stream
            pBinSerializer.Serialize(pMemBuffer, Me)

            '....Move the stream pointer to the beginning of the memory stream
            pMemBuffer.Seek(0, SeekOrigin.Begin)

            '....Get the serialized object from the memory stream
            Dim pobjClone As Object
            pobjClone = pBinSerializer.Deserialize(pMemBuffer)

            pMemBuffer.Close()      '....Release the memory stream
            Return pobjClone    '....Return the deeply cloned object

        Catch ex As Exception

        End Try

    End Function

#End Region

End Class
