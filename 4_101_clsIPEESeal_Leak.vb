
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsESeal_Leak                          '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29MAR16                                '
'                                                                              '
'===============================================================================


Imports System.Data.OleDb
Imports System.Math
Imports System.Linq
Imports clsLibrary11

Partial Public Class IPE_clsESeal
    Inherits IPE_clsSeal


#Region "NAMED CONSTANTS:"
    '....The following arbitrary constant indicates missing leakage data in a table. 
    Const mcConst As Single = 999.99

#End Region


#Region "MEMBER VARIABLE DECLARATIONS:"
    '=================================

    Private mSFinish As Integer

    '....Intermediate variables:
    Private mPress As Single()
    Private mLoad As Single()
    Private mLeak As Single(,)

    Private mIUp As Integer             '....Upper Bound of the Press Array.
    Private mJUp As Integer             '....Upper Bound of the Load  Array.

#End Region


#Region "PROPERTY ROUTINES:"

    Public Property SFinish() As Integer
        '===============================
        Get
            Return mSFinish
        End Get

        Set(ByVal Value As Integer)
            '-------------------------
            mSFinish = Value
        End Set

    End Property


#End Region


#Region "CLASS METHODS:"


#Region "DATA RETRIEVAL FROM DATABASE:"


    Private Sub RetrieveDBParams(ByVal Coating_In As String)
        '===================================================

        Dim pTableName As String = ""
        Dim pFieldName As String = ""

        Select Case Coating_In

            Case "None"
                pTableName = "tblESeal_Leak_None"
                pFieldName = "fldLoad"

            Case "Tricom"
                pTableName = "tblESeal_Leak_Tricom"
                pFieldName = "fldLoad"

            Case "T800"
                pTableName = "tblESeal_Leak_T800"
                pFieldName = "fldSFinish"

        End Select


        '....Pressure
        mIUp = Retrieve_PressData(pTableName) - 1

        '....Load
        mJUp = Retrieve_LoadData(pTableName) - 1

        '   Leakage data:
        '   -------------
        Retrieve_LeakageData(pTableName, mIUp, mJUp)


        '....Determine in which cells leakage data are missing (earlier substituted with  
        '........the arbitrary constant "mcConst") and then place extrapolated values there.
        '
        For i As Integer = 0 To mIUp

            For j As Integer = 0 To mJUp

                If mLeak(i, j) = mcConst Then
                    mLeak(i, j) = BiDirectional_Extrapolation(i, j)
                End If

            Next

        Next

    End Sub


    Private Function Retrieve_PressData(ByVal TableName_In As String) As Integer
        '========================================================================

        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim i As Integer
        If (TableName_In = "tblESeal_Leak_None") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_None Select it.fldPress Distinct).ToList()

            For i = 0 To pQryESeal.Count - 1
                ReDim Preserve mPress(i)
                mPress(i) = pQryESeal(i)
            Next

        ElseIf (TableName_In = "tblESeal_Leak_Tricom") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_Tricom Select it.fldPress Distinct).ToList()

            For i = 0 To pQryESeal.Count - 1
                ReDim Preserve mPress(i)
                mPress(i) = pQryESeal(i)
            Next

        ElseIf (TableName_In = "tblESeal_Leak_T800") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_T800 Select it.fldPress Distinct).ToList()

            For i = 0 To pQryESeal.Count - 1
                ReDim Preserve mPress(i)
                mPress(i) = pQryESeal(i)
            Next

        End If

        ''....# of Pressure rows.
        Dim pCount As Integer
        pCount = i

        Return pCount

    End Function


    Private Function Retrieve_LoadData(ByVal TableName_In As String) As Integer
        '=====================================================================
        '....If the table names are 
        '........i) "tblESeal_Leak_None" & "tblESeal_Leak_Tricom", then the Load data ==> "Load/in".
        '.......ii) "tblESeal_Leak_T800",                            then the Load data ==> Surface Finish (RMS).

        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim i As Integer
        If (TableName_In = "tblESeal_Leak_None") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_None Select it.fldLoad Distinct).ToList()

            For i = 0 To pQryESeal.Count() - 1
                ReDim Preserve mLoad(i)
                mLoad(i) = pQryESeal(i)
                'i = i + 1
            Next

        ElseIf (TableName_In = "tblESeal_Leak_Tricom") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_Tricom Select it.fldLoad Distinct).ToList()

            For i = 0 To pQryESeal.Count() - 1
                ReDim Preserve mLoad(i)
                mLoad(i) = pQryESeal(i)
            Next

        ElseIf (TableName_In = "tblESeal_Leak_T800") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_T800 Select it.fldSFinish Distinct).ToList()

            For i = 0 To pQryESeal.Count() - 1
                ReDim Preserve mLoad(i)
                mLoad(i) = pQryESeal(i)
            Next

        End If

        ''....# of Pressure rows.
        Dim pCount As Integer
        pCount = i

        Return pCount


    End Function


    Private Sub Retrieve_LeakageData(ByVal tblName_In As String, _
                                     ByVal iDim_In As Integer, _
                                     ByVal jDim_In As Integer)
        '==========================================================

        ReDim mLeak(iDim_In, jDim_In)

        '....Initialize data element in the table with the preset constant.
        Dim i As Integer = 0, j As Integer = 0

        For i = 0 To iDim_In
            For j = 0 To jDim_In
                mLeak(i, j) = mcConst
            Next
        Next

        Dim pSealEntities As New SealIPEMCSDBEntities 'SealLibMCSDBEntities

        i = 0
        j = 0

        If (tblName_In = "tblESeal_Leak_None") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_None Select it.fldLeak).ToList()

            'Dim pRecord As New tblESeal_Leak_None
            For k As Integer = 0 To pQryESeal.Count - 1
                If j > jDim_In Then
                    i += 1
                    j = 0
                End If

                If IsNothing(pQryESeal(k)) = False Then
                    mLeak(i, j) = pQryESeal(k)
                End If
                j += 1
            Next

        ElseIf (tblName_In = "tblESeal_Leak_Tricom") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_Tricom Select it.fldLeak).ToList()

            For k As Integer = 0 To pQryESeal.Count - 1
                If j > jDim_In Then
                    i += 1
                    j = 0
                End If

                If IsNothing(pQryESeal(k)) = False Then
                    mLeak(i, j) = pQryESeal(k)
                End If
                j += 1
            Next

        ElseIf (tblName_In = "tblESeal_Leak_T800") Then

            Dim pQryESeal = (From it In pSealEntities.tblESeal_Leak_T800 Select it.fldLeak).ToList()

            For k As Integer = 0 To pQryESeal.Count() - 1
                If j > jDim_In Then
                    i += 1
                    j = 0
                End If

                If IsNothing(pQryESeal(k)) = False Then
                    mLeak(i, j) = pQryESeal(k)
                End If
                j += 1
            Next

        End If

    End Sub

