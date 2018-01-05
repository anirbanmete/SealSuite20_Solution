'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsSealCandidates                      '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05APR16                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Data
Imports System.Data.OleDb
Imports System.Threading
Imports System.Globalization
Imports clsLibrary11
Imports System.IO
Imports System.Linq

Public Class IPE_clsSealCandidates

#Region "MEMBER VARIABLES:"

#Region "NAMED CONTSTANT:"

    Private Const mcEPS As Single = 0.00001     '....An aribitrarily small number. 

    '....Buckling Factors:
    Private Const mcFacBuckling_ESeal As Single = 0.5
    Private Const mcFacBuckling_CSeal As Single = 0.0
    Private Const mcFacBuckling_USeal As Single = 0.0

    '....Minimum Compression Required. True for both E- & C-Seal.   
    '........Per discussion with APCO, Steve Stone, 01JUN06.
    Private Const mcCompressPcentValueMinReqd As Single = 5.0   '....in %.

#End Region

    '....Design Constraints, as calculated:
    Private mWidMaxAllowed As Single        '....On Seal Wid.
    Private mHFreeMinAllowed As Single      '....On Seal Free Height.


    'SECONDARY ASSIGNMENT MEMBER VARIABLES:        
    '-------------------------------------
    Private mType As String
    Private mUnit As IPE_clsUnit
    Private mPOrient As String

    Private mCavityDia(2) As Single

    '....Control dia related parameters:
    Private mZClear As Single
    Private mH11Tol As Single

#End Region


#Region "PROPERTY ROUTINES:"

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
        End Set

    End Property


    '....POrient.
    Public Property POrient() As String
        '==============================
        Get
            Return mPOrient
        End Get

        Set(ByVal strData As String)
            '------------------------
            mPOrient = strData
        End Set

    End Property


#End Region


#Region "READ-ONLY PROPERTIES:"

    '....Type:
    Public ReadOnly Property Type() As String
        '======================================
        Get
            Return mType
        End Get
    End Property

    '....ZClear:
    Public ReadOnly Property ZClear() As Single
        '========================================
        Get
            Return mZClear
        End Get
    End Property

    '....H11Tol:
    Public ReadOnly Property H11Tol() As Single
        '=====================================
        Get
            Return mH11Tol
        End Get
    End Property

    '....FacBuckling:
    Public ReadOnly Property FacBuckling() As Single
        '===========================================
        Get

            If mType = "E-Seal" Then
                Return mcFacBuckling_ESeal

            ElseIf mType = "C-Seal" Then
                Return mcFacBuckling_CSeal

            ElseIf mType = "U-Seal" Then
                Return mcFacBuckling_USeal
            End If

        End Get

    End Property


    '....WidMaxAllowed:
    Public ReadOnly Property WidMaxAllowed() As Single
        '=============================================
        Get
            Return mWidMaxAllowed
        End Get
    End Property


    '....CompressPcentValueMinReqd:
    Public ReadOnly Property CompressPcentValueMinReqd() As Single
        '=========================================================
        Get
            Return mcCompressPcentValueMinReqd
        End Get
    End Property


    '....HFreeMinAllowed:
    Public ReadOnly Property HFreeMinAllowed() As Single
        '===============================================
        Get
            Return mHFreeMinAllowed
        End Get
    End Property

#End Region


#Region "WRITE-ONLY PROPERTIES:"

    '....Cavity Diameters:
    Public WriteOnly Property CavityDia(ByVal i As Integer) As Single
        '============================================================
        Set(ByVal sngData As Single)
            mCavityDia(i) = sngData
        End Set
    End Property

#End Region

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New(ByVal strType_In As String, _
                   Optional ByVal strUnitSystem_In As String = "English", _
                   Optional ByVal strPOrient_In As String = "External")
        '==================================================================

        If Trim(strType_In) <> "C-Seal" And _
           Trim(strType_In) <> "E-Seal" And _
           Trim(strType_In) <> "U-Seal" Then

            MsgBox("Seal Type = " & strType_In & " is not accepted")
            Exit Sub
        End If

        mType = strType_In

        mUnit = New IPE_clsUnit(strUnitSystem_In)
        mPOrient = strPOrient_In

    End Sub

