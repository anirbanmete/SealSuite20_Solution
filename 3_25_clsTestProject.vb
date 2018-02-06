'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_Project                        '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  09JUN17                                '
'                                                                              '
'===============================================================================

Imports System.Threading
Imports System.Globalization
Imports System.Linq
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO


<Serializable()> _
Public Class Test_clsProject
    Implements ICloneable


#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mID As Integer

    Private mProjectID As Integer
    Private mPartProject As New clsPartProject

    'Private mPN As String
    'Private mRev As String
    'Private mCustName As String
    'Private mPN_Cust As String

    Private mTest_Unit As New Test_clsUnit

    Private mDate_ImportDesign As Date
    Private mAnalysis As IPE_clsAnalysis

    ''Private mAnalysisID_Sel As Integer
    ''Private mAnalysis_Sel As clsAnalysis

    ' ''AES 22MAR17
    ''Private mTestCavity As clsCavity
    ''Private mTestOpCond As New clsOpCond
    ''Private mTestSeal As clsSeal
    'Private mPart_HW As New clsPartProject.clsPNR.clsHW

    Private mTest_Spec As Test_clsSpec

    Private mUserAdmin As String
    Private mDateAdmin As DateTime
    Private mUserSignedOff As String
    Private mSignedOff As Boolean
    Private mDateSignedOff As DateTime = DateTime.MaxValue

    Private mTest_MO As New List(Of Test_clsMO)
    Private mSealIPE_FEA As Boolean = True

    'Private gfrmTestMain.MO_Sel As Integer          'PB29NOV16. May be in frmTestMain MO_Sel, Report_Sel
    'Private gfrmTestMain.Report_Sel As Integer      'PB29NOV16. May be in clsReport

    'AES 07JUN17
    Private mPN_Selected As String = ""
    Private mRev_Selected As String = ""

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


    '....ProjectID
    Public Property ProjectID() As Integer
        '=================================
        Get
            Return mProjectID
        End Get

        Set(ByVal value As Integer)
            mProjectID = value
        End Set

    End Property

    '....TestUnit
    Public Property Test_Unit() As Test_clsUnit
        '=================================
        Get
            Return mTest_Unit
        End Get

        Set(Obj As Test_clsUnit)
            mTest_Unit = Obj
        End Set

    End Property


    '....Date_ImportDesign
    Public Property Date_ImportDesign() As DateTime
        '===========================================
        Get
            Return mDate_ImportDesign
        End Get

        Set(ByVal value As DateTime)
            mDate_ImportDesign = value
        End Set

    End Property


    Public Property PartProject() As clsPartProject
        '===============================================
        Get
            Return mPartProject
        End Get
        Set(value As clsPartProject)
            mPartProject = value
        End Set
    End Property


    '....Analysis
    Public Property Analysis() As IPE_clsAnalysis
        '================================================
        Get
            Return mAnalysis
        End Get

        Set(ByVal value As IPE_clsAnalysis)
            mAnalysis = value
        End Set

    End Property

    '....Spec
    Public Property Test_Spec() As Test_clsSpec
        '=============================
        Get
            Return mTest_Spec
        End Get

        Set(Obj As Test_clsSpec)
            mTest_Spec = Obj
        End Set

    End Property


    '....UserAdmin
    Public Property UserAdmin() As String
        '=================================
        Get
            Return mUserAdmin
        End Get

        Set(ByVal value As String)
            mUserAdmin = value
        End Set

    End Property

    '....DateAdmin
    Public Property DateAdmin() As DateTime
        '===================================
        Get
            Return mDateAdmin
        End Get

        Set(ByVal value As DateTime)
            mDateAdmin = value
        End Set

    End Property

    '....UserSignedOf
    Public Property UserSignedOff() As String
        '=====================================
        Get
            Return mUserSignedOff
        End Get

        Set(ByVal value As String)
            mUserSignedOff = value
        End Set

    End Property

    '....SignedOff
    Public Property SignedOff() As Boolean
        '===================================
        Get
            Return mSignedOff
        End Get

        Set(ByVal value As Boolean)
            mSignedOff = value
        End Set

    End Property

    '....DateSignedOff
    Public Property DateSignedOff() As DateTime
        '=======================================
        Get
            Return mDateSignedOff
        End Get

        Set(ByVal value As DateTime)
            mDateSignedOff = value
        End Set

    End Property


    '....MO
    Public Property Test_MO() As List(Of Test_clsMO)
        '==================================
        Get
            Return mTest_MO
        End Get

        Set(ByVal Data As List(Of Test_clsMO))
            mTest_MO = Data
        End Set

    End Property



    '....SealIPE_FEA.    
    Public Property SealIPE_FEA() As Boolean
        '====================================
        Get
            Return mSealIPE_FEA
        End Get

        Set(ByVal pblnVal As Boolean)
            '--------------------------------
            mSealIPE_FEA = pblnVal
        End Set

    End Property


    '....PN Selected from Part
    Public Property PN_Selected() As String
        '===================================
        Get
            Return mPN_Selected
        End Get

        Set(ByVal value As String)
            mPN_Selected = value
        End Set

    End Property

    '....Rev Sealected from Part
    Public Property Rev_Selected() As String
        '===================================
        Get
            Return mRev_Selected
        End Get

        Set(ByVal value As String)
            mRev_Selected = value
        End Set

    End Property



#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(ByVal PartProject_In As clsPartProject)
        '=================================================
        mPartProject = PartProject_In
        mTest_Spec = New Test_clsSpec(Me)
    End Sub


#End Region

#Region "HELPER ROUTINES:"

    Public Function IsTesterSigned() As Boolean
        '======================================
        Dim pSigned As Boolean = False

        For i As Integer = 0 To mTest_MO.Count - 1
            For j As Integer = 0 To mTest_MO(i).Test_Report.Count - 1
                If (mTest_MO(i).Test_Report(j).Tester.Signed) Then
                    pSigned = True
                    Exit For
                End If
            Next
        Next

        Return pSigned

    End Function

#End Region

