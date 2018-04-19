'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsTest                        '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29DEC17                                '
'                                                                              '
'===============================================================================
Imports System.Globalization
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Linq
Imports System.Reflection
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms

<Serializable()> _
Public Class clsProcessProj_Test

#Region "MEMBER VARIABLES:"

    Private mOther As String

    Private mLeak As New clsLeak
    Private mLoad As New clsLoad
    Private mSpringBack As New clsSpringBack

    Private mIsNeeded As Boolean = False
    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....Other
    Public Property Other() As String
        '===================================
        Get
            Return mOther
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mOther = strData
        End Set

    End Property

#Region "Leak"

    Public Property Leak() As clsLeak
        '=============================
        Get
            Return mLeak
        End Get

        Set(ByVal strObj As clsLeak)
            '-------------------------------
            mLeak = strObj
        End Set

    End Property

#End Region

#Region "Load"

    Public Property Load() As clsLoad
        '=============================
        Get
            Return mLoad
        End Get

        Set(ByVal strObj As clsLoad)
            '-------------------------------
            mLoad = strObj
        End Set

    End Property

#End Region

#Region "SpringBack"

    Public Property SpringBack() As clsSpringBack
        '=============================
        Get
            Return mSpringBack
        End Get

        Set(ByVal strObj As clsSpringBack)
            '-------------------------------
            mSpringBack = strObj
        End Set

    End Property

#End Region

#Region "Risk"

    Public Property Risk() As clsProcessProj_Risk
        '=======================================
        Get
            Return mRisk
        End Get

        Set(ByVal strObj As clsProcessProj_Risk)
            '------------------------------------------------
            mRisk = strObj
        End Set

    End Property

#End Region

#Region "EditedBy"

    Public Property EditedBy() As clsProcessProj_EditedBy
        '================================================
        Get
            Return mEditedBy
        End Get

        Set(ByVal strObj As clsProcessProj_EditedBy)
            '-------------------------------
            mEditedBy = strObj
        End Set

    End Property

#End Region

#End Region

