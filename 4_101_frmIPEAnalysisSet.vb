
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmAnalysisSet                         '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  23MAY17                                '
'                                                                              '
'===============================================================================

Imports System.Linq
Imports System.Windows.Forms
Imports clsLibrary11

Public Class IPE_frmAnalysisSet

#Region "ENUMERATION TYPES:"
    '=======================

    Enum eSet
        _Existing
        _New
    End Enum

    Enum eModeOper
        None
        Edit
        Delete
        Copy
    End Enum

#End Region


#Region "MEMBER VARIABLES:"

    Private mSet As eSet
    Private mModeOper As eModeOper

    Private mICur As Integer
    Private mEditMode As Boolean

#End Region


#Region "PROPERTY ROUTINES:"
    '======================

    '....ModeOper
    Public ReadOnly Property ModeOper() As eModeOper
        '===========================================
        Get
            Return mModeOper
        End Get

    End Property

    '....ICur
    Public ReadOnly Property ICur() As Integer
        '======================================
        Get
            Return mICur
        End Get

    End Property

    '....Edit Mode
    Public ReadOnly Property EditMode() As Boolean
        '===========================================
        Get
            Return mEditMode
        End Get

    End Property

#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmAnalysisSet_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        '========================================================================================================
        Try

            DisplayData()

            Dim pCount As Integer = gIPE_Project.Analysis.Count

            If (pCount > 0) Then
                If (mICur > pCount) Then
                    mICur = 0
                End If
                grdAnalysisSet.Rows(mICur).Selected = True
                grdAnalysisSet.CurrentCell = grdAnalysisSet.Item(0, mICur)
            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub frmAnalysisSet_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==============================================================================================
        Dim pProjectIPE As New IPE_clsProject()
        pProjectIPE.SaveProject_FromPart(gIPE_Project.PN_ID, gIPE_Project.PN_Rev_ID, gIPE_Project.Project_ID, gIPE_User.Name)

        Try

            Dim pIsProjectExists As Boolean = True
            Dim pstrMsg As String = "Please select a "

            If (gIPE_Project.Project_ID = 0) Then
                pIsProjectExists = False
                pstrMsg = pstrMsg & "Parker Part No. first."
            End If

            If (Not pIsProjectExists) Then
                MessageBox.Show(pstrMsg, "Analysis set not found.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                ''Dim pfrmProject As New frmProject()
                ''pfrmProject.ShowDialog()
            End If

            mModeOper = eModeOper.None

            DisplayData()
            lblDesc.Text = Analysis_Desc()      'AES 22SEP16

            If (Not IsNothing(grdAnalysisSet.CurrentRow)) Then
                If (gIPE_Project.Analysis(grdAnalysisSet.CurrentRow.Index).Seal.Selected = True) Then
                    chkSel_Project.Checked = True
                    lblDesc.ForeColor = Color.Blue
                Else
                    If (gIPE_Project.Analysis(grdAnalysisSet.CurrentRow.Index).State = IPE_clsAnalysis.eState.Incomplete) Then
                        For j As Int16 = 0 To 5
                            grdAnalysisSet.Rows(grdAnalysisSet.CurrentRow.Index).Cells(j).Style.ForeColor = Color.OrangeRed
                        Next

                    Else
                        For j As Int16 = 0 To 5
                            grdAnalysisSet.Rows(grdAnalysisSet.CurrentRow.Index).Cells(j).Style.ForeColor = Color.Black
                        Next
                    End If
                    chkSel_Project.Checked = False
                    lblDesc.ForeColor = Color.Black
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub


#Region "HELPER ROUTINES:"

    Private Sub DisplayData()
        '====================
        grdAnalysisSet.AllowUserToAddRows = False
        grdAnalysisSet.Rows.Clear()

        Try

            Dim pCount As Integer = gIPE_Project.Analysis.Count

            If (pCount > 0) Then
                chkSel_Project.Enabled = True
                optExisting.Checked = True
                mSet = eSet._Existing

                For i As Integer = 0 To gIPE_Project.Analysis.Count - 1
                    With grdAnalysisSet
                        .Rows.Add()

                        .Rows(i).Cells(0).Value = (i + 1)

                        If Not IsNothing(gIPE_Project.Analysis(i).Seal) Then
                            .Rows(i).Cells(1).Value = gIPE_Project.Analysis(i).Seal.MCrossSecNo
                            .Rows(i).Cells(2).Value = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(i).Seal.Hfree)

                        Else
                            .Rows(i).Cells(1).Value = ""
                            .Rows(i).Cells(2).Value = ""
                        End If

                        .Rows(i).Cells(3).Value = gIPE_Project.Analysis(i).LoadCase.Name

                        If (gIPE_Project.Analysis(i).DateCreated = Date.MinValue) Then
                            .Rows(i).Cells(4).Value = ""
                            .Rows(i).Cells(5).Value = ""
                        Else
                            .Rows(i).Cells(4).Value = gIPE_Project.Analysis(i).DateCreated.ToShortDateString()
                            .Rows(i).Cells(5).Value = gIPE_Project.Analysis(i).TimeCreated.ToString("hh:mm tt")

                        End If

                        If (mICur = 0) Then
                            .Rows(0).Selected = True
                            mICur = grdAnalysisSet.CurrentRow.Index
                        End If



                        If (gIPE_Project.Analysis(i).Seal.Selected = True) Then
                            For j As Int16 = 0 To 5
                                .Rows(grdAnalysisSet.RowCount - 1).Cells(j).Style.ForeColor = Color.Blue
                            Next

                        Else

                            If (gIPE_Project.Analysis(i).State = IPE_clsAnalysis.eState.Incomplete) Then
                                For j As Int16 = 0 To 5
                                    .Rows(grdAnalysisSet.RowCount - 1).Cells(j).Style.ForeColor = Color.OrangeRed
                                Next
                            Else
                                For j As Int16 = 0 To 5
                                    .Rows(grdAnalysisSet.RowCount - 1).Cells(j).Style.ForeColor = Color.Black
                                Next
                            End If
                        End If

                    End With

                Next

            Else
                chkSel_Project.Enabled = False
                optNew.Checked = True
                mSet = eSet._New
            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#End Region


