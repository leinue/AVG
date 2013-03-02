Structure OpSingle
    Dim Op As String
    Dim TarType As String
    Dim Parameter() As Parameter
End Structure

Structure OpBlock
    Dim Op As String
    Dim TarType As String
    Dim Parameter() As Parameter
    Dim Operations() As OpPtr
End Structure

Structure OpList
    Dim OpBlock() As OpBlock
    Dim OpSingle() As OpSingle
    Dim OpBlockLength As Integer
    Dim OpSingleLength As Integer

    Public Function AddOpSingle(ByVal mSingle As OpSingle) As OpPtr
        If OpSingleLength = UBound(OpSingle) + 1 Then
            ReDim Preserve OpSingle(2 * OpSingle.Length - 1)
        End If

        OpSingle(OpSingleLength) = mSingle

        Dim Ptr As OpPtr
        Ptr.Type = "Single"
        Ptr.Offset = OpSingleLength

        OpSingleLength = OpSingleLength + 1

        Return Ptr
    End Function

    Public Function AddOpBlock(ByVal OpBlock As OpBlock) As OpPtr

    End Function
End Structure

Structure OpPtr
    Dim Type As String
    Dim Offset As Integer
End Structure

Structure Parameter
    Dim DefParaN() As String
    Dim DefParaT() As String

    Dim CallPara() As ParaOnCall
End Structure

Structure ParaOnCall
    Dim Type As String
    Dim Name As String
    Dim Expr As OASExpr
End Structure


Public Class OASOperation

    Public Sub DoOpBlock()

    End Sub

End Class
