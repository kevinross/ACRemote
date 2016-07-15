' Convenience class to get and set properties on an object by strings
' Author: Kevin Ross

Imports System.Reflection

Public Class PropertyUtil
    ' get a property by string
    Public Shared Function get_prop(client As Object, prop As String) As Object
        Dim thisType As Type = client.GetType()
        Dim theProp As PropertyInfo = thisType.GetProperty(prop)
        Return theProp.GetValue(client)
    End Function
    ' convert a string into the appropriate type for the given parameter
    Private Shared Function convertType(leftType As Type, rightObject As String) As Object
        If leftType = GetType(String) Then
            Return rightObject
        ElseIf leftType = GetType(Boolean) Then
            Select Case rightObject.ToLower()
                Case "on", "1"
                    rightObject = "true"
                Case "off", "0"
                    rightObject = "false"
            End Select
            Return Boolean.Parse(rightObject)
        ElseIf leftType = GetType(Integer) Then
            Return Integer.Parse(rightObject)
        ElseIf leftType = GetType(Double) Then
            Return Double.Parse(rightObject)
        ElseIf leftType = GetType(modes) Then
            Return convert_str_to_enum(Of modes)(rightObject)
        ElseIf leftType = GetType(speeds) Then
            Return convert_str_to_enum(Of speeds)(rightObject)
        Else
            Return Nothing
        End If

    End Function
    ' call a function by string
    Public Shared Sub call_func(client As Object, method As String, param As String)
        Dim thisType As Type = client.GetType()
        Dim theMethod As MethodInfo = thisType.GetMethod(method)
        If theMethod Is Nothing Then
            Return
        End If
        Dim params(0) As Object
        params.Initialize()
        params(0) = Convert.ChangeType(convertType(theMethod.GetParameters().First().GetType(), param), theMethod.GetParameters().First().GetType())
        theMethod.Invoke(client, params)
    End Sub
    ' set a property by string
    Public Shared Sub set_prop(client As Object, prop As String, param As String)
        Dim thisType As Type = client.[GetType]()
        Dim theProp As PropertyInfo = thisType.GetProperty(prop)
        If theProp Is Nothing Then
            Return
        End If
        theProp.SetValue(client, Convert.ChangeType(convertType(theProp.PropertyType, param), theProp.PropertyType))
    End Sub
    Public Shared Function convert_str_to_enum(Of T)(val As String) As T
        Return DirectCast(System.[Enum].Parse(GetType(T), val), T)
    End Function
End Class