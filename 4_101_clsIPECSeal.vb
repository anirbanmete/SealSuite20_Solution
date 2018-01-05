
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsCSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29MAR16                                '
'                                                                              '
'===============================================================================
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Collections.Specialized
Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports clsLibrary11
Imports System.Drawing

<Serializable()> _
Public Class IPE_clsCSeal
    Inherits IPE_clsSeal
    Implements ICloneable

#Region "USER-DEFINED STRUCTURE:"

    '....Plating.   
    <Serializable()> _
    Public Structure sPlating
        Public Exists As Boolean
        Public Code As String
        Public MatName As String
        Public Descrip As String
        Public ThickCode As String
        Public Thick As Single
        Public NLayer As Int16          '....An FEA parameter.
    End Structure

#End Region


#Region "NAMED CONTSTANT:"

    '....Design opening angle of the cross-section.
    Public Const mcTHETA_OPENING_DESIGN As Single = 100     '....Degree.

#End Region


#Region "MEMBER VARIABLE:"

    '....The following shared member variable store if the standard cross-section list
    '........has been retrieved from the database or not.
    '............Moving this variable from clsSeal and making it "Private" resolves DR ERROR # 61.
    Private Shared mbln_CrossSecList_Retrieved As Boolean = False  'PB 21SEP08.

    ''............Moving this variable from clsSeal and making it "Private" resolves V80Beta1_DR ERROR # 8.
    ''....Standard Cross-Section List from the Seal database.                        
    Private Shared mCrossSecList As New StringCollection

    '....Standard Cross-Section List from the Working database.
    Private Shared mCrossSecList_Envelope As New StringCollection

    '....Database connection object. 
    Private Shared sConn_CSealDB As OleDbConnection

    Private mPlating As sPlating                        '(FUNDAMENTAL)

    'Geometry Parameters:           
    '====================

    '   ThetaOpening Related:
    '   --------------------
    Private mThetaOpeningStd As Single          '....Standard.            (RETRIEVED)
    Private mDThetaOpening As Single            '....Adjustment Increment (FUNDAMENTAL)  
    Private mThetaOpening As Single             '....Adjusted             (DERIVED) 

    '   ....Adjustment Decrement/Increment Limits @ Std. Value. (Absolute Values):
    Private mDThetaOpeningAdjMin As Single   '....Min. Limit (DERIVED from RETRIEVED data)  
    Private mDThetaOpeningAdjMax As Single   '....Max. Limit (DERIVED from RETRIEVED data)


    '   Hfree Related:
    '   -------------
    '....Distinct Values - Standard Cross Section: Dynamic array
    Private mArrayHFreeStd() As Single                 '....(RETRIEVED)
    Private mDHfree As Single                          '....(FUNDAMENTAL)


    '   T Related:
    '   ----------
    '....The following indices correspond to the minimum and maximum allowable standard 
    '........thickness values. Not exposed thru' properties in this version. Kept for
    '........reference purpose only.
    Private mIndexTMin As Int16
    Private mIndexTMax As Int16

    '   ....Minimum and Maximum Limits of the adjusted T:  
    Private mTAdjMin As Single
    Private mTAdjMax As Single


    'Graphics Parameters: (DERIVED)
    '--------------------
    Private mCen As PointF         '....Center point.

    Private mToolNo As String       'AES 04APR16

#End Region


#Region "PROPERTY ROUTINES"


