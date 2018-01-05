
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsESeal                               '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29MAR16                                '
'                                                                              '
'===============================================================================

Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Collections.Specialized
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports clsLibrary11
Imports System.Windows.Forms
Imports System.Linq


<Serializable()> _
Public Class IPE_clsESeal
    Inherits IPE_clsSeal
    Implements ICloneable
    Implements IDisposable


#Region "MEMBER DECLARATIONS:"

#Region "NAMED  CONSTANT:"

    '....The radius of a straight segment, which is the following arbitrary large value,
    '........being used as a convention.
    Private Const mcSTRAIGHT_SEC_RADIUS As Single = 999.0  '....Irrespective of Unit Sys.

#End Region

    '....The following shared member variable store if the standard cross-section list
    '........has been retrieved from the database or not. 
    '............Moving this variable from clsSeal and making it "Private" resolves DR ERROR # 61.
    Private Shared mbln_CrossSecList_Retrieved As Boolean = False  'PB 21SEP08.

    ''............Moving this variable from clsSeal and making it "Private" resolves V80Beta1_DR ERROR # 8.
    ''....Standard Cross-Section List from the Seal database.                      
    Private Shared mCrossSecList As New StringCollection

    ''....Standard Cross-Section List from the Working database.
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

    '....-1 : Last Stage, -2 : (Last Stage-1) etc.
    'Private mStageNo As String                                          '(FUNDAMENTAL)

    '....When Template = "1Gen" and the RadM (2) & (4) are straight, it will be 
    '.......internally set to be Template "1GenS" (Special version of "1Gen"). 
    Private mTemplateNo As String  ' = "1", "2" or "1Gen" ("1GenS") '(RETRIEVED)


    '....Database connection objects.
    Private Shared sConn_ESealDB As OleDbConnection
    Private Shared sConn_ESealNewDB As OleDbConnection


    '   GEOMETRY PARAMETERS:
    '   ====================
    Private mNConv As Integer                                           '(RETRIEVED)


    '....In the following arrays, Index = 0 not used. Index = i refers to the Segment no.
    '
    '   End Convolution:
    '   ----------------
    Private mRadE(3) As Single                                          '(RETRIEVED)
    Private mThetaE(3) As Single                                        '(RETRIEVED)
    Private mHLeg As Single                                             '(DERIVED)


    '   For Template = 2 only.
    '
    '....The following length will remain constant thru' any geometry adjustment.
    Private mLFlatE As Single                                           '(RETRIEVED)

    '   Mid Convolutions:
    '   ----------------
    Private mRadM(5) As Single                                          '(RETRIEVED)
    Private mThetaM(5) As Single                                        '(RETRIEVED)


    '....The following parameters are relevant when the mid-segment of a mid-convolution
    '........is straight. Template No = "1", "2" and "1GenS".
    '
    '       Both parameters Invariant thru' geometry adjustment.
    '
    '   COMMENT: The earlier parameter "mLFlatM" of the Template No = "1" & "2" is now 
    '            replaced with "mLFlatM_End" & "mLFlatM_Mid" parameters to make the
    '            above templates' parametric definition in line with "1Gen". 
    '            This change will facilicate integration of "1" with "1Gen" and 
    '            subsequent elimination of "1". 
    '
    'Private mLFlatM As Single          '08APR08
    '                           
    Private mLFlatM_End As Single                                       '(DERIVED)    
    Private mLFlatM_Mid As Single                                       '(DERIVED)
    '                                    
    '------------------------------------------------------------------------------
    '   HConv:
    '   ------
    '....Template = "1", "2" or "1GenS": Independent/dependent parameter. 
    '....Template = "1Gen"             : Dependent (or Ref.) parameter. 
    '
    '...."HConv" pertains to the original geometry and will change thru' thetaM1 
    '........adjustment.
    Private mHConv As Single                                 '(RETRIEVED or DERIVED)
    '-------------------------------------------------------------------------------

    '....Arc length of the cross-section profile of the half model in 
    '........the meridional direction.
    'Private mLenArcHalfModel As Single      'PB 24MAR08. Moved to clsSeal.

    '   Geometry Adjustment/Modification Parameters:
    '   --------------------------------------------
    Private mDThetaE1 As Double                                         '(FUNDAMENTAL)
    Private mDThetaM1 As Double                                         '(FUNDAMENTAL)
    Private mDThetaM3 As Double  '....Same as mDThetaM1.                '(DERIVED)
    Private mDThetaM5 As Single  '....Same as mDThetaM1.                '(DERIVED)    

    '   FEA PARAMETERS:  
    '   =============== 
    '........No. of Contact Elements on either side of various contact regions.
    Private mNelConMid As Integer     ' Mid     Point.                  '(RETRIEVED)
    Private mNelConHeel As Integer    ' Heel    Point.                  '(RETRIEVED)

    '........Element Density Weightings:
    Private mWtE(3) As Single        ' End 1/2 Convolution              '(RETRIEVED)
    Private mWtM(5) As Single        ' Mid     Convolutions             '(RETRIEVED)


    '   GRAPHICS PARAMETERS:    (DERIVED)
    '   ====================

    '   END Convolution:
    '   ---------------
    '....Cordinates of the centers of the circular Segments:
    Private mCenE(3) As PointF

    '   MID Convolution:  
    '   ----------------
    '....Cordinates of the centers of the circular Segments:
    '........Segment 1 not used.
    Private mCenM(5) As PointF

    Private mToolNo As String       'AES 04APR16

    '*******************************************************************************
    '*                    MEMBER VARIABLE DECLARATIONS  - END                      *
    '*******************************************************************************

