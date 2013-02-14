'OAE Script Reader
'V2.0
'
'By TerryGeng(达) & leinue(xy)

Imports System.IO

Public Class OAECSDescriber

    '---------Structures---------
    Structure OAEAction
        Dim Name As String
        Dim Code As String
    End Structure

    Structure OAEVar
        Dim Name As String
        Dim Value As String
    End Structure

    '---------Vars---------
    Public ResReader As OAEUSDescriber

    Dim CVars(1) As OAEVar
    Dim CVarNum As Integer

    Dim ActionList(1) As OAEAction
    Dim ActionNum As Integer

    '---------Construct Functions---------
    Sub New(ByVal mResReader As OAEUSDescriber)
        ResReader = mResReader
    End Sub

    Sub New(ByVal Path As String, ByVal ResReader As OAEUSDescriber)
        LoadFromFile(Path)
    End Sub

    '---------Function To Load---------
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

    Sub LoadFromStreamReader(ByRef Stream As StreamReader)

        If CheckScriptVer(Stream) <> True Then
            Throw New Exception("Wrong script type or out-of-date script version.")
        End If

        Dim oChar As Char
        Dim Status As Char = ""
        Dim Buffer As String = ""
        Dim OperateType As String = ""

        While oChar = Chr(Stream.Read())
            If Status = "" And (oChar = Chr(13) Or oChar = Chr(10)) Then
                Continue While

            ElseIf oChar = "<" Then
                Status = "<"

            ElseIf Status = "<" Then
                Buffer = Buffer + oChar

            ElseIf oChar = ">" Then
                OperateType = Buffer
                Buffer = ""
                Status = ">"

            ElseIf Status = ">" And (oChar <> Chr(13) Or oChar <> Chr(10)) Then
                Buffer = Buffer + oChar

            ElseIf Status = ">" And (oChar = Chr(13) Or oChar = Chr(10)) Then
                DoOperation(OperateType, Buffer)
                Buffer = ""
                Status = ""

            ElseIf oChar = "{" Then
                Status = "{"

            ElseIf oChar = "{" Then
                Buffer = Buffer + oChar

            ElseIf oChar = "}" Then
                DoOperation(OperateType, Buffer, Buffer.Trim())
            End If
        End While
    End Sub

    '---------Other Functions---------
    Function CheckScriptVer(ByRef Stream As StreamReader) As Boolean
        Dim Version As String = Stream.ReadLine().Trim()

        If Version <> "#*OAE Ctrl Script V2.0" Then
            Return False
        Else
            Return True
        End If
    End Function

    '----------------------------------------
    ' 
    '         Describe Function Part
    ' 
    '----------------------------------------

    Sub DoOperation(ByVal Type As String, ByVal Op1 As String, Optional ByVal Op2 As String = "")
        If Type = "Include" Then
            LoadFromFile(Op1)
        End If

        If Type = "LoadRes" Then
            ResReader.LoadFromFile(Op1)
        End If

        If Type = "Action" Then
            AddAction(Op1, Op2)
        End If

        If Type = "Var" Then
            Dim Var() As String = Op1.Split(":")
            SetVar(Var(0), Var(1))
        End If

    End Sub

    Sub AddAction(ByVal Name As String, ByVal Code As String)

        If ActionList.Length = ActionNum Then
            ReDim Preserve ActionList(ActionNum * 2 - 1)
        End If

        ActionList(ActionNum).Name = Name
        ActionList(ActionNum).Code = Code

        ActionNum = ActionNum + 1

    End Sub

    Sub SetVar(ByVal Name As String, ByVal Value As String)
        If CVars.Length = CVarNum Then
            ReDim Preserve CVars(CVarNum * 2 - 1)
        End If

        CVars(CVarNum).Name = Name
        CVars(CVarNum).Value = Value
    End Sub



End Class