#Region "DB RELATED ROUTINES:"

    'Public Sub RetrieveFromHW(ByVal PNID_In As Integer, ByVal RevID_In As Integer)
    '    '=========================================================================
    '    'mPart_HW = New clsPartProject.clsPNR.clsHW

    '    Dim pSealPartFile As New clsPartFile()
    '    Dim pMatList_Prop As New List(Of String)
    '    pMatList_Prop = pSealPartFile.MatList_Prop

    '    Dim pSealPart_Project As New clsPartProject()
    '    Dim pPNID As Integer = PNID_In
    '    Dim pRevID As Integer = RevID_In

    '    Dim pPartEntities As New SealPartDBEntities()

    '    '....HW_Face table
    '    Dim pHWFace_Rec_Count As Integer = (From HWFace In pPartEntities.tblHW_Face
    '                                        Where HWFace.fldPNID = pPNID And
    '                                        HWFace.fldRevID = pRevID Select HWFace).Count()
    '    If (pHWFace_Rec_Count > 0) Then

    '        Dim pHWFace_Rec = (From HWFace In pPartEntities.tblHW_Face
    '                                        Where HWFace.fldPNID = pPNID And
    '                                        HWFace.fldRevID = pRevID Select HWFace).First()
    '        With mPartProject.PNR.HW
    '            mPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), pHWFace_Rec.fldType), clsPartProject.clsPNR.eType)
    '            .POrient = pHWFace_Rec.fldPOrient
    '            .MCrossSecNo = pHWFace_Rec.fldMCS
    '            .IsSegmented = pHWFace_Rec.fldSegmented
    '            .CountSegment = pHWFace_Rec.fldSegmentCount
    '            .MatName = pHWFace_Rec.fldMatName
    '            .Adjusted = pHWFace_Rec.fldAdjusted

    '            'AES 01AUG17
    '            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
    '                If (.Adjusted) Then
    '                    '....HW_AdjCSeal table
    '                    Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    '                                                        Where HWFace_AdjCSeal.fldPNID = pPNID And
    '                                                        HWFace_AdjCSeal.fldRevID = pRevID Select HWFace_AdjCSeal).Count()
    '                    If (pHW_AdjCSeal_Rec_Count > 0) Then

    '                        Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    '                                                        Where HWFace_AdjCSeal.fldPNID = pPNID And
    '                                                        HWFace_AdjCSeal.fldRevID = pRevID Select HWFace_AdjCSeal).First()

    '                        With mPartProject.PNR.HW

    '                            If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDHFree)) Then
    '                                .DHfree = pHWFace_AdjCSeal_Rec.fldDHFree
    '                            Else
    '                                .DHfree = 0.0#
    '                            End If

    '                            If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDThetaOpening)) Then
    '                                .DThetaOpening = pHWFace_AdjCSeal_Rec.fldDThetaOpening
    '                            Else
    '                                .DThetaOpening = 0.0#
    '                            End If

    '                            If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDT)) Then
    '                                .T = pHWFace_AdjCSeal_Rec.fldDT
    '                            Else
    '                                .T = 0.0#
    '                            End If

    '                        End With

    '                    End If

    '                End If
    '            End If



    '            If (Not IsDBNull(pHWFace_Rec.fldHT)) Then
    '                .HT = pHWFace_Rec.fldHT
    '            Else
    '                .HT = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldTemper)) Then
    '                .Temper = pHWFace_Rec.fldTemper
    '            Else
    '                .Temper = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldCoating)) Then
    '                .Coating = pHWFace_Rec.fldCoating
    '            Else
    '                .Coating = "None"
    '            End If

    '            If (.Coating = "T800") Then
    '                .SFinish = pHWFace_Rec.fldSFinish
    '            Else
    '                .SFinish = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldPlatingCode)) Then
    '                .PlatingCode = pHWFace_Rec.fldPlatingCode
    '            Else
    '                .PlatingCode = ""
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldPlatingThickCode)) Then
    '                .PlatingThickCode = pHWFace_Rec.fldPlatingThickCode
    '            Else
    '                .PlatingThickCode = ""
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldHfreeStd)) Then
    '                .Hfree = pHWFace_Rec.fldHfreeStd
    '            Else
    '                .Hfree = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldHFreeTol1)) Then
    '                .HFreeTol(1) = pHWFace_Rec.fldHFreeTol1
    '            Else
    '                .HFreeTol(1) = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldHFreeTol2)) Then
    '                .HFreeTol(2) = pHWFace_Rec.fldHFreeTol2
    '            Else
    '                .HFreeTol(2) = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldDControl)) Then
    '                .DControl = pHWFace_Rec.fldDControl
    '            Else
    '                .DControl = 0
    '            End If

    '            If (Not IsDBNull(pHWFace_Rec.fldH11Tol)) Then
    '                .H11Tol = pHWFace_Rec.fldH11Tol
    '            Else
    '                .H11Tol = 0
    '            End If

    '            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.U) Then
    '                Dim pSealEntities As New SealIPEMCSDBEntities()
    '                Dim pRecord = (From pRec In pSealEntities.tblUSeal_Geom
    '                                Where pRec.fldCrossSecNo = .MCrossSecNo Select pRec).ToList()
    '                If (pRecord.Count > 0) Then
    '                    If (pRecord(0).fldGeomTemplate = False) Then
    '                        mSealIPE_FEA = False
    '                    Else
    '                        mSealIPE_FEA = True
    '                    End If
    '                Else
    '                    mSealIPE_FEA = True
    '                End If
    '            Else
    '                mSealIPE_FEA = True
    '            End If

    '            Dim pIsMatExists As Boolean = False
    '            For i As Integer = 0 To pMatList_Prop.Count - 1
    '                If (.MatName = pMatList_Prop(i)) Then
    '                    pIsMatExists = True
    '                    Exit For
    '                End If
    '            Next

    '            If (pIsMatExists = False) Then
    '                mSealIPE_FEA = False
    '            End If

    '        End With

    '        mTest_Spec = New Test_clsSpec(Me)
    '        Set_SpecData_DefVal()

    '    End If


    'End Sub

    Public Sub RetrieveFrom_DB(ByVal Unit_In As clsUnit)
        '==========================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblTestProject
        Dim pQryTestProject = (From pRec In pSealTestEntities.tblTestProject
                               Where pRec.fldID = mID Select pRec).ToList()

        If (pQryTestProject.Count > 0) Then
            mProjectID = pQryTestProject(0).fldID
            'mPN = pQryTestProject(0).fldPN
            'mRev = pQryTestProject(0).fldRev
            'mCustName = pQryTestProject(0).fldCustomerName
            'mPN_Cust = pQryTestProject(0).fldPN_Cust
            mUserAdmin = pQryTestProject(0).fldUserAdmin
            mDateAdmin = pQryTestProject(0).fldDateAdmin
            mSignedOff = pQryTestProject(0).fldSignedOff
            mUserSignedOff = pQryTestProject(0).fldUserSignedOff
            mDateSignedOff = pQryTestProject(0).fldDateSignedOff

        End If

        '....Unit Data
        mTest_Unit.RetrieveFrom_DB(mID)

        '....Spec Data
        mTest_Spec = New Test_clsSpec(Me)
        Set_SpecData_DefVal()
        mTest_Spec.RetrieveFrom_DB(mID)

        '....Design Data
        ''Retrieve_DesignData(Unit_In)

        '....MO Data
        RetrieveFrom_tblTestMO()

    End Sub


    Private Sub RetrieveFrom_tblTestMO()
        '===============================
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pQry = (From pRec In pSealTestEntities.tblMO
                       Where pRec.fldTestProjectID = mID Select pRec Distinct).ToList()

        mTest_MO = New List(Of Test_clsMO)

        For i As Integer = 0 To pQry.Count - 1

            Dim pMO As New Test_clsMO
            pMO.ID = pQry(i).fldID
            pMO.No = pQry(i).fldNo

            mTest_MO.Add(pMO)
            mTest_MO(i).RetrieveFrom_Report(mID)
        Next

    End Sub

    Public Function GetPNID(ByVal PN_In As String) As Integer
        '====================================================   
        Dim pPartEntities As New SealPartDBEntities()

        Dim pPNID As Integer = 0
        Dim pQryCount As Integer = (From pRec In pPartEntities.tblPN
                                     Where pRec.fldCurrent = PN_In Select pRec).Count()
        If (pQryCount > 0) Then
            Dim pQry = (From pRec In pPartEntities.tblPN
                                     Where pRec.fldCurrent = PN_In Select pRec).First()
            pPNID = pQry.fldID

        Else
            pQryCount = (From pRec In pPartEntities.tblPN
                                     Where pRec.fldLegacy = PN_In Select pRec).Count()
            If (pQryCount > 0) Then
                Dim pQry = (From pRec In pPartEntities.tblPN
                                         Where pRec.fldLegacy = PN_In Select pRec).First()
                pPNID = pQry.fldID
            End If

        End If

        Return pPNID

    End Function


    Public Function GetRevID(ByVal PNID_In As Integer, ByVal Rev_In As String) As Integer
        '================================================================================   
        Dim pPartEntities As New SealPartDBEntities()
        Dim pRevID As Integer = 0

        Dim pQryCount = (From pRec In pPartEntities.tblRev
                                     Where pRec.fldPNID = PNID_In And
                                           pRec.fldCurrent = Rev_In Select pRec).Count()
        If (pQryCount > 0) Then
            Dim pQry = (From pRec In pPartEntities.tblRev
                                    Where pRec.fldPNID = PNID_In And
                                          pRec.fldCurrent = Rev_In Select pRec).First()
            pRevID = pQry.fldID
        Else
            'AES 21MAR17
            pQryCount = (From pRec In pPartEntities.tblRev
                                     Where pRec.fldPNID = PNID_In And
                                           pRec.fldLegacy = Rev_In Select pRec).Count()

            If (pQryCount > 0) Then
                Dim pQry = (From pRec In pPartEntities.tblRev
                                        Where pRec.fldPNID = PNID_In And
                                              pRec.fldLegacy = Rev_In Select pRec).First()
                pRevID = pQry.fldID
            End If

        End If

        Return pRevID

    End Function


    Public Function GetPN_Rev(ByVal PNID_In As Integer, ByVal RevID_In As Integer) As String
        '===================================================================================        
        Dim pPartEntities As New SealPartDBEntities()
        Dim pPN_Rev As String = ""

        If (RevID_In > 0) Then
            Dim pQry = (From pRec In pPartEntities.tblRev
                            Where pRec.fldPNID = PNID_In And
                            pRec.fldID = RevID_In Select pRec).First()

            pPN_Rev = pQry.fldCurrent
        End If

        Return pPN_Rev

    End Function


    ''Private Sub Retrieve_DesignData(ByVal Unit_In As SealPart.clsUnit)
    ''    '=============================================================
    ''    RetrieveFrom_tblTestCavity(Unit_In)
    ''    RetrieveFrom_tblTestOpCond(Unit_In)
    ''    RetrieveFrom_tbtTestSeal(Unit_In)
    ''End Sub


    ''Private Sub RetrieveFrom_tblTestCavity(ByVal Unit_In As clsUnit)
    ''    '===========================================================
    ''    Dim pSealTestEntities As New SealTestDBEntities()

    ''    '....tblTestCavity
    ''    Dim pQryTestCavity = (From pRec In pSealTestEntities.tblTestCavity
    ''                      Where pRec.fldTestProjectID = mID Order By pRec.fldDate Descending Select pRec).ToList()

    ''    If (pQryTestCavity.Count > 0) Then


    ''        mAnalysis_Sel.Cavity.Dia(1) = Unit_In.L_UserToCon(pQryTestCavity(0).fldCavityDia1)
    ''        mAnalysis_Sel.Cavity.Dia(2) = Unit_In.L_UserToCon(pQryTestCavity(0).fldCavityDia2)

    ''        mAnalysis_Sel.Cavity.Depth = Unit_In.L_UserToCon(ConvertToSng(pQryTestCavity(0).fldCavityDepth))
    ''        mAnalysis_Sel.Cavity.DepthTol(1) = Unit_In.L_UserToCon(ConvertToSng(pQryTestCavity(0).fldCavityDepthTol1))
    ''        mAnalysis_Sel.Cavity.DepthTol(2) = Unit_In.L_UserToCon(ConvertToSng(pQryTestCavity(0).fldCavityDepthTol2))

    ''    End If
    ''End Sub


    ''Private Sub RetrieveFrom_tblTestOpCond(ByVal Unit_In As clsUnit)
    ''    '===========================================================
    ''    Dim pSealTestEntities As New SealTestDBEntities()

    ''    '....tblTestOpCond
    ''    Dim pQryTestOpCond = (From pRec In pSealTestEntities.tblTestOpCond
    ''                      Where pRec.fldTestProjectID = mID Order By pRec.fldDate Descending Select pRec).ToList()

    ''    If (pQryTestOpCond.Count > 0) Then

    ''        mAnalysis_Sel.OpCond.UnitUserP = pQryTestOpCond(0).fldUnitUserP
    ''        mAnalysis_Sel.OpCond.PDiff = pQryTestOpCond(0).fldPDiff * Unit_In.CFacUserP
    ''        mAnalysis_Sel.OpCond.TOper = pQryTestOpCond(0).fldTOper
    ''        mAnalysis_Sel.OpCond.POrient = pQryTestOpCond(0).fldPOrient.Trim()

    ''    End If
    ''End Sub


    ''Private Sub RetrieveFrom_tbtTestSeal(ByVal Unit_In As clsUnit)
    ''    '=========================================================
    ''    Dim pSealTestEntities As New SealTestDBEntities()

    ''    '....Seal
    ''    Dim pQrySeal = (From pRec In pSealTestEntities.tblTestSeal
    ''                            Where pRec.fldTestProjectID = mID Order By pRec.fldDate Descending Select pRec).ToList()

    ''    If (pQrySeal.Count() > 0) Then
    ''        Dim pMCS As String = pQrySeal(0).fldMCS
    ''        Dim pSealType As String = mAnalysis_Sel.Seal.Type

    ''        If (pSealType = "E-Seal") Then
    ''            '-------------------------
    ''            'Instantiate ESeal object
    ''            mAnalysis_Sel.Seal = New clsESeal("E-Seal", Unit_In.System, mAnalysis_Sel.OpCond.POrient)
    ''            mAnalysis_Sel.Seal.MCrossSecNo = pMCS
    ''            mAnalysis_Sel.Cavity.CornerRad = mAnalysis_Sel.Seal.CavityCornerRad
    ''            '....Secondary Assignments:
    ''            '
    ''            '....Assign cavity diameters. 
    ''            Dim i As Int16
    ''            For i = 1 To 2
    ''                mAnalysis_Sel.Seal.CavityDia(i) = mAnalysis_Sel.Cavity.Dia(i)
    ''            Next

    ''            If (Not IsNothing(pQrySeal(0).fldSFinish)) Then
    ''                CType(mAnalysis_Sel.Seal, clsESeal).SFinish = Unit_In.L_UserToCon(pQrySeal(0).fldSFinish)
    ''            Else
    ''                CType(mAnalysis_Sel.Seal, clsESeal).SFinish = 0.0#
    ''            End If

    ''            Dim pQryAdjESeal = (From pRec In pSealTestEntities.tblTestAdjESeal
    ''                               Where pRec.fldTestProjectID = mID Order By pRec.fldDate Descending Select pRec).ToList()

    ''            If (pQryAdjESeal.Count() > 0) Then
    ''                If (Not IsNothing(pQryAdjESeal(0).fldDThetaE1)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsESeal).DThetaE1 = pQryAdjESeal(0).fldDThetaE1
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsESeal).DThetaE1 = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjESeal(0).fldDThetaM1)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsESeal).DThetaM1 = pQryAdjESeal(0).fldDThetaM1
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsESeal).DThetaM1 = 0.0#
    ''                End If


    ''            End If


    ''        ElseIf (pSealType = "C-Seal") Then
    ''            '---------------------------------
    ''            'Instantiate CSeal object
    ''            mAnalysis_Sel.Seal = New clsCSeal("C-Seal", Unit_In.System, mAnalysis_Sel.OpCond.POrient)
    ''            mAnalysis_Sel.Seal.MCrossSecNo = pMCS
    ''            mAnalysis_Sel.Cavity.CornerRad = mAnalysis_Sel.Seal.CavityCornerRad

    ''            CType(mAnalysis_Sel.Seal, clsCSeal).PlatingCode = pQrySeal(0).fldPlatingCode     'AES 13OCT16
    ''            CType(mAnalysis_Sel.Seal, clsCSeal).PlatingThickCode = pQrySeal(0).fldPlatingThickCode     'AES 25OCT16

    ''            '....Secondary Assignments:
    ''            '
    ''            '....Assign cavity diameters to the gSealSel object members. 
    ''            Dim i As Int16
    ''            For i = 1 To 2
    ''                mAnalysis_Sel.Seal.CavityDia(i) = mAnalysis_Sel.Cavity.Dia(i)
    ''            Next

    ''            Dim pQryAdjCSeal = (From pRec In pSealTestEntities.tblTestAdjCSeal
    ''                             Where pRec.fldTestProjectID = mID Order By pRec.fldDate Descending Select pRec).ToList()

    ''            If (pQryAdjCSeal.Count() > 0) Then

    ''                If (Not IsNothing(pQryAdjCSeal(0).fldDHFree)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsCSeal).DHfree = pQryAdjCSeal(0).fldDHFree
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsCSeal).DHfree = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjCSeal(0).fldDThetaOpening)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsCSeal).DThetaOpening = Unit_In.L_UserToCon(pQryAdjCSeal(0).fldDThetaOpening)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsCSeal).DThetaOpening = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjCSeal(0).fldDT)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsCSeal).T = Unit_In.L_UserToCon(pQryAdjCSeal(0).fldDT)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsCSeal).T = 0.0#
    ''                End If

    ''            End If

    ''        ElseIf (pSealType = "U-Seal") Then
    ''            '---------------------------------
    ''            'Instantiate USeal object
    ''            mAnalysis_Sel.Seal = New clsUSeal("U-Seal", Unit_In.System, mAnalysis_Sel.OpCond.POrient)
    ''            mAnalysis_Sel.Seal.MCrossSecNo = pMCS

    ''            '....Secondary Assignments:
    ''            '
    ''            '....Assign cavity diameters to the gSealSel object members. 
    ''            Dim i As Int16
    ''            For i = 1 To 2
    ''                mAnalysis_Sel.Seal.CavityDia(i) = mAnalysis_Sel.Cavity.Dia(i)
    ''            Next

    ''            Dim pQryAdjUSeal = (From pRec In pSealTestEntities.tblTestAdjUSeal
    ''                              Where pRec.fldTestProjectID = mID Order By pRec.fldDate Descending Select pRec).ToList()

    ''            If (pQryAdjUSeal.Count() > 0) Then

    ''                If (Not IsNothing(pQryAdjUSeal(0).fldDTheta1)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DTheta(1) = Unit_In.L_UserToCon(pQryAdjUSeal(0).fldDTheta1)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DTheta(1) = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjUSeal(0).fldDTheta2)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DTheta(2) = Unit_In.L_UserToCon(pQryAdjUSeal(0).fldDTheta2)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DTheta(2) = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjUSeal(0).fldDRad1)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DRad(1) = Unit_In.L_UserToCon(pQryAdjUSeal(0).fldDRad1)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DRad(1) = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjUSeal(0).fldDRad2)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DRad(2) = Unit_In.L_UserToCon(pQryAdjUSeal(0).fldDRad2)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DRad(2) = 0.0#
    ''                End If

    ''                If (Not IsNothing(pQryAdjUSeal(0).fldDLLeg)) Then
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DLLeg = Unit_In.L_UserToCon(pQryAdjUSeal(0).fldDLLeg)
    ''                Else
    ''                    CType(mAnalysis_Sel.Seal, clsUSeal).DLLeg = 0.0#
    ''                End If

    ''            End If

    ''        End If

    ''        With mAnalysis_Sel.Seal
    ''            .IsSegmented = pQrySeal(0).fldSegmented

    ''            If (.IsSegmented) Then
    ''                .CountSegment = pQrySeal(0).fldSegmentCount
    ''            Else
    ''                .CountSegment = 0
    ''            End If

    ''            .ZClear_Given = Unit_In.L_UserToCon(pQrySeal(0).fldZClear)
    ''            .HFreeTol(1) = Unit_In.L_UserToCon(pQrySeal(0).fldHFreeTol1)
    ''            .HFreeTol(2) = Unit_In.L_UserToCon(pQrySeal(0).fldHFreeTol2)
    ''        End With

    ''        'Material:
    ''        '-----------
    ''        With mAnalysis_Sel.Seal.Mat
    ''            .Name = pQrySeal(0).fldMatName
    ''            .HT = pQrySeal(0).fldHeatTreatment

    ''            If (mAnalysis_Sel.Seal.Type = "E-Seal") Then
    ''                If (IsNothing(pQrySeal(0).fldCoating)) Then
    ''                    .Coating = "None"
    ''                Else
    ''                    .Coating = pQrySeal(0).fldCoating
    ''                End If
    ''            End If

    ''        End With
    ''    End If
    ''End Sub


    ''Public Sub SaveTo_DB_DesignData(ByVal Unit_In As SealPart.clsUnit)
    ''    '=====================================================
    ''    Dim pSealTestEntities As New SealTestDBEntities()
    ''    Dim pTestProjectID As Integer = 1
    ''    Dim pIsExistingRec As Boolean = False

    ''    Try
    ''        Dim pQryProject = (From pRec In pSealTestEntities.tblTestProject
    ''                            Order By pRec.fldID Descending
    ''                                  Select pRec).ToList()

    ''        If (pQryProject.Count() > 0) Then
    ''            Dim pID As Integer = pQryProject(0).fldID
    ''            pTestProjectID = pID + 1
    ''        End If

    ''        Dim pQryProjectRec = (From pRec In pSealTestEntities.tblTestProject Where
    ''                              pRec.fldProjectID = mProjectID And pRec.fldPN = mPN And pRec.fldRev = mRev And
    ''                              pRec.fldCustomerName = mCustName And pRec.fldPN_Cust = mPN_Cust
    ''                              Select pRec).ToList()

    ''        If (pQryProjectRec.Count() > 0) Then
    ''            pTestProjectID = pQryProjectRec(0).fldID
    ''            pIsExistingRec = True
    ''        End If


    ''        Dim pTestProject As New tblTestProject

    ''        With pTestProject
    ''            .fldID = pTestProjectID
    ''            .fldProjectID = mProjectID
    ''            .fldPN = mPN
    ''            .fldRev = mRev
    ''            .fldCustomerName = mCustName
    ''            .fldPN_Cust = mPN_Cust
    ''            .fldUserAdmin = mUserAdmin
    ''            .fldDateAdmin = mDateAdmin
    ''            .fldSignedOff = mSignedOff
    ''            .fldUserSignedOff = mUserSignedOff
    ''            .fldDateSignedOff = mDateSignedOff

    ''            If (pIsExistingRec) Then
    ''                pSealTestEntities.SaveChanges()
    ''            Else
    ''                pSealTestEntities.AddTotblTestProject(pTestProject)
    ''                pSealTestEntities.SaveChanges()
    ''            End If

    ''        End With

    ''        '....Cavity
    ''        Dim pQryCavity = (From pRec In pSealTestEntities.tblTestCavity
    ''                           Where pRec.fldTestProjectID = pTestProjectID Select pRec).ToList()

    ''        Dim pCavity As New tblTestCavity

    ''        If (pQryCavity.Count() > 0) Then
    ''            pCavity = pQryCavity(0)
    ''        End If

    ''        With pCavity
    ''            .fldTestProjectID = pTestProjectID
    ''            .fldProjectID = mProjectID
    ''            .fldDate = DateTime.Now()
    ''            mDate_ImportDesign = DateTime.Now()
    ''            .fldCavityID = mAnalysisID_Sel
    ''            .fldCavityDia1 = Unit_In.L_ConToUser(mAnalysis_Sel.Cavity.Dia(1))
    ''            .fldCavityDia2 = Unit_In.L_ConToUser(mAnalysis_Sel.Cavity.Dia(2))

    ''            .fldCavityDepth = Unit_In.L_ConToUser(mAnalysis_Sel.Cavity.Depth)

    ''            .fldCavityDepthTol1 = Unit_In.L_ConToUser(mAnalysis_Sel.Cavity.DepthTol(1))
    ''            .fldCavityDepthTol2 = Unit_In.L_ConToUser(mAnalysis_Sel.Cavity.DepthTol(2))

    ''            If (pQryCavity.Count() > 0) Then
    ''                pSealTestEntities.SaveChanges()
    ''            Else
    ''                pSealTestEntities.AddTotblTestCavity(pCavity)
    ''                pSealTestEntities.SaveChanges()
    ''            End If

    ''        End With

    ''        '....OpCond
    ''        Dim pQryOpCond = (From pRec In pSealTestEntities.tblTestOpCond
    ''                           Where pRec.fldTestProjectID = pTestProjectID Select pRec).ToList()

    ''        Dim pOpCond As New tblTestOpCond

    ''        If (pQryOpCond.Count() > 0) Then
    ''            pOpCond = pQryOpCond(0)
    ''        End If

    ''        With pOpCond
    ''            .fldTestProjectID = pTestProjectID
    ''            .fldProjectID = mProjectID
    ''            .fldDate = mDate_ImportDesign 'DateTime.Now()
    ''            .fldOpCondID = mAnalysisID_Sel
    ''            .fldUnitUserP = gUnit.UserP()
    ''            .fldPDiff = mAnalysis_Sel.OpCond.PDiff / Unit_In.CFacUserP()     '....User Unit.
    ''            .fldTOper = mAnalysis_Sel.OpCond.TOper
    ''            .fldPOrient = mAnalysis_Sel.OpCond.POrient

    ''            If (pQryOpCond.Count() > 0) Then
    ''                pSealTestEntities.SaveChanges()
    ''            Else
    ''                pSealTestEntities.AddTotblTestOpCond(pOpCond)
    ''                pSealTestEntities.SaveChanges()
    ''            End If

    ''        End With

    ''        '....Seal
    ''        Dim pQrySeal = (From pRec In pSealTestEntities.tblTestSeal
    ''                           Where pRec.fldTestProjectID = pTestProjectID Select pRec).ToList()

    ''        Dim pSeal As New tblTestSeal
    ''        If (pQrySeal.Count() > 0) Then
    ''            pSeal = pQrySeal(0)
    ''        End If

    ''        Dim pDateSeal As DateTime = mDate_ImportDesign 'DateTime.Now()

    ''        With pSeal
    ''            .fldTestProjectID = pTestProjectID
    ''            .fldProjectID = mProjectID
    ''            .fldDate = pDateSeal
    ''            .fldSealID = mAnalysisID_Sel

    ''            .fldType = mAnalysis_Sel.Seal.Type
    ''            .fldMCS = mAnalysis_Sel.Seal.MCrossSecNo
    ''            .fldSegmented = mAnalysis_Sel.Seal.IsSegmented

    ''            If (mAnalysis_Sel.Seal.IsSegmented = True) Then
    ''                .fldSegmentCount = mAnalysis_Sel.Seal.CountSegment
    ''            Else
    ''                .fldSegmentCount = 0
    ''            End If

    ''            .fldMatName = mAnalysis_Sel.Seal.Mat.Name
    ''            .fldHeatTreatment = mAnalysis_Sel.Seal.Mat.HT
    ''            .fldCoating = mAnalysis_Sel.Seal.Mat.Coating

    ''            If (mAnalysis_Sel.Seal.Type = "E-Seal") Then
    ''                .fldSFinish = CType(mAnalysis_Sel.Seal, clsESeal).SFinish

    ''            ElseIf (mAnalysis_Sel.Seal.Type = "C-Seal") Then
    ''                .fldPlatingCode = CType(mAnalysis_Sel.Seal, clsCSeal).Plating.Code
    ''                .fldPlatingThickCode = CType(mAnalysis_Sel.Seal, clsCSeal).Plating.ThickCode
    ''            End If

    ''            .fldZClear = Unit_In.L_ConToUser(mAnalysis_Sel.Seal.ZClear_Given)
    ''            .fldHFreeTol1 = Unit_In.L_ConToUser(mAnalysis_Sel.Seal.HFreeTol(1))
    ''            .fldHFreeTol2 = Unit_In.L_ConToUser(mAnalysis_Sel.Seal.HFreeTol(2))

    ''            If (mAnalysis_Sel.Seal.Adjusted = "Y") Then
    ''                .fldAdjusted = True
    ''            Else
    ''                .fldAdjusted = False
    ''            End If


    ''            If (pQrySeal.Count() > 0) Then
    ''                pSealTestEntities.SaveChanges()
    ''            Else
    ''                pSealTestEntities.AddTotblTestSeal(pSeal)
    ''                pSealTestEntities.SaveChanges()
    ''            End If

    ''        End With

    ''        If (mAnalysis_Sel.Seal.Type = "E-Seal") Then
    ''            Dim pQryAdjESeal = (From it In pSealTestEntities.tblTestAdjESeal
    ''                                  Where it.fldTestProjectID = pTestProjectID Select it).ToList()
    ''            Dim pESeal As New tblTestAdjESeal

    ''            If (pQryAdjESeal.Count() > 0) Then
    ''                pESeal = pQryAdjESeal(0)

    ''            End If

    ''            With pESeal
    ''                .fldTestProjectID = pTestProjectID
    ''                .fldProjectID = mProjectID
    ''                .fldDate = pDateSeal
    ''                .fldSealID = mAnalysisID_Sel

    ''                .fldDThetaE1 = CType(mAnalysis_Sel.Seal, clsESeal).DThetaE1
    ''                .fldDThetaM1 = CType(mAnalysis_Sel.Seal, clsESeal).DThetaM1

    ''                If (pQryAdjESeal.Count() > 0) Then
    ''                    pSealTestEntities.SaveChanges()
    ''                Else
    ''                    pSealTestEntities.AddTotblTestAdjESeal(pESeal)
    ''                    pSealTestEntities.SaveChanges()
    ''                End If

    ''            End With

    ''        ElseIf (mAnalysis_Sel.Seal.Type = "C-Seal") Then
    ''            Dim pQryAdjCSeal = (From it In pSealTestEntities.tblTestAdjCSeal
    ''                                 Where it.fldTestProjectID = pTestProjectID Select it).ToList()
    ''            Dim pCSeal As New tblTestAdjCSeal

    ''            If (pQryAdjCSeal.Count() > 0) Then
    ''                pCSeal = pQryAdjCSeal(0)

    ''            End If

    ''            With pCSeal
    ''                .fldTestProjectID = pTestProjectID
    ''                .fldProjectID = mProjectID
    ''                .fldDate = pDateSeal
    ''                .fldSealID = mAnalysisID_Sel
    ''                .fldDHFree = Unit_In.L_ConToUser(CType(mAnalysis_Sel.Seal, clsCSeal).DHfree)
    ''                .fldDThetaOpening = CType(mAnalysis_Sel.Seal, clsCSeal).DThetaOpening
    ''                .fldDT = Unit_In.L_ConToUser(mAnalysis_Sel.Seal.T)
    ''                If (pQryAdjCSeal.Count() > 0) Then
    ''                    pSealTestEntities.SaveChanges()
    ''                Else
    ''                    pSealTestEntities.AddTotblTestAdjCSeal(pCSeal)
    ''                    pSealTestEntities.SaveChanges()
    ''                End If
    ''            End With

    ''        ElseIf (mAnalysis_Sel.Seal.Type = "U-Seal") Then
    ''            Dim pQryAdjUSeal = (From it In pSealTestEntities.tblTestAdjUSeal
    ''                                 Where it.fldTestProjectID = pTestProjectID Select it).ToList()
    ''            Dim pUSeal As New tblTestAdjUSeal

    ''            If (pQryAdjUSeal.Count() > 0) Then
    ''                pUSeal = pQryAdjUSeal(0)

    ''            End If

    ''            With pUSeal
    ''                .fldTestProjectID = pTestProjectID
    ''                .fldProjectID = mProjectID
    ''                .fldDate = pDateSeal
    ''                .fldSealID = mAnalysisID_Sel

    ''                .fldDTheta1 = CType(mAnalysis_Sel.Seal, clsUSeal).DTheta(1)
    ''                .fldDTheta2 = CType(mAnalysis_Sel.Seal, clsUSeal).DTheta(2)

    ''                .fldDRad1 = Unit_In.L_ConToUser(CType(mAnalysis_Sel.Seal, clsUSeal).DRad(1))
    ''                .fldDRad2 = Unit_In.L_ConToUser(CType(mAnalysis_Sel.Seal, clsUSeal).DRad(2))
    ''                .fldDLLeg = Unit_In.L_ConToUser(CType(mAnalysis_Sel.Seal, clsUSeal).DLLeg)

    ''                .fldDT = Unit_In.L_ConToUser(mAnalysis_Sel.Seal.T)

    ''                If (pQryAdjUSeal.Count() > 0) Then
    ''                    pSealTestEntities.SaveChanges()
    ''                Else
    ''                    pSealTestEntities.AddTotblTestAdjUSeal(pUSeal)
    ''                    pSealTestEntities.SaveChanges()
    ''                End If
    ''            End With
    ''        End If

    ''    Catch ex As Exception

    ''    End Try

    ''End Sub


    Public Sub SaveTo_DB(ByVal Unit_In As clsUnit, ByVal MO_Sel_In As Integer,
                         ByVal Report_Sel As Integer, ByVal PNID_In As Integer, ByVal RevID_In As Integer)          'SaveTo_DB
        '===============================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pTestProjectID As Integer = 1
        Dim pIsExistingRec As Boolean = False
        Dim pTestProject As New tblTestProject

        Try
            '....tblTestProject
            Dim pSealPart_Project As New clsPartProject()
            Dim pPNID As Integer = PNID_In
            Dim pRevID As Integer = RevID_In

            Dim pPNR_CustInfoID As Integer = 0

            Dim pSealPartEntities As New SealPartDBEntities

            Dim pQryPartProject = (From pRec In pSealPartEntities.tblProject Where
                                  pRec.fldPNID = pPNID And pRec.fldRevID = pRevID
                                   Select pRec).ToList()

            If (pQryPartProject.Count > 0) Then
                pPNR_CustInfoID = pQryPartProject(0).fldID
            End If

            Dim pQryProjectRec = (From pRec In pSealTestEntities.tblTestProject Where
                                  pRec.fldPNID = pPNID And pRec.fldRevID = pRevID
                                  Select pRec).ToList()

            If (pQryProjectRec.Count() > 0) Then
                pTestProject = pQryProjectRec(0)
                pTestProjectID = pQryProjectRec(0).fldID
                pIsExistingRec = True

            Else
                Dim pRec = (From Rec In pSealTestEntities.tblTestProject Order By Rec.fldID Descending
                            Select Rec).ToList()
                If (pRec.Count() > 0) Then
                    pTestProjectID = pRec(0).fldID + 1
                End If

            End If

            With pTestProject
                .fldPNID = pPNID
                .fldRevID = pRevID
                .fldPNR_CustInfoID = pPNR_CustInfoID
                .fldID = pTestProjectID
                mID = pTestProjectID
                .fldUserAdmin = mUserAdmin
                mDateAdmin = DateTime.Now()
                .fldDateAdmin = mDateAdmin
                .fldSignedOff = mSignedOff
                .fldUserSignedOff = mUserSignedOff
                .fldDateSignedOff = mDateSignedOff
            End With

            If (Not pIsExistingRec) Then
                pSealTestEntities.AddTotblTestProject(pTestProject)
            End If

            pSealTestEntities.SaveChanges()

            '....tblTestUnit
            mTest_Unit.SaveTo_DB(mID)

            '....tblTestSpec
            mTest_Spec.SaveTo_DB()

            '....tblTestMO
            SaveTo_tblTestMO(Unit_In, MO_Sel_In)

            If (mTest_MO.Count > MO_Sel_In) Then
                If (mTest_MO(MO_Sel_In).Test_Report.Count > Report_Sel) Then
                    mTest_MO(MO_Sel_In).Test_Report(Report_Sel).SaveTo_tblTestReport(mID, mTest_MO(MO_Sel_In).ID)
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub SaveTo_tblTestMO(ByVal Unit_In As clsUnit, ByVal MO_Sel_In As Integer)             'SaveTo_tblTestMO
        '=============================================================================
        Dim pSealTestEntities As New SealTestDBEntities()

        Dim pMO_ID As Integer = 1
        Dim pTestMO As New tblMO
        Dim pIsMOExists As Boolean = False

        If (mTest_MO.Count() > MO_Sel_In) Then
            pMO_ID = mTest_MO(MO_Sel_In).ID

            Dim pQry = (From pRec In pSealTestEntities.tblMO
                        Where pRec.fldTestProjectID = mID And pRec.fldID = pMO_ID Select pRec).ToList()

            If (pQry.Count > 0) Then
                pTestMO = pQry(0)
                pIsMOExists = True
            End If

            With pTestMO
                .fldTestProjectID = mID
                .fldID = pMO_ID
                .fldNo = mTest_MO(MO_Sel_In).No
            End With

            If (pIsMOExists) Then
                pSealTestEntities.SaveChanges()
            Else
                pSealTestEntities.AddTotblMO(pTestMO)
                pSealTestEntities.SaveChanges()

            End If
        End If

    End Sub


    Public Sub DeleteFrom_tblTestMO(ByVal MO_No_In As Integer)
        '======================================================
        Dim pSealTestEntities As New SealTestDBEntities

        Dim pQry = (From pRec In pSealTestEntities.tblMO
                           Where pRec.fldTestProjectID = mID And pRec.fldNo = MO_No_In Select pRec).ToList()

        If (pQry.Count() > 0) Then
            pSealTestEntities.DeleteObject(pQry(0))
            pSealTestEntities.SaveChanges()
        End If

    End Sub

