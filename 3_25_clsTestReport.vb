'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_Report                         '
'                        VERSION NO  :  2.4                                 '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  14JUN17                                '
'                                                                              '
'===============================================================================
Imports System.Linq
Imports Microsoft.Office.Interop.PowerPoint
Imports Microsoft.Office.Core
Imports WORD = Microsoft.Office.Interop.Word
Imports Microsoft.Office.Interop.Word
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Data.Sql
Imports clsLibrary11


<Serializable()> _
Public Class Test_clsReport

    Enum eReportType
        Normal
        Rejection
    End Enum

#Region "MEMBER VARIABLE DECLARATIONS:"

    Private Const mcImgAspectRatio As Double = 1.8
    Private mID As Integer
    Private mNo As String

    Private mDateOpen As DateTime
    Private mSealQty As Integer
    Private mIsPress As Boolean

    Private mLeak As sLeak
    Private mLeakPress As Double

    Private mLoad As sLoad

    Private mTester As sUser
    Private mEng As sUser
    Private mQuality As sUser

    Private mGen As sGen
    Private mLeakEquip As sLeak_Equip
    Private mLoadEquip As sLoad_Equip

    Private mNotes As String
    Private mOverridden As Boolean

    Private mTestSeal As New List(Of Test_clsSeal)

    Private mReportType As eReportType = eReportType.Normal

#End Region


#Region "REPORT TEMPLATE FILES LOCATION: "

    'Report document File:
    '---------------------
    Private Const mcDirTemplates As String = "C:\SealSuite\SealTest\Templates\"

    '....Test Report  
    Private Const mcTestReportFileName_Pass As String = mcDirTemplates & "TestReport_Rev04.dotx"
    Private Const mcTestReportFileName_Failed As String = mcDirTemplates & "TestReport_Rev04_FAILED.dotx"

    '....Rejection Report
    Private Const mcRejectionReportFileName As String = mcDirTemplates & "QA830000101 Inspection Reject Ticket Rev D.dotx"

#End Region

#Region "PROPERTY ROUTINES:"
    '======================

    '....ID
    Public Property ID() As Integer
        '==========================
        Get
            Return mID
        End Get

        Set(ByVal value As Integer)
            mID = value
        End Set
    End Property

    '....No
    Public Property No() As Integer
        '==========================
        Get
            Return mNo
        End Get

        Set(ByVal value As Integer)
            mNo = value
        End Set
    End Property


    '....DateOpen
    Public Property DateOpen() As DateTime
        '====================================
        Get
            Return mDateOpen
        End Get

        Set(ByVal value As DateTime)
            mDateOpen = value
        End Set
    End Property


    '....SealQty
    Public Property SealQty() As Integer
        '================================
        Get
            Return mSealQty
        End Get

        Set(ByVal value As Integer)
            mSealQty = value
            'SetSealData()
        End Set

    End Property

    '....Press
    Public Property IsPress() As Boolean
        '================================
        Get
            Return mIsPress
        End Get

        Set(ByVal value As Boolean)
            mIsPress = value
        End Set
    End Property

    '....LeakPress
    Public Property LeakPress() As Double
        '=================================
        Get
            Return mLeakPress
        End Get

        Set(ByVal value As Double)
            mLeakPress = value
        End Set
    End Property


#Region "LEAK"

    '....Leak
    Public ReadOnly Property Leak() As sLeak
        '===================================
        Get
            Return mLeak
        End Get
    End Property

    '....Leak_Exists
    Public WriteOnly Property Leak_Exists() As Boolean
        '================================================
        Set(ByVal Value As Boolean)
            mLeak.Exists = Value
            If (Not mLeak.Exists) Then
                mLeak.Springback = False
            End If
        End Set
    End Property

    '....Leak_Leakage
    Public WriteOnly Property Leak_Leakage() As Boolean
        '================================================
        Set(ByVal Value As Boolean)
            mLeak.Leakage = Value
        End Set
    End Property

    '....Leak_Plate
    Public WriteOnly Property Leak_Plate() As Boolean
        '================================================
        Set(ByVal Value As Boolean)
            mLeak.LeakagePlate = Value
        End Set
    End Property

    '....Leak_Springback
    Public WriteOnly Property Leak_Springback() As Boolean
        '================================================
        Set(ByVal Value As Boolean)
            mLeak.Springback = Value
        End Set
    End Property

#End Region

#Region "LOAD"

    '....Load
    Public ReadOnly Property Load() As sLoad
        '===================================
        Get
            Return mLoad
        End Get
    End Property

    '....Load_Exists
    Public WriteOnly Property Load_Exists() As Boolean
        '================================================
        Set(ByVal Value As Boolean)
            mLoad.Exists = Value
            If (Not mLoad.Exists) Then
                mLoad.Springback = False
            End If
        End Set
    End Property

    '....Load_Springback
    Public WriteOnly Property Load_Springback() As Boolean
        '================================================
        Set(ByVal Value As Boolean)
            mLoad.Springback = Value
        End Set
    End Property

#End Region


#Region "User Tester:"

    '....Tester
    Public ReadOnly Property Tester() As sUser
        '=====================================
        Get
            Return mTester
        End Get
    End Property

    '....Tester_Name
    Public WriteOnly Property Tester_Name() As String
        '============================================
        Set(ByVal Value As String)
            mTester.Name = Value
        End Set
    End Property

    '....Tester_Signed
    Public WriteOnly Property Tester_Signed() As Boolean
        '============================================
        Set(ByVal Value As Boolean)
            mTester.Signed = Value
        End Set
    End Property

    '....Tester_SignedDate
    Public WriteOnly Property Tester_SignedDate() As DateTime
        '============================================
        Set(ByVal Value As DateTime)
            mTester.DateSigned = Value
        End Set
    End Property

#End Region


#Region "User Eng:"

    '....Eng
    Public ReadOnly Property Eng() As sUser
        '=====================================
        Get
            Return mEng
        End Get
    End Property

    '....Eng_Name
    Public WriteOnly Property Eng_Name() As String
        '============================================
        Set(ByVal Value As String)
            mEng.Name = Value
        End Set
    End Property

    '....Eng_Signed
    Public WriteOnly Property Eng_Signed() As Boolean
        '============================================
        Set(ByVal Value As Boolean)
            mEng.Signed = Value
        End Set
    End Property

    '....Eng_SignedDate
    Public WriteOnly Property Eng_SignedDate() As DateTime
        '============================================
        Set(ByVal Value As DateTime)
            mEng.DateSigned = Value
        End Set
    End Property

#End Region

#Region "User Quality:"

    '....Quality
    Public ReadOnly Property Quality() As sUser
        '=====================================
        Get
            Return mQuality
        End Get
    End Property

    '....Quality_Name
    Public WriteOnly Property Quality_Name() As String
        '============================================
        Set(ByVal Value As String)
            mQuality.Name = Value
        End Set
    End Property

    '....Quality_Signed
    Public WriteOnly Property Quality_Signed() As Boolean
        '============================================
        Set(ByVal Value As Boolean)
            mQuality.Signed = Value
        End Set
    End Property

    '....Quality_SignedDate
    Public WriteOnly Property Quality_SignedDate() As DateTime
        '============================================
        Set(ByVal Value As DateTime)
            mQuality.DateSigned = Value
        End Set
    End Property

#End Region


#Region "Gen:"

    '....Read Only Property
    '....General
    Public ReadOnly Property Gen() As sGen
        '=================================
        Get
            Return mGen
        End Get
    End Property

    ''....GenImageID
    'Public WriteOnly Property GenImageID() As Integer
    '    '==============================================
    '    Set(ByVal Value As Integer)
    '        mGen.Image.ImageID = Value
    '    End Set

    'End Property


    ''....GenImageFile
    'Public WriteOnly Property GenImageFile() As String
    '    '==============================================
    '    Set(ByVal Value As String)
    '        mGen.ImageFile = Value
    '    End Set

    'End Property

    ''....GenImageNameTag
    'Public WriteOnly Property GenImageNameTag() As String
    '    '==============================================
    '    Set(ByVal Value As String)
    '        mGen.ImageNameTag = Value
    '    End Set

    'End Property

    ''....GenImageCaption
    'Public WriteOnly Property GenImageCaption() As String
    '    '==============================================
    '    Set(ByVal Value As String)
    '        mGen.ImageCaption = Value
    '    End Set

    'End Property

    ''....GenImage
    'Public WriteOnly Property GenImage() As Image
    '    '==============================================
    '    Set(ByVal Value As Image)
    '        mGen.Image = Value
    '    End Set

    'End Property

    '....GenImage
    Public WriteOnly Property GenImage() As List(Of sGenImage)
        '====================================================
        Set(ByVal Data As List(Of sGenImage))
            mGen.Image = Data
        End Set

    End Property

    '....GenSeal
    Public WriteOnly Property GenSeal() As List(Of sGenSeal)
        '====================================================
        Set(ByVal Data As List(Of sGenSeal))
            mGen.Seal = Data
        End Set

    End Property

#End Region


#Region "LEAK EQUIP:"

    '....LeakEquip
    Public ReadOnly Property LeakEquip() As sLeak_Equip
        '==============================================
        Get
            Return mLeakEquip
        End Get
    End Property

    '....Leak_StandName
    Public WriteOnly Property Leak_StandName() As String
        '================================================
        Set(ByVal Value As String)
            mLeakEquip.StandName = Value
        End Set

    End Property

    '....Leak_Fixture
    Public WriteOnly Property Leak_Fixture() As String
        '=============================================
        Set(ByVal Value As String)
            mLeakEquip.Fixture = Value
        End Set

    End Property


    '....Leak_T
    Public WriteOnly Property Leak_T() As Double
        '================================================
        Set(ByVal Value As Double)
            mLeakEquip.T = Value
        End Set

    End Property


    '....Leak_ShimActual
    Public WriteOnly Property Leak_ShimActual() As Double
        '================================================
        Set(ByVal Value As Double)
            mLeakEquip.ShimActual = Value
        End Set

    End Property

    '....Leak_ShimDescrip
    Public WriteOnly Property Leak_ShimDescrip() As String
        '=================================================
        Set(ByVal Value As String)
            mLeakEquip.ShimDescrip = Value
        End Set

    End Property

    '....Platen Surface Finish
    Public WriteOnly Property Leak_PlatenSF() As Integer
        '===============================================
        Set(ByVal Value As Integer)
            mLeakEquip.Platen_SF = Value
        End Set

    End Property

    '....Leak_TestMeterMake
    Public WriteOnly Property Leak_TestMeterMake() As String
        '===================================================
        Set(ByVal Value As String)
            mLeakEquip.TestMeterMake = Value
        End Set

    End Property


    '....Leak_TestMeterSN
    Public WriteOnly Property Leak_TestMeterSN() As String
        '=================================================
        Set(ByVal Value As String)
            mLeakEquip.TestMeterSN = Value
        End Set

    End Property

    '....Leak_TestMeterRange
    Public WriteOnly Property Leak_TestMeterRange() As String
        '====================================================
        Set(ByVal Value As String)
            mLeakEquip.TestMeterRange = Value
        End Set

    End Property

    '....Leak_TestMeterModelNo
    Public WriteOnly Property Leak_TestMeterModelNo() As String
        '======================================================
        Set(ByVal Value As String)
            mLeakEquip.TestMeterModelNo = Value
        End Set

    End Property

    '....Leak_TestMeterDateCalibrationDue
    Public WriteOnly Property Leak_TestMeterDateCalibrationDue() As DateTime
        '===================================================================
        Set(ByVal Value As DateTime)
            mLeakEquip.TestMeterDateCalibrationDue = Value
        End Set

    End Property

    ''....Leak_IsSpringBack
    'Public WriteOnly Property Leak_IsSpringBack() As Boolean
    '    '===================================================
    '    Set(ByVal Value As Boolean)
    '        mLeak.IsSpringBack = Value
    '    End Set

    'End Property

