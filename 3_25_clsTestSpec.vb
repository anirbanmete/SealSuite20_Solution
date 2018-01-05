'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_Spec                           '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07DEC16                                '
'                                                                              '
'===============================================================================

Imports System.Linq

<Serializable()> _
Public Class Test_clsSpec

    Enum eLoadType
        Min
        Max
        Range
    End Enum


#Region "MEMBER VARIABLE DECLARATIONS:"
    Protected mTestProject As Test_clsProject
    Private mPOrient As String

    Private mSealQty As Integer = 1
    Private mLeak As sLeak
    Private mLoad As sLoad
    Private mIsPress As Boolean

    Private mSealFHIni(1) As sSealFHIni
    Private mSealODPre(1) As sSealODPre
    Private mSealIDPre(1) As sSealIDPre

    Private mLeakCavityDepth As Double
    Private mLeakProcedureFile As String
    Private mLeakMedium As String
    Private mLeakPress As Double
    Private mLeakMax As sLeakMax
    Private mLeakSpringBackMin As sLeakSpringBackMin

    Private mLoadProcedureFile As String
    Private mLoadType As eLoadType = eLoadType.Range
    Private mLoadMin_CavityDepth As Double
    Private mLoadRange_CavityDepth As Double
    Private mLoadMax_CavityDepth As Double
    Private mLoadVal(2) As Double
    Private mLoadSpringBackMin As sLoadSpringBackMin

    Private mResult As sResultIPE

#End Region


