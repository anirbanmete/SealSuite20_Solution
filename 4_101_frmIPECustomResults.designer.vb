<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPE_frmCustomResults
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IPE_frmCustomResults))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkMatModel = New System.Windows.Forms.CheckBox()
        Me.chkLoadCaseType = New System.Windows.Forms.CheckBox()
        Me.chkName = New System.Windows.Forms.CheckBox()
        Me.chkCompTolType = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkUnitSeatingLoad = New System.Windows.Forms.CheckBox()
        Me.chkLeakage_BL = New System.Windows.Forms.CheckBox()
        Me.chkSpringBack = New System.Windows.Forms.CheckBox()
        Me.grpAppliedLoading = New System.Windows.Forms.GroupBox()
        Me.chkPreCompressedH = New System.Windows.Forms.CheckBox()
        Me.chkRadCon = New System.Windows.Forms.CheckBox()
        Me.chkPreCompressed = New System.Windows.Forms.CheckBox()
        Me.grpAnalysis = New System.Windows.Forms.GroupBox()
        Me.chkHFree = New System.Windows.Forms.CheckBox()
        Me.chkMCS = New System.Windows.Forms.CheckBox()
        Me.chkDateCreated = New System.Windows.Forms.CheckBox()
        Me.chkTimeCreated = New System.Windows.Forms.CheckBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpCavirtyDim = New System.Windows.Forms.GroupBox()
        Me.chkCavityID = New System.Windows.Forms.CheckBox()
        Me.chkCavityOD = New System.Windows.Forms.CheckBox()
        Me.chkCavityDepth = New System.Windows.Forms.CheckBox()
        Me.grpOpCond = New System.Windows.Forms.GroupBox()
        Me.chkTOper = New System.Windows.Forms.CheckBox()
        Me.chkPOrient = New System.Windows.Forms.CheckBox()
        Me.chkPress = New System.Windows.Forms.CheckBox()
        Me.grpSeal = New System.Windows.Forms.GroupBox()
        Me.grpGroupBox3 = New System.Windows.Forms.GroupBox()
        Me.grpUSeal = New System.Windows.Forms.GroupBox()
        Me.chkDT_USeal = New System.Windows.Forms.CheckBox()
        Me.chkDLLeg = New System.Windows.Forms.CheckBox()
        Me.chkDRad2 = New System.Windows.Forms.CheckBox()
        Me.chkDRad1 = New System.Windows.Forms.CheckBox()
        Me.chkDTheta1 = New System.Windows.Forms.CheckBox()
        Me.chkDTheta2 = New System.Windows.Forms.CheckBox()
        Me.grpCSeal = New System.Windows.Forms.GroupBox()
        Me.chkDT_CSeal = New System.Windows.Forms.CheckBox()
        Me.chkDHFree = New System.Windows.Forms.CheckBox()
        Me.chkDThetaOpening = New System.Windows.Forms.CheckBox()
        Me.grpESeal = New System.Windows.Forms.GroupBox()
        Me.chkDThetaE1 = New System.Windows.Forms.CheckBox()
        Me.chkDThetaM1 = New System.Windows.Forms.CheckBox()
        Me.chkAdjusted = New System.Windows.Forms.CheckBox()
        Me.chkZClear = New System.Windows.Forms.CheckBox()
        Me.chkSFinish = New System.Windows.Forms.CheckBox()
        Me.chkSegCount = New System.Windows.Forms.CheckBox()
        Me.chkCoating = New System.Windows.Forms.CheckBox()
        Me.chkHT = New System.Windows.Forms.CheckBox()
        Me.chkPlating = New System.Windows.Forms.CheckBox()
        Me.chkSeg = New System.Windows.Forms.CheckBox()
        Me.chkMatName = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpAppliedLoading.SuspendLayout()
        Me.grpAnalysis.SuspendLayout()
        Me.grpCavirtyDim.SuspendLayout()
        Me.grpOpCond.SuspendLayout()
        Me.grpSeal.SuspendLayout()
        Me.grpGroupBox3.SuspendLayout()
        Me.grpUSeal.SuspendLayout()
        Me.grpCSeal.SuspendLayout()
        Me.grpESeal.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Location = New System.Drawing.Point(3, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(724, 477)
        Me.Label1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.grpAppliedLoading)
        Me.Panel1.Controls.Add(Me.grpAnalysis)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.grpCavirtyDim)
        Me.Panel1.Controls.Add(Me.grpOpCond)
        Me.Panel1.Controls.Add(Me.grpSeal)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(722, 475)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkMatModel)
        Me.GroupBox2.Controls.Add(Me.chkLoadCaseType)
        Me.GroupBox2.Controls.Add(Me.chkName)
        Me.GroupBox2.Controls.Add(Me.chkCompTolType)
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.Black
        Me.GroupBox2.Location = New System.Drawing.Point(555, 17)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(154, 110)
        Me.GroupBox2.TabIndex = 57
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Load Case:"
        '
        'chkMatModel
        '
        Me.chkMatModel.AutoSize = True
        Me.chkMatModel.Checked = True
        Me.chkMatModel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMatModel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatModel.Location = New System.Drawing.Point(10, 88)
        Me.chkMatModel.Name = "chkMatModel"
        Me.chkMatModel.Size = New System.Drawing.Size(108, 17)
        Me.chkMatModel.TabIndex = 57
        Me.chkMatModel.Text = "Material Model"
        '
        'chkLoadCaseType
        '
        Me.chkLoadCaseType.AutoSize = True
        Me.chkLoadCaseType.Checked = True
        Me.chkLoadCaseType.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLoadCaseType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLoadCaseType.Location = New System.Drawing.Point(10, 21)
        Me.chkLoadCaseType.Name = "chkLoadCaseType"
        Me.chkLoadCaseType.Size = New System.Drawing.Size(54, 17)
        Me.chkLoadCaseType.TabIndex = 56
        Me.chkLoadCaseType.Text = "Type"
        '
        'chkName
        '
        Me.chkName.AutoSize = True
        Me.chkName.Checked = True
        Me.chkName.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkName.Location = New System.Drawing.Point(10, 67)
        Me.chkName.Name = "chkName"
        Me.chkName.Size = New System.Drawing.Size(59, 17)
        Me.chkName.TabIndex = 49
        Me.chkName.Text = "Name"
        '
        'chkCompTolType
        '
        Me.chkCompTolType.AutoSize = True
        Me.chkCompTolType.Checked = True
        Me.chkCompTolType.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCompTolType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCompTolType.Location = New System.Drawing.Point(10, 44)
        Me.chkCompTolType.Name = "chkCompTolType"
        Me.chkCompTolType.Size = New System.Drawing.Size(141, 17)
        Me.chkCompTolType.TabIndex = 55
        Me.chkCompTolType.Text = "Compress Tol. Type"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkUnitSeatingLoad)
        Me.GroupBox1.Controls.Add(Me.chkLeakage_BL)
        Me.GroupBox1.Controls.Add(Me.chkSpringBack)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Black
        Me.GroupBox1.Location = New System.Drawing.Point(555, 142)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(154, 98)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Results:"
        '
        'chkUnitSeatingLoad
        '
        Me.chkUnitSeatingLoad.AutoSize = True
        Me.chkUnitSeatingLoad.Checked = True
        Me.chkUnitSeatingLoad.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUnitSeatingLoad.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnitSeatingLoad.Location = New System.Drawing.Point(10, 68)
        Me.chkUnitSeatingLoad.Name = "chkUnitSeatingLoad"
        Me.chkUnitSeatingLoad.Size = New System.Drawing.Size(126, 17)
        Me.chkUnitSeatingLoad.TabIndex = 42
        Me.chkUnitSeatingLoad.Text = "Unit Seating Load"
        '
        'chkLeakage_BL
        '
        Me.chkLeakage_BL.AutoSize = True
        Me.chkLeakage_BL.Checked = True
        Me.chkLeakage_BL.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLeakage_BL.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLeakage_BL.Location = New System.Drawing.Point(10, 43)
        Me.chkLeakage_BL.Name = "chkLeakage_BL"
        Me.chkLeakage_BL.Size = New System.Drawing.Size(95, 17)
        Me.chkLeakage_BL.TabIndex = 41
        Me.chkLeakage_BL.Text = "Leakage_BL"
        '
        'chkSpringBack
        '
        Me.chkSpringBack.AutoSize = True
        Me.chkSpringBack.Checked = True
        Me.chkSpringBack.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSpringBack.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpringBack.Location = New System.Drawing.Point(10, 20)
        Me.chkSpringBack.Name = "chkSpringBack"
        Me.chkSpringBack.Size = New System.Drawing.Size(95, 17)
        Me.chkSpringBack.TabIndex = 36
        Me.chkSpringBack.Text = "Spring Back"
        '
        'grpAppliedLoading
        '
        Me.grpAppliedLoading.Controls.Add(Me.chkPreCompressedH)
        Me.grpAppliedLoading.Controls.Add(Me.chkRadCon)
        Me.grpAppliedLoading.Controls.Add(Me.chkPreCompressed)
        Me.grpAppliedLoading.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpAppliedLoading.ForeColor = System.Drawing.Color.Black
        Me.grpAppliedLoading.Location = New System.Drawing.Point(377, 17)
        Me.grpAppliedLoading.Name = "grpAppliedLoading"
        Me.grpAppliedLoading.Size = New System.Drawing.Size(166, 110)
        Me.grpAppliedLoading.TabIndex = 55
        Me.grpAppliedLoading.TabStop = False
        Me.grpAppliedLoading.Text = " Applied Loading:"
        '
        'chkPreCompressedH
        '
        Me.chkPreCompressedH.AutoSize = True
        Me.chkPreCompressedH.Checked = True
        Me.chkPreCompressedH.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPreCompressedH.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPreCompressedH.Location = New System.Drawing.Point(10, 42)
        Me.chkPreCompressedH.Name = "chkPreCompressedH"
        Me.chkPreCompressedH.Size = New System.Drawing.Size(153, 17)
        Me.chkPreCompressedH.TabIndex = 56
        Me.chkPreCompressedH.Text = "Pre-Compressed HMin"
        '
        'chkRadCon
        '
        Me.chkRadCon.AutoSize = True
        Me.chkRadCon.Checked = True
        Me.chkRadCon.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRadCon.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRadCon.Location = New System.Drawing.Point(10, 64)
        Me.chkRadCon.Name = "chkRadCon"
        Me.chkRadCon.Size = New System.Drawing.Size(124, 17)
        Me.chkRadCon.TabIndex = 35
        Me.chkRadCon.Text = "Radial Constraint"
        '
        'chkPreCompressed
        '
        Me.chkPreCompressed.AutoSize = True
        Me.chkPreCompressed.Checked = True
        Me.chkPreCompressed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPreCompressed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPreCompressed.Location = New System.Drawing.Point(10, 20)
        Me.chkPreCompressed.Name = "chkPreCompressed"
        Me.chkPreCompressed.Size = New System.Drawing.Size(122, 17)
        Me.chkPreCompressed.TabIndex = 33
        Me.chkPreCompressed.Text = "Pre-Compressed"
        '
        'grpAnalysis
        '
        Me.grpAnalysis.Controls.Add(Me.chkHFree)
        Me.grpAnalysis.Controls.Add(Me.chkMCS)
        Me.grpAnalysis.Controls.Add(Me.chkDateCreated)
        Me.grpAnalysis.Controls.Add(Me.chkTimeCreated)
        Me.grpAnalysis.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpAnalysis.ForeColor = System.Drawing.Color.Black
        Me.grpAnalysis.Location = New System.Drawing.Point(12, 17)
        Me.grpAnalysis.Name = "grpAnalysis"
        Me.grpAnalysis.Size = New System.Drawing.Size(118, 110)
        Me.grpAnalysis.TabIndex = 54
        Me.grpAnalysis.TabStop = False
        Me.grpAnalysis.Text = "Analysis:"
        '
        'chkHFree
        '
        Me.chkHFree.AutoSize = True
        Me.chkHFree.Checked = True
        Me.chkHFree.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHFree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHFree.Location = New System.Drawing.Point(10, 42)
        Me.chkHFree.Name = "chkHFree"
        Me.chkHFree.Size = New System.Drawing.Size(91, 17)
        Me.chkHFree.TabIndex = 39
        Me.chkHFree.Text = "Free Height"
        '
        'chkMCS
        '
        Me.chkMCS.AutoSize = True
        Me.chkMCS.Checked = True
        Me.chkMCS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMCS.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMCS.Location = New System.Drawing.Point(10, 20)
        Me.chkMCS.Name = "chkMCS"
        Me.chkMCS.Size = New System.Drawing.Size(52, 17)
        Me.chkMCS.TabIndex = 33
        Me.chkMCS.Text = "MCS"
        '
        'chkDateCreated
        '
        Me.chkDateCreated.AutoSize = True
        Me.chkDateCreated.Checked = True
        Me.chkDateCreated.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDateCreated.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDateCreated.Location = New System.Drawing.Point(10, 64)
        Me.chkDateCreated.Name = "chkDateCreated"
        Me.chkDateCreated.Size = New System.Drawing.Size(103, 17)
        Me.chkDateCreated.TabIndex = 35
        Me.chkDateCreated.Text = "Date Created"
        '
        'chkTimeCreated
        '
        Me.chkTimeCreated.AutoSize = True
        Me.chkTimeCreated.Checked = True
        Me.chkTimeCreated.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTimeCreated.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTimeCreated.Location = New System.Drawing.Point(10, 86)
        Me.chkTimeCreated.Name = "chkTimeCreated"
        Me.chkTimeCreated.Size = New System.Drawing.Size(104, 17)
        Me.chkTimeCreated.TabIndex = 36
        Me.chkTimeCreated.Text = "Time Created"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.Color.Silver
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Image = CType(resources.GetObject("cmdOK.Image"), System.Drawing.Image)
        Me.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdOK.Location = New System.Drawing.Point(555, 438)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(72, 28)
        Me.cmdOK.TabIndex = 52
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.Silver
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = CType(resources.GetObject("cmdCancel.Image"), System.Drawing.Image)
        Me.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCancel.Location = New System.Drawing.Point(637, 438)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 28)
        Me.cmdCancel.TabIndex = 53
        Me.cmdCancel.Text = "  &Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'grpCavirtyDim
        '
        Me.grpCavirtyDim.Controls.Add(Me.chkCavityID)
        Me.grpCavirtyDim.Controls.Add(Me.chkCavityOD)
        Me.grpCavirtyDim.Controls.Add(Me.chkCavityDepth)
        Me.grpCavirtyDim.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCavirtyDim.ForeColor = System.Drawing.Color.Black
        Me.grpCavirtyDim.Location = New System.Drawing.Point(142, 17)
        Me.grpCavirtyDim.Name = "grpCavirtyDim"
        Me.grpCavirtyDim.Size = New System.Drawing.Size(122, 110)
        Me.grpCavirtyDim.TabIndex = 2
        Me.grpCavirtyDim.TabStop = False
        Me.grpCavirtyDim.Text = "Cavity:"
        '
        'chkCavityID
        '
        Me.chkCavityID.AutoSize = True
        Me.chkCavityID.Checked = True
        Me.chkCavityID.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCavityID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCavityID.Location = New System.Drawing.Point(10, 42)
        Me.chkCavityID.Name = "chkCavityID"
        Me.chkCavityID.Size = New System.Drawing.Size(104, 17)
        Me.chkCavityID.TabIndex = 35
        Me.chkCavityID.Text = "Cavity Dia ID"
        '
        'chkCavityOD
        '
        Me.chkCavityOD.AutoSize = True
        Me.chkCavityOD.Checked = True
        Me.chkCavityOD.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCavityOD.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCavityOD.Location = New System.Drawing.Point(10, 20)
        Me.chkCavityOD.Name = "chkCavityOD"
        Me.chkCavityOD.Size = New System.Drawing.Size(108, 17)
        Me.chkCavityOD.TabIndex = 34
        Me.chkCavityOD.Text = "Cavity Dia OD"
        '
        'chkCavityDepth
        '
        Me.chkCavityDepth.AutoSize = True
        Me.chkCavityDepth.Checked = True
        Me.chkCavityDepth.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCavityDepth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCavityDepth.Location = New System.Drawing.Point(10, 64)
        Me.chkCavityDepth.Name = "chkCavityDepth"
        Me.chkCavityDepth.Size = New System.Drawing.Size(101, 17)
        Me.chkCavityDepth.TabIndex = 33
        Me.chkCavityDepth.Text = "Cavity Depth"
        '
        'grpOpCond
        '
        Me.grpOpCond.Controls.Add(Me.chkTOper)
        Me.grpOpCond.Controls.Add(Me.chkPOrient)
        Me.grpOpCond.Controls.Add(Me.chkPress)
        Me.grpOpCond.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpOpCond.ForeColor = System.Drawing.Color.Black
        Me.grpOpCond.Location = New System.Drawing.Point(276, 17)
        Me.grpOpCond.Name = "grpOpCond"
        Me.grpOpCond.Size = New System.Drawing.Size(89, 110)
        Me.grpOpCond.TabIndex = 1
        Me.grpOpCond.TabStop = False
        Me.grpOpCond.Text = "OpCond:"
        '
        'chkTOper
        '
        Me.chkTOper.AutoSize = True
        Me.chkTOper.Checked = True
        Me.chkTOper.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTOper.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTOper.Location = New System.Drawing.Point(10, 42)
        Me.chkTOper.Name = "chkTOper"
        Me.chkTOper.Size = New System.Drawing.Size(61, 17)
        Me.chkTOper.TabIndex = 38
        Me.chkTOper.Text = "TOper"
        '
        'chkPOrient
        '
        Me.chkPOrient.AutoSize = True
        Me.chkPOrient.Checked = True
        Me.chkPOrient.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPOrient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPOrient.Location = New System.Drawing.Point(10, 64)
        Me.chkPOrient.Name = "chkPOrient"
        Me.chkPOrient.Size = New System.Drawing.Size(68, 17)
        Me.chkPOrient.TabIndex = 34
        Me.chkPOrient.Text = "POrient"
        '
        'chkPress
        '
        Me.chkPress.AutoSize = True
        Me.chkPress.Checked = True
        Me.chkPress.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPress.Location = New System.Drawing.Point(10, 20)
        Me.chkPress.Name = "chkPress"
        Me.chkPress.Size = New System.Drawing.Size(76, 17)
        Me.chkPress.TabIndex = 33
        Me.chkPress.Text = "Pressure"
        '
        'grpSeal
        '
        Me.grpSeal.Controls.Add(Me.grpGroupBox3)
        Me.grpSeal.Controls.Add(Me.chkZClear)
        Me.grpSeal.Controls.Add(Me.chkSFinish)
        Me.grpSeal.Controls.Add(Me.chkSegCount)
        Me.grpSeal.Controls.Add(Me.chkCoating)
        Me.grpSeal.Controls.Add(Me.chkHT)
        Me.grpSeal.Controls.Add(Me.chkPlating)
        Me.grpSeal.Controls.Add(Me.chkSeg)
        Me.grpSeal.Controls.Add(Me.chkMatName)
        Me.grpSeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpSeal.ForeColor = System.Drawing.Color.Black
        Me.grpSeal.Location = New System.Drawing.Point(12, 142)
        Me.grpSeal.Name = "grpSeal"
        Me.grpSeal.Size = New System.Drawing.Size(531, 299)
        Me.grpSeal.TabIndex = 0
        Me.grpSeal.TabStop = False
        Me.grpSeal.Text = "Seal Design Parameters:"
        '
        'grpGroupBox3
        '
        Me.grpGroupBox3.Controls.Add(Me.grpUSeal)
        Me.grpGroupBox3.Controls.Add(Me.grpCSeal)
        Me.grpGroupBox3.Controls.Add(Me.grpESeal)
        Me.grpGroupBox3.Controls.Add(Me.chkAdjusted)
        Me.grpGroupBox3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGroupBox3.Location = New System.Drawing.Point(11, 96)
        Me.grpGroupBox3.Name = "grpGroupBox3"
        Me.grpGroupBox3.Size = New System.Drawing.Size(499, 194)
        Me.grpGroupBox3.TabIndex = 56
        Me.grpGroupBox3.TabStop = False
        Me.grpGroupBox3.Text = "     Adjusted:"
        '
        'grpUSeal
        '
        Me.grpUSeal.Controls.Add(Me.chkDT_USeal)
        Me.grpUSeal.Controls.Add(Me.chkDLLeg)
        Me.grpUSeal.Controls.Add(Me.chkDRad2)
        Me.grpUSeal.Controls.Add(Me.chkDRad1)
        Me.grpUSeal.Controls.Add(Me.chkDTheta1)
        Me.grpUSeal.Controls.Add(Me.chkDTheta2)
        Me.grpUSeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpUSeal.ForeColor = System.Drawing.Color.Black
        Me.grpUSeal.Location = New System.Drawing.Point(12, 116)
        Me.grpUSeal.Name = "grpUSeal"
        Me.grpUSeal.Size = New System.Drawing.Size(476, 68)
        Me.grpUSeal.TabIndex = 60
        Me.grpUSeal.TabStop = False
        Me.grpUSeal.Text = "U-Seal:"
        '
        'chkDT_USeal
        '
        Me.chkDT_USeal.AutoSize = True
        Me.chkDT_USeal.Checked = True
        Me.chkDT_USeal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDT_USeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDT_USeal.Location = New System.Drawing.Point(10, 46)
        Me.chkDT_USeal.Name = "chkDT_USeal"
        Me.chkDT_USeal.Size = New System.Drawing.Size(42, 17)
        Me.chkDT_USeal.TabIndex = 60
        Me.chkDT_USeal.Text = "DT"
        '
        'chkDLLeg
        '
        Me.chkDLLeg.AutoSize = True
        Me.chkDLLeg.Checked = True
        Me.chkDLLeg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDLLeg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDLLeg.Location = New System.Drawing.Point(409, 23)
        Me.chkDLLeg.Name = "chkDLLeg"
        Me.chkDLLeg.Size = New System.Drawing.Size(61, 17)
        Me.chkDLLeg.TabIndex = 59
        Me.chkDLLeg.Text = "DLLeg"
        '
        'chkDRad2
        '
        Me.chkDRad2.AutoSize = True
        Me.chkDRad2.Checked = True
        Me.chkDRad2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDRad2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDRad2.Location = New System.Drawing.Point(327, 23)
        Me.chkDRad2.Name = "chkDRad2"
        Me.chkDRad2.Size = New System.Drawing.Size(64, 17)
        Me.chkDRad2.TabIndex = 58
        Me.chkDRad2.Text = "DRad2"
        '
        'chkDRad1
        '
        Me.chkDRad1.AutoSize = True
        Me.chkDRad1.Checked = True
        Me.chkDRad1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDRad1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDRad1.Location = New System.Drawing.Point(227, 23)
        Me.chkDRad1.Name = "chkDRad1"
        Me.chkDRad1.Size = New System.Drawing.Size(64, 17)
        Me.chkDRad1.TabIndex = 57
        Me.chkDRad1.Text = "DRad1"
        '
        'chkDTheta1
        '
        Me.chkDTheta1.AutoSize = True
        Me.chkDTheta1.Checked = True
        Me.chkDTheta1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDTheta1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDTheta1.Location = New System.Drawing.Point(10, 23)
        Me.chkDTheta1.Name = "chkDTheta1"
        Me.chkDTheta1.Size = New System.Drawing.Size(74, 17)
        Me.chkDTheta1.TabIndex = 56
        Me.chkDTheta1.Text = "DTheta1"
        '
        'chkDTheta2
        '
        Me.chkDTheta2.AutoSize = True
        Me.chkDTheta2.Checked = True
        Me.chkDTheta2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDTheta2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDTheta2.Location = New System.Drawing.Point(104, 23)
        Me.chkDTheta2.Name = "chkDTheta2"
        Me.chkDTheta2.Size = New System.Drawing.Size(74, 17)
        Me.chkDTheta2.TabIndex = 55
        Me.chkDTheta2.Text = "DTheta2"
        '
        'grpCSeal
        '
        Me.grpCSeal.Controls.Add(Me.chkDT_CSeal)
        Me.grpCSeal.Controls.Add(Me.chkDHFree)
        Me.grpCSeal.Controls.Add(Me.chkDThetaOpening)
        Me.grpCSeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCSeal.ForeColor = System.Drawing.Color.Black
        Me.grpCSeal.Location = New System.Drawing.Point(12, 68)
        Me.grpCSeal.Name = "grpCSeal"
        Me.grpCSeal.Size = New System.Drawing.Size(474, 44)
        Me.grpCSeal.TabIndex = 59
        Me.grpCSeal.TabStop = False
        Me.grpCSeal.Text = "C-Seal:"
        '
        'chkDT_CSeal
        '
        Me.chkDT_CSeal.AutoSize = True
        Me.chkDT_CSeal.Checked = True
        Me.chkDT_CSeal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDT_CSeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDT_CSeal.Location = New System.Drawing.Point(227, 20)
        Me.chkDT_CSeal.Name = "chkDT_CSeal"
        Me.chkDT_CSeal.Size = New System.Drawing.Size(42, 17)
        Me.chkDT_CSeal.TabIndex = 57
        Me.chkDT_CSeal.Text = "DT"
        '
        'chkDHFree
        '
        Me.chkDHFree.AutoSize = True
        Me.chkDHFree.Checked = True
        Me.chkDHFree.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDHFree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDHFree.Location = New System.Drawing.Point(10, 20)
        Me.chkDHFree.Name = "chkDHFree"
        Me.chkDHFree.Size = New System.Drawing.Size(68, 17)
        Me.chkDHFree.TabIndex = 56
        Me.chkDHFree.Text = "DHFree"
        '
        'chkDThetaOpening
        '
        Me.chkDThetaOpening.AutoSize = True
        Me.chkDThetaOpening.Checked = True
        Me.chkDThetaOpening.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDThetaOpening.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDThetaOpening.Location = New System.Drawing.Point(104, 20)
        Me.chkDThetaOpening.Name = "chkDThetaOpening"
        Me.chkDThetaOpening.Size = New System.Drawing.Size(114, 17)
        Me.chkDThetaOpening.TabIndex = 55
        Me.chkDThetaOpening.Text = "DThetaOpening"
        '
        'grpESeal
        '
        Me.grpESeal.Controls.Add(Me.chkDThetaE1)
        Me.grpESeal.Controls.Add(Me.chkDThetaM1)
        Me.grpESeal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpESeal.ForeColor = System.Drawing.Color.Black
        Me.grpESeal.Location = New System.Drawing.Point(12, 20)
        Me.grpESeal.Name = "grpESeal"
        Me.grpESeal.Size = New System.Drawing.Size(474, 44)
        Me.grpESeal.TabIndex = 58
        Me.grpESeal.TabStop = False
        Me.grpESeal.Text = "E-Seal:"
        '
        'chkDThetaE1
        '
        Me.chkDThetaE1.AutoSize = True
        Me.chkDThetaE1.Checked = True
        Me.chkDThetaE1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDThetaE1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDThetaE1.Location = New System.Drawing.Point(14, 21)
        Me.chkDThetaE1.Name = "chkDThetaE1"
        Me.chkDThetaE1.Size = New System.Drawing.Size(81, 17)
        Me.chkDThetaE1.TabIndex = 56
        Me.chkDThetaE1.Text = "DThetaE1"
        '
        'chkDThetaM1
        '
        Me.chkDThetaM1.AutoSize = True
        Me.chkDThetaM1.Checked = True
        Me.chkDThetaM1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDThetaM1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDThetaM1.Location = New System.Drawing.Point(106, 20)
        Me.chkDThetaM1.Name = "chkDThetaM1"
        Me.chkDThetaM1.Size = New System.Drawing.Size(83, 17)
        Me.chkDThetaM1.TabIndex = 55
        Me.chkDThetaM1.Text = "DThetaM1"
        '
        'chkAdjusted
        '
        Me.chkAdjusted.AutoSize = True
        Me.chkAdjusted.Checked = True
        Me.chkAdjusted.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAdjusted.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAdjusted.Location = New System.Drawing.Point(12, 1)
        Me.chkAdjusted.Name = "chkAdjusted"
        Me.chkAdjusted.Size = New System.Drawing.Size(15, 14)
        Me.chkAdjusted.TabIndex = 35
        '
        'chkZClear
        '
        Me.chkZClear.AutoSize = True
        Me.chkZClear.Checked = True
        Me.chkZClear.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkZClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkZClear.Location = New System.Drawing.Point(11, 68)
        Me.chkZClear.Name = "chkZClear"
        Me.chkZClear.Size = New System.Drawing.Size(65, 17)
        Me.chkZClear.TabIndex = 55
        Me.chkZClear.Text = "ZClear"
        '
        'chkSFinish
        '
        Me.chkSFinish.AutoSize = True
        Me.chkSFinish.Checked = True
        Me.chkSFinish.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSFinish.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSFinish.Location = New System.Drawing.Point(350, 44)
        Me.chkSFinish.Name = "chkSFinish"
        Me.chkSFinish.Size = New System.Drawing.Size(66, 17)
        Me.chkSFinish.TabIndex = 53
        Me.chkSFinish.Text = "SFinish"
        '
        'chkSegCount
        '
        Me.chkSegCount.AutoSize = True
        Me.chkSegCount.Checked = True
        Me.chkSegCount.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSegCount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSegCount.Location = New System.Drawing.Point(121, 20)
        Me.chkSegCount.Name = "chkSegCount"
        Me.chkSegCount.Size = New System.Drawing.Size(129, 17)
        Me.chkSegCount.TabIndex = 48
        Me.chkSegCount.Text = "Segmented Count"
        '
        'chkCoating
        '
        Me.chkCoating.AutoSize = True
        Me.chkCoating.Checked = True
        Me.chkCoating.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCoating.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCoating.Location = New System.Drawing.Point(264, 44)
        Me.chkCoating.Name = "chkCoating"
        Me.chkCoating.Size = New System.Drawing.Size(70, 17)
        Me.chkCoating.TabIndex = 43
        Me.chkCoating.Text = "Coating"
        '
        'chkHT
        '
        Me.chkHT.AutoSize = True
        Me.chkHT.Checked = True
        Me.chkHT.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHT.Location = New System.Drawing.Point(121, 44)
        Me.chkHT.Name = "chkHT"
        Me.chkHT.Size = New System.Drawing.Size(115, 17)
        Me.chkHT.TabIndex = 35
        Me.chkHT.Text = "Heat Treatment"
        '
        'chkPlating
        '
        Me.chkPlating.AutoSize = True
        Me.chkPlating.Checked = True
        Me.chkPlating.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPlating.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPlating.Location = New System.Drawing.Point(429, 44)
        Me.chkPlating.Name = "chkPlating"
        Me.chkPlating.Size = New System.Drawing.Size(64, 17)
        Me.chkPlating.TabIndex = 54
        Me.chkPlating.Text = "Plating"
        '
        'chkSeg
        '
        Me.chkSeg.AutoSize = True
        Me.chkSeg.Checked = True
        Me.chkSeg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSeg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeg.Location = New System.Drawing.Point(10, 20)
        Me.chkSeg.Name = "chkSeg"
        Me.chkSeg.Size = New System.Drawing.Size(91, 17)
        Me.chkSeg.TabIndex = 33
        Me.chkSeg.Text = "Segmented"
        '
        'chkMatName
        '
        Me.chkMatName.AutoSize = True
        Me.chkMatName.Checked = True
        Me.chkMatName.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMatName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMatName.Location = New System.Drawing.Point(10, 44)
        Me.chkMatName.Name = "chkMatName"
        Me.chkMatName.Size = New System.Drawing.Size(108, 17)
        Me.chkMatName.TabIndex = 38
        Me.chkMatName.Text = "Material Name"
        '
        'IPE_frmCustomResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(729, 482)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "IPE_frmCustomResults"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Result Form Layout Customization"
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpAppliedLoading.ResumeLayout(False)
        Me.grpAppliedLoading.PerformLayout()
        Me.grpAnalysis.ResumeLayout(False)
        Me.grpAnalysis.PerformLayout()
        Me.grpCavirtyDim.ResumeLayout(False)
        Me.grpCavirtyDim.PerformLayout()
        Me.grpOpCond.ResumeLayout(False)
        Me.grpOpCond.PerformLayout()
        Me.grpSeal.ResumeLayout(False)
        Me.grpSeal.PerformLayout()
        Me.grpGroupBox3.ResumeLayout(False)
        Me.grpGroupBox3.PerformLayout()
        Me.grpUSeal.ResumeLayout(False)
        Me.grpUSeal.PerformLayout()
        Me.grpCSeal.ResumeLayout(False)
        Me.grpCSeal.PerformLayout()
        Me.grpESeal.ResumeLayout(False)
        Me.grpESeal.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents grpSeal As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeg As System.Windows.Forms.CheckBox
    Friend WithEvents chkHFree As System.Windows.Forms.CheckBox
    Friend WithEvents chkMatName As System.Windows.Forms.CheckBox
    Friend WithEvents chkHT As System.Windows.Forms.CheckBox
    Friend WithEvents grpOpCond As System.Windows.Forms.GroupBox
    Friend WithEvents chkPOrient As System.Windows.Forms.CheckBox
    Friend WithEvents chkPress As System.Windows.Forms.CheckBox
    Friend WithEvents grpCavirtyDim As System.Windows.Forms.GroupBox
    Friend WithEvents chkCavityID As System.Windows.Forms.CheckBox
    Friend WithEvents chkCavityOD As System.Windows.Forms.CheckBox
    Friend WithEvents chkCavityDepth As System.Windows.Forms.CheckBox
    Friend WithEvents chkSpringBack As System.Windows.Forms.CheckBox
    Friend WithEvents chkTimeCreated As System.Windows.Forms.CheckBox
    Friend WithEvents chkDateCreated As System.Windows.Forms.CheckBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents chkTOper As System.Windows.Forms.CheckBox
    Friend WithEvents chkCoating As System.Windows.Forms.CheckBox
    Friend WithEvents chkSegCount As System.Windows.Forms.CheckBox
    Friend WithEvents chkLeakage_BL As System.Windows.Forms.CheckBox
    Friend WithEvents chkPlating As System.Windows.Forms.CheckBox
    Friend WithEvents chkSFinish As System.Windows.Forms.CheckBox
    Friend WithEvents grpAppliedLoading As System.Windows.Forms.GroupBox
    Friend WithEvents chkPreCompressedH As System.Windows.Forms.CheckBox
    Friend WithEvents chkRadCon As System.Windows.Forms.CheckBox
    Friend WithEvents chkPreCompressed As System.Windows.Forms.CheckBox
    Friend WithEvents grpAnalysis As System.Windows.Forms.GroupBox
    Friend WithEvents chkName As System.Windows.Forms.CheckBox
    Friend WithEvents chkMCS As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdjusted As System.Windows.Forms.CheckBox
    Friend WithEvents chkZClear As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLoadCaseType As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompTolType As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents grpGroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents grpUSeal As System.Windows.Forms.GroupBox
    Friend WithEvents chkDLLeg As System.Windows.Forms.CheckBox
    Friend WithEvents chkDRad2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkDRad1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkDTheta1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkDTheta2 As System.Windows.Forms.CheckBox
    Friend WithEvents grpCSeal As System.Windows.Forms.GroupBox
    Friend WithEvents chkDT_CSeal As System.Windows.Forms.CheckBox
    Friend WithEvents chkDHFree As System.Windows.Forms.CheckBox
    Friend WithEvents chkDThetaOpening As System.Windows.Forms.CheckBox
    Friend WithEvents grpESeal As System.Windows.Forms.GroupBox
    Friend WithEvents chkDThetaE1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkDThetaM1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkDT_USeal As System.Windows.Forms.CheckBox
    Friend WithEvents chkUnitSeatingLoad As System.Windows.Forms.CheckBox
    Friend WithEvents chkMatModel As System.Windows.Forms.CheckBox
End Class
