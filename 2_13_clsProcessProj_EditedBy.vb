'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsEditedBy                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28DEC17                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_EditedBy

#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sUser      '....SignOffUser
        Public Name As String
        Public Signed As Boolean
        Public DateSigned As Date
    End Structure

#End Region

#Region "MEMBER VARIABLES:"

    Private mTabName As String
    Private mID_Edit As New List(Of Integer)
    Private mDateEdited As New List(Of Date)
    Private mName As New List(Of String)
    Private mComment As New List(Of String)

    Private mUser As sUser

#End Region

#Region "PROPERTY ROUTINES:"

    '....TabName
    Public Property TabName() As String
        '===============================
        Get
            Return mTabName
        End Get

        Set(Obj As String)
            mTabName = Obj
        End Set
    End Property

    '....ID
    Public Property ID_Edit() As List(Of Integer)
        '========================================
        Get
            Return mID_Edit
        End Get

        Set(Obj As List(Of Integer))
            mID_Edit = Obj
        End Set
    End Property

    '....DateEdited
    Public Property DateEdited() As List(Of Date)
        '=========================================
        Get
            Return mDateEdited
        End Get

        Set(Obj As List(Of Date))
            mDateEdited = Obj
        End Set
    End Property

    '....Name
    Public Property Name() As List(Of String)
        '===================================
        Get
            Return mName
        End Get

        Set(Obj As List(Of String))
            mName = Obj
        End Set
    End Property

    '....Comment
    Public Property Comment() As List(Of String)
        '======================================
        Get
            Return mComment
        End Get

        Set(Obj As List(Of String))
            mComment = Obj
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

    '....User_DateSigned
    Public WriteOnly Property User_DateSigned() As Date
        '===============================================
        Set(ByVal strVal As Date)
            mUser.DateSigned = strVal
        End Set
    End Property

#End Region

#Region "DATABASE RELATED ROUTINES:"

    Public Function RetrieveFromDB(ByVal ProjectID_In As Integer, ByVal TabName_In As String) As Boolean
        '==============================================================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()
        Dim pIsRecExists As Boolean = False

        Try
            '....tblEditedBy
            Dim pQryEditedByCount As Integer = (From pRec In pSealProcessDBEntities.tblEditedBy
                                                Where pRec.fldProcessProjectID = ProjectID_In And pRec.fldTabName = TabName_In Select pRec).Count()


            If (pQryEditedByCount > 0) Then
                pIsRecExists = True
                Dim pQryEditedBy = (From pRec In pSealProcessDBEntities.tblEditedBy
                                    Where pRec.fldProcessProjectID = ProjectID_In And pRec.fldTabName = TabName_In Select pRec).ToList()

                For i As Integer = 0 To pQryEditedBy.Count - 1
                    mID_Edit.Add(pQryEditedBy(i).fldID)
                    mDateEdited.Add(pQryEditedBy(i).fldDate)
                    mName.Add(pQryEditedBy(i).fldName)
                    mComment.Add(pQryEditedBy(i).fldComment)
                Next

            End If

        Catch ex As Exception

        End Try

        Return pIsRecExists

    End Function

    'Public Sub SaveToDB(ByVal ProjectID_In As Integer, ByVal TabName_In As String)
    '    '===========================================================================

    '    Dim pSealProcessDBEntities As New SealProcessDBEntities()

    '    '....tblEditedBy
    '    Dim pEditedByCount As Integer = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                                     Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In And
    '                                         EditedBy.fldDate = mDateEdited And EditedBy.fldName = mName Select EditedBy).Count()

    '    If (pEditedByCount > 0) Then
    '        '....Record already exists
    '        Dim pEditedBy = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                         Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Select EditedBy).First()

    '        pEditedBy.fldDate = mDateEdited
    '        pEditedBy.fldName = mName
    '        pEditedBy.fldComment = mComment

    '        pSealProcessDBEntities.SaveChanges()

    '    Else
    '        Dim pRecCount As Integer = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                                    Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Select EditedBy).Count()

    '        Dim pID As Integer = 0
    '        If (pRecCount > 0) Then
    '            Dim pRec = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                        Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Order By EditedBy.fldID Descending Select EditedBy).ToList()
    '            pID = pRec(0).fldID
    '        End If
    '        '....New Record
    '        Dim pProjectID As Integer = ProjectID_In

    '        Dim pEditedBy As New tblEditedBy
    '        pEditedBy.fldProcessProjectID = pProjectID
    '        pEditedBy.fldTabName = TabName_In
    '        pEditedBy.fldID = pID + 1
    '        pEditedBy.fldDate = mDateEdited
    '        pEditedBy.fldName = mName
    '        pEditedBy.fldComment = mComment

    '        pSealProcessDBEntities.AddTotblEditedBy(pEditedBy)
    '        pSealProcessDBEntities.SaveChanges()
    '    End If

    'End Sub

    Public Sub SaveToDB(ByVal ProjectID_In As Integer, ByVal TabName_In As String)
        '=========================================================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblEditedBy
        Dim pEditedBy = (From EditedBy In pSealProcessDBEntities.tblEditedBy
                         Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Select EditedBy).ToList()


        If (pEditedBy.Count > 0) Then
            For j As Integer = 0 To pEditedBy.Count() - 1
                pSealProcessDBEntities.DeleteObject(pEditedBy(j))
                pSealProcessDBEntities.SaveChanges()
            Next
        End If

        Dim pEdited As New List(Of tblEditedBy)

        For j As Integer = 0 To mID_Edit.Count - 1
            Dim ptblEdited As New tblEditedBy
            pEdited.Add(ptblEdited)
            With pEdited(j)
                .fldProcessProjectID = ProjectID_In
                .fldTabName = TabName_In
                .fldID = j + 1
                .fldDate = mDateEdited(j)
                .fldName = mName(j)
                .fldComment = mComment(j)

            End With

            pSealProcessDBEntities.AddTotblEditedBy(pEdited(j))
        Next
        pSealProcessDBEntities.SaveChanges()

    End Sub

#End Region


End Class
