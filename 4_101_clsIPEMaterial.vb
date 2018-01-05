
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsMaterial                            '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05FEB16                                '
'                                                                              '
'===============================================================================
Imports System.IO
Imports System.Globalization
Imports System
Imports System.Threading
Imports clsLibrary11
Imports System.Windows.Forms

<Serializable()> _
Public Class IPE_clsMaterial

#Region "NAMED CONSTANT:"

    Private Const mcDirMatData As String = "C:\SealSuite\WorkDir\V100\"
    Private Const mcFileTitle As String = "Material2d.Dat"  'AM 12FEB10 '"Material2c.Dat"
    Private Const mcPlatingFileTitle As String = "MaterialPlating1.Dat"

    Public Const mcTRoom_English As Single = 70     '....Standard Room Temperature (F)

#End Region


#Region "MEMBER VARIABLE:"

    '....Material File                      
    Private mFileName As String
    Private mFileTitle As String
    Private mFileDir As String
    '....Plating File
    Private mPlatingFileName As String
    Private mPlatingFileTitle As String
    Private mPlatingFileDir As String

    Private mUnit As IPE_clsUnit                    '(FUNDAMENTAL)
    Private mName As String
    Private mHT As Integer                      'AES 13JAN16
    Private mCoating As String
    Private mTRoom As Single
    Private mTOper As Single

    '25FEB11
    'Private mEmod As Single                     '....Young's Modulus    (psi or MPa)
    'Private mNu As Single                       '....Poisson's Ratio    (psi or MPa)
    Private mModel As String                     '....Material Model e.g. "Bilinear Kinematic"
    'Private mTanMod As Single                   '....Tangent Modulus    (psi or MPa)

    Dim mNTemp As Integer

    '....Dynamic arrays
    Dim mTemp() As Single
    Dim mNu() As Single
    Dim mEMod() As Single
    Dim mSigmaY() As Single
    Dim mTanMod() As Single

    '....Yield Strengths (psi or MPa):     
    Private mSigmaY_TRoom As Single             '....@TRoom
    Private mSigmaY_TOper As Single             '....@TOper

    Private mSpWt As Single                     '....Specific Wt.       (lbf/in^3 or N/m^3)
    Private mTOperLimitUp As Single             '....Operating Temp. - Upper Limit. (F or C)

#End Region


#Region "PROPERTY ROUTINE"

#Region "READ-ONLY PROPERTIES:"

    Public ReadOnly Property FileName() As String
        '========================================
        Get
            Return mFileName
        End Get
    End Property

    Public ReadOnly Property PlatingFileName() As String
        '================================================
        Get
            Return mPlatingFileName
        End Get
    End Property

    '....No. of Temp
    Public ReadOnly Property NTemp() As Integer
        '=======================================                
        Get
            Return mNTemp
        End Get
    End Property

    '....Temp
    Public ReadOnly Property Temp(ByVal i As Integer) As Single
        '====================================================== 
        Get
            Return mTemp(i)
        End Get
    End Property

    '....Young's Modulus (psi)
    Public ReadOnly Property Emod(ByVal i As Integer) As Single
        '====================================================== 
        Get
            Return mEMod(i)
        End Get
    End Property


    '....Poisson's Ratio
    Public ReadOnly Property Nu(ByVal i As Integer) As Single
        '======================================================  
        Get
            Return mNu(i)
        End Get
    End Property

    '....Yield Strength
    Public ReadOnly Property SigmaY(ByVal i As Integer) As Single
        '=========================================================  
        Get
            Return mSigmaY(i)
        End Get
    End Property

    '....Tangent Modulus (psi)
    Public ReadOnly Property TanMod(ByVal i As Integer) As Single
        '=========================================================  
        Get
            Return mTanMod(i)
        End Get
    End Property


    '....Material Model e.g. "Bilinear Kinematic"
    Public ReadOnly Property Model() As String
        '=====================================
        Get
            Return mModel
        End Get
    End Property


    '........@TRoom.
    Public ReadOnly Property SigmaY_TRoom() As Single
        '============================================
        Get
            Return mSigmaY_TRoom
        End Get
    End Property

    '........@TOper.
    Public ReadOnly Property SigmaY_TOper() As Single
        '============================================
        Get
            Return mSigmaY_TOper
        End Get
    End Property


    Public ReadOnly Property SpWt() As Single
        '====================================
        Get
            Return mSpWt
        End Get
    End Property


    Public ReadOnly Property TOperLimitUp() As Single
        '============================================
        Get
            Return mTOperLimitUp
        End Get
    End Property

