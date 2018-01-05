
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmDesignUSeal                         '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  22SEP16                                '
'                                                                              '
'===============================================================================
'
Imports System.Math
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports System.Linq
Imports clsLibrary11
Imports SealIPELib = SealIPELib101

Public Class IPE_frmDesignUSeal

#Region "MEMBER VARIABLE DECLARATION:"
    '===================================

    '....Shared Variables:  Initialize.
    Private Shared mFormOpened_FirstTime As Boolean = True
    Private Shared mSuccess_Candidate_CrossSecs As Boolean = False
    Private mFlag_DontFitCavity_Msg As Boolean = False

    Private mUSeal As IPE_clsUSeal              '....Local Seal object. 
    Private mMargin(4) As Single

    Private mfrmDesignCentreUSeal As IPE_frmDesignCenterUSeal

    Private mCrossSecList As New List(Of String)
    Private mCandidateList As New List(Of Boolean)

#End Region

    '....Constructor
    Public Sub New()
        '==========         
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        'AES 21SEP16
        With cmbCompressTolType
            .Items().Clear()
            .Items.Add("Nominal")
            .Items.Add("Minimum")
            .Items.Add("Maximum")
        End With


    End Sub


#Region "FORM EVENT ROUTINES:"

    Private Sub frmDesignUSeal_Load(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) Handles MyBase.Load
        '============================================================================

        '....This form load event is triggered only when the form is displayed by
        '........"Show Dialog", not when it is created and neither when control 
        '........comes back to this form after a subordinate form closes.
        '
        '
        '----------------------------------------------------------------------------
        '                   Picture Box Margins Calculation                         '
        '----------------------------------------------------------------------------
        '
        '....Uniform margin around the Picture Box 
        Dim pUniformMargin As Single = 0.05      '....Uniform margin around the
        '                                       '........Picture Box - (in)
        Set_Margin_PicBox(pUniformMargin, mMargin)

        'gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.PopulateMaterialList(cmbMatName)               '....Material List

        '   Update Working DB Table:        
        '   ========================
        '
        'If mFormOpened_FirstTime = True Then
        '    '   ....This update operation will be done once in every execution session.
        '    '
        '    '   ....This operation reads the records of each cross-sections from the CSealDB
        '    '   ........and repopulates the "CrossSecNo", "HfreeStd", "DHfreeAdjMax", "WidMax" 
        '    '   ........fields in the Working DB and initializes the "Candidate" field to "NO".
        '    '
        '    Update_WorkingDB()
        '    mFormOpened_FirstTime = False            '....Reset the FLAG.
        'End If

        'AES 29MAR16
        Dim pSealMCSDB As New SealIPEMCSDBEntities()
        mCrossSecList.Clear()
        mCandidateList.Clear()

        '....USealGeom
        'Dim pQryUSealGeom = (From it In pSealDBEntities.tblUSeal_Geom Order By it.fldCrossSecNo Ascending Select it).Distinct()

        'Dim pRecord As tblUSeal_Geom
        'For Each pRecord In pQryUSealGeom
        '    mCrossSecList.Add(pRecord.fldCrossSecNo)
        '    mCandidateList.Add(False)
        'Next

        Dim pQryUSealGeom = (From it In pSealMCSDB.tblUSeal_Geom Select it.fldCrossSecNo Distinct).ToList()
        For i As Integer = 0 To pQryUSealGeom.Count - 1
            mCrossSecList.Add(pQryUSealGeom(i))
            mCandidateList.Add(False)
        Next

        Update_Envelope()


        '   Update the "Candidate" field of the "CSealCandidate" table in the  
        '   ....Working DB. 
        '=======================================================================
        '
        If gUpdate_Candidate_CrossSecs = True Then
            '-------------------------------------

            '....The following flag indicates that a candidate design set has been found  
            '........in the database for the current cavity envelope.  

            'AES 28MAR16
            ''mSuccess_Candidate_CrossSecs = gIPE_SealCandidates.Update_Candidate_CrossSecs _
            ''                                           (gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.WidMin, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth)

            mSuccess_Candidate_CrossSecs = gIPE_SealCandidates.Update_Candidate_CrossSecs _
                                                       (gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.WidMin, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Depth, _
                                                        gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.HFree_Rep, mCandidateList)

            If mSuccess_Candidate_CrossSecs = False Then
                Display_Msg_NoCandidate_CrossSecs()
            End If

            '....Candidate selection has been done. 
            gUpdate_Candidate_CrossSecs = False         '....Reset the FLAG.

        End If

        InitializeControls()

        '....Create & initialize the local seal object.
        InitializeLocalObject()                   '....gSeal ===> mUSeal.


        'Set the AutoSelect checkbox "CheckState". 
        '-----------------------------------------
        '....The following assignment may or may not trigger the    
        '........"chkAutoSelect_CheckedChanged" event, which in turn calls 
        '........PopulateCmbCrossSec() & DisplayData()routines.
        '
        If gDisplay_Candidate_CrossSecs = True And mSuccess_Candidate_CrossSecs = True Then
            chkAutoSelect.CheckState = CheckState.Checked
        Else
            chkAutoSelect.CheckState = CheckState.Unchecked
        End If


        '....Polpulate Cross-section ComboBox.
        PopulateCmbCrossSec()

        '....Refresh Display upon entry to the form.
        '........Local seal object "mSeal" is used.
        DisplayData()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================
        lblUnitUserL.Text = "(" & gIPE_Unit.UserL & ")"
        cmdDesignCenter.Enabled = True

        lblNote.Visible = False

        If (gIPE_frmAnalysisSet.ModeOper = IPE_frmAnalysisSet.eModeOper.None) Then
            cmbCrossSecNo.Enabled = False
            cmbCrossSecNo_New.Enabled = False
            chkAutoSelect.Enabled = False
            txtHFreeTolPlus.Enabled = False
            txtHFreeTolMinus.Enabled = False
            cmbCompressTolType.Enabled = False
        Else
            cmbCrossSecNo.Enabled = True
            cmbCrossSecNo_New.Enabled = True
            chkAutoSelect.Enabled = True
            txtHFreeTolPlus.Enabled = True
            txtHFreeTolMinus.Enabled = True
            cmbCompressTolType.Enabled = True
        End If
        'txtZClear.Enabled = False

    End Sub


    Private Sub Update_Envelope()
        '========================

        Me.Cursor = Cursors.WaitCursor  '....Show Hourglass Cursor.

        If IPE_clsSealCandidates.CheckIfReqd_Populate_Envelope(gIPE_File, gIPE_User, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type) = True Then
            MessageBox.Show("Envelope for USeal is being populated. Please wait...", _
                       "Update U-Seal Envelope", MessageBoxButtons.OK, MessageBoxIcon.Information)
            IPE_clsSealCandidates.Populate_Envelope(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type)
        End If


        Me.Cursor = Cursors.Default     '....Restore the Default Cursor.

    End Sub


    Private Sub Display_Msg_NoCandidate_CrossSecs()
        '==========================================

        Dim pstrTitle, pstrMsg As String
        Dim pintAttributes As Integer

        pstrTitle = "Candidate CrossSection Selection"
        pintAttributes = MsgBoxStyle.OkOnly + MsgBoxStyle.Information

        '....No candidate cross-section set has been found.             
        '........All the cross-sections will be displayed.
        pstrMsg = "No candidate STD CrossSection is found for " & _
                  "the current cavity envelope." & Chr(Keys.Return) & _
                  "The menu will display all the available CrossSections."
        MsgBox(pstrMsg, pintAttributes, pstrTitle)

    End Sub


    Private Sub InitializeLocalObject()
        '==============================
        '....From gSeal ===> mUSeal. 
        '........Now onwards, mUSeal will be used within the code.
        '........Any change in the seal data will be saved on to the gSeal in the 
        '........"SaveData" routine which is called when the form is exited and another
        '........form is opened either modal or non-modal.     

        '....Create & initialize the local Seal Object.
        mUSeal = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        If gIPE_SealOrg Is Nothing Then
            gIPE_SealOrg = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        End If


        '....The following check and assignment resolves DR, V41, Error 12.
        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo = "" Then

            '....PRIMARY ASSIGNMENTS are not done.  
            '........Initialize with the first cross-section of the list.
            mUSeal.MCrossSecNo = IPE_clsUSeal.CrossSecList(0) 'IPE_clsUSeal.CrossSecList(0)

            '....Cross-section is assigned here locally and hence, no need to 
            '........show the message.
            mFlag_DontFitCavity_Msg = False

            '....SECONDARY ASSIGNMENTS:
            Dim i As Int16
            For i = 1 To 2

                If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity Is Nothing = False Then
                    mUSeal.CavityDia(i) = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Cavity.Dia(i)
                End If

            Next

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.MCrossSecNo <> "" Then
            mUSeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).Clone()
            gIPE_SealOrg = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).Clone()

            '....Cross-section is coming from the global variable and hence, 
            '........if necessary, show the message.
            mFlag_DontFitCavity_Msg = True
        End If

    End Sub


    Private Sub DisplayData()
        '====================
        '....This routine displays the current state of the local seal object "mUSeal". 

        'Cross-Section :
        '---------------
        With mUSeal 'CType(gSeal, IPE_clsUSeal) 
            cmbCrossSecNo.Text = .CrossSecNoOrg           '...."SelectedIndexChanged" Event.

            If .NewDesign Then
                'txtCrossSecNo_New.Text = .CrossSecNo
                cmbCrossSecNo_New.Text = .MCrossSecNo
                If IPE_clsUSeal.CrossSecNewList.Count > 0 And _
                        Not IPE_clsUSeal.CrossSecNewList.Contains(mUSeal.MCrossSecNo) Then
                    lblCrossSecNew.ForeColor = Color.Green
                Else
                    lblCrossSecNew.ForeColor = Color.Black
                End If
            Else
                If (cmbCrossSecNo_New.Items.Count > 0) Then
                    cmbCrossSecNo_New.SelectedIndex = 0
                End If

            End If

        End With

        '....Display all the relevant length parameters: 
        '........Adjusted, HfreeStd & Tolerances, Wid & DControl.    
        DisplayLengthParams()

        'AES 21SEP16
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        cmbCompressTolType.Text = gIPE_Project.Analysis(pICur).Compression.TolType
        DisplayCompressionVal()



        'Material Name
        '-----------------
        '....As the global variable "gMat" does not get modified while the form is open,
        '........no local "Material" object is used here.
        'If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Name <> "" Then
        '    cmbMatName.Text = gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Name
        'Else
        '    cmbMatName.SelectedIndex = 0
        'End If

    End Sub

