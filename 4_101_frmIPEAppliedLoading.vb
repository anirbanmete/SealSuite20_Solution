'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmAppliedLoading                      '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  16MAY16                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports clsLibrary11
Imports System.Linq

Public Class IPE_frmAppliedLoading

#Region "MEMBER VARIABLES:"

    ''Private mProjectEntity As New ProjectDBEntities()
    ''Private mSealEntity As New SealMCSDBEntities()

    Private mAppLoad As New IPE_clsAppLoad                    '....Local AppLoad Object.  
    Private mSel_RowIndx As Integer

#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmAppliedLoading_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==================================================================================================
        chkPreComp.Checked = False
        chkRadCon.Checked = True
        InitializeLocalObject()
        DisplayData()

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            chkPreComp.Enabled = False
            txtPreCompressedH.Enabled = False

            grdLoadSteps.Enabled = False
            chkRadCon.Enabled = False

            cmdInsert.Enabled = False
            cmdDelete.Enabled = False
        Else
            chkPreComp.Enabled = True
            txtPreCompressedH.Enabled = True

            grdLoadSteps.Enabled = True
            chkRadCon.Enabled = True

            cmdInsert.Enabled = True
            cmdDelete.Enabled = True
        End If

        If ((gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.IsSegmented)) Then
            chkRadCon.Checked = True
            chkRadCon.Enabled = False
        Else
            chkRadCon.Enabled = True
        End If

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '==============================

        '....Instantiate Local AppLoad Object. 
        mAppLoad = New IPE_clsAppLoad()

        With mAppLoad
            .PreComp_Exits = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.PreComp.Exists
            '.PreCompressed = gAppLoad.PreCompressed

            If (.PreComp.Exists) Then
                .PreComp_HMin = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.PreComp.HMin
            End If

            For i As Integer = 0 To gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.AddLoad.Count - 1
                .AddLoad.Add(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.AddLoad(i))
            Next

            .RadConstraint = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.RadConstraint

        End With

    End Sub


    Private Sub DisplayData()
        '====================

        With mAppLoad
            chkPreComp.Checked = .PreComp.Exists

            If (.PreComp.Exists) Then
                txtPreCompressedH.Text = ConvertToStr(.PreComp.HMin, "#0.000")
            End If

            grdLoadSteps.Rows.Clear()
            With grdLoadSteps
                .Rows.Add()
                .Rows(0).Cells(0).Value = "BL"
                .Rows(0).Cells(1).Value = gIPE_Unit.FormatPDiffUnitUser(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.PDiff)
                .Rows(0).Cells(2).Value = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.TOper
                .Rows(0).Cells(3).Value = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth)
                .Rows(0).Selected = True

                .Rows(0).ReadOnly = True
                .Rows(0).DefaultCellStyle.BackColor = Color.LightGray
            End With

            For i As Integer = 0 To mAppLoad.AddLoad.Count - 1
                With grdLoadSteps
                    .Rows.Add()
                    .Rows(i + 1).Cells(0).Value = "A" + (i + 1).ToString()

                    .Rows(i + 1).Cells(1).Value = gIPE_Unit.FormatPDiffUnitUser(mAppLoad.AddLoad(i).PDiff)
                    .Rows(i + 1).Cells(2).Value = mAppLoad.AddLoad(i).TOper
                    .Rows(i + 1).Cells(3).Value = gIPE_Unit.WriteInUserL(mAppLoad.AddLoad(i).CavityDepth)
                    .Rows(0).Selected = True
                    .Rows(i + 1).Cells(0).ReadOnly = True
                End With
            Next
            grdLoadSteps.Rows(0).ReadOnly = True

            chkRadCon.Checked = .RadConstraint

        End With

    End Sub

