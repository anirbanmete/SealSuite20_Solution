'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  modMain_Part                            '
'                        VERSION NO  :  101                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  24JUL17                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb

Module modMain_IPE

#Region "GLOBAL CONSTANTS:"

    '....Program Name & Version No.
    Public Const gcstrIPEProgramName As String = "SealIPE"
    Public Const gcstrIPEVersionNo As String = "10.1"

#End Region

#Region "INPUT DATA SET:"

    Public gIPE_User As New IPE_clsUser()
    Public gIPE_Unit As New IPE_clsUnit()
    Public gIPE_File As New IPE_clsFile()

    Public gIPE_Project As New IPE_clsProject()
    Public gIPE_ANSYS As New IPE_clsANSYS(gIPE_File.DirWorkANSYS)
    Public gIPE_DataValidate As New IPE_clsDataValidate()

#End Region


#Region "CURRENT ANALYSIS SET:"
    '*************************
    '....The following object would hold the particular Seal Design, 
    '........directly retrieved from the database.  
    Public gIPE_SealOrg As IPE_clsSeal
    Public gIPE_SealNew As IPE_clsSeal           '....New Seal object. 
    Public gIPE_SealCandidates As IPE_clsSealCandidates

    Public gIPE_Dwg As New IPE_clsDwg()
    Public gIPE_Report As New IPE_clsReport()


    '    Candidate Design Selection Variables:
    '    =====================================
    '
    '   A. gUpdate_Candidate_CrossSecs:
    '      ---------------------------------
    '   ....This variable indicates whether the "fldCandidate" in "tbl*SealCandidate" 
    '   ........in the working database should be updated when the "frmDesign*Seal" is 
    '   ........reopened. The associated logic will improve the performance.
    '
    '   This variable is set to TRUE in the following circumstances:
    '       i) Here, during initialization in "modMain".
    '      ii) After reading an input file or going for a new case in "frmMain". 
    '     iii) If the cavity envelope changes in "frmCavity".
    '
    '   After performing the task of selecting the candidate designs in frmDesign*Seal,
    '   ....this variable is set to FALSE.
    '
    Public gUpdate_Candidate_CrossSecs As Boolean = True    '....Initialize.


    '   B. gDisplay_Candidate_CrossSecs:
    '      -----------------------------
    '   ....This variable indicates if the "Automatic Candidate Selection" 
    '   ........should be attempted to be checked in "frmDesign*Seal" when it opens.
    '   ........14NOV06. Resolves, DR V41, error 9
    Public gDisplay_Candidate_CrossSecs As Boolean = True             '....Initialize.  

    '....Store Operating System of the current machine.                  
    Public gOS As String
    Public gOSBit As Integer
    'Public gDBConnectionState As Boolean = True
    'Public gIsTestMainActive As Boolean = False
    'Public gIsTestLeakActive As Boolean = False
    'Public gIsTestLoadActive As Boolean = False

#End Region             '...."CURRENT CASE VARIABLES:"


#Region "FORM VARIABLES:"

    Public gIPE_frmMain As New IPE_frmMain()

    Public gIPE_frmUser As New IPE_frmUser()
    Public gIPE_frmUserInfo As New IPE_frmUserInfo()
    ''Public gfrmProject As New frmProject()
    Public gIPE_frmAnalysisSet As New IPE_frmAnalysisSet()
    Public gIPE_frmOutPut As New IPE_frmOutput()
    Public gIPE_frmResults As New IPE_frmResult()
    Public gIPE_frmNomenclature_DesignCenter As IPE_frmNomenclature_DesignCenter
    Public gIPE_frmNomenclature_AdjGeom As IPE_frmNomenclature_AdjGeom

#End Region

End Module