#Region "PROPERTY ROUTINES:"
    '======================

    '....SealQty
    Public Property SealQty() As Integer
        '================================
        Get
            Return mSealQty
        End Get

        Set(ByVal value As Integer)
            mSealQty = value
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


    '....SealFHIni
    Public ReadOnly Property SealFHIni(ByVal i As Integer) As sSealFHIni
        '===============================================================
        Get
            Return mSealFHIni(i)
        End Get
    End Property

    Public WriteOnly Property SealFHIni_Unplated(ByVal i As Integer) As Double
        '=====================================================================
        Set(ByVal value As Double)
            mSealFHIni(i).Unplated = value
        End Set
    End Property

    Public WriteOnly Property SealFHIni_Plated(ByVal i As Integer) As Double
        '===================================================================
        Set(ByVal value As Double)
            mSealFHIni(i).Plated = value
        End Set
    End Property


    '....SealODPre
    Public ReadOnly Property SealODPre(ByVal i As Integer) As sSealODPre
        '===============================================================
        Get
            Return mSealODPre(i)
        End Get
    End Property

    Public WriteOnly Property SealODPre_Unplated(ByVal i As Integer) As Double
        '=====================================================================
        Set(ByVal value As Double)
            mSealODPre(i).Unplated = value
        End Set
    End Property

    Public WriteOnly Property SealODPre_Plated(ByVal i As Integer) As Double
        '===================================================================
        Set(ByVal value As Double)
            mSealODPre(i).Plated = value
        End Set
    End Property

    '....SealIDPre
    Public ReadOnly Property SealIDPre(ByVal i As Integer) As sSealIDPre
        '===============================================================
        Get
            Return mSealIDPre(i)
        End Get
    End Property

    Public WriteOnly Property SealIDPre_Unplated(ByVal i As Integer) As Double
        '=====================================================================
        Set(ByVal value As Double)
            mSealIDPre(i).Unplated = value
        End Set
    End Property

    Public WriteOnly Property SealIDPre_Plated(ByVal i As Integer) As Double
        '===================================================================
        Set(ByVal value As Double)
            mSealIDPre(i).Plated = value
        End Set
    End Property


    '....LeakCavityDepth
    Public Property LeakCavityDepth As Double
        '======================================
        Get
            Return mLeakCavityDepth
        End Get

        Set(ByVal value As Double)
            mLeakCavityDepth = value
        End Set

    End Property


    '....LeakProcedureFile
    Public Property LeakProcedureFile() As String
        '========================================
        Get
            Return mLeakProcedureFile
        End Get

        Set(ByVal value As String)
            mLeakProcedureFile = value
        End Set

    End Property


    '....LeakMedium
    Public Property LeakMedium() As String
        '==================================
        Get
            Return mLeakMedium
        End Get

        Set(ByVal value As String)
            mLeakMedium = value
        End Set

    End Property


    '....LeakPress
    Public Property LeakPress() As Double
        '==================================
        Get
            Return mLeakPress
        End Get

        Set(ByVal value As Double)
            mLeakPress = value
        End Set

    End Property


    '....LeakMax
    Public ReadOnly Property LeakMax() As sLeakMax
        '=========================================
        Get
            Return mLeakMax
        End Get
    End Property

    Public WriteOnly Property LeakMax_Unplated() As Double
        '=================================================
        Set(ByVal value As Double)
            mLeakMax.Unplated = value
        End Set
    End Property

    Public WriteOnly Property LeakMax_Plated() As Double
        '===============================================
        Set(ByVal value As Double)
            mLeakMax.Plated = value
        End Set
    End Property


    '....LeakSpringBackMin
    Public ReadOnly Property LeakSpringBackMin() As sLeakSpringBackMin
        '=============================================================
        Get
            Return mLeakSpringBackMin
        End Get
    End Property

    Public WriteOnly Property LeakSpringBackMin_Unplated() As Double
        '=================================================
        Set(ByVal value As Double)
            mLeakSpringBackMin.Unplated = value
        End Set
    End Property

    Public WriteOnly Property LeakSpringBackMin_Plated() As Double
        '===============================================
        Set(ByVal value As Double)
            mLeakSpringBackMin.Plated = value
        End Set
    End Property


    '....LoadProcedureFile
    Public Property LoadProcedureFile() As String
        '========================================
        Get
            Return mLoadProcedureFile
        End Get

        Set(ByVal value As String)
            mLoadProcedureFile = value
        End Set

    End Property


    '....LoadType
    Public Property LoadType() As eLoadType
        '================================
        Get
            Return mLoadType
        End Get

        Set(ByVal value As eLoadType)
            mLoadType = value
        End Set

    End Property


    '....LoadMin_CavityDepth
    Public Property LoadMin_CavityDepth As Double
        '=========================================
        Get
            Return mLoadMin_CavityDepth
        End Get

        Set(ByVal value As Double)
            mLoadMin_CavityDepth = value
        End Set

    End Property


    '....LoadRange_CavityDepth
    Public Property LoadRange_CavityDepth As Double
        '==========================================
        Get
            Return mLoadRange_CavityDepth
        End Get

        Set(ByVal value As Double)
            mLoadRange_CavityDepth = value
        End Set

    End Property


    '....LoadMax_CavityDepth
    Public Property LoadMax_CavityDepth As Double
        '========================================
        Get
            Return mLoadMax_CavityDepth
        End Get

        Set(ByVal value As Double)
            mLoadMax_CavityDepth = value
        End Set

    End Property


    '....LoadVal
    Public Property LoadVal(ByVal i As Integer) As Double
        '=============================================
        Get
            Return mLoadVal(i)
        End Get

        Set(ByVal value As Double)
            mLoadVal(i) = value
        End Set

    End Property


    '....LoadSpringBackMin
    Public ReadOnly Property LoadSpringBackMin() As sLoadSpringBackMin
        '=============================================================
        Get
            Return mLoadSpringBackMin
        End Get
    End Property

    Public WriteOnly Property LoadSpringBackMin_Unplated() As Double
        '=================================================
        Set(ByVal value As Double)
            mLoadSpringBackMin.Unplated = value
        End Set
    End Property

    Public WriteOnly Property LoadSpringBackMin_Plated() As Double
        '===============================================
        Set(ByVal value As Double)
            mLoadSpringBackMin.Plated = value
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