#End Region


#Region "SealIPE Design:"

    'Public Sub Import_SealIPEDesign(ByVal ProjectIn As clsProject)
    '    '==========================================================
    '    Dim pProjectEntities As New ProjectDBEntities

    '    mUserAdmin = gTestUser.Name
    '    mDateAdmin = DateTime.Now()
    '    mSignedOff = False
    '    mUserSignedOff = ""
    '    mDateSignedOff = DateTime.MaxValue

    '    Dim pQry = (From pRec In pProjectEntities.tblProject
    '                        Where pRec.fldID = mProjectID Select pRec).ToList()

    '    If (pQry.Count() > 0) Then
    '        ProjectIn.Customer_ID = pQry(0).fldCustID
    '        ProjectIn.Platform_ID = pQry(0).fldPlatformID
    '        ProjectIn.Location_ID = pQry(0).fldLocID
    '        ProjectIn.PN_ID = pQry(0).fldPNID
    '        ProjectIn.PN_Rev_ID = pQry(0).fldRevID
    '        ProjectIn.Project_ID = mProjectID
    '        mPN_Cust = pQry(0).fldPN_Cust

    '        mCustName = ProjectIn.GetCustomer(ProjectIn.Customer_ID)

    '        Dim pQrySeal = (From pRec In pProjectEntities.tblSeal
    '                      Where pRec.fldProjectID = mProjectID Select pRec).ToList()

    '        If (pQrySeal.Count() > 0) Then
    '            For i As Integer = 0 To pQrySeal.Count() - 1

    '                Dim pSelected As Boolean = pQrySeal(i).fldSelected
    '                If (pSelected) Then
    '                    mAnalysisID_Sel = i + 1
    '                End If
    '            Next
    '        End If

    '        'mAnalysis = New clsAnalysis(ProjectIn)     'AES 04NOV16
    '        mAnalysis_Sel.ID = mAnalysisID_Sel
    '        mAnalysis_Sel.Retrieve_FromDB(gUnit, gANSYS)

    '        mDate_ImportDesign = Date.Now()

    '        Set_SpecData_DefVal()
    '    End If

    'End Sub


    Public Sub Set_SpecData_DefVal()
        '============================

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            If (mPartProject.PNR.HW.Adjusted) Then
                mTest_Spec.SealFHIni_Unplated(0) = (mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.DHfree) - mPartProject.PNR.HW.HFreeTol(1) 'mPartProject.PNR.HW.Hfree - mPartProject.PNR.HW.HFreeTol(1)
                mTest_Spec.SealFHIni_Unplated(1) = (mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.DHfree) + mPartProject.PNR.HW.HFreeTol(2) 'mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.HFreeTol(2)

                mTest_Spec.SealFHIni_Plated(0) = (mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.DHfree) - mPartProject.PNR.HW.HFreeTol(1)
                mTest_Spec.SealFHIni_Plated(1) = (mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.DHfree) + mPartProject.PNR.HW.HFreeTol(2)
            Else
                mTest_Spec.SealFHIni_Unplated(0) = mPartProject.PNR.HW.Hfree - mPartProject.PNR.HW.HFreeTol(1)
                mTest_Spec.SealFHIni_Unplated(1) = mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.HFreeTol(2)

                mTest_Spec.SealFHIni_Plated(0) = mPartProject.PNR.HW.Hfree - mPartProject.PNR.HW.HFreeTol(1)
                mTest_Spec.SealFHIni_Plated(1) = mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.HFreeTol(2)
            End If
        Else
            mTest_Spec.SealFHIni_Unplated(0) = mPartProject.PNR.HW.Hfree - mPartProject.PNR.HW.HFreeTol(1)
            mTest_Spec.SealFHIni_Unplated(1) = mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.HFreeTol(2)

            mTest_Spec.SealFHIni_Plated(0) = mPartProject.PNR.HW.Hfree - mPartProject.PNR.HW.HFreeTol(1)
            mTest_Spec.SealFHIni_Plated(1) = mPartProject.PNR.HW.Hfree + mPartProject.PNR.HW.HFreeTol(2)
        End If


        Dim pSealType As String = mPartProject.PNR.SealType

        Dim psngTol1DControl As Single = 0.0
        Dim psngTol2DControl As Single = 0.0

        If (Not IsNothing(mPartProject.PNR.HW.POrient)) Then
            If (mPartProject.PNR.HW.POrient = "Internal") Then

                psngTol1DControl = 0.0#
                psngTol2DControl = mPartProject.PNR.HW.H11Tol

                mTest_Spec.SealODPre_Unplated(1) = mPartProject.PNR.HW.DControl + psngTol1DControl
                mTest_Spec.SealODPre_Unplated(0) = mPartProject.PNR.HW.DControl - psngTol2DControl

                mTest_Spec.SealODPre_Plated(1) = mPartProject.PNR.HW.DControl + psngTol1DControl
                mTest_Spec.SealODPre_Plated(0) = mPartProject.PNR.HW.DControl - psngTol2DControl
            Else
                psngTol1DControl = mPartProject.PNR.HW.H11Tol
                psngTol2DControl = 0.0#

                mTest_Spec.SealIDPre_Unplated(1) = mPartProject.PNR.HW.DControl + psngTol1DControl
                mTest_Spec.SealIDPre_Unplated(0) = mPartProject.PNR.HW.DControl - psngTol2DControl

                mTest_Spec.SealIDPre_Plated(1) = mPartProject.PNR.HW.DControl + psngTol1DControl
                mTest_Spec.SealIDPre_Plated(0) = mPartProject.PNR.HW.DControl - psngTol2DControl

            End If

        Else
            mTest_Spec.SealODPre_Unplated(1) = 0
            mTest_Spec.SealODPre_Unplated(0) = 0

            mTest_Spec.SealODPre_Plated(1) = 0
            mTest_Spec.SealODPre_Plated(0) = 0

        End If

        mTest_Spec.LeakCavityDepth = 0
        mTest_Spec.LeakPress = 0

        mTest_Spec.LoadMin_CavityDepth = 0
        mTest_Spec.LoadRange_CavityDepth = 0
        mTest_Spec.LoadMax_CavityDepth = 0

        mTest_Spec.Leak_Springback = True
        mTest_Spec.LeakSpringBackMin_Unplated = 0.0
        mTest_Spec.LeakSpringBackMin_Plated = 0.0

    End Sub

