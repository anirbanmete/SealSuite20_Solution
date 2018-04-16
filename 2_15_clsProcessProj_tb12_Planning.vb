'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsPlanning                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28DEC17                                '
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

<Serializable()> _
Public Class clsProcessProj_Planning

#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sUser
        Public Name As String
        Public Signed As Boolean
        Public SignedDate As Date
    End Structure

#End Region

#Region "MEMBER VARIABLES:"

    Private mNotes As String
    Private mUser As sUser

    Private mSplOperation As New clsSplOperation
    Private mMileOperation As New clsMileOperation

#End Region

#Region "PROPERTY ROUTINES:"

    '....Notes
    Public Property Notes() As String
        '============================
        Get
            Return mNotes
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mNotes = strData
        End Set

    End Property

    '....User
    Public ReadOnly Property User() As sUser
        '====================================     
        Get
            Return mUser
        End Get
    End Property

    '....User_Name
    Public WriteOnly Property User_Name() As String
        '==========================================
        Set(ByVal strVal As String)
            mUser.Name = strVal
        End Set
    End Property

    '....User_Signed
    Public WriteOnly Property User_Signed() As Boolean
        '===============================================
        Set(ByVal blnVal As Boolean)
            mUser.Signed = blnVal
        End Set
    End Property

    '....User_SignedDate
    Public WriteOnly Property User_SignedDate() As Date
        '===============================================
        Set(ByVal strVal As Date)
            mUser.SignedDate = strVal
        End Set
    End Property

#Region "SplOperation"

    Public Property SplOperation() As clsSplOperation
        '=================================
        Get
            Return mSplOperation
        End Get

        Set(ByVal strObj As clsSplOperation)
            '-------------------------------
            mSplOperation = strObj
        End Set

    End Property

#End Region

#Region "MileOperation"

    Public Property MileOperation() As clsMileOperation
        '===========================
        Get
            Return mMileOperation
        End Get

        Set(ByVal strObj As clsMileOperation)
            '-------------------------------
            mMileOperation = strObj
        End Set

    End Property

#End Region

#End Region

