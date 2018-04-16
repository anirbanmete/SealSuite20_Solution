'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  clsFile                                '
'                        VERSION NO  :  10.1                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  25MAY17                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Math
Imports System.Globalization
Imports clsLibrary11
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data.EntityClient
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Configuration

Public Class IPE_clsFile

    'Directories & Folders:
    '----------------------
    Private Const mcDriveRoot As String = "C:"

    '....Root Directory.
    '........Installation: 
    Private Const mcDirRoot As String = mcDriveRoot & "\SealSuite\"

    '....Drawing Templates. 
    'Private Const mcDirTemplates As String = mcDirRoot & "Templates\"
    Private Const mcDirTemplates As String = mcDirRoot & "SealIPE\Templates\"

    '....Drawing Text File
    'Private Const mcDwgTxt_Name As String = mcDirRoot & "DwgFile.txt"
    Private Const mcDwgTxt_Name As String = mcDirRoot & "SealIPE\DwgFile.txt"


    '....Users Manual. 
    Private Const mcDirUsersManual As String = mcDirRoot & "Docu\V70\Users Manual\"


    'Titles & Names:            
    '---------------
    '....Initialization.                                        
    Private Const mcIniFile_Title As String = "SealSuite10.ini"
    Private Const mIniFile_Name As String = mcDirRoot & mcIniFile_Title

    Private Const mcConfigFile_Title As String = "SealSuite10.config"
    Private Const mConfigFile_Name As String = mcDirRoot & mcConfigFile_Title

    Private Const mcProgrameName As String = "SealIPE101"

    '....Logo.
    Private Const mcLogo_Title As String = "parker_color.bmp"

    '....Nomenclature Image Files.      
    '........E-Seal:
    Private Const mcESeal_1Gen_Ext_Title As String = "E-Seal_1Gen_Ext.bmp"
    Private Const mcESeal_1Gen_Int_Title As String = "E-Seal_1Gen_Int.bmp"
    Private Const mcESeal_1GenS_Ext_Title As String = "E-Seal_1GenS_Ext.bmp"
    Private Const mcESeal_1GenS_Int_Title As String = "E-Seal_1GenS_Int.bmp"

    Private Const mcESeal_Ext_Title As String = "E-Seal_Ext.bmp"
    Private Const mcESeal_Int_Title As String = "E-Seal_Int.bmp"

    '........U-Seal.
    Private Const mcUSeal_Ext_Title As String = "U-Seal_Ext.bmp"
    Private Const mcUSeal_Int_Title As String = "U-Seal_Int.bmp"


    '....Drawing Template File Name: 
    Private Const mcDwt_Name As String = "APCO_TEMPLATE_01.dwt"

    '....User's Manual File Name: 
    Private Const mcUsersManual_Name As String = "SealIPE70UsersManual.pdf"

    Private Const mcObjFile_Count As Integer = 3

    'MEMBER VARIABLES:
    '-----------------
    '....Input Data File.
    Private mIn_Name As String
    Private mIn_Title As String
    Private mFileName_SealIPE As String = ""       'AES 23NOV16


    Public Sub New()
        '===========
        'ReadConfigFile()

    End Sub

