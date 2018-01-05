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

    Private mGUI_TabName As String
    Private mDateEdited As New List(Of Date)
    Private mName As New List(Of String)
    Private mComment As New List(Of String)

    Private mUser As sUser

#End Region

#Region "PROPERTY ROUTINES:"

    '....GUI_TabName
    Public Property GUI_TabName() As String
        '======================================
        Get
            Return mGUI_TabName
        End Get

        Set(Obj As String)
            mGUI_TabName = Obj
        End Set
    End Property

    '....DateEdited
    Public Property DateEdited() As List(Of Date)
        '======================================
        Get
            Return mDateEdited
        End Get

        Set(Obj As List(Of Date))
            mDateEdited = Obj
        End Set
    End Property

    '....Name
    Public Property Name() As List(Of String)
        '======================================
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

End Class
