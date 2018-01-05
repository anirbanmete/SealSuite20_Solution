'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmDesignChoiceESeal                   '
'                        VERSION NO  :  9.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05FEB16                                '
'                                                                              '
'===============================================================================
'
'  Routines :   
'  ---------

Public Class IPE_frmDesignChoiceESeal
    Inherits System.Windows.Forms.Form

    Private mOptNew As Boolean

    Public ReadOnly Property OptNew() As Boolean
        '=======================================
        Get
            Return mOptNew
        End Get
    End Property

    Public Sub New()
        '============
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mOptNew = False
    End Sub

#Region "FORM EVENT ROUTINES:"

    Private Sub frmDesignChoiceESeal_Load(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles MyBase.Load
        '=================================================================

        optExistingCrossSec.Checked = Not CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).NewDesign
        optNewCrossSec.Checked = CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).NewDesign

    End Sub

#End Region

#Region "COMMAND BUTTON RELATED ROUTINES:"


    Private Sub cmdOK_Click(ByVal sender As System.Object, _
                            ByVal e As System.EventArgs) _
                            Handles cmdOK.Click
        '====================================================

        If optExistingCrossSec.Checked Then
            '------------------------------

            If CType(gIPE_Project.Analysis(gIPE_frmAnalysisSet.ICur).Seal, IPE_clsESeal).NewDesign = False Then    '....STD. CROSS-SECTION.    
                'gfrmDesignESeal.ShowDialog()
                Dim pfrmDesignESeal As New IPE_frmDesignESeal()
                pfrmDesignESeal.ShowDialog()



            Else                                                '....NEW CROSS-SECTION.

                Dim pstrTitle, pstrMsg As String
                Dim pintAttributes As Integer, pintAnswer As Int16

                pstrTitle = "WARNING MESSAGE: Existing STD CrossSections."

                pstrMsg = "The opening form will be for STD CrossSections only. " & vbCrLf & _
                          "If you still want to switch to a STD CrossSection, click OK. " & vbCrLf & _
                          "Otherwise, if you want to stay with the current NEW CrossSection, click CANCEL."

                pintAttributes = MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel
                pintAnswer = MsgBox(pstrMsg, pintAttributes, pstrTitle)

                If pintAnswer = MsgBoxResult.Ok Then
                    'gfrmDesignESeal.ShowDialog()
                    Dim pfrmDesignESeal As New IPE_frmDesignESeal()
                    pfrmDesignESeal.ShowDialog()

                ElseIf pintAnswer = MsgBoxResult.Cancel Then
                    optExistingCrossSec.Checked = False
                    'gfrmDesignESeal_New.ShowDialog()
                    Dim pfrmDesignESeal_New As New IPE_frmDesignESeal_New()
                    pfrmDesignESeal_New.ShowDialog()

                End If
            End If

        ElseIf optNewCrossSec.Checked Then
            '-----------------------------
            'gfrmDesignESeal_New.ShowDialog()
            Dim pfrmDesignESeal_New As New IPE_frmDesignESeal_New()
            pfrmDesignESeal_New.ShowDialog()

        End If


        Me.Close()

    End Sub


    Private Sub cmdCancel_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) _
                                Handles cmdCancel.Click
        '=========================================================

        Me.Close()
    End Sub


#End Region



End Class