'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsDataValidate                        '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  01FEB16                                '
'                                                                              '
'===============================================================================
'PB 23APR11. A couple of message sentences changed. 

Imports clsLibrary11

Public Class IPE_clsDataValidate

#Region "INPUT DATA VALIDATION ROUTINES:"

    '*******************************************************************************
    '*               INPUT DATA VALIDATION ROUTINES - BEGIN                        *
    '*******************************************************************************

    Public Sub ValidateInputParameters(ByRef blnError_In As Boolean, ByVal strName_In As String, _
                                       Optional ByVal Project_In As IPE_clsProject = Nothing, _
                                       Optional ByVal Cavity_In As IPE_clsCavity = Nothing, _
                                       Optional ByVal Seal_In As IPE_clsSeal = Nothing, _
                                       Optional ByVal SealCandidates_In As IPE_clsSealCandidates = Nothing, _
                                       Optional ByVal Mat_In As IPE_clsMaterial = Nothing, _
                                       Optional ByVal Unit_In As IPE_clsUnit = Nothing _
                                       )
        '=======================================================================================
        'Check different input values and calculated values for frmMain

        '....Check For Null Values.
        CheckForNullValues(blnError_In, strName_In, _
                           Project_In, Cavity_In, Seal_In, SealCandidates_In, _
                           Mat_In, Unit_In)

        If blnError_In = True Then Exit Sub


        '....Check For Limits.
        CheckForLimits(blnError_In, strName_In, _
                       Cavity_In, Seal_In, SealCandidates_In, _
                       Mat_In, Unit_In)

        If blnError_In = True Then Exit Sub

    End Sub


    Private Sub CheckForNullValues(ByRef blnError_Out As Boolean, ByVal strName_In As String, _
                                   Optional ByVal Project_In As IPE_clsProject = Nothing, _
                                   Optional ByVal Cavity_In As IPE_clsCavity = Nothing, _
                                   Optional ByVal Seal_In As IPE_clsSeal = Nothing, _
                                   Optional ByVal SealCandidates_In As IPE_clsSealCandidates = Nothing, _
                                   Optional ByVal Mat_In As IPE_clsMaterial = Nothing, _
                                   Optional ByVal Unit_In As IPE_clsUnit = Nothing _
                                   )
        '=============================================================================
        'This routine checks for null values in the text boxes corresponding to the
        'following input data :
        '       1. Customer Name                            (Warning message)
        '       2. Project Name                             (Error   message)
        '       3. Cavity Min. OD                           (Error   message)
        '       4. Cavity Max. ID                           (Error   message)
        '       5. Cavity Depth                             (Error   message)
        '   6 & 7. Cavity Depth Tolerances - minus & plus.  (Warning message)
        '       8. Cross Section No                         (Error   message)
        '  9 & 10. Free Height  Tolerances - minus & plus.  (Warning message)
        '
        '-------------------------------------------------------------------------

        Dim pstrPrompt_Warning As String
        Dim pintAttributes_Warning As Integer, pstrTitle_Warning As String

        Dim pstrPrompt_Error As String
        Dim pintAttributes_Error As Integer, pstrTitle_Error As String

        Dim pstrMsg As String, pintAnswer As Integer
        Dim i As Integer
        Dim pstrSign As String = ""
        Dim pstrAny As String = ""

        'WARNING MESSAGES :
        '================
        pstrPrompt_Warning = " Should not be blank." & vbCrLf & " Do you want to continue ?"
        pintAttributes_Warning = vbExclamation + vbYesNo
        pstrTitle_Warning = "Input Data Validation : Warning Message."

        'ERROR MESSAGES :
        '===============
        pstrPrompt_Error = " Should not be blank. "
        pintAttributes_Error = vbCritical + vbOKOnly
        pstrTitle_Error = "Input Data Validation : Error Message."

        Select Case strName_In

            ''Project:
            ''--------
            ''
            'Case "Project"
            '    '....Customer Name.
            '    If Project_In.Customer = "" Then
            '        pstrMsg = "Customer Name" + pstrPrompt_Warning
            '        pintAnswer = MsgBox(pstrMsg, pintAttributes_Warning, pstrTitle_Warning)

            '        If pintAnswer = vbNo Then
            '            blnError_Out = True
            '            'frmProject_In.ShowDialog()
            '            Exit Sub
            '        End If
            '    End If

            '    '....Project Title.
            '    If Project_In.Name = "" Then
            '        blnError_Out = True
            '        pstrMsg = "Project Name" + pstrPrompt_Error
            '        pintAnswer = MsgBox(pstrMsg, pintAttributes_Error, pstrTitle_Error)

            '        'frmProject_In.ShowDialog()
            '        Exit Sub
            '    End If

            'Cavity: 
            '-------
            '
            Case "Cavity"
                '....Depth Tolerances.

                For i = 1 To 2

                    If i = 1 Then
                        pstrSign = "(-)"
                    ElseIf i = 2 Then
                        pstrSign = "(+)"
                    End If


                    If Cavity_In.DepthTol(i) < gcEPS Then
                        pstrMsg = "Cavity " & pstrSign + " Depth Tolerance" + pstrPrompt_Warning
                        pintAnswer = MsgBox(pstrMsg, pintAttributes_Warning, pstrTitle_Warning)

                        If pintAnswer = vbNo Then
                            blnError_Out = True
                            'frmCavity_In.ShowDialog()
                            Exit Sub
                        End If
                    End If

                Next

                '....Diameters:

                For i = 2 To 1 Step -1

                    If i = 2 Then
                        pstrAny = "Min. OD"
                    ElseIf i = 1 Then
                        pstrAny = "Max. ID"
                    End If

                    If Cavity_In.Dia(i) < gcEPS Then
                        blnError_Out = True
                        pstrMsg = pstrAny + pstrPrompt_Error           '....Min. OD or Max. ID
                        pintAnswer = MsgBox(pstrMsg, pintAttributes_Error, pstrTitle_Error)

                        'frmCavity_In.ShowDialog()
                        Exit Sub
                    End If

                Next


                '....Depth. 
                If Cavity_In.Depth < gcEPS Then
                    blnError_Out = True
                    pstrMsg = "Cavity Depth" + pstrPrompt_Error
                    pintAnswer = MsgBox(pstrMsg, pintAttributes_Error, pstrTitle_Error)

                    'frmCavity_In.ShowDialog()
                    Exit Sub
                End If

                'Seal:
                '----
            Case "Seal"

                '....Free Height Tolerances:
                For i = 1 To 2

                    If i = 1 Then
                        pstrSign = "(-)"
                    ElseIf i = 2 Then
                        pstrSign = "(+)"
                    End If

                    If Seal_In Is Nothing = False Then

                        If Seal_In.HFreeTol(i) < gcEPS Then
                            pstrMsg = "Free Height " & pstrSign & " Tolerance " + pstrPrompt_Warning
                            pintAnswer = MsgBox(pstrMsg, pintAttributes_Warning, pstrTitle_Warning)

                            If pintAnswer = vbNo Then
                                blnError_Out = True
                                'frmDesign_In.ShowDialog()
                                Exit Sub
                            End If
                        End If

                    End If

                Next

                If Seal_In Is Nothing = True Then
                    blnError_Out = True
                    pstrMsg = "Seal Type" + pstrPrompt_Error
                    pintAnswer = MsgBox(pstrMsg, pintAttributes_Error, pstrTitle_Error)

                    'frmDesign_In.ShowDialog()
                    Exit Sub
                End If


                '....Cross Section No.
                CheckForNullCrossSectionNo(Seal_In, blnError_Out)

        End Select

    End Sub


    Public Sub CheckForNullCrossSectionNo(ByVal Seal_In As IPE_clsSeal, _
                                          ByRef pblnError_out As Boolean)
        '========================================================================
        Dim pstrPrompt As String, pintAttributes As Integer, pstrTitle As String
        Dim pintAnswer As Integer

        pstrPrompt = "Seal Cross Section No should not be blank. "
        pintAttributes = vbCritical + vbOKOnly
        pstrTitle = "Input Data Validation : Error Message."

        If Seal_In.MCrossSecNo = "" Then
            pblnError_out = True
            pintAnswer = MsgBox(pstrPrompt, pintAttributes, pstrTitle)
            'frmDesign_In.ShowDialog()
            Exit Sub
        End If

    End Sub


    Private Sub CheckForLimits(ByRef blnError_Out As Boolean, ByVal strName_In As String, _
                               Optional ByVal Cavity_In As IPE_clsCavity = Nothing, _
                               Optional ByVal Seal_In As IPE_clsSeal = Nothing, _
                               Optional ByVal SealCandidates_In As IPE_clsSealCandidates = Nothing, _
                               Optional ByVal Mat_In As IPE_clsMaterial = Nothing, _
                               Optional ByVal Unit_In As IPE_clsUnit = Nothing)
        '===================================================================================
        ''This routine checks for the following limits :
        ''   1. Pressure      - max. allow. for the chosen material @ TOper. (Error   message)
        ''   2. Temperature   - max. allow. for the chosen material.         (Error   message)
        ''   3. Seal Compression - min. allow.                               (Warning message)
        ''   4. Control Dia      - min. allow.                               (Warning message)
        ''   5. Seal   Width     - max. allow.                               (Error   message)
        ''-----------------------------------------------------------------------------------

        '' 'Unit Aware' Routine
        ''  -------------------

        'Const pcFACTOR_SAFETY_PRES As Single = 2.0#   '....Named Constant

        ''WARNING Message Strings:
        ''-----------------------
        'Dim pintAttributesWarning As Integer = vbExclamation + vbYesNo
        'Dim pstrTitleWarning As String
        'pstrTitleWarning = "SealIPE  -  Input Data Validation : Warning Message."

        ''ERROR Message Strings:
        ''----------------------
        'Dim pintAttributesError As Integer = vbCritical + vbOKOnly
        'Dim pstrTitleError As String
        'pstrTitleError = "SealIPE  -  Input Data Validation : Error Message."

        'Dim pstrPrompt As String
        'Dim pintAnswer As Integer


        ''Pressure: Upper Limit                              (Warning Message)
        ''====================

        'Select Case strName_In

        '    Case "Seal"

        '        If Seal_In.Type = "E-Seal" Then
        '            '------------------------

        '            '...Max. allowable Pressure for the chosen material @ TOper) : EDGMA Formula

        '            '....HConv pertains to the standard geometry (not the adjusted one).
        '            Dim pNumer As Single
        '            pNumer = Mat_In.SigmaY_TOper * 2 * Seal_In.T * Seal_In.T

        '            Dim pHConv As Single
        '            pHConv = CType(Seal_In, IPE_clsESeal).HConv

        '            Dim pDenom As Single
        '            pDenom = pcFACTOR_SAFETY_PRES * pHConv * pHConv

        '            Dim pPDiffLimitUp As Single
        '            pPDiffLimitUp = pNumer / pDenom


        '            If AppCond_In.PDiff > pPDiffLimitUp Then

        '                'PB 23APR11.
        '                pstrPrompt = "The pressure applied to this seal exceeds the " & _
        '                        "recommended operating pressure of " & _
        '                        NInt(pPDiffLimitUp / Unit_In.CFacUserP) & " " & Unit_In.UserP & " !!" _
        '                        & vbCrLf & vbCrLf & "You may continue with the analysis," & _
        '                        " but please consult with engineering before placing an order." _
        '                        & vbCrLf & vbCrLf & "Do you want to continue ?"

        '                pintAnswer = MsgBox(pstrPrompt, pintAttributesWarning, pstrTitleWarning)

        '                If pintAnswer = vbNo Then
        '                    blnError_Out = True
        '                    'frmAppCond_In.ShowDialog()
        '                    Exit Sub
        '                End If

        '            End If

        '        End If


        '        'Temperature: Upper Limit                           (Error Message)
        '        '------------------------
        '        If AppCond_In.TOper > Mat_In.TOperLimitUp Then

        '            blnError_Out = True
        '            'SG 16SEP09
        '            pstrPrompt = "You have exceeded the temperature limit for this " & _
        '                         "material !! " & vbCrLf & vbCrLf & _
        '                         "To continue, reduce the temperature to " & _
        '                         NInt(Mat_In.TOperLimitUp) & " " & Unit_In.T _
        '                         & ", or choose a different material." & vbCrLf & vbCrLf _
        '                         & "Consult with engineering for further assistance."

        '            MsgBox(pstrPrompt, pintAttributesError, pstrTitleError)
        '            Exit Sub

        '        End If


        '        'Control Dia: Lower Limit                           (Warning Message)
        '        '------------------------
        '        '....Note: DiMin data is not available in the CSeal database. 
        '        '........It is only available in the ESeal database.
        '        '....MinID Limit for the given seal cross section, as stored in the database.

        '        Dim pDControlLimitLow As Single

        '        If Seal_In.POrient = "External" Then
        '            pDControlLimitLow = Seal_In.DiMin

        '        ElseIf Seal_In.POrient = "Internal" Then
        '            pDControlLimitLow = Seal_In.DiMin + Seal_In.WidMax
        '        End If


        '        If Seal_In.DControl < pDControlLimitLow Then

        '            pstrPrompt = "The diameter of the seal you have selected is smaller than" & _
        '                    vbCrLf & "what the current tooling can produce e.g. " & _
        '                    Unit_In.WriteInUserL(pDControlLimitLow) & " " & _
        '                    Unit_In.UserL & " !!" & vbCrLf & vbCrLf _
        '                & "Consult with engineering to ensure that the seal can be made." & _
        '                vbCrLf & vbCrLf & "Do you want to continue ?"

        '            pintAnswer = MsgBox(pstrPrompt, pintAttributesWarning, pstrTitleWarning)

        '            If pintAnswer = vbNo Then
        '                blnError_Out = True
        '                'frmDesign_In.ShowDialog()
        '                Exit Sub
        '            End If

        '        End If


        '        'Seal Width: Upper Limit                            (Error Message)
        '        '----------------------
        '        '....Seal Width should not be > Cavity Width: Lower limit.
        '        '.......Cavity width required to accommodate the seal.
        '        '....The expression associated with the ZClear is multiplied with 0.5,
        '        '........since ZClear is diametral, after discussion with Steve (16JAN05)
        '        '
        '        Dim psngMargin As Single
        '        psngMargin = 0.5 * ((1 + SealCandidates_In.FacBuckling) * Seal_In.ZClear + Seal_In.H11Tol)

        '        Dim pWidCavityReqd As Single
        '        pWidCavityReqd = Seal_In.WidMax + psngMargin

        '        'PB 28AUG08. Needs work.
        '        If Cavity_In.WidMin < pWidCavityReqd Then
        '            blnError_Out = True

        '            'PB 23APR11.
        '            'pstrPrompt = "The radial width of the seal you have chosen is too " & _
        '            '      "big for the specified cavity !!" & vbCrLf & _
        '            '      "Min cavity width required for this seal, accounting for various " & _
        '            '      "allowances is " & Unit_In.WriteInUserL(pWidCavityReqd) & " " & _
        '            '      Unit_In.UserL & " ." & vbCrLf & vbCrLf & _
        '            '      "Please adjust the cavity dimensions, or chose a different " & _
        '            '      "seal cross section." & vbCrLf & vbCrLf & _
        '            '      "The theoretical radial width of the seal is shown in the 'Width' " & _
        '            '      "box at the bottom" & vbCrLf & "of this screen."

        '            pstrPrompt = "The radial width of the selected seal is too " & _
        '                  "big for the specified cavity !!" & vbCrLf & _
        '                  "Min cavity width required for this seal accounting for various " & _
        '                  "allowances is " & Unit_In.WriteInUserL(pWidCavityReqd) & " " & _
        '                  Unit_In.UserL & " ." & vbCrLf & vbCrLf & _
        '                  "Please adjust the cavity dimensions, or chose a different " & _
        '                  "seal cross section." & vbCrLf & vbCrLf & _
        '                  "The theoretical radial width of the seal is shown in the 'Width' " & _
        '                  "box at the bottom" & vbCrLf & "of this screen."

        '            MsgBox(pstrPrompt, pintAttributesError, pstrTitleError)

        '            'frmDesign_In.ShowDialog()
        '            Exit Sub
        '        End If


        '        'Installation Compression : Lower Limit         (Warning Message)
        '        '--------------------------------------
        '        CheckForMinInstallComp(AppCond_In.Compression.PcentValueMin, _
        '                               SealCandidates_In.CompressPcentValueMinReqd, _
        '                               blnError_Out)

        '        Exit Sub
        'End Select

    End Sub


    Private Sub CheckForMinInstallComp(ByVal AppValMin_In As Single, _
                                       ByVal CompressPcentValMin_In As Single, _
                                       ByRef blnError_Out As Boolean)
        '====================================================================================
        '....Check for the Minimum Installation Compression.

        '.......Input  :  AppCondCompressionPcentValueMin_In (%)
        '.......Output :  blnError_Out = TRUE or FALSE

        Dim pintAttributesWarning As Integer
        Dim pstrTitleWarning As String
        pintAttributesWarning = vbExclamation + vbYesNo
        pstrTitleWarning = "SealIPE  -  Input Data Validation : Warning Message."

        'SG 09SEP09
        Dim pDesignVal_Min As Single = CompressPcentValMin_In ' gIPE_SealCandidates.CompressPcentValueMinReqd

        If AppValMin_In < pDesignVal_Min Then

            '.....Compression is less than 'Minimum Recommended Design Value'.
            Dim pstrPrompt As String
            pstrPrompt = "Minimum installation compression is less than the " & _
                    "minimum design value of " & pDesignVal_Min & "%." & vbCrLf & _
                    "Check on the 'Installation Compression' pull-down menu on the " & _
                    "Applied Condition Form." & _
                    vbCrLf & "Do you want to continue ?"

            Dim pintAnswer As Integer
            pintAnswer = MsgBox(pstrPrompt, pintAttributesWarning, pstrTitleWarning)

            If pintAnswer = vbNo Then
                blnError_Out = True
                'frmAppCond_In.ShowDialog() 'SG 09SEP09 
                Exit Sub
            End If

        End If

    End Sub

    '*******************************************************************************
    '*                  INPUT DATA VALIDATION ROUTINES - END                       *
    '*******************************************************************************
#End Region

End Class