#End Region         '...."DATA RETRIEVAL FROM DATABASE:"


#Region "UTILITY ROUTINES:"
    '----------------------

    Private Function Interpolate(ByVal x1 As Single, ByVal y1 As Single, _
                                 ByVal x2 As Single, ByVal y2 As Single, _
                                 ByVal xg As Single) As Single
        '=================================================================
        Dim Ratio As Single
        Ratio = (y2 - y1) / (x2 - x1)

        Dim yg As Single
        yg = y1 + Ratio * (xg - x1)

        Return yg

    End Function


    Private Function BiDirectional_Extrapolation(ByVal i_In As Integer, _
                                                 ByVal j_In As Integer) As Single
        '==========================================================================
        'This routine calculates the leakage in an empty cell in the table (indicated 
        '....by mcConst) by bidirectional extrapolation.

        Dim x1 As Single, x2 As Single, xg As Single     '....Press.  (Index i)
        Dim y1 As Single, y2 As Single, yg As Single     '....Load    (Index j)

        Dim z1 As Single, z2 As Single      '....Leakage (dependent variable)


        'I Direction:
        '------------
        x1 = mPress(i_In - 2)
        z1 = mLeak(i_In - 2, j_In)

        x2 = mPress(i_In - 1)
        z2 = mLeak(i_In - 1, j_In)

        xg = mPress(i_In)

        Dim zi As Single
        zi = Interpolate(x1, z1, x2, z2, xg)


        'J Direction:
        '------------
        y1 = mLoad(j_In - 2)
        z1 = mLeak(i_In, j_In - 2)

        y2 = mLoad(j_In - 1)
        z2 = mLeak(i_In, j_In - 1)

        yg = mLoad(j_In)

        Dim zj As Single
        zj = Interpolate(y1, z1, y2, z2, yg)


        'BiDirectional Extrapolation:
        '----------------------------
        Dim zij As Single
        zij = 0.5 * (zi + zj)

        Return zij

    End Function


    Private Function DoubleInterpolation(ByVal x1 As Single, ByVal x2 As Single, ByVal xg As Single, _
                                         ByVal y1 As Single, ByVal y2 As Single, ByVal yg As Single, _
                                         ByVal z11 As Single, ByVal z12 As Single, _
                                         ByVal z21 As Single, ByVal z22 As Single) As Single
        '============================================================================================== 

        Dim zg1 As Single, zg2 As Single
        Dim zgg As Single

        '....Single Interpolation: X direction @ y1.
        zg1 = Interpolate(x1, z11, x2, z21, xg)

        '....Single Interpolation: X direction @ y2.
        zg2 = Interpolate(x1, z12, x2, z22, xg)

        '....Single Interpolation: Y direction @ xg
        zgg = Interpolate(y1, zg1, y2, zg2, yg)

        Return zgg

    End Function

#End Region         '...."UTILITY ROUTINES:"


