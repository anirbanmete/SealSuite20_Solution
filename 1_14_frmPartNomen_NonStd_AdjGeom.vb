
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmNomenclature_AdjGeom                '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB16                                '
'                                                                              '
'===============================================================================


Public Class Part_frmNomenclature_NonStd_AdjGeom

    Private mblnFormClose As Boolean = False
    Private mPartProject As New clsPartProject


    Public Sub New(ByRef PartProject_In As clsPartProject)
        '================================
        ' This call is required by the designer.
        InitializeComponent()

        'mHW = HW_In.Clone()
        mPartProject = PartProject_In.Clone()
    End Sub

    Public ReadOnly Property FormClose()
        '================================
        Get
            Return mblnFormClose
        End Get

    End Property


    Private Sub frmNomenclature_NonStd_AdjGeom_Load(sender As System.Object,
                                                    e As System.EventArgs) Handles MyBase.Load
        '======================================================================================

        Dim pimgPath As String = ""

        If mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E Then
            '-------------------------
            If mPartProject.PNR.HW.POrient = "External" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++
                Me.Text = "E-Seal: External"
                pimgPath = gIPE_File.ESeal_Ext_ImgName


            ElseIf mPartProject.PNR.HW.POrient = "Internal" Then
                '++++++++++++++++++++++++++++++++++++++++++++++++++
                Me.Text = "E-Seal: Internal"
                pimgPath = gIPE_File.ESeal_Int_ImgName
            End If
        End If

        If pimgPath <> "" Then
            LoadImageNomanclature(imgControl, pimgPath)
        End If
    End Sub


    Private Sub frmNomenclature_NonStd_AdjGeom_FormClosed(sender As Object,
                                                          e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '======================================================================================================================
        mblnFormClose = True
    End Sub
End Class