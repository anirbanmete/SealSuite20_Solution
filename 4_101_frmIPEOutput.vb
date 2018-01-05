'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmOutput                              '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY17                                '
'                                                                              '
'===============================================================================

Imports Microsoft.Office.Interop.Word
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports SealIPELib = SealIPELib101


Public Class IPE_frmOutput

#Region "MEMBER VARIABLES:"

    Private mCavityExist As Boolean = False
    Private mPreProductionExist As Boolean

#End Region

    '....Cavity Exists
    Public ReadOnly Property CavityExist() As Boolean
        '============================================
        Get
            Return mCavityExist
        End Get

    End Property

    '....Pre-Production Exists
    Public ReadOnly Property PreProductionExist() As Boolean
        '====================================================
        Get
            Return mPreProductionExist
        End Get

    End Property

#Region "FORM EVENT ROUTINES:"

    Private Sub frmOutput_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '==========================================================================================

        Dim pAnaDesc As String
        Dim pISel As Integer = gIPE_frmResults.ISel
        Dim pFreeHt As String = gIPE_Project.Analysis(pISel).Seal.Hfree.ToString("#0.000").Replace(".", "")
        pAnaDesc = "MCS" & gIPE_Project.Analysis(pISel).Seal.MCrossSecNo & "_FH" & pFreeHt & _
                    "_" & gIPE_Project.Analysis(pISel).LoadCase.Name & "_" & gIPE_Project.Analysis(pISel).DateCreated.ToString("ddMMMyy") & _
                    "_" & gIPE_Project.Analysis(pISel).TimeCreated.ToString("t").Replace(":", "").Trim().Replace(" ", "")

        txtAnaDesc.Text = pAnaDesc
        chkCavityInfo.Checked = False
    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdButtons_Click(sender As System.Object,
                                 e As System.EventArgs) Handles cmdCustomer.Click, cmdPreProduction.Click, _
                                                                cmdWORD.Click, cmdPDF.Click, _
                                                                cmdCust_PPT.Click, cmdOK.Click
        '==============================================================================================
        Dim pcmdButton As Button = CType(sender, Button)

        Select Case pcmdButton.Name

            Case "cmdCustomer"
                '-----------------
                mPreProductionExist = False
                Dim pfrmDwg As New IPE_frmDwg()
                pfrmDwg.ShowDialog()


            Case "cmdPreProduction"
                '------------------------
                mPreProductionExist = True
                mCavityExist = False
                Dim pfrmDwg As New IPE_frmDwg()
                pfrmDwg.ShowDialog()


            Case "cmdWORD"
                '-----------
                gIPE_Report = New IPE_clsReport()
                gIPE_Report.DocuFormat = IPE_clsReport.eDocType.DOC
                Dim pFileName As String = ""
                saveFileDialog1.FilterIndex = 1
                saveFileDialog1.Filter = "DOC files (*.doc)|*.DOC"
                saveFileDialog1.Title = "Save"
                saveFileDialog1.FileName = gIPE_File.DirOut & "Report" & ".doc"

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then

                    pFileName = saveFileDialog1.FileName
                    gIPE_Report.WriteReport(gIPE_frmResults.ISel, gIPE_User, gIPE_Project, gIPE_Unit, pFileName)
                End If

            Case "cmdPDF"
                '-----------
                gIPE_Report = New IPE_clsReport()
                gIPE_Report.DocuFormat = IPE_clsReport.eDocType.PDF
                Dim pFileName As String = ""
                saveFileDialog1.FilterIndex = 1
                saveFileDialog1.Filter = "PDF files (*.PDF)|*.PDF"
                saveFileDialog1.Title = "Save"
                saveFileDialog1.FileName = gIPE_File.DirOut & "Report" & ".PDF"

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then

                    pFileName = saveFileDialog1.FileName
                    gIPE_Report.WriteReport(gIPE_frmResults.ISel, gIPE_User, gIPE_Project, gIPE_Unit, pFileName)

                End If

            Case "cmdCust_PPT"
                '------------------
                Dim pReportCust As New IPE_frmReportCust()
                pReportCust.ShowDialog()

            Case "cmdOK"
                '----------
                Me.Close()

        End Select

    End Sub


#End Region


#Region "TEXT BOX RELATED ROUTINES:"

    Private Sub txtAnaDesc_MouseHover(sender As System.Object, e As System.EventArgs) Handles txtAnaDesc.MouseHover
        '==========================================================================================================

        Dim pToolTip As New ToolTip()
        pToolTip.SetToolTip(Me.txtAnaDesc, txtAnaDesc.Text)

    End Sub

#End Region


    Private Sub chkCavityInfo_CheckedChanged(sender As System.Object,
                                             e As System.EventArgs) Handles chkCavityInfo.CheckedChanged
        '=================================================================================================
        If (chkCavityInfo.Checked) Then
            mCavityExist = True
        Else
            mCavityExist = False
        End If
    End Sub
End Class