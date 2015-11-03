' Provides a way to determine if a non-default-module exists
' Author: Kevin Ross

Namespace ACRemote
    Public MustInherit Class Defaultable
        ' check if a non-default-module exists
        Public Shared Function non_default_exists(type As Type, default_type As Type) As Boolean
            ' get all types that aren't the default_type
            Dim types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(Function(s) s.GetTypes()).Where(Function(p) type.IsAssignableFrom(p)).Where(Function(p) p <> type).Where(Function(p) Not p.IsAbstract).Where(Function(p) p <> default_type)
            Dim has_types As Boolean = types.Count() > 0
            ' found one, make sure it's enabled for use
            If has_types Then
                Dim t As Type = types.First()
                Dim done As Boolean = False
                For Each cls In KernelInstance.FindImplementations()
                    If done Then
                        Exit For
                    End If
                    Dim load As ILoadable = DirectCast(Activator.CreateInstance(cls), ILoadable)
                    If load.ImplementingClass() = t AndAlso Not load.Enable() Then
                        has_types = False
                        done = True
                    End If
                Next
            End If
            Return has_types
        End Function
        ' generics-based version of non_default_exists
        Public Shared Function non_default_exists(Of T)(default_type As Type) As Boolean
            Return non_default_exists(GetType(T), default_type)
        End Function
    End Class
End Namespace

