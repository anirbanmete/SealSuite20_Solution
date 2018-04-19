
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessPreOrder                     '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  27DEC17                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_PreOrder

#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sMgr
        Public Mkt As String
        Public Sales As String
    End Structure

    <Serializable()> _
    Public Structure sExport
        Public Reqd As Boolean
        Public Status As String
    End Structure

    <Serializable()> _
    Public Structure sPart
        Public Family As String
        Public Type As String
    End Structure

    <Serializable()> _
    Public Structure sMkt
        Public Seg As String
        Public Channel As String
    End Structure

    <Serializable()> _
    Public Structure sLoc
        Public CostFile As String
        Public RFQPkg As String
    End Structure

#End Region

#Region "MEMBER VARIABLES:"

    Private mMgr As sMgr
    Private mExport As sExport
    Private mPart As sPart
    Private mMkt As sMkt
    Private mLoc As sLoc
    Private mNotes As String
    Private mNotes_Price As String

#Region "Member Class Objects:"

    Private mQuote As New clsQuote
    Private mSalesData As New clsSalesData

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region


#End Region

#Region "PROPERTY ROUTINES:"

    '....Mgr
    Public ReadOnly Property Mgr() As sMgr
        '=================================     
        Get
            Return mMgr
        End Get
    End Property

    '....Export
    Public ReadOnly Property Export() As sExport
        '========================================     
        Get
            Return mExport
        End Get
    End Property

    '....Part
    Public ReadOnly Property Part() As sPart
        '====================================     
        Get
            Return mPart
        End Get
    End Property

    '....Mkt
    Public ReadOnly Property Mkt() As sMkt
        '=================================   
        Get
            Return mMkt
        End Get
    End Property

    '....Loc
    Public ReadOnly Property Loc() As sLoc
        '==================================     
        Get
            Return mLoc
        End Get
    End Property

    '....Notes
    Public Property Notes() As String
        '=============================
        Get
            Return mNotes
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mNotes = strData
        End Set

    End Property

    '....Notes Price
    Public Property Notes_Price() As String
        '=============================
        Get
            Return mNotes_Price
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mNotes_Price = strData
        End Set

    End Property


    '....Mgr_PreOrder
    Public WriteOnly Property Mgr_PreOrder() As String
        '============================================== 
        Set(ByVal strVal As String)
            mMgr.Mkt = strVal
        End Set
    End Property


    '....Mgr_Sales
    Public WriteOnly Property Mgr_Sales() As String
        '============================================
        Set(ByVal strVal As String)
            mMgr.Sales = strVal
        End Set
    End Property


    '....Export_Reqd
    Public WriteOnly Property Export_Reqd() As Boolean
        '============================================== 
        Set(ByVal blnVal As Boolean)
            mExport.Reqd = blnVal
        End Set
    End Property


    '....Export_Status
    Public WriteOnly Property Export_Status() As String
        '===============================================
        Set(ByVal blnVal As String)
            mExport.Status = blnVal
        End Set
    End Property

    '....Part_Family
    Public WriteOnly Property Part_Family() As String
        '============================================== 
        Set(ByVal strVal As String)
            mPart.Family = strVal
        End Set
    End Property

    '....Mgr_PreOrder
    Public WriteOnly Property Part_Type() As String
        '===========================================
        Set(ByVal strVal As String)
            mPart.Type = strVal
        End Set
    End Property

    '....PreOrder_Seg
    Public WriteOnly Property PreOrder_Seg() As String
        '============================================== 
        Set(ByVal strVal As String)
            mMkt.Seg = strVal
        End Set
    End Property

    '....PreOrder_Channel
    Public WriteOnly Property PreOrder_Channel() As String
        '===================================================
        Set(ByVal strVal As String)
            mMkt.Channel = strVal
        End Set
    End Property


    '....Loc_CostFile
    Public WriteOnly Property Loc_CostFile() As String
        '============================================== 
        Set(ByVal strVal As String)
            mLoc.CostFile = strVal
        End Set
    End Property

    '....Loc_RFQPkg
    Public WriteOnly Property Loc_RFQPkg() As String
        '============================================
        Set(ByVal strVal As String)
            mLoc.RFQPkg = strVal
        End Set
    End Property

#Region "Quote"

    Public Property Quote() As clsQuote
        '===============================
        Get
            Return mQuote
        End Get

        Set(ByVal strObj As clsQuote)
            '-------------------------------
            mQuote = strObj
        End Set

    End Property

#End Region

