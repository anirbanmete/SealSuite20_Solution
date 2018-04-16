'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsApp                         '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================
Imports System.Globalization
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms

<Serializable()> _
Public Class clsProcessProj_App

#Region "MEMBER VARIABLES:"

    Private mEqp As String
    Private mExistingSeal As String
    Private mType As String
    Private mFluid As String
    Private mMaxLeak As Double
    Private mIsPressCyclic As Boolean
    Private mPressCycle_Freq As Double
    Private mPressCycle_Amp As Double
    Private mShaped As Boolean
    Private mIsOoR As Boolean
    Private mIsSplitRing As Boolean
    Private mIsPreComp As Boolean

    Private mOpCond As New clsOpCond
    Private mLoad As New clsLoad
    Private mCavity As New clsCavity
    Private mCavityFlange As New clsCavityFlange

    Private mFace As New clsFace
    Private mAxial As New clsAxial

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....Eqp
    Public Property Eqp() As String
        '============================
        Get
            Return mEqp
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mEqp = strData
        End Set

    End Property

    '....ExistingSeal
    Public Property ExistingSeal() As String
        '====================================
        Get
            Return mExistingSeal
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mExistingSeal = strData
        End Set

    End Property

    '....Type
    Public Property Type() As String
        '===========================
        Get
            Return mType
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mType = strData
        End Set

    End Property

    '....Fluid
    Public Property Fluid() As String
        '====================================
        Get
            Return mFluid
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mFluid = strData
        End Set

    End Property

    '....MaxLeak
    Public Property MaxLeak() As Double
        '===============================
        Get
            Return mMaxLeak
        End Get

        Set(ByVal dblData As Double)
            '-------------------------------
            mMaxLeak = dblData
        End Set

    End Property

    '....IsPressCyclic
    Public Property IsPressCyclic() As Boolean
        '===============================
        Get
            Return mIsPressCyclic
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsPressCyclic = blnData
        End Set

    End Property

    '....PressCycle_Freq
    Public Property PressCycle_Freq() As Double
        '===============================
        Get
            Return mPressCycle_Freq
        End Get

        Set(ByVal dblData As Double)
            '-------------------------------
            mPressCycle_Freq = dblData
        End Set

    End Property

    '....PressCycle_Amp
    Public Property PressCycle_Amp() As Double
        '===============================
        Get
            Return mPressCycle_Amp
        End Get

        Set(ByVal dblData As Double)
            '-------------------------------
            mPressCycle_Amp = dblData
        End Set

    End Property

    '....Shaped
    Public Property Shaped() As Boolean
        '===============================
        Get
            Return mShaped
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mShaped = blnData
        End Set

    End Property

    '....IsOoR
    Public Property IsOoR() As Boolean
        '===========================
        Get
            Return mIsOoR
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsOoR = blnData
        End Set

    End Property

    '....IsSplitRing
    Public Property IsSplitRing() As Boolean
        '=================================
        Get
            Return mIsSplitRing
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsSplitRing = blnData
        End Set

    End Property

    '....IsPreComp
    Public Property IsPreComp() As Boolean
        '===============================
        Get
            Return mIsPreComp
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsPreComp = blnData
        End Set

    End Property


#Region "OpCond"

    Public Property OpCond() As clsOpCond
        '==================================
        Get
            Return mOpCond
        End Get

        Set(ByVal strObj As clsOpCond)
            '-------------------------------
            mOpCond = strObj
        End Set

    End Property

#End Region

#Region "Load"

    Public Property Load() As clsLoad
        '==============================
        Get
            Return mLoad
        End Get

        Set(ByVal strObj As clsLoad)
            '-------------------------------
            mLoad = strObj
        End Set

    End Property

#End Region

#Region "Cavity"

    Public Property Cavity() As clsCavity
        '==================================
        Get
            Return mCavity
        End Get

        Set(ByVal strObj As clsCavity)
            '-------------------------------
            mCavity = strObj
        End Set

    End Property

#End Region

#Region "CavityFlange"

    Public Property CavityFlange() As clsCavityFlange
        '==================================
        Get
            Return mCavityFlange
        End Get

        Set(ByVal strObj As clsCavityFlange)
            '-------------------------------
            mCavityFlange = strObj
        End Set

    End Property

