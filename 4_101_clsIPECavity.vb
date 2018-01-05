
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsCavity                              '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  11APR16                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class IPE_clsCavity
    Implements ICloneable

#Region "MEMBER VARIABLES:"

#Region "NAMED CONSTANT:"
    Public Const mcEPS As Single = 0.00001         '....An aribitrarily small number. 

    '....Minimum Compression Required. True for both E- & C-Seal.   
    '........Per discussion with APCO, Steve Stone, 01JUN06.
    Private Const mcCompressPcentValue_Rep As Single = 5.0   '....in %.

#End Region

    Private mUnit As IPE_clsUnit            '(FUNDAMENTAL)           
    Private mDia(2) As Single           '....1: Max. ID, 2: Min. OD  (FUNDAMENTAL)
    Private mDepth As Single            '....Nominal Value           (FUNDAMENTAL)
    Private mHFree_Rep As Single

    Private mDepthTol(2) As Single      '....1: - Tol.,  2: + Tol.   (FUNDAMENTAL)
    Private mCornerRad As Single        '....Corner Radius (Max.)    (RETRIEVED by clsSeal)

    'Private mSealDControl As Single     '....Required to calculate H10Tol.  'PB 07APR16 Not needed.
    'Private mH10Tol As Single               'PB 07APR16. Make it a method H10Tol (DControl) 
    Private mWidMin As Single

    Private mParam_Calculated As String
#End Region


#Region "PROPERTY ROUTINES:"

    'READ & WRITE PROPERTIES:
    '------------------------

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


    '....Dia:
    Public Property Dia(ByVal i As Integer) As Single
        '==============================================
        Get
            Return mDia(i)
        End Get

        Set(ByVal sngValue As Single)
            mDia(i) = sngValue
            If mDia(1) > mcEPS And mDia(2) > mcEPS Then
                CalcWidMin()
            End If

        End Set
    End Property


    '....Min. Width 
    Public Property WidMin() As Single
        '==============================     
        Get
            Return mWidMin
        End Get

        Set(ByVal value As Single)
            mWidMin = value
        End Set

    End Property


    Public Property Param_Calculated() As String
        '======================================
        Get
            Return mParam_Calculated
        End Get

        Set(ByVal value As String)
            mParam_Calculated = value
        End Set

    End Property


    '....Depth:
    Public Property Depth() As Single
        '==============================
        Get
            Return mDepth
        End Get

        Set(ByVal sngValue As Single)
            mDepth = sngValue
            mHFree_Rep = mDepth / (1 - mcCompressPcentValue_Rep / 100)      '....HFree - Representative Value
        End Set
    End Property


    '....DepthTol:
    Public Property DepthTol(ByVal i As Integer) As Single
        '=================================================
        Get
            Return mDepthTol(i)
        End Get

        Set(ByVal sngValue As Single)
            mDepthTol(i) = sngValue
        End Set
    End Property


    '....CornerRad:
    Public Property CornerRad() As Single
        '==================================
        Get
            Return mCornerRad
        End Get

        Set(ByVal sngValue As Single)
            mCornerRad = sngValue
        End Set
    End Property


    'READ-ONLY PROPERTY:
    '-------------------

    '....HFree_Rep.
    Public ReadOnly Property HFree_Rep() As Single
        '=========================================
        Get
            Return mHFree_Rep
        End Get
    End Property

    'PB 07APR16
    '....H10Tol.
    'Public ReadOnly Property H10Tol() As Single
    '    '=============================
    '    Get
    '        Return mH10Tol
    '    End Get
    'End Property


    'WRITE-ONLY PROPERTY:
    '-------------------
    '....Seal Control Dia.
    'Public WriteOnly Property SealDControl() As Single         'PB 07APR16
    '    '=============================================
    '    Set(ByVal sngData As Single)
    '        mSealDControl = sngData
    '        CalcH10Tol()
    '    End Set
    'End Property

#End Region


