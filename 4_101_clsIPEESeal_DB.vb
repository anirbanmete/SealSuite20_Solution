
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsESeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  30AUG16                                '
'                                                                              '
'===============================================================================
'
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


Partial Public Class IPE_clsESeal
    Inherits IPE_clsSeal

#Region "MEMBER VARIABLE DECLARATIONS:"

    '   ........Hfree Adjustment Percentages:
    Private mHfreeAdjMinPCent As Single                                 '(RETRIEVED)  
    Private mHfreeAdjMaxPCent As Single                                 '(RETRIEVED) 

#End Region


#Region "UTILITY ROUTINES:"

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
        mWidMax = mWidStd * mUnit.CFacConL          'PB 15APR10

        RetrieveFEAParams(mCrossSecNoOrg)           '....FEA           Parameters.

    End Sub


    Private Sub RetrieveGeomParams(ByVal CrossSecNo_In As String)
        '========================================================
        'This routine retrieves the geometrical parameters from the "ESeal" database,
        '....based on the selected Cross-Sec No.

        'Unit Conversion:
        '---------------
        'The data stored in the database are in the English consistent unit.
        '....The unit conversion factor affects the retrieved data, only if
        '........unitSys = "Metric".

        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pSealNewDBEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        'Dim pRecord As New List(Of tblESeal_Geom)
        Try

            Dim pRecord As Object = (From pRec In pSealEntities.tblESeal_Geom
                            Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

            If (mNewDesign = True) Then
                pRecord = (From pRec In pSealNewDBEntities.tblESealNew_Geom
                            Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

            End If


            '   Both "Standard" & "New Design":         
            '   ==============================
            mTemplateNo = pRecord(0).fldTemplateNo
            mNConv = pRecord(0).fldNConv

            '....Standard Thickness
            mTStd = pRecord(0).fldThickness * mUnit.CFacConL

            '....In the present version, as the thickness cannot be adjusted for 
            '........an "E-Seal", keeping the same cross-section no, the adjusted 
            '........thickness is set equal to the std. value.
            mT = mTStd      '....Initialization.


            'End Convolutions:
            '-----------------
            '....E1
            mRadE(1) = pRecord(0).fldRadE1 * mUnit.CFacConL
            mThetaE(1) = pRecord(0).fldThetaE1

            '....E2
            mRadE(2) = pRecord(0).fldRadE2 * mUnit.CFacConL
            mThetaE(2) = pRecord(0).fldThetaE2

            '....E3
            mRadE(3) = pRecord(0).fldRadE3 * mUnit.CFacConL
            mThetaE(3) = pRecord(0).fldThetaE3


            If mTemplateNo = "2" Then
                mLFlatE = pRecord(0).fldLFlatE * mUnit.CFacConL
            End If


            'Mid Convolutions:
            '-----------------
            '
            '....M1:
            '
            '........mRadM(1) is a dependent variable.
            '
            mRadM(1) = mRadE(1)
            mThetaM(1) = pRecord(0).fldThetaM1

            Dim pRadM2_Val As Single = pRecord(0).fldRadM2


            '....M2:        
            If IsNothing(pRecord(0).fldRadM2) Then
                mRadM(2) = mcSTRAIGHT_SEC_RADIUS

            ElseIf Abs(pRadM2_Val - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then
                mRadM(2) = mcSTRAIGHT_SEC_RADIUS

            Else
                mRadM(2) = pRecord(0).fldRadM2 * mUnit.CFacConL
            End If


            If Abs(mRadM(2) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then
                mThetaM(2) = 0.0

            Else
                mThetaM(2) = pRecord(0).fldThetaM2
            End If

            '....M3
            '
            If IsNothing(pRecord(0).fldRadM3) Then
                mRadM(3) = mRadM(1)

            Else
                mRadM(3) = pRecord(0).fldRadM3 * mUnit.CFacConL
            End If

            '....mThetaM (3) is a dependent (ref.) variable and is calculated 
            '........at the end. 

            '....M4 
            '
            Dim pRadM4_Val As Single = pRecord(0).fldRadM4

            If IsNothing(pRecord(0).fldRadM4) Then
                mRadM(4) = mcSTRAIGHT_SEC_RADIUS

            ElseIf Abs(pRadM4_Val - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then
                mRadM(4) = mcSTRAIGHT_SEC_RADIUS

            Else
                mRadM(4) = pRecord(0).fldRadM4 * mUnit.CFacConL
            End If


            If Abs(mRadM(4) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then
                mThetaM(4) = 0.0
            Else
                mThetaM(4) = pRecord(0).fldThetaM4
            End If

            '....M5                
            If IsNothing(pRecord(0).fldRadM5) Then
                mRadM(5) = mRadM(1)
            Else
                mRadM(5) = pRecord(0).fldRadM5 * mUnit.CFacConL
            End If

            '....mThetaM(5) is a dependent variable and calculated at the end.


            '   Additional Data Retrieval.
            '   ==========================          
            If mNewDesign = True Then
                '   NEW DESIGN:
                '   -----------
                '....Original CrossSecNo.
                mCrossSecNoOrg = pRecord(0).fldCrossSecNoOrg

            Else
                If IsNothing(pRecord(0).fldToolNo) Then
                    mToolNo = ""
                Else
                    mToolNo = pRecord(0).fldToolNo
                End If

            End If

            If IsNothing(pRecord(0).fldCavityCornerRad) Then
                mCavityCornerRad = mcCavityCornerRad_Def '0.0#  'AES 07APR16
            Else
                mCavityCornerRad = pRecord(0).fldCavityCornerRad * mUnit.CFacConL
            End If




            '   Calculate/Assign the dependent variables:                 
            '   =========================================
            '   ....Invoke Design Rules.
            '
            mRadM(1) = mRadE(1)                 '....Assignment done again for clarity.
            mThetaM(3) = DesignRule_ThetaM(3)
            mThetaM(5) = DesignRule_ThetaM(5)


            If Abs(mRadM(2) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Or _
               Abs(mRadM(4) - mcSTRAIGHT_SEC_RADIUS) <= gcEPS Then

                DesignRule_RadM_Straight()      '....Template No assigned 
            End If                              '........to "1GenS" inside.


            If mTemplateNo <> "1Gen" Then
                '
                '....Template = "1", "2" & "1GenS".
                '
                mHConv = pRecord(0).fldhConv * mUnit.CFacConL
                UpdateLFlatM_End()              '....mLFlatM_End           
                UpdateLFlatM_Mid()              '....mLFlatM_Mid    
            End If


        Catch pEXP As Exception

            MessageBox.Show("No Record Found.", "ERROR to retrieve E-Seal Geom. Data", _
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
        Dim pRecord = (From pRec In pSealEntities.tblESeal_ManfData
                        Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()


        If (pRecord.Count() > 0) Then

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

            '....Non-Standard Tolerance. User adjustble. 
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

        Else
            MessageBox.Show("No Record Found.", "ERROR to retrieve Manf. Data", _
                           MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

    End Sub


    Private Sub RetrieveFEAParams(ByVal CrossSecNo_In As String)
        '=======================================================
        'This routine retrieves the FEA parameters from the "ESeal" database,
        '....based on the 'selected' E-Seal cross-section.

        'Unit Conversion:
        '---------------
        'The data stored in the database are in the English consistent unit.
        '....The unit conversion factor affects the retrieved data, only if
        '........unitSys = "Metric".

        'Data Reader object.
        '------------------
        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pRecord = (From pRec In pSealEntities.tblESeal_FEAParams
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


            '....Mid Point:
            If IsNothing(pRecord(0).fldNelConMid) = True Then
                mNelConMid = 3
            Else
                mNelConMid = pRecord(0).fldNelConMid
            End If

            mNelConMid = 3


            '....Heel Point:
            If IsNothing(pRecord(0).fldNelConHeel) = True Then
                mNelConHeel = 3
            Else
                mNelConHeel = pRecord(0).fldNelConHeel
            End If


            '....Radial Point(s):
            If IsNothing(pRecord(0).fldNelConHeel) = True Then
                NelConRad = 4
            Else
                NelConRad = pRecord(0).fldNelConHeel
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


            '....Weightage on the element density over segements:
            '
            '....Segment E1.
            If IsNothing(pRecord(0).fldWtE1) = True Then
                mWtE(1) = 1.5
            Else
                mWtE(1) = pRecord(0).fldWtE1
            End If

            '....Segment E2.
            If IsNothing(pRecord(0).fldWtE2) = True Then
                mWtE(2) = 1.0#
            Else
                mWtE(2) = pRecord(0).fldWtE2
            End If

            '....Segment E3.
            If IsNothing(pRecord(0).fldWtE3) = True Then
                mWtE(3) = 1.0#
            Else
                mWtE(3) = pRecord(0).fldWtE3
            End If

            '....Segment M2
            If IsNothing(pRecord(0).fldWtM2) = True Then
                mWtM(2) = 1.0#
            Else
                mWtM(2) = pRecord(0).fldWtM2
            End If

            '....Segment M1 & M3
            mWtM(1) = mWtE(1)
            mWtM(3) = mWtE(1)

        Catch pEXP As Exception
            MessageBox.Show("No Record Found.", "ERROR to retrieve FEA Data", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

#End Region


#Region "NEW DATABASE WRITING AND DELETE"

    Public Shared Sub DeleteNewESealRec(ByVal CrossSecNo_In As String, _
                                         ByVal Files_In As IPE_clsFile)
        '===================================================================

        ''Dim pSealNewDBUserEntities As New SealDBEntities()
        ''Dim pRecord = (From pRec In pSealNewDBUserEntities.tblESealNew_Geom
        ''                Where pRec.fldCrossSecNo = CrossSecNo_In Select pRec).ToList()

        ''If (pRecord.Count() > 0) Then
        ''    pSealNewDBUserEntities.DeleteObject(pRecord(0))
        ''    pSealNewDBUserEntities.SaveChanges()
        ''End If


        ' ''....Delete from New CrossSec List.
        ''If CrossSecNewList.Contains(CrossSecNo_In) Then _
        ''    CrossSecNewList.Remove(CrossSecNo_In)

        ' ''....Delete the txt file, if any.
        ''Dim pNewFileName As String = Files_In.DirIn & CrossSecNo_In & ".txt"
        ''If File.Exists(pNewFileName) Then File.Delete(pNewFileName)

        ' ''....Delete Records from ProjectDB corresponding to the CrossSecNo, if any.
        ''Dim pProjectDBEntities As New ProjectDBEntities()
        ''Dim pQryRec = (From pRec In pProjectDBEntities.tblSeal
        ''                Where pRec.fldMCS = CrossSecNo_In Select pRec).ToList()

        ''Dim pRecDB As tblAnalysis
        ''For Each pRecDB In pQryRec
        ''    pProjectDBEntities.DeleteObject(pRecDB)
        ''Next

        ''pProjectDBEntities.SaveChanges()

    End Sub


    Public Sub AddRecToESealNewDB(ByVal Files_In As IPE_clsFile, _
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

        mTStd = mT

        Dim pTStd As Single
        Dim pRadM(5) As Single
        Dim pRadE(3) As Single
        Dim pHConv As Single

        pTStd = Unit_In.L_ConToEnglish(mTStd)
        pHConv = Unit_In.L_ConToEnglish(mHConv)

        For i As Integer = 0 To 3
            pRadE(i) = Unit_In.L_ConToEnglish(mRadE(i))
        Next

        For i As Integer = 0 To 5
            If (TemplateNo = "1GenS" And (i = 2 Or i = 4)) Then
                pRadM(i) = mRadM(i)

            Else
                pRadM(i) = Unit_In.L_ConToEnglish(mRadM(i))
            End If

        Next

        Dim pSealNewDBEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        If Not mCrossSecNewList.Contains(MCrossSecNo) Then       '....CROSS-SECTION DOES'NT EXIST.
            '--------------------------------------------
            '
            '....INSERT Data:

            Dim pRecord As New tblESealNew_Geom
            pRecord.fldCustID = gIPE_Project.Customer_ID
            pRecord.fldPlatformID = gIPE_Project.Platform_ID
            pRecord.fldProjectID = gIPE_Project.Project_ID
            pRecord.fldCrossSecNo = MCrossSecNo
            pRecord.fldCrossSecNoOrg = mCrossSecNoOrg
            pRecord.fldTemplateNo = "1Gen"
            pRecord.fldNConv = mNConv
            pRecord.fldThickness = pTStd
            pRecord.fldRadE1 = pRadE(1)
            pRecord.fldThetaE1 = mThetaE(1)
            pRecord.fldRadE2 = pRadE(2)
            pRecord.fldThetaE2 = mThetaE(2)
            pRecord.fldRadE3 = pRadE(3)
            pRecord.fldThetaE3 = mThetaE(3)
            pRecord.fldThetaM1 = mThetaM(1)
            pRecord.fldRadM2 = pRadM(2)
            pRecord.fldThetaM2 = mThetaM(2)
            pRecord.fldRadM3 = pRadM(3)
            pRecord.fldThetaM3 = mThetaM(3)
            pRecord.fldRadM4 = pRadM(4)
            pRecord.fldThetaM4 = mThetaM(4)
            pRecord.fldRadM5 = pRadM(5)
            pRecord.fldThetaM5 = mThetaM(5)
            pRecord.fldHConv = pHConv
            pRecord.fldCavityCornerRad = Cavity_In.CornerRad

            pSealNewDBEntities.AddTotblESealNew_Geom(pRecord)
            pSealNewDBEntities.SaveChanges()
        ElseIf mCrossSecNewList.Contains(MCrossSecNo) Then       '....CROSS-SECTION DOES EXIST.
            '--------------------------------------------
            '
            '....UPDATE Data:
            '
            Dim pRecord = (From pRec In pSealNewDBEntities.tblESealNew_Geom
                            Where pRec.fldCrossSecNo = MCrossSecNo Select pRec).ToList()

            If (pRecord.Count() > 0) Then
                pRecord(0).fldCustID = gIPE_Project.Customer_ID
                pRecord(0).fldPlatformID = gIPE_Project.Platform_ID
                pRecord(0).fldProjectID = gIPE_Project.Project_ID
                pRecord(0).fldCrossSecNoOrg = mCrossSecNoOrg
                pRecord(0).fldTemplateNo = "1Gen"
                pRecord(0).fldNConv = mNConv
                pRecord(0).fldThickness = pTStd
                pRecord(0).fldRadE1 = pRadE(1)
                pRecord(0).fldThetaE1 = mThetaE(1)
                pRecord(0).fldRadE2 = pRadE(2)
                pRecord(0).fldThetaE2 = mThetaE(2)
                pRecord(0).fldRadE3 = pRadE(3)
                pRecord(0).fldThetaE3 = mThetaE(3)
                pRecord(0).fldThetaM1 = mThetaM(1)
                pRecord(0).fldRadM2 = pRadM(2)
                pRecord(0).fldThetaM2 = mThetaM(2)
                pRecord(0).fldRadM3 = pRadM(3)
                pRecord(0).fldThetaM3 = mThetaM(3)
                pRecord(0).fldRadM4 = pRadM(4)
                pRecord(0).fldThetaM4 = mThetaM(4)
                pRecord(0).fldRadM5 = pRadM(5)
                pRecord(0).fldThetaM5 = mThetaM(5)
                pRecord(0).fldHConv = pHConv
                pRecord(0).fldCavityCornerRad = Cavity_In.CornerRad

                pSealNewDBEntities.SaveChanges()
            End If

            ' ''....Delete Records from ProjectDB corresponding to the CrossSecNo, if any.
            ''Dim pProjectDBEntities As New ProjectDBEntities()
            ''Dim pQryRec = (From pRec In pProjectDBEntities.tblSeal
            ''                Where pRec.fldMCS = MCrossSecNo Select pRec).ToList()

            ''Dim pRecDB As tblAnalysis
            ''For Each pRecDB In pQryRec
            ''    pProjectDBEntities.DeleteObject(pRecDB)
            ''Next

            ''pProjectDBEntities.SaveChanges()

        End If

        'AES 30AUG16
        ''....Write .txt file.
        'WriteFile_NewDesign(Files_In, Project_In, UserInfo_In, Unit_In, Cavity_In)

        '.....Reset the culture back to the "user-chosen" one.
        Thread.CurrentThread.CurrentCulture = New CultureInfo(pCultureName_Chosen)

    End Sub


#End Region

#End Region

End Class
