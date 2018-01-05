'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsUSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  03JUN11                                '
'                                                                              '
'===============================================================================
'
Imports System.Drawing

Partial Public Class IPE_clsUSeal
    Inherits IPE_clsSeal


    Private mY_Sym_Axis As Single       '....Symmetry Axis Location. 


    Public Sub Create_DXF(ByVal FileName_In As String)
        '==================================================    

        '....HEADER.   
        DXF_Header(FileName_In)

        '=======================================================
        '                  DXF CREATION DRAWING                '
        '=======================================================
        DrawUSeal_DXF()

        '....FOOTER.
        DXF_Footer()

    End Sub


    Private Sub DrawUSeal_DXF()
        '=====================

        Dim pAngStart As Single, pAngEnd As Single
        Dim pRadOut As Single, pRadIn As Single

        Dim pCen As PointF

        '   SEGMENT 1:
        '   ----------
        pRadIn = mR(1) - 0.5 * mT
        pRadOut = mR(1) + 0.5 * mT

        pCen = mCen(1)

        mY_Sym_Axis = mCen(1).Y

        If POrient = "External" Then
            pAngStart = -90 - (0.5 * mTheta(1))

        ElseIf POrient = "Internal" Then
            pAngStart = 90 - (0.5 * mTheta(1))

        End If
        pAngEnd = pAngStart + mTheta(1)

        DXF_Arcs_SymAxis(mY_Sym_Axis, pCen, pRadIn, pAngStart, pAngEnd)
        DXF_Arcs_SymAxis(mY_Sym_Axis, pCen, pRadOut, pAngStart, pAngEnd)


        '   SEGMENT 2:  straight segment.               
        '   ----------

        DXF_Lines_SymAxis(mY_Sym_Axis, mKP(5), mKP(3))
        DXF_Lines_SymAxis(mY_Sym_Axis, mKP(6), mKP(4))


        '   SEGMENT 3:       
        '   ----------
        '....Inner & Outer Radii.
        pRadIn = mR(2) - 0.5 * mT
        pRadOut = mR(2) + 0.5 * mT

        pCen = mCen(2)
        Dim pBeta As Single = 90 - (0.5 * mTheta(1))

        If POrient = "External" Then
            pAngStart = -pBeta
        ElseIf POrient = "Internal" Then
            pAngStart = -mTheta(2) + pBeta
        End If
        pAngEnd = pAngStart + mTheta(2)

        DXF_Arcs_SymAxis(mY_Sym_Axis, pCen, pRadIn, pAngStart, pAngEnd)
        DXF_Arcs_SymAxis(mY_Sym_Axis, pCen, pRadOut, pAngStart, pAngEnd)


        '   Straight Edge.               
        '   -------------

        DXF_Lines_SymAxis(mY_Sym_Axis, mKP(1), mKP(2))
        DXF_Lines_SymAxis(mY_Sym_Axis, mKP(13), mKP(14))


    End Sub


End Class