#Region "Result:"

    '....LeakResult
    Public ReadOnly Property Result() As sResultIPE
        '==========================================
        Get
            Return mResult
        End Get
    End Property


    '....Result_DSealing
    Public WriteOnly Property Result_DSealing() As Double
        '==================================================
        Set(ByVal Value As Double)
            mResult.DSealing = Value
        End Set

    End Property

    '....Result_UnitLoad_Leak
    Public WriteOnly Property Result_Leak_UnitLoad() As Double
        '==================================================
        Set(ByVal Value As Double)
            mResult.Leak_UnitLoad = Value
        End Set

    End Property


    '....Result_FHFinal_Leak
    Public WriteOnly Property Result_Leak_FHFinal() As Double
        '====================================================
        Set(ByVal Value As Double)
            mResult.Leak_FHFinal = Value
        End Set

    End Property


    '....Result_UnitLoad_Load
    Public WriteOnly Property Result_Load_UnitLoad() As Double
        '==================================================
        Set(ByVal Value As Double)
            mResult.Load_UnitLoad = Value
        End Set

    End Property


    '....Result_FHFinal_Load
    Public WriteOnly Property Result_Load_FHFinal() As Double
        '====================================================
        Set(ByVal Value As Double)
            mResult.Load_FHFinal = Value
        End Set

    End Property

#End Region


#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(ByVal TestProject_In As Test_clsProject)
        '==================================================
        mTestProject = TestProject_In

    End Sub

#End Region