#Region " GRIDVIEW RELATED ROUTINES:"

    Private Sub grdAnalysisSet_RowHeaderMouseClick(sender As System.Object,
                                                   e As DataGridViewCellMouseEventArgs) _
                                                   Handles grdAnalysisSet.RowHeaderMouseClick
        '======================================================================================
        If (mICur <> grdAnalysisSet.CurrentRow.Index) Then
            cmdEdit.ForeColor = Color.Black
            mEditMode = False
            mModeOper = eModeOper.None
        End If
        lblDesc.Text = Analysis_Desc()  'AES 22SEP16

        mICur = grdAnalysisSet.CurrentRow.Index

        If (Not IsNothing(grdAnalysisSet.CurrentRow)) Then
            If (gIPE_Project.Analysis(grdAnalysisSet.CurrentRow.Index).Seal.Selected = True) Then
                chkSel_Project.Checked = True
                lblDesc.ForeColor = Color.Blue
            Else

                chkSel_Project.Checked = False
                lblDesc.ForeColor = Color.Black

                If (gIPE_Project.Analysis(grdAnalysisSet.CurrentRow.Index).State = IPE_clsAnalysis.eState.Incomplete) Then
                    For j As Int16 = 0 To 5
                        grdAnalysisSet.Rows(grdAnalysisSet.CurrentRow.Index).Cells(j).Style.ForeColor = Color.OrangeRed
                    Next

                Else
                    For j As Int16 = 0 To 5
                        grdAnalysisSet.Rows(grdAnalysisSet.CurrentRow.Index).Cells(j).Style.ForeColor = Color.Black
                    Next
                End If
            End If
        End If

    End Sub

#End Region


#Region "OPTION BUTTON RELATED ROUTINE:"

    Private Sub opt_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                  Handles optNew.CheckedChanged, optExisting.CheckedChanged
        '====================================================================================

        If (optExisting.Checked) Then
            mSet = eSet._Existing
            cmdImport.Enabled = False
        ElseIf (optNew.Checked) Then
            mSet = eSet._New
            cmdImport.Enabled = True
        End If

    End Sub

#End Region


