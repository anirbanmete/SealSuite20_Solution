'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_Seal                           '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29NOV16                                '
'                                                                              '
'===============================================================================

Imports System.Linq
Imports clsLibrary11

<Serializable()> _
Public Class Test_clsSeal

    Enum eStatus
        Pass
        Fail
    End Enum


#Region "STRUCTURE:"

#Region "LEAK:"
    <Serializable()> _
    Public Structure sLeak
        Public FHIni As Double
        Public FHFinal As Double
        Public ODPre As Double
        Public ODPost As Double
        Public IDPre As Double
        Public IDPost As Double
        Public Val As Double
    End Structure

#End Region


#Region "LOAD:"
    <Serializable()> _
    Public Structure sLoad
        Public FHIni As Double
        Public FHFinal As Double
        Public ODPre As Double
        Public ODPost As Double
        Public IDPre As Double
        Public IDPost As Double
        Public Val As Double
    End Structure

#End Region

#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mLeak As sLeak
    Private mLoad As sLoad

    Private mStatus As eStatus = eStatus.Pass
#End Region


#Region "PROPERTY ROUTINES:"
    '======================

#Region "LEAK:"

    '....Leak
    Public ReadOnly Property Leak() As sLeak
        '=====================================
        Get
            Return mLeak
        End Get
    End Property


    '....Leak_FHIni
    Public WriteOnly Property Leak_FHIni() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLeak.FHIni = Value
        End Set
    End Property

    '....Leak_FHFinal
    Public WriteOnly Property Leak_FHFinal() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLeak.FHFinal = Value
        End Set

    End Property

    '....Leak_ODPre
    Public WriteOnly Property Leak_ODPre() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLeak.ODPre = Value
        End Set

    End Property

    '....Leak_ODPost
    Public WriteOnly Property Leak_ODPost() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLeak.ODPost = Value
        End Set

    End Property

    '....Leak_IDPre
    Public WriteOnly Property Leak_IDPre() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLeak.IDPre = Value
        End Set

    End Property

    '....Leak_IDPost
    Public WriteOnly Property Leak_IDPost() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLeak.IDPost = Value
        End Set

    End Property

    '....Leak_Val
    Public WriteOnly Property Leak_Val() As Double
        '==========================================
        Set(ByVal Value As Double)
            mLeak.Val = Value
        End Set

    End Property

#End Region


#Region "LOAD:"

    '....Leak
    Public ReadOnly Property Load() As sLoad
        '=====================================
        Get
            Return mLoad
        End Get
    End Property

    '....Load_FHIni
    Public WriteOnly Property Load_FHIni() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLoad.FHIni = Value
        End Set
    End Property

    '....Load_FHFinal
    Public WriteOnly Property Load_FHFinal() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLoad.FHFinal = Value
        End Set

    End Property

    '....Load_ODPre
    Public WriteOnly Property Load_ODPre() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLoad.ODPre = Value
        End Set

    End Property

    '....Load_ODPost
    Public WriteOnly Property Load_ODPost() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLoad.ODPost = Value
        End Set

    End Property

    '....Load_IDPre
    Public WriteOnly Property Load_IDPre() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLoad.IDPre = Value
        End Set

    End Property

    '....Load_IDPost
    Public WriteOnly Property Load_IDPost() As Double
        '===========================================
        Set(ByVal Value As Double)
            mLoad.IDPost = Value
        End Set

    End Property

    '....Load_Val
    Public WriteOnly Property Load_Val() As Double
        '==========================================
        Set(ByVal Value As Double)
            mLoad.Val = Value
        End Set

    End Property

#End Region

    Public Property Status() As eStatus
        '==============================
        Get
            Return mStatus
        End Get
        Set(value As eStatus)
            mStatus = value
        End Set
    End Property

#End Region


#Region "UTILITY ROUTINES:"

    Public Sub IsTestData(ByRef IsLeak_Out As Boolean, ByRef IsLoad_Out As Boolean, ByRef IsPress_Out As Boolean)
        '==========================================================================================================
        IsLeak_Out = False
        IsLoad_Out = False
        IsPress_Out = False

        If (mLeak.FHIni > gcEPS Or mLeak.FHFinal > gcEPS) Then
            IsLeak_Out = True
        End If

        If (mLoad.FHIni > gcEPS Or mLoad.FHFinal > gcEPS) Then
            IsLoad_Out = True
        End If

    End Sub

#End Region


End Class