#Region "READ & WRITE PROPERTIES:"

    '....CrossSecNo.
    Public Overrides Property MCrossSecNo() As String
        '===========================================
        Get
            Return MyBase.MCrossSecNo()
        End Get

        Set(ByVal strData As String)
            '-----------------------
            MyBase.MCrossSecNo = strData
            Initialize()
        End Set

    End Property


    '....POrient.
    Public Overrides Property POrient() As String
        '========================================
        Get
            Return MyBase.POrient
        End Get

        Set(ByVal strData As String)
            '------------------------
            MyBase.POrient = strData
        End Set

    End Property


    '....DHfree: 
    Public Property DHfree() As Single
        '==============================
        Get
            Return mDHfree
        End Get


        Set(ByVal sngValue As Single)
            '------------------------
            mDHfree = sngValue
            mHfree = mHfreeStd + mDHfree         '....Update free height.

            GetTAdjParams()     '....Calculates "mTAdjMin" & "mTAdjMax".
            SetAdjusted()       '....Sets the value of the "mAdjusted" = "Y" or "N".

            '....The following method updates "ZClear" (which depends on Hfree), 
            '........"DControl" & "H11Tol".
            Update_ControlDiaParams()

            '....Update 'mWid' which is a "DERIVED" member variable, 
            '........ and depends on 'Hfree' & 'mThetaOpening'.
            Update_Wid()

        End Set

    End Property


    '....DThetaOpening:            
    Public Property DThetaOpening() As Single
        '====================================
        Get
            Return mDThetaOpening
        End Get

        Set(ByVal sngValue As Single)
            '------------------------
            mDThetaOpening = sngValue
            mThetaOpening = mThetaOpeningStd + mDThetaOpening   '...Update Theta Opening.
            SetAdjusted()       '....Sets the value of the "mAdjusted" = "Y" or "N".

            '....Update 'mWid' which is a "DERIVED" member variable, 
            '........and depends on 'Hfree' & 'mThetaOpening'.
            Update_Wid()

        End Set

    End Property


    '....T (Adjusted value, if any).
    Public Overrides Property T() As Single
        '==================================
        Get
            Return MyBase.T
        End Get

        Set(ByVal sngVal As Single)
            '----------------------
            MyBase.T = sngVal
            SetAdjusted()       '....Sets the value of the "mAdjusted" = "Y" or "N".
        End Set

    End Property

#End Region


#Region "READ-ONLY PROPERTIES:"


    Public Shared ReadOnly Property CrossSecList() As StringCollection
        '=============================================================
        Get
            Return mCrossSecList
        End Get

    End Property


    Public Shared ReadOnly Property CrossSecList_Envelope() As StringCollection
        '=======================================================================    
        Get
            Return mCrossSecList_Envelope
        End Get

    End Property


    '   "ThetaOpening" Related:
    '   -----------------------
    '
    '   ....ThetaOpeningStd.                 
    Public ReadOnly Property ThetaOpeningStd() As Single
        '================================================
        Get
            Return mThetaOpeningStd
        End Get
    End Property


    '   ....Adjusted Value.
    Public ReadOnly Property ThetaOpening() As Single
        '============================================
        Get
            Return mThetaOpening
        End Get
    End Property


    '   ....Adjustment Parameters:
    '   
    '   ........Decrement Limit (Absolute Value).
    Public ReadOnly Property DThetaOpeningAdjMin() As Single
        '==================================================
        Get
            Return mDThetaOpeningAdjMin
        End Get
    End Property

    '   ........Increment Limit.
    Public ReadOnly Property DThetaOpeningAdjMax() As Single
        '===================================================
        Get
            Return mDThetaOpeningAdjMax
        End Get
    End Property


    '   ....Adjustment Limits:
    '   
    '   ........Minimum Limit.
    Public ReadOnly Property TAdjMin() As Single
        '========================================
        Get
            Return mTAdjMin
        End Get
    End Property


    '   ........Maximum Limit.
    Public ReadOnly Property TAdjMax() As Single
        '=======================================
        Get
            Return mTAdjMax
        End Get
    End Property

    '....Plating Data (English) 
    Public ReadOnly Property Plating() As sPlating
        '=========================================
        Get
            Return mPlating
        End Get
    End Property


    '....Plating Name (if any). 
    '........C-Seal: Has plating option.
    '........E-Seal and U-Seal: No  plating option.
    '
    Public ReadOnly Property PlatingName() As String
        '============================================
        Get
            Return mPlating.MatName
        End Get
    End Property


    Public ReadOnly Property ToolNo() As String
        '======================================     'AES 04APR16
        Get
            Return mToolNo
        End Get

    End Property


    '....The following member variables are not exposed in this version,
    '........but may be exposed in a future version.  29OCT06.
    'Public ReadOnly Property IndexTMin() As Int16      
    '    '=========================================
    '    Get
    '        Return mIndexTMin
    '    End Get
    'End Property


    'Public ReadOnly Property IndexTMax() As Int16
    '    '========================================
    '    Get
    '        Return mIndexTMax
    '    End Get
    'End Property

#End Region


