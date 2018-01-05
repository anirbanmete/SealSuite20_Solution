'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsUSeal_DB                            '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  08MAY17                                '
'                                                                              '
'===============================================================================

Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Collections.Specialized
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Threading
Imports System.Globalization
Imports System.Linq
Imports clsLibrary11
Imports System.Windows.Forms


Partial Public Class IPE_clsUSeal
    Inherits IPE_clsSeal


#Region "MEMBER VARIABLE DECLARATIONS:"
    '==================================

    '   ........Hfree Adjustment Percentages:
    Private mHfreeAdjMinPCent As Single                                 '(RETRIEVED)  
    Private mHfreeAdjMaxPCent As Single                                 '(RETRIEVED) 

#End Region
    '

#Region "CLASS METHODS:"
    '===================

#Region "DATABASE READING"

    Private Sub RetrieveDBParams()
        '=========================
        '....Retrieve Standard ESeal parameters from the database.
        RetrieveGeomParams(MCrossSecNo)          '....Geomteric     Parameters.      

        '....Initialize some of the "DERIVED" member variable set, which are dependent 
        '........on the geometry parameters, retrieved above.
        Calc_MemberVariables("STD")

        RetrieveManfParams(MCrossSecNo)          '....Manufacturing Parameters.
        RetrieveFEAParams(MCrossSecNo)           '....FEA           Parameters.

    End Sub


    Private Sub RetrieveNewDBParams()
        '============================
        '....Retrieve New ESeal DEsign parameters from the new database.
        RetrieveGeomParams(MCrossSecNo)              '....Geomteric     Parameters.

        '....Initialize some of the "DERIVED" member variable set, which are dependent on the 
        '........geometry parameters, retrieved above.
        Calc_MemberVariables("STD")

        RetrieveManfParams(mCrossSecNoOrg)          '....Manufacturing Parameters.
        mWidMax = mWidStd * mUnit.CFacConL

        RetrieveFEAParams(mCrossSecNoOrg)           '....FEA           Parameters.

    End Sub


    Private Sub RetrieveGeomParams(ByVal CrossSecNo_In As String)
        '========================================================
        'This routine retrieves the geometrical parameters from the "USeal" database,
        '....based on the selected Cross-Sec No.

        '....Unit Conversion:
        'The data stored in the database are in the English consistent unit.
        '....The unit conversion factor affects the retrieved data, only if
        '........unitSys = "Metric".
        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pSealNewDBEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        Try

            If mNewDesign = False Then
                '----------------------
                Dim pRecord = (From pRec In pSealEntities.tblUSeal_Geom
                          Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

                mTStd = pRecord(0).fldThick * mUnit.CFacConL    '....Thickness
                mLLegStd = pRecord(0).fldLLeg * mUnit.CFacConL  '....Leg Length

                mRStd(1) = pRecord(0).fldR1 * mUnit.CFacConL    '....Conv. Radius
                mThetaStd(1) = pRecord(0).fldTheta1             '....Conv. Arc

                mRStd(2) = pRecord(0).fldR2 * mUnit.CFacConL    '....Sealing Surface Radius
                mThetaStd(2) = pRecord(0).fldTheta2             '....Sealing Surface Arc

                '....Cavity Corner Radius
                If IsNothing(pRecord(0).fldCavityCornerRad) = True Then
                    mCavityCornerRad = mcCavityCornerRad_Def '0.0#  'AES 07APR16
                Else
                    mCavityCornerRad = pRecord(0).fldCavityCornerRad * mUnit.CFacConL
                End If

                '....Initialize the adjusted / modified values to the standard values.
                mT = mTStd
                mLLeg = mLLegStd

                mR(1) = mRStd(1)
                mTheta(1) = mThetaStd(1)

                mR(2) = mRStd(2)
                mTheta(2) = mThetaStd(2)


            ElseIf mNewDesign = True Then
                '------------------------
                Dim pRecord As Object = (From pRec In pSealNewDBEntities.tblUSealNew_Geom
                                Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

                '   NEW DESIGN:
                '   -----------
                '
                mT = pRecord(0).fldThick * mUnit.CFacConL    '....Thickness
                mLLeg = pRecord(0).fldLLeg * mUnit.CFacConL  '....Leg Length

                mR(1) = pRecord(0).fldR1 * mUnit.CFacConL    '....Conv. Radius
                mTheta(1) = pRecord(0).fldTheta1             '....Conv. Arc

                mR(2) = pRecord(0).fldR2 * mUnit.CFacConL    '....Sealing Surface Radius
                mTheta(2) = pRecord(0).fldTheta2             '....Sealing Surface Arc

                '....Cavity Corner Radius                            
                If IsNothing(pRecord(0).fldCavityCornerRad) = True Then
                    mCavityCornerRad = 0.0#
                Else
                    mCavityCornerRad = pRecord(0).fldCavityCornerRad * mUnit.CFacConL
                End If


                '....ORIGINAL CrossSecNo (Std):
                mCrossSecNoOrg = pRecord(0).fldCrossSecNoOrg

                pRecord = (From pRec In pSealEntities.tblUSeal_Geom
                         Where pRec.fldCrossSecNo = mCrossSecNoOrg Select pRec).ToList()

                If (pRecord.Count() > 0) Then
                    mTStd = pRecord(0).fldThick * mUnit.CFacConL    '....Thickness
                    mLLegStd = pRecord(0).fldLLeg * mUnit.CFacConL  '....Leg Length

                    mRStd(1) = pRecord(0).fldR1 * mUnit.CFacConL    '....Conv. Radius
                    mThetaStd(1) = pRecord(0).fldTheta1             '....Conv. Arc

                    mRStd(2) = pRecord(0).fldR2 * mUnit.CFacConL    '....Sealing Surface Radius
                    mThetaStd(2) = pRecord(0).fldTheta2             '....Sealing Surface Arc
                End If

            End If

            Dim pBeta1 As Single
            pBeta1 = 90 - (0.5 * mTheta(1))
            If (mTheta(2) - pBeta1) < gcEPS Then
                Dim pstrMsg As String
                pstrMsg = "The value of Theta2 (" & Format(mTheta(2), "#0.0") & ") " & _
                          "in the database table 'tblUSeal_Geom' makes the edge horizontal, " & vbLf & _
                          "which is not supported in the existing FE model." & vbLf & _
                          "Please increase Theta2 by at least 1 degree."

                MessageBox.Show(pstrMsg, "Input Data Validation : Warning", _
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If


        Catch pEXP As Exception
            MsgBox(pEXP.Message)
            MessageBox.Show("No Record Found.", "ERROR to retrieve U-Seal Geom. Data", _
                           MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub RetrieveManfParams(ByVal CrossSecNo_In As String)
        '========================================================  
        'Unit Conversion:
        '----------------
        'The data stored in the database are in the English consistent unit.
        '....The unit conversion factor affects the retrieved data, only if
        '........unitSys = "Metric".

        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        Dim pRecord = (From pRec In pSealEntities.tblUSeal_ManfData
                           Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

        Try

            'Retrieve Manufacturing Parameters:
            '---------------------------------
            '....Build Tolerance.
            Dim psngBuildTol As Single
            If IsNothing(pRecord(0).fldBuildTol) = True Then
                psngBuildTol = 0.0#

            Else
                psngBuildTol = pRecord(0).fldBuildTol * mUnit.CFacConL
            End If

            '....Assign Build tolerance value to the corresponding member variables.
            mHfreeTolStd = psngBuildTol         '....Standard Tolerance. Fixed. 

            '....Initialize user-adjustable non-standard Tolerances. 
            HFreeTol(1) = psngBuildTol
            HFreeTol(2) = psngBuildTol

            '....Min ID.
            If IsNothing(pRecord(0).fldDiMin) = True Then
                mDiMin = 0.0#
            Else
                mDiMin = pRecord(0).fldDiMin * mUnit.CFacConL
            End If

            '....Max Seal Width.
            If IsNothing(pRecord(0).fldWidMax) = True Then
                mWidMax = mWid
            Else
                mWidMax = pRecord(0).fldWidMax
            End If
            mWidMax = mWidMax * mUnit.CFacConL

            'Allowable Hfree Adjustments:
            '----------------------------
            '
            '....Min Value
            If IsNothing(pRecord(0).fldHfreeAdjMinPCent) = True Then
                mHfreeAdjMinPCent = 20
            Else
                mHfreeAdjMinPCent = pRecord(0).fldHfreeAdjMinPCent
            End If

            '....Max Value
            If IsNothing(pRecord(0).fldHfreeAdjMaxPCent) = True Then
                mHfreeAdjMaxPCent = 20
            Else
                mHfreeAdjMaxPCent = pRecord(0).fldHfreeAdjMaxPCent
            End If

            '....Calculate the allowable decrement & increment on adjustment about HfreeStd.        
            mDHfreeAdjMin = mHfreeStd * (mHfreeAdjMinPCent / 100.0)
            mDHfreeAdjMax = mHfreeStd * (mHfreeAdjMaxPCent / 100.0)

            '....Strip Width.
            If IsNothing(pRecord(0).fldStripWid) = True Then
                mStripWid = 0.0#
            Else
                mStripWid = pRecord(0).fldStripWid * mUnit.CFacConL
            End If

        Catch ex As Exception
            MessageBox.Show("No Record Found.", "ERROR to retrieve Manf. Data", _
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub RetrieveFEAParams(ByVal CrossSecNo_In As String)
        '=======================================================
        'This routine retrieves the FEA parameters from the "USeal" database,
        '....based on the 'selected' U-Seal cross-section.

        'Unit Conversion:
        '---------------
        'The data stored in the database are in the English consistent unit.
        '....The unit conversion factor affects the retrieved data, only if
        '........unitSys = "Metric".

        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        Dim pRecord = (From pRec In pSealEntities.tblUSeal_FEAParams
                           Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

        Try

            'Retrieve FEA Parameters:
            '------------------------
            '....No of Contact Elements on either side of the assumed contact points:
            '
            '....Sealing Point.
            If IsNothing(pRecord(0).fldNelConSeal) = True Then
                NelConSeal = 4
            Else
                NelConSeal = pRecord(0).fldNelConSeal
            End If

            '....Heel Point:
            If IsNothing(pRecord(0).fldNelConHeel) = True Then
                mNelConHeel = 3
            Else
                mNelConHeel = pRecord(0).fldNelConHeel
            End If

            '....Radial Point:
            If IsNothing(pRecord(0).fldNelConRad) = True Then
                NelConRad = 4
            Else
                NelConRad = pRecord(0).fldNelConRad
            End If

            '....# of Element Layers thru' Thickness.
            If IsNothing(pRecord(0).fldNLayer) = True Then
                NLayer = 5
            Else
                NLayer = pRecord(0).fldNLayer
            End If

            '....Clustering Parameters:
            '
            '....Thickness Direction.
            If IsNothing(pRecord(0).fldBetaT) = True Then
                BetaT = 1.3
            Else
                BetaT = pRecord(0).fldBetaT
            End If

            '....Meridional Direction.
            If IsNothing(pRecord(0).fldBetaM) = True Then
                BetaM = 5.0#
            Else
                BetaM = pRecord(0).fldBetaM
            End If


        Catch pEXP As Exception
            MessageBox.Show("No Record Found.", "ERROR to retrieve FEA Data", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

#End Region


#Region "NEW DATABASE - DELETING & WRITING RECORDS:"

    Public Shared Sub DeleteNewUSealRec(ByVal CrossSecNo_In As String, _
                                              ByVal Files_In As IPE_clsFile)
        '===================================================================

        Dim pSealNewDBUserEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pRecord = (From pRec In pSealNewDBUserEntities.tblUSealNew_Geom
                       Where pRec.fldCustID = gIPE_Project.Customer_ID And
                                         pRec.fldPlatformID = gIPE_Project.Platform_ID And
                                         pRec.fldProjectID = gIPE_Project.Project_ID And
                                         pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

        If (pRecord.Count() > 0) Then
            pSealNewDBUserEntities.DeleteObject(pRecord(0))
            pSealNewDBUserEntities.SaveChanges()
        End If

        '....Delete from New CrossSec List.
        If CrossSecNewList.Contains(CrossSecNo_In) Then _
            CrossSecNewList.Remove(CrossSecNo_In)

        '....Delete the txt file, if any.
        Dim pNewFileName As String = Files_In.DirIn & CrossSecNo_In & ".txt"
        If File.Exists(pNewFileName) Then File.Delete(pNewFileName)

        ' ''....Delete Records from ProjectDB corresponding to the CrossSecNo, if any.
        ''Dim pProjectDBEntities As New ProjectDBEntities()
        ''Dim pQryRec = (From pRec In pProjectDBEntities.tblSeal
        ''                Where pRec.fldMCS = CrossSecNo_In Select pRec).ToList()

        ''Dim pRecDB As New tblAnalysis
        ''For Each pRecDB In pQryRec
        ''    pProjectDBEntities.DeleteObject(pRecDB)
        ''Next

        ''pProjectDBEntities.SaveChanges()

    End Sub


    Public Sub AddRecToUSealNewDB(ByVal Files_In As IPE_clsFile, _
                                  ByVal Project_In As IPE_clsProject, _
                                  ByVal UserInfo_In As IPE_clsUser, _
                                  ByVal Unit_In As IPE_clsUnit,
                                  ByVal Cavity_In As IPE_clsCavity)
        '==============================================================
        '....This routine saves data in the database in Engilsh Unit. 
        '........Add data to 'tblESeal_Geom' of new DB 'SealNewDB2_User.mdf'    

        '....Store the chosen culture name.
        Dim pCultureName_Chosen As String = Thread.CurrentThread.CurrentCulture.Name()

        '......Change Current Culture to 'USA'. Required for storing data into the database.
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        Dim pT As Single
        Dim pLLeg As Single
        Dim pRad(3) As Single
        Dim pTheta(3) As Single

        pT = Unit_In.L_ConToEnglish(mT)

        For i As Integer = 1 To 2
            pRad(i) = Unit_In.L_ConToEnglish(mR(i))
            pTheta(i) = mTheta(i)
        Next

        pLLeg = mLLeg

        Dim pSealNewDBUserEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        If Not mCrossSecNewList.Contains(MCrossSecNo) Then       '....CROSS-SECTION DOES'NT EXIST.
            '--------------------------------------------
            '
            '....INSERT Data:
            Dim pRecord As New tblUSealNew_Geom
            pRecord.fldCustID = Project_In.Customer_ID
            pRecord.fldPlatformID = Project_In.Platform_ID
            pRecord.fldProjectID = Project_In.Project_ID
            pRecord.fldCrossSecNo = MCrossSecNo
            pRecord.fldCrossSecNoOrg = mCrossSecNoOrg
            pRecord.fldThick = pT
            pRecord.fldLLeg = pLLeg
            pRecord.fldR1 = pRad(1)
            pRecord.fldR2 = pRad(2)
            pRecord.fldTheta1 = mTheta(1)
            pRecord.fldTheta2 = mTheta(2)
            pRecord.fldCavityCornerRad = Cavity_In.CornerRad

            pSealNewDBUserEntities.AddTotblUSealNew_Geom(pRecord)

            pSealNewDBUserEntities.SaveChanges()

        ElseIf mCrossSecNewList.Contains(MCrossSecNo) Then       '....CROSS-SECTION DOES EXIST.
            '--------------------------------------------
            '
            '....UPDATE Data:
            '
            Dim pRecord = (From pRec In pSealNewDBUserEntities.tblUSealNew_Geom
                            Where pRec.fldCrossSecNo = MCrossSecNo Select pRec).ToList()

            If (pRecord.Count() > 0) Then
                pRecord(0).fldCustID = Project_In.Customer_ID
                pRecord(0).fldPlatformID = Project_In.Platform_ID
                pRecord(0).fldProjectID = Project_In.Project_ID
                pRecord(0).fldCrossSecNoOrg = mCrossSecNoOrg
                pRecord(0).fldThick = pT
                pRecord(0).fldLLeg = pLLeg
                pRecord(0).fldR1 = pRad(1)
                pRecord(0).fldR2 = pRad(2)
                pRecord(0).fldTheta1 = mTheta(1)
                pRecord(0).fldTheta2 = mTheta(2)
                pRecord(0).fldCavityCornerRad = Cavity_In.CornerRad

                pSealNewDBUserEntities.SaveChanges()

                '....Delete Records from ProjectDB corresponding to the CrossSecNo, if any.
                ''Dim pProjectDBEntities As New ProjectDBEntities()
                ''Dim pQryRec = (From pRec In pProjectDBEntities.tblSeal
                ''                Where pRec.fldMCS = MCrossSecNo Select pRec).ToList()

                ''Dim pRecDB As tblAnalysis
                ''For Each pRecDB In pQryRec
                ''    pProjectDBEntities.DeleteObject(pRecDB)
                ''Next

                ''pProjectDBEntities.SaveChanges()
            End If
        End If

        Retrieve_CrossSections(mCrossSecNewList, "SealNewDB", "tblUSeal_Geom")

        'AES 30AUG16
        ''....Write .txt file.
        'WriteFile_NewDesign(Files_In, Project_In, UserInfo_In, Unit_In, Cavity_In)

        '.....Reset the culture back to the "user-chosen" one.
        Thread.CurrentThread.CurrentCulture = New CultureInfo(pCultureName_Chosen)

    End Sub

#End Region

#End Region

End Class
