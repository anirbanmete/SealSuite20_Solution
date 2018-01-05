'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsPN                                  '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07NOV17                                '
'                                                                              '
'===============================================================================
'PB 07NOV17. Instructions. PN DB will change also. Let's discuss tomorrow)

Imports System.Threading
Imports System.Globalization
Imports System.Linq
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class clsPart_PN

#Region "ENUMERATION TYPES:"

    Enum eTypeNo              ' PB 11NOV17. May be eTypeNo
        E = 69              'PB 11NOV17. Just E, C, SC, U, Use string as the value '69'. It may contain letter e.g. '68A', '9X'
        C = 76
        SC = 44
        U = 79
    End Enum

    Enum eLegacyType
        None = -1
        Other = 0
        Catalogued = 1
    End Enum

#End Region

#Region "STRUCTURES:"

#Region "Parker P/N:"
    <Serializable()>
    Public Structure sCurrent           'sCurrent
        Public Exists As Boolean
        Public TypeNo As eTypeNo
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


#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"

    'Private mID As Integer 'PB 11NOV17. Id may not be necessary here. Id tracking may be done in frm. 

    'Private mRev As New List(Of sRev)

    'Private mParent As String           'PB 11NOV17. We may want to have mParentNew, mParentLegacy (DB will change also. Let's discuss tomorrow).
    'Private mParentRev As String        'PB 11NOV17.


    Private mCurrent As sCurrent
    Private mLegacy As sLegacy

    Private mParentCurrent As sCurrent
    Private mParentLegacy As sLegacy

    Private mRefDimCurrent As sCurrent
    Private mRefDimLegacy As sLegacy

    Private mRefNotesCurrent As sCurrent
    Private mRefNotesLegacy As sLegacy

    ' Let's try to add: mRefDimNew, mRefDimLegacy, mRefNotesNew, mRefNotesLegacy,
    'Private mRefDimNew As sRefDimNew
    'Private mRefDimLegacy As sRefDimLegacy
    'Private mRefNotesNew As sRefNotesNew
    'Private mRefNotesLegacy As sRefNotesLegacy

    Private mAppType As String      '....Face Seal/Axial Seal   'PB 11NOV17. Let's discuss the name. 

    Private mGeomTemplate As Boolean
    Private mSealIPE As Boolean
    Private mSealTest As Boolean
    Private mSealProcess As Boolean

    Private mDate As DateTime

#End Region


#Region "PROPERTY ROUTINES:"
    '======================

    ''....ID
    'Public Property ID() As Integer
    '    '============================
    '    Get
    '        Return mID
    '    End Get

    '    Set(strVal As Integer)
    '        mID = strVal
    '    End Set
    'End Property

    '....NewPN
    Public ReadOnly Property NewPN() As sNew
        '========================================
        Get
            Return mNewPN
        End Get

    End Property

    Public ReadOnly Property NewPNNo() As String
        '=====================================
        Get
            Return GetNewVal()
        End Get

    End Property

    '....New_Exists
    Public WriteOnly Property New_Exists() As Boolean
        '============================================== 
        Set(ByVal Value As Boolean)
            mNewPN.Exists = Value
        End Set
    End Property

    '....New_TypeNo
    Public WriteOnly Property New_TypeNo() As eTypeNo
        '============================================== 
        Set(ByVal Value As eTypeNo)
            mNewPN.TypeNo = Value
        End Set
    End Property

    '....New_Val
    Public WriteOnly Property New_Val() As String
        '=========================================== 
        Set(ByVal Value As String)
            mNewPN.Val = Value
        End Set
    End Property

    '....New_Rev
    Public WriteOnly Property New_Rev() As String
        '=========================================== 
        Set(ByVal Value As String)
            mNewPN.Rev = Value
        End Set
    End Property

    '....New_ParentNewExists
    Public WriteOnly Property New_ParentNewExists() As Boolean
        '=================================================== 
        Set(ByVal Value As Boolean)
            mNewPN.ParentNewExists = Value
        End Set
    End Property

    '....New_ParentNewVal
    Public WriteOnly Property New_ParentNewVal() As String
        '=========================================== 
        Set(ByVal Value As String)
            mNewPN.ParentNewVal = Value
        End Set
    End Property

    '....New_ParentNewRev
    Public WriteOnly Property New_ParentNewRev() As String
        '=========================================== 
        Set(ByVal Value As String)
            mNewPN.ParentNewRev = Value
        End Set
    End Property

    '....New_ParentLegacyExists
    Public WriteOnly Property New_ParentLegacyExists() As Boolean
        '=================================================== 
        Set(ByVal Value As Boolean)
            mNewPN.ParentLegacyExists = Value
        End Set
    End Property

    '....New_ParentLegacyVal
    Public WriteOnly Property New_ParentLegacyVal() As String
        '=========================================== 
        Set(ByVal Value As String)
            mNewPN.ParentLegacyVal = Value
        End Set
    End Property

    '....New_ParentLegacyRev
    Public WriteOnly Property New_ParentLegacyRev() As String
        '=========================================== 
        Set(ByVal Value As String)
            mNewPN.ParentLegacyRev = Value
        End Set
    End Property


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

    '....Legacy_ParentNewExists
    Public WriteOnly Property Legacy_ParentNewExists() As Boolean
        '=================================================== 
        Set(ByVal Value As Boolean)
            mLegacy.ParentNewExists = Value
        End Set
    End Property

    '....Legacy_ParentNewVal
    Public WriteOnly Property Legacy_ParentNewVal() As String
        '=========================================== 
        Set(ByVal Value As String)
            mLegacy.ParentNewVal = Value
        End Set
    End Property

    '....Legacy_ParentNewRev
    Public WriteOnly Property Legacy_ParentNewRev() As String
        '=========================================== 
        Set(ByVal Value As String)
            mLegacy.ParentNewRev = Value
        End Set
    End Property

    '....Legacy_ParentLegacyExists
    Public WriteOnly Property Legacy_ParentLegacyExists() As Boolean
        '=================================================== 
        Set(ByVal Value As Boolean)
            mLegacy.ParentLegacyExists = Value
        End Set
    End Property

    '....Legacy_ParentLegacyVal
    Public WriteOnly Property Legacy_ParentLegacyVal() As String
        '=========================================== 
        Set(ByVal Value As String)
            mLegacy.ParentLegacyVal = Value
        End Set
    End Property

    '....Legacy_ParentLegacyRev
    Public WriteOnly Property Legacy_ParentLegacyRev() As String
        '=========================================== 
        Set(ByVal Value As String)
            mLegacy.ParentLegacyRev = Value
        End Set
    End Property

    '....RefDimNew           
    Public ReadOnly Property RefDimNew() As sRefDimNew
        '==============================================
        Get
            Return mRefDimNew
        End Get

    End Property

    Public WriteOnly Property RefDimNew_Exists() As Boolean
        '===================================================
        Set(value As Boolean)
            mRefDimNew.Exists = value
        End Set
    End Property

    Public WriteOnly Property RefDimNew_Val() As String
        '================================================
        Set(value As String)
            mRefDimNew.Val = value
        End Set
    End Property

    Public WriteOnly Property RefDimNew_Rev() As String
        '================================================
        Set(value As String)
            mRefDimNew.Rev = value
        End Set
    End Property

    Public ReadOnly Property RefDimLegacy() As sRefDimLegacy
        '==============================================
        Get
            Return mRefDimLegacy
        End Get

    End Property

    Public WriteOnly Property RefDimLegacy_Exists() As Boolean
        '===================================================
        Set(value As Boolean)
            mRefDimLegacy.Exists = value
        End Set
    End Property

    Public WriteOnly Property RefDimLegacy_Val() As String
        '================================================
        Set(value As String)
            mRefDimLegacy.Val = value
        End Set
    End Property

    Public WriteOnly Property RefDimLegacy_Rev() As String
        '================================================
        Set(value As String)
            mRefDimLegacy.Rev = value
        End Set
    End Property

    Public ReadOnly Property RefNotesNew() As sRefNotesNew
        '==============================================
        Get
            Return mRefNotesNew
        End Get

    End Property

    Public WriteOnly Property RefNotesNew_Exists() As Boolean
        '===================================================
        Set(value As Boolean)
            mRefNotesNew.Exists = value
        End Set
    End Property

    Public WriteOnly Property RefNotesNew_Val() As String
        '================================================
        Set(value As String)
            mRefNotesNew.Val = value
        End Set
    End Property

    Public WriteOnly Property RefNotesNew_Rev() As String
        '================================================
        Set(value As String)
            mRefNotesNew.Rev = value
        End Set
    End Property

    Public ReadOnly Property RefNotesLegacy() As sRefNotesLegacy
        '==============================================
        Get
            Return mRefNotesLegacy
        End Get

    End Property

    Public WriteOnly Property RefNotesLegacy_Exists() As Boolean
        '===================================================
        Set(value As Boolean)
            mRefNotesLegacy.Exists = value
        End Set
    End Property

    Public WriteOnly Property RefNotesLegacy_Val() As String
        '================================================
        Set(value As String)
            mRefNotesLegacy.Val = value
        End Set
    End Property

    Public WriteOnly Property RefNotesLegacy_Rev() As String
        '================================================
        Set(value As String)
            mRefNotesLegacy.Rev = value
        End Set
    End Property

    '....AppType
    Public Property AppType() As String
        '================================
        Get
            Return mAppType
        End Get

        Set(strData As String)
            mAppType = strData
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


    '....Date
    Public Property DateTime() As DateTime
        '===================================
        Get
            Return mDate
        End Get

        Set(strDate As DateTime)
            mDate = strDate
        End Set
    End Property

    '....Index
    'Public Property Index() As Integer
    '    '==============================
    '    Get
    '        Return mIndex
    '    End Get

    '    Set(sngData As Integer)
    '        mSealProcess = sngData
    '    End Set
    'End Property

#End Region

#Region "UTILITY ROUTINES:"

    Private Function GetNewVal() As String
        '=================================
        Dim pPN As String = ""
        Dim pVal As String = ""

        pVal = mNewPN.Val
        'If (mNew.SeqNo > 0) Then
        'pSeqNo = mNew.SeqNo.ToString()
        'If (mNew.SeqNo.ToString().Length < 2) Then
        '    pSeqNo = "000" & mNew.SeqNo.ToString()
        'ElseIf (mNew.SeqNo.ToString().Length < 3) Then
        '    pSeqNo = "00" & mNew.SeqNo.ToString()
        'ElseIf (mNew.SeqNo.ToString().Length < 4) Then
        '    pSeqNo = "0" & mNew.SeqNo.ToString()
        'Else
        '    pSeqNo = mNew.SeqNo.ToString()
        'End If
        'End If


        If (mNewPN.TypeNo = eTypeNo.E) Then
            pPN = "NH-" & Int(eTypeNo.E) & pVal

        ElseIf (mNewPN.TypeNo = eTypeNo.C) Then
            pPN = "NH-" & Int(eTypeNo.C) & pVal

        ElseIf (mNewPN.TypeNo = eTypeNo.SC) Then
            pPN = "NH-" & Int(eTypeNo.SC) & pVal

        ElseIf (mNewPN.TypeNo = eTypeNo.U) Then
            pPN = "NH-" & Int(eTypeNo.U) & pVal
        End If

        'pVal = "NH" & mNew.Type & mNew.SeqNo

        Return pPN

    End Function

#End Region

End Class