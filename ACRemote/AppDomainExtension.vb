' Provides methods to discover assemblies for loading
' Author: Kevin Ross

Imports System.Reflection
Imports System.IO

Public Class AppDomainMethods
    ' Get the path to the currently-executing program's directory (not working directory)
    Public Shared ReadOnly Property AssemblyDirectory() As String
        Get
            Dim codeBase As String = Assembly.GetExecutingAssembly().CodeBase
            Dim U As New UriBuilder(codeBase)
            Dim pth As String = Uri.UnescapeDataString(U.Path)
            Return Path.GetDirectoryName(pth)
        End Get
    End Property
    ' Return an array of Assemblys in AssemblyDirectory()
    Public Shared Function GetLocalAssemblies() As Assembly()
        Return GetAssemblyList().ToArray()
    End Function
    ' Return a list of Assemblys in AssemblyDirectory()
    Private Shared Function GetAssemblyList() As IEnumerable(Of Assembly)
        Dim assemblies As New List(Of Assembly)()
        For Each file As String In Directory.GetFiles(AssemblyDirectory)
            If Path.GetExtension(file) = ".dll" Then
                Try
                    Dim asm As Assembly = Assembly.LoadFile(file)
                    assemblies.Add(asm)
                Catch
                End Try
            End If
        Next
        assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().AsEnumerable())
        ' make sure duplicates are stripped out
        Return New List(Of Assembly)(New HashSet(Of Assembly)(assemblies))
    End Function
    ' Get a type from the current set of loaded assemblies
    Public Overloads Shared Function [GetType](typeName As String) As Type
        Dim ty = Type.[GetType](typeName)
        If ty <> Nothing Then
            Return ty
        End If
        For Each a In AppDomain.CurrentDomain.GetAssemblies()
            ty = a.[GetType](typeName)
            If ty <> Nothing Then
                Return ty
            End If
        Next
        Return Nothing
    End Function
End Class