#End Region


#End Region


#Region "CONTROL EVENT ROUTINES:"

#Region "LABEL RELATED ROUTINES:"


    'Private Sub lblAdjusted_TextChanged(ByVal sender As Object, _
    '                                    ByVal e As System.EventArgs)

    '    '===============================================================

    '    If lblAdjusted.Text = "N" Then
    '        lblAdjusted.ForeColor = Color.Magenta

    '    ElseIf lblAdjusted.Text = "Y" Then
    '        lblAdjusted.ForeColor = Color.Blue
    '    End If

    '    lblAdjusted.BackColor = Color.Gainsboro

    'End Sub

#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtHFreeTolMinus_TextChanged(ByVal sender As Object, _
                                              ByVal e As System.EventArgs) _
                                              Handles txtHFreeTolMinus.TextChanged
        '=========================================================================
        '....i = 1 : Minus Value
        mUSeal.HFreeTol(1) = gIPE_Unit.L_UserToCon(txtHFreeTolMinus.Text)
        SetForeColor_HfreeTol()
        DisplayCompressionVal()     'AES 22SEP16

    End Sub


    Private Sub txtHFreeTolPlus_TextChanged(ByVal sender As Object, _
                                            ByVal e As System.EventArgs) _
                                            Handles txtHFreeTolPlus.TextChanged
        '=========================================================================
        '....i = 2 : Plus Value
        mUSeal.HFreeTol(2) = gIPE_Unit.L_UserToCon(txtHFreeTolPlus.Text)
        SetForeColor_HfreeTol()
        DisplayCompressionVal()     'AES 22SEP16

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub SetForeColor_HfreeTol()
        '==============================                 

        With mUSeal

            Dim pColor As Color

            pColor = IIf(Abs(.HFreeTol(1) - .HfreeTolStd) < gcEPS, _
                                             Color.Magenta, Color.Black)
            txtHFreeTolMinus.ForeColor = pColor

            pColor = IIf(Abs(.HFreeTol(2) - .HfreeTolStd) < gcEPS, _
                                            Color.Magenta, Color.Black)
            txtHFreeTolPlus.ForeColor = pColor

        End With

    End Sub

