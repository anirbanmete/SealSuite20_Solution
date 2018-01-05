'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                       FORM MODULE  :  frmFEA                                 '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  11MAR16                                '
'                                                                              '
'===============================================================================

'Routines
'----------
'   Private Sub     frmFEA_Load                 ()
'   Private Sub     DisplayData                 ()
'   Private Sub     Set_Controls                ()

'   Private Sub     mnuItem_Click               ()
'   Private Sub     optButtons_CheckedChanged   ()

'   Private Sub     cmdRunANSYS_Click           ()
'   Private Sub     cmdFEAGraphics_Click        ()

'   Private Sub     cmdOK_Click                 ()
'   Private Sub     SaveData                    ()
'------------------------------------------------------------------------------
'

Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports SealIPELib = SealIPELib101


Public Class IPE_frmReportFormat
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents grpType As System.Windows.Forms.GroupBox
    Friend WithEvents optPDF As System.Windows.Forms.RadioButton
    Friend WithEvents tpFormat As System.Windows.Forms.ToolTip
    Friend WithEvents optWord As System.Windows.Forms.RadioButton

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmReportFormat))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpType = New System.Windows.Forms.GroupBox()
        Me.optPDF = New System.Windows.Forms.RadioButton()
        Me.optWord = New System.Windows.Forms.RadioButton()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tpFormat = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        Me.grpType.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(1, 1)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(306, 128)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.grpType)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Location = New System.Drawing.Point(2, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(304, 126)
        Me.Panel1.TabIndex = 1
        '
        'grpType
        '
        Me.grpType.Controls.Add(Me.optPDF)
        Me.grpType.Controls.Add(Me.optWord)
        Me.grpType.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpType.Location = New System.Drawing.Point(18, 14)
        Me.grpType.Name = "grpType"
        Me.grpType.Size = New System.Drawing.Size(265, 69)
        Me.grpType.TabIndex = 73
        Me.grpType.TabStop = False
        Me.grpType.Text = "Select Format"
        '
        'optPDF
        '
        Me.optPDF.AutoSize = True
        Me.optPDF.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPDF.Location = New System.Drawing.Point(170, 30)
        Me.optPDF.Name = "optPDF"
        Me.optPDF.Size = New System.Drawing.Size(47, 19)
        Me.optPDF.TabIndex = 1
        Me.optPDF.TabStop = True
        Me.optPDF.Text = "PDF"
        Me.optPDF.UseVisualStyleBackColor = True
        '
        'optWord
        '
        Me.optWord.AutoSize = True
        Me.optWord.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optWord.Location = New System.Drawing.Point(43, 30)
        Me.optWord.Name = "optWord"
        Me.optWord.Size = New System.Drawing.Size(56, 19)
        Me.optWord.TabIndex = 0
        Me.optWord.TabStop = True
        Me.optWord.Text = "Word"
        Me.optWord.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(212, 90)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 72
        Me.cmdOK.Text = "&OK"
        Me.tpFormat.SetToolTip(Me.cmdOK, "Report Creation will be done in selected format.")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tpFormat
        '
        Me.tpFormat.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.tpFormat.IsBalloon = True
        Me.tpFormat.ShowAlways = True
        '
        'IPE_frmReportFormat
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(308, 130)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmReportFormat"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Report Format"
        Me.Panel1.ResumeLayout(False)
        Me.grpType.ResumeLayout(False)
        Me.grpType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


#Region "FORM EVENT ROUTINES:"

    Private Const mcPDFFileName_Dll As String = _
                "C:\Program Files\Adobe\Acrobat 7.0\PDFMaker\Project\PDFMProject.dll"

    '*******************************************************************************
    '*                       FORM EVENT ROUTINES - BEGIN                           *
    '*******************************************************************************

    Private Sub frmReportFormat_Load(ByVal sender As Object, ByVal e As System.EventArgs) _
                                     Handles MyBase.Load
        '==========================================================================     

        optWord.Checked = True

        If Not File.Exists(mcPDFFileName_Dll) Then
            optPDF.Enabled = False
        End If

    End Sub

    '*******************************************************************************
    '*                       FORM EVENT ROUTINES - END                             *
    '*******************************************************************************

#End Region


#Region "CONTROL EVENT ROUTINES:"

    '*******************************************************************************
    '*                       CONTROL EVENT ROUTINES - BEGIN                        *
    '*******************************************************************************




    '*******************************************************************************
    '*                       CONTROL EVENT ROUTINES - END                          *
    '*******************************************************************************
#End Region


#Region "COMMAND BUTTON EVENT ROUTINE:"

    '*******************************************************************************
    '*                    COMMAND BUTTON EVENT ROUTINE - BEGIN                     *
    '*******************************************************************************

    Private Sub cmdOK_Click(ByVal sender As System.Object, _
                            ByVal e As System.EventArgs) _
                            Handles cmdOK.Click
        '==================================================
        SaveData()
    End Sub


    Private Sub SaveData()
        '=================

        With gIPE_Report

            If optWord.Checked = True Then
                .DocuFormat = "Word"
            ElseIf optPDF.Checked = True Then
                If File.Exists(mcPDFFileName_Dll) Then
                    .DocuFormat = "PDF"

                Else
                    MessageBox.Show("PDF Maker does not exist." & vbCrLf & _
                                    "Install PDF Writer to get Report in PDF.", "ERROR", _
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)


                    optPDF.Enabled = False
                    optWord.Checked = True
                    Exit Sub
                End If
            End If

            Me.Close()
        End With

    End Sub


    'Private Sub cmdCancel_Click(ByVal sender As System.Object, _
    '                            ByVal e As System.EventArgs) _
    '                            Handles cmdCancel.Click
    '    '========================================================

    '    Me.Close()

    'End Sub

    Private Sub cmdOK_MouseHover(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles cmdOK.MouseHover
        '=======================================================================

        tpFormat.SetToolTip(cmdOK, "Report will be created in selected format.")
    End Sub

    Private Sub cmdOK_MouseDown(ByVal sender As System.Object, _
                                ByVal e As System.Windows.Forms.MouseEventArgs) _
                                Handles cmdOK.MouseDown
        '=======================================================================

        tpFormat.SetToolTip(cmdOK, "Report creation is in progress....")
    End Sub

    '*******************************************************************************
    '*                      COMMAND BUTTON EVENT ROUTINES - END                    *
    '*******************************************************************************
#End Region



End Class
