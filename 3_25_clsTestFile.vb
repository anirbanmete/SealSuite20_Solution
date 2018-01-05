'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsFile                                '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  04JUL17                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Math
Imports System.Globalization
Imports clsLibrary11
Imports System.Xml
Imports System.IO.FileSystemWatcher
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data.EntityClient
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Configuration
'Imports SealIPELib = SealIPELib101
<Serializable()> _
Public Class Test_clsFile

    'Directories & Folders:
    '---------------------------------------
    Private Const mcDriveRoot As String = "C:"

    '....Root Directory.
    '........Installation: 
    Private Const mcDirRoot As String = mcDriveRoot & "\SealSuite\"

    '....Executable Files & Ini File. 
    Private Const mcDirRootInstall As String = mcDirRoot & "SealTest\V20\"

    '....Drawing Templates. 
    Private Const mcDirTemplates As String = mcDirRoot & "SealTest\Templates\"


    'Titles & Names:            
    '-------------------------
    Private Const mcConfigFile_Title As String = "SealSuite10.config"
    Private Const mConfigFile_Name As String = mcDirRoot & mcConfigFile_Title
    Private Const mcIniFile_Title As String = "SealSuite10.ini"
    Private Const mIniFile_Name As String = mcDirRoot & mcIniFile_Title

    Private Const mcProgrameName As String = "SealTest"

    '....Logo.
    Private Const mcLogo_Title As String = "parker_color.bmp"
    Private Const mcObjFile_Count As Integer = 2

    Private mFileName_SealTest As String = ""       'AES 23NOV16

    Public Sub New()
        '===========
        'ReadConfigFile()

    End Sub

    Public ReadOnly Property DirEquip() As String
        '=========================================       
        Get
            Return mcDirRoot & "SealTest\Equipment File\"
        End Get
    End Property


    Public ReadOnly Property DirProdedure() As String
        '=============================================         
        Get
            Return mcDirRoot & "SealTest\Procedure File\"
        End Get
    End Property

    Public ReadOnly Property DirOut() As String
        '=======================================       
        Get
            Return mcDirRoot & "SealTest\Output Files\"
        End Get
    End Property

    Public Property FileName_SealTest() As String
        '==========================================      'AES 23NOV16
        Get
            Return mFileName_SealTest
        End Get

        Set(ByVal strData As String)
            mFileName_SealTest = strData
        End Set

    End Property

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
    '        Dim pEFB As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealPartDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB As New SqlConnectionStringBuilder(pEFB.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB.ProviderConnectionString = pSQB.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealPartDBEntities").ConnectionString = pEFB.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")

    '        '....Second Connection String
    '        '........Because it's an EF connection string it's not a normal connection string
    '        '........so we pull it into the EntityConnectionStringBuilder instead
    '        Dim pEFB1 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealIPEDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB1 As New SqlConnectionStringBuilder(pEFB1.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB1.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB1.ProviderConnectionString = pSQB1.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealIPEDBEntities").ConnectionString = pEFB1.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")


    '        '....Third Connection String
    '        '........Because it's an EF connection string it's not a normal connection string
    '        '........so we pull it into the EntityConnectionStringBuilder instead
    '        Dim pEFB2 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealTestDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB2 As New SqlConnectionStringBuilder(pEFB2.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB2.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB2.ProviderConnectionString = pSQB2.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealTestDBEntities").ConnectionString = pEFB2.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")

    '        '....Fourth Connection String
    '        '........Because it's an EF connection string it's not a normal connection string
    '        '........so we pull it into the EntityConnectionStringBuilder instead
    '        Dim pEFB3 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealIPEMCSDBEntities").ConnectionString)

    '        '....Then we extract the actual underlying provider connection string
    '        Dim pSQB3 As New SqlConnectionStringBuilder(pEFB3.ProviderConnectionString)

    '        '....Now we can set the datasource
    '        pSQB3.DataSource = DataSource_In

    '        '....Pop it back into the EntityConnectionStringBuilder 
    '        pEFB3.ProviderConnectionString = pSQB3.ConnectionString

    '        '....And update
    '        pConfig.ConnectionStrings.ConnectionStrings("SealIPEMCSDBEntities").ConnectionString = pEFB3.ConnectionString

    '        pConfig.Save(ConfigurationSaveMode.Modified, True)
    '        ConfigurationManager.RefreshSection("connectionStrings")


    '    End Sub

    '#End Region


    Public Sub ReadIniFile(ByRef ANSYS_Out As IPE_clsANSYS, _
                           ByRef UnitSystem_Out As IPE_clsUnit)
        '============================================================== 
        '....XML File.
        Dim pSR As FileStream
        Dim pXML As XmlDocument
        pXML = New XmlDocument()

        Try
            pSR = New FileStream(mIniFile_Name, FileMode.Open)
            pXML.Load(pSR)

            '....Root Node of XML.
            Dim pRoot As XmlNode
            pRoot = pXML.DocumentElement

            For Each pRChild As XmlNode In pRoot.ChildNodes

                Select Case pRChild.Name

                    Case "UserName"
                        'UserInfo_Out.Name = pRChild.InnerText

                    Case "PhoneNo"
                        'UserInfo_Out.PhoneNo = pRChild.InnerText

                    Case "UnitSystem"
                        '....Unit System:
                        UnitSystem_Out.System = pRChild.InnerText

                    Case "ANSYSVersion"
                        '....ANSYS Version:
                        ANSYS_Out.Version = pRChild.InnerText

                    Case "CultureFormat"
                        '....Culture Format:
                        'Project_Out.CultureName = pRChild.InnerText

                    Case "IPE_LastSession_TimeStamp"
                        '....TimeStamp:
                        'UserInfo_Out.IPE_LastSession_TimeStamp = pRChild.InnerText

                End Select

            Next

            pSR.Close()

        Catch pEXP As Exception
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
                                                                 MessageBoxIcon.Error)
        End Try

    End Sub