#End Region


#End Region


#Region "CHECK BOX RELATED ROUTINES:"

    Private Sub chkAutoSelect_CheckedChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles chkAutoSelect.CheckedChanged
        '=========================================================================

        'Show Criteria Label    
        '-------------------
        lblShowCriteria.Visible = chkAutoSelect.Checked

        If chkAutoSelect.CheckState = CheckState.Checked And _
           mSuccess_Candidate_CrossSecs = False Then

            Display_Msg_NoCandidate_CrossSecs()
            chkAutoSelect.CheckState = CheckState.Unchecked
            Exit Sub
        End If


        If chkAutoSelect.CheckState = CheckState.Checked Then
            '------------------------------------------------
            '
            gDisplay_Candidate_CrossSecs = True

            With cmdForm_SelectionCriteria
                .Enabled = True
                .Text = "Candidates Only"
                .TextAlign = ContentAlignment.MiddleCenter
            End With


        ElseIf chkAutoSelect.CheckState = CheckState.Unchecked Then
            '------------------------------------------------------

            gDisplay_Candidate_CrossSecs = False

            With cmdForm_SelectionCriteria
                .Enabled = False
                .Text = "All Cross Sections"
                .TextAlign = ContentAlignment.MiddleCenter
            End With

        End If

        PopulateCmbCrossSec()

        '....Display the current state of the local seal object.
        DisplayData()

    End Sub

