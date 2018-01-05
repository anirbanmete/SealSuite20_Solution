
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsUser                                '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07NOV16                                '
'                                                                              '
'===============================================================================
Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports clsLibrary11

<Serializable()> _
Public Class IPE_clsUser

#Region "MEMBER VARIABLES:"

    Private mName As String
    Private mPhoneNo As String
    Private mIPE_LastSession_TimeStamp As String
#End Region


#Region "PROPERTY ROUTINES:"

    '....UserName
    Public Property Name() As String
        '===========================
        Get
            Return mName
        End Get

        Set(ByVal strValue As String)
            '---------------------
            mName = strValue
        End Set

    End Property


    '....Phone No
    Public Property PhoneNo() As String
        '==============================
        Get
            Return mPhoneNo
        End Get

        Set(ByVal strValue As String)
            '---------------------
            mPhoneNo = strValue
        End Set

    End Property


    '....System LogIn Name
    Public ReadOnly Property SysLoginName() As String
        '============================================
        Get
            Return Environment.UserName
        End Get
    End Property


    '....TimeStamp.
    Public Property IPE_LastSession_TimeStamp() As String
        '=================================================     
        Get
            Return mIPE_LastSession_TimeStamp
        End Get

        Set(ByVal strData As String)
            '-----------------------
            mIPE_LastSession_TimeStamp = strData

        End Set

    End Property

#End Region


End Class
