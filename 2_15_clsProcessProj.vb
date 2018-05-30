'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj                         '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29DEC17                                '
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

<Serializable()> _
Public Class clsProcessProj
    Implements ICloneable

#Region "MEMBER VARIABLES:"

    Private mID As Integer
    Private mPOPCoding As String
    Private mRating As String
    Private mType As String
    Private mDateOpen As Date
    Private mDateLastModified As Date
    Private mDateClose As Date
    Private mLastModifiedBy As String

    '....Local Object.
    Private mPartProject As New clsPartProject
    Private mUnit As New clsProcessProj_Unit

    Private mPreOrder As New clsProcessProj_PreOrder
    Private mITAR_Export As New clsProcessProj_ITAR_Export
    Private mOrdEntry As New clsProcessProj_OrdEntry
    Private mCost As New clsProcessProj_Cost
    Private mApp As New clsProcessProj_App
    Private mDesign As New clsProcessProj_Design
    Private mManf As New clsProcessProj_Manf
    Private mPurchase As New clsProcessProj_Purchase
    Private mQlty As New clsProcessProj_Qlty
    Private mDwg As New clsProcessProj_Dwg
    Private mTest As New clsProcessProj_Test
    Private mPlanning As New clsProcessProj_Planning
    Private mShipping As New clsProcessProj_Shipping
    Private mIssueComnt As New clsProcessProj_IssueComment
    Private mApproval As New clsProcessProj_Approval
    Private mEditedBy As New clsProcessProj_EditedBy
    Private mRiskAna As New clsProcessProj_Risk

    Private mCustContact As New clsCustContact

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New(ByVal PartProject_In As clsPartProject)
        '==================================================
        mPartProject = PartProject_In.Clone()

    End Sub
#End Region

