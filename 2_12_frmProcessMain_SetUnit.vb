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

    Dim mcLUnit_List() As String = New String() {"in", "mm"}
    Dim mcFUnit_List() As String = New String() {"lbf", "N", "kgf"}
    Dim mcPUnit_List() As String = New String() {"psi", "kPa", "atm", "bar"}
    Dim mcTUnit_List() As String = New String() {"ºF", "ºC"}
    Dim mcLeakUnit_List() As String = New String() {"cc/s", "mL/min", "L/min", "scfm", "sccm", "mbar L/s"}

    Private Sub Process_frmUnit_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '===============================================================================================
        PopulateComboBox(cmbLUnitPH, mcLUnit_List)
        PopulateComboBox(cmbLUnitCust, mcLUnit_List)

        PopulateComboBox(cmbFUnitPH, mcFUnit_List)
        PopulateComboBox(cmbFUnitCust, mcFUnit_List)

        PopulateComboBox(cmbPUnitPH, mcPUnit_List)
        PopulateComboBox(cmbPUnitCust, mcPUnit_List)

        PopulateComboBox(cmbTUnitPH, mcTUnit_List)
        PopulateComboBox(cmbTUnitCust, mcTUnit_List)

        PopulateComboBox(cmbLeakUnitPH, mcLeakUnit_List)
        PopulateComboBox(cmbLeakUnitCust, mcLeakUnit_List)

        cmbLUnitPH.SelectedIndex = 0
        cmbLUnitCust.SelectedIndex = 0

        cmbFUnitPH.SelectedIndex = 0
        cmbFUnitCust.SelectedIndex = 0

        cmbPUnitPH.SelectedIndex = 0
        cmbPUnitCust.SelectedIndex = 0

        cmbTUnitPH.SelectedIndex = 0
        cmbTUnitCust.SelectedIndex = 0

        cmbLeakUnitPH.SelectedIndex = 0
        cmbLeakUnitCust.SelectedIndex = 0

    End Sub

    Private Sub PopulateComboBox(ByVal ComboBox_In As ComboBox, ByVal Str_In() As String)
        '===================================================================================
        ComboBox_In.Items.Clear()
        For i As Integer = 0 To Str_In.Length - 1
            ComboBox_In.Items.Add(Str_In(i))
        Next

    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        '==============================================================================================
        Me.Close()
    End Sub
End Class