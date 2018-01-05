'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  frmTest_Unit                           '
'                        VERSION NO  :  2.6                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  27APR17                                '
'                                                                              '
'===============================================================================

Public Class Test_frmUnit

#Region "MEMBER VARIABLE DECLARATIONS:"
    Dim mTestProject As New Test_clsProject(gPartProject)
    Dim mDataLoadedFromDB As Boolean = False

#End Region


#Region "FORM LOAD EVENT ROUTINES:"

    Private Sub frmTest_Unit_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '============================================================================================
        mTestProject.ID = gTest_Project.ID
        'mTestProject.CustName = gTest_Project.CustName
        'mTestProject.Unit = gTest_Project.Unit.Clone()
        mTestProject = gTest_Project.Clone()     'AES 20FEB17

        mTestProject.Test_Unit.RetrieveFrom_DB(mTestProject.ID)

        PopulateComboBox(cmbLUnitPH, mTestProject.Test_Unit.LUnit_List)
        PopulateComboBox(cmbLUnitCust, mTestProject.Test_Unit.LUnit_List)

        PopulateComboBox(cmbFUnitPH, mTestProject.Test_Unit.FUnit_List)
        PopulateComboBox(cmbFUnitCust, mTestProject.Test_Unit.FUnit_List)

        PopulateComboBox(cmbPUnitPH, mTestProject.Test_Unit.PUnit_List)
        PopulateComboBox(cmbPUnitCust, mTestProject.Test_Unit.PUnit_List)

        PopulateComboBox(cmbLeakUnitPH, mTestProject.Test_Unit.LeakUnit_List)
        PopulateComboBox(cmbLeakUnitCust, mTestProject.Test_Unit.LeakUnit_List)

        DisplayData()
        gIsTestMainActive = False
        mDataLoadedFromDB = True
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub PopulateComboBox(ByVal ComboBox_In As ComboBox, ByVal Str_In() As String)
        '===================================================================================
        ComboBox_In.Items.Clear()
        For i As Integer = 0 To Str_In.Length - 1
            ComboBox_In.Items.Add(Str_In(i))
        Next

    End Sub


    Private Sub DisplayData()
        '====================

        If (mTestProject.PartProject.CustInfo.CustName <> "") Then
            lblCust.Text = mTestProject.PartProject.CustInfo.CustName
        End If

        '....PH
        If (mTestProject.Test_Unit.LUnit_PH = "") Then
            cmbLUnitPH.SelectedIndex = 0
        Else
            cmbLUnitPH.Text = mTestProject.Test_Unit.LUnit_PH
        End If

        If (mTestProject.Test_Unit.FUnit_PH = "") Then
            cmbFUnitPH.SelectedIndex = 0
        Else
            cmbFUnitPH.Text = mTestProject.Test_Unit.FUnit_PH
        End If

        If (mTestProject.Test_Unit.PUnit_PH = "") Then
            cmbPUnitPH.SelectedIndex = 0
        Else
            cmbPUnitPH.Text = mTestProject.Test_Unit.PUnit_PH
        End If

        If (mTestProject.Test_Unit.LeakUnit_PH = "") Then
            cmbLeakUnitPH.SelectedIndex = 0
        Else
            cmbLeakUnitPH.Text = mTestProject.Test_Unit.LeakUnit_PH
        End If

        '....Cust
        If (mTestProject.Test_Unit.LUnit_Cust = "") Then
            cmbLUnitCust.Text = cmbLUnitPH.Text
        Else
            cmbLUnitCust.Text = mTestProject.Test_Unit.LUnit_Cust
        End If

        If (mTestProject.Test_Unit.FUnit_Cust = "") Then
            cmbFUnitCust.Text = cmbFUnitPH.Text
        Else
            cmbFUnitCust.Text = mTestProject.Test_Unit.FUnit_Cust
        End If

        If (mTestProject.Test_Unit.PUnit_Cust = "") Then
            cmbPUnitCust.Text = cmbPUnitPH.Text
        Else
            cmbPUnitCust.Text = mTestProject.Test_Unit.PUnit_Cust
        End If

        If (mTestProject.Test_Unit.LeakUnit_Cust = "") Then
            cmbLeakUnitCust.Text = cmbLeakUnitPH.Text
        Else
            cmbLeakUnitCust.Text = mTestProject.Test_Unit.LeakUnit_Cust
        End If

    End Sub

#End Region

#End Region

