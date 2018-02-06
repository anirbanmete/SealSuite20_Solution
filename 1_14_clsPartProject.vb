
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      CLASS MODULE  :  clsProject                             '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  13NOV17                                '
'                                                                              '
'===============================================================================
'PB 09NOV17. See inside for instructions and comments. 

Imports System.Threading
Imports System.Globalization.CultureInfo
Imports System.Linq
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Math
Imports System.Environment
Imports clsLibrary11

<Serializable()> _
Public Class clsPartProject
    Implements ICloneable

#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mProject_ID As Integer

    Private mPNR As New clsPNR
    Private mCustInfo As New clsCustInfo

    Private mSealProcess As Boolean
    Private mSealTest As Boolean
    Private mSealIPE As Boolean

    Private mDateCreated As DateTime
    Private mDateLastModified As DateTime

#End Region

#Region "PROPERTY ROUTINES:"

    '....Project_ID.
    Public Property Project_ID() As Integer
        '===================================
        Get
            Return mProject_ID
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mProject_ID = sngData
        End Set

    End Property


    '....SealIPE
    Public Property SealIPE() As Boolean
        '===============================
        Get
            Return mSealIPE
        End Get

        Set(blnVal As Boolean)
            mSealIPE = blnVal
        End Set
    End Property


    '....SealTest
    Public Property SealTest() As Boolean
        '=================================
        Get
            Return mSealTest
        End Get

        Set(blnVal As Boolean)
            mSealTest = blnVal
        End Set
    End Property


    '....SealProcess
    Public Property SealProcess() As Boolean
        '===================================
        Get
            Return mSealProcess
        End Get

        Set(blnVal As Boolean)
            mSealProcess = blnVal
        End Set
    End Property

    '....DateCreated
    Public Property DateCreated() As DateTime
        '===================================
        Get
            Return mDateCreated
        End Get

        Set(strDate As DateTime)
            mDateCreated = strDate
        End Set
    End Property


    '....DateModified
    Public Property DateLastModified() As DateTime
        '========================================
        Get
            Return mDateLastModified
        End Get

        Set(strDate As DateTime)
            mDateLastModified = strDate
        End Set
    End Property


#Region "PNR:"

    Public Property PNR() As clsPNR
        '==========================
        Get
            Return mPNR
        End Get

        Set(ByVal strObj As clsPNR)
            '----------------------
            mPNR = strObj
        End Set

    End Property

#End Region


#Region "CustInfo:"

    Public Property CustInfo() As clsCustInfo
        '====================================
        Get
            Return mCustInfo
        End Get

        Set(ByVal strObj As clsCustInfo)
            '-------------------------------
            mCustInfo = strObj
        End Set

    End Property

#End Region

#End Region

#Region "UTILITY ROUTINE:"


#End Region

#Region "SERIALIZE-DESERIALIZE:"

    Public Function SaveData_Serialize(FilePath_In As String) As Boolean
        '===============================================================
        Try
            Dim serializer As IFormatter = New BinaryFormatter()
            Dim pFileName As String = FilePath_In & "1.SealIPE"

            Dim saveFile As New FileStream(pFileName, FileMode.Create, FileAccess.Write)

            serializer.Serialize(saveFile, Me)

            saveFile.Close()

            Return True
        Catch
            Return False
        End Try
    End Function


    Public Function RestoreData_Deserialize(FilePath_In As String) As Object
        '===================================================================
        Dim serializer As IFormatter = New BinaryFormatter()
        Dim pFileName As String = FilePath_In & "1.SealIPE"
        Dim openFile As New FileStream(pFileName, FileMode.Open, FileAccess.Read)
        Dim pObj As Object
        pObj = serializer.Deserialize(openFile)

        openFile.Close()

        Return pObj

    End Function

#End Region

#Region "NESTED CLASSES:"

#Region "clsPNR:"

    <Serializable()>
    Public Class clsPNR
        Implements ICloneable

#Region "ENUMERATION TYPES:"
        Enum eDimUnit
            English
            Metric
        End Enum

        Enum eType
            E
            C
            SC
            U
        End Enum

        Enum eAppType
            Face
            Axial
        End Enum

        Enum eLegacyType
            None = -1
            Catalogued = 0
            Other = 1
        End Enum