#End Region


#Region "UTILITY ROUTINES:"

    Public Shared Function CheckIfReqd_Populate_Envelope(ByVal File_In As IPE_clsFile, _
                                                          ByVal UserInfo_In As IPE_clsUser, _
                                                          ByVal SealType_In As String) As Boolean
        '=========================================================================================  

        Dim pbln = False

        '....Check if the cross-section list in the WorkingDB contains all the cross-sections in the SealDB.
        '........If not, WorkingDB population is required.

        Dim pCount_MainDB As Integer = 0
        If SealType_In = "E-Seal" Then
            pCount_MainDB = IPE_clsESeal.CrossSecList.Count

        ElseIf SealType_In = "C-Seal" Then
            pCount_MainDB = IPE_clsCSeal.CrossSecList.Count

        ElseIf SealType_In = "U-Seal" Then
            pCount_MainDB = IPE_clsUSeal.CrossSecList.Count
        End If

        Dim pCount_Envelope = 0
        For i As Integer = 0 To pCount_MainDB - 1

            If SealType_In = "E-Seal" Then
                If (IPE_clsESeal.CrossSecList_Envelope.Contains(IPE_clsESeal.CrossSecList(i))) Then
                    pCount_Envelope = pCount_Envelope + 1
                End If

            ElseIf SealType_In = "C-Seal" Then
                If (IPE_clsCSeal.CrossSecList_Envelope.Contains(IPE_clsCSeal.CrossSecList(i))) Then
                    pCount_Envelope = pCount_Envelope + 1
                End If

            ElseIf SealType_In = "U-Seal" Then
                If (IPE_clsUSeal.CrossSecList_Envelope.Contains(IPE_clsUSeal.CrossSecList(i))) Then
                    pCount_Envelope = pCount_Envelope + 1
                End If

            End If
        Next

        If pCount_Envelope < pCount_MainDB Then
            pbln = True
        Else
            pbln = False
        End If

        Return pbln

    End Function


    Public Shared Sub Populate_Envelope(ByVal SealType_In As String)
        '===========================================================
        '
        'This routine populates the fields - fldCrossSecNo, fldHfreeStd, fldDHfreeAdjMax, 
        '....fldWidMax of the following tables of the 
        '....Seal DB for the corresponding cross-sections:   
        '    1.  "tblESeal_Envelope". 
        '    2.  "tblCSeal_Envelope".
        '    3.  "tblUSeal_Envelope".

        '......Save the chosen culture name.
        Dim pCultureName_Chosen As String = Thread.CurrentThread.CurrentCulture.Name()

        '......Temporarily set the current culture to 'USA'. Required for storing data into the database.
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        Dim pSealDBEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pSeal As IPE_clsSeal = Nothing
        Dim pCrossSecNo As String
        Dim pHFreeStd As Single
        Dim pDHFreeAdjMax As Single
        Dim pWidMax As Single

        Try

            If SealType_In = "E-Seal" Then

                '....Local ESeal Object 
                pSeal = New IPE_clsESeal("E-Seal", "English")

                Dim pQryESealEnvelope = (From pRec In pSealDBEntities.tblESeal_Envelope Select pRec).ToList()

                Dim pRec_ESealEnvelope As tblESeal_Envelope
                For Each pRec_ESealEnvelope In pQryESealEnvelope
                    pSealDBEntities.DeleteObject(pRec_ESealEnvelope)

                Next
                pSealDBEntities.SaveChanges()

                '....ESealGeom
                Dim pQryESealGeom = (From it In pSealDBEntities.tblESeal_Geom Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryESealGeom.Count - 1
                    pCrossSecNo = pQryESealGeom(i)

                    With pSeal
                        .MCrossSecNo = pCrossSecNo
                        pHFreeStd = .HfreeStd
                        pDHFreeAdjMax = .DHfreeAdjMax

                        If .WidMax > mcEPS Then
                            pWidMax = .WidMax

                        Else
                            MsgBox("ESeal CrossSecNo = " & pCrossSecNo & _
                                       " has zero WidMax value in the Manf. Data table Of the ESealDB." & vbCrLf & _
                                       "The Program uses the Std. Width.")
                            pWidMax = .WidStd
                        End If
                    End With

                    Dim pESealEnvelope As New tblESeal_Envelope
                    With pESealEnvelope
                        .fldCrossSecNo = pSeal.MCrossSecNo
                        .fldHfreeStd = pHFreeStd
                        .fldDHfreeAdjMax = pDHFreeAdjMax
                        .fldWidMax = pWidMax
                    End With

                    pSealDBEntities.AddTotblESeal_Envelope(pESealEnvelope)

                Next
                pSealDBEntities.SaveChanges()


            ElseIf SealType_In = "C-Seal" Then
                '....CSeal

                '....Local CSeal Object 
                pSeal = New IPE_clsCSeal("C-Seal", "English")

                Dim pQryCSealEnvelope = (From pRec In pSealDBEntities.tblCSeal_Envelope Select pRec).ToList()

                Dim pRec_SealCandidate As tblCSeal_Envelope
                For Each pRec_SealCandidate In pQryCSealEnvelope
                    pSealDBEntities.DeleteObject(pRec_SealCandidate)

                Next
                pSealDBEntities.SaveChanges()

                '....CSealGeom
                Dim pQryCSealGeom = (From it In pSealDBEntities.tblCSeal_Geom Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryCSealGeom.Count - 1
                    pCrossSecNo = pQryCSealGeom(i)

                    With pSeal
                        .MCrossSecNo = pCrossSecNo
                        pHFreeStd = .HfreeStd
                        pDHFreeAdjMax = .DHfreeAdjMax

                        pWidMax = .WidStd
                    End With

                    Dim pCSealEnvelope As New tblCSeal_Envelope

                    With pCSealEnvelope
                        .fldCrossSecNo = pSeal.MCrossSecNo
                        .fldHfreeStd = pHFreeStd
                        .fldDHfreeAdjMax = pDHFreeAdjMax
                        .fldWidMax = pWidMax
                    End With
                    pSealDBEntities.AddTotblCSeal_Envelope(pCSealEnvelope)

                Next

                pSealDBEntities.SaveChanges()


            ElseIf SealType_In = "U-Seal" Then
                '....USeal

                '....Local USeal Object 
                pSeal = New IPE_clsUSeal("U-Seal", "English")

                Dim pQryUSealEnvelope = (From pRec In pSealDBEntities.tblUSeal_Envelope Select pRec).ToList()

                Dim pRec_SealCandidate As tblUSeal_Envelope
                For Each pRec_SealCandidate In pQryUSealEnvelope
                    pSealDBEntities.DeleteObject(pRec_SealCandidate)

                Next
                pSealDBEntities.SaveChanges()

                '....USealGeom
                Dim pQryUSealGeom = (From it In pSealDBEntities.tblUSeal_Geom Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryUSealGeom.Count - 1
                    pCrossSecNo = pQryUSealGeom(i)

                    With pSeal
                        .MCrossSecNo = pCrossSecNo
                        pHFreeStd = .HfreeStd
                        pDHFreeAdjMax = .DHfreeAdjMax

                        pWidMax = .WidStd
                    End With

                    Dim pUSealEnvelope As New tblUSeal_Envelope

                    With pUSealEnvelope
                        .fldCrossSecNo = pSeal.MCrossSecNo
                        .fldHfreeStd = pHFreeStd
                        .fldDHfreeAdjMax = pDHFreeAdjMax
                        .fldWidMax = pWidMax
                    End With
                    pSealDBEntities.AddTotblUSeal_Envelope(pUSealEnvelope)

                Next
                pSealDBEntities.SaveChanges()

            End If

        Catch ex As Exception

        End Try

        '.....Reset the culture back to the "user-chosen" one. 
        Thread.CurrentThread.CurrentCulture = New CultureInfo(pCultureName_Chosen)

    End Sub


    Public Function Update_Candidate_CrossSecs(ByVal CavityWidMin_In As Single, _
                                               ByVal CavityDepth_In As Single, ByVal HFree_Rep_In As Single, _
                                               ByRef CandidateList As List(Of Boolean)) As Boolean
        '======================================================================================================
        'This method will select the candidate designs in the workingDB, if any.

        '....Check if the cavity envelope has been adequately defined. 
        '........Otherwise, exit the routine.
        If CavityWidMin_In < gcEPS Or CavityDepth_In < gcEPS Then
            Return False
            Exit Function
        End If


        'Cavity envelope has been defined. Check if any candidate design set can be found.
        '---------------------------------------------------------------------------------
        '....Get dimensions: mWidMaxAllowed & mHFreeMinAllowed
        GetDimensionLimits(CavityWidMin_In, CavityDepth_In, HFree_Rep_In)

        '....Check Candidate Designs.
        Dim pblnSuccess As Boolean = Check_CandidateCrossSecs(CandidateList)
        Return pblnSuccess

    End Function


    Private Sub GetDimensionLimits(ByVal CavityWidMin_In As Single, ByVal CavityDepth_In As Single, _
                                   ByVal HFree_Rep_In As Single)
        '=============================================================================================
        'This routine calculates the limits (lower or upper) on the Seal dimensions.

        'Min. Free Height allowed:      
        '-------------------------
        mHFreeMinAllowed = CavityDepth_In / (1 - mcCompressPcentValueMinReqd / 100)

        'Max. Width allowed:
        '-------------------
        '
        '....Get ZClear & H11Tol:

        '........Temporary Local Seal Object:    
        Dim pSeal As IPE_clsSeal = Nothing
        If mType = "E-Seal" Then
            pSeal = New IPE_clsESeal(mType, mUnit.System, mPOrient)

        ElseIf mType = "C-Seal" Then
            pSeal = New IPE_clsCSeal(mType, mUnit.System, mPOrient)

        ElseIf mType = "U-Seal" Then
            pSeal = New IPE_clsUSeal(mType, mUnit.System, mPOrient)
        End If


        mZClear = pSeal.ZClear_Calc(HFree_Rep_In)

        Dim pDControl As Single
        pDControl = pSeal.DControl_Calc(mPOrient, mCavityDia, mZClear)

        mH11Tol = pSeal.H11Tol_Calc(pDControl)       '....Tolerance on the Control Dia.


        '....Max allowable Seal Width.
        Dim psngMargin As Single
        psngMargin = 0.5 * ((1 + FacBuckling) * mZClear + mH11Tol)
        mWidMaxAllowed = CavityWidMin_In - psngMargin

        pSeal = Nothing

    End Sub


    Private Function Check_CandidateCrossSecs(ByRef CandidateList As List(Of Boolean)) As Boolean
        '========================================================================================
        'This routine checks the candidate cross-sections in the Working DB that conform
        '....to the cavity envelope as per the given design constraints e.g.
        '........1. mWidMaxAllowed
        '........2. mHFreeMinAllowed

        Dim pSealEnvelope As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim iCount As Integer = 0       '....Initialize
        Dim pCrossSecNo As String

        Dim pArrayCrossSecNo() As String = {}
        Dim pblnArrayCandidate() As Boolean = {}
        Dim jRec As Int16 = 0

        Dim pWidMax As Single
        Dim pHfreeStd As Single
        Dim pDHfreeAdjMax As Single
        Dim pHfreeMax As Single

        If mType = "E-Seal" Then
            Dim pQryESealCandidate = (From pRec In pSealEnvelope.tblESeal_Envelope
                                        Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()

            'Select candidate designs.
            '========================
            Dim pRec_SealCandidate As tblESeal_Envelope
            For Each pRec_SealCandidate In pQryESealCandidate

                jRec = jRec + 1
                pCrossSecNo = pRec_SealCandidate.fldCrossSecNo

                'Determine if the above cross-section is a candidate design. 
                '-----------------------------------------------------------
                '
                pWidMax = pRec_SealCandidate.fldWidMax * mUnit.CFacConL

                pHfreeStd = pRec_SealCandidate.fldHfreeStd * mUnit.CFacConL

                pDHfreeAdjMax = pRec_SealCandidate.fldDHfreeAdjMax * mUnit.CFacConL

                '....Estimate Max. Possible Hfree with the current cross-section.

                pHfreeMax = pHfreeStd + pDHfreeAdjMax

                Dim pblnCandidate As Boolean
                If (pWidMax <= mWidMaxAllowed) And (pHfreeMax >= mHFreeMinAllowed) Then
                    pblnCandidate = True
                    iCount = iCount + 1

                Else
                    pblnCandidate = False
                End If

                ReDim Preserve pArrayCrossSecNo(jRec + 1)       '....The '0' index element         
                ReDim Preserve pblnArrayCandidate(jRec + 1)     '........to be disregarded.

                pArrayCrossSecNo(jRec) = pCrossSecNo
                pblnArrayCandidate(jRec) = pblnCandidate

                CandidateList(jRec - 1) = pblnCandidate     'AES 28MAR16

            Next

        ElseIf mType = "C-Seal" Then
            Dim pQryCSealCandidate = (From pRec In pSealEnvelope.tblCSeal_Envelope
                                        Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()

            'Select candidate designs.
            '========================
            Dim pRec_SealCandidate As tblCSeal_Envelope
            For Each pRec_SealCandidate In pQryCSealCandidate
                jRec = jRec + 1
                pCrossSecNo = pRec_SealCandidate.fldCrossSecNo

                'Determine if the above cross-section is a candidate design. 
                '-----------------------------------------------------------
                '
                pWidMax = pRec_SealCandidate.fldWidMax * mUnit.CFacConL

                pHfreeStd = pRec_SealCandidate.fldHfreeStd * mUnit.CFacConL

                pDHfreeAdjMax = pRec_SealCandidate.fldDHfreeAdjMax * mUnit.CFacConL

                '....Estimate Max. Possible Hfree with the current cross-section.
                pHfreeMax = pHfreeStd + pDHfreeAdjMax

                Dim pblnCandidate As Boolean
                If (pWidMax <= mWidMaxAllowed) And (pHfreeMax >= mHFreeMinAllowed) Then
                    pblnCandidate = True
                    iCount = iCount + 1

                Else
                    pblnCandidate = False
                End If

                ReDim Preserve pArrayCrossSecNo(jRec + 1)       '....The '0' index element         
                ReDim Preserve pblnArrayCandidate(jRec + 1)     '........to be disregarded.

                pArrayCrossSecNo(jRec) = pCrossSecNo
                pblnArrayCandidate(jRec) = pblnCandidate

                CandidateList(jRec - 1) = pblnCandidate
            Next

        ElseIf mType = "U-Seal" Then
            Dim pQryUSealCandidate = (From pRec In pSealEnvelope.tblUSeal_Envelope
                                      Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()

            'Select candidate designs.
            '========================
            Dim pRec_SealCandidate As tblUSeal_Envelope
            For Each pRec_SealCandidate In pQryUSealCandidate
                jRec = jRec + 1
                pCrossSecNo = pRec_SealCandidate.fldCrossSecNo

                'Determine if the above cross-section is a candidate design. 
                '-----------------------------------------------------------
                '
                pWidMax = pRec_SealCandidate.fldWidMax * mUnit.CFacConL

                pHfreeStd = pRec_SealCandidate.fldHfreeStd * mUnit.CFacConL

                pDHfreeAdjMax = pRec_SealCandidate.fldDHfreeAdjMax * mUnit.CFacConL

                '....Estimate Max. Possible Hfree with the current cross-section.

                pHfreeMax = pHfreeStd + pDHfreeAdjMax

                Dim pblnCandidate As Boolean
                If (pWidMax <= mWidMaxAllowed) And (pHfreeMax >= mHFreeMinAllowed) Then
                    pblnCandidate = True
                    iCount = iCount + 1

                Else
                    pblnCandidate = False
                End If

                ReDim Preserve pArrayCrossSecNo(jRec + 1)       '....The '0' index element         
                ReDim Preserve pblnArrayCandidate(jRec + 1)     '........to be disregarded.

                pArrayCrossSecNo(jRec) = pCrossSecNo
                pblnArrayCandidate(jRec) = pblnCandidate

                CandidateList(jRec - 1) = pblnCandidate
            Next

        End If

        '....Check if any candidate design set has been found.
        If iCount = 0 Then
            Return False
        ElseIf iCount > 0 Then
            Return True
        End If

    End Function

#End Region

End Class