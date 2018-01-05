'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      FORM MODULE   :  frmCustomResults                       '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  17MAR17                                '
'                                                                              '
'===============================================================================

Imports System.Windows.Forms
Imports System.IO
Imports SealIPELib = SealIPELib101

Public Class IPE_frmCustomResults

#Region "MEMBER VARIABLE DECLARATION:"
    '===================================


#End Region


#Region "FORM EVENT ROUTINES:"
    '===========================

    Private Sub frmCustomResults_Load(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) Handles MyBase.Load
        '==============================================================================

        DisplayData()

    End Sub


    Private Sub DisplayData()
        '====================

        With My.Settings
            If gIPE_Project.Analysis(gIPE_frmResults.ISel).Seal.Type = "E-Seal" Then

                '....Analysis
                chkMCS.Checked = .MCS
                chkHFree.Checked = .HFree
                chkDateCreated.Checked = .DateCreated
                chkTimeCreated.Checked = .TimeCreated

                '....Cavity
                chkCavityOD.Checked = .CavityOD
                chkCavityID.Checked = .CavityID
                chkCavityDepth.Checked = .CavityDepth

                '....OpCond
                chkPress.Checked = .Press
                chkTOper.Checked = .TOper
                chkPOrient.Checked = .POrient

                '....Applied Loading
                chkPreCompressed.Checked = .PreCompressed
                chkPreCompressedH.Checked = .PreCompressedH
                chkRadCon.Checked = .RadCon

                '....Load Case
                chkLoadCaseType.Checked = .LoadCaseType
                chkCompTolType.Checked = .CompressTolType
                chkName.Checked = .Name
                chkMatModel.Checked = .MatModel     'AES 17MAR17


                '....Seal Design Parameters
                chkSeg.Checked = .Seg
                chkSegCount.Checked = .SegCount
                chkMatName.Checked = .MatName
                chkHT.Checked = .HT
                chkCoating.Checked = .Coating
                If gIPE_Project.SealType = "E-Seal" Then
                    chkSFinish.Enabled = True
                    chkSFinish.Checked = .SFinish
                Else
                    chkSFinish.Enabled = False
                    chkSFinish.Checked = False
                End If

                If gIPE_Project.SealType = "C-Seal" Then
                    chkPlating.Enabled = True
                    chkPlating.Checked = .Plating
                Else
                    chkPlating.Enabled = False
                    chkPlating.Checked = False
                End If


                chkZClear.Checked = .ZClear

                chkAdjusted.Checked = .Adjusted

                If (.Adjusted) Then
                    '....Adjusted Parameters
                    '........E-Seal
                    If gIPE_Project.SealType = "E-Seal" Then
                        grpESeal.Enabled = True
                        chkDThetaE1.Checked = .DThetaE1
                        chkDThetaM1.Checked = .DThetaM1
                    Else
                        grpESeal.Enabled = False
                        chkDThetaE1.Checked = False
                        chkDThetaM1.Checked = False
                    End If


                    '........C-Seal
                    If gIPE_Project.SealType = "C-Seal" Then
                        grpCSeal.Enabled = True
                        chkDHFree.Checked = .DHFree
                        chkDThetaOpening.Checked = .DThetaOpening
                        chkDT_CSeal.Checked = .DT_CSeal
                    Else
                        grpCSeal.Enabled = False
                        chkDHFree.Checked = False
                        chkDThetaOpening.Checked = False
                        chkDT_CSeal.Checked = False
                    End If

                    '........U-Seal
                    If gIPE_Project.SealType = "U-Seal" Then
                        grpUSeal.Enabled = True
                        chkDTheta1.Checked = .DTheta1
                        chkDTheta2.Checked = .DTheta2
                        chkDRad1.Checked = .DRad1
                        chkDRad2.Checked = .DRad2
                        chkDLLeg.Checked = .DLLeg
                        chkDT_USeal.Checked = .DT_USeal
                    Else
                        grpUSeal.Enabled = False
                        chkDTheta1.Checked = False
                        chkDTheta2.Checked = False
                        chkDRad1.Checked = False
                        chkDRad2.Checked = False
                        chkDLLeg.Checked = False
                        chkDT_USeal.Checked = False
                    End If

                Else
                    grpESeal.Enabled = False
                    chkDThetaE1.Checked = False
                    chkDThetaM1.Checked = False

                    grpCSeal.Enabled = False
                    chkDHFree.Checked = False
                    chkDThetaOpening.Checked = False
                    chkDT_CSeal.Checked = False

                    grpUSeal.Enabled = False
                    chkDTheta1.Checked = False
                    chkDTheta2.Checked = False
                    chkDRad1.Checked = False
                    chkDRad2.Checked = False
                    chkDLLeg.Checked = False
                    chkDT_USeal.Checked = False
                End If


                '....Results
                'chkFContact.Checked = .FContact
                'chkSigEMax.Checked = .SigEMax
                chkSpringBack.Checked = .SpringBack
                chkLeakage_BL.Checked = .Leakage_BL

            End If

        End With

    End Sub

#End Region