#End Region


#Region "ANSYS Related Routines:"

    Public Sub RunANSYS_FEA(ByVal TestType_In As String, ByVal ANSYS_In As IPE_clsANSYS)
        '====================================================================================== 

        mAnalysis.Seal.MCrossSecNo = mPartProject.PNR.HW.MCrossSecNo
        mAnalysis.Seal.Mat.Name = mPartProject.PNR.HW.MatName

        '....Delete Previous ANSYS files
        mAnalysis.ANSYS.DeletePrevFiles_ANSYS()

        '....Create Load Steps for Leak Test.
        Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)
        If (TestType_In = "Leak") Then
            pLoadStep = Create_LoadStep_Leak()

        ElseIf (TestType_In = "Load") Then
            pLoadStep = Create_LoadStep_Load()
        End If

        '....Create the Input data file - "file.inp". 
        gIPE_Unit.System = gUnit.System
        ANSYS_In.NelMax = 2000
        mAnalysis.WriteFile_ANSYS_Input(gIPE_Unit, ANSYS_In, pLoadStep, mPartProject.PNR.HW.DControl)

        If mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E Then

            Dim pESeal As IPE_clsESeal
            pESeal = CType(mAnalysis.Seal, IPE_clsESeal)

            If pESeal.TemplateNo.Contains("1Gen") Then
                '....Includes both "1Gen" & "1GenS".
                pESeal.WriteFile_KP_T1Gen(gIPE_Unit, ANSYS_In)
            End If

        End If

        If mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.U Then

            Dim pUSeal As IPE_clsUSeal
            pUSeal = CType(mAnalysis.Seal, IPE_clsUSeal)

            pUSeal.WriteFile_KP_T1Gen(gIPE_Unit, ANSYS_In)

        End If

        With ANSYS_In
            .Run("ANSYS")

            'EXAMINE SOLUTION:
            '----------------
            If .Solve = "Y" And .ExitNormal = True Then

                '....The following method reads if the solution converged or not. 
                '........If converged, read the results into appropriate "gSeal" properties.
                mAnalysis.ReadFile_ANSYS_Output(gIPE_ANSYS)

                If .SolnConv = 0 Then               '....No convergence.
                    '----------------
                    Dim pstrPrompt As String
                    Dim pintAttributes As String
                    Dim pstrTitle As String
                    Dim pintAnswer As Integer

                    pstrTitle = "ANSYS Solution : No Convergence"
                    pstrPrompt = " The ANSYS solution did not converge." & vbCrLf & _
                            "You may try with different set of applied loading." & _
                            vbCrLf & "Consult with engineering."
                    pintAttributes = vbCritical + vbOKOnly
                    pintAnswer = MsgBox(pstrPrompt, pintAttributes, pstrTitle)

                    mAnalysis.DateCreated = DateTime.Now
                    mAnalysis.TimeCreated = DateTime.Now

                ElseIf .SolnConv = 1 Then           '....Convergence achieved.
                    '--------------------
                    mAnalysis.DateCreated = DateTime.Now
                    mAnalysis.TimeCreated = DateTime.Now

                End If

            End If

        End With

    End Sub


    Private Function Create_LoadStep_Leak() As List(Of IPE_clsAnalysis.sLoadStep)
        '================================================================== //AES 06DEC16

        Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)

        '....Load Step: 'Assembly'
        Dim pLoadStep_Assy As New IPE_clsAnalysis.sLoadStep

        With pLoadStep_Assy
            .PDiff = 0
            .T = 70
            .CavityDepth = mTest_Spec.LeakCavityDepth
            .CompressionVal = mTest_Spec.SealFHIni(1).Unplated - mTest_Spec.LeakCavityDepth
            .Descrip = "Assembly"
        End With

        '....Load Step: Baseline
        Dim pLoadStep_BL As New IPE_clsAnalysis.sLoadStep

        With pLoadStep_BL
            .PDiff = mTest_Spec.LeakPress
            .T = 70
            .CavityDepth = mTest_Spec.LeakCavityDepth
            .CompressionVal = mTest_Spec.SealFHIni(1).Unplated - mTest_Spec.LeakCavityDepth
            .Descrip = "BL"
        End With

        '....Load Step: 'Open'
        Dim pLoadStep_Open As New IPE_clsAnalysis.sLoadStep

        With pLoadStep_Open
            .PDiff = 0
            .T = 70
            .CavityDepth = mTest_Spec.LeakCavityDepth
            .CompressionVal = 0.0
            .Descrip = "Open"
        End With

        '....List
        With pLoadStep
            .Add(pLoadStep_Assy)
            .Add(pLoadStep_BL)
            .Add(pLoadStep_Assy)
            .Add(pLoadStep_Open)
        End With

        Return pLoadStep

    End Function


    Private Function Create_LoadStep_Load() As List(Of IPE_clsAnalysis.sLoadStep)
        '================================================================================

        Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)

        '....Load Step: 'Assembly'
        Dim pLoadStep_Assy As New IPE_clsAnalysis.sLoadStep

        Dim pCavityDepth As Double = 0.0

        If (mTest_Spec.LoadType = Test_clsSpec.eLoadType.Min) Then
            pCavityDepth = mTest_Spec.LoadMin_CavityDepth

        ElseIf (mTest_Spec.LoadType = Test_clsSpec.eLoadType.Max) Then
            pCavityDepth = mTest_Spec.LoadMax_CavityDepth

        ElseIf (mTest_Spec.LoadType = Test_clsSpec.eLoadType.Range) Then
            pCavityDepth = mTest_Spec.LoadRange_CavityDepth
        End If

        With pLoadStep_Assy
            .PDiff = 0
            .T = 70
            .CavityDepth = pCavityDepth
            .CompressionVal = mTest_Spec.SealFHIni(1).Unplated - pCavityDepth
            .Descrip = "Assembly"
        End With

        '....Load Step: Baseline
        Dim pLoadStep_BL As New IPE_clsAnalysis.sLoadStep

        With pLoadStep_BL
            .PDiff = 0
            .T = 70
            .CavityDepth = pCavityDepth
            .CompressionVal = mTest_Spec.SealFHIni(1).Unplated - pCavityDepth
            .Descrip = "BL"
        End With

        '....Load Step: 'Open'
        Dim pLoadStep_Open As New IPE_clsAnalysis.sLoadStep

        With pLoadStep_Open
            .PDiff = 0
            .T = 70
            .CavityDepth = pCavityDepth
            .CompressionVal = 0.0
            .Descrip = "Open"
        End With

        '....List
        With pLoadStep
            .Add(pLoadStep_Assy)
            .Add(pLoadStep_BL)
            .Add(pLoadStep_Assy)
            .Add(pLoadStep_Open)
        End With

        Return pLoadStep

    End Function

