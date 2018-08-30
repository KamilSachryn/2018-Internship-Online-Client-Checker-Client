Imports System.Xml
Public Class XMLController

    'File name of XML settings file
    Shared settingsFileLocation As String = "ClientSettings.xml"

    Shared SettingsXmlDoc As New XmlDocument()

    'On load, check if XML file exists, if it does, load it, otherwise create a new one with default values, then load it
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



    'Loads settings from XML file to the program's memory
    'Settings are saved as a list of strings
    'TODO: make them a Dictionary instead
    'Node 0 is the IP
    'Node 1 is the Refresh Rate in seconds
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


    'save settings from list of strings to XML file
    'Settings are saved as a list of strings
    'TODO: make them a Dictionary instead
    'Node 0 is the IP
    'Node 1 is the Refresh Rate in seconds
    Public Shared Sub saveSettings(list As List(Of String))

        Dim doc As XDocument = XDocument.Load(settingsFileLocation)

        Dim node As XElement = doc.Descendants("Setting").FirstOrDefault()


        'If the current connection does not exist, create it, otherwise skip if
        If node Is Nothing Then
            Dim currEle = New XElement("Setting")
            doc.Element("Settings").Add(currEle)
            node = currEle
        End If


        'updated current connection settings
        node.SetElementValue("IP", list(0))
        node.SetElementValue("RefreshRate", list(1))


        doc.Save(settingsFileLocation)

    End Sub


End Class
