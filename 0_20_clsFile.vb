'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealProcess"                          '
'                      CLASS MODULE  :  clsSuiteFile                           '
'                        VERSION NO  :  2.0                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  13DEC17                                '
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
Imports System.Data.EntityClient
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> _
Public Class clsFile

#Region "NAMED CONSTANT:"
    'Directories & Folders:
    '----------------------
    Private Const mcDriveRoot As String = "C:"

    '....Root Directory.
    '........Installation: 
    Private Const mcDirRoot As String = mcDriveRoot & "\SealSuite\"

    Private Const mcConfigFile_Title As String = "SealSuite10.config"
    Private Const mConfigFile_Name As String = mcDirRoot & mcConfigFile_Title

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

End Class
