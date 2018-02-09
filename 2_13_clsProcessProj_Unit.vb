'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_Unit                    '
'                        VERSION NO  :  1.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB18                                '
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

<Serializable()>
Public Class clsProcessProj_Unit
    Implements ICloneable

    Dim mcLUnit_List() As String = New String() {"in", "mm"}
    Dim mcFUnit_List() As String = New String() {"lbf", "N", "kgf"}
    Dim mcPUnit_List() As String = New String() {"psi", "kPa", "bar", "atm"}
    Dim mcTUnit_List() As String = New String() {"ºF", "ºC"}
    Dim mcLeakUnit_List() As String = New String() {"scfm", "cc/s", "mL/min", "L/min", "sccm", "mbar L/s"}

#Region "Structures:"
    <Serializable()>
    Public Structure sPH
        Public LIndx As Integer
        Public FIndx As Integer
        Public PIndx As Integer
        Public TIndx As Integer
        Public LeakIndx As Integer
    End Structure


    <Serializable()>
    Public Structure sCust
        Public LIndx As Integer
        Public FIndx As Integer
        Public PIndx As Integer
        Public TIndx As Integer
        Public LeakIndx As Integer
    End Structure
#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"
    Public mPH As sPH
    Public mCust As sCust

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

    Public ReadOnly Property TUnit_List() As String()
        '============================================
        Get
            Return mcTUnit_List
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

    Public WriteOnly Property TIndx_PH() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mPH.TIndx = Value
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

    Public ReadOnly Property TUnit_PH() As String
        '========================================
        Get
            Return mcTUnit_List(mPH.TIndx)
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

    Public WriteOnly Property TIndx_Cust() As Integer
        '=========================================
        Set(ByVal Value As Integer)
            mCust.TIndx = Value
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

    Public ReadOnly Property TUnit_Cust() As String
        '========================================
        Get
            Return mcTUnit_List(mCust.TIndx)
        End Get

    End Property

    Public ReadOnly Property LeakUnit_Cust() As String
        '========================================
        Get
            Return mcLeakUnit_List(mCust.LeakIndx)
        End Get

    End Property

#End Region

    Public Sub SetDefaultVal()
        '======================
        '....PH
        mPH.LIndx = Array.IndexOf(mcLUnit_List, "in")
        mPH.FIndx = Array.IndexOf(mcFUnit_List, "lbf")
        mPH.PIndx = Array.IndexOf(mcPUnit_List, "psi")
        mPH.TIndx = Array.IndexOf(mcTUnit_List, "ºF")
        mPH.LeakIndx = Array.IndexOf(mcLeakUnit_List, "cc/s")

        '....Cust
        mCust.LIndx = Array.IndexOf(mcLUnit_List, "in")
        mCust.FIndx = Array.IndexOf(mcFUnit_List, "lbf")
        mCust.PIndx = Array.IndexOf(mcPUnit_List, "psi")
        mCust.TIndx = Array.IndexOf(mcTUnit_List, "ºF")
        mCust.LeakIndx = Array.IndexOf(mcLeakUnit_List, "cc/s")

    End Sub

#End Region

#Region "DB RELATED ROUTINES"

    Public Sub RetrieveFrom_DB(ByVal ProcessProjectID_In As Integer)
        '==========================================================
        Try

            Dim pSealProcessEntities As New SealProcessDBEntities()

            Dim pQry = (From pRec In pSealProcessEntities.tblProcess_Unit
                        Where pRec.fldProcessProjectID = ProcessProjectID_In Select pRec).ToList()

            If (pQry.Count > 0) Then

                '....PH
                mPH.LIndx = Array.IndexOf(mcLUnit_List, pQry(0).fldLUnitPH)
                mPH.FIndx = Array.IndexOf(mcFUnit_List, pQry(0).fldFUnitPH)
                mPH.PIndx = Array.IndexOf(mcPUnit_List, pQry(0).fldPUnitPH)
                mPH.TIndx = Array.IndexOf(mcTUnit_List, pQry(0).fldTUnitPH)
                mPH.LeakIndx = Array.IndexOf(mcLeakUnit_List, pQry(0).fldLeakUnitPH)

                '....Cust
                mCust.LIndx = Array.IndexOf(mcLUnit_List, pQry(0).fldLUnitCust)
                mCust.FIndx = Array.IndexOf(mcFUnit_List, pQry(0).fldFUnitCust)
                mCust.PIndx = Array.IndexOf(mcPUnit_List, pQry(0).fldPUnitCust)
                mCust.TIndx = Array.IndexOf(mcTUnit_List, pQry(0).fldTUnitCust)
                mCust.LeakIndx = Array.IndexOf(mcLeakUnit_List, pQry(0).fldLeakUnitCust)
            End If

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveTo_DB(ByVal ProcessProjectID_In As Integer)
        '==================================================
        Try
            Dim pSealProcessEntities As New SealProcessDBEntities()
            Dim pQry = (From pRec In pSealProcessEntities.tblProcess_Unit
                        Where pRec.fldProcessProjectID = ProcessProjectID_In Select pRec).ToList()

            Dim pProcessUnit As New tblProcess_Unit

            If (pQry.Count > 0) Then
                pProcessUnit = pQry(0)
            End If

            With pProcessUnit
                .fldProcessProjectID = ProcessProjectID_In
                .fldLUnitPH = mcLUnit_List(mPH.LIndx)
                .fldFUnitPH = mcFUnit_List(mPH.FIndx)
                .fldPUnitPH = mcPUnit_List(mPH.PIndx)
                .fldTUnitPH = mcTUnit_List(mPH.TIndx)
                .fldLeakUnitPH = mcLeakUnit_List(mPH.LeakIndx)

                .fldLUnitCust = mcLUnit_List(mCust.LIndx)
                .fldFUnitCust = mcFUnit_List(mCust.FIndx)
                .fldPUnitCust = mcPUnit_List(mCust.PIndx)
                .fldTUnitCust = mcTUnit_List(mCust.TIndx)
                .fldLeakUnitCust = mcLeakUnit_List(mCust.LeakIndx)
            End With

            If (pQry.Count > 0) Then
                pSealProcessEntities.SaveChanges()
            Else
                pSealProcessEntities.AddTotblProcess_Unit(pProcessUnit)
                pSealProcessEntities.SaveChanges()

            End If
        Catch ex As Exception

        End Try

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
        Dim pBinSerializer As New BinaryFormatter(Nothing,
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