#End Region

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"

    Private Sub cmdButton_Click(sender As System.Object, e As System.EventArgs) _
                                Handles cmdInsert.Click, cmdDelete.Click, cmdOK.Click, cmdCancel.Click
        '=============================================================================================
        Dim pButton As Button = CType(sender, Button)
        Dim pCurIndx As Integer = 0

        If (IsNothing(grdLoadSteps.CurrentRow)) Then
            pCurIndx = 0
        Else
            pCurIndx = grdLoadSteps.CurrentRow.Index
        End If

        Select Case pButton.Name

            Case "cmdInsert"
                If (grdLoadSteps.Rows.Count > 1) Then

                    Dim pAddLoad As New IPE_clsAppLoad.sAddLoad

                    pAddLoad.PDiff = 0
                    pAddLoad.TOper = 0
                    pAddLoad.CavityDepth = 0

                    If (pCurIndx > mAppLoad.AddLoad.Count) Then
                        mAppLoad.AddLoad.Add(pAddLoad)
                    Else
                        mAppLoad.AddLoad.Insert(pCurIndx, pAddLoad)
                    End If

                    DisplayData()

                End If

            Case "cmdDelete"
                If (pCurIndx = 0) Then
                    MessageBox.Show("Baseline Load Step can't be deleted", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                mAppLoad.AddLoad.RemoveAt(pCurIndx - 1)
                DisplayData()

            Case "cmdOK"
                SaveData()
                Me.Hide()

            Case "cmdCancel"
                Me.Close()

        End Select

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '==================

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.PreComp_Exits = mAppLoad.PreComp.Exists
        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.PreComp_HMin = mAppLoad.PreComp.HMin

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.AddLoad.Clear()
        For i As Integer = 0 To mAppLoad.AddLoad.Count - 1
            gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.AddLoad.Add(mAppLoad.AddLoad(i))
        Next

        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).AppLoad.RadConstraint = mAppLoad.RadConstraint

    End Sub

#End Region

#End Region


#Region "CONTROL EVENT ROUTINES:"

#Region "CHECKBOX CHECKED EVENT ROUTINES:"

    Private Sub chkPreComp_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                          Handles chkPreComp.CheckedChanged
        '=====================================================================================
        If (chkPreComp.Checked) Then
            lblHMin.Visible = True
            txtPreCompressedH.Visible = True
            txtPreCompressedH.Focus()
        Else
            lblHMin.Visible = False
            txtPreCompressedH.Visible = False
        End If

        mAppLoad.PreComp_Exits = chkPreComp.Checked

    End Sub

    Private Sub chkRadCon_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                         Handles chkRadCon.CheckedChanged
        '==================================================================================
        mAppLoad.RadConstraint = chkRadCon.Checked
    End Sub

#End Region


#Region "GRIDVIEW EVENT ROUTINES:"

    Private Sub grdLoadSteps_CellEndEdit(sender As System.Object, _e As System.Windows.Forms.DataGridViewCellEventArgs) _
                                         Handles grdLoadSteps.CellEndEdit
        '===============================================================================================================
        Dim pCurIndx As Integer = 0

        If (IsNothing(grdLoadSteps.CurrentRow)) Then
            Exit Sub
        Else
            pCurIndx = grdLoadSteps.CurrentRow.Index
        End If

        If (pCurIndx = 0) Then
            MessageBox.Show("Baseline Load Step can't be edited", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim pAddLoad As New IPE_clsAppLoad.sAddLoad

        pAddLoad.PDiff = Convert.ToSingle(grdLoadSteps.Rows(pCurIndx).Cells(1).Value) * gIPE_Unit.CFacUserP
        pAddLoad.TOper = Convert.ToDouble(grdLoadSteps.Rows(pCurIndx).Cells(2).Value)
        pAddLoad.CavityDepth = gIPE_Unit.L_UserToCon(Convert.ToSingle(grdLoadSteps.Rows(pCurIndx).Cells(3).Value))

        mAppLoad.AddLoad(pCurIndx - 1) = pAddLoad

    End Sub

#End Region


#Region "TEXTBOX TEXTCHANGE EVENT ROUTINES:"

    Private Sub txtPreCompressedH_TextChanged(sender As System.Object, e As System.EventArgs) _
                                              Handles txtPreCompressedH.TextChanged
        '========================================================================================
        If (mAppLoad.PreComp.Exists) Then
            mAppLoad.PreComp_HMin = Convert.ToDouble(txtPreCompressedH.Text)
        End If
    End Sub

#End Region

#End Region


End Class