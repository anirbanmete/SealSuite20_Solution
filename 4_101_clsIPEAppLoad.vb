'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsAppLoad                             '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  23FEB16                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class IPE_clsAppLoad

#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sPreComp
        Public Exists As Boolean
        Public HMin As Double
    End Structure

    <Serializable()> _
    Public Structure sAddLoad
        Public PDiff As Double
        Public TOper As Double
        Public CavityDepth As Double
    End Structure

#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mPreComp As sPreComp
    Private mAddLoad As New List(Of sAddLoad)      'PB 22FEB16. No List
    Private mRadConstraint As Boolean

#End Region


#Region "PROPERTY ROUTINES:"

#Region "PRE-COMPRESSED:"

    '....Pre-Compressed
    Public ReadOnly Property PreComp() As sPreComp
        '==========================================
        Get
            Return mPreComp
        End Get
    End Property


    '....PreComp Exists
    Public WriteOnly Property PreComp_Exits() As Boolean
        '=====================================

        Set(ByVal blnData As Boolean)
            mPreComp.Exists = blnData
        End Set

    End Property


    '....PreComp HMin
    Public WriteOnly Property PreComp_HMin() As Double
        '=============================================

        Set(ByVal sngData As Double)
            mPreComp.HMin = sngData
        End Set

    End Property

#End Region

    '....Additional Load
    Public Property AddLoad() As List(Of sAddLoad)
        '=============================================
        Get
            Return mAddLoad
        End Get

        Set(ByVal Data As List(Of sAddLoad))
            mAddLoad = Data
        End Set

    End Property


    '....Radial Constraint.
    Public Property RadConstraint() As Boolean
        '=====================================
        Get
            Return mRadConstraint
        End Get

        Set(ByVal blnData As Boolean)
            mRadConstraint = blnData
        End Set

    End Property

#End Region


#Region "CONSTRUCTOR:"

    Public Sub New()
        '=============
        mRadConstraint = True

    End Sub

#End Region

End Class