#Region "CHECK BOX RELATED ROUTINE:"

    Private Sub chkSel_Project_CheckedChanged(sender As System.Object,
                                              e As System.EventArgs) Handles chkSel_Project.CheckedChanged
        '===================================================================================================

        Dim pCurRowIndex As Integer

        If (chkSel_Project.Checked) Then

            If (Not IsNothing(grdAnalysisSet.CurrentRow)) Then
                pCurRowIndex = grdAnalysisSet.CurrentRow.Index
                gIPE_Project.Analysis(mICur).Seal.Selected = True

                For j As Int16 = 0 To 5
                    grdAnalysisSet.Rows(pCurRowIndex).Cells(j).Style.ForeColor = Color.Blue
                Next
                lblDesc.ForeColor = Color.Blue

                For i As Integer = 0 To grdAnalysisSet.RowCount - 1
                    If (i <> grdAnalysisSet.CurrentRow.Index) Then
                        gIPE_Project.Analysis(i).Seal.Selected = False
                        If (gIPE_Project.Analysis(i).State = IPE_clsAnalysis.eState.Incomplete) Then
                            For j As Int16 = 0 To 5
                                grdAnalysisSet.Rows(i).Cells(j).Style.ForeColor = Color.OrangeRed
                            Next

                        Else
                            For j As Int16 = 0 To 5
                                grdAnalysisSet.Rows(i).Cells(j).Style.ForeColor = Color.Black
                            Next
                        End If
                    End If
                Next

            End If

        Else
            gIPE_Project.Analysis(mICur).Seal.Selected = False
            pCurRowIndex = grdAnalysisSet.CurrentRow.Index

            If (gIPE_Project.Analysis(pCurRowIndex).State = IPE_clsAnalysis.eState.Incomplete) Then
                For j As Int16 = 0 To 5
                    grdAnalysisSet.Rows(pCurRowIndex).Cells(j).Style.ForeColor = Color.OrangeRed
                Next

            Else
                For j As Int16 = 0 To 5
                    grdAnalysisSet.Rows(pCurRowIndex).Cells(j).Style.ForeColor = Color.Black
                Next

            End If
            lblDesc.ForeColor = Color.Black

        End If
    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdButtons_Click(sender As System.Object, e As System.EventArgs) _
                                 Handles cmdCavity.Click, cmdOpCond.Click, cmdAppLoad.Click, _
                                         cmdSealDesign.Click, cmdFEA.Click
        '=========================================================================================

        If (mSet = eSet._New) Then
            gIPE_Project.Add_Analysis()
            'Dim pProjectEntities As New ProjectDBEntities
            'Dim pQryAnalysis = (From pRec In pProjectEntities.tblAnalysis
            '                           Where pRec.fldProjectID = gIPE_Project.Project_ID Order By pRec.fldID Descending Select pRec).ToList()

            Dim pSealIPEEntities As New SealIPEDBEntities 'SealIPEDBEntities
            Dim pQryAnalysis = (From pRec In pSealIPEEntities.tblAnalysis
                                       Where pRec.fldProjectID = gIPE_Project.Project_ID Order By pRec.fldID Descending Select pRec).ToList()

            Dim pNewAnalysisID As Integer
            If pQryAnalysis.Count() > 1 Then

                pNewAnalysisID = pQryAnalysis(0).fldID + 1
            Else
                pNewAnalysisID = 1
            End If

            gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).ID = pNewAnalysisID
            grdAnalysisSet.Rows.Add()
            grdAnalysisSet.Rows(grdAnalysisSet.RowCount - 1).Cells(0).Value = grdAnalysisSet.RowCount
            grdAnalysisSet.Rows(grdAnalysisSet.RowCount - 1).Selected = True
            mICur = grdAnalysisSet.RowCount - 1
            grdAnalysisSet.CurrentCell = grdAnalysisSet.Item(0, mICur)
            mModeOper = eModeOper.Edit
            mEditMode = True
            optExisting.Checked = True
        End If

        mICur = grdAnalysisSet.CurrentRow.Index

        Dim pcmdButton As Button = CType(sender, Button)

        Select Case pcmdButton.Name

            Case "cmdCavity"
                '--------------
                Dim pfrmCavity As New IPE_frmCavity
                pfrmCavity.ShowDialog()

            Case "cmdOpCond"
                '---------------
                Dim pfrmOpCond As New IPE_frmOpCond
                pfrmOpCond.ShowDialog()

            Case "cmdAppLoad"
                '----------------
                Dim pfrmAppLoad As New IPE_frmAppliedLoading
                pfrmAppLoad.ShowDialog()

            Case "cmdSealDesign"
                '-------------------
                Dim pfrmDesignSeal As New IPE_frmDesignSeal
                pfrmDesignSeal.ShowDialog()

            Case "cmdFEA"
                '-----------
                Dim pfrmFEA As New IPE_frmFEA
                pfrmFEA.ShowDialog()

        End Select

    End Sub


    Private Sub cmdEdit_Click(sender As System.Object, e As System.EventArgs) Handles cmdEdit.Click
        '===========================================================================================
        If grdAnalysisSet.Rows.Count > 0 Then
            mICur = grdAnalysisSet.CurrentRow.Index
            Dim pintAnswer As Integer
            pintAnswer = MessageBox.Show("The corresponding results for this analysis will be deleted." & vbCrLf & "Would you like to proceed?", "Edit Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If pintAnswer = Windows.Forms.DialogResult.Yes Then
                cmdEdit.ForeColor = Color.Green
                mModeOper = eModeOper.Edit
                mEditMode = True
                gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Reset_Result()
            Else
                mModeOper = eModeOper.None
                mEditMode = False
                cmdEdit.ForeColor = Color.Black
            End If

        End If

    End Sub


    Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete.Click
        '==============================================================================================
        Dim pSealIPEEntities As New SealIPEDBEntities 'SealIPEDBEntities
        Try

            If grdAnalysisSet.Rows.Count > 0 Then
                Dim pCurrRowIndx As Integer = grdAnalysisSet.CurrentRow.Index
                Dim pAnalysisID As Integer = gIPE_Project.Analysis(pCurrRowIndx).ID

                If (pAnalysisID <> 0) Then

                    Dim pResult_Gen = (From Rec In pSealIPEEntities.tblResult_Gen
                                    Where Rec.fldProjectID = gIPE_Project.Project_ID And
                                          Rec.fldAnalysisID = pAnalysisID Select Rec).ToList()

                    If (pResult_Gen.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pResult_Gen(0))
                        pSealIPEEntities.SaveChanges()
                    End If

                    Dim pResult_LoadStep = (From Rec In pSealIPEEntities.tblResult_LoadStep
                                    Where Rec.fldProjectID = gIPE_Project.Project_ID And
                                          Rec.fldAnalysisID = pAnalysisID Select Rec).ToList()

                    If (pResult_LoadStep.Count() > 0) Then

                        For j As Integer = 0 To pResult_LoadStep.Count() - 1
                            pSealIPEEntities.DeleteObject(pResult_LoadStep(j))
                            pSealIPEEntities.SaveChanges()
                        Next

                    End If

                    Dim pFEA = (From Rec In pSealIPEEntities.tblFEAParam
                                   Where Rec.fldProjectID = gIPE_Project.Project_ID And
                                         Rec.fldAnalysisID = pAnalysisID Select Rec).ToList()

                    If (pFEA.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pFEA(0))
                        pSealIPEEntities.SaveChanges()
                    End If

                    Dim pAnalysis = (From Analysis In pSealIPEEntities.tblAnalysis
                                        Where Analysis.fldID = pAnalysisID And
                                              Analysis.fldProjectID = gIPE_Project.Project_ID Select Analysis).ToList()

                    If (pAnalysis.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pAnalysis(0))
                        pSealIPEEntities.SaveChanges()
                    End If

                    '....Cavity
                    Dim pCavity = (From pRec In pSealIPEEntities.tblCavity
                                       Where pRec.fldProjectID = gIPE_Project.Project_ID And
                                             pRec.fldID = pAnalysisID Select pRec).ToList()

                    If (pCavity.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pCavity(0))
                        pSealIPEEntities.SaveChanges()
                    End If


                    '....OpCond
                    Dim pOpCond = (From pRec In pSealIPEEntities.tblOpCond
                                       Where pRec.fldProjectID = gIPE_Project.Project_ID And
                                             pRec.fldID = pAnalysisID Select pRec).ToList()

                    If (pOpCond.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pOpCond(0))
                        pSealIPEEntities.SaveChanges()
                    End If

                    '....AddLoad
                    Dim pAddLoad = (From pRec In pSealIPEEntities.tblAddLoad
                                       Where pRec.fldProjectID = gIPE_Project.Project_ID And
                                             pRec.fldAppLoadID = pAnalysisID Select pRec).ToList()

                    If (pAddLoad.Count() > 0) Then
                        For j As Integer = 0 To pAddLoad.Count() - 1
                            pSealIPEEntities.DeleteObject(pAddLoad(j))
                            pSealIPEEntities.SaveChanges()
                        Next
                    End If

                    '....AppLoad
                    Dim pAppLoad = (From pRec In pSealIPEEntities.tblAppLoad
                                       Where pRec.fldProjectID = gIPE_Project.Project_ID And
                                             pRec.fldID = pAnalysisID Select pRec).ToList()

                    If (pAppLoad.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pAppLoad(0))
                        pSealIPEEntities.SaveChanges()
                    End If

                    Dim pSealType As String = gIPE_Project.Analysis(pCurrRowIndx).Seal.Type

                    If (pSealType = "E-Seal") Then
                        Dim pAdjESeal = (From it In pSealIPEEntities.tblAdjESeal
                                              Where it.fldProjectID = gIPE_Project.Project_ID And
                                                 it.fldSealID = pAnalysisID Select it).ToList()

                        If (pAdjESeal.Count() > 0) Then
                            pSealIPEEntities.DeleteObject(pAdjESeal(0))
                            pSealIPEEntities.SaveChanges()
                        End If

                    ElseIf (pSealType = "C-Seal") Then
                        Dim pAdjCSeal = (From it In pSealIPEEntities.tblAdjCSeal
                                              Where it.fldProjectID = gIPE_Project.Project_ID And
                                                 it.fldSealID = pAnalysisID Select it).ToList()

                        If (pAdjCSeal.Count() > 0) Then
                            pSealIPEEntities.DeleteObject(pAdjCSeal(0))
                            pSealIPEEntities.SaveChanges()
                        End If

                    ElseIf (pSealType = "U-Seal") Then
                        Dim pAdjUSeal = (From it In pSealIPEEntities.tblAdjUSeal
                                              Where it.fldProjectID = gIPE_Project.Project_ID And
                                                 it.fldSealID = pAnalysisID Select it).ToList()

                        If (pAdjUSeal.Count() > 0) Then
                            pSealIPEEntities.DeleteObject(pAdjUSeal(0))
                            pSealIPEEntities.SaveChanges()
                        End If
                    End If

                    '....Seal
                    Dim pSeal = (From pRec In pSealIPEEntities.tblSeal
                                       Where pRec.fldProjectID = gIPE_Project.Project_ID And
                                             pRec.fldID = pAnalysisID Select pRec).ToList()


                    If (pSeal.Count() > 0) Then
                        pSealIPEEntities.DeleteObject(pSeal(0))
                        pSealIPEEntities.SaveChanges()
                    End If

                    For i As Integer = 0 To gIPE_Project.Analysis.Count - 1
                        If (gIPE_Project.Analysis(i).ID = pAnalysisID) Then
                            gIPE_Project.Analysis.RemoveAt(i)
                            Exit For
                        End If
                    Next

                    'mModeOper = eModeOper.Delete
                    mModeOper = eModeOper.None
                    DisplayData()

                End If
            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub cmdCopy_Click(sender As System.Object, e As System.EventArgs) Handles cmdCopy.Click
        '===========================================================================================
        Dim pSealIPEEntities As New SealIPEDBEntities 'SealIPEDBEntities
        Try

            If grdAnalysisSet.Rows.Count > 0 Then
                mModeOper = eModeOper.Copy

                Dim pCurrRowIndx As Integer
                If (IsNothing(grdAnalysisSet.CurrentRow)) Then
                    pCurrRowIndx = 0
                Else
                    pCurrRowIndx = grdAnalysisSet.CurrentRow.Index
                End If

                'Dim pAnalysisID As Integer = gIPE_Project.Analysis(pCurrRowIndx).ID

                gIPE_Project.Add_Analysis(pCurrRowIndx)

                Dim pCustomerID As Integer, pProjectID As Integer, pPlatformID As Integer, pNewAnalysisID As Integer

                pCustomerID = gIPE_Project.Customer_ID
                pPlatformID = gIPE_Project.Platform_ID
                pProjectID = gIPE_Project.Project_ID

                Dim pQryAnalysis = (From pRec In pSealIPEEntities.tblAnalysis
                                        Where pRec.fldProjectID = pProjectID Order By pRec.fldID Descending Select pRec).First()

                pNewAnalysisID = pQryAnalysis.fldID + 1

                gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).ID = pNewAnalysisID

                With grdAnalysisSet
                    .Rows(.Rows.Count - 1).Selected = True
                    .Rows.Add()

                    .Rows(grdAnalysisSet.RowCount - 1).Cells(0).Value = grdAnalysisSet.RowCount
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(1).Value = gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).Seal.MCrossSecNo
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(2).Value = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).Seal.Hfree)
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(3).Value = gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).LoadCase.Name

                    For j As Int16 = 0 To 5
                        .Rows(grdAnalysisSet.RowCount - 1).Cells(j).Style.ForeColor = Color.OrangeRed
                    Next
                    .Rows(.Rows.Count - 1).Selected = True

                    mICur = .Rows.Count - 1
                    .CurrentCell = .Item(0, mICur)
                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

    End Sub

    Private Sub cmdPrint_Click(sender As System.Object, e As System.EventArgs) Handles cmdPrint.Click
        '==============================================================================================
        'AES 16SEP16
        If grdAnalysisSet.Rows.Count > 0 Then
            Dim pCurrRowIndx As Integer = grdAnalysisSet.CurrentRow.Index
            'Dim pAnalysisID As Integer = gIPE_Project.Analysis(pCurrRowIndx).ID

            Dim pFileTitle As String = Analysis_Desc() + ".in"

            gIPE_Project.Analysis(pCurrRowIndx).WriteInFile(gIPE_User, gIPE_Unit, gIPE_ANSYS, pFileTitle)
        End If

    End Sub


    Private Sub cmdImport_Click(sender As System.Object, e As System.EventArgs) Handles cmdImport.Click
        '===============================================================================================

        mModeOper = eModeOper.Edit

        gIPE_Project.Add_Analysis()

        Dim pSealIPEEntities As New SealIPEDBEntities 'SealIPEDBEntities
        Dim pQryAnalysis = (From pRec In pSealIPEEntities.tblAnalysis
                                   Where pRec.fldProjectID = gIPE_Project.Project_ID Order By pRec.fldID Descending Select pRec).ToList()

        Dim pNewAnalysisID As Integer
        If pQryAnalysis.Count() > 0 Then

            pNewAnalysisID = pQryAnalysis(0).fldID + 1
        Else
            pNewAnalysisID = 1
        End If

        gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).ID = pNewAnalysisID

        With OpenFileDialog1
            .Filter = "SealIPE Input Files (*.in)|*.in"
            .FilterIndex = 1
            .InitialDirectory = gIPE_File.DirIn
            .FileName = ""
            .Title = "Open"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                gIPE_File.In_Name = .FileName
                gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).ReadInFile(gIPE_Project, gIPE_frmAnalysisSet.ICur, gIPE_File.In_Name, gIPE_Unit, gIPE_ANSYS)

                With grdAnalysisSet
                    .Rows(.Rows.Count - 1).Selected = True
                    '.Rows.Add()
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(0).Value = grdAnalysisSet.RowCount
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(1).Value = gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).Seal.MCrossSecNo
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(2).Value = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).Seal.Hfree)
                    .Rows(grdAnalysisSet.RowCount - 1).Cells(3).Value = gIPE_Project.Analysis(gIPE_Project.Analysis.Count - 1).LoadCase.Name

                    For j As Int16 = 0 To 5
                        .Rows(grdAnalysisSet.RowCount - 1).Cells(j).Style.ForeColor = Color.OrangeRed
                    Next
                    .Rows(.Rows.Count - 1).Selected = True

                    mICur = .Rows.Count - 1
                    .CurrentCell = .Item(0, mICur)
                End With

            End If

        End With

    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        If grdAnalysisSet.Rows.Count > 0 Then

            Dim pCurrRowIndx As Integer = grdAnalysisSet.CurrentRow.Index
            Dim pAnalysisID As Integer = gIPE_Project.Analysis(pCurrRowIndx).ID

            If (gIPE_Project.Analysis.Count > gIPE_frmAnalysisSet.ICur) Then
                If (pAnalysisID <> gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).ID) Then
                    mICur = grdAnalysisSet.CurrentRow.Index
                End If
            Else
                mICur = 0
            End If


            If (mICur <> grdAnalysisSet.CurrentRow.Index) Then
                mModeOper = eModeOper.None
            End If

        End If

        cmdEdit.ForeColor = Color.Black
        mEditMode = False
        mModeOper = eModeOper.None

        Me.Hide()

    End Sub

    Private Sub cmdSummaryResult_Click(sender As System.Object, e As System.EventArgs) Handles cmdSummaryResult.Click
        '============================================================================================================
        gIPE_frmResults.ShowDialog()

    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '===============================================================================================
        Me.Close()
    End Sub

