'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsAnalysis                            '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Linq
Imports System.IO
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports clsLibrary11
Imports System.Windows.Forms

<Serializable()> _
Public Class IPE_clsAnalysis
    Implements ICloneable

    Public Const mcEPS As Single = 0.00001          '....An aribitrarily small number. 

#Region "ENUMERATION TYPES:"

    Enum eState
        Complete
        Incomplete
    End Enum

    Enum eLoadType
        Baseline
        Additional
    End Enum

    Enum eMatModel
        ElastoPlastic
        LinearElastic
    End Enum


#End Region


#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sCompression
        Public TolType As String        '...."Nominal", "Maximum" or "Minimum"  '(FUNDAMENTAL)
        Public Val As Single            '....Installation Compression (in)      '(DERIEVED)
        Public PcentVal As Single       '....Installation Compression (%)       '(DERIEVED)
        Public PcentValMin As Single    '....Installation Compression (%) - Min '(DERIEVED)
    End Structure

    <Serializable()> _
    Public Structure sLoadCase
        Public Type As eLoadType
        Public CompressionTolType As String        '...."Nominal", "Maximum" or "Minimum"
        Public Name As String
    End Structure

    <Serializable()> _
    Public Structure sLoadStep
        Public PDiff As String
        Public T As Single
        Public CavityDepth As Single
        Public CompressionVal As Single
        Public Descrip As String
    End Structure

    <Serializable()> _
    Public Structure sResult_Gen
        Public SolnConv As Single
        Public PeneMax1 As Single
        Public HFreeFinal As Single
        Public Leakage_BL As Single
    End Structure

    <Serializable()> _
    Public Structure sResult_LoadStep
        Public FContact As Single
        Public SigEqvMax As Single
    End Structure

#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"
    '==================================
    Protected mProject As IPE_clsProject

    <NonSerialized()>
    Private mID As Int32

    Private mCavity As IPE_clsCavity        'AES 11APR16
    Private mOpCond As New IPE_clsOpCond
    Private mAppLoad As New IPE_clsAppLoad
    Private mSeal As IPE_clsSeal
    Private mANSYS As IPE_clsANSYS

    Private mCompression As sCompression

    Private mMatModel As eMatModel  'AES 10MAR17

    Private mLoadCase As sLoadCase

    Private mFatigueData As Boolean = False    'AES 30AUG16

    <NonSerialized()>
    Private mResult_Gen As sResult_Gen

    <NonSerialized()>
    Private mResult_LoadStep As New List(Of sResult_LoadStep)

    <NonSerialized()>
    Private mDateCreated As DateTime

    <NonSerialized()>
    Private mTimeCreated As DateTime

#End Region


#Region "PROPERTY ROUTINES:"
    '======================

    '....ID
    Public Property ID() As Int32
        '========================
        Get
            Return mID
        End Get

        Set(ByVal sngData As Int32)
            '-------------------------------
            mID = sngData
        End Set

    End Property


    '....Cavity
    Public Property Cavity() As IPE_clsCavity
        '================================
        Get
            Return mCavity
        End Get

        Set(Obj As IPE_clsCavity)
            mCavity = Obj
        End Set
    End Property


    '....OpCond
    Public Property OpCond() As IPE_clsOpCond
        '================================
        Get
            Return mOpCond
        End Get

        Set(Obj As IPE_clsOpCond)
            mOpCond = Obj
        End Set
    End Property


    '....AppLoad
    Public Property AppLoad() As IPE_clsAppLoad
        '==================================
        Get
            Return mAppLoad
        End Get

        Set(Obj As IPE_clsAppLoad)
            mAppLoad = Obj
        End Set
    End Property


    '....Seal
    Public Property Seal() As IPE_clsSeal
        '============================
        Get
            Return mSeal
        End Get

        Set(Obj As IPE_clsSeal)
            mSeal = Obj
        End Set

    End Property

    '....ANSYS
    Public Property ANSYS() As IPE_clsANSYS
        '================================
        Get
            Return mANSYS
        End Get

        Set(Obj As IPE_clsANSYS)
            mANSYS = Obj
        End Set

    End Property

    '....Compression.
    Public ReadOnly Property Compression() As sCompression
        '=================================================
        Get
            Return mCompression
        End Get
    End Property

    '....MatModel
    Public Property MatModel() As eMatModel
        '===================================
        Get
            Return mMatModel
        End Get

        Set(ByVal sngData As eMatModel)
            '-------------------------------
            mMatModel = sngData
        End Set

    End Property


    '....LoadCase
    Public ReadOnly Property LoadCase() As sLoadCase
        '=================================================
        Get
            Return mLoadCase
        End Get
    End Property


    '....LoadType
    Public WriteOnly Property LoadType() As eLoadType
        '============================================ 
        Set(ByVal Value As eLoadType)
            mLoadCase.Type = Value
        End Set
    End Property


    '....CompressionTolType
    Public WriteOnly Property CompressionTolType() As String
        '=====================================================      
        Set(ByVal strValue As String)
            mLoadCase.CompressionTolType = strValue
            mCompression.TolType = strValue
            Calc_Compression()
        End Set
    End Property


    '....Name
    Public WriteOnly Property LoadCaseName() As String
        '=============================================
        Set(ByVal Value As String)
            mLoadCase.Name = Value
        End Set
    End Property


    '....Fatigue Data
    Public Property FatigueData() As Boolean
        '======================================
        Get
            Return mFatigueData
        End Get

        Set(value As Boolean)
            mFatigueData = value
        End Set

    End Property


    '....Result
    Public Property Result_Gen() As sResult_Gen
        '======================================
        Get
            Return mResult_Gen
        End Get

        Set(value As sResult_Gen)
            mResult_Gen = value
        End Set

    End Property


    '....ResultLoadStep
    Public Property Result_LoadStep() As List(Of sResult_LoadStep)
        '=========================================================
        Get
            Return mResult_LoadStep
        End Get

        Set(ByVal Data As List(Of sResult_LoadStep))
            mResult_LoadStep = Data
        End Set

    End Property


    '....DateCreated
    Public Property DateCreated() As DateTime
        '============================
        Get
            Return mDateCreated
        End Get

        Set(value As DateTime)
            mDateCreated = value
        End Set

    End Property


    '....TimeCreated
    Public Property TimeCreated() As DateTime
        '=====================================
        Get
            Return mTimeCreated
        End Get

        Set(value As DateTime)
            mTimeCreated = value
        End Set

    End Property

#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(ByVal Project_In As IPE_clsProject)
        '=========================================
        mProject = Project_In
        'mCavity = New clsCavity(Project_In.SealType)         'AES 15APR16
        mCavity = New IPE_clsCavity(Project_In.SealType, Project_In.UnitSystem())         'AES 15APR16
        mOpCond = New IPE_clsOpCond(Project_In.UnitSystem())
        mAppLoad = New IPE_clsAppLoad()
        mANSYS = New IPE_clsANSYS(gIPE_File.DirWorkANSYS)           'AES 15SEP16

        If (Project_In.SealType = "E-Seal") Then
            mSeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, mOpCond.POrient)

        ElseIf (Project_In.SealType = "C-Seal") Then
            mSeal = New IPE_clsCSeal("C-Seal", gIPE_Unit.System, mOpCond.POrient)

        ElseIf (Project_In.SealType = "U-Seal") Then
            mSeal = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, mOpCond.POrient)

        End If

        mLoadCase.CompressionTolType = "Nominal"
        mCompression.TolType = mLoadCase.CompressionTolType

    End Sub


    Public Sub New(ByVal SealType_In As String, ByVal UnitSystem_In As String)
        '====================================================================
        mCavity = New IPE_clsCavity(SealType_In, UnitSystem_In)
        mOpCond = New IPE_clsOpCond(UnitSystem_In)
        mAppLoad = New IPE_clsAppLoad()
        mANSYS = New IPE_clsANSYS(gIPE_File.DirWorkANSYS)

        If (SealType_In = "E-Seal") Then
            mSeal = New IPE_clsESeal("E-Seal", gIPE_Unit.System, mOpCond.POrient)

        ElseIf (SealType_In = "C-Seal") Then
            mSeal = New IPE_clsCSeal("C-Seal", gIPE_Unit.System, mOpCond.POrient)

        ElseIf (SealType_In = "U-Seal") Then
            mSeal = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, mOpCond.POrient)

        End If

        mLoadCase.CompressionTolType = "Nominal"
        mCompression.TolType = mLoadCase.CompressionTolType
    End Sub

#End Region


