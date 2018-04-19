'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessFile                         '
'                        VERSION NO  :  1.5                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  19APR18                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Math
Imports System.Globalization
Imports clsLibrary11
Imports System.Xml
Imports System.Configuration
Imports System.Data.SqlClient
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.Text
Imports System.Data.EntityClient
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class clsProcessFile

    'List of Files Processed:
    '--------------------------------
    '1. PD_DropDownLists
    '2. PD_RiskQs
    '3. PD_Notes
    '4. PD_PDS_Mapping

#Region "NAMED CONSTANT:"

    'Directories & Folders:
    '----------------------
    '....Root Directory.
    Private Const mcDriveRoot As String = "C:"

    Private Const mcDirRoot As String = mcDriveRoot & "\SealSuite\SealProcess\"
    Private Const mcPDSMappingFile As String = mcDirRoot & "Program Data Files\PD_PDSRevW_Mapping_27FEB18.xlsx"

    'PDS Template File:
    '------------------------
    Private Const mcDirTemplates As String = mcDirRoot & "Templates\"

    'PDS Output File:
    '----------------
    Private Const mcDirOutput As String = mcDirRoot & "Output Files\"

    '....PDS Report  
    Private Const mcPDSReportFileName As String = mcDirTemplates & "EN7300007 - Product Definition Sheet_Rev W_Rev03.xltx"

    Private mPDS_FieldName As New List(Of String)
    Private mPDS_CellColName As New List(Of String)
    Private mPDS_Val As New List(Of String)
    Private mDesignInput As String
    Private mCustSpecType As String
    Private mCustSpecInterpretation As String
    Private mSealDimName As String
    Private mSealDimMin_in As String
    Private mSealDimMax_in As String
    Private mSealDimMin_mm As String
    Private mSealDimMax_mm As String
    Private mTool_Gage_PartNo As String
    Private mTool_Gage_Desc As String
    Private mTool_Gage_Response As String
    Private mDwg_Needed_DwgNo As String
    Private mDwg_Needed_DwgDesc As String
    Private mBOM_Parent_PartNo As String
    Private mBOM_Child_PartNo As String
    Private mBOM_Qty As String

    Private mAttendeesStartColName_Dept As String
    Private mAttendeesStartColName_Sign As String
    Private mAttendeesStartColName_Name As String
    Private mAttendeesStartColName_Title As String
    Private mAttendeesStartColName_Date As String

    Private mIssueCommentStartColName_Issue As String
    Private mIssueCommentStartColName_By As String
    Private mIssueCommentStartColName_Date As String
    Private mIssueCommentStartColName_Resolution As String

#End Region

#Region "CONSTRUCTOR:"

    Public Sub New()
        '===========

    End Sub

#End Region

#Region "PROPERTY ROUTINES:"

    
#End Region

