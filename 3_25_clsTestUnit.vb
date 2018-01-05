'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_Unit                           '
'                        VERSION NO  :  10.1.0                                 '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03MAR17                                '
'                                                                              '
'===============================================================================

Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Linq
Imports clsLibrary11
Imports System.Globalization.CultureInfo
Imports System.Math

<Serializable()> _
Public Class Test_clsUnit
    Implements ICloneable

    Dim mcLUnit_List() As String = New String() {"in", "mm"}
    Dim mcFUnit_List() As String = New String() {"lbf", "N", "kgf"}
    Dim mcPUnit_List() As String = New String() {"psi", "kPa", "atm", "bar"}
    Dim mcLeakUnit_List() As String = New String() {"cc/s", "mL/min", "L/min", "scfm", "sccm", "mbar L/s"}      ' "mbar l/s"


#Region "Structures:"
    <Serializable()> _
    Public Structure sPH
        Public LIndx As Integer
        Public FIndx As Integer
        Public PIndx As Integer
        Public LeakIndx As Integer
    End Structure


    <Serializable()> _
    Public Structure sCust
        Public LIndx As Integer
        Public FIndx As Integer
        Public PIndx As Integer
        Public LeakIndx As Integer
    End Structure
#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"
    Private Const mcPcentValMin As Integer = 1
    Public mPH As sPH
    Public mCust As sCust

    '....User unit Format:
    Private mLFormat_PH As String = "##0.000"
    Private mLFormat_Cust As String = "##0.000"

#End Region


#Region "CLASS PROPERTY ROUTINE:"


    Public ReadOnly Property LUnit_List() As String()
        '============================================
        Get
            Return mcLUnit_List
        End Get

    End Property


    Public ReadOnly Property FUnit_List() As String()
        '============================================
        Get
            Return mcFUnit_List
        End Get

    End Property


    Public ReadOnly Property PUnit_List() As String()
        '============================================
        Get
            Return mcPUnit_List
        End Get

    End Property


    Public ReadOnly Property LeakUnit_List() As String()
        '===============================================
        Get
            Return mcLeakUnit_List
        End Get

    End Property


#Region "PH:"

    Public WriteOnly Property LIndx_PH() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mPH.LIndx = Value
        End Set
    End Property


    Public WriteOnly Property FIndx_PH() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mPH.FIndx = Value
        End Set
    End Property


    Public WriteOnly Property PIndx_PH() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mPH.PIndx = Value
        End Set
    End Property


    Public WriteOnly Property LeakIndx_PH() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mPH.LeakIndx = Value
        End Set
    End Property

    Public ReadOnly Property LUnit_PH() As String
        '========================================
        Get
            Return mcLUnit_List(mPH.LIndx)
        End Get
    End Property

    Public ReadOnly Property FUnit_PH() As String
        '========================================
        Get
            Return mcFUnit_List(mPH.FIndx)
        End Get

    End Property

    Public ReadOnly Property PUnit_PH() As String
        '========================================
        Get
            Return mcPUnit_List(mPH.PIndx)
        End Get

    End Property

    Public ReadOnly Property LeakUnit_PH() As String
        '========================================
        Get
            Return mcLeakUnit_List(mPH.LeakIndx)
        End Get

    End Property

#End Region

#Region "CUST:"

    Public WriteOnly Property LIndx_Cust() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mCust.LIndx = Value
        End Set
    End Property


    Public WriteOnly Property FIndx_Cust() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mCust.FIndx = Value
        End Set
    End Property


    Public WriteOnly Property PIndx_Cust() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mCust.PIndx = Value
        End Set
    End Property


    Public WriteOnly Property LeakIndx_Cust() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mCust.LeakIndx = Value
        End Set
    End Property

    Public ReadOnly Property LUnit_Cust() As String
        '========================================
        Get
            Return mcLUnit_List(mCust.LIndx)
        End Get

    End Property

    Public ReadOnly Property FUnit_Cust() As String
        '========================================
        Get
            Return mcFUnit_List(mCust.FIndx)
        End Get

    End Property

    Public ReadOnly Property PUnit_Cust() As String
        '========================================
        Get
            Return mcPUnit_List(mCust.PIndx)
        End Get

    End Property

    Public ReadOnly Property LeakUnit_Cust() As String
        '========================================
        Get
            Return mcLeakUnit_List(mCust.LeakIndx)
        End Get

    End Property

