'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  modMain_Test                           '
'                        VERSION NO  :  2.5                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  10NOV17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb

Module modMain_Test

#Region "GLOBAL CONSTANTS:"

    Public Const gcstrSealTestVersionNo As String = "" '"2.4"

#End Region

#Region "CLASS VARIABLES:"

    Public gTest_Test As New Test_clsTest()
    Public gTest_User As New Test_clsUser()
    Public gTest_Project As New Test_clsProject(gPartProject)
    Public gTest_Equipment As New Test_clsEquipment()

    Public gTest_File As New Test_clsFile()

    Public gTest_Report As New Test_clsReport()

#End Region

#Region "FORM VARIABLES:"

    Public gTest_frmMain As New Test_frmMain()

#End Region

#Region "GLOBAL VARIABLES:"

    Public gIsTestMainActive As Boolean = False
    Public gIsTestLeakActive As Boolean = False
    Public gIsTestLoadActive As Boolean = False

#End Region

End Module
