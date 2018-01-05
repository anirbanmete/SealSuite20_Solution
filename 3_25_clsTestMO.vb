'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealTest"                             '
'                      CLASS MODULE  :  clsTest_MO                             '
'                        VERSION NO  :  2.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  29NOV16                                '
'                                                                              '
'===============================================================================
Imports System.Linq

<Serializable()> _
Public Class Test_clsMO

#Region "MEMBER VARIABLE DECLARATIONS:"

    Public mID As Integer
    Public mNo As Integer

    Private mTest_Report As New List(Of Test_clsReport)

    <NonSerialized()> _
    Private mSealTestEntities As New SealTestDBEntities()

#End Region


#Region "CLASS PROPERTY ROUTINE:"

    Public Property ID() As Integer
        '===========================
        Get
            Return mID
        End Get

        Set(ByVal Value As Integer)
            mID = Value
        End Set
    End Property


    Public Property No() As Integer
        '===========================
        Get
            Return mNo
        End Get

        Set(ByVal Value As Integer)
            mNo = Value
        End Set
    End Property


    Public Property Test_Report() As List(Of Test_clsReport)
        '==============================================
        Get
            Return mTest_Report
        End Get
        Set(Obj As List(Of Test_clsReport))
            mTest_Report = Obj
        End Set
    End Property

#End Region


#Region "DB RELATED ROUTINES:"

    Public Sub RetrieveFrom_Report(ByVal TestProjectID_In As Integer)
        '============================================================
        Dim pQry = (From pRec In mSealTestEntities.tblReport
                     Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = mID Select pRec Distinct).ToList()

        mTest_Report = New List(Of Test_clsReport)

        For i As Integer = 0 To pQry.Count - 1

            Dim pReport As New Test_clsReport
            pReport.ID = pQry(i).fldID
            pReport.No = pQry(i).fldNo
            pReport.SealQty = pQry(i).fldSealQty
            pReport.DateOpen = pQry(i).fldDateOpen

            pReport.Leak_Exists = pQry(i).fldIsLeak

            If (Not IsNothing(pQry(i).fldIsLeak_Leakage)) Then
                pReport.Leak_Leakage = pQry(i).fldIsLeak_Leakage
            End If

            If (Not IsNothing(pQry(i).fldIsLeak_LeakagePlated)) Then
                pReport.Leak_Plate = pQry(i).fldIsLeak_LeakagePlated
            End If

            If (Not IsNothing(pQry(i).fldIsLeak_Springback)) Then
                pReport.Leak_Springback = pQry(i).fldIsLeak_Springback
            End If

            pReport.Load_Exists = pQry(i).fldIsLoad

            If (Not IsNothing(pQry(i).fldIsLoad_Springback)) Then
                pReport.Load_Springback = pQry(i).fldIsLoad_Springback
            End If


            pReport.IsPress = pQry(i).fldIsPressure
            pReport.LeakPress = pQry(i).fldLeakPress

            pReport.Notes = pQry(i).fldNotes
            pReport.Overridden = pQry(i).fldOverridden

            pReport.Tester_Name = pQry(i).fldUserTester
            pReport.Tester_Signed = pQry(i).fldTesterSigned
            pReport.Tester_SignedDate = pQry(i).fldDateTester

            pReport.Eng_Name = pQry(i).fldUserEngg
            pReport.Eng_Signed = pQry(i).fldEnggSigned
            pReport.Eng_SignedDate = pQry(i).fldDateEngg

            pReport.Quality_Name = pQry(i).fldUserQuality
            pReport.Quality_Signed = pQry(i).fldQualitySigned
            pReport.Quality_SignedDate = pQry(i).fldDateQuality

            '....Retrieve Data from Report
            pReport.RetrieveFrom_DB(TestProjectID_In, mID)

            mTest_Report.Add(pReport)
        Next

    End Sub


    Public Sub DeleteFrom_Report(ByVal TestProjectID_In As Integer, ByVal ReportNo_In As Integer)
        '=========================================================================================
        Dim pSealTestEntities As New SealTestDBEntities

        Dim pQry = (From pRec In pSealTestEntities.tblReport
                           Where pRec.fldTestProjectID = TestProjectID_In And pRec.fldTestMOID = mID And pRec.fldNo = ReportNo_In Select pRec).ToList()

        If (pQry.Count() > 0) Then
            pSealTestEntities.DeleteObject(pQry(0))
            pSealTestEntities.SaveChanges()
        End If

    End Sub

#End Region

End Class
