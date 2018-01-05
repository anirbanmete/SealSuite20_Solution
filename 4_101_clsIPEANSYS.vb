
'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealIPE"                              '
'                      CLASS MODULE  :  IPE_clsANSYS                               '
'                        VERSION NO  :  10.1                                   '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  07DEC16                                '
'                                                                              '
'===============================================================================

Imports System.Math
Imports System.Data.OleDb
Imports System.IO
Imports System.IO.FileSystemWatcher
Imports System.Timers
Imports System.Threading.Timer
Imports System.DateTime
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary


<Serializable()> _
Public Class IPE_clsANSYS
    Implements System.ICloneable

#Region "NAMED CONTSTANTS:"

    '....Conversion Factor: Degree to Radian.
    Private mcFAC_RAD As Single = Math.PI / 180.0#

    '
    '   ANSYS File Directories & Names:    
    '   -------------------------------
    '
    Private Const mcDirRoot As String = "C:\Program Files\ANSYS Inc\"

    '....The following suprious file appears in the Working Directory occasionally
    '....and creates a lot of unexpected and difficult-to-find problems. 
    '........Should be deleted if it exists. 
    Private Const mcSpuriousFile As String = "input"    '...Doesn't have any extension.

#End Region


#Region "MEMBER VARIABLE:"

    '....Analysis Type: "Static" (always now).
    Private mAnalysisType As String                                     '(FUNDAMENTAL)

    '....ANSYS  Version No. 
    Private mVersion As String                                          '(FUNDAMENTAL)

    '....Max. # of Elements allowed.
    Private mNelMax As Integer                                          '(FUNDAMENTAL)

    '....Geometry Model:    "Half" or "Full" 
    Private mModel As String                                            '(FUNDAMENTAL)  

    '....Solve ANSYS?       "Y" or "N"  
    Private mSolve As String                                            '(FUNDAMENTAL)

    '....Run Type:           "Batch" or "Interactive" 
    Private mRunType As String                                          '(FUNDAMENTAL)

    '....Solution Converged? 0 = No Convergence, 1 = Convergence
    Private mSolnConv As Integer                                        '(DERIVED)

    '....Exit Type? Normal or Abnormal (aborted by the user).
    Private mExitNormal As Boolean                                      '(DERIVED)


    'ANSYS Input & Output Files:
    '---------------------------
    Private mDirWork As String          '....Working Directory where the following 
    '                                   '........files are placed.

    Private mInpFileName As String        '....Input File .
    Private mOutFileName As String        '....Output File. 
    Private mLoadStep_FileName As String  '....Load Step File.
    Private mGraphFileName As String      '....Output File - Graphics: ANSYS plots.
    Private mDoneFileName As String
    Private mSpuriousFileName As String

    'ANSYS Model File:              
    '-----------------
    '....Key Point file for Template '1Gen' & '1GenS'
    Private mESealKP_FileName As String
    Private mUSealKP_FileName As String

#End Region


