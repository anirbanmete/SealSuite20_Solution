'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmDwg                                 '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28APR16                                '
'                                                                              '
'===============================================================================

Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.PowerPoint
Imports Microsoft.Office.Core
Imports System.Reflection

Public Class IPE_frmDwg
    Inherits System.Windows.Forms.Form

#Region "MEMBER VARIABLES:"

    Private mIPE_Dwg As IPE_clsDwg

#End Region


#Region " Windows Form Designer generated code "

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
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
    Public WithEvents PictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents cmdPrintDwg As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents imgLogo As System.Windows.Forms.PictureBox
    Friend WithEvents printDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents printDocSeal1 As System.Drawing.Printing.PrintDocument
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmDwg))
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.imgLogo = New System.Windows.Forms.PictureBox()
        Me.cmdPrintDwg = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.PictureBox = New System.Windows.Forms.PictureBox()
        Me.printDocSeal1 = New System.Drawing.Printing.PrintDocument()
        Me.printDialog1 = New System.Windows.Forms.PrintDialog()
        Me.Panel1.SuspendLayout()
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBorder
        '
        Me.lblBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBorder.BackColor = System.Drawing.Color.Black
        Me.lblBorder.Location = New System.Drawing.Point(4, 2)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(856, 717)
        Me.lblBorder.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.imgLogo)
        Me.Panel1.Controls.Add(Me.cmdPrintDwg)
        Me.Panel1.Controls.Add(Me.cmdClose)
        Me.Panel1.Controls.Add(Me.PictureBox)
        Me.Panel1.Location = New System.Drawing.Point(6, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(852, 713)
        Me.Panel1.TabIndex = 1
        '
        'imgLogo
        '
        Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
        Me.imgLogo.Location = New System.Drawing.Point(14, 641)
        Me.imgLogo.Name = "imgLogo"
        Me.imgLogo.Size = New System.Drawing.Size(176, 56)
        Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgLogo.TabIndex = 3
        Me.imgLogo.TabStop = False
        '
        'cmdPrintDwg
        '
        Me.cmdPrintDwg.BackColor = System.Drawing.Color.Silver
        Me.cmdPrintDwg.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintDwg.Image = CType(resources.GetObject("cmdPrintDwg.Image"), System.Drawing.Image)
        Me.cmdPrintDwg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdPrintDwg.Location = New System.Drawing.Point(382, 673)
        Me.cmdPrintDwg.Name = "cmdPrintDwg"
        Me.cmdPrintDwg.Size = New System.Drawing.Size(100, 28)
        Me.cmdPrintDwg.TabIndex = 2
        Me.cmdPrintDwg.Text = "    &Print "
        Me.cmdPrintDwg.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.Color.Silver
        Me.cmdClose.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Image = CType(resources.GetObject("cmdClose.Image"), System.Drawing.Image)
        Me.cmdClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New System.Drawing.Point(737, 675)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(90, 25)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "    &Close"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'PictureBox
        '
        Me.PictureBox.BackColor = System.Drawing.Color.White
        Me.PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox.Image = Global.SealSuite21.My.Resources.Resources.WaterMark
        Me.PictureBox.Location = New System.Drawing.Point(4, 6)
        Me.PictureBox.Name = "PictureBox"
        Me.PictureBox.Size = New System.Drawing.Size(840, 610)
        Me.PictureBox.TabIndex = 0
        Me.PictureBox.TabStop = False
        '
        'printDocSeal1
        '
        '
        'printDialog1
        '
        Me.printDialog1.AllowCurrentPage = True
        Me.printDialog1.AllowSelection = True
        Me.printDialog1.AllowSomePages = True
        Me.printDialog1.Document = Me.printDocSeal1
        Me.printDialog1.UseEXDialog = True
        '
        'frmDwg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(863, 721)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblBorder)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmDwg"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Proposal Drawing Form"
        Me.Panel1.ResumeLayout(False)
        CType(Me.imgLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region


#Region "FORM EVENT ROUTINES:"

    Private Sub frmDwg_Load(ByVal sender As Object, _
                            ByVal e As System.EventArgs) Handles Me.Load
        '=================================================================

        '....Display logo:
        With imgLogo
            .Width = 176 : .Height = .Width / gcSngLogoAspectRatio
        End With
        LoadImageLogo(imgLogo)

        'Create & initialize the local Form Object.
        '-----------------------------------------
        InitializeLocalObject()

    End Sub


#Region "HELPER ROUTINES:"

    Private Sub InitializeLocalObject()
        '=============================
        'Create & initialize the local Form Object.
        '-----------------------------------------
        If gIPE_Unit.System <> "" Then
            mIPE_Dwg = New IPE_clsDwg(gIPE_Unit.System)
        Else
            mIPE_Dwg = New IPE_clsDwg()
        End If

        '....Initialize drawing object:
        With mIPE_Dwg
            .SngLogoAspectRatio = gcSngLogoAspectRatio
            .UnitSelSystem = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.UnitSystem
            .MatSelName = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Mat.Name
            .UserInfo = gIPE_User
            If gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Type = "E-Seal" Then
                .MatSelCoating = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Mat.Coating
            End If
        End With


        '....SECONDARY ASSIGNMENTS:     
        If gIPE_Project.Analysis(gIPE_frmResults.ISel).Cavity Is Nothing = False Then mIPE_Dwg.CavitySel = gIPE_Project.Analysis(gIPE_frmResults.ISel).Cavity
        If gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal Is Nothing = False Then mIPE_Dwg.SealSel = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal
        If gIPE_File Is Nothing = False Then mIPE_Dwg.FilesSel = gIPE_File

    End Sub

#End Region

#End Region


#Region "PICTURE BOX EVENT ROUTINES:"

    Private Sub PictureBox_Paint(ByVal sender As Object, _
                                 ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox.Paint

        '===========================================================================

        Dim pGr As Graphics = e.Graphics
        'e.Graphics.DrawImage(PictureBox.Image, 0, 0)
        With mIPE_Dwg
            .Gr = pGr
            .DisplayType = "PictureBox"
            .DisplayDrawing(gIPE_frmResults.ISel, gIPE_frmOutPut.PreProductionExist, gIPE_frmOutPut.CavityExist, gIPE_Project)
        End With

    End Sub

#End Region


#Region "COMMAND BUTTON RELATED ROUTINES:"

    Private Sub cmdButtons_Click(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles cmdClose.Click, cmdPrintDwg.Click
        '======================================================================
        Dim pcmdButton As Button = CType(sender, Button)

        Select Case pcmdButton.Name

            Case "cmdClose"
                '-----------------
                gIPE_frmResults.FromFrmDrawing = True
                Me.Hide()

            Case "cmdPrintDwg"
                '-------------
                With printDialog1
                    If .ShowDialog = DialogResult.OK Then
                        .Document.DefaultPageSettings.Landscape = True
                        .Document.Print()
                    End If
                End With


                'With printDocSeal1
                '    .DefaultPageSettings.Landscape = True
                '    .PrinterSettings.PrinterName = "Adobe PDF"
                '    .Print()
                'End With

        End Select

    End Sub

#Region "HELPER ROUTINES:"

    Private Sub printDocSeal1_PrintPage(ByVal sender As Object, _
                                        ByVal e As System.Drawing.Printing.PrintPageEventArgs) _
                                        Handles printDocSeal1.PrintPage
        '=======================================================================================
        Dim pGr As Graphics = e.Graphics
        e.Graphics.DrawImage(PictureBox.Image, 0, 0)   'AES 07APR16
        With mIPE_Dwg
            .Gr = pGr
            .DisplayType = "Printer"
            .DisplayDrawing(gIPE_frmResults.ISel, gIPE_frmOutPut.PreProductionExist, gIPE_frmOutPut.CavityExist, gIPE_Project)
        End With

    End Sub

#End Region

#End Region


#Region "UTILITY ROUTINES:"

    'Private Sub cmdCreatePptReport_Click(sender As System.Object, e As System.EventArgs)

    '    '===============================================================================
    '    Dim pPicForm As New frmPicBox()
    '    pPicForm.ShowDialog()

    '    Dim pPicForm_Seal As New frmPicBox_Seal()
    '    pPicForm_Seal.ShowDialog()

    '    CratePowerPoint_Report()
    'End Sub




#End Region


End Class