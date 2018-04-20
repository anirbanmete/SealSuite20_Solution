'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsEditedBy                    '
'                        VERSION NO  :  1.5                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  11APR18                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_EditedBy

#Region "STRUCTURES:"

    <Serializable()> _
    Public Structure sUser      '....SignOffUser
        Public Name As String
        Public Signed As Boolean
        Public DateSigned As Date
    End Structure

#End Region

#Region "MEMBER VARIABLES:"

    Private mTabName As String
    Private mID_Edit As New List(Of Integer)
    Private mDateEdited As New List(Of Date)
    Private mName As New List(Of String)
    Private mComment As New List(Of String)

    Private mUser As sUser

#End Region

#Region "PROPERTY ROUTINES:"

    '....TabName
    Public Property TabName() As String
        '===============================
        Get
            Return mTabName
        End Get

        Set(Obj As String)
            mTabName = Obj
        End Set
    End Property

    '....ID
    Public Property ID_Edit() As List(Of Integer)
        '========================================
        Get
            Return mID_Edit
        End Get

        Set(Obj As List(Of Integer))
            mID_Edit = Obj
        End Set
    End Property

    '....DateEdited
    Public Property DateEdited() As List(Of Date)
        '=========================================
        Get
            Return mDateEdited
        End Get

        Set(Obj As List(Of Date))
            mDateEdited = Obj
        End Set
    End Property

    '....Name
    Public Property Name() As List(Of String)
        '===================================
        Get
            Return mName
        End Get

        Set(Obj As List(Of String))
            mName = Obj
        End Set
    End Property

    '....Comment
    Public Property Comment() As List(Of String)
        '======================================
        Get
            Return mComment
        End Get

        Set(Obj As List(Of String))
            mComment = Obj
        End Set
    End Property

    '....User
    Public ReadOnly Property User() As sUser
        '====================================     
        Get
            Return mUser
        End Get
    End Property

    '....User_Name
    Public WriteOnly Property User_Name() As String
        '==========================================
        Set(ByVal strVal As String)
            mUser.Name = strVal
        End Set
    End Property

    '....User_Signed
    Public WriteOnly Property User_Signed() As Boolean
        '===============================================
        Set(ByVal blnVal As Boolean)
            mUser.Signed = blnVal
        End Set
    End Property

    '....User_DateSigned
    Public WriteOnly Property User_DateSigned() As Date
        '===============================================
        Set(ByVal strVal As Date)
            mUser.DateSigned = strVal
        End Set
    End Property

#End Region