#Region "CLASS METHODS:"

    Public Sub Reset_Result()
        '===================
        mResult_Gen.SolnConv = 0.0
        mResult_Gen.PeneMax1 = 0.0
        mResult_Gen.HFreeFinal = 0.0

        mResult_LoadStep.Clear()
    End Sub


    Public Sub Retrieve_FromDB(ByVal Unit_In As IPE_clsUnit, ByRef ANSYS_Out As IPE_clsANSYS)
        '============================================================================
        Dim pSealEntities As New SealIPEDBEntities

        Dim pCustomerID As Integer, pProjectID As Integer, pPlatformID As Integer, pAnalysisID As Integer

        pCustomerID = mProject.Customer_ID
        pPlatformID = mProject.Platform_ID
        pProjectID = mProject.Project_ID
        pAnalysisID = mID

        Dim pQryAnalysis = (From pRec In pSealEntities.tblAnalysis
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldID = pAnalysisID Select pRec).First()

        'Analysis: 
        '---------
        mLoadCase.Name = pQryAnalysis.fldLoadCaseName

        If Not IsNothing(pQryAnalysis.fldMatModel) Then
            Dim pMatModel As String = pQryAnalysis.fldMatModel
            If (pMatModel = eMatModel.ElastoPlastic.ToString()) Then
                mMatModel = eMatModel.ElastoPlastic

            ElseIf (mMatModel = eMatModel.LinearElastic.ToString()) Then
                mMatModel = eMatModel.LinearElastic
            End If
        End If


        If Not IsNothing(pQryAnalysis.fldDate) Then
            Dim pVal() As String = pQryAnalysis.fldDate.ToString().Split(" ")
            mDateCreated = pVal(0)
            mTimeCreated = pVal(1)
        End If

        '....Cavity
        Dim pQryCavity = (From pRec In pSealEntities.tblCavity
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldID = pAnalysisID Select pRec).ToList()

        If (pQryCavity.Count() > 0) Then

            With mCavity

                Dim pCavityDia1, pCavityDia2, pCavityDepth As Double

                pCavityDia1 = pQryCavity(0).fldCavityDia1
                pCavityDia2 = pQryCavity(0).fldCavityDia2
                .Dia(1) = Unit_In.L_UserToCon(pCavityDia1)
                .Dia(2) = Unit_In.L_UserToCon(pCavityDia2)

                pCavityDepth = Unit_In.L_UserToCon(ConvertToSng(pQryCavity(0).fldCavityDepth))

                .DepthTol(1) = Unit_In.L_UserToCon(ConvertToSng(pQryCavity(0).fldCavityDepthTol1))
                .DepthTol(2) = Unit_In.L_UserToCon(ConvertToSng(pQryCavity(0).fldCavityDepthTol2))

                .Depth = pCavityDepth
            End With
        End If

        '....OpCond
        Dim pQryOpCond = (From pRec In pSealEntities.tblOpCond
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldID = pAnalysisID Select pRec).ToList()

        If (pQryOpCond.Count() > 0) Then

            With mOpCond
                .PDiff = pQryOpCond(0).fldPDiff * Unit_In.CFacUserP
                .TOper = pQryOpCond(0).fldTOper
                .POrient = pQryOpCond(0).fldPOrient.Trim()
            End With
        End If

        '....App Load
        Dim pQryAppLoad = (From pRec In pSealEntities.tblAppLoad
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldID = pAnalysisID Select pRec).ToList()

        If (pQryAppLoad.Count() > 0) Then
            With mAppLoad

                .PreComp_Exits = pQryAppLoad(0).fldPreCompressed
                If (.PreComp.Exists) Then
                    Dim pHMin As Double = pQryAppLoad(0).fldPreCompressed_HMin
                    .PreComp_HMin = Unit_In.L_UserToCon(pHMin)
                End If

                .RadConstraint = pQryAppLoad(0).fldRadConstraint

                Dim pQryAddLoad = (From it In pSealEntities.tblAddLoad
                                            Where it.fldProjectID = pProjectID And
                                                  it.fldAppLoadID = pAnalysisID
                                            Order By it.fldStep Ascending Select it).ToList()


                Dim pRecord As New tblAddLoad
                Dim pAddLoad As New IPE_clsAppLoad.sAddLoad

                For Each pRecord In pQryAddLoad
                    pAddLoad = New IPE_clsAppLoad.sAddLoad
                    pAddLoad.PDiff = pRecord.fldPDiff * Unit_In.CFacUserP
                    pAddLoad.TOper = pRecord.fldTOper
                    pAddLoad.CavityDepth = Unit_In.L_UserToCon(ConvertToSng(pRecord.fldCavityDepth))

                    .AddLoad.Add(pAddLoad)
                Next
            End With
        End If

        '....Seal
        Dim pQrySeal = (From pRec In pSealEntities.tblSeal
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldID = pAnalysisID Select pRec).ToList()

        If (pQrySeal.Count() > 0) Then
            Dim pMCS As String = pQrySeal(0).fldMCS
            Dim pSealType As String = mProject.SealType()

            If (pSealType = "E-Seal") Then
                '-------------------------
                'Instantiate ESeal object
                mSeal = New IPE_clsESeal("E-Seal", Unit_In.System, mOpCond.POrient)
                mSeal.MCrossSecNo = pMCS
                mCavity.CornerRad = mSeal.CavityCornerRad
                '....Secondary Assignments:
                '
                '....Assign cavity diameters. 
                Dim i As Int16
                For i = 1 To 2
                    mSeal.CavityDia(i) = mCavity.Dia(i)
                Next

                If (Not IsNothing(pQrySeal(0).fldSFinish)) Then
                    Dim pSFinish As Double = pQrySeal(0).fldSFinish
                    CType(mSeal, IPE_clsESeal).SFinish = Unit_In.L_UserToCon(pSFinish)
                Else
                    CType(mSeal, IPE_clsESeal).SFinish = 0.0#
                End If

                Dim pQryAdjESeal = (From it In pSealEntities.tblAdjESeal
                                   Where it.fldProjectID = pProjectID And
                                      it.fldSealID = pAnalysisID Select it).ToList()

                If (pQryAdjESeal.Count() > 0) Then
                    If (Not IsNothing(pQryAdjESeal(0).fldDThetaE1)) Then
                        CType(mSeal, IPE_clsESeal).DThetaE1 = pQryAdjESeal(0).fldDThetaE1
                    Else
                        CType(mSeal, IPE_clsESeal).DThetaE1 = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjESeal(0).fldDThetaM1)) Then
                        CType(mSeal, IPE_clsESeal).DThetaM1 = pQryAdjESeal(0).fldDThetaM1
                    Else
                        CType(mSeal, IPE_clsESeal).DThetaM1 = 0.0#
                    End If

                End If


            ElseIf (pSealType = "C-Seal") Then
                '-----------------------------
                'Instantiate CSeal object
                mSeal = New IPE_clsCSeal("C-Seal", Unit_In.System, mOpCond.POrient)
                mSeal.MCrossSecNo = pMCS
                mCavity.CornerRad = mSeal.CavityCornerRad

                CType(mSeal, IPE_clsCSeal).PlatingCode = pQrySeal(0).fldPlating
                CType(mSeal, IPE_clsCSeal).PlatingThickCode = pQrySeal(0).fldPlatingThickCode

                '....Secondary Assignments:
                '
                '....Assign cavity diameters to the gSealSel object members. 
                Dim i As Int16
                For i = 1 To 2
                    mSeal.CavityDia(i) = mCavity.Dia(i)
                Next

                Dim pQryAdjCSeal = (From it In pSealEntities.tblAdjCSeal
                                 Where it.fldProjectID = pProjectID And
                                    it.fldSealID = pAnalysisID Select it).ToList()

                If (pQryAdjCSeal.Count() > 0) Then

                    If (Not IsNothing(pQryAdjCSeal(0).fldDHFree)) Then
                        CType(mSeal, IPE_clsCSeal).DHfree = pQryAdjCSeal(0).fldDHFree
                    Else
                        CType(mSeal, IPE_clsCSeal).DHfree = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjCSeal(0).fldDThetaOpening)) Then
                        Dim pDThetaOpening As Double = pQryAdjCSeal(0).fldDThetaOpening
                        CType(mSeal, IPE_clsCSeal).DThetaOpening = Unit_In.L_UserToCon(pDThetaOpening)
                    Else
                        CType(mSeal, IPE_clsCSeal).DThetaOpening = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjCSeal(0).fldDT)) Then
                        Dim pDT As Double = pQryAdjCSeal(0).fldDT
                        CType(mSeal, IPE_clsCSeal).T = Unit_In.L_UserToCon(pDT)
                    Else
                        CType(mSeal, IPE_clsCSeal).T = 0.0#
                    End If

                End If

            ElseIf (pSealType = "U-Seal") Then
                '-----------------------------
                'Instantiate USeal object
                mSeal = New IPE_clsUSeal("U-Seal", Unit_In.System, mOpCond.POrient)
                mSeal.MCrossSecNo = pMCS

                '....Secondary Assignments:
                '
                '....Assign cavity diameters to the gSealSel object members. 
                Dim i As Int16
                For i = 1 To 2
                    mSeal.CavityDia(i) = mCavity.Dia(i)
                Next

                Dim pQryAdjUSeal = (From it In pSealEntities.tblAdjUSeal
                                  Where it.fldProjectID = pProjectID And
                                     it.fldSealID = pAnalysisID Select it).ToList()

                If (pQryAdjUSeal.Count() > 0) Then

                    If (Not IsNothing(pQryAdjUSeal(0).fldDTheta1)) Then
                        Dim pDTheta1 As Double = pQryAdjUSeal(0).fldDTheta1
                        CType(mSeal, IPE_clsUSeal).DTheta(1) = Unit_In.L_UserToCon(pDTheta1)
                    Else
                        CType(mSeal, IPE_clsUSeal).DTheta(1) = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjUSeal(0).fldDTheta2)) Then
                        Dim pDTheta2 As Double = pQryAdjUSeal(0).fldDTheta2
                        CType(mSeal, IPE_clsUSeal).DTheta(2) = Unit_In.L_UserToCon(pDTheta2)
                    Else
                        CType(mSeal, IPE_clsUSeal).DTheta(2) = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjUSeal(0).fldDRad1)) Then
                        Dim pDRad1 As Double = pQryAdjUSeal(0).fldDRad1
                        CType(mSeal, IPE_clsUSeal).DRad(1) = Unit_In.L_UserToCon(pDRad1)
                    Else
                        CType(mSeal, IPE_clsUSeal).DRad(1) = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjUSeal(0).fldDRad2)) Then
                        Dim pDRad2 As Double = pQryAdjUSeal(0).fldDRad2
                        CType(mSeal, IPE_clsUSeal).DRad(2) = Unit_In.L_UserToCon(pDRad2)
                    Else
                        CType(mSeal, IPE_clsUSeal).DRad(2) = 0.0#
                    End If

                    If (Not IsNothing(pQryAdjUSeal(0).fldDLLeg)) Then
                        Dim pDLLeg As Double = pQryAdjUSeal(0).fldDLLeg
                        CType(mSeal, IPE_clsUSeal).DLLeg = Unit_In.L_UserToCon(pDLLeg)
                    Else
                        CType(mSeal, IPE_clsUSeal).DLLeg = 0.0#
                    End If

                End If

            End If

            With mSeal
                .IsSegmented = pQrySeal(0).fldSegmented

                If (.IsSegmented) Then
                    .CountSegment = pQrySeal(0).fldSegmentCount
                Else
                    .CountSegment = 0
                End If

                Dim pblnVal As Boolean = pQrySeal(0).fldSelected

                If (pblnVal) Then
                    .Selected = True
                Else
                    .Selected = False
                End If
                Dim pZClear, pHFreeTol1, pHFreeTol2 As Double
                pZClear = pQrySeal(0).fldZClear
                pHFreeTol1 = pQrySeal(0).fldHFreeTol1
                pHFreeTol2 = pQrySeal(0).fldHFreeTol2

                .ZClear_Given = Unit_In.L_UserToCon(pZClear)
                .HFreeTol(1) = Unit_In.L_UserToCon(pHFreeTol1)
                .HFreeTol(2) = Unit_In.L_UserToCon(pHFreeTol2)
            End With

            'Material:
            '---------
            With mSeal.Mat
                .Name = pQrySeal(0).fldMatName
                .HT = pQrySeal(0).fldHeatTreatment
                .TOper = mOpCond.TOper

                If (mSeal.Type = "E-Seal") Then
                    If (IsNothing(pQrySeal(0).fldCoating)) Then
                        .Coating = "None"
                    Else
                        .Coating = pQrySeal(0).fldCoating
                    End If
                End If

            End With
        End If

        '....Load Case ID
        Dim pLoadCaseID As Integer = pQryAnalysis.fldLoadCase_GenID

        Dim pQryLoadCase_Gen = (From it In pSealEntities.tblLoadCase_Gen
                                     Where it.fldID = pLoadCaseID Select it).ToList()

        If (pQryLoadCase_Gen.Count() > 0) Then
            Dim pstrVal As String = pQryLoadCase_Gen(0).fldLoadType.ToString().Trim()

            If (pstrVal = eLoadType.Baseline.ToString()) Then
                mLoadCase.Type = eLoadType.Baseline

            Else
                mLoadCase.Type = eLoadType.Additional
            End If

            'Compression: 
            '------------
            CompressionTolType = pQryLoadCase_Gen(0).fldCompressTolType.ToString()
        End If

        '....Result Gen
        Dim pQryResultGen = (From pRec In pSealEntities.tblResult_Gen
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldAnalysisID = pAnalysisID Select pRec).ToList()

        If (pQryResultGen.Count() > 0) Then

            Dim pPeneMax1, pHFreeFinal, pLeakage_BL, pDSealing As Double
            With mResult_Gen
                .SolnConv = pQryResultGen(0).fldSolnConv
                pPeneMax1 = pQryResultGen(0).fldPeneMax1
                pHFreeFinal = pQryResultGen(0).fldHFreeFinal
                pLeakage_BL = pQryResultGen(0).fldLeakage_BL

                .PeneMax1 = Unit_In.L_UserToCon(pPeneMax1)
                .HFreeFinal = Unit_In.L_UserToCon(pHFreeFinal)
                If (Not IsNothing(pQryResultGen(0).fldLeakage_BL)) Then
                    .Leakage_BL = Unit_In.L_UserToCon(pLeakage_BL)
                End If

            End With

            With mSeal
                pPeneMax1 = pQryResultGen(0).fldPeneMax1
                pDSealing = pQryResultGen(0).fldDSealing
                pHFreeFinal = pQryResultGen(0).fldHFreeFinal

                .PeneMax1 = Unit_In.L_UserToCon(pPeneMax1)
                .DSealing = Unit_In.L_UserToCon(pDSealing)
                .HfreeFinal = Unit_In.L_UserToCon(pHFreeFinal)

            End With
        End If

        '....Result Load Step
        Dim pQryResultLoadStep = (From it In pSealEntities.tblResult_LoadStep
                                           Where it.fldProjectID = pProjectID And
                                                 it.fldAnalysisID = pAnalysisID
                                           Order By it.fldStepID Ascending Select it).ToList()

        Dim pRec_ResultLoadStep As New tblResult_LoadStep

        Dim pIndex As Integer = 0
        mResult_LoadStep = New List(Of sResult_LoadStep)
        For Each pRec_ResultLoadStep In pQryResultLoadStep

            If (Not IsNothing(pRec_ResultLoadStep.fldFSeat_Unit)) Then
                mSeal.FSeat_Unit(pIndex) = pRec_ResultLoadStep.fldFSeat_Unit / Unit_In.CFacUserL
            Else
                Seal.FSeat_Unit(pIndex) = 0.0#
            End If

            If (Not IsNothing(pRec_ResultLoadStep.fldSigEqvMax)) Then
                mSeal.StressMax(pIndex) = pRec_ResultLoadStep.fldSigEqvMax / Unit_In.CFacUserL
            Else
                Seal.StressMax(pIndex) = 0.0#
            End If

            Dim pResultLoadStep As New sResult_LoadStep
            pResultLoadStep.FContact = mSeal.FSeat_Unit(pIndex) / Unit_In.CFacUserL
            pResultLoadStep.SigEqvMax = Unit_In.Stress_UserToCon(mSeal.StressMax(pIndex))
            mResult_LoadStep.Add(pResultLoadStep)

            pIndex = pIndex + 1
        Next

        'AES 15SEP16
        '....FEA Params
        Dim pQryFEAParam = (From pRec In pSealEntities.tblFEAParam
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldAnalysisID = pAnalysisID Select pRec).ToList()

        If (pQryFEAParam.Count() > 0) Then

            With ANSYS_Out
                .Version = pQryFEAParam(0).fldANSYS_Ver
                .NelMax = pQryFEAParam(0).fldMaxElement
                mSeal.NelMax = pQryFEAParam(0).fldMaxElement

                If (mSeal.Type = "E-Seal") Then
                    CType(mSeal, IPE_clsESeal).NLayer = pQryFEAParam(0).fldNLayer

                    CType(mSeal, IPE_clsESeal).NelConSeal = pQryFEAParam(0).fldNelConSeal
                    CType(mSeal, IPE_clsESeal).NelConMid = pQryFEAParam(0).fldNelConMid
                    CType(mSeal, IPE_clsESeal).NelConHeel = pQryFEAParam(0).fldNelConHeel
                    CType(mSeal, IPE_clsESeal).NelConRad = pQryFEAParam(0).fldNelConRad

                    CType(mSeal, IPE_clsESeal).FacKN = pQryFEAParam(0).fldFacKN
                    CType(mSeal, IPE_clsESeal).FacTOLN = pQryFEAParam(0).fldFacTOLN

                    CType(mSeal, IPE_clsESeal).NSBSTP = pQryFEAParam(0).fldNSBSTP
                    CType(mSeal, IPE_clsESeal).NSBMX = pQryFEAParam(0).fldNSBMX
                    CType(mSeal, IPE_clsESeal).NSBMN = pQryFEAParam(0).fldNSBMN

                ElseIf (mSeal.Type = "C-Seal") Then

                    CType(mSeal, IPE_clsCSeal).NLayer = pQryFEAParam(0).fldNLayer

                    If (CType(mSeal, IPE_clsCSeal).Plating.Exists) Then
                        CType(mSeal, IPE_clsCSeal).PlatingNLayer = pQryFEAParam(0).fldNLayer_Plating
                    End If

                    CType(mSeal, IPE_clsCSeal).NelConSeal = pQryFEAParam(0).fldNelConSeal
                    CType(mSeal, IPE_clsCSeal).NelConRad = pQryFEAParam(0).fldNelConRad

                    CType(mSeal, IPE_clsCSeal).FacKN = pQryFEAParam(0).fldFacKN
                    CType(mSeal, IPE_clsCSeal).FacTOLN = pQryFEAParam(0).fldFacTOLN

                    CType(mSeal, IPE_clsCSeal).NSBSTP = pQryFEAParam(0).fldNSBSTP
                    CType(mSeal, IPE_clsCSeal).NSBMX = pQryFEAParam(0).fldNSBMX
                    CType(mSeal, IPE_clsCSeal).NSBMN = pQryFEAParam(0).fldNSBMN

                ElseIf (mSeal.Type = "U-Seal") Then

                    CType(mSeal, IPE_clsUSeal).NLayer = pQryFEAParam(0).fldNLayer

                    CType(mSeal, IPE_clsUSeal).NelConSeal = pQryFEAParam(0).fldNelConSeal
                    CType(mSeal, IPE_clsUSeal).NelConHeel = pQryFEAParam(0).fldNelConHeel
                    CType(mSeal, IPE_clsUSeal).NelConRad = pQryFEAParam(0).fldNelConRad

                    CType(mSeal, IPE_clsUSeal).FacKN = pQryFEAParam(0).fldFacKN
                    CType(mSeal, IPE_clsUSeal).FacTOLN = pQryFEAParam(0).fldFacTOLN

                    CType(mSeal, IPE_clsUSeal).NSBSTP = pQryFEAParam(0).fldNSBSTP
                    CType(mSeal, IPE_clsUSeal).NSBMX = pQryFEAParam(0).fldNSBMX
                    CType(mSeal, IPE_clsUSeal).NSBMN = pQryFEAParam(0).fldNSBMN
                End If

            End With

        End If


    End Sub


    Public Sub Save_ToDB(ByVal Unit_In As IPE_clsUnit, ByVal ANSYS_In As IPE_clsANSYS)
        '=====================================================================
        Dim pIPEDBEntities As New SealIPEDBEntities
        Dim pCustomerID As Integer, pProjectID As Integer, pPlatformID As Integer, pAnalysisID As Integer

        pCustomerID = mProject.Customer_ID
        pPlatformID = mProject.Platform_ID
        pProjectID = mProject.Project_ID
        pAnalysisID = mID

        Try

            Dim pQryAnalysis = (From pRec In pIPEDBEntities.tblAnalysis
                                Where pRec.fldProjectID = pProjectID And
                                      pRec.fldID = pAnalysisID Select pRec).ToList()

            Dim pAnalysis As New tblAnalysis

            If (pQryAnalysis.Count() > 0) Then
                pAnalysis = pQryAnalysis(0)
            End If

            With pAnalysis
                .fldProjectID = mProject.Project_ID
                .fldID = pAnalysisID
                .fldCavityID = pAnalysisID
                .fldOpCondID = pAnalysisID
                .fldAppLoadID = pAnalysisID
                .fldSealID = pAnalysisID
                .fldDate = mDateCreated
                .fldUser = " " 'gIPE_User.Name
                .fldLoadCaseName = mLoadCase.Name

                If (mMatModel = eMatModel.ElastoPlastic) Then
                    .fldMatModel = eMatModel.ElastoPlastic.ToString()

                ElseIf (mMatModel = eMatModel.LinearElastic) Then
                    .fldMatModel = eMatModel.LinearElastic.ToString()
                End If

                Dim pLoadCaseGen_ID As Integer = 1
                Dim pLoadCaseType As String = mLoadCase.Type.ToString()
                Dim pQryLoadCaseGen = (From it In pIPEDBEntities.tblLoadCase_Gen
                                       Where it.fldLoadType = pLoadCaseType And
                                             it.fldCompressTolType = mLoadCase.CompressionTolType Select it).ToList()

                If (pQryLoadCaseGen.Count() > 0) Then
                    pLoadCaseGen_ID = pQryLoadCaseGen(0).fldID
                End If

                .fldLoadCase_GenID = pLoadCaseGen_ID

                If (pQryAnalysis.Count() > 0) Then
                    pIPEDBEntities.SaveChanges()
                Else
                    pIPEDBEntities.AddTotblAnalysis(pAnalysis)
                    pIPEDBEntities.SaveChanges()
                End If

            End With

            '....Cavity
            Dim pQryCavity = (From pRec In pIPEDBEntities.tblCavity
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldID = pAnalysisID Select pRec).ToList()

            Dim pCavity As New tblCavity

            If (pQryCavity.Count() > 0) Then
                pCavity = pQryCavity(0)
            End If

            With pCavity
                .fldProjectID = mProject.Project_ID
                .fldID = pAnalysisID
                .fldCavityDia1 = Unit_In.L_ConToUser(mCavity.Dia(1))
                .fldCavityDia2 = Unit_In.L_ConToUser(mCavity.Dia(2))

                .fldCavityDepth = Unit_In.L_ConToUser(mCavity.Depth)

                .fldCavityDepthTol1 = Unit_In.L_ConToUser(mCavity.DepthTol(1))
                .fldCavityDepthTol2 = Unit_In.L_ConToUser(mCavity.DepthTol(2))

                If (pQryCavity.Count() > 0) Then
                    pIPEDBEntities.SaveChanges()
                Else
                    pIPEDBEntities.AddTotblCavity(pCavity)
                    pIPEDBEntities.SaveChanges()
                End If

            End With

            '....OpCond
            Dim pQryOpCond = (From pRec In pIPEDBEntities.tblOpCond
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldID = pAnalysisID Select pRec).ToList()

            Dim pOpCond As New tblOpCond

            If (pQryOpCond.Count() > 0) Then
                pOpCond = pQryOpCond(0)
            End If

            With pOpCond
                .fldProjectID = mProject.Project_ID
                .fldID = pAnalysisID
                .fldUnitUserP = gIPE_Unit.UserP()
                .fldPDiff = mOpCond.PDiff / Unit_In.CFacUserP()     '....User Unit.
                .fldTOper = mOpCond.TOper
                .fldPOrient = mOpCond.POrient

                If (pQryOpCond.Count() > 0) Then
                    pIPEDBEntities.SaveChanges()
                Else
                    pIPEDBEntities.AddTotblOpCond(pOpCond)
                    pIPEDBEntities.SaveChanges()
                End If

            End With

            '....AppLoad
            Dim pQryAppLoad = (From pRec In pIPEDBEntities.tblAppLoad
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldID = pAnalysisID Select pRec).ToList()

            Dim pAppLoad As New tblAppLoad

            If (pQryAppLoad.Count() > 0) Then
                pAppLoad = pQryAppLoad(0)
            End If

            With pAppLoad
                .fldProjectID = mProject.Project_ID
                .fldID = pAnalysisID
                .fldPreCompressed = mAppLoad.PreComp.Exists
                .fldPreCompressed_HMin = Unit_In.L_ConToUser(mAppLoad.PreComp.HMin)
                .fldRadConstraint = mAppLoad.RadConstraint

                If (pQryAppLoad.Count() > 0) Then
                    pIPEDBEntities.SaveChanges()
                Else
                    pIPEDBEntities.AddTotblAppLoad(pAppLoad)
                    pIPEDBEntities.SaveChanges()
                End If

            End With

            '....AddLoad
            Dim pQryAddLoad = (From pRec In pIPEDBEntities.tblAddLoad
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldAppLoadID = pAnalysisID Select pRec).ToList()

            If (pQryAddLoad.Count() > 0) Then
                For j As Integer = 0 To pQryAddLoad.Count() - 1
                    pIPEDBEntities.DeleteObject(pQryAddLoad(j))
                    pIPEDBEntities.SaveChanges()
                Next
            End If


            Dim pAddLoad As New List(Of tblAddLoad)
            For j As Integer = 0 To mAppLoad.AddLoad.Count - 1
                Dim pLoad As New tblAddLoad
                pAddLoad.Add(pLoad)
                With pAddLoad(j)

                    .fldProjectID = mProject.Project_ID
                    .fldAppLoadID = pAnalysisID
                    .fldStep = (j + 1)
                    .fldPDiff = mAppLoad.AddLoad(j).PDiff / Unit_In.CFacUserP()
                    .fldTOper = mAppLoad.AddLoad(j).TOper
                    .fldCavityDepth = Unit_In.L_ConToUser(mAppLoad.AddLoad(j).CavityDepth)
                End With

                pIPEDBEntities.AddTotblAddLoad(pAddLoad(j))

            Next

            pIPEDBEntities.SaveChanges()


            '....Seal
            Dim pQrySeal = (From pRec In pIPEDBEntities.tblSeal
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldID = pAnalysisID Select pRec).ToList()


            Dim pSeal As New tblSeal
            If (pQrySeal.Count() > 0) Then
                pSeal = pQrySeal(0)
            End If

            With pSeal
                .fldProjectID = mProject.Project_ID
                .fldID = pAnalysisID
                .fldType = mProject.SealType()
                .fldMCS = mSeal.MCrossSecNo
                .fldSegmented = mSeal.IsSegmented

                If (mSeal.IsSegmented = True) Then
                    .fldSegmentCount = mSeal.CountSegment
                Else
                    .fldSegmentCount = 0
                End If

                .fldMatName = mSeal.Mat.Name
                .fldHeatTreatment = mSeal.Mat.HT
                .fldCoating = mSeal.Mat.Coating

                If (mSeal.Type = "E-Seal") Then
                    .fldSFinish = CType(mSeal, IPE_clsESeal).SFinish

                ElseIf (mSeal.Type = "C-Seal") Then
                    .fldPlating = CType(mSeal, IPE_clsCSeal).Plating.Code
                    .fldPlatingThickCode = CType(mSeal, IPE_clsCSeal).Plating.ThickCode
                End If

                .fldZClear = Unit_In.L_ConToUser(mSeal.ZClear_Given)
                .fldHFreeTol1 = Unit_In.L_ConToUser(mSeal.HFreeTol(1))
                .fldHFreeTol2 = Unit_In.L_ConToUser(mSeal.HFreeTol(2))

                If (mSeal.Adjusted = "Y") Then
                    .fldAdjusted = True
                Else
                    .fldAdjusted = False
                End If

                .fldSelected = mSeal.Selected

                If (pQrySeal.Count() > 0) Then
                    pIPEDBEntities.SaveChanges()
                Else
                    pIPEDBEntities.AddTotblSeal(pSeal)
                    pIPEDBEntities.SaveChanges()
                End If

            End With

            If (mSeal.Type = "E-Seal") Then
                Dim pQryAdjESeal = (From it In pIPEDBEntities.tblAdjESeal
                                      Where it.fldProjectID = pProjectID And
                                         it.fldSealID = pAnalysisID Select it).ToList()
                Dim pESeal As New tblAdjESeal

                If (pQryAdjESeal.Count() > 0) Then
                    pESeal = pQryAdjESeal(0)

                End If

                With pESeal
                    .fldProjectID = mProject.Project_ID
                    .fldSealID = pAnalysisID
                    .fldDThetaE1 = CType(mSeal, IPE_clsESeal).DThetaE1
                    .fldDThetaM1 = CType(mSeal, IPE_clsESeal).DThetaM1

                    If (pQryAdjESeal.Count() > 0) Then
                        pIPEDBEntities.SaveChanges()
                    Else
                        pIPEDBEntities.AddTotblAdjESeal(pESeal)
                        pIPEDBEntities.SaveChanges()
                    End If

                End With

            ElseIf (mSeal.Type = "C-Seal") Then
                Dim pQryAdjCSeal = (From it In pIPEDBEntities.tblAdjCSeal
                                     Where it.fldProjectID = pProjectID And
                                        it.fldSealID = pAnalysisID Select it).ToList()
                Dim pCSeal As New tblAdjCSeal

                If (pQryAdjCSeal.Count() > 0) Then
                    pCSeal = pQryAdjCSeal(0)

                End If

                With pCSeal
                    .fldProjectID = mProject.Project_ID
                    .fldSealID = pAnalysisID
                    .fldDHFree = Unit_In.L_ConToUser(CType(mSeal, IPE_clsCSeal).DHfree)
                    .fldDThetaOpening = CType(mSeal, IPE_clsCSeal).DThetaOpening
                    .fldDT = Unit_In.L_ConToUser(mSeal.T)
                    If (pQryAdjCSeal.Count() > 0) Then
                        pIPEDBEntities.SaveChanges()
                    Else
                        pIPEDBEntities.AddTotblAdjCSeal(pCSeal)
                        pIPEDBEntities.SaveChanges()
                    End If
                End With

            ElseIf (mSeal.Type = "U-Seal") Then
                Dim pQryAdjUSeal = (From it In pIPEDBEntities.tblAdjUSeal
                                     Where it.fldProjectID = pProjectID And
                                        it.fldSealID = pAnalysisID Select it).ToList()
                Dim pUSeal As New tblAdjUSeal

                If (pQryAdjUSeal.Count() > 0) Then
                    pUSeal = pQryAdjUSeal(0)

                End If

                With pUSeal
                    .fldProjectID = mProject.Project_ID
                    .fldSealID = pAnalysisID
                    .fldDTheta1 = CType(mSeal, IPE_clsUSeal).DTheta(1)
                    .fldDTheta2 = CType(mSeal, IPE_clsUSeal).DTheta(2)

                    .fldDRad1 = Unit_In.L_ConToUser(CType(mSeal, IPE_clsUSeal).DRad(1))
                    .fldDRad2 = Unit_In.L_ConToUser(CType(mSeal, IPE_clsUSeal).DRad(2))
                    .fldDLLeg = Unit_In.L_ConToUser(CType(mSeal, IPE_clsUSeal).DLLeg)

                    .fldDT = Unit_In.L_ConToUser(mSeal.T)

                    If (pQryAdjUSeal.Count() > 0) Then
                        pIPEDBEntities.SaveChanges()
                    Else
                        pIPEDBEntities.AddTotblAdjUSeal(pUSeal)
                        pIPEDBEntities.SaveChanges()
                    End If
                End With
            End If

            '....Result_Gen
            Dim pQryResultGen = (From pRec In pIPEDBEntities.tblResult_Gen
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldAnalysisID = pAnalysisID Select pRec).ToList()

            Dim pResultGen As New tblResult_Gen

            If (pQryResultGen.Count() > 0) Then
                pResultGen = pQryResultGen(0)
            End If

            With pResultGen
                .fldProjectID = mProject.Project_ID
                .fldAnalysisID = pAnalysisID
                .fldSolnConv = mResult_Gen.SolnConv
                .fldDSealing = Unit_In.L_ConToUser(mSeal.DSealing)
                .fldPeneMax1 = Unit_In.L_ConToUser(mResult_Gen.PeneMax1)
                .fldHFreeFinal = Unit_In.L_ConToUser(mResult_Gen.HFreeFinal)
                .fldLeakage_BL = Unit_In.L_ConToUser(mResult_Gen.Leakage_BL)
            End With

            With pIPEDBEntities
                If (pQryResultGen.Count() > 0) Then
                    '....Record Exists
                    .SaveChanges()
                Else
                    '....New Record
                    .AddTotblResult_Gen(pResultGen)
                    .SaveChanges()
                End If
            End With


            '....Result Load Step
            Dim pQryResultLoadStep = (From pRec In pIPEDBEntities.tblResult_LoadStep
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldAnalysisID = pAnalysisID Select pRec).ToList()

            If (pQryResultLoadStep.Count() > 0) Then
                For j As Integer = 0 To pQryResultLoadStep.Count() - 1
                    pIPEDBEntities.DeleteObject(pQryResultLoadStep(j))
                    pIPEDBEntities.SaveChanges()
                Next
            End If


            Dim pResultLoadStep As New List(Of tblResult_LoadStep)

            For j As Integer = 0 To mResult_LoadStep.Count - 1
                Dim pResultLoad As New tblResult_LoadStep
                pResultLoadStep.Add(pResultLoad)
                With pResultLoadStep(j)
                    .fldProjectID = mProject.Project_ID
                    .fldAnalysisID = pAnalysisID
                    .fldStepID = (j + 1)
                    '.fldFContact = Val(Format((mResult_LoadStep(j).FContact * Unit_In.CFacUserL), "#0.00"))    'mResult_LoadStep(j).FContact

                    'AES 17MAY17
                    .fldFSeat_Unit = Val(Format((mResult_LoadStep(j).FContact * Unit_In.CFacUserL), "#0.00"))    'mResult_LoadStep(j).FContact
                    .fldSigEqvMax = Val(Unit_In.RoundStressUnitUser(mResult_LoadStep(j).SigEqvMax)) 'mResult_LoadStep(j).SigEqvMax
                End With

                pIPEDBEntities.AddTotblResult_LoadStep(pResultLoadStep(j))
            Next

            pIPEDBEntities.SaveChanges()

            '....FEA Params
            Dim pQryFEA = (From pRec In pIPEDBEntities.tblFEAParam
                               Where pRec.fldProjectID = pProjectID And
                                     pRec.fldAnalysisID = pAnalysisID Select pRec).ToList()

            Dim pFEAParam As New tblFEAParam

            If (pQryFEA.Count() > 0) Then
                pFEAParam = pQryFEA(0)
            End If

            With pFEAParam
                .fldProjectID = mProject.Project_ID
                .fldAnalysisID = pAnalysisID
                .fldANSYS_Ver = ANSYS_In.Version
                .fldMaxElement = mSeal.NelMax 'mANSYS.NelMax

                If (mSeal.Type = "E-Seal") Then
                    .fldNLayer = CType(mSeal, IPE_clsESeal).NLayer

                    .fldNelConSeal = CType(mSeal, IPE_clsESeal).NelConSeal
                    .fldNelConMid = CType(mSeal, IPE_clsESeal).NelConMid
                    .fldNelConHeel = CType(mSeal, IPE_clsESeal).NelConHeel
                    .fldNelConRad = CType(mSeal, IPE_clsESeal).NelConRad

                    .fldFacKN = CType(mSeal, IPE_clsESeal).FacKN
                    .fldFacTOLN = CType(mSeal, IPE_clsESeal).FacTOLN

                    .fldNSBSTP = CType(mSeal, IPE_clsESeal).NSBSTP
                    .fldNSBMX = CType(mSeal, IPE_clsESeal).NSBMX
                    .fldNSBMN = CType(mSeal, IPE_clsESeal).NSBMN

                ElseIf (mSeal.Type = "C-Seal") Then

                    .fldNLayer = CType(mSeal, IPE_clsCSeal).NLayer

                    If (CType(mSeal, IPE_clsCSeal).Plating.Exists) Then
                        .fldNLayer_Plating = CType(mSeal, IPE_clsCSeal).Plating.NLayer
                    End If

                    .fldNelConSeal = CType(mSeal, IPE_clsCSeal).NelConSeal
                    .fldNelConRad = CType(mSeal, IPE_clsCSeal).NelConRad

                    .fldFacKN = CType(mSeal, IPE_clsCSeal).FacKN
                    .fldFacTOLN = CType(mSeal, IPE_clsCSeal).FacTOLN

                    .fldNSBSTP = CType(mSeal, IPE_clsCSeal).NSBSTP
                    .fldNSBMX = CType(mSeal, IPE_clsCSeal).NSBMX
                    .fldNSBMN = CType(mSeal, IPE_clsCSeal).NSBMN

                ElseIf (mSeal.Type = "U-Seal") Then

                    .fldNLayer = CType(mSeal, IPE_clsUSeal).NLayer

                    .fldNelConSeal = CType(mSeal, IPE_clsUSeal).NelConSeal
                    .fldNelConHeel = CType(mSeal, IPE_clsUSeal).NelConHeel
                    .fldNelConRad = CType(mSeal, IPE_clsUSeal).NelConRad

                    .fldFacKN = CType(mSeal, IPE_clsUSeal).FacKN
                    .fldFacTOLN = CType(mSeal, IPE_clsUSeal).FacTOLN

                    .fldNSBSTP = CType(mSeal, IPE_clsUSeal).NSBSTP
                    .fldNSBMX = CType(mSeal, IPE_clsUSeal).NSBMX
                    .fldNSBMN = CType(mSeal, IPE_clsUSeal).NSBMN
                End If

                If (pQryFEA.Count() > 0) Then
                    pIPEDBEntities.SaveChanges()
                Else
                    pIPEDBEntities.AddTotblFEAParam(pFEAParam)
                    pIPEDBEntities.SaveChanges()
                End If

            End With

        Catch ex As Exception

        End Try

    End Sub


    Public Function IsRecordExists_DB() As Boolean
        '=========================================
        Dim pRecExists As Boolean
        Dim pSealEntities As New SealIPEDBEntities

        Dim pRecQry = (From pRec In pSealEntities.tblAnalysis
                                            Where pRec.fldProjectID = mProject.Project_ID And
                                                  pRec.fldID = mID Select pRec).ToList()
        If (pRecQry.Count() > 0) Then
            pRecExists = True
        End If

        Return pRecExists

    End Function


    Public Function State() As eState
        '============================
        Dim pState As eState
        If (mID > 0) Then
            If (mResult_Gen.SolnConv = 1) Then
                pState = eState.Complete
            Else
                pState = eState.Incomplete
            End If
        Else
            pState = eState.Incomplete
        End If

        Return pState

    End Function


    Private Sub Calc_Compression()
        '=========================
        'This subroutine calculates various 'compression parameters' for a given "Cavity"  
        '....and "Seal" objects for the given tolerance type .

        If mCavity Is Nothing = True Or mSeal Is Nothing = True Then
            Exit Sub
        Else
            If mCavity.Depth < mcEPS Or mSeal.Hfree < mcEPS Or mCompression.TolType = "" Then _
                Exit Sub
        End If

        With mCompression
            '....Installation Compression.
            .Val = mSeal.HActual("Initial", mCompression.TolType) - mCavity.DepthActual(mCompression.TolType)
            .PcentVal = (.Val / mSeal.Hfree) * 100.0#

            If .PcentVal.ToString = "NaN" Then
                .PcentVal = 0.0
            End If

            '....Min Installation Compress (%)
            '........This is to be checked against a minimum recommended value.
            Dim psngCompMin As Single
            psngCompMin = mSeal.HActual("Initial", "Minimum") - mCavity.DepthActual("Minimum")
            .PcentValMin = (psngCompMin / mSeal.Hfree) * 100.0#
        End With

    End Sub


    Public Function LoadStep() As List(Of sLoadStep)
        '===========================================
        Dim pLoadStep As New List(Of sLoadStep)

        '....Load Step: 'Assembly'
        Dim pLoadStep_Assy As New sLoadStep

        With pLoadStep_Assy
            .PDiff = 0
            .T = mOpCond.TRoom
            .CavityDepth = mCavity.Depth
            .CompressionVal = mCompression.Val
            .Descrip = "Assembly"
        End With

        '....Load Step: 'Open'
        Dim pLoadStep_Open As New sLoadStep

        With pLoadStep_Open
            .PDiff = 0
            .T = mOpCond.TRoom
            .CavityDepth = mCavity.Depth
            .CompressionVal = 0.0
            .Descrip = "Open"
        End With


        'CREATE LOADSTEP LIST:
        '--------------------
        If (mLoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then        '....LOAD CASE: BASELINE

            If mAppLoad.PreComp.Exists = True Then

                '....Load Step: Pre-Compressed
                Dim pLoadStep_PC As New sLoadStep

                With pLoadStep_PC
                    .PDiff = 0 'mOpCond.PDiff
                    .T = mOpCond.TOper
                    .CavityDepth = mCavity.Depth
                    .CompressionVal = mSeal.HActual("Initial", mCompression.TolType) - mAppLoad.PreComp.HMin
                    .Descrip = "PC"
                End With

                '....List
                pLoadStep.Add(pLoadStep_PC)

            End If


            '....Load Step: Baseline
            Dim pLoadStep_BL As New sLoadStep

            With pLoadStep_BL
                .PDiff = mOpCond.PDiff
                .T = mOpCond.TOper
                .CavityDepth = mCavity.Depth
                .CompressionVal = mCompression.Val
                .Descrip = "BL"
            End With

            '....List
            With pLoadStep
                .Add(pLoadStep_Assy)
                .Add(pLoadStep_BL)
                .Add(pLoadStep_Assy)
                .Add(pLoadStep_Open)
            End With


        Else                                                    '....LOAD CASE: ADDITIONAL
            '....List
            pLoadStep.Add(pLoadStep_Assy)

            Dim pLoadStep_AddLoad As New sLoadStep

            For i As Integer = 0 To mAppLoad.AddLoad.Count - 1
                pLoadStep_AddLoad = New sLoadStep
                pLoadStep_AddLoad.PDiff = mAppLoad.AddLoad(i).PDiff
                pLoadStep_AddLoad.T = mAppLoad.AddLoad(i).TOper
                pLoadStep_AddLoad.CavityDepth = mAppLoad.AddLoad(i).CavityDepth

                'Dim pCavity As New clsCavity(mSeal.Type)
                Dim pCavity As New IPE_clsCavity(mSeal.Type, mProject.UnitSystem())         'AES 15APR16
                pCavity = mCavity.Clone()
                pCavity.Depth = mAppLoad.AddLoad(i).CavityDepth

                Dim pCompressionVal As Single
                pCompressionVal = mSeal.HActual("Initial", mCompression.TolType) - pCavity.DepthActual(mCompression.TolType)
                pLoadStep_AddLoad.CompressionVal = pCompressionVal

                pLoadStep_AddLoad.Descrip = "A" & (i + 1).ToString()
                pLoadStep.Add(pLoadStep_AddLoad)
            Next

            With pLoadStep
                .Add(pLoadStep_Assy)
                .Add(pLoadStep_Open)
            End With

        End If

        Return pLoadStep

    End Function


    Public Sub WriteFile_ANSYS_Input(ByVal Unit_In As IPE_clsUnit, ByVal ANSYS_In As IPE_clsANSYS,
                                     Optional ByVal LoadStep_In As List(Of sLoadStep) = Nothing,
                                     Optional ByVal DControl_In As Double = 0.0)
        '=======================================================================================
        'This routine creates the input data file for the ANSYS. This file is
        '....read by the preprocessor model file - "SealIPE_V9.pre".

        'Material Data File:        
        '-------------------    
        Dim pstrFileName As String
        pstrFileName = mSeal.Mat.FileName

        Dim iPos As Integer, iPos1 As Integer
        iPos = InStr(1, pstrFileName, "\Material")
        iPos1 = InStr(1, pstrFileName, ".")

        Dim pMatFileDir As String, pMatFile As String
        pMatFileDir = Left$(pstrFileName, iPos - 1)


        pMatFile = Trim(modMain.MID_String$(pstrFileName, iPos + 1, iPos1 - (iPos + 1)))


        'Plating Material Data File:            
        '---------------------------
        pstrFileName = mSeal.Mat.PlatingFileName

        iPos = InStr(1, pstrFileName, "\Material")
        iPos1 = InStr(1, pstrFileName, ".")

        Dim pPlatingMatFile As String

        pPlatingMatFile = Trim(modMain.MID_String$(pstrFileName, iPos + 1, iPos1 - (iPos + 1)))


        Dim pSW As StreamWriter
        Try
            pSW = File.CreateText(ANSYS_In.InpFileName)

        Catch pEXP As IOException
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
                                                                 MessageBoxIcon.Error)
            Exit Sub
        End Try


        'Write Data to the file.             
        '----------------------
        With pSW

            .WriteLine("INPUT")
            .WriteLine("/COM,")
            .WriteLine("/COM, ANSYS Input Data File: Read in SealIPE_V10.pre'  ")

            Dim pstrSealID As String
            pstrSealID = mSeal.Type & "-" & mSeal.MCrossSecNo

            .WriteLine("/title, " & pstrSealID)
            .WriteLine("/COM,   **************")

            .WriteLine("unitSys       = '" & Unit_In.System & "'")
            .WriteLine("SealType      = '" & mSeal.Type & "'")
            .WriteLine("CrossSecNo    = '" & mSeal.MCrossSecNo & "'")
            .WriteLine("Adjusted      = '" & mSeal.Adjusted & "'")
            .WriteLine("POrient       = '" & mSeal.POrient & "'")
            .WriteLine("ANSYS_Model   = '" & ANSYS_In.Model & "'")
            .WriteLine()

            .WriteLine("/COM, Geometry")
            .WriteLine("/COM, ========")

            'AES 10MAY17
            If (DControl_In > gcEPS) Then
                .WriteLine("DControl  = " & Unit_In.WriteInUserL(DControl_In))
            Else
                .WriteLine("DControl  = " & mSeal.DControl)
            End If

            .WriteLine("ZClear    = " & mSeal.ZClear)
            .WriteLine("TStd      = " & mSeal.TStd)
            .WriteLine()

            'AES 28APR16
            If (mSeal.IsSegmented) Then
                .WriteLine("IsSegmented  = " & "'Y'")
                .WriteLine("CountSegment = " & mSeal.CountSegment)
            Else
                .WriteLine("IsSegmented  = " & "'N'")
            End If
            .WriteLine()

            Dim pstrAny As String

            If mSeal.Type = "C-Seal" Then
                '--------------------------

                .WriteLine("/COM, Plating Data:")
                .WriteLine("/COM, -------------")

                'AES 20SEP16 Not needed now.
                'pstrAny = IIf(CType(mSeal, IPE_clsCSeal).Plating.Exists = True, "Y", "N")
                '.WriteLine("PlatingExists = '" & pstrAny & "'")

                'Dim pPlatingThick As Single
                'pPlatingThick = IIf(CType(mSeal, IPE_clsCSeal).Plating.Exists = True,
                '                    CType(mSeal, IPE_clsCSeal).Plating.Thick, 0.0)
                '.WriteLine("T_pl  = " & pPlatingThick)

                'AES 20SEP16
                .WriteLine("PlatingExists = 'N'")
                .WriteLine("T_pl  = 0")

                .WriteLine()

            End If


            .WriteLine("/COM, Discretization Parameters")
            .WriteLine("/COM, =======================")
            .WriteLine("/COM, Element = PLANE42 ")
            .WriteLine("nelMax    = " & ANSYS_In.NelMax)
            .WriteLine("nLayer    = " & mSeal.NLayer)

            If mSeal.Type = "C-Seal" Then
                '--------------------------

                'AES 20SEP16
                'Dim pNLayer_pl As Int16
                'pNLayer_pl = IIf(CType(mSeal, IPE_clsCSeal).Plating.Exists = True,
                '                 CType(mSeal, IPE_clsCSeal).Plating.NLayer, 0)
                '.WriteLine("nLayer_pl = " & pNLayer_pl)

                'AES 20SEP16
                .WriteLine("nLayer_pl = 0")

            End If

            .WriteLine("ElSize    = " & mSeal.ElSize)
            .WriteLine()


            .WriteLine("/COM, Contact Regions:")
            .WriteLine("/COM, ----------------")
            .WriteLine("/COM, # of Contact Elements on either side of various " & _
                               "contact regions midpoint")
            .WriteLine("/COM, ....Rigid Flange - Axial Surface")
            .WriteLine("nelConSeal = " & mSeal.NelConSeal & Space(11) & _
                                                        "!....Sealing Region")

            .WriteLine("/COM, ....Rigid Flange - Radial Surface")
            .WriteLine("nelConRad  = " & mSeal.NelConRad)

            .WriteLine()
            .WriteLine("/COM, 2D Surface-to-Surface Contact.")
            .WriteLine("facKN     = " & mSeal.FacKN & Space(5) & _
                        "! Contact Stiffness Factor     (Range 0.001-100, Default = 1))")
            .WriteLine("facTOLN   = " & mSeal.FacTOLN & Space(5) & _
                        "!....Penetration Tolerance Factor (Range < 1.0,Default = 0.1)")

            .WriteLine()
            .WriteLine("/COM, Grid Clustering Parameters")
            .WriteLine("betaT = " & mSeal.BetaT & Space(5) & _
                                                    "! Thickness  Direction")
            .WriteLine("betaM = " & mSeal.BetaM & Space(7) & _
                                                    "! Meridional Direction")
            .WriteLine()


            .WriteLine("/COM, Material Model & Properties")
            .WriteLine("/COM, ===========================")

            .WriteLine("/COM, Conversion factors from 'English' to '" & Unit_In.System & _
                                                     "' in respective consistent units.")
            .WriteLine("/COM,")
            .WriteLine("cFacP  = " & Unit_In.CFacConP & Space(5) & _
                                                "! Pressure (Stress, Elasticity Modulus)")
            .WriteLine("cFacT  = " & Unit_In.CFacConT & Space(5) & _
                                                "! Temperature")
            .WriteLine("cOffT  = " & Unit_In.CFacConTOff & Space(5) & _
                                                "! Temperature - Offset")

            .WriteLine("/COM,")
            .WriteLine("/COM, The following database is in English units.")
            .WriteLine("/COM, Conversion to the '" & Unit_In.System & "'" & _
                                                " performed within the following file")
            .WriteLine("/COM, using the above conversion factors.")
            .WriteLine("/COM,")

            'AES 10MAR17
            .WriteLine("MatModel = '" & mMatModel.ToString() & "'")
            .WriteLine("*ULIB," & pMatFile & ",dat," & "'" & pMatFileDir & "'")

            Dim pMatName As String = IIf(mSeal.Mat.Coating <> "None", _
                                         mSeal.Mat.Name & "_" & mSeal.Mat.Coating, _
                                         mSeal.Mat.Name)
            .WriteLine("*USE," & pMatName)

            'AES 20SEP16 Not needed now.
            'If mSeal.Type = "C-Seal" Then
            '    If CType(mSeal, IPE_clsCSeal).Plating.Exists = True Then
            '        .WriteLine("/COM, Plating material Data File.")
            '        .WriteLine("*ULIB," & pPlatingMatFile & ",dat," & pMatFileDir)
            '        .WriteLine("*USE," & CType(mSeal, IPE_clsCSeal).PlatingName)
            '    End If
            'End If

            '....Yield Strength @ TRoom & TOper: in psi or MPa.     
            .WriteLine("")
            .WriteLine("/COM, Yield Strengths: (For ANSYS Display only)")
            .WriteLine("UnitUserStress = '" & Unit_In.UserStress & "'")
            'AES 20MAR17    'As per discussion with PB
            '.WriteLine("SigmaY_TRoom = " & Unit_In.RoundStressUnitUser(mSeal.Mat.SigmaY_TRoom))
            'AES 17MAR17
            '.WriteLine("SigmaY_TOper = " & Unit_In.RoundStressUnitUser(mSeal.Mat.SigmaY_TOper))    


            '....Stress Plot Consistent Units:
            Dim pUnitConStress As String = ""
            If Unit_In.System = "English" Then
                pUnitConStress = "psi"
            ElseIf Unit_In.System = "Metric" Then
                pUnitConStress = "Pa"
            End If

            .WriteLine("")
            .WriteLine("/COM, Stress Consistent Unit: (For ANSYS Display only)")
            .WriteLine("UnitConStress = '" & pUnitConStress & "'")
            .WriteLine("")

            .WriteLine("/COM, Opering Conditions")
            .WriteLine("/COM, ==================")
            .WriteLine("PDiff_Oper = " & mOpCond.PDiff)
            .WriteLine("")

            .WriteLine("/COM, Applied Loading")
            .WriteLine("/COM, ===============")
            If (mAppLoad.PreComp.Exists) Then
                .WriteLine("/COM, Pre-Compressed = 'Y', HMin = " & mAppLoad.PreComp.HMin.ToString("#0.000"))
            Else
                .WriteLine("/COM, Pre-Compressed = 'N'")
            End If
            pstrAny = IIf(mAppLoad.RadConstraint = True, "Y", "N")
            .WriteLine("RadConstraint = '" & pstrAny & "'")

            .WriteLine()
            .WriteLine("/COM, Want to Solve ?  " & Space(5) & _
                                                    "! 'N'  : Just examine the model.")
            .WriteLine("/COM, ============  " & Space(5) & _
                                                    "! 'Y' : Model is ok.  Solve.")

            .WriteLine("ANSYS_Solve   = '" & ANSYS_In.Solve & "'")
            .WriteLine("ANSYS_RunType = '" & ANSYS_In.RunType & "'" & Space(5) & "! Batch' or 'Interact'")

            .WriteLine("")
            .WriteLine("/COM, LOAD CASE:")
            .WriteLine("/COM, ==========")
            .WriteLine("/COM, Type = '" & mLoadCase.Type.ToString() & "'")
            .WriteLine("/COM, Compression Tol Type = '" & mCompression.TolType & "'")

            .WriteLine()
            .WriteLine("/COM, LOAD STEPS:")
            Dim pLoadStep As New List(Of sLoadStep)

            If (Not IsNothing(LoadStep_In)) Then
                pLoadStep = LoadStep_In
            Else
                pLoadStep = LoadStep()
            End If


            .WriteLine("CountLoadStep = " & pLoadStep.Count)
            .WriteLine()
            .WriteLine("*DIM, PDiff, ARRAY, CountLoadStep")
            .WriteLine("*DIM, TOper, ARRAY, CountLoadStep")
            .WriteLine("*DIM, CompressionVal, ARRAY, CountLoadStep")
            .WriteLine("*DIM, Descrip, CHAR, CountLoadStep")
            .WriteLine("*DIM, SigmaY_TOper, ARRAY, CountLoadStep")        'AES 17MAR17

            .WriteLine()
            Dim pCount As Integer = pLoadStep.Count
            For j As Integer = 0 To pCount - 1
                .WriteLine("PDiff(" & (j + 1).ToString & ") = " & pLoadStep(j).PDiff)
                .WriteLine("TOper(" & (j + 1).ToString & ") = " & pLoadStep(j).T)
                .WriteLine("CompressionVal(" & (j + 1).ToString & ") = " & pLoadStep(j).CompressionVal)
                .WriteLine("Descrip(" & (j + 1).ToString & ") = '" & pLoadStep(j).Descrip & "'")

                mSeal.Mat.TOper = pLoadStep(j).T     'AES 17MAR17
                .WriteLine("SigmaY_TOper(" & (j + 1).ToString & ") = " & Unit_In.RoundStressUnitUser(mSeal.Mat.SigmaY_TOper))
                .WriteLine()
            Next

            .WriteLine("/COM, Load Sub-Step Parameters:")
            .WriteLine(" NSBSTP = " & mSeal.NSBSTP)
            .WriteLine(" NSBMX  = " & mSeal.NSBMX)
            .WriteLine(" NSBMN  = " & mSeal.NSBMN)

            .WriteLine()
            pstrAny = IIf(mFatigueData = True, "Y", "N")
            .WriteLine(" FatigueData = '" & pstrAny & "'")      'AES 30AUG16
            .WriteLine()
            .WriteLine("/COM,")


            'SPECIFIC DATA:
            '-------------
            '
            If mSeal.Type = "C-Seal" Then
                '==========================
                Dim pCSeal As IPE_clsCSeal
                pCSeal = CType(mSeal, IPE_clsCSeal)

                .WriteLine("/COM, SPECIFIC DATA (CSeal):")
                .WriteLine("/COM, **********************")
                .WriteLine("/COM, Geometry")
                .WriteLine("/COM, ========")
                .WriteLine("HfreeStd        = " & pCSeal.HfreeStd)
                .WriteLine("ThetaOpeningStd = " & pCSeal.ThetaOpeningStd)
                .WriteLine("WidStd          = " & pCSeal.WidStd & Space(5) & _
                                              "! Not used in ANSYS Model")

                .WriteLine("/COM,")
                .WriteLine("/COM, Geometry adjustment parameters:")
                .WriteLine("DHfree        = " & pCSeal.DHfree)
                .WriteLine("DThetaOpening = " & pCSeal.DThetaOpening)
                .WriteLine("T             = " & pCSeal.T)


            ElseIf mSeal.Type = "E-Seal" Then
                '==============================
                Dim pESeal As IPE_clsESeal
                pESeal = CType(mSeal, IPE_clsESeal)

                .WriteLine("/COM, SPECIFIC DATA (ESeal):")
                .WriteLine("/COM, **********************")
                .WriteLine("/COM, Geometry")
                .WriteLine("/COM, ========")

                .WriteLine("TemplateNo = " & "'" & pESeal.TemplateNo & "'")

                .WriteLine("NConv = " & pESeal.NConv)
                .WriteLine()

                .WriteLine("/COM,")
                .WriteLine("/COM, Paremeters")
                .WriteLine("/COM, ----------")
                .WriteLine("/COM, End Convolution")
                .WriteLine("RadE      (1) = " & pESeal.RadE(1) & ", " & _
                                                pESeal.RadE(2) & ", " & _
                                                pESeal.RadE(3))

                Dim pThetaE2 As String
                pThetaE2 = Format(pESeal.ThetaE(2), "##00.0000")
                .WriteLine("ThetaEStd (1) = " & pESeal.ThetaE(1) & ", " & _
                                                pThetaE2 & ", " & pESeal.ThetaE(3))

                .WriteLine("/COM,")
                .WriteLine("/COM, Mid Convolution")
                .WriteLine("RadM      (1) = RadE(1)")
                .WriteLine("RadM      (2) = " & pESeal.RadM(2))
                .WriteLine("RadM      (3) = " & pESeal.RadM(3))
                .WriteLine("RadM      (4) = " & pESeal.RadM(4))
                .WriteLine("RadM      (5) = " & pESeal.RadM(5))
                .WriteLine()
                .WriteLine("ThetaMStd (1) = " & pESeal.ThetaM(1))
                .WriteLine("ThetaMStd (2) = " & pESeal.ThetaM(2))
                .WriteLine("ThetaMStd (3) = " & pESeal.ThetaM(3))
                .WriteLine("ThetaMStd (4) = " & pESeal.ThetaM(4))
                .WriteLine("ThetaMStd (5) = " & pESeal.ThetaM(5))


                .WriteLine("/COM,")
                .WriteLine("/COM, Convolution Depth")
                .WriteLine("HConv = " & pESeal.HConv & Space(5) & _
                                                "! Pertains to the original geometry")

                If pESeal.TemplateNo = "2" Then
                    Call .WriteLine("/COM,")
                    Call .WriteLine("/COM, Straight Segment Length")
                    Call .WriteLine("LFlatE   = " & pESeal.LFlatE & _
                                          "                 ! End Convolution")
                End If

                .WriteLine("/COM,")
                .WriteLine("/COM, Geometry adjustment parameters:")
                .WriteLine("DThetaE1 = " & pESeal.DThetaE1)
                .WriteLine("DThetaM1 = " & pESeal.DThetaM1)


                .WriteLine()
                .WriteLine("/COM, Discretization Parameters")
                .WriteLine("/COM, ====================")
                .WriteLine()
                .WriteLine("/COM, Contact Regions:")
                .WriteLine("/COM, ----------------")
                .WriteLine("/COM, # of Contact Elements on either side of various contact regions midpoint.")
                .WriteLine("/COM, ....Rigid Flange - Axial Surface")

                .WriteLine("nelConMid  = " & pESeal.NelConMid & Space(5) & _
                                                                "!.... Mid Region")
                .WriteLine("nelConHeel = " & pESeal.NelConHeel & Space(5) & _
                                                                "!.... Heel Region")
                .WriteLine()
                .WriteLine("/COM,")
                .WriteLine("/COM, Weightage on # of Elements ")
                .WriteLine("WtE (1)  = " & pESeal.WtE(1) & ", " & _
                                           pESeal.WtE(2) & ", " & _
                                           pESeal.WtE(3) & Space(7) & _
                                           "! End 1/2 Convolution")

                .WriteLine("WtM (1)  = " & pESeal.WtM(1) & ", " & _
                                           pESeal.WtM(2) & ", " & _
                                           pESeal.WtM(3) & Space(5) & _
                                                          "! Mid 1/2 Convolution")

            ElseIf mSeal.Type = "U-Seal" Then
                '==============================     
                Dim pUSeal As IPE_clsUSeal
                pUSeal = CType(mSeal, IPE_clsUSeal)

                .WriteLine("/COM, SPECIFIC DATA (USeal):")
                .WriteLine("/COM, **********************")
                .WriteLine("/COM, Geometry")
                .WriteLine("/COM, ========")

                .WriteLine("/COM,")
                .WriteLine("/COM, Paremeters")
                .WriteLine("/COM, ----------")

                .WriteLine("Rad (1)   = " & pUSeal.R(1))
                .WriteLine("Rad (2)   = " & pUSeal.R(2))

                .WriteLine()
                .WriteLine("Theta (1) = " & pUSeal.Theta(1))
                .WriteLine("Theta (2) = " & pUSeal.Theta(2))
                .WriteLine()

                .WriteLine("Beta (1) = " & pUSeal.Beta(1))
                .WriteLine("Beta (2) = " & pUSeal.Beta(2))
                .WriteLine()

                .WriteLine("LLeg      = " & pUSeal.LLeg)
                .WriteLine()

                .WriteLine("T         = " & pUSeal.T)

                .WriteLine()
                .WriteLine("/COM, Discretization Parameters")
                .WriteLine("/COM, =========================")
                .WriteLine()
                .WriteLine("/COM, Contact Regions:")
                .WriteLine("/COM, ----------------")
                .WriteLine("/COM, ....Rigid Flange - Axial Surface")
                .WriteLine("nelConHeel = " & pUSeal.NelConHeel & Space(5) & _
                                                                "!.... Heel Region")
                .WriteLine()

            End If

            .WriteLine("/EOF")
            .Close()

        End With

    End Sub


    Public Sub ReadFile_ANSYS_Output(ByVal ANSYS_In As IPE_clsANSYS)
        '=======================================================
        '....This subroutine reads the FEA output results from the mOutFileName. 

        Dim pSR As StreamReader

        Try
            pSR = File.OpenText(ANSYS_In.OutFileName)

        Catch pEXP As IOException
            '....Error Handler
            MessageBox.Show(pEXP.Message, "File Not Found", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
            Exit Sub
        End Try

        pSR.ReadLine()
        pSR.ReadLine()

        Dim pstrAny As String
        pstrAny = pSR.ReadLine()

        Dim pstrOut(3) As String
        pstrOut = pstrAny.Split(",")         '....Delimiter = ","

        If pstrOut.Length = 5 Then
            With mSeal
                ANSYS_In.SolnConv = Val(Trim(pstrOut(0)))
                .PeneMax1 = Val(Trim(pstrOut(1)))
                .DSealing = Val(Trim(pstrOut(2)))
                .HfreeFinal = Val(Trim(pstrOut(3)))

                mResult_Gen.SolnConv = ANSYS_In.SolnConv
                mResult_Gen.PeneMax1 = .PeneMax1
                mResult_Gen.HFreeFinal = .HfreeFinal


                Dim pLoadStep As New List(Of sLoadStep)
                pLoadStep = LoadStep()

                mResult_LoadStep = New List(Of sResult_LoadStep)
                For i As Integer = 0 To pLoadStep.Count - 1
                    pSR.ReadLine()
                    pstrAny = pSR.ReadLine()
                    pstrOut = pstrAny.Split(",")
                    '....Contact Forces / unit circumference.
                    Dim pCircum As Single = (Math.PI * .DSealing)
                    .FSeat_Unit(i) = Val(Trim(pstrOut(0))) / pCircum
                    .StressMax(i) = Val(Trim(pstrOut(1)))

                    Dim pResultLoadStep As New sResult_LoadStep
                    pResultLoadStep.FContact = mSeal.FSeat_Unit(i)
                    pResultLoadStep.SigEqvMax = mSeal.StressMax(i)
                    mResult_LoadStep.Add(pResultLoadStep)
                Next

                If (mSeal.Type = "E-Seal") Then
                    mResult_Gen.Leakage_BL = CType(mSeal, IPE_clsESeal).Leakage_Oper(mOpCond.PDiff, mSeal.Mat.Coating)
                End If

            End With
        End If

        pSR.Close()

    End Sub

    Public Sub ReadInFile(ByVal Project_In As IPE_clsProject, ByVal Analysis_Cur_In As Integer, ByVal FileName_In As String,
                          ByRef Unit_Out As IPE_clsUnit, ByRef ANSYS_Out As IPE_clsANSYS)
        '================================================================================
        'This routine reads the input data from a file and store the values in the
        '....input data set.

        '....Delimiter for data extraction.
        Const pcstrSearch As String = "="

        Dim pstrAny As String
        Dim pSR As StreamReader = Nothing

        pSR = File.OpenText(FileName_In)
        Dim pText As String = pSR.ReadToEnd()

        Dim pblnAddlnLoadSteps As Boolean = pText.Contains("Additional Load Steps")

        pSR.Close()
        pSR = Nothing

        Try
            pSR = File.OpenText(FileName_In)

            With pSR

                For i As Integer = 0 To 15
                    .ReadLine()
                Next

                'Dim pVal As String = .ReadLine()
                '....Cavity gIPE_Unit.L_UserToCon
                mCavity.Dia(2) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                mCavity.Dia(1) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                mCavity.WidMin = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                mCavity.CornerRad = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                mCavity.Depth = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                mCavity.DepthTol(1) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                mCavity.DepthTol(2) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                .ReadLine()

                '....Oparating Condition
                .ReadLine()
                pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                Dim psngPDiff_UnitUserP As Single
                psngPDiff_UnitUserP = Val(ExtractPreData(pstrAny, " "))
                Unit_Out.UserP = Trim(ExtractPostData(pstrAny, " "))

                mOpCond.PDiff = psngPDiff_UnitUserP * Unit_Out.CFacUserP
                mOpCond.TOper = NInt(ExtractPostData(.ReadLine, pcstrSearch))
                mOpCond.POrient = ExtractPostData(.ReadLine, pcstrSearch)
                .ReadLine()

                '....Applied Loading
                .ReadLine()
                Dim pPreCompressed As String = ""
                pPreCompressed = ExtractPostData(.ReadLine, pcstrSearch)

                If (pPreCompressed = "Y") Then
                    mAppLoad.PreComp_Exits = True
                ElseIf (pPreCompressed = "N") Then
                    mAppLoad.PreComp_Exits = False
                End If

                mAppLoad.PreComp_HMin = Val(ExtractPreData(.ReadLine, " "))

                Dim pRadConstraint As String = ""
                pRadConstraint = ExtractPostData(.ReadLine, pcstrSearch)

                If (pRadConstraint = "Y") Then
                    mAppLoad.RadConstraint = True
                ElseIf (pRadConstraint = "N") Then
                    mAppLoad.RadConstraint = False
                End If
                .ReadLine()

                '....Additional Load Steps
                If (pblnAddlnLoadSteps) Then
                    Dim pCount As Integer = Val(ExtractPostData(.ReadLine, ":"))
                    For i As Integer = 0 To pCount - 1
                        .ReadLine()

                        Dim pAddLoad As New IPE_clsAppLoad.sAddLoad

                        'pAddLoad.PDiff = 0
                        'pAddLoad.TOper = 0

                        pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                        psngPDiff_UnitUserP = Val(ExtractPreData(pstrAny, " "))
                        Unit_Out.UserP = Trim(ExtractPostData(pstrAny, " "))

                        pAddLoad.PDiff = psngPDiff_UnitUserP * Unit_Out.CFacUserP
                        'pAddLoad.TOper = NInt(Val(ExtractPreData(pstrAny, " ")))
                        pAddLoad.TOper = ExtractPostData(.ReadLine, pcstrSearch)
                        pAddLoad.CavityDepth = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                        mAppLoad.AddLoad.Add(pAddLoad)
                        .ReadLine()
                    Next
                End If
                .ReadLine()

                '....Seal Design
                .ReadLine()         '....SealType
                Dim pMCS As String = ExtractPostData(.ReadLine, pcstrSearch).Trim()
                'mSeal.MCrossSecNo = ExtractPostData(.ReadLine, pcstrSearch)
                Dim pSealType As String = ""
                Dim pMCS_Prefix As Integer = Val(pMCS.Substring(0, 2))

                If (pMCS_Prefix = 69) Then
                    pSealType = "E-Seal"
                ElseIf (pMCS_Prefix = 76) Then
                    pSealType = "C-Seal"
                ElseIf (pMCS_Prefix = 79) Then
                    pSealType = "U-Seal"
                End If


                'CType(mSeal, IPE_clsESeal).Type = ExtractPostData(.ReadLine, pcstrSearch)

                If pSealType = "E-Seal" Then
                    mSeal = New IPE_clsESeal(pSealType, gIPE_Unit.System, Project_In.Analysis(Analysis_Cur_In).OpCond.POrient)

                ElseIf pSealType = "C-Seal" Then
                    mSeal = New IPE_clsCSeal(pSealType, gIPE_Unit.System, Project_In.Analysis(Analysis_Cur_In).OpCond.POrient)

                ElseIf pSealType = "U-Seal" Then
                    mSeal = New IPE_clsUSeal(pSealType, gIPE_Unit.System, Project_In.Analysis(Analysis_Cur_In).OpCond.POrient)
                End If

                If pMCS.Contains("New") Then
                    Dim pCrossSecNo As String
                    pCrossSecNo = ExtractPreData(pMCS, "(New)")
                    mSeal.MCrossSecNo = pCrossSecNo

                    If pMCS.Contains("(Original)") Then
                        Dim pCrossSecNoOrg As String
                        pstrAny = ExtractPostData(pMCS, ")")
                        pCrossSecNoOrg = ExtractPreData(pstrAny, "(Original)")

                        If pSealType = "E-Seal" Then
                            CType(mSeal, IPE_clsESeal).CrossSecNoOrg = pCrossSecNoOrg

                        ElseIf pSealType = "U-Seal" Then
                            CType(mSeal, IPE_clsUSeal).CrossSecNoOrg = pCrossSecNoOrg
                        End If
                    End If

                Else
                    mSeal.MCrossSecNo = pMCS
                End If


                Dim pSegmented As String
                pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                pSegmented = ExtractPreData(pstrAny, "Count").Trim()

                If (pSegmented = "Y") Then
                    mSeal.IsSegmented = True
                ElseIf (pSegmented = "N") Then
                    mSeal.IsSegmented = False
                End If

                pstrAny = ExtractPostData(pstrAny, "")
                mSeal.CountSegment = ConvertToInt(ExtractPostData(pstrAny, pcstrSearch))

                Dim pstrValue As String = ExtractPostData(.ReadLine, pcstrSearch)
                mSeal.Mat.Name = ExtractPreData(pstrValue, "HT")

                Dim pstrValue1 As String = ExtractPostData(pstrValue, "HT")
                pstrValue1 = ExtractPreData(pstrValue1, "Coating")
                mSeal.Mat.HT = ConvertToInt(ExtractPostData(pstrValue1, pcstrSearch))
                mSeal.Mat.TOper = mOpCond.TOper     'AES 14MAR17

                pstrValue = ExtractPostData(pstrValue, "Coating")
                mSeal.Mat.Coating = ExtractPostData(pstrValue, pcstrSearch)
                mCompression.TolType = ExtractPostData(.ReadLine, pcstrSearch)
                .ReadLine()     '....DControl
                .ReadLine()     '....HFree

                If pSealType = "E-Seal" Then
                    CType(mSeal, IPE_clsESeal).HFreeTol(1) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                    CType(mSeal, IPE_clsESeal).HFreeTol(2) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))

                ElseIf pSealType = "C-Seal" Then
                    CType(mSeal, IPE_clsCSeal).HFreeTol(1) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                    CType(mSeal, IPE_clsCSeal).HFreeTol(2) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))

                ElseIf pSealType = "U-Seal" Then
                    CType(mSeal, IPE_clsUSeal).HFreeTol(1) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                    CType(mSeal, IPE_clsUSeal).HFreeTol(2) = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))
                End If
                .ReadLine()     '....Seal Width

                mSeal.ZClear_Given = Unit_Out.L_UserToCon(ExtractPostData(.ReadLine, pcstrSearch))

                .ReadLine()

                '....Geometry Adjustment Parameters:
                '
                .ReadLine()         '....Adjusted       (Assigned internally)

                If pSealType = "E-Seal" Then
                    '---------------------------                                
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsESeal).DThetaE1 = Val(pstrAny) 'Unit_Out.L_UserToCon(pstrAny)

                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsESeal).DThetaM1 = Val(pstrAny) 'Unit_Out.L_UserToCon(pstrAny)

                    .ReadLine()     '....T (which is same as TStd) (Retrieved) 


                ElseIf pSealType = "C-Seal" Then
                    '-------------------------------
                    '....DHfree:
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    Dim pDHfree_UserL As Single = Val(pstrAny)
                    CType(mSeal, IPE_clsCSeal).DHfree = Unit_Out.L_UserToCon(pDHfree_UserL)

                    '....DThetaOpening.
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsCSeal).DThetaOpening = Val(pstrAny)

                    '....T:
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsCSeal).T = Unit_Out.L_UserToCon(pstrAny)


                ElseIf pSealType = "U-Seal" Then
                    '--------------------------------'SG 15SEP11

                    '....DTheta (1):                                
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsUSeal).DTheta(1) = Val(pstrAny) 'Unit_Out.L_UserToCon(pstrAny) PB 13SEP11. SG, No need to make the assignment here. It will be automatically assigned in clasUSeal as the geom parameters are retrieved from SealNewDB_user. Think about and let me know if it is correct or not.

                    '....DTheta (2):
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsUSeal).DTheta(2) = Val(pstrAny) 'Unit_Out.L_UserToCon(pstrAny)

                    '....DRad (1):
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsUSeal).DRad(1) = Unit_Out.L_UserToCon(pstrAny)

                    '....DRad (2):
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsUSeal).DRad(2) = Unit_Out.L_UserToCon(pstrAny)

                    '....DLLeg:
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsUSeal).DLLeg = Unit_Out.L_UserToCon(pstrAny)


                    '....T:
                    pstrAny = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsUSeal).T = Unit_Out.L_UserToCon(pstrAny)

                End If

                'FEA Parameters:
                '---------------
                .ReadLine() '....Header line.

                ANSYS_Out.Version = ExtractPostData(.ReadLine, pcstrSearch)
                ANSYS_Out.Model = ExtractPostData(.ReadLine, pcstrSearch)
                ANSYS_Out.NelMax = ExtractPostData(.ReadLine, pcstrSearch)
                ANSYS_Out.Solve = ExtractPostData(.ReadLine, pcstrSearch)
                ANSYS_Out.RunType = ExtractPostData(.ReadLine, pcstrSearch)
                mLoadCase.Name = ExtractPostData(.ReadLine, pcstrSearch)
                .ReadLine() '....Blank line.

                pstrAny = ExtractPostData(.ReadLine, ":")

                'If pstrAny.ToUpper().Trim() = pSealType Then

                '....No. of Element Layers across thickness
                mSeal.NLayer = ExtractPostData(.ReadLine, pcstrSearch)

                If pSealType = "C-Seal" Then
                    '---------------------------

                    If CType(mSeal, IPE_clsCSeal).Plating.Code <> "None" Then
                        CType(mSeal, IPE_clsCSeal).PlatingNLayer = ExtractPostData(.ReadLine, pcstrSearch)
                        CType(mSeal, IPE_clsCSeal).PlatingExists = True
                    Else
                        .ReadLine()
                    End If

                End If

                .ReadLine()

                '....# of Contact Elements:
                mSeal.NelConSeal = ExtractPostData(.ReadLine, pcstrSearch)   '....Sealing Region

                If pSealType = "E-Seal" Then
                    '----------------------------
                    CType(mSeal, IPE_clsESeal).NelConMid = ExtractPostData(.ReadLine, pcstrSearch)   '....Mid Region
                    CType(mSeal, IPE_clsESeal).NelConHeel = ExtractPostData(.ReadLine, pcstrSearch)  '....Heel Region

                ElseIf pSealType = "U-Seal" Then
                    '----------------------------
                    CType(mSeal, IPE_clsUSeal).NelConHeel = ExtractPostData(.ReadLine, pcstrSearch)  '....Heel Region

                End If

                mSeal.NelConRad = ExtractPostData(.ReadLine, pcstrSearch)                        '....Radial Region
                .ReadLine()


                mSeal.FacKN = ExtractPostData(.ReadLine, pcstrSearch)
                mSeal.FacTOLN = ExtractPostData(.ReadLine, pcstrSearch)

                .ReadLine()

                If pSealType = "E-Seal" Then
                    '----------------------------
                    '....Element Density Weightage on Segments:
                    CType(mSeal, IPE_clsESeal).WtE(1) = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsESeal).WtE(2) = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsESeal).WtE(3) = ExtractPostData(.ReadLine, pcstrSearch)

                    CType(mSeal, IPE_clsESeal).WtM(1) = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsESeal).WtM(2) = ExtractPostData(.ReadLine, pcstrSearch)
                    CType(mSeal, IPE_clsESeal).WtM(3) = ExtractPostData(.ReadLine, pcstrSearch)
                    .ReadLine()
                End If

                '....Thickness Direction
                mSeal.BetaT = ExtractPostData(.ReadLine, pcstrSearch)
                .ReadLine()

                '....Load Sub Step Definitions 
                mSeal.NSBSTP = ExtractPostData(.ReadLine, pcstrSearch)
                mSeal.NSBMX = ExtractPostData(.ReadLine, pcstrSearch)
                mSeal.NSBMN = ExtractPostData(.ReadLine, pcstrSearch)

                'End If

            End With
        Catch pEXP As IOException
            '....Error Handler
            MessageBox.Show(pEXP.Message, "File Not Found", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
            Exit Sub


        Catch pEXP As Exception
            '....ERROR HANDLER

            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes As Short
            Dim pintAnswer As Short

            pstrTitle = "ERROR MESSAGE:  Input Data File Reading"

            pstrMsg = "Error in file reading!!" & vbCrLf & _
                     "Please check if it is comaptible with SealIPE V8.0 format." & _
                     vbCrLf & "Consult with Engineering."
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly
            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

            Exit Sub

        Finally
            pSR.Close()
            pSR.Dispose()

        End Try

    End Sub


    Public Sub WriteInFile(ByVal UserInfo_In As IPE_clsUser, ByVal Unit_In As IPE_clsUnit,
                           ByVal ANSYS_In As IPE_clsANSYS, ByVal FileTitle_In As String)
        '==============================================================================

        Dim pSW As StreamWriter = Nothing
        Dim pFilePath As String = "C:\SealIPE\Input Files\V100"
        'Dim pFileTitle As String = "\Test" & mSeal.Type & mID & ".in"
        Dim pFileName As String = pFilePath & "\" & FileTitle_In

        Try
            If (Not Directory.Exists(pFilePath)) Then
                Directory.CreateDirectory(pFilePath)
            End If

            pSW = File.CreateText(pFileName)

            With pSW

                .WriteLine("File Name: " & Trim(pFileName))
                .WriteLine("==========")
                .WriteLine()

                'Header Information:
                '-------------------
                .WriteLine("Header: ")
                .WriteLine(Space(3) & "UserName         = " & UserInfo_In.Name)
                .WriteLine(Space(3) & "PhoneNo          = " & UserInfo_In.PhoneNo)
                .WriteLine(Space(3) & "Customer         = " & mProject.Customer())
                .WriteLine(Space(3) & "Platform         = " & mProject.Platform())
                .WriteLine(Space(3) & "Location         = " & mProject.Location())
                .WriteLine(Space(3) & "Customer PN      = " & mProject.CustomerPN())
                .WriteLine(Space(3) & "Parker PN        = " & mProject.ParkerPN())
                .WriteLine(Space(3) & "Unit System      = " & mProject.UnitSystem) ' Unit_In.System)

                '........Analysis Desc.
                Dim pAnaDesc As String = ""
                Dim pMCS As String = mSeal.MCrossSecNo
                If (pMCS <> "") Then
                    pAnaDesc = "MCS" & pMCS
                End If
                Dim pFreeHt As String = Unit_In.WriteInUserL(mSeal.Hfree).ToString().Replace(".", "")
                If (pFreeHt <> "") Then
                    pAnaDesc = pAnaDesc & "_FH" & pFreeHt
                End If
                Dim pLoadCaseName As String = mLoadCase.Name
                If (pLoadCaseName <> "") Then
                    pAnaDesc = pAnaDesc & "_" & pLoadCaseName
                End If
                Dim pDate As String = ""
                Dim pTime As String = ""
                If (mDateCreated <> Date.MinValue) Then
                    pDate = mDateCreated.ToString("ddMMMyy")
                    pTime = mTimeCreated.ToString("t").Replace(":", "").Trim().Replace(" ", "")
                    pAnaDesc = pAnaDesc & "_" & pDate & "_" & pTime
                End If

                .WriteLine(Space(3) & "Analysis Desc.   = " & pAnaDesc)
                .WriteLine(Space(3) & "Date             = " & Format(Today, "dd-MMM-yy"))
                .WriteLine()

                'Cavity Dimensions & Movements:
                '------------------------------------------
                .WriteLine("Cavity Dimensions & Movements: ")
                .WriteLine(Space(3) & "Cavity Min OD   = " & Unit_In.WriteInUserL(mCavity.Dia(2)))
                .WriteLine(Space(3) & "Cavity Max ID   = " & Unit_In.WriteInUserL(mCavity.Dia(1)))

                .WriteLine(Space(3) & "Width           = " & Unit_In.WriteInUserL(mCavity.WidMin))
                .WriteLine(Space(3) & "Corner Rad      = " & Unit_In.WriteInUserL(mCavity.CornerRad))
                .WriteLine(Space(3) & "Cavity Depth    = " & Unit_In.WriteInUserL(mCavity.Depth))

                .WriteLine(Space(3) & "Depth Tol  (-)  = " & Unit_In.WriteInUserL(mCavity.DepthTol(1), "TFormat"))
                .WriteLine(Space(3) & "Depth Tol  (+)  = " & Unit_In.WriteInUserL(mCavity.DepthTol(2), "TFormat"))
                .WriteLine()

                'Operating Conditions:
                '------------------------------
                .WriteLine("Operating Conditions: ")

                Dim pPDiffUnitUser As String = Unit_In.FormatPDiffUnitUser(mOpCond.PDiff)

                .WriteLine(Space(3) & "Pressure        = " & pPDiffUnitUser & Space(2) & Unit_In.UserP)
                .WriteLine(Space(3) & "Temp            = " & NInt(mOpCond.TOper))
                .WriteLine(Space(3) & "POrient         = " & mOpCond.POrient)
                .WriteLine()

                'Applied Loading:
                '-----------------------
                .WriteLine("Applied Loading: ")

                Dim pPreCompressed As String = ""
                If (mAppLoad.PreComp.Exists) Then
                    pPreCompressed = "Y"
                Else
                    pPreCompressed = "N"
                End If

                .WriteLine(Space(3) & "PreCompressed      = " & pPreCompressed)
                .WriteLine(Space(3) & "PreCompressed_HMin = " & mAppLoad.PreComp.HMin)

                Dim pRadConstraint As String = ""
                If (mAppLoad.RadConstraint) Then
                    pRadConstraint = "Y"
                Else
                    pRadConstraint = "N"
                End If
                .WriteLine(Space(3) & "Radial Constraint  = " & pRadConstraint)
                .WriteLine()

                If (mAppLoad.AddLoad.Count > 0) Then

                    .WriteLine("Additional Load Steps: " & mAppLoad.AddLoad.Count)

                    For i As Integer = 0 To mAppLoad.AddLoad.Count - 1
                        .WriteLine(Space(3) & "A" & (i + 1).ToString() & ":")
                        .WriteLine(Space(6) & "Pressure     = " & Unit_In.FormatPDiffUnitUser(mAppLoad.AddLoad(i).PDiff) & Space(2) & Unit_In.UserP)
                        .WriteLine(Space(6) & "Temp         = " & (mAppLoad.AddLoad(i).TOper))
                        .WriteLine(Space(6) & "Cavity Depth = " & Unit_In.WriteInUserL(mAppLoad.AddLoad(i).CavityDepth))
                        .WriteLine()
                    Next

                End If


                If mSeal IsNot Nothing Then

                    'Seal Design:
                    '------------   
                    .WriteLine("Seal Design: ")
                    .WriteLine(Space(3) & "Type         = " & mSeal.Type)

                    If mSeal.Type = "E-Seal" Then
                        If CType(mSeal, IPE_clsESeal).NewDesign Then
                            .WriteLine(Space(3) & "MCS          = " & mSeal.MCrossSecNo & " (New)" & _
                                                                   Space(3) & CType(mSeal, IPE_clsESeal).CrossSecNoOrg & " (Original)")
                        Else
                            .WriteLine(Space(3) & "MCS          = " & mSeal.MCrossSecNo)
                        End If

                    ElseIf mSeal.Type = "C-Seal" Then
                        .WriteLine(Space(3) & "MCS          = " & mSeal.MCrossSecNo)

                    ElseIf mSeal.Type = "U-Seal" Then
                        If CType(mSeal, IPE_clsUSeal).NewDesign Then
                            .WriteLine(Space(3) & "MCS          = " & mSeal.MCrossSecNo & " (New)" & _
                                                                   Space(3) & CType(mSeal, IPE_clsUSeal).CrossSecNoOrg & " (Original)")
                        Else
                            .WriteLine(Space(3) & "MCS          = " & mSeal.MCrossSecNo)
                        End If

                    End If


                    Dim pSegmented As String = ""

                    If (mSeal.IsSegmented) Then
                        pSegmented = "Y"
                        .WriteLine(Space(3) & "Segmented    = " & pSegmented & vbTab & Space(5) & "Count   = " & mSeal.CountSegment)
                    Else
                        pSegmented = "N"
                        .WriteLine(Space(3) & "Segmented    = " & pSegmented & vbTab & Space(5) & "Count   = 0")
                    End If

                    '....Material related:
                    '

                    If mSeal.Type = "E-Seal" Then
                        '--------------------------------------------------------------------------------------

                        If mSeal.Mat.Coating = "T800" Then
                            .WriteLine(Space(3) & "Material     = " & mSeal.Mat.Name & _
                                       Space(3) & "HT      = " & mSeal.Mat.HT & _
                                       Space(3) & "Coating = " & mSeal.Mat.Coating & _
                                       Space(4) & "(RMS = " & CType(mSeal, IPE_clsESeal).SFinish & ")")

                        Else        '....Coating = "None" & "Tricom".
                            .WriteLine(Space(3) & "Material     = " & mSeal.Mat.Name & _
                                       Space(3) & "HT      = " & mSeal.Mat.HT & _
                                       Space(4) & "Coating = " & mSeal.Mat.Coating)
                        End If


                    ElseIf mSeal.Type = "C-Seal" Then
                        '-------------------------------------------------------------------------------------------

                        .WriteLine(Space(3) & "Material     = " & mSeal.Mat.Name & _
                                   Space(3) & "Plating = " & CType(mSeal, IPE_clsCSeal).Plating.Code)

                    Else
                        .WriteLine(Space(3) & "Material     = " & mSeal.Mat.Name)

                    End If

                    .WriteLine(Space(3) & "Comp. Tol    = " & mCompression.TolType)     'AES 26SEP16
                    .WriteLine(Space(3) & "Control Dia  = " & Unit_In.WriteInUserL(mSeal.DControl))
                    .WriteLine(Space(3) & "Free Height  = " & Unit_In.WriteInUserL(mSeal.Hfree))

                    .WriteLine(Space(3) & "Seal Tol (-) = " & Unit_In.WriteInUserL(mSeal.HFreeTol(1), "TFormat"))
                    .WriteLine(Space(3) & "Seal Tol (+) = " & Unit_In.WriteInUserL(mSeal.HFreeTol(2), "TFormat"))

                    .WriteLine(Space(3) & "Seal Width   = " & Unit_In.WriteInUserL((mSeal.Wid)))


                    .WriteLine(Space(3) & "ZClear       = " & Unit_In.WriteInUserL((mSeal.ZClear_Given)))
                    .WriteLine()


                    '....Geometry Adjustment Parameters:
                    '
                    .WriteLine(Space(3) & "Adjusted       = " & mSeal.Adjusted)

                    If mSeal.Type = "E-Seal" Then
                        '---------------------------
                        .WriteLine(Space(3) & " dthetaE1      = " & CType(mSeal, IPE_clsESeal).DThetaE1)
                        .WriteLine(Space(3) & " dthetaM1      = " & CType(mSeal, IPE_clsESeal).DThetaM1)


                    ElseIf mSeal.Type = "C-Seal" Then
                        '------------------------------             
                        Dim pstrDHfree_UserL As String
                        pstrDHfree_UserL = Unit_In.WriteInUserL(CType(mSeal, IPE_clsCSeal).DHfree, "TFormat")
                        .WriteLine(Space(3) & " DHfree        = " & pstrDHfree_UserL)

                        .WriteLine(Space(3) & " DThetaOpening = " & CType(mSeal, IPE_clsCSeal).DThetaOpening)
                    End If
                    .WriteLine()


                    'FEA Parameters:       
                    '---------------

                    '....General Parameters:
                    '
                    .WriteLine("FEA Parameters: General")
                    .WriteLine(Space(3) & "ANSYS Version        = " & ANSYS_In.Version)
                    .WriteLine(Space(3) & "Model                = " & ANSYS_In.Model)
                    .WriteLine(Space(3) & "Max No. of Elements  = " & ANSYS_In.NelMax)
                    .WriteLine(Space(3) & "Solve                = " & ANSYS_In.Solve)
                    .WriteLine(Space(3) & "Run Type             = " & ANSYS_In.RunType)
                    .WriteLine(Space(3) & "Load Case            = " & mLoadCase.Name)
                    .WriteLine()

                    '....Seal Specific Paramters:
                    '
                    .WriteLine("FEA Parameters: " & mSeal.Type)
                    .WriteLine(Space(3) & "NLayer         = " & mSeal.NLayer)

                    If mSeal.Type = "C-Seal" Then

                        If CType(mSeal, IPE_clsCSeal).Plating.Exists = True Then
                            .WriteLine(Space(3) & "NLayer_Plating = " & CType(mSeal, IPE_clsCSeal).Plating.NLayer)
                        Else
                            .WriteLine(Space(3) & "NLayer_Plating = 0")

                        End If

                    End If

                    .WriteLine()

                    '....# of Contact Elements
                    .WriteLine(Space(3) & "NelConSeal     = " & mSeal.NelConSeal)


                    If mSeal.Type = "E-Seal" Then
                        '---------------------------
                        .WriteLine(Space(3) & "NelConMid      = " & CType(mSeal, IPE_clsESeal).NelConMid)
                        .WriteLine(Space(3) & "NelConHeel     = " & CType(mSeal, IPE_clsESeal).NelConHeel)
                        'End If

                    ElseIf mSeal.Type = "U-Seal" Then
                        '------------------------------
                        .WriteLine(Space(3) & "NelConHeel     = " & CType(mSeal, IPE_clsUSeal).NelConHeel)
                    End If

                    .WriteLine(Space(3) & "NelConRad      = " & mSeal.NelConRad)
                    .WriteLine()

                    .WriteLine(Space(3) & "FacKN    = " & mSeal.FacKN)
                    .WriteLine(Space(3) & "FacTOLN  = " & mSeal.FacTOLN)

                    .WriteLine()


                    If mSeal.Type = "E-Seal" Then
                        '---------------------------
                        '....Element Density Weightage on Segments
                        .WriteLine(Space(3) & "WtE1     = " & CType(mSeal, IPE_clsESeal).WtE(1))
                        .WriteLine(Space(3) & "WtE2     = " & CType(mSeal, IPE_clsESeal).WtE(2))
                        .WriteLine(Space(3) & "WtE3     = " & CType(mSeal, IPE_clsESeal).WtE(3))

                        .WriteLine(Space(3) & "WtM1     = " & CType(mSeal, IPE_clsESeal).WtM(1))
                        .WriteLine(Space(3) & "WtM2     = " & CType(mSeal, IPE_clsESeal).WtM(2))
                        .WriteLine(Space(3) & "WtM3     = " & CType(mSeal, IPE_clsESeal).WtM(3))
                        .WriteLine()
                    End If

                    '....Thickness Direction
                    .WriteLine(Space(3) & "BetaT    = " & mSeal.BetaT)
                    .WriteLine()

                    '....Load Sub Step Definitions:
                    .WriteLine(Space(3) & "NSBSTP   = " & mSeal.NSBSTP)
                    .WriteLine(Space(3) & "NSBMX    = " & mSeal.NSBMX)
                    .WriteLine(Space(3) & "NSBMN    = " & mSeal.NSBMN)
                    .WriteLine()

                End If

            End With

        Catch pEXP As IOException
            '....ERROR HANDLER            
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
                                                                 MessageBoxIcon.Error)
            Exit Sub

        Catch pEXP As Exception
            '....ERROR HANDLER
            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes, pintAnswer As Short

            pstrTitle = "ERROR MESSAGE:  Output Data File Writing"
            pstrMsg = "Error in file writing!!" & vbCrLf
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly

            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

            Exit Sub

        Finally

            pSW.Close()

            Process.Start(pFileName)

        End Try

    End Sub

    'AES 22MAR17
    Public Function IsCavityNull() As Boolean
        '=====================================
        If (mCavity.Dia(0) <> 0.0 And mCavity.Dia(1) <> 0.0 And mCavity.Depth <> 0.0) Then
            Return False
        Else
            Return True
        End If
    End Function

    'AES 22MAR17
    Public Function IsOpCondNull() As Boolean
        '=====================================
        If (mOpCond.PDiff <> 0.0 And mOpCond.TOper <> 0.0) Then
            Return False
        Else
            Return True
        End If
    End Function

    'AES 22MAR17
    Public Function IsSealNull() As Boolean
        '=====================================
        If (mSeal.MCrossSecNo <> "") Then
            Return False
        Else
            Return True
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
        Try


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

        Catch ex As Exception

        End Try

    End Function

#End Region

    Protected Overrides Sub Finalize()
        '=============================
        MyBase.Finalize()
    End Sub
End Class