#Region "SESSION SAVE/RESTORE RELATED ROUTINES:"

#Region "SAVE SESSION:"

    Public Sub Save_SessionData(ByVal TestProject_In As Test_clsProject)
        '===============================================================
        Try
            Dim pFilePath As String = mFileName_SealTest.Remove(mFileName_SealTest.Length - 9)

            '....1. clsTestProject
            Dim pTestProject As Boolean = TestProject_In.SaveData_Serialize(pFilePath)

            '....2. clsFiles
            Dim pFile As Boolean = Me.SaveData_Serialize(pFilePath)

            '....Merge two Binary files created for two different objects.
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

        Dim pFileName_Out As String = FilePath_In & ".SealTest"
        Dim OutputFile As New FileStream(pFileName_Out, FileMode.Create, FileAccess.Write)

        For index As Integer = 1 To mcObjFile_Count
            Dim pFileName As String = FilePath_In & index & ".SealTest"

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
        '=============================================
        Dim pFileName As String = Nothing

        For index As Integer = 1 To mcObjFile_Count
            pFileName = FilePath_In & index & ".SealTest"
            File.Delete(pFileName)
        Next
    End Sub

#End Region

#Region "RESTORE SESSION:"

    Public Sub Restore_SessionData(ByRef TestProject_Out As Test_clsProject,
                                   ByVal FilePath_In As String)
        '====================================================================
        Try
            Split_SessionFile()
            TestProject_Out = DirectCast(TestProject_Out.RestoreData_Deserialize(FilePath_In), Test_clsProject)

            RestoreData_Deserialize(FilePath_In)
            Delete_ObjFiles(FilePath_In)

        Catch pEXP As Exception
        End Try
    End Sub


    Private Sub Split_SessionFile()
        '===========================
        Dim line As String = Nothing
        Dim pLength As Int32 = 0
        Dim pIndex As Integer = 1

        Dim OpenFile As FileStream = Nothing
        OpenFile = New FileStream(mFileName_SealTest, FileMode.Open, FileAccess.Read, FileShare.Read)

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
        If mFileName_SealTest <> "" Then
            pFilePath = mFileName_SealTest.Remove(mFileName_SealTest.Length - 9)
        End If

        Try
            Dim pFileName_Out As String = pFilePath & Index_In & ".SealTest"

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
            Dim pFileName As String = FilePath_In & "2.SealTest"

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
        Dim pFileName As String = FilePath_In & "2.SealTest"
        Dim openFile As New FileStream(pFileName, FileMode.Open, FileAccess.Read)
        Dim pObj As Object
        pObj = serializer.Deserialize(openFile)

        openFile.Close()

        Return pObj

    End Function

#End Region

End Class