#End Region


#Region "COMBO-BOX RELATED CONTROLS"

    Private Sub cmbCrossSec_SelectedIndexChanged(ByVal sender As System.Object, _
                                                 ByVal e As System.EventArgs) _
                                                 Handles cmbCrossSecNo.SelectedIndexChanged
        '================================================================================

        If mUSeal Is Nothing = True Then Exit Sub

        With mUSeal

            If cmbCrossSecNo.Text <> .CrossSecNoOrg Then

                '....The following assignment resets all the length parameters to the 
                '........standard values for the selected "CrossSecNo". The above check
                '........makes sure that the "CrossSecNo" selected is different from that
                '........of the local seal object, only when resetting will be effected.
                '
                .MCrossSecNo = cmbCrossSecNo.Text        '....Sets .NewDesign = False.
                'txtCrossSecNo_New.Text = ""

            End If

            DisplayLengthParams()      '....Update Length Parameters Display.

            DoGraphics()

            'AES 21SEP16
            'Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
            'Dim pVal As Double = mUSeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
            'Dim pPCentVal As Double = (pVal / mUSeal.Hfree) * 100.0#

            'lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

            DisplayCompressionVal()

        End With

        PopulateCmbCrossSec_New()

    End Sub


    Private Sub cmbCrossSecNo_New_SelectedIndexChanged(ByVal sender As System.Object, _
                                                       ByVal e As System.EventArgs) _
                                                       Handles cmbCrossSecNo_New.SelectedIndexChanged
        '=============================================================================================

        Dim pCrossSecNoOrg As String = cmbCrossSecNo.Text.Trim()
        Dim pCrossSecNoNew As String = cmbCrossSecNo_New.Text.Trim()

        lblNote.Visible = False

        With mUSeal
            .MCrossSecNo = pCrossSecNoNew
            .CrossSecNoOrg = pCrossSecNoOrg
        End With

        DisplayLengthParams()      '....Update Length Parameters Display.

        DoGraphics()

        DisplayCompressionVal()


    End Sub


    Private Sub cmbCrossSecNo_New_TextChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles cmbCrossSecNo_New.TextChanged
        '============================================================================
        Dim pCrossSecNo As String = cmbCrossSecNo_New.Text.Trim()

        If IPE_clsUSeal.CrossSecNewList.Count > 0 And _
                Not IPE_clsUSeal.CrossSecNewList.Contains(pCrossSecNo) Then

            'cmbCrossSecNo_New.ForeColor = Color.Green
            lblCrossSecNew.ForeColor = Color.Green

            mUSeal = CType(gIPE_SealOrg, IPE_clsUSeal).Clone()
            DisplayLengthParams()      '....Update Length Parameters Display.

            DoGraphics()

            With lblNote
                .Visible = True
                .Text = "Original CrossSection"
            End With

        Else
            'cmbCrossSecNo_New.ForeColor = Color.Black
            lblCrossSecNew.ForeColor = Color.Black
            cmbCrossSecNo_New_SelectedIndexChanged(sender, e)

        End If
    End Sub


    Private Sub cmbCompressTolType_SelectedIndexChanged(sender As System.Object,
                                                        e As System.EventArgs) Handles cmbCompressTolType.SelectedIndexChanged
        '======================================================================================================================
        DisplayCompressionVal()

    End Sub