#Region "DATABASE RELATED ROUTINES:"

    Public Function RetrieveFromDB(ByVal ProjectID_In As Integer, ByVal TabName_In As String) As Boolean
        '==============================================================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()
        Dim pIsRecExists As Boolean = False

        Try
            '....tblEditedBy
            mID_Edit.Clear()
            mDateEdited.Clear()
            mName.Clear()
            mComment.Clear()
            Dim pQryEditedByCount As Integer = (From pRec In pSealProcessDBEntities.tblEditedBy
                                                Where pRec.fldProcessProjectID = ProjectID_In And pRec.fldTabName = TabName_In Select pRec).Count()


            If (pQryEditedByCount > 0) Then
                pIsRecExists = True
                Dim pQryEditedBy = (From pRec In pSealProcessDBEntities.tblEditedBy
                                    Where pRec.fldProcessProjectID = ProjectID_In And pRec.fldTabName = TabName_In Select pRec).ToList()

                For i As Integer = 0 To pQryEditedBy.Count - 1
                    mID_Edit.Add(pQryEditedBy(i).fldID)
                    mDateEdited.Add(pQryEditedBy(i).fldDate)
                    mName.Add(pQryEditedBy(i).fldName)
                    mComment.Add(pQryEditedBy(i).fldComment)
                Next

            End If

        Catch ex As Exception

        End Try

        Return pIsRecExists

    End Function

    Public Function RetrieveFromDB_UserSignOff(ByVal ProjectID_In As Integer, ByVal TabName_In As String) As Boolean
        '==========================================================================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()
        Dim pIsRecExists As Boolean = False
        mUser.Name = ""
        mUser.Signed = False
        mUser.DateSigned = DateTime.MinValue

        Try

            If (TabName_In = "PreOrder") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblPreOrder
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Export") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblITAR_Export
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "OrdEntry") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblOrdEntry
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Cost") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblCost
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "App") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblApp
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Design") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblDesign
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Manf") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblManf
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Purchase") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblPurchase
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Qlty") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblQlty
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Dwg") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblDwg
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Dwg") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblDwg
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            ElseIf (TabName_In = "Test") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblTest
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

                ''ElseIf (TabName_In = "Planning") Then
                ''    Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblPlanning
                ''                        Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldUserSigned = True Select SignOff).ToList()

                ''    If (pUserSignOff.Count > 0) Then
                ''        pIsRecExists = True
                ''        '....Record already exists
                ''        mUser.Name = pUserSignOff(0).fldUserName
                ''        mUser.Signed = pUserSignOff(0).fldUserSigned
                ''        mUser.DateSigned = pUserSignOff(0).fldUserDate
                ''    End If

            ElseIf (TabName_In = "Shipping") Then
                Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblShipping
                                    Where SignOff.fldProcessProjectID = ProjectID_In Where SignOff.fldSigned = True Select SignOff).ToList()

                If (pUserSignOff.Count > 0) Then
                    pIsRecExists = True
                    '....Record already exists
                    mUser.Name = pUserSignOff(0).fldUserName
                    mUser.Signed = pUserSignOff(0).fldSigned
                    mUser.DateSigned = pUserSignOff(0).fldDateSigned
                End If

            End If


        Catch ex As Exception

        End Try

        Return pIsRecExists

    End Function

    'Public Sub SaveToDB(ByVal ProjectID_In As Integer, ByVal TabName_In As String)
    '    '===========================================================================

    '    Dim pSealProcessDBEntities As New SealProcessDBEntities()

    '    '....tblEditedBy
    '    Dim pEditedByCount As Integer = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                                     Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In And
    '                                         EditedBy.fldDate = mDateEdited And EditedBy.fldName = mName Select EditedBy).Count()

    '    If (pEditedByCount > 0) Then
    '        '....Record already exists
    '        Dim pEditedBy = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                         Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Select EditedBy).First()

    '        pEditedBy.fldDate = mDateEdited
    '        pEditedBy.fldName = mName
    '        pEditedBy.fldComment = mComment

    '        pSealProcessDBEntities.SaveChanges()

    '    Else
    '        Dim pRecCount As Integer = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                                    Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Select EditedBy).Count()

    '        Dim pID As Integer = 0
    '        If (pRecCount > 0) Then
    '            Dim pRec = (From EditedBy In pSealProcessDBEntities.tblEditedBy
    '                        Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Order By EditedBy.fldID Descending Select EditedBy).ToList()
    '            pID = pRec(0).fldID
    '        End If
    '        '....New Record
    '        Dim pProjectID As Integer = ProjectID_In

    '        Dim pEditedBy As New tblEditedBy
    '        pEditedBy.fldProcessProjectID = pProjectID
    '        pEditedBy.fldTabName = TabName_In
    '        pEditedBy.fldID = pID + 1
    '        pEditedBy.fldDate = mDateEdited
    '        pEditedBy.fldName = mName
    '        pEditedBy.fldComment = mComment

    '        pSealProcessDBEntities.AddTotblEditedBy(pEditedBy)
    '        pSealProcessDBEntities.SaveChanges()
    '    End If

    'End Sub

    Public Sub SaveToDB(ByVal ProjectID_In As Integer, ByVal TabName_In As String)
        '=========================================================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblEditedBy
        Dim pEditedBy = (From EditedBy In pSealProcessDBEntities.tblEditedBy
                         Where EditedBy.fldProcessProjectID = ProjectID_In And EditedBy.fldTabName = TabName_In Select EditedBy).ToList()


        If (pEditedBy.Count > 0) Then
            For j As Integer = 0 To pEditedBy.Count() - 1
                pSealProcessDBEntities.DeleteObject(pEditedBy(j))
                pSealProcessDBEntities.SaveChanges()
            Next
        End If

        Dim pEdited As New List(Of tblEditedBy)

        For j As Integer = 0 To mID_Edit.Count - 1
            Dim ptblEdited As New tblEditedBy
            pEdited.Add(ptblEdited)
            With pEdited(j)
                'AES 20APR18
                If (IsNothing(mName(j))) Then
                    Continue For
                End If
                .fldProcessProjectID = ProjectID_In
                .fldTabName = TabName_In
                .fldID = j + 1
                .fldDate = mDateEdited(j)
                .fldName = mName(j)
                .fldComment = mComment(j)

            End With

            pSealProcessDBEntities.AddTotblEditedBy(pEdited(j))
        Next
        pSealProcessDBEntities.SaveChanges()

    End Sub

    Public Sub SaveToDB_UserSignOff(ByVal ProjectID_In As Integer, ByVal TabName_In As String)
        '=====================================================================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        If (TabName_In = "PreOrder") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblPreOrder
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblPreOrder
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblPreOrder(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Export") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblITAR_Export
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblITAR_Export
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblITAR_Export(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "OrdEntry") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblOrdEntry
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblOrdEntry
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblOrdEntry(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Cost") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblCost
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblCost
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblCost(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "App") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblApp
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblApp
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblApp(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Design") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblDesign
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblDesign
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblDesign(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Manf") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblManf
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblManf
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblManf(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Purchase") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblPurchase
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblPurchase
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblPurchase(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Qlty") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblQlty
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblQlty
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblQlty(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Dwg") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblDwg
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblDwg
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblDwg(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        ElseIf (TabName_In = "Test") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblTest
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblTest
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblTest(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

            ''ElseIf (TabName_In = "Planning") Then
            ''    Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblPlanning
            ''                        Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            ''    If (pUserSignOff.Count > 0) Then
            ''        '....Record already exists
            ''        pUserSignOff(0).fldUserName = mUser.Name
            ''        pUserSignOff(0).fldUserSigned = mUser.Signed
            ''        pUserSignOff(0).fldUserDate = mUser.DateSigned
            ''        pSealProcessDBEntities.SaveChanges()
            ''    Else
            ''        '....New Record
            ''        Dim pRec As New tblPlanning
            ''        pRec.fldProcessProjectID = ProjectID_In
            ''        pRec.fldUserName = mUser.Name
            ''        pRec.fldUserSigned = mUser.Signed
            ''        pRec.fldUserDate = mUser.DateSigned

            ''        pSealProcessDBEntities.AddTotblPlanning(pRec)
            ''        pSealProcessDBEntities.SaveChanges()
            ''    End If

        ElseIf (TabName_In = "Shipping") Then
            Dim pUserSignOff = (From SignOff In pSealProcessDBEntities.tblShipping
                                Where SignOff.fldProcessProjectID = ProjectID_In Select SignOff).ToList()

            If (pUserSignOff.Count > 0) Then
                '....Record already exists
                pUserSignOff(0).fldUserName = mUser.Name
                pUserSignOff(0).fldSigned = mUser.Signed
                pUserSignOff(0).fldDateSigned = mUser.DateSigned
                pSealProcessDBEntities.SaveChanges()
            Else
                '....New Record
                Dim pRec As New tblShipping
                pRec.fldProcessProjectID = ProjectID_In
                pRec.fldUserName = mUser.Name
                pRec.fldSigned = mUser.Signed
                pRec.fldDateSigned = mUser.DateSigned

                pSealProcessDBEntities.AddTotblShipping(pRec)
                pSealProcessDBEntities.SaveChanges()
            End If

        End If

    End Sub

#End Region


End Class
