Imports System.IO

Public Class OAEUSReader

    Sub LoadFromFile(ByVal Path As String)
        Dim Stream As StreamReader = New StreamReader(Path)

        LoadFromStreamReader(Stream)

        Stream.Dispose()
    End Sub

    Sub LoadFromString(ByVal mStr As String)
        Dim MStream As MemoryStream = New MemoryStream
        Dim WStream As StreamWriter = New StreamWriter(MStream)

        WStream.Write(mStr)

        Dim RStream As StreamReader = New StreamReader(MStream)

        LoadFromStreamReader(RStream)

        MStream.Dispose()
        WStream.Dispose()
        RStream.Dispose()
    End Sub

    Sub LoadFromStreamReader(ByRef Stream As StreamReader) ' The Code below is directly copied from OAECSReader.Do changes before run.

        'If CheckScriptVer(Stream) <> True Then
        '    Throw New Exception("Wrong script type or out-of-date script version.")
        'End If

        'Dim oChar As Char
        'Dim Status As Char = ""
        'Dim Buffer As String = ""
        'Dim OperateType As String = ""

        'While oChar = Chr(Stream.Read())
        '    If Status = "" And (oChar = Chr(13) Or oChar = Chr(10)) Then
        '        Continue While

        '    ElseIf oChar = "<" Then
        '        Status = "<"

        '    ElseIf Status = "<" Then
        '        Buffer = Buffer + oChar

        '    ElseIf oChar = ">" Then
        '        OperateType = Buffer
        '        Buffer = ""
        '        Status = ">"

        '    ElseIf Status = ">" And (oChar <> Chr(13) Or oChar <> Chr(10)) Then
        '        Buffer = Buffer + oChar

        '    ElseIf Status = ">" And (oChar = Chr(13) Or oChar = Chr(10)) Then
        '        DoOperation(OperateType, Buffer, "")
        '        Buffer = ""
        '        Status = ""

        '    ElseIf oChar = "{" Then
        '        Status = "{"

        '    ElseIf oChar = "{" Then
        '        Buffer = Buffer + oChar

        '    ElseIf oChar = "}" Then
        '        DoOperation(OperateType, Buffer, Buffer.Trim())
        '    End If
        'End While
    End Sub

    Function CheckScriptVer(ByRef Stream As StreamReader) As Boolean
        Dim Version As String = Stream.ReadLine().Trim()

        If Version <> "#*OAE Ctrl Script V2.0" Then
            Return False
        Else
            Return True
        End If
    End Function


End Class