#End Region

#Region "STRUCTURES:"

        <Serializable()>
        Public Structure sCurrent
            Public Exists As Boolean
            Public TypeNo As String
            Public Val As String
            Public Rev As String
        End Structure

        <Serializable()>
        Public Structure sLegacy
            Public Exists As Boolean
            Public Type As eLegacyType
            Public Val As String
            Public Rev As String
        End Structure

#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"

        Private mCurrent As sCurrent
        Private mLegacy As sLegacy

        Private mParentCurrent As sCurrent
        Private mParentLegacy As sLegacy

        Private mRefDimCurrent As sCurrent
        Private mRefDimLegacy As sLegacy

        Private mRefNotesCurrent As sCurrent
        Private mRefNotesLegacy As sLegacy

        Private mAppType As eAppType
        Private mUnitSystem As eDimUnit

        'Dim mSealTypeNo As String
        Dim mSealType As eType

        Private mHW As New clsHW()

#End Region

#Region "PROPERTY ROUTINES:"
        '===================

#Region "Current:"
        '....Current
        Public ReadOnly Property Current() As sCurrent
            '========================================
            Get
                Return mCurrent
            End Get

        End Property

        Public ReadOnly Property CurrentPN() As String
            '=====================================
            Get
                Return GetCurrentVal()
            End Get

        End Property

        '....Current_Exists
        Public WriteOnly Property Current_Exists() As Boolean
            '============================================== 
            Set(ByVal Value As Boolean)
                mCurrent.Exists = Value
            End Set
        End Property

        '....Current_TypeNo
        Public WriteOnly Property Current_TypeNo() As String
            '============================================== 
            Set(ByVal Value As String)
                mCurrent.TypeNo = Value
                mSealType = CType([Enum].Parse(GetType(eType), SealType_No_Mapping().Item(mCurrent.TypeNo)), eType)
            End Set
        End Property

        '....Current_Val
        Public WriteOnly Property Current_Val() As String
            '=========================================== 
            Set(ByVal Value As String)
                mCurrent.Val = Value
            End Set
        End Property

        '....Current_Rev
        Public WriteOnly Property Current_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mCurrent.Rev = Value
            End Set
        End Property

#End Region

#Region "Legacy:"

        '....Legacy
        Public ReadOnly Property Legacy() As sLegacy
            '========================================
            Get
                Return mLegacy
            End Get

        End Property


        '....Legacy_Exists
        Public WriteOnly Property Legacy_Exists() As Boolean
            '============================================== 
            Set(ByVal Value As Boolean)
                mLegacy.Exists = Value
            End Set
        End Property


        '....Legacy_Type
        Public WriteOnly Property Legacy_Type() As eLegacyType
            '================================================== 
            Set(ByVal Value As eLegacyType)
                mLegacy.Type = Value
            End Set
        End Property

        '....Legacy_Val
        Public WriteOnly Property Legacy_Val() As String
            '============================================ 
            Set(ByVal Value As String)
                mLegacy.Val = Value
            End Set
        End Property

        '....Legacy_Rev
        Public WriteOnly Property Legacy_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mLegacy.Rev = Value
            End Set
        End Property

#End Region

#Region "ParentCurrent:"

        '....ParentCurrent
        Public ReadOnly Property ParentCurrent() As sCurrent
            '===============================================
            Get
                Return mParentCurrent
            End Get

        End Property

        '....ParentCurrent_Exists
        Public WriteOnly Property ParentCurrent_Exists() As Boolean
            '=================================================== 
            Set(ByVal Value As Boolean)
                mParentCurrent.Exists = Value
            End Set
        End Property

        '....ParentCurrent_TypeNo
        Public WriteOnly Property ParentCurrent_TypeNo() As String
            '============================================== 
            Set(ByVal Value As String)
                mParentCurrent.TypeNo = Value
            End Set
        End Property

        '....ParentCurrent_Val
        Public WriteOnly Property ParentCurrent_Val() As String
            '=========================================== 
            Set(ByVal Value As String)
                mParentCurrent.Val = Value
            End Set
        End Property

        '....ParentCurrent_Rev
        Public WriteOnly Property ParentCurrent_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mParentCurrent.Rev = Value
            End Set
        End Property

