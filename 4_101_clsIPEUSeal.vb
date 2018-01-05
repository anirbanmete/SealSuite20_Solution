
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsUSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29MAR16                                '
'                                                                              '
'===============================================================================
'
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Collections.Specialized
Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports clsLibrary11
Imports System.Drawing


<Serializable()> _
Public Class IPE_clsUSeal
    Inherits IPE_clsSeal
    Implements ICloneable
    Implements IDisposable


#Region "MEMBER VARIABLE DECLARATIONS:"

    '....The following shared member variable stores if the standard cross-section list
    '........has been retrieved from the database or not.
    Private Shared mbln_CrossSecList_Retrieved As Boolean = False

    ''............Moving this variable from clsSeal and making it "Private" resolves V80Beta1_DR ERROR # 8.
    '....Standard Cross-Section List from the Seal database.                        
    Private Shared mCrossSecList As New StringCollection

    '....Standard Cross-Section List from the Working database.
    Private Shared mCrossSecList_Envelope As New StringCollection

    '   GENERAL INFORMATION:
    '   ====================
    '....If a new cross-section is to be created in the Design Center ==> TRUE. 
    '........Otherwise, if a cross-section, which already exists, is to be retrieved
    '........from the database and may be adjusted to some extent ===> FALSE.
    '
    '....The value of "NewDesign" is assigned in the Property "CrossSection".
    '
    Private mNewDesign As Boolean                                       '(FUNDAMENTAL)


    '....The following member is included in case this curent object is modified 
    '........to create a new cross-section with a new no. Then, the following variable
    '........would preserve the original cross-section no.
    Private mCrossSecNoOrg As String                                    '(FUNDAMENTAL)

    '....New Cross-Section List from the new database.
    Private Shared mCrossSecNewList As New StringCollection

    '....The following shared member variable store if the new cross-section list
    '........has been retrieved from the database or not
    Private Shared mbln_CrossSecNewList_Retrieved As Boolean = False

    '....Database connection objects. 
    Private Shared sConn_USealDB As OleDbConnection
    Private Shared sConn_USealNewDB As OleDbConnection


    'Geometry Parameters:           
    '====================

    '....Not Used now.
    '   T Related:
    '   ----------
    '   ....The following indices correspond to the minimum and maximum allowable standard 
    '   ........thickness values. Not exposed thru' properties in this version. Kept for
    '   ........reference purpose only.
    Private mIndexTMin As Int16
    Private mIndexTMax As Int16

    '   ....Minimum and Maximum Limits of the adjusted T:  
    Private mTAdjMin As Single
    Private mTAdjMax As Single
    '-------------------------------------------------------------------------------------

    '   LLeg:
    '   -----
    Private mLLegStd As Single              '....Standard.            (RETRIEVED)
    Private mDLLeg As Single                '....Adjustment Increment.(FUNDAMENTAL)
    Private mLLeg As Single                 '....Adjusted             (FUNDAMENTAL)


    '   Radius Related:
    '   ----------------
    '   ....Index 0 is not used. 
    '   ........i = 1 : R1                          
    '   ........i = 2 : R2
    Private mRStd(2) As Single              '....Standard.            (RETRIEVED) 
    Private mDRad(2) As Single              '....Adjustment Increment.(FUNDAMENTAL)
    Private mR(2) As Single                 '....Adjusted             (FUNDAMENTAL)


    '   Theta Related:
    '   --------------------
    '   ....1 : Theta1
    '   ....2 : Theta2
    Private mThetaStd(2) As Single          '....Standard.              (RETRIEVED)
    Private mDTheta(2) As Single            '....Adjustment Increment.  (FUNDAMENTAL)   
    Private mTheta(2) As Single             '....Adjusted.              (DERIVED) 

    '....Derived Angles:
    Private mBeta(2) As Single              '                           (DERIVED)


    '....Not Used now.
    '   ....Adjustment Decrement/Increment Limits @ Std. Value. (Absolute Values):
    Private mDThetaAdjMin(2) As Single   '....Min. Limit (DERIVED from RETRIEVED data)  
    Private mDThetaAdjMax(2) As Single   '....Max. Limit (DERIVED from RETRIEVED data)      '(RETRIEVED) 
    '-----------------------------------------------------------------------------------

    '   FEA PARAMETERS:             
    '   =============== 
    '........No. of Contact Elements on either side of various contact regions.
    Private mNelConHeel As Integer    ' Heel Point.   

    'Graphics Parameters: (DERIVED)
    '--------------------
    Private mCen(3) As PointF  '....Center points: Seal.
    Private mKP(14) As PointF  '.....Key points: Seal. 

