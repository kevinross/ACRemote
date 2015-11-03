' Basic CLI set/get client using JSON-RPC
' Author: Kevin Ross

Imports API.ACRemote.API

Namespace CliClient
    Class MainClass
        Public Shared Sub Main(args As String())
            If args.Length < 3 Then
                'return;
                Console.WriteLine("Usage: CliClient.exe host prop_name get|set [value] ")
            End If
            Dim endpoint As String = args(0)
            Dim method As String = args(1)
            Dim action As String = args(2)
            Dim param As String = Nothing
            If args.Length > 3 Then
                param = args(3)
            End If
            Dim client As New RemoteServiceClient(endpoint)
            If action = "get" Then
                Console.WriteLine(PropertyUtil.get_prop(client, method))
            Else
                PropertyUtil.set_prop(client, method, param)
                Console.WriteLine(PropertyUtil.get_prop(client, method))
            End If
        End Sub
    End Class
End Namespace
