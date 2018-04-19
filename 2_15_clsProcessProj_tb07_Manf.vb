'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  Process_clsManf                        '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JAN18                                '
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
Public Class clsProcessProj_Manf

#Region "MEMBER VARIABLES:"

    Private mBaseMat_PartNo As String
    Private mSpringMat_PartNo As String
    Private mHT As String
    Private mPreComp_Glue As String

    Private mToolNGage As New clsToolNGage

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....BaseMat_PartNo
    Public Property BaseMat_PartNo() As String
        '=====================================
        Get
            Return mBaseMat_PartNo
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mBaseMat_PartNo = strData
        End Set

    End Property

    '....SpringMat_PartNo
    Public Property SpringMat_PartNo() As String
        '=======================================
        Get
            Return mSpringMat_PartNo
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mSpringMat_PartNo = strData
        End Set

    End Property

    '....HT
    Public Property HT() As String
        '============================
        Get
            Return mHT
        End Get

        Set(ByVal dblData As String)
            '-------------------------------
            mHT = dblData
        End Set

    End Property

    '....PreComp_Glue
    Public Property PreComp_Glue() As String
        '===================================
        Get
            Return mPreComp_Glue
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mPreComp_Glue = strData
        End Set

    End Property


#Region "ToolNGage"

    Public Property ToolNGage() As clsToolNGage
        '=====================================
        Get
            Return mToolNGage
        End Get

        Set(ByVal strObj As clsToolNGage)
            '-------------------------------
            mToolNGage = strObj
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

            '....tblManf
            Dim pQryManfCount As Integer = (From pRec In pSealProcessDBEntities.tblManf
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryManfCount > 0) Then

                Dim pQryManf = (From pRec In pSealProcessDBEntities.tblManf
                                Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mBaseMat_PartNo = pQryManf.fldBaseMat_PartNo
                mSpringMat_PartNo = pQryManf.fldSpringMat_PartNo
                mHT = pQryManf.fldHT
                mPreComp_Glue = pQryManf.fldPreComp_Glue

                'mEditedBy.User_Name = pQryManf.fldUserName
                'mEditedBy.User_Signed = pQryManf.fldSigned

                'If (Not IsNothing(pQryManf.fldDateSigned) And Not IsDBNull(pQryManf.fldDateSigned)) Then
                '    mEditedBy.User_DateSigned = pQryManf.fldDateSigned
                'End If

            Else
                mBaseMat_PartNo = ""
                mSpringMat_PartNo = ""
                mHT = ""
                mPreComp_Glue = ""

            End If

            mToolNGage.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....App table
        Dim pManfCount As Integer = (From Manf In pSealProcessDBEntities.tblManf
                                            Where Manf.fldProcessProjectID = ProjectID_In Select Manf).Count()

        If (pManfCount > 0) Then
            '....Record already exists
            Dim pManf = (From Manf In pSealProcessDBEntities.tblManf
                                           Where Manf.fldProcessProjectID = ProjectID_In Select Manf).First()


            pManf.fldBaseMat_PartNo = mBaseMat_PartNo
            pManf.fldSpringMat_PartNo = mSpringMat_PartNo
            pManf.fldHT = mHT
            pManf.fldPreComp_Glue = mPreComp_Glue

            'pManf.fldUserName = mEditedBy.User.Name
            'pManf.fldSigned = mEditedBy.User.Signed
            'pManf.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pManf As New tblManf
            pManf.fldProcessProjectID = pID

            pManf.fldBaseMat_PartNo = mBaseMat_PartNo
            pManf.fldSpringMat_PartNo = mSpringMat_PartNo
            pManf.fldHT = mHT
            pManf.fldPreComp_Glue = mPreComp_Glue

            'pManf.fldUserName = mEditedBy.User.Name
            'pManf.fldSigned = mEditedBy.User.Signed
            'pManf.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblManf(pManf)
            pSealProcessDBEntities.SaveChanges()
        End If

        mToolNGage.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "Class clsToolNGage"

    <Serializable()> _
    Public Class clsToolNGage

#Region "MEMBER VARIABLES:"

        Private mID_Tool As New List(Of Integer)
        Private mPartNo As New List(Of String)
        Private mDesc As New List(Of String)
        Private mType As New List(Of String)
        Private mStatus As New List(Of String)
        Private mLeadTime As New List(Of Double)
        Private mDesignResponsibility As New List(Of String)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID_Input
        Public Property ID_Tool() As List(Of Integer)
            '==========================================
            Get
                Return mID_Tool
            End Get

            Set(Obj As List(Of Integer))
                mID_Tool = Obj
            End Set
        End Property

        '....PartNo
        Public Property PartNo() As List(Of String)
            '=====================================
            Get
                Return mPartNo
            End Get

            Set(Obj As List(Of String))
                mPartNo = Obj
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

        '....Type
        Public Property Type() As List(Of String)
            '=====================================
            Get
                Return mType
            End Get

            Set(Obj As List(Of String))
                mType = Obj
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
            '========================================
            Get
                Return mLeadTime
            End Get

            Set(Obj As List(Of Double))
                mLeadTime = Obj
            End Set
        End Property

        '....DesignResponsibility
        Public Property DesignResponsibility() As List(Of String)
            '======================================================
            Get
                Return mDesignResponsibility
            End Get

            Set(Obj As List(Of String))
                mDesignResponsibility = Obj
            End Set
        End Property


#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblManf_ToolNGage
                Dim pQryManfCount As Integer = (From pRec In pSealProcessDBEntities.tblManf_Tool_Gage
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryManfCount > 0) Then

                    Dim pQryManf = (From pRec In pSealProcessDBEntities.tblManf_Tool_Gage
                                    Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryManf.Count - 1
                        mID_Tool.Add(pQryManf(i).fldID)
                        mPartNo.Add(pQryManf(i).fldPartNo)
                        mDesc.Add(pQryManf(i).fldDesc)
                        mType.Add(pQryManf(i).fldType)
                        mStatus.Add(pQryManf(i).fldStatus)
                        mLeadTime.Add(pQryManf(i).fldLeadTime)
                        mDesignResponsibility.Add(pQryManf(i).fldDesignResponsibility)

                    Next

                Else
                    mID_Tool.Clear()
                    mPartNo.Clear()
                    mDesc.Clear()
                    mType.Clear()
                    mStatus.Clear()
                    mLeadTime.Clear()
                    mDesignResponsibility.Clear()

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()


            Dim pQryManf = (From Manf In pSealProcessDBEntities.tblManf_Tool_Gage
                                                Where Manf.fldProcessProjectID = ProjectID_In Select Manf).ToList()

            If (pQryManf.Count > 0) Then
                For j As Integer = 0 To pQryManf.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryManf(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pManfTool As New List(Of tblManf_Tool_Gage)

            For j As Integer = 0 To mID_Tool.Count - 1
                Dim pManfToolNGage As New tblManf_Tool_Gage
                pManfTool.Add(pManfToolNGage)
                With pManfTool(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldPartNo = mPartNo(j)
                    .fldDesc = mDesc(j)
                    .fldType = mType(j)
                    .fldStatus = mStatus(j)
                    .fldLeadTime = mLeadTime(j)
                    .fldDesignResponsibility = mDesignResponsibility(j)

                End With

                pSealProcessDBEntities.AddTotblManf_Tool_Gage(pManfTool(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#End Region

End Class
