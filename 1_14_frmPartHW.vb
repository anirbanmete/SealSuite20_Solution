'===============================================================================
'                                                                              '
'                          SOFTWARE  :  "SealPart"                             '
'                      FORM MODULE   :  frmHW                                  '
'                        VERSION NO  :  1.4                                    '
'                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
'                     LAST MODIFIED  :  18APR18                                '
'                                                                              '
'===============================================================================
Imports System.IO
Imports System.Globalization
Imports System
Imports System.Threading
Imports System.Math
Imports clsLibrary11

Public Class Part_frmHW

#Region "MEMBER VARIABLES:"
    Private mPartProject As New clsPartProject()
    Private mPNID As Integer
    Private mRevID As Integer
    
#End Region
    


    Public Sub New(Optional ByVal SealProcess_In As String = "PN",
                   Optional ByVal PNID_In As Integer = 0, Optional ByVal RevID_In As Integer = 0)
        '=========================================================================================

        ' This call is required by the designer.
        InitializeComponent()
        mPNID = PNID_In
        mRevID = RevID_In

        If (SealProcess_In = "Project") Then
            pnlHW.Enabled = False
        Else
            pnlHW.Enabled = True
        End If

        '....Pressure Orientation.
        With cmbPOrient.Items
            .Clear()
            .Add("External")
            .Add("Internal")
        End With

        With cmbType
            .Items.Clear()
            .Items.Add("E")
            .Items.Add("C")
            .Items.Add("SC")
            .Items.Add("U")
        End With


    End Sub


    Private Sub frmTestHW_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        '=========================================================================================

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
            If (mPartProject.PNR.HW.Adjusted) Then
                lblStandard.Text = "N"
                lblStandard.BackColor = Color.Yellow
            Else
                lblStandard.Text = "Y"
                lblStandard.BackColor = Color.White
            End If
        End If
    End Sub


    Private Sub frmTestHW_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        '=========================================================================================
        Me.Text = "Hardware Form"
        mPartProject = gPartProject.Clone()
        mPartProject.PNR.HW.InitializePNR(mPartProject.PNR)

        GetPartProjectInfo()

        mPartProject.PNR.HW.UnitSystem = gPartProject.PNR.UnitSystem.ToString()

        gIsHWActive = True
        cmbPOrient.SelectedIndex = 0

        'cmbType.SelectedIndex = 0

        chkSeg.Checked = False
        lblSegNo.Enabled = False
        txtSegNo.Text = ""
        txtSegNo.Enabled = False

        PopulatePlatingType()
        PopulatePlatingTCode()
        ''RetrieveFromDB()
        mPartProject.PNR.RetrieveFromDB(mPNID, mRevID)      'AES 18APR18

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
            grpCoating.Enabled = True
        Else
            grpCoating.Enabled = False
        End If

        cmbCoating.Enabled = False
        cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
        cmbCoating.Text = ""

        lblSFinish.Enabled = False
        cmbSFinish.Enabled = False
        lblUnitSFinish.Enabled = False
        cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
        cmbSFinish.Text = ""

        cmbPlatingCode.Enabled = False
        cmbPlatingCode.DropDownStyle = ComboBoxStyle.DropDown
        cmbPlatingCode.Text = ""

        cmbPlatingThickCode.Enabled = False
        cmbPlatingThickCode.DropDownStyle = ComboBoxStyle.DropDown
        cmbPlatingThickCode.Text = ""

        DisplayData()

        Dim pSealType As String = cmbType.Text
        If (pSealType = "") Then
            pSealType = mPartProject.PNR.SealType.ToString()
            cmbType.Text = pSealType
        End If
        If (pSealType = "E") Then
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E
        ElseIf (pSealType = "C") Then
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C
        ElseIf (pSealType = "SC") Then
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC
        ElseIf (pSealType = "U") Then
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.U
        End If

        If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                grpPlating.Enabled = True
            Else
                grpPlating.Enabled = False
            End If

            cmdNonStdCS.Visible = False
            lblStandard.Text = "Y"
            lblStandard.BackColor = Color.White

        Else
            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                grpPlating.Enabled = True
                cmdNonStdCS.Visible = True

            ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                grpPlating.Enabled = False
                cmdNonStdCS.Visible = True
            Else
                lblStandard.Text = "Y"
                grpPlating.Enabled = False
                cmdNonStdCS.Visible = False
            End If
        End If


    End Sub

    Private Sub PopulateMatComboBox(ByVal SealType_In As String, ByRef cmbBox As ComboBox)
        '=================================================================================
        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pMatList As New List(Of String)
        cmbBox.Items.Clear()

        Dim pUsedMatCodeType As New List(Of String)
        pUsedMatCodeType.Add("P")
        pUsedMatCodeType.Add("S")
        pUsedMatCodeType.Add("O")


        Select Case SealType_In
            Case "E"
                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                        Where pRec.fldE = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "C"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                        Where pRec.fldC = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "U"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial
                        Where pRec.fldU = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            pMatList.Add(pQry(j).fldName.Trim())
                        Next
                    End If
                Next

            Case "SC"

                For i As Integer = 0 To pUsedMatCodeType.Count - 1
                    Dim pMatCodeType As String = pUsedMatCodeType(i)
                    Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
                        Where pRec.fldC = pMatCodeType Select pRec).ToList()

                    If (pQry.Count > 0) Then
                        For j As Integer = 0 To pQry.Count - 1
                            Dim pMatName As String = pQry(j).fldName.Trim()
                            If (pMatName.Contains("Cobalt Chromium-Nickel Alloy")) Then
                                pMatName = pMatName.Replace("Cobalt Chromium-Nickel Alloy", "Co-Cr-Ni")
                            End If

                            pMatList.Add(pMatName)
                        Next
                    End If
                Next

        End Select

        For i As Integer = 0 To pMatList.Count - 1
            cmbBox.Items.Add(pMatList(i))
        Next

        If (cmbBox.Items.Count > 0) Then
            cmbBox.SelectedIndex = 0
        End If

    End Sub


    Private Sub DisplayData()
        '====================
        InitializeControls()
        ''RetrieveFromDB()
        mPartProject.PNR.RetrieveFromDB(mPNID, mRevID)      'AES 18APR18
        'txtHFree.Text = ""
        Dim pDHFree As Single = mPartProject.PNR.HW.DHfree
        Dim pDThetaOpening As Single = mPartProject.PNR.HW.DThetaOpening
        Dim pT As Single = mPartProject.PNR.HW.T

        'AES 11OCT17
        Dim pDThetaE1 As Single = mPartProject.PNR.HW.DThetaE1
        Dim pDThetaM1 As Single = mPartProject.PNR.HW.DThetaM1

        If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then

            Dim pCrossSec As String = ""
            Dim pHFreeTol1 As Single = 0.0
            Dim pHFreeTol2 As Single = 0.0

            If (mPartProject.PNR.HW.POrient <> "") Then
                pCrossSec = mPartProject.PNR.HW.MCrossSecNo
                pHFreeTol1 = mPartProject.PNR.HW.HFreeTol(1)
                pHFreeTol2 = mPartProject.PNR.HW.HFreeTol(2)
            End If

            Dim pPN As String = gPartProject.PNR.Legacy.Val

            Dim pPN_Legacy_Array() As String = pPN.Split("-")
            Dim pUnitSystem As String = pPN_Legacy_Array(0).Substring(0, 1)

            If (pUnitSystem = "E") Then
                '....English Unit     'Seal Diameter (Three Decimal places. Eg: 3.000 in seal is specified as 003000)
                Dim pDia As String = pPN_Legacy_Array(1)
                txtDControl.Text = Convert.ToInt16(pDia.Substring(0, 3)).ToString() & "." & pDia.Substring(3, 3)

            ElseIf (pUnitSystem = "M") Then
                '....Metric Unit     'Seal Diameter (Two Decimal places. Eg: 3.00 in seal is specified as 000300)
                Dim pDia As String = pPN_Legacy_Array(1)
                txtDControl.Text = Convert.ToInt16(pDia.Substring(0, 4)).ToString() & "." & pDia.Substring(4, 2)
            End If

            Dim pOrient As String = pPN_Legacy_Array(0).Substring(2, 1)
            If (pOrient = "E") Then
                cmbPOrient.SelectedIndex = 0
            Else
                cmbPOrient.SelectedIndex = 1
            End If
            Dim pSealType As String = pPN_Legacy_Array(0).Substring(1, 1)
            If (pSealType = "C") Then
                cmbType.SelectedIndex = 1

            ElseIf (pSealType = "S") Then
                cmbType.SelectedIndex = 2

            ElseIf (pSealType = "E") Then
                cmbType.SelectedIndex = 0

            ElseIf (pSealType = "U") Then
                cmbType.SelectedIndex = 3

            End If

            Dim pMCS As String = GetMCS(pSealType, pPN_Legacy_Array(2))
            cmbCrossSec.Text = pMCS

            cmbMatName.Text = gPartProject.PNR.HW.GetMatName(pSealType, pPN_Legacy_Array(3))

            If (pSealType = "C" Or pSealType = "S" Or pSealType = "E") Then

                Dim pPlating As String = pPN_Legacy_Array(5)
                If (pPlating <> "000") Then
                    chkPlating.Checked = True
                    cmbPlatingCode.Text = pPlating.Substring(0, 2)
                    cmbPlatingThickCode.Text = pPlating.Substring(2, 1)

                    Dim pThickCode As String = cmbPlatingThickCode.Text
                    If (pThickCode <> "X" And pThickCode <> "") Then
                        Dim pMCSEntities As New SealIPEMCSDBEntities()

                        Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = pThickCode
                                    Select pRec).ToList()

                        If (pQry.Count() > 0) Then
                            If (mPartProject.PNR.HW.UnitSystem = "English") Then
                                txtPlatingThickMin.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMinEng, "TFormat")
                                txtPlatingThickMax.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMaxEng, "TFormat")
                            Else
                                txtPlatingThickMin.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMinMet, "TFormat")
                                txtPlatingThickMax.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMaxMet, "TFormat")
                            End If
                        End If
                    Else
                        If (mPartProject.PNR.HW.UnitSystem = "English") Then
                            txtPlatingThickMin.Text = mPartProject.PNR.HW.Plating.ThickMin.ToString("##0.0000")
                            txtPlatingThickMax.Text = mPartProject.PNR.HW.Plating.ThickMax.ToString("##0.0000")
                        Else
                            txtPlatingThickMin.Text = mPartProject.PNR.HW.Plating.ThickMin.ToString("###0.000")
                            txtPlatingThickMax.Text = mPartProject.PNR.HW.Plating.ThickMax.ToString("###0.000")
                        End If
                        'txtPlatingThickMin.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMin, "TFormat")
                        'txtPlatingThickMax.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMax, "TFormat")
                    End If
                Else
                    txtPlatingThickMin.Text = ""
                    txtPlatingThickMax.Text = ""
                End If

                cmdNonStdCS.Visible = False
                lblStandard.Text = "Y"
                lblStandard.BackColor = Color.White

            End If

            If (mPartProject.PNR.HW.Hfree > gcEPS) Then
                txtHFree.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.Hfree)
            End If

            If (pCrossSec <> "") Then
                txtHFreeTolPlus.Text = gUnit.WriteInUserL(pHFreeTol1)
                txtHFreeTolMinus.Text = gUnit.WriteInUserL(pHFreeTol2)
            Else
                txtHFreeTolPlus.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.HFreeTol(1))
                txtHFreeTolMinus.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.HFreeTol(2))

            End If

        Else

            If (mPartProject.PNR.HW.POrient <> "") Then
                With mPartProject.PNR.HW
                    Dim pCrossSec As String = .MCrossSecNo
                    Dim pHFreeTol1 As Single = .HFreeTol(1)
                    Dim pHFreeTol2 As Single = .HFreeTol(2)

                    cmbPOrient.Text = .POrient
                    cmbType.Text = mPartProject.PNR.SealType.ToString()

                    .MCrossSecNo = pCrossSec
                    'cmbPOrient.Text = .POrient
                    cmbCrossSec.Text = .MCrossSecNo
                    chkSeg.Checked = .IsSegmented
                    If (.IsSegmented) Then
                        txtSegNo.Text = .CountSegment
                    Else
                        txtSegNo.Text = ""
                    End If

                    cmbMatName.Text = .MatName
                    cmbHT.Text = .HT
                    cmbTemperCode.Text = .Temper
                    If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                        If (.Coating = "None") Then
                            chkCoating.Checked = False
                            cmbCoating.Text = ""
                        Else
                            chkCoating.Checked = True
                            cmbCoating.Text = .Coating
                        End If

                        If (.SFinish = "0") Then
                            cmbSFinish.Text = ""
                        Else
                            cmbSFinish.Text = .SFinish
                        End If

                    End If

                    'AES 11OCT17
                    ''If (.Type = "C-Seal" Or .Type = "SC-Seal") Then
                    ''    If (.Plating.Code <> "") Then
                    ''        chkPlating.Checked = True
                    ''        cmbPlatingCode.Text = .Plating.Code
                    ''        cmbPlatingThickCode.Text = .Plating.ThickCode
                    ''    End If

                    ''    'AES 31JUL17
                    ''    mHW.DHfree = pDHFree
                    ''    mHW.DThetaOpening = pDThetaOpening
                    ''    If (pT > gcEPS) Then
                    ''        mHW.T = pT
                    ''    End If


                    ''    If (mHW.Adjusted) Then
                    ''        lblStandard.Text = "N"
                    ''        lblStandard.BackColor = Color.Yellow
                    ''    Else
                    ''        lblStandard.Text = "Y"
                    ''        lblStandard.BackColor = Color.White
                    ''    End If

                    ''End If

                    'AES 11OCT17
                    If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                        If (.Plating.Exists) Then
                            chkPlating.Checked = True
                            cmbPlatingCode.Text = .Plating.Code
                            cmbPlatingThickCode.Text = .Plating.ThickCode
                            If (.Plating.ThickCode = "X") Then
                                'txtPlatingThickMin.Text = mPartProject.PNR.HW.Plating.ThickMin.ToString(gUnit.TFormat) ' gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMin, "TFormat") 'gUnit.WriteInUserL(mHW.Plating.ThickMin)
                                'txtPlatingThickMax.Text = mPartProject.PNR.HW.Plating.ThickMax.ToString(gUnit.TFormat) 'gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMax, "TFormat")
                                If (mPartProject.PNR.HW.UnitSystem = "English") Then
                                    txtPlatingThickMin.Text = mPartProject.PNR.HW.Plating.ThickMin.ToString("##0.0000")
                                    txtPlatingThickMax.Text = mPartProject.PNR.HW.Plating.ThickMax.ToString("##0.0000")
                                Else
                                    txtPlatingThickMin.Text = mPartProject.PNR.HW.Plating.ThickMin.ToString("###0.000")
                                    txtPlatingThickMax.Text = mPartProject.PNR.HW.Plating.ThickMax.ToString("###0.000")
                                End If
                            Else
                                'txtPlatingThickMin.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMin, "TFormat") ' gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMin, "TFormat") 'gUnit.WriteInUserL(mHW.Plating.ThickMin)
                                'txtPlatingThickMax.Text = gUnit.WriteInUserL(mPartProject.PNR.HW.Plating.ThickMax, "TFormat")
                                Dim pMCSEntities As New SealIPEMCSDBEntities()

                                Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = .Plating.ThickCode
                                            Select pRec).ToList()

                                If (pQry.Count() > 0) Then
                                    If (mPartProject.PNR.HW.UnitSystem = "English") Then
                                        txtPlatingThickMin.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMinEng, "TFormat")
                                        txtPlatingThickMax.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMaxEng, "TFormat")
                                    Else
                                        txtPlatingThickMin.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMinMet, "TFormat")
                                        txtPlatingThickMax.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMaxMet, "TFormat")
                                    End If
                                End If
                            End If

                        Else
                            txtPlatingThickMin.Enabled = False
                            txtPlatingThickMax.Enabled = False
                            txtPlatingThickMin.Text = ""
                            txtPlatingThickMax.Text = ""
                        End If

                        'AES 31JUL17
                        mPartProject.PNR.HW.DHfree = pDHFree
                        mPartProject.PNR.HW.DThetaOpening = pDThetaOpening
                        If (pT > gcEPS) Then
                            mPartProject.PNR.HW.T = pT
                        End If


                        If (mPartProject.PNR.HW.Adjusted) Then
                            lblStandard.Text = "N"
                            lblStandard.BackColor = Color.Yellow
                        Else
                            lblStandard.Text = "Y"
                            lblStandard.BackColor = Color.White
                        End If

                    ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                        mPartProject.PNR.HW.DThetaE1 = pDThetaE1
                        mPartProject.PNR.HW.DThetaM1 = pDThetaM1

                        If (mPartProject.PNR.HW.Adjusted) Then
                            lblStandard.Text = "N"
                            lblStandard.BackColor = Color.Yellow
                        Else
                            lblStandard.Text = "Y"
                            lblStandard.BackColor = Color.White
                        End If

                        txtPlatingThickMin.Text = ""
                        txtPlatingThickMax.Text = ""

                    ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.U) Then
                        txtPlatingThickMin.Text = ""
                        txtPlatingThickMax.Text = ""

                    End If

                    If (.Hfree > gcEPS) Then
                        txtHFree.Text = gUnit.WriteInUserL(.Hfree)
                    End If
                    txtHFreeTolPlus.Text = gUnit.WriteInUserL(pHFreeTol1)
                    txtHFreeTolMinus.Text = gUnit.WriteInUserL(pHFreeTol2)
                    txtDControl.Text = gUnit.WriteInUserL(.DControl)
                    'txtH11Tol.Text = gUnit.WriteInUserL(.H11Tol)

                    txtThick.Text = gUnit.WriteInUserL(.TStd)

                End With

            End If

        End If

    End Sub


    'AES 02MAY17
    Private Function GetMCS(ByVal SealType_In As String, ByVal CCSCode_In As Integer) As String
        '=======================================================================================
        Dim pMCS As String = ""
        Dim pMCSEntities As New SealIPEMCSDBEntities()
        lblCrossSec.Text = "MCS"

        Select Case SealType_In

            Case "E"
                Dim pQry = (From pRec In pMCSEntities.tblESeal_Geom Where pRec.fldCCS = CCSCode_In Select pRec).First()
                pMCS = pQry.fldCrossSecNo.ToString()

            Case "C"
                Dim pQry = (From pRec In pMCSEntities.tblCSeal_Geom Where pRec.fldCCS = CCSCode_In Select pRec).First()
                pMCS = pQry.fldCrossSecNo.ToString()

            Case "S"
                Dim pQry = (From pRec In pMCSEntities.tblCSeal_Geom
                            Where pRec.fldCCS = CCSCode_In And pRec.fldSpringEnergized = True Select pRec).First()
                pMCS = pQry.fldCrossSecNo.ToString()

            Case "U"
                Dim pQry = (From pRec In pMCSEntities.tblUSeal_Geom Where pRec.fldCCS = CCSCode_In Select pRec).First()
                pMCS = pQry.fldCrossSecNo.ToString()

        End Select

        Return pMCS

    End Function


    'Private Function GetMatName(ByVal SealType_In As String, ByVal MatCode_In As String) As String
    '    '=========================================================================================
    '    Dim pMatName As String = ""

    '    Dim pMCSEntities As New SealMCSDBEntities()

    '    If (SealType_In = "S") Then
    '        Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
    '                               Where pRec.fldCode = MatCode_In Select pRec).First()

    '        pMatName = pQry.fldName.ToString().Trim()

    '        If (pMatName.Contains("Cobalt Chromium-Nickel Alloy")) Then
    '            pMatName = pMatName.Replace("Cobalt Chromium-Nickel Alloy", "Co-Cr-Ni")
    '        End If

    '    Else
    '        Dim pQry = (From pRec In pMCSEntities.tblMaterial
    '                               Where pRec.fldCode = MatCode_In Select pRec).First()

    '        pMatName = pQry.fldName.ToString().Trim()
    '    End If

    '    Return pMatName

    'End Function

    Private Sub InitializeControls()
        '===========================
        txtThick.ReadOnly = True
        txtHFree.ReadOnly = True
        txtH11Tol.ReadOnly = True

        If (gPartProject.PNR.Current.Exists) Then

            'Dim pSealType As String = gPartProject.PNR.SealType

            cmbType.Text = gPartProject.PNR.SealType '& "-Seal"  'pSealType
            cmbType.Enabled = False

            '....Coating
            If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                grpCoating.Enabled = True

                '....Populate Coating Combo Box:
                With cmbCoating
                    .Items.Clear()
                    .Items.Add("Tricom")
                    .Items.Add("Tricom-HT")
                    .Items.Add("T800")

                    .SelectedIndex = 0
                End With

                If mPartProject.PNR.HW.Coating <> "None" Then
                    chkCoating.Checked = True
                Else
                    chkCoating.Checked = False
                    cmbCoating.Enabled = False
                End If

                '....Populate Surface Finish Combo Box.
                PopulateCmbSFinish()

                If mPartProject.PNR.HW.Coating <> "" And mPartProject.PNR.HW.Coating <> "None" Then
                    cmbCoating.Enabled = True
                    cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
                    cmbCoating.Text = mPartProject.PNR.HW.Coating

                    If cmbCoating.Text = "T800" Then
                        lblSFinish.Enabled = True
                        cmbSFinish.Enabled = True
                        cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                    Else
                        lblSFinish.Enabled = False
                        cmbSFinish.Enabled = False
                        cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                        cmbSFinish.Text = ""
                    End If

                Else
                    cmbCoating.Enabled = False
                    cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
                    cmbCoating.Text = ""
                    lblSFinish.Enabled = False
                    cmbSFinish.Enabled = False
                    cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
                    cmbSFinish.Text = ""
                End If

                If cmbSFinish.Items.Count > 0 Then
                    If mPartProject.PNR.HW.SFinish > gcEPS Then
                        cmbSFinish.Text = mPartProject.PNR.HW.SFinish

                    Else
                        cmbSFinish.SelectedIndex = 0

                    End If
                End If

            Else
                grpCoating.Enabled = False
            End If

            '....Plating
            If (gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or gPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                grpPlating.Enabled = True
                chkPlating.Checked = False
                cmbPlatingCode.Enabled = False
                cmbPlatingThickCode.Enabled = False
                'PopulatePlatingType()
                'PopulatePlatingTCode()
            Else
                grpPlating.Enabled = False
            End If

        ElseIf (gPartProject.PNR.Legacy.Exists) Then
            If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                '....Catalogued
                cmbType.Enabled = False
                cmbPOrient.Enabled = False
                cmbCrossSec.Enabled = False
                cmbMatName.Enabled = False
                cmbHT.Enabled = False
                cmbTemperCode.Enabled = False
                grpCoating.Enabled = False
                grpPlating.Enabled = False
                txtDControl.Enabled = False

            Else
                '....Other
                cmbType.Enabled = True
                cmbPOrient.Enabled = True
                cmbCrossSec.Enabled = True
                cmbMatName.Enabled = True
                cmbHT.Enabled = True
                cmbTemperCode.Enabled = True

                If (cmbType.Text = "E") Then
                    grpCoating.Enabled = True
                Else
                    grpCoating.Enabled = False
                End If

                If (cmbType.Text = "C" Or cmbType.Text = "SC") Then
                    grpPlating.Enabled = True
                Else
                    grpPlating.Enabled = False
                End If

                txtDControl.Enabled = True

            End If

        End If
    End Sub

    Private Sub GetPartProjectInfo()
        '===========================
        Dim pPartEntities As New SealPartDBEntities
        Dim pQryProject = (From it In pPartEntities.tblProject
                           Where it.fldID = gPartProject.Project_ID Select it).ToList()

        If (pQryProject.Count() > 0) Then
            mPNID = pQryProject(0).fldPNID
            mRevID = pQryProject(0).fldRevID
        End If
    End Sub

    Private Sub PopulateCrossSection()
        '=============================
        Try
            cmbCrossSec.Items.Clear()
            Dim pMCSEntities As New SealIPEMCSDBEntities()

            Dim pSealType As String = cmbType.Text

            Select Case pSealType

                Case "E"
                    Dim pQry = (From pRec In pMCSEntities.tblESeal_Geom
                                Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
                    If (pQry.Count() > 0) Then
                        For i As Integer = 0 To pQry.Count() - 1
                            Dim pMCS As String = pQry(i).fldCrossSecNo.ToString().Trim()

                            If (pMCS <> "") Then
                                cmbCrossSec.Items.Add(pMCS)
                            End If
                        Next

                    End If

                Case "C"

                    Dim pQry = (From pRec In pMCSEntities.tblCSeal_Geom
                                Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
                    If (pQry.Count() > 0) Then
                        For i As Integer = 0 To pQry.Count() - 1
                            Dim pMCS As String = pQry(i).fldCrossSecNo.ToString().Trim()

                            If (pMCS <> "") Then
                                cmbCrossSec.Items.Add(pMCS)
                            End If
                        Next

                    End If

                Case "SC"

                    Dim pQry = (From pRec In pMCSEntities.tblCSeal_Geom
                                Where pRec.fldSpringEnergized = True Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
                    If (pQry.Count() > 0) Then
                        For i As Integer = 0 To pQry.Count() - 1
                            Dim pMCS As String = pQry(i).fldCrossSecNo.ToString().Trim()

                            If (pMCS <> "") Then
                                cmbCrossSec.Items.Add(pMCS)
                            End If
                        Next

                    End If

                Case "U"

                    If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                        Dim pQry = (From pRec In pMCSEntities.tblUSeal_Geom Where pRec.fldGeomTemplate = False
                                    Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
                        If (pQry.Count() > 0) Then
                            For i As Integer = 0 To pQry.Count() - 1
                                Dim pMCS As String = pQry(i).fldCrossSecNo.ToString().Trim()

                                If (pMCS <> "") Then
                                    cmbCrossSec.Items.Add(pMCS)
                                End If
                            Next

                        End If
                    Else
                        Dim pQry = (From pRec In pMCSEntities.tblUSeal_Geom Where pRec.fldGeomTemplate = True
                                    Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
                        If (pQry.Count() > 0) Then
                            For i As Integer = 0 To pQry.Count() - 1
                                Dim pMCS As String = pQry(i).fldCrossSecNo.ToString().Trim()

                                If (pMCS <> "") Then
                                    cmbCrossSec.Items.Add(pMCS)
                                End If
                            Next

                        End If

                    End If


                    ''If (gPartProject.Catalogued = True) Then

                    ''    Dim pQry = (From pRec In pMCSEntities.tblUSeal_Geom
                    ''                    Order By pRec.fldCCS Ascending Select pRec).ToList()
                    ''    If (pQry.Count() > 0) Then
                    ''        For i As Integer = 0 To pQry.Count() - 1
                    ''            If (Not IsDBNull(pQry(i).fldCCS) And (Not IsNothing(pQry(i).fldCCS))) Then
                    ''                Dim pCCS As String = pQry(i).fldCCS.ToString().Trim()
                    ''                If (pCCS <> "") Then
                    ''                    If (pCCS.Length = 1) Then
                    ''                        pCCS = "0" + pCCS
                    ''                    End If
                    ''                    cmbCrossSec.Items.Add(pCCS)
                    ''                End If
                    ''            End If
                    ''        Next

                    ''    End If

                    ''Else

                    ''    Dim pQry = (From pRec In pMCSEntities.tblUSeal_Geom
                    ''                   Order By pRec.fldCrossSecNo Ascending Select pRec).ToList()
                    ''    If (pQry.Count() > 0) Then
                    ''        For i As Integer = 0 To pQry.Count() - 1
                    ''            Dim pMCS As String = pQry(i).fldCrossSecNo.ToString().Trim()

                    ''            If (pMCS <> "" And pMCS.Length < 7) Then
                    ''                cmbCrossSec.Items.Add(pMCS)
                    ''            End If
                    ''        Next

                    ''    End If




                    ''End If

            End Select

            If (cmbCrossSec.Items.Count > 0) Then
                cmbCrossSec.SelectedIndex = 0
            End If

            ''If (Not IsNothing(mHW.MCrossSecNo)) Then
            ''    If (cmbCrossSec.Items.Count > 0) Then
            ''        cmbCrossSec.Text = mHW.MCrossSecNo
            ''    End If
            ''Else
            ''    If (cmbCrossSec.Items.Count > 0) Then
            ''        cmbCrossSec.SelectedIndex = 0
            ''    End If

            ''End If


        Catch ex As Exception

        End Try

    End Sub


    'Private Sub PopulateMatName()
    '    '========================
    '    cmbMatName.Items.Clear()

    '    Dim pMCSEntities As New SealMCSDBEntities()

    '    Dim pSealType As String = cmbType.Text

    '    If (pSealType = "SC-Seal") Then

    '        Dim pQry = (From pRec In pMCSEntities.tblMaterial_S
    '                                 Order By pRec.fldCode Ascending Select pRec).ToList()
    '        If (pQry.Count() > 0) Then
    '            For i As Integer = 0 To pQry.Count() - 1
    '                Dim pName As String = pQry(i).fldName.ToString().Trim()

    '                If (pName <> "") Then

    '                    cmbMatName.Items.Add(pName)
    '                End If
    '            Next

    '        End If
    '    Else

    '        Dim pQry = (From pRec In pMCSEntities.tblMaterial
    '                                 Order By pRec.fldCode Ascending Select pRec).ToList()

    '        If (pQry.Count() > 0) Then
    '            For i As Integer = 0 To pQry.Count() - 1
    '                Dim pName As String = pQry(i).fldName.ToString().Trim()

    '                If (pName <> "") Then
    '                    cmbMatName.Items.Add(pName)
    '                End If
    '            Next

    '        End If

    '    End If

    '    If (cmbMatName.Items.Count > 0) Then
    '        cmbMatName.SelectedIndex = 0
    '    End If


    'End Sub


    Private Sub PopulateTemperCode(ByVal MatName_In As String)
        '======================================================
        cmbTemperCode.Items.Clear()

        Dim pMCSEntities As New SealIPEMCSDBEntities()

        Dim pQryCountTemp = (From it In pMCSEntities.tblTemper Select it).ToList()

        If (pQryCountTemp.Count() > 0) Then

            For i As Integer = 0 To pQryCountTemp.Count - 1
                cmbTemperCode.Items.Add(pQryCountTemp(i).fldCode.ToString())
            Next

            If (cmbType.Text = "SC") Then

                If MatName_In.Contains("Co-Cr-Ni") Then
                    MatName_In = MatName_In.Replace("Co-Cr-Ni", "Cobalt Chromium-Nickel Alloy")
                End If

                Dim pCountMat As Integer = (From it In pMCSEntities.tblMaterial_S
                                            Where it.fldName = MatName_In Select it).Count()
                Dim pMatCode As Integer = 0

                If (pCountMat > 0) Then
                    Dim pQry_Mat = (From it In pMCSEntities.tblMaterial_S
                                    Where it.fldName = MatName_In Select it).First()
                    pMatCode = pQry_Mat.fldCode

                    Dim pCount_Temp As Integer = (From it In pMCSEntities.tblMatTemper_S
                                                  Where it.fldMatCode = pMatCode Select it).Count()

                    If (pCount_Temp > 0) Then

                        Dim pQry = (From pRec In pMCSEntities.tblMatTemper_S
                                    Where pRec.fldMatCode = pMatCode Select pRec).First()

                        Dim pTemp_All As String = pQry.fldTemperCode.Trim()

                        If (pTemp_All.Contains(",")) Then
                            Dim pTempCode() As String = pTemp_All.Split(",")

                            For i As Integer = 0 To pTempCode.Count - 1
                                cmbTemperCode.SelectedIndex = cmbTemperCode.Items.IndexOf(pTempCode(0).Trim())
                                Exit For
                            Next
                        Else
                            cmbTemperCode.SelectedIndex = cmbTemperCode.Items.IndexOf(pTemp_All.Trim())
                        End If

                    Else
                        cmbTemperCode.SelectedIndex = -1
                    End If

                Else
                    cmbTemperCode.SelectedIndex = -1
                End If

            Else

                Dim pCountMat As Integer = (From it In pMCSEntities.tblMaterial
                                            Where it.fldName = MatName_In Select it).Count()
                Dim pMatCode As Integer = 0

                If (pCountMat > 0) Then
                    Dim pQry_Mat = (From it In pMCSEntities.tblMaterial
                                    Where it.fldName = MatName_In Select it).First()
                    pMatCode = pQry_Mat.fldCode

                    Dim pCount_Temp As Integer = (From it In pMCSEntities.tblMatTemper
                                                  Where it.fldMatCode = pMatCode Select it).Count()

                    If (pCount_Temp > 0) Then

                        Dim pQry = (From pRec In pMCSEntities.tblMatTemper
                                    Where pRec.fldMatCode = pMatCode Select pRec).First()

                        Dim pTemp_All As String = pQry.fldTemperCode.Trim()

                        If (pTemp_All.Contains(",")) Then
                            Dim pTempCode() As String = pTemp_All.Split(",")

                            For i As Integer = 0 To pTempCode.Count - 1
                                cmbTemperCode.SelectedIndex = cmbTemperCode.Items.IndexOf(pTempCode(0).Trim())
                                Exit For
                            Next
                        Else
                            cmbTemperCode.SelectedIndex = cmbTemperCode.Items.IndexOf(pTemp_All.Trim())
                        End If


                    Else
                        cmbTemperCode.SelectedIndex = -1
                    End If

                Else
                    cmbTemperCode.SelectedIndex = -1

                End If

            End If

        End If

    End Sub


    Private Sub PopulatePlatingType()
        '============================
        cmbPlatingCode.Items.Clear()

        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pQry = (From pRec In pMCSEntities.tblPlatingType
                    Order By pRec.fldPlatingCode Ascending Select pRec).ToList()

        If (pQry.Count() > 0) Then
            For i As Integer = 0 To pQry.Count() - 1
                Dim pCode As String = pQry(i).fldPlatingCode.ToString()

                If (pCode <> "") Then
                    cmbPlatingCode.Items.Add(pCode)
                End If
            Next

        End If

        If (cmbPlatingCode.Items.Count > 0) Then
            cmbPlatingCode.SelectedIndex = 0
        End If

    End Sub


    Private Sub PopulatePlatingTCode()
        '============================
        cmbPlatingThickCode.Items.Clear()

        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pQry = (From pRec In pMCSEntities.tblPlatingThick
                    Order By pRec.fldPlatingThickCode Ascending Select pRec).ToList()

        If (pQry.Count() > 0) Then
            For i As Integer = 0 To pQry.Count() - 1
                Dim pCode As String = pQry(i).fldPlatingThickCode.ToString()

                If (pCode <> "") Then
                    cmbPlatingThickCode.Items.Add(pCode)
                End If
            Next

            '....User Editable ThickCode
            cmbPlatingThickCode.Items.Add("X")

        End If

        If (cmbPlatingThickCode.Items.Count > 0) Then
            cmbPlatingThickCode.SelectedIndex = 0
        End If

    End Sub

    Private Sub PopulateCmbSFinish()
        '===========================  
        '....This routine populates the Surface Finish combo box. (Database Driven).
        Dim pSealMCSEntity As New SealIPEMCSDBEntities()
        Dim pRecordLeak = (From it In pSealMCSEntity.tblESeal_Leak_T800
                           Select it.fldSFinish Distinct).ToList()

        cmbSFinish.Items.Clear()
        Dim pRecord As New tblESeal_Leak_T800

        If (pRecordLeak.Count > 0) Then
            For i As Integer = 0 To pRecordLeak.Count - 1
                cmbSFinish.Items.Add(pRecordLeak(i))
            Next
        End If

        cmbSFinish.SelectedIndex = 0
    End Sub


    Private Sub cmbType_SelectedIndexChanged(sender As System.Object,
                                             e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        '===============================================================================================
        'mHW.Type = cmbType.Text
        Dim pSealType As String = cmbType.Text
        Dim pSealTypeChanged As Boolean = False
        If (pSealType = "E") Then
            If (mPartProject.PNR.SealType <> clsPartProject.clsPNR.eType.E) Then
                pSealTypeChanged = True
            End If
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E
        ElseIf (pSealType = "C") Then
            If (mPartProject.PNR.SealType <> clsPartProject.clsPNR.eType.C) Then
                pSealTypeChanged = True
            End If
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C
        ElseIf (pSealType = "SC") Then
            If (mPartProject.PNR.SealType <> clsPartProject.clsPNR.eType.SC) Then
                pSealTypeChanged = True
            End If
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC
        ElseIf (pSealType = "U") Then
            If (mPartProject.PNR.SealType <> clsPartProject.clsPNR.eType.U) Then
                pSealTypeChanged = True
            End If
            mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.U
        End If

        If (pSealTypeChanged) Then
            mPartProject.PNR.HW.InitializePNR(mPartProject.PNR)     'AES 07DEC17
            'RetrieveFromDB()        'AES 07DEC17
        End If


        PopulateCrossSection()
        mPartProject.PNR.HW.MCrossSecNo = cmbCrossSec.Text

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
            grpCoating.Enabled = True
        Else
            grpCoating.Enabled = False

        End If

        chkCoating.Checked = False
        cmbCoating.Enabled = False
        cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
        cmbCoating.Text = ""

        lblSFinish.Enabled = False
        cmbSFinish.Enabled = False
        lblUnitSFinish.Enabled = False
        cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
        cmbSFinish.Text = ""

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then

            If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
                grpPlating.Enabled = False
            Else
                grpPlating.Enabled = True
            End If

            cmdNonStdCS.Visible = True

        ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then       'AES 11OCT17
            grpPlating.Enabled = False
            cmdNonStdCS.Visible = True

        Else
            grpPlating.Enabled = False
            cmdNonStdCS.Visible = False
            lblStandard.Text = "Y"
            lblStandard.BackColor = Color.White
        End If

        chkPlating.Checked = False
        cmbPlatingCode.Enabled = False
        cmbPlatingCode.DropDownStyle = ComboBoxStyle.DropDown
        cmbPlatingCode.Text = ""

        cmbPlatingThickCode.Enabled = False
        cmbPlatingThickCode.DropDownStyle = ComboBoxStyle.DropDown
        cmbPlatingThickCode.Text = ""

        PopulateMatComboBox(mPartProject.PNR.SealType.ToString(), cmbMatName)

    End Sub


    Private Sub chkSeg_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                     Handles chkSeg.CheckedChanged
        '==============================================================================
        If (chkSeg.Checked) Then
            lblSegNo.Enabled = True
            txtSegNo.Enabled = True
            txtSegNo.Text = mPartProject.PNR.HW.CountSegment

        Else
            lblSegNo.Enabled = False
            txtSegNo.Text = ""
            txtSegNo.Enabled = False
        End If

        mPartProject.PNR.HW.IsSegmented = chkSeg.Checked

    End Sub

#Region "TEXTBOX RELATED ROUTINES"

    Private Sub txtSegNo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSegNo.TextChanged
        '========================================================================================================
        If (mPartProject.PNR.HW.IsSegmented) Then
            If (txtSegNo.Text <> "") Then
                mPartProject.PNR.HW.CountSegment = Convert.ToInt16(txtSegNo.Text)
            End If

        End If

    End Sub


    'Private Sub txtSegNo_Validated(sender As System.Object, e As System.EventArgs) Handles txtSegNo.Validated
    '    '=====================================================================================================
    'If (mHW.IsSegmented) Then

    '    If (Not mHW.CountSegment > 1) Then
    '        MessageBox.Show("No. of segment is always greater than 1", "Segment Count", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        txtSegNo.Focus()
    '        Return
    '    End If

    'End If
    'End Sub

#End Region

    Private Sub cmbMatName_SelectedIndexChanged(sender As System.Object,
                                                e As System.EventArgs) Handles cmbMatName.SelectedIndexChanged
        '======================================================================================================
        Pupulate_HT(cmbMatName.Text)
        PopulateTemperCode(cmbMatName.Text)
    End Sub


    Private Sub Pupulate_HT(ByVal MatName_In As String)
        '===============================================
        Dim pSealMCSEntity As New SealIPEMCSDBEntities()
        cmbHT.Items.Clear()

        If cmbType.Text = "SC" Then

            'AES 28JUL17
            ''If MatName_In.Contains("Co-Cr-Ni") Then
            ''    MatName_In = MatName_In.Replace("Co-Cr-Ni", "Cobalt Chromium-Nickel Alloy")
            ''End If
            ''Dim pCount As Integer = (From it In pSealMCSEntity.tblMaterial_S
            ''                        Where it.fldName = MatName_In Select it).Count()
            ''Dim pMatCode As String = ""
            ''If (pCount > 0) Then
            ''    Dim pQry_Mat = (From it In pSealMCSEntity.tblMaterial_S
            ''                        Where it.fldName = MatName_In Select it).First()
            ''    pMatCode = pQry_Mat.fldCode

            ''    Dim pCount_HT As Integer = (From it In pSealMCSEntity.tblHT
            ''                                    Where it.fldMatCode = pMatCode Select it).Count()
            ''    If (pCount_HT > 0) Then
            ''        Dim pQry_HT = (From it In pSealMCSEntity.tblHT
            ''                        Where it.fldMatCode = pMatCode Select it).First()
            ''        cmbHT.Items.Add(pQry_HT.fldCode)
            ''        cmbHT.SelectedIndex = 0
            ''    End If
            ''Else
            ''    cmbHT.SelectedIndex = -1
            ''End If

            cmbHT.SelectedIndex = -1
        Else

            Dim pCount As Integer = (From it In pSealMCSEntity.tblMaterial
                                     Where it.fldName = MatName_In Select it).Count()
            'AES 06MAR18
            Dim pMatCode As Integer = 0
            If (pCount > 0) Then
                Dim pQry_Mat = (From it In pSealMCSEntity.tblMaterial
                                Where it.fldName = MatName_In Select it).First()
                pMatCode = pQry_Mat.fldCode

                Dim pCount_HT As Integer = (From it In pSealMCSEntity.tblHT
                                            Where it.fldMatCode = pMatCode Select it).Count()
                Dim pCode As String = ""
                If (pCount_HT > 0) Then
                    Dim pQry_HT = (From it In pSealMCSEntity.tblHT
                                   Where it.fldMatCode = pMatCode Select it).First()

                    'AES 06MAR18
                    Dim pHT_All As String = pQry_HT.fldCode

                    If (pHT_All.Contains(",")) Then
                        Dim pHT() As String = pHT_All.Split(",")

                        For i As Integer = 0 To pHT.Count - 1
                            cmbHT.Items.Add(pHT(i).Trim())
                            cmbHT.SelectedIndex = 0
                        Next
                    Else
                        cmbHT.Items.Add(pHT_All.Trim())
                        cmbHT.SelectedIndex = 0
                    End If

                End If
            Else
                cmbHT.SelectedIndex = -1
            End If

        End If

    End Sub

    Private Sub chkCoating_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                          Handles chkCoating.CheckedChanged
        '==================================================================================
        If chkCoating.Checked Then
            cmbCoating.Enabled = True
            cmbCoating.DropDownStyle = ComboBoxStyle.DropDownList
            cmbCoating.SelectedIndex = 0
            cmbCoating.Text = mPartProject.PNR.HW.Coating

            If cmbCoating.Text = "T800" Then
                lblSFinish.Enabled = True
                cmbSFinish.Enabled = True
                cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
                cmbSFinish.Text = mPartProject.PNR.HW.SFinish.ToString()
                lblUnitSFinish.Enabled = True
            End If

        Else
            cmbCoating.Enabled = False
            cmbCoating.DropDownStyle = ComboBoxStyle.DropDown
            cmbCoating.Text = ""

            lblSFinish.Enabled = False
            cmbSFinish.Enabled = False
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
            cmbSFinish.Text = ""
            lblUnitSFinish.Enabled = False

        End If
    End Sub

    Private Sub cmbCoating_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles cmbCoating.SelectedIndexChanged
        '=========================================================================================
        Dim pCoat As String = cmbCoating.Text
        If pCoat = "T800" Then
            lblSFinish.Enabled = True
            cmbSFinish.Enabled = True
            lblUnitSFinish.Enabled = True
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDownList
            cmbSFinish.SelectedIndex = 0
        Else
            lblSFinish.Enabled = False
            cmbSFinish.Enabled = False
            lblUnitSFinish.Enabled = False
            cmbSFinish.DropDownStyle = ComboBoxStyle.DropDown
            cmbSFinish.Text = ""
        End If
    End Sub

    Private Sub chkPlating_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                          Handles chkPlating.CheckedChanged
        '======================================================================================
        If (chkPlating.Checked) Then
            'If (grpPlating.Enabled) Then
            cmbPlatingCode.Enabled = True
            cmbPlatingThickCode.Enabled = True
            mPartProject.PNR.HW.PlatingExists = True

            cmbPlatingCode.DropDownStyle = ComboBoxStyle.DropDownList
            cmbPlatingThickCode.DropDownStyle = ComboBoxStyle.DropDownList

            cmbPlatingCode.SelectedIndex = 0
            cmbPlatingThickCode.SelectedIndex = -1
            cmbPlatingThickCode.SelectedIndex = 0
            'End If

        Else
            cmbPlatingCode.Enabled = False
            cmbPlatingThickCode.Enabled = False
            mPartProject.PNR.HW.PlatingExists = False

            cmbPlatingCode.DropDownStyle = ComboBoxStyle.DropDown
            cmbPlatingCode.Text = ""

            cmbPlatingThickCode.DropDownStyle = ComboBoxStyle.DropDown
            cmbPlatingThickCode.Text = ""

            txtPlatingThickMin.Text = ""
            txtPlatingThickMax.Text = ""
        End If

        If (gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then
            grpPlating.Enabled = False
        End If

    End Sub


    Private Sub cmbCrossSec_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
                                                Handles cmbCrossSec.SelectedIndexChanged
        '==========================================================================================
        mPartProject.PNR.HW.POrient = cmbPOrient.Text
        'mHW.Type = cmbType.Text
        mPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), cmbType.Text), clsPartProject.clsPNR.eType) 'cmbType.Text

        'AES 31JUL17
        If (mPartProject.PNR.HW.MCrossSecNo <> cmbCrossSec.Text) Then

            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                mPartProject.PNR.HW.DHfree = 0.0
                mPartProject.PNR.HW.DThetaOpening = 0.0

                Dim pCSeal As New IPE_clsCSeal("C-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
                pCSeal.MCrossSecNo = cmbCrossSec.Text

                mPartProject.PNR.HW.T = pCSeal.T


            ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then        'AES 11OCT17

                mPartProject.PNR.HW.DThetaE1 = 0.0
                mPartProject.PNR.HW.DThetaM1 = 0.0

                Dim pESeal As New IPE_clsESeal("E-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
                pESeal.MCrossSecNo = cmbCrossSec.Text
            End If

        End If

        mPartProject.PNR.HW.MCrossSecNo = cmbCrossSec.Text

        With mPartProject.PNR.HW
            If (.Hfree > gcEPS) Then
                txtHFree.Text = gUnit.WriteInUserL(.Hfree)
            End If
            txtHFreeTolMinus.Text = gUnit.WriteInUserL(.HFreeTol(1))
            txtHFreeTolPlus.Text = gUnit.WriteInUserL(.HFreeTol(2))
            txtThick.Text = gUnit.WriteInUserL(.TStd)

            'AES 31JUL17
            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                mPartProject.PNR.HW.DHfree = mPartProject.PNR.HW.DHfree
                If (mPartProject.PNR.HW.Adjusted) Then
                    lblStandard.Text = "N"
                    lblStandard.BackColor = Color.Yellow
                Else
                    lblStandard.Text = "Y"
                    lblStandard.BackColor = Color.White
                End If
            ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                If (mPartProject.PNR.HW.Adjusted) Then
                    lblStandard.Text = "N"
                    lblStandard.BackColor = Color.Yellow
                Else
                    lblStandard.Text = "Y"
                    lblStandard.BackColor = Color.White
                End If
            Else
                lblStandard.Text = "Y"
                lblStandard.BackColor = Color.White

            End If

        End With

    End Sub


    Private Sub txtDControl_TextChanged(sender As System.Object, e As System.EventArgs) _
                                        Handles txtDControl.TextChanged
        '==================================================================================
        Dim pDControl As Double = 0
        If (txtDControl.Text <> "") Then
            pDControl = Convert.ToDouble(txtDControl.Text)
            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                Dim pESeal As New IPE_clsESeal("E-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
                txtH11Tol.Text = gUnit.WriteInUserL(pESeal.H11Tol_Calc(pDControl))

            ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                Dim pCSeal As New IPE_clsCSeal("C-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
                txtH11Tol.Text = gUnit.WriteInUserL(pCSeal.H11Tol_Calc(pDControl))

            ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.U) Then
                Dim pUSeal As New IPE_clsUSeal("U-Seal", mPartProject.PNR.HW.UnitSystem, mPartProject.PNR.HW.POrient)
                txtH11Tol.Text = gUnit.WriteInUserL(pUSeal.H11Tol_Calc(pDControl))

            End If

        End If

    End Sub


    Private Function SetForeColor_Pink(ByVal OrgVal_In As Double, ByVal CurrVal_In As Double) As Color
        '==============================================================================================               
        Dim pColor As Color = Color.Black
        pColor = IIf(Abs(CurrVal_In - OrgVal_In) < gcEPS,
                                         Color.Magenta, Color.Black)

        Return pColor

    End Function


    Private Function SetForeColor_Blue(ByVal OrgVal_In As Double, ByVal CurrVal_In As Double) As Color
        '==============================================================================================               
        Dim pColor As Color = Color.Black
        pColor = IIf(Abs(CurrVal_In - OrgVal_In) < gcEPS,
                                         Color.Blue, Color.Black)

        Return pColor

    End Function


    Private Sub txtThick_TextChanged(sender As System.Object, e As System.EventArgs) _
                                    Handles txtThick.TextChanged
        '=============================================================================
        'Dim pThick As Double = 0
        'If (txtThick.Text <> "") Then
        '    pThick = Convert.ToDouble(txtThick.Text)
        '    txtThick.ForeColor = SetForeColor_Pink(mHW.T, pThick)

        'End If

    End Sub

    Private Sub txtHFree_TextChanged(sender As System.Object, e As System.EventArgs) _
                                    Handles txtHFree.TextChanged
        '==============================================================================
        'Dim pHFree As Double = 0
        'If (txtHFree.Text <> "") Then
        '    pHFree = Convert.ToDouble(txtHFree.Text)
        '    txtHFree.ForeColor = SetForeColor_Blue(mHW.Hfree, pHFree)
        '    'If (mHW.Type = "C-Seal" Or mHW.Type = "SC-Seal") Then
        '    '    txtHFree.ForeColor = SetForeColor_Blue(mHW.Hfree, pHFree)
        '    'Else
        '    '    txtHFree.ForeColor = Color.Black
        '    'End If

        'End If
    End Sub

    Private Sub txtHFreeTolPlus_TextChanged(sender As System.Object, e As System.EventArgs) _
                                            Handles txtHFreeTolPlus.TextChanged
        '====================================================================================
        Dim pHFreeTol As Double = 0
        If (txtHFreeTolPlus.Text <> "") Then
            pHFreeTol = Convert.ToDouble(txtHFreeTolPlus.Text)
            txtHFreeTolPlus.ForeColor = SetForeColor_Pink(mPartProject.PNR.HW.HFreeTolStd, pHFreeTol)

        End If
    End Sub

    Private Sub txtHFreeTolMinus_TextChanged(sender As System.Object, e As System.EventArgs) _
                                            Handles txtHFreeTolMinus.TextChanged
        '======================================================================================
        Dim pHFreeTol As Double = 0
        If (txtHFreeTolMinus.Text <> "") Then
            pHFreeTol = Convert.ToDouble(txtHFreeTolMinus.Text)
            txtHFreeTolMinus.ForeColor = SetForeColor_Pink(mPartProject.PNR.HW.HFreeTolStd, pHFreeTol)

        End If
    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        '======================================================================================
        If (mPartProject.PNR.HW.IsSegmented) Then
            If (Not mPartProject.PNR.HW.CountSegment > 1) Then
                MessageBox.Show("No. of segment is always greater than 1", "Segment Count", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtSegNo.Focus()
                Return
            End If
        End If

        SaveData()
        'SaveToDB()
        mPartProject.PNR.SaveToDB(mPNID, mRevID)        'AES 18APR18
        Me.Close()
    End Sub

    Private Sub SaveData()
        '==================
        With mPartProject.PNR.HW
            mPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), cmbType.Text), clsPartProject.clsPNR.eType) 'cmbType.Text'cmbType.Text
            .POrient = cmbPOrient.Text
            .MCrossSecNo = cmbCrossSec.Text
            .IsSegmented = chkSeg.Checked
            If (.IsSegmented) Then
                .CountSegment = Convert.ToInt16(txtSegNo.Text)
            Else
                .CountSegment = 0
            End If

            .MatName = cmbMatName.Text
            If (cmbHT.Text <> "") Then
                .HT = Convert.ToInt16(cmbHT.Text)
            Else
                .HT = 0
            End If

            If (cmbTemperCode.Text <> "") Then
                .Temper = Convert.ToInt16(cmbTemperCode.Text)
            Else
                .Temper = 0
            End If

            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
                If (chkCoating.Checked) Then
                    .Coating = cmbCoating.Text

                    If (.Coating = "T800") Then
                        .SFinish = Convert.ToInt16(cmbSFinish.Text)
                    End If
                Else
                    .Coating = "None"
                    .SFinish = 0
                End If
            End If

            If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
                .PlatingExists = chkPlating.Checked
                If (.Plating.Exists) Then
                    .PlatingCode = cmbPlatingCode.Text
                    .PlatingThickCode = cmbPlatingThickCode.Text
                    .PlatingThickMin = Convert.ToDouble(txtPlatingThickMin.Text)
                    .PlatingThickMax = Convert.ToDouble(txtPlatingThickMax.Text)
                Else
                    .PlatingCode = ""
                    .PlatingThickCode = ""
                    .PlatingThickMin = 0
                    .PlatingThickMax = 0
                End If
            End If

            If (txtHFree.Text <> "") Then
                .Hfree = Convert.ToDouble(txtHFree.Text)
            Else
                .Hfree = 0
            End If

            If (txtHFreeTolPlus.Text <> "") Then
                .HFreeTol(1) = Convert.ToDouble(txtHFreeTolPlus.Text)
            Else
                .HFreeTol(1) = 0
            End If

            If (txtHFreeTolMinus.Text <> "") Then
                .HFreeTol(2) = Convert.ToDouble(txtHFreeTolMinus.Text)
            Else
                .HFreeTol(2) = 0
            End If


            If (txtDControl.Text <> "") Then
                .DControl = Convert.ToDouble(txtDControl.Text)
            Else
                .DControl = 0
            End If

            If (txtH11Tol.Text <> "") Then
                .H11Tol = Convert.ToDouble(txtH11Tol.Text)
            Else
                .H11Tol = 0
            End If

            If (txtThick.Text <> "") Then
                .TStd = Convert.ToDouble(txtThick.Text)
            Else
                .TStd = 0
            End If

        End With

        gPartProject.PNR.HW = mPartProject.PNR.HW.Clone()      'AES 02NOV17

    End Sub


    ''Private Sub SaveToDB()
    ''    '================
    ''    Dim pPartEntities As New SealPartDBEntities()

    ''    '....HW_Face table
    ''    Dim pHWFace_Rec_Count As Integer = (From HWFace In pPartEntities.tblHW_Face
    ''                                        Where HWFace.fldPNID = mPNID And
    ''                                        HWFace.fldRevID = mRevID Select HWFace).Count()
    ''    If (pHWFace_Rec_Count > 0) Then
    ''        '....Record already exists
    ''        Dim pHWFace_Rec = (From HWFace In pPartEntities.tblHW_Face
    ''                           Where HWFace.fldPNID = mPNID And
    ''                                        HWFace.fldRevID = mRevID Select HWFace).First()
    ''        pHWFace_Rec.fldType = mPartProject.PNR.SealType.ToString()
    ''        pHWFace_Rec.fldMCS = mPartProject.PNR.HW.MCrossSecNo
    ''        pHWFace_Rec.fldSegmented = mPartProject.PNR.HW.IsSegmented
    ''        If (mPartProject.PNR.HW.IsSegmented) Then
    ''            pHWFace_Rec.fldSegmentCount = mPartProject.PNR.HW.CountSegment
    ''        Else
    ''            pHWFace_Rec.fldSegmentCount = 0
    ''        End If
    ''        pHWFace_Rec.fldMatName = mPartProject.PNR.HW.MatName
    ''        pHWFace_Rec.fldHT = mPartProject.PNR.HW.HT
    ''        pHWFace_Rec.fldTemper = mPartProject.PNR.HW.Temper
    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
    ''            pHWFace_Rec.fldCoating = mPartProject.PNR.HW.Coating
    ''            pHWFace_Rec.fldSFinish = mPartProject.PNR.HW.SFinish
    ''        End If
    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
    ''            If (mPartProject.PNR.HW.Plating.Exists) Then
    ''                pHWFace_Rec.fldIsPlating = True
    ''            Else
    ''                pHWFace_Rec.fldIsPlating = False
    ''            End If

    ''            pHWFace_Rec.fldPlatingCode = mPartProject.PNR.HW.Plating.Code
    ''            pHWFace_Rec.fldPlatingThickCode = mPartProject.PNR.HW.Plating.ThickCode
    ''            pHWFace_Rec.fldPlatingThickMin = mPartProject.PNR.HW.Plating.ThickMin
    ''            pHWFace_Rec.fldPlatingThickMax = mPartProject.PNR.HW.Plating.ThickMax

    ''            SaveToDB_NonStd_CSeal()
    ''        ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then

    ''            SaveToDB_NonStd_ESeal()
    ''        End If

    ''        If (txtHFree.Text <> "") Then
    ''            pHWFace_Rec.fldHfreeStd = Convert.ToDouble(txtHFree.Text)
    ''        Else
    ''            pHWFace_Rec.fldHfreeStd = 0
    ''        End If

    ''        pHWFace_Rec.fldHFreeTol1 = mPartProject.PNR.HW.HFreeTol(1)
    ''        pHWFace_Rec.fldHFreeTol2 = mPartProject.PNR.HW.HFreeTol(2)
    ''        pHWFace_Rec.fldPOrient = mPartProject.PNR.HW.POrient

    ''        If (txtDControl.Text <> "") Then
    ''            pHWFace_Rec.fldDControl = Convert.ToDouble(txtDControl.Text)
    ''        Else
    ''            pHWFace_Rec.fldDControl = 0
    ''        End If

    ''        If (txtH11Tol.Text <> "") Then
    ''            pHWFace_Rec.fldH11Tol = Convert.ToDouble(txtH11Tol.Text)
    ''        Else
    ''            pHWFace_Rec.fldH11Tol = 0
    ''        End If

    ''        'AES 31JUL17
    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
    ''            pHWFace_Rec.fldAdjusted = mPartProject.PNR.HW.Adjusted
    ''        Else
    ''            pHWFace_Rec.fldAdjusted = False
    ''        End If


    ''        pPartEntities.SaveChanges()

    ''    Else
    ''        '....New Record
    ''        Dim pHWFace As New tblHW_Face
    ''        pHWFace.fldPNID = mPNID
    ''        pHWFace.fldRevID = mRevID

    ''        pHWFace.fldType = gPartProject.PNR.SealType.ToString()
    ''        pHWFace.fldMCS = mPartProject.PNR.HW.MCrossSecNo
    ''        pHWFace.fldSegmented = mPartProject.PNR.HW.IsSegmented
    ''        If (mPartProject.PNR.HW.IsSegmented) Then
    ''            pHWFace.fldSegmentCount = mPartProject.PNR.HW.CountSegment
    ''        Else
    ''            pHWFace.fldSegmentCount = 0
    ''        End If
    ''        pHWFace.fldMatName = mPartProject.PNR.HW.MatName
    ''        pHWFace.fldHT = mPartProject.PNR.HW.HT
    ''        pHWFace.fldTemper = mPartProject.PNR.HW.Temper
    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
    ''            pHWFace.fldCoating = mPartProject.PNR.HW.Coating
    ''            pHWFace.fldSFinish = mPartProject.PNR.HW.SFinish
    ''        End If
    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
    ''            If (mPartProject.PNR.HW.Plating.Exists) Then
    ''                pHWFace.fldIsPlating = True
    ''            Else
    ''                pHWFace.fldIsPlating = False
    ''            End If

    ''            pHWFace.fldPlatingCode = mPartProject.PNR.HW.Plating.Code
    ''            pHWFace.fldPlatingThickCode = mPartProject.PNR.HW.Plating.ThickCode

    ''            pHWFace.fldPlatingThickMin = mPartProject.PNR.HW.Plating.ThickMin
    ''            pHWFace.fldPlatingThickMax = mPartProject.PNR.HW.Plating.ThickMax

    ''        End If

    ''        If (txtHFree.Text <> "") Then
    ''            pHWFace.fldHfreeStd = Convert.ToDouble(txtHFree.Text)
    ''        Else
    ''            pHWFace.fldHfreeStd = 0
    ''        End If

    ''        pHWFace.fldHFreeTol1 = mPartProject.PNR.HW.HFreeTol(1)
    ''        pHWFace.fldHFreeTol2 = mPartProject.PNR.HW.HFreeTol(2)
    ''        pHWFace.fldPOrient = mPartProject.PNR.HW.POrient

    ''        If (txtDControl.Text <> "") Then
    ''            pHWFace.fldDControl = Convert.ToDouble(txtDControl.Text)
    ''        Else
    ''            pHWFace.fldDControl = 0
    ''        End If

    ''        If (txtH11Tol.Text <> "") Then
    ''            pHWFace.fldH11Tol = Convert.ToDouble(txtH11Tol.Text)
    ''        Else
    ''            pHWFace.fldH11Tol = 0
    ''        End If

    ''        'pHWFace.fldAdjusted = False
    ''        'AES 31JUL17
    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
    ''            pHWFace.fldAdjusted = mPartProject.PNR.HW.Adjusted
    ''            pPartEntities.AddTotblHW_Face(pHWFace)
    ''            pPartEntities.SaveChanges()
    ''            SaveToDB_NonStd_CSeal()

    ''        ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then       'AES 11OCT17
    ''            pHWFace.fldAdjusted = mPartProject.PNR.HW.Adjusted
    ''            pPartEntities.AddTotblHW_Face(pHWFace)
    ''            pPartEntities.SaveChanges()
    ''            SaveToDB_NonStd_ESeal()
    ''        Else
    ''            pHWFace.fldAdjusted = False
    ''            pPartEntities.AddTotblHW_Face(pHWFace)
    ''            pPartEntities.SaveChanges()
    ''        End If

    ''    End If

    ''End Sub


    ''Private Sub SaveToDB_NonStd_CSeal()
    ''    '==============================
    ''    Dim pPartEntities As New SealPartDBEntities()

    ''    '....HW_AdjCSeal table
    ''    Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    ''                                             Where HWFace_AdjCSeal.fldPNID = mPNID And
    ''                                        HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).Count()
    ''    If (pHW_AdjCSeal_Rec_Count > 0) Then
    ''        '....Record already exists
    ''        Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    ''                                    Where HWFace_AdjCSeal.fldPNID = mPNID And
    ''                                        HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).First()

    ''        pHWFace_AdjCSeal_Rec.fldDHFree = mPartProject.PNR.HW.DHfree
    ''        pHWFace_AdjCSeal_Rec.fldDThetaOpening = mPartProject.PNR.HW.DThetaOpening
    ''        pHWFace_AdjCSeal_Rec.fldDT = mPartProject.PNR.HW.T

    ''        pPartEntities.SaveChanges()

    ''    Else
    ''        '....New Record
    ''        Dim pHWFace_AdjCSeal As New tblHW_AdjCSeal
    ''        pHWFace_AdjCSeal.fldPNID = mPNID
    ''        pHWFace_AdjCSeal.fldRevID = mRevID

    ''        pHWFace_AdjCSeal.fldDHFree = mPartProject.PNR.HW.DHfree
    ''        pHWFace_AdjCSeal.fldDThetaOpening = mPartProject.PNR.HW.DThetaOpening
    ''        pHWFace_AdjCSeal.fldDT = mPartProject.PNR.HW.T

    ''        pPartEntities.AddTotblHW_AdjCSeal(pHWFace_AdjCSeal)
    ''        pPartEntities.SaveChanges()
    ''    End If

    ''End Sub

    '''AES 11OCT17
    ''Private Sub SaveToDB_NonStd_ESeal()
    ''    '==============================
    ''    Dim pPartEntities As New SealPartDBEntities()

    ''    '....HW_AdjESeal table
    ''    Dim pHW_AdjESeal_Rec_Count As Integer = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
    ''                                             Where HWFace_AdjESeal.fldPNID = mPNID And
    ''                                        HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).Count()
    ''    If (pHW_AdjESeal_Rec_Count > 0) Then
    ''        '....Record already exists
    ''        Dim pHWFace_AdjESeal_Rec = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
    ''                                    Where HWFace_AdjESeal.fldPNID = mPNID And
    ''                                        HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).First()

    ''        pHWFace_AdjESeal_Rec.fldDThetaE1 = mPartProject.PNR.HW.DThetaE1
    ''        pHWFace_AdjESeal_Rec.fldDThetaM1 = mPartProject.PNR.HW.DThetaM1

    ''        pPartEntities.SaveChanges()

    ''    Else
    ''        '....New Record
    ''        Dim pHWFace_AdjESeal As New tblHW_AdjESeal
    ''        pHWFace_AdjESeal.fldPNID = mPNID
    ''        pHWFace_AdjESeal.fldRevID = mRevID

    ''        pHWFace_AdjESeal.fldDThetaE1 = mPartProject.PNR.HW.DThetaE1
    ''        pHWFace_AdjESeal.fldDThetaM1 = mPartProject.PNR.HW.DThetaM1

    ''        pPartEntities.AddTotblHW_AdjESeal(pHWFace_AdjESeal)
    ''        pPartEntities.SaveChanges()
    ''    End If

    ''End Sub


    ''Private Sub RetrieveFromDB()
    ''    '=======================
    ''    Try

    ''        Dim pPartEntities As New SealPartDBEntities()

    ''        '....HW_Face table
    ''        Dim pHWFace_Rec_Count As Integer = (From HWFace In pPartEntities.tblHW_Face
    ''                                            Where HWFace.fldPNID = mPNID And
    ''                                            HWFace.fldRevID = mRevID Select HWFace).Count()
    ''        If (pHWFace_Rec_Count > 0) Then

    ''            Dim pHWFace_Rec = (From HWFace In pPartEntities.tblHW_Face
    ''                               Where HWFace.fldPNID = mPNID And
    ''                                        HWFace.fldRevID = mRevID Select HWFace).First()

    ''            Dim pType As String = pHWFace_Rec.fldType.ToString().Trim()
    ''            mPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), pType), clsPartProject.clsPNR.eType)
    ''            mPartProject.PNR.HW.InitializePNR(mPartProject.PNR)

    ''            If (gPartProject.PNR.Legacy.Exists And gPartProject.PNR.Legacy.Type = clsPartProject.clsPNR.eLegacyType.Catalogued) Then

    ''                Dim pSealType As String = pHWFace_Rec.fldType.ToString().Trim()

    ''                With mPartProject.PNR.HW
    ''                    mPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsPartProject.clsPNR.eType), pSealType), clsPartProject.clsPNR.eType) 'pSealType
    ''                    .MCrossSecNo = pHWFace_Rec.fldMCS
    ''                    '.Hfree = pHWFace_Rec.fldHfreeStd
    ''                    .HFreeTol(1) = pHWFace_Rec.fldHFreeTol1
    ''                    .HFreeTol(2) = pHWFace_Rec.fldHFreeTol2
    ''                    .T = .TStd      'AES 02AUG17

    ''                    If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
    ''                        '....HW_AdjCSeal table
    ''                        Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    ''                                                                 Where HWFace_AdjCSeal.fldPNID = mPNID And
    ''                                                        HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).Count()
    ''                        If (pHW_AdjCSeal_Rec_Count > 0) Then

    ''                            Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    ''                                                        Where HWFace_AdjCSeal.fldPNID = mPNID And
    ''                                                        HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).First()

    ''                            With mPartProject.PNR.HW
    ''                                If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDHFree)) Then
    ''                                    .DHfree = pHWFace_AdjCSeal_Rec.fldDHFree
    ''                                Else
    ''                                    .DHfree = 0.0#
    ''                                End If

    ''                                If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDThetaOpening)) Then
    ''                                    .DThetaOpening = pHWFace_AdjCSeal_Rec.fldDThetaOpening
    ''                                Else
    ''                                    .DThetaOpening = 0.0#
    ''                                End If

    ''                                If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDT)) Then
    ''                                    .T = pHWFace_AdjCSeal_Rec.fldDT
    ''                                Else
    ''                                    .T = 0.0#
    ''                                End If

    ''                            End With

    ''                        End If

    ''                        If (Not IsDBNull(pHWFace_Rec.fldIsPlating) And Not IsNothing(pHWFace_Rec.fldIsPlating)) Then
    ''                            .PlatingExists = pHWFace_Rec.fldIsPlating
    ''                        End If

    ''                        If (Not IsDBNull(pHWFace_Rec.fldPlatingThickCode) And Not IsNothing(pHWFace_Rec.fldPlatingThickCode)) Then
    ''                            .PlatingThickCode = pHWFace_Rec.fldPlatingThickCode

    ''                            If (.Plating.ThickCode = "X") Then
    ''                                .PlatingThickMin = pHWFace_Rec.fldPlatingThickMin
    ''                                .PlatingThickMax = pHWFace_Rec.fldPlatingThickMax

    ''                            Else
    ''                                Dim pMCSEntities As New SealIPEMCSDBEntities()

    ''                                Dim pThickCode As String = .Plating.ThickCode
    ''                                Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = pThickCode
    ''                                            Select pRec).ToList()

    ''                                If (pQry.Count() > 0) Then

    ''                                    If (mPartProject.PNR.HW.UnitSystem = "English") Then
    ''                                        .PlatingThickMin = pQry(0).fldPlatingThickMinEng
    ''                                        .PlatingThickMax = pQry(0).fldPlatingThickMaxEng
    ''                                    Else
    ''                                        .PlatingThickMin = pQry(0).fldPlatingThickMinMet
    ''                                        .PlatingThickMax = pQry(0).fldPlatingThickMaxMet
    ''                                    End If
    ''                                End If

    ''                            End If
    ''                        Else
    ''                            .PlatingThickCode = ""
    ''                            .PlatingThickMin = 0
    ''                            .PlatingThickMax = 0
    ''                        End If
    ''                        'Else
    ''                        '    .PlatingThickCode = ""
    ''                        '    .PlatingThickMin = 0
    ''                        '    .PlatingThickMax = 0
    ''                        'End If


    ''                    ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then        'AES 11OCT17

    ''                        '....HW_AdjESeal table
    ''                        Dim pHW_AdjESeal_Rec_Count As Integer = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
    ''                                                                 Where HWFace_AdjESeal.fldPNID = mPNID And
    ''                                                        HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).Count()
    ''                        If (pHW_AdjESeal_Rec_Count > 0) Then

    ''                            Dim pHWFace_AdjESeal_Rec = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
    ''                                                        Where HWFace_AdjESeal.fldPNID = mPNID And
    ''                                                        HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).First()

    ''                            With mPartProject.PNR.HW
    ''                                If (Not IsNothing(pHWFace_AdjESeal_Rec.fldDThetaE1)) Then
    ''                                    .DThetaE1 = pHWFace_AdjESeal_Rec.fldDThetaE1
    ''                                Else
    ''                                    .DThetaE1 = 0.0#
    ''                                End If

    ''                                If (Not IsNothing(pHWFace_AdjESeal_Rec.fldDThetaM1)) Then
    ''                                    .DThetaM1 = pHWFace_AdjESeal_Rec.fldDThetaM1
    ''                                Else
    ''                                    .DThetaM1 = 0.0#
    ''                                End If


    ''                            End With

    ''                        End If

    ''                    End If

    ''                    Exit Sub
    ''                End With

    ''            Else
    ''                If (gPartProject.PNR.Current.Exists) Then
    ''                    Dim pPN As String = gPartProject.PNR.PN
    ''                    Dim pSealType As String = ""
    ''                    Dim pSealType_No As String = pPN.Substring(3, 2)

    ''                    Select Case pSealType_No

    ''                        Case "69"
    ''                            pSealType = "E"

    ''                        Case "76"
    ''                            pSealType = "C"

    ''                        Case "79"
    ''                            pSealType = "U"

    ''                        Case "44"
    ''                            pSealType = "SC"

    ''                    End Select

    ''                    If (pSealType <> pHWFace_Rec.fldType) Then
    ''                        Exit Sub
    ''                    End If

    ''                End If

    ''            End If

    ''            With mPartProject.PNR.HW
    ''                'Dim pSealType As String = pHWFace_Rec.fldType
    ''                'mPartProject.PNR.SealType = CType([Enum].Parse(GetType(clsProject.clsPNR.eType), pSealType), clsProject.clsPNR.eType)
    ''                .POrient = pHWFace_Rec.fldPOrient
    ''                .MCrossSecNo = pHWFace_Rec.fldMCS
    ''                .IsSegmented = pHWFace_Rec.fldSegmented
    ''                If (.IsSegmented) Then
    ''                    .CountSegment = pHWFace_Rec.fldSegmentCount
    ''                End If

    ''                .MatName = pHWFace_Rec.fldMatName
    ''                .HT = pHWFace_Rec.fldHT
    ''                .Temper = pHWFace_Rec.fldTemper
    ''                .T = .TStd      'AES 31JUL17

    ''                If (Not IsDBNull(pHWFace_Rec.fldCoating) And Not IsNothing(pHWFace_Rec.fldCoating)) Then
    ''                    .Coating = pHWFace_Rec.fldCoating
    ''                Else
    ''                    .Coating = "None"
    ''                End If

    ''                If (Not IsDBNull(pHWFace_Rec.fldSFinish) And Not IsNothing(pHWFace_Rec.fldSFinish)) Then
    ''                    .SFinish = pHWFace_Rec.fldSFinish
    ''                Else
    ''                    .SFinish = 0
    ''                End If

    ''                If (Not IsDBNull(pHWFace_Rec.fldIsPlating) And Not IsNothing(pHWFace_Rec.fldIsPlating)) Then
    ''                    .PlatingExists = pHWFace_Rec.fldIsPlating
    ''                End If

    ''                If (Not IsDBNull(pHWFace_Rec.fldPlatingCode) And Not IsNothing(pHWFace_Rec.fldPlatingCode)) Then
    ''                    .PlatingCode = pHWFace_Rec.fldPlatingCode

    ''                Else
    ''                    .PlatingCode = ""
    ''                End If

    ''                If (Not IsDBNull(pHWFace_Rec.fldPlatingThickCode) And Not IsNothing(pHWFace_Rec.fldPlatingThickCode)) Then
    ''                    .PlatingThickCode = pHWFace_Rec.fldPlatingThickCode

    ''                    If (.Plating.ThickCode = "X") Then
    ''                        .PlatingThickMin = pHWFace_Rec.fldPlatingThickMin
    ''                        .PlatingThickMax = pHWFace_Rec.fldPlatingThickMax

    ''                    Else
    ''                        Dim pMCSEntities As New SealIPEMCSDBEntities()

    ''                        Dim pThickCode As String = .Plating.ThickCode
    ''                        Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = pThickCode
    ''                                    Select pRec).ToList()

    ''                        If (pQry.Count() > 0) Then

    ''                            If (mPartProject.PNR.HW.UnitSystem = "English") Then
    ''                                .PlatingThickMin = pQry(0).fldPlatingThickMinEng
    ''                                .PlatingThickMax = pQry(0).fldPlatingThickMaxEng
    ''                            Else
    ''                                .PlatingThickMin = pQry(0).fldPlatingThickMinMet
    ''                                .PlatingThickMax = pQry(0).fldPlatingThickMaxMet
    ''                            End If
    ''                        Else
    ''                            .PlatingThickCode = ""
    ''                            .PlatingThickMin = 0
    ''                            .PlatingThickMax = 0
    ''                        End If

    ''                    End If
    ''                Else
    ''                    .PlatingThickCode = ""
    ''                    .PlatingThickMin = 0
    ''                    .PlatingThickMax = 0
    ''                End If

    ''                '.Hfree = pHWFace_Rec.fldHfreeStd
    ''                .HFreeTol(1) = pHWFace_Rec.fldHFreeTol1
    ''                .HFreeTol(2) = pHWFace_Rec.fldHFreeTol2
    ''                .DControl = pHWFace_Rec.fldDControl
    ''                '.H11Tol = pHWFace_Rec.fldH11Tol

    ''            End With

    ''        End If

    ''        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
    ''            '....HW_AdjCSeal table
    ''            Dim pHW_AdjCSeal_Rec_Count As Integer = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    ''                                                     Where HWFace_AdjCSeal.fldPNID = mPNID And
    ''                                            HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).Count()
    ''            If (pHW_AdjCSeal_Rec_Count > 0) Then

    ''                Dim pHWFace_AdjCSeal_Rec = (From HWFace_AdjCSeal In pPartEntities.tblHW_AdjCSeal
    ''                                            Where HWFace_AdjCSeal.fldPNID = mPNID And
    ''                                                HWFace_AdjCSeal.fldRevID = mRevID Select HWFace_AdjCSeal).First()

    ''                With mPartProject.PNR.HW
    ''                    If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDHFree)) Then
    ''                        .DHfree = pHWFace_AdjCSeal_Rec.fldDHFree
    ''                    Else
    ''                        .DHfree = 0.0#
    ''                    End If

    ''                    If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDThetaOpening)) Then
    ''                        .DThetaOpening = pHWFace_AdjCSeal_Rec.fldDThetaOpening
    ''                    Else
    ''                        .DThetaOpening = 0.0#
    ''                    End If

    ''                    If (Not IsNothing(pHWFace_AdjCSeal_Rec.fldDT)) Then
    ''                        .T = pHWFace_AdjCSeal_Rec.fldDT
    ''                    Else
    ''                        .T = 0.0#
    ''                    End If

    ''                End With

    ''            End If
    ''        ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then        'AES 11OCT17

    ''            '....HW_AdjESeal table
    ''            Dim pHW_AdjESeal_Rec_Count As Integer = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
    ''                                                     Where HWFace_AdjESeal.fldPNID = mPNID And
    ''                                             HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).Count()
    ''            If (pHW_AdjESeal_Rec_Count > 0) Then

    ''                Dim pHWFace_AdjESeal_Rec = (From HWFace_AdjESeal In pPartEntities.tblHW_AdjESeal
    ''                                            Where HWFace_AdjESeal.fldPNID = mPNID And
    ''                                                HWFace_AdjESeal.fldRevID = mRevID Select HWFace_AdjESeal).First()

    ''                With mPartProject.PNR.HW
    ''                    If (Not IsNothing(pHWFace_AdjESeal_Rec.fldDThetaE1)) Then
    ''                        .DThetaE1 = pHWFace_AdjESeal_Rec.fldDThetaE1
    ''                    Else
    ''                        .DThetaE1 = 0.0#
    ''                    End If

    ''                    If (Not IsNothing(pHWFace_AdjESeal_Rec.fldDThetaM1)) Then
    ''                        .DThetaM1 = pHWFace_AdjESeal_Rec.fldDThetaM1
    ''                    Else
    ''                        .DThetaM1 = 0.0#
    ''                    End If

    ''                End With

    ''            End If

    ''        End If
    ''    Catch ex As Exception

    ''    End Try

    ''End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) _
                                Handles cmdCancel.Click
        '========================================================================
        Me.Close()
    End Sub

    Private Sub cmdNonStdCS_Click(sender As System.Object, e As System.EventArgs) _
                                  Handles cmdNonStdCS.Click
        '==========================================================================
        'Dim pfrmNonStdCS_CSeal As New frmNonStdCS_CSeal_Old(mHW)
        SaveData()

        If (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.C Or mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.SC) Then
            Dim pfrmNonStdCS_CSeal As New Part_frmNonStdCS_CSeal(mPartProject)
            pfrmNonStdCS_CSeal.ShowDialog()
        ElseIf (mPartProject.PNR.SealType = clsPartProject.clsPNR.eType.E) Then
            Dim pfrmNonStdCS_ESeal As New Part_frmNonStdCS_ESeal(mPartProject)
            pfrmNonStdCS_ESeal.ShowDialog()
        End If

    End Sub


    'Private Sub cmbPlatingThickCode_DrawItem(sender As System.Object,
    '                                         e As System.Windows.Forms.DrawItemEventArgs) Handles cmbPlatingThickCode.DrawItem
    '    '=======================================================================================================================

    'End Sub

    'Private Sub cmbPlatingThickCode_MouseHover(sender As System.Object, e As System.EventArgs) Handles cmbPlatingThickCode.MouseHover
    '    '==================================================================
    '    If (cmbPlatingThickCode.Text = "B") Then
    '        ttpToolTip1.SetToolTip(cmbPlatingThickCode, ".0010 - .0015")
    '    Else
    '        ttpToolTip1.SetToolTip(cmbPlatingThickCode, "")
    '    End If
    'End Sub

    Private Sub cmbPlatingThickCode_SelectedIndexChanged(sender As System.Object,
                                                         e As System.EventArgs) Handles cmbPlatingThickCode.SelectedIndexChanged
        '========================================================================================================================
        Dim pMCSEntities As New SealIPEMCSDBEntities()
        Dim pThickCode As String = cmbPlatingThickCode.Text
        If (pThickCode = "X") Then
            txtPlatingThickMin.Enabled = True
            txtPlatingThickMax.Enabled = True
            txtPlatingThickMin.Text = ""
            txtPlatingThickMax.Text = ""
        ElseIf (pThickCode <> "") Then
            txtPlatingThickMin.Enabled = False
            txtPlatingThickMax.Enabled = False
            Dim pQry = (From pRec In pMCSEntities.tblPlatingThick Where pRec.fldPlatingThickCode = pThickCode
                        Select pRec).ToList()

            If (pQry.Count() > 0) Then
                If (mPartProject.PNR.HW.UnitSystem = "English") Then
                    txtPlatingThickMin.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMinEng, "TFormat")
                    txtPlatingThickMax.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMaxEng, "TFormat")
                Else
                    txtPlatingThickMin.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMinMet, "TFormat")
                    txtPlatingThickMax.Text = gUnit.WriteInUserL(pQry(0).fldPlatingThickMaxMet, "TFormat")
                End If
            End If
        Else
            txtPlatingThickMin.Enabled = False
            txtPlatingThickMax.Enabled = False
            txtPlatingThickMin.Text = ""
            txtPlatingThickMax.Text = ""

        End If

        If (cmbPlatingThickCode.Text = "X") Then
            ttpToolTip1.SetToolTip(cmbPlatingThickCode, "User editable thickness range.")
        Else
            ttpToolTip1.SetToolTip(cmbPlatingThickCode, "")
        End If

    End Sub
End Class