#Region "PROPERTY ROUTINES:"

    '....ID
    Public Property ID() As Integer
        '============================
        Get
            Return mID
        End Get

        Set(ByVal intData As Integer)
            '-------------------------------
            mID = intData
        End Set

    End Property

    '....Unit
    Public Property Unit() As clsProcessProj_Unit
        '========================================
        Get
            Return mUnit
        End Get

        Set(Obj As clsProcessProj_Unit)
            mUnit = Obj
        End Set

    End Property

    '....POPCoding
    Public Property POPCoding() As String
        '=================================
        Get
            Return mPOPCoding
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mPOPCoding = strData
        End Set

    End Property

    '....Rating
    Public Property Rating() As String
        '=================================
        Get
            Return mRating
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mRating = strData
        End Set

    End Property

    '....Type
    Public Property Type() As String
        '=================================
        Get
            Return mType
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mType = strData
        End Set

    End Property

    '....DateOpen
    Public Property DateOpen() As Date
        '=============================
        Get
            Return mDateOpen
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDateOpen = strData
        End Set

    End Property

    '....DateLastModified
    Public Property DateLastModified() As Date
        '=====================================
        Get
            Return mDateLastModified
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDateLastModified = strData
        End Set

    End Property

    '....DateClose
    Public Property DateClose() As Date
        '==============================
        Get
            Return mDateClose
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDateClose = strData
        End Set

    End Property

    '....LastModifiedBy
    Public Property LastModifiedBy() As String
        '=====================================
        Get
            Return mLastModifiedBy
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mLastModifiedBy = strData
        End Set

    End Property


    '....PreOrder
    Public Property PreOrder() As clsProcessProj_PreOrder
        '================================================
        Get
            Return mPreOrder
        End Get

        Set(ByVal value As clsProcessProj_PreOrder)
            mPreOrder = value
        End Set

    End Property

    '....ITAR_Export
    Public Property ITAR_Export() As clsProcessProj_ITAR_Export
        '======================================================
        Get
            Return mITAR_Export
        End Get

        Set(ByVal value As clsProcessProj_ITAR_Export)
            mITAR_Export = value
        End Set

    End Property

    '....OrdEntry
    Public Property OrdEntry() As clsProcessProj_OrdEntry
        '================================================
        Get
            Return mOrdEntry
        End Get

        Set(ByVal value As clsProcessProj_OrdEntry)
            mOrdEntry = value
        End Set

    End Property

    '....Cost
    Public Property Cost() As clsProcessProj_Cost
        '========================================
        Get
            Return mCost
        End Get

        Set(ByVal value As clsProcessProj_Cost)
            mCost = value
        End Set

    End Property

    '....App
    Public Property App() As clsProcessProj_App
        '======================================
        Get
            Return mApp
        End Get

        Set(ByVal value As clsProcessProj_App)
            mApp = value
        End Set

    End Property

    '....Design
    Public Property Design() As clsProcessProj_Design
        '============================================
        Get
            Return mDesign
        End Get

        Set(ByVal value As clsProcessProj_Design)
            mDesign = value
        End Set

    End Property

    '....Manufacturing
    Public Property Manf() As clsProcessProj_Manf
        '========================================
        Get
            Return mManf
        End Get

        Set(ByVal value As clsProcessProj_Manf)
            mManf = value
        End Set

    End Property

    '....Purchase
    Public Property Purchase() As clsProcessProj_Purchase
        '================================================
        Get
            Return mPurchase
        End Get

        Set(ByVal value As clsProcessProj_Purchase)
            mPurchase = value
        End Set

    End Property

    '....Qlty
    Public Property Qlty() As clsProcessProj_Qlty
        '========================================
        Get
            Return mQlty
        End Get

        Set(ByVal value As clsProcessProj_Qlty)
            mQlty = value
        End Set

    End Property


    '....Dwg
    Public Property Dwg() As clsProcessProj_Dwg
        '======================================
        Get
            Return mDwg
        End Get

        Set(ByVal value As clsProcessProj_Dwg)
            mDwg = value
        End Set

    End Property

    '....Test
    Public Property Test() As clsProcessProj_Test
        '=======================================
        Get
            Return mTest
        End Get

        Set(ByVal value As clsProcessProj_Test)
            mTest = value
        End Set

    End Property

    '....Planning
    Public Property Planning() As clsProcessProj_Planning
        '================================================
        Get
            Return mPlanning
        End Get

        Set(ByVal value As clsProcessProj_Planning)
            mPlanning = value
        End Set

    End Property

    '....Shipping
    Public Property Shipping() As clsProcessProj_Shipping
        '================================================
        Get
            Return mShipping
        End Get

        Set(ByVal value As clsProcessProj_Shipping)
            mShipping = value
        End Set

    End Property


    '....IssueComnt
    Public Property IssueCommnt() As clsProcessProj_IssueComment
        '=======================================================
        Get
            Return mIssueComnt
        End Get

        Set(ByVal value As clsProcessProj_IssueComment)
            mIssueComnt = value
        End Set

    End Property


    '....Approval
    Public Property Approval() As clsProcessProj_Approval
        '================================================
        Get
            Return mApproval
        End Get

        Set(ByVal value As clsProcessProj_Approval)
            mApproval = value
        End Set

    End Property

    '....EditedBy
    Public Property EditedBy() As clsProcessProj_EditedBy
        '================================================
        Get
            Return mEditedBy
        End Get

        Set(ByVal value As clsProcessProj_EditedBy)
            mEditedBy = value
        End Set

    End Property


    '....Risk Ana
    Public Property RiskAna() As clsProcessProj_Risk
        '================================================
        Get
            Return mRiskAna
        End Get

        Set(ByVal value As clsProcessProj_Risk)
            mRiskAna = value
        End Set

    End Property

#Region "CustContact"

    Public Property CustContact() As clsCustContact
        '===========================================
        Get
            Return mCustContact
        End Get

        Set(ByVal strObj As clsCustContact)
            '-------------------------------
            mCustContact = strObj
        End Set

    End Property

#End Region

#End Region

