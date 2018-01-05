'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_Risk                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_Risk

#Region "MEMBER VARIABLES:"

    Private mTabName As String
    Private mRiskAnlayisQ As List(Of Dictionary(Of String, Integer))
    Private mAnswered As List(Of Boolean)
    Private mReason As List(Of String)

#End Region

#Region "PROPERTY ROUTINES:"

    '....TabName
    Public Property TabName() As String
        '==============================
        Get
            Return mTabName
        End Get

        Set(Obj As String)
            mTabName = Obj
        End Set
    End Property


    '....RiskAnlayisQ
    Public Property RiskAnlayisQ() As List(Of Dictionary(Of String, Integer))
        '====================================================================
        Get
            Return mRiskAnlayisQ
        End Get

        Set(Obj As List(Of Dictionary(Of String, Integer)))
            mRiskAnlayisQ = Obj
        End Set
    End Property

    '....Answered
    Public Property Answered() As List(Of Boolean)
        '=========================================
        Get
            Return mAnswered
        End Get

        Set(Obj As List(Of Boolean))
            mAnswered = Obj
        End Set
    End Property

    '....Reason
    Public Property Reason() As List(Of String)
        '=======================================
        Get
            Return mReason
        End Get

        Set(Obj As List(Of String))
            mReason = Obj
        End Set
    End Property

#End Region


End Class
