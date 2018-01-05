'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_Cost                    '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28DEC17                                '
'                                                                              '
'===============================================================================
<Serializable()> _
Public Class clsProcessProj_Cost

#Region "MEMBER VARIABLES:"

    Private mLocQuoteFile As String
    Private mNotes As String

#Region "Member Class Objects:"
    Private mSplOperation As New clsSplOperation
    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#End Region

#Region "PROPERTY ROUTINES:"

    '....QuoteFileLoc
    Public Property QuoteFileLoc() As String
        '====================================
        Get
            Return mLocQuoteFile
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mLocQuoteFile = strData
        End Set

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


#Region "SplCharges"

    Public Property SplOperation() As clsSplOperation
        '=========================================
        Get
            Return mSplOperation
        End Get

        Set(ByVal strObj As clsSplOperation)
            '-------------------------------
            mSplOperation = strObj
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

#Region "DATABASE RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '=====================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try

            '....tblCost
            Dim pQryCostCount As Integer = (From pRec In pSealProcessDBEntities.tblCost
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryCostCount > 0) Then

                Dim pQryCost = (From pRec In pSealProcessDBEntities.tblCost
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mLocQuoteFile = pQryCost.fldLocQuoteFile
                mNotes = pQryCost.fldNotes
                mEditedBy.User_Name = pQryCost.fldUserName
                mEditedBy.User_Signed = pQryCost.fldSigned

                If (Not IsNothing(pQryCost.fldDateSigned) And Not IsDBNull(pQryCost.fldDateSigned)) Then
                    mEditedBy.User_DateSigned = pQryCost.fldDateSigned
                End If

            End If

            mSplOperation.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblCost
        Dim pCostCount As Integer = (From Cost In pSealProcessDBEntities.tblCost
                                            Where Cost.fldProcessProjectID = ProjectID_In Select Cost).Count()

        If (pCostCount > 0) Then
            '....Record already exists
            Dim pCost = (From Cost In pSealProcessDBEntities.tblCost
                                           Where Cost.fldProcessProjectID = ProjectID_In Select Cost).First()

            pCost.fldLocQuoteFile = mLocQuoteFile
            pCost.fldNotes = mNotes
            pCost.fldUserName = mEditedBy.User.Name
            pCost.fldSigned = mEditedBy.User.Signed
            pCost.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pCost As New tblCost
            pCost.fldProcessProjectID = pID
            pCost.fldLocQuoteFile = mLocQuoteFile
            pCost.fldNotes = mNotes
            pCost.fldUserName = mEditedBy.User.Name
            pCost.fldSigned = mEditedBy.User.Signed
            pCost.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblCost(pCost)
            pSealProcessDBEntities.SaveChanges()
        End If

        mSplOperation.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "Class SplOperation"
    <Serializable()> _
    Public Class clsSplOperation

#Region "MEMBER VARIABLES:"

        Private mID_SplOp As New List(Of Integer)
        Private mDesc As New List(Of String)
        Private mSpec As New List(Of String)
        Private mLeadTime As New List(Of Double)
        Private mCost As New List(Of Double)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_SplOp
        Public Property ID_SplOp() As List(Of Integer)
            '========================================
            Get
                Return mID_SplOp
            End Get

            Set(Obj As List(Of Integer))
                mID_SplOp = Obj
            End Set
        End Property

        '....Desc 
        Public Property Desc() As List(Of String)
            '=======================================
            Get
                Return mDesc
            End Get

            Set(Obj As List(Of String))
                mDesc = Obj
            End Set
        End Property

        '....Spec 
        Public Property Spec() As List(Of String)
            '=======================================
            Get
                Return mSpec
            End Get

            Set(Obj As List(Of String))
                mSpec = Obj
            End Set
        End Property

        '....LeadTime 
        Public Property LeadTime() As List(Of Double)
            '=======================================
            Get
                Return mLeadTime
            End Get

            Set(Obj As List(Of Double))
                mLeadTime = Obj
            End Set
        End Property

        '....Cost
        Public Property Cost() As List(Of Double)
            '=====================================
            Get
                Return mCost
            End Get

            Set(Obj As List(Of Double))
                mCost = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblCost
                Dim pQryCostCount As Integer = (From pRec In pSealProcessDBEntities.tblCost_SplOperation
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryCostCount > 0) Then

                    Dim pQryCostSplOperation = (From pRec In pSealProcessDBEntities.tblCost_SplOperation
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryCostSplOperation.Count - 1
                        mID_SplOp.Add(pQryCostSplOperation(i).fldID)
                        mDesc.Add(pQryCostSplOperation(i).fldDesc)
                        mSpec.Add(pQryCostSplOperation(i).fldSpec)
                        mLeadTime.Add(pQryCostSplOperation(i).fldLeadTime)
                        mCost.Add(pQryCostSplOperation(i).fldCost)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            'Dim pCost_SplOperation As New tblCost_SplOperation

            Dim pQryCost_SplOperation = (From Cost_Spl In pSealProcessDBEntities.tblCost_SplOperation
                                                Where Cost_Spl.fldProcessProjectID = ProjectID_In Select Cost_Spl).ToList()

            If (pQryCost_SplOperation.Count > 0) Then
                For j As Integer = 0 To pQryCost_SplOperation.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryCost_SplOperation(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pCost_SplOperation As New List(Of tblCost_SplOperation)

            For j As Integer = 0 To mID_SplOp.Count - 1
                Dim pCost_Spl As New tblCost_SplOperation
                pCost_SplOperation.Add(pCost_Spl)
                With pCost_SplOperation(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDesc = mDesc(j)
                    .fldSpec = mSpec(j)
                    .fldLeadTime = mLeadTime(j)
                    .fldCost = mCost(j)

                End With

                pSealProcessDBEntities.AddTotblCost_SplOperation(pCost_SplOperation(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#End Region

End Class
