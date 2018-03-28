'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      FORM MODULE   :  Process_frmUnit                        '
'                        VERSION NO  :  1.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  02NOV17                                '
'                                                                              '
'===============================================================================
Public Class Process_frmUnit

    Dim mProcessProject As New clsProcessProj(gPartProject)
    Dim mDataLoadedFromDB As Boolean = False

    Private Sub Process_frmUnit_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===============================================================================================
        mProcessProject = gProcessProject.Clone()

        mProcessProject.Unit.SetDefaultVal()

        mProcessProject.Unit.RetrieveFrom_DB(mProcessProject.ID)

        PopulateComboBox(cmbLUnitPH, mProcessProject.Unit.LUnit_List)
        PopulateComboBox(cmbLUnitCust, mProcessProject.Unit.LUnit_List)

        PopulateComboBox(cmbFUnitPH, mProcessProject.Unit.FUnit_List)
        PopulateComboBox(cmbFUnitCust, mProcessProject.Unit.FUnit_List)

        PopulateComboBox(cmbPUnitPH, mProcessProject.Unit.PUnit_List)
        PopulateComboBox(cmbPUnitCust, mProcessProject.Unit.PUnit_List)

        PopulateComboBox(cmbTUnitPH, mProcessProject.Unit.TUnit_List)
        PopulateComboBox(cmbTUnitCust, mProcessProject.Unit.TUnit_List)

        PopulateComboBox(cmbLeakUnitPH, mProcessProject.Unit.LeakUnit_List)
        PopulateComboBox(cmbLeakUnitCust, mProcessProject.Unit.LeakUnit_List)

        DisplayData()
        gIsProcessMainActive = False
        mDataLoadedFromDB = True

    End Sub

    Private Sub PopulateComboBox(ByVal ComboBox_In As ComboBox, ByVal Str_In() As String)
        '===================================================================================
        ComboBox_In.Items.Clear()
        For i As Integer = 0 To Str_In.Length - 1
            ComboBox_In.Items.Add(Str_In(i))
        Next

    End Sub

    Private Sub DisplayData()
        '====================
        lblCust.Text = gPartProject.CustInfo.CustName

        '....PH
        If (mProcessProject.Unit.LUnit_PH = "") Then
            cmbLUnitPH.SelectedIndex = 0
        Else
            cmbLUnitPH.Text = mProcessProject.Unit.LUnit_PH
        End If

        If (mProcessProject.Unit.FUnit_PH = "") Then
            cmbFUnitPH.SelectedIndex = 0
        Else
            cmbFUnitPH.Text = mProcessProject.Unit.FUnit_PH
        End If

        If (mProcessProject.Unit.PUnit_PH = "") Then
            cmbPUnitPH.SelectedIndex = 0
        Else
            cmbPUnitPH.Text = mProcessProject.Unit.PUnit_PH
        End If

        If (mProcessProject.Unit.TUnit_PH = "") Then
            cmbTUnitPH.SelectedIndex = 0
        Else
            cmbTUnitPH.Text = mProcessProject.Unit.TUnit_PH
        End If

        If (mProcessProject.Unit.LeakUnit_PH = "") Then
            cmbLeakUnitPH.SelectedIndex = 0
        Else
            cmbLeakUnitPH.Text = mProcessProject.Unit.LeakUnit_PH
        End If

        '....Cust
        If (mProcessProject.Unit.LUnit_Cust = "") Then
            cmbLUnitCust.Text = cmbLUnitPH.Text
        Else
            cmbLUnitCust.Text = mProcessProject.Unit.LUnit_Cust
        End If

        If (mProcessProject.Unit.FUnit_Cust = "") Then
            cmbFUnitCust.Text = cmbFUnitPH.Text
        Else
            cmbFUnitCust.Text = mProcessProject.Unit.FUnit_Cust
        End If

        If (mProcessProject.Unit.PUnit_Cust = "") Then
            cmbPUnitCust.Text = cmbPUnitPH.Text
        Else
            cmbPUnitCust.Text = mProcessProject.Unit.PUnit_Cust
        End If

        If (mProcessProject.Unit.TUnit_Cust = "") Then
            cmbTUnitCust.Text = cmbTUnitPH.Text
        Else
            cmbTUnitCust.Text = mProcessProject.Unit.TUnit_Cust
        End If

        If (mProcessProject.Unit.LeakUnit_Cust = "") Then
            cmbLeakUnitCust.Text = cmbLeakUnitPH.Text
        Else
            cmbLeakUnitCust.Text = mProcessProject.Unit.LeakUnit_Cust
        End If

    End Sub

    Private Sub SaveData()
        '=================
        '....PH
        mProcessProject.Unit.LIndx_PH = cmbLUnitPH.SelectedIndex
        mProcessProject.Unit.FIndx_PH = cmbFUnitPH.SelectedIndex
        mProcessProject.Unit.PIndx_PH = cmbPUnitPH.SelectedIndex
        mProcessProject.Unit.TIndx_PH = cmbTUnitPH.SelectedIndex
        mProcessProject.Unit.LeakIndx_PH = cmbLeakUnitPH.SelectedIndex

        '....Cust
        mProcessProject.Unit.LIndx_Cust = cmbLUnitCust.SelectedIndex
        mProcessProject.Unit.FIndx_Cust = cmbFUnitCust.SelectedIndex
        mProcessProject.Unit.PIndx_Cust = cmbPUnitCust.SelectedIndex
        mProcessProject.Unit.TIndx_Cust = cmbTUnitCust.SelectedIndex
        mProcessProject.Unit.LeakIndx_Cust = cmbLeakUnitCust.SelectedIndex

        mProcessProject.Unit.SaveTo_DB(mProcessProject.ID)
        gProcessProject = mProcessProject.Clone()


    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        SaveData()
        'AES 26FEB18
        If (mProcessProject.Unit.LUnit_Cust = "in") Then
            gUnit.SetLFormat("English")
        Else
            gUnit.SetLFormat("Metric")
        End If

        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub

    Private Sub cmbLUnitPH_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLUnitPH.SelectedIndexChanged
        '================================================================================================================
        If (mDataLoadedFromDB) Then
            If cmbLUnitPH.Text = "in" Then
                cmbFUnitPH.Text = "lbf"
                cmbPUnitPH.Text = "psi"
                cmbTUnitPH.Text = "ºF"
                cmbLeakUnitPH.Text = "cc/s"

            ElseIf cmbLUnitPH.Text = "mm" Then
                cmbFUnitPH.Text = "N"
                cmbPUnitPH.Text = "kPa"
                cmbTUnitPH.Text = "ºC"
                cmbLeakUnitPH.Text = "cc/s"
            End If
        End If
    End Sub

    Private Sub cmbLUnitCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLUnitCust.SelectedIndexChanged
        '====================================================================================================================
        If (mDataLoadedFromDB) Then
            If cmbLUnitCust.Text = "in" Then
                cmbFUnitCust.Text = "lbf"
                cmbPUnitCust.Text = "psi"
                cmbTUnitCust.Text = "ºF"
                cmbLeakUnitCust.Text = "cc/s"

            ElseIf cmbLUnitCust.Text = "mm" Then
                cmbFUnitCust.Text = "N"
                cmbPUnitCust.Text = "kPa"
                cmbTUnitCust.Text = "ºC"
                cmbLeakUnitCust.Text = "cc/s"
            End If
        End If

    End Sub
End Class