#End Region

#Region "ParentLegacy:"

        '....ParentLegacy
        Public ReadOnly Property ParentLegacy() As sLegacy
            '========================================
            Get
                Return mParentLegacy
            End Get

        End Property


        '....ParentLegacy_Exists
        Public WriteOnly Property ParentLegacy_Exists() As Boolean
            '============================================== 
            Set(ByVal Value As Boolean)
                mParentLegacy.Exists = Value
            End Set
        End Property


        '....ParentLegacy_Type
        Public WriteOnly Property ParentLegacy_Type() As eLegacyType
            '================================================== 
            Set(ByVal Value As eLegacyType)
                mParentLegacy.Type = Value
            End Set
        End Property

        '....ParentLegacy_Val
        Public WriteOnly Property ParentLegacy_Val() As String
            '============================================ 
            Set(ByVal Value As String)
                mParentLegacy.Val = Value
            End Set
        End Property

        '....ParentLegacy_Rev
        Public WriteOnly Property ParentLegacy_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mParentLegacy.Rev = Value
            End Set
        End Property

#End Region

#Region "RefDimCurrent:"
        '....RefDimCurrent
        Public ReadOnly Property RefDimCurrent() As sCurrent
            '========================================
            Get
                Return mRefDimCurrent
            End Get

        End Property

        '....RefDimCurrent_Exists
        Public WriteOnly Property RefDimCurrent_Exists() As Boolean
            '============================================== 
            Set(ByVal Value As Boolean)
                mRefDimCurrent.Exists = Value
            End Set
        End Property

        '....RefDimCurrent_TypeNo
        Public WriteOnly Property RefDimCurrent_TypeNo() As String
            '============================================== 
            Set(ByVal Value As String)
                mRefDimCurrent.TypeNo = Value
            End Set
        End Property

        '....RefDimCurrent_Val
        Public WriteOnly Property RefDimCurrent_Val() As String
            '=========================================== 
            Set(ByVal Value As String)
                mRefDimCurrent.Val = Value
            End Set
        End Property

        '....RefDimCurrent_Rev
        Public WriteOnly Property RefDimCurrent_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mRefDimCurrent.Rev = Value
            End Set
        End Property

#End Region

#Region "RefDimLegacy:"

        '....RefDimLegacy
        Public ReadOnly Property RefDimLegacy() As sLegacy
            '========================================
            Get
                Return mRefDimLegacy
            End Get

        End Property


        '....RefDimLegacy_Exists
        Public WriteOnly Property RefDimLegacy_Exists() As Boolean
            '============================================== 
            Set(ByVal Value As Boolean)
                mRefDimLegacy.Exists = Value
            End Set
        End Property


        '....RefDimLegacy_Type
        Public WriteOnly Property RefDimLegacy_Type() As eLegacyType
            '================================================== 
            Set(ByVal Value As eLegacyType)
                mRefDimLegacy.Type = Value
            End Set
        End Property

        '....RefDimLegacy_Val
        Public WriteOnly Property RefDimLegacy_Val() As String
            '============================================ 
            Set(ByVal Value As String)
                mRefDimLegacy.Val = Value
            End Set
        End Property

        '....RefDimLegacy_Rev
        Public WriteOnly Property RefDimLegacy_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mRefDimLegacy.Rev = Value
            End Set
        End Property

#End Region

#Region "RefNotesCurrent:"

        '....RefNotesCurrent
        Public ReadOnly Property RefNotesCurrent() As sCurrent
            '========================================
            Get
                Return mRefNotesCurrent
            End Get

        End Property

        '....RefNotesCurrent_Exists
        Public WriteOnly Property RefNotesCurrent_Exists() As Boolean
            '============================================== 
            Set(ByVal Value As Boolean)
                mRefNotesCurrent.Exists = Value
            End Set
        End Property

        '....RefNotesCurrent_TypeNo
        Public WriteOnly Property RefNotesCurrent_TypeNo() As String
            '============================================== 
            Set(ByVal Value As String)
                mRefNotesCurrent.TypeNo = Value
            End Set
        End Property

        '....RefNotesCurrent_Val
        Public WriteOnly Property RefNotesCurrent_Val() As String
            '=========================================== 
            Set(ByVal Value As String)
                mRefNotesCurrent.Val = Value
            End Set
        End Property

        '....RefNotesCurrent_Rev
        Public WriteOnly Property RefNotesCurrent_Rev() As String
            '=========================================== 
            Set(ByVal Value As String)
                mRefNotesCurrent.Rev = Value
            End Set
        End Property

