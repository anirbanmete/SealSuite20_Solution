﻿'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  modMain_Process                        '
'                        VERSION NO  :  1.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  14DEC17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb

Module modMain_Process

#Region "INPUT DATA SET:"

    Public gProcessProject As New clsProcessProj(gPartProject)
    Public gProcessFile As New clsProcessFile
    Public gIsProcessMainActive As Boolean = False
    Public gIsIssueCommentActive As Boolean = False
    Public gIsResolutionActive As Boolean = False

#End Region


End Module
