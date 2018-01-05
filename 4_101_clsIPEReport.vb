
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsReport                              '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY17                                '
'                                                                              '
'===============================================================================


Imports Microsoft.Office.Interop
Imports System.Data
Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Drawing.Graphics
Imports System.Globalization
Imports System.Threading
Imports System.Linq
Imports WORD = Microsoft.Office.Interop.Word
Imports Microsoft.Office.Interop.Word
Imports Microsoft.Office.Interop.PowerPoint
Imports Microsoft.Office.Core
Imports System.Reflection
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports clsLibrary11
Imports System.Windows.Forms


Public Class IPE_clsReport

#Region "ENUMERATION TYPES:"

    Enum eDocType
        DOC
        PDF
    End Enum

#End Region


#Region "REPORT TEMPLATE FILES LOCATION: "

    'Report document File:
    '---------------------
    Private Const mcDirTemplates As String = "C:\SealSuite\SealIPE\Templates\"

    '....V8.0 files:    
    Private Const mcResultsDotFileName_ESeal As String = mcDirTemplates & "ReportResultsESealRev05e.dot"
    Private Const mcResultsDotFileName_CSeal As String = mcDirTemplates & "ReportResultsCSealRev05d.dot"
    Private Const mcResultsDotFileName_USeal As String = mcDirTemplates & "ReportResultsUSealRev01b.dot"

    '....Summary Table  
    Private Const mcSummaryTableDotFileName As String = mcDirTemplates & "ReportSummaryTableRev01a.dot"

    '....Adjust Geometry
    Private Const mcAdjGeomDotFileName As String = mcDirTemplates & "ReportAdjGeomRev01.dot"

    '....PowerPoint Template:
    Private Const mcPowerPoint_Template As String = mcDirTemplates & "ReportTemplate_Rev2.ppt"

    '....FatigueData_BL:
    Private Const mcFatigueDataBL_Template As String = mcDirTemplates & "FatigueData_BL.xlt"

    '....FatigueData_Additional:
    Private Const mcFatigueDataAdditional_Template As String = mcDirTemplates & "FatigueData_Additional.xlt"

#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"
    '....This is Used for Date Format in WriteReport,CreateAdjGeomDoc and SummaryTable

    Private mCI As New CultureInfo("en-US")
    Private mDocuFormat As eDocType

#End Region


#Region "CLASS PROPERTY ROUTINE:"

    '....READ & WRITE  PROPERTIES     
    '......Docu format .DOC or .PDF (Not used now)
    Public Property DocuFormat() As eDocType
        '===================================
        Get
            Return mDocuFormat
        End Get

        Set(ByVal strData As eDocType)
            mDocuFormat = strData
        End Set

    End Property

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New()
        '===========

    End Sub

#End Region