#End Region

#Region "RefNotesLegacy:"

        '....RefNotesLegacy
        Public ReadOnly Property RefNotesLegacy() As sLegacy
            '========================================
            Get
                Return mRefNotesLegacy
            End Get

        End Property


        '....RefNotesLegacy_Exists
        Public WriteOnly Property RefNotesLegacy_Exists() As Boolean
            '======================================================= 
            Set(ByVal Value As Boolean)
                mRefNotesLegacy.Exists = Value
            End Set
        End Property


        '....RefNotesLegacy_Type
        Public WriteOnly Property RefNotesLegacy_Type() As eLegacyType
            '========================================================= 
            Set(ByVal Value As eLegacyType)
                mRefNotesLegacy.Type = Value
            End Set
        End Property

        '....RefNotesLegacy_Val
        Public WriteOnly Property RefNotesLegacy_Val() As String
            '=================================================== 
            Set(ByVal Value As String)
                mRefNotesLegacy.Val = Value
            End Set
        End Property

        '....RefNotesLegacy_Rev
        Public WriteOnly Property RefNotesLegacy_Rev() As String
            '=================================================== 
            Set(ByVal Value As String)
                mRefNotesLegacy.Rev = Value
            End Set
        End Property

#End Region

        '....AppType
        Public Property AppType() As eAppType
            '================================
            Get
                Return mAppType
            End Get

            Set(strData As eAppType)
                mAppType = strData
            End Set
        End Property

        '....UnitSystem
        Public Property UnitSystem() As eDimUnit
            '================================
            Get
                Return mUnitSystem
            End Get

            Set(strData As eDimUnit)
                mUnitSystem = strData
            End Set
        End Property


        Public ReadOnly Property PN() As String
            '==================================
            Get
                Return GetPN()
            End Get
        End Property

        Public ReadOnly Property PN_Rev() As String
            '==================================
            Get
                Return GetPN_Rev()
            End Get
        End Property

        'PB 09NOV17. Have mSealTypeNo & mSealType both as members, having both read/write property 
        ' in mSealTypeNo Property, when you set its value, determine the value mSealType = from the dictionary. Also, have
        ' a direct Get property. 
        'in mSealtype Property, do the similar thing. When you set its value, determine mSealTypeNo. have direct get property.

        'In the program - SealPart & Process, one or the other will be set and its counterpart will be assigned value from the dictionary.

        'Public Property SealTypeNo() As String
        '    '=================================
        '    Get
        '        Return mSealTypeNo
        '    End Get
        '    Set(value As String)
        '        mSealTypeNo = value
        '        mSealType = CType([Enum].Parse(GetType(eType), SealType_No_Mapping().Item(mSealTypeNo)), eType)
        '    End Set
        'End Property


        Public Property SealType() As eType
            '==============================
            Get
                Return mSealType
            End Get
            Set(value As eType)
                mSealType = value
                mCurrent.TypeNo = SealType_No_Mapping().Keys(mSealType)
            End Set

        End Property

#Region "HW"

        Public Property HW() As clsHW
            '========================
            Get
                Return mHW
            End Get

            Set(ByVal strObj As clsHW)
                '---------------------
                mHW = strObj
            End Set

        End Property

#End Region

#End Region


#Region "CONSTRUCTOR:"

        Public Sub New()
            '==========

        End Sub

#End Region

