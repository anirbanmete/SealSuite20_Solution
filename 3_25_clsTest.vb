'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest                                '
'                        VERSION NO  :  2.2                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  09MAY17                                '
'                                                                              '
'===============================================================================
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms

Public Class Test_clsTest


#Region "REPORT TEMPLATE FILES LOCATION: "

    'Report Template File:
    '-----------------------------
    Private Const mcDirTemplates As String = "C:\SealSuite\SealTest\Templates\"

    '....Status Report  
    Private Const mcStatusReportFileName As String = mcDirTemplates & "StatusReport_Rev01.xlt"


#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"

    Private mTestProject As New List(Of Test_clsProject)

    Private mFileName_EquipList As String
    Private mFileName_LeakProcedure As String
    Private mFileName_LoadProcedure As String

#End Region


#Region "PROPERTY ROUTINES:"

    '....FileName_EquipList
    Public Property FileName_EquipList() As String
        '==========================================
        Get
            Return mFileName_EquipList
        End Get

        Set(ByVal value As String)
            mFileName_EquipList = value

        End Set

    End Property


    '....FileName_LeakProcedure
    Public Property FileName_LeakProcedure() As String
        '==============================================
        Get
            Return mFileName_LeakProcedure
        End Get

        Set(ByVal value As String)
            mFileName_LeakProcedure = value

        End Set

    End Property


    '....FileName_LoadProcedure
    Public Property FileName_LoadProcedure() As String
        '=============================================
        Get
            Return mFileName_LoadProcedure
        End Get

        Set(ByVal value As String)
            mFileName_LoadProcedure = value

        End Set

    End Property

#End Region