#Region "PD - PDS:"

    Public Sub Handle_PDSFile(ByVal Mode_In As String, ByVal ProcessProj_In As clsProcessProj, _
                              ByVal PartProj_In As clsPartProject)
        '=======================================================================================
        If (Mode_In = "Write") Then
            ReadPDSMapping()
            PopulatePDS_Val(ProcessProj_In, PartProj_In)
            WritePDSFile(ProcessProj_In, PartProj_In)

        ElseIf (Mode_In = "Read") Then
            'ReadPDSMapping_PDS_DS()
            'ReadPDSFile()/'PopulateDS_Val()

        End If

    End Sub


    Private Sub ReadPDSMapping()
        '=======================
        CloseExcelFiles()

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing

        Dim pSealTestDBEntities As New SealTestDBEntities

        Try
            pWkbOrg = pApp.Workbooks.Open(mcPDSMappingFile, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value, _
                                          Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, _
                                          Missing.Value, Missing.Value, Missing.Value)

            Dim pWkSheet As EXCEL.Worksheet

            pWkSheet = pWkbOrg.Worksheets("Mapping")

            Dim pExcelCellRange As EXCEL.Range = pWkSheet.UsedRange

            For i As Integer = 4 To pExcelCellRange.Rows.Count

                Dim pFieldName As String = pExcelCellRange.Cells(i, 4).Value
                mPDS_FieldName.Add(pFieldName)

                Dim pCellColName As String = ""
                If (Not IsNothing(pExcelCellRange.Cells(i, 5).Value)) Then
                    pCellColName = pExcelCellRange.Cells(i, 5).Value
                End If

                Dim pHeaderIndx As Integer = 0 'pExcelCellRange.Cells(i, 6).Value
                If (Not IsNothing(pExcelCellRange.Cells(i, 6).Value)) Then
                    pHeaderIndx = pExcelCellRange.Cells(i, 6).Value
                End If
                Dim pRelativeIndx As Integer = 0 'pExcelCellRange.Cells(i, 7).Value
                If (Not IsNothing(pExcelCellRange.Cells(i, 7).Value)) Then
                    pRelativeIndx = pExcelCellRange.Cells(i, 7).Value
                End If

                Dim pIndex_Tot As String = ""
                If (pHeaderIndx > 0) Then
                    pIndex_Tot = pHeaderIndx + pRelativeIndx
                End If

                Dim pCellName As String = pCellColName.Trim() & pIndex_Tot
                mPDS_CellColName.Add(pCellName)
            Next

        Catch ex As Exception

        Finally

            pApp.Visible = False

        End Try

    End Sub


    Private Sub PopulatePDS_Val(ByVal ProcessProj_In As clsProcessProj, ByVal PartProj_In As clsPartProject)
        '===================================================================================================

        Dim pCI As New CultureInfo("en-US")

        Try


            For i As Integer = 0 To mPDS_FieldName.Count - 1
                mPDS_Val.Add("")

                Select Case mPDS_FieldName(i)
                    Case "Parker Part #"
                        mPDS_Val(i) = PartProj_In.PNR.PN()

                    Case "Parker Rev"
                        mPDS_Val(i) = PartProj_In.PNR.PN_Rev()

                    Case "Customer Name"
                        mPDS_Val(i) = PartProj_In.CustInfo.CustName

                    Case "Customer Location"
                        mPDS_Val(i) = PartProj_In.CustInfo.LocName

                    Case "Customer Part #"
                        mPDS_Val(i) = PartProj_In.CustInfo.PN_Cust

                    Case "Customer Rev"
                        mPDS_Val(i) = PartProj_In.CustInfo.PN_Cust_Rev

                    Case "Customer POP Coding"
                        mPDS_Val(i) = ProcessProj_In.POPCoding

                    Case "Export Controlled"
                        mPDS_Val(i) = IIf(ProcessProj_In.ITAR_Export.IsUnder_ITAR_Reg = True, "Yes, Non Military", "No")

                    Case "Last Modified Date"
                        mPDS_Val(i) = ProcessProj_In.DateLastModified.ToString("MM/dd/yyyy", pCI.DateTimeFormat())

                    Case "Quote No"
                        If (ProcessProj_In.PreOrder.Quote.QID.Count > 0) Then
                            mPDS_Val(i) = ProcessProj_In.PreOrder.Quote.No(0)
                        End If

                    Case "Quote Date"
                        ''If (ProcessProj_In.PreOrder.Quote.QID.Count > 0) Then
                        ''    mPDS_Val(i) = ProcessProj_In.PreOrder.Quote.QDate(0).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                        ''End If

                    Case "Winnovation No"
                        If (ProcessProj_In.Design.IsWinnovation) Then
                            mPDS_Val(i) = ProcessProj_In.Design.WinnovationNo
                        Else
                            mPDS_Val(i) = "No"
                        End If


                    Case "Parker Manager Marketing"
                        mPDS_Val(i) = ProcessProj_In.PreOrder.Mgr.Mkt

                    Case "Sales"
                        mPDS_Val(i) = ProcessProj_In.PreOrder.Mgr.Sales

                    Case "Customer Contact Engineering Name"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Engineering") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Name(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Engineering Phone"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Engineering") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Phone(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Engineering Email"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Engineering") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Email(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Purchasing Name"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Purchasing") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Name(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Purchasing Phone"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Purchasing") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Phone(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Purchasing Email"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Purchasing") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Email(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Distributor Name"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Distributor") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Name(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Distributor Phone"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Distributor") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Phone(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Distributor Email"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Distributor") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Email(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Quality Name"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Quality") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Name(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Quality Phone"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Quality") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Phone(j)
                                Exit For
                            End If
                        Next

                    Case "Customer Contact Quality Email"
                        For j As Integer = 0 To ProcessProj_In.CustContact.DeptName.Count - 1
                            If (ProcessProj_In.CustContact.DeptName(j) = "Quality") Then
                                mPDS_Val(i) = ProcessProj_In.CustContact.Email(j)
                                Exit For
                            End If
                        Next

                    Case "Market Segment"
                        mPDS_Val(i) = ProcessProj_In.PreOrder.Mkt.Seg

                    Case "PreOrder Notes"
                        mPDS_Val(i) = ProcessProj_In.PreOrder.Notes

                    Case "Is the product under ITAR regulations?"
                        mPDS_Val(i) = IIf(ProcessProj_In.ITAR_Export.IsUnder_ITAR_Reg = True, "Yes", "No")

                    Case "Does sale of this part require and export license?"
                        mPDS_Val(i) = IIf(ProcessProj_In.ITAR_Export.SaleExportControlled = True, "Yes", "No")

                    Case "Sales Order No"
                        mPDS_Val(i) = ProcessProj_In.OrdEntry.SalesOrderNo

                    Case "PO #"
                        mPDS_Val(i) = ProcessProj_In.OrdEntry.PONo

                    Case "PO Date Received"
                        mPDS_Val(i) = ProcessProj_In.OrdEntry.DatePO.ToString("MM/dd/yyyy", pCI.DateTimeFormat())

                    Case "Special PO Requirements?"
                        mPDS_Val(i) = IIf(ProcessProj_In.OrdEntry.HasSplReq = True, "Yes", "No")

                    Case "Tooling on PO?"
                        mPDS_Val(i) = IIf(ProcessProj_In.OrdEntry.Tool_Reqd = True, "Yes", "No")

                    Case "Special Packaging & Labeling"
                        mPDS_Val(i) = IIf(ProcessProj_In.OrdEntry.SplPkg_Lbl_Reqd = True, "Yes", "No")

                    Case "Order Qty"
                        If (ProcessProj_In.OrdEntry.OrdQty > 0) Then
                            mPDS_Val(i) = ProcessProj_In.OrdEntry.OrdQty
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Required Ship Date"
                        mPDS_Val(i) = ProcessProj_In.OrdEntry.DateOrdShip.ToString("MM/dd/yyyy", pCI.DateTimeFormat())

                    Case "Expedited?"
                        mPDS_Val(i) = IIf(ProcessProj_In.OrdEntry.Expedited = True, "Yes", "No")

                    Case "DFAR Clauses?"
                        mPDS_Val(i) = IIf(ProcessProj_In.OrdEntry.IsDFAR = True, "Yes", "No")

                    Case "Is Pressure Cyclic?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.IsPressCyclic = True, "Yes", "No")

                    Case "Frequency"
                        If (ProcessProj_In.App.PressCycle_Freq > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.PressCycle_Freq
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Amplitude"
                        If (ProcessProj_In.App.PressCycle_Amp > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.PressCycle_Amp
                        Else
                            mPDS_Val(i) = ""
                        End If


                    Case "Fluid Medium"
                        mPDS_Val(i) = ProcessProj_In.App.Fluid

                    Case "Max Leakage Rate"
                        If (ProcessProj_In.App.MaxLeak > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.MaxLeak
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Shaped?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.Shaped = True, "Yes", "No")

                    Case "Out of Round?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.IsOoR = True, "Yes", "No")

                    Case "Split Ring?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.IsSplitRing = True, "Yes", "No")

                    Case "Precompressed?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.IsPreComp = True, "Yes", "No")

                    Case "Operating Conditions Temp Assy (F)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Assy > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Assy
                            Else
                                mPDS_Val(i) = ""
                            End If

                        Else
                            If (ProcessProj_In.App.OpCond.T.Assy > gcEPS) Then
                                Dim pVal As Double = ProcessProj_In.App.OpCond.T.Assy
                                mPDS_Val(i) = ConvCToF(pVal)
                            Else
                                mPDS_Val(i) = ""
                            End If

                        End If


                    Case "Operating Conditions Temp Min (F)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Min > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Min
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Min > gcEPS) Then
                                Dim pVal As Double = ProcessProj_In.App.OpCond.T.Min
                                mPDS_Val(i) = ConvCToF(pVal)
                            Else
                                mPDS_Val(i) = ""
                            End If

                        End If



                    Case "Operating Conditions Temp Max (F)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Max > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Max
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Max > gcEPS) Then
                                Dim pVal As Double = ProcessProj_In.App.OpCond.T.Max
                                mPDS_Val(i) = ConvCToF(pVal)
                            Else
                                mPDS_Val(i) = ""
                            End If
                        End If


                    Case "Operating Conditions Temp Operating (F)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Oper > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Oper
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Oper > gcEPS) Then
                                Dim pVal As Double = ProcessProj_In.App.OpCond.T.Oper
                                mPDS_Val(i) = ConvCToF(pVal)

                            Else
                                mPDS_Val(i) = ""
                            End If
                        End If

                    '-------------------------------
                    Case "Operating Conditions Temp Assy (C)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Assy > gcEPS) Then
                                mPDS_Val(i) = ConvFtoC(ProcessProj_In.App.OpCond.T.Assy)
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Assy > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Assy
                            Else
                                mPDS_Val(i) = ""
                            End If
                        End If


                    Case "Operating Conditions Temp Min (C)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Min > gcEPS) Then
                                mPDS_Val(i) = ConvFtoC(ProcessProj_In.App.OpCond.T.Min)
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Min > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Min
                            Else
                                mPDS_Val(i) = ""
                            End If
                        End If



                    Case "Operating Conditions Temp Max (C)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Max > gcEPS) Then
                                mPDS_Val(i) = ConvFtoC(ProcessProj_In.App.OpCond.T.Max)
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Max > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Max
                            Else
                                mPDS_Val(i) = ""
                            End If
                        End If


                    Case "Operating Conditions Temp Operating (C)"
                        If (ProcessProj_In.Unit.TUnit_Cust = "ºF") Then
                            If (ProcessProj_In.App.OpCond.T.Oper > gcEPS) Then
                                mPDS_Val(i) = ConvFtoC(ProcessProj_In.App.OpCond.T.Oper)
                            Else
                                mPDS_Val(i) = ""
                            End If
                        Else
                            If (ProcessProj_In.App.OpCond.T.Oper > gcEPS) Then
                                mPDS_Val(i) = ProcessProj_In.App.OpCond.T.Oper
                            Else
                                mPDS_Val(i) = ""
                            End If
                        End If


                    '-----------------------
                    Case "Operating Conditions Pressure Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.PUnit_Cust

                    Case "Operating Conditions Pressure Assy (PSI)"
                        If (ProcessProj_In.App.OpCond.Press.Assy > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.OpCond.Press.Assy
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Operating Conditions Pressure Min (PSI)"
                        If (ProcessProj_In.App.OpCond.Press.Min > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.OpCond.Press.Min
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Operating Conditions Pressure Max (PSI)"
                        If (ProcessProj_In.App.OpCond.Press.Max > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.OpCond.Press.Max
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Operating Conditions Pressure Operating (PSI)"
                        If (ProcessProj_In.App.OpCond.Press.Oper > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.OpCond.Press.Oper
                        Else
                            mPDS_Val(i) = ""
                        End If

                    '-----------
                    ''Case "Operating Conditions Pressure Assy (Bar)"
                    ''    If (ProcessProj_In.App.OpCond.Press.Assy > gcEPS) Then
                    ''        mPDS_Val(i) = ConvPSIToBar(ProcessProj_In.App.OpCond.Press.Assy)
                    ''    Else
                    ''        mPDS_Val(i) = ""
                    ''    End If

                    ''Case "Operating Conditions Pressure Min (Bar)"
                    ''    If (ProcessProj_In.App.OpCond.Press.Min > gcEPS) Then
                    ''        mPDS_Val(i) = ConvPSIToBar(ProcessProj_In.App.OpCond.Press.Min)
                    ''    Else
                    ''        mPDS_Val(i) = ""
                    ''    End If

                    ''Case "Operating Conditions Pressure Max (Bar)"
                    ''    If (ProcessProj_In.App.OpCond.Press.Max > gcEPS) Then
                    ''        mPDS_Val(i) = ConvPSIToBar(ProcessProj_In.App.OpCond.Press.Max)
                    ''    Else
                    ''        mPDS_Val(i) = ""
                    ''    End If

                    ''Case "Operating Conditions Pressure Operating (Bar)"
                    ''    If (ProcessProj_In.App.OpCond.Press.Oper > gcEPS) Then
                    ''        mPDS_Val(i) = ConvPSIToBar(ProcessProj_In.App.OpCond.Press.Oper)
                    ''    Else
                    ''        mPDS_Val(i) = ""
                    ''    End If
                    '----------

                    Case "Pressure Direction"
                        mPDS_Val(i) = ProcessProj_In.App.Face.POrient

                    Case "Max Flange Separation"
                        mPDS_Val(i) = ProcessProj_In.App.Face.MaxFlangeSep

                    Case "CavityDepthAssyMin (in)"

                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityDepthAssyMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityDepthOpMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityDepthOpMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '--------
                    Case "CavityAxialLenAssyMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityAxialLenAssyMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityAxialLenOpMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityAxialLenOpMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    '--------
                    Case "CavityRadialWidthAssyMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityRadialWidthAssyMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityRadialWidthOpMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityRadialWidthOpMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '------

                    Case "CavityIDAssyMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityIDAssyMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityIDOpMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityIDOpMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '------

                    Case "CavityODAssyMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityODAssyMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityODOpMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityODOpMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    '----
                    Case "CavityCornerRadAssyMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityCornerRadAssyMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityCornerRadOpMin (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityCornerRadOpMax (in)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    Else
                                        mPDS_Val(i) = ConvMMToIn(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    '....mm
                    Case "CavityDepthAssyMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityDepthAssyMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityDepthOpMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityDepthOpMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Depth") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '--------
                    Case "CavityAxialLenAssyMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityAxialLenAssyMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityAxialLenOpMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityAxialLenOpMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Axial L") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    '--------
                    Case "CavityRadialWidthAssyMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityRadialWidthAssyMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityRadialWidthOpMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityRadialWidthOpMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Radial Wid") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '------

                    Case "CavityIDAssyMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityIDAssyMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityIDOpMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityIDOpMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "ID") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '------

                    Case "CavityODAssyMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityODAssyMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityODOpMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityODOpMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "OD") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    '----
                    Case "CavityCornerRadAssyMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityCornerRadAssyMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Assy(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Assy(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Assy(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityCornerRadOpMin (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Min > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Min)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Min
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next

                    Case "CavityCornerRadOpMax (mm)"
                        For j As Integer = 0 To ProcessProj_In.App.Cavity.DimName.Count - 1
                            If (ProcessProj_In.App.Cavity.DimName(j).Trim() = "Corner Radius") Then
                                If (ProcessProj_In.App.Cavity.Oper(j).Max > gcEPS) Then
                                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                                        mPDS_Val(i) = ConvInToMM(ProcessProj_In.App.Cavity.Oper(j).Max)
                                    Else
                                        mPDS_Val(i) = ProcessProj_In.App.Cavity.Oper(j).Max
                                    End If

                                Else
                                    mPDS_Val(i) = ""
                                End If
                                Exit For
                            End If
                        Next
                    '----

                    Case "Static?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.Axial.IsStatic = True, "Yes", "No")

                    Case "Rotating?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.Axial.IsRotating = True, "Yes", "No")

                    Case "RPM"
                        If (ProcessProj_In.App.Axial.RPM > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.RPM
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Reciprocating?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.Axial.IsRecip = True, "Yes", "No")

                    Case "Recip_Stroke"
                        If (ProcessProj_In.App.Axial.Recip_Stroke > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.Recip_Stroke
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Recip_Velocity"
                        If (ProcessProj_In.App.Axial.Recip_V > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.Recip_V
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Recip_Cycle Rate"
                        If (ProcessProj_In.App.Axial.Recip_CycleRate > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.Recip_CycleRate
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Oscillatory?"
                        mPDS_Val(i) = IIf(ProcessProj_In.App.Axial.IsOscilatory = True, "Yes", "No")

                    Case "Oscilate_Rot"
                        If (ProcessProj_In.App.Axial.Oscilate_Rot > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.Oscilate_Rot
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Oscilate_Velocity"
                        If (ProcessProj_In.App.Axial.Oscilate_V > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.Oscilate_V
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Oscilate_Cycle Rate"
                        If (ProcessProj_In.App.Axial.Oscilate_CycleRate > gcEPS) Then
                            mPDS_Val(i) = ProcessProj_In.App.Axial.Oscilate_CycleRate
                        Else
                            mPDS_Val(i) = ""
                        End If

                    Case "Customer Drawing #"
                        mPDS_Val(i) = ProcessProj_In.Design.CustDwgNo

                    Case "Customer Drawing Rev"
                        mPDS_Val(i) = ProcessProj_In.Design.CustDwgRev

                    Case "Frozen Desgin?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Design.Frozen.Design = True, "Yes", "No")

                    Case "Frozen Process?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Design.Frozen.Process = True, "Yes", "No")

                    Case "Class 1?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Design.IsClass1 = True, "Yes", "No")

                    Case "Build to Print?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Design.IsBuildToPrint = True, "Yes", "No")


                    Case "Winnovation No"
                        If (ProcessProj_In.Design.IsWinnovation) Then
                            mPDS_Val(i) = ProcessProj_In.Design.WinnovationNo
                        Else
                            mPDS_Val(i) = ""
                        End If


                    Case "Material Specification Seal"
                        mPDS_Val(i) = PartProj_In.PNR.HW.MatName

                    'Case "Spring Material Specification"
                    '    mPDS_Val(i) = ProcessProj_In.Design.CustDwgRev

                    Case "Heat Treat Type"
                        Dim pTemperCode As Integer = PartProj_In.PNR.HW.Temper

                        If (pTemperCode = 1) Then
                            mPDS_Val(i) = "Work Hardened"
                        ElseIf (pTemperCode = 2) Then
                            mPDS_Val(i) = "Age Hardened"
                        ElseIf (pTemperCode = 4) Then
                            mPDS_Val(i) = "Annealed"
                        ElseIf (pTemperCode = 6) Then
                            mPDS_Val(i) = "Solution and Precip"
                        ElseIf (pTemperCode = 8) Then
                            mPDS_Val(i) = "NACE"
                        Else
                            mPDS_Val(i) = ""
                        End If


                    Case "Plating/Coating Type"
                        If (PartProj_In.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                            If (PartProj_In.PNR.HW.Coating = "None") Then
                                mPDS_Val(i) = "None"
                            Else
                                mPDS_Val(i) = PartProj_In.PNR.HW.Coating
                            End If

                        ElseIf (PartProj_In.PNR.SealType = clsPartProject.clsPNR.eType.C Or PartProj_In.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                            If (PartProj_In.PNR.HW.Plating.Code <> "") Then
                                mPDS_Val(i) = PartProj_In.PNR.HW.Plating.Code
                            Else
                                mPDS_Val(i) = "None"

                            End If

                        End If

                    Case "Plating Thickness"
                        If (PartProj_In.PNR.SealType = clsPartProject.clsPNR.eType.C Or PartProj_In.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                            If (PartProj_In.PNR.HW.Plating.Code <> "") Then
                                mPDS_Val(i) = PartProj_In.PNR.HW.Plating.ThickCode
                            End If

                        End If

                    Case "Plating/Coating by Outside Vendor?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Design.IsMat_OutsideVender = True, "Yes", "No")

                    Case "Lessons Learned"
                        mPDS_Val(i) = ProcessProj_In.Design.LessonsLearned

                    Case "Design Inputs"
                        mDesignInput = mPDS_CellColName(i)

                    Case "FOD Design Risks"
                        mPDS_Val(i) = ProcessProj_In.Design.FOD_Risks

                    Case "Raw Material Part No"
                        mPDS_Val(i) = ProcessProj_In.Manf.BaseMat_PartNo

                    Case "Spring Material Part No"
                        mPDS_Val(i) = ProcessProj_In.Manf.SpringMat_PartNo

                    Case "Heat Treat Process No"
                        mPDS_Val(i) = ProcessProj_In.Manf.HT

                    Case "Precompression Glue"
                        mPDS_Val(i) = ProcessProj_In.Manf.PreComp_Glue

                    Case "Precompression Glue"
                        mPDS_Val(i) = ProcessProj_In.Manf.PreComp_Glue

                    Case "CustSpecType"
                        mCustSpecType = mPDS_CellColName(i)

                    Case "CustSpecInterpretation"
                        mCustSpecInterpretation = mPDS_CellColName(i)

                    Case "Seal Dimensions Name"
                        mSealDimName = mPDS_CellColName(i)

                    Case "Seal Dimensions Min (in)"
                        mSealDimMin_in = mPDS_CellColName(i)

                    Case "Seal Dimensions Max (in)"
                        mSealDimMax_in = mPDS_CellColName(i)

                    Case "Seal Dimensions Min (mm)"
                        mSealDimMin_mm = mPDS_CellColName(i)

                    Case "Seal Dimensions Max (mm)"
                        mSealDimMax_mm = mPDS_CellColName(i)

                    Case "Tooling and Gages Part No"
                        mTool_Gage_PartNo = mPDS_CellColName(i)

                    Case "Tooling and Gages Description"
                        mTool_Gage_Desc = mPDS_CellColName(i)

                    Case "Tooling and Gages Responsibility"
                        mTool_Gage_Response = mPDS_CellColName(i)

                    '....Quality
                    Case "Approved Suppliers Only?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Qlty.IsApvdSupplierOnly = True, "Yes", "No")

                    Case "Separate Tooling and Inspectin Gages (Split Ring)?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Qlty.Separate_Tool_Gage_Reqd = True, "Yes", "No")

                'Case "Customer Complaints on Similar Products"
                '    mPDS_Val(i) = IIf(ProcessProj_In.Qlty.HasCustComplaint = True, "Yes", "No")

                    Case "Visual Inspection with magnification"
                        Dim pVisualInspection As String = ""
                        If (ProcessProj_In.Qlty.VisualInspection) Then
                            pVisualInspection = "Yes, " & ProcessProj_In.Qlty.VisualInspection_Type
                        Else
                            pVisualInspection = "No"
                        End If
                        mPDS_Val(i) = pVisualInspection

                    Case "Customer Acceptance Standards"
                        mPDS_Val(i) = ProcessProj_In.Qlty.CustAcceptStd

                    Case "SPC Required?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Qlty.SPC_Reqd = True, "Yes", "No")

                    Case "Gage R&Rs Required?"
                        mPDS_Val(i) = IIf(ProcessProj_In.Qlty.GageRnR_Reqd = True, "Yes", "No")

                    '....Drawing
                    Case "Design Level"
                        mPDS_Val(i) = ProcessProj_In.Dwg.DesignLevel

                    Case "Drawing No"
                        mDwg_Needed_DwgNo = mPDS_CellColName(i)

                    Case "Drawing Description"
                        mDwg_Needed_DwgDesc = mPDS_CellColName(i)

                    Case "BOM's Parent Part No"
                        mBOM_Parent_PartNo = mPDS_CellColName(i)

                    Case "BOM's Child Part No"
                        mBOM_Child_PartNo = mPDS_CellColName(i)

                    Case "BOM's Qty"
                        mBOM_Qty = mPDS_CellColName(i)

                    '....Testing
                    Case "Leak Compress To Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.LUnit_Cust

                    Case "Leak Compress To (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Compress_Unplated

                    Case "Leak Media (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Medium_Unplated

                    Case "Leak Pressure Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.PUnit_Cust

                    Case "Leak Pressure (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Press_Unplated

                    Case "Leak Requirement Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.LeakUnit_Cust

                    Case "Leak Requirement (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Max_Unplated

                    Case "Leak Qty (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Qty_Unplated

                    Case "Leak Frequency (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Freq_Unplated

                    Case "Leak Compress To (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Compress_Plated

                    Case "Leak Media (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Medium_Plated

                    Case "Leak Pressure (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Press_Plated

                    Case "Leak Requirement (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Max_Plated

                    Case "Leak Qty (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Qty_Plated

                    Case "Leak Frequency (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Leak.Freq_Plated

                    Case "Load Compress To Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.LUnit_Cust

                    Case "Load Compress To (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Compress_Unplated

                    Case "Load Requirement Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.FUnit_Cust & "/" & ProcessProj_In.Unit.LUnit_Cust

                    Case "Load Requirement (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Max_Unplated

                    Case "Load Qty (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Qty_Unplated

                    Case "Load Frequency (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Freq_Unplated

                    Case "Load Compress To (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Compress_Plated

                    Case "Load Requirement (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Max_Plated

                    Case "Load Qty (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Qty_Plated

                    Case "Load Frequency (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.Load.Freq_Plated

                    Case "Springback Compress To Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.LUnit_Cust

                    Case "Springback Compress To (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Compress_Unplated

                    Case "Springback Requirement Unit"
                        mPDS_Val(i) = ProcessProj_In.Unit.LUnit_Cust

                    Case "Springback Requirement (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Max_Unplated

                    Case "Springback Qty (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Qty_Unplated

                    Case "Springback Frequency (Pre-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Freq_Unplated

                    Case "Springback Compress To (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Compress_Plated

                    Case "Springback Requirement (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Max_Plated

                    Case "Springback Qty (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Qty_Plated

                    Case "Springback Frequency (Post-Plate)"
                        mPDS_Val(i) = ProcessProj_In.Test.SpringBack.Freq_Plated

                    '....Approval
                    Case "Attendees Department"
                        mAttendeesStartColName_Dept = mPDS_CellColName(i)

                    Case "Attendees Signature"
                        mAttendeesStartColName_Sign = mPDS_CellColName(i)

                    Case "Attendees Name"
                        mAttendeesStartColName_Name = mPDS_CellColName(i)

                    Case "Attendees Title"
                        mAttendeesStartColName_Title = mPDS_CellColName(i)

                    Case "Attendees Date"
                        mAttendeesStartColName_Date = mPDS_CellColName(i)


                End Select

            Next

        Catch ex As Exception

        End Try

    End Sub


    Private Sub WritePDSFile(ByVal ProcessProj_In As clsProcessProj, ByVal PartProj_In As clsPartProject)
        '================================================================================================

        CloseExcelFiles()

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing

        Dim pSealTestDBEntities As New SealTestDBEntities

        Try
            pWkbOrg = pApp.Workbooks.Open(mcPDSReportFileName, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value,
                                          Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                          Missing.Value, Missing.Value, Missing.Value)

            Dim pWkSheet As EXCEL.Worksheet

            pWkSheet = pWkbOrg.Worksheets("General")

            Dim pExcelCellRange As EXCEL.Range = Nothing

            For i As Integer = 0 To mPDS_CellColName.Count - 1

                If (mPDS_Val(i) <> "") Then
                    pExcelCellRange = pWkSheet.Range(mPDS_CellColName(i)) : pExcelCellRange.Value = mPDS_Val(i)
                End If
            Next

            '....DesignInput
            For i As Integer = 0 To ProcessProj_In.Design.Input.ID_Input.Count - 1
                Dim pColumn_Input As String = mDesignInput.Substring(0, 1)
                Dim pIndex As Integer = ConvertToInt(mDesignInput.Substring(1, mDesignInput.Length - 1)) + i
                pExcelCellRange = pWkSheet.Range(pColumn_Input & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Design.Input.Desc(i)
            Next

            '....CustSpec
            For i As Integer = 0 To ProcessProj_In.Design.CustSpec.ID_Cust.Count - 1
                Dim pColumn_Type As String = mCustSpecType.Substring(0, 1)
                Dim pColumn_Interpretation As String = mCustSpecInterpretation.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mCustSpecType.Substring(1, mCustSpecType.Length - 1)) + i
                Dim pCustSpecDesc As String = ProcessProj_In.Design.CustSpec.Type(i)
                If (ProcessProj_In.Design.CustSpec.Desc(i) <> "") Then
                    pCustSpecDesc = pCustSpecDesc & " - " & ProcessProj_In.Design.CustSpec.Desc(i)
                End If
                pExcelCellRange = pWkSheet.Range(pColumn_Type & pIndex.ToString()) : pExcelCellRange.Value = pCustSpecDesc
                pExcelCellRange = pWkSheet.Range(pColumn_Interpretation & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Design.CustSpec.Interpret(i)
            Next

            '....Seal Dim
            For i As Integer = 0 To ProcessProj_In.Design.SealDim.ID_Seal.Count - 1
                Dim pColumn_Name As String = mSealDimName.Substring(0, 1)
                Dim pColumn_Min_in As String = mSealDimMin_in.Substring(0, 1)
                Dim pColumn_Max_in As String = mSealDimMax_in.Substring(0, 1)
                Dim pColumn_Min_mm As String = mSealDimMin_mm.Substring(0, 1)
                Dim pColumn_Max_mm As String = mSealDimMax_mm.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mSealDimName.Substring(1, mSealDimName.Length - 1)) + i
                Dim pMinVal As String = ""
                If (ProcessProj_In.Design.SealDim.Min(i) > gcEPS) Then
                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                        pMinVal = ProcessProj_In.Design.SealDim.Min(i)
                    Else
                        pMinVal = ConvMMToIn(ProcessProj_In.Design.SealDim.Min(i))
                    End If

                End If
                Dim pMaxVal As String = ""
                If (ProcessProj_In.Design.SealDim.Max(i) > gcEPS) Then
                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                        pMaxVal = ProcessProj_In.Design.SealDim.Max(i)
                    Else
                        pMaxVal = ConvMMToIn(ProcessProj_In.Design.SealDim.Max(i))
                    End If

                End If
                pExcelCellRange = pWkSheet.Range(pColumn_Name & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Design.SealDim.Name(i)
                pExcelCellRange = pWkSheet.Range(pColumn_Min_in & pIndex.ToString()) : pExcelCellRange.Value = pMinVal
                pExcelCellRange = pWkSheet.Range(pColumn_Max_in & pIndex.ToString()) : pExcelCellRange.Value = pMaxVal

                If (ProcessProj_In.Design.SealDim.Min(i) > gcEPS) Then
                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                        pMinVal = ConvInToMM(ProcessProj_In.Design.SealDim.Min(i))
                    Else
                        pMinVal = ProcessProj_In.Design.SealDim.Min(i)
                    End If

                End If

                If (ProcessProj_In.Design.SealDim.Max(i) > gcEPS) Then
                    If (ProcessProj_In.Unit.LUnit_Cust = "in") Then
                        pMaxVal = ConvInToMM(ProcessProj_In.Design.SealDim.Max(i))
                    Else
                        pMaxVal = ProcessProj_In.Design.SealDim.Max(i)
                    End If

                End If

                pExcelCellRange = pWkSheet.Range(pColumn_Min_mm & pIndex.ToString()) : pExcelCellRange.Value = pMinVal
                pExcelCellRange = pWkSheet.Range(pColumn_Max_mm & pIndex.ToString()) : pExcelCellRange.Value = pMaxVal
            Next

            '....Manf. Tool_Gage
            For i As Integer = 0 To ProcessProj_In.Manf.ToolNGage.ID_Tool.Count - 1
                Dim pColumn_PartNo As String = mTool_Gage_PartNo.Substring(0, 1)
                Dim pColumn_Desc As String = mTool_Gage_Desc.Substring(0, 1)
                Dim pColumn_Response As String = mTool_Gage_Response.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mTool_Gage_PartNo.Substring(1, mTool_Gage_PartNo.Length - 1)) + i
                pExcelCellRange = pWkSheet.Range(pColumn_PartNo & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Manf.ToolNGage.PartNo(i)
                Dim pDesc As String = ProcessProj_In.Manf.ToolNGage.Desc(i)
                If (pDesc <> "") Then
                    pDesc = pDesc & "  " & ProcessProj_In.Manf.ToolNGage.Type(i)
                End If
                pExcelCellRange = pWkSheet.Range(pColumn_Desc & pIndex.ToString()) : pExcelCellRange.Value = pDesc

                Dim pStatus As String = ""
                If (ProcessProj_In.Manf.ToolNGage.Status(i) <> "" And Not IsNothing(ProcessProj_In.Manf.ToolNGage.Status(i))) Then
                    If (ProcessProj_In.Manf.ToolNGage.Status(i).Trim() = "Inventory") Then
                        pStatus = "Yes" & "  " & ProcessProj_In.Manf.ToolNGage.DesignResponsibility(i)
                    Else
                        pStatus = "No" & "  " & ProcessProj_In.Manf.ToolNGage.DesignResponsibility(i)
                    End If
                End If

                pExcelCellRange = pWkSheet.Range(pColumn_Response & pIndex.ToString()) : pExcelCellRange.Value = pStatus

            Next

            '....Dwg_Needed
            For i As Integer = 0 To ProcessProj_In.Dwg.Needed.ID_Needed.Count - 1
                Dim pColumn_DwgNo As String = mDwg_Needed_DwgNo.Substring(0, 1)
                Dim pColumn_DwgDesc As String = mDwg_Needed_DwgDesc.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mDwg_Needed_DwgNo.Substring(1, mDwg_Needed_DwgNo.Length - 1)) + i
                pExcelCellRange = pWkSheet.Range(pColumn_DwgNo & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Dwg.Needed.DwgNo(i)
                pExcelCellRange = pWkSheet.Range(pColumn_DwgDesc & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Dwg.Needed.Desc(i)

            Next

            '....BOM
            For i As Integer = 0 To ProcessProj_In.Dwg.BOM.ID_BOM.Count - 1
                Dim pColumn_ParentPartNo As String = mBOM_Parent_PartNo.Substring(0, 1)
                Dim pColumn_ChildPartNo As String = mBOM_Child_PartNo.Substring(0, 1)
                Dim pColumn_Qty As String = mBOM_Qty.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mBOM_Parent_PartNo.Substring(1, mBOM_Parent_PartNo.Length - 1)) + i
                pExcelCellRange = pWkSheet.Range(pColumn_ParentPartNo & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Dwg.BOM.Parent_PartNo(i)
                pExcelCellRange = pWkSheet.Range(pColumn_ChildPartNo & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Dwg.BOM.Child_PartNo(i)
                pExcelCellRange = pWkSheet.Range(pColumn_Qty & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Dwg.BOM.Qty(i)

            Next

            Dim pSealSuiteEntities As New SealSuiteDBEntities()
            Dim mcImgAspectRatio As Double = 1.8
            '....Attendees
            For i As Integer = 0 To ProcessProj_In.Approval.Dept.Count - 1
                Dim pColumn_Dept As String = mAttendeesStartColName_Dept.Substring(0, 1)
                Dim pColumn_Signature As String = mAttendeesStartColName_Sign.Substring(0, 1)
                Dim pColumn_Name As String = mAttendeesStartColName_Name.Substring(0, 1)
                Dim pColumn_Title As String = mAttendeesStartColName_Title.Substring(0, 1)
                Dim pColumn_Date As String = mAttendeesStartColName_Date.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mAttendeesStartColName_Dept.Substring(1, mAttendeesStartColName_Dept.Length - 1)) + i
                pExcelCellRange = pWkSheet.Range(pColumn_Dept & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Approval.Dept(i)

                'AES 19APR18
                If (Not IsNothing(ProcessProj_In.Approval.Name(i))) Then

                    Dim pQry = (From pRec In pSealSuiteEntities.tblUser).ToList()

                    If (pQry.Count > 0) Then

                        For j As Integer = 0 To pQry.Count - 1
                            Dim pName As String = pQry(j).fldFirstName + " " + pQry(j).fldLastName

                            If (ProcessProj_In.Approval.Name(i).Trim() = pName.Trim()) Then
                                Dim pID As Integer = pQry(j).fldID
                                Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldID = pID Select pRec).ToList()

                                Dim pWidth As Integer = 120
                                Dim pHeight As Integer = pWidth / mcImgAspectRatio

                                If (pQryUser.Count > 0) Then
                                    If Not IsNothing(pQryUser(0).fldSignature) Then
                                        Dim pArray As Byte() = DirectCast(pQryUser(0).fldSignature, Byte())
                                        Dim pMS As New MemoryStream(pArray)

                                        Dim pImage As Image = Image.FromStream(pMS)

                                        Dim pBmp As Bitmap = New Bitmap(pImage)
                                        Dim pNewBmp As Bitmap = New Bitmap(pWidth, pHeight)

                                        '....Declare graphic taken from new bitmap
                                        Dim pGr As Graphics = Graphics.FromImage(pNewBmp)
                                        pGr.DrawImage(pBmp, 0, 0, pNewBmp.Width, pNewBmp.Height)

                                        Dim pTemp_ImagePath As String = "C:\SealSuite\Images\User.jpeg"
                                        pNewBmp.Save(pTemp_ImagePath, Imaging.ImageFormat.Jpeg)

                                        pExcelCellRange = pWkSheet.Range(pColumn_Signature & pIndex.ToString())
                                        pWkSheet.Shapes.AddPicture(pTemp_ImagePath, False, True, pExcelCellRange.Left, pExcelCellRange.Top, pExcelCellRange.Width * 2, pExcelCellRange.Height)
                                        'Dim picture As EXCEL.Picture = pWkSheet.Pictures.(1, 1, pTemp_ImagePath)


                                        If (File.Exists(pTemp_ImagePath)) Then
                                            File.Delete(pTemp_ImagePath)
                                        End If
                                    End If
                                End If
                            End If

                        Next

                    End If


                    ''If (ProcessProj_In.Approval.Name(i).Trim() = "Adam") Then
                    ''    Dim pQryUser = (From pRec In pSealSuiteEntities.tblUser Where pRec.fldID = 1 Select pRec).ToList()

                    ''    Dim pWidth As Integer = 120
                    ''    Dim pHeight As Integer = pWidth / mcImgAspectRatio

                    ''    If (pQryUser.Count > 0) Then
                    ''        If Not IsNothing(pQryUser(0).fldSignature) Then
                    ''            Dim pArray As Byte() = DirectCast(pQryUser(0).fldSignature, Byte())
                    ''            Dim pMS As New MemoryStream(pArray)

                    ''            'pExcelCellRange = pWkSheet.Range("B" & (i + 1).ToString()) : pExcelCellRange.Value = Image.FromStream(pMS)
                    ''            Dim pImage As Image = Image.FromStream(pMS)
                    ''            ''pWkSheet.Cells(i + 2, 2) = Image.FromStream(pMS)

                    ''            Dim pBmp As Bitmap = New Bitmap(pImage)
                    ''            'Dim pWidth As Integer = 120
                    ''            'Dim pHeight As Integer = pWidth / mcImgAspectRatio
                    ''            Dim pNewBmp As Bitmap = New Bitmap(pWidth, pHeight)

                    ''            '....Declare graphic taken from new bitmap
                    ''            Dim pGr As Graphics = Graphics.FromImage(pNewBmp)
                    ''            pGr.DrawImage(pBmp, 0, 0, pNewBmp.Width, pNewBmp.Height)

                    ''            Dim pTemp_ImagePath As String = "C:\SealSuite\Images\User.jpeg"
                    ''            pNewBmp.Save(pTemp_ImagePath, Imaging.ImageFormat.Jpeg)

                    ''            pExcelCellRange = pWkSheet.Range(pColumn_Signature & pIndex.ToString())
                    ''            pWkSheet.Shapes.AddPicture(pTemp_ImagePath, False, True, pExcelCellRange.Left, pExcelCellRange.Top, pExcelCellRange.Width * 2, pExcelCellRange.Height)
                    ''            'Dim picture As EXCEL.Picture = pWkSheet.Pictures.(1, 1, pTemp_ImagePath)


                    ''            If (File.Exists(pTemp_ImagePath)) Then
                    ''                File.Delete(pTemp_ImagePath)
                    ''            End If
                    ''        End If
                    ''    End If
                    ''End If
                End If

                'pExcelCellRange = pWkSheet.Range(pColumn_Signature & pIndex.ToString()) : pExcelCellRange.Insert(pTemp_ImagePath)

                pExcelCellRange = pWkSheet.Range(pColumn_Name & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Approval.Name(i)
                pExcelCellRange = pWkSheet.Range(pColumn_Title & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Approval.Title(i)

                If (ProcessProj_In.Approval.DateSigned(i) <> DateTime.MinValue) Then
                    pExcelCellRange = pWkSheet.Range(pColumn_Date & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.Approval.DateSigned(i)
                Else
                    pExcelCellRange = pWkSheet.Range(pColumn_Date & pIndex.ToString()) : pExcelCellRange.Value = ""
                End If

            Next

            pWkSheet = pWkbOrg.Worksheets("Issues-Comments")
            mIssueCommentStartColName_Issue = "A4"
            mIssueCommentStartColName_By = "B4"
            mIssueCommentStartColName_Date = "C4"
            mIssueCommentStartColName_Resolution = "D4"

            Dim pCI As New CultureInfo("en-US")

            For i As Integer = 0 To ProcessProj_In.IssueCommnt.ID.Count - 1
                Dim pColumn_Issue As String = mIssueCommentStartColName_Issue.Substring(0, 1)
                Dim pColumn_By As String = mIssueCommentStartColName_By.Substring(0, 1)
                Dim pColumn_Date As String = mIssueCommentStartColName_Date.Substring(0, 1)
                Dim pColumn_Resolution As String = mIssueCommentStartColName_Resolution.Substring(0, 1)

                Dim pIndex As Integer = ConvertToInt(mIssueCommentStartColName_Issue.Substring(1, mIssueCommentStartColName_Issue.Length - 1)) + i
                pExcelCellRange = pWkSheet.Range(pColumn_Issue & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.IssueCommnt.Comment(i)

                pExcelCellRange = pWkSheet.Range(pColumn_By & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.IssueCommnt.ByDept(i)


                If (ProcessProj_In.IssueCommnt.ByDate(i) <> DateTime.MinValue) Then
                    pExcelCellRange = pWkSheet.Range(pColumn_Date & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.IssueCommnt.ByDate(i).ToString("MM/dd/yyyy", pCI.DateTimeFormat())
                Else
                    pExcelCellRange = pWkSheet.Range(pColumn_Date & pIndex.ToString()) : pExcelCellRange.Value = ""
                End If

                pExcelCellRange = pWkSheet.Range(pColumn_Resolution & pIndex.ToString()) : pExcelCellRange.Value = ProcessProj_In.IssueCommnt.Resolution(i)
            Next

            pWkSheet = pWkbOrg.Worksheets("DateTracking")
            pWkSheet.Cells(6, 5) = ProcessProj_In.OrdEntry.LeadTimeQuoted

            Dim pOutputFileName As String = PartProj_In.PNR.PN() & PartProj_In.PNR.PN_Rev() & " PDS_RevW"
            pWkbOrg.SaveAs(mcDirOutput & pOutputFileName)

        Catch ex As Exception
            MessageBox.Show(ex.ToString())

        Finally


            pApp.Visible = True

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

#Region "HELPER ROUTINES:"

    Private Function ConvFtoC(ByVal Val_In As Double) As Double
        '====================================================
        Dim pC As Double
        pC = (5 * Val_In - 160) / 9
        Return pC
    End Function


    Public Function ConvCToF(ByVal C_In As Double) As Double
        '====================================================
        Return (9 * C_In + 160) / 5

    End Function

    Private Function ConvPSIToBar(ByVal Val_In As Double) As Double
        '==========================================================
        Dim pcPSIToBar_ConvFac As Double = 0.0689476F
        Dim pVal As Double
        pVal = Val_In * pcPSIToBar_ConvFac
        Return pVal
    End Function

    Private Function ConvInToMM(ByVal Val_In As Double) As Double
        '========================================================
        Dim pcInToMM_ConvFac As Double = 25.4
        Dim pVal As Double
        pVal = Val_In * pcInToMM_ConvFac
        Return pVal

    End Function

    Private Function ConvMMToIn(ByVal Val_In As Double) As Double
        '========================================================
        Dim pcInToMM_ConvFac As Double = 25.4
        Dim pVal As Double
        pVal = Val_In / pcInToMM_ConvFac
        Return pVal

    End Function

#End Region


#End Region

#Region "PD - DropDownLists:"

    Public Sub ReadDropDownLst(ByVal FileName_In As String)
        '==================================================
    End Sub

#End Region
  
End Class
