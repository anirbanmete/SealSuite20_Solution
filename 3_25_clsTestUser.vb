'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  Test_clsUser                           '
'                        VERSION NO  :  2.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  27APR17                                '
'                                                                              '
'===============================================================================
Imports System.Linq
Imports System.IO

Public Class Test_clsUser

    Enum eRole
        SuperAdmin      'AES 20JUL17
        Admin
        Tester
        Eng
        Quality
        Viewer
    End Enum

#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mName As String
    Private mSystemLogin As String
    Private mRole As eRole
    Private mAdmin As Boolean
    Private mTester As Boolean
    Private mEngg As Boolean
    Private mQuality As Boolean
    Private mViewer As Boolean
    Private mSignature As Image

#End Region


#Region "CLASS PROPERTY ROUTINE:"

    Public ReadOnly Property Name() As String
        '====================================
        Get
            Return mName
        End Get

    End Property

    Public ReadOnly Property SystemLogin() As String
        '===========================================
        Get
            Return mSystemLogin
        End Get

    End Property

    Public ReadOnly Property Signature() As Image
        '========================================
        Get
            Return mSignature
        End Get
    End Property


    Public Property Role() As eRole
        '===========================
        Get
            Return mRole
        End Get

        Set(ByVal Value As eRole)
            mRole = Value
            SetRole()
        End Set
    End Property


    Public Property Admin() As Boolean
        '==============================
        Get
            Return mAdmin
        End Get

        Set(ByVal Value As Boolean)
            mAdmin = Value
        End Set
    End Property


    Public Property Tester() As Boolean
        '===============================
        Get
            Return mTester
        End Get

        Set(ByVal Value As Boolean)
            mTester = Value
        End Set
    End Property


    Public Property Engg() As Boolean
        '=============================
        Get
            Return mEngg
        End Get

        Set(ByVal Value As Boolean)
            mEngg = Value
        End Set
    End Property


    Public Property Quality() As Boolean
        '================================
        Get
            Return mQuality
        End Get

        Set(ByVal Value As Boolean)
            mQuality = Value
        End Set
    End Property


    Public Property Viewer() As Boolean
        '================================
        Get
            Return mViewer
        End Get

        Set(ByVal Value As Boolean)
            mViewer = Value
        End Set
    End Property


#End Region


#Region "ROUTINES:"

    Public Sub RetrieveUserRoles()
        '=========================
        mAdmin = False
        mTester = False
        mEngg = False
        mQuality = False
        mViewer = False

        mSystemLogin = Environment.UserName
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pRecCount As Integer = (From pRec In pSealTestEntities.tblTestUser
                                      Where pRec.fldSystemLogin = mSystemLogin Select pRec).Count()

        If (pRecCount > 0) Then
            Dim pQry = (From pRec In pSealTestEntities.tblTestUser
                                       Where pRec.fldSystemLogin = mSystemLogin Select pRec).First()
            mName = pQry.fldName

            If (IsDBNull(pQry.fldSignature)) Then
                Dim pArray As Byte() = DirectCast(pQry.fldSignature, Byte())
                Dim pMS As New MemoryStream(pArray)
                mSignature = Image.FromStream(pMS)
            End If

            If (pQry.fldRoleAdmin) Then
                mAdmin = True
            End If
            If (pQry.fldRoleTester) Then
                mTester = True
            End If
            If (pQry.fldRoleEngg) Then
                mEngg = True
            End If
            If (pQry.fldRoleQuality) Then
                mQuality = True
            End If
        Else
            mViewer = True
        End If

    End Sub


    Private Sub SetRole()
        '===============
        If mRole = eRole.Admin Then
            mAdmin = True
            mTester = False
            mEngg = False
            mQuality = False
            mViewer = False

        ElseIf mRole = eRole.Tester Then
            mTester = True
            mAdmin = False
            mEngg = False
            mQuality = False
            mViewer = False

        ElseIf mRole = eRole.Eng Then

            mEngg = True
            mAdmin = False
            mTester = False
            mQuality = False
            mViewer = False

        ElseIf mRole = eRole.Quality Then

            mQuality = True
            mAdmin = False
            mTester = False
            mEngg = False
            mViewer = False

        ElseIf mRole = eRole.Viewer Then

            mViewer = True
            mAdmin = False
            mTester = False
            mEngg = False
            mQuality = False

        End If

    End Sub


    Public Function IsAdminExists() As Boolean
        '======================================
        Dim pAdminExists As Boolean = False
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pRecCount As Integer = (From pRec In pSealTestEntities.tblTestUser
                                     Where pRec.fldRoleAdmin = True Select pRec).Count()

        If (pRecCount > 0) Then
            pAdminExists = True
        End If

        Return pAdminExists

    End Function


    Public Function GetUserList() As List(Of String)
        '===========================================
        Dim pUserList As New List(Of String)
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pUserRec = (From pRec In pSealTestEntities.tblTestUser
                                     Select pRec).ToList()

        If (pUserRec.Count() > 0) Then
            For i As Integer = 0 To pUserRec.Count() - 1
                If ((mRole = eRole.Tester And pUserRec(i).fldRoleTester = True) Or _
                    (mRole = eRole.Admin And pUserRec(i).fldRoleAdmin = True)) Then
                    pUserList.Add(pUserRec(i).fldName)

                ElseIf ((mRole = eRole.Eng And pUserRec(i).fldRoleEngg = True) Or _
                    (mRole = eRole.Admin And pUserRec(i).fldRoleAdmin = True)) Then
                    pUserList.Add(pUserRec(i).fldName)

                ElseIf ((mRole = eRole.Quality And pUserRec(i).fldRoleQuality = True) Or _
                    (mRole = eRole.Admin And pUserRec(i).fldRoleAdmin = True)) Then
                    pUserList.Add(pUserRec(i).fldName)

                End If
            Next

        End If

        Return pUserList

    End Function



#End Region

End Class