#End Region


#End Region


#Region "DB RELATED ROUTINES"

    Public Sub RetrieveFrom_DB(ByVal TestProjectID_In As Integer)
        '==========================================================
        Try

            Dim pSealTestEntities As New SealTestDBEntities()

            Dim pQry = (From pRec In pSealTestEntities.tblUnit
                         Where pRec.fldTestProjectID = TestProjectID_In Select pRec).ToList()

            If (pQry.Count > 0) Then

                '....PH
                mPH.LIndx = Array.IndexOf(mcLUnit_List, pQry(0).fldLUnitPH)
                mPH.FIndx = Array.IndexOf(mcFUnit_List, pQry(0).fldFUnitPH)
                mPH.PIndx = Array.IndexOf(mcPUnit_List, pQry(0).fldPUnitPH)
                mPH.LeakIndx = Array.IndexOf(mcLeakUnit_List, pQry(0).fldLeakUnitPH)

                '....Cust
                mCust.LIndx = Array.IndexOf(mcLUnit_List, pQry(0).fldLUnitCust)
                mCust.FIndx = Array.IndexOf(mcFUnit_List, pQry(0).fldFUnitCust)
                mCust.PIndx = Array.IndexOf(mcPUnit_List, pQry(0).fldPUnitCust)
                mCust.LeakIndx = Array.IndexOf(mcLeakUnit_List, pQry(0).fldLeakUnitCust)
            End If

            SetValues()

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveTo_DB(ByVal TestProjectID_In As Integer)
        '==================================================
        Try
            Dim pSealTestEntities As New SealTestDBEntities()
            Dim pQry = (From pRec In pSealTestEntities.tblUnit
                         Where pRec.fldTestProjectID = TestProjectID_In Select pRec).ToList()

            Dim pTestUnit As New tblUnit

            If (pQry.Count > 0) Then
                pTestUnit = pQry(0)
            End If

            With pTestUnit
                .fldTestProjectID = TestProjectID_In
                .fldLUnitPH = mcLUnit_List(mPH.LIndx)
                .fldFUnitPH = mcFUnit_List(mPH.FIndx)
                .fldPUnitPH = mcPUnit_List(mPH.PIndx)
                .fldLeakUnitPH = mcLeakUnit_List(mPH.LeakIndx)

                .fldLUnitCust = mcLUnit_List(mCust.LIndx)
                .fldFUnitCust = mcFUnit_List(mCust.FIndx)
                .fldPUnitCust = mcPUnit_List(mCust.PIndx)
                .fldLeakUnitCust = mcLeakUnit_List(mCust.LeakIndx)
            End With

            If (pQry.Count > 0) Then
                pSealTestEntities.SaveChanges()
            Else
                pSealTestEntities.AddTotblUnit(pTestUnit)
                pSealTestEntities.SaveChanges()

            End If
        Catch ex As Exception

        End Try

    End Sub

#End Region


