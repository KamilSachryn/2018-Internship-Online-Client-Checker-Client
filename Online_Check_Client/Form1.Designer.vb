<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tb_ip = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ep_RefreshRate = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ep_ip = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.btn_Save = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmi_Open = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmi_close = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.ep_RefreshRate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ep_ip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.NotifyIcon1.BalloonTipText = "a"
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "NotifyIcon1"
        '
        'tb_ip
        '
        Me.tb_ip.Location = New System.Drawing.Point(69, 22)
        Me.tb_ip.Name = "tb_ip"
        Me.tb_ip.Size = New System.Drawing.Size(100, 20)
        Me.tb_ip.TabIndex = 0
        Me.tb_ip.Text = "192.168.1.9"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Server IP"
        '
        'ep_RefreshRate
        '
        Me.ep_RefreshRate.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ep_RefreshRate.ContainerControl = Me
        '
        'ep_ip
        '
        Me.ep_ip.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ep_ip.ContainerControl = Me
        '
        'btn_Save
        '
        Me.btn_Save.Location = New System.Drawing.Point(79, 48)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(75, 23)
        Me.btn_Save.TabIndex = 3
        Me.btn_Save.Text = "Save"
        Me.btn_Save.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmi_Open, Me.tsmi_close})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowImageMargin = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(156, 70)
        '
        'tsmi_Open
        '
        Me.tsmi_Open.Name = "tsmi_Open"
        Me.tsmi_Open.Size = New System.Drawing.Size(155, 22)
        Me.tsmi_Open.Text = "Open"
        '
        'tsmi_close
        '
        Me.tsmi_close.Name = "tsmi_close"
        Me.tsmi_close.Size = New System.Drawing.Size(155, 22)
        Me.tsmi_close.Text = "Exit"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(224, 87)
        Me.Controls.Add(Me.btn_Save)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tb_ip)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.ep_RefreshRate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ep_ip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents tb_ip As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ep_RefreshRate As ErrorProvider
    Friend WithEvents ep_ip As ErrorProvider
    Friend WithEvents btn_Save As Button
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents tsmi_Open As ToolStripMenuItem
    Friend WithEvents tsmi_close As ToolStripMenuItem
End Class