#End Region


#Region "PROPERTY ROUTINES:"

#Region "READ ONLY PROPERTIES:"

    Public Shared ReadOnly Property STRAIGHT_SEC_RADIUS() As Single
        '==========================================================
        Get
            Return mcSTRAIGHT_SEC_RADIUS
        End Get

    End Property


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


    '....TemplateNo
    Public ReadOnly Property TemplateNo() As String
        '===========================================
        Get
            Return mTemplateNo
        End Get
    End Property


    Public ReadOnly Property NewDesign() As Boolean
        '==========================================

        Get
            Return mNewDesign
        End Get

    End Property


    '....LFlatm (TemplateNo <> "1Gen")      'PB 20SEP08. Not needed any more. 
    'Public ReadOnly Property LFlatM() As Single
    '    '======================================
    '    Get
    '        'Return mLFlatM
    '    End Get
    'End Property


    '....LFlatE (mTemplateNo = "2")
    Public ReadOnly Property LFlatE() As Single
        '======================================
        Get
            Return mLFlatE
        End Get
    End Property


    Public ReadOnly Property HLeg() As Single
        '====================================
        Get
            Return mHLeg
        End Get

    End Property


    Public ReadOnly Property ToolNo() As String
        '======================================     'AES 04APR16
        Get
            Return mToolNo
        End Get

    End Property

#End Region