#End Region


#Region "CLASS PROPERTY ROUTINES:"
    '============================

#Region "READ ONLY PROPERTIES:"

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


    Public Shared ReadOnly Property CrossSecNewList() As StringCollection
        '================================================================
        Get
            Return mCrossSecNewList
        End Get

    End Property

    Public ReadOnly Property NewDesign() As Boolean
        '==========================================

        Get
            Return mNewDesign
        End Get

    End Property

    'Public Shared ReadOnly Property CrossSecList() As StringCollection
    '    '=============================================================
    '    Get
    '        Return mCrossSecList
    '    End Get

    'End Property


    'Public Shared ReadOnly Property CrossSecList_WorkingDB() As StringCollection
    '    '=======================================================================    
    '    Get
    '        Return mCrossSecList_WorkingDB
    '    End Get

    'End Property


    '   Theta (i), i = 1, 2.
    '   --------------------

    '   ....Standard.
    '
    Public ReadOnly Property ThetaStd(ByVal i As Integer) As Single
        '==========================================================
        Get
            Return mThetaStd(i)
        End Get

    End Property


    '   R (i), i = 1, 2.
    '   ----------------

    '   ....Standard.
    '
    Public ReadOnly Property RStd(ByVal i As Integer) As Single
        '======================================================
        Get
            Return mRStd(i)
        End Get

    End Property

    '   LLeg:
    '   -----

    '   ....Standard.
    '
    Public ReadOnly Property LLegStd() As Single
        '========================================
        Get
            Return mLLegStd
        End Get

    End Property


    '   ....Adjustment Parameters: Not used
    '   
    '   ........Decrement Limit (Absolute Value).
    Public ReadOnly Property DThetaAdjMin(ByVal i As Integer) As Single
        '==============================================================
        Get
            Return mDThetaAdjMin(i)
        End Get
    End Property


    '   ........Increment Limit.
    Public ReadOnly Property DThetaAdjMax(ByVal i As Integer) As Single
        '===============================================================
        Get
            Return mDThetaAdjMax(i)

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


    Public ReadOnly Property Beta(ByVal i As Integer) As Single
        '======================================================     
        Get
            Return mBeta(i)
        End Get

    End Property


#End Region

