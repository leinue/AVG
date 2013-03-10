Imports System.IO

Public Class OAS_Operation

    Const EngineVersion As String = "2.0"

    Dim DataManager As OASData

    Public Sub New(ByRef Manager As OASData)
        DataManager = Manager
    End Sub

    Public Sub LoadScript(ByVal Path As String)
        Dim Reader As StreamReader = New StreamReader(Path)

        Dim Version As String = Reader.ReadLine

        DebugOutPut("OAS_Operation LoadScript:(INFO) Start loading '" + Path + "'")

        If CheckVersion(Version) Then
            DebugOutPut("OAS_Operation LoadScript:(UE) Out-of-date script.Back. ")
            Return
        End If

        DebugOutPut("OAS_Operation LoadScript:(INFO) Finish loading '" + Path + "'")

    End Sub

    Public Function PackOpBlock(ByRef Reader As StreamReader) As OpTable.OpBlock
        Dim oChar As Char
        Dim Status As String = "WaitForOpName"
        Dim Buffer As String = ""
        Dim OpName As String = ""
        Dim TarType As String = ""
        Dim TarName As String = ""
        Dim TextParameter As String = ""
        Dim Parameter As OpTable.Parameter

        While oChar = Chr(Reader.Read())
            If oChar = "[" Then
                If Status = "WaitForOpName" Then
                    Status = "ReadInTarType"
                    OpName = "Def"
                ElseIf Status = "ReadInOpName" Then
                    Status = "ReadInTarType"
                Else
                    DebugOutPut("OAS_Operation PackOpBlock:(UE) Syntax error. Unexpected '['.")
                    Return Nothing
                End If

            ElseIf oChar = "]" Then
                If Status = "ReadInTarType" Then
                    Status = "WaitForName,Para,Block"
                Else
                    DebugOutPut("OAS_Operation PackOpBlock:(UE) Syntax error. Unexpected ']'.")
                    Return Nothing
                End If

            ElseIf oChar = ":" Then
                If Status = "WaitForName,Para,Block" Then
                    Status = "ReadInPara"
                Else
                    DebugOutPut("OAS_Operation PackOpBlock:(UE) Syntax error. Unexpected ':'.")
                    Return Nothing
                End If

            ElseIf oChar = vbCr Or oChar = vbLf Then
                If Status = "WaitForName,Para,Block" Or Status = "ReadInPara" Then
                    Status = "WaitForOpName"
                    Dim mSingle As OpTable.OpSingle = New OpTable.OpSingle
                    mSingle.TarName = TarName
                    mSingle.TarType = TarType
                    mSingle.Op = OpName
                    mSingle.Parameter = New OpTable.Parameter(TextParameter)

                    DataManager.Entity.OpreationTable.AddOpSingle(mSingle)
                End If

            Else
                If Status = "WaitForOpName" Then
                    Status = "ReadInOpName"
                    OpName = OpName + oChar

                ElseIf Status = "ReadInOpName" Then
                    OpName = OpName + oChar

                ElseIf Status = "ReadInTarType" Then
                    TarType = TarType + oChar

                ElseIf Status = "WaitForName,Para,Block" Then
                    Status = "ReadInTarName"
                    TarName = TarName + oChar

                ElseIf Status = "ReadInTarName" Then
                    TarName = TarName + oChar

                ElseIf Status = "ReadInPara" Then
                    TextParameter = TextParameter + oChar
                End If
            End If
        End While

    End Function

    Public Sub DoOpBlock()

    End Sub

    Private Function CheckVersion(ByVal Version As String) As Boolean
        Throw New NotImplementedException
    End Function

    Private Sub DebugOutPut(ByVal p1 As String)
        Throw New NotImplementedException
    End Sub

End Class
