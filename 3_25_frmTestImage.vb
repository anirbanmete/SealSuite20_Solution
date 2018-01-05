'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Image                          '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05JUL17                                '
'                                                                              '
'===============================================================================
'
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports System.IO

Public Class Test_frmImage

#Region "MEMBER VARIABLES:"
    Private mTestProject As New Test_clsProject(gPartProject)
    Private mCurRecNo As Integer = 0
    Private mControl() As Control

    Private mImageFilePath As New List(Of String)

#End Region


    Private Sub frmTest_Image_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==============================================================================================
        mTestProject = gTest_Project.Clone()

        mControl = {txtImageFileName, cmdBrowse, txtNameTag, txtCaption, cmdAdd, cmdDelete, cmdUp, cmdDown}
        InitializeControls()
        DisplayData()

    End Sub

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
                If (mTestProject.IsTesterSigned) Then
                    mControl(i).Enabled = False
                Else
                    mControl(i).Enabled = True
                End If


            ElseIf (gTest_User.Role = Test_clsUser.eRole.Tester) Then

                If (pVal.Contains("ADef") Or pVal.Contains(gTest_User.Role.ToString())) Then
                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Tester.Signed) Then
                        mControl(i).Enabled = False
                    Else
                        mControl(i).Enabled = True
                    End If

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


    Private Sub DisplayData()
        '===================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).RetrieveFrom_DB(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
                lstImageName.Items.Clear()
                mImageFilePath.Clear()
                For i As Integer = 0 To mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Count - 1
                    lstImageName.Items.Add(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).NameTag)
                    mImageFilePath.Add(mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(i).File)
                Next
                If (lstImageName.Items.Count > 0) Then
                    txtImageFileName.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(0).File
                    txtNameTag.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(0).NameTag
                    txtCaption.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(0).Caption
                    lstImageName.SelectedIndex = 0
                End If

            End If
        End If

    End Sub


    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '=======================================================================================
        If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then
            If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).SaveTo_tblTestRptGenImage(mTestProject.ID, mTestProject.Test_MO(gTest_frmMain.MO_Sel).ID)
            End If
        End If

        Me.Close()

    End Sub


    Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
        '===============================================================================================
        With openFileDialog1
            .Filter = "SealTest Image Files Bitmap|*.bmp|JPEG|*.jpg|PNG|*.png"
            .FilterIndex = 1
            .InitialDirectory = "C:"
            .FileName = ""
            .Title = "Open"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtImageFileName.Text = openFileDialog1.FileName
                txtNameTag.Text = ""
                txtCaption.Text = ""
            End If

        End With
    End Sub


    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        '=========================================================================================
        Try

            If (txtNameTag.Text <> "") Then
                mImageFilePath.Add(txtImageFileName.Text.Trim())
                lstImageName.Items.Add(txtNameTag.Text)

                If (mTestProject.Test_MO.Count > gTest_frmMain.MO_Sel) Then

                    If (mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report.Count > gTest_frmMain.Report_Sel) Then
                        Dim pImage As New Test_clsReport.sGenImage
                        pImage.File = txtImageFileName.Text.Trim()
                        pImage.NameTag = txtNameTag.Text.Trim()
                        pImage.Caption = txtCaption.Text.Trim()
                        pImage.Image = Image.FromFile(pImage.File)
                        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Add(pImage)
                        lstImageName.SelectedIndex = mImageFilePath.Count - 1

                        txtImageFileName.Text = ""
                        txtNameTag.Text = ""
                        txtCaption.Text = ""
                    End If

                End If
            Else
                MessageBox.Show("Please enter proper Name Tag for the image.", "Blank Name Tag!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtNameTag.Focus()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete.Click
        '==============================================================================================
        Dim pIndex As Integer = lstImageName.SelectedIndex
        Dim ImagePath As String = mImageFilePath(pIndex)
        lstImageName.Items.RemoveAt(pIndex)
        mImageFilePath.RemoveAt(pIndex)
        mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.RemoveAt(pIndex)
        If (lstImageName.Items.Count > 0) Then
            lstImageName.SelectedIndex = 0
        End If

    End Sub

    Private Sub lstImageName_SelectedIndexChanged(sender As System.Object,
                                                  e As System.EventArgs) Handles lstImageName.SelectedIndexChanged
        '==========================================================================================================
        Try
            Dim pIndex As Integer = lstImageName.SelectedIndex
            Dim ImagePath As String = mImageFilePath(pIndex)
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

            txtImageFileName.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(pIndex).File
            txtNameTag.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(pIndex).NameTag
            txtCaption.Text = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(pIndex).Caption

        Catch ex As Exception

        End Try

    End Sub


    Private Sub cmdUp_Click(sender As System.Object, e As System.EventArgs) Handles cmdUp.Click
        '========================================================================================
        Dim pItem As Object
        Dim pFilePath As Object
        Dim pImage As Test_clsReport.sGenImage
        Dim pIndex As Integer

        If lstImageName.SelectedItem <> Nothing Then
            pItem = lstImageName.SelectedItem()
            pIndex = lstImageName.Items.IndexOf(pItem)
            pFilePath = mImageFilePath(pIndex)
            pImage = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(pIndex)

            If pIndex <> 0 Then
                lstImageName.Items.Insert(pIndex - 1, pItem)
                'lstImageName.SelectedIndex = pIndex - 1
                lstImageName.Items.RemoveAt(pIndex + 1)

                mImageFilePath.Insert(pIndex - 1, pFilePath)
                mImageFilePath.RemoveAt(pIndex + 1)

                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Insert(pIndex - 1, pImage)
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.RemoveAt(pIndex + 1)
                lstImageName.SelectedIndex = pIndex - 1
            End If

        End If

    End Sub

    Private Sub cmdDown_Click(sender As System.Object, e As System.EventArgs) Handles cmdDown.Click
        '===========================================================================================
        Dim pItem As Object
        Dim pFilePath As Object
        Dim pImage As Test_clsReport.sGenImage
        Dim pIndex As Integer

        If lstImageName.SelectedItem <> Nothing Then
            pItem = lstImageName.SelectedItem()
            pIndex = lstImageName.Items.IndexOf(pItem)
            pFilePath = mImageFilePath(pIndex)
            pImage = mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image(pIndex)

            If pIndex <> lstImageName.Items.Count - 1 Then
                lstImageName.Items.Insert(pIndex + 2, pItem)
                lstImageName.Items.RemoveAt(pIndex)

                mImageFilePath.Insert(pIndex + 2, pFilePath)
                mImageFilePath.RemoveAt(pIndex)

                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.Insert(pIndex + 2, pImage)
                mTestProject.Test_MO(gTest_frmMain.MO_Sel).Test_Report(gTest_frmMain.Report_Sel).Gen.Image.RemoveAt(pIndex)
                lstImageName.SelectedIndex = pIndex + 1

            End If

        End If

    End Sub

    Private Function imgToByteArray(ByVal img As Image) As Byte()
        '=========================================================
        Using mStream As New MemoryStream()
            img.Save(mStream, img.RawFormat)
            Return mStream.ToArray()
        End Using
    End Function


End Class