#Region "WRITE-ONLY PROPERTIES"

    'Plating Data Set (English): 
    '--------------------------
    '
    Public WriteOnly Property PlatingExists() As Boolean
        '===============================================
        Set(ByVal Value As Boolean)
            mPlating.Exists = Value
            Set_DefLoadStepParams(mPlating.Exists)
        End Set
    End Property


    Public WriteOnly Property PlatingCode() As String
        '=============================================
        Set(ByVal Value As String)
            mPlating.Code = Value
            GetPlatingMatName(mPlating.Code)
        End Set

    End Property


    Public WriteOnly Property PlatingDescrip() As String
        '===============================================
        Set(ByVal Value As String)
            mPlating.Descrip = Value
        End Set
    End Property


    Public WriteOnly Property PlatingThickCode() As String
        '==============================================
        Set(ByVal Value As String)
            mPlating.ThickCode = Value
        End Set
    End Property


    Public WriteOnly Property PlatingThick() As Single
        '==============================================
        Set(ByVal Value As Single)
            mPlating.Thick = Value
        End Set
    End Property


    Public WriteOnly Property PlatingNLayer() As Int16
        '==============================================
        Set(ByVal Value As Int16)
            mPlating.NLayer = Value
        End Set
    End Property

#End Region

#End Region

#Region "CONTRUCTOR:"

    Public Sub New(ByVal strType_In As String, _
                   Optional ByVal strUnitSystem_In As String = "English", _
                   Optional ByVal strPOrient_In As String = "External")
        '==================================================================

        MyBase.New(strType_In, strUnitSystem_In, strPOrient_In)

        '....Retrieve the distinct "HfreeStd" values and populate  
        '........the Array "mArrayHFreeStd".
        RetrieveHfreeStdValues()

        If mbln_CrossSecList_Retrieved = False Then
            Retrieve_CrossSections(mCrossSecList, "SealDB", "tblCSeal_Geom")
            Retrieve_CrossSections(mCrossSecList_Envelope, "SealDB", "tblCSeal_Envelope")
            mbln_CrossSecList_Retrieved = True      '....Reset the value.
        End If

        PlatingExists = False
    End Sub

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub Initialize()
        '===================
        'This routine is invoked when a new CrossSec No. is assigned. 

        '....The following assignment is to made before "RetrieveDBParams" call,
        '........as the following member variable is used within the above routine in 
        '........"RetrieveManfParams" call.
        '
        mThetaOpeningStd = mcTHETA_OPENING_DESIGN

        '....Retrieve Standard CSeal data from the database.
        RetrieveDBParams()
        'mThetaOpeningStd = mcTHETA_OPENING_DESIGN

        '----------------------------------------------------------------------------
        '....Initialize adjusted geometry parameters: 
        '........(Standard values will be used as there is no adjustment yet)
        T = mTStd       'AM 19NOV09
        DHfree = 0.0
        DThetaOpening = 0.0
        'T = mTStd

    End Sub


    Public Sub GetPlatingMatName(ByVal platingCode_in As String)
        '=======================================================

        Select Case platingCode_in

            Case "AP", "SA", "SP", "SS"
                mPlating.MatName = "Silver"

            Case "GP", "SG"
                mPlating.MatName = "Gold"

            Case "CP", "SC"
                mPlating.MatName = "Copper"

            Case "NP", "SN"
                mPlating.MatName = "Nickel"

            Case "IP", "SI"
                mPlating.MatName = "Indium"

            Case "LP", "SL"
                mPlating.MatName = "Lead"

            Case "TC"
                mPlating.MatName = "Teflon"

        End Select

    End Sub

    Private Sub Update_Wid()
        '==================
        'This sub updates the radial width.

        If mHfree < gcEPS Or mThetaOpening < gcEPS Then
            mWid = 0.0
            mWidStd = 0.0
            Exit Sub

        Else
            '----------------------------------------------------------------------
            '....mHfree & mThetaOpening have been duly assigned so that mWid can be  
            '........calculated.

            Dim pRout As Single     '....Outer radius
            pRout = 0.5 * mHfree

            mWid = pRout * (1 + CosD(0.5 * mThetaOpening))

            If mAdjusted = "N" Then
                '....Save "STANDARD" geometry width. 
                mWidStd = mWid
            End If

        End If

    End Sub


    Public Function CalcZClear(ByVal HFree_In As Single) As Single
        '==========================================================
        'This function selects the necessary diametral clearance for a given free height.
        '
        '....Valid for: C-Seal only.
        '
        'STAND-ALONE FUNCTION:
        '---------------------
        '....Used in other modules (e.g. clsSealSelect), possibly without being associated
        '........with any particular seal design.

        '
        ' 'Unit Aware' Function.
        ' ----------------------

        'The following array table is in English unit e.g. in.
        '-----------------------------------------------------
        '(Ref : Advanced Products Catalogue - Page C-5, C-Ring)
        '....The HfreeArray(UBound) value has been arbitrarily taken as 0.75. 

        Dim hFreeArray() As Object = {0.039, 0.055, 0.078, 0.11, 0.141, 0.172, 0.219, _
                                      0.313, 0.438, 0.75}
        Dim zClearArray() As Object = {0.003, 0.005, 0.006, 0.008, 0.012, 0.016, 0.018, _
                                       0.02, 0.03, 0.04}

        '....Lower & Upper Bounds of the arrays
        Dim iLow As Integer = LBound(hFreeArray)
        Dim iUp As Integer = UBound(hFreeArray)

        'Calculate ZClear.
        '-----------------
        Dim HFreeInEng As Single, ZClearEng As Single
        Dim i As Integer

        '....Convert mUnit.System ===> English Unit.
        HFreeInEng = HFree_In / mUnit.CFacConL

        '....Select the recommended Diametral Clearance (Z).
        If HFreeInEng <= hFreeArray(iLow) Then
            ZClearEng = zClearArray(iLow)

        ElseIf HFreeInEng > hFreeArray(iUp) Then
            ZClearEng = zClearArray(iUp)

        Else
            i = 1
            Do While HFreeInEng >= hFreeArray(i)
                i = i + 1
            Loop
            ZClearEng = zClearArray(i)
        End If

        '....Convert English Unit ===> mUnit.System.
        CalcZClear = ZClearEng * mUnit.CFacConL

    End Function

