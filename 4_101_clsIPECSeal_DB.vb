'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsCSeal                               '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29MAR16                                '
'                                                                              '
'===============================================================================
'Routines       
'--------
'
'   METHODS:
'   -------- 
'
'       Private Sub                 RetrieveDBParams        ()
'       Private Sub                 RetrieveGeomParams      ()
'       Private Sub                 RetrieveFEAParams       ()
'       Private Sub                 RetrieveManfParams      ()
'       Private Sub                 RetrieveHfreeStdValues  ()
'
'
'--------------------------------------------------------------------------------

Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms


Partial Public Class IPE_clsCSeal
    Inherits IPE_clsSeal

    '*******************************************************************************
    '*                        CLASS METHODS - BEGIN                                *
    '*******************************************************************************

    '-------------------------------------------------------------------------------
    '                          DATABASE READING - BEGIN                            '
    '-------------------------------------------------------------------------------

#Region "DATABASE READING"


    Private Sub RetrieveDBParams()
        '=========================       
        '....Retrieve Standard CSeal parameters from the database.
        RetrieveGeomParams()
        RetrieveManfParams()
        RetrieveFEAParams()

    End Sub


    Private Sub RetrieveGeomParams()
        '===========================
        'This routine retrieves the geometrical parameters from the "CSeal" database,
        '....based on the selected Cross-Sec No.

        '....Unit Conversion:
        'The data stored in the database are in the English consistent unit.
        '....The unit conversion factor affects the retrieved data, only if
        '........unitSys = "Metric".

        'AES 08MAY17
        'Dim pSealEntities As New SealDBEntities()
        Dim pSealMCSEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        Dim pRecord = (From pRec In pSealMCSEntities.tblCSeal_Geom
                           Where pRec.fldCrossSecNo = MCrossSecNo Select pRec).ToList()


        Try
            mTStd = pRecord(0).fldThick * mUnit.CFacConL

            '....ZClear is not read from the CSeal geometry database table, but 
            '........calculated by interpolation from "Hfree" when the routine 
            '........"Update_ControlDiaParams" is called in the "Initialize" routine.
            ''mZClear = pobjDR("fldZClear") * mUnit.CFacConL 

            mHfreeStd = pRecord(0).fldHFree * mUnit.CFacConL

            '....Tolerances on Hfree:
            Dim psngHFreeTol As Single
            psngHFreeTol = pRecord(0).fldHFreeTol * mUnit.CFacConL

            mHfreeTolStd = psngHFreeTol         '....Standard Tolerance: Fixed. 
            '....Non-Standard Tolerances: User adjustble. These are initialized to the 
            '........standard tolerance.
            HFreeTol(1) = psngHFreeTol
            HFreeTol(2) = psngHFreeTol

            mWidMax = pRecord(0).fldWidMax * mUnit.CFacConL
            mCavityCornerRad = pRecord(0).fldCavityCornerRad * mUnit.CFacConL

            If IsNothing(pRecord(0).fldToolNo) Then
                mToolNo = ""
            Else
                mToolNo = pRecord(0).fldToolNo
            End If

            '....Strip Width.      
            If IsNothing(pRecord(0).fldStripWid) = True Then
                mStripWid = 0.0#
            Else
                mStripWid = pRecord(0).fldStripWid * mUnit.CFacConL
            End If

        Catch
            'MsgBox(Err.Description)
            MessageBox.Show("No Record Found.", "ERROR to retrieve C-Seal Geom. Data", _
                           MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub RetrieveFEAParams()
        '==========================
        'This routine retrieves the FEA parameters from the "ESeal" database,
        '....based on the 'selected' E-Seal design..

        'AES 08MAY17
        'Dim pSealEntities As New SealDBEntities()
        Dim pSealMCSEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        Dim pRecord = (From pRec In pSealMCSEntities.tblCSeal_FEAParams
                           Where pRec.fldCrossSecNo = MCrossSecNo Select pRec).ToList()

        Try

            'Retrieve FEA Parameters:
            '------------------------
            '....No. of Contact Elements on either side of the assumed contact points:
            '
            '....Sealing Point.

            If IsNothing(pRecord(0).fldNelConSeal) = True Then
                NelConSeal = 15             '....10 is not adequate with a finer mesh. 20MAR07.
            Else
                NelConSeal = pRecord(0).fldNelConSeal
            End If

            '....Radial Point:
            If IsNothing(pRecord(0).fldNelConRad) = True Then
                NelConRad = 10
            Else
                NelConRad = pRecord(0).fldNelConRad
            End If

            '....# of Element Layers thru' Thickness.
            If IsNothing(pRecord(0).fldNLayer) = True Then
                NLayer = 7
            Else
                NLayer = pRecord(0).fldNLayer
            End If

            '....# of Element Layers Plating thru' Thickness.
            If IsNothing(pRecord(0).fldNLayer_pl) = True Then
                PlatingNLayer = 4
            Else
                PlatingNLayer = pRecord(0).fldNLayer_pl
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

        Catch ex As Exception

            MessageBox.Show("No Record Found.", "ERROR to retrieve FEA Data", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub RetrieveManfParams()
        '===========================  

        '....Allowable maximum and minimum values the adjusted Hfree. 
        GetHFreeAdjParams()             '....Depends on HfreeStd. 

        'AES 08MAY17
        'Dim pSealEntities As New SealDBEntities()
        Dim pSealMCSEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pRecord = (From pRec In pSealMCSEntities.tblCSeal_ManfData
                           Where pRec.fldCrossSecNo = MCrossSecNo Select pRec).ToList()
        Try

            'Allowable ThetaOpening Adjustments:
            '-----------------------------------
            Dim pThetaOpeningAdjMinPCent As Single, pThetaOpeningAdjMaxPCent As Single

            '....Min Percent Decrement.
            If IsNothing(pRecord(0).fldThetaOpeningAdjMinPCent) = True Then
                pThetaOpeningAdjMinPCent = 20
            Else
                pThetaOpeningAdjMinPCent = pRecord(0).fldThetaOpeningAdjMinPCent
            End If


            '....Max Percent Increment.
            If IsNothing(pRecord(0).fldThetaOpeningAdjMaxPCent) = True Then
                pThetaOpeningAdjMaxPCent = 20
            Else
                pThetaOpeningAdjMaxPCent = pRecord(0).fldThetaOpeningAdjMaxPCent
            End If

            '....Calculate the allowable decrement & increment on adjustment about 
            '........ThetaOpeningStd.         
            mDThetaOpeningAdjMin = mThetaOpeningStd * (pThetaOpeningAdjMinPCent / 100.0)
            mDThetaOpeningAdjMax = mThetaOpeningStd * (pThetaOpeningAdjMaxPCent / 100.0)


            'Minimum manufacturable diameter.
            '--------------------------------
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

        Catch ex As Exception
            MessageBox.Show("No Record Found.", "ERROR to retrieve Manf. Data", _
                           MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub RetrieveHfreeStdValues()
        '===============================
        'This routine retrieves the distinct Hfree values of the standard cross-sections 
        '....from the database.

        'AES 08MAY17
        'Dim pSealDBEntities As New SealDBEntities
        Dim pSealMCSEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        '....CSeal
        'Dim pQryCSeal = (From pRec In pSealDBEntities.tblCSeal_Geom Order By pRec.fldHFree Ascending Select pRec).Distinct()
        Dim pQryCSeal = (From pRec In pSealMCSEntities.tblCSeal_Geom Select pRec.fldHFree Distinct).ToList()

        Dim j As Int16 = 1      '....Starting index value set to 1. 
        Dim pRecord As New tblCSeal_Geom
        'For Each pRecord In pQryCSeal
        For i As Integer = 0 To pQryCSeal.Count - 1
            'CrossSecList_In.Add(pRecord.fldCrossSecNo)
            ReDim Preserve mArrayHFreeStd(j)
            mArrayHFreeStd(j) = pQryCSeal(i) * mUnit.CFacConL
            j = j + 1
        Next

        Dim jLast As Int16 = j                '....One extra index is kept at the end for 
        ReDim Preserve mArrayHFreeStd(jLast)  '........the last value to be extrapolated.

        '....Index = 0     : Extrapolated Value. 
        Dim pDiff As Single
        pDiff = (mArrayHFreeStd(2) - mArrayHFreeStd(1))
        mArrayHFreeStd(0) = mArrayHFreeStd(1) - pDiff

        '....Index = jLast : Extrapolated Value.
        pDiff = mArrayHFreeStd(jLast - 1) - mArrayHFreeStd(jLast - 2)
        mArrayHFreeStd(jLast) = mArrayHFreeStd(jLast - 1) + pDiff

    End Sub

#End Region

    '-------------------------------------------------------------------------------
    '                          DATABASE READING - END                              '
    '-------------------------------------------------------------------------------

    '*******************************************************************************
    '*                        CLASS METHODS - END                                  *
    '*******************************************************************************
End Class
