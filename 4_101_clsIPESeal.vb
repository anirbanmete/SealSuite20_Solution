
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsSeal                                '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  15MAR17                                '
'                                                                              '
'===============================================================================

Imports System.Collections.Specialized
Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Drawing.Drawing2D
Imports clsLibrary11
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing

<Serializable()> _
Public MustInherit Class IPE_clsSeal
    Implements ICloneable


#Region "MEMBER VARIABLE DECLARATIONS:"

    Protected Const mcCavityCornerRad_Def As Single = 0.02F
    Protected Const mcCountSegment As Integer = 3
    '
    '   GENERAL INFORMATION:
    '   -------------------
    Private mType As String                             '(FUNDAMENTAL)
    Private mMCrossSecNo As String                      '(FUNDAMENTAL)

    Private mIsSegmented As Boolean
    Private mCountSegment As Integer = 1 'PB 01MAY16


    '   GEOMETRY PARAMETERS:
    '   --------------------
    '
    '   ....Standard:
    Protected mHfreeStd As Single                         '(RETRIEVED)   - Std. Geom.
    Protected mHfreeTolStd As Single                      '(RETRIEVED) 
    Private mHfreeTol(2) As Single    '....1 : - Tol.,    '(FUNDAMENTAL)     
    '                                 '....2 : + Tol.     

    Protected mWidStd As Single                           '(RETRIEVED)   - Std. Geom.
    Protected mTStd As Single                             '(RETRIEVED)   - Std. Geom.


    '   ....Adjusted:
    Protected mHfree As Single                            '(FUNDAMENTAL) - C-Seal: Non-Std.
    '                                                     '(DERIVED)     - E-Seal: Std. & Non-Std.
    '   ........Adjustment Decrement/Increment Limits about HfreeStd (Absolute Values):
    Protected mDHfreeAdjMin As Single                      '(DERIVED from RETRIEVED data)
    Protected mDHfreeAdjMax As Single                      '(DERIVED from RETRIEVED data)  

    Protected mWid As Single                              '(DERIVED)     - C-Seal: Std. & Non-Std.  
    '                                                     '(DERIVED)     - E-Seal: Std. & Non-Std.

    '   ----------
    '   T Related:      
    '   ----------
    '
    Protected mT As Single                      '(RETRIEVED)   - C-Seal: Std.
    '                                           '(FUNDAMENTAL) - C-Seal: Non-Std.
    '                                           '(RETRIEVED)   - E-Seal: Std. & Non-Std.

    '   ....Standard Plate Thicknesses Available.   
    Protected mArrayTStd_Eng() As Single = {0.006, 0.007, 0.008, 0.009, 0.01, 0.012, _
                                            0.014, 0.015, 0.016, 0.017, 0.018, 0.02, _
                                            0.024, 0.025, 0.03, 0.038, 0.05, 0.065}

    Protected mUBArrayTStd As Int16    '....Upper Bound of the Array.
    Protected mIndexTStd As Int16      '....Index corresponding to the current TStd.  

    '----------------------------------------------------------------------------------

    Protected mAdjusted As String                         '(DERIVED)


    '   ....Control dia related parameters:
    Private mDControl As Single                           '(DERIVED)
    Private mH11Tol As Single                             '(DERIVED)
    Protected mZClear As Single                           '(DERIVED)
    Private mZClear_Given As Single

    Protected mCavityCornerRad As Single                  '(RETRIEVED)   - to be passed 
    '                                                     '......on to the Cavity class.

    '   ....Manufacturing Data.
    Protected mDiMin As Single                            '(RETRIEVED) 
    Protected mWidMax As Single                           '(RETRIEVED)  
    Protected mStripWid As Single                         '(RETRIEVED)


    '   FEA PARAMETERS:  
    '   --------------- 
    Private mNLayer As Integer                                          '(FUNDAMENTAL)

    '   ....No. of Contact Elements on either side of various contact regions.
    Private mNelConSeal As Integer  '....Sealing Point.                 '(FUNDAMENTAL)
    Private mNelConRad As Integer   '....Radial  Point(s) at the        '(FUNDAMENTAL)
    '                               '........Bottom (External) or 
    '                               '........Top    (Internal) of the Seal.

    Private mNelMax As Integer      '....Max # of Elements allowed.     

    '....Arc length of the cross-section profile of the half model in 
    '........the meridional direction.
    Protected mLenArcHalfModel As Single
    Private mElSize As Single       '....Element size in the meridional direction.


    '   ....Contact Model Parameters:
    Private mFacKN As Single                                            '(FUNDAMENTAL)  
    Private mFacTOLN As Single                                          '(FUNDAMENTAL) 

    '   ....Clustering Parameters:
    Private mBetaT As Single            ' Thickness  Direction          '(FUNDAMENTAL)
    Private mBetaM As Single            ' Meridional Direction          '(FUNDAMENTAL)


    '....Load Substepping Control Parameters:
    Private mNSBSTP As Integer      '........Defines the first substep size.
    Private mNSBMX As Integer       '........Max. no. of substeps (Min. Load step size)
    Private mNSBMN As Integer       '........Min. no. of substeps (Max. Load step size)


    '   FEA RESULTS:
    '   ------------
    Private mDSealing As Single                                         '(DERIVED)

    '   ....After Load Cycling. 
    Private mFSeat_Unit(10) As Single    'mFConUnit(10)                                  '(DERIVED)
    Private mStressMax(10) As Single                                     '(DERIVED)
    Private mHfreeFinal As Single       '....After permanent set.       '(DERIVED)
    Private mPeneMax1 As Single         '....Max Penetration @ LS = 1   '(DERIVED)


    '   GRAPHICS PARAMETERS:
    '   -------------------
    '
    '   ....Scaling between the two coordinate systems - 'World Coordinate System'(WCS) &
    '   ........the 'Page Coordinate System' (PCS) attached to the picturebox.
    Protected mScalePCS As Single
    Protected mMarginMod(4) As Single
    Protected mXVB_OrigWCS As Single, mYVB_OrigWCS As Single


    'SECONDARY ASSIGNMENT MEMBER VARIABLES:        
    '---------------------------------------
    Protected mUnit As IPE_clsUnit
    Private mPOrient As String
    Private mCavityDia(2) As Single     '....Needed in the DControl calculation.

    Private mMat As IPE_clsMaterial

    Private mSW As StreamWriter         '....Stream Writer for DXf File.
    Private mSelected As Boolean = False

