'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_Equipment                      '
'                        VERSION NO  :  10.0                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29NOV16                                '
'                                                                              '
'===============================================================================
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.IO
Imports System.Threading

Public Class Test_clsEquipment

#Region "DB RELATED ROUTINES:"


    Public Sub UpdateTo_DB(ByVal FileName_In As String)
        '===============================================

        CloseExcelFiles()

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing

        Dim pSealTestDBEntities As New SealTestDBEntities

        Try
            pWkbOrg = pApp.Workbooks.Open(FileName_In, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value, _
             Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
             Missing.Value, Missing.Value, Missing.Value)

            Dim pWkSheet As EXCEL.Worksheet

            pWkSheet = pWkbOrg.Worksheets("Sheet1")

            Dim pExcelCellRange As EXCEL.Range = Nothing

            'Dim cb As Microsoft.Office.Interop.Excel.CheckBox
            'cb = CType(pWkSheet.CheckBoxes("CheckBox502"), Microsoft.Office.Interop.Excel.CheckBox)
            'cb.Value = True

            '....Leakage Equipment
            Dim pLeakStand_Start As Integer = 2
            Dim pLeakFixture_Start As Integer = 5

            Dim pLeakEquip As New List(Of String)
            Dim pLeakFixture As New List(Of String)

            For i As Integer = 0 To 10
                Dim pVal1 As String = pWkSheet.Cells(7 + i, pLeakStand_Start).value
                Dim pVal2 As String = pWkSheet.Cells(7 + i, pLeakFixture_Start).value

                If (IsNothing(pVal1)) Then
                    pVal1 = ""
                End If

                If (IsNothing(pVal2)) Then
                    pVal2 = ""
                End If

                If (pVal1 <> "" Or pVal2 <> "") Then
                    pLeakEquip.Add(pVal1)
                    pLeakFixture.Add(pVal2)

                Else
                    Exit For
                End If
            Next

            If (pLeakEquip.Count > 0) Then
                Dim pLeakStandRec = (From Rec In pSealTestDBEntities.tblLeakStand
                                     Select Rec).ToList()

                For i1 As Integer = 0 To pLeakStandRec.Count() - 1
                    pSealTestDBEntities.DeleteObject(pLeakStandRec(i1))
                    pSealTestDBEntities.SaveChanges()
                Next

                Dim pLeakStandList As New List(Of tblLeakStand)

                For j As Integer = 0 To pLeakEquip.Count - 1
                    Dim pLeakStand As New tblLeakStand
                    pLeakStandList.Add(pLeakStand)
                    pLeakStandList(j).fldName = pLeakEquip(j)
                    pLeakStandList(j).fldFixture = pLeakFixture(j)
                    pSealTestDBEntities.AddTotblLeakStand(pLeakStandList(j))
                Next

                pSealTestDBEntities.SaveChanges()

            End If

            Dim pLeakMedium_Start As Integer = 7
            Dim pMedium As New List(Of String)
            For i As Integer = 0 To 10
                Dim pVal1 As String = pWkSheet.Cells(7 + i, pLeakMedium_Start).value
                If (IsNothing(pVal1)) Then
                    pVal1 = ""
                End If
                If (pVal1 <> "") Then
                    pMedium.Add(pVal1)

                Else
                    Exit For
                End If
            Next

            If (pMedium.Count > 0) Then
                Dim pMediumRec = (From Rec In pSealTestDBEntities.tblLeakMedium
                                     Select Rec).ToList()

                For i1 As Integer = 0 To pMediumRec.Count() - 1
                    pSealTestDBEntities.DeleteObject(pMediumRec(i1))
                    pSealTestDBEntities.SaveChanges()
                Next

                Dim pLeakMediumList As New List(Of tblLeakMedium)

                For j As Integer = 0 To pMedium.Count - 1
                    Dim pLeakMedium As New tblLeakMedium
                    pLeakMediumList.Add(pLeakMedium)
                    pLeakMediumList(j).fldName = pMedium(j)

                    pSealTestDBEntities.AddTotblLeakMedium(pLeakMediumList(j))
                Next

                pSealTestDBEntities.SaveChanges()

            End If

            Dim pFMType_Start As Integer = 2
            Dim pFMSN_Start As Integer = 3
            Dim pFMRange_Start As Integer = 4
            Dim pFMModel_Start As Integer = 5
            Dim pFMCalDue_Start As Integer = 6

            Dim pFMType As New List(Of String)
            Dim pFMSN As New List(Of String)
            Dim pFMRange As New List(Of String)
            Dim pFMModel As New List(Of String)
            Dim pFMCalDue As New List(Of String)

            For i As Integer = 0 To 10
                Dim pVal1 As String = pWkSheet.Cells(27 + i, pFMType_Start).value
                Dim pVal2 As String = pWkSheet.Cells(27 + i, pFMSN_Start).value
                Dim pVal3 As String = pWkSheet.Cells(27 + i, pFMRange_Start).value
                Dim pVal4 As String = pWkSheet.Cells(27 + i, pFMModel_Start).value
                Dim pVal5 As String = pWkSheet.Cells(27 + i, pFMCalDue_Start).value

                If (IsNothing(pVal1)) Then
                    pVal1 = ""
                End If

                If (IsNothing(pVal2)) Then
                    pVal2 = ""
                End If

                If (IsNothing(pVal3)) Then
                    pVal3 = ""
                End If

                If (IsNothing(pVal4)) Then
                    pVal4 = ""
                End If

                If (IsNothing(pVal5)) Then
                    pVal5 = ""
                End If

                If (pVal1 <> "" Or pVal2 <> "" Or pVal3 <> "" Or pVal4 <> "" Or pVal5 <> "") Then
                    pFMType.Add(pVal1)
                    pFMSN.Add(pVal2)
                    pFMRange.Add(pVal3)
                    pFMModel.Add(pVal4)
                    pFMCalDue.Add(pVal5)

                Else
                    Exit For
                End If
            Next

            If (pFMType.Count > 0) Then
                Dim pFMRec = (From Rec In pSealTestDBEntities.tblFlowMeter
                                     Select Rec).ToList()

                For i1 As Integer = 0 To pFMRec.Count() - 1
                    pSealTestDBEntities.DeleteObject(pFMRec(i1))
                    pSealTestDBEntities.SaveChanges()
                Next

                Dim pFMList As New List(Of tblFlowMeter)

                For j As Integer = 0 To pFMType.Count - 1
                    Dim pFM As New tblFlowMeter
                    pFMList.Add(pFM)
                    pFMList(j).fldMake = pFMType(j)
                    pFMList(j).fldSN = pFMSN(j)
                    pFMList(j).fldRange = pFMRange(j)
                    pFMList(j).fldModelNo = pFMModel(j)
                    pFMList(j).fldDateCalibrationDue = pFMCalDue(j)
                    pSealTestDBEntities.AddTotblFlowMeter(pFMList(j))
                Next

                pSealTestDBEntities.SaveChanges()

            End If

            '....Load Equipment
            Dim pLoadEquipStand_Start As Integer = 11
            Dim pLoadEquipSN_Start As Integer = 12
            Dim pLoadEquipCalDue_Start As Integer = 13

            Dim pLoadEquipStand As New List(Of String)
            Dim pLoadEquipSN As New List(Of String)
            Dim pLoadEquipCalDue As New List(Of String)

            For i As Integer = 0 To 10
                Dim pVal1 As String = pWkSheet.Cells(7 + i, pLoadEquipStand_Start).value
                Dim pVal2 As String = pWkSheet.Cells(7 + i, pLoadEquipSN_Start).value
                Dim pVal3 As String = pWkSheet.Cells(7 + i, pLoadEquipCalDue_Start).value


                If (IsNothing(pVal1)) Then
                    pVal1 = ""
                End If

                If (IsNothing(pVal2)) Then
                    pVal2 = ""
                End If

                If (IsNothing(pVal3)) Then
                    pVal3 = ""
                End If


                If (pVal1 <> "" Or pVal2 <> "" Or pVal3 <> "") Then
                    pLoadEquipStand.Add(pVal1)
                    pLoadEquipSN.Add(pVal2)
                    pLoadEquipCalDue.Add(pVal3)
                Else
                    Exit For
                End If
            Next

            If (pLoadEquipStand.Count > 0) Then
                Dim pLoadEquipStandRec = (From Rec In pSealTestDBEntities.tblForceStand
                                     Select Rec).ToList()

                For i As Integer = 0 To pLoadEquipStandRec.Count() - 1
                    pSealTestDBEntities.DeleteObject(pLoadEquipStandRec(i))
                    pSealTestDBEntities.SaveChanges()
                Next

                Dim pLoadEquipStandList As New List(Of tblForceStand)

                For j As Integer = 0 To pLoadEquipStand.Count - 1
                    Dim pLoadEquip As New tblForceStand
                    pLoadEquipStandList.Add(pLoadEquip)
                    pLoadEquipStandList(j).fldName = pLoadEquipStand(j)
                    pLoadEquipStandList(j).fldSN = pLoadEquipSN(j)
                    pLoadEquipStandList(j).fldDateCalibrationDue = pLoadEquipCalDue(j)

                    pSealTestDBEntities.AddTotblForceStand(pLoadEquipStandList(j))
                Next

                pSealTestDBEntities.SaveChanges()

            End If

            '....Load Cell
            Dim pLoadCellName_Start As Integer = 11
            Dim pLoadCellSN_Start As Integer = 12
            Dim pLoadCellRange_Start As Integer = 13
            Dim pLoadCellModel_Start As Integer = 14
            Dim pLoadCellCalDue_Start As Integer = 15

            Dim pLoadCellName As New List(Of String)
            Dim pLoadCellSN As New List(Of String)
            Dim pLoadCellRange As New List(Of String)
            Dim pLoadCellModel As New List(Of String)
            Dim pLoadCellCalDue As New List(Of String)

            For i As Integer = 0 To 10
                Dim pVal1 As String = pWkSheet.Cells(27 + i, pLoadCellName_Start).value
                Dim pVal2 As String = pWkSheet.Cells(27 + i, pLoadCellSN_Start).value
                Dim pVal3 As String = pWkSheet.Cells(27 + i, pLoadCellRange_Start).value
                Dim pVal4 As String = pWkSheet.Cells(27 + i, pLoadCellModel_Start).value
                Dim pVal5 As String = pWkSheet.Cells(27 + i, pLoadCellCalDue_Start).value

                If (IsNothing(pVal1)) Then
                    pVal1 = ""
                End If

                If (IsNothing(pVal2)) Then
                    pVal2 = ""
                End If

                If (IsNothing(pVal3)) Then
                    pVal3 = ""
                End If

                If (IsNothing(pVal4)) Then
                    pVal4 = ""
                End If

                If (IsNothing(pVal5)) Then
                    pVal5 = ""
                End If

                If (pVal1 <> "" Or pVal2 <> "" Or pVal3 <> "" Or pVal4 <> "" Or pVal5 <> "") Then
                    pLoadCellName.Add(pVal1)
                    pLoadCellSN.Add(pVal2)
                    pLoadCellRange.Add(pVal3)
                    pLoadCellModel.Add(pVal4)
                    pLoadCellCalDue.Add(pVal5)

                Else
                    Exit For
                End If
            Next

            If (pLoadCellName.Count > 0) Then
                Dim pLoadCellRec = (From Rec In pSealTestDBEntities.tblLoadCell
                                     Select Rec).ToList()

                For i As Integer = 0 To pLoadCellRec.Count() - 1
                    pSealTestDBEntities.DeleteObject(pLoadCellRec(i))
                    pSealTestDBEntities.SaveChanges()
                Next

                Dim pLoadCellList As New List(Of tblLoadCell)

                For j As Integer = 0 To pLoadCellName.Count - 1
                    Dim pLoadCell As New tblLoadCell
                    pLoadCellList.Add(pLoadCell)
                    pLoadCellList(j).fldMake = pLoadCellName(j)
                    pLoadCellList(j).fldSN = pLoadCellSN(j)
                    pLoadCellList(j).fldRange = pLoadCellRange(j)
                    pLoadCellList(j).fldModelNo = pLoadCellModel(j)
                    pLoadCellList(j).fldDateCalibrationDue = pLoadCellCalDue(j)
                    pSealTestDBEntities.AddTotblLoadCell(pLoadCellList(j))
                Next

                pSealTestDBEntities.SaveChanges()

            End If

            Thread.Sleep(100)
            Dim pFileTitle As String = Path.GetFileName(FileName_In)
            Dim pMsg As String = "Equipment List Updated from: " & vbLf & Space(10) & pFileTitle
            MessageBox.Show(pMsg, "Equipment List", MessageBoxButtons.OK)

        Catch ex As Exception

        Finally
            pWkbOrg.Close()
            pApp.Quit()

        End Try

    End Sub


    Private Sub CloseExcelFiles()
        '=======================

        Dim pProcesses As Process() = Process.GetProcesses()

        Try
            For Each p As Process In pProcesses
                If p.ProcessName = "EXCEL" Then
                    p.Kill()
                End If
            Next

        Catch pEXP As Exception
        End Try
    End Sub

#End Region

End Class
