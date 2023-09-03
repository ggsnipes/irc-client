Public Class Form1
    Inherits System.Windows.Forms.Form

    Private WithEvents wsClient As New SocketsClient()

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(12, 12)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(412, 307)
        Me.TextBox1.TabIndex = 0
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(12, 328)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(344, 20)
        Me.TextBox2.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(368, 325)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(56, 24)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Send"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(437, 352)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region


    Private Sub AT(ByVal Text As String)
        TextBox1.AppendText(Text & vbCrLf)
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AT("Attempting connection to host...")
        wsClient.Connect("irc.rizon.net", 6667)
    End Sub

    Private Sub wsClient_onConnect() Handles wsClient.onConnect
        Dim a() As Byte

        AT("Connected to server.")

        wsClient.SendData(wsClient.StringToBytes("PRIVMSG #mb-au hi im bob" & vbCr))
        wsClient.SendData(wsClient.StringToBytes("JOIN #mb-au" & vbCrLf))
        wsClient.SendData(wsClient.StringToBytes("NICK ggtlk" & vbCrLf))
        wsClient.SendData(wsClient.StringToBytes("USER aaa localhost 0.0.0.0 :I am coded in VB.NET" & vbCrLf))
    End Sub

    Private Sub wsClient_onError(ByVal Description As String) Handles wsClient.onError
        AT("Error: " & Description)
    End Sub

    Private Sub wsClient_onDataArrival(ByVal Data() As Byte, ByVal totBytes As Integer) Handles wsClient.onDataArrival
        Dim inData As String = wsClient.BytestoString(Data)

        Dim lData() As String
        Dim C() As String
        Dim tData As Integer
        Dim CL As Integer
        Dim CLi As String
        Dim a As Integer

        lData = Split(inData, Chr(10))
        tData = UBound(lData) - LBound(lData)

        For a = 0 To tData
            If Mid(lData(a), 1, 1) = ":" Then
                lData(a) = Mid(lData(a), 2)
            End If
        Next

        For CL = 0 To tData
            C = Split(lData(CL), " ")
            ReDim Preserve C(2000)
            CLi = lData(CL)

            If C(0) = "PING" Then
                wsClient.SendData(wsClient.StringToBytes("PONG :" & Mid(C(1), 2) & vbCrLf))
            End If

            AT(CLi)
        Next
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            wsClient.SendData(wsClient.StringToBytes(TextBox2.Text & vbCrLf))
            TextBox2.SelectAll()
        End If
    End Sub
End Class