#Region "ADJUSTED PARAMETER"

    Private Sub SetAdjusted()
        '====================

        If mHfree < gcEPS Or mThetaOpening < gcEPS Or mT < gcEPS Then
            '....The baseline values of all the adjusting parameters have not been 
            '........assigned yet.

            mAdjusted = "N"
            Exit Sub


        Else
            '....mHfree, mThetaOpening and mT all have been duly assigned.
            '........Check any adjustment has been done.

            If Abs(mDHfree) > gcEPS Or _
               Abs(mDThetaOpening) > gcEPS Or _
               Abs(mT - mTStd) > gcEPS Then

                mAdjusted = "Y"

            Else
                mAdjusted = "N"
            End If

        End If

    End Sub


    Private Sub GetHFreeAdjParams()
        '==========================
        '....Calculate the allowable Min. and Max. range of HFree for 
        '........the given Cross Section.    

        Dim j As Int16, iLoc As Int16

        '....Locate the index of the "mHfreeStd" of the given Cross Section on the Array.
        '.......Disregard the first index (0) & the last one e.g. UBound, which  
        '.......are the artificially extrapolated points.
        '
        For j = 1 To UBound(mArrayHFreeStd) - 1
            If Abs(mArrayHFreeStd(j) - mHfreeStd) <= gcEPS Then
                iLoc = j
                Exit For
            End If
        Next

        mDHfreeAdjMin = (mArrayHFreeStd(iLoc) - mArrayHFreeStd(iLoc - 1)) * 0.5
        mDHfreeAdjMax = (mArrayHFreeStd(iLoc + 1) - mArrayHFreeStd(iLoc)) * 0.5

    End Sub


    Private Sub GetTAdjParams()
        '======================

        If mHfree < gcEPS Then Exit Sub

        '---------------------------------------------------------------------------
        '....mHfree has been duly assigned.

        '....Calculate the minimum & maximum allowable T values for a given  
        '........Hfree (adjusted value). 
        '........Based on Manufacturing Thumb Rules (Refer to the SealIPE SDD Manual).
        '
        mTAdjMin = 0.042 * mHfree
        mTAdjMax = 0.17 * mHfree

        '....Get the Indices corresponding to the abobe Min. & Max. values,
        '........as well as the TStd.
        '
        mIndexTMin = GetIndex_ArrayTStd(mTAdjMin, "Ceiling")
        mIndexTMax = GetIndex_ArrayTStd(mTAdjMax, "Floor")

    End Sub

#End Region

#End Region


#Region "CLONE METHOD"

    Public Function Clone() As Object Implements ICloneable.Clone
        '================================================================
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