#Region "READ AND WRITE PROPERTIES:"

    Public Property CrossSecNoOrg() As String
        '====================================     
        Get
            Return mCrossSecNoOrg
        End Get

        Set(ByVal strData As String)
            If mNewDesign = True Then
                mCrossSecNoOrg = strData
            End If
        End Set

    End Property


    '....CrossSecNo.
    Public Overrides Property MCrossSecNo() As String
        '===========================================
        Get
            Return MyBase.MCrossSecNo()
        End Get

        Set(ByVal strData As String)
            '--------------------------
            MyBase.MCrossSecNo = strData

            If mCrossSecList.Contains(strData) Then
                mNewDesign = False
                mCrossSecNoOrg = MCrossSecNo

            Else
                mNewDesign = True

                If mbln_CrossSecNewList_Retrieved = False Then
                    Retrieve_CrossSections(mCrossSecNewList, "SealNewDB", "tblUSeal_Geom")
                    mbln_CrossSecNewList_Retrieved = True      '....Reset the FLAG.                
                End If

            End If

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
            MyBase.POrient = strData

            If MCrossSecNo <> "" Then

                Dim pstrGeomType As String = ""

                If mAdjusted = "N" Then
                    pstrGeomType = "STD"

                ElseIf mAdjusted = "Y" Then
                    pstrGeomType = "ADJ"
                End If

                '....Reinitialize some of the "DERIVED" member variable set.
                Calc_MemberVariables(pstrGeomType)
            End If

        End Set

    End Property


    '   T:
    '   --

    '   ....Standard.  Refer to clsSeal.

    '   ....Adjusted.
    '
    Public Overrides Property T() As Single
        '==================================
        Get
            Return MyBase.T
        End Get

        Set(ByVal val As Single)
            '-------------------
            If mNewDesign = True Then
                MyBase.T = val
                SetAdjusted()

            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '  ....LLeg Adjustment.

    Public Property DLLeg() As Single
        '===========================
        Get
            Return mDLLeg
        End Get


        Set(ByVal sngValue As Single)
            mDLLeg = sngValue
            'mLLeg = mLLegStd + mDLLeg   '....Update LLeg

            SetAdjusted()               '....Set the value of 'mAdjusted" = "Y" or "N".

        End Set

    End Property


    '   ....R (i) Adjustment.
    Public Property DRad(ByVal i As Integer) As Single
        '=================================================
        Get
            Return mDRad(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDRad(i) = sngData
            'mR(i) = mRStd(i) + mDRad(i)  '....Update R(i)

            SetAdjusted()                '....Set the value of 'mAdjusted" = "Y" or "N".

        End Set

    End Property


    '   ....Theta (i) Adjustment.
    '
    Public Property DTheta(ByVal i As Integer) As Single
        '===============================================
        Get
            Return mDTheta(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDTheta(i) = sngData
            'mTheta(i) = mThetaStd(i) + mDTheta(i)           '....Update Theta (i)

            SetAdjusted()               '....Set the value of 'mAdjusted" = "Y" or "N".

            '....Update some of the "DERIVED" member variable set.
            'Calc_MemberVariables("ADJ")   'PB 21MAY09. Moved inside SetAdjusted.

        End Set

    End Property

    '   ....Theta (i) Adjusted Value.
    '
    Public Property Theta(ByVal i As Integer) As Single
        '=======================================================
        Get
            Return mTheta(i)
        End Get

        Set(ByVal sngVal As Single)
            '----------------------

            If mNewDesign = True Then
                mTheta(i) = sngVal
                'mDTheta(i) = mTheta(i) - mThetaStd(i)

            ElseIf mNewDesign = False Then
                '....This parameter value is already retrived from the database and 
                '........can not be reset.
            End If

        End Set
    End Property
    'SG 21AUG09
    '   ....Adjusted.
    '
    Public Property R(ByVal i As Integer) As Single
        '=================================================
        Get
            Return mR(i)
        End Get
        Set(ByVal sngVal As Single)
            '----------------------

            If mNewDesign = True Then
                mR(i) = sngVal
                'mDRad(i) = mR(i) - mRStd(i)

            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set
    End Property

    '   ....Adjusted.
    '
    Public Property LLeg() As Single
        '===========================
        Get
            Return mLLeg
        End Get
        Set(ByVal sngVal As Single)
            '----------------------
            If mNewDesign = True Then
                mLLeg = sngVal
                'mDLLeg = mLLeg - mLLegStd

            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If


        End Set
    End Property


    '....NelConHeel.        
    Public Property NelConHeel() As Integer
        '==================================
        Get
            Return mNelConHeel
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mNelConHeel = sngData
        End Set
    End Property

#End Region


#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(ByVal strType_In As String, _
                   Optional ByVal strUnitSystem_In As String = "English", _
                   Optional ByVal strPOrient_In As String = "External")
        '==================================================================

        MyBase.New(strType_In, strUnitSystem_In, strPOrient_In)

        '....Initialization: The USeal being instantiated already exists in the 
        '........USeal database.    
        '........On the other hand, if it is to be designed as a new cross-section 
        '........in the "Design Center", the following parameter will be set to 
        '........TRUE later during program execution.
        mNewDesign = False

        If mbln_CrossSecList_Retrieved = False Then
            Retrieve_CrossSections(mCrossSecList, "SealDB", "tblUSeal_Geom")
            'Retrieve_CrossSections(mCrossSecList_WorkingDB, "WorkingDB", "tblUSealCandidate")
            Retrieve_CrossSections(mCrossSecList_Envelope, "SealDB", "tblUSeal_Envelope")
            mbln_CrossSecList_Retrieved = True      '....Reset the value.
        End If

    End Sub

#End Region


#Region "UTILITY ROUTINES:"


    Private Sub Initialize()
        '===================
        '....This routine is invoked when a new Cross-Sec No is assigned.

        '....Initialize Geometry Adjustment Parameters:
        mAdjusted = "N"

        '....Set the adjusted parameter values = standard values.        
        'T = mTStd
        'mLLeg = mLLegStd

        For i As Integer = 1 To 2
            DRad(i) = 0.0
            DTheta(i) = 0.0
        Next

        DLLeg = 0.0

        If mNewDesign = False Then
            '....Retrieve Standard USeal data from the database.
            RetrieveDBParams()

        ElseIf mNewDesign = True And mCrossSecNewList.Contains(MCrossSecNo) = True Then
            '....The cross-section has already been designed in the "Design Center" and 
            '........its geometry data saved in the new database.  

            RetrieveNewDBParams()
        End If


    End Sub


    Private Sub SetAdjusted()
        '====================
        '....This routine is called when any geometry parameter 
        '........e.g. Rad (i), LLeg, Theta (i) & t is adjusted.

        '........Check if any adjustment has been done.
        '
        If mT < gcEPS Or mLLeg < gcEPS Or _
           mR(1) < gcEPS Or mR(2) < gcEPS Or _
           mTheta(1) < gcEPS Or mTheta(2) < gcEPS Then

            '....The baseline values of all the adjusting parameters have not been 
            '........assigned yet.
            mAdjusted = "N"

        Else

            '....All the baseline values have been duly assigned and geometry 
            '........adjustement has been done.

            If Abs(mT - mTStd) > gcEPS Or _
               Abs(mDLLeg) > gcEPS Or _
               Abs(mDRad(1)) > gcEPS Or _
               Abs(mDRad(2)) > gcEPS Or _
               Abs(mDTheta(1)) > gcEPS Or _
               Abs(mDTheta(2)) > gcEPS Then

                mAdjusted = "Y"

            Else
                mAdjusted = "N"

            End If

        End If


        '....Reinitialize some of the "DERIVED" member variable set.
        '
        If mAdjusted = "N" Then
            Calc_MemberVariables("STD")

        ElseIf mAdjusted = "Y" Then
            Calc_MemberVariables("ADJ")
        End If

    End Sub


    Private Sub Calc_MemberVariables(ByVal strGeomType_In As String)
        '============================================================
        '   This routine calculates the following:
        '   1. Graphics Parameters.
        '   2. HFree & HFreeStd, and "ZClear", "DControl" & "H11Tol".
        '   3. Wid & WidStd.                                              

        '....This procedure is called within the following routines:
        '   
        '   1. RetrieveDBParams   (Std).
        '   2. POrient            (Std / Adj). 
        '   3. Draw               (Std / Adj).
        '   4. WriteFile_KP_T1Gen (Std / Adj).
        '   5. SetAdjusted        (Std / Adj).

        Dim pTheta1Draw As Single
        Dim pTheta2Draw As Single
        Dim pR1Draw As Single
        Dim pR2Draw As Single
        Dim pLLegDraw As Single
        Dim pTDraw As Single

        If strGeomType_In = "STD" Then

            pTheta1Draw = mThetaStd(1)
            pTheta2Draw = mThetaStd(2)
            pR1Draw = mRStd(1)
            pR2Draw = mRStd(2)
            pLLegDraw = mLLegStd
            pTDraw = mTStd

        ElseIf strGeomType_In = "ADJ" Then

            pTheta1Draw = mTheta(1)
            pTheta2Draw = mTheta(2)
            pR1Draw = mR(1)
            pR2Draw = mR(2)
            pLLegDraw = mLLeg
            pTDraw = mT
        End If


        Dim i As Int16
        For i = 1 To 2
            '
            '....Do the following set of calculation twice to avoid small error because of the 
            '........following reason:
            '....The WCS paramters depend on DControl.
            '....Hfree depends on WCS and in turn modifies ZClear and hence, changes 
            '........DControl slightly.
            '....Wid depends on WCS paramters & DControl.
            '........Hence, doing this loop twice will make DControl the same thru' 
            '........the second set of calculations. Solves Error # 22 (21JUL06)
            '

            Calc_GraphicsParams(pTheta1Draw, pTheta2Draw, pR1Draw, pR2Draw, pLLegDraw, pTDraw)
            Calc_HFree(pR2Draw, pTDraw)                         '....mHfree and ZClear, DControl & H11Tol.
            Calc_Wid() '....mWid, HLeg & HConv. 'AM 26MAY09

        Next


        If strGeomType_In = "STD" Then
            '....Save "STANDARD" geometry envelope dimensions.
            mHfreeStd = mHfree
            mWidStd = mWid

        End If

    End Sub

    'Private Sub Calc_GraphicsParams(ByVal Theta1Draw_In As Single, _
    '                                ByVal Theta2Draw_In As Single, _
    '                                ByVal R1Draw_In As Single, _
    '                                ByVal R2Draw_In As Single, _
    '                                ByVal LLegDraw_In As Single, _
    '                                ByVal TDraw_In As Single)
    '    '===========================================================================
    '    'This subroutine calculates various Graphics Parameters in CYS = CARTW (WCS),  
    '    '....which are needed to draw an U-Seal graphics on a selected device. 
    '    '....e.g. picture box or printer object.
    '    '
    '    'Refer to the Figure 1 of the Theoretical Manual for the 
    '    '....World Coordinate System'(WCS).
    '    '
    '    'Before plotting, the above graphics parameters in WCS will be converted to 
    '    '....the 'VB Coordinate System' (PCS) attached to the picture box. 
    '    '....This conversion will be done in a seperate routine.

    '    If POrient = "External" Then
    '        Calc_GraphicsParams_External(Theta1Draw_In, Theta2Draw_In,
    '                                     R1Draw_In, R2Draw_In, LLegDraw_In, TDraw_In)

    '    ElseIf POrient = "Internal" Then
    '        Calc_GraphicsParams_Internal(Theta1Draw_In, Theta2Draw_In,
    '                                     R1Draw_In, R2Draw_In, LLegDraw_In, TDraw_In)

    '    End If

    'End Sub

    Private Sub Calc_HFree(ByVal R2_In As Single, ByVal T_In As Single)
        '==============================================================

        mHfree = 2 * ((mCen(1).Y - mCen(2).Y) + R2_In) + T_In
        Update_ControlDiaParams()

    End Sub


    Private Sub Calc_Wid()
        '=================  
        'This sub calculates the radial width.
        '
        '   Routine called by:
        '       Calc_MemberVariables

        If POrient = "External" Then
            mWid = mKP(2).X - mKP(8).X

        ElseIf POrient = "Internal" Then
            mWid = mKP(8).X - mKP(2).X
        End If

    End Sub


    'Private Function CylToCart(ByVal Rad As Single, ByVal Theta As Single) As PointF
    '    '============================================================================   
    '    CylToCart.X = Rad * CosD(Theta)
    '    CylToCart.Y = Rad * SinD(Theta)

    'End Function

    Public Function CalcZClear(ByVal HFree_In As Single) As Single
        '==========================================================     
        'This function selects the necessary diametral clearance for a given free height.
        '
        '....Valid for: U-Seal only.
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
        '(Ref : Advanced Products Catalogue - Page C-15, U-Ring)
        '....The HfreeArray(UBound) value has been arbitrarily taken as 0.309     

        'Dim hFreeArray() As Object = {0.078, 0.109, 0.155, 0.216, 0.309}
        'Dim zClearArray() As Object = {0.003, 0.003, 0.005, 0.006, 0.008}

        Dim pHFree_Array As New ArrayList()
        With pHFree_Array
            .Add(0.078)
            .Add(0.109)
            .Add(0.155)
            .Add(0.216)
            .Add(0.309)
        End With

        Dim pZClear_Array As New ArrayList()
        With pZClear_Array
            .Add(0.003)
            .Add(0.003)
            .Add(0.005)
            .Add(0.006)
            .Add(0.008)
        End With

        Dim iLow As Integer = 0
        Dim iUp As Integer = pHFree_Array.Count - 1


        'Calculate ZClear.
        '-----------------
        Dim HFreeInEng As Single, ZClearEng As Single
        Dim i As Integer

        '....Convert mUnit.System ===> English Unit.
        HFreeInEng = HFree_In / mUnit.CFacConL

        If HFreeInEng <= pHFree_Array.Item(iLow) Then
            ZClearEng = pZClear_Array.Item(iLow)

        ElseIf HFreeInEng > pHFree_Array.Item(iUp) Then
            ZClearEng = pZClear_Array.Item(iUp)

        Else
            i = 1
            Do While HFreeInEng >= pHFree_Array.Item(i)
                i = i + 1
            Loop
            ZClearEng = pZClear_Array.Item(i)
        End If


        '....Convert English Unit ===> mUnit.System.
        CalcZClear = ZClearEng * mUnit.CFacConL

    End Function


#Region "ADJUSTED PARAMETERS:"

    Private Sub GetTAdjParams()
        '======================
        '....Not used now. May be in the future. PB 30APR09.

        'If mHfree < gcEPS Then Exit Sub

        ''---------------------------------------------------------------------------
        ''....mHfree has been duly assigned.

        ''....Calculate the minimum & maximum allowable T values for a given  
        ''........Hfree (adjusted value). 
        ''........Based on Manufacturing Thumb Rules (Refer to the SealIPE SDD Manual).
        ''
        'mTAdjMin = 0.042 * mHfree
        'mTAdjMax = 0.17 * mHfree

        ''....Get the Indices corresponding to the abobe Min. & Max. values,
        ''........as well as the TStd.
        ''
        'mIndexTMin = GetIndex_ArrayTStd(mTAdjMin, "Ceiling")
        'mIndexTMax = GetIndex_ArrayTStd(mTAdjMax, "Floor")

    End Sub


#End Region


#Region "CLONE METHOD"

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

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        '=========================================================
        'mDB.Dispose()
        Me.Finalize()

    End Sub

#End Region

End Class