#Region "UTILITY ROUTINES:"

        Private Function SealType_No_Mapping() As Dictionary(Of String, String)
            '==================================================================
            Dim pMapping As New Dictionary(Of String, String)
            pMapping.Add("69", eType.E.ToString())
            pMapping.Add("76", eType.C.ToString())
            pMapping.Add("44", eType.SC.ToString())
            pMapping.Add("79", eType.U.ToString())

            Return pMapping
        End Function

        Private Function GetPN() As String
            '=========================
            Dim pPN As String = ""
            If (mCurrent.Exists) Then
                pPN = GetCurrentVal()
            ElseIf (mLegacy.Exists) Then
                pPN = mLegacy.Val
            End If
            Return pPN
        End Function

        Private Function GetPN_Rev() As String
            '=============================
            Dim pPN_Rev As String = ""
            If (mCurrent.Exists) Then
                pPN_Rev = mCurrent.Rev
            ElseIf (mLegacy.Exists) Then
                pPN_Rev = mLegacy.Rev
            End If
            Return pPN_Rev
        End Function


        Private Function GetCurrentVal() As String
            '=================================
            Dim pPN As String = ""
            Dim pVal As String = ""

            If (mCurrent.Exists) Then
                pVal = mCurrent.Val
                pPN = "NH-" & mCurrent.TypeNo & pVal
            End If

            Return pPN

        End Function



#End Region


#Region "NESTED CLASS:"

#Region "clsHW:"

        <Serializable()>
        Public Class clsHW
            Implements ICloneable

#Region "NAMED CONSTANT:"
            Private Const mcCountSegment As Integer = 3
            Private Const mcSTRAIGHT_SEC_RADIUS As Single = 999.0  '....Irrespective of Unit Sys.

#End Region

#Region "STRUCTURE:"
            <Serializable()>
            Public Structure sPlating
                Public Exists As Boolean
                Public Code As String
                Public ThickCode As String
                Public ThickMin As Double
                Public ThickMax As Double
            End Structure
#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"  'PB 09NOV17. Double indentation everywhere. Do properly not such a sloppy job. Everything should be neat and clean at the first place.
            'AES 10NOV17    Manual indentation is not possible in Visual Studio.

            Private mPOrient As String
            Private mMCrossSecNo As String
            Private mGeomTemplate As Boolean

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

            Private mDHfree As Single
            Private mDThetaOpening As Single

            Private mT As Single

            'Private mUnit As New clsPartUnit
            Private mUnit As clsUnit
            Private mDThetaE1 As Single
            Private mDThetaM1 As Single

            Private mPNR As clsPNR

#End Region

#Region "CONSTRUCTOR:"

            'Public Sub New(ByVal PNR_IN As clsPNR)
            '    '================================
            '    mPNR = PNR_IN.Clone()
            '    mCountSegment = mcCountSegment
            '    mCoating = "None"

            'End Sub

            Public Sub New()
                '===========
                'mPNR = New clsPartProject.clsPNR()
                mCountSegment = mcCountSegment
                mCoating = "None"

            End Sub

#End Region

#Region "PROPERTY ROUTINE:"

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
                    If (mPNR.SealType = eType.E) Then
                        Dim pESeal As New IPE_clsESeal("E-Seal", mUnit.System, mPOrient)
                        With pESeal
                            .MCrossSecNo = mMCrossSecNo
                            mHfree = .Hfree
                            mHFreeTolStd = .HfreeTolStd
                            mHfreeTol(1) = .HFreeTol(1)
                            mHfreeTol(2) = .HFreeTol(2)
                            mTStd = .TStd
                        End With

                    ElseIf (mPNR.SealType = eType.C Or mPNR.mSealType = eType.SC) Then
                        Dim pCSeal As New IPE_clsCSeal("C-Seal", mUnit.System, mPOrient)
                        With pCSeal
                            .MCrossSecNo = mMCrossSecNo
                            mHfree = .Hfree
                            mHFreeTolStd = .HfreeTolStd
                            mHfreeTol(1) = .HFreeTol(1)
                            mHfreeTol(2) = .HFreeTol(2)
                            mTStd = .TStd
                        End With

                    ElseIf (mPNR.SealType = eType.U) Then
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


            '....GeomTemplate
            Public Property GeomTemplate() As Boolean
                '=====================================
                Get
                    Return mGeomTemplate
                End Get

                Set(blnVal As Boolean)
                    mGeomTemplate = blnVal
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

            Public WriteOnly Property PlatingThickMin() As Double
                '================================================
                Set(ByVal Value As Double)
                    mPlating.ThickMin = Value
                End Set
            End Property

            Public WriteOnly Property PlatingThickMax() As Double
                '=================================================
                Set(ByVal Value As Double)
                    mPlating.ThickMax = Value
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

