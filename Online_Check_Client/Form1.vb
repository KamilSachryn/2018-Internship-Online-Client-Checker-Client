Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Timers

Public Class Form1

    'Message recived from the Server program, once a message is recieved it checks what it is vs. this string variable
    Dim RefreshMessage As String = "Refresh"

    'Accepts connections from the server for use in Refreshing
    Dim listener As TcpListener

    'Refresh every timerRickTime miliseconds
    Dim timer As Timers.Timer
    Dim timerTickTime As Integer = 30000

    'XML file to save the server's IP
    Dim xmlController As New XMLController


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Create a new timer and bind it to the TimerTick function
        timer = New Timers.Timer(timerTickTime)
        AddHandler timer.Elapsed, New ElapsedEventHandler(AddressOf TimerTick)
        timer.Enabled = True

        'Load settings from the XML file
        'This could be improved, right now changing the order of strings in the code makes it bug out
        Dim list As List(Of String) = XMLController.loadSettings()

        'Set the ip textbox to be the IP from the XML file
        tb_ip.Text = list(0)

        'Listen to the server so that we can get any messages/actions
        StartListener()

        'Create a connection to the Server
        InitConnection()


    End Sub

    'Standard TCP listener connection
    'Creates a thread to listen on then goes to the Service function once created
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
    'Will accept a message from the server (i.e. REFRESH) and then do an action depending on that message
    Public Sub Service()

        'Loop infinitely to constantly scan for incomming packets
        While True
            'create socket, parse IP to be the correct format (1.2.3.4 instead of 1.2.3.4:1234)
            Dim socket As Socket = listener.AcceptSocket()
            Dim ip As String = socket.RemoteEndPoint.ToString()
            ip = ip.Substring(0, ip.LastIndexOf(":"))

            'Listen to the stream to grab the message
            Try
                Dim networkStream As NetworkStream = New NetworkStream(socket)

                Dim streamReader As StreamReader = New StreamReader(networkStream)

                'read the message and then run the ReadMessageFromServer function to do the correct action
                Dim message As String = streamReader.ReadLine()
                ReadMessageFromServer(message)

                networkStream.Close()
            Catch e As Exception
                Console.WriteLine("EXCEPTION: " & e.Message)
            End Try

            socket.Close()
        End While
    End Sub

    'Reads a message and the preforms an action based on it
    'Add new actions here (i.e. a shutdown message to turn off the client)
    Sub ReadMessageFromServer(message As String)
        'If the message is the RefreshMessage, create a new connection to the server
        If message = RefreshMessage Then
            InitConnection()
        End If
    End Sub

    'if cannot connect to server, display error
    'TODO: make it only pop up once
    Public Sub TimerTick(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        If InitConnection() = False Then
            MsgBox("Failed to connect to server", MsgBoxStyle.Exclamation)
        Else

        End If
    End Sub

    'Create a new connection to the server and return success or failure
    Public Function InitConnection() As Boolean

        'if error on ip textbox, return flase
        'if connection failed, return false
        If ep_ip.GetError(tb_ip) = "" Then
            Dim con As New Connection(tb_ip.Text)

            Return con.getConnected()
        Else
            Return False
        End If

    End Function

    'Gets ip of local machine for use with TCP listener
    Public Function GetLocalIP() As IPAddress
        Dim host = Dns.GetHostEntry(Dns.GetHostName())

        For Each ip In host.AddressList

            If ip.AddressFamily = AddressFamily.InterNetwork Then
                Return ip
            End If
        Next

        Throw New Exception("No network adapters with an IPv4 address in the system!")
    End Function

    'Maximise window on clicking system tray icon
    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub



    'On minimize, put it in system tray instead
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If FormWindowState.Minimized = Me.WindowState Then

            NotifyIcon1.Visible = True
            ' NotifyIcon1.ShowBalloonTip(500)
            Me.Hide()


        ElseIf FormWindowState.Normal = Me.WindowState Then

            NotifyIcon1.Visible = False

        End If

    End Sub


    'On clicking connect, create connection
    'ip error checking handled in initconnection
    Private Sub btn_Send_Click(sender As Object, e As EventArgs)

        InitConnection()

    End Sub

    'On changing refreshrate, error check and upate it automatically
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

    'on changing ip, error check and update it automatically
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

    'On clicking save, save all data to XML file
    Private Sub btn_Save_Click(sender As Object, e As EventArgs) Handles btn_Save.Click
        'if IP is incorrect, set interval to infinite (Easier than turning the timer on and off
        'and makes sure no expections are thrown in other threads)
        If ep_ip.GetError(tb_ip) IsNot "" Then
            MsgBox("Invalid IP entered")
            timer.Interval = Int64.MaxValue
        Else
            'if connection is valid (server MUST be online), save settings to the XML
            If InitConnection() = True Then
                MsgBox("Connection established and saved", MsgBoxStyle.OkOnly)
                timer.Interval = timerTickTime

                'create a list of the settings to save
                Dim list As New List(Of String)
                list.Add(tb_ip.Text)
                list.Add(timerTickTime.ToString())
                XMLController.saveSettings(list)
            Else
                MsgBox("Failed to connect to server", MsgBoxStyle.Exclamation)
                timer.Interval = Int64.MaxValue
            End If
        End If

    End Sub

    'Correctly quit app on clicking X
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Application.Exit()
        End
    End Sub

    'correct quit app on clicking EXIT in system tray
    Private Sub tsmi_close_Click(sender As Object, e As EventArgs) Handles tsmi_close.Click
        Application.Exit()
        End
    End Sub

    'Show app on clicking OPEN in system tray
    Private Sub tsmi_Open_Click(sender As Object, e As EventArgs) Handles tsmi_Open.Click
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub
End Class
