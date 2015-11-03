' Implementation of a JSON-RPC client
' Author: Kevin Ross

Imports System.Net
Imports System.Text
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace ACRemote.API
    Public Class JsonRpcClient
        Private endpoint As Uri = Nothing
        Private id As Integer = 1
        Public Sub New(endpoint As Uri)
            Me.endpoint = endpoint
        End Sub
        ' Invoke a method and return an "Object"
        Public Function InvokeMethod(method As String, ParamArray p As Object()) As Object
            Return Me.InvokeMethod_(method, p).SelectToken("result").ToObject(Of Object)()
        End Function
        ' Invoke a method and return a cast to T
        Public Function InvokeMethod(Of T)(method As String, ParamArray p As Object()) As T
            Return DirectCast(Me.InvokeMethod_(method, p).SelectToken("result").ToObject(Of T)(), T)
        End Function
        ' code taken from http://bitcoin.stackexchange.com/a/5811
        ' ported to VB by Kevin
        ' no license provided
        Public Function InvokeMethod_(a_sMethod As String, ParamArray a_params As Object()) As JObject
            Dim webreq As HttpWebRequest = DirectCast(WebRequest.Create(endpoint), HttpWebRequest)

            webreq.ContentType = "application/json-rpc"
            webreq.Method = "POST"

            Dim joe As New JObject()
            joe("jsonrpc") = "1.0"
            joe("id") = (System.Math.Max(System.Threading.Interlocked.Increment(id), id - 1)).ToString()
            joe("method") = a_sMethod

            If a_params Is Nothing Then
                If a_params.Length > 0 Then
                    Dim props As New JArray()
                    For Each p In a_params
                        props.Add(p)
                    Next
                    joe.Add(New JProperty("params", props))
                Else
                    joe.Add(New JProperty("params", New JArray()))
                End If
            End If

            Dim s As String = JsonConvert.SerializeObject(joe)
            ' serialize json for the request
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(s)
            webreq.ContentLength = byteArray.Length

            Try
                Using dataStream As Stream = webreq.GetRequestStream()
                    dataStream.Write(byteArray, 0, byteArray.Length)
                End Using
            Catch generatedExceptionName As WebException
                'inner exception is socket
                '{"A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 23.23.246.5:8332"}
                Throw
            End Try
            Try
                Using webResponse = webreq.GetResponse()
                    Using str As Stream = webResponse.GetResponseStream()
                        Using sr As New StreamReader(str)
                            Return JsonConvert.DeserializeObject(Of JObject)(sr.ReadToEnd())
                        End Using
                    End Using
                End Using
            Catch webex As WebException
                Using str As Stream = webex.Response.GetResponseStream()
                    Using sr As New StreamReader(str)
                        Dim tempRet = JsonConvert.DeserializeObject(Of JObject)(sr.ReadToEnd())
                        Return tempRet
                    End Using
                End Using
            Catch generatedExceptionName As Exception
                Throw
            End Try
        End Function
    End Class
End Namespace

