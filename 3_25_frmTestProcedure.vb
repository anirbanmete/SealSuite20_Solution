'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmTest_Procedure                      '
'                        VERSION NO  :  2.6                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  12JUN17                                '
'                                                                              '
'===============================================================================
'
Imports System.DateTime
Imports System.Globalization
Imports System.Linq
Imports Microsoft.Office.Interop.PowerPoint
Imports Microsoft.Office.Core

Public Class Test_frmProcedure

#Region "ENUMERATION TYPES:"

    Enum eType
        Leak
        Load
    End Enum

#End Region


#Region "MEMBER VARIABLES:"

    Private mTestProject As New Test_clsProject(gPartProject)
    Private mControl() As Control
    Private mType As eType

#End Region


#Region "CONSTRUCTOR:"
    '=================

    Public Sub New(ByVal Type_In As eType)
        '==================================
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mType = Type_In

    End Sub

#End Region


    Private Sub frmTest_Procedure_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '=================================================================================================
        gIsTestLeakActive = False
        gIsTestLoadActive = False
        mControl = {txtFileName, cmdBrowse}
        InitializeControls()
        mTestProject = gTest_Project.Clone()

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

    Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
        '==============================================================================================
        With openFileDialog1
            .Filter = "SealTest PowerPoint Files (*.pptx)|*.pptx"
            .FilterIndex = 1
            .InitialDirectory = gTest_File.DirProdedure
            .FileName = ""
            .Title = "Open"

            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If (mType = eType.Leak) Then
                    mTestProject.Test_Spec.LeakProcedureFile = openFileDialog1.FileName
                ElseIf (mType = eType.Load) Then
                    mTestProject.Test_Spec.LoadProcedureFile = openFileDialog1.FileName
                End If

                DisplayData()

            End If

        End With
    End Sub


    Private Sub DisplayData()
        '====================

        If (mType = eType.Leak) Then

            'AES 12JUN17
            If (gTest_Test.FileName_LeakProcedure <> "") Then
                txtFileName.Text = gTest_Test.FileName_LeakProcedure
                mTestProject.Test_Spec.LeakProcedureFile = gTest_Test.FileName_LeakProcedure
            End If

            'txtFileName.Text = mTestProject.Test_Spec.LeakProcedureFile

        ElseIf (mType = eType.Load) Then

            'AES 12JUN17
            If (gTest_Test.FileName_LoadProcedure <> "") Then
                txtFileName.Text = gTest_Test.FileName_LoadProcedure
                mTestProject.Test_Spec.LoadProcedureFile = gTest_Test.FileName_LoadProcedure
            End If

            'txtFileName.Text = mTestProject.Test_Spec.LoadProcedureFile
        End If

    End Sub

    Private Sub SaveData()
        '=================
        gTest_Project = mTestProject.Clone()

        If (mTestProject.Test_Spec.LeakProcedureFile <> "") Then
            gTest_Test.FileName_LeakProcedure = mTestProject.Test_Spec.LeakProcedureFile
        End If

        If (mTestProject.Test_Spec.LoadProcedureFile <> "") Then
            gTest_Test.FileName_LoadProcedure = mTestProject.Test_Spec.LoadProcedureFile
        End If

        gTest_Test.SaveTo_DB()

    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        SaveData()
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '=============================================================================================
        Me.Close()
    End Sub


    Private Sub cmdView_Click(sender As System.Object, e As System.EventArgs) Handles cmdView.Click
        '===========================================================================================
        If (mType = eType.Leak) Then
            If (mTestProject.Test_Spec.LeakProcedureFile <> "") Then
                gTest_Report.OpenPowerPoint(mTestProject.Test_Spec.LeakProcedureFile)
            End If

        ElseIf (mType = eType.Load) Then
            If (mTestProject.Test_Spec.LoadProcedureFile <> "") Then
                gTest_Report.OpenPowerPoint(mTestProject.Test_Spec.LoadProcedureFile)
            End If

        End If

    End Sub
End Class