#Region "MAIN ROUTINES:"

    Public Function ConvF(ByVal Type_In As String) As Double
        '====================================================
        '....Conversion Factor List
        Dim pcConvF_LList(,) As Double = {{1.0, 1 / 25.4},
                                          {25.4, 1.0}}

        Dim pcConvF_FList(,) As Double = {{1.0, 0.224809, 0.101972},
                                         {1.0 / 0.224809, 1.0, 0.453592},
                                         {1.0 / 0.101972, 1.0 / 0.453592, 1.0}}

        Dim pcConvF_PList(,) As Double = {{1.0, 6.89476, 0.068046, 0.0689476},
                                          {1 / 6.89476, 1.0, 0.00986923, 0.01},
                                          {1 / 0.06846, 1 / 0.00986923, 1.0, 1.01325},
                                          {1 / 0.0689476, 100, 1 / 1.01325, 1.0}}

        Dim pcConvF_LeakList(,) As Double = {{1.0, 1 / 1000, 0.0000353, 1.0, 1 / 60},
                                             {1000, 1.0, 0.0353, 1000, 1000 / 60},
                                             {1 / 0.000353, 1 / 0.0353, 1.0, 28316.847, 471.947},
                                             {1.0, 1 / 1000, 1 / 28316.847, 1.0, 1 / 60},
                                             {60, 60 / 1000, 1 / 471.947, 60, 1.0}}

        If (Type_In = "L") Then
            Return pcConvF_LList(mPH.LIndx, mCust.LIndx)

        ElseIf (Type_In = "F") Then
            Return pcConvF_FList(mPH.FIndx, mCust.FIndx)

        ElseIf (Type_In = "P") Then
            Return pcConvF_PList(mPH.PIndx, mCust.PIndx)

        ElseIf (Type_In = "Leak") Then
            Return pcConvF_LeakList(mPH.LeakIndx, mCust.LeakIndx)
        Else
            Return 1.0
        End If

    End Function


    Public Sub SetValues()
        '==================
        If (mcLUnit_List(mPH.LIndx) = "mm") Then
            mLFormat_PH = "###0.00#"

        ElseIf (mcLUnit_List(mPH.LIndx) = "in") Then
            mLFormat_PH = "##0.000"

        End If

        If (mcLUnit_List(mCust.LIndx) = "mm") Then
            'mLFormat_Cust = "###0.00"
            mLFormat_Cust = "#.00#"
        ElseIf (mcLUnit_List(mCust.LIndx) = "in") Then
            'mLFormat_Cust = "##0.000"
            mLFormat_Cust = "#.000#"
        End If

    End Sub


    Public Function FormatLoadVal_Cust(ByVal Load_In As Double) As String
        '===============================================================     'AES 02AUG17
        Dim pLoad_Cust As Double
        pLoad_Cust = Load_In * ConvF("F")

        Dim pDecimal As Double = Math.Abs(pLoad_Cust - NInt(pLoad_Cust))
        Dim pDecimal_Percent As Double = (pDecimal / pLoad_Cust) * 100.0#

        If pDecimal_Percent.ToString = "NaN" Then
            pDecimal_Percent = 0.0
        End If

        If (pDecimal_Percent > mcPcentValMin) Then
            FormatLoadVal_Cust = pLoad_Cust.ToString("#.0") 'WriteInUserL_Cust(pLoad_Cust)
        Else
            FormatLoadVal_Cust = NInt(pLoad_Cust)
        End If

    End Function


    Public Function WriteInUserL_PH(ByVal sngData_In As Double) As String
        '=============================================================
        '........formats appropriately and returns the corresponding string.

        '....Default Format: mLFormat.

        If Abs(sngData_In) > gcEPS Then
            WriteInUserL_PH = sngData_In.ToString(mLFormat_PH, CurrentCulture)
        Else
            WriteInUserL_PH = sngData_In.ToString(mLFormat_PH, CurrentCulture)
        End If

    End Function


    Public Function WriteInUserL_Cust(ByVal sngData_In As Double) As String
        '=============================================================
        '........formats appropriately and returns the corresponding string.

        '....Default Format: mLFormat.

        If Abs(sngData_In) > gcEPS Then
            WriteInUserL_Cust = sngData_In.ToString(mLFormat_Cust, CurrentCulture)
        Else
            Dim pLFormat As String = "#0"
            WriteInUserL_Cust = sngData_In.ToString(pLFormat, CurrentCulture)
            'mLFormat_Cust As String = "##0.000"
        End If

    End Function

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
