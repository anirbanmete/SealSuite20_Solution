
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                              '
'                      CLASS MODULE  :  clsUnit                                '
'                        VERSION NO  :  1.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  27APR17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Globalization.CultureInfo
Imports clsLibrary11
Imports System.IO
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class clsPartUnit
    Implements ICloneable

#Region "NAMED CONSTANT:"

    Public Const mcEPS As Single = 0.00001     '....An aribitrarily small number. 

#End Region

    'CONVERSION FACTORS:
    '-------------------
    '....Consistent Unit: English Unit ===> unit.System.
    '........This is for the database values which are in English units.

#Region "STRUCTURE:"

    <Serializable()> _
    Public Structure sCon
        Public L As Single
        Public P As Single
        Public F As Single
        Public T As Single
        Public TOff As Single        '....This is an offset, not a conversion factor.
    End Structure


    <Serializable()> _
    Public Structure SUser
        Public L As Single
        Public Stress As Single
    End Structure


    <Serializable()> _
    Public Structure sCFac
        Public Con As sCon
        Public User As SUser
    End Structure

#End Region


#Region "MEMBER VARIALES:"

    Private mSystem As String       '....English or Metric. (FUNDAMENTAL)
    Private mCFac As sCFac          '....Conversion Factors

    '....Consistent Unit Labels:
    Private mT As String
    Private mF As String

    '....User unit Labels & Format:
    Private mLFormat As String
    Private mTFormat As String
    Private mUserL As String
    Private mUserP As String
    Private mUserStress As String
    Private mUserWt As String
    Private mUserLeakage As String              'PB 26NOV09.

#End Region


#Region "PROPERTY ROUTINES"

    'READ & WRITE  PROPERTIES:
    '========================

    Public Property System() As String
        '=================================
        Get
            Return mSystem
        End Get

        Set(ByVal strData As String)
            mSystem = strData
            SetValues()
        End Set

    End Property


    Public Property UserP() As String
        '=============================
        Get
            Return mUserP
        End Get

        Set(ByVal strData As String)
            mUserP = strData
        End Set

    End Property


    'READ-ONLY PROPERTIES:
    '=====================
    Public ReadOnly Property CFacConL() As Single
        '=========================================
        Get
            Return mCFac.Con.L
        End Get
    End Property


    Public ReadOnly Property CFacConP() As Single
        '==========================================
        Get
            Return mCFac.Con.P
        End Get
    End Property


    Public ReadOnly Property CFacConF() As Single
        '==========================================
        Get
            Return mCFac.Con.F
        End Get
    End Property


    Public ReadOnly Property CFacConT() As Single
        '==========================================
        Get
            Return mCFac.Con.T
        End Get
    End Property


    Public ReadOnly Property CFacConTOff() As Single
        '==========================================
        Get
            Return mCFac.Con.TOff
        End Get
    End Property


    '....In unit.System:  User Unit ===> Consistent Unit.  
    Public ReadOnly Property CFacUserL() As Single
        '==========================================
        Get
            Return mCFac.User.L
        End Get
    End Property


    '....Unit Name: Temperature.
    Public ReadOnly Property T() As String
        '==================================
        Get
            Return mT
        End Get
    End Property


    Public ReadOnly Property F() As String
        '==================================
        Get
            Return mF
        End Get
    End Property


    'DATA FORMATS:
    '-------------
    '....L in User unit.:

    Public ReadOnly Property LFormat() As String
        '========================================
        Get
            Return mLFormat
        End Get
    End Property

    '....Thickness in User unit:
    Public ReadOnly Property TFormat() As String
        '========================================
        Get
            Return mTFormat
        End Get
    End Property


    '....User Unit Name - L:
    Public ReadOnly Property UserL() As String
        '======================================
        Get
            Return mUserL
        End Get
    End Property


    '....User Unit Name - Stress:
    Public ReadOnly Property UserStress() As String
        '==========================================
        Get
            Return mUserStress
        End Get
    End Property


    '....User Unit Name - Weight
    Public ReadOnly Property UserWt() As String
        '======================================
        Get
            Return mUserWt
        End Get
    End Property


    '....User Unit Name - Leakage.   'PB 26NOV09.
    Public ReadOnly Property UserLeakage() As String
        '===========================================
        Get
            Return mUserLeakage
        End Get
    End Property


    '*******************************************************************************
    '*                           PROPERTIES ROUTINE  - END                         *
    '*******************************************************************************
#End Region