#Region "SalesData"

    Public Property SalesData() As clsSalesData
        '=======================================
        Get
            Return mSalesData
        End Get

        Set(ByVal strObj As clsSalesData)
            '-------------------------------
            mSalesData = strObj
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
            '....tblPreOrder
            Dim pQryPreOrderCount As Integer = (From pRec In pSealProcessDBEntities.tblPreOrder
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryPreOrderCount > 0) Then

                Dim pQryPreOrder = (From pRec In pSealProcessDBEntities.tblPreOrder
                                    Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mMgr.Mkt = pQryPreOrder.fldMgrMkt
                mMgr.Sales = pQryPreOrder.fldMgrSales

                mExport.Reqd = pQryPreOrder.fldExportReqd
                mExport.Status = pQryPreOrder.fldExportStatus

                mPart.Family = pQryPreOrder.fldPartFamily
                mPart.Type = pQryPreOrder.fldPartType


                mMkt.Seg = pQryPreOrder.fldMktSeg
                mMkt.Channel = pQryPreOrder.fldMktChannel

                mLoc.CostFile = pQryPreOrder.fldLocCostFile
                mLoc.RFQPkg = pQryPreOrder.fldLocRFQPkg

                mNotes = pQryPreOrder.fldNotes
                mNotes_Price = pQryPreOrder.fldNotesPrice

                ''mEditedBy.User_Name = pQryPreOrder.fldUserName
                ''mEditedBy.User_Signed = pQryPreOrder.fldSigned

                ''If (Not IsNothing(pQryPreOrder.fldDateSigned) And Not IsDBNull(pQryPreOrder.fldDateSigned)) Then
                ''    mEditedBy.User_DateSigned = pQryPreOrder.fldDateSigned
                ''End If
            Else

                mMgr.Mkt = ""
                mMgr.Sales = ""

                mExport.Reqd = False
                mExport.Status = ""

                mPart.Family = ""
                mPart.Type = ""

                mMkt.Seg = ""
                mMkt.Channel = ""

                mLoc.CostFile = ""
                mLoc.RFQPkg = ""

                mNotes = ""
                mNotes_Price = ""

            End If

            mQuote.RetrieveFromDB(ProjectID_In)
            mSalesData.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....PreOrder table
        Dim pPreOrderCount As Integer = (From ProcessPreOrder In pSealProcessDBEntities.tblPreOrder
                                            Where ProcessPreOrder.fldProcessProjectID = ProjectID_In Select ProcessPreOrder).Count()

        If (pPreOrderCount > 0) Then
            '....Record already exists
            Dim pPreOrder = (From PreOrder In pSealProcessDBEntities.tblPreOrder
                                           Where PreOrder.fldProcessProjectID = ProjectID_In Select PreOrder).First()


            pPreOrder.fldMgrMkt = mMgr.Mkt
            pPreOrder.fldMgrSales = mMgr.Sales
            pPreOrder.fldExportReqd = mExport.Reqd
            pPreOrder.fldExportStatus = mExport.Status
            pPreOrder.fldPartFamily = mPart.Family
            pPreOrder.fldPartType = mPart.Type
            pPreOrder.fldMktSeg = mMkt.Seg
            pPreOrder.fldMktChannel = mMkt.Channel
            pPreOrder.fldLocCostFile = mLoc.CostFile
            pPreOrder.fldLocRFQPkg = mLoc.RFQPkg
            pPreOrder.fldNotes = mNotes
            pPreOrder.fldNotesPrice = mNotes_Price

            'pPreOrder.fldUserName = mEditedBy.User.Name ' mUser.Name
            'pPreOrder.fldSigned = mEditedBy.User.Signed
            'pPreOrder.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In
            Dim pPreOrder As New tblPreOrder
            pPreOrder.fldProcessProjectID = pID
            pPreOrder.fldMgrMkt = mMgr.Mkt
            pPreOrder.fldMgrSales = mMgr.Sales
            pPreOrder.fldExportReqd = mExport.Reqd
            pPreOrder.fldExportStatus = mExport.Status
            pPreOrder.fldPartFamily = mPart.Family
            pPreOrder.fldPartType = mPart.Type
            pPreOrder.fldMktSeg = mMkt.Seg
            pPreOrder.fldMktChannel = mMkt.Channel
            pPreOrder.fldLocCostFile = mLoc.CostFile
            pPreOrder.fldLocRFQPkg = mLoc.RFQPkg
            pPreOrder.fldNotes = mNotes
            pPreOrder.fldNotesPrice = mNotes_Price
            'pPreOrder.fldUserName = mEditedBy.User.Name ' mUser.Name
            'pPreOrder.fldSigned = mEditedBy.User.Signed
            'pPreOrder.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblPreOrder(pPreOrder)
            pSealProcessDBEntities.SaveChanges()
        End If

        mQuote.SaveToDB(ProjectID_In)
        mSalesData.SaveToDB(ProjectID_In)
    End Sub

#End Region


#Region "NESTED CLASS:"

#Region "Class Quote"
    <Serializable()> _
    Public Class clsQuote

#Region "MEMBER VARIABLES:"

        Private mQID As New List(Of Integer)
        Private mQDate As New List(Of Date)
        Private mNo As New List(Of String)

#End Region


#Region "PROPERTY ROUTINES:"

        '....QID
        Public Property QID() As List(Of Integer)
            '=====================================
            Get
                Return mQID
            End Get

            Set(Obj As List(Of Integer))
                mQID = Obj
            End Set
        End Property

        '....QDate
        Public Property QDate() As List(Of Date)
            '=======================================
            Get
                Return mQDate
            End Get

            Set(Obj As List(Of Date))
                mQDate = Obj
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

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblPreOrder_Quote
                Dim pQryQuoteCount As Integer = (From pRec In pSealProcessDBEntities.tblPreOrder_Quote
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryQuoteCount > 0) Then

                    Dim pQryQuote = (From pRec In pSealProcessDBEntities.tblPreOrder_Quote
                                     Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryQuote.Count - 1
                        mQID.Add(pQryQuote(i).fldID)
                        mQDate.Add(pQryQuote(i).fldDate)
                        mNo.Add(pQryQuote(i).fldNo)

                    Next

                Else
                    mQID.Clear()
                    mQDate.Clear()
                    mNo.Clear()

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pQuote As New tblPreOrder_Quote

            Dim pQryQuote = (From PreOrderQuote In pSealProcessDBEntities.tblPreOrder_Quote
                                                Where PreOrderQuote.fldProcessProjectID = ProjectID_In Select PreOrderQuote).ToList()

            If (pQryQuote.Count > 0) Then
                For j As Integer = 0 To pQryQuote.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryQuote(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pPreOrderQuote As New List(Of tblPreOrder_Quote)

            For j As Integer = 0 To mQID.Count - 1
                Dim PreOrderQuote As New tblPreOrder_Quote
                pPreOrderQuote.Add(PreOrderQuote)
                With pPreOrderQuote(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDate = mQDate(j)
                    .fldNo = mNo(j)

                End With

                pSealProcessDBEntities.AddTotblPreOrder_Quote(pPreOrderQuote(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#Region "Class SalesData"
    <Serializable()> _
    Public Class clsSalesData

#Region "MEMBER VARIABLES:"

        Private mID_Sales As New List(Of Integer)
        Private mYear As New List(Of Integer)
        Private mQty As New List(Of Integer)
        Private mPrice As New List(Of Double)
        Private mTotal As New List(Of Double)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID
        Public Property ID_Sales() As List(Of Integer)
            '=====================================
            Get
                Return mID_Sales
            End Get

            Set(Obj As List(Of Integer))
                mID_Sales = Obj
            End Set
        End Property

        '....Year
        Public Property Year() As List(Of Integer)
            '=======================================
            Get
                Return mYear
            End Get

            Set(Obj As List(Of Integer))
                mYear = Obj
            End Set
        End Property

        '....Qty
        Public Property Qty() As List(Of Integer)
            '=====================================
            Get
                Return mQty
            End Get

            Set(Obj As List(Of Integer))
                mQty = Obj
            End Set
        End Property

        '....Price
        Public Property Price() As List(Of Double)
            '=====================================
            Get
                Return mPrice
            End Get

            Set(Obj As List(Of Double))
                mPrice = Obj
            End Set
        End Property

        '....Total
        Public Property Total() As List(Of Double)
            '=====================================
            Get
                Return mTotal
            End Get

            Set(Obj As List(Of Double))
                mTotal = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblPreOrder_SalesData
                Dim pQrySalesCount As Integer = (From pRec In pSealProcessDBEntities.tblPreOrder_SalesData
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQrySalesCount > 0) Then

                    Dim pQrySalesData = (From pRec In pSealProcessDBEntities.tblPreOrder_SalesData
                                         Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQrySalesData.Count - 1
                        mID_Sales.Add(pQrySalesData(i).fldID)
                        mYear.Add(pQrySalesData(i).fldYear)
                        mQty.Add(pQrySalesData(i).fldQty)
                        mPrice.Add(pQrySalesData(i).fldPrice)
                        mTotal.Add(pQrySalesData(i).fldTotal)
                    Next

                Else
                    mID_Sales.Clear()
                    mYear.Clear()
                    mQty.Clear()
                    mPrice.Clear()
                    mTotal.Clear()
                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pSalesData As New tblPreOrder_SalesData

            Dim pQrySalesData = (From PreOrderSales In pSealProcessDBEntities.tblPreOrder_SalesData
                                                Where PreOrderSales.fldProcessProjectID = ProjectID_In Select PreOrderSales).ToList()

            If (pQrySalesData.Count > 0) Then
                For j As Integer = 0 To pQrySalesData.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQrySalesData(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pPreOrderSales As New List(Of tblPreOrder_SalesData)

            For j As Integer = 0 To mID_Sales.Count - 1
                Dim PreOrderSales As New tblPreOrder_SalesData
                pPreOrderSales.Add(PreOrderSales)
                With pPreOrderSales(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldYear = mYear(j)
                    .fldQty = mQty(j)
                    .fldPrice = mPrice(j)
                    .fldTotal = mTotal(j)

                End With

                pSealProcessDBEntities.AddTotblPreOrder_SalesData(pPreOrderSales(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class
#End Region

#End Region

End Class
