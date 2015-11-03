' Convenience class to get and set properties on an object by strings
' Author: Kevin Ross

Imports System.Reflection
Imports ACRemote.ACRemote

Namespace ACRemote.API
    Public Class PropertyUtil
        Public Shared Function get_prop(client As Object, prop As String) As Object
            Dim thisType As Type = client.[GetType]()
            Dim theProp As PropertyInfo = thisType.GetProperty(prop)
            Return theProp.GetValue(client)
        End Function
        Public Shared Sub set_prop(client As Object, prop As String, param As String)
            Dim thisType As Type = client.[GetType]()
            Dim theProp As PropertyInfo = thisType.GetProperty(prop)
            If theProp.PropertyType = GetType(String) Then
                theProp.SetValue(client, param)
            ElseIf theProp.PropertyType = GetType(Boolean) Then
                If param.ToLower() = "on" Then
                    param = "true"
                ElseIf param.ToLower() = "off" Then
                    param = "false"
                End If
                theProp.SetValue(client, [Boolean].Parse(param))
            ElseIf theProp.PropertyType = GetType(Integer) Then
                theProp.SetValue(client, Integer.Parse(param))
            ElseIf theProp.PropertyType = GetType(modes) Then
                theProp.SetValue(client, convert_str_to_enum(Of modes)(param))
            ElseIf theProp.PropertyType = GetType(speeds) Then
                theProp.SetValue(client, convert_str_to_enum(Of speeds)(param))
            Else
                Console.WriteLine("type is " + theProp.PropertyType.ToString())
            End If
        End Sub
    Public Shared Function convert_str_to_enum(Of T)(val As String) As T
      Return DirectCast(System.[Enum].Parse(GetType(T), val), T)
    End Function
  End Class
End Namespace