#End Region


#Region "CLASS PROPERTY ROUTINES:"

#Region "READ & WRITE PROPERTIES"
    '--------------------------------

    '....Cross Section No.
    Public Overridable Property MCrossSecNo() As String
        '=============================================
        Get
            Return mMCrossSecNo
        End Get

        Set(ByVal strData As String)
            mMCrossSecNo = strData
        End Set

    End Property


    '....POrient.
    Public Overridable Property POrient() As String
        '==========================================
        Get
            Return mPOrient
        End Get

        Set(ByVal strData As String)
            '------------------------
            mPOrient = strData
            Update_ControlDiaParams()
        End Set

    End Property


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


    '....IsSegmented
    Public Property IsSegmented() As Boolean
        '============================
        Get
            Return mIsSegmented
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsSegmented = blnData

            If mIsSegmented = False Then
                mCountSegment = mcCountSegment
            End If
        End Set

    End Property


    '....No of Segments
    Public Property CountSegment() As Integer
        '====================================
        Get
            Return mCountSegment
        End Get

        Set(ByVal sngData As Integer)
            '---------------------------------
            mCountSegment = sngData
        End Set

    End Property


    '....Hfree Tolerances:
    Public Property HFreeTol(ByVal i As Integer) As Single
        '=================================================
        Get
            Return mHfreeTol(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mHfreeTol(i) = sngData
        End Set
    End Property


    Public Overridable Property T() As Single
        '====================================
        Get
            Return mT
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mT = sngData

        End Set

    End Property


    Public Property Mat() As IPE_clsMaterial
        '================================
        Get
            Return mMat
        End Get
        Set(Obj As IPE_clsMaterial)
            mMat = Obj
        End Set
    End Property


    Public Property Selected() As Boolean
        '================================
        Get
            Return mSelected
        End Get

        Set(ByVal sngData As Boolean)
            '---------------------------------
            mSelected = sngData
        End Set

    End Property


    '....ZClear_Given.
    Public Property ZClear_Given() As Single
        '===================================
        Get
            Return mZClear_Given
        End Get

        Set(ByVal sngData As Single)
            '---------------------------------
            mZClear_Given = sngData
        End Set
    End Property


#Region "ANSYS MODEL PARAMETERS:"

    '....NLayer.
    Public Property NLayer() As Integer
        '==============================
        Get
            Return mNLayer
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mNLayer = sngData
        End Set
    End Property


    '....NelConSeal.
    Public Property NelConSeal() As Integer
        '==================================
        Get
            Return mNelConSeal
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mNelConSeal = sngData
        End Set
    End Property


    '....NelConRad.
    Public Property NelConRad() As Integer
        '==================================
        Get
            Return mNelConRad
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mNelConRad = sngData
        End Set
    End Property


    Public Property NelMax() As Integer
        '===============================        
        Get
            Return mNelMax
        End Get

        Set(ByVal value As Integer)
            mNelMax = value
            Calc_ElSize()
        End Set
    End Property


    '....FacKN.                                 
    Public Property FacKN() As Single
        '============================
        Get
            Return mFacKN
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mFacKN = sngData
        End Set
    End Property


    '....FacTOLN.
    Public Property FacTOLN() As Single
        '============================
        Get
            Return mFacTOLN
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mFacTOLN = sngData
        End Set
    End Property


    '....BetaT.
    Public Property BetaT() As Single
        '============================
        Get
            Return mBetaT
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mBetaT = sngData
        End Set
    End Property


    '....BetaM.
    Public Property BetaM() As Single
        '============================
        Get
            Return mBetaM
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mBetaM = sngData
        End Set
    End Property


    '........ANSYS LOAD SUB-STEPPING PARAMETERS.

    '....NSBSTP.
    Public Property NSBSTP() As Integer
        '==============================     
        Get
            Return mNSBSTP
        End Get


        Set(ByVal sngData As Integer)
            '------------------------
            If sngData <= 0 Then
                mNSBSTP = 25
            Else
                mNSBSTP = sngData
            End If

        End Set

    End Property


    '....NSBMX.
    Public Property NSBMX() As Integer
        '==============================     
        Get
            Return mNSBMX
        End Get


        Set(ByVal sngData As Integer)
            '------------------------
            If sngData <= 0 Then
                mNSBMX = 25
            Else
                mNSBMX = sngData
            End If

        End Set

    End Property


    '....NSBMN.
    Public Property NSBMN() As Integer
        '==============================     
        Get
            Return mNSBMN
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            If sngData <= 0 Then
                mNSBMN = 5
            Else
                mNSBMN = sngData
            End If

        End Set

    End Property

#End Region


#Region "ANSYS FEA RESULTS:"

    '....DSealing.
    Public Property DSealing() As Single
        '================================
        Get
            Return mDSealing
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDSealing = sngData
        End Set
    End Property


    '....HfreeFinal. After loading cycle upon parmanent set.
    Public Property HfreeFinal() As Single
        '=================================
        Get
            Return mHfreeFinal
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mHfreeFinal = sngData
        End Set
    End Property


    '....FConUnit : Contact Force/Circumference @ different load steps.
    Public Property FSeat_Unit(ByVal i As Integer) As Single
        '=================================================      'AES 15MAR17    FConUnit => FSeat_Unit
        Get
            Return mFSeat_Unit(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mFSeat_Unit(i) = sngData
        End Set
    End Property


    '....Maximum Stresses @ different load steps:
    Public Property StressMax(ByVal i As Integer) As Single
        '==================================================
        Get
            Return mStressMax(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mStressMax(i) = sngData
        End Set
    End Property


    '....Maximum Penetration @ Load Step = 1:
    Public Property PeneMax1() As Single
        '===============================
        Get
            Return mPeneMax1
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mPeneMax1 = sngData
        End Set
    End Property

#End Region

#End Region


#Region "READ-ONLY PROPERTIES"
    '-----------------------------

    'Public Shared ReadOnly Property LoadCaseDesc As StringCollection
    '    '===========================================================
    '    Get
    '        Return mLoadCaseDesc
    '    End Get
    'End Property

    '....Type.
    Public ReadOnly Property Type() As String
        '====================================
        Get
            Return mType
        End Get
    End Property


    'Free Height:
    '------------
    '
    '....HFreeStd (Standard).
    Public ReadOnly Property HfreeStd() As Single
        '========================================
        Get
            Return mHfreeStd
        End Get
    End Property


    '....DHfreeAdjMin (Absolute Value).
    Public ReadOnly Property DHfreeAdjMin() As Single
        '===========================================
        Get
            Return mDHfreeAdjMin
        End Get
    End Property


    '....DHfreeAdjMax.
    Public ReadOnly Property DHfreeAdjMax() As Single
        '===========================================
        Get
            Return mDHfreeAdjMax
        End Get
    End Property


    '....HFree (Adjusted value, if any).  
    Public ReadOnly Property Hfree() As Single
        '============================
        Get
            Return mHfree
        End Get
    End Property


    '....Hfree Standard build tolerance.
    Public ReadOnly Property HfreeTolStd() As Single
        '===========================================
        Get
            Return mHfreeTolStd
        End Get
    End Property


    'Width:
    '-----
    '
    '....Standard Width.
    Public ReadOnly Property WidStd() As Single
        '======================================
        Get
            Return mWidStd
        End Get
    End Property


    '....Width (Adjusted value, if any). 
    Public ReadOnly Property Wid() As Single
        '===================================
        Get
            Return mWid
        End Get
    End Property


    '....Width Max         
    Public ReadOnly Property WidMax() As Single
        '======================================
        Get
            Return mWidMax
        End Get
    End Property


    '....Strip Wid         
    Public ReadOnly Property StripWid() As Single
        '======================================
        Get
            Return mStripWid
        End Get

    End Property



    '   ------------
    '   "T" Related:            
    '   ------------
    '
    '....Standard T.
    Public ReadOnly Property TStd() As Single
        '====================================
        Get
            Return mTStd
        End Get
    End Property

    '
    '....ArrayTStd.
    Public ReadOnly Property ArrayTStd(ByVal i As Integer) As Single
        '===========================================================
        Get
            Return mArrayTStd_Eng(i) * mUnit.CFacConL
        End Get
    End Property


    '....Upper Bound of the "ArrayTStd".
    Public ReadOnly Property UBArrayTStd() As Int16
        '==========================================
        Get
            Return UBound(mArrayTStd_Eng)
        End Get
    End Property


    '....Index corresponding to TStd. 
    Public ReadOnly Property IndexTStd() As Int16
        '=========================================
        Get
            Return GetIndex_ArrayTStd(mTStd, "Floor")
        End Get
    End Property


    '....Index corresponding to mT member value. 
    '........If the given mT doesn't fall on any discrete array value, return the index
    '........corresponding to "Ceiling". However, one exception is if the mT value is 
    '........lower than the ArrayTStd (0), the "0" index is returned.
    '
    Public ReadOnly Property IndexT() As Int16
        '=====================================
        Get
            Return GetIndex_ArrayTStd(mT, "Ceiling")
        End Get
    End Property

    '------------------------------------------------------------------------------------

    '....Adjusted Geometry.
    Public ReadOnly Property Adjusted() As String
        '========================================
        Get
            Return mAdjusted
        End Get
    End Property


    '....Control Dia.
    Public ReadOnly Property DControl() As Single
        '========================================
        Get
            Return mDControl
        End Get
    End Property


    '....ZClear.
    Public ReadOnly Property ZClear() As Single
        '======================================
        Get
            Return mZClear
        End Get

    End Property


    '....H11Tol.
    Public ReadOnly Property H11Tol() As Single
        '======================================
        Get
            Return mH11Tol
        End Get
    End Property


    '....Cavity Corner Radius.
    Public ReadOnly Property CavityCornerRad() As Single
        '===============================================
        Get
            Return mCavityCornerRad
        End Get
    End Property


    '....DiMin.
    Public ReadOnly Property DiMin() As Single
        '=====================================
        Get
            Return mDiMin
        End Get
    End Property


    Public ReadOnly Property ElSize() As String
        '======================================
        Get
            Return mElSize
        End Get
    End Property

#End Region


#Region "WRITE-ONLY PROPERTIES"
    '-------------------------------

    '....Cavity Diameters:
    Public WriteOnly Property CavityDia(ByVal i As Integer) As Single
        '============================================================
        Set(ByVal sngData As Single)
            mCavityDia(i) = sngData
            Update_ControlDiaParams()
        End Set
    End Property


#End Region

#End Region


#Region "CLASS METHODS:"
    '====================

    Public Sub New(ByVal strType_In As String, _
                   Optional ByVal strUnitSystem_In As String = "English", _
                   Optional ByVal strPOrient_In As String = "External")
        '===================================================================

        If Trim(strType_In) <> "C-Seal" And _
           Trim(strType_In) <> "E-Seal" And _
           Trim(strType_In) <> "U-Seal" Then

            MsgBox("Seal Type = " & strType_In & " is not acceptable.")
            Exit Sub
        End If

        mType = strType_In
        mMat = New IPE_clsMaterial(strUnitSystem_In)

        mUnit = New IPE_clsUnit(strUnitSystem_In)
        mPOrient = strPOrient_In

        mIsSegmented = False
        mCountSegment = mcCountSegment

        mZClear_Given = 0.0
        mCavityCornerRad = mcCavityCornerRad_Def

        'FEA Parameters: (Default values).
        '---------------
        '....Contact model parameters:
        'mFacKN = 0.01
        mFacKN = 0.001      'AES 12APR16
        mFacTOLN = 0.1

        '....Clustering parameters:
        mBetaT = 1.3
        mBetaM = 5.0#

        Set_DefLoadStepParams()

    End Sub


    Protected Shared Sub Retrieve_CrossSections(ByRef CrossSecList_In As StringCollection, _
                                                ByVal DBTypeName_In As String, ByVal TblName_In As String)
        '=================================================================================================
        '....Populate the CrossSec List:
        CrossSecList_In.Clear()

        If (DBTypeName_In = "SealDB") Then


            Dim pSealMCSDBEntities As New SealIPEMCSDBEntities()

            '....Populate the CrossSec List:
            CrossSecList_In.Clear()

            If (TblName_In = "tblESeal_Geom") Then

                Try

                    '....ESeal
                    Dim pQryESealGeom As Object = (From pRec In pSealMCSDBEntities.tblESeal_Geom
                                                    Select pRec.fldCrossSecNo Distinct).ToList()

                    For i As Integer = 0 To pQryESealGeom.Count - 1
                        CrossSecList_In.Add(pQryESealGeom(i))
                    Next

                Catch ex As Exception

                End Try

            ElseIf (TblName_In = "tblESeal_Envelope") Then
                '....ESeal Envelope
                Dim pQryESeal_Envelope As Object = (From it In pSealMCSDBEntities.tblESeal_Envelope
                                   Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryESeal_Envelope.Count - 1
                    CrossSecList_In.Add(pQryESeal_Envelope(i))
                Next

            ElseIf (TblName_In = "tblCSeal_Geom") Then
                '....CSeal
                Dim pQryCSeal_Geom As Object = (From it In pSealMCSDBEntities.tblCSeal_Geom
                                  Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryCSeal_Geom.Count - 1
                    CrossSecList_In.Add(pQryCSeal_Geom(i))
                Next

            ElseIf (TblName_In = "tblCSeal_Envelope") Then
                '....CSeal Envelope
                Dim pQryCSeal_Envelope As Object = (From it In pSealMCSDBEntities.tblCSeal_Envelope
                                 Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryCSeal_Envelope.Count - 1
                    CrossSecList_In.Add(pQryCSeal_Envelope(i))
                Next

            ElseIf (TblName_In = "tblUSeal_Geom") Then
                '....USeal
                Dim pQryUSeal_Geom As Object = (From it In pSealMCSDBEntities.tblUSeal_Geom
                               Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryUSeal_Geom.Count - 1
                    CrossSecList_In.Add(pQryUSeal_Geom(i))
                Next

            ElseIf (TblName_In = "tblUSeal_Envelope") Then
                '....USeal Envelope
                Dim pQryUSeal_Envelope As Object = (From it In pSealMCSDBEntities.tblUSeal_Envelope
                              Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryUSeal_Envelope.Count - 1
                    CrossSecList_In.Add(pQryUSeal_Envelope(i))
                Next

            End If


        ElseIf (DBTypeName_In = "SealNewDB") Then

            Dim pSealNewDBUserEntities As New SealIPEMCSDBEntities

            If (TblName_In = "tblESeal_Geom") Then
                '....ESeal
                Dim pQryESeal_Geom As Object = (From it In pSealNewDBUserEntities.tblESealNew_Geom
                              Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryESeal_Geom.Count - 1
                    CrossSecList_In.Add(pQryESeal_Geom(i))
                Next


            ElseIf (TblName_In = "tblUSeal_Geom") Then
                '....USeal
                Dim pQryUSeal_Geom As Object = (From it In pSealNewDBUserEntities.tblUSealNew_Geom
                             Select it.fldCrossSecNo Distinct).ToList()

                For i As Integer = 0 To pQryUSeal_Geom.Count - 1
                    CrossSecList_In.Add(pQryUSeal_Geom(i))
                Next

            End If

        End If

    End Sub


    Protected Function GetIndex_ArrayTStd(ByVal T_In As Single, ByVal IndexType_In As String) As Int16
        '=============================================================================================
        'For a given thickness value 'T_In', this function gets an appropriate index  
        '....in the array "ArrayTStd" that corresponds to it, whereas
        '........IndexType_In = "Floor" or "Ceiling".

        Const pcT_Tol As Single = 0.00025         '....A representative Thick. Tol. (in).
        Dim pUB As Int16 = UBound(mArrayTStd_Eng) '....Upper bound of the TStd Array.     

        '....Change unit of T_In from the current Unit ===> Consistent English Unit.
        Dim pT_In_Eng As Single
        pT_In_Eng = T_In / mUnit.CFacConL


        If pT_In_Eng <= mArrayTStd_Eng(0) Then
            '....CASE 1 : pT_In_Eng is less than the first value of ArrayTStd array.
            Return 0


        ElseIf pT_In_Eng >= mArrayTStd_Eng(pUB) Then
            '....CASE 2 : pT_In_Eng is greater than the last value of ArrayTStd array.
            Return pUB


        Else
            '....CASE 3: pT_In_Eng is within the array values.

            Dim j As Integer
            For j = 0 To pUB

                If Abs(pT_In_Eng - mArrayTStd_Eng(j)) <= pcT_Tol Then
                    Return j

                ElseIf pT_In_Eng < mArrayTStd_Eng(j) Then

                    If IndexType_In = "Ceiling" Then
                        Return j

                    ElseIf IndexType_In = "Floor" Then
                        Return j - 1
                    End If
                End If

            Next

        End If

    End Function


    Private Function Calc_ElSize() As Single
        '===================================

        '....# of elements / thickness layer in the meridional direction.
        Dim pNel_Meridional As Single
        pNel_Meridional = Math.Floor(mNelMax / mNLayer)

        '....Element size in the meridional direc.
        mElSize = mLenArcHalfModel / pNel_Meridional
        Return mElSize

    End Function


    Public Sub Set_DefLoadStepParams(Optional ByVal blnPlatingExists_In As Boolean = False)
        '==================================================================================
        '....Load Substepping control parameters:

        'AES 12APR16
        'If blnPlatingExists_In = False Then
        '    mNSBSTP = 25
        '    mNSBMX = 25
        '    mNSBMN = 5

        'ElseIf blnPlatingExists_In = True Then
        '    mNSBSTP = 100
        '    mNSBMX = 100
        '    mNSBMN = 25
        'End If

        'AES 12APR16
        mNSBSTP = 100
        mNSBMX = 100
        mNSBMN = 25


    End Sub


    Protected Sub Update_ControlDiaParams()
        '=================================

        '....Input  Paramters   :  mPOrient, mCavityDia, mHfree. 
        '....Updated Parameters :  mZClear, mDControl, mH11Tol.
        '
        'Get_ControlDiaParams(mPOrient, mCavityDia, mHfree, _
        '                     mDControl, mZClear, mH11Tol)


        'This routine updates: 
        '   1. mZClear 
        '   2. mDControl 
        '   3. mH11Tol

        '....The above parameters depends on:
        '       1. HFree                                 } ====> ZClear 
        '       
        '       2. POrient                               }
        '       3. Cavity ID (POrient = "External") or   } ====> DControl  } ====> H11Tol
        '          Cavity OD (POrient = "Internal")      }

        '....Calculate ZClear
        mZClear = ZClear_Calc(mHfree)

        '....Calculate Control Dia.
        mDControl = DControl_Calc(mPOrient, mCavityDia, mZClear)

        '....Calculate H11Tol
        mH11Tol = H11Tol_Calc(mDControl)       '....Tolerance on the Control Dia.

    End Sub


    Public Function ZClear_Calc(ByVal HFree_In As Single) As Single
        '========================================================== 
        Dim pZClear As Single = 0.0

        If HFree_In < gcEPS Then
            pZClear = 0.0

        Else

            If mType = "C-Seal" Then
                pZClear = CType(Me, IPE_clsCSeal).CalcZClear(HFree_In)

            ElseIf mType = "E-Seal" Then
                pZClear = CType(Me, IPE_clsESeal).CalcZClear(HFree_In)

            ElseIf mType = "U-Seal" Then
                pZClear = CType(Me, IPE_clsUSeal).CalcZClear(HFree_In)
            End If

        End If

        Return pZClear

    End Function


    Public Function DControl_Calc(ByVal POrient_In As String, ByVal cavityDia_In() As Single, ByVal ZClear_In As Single) As Single
        '=========================================================================================================================    
        Dim pDControl As Single = 0.0
        Dim pblnDataValid As Boolean = True

        If POrient_In = "" Then
            pblnDataValid = False

        ElseIf POrient_In = "External" And cavityDia_In(1) < gcEPS Then
            pblnDataValid = False

        ElseIf POrient_In = "Internal" And cavityDia_In(2) < gcEPS Then
            pblnDataValid = False
        End If

        If pblnDataValid = False Then
            pDControl = 0.0

        ElseIf pblnDataValid = True Then

            If POrient_In = "External" Then
                pDControl = cavityDia_In(1) + ZClear_In

            ElseIf POrient_In = "Internal" Then
                pDControl = cavityDia_In(2) - ZClear_In
            End If

        End If

        Return pDControl

    End Function


    Public Function H11Tol_Calc(ByVal DControl_In As Single) As Single
        '=============================================================
        ' This routine calculates the H11 or h11 tolerance on the ID (External) or 
        ' ....OD (Internal).
        '
        ' ....Valid for: E-Seal, C-Seal.

        ' 'Unit Aware' Routine.
        '  -------------------

        '"Tolerance Reference Table"
        '--------------------------
        '......Ref. : Catalogue (Page E-22) - 'H11' tolerances.
        Dim dControlArray() As Object = Nothing
        Dim H11TolArray() As Object = Nothing

        If (DControl_In = 0.0) Then
            H11Tol_Calc = 0.0
            Exit Function
        End If

        If mUnit.System = "English" Then
            '---------------------------
            dControlArray = New Object() {0.12, 0.24, 0.4, 0.71, 1.19, 1.97, 3.15, 4.73, _
                                          7.09, 9.85, 12.41, 15.75, 19.69, 30.09, 41.49, _
                                          56.19, 76.39}                     '....inch

            '....The following dimensions are in 0.001 in (mil).
            '........Needs to be converted to inch before use.
            H11TolArray = New Object() {2.5, 3.0#, 3.5, 4.0#, 5.0#, 6.0#, 7.0#, _
                                        9.0#, 10.0#, 12.0#, 12.0#, 14.0#, 16.0#, 20.0#, _
                                       25.0#, 30.0#, 40.0#}                '.... mils


        ElseIf mUnit.System = "Metric" Then
            '------------------------------
            dControlArray = New Object() {3, 6, 10, 18, 30, 50, 80, 120, 180, 250, 315, _
                                          400, 500, 760, 1050, 1425, 1940}  '....mm

            '....The following dimensions are in 0.001 mm (or micron)
            '........Needs to be converted to m before use.
            H11TolArray = New Object() {60, 75, 90, 110, 130, 160, 190, 220, 250, 290, _
                                        320, 360, 400, 500, 630, 760, 1000} '....microns
        End If

        '....Lower & Upper Bounds of the arrays
        Dim iLow As Integer = LBound(dControlArray)
        Dim iUp As Integer = UBound(dControlArray)

        'Get the appropriate H11 tolerance.
        '----------------------------------
        '....Control Dia in user unit - in or mm.
        Dim diaUserU As Single
        diaUserU = mUnit.L_ConToUser(DControl_In)

        Dim i As Integer
        Dim H11_TableUnit As Single     '....Value in the above Table Unit.

        If diaUserU <= dControlArray(iLow) Then
            H11_TableUnit = H11TolArray(iLow)

        ElseIf diaUserU >= dControlArray(iUp) Then
            H11_TableUnit = H11TolArray(iUp)

        Else
            i = 1
            Do While diaUserU >= dControlArray(i)
                i = i + 1
            Loop

            H11_TableUnit = H11TolArray(i)
        End If

        '...Divide   by 1000 ===> Table unit to User unit.
        '........Then, convert User Unit ===> Consistent Unit (in or m).
        H11Tol_Calc = mUnit.L_UserToCon(H11_TableUnit / 1000)

    End Function


    Public Function HActual(ByVal strHFreeType_In As String, _
                            ByVal strTolType_In As String) As Single
        '============================================================
        '
        'STAND-ALONE FUNCTION:
        '---------------------
        '....Used in other modules, possibly without being associated with any particular
        '........seal design.

        Dim psngHtNominal As Single
        If strHFreeType_In = "Initial" Then
            psngHtNominal = mHfree

        ElseIf strHFreeType_In = "Final" Then
            psngHtNominal = mHfreeFinal
        End If


        If strTolType_In = "Minimum" Then
            HActual = psngHtNominal - mHfreeTol(1)

        ElseIf strTolType_In = "Nominal" Then
            HActual = psngHtNominal

        ElseIf strTolType_In = "Maximum" Then
            HActual = psngHtNominal + mHfreeTol(2)
        End If

    End Function


    Public Function SpringBack(ByVal TolType_In As String, _
                               ByVal CavityDepthActual_In As Single, _
                               ByVal CompressValue_In As Single) As Integer
        '=========================================================================
        'This subroutine calculates seal 'Spring Back' value in (%) after the loading cycle.

        '....Effective Compression after Permanent Set (if any).
        Dim pCompressValueEff As Single
        pCompressValueEff = HActual("Final", TolType_In) - CavityDepthActual_In

        '....Calculate Spring Back (%).
        SpringBack = NInt(pCompressValueEff * 100.0# / CompressValue_In)

    End Function


    Public Function Wt(ByVal sngMatSpWt As Single) As Single
        '===================================================
        Dim psngVolume As Single
        psngVolume = PI * mDControl * mStripWid * mT
        Wt = psngVolume * sngMatSpWt
    End Function


#Region "GRAPHICS ROUTINES:"
    '   ====================

    '....Draw ARC:
    '
    '........Overloaded Version 1:
    '
    Protected Sub Draw_Arc(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                           ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
                           ByVal xCenPCS_In As Single, ByVal yCenPCS_In As Single, _
                           ByVal rad_In As Single, _
                           ByVal angStart_In As Single, ByVal angSweep_In As Single)
        '===========================================================================
        '
        '....TEMPLATE INDEPENDENT.
        '
        'This subroutine draws a circular arc. 

        'Enclosing Rectangle:
        '--------------------
        '....LHS Top.
        Dim pPtLoc As New PointF(xCenPCS_In - rad_In, yCenPCS_In - rad_In) '....Location
        Dim pSizeF As New SizeF(rad_In * 2, rad_In * 2)                    '....Size
        Dim pRect As RectangleF = New RectangleF(pPtLoc, pSizeF)           '....Rectangle      


        '....Pixel densities per unit "PageUnit" dimension (in or mm)
        Dim pDpX As Single
        pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

        Dim pPen As New Pen(color_In, drawWid_In / pDpX)
        pPen.DashStyle = intDashStyle_In
        grphObj_In.DrawArc(pPen, pRect, angStart_In, angSweep_In)
        pPen = Nothing

    End Sub


    '........Overloaded Version 2:
    '
    Protected Sub Draw_Arc(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                         ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
                         ByVal CenPCS_In As PointF, ByVal rad_In As Single, _
                         ByVal angStart_In As Single, ByVal angSweep_In As Single)
        '===========================================================================
        '
        '....TEMPLATE INDEPENDENT.
        '
        'This subroutine draws a circular arc. 

        'Enclosing Rectangle:
        '--------------------
        '....LHS Top.
        Dim pPtLoc As New PointF(CenPCS_In.X - rad_In, CenPCS_In.Y - rad_In) '....Location
        Dim pSizeF As New SizeF(rad_In * 2, rad_In * 2)                      '....Size
        Dim pRect As RectangleF = New RectangleF(pPtLoc, pSizeF)             '....Rectangle      


        '....Pixel densities per unit "PageUnit" dimension (in or mm)
        Dim pDpX As Single
        pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

        Dim pPen As New Pen(color_In, drawWid_In / pDpX)
        pPen.DashStyle = intDashStyle_In

        Try
            grphObj_In.DrawArc(pPen, pRect, angStart_In, angSweep_In)

        Catch pEXP As Exception
            'MsgBox(pEXP.Message)       
        End Try

        pPen = Nothing

    End Sub


    '....Draw LINE:
    '
    Protected Sub Draw_Line(ByVal grphObj_In As Graphics, ByVal color_In As Color, _
                            ByVal drawWid_In As Integer, ByVal intDashStyle_In As Integer, _
                            ByVal ptBeg_In As PointF, ByVal ptEnd_In As PointF)
        '====================================================================================
        '....Used by ESeal, CSeal
        '....TEMPLATE INDEPENDENT.
        '
        'This subroutine draws a line. 

        '....Pixel densities per unit "PageUnit" dimension (in or mm)
        Dim pDpX As Single
        pDpX = grphObj_In.DpiX / mUnit.EngLToUserL(1.0)

        Dim pPen As New Pen(color_In, drawWid_In / pDpX)
        pPen.DashStyle = intDashStyle_In
        grphObj_In.DrawLine(pPen, ptBeg_In, ptEnd_In)
        pPen = Nothing

    End Sub

#End Region


#Region "DXF CREATION ROUTINES:"
    '   =======================

    Protected Sub DXF_Header(ByVal FileName_In As String)
        '=================================================
        If File.Exists(FileName_In) Then _
                File.Delete(FileName_In)

        mSW = File.CreateText(FileName_In)

        '....HEADER.
        With mSW
            .WriteLine("0")
            .WriteLine("SECTION")
            .WriteLine("2")
            .WriteLine("ENTITIES")
        End With

    End Sub


    Protected Sub DXF_Footer()
        '=====================
        '....FOOTER.
        With mSW
            .WriteLine("0")
            .WriteLine("ENDSEC")
            .WriteLine("0")
            .WriteLine("EOF")

            .Close()
        End With

    End Sub


    Protected Sub DXF_Lines_SymAxis(ByVal Y_Sym_Axis_In As Single, ByVal Pt1_In As PointF, _
                                    ByVal Pt2_In As PointF)
        '===============================================================================    
        '....Set the ActiveSpace : ModelSpace
        Dim pLineBeg(2) As Double
        Dim pLineEnd(2) As Double
        Dim pPt1, pPt2 As PointF

        '   Given Line:
        '   -----------
        '
        '....Rotate begin Point 90 degree clockwise direction
        pPt1 = RotateAxes(Pt1_In, -90)

        '....Begin Point
        pLineBeg(0) = pPt1.X
        pLineBeg(1) = pPt1.Y
        pLineBeg(2) = 0

        '....Rotate end Point 90 degree clockwise direction
        pPt2 = RotateAxes(Pt2_In, -90)

        '....End Point
        pLineEnd(0) = pPt2.X
        pLineEnd(1) = pPt2.Y
        pLineEnd(2) = 0

        'pLine = mDWG.ModelSpace.AddLine(pLineBeg, pLineEnd)
        DXF_Line(pLineBeg, pLineEnd)


        '   Symmetry Line:
        '   -------------
        '
        Dim pPt1_Sym, pPt2_Sym As PointF
        pPt1_Sym = RotateAxes(SymPt(Y_Sym_Axis_In, Pt1_In), -90)
        pPt2_Sym = RotateAxes(SymPt(Y_Sym_Axis_In, Pt2_In), -90)

        '....Begin Point
        pLineBeg(0) = pPt1_Sym.X
        pLineBeg(1) = pPt1_Sym.Y
        pLineBeg(2) = 0

        '....End Point
        pLineEnd(0) = pPt2_Sym.X
        pLineEnd(1) = pPt2_Sym.Y
        pLineEnd(2) = 0

        DXF_Line(pLineBeg, pLineEnd)

    End Sub


    Private Sub DXF_Line(ByVal LineBeg_In As Double(), ByVal LineEnd_In As Double())
        '=======================================================================    
        '....LINE Header.
        mSW.WriteLine("0")
        mSW.WriteLine("LINE")

        '....Group Code for Layer name.
        mSW.WriteLine("8")

        '....Layer No.
        mSW.WriteLine("0")

        '....First point of Line
        mSW.WriteLine("10")
        mSW.WriteLine(LineBeg_In(0).ToString())         '  Value of X

        '....Y.
        mSW.WriteLine("20")
        mSW.WriteLine(LineBeg_In(1).ToString())         '  Value of Y

        '....Z.
        mSW.WriteLine("30")
        mSW.WriteLine("0")                              '  Value of Z

        '....Second point of Line
        mSW.WriteLine("11")
        mSW.WriteLine(LineEnd_In(0).ToString())         '  Value of X

        '....Y.
        mSW.WriteLine("21")
        mSW.WriteLine(LineEnd_In(1).ToString())         '  Value of Y

        '....Z.
        mSW.WriteLine("31")
        mSW.WriteLine("0")                              '  Value of Z

    End Sub


    Protected Sub DXF_Arcs_SymAxis(ByVal Y_Sym_Axis_In As Single, ByVal Cen_In As PointF, _
                                   ByVal Rad_In As Single, ByVal StartAng_In As Single, _
                                   ByVal EndAng_In As Single)
        '===================================================================================    
        '....Set the ActiveSpace : ModelSpace
        Dim pCenter(2) As Double
        Dim pCen As PointF

        '....Rotate Centre Point 90 degree clockwise direction
        pCen = RotateAxes(Cen_In, -90)

        pCenter(0) = pCen.X
        pCenter(1) = pCen.Y
        pCenter(2) = 0.0#

        '   Draw Arc.
        '   --------
        DXF_Arc(pCenter, Rad_In, StartAng_In, EndAng_In)

        '   Draw another Arc @ the Symmetry Axis:
        '   -------------------------------------
        '
        Dim pCen_Sym As PointF
        Dim StartAng_Sym As Single
        Dim EndAng_Sym As Single

        '....Rotate Centre Point 90 degree clockwise direction
        pCen_Sym = RotateAxes(SymPt(Y_Sym_Axis_In, Cen_In), -90)

        pCenter(0) = pCen_Sym.X
        pCenter(1) = pCen_Sym.Y
        pCenter(2) = 0.0#

        StartAng_Sym = 180 - EndAng_In
        EndAng_Sym = 180 - StartAng_In

        '....Draw Sym Arc.
        DXF_Arc(pCenter, Rad_In, StartAng_Sym, EndAng_Sym)

    End Sub


    Private Sub DXF_Arc(ByVal Cen_In As Double(), ByVal Rad_In As Single, _
                        ByVal StartAng_In As Single, ByVal EndAng_In As Single)
        '======================================================================         
        '....ARC Header.
        mSW.WriteLine("0")
        mSW.WriteLine("ARC")

        '....Group Code for Layer name.
        mSW.WriteLine("8")

        '....Layer No.
        mSW.WriteLine("0")

        '....First point of Line
        mSW.WriteLine("10")
        mSW.WriteLine(Cen_In(0).ToString())                      '  Value of X
        mSW.WriteLine("20")
        mSW.WriteLine(Cen_In(1).ToString())                      '  Value of Y
        mSW.WriteLine("30")
        mSW.WriteLine("0.0")                                    '  Value of Z
        mSW.WriteLine("40")
        mSW.WriteLine(Rad_In.ToString())                        '  Value of Radius
        mSW.WriteLine("100")

        mSW.WriteLine("AcDbArc")
        mSW.WriteLine("50")
        mSW.WriteLine(StartAng_In.ToString())                   '  Value of Start Angle
        mSW.WriteLine("51")
        mSW.WriteLine(EndAng_In.ToString())                     '  Value of End Angel

    End Sub


    Private Function SymPt(ByVal Y_Sym_Axis_In As Single, ByVal Pt_In As PointF) As PointF
        '===================================================================================    

        Dim pY1_SymDist_Refl As Single              '....holds Symmetry distance
        pY1_SymDist_Refl = Y_Sym_Axis_In - Pt_In.Y

        With SymPt
            .X = Pt_In.X
            .Y = Y_Sym_Axis_In + pY1_SymDist_Refl
        End With

        Return SymPt

    End Function

#End Region


#Region "UTILITY ROUTINES - GRAPHICS & DXF CREATION:"
    '    ===========================================

    Protected Function Pt_PCS(ByVal Pt_WCS As PointF) As PointF
        '==========================================================
        ' This function calculates the coordinates in the "Page Cordinate System (PCS)" when  
        ' ....the  coordinates of a point in the "World Cordinate System (WCS)"are given. 
        ' ....Dependent on "STD" or "ADJ" geometry.

        With Pt_PCS
            .X = mXVB_OrigWCS - mUnit.L_ConToUser(Pt_WCS.Y) * mScalePCS
            .Y = mYVB_OrigWCS - mUnit.L_ConToUser(Pt_WCS.X) * mScalePCS
        End With

        Return Pt_PCS

    End Function


    Protected Function RotateAxes(ByVal pPt_P As PointF, ByVal Alpha As Single) As PointF
        '===============================================================================    
        RotateAxes.X = pPt_P.X * CosD(Alpha) + pPt_P.Y * SinD(Alpha)
        RotateAxes.Y = -pPt_P.X * SinD(Alpha) + pPt_P.Y * CosD(Alpha)

    End Function


    Protected Function TranslateAxes(ByVal pPt_1 As PointF, ByVal Org_1_W As PointF) As PointF
        '===================================================================================
        TranslateAxes.X = pPt_1.X + Org_1_W.X
        TranslateAxes.Y = pPt_1.Y + Org_1_W.Y

    End Function


    '....SCALING:
    Protected Sub CalcScale(ByVal grphObj_In As Graphics, ByVal size_In As SizeF, _
                    ByVal margin_In() As Single, ByVal strScalingGeom_In As String, _
                    ByVal sngMultFacWidDir_In As Single, _
                    ByRef scalePCS_Out As Single, ByRef marginMod_Out() As Single)
        '==============================================================================
        '
        '....TEMPLATE INDEPENDENT.
        '
        'This function calculates an appropriate scale based on the given Geometry Type 
        '....e.g. "STD" or "ADJ".
        '
        '   Input Margins:
        '       margin_In (1) : LHS
        '       margin_In (2) : RHS
        '       margin_In (3) : TOP
        '       margin_In (4) : BOT
        '
        '   Modified Margins:
        '       marginMod_Out ()


        'Seal envelope dimensions in user unit.
        '-------------------------------------
        Dim pHfreeScale As Single
        Dim pWidScale As Single

        If strScalingGeom_In = "SCALE_BY_STD" Then
            pHfreeScale = mHfreeStd
            pWidScale = mWidStd

        ElseIf strScalingGeom_In = "SCALE_BY_ADJ" Then
            pHfreeScale = mHfree
            pWidScale = mWid
        End If


        Dim pHfree_UUnit As Single
        pHfree_UUnit = mUnit.L_ConToUser(pHfreeScale)

        Dim pWid_UUnit As Single
        pWid_UUnit = mUnit.L_ConToUser(pWidScale)


        'Establish SCALING:
        '==================
        '
        'Width Direction - Based on Free Height:
        '---------------------------------------
        '....Along the graphics object's width direction.
        '........Accommodate some extra to account for adjustment of the Free Height.

        Dim psngWidEnv As Single
        psngWidEnv = sngMultFacWidDir_In * pHfree_UUnit

        Dim pMarginTot_WidDir As Single
        pMarginTot_WidDir = margin_In(1) + margin_In(2)

        Dim pScaleW As Single
        pScaleW = (size_In.Width - pMarginTot_WidDir) / psngWidEnv


        'Height Direction - Based on Radial Width:
        '-----------------------------------------
        '....Along the graphics object's height direction.
        Dim psngHtEnv As Single
        psngHtEnv = pWid_UUnit

        Dim pMarginTot_HtDir As Single
        pMarginTot_HtDir = margin_In(3) + margin_In(4)

        Dim pScaleH As Single
        pScaleH = (size_In.Height - pMarginTot_HtDir) / psngHtEnv


        'Scaling for the PCS - choose the smaller of the above two.
        '----------------------------------------------------------
        If pScaleW <= pScaleH Then
            scalePCS_Out = pScaleW

        ElseIf pScaleW > pScaleH Then
            scalePCS_Out = pScaleH
        End If


        'Find the modified margin at the Top & Botm (maintaining the same ratio).
        '-----------------------------------------------------------------------
        Dim pRatio As Single
        pRatio = margin_In(3) / (margin_In(3) + margin_In(4))
        pMarginTot_HtDir = (size_In.Height - pWid_UUnit * scalePCS_Out)

        marginMod_Out(3) = pMarginTot_HtDir * pRatio           '....Top    Margin
        marginMod_Out(4) = pMarginTot_HtDir * (1.0# - pRatio)  '....Bottom Margin

        '....Leave the LHS & RHS margins as before for now. They are modified in
        '........the routine CalcOrigWCS.
        marginMod_Out(1) = margin_In(1)
        marginMod_Out(2) = margin_In(2)

    End Sub


    '....The following two functions could be consolidated into a single function with pointF return type in the future. 
    Protected Function CalcXVB(ByVal xWCS_Point As Single, ByVal yWCS_Point As Single) As Single
        '=======================================================================================
        ' This function calculates the X Coordinate in the PCS when the WCS coordinates 
        ' ....of a point are given. Dependent on "STD" or "ADJ" geometry.
        CalcXVB = mXVB_OrigWCS - mUnit.L_ConToUser(yWCS_Point) * mScalePCS
    End Function


    Protected Function CalcYVB(ByVal xWCS_Point As Single, ByVal yWCS_Point As Single) As Single
        '=======================================================================================
        ' This function calculates the X Coordinate in the PCS when the WCS coordinates 
        ' ....of a point are given. Dependent on "STD" or "ADJ" geometry.
        CalcYVB = mYVB_OrigWCS - mUnit.L_ConToUser(xWCS_Point) * mScalePCS
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