#Region "CLASS METHODS:"

    '*******************************************************************************
    '*                        CLASS METHODS - BEGIN                                *
    '*******************************************************************************

    Public Sub New(Optional ByVal strUnitSystem_In As String = "English")
        '================================================================
        mSystem = strUnitSystem_In
        SetValues()
    End Sub


    Private Sub SetValues()
        '==================

        Select Case mSystem
            Case "English"
                Call SetValuesEnglish()

            Case "Metric"
                Call SetValuesMetric()
        End Select

    End Sub


    Private Sub SetValuesEnglish()
        '=========================

        'Conversion Factors:
        '-------------------
        '....English consistent unit ==> English consistent unit.
        mCFac.Con.L = 1.0
        mCFac.Con.P = 1.0
        mCFac.Con.F = 1.0
        mCFac.Con.T = 1.0
        mCFac.Con.TOff = 0.0

        '....User unit ==> Consistent unit
        mCFac.User.L = 1.0

        'User Unit Labels & Format:
        '-------------------------- 
        mLFormat = "##0.000"
        mTFormat = "##0.0000"               '....Thickness Format
        mUserL = "in"
        'AES 14MAR16
        'mUserP = "psi"
        mUserP = "psid"
        mT = Chr(186) & "F"
        mF = "lbf"
        mUserStress = "psi"                 '....Stress.
        mUserWt = "lb"
        mUserLeakage = "cc/s"               'PB 26NOV09

    End Sub


    Private Sub SetValuesMetric()
        '========================

        'Conversion Factors:
        '-------------------
        '....English consistent unit ==> Metric consistent unit.
        mCFac.Con.L = 0.0254         '....1 inch = 0.0254   m
        mCFac.Con.P = 6911.605       '....1 psi  = 6911.605 Pa
        mCFac.Con.F = 4.459          '....1 lbf  = 4.459    N
        mCFac.Con.T = 1.0# / 1.8     '....1 F    = (1/1.8)  C

        mCFac.Con.TOff = -32.0# / 1.8 '.... C = (F-32)/1.8

        '....User unit ==> Consistent unit
        mCFac.User.L = 0.001         '....mm ==> m

        'User Unit Labels & Format:
        '--------------------------
        mLFormat = "###0.00"
        mTFormat = "###0.000"               '....Thickness Format
        mUserL = "mm"
        mUserP = "Bar"  '....Default User Unit.
        mT = Chr(186) & "C"
        mF = "N"
        mUserStress = "MPa"                 '....Stress.
        mUserWt = "N"
        mUserLeakage = "cc/s"               'PB 26NOV09

    End Sub


    Public Function CFacUserP() As Single
        '================================
        '....Conversion Factor for P:  User Unit ===> Consistent Unit.

        If mSystem = "English" Then
            '----------------------
            CFacUserP = 1.0#

        ElseIf mSystem = "Metric" Then
            '-------------------------
            Dim psngCFac As Single

            Select Case mUserP
                Case "Bar"
                    psngCFac = 10 ^ 5          '....1 Bar     = 10^5 Pa

                Case "kPa"
                    psngCFac = 1.0#            '....1 kPa     = 10^3 Pa

                Case "kg/cm^2"
                    psngCFac = 9.81 * 10 ^ 4   '....1 kg/cm^2 = 9.81*10^4 Pa
            End Select

            CFacUserP = psngCFac
        End If

    End Function


#Region "LENGTH CONVERSIONS ROUTINES:"

    '--------------------------------------------------------------------------------
    '                         LENGTH CONVERSIONS  - BEGIN                           '
    '--------------------------------------------------------------------------------

    Public Function L_ConToUser(ByVal sngData As Single) As Single
        '=========================================================
        Return (sngData / mCFac.User.L)
    End Function


    Public Function L_ConToEnglish(ByVal sngData As Single) As Single
        '============================================================   'AM 19FEB09
        Return (sngData / mCFac.Con.L)
    End Function


    '....Overloaded Verison 1:
    Public Function L_UserToCon(ByVal strData As String) As Single
        '=========================================================
        '....This utility function converts a length data string from user 
        '........to consistent unit and returns the corresponding value.

        If strData.Contains(",") Then
            strData = strData.Replace(",", ".")
        End If

        'PB 20SEP08. SG, why do you need 'CurrentCulture' here. It is a single value operation. Please explain.
        ' L_UserToCon = Convert.ToSingle((Val(strData) * mCFac.User.L), CurrentCulture) 'SG 08APR08
        'SG 22SEP08                             CurrentCulture is not required 
        L_UserToCon = Val(strData) * mCFac.User.L
    End Function


    '....Overloaded Verison 2:
    Public Function L_UserToCon(ByVal sngData As Single) As Single
        '=========================================================
        '....This utility function converts a length data string from user 
        '........to consistent unit and returns the corresponding value.
        L_UserToCon = sngData * mCFac.User.L
    End Function


    '....Overloaded Verison 1:
    Public Function WriteInUserL(ByVal sngData_In As Single) As String
        '==========================================================
        'This function is used in desplaying length dataset members on the the forms.
        '....This utility function converts a length data from consistent to user unit,
        '........formats appropriately and returns the corresponding string.

        '....Default Format: mLFormat.'SG 08APR08

        If Abs(sngData_In) > mcEPS Then
            'WriteInUserL = Format(sngData / mCFac.User.L, mLFormat)
            WriteInUserL = L_ConToUser(sngData_In).ToString(mLFormat, CurrentCulture)
        Else
            'WriteInUserL = Format(0.0, mLFormat)
            WriteInUserL = sngData_In.ToString(mLFormat, CurrentCulture)
        End If

    End Function


    '....Overloaded Verison 2:          
    Public Function WriteInUserL(ByVal sngData_In As Single, _
                                 ByVal strFormat_In As String) As String
        '===============================================================
        'This function is used in desplaying length dataset members on the the forms.
        '....This utility function converts a length data from consistent to user unit,
        '........formats appropriately and returns the corresponding string.

        Dim pFormat As String = ""
        If strFormat_In = "LFormat" Then
            pFormat = mLFormat
        ElseIf strFormat_In = "TFormat" Then
            pFormat = mTFormat
        Else
            pFormat = strFormat_In       'SG 08APR08
        End If
        'SG 08APR08
        If Abs(sngData_In) > mcEPS Then
            ' WriteInUserL = Format(sngData_In / mCFac.User.L, pFormat)
            WriteInUserL = L_ConToUser(sngData_In).ToString(pFormat, CurrentCulture)
        Else
            'WriteInUserL = Format(0.0, pFormat)
            WriteInUserL = sngData_In.ToString(pFormat, CurrentCulture)
        End If

    End Function


    Public Function EngLToUserL(ByVal sngData As Single) As Single
        '========================================================
        '....This function converts a length in English unit (e.g. in) ===> User unit
        '........in unit.system. (in or mm).
        '
        EngLToUserL = sngData * mCFac.Con.L / mCFac.User.L
    End Function


    Public Function UserLToEngL(ByVal sngData As Single) As Single
        '========================================================
        '....This function converts a length in User unit (e.g. in or mm) ===> English (in).
        '
        UserLToEngL = sngData * mCFac.User.L / mCFac.Con.L
    End Function

    '--------------------------------------------------------------------------------
    '                         LENGTH CONVERSIONS  - END                             '
    '--------------------------------------------------------------------------------
