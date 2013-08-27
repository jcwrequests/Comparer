Imports System.Dynamic

Public Class ExpandoObjectComparer

    Implements IEqualityComparer(Of Object)

    Private Sub New()

    End Sub
    Public Shared Function [Default]() As ExpandoObjectComparer
        Return New ExpandoObjectComparer
    End Function


    Public Shadows Function Equals(x As Object, y As Object) As Boolean Implements IEqualityComparer(Of Object).Equals

        If Object.ReferenceEquals(x, y) Then Return True


        If x.GetType().Equals(y.GetType) AndAlso x.GetType.Equals(GetType(ExpandoObject)) Then
            Dim xKeyValues As New Dictionary(Of String, Object)(CType(x, IDictionary(Of String, Object)))
            Dim yKeyValues As New Dictionary(Of String, Object)(CType(y, IDictionary(Of String, Object)))

            Dim xFieldsCount = xKeyValues.Count()
            Dim yFieldsCount = yKeyValues.Count()

            If Not xFieldsCount.Equals(yFieldsCount) Then Return False
            If Not xKeyValues.Keys.Where(Function(k) Not yKeyValues.ContainsKey(k)).FirstOrDefault Is Nothing Then Return False

            For Each kvPair In xKeyValues
                Dim key = kvPair.Key
                Dim xValueItem = kvPair.Value
                Dim yValueItem = yKeyValues.Item(key)

                If xValueItem Is Nothing And Not yValueItem Is Nothing Then Return False
                If Not xValueItem Is Nothing And yValueItem Is Nothing Then Return False
                If Not xValueItem Is Nothing And Not yValueItem Is Nothing Then
                    If Not xValueItem.Equals(yValueItem) Then Return False
                End If
            Next

            Return True
        Else
            Return False
        End If

    End Function


    Public Shadows Function GetHashCode(obj As Object) As Integer Implements IEqualityComparer(Of Object).GetHashCode

        Dim _hashCode As Integer = 0

        Dim getHash = Function(item As Object) As Integer
                          If item Is Nothing Then Return 0
                          Return item.GetHashCode
                      End Function

        If obj.GetType().Equals(GetType(ExpandoObject)) Then
            Dim values As New Dictionary(Of String, Object)(CType(obj, IDictionary(Of String, Object)))
            values.Values.ToList.ForEach(Sub(v) _hashCode = _hashCode Xor getHash(v))
            Return _hashCode
        Else
            Return obj.GetHashCode
        End If
    End Function

End Class