#End Region


#Region "LOAD EQUIP:"

    '....Load Equip
    Public ReadOnly Property LoadEquip() As sLoad_Equip
        '==============================================
        Get
            Return mLoadEquip
        End Get
    End Property

    '....Load_StandName
    Public WriteOnly Property Load_StandName() As String
        '====================================================
        Set(ByVal Value As String)
            mLoadEquip.StandName = Value
        End Set

    End Property

    '....Load_SN
    Public WriteOnly Property Load_SN() As String
        '=============================================
        Set(ByVal Value As String)
            mLoadEquip.StandSN = Value
        End Set

    End Property

    '....Load_StandDateCalibrationDue
    Public WriteOnly Property Load_StandDateCalibrationDue() As DateTime
        '===================================================================
        Set(ByVal Value As DateTime)
            mLoadEquip.StandDateCalibrationDue = Value
        End Set

    End Property

    '....Load_LoadCellMake
    Public WriteOnly Property Load_LoadCellMake() As String
        '======================================================
        Set(ByVal Value As String)
            mLoadEquip.LoadCellMake = Value
        End Set

    End Property

    '....Load_LoadCellSN
    Public WriteOnly Property Load_LoadCellSN() As String
        '=============================================
        Set(ByVal Value As String)
            mLoadEquip.LoadCellSN = Value
        End Set

    End Property

    '....Load_LoadCellRange
    Public WriteOnly Property Load_LoadCellRange() As String
        '=======================================================
        Set(ByVal Value As String)
            mLoadEquip.LoadCellRange = Value
        End Set

    End Property

    '....Load_LoadCellModelNo
    Public WriteOnly Property Load_LoadCellModelNo() As String
        '=======================================================
        Set(ByVal Value As String)
            mLoadEquip.LoadCellModelNo = Value
        End Set

    End Property

    '....Load_LoadCellDateCalibrationDue
    Public WriteOnly Property Load_LoadCellDateCalibrationDue() As DateTime
        '===================================================================
        Set(ByVal Value As DateTime)
            mLoadEquip.LoadCellDateCalibrationDue = Value
        End Set

    End Property

#End Region


    Public Property TestSeal() As List(Of Test_clsSeal)
        '=========================================
        Get
            Return mTestSeal
        End Get
        Set(Obj As List(Of Test_clsSeal))
            mTestSeal = Obj
        End Set
    End Property


    '....Notes
    Public Property Notes() As String
        '=================================
        Get
            Return mNotes
        End Get

        Set(ByVal value As String)
            mNotes = value
        End Set
    End Property

    '....Overridden
    Public Property Overridden() As Boolean
        '=================================
        Get
            Return mOverridden
        End Get

        Set(ByVal value As Boolean)
            mOverridden = value
        End Set
    End Property


    '....ReportType
    Public Property ReportType() As eReportType
        '=======================================
        Get
            Return mReportType
        End Get

        Set(ByVal value As eReportType)
            mReportType = value
        End Set
    End Property

#End Region



