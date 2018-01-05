'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsDwg                                 '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18MAY16                                '
'                                                                              '
'=============================================================================== 

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Linq
Imports clsLibrary11


Public Class IPE_clsDwg

#Region "STRUCTURES:"

    Private Structure sRectBlock
        Public Wid As Single
        Public Ht As Single
        Public H1 As Single        '....Height of any subblock within the block
    End Structure

#End Region


#Region "ENUMERATION TYPE DECLARATIONS:"

    Public Enum eDrawingType
        Customer
        PreProduction
    End Enum

    Public Enum eBlockCornerName
        TopL = 1
        TopR = 2
        BotL = 3
        BotR = 4
    End Enum

    Private Enum eArrowType
        HorzArrowLeft = 1
        HorzArrowRight = 2
        VertArrowUp = 3
        VertArrowDown = 4
        DiagArrowDown = 5
    End Enum

#End Region


#Region "MEMBER VARIABLES:"
    Private mcFac_Rad As Single = Convert.ToSingle(Math.PI / 180.0F)

    Private mUserInfo As IPE_clsUser

    Private mGr As Graphics
    Private mDisplayType As String
    Private mSngLogoAspectRatio As Single

    '....Member variables corresponding the "Selected Case" for drawing:
    Private mUnitSel As IPE_clsUnit
    Private mCavitySel As IPE_clsCavity
    Private mMatSelName As String
    Private mSealSel As IPE_clsSeal
    Private mFilesSel As IPE_clsFile
    Private mMatSelCoating As String


    'Member Variables - Unexposed:
    '-------------------------------
    Private mDpX As Single      '....# of Pixels/in the X-Direction. 

    'Margins around the Picture Box are stored in the following array (in user unit).
    '....Index 1 & 2 : Left & Right
    '....Index 3 & 4 : Top & Bottom
    Private mMargin(4) As Single

#End Region


#Region "PROPERTY ROUTINES"

    'WRITE-ONLY PROPERTY:
    '-------------------

    Public WriteOnly Property Gr() As Graphics
        '=====================================

        Set(ByVal Value As Graphics)
            '-----------------------
            mGr = Value
        End Set

    End Property


    Public WriteOnly Property DisplayType() As String
        '============================================

        Set(ByVal strValue As String)
            '------------------------
            mDisplayType = strValue
        End Set

    End Property


    Public WriteOnly Property SngLogoAspectRatio() As Single
        '===================================================

        Set(ByVal SngValue As Single)
            '------------------------
            mSngLogoAspectRatio = SngValue
        End Set

    End Property


    'Assignment of the Member variables corresponding the "Selected Case" for drawing:
    '---------------------------------------------------------------------------------

    Public WriteOnly Property UnitSelSystem() As String
        '==============================================

        Set(ByVal strData As String)
            '-----------------------
            mUnitSel.System = strData
        End Set

    End Property


    Public WriteOnly Property CavitySel() As IPE_clsCavity
        '=============================================

        Set(ByVal Obj As IPE_clsCavity)
            '----------------------
            mCavitySel = Obj
        End Set

    End Property


    Public WriteOnly Property MatSelName() As String
        '===========================================

        Set(ByVal strValue As String)
            '------------------------
            mMatSelName = strValue
        End Set
    End Property


    Public WriteOnly Property SealSel() As IPE_clsSeal
        '=========================================

        Set(ByVal Obj As IPE_clsSeal)
            '--------------------
            mSealSel = Obj
        End Set
    End Property


    Public WriteOnly Property FilesSel() As IPE_clsFile
        '===========================================

        Set(ByVal Obj As IPE_clsFile)
            '---------------------
            mFilesSel = Obj
        End Set
    End Property


    Public WriteOnly Property UserInfo() As IPE_clsUser
        '==============================================

        Set(ByVal Obj As IPE_clsUser)
            '--------------------------
            mUserInfo = Obj
        End Set

    End Property


    Public WriteOnly Property MatSelCoating() As String
        '==============================================     

        Set(ByVal strValue As String)
            '------------------------
            mMatSelCoating = strValue
        End Set
    End Property

#End Region


#Region "CONSTRUCTOR:"

    '....Class Constructor.
    Public Sub New(Optional ByVal strUnitSystem_In As String = "English")
        '================================================================

        Try
            mUnitSel = New IPE_clsUnit(strUnitSystem_In)
            mCavitySel = Nothing
            mSealSel = Nothing
        Catch
            MsgBox(Err.Description)
        End Try

    End Sub

#End Region


