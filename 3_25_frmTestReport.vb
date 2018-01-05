'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Report                         '
'                        VERSION NO  :  2.6                                 '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29AUG17                                '
'                                                                              '
'===============================================================================
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports System.IO
Imports clsLibrary11

Public Class Test_frmReport

#Region "MEMBER VARIABLES:"

    Private mTestProject As New Test_clsProject(gPartProject)
    Private mControl() As Control

#End Region


#Region "FORM LOAD EVENT ROUTINES:"


    Private Sub frmTest_Report_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===============================================================================================
        Me.Text = "SealTest: Report Form"
        mTestProject = gTest_Project.Clone()
        mControl = {clbImage, txtDrawing, cmdBrowse, cmdAdd}
        'mControl = {clbImage, txtDrawing}

        InitializeControls()
        DisplayData()
    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeControls()
        '===========================

        '....Set all controls read-only.
        For i As Integer = 0 To mControl.Count - 1
            mControl(i).Enabled = False
        Next

        '....Set the controls (whether read-only or not) according to the gTest_User.Role
        For i As Integer = 0 To mControl.Count - 1
            Dim pVal As String = mControl(i).Tag

            If (gTest_User.Role = Test_clsUser.eRole.Admin) Then
                mControl(i).Enabled = True

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (pVal.Contains("ADef") Or pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Eng) Then

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            ElseIf (gTest_User.Role = Test_clsUser.eRole.Quality) Then

                If (pVal.Contains(gTest_User.Role.ToString())) Then
                    mControl(i).Enabled = True
                End If

            End If
        Next

    End Sub

#End Region

#End Region