#Region "HELPER ROUTINES:"

    '....New Cross Section Number                   
    Private Sub PopulateCmbCrossSec_New()
        '================================

        Dim pSealMCSDB As New SealIPEMCSDBEntities()

        Dim pCrossSecNo As String = cmbCrossSecNo.Text.Trim()
        'Dim pQryUSealCandidate = (From pRec In pSealNewDB.tblUSeal_NewGeom
        '                            Where pRec.fldCrossSecNoOrg = pCrossSecNo
        '                            Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()

        Dim pQryUSealCandidate = (From pRec In pSealMCSDB.tblUSealNew_Geom
                                   Where pRec.fldCustID = gIPE_Project.Customer_ID And
                                         pRec.fldPlatformID = gIPE_Project.Platform_ID And
                                         pRec.fldProjectID = gIPE_Project.Project_ID And
                                         pRec.fldCrossSecNoOrg = pCrossSecNo
                                   Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()

        Dim pRecord As New tblUSealNew_Geom
        cmbCrossSecNo_New.Items.Clear()
        For Each pRecord In pQryUSealCandidate
            cmbCrossSecNo_New.Items.Add(pRecord.fldCrossSecNo)
        Next

    End Sub


    Private Sub DisplayCompressionVal()
        '==============================
        'AES 22SEP16
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur
        Dim pVal As Double = mUSeal.HActual("Initial", cmbCompressTolType.Text) - gIPE_Project.Analysis(pICur).Cavity.DepthActual(cmbCompressTolType.Text)
        Dim pPCentVal As Double = (pVal / mUSeal.Hfree) * 100.0#

        lblComp.Text = gIPE_Unit.WriteInUserL(pVal) + " (" + pPCentVal.ToString("#00.0") + "%)"

    End Sub

#End Region

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdForm_SelectionCriteria_EnabledChanged _
                                (ByVal sender As Object, ByVal e As System.EventArgs) _
                                 Handles cmdForm_SelectionCriteria.EnabledChanged
        '==============================================================================

        With cmdForm_SelectionCriteria

            If .Enabled = True Then
                .Text = "Auto-Select"
                .TextAlign = ContentAlignment.MiddleCenter

            ElseIf .Enabled = False Then
                .Text = "No Auto-Select"
                .TextAlign = ContentAlignment.MiddleRight
            End If

        End With

    End Sub


    Private Sub cmdForm_SelectionCriteria_Click(ByVal sender As System.Object, _
                                                ByVal e As System.EventArgs) _
                                                Handles cmdForm_SelectionCriteria.Click
        '===============================================================================
        'gfrmSelectionCriteria.ShowDialog()
        Dim pfrmSelectionCriteria As New IPE_frmSelectionCriteria()
        pfrmSelectionCriteria.ShowDialog()

    End Sub


    Private Sub cmdDesignCenter_Click(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles cmdDesignCenter.Click
        '==============================================================

        Dim pblnSave As Boolean
        Check_CrossSecNo(pblnSave)
        If pblnSave = False Then Exit Sub


        '   ORIGINAL SEAL OBJECT:
        '   ---------------------
        '   ....Create it, if it doesn't already exist, and initialize it. 
        '
        If gIPE_SealOrg Is Nothing Then
            gIPE_SealOrg = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)

        ElseIf gIPE_SealOrg.UnitSystem <> gIPE_Unit.System Then
            gIPE_SealOrg = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
            CType(gIPE_SealOrg, IPE_clsUSeal).MCrossSecNo = cmbCrossSecNo.Text
        End If

        'If mUSeal.NewDesign = False Then _         
        CType(gIPE_SealOrg, IPE_clsUSeal).MCrossSecNo = cmbCrossSecNo.Text

        '    New Seal Object:
        '   ------------------
        If gIPE_SealNew Is Nothing Then
            gIPE_SealNew = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        ElseIf gIPE_SealNew.Type <> gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type Then
            gIPE_SealNew = New IPE_clsUSeal("U-Seal", gIPE_Unit.System, gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).OpCond.POrient)
        End If
        gIPE_SealNew = mUSeal.Clone()
        CType(gIPE_SealNew, IPE_clsUSeal).MCrossSecNo = cmbCrossSecNo_New.Text

        '   Save data.
        '   ----------
        SaveData()

        '....Save the Design State before entering the "Design Centre" FORM.
        Dim pNewDesign_Before As Boolean
        pNewDesign_Before = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).NewDesign

        '   OPEN "Design Centre" FORM. 
        '   --------------------------
        mfrmDesignCentreUSeal = New IPE_frmDesignCenterUSeal()
        mfrmDesignCentreUSeal.ShowDialog()

        '   RETURNED FROM "Design Center":
        '   ------------------------------
        '....Design State on return.
        Dim pNewDesign_After As Boolean
        pNewDesign_After = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).NewDesign

        '   ....Upon returning (may be with a new Cross-section), update the local
        '   ........seal object. gSeal ===> mUSeal.
        mUSeal = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).Clone

        If IPE_clsUSeal.CrossSecNewList.Count > 0 And _
             Not IPE_clsUSeal.CrossSecNewList.Contains(mUSeal.MCrossSecNo) Then
            IPE_clsUSeal.CrossSecNewList.Add(mUSeal.MCrossSecNo)
        End If

        DisplayData()

        DoGraphics()
        picThumbnail.Refresh()

    End Sub


    Private Sub cmdAdjustGeometry_Click(ByVal sender As System.Object, _
                                        ByVal e As System.EventArgs) _
                                        Handles cmdAdjustGeometry.Click
        '====================================================================
        ''....Save before opening frmAdjGeomUSeal form
        'SaveData()

        'Dim pblnError As Boolean = False    '....Initialize
        ''gDataValidate.CheckForNullCrossSectionNo(pblnError)
        'gDataValidate.CheckForNullCrossSectionNo(gSeal, pblnError) 

        ''If pblnError = False Then gfrmAdjGeomUSeal.ShowDialog()

        ''RETURNED FROM "frmAdjGeomUSeal":
        ''--------------------------------
        ''....Upon returning from the Adjust Geometry Form, update the corresponding 
        ''........geometry adjustment parameters. gSeal ===> mUSeal.
        'mUSeal = CType(gSeal, IPE_clsUSeal).Clone

        ''....Update Length Parameters Display.
        'DisplayLengthParams()

    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '====================================================================
        Dim pCmdBtn As Button = CType(sender, Button)
        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
        End If

        Me.Close()

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub Check_CrossSecNo(ByRef blnSave_Out As Boolean)
        '======================================================
        '....Initialize.
        blnSave_Out = True

        Dim pCrossSec As String = cmbCrossSecNo_New.Text
        Dim pMsg As String = ""
        Dim pintAnswer As Integer

        If IPE_clsUSeal.CrossSecList.Contains(pCrossSec) Then
            '....Exists in STD CrossSection List
            pMsg = "The NEW CrossSection No. must not be same as the existing STD CrossSection no." & _
                   vbCrLf & "Please assign an appropriate number."
            pintAnswer = MessageBox.Show(pMsg, "Data Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            If pintAnswer = Windows.Forms.DialogResult.OK Then
                cmbCrossSecNo_New.Focus()
                blnSave_Out = False
                Exit Sub
            End If

        End If

    End Sub


    Private Sub SaveData()
        '=================
        Dim pICur As Integer = gIPE_frmAnalysisSet.ICur

        'MAIN SEAL OBJECT:
        '-----------------
        With CType(gIPE_Project.Analysis(pICur).Seal, IPE_clsUSeal)

            If Not .NewDesign And Not mUSeal.NewDesign Then
                '   CASE 1:  "Standard Design"
                '    ------
                If .MCrossSecNo <> cmbCrossSecNo.Text Then .MCrossSecNo = cmbCrossSecNo.Text


            ElseIf .NewDesign And mUSeal.NewDesign Then
                '   CASE 2: "New Design"
                '   -------
                '   ....The following conditions will be automatically satisfied:
                '        i) .CrossSecNoOrg = cmbCrossSec.Text 
                '       ii) .CrossSecNo    = txtCrossSecNo_New (Read-only text box).
                '
                '   ....No need to do anything.
                .MCrossSecNo = cmbCrossSecNo_New.Text
                .CrossSecNoOrg = cmbCrossSecNo.Text

            ElseIf .NewDesign And Not mUSeal.NewDesign Then
                '   CASE 3: Toggled from "New Design" to "Standard Design" on the form.
                '   ------
                .MCrossSecNo = cmbCrossSecNo.Text

            ElseIf Not .NewDesign And mUSeal.NewDesign Then
                '   CASE 4: Toggled from "Standard Design" to "New Design" on the form.
                '   --------
                .MCrossSecNo = cmbCrossSecNo_New.Text
                .CrossSecNoOrg = cmbCrossSecNo.Text


            End If

            'If .CrossSecNo <> cmbCrossSecNo.Text Then .CrossSecNo = cmbCrossSecNo.Text

            .HFreeTol(1) = gIPE_Unit.L_UserToCon(txtHFreeTolMinus.Text)  ' i = 1 : Minus Value
            .HFreeTol(2) = gIPE_Unit.L_UserToCon(txtHFreeTolPlus.Text)   ' i = 2 : Plus Value

        End With

        'gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Mat.Name = cmbMatName.Text


        'SECONDARY ASSIGNMENTS:
        '----------------------     
        If gIPE_Project.Analysis(pICur).Seal Is Nothing = False Then
            gIPE_Project.Analysis(pICur).Cavity.CornerRad = IIf(Abs(gIPE_Project.Analysis(pICur).Cavity.CornerRad - gIPE_Project.Analysis(pICur).Seal.CavityCornerRad) < gcEPS, _
                                                                   gIPE_Project.Analysis(pICur).Seal.CavityCornerRad, gIPE_Project.Analysis(pICur).Cavity.CornerRad)
        End If

        'AES 21SEP16
        gIPE_Project.Analysis(pICur).CompressionTolType = cmbCompressTolType.Text

    End Sub

#End Region

#End Region


#Region "UTILITY ROUTINES:"

    Private Sub DisplayLengthParams()
        '============================

        With mUSeal
            'lblAdjusted.Text = .Adjusted
            lblHFree.Text = gIPE_Unit.WriteInUserL(.Hfree)
            lblHFree.BackColor = Color.Gainsboro
            txtHFreeTolMinus.Text = gIPE_Unit.WriteInUserL(.HFreeTol(1))
            txtHFreeTolPlus.Text = gIPE_Unit.WriteInUserL(.HFreeTol(2))

            If (Math.Abs(mUSeal.ZClear_Given) > gcEPS) Then
                lblZClear.Text = gIPE_Unit.WriteInUserL(mUSeal.ZClear_Given)
                lblZClear.ForeColor = Color.Black
                lblZClear.BackColor = Color.Gainsboro
            Else
                lblZClear.Text = gIPE_Unit.WriteInUserL(.ZClear)
                lblZClear.ForeColor = Color.Blue
                lblZClear.BackColor = Color.Gainsboro
            End If

            lblWid.Text = gIPE_Unit.WriteInUserL(.Wid)
            lblWid.BackColor = Color.Gainsboro
            lblDControl.Text = gIPE_Unit.WriteInUserL(.DControl)
            lblDControl.BackColor = Color.Gainsboro

            lblThick.Text = gIPE_Unit.WriteInUserL(.T)
            lblThick.BackColor = Color.Gainsboro
        End With

    End Sub


    Private Sub PopulateCmbCrossSec()
        '============================

        If chkAutoSelect.CheckState = CheckState.Checked Then
            cmbCrossSecNo.Items.Clear()
            For i As Integer = 0 To mCrossSecList.Count - 1
                If (mCandidateList(i) = True) Then
                    cmbCrossSecNo.Items.Add(mCrossSecList(i))
                End If
            Next

        Else
            cmbCrossSecNo.Items.Clear()
            For i As Integer = 0 To mCrossSecList.Count - 1
                cmbCrossSecNo.Items.Add(mCrossSecList(i))
            Next

        End If

    End Sub

#End Region

#Region "GRAPHICS ROUTINES:"


    Private Sub DoGraphics()
        '===================        

        'This routine draws the 'Standard' & 'Adjusted' Geometries.

        '....Drawing envelope:
        Dim EnvpTopL As PointF
        Dim EnvpBotR As PointF


        'Graphics Settings:
        '------------------
        '....Array Index = 0 ===> "Standard Geometry"
        '....Array Index = 1 ===> "Adjusted Geometry"

        '....Color:
        Dim pColor(1) As Color
        pColor(0) = Color.Black
        pColor(1) = Color.Blue

        '....Drawing Width (Pixels)  
        Dim pDrawWid(1) As Integer
        pDrawWid(0) = 1
        pDrawWid(1) = 1

        '....Dash Style:
        Dim pDashStyle(1) As Integer
        pDashStyle(0) = DashStyle.Solid     '....Value = 0
        pDashStyle(1) = DashStyle.DashDot   '....Value = 1    


        'Draw the seals.
        '---------------
        Dim pGr As Graphics = GetGraphicsObj(picThumbnail)

        '....Pixel densities per unit "PageUnit" dimension (in or mm)
        Dim pDpX As Single
        Dim pDpY As Single

        '....Set the PageUnit property.
        If gIPE_Unit.System = "English" Then
            pGr.PageUnit = GraphicsUnit.Inch

            '....# of Pixels/in
            pDpX = pGr.DpiX
            pDpY = pGr.DpiY

        ElseIf gIPE_Unit.System = "Metric" Then
            pGr.PageUnit = GraphicsUnit.Millimeter

            '....# of Pixels/mm
            pDpX = pGr.DpiX / gIPE_Unit.EngLToUserL(1.0)
            pDpY = pGr.DpiY / gIPE_Unit.EngLToUserL(1.0)
        End If


        '....Size of the graphics area in the "page unit" system.
        Dim pSize As New SizeF(picThumbnail.Width / pDpX, picThumbnail.Height / pDpY)

        '....Draw "Standard" Seal Geometry.       
        mUSeal.Draw(pGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                    "STD", "SCALE_BY_STD", 1.25, _
                                     EnvpTopL, EnvpBotR)

        picThumbnail.Refresh()

    End Sub

#End Region


#End Region



End Class