#Region "READ AND WRITE PROPERTIES:"

    '....CrossSecNo.
    Public Overrides Property MCrossSecNo() As String
        '===========================================
        Get
            Return MyBase.MCrossSecNo()
        End Get

        Set(ByVal strData As String)
            MyBase.MCrossSecNo = strData
            'mStageNo = "-1"         '....Last Stage (default value).

            If mCrossSecList.Contains(strData) Then
                mNewDesign = False
                mCrossSecNoOrg = MCrossSecNo

            Else
                mNewDesign = True

                If mbln_CrossSecNewList_Retrieved = False Then
                    Retrieve_CrossSections(mCrossSecNewList, "SealNewDB", "tblESeal_Geom")
                    mbln_CrossSecNewList_Retrieved = True      '....Reset the FLAG.                
                End If

            End If

            Initialize()
        End Set

    End Property


    '....Original Cross-section no. from which the new cross-section, if so, 
    '........is designed. 
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


    ''....StageNo.
    'Public Property StageNo() As String
    '    '==============================
    '    Get
    '        Return mStageNo
    '    End Get

    '    Set(ByVal strData As String)
    '        '-----------------------
    '        '....Data Validation:
    '        If strData = "" Then
    '            MsgBox("Stage No " & strData & " not accepted.")
    '            Exit Property
    '        Else
    '            mStageNo = strData

    '        End If

    '        Initialize()
    '    End Set

    'End Property


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


    '....NConv
    Public Property NConv() As Integer
        '=============================
        Get
            Return mNConv
        End Get

        Set(ByVal val As Integer)
            '--------------------

            If mNewDesign = True Then
                mNConv = val

            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '....T:
    Public Overrides Property T() As Single
        '==================================
        Get
            Return MyBase.T
        End Get

        Set(ByVal val As Single)
            '-------------------

            If mNewDesign = True Then
                MyBase.T = val

                UpdateLFlatM_End()
                UpdateLFlatM_Mid()


            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '....HConv.
    Public Property HConv() As Single
        '============================
        Get
            Return mHConv
        End Get

        Set(ByVal val As Single)
            '-------------------
            '....Only assigned when TemplateNo = "1GenS".
            '....For TemplateNo = "1Gen", it is a derived value.
            If mNewDesign = True Then
                mHConv = val

                'UpdateLFlatM()
                UpdateLFlatM_End()
                UpdateLFlatM_Mid()


            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '....RadE.
    Public Property RadE(ByVal i As Integer) As Single
        '=============================================
        Get
            Return mRadE(i)
        End Get

        Set(ByVal val As Single)
            '-------------------

            If mNewDesign = True Then
                mRadE(i) = val

                If i = 1 Then
                    mRadM(1) = mRadE(1)
                    If mTemplateNo = "1GenS" Then UpdateLFlatM_End()
                End If

            ElseIf mNewDesign = False Then
                '....This parameter value is already retrived from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '....RadM.
    Public Property RadM(ByVal i As Integer) As Single
        '=============================================
        Get
            Return mRadM(i)
        End Get

        Set(ByVal val As Single)
            '-------------------

            If mNewDesign = True Then

                If i = 1 Then
                    Dim pTitle As String = "Warning Message: RadM assignment."
                    Dim pPrompt As String
                    pPrompt = "RadM (1) can't be directly assigned as RadM(1) = RadE(1)."
                    MessageBox.Show(pPrompt, pTitle, MessageBoxButtons.OK, _
                                                     MessageBoxIcon.Warning)
                    Exit Property
                End If

                If i > 1 Then
                    mRadM(i) = val
                    '....Invoke Design Rule: "Straight Mid. Sec"
                    DesignRule_RadM_Straight()

                    If i = 3 And mTemplateNo = "1GenS" Then UpdateLFlatM_Mid() _
                                                         : UpdateLFlatM_End()

                    If i = 5 And mTemplateNo = "1GenS" Then UpdateLFlatM_Mid()
                End If


            ElseIf mNewDesign = False Then
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '....ThetaE
    Public Property ThetaE(ByVal i As Integer) As Single
        '===============================================
        Get
            Return mThetaE(i)
        End Get


        Set(ByVal val As Single)
            '-------------------

            Dim pAttributes As Integer
            Dim pTitle As String
            Dim pstrPrompt As String = ""

            pAttributes = vbExclamation + vbOKOnly
            pTitle = "Warning Message: ThetaE assignment."


            If mNewDesign = True Then
                '--------------------

                mThetaE(i) = val

            ElseIf mNewDesign = False Then
                '-------------------------
                '....This parameter value is already retrieved from the database and 
                '........can not be reset.
            End If

        End Set

    End Property


    '....ThetaM
    Public Property ThetaM(ByVal i As Integer) As Single
        '===============================================      

        Get
            If i = 3 Then       '....Dependent (ref.) variable.
                Return DesignRule_ThetaM(3)

            ElseIf i = 5 Then   '....Dependent (ref.) variable.
                Return DesignRule_ThetaM(5)

            Else
                Return mThetaM(i)
            End If

        End Get


        Set(ByVal val As Single)
            '-------------------

            If i <> 3 And i <> 5 Then

                If mNewDesign = True Then
                    mThetaM(i) = val

                    '....Recalculate the following dependent variables.
                    mThetaM(3) = DesignRule_ThetaM(3)
                    mThetaM(5) = DesignRule_ThetaM(5)

                ElseIf mNewDesign = False Then
                    '....This parameter value is already retrieved from the database and 
                    '........can not be reset.
                End If


            Else
                '....i = 3 & 5 are dependent variables and can not be set.
                '........This condition is valid for all the templates.
                Dim pTitle As String = "Warning Message: ThetaM assignment."
                Dim pPrompt As String
                pPrompt = "ThetaM (" & i & ") can't be directly assigned, " & _
                          "as it is a dependent variable."

                MessageBox.Show(pPrompt, pTitle, MessageBoxButtons.OK, _
                                                    MessageBoxIcon.Warning)
                Exit Property
            End If

        End Set

    End Property


    '....ThetaE1 Adjustment.
    Public Property DThetaE1() As Single
        '===============================
        Get
            Return mDThetaE1
        End Get


        Set(ByVal sngData As Single)
            If Abs(sngData) > 60 Then
                MsgBox("DThetaE1 value > 60 degree not accepted.")
                Exit Property
            End If

            mDThetaE1 = sngData

            If Abs(mDThetaE1) > gcEPS Or Abs(mDThetaM1) > gcEPS Then
                mAdjusted = "Y"
            Else
                mAdjusted = "N"
            End If

            SetAdjusted()

            '....Update some of the "DERIVED" member variable set.
            Calc_MemberVariables("ADJ")
        End Set

    End Property


    '....ThetaM1 Adjustment.
    Public Property DThetaM1() As Single
        '===============================
        Get
            Return mDThetaM1
        End Get


        Set(ByVal sngData As Single)
            '-----------------------
            If Abs(sngData) > 60 Then
                MsgBox("DThetaM1 value > 60 degree not accepted.")
                Exit Property
            End If

            mDThetaM1 = sngData
            mDThetaM3 = mDThetaM1       '....Assumption: Change in ThetaM(3) is same as
            '                           '..................that in ThetaM(1).

            If mTemplateNo = "1Gen" Or mTemplateNo = "1GenS" Then
                mDThetaM5 = mDThetaM1
            End If


            If Abs(mDThetaE1) > gcEPS Or Abs(mDThetaM1) > gcEPS Then
                mAdjusted = "Y"
            Else
                mAdjusted = "N"
            End If

            SetAdjusted()

            '....Update some of the "DERIVED" member variable set.
            Calc_MemberVariables("ADJ")
        End Set

    End Property


    'FEA Parameters:
    '---------------
    '..No. of Contact Elements on either side of various contact regions.
    '
    '....NelConMid.
    Public Property NelConMid() As Integer
        '=================================
        Get
            Return mNelConMid
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mNelConMid = sngData
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


    '....Element Density Weightings:
    '
    '........End 1/2 Convolution        
    Public Property WtE(ByVal i As Integer) As Single
        '============================================
        Get
            Return mWtE(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mWtE(i) = sngData
        End Set
    End Property


    '....Mid 1/2 Convolution     
    Public Property WtM(ByVal i As Integer) As Single
        '============================================
        Get
            Return mWtM(i)
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mWtM(i) = sngData
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

        '....Initialization: The ESeal being instantiated already exists in the 
        '........ESeal database.    
        '........On the other hand, if it is to be designed as a new cross-section 
        '........in the "Design Center", the following parameter will be set to 
        '........TRUE later during program execution.
        mNewDesign = False

        If mbln_CrossSecList_Retrieved = False Then
            Retrieve_CrossSections(mCrossSecList, "SealDB", "tblESeal_Geom")
            Retrieve_CrossSections(mCrossSecList_Envelope, "SealDB", "tblESeal_Envelope")
            mbln_CrossSecList_Retrieved = True      '....Reset the value.
        End If

    End Sub

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub Initialize()
        '===================
        'This routine is used when a new CrossSec No. or a Stage No. is assigned. 

        '....Initialize Geometry Adjustment Parameters:
        mAdjusted = "N"

        mDThetaE1 = 0.0#
        mDThetaM1 = 0.0#

        '....Derived:
        mDThetaM3 = mDThetaM1
        mDThetaM5 = mDThetaM1            '....TemplateNo = "1Gen" or "1GenS"    

        If mNewDesign = False Then
            '....Retrieve standard ESeal data from the main database.
            RetrieveDBParams()

        ElseIf mNewDesign = True And mCrossSecNewList.Contains(MCrossSecNo) = True Then

            '....The cross-section has already been designed in the "Design Center" and 
            '........its geometry data saved in the new database.            
            RetrieveNewDBParams()
        End If

    End Sub


    Private Sub SetAdjusted()
        '====================

        If mThetaE(1) < gcEPS Or mThetaM(1) < gcEPS Then
            '....The baseline values of all the adjusting parameters have not been 
            '........assigned yet.
            mAdjusted = "N"
            Exit Sub


        Else
            '....mThetaE(1) and mThetaM(1) all have been duly assigned.
            '........Check any adjustment has been done.

            If Abs(mDThetaE1) > gcEPS Or Abs(mDThetaM1) > gcEPS Then
                mAdjusted = "Y"
            Else
                mAdjusted = "N"
            End If

        End If

    End Sub


#Region "DESIGN RULES:"

    '....For Template = "1Gen".
    Private Function DesignRule_ThetaM(ByVal i As Int16) As Single
        '==========================================================
        '....If i = 3 or 5, mThetaM (i) is a dependent (ref.) variable.

        If i = 3 Then

            If mTemplateNo <> "1Gen" Then
                '....TemplateNo = 1, 2 & 1GenS.
                Return mThetaM(1)

            ElseIf mTemplateNo = "1Gen" Then
                '....TemplateNo = "1Gen". 
                Return mThetaM(1) - mThetaM(2)
            End If


        ElseIf i = 5 Then

            If mTemplateNo <> "1Gen" Then
                '....TemplateNo = 1, 2 & 1GenS.
                Return mThetaM(1)

            ElseIf mTemplateNo = "1Gen" Then
                '....TemplateNo = "1Gen".
                Return mThetaM(3) + mThetaM(4)
            End If

        Else
            Return 0
        End If

    End Function


    '....For Template = "1Gen". 
    Private Sub DesignRule_RadM_Straight()
        '=================================

        If Abs(mRadM(2) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Or _
           Abs(mRadM(4) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then

            '....If one of the two radii e.g. RadM(2) or RadM(4) is straight, 
            '........the other one has to be straight too. DESIGN RULE.
            '
            mRadM(2) = mcSTRAIGHT_SEC_RADIUS
            mThetaM(2) = 0.0

            mRadM(4) = mcSTRAIGHT_SEC_RADIUS
            mThetaM(4) = 0.0

            If mTemplateNo = "1Gen" Then mTemplateNo = "1GenS"

        End If

    End Sub


#End Region


#Region "MEMBER VARIABLE CALCULATION:"

    'Private Sub UpdateLFlatM()
    '    '===================
    '    'For Template = "1", "2" or "1GenS".
    '    '
    '    'This function calculates 'LFlatM', the length of the flat segment of the
    '    '....Mid-Convolution. This length will remain constant thruought the geometry
    '    '....adjustment process but may be changed during new cross-section creation 
    '    '........process in the "Design Center".

    '    '....Intermediate variables.
    '    Dim dX1 As Double, dX3 As Double
    '    dX1 = mRadM(1) * (1 - CosD(mThetaM(1)))
    '    dX3 = mRadM(3) * (1 - CosD(mThetaM(3)))

    '    '....Pertains to a given original "mHConv".
    '    Dim pHFlatM As Double
    '    pHFlatM = mHConv - T - (dX1 + dX3)

    '    mLFlatM = pHFlatM / CosD((90 - mThetaM(3)))   '....Invariant parameter.

    'End Sub


    Private Sub UpdateLFlatM_End()
        '=========================      
        'For Template = "1", "2" or "1GenS".
        '
        'This function calculates 'LFlatM', the length of the flat segment of a
        '....Mid_End 1/2-Convolution. This length will remain constant thruought the 
        '....geometry adjustment process but may be changed during new cross-section 
        '.........creation process in the "Design Center".

        '....Intermediate variables.
        Dim dX1 As Double, dX3 As Double
        dX1 = mRadM(1) * (1 - CosD(mThetaM(1)))
        dX3 = mRadM(3) * (1 - CosD(mThetaM(3)))

        '....Pertains to a given original "mHConv".
        Dim pHFlatM As Double
        pHFlatM = mHConv - T - (dX1 + dX3)

        mLFlatM_End = pHFlatM / CosD((90 - mThetaM(3)))   '....Invariant parameter. 

    End Sub


    Private Sub UpdateLFlatM_Mid()
        '=========================      
        'For Template = "1", "2" or "1GenS".
        '
        'This function calculates 'LFlatM_Mid', the length of the flat segment of a
        '....Mid_End Convolution. This length will remain constant thruought the geometry
        '....adjustment process but may be changed during new cross-section creation 
        '........process in the "Design Center".

        '....Intermediate variables.
        Dim dX3 As Double, dX5 As Double
        dX3 = mRadM(3) * (1 - CosD(mThetaM(3)))
        dX5 = mRadM(5) * (1 - CosD(mThetaM(5)))

        '....Pertains to a given original "mHConv".
        Dim pHFlatM As Double
        pHFlatM = mHConv - T - (dX3 + dX5)

        mLFlatM_Mid = pHFlatM / CosD((90 - mThetaM(5)))   '....Invariant parameter. 'SB 08APR08

    End Sub


    Private Sub Calc_MemberVariables(ByVal strGeomType_In As String)
        '============================================================
        '   This routine calcultes the following:
        '   1. Graphics Parameters.
        '   2. HFree and "ZClear", "DControl" & "H11Tol".
        '   3. Wid and HLeg & HConv.

        '
        '....This procedure is called within the following routines:
        '
        '   1. POrient. 
        '   2. RetrieveDBParams, RetrieveNewDBParams.
        '   3. DThetaE1, DThetaM1
        '   4. Draw & DrawESeal.

        Dim pThetaE1Draw As Single
        Dim pThetaM1Draw As Single
        Dim pThetaM3Draw As Single
        Dim pThetaM5Draw As Single          '....Not used here. 

        If strGeomType_In = "STD" Then
            pThetaE1Draw = mThetaE(1)
            pThetaM1Draw = mThetaM(1)
            pThetaM3Draw = mThetaM(3)
            pThetaM5Draw = mThetaM(5)

        ElseIf strGeomType_In = "ADJ" Then
            pThetaE1Draw = mThetaE(1) + mDThetaE1
            pThetaM1Draw = mThetaM(1) + mDThetaM1
            pThetaM3Draw = mThetaM(3) + mDThetaM3
            pThetaM5Draw = mThetaM(5) + mDThetaM5
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
            Calc_GraphicsParams(pThetaE1Draw, pThetaM1Draw, pThetaM3Draw)
            Calc_HFree()        '....mHfree and ZClear, DControl & H11Tol.
            Calc_Wid()          '....mWid, HLeg & HConv.
        Next

        If strGeomType_In = "STD" Then
            '....Save "STANDARD" geometry envelope dimensions.
            mHfreeStd = mHfree
            mWidStd = mWid
        End If

    End Sub

#End Region


#Region "CALCULATIONS: Hfree, Wid :"

    Private Sub Calc_HFree()
        '===================
        'This routine calculates the free height.  Requires graphics parameters.

        '   Calculate Pitches (on mid line).
        '   --------------------------------
        '....End Convolution: 1/2  Pitch.
        Dim pPitch_Half_E As Single
        pPitch_Half_E = (mCenE(1).Y - mCenE(3).Y) + mRadE(3)

        '....Mid-End Convolution: 1/2  Pitch.
        mPitch_Half_M_End = mCenM(3).Y - mCenM(1).Y


        If mTemplateNo = "1" Or mTemplateNo = "2" Then
            '-----------------------------------------
            mPitchM = 2.0# * mPitch_Half_M_End     '....Mid Conv.: Full Pitch.

            '....Free Height. 
            mHfree = 2 * pPitch_Half_E + mNConv * mPitchM + T


        ElseIf mTemplateNo = "1Gen" Or mTemplateNo = "1GenS" Then
            '----------------------------------------------------
            '....Mid-Mid Convolution: Full Pitch
            mPitchM_Mid = 2 * (mCenM(5).Y - mCenM(3).Y)

            '....Free Height. 
            mHfree = 2 * (pPitch_Half_E + mPitch_Half_M_End) + T

            If mNConv >= 2 Then
                mHfree = mHfree + (mNConv - 1) * mPitchM_Mid
            End If

        End If

        '....This following method updates "ZClear", "DControl" & "H11Tol".
        Update_ControlDiaParams()

    End Sub


    Private Sub Calc_Wid()
        '=================
        'This sub calculates the radial width, HLeg & Hconv.
        '
        '   Routine called by:
        '       Calc_MemberVariables

        Dim pRad As Single
        Dim pTheta As Single
        Dim pPt_P As PointF
        Dim pPt_1 As PointF
        Dim pAlpha As Single
        Dim pLegCorner As PointF        '....Corner Edge of the Leg.


        '   CALCULATE: mHLeg.
        '   =================
        '   ....Width between LegCorner & End Convolution.
        '
        If POrient = "External" Then
            '-----------------------
            '....CSYS = CYL-E3 (The datum axis along -Y direction. Angle measured CCW).             
            pRad = mRadE(3) + 0.5 * mT
            pTheta = mThetaE(3)

            '....CSYS = CARTP-E3 (XP axis along datum and YP upwards).
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E3.
            pAlpha = 90    '....CCW angle of rotation between X1 and XP axes.
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            pLegCorner = TranslateAxes(pPt_1, mCenE(3))

            mHLeg = pLegCorner.X - 0.5 * DControl


        ElseIf POrient = "Internal" Then
            '----------------------------       
            '....CSYS = CYL-E3 (The datum axis along -Y direction. Angle measured CW).             
            pRad = mRadE(3) + 0.5 * mT
            pTheta = -mThetaE(3)

            '....CSYS = CARTP-E3 (XP axis along datum and YP downwards).
            pPt_P = CylToCart(pRad, pTheta)

            '....CSYS = CART1_E3.
            pAlpha = 90    '....CCW angle of rotation between X1 and XP axes.
            pPt_1 = RotateAxes(pPt_P, pAlpha)

            '....CSYS = CARTW
            pLegCorner = TranslateAxes(pPt_1, mCenE(3))

            mHLeg = 0.5 * DControl - pLegCorner.X

        End If


        '   CALCULATE: mHConv and various Widths.
        '   =====================================
        '
        '   1. Width of Mid-mid Convolution:
        '   ----------------------------
        '
        '....Radial distance between Mid Segment (3) & (5).
        Dim pDelX As Single
        If POrient = "External" Then
            pDelX = mCenM(3).X - mCenM(5).X

        ElseIf POrient = "Internal" Then
            pDelX = mCenM(5).X - mCenM(3).X
        End If

        Dim pWid_Mid As Single
        pWid_Mid = pDelX + mRadM(3) + mRadM(5) + mT


        '   2. Width of Mid-end Convolution (also known as "HConv"):
        '   --------------------------------
        '
        '....Radial distance between Mid Segment (3) & (1).
        If POrient = "External" Then
            pDelX = mCenM(3).X - mCenE(1).X

        ElseIf POrient = "Internal" Then
            pDelX = mCenE(1).X - mCenM(3).X
        End If

        Dim pWid_End As Single
        pWid_End = pDelX + mRadM(3) + mRadE(1) + mT

        If mTemplateNo <> "1GenS" Then _
        mHConv = pWid_End '....Per our orginal definition. 
        '                  '........Refer to the sketch in SDD.


        '   3. Width between LegCorner & Mid Convolution.
        '   ---------------------------------------------
        '
        Dim pWid_LegCorner_Mid As Single

        If POrient = "External" Then
            pWid_LegCorner_Mid = pLegCorner.X - mCenM(5).X + mRadM(5) + 0.5 * mT

        ElseIf POrient = "Internal" Then
            pWid_LegCorner_Mid = mCenM(5).X + mRadM(5) + 0.5 * mT - pLegCorner.X
        End If


        '   WIDTH:
        '   ======
        '   ....Maximum of mHLeg, pWid_LegCorner_Mid, pWid_End & pWid_Mid
        mWid = Max(Max(mHLeg, pWid_LegCorner_Mid), Max(pWid_End, pWid_Mid))

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

#End Region


#Region "DISPOSE:"

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        '=========================================================
        'mDB.Dispose()
        Me.Finalize()

    End Sub

#End Region


End Class