#End Region

#Region "UTILITY ROUTINES:"

            Public Sub InitializePNR(ByVal PNR_IN As clsPNR)
                '===========================================
                mPNR = New clsPartProject.clsPNR()
                mPNR = PNR_IN.Clone()
                mUnit = New clsUnit
                mUnit.System = mPNR.UnitSystem.ToString()
            End Sub


            Private Sub SetAdjusted()
                '====================

                If (mPNR.SealType = eType.C) Then

                    If mHfree < gcEPS Or mTStd < gcEPS Then
                        '....The baseline values of all the adjusting parameters have not been 
                        '........assigned yet.

                        mAdjusted = False
                        Exit Sub


                    Else
                        '....mHfree, mThetaOpening and mT all have been duly assigned.
                        '........Check any adjustment has been done.

                        If Abs(mDHfree) > gcEPS Or
                           Abs(mDThetaOpening) > gcEPS Or
                           Abs(mT - mTStd) > gcEPS Then

                            mAdjusted = True

                        Else
                            mAdjusted = False
                        End If

                    End If

                ElseIf (mPNR.SealType = eType.E) Then

                    If Abs(mDThetaE1) > gcEPS Or
                        Abs(mDThetaM1) > gcEPS Then

                        mAdjusted = True

                    Else
                        '    '....The baseline values of all the adjusting parameters have not been 
                        '    '........assigned yet.
                        mAdjusted = False
                    End If

                End If

            End Sub


#Region "DATABASE RELATED ROUTINES:"

            Public Function GetMatName(ByVal SealType_In As String, ByVal MatCode_In As String) As String
                '=========================================================================================
                Dim pMatName As String = ""

                Dim pMCSEntities As New SealIPEMCSDBEntities()

                If (SealType_In = "S") Then
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
                       Where pRec.fldCode = MatCode_In Select pRec).First()

                    pMatName = pQry.fldName.ToString().Trim()

                    If (pMatName.Contains("Cobalt Chromium-Nickel Alloy")) Then
                        pMatName = pMatName.Replace("Cobalt Chromium-Nickel Alloy", "Co-Cr-Ni")
                    End If

                Else
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                       Where pRec.fldCode = MatCode_In Select pRec).First()

                    pMatName = pQry.fldName.ToString().Trim()
                End If

                Return pMatName

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
                Dim pBinSerializer As New BinaryFormatter(Nothing,
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
            Dim pBinSerializer As New BinaryFormatter(Nothing,
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

#End Region

#Region "clsCustInfo:"

    <Serializable()> _
    Public Class clsCustInfo
        Implements ICloneable

#Region "MEMBER VARIABLES:"

        Private mCustName As String
        Private mPlatName As String
        Private mLocName As String
        Private mPN_Cust As String
        Private mPN_Cust_Rev As String
#End Region


#Region "PROPERTY ROUTINE:"

        '....CustName
        Public Property CustName() As String
            '================================
            Get
                Return mCustName
            End Get

            Set(ByVal value As String)
                mCustName = value
            End Set

        End Property

        '....PlatName
        Public Property PlatName() As String
            '================================
            Get
                Return mPlatName
            End Get

            Set(ByVal value As String)
                mPlatName = value
            End Set

        End Property

        '....LocName
        Public Property LocName() As String
            '================================
            Get
                Return mLocName
            End Get

            Set(ByVal value As String)
                mLocName = value
            End Set

        End Property

        '....PN_Cust
        Public Property PN_Cust() As String
            '================================
            Get
                Return mPN_Cust
            End Get

            Set(ByVal value As String)
                mPN_Cust = value
            End Set

        End Property

        '....PN_Cust_Rev
        Public Property PN_Cust_Rev() As String
            '===================================
            Get
                Return mPN_Cust_Rev
            End Get

            Set(ByVal value As String)
                mPN_Cust_Rev = value
            End Set

        End Property

#End Region

#Region "UTILITY ROUTINE:"

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
