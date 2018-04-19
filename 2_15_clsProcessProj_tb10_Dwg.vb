'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsDwg                         '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29DEC17                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_Dwg

#Region "MEMBER VARIABLES:"

    Private mDesignLevel As String

    Private mNeeded As New clsNeeded
    Private mBOM As New clsBOM

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....DesignLevel
    Public Property DesignLevel() As String
        '===================================
        Get
            Return mDesignLevel
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mDesignLevel = strData
        End Set

    End Property

#Region "Needed"

    Public Property Needed() As clsNeeded
        '=================================
        Get
            Return mNeeded
        End Get

        Set(ByVal strObj As clsNeeded)
            '-------------------------------
            mNeeded = strObj
        End Set

    End Property

#End Region

#Region "BOM"

    Public Property BOM() As clsBOM
        '===========================
        Get
            Return mBOM
        End Get

        Set(ByVal strObj As clsBOM)
            '-------------------------------
            mBOM = strObj
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

            '....tblDwg
            Dim pQryDwgCount As Integer = (From pRec In pSealProcessDBEntities.tblDwg
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryDwgCount > 0) Then

                Dim pQryDwg = (From pRec In pSealProcessDBEntities.tblDwg
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mDesignLevel = pQryDwg.fldDesignLevel
                'mEditedBy.User_Name = pQryDwg.fldUserName
                'mEditedBy.User_Signed = pQryDwg.fldSigned

                'If (Not IsNothing(pQryDwg.fldDateSigned) And Not IsDBNull(pQryDwg.fldDateSigned)) Then
                '    mEditedBy.User_DateSigned = pQryDwg.fldDateSigned
                'End If

            Else
                mDesignLevel = ""

            End If

            mNeeded.RetrieveFromDB(ProjectID_In)
            mBOM.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblDwg
        Dim pDwgCount As Integer = (From Cost In pSealProcessDBEntities.tblDwg
                                            Where Cost.fldProcessProjectID = ProjectID_In Select Cost).Count()

        If (pDwgCount > 0) Then
            '....Record already exists
            Dim pDwg = (From Dwg In pSealProcessDBEntities.tblDwg
                                           Where Dwg.fldProcessProjectID = ProjectID_In Select Dwg).First()

            pDwg.fldDesignLevel = mDesignLevel
            'pDwg.fldUserName = mEditedBy.User.Name
            'pDwg.fldSigned = mEditedBy.User.Signed
            'pDwg.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pDwg As New tblDwg
            pDwg.fldProcessProjectID = pID
            pDwg.fldDesignLevel = mDesignLevel
            'pDwg.fldUserName = mEditedBy.User.Name
            'pDwg.fldSigned = mEditedBy.User.Signed
            'pDwg.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblDwg(pDwg)
            pSealProcessDBEntities.SaveChanges()
        End If

        mNeeded.SaveToDB(ProjectID_In)
        mBOM.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "clsNeeded:"

    <Serializable()> _
    Public Class clsNeeded

#Region "MEMBER VARIABLES:"

        Private mID_Needed As New List(Of Integer)
        Private mDwgNo As New List(Of String)
        Private mDesc As New List(Of String)
        Private mStatus As New List(Of String)
        Private mLeadTime As New List(Of Double)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_Needed
        Public Property ID_Needed() As List(Of Integer)
            '========================================
            Get
                Return mID_Needed
            End Get

            Set(Obj As List(Of Integer))
                mID_Needed = Obj
            End Set
        End Property


        '....DwgNo 
        Public Property DwgNo() As List(Of String)
            '=======================================
            Get
                Return mDwgNo
            End Get

            Set(Obj As List(Of String))
                mDwgNo = Obj
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

        '....Status 
        Public Property Status() As List(Of String)
            '=======================================
            Get
                Return mStatus
            End Get

            Set(Obj As List(Of String))
                mStatus = Obj
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

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblDwgNeeded
                Dim pQryNeededCount As Integer = (From pRec In pSealProcessDBEntities.tblDwg_Needed
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryNeededCount > 0) Then

                    Dim pQryDwgNeeded = (From pRec In pSealProcessDBEntities.tblDwg_Needed
                                         Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryDwgNeeded.Count - 1
                        mID_Needed.Add(pQryDwgNeeded(i).fldID)
                        mDwgNo.Add(pQryDwgNeeded(i).fldDwgNo)
                        mDesc.Add(pQryDwgNeeded(i).fldDesc)
                        mStatus.Add(pQryDwgNeeded(i).fldStatus)
                        mLeadTime.Add(pQryDwgNeeded(i).fldLeadTime)
                    Next

                Else
                    mID_Needed.Clear()
                    mDwgNo.Clear()
                    mDesc.Clear()
                    mStatus.Clear()
                    mLeadTime.Clear()

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pQryDwgNeeded = (From DwgNeeded In pSealProcessDBEntities.tblDwg_Needed
                                                Where DwgNeeded.fldProcessProjectID = ProjectID_In Select DwgNeeded).ToList()

            If (pQryDwgNeeded.Count > 0) Then
                For j As Integer = 0 To pQryDwgNeeded.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryDwgNeeded(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pDwgNeeded As New List(Of tblDwg_Needed)

            For j As Integer = 0 To mID_Needed.Count - 1
                Dim pDwg_Needed As New tblDwg_Needed
                pDwgNeeded.Add(pDwg_Needed)
                With pDwgNeeded(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDwgNo = mDwgNo(j)
                    .fldDesc = mDesc(j)
                    .fldStatus = mStatus(j)
                    .fldLeadTime = mLeadTime(j)

                End With

                pSealProcessDBEntities.AddTotblDwg_Needed(pDwgNeeded(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region


    End Class

#End Region

#Region "clsBOM:"

    <Serializable()> _
    Public Class clsBOM

#Region "MEMBER VARIABLES:"

        Private mID_BOM As New List(Of Integer)
        Private mParent_PartNo As New List(Of String)
        Private mChild_PartNo As New List(Of String)
        Private mStatus As New List(Of String)
        Private mQty As New List(Of Integer)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_BOM
        Public Property ID_BOM() As List(Of Integer)
            '========================================
            Get
                Return mID_BOM
            End Get

            Set(Obj As List(Of Integer))
                mID_BOM = Obj
            End Set
        End Property


        '....Parent_PartNo 
        Public Property Parent_PartNo() As List(Of String)
            '===========================================
            Get
                Return mParent_PartNo
            End Get

            Set(Obj As List(Of String))
                mParent_PartNo = Obj
            End Set
        End Property

        '....Child_PartNo 
        Public Property Child_PartNo() As List(Of String)
            '===========================================
            Get
                Return mChild_PartNo
            End Get

            Set(Obj As List(Of String))
                mChild_PartNo = Obj
            End Set
        End Property


        '....Qty 
        Public Property Qty() As List(Of Integer)
            '=======================================
            Get
                Return mQty
            End Get

            Set(Obj As List(Of Integer))
                mQty = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblDwg_Bom
                Dim pQryBOMCount As Integer = (From pRec In pSealProcessDBEntities.tblDwg_BOM
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryBOMCount > 0) Then

                    Dim pQryBOM = (From pRec In pSealProcessDBEntities.tblDwg_BOM
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryBOM.Count - 1
                        mID_BOM.Add(pQryBOM(i).fldID)
                        mParent_PartNo.Add(pQryBOM(i).fldParent_PartNo)
                        mChild_PartNo.Add(pQryBOM(i).fldChild_PartNo)
                        mQty.Add(pQryBOM(i).fldQty)
                    Next

                Else
                    mID_BOM.Clear()
                    mParent_PartNo.Clear()
                    mChild_PartNo.Clear()
                    mQty.Clear()

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Dim pQryDwgBOM = (From DwgBOM In pSealProcessDBEntities.tblDwg_BOM
                                                Where DwgBOM.fldProcessProjectID = ProjectID_In Select DwgBOM).ToList()

            If (pQryDwgBOM.Count > 0) Then
                For j As Integer = 0 To pQryDwgBOM.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryDwgBOM(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pDwgBOM As New List(Of tblDwg_BOM)

            For j As Integer = 0 To mID_BOM.Count - 1
                Dim pDwg_BOM As New tblDwg_BOM
                pDwgBOM.Add(pDwg_BOM)
                With pDwgBOM(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldParent_PartNo = mParent_PartNo(j)
                    .fldChild_PartNo = mChild_PartNo(j)
                    .fldQty = mQty(j)

                End With

                pSealProcessDBEntities.AddTotblDwg_BOM(pDwgBOM(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#End Region

End Class