#Region "DB RELATED ROUTINES"

    Public Sub RetrieveFrom_DB(ByVal TestProjectID_In As Integer)
        '========================================================
        Try

            Dim pSealTestEntities As New SealTestDBEntities

            Dim pQrySpec = (From pRec In pSealTestEntities.tblSpec
                             Where pRec.fldTestProjectID = TestProjectID_In Select pRec).ToList()

            If (pQrySpec.Count > 0) Then

                mSealQty = pQrySpec(0).fldSealQty
                mLeak.Exists = pQrySpec(0).fldIsLeak
                mLoad.Exists = pQrySpec(0).fldIsLoad
                mIsPress = pQrySpec(0).fldIsPressure

                If (Not IsDBNull(pQrySpec(0).fldSealFHIniMin_Unplated) And (Not IsNothing(pQrySpec(0).fldSealFHIniMin_Unplated))) Then
                    mSealFHIni(0).Unplated = pQrySpec(0).fldSealFHIniMin_Unplated
                Else
                    mSealFHIni(0).Unplated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealFHIniMax_Unplated) And (Not IsNothing(pQrySpec(0).fldSealFHIniMax_Unplated))) Then
                    mSealFHIni(1).Unplated = pQrySpec(0).fldSealFHIniMax_Unplated
                Else
                    mSealFHIni(1).Unplated = 0
                End If


                If (Not IsDBNull(pQrySpec(0).fldSealODPreMin_Unplated) And (Not IsNothing(pQrySpec(0).fldSealODPreMin_Unplated))) Then
                    mSealODPre(0).Unplated = pQrySpec(0).fldSealODPreMin_Unplated
                Else
                    mSealODPre(0).Unplated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealODPreMax_Unplated) And (Not IsNothing(pQrySpec(0).fldSealODPreMax_Unplated))) Then
                    mSealODPre(1).Unplated = pQrySpec(0).fldSealODPreMax_Unplated
                Else
                    mSealODPre(1).Unplated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealIDPreMin_Unplated) And (Not IsNothing(pQrySpec(0).fldSealIDPreMin_Unplated))) Then
                    mSealIDPre(0).Unplated = pQrySpec(0).fldSealIDPreMin_Unplated
                Else
                    mSealIDPre(0).Unplated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealIDPreMax_Unplated) And (Not IsNothing(pQrySpec(0).fldSealIDPreMax_Unplated))) Then
                    mSealIDPre(1).Unplated = pQrySpec(0).fldSealIDPreMax_Unplated
                Else
                    mSealIDPre(1).Unplated = 0
                End If

                '---------
                If (Not IsDBNull(pQrySpec(0).fldSealFHIniMin_Plated) And (Not IsNothing(pQrySpec(0).fldSealFHIniMin_Plated))) Then
                    mSealFHIni(0).Plated = pQrySpec(0).fldSealFHIniMin_Plated
                Else
                    mSealFHIni(0).Plated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealFHIniMax_Plated) And (Not IsNothing(pQrySpec(0).fldSealFHIniMax_Plated))) Then
                    mSealFHIni(1).Plated = pQrySpec(0).fldSealFHIniMax_Plated
                Else
                    mSealFHIni(1).Plated = 0
                End If

                'mSealFHIni(0).Plated = pQrySpec(0).fldSealFHIniMin_Plated
                'mSealFHIni(1).Plated = pQrySpec(0).fldSealFHIniMax_Plated

                If (Not IsDBNull(pQrySpec(0).fldSealODPreMin_Plated) And (Not IsNothing(pQrySpec(0).fldSealODPreMin_Plated))) Then
                    mSealODPre(0).Plated = pQrySpec(0).fldSealODPreMin_Plated
                Else
                    mSealODPre(0).Plated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealODPreMax_Plated) And (Not IsNothing(pQrySpec(0).fldSealODPreMax_Plated))) Then
                    mSealODPre(1).Plated = pQrySpec(0).fldSealODPreMax_Plated
                Else
                    mSealODPre(1).Plated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealIDPreMin_Plated) And (Not IsNothing(pQrySpec(0).fldSealIDPreMin_Plated))) Then
                    mSealIDPre(0).Plated = pQrySpec(0).fldSealIDPreMin_Plated
                Else
                    mSealIDPre(0).Plated = 0
                End If

                If (Not IsDBNull(pQrySpec(0).fldSealIDPreMax_Plated) And (Not IsNothing(pQrySpec(0).fldSealIDPreMax_Plated))) Then
                    mSealIDPre(1).Plated = pQrySpec(0).fldSealIDPreMax_Plated
                Else
                    mSealIDPre(1).Plated = 0
                End If

                '---------
                mLeakCavityDepth = pQrySpec(0).fldLeakCavityDepth
                If (Not IsDBNull(pQrySpec(0).fldLeakProcedure_FileName) And (Not IsNothing(pQrySpec(0).fldLeakProcedure_FileName))) Then
                    mLeakProcedureFile = pQrySpec(0).fldLeakProcedure_FileName
                Else
                    mLeakProcedureFile = ""
                End If

                If (Not IsDBNull(pQrySpec(0).fldLeakMedium) And (Not IsNothing(pQrySpec(0).fldLeakMedium))) Then
                    mLeakMedium = pQrySpec(0).fldLeakMedium
                Else
                    mLeakMedium = ""
                End If

                'mLeakMedium = pQrySpec(0).fldLeakMedium

                If (Not IsDBNull(pQrySpec(0).fldLeakPress) And (Not IsNothing(pQrySpec(0).fldLeakPress))) Then
                    mLeakPress = pQrySpec(0).fldLeakPress
                Else
                    mLeakPress = 0
                End If

                'mLeakPress = pQrySpec(0).fldLeakPress

                If (Not IsDBNull(pQrySpec(0).fldIsLeak_Leakage) And (Not IsNothing(pQrySpec(0).fldIsLeak_Leakage))) Then
                    mLeak.Leakage = pQrySpec(0).fldIsLeak_Leakage
                End If

                If (Not IsDBNull(pQrySpec(0).fldLeakMax_Unplated) And (Not IsNothing(pQrySpec(0).fldLeakMax_Unplated))) Then
                    mLeakMax.Unplated = pQrySpec(0).fldLeakMax_Unplated
                End If

                If (Not IsDBNull(pQrySpec(0).fldLeakMax_Plated) And (Not IsNothing(pQrySpec(0).fldLeakMax_Plated))) Then
                    mLeakMax.Plated = pQrySpec(0).fldLeakMax_Plated
                End If


                If (Not IsDBNull(pQrySpec(0).fldIsLeakSpringBack) And (Not IsNothing(pQrySpec(0).fldIsLeakSpringBack))) Then
                    mLeak.Springback = pQrySpec(0).fldIsLeakSpringBack
                End If

                If (Not IsDBNull(pQrySpec(0).fldLeakSpringBackMin_Unplated) And (Not IsNothing(pQrySpec(0).fldLeakSpringBackMin_Unplated))) Then
                    mLeakSpringBackMin.Unplated = pQrySpec(0).fldLeakSpringBackMin_Unplated
                End If

                If (Not IsDBNull(pQrySpec(0).fldLeakSpringBackMin_Plated) And (Not IsNothing(pQrySpec(0).fldLeakSpringBackMin_Plated))) Then
                    mLeakSpringBackMin.Plated = pQrySpec(0).fldLeakSpringBackMin_Plated
                End If

                If (Not IsDBNull(pQrySpec(0).fldLoadProcedure_FileName) And (Not IsNothing(pQrySpec(0).fldLoadProcedure_FileName))) Then
                    mLoadProcedureFile = pQrySpec(0).fldLoadProcedure_FileName
                Else
                    mLoadProcedureFile = ""
                End If

                mLoadType = pQrySpec(0).fldLoadType
                mLoadMin_CavityDepth = pQrySpec(0).fldLoadMin_CavityDepth
                mLoadRange_CavityDepth = pQrySpec(0).fldLoadRange_CavityDepth
                mLoadMax_CavityDepth = pQrySpec(0).fldLoadMax_CavityDepth
                mLoadVal(0) = pQrySpec(0).fldLoadMin
                mLoadVal(1) = pQrySpec(0).fldLoadMax
                mLoad.Springback = pQrySpec(0).fldIsLoadSpringBack

                If (Not IsDBNull(pQrySpec(0).fldLoadSpringBackMin_Unplated) And (Not IsNothing(pQrySpec(0).fldLoadSpringBackMin_Unplated))) Then
                    mLoadSpringBackMin.Unplated = pQrySpec(0).fldLoadSpringBackMin_Unplated
                End If

                If (Not IsDBNull(pQrySpec(0).fldLoadSpringBackMin_Plated) And (Not IsNothing(pQrySpec(0).fldLoadSpringBackMin_Plated))) Then
                    mLoadSpringBackMin.Plated = pQrySpec(0).fldLoadSpringBackMin_Plated
                End If


                '....Result
                Dim pQryResults = (From pRec In pSealTestEntities.tblResult_FEA
                             Where pRec.fldTestProjectID = TestProjectID_In Select pRec).ToList()

                If (pQryResults.Count > 0) Then
                    mResult.DSealing = pQryResults(0).fldDSealing
                    mResult.Leak_UnitLoad = pQryResults(0).fldLeak_UnitLoad
                    mResult.Leak_FHFinal = pQryResults(0).fldLeak_FHFinal
                    mResult.Load_UnitLoad = pQryResults(0).fldLoad_UnitLoad
                    mResult.Load_FHFinal = pQryResults(0).fldLoad_FHFinal
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Public Sub SaveTo_DB()
        '=================
        Dim pSealTestEntities As New SealTestDBEntities

        Dim pQrySpec = (From pRec In pSealTestEntities.tblSpec
                         Where pRec.fldTestProjectID = mTestProject.ID Select pRec).ToList()

        Dim pSpec As New tblSpec

        If (pQrySpec.Count() > 0) Then
            pSpec = pQrySpec(0)
        End If

        With pSpec
            .fldTestProjectID = mTestProject.ID
            .fldSealQty = mSealQty
            .fldIsLeak = mLeak.Exists
            .fldIsLoad = mLoad.Exists
            .fldIsPressure = mIsPress

            .fldSealFHIniMin_Unplated = mSealFHIni(0).Unplated
            .fldSealFHIniMin_Plated = mSealFHIni(0).Plated

            .fldSealFHIniMax_Unplated = mSealFHIni(1).Unplated
            .fldSealFHIniMax_Plated = mSealFHIni(1).Plated

            If (mTestProject.PartProject.PNR.HW.POrient = "Internal") Then
                .fldSealODPreMin_Unplated = mSealODPre(0).Unplated
                .fldSealODPreMin_Plated = mSealODPre(0).Plated

                .fldSealODPreMax_Unplated = mSealODPre(1).Unplated
                .fldSealODPreMax_Plated = mSealODPre(1).Plated

                .fldSealIDPreMin_Unplated = 0
                .fldSealIDPreMin_Plated = 0

                .fldSealIDPreMax_Unplated = 0
                .fldSealIDPreMax_Plated = 0

            ElseIf (mTestProject.PartProject.PNR.HW.POrient = "External") Then
                .fldSealODPreMin_Unplated = 0
                .fldSealODPreMin_Plated = 0

                .fldSealODPreMax_Unplated = 0
                .fldSealODPreMax_Plated = 0

                .fldSealIDPreMin_Unplated = mSealIDPre(0).Unplated
                .fldSealIDPreMin_Plated = mSealIDPre(0).Plated

                .fldSealIDPreMax_Unplated = mSealIDPre(1).Unplated
                .fldSealIDPreMax_Plated = mSealIDPre(1).Plated

            End If

            .fldLeakCavityDepth = mLeakCavityDepth
            .fldLeakProcedure_FileName = mLeakProcedureFile
            .fldLeakMedium = mLeakMedium
            .fldLeakPress = mLeakPress
            .fldIsLeak_Leakage = mLeak.Leakage
            .fldLeakMax_Unplated = mLeakMax.Unplated
            .fldLeakMax_Plated = mLeakMax.Plated

            .fldIsLeakSpringBack = mLeak.Springback
            .fldLeakSpringBackMin_Unplated = mLeakSpringBackMin.Unplated
            .fldLeakSpringBackMin_Plated = mLeakSpringBackMin.Plated

            .fldLoadProcedure_FileName = mLoadProcedureFile
            .fldLoadType = mLoadType
            .fldLoadMin_CavityDepth = mLoadMin_CavityDepth
            .fldLoadRange_CavityDepth = mLoadRange_CavityDepth
            .fldLoadMax_CavityDepth = mLoadMax_CavityDepth
            .fldLoadMin = mLoadVal(0)
            .fldLoadMax = mLoadVal(1)
            .fldIsLoadSpringBack = mLoad.Springback
            .fldLoadSpringBackMin_Unplated = mLoadSpringBackMin.Unplated
            .fldLoadSpringBackMin_Plated = mLoadSpringBackMin.Plated

        End With

        If (pQrySpec.Count() > 0) Then
            pSealTestEntities.SaveChanges()
        Else
            pSealTestEntities.AddTotblSpec(pSpec)
            pSealTestEntities.SaveChanges()
        End If

        '....Result
        Dim pQryResults = (From pRec In pSealTestEntities.tblResult_FEA
                     Where pRec.fldTestProjectID = mTestProject.ID Select pRec).ToList()

        Dim pResults As New tblResult_FEA

        If (pQryResults.Count() > 0) Then
            pResults = pQryResults(0)
        End If

        With pResults
            .fldTestProjectID = mTestProject.ID
            .fldDSealing = mResult.DSealing
            .fldLeak_UnitLoad = mResult.Leak_UnitLoad
            .fldLeak_FHFinal = mResult.Leak_FHFinal
            .fldLoad_UnitLoad = mResult.Load_UnitLoad
            .fldLoad_FHFinal = mResult.Load_FHFinal
        End With

        If (pQryResults.Count() > 0) Then
            pSealTestEntities.SaveChanges()
        Else
            pSealTestEntities.AddTotblResult_FEA(pResults)
            pSealTestEntities.SaveChanges()
        End If

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
    Public Structure sSealFHIni
        Public Unplated As Double
        Public Plated As Double
    End Structure

    <Serializable()> _
    Public Structure sSealODPre
        Public Unplated As Double
        Public Plated As Double
    End Structure

    <Serializable()> _
    Public Structure sSealIDPre
        Public Unplated As Double
        Public Plated As Double
    End Structure

    <Serializable()> _
    Public Structure sLeakMax
        Public Unplated As Double
        Public Plated As Double
    End Structure

    <Serializable()> _
    Public Structure sLeakSpringBackMin
        Public Unplated As Double
        Public Plated As Double
    End Structure

    <Serializable()> _
    Public Structure sLoadSpringBackMin
        Public Unplated As Double
        Public Plated As Double
    End Structure


    <Serializable()> _
    Public Structure sResultIPE
        Public DSealing As Double
        Public Leak_UnitLoad As Double
        Public Leak_FHFinal As Double
        Public Load_UnitLoad As Double
        Public Load_FHFinal As Double
    End Structure

#End Region

End Class