#End Region


#Region "UTILTY ROUTINES:"

    Private Function Analysis_Desc() As String
        '=====================================
        Dim pDesc As String = ""
        Dim pCount As Integer = gIPE_Project.Analysis.Count

        If (pCount > 0) Then
            Dim pIndex As Integer = 0
            If (Not IsNothing(grdAnalysisSet.CurrentRow)) Then
                pIndex = grdAnalysisSet.CurrentRow.Index
            End If

            Dim pAnaDesc As String = ""
            Dim pMCS As String = gIPE_Project.Analysis(pIndex).Seal.MCrossSecNo
            If (pMCS <> "") Then
                pAnaDesc = "MCS" & pMCS
            End If
            Dim pFreeHt As String = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(pIndex).Seal.Hfree).ToString().Replace(".", "")
            If (pFreeHt <> "") Then
                pAnaDesc = pAnaDesc & "_FH" & pFreeHt
            End If
            Dim pLoadCaseName As String = gIPE_Project.Analysis(pIndex).LoadCase.Name
            If (pLoadCaseName <> "") Then
                pAnaDesc = pAnaDesc & "_" & pLoadCaseName
            End If
            Dim pDate As String = ""
            Dim pTime As String = ""
            If (gIPE_Project.Analysis(pIndex).DateCreated <> Date.MinValue) Then
                pDate = gIPE_Project.Analysis(pIndex).DateCreated.ToString("ddMMMyy")
                pTime = gIPE_Project.Analysis(pIndex).TimeCreated.ToString("t").Replace(":", "").Trim().Replace(" ", "")
                pAnaDesc = pAnaDesc & "_" & pDate & "_" & pTime
            End If

            'lblDesc.Text = pAnaDesc
            pDesc = pAnaDesc
        Else
            'lblDesc.Text = ""
            pDesc = ""
        End If

        Return pDesc

    End Function