#Region "DATABASE RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '=====================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try

            '....tblPlanning
            Dim pQryPlanningCount As Integer = (From pRec In pSealProcessDBEntities.tblPlanning
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryPlanningCount > 0) Then

                Dim pQryPlanning = (From pRec In pSealProcessDBEntities.tblPlanning
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mNotes = pQryPlanning.fldNotes
                'mUser.Name = pQryPlanning.fldUserName
                'mUser.Signed = pQryPlanning.fldUserSigned

                'If (Not IsNothing(pQryPlanning.fldUserDate) And Not IsDBNull(pQryPlanning.fldUserDate)) Then
                '    mUser.SignedDate = pQryPlanning.fldUserDate
                'End If

            End If

            mSplOperation.RetrieveFromDB(ProjectID_In)
            mMileOperation.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblPlanning
        Dim pPlanningCount As Integer = (From Planning In pSealProcessDBEntities.tblPlanning
                                            Where Planning.fldProcessProjectID = ProjectID_In Select Planning).Count()

        If (pPlanningCount > 0) Then
            '....Record already exists
            Dim pPlanning = (From Planning In pSealProcessDBEntities.tblPlanning
                                           Where Planning.fldProcessProjectID = ProjectID_In Select Planning).First()

            pPlanning.fldNotes = mNotes
            'pPlanning.fldUserName = mUser.Name
            'pPlanning.fldUserSigned = mUser.Signed
            'pPlanning.fldUserDate = mUser.SignedDate

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pPlanning As New tblPlanning
            pPlanning.fldProcessProjectID = pID
            pPlanning.fldNotes = mNotes
            'pPlanning.fldUserName = mUser.Name
            'pPlanning.fldUserSigned = mUser.Signed
            'pPlanning.fldUserDate = mUser.SignedDate

            pSealProcessDBEntities.AddTotblPlanning(pPlanning)
            pSealProcessDBEntities.SaveChanges()
        End If

        mSplOperation.SaveToDB(ProjectID_In)
        mMileOperation.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "clsSplOperation:"

    <Serializable()> _
    Public Class clsSplOperation

#Region "MEMBER VARIABLES:"

        Private mID_SplOperation As New List(Of Integer)
        Private mDesc As New List(Of String)
        Private mLeadTimeStart As New List(Of Double)
        Private mIndex As New List(Of Integer)


#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_Needed
        Public Property ID_SplOperation() As List(Of Integer)
            '================================================
            Get
                Return mID_SplOperation
            End Get

            Set(Obj As List(Of Integer))
                mID_SplOperation = Obj
            End Set
        End Property


        '....Desc 
        Public Property Desc() As List(Of String)
            '=======================================
            Get
                Return mDesc
            End Get

            Set(Obj As List(Of String))
                mDesc = Obj
            End Set
        End Property

        '....LeadTimeStart 
        Public Property LeadTimeStart() As List(Of Double)
            '=======================================
            Get
                Return mLeadTimeStart
            End Get

            Set(Obj As List(Of Double))
                mLeadTimeStart = Obj
            End Set
        End Property

        '....Index 
        Public Property Index() As List(Of Integer)
            '=======================================
            Get
                Return mIndex
            End Get

            Set(Obj As List(Of Integer))
                mIndex = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblPlanning_SplOperation
                Dim pQrySplOperationCount As Integer = (From pRec In pSealProcessDBEntities.tblPlanning_SplOperation
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQrySplOperationCount > 0) Then

                    Dim pQrySplOperation = (From pRec In pSealProcessDBEntities.tblPlanning_SplOperation
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQrySplOperation.Count - 1
                        mID_SplOperation.Add(pQrySplOperation(i).fldID)
                        mDesc.Add(pQrySplOperation(i).fldDesc)
                        mLeadTimeStart.Add(pQrySplOperation(i).fldLeadTimeStart)
                        mIndex.Add(pQrySplOperation(i).fldIndex)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pQrySplOperation = (From SplOperation In pSealProcessDBEntities.tblPlanning_SplOperation
                                                Where SplOperation.fldProcessProjectID = ProjectID_In Select SplOperation).ToList()

            If (pQrySplOperation.Count > 0) Then
                For j As Integer = 0 To pQrySplOperation.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQrySplOperation(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pPlanning_SplOperation As New List(Of tblPlanning_SplOperation)

            For j As Integer = 0 To mID_SplOperation.Count - 1
                Dim pSplOperation As New tblPlanning_SplOperation
                pPlanning_SplOperation.Add(pSplOperation)
                With pPlanning_SplOperation(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDesc = mDesc(j)
                    .fldLeadTimeStart = mLeadTimeStart(j)
                    .fldIndex = j + 1
                End With

                pSealProcessDBEntities.AddTotblPlanning_SplOperation(pPlanning_SplOperation(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region


    End Class

#End Region

#Region "clsMileOperation:"

    <Serializable()> _
    Public Class clsMileOperation

#Region "MEMBER VARIABLES:"

        Private mID_MileOperation As New List(Of Integer)
        Private mName As New List(Of String)
        Private mLeadTime As New List(Of Double)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_Needed
        Public Property ID_MileOperation() As List(Of Integer)
            '================================================
            Get
                Return mID_MileOperation
            End Get

            Set(Obj As List(Of Integer))
                mID_MileOperation = Obj
            End Set
        End Property


        '....Name 
        Public Property Name() As List(Of String)
            '=======================================
            Get
                Return mName
            End Get

            Set(Obj As List(Of String))
                mName = Obj
            End Set
        End Property

        '....LeadTime 
        Public Property LeadTime() As List(Of Double)
            '=======================================
            Get
                Return mLeadTime
            End Get

            Set(Obj As List(Of Double))
                mLeadTime = Obj
            End Set
        End Property


#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblMile_SplOperation
                Dim pQryMileOperationCount As Integer = (From pRec In pSealProcessDBEntities.tblPlanning_MileOperation
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryMileOperationCount > 0) Then

                    Dim pQryMileOperation = (From pRec In pSealProcessDBEntities.tblPlanning_MileOperation
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryMileOperation.Count - 1
                        mID_MileOperation.Add(pQryMileOperation(i).fldID)
                        mName.Add(pQryMileOperation(i).fldName)
                        mLeadTime.Add(pQryMileOperation(i).fldLeadTime)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pQryMileOperation = (From MileOperation In pSealProcessDBEntities.tblPlanning_MileOperation
                                                Where MileOperation.fldProcessProjectID = ProjectID_In Select MileOperation).ToList()

            If (pQryMileOperation.Count > 0) Then
                For j As Integer = 0 To pQryMileOperation.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryMileOperation(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pMile_SplOperation As New List(Of tblPlanning_MileOperation)

            For j As Integer = 0 To mID_MileOperation.Count - 1
                Dim pMileOperation As New tblPlanning_MileOperation
                pMile_SplOperation.Add(pMileOperation)
                With pMile_SplOperation(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldName = mName(j)
                    .fldLeadTime = mLeadTime(j)
                End With

                pSealProcessDBEntities.AddTotblPlanning_MileOperation(pMile_SplOperation(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region


    End Class

#End Region

#End Region

End Class