#End Region


#Region "SERIALIZE-DESERIALIZE:"

    Public Function SaveData_Serialize(FilePath_In As String) As Boolean
        '================================================================
        Try
            Dim serializer As IFormatter = New BinaryFormatter()
            Dim pFileName As String = FilePath_In & "1.SealTest"

            Dim saveFile As New FileStream(pFileName, FileMode.Create, FileAccess.Write)

            serializer.Serialize(saveFile, Me)

            saveFile.Close()

            Return True
        Catch
            Return False
        End Try
    End Function


    Public Function RestoreData_Deserialize(FilePath_In As String) As Object
        '====================================================================
        Dim serializer As IFormatter = New BinaryFormatter()
        Dim pFileName As String = FilePath_In & "1.SealTest"
        Dim openFile As New FileStream(pFileName, FileMode.Open, FileAccess.Read)
        Dim pObj As Object
        pObj = serializer.Deserialize(openFile)

        openFile.Close()

        Return pObj

    End Function

#End Region


#Region "CLONE METHOD"

    '   DEEP CLONING:
    '   -------------
    '
    Public Function Clone() As Object Implements ICloneable.Clone
        '========================================================

        '....Inherited from the ICloneable interface, supports deep cloning
        Dim pobjClone As Object
        Try

            Dim pMemBuffer As New MemoryStream()
            Dim pBinSerializer As New BinaryFormatter(Nothing, _
                                  New StreamingContext(StreamingContextStates.Clone))


            '....Serialize the object into the memory stream
            pBinSerializer.Serialize(pMemBuffer, Me)

            '....Move the stream pointer to the beginning of the memory stream
            pMemBuffer.Seek(0, SeekOrigin.Begin)

            '....Get the serialized object from the memory stream

            pobjClone = pBinSerializer.Deserialize(pMemBuffer)

            pMemBuffer.Close()      '....Release the memory stream



        Catch ex As Exception

        End Try
        Return pobjClone    '....Return the deeply cloned object

    End Function

#End Region


End Class