#End Region


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) _
                             Handles cmdExportToPNDB.Click
        '======================================================================
        Dim pSelectedSealIndex As Integer = -1
        For i As Integer = 0 To gIPE_Project.Analysis.Count - 1
            If (gIPE_Project.Analysis(i).Seal.Selected) Then
                pSelectedSealIndex = i
                Exit For
            End If
        Next

        If pSelectedSealIndex > -1 Then
            Dim pPartDBEntities As New SealPartDBEntities

            Dim pQryHWFace_Count As Integer = (From pRec In pPartDBEntities.tblHW_Face
                                       Where pRec.fldPNID = gIPE_Project.PN_ID And pRec.fldRevID = gIPE_Project.PN_Rev_ID Select pRec).Count()
            If (pQryHWFace_Count > 0) Then
                '....Record already exists
                MessageBox.Show("Record already exists in the Part DB.", "Record Exists!", MessageBoxButtons.OK, MessageBoxIcon.Stop)

            Else
                '....New Record
                Dim pHWFace As New tblHW_Face
                pHWFace.fldPNID = gIPE_Project.PN_ID
                pHWFace.fldRevID = gIPE_Project.PN_Rev_ID

                pHWFace.fldType = gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type
                pHWFace.fldMCS = gIPE_Project.Analysis(pSelectedSealIndex).Seal.MCrossSecNo
                pHWFace.fldSegmented = gIPE_Project.Analysis(pSelectedSealIndex).Seal.IsSegmented
                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.IsSegmented) Then
                    pHWFace.fldSegmentCount = gIPE_Project.Analysis(pSelectedSealIndex).Seal.CountSegment
                Else
                    pHWFace.fldSegmentCount = 0
                End If
                pHWFace.fldMatName = gIPE_Project.Analysis(pSelectedSealIndex).Seal.Mat.Name
                pHWFace.fldHT = gIPE_Project.Analysis(pSelectedSealIndex).Seal.Mat.HT
                pHWFace.fldTemper = 0
                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type = "E-Seal") Then
                    pHWFace.fldCoating = gIPE_Project.Analysis(pSelectedSealIndex).Seal.Mat.Coating
                    If (pHWFace.fldCoating = gIPE_Project.Analysis(pSelectedSealIndex).Seal.Mat.Coating <> "None") Then
                        pHWFace.fldSFinish = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsESeal).SFinish 'gIPE_Project.Analysis(pSelectedSealIndex).Seal.SFinish
                    Else
                        pHWFace.fldSFinish = 0
                    End If

                End If
                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type = "C-Seal" Or gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type = "SC-Seal") Then
                    pHWFace.fldPlatingCode = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsCSeal).Plating.Code 'mHW.Plating.Code
                    pHWFace.fldPlatingThickCode = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsCSeal).Plating.ThickCode

                End If

                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.HfreeStd > gcEPS) Then
                    pHWFace.fldHfreeStd = gIPE_Project.Analysis(pSelectedSealIndex).Seal.HfreeStd
                Else
                    pHWFace.fldHfreeStd = 0
                End If

                pHWFace.fldHFreeTol1 = gIPE_Project.Analysis(pSelectedSealIndex).Seal.HFreeTol(1)
                pHWFace.fldHFreeTol2 = gIPE_Project.Analysis(pSelectedSealIndex).Seal.HFreeTol(2)
                pHWFace.fldPOrient = gIPE_Project.Analysis(pSelectedSealIndex).Seal.POrient

                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.DControl > gcEPS) Then
                    pHWFace.fldDControl = gIPE_Project.Analysis(pSelectedSealIndex).Seal.DControl
                Else
                    pHWFace.fldDControl = 0
                End If

                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.H11Tol > gcEPS) Then
                    pHWFace.fldH11Tol = gIPE_Project.Analysis(pSelectedSealIndex).Seal.H11Tol
                Else
                    pHWFace.fldH11Tol = 0
                End If

                If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.Adjusted = "N") Then
                    pHWFace.fldAdjusted = False
                Else
                    pHWFace.fldAdjusted = True

                    If (gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type = "E-Seal") Then
                        Dim pHW_AdjESeal As New tblHW_AdjESeal
                        pHW_AdjESeal.fldPNID = gIPE_Project.PN_ID
                        pHW_AdjESeal.fldRevID = gIPE_Project.PN_Rev_ID

                        pHW_AdjESeal.fldDThetaE1 = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsESeal).DThetaE1
                        pHW_AdjESeal.fldDThetaM1 = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsESeal).DThetaM1

                        pPartDBEntities.AddTotblHW_AdjESeal(pHW_AdjESeal)
                        pPartDBEntities.SaveChanges()

                    ElseIf (gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type = "C-Seal") Then

                        Dim pHW_AdjCSeal As New tblHW_AdjCSeal
                        pHW_AdjCSeal.fldPNID = gIPE_Project.PN_ID
                        pHW_AdjCSeal.fldRevID = gIPE_Project.PN_Rev_ID

                        pHW_AdjCSeal.fldDHFree = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsCSeal).DHfree
                        pHW_AdjCSeal.fldDThetaOpening = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsCSeal).DThetaOpening
                        pHW_AdjCSeal.fldDT = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsCSeal).T

                        pPartDBEntities.AddTotblHW_AdjCSeal(pHW_AdjCSeal)
                        pPartDBEntities.SaveChanges()

                    ElseIf (gIPE_Project.Analysis(pSelectedSealIndex).Seal.Type = "U-Seal") Then
                        Dim pHW_AdjUSeal As New tblHW_AdjUSeal
                        pHW_AdjUSeal.fldPNID = gIPE_Project.PN_ID
                        pHW_AdjUSeal.fldRevID = gIPE_Project.PN_Rev_ID

                        pHW_AdjUSeal.fldDT = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsUSeal).T
                        pHW_AdjUSeal.fldDTheta1 = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsUSeal).DTheta(1)
                        pHW_AdjUSeal.fldDTheta2 = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsUSeal).DTheta(2)
                        pHW_AdjUSeal.fldDRad1 = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsUSeal).DRad(1)
                        pHW_AdjUSeal.fldDRad2 = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsUSeal).DRad(2)
                        pHW_AdjUSeal.fldDLLeg = CType(gIPE_Project.Analysis(pSelectedSealIndex).Seal, IPE_clsUSeal).DLLeg

                        pPartDBEntities.AddTotblHW_AdjUSeal(pHW_AdjUSeal)
                        pPartDBEntities.SaveChanges()
                    End If


                End If

                'pHWFace.fldAdjusted = gIPE_Project.Analysis(pSelectedSealIndex).Seal.Adjusted

                pPartDBEntities.AddTotblHW_Face(pHWFace)
                pPartDBEntities.SaveChanges()

                MessageBox.Show("Data Exported successfully.", "Data Export!", MessageBoxButtons.OK, MessageBoxIcon.None)
            End If

        Else
            MessageBox.Show("There is no Analysis selected for this project.", "Analysis not selected!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub
End Class