#Region "PROPERTY ROUTINES"

    Public Property In_Name() As String
        '==============================
        Get
            Return mIn_Name
        End Get

        Set(ByVal strData As String)
            mIn_Name = strData
        End Set

    End Property

    Public Property FileName_SealIPE() As String
        '==========================================      'AES 23NOV16
        Get
            Return mFileName_SealIPE
        End Get

        Set(ByVal strData As String)
            mFileName_SealIPE = strData
        End Set

    End Property

    'User Directory Sub-Folders & Files:
    '-----------------------------------

    Public ReadOnly Property DirWorkANSYS() As String
        '============================================       
        Get
            Return mcDirRoot & "WorkDir\V100\"
        End Get
    End Property


    Public ReadOnly Property DirIn() As String
        '=====================================       
        Get
            Return mcDirRoot & "Input Files\V100\"
        End Get
    End Property


    Public ReadOnly Property DirOut() As String
        '=====================================       
        Get
            Return mcDirRoot & "SealIPE\Output Files\"
        End Get
    End Property

    'Installation Directory, Sub-Folders & Files:
    '-------------------------------------------------------------

    Public ReadOnly Property DirDB() As String
        '=====================================
        Get
            Return mcDirRoot & "Databases\"
        End Get
    End Property


    Public ReadOnly Property Logo() As String
        '====================================
        Get
            Return mcDirRoot & "Images\" & mcLogo_Title
        End Get
    End Property


    Public ReadOnly Property ESeal_1Gen_Ext_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcESeal_1Gen_Ext_Title
        End Get
    End Property


    Public ReadOnly Property ESeal_1Gen_Int_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcESeal_1Gen_Int_Title
        End Get
    End Property


    Public ReadOnly Property ESeal_1GenS_Ext_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcESeal_1GenS_Ext_Title
        End Get
    End Property


    Public ReadOnly Property ESeal_1GenS_Int_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcESeal_1GenS_Int_Title
        End Get
    End Property


    Public ReadOnly Property ESeal_Int_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcESeal_Int_Title
        End Get
    End Property


    Public ReadOnly Property ESeal_Ext_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcESeal_Ext_Title
        End Get
    End Property


    Public ReadOnly Property USeal_Int_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcUSeal_Int_Title
        End Get
    End Property


    Public ReadOnly Property USeal_Ext_ImgName() As String
        '===================================================
        Get
            Return mcDirRoot & "Images\" & mcUSeal_Ext_Title
        End Get
    End Property

    Public ReadOnly Property UsersManual() As String
        '===========================================    
        Get
            Return mcDirUsersManual & mcUsersManual_Name
        End Get
    End Property

    Public ReadOnly Property In_Title() As String
        '=========================================
        Get
            If mIn_Name <> "" Then
                Dim iPos As Integer = InStrRev(mIn_Name, "\")
                mIn_Title = Mid(mIn_Name, iPos + 1)

            Else
                mIn_Title = ""
            End If

            Return mIn_Title
        End Get

    End Property

#End Region


    '#Region "READ,WRITE INI FILE"

    '    Public Sub ReadIniFile(ByRef UserInfo_Out As IPE_clsUser, _
    '                           ByRef Project_Out As IPE_clsProject, _
    '                           ByRef ANSYS_Out As IPE_clsANSYS, _
    '                           ByRef UnitSystem_Out As IPE_clsUnit)
    '        '====================================================== 
    '        '....XML File.
    '        Dim pSR As FileStream
    '        Dim pXML As XmlDocument
    '        pXML = New XmlDocument()

    '        Try
    '            pSR = New FileStream(mIniFile_Name, FileMode.Open)
    '            pXML.Load(pSR)

    '            '....Root Node of XML.
    '            Dim pRoot As XmlNode
    '            pRoot = pXML.DocumentElement

    '            For Each pRChild As XmlNode In pRoot.ChildNodes

    '                Select Case pRChild.Name

    '                    Case "UserName"
    '                        UserInfo_Out.Name = pRChild.InnerText

    '                    Case "PhoneNo"
    '                        UserInfo_Out.PhoneNo = pRChild.InnerText

    '                    Case "UnitSystem"
    '                        '....Unit System:
    '                        UnitSystem_Out.System = pRChild.InnerText

    '                    Case "ANSYSVersion"
    '                        '....ANSYS Version:
    '                        ANSYS_Out.Version = pRChild.InnerText

    '                    Case "CultureFormat"
    '                        '....Culture Format:
    '                        Project_Out.CultureName = pRChild.InnerText

    '                    Case "IPE_LastSession_TimeStamp"
    '                        '....TimeStamp:
    '                        UserInfo_Out.IPE_LastSession_TimeStamp = pRChild.InnerText

    '                End Select

    '            Next

    '            pSR.Close()

    '        Catch pEXP As Exception
    '            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
    '                                                                 MessageBoxIcon.Error)
    '        End Try

    '    End Sub


    '    Public Sub SaveIniFile(ByVal UserInfo_In As IPE_clsUser, _
    '                           ByVal Project_In As IPE_clsProject, _
    '                           ByVal ANSYS_In As IPE_clsANSYS, _
    '                           ByVal UnitSystem_In As IPE_clsUnit)
    '        '======================================================= 
    '        '....XML File.

    '        Dim pXML As XmlDocument
    '        pXML = New XmlDocument()

    '        Try
    '            Dim pXMLWriter As New XmlTextWriter(mIniFile_Name, System.Text.Encoding.UTF8)

    '            With pXMLWriter

    '                .WriteStartDocument(True)
    '                .Formatting = Formatting.Indented
    '                .Indentation = 2
    '                .WriteStartElement(mcProgrameName)
    '                .WriteStartElement("UserName")
    '                .WriteString(Trim(UserInfo_In.Name))
    '                .WriteEndElement()

    '                .WriteStartElement("PhoneNo")
    '                .WriteString(Trim(UserInfo_In.PhoneNo))
    '                .WriteEndElement()

    '                .WriteStartElement("UnitSystem")
    '                .WriteComment("Unit System: English/Metric")
    '                .WriteString(Trim(UnitSystem_In.System))
    '                .WriteEndElement()

    '                'AES 01APR16
    '                .WriteStartElement("ANSYSVersion")
    '                .WriteString(Trim(ANSYS_In.Version))
    '                .WriteEndElement()

    '                .WriteStartElement("CultureFormat")
    '                .WriteComment("Culture Format: USA/UK/Germany/France")
    '                .WriteString(Trim(Project_In.CultureName))
    '                .WriteEndElement()

    '                .WriteStartElement("IPE_LastSession_TimeStamp")
    '                .WriteString(DateAndTime.Now())
    '                .WriteEndElement()

    '                .WriteEndElement()
    '                .WriteEndDocument()
    '                .Close()

    '            End With


    '        Catch pEXP As Exception
    '            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
    '                                                                 MessageBoxIcon.Error)
    '        End Try

    '    End Sub


    '#End Region


    '#Region "READ CONFIG FILE"

    '    Public Sub ReadConfigFile()
    '        '======================
    '        '....XML File.
    '        Dim pSR As FileStream
    '        Dim pXML As XmlDocument
    '        pXML = New XmlDocument()
    '        Dim pDS As String = ""

    '        Try

    '            pSR = New FileStream(mConfigFile_Name, FileMode.Open)
    '            pXML.Load(pSR)

    '            '....Root Node of XML.
    '            Dim pRoot As XmlNode
    '            pRoot = pXML.DocumentElement

    '            For Each pRChild As XmlNode In pRoot.ChildNodes

    '                Select Case pRChild.Name

    '                    Case "DataSource"
    '                        '....AutoCAD Version:
    '                        pDS = pRChild.InnerText

    '                End Select

    '            Next

    '            pSR.Close()
    '            UpdateAppConfig(pDS)

    '        Catch pEXP As Exception
    '            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
    '                                                                 MessageBoxIcon.Error)
    '        End Try

    '    End Sub


    '    Private Sub UpdateAppConfig(ByVal DataSource_In As String)
    '        '====================================================== 

    '        Dim pConfig As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

    '        '....First Connection String
    '        '........Because it's an EF connection string it's not a normal connection string
    '        '........so we pull it into the EntityConnectionStringBuilder instead
    '        Dim pEFB As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealIPEDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB As New SqlConnectionStringBuilder(pEFB.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB.ProviderConnectionString = pSQB.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealIPEDBEntities").ConnectionString = pEFB.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")

    '        '....Second Connection String
    '        '........Because it's an EF connection string it's not a normal connection string
    '        '........so we pull it into the EntityConnectionStringBuilder instead
    '        Dim pEFB1 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealPartDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB1 As New SqlConnectionStringBuilder(pEFB1.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB1.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB1.ProviderConnectionString = pSQB1.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealPartDBEntities").ConnectionString = pEFB1.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")


    '        '....Third Connection String
    '        '........Because it's an EF connection string it's not a normal connection string
    '        '........so we pull it into the EntityConnectionStringBuilder instead
    '        Dim pEFB2 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealIPEMCSDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB2 As New SqlConnectionStringBuilder(pEFB2.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB2.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB2.ProviderConnectionString = pSQB2.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealIPEMCSDBEntities").ConnectionString = pEFB2.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")

    '    End Sub

    '#End Region


#Region "UTILITY ROUTINES:"


    Public Sub ReadFile_ANSYS_FatigueData(ByVal FileName_In As String, ByRef SX_Out As List(Of Double), _
                                          ByRef SY_Out As List(Of Double), ByRef SZ_Out As List(Of Double))
        '==================================================================================================
        '....This subroutine reads the FEA output results from the mOutFileName. 

        Dim pSR As StreamReader

        Try
            Dim pDirWork As String = DirWorkANSYS()
            pSR = File.OpenText(pDirWork + "\" + FileName_In)

            SX_Out.Clear()
            SY_Out.Clear()
            SZ_Out.Clear()

            pSR.ReadLine()

            Dim pstrAny As String
            pstrAny = pSR.ReadLine()

            Do While pstrAny <> ""

                Dim pstrOut(3) As String
                pstrOut = pstrAny.Split(",")         '....Delimiter = ","

                SX_Out.Add(Val(Trim(pstrOut(0))))
                SY_Out.Add(Val(Trim(pstrOut(1))))
                SZ_Out.Add(Val(Trim(pstrOut(2))))

                pstrAny = pSR.ReadLine()
            Loop


        Catch pEXP As IOException
            '....Error Handler
            MessageBox.Show(pEXP.Message, "File Not Found", MessageBoxButtons.OK, _
                                                            MessageBoxIcon.Error)
            Exit Sub
        End Try



    End Sub


    Public Sub ResetData(ByRef Unit_Out As IPE_clsUnit, _
                         ByRef Project_Out As IPE_clsProject, _
                         ByRef Cavity_Out As IPE_clsCavity, _
                         ByRef Seal_Out As IPE_clsSeal, _
                         ByRef UserInfo_Out As IPE_clsUser, _
                         ByRef Mat_Out As IPE_clsMaterial, _
                         ByRef ANSYS_Out As IPE_clsANSYS, _
                         ByRef SealCandidates_Out As IPE_clsSealCandidates)
        '==================================================================  

        'Initialize Data 
        '----------------
        '....File Name
        mIn_Name = ""

        '....Project
        Project_Out = New IPE_clsProject
        'With Project_Out
        '    .Customer = ""
        '    ''.Name = ""

        '    'AES 28OCT15
        '    .CustomerPN = ""
        '    .ParkerPN = ""
        'End With

        '....Unit System
        Unit_Out = New IPE_clsUnit("English")

        '....Applied Condition 
        'AppCond_Out = New clsAppCond()

        '....initialize Cavity Object. 
        'Cavity_Out = New clsCavity()

        '....initialize Seal Object. 
        '........E-Seal Object

        'Seal_Out = New clsESeal("E-Seal", gDB)

        'Mat_Out.Name = ""

        '....FEA Parameters
        ANSYS_Out = New IPE_clsANSYS(gIPE_File.DirWorkANSYS)

        '....SECONDARY ASSIGNMENTS
        Set_SecondaryAssignments(Unit_Out, Cavity_Out, Seal_Out, _
                                 Mat_Out, SealCandidates_Out)

    End Sub


    Public Sub Set_SecondaryAssignments(ByVal Unit_In As IPE_clsUnit, _
                                         ByRef Cavity_Out As IPE_clsCavity, _
                                         ByRef Seal_Out As IPE_clsSeal, _
                                         ByRef Mat_Out As IPE_clsMaterial, _
                                         ByRef SealCandidates_Out As IPE_clsSealCandidates)
        '==================================================================================

        ''Form: frmSealDesign.       
        ''--------------------
        ''....Primary Assignment  : mSeal object instantiation.
        ''....Secondary Assignment: mSealCandidates object
        ''
        'If Seal_Out Is Nothing = False Then

        '    '....Set 'SealCandidates_In' object reference, if any, here to null.
        '    '........It will be intantiated latter here.
        '    If SealCandidates_Out Is Nothing = False Then SealCandidates_Out = Nothing

        '    If Seal_Out.Type = "E-Seal" Then
        '        SealCandidates_Out = New clsSealCandidates("E-Seal", DB_In, Unit_In.System, _
        '                                                   AppCond_Out.POrient)
        '    ElseIf Seal_Out.Type = "C-Seal" Then
        '        SealCandidates_Out = New clsSealCandidates("C-Seal", DB_In, Unit_In.System, _
        '                                                   AppCond_Out.POrient)
        '    ElseIf Seal_Out.Type = "U-Seal" Then
        '        SealCandidates_Out = New clsSealCandidates("U-Seal", DB_In, Unit_In.System, _
        '                                                   AppCond_Out.POrient)

        '    End If

        '    'A new file has been read. The candidate seal designs in the appropriate
        '    '....working database table should be updated.
        '    '----------------------------------------------------------------------- 
        '    'blnUpdate_CandidateDesigns_Out = True      

        'End If


        ''Form: frmProject.
        ''-----------------
        ''....Primary Assignment: Unit.System
        ''....Secondary Assignments:
        ''
        'Mat_Out.UnitSystem = Unit_In.System
        'AppCond_Out.UnitSystem = Unit_In.System
        'Cavity_Out.UnitSystem = Unit_In.System

        'If Seal_Out Is Nothing = False Then _
        'Seal_Out.UnitSystem = Unit_In.System


        ''Form: frmAppCond.
        ''-----------------
        ''....Primary Assignment: Unit.UserP
        ''....Secondary Assignment.
        'AppCond_Out.UnitUserP = Unit_In.UserP

        ''....Primary Assignment: AppCod.TOper
        ''....Secondary Assignment.
        'Mat_Out.TOper = AppCond_Out.TOper

        ''....Primary Assignment: AppCond.POrient
        ''....Secondary Assignment.
        'If Seal_Out Is Nothing = False Then _
        '    Seal_Out.POrient = AppCond_Out.POrient


        ''Form: frmCavity
        ''---------------
        ''....Primary Assignment: Cavity.Dia(i)
        ''
        ''....Secondary Assignments:
        'Dim i As Int16
        'If Seal_Out Is Nothing = False Then
        '    For i = 1 To 2
        '        Seal_Out.CavityDia(i) = Cavity_Out.Dia(i)
        '        SealCandidates_Out.CavityDia(i) = Cavity_Out.Dia(i)
        '    Next
        'End If

        ''....Primary Assignment: mCavity object member variables.
        ''....Secondary Assignment.
        'AppCond_Out.Cavity = Cavity_Out


        ''Form: frm*SealDesign.
        ''----------------------
        ''....Primary Assignment: Seal.CavityCornerRad
        ''....Secondary Assignment.
        'If Seal_Out Is Nothing = False Then                 
        '    'If Cavity_Out.CornerRad <> 0 Then              
        '    If Abs(Cavity_Out.CornerRad) > gcEPS Then
        '        Cavity_Out.CornerRad = IIf(Abs(Cavity_Out.CornerRad - Seal_Out.CavityCornerRad) < gcEPS, _
        '                                                              Seal_Out.CavityCornerRad, Cavity_Out.CornerRad)
        '    Else
        '        Cavity_Out.CornerRad = Seal_Out.CavityCornerRad
        '    End If

        'End If

        ''....Primary Assignment: Seal object member variables.
        ''....Secondary Assignment.
        'If Seal_Out Is Nothing = False Then _
        'AppCond_Out.Seal = Seal_Out

    End Sub

    Public Sub Move_GraphicsFiles(ByVal CustID_In As Integer, ByVal PlatformID_In As Integer,
                                  ByVal ProjectID_In As Integer, ByVal AnalysisID_In As Integer)
        '=========================================================================================
        Try

            'Dim pTargetFolderTitle As String = gProject.Customer_ID & "-" & gProject.Platform_ID & "-" & gProject.Project_ID & "-" & AnalysisID_In
            Dim pTargetFolderTitle As String = CustID_In & "-" & PlatformID_In & "-" & ProjectID_In & "-" & AnalysisID_In
            Dim pFolderName As String = DirOut() & pTargetFolderTitle
            If Not Directory.Exists(pFolderName) Then
                Directory.CreateDirectory(pFolderName)
            Else
                DeleteFilesFromFolder(pFolderName)
            End If

            Dim pDir As New IO.DirectoryInfo(DirWorkANSYS())
            Dim pGraphicsFiles As IO.FileInfo() = pDir.GetFiles("*.png")
            Dim pFile As IO.FileInfo

            Dim pIndex As Integer = 1
            For Each pFile In pGraphicsFiles
                Dim pNewFileName As String = pIndex & ".png"
                File.Move(pFile.FullName, pFolderName & "\" & pNewFileName)
                pIndex = pIndex + 1
            Next

        Catch ex As Exception

        End Try

    End Sub

    Private Sub DeleteFilesFromFolder(FolderName_In As String)
        '======================================================
        If Directory.Exists(FolderName_In) Then
            For Each pFile As String In Directory.GetFiles(FolderName_In)
                File.Delete(pFile)
            Next
        End If

    End Sub


#End Region


#Region "SESSION SAVE/RESTORE RELATED ROUTINES:"

#Region "SAVE SESSION:"

    Public Sub Save_SessionData(ByVal Project_In As IPE_clsProject)
        '==============================================================================================
        Try
            Dim pFilePath As String = mFileName_SealIPE.Remove(mFileName_SealIPE.Length - 8)

            '....1. clsProject
            Dim pProject As Boolean = Project_In.SaveData_Serialize(pFilePath)

            '....2. clsTestProject
            'Dim pTestProject As Boolean = TestProject_In.SaveData_Serialize(pFilePath)

            '....3. clsFiles
            Me.SaveData_Serialize(pFilePath)

            '....Merge three Binary files created for three different objects.
            Merge_ObjFiles(pFilePath)

            '....Delete Binary files.
            Delete_ObjFiles(pFilePath)

        Catch pEXP As Exception
        End Try

    End Sub


    Private Sub Merge_ObjFiles(FilePath_In As String)
        '=============================================
        Dim pHeader As Byte()
        Dim buffer As Byte()
        Dim count As Integer = 0
        Dim pFileHeader As String = Nothing
        Dim OpenFile As FileStream = Nothing

        Dim pFileName_Out As String = FilePath_In & ".SealIPE"
        Dim OutputFile As New FileStream(pFileName_Out, FileMode.Create, FileAccess.Write)

        For index As Integer = 1 To mcObjFile_Count
            Dim pFileName As String = FilePath_In & index & ".SealIPE"

            OpenFile = New FileStream(pFileName, FileMode.Open, FileAccess.Read, FileShare.Read)

            '....Initialize the buffer by the total byte length of the file.
            buffer = New Byte(OpenFile.Length - 1) {}

            '....Read the file and store it into the buffer.
            OpenFile.Read(buffer, 0, buffer.Length)
            count = OpenFile.Read(buffer, 0, buffer.Length)

            '....Create a header for each file.
            pFileHeader = "BeginFile" & index & "," & buffer.Length.ToString()

            '....Transfer the header string into bytes.
            pHeader = Encoding.[Default].GetBytes(pFileHeader)

            '....Write the header info. into file.
            OutputFile.Write(pHeader, 0, pHeader.Length)

            '....Write a Linefeed into file for seperating header info and file info.
            OutputFile.WriteByte(10)
            ' linefeed
            '....Write buffer data into file.
            OutputFile.Write(buffer, 0, buffer.Length)
            OpenFile.Close()
        Next

        OutputFile.Close()
    End Sub


    Private Sub Delete_ObjFiles(FilePath_In As String)
        '==========================================
        Dim pFileName As String = Nothing

        For index As Integer = 1 To mcObjFile_Count
            pFileName = FilePath_In & index & ".SealIPE"
            File.Delete(pFileName)
        Next
    End Sub

#End Region

#Region "RESTORE SESSION:"

    Public Sub Restore_SessionData(ByRef Project_Out As IPE_clsProject,
                                   ByVal FilePath_In As String)
        '=================================================================================================
        Try
            Split_SessionFile()
            Project_Out = DirectCast(Project_Out.RestoreData_Deserialize(FilePath_In), IPE_clsProject)
            'TestProject_Out = DirectCast(TestProject_Out.RestoreData_Deserialize(FilePath_In), clsTest_Project)

            RestoreData_Deserialize(FilePath_In)
            Delete_ObjFiles(FilePath_In)

        Catch pEXP As Exception
        End Try
    End Sub


    Private Sub Split_SessionFile()
        '==============================
        Dim line As String = Nothing
        Dim pLength As Int32 = 0
        Dim pIndex As Integer = 1

        Dim OpenFile As FileStream = Nothing
        OpenFile = New FileStream(mFileName_SealIPE, FileMode.Open, FileAccess.Read, FileShare.Read)

        While OpenFile.Position <> OpenFile.Length
            line = Nothing
            While String.IsNullOrEmpty(line) AndAlso OpenFile.Position <> OpenFile.Length
                '....Read the header info.
                line = ReadLine(OpenFile)
            End While

            If Not String.IsNullOrEmpty(line) AndAlso OpenFile.Position <> OpenFile.Length Then
                '....Store the total byte length of the file stored into the header.
                pLength = GetLength(line)
            End If
            If Not String.IsNullOrEmpty(line) Then
                '....Write bin files from the marged file.
                Write_ObjFiles(OpenFile, pLength, pIndex)
                pIndex += 1
            End If
        End While
        OpenFile.Close()

    End Sub


    Private Function ReadLine(fs As FileStream) As String
        '=================================================
        Dim line As String = String.Empty

        Const bufferSize As Integer = 4096
        Dim buffer As Byte() = New Byte(bufferSize - 1) {}
        Dim b As Byte = 0
        Dim lf As Byte = 10
        Dim i As Integer = 0

        While b <> lf
            b = CByte(fs.ReadByte())
            buffer(i) = b
            i += 1
        End While

        line = System.Text.Encoding.Default.GetString(buffer, 0, i - 1)

        Return line
    End Function

    Private Function GetLength(fileInfo As String) As Int32
        '===================================================
        Dim pLength As Int32 = 0
        If Not String.IsNullOrEmpty(fileInfo) Then
            '....get the file information
            Dim info As String() = fileInfo.Split(","c)
            If info IsNot Nothing AndAlso info.Length = 2 Then
                pLength = Convert.ToInt32(info(1))
            End If
        End If
        Return pLength

    End Function

    Private Sub Write_ObjFiles(fs As FileStream, fileLength As Integer, Index_In As Integer)
        '====================================================================================
        Dim fsFile As FileStream = Nothing
        Dim pFilePath As String = ""
        If mFileName_SealIPE <> "" Then
            pFilePath = mFileName_SealIPE.Remove(mFileName_SealIPE.Length - 8)
        End If

        Try
            Dim pFileName_Out As String = pFilePath & Index_In & ".SealIPE"

            Dim buffer As Byte() = New Byte(fileLength - 1) {}
            Dim count As Integer = fs.Read(buffer, 0, fileLength)
            fsFile = New FileStream(pFileName_Out, FileMode.Create, FileAccess.Write, FileShare.None)
            fsFile.Write(buffer, 0, buffer.Length)
            fsFile.Write(buffer, 0, count)
        Catch ex1 As Exception
            ' handle or display the error
            Throw ex1
        Finally
            If fsFile IsNot Nothing Then
                fsFile.Flush()
                fsFile.Close()
                fsFile = Nothing
            End If
        End Try
    End Sub

#End Region

#End Region


#Region "SERIALIZE-DESERIALIZE:"

    Public Function SaveData_Serialize(FilePath_In As String) As Boolean
        '===============================================================
        Try
            Dim serializer As IFormatter = New BinaryFormatter()
            Dim pFileName As String = FilePath_In & "3.SealIPE"

            Dim saveFile As New FileStream(pFileName, FileMode.Create, FileAccess.Write)

            serializer.Serialize(saveFile, Me)

            saveFile.Close()

            Return True
        Catch
            Return False
        End Try
    End Function


    Public Function RestoreData_Deserialize(FilePath_In As String) As Object
        '===================================================================
        Dim serializer As IFormatter = New BinaryFormatter()
        Dim pFileName As String = FilePath_In & "3.SealIPE"
        Dim openFile As New FileStream(pFileName, FileMode.Open, FileAccess.Read)
        Dim pObj As Object
        pObj = serializer.Deserialize(openFile)

        openFile.Close()

        Return pObj

    End Function

#End Region


End Class
