'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_IssueComment            '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_IssueComment

#Region "MEMBER VARIABLES:"

    Private mID As New List(Of Integer)
    Private mComment As New List(Of String)
    Private mByDept As New List(Of String)
    Private mByName As New List(Of String)
    Private mByDate As New List(Of Date)
    Private mToDept As New List(Of String)
    Private mResolved As New List(Of Boolean)
    Private mName As New List(Of String)
    Private mDateResolution As New List(Of Date)
    Private mResolution As New List(Of String)

#End Region

#Region "PROPERTY ROUTINES:"

    '....ID
    Public Property ID() As List(Of Integer)
        '======================================
        Get
            Return mID
        End Get

        Set(Obj As List(Of Integer))
            mID = Obj
        End Set
    End Property

    '....Comment
    Public Property Comment() As List(Of String)
        '=======================================
        Get
            Return mComment
        End Get

        Set(Obj As List(Of String))
            mComment = Obj
        End Set

    End Property

    '....ByDept
    Public Property ByDept() As List(Of String)
        '=====================================
        Get
            Return mByDept
        End Get

        Set(Obj As List(Of String))
            mByDept = Obj
        End Set

    End Property

    '....ByName
    Public Property ByName() As List(Of String)
        '======================================
        Get
            Return mByName
        End Get

        Set(Obj As List(Of String))
            '-------------------------------
            mByName = Obj
        End Set

    End Property

    '....ByDate
    Public Property ByDate() As List(Of Date)
        '=====================================
        Get
            Return mByDate
        End Get

        Set(Obj As List(Of Date))
            '-------------------------------
            mByDate = Obj
        End Set

    End Property

    '....ToDept
    Public Property ToDept() As List(Of String)
        '======================================
        Get
            Return mToDept
        End Get

        Set(Obj As List(Of String))
            '-------------------------------
            mToDept = Obj
        End Set

    End Property

    '....Resolved
    Public Property Resolved() As List(Of Boolean)
        '==========================================
        Get
            Return mResolved
        End Get

        Set(Obj As List(Of Boolean))
            '-------------------------------
            mResolved = Obj
        End Set

    End Property


    '....Name
    Public Property Name() As List(Of String)
        '====================================
        Get
            Return mName
        End Get

        Set(Obj As List(Of String))
            '-------------------------------
            mName = Obj
        End Set

    End Property

    '....DateResolution
    Public Property DateResolution() As List(Of Date)
        '===========================================
        Get
            Return mDateResolution
        End Get

        Set(Obj As List(Of Date))
            '-------------------------------
            mDateResolution = Obj
        End Set

    End Property


    '....Resolution
    Public Property Resolution() As List(Of String)
        '==========================================
        Get
            Return mResolution
        End Get

        Set(Obj As List(Of String))
            '-------------------------------
            mResolution = Obj
        End Set

    End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '===================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try
            '....tblIssueComnt
            Dim pQryIssueCount As Integer = (From pRec In pSealProcessDBEntities.tblIssueComnt
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryIssueCount > 0) Then

                Dim pQryIssue = (From pRec In pSealProcessDBEntities.tblIssueComnt
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                For i As Integer = 0 To pQryIssue.Count - 1
                    mID.Add(pQryIssue(i).fldID)
                    mComment.Add(pQryIssue(i).fldComment)
                    mByDept.Add(pQryIssue(i).fldByDept)
                    mByName.Add(pQryIssue(i).fldByName)
                    mByDate.Add(pQryIssue(i).fldByDate)
                    mToDept.Add(pQryIssue(i).fldToDept)
                    mResolved.Add(pQryIssue(i).fldResolved)
                    mName.Add(pQryIssue(i).fldName)
                    mDateResolution.Add(pQryIssue(i).fldDate)
                    mResolution.Add(pQryIssue(i).fldResolution)
                Next

            End If

        Catch ex As Exception

        End Try

    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()


        Dim pQryIssuesComment = (From IssueComment In pSealProcessDBEntities.tblIssueComnt
                                            Where IssueComment.fldProcessProjectID = ProjectID_In Select IssueComment).ToList()

        If (pQryIssuesComment.Count > 0) Then
            For j As Integer = 0 To pQryIssuesComment.Count() - 1
                pSealProcessDBEntities.DeleteObject(pQryIssuesComment(j))
                pSealProcessDBEntities.SaveChanges()
            Next
        End If

        Dim pIssueComment As New List(Of tblIssueComnt)

        For j As Integer = 0 To mID.Count - 1
            Dim IssueComment As New tblIssueComnt
            pIssueComment.Add(IssueComment)
            With pIssueComment(j)
                .fldProcessProjectID = ProjectID_In
                .fldID = j + 1
                .fldComment = mComment(j)
                .fldByDept = mByDept(j)
                .fldByName = mByName(j)
                .fldByDate = mByDate(j)
                .fldToDept = mToDept(j)
                .fldResolved = mResolved(j)
                .fldName = mName(j)
                .fldDate = mDateResolution(j)
                .fldResolution = mResolution(j)

            End With

            pSealProcessDBEntities.AddTotblIssueComnt(pIssueComment(j))
        Next
        pSealProcessDBEntities.SaveChanges()

    End Sub


#End Region

End Class
