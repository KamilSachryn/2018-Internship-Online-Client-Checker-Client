Imports System.Xml
Public Class XMLController

    Shared settingsFileLocation As String = "ClientSettings.xml"

    'Dim xmlReader As XmlReader '= XmlReader.Create("ClientList.xml")
    Shared ClientListXmlDoc As New XmlDocument()
    Shared SettingsXmlDoc As New XmlDocument()
    Sub New()

        If System.IO.File.Exists(settingsFileLocation) Then
            Console.WriteLine("Settings FILE EXISTS")
        Else
            Console.WriteLine("Settings FILE DOES NOT EXIST")
            ' Create XmlWriterSettings.
            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.Indent = True

            ' Create XmlWriter.
            Using writer As XmlWriter = XmlWriter.Create(settingsFileLocation, settings)
                ' Begin writing.
                writer.WriteStartDocument()
                writer.WriteStartElement("Settings") ' Root.


                ' End document.
                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using
            Console.WriteLine("FILE CREATED")
        End If

        SettingsXmlDoc.Load(settingsFileLocation)



    End Sub




    Public Shared Function loadSettings() As List(Of String)
        Dim list As New List(Of String)



        Try
            Dim doc As XmlDocument
            Dim nodeList As XmlNodeList
            Dim currentNode As XmlNode
            'Create the XML Document
            doc = New XmlDocument()
            'Load the Xml file
            doc.Load(settingsFileLocation)
            'Get the list of name nodes 
            nodeList = doc.SelectNodes("/Settings/Setting")
            'Loop through the nodes
            For Each currentNode In nodeList
                list.Add(currentNode.ChildNodes(0).InnerText)
                list.Add(currentNode.ChildNodes(1).InnerText)

                Return list
            Next
        Catch errorVariable As Exception
            'Error trapping
            Console.Write(errorVariable.ToString())
        End Try

        If list.Count = 0 Then
            list.AddRange({"", "30"})
        End If

        Return list

    End Function

    Public Shared Sub saveSettings(list As List(Of String))

        Dim doc As XDocument = XDocument.Load(settingsFileLocation)




        Dim node As XElement = doc.Descendants("Setting").FirstOrDefault()


        'If the current connection does not exist, create it, otherwise skip if
        If node Is Nothing Then
            Dim currEle = New XElement("Setting")
            doc.Element("Settings").Add(currEle)
            node = currEle
        End If

        'Console.WriteLine("START SETTING OR CHANGING NEW CONNECTION ___")
        'Console.WriteLine(connection.ToString())
        'Console.WriteLine("END   SETTING OR CHANGING NEW CONNECTION ___")


        'updated current connection settings
        node.SetElementValue("IP", list(0))
        node.SetElementValue("RefreshRate", list(1))


        doc.Save(settingsFileLocation)

    End Sub


End Class