#Region "LEAKAGE CALCULATION ROUTINES:"
    '----------------------------------

    Public Function Leakage_Oper(ByVal PDiff_In As Single, _
                                 ByVal Coating_In As String) As Single
        '==============================================================
        '
        '....Input argument: PDiff_In is in Consistent Unit ("English" or "Metric").
        '....Return Value  : Leakage in User Unit (cc/s for both "English" & "Metric").

        '........The "Calc_Leakage" routine's (shown below) input arguments are in   
        '............Consistent "English" & the output is in User Unit. 
        '               Input : Press (psi), Load (lbf/in) or Surface Finish (rms).
        '               Output: Leakage (cc/s).

        Dim pPDiff_Con_Eng As Single
        pPDiff_Con_Eng = PDiff_In / mUnit.CFacConP     '....Consistent - English (psi)

        Dim pLeakage_Oper As Single

        If Coating_In = "None" Or Coating_In = "Tricom" Then

            '....The following conversion factor does for:
            '       English Unit: lbf/in ===> lbf/in.
            '       Metric  Unit:   N/m  ===> lbf/in.
            '
            Dim pCFac As Single
            pCFac = mUnit.CFacConL / mUnit.CFacConF

            '....Unit Circumferential Loads: Operating.
            Dim pLoad_Oper_Con_Eng As Single
            pLoad_Oper_Con_Eng = FSeat_Unit(1) * pCFac             '....Consistent - English (lbf/in)

            'If (Project_In.Analysis(gIPE_frmResults.ISel).LoadCase.Type = clsAnalysis.eLoadType.Baseline) Then
            '    If (Project_In.Analysis(gIPE_frmResults.ISel).AppLoad.PreComp.Exists = True) Then
            '        psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(gIPE_frmResults.ISel).Seal.StressMax(3)))
            '    Else
            '        psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(gIPE_frmResults.ISel).Seal.StressMax(2)))
            '    End If
            'Else
            '    psngMaxStress_Last = Val(Unit_In.RoundStressUnitUser(Project_In.Analysis(gIPE_frmResults.ISel).Seal.StressMax(2)))
            'End If

            '....Leakage.
            pLeakage_Oper = Calc_Leakage(pPDiff_Con_Eng, pLoad_Oper_Con_Eng, Coating_In)

        ElseIf Coating_In = "T800" Then
            pLeakage_Oper = Calc_Leakage(pPDiff_Con_Eng, mSFinish, Coating_In)

        End If

        If pLeakage_Oper < gcEPS Then
            pLeakage_Oper = 0.0
        End If

        Return pLeakage_Oper

    End Function


    Private Function Calc_Leakage(ByVal PDiff_Con_Eng_In As Single, ByVal Load_Con_Eng_In As Single, _
                                  ByVal Coating_In As String) As Single
        '==============================================================================================
        '....If Coating_In = "None" or "Tricom", then Load_Con_Eng_In ===> Seating Load / Circum. in.
        '....Elseif        = "T800",             then Load_Con_Eng_In ===> Surface finish (RMS).

        '   Input arguments: In Consisten Unit - "English".
        '   Output         : In User Unit (cc/s)


        '....Retrieve parameters from the database.

        RetrieveDBParams(Coating_In)


        'Determine the indices of the upper LHS corner of the appropriate cell 
        '........for interpolation / extrapolation.
        '---------------------------------------------------------------------

        '....Pressure Direction:
        '
        Dim iCell As Integer
        Dim i As Integer

        If PDiff_Con_Eng_In <= mPress(0) Then
            iCell = 0

        ElseIf PDiff_Con_Eng_In >= mPress(mIUp) Then
            iCell = mIUp - 1

        Else
            '....Pressure is within the table.

            For i = 0 To mIUp

                If PDiff_Con_Eng_In <= mPress(i) Then
                    Exit For
                End If

            Next

            iCell = i - 1

        End If


        '....Load Direction:
        '
        Dim jCell As Integer
        Dim j As Integer

        If Load_Con_Eng_In <= mLoad(0) Then
            jCell = 0

        ElseIf Load_Con_Eng_In >= mLoad(mJUp) Then
            jCell = mJUp - 1

        Else
            '....Load is within the table.

            For j = 0 To mJUp

                If Load_Con_Eng_In <= mLoad(j) Then
                    Exit For
                End If

            Next

            jCell = j - 1

        End If


        'Calculate Leakage:
        '------------------

        '....Pressures:
        Dim x1 As Single, x2 As Single, xg As Single

        x1 = mPress(iCell)
        x2 = mPress(iCell + 1)
        xg = PDiff_Con_Eng_In


        '....Loads:
        Dim y1 As Single, y2 As Single, yg As Single

        y1 = mLoad(jCell)
        y2 = mLoad(jCell + 1)
        yg = Load_Con_Eng_In


        '....Leakages:
        Dim z11 As Single, z12 As Single
        Dim z21 As Single, z22 As Single

        z11 = mLeak(iCell, jCell)
        z12 = mLeak(iCell, jCell + 1)
        z21 = mLeak(iCell + 1, jCell)
        z22 = mLeak(iCell + 1, jCell + 1)

        Calc_Leakage = DoubleInterpolation(x1, x2, xg, y1, y2, yg, z11, z12, z21, z22)

    End Function

#End Region

#End Region

End Class