#Region "PROPERTY ROUTINES:"

    'READ & WRITE PROPERTIES:
    '------------------------
    '
    '....AnalysisType 

    Public Property AnalysisType() As String
        '====================================
        Get
            Return mAnalysisType
        End Get

        Set(ByVal strValue As String)
            mAnalysisType = strValue
        End Set
    End Property

    '....Version.
    Public Property Version() As String
        '===============================
        Get
            Return mVersion
        End Get

        Set(ByVal strValue As String)
            mVersion = strValue
            SetNelMax()
        End Set
    End Property


    '....Geometric Model - "Half" or "Full"
    Public Property Model() As String
        '===============================
        Get
            Return mModel
        End Get

        Set(ByVal strValue As String)
            mModel = strValue
        End Set
    End Property


    '....Solve ANSYS?
    Public Property Solve() As String
        '============================
        Get
            Return mSolve
        End Get

        Set(ByVal strValue As String)
            mSolve = strValue
        End Set
    End Property


    '....Run Type.
    Public Property RunType() As String
        '===========================
        Get
            Return mRunType
        End Get

        Set(ByVal strValue As String)
            mRunType = strValue
        End Set
    End Property


    '....NelMax.
    Public Property NelMax() As Integer
        '==============================
        Get
            Return mNelMax
        End Get

        Set(ByVal sngData As Integer)
            '------------------------
            mNelMax = sngData
        End Set

    End Property


    'READ-ONLY PROPERTIES:
    '---------------------
    '
    Public Property SolnConv() As Integer
        '=========================================
        Get
            Return mSolnConv
        End Get

        Set(ByVal sngData As Integer)
            '-------------------------------
            mSolnConv = sngData
        End Set
    End Property


    Public ReadOnly Property ExitNormal() As Boolean
        '===========================================
        Get
            Return mExitNormal
        End Get
    End Property


    Public ReadOnly Property GraphFileName() As String
        '=============================================
        Get
            Return mGraphFileName
        End Get
    End Property


    Public ReadOnly Property InpFileName() As String
        '=================================================
        Get
            Return mInpFileName
        End Get
    End Property


    Public ReadOnly Property OutFileName() As String
        '=================================================
        Get
            Return mOutFileName
        End Get
    End Property

    'AES 25FEB16
    Public ReadOnly Property LoadStep_FileName() As String
        '=================================================
        Get
            Return mLoadStep_FileName
        End Get
    End Property


    Public ReadOnly Property ESealKP_FileName() As String
        '=================================================  
        '....Name changed from KP_FileName to ESealKP_FileName 
        Get
            Return mESealKP_FileName
        End Get
    End Property


    Public ReadOnly Property USealKP_FileName() As String
        '=================================================  
        Get
            Return mUSealKP_FileName
        End Get
    End Property


#End Region


#Region "CONSTRUCTOR:"

    Public Sub New(ByVal dirWork_In As String)
        '=====================================

        Try
            mDirWork = dirWork_In

            'DEFAULT VALUES:
            '---------------
            mAnalysisType = "Static"

            '....ANSYS Edition & Version No. 

            '........Edition & Version are set thru' properties so that mNelMax is 
            '............set accordingly.
            Version = "17.0"

            mModel = "Full" '"Half"             '....Geometry Model:
            mSolve = "Y"
            mRunType = "Batch"
            mExitNormal = False

            '....Various ANSYS file names:
            mInpFileName = mDirWork & "file.inp"
            mOutFileName = mDirWork & "file.out"
            mLoadStep_FileName = mDirWork & "LoadStepFile.txt"
            mGraphFileName = mDirWork & "file.grph"
            mDoneFileName = mDirWork & "file.done"
            mSpuriousFileName = mDirWork & mcSpuriousFile

            '....ANSYS Model File. 
            '........Key Point file for Template '1Gen' & '1GenS'
            mESealKP_FileName = mDirWork & "ESeal_KP_T1Gen_V1.txt"

            '........Key Point file for USeal.
            mUSealKP_FileName = mDirWork & "USeal_KP_V1.txt"

        Catch
            MsgBox(Err.Description)
        End Try

    End Sub

#End Region


