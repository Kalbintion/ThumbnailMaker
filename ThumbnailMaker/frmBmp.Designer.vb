<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBmp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBmp))
        Me.pbImg = New System.Windows.Forms.PictureBox()
        Me.lblEp = New System.Windows.Forms.Label()
        CType(Me.pbImg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbImg
        '
        Me.pbImg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbImg.Image = CType(resources.GetObject("pbImg.Image"), System.Drawing.Image)
        Me.pbImg.Location = New System.Drawing.Point(0, 0)
        Me.pbImg.Name = "pbImg"
        Me.pbImg.Size = New System.Drawing.Size(1280, 720)
        Me.pbImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbImg.TabIndex = 0
        Me.pbImg.TabStop = False
        '
        'lblEp
        '
        Me.lblEp.BackColor = System.Drawing.Color.Transparent
        Me.lblEp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblEp.Font = New System.Drawing.Font("Arial Narrow", 195.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEp.ForeColor = System.Drawing.Color.White
        Me.lblEp.Location = New System.Drawing.Point(0, 0)
        Me.lblEp.Name = "lblEp"
        Me.lblEp.Size = New System.Drawing.Size(1280, 720)
        Me.lblEp.TabIndex = 1
        Me.lblEp.Text = "1"
        Me.lblEp.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'frmBmp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.lblEp)
        Me.Controls.Add(Me.pbImg)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmBmp"
        Me.Text = "Form1"
        CType(Me.pbImg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbImg As System.Windows.Forms.PictureBox
    Friend WithEvents lblEp As System.Windows.Forms.Label

End Class