#End Region


#Region "READ & WRITE PROPERTIES:"

    '....UnitSystem.
    Public Property UnitSystem() As String
        '==================================
        Get
            Return mUnit.System
        End Get

        Set(ByVal strData As String)
            '-----------------------
            mUnit.System = strData
            UpdateProp()
        End Set
    End Property


    Public Property Name() As String
        '===========================
        Get
            Return mName
        End Get

        Set(ByVal strValue As String)
            '-----------------------
            mName = strValue
            UpdateProp()
        End Set
    End Property


    Public Property HT() As Integer
        '===========================
        Get
            Return mHT
        End Get

        Set(ByVal Value As Integer)
            '-----------------------
            mHT = Value
        End Set
    End Property


    Public Property Coating() As String
        '===========================
        Get
            Return mCoating
        End Get

        Set(ByVal strValue As String)
            '-----------------------
            mCoating = strValue

        End Set
    End Property


    '....TOper.
    Public Property TOper() As Single
        '===========================
        Get
            Return mTOper
        End Get

        Set(ByVal sngValue As Single)
            '------------------------
            mTOper = sngValue
            UpdateProp()
        End Set
    End Property


    Public ReadOnly Property PlatingFileTitle() As String
        '================================================
        Get
            If mPlatingFileName <> "" Then
                Dim iPos As Integer = InStrRev(mPlatingFileName, "\")
                mPlatingFileTitle = Mid(mPlatingFileName, iPos + 1)
            Else
                mPlatingFileTitle = ""
            End If

            Return mPlatingFileTitle
        End Get

    End Property


    Public ReadOnly Property PlatingFileDir() As String
        '===========================================
        Get
            If mPlatingFileName <> "" Then
                Dim iPos As Integer = InStrRev(mPlatingFileName, "\")
                mPlatingFileDir = Mid(mPlatingFileName, 1, iPos - 1)
            Else
                mPlatingFileDir = ""
            End If

            Return mPlatingFileDir
        End Get

    End Property


    Public ReadOnly Property FileTitle() As String
        '===========================================
        Get
            If mFileName <> "" Then
                Dim iPos As Integer = InStrRev(mFileName, "\")
                mFileTitle = Mid(mFileName, iPos + 1)
            Else
                mFileTitle = ""
            End If

            Return mFileTitle
        End Get

    End Property


    Public ReadOnly Property FileDir() As String
        '===========================================
        Get
            If mFileName <> "" Then
                Dim iPos As Integer = InStrRev(mFileName, "\")
                mFileDir = Mid(mFileName, 1, iPos - 1)
            Else
                mFileDir = ""
            End If

            Return mFileDir
        End Get

    End Property

#End Region


#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(Optional ByVal strUnitSystem_In As String = "English")
        '================================================================

        Try
            mFileName = mcDirMatData & mcFileTitle
            mPlatingFileName = mcDirMatData & mcPlatingFileTitle
            mUnit = New IPE_clsUnit(strUnitSystem_In)
            mCoating = "None"
        Catch
            MsgBox(Err.Description)
        End Try

    End Sub
#End Region


#Region "UTILITY ROUTINES:"

    Public Sub PopulateMaterialList(ByRef cmbBox As ComboBox)
        '=====================================================
        'This routine reads the material data file and retrieves the material list
        '....items for the corresponding combo box.
        '---------------------------------------------------------------------

        Try

            Dim pSR As StreamReader
            pSR = File.OpenText(mFileName)

            Dim pstrAny As String
            Dim i As Int16
            Dim nItem As Int16
            Dim iPos As Int16

            Do Until pSR.Peek = -1  '....Until no more characters to read.

                With pSR
                    '....Go to the keyword "List"
                    pstrAny = pSR.ReadLine

                    Do Until Left(pstrAny, 4) = "List"
                        pstrAny = pSR.ReadLine
                    Loop
                    pSR.ReadLine()

                    pstrAny = pSR.ReadLine
                    nItem = Val(Left(pstrAny, 3))

                    For i = 0 To nItem - 1
                        pstrAny = pSR.ReadLine
                        iPos = InStr(1, pstrAny, ",")

                        cmbBox.Items.Add(Trim(Mid(pstrAny, 1, iPos - 1)))
                    Next

                End With

            Loop

            cmbBox.SelectedIndex = 1
            pSR.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub


    Private Sub UpdateProp()
        '===================
        'This routine call is triggered by the following property set calls:
        '   1. UnitSystem 
        '   2. Name
        '   3. TOper
        '
        'This routine reads the "MatFile" and retrieves/updates the following parameters: 
        '   1. Yield Strength (SigmaY) @ TOper & @ TRoom
        '   2. Upper limit of the material operating temperature.
        '   3. Density @ TOper.
        '--------------------------------------------------------------------------------

        ' 'Unit Aware' Routine
        '  -------------------

        'TRoom. 
        '------
        mTRoom = mcTRoom_English * mUnit.CFacConT + mUnit.CFacConTOff

        Try
            Dim pSR As StreamReader
            pSR = File.OpenText(mFileName)

            'Index to the data section for the "mName".
            '------------------------------------------
            Dim pstrAny As String = pSR.ReadLine

            Do While mName <> Trim(pstrAny)
                pstrAny = pSR.ReadLine
                If mName = Trim(pstrAny) Then Exit Do
            Loop


            'Specific Wt.
            '------------
            '
            '....Index to the keyword "dens"
            Do While Left(pstrAny, 2) <> "mp"
                pstrAny = pSR.ReadLine
            Loop

            pstrAny = ExtractPostData(pstrAny, ",")
            Dim pstrAny1 As String = UCase(Trim(ExtractPreData(pstrAny, ",")))


            Dim pSpWt_Eng As Single, pDens_Eng As Single
            If pstrAny1 = "DENS" Then
                pstrAny = ExtractPostData(pstrAny, ",")
                pstrAny = ExtractPostData(pstrAny, ",")

                '....Specific Weight & Density have the same magnitude in English system but 
                '........different units e.g. lbf/in^3 and lbm/in^3.

                pstrAny = ExtractPreData(pstrAny, "!").Trim()
                'pDens_Eng = Convert.ToSingle(Trim(ExtractPreData(pstrAny, "!")))

                Dim pCI As New CultureInfo("en-US")
                Dim pDensity As Single = Convert.ToSingle(pstrAny, pCI) 'SG 03JUN08

                pDens_Eng = pDensity
                'pDens_Eng = Single.Parse(pstrAny, NumberStyles.Number)

                pSpWt_Eng = pDens_Eng
            End If

            '....Unit Conversion: English System ===> mUnit.System
            mSpWt = pSpWt_Eng * mUnit.CFacConF / (mUnit.CFacConL) ^ 3


            ''SigmaY' at 'TRoom' & 'TOper'
            '-----------------------------

            '....Index to the keyword "nTemp"
            Do While Left(pstrAny, 5) <> "nTemp"
                pstrAny = pSR.ReadLine
            Loop

            Dim pintNTemp As Int16 = Val(ExtractPostData(pstrAny, "="))

            '....Dynamic arrays
            Dim psngTemp() As Single
            Dim psngSigmaY() As Single

            ReDim psngTemp(pintNTemp)
            ReDim psngSigmaY(pintNTemp)


            '....Go to each occurence of the keyword "tbTemp" and store the
            '........corresponding Temp & SigmaY values in the defined arrays.

            Dim i As Int16
            For i = 1 To pintNTemp
                pstrAny = pSR.ReadLine

                Do While Left(pstrAny, 6) <> "tbtemp"
                    pstrAny = pSR.ReadLine
                Loop

                pstrAny1 = ExtractPostData(pstrAny, ",")

                Dim TempI_Eng As Single
                TempI_Eng = Val(ExtractPreData(pstrAny1, "*"))

                '....Unit Conversion: English System ===> mUnit.System
                psngTemp(i) = TempI_Eng * mUnit.CFacConT + mUnit.CFacConTOff


                pstrAny = pSR.ReadLine
                pstrAny1 = ExtractPostData(pstrAny, "1")
                pstrAny1 = ExtractPostData(pstrAny1, ",")

                Dim SigmaYI_Eng As Single
                SigmaYI_Eng = Val(ExtractPreData(pstrAny1, "*"))

                '....Unit Conversion: English System ===> mUnit.System
                psngSigmaY(i) = SigmaYI_Eng * mUnit.CFacConP
            Next


            'PB 10SEP09. V 6.0 Release Error dated 29MAR09 fixed. 
            '....@ TRoom:
            '------------
            '
            Dim iLow, iUp As Int16

            If mTRoom <= psngTemp(1) Then               '....Extrapolation
                iLow = 1
                iUp = 2

            ElseIf mTRoom > psngTemp(pintNTemp) Then    '....Extrapolation
                iLow = pintNTemp - 1
                iUp = pintNTemp

            Else                                        '....Interpolation
                i = 1
                Do While mTRoom > psngTemp(i)
                    i = i + 1
                Loop

                iUp = i
                iLow = i - 1
            End If

            mSigmaY_TRoom = Interpolate(psngTemp(iLow), psngSigmaY(iLow), _
                                psngTemp(iUp), psngSigmaY(iUp), mTRoom)

            '....@ TOper:
            '------------
            '
            If mTOper <= psngTemp(1) Then               '....Extrapolation
                iLow = 1
                iUp = 2

            ElseIf mTOper > psngTemp(pintNTemp) Then    '....Extrapolation
                iLow = pintNTemp - 1
                iUp = pintNTemp

            Else                                        '....Interpolation
                i = 1
                Do While mTOper > psngTemp(i)
                    i = i + 1
                Loop

                iUp = i
                iLow = i - 1
            End If

            mSigmaY_TOper = Interpolate(psngTemp(iLow), psngSigmaY(iLow), _
                                        psngTemp(iUp), psngSigmaY(iUp), mTOper)


            'Retrieve the Upper Limit of TOper
            '----------------------------------
            '....Index to given material name at the end of the file.
            pstrAny = pSR.ReadLine
            Do While mName <> ExtractPreData(pstrAny, ",")
                pstrAny = pSR.ReadLine
            Loop

            Dim pTOperLimitUp_Eng As Single
            pTOperLimitUp_Eng = Val(ExtractPostData(pstrAny, ","))

            '....Unit Conversion: English System ===> mUnit.System
            mTOperLimitUp = pTOperLimitUp_Eng * mUnit.CFacConT + mUnit.CFacConTOff


            pSR.Close()

        Catch 'pEXP As Exception
            MsgBox(Err.Description)
        End Try

    End Sub


    Public Function IsCoatingExists(ByVal MatName_In As String, _
                                    ByVal Coating_In As String) As Boolean
        '==================================================================
        Try
            Dim pSR As StreamReader
            pSR = File.OpenText(mFileName)

            'Index to the data section for the "MaterialName_CoatingName".
            '--------------------------------------------------------------
            Dim pstrMat As String = MatName_In & "_" & Coating_In
            Dim pstrAny As String = pSR.ReadToEnd()

            If pstrAny.Contains(pstrMat) Then
                IsCoatingExists = True
            Else
                IsCoatingExists = False
            End If

            pSR.Close()

        Catch ex As Exception
            MsgBox(Err.Description)
        End Try
    End Function


    Private Sub RetrieveNTemp(ByVal MatName_In As String, _
                             ByVal Coating_In As String)
        '=================================================

        Try
            Dim pSR As StreamReader
            pSR = File.OpenText(mFileName)

            'Index to the data section for the "mName".
            '------------------------------------------
            Dim pstrMat As String = ""
            Dim pstrAny As String = pSR.ReadLine

            If MatName_In <> "" And Coating_In <> "None" Then
                pstrMat = MatName_In & "_" & Coating_In
                Do While pstrMat <> Trim(pstrAny)
                    pstrAny = pSR.ReadLine
                    If pstrMat = Trim(pstrAny) Then Exit Do
                Loop
            ElseIf MatName_In <> "" And Coating_In = "None" Then
                Do While MatName_In <> Trim(pstrAny)
                    pstrAny = pSR.ReadLine
                    If MatName_In = Trim(pstrAny) Then Exit Do
                Loop
            End If

            '....No. of Temp
            '....Index to the keyword "nTemp"
            Do While Left(pstrAny, 5) <> "nTemp"
                pstrAny = pSR.ReadLine
            Loop

            Dim pintNTemp As Int16 = Val(ExtractPostData(pstrAny, "="))
            mNTemp = pintNTemp

            ReDim mTemp(pintNTemp)
            ReDim mSigmaY(pintNTemp)
            ReDim mTanMod(pintNTemp)
            ReDim mNu(pintNTemp)
            ReDim mEMod(pintNTemp)

            pSR.Close()

        Catch ex As Exception
            MsgBox(Err.Description)
        End Try

    End Sub


    Public Sub RetrieveProp(ByVal MatName_In As String, _
                            ByVal Coating_In As String)
        '===============================================       
        'This routine reads the "MatFile" and retrieves the following parameters: 
        '   1. Yield Strength (SigmaY) @ TOper & @ TRoom
        '   2. Upper limit of the material operating temperature.
        '   3. Density @ TOper.
        '--------------------------------------------------------------------------------

        ' 'Unit Aware' Routine
        '  -------------------  
        Dim pstrMat As String = "", pstrAny As String, pstrAny1 As String = ""
        Dim pstrVal() As String

        Dim pCI As New CultureInfo("en-US")

        '....No of Temp.
        RetrieveNTemp(MatName_In, Coating_In)

        Try
            Dim pSR As StreamReader
            pSR = File.OpenText(mFileName)

            'Index to the data section for the "mName".
            '------------------------------------------
            pstrAny = pSR.ReadLine

            If MatName_In <> "" And Coating_In <> "None" Then
                pstrMat = MatName_In & "_" & Coating_In
                Do While pstrMat <> Trim(pstrAny)
                    pstrAny = pSR.ReadLine
                    If pstrMat = Trim(pstrAny) Then Exit Do
                Loop
            ElseIf MatName_In <> "" And Coating_In = "None" Then
                Do While MatName_In <> Trim(pstrAny)
                    pstrAny = pSR.ReadLine
                    If MatName_In = Trim(pstrAny) Then Exit Do
                Loop
            End If

            'Specific Wt.
            '------------
            '
            '....Index to the keyword "dens"
            Do While Left(pstrAny, 2) <> "mp"
                pstrAny = pSR.ReadLine
            Loop

            pstrAny = ExtractPostData(pstrAny, ",")

            Dim pDensity As Single

            If UCase(pstrAny).Contains("DENS") Then
                If pstrAny.Contains("!") Then _
                    pstrAny = ExtractPreData(pstrAny, "!") '....Remove Comment line
                pstrVal = pstrAny.Split(",")

                Dim pIndx As Integer = pstrVal.Length - 1
                pDensity = Convert.ToSingle(pstrVal(pIndx).Trim(), pCI)

            End If

            '....Specific Weight & Density have the same magnitude in English system but 
            '........different units e.g. lbf/in^3 and lbm/in^3.

            Dim pSpWt_Eng As Single, pDens_Eng As Single
            pDens_Eng = pDensity

            pSpWt_Eng = pDens_Eng

            '....Unit Conversion: English System ===> mUnit.System
            mSpWt = pSpWt_Eng * mUnit.CFacConF / (mUnit.CFacConL) ^ 3


            '....Emod and Nu
            '----------------
            '
            Dim i As Int16

            '....Index to the keyword "mpdata"
            Do While Left(pstrAny, 6) <> "mpdata"
                pstrAny = pSR.ReadLine
            Loop

            Dim EMod_Eng As Single              '....Young Modulus

            If pstrAny.Contains("ex") Then
                If pstrAny.Contains("!") Then _
                    pstrAny = ExtractPreData(pstrAny, "!") '....Remove Comment line

                For i = 0 To 3
                    pstrAny = ExtractPostData(pstrAny, ",")
                Next

                For i = 1 To mNTemp
                    EMod_Eng = Convert.ToSingle(ExtractPreData(pstrAny, "e").Trim(), pCI)
                    pstrAny = ExtractPostData(pstrAny, ",")

                    '....Unit Conversion: English System ===> mUnit.System
                    mEMod(i) = EMod_Eng * mUnit.CFacConT + mUnit.CFacConTOff
                Next

            End If

            pstrAny = pSR.ReadLine

            Dim Nu_Eng As Single                '....Poisson's Ratio
            If pstrAny.Contains("nuxy") Then
                If pstrAny.Contains("!") Then _
                    pstrAny = ExtractPreData(pstrAny, "!") '....Remove Comment line

                For i = 0 To 3
                    pstrAny = ExtractPostData(pstrAny, ",")
                Next

                For i = 1 To mNTemp
                    If pstrAny.Contains(",") Then
                        Nu_Eng = Convert.ToSingle(ExtractPreData(pstrAny, ",").Trim(), pCI)
                    Else
                        Nu_Eng = Convert.ToSingle(pstrAny.Trim(), pCI)
                    End If
                    pstrAny = ExtractPostData(pstrAny, ",")

                    '....Unit Conversion: English System ===> mUnit.System
                    mNu(i) = Nu_Eng * mUnit.CFacConT + mUnit.CFacConTOff
                Next
            End If


            'Temp, 'SigmaY' and Tangent Modulus
            '-----------------------------
            '....Go to each occurence of the keyword "tbTemp" and store the
            '........corresponding Temp & SigmaY values in the defined arrays.


            For i = 1 To mNTemp
                pstrAny = pSR.ReadLine

                Do While Left(pstrAny, 6) <> "tbtemp"
                    pstrAny = pSR.ReadLine
                Loop

                pstrAny = ExtractPostData(pstrAny, ",")

                Dim TempI_Eng As Single
                TempI_Eng = Convert.ToSingle(ExtractPreData(pstrAny, "*"), pCI)

                '....Unit Conversion: English System ===> mUnit.System
                mTemp(i) = TempI_Eng * mUnit.CFacConT + mUnit.CFacConTOff


                pstrAny = pSR.ReadLine
                If UCase(pstrAny).Contains("TBDATA") Then
                    If pstrAny.Contains("!") Then _
                    pstrAny = ExtractPreData(pstrAny, "!") '....Remove Comment line

                    pstrAny = ExtractPostData(pstrAny, "1")
                    pstrAny = ExtractPostData(pstrAny, ",")
                End If

                Dim SigmaYI_Eng As Single
                SigmaYI_Eng = Convert.ToSingle(ExtractPreData(pstrAny, "e"), pCI)

                '....Unit Conversion: English System ===> mUnit.System
                mSigmaY(i) = SigmaYI_Eng * mUnit.CFacConP

                Dim TangentMod_Eng As Single
                pstrAny = ExtractPostData(pstrAny, ",")
                TangentMod_Eng = Convert.ToSingle(ExtractPreData(pstrAny, "e"), pCI)

                '....Unit Conversion: English System ===> mUnit.System
                mTanMod(i) = TangentMod_Eng * mUnit.CFacConP
            Next

            pSR.Close()

        Catch 'pEXP As Exception
            MsgBox(Err.Description)
        End Try

    End Sub


    Private Function Interpolate(ByRef x1 As Single, ByRef y1 As Single, _
                                 ByRef x2 As Single, ByRef y2 As Single, _
                                 ByRef xg As Single) As Single
        '===================================================================
        Dim Ratio As Single

        Ratio = (y1 - y2) / (x2 - x1)
        Interpolate = y2 + Ratio * (x2 - xg)

    End Function

#End Region


End Class
