'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsShipping                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29DEC17                                '
'                                                                              '
'===============================================================================
Imports System.Globalization
Imports System.IO.FileSystemWatcher
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Linq
Imports System.Reflection
Imports System.IO
Imports System.Threading

<Serializable()> _
Public Class clsProcessProj_Shipping

#Region "MEMBER VARIABLES:"

    Private mNotes As String

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....Notes
    Public Property Notes() As String
        '============================
        Get
            Return mNotes
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mNotes = strData
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

            '....tblShipping
            Dim pQryShippingCount As Integer = (From pRec In pSealProcessDBEntities.tblShipping
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryShippingCount > 0) Then

                Dim pQryShipping = (From pRec In pSealProcessDBEntities.tblShipping
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mNotes = pQryShipping.fldNotes
                mEditedBy.User_Name = pQryShipping.fldUserName
                mEditedBy.User_Signed = pQryShipping.fldSigned

                If (Not IsNothing(pQryShipping.fldDateSigned) And Not IsDBNull(pQryShipping.fldDateSigned)) Then
                    mEditedBy.User_DateSigned = pQryShipping.fldDateSigned
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblShipping
        Dim pShippingCount As Integer = (From Cost In pSealProcessDBEntities.tblShipping
                                            Where Cost.fldProcessProjectID = ProjectID_In Select Cost).Count()

        If (pShippingCount > 0) Then
            '....Record already exists
            Dim pShipping = (From Shipping In pSealProcessDBEntities.tblShipping
                                           Where Shipping.fldProcessProjectID = ProjectID_In Select Shipping).First()

            pShipping.fldNotes = mNotes
            pShipping.fldUserName = mEditedBy.User.Name
            pShipping.fldSigned = mEditedBy.User.Signed
            pShipping.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pShipping As New tblShipping
            pShipping.fldProcessProjectID = pID
            pShipping.fldNotes = mNotes
            pShipping.fldUserName = mEditedBy.User.Name
            pShipping.fldSigned = mEditedBy.User.Signed
            pShipping.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblShipping(pShipping)
            pSealProcessDBEntities.SaveChanges()
        End If

    End Sub

#End Region


End Class