#Region "MAIN ROUTINES:"

    Public Sub StatusReport(ByVal cmbParkerPN_In As ComboBox, ByVal Part_Unit_In As clsPartUnit)
        '=======================================================================================

        ''CloseExcelFiles()

        ''Dim pApp As EXCEL.Application = Nothing
        ''pApp = New EXCEL.Application()

        ''pApp.DisplayAlerts = False

        ' ''....Open Load.xls WorkBook.
        ''Dim pWkbOrg As EXCEL.Workbook = Nothing

        ''Dim pSealTestDBEntities As New SealTestDBEntities

        ''Try
        ''    pWkbOrg = pApp.Workbooks.Open(mcStatusReportFileName, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value, _
        ''                                  Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
        ''                                  Missing.Value, Missing.Value, Missing.Value)

        ''    Dim pWkSheet As EXCEL.Worksheet

        ''    pWkSheet = pWkbOrg.Worksheets("StatusReport")

        ''    Dim pExcelCellRange As EXCEL.Range = Nothing
        ''    pExcelCellRange = pWkSheet.Range("G2") : pExcelCellRange.Value = DateTime.Today.ToShortDateString()
        ''    pExcelCellRange = pWkSheet.Range("H2") : pExcelCellRange.Value = DateTime.Now.ToShortTimeString()

        ''    Dim pMinDate As DateTime = DateTime.MinValue

        ''    Dim pRowCount As Integer = 5
        ''    Dim pPNID As Integer = 0

        ''    For i As Integer = 0 To cmbParkerPN_In.Items.Count - 1
        ''        Dim pSealTestEntities As New SealTestDBEntities

        ''        Dim pPN As String = cmbParkerPN_In.Items(i)

        ''        Dim pSeal_Part As New clsPartProject

        ''        pPNID = pSeal_Part.GetPNID(pPN)
        ''        Dim pQry = (From pRec In pSealTestEntities.tblTestProject
        ''                       Where pRec.fldPNID = pPNID Select pRec).ToList()

        ''        For j As Integer = 0 To pQry.Count() - 1
        ''            Dim pRevID As Integer = pQry(j).fldRevID
        ''            Dim pRev As String = pSeal_Part.GetPN_Rev(pPNID, pRevID)

        ''            'Dim pProject As clsProject = New clsProject()
        ''            Dim pTestProject As Test_clsProject = New Test_clsProject()

        ''            pTestProject.PN = pPN
        ''            pTestProject.Rev = pRev

        ''            Dim pProjectID As Integer = 0

        ''            'Dim pPNID As Integer = pProject.GetPNID(pPN)
        ''            'Dim pRevID As Integer = pProject.GetRevID(pPNID, pRev)

        ''            'Dim pProjectEntities As New ProjectDBEntities
        ''            'Dim pQryProject = (From pRec In pProjectEntities.tblProject Where pRec.fldPNID = pPNID And pRec.fldRevID = pRevID Select pRec.fldID).ToList()

        ''            'If (pQryProject.Count() > 0) Then
        ''            '    pProjectID = pQryProject(0)
        ''            'End If

        ''            'pTestProject.ProjectID = pProjectID

        ''            'pProject.PN_ID = pProject.GetPNID(pPN)
        ''            'pProject.PN_Rev_ID = pProject.GetRevID(pProject.PN_ID, pRev)

        ''            'pTestProject.Analysis_Sel = New clsAnalysis(pProject)

        ''            '....Get TestProjectID
        ''            Dim pQryTestProject = (From pRec In pSealTestEntities.tblTestProject
        ''                                  Where pRec.fldPNID = pPNID And pRec.fldRevID = pRevID Select pRec).ToList()

        ''            Dim pEndDate_PNR As DateTime = DateTime.MaxValue
        ''            Dim pBegDate_PNR As DateTime = Nothing
        ''            If (pQryTestProject.Count() > 0) Then
        ''                pTestProject.ID = pQryTestProject(0).fldID
        ''                pEndDate_PNR = pQryTestProject(0).fldDateSignedOff
        ''                pBegDate_PNR = pQryTestProject(0).fldDateAdmin
        ''            End If

        ''            '....Retrieve Test Project Data
        ''            pTestProject.RetrieveFrom_DB(Part_Unit_In)

        ''            Dim pPN_Rev As String = pTestProject.PN

        ''            If (pTestProject.Rev <> "") Then
        ''                pPN_Rev = pPN_Rev & "-" & pTestProject.Rev
        ''            End If

        ''            pExcelCellRange = pWkSheet.Range("A" & pRowCount.ToString()) : pExcelCellRange.Value = pPN_Rev

        ''            Dim pSignOff As String = "Sign-off"
        ''            If (pTestProject.SignedOff) Then
        ''                pSignOff = "Signed-off"
        ''            End If

        ''            pExcelCellRange = pWkSheet.Range("D" & pRowCount.ToString()) : pExcelCellRange.Value = pSignOff
        ''            pExcelCellRange = pWkSheet.Range("E" & pRowCount.ToString()) : pExcelCellRange.Value = pTestProject.UserSignedOff
        ''            pExcelCellRange = pWkSheet.Range("F" & pRowCount.ToString()) : pExcelCellRange.Value = pBegDate_PNR.ToShortDateString()
        ''            If (pEndDate_PNR.Year <> DateTime.MaxValue.Year) Then
        ''                pExcelCellRange = pWkSheet.Range("G" & pRowCount.ToString()) : pExcelCellRange.Value = pEndDate_PNR.ToShortDateString()
        ''            Else
        ''                pExcelCellRange = pWkSheet.Range("G" & pRowCount.ToString()) : pExcelCellRange.Value = ""
        ''            End If


        ''            If (pSignOff = "Sign-off") Then
        ''                pExcelCellRange = pWkSheet.Range("H" & pRowCount.ToString()) : pExcelCellRange.Value = DateDiff(DateInterval.Day, pBegDate_PNR, DateTime.Today) & " Days"

        ''                pWkSheet.Range("D" & pRowCount.ToString()).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                pWkSheet.Range("E" & pRowCount.ToString()).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                pWkSheet.Range("F" & pRowCount.ToString()).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                pWkSheet.Range("G" & pRowCount.ToString()).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                pWkSheet.Range("H" & pRowCount.ToString()).Interior.Color = Color.FromArgb(146, 208, 80)
        ''            End If

        ''            For k As Integer = 0 To pTestProject.Test_MO.Count - 1
        ''                pRowCount = pRowCount + 1
        ''                pExcelCellRange = pWkSheet.Range("B" & pRowCount + k) : pExcelCellRange.Value = pTestProject.Test_MO(k).No

        ''                For l As Integer = 0 To pTestProject.Test_MO(k).Test_Report.Count - 1
        ''                    pExcelCellRange = pWkSheet.Range("C" & pRowCount + l) : pExcelCellRange.Value = pTestProject.Test_MO(k).Test_Report(l).No

        ''                    Dim pReportStatus As String = "Open"
        ''                    If (pTestProject.Test_MO(k).Test_Report(l).TesterSigned And pTestProject.Test_MO(k).Test_Report(l).EnggSigned And
        ''                        pTestProject.Test_MO(k).Test_Report(l).QualitySigned) Then
        ''                        pReportStatus = "Closed"
        ''                    End If

        ''                    pExcelCellRange = pWkSheet.Range("D" & pRowCount + l) : pExcelCellRange.Value = pReportStatus

        ''                    '....Determine
        ''                    pTestProject.Test_MO(k).Test_Report(l).SetTestStatus(pTestProject.Test_Spec.LeakMax, pTestProject.Test_Spec.LeakSpringBackMin, pTestProject.Test_Spec.LoadType, pTestProject.Test_Spec.Load(1), pTestProject.Test_Spec.Load(0))

        ''                    pExcelCellRange = pWkSheet.Range("E" & pRowCount + l) : pExcelCellRange.Value = pTestProject.Test_MO(k).Test_Report(l).DetermineOverallStatus().ToString()

        ''                    If (pTestProject.Test_MO(k).Test_Report(l).DateOpen() <> pMinDate) Then
        ''                        pExcelCellRange = pWkSheet.Range("F" & pRowCount + l) : pExcelCellRange.Value = pTestProject.Test_MO(k).Test_Report(l).DateOpen().ToShortDateString()
        ''                    Else
        ''                        pExcelCellRange = pWkSheet.Range("F" & pRowCount + l) : pExcelCellRange.Value = ""
        ''                    End If

        ''                    If (pTestProject.Test_MO(k).Test_Report(l).GetClosedDate() <> pMinDate) Then
        ''                        pExcelCellRange = pWkSheet.Range("G" & pRowCount + l) : pExcelCellRange.Value = pTestProject.Test_MO(k).Test_Report(l).GetClosedDate().ToShortDateString()
        ''                    Else
        ''                        pExcelCellRange = pWkSheet.Range("G" & pRowCount + l) : pExcelCellRange.Value = ""
        ''                    End If

        ''                    If ((pTestProject.Test_MO(k).Test_Report(l).DateOpen() <> pMinDate) And (pTestProject.Test_MO(k).Test_Report(l).GetClosedDate() = pMinDate)) Then
        ''                        Dim pDays As Integer = DateDiff(DateInterval.Day, pTestProject.Test_MO(k).Test_Report(l).DateOpen(), DateTime.Today())
        ''                        pExcelCellRange = pWkSheet.Range("H" & pRowCount + l) : pExcelCellRange.Value = DateDiff(DateInterval.Day, pTestProject.Test_MO(k).Test_Report(l).DateOpen(), DateTime.Today()) & " Days"
        ''                    Else
        ''                        pExcelCellRange = pWkSheet.Range("H" & pRowCount + l) : pExcelCellRange.Value = ""
        ''                    End If

        ''                    If (pReportStatus = "Open") Then
        ''                        pWkSheet.Range("C" & pRowCount + l).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                        pWkSheet.Range("D" & pRowCount + l).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                        pWkSheet.Range("E" & pRowCount + l).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                        pWkSheet.Range("F" & pRowCount + l).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                        pWkSheet.Range("G" & pRowCount + l).Interior.Color = Color.FromArgb(146, 208, 80)
        ''                        pWkSheet.Range("H" & pRowCount + l).Interior.Color = Color.FromArgb(146, 208, 80)

        ''                        pWkSheet.Range("D" & pRowCount + l).Font.Color = Color.Red

        ''                    Else
        ''                        pWkSheet.Range("D" & pRowCount + l).Font.Color = Color.DodgerBlue
        ''                        pWkSheet.Range("E" & pRowCount + l).Font.Color = Color.DodgerBlue

        ''                    End If

        ''                Next

        ''            Next

        ''            pRowCount = pRowCount + 2
        ''        Next
        ''        pRowCount = pRowCount + 1

        ''    Next


        ''Catch ex As Exception

        ''Finally
        ''    'pWkbOrg.Close()
        ''    'pApp.Quit()
        ''    pApp.Visible = True
        ''End Try

    End Sub


    Public Sub RetrieveFrom_DB()
        '=======================
        Dim pSealTestEntities As New SealTestDBEntities()

        '....tblTestFile
        Dim pQry = (From pRec In pSealTestEntities.tblFile
                          Where pRec.fldID = 1 Select pRec).ToList()

        If (pQry.Count > 0) Then
            mFileName_EquipList = pQry(0).fldFileName_EquipList
            mFileName_LeakProcedure = pQry(0).fldFileName_LeakProcedure
            mFileName_LoadProcedure = pQry(0).fldFileName_LoadProcedure

        End If

    End Sub


    Public Sub SaveTo_DB()
        '===================
        Try
            Dim pSealTestEntities As New SealTestDBEntities()
            Dim pRecExists As Boolean = False
            '....tblTestFile
            Dim pQry = (From pRec In pSealTestEntities.tblFile
                             Where pRec.fldID = 1 Select pRec).ToList()

            Dim pTestFile As New tblFile

            If (pQry.Count > 0) Then
                pTestFile = pQry(0)
                pRecExists = True
            End If

            With pTestFile
                .fldID = 1
                .fldFileName_EquipList = mFileName_EquipList
                .fldFileName_LeakProcedure = mFileName_LeakProcedure
                .fldFileName_LoadProcedure = mFileName_LoadProcedure


            End With

            If (pRecExists) Then
                pSealTestEntities.SaveChanges()
            Else
                pSealTestEntities.AddTotblFile(pTestFile)
                pSealTestEntities.SaveChanges()

            End If

        Catch ex As Exception

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