#Region "CONSTRUCTOR:"

    'Public Sub New(ByVal SealType_In As String, Optional ByVal strUnitSystem_In As String = "English")     
    Public Sub New(ByVal SealType_In As String, ByVal UnitSystem_In As String)           'AES 15APR16
        '=====================================================================

        Try
            mUnit = New IPE_clsUnit(UnitSystem_In)

            'AES 15APR16
            mCornerRad = 0.02F

            If (UnitSystem = "English") Then
                mCornerRad = 0.02F
            Else
                mCornerRad = 0.00005F
            End If


            If SealType_In = "E-Seal" Then
                For i As Integer = 1 To 2
                    'AES 15APR16
                    'mDepthTol(i) = 0.005
                    If (UnitSystem = "English") Then
                        mDepthTol(i) = 0.005F
                    Else
                        mDepthTol(i) = 0.0001F
                    End If

                Next
            ElseIf SealType_In = "C-Seal" Then
                For i As Integer = 1 To 2
                    'AES 15APR16
                    'mDepthTol(i) = 0.002
                    If (UnitSystem = "English") Then
                        mDepthTol(i) = 0.002F
                    Else
                        mDepthTol(i) = 0.00005F
                    End If

                Next

            ElseIf SealType_In = "U-Seal" Then


            End If

        Catch
            MsgBox(Err.Description)
        End Try

    End Sub

#End Region


#Region "CLASS METHODS:"