#End Region

#Region "Face"

    Public Property Face() As clsFace
        '=============================
        Get
            Return mFace
        End Get

        Set(ByVal strObj As clsFace)
            '-------------------------------
            mFace = strObj
        End Set

    End Property

#End Region

#Region "Axial"

    Public Property Axial() As clsAxial
        '=============================
        Get
            Return mAxial
        End Get

        Set(ByVal strObj As clsAxial)
            '-------------------------------
            mAxial = strObj
        End Set

    End Property

#End Region

#Region "Risk"

    Public Property Risk() As clsProcessProj_Risk
        '=======================================
        Get
            Return mRisk
        End Get

        Set(ByVal strObj As clsProcessProj_Risk)
            '------------------------------------------------
            mRisk = strObj
        End Set

    End Property

#End Region

#Region "EditedBy"

    Public Property EditedBy() As clsProcessProj_EditedBy
        '================================================
        Get
            Return mEditedBy
        End Get

        Set(ByVal strObj As clsProcessProj_EditedBy)
            '-------------------------------
            mEditedBy = strObj
        End Set

    End Property

#End Region

#End Region

#Region "DB RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '=====================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try

            '....tblApp
            Dim pQryAppCount As Integer = (From pRec In pSealProcessDBEntities.tblApp
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryAppCount > 0) Then

                Dim pQryApp = (From pRec In pSealProcessDBEntities.tblApp
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mEqp = pQryApp.fldEqp
                mExistingSeal = pQryApp.fldExistingSeal
                mType = pQryApp.fldType
                mFluid = pQryApp.fldFluid
                mMaxLeak = pQryApp.fldMaxLeak
                mIsPressCyclic = pQryApp.fldIsPressCyclic
                mPressCycle_Freq = pQryApp.fldPressCycle_Freq
                mPressCycle_Amp = pQryApp.fldPressCycle_Amp
                mShaped = pQryApp.fldShaped
                mIsOoR = pQryApp.fldIsOoR
                mIsSplitRing = pQryApp.fldIsSplitRing
                mIsPreComp = pQryApp.fldIsPreComp
                'mEditedBy.User_Name = pQryApp.fldUserName
                'mEditedBy.User_Signed = pQryApp.fldSigned

                'If (Not IsNothing(pQryApp.fldDateSigned) And Not IsDBNull(pQryApp.fldDateSigned)) Then
                '    mEditedBy.User_DateSigned = pQryApp.fldDateSigned
                'End If

            End If

            mOpCond.RetrieveFromDB(ProjectID_In)
            mLoad.RetrieveFromDB(ProjectID_In)
            mCavity.RetrieveFromDB(ProjectID_In)
            mCavityFlange.RetrieveFromDB(ProjectID_In)

            mFace.RetrieveFromDB(ProjectID_In)
            mAxial.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....App table
        Dim pAppCount As Integer = (From App In pSealProcessDBEntities.tblApp
                                            Where App.fldProcessProjectID = ProjectID_In Select App).Count()

        If (pAppCount > 0) Then
            '....Record already exists
            Dim pApp = (From App In pSealProcessDBEntities.tblApp
                                           Where App.fldProcessProjectID = ProjectID_In Select App).First()


            pApp.fldEqp = mEqp
            pApp.fldExistingSeal = mExistingSeal
            pApp.fldType = mType
            pApp.fldFluid = mFluid
            pApp.fldMaxLeak = mMaxLeak
            pApp.fldIsPressCyclic = mIsPressCyclic
            pApp.fldPressCycle_Freq = mPressCycle_Freq
            pApp.fldPressCycle_Amp = mPressCycle_Amp
            pApp.fldShaped = mShaped
            pApp.fldIsOoR = mIsOoR
            pApp.fldIsSplitRing = mIsSplitRing
            pApp.fldIsPreComp = mIsPreComp
            'pApp.fldUserName = mEditedBy.User.Name
            'pApp.fldSigned = mEditedBy.User.Signed
            'pApp.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pApp As New tblApp
            pApp.fldProcessProjectID = pID

            pApp.fldEqp = mEqp
            pApp.fldExistingSeal = mExistingSeal
            pApp.fldType = mType
            pApp.fldFluid = mFluid
            pApp.fldMaxLeak = mMaxLeak
            pApp.fldIsPressCyclic = mIsPressCyclic
            pApp.fldPressCycle_Freq = mPressCycle_Freq
            pApp.fldPressCycle_Amp = mPressCycle_Amp
            pApp.fldShaped = mShaped
            pApp.fldIsOoR = mIsOoR
            pApp.fldIsSplitRing = mIsSplitRing
            pApp.fldIsPreComp = mIsPreComp
            'pApp.fldUserName = mEditedBy.User.Name
            'pApp.fldSigned = mEditedBy.User.Signed
            'pApp.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblApp(pApp)
            pSealProcessDBEntities.SaveChanges()
        End If

        mOpCond.SaveToDB(ProjectID_In)
        mLoad.SaveToDB(ProjectID_In)
        mCavity.SaveToDB(ProjectID_In)
        mCavityFlange.SaveToDB(ProjectID_In)

        mFace.SaveToDB(ProjectID_In)
        mAxial.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "Class OpCond"

    <Serializable()> _
    Public Class clsOpCond

#Region "STRUCTURES:"
        <Serializable()> _
        Public Structure sT
            Public Assy As Double
            Public Min As Double
            Public Max As Double
            Public Oper As Double
        End Structure

        <Serializable()> _
        Public Structure sPress
            Public Assy As Double
            Public Min As Double
            Public Max As Double
            Public Oper As Double
        End Structure

#End Region

#Region "MEMBER VARIABLES:"

        Private mT As sT
        Private mPress As sPress

#End Region


#Region "PROPERTY ROUTINES:"

        '....T 
        Public ReadOnly Property T() As sT
            '==============================
            Get
                Return mT

            End Get

        End Property

        Public WriteOnly Property T_Assy() As Double
            '=======================================
            Set(value As Double)
                mT.Assy = value
            End Set
        End Property

        Public WriteOnly Property T_Min() As Double
            '=======================================
            Set(value As Double)
                mT.Min = value
            End Set
        End Property

        Public WriteOnly Property T_Max() As Double
            '=======================================
            Set(value As Double)
                mT.Max = value
            End Set
        End Property

        Public WriteOnly Property T_Oper() As Double
            '=======================================
            Set(value As Double)
                mT.Oper = value
            End Set
        End Property


        '....Press
        Public ReadOnly Property Press() As sPress
            '=====================================
            Get
                Return mPress
            End Get

        End Property


        Public WriteOnly Property Press_Assy() As Double
            '=======================================
            Set(value As Double)
                mPress.Assy = value
            End Set
        End Property

        Public WriteOnly Property Press_Min() As Double
            '=======================================
            Set(value As Double)
                mPress.Min = value
            End Set
        End Property

        Public WriteOnly Property Press_Max() As Double
            '=======================================
            Set(value As Double)
                mPress.Max = value
            End Set
        End Property

        Public WriteOnly Property Press_Oper() As Double
            '=======================================
            Set(value As Double)
                mPress.Oper = value
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblApp_OpCond
                Dim pQryOpCondCount As Integer = (From pRec In pSealProcessDBEntities.tblApp_OpCond
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()


                If (pQryOpCondCount > 0) Then

                    Dim pQryOpCond = (From pRec In pSealProcessDBEntities.tblApp_OpCond
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mT.Assy = pQryOpCond.fldTAssy
                    mT.Min = pQryOpCond.fldTMin
                    mT.Max = pQryOpCond.fldTMax
                    mT.Oper = pQryOpCond.fldTOper

                    mPress.Assy = pQryOpCond.fldPressAssy
                    mPress.Min = pQryOpCond.fldPressMin
                    mPress.Max = pQryOpCond.fldPressMax
                    mPress.Oper = pQryOpCond.fldPressOper
                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pOpCondCount = (From OpCond In pSealProcessDBEntities.tblApp_OpCond
                                                Where OpCond.fldProcessProjectID = ProjectID_In Select OpCond).ToList()


            If (pOpCondCount.Count > 0) Then
                '....Record already exists
                Dim pAppOpCond = (From OpCond In pSealProcessDBEntities.tblApp_OpCond
                            Where OpCond.fldProcessProjectID = ProjectID_In Select OpCond).First()

                With pAppOpCond
                    .fldTAssy = mT.Assy
                    .fldTMin = mT.Min
                    .fldTMax = mT.Max
                    .fldTOper = mT.Oper
                    .fldPressAssy = mPress.Assy
                    .fldPressMin = mPress.Min
                    .fldPressMax = mPress.Max
                    .fldPressOper = mPress.Oper
                End With

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pAppOpCond As New tblApp_OpCond

                With pAppOpCond
                    .fldProcessProjectID = pID
                    .fldTAssy = mT.Assy
                    .fldTMin = mT.Min
                    .fldTMax = mT.Max
                    .fldTOper = mT.Oper
                    .fldPressAssy = mPress.Assy
                    .fldPressMin = mPress.Min
                    .fldPressMax = mPress.Max
                    .fldPressOper = mPress.Oper
                End With

                pSealProcessDBEntities.AddTotblApp_OpCond(pAppOpCond)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class
#End Region

#Region "Class Load"

    <Serializable()> _
    Public Class clsLoad

#Region "STRUCTURES:"
        <Serializable()> _
        Public Structure sAssy
            Public Min As Double
            Public Max As Double
        End Structure

        <Serializable()> _
        Public Structure sOper
            Public Min As Double
            Public Max As Double
        End Structure

#End Region

#Region "MEMBER VARIABLES:"
        Private mAssy As sAssy
        Private mOper As sOper

#End Region


#Region "PROPERTY ROUTINES:"

        '....Assy 
        Public ReadOnly Property Assy() As sAssy
            '==============================
            Get
                Return mAssy

            End Get

        End Property


        Public WriteOnly Property Assy_Min() As Double
            '=======================================
            Set(value As Double)
                mAssy.Min = value
            End Set
        End Property


        Public WriteOnly Property Assy_Max() As Double
            '=======================================
            Set(value As Double)
                mAssy.Max = value
            End Set
        End Property

        '....Oper 
        Public ReadOnly Property Oper() As sOper
            '==============================
            Get
                Return mOper

            End Get

        End Property


        Public WriteOnly Property Oper_Min() As Double
            '=======================================
            Set(value As Double)
                mOper.Min = value
            End Set
        End Property


        Public WriteOnly Property Oper_Max() As Double
            '=======================================
            Set(value As Double)
                mOper.Max = value
            End Set
        End Property

#End Region


#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblApp_Load
                Dim pAppLoadCount As Integer = (From pRec In pSealProcessDBEntities.tblApp_Load
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pAppLoadCount > 0) Then

                    Dim pAppLoad = (From pRec In pSealProcessDBEntities.tblApp_Load
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()


                    mAssy.Min = pAppLoad.fldAssyMin
                    mAssy.Max = pAppLoad.fldAssyMax

                    mOper.Min = pAppLoad.fldOperMin
                    mOper.Max = pAppLoad.fldOperMax


                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()


            Dim pAppLoadCount = (From AppLoad In pSealProcessDBEntities.tblApp_Load
                                                Where AppLoad.fldProcessProjectID = ProjectID_In Select AppLoad).ToList()


            If (pAppLoadCount.Count > 0) Then
                '....Record already exists
                Dim pAppLoadRec = (From pRec In pSealProcessDBEntities.tblApp_Load
                            Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                With pAppLoadRec
                    .fldAssyMin = mAssy.Min
                    .fldAssyMax = mAssy.Max

                    .fldOperMin = mOper.Min
                    .fldOperMax = mOper.Max
                End With

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pAppLoad As New tblApp_Load

                With pAppLoad
                    .fldProcessProjectID = pID
                    .fldAssyMin = mAssy.Min
                    .fldAssyMax = mAssy.Max

                    .fldOperMin = mOper.Min
                    .fldOperMax = mOper.Max
                End With

                pSealProcessDBEntities.AddTotblApp_Load(pAppLoad)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class

#End Region

#Region "Class Cavity"

    <Serializable()> _
    Public Class clsCavity

        Private mID_Cavity As New List(Of Integer)
        Private mDimName As New List(Of String)

        Private mAssy As New List(Of sAssy)
        Private mOper As New List(Of sOper)


#Region "STRUCTURES:"

        <Serializable()> _
        Public Structure sAssy
            Public Min As Double
            Public Max As Double
        End Structure

        <Serializable()> _
        Public Structure sOper
            Public Min As Double
            Public Max As Double
        End Structure

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID
        Public Property ID_Cavity() As List(Of Integer)
            '=====================================
            Get
                Return mID_Cavity
            End Get

            Set(Obj As List(Of Integer))
                mID_Cavity = Obj
            End Set
        End Property

        '....DimName
        Public Property DimName() As List(Of String)
            '=======================================
            Get
                Return mDimName
            End Get

            Set(Obj As List(Of String))
                mDimName = Obj
            End Set
        End Property

        '....Assy 
        Public Property Assy() As List(Of sAssy)
            '==================================
            Get
                Return mAssy

            End Get

            Set(Obj As List(Of sAssy))
                mAssy = Obj
            End Set

        End Property

        '....Oper
        Public Property Oper() As List(Of sOper)
            '=====================================
            Get
                Return mOper
            End Get

            Set(Obj As List(Of sOper))
                mOper = Obj
            End Set
        End Property

#End Region


#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblAppFaceCavity
                Dim pAppFaceCavityCount As Integer = (From pRec In pSealProcessDBEntities.tblApp_Cavity
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pAppFaceCavityCount > 0) Then

                    Dim pAppFaceCavity = (From pRec In pSealProcessDBEntities.tblApp_Cavity
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pAppFaceCavity.Count - 1
                        mID_Cavity.Add(pAppFaceCavity(i).fldID)
                        mDimName.Add(pAppFaceCavity(i).fldDimName)

                        Dim pAssy As sAssy
                        pAssy.Min = pAppFaceCavity(i).fldAssyMin
                        pAssy.Max = pAppFaceCavity(i).fldAssyMax
                        Assy.Add(pAssy)

                        Dim pOper As sOper
                        pOper.Min = pAppFaceCavity(i).fldOperMin
                        pOper.Max = pAppFaceCavity(i).fldOperMax
                        Oper.Add(pOper)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()


            Dim pAppFaceCavity = (From AppFaceCavity In pSealProcessDBEntities.tblApp_Cavity
                                                Where AppFaceCavity.fldProcessProjectID = ProjectID_In Select AppFaceCavity).ToList()

            If (pAppFaceCavity.Count > 0) Then
                For j As Integer = 0 To pAppFaceCavity.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pAppFaceCavity(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pAppFace_Cavity As New List(Of tblApp_Cavity)

            For j As Integer = 0 To mID_Cavity.Count - 1

                Dim AppfaceCavity As New tblApp_Cavity
                pAppFace_Cavity.Add(AppfaceCavity)
                With pAppFace_Cavity(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDimName = mDimName(j)
                    .fldAssyMin = mAssy(j).Min
                    .fldAssyMax = mAssy(j).Max
                    .fldOperMin = mOper(j).Min
                    .fldOperMax = mOper(j).Max
                End With

                pSealProcessDBEntities.AddTotblApp_Cavity(pAppFace_Cavity(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#Region "Class CavityFlange"

    <Serializable()> _
    Public Class clsCavityFlange

#Region "MEMBER VARIABLES:"

        Private mMat1 As String
        Private mMat2 As String
        Private mHard1 As Double
        Private mHard2 As Double
        Private mSF1 As Double
        Private mSF2 As Double
        Private mMeasureSF As String
        Private mUnitSF As String

#End Region


#Region "PROPERTY ROUTINES:"

        '....Mat1
        Public Property Mat1() As String
            '================================
            Get
                Return mMat1
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mMat1 = strData
            End Set

        End Property

        '....Mat2
        Public Property Mat2() As String
            '================================
            Get
                Return mMat2
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mMat2 = strData
            End Set

        End Property

        '....Hard1
        Public Property Hard1() As Double
            '==========================================
            Get
                Return mHard1
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mHard1 = dblData
            End Set

        End Property

        '....Hard2
        Public Property Hard2() As Double
            '==========================================
            Get
                Return mHard2
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mHard2 = dblData
            End Set

        End Property


        '....SF1
        Public Property SF1() As Double
            '==========================================
            Get
                Return mSF1
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mSF1 = dblData
            End Set

        End Property


        '....SF2
        Public Property SF2() As Double
            '==========================================
            Get
                Return mSF2
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mSF2 = dblData
            End Set

        End Property

        '....MeasureSF
        Public Property MeasureSF() As String
            '================================
            Get
                Return mMeasureSF
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mMeasureSF = strData
            End Set

        End Property


        '....UnitSF
        Public Property UnitSF() As String
            '================================
            Get
                Return mUnitSF
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mUnitSF = strData
            End Set

        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblApp_CavityFlange
                Dim pQryCavityFlangeCount As Integer = (From pRec In pSealProcessDBEntities.tblApp_CavityFlange
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()


                If (pQryCavityFlangeCount > 0) Then

                    Dim pQryCavityFlange = (From pRec In pSealProcessDBEntities.tblApp_CavityFlange
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mMat1 = pQryCavityFlange.fldMat1
                    mMat2 = pQryCavityFlange.fldMat2
                    mHard1 = pQryCavityFlange.fldHard1
                    mHard2 = pQryCavityFlange.fldHard2
                    mSF1 = pQryCavityFlange.fldSF1
                    mSF2 = pQryCavityFlange.fldSF2
                    mMeasureSF = pQryCavityFlange.fldMeasureSF
                    mUnitSF = pQryCavityFlange.fldUnitSF
                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pCavityFlangeCount = (From pRec In pSealProcessDBEntities.tblApp_CavityFlange
                                                Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()


            If (pCavityFlangeCount.Count > 0) Then
                '....Record already exists
                Dim pCavityFlange = (From OpCond In pSealProcessDBEntities.tblApp_CavityFlange
                            Where OpCond.fldProcessProjectID = ProjectID_In Select OpCond).First()

                With pCavityFlange
                    .fldMat1 = mMat1
                    .fldMat2 = mMat2
                    .fldHard1 = mHard1
                    .fldHard2 = mHard2
                    .fldSF1 = mSF1
                    .fldSF2 = mSF2
                    .fldMeasureSF = mMeasureSF
                    .fldUnitSF = mUnitSF
                End With

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pAppCavityFlange As New tblApp_CavityFlange

                With pAppCavityFlange
                    .fldProcessProjectID = pID
                    .fldMat1 = mMat1
                    .fldMat2 = mMat2
                    .fldHard1 = mHard1
                    .fldHard2 = mHard2
                    .fldSF1 = mSF1
                    .fldSF2 = mSF2
                    .fldMeasureSF = mMeasureSF
                    .fldUnitSF = mUnitSF
                End With

                pSealProcessDBEntities.AddTotblApp_CavityFlange(pAppCavityFlange)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class

#End Region

#Region "Class Face"

    <Serializable()> _
    Public Class clsFace

#Region "MEMBER VARIABLES:"
        Private mPOrient As String
        Private mMaxFlangeSep As Double
#End Region

#Region "PROPERTY ROUTINES:"

        '....POrient
        Public Property POrient() As String
            '================================
            Get
                Return mPOrient
            End Get

            Set(ByVal strData As String)
                '-------------------------------
                mPOrient = strData
            End Set

        End Property

        '....MaxFlangeSep
        Public Property MaxFlangeSep() As Double
            '===================================
            Get
                Return mMaxFlangeSep
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mMaxFlangeSep = dblData
            End Set

        End Property

#End Region

#Region "DB RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '=====================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try

                '....tblAppFace
                Dim pAppFaceCount As Integer = (From pRec In pSealProcessDBEntities.tblApp_Face
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pAppFaceCount > 0) Then

                    Dim pQryAppFace = (From pRec In pSealProcessDBEntities.tblApp_Face
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mPOrient = pQryAppFace.fldPOrient
                    mMaxFlangeSep = pQryAppFace.fldMaxFlangeSep

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            '....AppFace table
            Dim pAppFaceCount As Integer = (From AppFace In pSealProcessDBEntities.tblApp_Face
                                                Where AppFace.fldProcessProjectID = ProjectID_In Select AppFace).Count()

            If (pAppFaceCount > 0) Then
                '....Record already exists
                Dim pAppFace = (From AppFace In pSealProcessDBEntities.tblApp_Face
                                               Where AppFace.fldProcessProjectID = ProjectID_In Select AppFace).First()


                pAppFace.fldPOrient = mPOrient
                pAppFace.fldMaxFlangeSep = mMaxFlangeSep

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pAppFace As New tblApp_Face
                pAppFace.fldProcessProjectID = pID
                pAppFace.fldPOrient = mPOrient
                pAppFace.fldMaxFlangeSep = mMaxFlangeSep

                pSealProcessDBEntities.AddTotblApp_Face(pAppFace)
                pSealProcessDBEntities.SaveChanges()
            End If


        End Sub

#End Region


    End Class

#End Region

#Region "Class Axial"

    <Serializable()> _
    Public Class clsAxial

#Region "MEMBER VARIABLES:"

        Private mIsStatic As Boolean
        Private mIsRotating As Boolean
        Private mRPM As Double
        Private mIsRecip As Boolean
        Private mRecip_Stroke As Double
        Private mRecip_V As Double
        Private mRecip_CycleRate As Double
        Private mRecip_ServiceLife As Double
        Private mIsOscilatory As Boolean
        Private mOscilate_Rot As Double
        Private mOscilate_V As Double
        Private mOscilate_CycleRate As Double
        Private mOscilate_ServiceLife As Double

#End Region

#Region "PROPERTY ROUTINES:"

        '....IsStatic
        Public Property IsStatic() As Boolean
            '================================
            Get
                Return mIsStatic
            End Get

            Set(ByVal blnData As Boolean)
                '-------------------------------
                mIsStatic = blnData
            End Set

        End Property

        '....IsRotating
        Public Property IsRotating() As Boolean
            '===================================
            Get
                Return mIsRotating
            End Get

            Set(ByVal blnData As Boolean)
                '-------------------------------
                mIsRotating = blnData
            End Set

        End Property

        '....RPM
        Public Property RPM() As Double
            '=========================
            Get
                Return mRPM
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mRPM = dblData
            End Set

        End Property

        '....IsRecip
        Public Property IsRecip() As Boolean
            '===============================
            Get
                Return mIsRecip
            End Get

            Set(ByVal blnData As Boolean)
                '-------------------------------
                mIsRecip = blnData
            End Set

        End Property

        '....Recip_Stroke
        Public Property Recip_Stroke() As Double
            '===================================
            Get
                Return mRecip_Stroke
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mRecip_Stroke = dblData
            End Set

        End Property


        '....Recip_V
        Public Property Recip_V() As Double
            '================================
            Get
                Return mRecip_V
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mRecip_V = dblData
            End Set

        End Property


        '....Recip_CycleRate
        Public Property Recip_CycleRate() As Double
            '======================================
            Get
                Return mRecip_CycleRate
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mRecip_CycleRate = dblData
            End Set

        End Property

        '....Recip_ServiceLife
        Public Property Recip_ServiceLife() As Double
            '========================================
            Get
                Return mRecip_ServiceLife
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mRecip_ServiceLife = dblData
            End Set

        End Property


        '....IsOscilatory
        Public Property IsOscilatory() As Boolean
            '====================================
            Get
                Return mIsOscilatory
            End Get

            Set(ByVal blnData As Boolean)
                '-------------------------------
                mIsOscilatory = blnData
            End Set

        End Property

        '....Oscilate_Rot
        Public Property Oscilate_Rot() As Double
            '================================
            Get
                Return mOscilate_Rot
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mOscilate_Rot = dblData
            End Set

        End Property


        '....Oscilate_V
        Public Property Oscilate_V() As Double
            '================================
            Get
                Return mOscilate_V
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mOscilate_V = dblData
            End Set

        End Property


        '....Oscilate_CycleRate
        Public Property Oscilate_CycleRate() As Double
            '==========================================
            Get
                Return mOscilate_CycleRate
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mOscilate_CycleRate = dblData
            End Set

        End Property

        '....Oscilate_ServiceLife
        Public Property Oscilate_ServiceLife() As Double
            '===========================================
            Get
                Return mOscilate_ServiceLife
            End Get

            Set(ByVal dblData As Double)
                '-------------------------------
                mOscilate_ServiceLife = dblData
            End Set

        End Property

#End Region

#Region "DB RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '=====================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try

                '....tblAppAxial
                Dim pAppAxialCount As Integer = (From pRec In pSealProcessDBEntities.tblApp_Axial
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pAppAxialCount > 0) Then

                    Dim pQryAppAxial = (From pRec In pSealProcessDBEntities.tblApp_Axial
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                    mIsStatic = pQryAppAxial.fldIsStatic
                    mIsRotating = pQryAppAxial.fldIsRotating
                    mRPM = pQryAppAxial.fldRPM
                    mIsRecip = pQryAppAxial.fldIsRecip
                    mRecip_Stroke = pQryAppAxial.fldRecip_Stroke
                    mRecip_V = pQryAppAxial.fldRecip_V
                    mRecip_CycleRate = pQryAppAxial.fldRecip_CycleRate
                    mRecip_ServiceLife = pQryAppAxial.fldRecip_ServiceLife
                    mIsOscilatory = pQryAppAxial.fldIsOscilatory
                    mOscilate_Rot = pQryAppAxial.fldOscilate_Rot
                    mOscilate_V = pQryAppAxial.fldOscilate_V
                    mOscilate_CycleRate = pQryAppAxial.fldOscilate_CycleRate
                    mOscilate_ServiceLife = pQryAppAxial.fldOscilate_ServiceLife

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            '....AppAxial table
            Dim pAppAxialCount As Integer = (From AppAxial In pSealProcessDBEntities.tblApp_Axial
                                                Where AppAxial.fldProcessProjectID = ProjectID_In Select AppAxial).Count()

            If (pAppAxialCount > 0) Then
                '....Record already exists
                Dim pAppAxial = (From AppFace In pSealProcessDBEntities.tblApp_Axial
                                               Where AppFace.fldProcessProjectID = ProjectID_In Select AppFace).First()


                pAppAxial.fldIsStatic = mIsStatic
                pAppAxial.fldIsRotating = mIsRotating
                pAppAxial.fldRPM = mRPM
                pAppAxial.fldIsRecip = mIsRecip
                pAppAxial.fldRecip_Stroke = mRecip_Stroke
                pAppAxial.fldRecip_V = mRecip_V
                pAppAxial.fldRecip_CycleRate = mRecip_CycleRate
                pAppAxial.fldRecip_ServiceLife = mRecip_ServiceLife

                pAppAxial.fldIsOscilatory = mIsOscilatory
                pAppAxial.fldOscilate_Rot = mOscilate_Rot
                pAppAxial.fldOscilate_V = mOscilate_V
                pAppAxial.fldOscilate_CycleRate = mOscilate_CycleRate
                pAppAxial.fldOscilate_ServiceLife = mOscilate_ServiceLife

                pSealProcessDBEntities.SaveChanges()

            Else
                '....New Record
                Dim pID As Integer = ProjectID_In

                Dim pAppAxial As New tblApp_Axial
                pAppAxial.fldProcessProjectID = pID
                pAppAxial.fldIsStatic = mIsStatic
                pAppAxial.fldIsRotating = mIsRotating
                pAppAxial.fldRPM = mRPM
                pAppAxial.fldIsRecip = mIsRecip
                pAppAxial.fldRecip_Stroke = mRecip_Stroke
                pAppAxial.fldRecip_V = mRecip_V
                pAppAxial.fldRecip_CycleRate = mRecip_CycleRate
                pAppAxial.fldRecip_ServiceLife = mRecip_ServiceLife

                pAppAxial.fldIsOscilatory = mIsOscilatory
                pAppAxial.fldOscilate_Rot = mOscilate_Rot
                pAppAxial.fldOscilate_V = mOscilate_V
                pAppAxial.fldOscilate_CycleRate = mOscilate_CycleRate
                pAppAxial.fldOscilate_ServiceLife = mOscilate_ServiceLife

                pSealProcessDBEntities.AddTotblApp_Axial(pAppAxial)
                pSealProcessDBEntities.SaveChanges()
            End If

        End Sub

#End Region

    End Class

#End Region


#End Region

End Class
