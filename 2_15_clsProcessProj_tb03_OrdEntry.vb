'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsOrdEntry                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_OrdEntry

#Region "MEMBER VARIABLES:"

    Private mSalesOrderNo As String
    Private mDateSales As Date
    Private mLeadTimeQuoted As Double
    Private mPONo As String
    Private mDatePO As Date
    Private mDatePO_EDI As Date
    Private mHasSplReq As Boolean
    Private mTool_Reqd As Boolean
    Private mSplPkg_Lbl_Reqd As Boolean
    Private mOrdQty As Integer
    Private mDateOrdShip As Date
    Private mExpedited As Boolean
    Private mIsDFAR As Boolean

#End Region

#Region "Member Class Objects:"

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....SalesOrderNo
    Public Property SalesOrderNo() As String
        '===================================
        Get
            Return mSalesOrderNo
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mSalesOrderNo = strData
        End Set

    End Property

    '....DateSales
    Public Property DateSales() As Date
        '==============================
        Get
            Return mDateSales
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDateSales = strData
        End Set

    End Property

    '....Lead Time Quoted 
    Public Property LeadTimeQuoted() As Double
        '=====================================
        Get
            Return mLeadTimeQuoted
        End Get

        Set(ByVal strData As Double)
            '-------------------------------
            mLeadTimeQuoted = strData
        End Set

    End Property

    '....PONo
    Public Property PONo() As String
        '===================================
        Get
            Return mPONo
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mPONo = strData
        End Set

    End Property

    '....DatePO
    Public Property DatePO() As Date
        '===========================
        Get
            Return mDatePO
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDatePO = strData
        End Set

    End Property

    '....DatePO_EDI
    Public Property DatePO_EDI() As Date
        '===============================
        Get
            Return mDatePO_EDI
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDatePO_EDI = strData
        End Set

    End Property

    '....HasSplReq
    Public Property HasSplReq() As Boolean
        '=================================
        Get
            Return mHasSplReq
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mHasSplReq = blnData
        End Set

    End Property

    '....Tool_Reqd
    Public Property Tool_Reqd() As Boolean
        '=================================
        Get
            Return mTool_Reqd
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mTool_Reqd = blnData
        End Set

    End Property

    '....SplPkg_Lbl_Reqd
    Public Property SplPkg_Lbl_Reqd() As Boolean
        '=======================================
        Get
            Return mSplPkg_Lbl_Reqd
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mSplPkg_Lbl_Reqd = blnData
        End Set

    End Property

    '....OrdQty
    Public Property OrdQty() As Integer
        '==============================
        Get
            Return mOrdQty
        End Get

        Set(ByVal intData As Integer)
            '-------------------------------
            mOrdQty = intData
        End Set

    End Property


    '....DateOrdShip
    Public Property DateOrdShip() As Date
        '================================
        Get
            Return mDateOrdShip
        End Get

        Set(ByVal strData As Date)
            '-------------------------------
            mDateOrdShip = strData
        End Set

    End Property

    '....Expedited
    Public Property Expedited() As Boolean
        '==================================
        Get
            Return mExpedited
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mExpedited = blnData
        End Set

    End Property

    '....IsDFAR
    Public Property IsDFAR() As Boolean
        '=============================
        Get
            Return mIsDFAR
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsDFAR = blnData
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

            '....tblOrdEntry
            Dim pQryOrdEntryCount As Integer = (From pRec In pSealProcessDBEntities.tblOrdEntry
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryOrdEntryCount > 0) Then

                Dim pQryOrdEntry = (From pRec In pSealProcessDBEntities.tblOrdEntry
                                    Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mSalesOrderNo = pQryOrdEntry.fldSalesOrderNo

                If (Not IsNothing(pQryOrdEntry.fldDateSales) And Not IsDBNull(pQryOrdEntry.fldDateSales)) Then
                    mDateSales = pQryOrdEntry.fldDateSales
                End If

                If (Not IsNothing(pQryOrdEntry.fldLeadTimeQuoted) And Not IsDBNull(pQryOrdEntry.fldLeadTimeQuoted)) Then
                    mLeadTimeQuoted = pQryOrdEntry.fldLeadTimeQuoted
                Else
                    mLeadTimeQuoted = 0
                End If

                mPONo = pQryOrdEntry.fldPONo

                If (Not IsNothing(pQryOrdEntry.fldDatePO) And Not IsDBNull(pQryOrdEntry.fldDatePO)) Then
                    mDatePO = pQryOrdEntry.fldDatePO
                End If


                If (Not IsNothing(pQryOrdEntry.fldDatePO_EDI) And Not IsDBNull(pQryOrdEntry.fldDatePO_EDI)) Then
                    mDatePO_EDI = pQryOrdEntry.fldDatePO_EDI
                End If

                mHasSplReq = pQryOrdEntry.fldHasSplReq
                mTool_Reqd = pQryOrdEntry.fldTool_Reqd
                mSplPkg_Lbl_Reqd = pQryOrdEntry.fldSplPkg_Lbl_Reqd

                If (Not IsNothing(pQryOrdEntry.fldOrdQty) And Not IsDBNull(pQryOrdEntry.fldOrdQty)) Then
                    mOrdQty = pQryOrdEntry.fldOrdQty
                End If


                If (Not IsNothing(pQryOrdEntry.fldDateOrdShip) And Not IsDBNull(pQryOrdEntry.fldDateOrdShip)) Then
                    mDateOrdShip = pQryOrdEntry.fldDateOrdShip
                End If

                mExpedited = pQryOrdEntry.fldExpedited
                mIsDFAR = pQryOrdEntry.fldIsDFAR

                'mEditedBy.User_Name = pQryOrdEntry.fldUserName
                'mEditedBy.User_Signed = pQryOrdEntry.fldSigned

                'If (Not IsNothing(pQryOrdEntry.fldDateSigned) And Not IsDBNull(pQryOrdEntry.fldDateSigned)) Then
                '    mEditedBy.User_DateSigned = pQryOrdEntry.fldDateSigned
                'End If

            Else

                mSalesOrderNo = ""

                mDateSales = DateTime.MinValue
                mLeadTimeQuoted = 0

                mPONo = ""
                mDatePO = DateTime.MinValue
                mDatePO_EDI = DateTime.MinValue

                mHasSplReq = False
                mTool_Reqd = False
                mSplPkg_Lbl_Reqd = False
                mOrdQty = 0
                mDateOrdShip = DateTime.MinValue

                mExpedited = False
                mIsDFAR = False

            End If
            'mCustContact.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblOrdEntry
        Dim pOrdEntryCount As Integer = (From OrdEntry In pSealProcessDBEntities.tblOrdEntry
                                            Where OrdEntry.fldProcessProjectID = ProjectID_In Select OrdEntry).Count()

        If (pOrdEntryCount > 0) Then
            '....Record already exists
            Dim pOrdEntry = (From OrdEntry In pSealProcessDBEntities.tblOrdEntry
                                           Where OrdEntry.fldProcessProjectID = ProjectID_In Select OrdEntry).First()


            pOrdEntry.fldSalesOrderNo = mSalesOrderNo
            pOrdEntry.fldDateSales = mDateSales
            pOrdEntry.fldLeadTimeQuoted = mLeadTimeQuoted
            pOrdEntry.fldPONo = mPONo
            pOrdEntry.fldDatePO = mDatePO
            pOrdEntry.fldDatePO_EDI = mDatePO_EDI
            pOrdEntry.fldHasSplReq = mHasSplReq
            pOrdEntry.fldTool_Reqd = mTool_Reqd
            pOrdEntry.fldSplPkg_Lbl_Reqd = mSplPkg_Lbl_Reqd
            pOrdEntry.fldOrdQty = mOrdQty
            pOrdEntry.fldDateOrdShip = mDateOrdShip
            pOrdEntry.fldExpedited = mExpedited
            pOrdEntry.fldIsDFAR = mIsDFAR

            'pOrdEntry.fldUserName = mEditedBy.User.Name
            'pOrdEntry.fldSigned = mEditedBy.User.Signed
            'pOrdEntry.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pOrdEntry As New tblOrdEntry
            pOrdEntry.fldProcessProjectID = pID
            pOrdEntry.fldSalesOrderNo = mSalesOrderNo
            pOrdEntry.fldDateSales = mDateSales
            pOrdEntry.fldLeadTimeQuoted = mLeadTimeQuoted
            pOrdEntry.fldPONo = mPONo
            pOrdEntry.fldDatePO = mDatePO
            pOrdEntry.fldDatePO_EDI = mDatePO_EDI
            pOrdEntry.fldHasSplReq = mHasSplReq
            pOrdEntry.fldTool_Reqd = mTool_Reqd
            pOrdEntry.fldSplPkg_Lbl_Reqd = mSplPkg_Lbl_Reqd
            pOrdEntry.fldOrdQty = mOrdQty
            pOrdEntry.fldDateOrdShip = mDateOrdShip
            pOrdEntry.fldExpedited = mExpedited
            pOrdEntry.fldIsDFAR = mIsDFAR

            'pOrdEntry.fldUserName = mEditedBy.User.Name
            'pOrdEntry.fldSigned = mEditedBy.User.Signed
            'pOrdEntry.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblOrdEntry(pOrdEntry)
            pSealProcessDBEntities.SaveChanges()
        End If
        'mCustContact.SaveToDB(ProjectID_In)
    End Sub

#End Region

End Class
