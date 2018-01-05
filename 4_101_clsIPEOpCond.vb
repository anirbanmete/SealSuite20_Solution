
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsOpCond                              '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  26OCT16                                '
'                                                                              '
'===============================================================================
Imports clsLibrary11
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class IPE_clsOpCond
    Implements ICloneable

    Public Const mcTRoom As Single = 70             '....Standard Room Temperature (F)

#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mUnit As IPE_clsUnit            'PB 21JAN16. Not necessary, we may just save UnitUserP
    Private mPDiff As Single
    Private mTOper As Single
    Private mPOrient As String

#End Region


#Region "PROPERTY ROUTINES:"

    '....UnitSystem.
    Public Property UnitSystem() As String
        '==================================
        Get
            Return mUnit.System
        End Get

        Set(ByVal strData As String)
            '-----------------------
            mUnit.System = strData
        End Set

    End Property


    '....User Unit: Pressure.
    Public Property UnitUserP() As String
        '================================
        Get
            Return mUnit.UserP
        End Get

        Set(ByVal strData As String)
            mUnit.UserP = strData
        End Set

    End Property


    '....Pressure.
    Public Property PDiff() As Single
        '============================
        Get
            Return mPDiff
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mPDiff = sngData
        End Set

    End Property


    '....Temperature.
    Public Property TOper() As Single
        '============================
        Get
            Return mTOper
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mTOper = sngData
        End Set

    End Property


    '....POrient.
    Public Property POrient() As String
        '==============================
        Get
            Return mPOrient
        End Get

        Set(ByVal strData As String)
            '-----------------------
            mPOrient = strData
        End Set

    End Property


    Public ReadOnly Property TRoom() As Single
        '=====================================
        Get
            Return NInt((mcTRoom * mUnit.CFacConT + mUnit.CFacConTOff))
        End Get
    End Property


#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(Optional ByVal strUnitSystem_In As String = "English")
        '================================================================
        mUnit = New IPE_clsUnit(strUnitSystem_In)
        mPOrient = "External"

    End Sub

#End Region

#Region "CLONE METHOD"

    '   DEEP CLONING:
    '   -------------
    '
    Public Function Clone() As Object Implements ICloneable.Clone
        '========================================================
        '....Inherited from the ICloneable interface, supports deep cloning

        Dim pMemBuffer As New MemoryStream()
        Dim pBinSerializer As New BinaryFormatter(Nothing, _
                              New StreamingContext(StreamingContextStates.Clone))


        '....Serialize the object into the memory stream
        pBinSerializer.Serialize(pMemBuffer, Me)

        '....Move the stream pointer to the beginning of the memory stream
        pMemBuffer.Seek(0, SeekOrigin.Begin)

        '....Get the serialized object from the memory stream
        Dim pobjClone As Object
        pobjClone = pBinSerializer.Deserialize(pMemBuffer)

        pMemBuffer.Close()      '....Release the memory stream
        Return pobjClone    '....Return the deeply cloned object

    End Function

#End Region

End Class