#Region "UTILITY ROUTINE:"

    Private Sub DisplayData()
        '=====================

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then

                'AES 21FEB17
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).RetrieveFrom_DB(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)

                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SetTestStatus(mTestProject.Test_Spec.LeakMax.Unplated, mTestProject.Test_Spec.LeakSpringBackMin.Unplated, mTestProject.Test_Spec.LoadType, mTestProject.Test_Spec.LoadVal(1), mTestProject.Test_Spec.LoadVal(0))


                'If (mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).DetermineOverallStatus() = clsTest_Seal.eStatus.Pass) Then

                '    grpImage.Enabled = True

                '    With grpReport
                '        .Text = "Report"
                '        .ForeColor = Color.FromArgb(0, 192, 0)
                '    End With

                '    clbImage.Items.Clear()
                '    For i As Integer = 0 To mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Image.Count - 1
                '        clbImage.Items.Add(mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Image(i).NameTag)
                '    Next

                '    If (clbImage.Items.Count > 0) Then
                '        clbImage.SelectedIndex = 0
                '    End If

                '    cmdWORD.Enabled = True
                '    cmdPDF.Enabled = True

                'ElseIf (mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).DetermineOverallStatus() = clsTest_Seal.eStatus.Fail) Then

                '    grpImage.Enabled = False

                '    With grpReport
                '        .Text = "Reject Report"
                '        .ForeColor = Color.Red
                '    End With

                '    clbImage.Items.Clear()
                '    For i As Integer = 0 To mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Image.Count - 1
                '        clbImage.Items.Add(mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Gen.Image(i).NameTag)
                '    Next

                '    cmdWORD.Enabled = False
                '    cmdPDF.Enabled = False
                'End If

                'AES 02MAR17

                grpImage.Enabled = True

                'With grpReport
                '    .Text = "Report"
                '    '.ForeColor = Color.FromArgb(0, 192, 0)
                '    .ForeColor = Color.FromArgb(255, 0, 0)
                'End With

                optTest.Checked = True

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DetermineOverallStatus() = Test_clsSeal.eStatus.Pass) Then

                    With grpReport
                        .Text = "Report"
                        .ForeColor = Color.FromArgb(0, 192, 0)
                    End With
                    optRejection.Visible = False

                ElseIf (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DetermineOverallStatus() = Test_clsSeal.eStatus.Fail) Then

                    With grpReport
                        .Text = "Report"
                        .ForeColor = Color.Red
                    End With
                    optRejection.Visible = True
                End If

                clbImage.Items.Clear()
                For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Count - 1
                    clbImage.Items.Add(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).NameTag)
                Next

                If (clbImage.Items.Count > 0) Then
                    clbImage.SelectedIndex = 0
                End If

                cmdWORD.Enabled = True
                cmdPDF.Enabled = True

            End If
        End If

    End Sub


    Private Sub SaveData()
        '=================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Count - 1
                    Dim pImage As New Test_clsReport.sGenImage
                    pImage.File = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).File
                    pImage.NameTag = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).NameTag
                    pImage.Caption = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).Caption
                    pImage.Image = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).Image

                    If (clbImage.GetItemCheckState(i)) Then
                        pImage.Selected = True
                    Else
                        pImage.Selected = False

                    End If
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i) = pImage
                Next
            End If
        End If
        gTest_Project = mTestProject.Clone()

    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINE:"

    Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
        '==============================================================================================
        With openFileDialog1
            .Filter = "SealTest Drawing File PDF|*.pdf"
            .FilterIndex = 1
            .InitialDirectory = "C:"
            .FileName = ""
            .Title = "Open"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtDrawing.Text = openFileDialog1.FileName
            End If

        End With
    End Sub

    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        '=========================================================================================
        Try

            If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                    If (txtDrawing.Text <> "") Then
                        Dim pImage As New Test_clsReport.sGenImage
                        pImage.File = txtDrawing.Text.Trim()

                        Dim pFileTitle As String = Path.GetFileNameWithoutExtension(txtDrawing.Text.Trim())
                        pImage.NameTag = pFileTitle
                        pImage.Caption = ""
                        pImage.Image = Nothing
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Add(pImage)
                        txtDrawing.Text = ""
                        clbImage.Items.Add(pFileTitle)
                        clbImage.SetSelected(clbImage.Items.IndexOf(pFileTitle), True)

                        Dim pSealTestEntities As New SealTestDBEntities
                        Dim pGenImage As New tblReportGenImage

                        With pGenImage
                            .fldTestProjectID = mTestProject.ID
                            .fldTestMOID = mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID
                            .fldTestRptID = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ID
                            .fldImageID = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Count
                            .fldImageFile = pImage.File
                            .fldImageNameTag = pFileTitle
                            .fldImageCaption = ""
                            '.fldImage = Nothing
                            .fldSelected = True

                        End With

                        pSealTestEntities.AddTotblReportGenImage(pGenImage)
                        pSealTestEntities.SaveChanges()
                    End If
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub


    Private Sub cmdWORD_Click(sender As System.Object, e As System.EventArgs) Handles cmdWORD.Click
        '===========================================================================================
        SaveData()
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                'mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).Create(mTestProject)
                Dim pFileName As String = ""
                saveFileDialog1.FilterIndex = 1
                saveFileDialog1.Filter = "WORD files (*.DOC)|*.doc"
                saveFileDialog1.Title = "Save"
                saveFileDialog1.FileName = ""

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                    Cursor = Cursors.WaitCursor
                    pFileName = saveFileDialog1.FileName

                    'If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DetermineOverallStatus() = Test_clsSeal.eStatus.Pass) Then
                    '    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ReportType = Test_clsReport.eReportType.Normal
                    'ElseIf (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).DetermineOverallStatus() = Test_clsSeal.eStatus.Fail) Then
                    '    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ReportType = Test_clsReport.eReportType.Rejection
                    'End If

                    'AES 20MAR17
                    If (optTest.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ReportType = Test_clsReport.eReportType.Normal
                    ElseIf (optRejection.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ReportType = Test_clsReport.eReportType.Rejection
                    End If

                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Create(mTestProject, gTest_frmMain.MO_Sel, gTest_frmMain.Report_Sel, "WORD", pFileName)
                    Cursor = Cursors.Default
                End If

            End If
        End If

    End Sub


    Private Sub cmdPDF_Click(sender As System.Object, e As System.EventArgs) Handles cmdPDF.Click
        '=========================================================================================
        SaveData()

        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                Dim pFileName As String = ""
                saveFileDialog1.FilterIndex = 1
                saveFileDialog1.Filter = "PDF files (*.PDF)|*.PDF"
                saveFileDialog1.Title = "Save"

                'AES 02MAR17
                Dim pCloseDate As String = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).GetClosedDate().ToString("ddMMMyy")
                saveFileDialog1.FileName = "Report_" & mTestProject.Test_MO(gTest_frmMain.MO_Sel).No & "_" & mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).No & "_" & pCloseDate & ".pdf" '"" 'gTest_File.DirOut & "Report" & ".PDF"

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                    Cursor = Cursors.WaitCursor
                    pFileName = saveFileDialog1.FileName
                    'If (mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).DetermineOverallStatus() = clsTest_Seal.eStatus.Pass) Then
                    '    mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).ReportType = clsTest_Report.eReportType.Normal
                    'ElseIf (mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).DetermineOverallStatus() = clsTest_Seal.eStatus.Fail) Then
                    '    mTestProject.Test_MO(gfrmTestMain.MO_Sel).Report(gfrmTestMain.Report_Sel).ReportType = clsTest_Report.eReportType.Rejection
                    'End If

                    'AES 20MAR17
                    If (optTest.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ReportType = Test_clsReport.eReportType.Normal
                    ElseIf (optRejection.Checked) Then
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).ReportType = Test_clsReport.eReportType.Rejection
                    End If
                    mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Create(mTestProject, gTest_frmMain.MO_Sel, gTest_frmMain.Report_Sel, "PDF", pFileName)
                    Cursor = Cursors.Default
                End If
            End If
        End If

    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        SaveData()
        Me.Close()
    End Sub

#End Region


#Region "CHECK LIST BOX RELATED ROUTINE:"

    Private Sub clbImage_SelectedIndexChanged(sender As System.Object,
                                              e As System.EventArgs) Handles clbImage.SelectedIndexChanged
        '==================================================================================================
        Try

            Dim pIndex As Integer = clbImage.SelectedIndex
            Dim ImagePath As String = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(pIndex).File
            Dim img1 As Bitmap

            Dim newImage As Image = Nothing
            If (ImagePath.Contains(".pdf") Or ImagePath.Contains(".PDF")) Then
                picPreview.Image = Nothing
            Else
                newImage = Image.FromFile(ImagePath)
                img1 = New Bitmap(ImagePath)
                picPreview.ImageLocation = ImagePath

                picPreview.Image = newImage
            End If


        Catch ex As Exception

        End Try
    End Sub

#End Region


End Class