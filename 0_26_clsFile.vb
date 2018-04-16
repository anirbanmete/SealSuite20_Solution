'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealSuite"                          '
'                      CLASS MODULE  :  clsFile                                '
'                        VERSION NO  :  2.6                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  16APR18                                '
'                                                                              '
'===============================================================================

Imports System.IO
Imports System.Math
Imports System.Globalization
Imports clsLibrary11
Imports System.Xml
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data.Entity
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
'Imports System.Data.Entity.Core.EntityClient
Imports System.Data.EntityClient

<Serializable()> _
Public Class clsFile

#Region "NAMED CONSTANT:"
    'Directories & Folders:
    '----------------------
    Private Const mcDriveRoot As String = "C:"

    '....Root Directory.
    '........Installation: 
    Private Const mcDirRoot As String = mcDriveRoot & "\SealSuite\"

    Private Const mcConfigFile_Title As String = "SealSuite.config"
    Private Const mConfigFile_Name As String = mcDirRoot & mcConfigFile_Title

    'Titles & Names:            
    '---------------
    '....Initialization.                                        
    Private Const mcIniFile_Title As String = "SealSuite.ini"
    Private Const mIniFile_Name As String = mcDirRoot & mcIniFile_Title

    Private Const mcProgrameName As String = "SealSuite"

#End Region

#Region "MEMBER VARIABLE DECLARATIONS:"


#End Region


#Region "CONSTRUCTOR:"

    Public Sub New()
        '===========
        ReadConfigFile()

    End Sub

#End Region


#Region "PROPERTY ROUTINES:"

    Public ReadOnly Property DirUserData() As String
        '============================================       
        Get
            Return mcDirRoot & "Program Data Files\"
        End Get

    End Property

    Public ReadOnly Property DirProgramDataFile_Process() As String
        '==========================================================       
        Get
            Return mcDirRoot & "SealProcess\Program Data Files\"
        End Get

    End Property

#End Region


#Region "READ CONFIG FILE"

    Public Sub ReadConfigFile()
        '======================
        '....XML File.
        Dim pSR As FileStream
        Dim pXML As XmlDocument
        pXML = New XmlDocument()
        Dim pDS As String = ""

        Try

            pSR = New FileStream(mConfigFile_Name, FileMode.Open)
            pXML.Load(pSR)

            '....Root Node of XML.
            Dim pRoot As XmlNode
            pRoot = pXML.DocumentElement

            For Each pRChild As XmlNode In pRoot.ChildNodes

                Select Case pRChild.Name

                    Case "DataSource"
                        '....AutoCAD Version:
                        pDS = pRChild.InnerText

                End Select

            Next

            pSR.Close()
            UpdateAppConfig(pDS)

        Catch pEXP As Exception
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK, _
                                                                 MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub UpdateAppConfig(ByVal DataSource_In As String)
        '====================================================== 

        Dim pConfig As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

        '....First Connection String
        '........Because it's an EF connection string it's not a normal connection string
        '........so we pull it into the EntityConnectionStringBuilder instead
        Dim pEFB As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealPartDBEntities").ConnectionString)

        '....Then we extract the actual underlying provider connection string
        Dim pSQB As New SqlConnectionStringBuilder(pEFB.ProviderConnectionString)

        '....Now we can set the datasource
        pSQB.DataSource = DataSource_In

        '....Pop it back into the EntityConnectionStringBuilder 
        pEFB.ProviderConnectionString = pSQB.ConnectionString

        '....And update
        pConfig.ConnectionStrings.ConnectionStrings("SealPartDBEntities").ConnectionString = pEFB.ConnectionString

        pConfig.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")

        '....Second Connection String
        '........Because it's an EF connection string it's not a normal connection string
        '........so we pull it into the EntityConnectionStringBuilder instead
        Dim pEFB1 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealIPEDBEntities").ConnectionString)

        '....Then we extract the actual underlying provider connection string
        Dim pSQB1 As New SqlConnectionStringBuilder(pEFB1.ProviderConnectionString)

        '....Now we can set the datasource
        pSQB1.DataSource = DataSource_In

        '....Pop it back into the EntityConnectionStringBuilder 
        pEFB1.ProviderConnectionString = pSQB1.ConnectionString

        '....And update
        pConfig.ConnectionStrings.ConnectionStrings("SealIPEDBEntities").ConnectionString = pEFB1.ConnectionString

        pConfig.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")


        '....Third Connection String
        '........Because it's an EF connection string it's not a normal connection string
        '........so we pull it into the EntityConnectionStringBuilder instead
        Dim pEFB2 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealTestDBEntities").ConnectionString)

        '....Then we extract the actual underlying provider connection string
        Dim pSQB2 As New SqlConnectionStringBuilder(pEFB2.ProviderConnectionString)

        '....Now we can set the datasource
        pSQB2.DataSource = DataSource_In

        '....Pop it back into the EntityConnectionStringBuilder 
        pEFB2.ProviderConnectionString = pSQB2.ConnectionString

        '....And update
        pConfig.ConnectionStrings.ConnectionStrings("SealTestDBEntities").ConnectionString = pEFB2.ConnectionString

        pConfig.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")

        '....Fourth Connection String
        '........Because it's an EF connection string it's not a normal connection string
        '........so we pull it into the EntityConnectionStringBuilder instead
        Dim pEFB3 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealIPEMCSDBEntities").ConnectionString)

        '....Then we extract the actual underlying provider connection string
        Dim pSQB3 As New SqlConnectionStringBuilder(pEFB3.ProviderConnectionString)

        '....Now we can set the datasource
        pSQB3.DataSource = DataSource_In

        '....Pop it back into the EntityConnectionStringBuilder 
        pEFB3.ProviderConnectionString = pSQB3.ConnectionString

        '....And update
        pConfig.ConnectionStrings.ConnectionStrings("SealIPEMCSDBEntities").ConnectionString = pEFB3.ConnectionString

        pConfig.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")

        '....Fifth Connection String
        '........Because it's an EF connection string it's not a normal connection string
        '........so we pull it into the EntityConnectionStringBuilder instead
        Dim pEFB4 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealProcessDBEntities").ConnectionString)

        '....Then we extract the actual underlying provider connection string
        Dim pSQB4 As New SqlConnectionStringBuilder(pEFB4.ProviderConnectionString)

        '....Now we can set the datasource
        pSQB4.DataSource = DataSource_In

        '....Pop it back into the EntityConnectionStringBuilder 
        pEFB4.ProviderConnectionString = pSQB4.ConnectionString

        '....And update
        pConfig.ConnectionStrings.ConnectionStrings("SealProcessDBEntities").ConnectionString = pEFB4.ConnectionString

        pConfig.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")

        '....Sixth Connection String
        '........Because it's an EF connection string it's not a normal connection string
        '........so we pull it into the EntityConnectionStringBuilder instead
        Dim pEFB5 As New EntityConnectionStringBuilder(pConfig.ConnectionStrings.ConnectionStrings("SealSuiteDBEntities").ConnectionString)

        '....Then we extract the actual underlying provider connection string
        Dim pSQB5 As New SqlConnectionStringBuilder(pEFB5.ProviderConnectionString)

        '....Now we can set the datasource
        pSQB5.DataSource = DataSource_In

        '....Pop it back into the EntityConnectionStringBuilder 
        pEFB5.ProviderConnectionString = pSQB5.ConnectionString

        '....And update
        pConfig.ConnectionStrings.ConnectionStrings("SealSuiteDBEntities").ConnectionString = pEFB5.ConnectionString

        pConfig.Save(ConfigurationSaveMode.Modified, True)
        ConfigurationManager.RefreshSection("connectionStrings")


    End Sub