#Region "UTILITY ROUTINES:"

    Public Sub DeletePrevFiles_ANSYS()
        '=============================
        Try
            '....Delete the existing FEA files.
            '
            If File.Exists(mInpFileName) Then File.Delete(mInpFileName)
            If File.Exists(mGraphFileName) Then File.Delete(mGraphFileName)
            If File.Exists(mSpuriousFileName) Then File.Delete(mSpuriousFileName)
            'If File.Exists(mOutFileName) Then File.Delete(mOutFileName)
            If File.Exists(mDoneFileName) Then File.Delete(mDoneFileName)
            If File.Exists(mESealKP_FileName) Then File.Delete(mESealKP_FileName)

            Dim pDir As New IO.DirectoryInfo(mDirWork)
            Dim pGraphicsFiles As IO.FileInfo() = pDir.GetFiles("*.png")
            Dim pFile As IO.FileInfo

            'Dim pIndex As Integer = 1
            For Each pFile In pGraphicsFiles
                'Dim pNewFileName As String = pIndex & ".png"
                File.Delete(pFile.FullName)
                'pIndex = pIndex + 1
            Next

            'AES 29JUN16
            '....Delete *.out file
            Dim pOutFiles As IO.FileInfo() = pDir.GetFiles("*.out")
            For Each pFile In pOutFiles
                File.Delete(pFile.FullName)
            Next

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SetNelMax()
        '==================
        If mNelMax = 0 Then
            mNelMax = 5000              '....There is no practical limit. 
            '                           '........This is just a starting default value.
        ElseIf mNelMax > 0 Then
            '....Don't do anything. Leave the prior value undisturbed.
        End If

    End Sub


    Public Sub Run(ByVal ProgramName_In As String)
        '=========================================
        'This routine invokes one of the following ANSYS programs. 
        '....ProgramName_In:
        '       "ANSYS"   ===> FEA program is invoked.
        '       "DISPLAY" ===> Graphics program is invoked.

        Dim pExeFileTitle As String = ""

        If ProgramName_In = "ANSYS" Then
            '===========================

            If mVersion = "13.0" Or mVersion = "13" Then
                pExeFileTitle = "ANSYS130_Win7.bat"

            ElseIf mVersion = "15.0" Or mVersion = "15" Then
                pExeFileTitle = "ANSYS150_Win7.bat"

            ElseIf mVersion = "16.0" Or mVersion = "16" Then
                pExeFileTitle = "ANSYS160_Win7.bat"

            ElseIf mVersion = "17.0" Or mVersion = "17" Then
                pExeFileTitle = "ANSYS170_Win7.bat"

            End If


        ElseIf ProgramName_In = "DISPLAY" Then
            '=================================
            Dim pDisplayW As String = ""
            pDisplayW = "ansys\bin\winx64\DisplayW.exe"

            If mVersion = "13.0" Or mVersion = "13" Then
                pExeFileTitle = mcDirRoot & "v130\" & pDisplayW

            ElseIf mVersion = "15.0" Or mVersion = "15" Then
                pExeFileTitle = mcDirRoot & "v150\" & pDisplayW

            ElseIf mVersion = "16.0" Or mVersion = "16" Then
                pExeFileTitle = mcDirRoot & "v160\" & pDisplayW

            ElseIf mVersion = "17.0" Or mVersion = "17" Then
                pExeFileTitle = mcDirRoot & "v170\" & pDisplayW

            End If

        End If


        Dim pProc As New Process()
        With pProc
            .StartInfo.FileName = pExeFileTitle
            .StartInfo.WorkingDirectory = mDirWork
            .Start()
            .WaitForExit()

        End With


        If ProgramName_In = "ANSYS" Then
            '---------------------------

            Dim pblnANSYSProcess As Boolean = False     '....ANSYS process intiated yet?
            Dim p As Process

            Do While pblnANSYSProcess = False

                For Each p In Process.GetProcesses
                    If UCase(p.ProcessName) = "ANSYS" Then

                        pblnANSYSProcess = True
                        p.WaitForExit()

                        '....Diagonostic statement.
                        Exit Do
                    End If
                Next

            Loop

            '....Check if the "Done File" exists (NORMAL) or not (ABNORMAL).
            If File.Exists(mDoneFileName) = False Then
                mExitNormal = False

            ElseIf File.Exists(mDoneFileName) = True Then
                mExitNormal = True
            End If

        End If

    End Sub


#End Region


#Region "CLONE INTERFACE"

    Public Function Clone() As Object Implements ICloneable.Clone
        '========================================================
        '....Inherited from the ICloneable interface, supports deep cloning

        Dim pBinSerializer As New BinaryFormatter(Nothing, _
                              New StreamingContext(StreamingContextStates.Clone))
        Dim pMemBuffer As New MemoryStream()

        '....Serialize the object into the memory stream
        pBinSerializer.Serialize(pMemBuffer, Me)

        '....Move the stream pointer to the beginning of the memory stream
        pMemBuffer.Seek(0, SeekOrigin.Begin)

        '....Get the serialized object from the memory stream
        Dim pobjClone As Object
        pobjClone = pBinSerializer.Deserialize(pMemBuffer)
        pMemBuffer.Close()      '....Release the memory stream.

        Return pobjClone    '.... Return the deeply cloned object.

    End Function

#End Region

End Class