#Region "COMBO BOX RELATED ROUTINES:"

    Private Sub cmbLUnitPH_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles cmbLUnitPH.SelectedIndexChanged
        '========================================================================================
        If (mDataLoadedFromDB) Then
            If cmbLUnitPH.Text = "in" Then
                cmbFUnitPH.Text = "lbf"
                cmbPUnitPH.Text = "psi"
                cmbLeakUnitPH.Text = "cc/s"

            ElseIf cmbLUnitPH.Text = "mm" Then
                cmbFUnitPH.Text = "N"
                cmbPUnitPH.Text = "kPa"
                cmbLeakUnitPH.Text = "cc/s"
            End If
        End If

    End Sub

    Private Sub cmbLUnitCust_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                  Handles cmbLUnitCust.SelectedIndexChanged
        '==========================================================================================
        If (mDataLoadedFromDB) Then
            If cmbLUnitCust.Text = "in" Then
                cmbFUnitCust.Text = "lbf"
                cmbPUnitCust.Text = "psi"
                cmbLeakUnitCust.Text = "cc/s"

            ElseIf cmbLUnitCust.Text = "mm" Then
                cmbFUnitCust.Text = "N"
                cmbPUnitCust.Text = "kPa"
                cmbLeakUnitCust.Text = "cc/s"
            End If
        End If
    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        SaveData()
        Me.Close()

    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub

#Region "HELPER ROUTINES:"

    Private Sub SaveData()
        '=================

        Dim pDataChanged As Boolean = IsDataChanged()

        Dim pblnSave As Boolean = True

        If (pDataChanged) Then
            If (mTestProject.Test_MO.Count > 0) Then
                If (mTestProject.Test_MO(0).Test_Report.Count > 0) Then
                    pblnSave = False
                End If
            End If
        End If

        If (pblnSave = False) Then
            MessageBox.Show("For this P/N & Rev. level, there are already existing record(s)." & vbCrLf &
                            "Hence, the 'Parker' unit system change is not permitted." & vbCrLf &
                            "If such change is needed, all the subordinate MOs & the" & vbCrLf &
                            "Reports should be deleted first.",
                            "Unit change not permitted!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            '....PH
            mTestProject.Test_Unit.LIndx_PH = cmbLUnitPH.SelectedIndex
            mTestProject.Test_Unit.FIndx_PH = cmbFUnitPH.SelectedIndex
            mTestProject.Test_Unit.PIndx_PH = cmbPUnitPH.SelectedIndex
            mTestProject.Test_Unit.LeakIndx_PH = cmbLeakUnitPH.SelectedIndex

            '....Cust
            mTestProject.Test_Unit.LIndx_Cust = cmbLUnitCust.SelectedIndex
            mTestProject.Test_Unit.FIndx_Cust = cmbFUnitCust.SelectedIndex
            mTestProject.Test_Unit.PIndx_Cust = cmbPUnitCust.SelectedIndex
            mTestProject.Test_Unit.LeakIndx_Cust = cmbLeakUnitCust.SelectedIndex

            mTestProject.Test_Unit.SetValues()

            mTestProject.Test_Unit.SaveTo_DB(mTestProject.ID)
            gTest_Project.Test_Unit = mTestProject.Test_Unit.Clone()
        End If


    End Sub

    Private Function IsDataChanged() As Boolean
        '======================================
        Dim pFlag As Boolean = False
        If (mTestProject.Test_Unit.LUnit_PH <> mTestProject.Test_Unit.LUnit_List(cmbLUnitPH.SelectedIndex) Or
            mTestProject.Test_Unit.FUnit_PH <> mTestProject.Test_Unit.FUnit_List(cmbFUnitPH.SelectedIndex) Or
            mTestProject.Test_Unit.PUnit_PH <> mTestProject.Test_Unit.PUnit_List(cmbPUnitPH.SelectedIndex) Or
            mTestProject.Test_Unit.LeakUnit_PH <> mTestProject.Test_Unit.LeakUnit_List(cmbLeakUnitPH.SelectedIndex)) Then

            pFlag = True

        End If

        'If (mTestProject.Unit.LUnit_Cust <> mTestProject.Unit.LUnit_List(cmbLUnitCust.SelectedIndex) Or
        '    mTestProject.Unit.FUnit_Cust <> mTestProject.Unit.FUnit_List(cmbFUnitCust.SelectedIndex) Or
        '    mTestProject.Unit.PUnit_Cust <> mTestProject.Unit.PUnit_List(cmbPUnitCust.SelectedIndex) Or
        '    mTestProject.Unit.LeakUnit_Cust <> mTestProject.Unit.LeakUnit_List(cmbLeakUnitCust.SelectedIndex)) Then

        '    pFlag = True

        'End If

        Return pFlag
    End Function

#End Region

#End Region



    Private Sub cmbLeakUnitCust_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                    Handles cmbLeakUnitCust.SelectedIndexChanged
        '==============================================================================================

        If cmbLeakUnitCust.Text = "mbar L/s" Then
            MessageBox.Show("This Leak unit is not yet implemented.", "Unit Not Implemented!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmbLeakUnitCust.Text = "cc/s"
        End If
    End Sub

    Private Sub cmbLeakUnitPH_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbLeakUnitPH.SelectedIndexChanged
        If cmbLeakUnitPH.Text = "mbar L/s" Then
            MessageBox.Show("This Leak unit is not yet implemented.", "Unit Not Implemented!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmbLeakUnitPH.Text = "cc/s"
        End If
    End Sub
End Class