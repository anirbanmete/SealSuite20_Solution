'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsProcessProj_Risk                    '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  19FEB18                                '
'                                                                              '
'===============================================================================
Imports System.Linq
Imports EXCEL = Microsoft.Office.Interop.Excel
Imports System.Reflection
Imports System.IO
Imports System.Threading
<Serializable()> _
Public Class clsProcessProj_Risk

#Region "MEMBER VARIABLES:"

    Private mTabName As String
    Private mRiskAnlayisQ As List(Of Dictionary(Of String, Integer))
    Private mAnswered As List(Of Boolean)
    Private mReason As List(Of String)

    '....Tab Variables
    Dim mPreOrder, mExport, mOrdEntry, mCost, mApp, mDesign, mManf, mPurchase, mQlty, mDwg, mTest, mPlanning, mShipping As Boolean

#End Region

#Region "PROPERTY ROUTINES:"

    '....TabName
    Public Property TabName() As String
        '==============================
        Get
            Return mTabName
        End Get

        Set(Obj As String)
            mTabName = Obj
        End Set
    End Property


    '....RiskAnlayisQ
    Public Property RiskAnlayisQ() As List(Of Dictionary(Of String, Integer))
        '====================================================================
        Get
            Return mRiskAnlayisQ
        End Get

        Set(Obj As List(Of Dictionary(Of String, Integer)))
            mRiskAnlayisQ = Obj
        End Set
    End Property

    '....Answered
    Public Property Answered() As List(Of Boolean)
        '=========================================
        Get
            Return mAnswered
        End Get

        Set(Obj As List(Of Boolean))
            mAnswered = Obj
        End Set
    End Property

    '....Reason
    Public Property Reason() As List(Of String)
        '=======================================
        Get
            Return mReason
        End Get

        Set(Obj As List(Of String))
            mReason = Obj
        End Set
    End Property