#Region "CALCULATION OF H10 Tol, MIN WIDTH & DEPTH"

    'PB 07APR16. AM, make it public, use DControl_In as argument. When consume this in clsDWg, pass the argument. The proposal dwg, Cavity Tol will be correct.
    Public Function H10Tol(ByVal DControl_In As Single) As Single
        '========================================================   'AES 08APR16
        ' This routine calculates the h10 or H10 tolerance on the cavity ID (External) or
        ' ....OD (Internal).
        '
        ' 'Unit Aware' Routine.
        '  -------------------

        '"Tolerance Reference Table"
        '--------------------------
        '....Ref. : Catalogue (Page E-22) - 'H10' Tolerances Ref. Table - Valid for both E-Seal & C-Seal..

        Dim dControlArray() As Object = Nothing
        Dim H10TolArray() As Object = Nothing

        If mUnit.System = "English" Then
            '--------------------------------
            dControlArray = New Object() {0.12, 0.24, 0.4, 0.71, 1.19, 1.97, 3.15, 4.73, _
                7.09, 9.85, 12.41, 15.75, 19.69, 30.09, 41.49, 56.19, 76.39} '....inch

            '....The following dimensions are in 0.001 in (mil).
            '........Needs to be converted to inch.
            H10TolArray = New Object() {1.6, 1.8, 2.2, 2.8, 3.5, 4, 4.5, 5, 6, 7, 8, _
                                9, 10, 12, 16, 20, 25}               '....mils


        ElseIf mUnit.System = "Metric" Then
            '----------------------------------
            dControlArray = New Object() {3, 6, 10, 18, 30, 50, 80, 120, 180, 250, 315, 400, _
                                  500, 760, 1050, 1425, 1940}            '....mm

            '....The following dimensions are in 0.001 mm (or micron)
            '........Needs to be converted to m.
            H10TolArray = New Object() {40, 48, 58, 70, 84, 100, 120, 140, 160, 185, 210, _
                                 230, 250, 300, 400, 500, 630}           '....microns

        End If

        '....Lower & Upper Bounds of the arrays
        Dim iLow As Int16 = LBound(dControlArray)
        Dim iUp As Int16 = UBound(dControlArray)


        'Get the appropriate H10 tolerance.
        '----------------------------------
        Dim pDiaUserU As Single
        pDiaUserU = mUnit.L_ConToUser(DControl_In)     '....Control Dia in user unit.
        '                                                '........(in or mm)

        Dim i As Integer
        Dim pH10_TableUnit As Single

        If pDiaUserU <= dControlArray(iLow) Then
            pH10_TableUnit = H10TolArray(iLow)

        ElseIf pDiaUserU >= dControlArray(iUp) Then
            pH10_TableUnit = H10TolArray(iUp)

        Else
            i = 1
            Do While pDiaUserU >= dControlArray(i)
                i = i + 1
            Loop
            pH10_TableUnit = H10TolArray(i)
        End If

        '....Divide   by 1000                ==> Table unit to User unit.
        '........Multiply by unit.CFacUserL  ==> User unit to Consistent unit (in or m).
        H10Tol = mUnit.L_UserToCon((pH10_TableUnit / 1000))

    End Function


    'Private Sub CalcH10Tol() 
    '    '====================
    '    ' This routine calculates the h10 or H10 tolerance on the cavity ID (External) or
    '    ' ....OD (Internal).
    '    '
    '    ' 'Unit Aware' Routine.
    '    '  -------------------

    '    '"Tolerance Reference Table"
    '    '--------------------------
    '    '....Ref. : Catalogue (Page E-22) - 'H10' Tolerances Ref. Table - Valid for both E-Seal & C-Seal..

    '    Dim dControlArray() As Object = Nothing
    '    Dim H10TolArray() As Object = Nothing

    '    If mUnit.System = "English" Then
    '        '--------------------------------
    '        dControlArray = New Object() {0.12, 0.24, 0.4, 0.71, 1.19, 1.97, 3.15, 4.73, _
    '            7.09, 9.85, 12.41, 15.75, 19.69, 30.09, 41.49, 56.19, 76.39} '....inch

    '        '....The following dimensions are in 0.001 in (mil).
    '        '........Needs to be converted to inch.
    '        H10TolArray = New Object() {1.6, 1.8, 2.2, 2.8, 3.5, 4, 4.5, 5, 6, 7, 8, _
    '                            9, 10, 12, 16, 20, 25}               '....mils


    '    ElseIf mUnit.System = "Metric" Then
    '        '----------------------------------
    '        dControlArray = New Object() {3, 6, 10, 18, 30, 50, 80, 120, 180, 250, 315, 400, _
    '                              500, 760, 1050, 1425, 1940}            '....mm

    '        '....The following dimensions are in 0.001 mm (or micron)
    '        '........Needs to be converted to m.
    '        H10TolArray = New Object() {40, 48, 58, 70, 84, 100, 120, 140, 160, 185, 210, _
    '                             230, 250, 300, 400, 500, 630}           '....microns

    '    End If

    '    '....Lower & Upper Bounds of the arrays
    '    Dim iLow As Int16 = LBound(dControlArray)
    '    Dim iUp As Int16 = UBound(dControlArray)


    '    'Get the appropriate H10 tolerance.
    '    '----------------------------------
    '    Dim pDiaUserU As Single
    '    pDiaUserU = mUnit.L_ConToUser(mSealDControl)     '....Control Dia in user unit.
    '    '                                                '........(in or mm)

    '    Dim i As Integer
    '    Dim pH10_TableUnit As Single

    '    If pDiaUserU <= dControlArray(iLow) Then
    '        pH10_TableUnit = H10TolArray(iLow)

    '    ElseIf pDiaUserU >= dControlArray(iUp) Then
    '        pH10_TableUnit = H10TolArray(iUp)

    '    Else
    '        i = 1
    '        Do While pDiaUserU >= dControlArray(i)
    '            i = i + 1
    '        Loop
    '        pH10_TableUnit = H10TolArray(i)
    '    End If

    '    '....Divide   by 1000                ==> Table unit to User unit.
    '    '........Multiply by unit.CFacUserL  ==> User unit to Consistent unit (in or m).
    '    mH10Tol = mUnit.L_UserToCon((pH10_TableUnit / 1000))

    'End Sub


    Private Function CalcWidMin() As Single
        '==================================
        'Calculate min. width of the cavity.
        '....Index 2 : Min. Cavity OD
        '....Index 1 : Max. Cavity ID,  

        If mDia(2) > mcEPS And mDia(1) > mcEPS Then
            mWidMin = 0.5 * ((mDia(2)) - (mDia(1)))
        Else
            mWidMin = 0.0#
        End If

    End Function


    Public Function CalcDia(ByVal Indx_In As Integer) As Single
        '======================================================
        'PB 21JAN16. The index should be reversed. The Indx_In should correspond to that of the Dia to be calculated. 
        '....Index 1 : Max. Cavity ID,  
        '....Index 2 : Min. Cavity OD

        Select Case Indx_In
            Case 1
                If mDia(1) > mcEPS And mWidMin > mcEPS Then
                    mDia(2) = (2 * mWidMin) + mDia(1)
                Else
                    mDia(2) = 0.0#
                End If

            Case 2
                If mDia(2) > mcEPS And mWidMin > mcEPS Then
                    mDia(1) = mDia(2) - (2 * mWidMin)
                Else
                    mDia(1) = 0.0#
                End If
        End Select

    End Function


    Public Function DepthActual(ByVal strTolType_In As String) As Single
        '===============================================================
        '....Returns actual cavity depth according to the compression tolerance type.

        If strTolType_In = "Maximum" Then         '....Minimum depth
            DepthActual = mDepth - mDepthTol(1)

        ElseIf strTolType_In = "Nominal" Then     '....Nominal depth
            DepthActual = mDepth

        ElseIf strTolType_In = "Minimum" Then     '....Maximum depth
            DepthActual = mDepth + mDepthTol(2)
        End If

    End Function

#End Region

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