#Region "UTILITY ROUTINES:"


    Public Sub WriteFatigueData(ByVal Analysis_Cur_In As Integer, ByVal Project_In As IPE_clsProject)
        '=========================================================================================

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()
        Dim mobjMissing As Object = Missing.Value

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing

        Dim SX_Out As List(Of Double) = New List(Of Double)
        Dim SY_Out As List(Of Double) = New List(Of Double)
        Dim SZ_Out As List(Of Double) = New List(Of Double)

        Dim SX_Out_Assembly As List(Of Double) = New List(Of Double)
        Dim SY_Out_Assembly As List(Of Double) = New List(Of Double)
        Dim SZ_Out_Assembly As List(Of Double) = New List(Of Double)


        If (Project_In.Analysis(Analysis_Cur_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then

            pWkbOrg = pApp.Workbooks.Open(mcFatigueDataBL_Template, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value, _
                                          Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
                                          Missing.Value, Missing.Value, Missing.Value)

            modMain_IPE.gIPE_File.ReadFile_ANSYS_FatigueData("BL.Out", SX_Out, SY_Out, SZ_Out)

        ElseIf (Project_In.Analysis(Analysis_Cur_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Additional) Then

            pWkbOrg = pApp.Workbooks.Open(mcFatigueDataAdditional_Template, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value, _
                                         Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
                                         Missing.Value, Missing.Value, Missing.Value)

            modMain_IPE.gIPE_File.ReadFile_ANSYS_FatigueData("A1.Out", SX_Out, SY_Out, SZ_Out)

        End If

        modMain_IPE.gIPE_File.ReadFile_ANSYS_FatigueData("Assembly.Out", SX_Out_Assembly, SY_Out_Assembly, SZ_Out_Assembly)

        Dim pSVal As List(Of Double) = New List(Of Double)
        Dim pSVal_Assembly As List(Of Double) = New List(Of Double)
        Dim pSheetName As String = ""

        Dim pICur As Integer = Analysis_Cur_In
        Dim pAnaDesc As String = ""

        Dim pMCS As String = gIPE_Project.Analysis(pICur).Seal.MCrossSecNo
        If (pMCS <> "") Then
            pAnaDesc = "MCS" & pMCS
        End If
        Dim pFreeHt As String = gIPE_Unit.WriteInUserL(gIPE_Project.Analysis(pICur).Seal.Hfree).ToString().Replace(".", "")
        If (pFreeHt <> "") Then
            pAnaDesc = pAnaDesc & "_FH" & pFreeHt
        End If
        Dim pLoadCaseName As String = gIPE_Project.Analysis(pICur).LoadCase.Name
        If (pLoadCaseName <> "") Then
            pAnaDesc = pAnaDesc & "_" & pLoadCaseName
        End If

        pAnaDesc = pAnaDesc & "_Fatigue"

        Dim pDate As String = ""
        Dim pTime As String = ""
        If (gIPE_Project.Analysis(pICur).DateCreated <> Date.MinValue) Then
            pDate = gIPE_Project.Analysis(pICur).DateCreated.ToString("ddMMMyy")
            pTime = gIPE_Project.Analysis(pICur).TimeCreated.ToString("t").Replace(":", "").Trim().Replace(" ", "")
            pAnaDesc = pAnaDesc & "_" & pDate & "_" & pTime
        End If

        For i As Integer = 0 To 2
            If (i = 0) Then
                pSheetName = "SX"
                pSVal = SX_Out
                pSVal_Assembly = SX_Out_Assembly

            ElseIf (i = 1) Then
                pSheetName = "SY"
                pSVal = SY_Out
                pSVal_Assembly = SY_Out_Assembly

            ElseIf (i = 2) Then
                pSheetName = "SZ"
                pSVal = SZ_Out
                pSVal_Assembly = SZ_Out_Assembly

            End If

            Dim pWkSheet As EXCEL.Worksheet = DirectCast(pWkbOrg.Sheets(pSheetName), EXCEL.Worksheet)

            Dim pRange As Object
            pRange = pWkSheet.Range("B1") : pRange.value = Project_In.Customer()
            pRange = pWkSheet.Range("D1") : pRange.value = Project_In.Platform()
            pRange = pWkSheet.Range("F1") : pRange.value = Project_In.CustomerPN()
            pRange = pWkSheet.Range("H1") : pRange.value = Project_In.ParkerPN()
            pRange = pWkSheet.Range("B2") : pRange.value = Project_In.Analysis(pICur).Seal.MCrossSecNo
            pRange = pWkSheet.Range("D2") : pRange.value = Project_In.Analysis(pICur).Seal.Adjusted

            If gIPE_Project.Analysis(pICur).Seal.IsSegmented Then
                pRange = pWkSheet.Range("F2") : pRange.value = "Y"
            Else
                pRange = pWkSheet.Range("F2") : pRange.value = "N"
            End If

            pRange = pWkSheet.Range("B3") : pRange.value = pAnaDesc

            Dim pMaxDiff As Double = 0
            Dim pRowIndex As Integer = 0
            For j As Integer = 0 To pSVal.Count - 1

                pWkSheet.Range("A" & j + 7).Value = j + 1
                pWkSheet.Range("B" & j + 7).Value = pSVal(j)
                pWkSheet.Range("C" & j + 7).Value = pSVal_Assembly(j)

                Dim pDiff As Double = Math.Abs(pSVal(j) - pSVal_Assembly(j))

                pWkSheet.Range("D" & j + 7).Value = pDiff
                pWkSheet.Range("E" & j + 7).Value = (pSVal(j) + pSVal_Assembly(j)) / 2
                pWkSheet.Range("F" & j + 7).Value = pDiff / 2

                If pDiff > pMaxDiff Then
                    pMaxDiff = pDiff
                    pRowIndex = j + 7
                End If

            Next

            pWkSheet.Range("G" & pRowIndex).Value = "Max. Diff."

            pWkSheet.Range("A" & pRowIndex).Interior.Color = Color.YellowGreen
            pWkSheet.Range("B" & pRowIndex).Interior.Color = Color.YellowGreen
            pWkSheet.Range("C" & pRowIndex).Interior.Color = Color.YellowGreen
            pWkSheet.Range("D" & pRowIndex).Interior.Color = Color.YellowGreen
            pWkSheet.Range("E" & pRowIndex).Interior.Color = Color.YellowGreen
            pWkSheet.Range("F" & pRowIndex).Interior.Color = Color.YellowGreen
            pWkSheet.Range("G" & pRowIndex).Interior.Color = Color.YellowGreen

        Next

        Dim pAccessMode As EXCEL.XlSaveAsAccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive
        Dim pFileName As String = gIPE_File.DirOut & pAnaDesc & ".xls"

        pWkbOrg.SaveAs(pFileName, pWkbOrg.FileFormat, mobjMissing, mobjMissing, False, mobjMissing, _
                        pAccessMode, mobjMissing, mobjMissing, mobjMissing, mobjMissing, mobjMissing)

        pApp.Visible = True

    End Sub

    'Public Sub WriteReport(ByVal UserInfo_In As clsUser, ByVal Project_In As clsProject, ByVal Unit_In As clsUnit, ByVal FileName_In As String)
    Public Sub WriteReport(ByVal Result_Sel_In As Integer, ByVal UserInfo_In As IPE_clsUser,
                           ByVal Project_In As IPE_clsProject, ByVal Unit_In As IPE_clsUnit, ByVal FileName_In As String)
        '==========================================================================================================
        '....This routine creates a report corresponding to the selected record on the DBGrid control .

        Dim pWordApp As New WORD.Application()
        Dim pWordDoc As WORD.Document = Nothing

        Try

            If Project_In.Analysis(Result_Sel_In).Seal.Type = "E-Seal" Then
                pWordDoc = pWordApp.Documents.Add(mcResultsDotFileName_ESeal)

            ElseIf Project_In.Analysis(Result_Sel_In).Seal.Type = "C-Seal" Then
                pWordDoc = pWordApp.Documents.Add(mcResultsDotFileName_CSeal)

            ElseIf Project_In.Analysis(Result_Sel_In).Seal.Type = "U-Seal" Then
                pWordDoc = pWordApp.Documents.Add(mcResultsDotFileName_USeal)
            End If

            Dim psngWt As Single
            Dim psngFConUnit As Single
            Dim pstrFConUnit As String = ""
            Dim pSpringBack As Integer = 0

            With pWordDoc
                .Bookmarks.Item("Customer").Range.Text = Project_In.Customer()

                With pWordDoc.Bookmarks.Item("Date").Range
                    .Text = Today.ToString("MMMM dd, yyyy", mCI.DateTimeFormat()) 'US Format only

                    '.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight
                    '.Font.Size = 12
                End With

                '....User Name & Phone No
                .Bookmarks.Item("ProducedBy").Range.Text = UserInfo_In.Name
                .Bookmarks.Item("PhoneNo").Range.Text = Trim(UserInfo_In.PhoneNo)
                .Bookmarks.Item("EndOfLine1").Range.Delete()


                'Input Parameters :
                '------------------

                'Applied Conditions:
                '------------------
                .Bookmarks.Item("SealOrient").Range.Text = Project_In.Analysis(Result_Sel_In).OpCond.POrient

                '....Pressure.
                Dim pPress As String = 0
                pPress = Unit_In.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff)

                If pPress = "" Then pPress = 0

                .Bookmarks.Item("Pressure").Range.Text = pPress
                .Bookmarks.Item("UnitP").Range.Text = Unit_In.UserP

                '....Temperature.
                .Bookmarks.Item("Temp").Range.Text = CStr(NInt(Project_In.Analysis(Result_Sel_In).OpCond.TOper))
                .Bookmarks.Item("UnitT").Range.Text = Unit_In.T

                '....Installation Compression
                .Bookmarks.Item("CompInst").Range.Text = Unit_In.WriteInUserL(Project_In.Analysis(Result_Sel_In).Compression.Val)

                .Bookmarks.Item("UnitComp").Range.Text = Unit_In.UserL & ", "

                .Bookmarks.Item("CompPcnt").Range.Text = Format(Project_In.Analysis(Result_Sel_In).Compression.PcentVal, "#0.0") & " %"

                '....Compression Tolerance Statement:                
                '
                Dim pTolType_Hfree As String, pHfree_Actual As Single
                Dim pTolType_CavityDepth As String = "", pCavityDepth_Actual As Single

                pTolType_Hfree = Project_In.Analysis(Result_Sel_In).Compression.TolType

                Select Case Project_In.Analysis(Result_Sel_In).Compression.TolType

                    Case "Minimum"
                        '---------
                        pTolType_CavityDepth = "Maximum"
                        pCavityDepth_Actual = Project_In.Analysis(Result_Sel_In).Cavity.Depth + Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(2)
                        pHfree_Actual = Project_In.Analysis(Result_Sel_In).Seal.Hfree - Project_In.Analysis(Result_Sel_In).Seal.HFreeTol(1)

                    Case "Nominal"
                        '---------
                        pTolType_CavityDepth = "Nominal"
                        pCavityDepth_Actual = Project_In.Analysis(Result_Sel_In).Cavity.Depth
                        pHfree_Actual = Project_In.Analysis(Result_Sel_In).Seal.Hfree

                    Case "Maximum"
                        '---------
                        pTolType_CavityDepth = "Minimum"
                        pCavityDepth_Actual = Project_In.Analysis(Result_Sel_In).Cavity.Depth - Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(1)
                        pHfree_Actual = Project_In.Analysis(Result_Sel_In).Seal.Hfree + Project_In.Analysis(Result_Sel_In).Seal.HFreeTol(2)

                End Select

                Dim pstrAny As String
                pstrAny = "Analysis Represents the " & pTolType_Hfree & " Seal Free Height Compressed in the " & _
                                                       pTolType_CavityDepth & " Cavity Depth."
                .Bookmarks.Item("AppCondNote").Range.Text = pstrAny


                'Cavity Dimensions
                '-----------------
                .Bookmarks.Item("UnitL").Range.Text = Trim(Unit_In.UserL)

                '....Min. OD & Max. ID:
                .Bookmarks.Item("MinOD").Range.Text = Unit_In.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Dia(2))
                .Bookmarks.Item("MaxID").Range.Text = Unit_In.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Dia(1))
                .Bookmarks.Item("EndOfLine3").Range.Delete()

                '....Width & Depth 
                .Bookmarks.Item("MinWid").Range.Text = Unit_In.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.WidMin)
                .Bookmarks.Item("Depth").Range.Text = Unit_In.WriteInUserL(pCavityDepth_Actual)
                .Bookmarks.Item("EndOfLine4").Range.Delete()


                'General Seal Information:
                '------------------------
                .Bookmarks.Item("CrossSecNo").Range.Text = Project_In.Analysis(Result_Sel_In).Seal.MCrossSecNo
                .Bookmarks.Item("EndOfLine5").Range.Delete()

                '....Material
                .Bookmarks.Item("Mat").Range.Text = Project_In.Analysis(Result_Sel_In).Seal.Mat.Name
                .Bookmarks.Item("MatThick").Range.Text = Unit_In.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.T)

                '....Weight
                psngWt = Project_In.Analysis(Result_Sel_In).Seal.Wt(Project_In.Analysis(Result_Sel_In).Seal.Mat.SpWt)
                .Bookmarks.Item("Wt").Range.Text = Format(psngWt, "##0.00") & Space(2) & Unit_In.UserWt

                If Project_In.Analysis(Result_Sel_In).Seal.Type = "E-Seal" Then
                    .Bookmarks.Item("Coating").Range.Text = Project_In.Analysis(Result_Sel_In).Seal.Mat.Coating '....Coating

                    If Project_In.Analysis(Result_Sel_In).Seal.Mat.Coating = "T800" Then
                        .Bookmarks.Item("SFinish").Range.Text = "Surface Finish: " & _
                                  CType(Project_In.Analysis(Result_Sel_In).Seal, IPE_clsESeal).SFinish & Space(2) & "rms"
                    End If

                ElseIf Project_In.Analysis(Result_Sel_In).Seal.Type = "C-Seal" Then
                    .Bookmarks.Item("Plating").Range.Text = CType(Project_In.Analysis(Result_Sel_In).Seal, IPE_clsCSeal).Plating.Code

                End If

                '....Diameter
                .Bookmarks.Item("Dia").Range.Text = Unit_In.WriteInUserL((Project_In.Analysis(Result_Sel_In).Seal.DControl))

                '....Free Height
                .Bookmarks.Item("HFree").Range.Text = Unit_In.WriteInUserL(pHfree_Actual)


                'Seal Performance
                '-----------------   
                '....Load Case.        
                'Dim pIndx As Integer
                'Select Case Project_In.Analysis(Result_Sel_In).Seal.LoadCase
                '    Case "A"
                '        pIndx = 0
                '    Case "B"
                '        pIndx = 1
                '    Case "C"
                '        pIndx = 2
                'End Select
                '.Bookmarks.Item("LoadCase").Range.Text = clsSeal.LoadCaseDesc(pIndx)
                .Bookmarks.Item("LoadCase").Range.Text = Project_In.Analysis(Result_Sel_In).LoadCase.Type.ToString()

                '....Seating Load.

                '....SeatingLoad1   'ASSEMBLY
                If (Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    If (Project_In.Analysis(Result_Sel_In).AppLoad.PreComp.Exists = True) Then
                        psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(1) * Unit_In.CFacUserL       '....User Unit.
                    Else
                        psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(0) * Unit_In.CFacUserL       '....User Unit.
                    End If
                Else
                    psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(0) * Unit_In.CFacUserL       '....User Unit.
                End If
                'psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FConUnit(1) * Unit_In.CFacUserL       '....User Unit.

                If psngFConUnit <= 10.0# Then
                    pstrFConUnit = Format(psngFConUnit, "##.00")

                ElseIf psngFConUnit > 10.0# Then
                    pstrFConUnit = Format(NInt(psngFConUnit), "##,##0")
                End If

                .Bookmarks.Item("SeatingLoad1").Range.Text = pstrFConUnit
                .Bookmarks.Item("UnitSL1").Range.Text = Unit_In.F & "/" & Unit_In.UserL

                '....SeatingLoad5   'BASELINE
                If (Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    If (Project_In.Analysis(Result_Sel_In).AppLoad.PreComp.Exists = True) Then
                        psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(2) * Unit_In.CFacUserL       '....User Unit.
                    Else
                        psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(1) * Unit_In.CFacUserL       '....User Unit.
                    End If
                Else
                    psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(1) * Unit_In.CFacUserL       '....User Unit.
                End If
                'psngFConUnit = Project_In.Analysis(Result_Sel_In).Seal.FConUnit(5) * Unit_In.CFacUserL       '....User Unit.


                If psngFConUnit <= 10.0# Then
                    pstrFConUnit = Format(psngFConUnit, "##.00")
                ElseIf psngFConUnit > 10.0# Then
                    pstrFConUnit = Format(NInt(psngFConUnit), "##,##0")
                End If


                Dim pstrOpCond As String
                'pstrOpCond = "(" & Unit_In.FormatPDiffUnitUser(AppCondSel_In.PDiff) & _
                '            Space(1) & Unit_In.UserP & ", " & NInt(AppCondSel_In.TOper) & _
                '            Space(1) & Unit_In.T & ")"

                'If Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "A" Or Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "B" Then
                '    pPress = 0
                'End If

                pstrOpCond = "(BL: " & pPress & Space(1) & Unit_In.UserP & ", " & NInt(Project_In.Analysis(Result_Sel_In).OpCond.TOper) & _
                              Space(1) & Unit_In.T & ")"


                .Bookmarks.Item("SeatingLoad5").Range.Text = pstrFConUnit '& Space(2) & _

                .Bookmarks.Item("UnitSL5").Range.Text = Unit_In.F & "/" & _
                                                      Unit_In.UserL
                .Bookmarks.Item("NoteSL5").Range.Text = pstrOpCond



                '....Max Stresses
                .Bookmarks.Item("MaxStress1").Range.Text = ""

                'ASSEMBLY
                Dim psngMaxStress1 As Single = 0.0

                If (Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    If (Project_In.Analysis(Result_Sel_In).AppLoad.PreComp.Exists = True) Then
                        psngMaxStress1 = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(1)))
                    Else
                        psngMaxStress1 = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(0)))
                    End If
                Else
                    psngMaxStress1 = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(0)))
                End If

                'psngMaxStress1 = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(1)))

                .Bookmarks.Item("MaxStress1").Range.Text = Format(psngMaxStress1, "##,##0")        '....User Unit.  
                .Bookmarks.Item("UnitMS1").Range.Text = Unit_In.UserStress

                Dim psngMaxStress_Last As Single = 0.0

                If (Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline) Then
                    If (Project_In.Analysis(Result_Sel_In).AppLoad.PreComp.Exists = True) Then
                        psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(2)))
                    Else
                        psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(1)))
                    End If
                Else
                    psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(1)))
                End If

                'psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(Result_Sel_In).Seal.StressMax(5)))

                .Bookmarks.Item("MaxStress5").Range.Text = Format(psngMaxStress_Last, "##,##0")
                .Bookmarks.Item("UnitMS5").Range.Text = Unit_In.UserStress
                .Bookmarks.Item("NoteMS5").Range.Text = pstrOpCond

                'Dim pSpringBack As Integer = 0
                'pSpringBack = gIPE_Project.Analysis(i).Seal.SpringBack(gIPE_Project.Analysis(i).Compression.TolType,
                '                                                   gIPE_Project.Analysis(i).Cavity.DepthActual(gIPE_Project.Analysis(i).Compression.TolType),
                '                                                   gIPE_Project.Analysis(i).Compression.Val)


                '....Spring Back
                'If Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "B" Or Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "C" Then
                Dim pTolType As String = Project_In.Analysis(Result_Sel_In).Compression.TolType
                pSpringBack = Project_In.Analysis(Result_Sel_In).Seal.SpringBack(pTolType, _
                                                     Project_In.Analysis(Result_Sel_In).Cavity.DepthActual(pTolType), _
                                                     Project_In.Analysis(Result_Sel_In).Compression.Val)
                'End If

                .Bookmarks.Item("SpringBack").Range.Text = pSpringBack & " %"

                'If Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "B" Or Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "C" Then
                Dim pHFreeFinal As Single = Project_In.Analysis(Result_Sel_In).Result_Gen.HFreeFinal 'Project_In.Analysis(Result_Sel_In).Seal.HActual("Final", Project_In.Analysis(Result_Sel_In).Compression.TolType)
                Dim pNote As String = "(Free Height: "
                .Bookmarks.Item("Note_SpringBack").Range.Text = pNote & _
                                                                Unit_In.WriteInUserL(pHFreeFinal) & ")"
                'End If

                If Project_In.Analysis(Result_Sel_In).Seal.Type = "E-Seal" Then
                    Dim pLeakage_Oper As Single = 0.0
                    Dim pUnitUser_Leak As String = ""

                    Dim pCoating As String
                    pCoating = Project_In.Analysis(Result_Sel_In).Seal.Mat.Coating

                    'If Project_In.Analysis(Result_Sel_In).Seal.LoadCase = "C" Then
                    pLeakage_Oper = CType(Project_In.Analysis(Result_Sel_In).Seal, IPE_clsESeal).Leakage_Oper(Project_In.Analysis(Result_Sel_In).OpCond.PDiff, pCoating)
                    pUnitUser_Leak = Unit_In.UserLeakage
                    'End If

                    .Bookmarks.Item("Leakage_Oper").Range.Text = Format(pLeakage_Oper, "#0.00")
                    .Bookmarks.Item("LeakageUnit_Oper").Range.Text = pUnitUser_Leak
                    .Bookmarks.Item("NoteLeakage_Oper").Range.Text = pstrOpCond

                End If

            End With


            With pWordApp

                If mDocuFormat = eDocType.PDF Then

                    pWordDoc.SaveAs2(gIPE_File.DirOut & "Doc", WdSaveFormat.wdFormatDocument97)

                    Dim paramExportFormat As WdExportFormat = WdExportFormat.wdExportFormatPDF
                    Dim paramMissing As Object = Type.Missing
                    Dim paramOpenAfterExport As Boolean = True
                    Dim paramExportOptimizeFor As WdExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint
                    Dim paramExportRange As WdExportRange = WdExportRange.wdExportAllDocument
                    Dim paramStartPage As Integer = 0
                    Dim paramEndPage As Integer = 0
                    Dim paramExportItem As WdExportItem = WdExportItem.wdExportDocumentContent
                    Dim paramIncludeDocProps As Boolean = True
                    Dim paramKeepIRM As Boolean = True
                    Dim paramCreateBookmarks As WdExportCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks
                    Dim paramDocStructureTags As Boolean = True
                    Dim paramBitmapMissingFonts As Boolean = True
                    Dim paramUseISO19005_1 As Boolean = False

                    pWordDoc.ExportAsFixedFormat(FileName_In, paramExportFormat, paramOpenAfterExport, paramExportOptimizeFor, _
                                                 paramExportRange, paramStartPage, paramEndPage, paramExportItem, _
                                                 paramIncludeDocProps, paramKeepIRM, paramCreateBookmarks, paramDocStructureTags, _
                                                 paramBitmapMissingFonts, paramUseISO19005_1, paramMissing)

                    pWordDoc.Close(WORD.WdSaveOptions.wdSaveChanges)    '..Close Document first
                    .Quit(WORD.WdSaveOptions.wdSaveChanges)             '..then Quit Word application

                    File.Delete(gIPE_File.DirOut & "Doc.doc")

                ElseIf mDocuFormat = eDocType.DOC Then

                    pWordDoc.SaveAs2(FileName_In, WdSaveFormat.wdFormatDocument97)
                    .Visible = True
                    .WindowState = WORD.WdWindowState.wdWindowStateMaximize

                End If

            End With


        Catch pEXP As Exception

            Dim pstrTitle, pstrMsg As String
            Dim pintAttributes As Integer
            Dim pintAnswer As Integer

            pstrTitle = "ERROR MESSAGE: "
            pstrMsg = "Error in clsReport"
            pintAttributes = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly
            pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

        Finally

            pWordApp = Nothing
            pWordDoc = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()

        End Try

    End Sub


    Public Sub CratePowerPoint_Report(ByVal Result_Sel_In As Integer, ByVal LoadStep_In As List(Of Boolean),
                                      ByVal ThermalGrowth_In As Boolean, ByVal Project_In As IPE_clsProject)
        '===============================================================================================
        Try

            Dim pApp As New Microsoft.Office.Interop.PowerPoint.Application()
            Dim pPresentations As Microsoft.Office.Interop.PowerPoint.Presentations = pApp.Presentations

            Dim pCurPresentation As Microsoft.Office.Interop.PowerPoint.Presentation = pPresentations.Open(mcPowerPoint_Template, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue)
            Dim pSlides As Microsoft.Office.Interop.PowerPoint.Slides = pCurPresentation.Slides

            Dim pSlide As Microsoft.Office.Interop.PowerPoint._Slide
            Dim pShapes As Microsoft.Office.Interop.PowerPoint.Shapes

            Dim pText As Microsoft.Office.Interop.PowerPoint.TextRange

            Dim pLoadStep As New List(Of IPE_clsAnalysis.sLoadStep)
            pLoadStep = Project_In.Analysis(Result_Sel_In).LoadStep()

            '---------------
            'AES 03MAR17
            '....Compression Tolerance Statement:                
            '
            Dim pHfree_Actual As Single
            Dim pCavityDepth_Actual As Single


            Select Case Project_In.Analysis(Result_Sel_In).Compression.TolType

                Case "Minimum"
                    '---------
                    pCavityDepth_Actual = Project_In.Analysis(Result_Sel_In).Cavity.Depth + Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(2)
                    pHfree_Actual = Project_In.Analysis(Result_Sel_In).Seal.Hfree - Project_In.Analysis(Result_Sel_In).Seal.HFreeTol(1)

                Case "Nominal"
                    '---------
                    pCavityDepth_Actual = Project_In.Analysis(Result_Sel_In).Cavity.Depth
                    pHfree_Actual = Project_In.Analysis(Result_Sel_In).Seal.Hfree

                Case "Maximum"
                    '---------
                    pCavityDepth_Actual = Project_In.Analysis(Result_Sel_In).Cavity.Depth - Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(1)
                    pHfree_Actual = Project_In.Analysis(Result_Sel_In).Seal.Hfree + Project_In.Analysis(Result_Sel_In).Seal.HFreeTol(2)

            End Select

            'AES 07MAR17
            Dim pAddLoadCavityDepth As New List(Of Single)
            For k As Integer = 0 To Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad.Count - 1
                Dim pCavityDepth As Single
                Select Case Project_In.Analysis(Result_Sel_In).Compression.TolType

                    Case "Minimum"
                        '---------
                        pCavityDepth = Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).CavityDepth + Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(2)

                    Case "Nominal"
                        '---------
                        pCavityDepth = Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).CavityDepth

                    Case "Maximum"
                        '---------
                        pCavityDepth = Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).CavityDepth - Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(1)

                End Select
                pAddLoadCavityDepth.Add(pCavityDepth)

            Next
            '---------------

            For i As Integer = 0 To pSlides.Count - 1

                'Slide #1
                '-----------
                If i = 0 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            pText = pShape1.TextFrame.TextRange
                            pText.Text = Project_In.Analysis(Result_Sel_In).Seal.Type & " Analysis" & vbLf & _
                                         Project_In.Customer().Trim() & " " & Project_In.Platform().Trim() & " " & Project_In.Location().Trim() & vbLf & _
                                        "PN " & Project_In.CustomerPN & vbLf & "Parker PN " & Project_In.ParkerPN()
                        ElseIf j1 = 2 Then
                            pText = pShape1.TextFrame.TextRange
                            pText.Text = "Performed by: " & modMain_IPE.gIPE_User.Name & vbLf & "Date: " & DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) '("ddMMMyy")
                            'DateTime.Now.ToShortDateString()
                        End If
                    Next
                End If

                'Slide #2
                '-----------
                If i = 1 Then

                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'CAVITY.
                            '-------  
                            'pSlide.Shapes.Range.PictureFormat.
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            'pSlide.Shapes.AddPicture("C:\SealIPE\Templates\cavity.jpg", False, True, 150, 150, pShape.Width, pShape.Height)
                            pSlide.Shapes.AddPicture(mcDirTemplates & "cavity.jpg", False, True, 230, 210, , )

                        ElseIf j1 = 3 Then

                            Dim psngDCavity As Single
                            If Project_In.Analysis(Result_Sel_In).Seal.POrient = "External" Then
                                psngDCavity = Project_In.Analysis(Result_Sel_In).Cavity.Dia(1)
                            ElseIf Project_In.Analysis(Result_Sel_In).Seal.POrient = "Internal" Then
                                psngDCavity = Project_In.Analysis(Result_Sel_In).Cavity.Dia(2)
                            End If

                            pText = pShape1.TextFrame.TextRange

                            'pText.Text = "Cavity D (Min) = " & modMain.gIPE_Unit.WriteInUserL(psngDCavity) & " " & modMain.gIPE_Unit.UserL & vbTab & _
                            '            "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(modMain.Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & modMain.gIPE_Unit.UserP & vbLf & _
                            '            vbTab & " W = " & modMain.gIPE_Unit.WriteInUserL(modMain.Project_In.Analysis(Result_Sel_In).Cavity.WidMin) & vbTab & Space(13) & _
                            '            "Temperature = " & modMain.Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                            '            vbTab & "  h = " & modMain.gIPE_Unit.WriteInUserL(modMain.Project_In.Analysis(Result_Sel_In).Cavity.Depth) & vbLf & _
                            '             Space(11) & "r (Max) = " & modMain.gIPE_Unit.WriteInUserL(modMain.Project_In.Analysis(Result_Sel_In).Seal.CavityCornerRad)

                            'AES 03MAR17
                            pText.Text = "Cavity D (Min) = " & modMain_IPE.gIPE_Unit.WriteInUserL(psngDCavity) & " " & modMain_IPE.gIPE_Unit.UserL & vbTab & _
                                       "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & modMain_IPE.gIPE_Unit.UserP & vbLf & _
                                       vbTab & " W = " & modMain_IPE.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.WidMin) & vbTab & Space(13) & _
                                       "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                                       vbTab & "  h = " & modMain_IPE.gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & vbLf & _
                                        Space(11) & "r (Max) = " & modMain_IPE.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.CavityCornerRad)
                        End If
                    Next
                End If

                'Slide #3
                '-----------
                If i = 2 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    '....Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            pSlide.Shapes.AddPicture(mcDirTemplates & "Seal.jpg", False, True, 230, 210, 150 + pShape1.Width, 150 + pShape1.Height)

                        ElseIf j1 = 3 Then
                            pText = pShape1.TextFrame.TextRange
                            'pText.Text = Space(4) & "Material = " & Project_In.Analysis(Result_Sel_In).Seal.Mat.Name & _
                            '          Space(14) & " Width = " & gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Wid) & _
                            '           " " & modMain.gIPE_Unit.UserL & vbLf & _
                            '          " Thickness = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.T) & _
                            '          vbTab & "   Free Height = " & gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & vbLf & _
                            '         "Orientation = " & modMain.Project_In.Analysis(Result_Sel_In).Seal.POrient & vbLf & _
                            '         Space(3) & "Diameter = " & modMain.gIPE_Unit.WriteInUserL(modMain.Project_In.Analysis(Result_Sel_In).Seal.DControl)

                            'AES 03MAR17
                            pText.Text = Space(4) & "Material = " & Project_In.Analysis(Result_Sel_In).Seal.Mat.Name & _
                                      Space(14) & " Width = " & gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Wid) & _
                                       " " & modMain_IPE.gIPE_Unit.UserL & vbLf & _
                                      " Thickness = " & modMain_IPE.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.T) & _
                                      vbTab & "   Free Height = " & gIPE_Unit.WriteInUserL(pHfree_Actual) & vbLf & _
                                     "Orientation = " & Project_In.Analysis(Result_Sel_In).Seal.POrient & vbLf & _
                                     Space(3) & "Diameter = " & modMain_IPE.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.DControl)
                        End If
                    Next
                End If


                'Slide #4
                '-----------
                If i = 3 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    '....Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If (j1 = 2) Then
                            pSlide = pSlides(i + 1)

                            Dim pRowCount As Integer

                            pRowCount = pLoadStep.Count + 1

                            pSlide.Shapes.AddTable(pRowCount, 6, 150, 150, 400, 50)
                            'Dim pTbl As Microsoft.Office.Interop.PowerPoint.Table = pShapes(6).Table
                            Dim pTbl As Microsoft.Office.Interop.PowerPoint.Table = pShapes(7).Table

                            With pTbl
                                .Columns(1).Width = 40
                                .Columns(2).Width = 50
                                .Columns(3).Width = 50
                                .Columns(4).Width = 50
                                .Columns(5).Width = 100
                                .Columns(6).Width = 120
                            End With

                            With pTbl
                                .Cell(1, 1).Shape.TextFrame.TextRange.Text = vbLf & "Step"
                                .Cell(1, 1).Shape.TextFrame.TextRange.Font.Size = 10
                                .Cell(1, 1).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter
                                .Cell(1, 2).Shape.TextFrame.TextRange.Text = vbLf & "PDiff (" & modMain_IPE.gIPE_Unit.UserP & ")" ' (psid)"
                                .Cell(1, 2).Shape.TextFrame.TextRange.Font.Size = 10
                                .Cell(1, 2).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter
                                .Cell(1, 3).Shape.TextFrame.TextRange.Text = vbLf & "T" & vbLf & "(" & gIPE_Unit.T & ")"
                                .Cell(1, 3).Shape.TextFrame.TextRange.Font.Size = 10
                                .Cell(1, 3).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter
                                .Cell(1, 4).Shape.TextFrame.TextRange.Text = "Cavity Depth" & vbLf & "(" & gIPE_Unit.UserL & ")"
                                .Cell(1, 4).Shape.TextFrame.TextRange.Font.Size = 10
                                .Cell(1, 4).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter
                                .Cell(1, 5).Shape.TextFrame.TextRange.Text = vbLf & "Compression " & vbLf & "(" & Project_In.Analysis(Result_Sel_In).Compression.TolType & ")"
                                .Cell(1, 5).Shape.TextFrame.TextRange.Font.Size = 10
                                .Cell(1, 5).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter
                                .Cell(1, 6).Shape.TextFrame.TextRange.Text = vbLf & "Description"
                                .Cell(1, 6).Shape.TextFrame.TextRange.Font.Size = 10
                                .Cell(1, 6).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter
                                Dim pCurRowIndex = 2
                                For k As Integer = 0 To pLoadStep.Count - 1
                                    Dim pRowIndex As Integer = pCurRowIndex + k
                                    Dim pPCentVal As Double = (pLoadStep(k).CompressionVal / Project_In.Analysis(Result_Sel_In).Seal.Hfree) * 100.0#

                                    '....Step
                                    .Cell(pRowIndex, 1).Shape.TextFrame.TextRange.Text = (k + 1).ToString()
                                    .Cell(pRowIndex, 1).Shape.TextFrame.TextRange.Font.Size = 10
                                    .Cell(pRowIndex, 1).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignRight

                                    '....PDiff
                                    .Cell(pRowIndex, 2).Shape.TextFrame.TextRange.Text = gIPE_Unit.FormatPDiffUnitUser(pLoadStep(k).PDiff)
                                    .Cell(pRowIndex, 2).Shape.TextFrame.TextRange.Font.Size = 10
                                    .Cell(pRowIndex, 2).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignRight

                                    '....T
                                    .Cell(pRowIndex, 3).Shape.TextFrame.TextRange.Text = pLoadStep(k).T
                                    .Cell(pRowIndex, 3).Shape.TextFrame.TextRange.Font.Size = 10
                                    .Cell(pRowIndex, 3).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignRight

                                    '-------------------------
                                    '....Cavity Depth
                                    '.Cell(pRowIndex, 4).Shape.TextFrame.TextRange.Text = gIPE_Unit.WriteInUserL(pLoadStep(k).CavityDepth) '.ToString("#0.000")

                                    'AES 03MAR17
                                    '....Compression Tolerance Statement:                
                                    '
                                    Dim pCavityDepth_Actual_LoadStep As Single = pLoadStep(k).CavityDepth

                                    If (pLoadStep(k).Descrip = "Assembly" Or pLoadStep(k).Descrip = "BL" Or pLoadStep(k).Descrip = "Open") Then

                                        Select Case Project_In.Analysis(Result_Sel_In).Compression.TolType

                                            Case "Minimum"
                                                '---------
                                                pCavityDepth_Actual_LoadStep = pLoadStep(k).CavityDepth + Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(2)

                                            Case "Nominal"
                                                '---------
                                                pCavityDepth_Actual_LoadStep = pLoadStep(k).CavityDepth

                                            Case "Maximum"
                                                '---------
                                                pCavityDepth_Actual_LoadStep = pLoadStep(k).CavityDepth - Project_In.Analysis(Result_Sel_In).Cavity.DepthTol(1)

                                        End Select
                                    End If

                                    .Cell(pRowIndex, 4).Shape.TextFrame.TextRange.Text = gIPE_Unit.WriteInUserL(pCavityDepth_Actual_LoadStep) '.ToString("#0.000")

                                    '------------------

                                    .Cell(pRowIndex, 4).Shape.TextFrame.TextRange.Font.Size = 10
                                    .Cell(pRowIndex, 4).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignRight

                                    '....Compression
                                    .Cell(pRowIndex, 5).Shape.TextFrame.TextRange.Text = gIPE_Unit.WriteInUserL(pLoadStep(k).CompressionVal) & " (" & pPCentVal.ToString("#0.0") & "%)"
                                    .Cell(pRowIndex, 5).Shape.TextFrame.TextRange.Font.Size = 10
                                    .Cell(pRowIndex, 5).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignRight

                                    '....Description
                                    .Cell(pRowIndex, 6).Shape.TextFrame.TextRange.Text = pLoadStep(k).Descrip
                                    .Cell(pRowIndex, 6).Shape.TextFrame.TextRange.Font.Size = 10
                                    .Cell(pRowIndex, 6).Shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignLeft

                                Next

                            End With

                        ElseIf j1 = 4 Then
                            pText = pShape1.TextFrame.TextRange
                            Dim pstr1 As String = "2-D axisymmetric"
                            Dim pstr2 As String = "hoop"
                            Dim pstr3 As String = "round"

                            If (Project_In.Analysis(Result_Sel_In).Seal.IsSegmented) Then
                                pstr1 = "plane stress"
                                pstr2 = "non-continuous"
                                pstr3 = "segmented"
                            End If

                            pText.Text = "The " & Project_In.Analysis(Result_Sel_In).Seal.Type & " is modeled using temperature dependent, non-linear material properties of (" & Project_In.Analysis(Result_Sel_In).Seal.Mat.Name & ")" & vbLf & _
                                         pstr1 & " behavior is employed to simulate the " & pstr2 & " effects of a " & pstr3 & " part"

                        End If
                    Next
                End If


                'Slide #5
                '-----------
                If i = 4 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            If (File.Exists(pFolderName & "1.png")) Then
                                'pSlide.Shapes.AddPicture(pFolderName & "1.png", False, True, 200, 179, 10 + pShape1.Width, 192 + pShape1.Height)
                                pSlide.Shapes.AddPicture(pFolderName & "1.png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            End If


                        ElseIf j1 = 3 Then
                            pText = pShape1.TextFrame.TextRange
                            'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                            '              " " & modMain.gIPE_Unit.UserL & Space(12) & _
                            '            "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                            '            "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                            '            "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T

                            'AES 03MAR17
                            pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                          " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                        "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                        "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & Space(14) & _
                                        "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T

                        End If
                    Next
                End If

                Dim pAssyImageNo As Integer = 3
                If (pLoadStep(0).Descrip = "PC") Then
                    pAssyImageNo = 5
                End If

                'Slide #6   :   ASSEMBLY CONDITION DISPLACEMENT
                '-----------
                If i = 5 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            'pSlide.Shapes.AddPicture("C:\SealIPE\Templates\file001.png", False, True, 200, 179, 10 + pShape.Width, 192 + pShape.Height)
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            If (File.Exists(pFolderName & pAssyImageNo & ".png")) Then
                                pSlide.Shapes.AddPicture(pFolderName & pAssyImageNo & ".png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            End If

                        ElseIf j1 = 3 Then
                            Dim pLoad As String = "0.0"
                            If (Project_In.Analysis(Result_Sel_In).AppLoad.PreComp.Exists) Then
                                pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(1).ToString("#0.00")
                            Else
                                pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(0).ToString("#0.00")
                            End If

                            pText = pShape1.TextFrame.TextRange
                            'pText.Text = Space(2) & "Free Height = " & gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                            '              " " & gIPE_Unit.UserL & Space(12) & _
                            '            "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                            '            "Cavity Depth = " & gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                            '            "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                            '            Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '" lbf/in"

                            'AES 03MAR17
                            pText.Text = Space(2) & "Free Height = " & gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                          " " & gIPE_Unit.UserL & Space(12) & _
                                        "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                                        "Cavity Depth = " & gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & Space(14) & _
                                        "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                                        Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '" lbf/in"
                        End If

                    Next
                End If

                'Slide #7   :   ASSEMBLY CONDITION STRESS
                '-----------
                If i = 6 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            'pSlide.Shapes.AddPicture("C:\SealIPE\Templates\file001.png", False, True, 200, 179, 10 + pShape.Width, 192 + pShape.Height)
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            If (File.Exists(pFolderName & (pAssyImageNo + 1) & ".png")) Then
                                pSlide.Shapes.AddPicture(pFolderName & (pAssyImageNo + 1) & ".png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            End If

                        ElseIf j1 = 3 Then
                            pText = pShape1.TextFrame.TextRange
                            'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                            '              " " & modMain.gIPE_Unit.UserL & Space(12) & _
                            '            "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                            '            "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                            '            "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T

                            'AES 03MAR17
                            pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                          " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                        "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                                        "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & Space(14) & _
                                        "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T

                        End If
                    Next
                End If

                'Slide #8   :   BASELINE CONDITION STRESS
                '-----------
                If i = 7 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            If (File.Exists(pFolderName & (pAssyImageNo + 3) & ".png")) Then
                                pSlide.Shapes.AddPicture(pFolderName & (pAssyImageNo + 3) & ".png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            End If

                        ElseIf j1 = 2 Then
                            pText = pShape1.TextFrame.TextRange
                            If Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline Then
                                Dim pLoad As String = "0.0"
                                If (Project_In.Analysis(Result_Sel_In).AppLoad.PreComp.Exists) Then
                                    pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(2).ToString("#0.00")
                                Else
                                    pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(1).ToString("#0.00")
                                End If

                                'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                                '         " " & modMain.gIPE_Unit.UserL & Space(12) & _
                                '        "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                '        "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                                '        "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                                '        Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"

                                'AES 03MAR17
                                pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                        " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                       "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                       "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & Space(14) & _
                                       "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                                       Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"
                            Else
                                Dim pLoad As String = "0.0"
                                pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(1).ToString("#0.00")
                                'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                                '         " " & modMain.gIPE_Unit.UserL & Space(12) & _
                                '        "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(0).PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                '        "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(0).CavityDepth) & Space(14) & _
                                '        "Temperature = " & Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(0).TOper & " " & gIPE_Unit.T & vbLf & _
                                '        Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"

                                'AES 07MAR17
                                pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                         " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                        "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(0).PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                        "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pAddLoadCavityDepth(0)) & Space(14) & _
                                        "Temperature = " & Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(0).TOper & " " & gIPE_Unit.T & vbLf & _
                                        Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"
                            End If


                        ElseIf j1 = 4 Then
                            pText = pShape1.TextFrame.TextRange
                            If Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Baseline Then
                                pText.Text = "Baseline Condition"
                            Else
                                pText.Text = "Additional-1 Condition"
                            End If


                        End If
                    Next
                End If

                'Slide #9   :   OPEN CONDITION STRESS
                '-----------
                If i = 8 Then
                    '....Accessing Slides
                    pSlide = pSlides(i + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            Dim pCounter = My.Computer.FileSystem.GetFiles(pFolderName)

                            If (File.Exists(pFolderName & (pCounter.Count - 1) & ".png")) Then
                                pSlide.Shapes.AddPicture(pFolderName & (pCounter.Count - 1) & ".png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            End If

                        ElseIf j1 = 4 Then
                            pText = pShape1.TextFrame.TextRange

                            'AES 14MAR17
                            '--------------
                            Dim pHfree_Final As Single

                            Select Case Project_In.Analysis(Result_Sel_In).Compression.TolType

                                Case "Minimum"
                                    '---------
                                    pHfree_Final = Project_In.Analysis(Result_Sel_In).Result_Gen.HFreeFinal - Project_In.Analysis(Result_Sel_In).Seal.HFreeTol(1)

                                Case "Nominal"
                                    '---------
                                    pHfree_Final = Project_In.Analysis(Result_Sel_In).Result_Gen.HFreeFinal

                                Case "Maximum"
                                    '---------
                                    pHfree_Final = Project_In.Analysis(Result_Sel_In).Result_Gen.HFreeFinal + Project_In.Analysis(Result_Sel_In).Seal.HFreeTol(2)

                            End Select
                            '---------------

                            'pText.Text = "Seal springs back to " & gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Result_Gen.HFreeFinal) & " after all loads are relieved"
                            pText.Text = "Seal springs back to " & gIPE_Unit.WriteInUserL(pHfree_Final) & " after all loads are relieved"

                            'Seal springs back to 0.200 after all loads are relieved

                        End If

                    Next

                End If

            Next

            '....Pre-Compression Condition
            If (pLoadStep(0).Descrip = "PC") Then

                For k As Integer = 1 To 2

                    If (k = 1) Then
                        pSlides(5).Duplicate()

                        '....Accessing Slides
                        pSlide = pSlides(6)
                    Else
                        pSlides(6).Duplicate()

                        '....Accessing Slides
                        pSlide = pSlides(7)
                    End If

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            If (k = 1) Then
                                If (File.Exists(pFolderName & "3.png")) Then
                                    pSlide.Shapes.AddPicture(pFolderName & "3.png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                                End If
                            Else
                                If (File.Exists(pFolderName & "4.png")) Then
                                    pSlide.Shapes.AddPicture(pFolderName & "4.png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                                End If
                            End If


                        ElseIf j1 = 4 Then
                            pText = pShape1.TextFrame.TextRange
                            pText.Text = "Pre-Compressed Condition"

                        ElseIf j1 = 3 Then
                            pText = pShape1.TextFrame.TextRange
                            If (k = 1) Then
                                Dim pLoad As String = "0.0"
                                pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(0).ToString("#0.00")

                                'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                                '         " " & modMain.gIPE_Unit.UserL & Space(12) & _
                                '       "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                                '       "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                                '       "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                                '       Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"

                                'AES 03MAR17
                                pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                        " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                      "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                                      "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & Space(14) & _
                                      "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                                      Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"

                            Else
                                'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                                '             " " & modMain.gIPE_Unit.UserL & Space(12) & _
                                '            "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                '            "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                                '            "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T

                                'AES 03MAR17
                                pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                             " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                            "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).OpCond.PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                            "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pCavityDepth_Actual) & Space(14) & _
                                            "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T
                            End If

                        End If
                    Next
                Next
            End If

            '....Additional Condition
            If (Project_In.Analysis(Result_Sel_In).LoadCase.Type = IPE_clsAnalysis.eLoadType.Additional) Then

                Dim pAdditionalSlideIndex As Integer = 7
                Dim pAdditionalImageIndex As Integer = 6

                For k As Integer = 1 To Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad.Count - 1

                    '    pAdditionalSlideIndex = pAdditionalSlideIndex + k
                    pAdditionalSlideIndex = pAdditionalSlideIndex + 1       'AES 04OCT16
                    pAdditionalImageIndex = pAdditionalImageIndex + 2       'AES 24OCT16

                    pSlides(pAdditionalSlideIndex).Duplicate()

                    '....Accessing Slides
                    pSlide = pSlides(pAdditionalSlideIndex + 1)

                    'Accessing all shapes in slide
                    pShapes = pSlide.Shapes
                    Dim pShape1 As Microsoft.Office.Interop.PowerPoint.Shape
                    For j1 As Integer = 0 To pShapes.Count - 1
                        pShape1 = pShapes(j1 + 1)

                        If j1 = 1 Then
                            'Seal Geometry.
                            '------------------ 
                            Dim pval1 As Single = pShape1.Width
                            Dim pval2 As Single = pShape1.Height
                            Dim pTargetFolderTitle As String = Project_In.Customer_ID & "-" & Project_In.Platform_ID & "-" & Project_In.Project_ID & "-" & Project_In.Analysis(Result_Sel_In).ID
                            Dim pFolderName As String = gIPE_File.DirOut & pTargetFolderTitle & "\"

                            'If (File.Exists(pFolderName & (pAdditionalImageIndex + 2) & ".png")) Then
                            '    pSlide.Shapes.AddPicture(pFolderName & (pAdditionalImageIndex + 2) & ".png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            'End If

                            'AES 24OCT16
                            If (File.Exists(pFolderName & (pAdditionalImageIndex) & ".png")) Then
                                pSlide.Shapes.AddPicture(pFolderName & (pAdditionalImageIndex) & ".png", False, True, 170, 142, 35 + pShape1.Width, 280 + pShape1.Height)
                            End If

                        ElseIf j1 = 4 Then
                            pText = pShape1.TextFrame.TextRange
                            pText.Text = "Additional-" & (k + 1) & " Condition"

                        ElseIf j1 = 2 Then
                            pText = pShape1.TextFrame.TextRange
                            Dim pLoad As String = "0.0"
                            pLoad = Project_In.Analysis(Result_Sel_In).Seal.FSeat_Unit(k + 1).ToString("#0.00")

                            'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                            '         " " & modMain.gIPE_Unit.UserL & Space(12) & _
                            '       "Pressure = 0 " & gIPE_Unit.UserP & vbLf & _
                            '       "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Cavity.Depth) & Space(14) & _
                            '       "Temperature = " & Project_In.Analysis(Result_Sel_In).OpCond.TOper & " " & gIPE_Unit.T & vbLf & _
                            '       Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"


                            'pText.Text = Space(2) & "Free Height = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).Seal.Hfree) & _
                            '         " " & modMain.gIPE_Unit.UserL & Space(12) & _
                            '        "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                            '        "Cavity Depth = " & modMain.gIPE_Unit.WriteInUserL(Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).CavityDepth) & Space(14) & _
                            '        "Temperature = " & Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).TOper & " " & gIPE_Unit.T & vbLf & _
                            '        Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"

                            'AES 07MAR17
                            pText.Text = Space(2) & "Free Height = " & modMain_IPE.gIPE_Unit.WriteInUserL(pHfree_Actual) & _
                                    " " & modMain_IPE.gIPE_Unit.UserL & Space(12) & _
                                   "Pressure = " & gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).PDiff) & " " & gIPE_Unit.UserP & vbLf & _
                                   "Cavity Depth = " & modMain_IPE.gIPE_Unit.WriteInUserL(pAddLoadCavityDepth(k)) & Space(14) & _
                                   "Temperature = " & Project_In.Analysis(Result_Sel_In).AppLoad.AddLoad(k).TOper & " " & gIPE_Unit.T & vbLf & _
                                   Space(12) & "Load  = " & (pLoad * gIPE_Unit.CFacUserL).ToString("#0.00") & " " & gIPE_Unit.F & "/" & gIPE_Unit.UserL '& pLoad & " lbf/in"

                        End If
                    Next
                Next

            End If

            Dim pSlideNoCur As Integer = 6
            Dim pAssySlideNo As Integer = 6
            Dim pStartIndex As Integer = 0
            If (pLoadStep(0).Descrip = "PC") Then
                pAssySlideNo = 8
                pStartIndex = 1
            End If

            Dim pDeletedSlide_Count As Integer = 0

            '....Delete slide
            For k As Integer = 0 To LoadStep_In.Count - 1
                If (k <> 0 And k <> 1 And pLoadStep(k).Descrip = "Assembly") Then
                    Continue For
                End If

                If (LoadStep_In(k) = False) Then
                    If (pLoadStep(0).Descrip = "PC" And k = 0) Then
                        pSlides(pSlideNoCur).Delete()
                        pSlides(pSlideNoCur).Delete()
                        pDeletedSlide_Count = pDeletedSlide_Count + 2

                    Else
                        If (k = pStartIndex) Then
                            pSlides(pAssySlideNo - pDeletedSlide_Count).Delete()
                            pSlides(pAssySlideNo - pDeletedSlide_Count).Delete()
                            pDeletedSlide_Count = pDeletedSlide_Count + 2

                        Else
                            Dim pSlideNo As Integer = pSlideNoCur
                            If (pDeletedSlide_Count > 0) Then
                                pSlideNo = pSlideNo - pDeletedSlide_Count
                            End If
                            pSlides(pSlideNo).Delete()
                            pDeletedSlide_Count = pDeletedSlide_Count + 1
                        End If
                    End If

                End If

                pSlideNoCur = pSlideNoCur + 1

                If (k = pStartIndex Or pLoadStep(k).Descrip = "PC") Then
                    pSlideNoCur = pSlideNoCur + 1
                End If

            Next


            If (ThermalGrowth_In = False) Then
                Dim pSlideNo As Integer = pSlideNoCur
                If (pDeletedSlide_Count > 0) Then
                    pSlideNo = pSlideNo - pDeletedSlide_Count
                End If
                pSlides(pSlideNo).Delete()
                pDeletedSlide_Count = pDeletedSlide_Count + 1
            End If

            pApp.Visible = MsoTriState.msoTrue

            Dim pSSWs As Microsoft.Office.Interop.PowerPoint.SlideShowWindows
            Dim pSSS As Microsoft.Office.Interop.PowerPoint.SlideShowSettings
            'Run the Slide show
            pSSS = pCurPresentation.SlideShowSettings
            pSSS.Run()
            pSSWs = pApp.SlideShowWindows
            While pSSWs.Count >= 1
                System.Threading.Thread.Sleep(100)
            End While

        Catch ex As Exception

        End Try

    End Sub


    Public Sub OpenPowerPoint(ByVal FileName_In As String)
        '===================================================        'AES 08NOV16
        Dim pApp As New Microsoft.Office.Interop.PowerPoint.Application()
        Dim pPresentations As Microsoft.Office.Interop.PowerPoint.Presentations = pApp.Presentations

        Dim pCurPresentation As Microsoft.Office.Interop.PowerPoint.Presentation = pPresentations.Open(FileName_In, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue)

        Dim pSSWs As Microsoft.Office.Interop.PowerPoint.SlideShowWindows
        Dim pSSS As Microsoft.Office.Interop.PowerPoint.SlideShowSettings

        '....Run the Slide show
        pSSS = pCurPresentation.SlideShowSettings
        pSSS.Run()
        pSSWs = pApp.SlideShowWindows
        While pSSWs.Count >= 1
            System.Threading.Thread.Sleep(100)
        End While

    End Sub



    Private Sub NewPage(ByVal wordDoc_In As WORD.Document, ByVal LineNo As Integer)
        '==========================================================================
        '...Create new page with Project Information            Not Used NOW

        With wordDoc_In.Range
            .Paragraphs.Item(LineNo).Range.InsertAfter(vbCrLf) 'LineNo=41

            '....Date, UserInformation & Project Name
            .Paragraphs.Item(LineNo + 1).Range.Paste() 'LineNo=42
        End With

    End Sub


    Public Sub WriteSummaryTable(ByVal Project_In As IPE_clsProject, ByVal Analysis_Cur_In As Integer,
                                 ByVal Unit_In As IPE_clsUnit, ByRef ANSYS_Out As IPE_clsANSYS)
        '========================================================================================
        '...This routine writes the Summary Table - Results & Analysis.

        '....AES 17MAR16
        'Const pcNColumns As Integer = 14        '....# of Table Columns
        Const pcNColumns As Integer = 10        '....# of Table Columns


        Dim pWordApp As New WORD.Application()
        Dim pWordDoc As WORD.Document = Nothing

        Try
            pWordDoc = pWordApp.Documents.Add(mcSummaryTableDotFileName)
            pWordApp.Visible = True

            Dim iCell, iRow As Integer
            Dim pCaptionArray(pcNColumns) As String
            Dim pCaptionUnitArray(pcNColumns) As String

            Dim pobjTable As WORD.Table

            Dim pSealEntities As New SealIPEDBEntities

            Dim pRecAnalysisCount As Integer = (From pRec In pSealEntities.tblAnalysis
                                                    Where pRec.fldProjectID = Project_In.Project_ID Select pRec).Count()

            '...Store the caption array elements
            pCaptionArray(1) = "Seal Design"
            pCaptionArray(2) = "Seal Adj."
            pCaptionArray(3) = "Material"
            pCaptionArray(4) = "Units"
            pCaptionArray(5) = "Free Height"
            pCaptionArray(6) = "Pressure"
            pCaptionArray(7) = "Press Units"
            pCaptionArray(8) = "POrient"
            pCaptionArray(9) = "Temp"
            pCaptionArray(10) = "Spring Back"


            '...Store the caption unit array elements
            pCaptionUnitArray(1) = ""
            pCaptionUnitArray(2) = ""
            pCaptionUnitArray(3) = ""
            pCaptionUnitArray(4) = ""
            pCaptionUnitArray(5) = "(in/mm)"
            pCaptionUnitArray(6) = ""
            pCaptionUnitArray(7) = ""
            pCaptionUnitArray(8) = ""
            pCaptionUnitArray(9) = "(" & Chr(186) & "F/" & Chr(186) & "C" & ")"
            pCaptionUnitArray(10) = "(%)"


            '..Check the no. of Records.
            '
            '...No. of Rows in Table
            Dim nRows As Integer
            nRows = pRecAnalysisCount + 2       '....1st row for Columns Heading
            '                                   '......2nd for ColumnUnits Heading,Others for Records

            With pWordDoc
                '....Date   
                With pWordDoc.Bookmarks.Item("Date").Range
                    .Text = Today.ToString("MMMM dd, yyyy", mCI.DateTimeFormat()) 'US Format only 
                    .Font.Size = 10
                    .ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphRight
                End With

                '....Design Type        
                If Project_In.Analysis(Analysis_Cur_In).Seal Is Nothing = False Then
                    With pWordDoc.Bookmarks.Item("DesignType").Range
                        .Text = Project_In.Analysis(Analysis_Cur_In).Seal.Type
                        '.Font.Underline = Word.WdUnderline.wdUnderlineSingle
                    End With
                End If

                '....Table to show Results Summary
                pobjTable = .Bookmarks.Item("Table").Range.Tables. _
                           Add(.Bookmarks.Item("Table").Range, nRows, pcNColumns)

            End With


            With pobjTable
                .Shading.BackgroundPatternColor = WORD.WdColor.wdColorGray10
                '.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble
                .Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter

                With .Range.Font
                    .Underline = WORD.WdUnderline.wdUnderlineNone
                    .Bold = True
                    .Size = 8
                End With

                .AllowAutoFit = True

                iRow = 1
                '...Column Captions                    '....1st Row is for Captions
                For iCell = 1 To pcNColumns
                    .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionArray(iCell))
                    '.Rows.Item(iRow).Cells.Item(iCell).Range.ParagraphFormat.Alignment = _
                    '                    Word.WdParagraphAlignment.wdAlignParagraphCenter

                    '
                    .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                    '.Rows.Item(iRow).Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble

                Next iCell

                iRow = iRow + 1
                iCell = 1

                '...Column Unit Captions                '....2nd Row is for Unit Captions
                For iCell = 1 To pcNColumns
                    .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pCaptionUnitArray(iCell))
                    '.Rows.Item(iRow).Cells.Item(iCell).Range.ParagraphFormat.Alignment = _
                    '                    Word.WdParagraphAlignment.wdAlignParagraphCenter


                    .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                    '.Rows.Item(iRow).Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble

                Next iCell


                iRow = iRow + 1
                '...Insert records
                Dim pStrList As New List(Of String)

                Dim pQryAnalysis = (From pRec In pSealEntities.tblAnalysis
                                        Where pRec.fldProjectID = Project_In.Project_ID Select pRec)


                Dim pRecord As tblAnalysis
                For Each pRecord In pQryAnalysis
                    pStrList.Clear()
                    Dim pISel As Integer = Analysis_Cur_In
                    Project_In.Analysis(pISel).ID = pRecord.fldID

                    Project_In.Analysis(pISel).Retrieve_FromDB(Unit_In, ANSYS_Out)

                    If (Project_In.Analysis(pISel).State = IPE_clsAnalysis.eState.Complete) Then

                        pStrList.Add(Project_In.Analysis(pISel).Seal.MCrossSecNo)
                        pStrList.Add(Project_In.Analysis(pISel).Seal.Adjusted)
                        pStrList.Add(Project_In.Analysis(pISel).Seal.Mat.Name)
                        pStrList.Add(Project_In.UnitSystem())
                        pStrList.Add(gIPE_Unit.WriteInUserL(Project_In.Analysis(pISel).Seal.Hfree))
                        pStrList.Add(gIPE_Unit.FormatPDiffUnitUser(Project_In.Analysis(pISel).OpCond.PDiff))
                        pStrList.Add(Unit_In.UserP)
                        pStrList.Add(Project_In.Analysis(pISel).OpCond.POrient)
                        pStrList.Add(Project_In.Analysis(pISel).OpCond.TOper)

                        Dim pSpringBack As Integer = 0
                        pSpringBack = Project_In.Analysis(pISel).Seal.SpringBack(Project_In.Analysis(pISel).Compression.TolType, Project_In.Analysis(pISel).Cavity.DepthActual(Project_In.Analysis(pISel).Compression.TolType), Project_In.Analysis(pISel).Compression.Val)
                        pStrList.Add(pSpringBack)


                        For iCell = 1 To pcNColumns
                            Dim pStr As String

                            Dim pCI As New CultureInfo(Thread.CurrentThread.CurrentCulture.Name)
                            If iCell = 12 Or iCell = 13 Then
                                pStr = Format(pStrList(iCell - 1), "#0,000")
                            Else
                                pStr = pStrList(iCell - 1)
                            End If

                            .Rows.Item(iRow).Cells.Item(iCell).Range.InsertAfter(pStr)
                            .Rows.Item(iRow).Cells.Item(iCell).Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                            ''.Tables.Item(iCell).Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle
                        Next iCell

                        .Rows.Item(iRow).Borders.InsideLineStyle = WORD.WdLineStyle.wdLineStyleSingle
                        '.Rows.Item(iRow).Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle

                        .Rows.Item(iRow).Range.Font.Bold = False
                        .Rows.Item(iRow).Range.ParagraphFormat.Alignment = _
                                            WORD.WdParagraphAlignment.wdAlignParagraphCenter

                        .Rows.Item(iRow).Range.Shading.BackgroundPatternColor = WORD.WdColor.wdColorGray05
                        iRow = iRow + 1
                    End If
                Next

                .Borders.OutsideLineStyle = WORD.WdLineStyle.wdLineStyleDouble

            End With


        Catch pEXP As Exception
            Dim pintAttributes As Integer = MsgBoxStyle.Critical + MsgBoxStyle.OkOnly
            Dim pstrTitle As String = "Write Summary Table Error"
            MsgBox(pEXP.Message, pintAttributes, pstrTitle)

        Finally
            pWordApp = Nothing
            pWordDoc = Nothing

        End Try

    End Sub


    Public Sub CreateAdjGeomDoc(ByRef Pic_In As PictureBox, _
                                ByVal Seal_In As IPE_clsSeal, _
                                ByVal Unit_In As IPE_clsUnit, _
                                ByVal UserInfo_In As IPE_clsUser, _
                                ByVal Project_In As IPE_clsProject)
        '===============================================================================  
        '....This Routine Creates Adjust Geometry Report corresponding to the current 
        '........case.

        Dim pWordApp As WORD.Application
        Dim pWordDoc As WORD.Document = Nothing

        Try
            pWordApp = New WORD.Application()
            pWordDoc = pWordApp.Documents.Add(mcAdjGeomDotFileName)

            pWordApp.Visible = True

            'Report Body
            '===========
            With pWordDoc
                '.Bookmarks.Item("Date").Range.Text = Format(Today, "MMMM dd, yyyy")
                .Bookmarks.Item("SealType").Range.Text = ExtractPreData(Seal_In.Type, "-")

                ''.Bookmarks.Item("Project").Range.Text = Project_In.Name
                .Bookmarks.Item("Date").Range.Text = _
                    Today.ToString("MMMM dd, yyyy", mCI.DateTimeFormat()) 'US Format only   

                .Bookmarks.Item("ProducedBy").Range.Text = UserInfo_In.Name
                .Bookmarks.Item("PhoneNo").Range.Text = UserInfo_In.PhoneNo

                Pic_In.Select()
                Clipboard.SetDataObject(Pic_In.Image)

                'Clipboard.GetDataObject()

                With .Bookmarks.Item("Drawing").Range
                    '.InlineShapes.AddPicture(bmpImgFile)
                    .Paste()
                    .InsertAfter(vbCrLf)
                End With

                'With .Paragraphs(7).Range.Characters(1)

                '       .InsertAfter "Free Height - Standard Geometry: " & _
                'unit.WriteInUserL(eSeal.HfreeStd) & _
                '" " & unit.UserL & vbCrLf

                .Bookmarks.Item("HFree").Range.Text = Unit_In.WriteInUserL((Seal_In.HfreeStd)) & _
                                                        " " & Unit_In.UserL

                Dim psngDelHfreePCent As Single
                psngDelHfreePCent = (Seal_In.Hfree - Seal_In.HfreeStd) * 100 / Seal_In.HfreeStd


                Dim pERROR_ROUNDING_Max_Eng As Single = 0.0005
                Dim pERROR_ROUNDING_Max_Met As Single = 0.005

                Dim pERROR_ROUNDING_Max As Single
                If Unit_In.System = "English" Then
                    pERROR_ROUNDING_Max = pERROR_ROUNDING_Max_Eng

                ElseIf Unit_In.System = "Metric" Then
                    pERROR_ROUNDING_Max = pERROR_ROUNDING_Max_Met
                End If

                Dim pERROR_DelHfreePCent_Max As Single
                Dim pHfreeStd_UserL As Single = gIPE_Unit.L_ConToUser(Seal_In.HfreeStd)
                pERROR_DelHfreePCent_Max = pERROR_ROUNDING_Max * 100 / pHfreeStd_UserL


                If Abs(psngDelHfreePCent) > pERROR_DelHfreePCent_Max Then  ' 0.1# Then
                    .Bookmarks.Item("AdjGeom").Range.Text = "-  Adjusted Geometry :" & Space(2) & _
                                                           Unit_In.WriteInUserL((Seal_In.Hfree)) & _
                                                           " " & Unit_In.UserL & _
                                                           "  ( " & Format(psngDelHfreePCent, "##0.0") & " %)"
                End If

            End With

            pWordApp.ChangeFileOpenDirectory(gIPE_File.DirOut)
            'myWordApp.Quit(Word.WdSaveOptions.wdPromptToSaveChanges, Word.WdOriginalFormat.wdWordDocument)

        Catch pEXP As Exception
            MsgBox(pEXP.Message)

        Finally
            pWordApp = Nothing
            pWordDoc = Nothing
        End Try

    End Sub


#End Region


End Class