#Region "PROPOSAL DRAWING ROUTINES"

    Public Sub DisplayDrawing(ByVal Result_Sel_In As Integer, ByVal PreProductionExist_In As Boolean, ByVal CavityExists_In As Boolean, ByVal Project_In As IPE_clsProject)
        '============================================================================================================================
        'This routine displays the "gSealSel" object drawing on a picture box or
        '....prints a hard copy on a printer.

        mGr.PageUnit = GraphicsUnit.Inch
        mDpX = mGr.DpiX     '....# of Pixels/in on the current Graphics Object.

        'Border lines.
        '-------------
        Dim pBorderTopL As PointF, pborderBotR As PointF

        '....Furthermore get the top left & bottom right corner points of the border.
        DrawBorder(pBorderTopL, pborderBotR)


        'Proprietary Note Block.
        '-----------------------
        Dim pCornerName As eBlockCornerName
        Dim pCornerPoint As PointF

        '....The following variable - 'pCornerName' not used anywhere.
        '........Assignment done here for the sake of better readability.
        '
        pCornerName = eBlockCornerName.TopR

        With pCornerPoint
            .X = pborderBotR.X
            .Y = pBorderTopL.Y
        End With

        DrawProprietaryNotesBlock(eBlockCornerName.TopR, pCornerPoint)

        With pCornerPoint
            .X = pborderBotR.X - 2.5
            .Y = pBorderTopL.Y + 1
        End With

        If (PreProductionExist_In) Then
            DrawPreProductionDesignTable(Result_Sel_In, eBlockCornerName.TopR, pCornerPoint, Project_In)
        End If

        'Title Block.
        '------------
        Dim pTitleBlockTopL As PointF
        pCornerName = eBlockCornerName.BotR

        With pCornerPoint
            .X = pborderBotR.X
            .Y = pborderBotR.Y
        End With

        DrawTitleBlock(eBlockCornerName.BotR, pCornerPoint, pTitleBlockTopL)


        'Proposal Information.
        '--------------------
        pCornerName = eBlockCornerName.BotL

        With pCornerPoint
            .X = pBorderTopL.X
            .Y = pborderBotR.Y
        End With

        DrawProposalInfo(Result_Sel_In, eBlockCornerName.BotL, pCornerPoint, Project_In)


        'Seal Design Data Table.
        '-----------------------
        Dim dataTableBotR As PointF
        pCornerName = eBlockCornerName.TopL

        With pCornerPoint
            .X = pBorderTopL.X
            .Y = pBorderTopL.Y
        End With

        DrawSealDesignDataTable(CavityExists_In, eBlockCornerName.TopL, pCornerPoint, dataTableBotR, Project_In)


        'CAVITY.
        ''-------
        Dim cavityBotR As PointF
        pCornerName = eBlockCornerName.TopL

        With pCornerPoint
            .X = pBorderTopL.X + 3.5
            .Y = pBorderTopL.Y + 0.5
        End With

        DrawCavity(CavityExists_In, pCornerName, pCornerPoint, cavityBotR)


        'E-Seal Drawings:
        '================
        Dim drawBoxTopL As PointF, drawBoxBotR As PointF


        'End view.
        '---------
        '....Establish the drawing box dimensions within which
        '........the end view will be drawn.

        With drawBoxTopL
            .X = cavityBotR.X
            .Y = dataTableBotR.Y
        End With

        With drawBoxBotR
            .X = pborderBotR.X
            .Y = pTitleBlockTopL.Y
        End With

        DrawSealEndView(drawBoxTopL, drawBoxBotR)


        'Cross-section.
        '-------------
        '....Establish the drawing box dimensions within which
        '........the end view will be drawn.

        With drawBoxTopL
            .X = pBorderTopL.X
            .Y = dataTableBotR.Y
        End With

        With drawBoxBotR
            .X = cavityBotR.X
            .Y = pTitleBlockTopL.Y
        End With

        DrawSealCrossSec(pBorderTopL, pborderBotR, drawBoxTopL, drawBoxBotR)

    End Sub


    Public Sub DrawBorder(ByRef borderTopL_out As PointF, ByRef borderBotR_out As PointF)
        '=================================================================================

        'This routine draws the thick border margin lines all around the drawing page 
        '....and places a reference coordinate system for drawing sheet at the upper LHS
        '....corner of the border margin.

        '....Output parameters :  borderTopL_out, borderBotR_out

        Const pcBORDER_MARGIN As Single = 0.25


        'PRINTER:
        '-------
        'The following two dimensions are automatically determined when
        'the papersize & orientation are selected. The corresponding values are
        'given below for reference purpose.
        '
        'Printer.ScaleHeight = 8.133
        'Printer.ScaleWidth = 10.6

        'The corresponding orgin location for the default coordinate system has been
        'found to be (0.25,0.1875) from the LHS top paper corner.
        '
        'The X axis is at the above origin along the longer edge from left to right.
        'The Y axis is at the above origin along the shorter edge from top to bottom.


        'Pen Width.
        '----------
        Dim pPenWid As Int16    '....in Pixels
        If mDisplayType = "Printer" Then
            pPenWid = 8

        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
            '.ScaleHeight = 6.933
            '.ScaleWidth = 14
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        'Establish Corner Points Coordinates:
        '-----------------------------------
        '
        '....Top LHS Corner Point:
        With borderTopL_out

            If mDisplayType = "Printer" Then
                .X = pcBORDER_MARGIN - 0.125
                .Y = 0.35  '....Needs extra margin at the top for punch holes.
                '          '....Gives rise to 0.35 + 0.1875 = 0.5375" from the top edge.

            ElseIf mDisplayType = "PictureBox" Then
                .X = pcBORDER_MARGIN
                .Y = 0.25 '0.35
            End If

        End With


        If mDisplayType = "Printer" Then
            '....Bottom RHS Corner Point:
            With borderBotR_out
                .X = (10.6 - pcBORDER_MARGIN)
                .Y = (8.133 - pcBORDER_MARGIN)
            End With


        ElseIf mDisplayType = "PictureBox" Then

            With borderBotR_out
                .X = (8.8 - pcBORDER_MARGIN)
                .Y = (6.3 - pcBORDER_MARGIN)
            End With
        End If


        'Border Rectangle Size.
        '---------------------
        Dim pSize As SizeF
        With pSize
            .Width = borderBotR_out.X - borderTopL_out.X
            .Height = borderBotR_out.Y - borderTopL_out.Y
        End With


        'Draw the border all around.
        '--------------------------
        mGr.DrawRectangle(pPen, borderTopL_out.X, borderTopL_out.Y, _
                          pSize.Width, pSize.Height)

    End Sub


    Private Sub DrawProprietaryNotesBlock(ByVal cornerName_in As eBlockCornerName, _
                                          ByVal cornerPoint_in As PointF)
        '============================================================================
        'This routine creates the 'Proprietary notes' block.

        'Input  parameters :  cornerName_in, cornerPoint_in.
        '                     The last two input will define the placement of the
        '                     block w.r.t the border.
        'Output parameters :  None


        'Initialize the block dimensions (inches):
        '----------------------------------------
        Dim pcPROPRIETARY_NOTES_BLOCK As sRectBlock

        If mDisplayType = "Printer" Then

            With pcPROPRIETARY_NOTES_BLOCK
                .Wid = 2.8
                .Ht = 0.94
                .H1 = 0.1875
            End With


        ElseIf mDisplayType = "PictureBox" Then

            With pcPROPRIETARY_NOTES_BLOCK
                .Wid = 2.45
                .Ht = 0.9
                .H1 = 0.15
            End With

        End If


        'Pen width.
        '----------
        Dim pPenWid As Int16        '....in Pixels.
        If mDisplayType = "Printer" Then
            pPenWid = 10
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        'Block rectangle:
        '---------------
        Dim pBlockTopL As PointF, pBlockBotR As PointF

        '....Top left corner of the block.
        If cornerName_in = eBlockCornerName.TopL Then
            '....It will be done later, as needed.

        ElseIf cornerName_in = eBlockCornerName.TopR Then
            With pBlockTopL
                .X = cornerPoint_in.X - pcPROPRIETARY_NOTES_BLOCK.Wid
                .Y = cornerPoint_in.Y
            End With

        ElseIf cornerName_in = eBlockCornerName.BotL Then
            '....May be done later

        ElseIf cornerName_in = eBlockCornerName.BotR Then
            '....May be done later.
        End If


        '....Bottom right corner of the block.
        With pBlockBotR
            .X = pBlockTopL.X + pcPROPRIETARY_NOTES_BLOCK.Wid
            .Y = pBlockTopL.Y + pcPROPRIETARY_NOTES_BLOCK.Ht
        End With


        Dim pSize As SizeF
        With pSize
            .Width = pBlockBotR.X - pBlockTopL.X
            .Height = pBlockBotR.Y - pBlockTopL.Y
        End With


        '....Draw the Block rectangle.
        mGr.DrawRectangle(pPen, pBlockTopL.X, pBlockTopL.Y, pSize.Width, pSize.Height)


        'Draw the heading line
        '---------------------
        Dim pY_H1 As Single
        pY_H1 = pBlockTopL.Y + pcPROPRIETARY_NOTES_BLOCK.H1
        mGr.DrawLine(pPen, New PointF(pBlockTopL.X, pY_H1), _
                           New PointF(pBlockBotR.X, pY_H1))


        'Write the heading
        '------------------
        Dim pFontSize As Single
        Dim pPt As PointF

        If mDisplayType = "Printer" Then
            pPt.X = pBlockBotR.X - 1.9
            pPt.Y = pBlockTopL.Y + 0.02
            pFontSize = 10

        ElseIf mDisplayType = "PictureBox" Then
            pPt.X = pBlockBotR.X - 1.9 '2.7
            pPt.Y = pBlockTopL.Y
            pFontSize = 8
        End If

        Dim pFont As Font
        pFont = New Font("Arial", pFontSize, FontStyle.Bold)

        mGr.DrawString("PROPRIETARY", pFont, Brushes.Black, pPt)
        pFont = Nothing


        'Write Proprietary Notes.
        '------------------------
        Dim pNotes(8) As String

        pNotes(0) = "THIS DOCUMENT CONTAINS INFORMATION THAT IS CONFIDENTIAL " & _
                    "AND PROPRIETARY"
        pNotes(1) = "TO THE ADVANCED PRODUCTS BUSINESS UNIT, COMPOSITE SEALING " & _
                    "SYSTEMS"
        pNotes(2) = "DIVISION, PARKER HANNIFIN CORPORATION (""PARKER""). " & _
                    "THIS DOCUMENT IS"
        pNotes(3) = "FURNISHED ON THE UNDERSTANDING THAT THE DOCUMENT AND THE " & _
                    "INFORMATION"
        pNotes(4) = "IT CONTAINS WILL NOT BE COPIED OR DISCLOSED TO OTHERS OR " & _
                    "USED FOR ANY"
        pNotes(5) = "PURPOSE OTHER THAN CONDUCTING BUSINESS WITH PARKER, AND WILL BE "
        pNotes(6) = "RETURNED AND ALL FURTHER USE DISCONTINUED UPON REQUEST BY " & _
                    "PARKER."
        pNotes(7) = "COPYRIGHT PARKER. YEAR OF COPYRIGHT IS FIRST YEAR INDICATED " & _
                    "ON THIS "
        pNotes(8) = "DOCUMENT. ALL RIGHT RESERVED. "


        If mDisplayType = "Printer" Then
            pFontSize = 4.4
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 4.0#
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        Dim pXText As Single
        pXText = pBlockTopL.X + 0.05

        Dim pYInc As Single
        pYInc = 0.08

        '....Initialize.
        Dim pYText As Single
        pYText = pY_H1 + 0.02

        Dim iLine As Integer

        For iLine = 1 To 9
            pPt.X = pXText
            pPt.Y = pYText
            mGr.DrawString(pNotes(iLine - 1), pFont, Brushes.Black, pPt)

            pYText = pYText + pYInc
        Next

        pFont = Nothing

    End Sub


    Private Sub DrawTitleBlock(ByVal cornerName_in As eBlockCornerName, _
                               ByVal cornerPoint_in As PointF, _
                               ByRef blockTopL_out As PointF)
        '======================================================================
        'This routine creates the 'Title' block - RHS bottom rectangle.

        'Input  parameters :  cornerName_in, cornerPoint_in.
        '                     The last two input will define the placement of the
        '                     block w.r.t the border.
        'Output parameters :  blockTopL_out


        'Initialize the block dimensions (inches) & Line width.
        '------------------------------------------------------
        Dim pcTITLE_BLOCK As sRectBlock

        If mDisplayType = "Printer" Then

            With pcTITLE_BLOCK
                .Wid = 3.1
                .Ht = 1.4375
                .H1 = 0.4
            End With


        ElseIf mDisplayType = "PictureBox" Then

            With pcTITLE_BLOCK
                .Wid = 2.52
                .Ht = 1.37
                .H1 = 0.3
            End With

        End If


        'Pen width.
        '----------
        Dim pPenWid As Int16        '....in Pixels.
        If mDisplayType = "Printer" Then
            pPenWid = 10
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        'Draw the block rectangle
        '------------------------
        Dim pblockTopL As PointF, pblockBotR As PointF

        '....Top LHS Corner.
        '
        If cornerName_in = eBlockCornerName.BotR Then
            '....Bottom right corner location of the block given.
            With pblockTopL
                .X = cornerPoint_in.X - pcTITLE_BLOCK.Wid
                .Y = cornerPoint_in.Y - pcTITLE_BLOCK.Ht
            End With
        End If

        blockTopL_out = pblockTopL        '....Output parameter. 


        '....Bottom RHS Corner.
        '
        With pblockBotR
            .X = pblockTopL.X + pcTITLE_BLOCK.Wid
            .Y = pblockTopL.Y + pcTITLE_BLOCK.Ht
        End With


        '....Size of the Rectangle.
        Dim pSize As SizeF
        With pSize
            .Width = pblockBotR.X - pblockTopL.X
            .Height = pblockBotR.Y - pblockTopL.Y
        End With


        '....Draw the Block rectangle.
        mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, pSize.Width, pSize.Height)


        'Draw the heading line
        '---------------------
        Dim pY_H1 As Single
        pY_H1 = pblockTopL.Y + pcTITLE_BLOCK.H1
        mGr.DrawLine(pPen, New PointF(pblockTopL.X, pY_H1), New PointF(pblockBotR.X, pY_H1))



        'Draw the Company Logo.
        '----------------------
        '
        '....Set Rectangle within which Logo will be placed.
        Dim pRect As RectangleF
        With pRect
            .X = pblockTopL.X
            .Y = pblockTopL.Y + 0.015
            .Height = pcTITLE_BLOCK.H1 - 0.03
            .Width = .Height * mSngLogoAspectRatio
        End With

        '....Draw.
        Dim pImg As Image = Image.FromFile(mFilesSel.Logo)
        mGr.DrawImage(pImg, pRect)


        'Write the Heading Texts:
        '------------------------
        '
        Dim pPt As PointF
        pPt.X = pblockTopL.X + 1.14
        pPt.Y = pblockTopL.Y + 0.05     '....First Line 

        Dim pYInc As Single
        pYInc = 0.16                    '....Increment for the Second Line.

        Dim pFontSize As Single
        If mDisplayType = "Printer" Then
            pFontSize = 6.365

        ElseIf mDisplayType = "PictureBox" Then
            pPt.X = pPt.X - 0.3
            pFontSize = 5.5
        End If

        Dim pFont As Font
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        Dim pTitleText(2) As String
        pTitleText(0) = "THE ADVANCED PRODUCTS BUSINESS UNIT"
        pTitleText(1) = "COMPOSITE SEALING SYSTEMS DIVISION"

        Dim iLine As Integer
        For iLine = 1 To 2
            mGr.DrawString(pTitleText(iLine - 1), pFont, Brushes.Black, pPt)
            pPt.Y = pPt.Y + pYInc
        Next

        pFont = Nothing


        'Write Texts inside:
        '==================
        '
        '   First Line:
        '    ----------
        '
        '....TITLE.
        With pPt
            .X = pblockTopL.X + 0.05
            .Y = pblockTopL.Y + 0.43
        End With

        If mDisplayType = "Printer" Then
            pFontSize = 10
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 10
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("TITLE", pFont, Brushes.Black, pPt)


        '....*-TYPE SEAL.
        Dim pXInc As Single
        pXInc = 0.85

        Dim pPtCur As PointF
        With pPtCur
            .X = pPt.X + pXInc
            .Y = pPt.Y
        End With

        If mDisplayType = "Printer" Then
            pFontSize = 13
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 12
        End If
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)


        Dim pstrText As String = ""
        If mSealSel.Type = "E-Seal" Then
            pstrText = "E-TYPE SEAL, "
        ElseIf mSealSel.Type = "C-Seal" Then
            pstrText = "C-TYPE SEAL, "
        ElseIf mSealSel.Type = "U-Seal" Then
            pstrText = "U-TYPE SEAL, "
        End If

        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtCur)


        '   Second Line.
        '   ------------   
        '
        pXInc = 0.49
        pYInc = 0.23
        With pPtCur
            .X = pPt.X + pXInc
            .Y = pPt.Y + pYInc
        End With

        If mDisplayType = "Printer" Then
            pFontSize = 12
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 11
        End If
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = UCase$(mSealSel.POrient) & " PRESSURE"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtCur)


        '   Third Line.
        '   ----------
        '
        pXInc = 0.4
        With pPtCur
            .X = pPt.X + pXInc
            .Y = .Y + pYInc + 0.035
        End With

        If mDisplayType = "Printer" Then
            pFontSize = 12
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 10
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("UNRELEASED PROPOSAL", pFont, Brushes.Black, pPtCur)


        '   Fourth Line.
        '   ------------
        'pXInc = 0.3
        With pPtCur
            '.X = pPt.X + pXInc
            .Y = pPtCur.Y + pYInc
        End With

        If mDisplayType = "Printer" Then
            pFontSize = 12
            pPtCur.X = pPt.X + 0.3

        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 10.5
            pPtCur.X = pPt.X + 0.2
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "Reference  CrossSection " & mSealSel.MCrossSecNo
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtCur)


        '   Fifth Line.
        '   -----------
        With pPtCur
            .X = pPt.X + 1
            .Y = .Y + pYInc + 0.05
        End With

        If mDisplayType = "Printer" Then
            pFontSize = 10
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 8
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        pstrText = " SCALE : NONE "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtCur)


        'Tolerance Note Block - English
        '------------------------------
        Dim pCornerName As eBlockCornerName
        pCornerName = eBlockCornerName.TopR

        Dim pCornerPoint As PointF
        With pCornerPoint
            .X = pblockTopL.X
            .Y = pblockTopL.Y
        End With


        '....Location Variables of the next Adjacent Blocks.
        '
        '........TolBlockMetric:
        Dim pCornerName_TolBlockMetric As eBlockCornerName
        Dim pCornerPt_TolBlockMetric As PointF
        pCornerName_TolBlockMetric = eBlockCornerName.BotR

        '........CodeBlock:
        Dim pCornerName_CodeBlock As eBlockCornerName
        Dim pCornerPt_CodeBlock As PointF
        pCornerName_CodeBlock = eBlockCornerName.TopR


        '....Draw the block and get the location of the next adjacent blocks.    
        DrawTolBlockEng(pCornerName, pCornerPoint, _
                        pCornerName_TolBlockMetric, pCornerPt_TolBlockMetric, _
                        pCornerName_CodeBlock, pCornerPt_CodeBlock)


        'Tolerance Note Block - Metric
        '------------------------------
        DrawTolBlockMetric(pCornerName_TolBlockMetric, pCornerPt_TolBlockMetric)


        'Code Ident. Block
        '-----------------
        DrawCodeBlock(pCornerName_CodeBlock, pCornerPt_CodeBlock)

    End Sub


    Private Sub DrawTolBlockEng(ByVal cornerName_in As eBlockCornerName, _
                                ByVal cornerPoint_in As PointF, _
                                ByRef cornerName_TolBlockMetric_out As eBlockCornerName, _
                                ByRef cornerPoint_TolBlockMetric_out As PointF, _
                                ByRef cornerName_CodeBlock_out As eBlockCornerName, _
                                ByRef cornerPoint_CodeBlock_out As PointF)
        '=================================================================================
        'This routine creates the 'Tolerance block - English' on the left hand side of
        '.... the Title Block.

        'Input  parameters :  cornerName_in, cornerPoint_in.
        '                     These two input will define the placement of the block.
        'Output parameters :  None


        'Draw the Block Rectangle:
        '------------------------

        '....Initialize the block dimensions (inches).
        Dim pcTOL_BLOCK_ENG As sRectBlock

        If mDisplayType = "Printer" Then
            With pcTOL_BLOCK_ENG
                .Wid = 1.35
                .Ht = 1.4375            '....Same as TitleBlock
            End With

        ElseIf mDisplayType = "PictureBox" Then
            With pcTOL_BLOCK_ENG
                .Wid = 1.35  '2.2
                .Ht = 1.37 '375         '....Same as TitleBlock
            End With

        End If


        '....Pen width.
        Dim pPenWid As Int16        '....in Pixels.
        If mDisplayType = "Printer" Then
            pPenWid = 10
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        '....Rectangle Location.
        Dim pblockTopL As PointF

        If cornerName_in = eBlockCornerName.TopR Then
            With pblockTopL
                .X = cornerPoint_in.X - pcTOL_BLOCK_ENG.Wid
                .Y = cornerPoint_in.Y
            End With
        End If

        '....Draw the Rectangle.
        mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, _
                          pcTOL_BLOCK_ENG.Wid, pcTOL_BLOCK_ENG.Ht)


        'Write Texts: 
        '------------

        '....Pen width.
        If mDisplayType = "Printer" Then
            pPenWid = 2
        End If
        pPen = New Pen(Color.Black, pPenWid / mDpX)


        Dim xText As Single, yText As Single
        Dim xIncr As Single, x1Incr As Single, x2Incr As Single
        Dim yIncr As Single, y1Incr As Single, y2Incr As Single, y3Incr As Single

        xIncr = 0.02 : x1Incr = 0.2 : x2Incr = 0.4
        yIncr = 0.1 : y1Incr = 0.13 : y2Incr = 0.16 : y3Incr = 0.04

        '   First Line.         
        '   -----------
        xText = pblockTopL.X + 0.04
        yText = pblockTopL.Y + 0.02

        Dim pFontSize As Single = 6
        Dim pFont As Font
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)


        Dim pPt As PointF
        With pPt
            .X = xText
            .Y = yText
        End With

        Dim pstrText As String
        pstrText = "UNLESS  OTHERWISE  NOTED:"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Second Line.            
        '   ------------
        xText = pblockTopL.X + 0.04
        yText = pblockTopL.Y + 0.2

        With pPt
            .X = xText
            .Y = yText
        End With

        pFontSize = 5.2
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "ALL DIMENSIONS ARE IN INCHES"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Third Line.
        '   -----------
        xText = pblockTopL.X + 0.04
        yText = yText + yIncr

        With pPt
            .X = xText
            .Y = yText
        End With

        pFontSize = 5.2 '4.9
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "INTERPRET DWG PER MIL-STD-100"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Fourth Line.    
        '   ------------
        xText = pblockTopL.X + 0.04
        yText = yText + 0.2

        With pPt
            .X = xText
            .Y = yText
        End With

        pFontSize = 6
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "SURFACE TEXTURE"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        Dim pBotPoint As PointF
        xText = xText + 2.2 * x2Incr
        yText = yText - y3Incr

        With pPt
            .X = xText
            .Y = yText
        End With

        With pBotPoint
            .X = xText + 0.1
            .Y = yText + 0.15
        End With

        pFontSize = 5.2
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        mGr.DrawString("125", pFont, Brushes.Black, pPt)
        DrawAngle(pBotPoint)


        '   Fifth Line.
        '   -----------
        xText = pblockTopL.X + 0.2
        yText = yText + 0.25

        With pPt
            .X = xText
            .Y = yText
        End With

        pFontSize = 6
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "TOLERANCES  ARE:"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        xText = pblockTopL.X + 0.4
        yText = yText + y2Incr

        With pPt
            .X = xText
            .Y = yText
        End With

        pFontSize = 5.2
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("ANGLES", pFont, Brushes.Black, pPt)


        If mDisplayType = "PictureBox" Then
            pFontSize = 6
        End If

        xText = pblockTopL.X + 0.9
        yText = yText

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("    2'", pFont, Brushes.Black, pPt)


        '   Sixth Line.
        '   ------------
        If mDisplayType = "Printer" Then
            xText = pblockTopL.X + 0.1
            yText = yText + 0.13

        ElseIf mDisplayType = "PictureBox" Then
            xText = pblockTopL.X + 0.1
            yText = yText + 0.16
        End If

        With pPt
            .X = xText
            .Y = yText
        End With

        mGr.DrawString("DECIMALS:", pFont, Brushes.Black, pPt)

        xText = pblockTopL.X + 0.65
        yText = yText

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = ".XX  =  "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        If mDisplayType = "Printer" Then
            xText = pblockTopL.X + 0.9
            yText = yText

        ElseIf mDisplayType = "PictureBox" Then
            xText = pblockTopL.X + 0.9 '1.4
            yText = yText
        End If

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        pstrText = "  .0100"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Seventh Line.
        '   --------------
        xText = pblockTopL.X + 0.6
        yText = yText + yIncr

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = ".XXX  =  "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        If mDisplayType = "Printer" Then
            xText = pblockTopL.X + 0.9
            yText = yText
        ElseIf mDisplayType = "PictureBox" Then
            xText = pblockTopL.X + 0.9 '1.4
            yText = yText
        End If

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)


        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        pstrText = "  .0050"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Eighth Line.
        '   ------------
        xText = pblockTopL.X + 0.545
        yText = yText + yIncr + 0.005

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = ".XXXX  =  "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        xText = pblockTopL.X + 0.9
        yText = yText

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        pstrText = "  .0005"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        'Location of the Adjacent Blocks:
        '-------------------------------
        '....Tolerance Note Block - Metric
        cornerName_TolBlockMetric_out = eBlockCornerName.BotR

        With cornerPoint_TolBlockMetric_out
            .X = pblockTopL.X
            .Y = pblockTopL.Y + pcTOL_BLOCK_ENG.Ht
        End With


        '....Code Ident. Block
        cornerName_CodeBlock_out = eBlockCornerName.TopR

        With cornerPoint_CodeBlock_out
            .X = pblockTopL.X
            .Y = pblockTopL.Y
        End With

    End Sub


    Private Sub DrawTolBlockMetric(ByVal cornerName_in As eBlockCornerName, _
                                   ByVal cornerPoint_in As PointF)
        '===========================================================================
        'Input  parameters :  cornerName_in, cornerPoint_in
        '                     The last two inputs will define the placement of the block.
        'Output parameters :  None


        'Draw the Block Rectangle:
        '------------------------

        '....Initialize the block dimensions (inches).
        Dim pcTOL_BLOCK_METRIC As sRectBlock

        '....Line width.
        Dim pPenWid As Single    '....in Pixels

        If mDisplayType = "Printer" Then
            With pcTOL_BLOCK_METRIC
                .Wid = 1.25
                .Ht = 0.45
            End With

            pPenWid = 10


        ElseIf mDisplayType = "PictureBox" Then
            With pcTOL_BLOCK_METRIC
                .Wid = 1.2 '1.98
                .Ht = 0.45
            End With

            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        '....Block location.
        Dim pblockTopL As PointF, pblockBotR As PointF

        If cornerName_in = eBlockCornerName.BotR Then        '....Bottom Right Corner
            With pblockBotR
                .X = cornerPoint_in.X
                .Y = cornerPoint_in.Y
            End With
        End If


        '....Top Left Corner
        With pblockTopL
            .X = pblockBotR.X - pcTOL_BLOCK_METRIC.Wid
            .Y = pblockBotR.Y - pcTOL_BLOCK_METRIC.Ht
        End With

        '....Draw the Block rectangle.
        mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, _
                          pcTOL_BLOCK_METRIC.Wid, pcTOL_BLOCK_METRIC.Ht)


        'Texts inside the Block :
        '------------------------
        Dim xText As Single, yText As Single, xIncr As Single, yIncr As Single

        xIncr = 0.55
        yIncr = 0.1

        '   First Line:                 
        '   ------------
        xText = pblockTopL.X + 0.04
        yText = pblockTopL.Y + 0.02

        Dim pPt As PointF
        With pPt
            .X = xText
            .Y = yText
        End With

        Dim pFontSize As Single = 6
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Regular)

        mGr.DrawString("TOLERANCES  CONTINUED:", pFont, Brushes.Black, pPt)


        '   Second Line:
        '   ------------
        xText = pblockTopL.X + 0.2
        yText = yText + 0.12

        With pPt
            .X = xText
            .Y = yText
        End With

        pFontSize = 5.2
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        mGr.DrawString("MM", pFont, Brushes.Black, pPt)

        xText = pblockTopL.X + 0.5

        With pPt
            .X = xText
            .Y = yText
        End With

        mGr.DrawString("X.X  =  ", pFont, Brushes.Black, pPt)

        xText = pblockTopL.X + 0.75
        yText = yText

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("  0.25", pFont, Brushes.Black, pPt)


        '   Third Line:
        '   -----------
        xText = pblockTopL.X + 0.45
        yText = yText + yIncr

        With pPt
            .X = xText
            .Y = yText
        End With

        mGr.DrawString("X.XX  =  ", pFont, Brushes.Black, pPt)

        xText = pblockTopL.X + 0.75

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("  0.03", pFont, Brushes.Black, pPt)


        '   Fourth Line:
        '   ------------
        xText = pblockTopL.X + 0.4
        yText = yText + yIncr

        With pPt
            .X = xText
            .Y = yText
        End With

        mGr.DrawString("X.XXX  =  ", pFont, Brushes.Black, pPt)

        If mDisplayType = "Printer" Then
            xText = pblockTopL.X + 0.75
        ElseIf mDisplayType = "PictureBox" Then
            xText = pblockTopL.X + 0.75 '1
        End If

        With pPt
            .X = xText
            .Y = yText
        End With

        pFont = New Font("Arial", pFontSize, FontStyle.Underline)
        mGr.DrawString("+", pFont, Brushes.Black, pPt)

        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("  0.003", pFont, Brushes.Black, pPt)

    End Sub


    Private Sub DrawCodeBlock(ByVal cornerName_in As eBlockCornerName, _
                              ByVal cornerPoint_in As PointF)
        '==================================================================
        'This routine draw the Code Ident. Block.


        'Draw the Block Rectangle:
        '------------------------

        '....Initialize the block dimensions (inches). 
        Dim pcCODE_BLOCK As sRectBlock

        '....Line Width.
        Dim pPenWid As Single    '....in Pixels

        If mDisplayType = "Printer" Then
            With pcCODE_BLOCK
                .Wid = 1.25
                .Ht = 0.9875
            End With

            pPenWid = 10

        ElseIf mDisplayType = "PictureBox" Then
            With pcCODE_BLOCK
                .Wid = 1.2
                .Ht = 0.92
            End With

            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        '....Block location.
        Dim pblockTopL As PointF

        If cornerName_in = eBlockCornerName.TopR Then
            With pblockTopL
                .X = cornerPoint_in.X - pcCODE_BLOCK.Wid
                .Y = cornerPoint_in.Y
            End With
        End If

        mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, _
                          pcCODE_BLOCK.Wid, pcCODE_BLOCK.Ht)


        'Text on Code Block
        '------------------
        Dim xText As Single, yText As Single, xIncr As Single, yIncr As Single

        xIncr = 0.1
        yIncr = 0.1

        '   First Line.
        '   -----------
        xText = pblockTopL.X + 0.075
        yText = pblockTopL.Y + 0.3

        Dim ppt As PointF
        With ppt
            .X = xText
            .Y = yText
        End With

        Dim pFontSize As Single = 9
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Regular)

        mGr.DrawString("CODE  IDENT. NO.", pFont, Brushes.Black, ppt)


        '   Second Line.
        '   ------------
        xText = xText + 0.26
        yText = yText + 0.25

        With ppt
            .X = xText
            .Y = yText
        End With

        pFontSize = 12
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("04319", pFont, Brushes.Black, ppt)

    End Sub


    Private Sub DrawProposalInfo(ByVal Result_Sel_In As Integer, ByVal cornerName_in As eBlockCornerName, _
                                 ByVal cornerPoint_in As PointF, ByVal Project_In As IPE_clsProject)
        '============================================================================
        'This routine creates  'Proposal Information' - near the bottom LHS corner.
        '
        'Input  parameters :  corner_Name, cornerPoint.
        '                     The last two input will define the placement of the
        '                     block w.r.t the border.
        'Output parameters :   None


        'Set Top Left Corner of the Proposal Block.
        '------------------------------------------
        Dim pcPROPOSAL_INFO As PointF   '....Location w.r.t the BotL

        With pcPROPOSAL_INFO
            .X = 0.2
            .Y = 0.9
        End With

        Dim blockTopL As PointF

        If cornerName_in = eBlockCornerName.BotL Then
            With blockTopL
                .X = cornerPoint_in.X + pcPROPOSAL_INFO.X
                .Y = cornerPoint_in.Y - pcPROPOSAL_INFO.Y
            End With
        End If


        'Write Proposal Information:
        '--------------------------
        Dim xText As Single, yText As Single

        xText = blockTopL.X
        yText = blockTopL.Y

        Dim xIncr As Single, yIncr As Single
        xIncr = 0.8         '....Position of the ":"
        yIncr = 0.2


        '   First Line.
        '   -----------
        Dim pPt As PointF
        With pPt
            .X = xText
            .Y = yText
        End With

        Dim pFontSize As Single = 8
        Dim pFont As New Font("Calibri", pFontSize, FontStyle.Regular)

        Dim pstrText As String = "Prepared by"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        Dim pPtTab1 As PointF
        pPtTab1 = pPt
        pPtTab1.X = pPt.X + xIncr

        pstrText = ":" & Space(3) & mUserInfo.Name
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtTab1)


        '   Second Line.
        '   ------------
        With pPt
            .X = xText
            .Y = .Y + yIncr
        End With
        pPtTab1.Y = pPt.Y

        pstrText = "Phone No"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pstrText = ":" & Space(3) & mUserInfo.PhoneNo
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtTab1)


        '   Third Line.
        '   -----------
        With pPt
            .X = xText
            .Y = .Y + yIncr
        End With
        pPtTab1.Y = pPt.Y

        pstrText = "Analysis Desc     : "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        Dim pAnaDesc As String = ""
        'Dim pISel As Integer = Result_Sel_In
        Dim pMCS As String = mSealSel.MCrossSecNo
        If (pMCS <> "") Then
            pAnaDesc = "MCS" & pMCS
        End If
        Dim pFreeHt As String = mSealSel.Hfree.ToString("#0.000").Replace(".", "")
        If (pFreeHt <> "") Then
            pAnaDesc = pAnaDesc & "_FH" & pFreeHt
        End If
        Dim pLoadCaseName As String = Project_In.Analysis(Result_Sel_In).LoadCase.Name 'mSealSel.LoadCase.ToString()
        If (pLoadCaseName <> "") Then
            pAnaDesc = pAnaDesc & "_" & pLoadCaseName
        End If
        Dim pDate As String = ""
        Dim pTime As String = ""
        If (Project_In.Analysis(Result_Sel_In).DateCreated <> Date.MinValue) Then
            pDate = Project_In.Analysis(Result_Sel_In).DateCreated.ToString("ddMMMyy")
            pTime = Project_In.Analysis(Result_Sel_In).TimeCreated.ToString("t").Replace(":", "").Trim().Replace(" ", "")
            pAnaDesc = pAnaDesc & "_" & pDate & "_" & pTime
        End If

        ''pstrText = ":" & Space(3) & Project_In.Name
        mGr.DrawString(Space(3) & pAnaDesc, pFont, Brushes.Black, pPtTab1)


        '   Fourth Line.
        '   ------------
        With pPt
            .X = xText
            .Y = .Y + yIncr
        End With
        pPtTab1.Y = pPt.Y

        pstrText = "Date"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        Dim pCI As New CultureInfo("en-US") 'US Format only 
        pstrText = ":" & Space(3) & Today.ToString("MMMM dd, yy", pCI.DateTimeFormat())
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPtTab1)

    End Sub


    Private Sub DrawPreProductionDesignTable(ByVal Result_Sel_In As Integer, ByVal cornerName_in As eBlockCornerName,
                                             ByVal cornerPoint_in As PointF, ByVal Project_In As IPE_clsProject)
        '=============================================================================================================

        'Initialize the Block Dimensions (inches) & Line width.
        '------------------------------------------------------
        Dim pcSEAL_DESIGN_DATA_TABLE As sRectBlock
        Dim pPenWid As Int16                        '....in Pixels

        If mDisplayType = "Printer" Then

            With pcSEAL_DESIGN_DATA_TABLE
                .Wid = 2.7
                .Ht = 2.45
                .H1 = 0.4
            End With

            pPenWid = 10

        ElseIf mDisplayType = "PictureBox" Then

            With pcSEAL_DESIGN_DATA_TABLE
                .Wid = 2.5
                .Ht = 2.45
                .H1 = 0.4
            End With

            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)

        '....Length User Unit (in or mm).
        Dim pstrUnitL As String
        pstrUnitL = mUnitSel.UserL


        'Create Table.
        '=============
        '....Top & Bottom Corner Points. 
        Dim pblockTopL As PointF, pblockBotR As PointF

        If cornerName_in = eBlockCornerName.TopR Then
            With pblockTopL
                .X = cornerPoint_in.X
                .Y = cornerPoint_in.Y
            End With
        End If

        With pblockBotR
            .X = pblockTopL.X + pcSEAL_DESIGN_DATA_TABLE.Wid
            .Y = pblockTopL.Y + pcSEAL_DESIGN_DATA_TABLE.Ht
        End With


        '....Rectangle Size.
        Dim pSize As SizeF
        With pSize
            If mDisplayType = "Printer" Then
                .Width = (pblockBotR.X - pblockTopL.X) + 0.1
                .Height = (pblockBotR.Y - pblockTopL.Y) * 0.4 + 0.015
            ElseIf mDisplayType = "PictureBox" Then
                .Width = (pblockBotR.X - pblockTopL.X) - 0.05
                .Height = (pblockBotR.Y - pblockTopL.Y) * 0.4
            End If
        End With

        If mDisplayType = "Printer" Then
            pblockTopL.X = pblockTopL.X - 0.3
        ElseIf mDisplayType = "PictureBox" Then
            pblockTopL.X = pblockTopL.X + 0.05
        End If

        mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, pSize.Width, pSize.Height)

        Dim yH1 As Single

        yH1 = pblockTopL.Y - 0.015

        '   Create Table Rows.
        '   ------------------
        Dim i As Integer, yIncr As Single, yRow As Single

        If mDisplayType = "Printer" Then
            yIncr = 0.2
            yRow = yH1
            'yRow = yH1 - 0.015

        ElseIf mDisplayType = "PictureBox" Then
            yIncr = 0.2
            yRow = yH1
        End If

        If mDisplayType = "Printer" Then
            pPenWid = 4
        End If
        pPen = New Pen(Color.Black, pPenWid / mDpX)

        'AES 09MAR16
        Dim nRows As Integer
        nRows = 5

        For i = 0 To nRows - 1
            If mDisplayType = "Printer" Then
                yRow = yRow + yIncr
                Dim pXVal As Single = pblockBotR.X - 0.2
                mGr.DrawLine(pPen, pblockTopL.X, yRow, pXVal, yRow)
            ElseIf mDisplayType = "PictureBox" Then
                yRow = yRow + yIncr
                mGr.DrawLine(pPen, pblockTopL.X, yRow, pblockBotR.X, yRow)
            End If

        Next i


        '   Create Table Columns.
        '   ---------------------
        Dim pxIncr As Single
        Dim xCol As Single

        If mDisplayType = "Printer" Then
            pxIncr = 0.8
            xCol = pblockTopL.X
            Dim pBotR As Single = pblockBotR.Y * 0.6
            xCol = xCol + pxIncr
            Dim pYT As Single = yH1 + 0.04
            Dim pYB As Single = pBotR + 0.06
            mGr.DrawLine(pPen, xCol, pYT, xCol, pYB)

        ElseIf mDisplayType = "PictureBox" Then
            pxIncr = 0.8
            xCol = pblockTopL.X

            Dim pBotR As Single = pblockBotR.Y * 0.6
            xCol = xCol + pxIncr
            mGr.DrawLine(pPen, xCol, yH1, xCol, pBotR)
        End If


        '   Fill in the Data Table.
        '   ======================
        Dim xText As Single, yText As Single, y1Incr As Single, y2Incr As Single
        y1Incr = 0.05
        y2Incr = 0.3

        pxIncr = 0.05

        '   Heading Lines:
        '   --------------
        xText = pblockTopL.X + pxIncr
        yText = pblockTopL.Y + y1Incr


        '....First Line
        Dim pPt As PointF
        With pPt
            .X = xText
            .Y = yText - 0.03
        End With

        Dim pFontSize As Single = 8
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Regular)

        Dim pstrText As String = "Tooling P/N "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pxIncr = 0.8
        Dim pxIncr2 As Single = 1
        Dim xTextColumn(3) As Single
        xTextColumn(1) = xText + pxIncr
        xTextColumn(2) = xTextColumn(1) + pxIncr2

        '....CrossSecNo
        pPt.X = xTextColumn(1)

        Dim pToolNo As String = ""
        If (Project_In.Analysis(Result_Sel_In).Seal.Type = "E-Seal") Then
            pToolNo = CType(Project_In.Analysis(Result_Sel_In).Seal, IPE_clsESeal).ToolNo
        ElseIf (Project_In.Analysis(Result_Sel_In).Seal.Type = "C-Seal") Then
            pToolNo = CType(Project_In.Analysis(Result_Sel_In).Seal, IPE_clsCSeal).ToolNo
        End If

        pstrText = pToolNo ' Project_In.Analysis(Result_Sel_In).Seal. "31921 THRU 31924"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Second Line
        yText = yText + 0.15
        With pPt
            .X = xText
            .Y = yText
        End With


        pstrText = "Raw Mat PN"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = xTextColumn(1)
        pstrText = "" '701-015-244-14
        Dim pStripWid As String = ""
        pStripWid = Project_In.Analysis(Result_Sel_In).Seal.StripWid.ToString("#0.000")

        Dim pMatCode As String = ""
        Dim pSealEntity As New SealIPEMCSDBEntities 'SealLibMCSDBEntities
        Dim pMatThick As String = ""
        pMatThick = Project_In.Analysis(Result_Sel_In).Seal.T.ToString("#0.000")
        Dim pMatName As String = Project_In.Analysis(Result_Sel_In).Seal.Mat.Name
        Dim pCount As Integer = (From it In pSealEntity.tblMaterial
                                    Where it.fldName = pMatName Select it).Count()
        If (pCount > 0) Then
            Dim pQry_Mat = (From it In pSealEntity.tblMaterial
                                Where it.fldName = pMatName Select it).First()
            pMatCode = pQry_Mat.fldCode
        End If

        If (pMatThick <> "") Then

            Dim pPredata As Integer = ConvertToInt(ExtractPreData(pMatThick, "."))
            If (pPredata > 0) Then
                pstrText = "701-" & pPredata.ToString() & ExtractPostData(pMatThick, ".")
            Else
                pstrText = "701-" & ExtractPostData(pMatThick, ".")
            End If
        End If

        If (pstrText <> "") Then
            Dim pPredata As Integer = ConvertToInt(ExtractPreData(pStripWid, "."))
            If (pPredata > 0) Then
                pstrText = pstrText & "-" & pPredata.ToString() & ExtractPostData(pStripWid, ".")

            Else
                pstrText = pstrText & "-" & ExtractPostData(pStripWid, ".")

            End If
        Else
            Dim pPredata As Integer = ConvertToInt(ExtractPreData(pStripWid, "."))
            If (pPredata > 0) Then
                pstrText = "701-" & "-" & pPredata.ToString() & ExtractPostData(pStripWid, ".")

            Else
                pstrText = "701-" & ExtractPostData(pStripWid, ".")

            End If

        End If

        If (pMatCode <> "") Then
            pstrText = pstrText & "-" & pMatCode
        End If

        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Third Line
        yText = yText + 0.2
        With pPt
            .X = xText
            .Y = yText
        End With


        pstrText = "H/T Process"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = xTextColumn(1)
        pstrText = mSealSel.Mat.HT
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Fourth Line
        yText = yText + 0.2
        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Pre H/T Dia"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        Dim pPreHTDia As Single = Project_In.Analysis(Result_Sel_In).Seal.DControl + (Project_In.Analysis(Result_Sel_In).Seal.DControl * 0.001)

        pPt.X = xTextColumn(1)
        pstrText = pPreHTDia.ToString("#0.000") + "  in"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Fifth Line
        yText = yText + 0.2
        With pPt
            .X = xText
            .Y = yText
        End With


        pstrText = "Post H/T Dia"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = xTextColumn(1)
        pstrText = Project_In.Analysis(Result_Sel_In).Seal.DControl.ToString("#0.000") + "  in"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


    End Sub


    Private Sub DrawSealDesignDataTable(ByVal Cavity_Exists_In As Boolean, ByVal cornerName_in As eBlockCornerName, _
                                        ByVal cornerPoint_in As PointF, _
                                        ByRef blockBotR_out As PointF, ByVal Project_In As IPE_clsProject)
        '=============================================================================================================
        'This routine creates Data Table at the LHS corner location.
        '
        '   Input  parameters : cornerName_in, cornerPoint_in.
        '                       The last two input will define the placement of the
        '                       block w.r.t the border.
        '   Output parameters : blockBotR_out


        'Initialize the Block Dimensions (inches) & Line width.
        '------------------------------------------------------
        Dim pcSEAL_DESIGN_DATA_TABLE As sRectBlock
        Dim pPenWid As Int16                        '....in Pixels

        If mDisplayType = "Printer" Then

            With pcSEAL_DESIGN_DATA_TABLE
                .Wid = 2.7
                'AES 08MAR16
                '.Ht = 2.2
                .Ht = 2.45
                .H1 = 0.4
            End With

            pPenWid = 10

        ElseIf mDisplayType = "PictureBox" Then

            With pcSEAL_DESIGN_DATA_TABLE
                .Wid = 2.5 '4        '2.5
                'AES 08MAR16
                '.Ht = 2.2
                .Ht = 2.45 '2.5
                .H1 = 0.4 '0.45
            End With

            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)

        '....Length User Unit (in or mm).
        Dim pstrUnitL As String
        pstrUnitL = mUnitSel.UserL


        'Create Table.
        '=============
        '....Top & Bottom Corner Points. 
        Dim pblockTopL As PointF, pblockBotR As PointF

        If cornerName_in = eBlockCornerName.TopL Then
            With pblockTopL
                .X = cornerPoint_in.X
                .Y = cornerPoint_in.Y
            End With
        End If

        With pblockBotR
            .X = pblockTopL.X + pcSEAL_DESIGN_DATA_TABLE.Wid
            .Y = pblockTopL.Y + pcSEAL_DESIGN_DATA_TABLE.Ht
        End With

        blockBotR_out = pblockBotR


        '....Rectangle Size.
        Dim pSize As SizeF
        With pSize
            .Width = pblockBotR.X - pblockTopL.X
            .Height = pblockBotR.Y - pblockTopL.Y
        End With

        '....Draw the Block rectangle.
        'AES 08MAR16
        'mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, pSize.Width, pSize.Height)

        'AES 08MAR16
        If (Cavity_Exists_In) Then
            mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, pSize.Width, pSize.Height)
        Else
            Dim pHeight As Single = pSize.Height * 0.7
            mGr.DrawRectangle(pPen, pblockTopL.X, pblockTopL.Y, pSize.Width, pHeight)
        End If


        '   Draw the Heading line.
        '   ----------------------
        Dim yH1 As Single

        '....AES 08MAR16
        'yH1 = pblockTopL.Y + pcSEAL_DESIGN_DATA_TABLE.H1
        yH1 = pblockTopL.Y + pcSEAL_DESIGN_DATA_TABLE.H1 + 0.27

        If mDisplayType = "Printer" Then
            pPenWid = 8
        End If
        pPen = New Pen(Color.Black, pPenWid / mDpX)

        mGr.DrawLine(pPen, pblockTopL.X, yH1, pblockBotR.X, yH1)


        '   Create Table Rows.
        '   ------------------
        Dim i As Integer, yIncr As Single, yRow As Single

        If mDisplayType = "Printer" Then
            yIncr = 0.2
            yRow = yH1

        ElseIf mDisplayType = "PictureBox" Then
            yIncr = 0.2
            yRow = yH1
        End If

        If mDisplayType = "Printer" Then
            pPenWid = 4
        End If
        pPen = New Pen(Color.Black, pPenWid / mDpX)

        'AES 08MAR16
        'Dim nRows As Integer
        'nRows = 9

        'For i = 1 To nRows - 1
        '    yRow = yRow + yIncr
        '    mGr.DrawLine(pPen, pblockTopL.X, yRow, pblockBotR.X, yRow)
        'Next i

        Dim nRows As Integer
        If (Cavity_Exists_In) Then
            nRows = 9
        Else
            nRows = 5
        End If

        For i = 1 To nRows - 1
            yRow = yRow + yIncr
            mGr.DrawLine(pPen, pblockTopL.X, yRow, pblockBotR.X, yRow)
        Next i


        '   Create Table Columns.
        '   ---------------------
        Dim pxIncr(3) As Single
        Dim xCol As Single

        If mDisplayType = "Printer" Then
            pxIncr(0) = 0.8
            pxIncr(1) = 0.7
            pxIncr(2) = 0.7
            xCol = pblockTopL.X

        ElseIf mDisplayType = "PictureBox" Then
            pxIncr(0) = 0.8
            pxIncr(1) = 0.7
            pxIncr(2) = 0.6
            xCol = pblockTopL.X
        End If

        'AES 08MAR16
        'For i = 0 To 2
        '    xCol = xCol + pxIncr(i)
        '    mGr.DrawLine(pPen, xCol, yH1, xCol, pblockBotR.Y)
        'Next i


        If (Cavity_Exists_In) Then
            For i = 0 To 2
                xCol = xCol + pxIncr(i)
                mGr.DrawLine(pPen, xCol, yH1, xCol, pblockBotR.Y)
            Next i
        Else
            For i = 0 To 2
                Dim pBotR As Single = pblockBotR.Y * 0.725
                xCol = xCol + pxIncr(i)
                mGr.DrawLine(pPen, xCol, yH1, xCol, pBotR)
            Next i
        End If



        '   Fill in the Data Table.
        '   ======================
        Dim xText As Single, yText As Single, y1Incr As Single, y2Incr As Single
        y1Incr = 0.05
        y2Incr = 0.3

        pxIncr(0) = 0.05

        '   Heading Lines:
        '   --------------
        xText = pblockTopL.X + pxIncr(0)
        yText = pblockTopL.Y + y1Incr

        If mDisplayType = "PictureBox" Then
            'xText = pblockTopL.X + -0.05
            'yText = yText - 0.13
        End If

        '....First Line
        Dim pPt As PointF
        With pPt
            .X = xText
            .Y = yText
        End With

        Dim pFontSize As Single = 8
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Regular)

        'Dim pstrText As String = "Ref. Cross Sec"
        Dim pstrText As String = "Customer:"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        'pxIncr(1) = 1.05 : pxIncr(2) = 0.2 : pxIncr(3) = 0.44
        pxIncr(1) = 0.6 : pxIncr(2) = 0.9 : pxIncr(3) = 0.5
        Dim xTextColumn(3) As Single
        xTextColumn(1) = xText + pxIncr(1)
        xTextColumn(2) = xTextColumn(1) + pxIncr(2)
        xTextColumn(3) = xTextColumn(2) + pxIncr(3)

        'pxIncr(1) = 0.05 : pxIncr(2) = 0.5 : pxIncr(3) = 0.5 : pxIncr(3) = 0.05
        'Dim xTextColumn(4) As Single
        'xTextColumn(1) = xText + pxIncr(1)
        'xTextColumn(2) = xTextColumn(1) + pxIncr(2)
        'xTextColumn(3) = xTextColumn(2) + pxIncr(3)
        'xTextColumn(4) = xTextColumn(3) + pxIncr(4)

        '....":" Text
        'pPt.X = xTextColumn(1)
        'pstrText = ": "
        'mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....CrossSecNo
        pPt.X = xTextColumn(1)
        'pstrText = mSealSel.MCrossSecNo
        pstrText = Project_In.Customer()
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Platform Text
        'pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pPt.X = xTextColumn(2) - 0.18
        mGr.DrawString("PlatForm:", pFont, Brushes.Black, pPt)

        pPt.X = xTextColumn(3) - 0.2
        pstrText = Project_In.Platform()
        mGr.DrawString(Space(1) & pstrText, pFont, Brushes.Black, pPt)


        '....Second Line
        ' yText = yText + pyIncr(2)
        yText = yText + 0.15
        With pPt
            .X = xText
            .Y = yText
        End With

        'pFontSize = 8
        'pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "Customer PN:"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....":" Text
        pPt.X = xTextColumn(1) + 0.18
        'pPt.X = pPt.X + pxIncr(1)
        pstrText = Project_In.CustomerPN()
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = pPt.X + 0.4
        pstrText = "Parker PN:"
        mGr.DrawString(Space(3) & pstrText, pFont, Brushes.Black, pPt)

        '....Parker PN Value
        pPt.X = pPt.X + 0.6
        pstrText = Space(2) + Project_In.ParkerPN()
        'pstrText = UCase$(mSealSel.Adjusted)
        mGr.DrawString(Space(1) & pstrText, pFont, Brushes.Black, pPt)

        '....Third Line
        ' yText = yText + pyIncr(2)
        yText = yText + 0.15
        With pPt
            .X = xText
            .Y = yText
        End With

        'pFontSize = 8
        'pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        pstrText = "MCS:"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = pPt.X + 0.3
        pstrText = mSealSel.MCrossSecNo
        'pstrText = UCase$(mSealSel.Adjusted)
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = pPt.X + 0.6
        pstrText = "Adjusted:"
        mGr.DrawString(Space(13) & pstrText, pFont, Brushes.Black, pPt)

        pPt.X = pPt.X + 0.5
        pstrText = UCase$(mSealSel.Adjusted)
        'pstrText = UCase$(mSealSel.Adjusted)
        mGr.DrawString(Space(14) & pstrText, pFont, Brushes.Black, pPt)

        '....Fourth Line
        ' yText = yText + pyIncr(2)
        yText = yText + 0.15
        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Segmented:"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pPt.X = pPt.X + 0.65

        If (mSealSel.IsSegmented) Then
            pstrText = "Y"
        Else
            pstrText = "N"
        End If

        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        'AES 28APR16
        If (mSealSel.IsSegmented) Then
            pPt.X = pPt.X + 0.26
            pstrText = "# of Segments:"
            mGr.DrawString(Space(13) & pstrText, pFont, Brushes.Black, pPt)

            pPt.X = pPt.X + 0.8
            pstrText = mSealSel.CountSegment
            mGr.DrawString(Space(14) & pstrText, pFont, Brushes.Black, pPt)
        End If


        '   Free Height Row:.
        '   ---------------
        'AES 08MAR16
        'yText = yText + 0.22
        yText = yText + 0.21

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Free Height"
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        pxIncr(0) = 0.88     '....For 2nd Column
        pxIncr(1) = 0.7      '....For 3rd Column   
        pxIncr(2) = 0.68     '....For 4th Column        

        xTextColumn(1) = xText + pxIncr(0)
        xTextColumn(2) = xTextColumn(1) + pxIncr(1)
        xTextColumn(3) = xTextColumn(2) + pxIncr(2)

        pPt.X = xTextColumn(1)
        pstrText = mUnitSel.WriteInUserL(mSealSel.Hfree)
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '....Tolerances:
        '
        '........(+) Value.
        Dim yTextSave As Single
        yTextSave = yText

        With pPt
            .X = xTextColumn(2)
            .Y = yTextSave - 0.018
        End With

        pFontSize = 7
        pFont = New Font("Arial", pFontSize, FontStyle.Bold) '.FontBold = True

        pstrText = "+ " & mUnitSel.WriteInUserL(mSealSel.HFreeTol(2))
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        With pPt
            .Y = yTextSave + 0.065
        End With

        pstrText = "- " & mUnitSel.WriteInUserL(mSealSel.HFreeTol(2))
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '....Free Height Unit
        pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        With pPt
            .X = xTextColumn(3)
            .Y = yTextSave
        End With

        pstrText = pstrUnitL
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Width:
        '   ------
        pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        'If mDisplayType = "Printer" Then
        yText = yText + yIncr
        'ElseIf mDisplayType = "PictureBox" Then
        '    yText = yText + yIncr
        'End If

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Width    "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Width Value
        pPt.X = xTextColumn(1)
        pstrText = mUnitSel.WriteInUserL(mSealSel.Wid)
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '....Width Unit
        With pPt
            .X = xTextColumn(3)
        End With
        pstrText = pstrUnitL
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Material
        '   --------
        'If mDisplayType = "Printer" Then
        yText = yText + yIncr
        'ElseIf mDisplayType = "PictureBox" Then
        '    yText = yText + yIncr 
        'End If

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Material   "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '....Material Name
        pPt.X = xTextColumn(1)
        pstrText = mMatSelName
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Thickness.
        '   ----------
        'If mDisplayType = "Printer" Then
        '    yText = yText + yIncr
        'ElseIf mDisplayType = "PictureBox" Then
        '    yText = yText + yIncr
        'End If


        '....Coating
        If mSealSel.Type = "E-Seal" Then
            'yText = yText + yIncr
            pPt.X = xTextColumn(2)
            If mMatSelCoating = "None" Then
                pPt.X = xTextColumn(2) - 0.05
                pstrText = "Uncoated"
            Else
                pstrText = mMatSelCoating
            End If
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        ElseIf mSealSel.Type = "C-Seal" Then
            pPt.X = xTextColumn(2)
            If CType(mSealSel, IPE_clsCSeal).Plating.Code = "None" Then
                pPt.X = xTextColumn(2) - 0.05
                pstrText = "Unplated"
            Else
                pstrText = CType(mSealSel, IPE_clsCSeal).Plating.Code
            End If

            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        End If

        yText = yText + yIncr

        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Thickness   "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Thickness Value
        pPt.X = xTextColumn(1)
        pstrText = mUnitSel.WriteInUserL(mSealSel.T)
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '....Unit
        pPt.X = xTextColumn(3)
        pstrText = pstrUnitL
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '   Diameter.
        '   ---------
        'If mDisplayType = "Printer" Then
        '    yText = yText + yIncr
        'ElseIf mDisplayType = "PictureBox" Then
        '    yText = yText + yIncr
        'End If

        yText = yText + yIncr
        With pPt
            .X = xText
            .Y = yText
        End With

        pstrText = "Diameter    "
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

        '....Value.
        pPt.X = xTextColumn(1)
        pstrText = mUnitSel.WriteInUserL(mSealSel.DControl)
        mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        '....Tolerances:
        yTextSave = yText
        pPt.X = xTextColumn(2)

        pFontSize = 7
        pFont = New Font("Arial", pFontSize, FontStyle.Bold)

        '....The following format is for displaying 0 tolerance.
        Dim pstrFormat As String = ""

        If mUnitSel.System = "English" Then
            pstrFormat = "##0.000"
        ElseIf mUnitSel.System = "Metric" Then
            pstrFormat = "###0.00"
        End If

        Dim psngTol1DControl As Single
        Dim psngTol2DControl As Single
        psngTol1DControl = mSealSel.H11Tol
        psngTol2DControl = 0.0#

        Dim pstrTol As String
        Dim pstrNull As String
        pstrTol = mUnitSel.WriteInUserL(psngTol1DControl)
        pstrNull = Format(psngTol2DControl, pstrFormat)


        If mSealSel.POrient = "External" Then
            pPt.Y = yTextSave - 0.02
            pstrText = "+ " & pstrTol
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            pPt.Y = yTextSave + 0.065
            pstrText = "-  " & pstrNull
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


        ElseIf mSealSel.POrient = "Internal" Then
            pPt.Y = yTextSave - 0.02
            pstrText = "+ " & pstrNull
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            pPt.Y = yTextSave + 0.065
            pstrText = "-  " & pstrTol
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)
        End If


        '....Unit.
        pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        With pPt
            .X = xTextColumn(3)
            .Y = yTextSave
        End With
        mGr.DrawString(pstrUnitL, pFont, Brushes.Black, pPt)


        '   Cavity Dia.
        '   -----------
        pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        yText = yText + yIncr
        With pPt
            .X = xText
            .Y = yText
        End With

        Dim psngDCavity As Single
        If mSealSel.POrient = "External" Then
            psngDCavity = mCavitySel.Dia(1)
        ElseIf mSealSel.POrient = "Internal" Then
            psngDCavity = mCavitySel.Dia(2)
        End If


        If (Cavity_Exists_In) Then

            pstrText = "Cavity Dia"
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            '....Value.
            pPt.X = xTextColumn(1)
            pstrText = mUnitSel.WriteInUserL(psngDCavity)
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


            '....Tolerances:
            yTextSave = yText
            pPt.X = xTextColumn(2) 'xText

            pFontSize = 7
            pFont = New Font("Arial", pFontSize, FontStyle.Bold)

            Dim psngTol1DCavity As Single
            Dim psngTol2DCavity As Single

            psngTol1DCavity = mCavitySel.H10Tol(mSealSel.DControl)  'AES 08APR16
            psngTol2DCavity = 0.0#

            pstrTol = mUnitSel.WriteInUserL(psngTol1DCavity)
            pstrNull = Format(psngTol2DCavity, pstrFormat)


            If mSealSel.POrient = "External" Then
                pPt.Y = yTextSave - 0.02
                pstrText = "+ " & pstrNull
                mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

                pPt.Y = yTextSave + 0.065
                pstrText = "-  " & pstrTol
                mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


            ElseIf mSealSel.POrient = "Internal" Then
                pPt.Y = yTextSave - 0.02
                pstrText = "+ " & pstrTol
                mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

                pPt.Y = yTextSave + 0.065
                pstrText = "-  " & pstrNull
                mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)
            End If

            '....Unit
            pFontSize = 8
            pFont = New Font("Arial", pFontSize, FontStyle.Regular)
            With pPt
                .X = xTextColumn(3)
                .Y = yTextSave
            End With
            mGr.DrawString(pstrUnitL, pFont, Brushes.Black, pPt)


            '   Cavity Width.
            '   ------------
            pFontSize = 8
            pFont = New Font("Arial", pFontSize, FontStyle.Regular)

            'If mDisplayType = "Printer" Then
            '    yText = yText + yIncr
            'ElseIf mDisplayType = "PictureBox" Then
            '    yText = yText + yIncr
            'End If

            yText = yText + yIncr

            With pPt
                .X = xText
                .Y = yText
            End With

            pstrText = "W"
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


            '....Value
            pPt.X = xTextColumn(1)
            pstrText = mUnitSel.WriteInUserL(mCavitySel.WidMin)
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


            '....Unit
            pPt.X = xTextColumn(3)
            mGr.DrawString(pstrUnitL, pFont, Brushes.Black, pPt)


            '   Cavity Depth.
            '   ------------
            'If mDisplayType = "Printer" Then
            '    yText = yText + yIncr
            'ElseIf mDisplayType = "PictureBox" Then
            '    yText = yText + yIncr
            'End If

            yText = yText + yIncr

            With pPt
                .X = xText
                .Y = yText
            End With

            pstrText = "h"
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            '....Measurement
            pPt.X = xTextColumn(1)
            pstrText = mUnitSel.WriteInUserL(mCavitySel.Depth)
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)


            '....Tolerances:
            '
            '........(+) Value.
            yTextSave = yText

            With pPt
                .X = xTextColumn(2)
                .Y = yTextSave - 0.018
            End With

            pFontSize = 7
            pFont = New Font("Arial", pFontSize, FontStyle.Bold) '.FontBold = True

            pstrText = "+ " & mUnitSel.WriteInUserL(mCavitySel.DepthTol(2))
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            '........(-) Value.
            With pPt
                .Y = yTextSave + 0.065
            End With

            pstrText = "- " & mUnitSel.WriteInUserL(mCavitySel.DepthTol(2))
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            '....Unit
            With pPt
                .X = xTextColumn(3)
                .Y = yTextSave
            End With
            mGr.DrawString(pstrUnitL, pFont, Brushes.Black, pPt)


            '   Corner Radius.
            '   --------------
            'If mDisplayType = "Printer" Then
            '    yText = yText + yIncr
            'ElseIf mDisplayType = "PictureBox" Then
            '    yText = yText + yIncr
            'End If

            yText = yText + yIncr

            With pPt
                .X = xText
                .Y = yText
            End With

            pstrText = "r (Max.)"
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            '....Measurement
            pPt.X = xTextColumn(1)
            pstrText = mUnitSel.WriteInUserL(mSealSel.CavityCornerRad)
            mGr.DrawString(pstrText, pFont, Brushes.Black, pPt)

            '....Unit
            pPt.X = xTextColumn(3)
            mGr.DrawString(pstrUnitL, pFont, Brushes.Black, pPt)
        End If


    End Sub


    Public Sub DrawCavity(ByVal Cavity_Exists_In As Boolean, ByVal cornerName_in As eBlockCornerName, _
                           ByVal cornerPoint_in As PointF, ByRef cavityBotR_out As PointF)
        '===================================================================================================
        'This routine creates the cavity drawing. 
        mGr.PageUnit = GraphicsUnit.Inch
        mDpX = mGr.DpiX     '....# of Pixels/in on the current Graphics Object.

        Const pcFLANGE_LENGTH As Single = 2.5
        Const pc_GAP As Single = 0.25
        Const pcCAVITY_LENGTH_TOP As Single = 0.75
        Const pcCAVITY_LENGTH_BOT As Single = 1.0#
        Const pcCAVITY_DEPTH As Single = 0.5
        Const pcCHAMFER_LENGTH As Single = 0.05
        Const pcCORNER_RADIUS As Single = 0.1 '0.05

        '....Temporary variables
        Dim begPoint As PointF, endPoint As PointF

        Dim pPen As Pen

        'Define Pen. 
        '----------
        Dim pPenWid As Int16
        If mDisplayType = "Printer" Then
            pPenWid = 8
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
        End If

        '....AES 08MAR16
        'Dim pPen As New Pen(Color.Black, pPenWid / mDpX)

        If (Cavity_Exists_In) Then
            pPen = New Pen(Color.Black, pPenWid / mDpX)
        Else
            pPen = New Pen(Color.White, pPenWid / mDpX)
        End If


        'Flange (Top surface)
        '--------------------
        Dim pFlangeL As PointF, pFlangeR As PointF

        If cornerName_in = eBlockCornerName.TopL Then

            With pFlangeL

                If mDisplayType = "Printer" Then
                    .X = cornerPoint_in.X
                ElseIf mDisplayType = "PictureBox" Then
                    .X = cornerPoint_in.X - 0.7
                End If

                .Y = cornerPoint_in.Y
            End With

        End If


        If mDisplayType = "Printer" Then
            pFlangeR.X = pFlangeL.X + pcFLANGE_LENGTH

        ElseIf mDisplayType = "PictureBox" Then
            pFlangeR.X = pFlangeL.X + pcFLANGE_LENGTH - 0.1
        End If

        pFlangeR.Y = pFlangeL.Y

        mGr.DrawLine(pPen, pFlangeL.X, pFlangeL.Y, pFlangeR.X, pFlangeR.Y)


        'Cavity:
        '=======

        'Top Line - Segment 1 (Left Segment)
        '-----------------------------------
        Dim pCavityTop1L As PointF, pCavityTop1R As PointF

        With pCavityTop1L
            .X = pFlangeL.X
            .Y = pFlangeL.Y + pc_GAP
        End With

        With pCavityTop1R
            .X = pCavityTop1L.X + pcCAVITY_LENGTH_TOP - pcCHAMFER_LENGTH
            .Y = pCavityTop1L.Y
        End With

        mGr.DrawLine(pPen, pCavityTop1L.X, pCavityTop1L.Y, pCavityTop1R.X, pCavityTop1R.Y)


        'Top Line - Segment 2 (Right Segment)
        '------------------------------------
        Dim pCavityTop2L As PointF, pCavityTop2R As PointF

        With pCavityTop2L
            .X = pCavityTop1R.X + pcCAVITY_LENGTH_BOT + 2.0# * pcCHAMFER_LENGTH
            .Y = pCavityTop1R.Y
        End With


        With pCavityTop2R

            If mDisplayType = "Printer" Then
                .X = pCavityTop2L.X + pcCAVITY_LENGTH_TOP
            ElseIf mDisplayType = "PictureBox" Then
                .X = pCavityTop2L.X + pcCAVITY_LENGTH_TOP - 0.15
            End If

            .Y = pCavityTop2L.Y

        End With

        mGr.DrawLine(pPen, pCavityTop2L.X, pCavityTop2L.Y, _
                           pCavityTop2R.X, pCavityTop2R.Y)


        'Side Vertical Line 1 - Left
        '---------------------------
        Dim pCavitySide1Top As PointF, pCavitySide1Bot As PointF
        With pCavitySide1Top
            .X = pCavityTop1R.X + pcCHAMFER_LENGTH
            .Y = pCavityTop1R.Y + pcCHAMFER_LENGTH
        End With

        With pCavitySide1Bot
            .X = pCavitySide1Top.X
            .Y = pCavityTop1R.Y + pcCAVITY_DEPTH - pcCORNER_RADIUS
        End With

        mGr.DrawLine(pPen, pCavitySide1Top.X, pCavitySide1Top.Y, _
                           pCavitySide1Bot.X, pCavitySide1Bot.Y)


        'Side Vertical Line 1 - Right
        '----------------------------
        Dim pCavitySide2Top As PointF, pCavitySide2Bot As PointF
        With pCavitySide2Top
            .X = pCavityTop2L.X - pcCHAMFER_LENGTH
            .Y = pCavityTop2L.Y + pcCHAMFER_LENGTH
        End With

        With pCavitySide2Bot
            .X = pCavitySide2Top.X
            .Y = pCavityTop2L.Y + pcCAVITY_DEPTH - pcCORNER_RADIUS
        End With

        mGr.DrawLine(pPen, pCavitySide2Top.X, pCavitySide2Top.Y, _
                           pCavitySide2Bot.X, pCavitySide2Bot.Y)


        'Bottom Line
        '-----------
        Dim pCavityBotL As PointF, pCavityBotR As PointF
        With pCavityBotL
            .X = pCavitySide1Bot.X + pcCORNER_RADIUS
            .Y = pCavitySide1Bot.Y + pcCORNER_RADIUS
        End With

        With pCavityBotR
            .X = pCavityBotL.X + pcCAVITY_LENGTH_BOT - 2 * pcCORNER_RADIUS
            .Y = pCavityBotL.Y
        End With

        mGr.DrawLine(pPen, pCavityBotL.X, pCavityBotL.Y, pCavityBotR.X, pCavityBotR.Y)
        cavityBotR_out = pCavityBotR     '....Output parameter


        'Left Chamfer Line
        '-----------------
        mGr.DrawLine(pPen, pCavityTop1R.X, pCavityTop1R.Y, _
                           pCavitySide1Top.X, pCavitySide1Top.Y)


        'Right Chamfer Line
        '------------------
        mGr.DrawLine(pPen, pCavityTop2L.X, pCavityTop2L.Y, _
                           pCavitySide2Top.X, pCavitySide2Top.Y)


        'Left Corner Radius    
        '------------------
        Dim pRad As Single = pcCORNER_RADIUS

        Dim pCenter As PointF
        pCenter.X = pCavitySide1Bot.X + pRad
        pCenter.Y = pCavitySide1Bot.Y

        mGr.DrawArc(pPen, pCenter.X - pRad, pCenter.Y - pRad, pRad * 2, pRad * 2, 90, 90)


        '....Radius line and symbol
        Dim del As Single
        del = 0.707 * pcCORNER_RADIUS       '....COS45 = 0.707

        Dim pArrowPt As PointF
        With pArrowPt
            .X = pCenter.X - del
            .Y = pCenter.Y + del
        End With

        mGr.DrawLine(pPen, pCenter.X, pCenter.Y, pArrowPt.X, pArrowPt.Y)

        '....AES08MAR16
        'DrawArrow(pArrowPt, eArrowType.DiagArrowDown)

        '....Draw Arrow.
        If (Cavity_Exists_In) Then
            DrawArrow(pArrowPt, eArrowType.DiagArrowDown)
        End If


        '....Write 'r (Max)'.
        Dim pPt As PointF
        With pPt
            .X = pCenter.X + 0.02
            .Y = pCenter.Y - 0.15
        End With

        Dim pFontSize As Single = 11
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Regular)

        '....AES 08MAR16
        'mGr.DrawString("r", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("r", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("r", pFont, Brushes.White, pPt)
        End If


        With pPt
            .X = pCenter.X + 0.1
            .Y = pCenter.Y - 0.12
        End With

        pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        '....AES 08MAR16
        'mGr.DrawString("(Max)", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("(Max)", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("(Max)", pFont, Brushes.White, pPt)
        End If


        'Right Corner Radius.       
        '--------------------
        pCenter.X = pCavitySide2Bot.X - pcCORNER_RADIUS
        pCenter.Y = pCavitySide2Bot.Y

        mGr.DrawArc(pPen, pCenter.X - pRad, pCenter.Y - pRad, pRad * 2, pRad * 2, 0, 90)


        'Auxilliary features drawing:
        '============================

        If mDisplayType = "Printer" Then
            pPenWid = 4
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 1
        End If


        'Surface finish specification on Flange.
        '---------------------------------------
        Dim pbotPoint As PointF
        pbotPoint.X = 0.5 * (pFlangeL.X + pFlangeR.X)
        pbotPoint.Y = pFlangeL.Y

        With pPt
            .X = pbotPoint.X - 0.08
            .Y = pbotPoint.Y - 0.15
        End With

        pFontSize = 7
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        '....AES 08MAR16
        'mGr.DrawString("32", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("32", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("32", pFont, Brushes.White, pPt)
        End If



        With pPt
            .X = pbotPoint.X + 0.08
            .Y = pbotPoint.Y - 0.11
        End With

        'mGr.DrawString("C", pFont, Brushes.Black, pPt)

        '....AES 08MAR16
        If (Cavity_Exists_In) Then
            mGr.DrawString("C", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("C", pFont, Brushes.White, pPt)
        End If

        '....AES 08MAR16
        'DrawAngle(pbotPoint)

        If (Cavity_Exists_In) Then
            DrawAngle(pbotPoint)
        End If


        'Dimension lines :
        '================

        'Cavity total depth 'h':
        '-----------------------
        '....Top horizontal line.
        With begPoint
            .X = pFlangeR.X + 0.1
            .Y = pFlangeR.Y
        End With

        With endPoint
            .X = begPoint.X + 0.2
            .Y = pFlangeR.Y
        End With

        '....AES 08MAR16
        'pPen = New Pen(Color.Black, pPenWid / mDpX)

        If (Cavity_Exists_In) Then
            pPen = New Pen(Color.Black, pPenWid / mDpX)
        Else
            pPen = New Pen(Color.White, pPenWid / mDpX)
        End If

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        '....Vertical dimension line - Upper segment.
        Dim topArrowPt As PointF
        With topArrowPt
            .X = 0.5 * (begPoint.X + endPoint.X)
            .Y = 0.5 * (begPoint.Y + endPoint.Y)
        End With

        '....AES 08MAR16
        'DrawArrow(topArrowPt, eArrowType.VertArrowUp)

        If (Cavity_Exists_In) Then
            DrawArrow(topArrowPt, eArrowType.VertArrowUp)
        End If

        pPt.Y = topArrowPt.Y + 0.25
        With topArrowPt
            mGr.DrawLine(pPen, .X, .Y, .X, pPt.Y)
        End With


        '....Write 'h'
        With pPt
            .X = topArrowPt.X - 0.02
            .Y = topArrowPt.Y + 0.3
            pFontSize = 11
        End With

        pFontSize = 11
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        '....AES 08MAR16
        'mGr.DrawString("h", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("h", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("h", pFont, Brushes.White, pPt)
        End If


        '....Vertical dimension line - Lower segment.
        Dim botArrowPt As PointF
        With botArrowPt
            .X = topArrowPt.X
            .Y = topArrowPt.Y + pcCAVITY_DEPTH + pc_GAP
        End With

        '....AES 08MAR16
        'DrawArrow(botArrowPt, eArrowType.VertArrowDown)

        If (Cavity_Exists_In) Then
            DrawArrow(botArrowPt, eArrowType.VertArrowDown)
        End If

        pPt.Y = botArrowPt.Y - 0.25

        With botArrowPt
            mGr.DrawLine(pPen, .X, .Y, .X, pPt.Y)
        End With


        '....Bottom horizontal line.
        With begPoint
            .X = pCavityBotR.X + 0.1
            .Y = pCavityBotR.Y
        End With

        With endPoint
            .X = begPoint.X + 1.25
            .Y = begPoint.Y
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        'Cavity width 'W'
        '----------------
        '....Left vertical line.

        With begPoint
            .X = pCavityBotL.X - pcCORNER_RADIUS
            .Y = pCavityBotL.Y + 0.1
        End With

        With endPoint
            .X = begPoint.X
            .Y = begPoint.Y + 0.2
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        Dim leftArrowPt As PointF
        With leftArrowPt
            .X = 0.5 * (begPoint.X + endPoint.X)
            .Y = 0.5 * (begPoint.Y + endPoint.Y)
        End With

        '....AES 08MAR16
        'DrawArrow(leftArrowPt, eArrowType.HorzArrowLeft)

        If (Cavity_Exists_In) Then
            DrawArrow(leftArrowPt, eArrowType.HorzArrowLeft)
        End If



        '....Horizontal dimension line - Left segment.
        pPt.X = leftArrowPt.X + 0.4
        With leftArrowPt
            mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
        End With

        '....Write 'W'
        With pPt
            .X = leftArrowPt.X + 0.45
            .Y = leftArrowPt.Y - 0.1
        End With

        pFontSize = 11
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        '....AES 08MAR16
        'mGr.DrawString("w", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("w", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("w", pFont, Brushes.White, pPt)
        End If


        '....Horizontal dimension line - Right segment.
        Dim rightArrowPt As PointF
        With rightArrowPt
            .X = leftArrowPt.X + pcCAVITY_LENGTH_BOT
            .Y = leftArrowPt.Y
        End With

        '....AES 08MAR16
        'DrawArrow(rightArrowPt, eArrowType.HorzArrowRight)

        If (Cavity_Exists_In) Then
            DrawArrow(rightArrowPt, eArrowType.HorzArrowRight)
        End If

        pPt.X = rightArrowPt.X - 0.4
        With rightArrowPt
            mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
        End With


        '....Right vertical line.
        With begPoint
            .X = pCavityBotR.X + pcCORNER_RADIUS
            .Y = pCavityBotR.Y + 0.1
        End With

        With endPoint
            .X = begPoint.X
            .Y = begPoint.Y + 0.2
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        'CAVITY DIAMETER
        '---------------
        '....Left vertical line.
        If mSealSel.POrient = "External" Then

            With begPoint
                .X = pCavityBotR.X + pcCORNER_RADIUS
                .Y = pCavityBotR.Y + 0.3 + 0.05
            End With

        ElseIf mSealSel.POrient = "Internal" Then

            With begPoint
                .X = pCavityBotL.X - pcCORNER_RADIUS
                .Y = pCavityBotL.Y + 0.3 + 0.05
            End With

        End If


        With endPoint
            .X = begPoint.X
            .Y = begPoint.Y + 0.2
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)

        With leftArrowPt
            .X = 0.5 * (begPoint.X + endPoint.X)
            .Y = 0.5 * (begPoint.Y + endPoint.Y)
        End With


        '....Horizontal dimension line - Left segment.
        'DrawArrow(leftArrowPt, eArrowType.HorzArrowLeft)

        If (Cavity_Exists_In) Then
            DrawArrow(leftArrowPt, eArrowType.HorzArrowLeft)
        End If

        With leftArrowPt
            If mSealSel.POrient = "External" Then
                pPt.X = .X + 0.5
                mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)

            ElseIf mSealSel.POrient = "Internal" Then
                pPt.X = .X + pcCAVITY_LENGTH_BOT + 0.5
                mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
            End If
        End With


        '....Write 'CAVITY DIAMETER (Max)/(Min)'
        Dim xPos As Single, yPos As Single
        xPos = pCavityBotR.X + pcCORNER_RADIUS + 0.6
        yPos = leftArrowPt.Y - 0.1

        With pPt
            .X = xPos
            .Y = yPos
        End With

        pFontSize = 7
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        '....AES 08MAR16
        'mGr.DrawString("CAVITY", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("CAVITY", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("CAVITY", pFont, Brushes.White, pPt)
        End If


        xPos = xPos - 0.07
        yPos = yPos + 0.13

        With pPt
            .X = xPos
            .Y = yPos
        End With

        '....AES 08MAR16
        'mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)

        If (Cavity_Exists_In) Then
            mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)
        Else
            mGr.DrawString("DIAMETER", pFont, Brushes.White, pPt)
        End If

        xPos = xPos + 0.12
        yPos = yPos + 0.14

        With pPt
            .X = xPos
            .Y = yPos
        End With

        If mSealSel.POrient = "External" Then
            '....AES 08MAR16
            'mGr.DrawString("(Max)", pFont, Brushes.Black, pPt)

            If (Cavity_Exists_In) Then
                mGr.DrawString("(Max)", pFont, Brushes.Black, pPt)
            Else
                mGr.DrawString("(Max)", pFont, Brushes.White, pPt)
            End If

        ElseIf mSealSel.POrient = "Internal" Then
            '....AES 08MAR16
            'mGr.DrawString("(Min)", pFont, Brushes.Black, pPt)

            If (Cavity_Exists_In) Then
                mGr.DrawString("(Min)", pFont, Brushes.Black, pPt)
            Else
                mGr.DrawString("(Min)", pFont, Brushes.White, pPt)
            End If
        End If


        '....Horizontal dimension line - Right segment.
        With rightArrowPt
            .X = pCavityBotR.X + pcCORNER_RADIUS + 1.4
            .Y = leftArrowPt.Y
        End With

        With rightArrowPt
            pPt.X = .X - 0.4
            mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
        End With

    End Sub

    Public Sub DrawCavity_PicBox(ByVal cornerName_in As eBlockCornerName, _
                           ByVal cornerPoint_in As PointF, _
                           ByRef cavityBotR_out As PointF)
        '====================================================================
        'This routine creates the cavity drawing. 
        mGr.PageUnit = GraphicsUnit.Inch
        mDpX = mGr.DpiX     '....# of Pixels/in on the current Graphics Object.

        Const pcFLANGE_LENGTH As Single = 2.5
        Const pc_GAP As Single = 0.25
        Const pcCAVITY_LENGTH_TOP As Single = 0.75
        Const pcCAVITY_LENGTH_BOT As Single = 1.0#
        Const pcCAVITY_DEPTH As Single = 0.5
        Const pcCHAMFER_LENGTH As Single = 0.05
        Const pcCORNER_RADIUS As Single = 0.1 '0.05

        '....Temporary variables
        Dim begPoint As PointF, endPoint As PointF

        'Define Pen. 
        '----------
        Dim pPenWid As Int16
        If mDisplayType = "Printer" Then
            pPenWid = 8
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
        End If
        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)


        'Flange (Top surface)
        '--------------------
        Dim pFlangeL As PointF, pFlangeR As PointF

        If cornerName_in = eBlockCornerName.TopL Then

            With pFlangeL

                If mDisplayType = "Printer" Then
                    .X = cornerPoint_in.X
                ElseIf mDisplayType = "PictureBox" Then
                    .X = cornerPoint_in.X - 0.7
                End If

                .Y = cornerPoint_in.Y
            End With

        End If


        If mDisplayType = "Printer" Then
            pFlangeR.X = pFlangeL.X + pcFLANGE_LENGTH

        ElseIf mDisplayType = "PictureBox" Then
            pFlangeR.X = pFlangeL.X + pcFLANGE_LENGTH - 0.1
        End If

        pFlangeR.Y = pFlangeL.Y

        mGr.DrawLine(pPen, pFlangeL.X, pFlangeL.Y, pFlangeR.X, pFlangeR.Y)


        'Cavity:
        '=======

        'Top Line - Segment 1 (Left Segment)
        '-----------------------------------
        Dim pCavityTop1L As PointF, pCavityTop1R As PointF

        With pCavityTop1L
            .X = pFlangeL.X
            .Y = pFlangeL.Y + pc_GAP
        End With

        With pCavityTop1R
            .X = pCavityTop1L.X + pcCAVITY_LENGTH_TOP - pcCHAMFER_LENGTH
            .Y = pCavityTop1L.Y
        End With

        mGr.DrawLine(pPen, pCavityTop1L.X, pCavityTop1L.Y, pCavityTop1R.X, pCavityTop1R.Y)


        'Top Line - Segment 2 (Right Segment)
        '------------------------------------
        Dim pCavityTop2L As PointF, pCavityTop2R As PointF

        With pCavityTop2L
            .X = pCavityTop1R.X + pcCAVITY_LENGTH_BOT + 2.0# * pcCHAMFER_LENGTH
            .Y = pCavityTop1R.Y
        End With


        With pCavityTop2R

            If mDisplayType = "Printer" Then
                .X = pCavityTop2L.X + pcCAVITY_LENGTH_TOP
            ElseIf mDisplayType = "PictureBox" Then
                .X = pCavityTop2L.X + pcCAVITY_LENGTH_TOP - 0.15
            End If

            .Y = pCavityTop2L.Y

        End With

        mGr.DrawLine(pPen, pCavityTop2L.X, pCavityTop2L.Y, _
                           pCavityTop2R.X, pCavityTop2R.Y)


        'Side Vertical Line 1 - Left
        '---------------------------
        Dim pCavitySide1Top As PointF, pCavitySide1Bot As PointF
        With pCavitySide1Top
            .X = pCavityTop1R.X + pcCHAMFER_LENGTH
            .Y = pCavityTop1R.Y + pcCHAMFER_LENGTH
        End With

        With pCavitySide1Bot
            .X = pCavitySide1Top.X
            .Y = pCavityTop1R.Y + pcCAVITY_DEPTH - pcCORNER_RADIUS
        End With

        mGr.DrawLine(pPen, pCavitySide1Top.X, pCavitySide1Top.Y, _
                           pCavitySide1Bot.X, pCavitySide1Bot.Y)


        'Side Vertical Line 1 - Right
        '----------------------------
        Dim pCavitySide2Top As PointF, pCavitySide2Bot As PointF
        With pCavitySide2Top
            .X = pCavityTop2L.X - pcCHAMFER_LENGTH
            .Y = pCavityTop2L.Y + pcCHAMFER_LENGTH
        End With

        With pCavitySide2Bot
            .X = pCavitySide2Top.X
            .Y = pCavityTop2L.Y + pcCAVITY_DEPTH - pcCORNER_RADIUS
        End With

        mGr.DrawLine(pPen, pCavitySide2Top.X, pCavitySide2Top.Y, _
                           pCavitySide2Bot.X, pCavitySide2Bot.Y)


        'Bottom Line
        '-----------
        Dim pCavityBotL As PointF, pCavityBotR As PointF
        With pCavityBotL
            .X = pCavitySide1Bot.X + pcCORNER_RADIUS
            .Y = pCavitySide1Bot.Y + pcCORNER_RADIUS
        End With

        With pCavityBotR
            .X = pCavityBotL.X + pcCAVITY_LENGTH_BOT - 2 * pcCORNER_RADIUS
            .Y = pCavityBotL.Y
        End With

        mGr.DrawLine(pPen, pCavityBotL.X, pCavityBotL.Y, pCavityBotR.X, pCavityBotR.Y)
        cavityBotR_out = pCavityBotR     '....Output parameter


        'Left Chamfer Line
        '-----------------
        mGr.DrawLine(pPen, pCavityTop1R.X, pCavityTop1R.Y, _
                           pCavitySide1Top.X, pCavitySide1Top.Y)


        'Right Chamfer Line
        '------------------
        mGr.DrawLine(pPen, pCavityTop2L.X, pCavityTop2L.Y, _
                           pCavitySide2Top.X, pCavitySide2Top.Y)


        'Left Corner Radius    
        '------------------
        Dim pRad As Single = pcCORNER_RADIUS

        Dim pCenter As PointF
        pCenter.X = pCavitySide1Bot.X + pRad
        pCenter.Y = pCavitySide1Bot.Y

        mGr.DrawArc(pPen, pCenter.X - pRad, pCenter.Y - pRad, pRad * 2, pRad * 2, 90, 90)


        '....Radius line and symbol
        Dim del As Single
        del = 0.707 * pcCORNER_RADIUS       '....COS45 = 0.707

        Dim pArrowPt As PointF
        With pArrowPt
            .X = pCenter.X - del
            .Y = pCenter.Y + del
        End With

        mGr.DrawLine(pPen, pCenter.X, pCenter.Y, pArrowPt.X, pArrowPt.Y)

        '....Draw Arrow.
        DrawArrow(pArrowPt, eArrowType.DiagArrowDown)

        '....Write 'r (Max)'.
        Dim pPt As PointF
        With pPt
            .X = pCenter.X + 0.02
            .Y = pCenter.Y - 0.15
        End With

        Dim pFontSize As Single = 11
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("r", pFont, Brushes.Black, pPt)


        With pPt
            .X = pCenter.X + 0.1
            .Y = pCenter.Y - 0.12
        End With

        pFontSize = 8
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("(Max)", pFont, Brushes.Black, pPt)


        'Right Corner Radius.       
        '--------------------
        pCenter.X = pCavitySide2Bot.X - pcCORNER_RADIUS
        pCenter.Y = pCavitySide2Bot.Y

        mGr.DrawArc(pPen, pCenter.X - pRad, pCenter.Y - pRad, pRad * 2, pRad * 2, 0, 90)


        'Auxilliary features drawing:
        '============================

        If mDisplayType = "Printer" Then
            pPenWid = 4
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 1
        End If


        'Surface finish specification on Flange.
        '---------------------------------------
        Dim pbotPoint As PointF
        pbotPoint.X = 0.5 * (pFlangeL.X + pFlangeR.X)
        pbotPoint.Y = pFlangeL.Y

        With pPt
            .X = pbotPoint.X - 0.08
            .Y = pbotPoint.Y - 0.15
        End With

        pFontSize = 7
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("32", pFont, Brushes.Black, pPt)


        With pPt
            .X = pbotPoint.X + 0.08
            .Y = pbotPoint.Y - 0.11
        End With

        mGr.DrawString("C", pFont, Brushes.Black, pPt)
        DrawAngle(pbotPoint)


        'Dimension lines :
        '================

        'Cavity total depth 'h':
        '-----------------------
        '....Top horizontal line.
        With begPoint
            .X = pFlangeR.X + 0.1
            .Y = pFlangeR.Y
        End With

        With endPoint
            .X = begPoint.X + 0.2
            .Y = pFlangeR.Y
        End With

        pPen = New Pen(Color.Black, pPenWid / mDpX)
        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        '....Vertical dimension line - Upper segment.
        Dim topArrowPt As PointF
        With topArrowPt
            .X = 0.5 * (begPoint.X + endPoint.X)
            .Y = 0.5 * (begPoint.Y + endPoint.Y)
        End With
        DrawArrow(topArrowPt, eArrowType.VertArrowUp)

        pPt.Y = topArrowPt.Y + 0.25
        With topArrowPt
            mGr.DrawLine(pPen, .X, .Y, .X, pPt.Y)
        End With


        '....Write 'h'
        With pPt
            .X = topArrowPt.X - 0.02
            .Y = topArrowPt.Y + 0.3
            pFontSize = 11
        End With

        pFontSize = 11
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("h", pFont, Brushes.Black, pPt)


        '....Vertical dimension line - Lower segment.
        Dim botArrowPt As PointF
        With botArrowPt
            .X = topArrowPt.X
            .Y = topArrowPt.Y + pcCAVITY_DEPTH + pc_GAP
        End With
        DrawArrow(botArrowPt, eArrowType.VertArrowDown)

        pPt.Y = botArrowPt.Y - 0.25
        With botArrowPt
            mGr.DrawLine(pPen, .X, .Y, .X, pPt.Y)
        End With


        '....Bottom horizontal line.
        With begPoint
            .X = pCavityBotR.X + 0.1
            .Y = pCavityBotR.Y
        End With

        With endPoint
            .X = begPoint.X + 1.25
            .Y = begPoint.Y
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        'Cavity width 'W'
        '----------------
        '....Left vertical line.

        With begPoint
            .X = pCavityBotL.X - pcCORNER_RADIUS
            .Y = pCavityBotL.Y + 0.1
        End With

        With endPoint
            .X = begPoint.X
            .Y = begPoint.Y + 0.2
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        Dim leftArrowPt As PointF
        With leftArrowPt
            .X = 0.5 * (begPoint.X + endPoint.X)
            .Y = 0.5 * (begPoint.Y + endPoint.Y)
        End With
        DrawArrow(leftArrowPt, eArrowType.HorzArrowLeft)


        '....Horizontal dimension line - Left segment.
        pPt.X = leftArrowPt.X + 0.4
        With leftArrowPt
            mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
        End With

        '....Write 'W'
        With pPt
            .X = leftArrowPt.X + 0.45
            .Y = leftArrowPt.Y - 0.1
        End With

        pFontSize = 11
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("w", pFont, Brushes.Black, pPt)


        '....Horizontal dimension line - Right segment.
        Dim rightArrowPt As PointF
        With rightArrowPt
            .X = leftArrowPt.X + pcCAVITY_LENGTH_BOT
            .Y = leftArrowPt.Y
        End With

        DrawArrow(rightArrowPt, eArrowType.HorzArrowRight)

        pPt.X = rightArrowPt.X - 0.4
        With rightArrowPt
            mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
        End With


        '....Right vertical line.
        With begPoint
            .X = pCavityBotR.X + pcCORNER_RADIUS
            .Y = pCavityBotR.Y + 0.1
        End With

        With endPoint
            .X = begPoint.X
            .Y = begPoint.Y + 0.2
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)


        'CAVITY DIAMETER
        '---------------
        '....Left vertical line.
        If mSealSel.POrient = "External" Then

            With begPoint
                .X = pCavityBotR.X + pcCORNER_RADIUS
                .Y = pCavityBotR.Y + 0.3 + 0.05
            End With

        ElseIf mSealSel.POrient = "Internal" Then

            With begPoint
                .X = pCavityBotL.X - pcCORNER_RADIUS
                .Y = pCavityBotL.Y + 0.3 + 0.05
            End With

        End If


        With endPoint
            .X = begPoint.X
            .Y = begPoint.Y + 0.2
        End With

        mGr.DrawLine(pPen, begPoint.X, begPoint.Y, endPoint.X, endPoint.Y)

        With leftArrowPt
            .X = 0.5 * (begPoint.X + endPoint.X)
            .Y = 0.5 * (begPoint.Y + endPoint.Y)
        End With


        '....Horizontal dimension line - Left segment.
        DrawArrow(leftArrowPt, eArrowType.HorzArrowLeft)

        With leftArrowPt
            If mSealSel.POrient = "External" Then
                pPt.X = .X + 0.5
                mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)

            ElseIf mSealSel.POrient = "Internal" Then
                pPt.X = .X + pcCAVITY_LENGTH_BOT + 0.5
                mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
            End If
        End With


        '....Write 'CAVITY DIAMETER (Max)/(Min)'
        Dim xPos As Single, yPos As Single
        xPos = pCavityBotR.X + pcCORNER_RADIUS + 0.6
        yPos = leftArrowPt.Y - 0.1

        With pPt
            .X = xPos
            .Y = yPos
        End With

        pFontSize = 7
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)
        mGr.DrawString("CAVITY", pFont, Brushes.Black, pPt)


        xPos = xPos - 0.07
        yPos = yPos + 0.13

        With pPt
            .X = xPos
            .Y = yPos
        End With

        mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)

        xPos = xPos + 0.12
        yPos = yPos + 0.14

        With pPt
            .X = xPos
            .Y = yPos
        End With

        If mSealSel.POrient = "External" Then
            mGr.DrawString("(Max)", pFont, Brushes.Black, pPt)

        ElseIf mSealSel.POrient = "Internal" Then
            mGr.DrawString("(Min)", pFont, Brushes.Black, pPt)
        End If


        '....Horizontal dimension line - Right segment.
        With rightArrowPt
            .X = pCavityBotR.X + pcCORNER_RADIUS + 1.4
            .Y = leftArrowPt.Y
        End With

        With rightArrowPt
            pPt.X = .X - 0.4
            mGr.DrawLine(pPen, .X, .Y, pPt.X, .Y)
        End With

    End Sub


    Private Sub DrawSealEndView(ByVal drawBoxTopL_In As PointF, _
                                ByVal drawBoxBotR_In As PointF)
        '=========================================================================
        'This routine draws the end view of the seal. 

        'Input  parameters:   drawBoxTopL_In} - Drawing box top left corner point &
        '                     drawBoxBotR_In}   bottom right corner point.
        '                     Drawing box is for reference purpose only and not drawn.

        Dim pcUNIFORM_MARGIN As Single
        Dim pcSecLineLen As Single

        '....Uniform margin around the graphics & End View - Sectioning line length. 
        If mDisplayType = "Printer" Then
            pcUNIFORM_MARGIN = 0.4
            pcSecLineLen = 0.9
        ElseIf mDisplayType = "PictureBox" Then
            pcUNIFORM_MARGIN = 0.3
            pcSecLineLen = 0.72
        End If


        'Get Seal OD & ID (always in inch irrespective of the unit system chosen)
        '------------------------------------------------------------------------
        '
        Dim psngDControl As Single, psngSealWid As Single
        psngDControl = mSealSel.DControl
        psngSealWid = mSealSel.Wid

        Dim psngSealOD As Single, psngSealID As Single

        If mSealSel.POrient = "External" Then
            psngSealID = psngDControl
            psngSealOD = psngSealID + 2 * psngSealWid

        ElseIf mSealSel.POrient = "Internal" Then
            psngSealOD = psngDControl
            psngSealID = psngSealOD - 2 * psngSealWid
        End If


        'Establish appropriate scale.
        '----------------------------
        '...Based on the X direction :
        Dim scaleX As Single, xSpaceAvailable As Single

        xSpaceAvailable = drawBoxBotR_In.X - drawBoxTopL_In.X - 2.0# * pcUNIFORM_MARGIN
        scaleX = xSpaceAvailable / psngSealOD

        '...Based on the Y direction :
        Dim scaleY As Single, ySpaceAvailable As Single

        ySpaceAvailable = drawBoxBotR_In.Y - drawBoxTopL_In.Y - 2.0# * pcUNIFORM_MARGIN
        scaleY = ySpaceAvailable / psngSealOD


        '...Choose the smaller of the two scales.
        Dim scaleVB As Single
        scaleVB = IIf(scaleX <= scaleY = True, scaleX, scaleY)


        '...Outside & inside radii of the circular annulus in the drawing scale.
        Dim rOut As Single, rIn As Single, delR As Single

        rOut = 0.5 * psngSealOD * scaleVB
        rIn = 0.5 * psngSealID * scaleVB
        delR = rOut - rIn

        '...Modified margins
        Dim marginX As Single, marginY As Single

        marginX = 0.5 * (drawBoxBotR_In.X - drawBoxTopL_In.X - psngSealOD * scaleVB)
        marginY = 0.5 * (drawBoxBotR_In.Y - drawBoxTopL_In.Y - psngSealOD * scaleVB)


        'Draw the Seal End View
        '----------------------
        Dim Center As PointF
        Dim pPenWid As Int16

        Center.X = drawBoxTopL_In.X + marginX + rOut
        Center.Y = drawBoxTopL_In.Y + marginY + rOut

        If mDisplayType = "Printer" Then
            pPenWid = 8
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 2
        End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)

        mGr.DrawEllipse(pPen, Center.X - rOut, Center.Y - rOut, 2 * rOut, 2 * rOut)
        mGr.DrawEllipse(pPen, Center.X - rIn, Center.Y - rIn, 2 * rIn, 2 * rIn)

        Dim pAngSegment As Single = 360 / mSealSel.CountSegment

        Dim pLoadAngleLinePtBeg_Line1 As New PointF()
        Dim pLoadAngleLinePtEnd_Line1 As New PointF()

        Dim pLoadAngleLinePtBeg_Line2 As New PointF()
        Dim pLoadAngleLinePtEnd_Line2 As New PointF()

        If (mSealSel.IsSegmented) Then

            If (mSealSel.CountSegment Mod 2 = 0) Then

                pPen = New Pen(Color.White, pPenWid / mDpX)
                mGr.DrawArc(pPen, Center.X - rOut, Center.Y - rOut, rOut * 2, rOut * 2, -2, 4)
                mGr.DrawArc(pPen, Center.X - rIn, Center.Y - rIn, rIn * 2, rIn * 2, -2, 4)

                pPen = New Pen(Color.Black, pPenWid / mDpX)
                pLoadAngleLinePtBeg_Line1 = TransCylnToCart(Center, rIn, -2)
                pLoadAngleLinePtEnd_Line1 = TransCylnToCart(Center, rOut, -2)
                mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line1, pLoadAngleLinePtEnd_Line1)    '....Line1

                pLoadAngleLinePtBeg_Line2 = TransCylnToCart(Center, rIn, 2)
                pLoadAngleLinePtEnd_Line2 = TransCylnToCart(Center, rOut, 2)
                mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line2, pLoadAngleLinePtEnd_Line2)    '....Line2


                Dim pAng As Single = 0
                For i As Integer = 0 To mSealSel.CountSegment - 2
                    pPen = New Pen(Color.Black, pPenWid / mDpX)
                    pAng = pAng + pAngSegment

                    pPen = New Pen(Color.White, pPenWid / mDpX)
                    mGr.DrawArc(pPen, Center.X - rOut, Center.Y - rOut, rOut * 2, rOut * 2, pAng - 2, 4)
                    mGr.DrawArc(pPen, Center.X - rIn, Center.Y - rIn, rIn * 2, rIn * 2, pAng - 2, 4)

                    pPen = New Pen(Color.Black, pPenWid / mDpX)
                    pLoadAngleLinePtBeg_Line1 = TransCylnToCart(Center, rIn, pAng - 2)
                    pLoadAngleLinePtEnd_Line1 = TransCylnToCart(Center, rOut, pAng - 2)
                    mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line1, pLoadAngleLinePtEnd_Line1)    '....Line1

                    pLoadAngleLinePtBeg_Line2 = TransCylnToCart(Center, rIn, pAng + 2)
                    pLoadAngleLinePtEnd_Line2 = TransCylnToCart(Center, rOut, pAng + 2)
                    mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line2, pLoadAngleLinePtEnd_Line2)    '....Line2

                Next

            Else

                pPen = New Pen(Color.White, pPenWid / mDpX)
                mGr.DrawArc(pPen, Center.X - rOut, Center.Y - rOut, rOut * 2, rOut * 2, 268, 4)
                mGr.DrawArc(pPen, Center.X - rIn, Center.Y - rIn, rIn * 2, rIn * 2, 268, 4)

                pPen = New Pen(Color.Black, pPenWid / mDpX)
                pLoadAngleLinePtBeg_Line1 = TransCylnToCart(Center, rIn, 90 - 2)
                pLoadAngleLinePtEnd_Line1 = TransCylnToCart(Center, rOut, 90 - 2)
                mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line1, pLoadAngleLinePtEnd_Line1)    '....Line1

                'pPen = New Pen(Color.Black, pPenWid / mDpX)
                pLoadAngleLinePtBeg_Line2 = TransCylnToCart(Center, rIn, 90 + 2)
                pLoadAngleLinePtEnd_Line2 = TransCylnToCart(Center, rOut, 90 + 2)
                mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line2, pLoadAngleLinePtEnd_Line2)    '....Line2

                Dim pAng As Single = 90
                Dim pStartAng As Single = 270
                For i As Integer = 0 To mSealSel.CountSegment - 2

                    pAng = pAng - pAngSegment
                    pStartAng = pStartAng + pAngSegment

                    If (pStartAng >= 360) Then
                        pStartAng = pStartAng - 360
                    End If

                    pPen = New Pen(Color.White, pPenWid / mDpX)
                    mGr.DrawArc(pPen, Center.X - rOut, Center.Y - rOut, rOut * 2, rOut * 2, pStartAng - 2, 4)
                    mGr.DrawArc(pPen, Center.X - rIn, Center.Y - rIn, rIn * 2, rIn * 2, pStartAng - 2, 4)

                    pPen = New Pen(Color.Black, pPenWid / mDpX)
                    pLoadAngleLinePtBeg_Line1 = TransCylnToCart(Center, rIn, pAng - 2)
                    pLoadAngleLinePtEnd_Line1 = TransCylnToCart(Center, rOut, pAng - 2)
                    mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line1, pLoadAngleLinePtEnd_Line1)    '....Line1

                    'pPen = New Pen(Color.Black, pPenWid / mDpX)
                    pLoadAngleLinePtBeg_Line2 = TransCylnToCart(Center, rIn, pAng + 2)
                    pLoadAngleLinePtEnd_Line2 = TransCylnToCart(Center, rOut, pAng + 2)
                    mGr.DrawLine(pPen, pLoadAngleLinePtBeg_Line2, pLoadAngleLinePtEnd_Line2)    '....Line2

                Next
            End If

        End If

        '----------------------
        'Draw the Sectioning lines
        '-------------------------
        If mDisplayType = "Printer" Then
            pPenWid = 4
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 1
        End If

        pPen = New Pen(Color.Black, pPenWid / mDpX)

        '...Vertical line
        Dim lineTop As PointF
        lineTop.X = Center.X
        lineTop.Y = Center.Y - rOut - 0.5 * (pcSecLineLen - delR)

        Dim lineBot As PointF
        lineBot.X = Center.X
        lineBot.Y = Center.Y - rIn + 0.5 * (pcSecLineLen - delR)

        mGr.DrawLine(pPen, lineTop.X, lineTop.Y, lineBot.X, lineBot.Y)


        '...Letters 'A-A'
        Dim pPt As PointF
        With pPt
            .X = lineTop.X - 0.2
            .Y = lineTop.Y - 0.05
        End With

        Dim pFontSize As Single = 11
        Dim pFont As New Font("Arial", pFontSize, FontStyle.Bold)
        mGr.DrawString("A", pFont, Brushes.Black, pPt)

        With pPt
            .X = lineBot.X - 0.2
            .Y = lineBot.Y
        End With

        mGr.DrawString("A", pFont, Brushes.Black, pPt)


        '...Horizontal lines w/ arrows
        Dim endPt As PointF

        endPt.X = lineTop.X + 0.3 : endPt.Y = lineTop.Y
        mGr.DrawLine(pPen, lineTop.X, lineTop.Y, endPt.X, endPt.Y)
        DrawArrow(endPt, eArrowType.HorzArrowRight)

        endPt.X = lineBot.X + 0.3 : endPt.Y = lineBot.Y
        mGr.DrawLine(pPen, lineBot.X, lineBot.Y, endPt.X, endPt.Y)
        DrawArrow(endPt, eArrowType.HorzArrowRight)

    End Sub


    Private Function TransCylnToCart(Center_In As PointF, Rad_In As Single, LoadAngle_In As Single) As PointF
        '====================================================================================================
        Dim pPoint As New PointF()
        'Dim pP As Single = Convert.ToSingle(Math.Cos(LoadAngle_In * mcFac_Rad))
        pPoint.X = Convert.ToSingle((Rad_In * (Math.Cos(LoadAngle_In * mcFac_Rad))))
        pPoint.Y = Convert.ToSingle((Rad_In * (Math.Sin(LoadAngle_In * mcFac_Rad))))
        If pPoint.X > 0 AndAlso pPoint.Y > 0 Then
            'For 0 to 90 Degree
            pPoint.X = Center_In.X + (pPoint.X)

            pPoint.Y = Center_In.Y - pPoint.Y

        ElseIf pPoint.X < 0 AndAlso pPoint.Y > 0 Then
            'For 91 to 180 Degree
            pPoint.X = Center_In.X + (pPoint.X)
            pPoint.Y = Center_In.Y - (pPoint.Y)
        ElseIf pPoint.X < 0 AndAlso pPoint.Y < 0 Then
            'For 181 to 270 Degree
            pPoint.X = Center_In.X + (pPoint.X)
            pPoint.Y = Center_In.Y + Math.Abs(pPoint.Y)
        ElseIf pPoint.X > 0 AndAlso pPoint.Y <= 0 Then
            'For 271 to 360 Degree
            pPoint.X = Center_In.X + pPoint.X
            pPoint.Y = Center_In.Y + Math.Abs(pPoint.Y)
        End If

        Return pPoint

    End Function


    Private Sub DrawSealCrossSec(ByVal borderTopL_in As PointF, _
                                 ByVal borderBotR_in As PointF, _
                                 ByVal drawBoxTopL_in As PointF, _
                                 ByVal drawBoxBotR_in As PointF)
        '=========================================================
        'This routine draws the cross-section of the seal.

        'Input  parameters :
        '-----------------
        '   borderTopL_in  - Border:   Top left corner point &
        '   borderBotR_in  -           Bottom right corner point
        '                              of the entire drawing area.
        '
        '   drawBoxTopL_in - Draw box: Top left corner point &
        '   drawBoxBotR_in -           Bottom right corner point.
        '                              of the cross-sec envelope. Drawing box is for
        '                              reference purpose only and not drawn.


        '....Uniform margin (in) around the graphics
        Const pcUNIFORM_MARGIN As Single = 0.75

        Dim cFacLUUnit As Single
        cFacLUUnit = mUnitSel.CFacConL / mUnitSel.CFacUserL   '....in ==> in (English)
        '                                                     '....in ==> mm (Metric)


        'Margins around the graphics
        '---------------------------
        '
        If mDisplayType = "Printer" Then
            '---------------------------
            'Additional margin is kept on the LHS (that's why a factor 2 is used in the
            '....first statement below).
            mMargin(1) = (drawBoxTopL_in.X + 2.0# * pcUNIFORM_MARGIN) - borderTopL_in.X
            mMargin(2) = borderBotR_in.X - drawBoxBotR_in.X + pcUNIFORM_MARGIN

            '....0.47 added. 07AUG06.
            mMargin(3) = (drawBoxTopL_in.Y + pcUNIFORM_MARGIN) - borderTopL_in.Y + 0.47
            mMargin(4) = borderBotR_in.Y - drawBoxBotR_in.Y + pcUNIFORM_MARGIN - 0.47


        ElseIf mDisplayType = "PictureBox" Then
            '----------------------------------

            'mMargin(1) = 4.0 - 3.7# '4.0# - 3.85  '...Left
            'mMargin(2) = 1.2# '1.0#         '...Right
            'mMargin(3) = 1.2# + 1.9 '1.0# + 1.9   '...Top              
            'mMargin(4) = 2.0# - 0.43 '2.0# - 0.68 '- 0.45  '...Bottom 

            mMargin(1) = (drawBoxTopL_in.X + 1.0# * pcUNIFORM_MARGIN) - borderTopL_in.X
            mMargin(2) = borderBotR_in.X - drawBoxBotR_in.X + pcUNIFORM_MARGIN

            mMargin(3) = (drawBoxTopL_in.Y + pcUNIFORM_MARGIN) - borderTopL_in.Y '+ 0.47    
            mMargin(4) = borderBotR_in.Y - drawBoxBotR_in.Y + pcUNIFORM_MARGIN - 0.47


        End If

        '....Convert the margin from English Unit ===> User Unit (in or mm).
        Dim i As Integer
        For i = 1 To 4
            mMargin(i) = mUnitSel.EngLToUserL(mMargin(i))
        Next


        'Draw the cross-section:
        '=======================

        'Graphics Settings:
        '------------------
        '....Array Index = 0 ===> "Standard Geometry"
        '....Array Index = 1 ===> "Adjusted Geometry"

        '....Color:
        Dim pColor(1) As Color
        pColor(0) = Color.Black
        pColor(1) = Color.Blue


        '....Drawing Width:
        Dim pDrawWid(2) As Integer
        If mDisplayType = "Printer" Then
            pDrawWid(0) = 8
        ElseIf mDisplayType = "PictureBox" Then
            pDrawWid(0) = 2
        End If

        pDrawWid(1) = 8                     '....Not needed.


        '....Dash Style:
        Dim pDashStyle(1) As Integer
        pDashStyle(0) = DashStyle.Solid     '....Value = 0
        pDashStyle(1) = DashStyle.DashDot   '....Value = 1   


        '....Size of the graphics area in the "page unit" system.
        '
        Dim pWid As Single = mUnitSel.EngLToUserL(borderBotR_in.X - borderTopL_in.X)
        Dim pHt As Single = mUnitSel.EngLToUserL(borderBotR_in.Y - borderTopL_in.Y)
        Dim pSize As New SizeF(pWid, pHt)

        If mUnitSel.System = "Metric" Then
            mGr.PageUnit = GraphicsUnit.Millimeter
        End If


        Dim sealEnvpTopL As PointF
        Dim sealEnvpBotR As PointF

        '....Draw only the "Adjusted" Seal Geometry.
        If mSealSel.Type = "E-Seal" Then
            CType(mSealSel, IPE_clsESeal).Draw(mGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                           "ADJ", "SCALE_BY_STD", 1.0, _
                                           sealEnvpTopL, sealEnvpBotR)


        ElseIf mSealSel.Type = "C-Seal" Then
            CType(mSealSel, IPE_clsCSeal).Draw(mGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                           "ADJ", "SCALE_BY_STD", 1.0, _
                                           sealEnvpTopL.X, sealEnvpTopL.Y, _
                                           sealEnvpBotR.X, sealEnvpBotR.Y)

        ElseIf mSealSel.Type = "U-Seal" Then
            CType(mSealSel, IPE_clsUSeal).Draw(mGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                           "ADJ", "SCALE_BY_STD", 1.0, _
                                           sealEnvpTopL, sealEnvpBotR)

        End If


        'Draw the seal envelope:
        '-----------------------
        '...Do the rest of drawings (e.g. labeling) in PageUnit = Inch. 

        If mUnitSel.System = "Metric" Then
            mGr.PageUnit = GraphicsUnit.Inch
        End If

        With sealEnvpTopL
            .X = .X / cFacLUUnit        '....User Unit L ===> in
            .Y = .Y / cFacLUUnit
        End With

        With sealEnvpBotR
            .X = .X / cFacLUUnit
            .Y = .Y / cFacLUUnit
        End With


        'Draw the Dimension Lines & Texts:
        '---------------------------------
        Dim pPenWid As Int16

        If mDisplayType = "Printer" Then
            pPenWid = 4
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 1
        End If

        Dim pPen As Pen
        pPen = New Pen(Color.Blue, (pPenWid / mDpX))

        '....Mainly for checking.
        'mGr.DrawLine(pPen, sealEnvpTopL.X, sealEnvpTopL.Y, _
        '                        sealEnvpBotR.X, sealEnvpBotR.Y)


        pPen = New Pen(Color.Black, (pPenWid / mDpX))

        Dim pFontSize As Single
        pFontSize = 7

        Dim pFont As Font
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        Dim begPt As PointF, endPt As PointF


        '   'FREE HEIGHT' label         
        '   -------------------
        Const pcLEN_FREE_HEIGHT_7PTS As Single = 0.625

        Dim lenLine As Single       ' Length of each horizontal line on either side.
        lenLine = 0.5 * ((sealEnvpBotR.X - sealEnvpTopL.X) - _
                          pcLEN_FREE_HEIGHT_7PTS - 0.2)

        Dim yMid As Single
        Dim pPt As PointF


        If mSealSel.POrient = "External" Then
            '--------------------------------

            '....Vertical lines:
            '    ....LHS.       
            begPt.X = sealEnvpTopL.X : begPt.Y = sealEnvpTopL.Y - 0.4 '- 0.35
            endPt.X = sealEnvpTopL.X : endPt.Y = sealEnvpTopL.Y - 0.2 '- 0.1

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

            '   ....RHS.
            begPt.X = sealEnvpBotR.X
            endPt.X = sealEnvpBotR.X

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Horizontal lines:
            yMid = 0.5 * (begPt.Y + endPt.Y)

            '   ....LHS.
            begPt.X = sealEnvpTopL.X : begPt.Y = yMid
            endPt.X = sealEnvpTopL.X + lenLine : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.HorzArrowLeft)


            '   ....RHS.
            begPt.X = sealEnvpBotR.X - lenLine : begPt.Y = yMid
            endPt.X = sealEnvpBotR.X : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(endPt, eArrowType.HorzArrowRight)


            '.....Write the text.
            With pPt
                .X = sealEnvpTopL.X + lenLine + 0.05
                .Y = yMid - 0.08 '- 0.1        
            End With

            mGr.DrawString("FREE HEIGHT", pFont, Brushes.Black, pPt)


        ElseIf mSealSel.POrient = "Internal" Then
            '------------------------------------

            '....Vertical lines:
            '    ....LHS.
            begPt.X = sealEnvpTopL.X : begPt.Y = sealEnvpBotR.Y + 0.4 '+ 0.35
            endPt.X = sealEnvpTopL.X : endPt.Y = sealEnvpBotR.Y + 0.2 '+ 0.1

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

            '   ....RHS
            begPt.X = sealEnvpBotR.X
            endPt.X = sealEnvpBotR.X
            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Horizontal lines:
            yMid = 0.5 * (begPt.Y + endPt.Y)

            '   ....LHS.
            begPt.X = sealEnvpTopL.X : begPt.Y = yMid
            endPt.X = sealEnvpTopL.X + lenLine : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.HorzArrowLeft)

            '   ....RHS.
            begPt.X = sealEnvpBotR.X - lenLine : begPt.Y = yMid
            endPt.X = sealEnvpBotR.X : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(endPt, eArrowType.HorzArrowRight)


            '.....Write the text
            With pPt
                .X = sealEnvpTopL.X + lenLine + 0.05
                .Y = yMid '- 0.08 '- 0.1
            End With

            mGr.DrawString("FREE HEIGHT", pFont, Brushes.Black, pPt)
        End If


        '   'WIDTH' label
        '   -------------
        '
        '....Horizontal lines:
        Const pcHorzShift As Single = 0.4

        '   ....Top.
        begPt.X = sealEnvpBotR.X + pcHorzShift : begPt.Y = sealEnvpTopL.Y
        endPt.X = begPt.X + 0.25 : endPt.Y = sealEnvpTopL.Y

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

        '   ....Bot.
        begPt.X = sealEnvpBotR.X + pcHorzShift : begPt.Y = sealEnvpBotR.Y
        endPt.X = begPt.X + 0.25 : endPt.Y = sealEnvpBotR.Y

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


        '....Vertical lines:
        Const pcHT_WIDTH_7PTS As Single = 0.06
        Const pcLEN_WIDTH_7PTS As Single = 8 / 25.4        ' 8 mm
        lenLine = 0.5 * ((sealEnvpBotR.Y - sealEnvpTopL.Y) - _
                          pcHT_WIDTH_7PTS - 0.2)

        Dim xMid As Single
        xMid = 0.5 * (begPt.X + endPt.X)

        '   ....Top.
        begPt.X = xMid : begPt.Y = sealEnvpTopL.Y
        endPt.X = xMid : endPt.Y = begPt.Y + lenLine

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
        DrawArrow(begPt, eArrowType.VertArrowUp)

        '   ....Bot.
        begPt.X = xMid : begPt.Y = sealEnvpBotR.Y
        endPt.X = xMid : endPt.Y = begPt.Y - lenLine

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
        DrawArrow(begPt, eArrowType.VertArrowDown)


        '   ....Write the text
        With pPt
            .X = sealEnvpBotR.X + pcHorzShift + 0.5 * (0.25 - pcLEN_WIDTH_7PTS)
            .Y = sealEnvpTopL.Y + lenLine + 0.1
        End With

        mGr.DrawString("WIDTH", pFont, Brushes.Black, pPt)


        '   'DIAMETER' label:
        '   -----------------
        Const pcLEN_DIAMETER_7PTS As Single = 0.5
        Const pcHT_DIAMETER_7PTS As Single = 0.06


        If mSealSel.POrient = "External" Then
            '--------------------------------

            '....Horizontal line.                
            If mDisplayType = "Printer" Then
                endPt.X = sealEnvpTopL.X + 0.1 : endPt.Y = sealEnvpBotR.Y
                begPt.X = endPt.X - 0.5 : begPt.Y = endPt.Y

            ElseIf mDisplayType = "PictureBox" Then
                endPt.X = sealEnvpTopL.X + 0.1 : endPt.Y = sealEnvpBotR.Y
                begPt.X = endPt.X - 0.4 : begPt.Y = endPt.Y
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Vertical lines: 
            '
            '   ....Top.
            xMid = 0.5 * (begPt.X + endPt.X)

            begPt.X = xMid : begPt.Y = sealEnvpBotR.Y
            endPt.X = begPt.X

            If mDisplayType = "Printer" Then
                endPt.Y = begPt.Y + 0.35
            ElseIf mDisplayType = "PictureBox" Then
                endPt.Y = begPt.Y + 0.15
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.VertArrowUp)


            '....Write the text:
            With pPt
                .X = endPt.X - 0.5 * pcLEN_DIAMETER_7PTS
                .Y = endPt.Y + 0.05
            End With

            mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)

            '   ....Bot.
            begPt.X = xMid
            endPt.X = xMid

            If mDisplayType = "Printer" Then
                begPt.Y = endPt.Y + 0.2 + pcHT_DIAMETER_7PTS
                endPt.Y = begPt.Y + 0.35

            ElseIf mDisplayType = "PictureBox" Then
                begPt.Y = endPt.Y + 0.1 + pcHT_DIAMETER_7PTS
                endPt.Y = begPt.Y + 0.15
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


        ElseIf mSealSel.POrient = "Internal" Then
            '------------------------------------
            '
            '....Horizontal line.           
            If mDisplayType = "Printer" Then
                endPt.X = sealEnvpTopL.X - 0.1 : endPt.Y = sealEnvpTopL.Y
                begPt.X = endPt.X - 0.6 : begPt.Y = endPt.Y

            ElseIf mDisplayType = "PictureBox" Then
                endPt.X = sealEnvpTopL.X - 0.1 : endPt.Y = sealEnvpTopL.Y
                begPt.X = endPt.X - 0.4 : begPt.Y = endPt.Y
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Vertical lines: 
            '
            '   ....Top.
            xMid = 0.5 * (begPt.X + endPt.X)
            Dim vLineLen As Single
            vLineLen = 0.5 * (sealEnvpBotR.Y - sealEnvpTopL.Y)

            begPt.X = xMid
            begPt.Y = sealEnvpTopL.Y

            endPt.X = begPt.X
            endPt.Y = begPt.Y + vLineLen

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.VertArrowUp)


            '.....Write the text.
            With pPt
                .X = endPt.X - 0.5 * pcLEN_DIAMETER_7PTS
                .Y = endPt.Y + 0.05
            End With

            mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)

            '   ....Bot.
            begPt.X = xMid : begPt.Y = endPt.Y + 0.2 + pcHT_DIAMETER_7PTS
            endPt.X = begPt.X : endPt.Y = begPt.Y + vLineLen

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

        End If


        '   'SECTION A-A' label:
        '   --------------------
        '
        Const pcLEN_SECTIONAA_11PTS As Single = 1.0#            '....inch

        If mDisplayType = "Printer" Then
            pFontSize = 11
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 8
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Bold Or FontStyle.Underline)

        '.........Write the text
        Dim delXfromLHS As Single
        delXfromLHS = 0.5 * ((sealEnvpBotR.X - sealEnvpTopL.X) - _
                              pcLEN_SECTIONAA_11PTS)

        If mDisplayType = "Printer" Then

            With pPt
                .X = sealEnvpTopL.X + delXfromLHS

                If mSealSel.POrient = "External" Then
                    .Y = sealEnvpBotR.Y + 0.5
                ElseIf mSealSel.POrient = "Internal" Then
                    .Y = sealEnvpTopL.Y - 0.5
                End If

            End With


        ElseIf mDisplayType = "PictureBox" Then

            With pPt
                .X = sealEnvpTopL.X + delXfromLHS + 0.1

                If mSealSel.POrient = "External" Then
                    .Y = sealEnvpBotR.Y + 0.2
                ElseIf mSealSel.POrient = "Internal" Then
                    .Y = sealEnvpTopL.Y - 0.35
                End If

            End With

        End If

        mGr.DrawString("SECTION  A-A", pFont, Brushes.Black, pPt)

    End Sub


    Public Sub DrawSealCrossSec_PicBox(ByVal borderTopL_in As PointF, _
                                       ByVal borderBotR_in As PointF, _
                                       ByVal drawBoxTopL_in As PointF, _
                                       ByVal drawBoxBotR_in As PointF)
        '================================================================
        'This routine draws the cross-section of the seal.

        mGr.PageUnit = GraphicsUnit.Inch
        mDpX = mGr.DpiX     '....# of Pixels/in on the current Graphics Object.

        'Input  parameters :
        '-----------------
        '   borderTopL_in  - Border:   Top left corner point &
        '   borderBotR_in  -           Bottom right corner point
        '                              of the entire drawing area.
        '
        '   drawBoxTopL_in - Draw box: Top left corner point &
        '   drawBoxBotR_in -           Bottom right corner point.
        '                              of the cross-sec envelope. Drawing box is for
        '                              reference purpose only and not drawn.


        '....Uniform margin (in) around the graphics
        Const pcUNIFORM_MARGIN As Single = 0.75

        Dim cFacLUUnit As Single
        cFacLUUnit = mUnitSel.CFacConL / mUnitSel.CFacUserL   '....in ==> in (English)
        '                                                     '....in ==> mm (Metric)


        'Margins around the graphics
        '---------------------------
        '
        If mDisplayType = "Printer" Then
            '---------------------------
            'Additional margin is kept on the LHS (that's why a factor 2 is used in the
            '....first statement below).
            mMargin(1) = (drawBoxTopL_in.X + 2.0# * pcUNIFORM_MARGIN) - borderTopL_in.X
            mMargin(2) = borderBotR_in.X - drawBoxBotR_in.X + pcUNIFORM_MARGIN

            '....0.47 added. 07AUG06.
            mMargin(3) = (drawBoxTopL_in.Y + pcUNIFORM_MARGIN) - borderTopL_in.Y + 0.47
            mMargin(4) = borderBotR_in.Y - drawBoxBotR_in.Y + pcUNIFORM_MARGIN - 0.47


        ElseIf mDisplayType = "PictureBox" Then
            '----------------------------------

            'mMargin(1) = 4.0 - 3.7# '4.0# - 3.85  '...Left
            'mMargin(2) = 1.2# '1.0#         '...Right
            'mMargin(3) = 1.2# + 1.9 '1.0# + 1.9   '...Top              
            'mMargin(4) = 2.0# - 0.43 '2.0# - 0.68 '- 0.45  '...Bottom 

            mMargin(1) = (drawBoxTopL_in.X + 1.0# * pcUNIFORM_MARGIN) - borderTopL_in.X
            mMargin(2) = borderBotR_in.X - drawBoxBotR_in.X + pcUNIFORM_MARGIN

            mMargin(3) = (drawBoxTopL_in.Y + pcUNIFORM_MARGIN) - borderTopL_in.Y '+ 0.47    
            mMargin(4) = borderBotR_in.Y - drawBoxBotR_in.Y + pcUNIFORM_MARGIN - 0.47

            'mMargin(3) = (drawBoxTopL_in.Y + pcUNIFORM_MARGIN) - borderTopL_in.Y + 0.4
            'mMargin(4) = borderBotR_in.Y - drawBoxBotR_in.Y + pcUNIFORM_MARGIN '- 0.47


        End If

        '....Convert the margin from English Unit ===> User Unit (in or mm).
        Dim i As Integer
        For i = 1 To 4
            mMargin(i) = mUnitSel.EngLToUserL(mMargin(i))
        Next


        'Draw the cross-section:
        '=======================

        'Graphics Settings:
        '------------------
        '....Array Index = 0 ===> "Standard Geometry"
        '....Array Index = 1 ===> "Adjusted Geometry"

        '....Color:
        Dim pColor(1) As Color
        pColor(0) = Color.Black
        pColor(1) = Color.Blue


        '....Drawing Width:
        Dim pDrawWid(2) As Integer
        If mDisplayType = "Printer" Then
            pDrawWid(0) = 8
        ElseIf mDisplayType = "PictureBox" Then
            pDrawWid(0) = 2
        End If

        pDrawWid(1) = 8                     '....Not needed.


        '....Dash Style:
        Dim pDashStyle(1) As Integer
        pDashStyle(0) = DashStyle.Solid     '....Value = 0
        pDashStyle(1) = DashStyle.DashDot   '....Value = 1   


        '....Size of the graphics area in the "page unit" system.
        '
        Dim pWid As Single = mUnitSel.EngLToUserL(borderBotR_in.X - borderTopL_in.X)
        Dim pHt As Single = mUnitSel.EngLToUserL(borderBotR_in.Y - borderTopL_in.Y)
        Dim pSize As New SizeF(pWid, pHt)

        If mUnitSel.System = "Metric" Then
            mGr.PageUnit = GraphicsUnit.Millimeter
        End If


        Dim sealEnvpTopL As PointF
        Dim sealEnvpBotR As PointF

        '....Draw only the "Adjusted" Seal Geometry.
        If mSealSel.Type = "E-Seal" Then
            CType(mSealSel, IPE_clsESeal).Draw(mGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                           "ADJ", "SCALE_BY_STD", 1.0, _
                                           sealEnvpTopL, sealEnvpBotR)


        ElseIf mSealSel.Type = "C-Seal" Then
            CType(mSealSel, IPE_clsCSeal).Draw(mGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                           "ADJ", "SCALE_BY_STD", 1.0, _
                                           sealEnvpTopL.X, sealEnvpTopL.Y, _
                                           sealEnvpBotR.X, sealEnvpBotR.Y)

        ElseIf mSealSel.Type = "U-Seal" Then
            CType(mSealSel, IPE_clsUSeal).Draw(mGr, pSize, mMargin, pColor, pDrawWid, pDashStyle, _
                                           "ADJ", "SCALE_BY_STD", 1.0, _
                                           sealEnvpTopL, sealEnvpBotR)

        End If


        'Draw the seal envelope:
        '-----------------------
        '...Do the rest of drawings (e.g. labeling) in PageUnit = Inch. 

        If mUnitSel.System = "Metric" Then
            mGr.PageUnit = GraphicsUnit.Inch
        End If

        With sealEnvpTopL
            .X = .X / cFacLUUnit        '....User Unit L ===> in
            .Y = .Y / cFacLUUnit
        End With

        With sealEnvpBotR
            .X = .X / cFacLUUnit
            .Y = .Y / cFacLUUnit
        End With


        'Draw the Dimension Lines & Texts:
        '---------------------------------
        Dim pPenWid As Int16

        If mDisplayType = "Printer" Then
            pPenWid = 4
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 1
        End If

        Dim pPen As Pen
        pPen = New Pen(Color.Blue, (pPenWid / mDpX))

        '....Mainly for checking.
        'mGr.DrawLine(pPen, sealEnvpTopL.X, sealEnvpTopL.Y, _
        '                        sealEnvpBotR.X, sealEnvpBotR.Y)


        pPen = New Pen(Color.Black, (pPenWid / mDpX))

        Dim pFontSize As Single
        pFontSize = 7

        Dim pFont As Font
        pFont = New Font("Arial", pFontSize, FontStyle.Regular)

        Dim begPt As PointF, endPt As PointF


        '   'FREE HEIGHT' label         
        '   -------------------
        Const pcLEN_FREE_HEIGHT_7PTS As Single = 0.625

        Dim lenLine As Single       ' Length of each horizontal line on either side.
        lenLine = 0.5 * ((sealEnvpBotR.X - sealEnvpTopL.X) - _
                          pcLEN_FREE_HEIGHT_7PTS - 0.2)

        Dim yMid As Single
        Dim pPt As PointF


        If mSealSel.POrient = "External" Then
            '--------------------------------

            '....Vertical lines:
            '    ....LHS.       
            begPt.X = sealEnvpTopL.X : begPt.Y = sealEnvpTopL.Y - 0.4 '- 0.35
            endPt.X = sealEnvpTopL.X : endPt.Y = sealEnvpTopL.Y - 0.2 '- 0.1

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

            '   ....RHS.
            begPt.X = sealEnvpBotR.X
            endPt.X = sealEnvpBotR.X

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Horizontal lines:
            yMid = 0.5 * (begPt.Y + endPt.Y)

            '   ....LHS.
            begPt.X = sealEnvpTopL.X : begPt.Y = yMid
            endPt.X = sealEnvpTopL.X + lenLine : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.HorzArrowLeft)


            '   ....RHS.
            begPt.X = sealEnvpBotR.X - lenLine : begPt.Y = yMid
            endPt.X = sealEnvpBotR.X : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(endPt, eArrowType.HorzArrowRight)


            '.....Write the text.
            With pPt
                .X = sealEnvpTopL.X + lenLine + 0.05
                .Y = yMid - 0.08 '- 0.1        
            End With

            mGr.DrawString("FREE HEIGHT", pFont, Brushes.Black, pPt)


        ElseIf mSealSel.POrient = "Internal" Then
            '------------------------------------

            '....Vertical lines:
            '    ....LHS.
            begPt.X = sealEnvpTopL.X : begPt.Y = sealEnvpBotR.Y + 0.4 '+ 0.35
            endPt.X = sealEnvpTopL.X : endPt.Y = sealEnvpBotR.Y + 0.2 '+ 0.1

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

            '   ....RHS
            begPt.X = sealEnvpBotR.X
            endPt.X = sealEnvpBotR.X
            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Horizontal lines:
            yMid = 0.5 * (begPt.Y + endPt.Y)

            '   ....LHS.
            begPt.X = sealEnvpTopL.X : begPt.Y = yMid
            endPt.X = sealEnvpTopL.X + lenLine : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.HorzArrowLeft)

            '   ....RHS.
            begPt.X = sealEnvpBotR.X - lenLine : begPt.Y = yMid
            endPt.X = sealEnvpBotR.X : endPt.Y = yMid

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(endPt, eArrowType.HorzArrowRight)


            '.....Write the text
            With pPt
                .X = sealEnvpTopL.X + lenLine + 0.05
                .Y = yMid '- 0.08 '- 0.1
            End With

            mGr.DrawString("FREE HEIGHT", pFont, Brushes.Black, pPt)
        End If


        '   'WIDTH' label
        '   -------------
        '
        '....Horizontal lines:
        Const pcHorzShift As Single = 0.4

        '   ....Top.
        begPt.X = sealEnvpBotR.X + pcHorzShift : begPt.Y = sealEnvpTopL.Y
        endPt.X = begPt.X + 0.25 : endPt.Y = sealEnvpTopL.Y

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

        '   ....Bot.
        begPt.X = sealEnvpBotR.X + pcHorzShift : begPt.Y = sealEnvpBotR.Y
        endPt.X = begPt.X + 0.25 : endPt.Y = sealEnvpBotR.Y

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


        '....Vertical lines:
        Const pcHT_WIDTH_7PTS As Single = 0.06
        Const pcLEN_WIDTH_7PTS As Single = 8 / 25.4        ' 8 mm
        lenLine = 0.5 * ((sealEnvpBotR.Y - sealEnvpTopL.Y) - _
                          pcHT_WIDTH_7PTS - 0.2)

        Dim xMid As Single
        xMid = 0.5 * (begPt.X + endPt.X)

        '   ....Top.
        begPt.X = xMid : begPt.Y = sealEnvpTopL.Y
        endPt.X = xMid : endPt.Y = begPt.Y + lenLine

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
        DrawArrow(begPt, eArrowType.VertArrowUp)

        '   ....Bot.
        begPt.X = xMid : begPt.Y = sealEnvpBotR.Y
        endPt.X = xMid : endPt.Y = begPt.Y - lenLine

        mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
        DrawArrow(begPt, eArrowType.VertArrowDown)


        '   ....Write the text
        With pPt
            .X = sealEnvpBotR.X + pcHorzShift + 0.5 * (0.25 - pcLEN_WIDTH_7PTS)
            .Y = sealEnvpTopL.Y + lenLine + 0.1
        End With

        mGr.DrawString("WIDTH", pFont, Brushes.Black, pPt)


        '   'DIAMETER' label:
        '   -----------------
        Const pcLEN_DIAMETER_7PTS As Single = 0.5
        Const pcHT_DIAMETER_7PTS As Single = 0.06


        If mSealSel.POrient = "External" Then
            '--------------------------------

            '....Horizontal line.                
            If mDisplayType = "Printer" Then
                endPt.X = sealEnvpTopL.X + 0.1 : endPt.Y = sealEnvpBotR.Y
                begPt.X = endPt.X - 0.5 : begPt.Y = endPt.Y

            ElseIf mDisplayType = "PictureBox" Then
                endPt.X = sealEnvpTopL.X + 0.1 : endPt.Y = sealEnvpBotR.Y
                begPt.X = endPt.X - 0.4 : begPt.Y = endPt.Y
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Vertical lines: 
            '
            '   ....Top.
            xMid = 0.5 * (begPt.X + endPt.X)

            begPt.X = xMid : begPt.Y = sealEnvpBotR.Y
            endPt.X = begPt.X

            If mDisplayType = "Printer" Then
                endPt.Y = begPt.Y + 0.35
            ElseIf mDisplayType = "PictureBox" Then
                endPt.Y = begPt.Y + 0.15
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.VertArrowUp)


            '....Write the text:
            With pPt
                .X = endPt.X - 0.5 * pcLEN_DIAMETER_7PTS
                .Y = endPt.Y + 0.05
            End With

            mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)

            '   ....Bot.
            begPt.X = xMid
            endPt.X = xMid

            If mDisplayType = "Printer" Then
                begPt.Y = endPt.Y + 0.2 + pcHT_DIAMETER_7PTS
                endPt.Y = begPt.Y + 0.35

            ElseIf mDisplayType = "PictureBox" Then
                begPt.Y = endPt.Y + 0.1 + pcHT_DIAMETER_7PTS
                endPt.Y = begPt.Y + 0.15
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


        ElseIf mSealSel.POrient = "Internal" Then
            '------------------------------------
            '
            '....Horizontal line.           
            If mDisplayType = "Printer" Then
                endPt.X = sealEnvpTopL.X - 0.1 : endPt.Y = sealEnvpTopL.Y
                begPt.X = endPt.X - 0.6 : begPt.Y = endPt.Y

            ElseIf mDisplayType = "PictureBox" Then
                endPt.X = sealEnvpTopL.X - 0.1 : endPt.Y = sealEnvpTopL.Y
                begPt.X = endPt.X - 0.4 : begPt.Y = endPt.Y
            End If

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)


            '....Vertical lines: 
            '
            '   ....Top.
            xMid = 0.5 * (begPt.X + endPt.X)
            Dim vLineLen As Single
            vLineLen = 0.5 * (sealEnvpBotR.Y - sealEnvpTopL.Y)

            begPt.X = xMid
            begPt.Y = sealEnvpTopL.Y

            endPt.X = begPt.X
            endPt.Y = begPt.Y + vLineLen

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)
            DrawArrow(begPt, eArrowType.VertArrowUp)


            '.....Write the text.
            With pPt
                .X = endPt.X - 0.5 * pcLEN_DIAMETER_7PTS
                .Y = endPt.Y + 0.05
            End With

            mGr.DrawString("DIAMETER", pFont, Brushes.Black, pPt)

            '   ....Bot.
            begPt.X = xMid : begPt.Y = endPt.Y + 0.2 + pcHT_DIAMETER_7PTS
            endPt.X = begPt.X : endPt.Y = begPt.Y + vLineLen

            mGr.DrawLine(pPen, begPt.X, begPt.Y, endPt.X, endPt.Y)

        End If


        '   'SECTION A-A' label:
        '   --------------------
        '
        Const pcLEN_SECTIONAA_11PTS As Single = 1.0#            '....inch

        If mDisplayType = "Printer" Then
            pFontSize = 11
        ElseIf mDisplayType = "PictureBox" Then
            pFontSize = 8
        End If

        pFont = New Font("Arial", pFontSize, FontStyle.Bold Or FontStyle.Underline)

        '.........Write the text
        Dim delXfromLHS As Single
        delXfromLHS = 0.5 * ((sealEnvpBotR.X - sealEnvpTopL.X) - _
                              pcLEN_SECTIONAA_11PTS)

        If mDisplayType = "Printer" Then

            With pPt
                .X = sealEnvpTopL.X + delXfromLHS

                If mSealSel.POrient = "External" Then
                    .Y = sealEnvpBotR.Y + 0.5
                ElseIf mSealSel.POrient = "Internal" Then
                    .Y = sealEnvpTopL.Y - 0.5
                End If

            End With


        ElseIf mDisplayType = "PictureBox" Then

            With pPt
                .X = sealEnvpTopL.X + delXfromLHS + 0.1

                If mSealSel.POrient = "External" Then
                    .Y = sealEnvpBotR.Y + 0.2
                ElseIf mSealSel.POrient = "Internal" Then
                    .Y = sealEnvpTopL.Y - 0.35
                End If

            End With

        End If

        mGr.DrawString("SECTION  A-A", pFont, Brushes.Black, pPt)

    End Sub

    '--------------------------------------------------------------------------------
    '                         UTILITY ROUTINES  - BEGIN                             '
    '--------------------------------------------------------------------------------

