'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsPurchase                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05JAN18                                '
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
Imports System.Windows.Forms

<Serializable()> _
Public Class clsProcessProj_Purchase

#Region "MEMBER VARIABLES:"

    Private mMat As New clsMat
    Private mDwg As New clsDwg

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

#Region "Mat"

    Public Property Mat() As clsMat
        '==========================
        Get
            Return mMat
        End Get

		Set(ByVal clsObj As clsMat)
			'----------------------
			mMat = clsObj
		End Set

	End Property

#End Region

#Region "DWG"

	Public Property Dwg() As clsDwg
        '===========================
        Get
            Return mDwg
        End Get

        Set(ByVal strObj As clsDwg)
            '-------------------------------
            mDwg = strObj
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

            '....tblPurchase
            Dim pQryPurchaseCount As Integer = (From pRec In pSealProcessDBEntities.tblPurchase
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryPurchaseCount > 0) Then

                Dim pQryPurchase = (From pRec In pSealProcessDBEntities.tblPurchase
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mEditedBy.User_Name = pQryPurchase.fldUserName
                mEditedBy.User_Signed = pQryPurchase.fldSigned

                If (Not IsNothing(pQryPurchase.fldDateSigned) And Not IsDBNull(pQryPurchase.fldDateSigned)) Then
                    mEditedBy.User_DateSigned = pQryPurchase.fldDateSigned
                End If

            End If

            mMat.RetrieveFromDB(ProjectID_In)
            mDwg.RetrieveFromDB(ProjectID_In)
        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....Purchase table
        Dim pPurchaseCount As Integer = (From Manf In pSealProcessDBEntities.tblPurchase
                                            Where Manf.fldProcessProjectID = ProjectID_In Select Manf).Count()

        If (pPurchaseCount > 0) Then
            '....Record already exists
            Dim pPurchase = (From Manf In pSealProcessDBEntities.tblPurchase
                                           Where Manf.fldProcessProjectID = ProjectID_In Select Manf).First()

            pPurchase.fldUserName = mEditedBy.User.Name
            pPurchase.fldSigned = mEditedBy.User.Signed
            pPurchase.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pPurchase As New tblPurchase
            pPurchase.fldProcessProjectID = pID

            pPurchase.fldUserName = mEditedBy.User.Name
            pPurchase.fldSigned = mEditedBy.User.Signed
            pPurchase.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblPurchase(pPurchase)
            pSealProcessDBEntities.SaveChanges()
        End If

        mMat.SaveToDB(ProjectID_In)
        mDwg.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "Class clsMat"

    <Serializable()> _
    Public Class clsMat

#Region "MEMBER VARIABLES:"

        Private mID_Mat As New List(Of Integer)
        Private mItem As New List(Of String)
        Private mEstQty As New List(Of Double)
        Private mQty_Unit As New List(Of String)
        Private mStatus As New List(Of String)
        Private mLeadTime As New List(Of Double)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_Mat
        Public Property ID_Mat() As List(Of Integer)
            '==========================================
            Get
                Return mID_Mat
            End Get

            Set(Obj As List(Of Integer))
                mID_Mat = Obj
            End Set
        End Property

        '....Item
        Public Property Item() As List(Of String)
            '=====================================
            Get
                Return mItem
            End Get

            Set(Obj As List(Of String))
                mItem = Obj
            End Set
        End Property

        '....EstQty
        Public Property EstQty() As List(Of Double)
            '=====================================
            Get
                Return mEstQty
            End Get

            Set(Obj As List(Of Double))
                mEstQty = Obj
            End Set
        End Property

        '....Qty_Unit
        Public Property Qty_Unit() As List(Of String)
            '========================================
            Get
                Return mQty_Unit
            End Get

            Set(Obj As List(Of String))
                mQty_Unit = Obj
            End Set
        End Property

        '....Status
        Public Property Status() As List(Of String)
            '=====================================
            Get
                Return mStatus
            End Get

            Set(Obj As List(Of String))
                mStatus = Obj
            End Set
        End Property


        '....LeadTime
        Public Property LeadTime() As List(Of Double)
            '=====================================
            Get
                Return mLeadTime
            End Get

            Set(Obj As List(Of Double))
                mLeadTime = Obj
            End Set
        End Property


#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblPurchase_Mat
                Dim pQryPurchaseMatCount As Integer = (From pRec In pSealProcessDBEntities.tblPurchase_Mat
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryPurchaseMatCount > 0) Then

                    Dim pQryMat = (From pRec In pSealProcessDBEntities.tblPurchase_Mat
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryMat.Count - 1
                        mID_Mat.Add(pQryMat(i).fldID)
                        mItem.Add(pQryMat(i).fldItem)
                        mEstQty.Add(pQryMat(i).fldEstQty)
                        mStatus.Add(pQryMat(i).fldStatus)
                        mLeadTime.Add(pQryMat(i).fldLeadTime)

                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()


            Dim pQryPurchaseMat = (From Mat In pSealProcessDBEntities.tblPurchase_Mat
                                                Where Mat.fldProcessProjectID = ProjectID_In Select Mat).ToList()

            If (pQryPurchaseMat.Count > 0) Then
                For j As Integer = 0 To pQryPurchaseMat.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryPurchaseMat(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pPurchaseMat As New List(Of tblPurchase_Mat)

            For j As Integer = 0 To mID_Mat.Count - 1
                Dim pMat As New tblPurchase_Mat
                pPurchaseMat.Add(pMat)
                With pPurchaseMat(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldItem = mItem(j)
                    .fldEstQty = mEstQty(j)
                    .fldStatus = mStatus(j)
                    .fldLeadTime = mLeadTime(j)

                End With

                pSealProcessDBEntities.AddTotblPurchase_Mat(pPurchaseMat(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region


    End Class


#End Region

#Region "Class clsDwg"

    <Serializable()> _
    Public Class clsDwg

#Region "MEMBER VARIABLES:"

        Private mID_Dwg As New List(Of Integer)
        Private mNo As New List(Of String)
        Private mDesc As New List(Of String)
        Private mLeadTime As New List(Of Double)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_Dwg
        Public Property ID_Dwg() As List(Of Integer)
            '==========================================
            Get
                Return mID_Dwg
            End Get

            Set(Obj As List(Of Integer))
                mID_Dwg = Obj
            End Set
        End Property

        '....No
        Public Property No() As List(Of String)
            '=====================================
            Get
                Return mNo
            End Get

            Set(Obj As List(Of String))
                mNo = Obj
            End Set
        End Property

        '....Desc
        Public Property Desc() As List(Of String)
            '=====================================
            Get
                Return mDesc
            End Get

            Set(Obj As List(Of String))
                mDesc = Obj
            End Set
        End Property


        '....LeadTime
        Public Property LeadTime() As List(Of Double)
            '=====================================
            Get
                Return mLeadTime
            End Get

            Set(Obj As List(Of Double))
                mLeadTime = Obj
            End Set
        End Property


#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblPurchase_Dwg
                Dim pQryPurchaseDwgCount As Integer = (From pRec In pSealProcessDBEntities.tblPurchase_Dwg
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryPurchaseDwgCount > 0) Then

                    Dim pQryPurchaseDwg = (From pRec In pSealProcessDBEntities.tblPurchase_Dwg
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryPurchaseDwg.Count - 1
                        mID_Dwg.Add(pQryPurchaseDwg(i).fldID)
                        mNo.Add(pQryPurchaseDwg(i).fldDwgNo)
                        mDesc.Add(pQryPurchaseDwg(i).fldDesc)
                        mLeadTime.Add(pQryPurchaseDwg(i).fldLeadTime)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()


            Dim pQryPurchaseDwg = (From Dwg In pSealProcessDBEntities.tblPurchase_Dwg
                                                Where Dwg.fldProcessProjectID = ProjectID_In Select Dwg).ToList()

            If (pQryPurchaseDwg.Count > 0) Then
                For j As Integer = 0 To pQryPurchaseDwg.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryPurchaseDwg(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pPurchaseDwg As New List(Of tblPurchase_Dwg)

            For j As Integer = 0 To mID_Dwg.Count - 1
                Dim pPurchaseDrawing As New tblPurchase_Dwg
                pPurchaseDwg.Add(pPurchaseDrawing)
                With pPurchaseDwg(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDwgNo = mNo(j)
                    .fldDesc = mDesc(j)
                    .fldLeadTime = mLeadTime(j)
                End With

                pSealProcessDBEntities.AddTotblPurchase_Dwg(pPurchaseDwg(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#End Region

End Class
