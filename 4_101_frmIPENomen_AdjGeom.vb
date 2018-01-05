
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmNomenclature_AdjGeom                '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB16                                '
'                                                                              '
'===============================================================================
Imports SealIPELib = SealIPELib101

Public Class IPE_frmNomenclature_AdjGeom

    Private mblnFormClose As Boolean = False

    Public ReadOnly Property FormClose()
        '================================
        Get
            Return mblnFormClose
        End Get

    End Property

    Private Sub frmNomenclature_AdjGeom_Load(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles MyBase.Load
        '=======================================================================

        Dim pimgPath As String = ""

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            '-------------------------
            If CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).POrient = "External" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++
                Me.Text = "E-Seal: External"
                pimgPath = gIPE_File.ESeal_Ext_ImgName


            ElseIf CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).POrient = "Internal" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++
                Me.Text = "E-Seal: Internal"
                pimgPath = gIPE_File.ESeal_Int_ImgName
            End If
        End If

        If pimgPath <> "" Then
            LoadImageNomanclature(imgControl, pimgPath)
        End If

    End Sub



    Private Sub frmNomenclature_AdjGeom_FormClosed(ByVal sender As System.Object, _
                                                   ByVal e As System.Windows.Forms.FormClosedEventArgs) _
                                                   Handles MyBase.FormClosed
        '=============================================================================

        mblnFormClose = True
    End Sub
End Class