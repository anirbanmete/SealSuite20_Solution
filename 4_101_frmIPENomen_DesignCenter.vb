
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmNomenclature_DesignCenter           '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB16                                '
'                                                                              '
'===============================================================================
Imports SealIPELib = SealIPELib101
Imports System.IO

Public Class IPE_frmNomenclature_DesignCenter


    Private mblnFormClose As Boolean = False

    Public ReadOnly Property FormClose()
        '================================
        Get
            Return mblnFormClose
        End Get

    End Property


    Private Sub frmNomenclature_DesignCenter_Load(ByVal sender As System.Object, _
                                                  ByVal e As System.EventArgs) _
                                                  Handles MyBase.Load
        '==============================================================================

        Dim pimgPath As String = ""

        If gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "E-Seal" Then
            '-------------------------
            If CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).POrient = "External" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++

                If CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).TemplateNo = "1Gen" Then
                    Me.Text = "E-Seal: 1Gen - External "
                    pimgPath = gIPE_File.ESeal_1Gen_Ext_ImgName

                ElseIf CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).TemplateNo = "1GenS" Then
                    Me.Text = "E-Seal: 1GenS - External "
                    pimgPath = gIPE_File.ESeal_1GenS_Ext_ImgName
                End If

            ElseIf CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).POrient = "Internal" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++
                If CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).TemplateNo = "1Gen" Then
                    Me.Text = "E-Seal: 1Gen - Internal "
                    pimgPath = gIPE_File.ESeal_1Gen_Int_ImgName

                ElseIf CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).TemplateNo = "1GenS" Then
                    Me.Text = "E-Seal: 1GenS - Internal "
                    pimgPath = gIPE_File.ESeal_1GenS_Int_ImgName
                End If
            End If

        ElseIf gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal.Type = "U-Seal" Then
            '-----------------------------
            If CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).POrient = "External" Then
                '++++++++++++++++++++++++++++++++++++++++++++++
                Me.Text = "U-Seal: External "
                pimgPath = gIPE_File.USeal_Ext_ImgName

            ElseIf CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsUSeal).POrient = "Internal" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++
                Me.Text = "U-Seal: Internal "
                pimgPath = gIPE_File.USeal_Int_ImgName
            End If

        End If
        If pimgPath <> "" Then

            LoadImageNomanclature(imgControl, pimgPath)
        End If

    End Sub

    Private Sub frmNomenclature_DesignCenter_FormClosed(ByVal sender As System.Object, _
                                                        ByVal e As System.Windows.Forms.FormClosedEventArgs) _
                                                        Handles MyBase.FormClosed
        '=====================================================================================================

        mblnFormClose = True

    End Sub


End Class