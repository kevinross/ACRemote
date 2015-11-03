' Kernel that loads and provides instances of classes
' Author: Kevin Ross

Imports TinyIoC

Namespace ACRemote
    Public Class KernelInstance
        Private Shared kern As TinyIoCContainer
        Private Sub New()
        End Sub
        Public Shared Function kernel() As TinyIoCContainer
            Return kern
        End Function
        ' resolve an interface into a class
        Public Shared Function Resolve(Of T)() As T
            If kern Is Nothing Then
                CreateKernel()
            End If

            Return DirectCast(kern.Resolve(GetType(T)), T)
        End Function
        ' find all implementations of a given interface
        Public Shared Function FindImplementations() As Type()
            Dim impls As List(Of Type) = (From t In AppDomainMethods.GetLocalAssemblies() From x In t.GetTypes() Where GetType(ILoadable).IsAssignableFrom(x) AndAlso Not x.IsAbstract Select x).ToList()
            Return New List(Of Type)(New HashSet(Of Type)(New List(Of Type)(impls))).ToArray()
        End Function
        ' find and register all implementations
        Public Shared Sub RegisterImplementations()
            For Each cls In FindImplementations()
                Dim load As ILoadable = DirectCast(Activator.CreateInstance(cls), ILoadable)
                If load.Enable() Then
                    Console.WriteLine(String.Format("Loading {0}...", cls.FullName))
                    load.Load(kern)
                End If
            Next
        End Sub
        ' create the kernel and register the implementations
        Private Shared Sub CreateKernel()
            kern = TinyIoC.TinyIoCContainer.Current
            RegisterImplementations()
        End Sub
    End Class
End Namespace