#End Region


#Region "STRESS CONVERSIONS ROUTINES:"

    Public Function Stress_ConToUser(ByVal sngData As Single) As Single
        '==============================================================

        If mSystem = "English" Then
            Return sngData                      '....psi ===>psi

        ElseIf mSystem = "Metric" Then
            Return sngData / 1000000.0          '.... Pa ===> MPa

        End If

    End Function


    Public Function Stress_UserToCon(ByVal sngData As Single) As Single
        '===============================================================

        If mSystem = "English" Then
            Return sngData                      '....psi ===> psi

        ElseIf mSystem = "Metric" Then
            Return sngData * 1000000.0          '....MPa ===> Pa

        End If

    End Function

#End Region


    Public Function FormatPDiffUnitUser(ByVal sngPDiff As Single) As String
        '=================================================================
        'The argument value is in consistent unit and the return value is in
        '....user unit, properly formatted for display.

        FormatPDiffUnitUser = "0"       'AES 04OCT16

        Try
            If sngPDiff > mcEPS Then
                Dim psngPDiffUnitUser As Single
                psngPDiffUnitUser = sngPDiff / CFacUserP()     '...In User Unit.

                Dim psngDecimalPart As Single
                psngDecimalPart = psngPDiffUnitUser - Int(psngPDiffUnitUser)

                If psngDecimalPart <= mcEPS Then
                    FormatPDiffUnitUser = Format(NInt(psngPDiffUnitUser), "#####")

                ElseIf psngDecimalPart > mcEPS Then
                    'FormatPDiffUnitUser = Format(psngPDiffUnitUser, "#####.#")
                    FormatPDiffUnitUser = psngPDiffUnitUser.ToString("#####.#", CurrentCulture)
                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Function


    Public Function RoundStressUnitUser(ByVal sngStressIn As Single) As String
        '====================================================================
        '....The return rounded value is in "Unit User Stress".

        RoundStressUnitUser = ""        '....Initialize.      

        Dim psngRound As Single
        Dim psngDiff As Single

        If mSystem = "English" Then
            '----------------------
            psngRound = Int(sngStressIn / 1000) * 1000              '....In Thousands
            psngDiff = sngStressIn - psngRound

            If psngDiff <= 250 Then
                psngRound = psngRound

            ElseIf psngDiff > 250 And psngDiff <= 750 Then
                psngRound = psngRound + 500

            ElseIf psngDiff > 750 Then
                psngRound = psngRound + 1000
            End If

            RoundStressUnitUser = Format(psngRound, "#######")      '....Return Value.


        ElseIf mSystem = "Metric" Then
            '-------------------------
            psngRound = Int(sngStressIn / 1000000.0#)       '....MPa
            psngDiff = (sngStressIn / 1000000.0#) - psngRound

            If psngDiff < 0.5 Then
                psngRound = psngRound

            ElseIf psngDiff >= 0.5 Then
                psngRound = psngRound + 1.0#
            End If

            RoundStressUnitUser = Format(psngRound, "#####")        '....Return Value.

        End If

    End Function


    Public Function WriteAngle(ByVal Angel_In As Single) As String
        '=========================================================
        Return Angel_In.ToString("##0.00", CurrentCulture) 'SG 08APR08
    End Function

    '*******************************************************************************
    '*                        CLASS METHODS - END                                  *
    '*******************************************************************************
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
