Module FileAssoc
    <System.Runtime.InteropServices.DllImport("shell32.dll")> _
    Sub SHChangeNotify(ByVal wEventId As Integer, ByVal uFlags As Integer, ByVal dwItem1 As Integer, ByVal dwItem2 As Integer)

    End Sub

    Private Const SHCNE_ASSOCCHANGED = &H8000000
    Private Const SHCNF_IDLIST = 0

    ' Extension is the extension to be registered (eg ".cad"
    ' ClassName is the name of the associated class (eg "CADDoc")
    ' Description is the textual description (eg "CAD Document")
    ' ExeProgram is the app that manages that extension (eg "c:\Cad\MyCad.exe")

    Public Function CreateAssoc(ByVal extension As String, ByVal className As String, ByVal description As String, ByVal exeProgram As String) As Boolean
        If Not extension.StartsWith(".") Then
            extension = "." & extension
        End If

        Dim key1 As Microsoft.Win32.RegistryKey = Nothing
        Dim key2 As Microsoft.Win32.RegistryKey = Nothing
        Dim key3 As Microsoft.Win32.RegistryKey = Nothing

        Try
            key1 = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(extension)
            key1.SetValue("", className)

            key2 = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(className)
            key2.SetValue("", description)

            key3 = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(className & "\shell\open\command")
            key3.SetValue("", exeProgram & " ""%1""")
        Catch ex As Exception
            Return False
        Finally
            If key1 IsNot Nothing Then key1.Close()
            If key2 IsNot Nothing Then key2.Close()
            If key3 IsNot Nothing Then key3.Close()
        End Try

        NotifyAssocChanges()

        Return True
    End Function

    Public Function RemoveAssoc(ByVal extension As String, ByVal className As String) As Boolean
        If Not extension.StartsWith(".") Then
            extension = "." & extension
        End If

        Try
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(extension)
            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(className)
        Catch ex As Exception
            Return False
        End Try

        NotifyAssocChanges()

        Return True
    End Function

    Public Function AssocExists(ByVal extension As String, ByVal className As String) As Boolean
        If Not extension.StartsWith(".") Then
            extension = "." & extension
        End If

        Try
            If Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension) Is Nothing Then Return False
            If Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(className) Is Nothing Then Return False
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Sub NotifyAssocChanges()
        SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, 0, 0)
    End Sub
End Module
