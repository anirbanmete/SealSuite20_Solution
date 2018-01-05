'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_Design                  '
'                        VERSION NO  :  1.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28DEC17                                '
'                                                                              '
'===============================================================================
<Serializable()> _
Public Class clsProcessProj_Design

#Region "STRUCTURES:"

    <Serializable()> _
    Structure sFrozen
        Public Design As Boolean
        Public Process As Boolean
    End Structure

#End Region

#Region "MEMBER VARIABLES:"

    Private mCustDwgNo As String
    Private mCustDwgRev As String
    Private mFrozen As sFrozen
    Private mIsClass1 As Boolean
    Private mIsBuildToPrint As Boolean
    Private mMCS As String
    Private mIsWinnovation As Boolean
    Private mWinnovationNo As String
    Private mIsMat_OutsideVender As Boolean
    Private mFOD_Risks As String
    Private mLessonsLearned As String
    Private mNotes As String

    Private mVerification As New clsVerification
    Private mInput As New clsInput
    Private mCustSpec As New clsCustSpec
    Private mSealDim As New clsSealDim

    Private mRisk As New clsProcessProj_Risk
    Private mEditedBy As New clsProcessProj_EditedBy

#End Region

#Region "PROPERTY ROUTINES:"

    '....CustDwgNo
    Public Property CustDwgNo() As String
        '=================================
        Get
            Return mCustDwgNo
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mCustDwgNo = strData
        End Set

    End Property

    '....CustDwgRev
    Public Property CustDwgRev() As String
        '=================================
        Get
            Return mCustDwgRev
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mCustDwgRev = strData
        End Set

    End Property

    '....Frozen
    Public ReadOnly Property Frozen() As sFrozen
        '=======================================
        Get
            Return mFrozen
        End Get

    End Property

    Public WriteOnly Property Frozen_Design() As Boolean
        '===============================================  
        Set(ByVal blnData As Boolean)
            '-------------------------------
            mFrozen.Design = blnData
        End Set

    End Property

    Public WriteOnly Property Frozen_Process() As Boolean
        '===============================================  
        Set(ByVal blnData As Boolean)
            '-------------------------------
            mFrozen.Process = blnData
        End Set

    End Property


    '....IsClass1
    Public Property IsClass1() As Boolean
        '===============================
        Get
            Return mIsClass1
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsClass1 = blnData
        End Set

    End Property

    '....IsBuildToPrint
    Public Property IsBuildToPrint() As Boolean
        '=====================================
        Get
            Return mIsBuildToPrint
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsBuildToPrint = blnData
        End Set

    End Property

    '....MCS
    Public Property MCS() As String
        '=================================
        Get
            Return mMCS
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mMCS = strData
        End Set

    End Property

    '....IsWinnovation
    Public Property IsWinnovation() As Boolean
        '=====================================
        Get
            Return mIsWinnovation
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsWinnovation = blnData
        End Set

    End Property

    '....WinnovationNo
    Public Property WinnovationNo() As String
        '====================================
        Get
            Return mWinnovationNo
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mWinnovationNo = strData
        End Set

    End Property


    '....IsMat_OutsideVender
    Public Property IsMat_OutsideVender() As Boolean
        '=====================================
        Get
            Return mIsMat_OutsideVender
        End Get

        Set(ByVal blnData As Boolean)
            '-------------------------------
            mIsMat_OutsideVender = blnData
        End Set

    End Property

    '....FOD_Risks
    Public Property FOD_Risks() As String
        '=================================
        Get
            Return mFOD_Risks
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mFOD_Risks = strData
        End Set

    End Property

    '....LessonsLearned
    Public Property LessonsLearned() As String
        '=================================
        Get
            Return mLessonsLearned
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mLessonsLearned = strData
        End Set

    End Property

    '....Notes
    Public Property Notes() As String
        '=================================
        Get
            Return mNotes
        End Get

        Set(ByVal strData As String)
            '-------------------------------
            mNotes = strData
        End Set

    End Property


