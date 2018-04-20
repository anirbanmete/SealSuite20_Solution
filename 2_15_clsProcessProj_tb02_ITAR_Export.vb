
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsITAR_Export                 '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================
<Serializable()> _
Public Class clsProcessProj_ITAR_Export

#Region "MEMBER VARIABLES:"

    Private mIsCustOnDenialList As Boolean
    Private mCountryProhibited As Boolean
    Private mHasAntiBoycottLang As Boolean
    Private mIsUnder_ITAR_Reg As Boolean
    Private mSaleExportControlled As Boolean
    Private mITAR_Class As String
    Private mEAR_Class As String
    Private mStatus As String
    Private mHTS_Class As String

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....IsCustOnDenialList
    Public Property IsCustOnDenialList() As Boolean
        '==========================================
        Get
            Return mIsCustOnDenialList
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsCustOnDenialList = blnData
        End Set

    End Property

    '....CountryProhibited
    Public Property CountryProhibited() As Boolean
        '========================================
        Get
            Return mCountryProhibited
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mCountryProhibited = blnData
        End Set

    End Property

    '....HasAntiBoycottLang
    Public Property HasAntiBoycottLang() As Boolean
        '========================================
        Get
            Return mHasAntiBoycottLang
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mHasAntiBoycottLang = blnData
        End Set

    End Property

    '....IsUnder_ITAR_Reg
    Public Property IsUnder_ITAR_Reg() As Boolean
        '========================================
        Get
            Return mIsUnder_ITAR_Reg
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsUnder_ITAR_Reg = blnData
        End Set

    End Property

    '....ITAR_Class
    Public Property ITAR_Class() As String
        '========================================
        Get
            Return mITAR_Class
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mITAR_Class = strData
        End Set

    End Property

    '....SaleExportControlled
    Public Property SaleExportControlled() As Boolean
        '============================================
        Get
            Return mSaleExportControlled
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mSaleExportControlled = blnData
        End Set

    End Property

    '....EAR_Class
    Public Property EAR_Class() As String
        '================================
        Get
            Return mEAR_Class
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mEAR_Class = strData
        End Set

    End Property

    '....Status
    Public Property Status() As String
        '==============================
        Get
            Return mStatus
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mStatus = strData
        End Set

    End Property

    '....HTS_Class
    Public Property HTS_Class() As String
        '========================================
        Get
            Return mHTS_Class
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mHTS_Class = strData
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

#Region "DATABASE RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '=====================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try

            '....tblITAR_Export
            Dim pQryITAR_ExportCount As Integer = (From pRec In pSealProcessDBEntities.tblITAR_Export
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryITAR_ExportCount > 0) Then

                Dim pQryITAR_Export = (From pRec In pSealProcessDBEntities.tblITAR_Export
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mIsCustOnDenialList = pQryITAR_Export.fldIsCustOnDenialList
                mCountryProhibited = pQryITAR_Export.fldCountryProhibited
                mHasAntiBoycottLang = pQryITAR_Export.fldHasAntiBoycottLang
                mIsUnder_ITAR_Reg = pQryITAR_Export.fldIsUnder_ITAR_Reg
                mITAR_Class = pQryITAR_Export.fldITAR_Class
                mSaleExportControlled = pQryITAR_Export.fldSaleExportControlled
                mEAR_Class = pQryITAR_Export.fldEAR_Class
                mStatus = pQryITAR_Export.fldStatus
                mHTS_Class = pQryITAR_Export.fldHTS_Class

                ''mEditedBy.User_Name = pQryITAR_Export.fldUserName
                ''mEditedBy.User_Signed = pQryITAR_Export.fldSigned

                ''If (Not IsNothing(pQryITAR_Export.fldDateSigned) And Not IsDBNull(pQryITAR_Export.fldDateSigned)) Then
                ''    mEditedBy.User_DateSigned = pQryITAR_Export.fldDateSigned
                ''End If
            Else
                mIsCustOnDenialList = False
                mCountryProhibited = False
                mHasAntiBoycottLang = False
                mIsUnder_ITAR_Reg = False
                mITAR_Class = ""
                mSaleExportControlled = False
                mEAR_Class = ""
                mStatus = ""
                mHTS_Class = ""

            End If

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblITAR_Export
        Dim pITAR_ExportCount As Integer = (From ITAR_Export In pSealProcessDBEntities.tblITAR_Export
                                            Where ITAR_Export.fldProcessProjectID = ProjectID_In Select ITAR_Export).Count()

        If (pITAR_ExportCount > 0) Then
            '....Record already exists
            Dim pITAR_Export = (From ITAR_Export In pSealProcessDBEntities.tblITAR_Export
                                           Where ITAR_Export.fldProcessProjectID = ProjectID_In Select ITAR_Export).First()


            pITAR_Export.fldIsCustOnDenialList = mIsCustOnDenialList
            pITAR_Export.fldCountryProhibited = mCountryProhibited
            pITAR_Export.fldHasAntiBoycottLang = mHasAntiBoycottLang
            pITAR_Export.fldIsUnder_ITAR_Reg = mIsUnder_ITAR_Reg
            pITAR_Export.fldITAR_Class = mITAR_Class
            pITAR_Export.fldSaleExportControlled = mSaleExportControlled
            pITAR_Export.fldEAR_Class = mEAR_Class
            pITAR_Export.fldStatus = mStatus
            pITAR_Export.fldHTS_Class = mHTS_Class

            ''pITAR_Export.fldUserName = mEditedBy.User.Name
            ''pITAR_Export.fldSigned = mEditedBy.User.Signed
            ''pITAR_Export.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pITAR_Export As New tblITAR_Export
            pITAR_Export.fldProcessProjectID = pID
            pITAR_Export.fldIsCustOnDenialList = mIsCustOnDenialList
            pITAR_Export.fldCountryProhibited = mCountryProhibited
            pITAR_Export.fldHasAntiBoycottLang = mHasAntiBoycottLang
            pITAR_Export.fldIsUnder_ITAR_Reg = mIsUnder_ITAR_Reg
            pITAR_Export.fldITAR_Class = mITAR_Class
            pITAR_Export.fldSaleExportControlled = mSaleExportControlled
            pITAR_Export.fldEAR_Class = mEAR_Class
            pITAR_Export.fldStatus = mStatus
            pITAR_Export.fldHTS_Class = mHTS_Class

            ''pITAR_Export.fldUserName = mEditedBy.User.Name
            ''pITAR_Export.fldSigned = mEditedBy.User.Signed
            ''pITAR_Export.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblITAR_Export(pITAR_Export)
            pSealProcessDBEntities.SaveChanges()
        End If

    End Sub

#End Region


#End Region


End Class
