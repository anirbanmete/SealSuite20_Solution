
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsESeal_NewDesign                     '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  05FEB16                                '
'                                                                              '
'===============================================================================

Imports System.Data.OleDb
Imports System.Math
Imports System.IO

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports clsLibrary11


Partial Public Class IPE_clsESeal
    Inherits IPE_clsSeal
    Implements System.IComparable


#Region "UTILIRY ROUTINES:"

    Private Sub WriteFile_NewDesign(ByVal Files_In As IPE_clsFile, _
                                   ByVal Project_In As IPE_clsProject, _
                                   ByVal UserInfo_In As IPE_clsUser, _
                                   ByVal Unit_In As IPE_clsUnit, _
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
                                                              Space(3) & mCrossSecNoOrg & "  (Original)") 'SG 14SEP
                '.WriteLine(Space(3) & "CrossSecNo.      = " & mCrossSecNoOrg & "  (Original)")
                .WriteLine(Space(3) & "TemplateNo.      = " & mTemplateNo)   '1Gen")
                .WriteLine(Space(3) & "POrient          = " & POrient)
                .WriteLine(Space(3) & "Corner Radius    = " & Cavity_In.CornerRad)  'SG 14SEP11
                .WriteLine(Space(3) & "Free Height      = " & Unit_In.WriteInUserL(mHfree))
                .WriteLine(Space(3) & "Seal Width       = " & Unit_In.WriteInUserL(mWid))
                .WriteLine()

                .WriteLine(Space(3) & "No. of Conv.     = " & mNConv)
                .WriteLine(Space(3) & "Thickness        = " & Unit_In.WriteInUserL(mT), "TFormat")
                .WriteLine(Space(3) & "HConv            = " & Unit_In.WriteInUserL(mHConv))
                .WriteLine(Space(3) & "HLeg             = " & Unit_In.WriteInUserL(mHLeg))
                .WriteLine()

                'Redius and Angles:
                '------------------
                .WriteLine(Space(3) & "End Convolution:")
                .WriteLine(Space(6) & "RadE1         = " & Unit_In.WriteInUserL(mRadE(1)))
                .WriteLine(Space(6) & "RadE2         = " & Unit_In.WriteInUserL(mRadE(2)))
                .WriteLine(Space(6) & "RadE3         = " & Unit_In.WriteInUserL(mRadE(3)))
                .WriteLine()

                .WriteLine(Space(6) & "ThetaE1       = " & mThetaE(1).ToString("##0.00"))
                .WriteLine(Space(6) & "ThetaE2       = " & mThetaE(2).ToString("##0.00"))
                .WriteLine(Space(6) & "ThetaE3       = " & mThetaE(3).ToString("##0.00"))
                .WriteLine()

                .WriteLine(Space(3) & "Mid Convolution:")
                .WriteLine(Space(6) & "RadM1         = " & Unit_In.WriteInUserL(mRadE(1)))


                If Abs(mRadM(2) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then
                    '--------------------------------------------------
                    .WriteLine(Space(6) & "RadM2         = " & "Straight")
                Else
                    .WriteLine(Space(6) & "RadM2         = " & Unit_In.WriteInUserL(mRadM(2)))
                End If


                .WriteLine(Space(6) & "RadM3         = " & Unit_In.WriteInUserL(mRadM(3)))


                If Abs(mRadM(4) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then
                    '--------------------------------------------------
                    .WriteLine(Space(6) & "RadM4         = " & "Straight")
                Else
                    .WriteLine(Space(6) & "RadM4         = " & Unit_In.WriteInUserL(mRadM(4)))

                End If

                .WriteLine(Space(6) & "RadM5         = " & Unit_In.WriteInUserL(mRadM(5)))
                .WriteLine()

                .WriteLine(Space(6) & "ThetaM1       = " & mThetaM(1).ToString("##0.00"))
                .WriteLine(Space(6) & "ThetaM2       = " & mThetaM(2).ToString("##0.00"))
                .WriteLine(Space(6) & "ThetaM3       = " & mThetaM(3).ToString("##0.00"))
                .WriteLine(Space(6) & "ThetaM4       = " & mThetaM(4).ToString("##0.00"))
                .WriteLine(Space(6) & "ThetaM5       = " & mThetaM(5).ToString("##0.00"))

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
            CompareVar(.NConv, MOD_In.NConv, pRetValue)
            CompareVar(.T, MOD_In.T, pRetValue)

            CompareVar(.HConv, MOD_In.HConv, pRetValue, 0.0001F)
            CompareVar(.HLeg, MOD_In.HLeg, pRetValue, 0.0001F)

            Dim i As Int16
            For i = 1 To 3
                CompareVar(.RadE(i), MOD_In.RadE(i), pRetValue)
                CompareVar(.ThetaE(i), MOD_In.Thetae(i), pRetValue, 0.005F)
            Next

            For i = 1 To 5
                CompareVar(.RadM(i), MOD_In.RadM(i), pRetValue)
                CompareVar(.ThetaM(i), MOD_In.ThetaM(i), pRetValue, 0.008F)
            Next

        End With

        Return pRetValue

    End Function

#End Region

End Class