#Region "COMMAND BUTTON EVENT ROUTINES:"
    '=====================================

    Private Sub cmdClose_Click(ByVal sender As System.Object, _
                               ByVal e As System.EventArgs) _
                               Handles cmdOK.Click, cmdCancel.Click
        '===========================================================
        Dim pCmdBtn As Button = CType(sender, Button)

        If pCmdBtn.Name = "cmdOK" Then
            SaveData()
        End If

        Me.Close()

    End Sub


    Private Sub SaveData()
        '=================

        With My.Settings

            '....Analysis
            .MCS = chkMCS.Checked
            .HFree = chkHFree.Checked
            .DateCreated = chkDateCreated.Checked
            .TimeCreated = chkTimeCreated.Checked

            '....Cavity
            .CavityOD = chkCavityOD.Checked
            .CavityID = chkCavityID.Checked
            .CavityDepth = chkCavityDepth.Checked

            '....OpCond
            .Press = chkPress.Checked
            .TOper = chkTOper.Checked
            .POrient = chkPOrient.Checked

            '....Applied Loading
            .PreCompressed = chkPreCompressed.Checked
            .PreCompressedH = chkPreCompressedH.Checked
            .RadCon = chkRadCon.Checked

            '....LoadCase
            .LoadCaseType = chkLoadCaseType.Checked
            .CompressTolType = chkCompTolType.Checked
            .Name = chkName.Checked
            .MatModel = chkMatModel.Checked     'AES 17MAR17

            '....Seal Design Parameters
            .Seg = chkSeg.Checked
            .SegCount = chkSegCount.Checked
            .MatName = chkMatName.Checked
            .HT = chkHT.Checked
            .Coating = chkCoating.Checked
            .SFinish = chkSFinish.Checked
            .Plating = chkPlating.Checked
            .ZClear = chkZClear.Checked
            .Adjusted = chkAdjusted.Checked

            '....Adjusted Parameters
            '........E-Seal
            .DThetaE1 = chkDThetaE1.Checked
            .DThetaM1 = chkDThetaM1.Checked

            '........C-Seal
            .DHFree = chkDHFree.Checked
            .DThetaOpening = chkDThetaOpening.Checked
            .DT_CSeal = chkDT_CSeal.Checked

            '........U-Seal
            .DTheta1 = chkDTheta1.Checked
            .DTheta2 = chkDTheta2.Checked
            .DRad1 = chkDRad1.Checked
            .DRad2 = chkDRad2.Checked
            .DLLeg = chkDLLeg.Checked
            .DT_USeal = chkDT_USeal.Checked

            '....Results
            '.FContact = chkFContact.Checked
            '.SigEMax = chkSigEMax.Checked
            .SpringBack = chkSpringBack.Checked
            .Leakage_BL = chkLeakage_BL.Checked
            .UnitSeatLoad = chkUnitSeatingLoad.Checked
            .Save()
        End With

    End Sub

#End Region

    Private Sub chkAdjusted_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAdjusted.CheckedChanged
        '====================================================================================================================
        If (chkAdjusted.Checked) Then
            '....Adjusted Parameters
            '........E-Seal
            If gIPE_Project.SealType = "E-Seal" Then
                grpESeal.Enabled = True
                chkDThetaE1.Checked = True
                chkDThetaM1.Checked = True
            Else
                grpESeal.Enabled = False
                chkDThetaE1.Checked = False
                chkDThetaM1.Checked = False
            End If

            '........C-Seal
            If gIPE_Project.SealType = "C-Seal" Then
                grpCSeal.Enabled = True
                chkDHFree.Checked = True
                chkDThetaOpening.Checked = True
                chkDT_CSeal.Checked = True
            Else
                grpCSeal.Enabled = False
                chkDHFree.Checked = False
                chkDThetaOpening.Checked = False
                chkDT_CSeal.Checked = False
            End If

            '........U-Seal
            If gIPE_Project.SealType = "U-Seal" Then
                grpUSeal.Enabled = True
                chkDTheta1.Checked = True
                chkDTheta2.Checked = True
                chkDRad1.Checked = True
                chkDRad2.Checked = True
                chkDLLeg.Checked = True
                chkDT_USeal.Checked = True
            Else
                grpUSeal.Enabled = False
                chkDTheta1.Checked = False
                chkDTheta2.Checked = False
                chkDRad1.Checked = False
                chkDRad2.Checked = False
                chkDLLeg.Checked = False
                chkDT_USeal.Checked = False
            End If

        Else
            grpESeal.Enabled = False
            chkDThetaE1.Checked = False
            chkDThetaM1.Checked = False

            grpCSeal.Enabled = False
            chkDHFree.Checked = False
            chkDThetaOpening.Checked = False
            chkDT_CSeal.Checked = False

            grpUSeal.Enabled = False
            chkDTheta1.Checked = False
            chkDTheta2.Checked = False
            chkDRad1.Checked = False
            chkDRad2.Checked = False
            chkDLLeg.Checked = False
            chkDT_USeal.Checked = False
        End If
    End Sub
End Class