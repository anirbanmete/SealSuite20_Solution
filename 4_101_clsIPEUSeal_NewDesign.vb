
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsUSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05FEB16                                '
'                                                                              '
'===============================================================================
'

Imports System.Math
Imports clsLibrary11
Imports System.IO
Imports System.Windows.Forms

Partial Public Class IPE_clsUSeal
    Inherits IPE_clsSeal
    Implements IComparable

#Region "CLASS METHODS"
    '===================

#Region "NEW USEAL DESIGN FILE - WRITE:"
    '----------------------------------
    Private Sub WriteFile_NewDesign(ByVal Files_In As IPE_clsFile, _
                                   ByVal Project_In As IPE_clsProject, _
                                   ByVal UserInfo_In As IPE_clsUser, _
                                   ByVal Unit_In As IPE_clsUnit,
                                   ByVal Cavity_In As IPE_clsCavity)
        '=================================================================

        Dim pNewFileName As String = Files_In.DirIn & MCrossSecNo & ".txt"
        If File.Exists(pNewFileName) Then File.Delete(pNewFileName)

        Dim pSW As StreamWriter = Nothing

        Try
            pSW = File.CreateText(pNewFileName)

            With pSW


                .WriteLine("File Name: " & Trim(pNewFileName))
                .WriteLine("==========")
                .WriteLine()

                'Header Information:
                '-------------------
                .WriteLine("Header:")
                .WriteLine(Space(3) & "UserName         = " & UserInfo_In.Name)
                .WriteLine(Space(3) & "PhoneNo          = " & UserInfo_In.PhoneNo)
                .WriteLine(Space(3) & "Date             = " & _
                                                       Format(Today, "dd-MMM-yy"))
                .WriteLine(Space(3) & "Project Customer = " & Project_In.Customer())
                ''.WriteLine(Space(3) & "Project Name     = " & Project_In.Name)
                .WriteLine(Space(3) & "Unit System      = " & Unit_In.System)
                .WriteLine(Space(3) & "Length User Unit = " & Unit_In.UserL)
                .WriteLine()

                'ESeal Design:
                '-------------                              
                .WriteLine("Seal Design Details:")
                .WriteLine(Space(3) & "Type             = " & Type)
                .WriteLine(Space(3) & "CrossSecNo.      = " & MCrossSecNo & " (New)" & _
                                                             Space(3) & mCrossSecNoOrg & "  (Original)")
                '.WriteLine(Space(3) & "CrossSecNo.      = " & mCrossSecNoOrg & "  (Original)")
                .WriteLine(Space(3) & "POrient          = " & POrient)
                .WriteLine(Space(3) & "Corner Radius    = " & Cavity_In.CornerRad)         'SG 14SEP11
                .WriteLine(Space(3) & "Free Height      = " & Unit_In.WriteInUserL(mHfree))
                .WriteLine(Space(3) & "Seal Width       = " & Unit_In.WriteInUserL(mWid))
                .WriteLine(Space(3) & "Thickness        = " & Unit_In.WriteInUserL(mT), "TFormat")
                .WriteLine()

                'Redius and Angles:
                '------------------
                .WriteLine(Space(6) & "LLeg         = " & Unit_In.WriteInUserL(mLLeg))

                .WriteLine(Space(6) & "Rad1         = " & Unit_In.WriteInUserL(mR(1)))
                .WriteLine(Space(6) & "Rad2         = " & Unit_In.WriteInUserL(mR(2)))

                .WriteLine(Space(6) & "Theta1       = " & mTheta(1).ToString("##0.00"))
                .WriteLine(Space(6) & "Theta2       = " & mTheta(2).ToString("##0.00"))
            End With

        Catch pEXP As IOException
            '....ERROR HANDLER            
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
                                                                 MessageBoxIcon.Error)
            Exit Sub

        Finally
            pSW.Close()
            pSW = Nothing
        End Try

    End Sub
#End Region

#Region "IComparable Interface:"

    Public Function Compare(ByVal MOD_In As Object) As Integer _
                            Implements System.IComparable.CompareTo
        '========================================================        
        '....This subroutine compares two ESeal objects members.

        '....The following counter keep tracks of the # of member variables  
        '........that differ in values.
        Dim pRetValue As Integer = 0

        With Me
            CompareVar(.T, MOD_In.T, pRetValue)
            Dim pLLeg As Single = Round(.LLeg, 3)
            Dim pLLeg_Mod As Single = Round(MOD_In.LLeg, 3)
            CompareVar(pLLeg, pLLeg_Mod, pRetValue)
            'CompareVar(pLLeg, MOD_In.LLeg, pRetValue)

            Dim i As Int16
            For i = 1 To 2
                Dim pTheta As Single = Round(.Theta(i), 1)
                Dim pTheta_Mod As Single = Round(MOD_In.Theta(i), 1)
                CompareVar(pTheta, pTheta_Mod, pRetValue)
                'CompareVar(pTheta, MOD_In.Theta(i), pRetValue)

                Dim pR As Single = Round(.R(i), 3)
                Dim pR_Mod As Single = Round(MOD_In.R(i), 3)
                CompareVar(pR, pR_Mod, pRetValue)
                'CompareVar(pR, MOD_In.R(i), pRetValue)
            Next

        End With

        Return pRetValue

    End Function

#End Region

#End Region

End Class