#End Region

    Public Sub LoadRiskQ(ByVal FileName_In As String)
        '==============================================
        CloseExcelFiles()

        Dim pApp As EXCEL.Application = Nothing
        pApp = New EXCEL.Application()
        'pApp.Visible = True

        pApp.DisplayAlerts = False

        '....Open Load.xls WorkBook.
        Dim pWkbOrg As EXCEL.Workbook = Nothing
        Dim pExitLoop As Boolean = False

        Dim pSealProcessDBEntities As New SealProcessDBEntities()

        Try
            pWkbOrg = pApp.Workbooks.Open(FileName_In, Missing.Value, False, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value)

            Dim pWkSheet As EXCEL.Worksheet

            pWkSheet = pWkbOrg.Worksheets("Risk Analysis Qs")

            Dim pHistoryID As Integer = 1

            Dim pQryHistory = (From Rec In pSealProcessDBEntities.tblFileHistory_RiskAnaQSet Order By Rec.fldID Descending
                               Select Rec).ToList()

            If (pQryHistory.Count > 0) Then
                pHistoryID = pQryHistory(0).fldID + 1
            End If

            Dim pTblHistory As New tblFileHistory_RiskAnaQSet

            pTblHistory.fldID = pHistoryID
            pTblHistory.fldFileName = Path.GetFileName(FileName_In)
            pTblHistory.fldDate = DateTime.Now()

            pSealProcessDBEntities.AddTotblFileHistory_RiskAnaQSet(pTblHistory)
            pSealProcessDBEntities.SaveChanges()

            '....Table - tblRiskAnaQSet
            Dim pRiskAnaQ_Start As Integer = 2

            Dim pQuestion As String

            Dim pIndx As Integer = 0

            While (Not pExitLoop)

                Dim pVal As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start).value
                Dim pPreOrder As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 1).value
                Dim pExport As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 2).value
                Dim pOrdEntry As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 3).value
                Dim pCost As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 4).value
                Dim pApplication As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 5).value
                Dim pDesign As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 6).value
                Dim pManf As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 7).value
                Dim pPurchase As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 8).value
                Dim pQlty As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 9).value
                Dim pDwg As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 10).value
                Dim pTest As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 11).value
                Dim pPlanning As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 12).value
                Dim pShipping As String = pWkSheet.Cells(7 + pIndx, pRiskAnaQ_Start + 13).value

                pExitLoop = String.IsNullOrEmpty(pVal)

                mPreOrder = False
                mExport = False
                mOrdEntry = False
                mCost = False
                mApp = False
                mDesign = False
                mManf = False
                mPurchase = False
                mQlty = False
                mDwg = False
                mTest = False
                mPlanning = False
                mShipping = False

                If (Not pExitLoop) Then
                    pQuestion = pVal

                    If (Not IsNothing(pPreOrder)) Then
                        If (pPreOrder.Trim() = "T") Then
                            mPreOrder = True
                        End If
                    End If

                    If (Not IsNothing(pExport)) Then
                        If (pExport.Trim() = "T") Then
                            mExport = True
                        End If
                    End If

                    If (Not IsNothing(pOrdEntry)) Then
                        If (pOrdEntry.Trim() = "T") Then
                            mOrdEntry = True
                        End If
                    End If

                    If (Not IsNothing(pCost)) Then
                        If (pCost.Trim() = "T") Then
                            mCost = True
                        End If
                    End If

                    If (Not IsNothing(pApplication)) Then
                        If (pApplication.Trim() = "T") Then
                            mApp = True
                        End If
                    End If

                    If (Not IsNothing(pDesign)) Then
                        If (pDesign.Trim() = "T") Then
                            mDesign = True
                        End If
                    End If

                    If (Not IsNothing(pManf)) Then
                        If (pManf.Trim() = "T") Then
                            mManf = True
                        End If
                    End If

                    If (Not IsNothing(pPurchase)) Then
                        If (pPurchase.Trim() = "T") Then
                            mPurchase = True
                        End If
                    End If

                    If (Not IsNothing(pQlty)) Then
                        If (pQlty.Trim() = "T") Then
                            mQlty = True
                        End If
                    End If

                    If (Not IsNothing(pDwg)) Then
                        If (pDwg.Trim() = "T") Then
                            mDwg = True
                        End If
                    End If

                    If (Not IsNothing(pTest)) Then
                        If (pTest.Trim() = "T") Then
                            mTest = True
                        End If
                    End If

                    If (Not IsNothing(pPlanning)) Then
                        If (pPlanning.Trim() = "T") Then
                            mPlanning = True
                        End If
                    End If

                    If (Not IsNothing(pShipping)) Then
                        If (pShipping.Trim() = "T") Then
                            mShipping = True
                        End If
                    End If

                    pIndx = pIndx + 1

                    Dim pRiskAnaQ_ID As Integer = 1
                    Dim pQryRiskAnaQ = (From Rec In pSealProcessDBEntities.tblRiskAnaQSet Where Rec.fldHistoryID = pHistoryID Order By Rec.fldID Descending
                                        Select Rec).ToList()

                    If (pQryRiskAnaQ.Count > 0) Then
                        pRiskAnaQ_ID = pQryRiskAnaQ(0).fldID + 1
                    End If

                    Dim pTabName As String = ""
                    If (mPreOrder) Then
                        pTabName = "PreOrder,"
                    End If
                    If (mExport) Then
                        pTabName = pTabName & "Export,"
                    End If
                    If (mOrdEntry) Then
                        pTabName = pTabName & "OrdEntry,"
                    End If
                    If (mCost) Then
                        pTabName = pTabName & "Cost,"
                    End If
                    If (mApp) Then
                        pTabName = pTabName & "App,"
                    End If
                    If (mDesign) Then
                        pTabName = pTabName & "Design,"
                    End If
                    If (mManf) Then
                        pTabName = pTabName & "Manf,"
                    End If
                    If (mPurchase) Then
                        pTabName = pTabName & "Purchase,"
                    End If
                    If (mQlty) Then
                        pTabName = pTabName & "Qlty,"
                    End If
                    If (mDwg) Then
                        pTabName = pTabName & "Dwg,"
                    End If
                    If (mTest) Then
                        pTabName = pTabName & "Test,"
                    End If
                    If (mPlanning) Then
                        pTabName = pTabName & "Planning,"
                    End If
                    If (mShipping) Then
                        pTabName = pTabName & "Shipping,"
                    End If

                    pTabName = pTabName.Substring(0, pTabName.Length - 1)

                    Dim pTblRiskAnaQ As New tblRiskAnaQSet

                    pTblRiskAnaQ.fldHistoryID = pHistoryID
                    pTblRiskAnaQ.fldID = pRiskAnaQ_ID
                    pTblRiskAnaQ.fldTabName = pTabName
                    pTblRiskAnaQ.fldDesc = pQuestion

                    pSealProcessDBEntities.AddTotblRiskAnaQSet(pTblRiskAnaQ)
                    pSealProcessDBEntities.SaveChanges()

                End If

            End While

            Dim pFileTitle As String = Path.GetFileName(FileName_In)
            Dim pMsg As String = "Risk Analysis Data Updated from: " & vbLf & Space(10) & pFileTitle
            MessageBox.Show(pMsg, "Risk Analysis DataFile!", MessageBoxButtons.OK)

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


End Class
