
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  clsHW                                  '
'                        VERSION NO  :  1.3                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  31JUL17                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Globalization
Imports System
Imports System.Threading
Imports System.Math
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary


<Serializable()> _
Public Class Part_clsHW
    Implements ICloneable

#Region "NAMED CONSTANT:"
    Private Const mcCountSegment As Integer = 3
    Private Const mcSTRAIGHT_SEC_RADIUS As Single = 999.0  '....Irrespective of Unit Sys.

#End Region

#Region "STRUCTURE:"
    <Serializable()> _
    Public Structure sPlating
        Public Exists As Boolean
        Public Code As String
        Public ThickCode As String
    End Structure
#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"


    Private mType As String
    Private mPOrient As String
    Private mMCrossSecNo As String

    Private mIsSegmented As Boolean
    Private mCountSegment As Integer = 3

    Private mMatName As String
    Private mHT As Integer
    Private mTemper As Integer
    Private mCoating As String
    Private mSFinish As Integer
    Private mPlating As sPlating
    Private mDControl As Double
    Private mH11Tol As Double
    Private mAdjusted As Boolean


    Private mHfree As Single
    Private mHFreeTolStd As Single
    Private mHfreeTol(2) As Single    '....1 : - Tol.,    '(FUNDAMENTAL)     
    '                                 '....2 : + Tol.  
    Private mTStd As Single

    'AES 27JUL17
    Private mDHfree As Single
    Private mDThetaOpening As Single

    Private mT As Single

    Private mUnit As New Part_clsUnit

    'AES 11OCT17
    Private mDThetaE1 As Single
    Private mDThetaM1 As Single

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New()
        '==========
        mCountSegment = mcCountSegment
        mType = "E-Seal"
        mCoating = "None"
    End Sub

#End Region

