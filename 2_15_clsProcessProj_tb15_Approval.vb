'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_Approval                '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
'                                                                              '
'===============================================================================

<Serializable()> _
Public Class clsProcessProj_Approval

#Region "MEMBER VARIABLES:"

    Private mID_Approval As New List(Of Integer)
    Private mDept As New List(Of String)
    Private mName As New List(Of String)
    Private mTitle As New List(Of String)
    Private mSigned As New List(Of Boolean)
    Private mDateSigned As New List(Of Date)

#End Region

#Region "PROPERTY ROUTINES:"

    '....ID_Approval
    Public Property ID_Approval() As List(Of Integer)
        '======================================
        Get
            Return mID_Approval
        End Get

        Set(Obj As List(Of Integer))
            mID_Approval = Obj
        End Set
    End Property


    '....Dept 
    Public Property Dept() As List(Of String)
        '=======================================
        Get
            Return mDept
        End Get

        Set(Obj As List(Of String))
            mDept = Obj
        End Set
    End Property

    '....Name 
    Public Property Name() As List(Of String)
        '=======================================
        Get
            Return mName
        End Get

        Set(Obj As List(Of String))
            mName = Obj
        End Set
    End Property

    '....Title 
    Public Property Title() As List(Of String)
        '=======================================
        Get
            Return mTitle
        End Get

        Set(Obj As List(Of String))
            mTitle = Obj
        End Set
    End Property

    '....Signed 
    Public Property Signed() As List(Of Boolean)
        '=======================================
        Get
            Return mSigned
        End Get

        Set(Obj As List(Of Boolean))
            mSigned = Obj
        End Set
    End Property


    '....DateSigned 
    Public Property DateSigned() As List(Of Date)
        '========================================
        Get
            Return mDateSigned
        End Get

        Set(Obj As List(Of Date))
            mDateSigned = Obj
        End Set
    End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

    Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
        '===================================================
        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try
            '....tblApproval
            Dim pQryApprovalCount As Integer = (From pRec In pSealProcessDBEntities.tblApproval
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryApprovalCount > 0) Then

                Dim pQryApproval = (From pRec In pSealProcessDBEntities.tblApproval
                                    Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                For i As Integer = 0 To pQryApproval.Count - 1
                    mID_Approval.Add(pQryApproval(i).fldID)
                    mDept.Add(pQryApproval(i).fldDept)
                    mName.Add(pQryApproval(i).fldName)
                    mTitle.Add(pQryApproval(i).fldTitle)
                    mSigned.Add(pQryApproval(i).fldSigned)
                    mDateSigned.Add(pQryApproval(i).fldDate)
                Next

            Else

                mID_Approval.Clear()
                mDept.Clear()
                mName.Clear()
                mTitle.Clear()
                mSigned.Clear()
                mDateSigned.Clear()

            End If

        Catch ex As Exception

        End Try

    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Dim pQryApproval = (From Approval In pSealProcessDBEntities.tblApproval
                                            Where Approval.fldProcessProjectID = ProjectID_In Select Approval).ToList()

        If (pQryApproval.Count > 0) Then
            For j As Integer = 0 To pQryApproval.Count() - 1
                pSealProcessDBEntities.DeleteObject(pQryApproval(j))
                pSealProcessDBEntities.SaveChanges()
            Next
        End If

        Dim pApproval As New List(Of tblApproval)

        For j As Integer = 0 To mID_Approval.Count - 1
            Dim pApprv As New tblApproval
            pApproval.Add(pApprv)
            With pApproval(j)
                .fldProcessProjectID = ProjectID_In
                .fldID = j + 1
                .fldDept = mDept(j)
                .fldName = mName(j)
                .fldTitle = mTitle(j)
                .fldSigned = mSigned(j)
                .fldDate = mDateSigned(j)
            End With

            pSealProcessDBEntities.AddTotblApproval(pApproval(j))
        Next

        pSealProcessDBEntities.SaveChanges()

    End Sub

#End Region

End Class