#Region "DB RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal PNID_In As Integer, ByVal RevID_In As Integer)
        '=========================================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblProcessProject
        Dim pQryProcessProjectCount As Integer = (From pRec In pSealProcessDBEntities.tblProcessProject
                               Where pRec.fldPartProjectID = mPartProject.Project_ID Select pRec).Count()

        If (pQryProcessProjectCount > 0) Then

            Dim pQryProcessProject = (From pRec In pSealProcessDBEntities.tblProcessProject
                               Where pRec.fldPartProjectID = mPartProject.Project_ID Select pRec).First()

            mID = pQryProcessProject.fldID
            If (Not IsNothing(pQryProcessProject.fldPOPCoding) And Not IsDBNull(pQryProcessProject.fldPOPCoding)) Then
                mPOPCoding = pQryProcessProject.fldPOPCoding.Trim()
            End If

            If (Not IsNothing(pQryProcessProject.fldRating) And Not IsDBNull(pQryProcessProject.fldRating)) Then
                mRating = pQryProcessProject.fldRating.Trim()
            End If

            If (Not IsNothing(pQryProcessProject.fldType) And Not IsDBNull(pQryProcessProject.fldType)) Then
                mType = pQryProcessProject.fldType.Trim()
            End If


            If (Not IsNothing(pQryProcessProject.fldDateOpen) And Not IsDBNull(pQryProcessProject.fldDateOpen)) Then
                mDateOpen = pQryProcessProject.fldDateOpen
            End If

            If (Not IsNothing(pQryProcessProject.fldDateLastModified) And Not IsDBNull(pQryProcessProject.fldDateLastModified)) Then
                mDateLastModified = pQryProcessProject.fldDateLastModified
            End If

            If (Not IsNothing(pQryProcessProject.fldDateClose) And Not IsDBNull(pQryProcessProject.fldDateClose)) Then
                mDateClose = pQryProcessProject.fldDateClose
            End If

            If (Not IsNothing(pQryProcessProject.fldLastModifiedBy) And Not IsDBNull(pQryProcessProject.fldLastModifiedBy)) Then
                mLastModifiedBy = pQryProcessProject.fldLastModifiedBy.Trim()
            End If



        Else
            mID = 0
            mPOPCoding = ""
            mRating = ""
            mType = ""
            mDateOpen = DateTime.MinValue
            mDateLastModified = DateTime.MinValue
            mDateClose = DateTime.MinValue
            mLastModifiedBy = ""
        End If

    End Sub


    Public Sub SaveToDB(ByVal PNID_In As Integer, ByVal RevID_In As Integer)
        '===================================================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....ProcessProject table
        Dim pProcessProjectCount As Integer = (From ProcessProjct In pSealProcessDBEntities.tblProcessProject
                                               Where ProcessProjct.fldPartProjectID = mPartProject.Project_ID And
                                            ProcessProjct.fldID = mID Select ProcessProjct).Count()

        If (pProcessProjectCount > 0) Then
            '....Record already exists
            Dim pProcessProject = (From ProcessProjct In pSealProcessDBEntities.tblProcessProject
                                   Where ProcessProjct.fldPartProjectID = mPartProject.Project_ID And
                                            ProcessProjct.fldID = mID Select ProcessProjct).First()

            pProcessProject.fldPOPCoding = mPOPCoding
            'pProcessProject.fldGovt = mGovt
            pProcessProject.fldRating = mRating
            pProcessProject.fldType = mType
            pProcessProject.fldDateOpen = mDateOpen
            pProcessProject.fldDateClose = mDateClose
            pProcessProject.fldDateLastModified = mDateLastModified
            pProcessProject.fldLastModifiedBy = mLastModifiedBy

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = 1
            Dim pProcessProject As New tblProcessProject
            Dim pProcessProjectRec = (From ProcessProjct In pSealProcessDBEntities.tblProcessProject
                                      Order By ProcessProjct.fldID Descending Select ProcessProjct).ToList()
            If (pProcessProjectRec.Count > 0) Then
                pID = pProcessProjectRec(0).fldID + 1
            End If
            'pProcessProject.fldPNID = PNID_In
            'pProcessProject.fldRevID = RevID_In
            pProcessProject.fldPartProjectID = mPartProject.Project_ID
            pProcessProject.fldID = pID
            pProcessProject.fldPOPCoding = mPOPCoding
            'pProcessProject.fldGovt = mGovt
            pProcessProject.fldRating = mRating
            pProcessProject.fldType = mType
            pProcessProject.fldDateOpen = mDateOpen
            pProcessProject.fldDateClose = mDateClose
            pProcessProject.fldDateLastModified = mDateLastModified
            pProcessProject.fldLastModifiedBy = mLastModifiedBy

            pSealProcessDBEntities.AddTotblProcessProject(pProcessProject)
            pSealProcessDBEntities.SaveChanges()
            mID = pID
        End If

    End Sub

#End Region


