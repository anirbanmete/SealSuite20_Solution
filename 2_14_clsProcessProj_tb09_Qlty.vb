'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsQlty                        '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_Qlty

#Region "MEMBER VARIABLES:"

    Private mIsApvdSupplierOnly As Boolean
    Private mSeparate_Tool_Gage_Reqd As Boolean
    Private mHasCustComplaint As Boolean
    Private mReason As String
    Private mVisualInspection As Boolean
    Private mVisualInspection_Type As String
    Private mCustAcceptStd As String
    Private mSPC_Reqd As Boolean
    Private mGageRnR_Reqd As Boolean

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....IsApvdSupplierOnly
    Public Property IsApvdSupplierOnly() As Boolean
        '=========================================
        Get
            Return mIsApvdSupplierOnly
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsApvdSupplierOnly = blnData
        End Set

    End Property

    '....Separate_Tool_Gage_Reqd
    Public Property Separate_Tool_Gage_Reqd() As Boolean
        '===============================================
        Get
            Return mSeparate_Tool_Gage_Reqd
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mSeparate_Tool_Gage_Reqd = blnData
        End Set

    End Property

    '....HasCustComplaint
    Public Property HasCustComplaint() As Boolean
        '========================================
        Get
            Return mHasCustComplaint
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mHasCustComplaint = blnData
        End Set

    End Property

    '....Reason
    Public Property Reason() As String
        '=================================
        Get
            Return mReason
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mReason = strData
        End Set

    End Property

    '....VisualInspection
    Public Property VisualInspection() As Boolean
        '=========================================
        Get
            Return mVisualInspection
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mVisualInspection = blnData
        End Set

    End Property

    '....VisualInspection_Type
    Public Property VisualInspection_Type() As String
        '=============================================
        Get
            Return mVisualInspection_Type
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mVisualInspection_Type = strData
        End Set

    End Property

    '....CustAcceptStd
    Public Property CustAcceptStd() As String
        '=====================================
        Get
            Return mCustAcceptStd
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mCustAcceptStd = strData
        End Set

    End Property

    '....SPC_Reqd
    Public Property SPC_Reqd() As Boolean
        '=============================
        Get
            Return mSPC_Reqd
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mSPC_Reqd = blnData
        End Set

    End Property

    '....GageRnR_Reqd
    Public Property GageRnR_Reqd() As Boolean
        '=====================================
        Get
            Return mGageRnR_Reqd
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mGageRnR_Reqd = blnData
        End Set

    End Property

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

#Region "DATABASE RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '=====================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try

            '....tblQlty
            Dim pQryQltyCount As Integer = (From pRec In pSealProcessDBEntities.tblQlty
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryQltyCount > 0) Then

                Dim pQryQlty = (From pRec In pSealProcessDBEntities.tblQlty
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mIsApvdSupplierOnly = pQryQlty.fldIsApvdSupplierOnly
                mSeparate_Tool_Gage_Reqd = pQryQlty.fldSeparate_Tool_Gage_Reqd
                mHasCustComplaint = pQryQlty.fldHasCustComplaint
                mReason = pQryQlty.fldReason
                mVisualInspection = pQryQlty.fldIsVisualInspection
                mVisualInspection_Type = pQryQlty.fldVisualInspection_Type
                mCustAcceptStd = pQryQlty.fldCustAcceptStd
                mSPC_Reqd = pQryQlty.fldSPC_Reqd
                mGageRnR_Reqd = pQryQlty.fldGageRnR_Reqd
                mEditedBy.User_Name = pQryQlty.fldUserName
                mEditedBy.User_Signed = pQryQlty.fldSigned

                If (Not IsNothing(pQryQlty.fldDateSigned) And Not IsDBNull(pQryQlty.fldDateSigned)) Then
                    mEditedBy.User_DateSigned = pQryQlty.fldDateSigned
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblQlty
        Dim pQltyCount As Integer = (From Cost In pSealProcessDBEntities.tblQlty
                                            Where Cost.fldProcessProjectID = ProjectID_In Select Cost).Count()

        If (pQltyCount > 0) Then
            '....Record already exists
            Dim pQlty = (From Qlty In pSealProcessDBEntities.tblQlty
                                           Where Qlty.fldProcessProjectID = ProjectID_In Select Qlty).First()

            pQlty.fldIsApvdSupplierOnly = mIsApvdSupplierOnly
            pQlty.fldSeparate_Tool_Gage_Reqd = mSeparate_Tool_Gage_Reqd
            pQlty.fldHasCustComplaint = mHasCustComplaint
            pQlty.fldReason = mReason
            pQlty.fldIsVisualInspection = mVisualInspection
            pQlty.fldVisualInspection_Type = mVisualInspection_Type
            pQlty.fldCustAcceptStd = mCustAcceptStd
            pQlty.fldSPC_Reqd = mSPC_Reqd
            pQlty.fldGageRnR_Reqd = mGageRnR_Reqd
            pQlty.fldUserName = mEditedBy.User.Name
            pQlty.fldSigned = mEditedBy.User.Signed
            pQlty.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pQlty As New tblQlty
            pQlty.fldProcessProjectID = pID

            pQlty.fldIsApvdSupplierOnly = mIsApvdSupplierOnly
            pQlty.fldSeparate_Tool_Gage_Reqd = mSeparate_Tool_Gage_Reqd
            pQlty.fldHasCustComplaint = mHasCustComplaint
            pQlty.fldReason = mReason
            pQlty.fldIsVisualInspection = mVisualInspection
            pQlty.fldVisualInspection_Type = mVisualInspection_Type
            pQlty.fldCustAcceptStd = mCustAcceptStd
            pQlty.fldSPC_Reqd = mSPC_Reqd
            pQlty.fldGageRnR_Reqd = mGageRnR_Reqd
            pQlty.fldUserName = mEditedBy.User.Name
            pQlty.fldSigned = mEditedBy.User.Signed
            pQlty.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblQlty(pQlty)
            pSealProcessDBEntities.SaveChanges()

        End If

    End Sub

#End Region


End Class
