<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IPE_frmNomenclature_DesignCenter
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.imgControl = New System.Windows.Forms.PictureBox()
        CType(Me.imgControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'imgControl
        '
        Me.imgControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.imgControl.BackColor = System.Drawing.Color.White
        Me.imgControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.imgControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.imgControl.Location = New System.Drawing.Point(3, 3)
        Me.imgControl.Name = "imgControl"
        Me.imgControl.Size = New System.Drawing.Size(453, 273)
        Me.imgControl.TabIndex = 0
        Me.imgControl.TabStop = False
        '
        'IPE_frmNomenclature_DesignCenter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(459, 278)
        Me.Controls.Add(Me.imgControl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "IPE_frmNomenclature_DesignCenter"
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Nomenclature - Design Center"
        Me.TopMost = True
        CType(Me.imgControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents imgControl As System.Windows.Forms.PictureBox
End Class