#Region "CLONE METHOD"

    '   DEEP CLONING:
    '   -------------
    '
    Public Function Clone() As Object Implements ICloneable.Clone
        '========================================================

        '....Inherited from the ICloneable interface, supports deep cloning
        Dim pobjClone As Object
        Try

            Dim pMemBuffer As New MemoryStream()
            Dim pBinSerializer As New BinaryFormatter(Nothing, _
                                  New StreamingContext(StreamingContextStates.Clone))


            '....Serialize the object into the memory stream
            pBinSerializer.Serialize(pMemBuffer, Me)

            '....Move the stream pointer to the beginning of the memory stream
            pMemBuffer.Seek(0, SeekOrigin.Begin)

            '....Get the serialized object from the memory stream

            pobjClone = pBinSerializer.Deserialize(pMemBuffer)

            pMemBuffer.Close()      '....Release the memory stream



        Catch ex As Exception

        End Try
        Return pobjClone    '....Return the deeply cloned object

    End Function

#End Region


#Region "Class CustContact"
    '-----------------------------
    <Serializable()> _
    Public Class clsCustContact

#Region "MEMBER VARIABLES:"

        Private mID_Cust As New List(Of Integer)
        Private mDeptName As New List(Of String)
        Private mName As New List(Of String)
        Private mPhone As New List(Of String)
        Private mEmail As New List(Of String)
#End Region

#Region "PROPERTY ROUTINES:"

        '....ID
        Public Property ID_Cust() As List(Of Integer)
            '=====================================
            Get
                Return mID_Cust
            End Get

            Set(Obj As List(Of Integer))
                mID_Cust = Obj
            End Set
        End Property

        '....DeptName
        Public Property DeptName() As List(Of String)
            '=======================================
            Get
                Return mDeptName
            End Get

            Set(Obj As List(Of String))
                mDeptName = Obj
            End Set
        End Property

        '....Name
        Public Property Name() As List(Of String)
            '=====================================
            Get
                Return mName
            End Get

            Set(Obj As List(Of String))
                mName = Obj
            End Set
        End Property

        '....Phone
        Public Property Phone() As List(Of String)
            '=====================================
            Get
                Return mPhone
            End Get

            Set(Obj As List(Of String))
                mPhone = Obj
            End Set
        End Property

        '....Email
        Public Property Email() As List(Of String)
            '=====================================
            Get
                Return mEmail
            End Get

            Set(Obj As List(Of String))
                mEmail = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblPreOrder_CustContact
                Dim pQryCustContactCount As Integer = (From pRec In pSealProcessDBEntities.tblPreOrder_CustContact
                                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryCustContactCount > 0) Then

                    Dim pQryCustContact = (From pRec In pSealProcessDBEntities.tblPreOrder_CustContact
                                           Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryCustContact.Count - 1
                        mID_Cust.Add(pQryCustContact(i).fldID)
                        mDeptName.Add(pQryCustContact(i).fldDept)
                        mName.Add(pQryCustContact(i).fldName)
                        mPhone.Add(pQryCustContact(i).fldPhone)
                        mEmail.Add(pQryCustContact(i).fldEmail)
                    Next

                Else
                    mID_Cust.Clear()
                    mDeptName.Clear()
                    mName.Clear()
                    mPhone.Clear()
                    mEmail.Clear()

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pCustContact As New tblPreOrder_CustContact

            Dim pQryCustContact = (From PreOrder In pSealProcessDBEntities.tblPreOrder_CustContact
                                   Where PreOrder.fldProcessProjectID = ProjectID_In Select PreOrder).ToList()

            If (pQryCustContact.Count > 0) Then
                For j As Integer = 0 To pQryCustContact.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryCustContact(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pPreOrderCustContact As New List(Of tblPreOrder_CustContact)

            For j As Integer = 0 To mID_Cust.Count - 1
                Dim PreOrderCust As New tblPreOrder_CustContact
                pPreOrderCustContact.Add(PreOrderCust)
                With pPreOrderCustContact(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDept = mDeptName(j)
                    .fldName = mName(j)
                    .fldPhone = mPhone(j)
                    .fldEmail = mEmail(j)

                End With

                pSealProcessDBEntities.AddTotblPreOrder_CustContact(pPreOrderCustContact(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub


#End Region


    End Class

#End Region

End Class
