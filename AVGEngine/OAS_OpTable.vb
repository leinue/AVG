Public Class OpTable
    Structure OpSingle
        Dim Op As String
        Dim TarType As String
        Dim TarName As String
        Dim Parameter As Parameter
    End Structure

    Structure OpBlock
        Dim Op As String
        Dim TarType As String
        Dim TarName As String
        Dim Parameter As Parameter
        Dim OpPtr() As DataEntityPtr
    End Structure

    Structure Parameter
        Dim Type As String
        Dim Name As String
        Dim Entity As DataEntityPtr

        Sub New(ByVal TextParameter As String)
            ' TODO: Complete member initialization 
        End Sub

    End Structure

    Dim mSingle() As OpSingle
    Dim mBlock() As OpBlock

    Dim mSingleLength As Integer
    Dim mBlockLength As Integer


    Public Function AddOpSingle(ByVal nSingle As OpSingle) As DataEntityPtr
        If mSingleLength = UBound(mSingle) + 1 Then
            ReDim Preserve mSingle(2 * mSingle.Length - 1)
        End If

        mSingle(mSingleLength) = nSingle

        Dim Ptr As DataEntityPtr
        Ptr.DataType = "Operation"
        Ptr.SecondType = "Single"
        Ptr.DataEntityIndex = mSingleLength

        mSingleLength = mSingleLength + 1

        Return Ptr
    End Function
End Class