#Region "DB RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '=====================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try

            '....tblTest
            Dim pQryTestCount As Integer = (From pRec In pSealProcessDBEntities.tblTest
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryTestCount > 0) Then
                'mIsNeeded = True       'AES 17APR18

                Dim pQryTest = (From pRec In pSealProcessDBEntities.tblTest
                                Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mOther = pQryTest.fldOther
                'mEditedBy.User_Name = pQryTest.fldUserName
                'mEditedBy.User_Signed = pQryTest.fldSigned

                'If (Not IsNothing(pQryTest.fldDateSigned) And Not IsDBNull(pQryTest.fldDateSigned)) Then
                '    mEditedBy.User_DateSigned = pQryTest.fldDateSigned
                'End If

            Else
                mOther = ""

            End If

            mLeak.RetrieveFromDB(ProjectID_In)
            mLoad.RetrieveFromDB(ProjectID_In)
            mSpringBack.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer, ByVal Needed_In As Boolean)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....Test table
        Dim pTestCount As Integer = (From Test In pSealProcessDBEntities.tblTest
                                     Where Test.fldProcessProjectID = ProjectID_In Select Test).Count()

        If (Needed_In) Then

            If (pTestCount > 0) Then
                '....Record already exists
                Dim pTest = (From Test In pSealProcessDBEntities.tblTest
                             Where Test.fldProcessProjectID = ProjectID_In Select Test).First()

                pTest.fldOther = mOther
                'pTest.fldUserName = mEditedBy.User.Name
                'pTest.fldSigned = mEditedBy.User.Signed
                'pTest.fldDateSigned = mEditedBy.User.DateSigned

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pTest As New tblTest
                pTest.fldProcessProjectID = pID

                pTest.fldOther = mOther
                'pTest.fldUserName = mEditedBy.User.Name
                'pTest.fldSigned = mEditedBy.User.Signed
                'pTest.fldDateSigned = mEditedBy.User.DateSigned

                pSealProcessDBEntities.AddTotblTest(pTest)
                pSealProcessDBEntities.SaveChanges()
            End If


            mLeak.SaveToDB(ProjectID_In)
            mLoad.SaveToDB(ProjectID_In)
            mSpringBack.SaveToDB(ProjectID_In)
        Else

            '....Test_Leak
            Dim pTestRec_Leak = (From Test In pSealProcessDBEntities.tblTest_Leak
                                 Where Test.fldProcessProjectID = ProjectID_In Select Test).ToList()

            If (pTestRec_Leak.Count > 0) Then
                For j As Integer = 0 To pTestRec_Leak.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pTestRec_Leak(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            '....Test_Load
            Dim pTestRec_Load = (From Test In pSealProcessDBEntities.tblTest_Load
                                 Where Test.fldProcessProjectID = ProjectID_In Select Test).ToList()

            If (pTestRec_Load.Count > 0) Then
                For j As Integer = 0 To pTestRec_Load.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pTestRec_Load(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            '....Test_SpringBack
            Dim pTestRec_SpringBack = (From Test In pSealProcessDBEntities.tblTest_SpringBack
                                       Where Test.fldProcessProjectID = ProjectID_In Select Test).ToList()

            If (pTestRec_SpringBack.Count > 0) Then
                For j As Integer = 0 To pTestRec_SpringBack.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pTestRec_SpringBack(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

        End If

    End Sub

#End Region

    Public Function IsNeeded() As Boolean
        '================================

        If (mIsNeeded Or mLeak.IsNeeded Or mLoad.IsNeeded Or mSpringBack.IsNeeded) Then
            Return True
        Else
            Return False
        End If

    End Function

#Region "NESTED CLASS:"

#Region "Leak:"

    <Serializable()> _
    Public Class clsLeak

#Region "MEMBER VARIABLES:"

        Private mCompress_Unplated As Double
        Private mMedium_Unplated As String
        Private mPress_Unplated As Double
        Private mMax_Unplated As Double
        Private mQty_Unplated As Int64
        Private mFreq_Unplated As String

        Private mCompress_Plated As Double
        Private mMedium_Plated As String
        Private mPress_Plated As Double
        Private mMax_Plated As Double
        Private mQty_Plated As Int64
        Private mFreq_Plated As String
        Private mIsNeeded As Boolean = False

#End Region

#Region "PROPERTY ROUTINES:"

        '....Compress_Unplated
        Public Property Compress_Unplated() As Double
            '=============================
            Get
                Return mCompress_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mCompress_Unplated = strData
            End Set

        End Property

        '....Medium_Unplated
        Public Property Medium_Unplated() As String
            '====================================
            Get
                Return mMedium_Unplated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mMedium_Unplated = strData
            End Set

        End Property


        '....Press_Unplated
        Public Property Press_Unplated() As Double
            '=============================
            Get
                Return mPress_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mPress_Unplated = strData
            End Set

        End Property


        '....Max_Unplated
        Public Property Max_Unplated() As Double
            '=============================
            Get
                Return mMax_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mMax_Unplated = strData
            End Set

        End Property


        '....Qty_Unplated
        Public Property Qty_Unplated() As Int64
            '=============================
            Get
                Return mQty_Unplated
            End Get

            Set(ByVal intData As Int64)
                '-------------------------------
                mQty_Unplated = intData
            End Set

        End Property

        '....Freq_Unplated
        Public Property Freq_Unplated() As String
            '====================================
            Get
                Return mFreq_Unplated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mFreq_Unplated = strData
            End Set

        End Property


        '....Compress_Plated
        Public Property Compress_Plated() As Double
            '===================================
            Get
                Return mCompress_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mCompress_Plated = strData
            End Set

        End Property


        '....Medium_Plated
        Public Property Medium_Plated() As String
            '====================================
            Get
                Return mMedium_Plated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mMedium_Plated = strData
            End Set

        End Property


        '....Press_Plated
        Public Property Press_Plated() As Double
            '===================================
            Get
                Return mPress_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mPress_Plated = strData
            End Set

        End Property

        '....Max_Plated
        Public Property Max_Plated() As Double
            '===================================
            Get
                Return mMax_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mMax_Plated = strData
            End Set

        End Property


        '....Qty_Plated
        Public Property Qty_Plated() As Int64
            '=============================
            Get
                Return mQty_Plated
            End Get

            Set(ByVal intData As Int64)
                '-------------------------------
                mQty_Plated = intData
            End Set

        End Property


        '....Freq_Plated
        Public Property Freq_Plated() As String
            '====================================
            Get
                Return mFreq_Plated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mFreq_Plated = strData
            End Set

        End Property

        '....IsNeeded
        Public ReadOnly Property IsNeeded() As Boolean
            '===================================
            Get
                Return mIsNeeded
            End Get

        End Property

#End Region

#Region "DB RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '=====================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try

                '....tblTest_Leak
                Dim pQryTestLeakCount As Integer = (From pRec In pSealProcessDBEntities.tblTest_Leak
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryTestLeakCount > 0) Then
                    mIsNeeded = True

                    Dim pQryTestLeak = (From pRec In pSealProcessDBEntities.tblTest_Leak
                                        Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mCompress_Unplated = pQryTestLeak.fldCompress_Unplated
                    mMedium_Unplated = pQryTestLeak.fldMedium_Unplated
                    mPress_Unplated = pQryTestLeak.fldPress_Unplated
                    mMax_Unplated = pQryTestLeak.fldMax_Unplated
                    mQty_Unplated = pQryTestLeak.fldQty_Unplated
                    mFreq_Unplated = pQryTestLeak.fldFreq_Unplated
                    mCompress_Plated = pQryTestLeak.fldCompress_Plated
                    mMedium_Plated = pQryTestLeak.fldMedium_Plated
                    mPress_Plated = pQryTestLeak.fldPress_Plated
                    mMax_Plated = pQryTestLeak.fldMax_Plated
                    mQty_Plated = pQryTestLeak.fldQty_Plated
                    mFreq_Plated = pQryTestLeak.fldFreq_Plated

                Else
                    mCompress_Unplated = 0.0
                    mMedium_Unplated = ""
                    mPress_Unplated = 0.0
                    mMax_Unplated = 0.0
                    mQty_Unplated = 0.0
                    mFreq_Unplated = ""
                    mCompress_Plated = 0.0
                    mMedium_Plated = ""
                    mPress_Plated = 0.0
                    mMax_Plated = 0.0
                    mQty_Plated = 0.0
                    mFreq_Plated = ""

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            '....TestLeak table
            Dim pTestLeakCount As Integer = (From TestLeak In pSealProcessDBEntities.tblTest_Leak
                                                Where TestLeak.fldProcessProjectID = ProjectID_In Select TestLeak).Count()

            If (pTestLeakCount > 0) Then
                '....Record already exists
                Dim pTestLeak = (From TestLeak In pSealProcessDBEntities.tblTest_Leak
                                               Where TestLeak.fldProcessProjectID = ProjectID_In Select TestLeak).First()

                pTestLeak.fldCompress_Unplated = mCompress_Unplated
                pTestLeak.fldMedium_Unplated = mMedium_Unplated
                pTestLeak.fldPress_Unplated = mPress_Unplated
                pTestLeak.fldMax_Unplated = mMax_Unplated
                pTestLeak.fldQty_Unplated = mQty_Unplated
                pTestLeak.fldFreq_Unplated = mFreq_Unplated

                pTestLeak.fldCompress_Plated = mCompress_Plated
                pTestLeak.fldMedium_Plated = mMedium_Plated
                pTestLeak.fldPress_Plated = mPress_Plated
                pTestLeak.fldMax_Plated = mMax_Plated
                pTestLeak.fldQty_Plated = mQty_Plated
                pTestLeak.fldFreq_Plated = mFreq_Plated

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pTestLeak As New tblTest_Leak
                pTestLeak.fldProcessProjectID = pID

                pTestLeak.fldCompress_Unplated = mCompress_Unplated
                pTestLeak.fldMedium_Unplated = mMedium_Unplated
                pTestLeak.fldPress_Unplated = mPress_Unplated
                pTestLeak.fldMax_Unplated = mMax_Unplated
                pTestLeak.fldQty_Unplated = mQty_Unplated
                pTestLeak.fldFreq_Unplated = mFreq_Unplated

                pTestLeak.fldCompress_Plated = mCompress_Plated
                pTestLeak.fldMedium_Plated = mMedium_Plated
                pTestLeak.fldPress_Plated = mPress_Plated
                pTestLeak.fldMax_Plated = mMax_Plated
                pTestLeak.fldQty_Plated = mQty_Plated
                pTestLeak.fldFreq_Plated = mFreq_Plated

                pSealProcessDBEntities.AddTotblTest_Leak(pTestLeak)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class

#End Region

#Region "Load:"

    <Serializable()> _
    Public Class clsLoad

#Region "MEMBER VARIABLES:"

        Private mCompress_Unplated As Double
        Private mMax_Unplated As Double
        Private mQty_Unplated As Int64
        Private mFreq_Unplated As String

        Private mCompress_Plated As Double
        Private mMax_Plated As Double
        Private mQty_Plated As Int64
        Private mFreq_Plated As String
        Private mIsNeeded As Boolean

#End Region

#Region "PROPERTY ROUTINES:"

        '....Compress_Unplated
        Public Property Compress_Unplated() As Double
            '=============================
            Get
                Return mCompress_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mCompress_Unplated = strData
            End Set

        End Property


        '....Max_Unplated
        Public Property Max_Unplated() As Double
            '=============================
            Get
                Return mMax_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mMax_Unplated = strData
            End Set

        End Property


        '....Qty_Unplated
        Public Property Qty_Unplated() As Int64
            '=============================
            Get
                Return mQty_Unplated
            End Get

            Set(ByVal intData As Int64)
                '-------------------------------
                mQty_Unplated = intData
            End Set

        End Property

        '....Freq_Unplated
        Public Property Freq_Unplated() As String
            '====================================
            Get
                Return mFreq_Unplated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mFreq_Unplated = strData
            End Set

        End Property


        '....Compress_Plated
        Public Property Compress_Plated() As Double
            '===================================
            Get
                Return mCompress_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mCompress_Plated = strData
            End Set

        End Property


        '....Max_Plated
        Public Property Max_Plated() As Double
            '===================================
            Get
                Return mMax_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mMax_Plated = strData
            End Set

        End Property


        '....Qty_Plated
        Public Property Qty_Plated() As Int64
            '=============================
            Get
                Return mQty_Plated
            End Get

            Set(ByVal intData As Int64)
                '-------------------------------
                mQty_Plated = intData
            End Set

        End Property


        '....Freq_Plated
        Public Property Freq_Plated() As String
            '====================================
            Get
                Return mFreq_Plated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mFreq_Plated = strData
            End Set

        End Property

        '....IsNeeded
        Public ReadOnly Property IsNeeded() As Boolean
            '===================================
            Get
                Return mIsNeeded
            End Get

        End Property

#End Region

#Region "DB RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '=====================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try

                '....tblTest_Load
                Dim pQryTestLoadCount As Integer = (From pRec In pSealProcessDBEntities.tblTest_Load
                                                    Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryTestLoadCount > 0) Then
                    mIsNeeded = True
                    Dim pQryTestLoad = (From pRec In pSealProcessDBEntities.tblTest_Load
                                        Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mCompress_Unplated = pQryTestLoad.fldCompress_Unplated
                    mMax_Unplated = pQryTestLoad.fldMax_Unplated
                    mQty_Unplated = pQryTestLoad.fldQty_Unplated
                    mFreq_Unplated = pQryTestLoad.fldFreq_Unplated
                    mCompress_Plated = pQryTestLoad.fldCompress_Plated
                    mMax_Plated = pQryTestLoad.fldMax_Plated
                    mQty_Plated = pQryTestLoad.fldQty_Plated
                    mFreq_Plated = pQryTestLoad.fldFreq_Plated

                Else
                    mCompress_Unplated = 0.0
                    mMax_Unplated = 0.0
                    mQty_Unplated = 0.0
                    mFreq_Unplated = ""
                    mCompress_Plated = 0.0
                    mMax_Plated = 0.0
                    mQty_Plated = 0.0
                    mFreq_Plated = ""

                End If

            Catch ex As Exception

            End Try

        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            '....TestLoad table
            Dim pTestLoadCount As Integer = (From TestLoad In pSealProcessDBEntities.tblTest_Load
                                                Where TestLoad.fldProcessProjectID = ProjectID_In Select TestLoad).Count()

            If (pTestLoadCount > 0) Then
                '....Record already exists
                Dim pTestLoad = (From TestLeak In pSealProcessDBEntities.tblTest_Load
                                               Where TestLeak.fldProcessProjectID = ProjectID_In Select TestLeak).First()

                pTestLoad.fldCompress_Unplated = mCompress_Unplated
                pTestLoad.fldMax_Unplated = mMax_Unplated
                pTestLoad.fldQty_Unplated = mQty_Unplated
                pTestLoad.fldFreq_Unplated = mFreq_Unplated

                pTestLoad.fldCompress_Plated = mCompress_Plated
                pTestLoad.fldMax_Plated = mMax_Plated
                pTestLoad.fldQty_Plated = mQty_Plated
                pTestLoad.fldFreq_Plated = mFreq_Plated

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pTestLoad As New tblTest_Load
                pTestLoad.fldProcessProjectID = pID

                pTestLoad.fldCompress_Unplated = mCompress_Unplated
                pTestLoad.fldMax_Unplated = mMax_Unplated
                pTestLoad.fldQty_Unplated = mQty_Unplated
                pTestLoad.fldFreq_Unplated = mFreq_Unplated

                pTestLoad.fldCompress_Plated = mCompress_Plated
                pTestLoad.fldMax_Plated = mMax_Plated
                pTestLoad.fldQty_Plated = mQty_Plated
                pTestLoad.fldFreq_Plated = mFreq_Plated

                pSealProcessDBEntities.AddTotblTest_Load(pTestLoad)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class

#End Region

#Region "SpringBack:"

    <Serializable()> _
    Public Class clsSpringBack

#Region "MEMBER VARIABLES:"

        Private mCompress_Unplated As Double
        Private mMax_Unplated As Double
        Private mQty_Unplated As Int64
        Private mFreq_Unplated As String

        Private mCompress_Plated As Double
        Private mMax_Plated As Double
        Private mQty_Plated As Int64
        Private mFreq_Plated As String

        Private mIsNeeded As Boolean = False

#End Region

#Region "PROPERTY ROUTINES:"

        '....Compress_Unplated
        Public Property Compress_Unplated() As Double
            '=============================
            Get
                Return mCompress_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mCompress_Unplated = strData
            End Set

        End Property


        '....Max_Unplated
        Public Property Max_Unplated() As Double
            '=============================
            Get
                Return mMax_Unplated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mMax_Unplated = strData
            End Set

        End Property


        '....Qty_Unplated
        Public Property Qty_Unplated() As Int64
            '=============================
            Get
                Return mQty_Unplated
            End Get

            Set(ByVal intData As Int64)
                '-------------------------------
                mQty_Unplated = intData
            End Set

        End Property

        '....Freq_Unplated
        Public Property Freq_Unplated() As String
            '====================================
            Get
                Return mFreq_Unplated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mFreq_Unplated = strData
            End Set

        End Property


        '....Compress_Plated
        Public Property Compress_Plated() As Double
            '===================================
            Get
                Return mCompress_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mCompress_Plated = strData
            End Set

        End Property


        '....Max_Plated
        Public Property Max_Plated() As Double
            '===================================
            Get
                Return mMax_Plated
            End Get

            Set(ByVal strData As Double)
                '-------------------------------
                mMax_Plated = strData
            End Set

        End Property


        '....Qty_Plated
        Public Property Qty_Plated() As Int64
            '=============================
            Get
                Return mQty_Plated
            End Get

            Set(ByVal intData As Int64)
                '-------------------------------
                mQty_Plated = intData
            End Set

        End Property


        '....Freq_Plated
        Public Property Freq_Plated() As String
            '====================================
            Get
                Return mFreq_Plated
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mFreq_Plated = strData
            End Set

        End Property

        '....IsNeeded
        Public ReadOnly Property IsNeeded() As Boolean
            '===================================
            Get
                Return mIsNeeded
            End Get

        End Property

#End Region

#Region "DB RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '=====================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try

                '....tblTest_SpringBack
                Dim pQryTestSpringBackCount As Integer = (From pRec In pSealProcessDBEntities.tblTest_SpringBack
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryTestSpringBackCount > 0) Then

                    mIsNeeded = True

                    Dim pQryTestSpringBack = (From pRec In pSealProcessDBEntities.tblTest_SpringBack
                                              Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mCompress_Unplated = pQryTestSpringBack.fldCompress_Unplated
                    mMax_Unplated = pQryTestSpringBack.fldMax_Unplated
                    mQty_Unplated = pQryTestSpringBack.fldQty_Unplated
                    mFreq_Unplated = pQryTestSpringBack.fldFreq_Unplated
                    mCompress_Plated = pQryTestSpringBack.fldCompress_Plated
                    mMax_Plated = pQryTestSpringBack.fldMax_Plated
                    mQty_Plated = pQryTestSpringBack.fldQty_Plated
                    mFreq_Plated = pQryTestSpringBack.fldFreq_Plated
                Else
                    mCompress_Unplated = 0.0
                    mMax_Unplated = 0.0
                    mQty_Unplated = 0.0
                    mFreq_Unplated = ""
                    mCompress_Plated = 0.0
                    mMax_Plated = 0.0
                    mQty_Plated = 0.0
                    mFreq_Plated = ""

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            '....Test_SpringBack table
            Dim pTestSpringBackCount As Integer = (From TestSpringBack In pSealProcessDBEntities.tblTest_SpringBack
                                                Where TestSpringBack.fldProcessProjectID = ProjectID_In Select TestSpringBack).Count()

            If (pTestSpringBackCount > 0) Then
                '....Record already exists
                Dim pTestSpringBack = (From TestLeak In pSealProcessDBEntities.tblTest_SpringBack
                                               Where TestLeak.fldProcessProjectID = ProjectID_In Select TestLeak).First()

                pTestSpringBack.fldCompress_Unplated = mCompress_Unplated
                pTestSpringBack.fldMax_Unplated = mMax_Unplated
                pTestSpringBack.fldQty_Unplated = mQty_Unplated
                pTestSpringBack.fldFreq_Unplated = mFreq_Unplated

                pTestSpringBack.fldCompress_Plated = mCompress_Plated
                pTestSpringBack.fldMax_Plated = mMax_Plated
                pTestSpringBack.fldQty_Plated = mQty_Plated
                pTestSpringBack.fldFreq_Plated = mFreq_Plated

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pTestSpringBack As New tblTest_SpringBack
                pTestSpringBack.fldProcessProjectID = pID

                pTestSpringBack.fldCompress_Unplated = mCompress_Unplated
                pTestSpringBack.fldMax_Unplated = mMax_Unplated
                pTestSpringBack.fldQty_Unplated = mQty_Unplated
                pTestSpringBack.fldFreq_Unplated = mFreq_Unplated

                pTestSpringBack.fldCompress_Plated = mCompress_Plated
                pTestSpringBack.fldMax_Plated = mMax_Plated
                pTestSpringBack.fldQty_Plated = mQty_Plated
                pTestSpringBack.fldFreq_Plated = mFreq_Plated

                pSealProcessDBEntities.AddTotblTest_SpringBack(pTestSpringBack)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class

#End Region

#End Region

End Class
