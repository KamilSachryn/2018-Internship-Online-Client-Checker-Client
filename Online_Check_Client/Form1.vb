Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Timers

Public Class Form1

    Dim RefreshMessage As String = "Refresh"

    Dim listener As TcpListener

    Dim timer As Timers.Timer
    Dim timerTickTime As Integer = 30000

    Dim xmlController As New XMLController


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        timer = New Timers.Timer(timerTickTime)
        AddHandler timer.Elapsed, New ElapsedEventHandler(AddressOf TimerTick)
        timer.Enabled = True
        If FormWindowState.Minimized = Me.WindowState Then

            NotifyIcon1.Visible = True
            NotifyIcon1.ShowBalloonTip(500)
            Me.Hide()


        ElseIf FormWindowState.Normal = Me.WindowState Then

            NotifyIcon1.Visible = False

        End If

        'Dim con As New Connection("192.168.1.9")


        StartListener()
        InitConnection()


    End Sub


    Public Sub StartListener()

        Dim localip As IPAddress = GetLocalIP()
        Dim localPort = 21

        Console.Write("Trying to create listener on ip " & localip.ToString() & " , port " & localPort & " ...")
        listener = New TcpListener(localip, localPort)
        listener.Start()
        Console.WriteLine("Listener created successfully")
        Console.WriteLine()
        Console.WriteLine("LISTENING ON IP: " & localip.ToString())
        Console.WriteLine()
        Console.Write("Trying to create thread(s)...")

        'using multiple threads for multiple connections at once
        Dim thread = New Thread(Sub() Service())
        thread.Start()
        Console.WriteLine("Threads Started")

    End Sub

    'Ran on each thread, listens for an incomming connection
    Public Sub Service()

        'Loop infinitely to constantly scan for incomming packets
        While True
            Dim socket As Socket = listener.AcceptSocket()
            Dim ip As String = socket.RemoteEndPoint.ToString()
            ip = ip.Substring(0, ip.LastIndexOf(":"))



            '''Console.WriteLine("Connected " & ip & "on thread ID " & id)
            '''Dim currCon As New Connection(IPAddress.Parse(ip), "Unnamed", Connection.ConnectionStatus.Online, True.ToString(), DateTime.Now)
            ''''addConToViewAndXML(currCon)
            '''XMLController.AddItemToClientListXML(currCon)
            '''UpdateViewFromXML()




            Try
                Dim networkStream As NetworkStream = New NetworkStream(socket)
                'Dim streamWriter As StreamWriter = New StreamWriter(networkStream)
                ' streamWriter.AutoFlush = True
                ' streamWriter.WriteLine("Message from SERVER")

                Dim streamReader As StreamReader = New StreamReader(networkStream)

                Dim message As String = streamReader.ReadLine()
                ' Console.WriteLine("Message from connected client: " & streamReader.ReadLine())
                ReadMessageFromServer(message)

                networkStream.Close()
            Catch e As Exception
                Console.WriteLine("EXCEPTION: " & e.Message)
            End Try

            socket.Close()
        End While
    End Sub

    Sub ReadMessageFromServer(message As String)
        If message = RefreshMessage Then
            InitConnection()
        End If
    End Sub

    Public Sub TimerTick(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        If InitConnection() = False Then
            MsgBox("Failed to connect to server", MsgBoxStyle.Exclamation)
        Else

        End If
    End Sub

    Public Function InitConnection() As Boolean

        If ep_ip.GetError(tb_ip) = "" Then
            Dim con As New Connection(tb_ip.Text)

            Return con.getConnected()
        Else
            Return False
        End If

    End Function


    Public Function GetLocalIP() As IPAddress
        Dim host = Dns.GetHostEntry(Dns.GetHostName())

        For Each ip In host.AddressList

            If ip.AddressFamily = AddressFamily.InterNetwork Then
                Return ip
            End If
        Next

        Throw New Exception("No network adapters with an IPv4 address in the system!")
    End Function
    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub


    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick

    End Sub



    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If FormWindowState.Minimized = Me.WindowState Then

            NotifyIcon1.Visible = True
            ' NotifyIcon1.ShowBalloonTip(500)
            Me.Hide()


        ElseIf FormWindowState.Normal = Me.WindowState Then

            NotifyIcon1.Visible = False

        End If

    End Sub



    Private Sub btn_Send_Click(sender As Object, e As EventArgs)

        InitConnection()

    End Sub

    Private Sub tb_RefreshRate_TextChanged(sender As Object, e As EventArgs)
        Dim tb As TextBox = sender
        Dim regexString = "^[0-9]*$" 'numbers only
        ' if lower and higher ip matches regex
        If Not Regex.IsMatch(tb.Text, regexString) Then
            ep_RefreshRate.SetError(tb, "Must be a number")
        Else
            ep_RefreshRate.SetError(tb, "")
        End If



    End Sub

    Private Sub tb_ip_TextChanged(sender As Object, e As EventArgs) Handles tb_ip.TextChanged
        Dim tb As TextBox = sender
        Dim regexString = "^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" 'ip only
        ' if lower and higher ip matches regex
        If Not Regex.IsMatch(tb.Text, regexString) Then
            ep_ip.SetError(tb, "Must be a valid IP address")
        Else
            '     timer.Interval = tb_RefreshRate.Text
            ep_ip.SetError(tb, "")
        End If


    End Sub

    Private Sub btn_Save_Click(sender As Object, e As EventArgs) Handles btn_Save.Click
        If ep_ip.GetError(tb_ip) IsNot "" Then
            MsgBox("Invalid IP entered")
            timer.Interval = Int64.MaxValue
        Else
            If InitConnection() = True Then
                MsgBox("Connection established and saved", MsgBoxStyle.OkOnly)
                timer.Interval = timerTickTime
            Else
                MsgBox("Failed to connect to server", MsgBoxStyle.Exclamation)
                timer.Interval = Int64.MaxValue
            End If
        End If

    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Application.Exit()
        End
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub tsmi_close_Click(sender As Object, e As EventArgs) Handles tsmi_close.Click
        Application.Exit()
        End
    End Sub

    Private Sub tsmi_Open_Click(sender As Object, e As EventArgs) Handles tsmi_Open.Click
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub
End Class