#Region "DB RELATED ROUTINES:"

    Public Sub RetrieveFrom_DB(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '======================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....GenImage
        Dim pQryGenImage = (From pRec In pSealTestEntities.tblReportGenImage
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And
                     pRec.fldTestRptID = mID Order By pRec.fldImageID Ascending Select pRec).ToList()

        If (pQryGenImage.Count() > 0) Then
            mGen.Image = New List(Of sGenImage)
            For i As Integer = 0 To pQryGenImage.Count() - 1
                Dim pGenImage As New sGenImage

                pGenImage.ID = pQryGenImage(i).fldImageID
                pGenImage.File = pQryGenImage(i).fldImageFile
                pGenImage.NameTag = pQryGenImage(i).fldImageNameTag
                pGenImage.Caption = pQryGenImage(i).fldImageCaption
                'pGenImage.Image = pQryGenImage(i).fldImage

                If (Not IsDBNull(pQryGenImage(i).fldImage)) Then
                    Dim pArray As Byte() = DirectCast(pQryGenImage(i).fldImage, Byte())
                    If (Not IsNothing(pArray)) Then
                        Dim pMS As New MemoryStream(pArray)
                        pGenImage.Image = Image.FromStream(pMS)
                    End If


                End If

                pGenImage.Selected = pQryGenImage(i).fldSelected
                mGen.Image.Add(pGenImage)
            Next
        End If

        Dim pQryGenSeal = (From pRec In pSealTestEntities.tblReportGenSeal
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And
                     pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

        If (pQryGenSeal.Count() > 0) Then
            mGen.Seal.Clear()
            For i As Integer = 0 To pQryGenSeal.Count() - 1
                Dim pGenSeal As New sGenSeal

                pGenSeal.SeqID = pQryGenSeal(i).fldSealSeqID
                pGenSeal.SN = pQryGenSeal(i).fldSealSN
                mGen.Seal.Add(pGenSeal)
            Next
        End If

        '....Add Test Seal HW
        'AES 21FEB17
        If (mSealQty > mTestSeal.Count) Then
            mTestSeal.Clear()
            For i As Integer = 0 To mSealQty - 1
                Dim pTestSeal As New Test_clsSeal()
                mTestSeal.Add(pTestSeal)
            Next
        End If
        'For i As Integer = 0 To mSealQty - 1
        '    Dim pTestSeal As New clsTest_Seal()
        '    mTestSeal.Add(pTestSeal)
        'Next

        '....Leak
        Dim pQryLeak = (From pRec In pSealTestEntities.tblLeak
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And
                     pRec.fldTestRptID = mID Select pRec).ToList()


        If (pQryLeak.Count() > 0) Then

            mLeakEquip.StandName = pQryLeak(0).fldStandName
            mLeakEquip.Fixture = pQryLeak(0).fldFixture
            mLeakEquip.ShimActual = pQryLeak(0).fldShimActual
            mLeakEquip.ShimDescrip = pQryLeak(0).fldShimDescrip
            mLeakEquip.Platen_SF = pQryLeak(0).fldSF_Platen          'AES 15MAR17
            mLeakEquip.TestMeterMake = pQryLeak(0).fldTestMeterMake
            mLeakEquip.TestMeterSN = pQryLeak(0).fldTestMeterSN

            Dim pQryFM = (From pRec In pSealTestEntities.tblFlowMeter
                   Where pRec.fldSN.Trim() = mLeakEquip.TestMeterSN.Trim() Select pRec Distinct).ToList()

            If (pQryFM.Count() > 0) Then
                mLeakEquip.TestMeterRange = pQryFM(0).fldRange
                mLeakEquip.TestMeterModelNo = pQryFM(0).fldModelNo

                If (Not IsDBNull(pQryFM(0).fldDateCalibrationDue) And Not IsNothing(pQryFM(0).fldDateCalibrationDue)) Then
                    mLeakEquip.TestMeterDateCalibrationDue = pQryFM(0).fldDateCalibrationDue
                End If

            End If

            'mLeak.Springback = pQryLeak(0).fldIsSpringBack

            Dim pQryLeakData = (From pRec In pSealTestEntities.tblLeakData
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And
                     pRec.fldTestRptID = mID Select pRec Distinct).ToList()

            If (pQryLeakData.Count() > 0) Then

                For i As Integer = 0 To pQryLeakData.Count() - 1

                    Dim pSeqID As Integer = pQryLeakData(i).fldSealSeqID

                    If (pSeqID > 0) Then
                        mTestSeal(pSeqID - 1).Leak_FHIni = pQryLeakData(i).fldFHIni
                        mTestSeal(pSeqID - 1).Leak_FHFinal = pQryLeakData(i).fldFHFinal
                        mTestSeal(pSeqID - 1).Leak_ODPre = pQryLeakData(i).fldODPre
                        mTestSeal(pSeqID - 1).Leak_ODPost = pQryLeakData(i).fldODPost
                        mTestSeal(pSeqID - 1).Leak_IDPre = pQryLeakData(i).fldIDPre
                        mTestSeal(pSeqID - 1).Leak_IDPost = pQryLeakData(i).fldIDPost
                        mTestSeal(pSeqID - 1).Leak_Val = pQryLeakData(i).fldVal

                    End If

                Next
            End If

        End If

        '....Load
        Dim pQryLoad = (From pRec In pSealTestEntities.tblLoad
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And
                     pRec.fldTestRptID = mID Select pRec Distinct).ToList()


        If (pQryLoad.Count() > 0) Then

            mLoadEquip.StandName = pQryLoad(0).fldStandName
            mLoadEquip.StandSN = pQryLoad(0).fldStandSN

            Dim pQryForceStand = (From pRec In pSealTestEntities.tblForceStand
                   Where pRec.fldSN = mLoadEquip.StandSN Select pRec Distinct).ToList()

            If (pQryForceStand.Count() > 0) Then
                If (Not IsDBNull(pQryForceStand(0).fldDateCalibrationDue) And Not IsNothing(pQryForceStand(0).fldDateCalibrationDue)) Then
                    mLoadEquip.StandDateCalibrationDue = pQryForceStand(0).fldDateCalibrationDue
                End If

            End If


            mLoadEquip.LoadCellMake = pQryLoad(0).fldLoadCellMake
            mLoadEquip.LoadCellSN = pQryLoad(0).fldLoadCellSN

            Dim pQryLoadCell = (From pRec In pSealTestEntities.tblLoadCell
                  Where pRec.fldSN.Trim() = mLoadEquip.LoadCellSN.Trim() Select pRec Distinct).ToList()

            If (pQryLoadCell.Count() > 0) Then
                mLoadEquip.LoadCellRange = pQryLoadCell(0).fldRange
                mLoadEquip.LoadCellModelNo = pQryLoadCell(0).fldModelNo
                If (Not IsDBNull(pQryLoadCell(0).fldDateCalibrationDue) And Not IsNothing(pQryLoadCell(0).fldDateCalibrationDue)) Then
                    mLoadEquip.LoadCellDateCalibrationDue = pQryLoadCell(0).fldDateCalibrationDue
                End If

            End If

            'mLoad.Springback = pQryLoad(0).fldIsSpringBack


            Dim pQryLoadData = (From pRec In pSealTestEntities.tblLoadData
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And
                     pRec.fldTestRptID = mID Select pRec Distinct).ToList()

            If (pQryLoadData.Count() > 0) Then

                For i As Integer = 0 To pQryLoadData.Count() - 1
                    Dim pSeqID As Integer = pQryLoadData(i).fldSealSeqID

                    If (pSeqID > 0) Then
                        mTestSeal(pSeqID - 1).Load_FHIni = pQryLoadData(i).fldFHIni
                        mTestSeal(pSeqID - 1).Load_FHFinal = pQryLoadData(i).fldFHFinal
                        mTestSeal(pSeqID - 1).Load_ODPre = pQryLoadData(i).fldODPre
                        mTestSeal(pSeqID - 1).Load_ODPost = pQryLoadData(i).fldODPost
                        mTestSeal(pSeqID - 1).Load_IDPre = pQryLoadData(i).fldIDPre
                        mTestSeal(pSeqID - 1).Load_IDPost = pQryLoadData(i).fldIDPost
                        mTestSeal(pSeqID - 1).Load_Val = pQryLoadData(i).fldVal
                    End If

                Next

            End If

        End If


    End Sub


    Public Sub SaveTo_tblTestReport(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '===========================================================================================
        Dim pRpt_ID As Integer = 1
        Dim pTestRpt As New tblReport
        Dim pIsRptExists As Boolean = False
        Dim pSealTestEntities As New SealTestDBEntities

        Try

            '....tblReport
            Dim pQry = (From pRec In pSealTestEntities.tblReport
                        Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                        pRec.fldID = mID Select pRec).ToList()

            If (pQry.Count() > 0) Then
                pTestRpt = pQry(0)
            End If

            With pTestRpt
                .fldTestProjectID = TestProjectID_In
                .fldTestMOID = TestMOID_In
                .fldID = mID
                .fldNo = mNo
                .fldDateOpen = mDateOpen
                .fldSealQty = mSealQty
                .fldIsLeak = mLeak.Exists
                .fldIsLeak_Leakage = mLeak.Leakage
                .fldIsLeak_LeakagePlated = mLeak.LeakagePlate
                .fldIsLeak_Springback = mLeak.Springback

                .fldIsLoad = mLoad.Exists
                .fldIsLoad_Springback = mLoad.Springback

                .fldIsPressure = mIsPress
                .fldLeakPress = mLeakPress
                .fldNotes = mNotes
                .fldOverridden = mOverridden

                .fldUserTester = mTester.Name
                .fldTesterSigned = mTester.Signed
                .fldDateTester = mTester.DateSigned

                .fldUserEngg = mEng.Name
                .fldEnggSigned = mEng.Signed
                .fldDateEngg = mEng.DateSigned

                .fldUserQuality = mQuality.Name
                .fldQualitySigned = mQuality.Signed
                .fldDateQuality = mQuality.DateSigned

                If (pQry.Count() > 0) Then
                    pSealTestEntities.SaveChanges()
                Else
                    pSealTestEntities.AddTotblReport(pTestRpt)
                    pSealTestEntities.SaveChanges()

                End If
            End With

        Catch ex As Exception

        End Try

    End Sub


    Public Sub SaveTo_tblTestRptGenImage(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '=================================================================================================
        Try

            Dim pSealTestEntities As New SealTestDBEntities()

            '....tblTestRptGenImage
            Dim pTestRepeortGenImage As New tblReportGenImage

            Dim pQryRptGen = (From pRec In pSealTestEntities.tblReportGenImage
                           Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                           pRec.fldTestRptID = mID Select pRec).ToList()

            If (pQryRptGen.Count() > 0) Then
                For j As Integer = 0 To pQryRptGen.Count() - 1
                    pSealTestEntities.DeleteObject(pQryRptGen(j))
                    pSealTestEntities.SaveChanges()
                Next
            End If

            Dim pRepeortGenImage As New List(Of tblReportGenImage)

            For j As Integer = 0 To mGen.Image.Count - 1
                Dim pGenImage As New tblReportGenImage
                pRepeortGenImage.Add(pGenImage)
                With pRepeortGenImage(j)
                    .fldTestProjectID = TestProjectID_In
                    .fldTestMOID = TestMOID_In
                    .fldTestRptID = mID
                    .fldImageID = (j + 1)
                    .fldImageFile = mGen.Image(j).File
                    .fldImageNameTag = mGen.Image(j).NameTag
                    .fldImageCaption = mGen.Image(j).Caption
                    If (Not IsNothing(mGen.Image(j).Image)) Then
                        .fldImage = ImgToByteArray(mGen.Image(j).Image)
                    End If


                    .fldSelected = mGen.Image(j).Selected

                End With

                pSealTestEntities.AddTotblReportGenImage(pRepeortGenImage(j))
            Next

            pSealTestEntities.SaveChanges()

        Catch ex As Exception

        End Try

    End Sub


    Public Sub SaveTo_tblTestRptGenSeal(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '================================================================================================
        '....tblTestRptGenSeal
        Dim pTestRepeortGenSeal As New tblReportGenSeal
        Dim pSealTestEntities As New SealTestDBEntities

        Dim pQryRptGenSeal = (From pRec In pSealTestEntities.tblReportGenSeal
                       Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                       pRec.fldTestRptID = mID Select pRec).ToList()

        If (pQryRptGenSeal.Count() > 0) Then
            For j As Integer = 0 To pQryRptGenSeal.Count() - 1
                pSealTestEntities.DeleteObject(pQryRptGenSeal(j))
                pSealTestEntities.SaveChanges()
            Next
        End If

        Dim pRepeortGenSeal As New List(Of tblReportGenSeal)

        For j As Integer = 0 To mGen.Seal.Count - 1
            Dim pGenSeal As New tblReportGenSeal
            pRepeortGenSeal.Add(pGenSeal)
            With pRepeortGenSeal(j)
                .fldTestProjectID = TestProjectID_In
                .fldTestMOID = TestMOID_In
                .fldTestRptID = mID
                .fldSealSeqID = (j + 1)
                .fldSealSN = mGen.Seal(j).SN
            End With

            pSealTestEntities.AddTotblReportGenSeal(pRepeortGenSeal(j))
        Next

        pSealTestEntities.SaveChanges()

    End Sub


    '....Leakage: tblLeak
    Public Sub SaveTo_tblLeak(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '=====================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblLeak
        Dim pLeak As New tblLeak
        Dim pRecExists As Boolean = False
        Dim pQryLeak = (From pRec In pSealTestEntities.tblLeak
                       Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                       pRec.fldTestRptID = mID Select pRec).ToList()

        If (pQryLeak.Count() > 0) Then
            pLeak = pQryLeak(0)
            pRecExists = True
        End If

        With pLeak
            .fldTestProjectID = TestProjectID_In
            .fldTestMOID = TestMOID_In
            .fldTestRptID = mID
            .fldStandName = mLeakEquip.StandName
            .fldT = mLeakEquip.T
            .fldFixture = mLeakEquip.Fixture
            .fldShimActual = mLeakEquip.ShimActual
            .fldShimDescrip = mLeakEquip.ShimDescrip
            .fldSF_Platen = mLeakEquip.Platen_SF     'AES 15MAR17
            .fldTestMeterMake = mLeakEquip.TestMeterMake
            .fldTestMeterSN = mLeakEquip.TestMeterSN
            .fldIsSpringBack = mLeak.Springback

            If (Not pRecExists) Then
                pSealTestEntities.AddTotblLeak(pLeak)
                pSealTestEntities.SaveChanges()
            Else
                pSealTestEntities.SaveChanges()
            End If

        End With

    End Sub


    Public Sub SaveTo_tblLeakData(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '==========================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblLeakData
        Dim pLeakData As New tblLeakData

        Dim pQryLeakData = (From pRec In pSealTestEntities.tblLeakData
                       Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                       pRec.fldTestRptID = mID Select pRec).ToList()

        '....Delete Existing Leak Data
        If (pQryLeakData.Count() > 0) Then
            For j As Integer = 0 To pQryLeakData.Count() - 1
                pSealTestEntities.DeleteObject(pQryLeakData(j))
                pSealTestEntities.SaveChanges()
            Next
        End If

        Dim pLeakData_list As New List(Of tblLeakData)
        Dim pIndex As Integer = 0
        For j As Integer = 0 To mTestSeal.Count - 1
            Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
            mTestSeal(j).IsTestData(pIsLeak, pIsLoad, pIsPress)

            If (pIsLeak) Then
                Dim pTblLeakData As New tblLeakData
                pLeakData_list.Add(pTblLeakData)
                With pLeakData_list(pIndex)
                    .fldTestProjectID = TestProjectID_In
                    .fldTestMOID = TestMOID_In
                    .fldTestRptID = mID
                    .fldSealSeqID = (j + 1)
                    .fldFHIni = mTestSeal(j).Leak.FHIni
                    .fldFHFinal = mTestSeal(j).Leak.FHFinal
                    .fldODPre = mTestSeal(j).Leak.ODPre
                    .fldODPost = mTestSeal(j).Leak.ODPost
                    .fldIDPre = mTestSeal(j).Leak.IDPre
                    .fldIDPost = mTestSeal(j).Leak.IDPost
                    .fldVal = mTestSeal(j).Leak.Val
                End With

                pSealTestEntities.AddTotblLeakData(pLeakData_list(pIndex))
                pIndex = pIndex + 1
            End If

        Next

        pSealTestEntities.SaveChanges()

    End Sub

    '....Load: tblLoad
    Public Sub SaveTo_tblLoad(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '=====================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblLoad
        Dim pLoad As New tblLoad
        Dim pRecExists As Boolean = False
        Dim pQryLoad = (From pRec In pSealTestEntities.tblLoad
                       Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                       pRec.fldTestRptID = mID Select pRec).ToList()

        If (pQryLoad.Count() > 0) Then
            pLoad = pQryLoad(0)
            pRecExists = True
        End If

        With pLoad
            .fldTestProjectID = TestProjectID_In
            .fldTestMOID = TestMOID_In
            .fldTestRptID = mID
            .fldStandName = mLoadEquip.StandName
            .fldStandSN = mLoadEquip.StandSN
            .fldLoadCellMake = mLoadEquip.LoadCellMake
            .fldLoadCellSN = mLoadEquip.LoadCellSN
            .fldIsSpringBack = mLeak.Springback

            If (Not pRecExists) Then
                pSealTestEntities.AddTotblLoad(pLoad)
                pSealTestEntities.SaveChanges()
            Else
                pSealTestEntities.SaveChanges()
            End If

        End With

    End Sub

    Public Sub SaveTo_tblLoadData(ByVal TestProjectID_In As Integer, ByVal TestMOID_In As Integer)
        '==========================================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblLoadData
        Dim pLoadData As New tblLoadData

        Dim pQryLoadData = (From pRec In pSealTestEntities.tblLoadData
                       Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = TestMOID_In And _
                       pRec.fldTestRptID = mID Select pRec).ToList()

        '....Delete Existing Load Data
        If (pQryLoadData.Count() > 0) Then
            For j As Integer = 0 To pQryLoadData.Count() - 1
                pSealTestEntities.DeleteObject(pQryLoadData(j))
                pSealTestEntities.SaveChanges()
            Next
        End If

        Dim pLoadData_list As New List(Of tblLoadData)
        Dim pIndex As Integer = 0
        For j As Integer = 0 To mTestSeal.Count - 1
            Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
            mTestSeal(j).IsTestData(pIsLeak, pIsLoad, pIsPress)

            If (pIsLoad) Then
                Dim pTblLoadData As New tblLoadData
                pLoadData_list.Add(pTblLoadData)
                With pLoadData_list(pIndex)
                    .fldTestProjectID = TestProjectID_In
                    .fldTestMOID = TestMOID_In
                    .fldTestRptID = mID
                    .fldSealSeqID = (j + 1)
                    .fldFHIni = mTestSeal(j).Load.FHIni
                    .fldFHFinal = mTestSeal(j).Load.FHFinal
                    .fldODPre = mTestSeal(j).Load.ODPre
                    .fldODPost = mTestSeal(j).Load.ODPost
                    .fldIDPre = mTestSeal(j).Load.IDPre
                    .fldIDPost = mTestSeal(j).Load.IDPost
                    .fldVal = mTestSeal(j).Load.Val
                End With

                pSealTestEntities.AddTotblLoadData(pLoadData_list(pIndex))
                pIndex = pIndex + 1
            End If

        Next

        pSealTestEntities.SaveChanges()

    End Sub


    Private Function ImgToByteArray(ByVal img As Image) As Byte()
        '=========================================================
        Using mStream As New MemoryStream()
            img.Save(mStream, img.RawFormat)
            Return mStream.ToArray()
        End Using
    End Function

#End Region


#Region "MAIN ROUTINES:"

    Public Sub Create(ByVal TestProject_In As Test_clsProject, ByVal MO_Sel_In As Integer,
                      ByVal Report_Sel_In As Integer, ByVal DocuFormat_In As String, ByVal FileName_In As String)
        '========================================================================================================

        If (mReportType = eReportType.Normal) Then
            NormalReport(TestProject_In, MO_Sel_In, Report_Sel_In, DocuFormat_In, FileName_In)

        ElseIf (mReportType = eReportType.Rejection) Then
            RejectionReport(TestProject_In, MO_Sel_In, Report_Sel_In, DocuFormat_In, FileName_In)
        End If
    End Sub


    Private Sub RejectionReport(ByVal TestProject_In As Test_clsProject, ByVal MO_Sel_In As Integer,
                             ByVal Report_Sel_In As Integer, ByVal DocuFormat_In As String, ByVal FileName_In As String)
        '===============================================================================================================

        Dim pWordApp As New WORD.Application()
        Dim pWordDoc As WORD.Document = Nothing

        Try

            pWordDoc = pWordApp.Documents.Add(mcRejectionReportFileName)


            With pWordDoc
                .Bookmarks.Item("bmrkQA").Range.Text = TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Quality.Name
                .Bookmarks.Item("bmrkMO").Range.Text = TestProject_In.Test_MO(MO_Sel_In).No & "/" & TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).No
                .Bookmarks.Item("bmrkClosingDate").Range.Text = GetClosedDate().ToShortDateString()
                .Bookmarks.Item("bmrkPN").Range.Text = TestProject_In.PartProject.PNR.PN() & " " & TestProject_In.PartProject.PNR.PN_Rev
                .Bookmarks.Item("bmrkSampleSize").Range.Text = TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).SealQty

                Dim pCountFaildSample As Integer = 0
                For i As Integer = 0 To TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).SealQty - 1
                    If (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).TestSeal(i).Status = Test_clsSeal.eStatus.Fail) Then
                        pCountFaildSample = pCountFaildSample + 1
                    End If
                Next
                .Bookmarks.Item("bmrkSampleFailedCount").Range.Text = pCountFaildSample
                .Bookmarks.Item("bmrkRejectedTotalCount").Range.Text = pCountFaildSample
                .Bookmarks.Item("bmrkTester").Range.Text = TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Tester.Name

                .Bookmarks.Item("bmrkNote").Range.Text = TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Notes

                '.Bookmarks.Item("bmrkMONo").Range.Text = TestProject_In.Test_MO(MO_Sel_In).No
                '.Bookmarks.Item("bmrkPartNo").Range.Text = TestProject_In.PN & " " & TestProject_In.Rev

            End With

            With pWordApp

                If DocuFormat_In = "PDF" Then

                    pWordDoc.SaveAs2(gTest_File.DirOut & "Doc", WdSaveFormat.wdFormatDocument97)

                    Dim paramExportFormat As WdExportFormat = WdExportFormat.wdExportFormatPDF
                    Dim paramMissing As Object = Type.Missing
                    Dim paramOpenAfterExport As Boolean = True
                    Dim paramExportOptimizeFor As WdExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint
                    Dim paramExportRange As WdExportRange = WdExportRange.wdExportAllDocument
                    Dim paramStartPage As Integer = 0
                    Dim paramEndPage As Integer = 0
                    Dim paramExportItem As WdExportItem = WdExportItem.wdExportDocumentContent
                    Dim paramIncludeDocProps As Boolean = True
                    Dim paramKeepIRM As Boolean = True
                    Dim paramCreateBookmarks As WdExportCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks
                    Dim paramDocStructureTags As Boolean = True
                    Dim paramBitmapMissingFonts As Boolean = True
                    Dim paramUseISO19005_1 As Boolean = False

                    pWordDoc.ExportAsFixedFormat(FileName_In, paramExportFormat, paramOpenAfterExport, paramExportOptimizeFor, _
                                                 paramExportRange, paramStartPage, paramEndPage, paramExportItem, _
                                                 paramIncludeDocProps, paramKeepIRM, paramCreateBookmarks, paramDocStructureTags, _
                                                 paramBitmapMissingFonts, paramUseISO19005_1, paramMissing)

                    pWordDoc.Close(WORD.WdSaveOptions.wdSaveChanges)    '....Close Document first
                    .Quit(WORD.WdSaveOptions.wdSaveChanges)             '........then Quit Word application

                    File.Delete(gTest_File.DirOut & "Doc.doc")

                ElseIf DocuFormat_In = "WORD" Then
                    Dim pObjMissing As Object = System.Reflection.Missing.Value
                    'pWordDoc.SaveAs2(FileName_In, WdSaveFormat.wdFormatDocument97)
                    pWordDoc.SaveAs2(FileName_In, WdSaveFormat.wdFormatDocument97, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing,
                                     pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing)
                    .Visible = True
                    .WindowState = WORD.WdWindowState.wdWindowStateMaximize

                End If

            End With

        Catch pEXP As Exception

            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes As Integer
            Dim pintAnswer As Integer

            pstrTitle = "ERROR MESSAGE: "
            pstrMsg = "Error in Report Creation"
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly
            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

        Finally

            pWordDoc = Nothing
            pWordApp = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()

        End Try
    End Sub

    Public Function GetClosedDate() As DateTime
        '=====================================
        Dim pDate As DateTime = DateTime.MinValue

        If (mTester.Signed And mEng.Signed And mQuality.Signed) Then

            If (mTester.DateSigned > mEng.DateSigned And mTester.DateSigned > mQuality.DateSigned) Then
                pDate = mTester.DateSigned

            ElseIf (mEng.DateSigned > mTester.DateSigned And mEng.DateSigned > mQuality.DateSigned) Then
                pDate = mEng.DateSigned

            ElseIf (mQuality.DateSigned > mTester.DateSigned And mQuality.DateSigned > mEng.DateSigned) Then
                pDate = mQuality.DateSigned
            End If

        End If

        Return pDate
    End Function


#End Region


#Region "UTILITY ROUTINES:"

    Private Sub SetSealData()
        '====================
        ''....SeaL data
        'If (mGen.Seal.Count > mSealQty) Then
        '    Dim pCount As Integer = mGen.Seal.Count
        '    For i As Integer = mSealQty To pCount - 1
        '        mGen.Seal.RemoveAt(i)
        '        mTestSeal.RemoveAt(i)
        '    Next
        'End If

        'For i As Integer = 0 To mSealQty - 1
        '    If (mGen.Seal.Count = i) Then
        '        '....New Case
        '        Dim pGenSeal As New sGenSeal
        '        pGenSeal.SeqID = i + 1
        '        pGenSeal.SN = i + 1
        '        mGen.Seal.Add(pGenSeal)

        '        Dim pTestSeal As New clsTestSeal
        '        mTestSeal.Add(pTestSeal)

        '    End If
        'Next

    End Sub


    Private Function GetSignature(Name_In As String) As Image
        '====================================================
        Dim pImage As Image = Nothing

        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblTestProject
        Dim pQry = (From pRec In pSealTestEntities.tblTestUser
                          Where pRec.fldName = Name_In.Trim() Select pRec).ToList()

        If (pQry.Count > 0) Then
            Dim pArray As Byte() = DirectCast(pQry(0).fldSignature, Byte())
            If (Not IsNothing(pArray)) Then
                Dim pMS As New MemoryStream(pArray)
                pImage = Image.FromStream(pMS)
            End If

        End If

        Return pImage

    End Function


    Private Function GetImage(FileName_In As String) As Image
        '=====================================================
        Dim pImage As Image = Nothing

        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblTestProject
        Dim pQry = (From pRec In pSealTestEntities.tblReportGenImage
                          Where pRec.fldImageFile = FileName_In.Trim() Select pRec).ToList()

        If (pQry.Count > 0) Then

            If (Not IsNothing(pQry(0).fldImage)) Then       'AES 29NOV16
                Dim pArray As Byte() = DirectCast(pQry(0).fldImage, Byte())
                Dim pMS As New MemoryStream(pArray)
                pImage = Image.FromStream(pMS)
            End If
        End If

        Return pImage

    End Function


    Private Sub NormalReport(ByVal TestProject_In As Test_clsProject, ByVal MO_Sel_In As Integer,
                             ByVal Report_Sel_In As Integer, ByVal DocuFormat_In As String, ByVal FileName_In As String)
        '===============================================================================================================

        Dim pWordApp As New WORD.Application()
        Dim pWordDoc As WORD.Document = Nothing

        Try

            Const pcNColumns As Integer = 5           '....# of Table Columns
            Const pcNColumns_SB As Integer = 4        '....# of Table Columns

            Dim iCell, iRow As Integer
            Dim pCaptionArray_Leak(pcNColumns) As String
            Dim pCaptionUnitArray_Leak(pcNColumns) As String

            Dim pCaptionArray_Load(pcNColumns) As String
            Dim pCaptionUnitArray_Load(pcNColumns) As String

            Dim pCaptionArray_SB(pcNColumns_SB) As String
            Dim pCaptionUnitArray_SB(pcNColumns_SB) As String

            Dim pobjTable_Leak, pobjTable_Load, pobjTable_SB, pobjTable_Unit As WORD.Table

            '....Store the caption array elements
            pCaptionArray_Leak(1) = "Serial No."    '"Sample"       'AES 14JUN17
            pCaptionArray_Leak(2) = "Initial Free Height"
            pCaptionArray_Leak(3) = "Leakage"
            pCaptionArray_Leak(4) = "FFH"
            pCaptionArray_Leak(5) = "Pass/Fail"

            '....Store the caption unit array elements
            pCaptionUnitArray_Leak(1) = ""
            pCaptionUnitArray_Leak(2) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"
            pCaptionUnitArray_Leak(3) = "(" & TestProject_In.Test_Unit.LeakUnit_Cust & ")" '"(cc/sec)"
            pCaptionUnitArray_Leak(4) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"
            pCaptionUnitArray_Leak(5) = ""

            '....Store the caption array elements
            pCaptionArray_Load(1) = "Serial No."    '"Sample"       'AES 14JUN17
            pCaptionArray_Load(2) = "Initial Free Height"
            pCaptionArray_Load(3) = "Load"
            pCaptionArray_Load(4) = "FFH"
            pCaptionArray_Load(5) = "Pass/Fail"

            '....Store the caption unit array elements
            pCaptionUnitArray_Load(1) = ""
            pCaptionUnitArray_Load(2) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"
            pCaptionUnitArray_Load(3) = "(" & TestProject_In.Test_Unit.FUnit_Cust & ")" '"(lbf)"
            pCaptionUnitArray_Load(4) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"
            pCaptionUnitArray_Load(5) = ""

            '....Store the caption array elements
            pCaptionArray_SB(1) = "Serial No."    '"Sample"       'AES 14JUN17
            pCaptionArray_SB(2) = "Initial Free Height"
            pCaptionArray_SB(3) = "FFH"
            pCaptionArray_SB(4) = "Total Springback"

            '....Store the caption unit array elements
            pCaptionUnitArray_SB(1) = ""
            pCaptionUnitArray_SB(2) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"
            pCaptionUnitArray_SB(3) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"
            pCaptionUnitArray_SB(4) = "(" & TestProject_In.Test_Unit.LUnit_Cust & ")" '"(in)"

            '....No. of Rows in Table
            Dim pCountLeak As Integer = 0
            Dim pCountLoad As Integer = 0
            For i As Integer = 0 To TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).TestSeal.Count - 1
                Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False
                TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).TestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)
                If (pIsLeak = True) Then
                    pCountLeak = pCountLeak + 1
                End If
                If (pIsLoad = True) Then
                    pCountLoad = pCountLoad + 1
                End If
            Next

            Dim nRows_Leak As Integer
            nRows_Leak = pCountLeak + 2       '....1st row for Columns Heading
            '                                   '......2nd for ColumnUnits Heading,Others for Records

            Dim nRows_Load As Integer
            nRows_Load = pCountLoad + 2       '....1st row for Columns Heading
            '                                   '......2nd for ColumnUnits Heading,Others for Records

            Dim pTestReportFileName As String = mcTestReportFileName_Pass

            If (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).DetermineOverallStatus() = Test_clsSeal.eStatus.Pass) Then
                pTestReportFileName = mcTestReportFileName_Pass
            ElseIf (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).DetermineOverallStatus() = Test_clsSeal.eStatus.Fail) Then
                pTestReportFileName = mcTestReportFileName_Failed
            End If

            pWordDoc = pWordApp.Documents.Add(pTestReportFileName)


            With pWordDoc
                Dim pTestType As String = ""
                If (mLeak.Exists And mLoad.Exists) Then
                    pTestType = "Leak & Load"
                ElseIf (mLeak.Exists And Not mLoad.Exists) Then
                    pTestType = "Leak"
                ElseIf (Not mLeak.Exists And mLoad.Exists) Then
                    pTestType = "Load"
                End If

                '.Bookmarks.Item("bmrkTestType").Range.Text = pTestType
                '.Bookmarks.Item("bmrkSealType").Range.Text = TestProject_In.Analysis.Seal.Type
                '.Bookmarks.Item("bmrkInternal_External").Range.Text = TestProject_In.Analysis.OpCond.POrient
                .Bookmarks.Item("bmrkPN").Range.Text = TestProject_In.PartProject.PNR.PN
                .Bookmarks.Item("bmrkRev").Range.Text = TestProject_In.PartProject.PNR.PN_Rev
                .Bookmarks.Item("bmrkMO").Range.Text = TestProject_In.Test_MO(MO_Sel_In).No & "/" & TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).No
                .Bookmarks.Item("bmrkPN_Date").Range.Text = GetClosedDate().ToShortDateString() 'DateTime.Now().ToShortDateString()
                .Bookmarks.Item("bmrkCustomer").Range.Text = TestProject_In.PartProject.CustInfo.CustName
                .Bookmarks.Item("bmrkCustomerPart").Range.Text = TestProject_In.PartProject.CustInfo.PN_Cust
                '.Bookmarks.Item("bmrkTestProcedure").Range.Text = "Per MA Spec"
                .Bookmarks.Item("bmrkTestParameters").Range.Text = "Per " & TestProject_In.PartProject.PNR.PN & " Rev " & TestProject_In.PartProject.PNR.PN_Rev & " drawing."

                '----------------
                Dim pLeakageDataExists As Boolean = False
                Dim pLoadDataExists As Boolean = False

                Dim pSealTestEntities As New SealTestDBEntities
                Dim pMOID As Integer = TestProject_In.Test_MO(MO_Sel_In).ID
                Dim pQryLeak = (From pRec In pSealTestEntities.tblLeakData
                    Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                    pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

                Dim pQryReport = (From pRec In pSealTestEntities.tblReport
                    Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                    pRec.fldID = mID Select pRec).ToList()

                If (pQryReport.Count() > 0) Then
                    If (pQryReport(0).fldIsLeak) Then
                        If (pQryReport(0).fldIsLeak_Leakage) Then
                            pLeakageDataExists = True
                        End If

                    End If

                End If

                Dim pQryLoadData = (From pRec In pSealTestEntities.tblLoadData
                    Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                    pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

                If (pQryLoadData.Count() > 0) Then
                    pLoadDataExists = True
                End If

                '---------------
                If (pLeakageDataExists) Then

                    If (mLeakEquip.StandName <> "") Then
                        .Bookmarks.Item("bmrkEquipR1C1").Range.Text = "Leak Test Stand"
                        .Bookmarks.Item("bmrkEquipR2C1").Range.Text = "Shims"
                        .Bookmarks.Item("bmrkEquipR3C1").Range.Text = "Leakage Device"
                        .Bookmarks.Item("bmrkEquipR4C1").Range.Text = "Calibration Due Date"
                        .Bookmarks.Item("bmrkEquipR1C2").Range.Text = mLeakEquip.StandName
                    End If

                    If (mLeakEquip.ShimDescrip <> "") Then
                        .Bookmarks.Item("bmrkEquipR2C2").Range.Text = mLeakEquip.ShimDescrip
                    End If

                    If (mLeakEquip.TestMeterModelNo <> "") Then
                        .Bookmarks.Item("bmrkEquipR3C2").Range.Text = mLeakEquip.TestMeterModelNo
                    End If

                    If (Not IsDBNull(mLeakEquip.TestMeterDateCalibrationDue)) Then
                        .Bookmarks.Item("bmrkEquipR4C2").Range.Text = mLeakEquip.TestMeterDateCalibrationDue.ToShortDateString()
                    End If

                End If


                If (pLoadDataExists) Then

                    If (pLeakageDataExists) Then

                        If (mLoadEquip.StandName <> "") Then
                            .Bookmarks.Item("bmrkEquipR1C3").Range.Text = "Load Test Stand"
                            .Bookmarks.Item("bmrkEquipR2C3").Range.Text = "Load Cell: SL No."
                            .Bookmarks.Item("bmrkEquipR3C3").Range.Text = "Range"
                            .Bookmarks.Item("bmrkEquipR4C3").Range.Text = "Calibration Due Date"
                            .Bookmarks.Item("bmrkEquipR1C4").Range.Text = mLoadEquip.StandName
                        End If

                        If (mLoadEquip.LoadCellSN <> "") Then
                            .Bookmarks.Item("bmrkEquipR2C4").Range.Text = mLoadEquip.LoadCellSN
                        End If

                        If (mLoadEquip.LoadCellRange <> "") Then
                            .Bookmarks.Item("bmrkEquipR3C4").Range.Text = mLoadEquip.LoadCellRange
                        End If

                        If (Not IsDBNull(mLoadEquip.LoadCellDateCalibrationDue)) Then
                            .Bookmarks.Item("bmrkEquipR4C4").Range.Text = mLoadEquip.LoadCellDateCalibrationDue.ToShortDateString()
                        End If
                    Else

                        If (mLoadEquip.StandName <> "") Then
                            .Bookmarks.Item("bmrkEquipR1C1").Range.Text = "Load Test Stand"
                            .Bookmarks.Item("bmrkEquipR2C1").Range.Text = "Load Cell: SL No."
                            .Bookmarks.Item("bmrkEquipR3C1").Range.Text = "Range"
                            .Bookmarks.Item("bmrkEquipR4C1").Range.Text = "Calibration Due Date"
                            .Bookmarks.Item("bmrkEquipR1C2").Range.Text = mLoadEquip.StandName
                        End If

                        If (mLoadEquip.LoadCellSN <> "") Then
                            .Bookmarks.Item("bmrkEquipR2C2").Range.Text = mLoadEquip.LoadCellSN
                        End If

                        If (mLoadEquip.LoadCellRange <> "") Then
                            .Bookmarks.Item("bmrkEquipR3C2").Range.Text = mLoadEquip.LoadCellRange
                        End If

                        If (Not IsDBNull(mLoadEquip.LoadCellDateCalibrationDue)) Then
                            .Bookmarks.Item("bmrkEquipR4C2").Range.Text = mLoadEquip.LoadCellDateCalibrationDue.ToShortDateString()
                        End If

                    End If

                End If

                '-----------
                Dim pblnLUnit As Boolean = False
                Dim pblnFUnit As Boolean = False
                Dim pblnPUnit As Boolean = False
                Dim pblnLeakUnit As Boolean = False

                Dim pRowCount As Integer = 0

                If (TestProject_In.Test_Unit.LUnit_PH <> TestProject_In.Test_Unit.LUnit_Cust) Then
                    pblnLUnit = True
                    pRowCount = pRowCount + 1
                End If

                If (TestProject_In.Test_Unit.FUnit_PH <> TestProject_In.Test_Unit.FUnit_Cust) Then
                    pblnFUnit = True
                    pRowCount = pRowCount + 1
                End If

                If (TestProject_In.Test_Unit.PUnit_PH <> TestProject_In.Test_Unit.PUnit_Cust) Then
                    pblnPUnit = True
                    pRowCount = pRowCount + 1
                End If

                If (TestProject_In.Test_Unit.LeakUnit_PH <> TestProject_In.Test_Unit.LeakUnit_Cust) Then
                    pblnLeakUnit = True
                    pRowCount = pRowCount + 1
                End If

                If (pRowCount > 0) Then

                    .Bookmarks.Item("bmrkConvTableTxt").Range.Text = "Conversion Table"
                    .Bookmarks.Item("bmrkConvTableTxt").Range.Select()
                    .Bookmarks.Item("bmrkConvTableTxt").Range.Font.Bold = True     'AES 17FEB17

                    .Bookmarks.Item("bmrkConvFac").Range.Text = "Conversion Factor" 'AES 17FEB17

                    '....Table to show Conv. Table
                    pobjTable_Unit = .Bookmarks.Item("bmrkConvTable").Range.Tables. _
                               Add(.Bookmarks.Item("bmrkConvTable").Range, pRowCount, 3)
                    With pobjTable_Unit
                        .PreferredWidth = 250
                        For k As Integer = 1 To pRowCount
                            If (pblnLUnit) Then
                                .Rows.Item(k).Cells.Item(1).Range.InsertAfter(TestProject_In.Test_Unit.LUnit_PH)
                                .Rows.Item(k).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(TestProject_In.Test_Unit.ConvF("L")))
                                .Rows.Item(k).Cells.Item(3).Range.InsertAfter(TestProject_In.Test_Unit.LUnit_Cust)
                                pblnLUnit = False

                            ElseIf (pblnFUnit) Then
                                .Rows.Item(k).Cells.Item(1).Range.InsertAfter(TestProject_In.Test_Unit.FUnit_PH)
                                .Rows.Item(k).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(TestProject_In.Test_Unit.ConvF("F")))
                                .Rows.Item(k).Cells.Item(3).Range.InsertAfter(TestProject_In.Test_Unit.FUnit_Cust)
                                pblnFUnit = False

                            ElseIf (pblnPUnit) Then
                                .Rows.Item(k).Cells.Item(1).Range.InsertAfter(TestProject_In.Test_Unit.PUnit_PH)
                                .Rows.Item(k).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(TestProject_In.Test_Unit.ConvF("P")))
                                .Rows.Item(k).Cells.Item(3).Range.InsertAfter(TestProject_In.Test_Unit.PUnit_Cust)
                                pblnPUnit = False
                            ElseIf (pblnLeakUnit) Then
                                .Rows.Item(k).Cells.Item(1).Range.InsertAfter(TestProject_In.Test_Unit.LeakUnit_PH)
                                .Rows.Item(k).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(TestProject_In.Test_Unit.ConvF("Leak")))
                                .Rows.Item(k).Cells.Item(3).Range.InsertAfter(TestProject_In.Test_Unit.LeakUnit_Cust)
                                pblnLeakUnit = False

                            End If
                            .Rows.Item(k).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            .Rows.Item(k).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle

                        Next
                        .Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter
                    End With

                End If

                '--------------
                If (mNotes <> "") Then
                    .Bookmarks.Item("bmrkNotes").Range.Text = mNotes
                Else
                    'AES 02MAR17
                    If (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).DetermineOverallStatus() = Test_clsSeal.eStatus.Pass) Then
                        .Bookmarks.Item("bmrkNotes").Range.Text = "ALL TEST PASSED"
                    ElseIf (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).DetermineOverallStatus() = Test_clsSeal.eStatus.Fail) Then
                        .Bookmarks.Item("bmrkNotes").Range.Text = "TEST FAILED"
                    End If

                End If

                Dim pWidth As Integer = 120
                Dim pHeight As Integer = pWidth / mcImgAspectRatio
                'Dim pNewBmp As Bitmap = New Bitmap(pWidth, pHeight)

                If (mTester.Signed) Then

                    If (mTester.Name <> "") Then
                        Dim pImage As Image = GetSignature(mTester.Name)
                        If (Not IsNothing(pImage)) Then
                            Dim pBmp As Bitmap = New Bitmap(pImage)
                            'Dim pWidth As Integer = 120
                            'Dim pHeight As Integer = pWidth / mcImgAspectRatio
                            Dim pNewBmp As Bitmap = New Bitmap(pWidth, pHeight)

                            '....Declare graphic taken from new bitmap
                            Dim pGr As Graphics = Graphics.FromImage(pNewBmp)
                            pGr.DrawImage(pBmp, 0, 0, pNewBmp.Width, pNewBmp.Height)

                            Dim pTemp_ImagePath As String = "C:\SealSuite\Images\UserTester.jpeg"
                            pNewBmp.Save(pTemp_ImagePath, Imaging.ImageFormat.Jpeg)
                            .Bookmarks.Item("bmrkTestPerformBy").Range.InlineShapes.AddPicture(pTemp_ImagePath)

                            If (File.Exists(pTemp_ImagePath)) Then
                                File.Delete(pTemp_ImagePath)
                            End If
                        End If

                    End If

                    .Bookmarks.Item("bmrkTestPerformDate").Range.Text = mTester.DateSigned.ToShortDateString()
                End If

                If (mEng.Signed) Then
                    '.Bookmarks.Item("bmrkEngApproval").Range.Text = mUserEngg

                    If (mEng.Name <> "") Then
                        Dim pImage As Image = GetSignature(mEng.Name)

                        If (Not IsNothing(pImage)) Then
                            Dim pBmp As Bitmap = New Bitmap(pImage)
                            'Dim pNewBmp As Bitmap = New Bitmap(CInt(80), CInt(25))
                            Dim pNewBmp As Bitmap = New Bitmap(pWidth, pHeight)

                            '....Declare graphic taken from new bitmap
                            Dim pGr As Graphics = Graphics.FromImage(pNewBmp)
                            pGr.DrawImage(pBmp, 0, 0, pNewBmp.Width, pNewBmp.Height)

                            Dim pTemp_ImagePath As String = "C:\SealSuite\Images\UserEngg.jpeg"
                            pNewBmp.Save(pTemp_ImagePath, Imaging.ImageFormat.Jpeg)
                            .Bookmarks.Item("bmrkEngApproval").Range.InlineShapes.AddPicture(pTemp_ImagePath)

                            If (File.Exists(pTemp_ImagePath)) Then
                                File.Delete(pTemp_ImagePath)
                            End If
                        End If


                        'Dim pImage As Image = GetSignature(mUserEngg)
                        'pRange = .Bookmarks.Item("bmrkEngApproval").Range
                        'Dim pName As String = pRange.Bookmarks.Count.ToString()
                        'Clipboard.SetImage(pImage)
                        'pRange.Paste()
                        'Clipboard.Clear()

                    End If
                    .Bookmarks.Item("bmrkEngApprovalDate").Range.Text = mEng.DateSigned.ToShortDateString()
                End If


                If (mQuality.Signed) Then

                    If (mQuality.Name <> "") Then
                        Dim pImage As Image = GetSignature(mQuality.Name)

                        If (Not IsNothing(pImage)) Then
                            Dim pBmp As Bitmap = New Bitmap(pImage)
                            'Dim pNewBmp As Bitmap = New Bitmap(CInt(80), CInt(25))
                            Dim pNewBmp As Bitmap = New Bitmap(pWidth, pHeight)

                            '....Declare graphic taken from new bitmap
                            Dim pGr As Graphics = Graphics.FromImage(pNewBmp)
                            pGr.DrawImage(pBmp, 0, 0, pNewBmp.Width, pNewBmp.Height)

                            Dim pTemp_ImagePath As String = "C:\SealSuite\Images\UserQuality.jpeg"
                            pNewBmp.Save(pTemp_ImagePath, Imaging.ImageFormat.Jpeg)
                            .Bookmarks.Item("bmrkQualityApproval").Range.InlineShapes.AddPicture(pTemp_ImagePath)

                            If (File.Exists(pTemp_ImagePath)) Then
                                File.Delete(pTemp_ImagePath)
                            End If
                        End If

                    End If
                    .Bookmarks.Item("bmrkQualityApprovalDate").Range.Text = mQuality.DateSigned.ToShortDateString()
                End If

                If (mLeak.Exists) Then

                    'Dim pSealTestEntities As New SealTestDBEntities
                    'Dim pMOID As Integer = TestProject_In.MO(MO_Sel_In).ID
                    'Dim pQryLeakData = (From pRec In pSealTestEntities.tblLeakData
                    '    Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                    '    pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

                    If (pLeakageDataExists) Then

                        Dim pVal As Single = TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).LeakEquip.ShimActual
                        Dim pTempVal As Double = pVal * TestProject_In.Test_Unit.ConvF("L")
                        Dim pPlating As String = Space(8) & "Plating:"
                        If (pQryReport(0).fldIsLeak_LeakagePlated) Then
                            pPlating = pPlating & " Y"
                        Else
                            pPlating = pPlating & " N"
                        End If

                        .Bookmarks.Item("bmrkLeakResultTxt").Range.Text = "Leak Results: tested to " & TestProject_In.Test_Unit.WriteInUserL_Cust(pTempVal) & " " & TestProject_In.Test_Unit.LUnit_Cust & " height." & pPlating

                        '....Table to show Leak Results
                        pobjTable_Leak = .Bookmarks.Item("bmrkLeakResult").Range.Tables. _
                                   Add(.Bookmarks.Item("bmrkLeakResult").Range, nRows_Leak, pcNColumns)

                        With pobjTable_Leak
                            '.Shading.BackgroundPatternColor = WORD.WdColor.wdColorGray10
                            '.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble
                            .Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter

                            'With .Range.Font
                            '    .Underline = WORD.WdUnderline.wdUnderlineNone
                            '    .Bold = True
                            '    .Size = 8
                            'End With

                            '.AllowAutoFit = True

                            iRow = 1
                            '....Column Captions                    '....1st Row is for Captions
                            For iCell = 1 To pcNColumns
                                With .Range.Font
                                    .Underline = WORD.WdUnderline.wdUnderlineNone
                                    .Bold = True
                                    .Size = 12
                                End With
                                .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionArray_Leak(iCell))

                                .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            Next iCell

                            iRow = iRow + 1
                            iCell = 1

                            '....Column Unit Captions                '....2nd Row is for Unit Captions
                            For iCell = 1 To pcNColumns
                                With .Range.Font
                                    .Underline = WORD.WdUnderline.wdUnderlineNone
                                    .Bold = False
                                    .Size = 8
                                End With
                                .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionUnitArray_Leak(iCell))

                                .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                '.Rows.Item(iRow).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            Next iCell


                            iRow = iRow + 1

                            Dim pQryLeakData = (From pRec In pSealTestEntities.tblLeakData
                                Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                                pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

                            For i As Integer = 0 To pQryLeakData.Count() - 1

                                Dim pSeqID As Integer = pQryLeakData(i).fldSealSeqID

                                If (pSeqID > 0) Then
                                    With .Range.Font
                                        .Underline = WORD.WdUnderline.wdUnderlineNone
                                        .Bold = False
                                        .Size = 10
                                    End With
                                    Dim pSeq_ID As Integer = pQryLeakData(i).fldSealSeqID
                                    .Rows.Item(iRow).Cells.Item(1).Range.InsertAfter(pSeq_ID)
                                    Dim pTemp As Double = pQryLeakData(i).fldFHIni * TestProject_In.Test_Unit.ConvF("L")
                                    .Rows.Item(iRow).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pTemp))

                                    pTemp = pQryLeakData(i).fldVal * TestProject_In.Test_Unit.ConvF("Leak")
                                    .Rows.Item(iRow).Cells.Item(3).Range.InsertAfter(pTemp.ToString("0.00E+00"))

                                    pTemp = pQryLeakData(i).fldFHFinal * TestProject_In.Test_Unit.ConvF("L")
                                    .Rows.Item(iRow).Cells.Item(4).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pTemp))

                                    'AES 02MAR17
                                    If (mTestSeal(pSeqID - 1).Status = Test_clsSeal.eStatus.Pass) Then
                                        .Rows.Item(iRow).Cells.Item(5).Range.InsertAfter("Pass")
                                    Else
                                        .Rows.Item(iRow).Cells.Item(5).Range.InsertAfter("FAIL")
                                    End If

                                    .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                    .Rows.Item(iRow).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                    iRow = iRow + 1
                                End If

                            Next

                            .Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleDouble

                        End With
                    End If

                End If

                '....Load
                If (mLoad.Exists) Then

                    'Dim pSealTestEntities As New SealTestDBEntities
                    'Dim pMOID As Integer = TestProject_In.MO(MO_Sel_In).ID
                    'Dim pQryLoadData = (From pRec In pSealTestEntities.tblLoadData
                    '    Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                    '    pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

                    If (pLoadDataExists) Then

                        Dim pVal As Single = 0.0

                        If (TestProject_In.Test_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
                            pVal = TestProject_In.Test_Spec.LoadMin_CavityDepth

                        ElseIf (TestProject_In.Test_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
                            pVal = TestProject_In.Test_Spec.LoadMax_CavityDepth

                        ElseIf (TestProject_In.Test_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
                            pVal = TestProject_In.Test_Spec.LoadRange_CavityDepth

                        End If

                        Dim pTempVal As Double = pVal * TestProject_In.Test_Unit.ConvF("L")
                        .Bookmarks.Item("bmrkLoadResultTxt").Range.Text = "Load Results: tested to " & TestProject_In.Test_Unit.WriteInUserL_Cust(pTempVal) & " " & TestProject_In.Test_Unit.LUnit_Cust & " height."

                        '....Table to show Load Tests
                        pobjTable_Load = .Bookmarks.Item("bmrkLoadResult").Range.Tables. _
                                   Add(.Bookmarks.Item("bmrkLoadResult").Range, nRows_Load, pcNColumns)

                        With pobjTable_Load
                            '.Shading.BackgroundPatternColor = WORD.WdColor.wdColorGray10
                            '.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble
                            .Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter

                            'With .Range.Font
                            '    .Underline = WORD.WdUnderline.wdUnderlineNone
                            '    .Bold = True
                            '    .Size = 8
                            'End With

                            .AllowAutoFit = True

                            iRow = 1
                            '....Column Captions                    '....1st Row is for Captions
                            For iCell = 1 To pcNColumns
                                With .Range.Font
                                    .Underline = WORD.WdUnderline.wdUnderlineNone
                                    .Bold = True
                                    .Size = 12
                                End With
                                .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionArray_Load(iCell))
                                .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            Next iCell

                            iRow = iRow + 1
                            iCell = 1

                            '....Column Unit Captions                '....2nd Row is for Unit Captions
                            For iCell = 1 To pcNColumns
                                With .Range.Font
                                    .Underline = WORD.WdUnderline.wdUnderlineNone
                                    .Bold = False
                                    .Size = 8
                                End With

                                .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionUnitArray_Load(iCell))

                                .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                '.Rows.Item(iRow).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            Next iCell


                            iRow = iRow + 1

                            For i As Integer = 0 To pQryLoadData.Count() - 1

                                Dim pSeqID As Integer = pQryLoadData(i).fldSealSeqID

                                If (pSeqID > 0) Then
                                    With .Range.Font
                                        .Underline = WORD.WdUnderline.wdUnderlineNone
                                        .Bold = False
                                        .Size = 10
                                    End With
                                    Dim pSeq_ID As Integer = pQryLoadData(i).fldSealSeqID
                                    .Rows.Item(iRow).Cells.Item(1).Range.InsertAfter(pSeq_ID)
                                    Dim pTemp As Double = pQryLoadData(i).fldFHIni * TestProject_In.Test_Unit.ConvF("L")
                                    .Rows.Item(iRow).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pTemp))


                                    pTemp = TestProject_In.Test_Unit.FormatLoadVal_Cust(pQryLoadData(i).fldVal)
                                    .Rows.Item(iRow).Cells.Item(3).Range.InsertAfter(pTemp)


                                    pTemp = pQryLoadData(i).fldFHFinal * TestProject_In.Test_Unit.ConvF("L")
                                    .Rows.Item(iRow).Cells.Item(4).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pTemp))

                                    'AES 02MAR17
                                    If (mTestSeal(pSeqID - 1).Status = Test_clsSeal.eStatus.Pass) Then
                                        .Rows.Item(iRow).Cells.Item(5).Range.InsertAfter("Pass")
                                    Else
                                        .Rows.Item(iRow).Cells.Item(5).Range.InsertAfter("FAIL")
                                    End If

                                    '.Rows.Item(iRow).Cells.Item(5).Range.InsertAfter("Pass")
                                    .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                    .Rows.Item(iRow).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                    iRow = iRow + 1
                                End If

                            Next
                            'End If

                            .Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleDouble

                        End With

                    End If

                End If


                '....Springback
                If (mLeak.Exists) Then

                    If (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Leak.Springback) Then



                        Dim pVal As Single = TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).LeakEquip.ShimActual
                        Dim pTempVal As Double = pVal * TestProject_In.Test_Unit.ConvF("L")
                        .Bookmarks.Item("bmrkSpringbackResultTxt").Range.Text = "Springback Results: tested to " & TestProject_In.Test_Unit.WriteInUserL_Cust(pTempVal) & " " & TestProject_In.Test_Unit.LUnit_Cust & " height."

                        '....Table to show Spring Results
                        pobjTable_SB = .Bookmarks.Item("bmrkSpringbackResult").Range.Tables. _
                                   Add(.Bookmarks.Item("bmrkSpringbackResult").Range, nRows_Leak, pcNColumns_SB)

                        With pobjTable_SB
                            '.Shading.BackgroundPatternColor = WORD.WdColor.wdColorGray10
                            '.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble
                            .Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter

                            'With .Range.Font
                            '    .Underline = WORD.WdUnderline.wdUnderlineNone
                            '    .Bold = True
                            '    .Size = 8
                            'End With

                            .AllowAutoFit = True

                            iRow = 1
                            '...Column Captions                    '....1st Row is for Captions
                            For iCell = 1 To pcNColumns_SB
                                With .Range.Font
                                    .Underline = WORD.WdUnderline.wdUnderlineNone
                                    .Bold = True
                                    .Size = 12
                                End With
                                .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionArray_SB(iCell))
                                .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            Next iCell

                            iRow = iRow + 1
                            iCell = 1

                            '....Column Unit Captions                '....2nd Row is for Unit Captions
                            For iCell = 1 To pcNColumns_SB
                                With .Range.Font
                                    .Underline = WORD.WdUnderline.wdUnderlineNone
                                    .Bold = False
                                    .Size = 8
                                End With
                                .AllowAutoFit = True
                                .Rows.Item(iRow).Height = 8
                                .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionUnitArray_SB(iCell))

                                .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                '.Rows.Item(iRow).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            Next iCell


                            iRow = iRow + 1

                            'Dim pSealTestEntities As New SealTestDBEntities
                            'Dim pMOID As Integer = TestProject_In.MO(MO_Sel_In).ID
                            Dim pQryLeakData = (From pRec In pSealTestEntities.tblLeakData
                                Where pRec.fldTestProjectID = TestProject_In.ID And pRec.fldTestMOID = pMOID And
                                pRec.fldTestRptID = mID Order By pRec.fldSealSeqID Ascending Select pRec).ToList()

                            If (pLeakageDataExists) Then

                                For i As Integer = 0 To pQryLeakData.Count() - 1

                                    Dim pSeqID As Integer = pQryLeakData(i).fldSealSeqID

                                    If (pSeqID > 0) Then
                                        With .Range.Font
                                            .Underline = WORD.WdUnderline.wdUnderlineNone
                                            .Bold = False
                                            .Size = 10
                                        End With
                                        Dim pSeq_ID As Integer = pQryLeakData(i).fldSealSeqID
                                        .Rows.Item(iRow).Cells.Item(1).Range.InsertAfter(pSeq_ID)

                                        Dim pTemp As Double = pQryLeakData(i).fldFHIni * TestProject_In.Test_Unit.ConvF("L")
                                        .Rows.Item(iRow).Cells.Item(2).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pTemp))

                                        pTemp = pQryLeakData(i).fldFHFinal * TestProject_In.Test_Unit.ConvF("L")
                                        .Rows.Item(iRow).Cells.Item(3).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pTemp))

                                        Dim pVal1 As Single = pQryLeakData(i).fldFHFinal
                                        Dim pFHFinal As Double = gPartUnit.L_UserToCon(pVal1)
                                        Dim pCavity As Double = pVal
                                        Dim pSpringBack As Double = (pFHFinal - pVal) * TestProject_In.Test_Unit.ConvF("L")
                                        .Rows.Item(iRow).Cells.Item(4).Range.InsertAfter(TestProject_In.Test_Unit.WriteInUserL_Cust(pSpringBack))
                                        .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                        .Rows.Item(iRow).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                                        iRow = iRow + 1
                                    End If

                                Next
                            End If

                            .Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleDouble
                        End With
                    End If

                End If

            End With

            '....Insert Images
            Dim pSelectedImg_Count As Integer = 0
            For i As Integer = 0 To TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Gen.Image.Count - 1
                If (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Gen.Image(i).Selected) Then
                    pSelectedImg_Count = pSelectedImg_Count + 1
                End If
            Next

            If (pSelectedImg_Count > 0) Then
                For i As Integer = 0 To TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Gen.Image.Count - 1

                    If (TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Gen.Image(i).Selected) Then
                        Dim pPageBreak = WORD.WdBreakType.wdPageBreak
                        Dim pUnit = WORD.WdUnits.wdStory
                        pWordApp.Selection.EndKey(pUnit)
                        pWordApp.Selection.InsertBreak(pPageBreak)

                        Dim pImage As Image = GetImage(TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Gen.Image(i).File)

                        Dim pPara As WORD.Paragraph = Nothing
                        If (Not IsNothing(pImage)) Then     'AES 29NOV16
                            'Dim pPara As WORD.Paragraph = pWordDoc.Paragraphs.Add()
                            pPara = pWordDoc.Paragraphs.Add()
                            Clipboard.SetImage(pImage)
                            pPara.Range.Paste()
                            pPara.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter
                            Clipboard.Clear()
                        Else
                            pPara = pWordDoc.Paragraphs.Add()
                            PDFToImage(pPara, TestProject_In.Test_MO(MO_Sel_In).Test_Report(Report_Sel_In).Gen.Image(i).File, gTest_File.DirOut)
                            'pPara.Range.Paste()
                            'pPara.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter
                            'Clipboard.Clear()
                        End If

                    End If
                Next

            End If

            With pWordApp

                If DocuFormat_In = "PDF" Then

                    pWordDoc.SaveAs2(gTest_File.DirOut & "Doc", WdSaveFormat.wdFormatDocument97)

                    Dim paramExportFormat As WdExportFormat = WdExportFormat.wdExportFormatPDF
                    Dim paramMissing As Object = Type.Missing
                    Dim paramOpenAfterExport As Boolean = True
                    Dim paramExportOptimizeFor As WdExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint
                    Dim paramExportRange As WdExportRange = WdExportRange.wdExportAllDocument
                    Dim paramStartPage As Integer = 0
                    Dim paramEndPage As Integer = 0
                    Dim paramExportItem As WdExportItem = WdExportItem.wdExportDocumentContent
                    Dim paramIncludeDocProps As Boolean = True
                    Dim paramKeepIRM As Boolean = True
                    Dim paramCreateBookmarks As WdExportCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks
                    Dim paramDocStructureTags As Boolean = True
                    Dim paramBitmapMissingFonts As Boolean = True
                    Dim paramUseISO19005_1 As Boolean = False

                    pWordDoc.ExportAsFixedFormat(FileName_In, paramExportFormat, paramOpenAfterExport, paramExportOptimizeFor, _
                                                 paramExportRange, paramStartPage, paramEndPage, paramExportItem, _
                                                 paramIncludeDocProps, paramKeepIRM, paramCreateBookmarks, paramDocStructureTags, _
                                                 paramBitmapMissingFonts, paramUseISO19005_1, paramMissing)

                    pWordDoc.Close(WORD.WdSaveOptions.wdSaveChanges)    '....Close Document first
                    .Quit(WORD.WdSaveOptions.wdSaveChanges)             '........then Quit Word application

                    File.Delete(gTest_File.DirOut & "Doc.doc")

                ElseIf DocuFormat_In = "WORD" Then
                    Dim pObjMissing As Object = System.Reflection.Missing.Value
                    'pWordDoc.SaveAs2(FileName_In, WdSaveFormat.wdFormatDocument97)
                    pWordDoc.SaveAs2(FileName_In, WdSaveFormat.wdFormatDocument97, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing,
                                     pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing, pObjMissing)
                    .Visible = True
                    .WindowState = WORD.WdWindowState.wdWindowStateMaximize

                End If

            End With

        Catch pEXP As Exception

            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes As Integer
            Dim pintAnswer As Integer

            pstrTitle = "ERROR MESSAGE: "
            pstrMsg = "Error in Report Creation"
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly
            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

        Finally

            pWordDoc = Nothing
            pWordApp = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()

        End Try

    End Sub


    Private Sub CloseWordFiles()
        '=======================

        Dim pProcesses As Process() = Process.GetProcesses()

        Try
            For Each p As Process In pProcesses
                If p.ProcessName = "WORD" Then
                    p.Kill()
                End If
            Next

        Catch pEXP As Exception
        End Try
    End Sub

    Public Function DetermineOverallStatus() As Test_clsSeal.eStatus
        '==========================================================
        Dim pStatus As Test_clsSeal.eStatus = Test_clsSeal.eStatus.Pass

        For i As Integer = 0 To mTestSeal.Count - 1
            If (mTestSeal(i).Status = Test_clsSeal.eStatus.Fail) Then
                pStatus = Test_clsSeal.eStatus.Fail
                Return pStatus
            End If
        Next

        Return pStatus

    End Function


    Public Sub SetTestStatus(ByVal LeakMax_In As Double, ByVal SpringBackMin_In As Double,
                             ByVal LoadType_In As Test_clsSpec.eLoadType, ByVal LoadMax_In As Double, ByVal LoadMin_In As Double)
        '==========================================================================================================================

        For i As Integer = 0 To mTestSeal.Count - 1

            Dim pIsLeak As Boolean = False, pIsLoad As Boolean = False, pIsPress As Boolean = False

            mTestSeal(i).IsTestData(pIsLeak, pIsLoad, pIsPress)

            If (pIsLeak = True) Then
                '....Leak
                Dim pLeakMax As Double = LeakMax_In
                Dim pLeakMeasured As Double = mTestSeal(i).Leak.Val
                Dim pSpringMeasured As Double = mTestSeal(i).Leak.FHFinal - mLeakEquip.ShimActual
                Dim pSpringBackMin As Double = SpringBackMin_In

                If (pLeakMeasured <> 0.0 And pLeakMax <> 0.0) Then

                    If (pLeakMeasured <= pLeakMax) Then

                        If (mLeak.Springback) Then

                            If (pSpringMeasured > gcEPS And pSpringBackMin > gcEPS) Then
                                If (pSpringMeasured >= pSpringBackMin) Then
                                    mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                                Else
                                    mTestSeal(i).Status = Test_clsSeal.eStatus.Fail
                                End If
                            Else

                                mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                            End If

                        Else
                            mTestSeal(i).Status = Test_clsSeal.eStatus.Pass

                        End If
                    Else
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Fail
                    End If

                Else
                    If (mLeak.Springback) Then
                        If (pSpringMeasured > gcEPS And pSpringBackMin > gcEPS) Then
                            If (pSpringMeasured >= pSpringBackMin) Then
                                mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                            Else
                                mTestSeal(i).Status = Test_clsSeal.eStatus.Fail
                            End If
                        Else

                            mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                        End If

                    Else
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                    End If

                End If

            ElseIf (pIsLoad = True) Then

                '....Load
                Dim pLoadMax As Double = LoadMax_In
                Dim pLoadMeasured As Double = mTestSeal(i).Load.Val
                Dim pLoadMin As Double = LoadMin_In

                If (LoadType_In = Test_clsSpec.eLoadType.Min) Then

                    If (pLoadMeasured > pLoadMin Or Math.Abs(pLoadMeasured - pLoadMin) < gcEPS) Then
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                    Else
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Fail
                    End If

                ElseIf (LoadType_In = Test_clsSpec.eLoadType.Max) Then

                    If (pLoadMeasured < pLoadMax Or Math.Abs(pLoadMeasured - pLoadMax) < gcEPS) Then
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                    Else
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Fail
                    End If

                ElseIf (LoadType_In = Test_clsSpec.eLoadType.Range) Then

                    If ((pLoadMeasured > pLoadMin Or Math.Abs(pLoadMeasured - pLoadMin) < gcEPS) And
                        (pLoadMeasured < pLoadMax Or Math.Abs(pLoadMeasured - pLoadMax) < gcEPS)) Then
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Pass
                    Else
                        mTestSeal(i).Status = Test_clsSeal.eStatus.Fail
                    End If
                End If

            End If

        Next

    End Sub


    Public Sub PDFToImage(ByVal pPara As WORD.Paragraph, ByVal SourceFileName_In As String, ByVal DestinationPath_In As String)
        '======================================================================================================================
        Dim pPageCount As Integer = 0
        Dim pPdfDoc As Acrobat.CAcroPDDoc = New Acrobat.AcroPDDoc()
        Dim pPdfPage As Acrobat.CAcroPDPage = Nothing
        Dim pPdfRect As Acrobat.CAcroRect = New Acrobat.AcroRect()
        Dim pPdfPoint As New Acrobat.AcroPoint()

        If pPdfDoc.Open(SourceFileName_In) Then

            pPageCount = pPdfDoc.GetNumPages()

            For i As Integer = 0 To pPageCount - 1
                pPdfPage = DirectCast(pPdfDoc.AcquirePage(i), Acrobat.CAcroPDPage)

                pPdfPoint = DirectCast(pPdfPage.GetSize(), Acrobat.AcroPoint)
                pPdfRect.Left = 0
                pPdfRect.right = pPdfPoint.x
                pPdfRect.Top = 0
                pPdfRect.bottom = pPdfPoint.y

                pPdfPage.CopyToClipboard(pPdfRect, 0, 0, 100)

                Dim pImgFile As String = ""
                Dim pFilename As String = SourceFileName_In.Substring(SourceFileName_In.LastIndexOf("\"))

                If pPageCount = 1 Then
                    pImgFile = DestinationPath_In & "\" & pFilename & "." & ImageFormat.Jpeg.ToString()
                Else
                    pImgFile = DestinationPath_In & "\" & pFilename & "_" & i.ToString() & "." & ImageFormat.Jpeg.ToString()
                End If

                Clipboard.GetImage().Save(pImgFile, ImageFormat.Jpeg)

                pPara.Range.Paste()
                pPara.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter
                Clipboard.Clear()

                If (File.Exists(pImgFile)) Then
                    File.Delete(pImgFile)
                End If

            Next

            Dispose(pPdfDoc, pPdfPage, pPdfPoint, pPdfRect)
        Else
            Dispose(pPdfDoc, pPdfPage, pPdfPoint, pPdfRect)

            Throw New System.IO.FileNotFoundException(SourceFileName_In & " Not Found!")
        End If

    End Sub


    Public Sub OpenPowerPoint(ByVal FileName_In As String)
        '===================================================        'AES 08NOV16
        Dim pApp As New Microsoft.Office.Interop.PowerPoint.Application()
        Dim pPresentations As Microsoft.Office.Interop.PowerPoint.Presentations = pApp.Presentations

        Dim pCurPresentation As Microsoft.Office.Interop.PowerPoint.Presentation = pPresentations.Open(FileName_In, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue)

        Dim pSSWs As Microsoft.Office.Interop.PowerPoint.SlideShowWindows
        Dim pSSS As Microsoft.Office.Interop.PowerPoint.SlideShowSettings

        '....Run the Slide show
        pSSS = pCurPresentation.SlideShowSettings
        pSSS.Run()
        pSSWs = pApp.SlideShowWindows
        While pSSWs.Count >= 1
            System.Threading.Thread.Sleep(100)
        End While

    End Sub

    Public Sub Dispose(ByVal PdfDoc_In As Acrobat.CAcroPDDoc, ByVal PdfPage_In As Acrobat.CAcroPDPage,
                       ByVal PdfPoint_In As Acrobat.AcroPoint, ByVal PdfRect_In As Acrobat.CAcroRect)
        '=============================================================================================
        GC.Collect()
        If PdfPage_In IsNot Nothing Then
            Marshal.ReleaseComObject(PdfPage_In)
        End If
        Marshal.ReleaseComObject(PdfPoint_In)
        Marshal.ReleaseComObject(PdfRect_In)
        Marshal.ReleaseComObject(PdfDoc_In)

    End Sub

#End Region


#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sLeak
        Public Exists As Boolean
        Public Leakage As Boolean
        Public LeakagePlate As Boolean
        Public Springback As Boolean
    End Structure

    <Serializable()> _
    Public Structure sLoad
        Public Exists As Boolean
        Public Springback As Boolean
    End Structure

    <Serializable()> _
    Public Structure sUser
        Public Name As String
        Public Signed As Boolean
        Public DateSigned As DateTime
    End Structure

    <Serializable()> _
    Public Structure sGen
        Public Image As List(Of sGenImage)
        Public Seal As List(Of sGenSeal)
    End Structure

    <Serializable()> _
    Public Structure sGenImage
        Public ID As Integer
        Public File As String
        Public NameTag As String
        Public Caption As String
        Public Image As Image
        Public Selected As Boolean
    End Structure

    <Serializable()> _
    Public Structure sGenSeal
        Public SeqID As Integer
        Public SN As String
    End Structure


    <Serializable()> _
    Public Structure sLeak_Equip
        Public StandName As String
        Public Fixture As String
        Public T As Double
        Public ShimActual As Double
        Public ShimDescrip As String
        Public Platen_SF As Integer

        Public TestMeterMake As String
        Public TestMeterSN As String
        Public TestMeterRange As String
        Public TestMeterModelNo As String
        Public TestMeterDateCalibrationDue As DateTime
    End Structure


    <Serializable()> _
    Public Structure sLoad_Equip
        Public StandName As String
        Public StandSN As String
        Public StandDateCalibrationDue As DateTime

        Public LoadCellMake As String
        Public LoadCellSN As String
        Public LoadCellRange As String
        Public LoadCellModelNo As String
        Public LoadCellDateCalibrationDue As DateTime
    End Structure

#End Region


#Region "CONSTRUCTOR:"

    Public Sub New()
        '===========
        mGen.Seal = New List(Of sGenSeal)
        mLeakEquip.T = 70.0
    End Sub

#End Region


End Class