#Region "UTILITY ROUTINES"


    Private Sub DrawAngle(ByVal botPoint_in As PointF)
        '=============================================
        'This routine creates the surface finish mark.

        'Input  parameters :  botPoint_in - defines the placement of the mark.
        'Output parameters :  None


        '....Pen Width.
        Dim pPenWid As Single    '....in Pixels
        'If mDisplayType = "PictureBox" Then
        pPenWid = 2
        'End If

        Dim pPen As New Pen(Color.Black, pPenWid / mDpX)

        Dim pPt As PointF

        With pPt
            .X = botPoint_in.X - 0.03
            .Y = botPoint_in.Y - 0.04
        End With
        mGr.DrawLine(pPen, botPoint_in.X, botPoint_in.Y, pPt.X, pPt.Y)

        With pPt
            .X = botPoint_in.X + 0.08
            .Y = botPoint_in.Y - 0.15
        End With
        mGr.DrawLine(pPen, botPoint_in.X, botPoint_in.Y, pPt.X, pPt.Y)

    End Sub


    Private Sub DrawArrow(ByVal arrowPoint_in As PointF, _
                          ByVal arrowType_in As eArrowType)
        '===================================================
        'This routine draws an arrow.

        Const pcDelAcross As Single = 0.025
        Const pcDelAlong As Single = 0.05
        Const pcCOS45 As Single = 0.707
        Const pcSIN45 As Single = 0.707

        Dim pPt1 As PointF, pPt2 As PointF

        Select Case arrowType_in

            Case eArrowType.HorzArrowLeft
                '------------------------

                With pPt1
                    .X = arrowPoint_in.X + pcDelAlong
                    .Y = arrowPoint_in.Y - pcDelAcross
                End With

                With pPt2
                    .X = arrowPoint_in.X + pcDelAlong
                    .Y = arrowPoint_in.Y + pcDelAcross
                End With


            Case eArrowType.HorzArrowRight
                '-------------------------

                With pPt1
                    .X = arrowPoint_in.X - pcDelAlong
                    .Y = arrowPoint_in.Y - pcDelAcross
                End With

                With pPt2
                    .X = arrowPoint_in.X - pcDelAlong
                    .Y = arrowPoint_in.Y + pcDelAcross
                End With


            Case eArrowType.VertArrowUp
                '----------------------

                With pPt1
                    .X = arrowPoint_in.X - pcDelAcross
                    .Y = arrowPoint_in.Y + pcDelAlong
                End With

                With pPt2
                    .X = arrowPoint_in.X + pcDelAcross
                    .Y = arrowPoint_in.Y + pcDelAlong
                End With


            Case eArrowType.VertArrowDown
                '------------------------

                With pPt1
                    .X = arrowPoint_in.X - pcDelAcross
                    .Y = arrowPoint_in.Y - pcDelAlong
                End With

                With pPt2
                    .X = arrowPoint_in.X + pcDelAcross
                    .Y = arrowPoint_in.Y - pcDelAlong
                End With


            Case eArrowType.DiagArrowDown
                '------------------------

                Dim delX As Single
                Dim delY As Single

                delX = pcDelAlong * pcCOS45 - pcDelAcross * pcCOS45
                delY = -pcDelAlong * pcSIN45 - pcDelAcross * pcSIN45

                With pPt1
                    .X = arrowPoint_in.X + delX
                    .Y = arrowPoint_in.Y + delY
                End With


                delX = pcDelAlong * pcCOS45 + pcDelAcross * pcCOS45
                delY = -pcDelAlong * pcSIN45 + pcDelAcross * pcSIN45

                With pPt2
                    .X = arrowPoint_in.X + delX
                    .Y = arrowPoint_in.Y + delY
                End With

        End Select


        '....Draw two lines of the Arrow:
        '
        Dim pPenWid As Int16
        If mDisplayType = "Printer" Then
            pPenWid = 8
        ElseIf mDisplayType = "PictureBox" Then
            pPenWid = 1
        End If

        Dim pPen As New Pen(Color.Black, (pPenWid / mDpX))

        With arrowPoint_in
            mGr.DrawLine(pPen, .X, .Y, pPt1.X, pPt1.Y)
            mGr.DrawLine(pPen, .X, .Y, pPt2.X, pPt2.Y)
        End With

    End Sub

#End Region


#End Region


End Class