#End Region


#Region "READ,WRITE INI FILE"

    Public Sub ReadIniFile(ByRef User_Out As clsUser,
                           ByRef Project_Out As IPE_clsProject,
                           ByRef ANSYS_Out As IPE_clsANSYS,
                           ByRef UnitSystem_Out As IPE_clsUnit)
        '====================================================== 
        '....XML File.
        Dim pSR As FileStream
        Dim pXML As XmlDocument
        pXML = New XmlDocument()

        gUser.RetrieveUserTitle()
        Dim pUserName As String = ""
        Dim pUserName_CurrentSession As String = (gUser.FirstName + " " + gUser.LastName).Trim()

        Try
            pSR = New FileStream(mIniFile_Name, FileMode.Open)
            pXML.Load(pSR)

            '....Root Node of XML.
            Dim pRoot As XmlNode
            pRoot = pXML.DocumentElement

            For Each pRChild As XmlNode In pRoot.ChildNodes

                Select Case pRChild.Name

                    Case "UserName"
                        pUserName = pRChild.InnerText

                    Case "Role"
                        If (pUserName = pUserName_CurrentSession) Then
                            User_Out.Role = pRChild.InnerText
                        End If


                    Case "UnitSystem"
                        '....Unit System:
                        UnitSystem_Out.System = pRChild.InnerText

                    Case "ANSYSVersion"
                        '....ANSYS Version:
                        ANSYS_Out.Version = pRChild.InnerText

                    Case "CultureFormat"
                        '....Culture Format:
                        Project_Out.CultureName = pRChild.InnerText

                        'Case "IPE_LastSession_TimeStamp"
                        '    '....TimeStamp:
                        '    UserInfo_Out.IPE_LastSession_TimeStamp = pRChild.InnerText

                End Select

            Next

            pSR.Close()

        Catch pEXP As Exception
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK,
                                                                 MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub SaveIniFile(ByVal User_In As clsUser,
                           ByVal Project_In As IPE_clsProject,
                           ByVal ANSYS_In As IPE_clsANSYS,
                           ByVal UnitSystem_In As IPE_clsUnit)
        '======================================================= 
        '....XML File.

        Dim pXML As XmlDocument
        pXML = New XmlDocument()

        Try
            Dim pXMLWriter As New XmlTextWriter(mIniFile_Name, System.Text.Encoding.UTF8)

            With pXMLWriter

                .WriteStartDocument(True)
                .Formatting = Formatting.Indented
                .Indentation = 2
                .WriteStartElement(mcProgrameName)
                Dim pUserName As String = gUser.FirstName + " " + gUser.LastName
                .WriteStartElement("UserName")
                .WriteString(Trim(pUserName))
                .WriteEndElement()

                .WriteStartElement("Role")
                .WriteString(Trim(gUser.Role))
                .WriteEndElement()

                .WriteStartElement("UnitSystem")
                .WriteComment("Unit System: English/Metric")
                .WriteString(Trim(UnitSystem_In.System))
                .WriteEndElement()

                'AES 01APR16
                .WriteStartElement("ANSYSVersion")
                .WriteString(Trim(ANSYS_In.Version))
                .WriteEndElement()

                .WriteStartElement("CultureFormat")
                .WriteComment("Culture Format: USA/UK/Germany/France")
                .WriteString(Trim(Project_In.CultureName))
                .WriteEndElement()

                ''.WriteStartElement("IPE_LastSession_TimeStamp")
                ''.WriteString(DateAndTime.Now())
                ''.WriteEndElement()

                .WriteEndElement()
                .WriteEndDocument()
                .Close()

            End With


        Catch pEXP As Exception
            MessageBox.Show(pEXP.Message, "File Path Not Found", MessageBoxButtons.OK,
                                                                 MessageBoxIcon.Error)
        End Try

    End Sub


#End Region

End Class
