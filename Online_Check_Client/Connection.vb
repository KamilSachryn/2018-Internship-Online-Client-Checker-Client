Imports System.IO
Imports System.Net.Sockets

Public Class Connection
    Dim serverNotFoundMessage As String = "Server not found"
    Dim conIP As String
    Dim conPort As Integer = 20

    Dim connected As Boolean = False
    Sub New(ip As String)
        conIP = ip


        Try
            '  Console.Write("Enter IP to connect to: ")
            '   Dim conIP As String = Console.ReadLine()
            '   Console.WriteLine()
            '    Console.Write("Enter port to connect to: ")
            '   Dim conPort As Integer = Integer.Parse(Console.ReadLine())
            '  Console.WriteLine()
            Dim client As TcpClient = New TcpClient(conIP, conPort)
            Console.WriteLine("Trying to connect to " & conIP & ":" & conPort)

            Try
                Dim stream As Stream = client.GetStream()
                Console.WriteLine("Created stream")
                Dim streamReader As StreamReader = New StreamReader(stream)
                Console.WriteLine("Created stream reader")
                Dim streamWriter As StreamWriter = New StreamWriter(stream)
                Console.WriteLine("Created stream writer")
                streamWriter.AutoFlush = True
                Console.Write("Reading message from SERVER: " & streamReader.ReadLine())
                Console.WriteLine()
                streamWriter.WriteLine("message from CLIENT")
                Console.WriteLine("closing stream")
                stream.Close()
                connected = True
            Finally
                Console.WriteLine("closing client")
                client.Close()
            End Try

            Console.WriteLine("")
            Console.WriteLine("All connections closed, Enter to turn off console")

            ' Console.Read()
        Catch ex As Exception
            connected = False
        End Try

    End Sub

    Public Function getConnected() As Boolean
        Return connected
    End Function
End Class