#Region "PROPERTY ROUTINE:"


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


    '....Type.
    Public Property Type() As String
        '===============================
        Get
            Return mType
        End Get

        Set(ByVal strData As String)
            '------------------------
            mType = strData
        End Set

    End Property


    '....POrient.
    Public Property POrient() As String
        '===============================
        Get
            Return mPOrient
        End Get

        Set(ByVal strData As String)
            '------------------------
            mPOrient = strData
        End Set

    End Property


    '....Cross Section No.
    Public Property MCrossSecNo() As String
        '===================================
        Get
            Return mMCrossSecNo
        End Get

        Set(ByVal strData As String)
            mMCrossSecNo = strData
            'Dim pUnitSystem As String = "English"
            If (mType = "E-Seal") Then
                Dim pESeal As New IPE_clsESeal("E-Seal", mUnit.System, mPOrient)
                With pESeal
                    .MCrossSecNo = mMCrossSecNo
                    mHfree = .Hfree
                    mHFreeTolStd = .HfreeTolStd
                    mHfreeTol(1) = .HFreeTol(1)
                    mHfreeTol(2) = .HFreeTol(2)
                    mTStd = .TStd
                End With

            ElseIf (mType = "C-Seal" Or mType = "SC-Seal") Then
                Dim pCSeal As New IPE_clsCSeal("C-Seal", mUnit.System, mPOrient)
                With pCSeal
                    .MCrossSecNo = mMCrossSecNo
                    mHfree = .Hfree
                    mHFreeTolStd = .HfreeTolStd
                    mHfreeTol(1) = .HFreeTol(1)
                    mHfreeTol(2) = .HFreeTol(2)
                    mTStd = .TStd
                End With

            ElseIf (mType = "U-Seal") Then
                Dim pUSeal As New IPE_clsUSeal("U-Seal", mUnit.System, mPOrient)
                With pUSeal
                    Dim pSealEntities As New SealIPEMCSDBEntities()
                    Dim pRecord = (From pRec In pSealEntities.tblUSeal_Geom
                                    Where pRec.fldCrossSecNo = mMCrossSecNo Select pRec).ToList()
                    If (pRecord.Count > 0) Then
                        If (pRecord(0).fldGeomTemplate = True) Then
                            .MCrossSecNo = mMCrossSecNo
                            mHfree = .Hfree
                            mHFreeTolStd = .HfreeTolStd
                            mHfreeTol(1) = .HFreeTol(1)
                            mHfreeTol(2) = .HFreeTol(2)
                            mTStd = .TStd
                        ElseIf (pRecord(0).fldGeomTemplate = False) Then
                            Dim pQry = (From pRec In pSealEntities.tblUSeal_ManfData
                                    Where pRec.fldCrossSecNo = mMCrossSecNo Select pRec).ToList()
                            If (pQry.Count > 0) Then

                                mHfree = pQry(0).fldHFree

                                'Retrieve Manufacturing Parameters:
                                '---------------------------------
                                '....Build Tolerance.
                                Dim psngBuildTol As Single
                                If IsNothing(pQry(0).fldBuildTol) = True Then
                                    psngBuildTol = 0.0#

                                Else
                                    psngBuildTol = pQry(0).fldBuildTol * mUnit.CFacConL
                                End If

                                '....Assign Build tolerance value to the corresponding member variables.
                                mHFreeTolStd = psngBuildTol         '....Standard Tolerance. Fixed. 

                                '....Initialize user-adjustable non-standard Tolerances. 
                                HFreeTol(1) = psngBuildTol
                                HFreeTol(2) = psngBuildTol
                            End If

                        End If
                    End If

                End With

            End If

        End Set

    End Property


    '....IsSegmented
    Public Property IsSegmented() As Boolean
        '===================================
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

    '....MatName.
    Public Property MatName() As String
        '===============================
        Get
            Return mMatName
        End Get

        Set(ByVal strData As String)
            '------------------------
            mMatName = strData
        End Set

    End Property


    '....HT.
    Public Property HT() As Integer
        '===============================
        Get
            Return mHT
        End Get

        Set(ByVal intData As Integer)
            '------------------------
            mHT = intData
        End Set

    End Property


    '....Temper.
    Public Property Temper() As Integer
        '===============================
        Get
            Return mTemper
        End Get

        Set(ByVal intData As Integer)
            '------------------------
            mTemper = intData
        End Set

    End Property


    '....Coating.
    Public Property Coating() As String
        '===============================
        Get
            Return mCoating
        End Get

        Set(ByVal strData As String)
            '------------------------
            mCoating = strData
        End Set

    End Property


    '....SFinish.
    Public Property SFinish() As Integer
        '===============================
        Get
            Return mSFinish
        End Get

        Set(ByVal intData As Integer)
            '------------------------
            mSFinish = intData
        End Set

    End Property


    Public ReadOnly Property Plating() As sPlating
        '=========================================
        Get
            Return mPlating
        End Get
    End Property

    Public WriteOnly Property PlatingExists() As Boolean
        '===============================================
        Set(ByVal Value As Boolean)
            mPlating.Exists = Value
        End Set
    End Property

    Public WriteOnly Property PlatingCode() As String
        '=============================================
        Set(ByVal Value As String)
            mPlating.Code = Value
        End Set

    End Property


    Public WriteOnly Property PlatingThickCode() As String
        '==============================================
        Set(ByVal Value As String)
            mPlating.ThickCode = Value
        End Set
    End Property

    '....HFree  
    Public Property Hfree() As Single
        '============================
        Get
            Return mHfree
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mHfree = sngData
        End Set

    End Property


    '....HFreeTolStd  
    Public Property HFreeTolStd() As Single
        '============================
        Get
            Return mHFreeTolStd
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mHFreeTolStd = sngData
        End Set

    End Property

    '....HFree  
    Public Property DHfree() As Single
        '============================
        Get
            Return mDHfree
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDHfree = sngData
            SetAdjusted()
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


    '....DThetaOpening  
    Public Property DThetaOpening() As Single
        '====================================
        Get
            Return mDThetaOpening
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDThetaOpening = sngData
            SetAdjusted()
        End Set

    End Property


    Public Property TStd() As Single
        '========================
        Get
            Return mTStd
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mTStd = sngData

        End Set

    End Property


    Public Property T() As Single
        '========================
        Get
            Return mT
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mT = sngData
            SetAdjusted()
        End Set

    End Property


    '....DControl  
    Public Property DControl() As Double
        '============================
        Get
            Return mDControl
        End Get

        Set(ByVal sngData As Double)
            '------------------------------
            mDControl = sngData
        End Set

    End Property

    '....H11Tol  
    Public Property H11Tol() As Double
        '============================
        Get
            Return mH11Tol
        End Get

        Set(ByVal sngData As Double)
            '------------------------------
            mH11Tol = sngData
        End Set

    End Property


    '....Adjusted  
    Public Property Adjusted() As Boolean
        '================================
        Get
            Return mAdjusted
        End Get

        Set(ByVal sngData As Boolean)
            '------------------------------
            mAdjusted = sngData
        End Set

    End Property


    '....DThetaE1  
    Public Property DThetaE1() As Single
        '===============================
        Get
            Return mDThetaE1
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDThetaE1 = sngData
            SetAdjusted()
        End Set

    End Property

    '....DThetaM1  
    Public Property DThetaM1() As Single
        '===============================
        Get
            Return mDThetaM1
        End Get

        Set(ByVal sngData As Single)
            '-----------------------
            mDThetaM1 = sngData
            SetAdjusted()
        End Set

    End Property

#End Region

#Region "UTILITY ROUTINES:"

    Private Sub SetAdjusted()
        '====================

        If (mType = "C-Seal") Then

            If mHfree < gcEPS Or mTStd < gcEPS Then
                '....The baseline values of all the adjusting parameters have not been 
                '........assigned yet.

                mAdjusted = False
                Exit Sub


            Else
                '....mHfree, mThetaOpening and mT all have been duly assigned.
                '........Check any adjustment has been done.

                If Abs(mDHfree) > gcEPS Or _
                   Abs(mDThetaOpening) > gcEPS Or _
                   Abs(mT - mTStd) > gcEPS Then

                    mAdjusted = True

                Else
                    mAdjusted = False
                End If

            End If

        ElseIf (mType = "E-Seal") Then

            If Abs(mDThetaE1) > gcEPS Or _
                   Abs(mDThetaM1) > gcEPS Then

                mAdjusted = True

            Else
                '    '....The baseline values of all the adjusting parameters have not been 
                '    '........assigned yet.
                mAdjusted = False
            End If

        End If

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


End Class