#Region "Input"

    Public Property Input() As clsInput
        '=================================
        Get
            Return mInput
        End Get

        Set(ByVal strObj As clsInput)
            '-------------------------------
            mInput = strObj
        End Set

    End Property

#End Region

#Region "Verification"

    Public Property Verification() As clsVerification
        '=================================
        Get
            Return mVerification
        End Get

        Set(ByVal strObj As clsVerification)
            '-------------------------------
            mVerification = strObj
        End Set

    End Property

#End Region

#Region "CustSpec"

    Public Property CustSpec() As clsCustSpec
        '=================================
        Get
            Return mCustSpec
        End Get

        Set(ByVal strObj As clsCustSpec)
            '-------------------------------
            mCustSpec = strObj
        End Set

    End Property

#End Region

#Region "SealDim"

    Public Property SealDim() As clsSealDim
        '=================================
        Get
            Return mSealDim
        End Get

        Set(ByVal strObj As clsSealDim)
            '-------------------------------
            mSealDim = strObj
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

            '....tblDesign
            Dim pQryDesignCount As Integer = (From pRec In pSealProcessDBEntities.tblDesign
                               Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

            If (pQryDesignCount > 0) Then

                Dim pQryDesign = (From pRec In pSealProcessDBEntities.tblDesign
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).First()

                mCustDwgNo = pQryDesign.fldCustDwgNo
                mCustDwgRev = pQryDesign.fldCustDwgRev
                mFrozen.Design = pQryDesign.fldDesignFrozen
                mFrozen.Process = pQryDesign.fldProcessFrozen
                mIsClass1 = pQryDesign.fldIsClass1
                mIsBuildToPrint = pQryDesign.fldIsBuildToPrint
                mMCS = pQryDesign.fldMCS
                mIsWinnovation = pQryDesign.fldIsWinnovation
                mWinnovationNo = pQryDesign.fldWinnovationNo
                mIsMat_OutsideVender = pQryDesign.fldIsMat_OutsideVender
                mFOD_Risks = pQryDesign.fldFOD_Risks
                mLessonsLearned = pQryDesign.fldLessonsLearned
                mNotes = pQryDesign.fldNotes
                mEditedBy.User_Name = pQryDesign.fldUserName
                mEditedBy.User_Signed = pQryDesign.fldSigned

                If (Not IsNothing(pQryDesign.fldDateSigned) And Not IsDBNull(pQryDesign.fldDateSigned)) Then
                    mEditedBy.User_DateSigned = pQryDesign.fldDateSigned
                End If

            End If

            mInput.RetrieveFromDB(ProjectID_In)
            mVerification.RetrieveFromDB(ProjectID_In)
            'mKeyChar.RetrieveFromDB(ProjectID_In)
            mCustSpec.RetrieveFromDB(ProjectID_In)
            mSealDim.RetrieveFromDB(ProjectID_In)

        Catch ex As Exception

        End Try
    End Sub


    Public Sub SaveToDB(ByVal ProjectID_In As Integer)
        '==============================================

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        '....tblDesign
        Dim pDesignCount As Integer = (From Cost In pSealProcessDBEntities.tblDesign
                                            Where Cost.fldProcessProjectID = ProjectID_In Select Cost).Count()

        If (pDesignCount > 0) Then
            '....Record already exists
            Dim pDesign = (From Design In pSealProcessDBEntities.tblDesign
                                           Where Design.fldProcessProjectID = ProjectID_In Select Design).First()

            pDesign.fldCustDwgNo = mCustDwgNo
            pDesign.fldCustDwgRev = mCustDwgRev
            pDesign.fldDesignFrozen = mFrozen.Design
            pDesign.fldProcessFrozen = mFrozen.Process
            pDesign.fldIsClass1 = mIsClass1
            pDesign.fldIsBuildToPrint = mIsBuildToPrint
            pDesign.fldMCS = mMCS
            pDesign.fldIsWinnovation = mIsWinnovation
            pDesign.fldWinnovationNo = mWinnovationNo
            pDesign.fldIsMat_OutsideVender = mIsMat_OutsideVender
            pDesign.fldFOD_Risks = mFOD_Risks
            pDesign.fldLessonsLearned = mLessonsLearned
            pDesign.fldNotes = mNotes
            pDesign.fldUserName = mEditedBy.User.Name
            pDesign.fldSigned = mEditedBy.User.Signed
            pDesign.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.SaveChanges()

        Else
            '....New Record
            Dim pID As Integer = ProjectID_In

            Dim pDesign As New tblDesign
            pDesign.fldProcessProjectID = pID
            pDesign.fldCustDwgNo = mCustDwgNo
            pDesign.fldCustDwgRev = mCustDwgRev
            pDesign.fldDesignFrozen = mFrozen.Design
            pDesign.fldProcessFrozen = mFrozen.Process
            pDesign.fldIsClass1 = mIsClass1
            pDesign.fldIsBuildToPrint = mIsBuildToPrint
            pDesign.fldMCS = mMCS
            pDesign.fldIsWinnovation = mIsWinnovation
            pDesign.fldWinnovationNo = mWinnovationNo
            pDesign.fldIsMat_OutsideVender = mIsMat_OutsideVender
            pDesign.fldFOD_Risks = mFOD_Risks
            pDesign.fldLessonsLearned = mLessonsLearned
            pDesign.fldNotes = mNotes
            pDesign.fldUserName = mEditedBy.User.Name
            pDesign.fldSigned = mEditedBy.User.Signed
            pDesign.fldDateSigned = mEditedBy.User.DateSigned

            pSealProcessDBEntities.AddTotblDesign(pDesign)
            pSealProcessDBEntities.SaveChanges()
        End If

        mInput.SaveToDB(ProjectID_In)
        mVerification.SaveToDB(ProjectID_In)
        'mKeyChar.SaveToDB(ProjectID_In)
        mCustSpec.SaveToDB(ProjectID_In)
        mSealDim.SaveToDB(ProjectID_In)

    End Sub

#End Region

#Region "NESTED CLASS:"

#Region "Class clsInput"

    <Serializable()> _
    Public Class clsInput

#Region "MEMBER VARIABLES:"

        Private mID_Input As New List(Of Integer)
        Private mDesc As New List(Of String)

#End Region


#Region "PROPERTY ROUTINES:"

        '....ID_Input
        Public Property ID_Input() As List(Of Integer)
            '==========================================
            Get
                Return mID_Input
            End Get

            Set(Obj As List(Of Integer))
                mID_Input = Obj
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


#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblDesignInput
                Dim pQryDesignInputCount As Integer = (From pRec In pSealProcessDBEntities.tblDesign_Input
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryDesignInputCount > 0) Then

                    Dim pQryInput = (From pRec In pSealProcessDBEntities.tblDesign_Input
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryInput.Count - 1
                        mID_Input.Add(pQryInput(i).fldID)
                        mDesc.Add(pQryInput(i).fldDesc)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            'Dim pCost_SplOperation As New tblCost_SplOperation

            Dim pQryInput = (From Input In pSealProcessDBEntities.tblDesign_Input
                                                Where Input.fldProcessProjectID = ProjectID_In Select Input).ToList()

            If (pQryInput.Count > 0) Then
                For j As Integer = 0 To pQryInput.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryInput(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pInput As New List(Of tblDesign_Input)

            For j As Integer = 0 To mID_Input.Count - 1
                Dim pDesignInput As New tblDesign_Input
                pInput.Add(pDesignInput)
                With pInput(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDesc = mDesc(j)

                End With

                pSealProcessDBEntities.AddTotblDesign_Input(pInput(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class
#End Region

#Region "Class clsVerification"

    <Serializable()> _
    Public Class clsVerification

#Region "MEMBER VARIABLES:"

        Private mID_Verification As New List(Of Integer)
        Private mDesc As New List(Of String)
        Private mOwner As New List(Of String)
        Private mResult As New List(Of String)

#End Region


#Region "PROPERTY ROUTINES:"

        '....ID_Verification
        Public Property ID_Verification() As List(Of Integer)
            '==========================================
            Get
                Return mID_Verification
            End Get

            Set(Obj As List(Of Integer))
                mID_Verification = Obj
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

        '....Owner
        Public Property Owner() As List(Of String)
            '=====================================
            Get
                Return mOwner
            End Get

            Set(Obj As List(Of String))
                mOwner = Obj
            End Set
        End Property

        '....Result
        Public Property Result() As List(Of String)
            '=====================================
            Get
                Return mResult
            End Get

            Set(Obj As List(Of String))
                mResult = Obj
            End Set
        End Property


#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblDesignVerification
                Dim pQryDesignVerificationCount As Integer = (From pRec In pSealProcessDBEntities.tblDesign_Verification
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryDesignVerificationCount > 0) Then

                    Dim pQryVerification = (From pRec In pSealProcessDBEntities.tblDesign_Verification
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQryVerification.Count - 1
                        mID_Verification.Add(pQryVerification(i).fldID)
                        mDesc.Add(pQryVerification(i).fldDesc)
                        mOwner.Add(pQryVerification(i).fldOwner)
                        mResult.Add(pQryVerification(i).fldResult)
                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            'Dim pCost_SplOperation As New tblCost_SplOperation

            Dim pQryVerification = (From Input In pSealProcessDBEntities.tblDesign_Verification
                                                Where Input.fldProcessProjectID = ProjectID_In Select Input).ToList()

            If (pQryVerification.Count > 0) Then
                For j As Integer = 0 To pQryVerification.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQryVerification(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pVerification As New List(Of tblDesign_Verification)

            For j As Integer = 0 To mID_Verification.Count - 1
                Dim pDesignVerification As New tblDesign_Verification
                pVerification.Add(pDesignVerification)
                With pVerification(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldDesc = mDesc(j)
                    .fldOwner = mOwner(j)
                    .fldResult = mResult(j)

                End With

                pSealProcessDBEntities.AddTotblDesign_Verification(pVerification(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#Region "Class clsCustSpec"

    Public Class clsCustSpec

#Region "MEMBER VARIABLES:"

        Private mID_Cust As New List(Of Integer)
        Private mType As New List(Of String)
        Private mDesc As New List(Of String)
        Private mInterpret As New List(Of String)

#End Region

#Region "PROPERTY ROUTINES:"

        '....ID
        Public Property ID_Cust() As List(Of Integer)
            '=====================================
            Get
                Return mID_Cust
            End Get

            Set(Obj As List(Of Integer))
                mID_Cust = Obj
            End Set
        End Property

        '....Type
        Public Property Type() As List(Of String)
            '=======================================
            Get
                Return mType
            End Get

            Set(Obj As List(Of String))
                mType = Obj
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

        '....Interpret
        Public Property Interpret() As List(Of String)
            '=====================================
            Get
                Return mInterpret
            End Get

            Set(Obj As List(Of String))
                mInterpret = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblDesignSpec
                Dim pQryDesignSpecCount As Integer = (From pRec In pSealProcessDBEntities.tblDesign_CustSpec
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryDesignSpecCount > 0) Then

                    Dim pQrySpec = (From pRec In pSealProcessDBEntities.tblDesign_CustSpec
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQrySpec.Count - 1
                        ID_Cust.Add(pQrySpec(i).fldID)
                        mType.Add(pQrySpec(i).fldType)
                        mDesc.Add(pQrySpec(i).fldDesc)
                        mInterpret.Add(pQrySpec(i).fldInterpret)

                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            'Dim pCost_SplOperation As New tblCost_SplOperation

            Dim pQrySpec = (From Spec In pSealProcessDBEntities.tblDesign_CustSpec
                                                Where Spec.fldProcessProjectID = ProjectID_In Select Spec).ToList()

            If (pQrySpec.Count > 0) Then
                For j As Integer = 0 To pQrySpec.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQrySpec(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pSpec As New List(Of tblDesign_CustSpec)

            For j As Integer = 0 To mID_Cust.Count - 1
                Dim pDesignSpec As New tblDesign_CustSpec
                pSpec.Add(pDesignSpec)
                With pSpec(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldType = mType(j)
                    .fldDesc = mDesc(j)
                    .fldInterpret = mInterpret(j)

                End With

                pSealProcessDBEntities.AddTotblDesign_CustSpec(pSpec(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class

#End Region

#Region "Class clsSealDim"

    Public Class clsSealDim

#Region "MEMBER VARIABLES:"

        Private mID_Seal As New List(Of Integer)
        Private mName As New List(Of String)
        Private mMin As New List(Of Double)
        Private mNom As New List(Of Double)
        Private mMax As New List(Of Double)

#End Region

#Region "Property Routines:"
        '....ID
        Public Property ID_Seal() As List(Of Integer)
            '=====================================
            Get
                Return mID_Seal
            End Get

            Set(Obj As List(Of Integer))
                mID_Seal = Obj
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

        '....Min
        Public Property Min() As List(Of Double)
            '=======================================
            Get
                Return mMin
            End Get

            Set(Obj As List(Of Double))
                mMin = Obj
            End Set
        End Property

        '....Nom
        Public Property Nom() As List(Of Double)
            '=======================================
            Get
                Return mNom
            End Get

            Set(Obj As List(Of Double))
                mNom = Obj
            End Set
        End Property

        '....Max
        Public Property Max() As List(Of Double)
            '=======================================
            Get
                Return mMax
            End Get

            Set(Obj As List(Of Double))
                mMax = Obj
            End Set
        End Property

#End Region

#Region "DATABASE RELATED ROUTINE:"

        Public Sub RetrieveFromDB(ByVal ProjectID_In As Integer)
            '===================================================
            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            Try
                '....tblDesignSeal
                Dim pQryDesignSealCount As Integer = (From pRec In pSealProcessDBEntities.tblDesign_SealDim
                                   Where pRec.fldProcessProjectID = ProjectID_In Select pRec).Count()

                If (pQryDesignSealCount > 0) Then

                    Dim pQrySeal = (From pRec In pSealProcessDBEntities.tblDesign_SealDim
                                       Where pRec.fldProcessProjectID = ProjectID_In Select pRec).ToList()

                    For i As Integer = 0 To pQrySeal.Count - 1
                        ID_Seal.Add(pQrySeal(i).fldID)
                        mName.Add(pQrySeal(i).fldName)
                        mMin.Add(pQrySeal(i).fldMin)
                        mNom.Add(pQrySeal(i).fldNom)
                        mMax.Add(pQrySeal(i).fldMax)

                    Next

                End If

            Catch ex As Exception

            End Try
        End Sub


        Public Sub SaveToDB(ByVal ProjectID_In As Integer)
            '==============================================

            Dim pSealProcessDBEntities As New SealProcessDBEntities()

            'Dim pCost_SplOperation As New tblCost_SplOperation

            Dim pQrySeal = (From Seal In pSealProcessDBEntities.tblDesign_SealDim
                                                Where Seal.fldProcessProjectID = ProjectID_In Select Seal).ToList()

            If (pQrySeal.Count > 0) Then
                For j As Integer = 0 To pQrySeal.Count() - 1
                    pSealProcessDBEntities.DeleteObject(pQrySeal(j))
                    pSealProcessDBEntities.SaveChanges()
                Next
            End If

            Dim pSeal As New List(Of tblDesign_SealDim)

            For j As Integer = 0 To mID_Seal.Count - 1
                Dim pDesignSeal As New tblDesign_SealDim
                pSeal.Add(pDesignSeal)
                With pSeal(j)
                    .fldProcessProjectID = ProjectID_In
                    .fldID = j + 1
                    .fldName = mName(j)
                    .fldMin = mMin(j)
                    .fldNom = mNom(j)
                    .fldMax = mMax(j)
                End With

                pSealProcessDBEntities.AddTotblDesign_SealDim(pSeal(j))
            Next
            pSealProcessDBEntities.SaveChanges()

        End Sub

#End Region

    End Class


#End Region

#End Region

End Class


