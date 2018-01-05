'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  frmPicBox                              '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07APR16                                '
'                                                                              '
'===============================================================================

Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Image
Imports System.IO
Imports SealIPELib = SealIPELib101

Public Class IPE_frmPicBox_Cavity

#Region "MEMBER VARIABLES:"

    Private mDwg As IPE_clsDwg
    Private mGr As Graphics
    Private mDpX As Single

#End Region

    Private Sub frmPicBox_Load(sender As System.Object, e As System.EventArgs) _
                               Handles MyBase.Load
        '=======================================================================

        'Create & initialize the local Form Object.
        '-----------------------------------------
        InitializeLocalObject()

    End Sub


    Private Sub InitializeLocalObject()
        '=============================
        'Create & initialize the local Form Object.
        '-----------------------------------------
        If gIPE_Unit.System <> "" Then
            mDwg = New IPE_clsDwg(gIPE_Unit.System)
        Else
            mDwg = New IPE_clsDwg()
        End If

        '....Initialize drawing object:
        With mDwg
            .SngLogoAspectRatio = gcSngLogoAspectRatio
            .UnitSelSystem = gIPE_Project.UnitSystem()
            .MatSelName = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Mat.Name
            .UserInfo = gIPE_User
            If gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Type = "E-Seal" Then
                .MatSelCoating = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Mat.Coating
            End If
        End With

        '....SECONDARY ASSIGNMENTS:     
        If gIPE_Project.Analysis(gIPE_frmResults.ISel).Cavity Is Nothing = False Then mDwg.CavitySel = gIPE_Project.Analysis(gIPE_frmResults.ISel).Cavity
        If gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal Is Nothing = False Then mDwg.SealSel = gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal
        If gIPE_File Is Nothing = False Then mDwg.FilesSel = gIPE_File

    End Sub


    Private Sub PictureBox1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) _
                                  Handles PictureBox1.Paint
        '=============================================================================================

        Try

            Dim pGr As Graphics = e.Graphics

            With mDwg
                .Gr = pGr
                .DisplayType = "PictureBox"
            End With
            DisplayCavityDrawing()

            Me.Hide()

        Catch ex As Exception

        End Try

    End Sub


    Private Sub DisplayCavityDrawing()
        '==============================

        Dim pCornerName As IPE_clsDwg.eBlockCornerName

        'Border lines.
        '-------------
        Dim pBorderTopL As PointF, pborderBotR As PointF


        'CAVITY.
        '-------
        Dim cavityBotR As PointF

        Dim pCornerPoint As PointF
        pCornerName = IPE_clsDwg.eBlockCornerName.TopL

        With pCornerPoint
            .X = pBorderTopL.X + 0.8
            .Y = pBorderTopL.Y + 0.5
        End With

        mDwg.DrawCavity_PicBox(pCornerName, pCornerPoint, cavityBotR)


    End Sub


    Private Sub frmPicBox_Cavity_FormClosing(sender As System.Object,
                                             e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        '=================================================================================================================
        Try

            Dim bitmap As Bitmap = New Bitmap(PictureBox1.Width, PictureBox1.Height)
            PictureBox1.DrawToBitmap(bitmap, PictureBox1.ClientRectangle)

            If (File.Exists("C:\SealSuite\SealIPE\Templates\cavity.jpg")) Then
                File.Delete("C:\SealSuite\SealIPE\Templates\cavity.jpg")
            End If
            bitmap.Save("C:\SealSuite\SealIPE\Templates\cavity.jpg", System.Drawing.Imaging.ImageFormat.Jpeg)

        Catch ex As Exception

        End Try

    End Sub
End Class