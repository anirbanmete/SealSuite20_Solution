'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  modMain_Part                           '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  12DEC17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb

Module modMain_Part

#Region "INPUT DATA SET:"
    Public gPartFile As New clsPartFile()
    Public gPartProject As New clsPartProject()

#End Region

#Region "FORM VARIABLES:"

    '....Create all the forms here:
    Public gfrmNomenclature_NonStd_AdjGeom As Part_frmNomenclature_NonStd_AdjGeom

#End Region

#Region "GLOBAL VARIABLES:"

    Public gIsLegacyPNActive As Boolean = False
    Public gIsHWActive As Boolean = False

#End Region

#Region "UTILITY ROUTINES:"

    Public Sub LoadImageNomanclature(ByVal imgControl As PictureBox, _
                                     ByVal imgPath As String)
        '==============================================================   

        Try
            imgControl.BackgroundImage = Image.FromFile(imgPath)

        Catch pEXP As IO.FileNotFoundException
            MessageBox.Show(pEXP.Message, "File Not Found", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
        End Try

    End Sub


#End Region

End Module
