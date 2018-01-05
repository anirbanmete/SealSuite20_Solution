
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsCSeal                               '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  28APR11                                '
'                                                                              '
'===============================================================================


Imports System.Data.OleDb
Imports System.Math
Imports System.IO
Imports clsLibrary11
Imports System.Drawing

Partial Public Class IPE_clsCSeal
    Inherits IPE_clsSeal

    Private mY_Sym_Axis As Single       '....Symmetry Axis Location. 
    'Private mSW As StreamWriter        '....Stream Writer for DXF File
    Private mKP(2) As PointF            '.....Keypoints: CSeal.   


    Public Sub Create_DXF(ByVal FileName_In As String)
        '==================================================    

        'Try
        '    If File.Exists(FileName_In) Then _
        '        File.Delete(FileName_In)

        '    mSW = File.CreateText(FileName_In)

        '    '....HEADER.                        
        '    mSW.WriteLine("0")
        '    mSW.WriteLine("SECTION")
        '    mSW.WriteLine("2")
        '    mSW.WriteLine("ENTITIES")

        '....HEADER.   
        DXF_Header(FileName_In)

        '=======================================================
        '                  DXF CREATION DRAWING                '
        '=======================================================
        DrawCSeal_DXF()

        '....FOOTER.
        DXF_Footer()
        'mSW.WriteLine("0")
        'mSW.WriteLine("ENDSEC")
        'mSW.WriteLine("0")
        'mSW.WriteLine("EOF")

        ''....CLOSE FILE.
        'mSW.Close()

        ''....Message.                   'move to Form SG 18APR11
        'Dim pMsg As String
        'pMsg = ExtractPreData(gFiles.In_Title, ".") & ".DXF" & " file has been created successfully. "
        'MessageBox.Show(pMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

        'Catch ex As Exception
        '    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

    End Sub


    Private Sub DrawCSeal_DXF()
        '=====================
        'Center Point:
        '-------------
        If POrient = "External" Then
            mCen.X = 0.5 * (DControl + mHfree)
        ElseIf POrient = "Internal" Then
            mCen.X = 0.5 * (DControl - mHfree)
        End If

        mCen.Y = 0.0#
        mY_Sym_Axis = mCen.Y

        Dim pRadOut As Single = 0.5 * mHfree
        Dim pRadIn As Single = pRadOut - mT

        Dim pBeta As Single = 90 - 0.5 * mThetaOpening
        Dim pStartAng As Single, pEndAng As Single

        If POrient = "External" Then
            '-----------------------
            pStartAng = -90
            pEndAng = pBeta

        ElseIf POrient = "Internal" Then
            '---------------------------
            pStartAng = -pBeta
            pEndAng = 90
        End If

        DXF_Arcs_SymAxis(mY_Sym_Axis, mCen, pRadIn, pStartAng, pEndAng)
        DXF_Arcs_SymAxis(mY_Sym_Axis, mCen, pRadOut, pStartAng, pEndAng)


        '   Straight Edge Points: 
        '   ---------------------  

        '....Key Point 1.
        With mKP(1)
            If POrient = "External" Then
                .X = mCen.X + pRadIn * SinD(pBeta)
            ElseIf POrient = "Internal" Then
                .X = mCen.X - pRadIn * SinD(pBeta)
            End If
            .Y = mCen.Y - pRadIn * CosD(pBeta)
        End With

        '....Key Point 2.
        With mKP(2)
            If POrient = "External" Then
                .X = mCen.X + pRadOut * SinD(pBeta)
            ElseIf POrient = "Internal" Then
                .X = mCen.X - pRadOut * SinD(pBeta)
            End If
            .Y = mCen.Y - pRadOut * CosD(pBeta)
        End With

        DXF_Lines_SymAxis(mY_Sym_Axis, mKP(1), mKP(2))

    End Sub

End Class
